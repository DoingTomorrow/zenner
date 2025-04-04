// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.StatefulPersistenceContext
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using Iesi.Collections.Generic;
using NHibernate.Collection;
using NHibernate.Engine.Loading;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

#nullable disable
namespace NHibernate.Engine
{
  [Serializable]
  public class StatefulPersistenceContext : 
    IPersistenceContext,
    ISerializable,
    IDeserializationCallback
  {
    private const int InitCollectionSize = 8;
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (StatefulPersistenceContext));
    private static readonly IInternalLogger ProxyWarnLog = LoggerProvider.LoggerFor(typeof (StatefulPersistenceContext).FullName + ".ProxyWarnLog");
    public static readonly object NoRow = new object();
    [NonSerialized]
    private ISessionImplementor session;
    private readonly Dictionary<EntityKey, object> entitiesByKey;
    private readonly Dictionary<EntityUniqueKey, object> entitiesByUniqueKey;
    private readonly IDictionary entityEntries;
    private readonly Dictionary<EntityKey, INHibernateProxy> proxiesByKey;
    private readonly Dictionary<EntityKey, object> entitySnapshotsByKey;
    private readonly IDictionary arrayHolders;
    private readonly IDictionary collectionEntries;
    private readonly Dictionary<CollectionKey, IPersistentCollection> collectionsByKey;
    private readonly HashedSet<EntityKey> nullifiableEntityKeys;
    private ISet<AssociationKey> nullAssociations;
    [NonSerialized]
    private List<IPersistentCollection> nonlazyCollections;
    private Dictionary<CollectionKey, IPersistentCollection> unownedCollections;
    private bool hasNonReadOnlyEntities;
    [NonSerialized]
    private IDictionary parentsByChild;
    [NonSerialized]
    private int cascading;
    [NonSerialized]
    private bool flushing;
    [NonSerialized]
    private int loadCounter;
    [NonSerialized]
    private LoadContexts loadContexts;
    [NonSerialized]
    private BatchFetchQueue batchFetchQueue;
    private bool defaultReadOnly;

    public StatefulPersistenceContext(ISessionImplementor session)
    {
      this.loadCounter = 0;
      this.flushing = false;
      this.cascading = 0;
      this.session = session;
      this.entitiesByKey = new Dictionary<EntityKey, object>(8);
      this.entitiesByUniqueKey = new Dictionary<EntityUniqueKey, object>(8);
      this.proxiesByKey = new Dictionary<EntityKey, INHibernateProxy>(8);
      this.entitySnapshotsByKey = new Dictionary<EntityKey, object>(8);
      this.entityEntries = IdentityMap.InstantiateSequenced(8);
      this.collectionEntries = IdentityMap.InstantiateSequenced(8);
      this.collectionsByKey = new Dictionary<CollectionKey, IPersistentCollection>(8);
      this.arrayHolders = IdentityMap.Instantiate(8);
      this.parentsByChild = IdentityMap.Instantiate(8);
      this.nullifiableEntityKeys = new HashedSet<EntityKey>();
      this.InitTransientState();
    }

    private void InitTransientState()
    {
      this.loadContexts = (LoadContexts) null;
      this.nullAssociations = (ISet<AssociationKey>) new HashedSet<AssociationKey>();
      this.nonlazyCollections = new List<IPersistentCollection>(8);
    }

    public bool IsStateless => false;

    public ISessionImplementor Session => this.session;

    public LoadContexts LoadContexts
    {
      get
      {
        if (this.loadContexts == null)
          this.loadContexts = new LoadContexts((IPersistenceContext) this);
        return this.loadContexts;
      }
    }

    public BatchFetchQueue BatchFetchQueue
    {
      get
      {
        if (this.batchFetchQueue == null)
          this.batchFetchQueue = new BatchFetchQueue((IPersistenceContext) this);
        return this.batchFetchQueue;
      }
    }

    public ISet NullifiableEntityKeys => (ISet) this.nullifiableEntityKeys;

    public IDictionary<EntityKey, object> EntitiesByKey
    {
      get => (IDictionary<EntityKey, object>) this.entitiesByKey;
    }

    public IDictionary EntityEntries => this.entityEntries;

    public IDictionary CollectionEntries => this.collectionEntries;

    public IDictionary<CollectionKey, IPersistentCollection> CollectionsByKey
    {
      get => (IDictionary<CollectionKey, IPersistentCollection>) this.collectionsByKey;
    }

    public int CascadeLevel => this.cascading;

    public bool Flushing
    {
      get => this.flushing;
      set => this.flushing = value;
    }

    public void AddUnownedCollection(CollectionKey key, IPersistentCollection collection)
    {
      if (this.unownedCollections == null)
        this.unownedCollections = new Dictionary<CollectionKey, IPersistentCollection>(8);
      this.unownedCollections[key] = collection;
    }

