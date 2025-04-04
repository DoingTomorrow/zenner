// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.AbstractFlushingEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Action;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public abstract class AbstractFlushingEventListener
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (AbstractFlushingEventListener));

    protected virtual object Anything => (object) null;

    protected virtual CascadingAction CascadingAction => CascadingAction.SaveUpdate;

    protected virtual void FlushEverythingToExecutions(FlushEvent @event)
    {
      AbstractFlushingEventListener.log.Debug((object) "flushing session");
      IEventSource session = @event.Session;
      IPersistenceContext persistenceContext = session.PersistenceContext;
      session.Interceptor.PreFlush((ICollection) persistenceContext.EntitiesByKey.Values);
      this.PrepareEntityFlushes(session);
      this.PrepareCollectionFlushes((ISessionImplementor) session);
      persistenceContext.Flushing = true;
      try
      {
        this.FlushEntities(@event);
        this.FlushCollections(session);
      }
      finally
      {
        persistenceContext.Flushing = false;
      }
      if (!AbstractFlushingEventListener.log.IsDebugEnabled)
        return;
      StringBuilder stringBuilder1 = new StringBuilder(100);
      stringBuilder1.Append("Flushed: ").Append(session.ActionQueue.InsertionsCount).Append(" insertions, ").Append(session.ActionQueue.UpdatesCount).Append(" updates, ").Append(session.ActionQueue.DeletionsCount).Append(" deletions to ").Append(persistenceContext.EntityEntries.Count).Append(" objects");
      AbstractFlushingEventListener.log.Debug((object) stringBuilder1.ToString());
      StringBuilder stringBuilder2 = new StringBuilder(100);
      stringBuilder2.Append("Flushed: ").Append(session.ActionQueue.CollectionCreationsCount).Append(" (re)creations, ").Append(session.ActionQueue.CollectionUpdatesCount).Append(" updates, ").Append(session.ActionQueue.CollectionRemovalsCount).Append(" removals to ").Append(persistenceContext.CollectionEntries.Count).Append(" collections");
      AbstractFlushingEventListener.log.Debug((object) stringBuilder2.ToString());
      new Printer(session.Factory).ToString((IEnumerator) persistenceContext.EntitiesByKey.Values.GetEnumerator(), session.EntityMode);
    }

    protected virtual void FlushCollections(IEventSource session)
    {
      AbstractFlushingEventListener.log.Debug((object) "Processing unreferenced collections");
      foreach (DictionaryEntry entry in (IEnumerable) IdentityMap.Entries(session.PersistenceContext.CollectionEntries))
      {
        CollectionEntry collectionEntry = (CollectionEntry) entry.Value;
        if (!collectionEntry.IsReached && !collectionEntry.IsIgnore)
          NHibernate.Engine.Collections.ProcessUnreachableCollection((IPersistentCollection) entry.Key, (ISessionImplementor) session);
      }
      AbstractFlushingEventListener.log.Debug((object) "Scheduling collection removes/(re)creates/updates");
      ICollection collection = IdentityMap.Entries(session.PersistenceContext.CollectionEntries);
      ActionQueue actionQueue = session.ActionQueue;
      foreach (DictionaryEntry dictionaryEntry in (IEnumerable) collection)
      {
        IPersistentCollection key = (IPersistentCollection) dictionaryEntry.Key;
        CollectionEntry collectionEntry = (CollectionEntry) dictionaryEntry.Value;
        if (collectionEntry.IsDorecreate)
        {
          session.Interceptor.OnCollectionRecreate((object) key, collectionEntry.CurrentKey);
          actionQueue.AddAction(new CollectionRecreateAction(key, collectionEntry.CurrentPersister, collectionEntry.CurrentKey, (ISessionImplementor) session));
        }
        if (collectionEntry.IsDoremove)
        {
          session.Interceptor.OnCollectionRemove((object) key, collectionEntry.LoadedKey);
          actionQueue.AddAction(new CollectionRemoveAction(key, collectionEntry.LoadedPersister, collectionEntry.LoadedKey, collectionEntry.IsSnapshotEmpty(key), (ISessionImplementor) session));
        }
        if (collectionEntry.IsDoupdate)
        {
          session.Interceptor.OnCollectionUpdate((object) key, collectionEntry.LoadedKey);
          actionQueue.AddAction(new CollectionUpdateAction(key, collectionEntry.LoadedPersister, collectionEntry.LoadedKey, collectionEntry.IsSnapshotEmpty(key), (ISessionImplementor) session));
        }
      }
      actionQueue.SortCollectionActions();
    }

    protected virtual void FlushEntities(FlushEvent @event)
    {
      AbstractFlushingEventListener.log.Debug((object) "Flushing entities and processing referenced collections");
      IEventSource session = @event.Session;
      foreach (DictionaryEntry concurrentEntry in (IEnumerable) IdentityMap.ConcurrentEntries(session.PersistenceContext.EntityEntries))
      {
        EntityEntry entry = (EntityEntry) concurrentEntry.Value;
        switch (entry.Status)
        {
          case Status.Gone:
          case Status.Loading:
            continue;
          default:
            FlushEntityEvent event1 = new FlushEntityEvent(session, concurrentEntry.Key, entry);
            foreach (IFlushEntityEventListener entityEventListener in session.Listeners.FlushEntityEventListeners)
              entityEventListener.OnFlushEntity(event1);
            continue;
        }
      }
      session.ActionQueue.SortActions();
    }

    protected virtual void PrepareCollectionFlushes(ISessionImplementor session)
    {
      AbstractFlushingEventListener.log.Debug((object) "dirty checking collections");
      foreach (DictionaryEntry entry in (IEnumerable) IdentityMap.Entries(session.PersistenceContext.CollectionEntries))
        ((CollectionEntry) entry.Value).PreFlush((IPersistentCollection) entry.Key);
    }

    protected virtual void PrepareEntityFlushes(IEventSource session)
    {
      AbstractFlushingEventListener.log.Debug((object) "processing flush-time cascades");
      foreach (DictionaryEntry concurrentEntry in (IEnumerable) IdentityMap.ConcurrentEntries(session.PersistenceContext.EntityEntries))
      {
        EntityEntry entityEntry = (EntityEntry) concurrentEntry.Value;
        switch (entityEntry.Status)
        {
          case Status.Loaded:
          case Status.Saving:
          case Status.ReadOnly:
            this.CascadeOnFlush(session, entityEntry.Persister, concurrentEntry.Key, this.Anything);
            continue;
          default:
            continue;
        }
      }
    }

    protected virtual void CascadeOnFlush(
      IEventSource session,
      IEntityPersister persister,
      object key,
      object anything)
    {
      session.PersistenceContext.IncrementCascadeLevel();
      try
      {
        new Cascade(this.CascadingAction, CascadePoint.AfterUpdate, session).CascadeOn(persister, key, anything);
      }
      finally
      {
        session.PersistenceContext.DecrementCascadeLevel();
      }
    }

    protected virtual void PerformExecutions(IEventSource session)
    {
      if (AbstractFlushingEventListener.log.IsDebugEnabled)
        AbstractFlushingEventListener.log.Debug((object) "executing flush");
      try
      {
        session.ConnectionManager.FlushBeginning();
        session.ActionQueue.PrepareActions();
        session.ActionQueue.ExecuteActions();
      }
      catch (HibernateException ex)
      {
        if (AbstractFlushingEventListener.log.IsErrorEnabled)
          AbstractFlushingEventListener.log.Error((object) "Could not synchronize database state with session", (Exception) ex);
        throw;
      }
      finally
      {
        session.ConnectionManager.FlushEnding();
      }
    }

    protected virtual void PostFlush(ISessionImplementor session)
    {
      if (AbstractFlushingEventListener.log.IsDebugEnabled)
        AbstractFlushingEventListener.log.Debug((object) "post flush");
      IPersistenceContext persistenceContext = session.PersistenceContext;
      persistenceContext.CollectionsByKey.Clear();
      persistenceContext.BatchFetchQueue.ClearSubselects();
      IDictionary collectionEntries = persistenceContext.CollectionEntries;
      List<IPersistentCollection> persistentCollectionList = new List<IPersistentCollection>(collectionEntries.Count);
      foreach (DictionaryEntry dictionaryEntry in collectionEntries)
      {
        CollectionEntry collectionEntry = (CollectionEntry) dictionaryEntry.Value;
        IPersistentCollection key1 = (IPersistentCollection) dictionaryEntry.Key;
        collectionEntry.PostFlush(key1);
        if (collectionEntry.LoadedPersister == null)
        {
          persistentCollectionList.Add(key1);
        }
        else
        {
          CollectionKey key2 = new CollectionKey(collectionEntry.LoadedPersister, collectionEntry.LoadedKey, session.EntityMode);
          persistenceContext.CollectionsByKey[key2] = key1;
        }
      }
      foreach (IPersistentCollection key in persistentCollectionList)
        persistenceContext.CollectionEntries.Remove((object) key);
      session.Interceptor.PostFlush((ICollection) persistenceContext.EntitiesByKey.Values);
    }
  }
}
