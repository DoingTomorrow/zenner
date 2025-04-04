// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Cascade
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using NHibernate.Collection;
using NHibernate.Event;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections;

#nullable disable
namespace NHibernate.Engine
{
  public sealed class Cascade
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (Cascade));
    private CascadePoint point;
    private readonly IEventSource eventSource;
    private readonly CascadingAction action;

    public Cascade(CascadingAction action, CascadePoint point, IEventSource eventSource)
    {
      this.point = point;
      this.eventSource = eventSource;
      this.action = action;
    }

    public void CascadeOn(IEntityPersister persister, object parent)
    {
      this.CascadeOn(persister, parent, (object) null);
    }

    public void CascadeOn(IEntityPersister persister, object parent, object anything)
    {
      if (!persister.HasCascades && !this.action.RequiresNoCascadeChecking)
        return;
      Cascade.log.Info((object) ("processing cascade " + (object) this.action + " for: " + persister.EntityName));
      IType[] propertyTypes = persister.PropertyTypes;
      CascadeStyle[] propertyCascadeStyles = persister.PropertyCascadeStyles;
      EntityMode entityMode = this.eventSource.EntityMode;
      bool flag = persister.HasUninitializedLazyProperties(parent, entityMode);
      for (int index = 0; index < propertyTypes.Length; ++index)
      {
        CascadeStyle style = propertyCascadeStyles[index];
        if (!flag || !persister.PropertyLaziness[index] || this.action.PerformOnLazyProperty)
        {
          if (style.DoCascade(this.action))
            this.CascadeProperty(parent, persister.GetPropertyValue(parent, index, entityMode), propertyTypes[index], style, anything, false);
          else if (this.action.RequiresNoCascadeChecking)
            this.action.NoCascade(this.eventSource, persister.GetPropertyValue(parent, index, entityMode), parent, persister, index);
        }
      }
      Cascade.log.Info((object) ("done processing cascade " + (object) this.action + " for: " + persister.EntityName));
    }

    private void CascadeProperty(
      object parent,
      object child,
      IType type,
      CascadeStyle style,
      object anything,
      bool isCascadeDeleteEnabled)
    {
      if (child == null)
        return;
      if (type.IsAssociationType)
      {
        if (!this.CascadeAssociationNow((IAssociationType) type))
          return;
        this.CascadeAssociation(parent, child, type, style, anything, isCascadeDeleteEnabled);
      }
      else
      {
        if (!type.IsComponentType)
          return;
        this.CascadeComponent(parent, child, (IAbstractComponentType) type, anything);
      }
    }

    private bool CascadeAssociationNow(IAssociationType associationType)
    {
      if (!associationType.ForeignKeyDirection.CascadeNow(this.point))
        return false;
      return this.eventSource.EntityMode != EntityMode.Xml || associationType.IsEmbeddedInXML;
    }

    private void CascadeComponent(
      object parent,
      object child,
      IAbstractComponentType componentType,
      object anything)
    {
      object[] propertyValues = componentType.GetPropertyValues(child, (ISessionImplementor) this.eventSource);
      IType[] subtypes = componentType.Subtypes;
      for (int i = 0; i < subtypes.Length; ++i)
      {
        CascadeStyle cascadeStyle = componentType.GetCascadeStyle(i);
        if (cascadeStyle.DoCascade(this.action))
          this.CascadeProperty(parent, propertyValues[i], subtypes[i], cascadeStyle, anything, false);
      }
    }

    private void CascadeAssociation(
      object parent,
      object child,
      IType type,
      CascadeStyle style,
      object anything,
      bool isCascadeDeleteEnabled)
    {
      if (type.IsEntityType || type.IsAnyType)
      {
        this.CascadeToOne(parent, child, type, style, anything, isCascadeDeleteEnabled);
      }
      else
      {
        if (!type.IsCollectionType)
          return;
        this.CascadeCollection(parent, child, style, anything, (CollectionType) type);
      }
    }

    private void CascadeCollection(
      object parent,
      object child,
      CascadeStyle style,
      object anything,
      CollectionType type)
    {
      ICollectionPersister collectionPersister = this.eventSource.Factory.GetCollectionPersister(type.Role);
      IType elementType = collectionPersister.ElementType;
      CascadePoint point = this.point;
      if (this.point == CascadePoint.AfterInsertBeforeDelete)
        this.point = CascadePoint.AfterInsertBeforeDeleteViaCollection;
      if (elementType.IsEntityType || elementType.IsAnyType || elementType.IsComponentType)
        this.CascadeCollectionElements(parent, child, type, style, elementType, anything, collectionPersister.CascadeDeleteEnabled);
      this.point = point;
    }

    private void CascadeToOne(
      object parent,
      object child,
      IType type,
      CascadeStyle style,
      object anything,
      bool isCascadeDeleteEnabled)
    {
      string associatedEntityName = type.IsEntityType ? ((EntityType) type).GetAssociatedEntityName() : (string) null;
      if (!style.ReallyDoCascade(this.action))
        return;
      this.eventSource.PersistenceContext.AddChildParent(child, parent);
      try
      {
        this.action.Cascade(this.eventSource, child, associatedEntityName, anything, isCascadeDeleteEnabled);
      }
      finally
      {
        this.eventSource.PersistenceContext.RemoveChildParent(child);
      }
    }

    private void CascadeCollectionElements(
      object parent,
      object child,
      CollectionType collectionType,
      CascadeStyle style,
      IType elemType,
      object anything,
      bool isCascadeDeleteEnabled)
    {
      bool flag = this.eventSource.EntityMode != EntityMode.Xml || ((EntityType) collectionType.GetElementType(this.eventSource.Factory)).IsEmbeddedInXML;
      if (style.ReallyDoCascade(this.action) && flag && child != CollectionType.UnfetchedCollection)
      {
        Cascade.log.Info((object) ("cascade " + (object) this.action + " for collection: " + collectionType.Role));
        foreach (object child1 in this.action.GetCascadableChildrenIterator(this.eventSource, collectionType, child))
          this.CascadeProperty(parent, child1, elemType, style, anything, isCascadeDeleteEnabled);
        Cascade.log.Info((object) ("done cascade " + (object) this.action + " for collection: " + collectionType.Role));
      }
      IPersistentCollection pc = child as IPersistentCollection;
      if (!style.HasOrphanDelete || !this.action.DeleteOrphans || !elemType.IsEntityType || pc == null)
        return;
      Cascade.log.Info((object) ("deleting orphans for collection: " + collectionType.Role));
      this.DeleteOrphans(collectionType.GetAssociatedEntityName(this.eventSource.Factory), pc);
      Cascade.log.Info((object) ("done deleting orphans for collection: " + collectionType.Role));
    }

    private void DeleteOrphans(string entityName, IPersistentCollection pc)
    {
      ICollection collection;
      if (pc.WasInitialized)
      {
        CollectionEntry collectionEntry = this.eventSource.PersistenceContext.GetCollectionEntry(pc);
        collection = collectionEntry == null ? CollectionHelper.EmptyCollection : collectionEntry.GetOrphans(entityName, pc);
      }
      else
        collection = pc.GetQueuedOrphans(entityName);
      foreach (object child in (IEnumerable) collection)
      {
        if (child != null)
        {
          Cascade.log.Info((object) ("deleting orphaned entity instance: " + entityName));
          this.eventSource.Delete(entityName, child, false, (ISet) null);
        }
      }
    }
  }
}