    public IPersistentCollection UseUnownedCollection(CollectionKey key)
    {
      if (this.unownedCollections == null)
        return (IPersistentCollection) null;
      IPersistentCollection persistentCollection;
      if (this.unownedCollections.TryGetValue(key, out persistentCollection))
        this.unownedCollections.Remove(key);
      return persistentCollection;
    }

    public void Clear()
    {
      foreach (INHibernateProxy nhibernateProxy in this.proxiesByKey.Values)
        nhibernateProxy.HibernateLazyInitializer.UnsetSession();
      foreach (DictionaryEntry concurrentEntry in (IEnumerable) IdentityMap.ConcurrentEntries(this.collectionEntries))
        ((IPersistentCollection) concurrentEntry.Key).UnsetSession(this.Session);
      this.arrayHolders.Clear();
      this.entitiesByKey.Clear();
      this.entitiesByUniqueKey.Clear();
      this.entityEntries.Clear();
      this.entitySnapshotsByKey.Clear();
      this.collectionsByKey.Clear();
      this.collectionEntries.Clear();
      if (this.unownedCollections != null)
        this.unownedCollections.Clear();
      this.proxiesByKey.Clear();
      this.nullifiableEntityKeys.Clear();
      if (this.batchFetchQueue != null)
        this.batchFetchQueue.Clear();
      this.hasNonReadOnlyEntities = false;
      if (this.loadContexts != null)
        this.loadContexts.Cleanup();
      this.parentsByChild.Clear();
    }

    public bool HasNonReadOnlyEntities => this.hasNonReadOnlyEntities;

    public bool DefaultReadOnly
    {
      get => this.defaultReadOnly;
      set => this.defaultReadOnly = value;
    }

    private void SetHasNonReadOnlyEnties(Status value)
    {
      if (value != Status.Deleted && value != Status.Loaded && value != Status.Saving)
        return;
      this.hasNonReadOnlyEntities = true;
    }

    public void SetEntryStatus(EntityEntry entry, Status status)
    {
      entry.Status = status;
      this.SetHasNonReadOnlyEnties(status);
    }

    public void AfterTransactionCompletion()
    {
      foreach (EntityEntry entityEntry in (IEnumerable) this.entityEntries.Values)
        entityEntry.LockMode = LockMode.None;
    }

    public object[] GetDatabaseSnapshot(object id, IEntityPersister persister)
    {
      EntityKey key = new EntityKey(id, persister, this.session.EntityMode);
      object obj;
      if (this.entitySnapshotsByKey.TryGetValue(key, out obj))
        return obj != StatefulPersistenceContext.NoRow ? (object[]) obj : (object[]) null;
      object[] databaseSnapshot = persister.GetDatabaseSnapshot(id, this.session);
      this.entitySnapshotsByKey[key] = (object) databaseSnapshot ?? StatefulPersistenceContext.NoRow;
      return databaseSnapshot;
    }

    public object[] GetCachedDatabaseSnapshot(EntityKey key)
    {
      object obj;
      if (!this.entitySnapshotsByKey.TryGetValue(key, out obj))
        return (object[]) null;
      return obj != StatefulPersistenceContext.NoRow ? (object[]) obj : throw new HibernateException("persistence context reported no row snapshot for " + MessageHelper.InfoString(key.EntityName, key.Identifier));
    }

    public object[] GetNaturalIdSnapshot(object id, IEntityPersister persister)
    {
      if (!persister.HasNaturalIdentifier)
        return (object[]) null;
      int[] identifierProperties = persister.NaturalIdentifierProperties;
      bool[] propertyUpdateability = persister.PropertyUpdateability;
      bool flag = true;
      for (int index = 0; index < identifierProperties.Length; ++index)
      {
        if (!propertyUpdateability[identifierProperties[index]])
        {
          flag = false;
          break;
        }
      }
      if (!flag)
        return persister.GetNaturalIdentifierSnapshot(id, this.session);
      object[] databaseSnapshot = this.GetDatabaseSnapshot(id, persister);
      if (databaseSnapshot == StatefulPersistenceContext.NoRow)
        return (object[]) null;
      object[] naturalIdSnapshot = new object[identifierProperties.Length];
      for (int index = 0; index < identifierProperties.Length; ++index)
        naturalIdSnapshot[index] = databaseSnapshot[identifierProperties[index]];
      return naturalIdSnapshot;
    }

    public void AddEntity(EntityKey key, object entity)
    {
      this.entitiesByKey[key] = entity;
      this.BatchFetchQueue.RemoveBatchLoadableEntityKey(key);
    }

    public object GetEntity(EntityKey key)
    {
      object entity;
      this.entitiesByKey.TryGetValue(key, out entity);
      return entity;
    }

    public bool ContainsEntity(EntityKey key) => this.entitiesByKey.ContainsKey(key);

