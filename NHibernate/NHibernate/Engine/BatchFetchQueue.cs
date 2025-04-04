// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.BatchFetchQueue
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Collection;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Util;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine
{
  public class BatchFetchQueue
  {
    private static readonly object Marker = new object();
    private readonly IDictionary<EntityKey, object> batchLoadableEntityKeys = (IDictionary<EntityKey, object>) new LinkedHashMap<EntityKey, object>(8);
    private readonly IDictionary<EntityKey, SubselectFetch> subselectsByEntityKey = (IDictionary<EntityKey, SubselectFetch>) new Dictionary<EntityKey, SubselectFetch>(8);
    private readonly IPersistenceContext context;

    public BatchFetchQueue(IPersistenceContext context) => this.context = context;

    public void Clear()
    {
      this.batchLoadableEntityKeys.Clear();
      this.subselectsByEntityKey.Clear();
    }

    public SubselectFetch GetSubselect(EntityKey key)
    {
      SubselectFetch subselect;
      this.subselectsByEntityKey.TryGetValue(key, out subselect);
      return subselect;
    }

    public void AddSubselect(EntityKey key, SubselectFetch subquery)
    {
      this.subselectsByEntityKey[key] = subquery;
    }

    public void RemoveSubselect(EntityKey key) => this.subselectsByEntityKey.Remove(key);

    public void ClearSubselects() => this.subselectsByEntityKey.Clear();

    public void AddBatchLoadableEntityKey(EntityKey key)
    {
      if (!key.IsBatchLoadable)
        return;
      this.batchLoadableEntityKeys[key] = BatchFetchQueue.Marker;
    }

    public void RemoveBatchLoadableEntityKey(EntityKey key)
    {
      if (!key.IsBatchLoadable)
        return;
      this.batchLoadableEntityKeys.Remove(key);
    }

    public object[] GetCollectionBatch(
      ICollectionPersister collectionPersister,
      object id,
      int batchSize,
      EntityMode entityMode)
    {
      object[] collectionBatch = new object[batchSize];
      collectionBatch[0] = id;
      int num1 = 1;
      int num2 = -1;
      bool flag = false;
      foreach (DictionaryEntry collectionEntry1 in this.context.CollectionEntries)
      {
        CollectionEntry collectionEntry2 = (CollectionEntry) collectionEntry1.Value;
        if (!((IPersistentCollection) collectionEntry1.Key).WasInitialized && collectionEntry2.LoadedPersister == collectionPersister)
        {
          if (flag && num1 == num2)
            return collectionBatch;
          if (collectionPersister.KeyType.IsEqual(id, collectionEntry2.LoadedKey, entityMode, collectionPersister.Factory))
            num2 = num1;
          else if (!this.IsCached(collectionEntry2.LoadedKey, collectionPersister, entityMode))
            collectionBatch[num1++] = collectionEntry2.LoadedKey;
          if (num1 == batchSize)
          {
            num1 = 1;
            if (num2 != -1)
              flag = true;
          }
        }
      }
      return collectionBatch;
    }

    public object[] GetEntityBatch(
      IEntityPersister persister,
      object id,
      int batchSize,
      EntityMode entityMode)
    {
      object[] entityBatch = new object[batchSize];
      entityBatch[0] = id;
      int num1 = 1;
      int num2 = -1;
      bool flag = false;
      foreach (EntityKey key in (IEnumerable<EntityKey>) this.batchLoadableEntityKeys.Keys)
      {
        if (key.EntityName.Equals(persister.EntityName))
        {
          if (flag && num1 == num2)
            return entityBatch;
          if (persister.IdentifierType.IsEqual(id, key.Identifier, entityMode))
            num2 = num1;
          else if (!this.IsCached(key, persister, entityMode))
            entityBatch[num1++] = key.Identifier;
          if (num1 == batchSize)
          {
            num1 = 1;
            if (num2 != -1)
              flag = true;
          }
        }
      }
      return entityBatch;
    }

    private bool IsCached(EntityKey entityKey, IEntityPersister persister, EntityMode entityMode)
    {
      if (!persister.HasCache)
        return false;
      CacheKey key = new CacheKey(entityKey.Identifier, persister.IdentifierType, entityKey.EntityName, entityMode, this.context.Session.Factory);
      return persister.Cache.Cache.Get((object) key) != null;
    }

    private bool IsCached(
      object collectionKey,
      ICollectionPersister persister,
      EntityMode entityMode)
    {
      if (!persister.HasCache)
        return false;
      CacheKey key = new CacheKey(collectionKey, persister.KeyType, persister.Role, entityMode, this.context.Session.Factory);
      return persister.Cache.Cache.Get((object) key) != null;
    }
  }
}
