// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultFlushEntityEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

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
  public class DefaultFlushEntityEventListener : IFlushEntityEventListener
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (DefaultFlushEntityEventListener));

    public virtual void OnFlushEntity(FlushEntityEvent @event)
    {
      object entity = @event.Entity;
      EntityEntry entityEntry = @event.EntityEntry;
      IEventSource session = @event.Session;
      IEntityPersister persister = entityEntry.Persister;
      Status status = entityEntry.Status;
      EntityMode entityMode = session.EntityMode;
      IType[] propertyTypes = persister.PropertyTypes;
      bool mightBeDirty = entityEntry.RequiresDirtyCheck(entity);
      object[] values = this.GetValues(entity, entityEntry, entityMode, mightBeDirty, (ISessionImplementor) session);
      @event.PropertyValues = values;
      bool flag = this.WrapCollections(session, persister, propertyTypes, values);
      if (this.IsUpdateNecessary(@event, mightBeDirty))
        flag = this.ScheduleUpdate(@event) || flag;
      if (status == Status.Deleted)
        return;
      if (flag)
        persister.SetPropertyValues(entity, values, entityMode);
      if (!persister.HasCollections)
        return;
      new FlushVisitor(session, entity).ProcessEntityPropertyValues(values, propertyTypes);
    }

    private object[] GetValues(
      object entity,
      EntityEntry entry,
      EntityMode entityMode,
      bool mightBeDirty,
      ISessionImplementor session)
    {
      object[] loadedState = entry.LoadedState;
      Status status = entry.Status;
      IEntityPersister persister = entry.Persister;
      object[] current;
      if (status == Status.Deleted)
        current = entry.DeletedState;
      else if (!mightBeDirty && loadedState != null)
      {
        current = loadedState;
      }
      else
      {
        this.CheckId(entity, persister, entry.Id, entityMode);
        current = persister.GetPropertyValues(entity, entityMode);
        this.CheckNaturalId(persister, entry, current, loadedState, entityMode, session);
      }
      return current;
    }

    public virtual void CheckId(
      object obj,
      IEntityPersister persister,
      object id,
      EntityMode entityMode)
    {
      if (id != null && id is DelayedPostInsertIdentifier || !persister.CanExtractIdOutOfEntity)
        return;
      if (id == null)
        throw new AssertionFailure("null id in " + persister.EntityName + " entry (don't flush the Session after an exception occurs)");
      object identifier = persister.GetIdentifier(obj, entityMode);
      if (!persister.IdentifierType.IsEqual(id, identifier, EntityMode.Poco))
        throw new HibernateException("identifier of an instance of " + persister.EntityName + " was altered from " + id + " to " + identifier);
    }

    private void CheckNaturalId(
      IEntityPersister persister,
      EntityEntry entry,
      object[] current,
      object[] loaded,
      EntityMode entityMode,
      ISessionImplementor session)
    {
      if (!persister.HasNaturalIdentifier || entry.Status == Status.ReadOnly)
        return;
      object[] objArray = (object[]) null;
      IType[] propertyTypes = persister.PropertyTypes;
      int[] identifierProperties = persister.NaturalIdentifierProperties;
      bool[] propertyUpdateability = persister.PropertyUpdateability;
      for (int index1 = 0; index1 < identifierProperties.Length; ++index1)
      {
        int index2 = identifierProperties[index1];
        if (!propertyUpdateability[index2])
        {
          object y;
          if (loaded == null)
          {
            if (objArray == null)
              objArray = session.PersistenceContext.GetNaturalIdSnapshot(entry.Id, persister);
            y = objArray[index1];
          }
          else
            y = loaded[index2];
          if (!propertyTypes[index2].IsEqual(current[index2], y, entityMode))
            throw new HibernateException("immutable natural identifier of an instance of " + persister.EntityName + " was altered");
        }
      }
    }

    private bool WrapCollections(
      IEventSource session,
      IEntityPersister persister,
      IType[] types,
      object[] values)
    {
      if (!persister.HasCollections)
        return false;
      WrapVisitor wrapVisitor = new WrapVisitor(session);
      wrapVisitor.ProcessEntityPropertyValues(values, types);
      return wrapVisitor.SubstitutionRequired;
    }

    private bool IsUpdateNecessary(FlushEntityEvent @event, bool mightBeDirty)
    {
      Status status = @event.EntityEntry.Status;
      if (!mightBeDirty && status != Status.Deleted)
        return this.HasDirtyCollections(@event, @event.EntityEntry.Persister, status);
      this.DirtyCheck(@event);
      return this.IsUpdateNecessary(@event);
    }

    private bool ScheduleUpdate(FlushEntityEvent @event)
    {
      EntityEntry entityEntry = @event.EntityEntry;
      IEventSource session = @event.Session;
      EntityMode entityMode = session.EntityMode;
      object entity = @event.Entity;
      Status status = entityEntry.Status;
      IEntityPersister persister = entityEntry.Persister;
      object[] propertyValues = @event.PropertyValues;
      if (DefaultFlushEntityEventListener.log.IsDebugEnabled)
      {
        if (status == Status.Deleted)
        {
          if (!persister.IsMutable)
            DefaultFlushEntityEventListener.log.Debug((object) ("Updating immutable, deleted entity: " + MessageHelper.InfoString(persister, entityEntry.Id, session.Factory)));
          else if (!entityEntry.IsModifiableEntity())
            DefaultFlushEntityEventListener.log.Debug((object) ("Updating non-modifiable, deleted entity: " + MessageHelper.InfoString(persister, entityEntry.Id, session.Factory)));
          else
            DefaultFlushEntityEventListener.log.Debug((object) ("Updating deleted entity: " + MessageHelper.InfoString(persister, entityEntry.Id, session.Factory)));
        }
        else
          DefaultFlushEntityEventListener.log.Debug((object) ("Updating entity: " + MessageHelper.InfoString(persister, entityEntry.Id, session.Factory)));
      }
      bool flag = !entityEntry.IsBeingReplicated && this.HandleInterception(@event);
      this.Validate(entity, persister, status, entityMode);
      object nextVersion = this.GetNextVersion(@event);
      int[] dirtyProperties = @event.DirtyProperties;
      if (@event.DirtyCheckPossible && dirtyProperties == null)
      {
        if (!flag && !@event.HasDirtyCollection)
          throw new AssertionFailure("dirty, but no dirty properties");
        dirtyProperties = ArrayHelper.EmptyIntArray;
      }
      new Nullability((ISessionImplementor) session).CheckNullability(propertyValues, persister, true);
      session.ActionQueue.AddAction(new EntityUpdateAction(entityEntry.Id, propertyValues, dirtyProperties, @event.HasDirtyCollection, status != Status.Deleted || entityEntry.IsModifiableEntity() ? entityEntry.LoadedState : persister.GetPropertyValues(entity, entityMode), entityEntry.Version, nextVersion, entity, persister, (ISessionImplementor) session));
      return flag;
    }

    protected virtual void Validate(
      object entity,
      IEntityPersister persister,
      Status status,
      EntityMode entityMode)
    {
      if (status != Status.Loaded || !persister.ImplementsValidatable(entityMode))
        return;
      ((IValidatable) entity).Validate();
    }

    protected virtual bool HandleInterception(FlushEntityEvent @event)
    {
      ISessionImplementor session = (ISessionImplementor) @event.Session;
      EntityEntry entityEntry = @event.EntityEntry;
      IEntityPersister persister = entityEntry.Persister;
      object entity = @event.Entity;
      object[] propertyValues = @event.PropertyValues;
      bool flag = this.InvokeInterceptor(session, entity, entityEntry, propertyValues, persister);
      if (flag && @event.DirtyCheckPossible && !@event.DirtyCheckHandledByInterceptor)
      {
        int[] numArray = !@event.HasDatabaseSnapshot ? persister.FindDirty(propertyValues, entityEntry.LoadedState, entity, session) : persister.FindModified(@event.DatabaseSnapshot, propertyValues, entity, session);
        @event.DirtyProperties = numArray;
      }
      return flag;
    }

    protected virtual bool InvokeInterceptor(
      ISessionImplementor session,
      object entity,
      EntityEntry entry,
      object[] values,
      IEntityPersister persister)
    {
      return session.Interceptor.OnFlushDirty(entity, entry.Id, values, entry.LoadedState, persister.PropertyNames, persister.PropertyTypes);
    }

    private object GetNextVersion(FlushEntityEvent @event)
    {
      EntityEntry entityEntry = @event.EntityEntry;
      IEntityPersister persister = entityEntry.Persister;
      if (!persister.IsVersioned)
        return (object) null;
      object[] propertyValues = @event.PropertyValues;
      if (entityEntry.IsBeingReplicated)
        return Versioning.GetVersion(propertyValues, persister);
      int[] dirtyProperties = @event.DirtyProperties;
      object version = this.IsVersionIncrementRequired(@event, entityEntry, persister, dirtyProperties) ? Versioning.Increment(entityEntry.Version, persister.VersionType, (ISessionImplementor) @event.Session) : entityEntry.Version;
      Versioning.SetVersion(propertyValues, version, persister);
      return version;
    }

    private bool IsVersionIncrementRequired(
      FlushEntityEvent @event,
      EntityEntry entry,
      IEntityPersister persister,
      int[] dirtyProperties)
    {
      return entry.Status != Status.Deleted && !persister.IsVersionPropertyGenerated && (dirtyProperties == null || Versioning.IsVersionIncrementRequired(dirtyProperties, @event.HasDirtyCollection, persister.PropertyVersionability));
    }

    protected bool IsUpdateNecessary(FlushEntityEvent @event)
    {
      IEntityPersister persister = @event.EntityEntry.Persister;
      Status status = @event.EntityEntry.Status;
      if (!@event.DirtyCheckPossible)
        return true;
      int[] dirtyProperties = @event.DirtyProperties;
      return dirtyProperties != null && dirtyProperties.Length != 0 || this.HasDirtyCollections(@event, persister, status);
    }

    private bool HasDirtyCollections(
      FlushEntityEvent @event,
      IEntityPersister persister,
      Status status)
    {
      if (!this.IsCollectionDirtyCheckNecessary(persister, status))
        return false;
      DirtyCollectionSearchVisitor collectionSearchVisitor = new DirtyCollectionSearchVisitor(@event.Session, persister.PropertyVersionability);
      collectionSearchVisitor.ProcessEntityPropertyValues(@event.PropertyValues, persister.PropertyTypes);
      bool dirtyCollectionFound = collectionSearchVisitor.WasDirtyCollectionFound;
      @event.HasDirtyCollection = dirtyCollectionFound;
      return dirtyCollectionFound;
    }

    private bool IsCollectionDirtyCheckNecessary(IEntityPersister persister, Status status)
    {
      return (status == Status.Loaded || status == Status.ReadOnly) && persister.IsVersioned && persister.HasCollections;
    }

    protected virtual void DirtyCheck(FlushEntityEvent @event)
    {
      object entity = @event.Entity;
      object[] propertyValues1 = @event.PropertyValues;
      ISessionImplementor session = (ISessionImplementor) @event.Session;
      EntityEntry entityEntry = @event.EntityEntry;
      IEntityPersister persister = entityEntry.Persister;
      object id = entityEntry.Id;
      object[] loadedState = entityEntry.LoadedState;
      int[] numArray = session.Interceptor.FindDirty(entity, id, propertyValues1, loadedState, persister.PropertyNames, persister.PropertyTypes);
      @event.DatabaseSnapshot = (object[]) null;
      bool flag1;
      bool flag2;
      if (numArray == null)
      {
        flag1 = false;
        flag2 = loadedState == null;
        if (!flag2)
          numArray = persister.FindDirty(propertyValues1, loadedState, entity, session);
        else if (entityEntry.Status == Status.Deleted && !@event.EntityEntry.IsModifiableEntity())
        {
          if (propertyValues1 != entityEntry.DeletedState)
            throw new InvalidOperationException("Entity has status Status.Deleted but values != entry.DeletedState");
          object[] propertyValues2 = persister.GetPropertyValues(@event.Entity, @event.Session.EntityMode);
          numArray = persister.FindDirty(entityEntry.DeletedState, propertyValues2, entity, session);
          flag2 = false;
        }
        else
        {
          object[] databaseSnapshot = this.GetDatabaseSnapshot(session, persister, id);
          if (databaseSnapshot != null)
          {
            numArray = persister.FindModified(databaseSnapshot, propertyValues1, entity, session);
            flag2 = false;
            @event.DatabaseSnapshot = databaseSnapshot;
          }
        }
      }
      else
      {
        flag2 = false;
        flag1 = true;
      }
      @event.DirtyProperties = numArray;
      @event.DirtyCheckHandledByInterceptor = flag1;
      @event.DirtyCheckPossible = !flag2;
    }

    private object[] GetDatabaseSnapshot(
      ISessionImplementor session,
      IEntityPersister persister,
      object id)
    {
      if (persister.IsSelectBeforeUpdateRequired)
      {
        object[] databaseSnapshot = session.PersistenceContext.GetDatabaseSnapshot(id, persister);
        if (databaseSnapshot == null)
        {
          if (session.Factory.Statistics.IsStatisticsEnabled)
            session.Factory.StatisticsImplementor.OptimisticFailure(persister.EntityName);
          throw new StaleObjectStateException(persister.EntityName, id);
        }
        return databaseSnapshot;
      }
      EntityKey key = new EntityKey(id, persister, session.EntityMode);
      return session.PersistenceContext.GetCachedDatabaseSnapshot(key);
    }
  }
}
