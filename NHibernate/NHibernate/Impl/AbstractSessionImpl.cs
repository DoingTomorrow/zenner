// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.AbstractSessionImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Engine.Query.Sql;
using NHibernate.Event;
using NHibernate.Exceptions;
using NHibernate.Hql;
using NHibernate.Loader.Custom;
using NHibernate.Persister.Entity;
using NHibernate.Transaction;
using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Impl
{
  [Serializable]
  public abstract class AbstractSessionImpl : ISessionImplementor
  {
    [NonSerialized]
    private ISessionFactoryImplementor factory;
    private readonly Guid sessionId = Guid.NewGuid();
    private bool closed;
    private bool isAlreadyDisposed;
    private static readonly IInternalLogger logger = LoggerProvider.LoggerFor(typeof (AbstractSessionImpl));

    public ITransactionContext TransactionContext { get; set; }

    public Guid SessionId => this.sessionId;

    internal AbstractSessionImpl()
    {
    }

    protected internal AbstractSessionImpl(ISessionFactoryImplementor factory)
    {
      this.factory = factory;
    }

    protected internal AbstractSessionImpl(ISessionFactoryImplementor factory, Guid sessionId)
      : this(factory)
    {
      this.sessionId = sessionId;
    }

    public void Initialize()
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.CheckAndUpdateSessionStatus();
    }

    public abstract void InitializeCollection(IPersistentCollection collection, bool writing);

    public abstract object InternalLoad(string entityName, object id, bool eager, bool isNullable);

    public abstract object ImmediateLoad(string entityName, object id);

    public abstract long Timestamp { get; }

    public ISessionFactoryImplementor Factory
    {
      get => this.factory;
      protected set => this.factory = value;
    }

    public abstract EntityMode EntityMode { get; }

    public abstract IBatcher Batcher { get; }

    public abstract void CloseSessionFromDistributedTransaction();

    public abstract IList List(string query, QueryParameters parameters);

    public abstract void List(string query, QueryParameters parameters, IList results);

    public virtual IList List(IQueryExpression queryExpression, QueryParameters parameters)
    {
      IList results = (IList) typeof (System.Collections.Generic.List<>).MakeGenericType(queryExpression.Type).GetConstructor(System.Type.EmptyTypes).Invoke((object[]) null);
      this.List(queryExpression, parameters, results);
      return results;
    }

    public abstract void List(
      IQueryExpression queryExpression,
      QueryParameters queryParameters,
      IList results);

    public abstract IList<T> List<T>(string query, QueryParameters queryParameters);

    public abstract IList<T> List<T>(CriteriaImpl criteria);

    public abstract void List(CriteriaImpl criteria, IList results);

    public abstract IList List(CriteriaImpl criteria);

    public abstract IEnumerable Enumerable(string query, QueryParameters parameters);

    public abstract IEnumerable<T> Enumerable<T>(string query, QueryParameters queryParameters);

    public abstract IList ListFilter(object collection, string filter, QueryParameters parameters);

    public abstract IList<T> ListFilter<T>(
      object collection,
      string filter,
      QueryParameters parameters);

    public abstract IEnumerable EnumerableFilter(
      object collection,
      string filter,
      QueryParameters parameters);

    public abstract IEnumerable<T> EnumerableFilter<T>(
      object collection,
      string filter,
      QueryParameters parameters);

    public abstract IEntityPersister GetEntityPersister(string entityName, object obj);

    public abstract void AfterTransactionBegin(ITransaction tx);

    public abstract void BeforeTransactionCompletion(ITransaction tx);

    public abstract void AfterTransactionCompletion(bool successful, ITransaction tx);

    public abstract object GetContextEntityIdentifier(object obj);

    public abstract object Instantiate(string clazz, object id);

    public abstract IList List(NativeSQLQuerySpecification spec, QueryParameters queryParameters);

    public abstract void List(
      NativeSQLQuerySpecification spec,
      QueryParameters queryParameters,
      IList results);

    public abstract IList<T> List<T>(
      NativeSQLQuerySpecification spec,
      QueryParameters queryParameters);

    public abstract void ListCustomQuery(
      ICustomQuery customQuery,
      QueryParameters queryParameters,
      IList results);

    public abstract IList<T> ListCustomQuery<T>(
      ICustomQuery customQuery,
      QueryParameters queryParameters);

    public abstract object GetFilterParameterValue(string filterParameterName);

    public abstract IType GetFilterParameterType(string filterParameterName);

    public abstract IDictionary<string, NHibernate.IFilter> EnabledFilters { get; }

    public virtual IQuery GetNamedSQLQuery(string name)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        NamedSQLQueryDefinition namedSqlQuery = this.factory.GetNamedSQLQuery(name);
        if (namedSqlQuery == null)
          throw new MappingException("Named SQL query not known: " + name);
        IQuery query = (IQuery) new SqlQueryImpl(namedSqlQuery, (ISessionImplementor) this, this.factory.QueryPlanCache.GetSQLParameterMetadata(namedSqlQuery.QueryString));
        query.SetComment("named native SQL query " + name);
        this.InitQuery(query, (NamedQueryDefinition) namedSqlQuery);
        return query;
      }
    }

    public abstract IQueryTranslator[] GetQueries(string query, bool scalar);

    public virtual IQueryTranslator[] GetQueries(IQueryExpression query, bool scalar)
    {
      throw new NotImplementedException();
    }

    public abstract IInterceptor Interceptor { get; }

    public abstract EventListeners Listeners { get; }

    public abstract int DontFlushFromFind { get; }

    public abstract ConnectionManager ConnectionManager { get; }

    public abstract bool IsEventSource { get; }

    public abstract object GetEntityUsingInterceptor(EntityKey key);

    public abstract IPersistenceContext PersistenceContext { get; }

    public abstract CacheMode CacheMode { get; set; }

    public abstract bool IsOpen { get; }

    public abstract bool IsConnected { get; }

    public abstract FlushMode FlushMode { get; set; }

    public abstract string FetchProfile { get; set; }

    public abstract string BestGuessEntityName(object entity);

    public abstract string GuessEntityName(object entity);

    public abstract IDbConnection Connection { get; }

    public abstract int ExecuteNativeUpdate(
      NativeSQLQuerySpecification specification,
      QueryParameters queryParameters);

    public abstract int ExecuteUpdate(string query, QueryParameters queryParameters);

    public abstract FutureCriteriaBatch FutureCriteriaBatch { get; internal set; }

    public abstract FutureQueryBatch FutureQueryBatch { get; internal set; }

    public virtual IQuery GetNamedQuery(string queryName)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        NamedQueryDefinition nqd = this.factory.GetNamedQuery(queryName);
        IQuery query;
        if (nqd != null)
        {
          string queryString = nqd.QueryString;
          query = (IQuery) new QueryImpl(queryString, nqd.FlushMode, (ISessionImplementor) this, this.GetHQLQueryPlan(queryString, false).ParameterMetadata);
          query.SetComment("named HQL query " + queryName);
        }
        else
        {
          NamedSQLQueryDefinition namedSqlQuery = this.factory.GetNamedSQLQuery(queryName);
          if (namedSqlQuery == null)
            throw new MappingException("Named query not known: " + queryName);
          query = (IQuery) new SqlQueryImpl(namedSqlQuery, (ISessionImplementor) this, this.factory.QueryPlanCache.GetSQLParameterMetadata(namedSqlQuery.QueryString));
          query.SetComment("named native SQL query " + queryName);
          nqd = (NamedQueryDefinition) namedSqlQuery;
        }
        this.InitQuery(query, nqd);
        return query;
      }
    }

    public bool IsClosed => this.closed;

    protected internal virtual void CheckAndUpdateSessionStatus()
    {
      this.ErrorIfClosed();
      this.EnlistInAmbientTransactionIfNeeded();
    }

    protected internal virtual void ErrorIfClosed()
    {
      if (this.IsClosed || this.IsAlreadyDisposed)
        throw new ObjectDisposedException("ISession", "Session is closed!");
    }

    protected bool IsAlreadyDisposed
    {
      get => this.isAlreadyDisposed;
      set => this.isAlreadyDisposed = value;
    }

    public abstract void Flush();

    public abstract bool TransactionInProgress { get; }

    protected internal void SetClosed()
    {
      try
      {
        if (this.TransactionContext != null)
          this.TransactionContext.Dispose();
      }
      catch (Exception ex)
      {
      }
      this.closed = true;
    }

    private void InitQuery(IQuery query, NamedQueryDefinition nqd)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        query.SetCacheable(nqd.IsCacheable);
        query.SetCacheRegion(nqd.CacheRegion);
        if (nqd.Timeout != -1)
          query.SetTimeout(nqd.Timeout);
        if (nqd.FetchSize != -1)
          query.SetFetchSize(nqd.FetchSize);
        if (nqd.CacheMode.HasValue)
          query.SetCacheMode(nqd.CacheMode.Value);
        query.SetReadOnly(nqd.IsReadOnly);
        if (nqd.Comment != null)
          query.SetComment(nqd.Comment);
        query.SetFlushMode(nqd.FlushMode);
      }
    }

    public virtual IQuery CreateQuery(IQueryExpression queryExpression)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        IQueryExpressionPlan hqlQueryPlan = this.GetHQLQueryPlan(queryExpression, false);
        ExpressionQueryImpl query = new ExpressionQueryImpl(hqlQueryPlan.QueryExpression, (ISessionImplementor) this, hqlQueryPlan.ParameterMetadata);
        query.SetComment("[expression]");
        return (IQuery) query;
      }
    }

    public virtual IQuery CreateQuery(string queryString)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        QueryImpl query = new QueryImpl(queryString, (ISessionImplementor) this, this.GetHQLQueryPlan(queryString, false).ParameterMetadata);
        query.SetComment(queryString);
        return (IQuery) query;
      }
    }

    public virtual ISQLQuery CreateSQLQuery(string sql)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        SqlQueryImpl sqlQuery = new SqlQueryImpl(sql, (ISessionImplementor) this, this.factory.QueryPlanCache.GetSQLParameterMetadata(sql));
        sqlQuery.SetComment("dynamic native SQL query");
        return (ISQLQuery) sqlQuery;
      }
    }

    protected internal virtual IQueryPlan GetHQLQueryPlan(string query, bool shallow)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.factory.QueryPlanCache.GetHQLQueryPlan(query, shallow, this.EnabledFilters);
    }

    protected internal virtual IQueryExpressionPlan GetHQLQueryPlan(
      IQueryExpression queryExpression,
      bool shallow)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.factory.QueryPlanCache.GetHQLQueryPlan(queryExpression, shallow, this.EnabledFilters);
    }

    protected internal virtual NativeSQLQueryPlan GetNativeSQLQueryPlan(
      NativeSQLQuerySpecification spec)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.factory.QueryPlanCache.GetNativeSQLQueryPlan(spec);
    }

    protected Exception Convert(Exception sqlException, string message)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return ADOExceptionHelper.Convert(this.factory.SQLExceptionConverter, sqlException, message);
    }

    protected void AfterOperation(bool success)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        if (this.ConnectionManager.IsInActiveTransaction)
          return;
        this.ConnectionManager.AfterNonTransactionalQuery(success);
      }
    }

    protected void EnlistInAmbientTransactionIfNeeded()
    {
      this.factory.TransactionFactory.EnlistInDistributedTransactionIfNeeded((ISessionImplementor) this);
    }
  }
}
