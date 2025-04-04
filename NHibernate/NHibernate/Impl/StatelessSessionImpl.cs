// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.StatelessSessionImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet;
using NHibernate.Cache;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Engine.Query.Sql;
using NHibernate.Event;
using NHibernate.Hql;
using NHibernate.Id;
using NHibernate.Loader.Criteria;
using NHibernate.Loader.Custom;
using NHibernate.Loader.Custom.Sql;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Impl
{
  [Serializable]
  public class StatelessSessionImpl : AbstractSessionImpl, IStatelessSession, IDisposable
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (StatelessSessionImpl));
    [NonSerialized]
    private readonly ConnectionManager connectionManager;
    [NonSerialized]
    private readonly StatefulPersistenceContext temporaryPersistenceContext;
    private bool _isAlreadyDisposed;

    internal StatelessSessionImpl(IDbConnection connection, SessionFactoryImpl factory)
      : base((ISessionFactoryImplementor) factory)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.temporaryPersistenceContext = new StatefulPersistenceContext((ISessionImplementor) this);
        this.connectionManager = new ConnectionManager((ISessionImplementor) this, connection, ConnectionReleaseMode.AfterTransaction, (IInterceptor) new EmptyInterceptor());
        if (StatelessSessionImpl.log.IsDebugEnabled)
          StatelessSessionImpl.log.DebugFormat("[session-id={0}] opened session for session factory: [{1}/{2}]", (object) this.SessionId, (object) factory.Name, (object) factory.Uuid);
        this.CheckAndUpdateSessionStatus();
      }
    }

    public override void InitializeCollection(IPersistentCollection collection, bool writing)
    {
      CollectionEntry collectionEntry = !this.temporaryPersistenceContext.IsLoadFinished ? this.temporaryPersistenceContext.GetCollectionEntry(collection) : throw new SessionException("Collections cannot be fetched by a stateless session. You can eager load it through specific query.");
      if (collection.WasInitialized)
        return;
      collectionEntry.LoadedPersister.Initialize(collectionEntry.LoadedKey, (ISessionImplementor) this);
    }

    public override object InternalLoad(string entityName, object id, bool eager, bool isNullable)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        IEntityPersister entityPersister = this.Factory.GetEntityPersister(entityName);
        object entity = this.temporaryPersistenceContext.GetEntity(new EntityKey(id, entityPersister, EntityMode.Poco));
        if (entity != null)
          return entity;
        return !eager && entityPersister.HasProxy ? entityPersister.CreateProxy(id, (ISessionImplementor) this) : this.Get(entityName, id);
      }
    }

    public override object ImmediateLoad(string entityName, object id)
    {
      throw new SessionException("proxies cannot be fetched by a stateless session");
    }

    public override long Timestamp => throw new NotSupportedException();

    public override IBatcher Batcher
    {
      get
      {
        this.CheckAndUpdateSessionStatus();
        return this.connectionManager.Batcher;
      }
    }

    public override void CloseSessionFromDistributedTransaction() => this.Dispose(true);

    public override IList List(string query, QueryParameters parameters)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        IList results = (IList) new ArrayList();
        this.List(query, parameters, results);
        return results;
      }
    }

    public override void List(string query, QueryParameters queryParameters, IList results)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        queryParameters.ValidateParameters();
        IQueryPlan hqlQueryPlan = this.GetHQLQueryPlan(query, false);
        bool success = false;
        try
        {
          hqlQueryPlan.PerformList(queryParameters, (ISessionImplementor) this, results);
          success = true;
        }
        catch (HibernateException ex)
        {
          throw;
        }
        catch (Exception ex)
        {
          throw this.Convert(ex, "Could not execute query");
        }
        finally
        {
          this.AfterOperation(success);
        }
        this.temporaryPersistenceContext.Clear();
      }
    }

    public override void List(
      IQueryExpression queryExpression,
      QueryParameters queryParameters,
      IList results)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        queryParameters.ValidateParameters();
        IQueryExpressionPlan hqlQueryPlan = this.GetHQLQueryPlan(queryExpression, false);
        bool success = false;
        try
        {
          hqlQueryPlan.PerformList(queryParameters, (ISessionImplementor) this, results);
          success = true;
        }
        catch (HibernateException ex)
        {
          throw;
        }
        catch (Exception ex)
        {
          throw this.Convert(ex, "Could not execute query");
        }
        finally
        {
          this.AfterOperation(success);
        }
        this.temporaryPersistenceContext.Clear();
      }
    }

    public override IList<T> List<T>(string query, QueryParameters queryParameters)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        System.Collections.Generic.List<T> results = new System.Collections.Generic.List<T>();
        this.List(query, queryParameters, (IList) results);
        return (IList<T>) results;
      }
    }

    public override IList<T> List<T>(CriteriaImpl criteria)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        System.Collections.Generic.List<T> results = new System.Collections.Generic.List<T>();
        this.List(criteria, (IList) results);
        return (IList<T>) results;
      }
    }

    public override void List(CriteriaImpl criteria, IList results)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        string[] implementors = this.Factory.GetImplementors(criteria.EntityOrClassName);
        int length = implementors.Length;
        CriteriaLoader[] criteriaLoaderArray = new CriteriaLoader[length];
        for (int index = 0; index < length; ++index)
          criteriaLoaderArray[index] = new CriteriaLoader(this.GetOuterJoinLoadable(implementors[index]), this.Factory, criteria, implementors[index], this.EnabledFilters);
        bool success = false;
        try
        {
          for (int index = length - 1; index >= 0; --index)
            ArrayHelper.AddAll(results, criteriaLoaderArray[index].List((ISessionImplementor) this));
          success = true;
        }
        catch (HibernateException ex)
        {
          throw;
        }
        catch (Exception ex)
        {
          throw this.Convert(ex, "Unable to perform find");
        }
        finally
        {
          this.AfterOperation(success);
        }
        this.temporaryPersistenceContext.Clear();
      }
    }

    private IOuterJoinLoadable GetOuterJoinLoadable(string entityName)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        IEntityPersister entityPersister = this.Factory.GetEntityPersister(entityName);
        return entityPersister is IOuterJoinLoadable ? (IOuterJoinLoadable) entityPersister : throw new MappingException("class persister is not IOuterJoinLoadable: " + entityName);
      }
    }

    public override IList List(CriteriaImpl criteria)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        ArrayList results = new ArrayList();
        this.List(criteria, (IList) results);
        return (IList) results;
      }
    }

    public override IEnumerable Enumerable(string query, QueryParameters parameters)
    {
      throw new NotSupportedException();
    }

    public override IEnumerable<T> Enumerable<T>(string query, QueryParameters queryParameters)
    {
      throw new NotSupportedException();
    }

    public override IList ListFilter(object collection, string filter, QueryParameters parameters)
    {
      throw new NotSupportedException();
    }

    public override IList<T> ListFilter<T>(
      object collection,
      string filter,
      QueryParameters parameters)
    {
      throw new NotSupportedException();
    }

    public override IEnumerable EnumerableFilter(
      object collection,
      string filter,
      QueryParameters parameters)
    {
      throw new NotSupportedException();
    }

    public override IEnumerable<T> EnumerableFilter<T>(
      object collection,
      string filter,
      QueryParameters parameters)
    {
      throw new NotSupportedException();
    }

    public override void AfterTransactionBegin(ITransaction tx)
    {
    }

    public override void BeforeTransactionCompletion(ITransaction tx)
    {
    }

    public override void AfterTransactionCompletion(bool successful, ITransaction tx)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.connectionManager.AfterTransaction();
    }

    public override object GetContextEntityIdentifier(object obj)
    {
      this.CheckAndUpdateSessionStatus();
      return (object) null;
    }

    public override object Instantiate(string clazz, object id)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        return this.Factory.GetEntityPersister(clazz).Instantiate(id, EntityMode.Poco);
      }
    }

    public override IList List(NativeSQLQuerySpecification spec, QueryParameters queryParameters)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        ArrayList results = new ArrayList();
        this.List(spec, queryParameters, (IList) results);
        return (IList) results;
      }
    }

    public override void List(
      NativeSQLQuerySpecification spec,
      QueryParameters queryParameters,
      IList results)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.ListCustomQuery((ICustomQuery) new SQLCustomQuery(spec.SqlQueryReturns, spec.QueryString, (ICollection<string>) spec.QuerySpaces, this.Factory), queryParameters, results);
    }

    public override IList<T> List<T>(
      NativeSQLQuerySpecification spec,
      QueryParameters queryParameters)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        System.Collections.Generic.List<T> results = new System.Collections.Generic.List<T>();
        this.List(spec, queryParameters, (IList) results);
        return (IList<T>) results;
      }
    }

    public override void ListCustomQuery(
      ICustomQuery customQuery,
      QueryParameters queryParameters,
      IList results)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        CustomLoader customLoader = new CustomLoader(customQuery, this.Factory);
        bool success = false;
        try
        {
          ArrayHelper.AddAll(results, customLoader.List((ISessionImplementor) this, queryParameters));
          success = true;
        }
        finally
        {
          this.AfterOperation(success);
        }
        this.temporaryPersistenceContext.Clear();
      }
    }

    public override IList<T> ListCustomQuery<T>(
      ICustomQuery customQuery,
      QueryParameters queryParameters)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        System.Collections.Generic.List<T> results = new System.Collections.Generic.List<T>();
        this.ListCustomQuery(customQuery, queryParameters, (IList) results);
        return (IList<T>) results;
      }
    }

    public override object GetFilterParameterValue(string filterParameterName)
    {
      throw new NotSupportedException();
    }

    public override IType GetFilterParameterType(string filterParameterName)
    {
      throw new NotSupportedException();
    }

    public override IDictionary<string, NHibernate.IFilter> EnabledFilters
    {
      get => (IDictionary<string, NHibernate.IFilter>) new CollectionHelper.EmptyMapClass<string, NHibernate.IFilter>();
    }

    public override IQueryTranslator[] GetQueries(string query, bool scalar)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.Factory.QueryPlanCache.GetHQLQueryPlan(query, scalar, this.EnabledFilters).Translators;
    }

    public override IQueryTranslator[] GetQueries(IQueryExpression query, bool scalar)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.Factory.QueryPlanCache.GetHQLQueryPlan(query, scalar, this.EnabledFilters).Translators;
    }

    public override IInterceptor Interceptor => (IInterceptor) new EmptyInterceptor();

    public override EventListeners Listeners => throw new NotSupportedException();

    public override int DontFlushFromFind => 0;

    public override ConnectionManager ConnectionManager => this.connectionManager;

    public override bool IsEventSource => false;

    public override object GetEntityUsingInterceptor(EntityKey key)
    {
      this.CheckAndUpdateSessionStatus();
      return (object) null;
    }

    public override IPersistenceContext PersistenceContext
    {
      get => (IPersistenceContext) this.temporaryPersistenceContext;
    }

    public override bool IsOpen => !this.IsClosed;

    public override bool IsConnected => this.connectionManager.IsConnected;

    public override FlushMode FlushMode
    {
      get => FlushMode.Commit;
      set => throw new NotSupportedException();
    }

    public override string BestGuessEntityName(object entity)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        if (entity.IsProxy())
          entity = (entity as INHibernateProxy).HibernateLazyInitializer.GetImplementation();
        return this.GuessEntityName(entity);
      }
    }

    public override string GuessEntityName(object entity)
    {
      this.CheckAndUpdateSessionStatus();
      return entity.GetType().FullName;
    }

    public override IDbConnection Connection => this.connectionManager.GetConnection();

    public IStatelessSession SetBatchSize(int batchSize)
    {
      this.Batcher.BatchSize = batchSize;
      return (IStatelessSession) this;
    }

    public override void Flush()
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.ManagedFlush();
    }

    public void ManagedFlush()
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        this.Batcher.ExecuteBatch();
      }
    }

    public override bool TransactionInProgress => this.Transaction.IsActive;

    public ITransaction Transaction => this.connectionManager.Transaction;

    public override CacheMode CacheMode
    {
      get => CacheMode.Ignore;
      set => throw new NotSupportedException();
    }

    public override EntityMode EntityMode => EntityMode.Poco;

    public override string FetchProfile
    {
      get => (string) null;
      set
      {
      }
    }

    public ISessionImplementor GetSessionImplementation() => (ISessionImplementor) this;

    public void Close()
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.ManagedClose();
    }

    public void ManagedClose()
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        if (this.IsClosed)
          throw new SessionException("Session was already closed!");
        this.ConnectionManager.Close();
        this.SetClosed();
      }
    }

    public object Insert(object entity)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        return this.Insert((string) null, entity);
      }
    }

    public object Insert(string entityName, object entity)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        IEntityPersister entityPersister = this.GetEntityPersister(entityName, entity);
        object id = entityPersister.IdentifierGenerator.Generate((ISessionImplementor) this, entity);
        object[] propertyValues = entityPersister.GetPropertyValues(entity, EntityMode.Poco);
        if (entityPersister.IsVersioned)
        {
          object version = propertyValues[entityPersister.VersionProperty];
          if (Versioning.SeedVersion(propertyValues, entityPersister.VersionProperty, entityPersister.VersionType, entityPersister.IsUnsavedVersion(version), (ISessionImplementor) this))
            entityPersister.SetPropertyValues(entity, propertyValues, EntityMode.Poco);
        }
        if (id == IdentifierGeneratorFactory.PostInsertIndicator)
          id = entityPersister.Insert(propertyValues, entity, (ISessionImplementor) this);
        else
          entityPersister.Insert(id, propertyValues, entity, (ISessionImplementor) this);
        entityPersister.SetIdentifier(entity, id, EntityMode.Poco);
        return id;
      }
    }

    public void Update(object entity)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        this.Update((string) null, entity);
      }
    }

    public void Update(string entityName, object entity)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        IEntityPersister entityPersister = this.GetEntityPersister(entityName, entity);
        object identifier = entityPersister.GetIdentifier(entity, EntityMode.Poco);
        object[] propertyValues = entityPersister.GetPropertyValues(entity, EntityMode.Poco);
        object obj;
        if (entityPersister.IsVersioned)
        {
          obj = entityPersister.GetVersion(entity, EntityMode.Poco);
          object version = Versioning.Increment(obj, entityPersister.VersionType, (ISessionImplementor) this);
          Versioning.SetVersion(propertyValues, version, entityPersister);
          entityPersister.SetPropertyValues(entity, propertyValues, EntityMode.Poco);
        }
        else
          obj = (object) null;
        entityPersister.Update(identifier, propertyValues, (int[]) null, false, (object[]) null, obj, entity, (object) null, (ISessionImplementor) this);
      }
    }

    public void Delete(object entity)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        this.Delete((string) null, entity);
      }
    }

    public void Delete(string entityName, object entity)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        IEntityPersister entityPersister = this.GetEntityPersister(entityName, entity);
        object identifier = entityPersister.GetIdentifier(entity, EntityMode.Poco);
        object version = entityPersister.GetVersion(entity, EntityMode.Poco);
        entityPersister.Delete(identifier, version, entity, (ISessionImplementor) this);
      }
    }

    public object Get(string entityName, object id)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.Get(entityName, id, LockMode.None);
    }

    public T Get<T>(object id)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return (T) this.Get(typeof (T), id);
    }

    private object Get(System.Type persistentClass, object id)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.Get(persistentClass.FullName, id);
    }

    public object Get(string entityName, object id, LockMode lockMode)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        object obj = this.Factory.GetEntityPersister(entityName).Load(id, (object) null, lockMode, (ISessionImplementor) this);
        if (this.temporaryPersistenceContext.IsLoadFinished)
          this.temporaryPersistenceContext.Clear();
        return obj;
      }
    }

    public T Get<T>(object id, LockMode lockMode)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return (T) this.Get(typeof (T).FullName, id, lockMode);
    }

    public void Refresh(object entity)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.Refresh(this.BestGuessEntityName(entity), entity, LockMode.None);
    }

    public void Refresh(string entityName, object entity)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.Refresh(entityName, entity, LockMode.None);
    }

    public void Refresh(object entity, LockMode lockMode)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.Refresh(this.BestGuessEntityName(entity), entity, lockMode);
    }

    public void Refresh(string entityName, object entity, LockMode lockMode)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        IEntityPersister entityPersister = this.GetEntityPersister(entityName, entity);
        object identifier = entityPersister.GetIdentifier(entity, this.EntityMode);
        if (StatelessSessionImpl.log.IsDebugEnabled)
          StatelessSessionImpl.log.Debug((object) ("refreshing transient " + MessageHelper.InfoString(entityPersister, identifier, this.Factory)));
        if (entityPersister.HasCache)
        {
          CacheKey key = new CacheKey(identifier, entityPersister.IdentifierType, entityPersister.RootEntityName, this.EntityMode, this.Factory);
          entityPersister.Cache.Remove(key);
        }
        string fetchProfile = this.FetchProfile;
        object o;
        try
        {
          this.FetchProfile = "refresh";
          o = entityPersister.Load(identifier, entity, lockMode, (ISessionImplementor) this);
        }
        finally
        {
          this.FetchProfile = fetchProfile;
        }
        UnresolvableObjectException.ThrowIfNull(o, identifier, entityPersister.EntityName);
      }
    }

    public ICriteria CreateCriteria<T>() where T : class
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.CreateCriteria(typeof (T));
    }

    public ICriteria CreateCriteria<T>(string alias) where T : class
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.CreateCriteria(typeof (T), alias);
    }

    public ICriteria CreateCriteria(System.Type entityType)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        return (ICriteria) new CriteriaImpl(entityType, (ISessionImplementor) this);
      }
    }

    public ICriteria CreateCriteria(System.Type entityType, string alias)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        return (ICriteria) new CriteriaImpl(entityType, alias, (ISessionImplementor) this);
      }
    }

    public ICriteria CreateCriteria(string entityName)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        return (ICriteria) new CriteriaImpl(entityName, (ISessionImplementor) this);
      }
    }

    public ICriteria CreateCriteria(string entityName, string alias)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        return (ICriteria) new CriteriaImpl(entityName, alias, (ISessionImplementor) this);
      }
    }

    public IQueryOver<T, T> QueryOver<T>() where T : class
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        return (IQueryOver<T, T>) new NHibernate.Criterion.QueryOver<T, T>(new CriteriaImpl(typeof (T), (ISessionImplementor) this));
      }
    }

    public IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : class
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        return (IQueryOver<T, T>) new NHibernate.Criterion.QueryOver<T, T>(new CriteriaImpl(typeof (T), ExpressionProcessor.FindMemberExpression(alias.Body), (ISessionImplementor) this));
      }
    }

    public ITransaction BeginTransaction() => this.BeginTransaction(IsolationLevel.Unspecified);

    public ITransaction BeginTransaction(IsolationLevel isolationLevel)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        return this.connectionManager.BeginTransaction(isolationLevel);
      }
    }

    ~StatelessSessionImpl() => this.Dispose(false);

    public void Dispose()
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        StatelessSessionImpl.log.Debug((object) "running IStatelessSession.Dispose()");
        if (this.TransactionContext != null)
          this.TransactionContext.ShouldCloseSessionOnDistributedTransactionCompleted = true;
        else
          this.Dispose(true);
      }
    }

    protected void Dispose(bool isDisposing)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        if (this._isAlreadyDisposed)
          return;
        if (isDisposing && !this.IsClosed)
          this.Close();
        this._isAlreadyDisposed = true;
        GC.SuppressFinalize((object) this);
      }
    }

    public override int ExecuteNativeUpdate(
      NativeSQLQuerySpecification nativeSQLQuerySpecification,
      QueryParameters queryParameters)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        queryParameters.ValidateParameters();
        NativeSQLQueryPlan nativeSqlQueryPlan = this.GetNativeSQLQueryPlan(nativeSQLQuerySpecification);
        bool success = false;
        int num;
        try
        {
          num = nativeSqlQueryPlan.PerformExecuteUpdate(queryParameters, (ISessionImplementor) this);
          success = true;
        }
        finally
        {
          this.AfterOperation(success);
        }
        this.temporaryPersistenceContext.Clear();
        return num;
      }
    }

    public override int ExecuteUpdate(string query, QueryParameters queryParameters)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        queryParameters.ValidateParameters();
        IQueryPlan hqlQueryPlan = this.GetHQLQueryPlan(query, false);
        bool success = false;
        int num;
        try
        {
          num = hqlQueryPlan.PerformExecuteUpdate(queryParameters, (ISessionImplementor) this);
          success = true;
        }
        finally
        {
          this.AfterOperation(success);
        }
        this.temporaryPersistenceContext.Clear();
        return num;
      }
    }

    public override FutureCriteriaBatch FutureCriteriaBatch
    {
      get
      {
        throw new NotSupportedException("future queries are not supported for stateless session");
      }
      internal set
      {
        throw new NotSupportedException("future queries are not supported for stateless session");
      }
    }

    public override FutureQueryBatch FutureQueryBatch
    {
      get
      {
        throw new NotSupportedException("future queries are not supported for stateless session");
      }
      internal set
      {
        throw new NotSupportedException("future queries are not supported for stateless session");
      }
    }

    public override IEntityPersister GetEntityPersister(string entityName, object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        return entityName == null ? this.Factory.GetEntityPersister(this.GuessEntityName(obj)) : this.Factory.GetEntityPersister(entityName).GetSubclassEntityPersister(obj, this.Factory, EntityMode.Poco);
      }
    }
  }
}
