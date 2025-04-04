// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Collections
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Engine
{
  public static class Collections
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (Collections));

    public static void ProcessUnreachableCollection(
      IPersistentCollection coll,
      ISessionImplementor session)
    {
      if (coll.Owner == null)
        Collections.ProcessNeverReferencedCollection(coll, session);
      else
        Collections.ProcessDereferencedCollection(coll, session);
    }

    private static void ProcessDereferencedCollection(
      IPersistentCollection coll,
      ISessionImplementor session)
    {
      IPersistenceContext persistenceContext = session.PersistenceContext;
      CollectionEntry collectionEntry = persistenceContext.GetCollectionEntry(coll);
      ICollectionPersister loadedPersister = collectionEntry.LoadedPersister;
      if (Collections.log.IsDebugEnabled && loadedPersister != null)
        Collections.log.Debug((object) ("Collection dereferenced: " + MessageHelper.InfoString(loadedPersister, collectionEntry.LoadedKey, session.Factory)));
      if (loadedPersister != null && loadedPersister.HasOrphanDelete)
      {
        EntityKey key = new EntityKey(loadedPersister.OwnerEntityPersister.GetIdentifier(coll.Owner, session.EntityMode), loadedPersister.OwnerEntityPersister, session.EntityMode);
        EntityEntry entry = persistenceContext.GetEntry(persistenceContext.GetEntity(key) ?? throw new AssertionFailure("collection owner not associated with session: " + loadedPersister.Role));
        if (entry != null && entry.Status != Status.Deleted && entry.Status != Status.Gone)
          throw new HibernateException("A collection with cascade=\"all-delete-orphan\" was no longer referenced by the owning entity instance: " + loadedPersister.Role);
      }
      collectionEntry.CurrentPersister = (ICollectionPersister) null;
      collectionEntry.CurrentKey = (object) null;
      Collections.PrepareCollectionForUpdate(coll, collectionEntry, session.EntityMode, session.Factory);
    }

    private static void ProcessNeverReferencedCollection(
      IPersistentCollection coll,
      ISessionImplementor session)
    {
      CollectionEntry collectionEntry = session.PersistenceContext.GetCollectionEntry(coll);
      Collections.log.Debug((object) ("Found collection with unloaded owner: " + MessageHelper.InfoString(collectionEntry.LoadedPersister, collectionEntry.LoadedKey, session.Factory)));
      collectionEntry.CurrentPersister = collectionEntry.LoadedPersister;
      collectionEntry.CurrentKey = collectionEntry.LoadedKey;
      Collections.PrepareCollectionForUpdate(coll, collectionEntry, session.EntityMode, session.Factory);
    }

    public static void ProcessReachableCollection(
      IPersistentCollection collection,
      CollectionType type,
      object entity,
      ISessionImplementor session)
    {
      collection.Owner = entity;
      CollectionEntry collectionEntry = session.PersistenceContext.GetCollectionEntry(collection);
      if (collectionEntry == null)
        throw new HibernateException(string.Format("Found two representations of same collection: {0}", (object) type.Role));
      collectionEntry.IsReached = !collectionEntry.IsReached ? true : throw new HibernateException(string.Format("Found shared references to a collection: {0}", (object) type.Role));
      ISessionFactoryImplementor factory = session.Factory;
      ICollectionPersister collectionPersister = factory.GetCollectionPersister(type.Role);
      collectionEntry.CurrentPersister = collectionPersister;
      collectionEntry.CurrentKey = type.GetKeyOfOwner(entity, session);
      if (Collections.log.IsDebugEnabled)
        Collections.log.Debug((object) ("Collection found: " + MessageHelper.InfoString(collectionPersister, collectionEntry.CurrentKey, factory) + ", was: " + MessageHelper.InfoString(collectionEntry.LoadedPersister, collectionEntry.LoadedKey, factory) + (collection.WasInitialized ? " (initialized)" : " (uninitialized)")));
      Collections.PrepareCollectionForUpdate(collection, collectionEntry, session.EntityMode, factory);
    }

    private static void PrepareCollectionForUpdate(
      IPersistentCollection collection,
      CollectionEntry entry,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      entry.IsProcessed = !entry.IsProcessed ? true : throw new AssertionFailure("collection was processed twice by flush()");
      ICollectionPersister loadedPersister = entry.LoadedPersister;
      ICollectionPersister currentPersister = entry.CurrentPersister;
      if (loadedPersister == null && currentPersister == null)
        return;
      if (loadedPersister != currentPersister || !currentPersister.KeyType.IsEqual(entry.LoadedKey, entry.CurrentKey, entityMode, factory))
      {
        if (loadedPersister != null && currentPersister != null && loadedPersister.HasOrphanDelete)
          throw new HibernateException("Don't change the reference to a collection with cascade=\"all-delete-orphan\": " + loadedPersister.Role);
        if (currentPersister != null)
          entry.IsDorecreate = true;
        if (loadedPersister == null)
          return;
        entry.IsDoremove = true;
        if (!entry.IsDorecreate)
          return;
        Collections.log.Debug((object) "Forcing collection initialization");
        collection.ForceInitialization();
      }
      else
      {
        if (!collection.IsDirty)
          return;
        entry.IsDoupdate = true;
      }
    }
  }
}
