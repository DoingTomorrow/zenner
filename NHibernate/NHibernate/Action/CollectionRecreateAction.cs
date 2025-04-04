// Decompiled with JetBrains decompiler
// Type: NHibernate.Action.CollectionRecreateAction
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
  public sealed class CollectionRecreateAction(
    IPersistentCollection collection,
    ICollectionPersister persister,
    object key,
    ISessionImplementor session) : CollectionAction(persister, collection, key, session)
  {
    public override void Execute()
    {
      bool statisticsEnabled = this.Session.Factory.Statistics.IsStatisticsEnabled;
      Stopwatch stopwatch = (Stopwatch) null;
      if (statisticsEnabled)
        stopwatch = Stopwatch.StartNew();
      IPersistentCollection collection = this.Collection;
      this.PreRecreate();
      this.Persister.Recreate(collection, this.Key, this.Session);
      this.Session.PersistenceContext.GetCollectionEntry(collection).AfterAction(collection);
      this.Evict();
      this.PostRecreate();
      if (!statisticsEnabled)
        return;
      stopwatch.Stop();
      this.Session.Factory.StatisticsImplementor.RecreateCollection(this.Persister.Role, stopwatch.Elapsed);
    }

    private void PreRecreate()
    {
      IPreCollectionRecreateEventListener[] recreateEventListeners = this.Session.Listeners.PreCollectionRecreateEventListeners;
      if (recreateEventListeners.Length <= 0)
        return;
      PreCollectionRecreateEvent @event = new PreCollectionRecreateEvent(this.Persister, this.Collection, (IEventSource) this.Session);
      for (int index = 0; index < recreateEventListeners.Length; ++index)
        recreateEventListeners[index].OnPreRecreateCollection(@event);
    }

    private void PostRecreate()
    {
      IPostCollectionRecreateEventListener[] recreateEventListeners = this.Session.Listeners.PostCollectionRecreateEventListeners;
      if (recreateEventListeners.Length <= 0)
        return;
      PostCollectionRecreateEvent @event = new PostCollectionRecreateEvent(this.Persister, this.Collection, (IEventSource) this.Session);
      for (int index = 0; index < recreateEventListeners.Length; ++index)
        recreateEventListeners[index].OnPostRecreateCollection(@event);
    }
  }
}
