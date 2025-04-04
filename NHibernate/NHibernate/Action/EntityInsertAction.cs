// Decompiled with JetBrains decompiler
// Type: NHibernate.Action.EntityInsertAction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using System;
using System.Diagnostics;

#nullable disable
namespace NHibernate.Action
{
  [Serializable]
  public sealed class EntityInsertAction : EntityAction
  {
    private readonly object[] state;
    private object version;
    private object cacheEntry;

    public EntityInsertAction(
      object id,
      object[] state,
      object instance,
      object version,
      IEntityPersister persister,
      ISessionImplementor session)
      : base(session, id, instance, persister)
    {
      this.state = state;
      this.version = version;
    }

    public object[] State => this.state;

    protected internal override bool HasPostCommitEventListeners
    {
      get => this.Session.Listeners.PostCommitInsertEventListeners.Length > 0;
    }

    public override void Execute()
    {
      IEntityPersister persister = this.Persister;
      ISessionImplementor session = this.Session;
      object instance = this.Instance;
      object id = this.Id;
      bool statisticsEnabled = this.Session.Factory.Statistics.IsStatisticsEnabled;
      Stopwatch stopwatch = (Stopwatch) null;
      if (statisticsEnabled)
        stopwatch = Stopwatch.StartNew();
      bool flag = this.PreInsert();
      if (!flag)
      {
        persister.Insert(id, this.state, instance, this.Session);
        EntityEntry entry = this.Session.PersistenceContext.GetEntry(instance);
        if (entry == null)
          throw new AssertionFailure("Possible nonthreadsafe access to session");
        entry.PostInsert();
        if (persister.HasInsertGeneratedProperties)
        {
          persister.ProcessInsertGeneratedProperties(id, instance, this.state, this.Session);
          if (persister.IsVersionPropertyGenerated)
            this.version = Versioning.GetVersion(this.state, persister);
          entry.PostUpdate(instance, this.state, this.version);
        }
      }
      ISessionFactoryImplementor factory = this.Session.Factory;
      if (this.IsCachePutEnabled(persister))
      {
        CacheEntry cacheEntry = new CacheEntry(this.state, persister, persister.HasUninitializedLazyProperties(instance, session.EntityMode), this.version, session, instance);
        this.cacheEntry = persister.CacheEntryStructure.Structure((object) cacheEntry);
        CacheKey key = new CacheKey(id, persister.IdentifierType, persister.RootEntityName, this.Session.EntityMode, this.Session.Factory);
        if (persister.Cache.Insert(key, this.cacheEntry, this.version) && factory.Statistics.IsStatisticsEnabled)
          factory.StatisticsImplementor.SecondLevelCachePut(this.Persister.Cache.RegionName);
      }
      this.PostInsert();
      if (!statisticsEnabled || flag)
        return;
      stopwatch.Stop();
      factory.StatisticsImplementor.InsertEntity(this.Persister.EntityName, stopwatch.Elapsed);
    }

    protected override void AfterTransactionCompletionProcessImpl(bool success)
    {
      IEntityPersister persister = this.Persister;
      if (success && this.IsCachePutEnabled(persister))
      {
        CacheKey key = new CacheKey(this.Id, persister.IdentifierType, persister.RootEntityName, this.Session.EntityMode, this.Session.Factory);
        if (persister.Cache.AfterInsert(key, this.cacheEntry, this.version) && this.Session.Factory.Statistics.IsStatisticsEnabled)
          this.Session.Factory.StatisticsImplementor.SecondLevelCachePut(this.Persister.Cache.RegionName);
      }
      if (!success)
        return;
      this.PostCommitInsert();
    }

    private void PostInsert()
    {
      IPostInsertEventListener[] insertEventListeners = this.Session.Listeners.PostInsertEventListeners;
      if (insertEventListeners.Length <= 0)
        return;
      PostInsertEvent @event = new PostInsertEvent(this.Instance, this.Id, this.state, this.Persister, (IEventSource) this.Session);
      foreach (IPostInsertEventListener insertEventListener in insertEventListeners)
        insertEventListener.OnPostInsert(@event);
    }

    private void PostCommitInsert()
    {
      IPostInsertEventListener[] insertEventListeners = this.Session.Listeners.PostCommitInsertEventListeners;
      if (insertEventListeners.Length <= 0)
        return;
      PostInsertEvent @event = new PostInsertEvent(this.Instance, this.Id, this.state, this.Persister, (IEventSource) this.Session);
      foreach (IPostInsertEventListener insertEventListener in insertEventListeners)
        insertEventListener.OnPostInsert(@event);
    }

    private bool PreInsert()
    {
      IPreInsertEventListener[] insertEventListeners = this.Session.Listeners.PreInsertEventListeners;
      bool flag = false;
      if (insertEventListeners.Length > 0)
      {
        PreInsertEvent @event = new PreInsertEvent(this.Instance, this.Id, this.state, this.Persister, (IEventSource) this.Session);
        foreach (IPreInsertEventListener insertEventListener in insertEventListeners)
          flag |= insertEventListener.OnPreInsert(@event);
      }
      return flag;
    }

    private bool IsCachePutEnabled(IEntityPersister persister)
    {
      return persister.HasCache && !persister.IsCacheInvalidationRequired && (this.Session.CacheMode & CacheMode.Put) == CacheMode.Put;
    }

    public override int CompareTo(EntityAction other) => 0;
  }
}
