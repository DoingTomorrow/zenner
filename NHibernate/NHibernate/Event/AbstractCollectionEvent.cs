// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.AbstractCollectionEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using System;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public abstract class AbstractCollectionEvent : AbstractEvent
  {
    private readonly object affectedOwner;
    private readonly string affectedOwnerEntityName;
    private readonly object affectedOwnerId;
    private readonly IPersistentCollection collection;

    protected AbstractCollectionEvent(
      ICollectionPersister collectionPersister,
      IPersistentCollection collection,
      IEventSource source,
      object affectedOwner,
      object affectedOwnerId)
      : base(source)
    {
      this.collection = collection;
      this.affectedOwner = affectedOwner;
      this.affectedOwnerId = affectedOwnerId;
      this.affectedOwnerEntityName = AbstractCollectionEvent.GetAffectedOwnerEntityName(collectionPersister, affectedOwner, source);
    }

    public IPersistentCollection Collection => this.collection;

    public object AffectedOwnerOrNull => this.affectedOwner;

    public object AffectedOwnerIdOrNull => this.affectedOwnerId;

    protected static ICollectionPersister GetLoadedCollectionPersister(
      IPersistentCollection collection,
      IEventSource source)
    {
      return source.PersistenceContext.GetCollectionEntry(collection)?.LoadedPersister;
    }

    protected static object GetLoadedOwnerOrNull(
      IPersistentCollection collection,
      IEventSource source)
    {
      return source.PersistenceContext.GetLoadedCollectionOwnerOrNull(collection);
    }

    protected static object GetLoadedOwnerIdOrNull(
      IPersistentCollection collection,
      IEventSource source)
    {
      return source.PersistenceContext.GetLoadedCollectionOwnerIdOrNull(collection);
    }

    protected static object GetOwnerIdOrNull(object owner, IEventSource source)
    {
      return source.PersistenceContext.GetEntry(owner)?.Id;
    }

    protected static string GetAffectedOwnerEntityName(
      ICollectionPersister collectionPersister,
      object affectedOwner,
      IEventSource source)
    {
      string entityName = collectionPersister == null ? (string) null : collectionPersister.OwnerEntityPersister.EntityName;
      if (affectedOwner != null)
      {
        EntityEntry entry = source.PersistenceContext.GetEntry(affectedOwner);
        if (entry != null && entry.EntityName != null)
          entityName = entry.EntityName;
      }
      return entityName;
    }

    public virtual string GetAffectedOwnerEntityName() => this.affectedOwnerEntityName;
  }
}
