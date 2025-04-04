// Decompiled with JetBrains decompiler
// Type: NHibernate.Action.EntityDeleteAction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Cache.Access;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using System;
using System.Diagnostics;

#nullable disable
namespace NHibernate.Action
{
  [Serializable]
  public sealed class EntityDeleteAction : EntityAction
  {
    private readonly object[] state;
    private readonly object version;
    private readonly bool isCascadeDeleteEnabled;
    private ISoftLock sLock;

    public EntityDeleteAction(
      object id,
      object[] state,
      object version,
      object instance,
      IEntityPersister persister,
      bool isCascadeDeleteEnabled,
      ISessionImplementor session)
      : base(session, id, instance, persister)
    {
      this.state = state;
      this.version = version;
      this.isCascadeDeleteEnabled = isCascadeDeleteEnabled;
    }

    protected internal override bool HasPostCommitEventListeners
    {
      get => this.Session.Listeners.PostCommitDeleteEventListeners.Length > 0;
    }

    public override void Execute()
    {
      object id = this.Id;
      IEntityPersister persister = this.Persister;
      ISessionImplementor session = this.Session;
      object instance = this.Instance;
      bool statisticsEnabled = this.Session.Factory.Statistics.IsStatisticsEnabled;
      Stopwatch stopwatch = (Stopwatch) null;
      if (statisticsEnabled)
        stopwatch = Stopwatch.StartNew();
      bool flag = this.PreDelete();
      object version = this.version;
      if (persister.IsVersionPropertyGenerated)
        version = persister.GetVersion(instance, session.EntityMode);
      CacheKey key1;
      if (persister.HasCache)
      {
        key1 = new CacheKey(id, persister.IdentifierType, persister.RootEntityName, session.EntityMode, session.Factory);
        this.sLock = persister.Cache.Lock(key1, this.version);
      }
      else
        key1 = (CacheKey) null;
      if (!this.isCascadeDeleteEnabled && !flag)
        persister.Delete(id, version, instance, session);
      IPersistenceContext persistenceContext = session.PersistenceContext;
      EntityEntry entityEntry = persistenceContext.RemoveEntry(instance);
      if (entityEntry == null)
        throw new AssertionFailure("Possible nonthreadsafe access to session");
      entityEntry.PostDelete();
      EntityKey key2 = new EntityKey(entityEntry.Id, entityEntry.Persister, session.EntityMode);
      persistenceContext.RemoveEntity(key2);
      persistenceContext.RemoveProxy(key2);
      if (persister.HasCache)
        persister.Cache.Evict(key1);
      this.PostDelete();
      if (!statisticsEnabled || flag)
        return;
      stopwatch.Stop();
      this.Session.Factory.StatisticsImplementor.DeleteEntity(this.Persister.EntityName, stopwatch.Elapsed);
    }

    private void PostDelete()
    {
      IPostDeleteEventListener[] deleteEventListeners = this.Session.Listeners.PostDeleteEventListeners;
      if (deleteEventListeners.Length <= 0)
        return;
      PostDeleteEvent @event = new PostDeleteEvent(this.Instance, this.Id, this.state, this.Persister, (IEventSource) this.Session);
      foreach (IPostDeleteEventListener deleteEventListener in deleteEventListeners)
        deleteEventListener.OnPostDelete(@event);
    }

    private bool PreDelete()
    {
      IPreDeleteEventListener[] deleteEventListeners = this.Session.Listeners.PreDeleteEventListeners;
      bool flag = false;
      if (deleteEventListeners.Length > 0)
      {
        PreDeleteEvent @event = new PreDeleteEvent(this.Instance, this.Id, this.state, this.Persister, (IEventSource) this.Session);
        foreach (IPreDeleteEventListener deleteEventListener in deleteEventListeners)
          flag |= deleteEventListener.OnPreDelete(@event);
      }
      return flag;
    }

    protected override void AfterTransactionCompletionProcessImpl(bool success)
    {
      if (this.Persister.HasCache)
        this.Persister.Cache.Release(new CacheKey(this.Id, this.Persister.IdentifierType, this.Persister.RootEntityName, this.Session.EntityMode, this.Session.Factory), this.sLock);
      if (!success)
        return;
      this.PostCommitDelete();
    }

    private void PostCommitDelete()
    {
      IPostDeleteEventListener[] deleteEventListeners = this.Session.Listeners.PostCommitDeleteEventListeners;
      if (deleteEventListeners.Length <= 0)
        return;
      PostDeleteEvent @event = new PostDeleteEvent(this.Instance, this.Id, this.state, this.Persister, (IEventSource) this.Session);
      foreach (IPostDeleteEventListener deleteEventListener in deleteEventListeners)
        deleteEventListener.OnPostDelete(@event);
    }

    public override int CompareTo(EntityAction other) => 0;
  }
}
