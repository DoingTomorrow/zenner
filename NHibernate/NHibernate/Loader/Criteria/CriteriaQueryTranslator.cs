// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Criteria.CriteriaQueryTranslator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Hql.Util;
using NHibernate.Impl;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Loader.Criteria
{
  public class CriteriaQueryTranslator : ICriteriaQuery
  {
    private const int AliasCount = 0;
    public static readonly string RootSqlAlias = CriteriaSpecification.RootAlias + (object) '_';
    private static readonly IInternalLogger logger = LoggerProvider.LoggerFor(typeof (CriteriaQueryTranslator));
    private readonly ICriteriaQuery outerQueryTranslator;
    private readonly CriteriaImpl rootCriteria;
    private readonly string rootEntityName;
    private readonly string rootSQLAlias;
    private int indexForAlias;
    private readonly IDictionary<ICriteria, ICriteriaInfoProvider> criteriaInfoMap = (IDictionary<ICriteria, ICriteriaInfoProvider>) new Dictionary<ICriteria, ICriteriaInfoProvider>();
    private readonly IDictionary<string, ICriteriaInfoProvider> nameCriteriaInfoMap = (IDictionary<string, ICriteriaInfoProvider>) new Dictionary<string, ICriteriaInfoProvider>();
    private readonly ISet<ICollectionPersister> criteriaCollectionPersisters = (ISet<ICollectionPersister>) new HashedSet<ICollectionPersister>();
    private readonly IDictionary<ICriteria, string> criteriaSQLAliasMap = (IDictionary<ICriteria, string>) new Dictionary<ICriteria, string>();
    private readonly IDictionary<string, ICriteria> aliasCriteriaMap = (IDictionary<string, ICriteria>) new Dictionary<string, ICriteria>();
    private readonly IDictionary<string, ICriteria> associationPathCriteriaMap = (IDictionary<string, ICriteria>) new LinkedHashMap<string, ICriteria>();
    private readonly IDictionary<string, JoinType> associationPathJoinTypesMap = (IDictionary<string, JoinType>) new LinkedHashMap<string, JoinType>();
    private readonly IDictionary<string, ICriterion> withClauseMap = (IDictionary<string, ICriterion>) new Dictionary<string, ICriterion>();
    private readonly ISessionFactoryImplementor sessionFactory;
    private SessionFactoryHelper helper;
    private readonly ICollection<IParameterSpecification> collectedParameterSpecifications;
    private readonly ICollection<NamedParameter> namedParameters;

    public CriteriaQueryTranslator(
      ISessionFactoryImplementor factory,
      CriteriaImpl criteria,
      string rootEntityName,
      string rootSQLAlias,
      ICriteriaQuery outerQuery)
      : this(factory, criteria, rootEntityName, rootSQLAlias)
    {
      this.outerQueryTranslator = outerQuery;
      this.collectedParameterSpecifications = outerQuery.CollectedParameterSpecifications;
      this.namedParameters = outerQuery.CollectedParameters;
    }

    public CriteriaQueryTranslator(
      ISessionFactoryImplementor factory,
      CriteriaImpl criteria,
      string rootEntityName,
      string rootSQLAlias)
    {
      this.rootCriteria = criteria;
      this.rootEntityName = rootEntityName;
      this.sessionFactory = factory;
      this.rootSQLAlias = rootSQLAlias;
      this.helper = new SessionFactoryHelper(factory);
      this.collectedParameterSpecifications = (ICollection<IParameterSpecification>) new List<IParameterSpecification>();
      this.namedParameters = (ICollection<NamedParameter>) new List<NamedParameter>();
      this.CreateAliasCriteriaMap();
      this.CreateAssociationPathCriteriaMap();
      this.CreateCriteriaEntityNameMap();
      this.CreateCriteriaCollectionPersisters();
      this.CreateCriteriaSQLAliasMap();
    }

    [CLSCompliant(false)]
    public string RootSQLAlias => this.rootSQLAlias;

    public ISet<string> GetQuerySpaces()
    {
      ISet<string> querySpaces = (ISet<string>) new HashedSet<string>();
      foreach (ICriteriaInfoProvider criteriaInfoProvider in (IEnumerable<ICriteriaInfoProvider>) this.criteriaInfoMap.Values)
        querySpaces.AddAll((ICollection<string>) criteriaInfoProvider.Spaces);
      foreach (ICollectionPersister collectionPersister in (IEnumerable<ICollectionPersister>) this.criteriaCollectionPersisters)
        querySpaces.AddAll((ICollection<string>) collectionPersister.CollectionSpaces);
      return querySpaces;
    }

    public int SQLAliasCount => this.criteriaSQLAliasMap.Count;

    public CriteriaImpl RootCriteria => this.rootCriteria;

    public QueryParameters GetQueryParameters()
    {
      RowSelection rowSelection = new RowSelection();
      rowSelection.FirstRow = this.rootCriteria.FirstResult;
      rowSelection.MaxRows = this.rootCriteria.MaxResults;
      rowSelection.Timeout = this.rootCriteria.Timeout;
      rowSelection.FetchSize = this.rootCriteria.FetchSize;
      Dictionary<string, LockMode> lockModes = new Dictionary<string, LockMode>();
      foreach (KeyValuePair<string, LockMode> lockMode in (IEnumerable<KeyValuePair<string, LockMode>>) this.rootCriteria.LockModes)
      {
        ICriteria aliasedCriteria = this.GetAliasedCriteria(lockMode.Key);
        lockModes[this.GetSQLAlias(aliasedCriteria)] = lockMode.Value;
      }
      foreach (CriteriaImpl.Subcriteria subcriteria in this.rootCriteria.IterateSubcriteria())
      {
        LockMode lockMode = subcriteria.LockMode;
        if (lockMode != null)
          lockModes[this.GetSQLAlias((ICriteria) subcriteria)] = lockMode;
      }
      return new QueryParameters((IDictionary<string, TypedValue>) this.CollectedParameters.ToDictionary<NamedParameter, string, TypedValue>((Func<NamedParameter, string>) (np => np.Name), (Func<NamedParameter, TypedValue>) (np => new TypedValue(np.Type, np.Value, EntityMode.Poco))), (IDictionary<string, LockMode>) lockModes, rowSelection, this.rootCriteria.IsReadOnlyInitialized, this.rootCriteria.IsReadOnlyInitialized && this.rootCriteria.IsReadOnly, this.rootCriteria.Cacheable, this.rootCriteria.CacheRegion, this.rootCriteria.Comment, this.rootCriteria.LookupByNaturalKey, this.rootCriteria.ResultTransformer);
    }

    public SqlString GetGroupBy()
    {
      return this.rootCriteria.Projection.IsGrouped ? this.rootCriteria.Projection.ToGroupSqlString(this.rootCriteria.ProjectionCriteria, (ICriteriaQuery) this, (IDictionary<string, IFilter>) new CollectionHelper.EmptyMapClass<string, IFilter>()) : SqlString.Empty;
    }

    public SqlString GetSelect(IDictionary<string, IFilter> enabledFilters)
    {
      return this.rootCriteria.Projection.ToSqlString(this.rootCriteria.ProjectionCriteria, 0, (ICriteriaQuery) this, enabledFilters);
    }

    public IType[] ProjectedTypes
    {
      get
      {
        return this.rootCriteria.Projection.GetTypes((ICriteria) this.rootCriteria, (ICriteriaQuery) this);
      }
    }

    public string[] ProjectedColumnAliases
    {
      get
      {
        return !(this.rootCriteria.Projection is IEnhancedProjection) ? this.rootCriteria.Projection.GetColumnAliases(0) : ((IEnhancedProjection) this.rootCriteria.Projection).GetColumnAliases(0, (ICriteria) this.rootCriteria, (ICriteriaQuery) this);
      }
    }

    public string[] ProjectedAliases => this.rootCriteria.Projection.Aliases;

    public SqlString GetWhereCondition(IDictionary<string, IFilter> enabledFilters)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(30);
      bool flag = true;
      foreach (CriteriaImpl.CriterionEntry iterateExpressionEntry in this.rootCriteria.IterateExpressionEntries())
      {
        if (!CriteriaQueryTranslator.HasGroupedOrAggregateProjection(iterateExpressionEntry.Criterion.GetProjections()))
        {
          if (!flag)
            sqlStringBuilder.Add(" and ");
          flag = false;
          SqlString sqlString = iterateExpressionEntry.Criterion.ToSqlString(iterateExpressionEntry.Criteria, (ICriteriaQuery) this, enabledFilters);
          sqlStringBuilder.Add(sqlString);
        }
      }
      return sqlStringBuilder.ToSqlString();
    }

    public SqlString GetOrderBy()
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(30);
      bool flag = true;
      foreach (CriteriaImpl.OrderEntry iterateOrdering in this.rootCriteria.IterateOrderings())
      {
        if (!flag)
          sqlStringBuilder.Add(", ");
        flag = false;
        sqlStringBuilder.Add(iterateOrdering.Order.ToSqlString(iterateOrdering.Criteria, (ICriteriaQuery) this));
      }
      return sqlStringBuilder.ToSqlString();
    }

    public ISessionFactoryImplementor Factory => this.sessionFactory;

    public string GenerateSQLAlias() => StringHelper.GenerateAlias(this.rootSQLAlias, 0);

    private ICriteria GetAliasedCriteria(string alias)
    {
      ICriteria aliasedCriteria;
      this.aliasCriteriaMap.TryGetValue(alias, out aliasedCriteria);
      return aliasedCriteria;
    }

    public bool IsJoin(string path) => this.associationPathCriteriaMap.ContainsKey(path);

    public JoinType GetJoinType(string path)
    {
      JoinType joinType;
      return this.associationPathJoinTypesMap.TryGetValue(path, out joinType) ? joinType : JoinType.InnerJoin;
    }

    public ICriteria GetCriteria(string path)
    {
      ICriteria criteria;
      this.associationPathCriteriaMap.TryGetValue(path, out criteria);
      CriteriaQueryTranslator.logger.DebugFormat("getCriteria for path={0} crit={1}", (object) path, (object) criteria);
      return criteria;
    }

    private void CreateAliasCriteriaMap()
    {
      this.aliasCriteriaMap[this.rootCriteria.Alias] = (ICriteria) this.rootCriteria;
      foreach (ICriteria criteria in this.rootCriteria.IterateSubcriteria())
      {
        if (criteria.Alias != null)
        {
          try
          {
            this.aliasCriteriaMap.Add(criteria.Alias, criteria);
          }
          catch (ArgumentException ex)
          {
            throw new QueryException("duplicate alias: " + criteria.Alias, (Exception) ex);
          }
        }
      }
    }

    private void CreateAssociationPathCriteriaMap()
    {
      foreach (CriteriaImpl.Subcriteria subcriteria in this.rootCriteria.IterateSubcriteria())
      {
        string wholeAssociationPath = this.GetWholeAssociationPath(subcriteria);
        try
        {
          this.associationPathCriteriaMap.Add(wholeAssociationPath, (ICriteria) subcriteria);
        }
        catch (ArgumentException ex)
        {
          throw new QueryException("duplicate association path: " + wholeAssociationPath, (Exception) ex);
        }
        try
        {
          this.associationPathJoinTypesMap.Add(wholeAssociationPath, subcriteria.JoinType);
        }
        catch (ArgumentException ex)
        {
          throw new QueryException("duplicate association path: " + wholeAssociationPath, (Exception) ex);
        }
        try
        {
          if (subcriteria.WithClause != null)
            this.withClauseMap.Add(wholeAssociationPath, subcriteria.WithClause);
        }
        catch (ArgumentException ex)
        {
          throw new QueryException("duplicate association path: " + wholeAssociationPath, (Exception) ex);
        }
      }
    }

    private string GetWholeAssociationPath(CriteriaImpl.Subcriteria subcriteria)
    {
      string qualifiedName = subcriteria.Path;
      ICriteria criteria = (ICriteria) null;
      if (qualifiedName.IndexOf('.') > 0)
      {
        string key = StringHelper.Root(qualifiedName);
        if (!key.Equals(subcriteria.Alias))
          this.aliasCriteriaMap.TryGetValue(key, out criteria);
      }
      if (criteria == null)
        criteria = subcriteria.Parent;
      else
        qualifiedName = StringHelper.Unroot(qualifiedName);
      return criteria.Equals((object) this.rootCriteria) ? qualifiedName : this.GetWholeAssociationPath((CriteriaImpl.Subcriteria) criteria) + (object) '.' + qualifiedName;
    }

    private void CreateCriteriaEntityNameMap()
    {
      ICriteriaInfoProvider criteriaInfoProvider = (ICriteriaInfoProvider) new EntityCriteriaInfoProvider((NHibernate.Persister.Entity.IQueryable) this.sessionFactory.GetEntityPersister(this.rootEntityName));
      this.criteriaInfoMap.Add((ICriteria) this.rootCriteria, criteriaInfoProvider);
      this.nameCriteriaInfoMap.Add(criteriaInfoProvider.Name, criteriaInfoProvider);
      foreach (KeyValuePair<string, ICriteria> associationPathCriteria in (IEnumerable<KeyValuePair<string, ICriteria>>) this.associationPathCriteriaMap)
      {
        ICriteriaInfoProvider pathInfo = this.GetPathInfo(associationPathCriteria.Key);
        this.criteriaInfoMap.Add(associationPathCriteria.Value, pathInfo);
        this.nameCriteriaInfoMap[pathInfo.Name] = pathInfo;
      }
    }

    private void CreateCriteriaCollectionPersisters()
    {
      foreach (KeyValuePair<string, ICriteria> associationPathCriteria in (IEnumerable<KeyValuePair<string, ICriteria>>) this.associationPathCriteriaMap)
      {
        IJoinable pathJoinable = this.GetPathJoinable(associationPathCriteria.Key);
        if (pathJoinable != null && pathJoinable.IsCollection)
          this.criteriaCollectionPersisters.Add((ICollectionPersister) pathJoinable);
      }
    }

    private IJoinable GetPathJoinable(string path)
    {
      IJoinable pathJoinable = (IJoinable) this.Factory.GetEntityPersister(this.rootEntityName);
      IPropertyMapping propertyMapping = (IPropertyMapping) pathJoinable;
      string str1 = "";
      foreach (string str2 in new StringTokenizer(path, ".", false))
      {
        string propertyName = str1 + str2;
        IType type = propertyMapping.ToType(propertyName);
        if (type.IsAssociationType)
        {
          if (type.IsCollectionType && !this.Factory.GetCollectionPersister(((CollectionType) type).Role).ElementType.IsEntityType)
            return (IJoinable) null;
          IAssociationType associationType = (IAssociationType) type;
          pathJoinable = associationType.GetAssociatedJoinable(this.Factory);
          propertyMapping = (IPropertyMapping) this.Factory.GetEntityPersister(associationType.GetAssociatedEntityName(this.Factory));
          str1 = "";
        }
        else
        {
          if (!type.IsComponentType)
            throw new QueryException("not an association: " + propertyName);
          str1 = propertyName + (object) '.';
        }
      }
      return pathJoinable;
    }

    private ICriteriaInfoProvider GetPathInfo(string path)
    {
      StringTokenizer stringTokenizer = new StringTokenizer(path, ".", false);
      string str1 = string.Empty;
      ICriteriaInfoProvider pathInfo;
      if (!this.nameCriteriaInfoMap.TryGetValue(this.rootEntityName, out pathInfo))
        throw new ArgumentException("Could not find ICriteriaInfoProvider for: " + path);
      foreach (string str2 in stringTokenizer)
      {
        string relativePath = str1 + str2;
        CriteriaQueryTranslator.logger.DebugFormat("searching for {0}", (object) relativePath);
        IType type = pathInfo.GetType(relativePath);
        if (type.IsAssociationType)
        {
          IAssociationType associationType = (IAssociationType) type;
          CollectionType collectionType = type.IsCollectionType ? (CollectionType) type : (CollectionType) null;
          IType elementType = collectionType?.GetElementType(this.sessionFactory);
          pathInfo = collectionType == null || !elementType.IsComponentType ? (collectionType == null || elementType.IsEntityType ? (ICriteriaInfoProvider) new EntityCriteriaInfoProvider((NHibernate.Persister.Entity.IQueryable) this.sessionFactory.GetEntityPersister(associationType.GetAssociatedEntityName(this.sessionFactory))) : (ICriteriaInfoProvider) new ScalarCollectionCriteriaInfoProvider(this.helper, collectionType.Role)) : (ICriteriaInfoProvider) new ComponentCollectionCriteriaInfoProvider(this.helper.GetCollectionPersister(collectionType.Role));
          str1 = string.Empty;
        }
        else
        {
          if (!type.IsComponentType)
            throw new QueryException("not an association: " + relativePath);
          str1 = relativePath + (object) '.';
        }
      }
      CriteriaQueryTranslator.logger.DebugFormat("returning entity name={0} for path={1} class={2}", (object) pathInfo.Name, (object) path, (object) pathInfo.GetType().Name);
      return pathInfo;
    }

    private void CreateCriteriaSQLAliasMap()
    {
      int num = 0;
      foreach (KeyValuePair<ICriteria, ICriteriaInfoProvider> criteriaInfo in (IEnumerable<KeyValuePair<ICriteria, ICriteriaInfoProvider>>) this.criteriaInfoMap)
      {
        ICriteria key = criteriaInfo.Key;
        string description = key.Alias ?? criteriaInfo.Value.Name;
        this.criteriaSQLAliasMap[key] = StringHelper.GenerateAlias(description, num++);
        CriteriaQueryTranslator.logger.DebugFormat("put criteria={0} alias={1}", (object) key, (object) this.criteriaSQLAliasMap[key]);
      }
      this.criteriaSQLAliasMap[(ICriteria) this.rootCriteria] = this.rootSQLAlias;
    }

    public bool HasProjection => this.rootCriteria.Projection != null;

    public string GetSQLAlias(ICriteria criteria)
    {
      string criteriaSqlAlias = this.criteriaSQLAliasMap[criteria];
      CriteriaQueryTranslator.logger.DebugFormat("returning alias={0} for criteria={1}", (object) criteriaSqlAlias, (object) criteria);
      return criteriaSqlAlias;
    }

    public string GetEntityName(ICriteria criteria)
    {
      ICriteriaInfoProvider criteriaInfoProvider;
      return !this.criteriaInfoMap.TryGetValue(criteria, out criteriaInfoProvider) ? (string) null : criteriaInfoProvider.Name;
    }

    public string GetColumn(ICriteria criteria, string propertyName)
    {
      string[] columns = this.GetColumns(criteria, propertyName);
      return columns.Length == 1 ? columns[0] : throw new QueryException("property does not map to a single column: " + propertyName);
    }

    public string[] GetColumnsUsingProjection(ICriteria subcriteria, string propertyName)
    {
      try
      {
        return this.GetColumns(subcriteria, propertyName);
      }
      catch (HibernateException ex)
      {
        if (this.outerQueryTranslator != null)
          return this.outerQueryTranslator.GetColumnsUsingProjection(subcriteria, propertyName);
        throw;
      }
    }

    public string[] GetIdentifierColumns(ICriteria subcriteria)
    {
      string[] identifierColumnNames = ((ILoadable) this.GetPropertyMapping(this.GetEntityName(subcriteria))).IdentifierColumnNames;
      return StringHelper.Qualify(this.GetSQLAlias(subcriteria), identifierColumnNames);
    }

    public IType GetIdentifierType(ICriteria subcriteria)
    {
      return ((IEntityPersister) this.GetPropertyMapping(this.GetEntityName(subcriteria))).IdentifierType;
    }

    public TypedValue GetTypedIdentifierValue(ICriteria subcriteria, object value)
    {
      return new TypedValue(((IEntityPersister) this.GetPropertyMapping(this.GetEntityName(subcriteria))).IdentifierType, value, EntityMode.Poco);
    }

    public string[] GetColumns(ICriteria subcriteria, string propertyName)
    {
      return this.GetPropertyMapping(this.GetEntityName(subcriteria, propertyName) ?? throw new QueryException("Could not find property " + propertyName)).ToColumns(this.GetSQLAlias(subcriteria, propertyName), this.GetPropertyName(propertyName));
    }

    public IType GetTypeUsingProjection(ICriteria subcriteria, string propertyName)
    {
      IProjection projection = this.rootCriteria.Projection;
      IType[] types = projection == null ? (IType[]) null : projection.GetTypes(propertyName, subcriteria, (ICriteriaQuery) this);
      if (types == null)
      {
        try
        {
          return this.GetType(subcriteria, propertyName);
        }
        catch (HibernateException ex)
        {
          if (this.outerQueryTranslator != null)
            return this.outerQueryTranslator.GetType(subcriteria, propertyName);
          throw;
        }
      }
      else
        return types.Length == 1 ? types[0] : throw new QueryException("not a single-length projection: " + propertyName);
    }

    public IType GetType(ICriteria subcriteria, string propertyName)
    {
      return this.GetPropertyMapping(this.GetEntityName(subcriteria, propertyName)).ToType(this.GetPropertyName(propertyName));
    }

    public TypedValue GetTypedValue(ICriteria subcriteria, string propertyName, object value)
    {
      if (value is System.Type type)
      {
        NHibernate.Persister.Entity.IQueryable queryableUsingImports = this.helper.FindQueryableUsingImports(type.FullName);
        if (queryableUsingImports != null && queryableUsingImports.DiscriminatorValue != null)
          return new TypedValue(queryableUsingImports.DiscriminatorType, queryableUsingImports.DiscriminatorValue, EntityMode.Poco);
      }
      return new TypedValue(this.GetTypeUsingProjection(subcriteria, propertyName), value, EntityMode.Poco);
    }

    private IPropertyMapping GetPropertyMapping(string entityName)
    {
      ICriteriaInfoProvider criteriaInfoProvider;
      if (!this.nameCriteriaInfoMap.TryGetValue(entityName, out criteriaInfoProvider))
        throw new InvalidOperationException("Could not find criteria info provider for: " + entityName);
      return criteriaInfoProvider.PropertyMapping;
    }

    public string GetEntityName(ICriteria subcriteria, string propertyName)
    {
      if (propertyName.IndexOf('.') > 0)
      {
        ICriteria aliasedCriteria = this.GetAliasedCriteria(StringHelper.Root(propertyName));
        if (aliasedCriteria != null)
          return this.GetEntityName(aliasedCriteria);
      }
      return this.GetEntityName(subcriteria);
    }

    public string GetSQLAlias(ICriteria criteria, string propertyName)
    {
      if (propertyName.IndexOf('.') > 0)
      {
        ICriteria aliasedCriteria = this.GetAliasedCriteria(StringHelper.Root(propertyName));
        if (aliasedCriteria != null)
          return this.GetSQLAlias(aliasedCriteria);
      }
      return this.GetSQLAlias(criteria);
    }

    public string GetPropertyName(string propertyName)
    {
      if (propertyName.IndexOf('.') > 0)
      {
        string alias = StringHelper.Root(propertyName);
        if (this.GetAliasedCriteria(alias) != null)
          return propertyName.Substring(alias.Length + 1);
      }
      return propertyName;
    }

    public SqlString GetWithClause(string path, IDictionary<string, IFilter> enabledFilters)
    {
      if (!this.withClauseMap.ContainsKey(path))
        return (SqlString) null;
      return this.withClauseMap[path]?.ToSqlString(this.GetCriteria(path), (ICriteriaQuery) this, enabledFilters);
    }

    public int GetIndexForAlias() => this.indexForAlias++;

    public IEnumerable<Parameter> NewQueryParameter(TypedValue parameter)
    {
      return this.NewQueryParameter("cp", parameter);
    }

    private IEnumerable<Parameter> NewQueryParameter(string parameterPrefix, TypedValue parameter)
    {
      string name = parameterPrefix + (object) this.CollectedParameterSpecifications.Count;
      CriteriaNamedParameterSpecification parameterSpecification = new CriteriaNamedParameterSpecification(name, parameter.Type);
      this.collectedParameterSpecifications.Add((IParameterSpecification) parameterSpecification);
      this.namedParameters.Add(new NamedParameter(name, parameter.Value, parameter.Type));
      return parameterSpecification.GetIdsForBackTrack((IMapping) this.Factory).Select<string, Parameter>((Func<string, Parameter>) (x =>
      {
        Parameter placeholder = Parameter.Placeholder;
        placeholder.BackTrack = (object) x;
        return placeholder;
      }));
    }

    public ICollection<IParameterSpecification> CollectedParameterSpecifications
    {
      get => this.collectedParameterSpecifications;
    }

    public ICollection<NamedParameter> CollectedParameters => this.namedParameters;

    public Parameter CreateSkipParameter(int value)
    {
      return this.NewQueryParameter("skip_", new TypedValue((IType) NHibernateUtil.Int32, (object) value, EntityMode.Poco)).Single<Parameter>();
    }

    public Parameter CreateTakeParameter(int value)
    {
      return this.NewQueryParameter("take_", new TypedValue((IType) NHibernateUtil.Int32, (object) value, EntityMode.Poco)).Single<Parameter>();
    }

    public SqlString GetHavingCondition(IDictionary<string, IFilter> enabledFilters)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(30);
      bool flag = true;
      foreach (CriteriaImpl.CriterionEntry iterateExpressionEntry in this.rootCriteria.IterateExpressionEntries())
      {
        if (CriteriaQueryTranslator.HasGroupedOrAggregateProjection(iterateExpressionEntry.Criterion.GetProjections()))
        {
          if (!flag)
            sqlStringBuilder.Add(" and ");
          flag = false;
          SqlString sqlString = iterateExpressionEntry.Criterion.ToSqlString(iterateExpressionEntry.Criteria, (ICriteriaQuery) this, enabledFilters);
          sqlStringBuilder.Add(sqlString);
        }
      }
      return sqlStringBuilder.ToSqlString();
    }

    protected static bool HasGroupedOrAggregateProjection(IProjection[] projections)
    {
      if (projections != null)
      {
        foreach (IProjection projection in projections)
        {
          if (projection.IsGrouped || projection.IsAggregate)
            return true;
        }
      }
      return false;
    }

    public string[] GetColumnAliasesUsingProjection(ICriteria subcriteria, string propertyName)
    {
      IProjection projection = this.rootCriteria.Projection;
      string[] columnAliases = projection == null ? (string[]) null : projection.GetColumnAliases(propertyName, 0);
      if (columnAliases != null)
        return columnAliases;
      try
      {
        return this.GetColumns(subcriteria, propertyName);
      }
      catch (HibernateException ex)
      {
        if (this.outerQueryTranslator != null)
          return this.outerQueryTranslator.GetColumnAliasesUsingProjection(subcriteria, propertyName);
        throw;
      }
    }
  }
}
