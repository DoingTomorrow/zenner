// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultMergeEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Classic;
using NHibernate.Engine;
using NHibernate.Intercept;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Type;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class DefaultMergeEventListener : AbstractSaveEventListener, IMergeEventListener
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (DefaultMergeEventListener));

    protected override CascadingAction CascadeAction => CascadingAction.Merge;

    protected override bool? AssumedUnsaved => new bool?(false);

    protected override IDictionary GetMergeMap(object anything)
    {
      return ((EventCache) anything).InvertMap();
    }

    public virtual void OnMerge(MergeEvent @event)
    {
      EventCache eventCache = new EventCache();
      this.OnMerge(@event, (IDictionary) eventCache);
      IDictionary transientCopyCache1 = (IDictionary) this.GetTransientCopyCache(@event, eventCache);
      if (transientCopyCache1.Count > 0)
      {
        this.RetryMergeTransientEntities(@event, transientCopyCache1, eventCache);
        IDictionary transientCopyCache2 = (IDictionary) this.GetTransientCopyCache(@event, eventCache);
        if (transientCopyCache2.Count > 0)
        {
          ISet<string> set = (ISet<string>) new HashedSet<string>();
          foreach (object key in (IEnumerable) transientCopyCache2.Keys)
          {
            string o = @event.Session.GuessEntityName(key);
            set.Add(o);
            DefaultMergeEventListener.log.InfoFormat("transient instance could not be processed by merge: {0} [{1}]", (object) o, (object) key.ToString());
          }
          throw new TransientObjectException("one or more objects is an unsaved transient instance - save transient instance(s) before merging: " + (object) set);
        }
      }
      eventCache.Clear();
    }

    public virtual void OnMerge(MergeEvent @event, IDictionary copiedAlready)
    {
      EventCache copyCache = (EventCache) copiedAlready;
      IEventSource session = @event.Session;
      object original = @event.Original;
      if (original == null)
        return;
      object obj;
      if (original.IsProxy())
      {
        ILazyInitializer hibernateLazyInitializer = ((INHibernateProxy) original).HibernateLazyInitializer;
        if (hibernateLazyInitializer.IsUninitialized)
        {
          DefaultMergeEventListener.log.Debug((object) "ignoring uninitialized proxy");
          @event.Result = session.Load(hibernateLazyInitializer.EntityName, hibernateLazyInitializer.Identifier);
          return;
        }
        obj = hibernateLazyInitializer.GetImplementation();
      }
      else
        obj = original;
      if (copyCache.Contains(obj) && copyCache.IsOperatedOn(obj))
      {
        DefaultMergeEventListener.log.Debug((object) "already in merge process");
        @event.Result = obj;
      }
      else
      {
        if (copyCache.Contains(obj))
        {
          DefaultMergeEventListener.log.Info((object) "already in copyCache; setting in merge process");
          copyCache.SetOperatedOn(obj, true);
        }
        @event.Entity = obj;
        EntityState entityState = EntityState.Undefined;
        if (object.ReferenceEquals((object) null, (object) @event.EntityName))
          @event.EntityName = session.BestGuessEntityName(obj);
        EntityEntry entry = session.PersistenceContext.GetEntry(obj);
        if (entry == null)
        {
          IEntityPersister entityPersister = session.GetEntityPersister(@event.EntityName, obj);
          object identifier = entityPersister.GetIdentifier(obj, session.EntityMode);
          if (identifier != null)
          {
            EntityKey key = new EntityKey(identifier, entityPersister, session.EntityMode);
            object entity = session.PersistenceContext.GetEntity(key);
            entry = session.PersistenceContext.GetEntry(entity);
            if (entry != null)
              entityState = EntityState.Detached;
          }
        }
        if (entityState == EntityState.Undefined)
          entityState = this.GetEntityState(obj, @event.EntityName, entry, (ISessionImplementor) session);
        switch (entityState)
        {
          case EntityState.Persistent:
            this.EntityIsPersistent(@event, (IDictionary) copyCache);
            break;
          case EntityState.Transient:
            this.EntityIsTransient(@event, (IDictionary) copyCache);
            break;
          case EntityState.Detached:
            this.EntityIsDetached(@event, (IDictionary) copyCache);
            break;
          default:
            throw new ObjectDeletedException("deleted instance passed to merge", (object) null, this.GetLoggableName(@event.EntityName, obj));
        }
      }
    }

    protected virtual void EntityIsPersistent(MergeEvent @event, IDictionary copyCache)
    {
      DefaultMergeEventListener.log.Debug((object) "ignoring persistent instance");
      object entity = @event.Entity;
      IEventSource session = @event.Session;
      IEntityPersister entityPersister = session.GetEntityPersister(@event.EntityName, entity);
      ((EventCache) copyCache).Add(entity, entity, true);
      this.CascadeOnMerge(session, entityPersister, entity, copyCache);
      this.CopyValues(entityPersister, entity, entity, (ISessionImplementor) session, copyCache);
      @event.Result = entity;
    }

    protected virtual void EntityIsTransient(MergeEvent @event, IDictionary copyCache)
    {
      DefaultMergeEventListener.log.Info((object) "merging transient instance");
      object entity = @event.Entity;
      IEventSource session = @event.Session;
      string entityName = session.GetEntityPersister(@event.EntityName, entity).EntityName;
      @event.Result = this.MergeTransientEntity(entity, entityName, @event.RequestedId, session, copyCache);
    }

    private object MergeTransientEntity(
      object entity,
      string entityName,
      object requestedId,
      IEventSource source,
      IDictionary copyCache)
    {
      IEntityPersister entityPersister = source.GetEntityPersister(entityName, entity);
      object identifier = entityPersister.HasIdentifierProperty ? entityPersister.GetIdentifier(entity, source.EntityMode) : (object) null;
      object obj;
      if (copyCache.Contains(entity))
      {
        obj = copyCache[entity];
        entityPersister.SetIdentifier(obj, identifier, source.EntityMode);
      }
      else
      {
        obj = source.Instantiate(entityPersister, identifier);
        ((EventCache) copyCache).Add(entity, obj, true);
      }
      base.CascadeBeforeSave(source, entityPersister, entity, (object) copyCache);
      this.CopyValues(entityPersister, entity, obj, (ISessionImplementor) source, copyCache, ForeignKeyDirection.ForeignKeyFromParent);
      try
      {
        this.SaveTransientEntity(obj, entityName, requestedId, source, copyCache);
      }
      catch (PropertyValueException ex)
      {
        string propertyName = ex.PropertyName;
        object propertyValue1 = entityPersister.GetPropertyValue(obj, propertyName, source.EntityMode);
        object propertyValue2 = entityPersister.GetPropertyValue(entity, propertyName, source.EntityMode);
        IType propertyType = entityPersister.GetPropertyType(propertyName);
        EntityEntry entry = source.PersistenceContext.GetEntry(obj);
        if (propertyValue1 == null || !propertyType.IsEntityType)
        {
          DefaultMergeEventListener.log.InfoFormat("property '{0}.{1}' is null or not an entity; {1} =[{2}]", (object) entry.EntityName, (object) propertyName, propertyValue1);
          throw;
        }
        else if (!copyCache.Contains(propertyValue2))
        {
          DefaultMergeEventListener.log.InfoFormat("property '{0}.{1}' from original entity is not in copyCache; {1} =[{2}]", (object) entry.EntityName, (object) propertyName, propertyValue2);
          throw;
        }
        else if (((EventCache) copyCache).IsOperatedOn(propertyValue2))
          DefaultMergeEventListener.log.InfoFormat("property '{0}.{1}' from original entity is in copyCache and is in the process of being merged; {1} =[{2}]", (object) entry.EntityName, (object) propertyName, propertyValue2);
        else
          DefaultMergeEventListener.log.InfoFormat("property '{0}.{1}' from original entity is in copyCache and is not in the process of being merged; {1} =[{2}]", (object) entry.EntityName, (object) propertyName, propertyValue2);
      }
      base.CascadeAfterSave(source, entityPersister, entity, (object) copyCache);
      this.CopyValues(entityPersister, entity, obj, (ISessionImplementor) source, copyCache, ForeignKeyDirection.ForeignKeyToParent);
      return obj;
    }

    private void SaveTransientEntity(
      object entity,
      string entityName,
      object requestedId,
      IEventSource source,
      IDictionary copyCache)
    {
      if (requestedId == null)
        this.SaveWithGeneratedId(entity, entityName, (object) copyCache, source, false);
      else
        this.SaveWithRequestedId(entity, requestedId, entityName, (object) copyCache, source);
    }

    protected virtual void EntityIsDetached(MergeEvent @event, IDictionary copyCache)
    {
      DefaultMergeEventListener.log.Debug((object) "merging detached instance");
      object entity = @event.Entity;
      IEventSource session = @event.Session;
      IEntityPersister entityPersister = session.GetEntityPersister(@event.EntityName, entity);
      string entityName = entityPersister.EntityName;
      object obj1 = @event.RequestedId;
      if (obj1 == null)
      {
        obj1 = entityPersister.GetIdentifier(entity, session.EntityMode);
      }
      else
      {
        object identifier = entityPersister.GetIdentifier(entity, session.EntityMode);
        if (!entityPersister.IdentifierType.IsEqual(obj1, identifier, session.EntityMode, session.Factory))
          throw new HibernateException("merge requested with id not matching id of passed entity");
      }
      string fetchProfile = session.FetchProfile;
      session.FetchProfile = "merge";
      object id = entityPersister.IdentifierType.DeepCopy(obj1, session.EntityMode, session.Factory);
      object obj2 = session.Get(entityPersister.EntityName, id);
      session.FetchProfile = fetchProfile;
      if (obj2 == null)
      {
        this.EntityIsTransient(@event, copyCache);
      }
      else
      {
        if (this.InvokeUpdateLifecycle(entity, entityPersister, session))
          return;
        ((EventCache) copyCache).Add(entity, obj2, true);
        object target = session.PersistenceContext.Unproxy(obj2);
        if (target == entity)
          throw new AssertionFailure("entity was not detached");
        if (!session.GetEntityName(target).Equals(entityName))
          throw new WrongClassException("class of the given object did not match class of persistent copy", @event.RequestedId, entityPersister.EntityName);
        if (DefaultMergeEventListener.IsVersionChanged(entity, session, entityPersister, target))
        {
          if (session.Factory.Statistics.IsStatisticsEnabled)
            session.Factory.StatisticsImplementor.OptimisticFailure(entityName);
          throw new StaleObjectStateException(entityPersister.EntityName, obj1);
        }
        this.CascadeOnMerge(session, entityPersister, entity, copyCache);
        this.CopyValues(entityPersister, entity, target, (ISessionImplementor) session, copyCache);
        this.MarkInterceptorDirty(entity, target);
        @event.Result = obj2;
      }
    }

    protected virtual bool InvokeUpdateLifecycle(
      object entity,
      IEntityPersister persister,
      IEventSource source)
    {
      if (persister.ImplementsLifecycle(source.EntityMode))
      {
        DefaultMergeEventListener.log.Debug((object) "calling onUpdate()");
        if (((ILifecycle) entity).OnUpdate((ISession) source) == LifecycleVeto.Veto)
        {
          DefaultMergeEventListener.log.Debug((object) "update vetoed by onUpdate()");
          return true;
        }
      }
      return false;
    }

    private void MarkInterceptorDirty(object entity, object target)
    {
      if (!FieldInterceptionHelper.IsInstrumented(entity))
        return;
      FieldInterceptionHelper.ExtractFieldInterceptor(target)?.MarkDirty();
    }

    private static bool IsVersionChanged(
      object entity,
      IEventSource source,
      IEntityPersister persister,
      object target)
    {
      return persister.IsVersioned && !persister.VersionType.IsSame(persister.GetVersion(target, source.EntityMode), persister.GetVersion(entity, source.EntityMode), source.EntityMode) && DefaultMergeEventListener.ExistsInDatabase(target, source, persister);
    }

    private static bool ExistsInDatabase(
      object entity,
      IEventSource source,
      IEntityPersister persister)
    {
      EntityEntry entry = source.PersistenceContext.GetEntry(entity);
      if (entry == null)
      {
        object identifier = persister.GetIdentifier(entity, source.EntityMode);
        if (identifier != null)
        {
          EntityKey key = new EntityKey(identifier, persister, source.EntityMode);
          object entity1 = source.PersistenceContext.GetEntity(key);
          entry = source.PersistenceContext.GetEntry(entity1);
        }
      }
      return entry != null && entry.ExistsInDatabase;
    }

    protected virtual void CopyValues(
      IEntityPersister persister,
      object entity,
      object target,
      ISessionImplementor source,
      IDictionary copyCache)
    {
      object[] values = TypeHelper.Replace(persister.GetPropertyValues(entity, source.EntityMode), persister.GetPropertyValues(target, source.EntityMode), persister.PropertyTypes, source, target, copyCache);
      persister.SetPropertyValues(target, values, source.EntityMode);
    }

    protected virtual void CopyValues(
      IEntityPersister persister,
      object entity,
      object target,
      ISessionImplementor source,
      IDictionary copyCache,
      ForeignKeyDirection foreignKeyDirection)
    {
      object[] values = !foreignKeyDirection.Equals((object) ForeignKeyDirection.ForeignKeyToParent) ? TypeHelper.Replace(persister.GetPropertyValues(entity, source.EntityMode), persister.GetPropertyValues(target, source.EntityMode), persister.PropertyTypes, source, target, copyCache, foreignKeyDirection) : TypeHelper.ReplaceAssociations(persister.GetPropertyValues(entity, source.EntityMode), persister.GetPropertyValues(target, source.EntityMode), persister.PropertyTypes, source, target, copyCache, foreignKeyDirection);
      persister.SetPropertyValues(target, values, source.EntityMode);
    }

    protected virtual void CascadeOnMerge(
      IEventSource source,
      IEntityPersister persister,
      object entity,
      IDictionary copyCache)
    {
      source.PersistenceContext.IncrementCascadeLevel();
      try
      {
        new Cascade(this.CascadeAction, CascadePoint.AfterUpdate, source).CascadeOn(persister, entity, (object) copyCache);
      }
      finally
      {
        source.PersistenceContext.DecrementCascadeLevel();
      }
    }

    protected EventCache GetTransientCopyCache(MergeEvent @event, EventCache copyCache)
    {
      EventCache transientCopyCache = new EventCache();
      foreach (object key in (IEnumerable) copyCache.Keys)
      {
        object implementation = copyCache[key];
        if (implementation.IsProxy())
          implementation = ((INHibernateProxy) implementation).HibernateLazyInitializer.GetImplementation();
        if (!(implementation is ILifecycle))
        {
          EntityEntry entry = @event.Session.PersistenceContext.GetEntry(implementation);
          if (entry == null)
          {
            DefaultMergeEventListener.log.InfoFormat("transient instance could not be processed by merge: {0} [{1}]", (object) @event.Session.GuessEntityName(implementation), key);
            throw new TransientObjectException("object is an unsaved transient instance - save the transient instance before merging: " + @event.Session.GuessEntityName(implementation));
          }
          if (entry.Status == Status.Saving)
            transientCopyCache.Add(key, implementation, copyCache.IsOperatedOn(key));
          else if (entry.Status != Status.Loaded && entry.Status != Status.ReadOnly)
            throw new AssertionFailure(string.Format("Merged entity does not have status set to MANAGED or READ_ONLY; {0} status = {1}", implementation, (object) entry.Status));
        }
      }
      return transientCopyCache;
    }

    protected void RetryMergeTransientEntities(
      MergeEvent @event,
      IDictionary transientCopyCache,
      EventCache copyCache)
    {
      foreach (object key in (IEnumerable) transientCopyCache.Keys)
      {
        object entity = transientCopyCache[key];
        EntityEntry entry = @event.Session.PersistenceContext.GetEntry(entity);
        if (key == @event.Entity)
          this.MergeTransientEntity(key, entry.EntityName, @event.RequestedId, @event.Session, (IDictionary) copyCache);
        else
          this.MergeTransientEntity(key, entry.EntityName, entry.Id, @event.Session, (IDictionary) copyCache);
      }
    }

    protected override void CascadeAfterSave(
      IEventSource source,
      IEntityPersister persister,
      object entity,
      object anything)
    {
    }

    protected override void CascadeBeforeSave(
      IEventSource source,
      IEntityPersister persister,
      object entity,
      object anything)
    {
    }
  }
}
