// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Loading.CollectionLoadContext
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Collection;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace NHibernate.Engine.Loading
{
  public class CollectionLoadContext
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (CollectionLoadContext));
    private readonly LoadContexts loadContexts;
    private readonly IDataReader resultSet;
    private readonly ISet<CollectionKey> localLoadingCollectionKeys = (ISet<CollectionKey>) new HashedSet<CollectionKey>();

    public CollectionLoadContext(LoadContexts loadContexts, IDataReader resultSet)
    {
      this.loadContexts = loadContexts;
      this.resultSet = resultSet;
    }

    public LoadContexts LoadContext => this.loadContexts;

    public IDataReader ResultSet => this.resultSet;

    public IPersistentCollection GetLoadingCollection(ICollectionPersister persister, object key)
    {
      EntityMode entityMode = this.loadContexts.PersistenceContext.Session.EntityMode;
      CollectionKey collectionKey = new CollectionKey(persister, key, entityMode);
      if (CollectionLoadContext.log.IsDebugEnabled)
        CollectionLoadContext.log.Debug((object) ("starting attempt to find loading collection [" + MessageHelper.InfoString(persister.Role, key) + "]"));
      LoadingCollectionEntry loadingCollectionEntry = this.loadContexts.LocateLoadingCollectionEntry(collectionKey);
      if (loadingCollectionEntry == null)
      {
        IPersistentCollection collection = this.loadContexts.PersistenceContext.GetCollection(collectionKey);
        if (collection != null)
        {
          if (collection.WasInitialized)
          {
            CollectionLoadContext.log.Debug((object) "collection already initialized; ignoring");
            return (IPersistentCollection) null;
          }
          CollectionLoadContext.log.Debug((object) "collection not yet initialized; initializing");
        }
        else
        {
          object collectionOwner = this.loadContexts.PersistenceContext.GetCollectionOwner(key, persister);
          if (collectionOwner != null && this.loadContexts.PersistenceContext.GetEntry(collectionOwner).Status != Status.Loading && entityMode != EntityMode.Xml)
          {
            CollectionLoadContext.log.Debug((object) "owning entity already loaded; ignoring");
            return (IPersistentCollection) null;
          }
          if (CollectionLoadContext.log.IsDebugEnabled)
            CollectionLoadContext.log.Debug((object) ("instantiating new collection [key=" + key + ", rs=" + (object) this.resultSet + "]"));
          collection = persister.CollectionType.Instantiate(this.loadContexts.PersistenceContext.Session, persister, key);
        }
        collection.BeforeInitialize(persister, -1);
        collection.BeginRead();
        this.localLoadingCollectionKeys.Add(collectionKey);
        this.loadContexts.RegisterLoadingCollectionXRef(collectionKey, new LoadingCollectionEntry(this.resultSet, persister, key, collection));
        return collection;
      }
      if (loadingCollectionEntry.ResultSet == this.resultSet)
      {
        CollectionLoadContext.log.Debug((object) "found loading collection bound to current result set processing; reading row");
        return loadingCollectionEntry.Collection;
      }
      CollectionLoadContext.log.Debug((object) "collection is already being initialized; ignoring row");
      return (IPersistentCollection) null;
    }

    public void EndLoadingCollections(ICollectionPersister persister)
    {
      if (!this.loadContexts.HasLoadingCollectionEntries && this.localLoadingCollectionKeys.Count == 0)
        return;
      List<CollectionKey> c = new List<CollectionKey>();
      List<LoadingCollectionEntry> matchedCollectionEntries = new List<LoadingCollectionEntry>();
      foreach (CollectionKey loadingCollectionKey in (IEnumerable<CollectionKey>) this.localLoadingCollectionKeys)
      {
        ISessionImplementor session = this.LoadContext.PersistenceContext.Session;
        LoadingCollectionEntry loadingCollectionEntry = this.loadContexts.LocateLoadingCollectionEntry(loadingCollectionKey);
        if (loadingCollectionEntry == null)
          CollectionLoadContext.log.Warn((object) ("In CollectionLoadContext#endLoadingCollections, localLoadingCollectionKeys contained [" + (object) loadingCollectionKey + "], but no LoadingCollectionEntry was found in loadContexts"));
        else if (loadingCollectionEntry.ResultSet == this.resultSet && loadingCollectionEntry.Persister == persister)
        {
          matchedCollectionEntries.Add(loadingCollectionEntry);
          if (loadingCollectionEntry.Collection.Owner == null)
            session.PersistenceContext.AddUnownedCollection(new CollectionKey(persister, loadingCollectionEntry.Key, session.EntityMode), loadingCollectionEntry.Collection);
          if (CollectionLoadContext.log.IsDebugEnabled)
            CollectionLoadContext.log.Debug((object) ("removing collection load entry [" + (object) loadingCollectionEntry + "]"));
          this.loadContexts.UnregisterLoadingCollectionXRef(loadingCollectionKey);
          c.Add(loadingCollectionKey);
        }
      }
      this.localLoadingCollectionKeys.RemoveAll((ICollection<CollectionKey>) c);
      this.EndLoadingCollections(persister, (IList<LoadingCollectionEntry>) matchedCollectionEntries);
      if (this.localLoadingCollectionKeys.Count != 0)
        return;
      this.loadContexts.Cleanup(this.resultSet);
    }

    private void EndLoadingCollections(
      ICollectionPersister persister,
      IList<LoadingCollectionEntry> matchedCollectionEntries)
    {
      if (matchedCollectionEntries == null || matchedCollectionEntries.Count == 0)
      {
        if (!CollectionLoadContext.log.IsDebugEnabled)
          return;
        CollectionLoadContext.log.Debug((object) ("no collections were found in result set for role: " + persister.Role));
      }
      else
      {
        int count = matchedCollectionEntries.Count;
        if (CollectionLoadContext.log.IsDebugEnabled)
          CollectionLoadContext.log.Debug((object) (count.ToString() + " collections were found in result set for role: " + persister.Role));
        for (int index = 0; index < count; ++index)
          this.EndLoadingCollection(matchedCollectionEntries[index], persister);
        if (!CollectionLoadContext.log.IsDebugEnabled)
          return;
        CollectionLoadContext.log.Debug((object) (count.ToString() + " collections initialized for role: " + persister.Role));
      }
    }

    private void EndLoadingCollection(LoadingCollectionEntry lce, ICollectionPersister persister)
    {
      if (CollectionLoadContext.log.IsDebugEnabled)
        CollectionLoadContext.log.Debug((object) ("ending loading collection [" + (object) lce + "]"));
      ISessionImplementor session = this.LoadContext.PersistenceContext.Session;
      EntityMode entityMode = session.EntityMode;
      bool statisticsEnabled = session.Factory.Statistics.IsStatisticsEnabled;
      Stopwatch stopwatch = new Stopwatch();
      if (statisticsEnabled)
        stopwatch.Start();
      bool flag = lce.Collection.EndRead(persister);
      if (persister.CollectionType.HasHolder(entityMode))
        this.LoadContext.PersistenceContext.AddCollectionHolder(lce.Collection);
      CollectionEntry collectionEntry = this.LoadContext.PersistenceContext.GetCollectionEntry(lce.Collection);
      if (collectionEntry == null)
        collectionEntry = this.LoadContext.PersistenceContext.AddInitializedCollection(persister, lce.Collection, lce.Key);
      else
        collectionEntry.PostInitialize(lce.Collection);
      if (flag && persister.HasCache && (session.CacheMode & CacheMode.Put) == CacheMode.Put && !collectionEntry.IsDoremove)
        this.AddCollectionToCache(lce, persister);
      if (CollectionLoadContext.log.IsDebugEnabled)
        CollectionLoadContext.log.Debug((object) ("collection fully initialized: " + MessageHelper.InfoString(persister, lce.Key, session.Factory)));
      if (!statisticsEnabled)
        return;
      stopwatch.Stop();
      session.Factory.StatisticsImplementor.LoadCollection(persister.Role, stopwatch.Elapsed);
    }

    private void AddCollectionToCache(LoadingCollectionEntry lce, ICollectionPersister persister)
    {
      ISessionImplementor session = this.LoadContext.PersistenceContext.Session;
      ISessionFactoryImplementor factory = session.Factory;
      if (CollectionLoadContext.log.IsDebugEnabled)
        CollectionLoadContext.log.Debug((object) ("Caching collection: " + MessageHelper.InfoString(persister, lce.Key, factory)));
      if (session.EnabledFilters.Count != 0 && persister.IsAffectedByEnabledFilters(session))
      {
        CollectionLoadContext.log.Debug((object) "Refusing to add to cache due to enabled filters");
      }
      else
      {
        IComparer versionComparer;
        object version;
        if (persister.IsVersioned)
        {
          versionComparer = persister.OwnerEntityPersister.VersionType.Comparator;
          version = this.LoadContext.PersistenceContext.GetEntry(this.LoadContext.PersistenceContext.GetCollectionOwner(lce.Key, persister)).Version;
        }
        else
        {
          version = (object) null;
          versionComparer = (IComparer) null;
        }
        CollectionCacheEntry collectionCacheEntry = new CollectionCacheEntry(lce.Collection, persister);
        CacheKey key = new CacheKey(lce.Key, persister.KeyType, persister.Role, session.EntityMode, factory);
        if (!persister.Cache.Put(key, persister.CacheEntryStructure.Structure((object) collectionCacheEntry), session.Timestamp, version, versionComparer, factory.Settings.IsMinimalPutsEnabled && session.CacheMode != CacheMode.Refresh) || !factory.Statistics.IsStatisticsEnabled)
          return;
        factory.StatisticsImplementor.SecondLevelCachePut(persister.Cache.RegionName);
      }
    }

    internal void Cleanup()
    {
      if (this.localLoadingCollectionKeys.Count != 0)
        CollectionLoadContext.log.Warn((object) ("On CollectionLoadContext#cleanup, localLoadingCollectionKeys contained [" + (object) this.localLoadingCollectionKeys.Count + "] entries"));
      this.LoadContext.CleanupCollectionXRefs((IEnumerable) this.localLoadingCollectionKeys);
      this.localLoadingCollectionKeys.Clear();
    }

    public override string ToString() => base.ToString() + "<rs=" + (object) this.ResultSet + ">";
  }
}
