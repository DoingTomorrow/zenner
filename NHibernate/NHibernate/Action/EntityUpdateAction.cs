// Decompiled with JetBrains decompiler
// Type: NHibernate.Action.EntityUpdateAction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Cache.Access;
using NHibernate.Cache.Entry;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System;
using System.Diagnostics;

#nullable disable
namespace NHibernate.Action
{
  [Serializable]
  public sealed class EntityUpdateAction : EntityAction
  {
    private readonly object[] state;
    private readonly object[] previousState;
    private object previousVersion;
    private object nextVersion;
    private readonly int[] dirtyFields;
    private readonly bool hasDirtyCollection;
    private object cacheEntry;
    private ISoftLock slock;

    public EntityUpdateAction(
      object id,
      object[] state,
      int[] dirtyProperties,
      bool hasDirtyCollection,
      object[] previousState,
      object previousVersion,
      object nextVersion,
      object instance,
      IEntityPersister persister,
      ISessionImplementor session)
      : base(session, id, instance, persister)
    {
      this.state = state;
      this.previousState = previousState;
      this.previousVersion = previousVersion;
      this.nextVersion = nextVersion;
      this.dirtyFields = dirtyProperties;
      this.hasDirtyCollection = hasDirtyCollection;
    }

    protected internal override bool HasPostCommitEventListeners
    {
      get => this.Session.Listeners.PostCommitUpdateEventListeners.Length > 0;
    }

    public override void Execute()
    {
      ISessionImplementor session = this.Session;
      object id = this.Id;
      IEntityPersister persister = this.Persister;
      object instance = this.Instance;
      bool statisticsEnabled = this.Session.Factory.Statistics.IsStatisticsEnabled;
      Stopwatch stopwatch = (Stopwatch) null;
      if (statisticsEnabled)
        stopwatch = Stopwatch.StartNew();
      bool flag = this.PreUpdate();
      ISessionFactoryImplementor factory = this.Session.Factory;
      if (persister.IsVersionPropertyGenerated)
        this.previousVersion = persister.GetVersion(instance, session.EntityMode);
      CacheKey key = (CacheKey) null;
      if (persister.HasCache)
      {
        key = new CacheKey(id, persister.IdentifierType, persister.RootEntityName, session.EntityMode, factory);
        this.slock = persister.Cache.Lock(key, this.previousVersion);
      }
      if (!flag)
        persister.Update(id, this.state, this.dirtyFields, this.hasDirtyCollection, this.previousState, this.previousVersion, instance, (object) null, session);
      EntityEntry entry = this.Session.PersistenceContext.GetEntry(instance);
      if (entry == null)
        throw new AssertionFailure("Possible nonthreadsafe access to session");
      if (entry.Status == Status.Loaded || persister.IsVersionPropertyGenerated)
      {
        TypeHelper.DeepCopy(this.state, persister.PropertyTypes, persister.PropertyCheckability, this.state, this.Session);
        if (persister.HasUpdateGeneratedProperties)
        {
          persister.ProcessUpdateGeneratedProperties(id, instance, this.state, this.Session);
          if (persister.IsVersionPropertyGenerated)
            this.nextVersion = Versioning.GetVersion(this.state, persister);
        }
        entry.PostUpdate(instance, this.state, this.nextVersion);
      }
      if (persister.HasCache)
      {
        if (persister.IsCacheInvalidationRequired || entry.Status != Status.Loaded)
        {
          persister.Cache.Evict(key);
        }
        else
        {
          CacheEntry cacheEntry = new CacheEntry(this.state, persister, persister.HasUninitializedLazyProperties(instance, session.EntityMode), this.nextVersion, this.Session, instance);
          this.cacheEntry = persister.CacheEntryStructure.Structure((object) cacheEntry);
          if (persister.Cache.Update(key, this.cacheEntry, this.nextVersion, this.previousVersion) && factory.Statistics.IsStatisticsEnabled)
            factory.StatisticsImplementor.SecondLevelCachePut(this.Persister.Cache.RegionName);
        }
      }
      this.PostUpdate();
      if (!statisticsEnabled || flag)
        return;
      stopwatch.Stop();
      factory.StatisticsImplementor.UpdateEntity(this.Persister.EntityName, stopwatch.Elapsed);
    }

    protected override void AfterTransactionCompletionProcessImpl(bool success)
    {
      IEntityPersister persister = this.Persister;
      if (persister.HasCache)
      {
        CacheKey key = new CacheKey(this.Id, persister.IdentifierType, persister.RootEntityName, this.Session.EntityMode, this.Session.Factory);
        if (success && this.cacheEntry != null)
        {
          if (persister.Cache.AfterUpdate(key, this.cacheEntry, this.nextVersion, this.slock) && this.Session.Factory.Statistics.IsStatisticsEnabled)
            this.Session.Factory.StatisticsImplementor.SecondLevelCachePut(this.Persister.Cache.RegionName);
        }
        else
          persister.Cache.Release(key, this.slock);
      }
      if (!success)
        return;
      this.PostCommitUpdate();
    }

    private void PostUpdate()
    {
      IPostUpdateEventListener[] updateEventListeners = this.Session.Listeners.PostUpdateEventListeners;
      if (updateEventListeners.Length <= 0)
        return;
      PostUpdateEvent @event = new PostUpdateEvent(this.Instance, this.Id, this.state, this.previousState, this.Persister, (IEventSource) this.Session);
      foreach (IPostUpdateEventListener updateEventListener in updateEventListeners)
        updateEventListener.OnPostUpdate(@event);
    }

    private void PostCommitUpdate()
    {
      IPostUpdateEventListener[] updateEventListeners = this.Session.Listeners.PostCommitUpdateEventListeners;
      if (updateEventListeners.Length <= 0)
        return;
      PostUpdateEvent @event = new PostUpdateEvent(this.Instance, this.Id, this.state, this.previousState, this.Persister, (IEventSource) this.Session);
      foreach (IPostUpdateEventListener updateEventListener in updateEventListeners)
        updateEventListener.OnPostUpdate(@event);
    }

    private bool PreUpdate()
    {
      IPreUpdateEventListener[] updateEventListeners = this.Session.Listeners.PreUpdateEventListeners;
      bool flag = false;
      if (updateEventListeners.Length > 0)
      {
        PreUpdateEvent @event = new PreUpdateEvent(this.Instance, this.Id, this.state, this.previousState, this.Persister, (IEventSource) this.Session);
        foreach (IPreUpdateEventListener updateEventListener in updateEventListeners)
          flag |= updateEventListener.OnPreUpdate(@event);
      }
      return flag;
    }
  }
}
