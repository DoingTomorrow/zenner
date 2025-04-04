// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.AbstractSaveEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Action;
using NHibernate.Classic;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Impl;
using NHibernate.Intercept;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public abstract class AbstractSaveEventListener : AbstractReassociateEventListener
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (AbstractSaveEventListener));

    protected virtual bool? AssumedUnsaved => new bool?();

    protected abstract CascadingAction CascadeAction { get; }

    protected virtual bool VersionIncrementDisabled => false;

    protected virtual bool InvokeSaveLifecycle(
      object entity,
      IEntityPersister persister,
      IEventSource source)
    {
      if (persister.ImplementsLifecycle(source.EntityMode))
      {
        AbstractSaveEventListener.log.Debug((object) "calling OnSave()");
        if (((ILifecycle) entity).OnSave((ISession) source) == LifecycleVeto.Veto)
        {
          AbstractSaveEventListener.log.Debug((object) "insertion vetoed by OnSave()");
          return true;
        }
      }
      return false;
    }

    protected virtual void Validate(object entity, IEntityPersister persister, IEventSource source)
    {
      if (!persister.ImplementsValidatable(source.EntityMode))
        return;
      ((IValidatable) entity).Validate();
    }

    protected virtual object SaveWithRequestedId(
      object entity,
      object requestedId,
      string entityName,
      object anything,
      IEventSource source)
    {
      return this.PerformSave(entity, requestedId, source.GetEntityPersister(entityName, entity), false, anything, source, true);
    }

    protected virtual object SaveWithGeneratedId(
      object entity,
      string entityName,
      object anything,
      IEventSource source,
      bool requiresImmediateIdAccess)
    {
      IEntityPersister entityPersister = source.GetEntityPersister(entityName, entity);
      object id = entityPersister.IdentifierGenerator.Generate((ISessionImplementor) source, entity);
      if (id == null)
        throw new IdentifierGenerationException("null id generated for:" + (object) entity.GetType());
      if (id == IdentifierGeneratorFactory.ShortCircuitIndicator)
        return source.GetIdentifier(entity);
      if (id == IdentifierGeneratorFactory.PostInsertIndicator)
        return this.PerformSave(entity, (object) null, entityPersister, true, anything, source, requiresImmediateIdAccess);
      if (AbstractSaveEventListener.log.IsDebugEnabled)
        AbstractSaveEventListener.log.Debug((object) string.Format("generated identifier: {0}, using strategy: {1}", (object) entityPersister.IdentifierType.ToLoggableString(id, source.Factory), (object) entityPersister.IdentifierGenerator.GetType().FullName));
      return this.PerformSave(entity, id, entityPersister, false, anything, source, true);
    }

    protected virtual object PerformSave(
      object entity,
      object id,
      IEntityPersister persister,
      bool useIdentityColumn,
      object anything,
      IEventSource source,
      bool requiresImmediateIdAccess)
    {
      if (AbstractSaveEventListener.log.IsDebugEnabled)
        AbstractSaveEventListener.log.Debug((object) ("saving " + MessageHelper.InfoString(persister, id, source.Factory)));
      EntityKey key;
      if (!useIdentityColumn)
      {
        key = new EntityKey(id, persister, source.EntityMode);
        object entity1 = source.PersistenceContext.GetEntity(key);
        if (entity1 != null)
        {
          if (source.PersistenceContext.GetEntry(entity1).Status != Status.Deleted)
            throw new NonUniqueObjectException(id, persister.EntityName);
          source.ForceFlush(source.PersistenceContext.GetEntry(entity1));
        }
        persister.SetIdentifier(entity, id, source.EntityMode);
      }
      else
        key = (EntityKey) null;
      return this.InvokeSaveLifecycle(entity, persister, source) ? id : this.PerformSaveOrReplicate(entity, key, persister, useIdentityColumn, anything, source, requiresImmediateIdAccess);
    }

    protected virtual object PerformSaveOrReplicate(
      object entity,
      EntityKey key,
      IEntityPersister persister,
      bool useIdentityColumn,
      object anything,
      IEventSource source,
      bool requiresImmediateIdAccess)
    {
      this.Validate(entity, persister, source);
      object id = key == null ? (object) null : key.Identifier;
      bool isDelayed = false;
      source.PersistenceContext.AddEntry(entity, Status.Saving, (object[]) null, (object) null, id, (object) null, LockMode.Write, useIdentityColumn, persister, false, false);
      this.CascadeBeforeSave(source, persister, entity, anything);
      if (useIdentityColumn && !isDelayed)
      {
        AbstractSaveEventListener.log.Debug((object) "executing insertions");
        source.ActionQueue.ExecuteInserts();
      }
      object[] propertyValuesToInsert = persister.GetPropertyValuesToInsert(entity, this.GetMergeMap(anything), (ISessionImplementor) source);
      IType[] propertyTypes = persister.PropertyTypes;
      bool flag = this.SubstituteValuesIfNecessary(entity, id, propertyValuesToInsert, persister, (ISessionImplementor) source);
      if (persister.HasCollections)
        flag = flag || this.VisitCollectionsBeforeSave(entity, id, propertyValuesToInsert, propertyTypes, source);
      if (flag)
        persister.SetPropertyValues(entity, propertyValuesToInsert, source.EntityMode);
      TypeHelper.DeepCopy(propertyValuesToInsert, propertyTypes, persister.PropertyUpdateability, propertyValuesToInsert, (ISessionImplementor) source);
      new ForeignKeys.Nullifier(entity, false, useIdentityColumn, (ISessionImplementor) source).NullifyTransientReferences(propertyValuesToInsert, propertyTypes);
      new Nullability((ISessionImplementor) source).CheckNullability(propertyValuesToInsert, persister, false);
      if (useIdentityColumn)
      {
        EntityIdentityInsertAction insert = new EntityIdentityInsertAction(propertyValuesToInsert, entity, persister, (ISessionImplementor) source, isDelayed);
        if (!isDelayed)
        {
          AbstractSaveEventListener.log.Debug((object) "executing identity-insert immediately");
          source.ActionQueue.Execute((IExecutable) insert);
          id = insert.GeneratedId;
          key = new EntityKey(id, persister, source.EntityMode);
          source.PersistenceContext.CheckUniqueness(key, entity);
        }
        else
        {
          AbstractSaveEventListener.log.Debug((object) "delaying identity-insert due to no transaction in progress");
          source.ActionQueue.AddAction(insert);
          key = insert.DelayedEntityKey;
        }
      }
      object version = Versioning.GetVersion(propertyValuesToInsert, persister);
      source.PersistenceContext.AddEntity(entity, persister.IsMutable ? Status.Loaded : Status.ReadOnly, propertyValuesToInsert, key, version, LockMode.Write, useIdentityColumn, persister, this.VersionIncrementDisabled, false);
      if (!useIdentityColumn)
        source.ActionQueue.AddAction(new EntityInsertAction(id, propertyValuesToInsert, entity, version, persister, (ISessionImplementor) source));
      this.CascadeAfterSave(source, persister, entity, anything);
      this.MarkInterceptorDirty(entity, persister, source);
      return id;
    }

    private void MarkInterceptorDirty(
      object entity,
      IEntityPersister persister,
      IEventSource source)
    {
      if (!FieldInterceptionHelper.IsInstrumented(entity))
        return;
      FieldInterceptionHelper.InjectFieldInterceptor(entity, persister.EntityName, persister.GetMappedClass(source.EntityMode), (ISet<string>) null, (ISet<string>) null, (ISessionImplementor) source).MarkDirty();
    }

    protected virtual IDictionary GetMergeMap(object anything) => (IDictionary) null;

    protected virtual bool VisitCollectionsBeforeSave(
      object entity,
      object id,
      object[] values,
      IType[] types,
      IEventSource source)
    {
      WrapVisitor wrapVisitor = new WrapVisitor(source);
      wrapVisitor.ProcessEntityPropertyValues(values, types);
      return wrapVisitor.SubstitutionRequired;
    }

    protected virtual bool SubstituteValuesIfNecessary(
      object entity,
      object id,
      object[] values,
      IEntityPersister persister,
      ISessionImplementor source)
    {
      bool flag = source.Interceptor.OnSave(entity, id, values, persister.PropertyNames, persister.PropertyTypes);
      if (persister.IsVersioned)
      {
        object version = values[persister.VersionProperty];
        flag |= Versioning.SeedVersion(values, persister.VersionProperty, persister.VersionType, persister.IsUnsavedVersion(version), source);
      }
      return flag;
    }

    protected virtual void CascadeBeforeSave(
      IEventSource source,
      IEntityPersister persister,
      object entity,
      object anything)
    {
      source.PersistenceContext.IncrementCascadeLevel();
      try
      {
        new Cascade(this.CascadeAction, CascadePoint.BeforeInsertAfterDelete, source).CascadeOn(persister, entity, anything);
      }
      finally
      {
        source.PersistenceContext.DecrementCascadeLevel();
      }
    }

    protected virtual void CascadeAfterSave(
      IEventSource source,
      IEntityPersister persister,
      object entity,
      object anything)
    {
      source.PersistenceContext.IncrementCascadeLevel();
      try
      {
        new Cascade(this.CascadeAction, CascadePoint.AfterInsertBeforeDelete, source).CascadeOn(persister, entity, anything);
      }
      finally
      {
        source.PersistenceContext.DecrementCascadeLevel();
      }
    }

    protected virtual EntityState GetEntityState(
      object entity,
      string entityName,
      EntityEntry entry,
      ISessionImplementor source)
    {
      if (entry != null)
      {
        if (entry.Status != Status.Deleted)
        {
          if (AbstractSaveEventListener.log.IsDebugEnabled)
            AbstractSaveEventListener.log.Debug((object) ("persistent instance of: " + this.GetLoggableName(entityName, entity)));
          return EntityState.Persistent;
        }
        if (AbstractSaveEventListener.log.IsDebugEnabled)
          AbstractSaveEventListener.log.Debug((object) ("deleted instance of: " + this.GetLoggableName(entityName, entity)));
        return EntityState.Deleted;
      }
      if (ForeignKeys.IsTransient(entityName, entity, this.AssumedUnsaved, source))
      {
        if (AbstractSaveEventListener.log.IsDebugEnabled)
          AbstractSaveEventListener.log.Debug((object) ("transient instance of: " + this.GetLoggableName(entityName, entity)));
        return EntityState.Transient;
      }
      if (AbstractSaveEventListener.log.IsDebugEnabled)
        AbstractSaveEventListener.log.Debug((object) ("detached instance of: " + this.GetLoggableName(entityName, entity)));
      return EntityState.Detached;
    }

    protected virtual string GetLoggableName(string entityName, object entity)
    {
      return entityName ?? entity.GetType().FullName;
    }
  }
}
