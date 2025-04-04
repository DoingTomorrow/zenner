// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultDeleteEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using NHibernate.Action;
using NHibernate.Classic;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using NHibernate.Util;
using System;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class DefaultDeleteEventListener : IDeleteEventListener
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (DefaultDeleteEventListener));

    public virtual void OnDelete(DeleteEvent @event)
    {
      this.OnDelete(@event, (ISet) new IdentitySet());
    }

    public virtual void OnDelete(DeleteEvent @event, ISet transientEntities)
    {
      IEventSource session = @event.Session;
      IPersistenceContext persistenceContext = session.PersistenceContext;
      object obj1 = persistenceContext.UnproxyAndReassociate(@event.Entity);
      EntityEntry entityEntry = persistenceContext.GetEntry(obj1);
      IEntityPersister persister;
      object obj2;
      object version;
      if (entityEntry == null)
      {
        DefaultDeleteEventListener.log.Debug((object) "entity was not persistent in delete processing");
        persister = session.GetEntityPersister(@event.EntityName, obj1);
        if (ForeignKeys.IsTransient(persister.EntityName, obj1, new bool?(), (ISessionImplementor) session))
        {
          this.DeleteTransientEntity(session, obj1, @event.CascadeDeleteEnabled, persister, transientEntities);
          return;
        }
        this.PerformDetachedEntityDeletionCheck(@event);
        obj2 = persister.GetIdentifier(obj1, session.EntityMode);
        if (obj2 == null)
          throw new TransientObjectException("the detached instance passed to delete() had a null identifier");
        EntityKey entityKey = new EntityKey(obj2, persister, session.EntityMode);
        persistenceContext.CheckUniqueness(entityKey, obj1);
        new OnUpdateVisitor(session, obj2, obj1).Process(obj1, persister);
        version = persister.GetVersion(obj1, session.EntityMode);
        entityEntry = persistenceContext.AddEntity(obj1, persister.IsMutable ? Status.Loaded : Status.ReadOnly, persister.GetPropertyValues(obj1, session.EntityMode), entityKey, version, LockMode.None, true, persister, false, false);
      }
      else
      {
        DefaultDeleteEventListener.log.Debug((object) "deleting a persistent instance");
        if (entityEntry.Status == Status.Deleted || entityEntry.Status == Status.Gone)
        {
          DefaultDeleteEventListener.log.Debug((object) "object was already deleted");
          return;
        }
        persister = entityEntry.Persister;
        obj2 = entityEntry.Id;
        version = entityEntry.Version;
      }
      if (this.InvokeDeleteLifecycle(session, obj1, persister))
        return;
      this.DeleteEntity(session, obj1, entityEntry, @event.CascadeDeleteEnabled, persister, transientEntities);
      if (!session.Factory.Settings.IsIdentifierRollbackEnabled)
        return;
      persister.ResetIdentifier(obj1, obj2, version, session.EntityMode);
    }

    protected virtual void PerformDetachedEntityDeletionCheck(DeleteEvent @event)
    {
    }

    protected virtual void DeleteTransientEntity(
      IEventSource session,
      object entity,
      bool cascadeDeleteEnabled,
      IEntityPersister persister,
      ISet transientEntities)
    {
      DefaultDeleteEventListener.log.Info((object) "handling transient entity in delete processing");
      if (transientEntities == null)
        transientEntities = (ISet) new HashedSet();
      if (!transientEntities.Add(entity))
      {
        DefaultDeleteEventListener.log.Debug((object) "already handled transient entity; skipping");
      }
      else
      {
        this.CascadeBeforeDelete(session, persister, entity, (EntityEntry) null, transientEntities);
        this.CascadeAfterDelete(session, persister, entity, transientEntities);
      }
    }

    protected virtual void DeleteEntity(
      IEventSource session,
      object entity,
      EntityEntry entityEntry,
      bool isCascadeDeleteEnabled,
      IEntityPersister persister,
      ISet transientEntities)
    {
      if (DefaultDeleteEventListener.log.IsDebugEnabled)
        DefaultDeleteEventListener.log.Debug((object) ("deleting " + MessageHelper.InfoString(persister, entityEntry.Id, session.Factory)));
      IPersistenceContext persistenceContext = session.PersistenceContext;
      IType[] propertyTypes = persister.PropertyTypes;
      object version = entityEntry.Version;
      object[] currentState = entityEntry.LoadedState != null ? entityEntry.LoadedState : persister.GetPropertyValues(entity, session.EntityMode);
      object[] deletedState = this.CreateDeletedState(persister, currentState, session);
      entityEntry.DeletedState = deletedState;
      session.Interceptor.OnDelete(entity, entityEntry.Id, deletedState, persister.PropertyNames, propertyTypes);
      persistenceContext.SetEntryStatus(entityEntry, Status.Deleted);
      EntityKey o = new EntityKey(entityEntry.Id, persister, session.EntityMode);
      this.CascadeBeforeDelete(session, persister, entity, entityEntry, transientEntities);
      new ForeignKeys.Nullifier(entity, true, false, (ISessionImplementor) session).NullifyTransientReferences(entityEntry.DeletedState, propertyTypes);
      new Nullability((ISessionImplementor) session).CheckNullability(entityEntry.DeletedState, persister, true);
      persistenceContext.NullifiableEntityKeys.Add((object) o);
      session.ActionQueue.AddAction(new EntityDeleteAction(entityEntry.Id, deletedState, version, entity, persister, isCascadeDeleteEnabled, (ISessionImplementor) session));
      this.CascadeAfterDelete(session, persister, entity, transientEntities);
    }

    private object[] CreateDeletedState(
      IEntityPersister persister,
      object[] currentState,
      IEventSource session)
    {
      IType[] propertyTypes = persister.PropertyTypes;
      object[] target = new object[propertyTypes.Length];
      bool[] flagArray = new bool[propertyTypes.Length];
      ArrayHelper.Fill<bool>(flagArray, true);
      TypeHelper.DeepCopy(currentState, propertyTypes, flagArray, target, (ISessionImplementor) session);
      return target;
    }

    protected virtual bool InvokeDeleteLifecycle(
      IEventSource session,
      object entity,
      IEntityPersister persister)
    {
      if (persister.ImplementsLifecycle(session.EntityMode))
      {
        DefaultDeleteEventListener.log.Debug((object) "calling onDelete()");
        if (((ILifecycle) entity).OnDelete((ISession) session) == LifecycleVeto.Veto)
        {
          DefaultDeleteEventListener.log.Debug((object) "deletion vetoed by onDelete()");
          return true;
        }
      }
      return false;
    }

    protected virtual void CascadeBeforeDelete(
      IEventSource session,
      IEntityPersister persister,
      object entity,
      EntityEntry entityEntry,
      ISet transientEntities)
    {
      ISessionImplementor sessionImplementor = (ISessionImplementor) session;
      CacheMode cacheMode = sessionImplementor.CacheMode;
      sessionImplementor.CacheMode = CacheMode.Get;
      session.PersistenceContext.IncrementCascadeLevel();
      try
      {
        new Cascade(CascadingAction.Delete, CascadePoint.AfterInsertBeforeDelete, session).CascadeOn(persister, entity, (object) transientEntities);
      }
      finally
      {
        session.PersistenceContext.DecrementCascadeLevel();
        sessionImplementor.CacheMode = cacheMode;
      }
    }

    protected virtual void CascadeAfterDelete(
      IEventSource session,
      IEntityPersister persister,
      object entity,
      ISet transientEntities)
    {
      ISessionImplementor sessionImplementor = (ISessionImplementor) session;
      CacheMode cacheMode = sessionImplementor.CacheMode;
      sessionImplementor.CacheMode = CacheMode.Get;
      session.PersistenceContext.IncrementCascadeLevel();
      try
      {
        new Cascade(CascadingAction.Delete, CascadePoint.BeforeInsertAfterDelete, session).CascadeOn(persister, entity, (object) transientEntities);
      }
      finally
      {
        session.PersistenceContext.DecrementCascadeLevel();
        sessionImplementor.CacheMode = cacheMode;
      }
    }
  }
}
