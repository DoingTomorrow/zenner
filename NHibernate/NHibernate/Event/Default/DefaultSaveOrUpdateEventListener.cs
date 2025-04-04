// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultSaveOrUpdateEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Classic;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using System;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class DefaultSaveOrUpdateEventListener : 
    AbstractSaveEventListener,
    ISaveOrUpdateEventListener
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (DefaultSaveOrUpdateEventListener));

    protected override CascadingAction CascadeAction => CascadingAction.SaveUpdate;

    public virtual void OnSaveOrUpdate(SaveOrUpdateEvent @event)
    {
      ISessionImplementor session = (ISessionImplementor) @event.Session;
      object entity1 = @event.Entity;
      object requestedId = @event.RequestedId;
      if (requestedId != null && entity1.IsProxy())
        ((INHibernateProxy) entity1).HibernateLazyInitializer.Identifier = requestedId;
      if (this.ReassociateIfUninitializedProxy(entity1, session))
      {
        DefaultSaveOrUpdateEventListener.log.Debug((object) "reassociated uninitialized proxy");
      }
      else
      {
        object entity2 = session.PersistenceContext.UnproxyAndReassociate(entity1);
        @event.Entity = entity2;
        @event.Entry = session.PersistenceContext.GetEntry(entity2);
        @event.ResultId = this.PerformSaveOrUpdate(@event);
      }
    }

    protected virtual bool ReassociateIfUninitializedProxy(object obj, ISessionImplementor source)
    {
      return source.PersistenceContext.ReassociateIfUninitializedProxy(obj);
    }

    protected virtual object PerformSaveOrUpdate(SaveOrUpdateEvent @event)
    {
      switch (this.GetEntityState(@event.Entity, @event.EntityName, @event.Entry, (ISessionImplementor) @event.Session))
      {
        case EntityState.Persistent:
          return this.EntityIsPersistent(@event);
        case EntityState.Detached:
          this.EntityIsDetached(@event);
          return (object) null;
        default:
          return this.EntityIsTransient(@event);
      }
    }

    protected virtual object EntityIsPersistent(SaveOrUpdateEvent @event)
    {
      DefaultSaveOrUpdateEventListener.log.Debug((object) "ignoring persistent instance");
      EntityEntry entry = @event.Entry;
      if (entry == null)
        throw new AssertionFailure("entity was transient or detached");
      if (entry.Status == Status.Deleted)
        throw new AssertionFailure("entity was deleted");
      ISessionFactoryImplementor factory = @event.Session.Factory;
      object requestedId = @event.RequestedId;
      object id;
      if (requestedId == null)
      {
        id = entry.Id;
      }
      else
      {
        if (!entry.Persister.IdentifierType.IsEqual(requestedId, entry.Id, EntityMode.Poco))
          throw new PersistentObjectException("object passed to save() was already persistent: " + MessageHelper.InfoString(entry.Persister, requestedId, factory));
        id = requestedId;
      }
      if (DefaultSaveOrUpdateEventListener.log.IsDebugEnabled)
        DefaultSaveOrUpdateEventListener.log.Debug((object) ("object already associated with session: " + MessageHelper.InfoString(entry.Persister, id, factory)));
      return id;
    }

    protected virtual object EntityIsTransient(SaveOrUpdateEvent @event)
    {
      DefaultSaveOrUpdateEventListener.log.Debug((object) "saving transient instance");
      IEventSource session = @event.Session;
      EntityEntry entry = @event.Entry;
      if (entry != null)
      {
        if (entry.Status != Status.Deleted)
          throw new AssertionFailure("entity was persistent");
        session.ForceFlush(entry);
      }
      object id = this.SaveWithGeneratedOrRequestedId(@event);
      session.PersistenceContext.ReassociateProxy(@event.Entity, id);
      return id;
    }

    protected virtual object SaveWithGeneratedOrRequestedId(SaveOrUpdateEvent @event)
    {
      return this.SaveWithGeneratedId(@event.Entity, @event.EntityName, (object) null, @event.Session, true);
    }

    protected virtual void EntityIsDetached(SaveOrUpdateEvent @event)
    {
      DefaultSaveOrUpdateEventListener.log.Debug((object) "updating detached instance");
      if (@event.Session.PersistenceContext.IsEntryFor(@event.Entity))
        throw new AssertionFailure("entity was persistent");
      object entity = @event.Entity;
      IEntityPersister entityPersister = @event.Session.GetEntityPersister(@event.EntityName, entity);
      @event.RequestedId = this.GetUpdateId(entity, entityPersister, @event.RequestedId, @event.Session.EntityMode);
      this.PerformUpdate(@event, entity, entityPersister);
    }

    protected virtual object GetUpdateId(
      object entity,
      IEntityPersister persister,
      object requestedId,
      EntityMode entityMode)
    {
      return persister.GetIdentifier(entity, entityMode) ?? throw new TransientObjectException("The given object has a null identifier: " + persister.EntityName);
    }

    protected virtual void PerformUpdate(
      SaveOrUpdateEvent @event,
      object entity,
      IEntityPersister persister)
    {
      if (!persister.IsMutable)
        DefaultSaveOrUpdateEventListener.log.Debug((object) "immutable instance passed to PerformUpdate(), locking");
      if (DefaultSaveOrUpdateEventListener.log.IsDebugEnabled)
        DefaultSaveOrUpdateEventListener.log.Debug((object) ("updating " + MessageHelper.InfoString(persister, @event.RequestedId, @event.Session.Factory)));
      IEventSource session = @event.Session;
      EntityKey entityKey = new EntityKey(@event.RequestedId, persister, session.EntityMode);
      session.PersistenceContext.CheckUniqueness(entityKey, entity);
      if (this.InvokeUpdateLifecycle(entity, persister, session))
      {
        this.Reassociate((AbstractEvent) @event, @event.Entity, @event.RequestedId, persister);
      }
      else
      {
        new OnUpdateVisitor(session, @event.RequestedId, entity).Process(entity, persister);
        session.PersistenceContext.AddEntity(entity, persister.IsMutable ? Status.Loaded : Status.ReadOnly, (object[]) null, entityKey, persister.GetVersion(entity, session.EntityMode), LockMode.None, true, persister, false, true);
        if (DefaultSaveOrUpdateEventListener.log.IsDebugEnabled)
          DefaultSaveOrUpdateEventListener.log.Debug((object) ("updating " + MessageHelper.InfoString(persister, @event.RequestedId, session.Factory)));
        this.CascadeOnUpdate(@event, persister, entity);
      }
    }

    protected virtual bool InvokeUpdateLifecycle(
      object entity,
      IEntityPersister persister,
      IEventSource source)
    {
      if (persister.ImplementsLifecycle(source.EntityMode))
      {
        DefaultSaveOrUpdateEventListener.log.Debug((object) "calling onUpdate()");
        if (((ILifecycle) entity).OnUpdate((ISession) source) == LifecycleVeto.Veto)
        {
          DefaultSaveOrUpdateEventListener.log.Debug((object) "update vetoed by onUpdate()");
          return true;
        }
      }
      return false;
    }

    private void CascadeOnUpdate(
      SaveOrUpdateEvent @event,
      IEntityPersister persister,
      object entity)
    {
      IEventSource session = @event.Session;
      session.PersistenceContext.IncrementCascadeLevel();
      try
      {
        new Cascade(CascadingAction.SaveUpdate, CascadePoint.AfterUpdate, session).CascadeOn(persister, entity);
      }
      finally
      {
        session.PersistenceContext.DecrementCascadeLevel();
      }
    }
  }
}
