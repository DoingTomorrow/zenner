// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Collection.OneToManyJoinWalker
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
namespace NHibernate.Loader.Collection
{
  public class OneToManyJoinWalker : CollectionJoinWalker
  {
    private readonly IOuterJoinLoadable elementPersister;
    private readonly IQueryableCollection oneToManyPersister;

    protected override bool IsDuplicateAssociation(
      string foreignKeyTable,
      string[] foreignKeyColumns)
    {
      return this.oneToManyPersister.TableName.Equals(foreignKeyTable) && CollectionHelper.CollectionEquals<string>((ICollection<string>) foreignKeyColumns, (ICollection<string>) this.oneToManyPersister.KeyColumnNames) || base.IsDuplicateAssociation(foreignKeyTable, foreignKeyColumns);
    }

    public OneToManyJoinWalker(
      IQueryableCollection oneToManyPersister,
      int batchSize,
      SqlString subquery,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
      : base(factory, enabledFilters)
    {
      this.oneToManyPersister = oneToManyPersister;
      this.elementPersister = (IOuterJoinLoadable) oneToManyPersister.ElementPersister;
      string rootAlias = this.GenerateRootAlias(oneToManyPersister.Role);
      this.WalkEntityTree(this.elementPersister, rootAlias);
      IList<OuterJoinableAssociation> associations = (IList<OuterJoinableAssociation>) new List<OuterJoinableAssociation>((IEnumerable<OuterJoinableAssociation>) this.associations);
      associations.Add(new OuterJoinableAssociation((IAssociationType) oneToManyPersister.CollectionType, (string) null, (string[]) null, rootAlias, JoinType.LeftOuterJoin, (SqlString) null, this.Factory, (IDictionary<string, IFilter>) new CollectionHelper.EmptyMapClass<string, IFilter>()));
      this.InitPersisters(associations, LockMode.None);
      this.InitStatementString(this.elementPersister, rootAlias, batchSize, subquery);
    }

    protected override string GenerateAliasForColumn(string rootAlias, string column)
    {
      return this.elementPersister.GenerateTableAliasForColumn(rootAlias, column);
    }

    private void InitStatementString(
      IOuterJoinLoadable elementPersister,
      string alias,
      int batchSize,
      SqlString subquery)
    {
      int index = JoinWalker.CountEntityPersisters(this.associations);
      this.Suffixes = BasicLoader.GenerateSuffixes(index + 1);
      int length = JoinWalker.CountCollectionPersisters(this.associations) + 1;
      this.CollectionSuffixes = BasicLoader.GenerateSuffixes(index + 1, length);
      SqlStringBuilder sqlStringBuilder = this.WhereString(alias, this.oneToManyPersister.KeyColumnNames, subquery, batchSize);
      string filter = this.oneToManyPersister.FilterFragment(alias, this.EnabledFilters);
      sqlStringBuilder.Insert(0, StringHelper.MoveAndToBeginning(filter));
      JoinFragment joinFragment = this.MergeOuterJoins(this.associations);
      SqlSelectBuilder sqlSelectBuilder = new SqlSelectBuilder(this.Factory).SetSelectClause(this.oneToManyPersister.SelectFragment((IJoinable) null, (string) null, alias, this.Suffixes[index], this.CollectionSuffixes[0], true) + this.SelectString(this.associations)).SetFromClause(elementPersister.FromTableFragment(alias) + (object) elementPersister.FromJoinFragment(alias, true, true)).SetWhereClause(sqlStringBuilder.ToSqlString()).SetOuterJoins(joinFragment.ToFromFragmentString, joinFragment.ToWhereFragmentString + elementPersister.WhereJoinFragment(alias, true, true));
      sqlSelectBuilder.SetOrderByClause(this.OrderBy(this.associations, this.oneToManyPersister.GetSQLOrderByString(alias)));
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlSelectBuilder.SetComment("load one-to-many " + this.oneToManyPersister.Role);
      this.SqlString = sqlSelectBuilder.ToSqlString();
    }

    public override string ToString()
    {
      return this.GetType().FullName + (object) '(' + this.oneToManyPersister.Role + (object) ')';
    }
  }
}
