// Decompiled with JetBrains decompiler
// Type: NHibernate.Action.CollectionRemoveAction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Collection;
using System;
using System.Diagnostics;

#nullable disable
namespace NHibernate.Action
{
  [Serializable]
  public sealed class CollectionRemoveAction : CollectionAction
  {
    private readonly bool emptySnapshot;
    private readonly object affectedOwner;

    public CollectionRemoveAction(
      IPersistentCollection collection,
      ICollectionPersister persister,
      object id,
      bool emptySnapshot,
      ISessionImplementor session)
      : base(persister, collection, id, session)
    {
      if (collection == null)
        throw new AssertionFailure("collection == null");
      this.emptySnapshot = emptySnapshot;
      this.affectedOwner = session.PersistenceContext.GetLoadedCollectionOwnerOrNull(collection);
    }

    public CollectionRemoveAction(
      object affectedOwner,
      ICollectionPersister persister,
      object id,
      bool emptySnapshot,
      ISessionImplementor session)
      : base(persister, (IPersistentCollection) null, id, session)
    {
      if (affectedOwner == null)
        throw new AssertionFailure("affectedOwner == null");
      this.emptySnapshot = emptySnapshot;
      this.affectedOwner = affectedOwner;
    }

    public override void Execute()
    {
      bool statisticsEnabled = this.Session.Factory.Statistics.IsStatisticsEnabled;
      Stopwatch stopwatch = (Stopwatch) null;
      if (statisticsEnabled)
        stopwatch = Stopwatch.StartNew();
      this.PreRemove();
      if (!this.emptySnapshot)
        this.Persister.Remove(this.Key, this.Session);
      IPersistentCollection collection = this.Collection;
      if (collection != null)
        this.Session.PersistenceContext.GetCollectionEntry(collection).AfterAction(collection);
      this.Evict();
      this.PostRemove();
      if (!statisticsEnabled)
        return;
      stopwatch.Stop();
      this.Session.Factory.StatisticsImplementor.RemoveCollection(this.Persister.Role, stopwatch.Elapsed);
    }

    private void PreRemove()
    {
      IPreCollectionRemoveEventListener[] removeEventListeners = this.Session.Listeners.PreCollectionRemoveEventListeners;
      if (removeEventListeners.Length <= 0)
        return;
      PreCollectionRemoveEvent @event = new PreCollectionRemoveEvent(this.Persister, this.Collection, (IEventSource) this.Session, this.affectedOwner);
      for (int index = 0; index < removeEventListeners.Length; ++index)
        removeEventListeners[index].OnPreRemoveCollection(@event);
    }

    private void PostRemove()
    {
      IPostCollectionRemoveEventListener[] removeEventListeners = this.Session.Listeners.PostCollectionRemoveEventListeners;
      if (removeEventListeners.Length <= 0)
        return;
      PostCollectionRemoveEvent @event = new PostCollectionRemoveEvent(this.Persister, this.Collection, (IEventSource) this.Session, this.affectedOwner);
      for (int index = 0; index < removeEventListeners.Length; ++index)
        removeEventListeners[index].OnPostRemoveCollection(@event);
    }

    public override int CompareTo(CollectionAction other) => 0;
  }
}
