// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.SessionImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using Iesi.Collections.Generic;
using NHibernate.AdoNet;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Engine.Query.Sql;
using NHibernate.Event;
using NHibernate.Hql;
using NHibernate.Intercept;
using NHibernate.Loader.Criteria;
using NHibernate.Loader.Custom;
using NHibernate.Loader.Custom.Sql;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Stat;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Security.Permissions;

#nullable disable
namespace NHibernate.Impl
{
  [Serializable]
  public sealed class SessionImpl : 
    AbstractSessionImpl,
    IEventSource,
    ISessionImplementor,
    ISession,
    IDisposable,
    ISerializable,
    IDeserializationCallback
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (SessionImpl));
    private readonly long timestamp;
    private CacheMode cacheMode = CacheMode.Normal;
    private FlushMode flushMode = FlushMode.Auto;
    private readonly IInterceptor interceptor;
    [NonSerialized]
    private readonly EntityMode entityMode;
    [NonSerialized]
    private FutureCriteriaBatch futureCriteriaBatch;
    [NonSerialized]
    private FutureQueryBatch futureQueryBatch;
    [NonSerialized]
    private readonly EventListeners listeners;
    [NonSerialized]
    private readonly ActionQueue actionQueue;
    private readonly ConnectionManager connectionManager;
    [NonSerialized]
    private int dontFlushFromFind;
    [NonSerialized]
    private readonly IDictionary<string, NHibernate.IFilter> enabledFilters = (IDictionary<string, NHibernate.IFilter>) new Dictionary<string, NHibernate.IFilter>();
    [NonSerialized]
    private readonly System.Collections.Generic.List<string> enabledFilterNames = new System.Collections.Generic.List<string>();
    [NonSerialized]
    private readonly StatefulPersistenceContext persistenceContext;
    [NonSerialized]
    private readonly ISession rootSession;
    [NonSerialized]
    private IDictionary<EntityMode, ISession> childSessionsByEntityMode;
    [NonSerialized]
    private readonly bool flushBeforeCompletionEnabled;
    [NonSerialized]
    private readonly bool autoCloseSessionEnabled;
    [NonSerialized]
    private readonly ConnectionReleaseMode connectionReleaseMode;
    private static readonly object[] NoArgs = new object[0];
    private static readonly IType[] NoTypes = new IType[0];
    private string fetchProfile;

    private SessionImpl(SerializationInfo info, StreamingContext context)
    {
      this.timestamp = info.GetInt64(nameof (timestamp));
      SessionFactoryImpl sessionFactoryImpl = (SessionFactoryImpl) info.GetValue("factory", typeof (SessionFactoryImpl));
      this.Factory = (ISessionFactoryImplementor) sessionFactoryImpl;
      this.listeners = sessionFactoryImpl.EventListeners;
      this.persistenceContext = (StatefulPersistenceContext) info.GetValue(nameof (persistenceContext), typeof (StatefulPersistenceContext));
      this.actionQueue = (ActionQueue) info.GetValue(nameof (actionQueue), typeof (ActionQueue));
      this.flushMode = (FlushMode) info.GetValue(nameof (flushMode), typeof (FlushMode));
      this.cacheMode = (CacheMode) info.GetValue(nameof (cacheMode), typeof (CacheMode));
      this.interceptor = (IInterceptor) info.GetValue(nameof (interceptor), typeof (IInterceptor));
      this.enabledFilters = (IDictionary<string, NHibernate.IFilter>) info.GetValue(nameof (enabledFilters), typeof (Dictionary<string, NHibernate.IFilter>));
      this.enabledFilterNames = (System.Collections.Generic.List<string>) info.GetValue(nameof (enabledFilterNames), typeof (System.Collections.Generic.List<string>));
      this.connectionManager = (ConnectionManager) info.GetValue(nameof (connectionManager), typeof (ConnectionManager));
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
      SessionImpl.log.Debug((object) "writting session to serializer");
      if (!this.connectionManager.IsReadyForSerialization)
        throw new InvalidOperationException("Cannot serialize a Session while connected");
      info.AddValue("factory", (object) this.Factory, typeof (SessionFactoryImpl));
      info.AddValue("persistenceContext", (object) this.persistenceContext, typeof (StatefulPersistenceContext));
      info.AddValue("actionQueue", (object) this.actionQueue, typeof (ActionQueue));
      info.AddValue("timestamp", this.timestamp);
      info.AddValue("flushMode", (object) this.flushMode);
      info.AddValue("cacheMode", (object) this.cacheMode);
      info.AddValue("interceptor", (object) this.interceptor, typeof (IInterceptor));
      info.AddValue("enabledFilters", (object) this.enabledFilters, typeof (IDictionary<string, NHibernate.IFilter>));
      info.AddValue("enabledFilterNames", (object) this.enabledFilterNames, typeof (System.Collections.Generic.List<string>));
      info.AddValue("connectionManager", (object) this.connectionManager, typeof (ConnectionManager));
    }

    void IDeserializationCallback.OnDeserialization(object sender)
    {
      SessionImpl.log.Debug((object) "OnDeserialization of the session.");
      this.persistenceContext.SetSession((ISessionImplementor) this);
      ((IDeserializationCallback) this.enabledFilters).OnDeserialization(sender);
      foreach (string enabledFilterName in this.enabledFilterNames)
        ((FilterImpl) this.enabledFilters[enabledFilterName]).AfterDeserialize(this.Factory.GetFilterDefinition(enabledFilterName));
    }

    internal SessionImpl(
      IDbConnection connection,
      SessionFactoryImpl factory,
      bool autoclose,
      long timestamp,
      IInterceptor interceptor,
      EntityMode entityMode,
      bool flushBeforeCompletionEnabled,
      bool autoCloseSessionEnabled,
      ConnectionReleaseMode connectionReleaseMode)
      : base((ISessionFactoryImplementor) factory)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        if (interceptor == null)
          throw new AssertionFailure("The interceptor can not be null.");
        this.rootSession = (ISession) null;
        this.timestamp = timestamp;
        this.entityMode = entityMode;
        this.interceptor = interceptor;
        this.listeners = factory.EventListeners;
        this.actionQueue = new ActionQueue((ISessionImplementor) this);
        this.persistenceContext = new StatefulPersistenceContext((ISessionImplementor) this);
        this.flushBeforeCompletionEnabled = flushBeforeCompletionEnabled;
        this.autoCloseSessionEnabled = autoCloseSessionEnabled;
        this.connectionReleaseMode = connectionReleaseMode;
        this.connectionManager = new ConnectionManager((ISessionImplementor) this, connection, connectionReleaseMode, interceptor);
        if (factory.Statistics.IsStatisticsEnabled)
          factory.StatisticsImplementor.OpenSession();
        if (SessionImpl.log.IsDebugEnabled)
          SessionImpl.log.DebugFormat("[session-id={0}] opened session at timestamp: {1}, for session factory: [{2}/{3}]", (object) this.SessionId, (object) timestamp, (object) factory.Name, (object) factory.Uuid);
        this.CheckAndUpdateSessionStatus();
      }
    }

    private SessionImpl(SessionImpl parent, EntityMode entityMode)
      : base(parent.Factory, parent.SessionId)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.rootSession = (ISession) parent;
        this.timestamp = parent.timestamp;
        this.connectionManager = parent.connectionManager;
        this.interceptor = parent.interceptor;
        this.listeners = parent.listeners;
        this.actionQueue = new ActionQueue((ISessionImplementor) this);
        this.entityMode = entityMode;
        this.persistenceContext = new StatefulPersistenceContext((ISessionImplementor) this);
        this.flushBeforeCompletionEnabled = false;
        this.autoCloseSessionEnabled = false;
        this.connectionReleaseMode = parent.ConnectionReleaseMode;
        if (this.Factory.Statistics.IsStatisticsEnabled)
          this.Factory.StatisticsImplementor.OpenSession();
        SessionImpl.log.Debug((object) ("opened session [" + (object) entityMode + "]"));
        this.CheckAndUpdateSessionStatus();
      }
    }

    public override FutureCriteriaBatch FutureCriteriaBatch
    {
      get
      {
        if (this.futureCriteriaBatch == null)
          this.futureCriteriaBatch = new FutureCriteriaBatch(this);
        return this.futureCriteriaBatch;
      }
      internal set => this.futureCriteriaBatch = value;
    }

    public override FutureQueryBatch FutureQueryBatch
    {
      get
      {
        if (this.futureQueryBatch == null)
          this.futureQueryBatch = new FutureQueryBatch(this);
        return this.futureQueryBatch;
      }
      internal set => this.futureQueryBatch = value;
    }

    public override IBatcher Batcher
    {
      get
      {
        this.CheckAndUpdateSessionStatus();
        return this.connectionManager.Batcher;
      }
    }

    public override long Timestamp => this.timestamp;

    public ConnectionReleaseMode ConnectionReleaseMode => this.connectionReleaseMode;

    public bool IsAutoCloseSessionEnabled => this.autoCloseSessionEnabled;

    public bool ShouldAutoClose => this.IsAutoCloseSessionEnabled && !this.IsClosed;

    public IDbConnection Close()
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        SessionImpl.log.Debug((object) "closing session");
        if (this.IsClosed)
          throw new SessionException("Session was already closed");
        if (this.Factory.Statistics.IsStatisticsEnabled)
          this.Factory.StatisticsImplementor.CloseSession();
        try
        {
          try
          {
            if (this.childSessionsByEntityMode != null)
            {
              foreach (KeyValuePair<EntityMode, ISession> keyValuePair in (IEnumerable<KeyValuePair<EntityMode, ISession>>) this.childSessionsByEntityMode)
                keyValuePair.Value.Close();
            }
          }
          catch
          {
          }
          return this.rootSession == null ? this.connectionManager.Close() : (IDbConnection) null;
        }
        finally
        {
          this.SetClosed();
          this.Cleanup();
        }
      }
    }

    public override void AfterTransactionCompletion(bool success, ITransaction tx)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        SessionImpl.log.Debug((object) "transaction completion");
        if (this.Factory.Statistics.IsStatisticsEnabled)
          this.Factory.StatisticsImplementor.EndTransaction(success);
        this.connectionManager.AfterTransaction();
        this.persistenceContext.AfterTransactionCompletion();
        this.actionQueue.AfterTransactionCompletion(success);
        if (this.rootSession != null)
          return;
        try
        {
          this.interceptor.AfterTransactionCompletion(tx);
        }
        catch (Exception ex)
        {
          SessionImpl.log.Error((object) "exception in interceptor afterTransactionCompletion()", ex);
        }
      }
    }

    private void Cleanup()
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.persistenceContext.Clear();
    }

    public LockMode GetCurrentLockMode(object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        if (obj == null)
          throw new ArgumentNullException(nameof (obj), "null object passed to GetCurrentLockMode");
        if (obj.IsProxy())
        {
          obj = (obj as INHibernateProxy).HibernateLazyInitializer.GetImplementation((ISessionImplementor) this);
          if (obj == null)
            return LockMode.None;
        }
        EntityEntry entry = this.persistenceContext.GetEntry(obj);
        if (entry == null)
          throw new TransientObjectException("Given object not associated with the session");
        return entry.Status == Status.Loaded ? entry.LockMode : throw new ObjectDeletedException("The given object was deleted", entry.Id, entry.EntityName);
      }
    }

    public override bool IsOpen => !this.IsClosed;

    public object Save(object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.FireSave(new SaveOrUpdateEvent((string) null, obj, (IEventSource) this));
    }

    public object Save(string entityName, object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.FireSave(new SaveOrUpdateEvent(entityName, obj, (IEventSource) this));
    }

    public void Save(object obj, object id)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireSave(new SaveOrUpdateEvent((string) null, obj, id, (IEventSource) this));
    }

    public void Delete(object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireDelete(new DeleteEvent(obj, (IEventSource) this));
    }

    public void Delete(string entityName, object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireDelete(new DeleteEvent(entityName, obj, (IEventSource) this));
    }

    public void Update(object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireUpdate(new SaveOrUpdateEvent((string) null, obj, (IEventSource) this));
    }

    public void Update(string entityName, object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireUpdate(new SaveOrUpdateEvent(entityName, obj, (IEventSource) this));
    }

    public void SaveOrUpdate(object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireSaveOrUpdate(new SaveOrUpdateEvent((string) null, obj, (IEventSource) this));
    }

    public void SaveOrUpdate(string entityName, object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireSaveOrUpdate(new SaveOrUpdateEvent(entityName, obj, (IEventSource) this));
    }

    public void Update(object obj, object id)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireUpdate(new SaveOrUpdateEvent((string) null, obj, id, (IEventSource) this));
    }

    private IList Find(string query, object[] values, IType[] types)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.List(query, new QueryParameters(types, values));
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

    public override IList<T> List<T>(string query, QueryParameters parameters)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        System.Collections.Generic.List<T> results = new System.Collections.Generic.List<T>();
        this.List(query, parameters, (IList) results);
        return (IList<T>) results;
      }
    }

    public override void List(string query, QueryParameters queryParameters, IList results)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        queryParameters.ValidateParameters();
        IQueryPlan hqlQueryPlan = this.GetHQLQueryPlan(query, false);
        this.AutoFlushIfRequired(hqlQueryPlan.QuerySpaces);
        bool success = false;
        ++this.dontFlushFromFind;
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
          --this.dontFlushFromFind;
          this.AfterOperation(success);
        }
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
        this.AutoFlushIfRequired(hqlQueryPlan.QuerySpaces);
        bool success = false;
        ++this.dontFlushFromFind;
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
          --this.dontFlushFromFind;
          this.AfterOperation(success);
        }
      }
    }

    public override IQueryTranslator[] GetQueries(string query, bool scalar)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        IQueryPlan hqlQueryPlan = this.Factory.QueryPlanCache.GetHQLQueryPlan(query, scalar, this.enabledFilters);
        this.AutoFlushIfRequired(hqlQueryPlan.QuerySpaces);
        return hqlQueryPlan.Translators;
      }
    }

    public override IQueryTranslator[] GetQueries(IQueryExpression query, bool scalar)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        IQueryExpressionPlan hqlQueryPlan = this.Factory.QueryPlanCache.GetHQLQueryPlan(query, scalar, this.enabledFilters);
        this.AutoFlushIfRequired(hqlQueryPlan.QuerySpaces);
        return hqlQueryPlan.Translators;
      }
    }

    public override IEnumerable<T> Enumerable<T>(string query, QueryParameters queryParameters)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        queryParameters.ValidateParameters();
        IQueryPlan hqlQueryPlan = this.GetHQLQueryPlan(query, true);
        this.AutoFlushIfRequired(hqlQueryPlan.QuerySpaces);
        ++this.dontFlushFromFind;
        try
        {
          return hqlQueryPlan.PerformIterate<T>(queryParameters, (IEventSource) this);
        }
        finally
        {
          --this.dontFlushFromFind;
        }
      }
    }

    public override IEnumerable Enumerable(string query, QueryParameters queryParameters)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        queryParameters.ValidateParameters();
        IQueryPlan hqlQueryPlan = this.GetHQLQueryPlan(query, true);
        this.AutoFlushIfRequired(hqlQueryPlan.QuerySpaces);
        ++this.dontFlushFromFind;
        try
        {
          return hqlQueryPlan.PerformIterate(queryParameters, (IEventSource) this);
        }
        finally
        {
          --this.dontFlushFromFind;
        }
      }
    }

    public int Delete(string query)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.Delete(query, SessionImpl.NoArgs, SessionImpl.NoTypes);
    }

    public int Delete(string query, object value, IType type)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.Delete(query, new object[1]{ value }, new IType[1]
        {
          type
        });
    }

    public int Delete(string query, object[] values, IType[] types)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        if (string.IsNullOrEmpty(query))
          throw new ArgumentNullException(nameof (query), "attempt to perform delete-by-query with null query");
        this.CheckAndUpdateSessionStatus();
        if (SessionImpl.log.IsDebugEnabled)
        {
          SessionImpl.log.Debug((object) ("delete: " + query));
          if (values.Length != 0)
            SessionImpl.log.Debug((object) ("parameters: " + StringHelper.ToString(values)));
        }
        IList list = this.Find(query, values, types);
        int count = list.Count;
        for (int index = 0; index < count; ++index)
          this.Delete(list[index]);
        return count;
      }
    }

    public void Lock(object obj, LockMode lockMode)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireLock(new LockEvent(obj, lockMode, (IEventSource) this));
    }

    public void Lock(string entityName, object obj, LockMode lockMode)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireLock(new LockEvent(entityName, obj, lockMode, (IEventSource) this));
    }

    public IQuery CreateFilter(object collection, string queryString)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        this.CheckAndUpdateSessionStatus();
        return (IQuery) new CollectionFilterImpl(queryString, collection, (ISessionImplementor) this, this.GetFilterQueryPlan(collection, queryString, (QueryParameters) null, false).ParameterMetadata);
      }
    }

    private FilterQueryPlan GetFilterQueryPlan(
      object collection,
      string filter,
      QueryParameters parameters,
      bool shallow)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        CollectionEntry collectionEntry = collection != null ? this.persistenceContext.GetCollectionEntryOrNull(collection) : throw new ArgumentNullException(nameof (collection), "null collection passed to filter");
        ICollectionPersister loadedPersister1 = collectionEntry == null ? (ICollectionPersister) null : collectionEntry.LoadedPersister;
        FilterQueryPlan filterQueryPlan;
        if (loadedPersister1 == null)
        {
          this.Flush();
          collectionEntry = this.persistenceContext.GetCollectionEntryOrNull(collection);
          filterQueryPlan = this.Factory.QueryPlanCache.GetFilterQueryPlan(filter, ((collectionEntry == null ? (ICollectionPersister) null : collectionEntry.LoadedPersister) ?? throw new QueryException("The collection was unreferenced")).Role, shallow, this.EnabledFilters);
        }
        else
        {
          filterQueryPlan = this.Factory.QueryPlanCache.GetFilterQueryPlan(filter, loadedPersister1.Role, shallow, this.EnabledFilters);
          if (this.AutoFlushIfRequired(filterQueryPlan.QuerySpaces))
          {
            collectionEntry = this.persistenceContext.GetCollectionEntryOrNull(collection);
            ICollectionPersister loadedPersister2 = collectionEntry == null ? (ICollectionPersister) null : collectionEntry.LoadedPersister;
            if (loadedPersister1 != loadedPersister2)
            {
              if (loadedPersister2 == null)
                throw new QueryException("The collection was dereferenced");
              filterQueryPlan = this.Factory.QueryPlanCache.GetFilterQueryPlan(filter, loadedPersister2.Role, shallow, this.EnabledFilters);
            }
          }
        }
        if (parameters != null)
        {
          parameters.PositionalParameterValues[0] = collectionEntry.LoadedKey;
          parameters.PositionalParameterTypes[0] = collectionEntry.LoadedPersister.KeyType;
        }
        return filterQueryPlan;
      }
    }

    public override object Instantiate(string clazz, object id)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.Instantiate(this.Factory.GetEntityPersister(clazz), id);
    }

    public ActionQueue ActionQueue
    {
      get
      {
        this.CheckAndUpdateSessionStatus();
        return this.actionQueue;
      }
    }

    public object Instantiate(IEntityPersister persister, object id)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.ErrorIfClosed();
        return this.interceptor.Instantiate(persister.EntityName, this.entityMode, id) ?? persister.Instantiate(id, this.entityMode);
      }
    }

    public void ForceFlush(EntityEntry entityEntry)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        if (SessionImpl.log.IsDebugEnabled)
          SessionImpl.log.Debug((object) ("flushing to force deletion of re-saved object: " + MessageHelper.InfoString(entityEntry.Persister, entityEntry.Id, this.Factory)));
        if (this.persistenceContext.CascadeLevel > 0)
          throw new ObjectDeletedException("deleted object would be re-saved by cascade (remove deleted object from associations)", entityEntry.Id, entityEntry.EntityName);
        this.Flush();
      }
    }

    public void Merge(string entityName, object obj, IDictionary copiedAlready)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireMerge(copiedAlready, new MergeEvent(entityName, obj, (IEventSource) this));
    }

    public void Persist(string entityName, object obj, IDictionary createdAlready)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FirePersist(createdAlready, new PersistEvent(entityName, obj, (IEventSource) this));
    }

    public void PersistOnFlush(string entityName, object obj, IDictionary copiedAlready)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FirePersistOnFlush(copiedAlready, new PersistEvent(entityName, obj, (IEventSource) this));
    }

    public void Refresh(object obj, IDictionary refreshedAlready)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireRefresh(refreshedAlready, new RefreshEvent(obj, (IEventSource) this));
    }

    [Obsolete("Use Merge(string, object, IDictionary) instead")]
    public void SaveOrUpdateCopy(string entityName, object obj, IDictionary copiedAlready)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireSaveOrUpdateCopy(copiedAlready, new MergeEvent(entityName, obj, (IEventSource) this));
    }

    public void Delete(
      string entityName,
      object child,
      bool isCascadeDeleteEnabled,
      ISet transientEntities)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireDelete(new DeleteEvent(entityName, child, isCascadeDeleteEnabled, (IEventSource) this), transientEntities);
    }

    public object Merge(string entityName, object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.FireMerge(new MergeEvent(entityName, obj, (IEventSource) this));
    }

    public T Merge<T>(T entity) where T : class => (T) this.Merge((object) entity);

    public T Merge<T>(string entityName, T entity) where T : class
    {
      return (T) this.Merge(entityName, (object) entity);
    }

    public object Merge(object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.Merge((string) null, obj);
    }

    public void Persist(string entityName, object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FirePersist(new PersistEvent(entityName, obj, (IEventSource) this));
    }

    public void Persist(object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.Persist((string) null, obj);
    }

    public void PersistOnFlush(string entityName, object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FirePersistOnFlush(new PersistEvent(entityName, obj, (IEventSource) this));
    }

    public void PersistOnFlush(object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.Persist((string) null, obj);
    }

    public override FlushMode FlushMode
    {
      get => this.flushMode;
      set => this.flushMode = value;
    }

    public bool FlushBeforeCompletionEnabled => this.flushBeforeCompletionEnabled;

    public override string BestGuessEntityName(object entity)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        if (entity.IsProxy())
        {
          ILazyInitializer hibernateLazyInitializer = (entity as INHibernateProxy).HibernateLazyInitializer;
          if (hibernateLazyInitializer.IsUninitialized)
            return hibernateLazyInitializer.PersistentClass.FullName;
          entity = hibernateLazyInitializer.GetImplementation();
        }
        if (FieldInterceptionHelper.IsInstrumented(entity))
          return FieldInterceptionHelper.ExtractFieldInterceptor(entity).EntityName;
        EntityEntry entry = this.persistenceContext.GetEntry(entity);
        return entry == null ? this.GuessEntityName(entity) : entry.Persister.EntityName;
      }
    }

    public override string GuessEntityName(object entity)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        string str = this.interceptor.GetEntityName(entity);
        if (str == null)
        {
          System.Type type = entity.GetType();
          str = this.Factory.TryGetGuessEntityName(type) ?? type.FullName;
        }
        return str;
      }
    }

    public override bool IsEventSource => true;

    public override object GetEntityUsingInterceptor(EntityKey key)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        object entity1 = this.persistenceContext.GetEntity(key);
        if (entity1 != null)
          return entity1;
        object entity2 = this.interceptor.GetEntity(key.EntityName, key.Identifier);
        if (entity2 != null)
          this.Lock(entity2, LockMode.None);
        return entity2;
      }
    }

    public override IPersistenceContext PersistenceContext
    {
      get
      {
        this.CheckAndUpdateSessionStatus();
        return (IPersistenceContext) this.persistenceContext;
      }
    }

    private bool AutoFlushIfRequired(ISet<string> querySpaces)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        if (!this.TransactionInProgress)
          return false;
        AutoFlushEvent @event = new AutoFlushEvent(querySpaces, (IEventSource) this);
        foreach (IAutoFlushEventListener flushEventListener in this.listeners.AutoFlushEventListeners)
          flushEventListener.OnAutoFlush(@event);
        return @event.FlushRequired;
      }
    }

    public void Load(object obj, object id)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireLoad(new LoadEvent(id, obj, (IEventSource) this), LoadEventListener.Reload);
    }

    public T Load<T>(object id)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return (T) this.Load(typeof (T), id);
    }

    public T Load<T>(object id, LockMode lockMode)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return (T) this.Load(typeof (T), id, lockMode);
    }

    public object Load(System.Type entityClass, object id, LockMode lockMode)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.Load(entityClass.FullName, id, lockMode);
    }

    public object Load(string entityName, object id)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        LoadEvent @event = id != null ? new LoadEvent(id, entityName, false, (IEventSource) this) : throw new ArgumentNullException(nameof (id), "null is not a valid identifier");
        bool success = false;
        try
        {
          this.FireLoad(@event, LoadEventListener.Load);
          if (@event.Result == null)
            this.Factory.EntityNotFoundDelegate.HandleEntityNotFound(entityName, id);
          success = true;
          return @event.Result;
        }
        finally
        {
          this.AfterOperation(success);
        }
      }
    }

    public object Load(string entityName, object id, LockMode lockMode)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        LoadEvent @event = new LoadEvent(id, entityName, lockMode, (IEventSource) this);
        this.FireLoad(@event, LoadEventListener.Load);
        return @event.Result;
      }
    }

    public object Load(System.Type entityClass, object id)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.Load(entityClass.FullName, id);
    }

    public T Get<T>(object id)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return (T) this.Get(typeof (T), id);
    }

    public T Get<T>(object id, LockMode lockMode)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return (T) this.Get(typeof (T), id, lockMode);
    }

    public object Get(System.Type entityClass, object id)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.Get(entityClass.FullName, id);
    }

    public object Get(System.Type clazz, object id, LockMode lockMode)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        LoadEvent @event = new LoadEvent(id, clazz.FullName, lockMode, (IEventSource) this);
        this.FireLoad(@event, LoadEventListener.Get);
        return @event.Result;
      }
    }

    public string GetEntityName(object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        if (obj.IsProxy())
        {
          INHibernateProxy proxy = obj as INHibernateProxy;
          obj = this.persistenceContext.ContainsProxy(proxy) ? proxy.HibernateLazyInitializer.GetImplementation() : throw new TransientObjectException("proxy was not associated with the session");
        }
        return (this.persistenceContext.GetEntry(obj) ?? throw new TransientObjectException("object references an unsaved transient instance - save the transient instance before flushing or set cascade action for the property to something that would make it autosave: " + obj.GetType().FullName)).Persister.EntityName;
      }
    }

    public object Get(string entityName, object id)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        LoadEvent @event = new LoadEvent(id, entityName, false, (IEventSource) this);
        bool success = false;
        try
        {
          this.FireLoad(@event, LoadEventListener.Get);
          success = true;
          return @event.Result;
        }
        finally
        {
          this.AfterOperation(success);
        }
      }
    }

    public override object ImmediateLoad(string entityName, object id)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        if (SessionImpl.log.IsDebugEnabled)
        {
          IEntityPersister entityPersister = this.Factory.GetEntityPersister(entityName);
          SessionImpl.log.Debug((object) ("initializing proxy: " + MessageHelper.InfoString(entityPersister, id, this.Factory)));
        }
        LoadEvent @event = new LoadEvent(id, entityName, true, (IEventSource) this);
        this.FireLoad(@event, LoadEventListener.ImmediateLoad);
        return @event.Result;
      }
    }

    public override object InternalLoad(string entityName, object id, bool eager, bool isNullable)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        LoadType loadType = isNullable ? LoadEventListener.InternalLoadNullable : (eager ? LoadEventListener.InternalLoadEager : LoadEventListener.InternalLoadLazy);
        LoadEvent @event = new LoadEvent(id, entityName, true, (IEventSource) this);
        this.FireLoad(@event, loadType);
        if (!isNullable)
          UnresolvableObjectException.ThrowIfNull(@event.Result, id, entityName);
        return @event.Result;
      }
    }

    public void Refresh(object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireRefresh(new RefreshEvent(obj, (IEventSource) this));
    }

    public void Refresh(object obj, LockMode lockMode)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireRefresh(new RefreshEvent(obj, lockMode, (IEventSource) this));
    }

    public ITransaction BeginTransaction(IsolationLevel isolationLevel)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        if (this.rootSession != null)
          SessionImpl.log.Warn((object) "Transaction started on non-root session");
        this.CheckAndUpdateSessionStatus();
        return this.connectionManager.BeginTransaction(isolationLevel);
      }
    }

    public ITransaction BeginTransaction()
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        if (this.rootSession != null)
          SessionImpl.log.Warn((object) "Transaction started on non-root session");
        this.CheckAndUpdateSessionStatus();
        return this.connectionManager.BeginTransaction();
      }
    }

    public ITransaction Transaction => this.connectionManager.Transaction;

    public override void Flush()
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        if (this.persistenceContext.CascadeLevel > 0)
          throw new HibernateException("Flush during cascade is dangerous");
        foreach (IFlushEventListener flushEventListener in this.listeners.FlushEventListeners)
          flushEventListener.OnFlush(new FlushEvent((IEventSource) this));
      }
    }

    public override bool TransactionInProgress => !this.IsClosed && this.Transaction.IsActive;

    public bool IsDirty()
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        SessionImpl.log.Debug((object) "checking session dirtiness");
        if (this.actionQueue.AreInsertionsOrDeletionsQueued)
        {
          SessionImpl.log.Debug((object) "session dirty (scheduled updates and insertions)");
          return true;
        }
        DirtyCheckEvent @event = new DirtyCheckEvent((IEventSource) this);
        foreach (IDirtyCheckEventListener checkEventListener in this.listeners.DirtyCheckEventListeners)
          checkEventListener.OnDirtyCheck(@event);
        return @event.Dirty;
      }
    }

    public object GetIdentifier(object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        if (obj.IsProxy())
        {
          ILazyInitializer hibernateLazyInitializer = (obj as INHibernateProxy).HibernateLazyInitializer;
          return hibernateLazyInitializer.Session == this ? hibernateLazyInitializer.Identifier : throw new TransientObjectException("The proxy was not associated with this session");
        }
        return (this.persistenceContext.GetEntry(obj) ?? throw new TransientObjectException("the instance was not associated with this session")).Id;
      }
    }

    public override object GetContextEntityIdentifier(object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return obj.IsProxy() ? (obj as INHibernateProxy).HibernateLazyInitializer.Identifier : this.persistenceContext.GetEntry(obj)?.Id;
    }

    internal ICollectionPersister GetCollectionPersister(string role)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.Factory.GetCollectionPersister(role);
    }

    public override void InitializeCollection(IPersistentCollection collection, bool writing)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (IInitializeCollectionEventListener collectionEventListener in this.listeners.InitializeCollectionEventListeners)
          collectionEventListener.OnInitializeCollection(new InitializeCollectionEvent(collection, (IEventSource) this));
      }
    }

    public override IDbConnection Connection => this.connectionManager.GetConnection();

    public override bool IsConnected => this.connectionManager.IsConnected;

    public IDbConnection Disconnect()
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        SessionImpl.log.Debug((object) "disconnecting session");
        return this.connectionManager.Disconnect();
      }
    }

    public void Reconnect()
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        SessionImpl.log.Debug((object) "reconnecting session");
        this.connectionManager.Reconnect();
      }
    }

    public void Reconnect(IDbConnection conn)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        SessionImpl.log.Debug((object) "reconnecting session");
        this.connectionManager.Reconnect(conn);
      }
    }

    ~SessionImpl() => this.Dispose(false);

    public void Dispose()
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        SessionImpl.log.Debug((object) string.Format("[session-id={0}] running ISession.Dispose()", (object) this.SessionId));
        if (this.TransactionContext != null)
          this.TransactionContext.ShouldCloseSessionOnDistributedTransactionCompleted = true;
        else
          this.Dispose(true);
      }
    }

    private void Dispose(bool isDisposing)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        if (this.IsAlreadyDisposed)
          return;
        SessionImpl.log.Debug((object) string.Format("[session-id={0}] executing real Dispose({1})", (object) this.SessionId, (object) isDisposing));
        if (isDisposing && !this.IsClosed)
          this.Close();
        this.IsAlreadyDisposed = true;
        GC.SuppressFinalize((object) this);
      }
    }

    private void Filter(
      object collection,
      string filter,
      QueryParameters queryParameters,
      IList results)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        FilterQueryPlan filterQueryPlan = this.GetFilterQueryPlan(collection, filter, queryParameters, false);
        bool success = false;
        ++this.dontFlushFromFind;
        try
        {
          filterQueryPlan.PerformList(queryParameters, (ISessionImplementor) this, results);
          success = true;
        }
        catch (HibernateException ex)
        {
          throw;
        }
        catch (Exception ex)
        {
          throw this.Convert(ex, "could not execute query");
        }
        finally
        {
          --this.dontFlushFromFind;
          this.AfterOperation(success);
        }
      }
    }

    public override IList ListFilter(
      object collection,
      string filter,
      QueryParameters queryParameters)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        IList results = (IList) new ArrayList();
        this.Filter(collection, filter, queryParameters, results);
        return results;
      }
    }

    public override IList<T> ListFilter<T>(
      object collection,
      string filter,
      QueryParameters queryParameters)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        System.Collections.Generic.List<T> results = new System.Collections.Generic.List<T>();
        this.Filter(collection, filter, queryParameters, (IList) results);
        return (IList<T>) results;
      }
    }

    public override IEnumerable EnumerableFilter(
      object collection,
      string filter,
      QueryParameters queryParameters)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        return this.GetFilterQueryPlan(collection, filter, queryParameters, true).PerformIterate(queryParameters, (IEventSource) this);
      }
    }

    public override IEnumerable<T> EnumerableFilter<T>(
      object collection,
      string filter,
      QueryParameters queryParameters)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        return this.GetFilterQueryPlan(collection, filter, queryParameters, true).PerformIterate<T>(queryParameters, (IEventSource) this);
      }
    }

    public ICriteria CreateCriteria<T>() where T : class
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.CreateCriteria(typeof (T));
    }

    public ICriteria CreateCriteria(System.Type persistentClass)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        return (ICriteria) new CriteriaImpl(persistentClass, (ISessionImplementor) this);
      }
    }

    public ICriteria CreateCriteria<T>(string alias) where T : class
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.CreateCriteria(typeof (T), alias);
    }

    public ICriteria CreateCriteria(System.Type persistentClass, string alias)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        return (ICriteria) new CriteriaImpl(persistentClass, alias, (ISessionImplementor) this);
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

    public ICriteria CreateCriteria(string entityName)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        return (ICriteria) new CriteriaImpl(entityName, (ISessionImplementor) this);
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

    public IQueryOver<T, T> QueryOver<T>(string entityName) where T : class
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        return (IQueryOver<T, T>) new NHibernate.Criterion.QueryOver<T, T>(new CriteriaImpl(entityName, (ISessionImplementor) this));
      }
    }

    public IQueryOver<T, T> QueryOver<T>(string entityName, Expression<Func<T>> alias) where T : class
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        string memberExpression = ExpressionProcessor.FindMemberExpression(alias.Body);
        return (IQueryOver<T, T>) new NHibernate.Criterion.QueryOver<T, T>(new CriteriaImpl(entityName, memberExpression, (ISessionImplementor) this));
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
        ISet<string> querySpaces = (ISet<string>) new HashedSet<string>();
        for (int index = 0; index < length; ++index)
        {
          criteriaLoaderArray[index] = new CriteriaLoader(this.GetOuterJoinLoadable(implementors[index]), this.Factory, criteria, implementors[index], this.enabledFilters);
          querySpaces.AddAll((ICollection<string>) criteriaLoaderArray[index].QuerySpaces);
        }
        this.AutoFlushIfRequired(querySpaces);
        ++this.dontFlushFromFind;
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
          --this.dontFlushFromFind;
          this.AfterOperation(success);
        }
      }
    }

    internal IOuterJoinLoadable GetOuterJoinLoadable(string entityName)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        if (!(this.Factory.GetEntityPersister(entityName) is IOuterJoinLoadable entityPersister))
          throw new MappingException("class persister is not OuterJoinLoadable: " + entityName);
        return entityPersister;
      }
    }

    public bool Contains(object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        if (obj.IsProxy())
        {
          ILazyInitializer hibernateLazyInitializer = (obj as INHibernateProxy).HibernateLazyInitializer;
          if (hibernateLazyInitializer.IsUninitialized)
            return hibernateLazyInitializer.Session == this;
          obj = hibernateLazyInitializer.GetImplementation();
        }
        EntityEntry entry = this.persistenceContext.GetEntry(obj);
        return entry != null && entry.Status != Status.Deleted && entry.Status != Status.Gone;
      }
    }

    public void Evict(object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireEvict(new EvictEvent(obj, (IEventSource) this));
    }

    public override ISQLQuery CreateSQLQuery(string sql)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        return base.CreateSQLQuery(sql);
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

    public override void List(
      NativeSQLQuerySpecification spec,
      QueryParameters queryParameters,
      IList results)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.ListCustomQuery((ICustomQuery) new SQLCustomQuery(spec.SqlQueryReturns, spec.QueryString, (ICollection<string>) spec.QuerySpaces, this.Factory), queryParameters, results);
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
        this.AutoFlushIfRequired(customLoader.QuerySpaces);
        bool success = false;
        ++this.dontFlushFromFind;
        try
        {
          ArrayHelper.AddAll(results, customLoader.List((ISessionImplementor) this, queryParameters));
          success = true;
        }
        finally
        {
          --this.dontFlushFromFind;
          this.AfterOperation(success);
        }
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

    public void Clear()
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        this.actionQueue.Clear();
        this.persistenceContext.Clear();
      }
    }

    public void Replicate(object obj, ReplicationMode replicationMode)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireReplicate(new ReplicateEvent(obj, replicationMode, (IEventSource) this));
    }

    public void Replicate(string entityName, object obj, ReplicationMode replicationMode)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        this.FireReplicate(new ReplicateEvent(entityName, obj, replicationMode, (IEventSource) this));
    }

    public ISessionFactory SessionFactory => (ISessionFactory) this.Factory;

    public void CancelQuery()
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        this.Batcher.CancelLastQuery();
      }
    }

    [Obsolete("Use Merge(object) instead")]
    public object SaveOrUpdateCopy(object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.FireSaveOrUpdateCopy(new MergeEvent((string) null, obj, (IEventSource) this));
    }

    [Obsolete("No direct replacement. Use Merge instead.")]
    public object SaveOrUpdateCopy(object obj, object id)
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return this.FireSaveOrUpdateCopy(new MergeEvent((string) null, obj, id, (IEventSource) this));
    }

    public NHibernate.IFilter GetEnabledFilter(string filterName)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        NHibernate.IFilter enabledFilter;
        this.enabledFilters.TryGetValue(filterName, out enabledFilter);
        return enabledFilter;
      }
    }

    public NHibernate.IFilter EnableFilter(string filterName)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        FilterImpl filterImpl = new FilterImpl(this.Factory.GetFilterDefinition(filterName));
        this.enabledFilters[filterName] = (NHibernate.IFilter) filterImpl;
        if (!this.enabledFilterNames.Contains(filterName))
          this.enabledFilterNames.Add(filterName);
        return (NHibernate.IFilter) filterImpl;
      }
    }

    public void DisableFilter(string filterName)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        this.enabledFilters.Remove(filterName);
        this.enabledFilterNames.Remove(filterName);
      }
    }

    public override object GetFilterParameterValue(string filterParameterName)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        string[] filterParameterName1 = this.ParseFilterParameterName(filterParameterName);
        NHibernate.IFilter filter;
        this.enabledFilters.TryGetValue(filterParameterName1[0], out filter);
        return filter is FilterImpl filterImpl ? filterImpl.GetParameter(filterParameterName1[1]) : throw new ArgumentException("Filter [" + filterParameterName1[0] + "] currently not enabled");
      }
    }

    public override IType GetFilterParameterType(string filterParameterName)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        string[] filterParameterName1 = this.ParseFilterParameterName(filterParameterName);
        return (this.Factory.GetFilterDefinition(filterParameterName1[0]) ?? throw new ArgumentNullException(filterParameterName1[0], "Filter [" + filterParameterName1[0] + "] not defined")).GetParameterType(filterParameterName1[1]) ?? throw new ArgumentNullException(filterParameterName1[1], "Unable to locate type for filter parameter");
      }
    }

    public override IDictionary<string, NHibernate.IFilter> EnabledFilters
    {
      get
      {
        this.CheckAndUpdateSessionStatus();
        foreach (NHibernate.IFilter filter in (IEnumerable<NHibernate.IFilter>) this.enabledFilters.Values)
          filter.Validate();
        return this.enabledFilters;
      }
    }

    private string[] ParseFilterParameterName(string filterParameterName)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        int length = filterParameterName.IndexOf(".");
        return length > 0 ? new string[2]
        {
          filterParameterName.Substring(0, length),
          filterParameterName.Substring(length + 1)
        } : throw new ArgumentException("Invalid filter-parameter name format", nameof (filterParameterName));
      }
    }

    public override ConnectionManager ConnectionManager => this.connectionManager;

    public IMultiQuery CreateMultiQuery()
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return (IMultiQuery) new MultiQueryImpl((ISessionImplementor) this);
    }

    public IMultiCriteria CreateMultiCriteria()
    {
      using (new SessionIdLoggingContext(this.SessionId))
        return (IMultiCriteria) new MultiCriteriaImpl(this, this.Factory);
    }

    public ISessionStatistics Statistics
    {
      get => (ISessionStatistics) new SessionStatisticsImpl((ISessionImplementor) this);
    }

    public override void AfterTransactionBegin(ITransaction tx)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        this.interceptor.AfterTransactionBegin(tx);
      }
    }

    public override void BeforeTransactionCompletion(ITransaction tx)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        SessionImpl.log.Debug((object) "before transaction completion");
        this.actionQueue.BeforeTransactionCompletion();
        if (this.rootSession != null)
          return;
        try
        {
          this.interceptor.BeforeTransactionCompletion(tx);
        }
        catch (Exception ex)
        {
          SessionImpl.log.Error((object) "exception in interceptor BeforeTransactionCompletion()", ex);
        }
      }
    }

    public ISession SetBatchSize(int batchSize)
    {
      this.Batcher.BatchSize = batchSize;
      return (ISession) this;
    }

    public ISessionImplementor GetSessionImplementation() => (ISessionImplementor) this;

    public ISession GetSession(EntityMode entityMode)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        if (this.rootSession != null)
          return this.rootSession.GetSession(entityMode);
        this.CheckAndUpdateSessionStatus();
        ISession session = (ISession) null;
        if (this.childSessionsByEntityMode == null)
          this.childSessionsByEntityMode = (IDictionary<EntityMode, ISession>) new Dictionary<EntityMode, ISession>();
        else if (this.childSessionsByEntityMode.ContainsKey(entityMode))
          session = this.childSessionsByEntityMode[entityMode];
        if (session == null)
        {
          SessionImpl.log.DebugFormat("Creating child session with {0}", (object) entityMode);
          session = (ISession) new SessionImpl(this, entityMode);
          this.childSessionsByEntityMode.Add(entityMode, session);
        }
        return session;
      }
    }

    public override IInterceptor Interceptor => this.interceptor;

    public override EventListeners Listeners => this.listeners;

    public override int DontFlushFromFind => this.dontFlushFromFind;

    public override CacheMode CacheMode
    {
      get => this.cacheMode;
      set
      {
        this.CheckAndUpdateSessionStatus();
        if (SessionImpl.log.IsDebugEnabled)
          SessionImpl.log.Debug((object) ("setting cache mode to: " + (object) value));
        this.cacheMode = value;
      }
    }

    public override EntityMode EntityMode => this.entityMode;

    public EntityMode ActiveEntityMode => this.entityMode;

    public override string FetchProfile
    {
      get => this.fetchProfile;
      set
      {
        this.CheckAndUpdateSessionStatus();
        this.fetchProfile = value;
      }
    }

    public void SetReadOnly(object entityOrProxy, bool readOnly)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        this.persistenceContext.SetReadOnly(entityOrProxy, readOnly);
      }
    }

    public bool DefaultReadOnly
    {
      get => this.persistenceContext.DefaultReadOnly;
      set => this.persistenceContext.DefaultReadOnly = value;
    }

    public bool IsReadOnly(object entityOrProxy)
    {
      this.ErrorIfClosed();
      return this.persistenceContext.IsReadOnly(entityOrProxy);
    }

    private void FireDelete(DeleteEvent @event)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (IDeleteEventListener deleteEventListener in this.listeners.DeleteEventListeners)
          deleteEventListener.OnDelete(@event);
      }
    }

    private void FireDelete(DeleteEvent @event, ISet transientEntities)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (IDeleteEventListener deleteEventListener in this.listeners.DeleteEventListeners)
          deleteEventListener.OnDelete(@event, transientEntities);
      }
    }

    private void FireEvict(EvictEvent evictEvent)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (IEvictEventListener evictEventListener in this.listeners.EvictEventListeners)
          evictEventListener.OnEvict(evictEvent);
      }
    }

    private void FireLoad(LoadEvent @event, LoadType loadType)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (ILoadEventListener loadEventListener in this.listeners.LoadEventListeners)
          loadEventListener.OnLoad(@event, loadType);
      }
    }

    private void FireLock(LockEvent lockEvent)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (ILockEventListener lockEventListener in this.listeners.LockEventListeners)
          lockEventListener.OnLock(lockEvent);
      }
    }

    private object FireMerge(MergeEvent @event)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (IMergeEventListener mergeEventListener in this.listeners.MergeEventListeners)
          mergeEventListener.OnMerge(@event);
        return @event.Result;
      }
    }

    private void FireMerge(IDictionary copiedAlready, MergeEvent @event)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (IMergeEventListener mergeEventListener in this.listeners.MergeEventListeners)
          mergeEventListener.OnMerge(@event, copiedAlready);
      }
    }

    private void FirePersist(IDictionary copiedAlready, PersistEvent @event)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (IPersistEventListener persistEventListener in this.listeners.PersistEventListeners)
          persistEventListener.OnPersist(@event, copiedAlready);
      }
    }

    private void FirePersist(PersistEvent @event)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (IPersistEventListener persistEventListener in this.listeners.PersistEventListeners)
          persistEventListener.OnPersist(@event);
      }
    }

    private void FirePersistOnFlush(IDictionary copiedAlready, PersistEvent @event)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (IPersistEventListener flushEventListener in this.listeners.PersistOnFlushEventListeners)
          flushEventListener.OnPersist(@event, copiedAlready);
      }
    }

    private void FirePersistOnFlush(PersistEvent @event)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (IPersistEventListener flushEventListener in this.listeners.PersistOnFlushEventListeners)
          flushEventListener.OnPersist(@event);
      }
    }

    private void FireRefresh(RefreshEvent refreshEvent)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (IRefreshEventListener refreshEventListener in this.listeners.RefreshEventListeners)
          refreshEventListener.OnRefresh(refreshEvent);
      }
    }

    private void FireRefresh(IDictionary refreshedAlready, RefreshEvent refreshEvent)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (IRefreshEventListener refreshEventListener in this.listeners.RefreshEventListeners)
          refreshEventListener.OnRefresh(refreshEvent, refreshedAlready);
      }
    }

    private void FireReplicate(ReplicateEvent @event)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (IReplicateEventListener replicateEventListener in this.listeners.ReplicateEventListeners)
          replicateEventListener.OnReplicate(@event);
      }
    }

    private object FireSave(SaveOrUpdateEvent @event)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (ISaveOrUpdateEventListener saveEventListener in this.listeners.SaveEventListeners)
          saveEventListener.OnSaveOrUpdate(@event);
        return @event.ResultId;
      }
    }

    private void FireSaveOrUpdate(SaveOrUpdateEvent @event)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (ISaveOrUpdateEventListener updateEventListener in this.listeners.SaveOrUpdateEventListeners)
          updateEventListener.OnSaveOrUpdate(@event);
      }
    }

    private void FireSaveOrUpdateCopy(IDictionary copiedAlready, MergeEvent @event)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (IMergeEventListener copyEventListener in this.listeners.SaveOrUpdateCopyEventListeners)
          copyEventListener.OnMerge(@event, copiedAlready);
      }
    }

    private object FireSaveOrUpdateCopy(MergeEvent @event)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (IMergeEventListener copyEventListener in this.listeners.SaveOrUpdateCopyEventListeners)
          copyEventListener.OnMerge(@event);
        return @event.Result;
      }
    }

    private void FireUpdate(SaveOrUpdateEvent @event)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        foreach (ISaveOrUpdateEventListener updateEventListener in this.listeners.UpdateEventListeners)
          updateEventListener.OnSaveOrUpdate(@event);
      }
    }

    public override int ExecuteNativeUpdate(
      NativeSQLQuerySpecification nativeQuerySpecification,
      QueryParameters queryParameters)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        queryParameters.ValidateParameters();
        NativeSQLQueryPlan nativeSqlQueryPlan = this.GetNativeSQLQueryPlan(nativeQuerySpecification);
        this.AutoFlushIfRequired(nativeSqlQueryPlan.CustomQuery.QuerySpaces);
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
        this.AutoFlushIfRequired(hqlQueryPlan.QuerySpaces);
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
        return num;
      }
    }

    public override IEntityPersister GetEntityPersister(string entityName, object obj)
    {
      using (new SessionIdLoggingContext(this.SessionId))
      {
        this.CheckAndUpdateSessionStatus();
        if (entityName == null)
          return this.Factory.GetEntityPersister(this.GuessEntityName(obj));
        try
        {
          return this.Factory.GetEntityPersister(entityName).GetSubclassEntityPersister(obj, this.Factory, this.entityMode);
        }
        catch (HibernateException ex1)
        {
          try
          {
            return this.GetEntityPersister((string) null, obj);
          }
          catch (HibernateException ex2)
          {
          }
          throw;
        }
      }
    }
  }
}
