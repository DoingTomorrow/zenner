// Decompiled with JetBrains decompiler
// Type: NHibernate.Action.CollectionUpdateAction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using System;
using System.Diagnostics;

#nullable disable
namespace NHibernate.Action
{
  [Serializable]
  public sealed class CollectionUpdateAction : CollectionAction
  {
    private readonly bool emptySnapshot;

    public CollectionUpdateAction(
      IPersistentCollection collection,
      ICollectionPersister persister,
      object key,
      bool emptySnapshot,
      ISessionImplementor session)
      : base(persister, collection, key, session)
    {
      this.emptySnapshot = emptySnapshot;
    }

    public override void Execute()
    {
      object key = this.Key;
      ISessionImplementor session = this.Session;
      ICollectionPersister persister = this.Persister;
      IPersistentCollection collection = this.Collection;
      bool flag = persister.IsAffectedByEnabledFilters(session);
      bool statisticsEnabled = session.Factory.Statistics.IsStatisticsEnabled;
      Stopwatch stopwatch = (Stopwatch) null;
      if (statisticsEnabled)
        stopwatch = Stopwatch.StartNew();
      this.PreUpdate();
      if (!collection.WasInitialized)
      {
        if (!collection.HasQueuedOperations)
          throw new AssertionFailure("no queued adds");
      }
      else if (!flag && collection.Empty)
      {
        if (!this.emptySnapshot)
          persister.Remove(key, session);
      }
      else if (collection.NeedsRecreate(persister))
      {
        if (flag)
          throw new HibernateException("cannot recreate collection while filter is enabled: " + MessageHelper.InfoString(persister, key, persister.Factory));
        if (!this.emptySnapshot)
          persister.Remove(key, session);
        persister.Recreate(collection, key, session);
      }
      else
      {
        persister.DeleteRows(collection, key, session);
        persister.UpdateRows(collection, key, session);
        persister.InsertRows(collection, key, session);
      }
      this.Session.PersistenceContext.GetCollectionEntry(collection).AfterAction(collection);
      this.Evict();
      this.PostUpdate();
      if (!statisticsEnabled)
        return;
      stopwatch.Stop();
      this.Session.Factory.StatisticsImplementor.UpdateCollection(this.Persister.Role, stopwatch.Elapsed);
    }

    private void PreUpdate()
    {
      IPreCollectionUpdateEventListener[] updateEventListeners = this.Session.Listeners.PreCollectionUpdateEventListeners;
      if (updateEventListeners.Length <= 0)
        return;
      PreCollectionUpdateEvent @event = new PreCollectionUpdateEvent(this.Persister, this.Collection, (IEventSource) this.Session);
      for (int index = 0; index < updateEventListeners.Length; ++index)
        updateEventListeners[index].OnPreUpdateCollection(@event);
    }

    private void PostUpdate()
    {
      IPostCollectionUpdateEventListener[] updateEventListeners = this.Session.Listeners.PostCollectionUpdateEventListeners;
      if (updateEventListeners.Length <= 0)
        return;
      PostCollectionUpdateEvent @event = new PostCollectionUpdateEvent(this.Persister, this.Collection, (IEventSource) this.Session);
      for (int index = 0; index < updateEventListeners.Length; ++index)
        updateEventListeners[index].OnPostUpdateCollection(@event);
    }

    public override BeforeTransactionCompletionProcessDelegate BeforeTransactionCompletionProcess
    {
      get => (BeforeTransactionCompletionProcessDelegate) null;
    }

    public override AfterTransactionCompletionProcessDelegate AfterTransactionCompletionProcess
    {
      get
      {
        return (AfterTransactionCompletionProcessDelegate) (success =>
        {
          if (!this.Persister.HasCache)
            return;
          CacheKey key = new CacheKey(this.Key, this.Persister.KeyType, this.Persister.Role, this.Session.EntityMode, this.Session.Factory);
          if (success)
          {
            if (!this.Collection.WasInitialized || !this.Session.PersistenceContext.ContainsCollection(this.Collection))
              return;
            CollectionCacheEntry collectionCacheEntry = new CollectionCacheEntry(this.Collection, this.Persister);
            if (!this.Persister.Cache.AfterUpdate(key, (object) collectionCacheEntry, (object) null, this.Lock) || !this.Session.Factory.Statistics.IsStatisticsEnabled)
              return;
            this.Session.Factory.StatisticsImplementor.SecondLevelCachePut(this.Persister.Cache.RegionName);
          }
          else
            this.Persister.Cache.Release(key, this.Lock);
        });
      }
    }
  }
}
