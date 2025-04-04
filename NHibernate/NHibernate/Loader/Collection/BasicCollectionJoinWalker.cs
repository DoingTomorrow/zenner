// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Collection.BasicCollectionJoinWalker
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader.Collection
{
  public class BasicCollectionJoinWalker : CollectionJoinWalker
  {
    private readonly IQueryableCollection collectionPersister;

    public BasicCollectionJoinWalker(
      IQueryableCollection collectionPersister,
      int batchSize,
      SqlString subquery,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
      : base(factory, enabledFilters)
    {
      this.collectionPersister = collectionPersister;
      string rootAlias = this.GenerateRootAlias(collectionPersister.Role);
      this.WalkCollectionTree(collectionPersister, rootAlias);
      IList<OuterJoinableAssociation> associations = (IList<OuterJoinableAssociation>) new List<OuterJoinableAssociation>((IEnumerable<OuterJoinableAssociation>) this.associations);
      associations.Add(new OuterJoinableAssociation((IAssociationType) collectionPersister.CollectionType, (string) null, (string[]) null, rootAlias, JoinType.LeftOuterJoin, (SqlString) null, this.Factory, enabledFilters));
      this.InitPersisters(associations, LockMode.None);
      this.InitStatementString(rootAlias, batchSize, subquery);
    }

    private void InitStatementString(string alias, int batchSize, SqlString subquery)
    {
      int num = JoinWalker.CountEntityPersisters(this.associations);
      int length = JoinWalker.CountCollectionPersisters(this.associations) + 1;
      this.Suffixes = BasicLoader.GenerateSuffixes(num);
      this.CollectionSuffixes = BasicLoader.GenerateSuffixes(num, length);
      SqlStringBuilder sqlStringBuilder = this.WhereString(alias, this.collectionPersister.KeyColumnNames, subquery, batchSize);
      string empty = string.Empty;
      string filter = this.collectionPersister.FilterFragment(alias, this.EnabledFilters);
      if (this.collectionPersister.IsManyToMany)
      {
        IAssociationType elementType = (IAssociationType) this.collectionPersister.ElementType;
        foreach (OuterJoinableAssociation association in (IEnumerable<OuterJoinableAssociation>) this.associations)
        {
          if (association.JoinableType == elementType)
          {
            filter += this.collectionPersister.GetManyToManyFilterFragment(association.RHSAlias, this.EnabledFilters);
            empty += this.collectionPersister.GetManyToManyOrderByString(association.RHSAlias);
          }
        }
      }
      sqlStringBuilder.Insert(0, StringHelper.MoveAndToBeginning(filter));
      JoinFragment joinFragment = this.MergeOuterJoins(this.associations);
      SqlSelectBuilder sqlSelectBuilder = new SqlSelectBuilder(this.Factory).SetSelectClause(this.collectionPersister.SelectFragment(alias, this.CollectionSuffixes[0]) + this.SelectString(this.associations)).SetFromClause(this.collectionPersister.TableName, alias).SetWhereClause(sqlStringBuilder.ToSqlString()).SetOuterJoins(joinFragment.ToFromFragmentString, joinFragment.ToWhereFragmentString);
      sqlSelectBuilder.SetOrderByClause(this.OrderBy(this.associations, this.MergeOrderings(this.collectionPersister.GetSQLOrderByString(alias), empty)));
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlSelectBuilder.SetComment("load collection " + this.collectionPersister.Role);
      this.SqlString = sqlSelectBuilder.ToSqlString();
    }

    protected JoinType GetJoinType(
      IAssociationType type,
      FetchMode config,
      string path,
      ISet visitedAssociations,
      string lhsTable,
      string[] lhsColumns,
      bool nullable,
      int currentDepth)
    {
      JoinType joinType = this.GetJoinType(type, config, path, lhsTable, lhsColumns, nullable, currentDepth, (CascadeStyle) null);
      if (joinType == JoinType.LeftOuterJoin && string.Empty.Equals(path))
        joinType = JoinType.InnerJoin;
      return joinType;
    }

    public override string ToString()
    {
      return this.GetType().FullName + (object) '(' + this.collectionPersister.Role + (object) ')';
    }
  }
}
