// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.JoinSequence
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Engine
{
  public class JoinSequence
  {
    private readonly ISessionFactoryImplementor factory;
    private readonly List<JoinSequence.Join> joins = new List<JoinSequence.Join>();
    private bool useThetaStyle;
    private readonly SqlStringBuilder conditions = new SqlStringBuilder();
    private string rootAlias;
    private IJoinable rootJoinable;
    private JoinSequence.ISelector selector;
    private JoinSequence next;
    private bool isFromPart;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("JoinSequence{");
      if (this.rootJoinable != null)
        stringBuilder.Append((object) this.rootJoinable).Append('[').Append(this.rootAlias).Append(']');
      for (int index = 0; index < this.joins.Count; ++index)
        stringBuilder.Append("->").Append((object) this.joins[index]);
      return stringBuilder.Append('}').ToString();
    }

    public JoinSequence(ISessionFactoryImplementor factory) => this.factory = factory;

    public JoinSequence GetFromPart()
    {
      JoinSequence fromPart = new JoinSequence(this.factory);
      fromPart.joins.AddRange((IEnumerable<JoinSequence.Join>) this.joins);
      fromPart.useThetaStyle = this.useThetaStyle;
      fromPart.rootAlias = this.rootAlias;
      fromPart.rootJoinable = this.rootJoinable;
      fromPart.selector = this.selector;
      fromPart.next = this.next == null ? (JoinSequence) null : this.next.GetFromPart();
      fromPart.isFromPart = true;
      return fromPart;
    }

    public JoinSequence Copy()
    {
      JoinSequence joinSequence = new JoinSequence(this.factory);
      joinSequence.joins.AddRange((IEnumerable<JoinSequence.Join>) this.joins);
      joinSequence.useThetaStyle = this.useThetaStyle;
      joinSequence.rootAlias = this.rootAlias;
      joinSequence.rootJoinable = this.rootJoinable;
      joinSequence.selector = this.selector;
      joinSequence.next = this.next == null ? (JoinSequence) null : this.next.Copy();
      joinSequence.isFromPart = this.isFromPart;
      joinSequence.conditions.Add(this.conditions.ToSqlString());
      return joinSequence;
    }

    public JoinSequence AddJoin(
      IAssociationType associationType,
      string alias,
      JoinType joinType,
      string[] referencingKey)
    {
      this.joins.Add(new JoinSequence.Join(this.factory, associationType, alias, joinType, referencingKey));
      return this;
    }

    public JoinFragment ToJoinFragment()
    {
      return this.ToJoinFragment((IDictionary<string, IFilter>) new CollectionHelper.EmptyMapClass<string, IFilter>(), true);
    }

    public JoinFragment ToJoinFragment(
      IDictionary<string, IFilter> enabledFilters,
      bool includeExtraJoins)
    {
      return this.ToJoinFragment(enabledFilters, includeExtraJoins, (SqlString) null, (string) null);
    }

    public JoinFragment ToJoinFragment(
      IDictionary<string, IFilter> enabledFilters,
      bool includeExtraJoins,
      SqlString withClauseFragment,
      string withClauseJoinAlias)
    {
      QueryJoinFragment joinFragment = new QueryJoinFragment(this.factory.Dialect, this.useThetaStyle);
      if (this.rootJoinable != null)
      {
        joinFragment.AddCrossJoin(this.rootJoinable.TableName, this.rootAlias);
        string condition = this.rootJoinable.FilterFragment(this.rootAlias, enabledFilters);
        joinFragment.HasFilterCondition = joinFragment.AddCondition(condition);
        if (includeExtraJoins)
          this.AddExtraJoins((JoinFragment) joinFragment, this.rootAlias, this.rootJoinable, true);
      }
      IJoinable joinable = this.rootJoinable;
      for (int index = 0; index < this.joins.Count; ++index)
      {
        JoinSequence.Join join = this.joins[index];
        string onCondition = join.AssociationType.GetOnCondition(join.Alias, this.factory, enabledFilters);
        SqlString sqlString = new SqlString(new object[0]);
        SqlString on;
        if (joinable != null && this.IsManyToManyRoot(joinable) && ((ICollectionPersister) joinable).ElementType == join.AssociationType)
        {
          string manyFilterFragment = ((ICollectionPersister) joinable).GetManyToManyFilterFragment(join.Alias, enabledFilters);
          on = new SqlString("".Equals(manyFilterFragment) ? onCondition : ("".Equals(onCondition) ? manyFilterFragment : onCondition + " and " + manyFilterFragment));
        }
        else
        {
          IDictionary<string, IFilter> enabledForManyToOne = FilterHelper.GetEnabledForManyToOne(enabledFilters);
          on = new SqlString(!string.IsNullOrEmpty(onCondition) || enabledForManyToOne.Count <= 0 ? onCondition : join.Joinable.FilterFragment(join.Alias, enabledForManyToOne));
        }
        if (withClauseFragment != null && join.Alias.Equals(withClauseJoinAlias))
          on = on.Append(" and ").Append(withClauseFragment);
        joinFragment.AddJoin(join.Joinable.TableName, join.Alias, join.LHSColumns, JoinHelper.GetRHSColumnNames(join.AssociationType, this.factory), join.JoinType, on);
        if (includeExtraJoins)
          this.AddExtraJoins((JoinFragment) joinFragment, join.Alias, join.Joinable, join.JoinType == JoinType.InnerJoin);
        joinable = join.Joinable;
      }
      if (this.next != null)
        joinFragment.AddFragment(this.next.ToJoinFragment(enabledFilters, includeExtraJoins));
      joinFragment.AddCondition(this.conditions.ToSqlString());
      if (this.isFromPart)
        joinFragment.ClearWherePart();
      return (JoinFragment) joinFragment;
    }

    private bool IsManyToManyRoot(IJoinable joinable)
    {
      return joinable != null && joinable.IsCollection && ((ICollectionPersister) joinable).IsManyToMany;
    }

    private bool IsIncluded(string alias)
    {
      return this.selector != null && this.selector.IncludeSubclasses(alias);
    }

    private void AddExtraJoins(
      JoinFragment joinFragment,
      string alias,
      IJoinable joinable,
      bool innerJoin)
    {
      bool includeSubclasses = this.IsIncluded(alias);
      joinFragment.AddJoins(joinable.FromJoinFragment(alias, innerJoin, includeSubclasses), joinable.WhereJoinFragment(alias, innerJoin, includeSubclasses));
    }

    public JoinSequence AddCondition(SqlString condition)
    {
      if (condition.Trim().Length != 0)
      {
        if (!condition.StartsWithCaseInsensitive(" and "))
          this.conditions.Add(" and ");
        this.conditions.Add(condition);
      }
      return this;
    }

    public JoinSequence AddCondition(
      string alias,
      string[] columns,
      string condition,
      bool appendParameter)
    {
      for (int index = 0; index < columns.Length; ++index)
      {
        this.conditions.Add(" and ").Add(alias).Add(".").Add(columns[index]).Add(condition);
        if (appendParameter)
          this.conditions.AddParameter();
      }
      return this;
    }

    public JoinSequence SetRoot(IJoinable joinable, string alias)
    {
      this.rootAlias = alias;
      this.rootJoinable = joinable;
      return this;
    }

    public JoinSequence SetNext(JoinSequence next)
    {
      this.next = next;
      return this;
    }

    public JoinSequence SetSelector(JoinSequence.ISelector s)
    {
      this.selector = s;
      return this;
    }

    public JoinSequence SetUseThetaStyle(bool useThetaStyle)
    {
      this.useThetaStyle = useThetaStyle;
      return this;
    }

    public bool IsThetaStyle => this.useThetaStyle;

    public int JoinCount => this.joins.Count;

    private sealed class Join
    {
      private readonly IAssociationType associationType;
      private readonly IJoinable joinable;
      private readonly JoinType joinType;
      private readonly string alias;
      private readonly string[] lhsColumns;

      public Join(
        ISessionFactoryImplementor factory,
        IAssociationType associationType,
        string alias,
        JoinType joinType,
        string[] lhsColumns)
      {
        this.associationType = associationType;
        this.joinable = associationType.GetAssociatedJoinable(factory);
        this.alias = alias;
        this.joinType = joinType;
        this.lhsColumns = lhsColumns;
      }

      public string Alias => this.alias;

      public IAssociationType AssociationType => this.associationType;

      public IJoinable Joinable => this.joinable;

      public JoinType JoinType => this.joinType;

      public string[] LHSColumns => this.lhsColumns;

      public override string ToString()
      {
        return this.joinable.ToString() + (object) '[' + this.alias + (object) ']';
      }
    }

    public interface ISelector
    {
      bool IncludeSubclasses(string alias);
    }
  }
}