    public object RemoveEntity(EntityKey key)
    {
      object obj1 = this.entitiesByKey[key];
      this.entitiesByKey.Remove(key);
      object obj2 = obj1;
      List<EntityUniqueKey> entityUniqueKeyList = new List<EntityUniqueKey>();
      foreach (KeyValuePair<EntityUniqueKey, object> keyValuePair in this.entitiesByUniqueKey)
      {
        if (keyValuePair.Value == obj2)
          entityUniqueKeyList.Add(keyValuePair.Key);
      }
      foreach (EntityUniqueKey key1 in entityUniqueKeyList)
        this.entitiesByUniqueKey.Remove(key1);
      this.entitySnapshotsByKey.Remove(key);
      this.nullifiableEntityKeys.Remove(key);
      this.BatchFetchQueue.RemoveBatchLoadableEntityKey(key);
      this.BatchFetchQueue.RemoveSubselect(key);
      this.parentsByChild.Clear();
      return obj2;
    }

    public object GetEntity(EntityUniqueKey euk)
    {
      object entity;
      this.entitiesByUniqueKey.TryGetValue(euk, out entity);
      return entity;
    }

    public void AddEntity(EntityUniqueKey euk, object entity)
    {
      this.entitiesByUniqueKey[euk] = entity;
    }

    public EntityEntry GetEntry(object entity) => (EntityEntry) this.entityEntries[entity];

    public EntityEntry RemoveEntry(object entity)
    {
      EntityEntry entityEntry = (EntityEntry) this.entityEntries[entity];
      this.entityEntries.Remove(entity);
      return entityEntry;
    }

    public bool IsEntryFor(object entity) => this.entityEntries.Contains(entity);

    public CollectionEntry GetCollectionEntry(IPersistentCollection coll)
    {
      return (CollectionEntry) this.collectionEntries[(object) coll];
    }

    public EntityEntry AddEntity(
      object entity,
      Status status,
      object[] loadedState,
      EntityKey entityKey,
      object version,
      LockMode lockMode,
      bool existsInDatabase,
      IEntityPersister persister,
      bool disableVersionIncrement,
      bool lazyPropertiesAreUnfetched)
    {
      this.AddEntity(entityKey, entity);
      return this.AddEntry(entity, status, loadedState, (object) null, entityKey.Identifier, version, lockMode, existsInDatabase, persister, disableVersionIncrement, lazyPropertiesAreUnfetched);
    }

    public EntityEntry AddEntry(
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
      bool lazyPropertiesAreUnfetched)
    {
      EntityEntry entityEntry = new EntityEntry(status, loadedState, rowId, id, version, lockMode, existsInDatabase, persister, this.session.EntityMode, disableVersionIncrement, lazyPropertiesAreUnfetched);
      this.entityEntries[entity] = (object) entityEntry;
      this.SetHasNonReadOnlyEnties(status);
      return entityEntry;
    }

    public bool ContainsCollection(IPersistentCollection collection)
    {
      return this.collectionEntries.Contains((object) collection);
    }

    public bool ContainsProxy(INHibernateProxy proxy) => this.proxiesByKey.ContainsValue(proxy);

    public bool ReassociateIfUninitializedProxy(object value)
    {
      if (NHibernateUtil.IsInitialized(value))
        return false;
      INHibernateProxy proxy = (INHibernateProxy) value;
      this.ReassociateProxy(proxy.HibernateLazyInitializer, proxy);
      return true;
    }

    public void ReassociateProxy(object value, object id)
    {
      if (!value.IsProxy())
        return;
      INHibernateProxy proxy = value as INHibernateProxy;
      if (StatefulPersistenceContext.log.IsDebugEnabled)
        StatefulPersistenceContext.log.Debug((object) ("setting proxy identifier: " + id));
      ILazyInitializer hibernateLazyInitializer = proxy.HibernateLazyInitializer;
      hibernateLazyInitializer.Identifier = id;
      this.ReassociateProxy(hibernateLazyInitializer, proxy);
    }

    private void ReassociateProxy(ILazyInitializer li, INHibernateProxy proxy)
    {
      if (li.Session == this.Session)
        return;
      IEntityPersister entityPersister = this.session.Factory.GetEntityPersister(li.EntityName);
      EntityKey key = new EntityKey(li.Identifier, entityPersister, this.session.EntityMode);
      if (!this.proxiesByKey.ContainsKey(key))
        this.proxiesByKey[key] = proxy;
      proxy.HibernateLazyInitializer.Session = this.Session;
    }

    public object Unproxy(object maybeProxy)
    {
      if (!maybeProxy.IsProxy())
        return maybeProxy;
      ILazyInitializer hibernateLazyInitializer = (maybeProxy as INHibernateProxy).HibernateLazyInitializer;
      return !hibernateLazyInitializer.IsUninitialized ? hibernateLazyInitializer.GetImplementation() : throw new PersistentObjectException("object was an uninitialized proxy for " + hibernateLazyInitializer.PersistentClass.FullName);
    }

