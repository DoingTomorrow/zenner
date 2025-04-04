// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultInitializeCollectionEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using System;
using System.Diagnostics;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class DefaultInitializeCollectionEventListener : IInitializeCollectionEventListener
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (DefaultInitializeCollectionEventListener));

    public virtual void OnInitializeCollection(InitializeCollectionEvent @event)
    {
      IPersistentCollection collection = @event.Collection;
      ISessionImplementor session = (ISessionImplementor) @event.Session;
      bool statisticsEnabled = session.Factory.Statistics.IsStatisticsEnabled;
      Stopwatch stopwatch = new Stopwatch();
      if (statisticsEnabled)
        stopwatch.Start();
      CollectionEntry collectionEntry = session.PersistenceContext.GetCollectionEntry(collection);
      if (collectionEntry == null)
        throw new HibernateException("collection was evicted");
      if (collection.WasInitialized)
        return;
      if (DefaultInitializeCollectionEventListener.log.IsDebugEnabled)
        DefaultInitializeCollectionEventListener.log.Debug((object) ("initializing collection " + MessageHelper.InfoString(collectionEntry.LoadedPersister, collectionEntry.LoadedKey, session.Factory)));
      DefaultInitializeCollectionEventListener.log.Debug((object) "checking second-level cache");
      if (this.InitializeCollectionFromCache(collectionEntry.LoadedKey, collectionEntry.LoadedPersister, collection, session))
      {
        DefaultInitializeCollectionEventListener.log.Debug((object) "collection initialized from cache");
      }
      else
      {
        DefaultInitializeCollectionEventListener.log.Debug((object) "collection not cached");
        collectionEntry.LoadedPersister.Initialize(collectionEntry.LoadedKey, session);
        DefaultInitializeCollectionEventListener.log.Debug((object) "collection initialized");
        if (!statisticsEnabled)
          return;
        stopwatch.Stop();
        session.Factory.StatisticsImplementor.FetchCollection(collectionEntry.LoadedPersister.Role, stopwatch.Elapsed);
      }
    }

    private bool InitializeCollectionFromCache(
      object id,
      ICollectionPersister persister,
      IPersistentCollection collection,
      ISessionImplementor source)
    {
      if (source.EnabledFilters.Count != 0 && persister.IsAffectedByEnabledFilters(source))
      {
        DefaultInitializeCollectionEventListener.log.Debug((object) "disregarding cached version (if any) of collection due to enabled filters ");
        return false;
      }
      if (!persister.HasCache || (source.CacheMode & CacheMode.Get) != CacheMode.Get)
        return false;
      ISessionFactoryImplementor factory = source.Factory;
      CacheKey key = new CacheKey(id, persister.KeyType, persister.Role, source.EntityMode, factory);
      object map = persister.Cache.Get(key, source.Timestamp);
      if (factory.Statistics.IsStatisticsEnabled)
      {
        if (map == null)
          factory.StatisticsImplementor.SecondLevelCacheMiss(persister.Cache.RegionName);
        else
          factory.StatisticsImplementor.SecondLevelCacheHit(persister.Cache.RegionName);
      }
      if (map == null)
        DefaultInitializeCollectionEventListener.log.DebugFormat("Collection cache miss: {0}", (object) key);
      else
        DefaultInitializeCollectionEventListener.log.DebugFormat("Collection cache hit: {0}", (object) key);
      if (map == null)
        return false;
      IPersistenceContext persistenceContext = source.PersistenceContext;
      ((CollectionCacheEntry) persister.CacheEntryStructure.Destructure(map, factory)).Assemble(collection, persister, persistenceContext.GetCollectionOwner(id, persister));
      persistenceContext.GetCollectionEntry(collection).PostInitialize(collection);
      return true;
    }
  }
}
