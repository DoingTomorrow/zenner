// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.AbstractEntityJoinWalker
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader
{
  public abstract class AbstractEntityJoinWalker : JoinWalker
  {
    private readonly IOuterJoinLoadable persister;
    private readonly string alias;

    public AbstractEntityJoinWalker(
      IOuterJoinLoadable persister,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
      : base(factory, enabledFilters)
    {
      this.persister = persister;
      this.alias = this.GenerateRootAlias(persister.EntityName);
    }

    public AbstractEntityJoinWalker(
      string rootSqlAlias,
      IOuterJoinLoadable persister,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
      : base(factory, enabledFilters)
    {
      this.persister = persister;
      this.alias = rootSqlAlias;
    }

    protected virtual void InitAll(
      SqlString whereString,
      SqlString orderByString,
      LockMode lockMode)
    {
      this.WalkEntityTree(this.persister, this.Alias);
      IList<OuterJoinableAssociation> associations = (IList<OuterJoinableAssociation>) new List<OuterJoinableAssociation>((IEnumerable<OuterJoinableAssociation>) this.associations);
      associations.Add(new OuterJoinableAssociation((IAssociationType) this.persister.EntityType, (string) null, (string[]) null, this.alias, JoinType.LeftOuterJoin, (SqlString) null, this.Factory, (IDictionary<string, IFilter>) new CollectionHelper.EmptyMapClass<string, IFilter>()));
      this.InitPersisters(associations, lockMode);
      this.InitStatementString(whereString, orderByString, lockMode);
    }

    protected void InitProjection(
      SqlString projectionString,
      SqlString whereString,
      SqlString orderByString,
      SqlString groupByString,
      SqlString havingString,
      IDictionary<string, IFilter> enabledFilters,
      LockMode lockMode)
    {
      this.WalkEntityTree(this.persister, this.Alias);
      this.Persisters = new ILoadable[0];
      this.InitStatementString(projectionString, whereString, orderByString, groupByString.ToString(), havingString, lockMode);
    }

    private void InitStatementString(SqlString condition, SqlString orderBy, LockMode lockMode)
    {
      this.InitStatementString((SqlString) null, condition, orderBy, string.Empty, (SqlString) null, lockMode);
    }

    private void InitStatementString(
      SqlString projection,
      SqlString condition,
      SqlString orderBy,
      string groupBy,
      SqlString having,
      LockMode lockMode)
    {
      int index = JoinWalker.CountEntityPersisters(this.associations);
      this.Suffixes = BasicLoader.GenerateSuffixes(index + 1);
      JoinFragment joinFragment = this.MergeOuterJoins(this.associations);
      SqlString selectClause = projection ?? new SqlString(this.persister.SelectFragment(this.alias, this.Suffixes[index]) + this.SelectString(this.associations));
      SqlSelectBuilder sqlSelectBuilder = new SqlSelectBuilder(this.Factory).SetLockMode(lockMode).SetSelectClause(selectClause).SetFromClause(this.Dialect.AppendLockHint(lockMode, this.persister.FromTableFragment(this.alias)) + (object) this.persister.FromJoinFragment(this.alias, true, true)).SetWhereClause(condition).SetOuterJoins(joinFragment.ToFromFragmentString, joinFragment.ToWhereFragmentString + this.WhereFragment).SetOrderByClause(this.OrderBy(this.associations, orderBy)).SetGroupByClause(groupBy).SetHavingClause(having);
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlSelectBuilder.SetComment(this.Comment);
      this.SqlString = sqlSelectBuilder.ToSqlString();
    }

    protected override bool IsJoinedFetchEnabled(
      IAssociationType type,
      FetchMode config,
      CascadeStyle cascadeStyle)
    {
      return this.IsJoinedFetchEnabledInMapping(config, type);
    }

    protected virtual SqlString WhereFragment
    {
      get => this.persister.WhereJoinFragment(this.alias, true, true);
    }

    public abstract string Comment { get; }

    protected IOuterJoinLoadable Persister => this.persister;

    protected string Alias => this.alias;

    public override string ToString()
    {
      return this.GetType().FullName + (object) '(' + this.Persister.EntityName + (object) ')';
    }
  }
}