    public object UnproxyAndReassociate(object maybeProxy)
    {
      if (!maybeProxy.IsProxy())
        return maybeProxy;
      INHibernateProxy proxy = maybeProxy as INHibernateProxy;
      ILazyInitializer hibernateLazyInitializer = proxy.HibernateLazyInitializer;
      this.ReassociateProxy(hibernateLazyInitializer, proxy);
      return hibernateLazyInitializer.GetImplementation();
    }

    public void CheckUniqueness(EntityKey key, object obj)
    {
      object entity = this.GetEntity(key);
      if (entity == obj)
        throw new AssertionFailure("object already associated, but no entry was found");
      if (entity != null)
        throw new NonUniqueObjectException(key.Identifier, key.EntityName);
    }

    public object NarrowProxy(
      INHibernateProxy proxy,
      IEntityPersister persister,
      EntityKey key,
      object obj)
    {
      if (!persister.GetConcreteProxyClass(this.session.EntityMode).IsAssignableFrom(proxy.GetType()))
      {
        if (StatefulPersistenceContext.ProxyWarnLog.IsWarnEnabled)
          StatefulPersistenceContext.ProxyWarnLog.Warn((object) ("Narrowing proxy to " + (object) persister.GetConcreteProxyClass(this.session.EntityMode) + " - this operation breaks =="));
        if (obj != null)
        {
          this.proxiesByKey.Remove(key);
          return obj;
        }
        proxy = (INHibernateProxy) persister.CreateProxy(key.Identifier, this.session);
        INHibernateProxy nhibernateProxy = this.proxiesByKey[key];
        this.proxiesByKey[key] = proxy;
        if (nhibernateProxy != null)
        {
          bool flag = nhibernateProxy.HibernateLazyInitializer.ReadOnly;
          proxy.HibernateLazyInitializer.ReadOnly = flag;
        }
        return (object) proxy;
      }
      if (obj != null)
        proxy.HibernateLazyInitializer.SetImplementation(obj);
      return (object) proxy;
    }

    public object ProxyFor(IEntityPersister persister, EntityKey key, object impl)
    {
      INHibernateProxy proxy;
      return !persister.HasProxy || key == null || !this.proxiesByKey.TryGetValue(key, out proxy) ? impl : this.NarrowProxy(proxy, persister, key, impl);
    }

    public object ProxyFor(object impl)
    {
      EntityEntry entry = this.GetEntry(impl);
      IEntityPersister persister = entry.Persister;
      return this.ProxyFor(persister, new EntityKey(entry.Id, persister, this.session.EntityMode), impl);
    }

    public object GetCollectionOwner(object key, ICollectionPersister collectionPersister)
    {
      return this.GetEntity(new EntityKey(key, collectionPersister.OwnerEntityPersister, this.session.EntityMode));
    }

    public virtual object GetLoadedCollectionOwnerOrNull(IPersistentCollection collection)
    {
      CollectionEntry collectionEntry = this.GetCollectionEntry(collection);
      if (collectionEntry.LoadedPersister == null)
        return (object) null;
      object collectionOwnerOrNull = (object) null;
      object collectionOwnerIdOrNull = this.GetLoadedCollectionOwnerIdOrNull(collectionEntry);
      if (collectionOwnerIdOrNull != null)
        collectionOwnerOrNull = this.GetCollectionOwner(collectionOwnerIdOrNull, collectionEntry.LoadedPersister);
      return collectionOwnerOrNull;
    }

    public virtual object GetLoadedCollectionOwnerIdOrNull(IPersistentCollection collection)
    {
      return this.GetLoadedCollectionOwnerIdOrNull(this.GetCollectionEntry(collection));
    }

    private object GetLoadedCollectionOwnerIdOrNull(CollectionEntry ce)
    {
      return ce == null || ce.LoadedKey == null || ce.LoadedPersister == null ? (object) null : ce.LoadedPersister.CollectionType.GetIdOfOwnerOrNull(ce.LoadedKey, this.session);
    }

    public void AddUninitializedCollection(
      ICollectionPersister persister,
      IPersistentCollection collection,
      object id)
    {
      CollectionEntry entry = new CollectionEntry(collection, persister, id, this.flushing);
      this.AddCollection(collection, entry, id);
    }

    public void AddUninitializedDetachedCollection(
      ICollectionPersister persister,
      IPersistentCollection collection)
    {
      CollectionEntry entry = new CollectionEntry(persister, collection.Key);
      this.AddCollection(collection, entry, collection.Key);
    }

    public void AddNewCollection(ICollectionPersister persister, IPersistentCollection collection)
    {
      this.AddCollection(collection, persister);
    }

    private void AddCollection(IPersistentCollection coll, CollectionEntry entry, object key)
    {
      this.collectionEntries[(object) coll] = (object) entry;
      CollectionKey key1 = new CollectionKey(entry.LoadedPersister, key, this.session.EntityMode);
      IPersistentCollection persistentCollection;
      this.collectionsByKey.TryGetValue(key1, out persistentCollection);
      this.collectionsByKey[key1] = coll;
      IPersistentCollection key2 = persistentCollection;
      if (key2 == null)
        return;
      if (key2 == coll)
        throw new AssertionFailure("bug adding collection twice");
      key2.UnsetSession(this.session);
      this.collectionEntries.Remove((object) key2);
    }

