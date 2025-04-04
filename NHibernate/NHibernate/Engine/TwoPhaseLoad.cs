// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.TwoPhaseLoad
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Event;
using NHibernate.Impl;
using NHibernate.Intercept;
using NHibernate.Persister.Entity;
using NHibernate.Properties;
using NHibernate.Proxy;
using NHibernate.Type;
using System.Collections;
using System.Diagnostics;

#nullable disable
namespace NHibernate.Engine
{
  public static class TwoPhaseLoad
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (TwoPhaseLoad));

    public static void PostHydrate(
      IEntityPersister persister,
      object id,
      object[] values,
      object rowId,
      object obj,
      LockMode lockMode,
      bool lazyPropertiesAreUnfetched,
      ISessionImplementor session)
    {
      object version = Versioning.GetVersion(values, persister);
      session.PersistenceContext.AddEntry(obj, Status.Loading, values, rowId, id, version, lockMode, true, persister, false, lazyPropertiesAreUnfetched);
      if (!TwoPhaseLoad.log.IsDebugEnabled || version == null)
        return;
      string str = persister.IsVersioned ? persister.VersionType.ToLoggableString(version, session.Factory) : "null";
      TwoPhaseLoad.log.Debug((object) ("Version: " + str));
    }

    public static void InitializeEntity(
      object entity,
      bool readOnly,
      ISessionImplementor session,
      PreLoadEvent preLoadEvent,
      PostLoadEvent postLoadEvent)
    {
      bool statisticsEnabled = session.Factory.Statistics.IsStatisticsEnabled;
      Stopwatch stopwatch = new Stopwatch();
      if (statisticsEnabled)
        stopwatch.Start();
      IPersistenceContext persistenceContext = session.PersistenceContext;
      EntityEntry entry = persistenceContext.GetEntry(entity);
      IEntityPersister persister = entry != null ? entry.Persister : throw new AssertionFailure("possible non-threadsafe access to the session");
      object id = entry.Id;
      object[] loadedState = entry.LoadedState;
      if (TwoPhaseLoad.log.IsDebugEnabled)
        TwoPhaseLoad.log.Debug((object) ("resolving associations for " + MessageHelper.InfoString(persister, id, session.Factory)));
      IType[] propertyTypes = persister.PropertyTypes;
      for (int index = 0; index < loadedState.Length; ++index)
      {
        object objB = loadedState[index];
        if (!object.Equals(LazyPropertyInitializer.UnfetchedProperty, objB) && !object.Equals(BackrefPropertyAccessor.Unknown, objB))
          loadedState[index] = propertyTypes[index].ResolveIdentifier(objB, session, entity);
      }
      if (session.IsEventSource)
      {
        preLoadEvent.Entity = entity;
        preLoadEvent.State = loadedState;
        preLoadEvent.Id = id;
        preLoadEvent.Persister = persister;
        foreach (IPreLoadEventListener loadEventListener in session.Listeners.PreLoadEventListeners)
          loadEventListener.OnPreLoad(preLoadEvent);
      }
      persister.SetPropertyValues(entity, loadedState, session.EntityMode);
      ISessionFactoryImplementor factory = session.Factory;
      if (persister.HasCache && (session.CacheMode & CacheMode.Put) == CacheMode.Put)
      {
        if (TwoPhaseLoad.log.IsDebugEnabled)
          TwoPhaseLoad.log.Debug((object) ("adding entity to second-level cache: " + MessageHelper.InfoString(persister, id, session.Factory)));
        object version = Versioning.GetVersion(loadedState, persister);
        CacheEntry cacheEntry = new CacheEntry(loadedState, persister, entry.LoadedWithLazyPropertiesUnfetched, version, session, entity);
        CacheKey key = new CacheKey(id, persister.IdentifierType, persister.RootEntityName, session.EntityMode, session.Factory);
        if (persister.Cache.Put(key, persister.CacheEntryStructure.Structure((object) cacheEntry), session.Timestamp, version, persister.IsVersioned ? persister.VersionType.Comparator : (IComparer) null, TwoPhaseLoad.UseMinimalPuts(session, entry)) && factory.Statistics.IsStatisticsEnabled)
          factory.StatisticsImplementor.SecondLevelCachePut(persister.Cache.RegionName);
      }
      bool flag = readOnly;
      if (!persister.IsMutable)
      {
        flag = true;
      }
      else
      {
        object proxy = persistenceContext.GetProxy(entry.EntityKey);
        if (proxy != null)
          flag = ((INHibernateProxy) proxy).HibernateLazyInitializer.ReadOnly;
      }
      if (flag)
      {
        persistenceContext.SetEntryStatus(entry, Status.ReadOnly);
      }
      else
      {
        TypeHelper.DeepCopy(loadedState, persister.PropertyTypes, persister.PropertyUpdateability, loadedState, session);
        persistenceContext.SetEntryStatus(entry, Status.Loaded);
      }
      persister.AfterInitialize(entity, entry.LoadedWithLazyPropertiesUnfetched, session);
      if (session.IsEventSource)
      {
        postLoadEvent.Entity = entity;
        postLoadEvent.Id = id;
        postLoadEvent.Persister = persister;
        foreach (IPostLoadEventListener loadEventListener in session.Listeners.PostLoadEventListeners)
          loadEventListener.OnPostLoad(postLoadEvent);
      }
      if (TwoPhaseLoad.log.IsDebugEnabled)
        TwoPhaseLoad.log.Debug((object) ("done materializing entity " + MessageHelper.InfoString(persister, id, session.Factory)));
      if (!statisticsEnabled)
        return;
      stopwatch.Stop();
      factory.StatisticsImplementor.LoadEntity(persister.EntityName, stopwatch.Elapsed);
    }

    private static bool UseMinimalPuts(ISessionImplementor session, EntityEntry entityEntry)
    {
      if (session.Factory.Settings.IsMinimalPutsEnabled && session.CacheMode != CacheMode.Refresh)
        return true;
      return entityEntry.Persister.HasLazyProperties && entityEntry.LoadedWithLazyPropertiesUnfetched && entityEntry.Persister.IsLazyPropertiesCacheable;
    }

    public static void AddUninitializedEntity(
      EntityKey key,
      object obj,
      IEntityPersister persister,
      LockMode lockMode,
      bool lazyPropertiesAreUnfetched,
      ISessionImplementor session)
    {
      session.PersistenceContext.AddEntity(obj, Status.Loading, (object[]) null, key, (object) null, lockMode, true, persister, false, lazyPropertiesAreUnfetched);
    }

    public static void AddUninitializedCachedEntity(
      EntityKey key,
      object obj,
      IEntityPersister persister,
      LockMode lockMode,
      bool lazyPropertiesAreUnfetched,
      object version,
      ISessionImplementor session)
    {
      session.PersistenceContext.AddEntity(obj, Status.Loading, (object[]) null, key, version, lockMode, true, persister, false, lazyPropertiesAreUnfetched);
    }
  }
}
