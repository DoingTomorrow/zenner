// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.MultiCriteriaImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using Iesi.Collections.Generic;
using NHibernate.Cache;
using NHibernate.Criterion;
using NHibernate.Driver;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Loader.Criteria;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace NHibernate.Impl
{
  public class MultiCriteriaImpl : IMultiCriteria
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (MultiCriteriaImpl));
    private readonly IList<ICriteria> criteriaQueries = (IList<ICriteria>) new System.Collections.Generic.List<ICriteria>();
    private readonly IList<System.Type> resultCollectionGenericType = (IList<System.Type>) new System.Collections.Generic.List<System.Type>();
    private readonly SessionImpl session;
    private readonly ISessionFactoryImplementor factory;
    private readonly System.Collections.Generic.List<CriteriaQueryTranslator> translators = new System.Collections.Generic.List<CriteriaQueryTranslator>();
    private readonly System.Collections.Generic.List<QueryParameters> parameters = new System.Collections.Generic.List<QueryParameters>();
    private readonly System.Collections.Generic.List<CriteriaLoader> loaders = new System.Collections.Generic.List<CriteriaLoader>();
    private readonly System.Collections.Generic.List<int> loaderCriteriaMap = new System.Collections.Generic.List<int>();
    private readonly NHibernate.Dialect.Dialect dialect;
    private IList criteriaResults;
    private readonly Dictionary<string, int> criteriaResultPositions = new Dictionary<string, int>();
    private bool isCacheable;
    private bool forceCacheRefresh;
    private string cacheRegion;
    private IResultTransformer resultTransformer;
    private readonly IResultSetsCommand resultSetsCommand;

    internal MultiCriteriaImpl(SessionImpl session, ISessionFactoryImplementor factory)
    {
      IDriver driver = session.Factory.ConnectionProvider.Driver;
      this.dialect = session.Factory.Dialect;
      this.resultSetsCommand = driver.GetResultSetsCommand((ISessionImplementor) session);
      this.session = session;
      this.factory = factory;
    }

    public SqlString SqlString => this.resultSetsCommand.Sql;

    public IList List()
    {
      using (new SessionIdLoggingContext(this.session.SessionId))
      {
        bool flag = this.session.Factory.Settings.IsQueryCacheEnabled && this.isCacheable;
        this.CreateCriteriaLoaders();
        this.CombineCriteriaQueries();
        if (MultiCriteriaImpl.log.IsDebugEnabled)
        {
          MultiCriteriaImpl.log.DebugFormat("Multi criteria with {0} criteria queries.", (object) this.criteriaQueries.Count);
          for (int index = 0; index < this.criteriaQueries.Count; ++index)
            MultiCriteriaImpl.log.DebugFormat("Query #{0}: {1}", (object) index, (object) this.criteriaQueries[index]);
        }
        this.criteriaResults = !flag ? this.ListIgnoreQueryCache() : this.ListUsingQueryCache();
        return this.criteriaResults;
      }
    }

    private IList ListUsingQueryCache()
    {
      IQueryCache queryCache = this.session.Factory.GetQueryCache(this.cacheRegion);
      ISet filterKeys = FilterKey.CreateFilterKeys(this.session.EnabledFilters, this.session.EntityMode);
      ISet<string> querySpaces = (ISet<string>) new HashedSet<string>();
      System.Collections.Generic.List<IType[]> assemblers = new System.Collections.Generic.List<IType[]>();
      int[] maxRows = new int[this.loaders.Count];
      int[] firstRows = new int[this.loaders.Count];
      for (int index = 0; index < this.loaders.Count; ++index)
      {
        querySpaces.AddAll((ICollection<string>) this.loaders[index].QuerySpaces);
        assemblers.Add(this.loaders[index].ResultTypes);
        firstRows[index] = this.parameters[index].RowSelection.FirstRow;
        maxRows[index] = this.parameters[index].RowSelection.MaxRows;
      }
      MultipleQueriesCacheAssembler queriesCacheAssembler = new MultipleQueriesCacheAssembler((IList) assemblers);
      QueryParameters combinedQueryParameters = this.CreateCombinedQueryParameters();
      QueryKey key = new QueryKey(this.session.Factory, this.SqlString, combinedQueryParameters, filterKeys).SetFirstRows(firstRows).SetMaxRows(maxRows);
      IList results = queriesCacheAssembler.GetResultFromQueryCache((ISessionImplementor) this.session, combinedQueryParameters, querySpaces, queryCache, key);
      if (results == null)
      {
        MultiCriteriaImpl.log.Debug((object) "Cache miss for multi criteria query");
        IList list = this.DoList();
        queryCache.Put(key, new ICacheAssembler[1]
        {
          (ICacheAssembler) queriesCacheAssembler
        }, (IList) new object[1]{ (object) list }, (combinedQueryParameters.NaturalKeyLookup ? 1 : 0) != 0, (ISessionImplementor) this.session);
        results = list;
      }
      return this.GetResultList(results);
    }

    private IList ListIgnoreQueryCache() => this.GetResultList(this.DoList());

    protected virtual IList GetResultList(IList results)
    {
      ArrayList resultList1 = new ArrayList(this.resultCollectionGenericType.Count);
      for (int index = 0; index < this.criteriaQueries.Count; ++index)
      {
        if (this.resultCollectionGenericType[index] == typeof (object))
          resultList1.Add((object) new ArrayList());
        else
          resultList1.Add(Activator.CreateInstance(typeof (System.Collections.Generic.List<>).MakeGenericType(this.resultCollectionGenericType[index])));
      }
      for (int index = 0; index < this.loaders.Count; ++index)
      {
        IList resultList2 = this.loaders[index].GetResultList((IList) results[index], this.parameters[index].ResultTransformer);
        int loaderCriteria = this.loaderCriteriaMap[index];
        ArrayHelper.AddAll((IList) resultList1[loaderCriteria], resultList2);
      }
      if (this.resultTransformer != null)
      {
        for (int index = 0; index < results.Count; ++index)
          resultList1[index] = (object) this.resultTransformer.TransformList((IList) resultList1[index]);
      }
      return (IList) resultList1;
    }

    private IList DoList()
    {
      System.Collections.Generic.List<IList> results = new System.Collections.Generic.List<IList>();
      this.GetResultsFromDatabase((IList) results);
      return (IList) results;
    }

    private void CombineCriteriaQueries()
    {
      foreach (CriteriaLoader loader in this.loaders)
      {
        CriteriaQueryTranslator translator = loader.Translator;
        this.translators.Add(translator);
        QueryParameters queryParameters = translator.GetQueryParameters();
        this.parameters.Add(queryParameters);
        this.resultSetsCommand.Append(loader.CreateSqlCommand(queryParameters, (ISessionImplementor) this.session));
      }
    }

    private void GetResultsFromDatabase(IList results)
    {
      bool statisticsEnabled = this.session.Factory.Statistics.IsStatisticsEnabled;
      Stopwatch stopwatch = new Stopwatch();
      if (statisticsEnabled)
        stopwatch.Start();
      int rows = 0;
      try
      {
        using (IDataReader reader = this.resultSetsCommand.GetReader(new int?()))
        {
          ArrayList[] arrayListArray = new ArrayList[this.loaders.Count];
          System.Collections.Generic.List<EntityKey[]>[] entityKeyArrayListArray = new System.Collections.Generic.List<EntityKey[]>[this.loaders.Count];
          bool[] flagArray = new bool[this.loaders.Count];
          for (int index1 = 0; index1 < this.loaders.Count; ++index1)
          {
            CriteriaLoader loader = this.loaders[index1];
            int length = loader.EntityPersisters.Length;
            arrayListArray[index1] = length == 0 ? (ArrayList) null : new ArrayList(length);
            EntityKey[] keys = new EntityKey[length];
            QueryParameters parameter = this.parameters[index1];
            IList list = (IList) new ArrayList();
            RowSelection rowSelection = this.parameters[index1].RowSelection;
            flagArray[index1] = loader.IsSubselectLoadingEnabled;
            entityKeyArrayListArray[index1] = flagArray[index1] ? new System.Collections.Generic.List<EntityKey[]>() : (System.Collections.Generic.List<EntityKey[]>) null;
            int num = NHibernate.Loader.Loader.HasMaxRows(rowSelection) ? rowSelection.MaxRows : int.MaxValue;
            if (!this.dialect.SupportsLimitOffset || !loader.UseLimit(rowSelection, this.dialect))
              NHibernate.Loader.Loader.Advance(reader, rowSelection);
            for (int index2 = 0; index2 < num && reader.Read(); ++index2)
            {
              ++rows;
              object rowFromResultSet = loader.GetRowFromResultSet(reader, (ISessionImplementor) this.session, parameter, loader.GetLockModes(parameter.LockModes), (EntityKey) null, (IList) arrayListArray[index1], keys, true);
              if (flagArray[index1])
              {
                entityKeyArrayListArray[index1].Add(keys);
                keys = new EntityKey[length];
              }
              list.Add(rowFromResultSet);
            }
            results.Add((object) list);
            reader.NextResult();
          }
          for (int index = 0; index < this.loaders.Count; ++index)
          {
            CriteriaLoader loader = this.loaders[index];
            loader.InitializeEntitiesAndCollections((IList) arrayListArray[index], (object) reader, (ISessionImplementor) this.session, false);
            if (flagArray[index])
              loader.CreateSubselects((IList<EntityKey[]>) entityKeyArrayListArray[index], this.parameters[index], (ISessionImplementor) this.session);
          }
        }
      }
      catch (Exception ex)
      {
        string message = string.Format("Failed to execute multi criteria: [{0}]", (object) this.resultSetsCommand.Sql);
        MultiCriteriaImpl.log.Error((object) message, ex);
        throw ADOExceptionHelper.Convert(this.session.Factory.SQLExceptionConverter, ex, "Failed to execute multi criteria", this.resultSetsCommand.Sql);
      }
      if (!statisticsEnabled)
        return;
      stopwatch.Stop();
      this.session.Factory.StatisticsImplementor.QueryExecuted(string.Format("{0} queries (MultiCriteria)", (object) this.loaders.Count), rows, stopwatch.Elapsed);
    }

    private void CreateCriteriaLoaders()
    {
      int num = 0;
      foreach (CriteriaImpl criteriaQuery in (IEnumerable<ICriteria>) this.criteriaQueries)
      {
        string[] implementors = this.factory.GetImplementors(criteriaQuery.EntityOrClassName);
        int length = implementors.Length;
        ISet<string> set = (ISet<string>) new HashedSet<string>();
        for (int index = 0; index < length; ++index)
        {
          CriteriaLoader criteriaLoader = new CriteriaLoader(this.session.GetOuterJoinLoadable(implementors[index]), this.factory, criteriaQuery, implementors[index], this.session.EnabledFilters);
          this.loaders.Add(criteriaLoader);
          this.loaderCriteriaMap.Add(num);
          set.AddAll((ICollection<string>) criteriaLoader.QuerySpaces);
        }
        ++num;
      }
    }

    public IMultiCriteria Add(System.Type resultGenericListType, ICriteria criteria)
    {
      this.criteriaQueries.Add(criteria);
      this.resultCollectionGenericType.Add(resultGenericListType);
      return (IMultiCriteria) this;
    }

    public IMultiCriteria Add(ICriteria criteria) => this.Add<object>(criteria);

    public IMultiCriteria Add(string key, ICriteria criteria) => this.Add<object>(key, criteria);

    public IMultiCriteria Add(DetachedCriteria detachedCriteria)
    {
      return this.Add<object>(detachedCriteria);
    }

    public IMultiCriteria Add(string key, DetachedCriteria detachedCriteria)
    {
      return this.Add<object>(key, detachedCriteria);
    }

    public IMultiCriteria Add<T>(ICriteria criteria)
    {
      this.criteriaQueries.Add(criteria);
      this.resultCollectionGenericType.Add(typeof (T));
      return (IMultiCriteria) this;
    }

    public IMultiCriteria Add<T>(string key, ICriteria criteria)
    {
      this.ThrowIfKeyAlreadyExists(key);
      this.criteriaQueries.Add(criteria);
      this.criteriaResultPositions.Add(key, this.criteriaQueries.Count - 1);
      this.resultCollectionGenericType.Add(typeof (T));
      return (IMultiCriteria) this;
    }

    public IMultiCriteria Add<T>(DetachedCriteria detachedCriteria)
    {
      this.criteriaQueries.Add(detachedCriteria.GetExecutableCriteria((ISession) this.session));
      this.resultCollectionGenericType.Add(typeof (T));
      return (IMultiCriteria) this;
    }

    public IMultiCriteria Add<T>(string key, DetachedCriteria detachedCriteria)
    {
      this.ThrowIfKeyAlreadyExists(key);
      this.criteriaQueries.Add(detachedCriteria.GetExecutableCriteria((ISession) this.session));
      this.criteriaResultPositions.Add(key, this.criteriaQueries.Count - 1);
      this.resultCollectionGenericType.Add(typeof (T));
      return (IMultiCriteria) this;
    }

    public IMultiCriteria Add(System.Type resultGenericListType, IQueryOver queryOver)
    {
      return this.Add(resultGenericListType, queryOver.RootCriteria);
    }

    public IMultiCriteria Add<T>(IQueryOver<T> queryOver) => this.Add<T>(queryOver.RootCriteria);

    public IMultiCriteria Add<U>(IQueryOver queryOver) => this.Add<U>(queryOver.RootCriteria);

    public IMultiCriteria Add<T>(string key, IQueryOver<T> queryOver)
    {
      return this.Add<T>(key, queryOver.RootCriteria);
    }

    public IMultiCriteria Add<U>(string key, IQueryOver queryOver)
    {
      return this.Add<U>(key, queryOver.RootCriteria);
    }

    public IMultiCriteria SetCacheable(bool cachable)
    {
      this.isCacheable = cachable;
      return (IMultiCriteria) this;
    }

    public IMultiCriteria ForceCacheRefresh(bool forceRefresh)
    {
      this.forceCacheRefresh = forceRefresh;
      return (IMultiCriteria) this;
    }

    public IMultiCriteria SetResultTransformer(IResultTransformer resultTransformer)
    {
      this.resultTransformer = resultTransformer;
      return (IMultiCriteria) this;
    }

    public object GetResult(string key)
    {
      if (this.criteriaResults == null)
        this.List();
      if (!this.criteriaResultPositions.ContainsKey(key))
        throw new InvalidOperationException(string.Format("The key '{0}' is unknown", (object) key));
      return this.criteriaResults[this.criteriaResultPositions[key]];
    }

    public IMultiCriteria SetCacheRegion(string cacheRegion)
    {
      this.cacheRegion = cacheRegion;
      return (IMultiCriteria) this;
    }

    private QueryParameters CreateCombinedQueryParameters()
    {
      QueryParameters combinedQueryParameters = new QueryParameters();
      combinedQueryParameters.ForceCacheRefresh = this.forceCacheRefresh;
      combinedQueryParameters.NamedParameters = (IDictionary<string, TypedValue>) new Dictionary<string, TypedValue>();
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      int num = 0;
      foreach (QueryParameters parameter in this.parameters)
      {
        foreach (KeyValuePair<string, TypedValue> namedParameter in (IEnumerable<KeyValuePair<string, TypedValue>>) parameter.NamedParameters)
          combinedQueryParameters.NamedParameters.Add(namedParameter.Key + (object) num, namedParameter.Value);
        ++num;
        arrayList1.AddRange((ICollection) parameter.PositionalParameterTypes);
        arrayList2.AddRange((ICollection) parameter.PositionalParameterValues);
      }
      combinedQueryParameters.PositionalParameterTypes = (IType[]) arrayList1.ToArray(typeof (IType));
      combinedQueryParameters.PositionalParameterValues = (object[]) arrayList2.ToArray(typeof (object));
      return combinedQueryParameters;
    }

    private void ThrowIfKeyAlreadyExists(string key)
    {
      if (this.criteriaResultPositions.ContainsKey(key))
        throw new InvalidOperationException(string.Format("The key '{0}' already exists", (object) key));
    }
  }
}