    private void AddCollection(IPersistentCollection collection, ICollectionPersister persister)
    {
      CollectionEntry collectionEntry = new CollectionEntry(persister, collection);
      this.collectionEntries[(object) collection] = (object) collectionEntry;
    }

    public void AddInitializedDetachedCollection(
      ICollectionPersister collectionPersister,
      IPersistentCollection collection)
    {
      if (collection.IsUnreferenced)
      {
        this.AddCollection(collection, collectionPersister);
      }
      else
      {
        CollectionEntry entry = new CollectionEntry(collection, this.session.Factory);
        this.AddCollection(collection, entry, collection.Key);
      }
    }

    public CollectionEntry AddInitializedCollection(
      ICollectionPersister persister,
      IPersistentCollection collection,
      object id)
    {
      CollectionEntry entry = new CollectionEntry(collection, persister, id, this.flushing);
      entry.PostInitialize(collection);
      this.AddCollection(collection, entry, id);
      return entry;
    }

    public IPersistentCollection GetCollection(CollectionKey collectionKey)
    {
      IPersistentCollection persistentCollection;
      return this.collectionsByKey.TryGetValue(collectionKey, out persistentCollection) ? persistentCollection : (IPersistentCollection) null;
    }

    public void AddNonLazyCollection(IPersistentCollection collection)
    {
      this.nonlazyCollections.Add(collection);
    }

    public void InitializeNonLazyCollections()
    {
      if (this.loadCounter != 0)
        return;
      StatefulPersistenceContext.log.Debug((object) "initializing non-lazy collections");
      ++this.loadCounter;
      try
      {
        while (this.nonlazyCollections.Count > 0)
        {
          IPersistentCollection nonlazyCollection = this.nonlazyCollections[this.nonlazyCollections.Count - 1];
          this.nonlazyCollections.RemoveAt(this.nonlazyCollections.Count - 1);
          nonlazyCollection.ForceInitialization();
        }
      }
      finally
      {
        --this.loadCounter;
        this.ClearNullProperties();
      }
    }

    private void ClearNullProperties() => this.nullAssociations.Clear();

    public IPersistentCollection GetCollectionHolder(object array)
    {
      return (IPersistentCollection) this.arrayHolders[array];
    }

    public void AddCollectionHolder(IPersistentCollection holder)
    {
      this.arrayHolders[holder.GetValue()] = (object) holder;
    }

    public IPersistentCollection RemoveCollectionHolder(object array)
    {
      IPersistentCollection arrayHolder = (IPersistentCollection) this.arrayHolders[array];
      this.arrayHolders.Remove(array);
      return arrayHolder;
    }

    public object GetSnapshot(IPersistentCollection coll) => this.GetCollectionEntry(coll).Snapshot;

    public CollectionEntry GetCollectionEntryOrNull(object collection)
    {
      if (!(collection is IPersistentCollection coll))
      {
        coll = this.GetCollectionHolder(collection);
        if (coll == null)
        {
          foreach (IPersistentCollection key in (IEnumerable) this.collectionEntries.Keys)
          {
            if (key.IsWrapper(collection))
            {
              coll = key;
              break;
            }
          }
        }
      }
      return coll != null ? this.GetCollectionEntry(coll) : (CollectionEntry) null;
    }

    public object GetProxy(EntityKey key)
    {
      INHibernateProxy nhibernateProxy;
      return this.proxiesByKey.TryGetValue(key, out nhibernateProxy) ? (object) nhibernateProxy : (object) null;
    }

    public void AddProxy(EntityKey key, INHibernateProxy proxy) => this.proxiesByKey[key] = proxy;

    public object RemoveProxy(EntityKey key)
    {
      if (this.batchFetchQueue != null)
      {
        this.batchFetchQueue.RemoveBatchLoadableEntityKey(key);
        this.batchFetchQueue.RemoveSubselect(key);
      }
      INHibernateProxy nhibernateProxy;
      if (this.proxiesByKey.TryGetValue(key, out nhibernateProxy))
        this.proxiesByKey.Remove(key);
      return (object) nhibernateProxy;
    }

    public int IncrementCascadeLevel() => ++this.cascading;

    public int DecrementCascadeLevel() => --this.cascading;

    public void BeforeLoad() => ++this.loadCounter;

    public void AfterLoad() => --this.loadCounter;

