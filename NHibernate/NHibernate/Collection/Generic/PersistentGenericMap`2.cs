// Decompiled with JetBrains decompiler
// Type: NHibernate.Collection.Generic.PersistentGenericMap`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.DebugHelpers;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace NHibernate.Collection.Generic
{
  [DebuggerTypeProxy(typeof (DictionaryProxy<,>))]
  [Serializable]
  public class PersistentGenericMap<TKey, TValue> : 
    PersistentMap,
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable
  {
    protected IDictionary<TKey, TValue> gmap;

    public PersistentGenericMap()
    {
    }

    public PersistentGenericMap(ISessionImplementor session)
      : base(session)
    {
    }

    public PersistentGenericMap(ISessionImplementor session, IDictionary<TKey, TValue> map)
      : base(session, map as IDictionary)
    {
      this.gmap = map;
    }

    public override ICollection GetSnapshot(ICollectionPersister persister)
    {
      EntityMode entityMode = this.Session.EntityMode;
      Dictionary<TKey, TValue> snapshot = new Dictionary<TKey, TValue>(this.map.Count);
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) this.gmap)
      {
        object obj = persister.ElementType.DeepCopy((object) keyValuePair.Value, entityMode, persister.Factory);
        snapshot[keyValuePair.Key] = (TValue) obj;
      }
      return (ICollection) snapshot;
    }

    public override void BeforeInitialize(ICollectionPersister persister, int anticipatedSize)
    {
      this.gmap = (IDictionary<TKey, TValue>) persister.CollectionType.Instantiate(anticipatedSize);
      this.map = (IDictionary) this.gmap;
    }

    public override IEnumerable GetDeletes(ICollectionPersister persister, bool indexIsFormula)
    {
      IList deletes = (IList) new List<object>();
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) this.GetSnapshot())
      {
        if (!this.gmap.ContainsKey(keyValuePair.Key))
        {
          object key = (object) keyValuePair.Key;
          deletes.Add(indexIsFormula ? (object) keyValuePair.Value : key);
        }
      }
      return (IEnumerable) deletes;
    }

    public override bool NeedsInserting(object entry, int i, IType elemType)
    {
      return !((IDictionary) this.GetSnapshot()).Contains((object) ((KeyValuePair<TKey, TValue>) entry).Key);
    }

    public override bool NeedsUpdating(object entry, int i, IType elemType)
    {
      IDictionary snapshot = (IDictionary) this.GetSnapshot();
      KeyValuePair<TKey, TValue> keyValuePair = (KeyValuePair<TKey, TValue>) entry;
      object old = snapshot[(object) keyValuePair.Key];
      bool flag = !snapshot.Contains((object) keyValuePair.Key);
      if ((object) keyValuePair.Value != null && old != null && elemType.IsDirty(old, (object) keyValuePair.Value, this.Session))
        return true;
      return !flag && (object) keyValuePair.Value == null != (old == null);
    }

    public override object GetIndex(object entry, int i, ICollectionPersister persister)
    {
      return (object) ((KeyValuePair<TKey, TValue>) entry).Key;
    }

    public override object GetElement(object entry)
    {
      return (object) ((KeyValuePair<TKey, TValue>) entry).Value;
    }

    public override object GetSnapshotElement(object entry, int i)
    {
      return ((IDictionary) this.GetSnapshot())[(object) ((KeyValuePair<TKey, TValue>) entry).Key];
    }

    public override bool EntryExists(object entry, int i)
    {
      return this.gmap.ContainsKey(((KeyValuePair<TKey, TValue>) entry).Key);
    }

    protected override void AddDuringInitialize(object index, object element)
    {
      this.gmap[(TKey) index] = (TValue) element;
    }

    bool IDictionary<TKey, TValue>.ContainsKey(TKey key)
    {
      bool? nullable = this.ReadIndexExistence((object) key);
      return nullable.HasValue ? nullable.Value : this.gmap.ContainsKey(key);
    }

    void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
    {
      if ((object) key == null)
        throw new ArgumentNullException(nameof (key));
      if (this.PutQueueEnabled)
      {
        object obj = this.ReadElementByIndex((object) key);
        if (obj != AbstractPersistentCollection.Unknown)
        {
          this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentMap.PutDelayedOperation((PersistentMap) this, (object) key, (object) value, obj == AbstractPersistentCollection.NotFound ? (object) null : obj));
          return;
        }
      }
      this.Initialize(true);
      this.gmap.Add(key, value);
      this.Dirty();
    }

    bool IDictionary<TKey, TValue>.Remove(TKey key)
    {
      object obj = this.PutQueueEnabled ? this.ReadElementByIndex((object) key) : AbstractPersistentCollection.Unknown;
      if (obj == AbstractPersistentCollection.Unknown)
      {
        this.Initialize(true);
        bool flag = this.gmap.Remove(key);
        if (flag)
          this.Dirty();
        return flag;
      }
      this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentMap.RemoveDelayedOperation((PersistentMap) this, (object) key, obj == AbstractPersistentCollection.NotFound ? (object) null : obj));
      return true;
    }

    bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
    {
      object obj = this.ReadElementByIndex((object) key);
      if (obj == AbstractPersistentCollection.Unknown)
        return this.gmap.TryGetValue(key, out value);
      if (obj == AbstractPersistentCollection.NotFound)
      {
        value = default (TValue);
        return false;
      }
      value = (TValue) obj;
      return true;
    }

    TValue IDictionary<TKey, TValue>.this[TKey key]
    {
      get
      {
        object obj = this.ReadElementByIndex((object) key);
        if (obj == AbstractPersistentCollection.Unknown)
          return this.gmap[key];
        return obj != AbstractPersistentCollection.NotFound ? (TValue) obj : throw new KeyNotFoundException();
      }
      set
      {
        if (this.PutQueueEnabled)
        {
          object obj = this.ReadElementByIndex((object) key);
          if (obj != AbstractPersistentCollection.Unknown)
          {
            this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentMap.PutDelayedOperation((PersistentMap) this, (object) key, (object) value, obj == AbstractPersistentCollection.NotFound ? (object) null : obj));
            return;
          }
        }
        this.Initialize(true);
        TValue obj1;
        this.gmap.TryGetValue(key, out obj1);
        this.gmap[key] = value;
        TValue objB = obj1;
        if (object.ReferenceEquals((object) value, (object) objB))
          return;
        this.Dirty();
      }
    }

    ICollection<TKey> IDictionary<TKey, TValue>.Keys
    {
      get
      {
        this.Read();
        return this.gmap.Keys;
      }
    }

    ICollection<TValue> IDictionary<TKey, TValue>.Values
    {
      get
      {
        this.Read();
        return this.gmap.Values;
      }
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
    {
      this.Add((object) item.Key, (object) item.Value);
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
    {
      bool? nullable = this.ReadIndexExistence((object) item.Key);
      if (!nullable.HasValue)
        return this.gmap.Contains(item);
      return nullable.Value && EqualityComparer<TValue>.Default.Equals(((IDictionary<TKey, TValue>) this)[item.Key], item.Value);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(
      KeyValuePair<TKey, TValue>[] array,
      int arrayIndex)
    {
      int count = this.Count;
      TKey[] keyArray = new TKey[count];
      TValue[] objArray = new TValue[count];
      if (this.Keys != null)
        this.Keys.CopyTo((Array) keyArray, arrayIndex);
      if (this.Values != null)
        this.Values.CopyTo((Array) objArray, arrayIndex);
      for (int index = arrayIndex; index < count; ++index)
      {
        if ((object) keyArray[index] != null || (object) objArray[index] != null)
          array.SetValue((object) new KeyValuePair<TKey, TValue>(keyArray[index], objArray[index]), index);
      }
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
      if (!((ICollection<KeyValuePair<TKey, TValue>>) this).Contains(item))
        return false;
      this.Remove((object) item.Key);
      return true;
    }

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
      this.Read();
      return this.gmap.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      this.Read();
      return (IEnumerator) this.gmap.GetEnumerator();
    }
  }
}
