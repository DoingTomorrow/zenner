// Decompiled with JetBrains decompiler
// Type: NHibernate.Collection.PersistentMap
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.DebugHelpers;
using NHibernate.Engine;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace NHibernate.Collection
{
  [DebuggerTypeProxy(typeof (DictionaryProxy))]
  [Serializable]
  public class PersistentMap : AbstractPersistentCollection, IDictionary, ICollection, IEnumerable
  {
    protected IDictionary map;

    public PersistentMap()
    {
    }

    public PersistentMap(ISessionImplementor session)
      : base(session)
    {
    }

    public PersistentMap(ISessionImplementor session, IDictionary map)
      : base(session)
    {
      this.map = map;
      this.SetInitialized();
      this.IsDirectlyAccessible = true;
    }

    public override ICollection GetSnapshot(ICollectionPersister persister)
    {
      EntityMode entityMode = this.Session.EntityMode;
      Hashtable snapshot = new Hashtable(this.map.Count);
      foreach (DictionaryEntry dictionaryEntry in this.map)
      {
        object obj = persister.ElementType.DeepCopy(dictionaryEntry.Value, entityMode, persister.Factory);
        snapshot[dictionaryEntry.Key] = obj;
      }
      return (ICollection) snapshot;
    }

    public override ICollection GetOrphans(object snapshot, string entityName)
    {
      return this.GetOrphans(((IDictionary) snapshot).Values, this.map.Values, entityName, this.Session);
    }

    public override bool EqualsSnapshot(ICollectionPersister persister)
    {
      IType elementType = persister.ElementType;
      IDictionary snapshot = (IDictionary) this.GetSnapshot();
      if (snapshot.Count != this.map.Count)
        return false;
      foreach (DictionaryEntry dictionaryEntry in this.map)
      {
        if (elementType.IsDirty(dictionaryEntry.Value, snapshot[dictionaryEntry.Key], this.Session))
          return false;
      }
      return true;
    }

    public override bool IsSnapshotEmpty(object snapshot) => ((ICollection) snapshot).Count == 0;

    public override bool IsWrapper(object collection) => this.map == collection;

    public override void BeforeInitialize(ICollectionPersister persister, int anticipatedSize)
    {
      this.map = (IDictionary) persister.CollectionType.Instantiate(anticipatedSize);
    }

    public override bool Empty => this.map.Count == 0;

    public override string ToString()
    {
      this.Read();
      return StringHelper.CollectionToString((ICollection) this.map);
    }

    public override object ReadFrom(
      IDataReader rs,
      ICollectionPersister role,
      ICollectionAliases descriptor,
      object owner)
    {
      object element = role.ReadElement(rs, owner, descriptor.SuffixedElementAliases, this.Session);
      this.AddDuringInitialize(role.ReadIndex(rs, descriptor.SuffixedIndexAliases, this.Session), element);
      return element;
    }

    protected virtual void AddDuringInitialize(object index, object element)
    {
      this.map[index] = element;
    }

    public override IEnumerable Entries(ICollectionPersister persister) => (IEnumerable) this.map;

    public override void InitializeFromCache(
      ICollectionPersister persister,
      object disassembled,
      object owner)
    {
      object[] objArray = (object[]) disassembled;
      int length = objArray.Length;
      this.BeforeInitialize(persister, length);
      for (int index = 0; index < length; index += 2)
        this.map[persister.IndexType.Assemble(objArray[index], this.Session, owner)] = persister.ElementType.Assemble(objArray[index + 1], this.Session, owner);
    }

    public override object Disassemble(ICollectionPersister persister)
    {
      object[] objArray1 = new object[this.map.Count * 2];
      int num1 = 0;
      foreach (DictionaryEntry dictionaryEntry in this.map)
      {
        object[] objArray2 = objArray1;
        int index1 = num1;
        int num2 = index1 + 1;
        object obj1 = persister.IndexType.Disassemble(dictionaryEntry.Key, this.Session, (object) null);
        objArray2[index1] = obj1;
        object[] objArray3 = objArray1;
        int index2 = num2;
        num1 = index2 + 1;
        object obj2 = persister.ElementType.Disassemble(dictionaryEntry.Value, this.Session, (object) null);
        objArray3[index2] = obj2;
      }
      return (object) objArray1;
    }

    public override IEnumerable GetDeletes(ICollectionPersister persister, bool indexIsFormula)
    {
      IList deletes = (IList) new List<object>();
      foreach (DictionaryEntry dictionaryEntry in (IDictionary) this.GetSnapshot())
      {
        object key = dictionaryEntry.Key;
        if (!this.map.Contains(key))
          deletes.Add(indexIsFormula ? dictionaryEntry.Value : key);
      }
      return (IEnumerable) deletes;
    }

    public override bool NeedsInserting(object entry, int i, IType elemType)
    {
      return !((IDictionary) this.GetSnapshot()).Contains(((DictionaryEntry) entry).Key);
    }

    public override bool NeedsUpdating(object entry, int i, IType elemType)
    {
      IDictionary snapshot = (IDictionary) this.GetSnapshot();
      DictionaryEntry dictionaryEntry = (DictionaryEntry) entry;
      object old = snapshot[dictionaryEntry.Key];
      bool flag = !snapshot.Contains(dictionaryEntry.Key);
      if (dictionaryEntry.Value != null && old != null && elemType.IsDirty(old, dictionaryEntry.Value, this.Session))
        return true;
      return !flag && dictionaryEntry.Value == null != (old == null);
    }

    public override object GetIndex(object entry, int i, ICollectionPersister persister)
    {
      return ((DictionaryEntry) entry).Key;
    }

    public override object GetElement(object entry) => ((DictionaryEntry) entry).Value;

    public override object GetSnapshotElement(object entry, int i)
    {
      return ((IDictionary) this.GetSnapshot())[((DictionaryEntry) entry).Key];
    }

    public override bool Equals(object other)
    {
      if (!(other is IDictionary b))
        return false;
      this.Read();
      return CollectionHelper.DictionaryEquals(this.map, b);
    }

    public override int GetHashCode()
    {
      this.Read();
      return this.map.GetHashCode();
    }

    public override bool EntryExists(object entry, int i)
    {
      return this.map.Contains(((DictionaryEntry) entry).Key);
    }

    public bool Contains(object key)
    {
      bool? nullable = this.ReadIndexExistence(key);
      return nullable.HasValue ? nullable.Value : this.map.Contains(key);
    }

    public void Add(object key, object value)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      if (this.PutQueueEnabled)
      {
        object obj = this.ReadElementByIndex(key);
        if (obj != AbstractPersistentCollection.Unknown)
        {
          this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentMap.PutDelayedOperation(this, key, value, obj == AbstractPersistentCollection.NotFound ? (object) null : obj));
          return;
        }
      }
      this.Initialize(true);
      this.map.Add(key, value);
      this.Dirty();
    }

    public void Clear()
    {
      if (this.ClearQueueEnabled)
      {
        this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentMap.ClearDelayedOperation(this));
      }
      else
      {
        this.Initialize(true);
        if (this.map.Count == 0)
          return;
        this.Dirty();
        this.map.Clear();
      }
    }

    IDictionaryEnumerator IDictionary.GetEnumerator()
    {
      this.Read();
      return this.map.GetEnumerator();
    }

    public void Remove(object key)
    {
      object obj = this.PutQueueEnabled ? this.ReadElementByIndex(key) : AbstractPersistentCollection.Unknown;
      if (obj == AbstractPersistentCollection.Unknown)
      {
        this.Initialize(true);
        int count = this.map.Count;
        this.map.Remove(key);
        if (count == this.map.Count)
          return;
        this.Dirty();
      }
      else
        this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentMap.RemoveDelayedOperation(this, key, obj == AbstractPersistentCollection.NotFound ? (object) null : obj));
    }

    public object this[object key]
    {
      get
      {
        object obj = this.ReadElementByIndex(key);
        if (obj == AbstractPersistentCollection.Unknown)
          return this.map[key];
        return obj != AbstractPersistentCollection.NotFound ? obj : (object) null;
      }
      set
      {
        if (this.PutQueueEnabled)
        {
          object old = this.ReadElementByIndex(key);
          if (old != AbstractPersistentCollection.Unknown && old != AbstractPersistentCollection.NotFound)
          {
            this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentMap.PutDelayedOperation(this, key, value, old));
            return;
          }
        }
        this.Initialize(true);
        object obj = this.map[key];
        this.map[key] = value;
        object objB = obj;
        if (object.ReferenceEquals(value, objB))
          return;
        this.Dirty();
      }
    }

    public ICollection Keys
    {
      get
      {
        this.Read();
        return this.map.Keys;
      }
    }

    public ICollection Values
    {
      get
      {
        this.Read();
        return this.map.Values;
      }
    }

    public bool IsReadOnly => false;

    public bool IsFixedSize => false;

    public void CopyTo(Array array, int index)
    {
      int count = this.Count;
      object[] objArray1 = new object[count];
      object[] objArray2 = new object[count];
      if (this.Keys != null)
        this.Keys.CopyTo((Array) objArray1, index);
      if (this.Values != null)
        this.Values.CopyTo((Array) objArray2, index);
      for (int index1 = index; index1 < count; ++index1)
      {
        if (objArray1[index1] != null || objArray2[index1] != null)
          array.SetValue((object) new DictionaryEntry(objArray1[index1], objArray2[index1]), index1);
      }
    }

    public int Count => !this.ReadSize() ? this.map.Count : this.CachedSize;

    public object SyncRoot => (object) this;

    public bool IsSynchronized => false;

    public IEnumerator GetEnumerator()
    {
      this.Read();
      return (IEnumerator) this.map.GetEnumerator();
    }

    protected sealed class ClearDelayedOperation : AbstractPersistentCollection.IDelayedOperation
    {
      private readonly PersistentMap enclosingInstance;

      public ClearDelayedOperation(PersistentMap enclosingInstance)
      {
        this.enclosingInstance = enclosingInstance;
      }

      public object AddedInstance => (object) null;

      public object Orphan
      {
        get => throw new NotSupportedException("queued clear cannot be used with orphan delete");
      }

      public void Operate() => this.enclosingInstance.map.Clear();
    }

    protected sealed class PutDelayedOperation : AbstractPersistentCollection.IDelayedOperation
    {
      private readonly PersistentMap enclosingInstance;
      private readonly object index;
      private readonly object value;
      private readonly object old;

      public PutDelayedOperation(
        PersistentMap enclosingInstance,
        object index,
        object value,
        object old)
      {
        this.enclosingInstance = enclosingInstance;
        this.index = index;
        this.value = value;
        this.old = old;
      }

      public object AddedInstance => this.value;

      public object Orphan => this.old;

      public void Operate() => this.enclosingInstance.map[this.index] = this.value;
    }

    protected sealed class RemoveDelayedOperation : AbstractPersistentCollection.IDelayedOperation
    {
      private readonly PersistentMap enclosingInstance;
      private readonly object index;
      private readonly object old;

      public RemoveDelayedOperation(PersistentMap enclosingInstance, object index, object old)
      {
        this.enclosingInstance = enclosingInstance;
        this.index = index;
        this.old = old;
      }

      public object AddedInstance => (object) null;

      public object Orphan => this.old;

      public void Operate() => this.enclosingInstance.map.Remove(this.index);
    }
  }
}