    public object GetOwnerId(
      string entityName,
      string propertyName,
      object childEntity,
      IDictionary mergeMap)
    {
      string role = entityName + (object) '.' + propertyName;
      IEntityPersister entityPersister = this.session.Factory.GetEntityPersister(entityName);
      ICollectionPersister collectionPersister = this.session.Factory.GetCollectionPersister(role);
      object obj = this.parentsByChild[childEntity];
      if (obj != null)
      {
        EntityEntry entityEntry = (EntityEntry) this.entityEntries[obj];
        if (entityPersister.IsSubclassEntityName(entityEntry.EntityName) && this.IsFoundInParent(propertyName, childEntity, entityPersister, collectionPersister, obj))
          return this.GetEntry(obj).Id;
        this.parentsByChild.Remove(childEntity);
      }
      foreach (DictionaryEntry entityEntry1 in this.entityEntries)
      {
        EntityEntry entityEntry2 = (EntityEntry) entityEntry1.Value;
        if (entityPersister.IsSubclassEntityName(entityEntry2.EntityName))
        {
          object key = entityEntry1.Key;
          bool flag = this.IsFoundInParent(propertyName, childEntity, entityPersister, collectionPersister, key);
          if (!flag && mergeMap != null)
          {
            object merge1 = mergeMap[key];
            object merge2 = mergeMap[childEntity];
            if (merge1 != null && merge2 != null)
              flag = this.IsFoundInParent(propertyName, merge2, entityPersister, collectionPersister, merge1);
          }
          if (flag)
            return entityEntry2.Id;
        }
      }
      if (mergeMap != null)
      {
        foreach (DictionaryEntry merge in mergeMap)
        {
          if (merge.Key is INHibernateProxy key && entityPersister.IsSubclassEntityName(key.HibernateLazyInitializer.EntityName))
          {
            bool flag = this.IsFoundInParent(propertyName, childEntity, entityPersister, collectionPersister, mergeMap[(object) key]);
            if (!flag)
              flag = this.IsFoundInParent(propertyName, mergeMap[childEntity], entityPersister, collectionPersister, mergeMap[(object) key]);
            if (flag)
              return key.HibernateLazyInitializer.Identifier;
          }
        }
      }
      return (object) null;
    }

    private bool IsFoundInParent(
      string property,
      object childEntity,
      IEntityPersister persister,
      ICollectionPersister collectionPersister,
      object potentialParent)
    {
      object propertyValue = persister.GetPropertyValue(potentialParent, property, this.session.EntityMode);
      return propertyValue != null && NHibernateUtil.IsInitialized(propertyValue) && collectionPersister.CollectionType.Contains(propertyValue, childEntity, this.session);
    }

    public object GetIndexInOwner(
      string entity,
      string property,
      object childEntity,
      IDictionary mergeMap)
    {
      IEntityPersister entityPersister = this.session.Factory.GetEntityPersister(entity);
      ICollectionPersister collectionPersister = this.session.Factory.GetCollectionPersister(entity + (object) '.' + property);
      object obj = this.parentsByChild[childEntity];
      if (obj != null)
      {
        EntityEntry entityEntry = (EntityEntry) this.entityEntries[obj];
        if (entityPersister.IsSubclassEntityName(entityEntry.EntityName))
        {
          object indexInParent = this.GetIndexInParent(property, childEntity, entityPersister, collectionPersister, obj);
          if (indexInParent == null && mergeMap != null)
          {
            object merge1 = mergeMap[obj];
            object merge2 = mergeMap[childEntity];
            if (merge1 != null && merge2 != null)
              indexInParent = this.GetIndexInParent(property, merge2, entityPersister, collectionPersister, merge1);
          }
          if (indexInParent != null)
            return indexInParent;
        }
        else
          this.parentsByChild.Remove(childEntity);
      }
      foreach (DictionaryEntry entityEntry1 in this.entityEntries)
      {
        EntityEntry entityEntry2 = (EntityEntry) entityEntry1.Value;
        if (entityPersister.IsSubclassEntityName(entityEntry2.EntityName))
        {
          object key = entityEntry1.Key;
          object indexInParent = this.GetIndexInParent(property, childEntity, entityPersister, collectionPersister, key);
          if (indexInParent == null && mergeMap != null)
          {
            object merge3 = mergeMap[key];
            object merge4 = mergeMap[childEntity];
            if (merge3 != null && merge4 != null)
              indexInParent = this.GetIndexInParent(property, merge4, entityPersister, collectionPersister, merge3);
          }
          if (indexInParent != null)
            return indexInParent;
        }
      }
      return (object) null;
    }

    private object GetIndexInParent(
      string property,
      object childEntity,
      IEntityPersister persister,
      ICollectionPersister collectionPersister,
      object potentialParent)
    {
      object propertyValue = persister.GetPropertyValue(potentialParent, property, this.session.EntityMode);
      return propertyValue != null && NHibernateUtil.IsInitialized(propertyValue) ? collectionPersister.CollectionType.IndexOf(propertyValue, childEntity) : (object) null;
    }

    public void AddNullProperty(EntityKey ownerKey, string propertyName)
    {
      this.nullAssociations.Add(new AssociationKey(ownerKey, propertyName));
    }

