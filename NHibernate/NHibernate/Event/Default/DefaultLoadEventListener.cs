// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultLoadEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Cache.Access;
using NHibernate.Cache.Entry;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Type;
using System;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class DefaultLoadEventListener : AbstractLockUpgradeEventListener, ILoadEventListener
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (DefaultLoadEventListener));
    public static readonly object RemovedEntityMarker = new object();
    public static readonly object InconsistentRTNClassMarker = new object();
    public static readonly LockMode DefaultLockMode = LockMode.None;

    public virtual void OnLoad(LoadEvent @event, LoadType loadType)
    {
      ISessionImplementor session = (ISessionImplementor) @event.Session;
      IEntityPersister entityPersister;
      if (@event.InstanceToLoad != null)
      {
        entityPersister = session.GetEntityPersister((string) null, @event.InstanceToLoad);
        @event.EntityClassName = @event.InstanceToLoad.GetType().FullName;
      }
      else
        entityPersister = this.GetEntityPersister(session.Factory, @event.EntityClassName);
      if (entityPersister == null)
      {
        StringBuilder stringBuilder = new StringBuilder(512);
        stringBuilder.AppendLine(string.Format("Unable to locate persister for the entity named '{0}'.", (object) @event.EntityClassName));
        stringBuilder.AppendLine("The persister define the persistence strategy for an entity.");
        stringBuilder.AppendLine("Possible causes:");
        stringBuilder.AppendLine(string.Format(" - The mapping for '{0}' was not added to the NHibernate configuration.", (object) @event.EntityClassName));
        throw new HibernateException(stringBuilder.ToString());
      }
      if (!entityPersister.IdentifierType.IsComponentType)
      {
        System.Type returnedClass = entityPersister.IdentifierType.ReturnedClass;
        if (returnedClass != null && !returnedClass.IsInstanceOfType(@event.EntityId))
          throw new TypeMismatchException("Provided id of the wrong type. Expected: " + (object) returnedClass + ", got " + (object) @event.EntityId.GetType());
      }
      EntityKey keyToLoad = new EntityKey(@event.EntityId, entityPersister, session.EntityMode);
      try
      {
        if (loadType.IsNakedEntityReturned)
          @event.Result = this.Load(@event, entityPersister, keyToLoad, loadType);
        else if (@event.LockMode == LockMode.None)
          @event.Result = this.ProxyOrLoad(@event, entityPersister, keyToLoad, loadType);
        else
          @event.Result = this.LockAndLoad(@event, entityPersister, keyToLoad, loadType, session);
      }
      catch (HibernateException ex)
      {
        DefaultLoadEventListener.log.Info((object) "Error performing load command", (Exception) ex);
        throw;
      }
    }

    protected virtual object Load(
      LoadEvent @event,
      IEntityPersister persister,
      EntityKey keyToLoad,
      LoadType options)
    {
      if (@event.InstanceToLoad != null)
      {
        if (@event.Session.PersistenceContext.GetEntry(@event.InstanceToLoad) != null)
          throw new PersistentObjectException("attempted to load into an instance that was already associated with the session: " + MessageHelper.InfoString(persister, @event.EntityId, @event.Session.Factory));
        persister.SetIdentifier(@event.InstanceToLoad, @event.EntityId, @event.Session.EntityMode);
      }
      object obj = this.DoLoad(@event, persister, keyToLoad, options);
      bool flag = @event.InstanceToLoad != null;
      if ((!options.IsAllowNulls || flag) && obj == null)
        @event.Session.Factory.EntityNotFoundDelegate.HandleEntityNotFound(@event.EntityClassName, @event.EntityId);
      if (flag && obj != @event.InstanceToLoad)
        throw new NonUniqueObjectException(@event.EntityId, @event.EntityClassName);
      return obj;
    }

    protected virtual object ProxyOrLoad(
      LoadEvent @event,
      IEntityPersister persister,
      EntityKey keyToLoad,
      LoadType options)
    {
      if (DefaultLoadEventListener.log.IsDebugEnabled)
        DefaultLoadEventListener.log.Debug((object) ("loading entity: " + MessageHelper.InfoString(persister, @event.EntityId, @event.Session.Factory)));
      if (!persister.HasProxy)
        return this.Load(@event, persister, keyToLoad, options);
      IPersistenceContext persistenceContext = @event.Session.PersistenceContext;
      object proxy = persistenceContext.GetProxy(keyToLoad);
      if (proxy != null)
        return this.ReturnNarrowedProxy(@event, persister, keyToLoad, options, persistenceContext, proxy);
      return options.IsAllowProxyCreation ? this.CreateProxyIfNecessary(@event, persister, keyToLoad, options, persistenceContext) : this.Load(@event, persister, keyToLoad, options);
    }

    private object ReturnNarrowedProxy(
      LoadEvent @event,
      IEntityPersister persister,
      EntityKey keyToLoad,
      LoadType options,
      IPersistenceContext persistenceContext,
      object proxy)
    {
      DefaultLoadEventListener.log.Debug((object) "entity proxy found in session cache");
      INHibernateProxy proxy1 = (INHibernateProxy) proxy;
      ILazyInitializer hibernateLazyInitializer = proxy1.HibernateLazyInitializer;
      if (hibernateLazyInitializer.Unwrap)
        return hibernateLazyInitializer.GetImplementation();
      object obj = (object) null;
      if (!options.IsAllowProxyCreation)
      {
        obj = this.Load(@event, persister, keyToLoad, options);
        if (obj == null && !options.IsAllowNulls)
          @event.Session.Factory.EntityNotFoundDelegate.HandleEntityNotFound(persister.EntityName, keyToLoad.Identifier);
      }
      return obj == null && !options.IsAllowProxyCreation && options.ExactPersister ? (object) null : persistenceContext.NarrowProxy(proxy1, persister, keyToLoad, obj);
    }

    private object CreateProxyIfNecessary(
      LoadEvent @event,
      IEntityPersister persister,
      EntityKey keyToLoad,
      LoadType options,
      IPersistenceContext persistenceContext)
    {
      object entity = persistenceContext.GetEntity(keyToLoad);
      if (entity != null)
      {
        DefaultLoadEventListener.log.Debug((object) "entity found in session cache");
        if (options.IsCheckDeleted)
        {
          switch (persistenceContext.GetEntry(entity).Status)
          {
            case Status.Deleted:
            case Status.Gone:
              return (object) null;
          }
        }
        return entity;
      }
      DefaultLoadEventListener.log.Debug((object) "creating new proxy for entity");
      object proxy = persister.CreateProxy(@event.EntityId, (ISessionImplementor) @event.Session);
      persistenceContext.BatchFetchQueue.AddBatchLoadableEntityKey(keyToLoad);
      persistenceContext.AddProxy(keyToLoad, (INHibernateProxy) proxy);
      ((INHibernateProxy) proxy).HibernateLazyInitializer.ReadOnly = @event.Session.DefaultReadOnly || !persister.IsMutable;
      return proxy;
    }

    protected virtual object LockAndLoad(
      LoadEvent @event,
      IEntityPersister persister,
      EntityKey keyToLoad,
      LoadType options,
      ISessionImplementor source)
    {
      ISoftLock @lock = (ISoftLock) null;
      CacheKey key;
      if (persister.HasCache)
      {
        key = new CacheKey(@event.EntityId, persister.IdentifierType, persister.RootEntityName, source.EntityMode, source.Factory);
        @lock = persister.Cache.Lock(key, (object) null);
      }
      else
        key = (CacheKey) null;
      object impl;
      try
      {
        impl = this.Load(@event, persister, keyToLoad, options);
      }
      finally
      {
        if (persister.HasCache)
          persister.Cache.Release(key, @lock);
      }
      return @event.Session.PersistenceContext.ProxyFor(persister, keyToLoad, impl);
    }

    protected virtual object DoLoad(
      LoadEvent @event,
      IEntityPersister persister,
      EntityKey keyToLoad,
      LoadType options)
    {
      if (DefaultLoadEventListener.log.IsDebugEnabled)
        DefaultLoadEventListener.log.Debug((object) ("attempting to resolve: " + MessageHelper.InfoString(persister, @event.EntityId, @event.Session.Factory)));
      object obj1 = this.LoadFromSessionCache(@event, keyToLoad, options);
      if (obj1 == DefaultLoadEventListener.RemovedEntityMarker)
      {
        DefaultLoadEventListener.log.Debug((object) "load request found matching entity in context, but it is scheduled for removal; returning null");
        return (object) null;
      }
      if (obj1 == DefaultLoadEventListener.InconsistentRTNClassMarker)
      {
        DefaultLoadEventListener.log.Debug((object) "load request found matching entity in context, but the matched entity was of an inconsistent return type; returning null");
        return (object) null;
      }
      if (obj1 != null)
      {
        if (DefaultLoadEventListener.log.IsDebugEnabled)
          DefaultLoadEventListener.log.Debug((object) ("resolved object in session cache: " + MessageHelper.InfoString(persister, @event.EntityId, @event.Session.Factory)));
        return obj1;
      }
      object obj2 = this.LoadFromSecondLevelCache(@event, persister, options);
      if (obj2 != null)
      {
        if (DefaultLoadEventListener.log.IsDebugEnabled)
          DefaultLoadEventListener.log.Debug((object) ("resolved object in second-level cache: " + MessageHelper.InfoString(persister, @event.EntityId, @event.Session.Factory)));
        return obj2;
      }
      if (DefaultLoadEventListener.log.IsDebugEnabled)
        DefaultLoadEventListener.log.Debug((object) ("object not resolved in any cache: " + MessageHelper.InfoString(persister, @event.EntityId, @event.Session.Factory)));
      return this.LoadFromDatasource(@event, persister, keyToLoad, options);
    }

    protected virtual object LoadFromDatasource(
      LoadEvent @event,
      IEntityPersister persister,
      EntityKey keyToLoad,
      LoadType options)
    {
      ISessionImplementor session = (ISessionImplementor) @event.Session;
      bool statisticsEnabled = session.Factory.Statistics.IsStatisticsEnabled;
      Stopwatch stopwatch = new Stopwatch();
      if (statisticsEnabled)
        stopwatch.Start();
      object obj = persister.Load(@event.EntityId, @event.InstanceToLoad, @event.LockMode, session);
      if (@event.IsAssociationFetch && statisticsEnabled)
      {
        stopwatch.Stop();
        session.Factory.StatisticsImplementor.FetchEntity(@event.EntityClassName, stopwatch.Elapsed);
      }
      return obj;
    }

    protected virtual object LoadFromSessionCache(
      LoadEvent @event,
      EntityKey keyToLoad,
      LoadType options)
    {
      ISessionImplementor session = (ISessionImplementor) @event.Session;
      object usingInterceptor = session.GetEntityUsingInterceptor(keyToLoad);
      if (usingInterceptor != null)
      {
        EntityEntry entry = session.PersistenceContext.GetEntry(usingInterceptor);
        if (options.IsCheckDeleted)
        {
          switch (entry.Status)
          {
            case Status.Deleted:
            case Status.Gone:
              return DefaultLoadEventListener.RemovedEntityMarker;
          }
        }
        if (options.IsAllowNulls && !this.GetEntityPersister(@event.Session.Factory, @event.EntityClassName).IsInstance(usingInterceptor, @event.Session.EntityMode))
          return DefaultLoadEventListener.InconsistentRTNClassMarker;
        this.UpgradeLock(usingInterceptor, entry, @event.LockMode, session);
      }
      return usingInterceptor;
    }

    protected virtual object LoadFromSecondLevelCache(
      LoadEvent @event,
      IEntityPersister persister,
      LoadType options)
    {
      ISessionImplementor session = (ISessionImplementor) @event.Session;
      if (persister.HasCache && (session.CacheMode & CacheMode.Get) == CacheMode.Get && @event.LockMode.LessThan(LockMode.Read))
      {
        ISessionFactoryImplementor factory = session.Factory;
        CacheKey key = new CacheKey(@event.EntityId, persister.IdentifierType, persister.RootEntityName, session.EntityMode, factory);
        object map = persister.Cache.Get(key, session.Timestamp);
        if (factory.Statistics.IsStatisticsEnabled)
        {
          if (map == null)
          {
            factory.StatisticsImplementor.SecondLevelCacheMiss(persister.Cache.RegionName);
            DefaultLoadEventListener.log.DebugFormat("Entity cache miss: {0}", (object) key);
          }
          else
          {
            factory.StatisticsImplementor.SecondLevelCacheHit(persister.Cache.RegionName);
            DefaultLoadEventListener.log.DebugFormat("Entity cache hit: {0}", (object) key);
          }
        }
        if (map != null)
        {
          CacheEntry entry = (CacheEntry) persister.CacheEntryStructure.Destructure(map, factory);
          if (!options.ExactPersister || persister.EntityMetamodel.SubclassEntityNames.Contains(entry.Subclass))
            return this.AssembleCacheEntry(entry, @event.EntityId, persister, @event);
        }
      }
      return (object) null;
    }

    private object AssembleCacheEntry(
      CacheEntry entry,
      object id,
      IEntityPersister persister,
      LoadEvent @event)
    {
      object instanceToLoad = @event.InstanceToLoad;
      IEventSource session = @event.Session;
      ISessionFactoryImplementor factory = session.Factory;
      if (DefaultLoadEventListener.log.IsDebugEnabled)
        DefaultLoadEventListener.log.Debug((object) ("assembling entity from second-level cache: " + MessageHelper.InfoString(persister, id, factory)));
      IEntityPersister entityPersister = factory.GetEntityPersister(entry.Subclass);
      object obj = instanceToLoad ?? session.Instantiate(entityPersister, id);
      EntityKey key = new EntityKey(id, entityPersister, session.EntityMode);
      TwoPhaseLoad.AddUninitializedCachedEntity(key, obj, entityPersister, LockMode.None, entry.AreLazyPropertiesUnfetched, entry.Version, (ISessionImplementor) session);
      IType[] propertyTypes = entityPersister.PropertyTypes;
      object[] objArray = entry.Assemble(obj, id, entityPersister, session.Interceptor, (ISessionImplementor) session);
      TypeHelper.DeepCopy(objArray, propertyTypes, entityPersister.PropertyUpdateability, objArray, (ISessionImplementor) session);
      object version = Versioning.GetVersion(objArray, entityPersister);
      if (DefaultLoadEventListener.log.IsDebugEnabled)
        DefaultLoadEventListener.log.Debug((object) ("Cached Version: " + version));
      IPersistenceContext persistenceContext = session.PersistenceContext;
      bool flag = session.DefaultReadOnly;
      if (persister.IsMutable)
      {
        object proxy = persistenceContext.GetProxy(key);
        if (proxy != null)
          flag = ((INHibernateProxy) proxy).HibernateLazyInitializer.ReadOnly;
      }
      else
        flag = true;
      persistenceContext.AddEntry(obj, flag ? Status.ReadOnly : Status.Loaded, objArray, (object) null, id, version, LockMode.None, true, entityPersister, false, entry.AreLazyPropertiesUnfetched);
      entityPersister.AfterInitialize(obj, entry.AreLazyPropertiesUnfetched, (ISessionImplementor) session);
      persistenceContext.InitializeNonLazyCollections();
      PostLoadEvent event1 = new PostLoadEvent(session);
      event1.Entity = obj;
      event1.Id = id;
      event1.Persister = persister;
      foreach (IPostLoadEventListener loadEventListener in session.Listeners.PostLoadEventListeners)
        loadEventListener.OnPostLoad(event1);
      return obj;
    }

    protected virtual IEntityPersister GetEntityPersister(
      ISessionFactoryImplementor factory,
      string entityName)
    {
      IEntityPersister entityPersister = factory.TryGetEntityPersister(entityName);
      if (entityPersister != null)
        return entityPersister;
      string[] implementors = factory.GetImplementors(entityName);
      if (implementors.Length > 1)
      {
        StringBuilder messageBuilder = new StringBuilder(512);
        messageBuilder.AppendLine(string.Format("Ambiguous persister for {0} implemented by more than one hierarchy: ", (object) entityName));
        Array.ForEach<string>(implementors, (Action<string>) (s => messageBuilder.AppendLine(s)));
        throw new HibernateException(messageBuilder.ToString());
      }
      return implementors.Length == 0 ? (IEntityPersister) null : factory.GetEntityPersister(implementors[0]);
    }
  }
}
