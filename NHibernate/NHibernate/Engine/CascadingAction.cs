// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.CascadingAction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using NHibernate.Collection;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Type;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Engine
{
  public abstract class CascadingAction
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (CascadingAction));
    public static readonly CascadingAction Delete = (CascadingAction) new CascadingAction.DeleteCascadingAction();
    public static readonly CascadingAction Lock = (CascadingAction) new CascadingAction.LockCascadingAction();
    public static readonly CascadingAction Refresh = (CascadingAction) new CascadingAction.RefreshCascadingAction();
    public static readonly CascadingAction Evict = (CascadingAction) new CascadingAction.EvictCascadingAction();
    public static readonly CascadingAction SaveUpdate = (CascadingAction) new CascadingAction.SaveUpdateCascadingAction();
    public static readonly CascadingAction Merge = (CascadingAction) new CascadingAction.MergeCascadingAction();
    public static readonly CascadingAction SaveUpdateCopy = (CascadingAction) new CascadingAction.SaveUpdateCopyCascadingAction();
    public static readonly CascadingAction Persist = (CascadingAction) new CascadingAction.PersistCascadingAction();
    public static readonly CascadingAction PersistOnFlush = (CascadingAction) new CascadingAction.PersistOnFlushCascadingAction();
    public static readonly CascadingAction Replicate = (CascadingAction) new CascadingAction.ReplicateCascadingAction();

    public abstract void Cascade(
      IEventSource session,
      object child,
      string entityName,
      object anything,
      bool isCascadeDeleteEnabled);

    public abstract IEnumerable GetCascadableChildrenIterator(
      IEventSource session,
      CollectionType collectionType,
      object collection);

    public abstract bool DeleteOrphans { get; }

    public virtual bool RequiresNoCascadeChecking => false;

    public virtual void NoCascade(
      IEventSource session,
      object child,
      object parent,
      IEntityPersister persister,
      int propertyIndex)
    {
    }

    public virtual bool PerformOnLazyProperty => true;

    private static IEnumerable GetAllElementsIterator(
      IEventSource session,
      CollectionType collectionType,
      object collection)
    {
      return collectionType.GetElementsIterator(collection, (ISessionImplementor) session);
    }

    public static IEnumerable GetLoadedElementsIterator(
      ISessionImplementor session,
      CollectionType collectionType,
      object collection)
    {
      return CascadingAction.CollectionIsInitialized(collection) ? collectionType.GetElementsIterator(collection, session) : ((IPersistentCollection) collection).QueuedAdditionIterator;
    }

    private static bool CollectionIsInitialized(object collection)
    {
      return !(collection is IPersistentCollection persistentCollection) || persistentCollection.WasInitialized;
    }

    private class DeleteCascadingAction : CascadingAction
    {
      public override void Cascade(
        IEventSource session,
        object child,
        string entityName,
        object anything,
        bool isCascadeDeleteEnabled)
      {
        if (CascadingAction.log.IsDebugEnabled)
          CascadingAction.log.Debug((object) ("cascading to delete: " + entityName));
        session.Delete(entityName, child, isCascadeDeleteEnabled, (ISet) anything);
      }

      public override IEnumerable GetCascadableChildrenIterator(
        IEventSource session,
        CollectionType collectionType,
        object collection)
      {
        return CascadingAction.GetAllElementsIterator(session, collectionType, collection);
      }

      public override bool DeleteOrphans => true;
    }

    private class LockCascadingAction : CascadingAction
    {
      public override void Cascade(
        IEventSource session,
        object child,
        string entityName,
        object anything,
        bool isCascadeDeleteEnabled)
      {
        if (CascadingAction.log.IsDebugEnabled)
          CascadingAction.log.Debug((object) ("cascading to lock: " + entityName));
        session.Lock(entityName, child, LockMode.None);
      }

      public override IEnumerable GetCascadableChildrenIterator(
        IEventSource session,
        CollectionType collectionType,
        object collection)
      {
        return CascadingAction.GetLoadedElementsIterator((ISessionImplementor) session, collectionType, collection);
      }

      public override bool DeleteOrphans => false;
    }

    private class RefreshCascadingAction : CascadingAction
    {
      public override void Cascade(
        IEventSource session,
        object child,
        string entityName,
        object anything,
        bool isCascadeDeleteEnabled)
      {
        if (CascadingAction.log.IsDebugEnabled)
          CascadingAction.log.Debug((object) ("cascading to refresh: " + entityName));
        session.Refresh(child, (IDictionary) anything);
      }

      public override IEnumerable GetCascadableChildrenIterator(
        IEventSource session,
        CollectionType collectionType,
        object collection)
      {
        return CascadingAction.GetLoadedElementsIterator((ISessionImplementor) session, collectionType, collection);
      }

      public override bool DeleteOrphans => false;
    }

    private class EvictCascadingAction : CascadingAction
    {
      public override void Cascade(
        IEventSource session,
        object child,
        string entityName,
        object anything,
        bool isCascadeDeleteEnabled)
      {
        if (CascadingAction.log.IsDebugEnabled)
          CascadingAction.log.Debug((object) ("cascading to evict: " + entityName));
        session.Evict(child);
      }

      public override IEnumerable GetCascadableChildrenIterator(
        IEventSource session,
        CollectionType collectionType,
        object collection)
      {
        return CascadingAction.GetLoadedElementsIterator((ISessionImplementor) session, collectionType, collection);
      }

      public override bool DeleteOrphans => false;

      public override bool PerformOnLazyProperty => false;
    }

    private class SaveUpdateCascadingAction : CascadingAction
    {
      public override void Cascade(
        IEventSource session,
        object child,
        string entityName,
        object anything,
        bool isCascadeDeleteEnabled)
      {
        if (CascadingAction.log.IsDebugEnabled)
          CascadingAction.log.Debug((object) ("cascading to saveOrUpdate: " + entityName));
        session.SaveOrUpdate(entityName, child);
      }

      public override IEnumerable GetCascadableChildrenIterator(
        IEventSource session,
        CollectionType collectionType,
        object collection)
      {
        return CascadingAction.GetLoadedElementsIterator((ISessionImplementor) session, collectionType, collection);
      }

      public override bool DeleteOrphans => true;

      public override bool PerformOnLazyProperty => false;
    }

    private class MergeCascadingAction : CascadingAction
    {
      public override void Cascade(
        IEventSource session,
        object child,
        string entityName,
        object anything,
        bool isCascadeDeleteEnabled)
      {
        if (CascadingAction.log.IsDebugEnabled)
          CascadingAction.log.Debug((object) ("cascading to merge: " + entityName));
        session.Merge(entityName, child, (IDictionary) anything);
      }

      public override IEnumerable GetCascadableChildrenIterator(
        IEventSource session,
        CollectionType collectionType,
        object collection)
      {
        return CascadingAction.GetLoadedElementsIterator((ISessionImplementor) session, collectionType, collection);
      }

      public override bool DeleteOrphans => false;
    }

    [Obsolete("Replaced by MergeCascadingAction")]
    private class SaveUpdateCopyCascadingAction : CascadingAction
    {
      public override void Cascade(
        IEventSource session,
        object child,
        string entityName,
        object anything,
        bool isCascadeDeleteEnabled)
      {
        if (CascadingAction.log.IsDebugEnabled)
          CascadingAction.log.Debug((object) ("cascading to saveOrUpdateCopy: " + entityName));
        session.SaveOrUpdateCopy(entityName, child, (IDictionary) anything);
      }

      public override IEnumerable GetCascadableChildrenIterator(
        IEventSource session,
        CollectionType collectionType,
        object collection)
      {
        return CascadingAction.GetLoadedElementsIterator((ISessionImplementor) session, collectionType, collection);
      }

      public override bool DeleteOrphans => false;
    }

    private class PersistCascadingAction : CascadingAction
    {
      public override void Cascade(
        IEventSource session,
        object child,
        string entityName,
        object anything,
        bool isCascadeDeleteEnabled)
      {
        if (CascadingAction.log.IsDebugEnabled)
          CascadingAction.log.Debug((object) ("cascading to persist: " + entityName));
        session.Persist(entityName, child, (IDictionary) anything);
      }

      public override IEnumerable GetCascadableChildrenIterator(
        IEventSource session,
        CollectionType collectionType,
        object collection)
      {
        return CascadingAction.GetLoadedElementsIterator((ISessionImplementor) session, collectionType, collection);
      }

      public override bool DeleteOrphans => false;

      public override bool PerformOnLazyProperty => false;
    }

    private class PersistOnFlushCascadingAction : CascadingAction
    {
      public override void Cascade(
        IEventSource session,
        object child,
        string entityName,
        object anything,
        bool isCascadeDeleteEnabled)
      {
        if (CascadingAction.log.IsDebugEnabled)
          CascadingAction.log.Debug((object) ("cascading to persistOnFlush: " + entityName));
        session.PersistOnFlush(entityName, child, (IDictionary) anything);
      }

      public override IEnumerable GetCascadableChildrenIterator(
        IEventSource session,
        CollectionType collectionType,
        object collection)
      {
        return CascadingAction.GetLoadedElementsIterator((ISessionImplementor) session, collectionType, collection);
      }

      public override bool DeleteOrphans => true;

      public override bool RequiresNoCascadeChecking => true;

      public override void NoCascade(
        IEventSource session,
        object child,
        object parent,
        IEntityPersister persister,
        int propertyIndex)
      {
        if (child == null)
          return;
        IType propertyType = persister.PropertyTypes[propertyIndex];
        if (!propertyType.IsEntityType)
          return;
        string associatedEntityName = ((EntityType) propertyType).GetAssociatedEntityName(session.Factory);
        if (!this.IsInManagedState(child, session) && !child.IsProxy() && ForeignKeys.IsTransient(associatedEntityName, child, new bool?(), (ISessionImplementor) session))
          throw new TransientObjectException(string.Format("object references an unsaved transient instance - save the transient instance before flushing or set cascade action for the property to something that would make it autosave: {0}.{1} -> {2}", (object) persister.EntityName, (object) persister.PropertyNames[propertyIndex], (object) associatedEntityName));
      }

      public override bool PerformOnLazyProperty => false;

      private bool IsInManagedState(object child, IEventSource session)
      {
        EntityEntry entry = session.PersistenceContext.GetEntry(child);
        if (entry == null)
          return false;
        return entry.Status == Status.Loaded || entry.Status == Status.ReadOnly;
      }
    }

    private class ReplicateCascadingAction : CascadingAction
    {
      public override void Cascade(
        IEventSource session,
        object child,
        string entityName,
        object anything,
        bool isCascadeDeleteEnabled)
      {
        if (CascadingAction.log.IsDebugEnabled)
          CascadingAction.log.Debug((object) ("cascading to replicate: " + entityName));
        session.Replicate(entityName, child, (ReplicationMode) anything);
      }

      public override IEnumerable GetCascadableChildrenIterator(
        IEventSource session,
        CollectionType collectionType,
        object collection)
      {
        return CascadingAction.GetLoadedElementsIterator((ISessionImplementor) session, collectionType, collection);
      }

      public override bool DeleteOrphans => false;
    }
  }
}