    public bool IsPropertyNull(EntityKey ownerKey, string propertyName)
    {
      return this.nullAssociations.Contains(new AssociationKey(ownerKey, propertyName));
    }

    public void SetReadOnly(object entityOrProxy, bool readOnly)
    {
      if (entityOrProxy == null)
        throw new ArgumentNullException(nameof (entityOrProxy));
      if (this.IsReadOnly(entityOrProxy) == readOnly)
        return;
      if (entityOrProxy is INHibernateProxy)
      {
        INHibernateProxy proxy = (INHibernateProxy) entityOrProxy;
        this.SetProxyReadOnly(proxy, readOnly);
        if (!NHibernateUtil.IsInitialized((object) proxy))
          return;
        this.SetEntityReadOnly(proxy.HibernateLazyInitializer.GetImplementation(), readOnly);
      }
      else
      {
        this.SetEntityReadOnly(entityOrProxy, readOnly);
        object proxy = this.Session.PersistenceContext.ProxyFor(entityOrProxy);
        if (!(proxy is INHibernateProxy))
          return;
        this.SetProxyReadOnly((INHibernateProxy) proxy, readOnly);
      }
    }

    private void SetProxyReadOnly(INHibernateProxy proxy, bool readOnly)
    {
      if (proxy.HibernateLazyInitializer.Session != this.Session)
        throw new AssertionFailure("Attempt to set a proxy to read-only that is associated with a different session");
      proxy.HibernateLazyInitializer.ReadOnly = readOnly;
    }

    private void SetEntityReadOnly(object entity, bool readOnly)
    {
      (this.GetEntry(entity) ?? throw new TransientObjectException("Instance was not associated with this persistence context")).SetReadOnly(readOnly, entity);
      this.hasNonReadOnlyEntities |= !readOnly;
    }

    public bool IsReadOnly(object entityOrProxy)
    {
      if (entityOrProxy == null)
        throw new AssertionFailure("object must be non-null.");
      bool isReadOnly;
      if (entityOrProxy is INHibernateProxy)
        isReadOnly = ((INHibernateProxy) entityOrProxy).HibernateLazyInitializer.ReadOnly;
      else
        isReadOnly = (this.GetEntry(entityOrProxy) ?? throw new TransientObjectException("Instance was not associated with this persistence context")).IsReadOnly;
      return isReadOnly;
    }

    public void ReplaceDelayedEntityIdentityInsertKeys(EntityKey oldKey, object generatedId)
    {
      object obj1 = this.entitiesByKey[oldKey];
      this.entitiesByKey.Remove(oldKey);
      object obj2 = obj1;
      object entityEntry1 = this.entityEntries[obj2];
      this.entityEntries.Remove(obj2);
      EntityEntry entityEntry2 = (EntityEntry) entityEntry1;
      this.parentsByChild.Clear();
      this.AddEntity(new EntityKey(generatedId, entityEntry2.Persister, this.Session.EntityMode), obj2);
      this.AddEntry(obj2, entityEntry2.Status, entityEntry2.LoadedState, entityEntry2.RowId, generatedId, entityEntry2.Version, entityEntry2.LockMode, entityEntry2.ExistsInDatabase, entityEntry2.Persister, entityEntry2.IsBeingReplicated, entityEntry2.LoadedWithLazyPropertiesUnfetched);
    }

    public bool IsLoadFinished => this.loadCounter == 0;

    public void AddChildParent(object child, object parent) => this.parentsByChild[child] = parent;

    public void RemoveChildParent(object child) => this.parentsByChild.Remove(child);

    public override string ToString()
    {
      return new StringBuilder().Append("PersistenceContext[entityKeys=").Append((object) new HashedSet((ICollection) this.entitiesByKey.Keys)).Append(",collectionKeys=").Append((object) new HashedSet((ICollection) this.collectionsByKey.Keys)).Append("]").ToString();
    }

    internal void SetSession(ISessionImplementor session) => this.session = session;

