// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Criteria.CriteriaJoinWalker
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader.Criteria
{
  public class CriteriaJoinWalker : AbstractEntityJoinWalker
  {
    private readonly CriteriaQueryTranslator translator;
    private readonly ISet<string> querySpaces;
    private readonly IType[] resultTypes;
    private readonly string[] userAliases;
    private readonly IList<string> userAliasList = (IList<string>) new List<string>();
    private static readonly IInternalLogger logger = LoggerProvider.LoggerFor(typeof (CriteriaJoinWalker));

    public CriteriaJoinWalker(
      IOuterJoinLoadable persister,
      CriteriaQueryTranslator translator,
      ISessionFactoryImplementor factory,
      ICriteria criteria,
      string rootEntityName,
      IDictionary<string, IFilter> enabledFilters)
      : base(translator.RootSQLAlias, persister, factory, enabledFilters)
    {
      this.translator = translator;
      this.querySpaces = translator.GetQuerySpaces();
      if (translator.HasProjection)
      {
        this.resultTypes = translator.ProjectedTypes;
        this.InitProjection(translator.GetSelect(enabledFilters), translator.GetWhereCondition(enabledFilters), translator.GetOrderBy(), translator.GetGroupBy(), translator.GetHavingCondition(enabledFilters), enabledFilters, LockMode.None);
      }
      else
      {
        this.resultTypes = new IType[1]
        {
          (IType) TypeFactory.ManyToOne(persister.EntityName)
        };
        this.InitAll(translator.GetWhereCondition(enabledFilters), translator.GetOrderBy(), LockMode.None);
      }
      this.userAliasList.Add(criteria.Alias);
      this.userAliases = ArrayHelper.ToStringArray((ICollection<string>) this.userAliasList);
    }

    protected override void WalkEntityTree(
      IOuterJoinLoadable persister,
      string alias,
      string path,
      int currentDepth)
    {
      base.WalkEntityTree(persister, alias, path, currentDepth);
      this.WalkCompositeComponentIdTree(persister, alias, path);
    }

    private void WalkCompositeComponentIdTree(
      IOuterJoinLoadable persister,
      string alias,
      string path)
    {
      IType identifierType = persister.IdentifierType;
      string identifierPropertyName = persister.IdentifierPropertyName;
      if (identifierType == null || !identifierType.IsComponentType || identifierType is EmbeddedComponentType)
        return;
      ILhsAssociationTypeSqlInfo idLhsSqlInfo = JoinHelper.GetIdLhsSqlInfo(alias, persister, (IMapping) this.Factory);
      this.WalkComponentTree((IAbstractComponentType) identifierType, 0, alias, JoinWalker.SubPath(path, identifierPropertyName), 0, idLhsSqlInfo);
    }

    public IType[] ResultTypes => this.resultTypes;

    public string[] UserAliases => this.userAliases;

    protected override SqlString WhereFragment
    {
      get
      {
        return base.WhereFragment.Append(this.Persister.FilterFragment(this.Alias, this.EnabledFilters));
      }
    }

    public ISet<string> QuerySpaces => this.querySpaces;

    public override string Comment => "criteria query";

    protected override JoinType GetJoinType(
      IAssociationType type,
      FetchMode config,
      string path,
      string lhsTable,
      string[] lhsColumns,
      bool nullable,
      int currentDepth,
      CascadeStyle cascadeStyle)
    {
      if (this.translator.IsJoin(path))
        return this.translator.GetJoinType(path);
      if (this.translator.HasProjection)
        return JoinType.None;
      FetchMode fetchMode = this.translator.RootCriteria.GetFetchMode(path);
      if (CriteriaJoinWalker.IsDefaultFetchMode(fetchMode))
        return base.GetJoinType(type, config, path, lhsTable, lhsColumns, nullable, currentDepth, cascadeStyle);
      if (fetchMode != FetchMode.Join)
        return JoinType.None;
      this.IsDuplicateAssociation(lhsTable, lhsColumns, type);
      return this.GetJoinType(nullable, currentDepth);
    }

    private static bool IsDefaultFetchMode(FetchMode fetchMode) => fetchMode == FetchMode.Default;

    protected override string GenerateTableAlias(int n, string path, IJoinable joinable)
    {
      bool flag = joinable.ConsumesEntityAlias();
      if (!flag && joinable.IsCollection)
      {
        IType elementType = ((ICollectionPersister) joinable).ElementType;
        if (elementType != null)
          flag = elementType.IsComponentType;
      }
      if (flag)
      {
        ICriteria criteria = this.translator.GetCriteria(path);
        string sqlAlias = criteria == null ? (string) null : this.translator.GetSQLAlias(criteria);
        if (sqlAlias != null)
        {
          this.userAliasList.Add(criteria.Alias);
          return sqlAlias;
        }
        this.userAliasList.Add((string) null);
      }
      return base.GenerateTableAlias(n + this.translator.SQLAliasCount, path, joinable);
    }

    protected override string GenerateRootAlias(string tableName)
    {
      return CriteriaQueryTranslator.RootSqlAlias;
    }

    protected override SqlString GetWithClause(string path)
    {
      return this.translator.GetWithClause(path, this.EnabledFilters);
    }
  }
}
