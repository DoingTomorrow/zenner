// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.IPersistenceContext
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using NHibernate.Collection;
using NHibernate.Engine.Loading;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine
{
  public interface IPersistenceContext
  {
    bool IsStateless { get; }

    ISessionImplementor Session { get; }

    LoadContexts LoadContexts { get; }

    BatchFetchQueue BatchFetchQueue { get; }

    ISet NullifiableEntityKeys { get; }

    IDictionary<EntityKey, object> EntitiesByKey { get; }

    IDictionary EntityEntries { get; }

    IDictionary CollectionEntries { get; }

    IDictionary<CollectionKey, IPersistentCollection> CollectionsByKey { get; }

    int CascadeLevel { get; }

    bool Flushing { get; set; }

    bool DefaultReadOnly { get; set; }

    void AddUnownedCollection(CollectionKey key, IPersistentCollection collection);

    IPersistentCollection UseUnownedCollection(CollectionKey key);

    void Clear();

    bool HasNonReadOnlyEntities { get; }

    void SetEntryStatus(EntityEntry entry, Status status);

    void AfterTransactionCompletion();

    object[] GetDatabaseSnapshot(object id, IEntityPersister persister);

    object[] GetCachedDatabaseSnapshot(EntityKey key);

    object[] GetNaturalIdSnapshot(object id, IEntityPersister persister);

    void AddEntity(EntityKey key, object entity);

    object GetEntity(EntityKey key);

    bool ContainsEntity(EntityKey key);

    object RemoveEntity(EntityKey key);

    object GetEntity(EntityUniqueKey euk);

    void AddEntity(EntityUniqueKey euk, object entity);

    EntityEntry GetEntry(object entity);

    EntityEntry RemoveEntry(object entity);

    bool IsEntryFor(object entity);

    CollectionEntry GetCollectionEntry(IPersistentCollection coll);

    EntityEntry AddEntity(
      object entity,
      Status status,
      object[] loadedState,
      EntityKey entityKey,
      object version,
      LockMode lockMode,
      bool existsInDatabase,
      IEntityPersister persister,
      bool disableVersionIncrement,
      bool lazyPropertiesAreUnfetched);

    EntityEntry AddEntry(
      object entity,
      Status status,
      object[] loadedState,
      object rowId,
      object id,
      object version,
      LockMode lockMode,
      bool existsInDatabase,
      IEntityPersister persister,
      bool disableVersionIncrement,
      bool lazyPropertiesAreUnfetched);

    bool ContainsCollection(IPersistentCollection collection);

    bool ContainsProxy(INHibernateProxy proxy);

    bool ReassociateIfUninitializedProxy(object value);

    void ReassociateProxy(object value, object id);

    object Unproxy(object maybeProxy);

    object UnproxyAndReassociate(object maybeProxy);

    void CheckUniqueness(EntityKey key, object obj);

    object NarrowProxy(
      INHibernateProxy proxy,
      IEntityPersister persister,
      EntityKey key,
      object obj);

    object ProxyFor(IEntityPersister persister, EntityKey key, object impl);

    object ProxyFor(object impl);

    object GetCollectionOwner(object key, ICollectionPersister collectionPersister);

    object GetLoadedCollectionOwnerOrNull(IPersistentCollection collection);

    object GetLoadedCollectionOwnerIdOrNull(IPersistentCollection collection);

    void AddUninitializedCollection(
      ICollectionPersister persister,
      IPersistentCollection collection,
      object id);

    void AddUninitializedDetachedCollection(
      ICollectionPersister persister,
      IPersistentCollection collection);

    void AddNewCollection(ICollectionPersister persister, IPersistentCollection collection);

    void AddInitializedDetachedCollection(
      ICollectionPersister collectionPersister,
      IPersistentCollection collection);

    CollectionEntry AddInitializedCollection(
      ICollectionPersister persister,
      IPersistentCollection collection,
      object id);

    IPersistentCollection GetCollection(CollectionKey collectionKey);

    void AddNonLazyCollection(IPersistentCollection collection);

    void InitializeNonLazyCollections();

    IPersistentCollection GetCollectionHolder(object array);

    void AddCollectionHolder(IPersistentCollection holder);

    IPersistentCollection RemoveCollectionHolder(object array);

    object GetSnapshot(IPersistentCollection coll);

    CollectionEntry GetCollectionEntryOrNull(object collection);

    object GetProxy(EntityKey key);

    void AddProxy(EntityKey key, INHibernateProxy proxy);

    object RemoveProxy(EntityKey key);

    int IncrementCascadeLevel();

    int DecrementCascadeLevel();

    void BeforeLoad();

    void AfterLoad();

    object GetOwnerId(string entity, string property, object childObject, IDictionary mergeMap);

    object GetIndexInOwner(
      string entity,
      string property,
      object childObject,
      IDictionary mergeMap);

    void AddNullProperty(EntityKey ownerKey, string propertyName);

    bool IsPropertyNull(EntityKey ownerKey, string propertyName);

    void SetReadOnly(object entityOrProxy, bool readOnly);

    bool IsReadOnly(object entityOrProxy);

    void ReplaceDelayedEntityIdentityInsertKeys(EntityKey oldKey, object generatedId);

    bool IsLoadFinished { get; }

    void AddChildParent(object child, object parent);

    void RemoveChildParent(object child);
  }
}
