// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.OuterJoinableAssociation
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader
{
  public sealed class OuterJoinableAssociation
  {
    private readonly IAssociationType joinableType;
    private readonly IJoinable joinable;
    private readonly string lhsAlias;
    private readonly string[] lhsColumns;
    private readonly string rhsAlias;
    private readonly string[] rhsColumns;
    private readonly JoinType joinType;
    private readonly SqlString on;
    private readonly IDictionary<string, IFilter> enabledFilters;

    public OuterJoinableAssociation(
      IAssociationType joinableType,
      string lhsAlias,
      string[] lhsColumns,
      string rhsAlias,
      JoinType joinType,
      SqlString withClause,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
    {
      this.joinableType = joinableType;
      this.lhsAlias = lhsAlias;
      this.lhsColumns = lhsColumns;
      this.rhsAlias = rhsAlias;
      this.joinType = joinType;
      this.joinable = joinableType.GetAssociatedJoinable(factory);
      this.rhsColumns = JoinHelper.GetRHSColumnNames(joinableType, factory);
      this.on = new SqlString(joinableType.GetOnCondition(rhsAlias, factory, enabledFilters));
      if (StringHelper.IsNotEmpty(withClause))
        this.on = this.on.Append(" and ( ").Append(withClause).Append(" )");
      this.enabledFilters = enabledFilters;
    }

    public JoinType JoinType => this.joinType;

    public string RHSAlias => this.rhsAlias;

    public SqlString On => this.on;

    private bool IsOneToOne
    {
      get => this.joinableType.IsEntityType && ((EntityType) this.joinableType).IsOneToOne;
    }

    public IAssociationType JoinableType => this.joinableType;

    public string RHSUniqueKeyName => this.joinableType.RHSUniqueKeyPropertyName;

    public bool IsCollection => this.joinableType.IsCollectionType;

    public IJoinable Joinable => this.joinable;

    public int GetOwner(IList<OuterJoinableAssociation> associations)
    {
      return this.IsOneToOne || this.IsCollection ? OuterJoinableAssociation.GetPosition(this.lhsAlias, (IEnumerable<OuterJoinableAssociation>) associations) : -1;
    }

    private static int GetPosition(
      string lhsAlias,
      IEnumerable<OuterJoinableAssociation> associations)
    {
      int position = 0;
      foreach (OuterJoinableAssociation association in associations)
      {
        if (association.Joinable.ConsumesEntityAlias())
        {
          if (association.rhsAlias.Equals(lhsAlias))
            return position;
          ++position;
        }
      }
      return -1;
    }

    public void AddJoins(JoinFragment outerjoin)
    {
      outerjoin.AddJoin(this.joinable.TableName, this.rhsAlias, this.lhsColumns, this.rhsColumns, this.joinType, this.on);
      outerjoin.AddJoins(this.joinable.FromJoinFragment(this.rhsAlias, false, true), this.joinable.WhereJoinFragment(this.rhsAlias, false, true));
    }

    public void ValidateJoin(string path)
    {
      if (this.rhsColumns == null || this.lhsColumns == null || this.lhsColumns.Length != this.rhsColumns.Length || this.lhsColumns.Length == 0)
        throw new MappingException("invalid join columns for association: " + path);
    }

    public bool IsManyToManyWith(OuterJoinableAssociation other)
    {
      if (this.joinable.IsCollection)
      {
        IQueryableCollection joinable = (IQueryableCollection) this.joinable;
        if (joinable.IsManyToMany)
          return joinable.ElementType == other.JoinableType;
      }
      return false;
    }

    public void AddManyToManyJoin(JoinFragment outerjoin, IQueryableCollection collection)
    {
      string manyFilterFragment = collection.GetManyToManyFilterFragment(this.rhsAlias, this.enabledFilters);
      SqlString on = string.Empty.Equals(manyFilterFragment) ? this.on : (StringHelper.IsEmpty(this.on) ? new SqlString(manyFilterFragment) : this.on.Append(" and ").Append(manyFilterFragment));
      outerjoin.AddJoin(this.joinable.TableName, this.rhsAlias, this.lhsColumns, this.rhsColumns, this.joinType, on);
      outerjoin.AddJoins(this.joinable.FromJoinFragment(this.rhsAlias, false, true), this.joinable.WhereJoinFragment(this.rhsAlias, false, true));
    }
  }
}
