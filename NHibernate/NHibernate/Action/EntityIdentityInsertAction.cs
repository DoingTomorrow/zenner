// Decompiled with JetBrains decompiler
// Type: NHibernate.Action.EntityIdentityInsertAction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using System;
using System.Diagnostics;

#nullable disable
namespace NHibernate.Action
{
  [Serializable]
  public sealed class EntityIdentityInsertAction : EntityAction
  {
    private readonly object lockObject = new object();
    private readonly object[] state;
    private readonly bool isDelayed;
    private readonly EntityKey delayedEntityKey;
    private object generatedId;

    public EntityIdentityInsertAction(
      object[] state,
      object instance,
      IEntityPersister persister,
      ISessionImplementor session,
      bool isDelayed)
      : base(session, (object) null, instance, persister)
    {
      this.state = state;
      this.isDelayed = isDelayed;
      this.delayedEntityKey = this.isDelayed ? this.GenerateDelayedEntityKey() : (EntityKey) null;
    }

    public object GeneratedId => this.generatedId;

    public EntityKey DelayedEntityKey => this.delayedEntityKey;

    private EntityKey GenerateDelayedEntityKey()
    {
      lock (this.lockObject)
      {
        if (!this.isDelayed)
          throw new HibernateException("Cannot request delayed entity-key for non-delayed post-insert-id generation");
        return new EntityKey((object) new DelayedPostInsertIdentifier(), this.Persister, this.Session.EntityMode);
      }
    }

    protected internal override bool HasPostCommitEventListeners
    {
      get => this.Session.Listeners.PostCommitInsertEventListeners.Length > 0;
    }

    public override void Execute()
    {
      IEntityPersister persister = this.Persister;
      object instance = this.Instance;
      bool statisticsEnabled = this.Session.Factory.Statistics.IsStatisticsEnabled;
      Stopwatch stopwatch = (Stopwatch) null;
      if (statisticsEnabled)
        stopwatch = Stopwatch.StartNew();
      bool flag = this.PreInsert();
      if (!flag)
      {
        this.generatedId = persister.Insert(this.state, instance, this.Session);
        if (persister.HasInsertGeneratedProperties)
          persister.ProcessInsertGeneratedProperties(this.generatedId, instance, this.state, this.Session);
        persister.SetIdentifier(instance, this.generatedId, this.Session.EntityMode);
      }
      this.PostInsert();
      if (!statisticsEnabled || flag)
        return;
      stopwatch.Stop();
      this.Session.Factory.StatisticsImplementor.InsertEntity(this.Persister.EntityName, stopwatch.Elapsed);
    }

    private void PostInsert()
    {
      if (this.isDelayed)
        this.Session.PersistenceContext.ReplaceDelayedEntityIdentityInsertKeys(this.delayedEntityKey, this.generatedId);
      IPostInsertEventListener[] insertEventListeners = this.Session.Listeners.PostInsertEventListeners;
      if (insertEventListeners.Length <= 0)
        return;
      PostInsertEvent @event = new PostInsertEvent(this.Instance, this.generatedId, this.state, this.Persister, (IEventSource) this.Session);
      foreach (IPostInsertEventListener insertEventListener in insertEventListeners)
        insertEventListener.OnPostInsert(@event);
    }

    private void PostCommitInsert()
    {
      IPostInsertEventListener[] insertEventListeners = this.Session.Listeners.PostCommitInsertEventListeners;
      if (insertEventListeners.Length <= 0)
        return;
      PostInsertEvent @event = new PostInsertEvent(this.Instance, this.generatedId, this.state, this.Persister, (IEventSource) this.Session);
      foreach (IPostInsertEventListener insertEventListener in insertEventListeners)
        insertEventListener.OnPostInsert(@event);
    }

    private bool PreInsert()
    {
      IPreInsertEventListener[] insertEventListeners = this.Session.Listeners.PreInsertEventListeners;
      bool flag = false;
      if (insertEventListeners.Length > 0)
      {
        PreInsertEvent @event = new PreInsertEvent(this.Instance, (object) null, this.state, this.Persister, (IEventSource) this.Session);
        foreach (IPreInsertEventListener insertEventListener in insertEventListeners)
          flag |= insertEventListener.OnPreInsert(@event);
      }
      return flag;
    }

    protected override void AfterTransactionCompletionProcessImpl(bool success)
    {
      if (!success)
        return;
      this.PostCommitInsert();
    }
  }
}