    void IDeserializationCallback.OnDeserialization(object sender)
    {
      StatefulPersistenceContext.log.Debug((object) "Deserialization callback persistent-context");
      this.parentsByChild = IdentityMap.Instantiate(8);
      this.entitiesByKey.OnDeserialization(sender);
      this.entitiesByUniqueKey.OnDeserialization(sender);
      ((IDeserializationCallback) this.entityEntries).OnDeserialization(sender);
      this.proxiesByKey.OnDeserialization(sender);
      this.entitySnapshotsByKey.OnDeserialization(sender);
      ((IDeserializationCallback) this.arrayHolders).OnDeserialization(sender);
      ((IDeserializationCallback) this.collectionEntries).OnDeserialization(sender);
      this.collectionsByKey.OnDeserialization(sender);
      if (this.unownedCollections != null)
        this.unownedCollections.OnDeserialization(sender);
      foreach (DictionaryEntry collectionEntry1 in this.collectionEntries)
      {
        try
        {
          ((IPersistentCollection) collectionEntry1.Key).SetCurrentSession(this.session);
          CollectionEntry collectionEntry2 = (CollectionEntry) collectionEntry1.Value;
          if (collectionEntry2.Role != null)
            collectionEntry2.AfterDeserialize(this.Session.Factory);
        }
        catch (HibernateException ex)
        {
          throw new InvalidOperationException(ex.Message);
        }
      }
      List<EntityKey> entityKeyList = new List<EntityKey>();
      foreach (KeyValuePair<EntityKey, INHibernateProxy> keyValuePair in this.proxiesByKey)
      {
        if (keyValuePair.Value != null)
          keyValuePair.Value.HibernateLazyInitializer.Session = this.session;
        else
          entityKeyList.Add(keyValuePair.Key);
      }
      for (int index = 0; index < entityKeyList.Count; ++index)
        this.proxiesByKey.Remove(entityKeyList[index]);
      foreach (EntityEntry entityEntry in (IEnumerable) this.entityEntries.Values)
      {
        try
        {
          entityEntry.Persister = this.session.Factory.GetEntityPersister(entityEntry.EntityName);
        }
        catch (MappingException ex)
        {
          throw new InvalidOperationException(ex.Message);
        }
      }
    }

    internal StatefulPersistenceContext(SerializationInfo info, StreamingContext context)
    {
      this.loadCounter = 0;
      this.flushing = false;
      this.cascading = 0;
      this.entitiesByKey = (Dictionary<EntityKey, object>) info.GetValue("context.entitiesByKey", typeof (Dictionary<EntityKey, object>));
      this.entitiesByUniqueKey = (Dictionary<EntityUniqueKey, object>) info.GetValue("context.entitiesByUniqueKey", typeof (Dictionary<EntityUniqueKey, object>));
      this.entityEntries = (IDictionary) info.GetValue("context.entityEntries", typeof (IdentityMap));
      this.proxiesByKey = (Dictionary<EntityKey, INHibernateProxy>) info.GetValue("context.proxiesByKey", typeof (Dictionary<EntityKey, INHibernateProxy>));
      this.entitySnapshotsByKey = (Dictionary<EntityKey, object>) info.GetValue("context.entitySnapshotsByKey", typeof (Dictionary<EntityKey, object>));
      this.arrayHolders = (IDictionary) info.GetValue("context.arrayHolders", typeof (IdentityMap));
      this.collectionEntries = (IDictionary) info.GetValue("context.collectionEntries", typeof (IdentityMap));
      this.collectionsByKey = (Dictionary<CollectionKey, IPersistentCollection>) info.GetValue("context.collectionsByKey", typeof (Dictionary<CollectionKey, IPersistentCollection>));
      this.nullifiableEntityKeys = (HashedSet<EntityKey>) info.GetValue("context.nullifiableEntityKeys", typeof (HashedSet<EntityKey>));
      this.unownedCollections = (Dictionary<CollectionKey, IPersistentCollection>) info.GetValue("context.unownedCollections", typeof (Dictionary<CollectionKey, IPersistentCollection>));
      this.hasNonReadOnlyEntities = info.GetBoolean("context.hasNonReadOnlyEntities");
      this.defaultReadOnly = info.GetBoolean("context.defaultReadOnly");
      this.InitTransientState();
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
      StatefulPersistenceContext.log.Debug((object) "serializing persistent-context");
      info.AddValue("context.entitiesByKey", (object) this.entitiesByKey, typeof (Dictionary<EntityKey, object>));
      info.AddValue("context.entitiesByUniqueKey", (object) this.entitiesByUniqueKey, typeof (Dictionary<EntityUniqueKey, object>));
      info.AddValue("context.entityEntries", (object) this.entityEntries, typeof (IdentityMap));
      info.AddValue("context.proxiesByKey", (object) this.proxiesByKey, typeof (Dictionary<EntityKey, INHibernateProxy>));
      info.AddValue("context.entitySnapshotsByKey", (object) this.entitySnapshotsByKey, typeof (Dictionary<EntityKey, object>));
      info.AddValue("context.arrayHolders", (object) this.arrayHolders, typeof (IdentityMap));
      info.AddValue("context.collectionEntries", (object) this.collectionEntries, typeof (IdentityMap));
      info.AddValue("context.collectionsByKey", (object) this.collectionsByKey, typeof (Dictionary<CollectionKey, IPersistentCollection>));
      info.AddValue("context.nullifiableEntityKeys", (object) this.nullifiableEntityKeys, typeof (HashedSet<EntityKey>));
      info.AddValue("context.unownedCollections", (object) this.unownedCollections, typeof (Dictionary<CollectionKey, IPersistentCollection>));
      info.AddValue("context.hasNonReadOnlyEntities", this.hasNonReadOnlyEntities);
      info.AddValue("context.defaultReadOnly", this.defaultReadOnly);
    }
  }
}
