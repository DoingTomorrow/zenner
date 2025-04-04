// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.MultiQueryImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using Iesi.Collections.Generic;
using NHibernate.Cache;
using NHibernate.Driver;
using NHibernate.Engine;
using NHibernate.Engine.Query.Sql;
using NHibernate.Exceptions;
using NHibernate.Hql;
using NHibernate.Loader.Custom;
using NHibernate.Loader.Custom.Sql;
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
  public class MultiQueryImpl : IMultiQuery
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (MultiQueryImpl));
    private readonly System.Collections.Generic.List<IQuery> queries = new System.Collections.Generic.List<IQuery>();
    private readonly System.Collections.Generic.List<MultiQueryImpl.ITranslator> translators = new System.Collections.Generic.List<MultiQueryImpl.ITranslator>();
    private readonly System.Collections.Generic.List<int> translatorQueryMap = new System.Collections.Generic.List<int>();
    private readonly IList<System.Type> resultCollectionGenericType = (IList<System.Type>) new System.Collections.Generic.List<System.Type>();
    private readonly System.Collections.Generic.List<QueryParameters> parameters = new System.Collections.Generic.List<QueryParameters>();
    private IList queryResults;
    private readonly Dictionary<string, int> queryResultPositions = new Dictionary<string, int>();
    private string cacheRegion;
    private int commandTimeout = RowSelection.NoValue;
    private bool isCacheable;
    private readonly ISessionImplementor session;
    private IResultTransformer resultTransformer;
    private readonly NHibernate.Dialect.Dialect dialect;
    private bool forceCacheRefresh;
    private QueryParameters combinedParameters;
    private FlushMode flushMode = FlushMode.Unspecified;
    private FlushMode sessionFlushMode = FlushMode.Unspecified;
    private readonly IResultSetsCommand resultSetsCommand;

    public MultiQueryImpl(ISessionImplementor session)
    {
      IDriver driver = session.Factory.ConnectionProvider.Driver;
      this.dialect = session.Factory.Dialect;
      this.resultSetsCommand = driver.GetResultSetsCommand(session);
      this.session = session;
    }

    public IMultiQuery SetResultTransformer(IResultTransformer transformer)
    {
      this.resultTransformer = transformer;
      return (IMultiQuery) this;
    }

    public IMultiQuery SetForceCacheRefresh(bool cacheRefresh)
    {
      this.forceCacheRefresh = cacheRefresh;
      return (IMultiQuery) this;
    }

    public IMultiQuery SetTimeout(int timeout)
    {
      this.commandTimeout = timeout;
      return (IMultiQuery) this;
    }

    public IMultiQuery SetParameter(string name, object val, IType type)
    {
      foreach (IQuery query in this.queries)
        query.SetParameter(name, val, type);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetParameter(string name, object val)
    {
      foreach (IQuery query in this.queries)
        query.SetParameter(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetParameterList(string name, ICollection vals, IType type)
    {
      foreach (IQuery query in this.queries)
        query.SetParameterList(name, (IEnumerable) vals, type);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetParameterList(string name, ICollection vals)
    {
      foreach (IQuery query in this.queries)
        query.SetParameterList(name, (IEnumerable) vals);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetAnsiString(string name, string val)
    {
      foreach (IQuery query in this.queries)
        query.SetAnsiString(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetBinary(string name, byte[] val)
    {
      foreach (IQuery query in this.queries)
        query.SetBinary(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetBoolean(string name, bool val)
    {
      foreach (IQuery query in this.queries)
        query.SetBoolean(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetByte(string name, byte val)
    {
      foreach (IQuery query in this.queries)
        query.SetByte(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetCharacter(string name, char val)
    {
      foreach (IQuery query in this.queries)
        query.SetCharacter(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetDateTime(string name, DateTime val)
    {
      foreach (IQuery query in this.queries)
        query.SetDateTime(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetDateTime2(string name, DateTime val)
    {
      foreach (IQuery query in this.queries)
        query.SetParameter(name, (object) val, (IType) NHibernateUtil.DateTime2);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetTimeSpan(string name, TimeSpan val)
    {
      foreach (IQuery query in this.queries)
        query.SetParameter(name, (object) val, (IType) NHibernateUtil.TimeSpan);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetTimeAsTimeSpan(string name, TimeSpan val)
    {
      foreach (IQuery query in this.queries)
        query.SetParameter(name, (object) val, (IType) NHibernateUtil.TimeAsTimeSpan);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetDateTimeOffset(string name, DateTimeOffset val)
    {
      foreach (IQuery query in this.queries)
        query.SetParameter(name, (object) val, (IType) NHibernateUtil.DateTimeOffset);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetDecimal(string name, Decimal val)
    {
      foreach (IQuery query in this.queries)
        query.SetDecimal(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetDouble(string name, double val)
    {
      foreach (IQuery query in this.queries)
        query.SetDouble(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetEntity(string name, object val)
    {
      foreach (IQuery query in this.queries)
        query.SetEntity(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetEnum(string name, Enum val)
    {
      foreach (IQuery query in this.queries)
        query.SetEnum(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetInt16(string name, short val)
    {
      foreach (IQuery query in this.queries)
        query.SetInt16(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetInt32(string name, int val)
    {
      foreach (IQuery query in this.queries)
        query.SetInt32(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetInt64(string name, long val)
    {
      foreach (IQuery query in this.queries)
        query.SetInt64(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetSingle(string name, float val)
    {
      foreach (IQuery query in this.queries)
        query.SetSingle(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetString(string name, string val)
    {
      foreach (IQuery query in this.queries)
        query.SetString(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetGuid(string name, Guid val)
    {
      foreach (IQuery query in this.queries)
        query.SetGuid(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetTime(string name, DateTime val)
    {
      foreach (IQuery query in this.queries)
        query.SetTime(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery SetTimestamp(string name, DateTime val)
    {
      foreach (IQuery query in this.queries)
        query.SetTimestamp(name, val);
      return (IMultiQuery) this;
    }

    public IMultiQuery AddNamedQuery<T>(string key, string namedQuery)
    {
      this.ThrowIfKeyAlreadyExists(key);
      return this.Add<T>(key, this.session.GetNamedQuery(namedQuery));
    }

    public IMultiQuery Add(System.Type resultGenericListType, IQuery query)
    {
      this.AddQueryForLaterExecutionAndReturnIndexOfQuery(resultGenericListType, query);
      return (IMultiQuery) this;
    }

    public IMultiQuery Add(string key, IQuery query) => this.Add<object>(key, query);

    public IMultiQuery Add(IQuery query) => this.Add<object>(query);

    public IMultiQuery Add(string key, string hql) => this.Add<object>(key, hql);

    public IMultiQuery Add(string hql) => this.Add<object>(hql);

    public IMultiQuery AddNamedQuery(string queryName) => this.AddNamedQuery<object>(queryName);

    public IMultiQuery AddNamedQuery(string key, string namedQuery)
    {
      return this.AddNamedQuery<object>(key, namedQuery);
    }

    public IMultiQuery Add<T>(IQuery query)
    {
      this.AddQueryForLaterExecutionAndReturnIndexOfQuery(typeof (T), query);
      return (IMultiQuery) this;
    }

    public IMultiQuery Add<T>(string key, IQuery query)
    {
      this.ThrowIfKeyAlreadyExists(key);
      this.queryResultPositions.Add(key, this.AddQueryForLaterExecutionAndReturnIndexOfQuery(typeof (T), query));
      return (IMultiQuery) this;
    }

    public IMultiQuery Add<T>(string hql)
    {
      return this.Add<T>(((ISession) this.session).CreateQuery(hql));
    }

    public IMultiQuery Add<T>(string key, string hql)
    {
      this.ThrowIfKeyAlreadyExists(key);
      return this.Add<T>(key, ((ISession) this.session).CreateQuery(hql));
    }

    public IMultiQuery AddNamedQuery<T>(string queryName)
    {
      return this.Add<T>(this.session.GetNamedQuery(queryName));
    }

    public IMultiQuery SetCacheable(bool cacheable)
    {
      this.isCacheable = cacheable;
      return (IMultiQuery) this;
    }

    public IMultiQuery SetCacheRegion(string region)
    {
      this.cacheRegion = region;
      return (IMultiQuery) this;
    }

    public IList List()
    {
      using (new SessionIdLoggingContext(this.session.SessionId))
      {
        bool flag = this.session.Factory.Settings.IsQueryCacheEnabled && this.isCacheable;
        this.combinedParameters = this.CreateCombinedQueryParameters();
        if (MultiQueryImpl.log.IsDebugEnabled)
        {
          MultiQueryImpl.log.DebugFormat("Multi query with {0} queries.", (object) this.queries.Count);
          for (int index = 0; index < this.queries.Count; ++index)
            MultiQueryImpl.log.DebugFormat("Query #{0}: {1}", (object) index, (object) this.queries[index]);
        }
        try
        {
          this.Before();
          return flag ? this.ListUsingQueryCache() : this.ListIgnoreQueryCache();
        }
        finally
        {
          this.After();
        }
      }
    }

    public IMultiQuery SetFlushMode(FlushMode mode)
    {
      this.flushMode = mode;
      return (IMultiQuery) this;
    }

    protected void Before()
    {
      if (this.flushMode == FlushMode.Unspecified)
        return;
      this.sessionFlushMode = this.session.FlushMode;
      this.session.FlushMode = this.flushMode;
    }

    protected void After()
    {
      if (this.sessionFlushMode == FlushMode.Unspecified)
        return;
      this.session.FlushMode = this.sessionFlushMode;
      this.sessionFlushMode = FlushMode.Unspecified;
    }

    protected virtual IList GetResultList(IList results)
    {
      ArrayList resultList = new ArrayList(this.resultCollectionGenericType.Count);
      for (int index = 0; index < this.queries.Count; ++index)
      {
        if (this.resultCollectionGenericType[index] == typeof (object))
          resultList.Add((object) new ArrayList());
        else
          resultList.Add(Activator.CreateInstance(typeof (System.Collections.Generic.List<>).MakeGenericType(this.resultCollectionGenericType[index])));
      }
      HolderInstantiator holderInstatiator = this.GetMultiQueryHolderInstatiator();
      for (int index = 0; index < results.Count; ++index)
      {
        IList transformedResults = this.GetTransformedResults(this.translators[index].Loader.GetResultList((IList) results[index], this.Parameters[index].ResultTransformer), holderInstatiator);
        int translatorQuery = this.translatorQueryMap[index];
        ArrayHelper.AddAll((IList) resultList[translatorQuery], transformedResults);
      }
      return (IList) resultList;
    }

    private IList GetTransformedResults(IList source, HolderInstantiator holderInstantiator)
    {
      if (!holderInstantiator.IsRequired)
        return source;
      for (int index = 0; index < source.Count; ++index)
      {
        if (!(source[index] is object[] objArray))
          objArray = new object[1]{ source[index] };
        object[] row = objArray;
        source[index] = holderInstantiator.Instantiate(row);
      }
      return holderInstantiator.ResultTransformer.TransformList(source);
    }

    private HolderInstantiator GetMultiQueryHolderInstatiator()
    {
      return !this.HasMultiQueryResultTransformer() ? HolderInstantiator.NoopInstantiator : new HolderInstantiator(this.resultTransformer, (string[]) null);
    }

    private bool HasMultiQueryResultTransformer() => this.resultTransformer != null;

    protected ArrayList DoList()
    {
      bool statisticsEnabled = this.session.Factory.Statistics.IsStatisticsEnabled;
      Stopwatch stopwatch = new Stopwatch();
      if (statisticsEnabled)
        stopwatch.Start();
      int rows = 0;
      ArrayList arrayList = new ArrayList();
      ArrayList[] arrayListArray = new ArrayList[this.Translators.Count];
      System.Collections.Generic.List<EntityKey[]>[] entityKeyArrayListArray = new System.Collections.Generic.List<EntityKey[]>[this.Translators.Count];
      bool[] flagArray = new bool[this.Translators.Count];
      try
      {
        using (IDataReader reader = this.resultSetsCommand.GetReader(this.commandTimeout != RowSelection.NoValue ? new int?(this.commandTimeout) : new int?()))
        {
          if (MultiQueryImpl.log.IsDebugEnabled)
            MultiQueryImpl.log.DebugFormat("Executing {0} queries", (object) this.translators.Count);
          for (int index = 0; index < this.translators.Count; ++index)
          {
            MultiQueryImpl.ITranslator translator = this.Translators[index];
            QueryParameters parameter = this.Parameters[index];
            int length = translator.Loader.EntityPersisters.Length;
            arrayListArray[index] = length > 0 ? new ArrayList() : (ArrayList) null;
            RowSelection rowSelection = parameter.RowSelection;
            int num1 = NHibernate.Loader.Loader.HasMaxRows(rowSelection) ? rowSelection.MaxRows : int.MaxValue;
            if (!this.dialect.SupportsLimitOffset || !translator.Loader.UseLimit(rowSelection, this.dialect))
              NHibernate.Loader.Loader.Advance(reader, rowSelection);
            LockMode[] lockModes = translator.Loader.GetLockModes(parameter.LockModes);
            EntityKey optionalObjectKey = NHibernate.Loader.Loader.GetOptionalObjectKey(parameter, this.session);
            flagArray[index] = translator.Loader.IsSubselectLoadingEnabled;
            entityKeyArrayListArray[index] = flagArray[index] ? new System.Collections.Generic.List<EntityKey[]>() : (System.Collections.Generic.List<EntityKey[]>) null;
            translator.Loader.HandleEmptyCollections(parameter.CollectionKeys, (object) reader, this.session);
            EntityKey[] keys = new EntityKey[length];
            if (MultiQueryImpl.log.IsDebugEnabled)
              MultiQueryImpl.log.Debug((object) "processing result set");
            IList list = (IList) new ArrayList();
            int num2;
            for (num2 = 0; num2 < num1 && reader.Read(); ++num2)
            {
              if (MultiQueryImpl.log.IsDebugEnabled)
                MultiQueryImpl.log.Debug((object) ("result set row: " + (object) num2));
              ++rows;
              object rowFromResultSet = translator.Loader.GetRowFromResultSet(reader, this.session, parameter, lockModes, optionalObjectKey, (IList) arrayListArray[index], keys, true);
              list.Add(rowFromResultSet);
              if (flagArray[index])
              {
                entityKeyArrayListArray[index].Add(keys);
                keys = new EntityKey[length];
              }
            }
            if (MultiQueryImpl.log.IsDebugEnabled)
              MultiQueryImpl.log.Debug((object) string.Format("done processing result set ({0} rows)", (object) num2));
            arrayList.Add((object) list);
            if (MultiQueryImpl.log.IsDebugEnabled)
              MultiQueryImpl.log.DebugFormat("Query {0} returned {1} results", (object) index, (object) list.Count);
            reader.NextResult();
          }
          for (int index = 0; index < this.translators.Count; ++index)
          {
            MultiQueryImpl.ITranslator translator = this.translators[index];
            QueryParameters parameter = this.parameters[index];
            translator.Loader.InitializeEntitiesAndCollections((IList) arrayListArray[index], (object) reader, this.session, false);
            if (flagArray[index])
              translator.Loader.CreateSubselects((IList<EntityKey[]>) entityKeyArrayListArray[index], parameter, this.session);
          }
        }
      }
      catch (Exception ex)
      {
        string message = string.Format("Failed to execute multi query: [{0}]", (object) this.resultSetsCommand.Sql);
        MultiQueryImpl.log.Error((object) message, ex);
        throw ADOExceptionHelper.Convert(this.session.Factory.SQLExceptionConverter, ex, "Failed to execute multi query", this.resultSetsCommand.Sql);
      }
      if (statisticsEnabled)
      {
        stopwatch.Stop();
        this.session.Factory.StatisticsImplementor.QueryExecuted(string.Format("{0} queries (MultiQuery)", (object) this.translators.Count), rows, stopwatch.Elapsed);
      }
      return arrayList;
    }

    protected SqlString SqlString
    {
      get
      {
        if (!this.resultSetsCommand.HasQueries)
          this.AggregateQueriesInformation();
        return this.resultSetsCommand.Sql;
      }
    }

    private void AggregateQueriesInformation()
    {
      int num = 0;
      foreach (AbstractQueryImpl query in this.queries)
      {
        QueryParameters queryParameters = query.GetQueryParameters();
        queryParameters.ValidateParameters();
        query.VerifyParameters();
        foreach (MultiQueryImpl.ITranslator translator in this.GetTranslators(query, queryParameters))
        {
          this.translators.Add(translator);
          this.translatorQueryMap.Add(num);
          this.parameters.Add(queryParameters);
          this.resultSetsCommand.Append(translator.Loader.CreateSqlCommand(queryParameters, this.session));
        }
        ++num;
      }
    }

    private IEnumerable<MultiQueryImpl.ITranslator> GetTranslators(
      AbstractQueryImpl query,
      QueryParameters queryParameters)
    {
      if (query is ExpressionQueryImpl expressionQuery)
      {
        if (!(this.session is AbstractSessionImpl abstractSessionImpl))
          throw new HibernateException("To use LINQ queries with MultiQuery, the session must inherit from AbstractSessionImpl.");
        IQueryExpression queryExpression = expressionQuery.ExpandParameters(queryParameters.NamedParameters);
        foreach (IQueryTranslator query1 in abstractSessionImpl.GetQueries(queryExpression, false))
          yield return (MultiQueryImpl.ITranslator) new MultiQueryImpl.HqlTranslatorWrapper(query1);
      }
      else
      {
        string queryString = query.ExpandParameterLists(queryParameters.NamedParameters);
        if (query is ISQLQuery sqlQuery)
        {
          yield return (MultiQueryImpl.ITranslator) new MultiQueryImpl.SqlTranslator(sqlQuery, this.session.Factory);
        }
        else
        {
          foreach (IQueryTranslator query2 in this.session.GetQueries(queryString, false))
            yield return (MultiQueryImpl.ITranslator) new MultiQueryImpl.HqlTranslatorWrapper(query2);
        }
      }
    }

    public object GetResult(string key)
    {
      if (this.queryResults == null)
        this.queryResults = this.List();
      if (!this.queryResultPositions.ContainsKey(key))
        throw new InvalidOperationException(string.Format("The key '{0}' is unknown", (object) key));
      return this.queryResults[this.queryResultPositions[key]];
    }

    public override string ToString() => "Multi Query: [" + (object) this.SqlString + "]";

    private IList ListIgnoreQueryCache() => this.GetResultList((IList) this.DoList());

    private IList ListUsingQueryCache()
    {
      IQueryCache queryCache = this.session.Factory.GetQueryCache(this.cacheRegion);
      ISet filterKeys = FilterKey.CreateFilterKeys(this.session.EnabledFilters, this.session.EntityMode);
      ISet<string> querySpaces = (ISet<string>) new HashedSet<string>();
      System.Collections.Generic.List<IType[]> assemblers = new System.Collections.Generic.List<IType[]>(this.Translators.Count);
      for (int index = 0; index < this.Translators.Count; ++index)
      {
        MultiQueryImpl.ITranslator translator = this.Translators[index];
        querySpaces.AddAll(translator.QuerySpaces);
        assemblers.Add(translator.ReturnTypes);
      }
      int[] firstRows = new int[this.Parameters.Count];
      int[] maxRows = new int[this.Parameters.Count];
      for (int index = 0; index < this.Parameters.Count; ++index)
      {
        RowSelection rowSelection = this.Parameters[index].RowSelection;
        firstRows[index] = rowSelection.FirstRow;
        maxRows[index] = rowSelection.MaxRows;
      }
      MultipleQueriesCacheAssembler queriesCacheAssembler = new MultipleQueriesCacheAssembler((IList) assemblers);
      QueryKey key = new QueryKey(this.session.Factory, this.SqlString, this.combinedParameters, filterKeys).SetFirstRows(firstRows).SetMaxRows(maxRows);
      IList results = queriesCacheAssembler.GetResultFromQueryCache(this.session, this.combinedParameters, querySpaces, queryCache, key);
      if (results == null)
      {
        MultiQueryImpl.log.Debug((object) "Cache miss for multi query");
        ArrayList arrayList = this.DoList();
        queryCache.Put(key, new ICacheAssembler[1]
        {
          (ICacheAssembler) queriesCacheAssembler
        }, (IList) new object[1]{ (object) arrayList }, false, this.session);
        results = (IList) arrayList;
      }
      return this.GetResultList(results);
    }

    private IList<MultiQueryImpl.ITranslator> Translators
    {
      get
      {
        if (!this.resultSetsCommand.HasQueries)
          this.AggregateQueriesInformation();
        return (IList<MultiQueryImpl.ITranslator>) this.translators;
      }
    }

    private QueryParameters CreateCombinedQueryParameters()
    {
      QueryParameters combinedQueryParameters = new QueryParameters();
      combinedQueryParameters.ForceCacheRefresh = this.forceCacheRefresh;
      combinedQueryParameters.NamedParameters = (IDictionary<string, TypedValue>) new Dictionary<string, TypedValue>();
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      int num = 0;
      foreach (QueryParameters parameter in (IEnumerable<QueryParameters>) this.Parameters)
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

    private IList<QueryParameters> Parameters
    {
      get
      {
        if (!this.resultSetsCommand.HasQueries)
          this.AggregateQueriesInformation();
        return (IList<QueryParameters>) this.parameters;
      }
    }

    private void ThrowIfKeyAlreadyExists(string key)
    {
      if (this.queryResultPositions.ContainsKey(key))
        throw new InvalidOperationException(string.Format("The key '{0}' already exists", (object) key));
    }

    private int AddQueryForLaterExecutionAndReturnIndexOfQuery(
      System.Type resultGenericListType,
      IQuery query)
    {
      ((AbstractQueryImpl) query).SetIgnoreUknownNamedParameters(true);
      this.queries.Add(query);
      this.resultCollectionGenericType.Add(resultGenericListType);
      return this.queries.Count - 1;
    }

    private interface ITranslator
    {
      NHibernate.Loader.Loader Loader { get; }

      IType[] ReturnTypes { get; }

      string[] ReturnAliases { get; }

      ICollection<string> QuerySpaces { get; }
    }

    private class HqlTranslatorWrapper : MultiQueryImpl.ITranslator
    {
      private readonly IQueryTranslator innerTranslator;

      public HqlTranslatorWrapper(IQueryTranslator translator) => this.innerTranslator = translator;

      public NHibernate.Loader.Loader Loader => this.innerTranslator.Loader;

      public IType[] ReturnTypes => this.innerTranslator.ActualReturnTypes;

      public ICollection<string> QuerySpaces
      {
        get => (ICollection<string>) this.innerTranslator.QuerySpaces;
      }

      public string[] ReturnAliases => this.innerTranslator.ReturnAliases;
    }

    private class SqlTranslator : MultiQueryImpl.ITranslator
    {
      private readonly CustomLoader loader;

      public SqlTranslator(ISQLQuery sqlQuery, ISessionFactoryImplementor sessionFactory)
      {
        SqlQueryImpl sqlQueryImpl = (SqlQueryImpl) sqlQuery;
        NativeSQLQuerySpecification querySpecification = sqlQueryImpl.GenerateQuerySpecification(sqlQueryImpl.NamedParams);
        this.loader = new CustomLoader((ICustomQuery) new SQLCustomQuery(querySpecification.SqlQueryReturns, querySpecification.QueryString, (ICollection<string>) querySpecification.QuerySpaces, sessionFactory), sessionFactory);
      }

      public IType[] ReturnTypes => this.loader.ResultTypes;

      public NHibernate.Loader.Loader Loader => (NHibernate.Loader.Loader) this.loader;

      public ICollection<string> QuerySpaces => (ICollection<string>) this.loader.QuerySpaces;

      public string[] ReturnAliases => this.loader.ReturnAliases;
    }
  }
}
