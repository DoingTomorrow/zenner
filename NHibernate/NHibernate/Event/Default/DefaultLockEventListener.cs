// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultLockEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Entity;
using System;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class DefaultLockEventListener : AbstractLockUpgradeEventListener, ILockEventListener
  {
    public virtual void OnLock(LockEvent @event)
    {
      if (@event.Entity == null)
        throw new NullReferenceException("attempted to lock null");
      if (@event.LockMode == LockMode.Write)
        throw new HibernateException("Invalid lock mode for lock()");
      ISessionImplementor session = (ISessionImplementor) @event.Session;
      if (@event.LockMode == LockMode.None && session.PersistenceContext.ReassociateIfUninitializedProxy(@event.Entity))
        return;
      object entity = session.PersistenceContext.UnproxyAndReassociate(@event.Entity);
      EntityEntry entry = session.PersistenceContext.GetEntry(entity);
      if (entry == null)
      {
        IEntityPersister entityPersister = session.GetEntityPersister(@event.EntityName, entity);
        object identifier = entityPersister.GetIdentifier(entity, session.EntityMode);
        if (!ForeignKeys.IsNotTransient(@event.EntityName, entity, new bool?(false), session))
          throw new TransientObjectException("cannot lock an unsaved transient instance: " + entityPersister.EntityName);
        entry = this.Reassociate((AbstractEvent) @event, entity, identifier, entityPersister);
        this.CascadeOnLock(@event, entityPersister, entity);
      }
      this.UpgradeLock(entity, entry, @event.LockMode, session);
    }

    private void CascadeOnLock(LockEvent @event, IEntityPersister persister, object entity)
    {
      IEventSource session = @event.Session;
      session.PersistenceContext.IncrementCascadeLevel();
      try
      {
        new Cascade(CascadingAction.Lock, CascadePoint.AfterUpdate, session).CascadeOn(persister, entity, (object) @event.LockMode);
      }
      finally
      {
        session.PersistenceContext.DecrementCascadeLevel();
      }
    }
  }
}
