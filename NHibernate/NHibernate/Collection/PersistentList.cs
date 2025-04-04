// Decompiled with JetBrains decompiler
// Type: NHibernate.Collection.PersistentList
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
  [DebuggerTypeProxy(typeof (CollectionProxy))]
  [Serializable]
  public class PersistentList : AbstractPersistentCollection, IList, ICollection, IEnumerable
  {
    protected IList list;

    protected virtual object DefaultForType => (object) null;

    public PersistentList()
    {
    }

    public PersistentList(ISessionImplementor session)
      : base(session)
    {
    }

    public PersistentList(ISessionImplementor session, IList list)
      : base(session)
    {
      this.list = list;
      this.SetInitialized();
      this.IsDirectlyAccessible = true;
    }

    public override ICollection GetSnapshot(ICollectionPersister persister)
    {
      EntityMode entityMode = this.Session.EntityMode;
      List<object> snapshot = new List<object>(this.list.Count);
      foreach (object val in (IEnumerable) this.list)
      {
        object obj = persister.ElementType.DeepCopy(val, entityMode, persister.Factory);
        snapshot.Add(obj);
      }
      return (ICollection) snapshot;
    }

    public override ICollection GetOrphans(object snapshot, string entityName)
    {
      return this.GetOrphans((ICollection) snapshot, (ICollection) this.list, entityName, this.Session);
    }

    public override bool EqualsSnapshot(ICollectionPersister persister)
    {
      IType elementType = persister.ElementType;
      IList snapshot = (IList) this.GetSnapshot();
      if (snapshot.Count != this.list.Count)
        return false;
      for (int index = 0; index < this.list.Count; ++index)
      {
        if (elementType.IsDirty(this.list[index], snapshot[index], this.Session))
          return false;
      }
      return true;
    }

    public override bool IsSnapshotEmpty(object snapshot) => ((ICollection) snapshot).Count == 0;

    public override void BeforeInitialize(ICollectionPersister persister, int anticipatedSize)
    {
      this.list = (IList) persister.CollectionType.Instantiate(anticipatedSize);
    }

    public override bool IsWrapper(object collection) => this.list == collection;

    public override bool Empty => this.list.Count == 0;

    public override string ToString()
    {
      this.Read();
      return StringHelper.CollectionToString((ICollection) this.list);
    }

    public override object ReadFrom(
      IDataReader rs,
      ICollectionPersister role,
      ICollectionAliases descriptor,
      object owner)
    {
      object obj = role.ReadElement(rs, owner, descriptor.SuffixedElementAliases, this.Session);
      int index = (int) role.ReadIndex(rs, descriptor.SuffixedIndexAliases, this.Session);
      for (int count = this.list.Count; count <= index; ++count)
        this.list.Insert(count, this.DefaultForType);
      this.list[index] = obj;
      return obj;
    }

    public override IEnumerable Entries(ICollectionPersister persister) => (IEnumerable) this.list;

    public override void InitializeFromCache(
      ICollectionPersister persister,
      object disassembled,
      object owner)
    {
      object[] objArray = (object[]) disassembled;
      int length = objArray.Length;
      this.BeforeInitialize(persister, length);
      for (int index = 0; index < length; ++index)
        this.list.Add(persister.ElementType.Assemble(objArray[index], this.Session, owner) ?? this.DefaultForType);
    }

    public override object Disassemble(ICollectionPersister persister)
    {
      int count = this.list.Count;
      object[] objArray = new object[count];
      for (int index = 0; index < count; ++index)
        objArray[index] = persister.ElementType.Disassemble(this.list[index], this.Session, (object) null);
      return (object) objArray;
    }

    public override IEnumerable GetDeletes(ICollectionPersister persister, bool indexIsFormula)
    {
      IList deletes = (IList) new List<object>();
      IList snapshot = (IList) this.GetSnapshot();
      int count1;
      if (snapshot.Count > this.list.Count)
      {
        for (int count2 = this.list.Count; count2 < snapshot.Count; ++count2)
          deletes.Add(indexIsFormula ? snapshot[count2] : (object) count2);
        count1 = this.list.Count;
      }
      else
        count1 = snapshot.Count;
      for (int index = 0; index < count1; ++index)
      {
        if (this.list[index] == null && snapshot[index] != null)
          deletes.Add(indexIsFormula ? snapshot[index] : (object) index);
      }
      return (IEnumerable) deletes;
    }

    public override bool NeedsInserting(object entry, int i, IType elemType)
    {
      IList snapshot = (IList) this.GetSnapshot();
      if (this.list[i] == null)
        return false;
      return i >= snapshot.Count || snapshot[i] == null;
    }

    public override bool NeedsUpdating(object entry, int i, IType elemType)
    {
      IList snapshot = (IList) this.GetSnapshot();
      return i < snapshot.Count && snapshot[i] != null && this.list[i] != null && elemType.IsDirty(this.list[i], snapshot[i], this.Session);
    }

    public override object GetIndex(object entry, int i, ICollectionPersister persister)
    {
      return (object) i;
    }

    public override object GetElement(object entry) => entry;

    public override object GetSnapshotElement(object entry, int i)
    {
      return ((IList) this.GetSnapshot())[i];
    }

    public override bool EntryExists(object entry, int i) => entry != null;

    public override bool Equals(object obj)
    {
      if (!(obj is ICollection c2))
        return false;
      this.Read();
      return CollectionHelper.CollectionEquals((ICollection) this.list, c2);
    }

    public override int GetHashCode()
    {
      this.Read();
      return this.list.GetHashCode();
    }

    public int Add(object value)
    {
      if (!this.IsOperationQueueEnabled)
      {
        this.Write();
        return this.list.Add(value);
      }
      this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentList.SimpleAddDelayedOperation(this, value));
      return -1;
    }

    public bool Contains(object value)
    {
      bool? nullable = this.ReadElementExistence(value);
      return nullable.HasValue ? nullable.Value : this.list.Contains(value);
    }

    public void Clear()
    {
      if (this.ClearQueueEnabled)
      {
        this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentList.ClearDelayedOperation(this));
      }
      else
      {
        this.Initialize(true);
        if (this.list.Count == 0)
          return;
        this.list.Clear();
        this.Dirty();
      }
    }

    public int IndexOf(object value)
    {
      this.Read();
      return this.list.IndexOf(value);
    }

    public void Insert(int index, object value)
    {
      if (index < 0)
        throw new IndexOutOfRangeException("negative index");
      if (!this.IsOperationQueueEnabled)
      {
        this.Write();
        this.list.Insert(index, value);
      }
      else
        this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentList.AddDelayedOperation(this, index, value));
    }

    public void Remove(object value)
    {
      bool? nullable = this.PutQueueEnabled ? this.ReadElementExistence(value) : new bool?();
      if (!nullable.HasValue)
      {
        this.Initialize(true);
        int count = this.list.Count;
        this.list.Remove(value);
        if (count == this.list.Count)
          return;
        this.Dirty();
      }
      else
      {
        if (!nullable.Value)
          return;
        this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentList.SimpleRemoveDelayedOperation(this, value));
      }
    }

    public void RemoveAt(int index)
    {
      if (index < 0)
        throw new IndexOutOfRangeException("negative index");
      object obj = this.PutQueueEnabled ? this.ReadElementByIndex((object) index) : AbstractPersistentCollection.Unknown;
      if (obj == AbstractPersistentCollection.Unknown)
      {
        this.Write();
        this.list.RemoveAt(index);
      }
      else
        this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentList.RemoveDelayedOperation(this, index, obj == AbstractPersistentCollection.NotFound ? (object) null : obj));
    }

    public object this[int index]
    {
      get
      {
        object obj = index >= 0 ? this.ReadElementByIndex((object) index) : throw new IndexOutOfRangeException("negative index");
        if (obj == AbstractPersistentCollection.Unknown)
          return this.list[index];
        if (AbstractPersistentCollection.NotFound != obj)
          return obj;
        if (index >= this.Count)
          throw new ArgumentOutOfRangeException(nameof (index));
        return (object) null;
      }
      set
      {
        if (index < 0)
          throw new IndexOutOfRangeException("negative index");
        object old;
        if (this.PutQueueEnabled)
        {
          old = this.ReadElementByIndex((object) index);
          if (old == AbstractPersistentCollection.NotFound)
            old = (object) null;
        }
        else
          old = AbstractPersistentCollection.Unknown;
        if (old == AbstractPersistentCollection.Unknown)
        {
          this.Write();
          this.list[index] = value;
        }
        else
          this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentList.SetDelayedOperation(this, index, value, old));
      }
    }

    public bool IsReadOnly => false;

    public bool IsFixedSize => false;

    public void CopyTo(Array array, int index)
    {
      for (int index1 = index; index1 < this.Count; ++index1)
        array.SetValue(this[index1], index1);
    }

    public int Count => !this.ReadSize() ? this.list.Count : this.CachedSize;

    public object SyncRoot => (object) this;

    public bool IsSynchronized => false;

    public IEnumerator GetEnumerator()
    {
      this.Read();
      return this.list.GetEnumerator();
    }

    protected sealed class ClearDelayedOperation : AbstractPersistentCollection.IDelayedOperation
    {
      private readonly PersistentList enclosingInstance;

      public ClearDelayedOperation(PersistentList enclosingInstance)
      {
        this.enclosingInstance = enclosingInstance;
      }

      public object AddedInstance => (object) null;

      public object Orphan
      {
        get => throw new NotSupportedException("queued clear cannot be used with orphan delete");
      }

      public void Operate() => this.enclosingInstance.list.Clear();
    }

    protected sealed class SimpleAddDelayedOperation : AbstractPersistentCollection.IDelayedOperation
    {
      private readonly PersistentList enclosingInstance;
      private readonly object value;

      public SimpleAddDelayedOperation(PersistentList enclosingInstance, object value)
      {
        this.enclosingInstance = enclosingInstance;
        this.value = value;
      }

      public object AddedInstance => this.value;

      public object Orphan => (object) null;

      public void Operate() => this.enclosingInstance.list.Add(this.value);
    }

    protected sealed class AddDelayedOperation : AbstractPersistentCollection.IDelayedOperation
    {
      private readonly PersistentList enclosingInstance;
      private readonly int index;
      private readonly object value;

      public AddDelayedOperation(PersistentList enclosingInstance, int index, object value)
      {
        this.enclosingInstance = enclosingInstance;
        this.index = index;
        this.value = value;
      }

      public object AddedInstance => this.value;

      public object Orphan => (object) null;

      public void Operate() => this.enclosingInstance.list.Insert(this.index, this.value);
    }

    protected sealed class SetDelayedOperation : AbstractPersistentCollection.IDelayedOperation
    {
      private readonly PersistentList enclosingInstance;
      private readonly int index;
      private readonly object value;
      private readonly object old;

      public SetDelayedOperation(
        PersistentList enclosingInstance,
        int index,
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

      public void Operate() => this.enclosingInstance.list[this.index] = this.value;
    }

    protected sealed class RemoveDelayedOperation : AbstractPersistentCollection.IDelayedOperation
    {
      private readonly PersistentList enclosingInstance;
      private readonly int index;
      private readonly object old;

      public RemoveDelayedOperation(PersistentList enclosingInstance, int index, object old)
      {
        this.enclosingInstance = enclosingInstance;
        this.index = index;
        this.old = old;
      }

      public object AddedInstance => (object) null;

      public object Orphan => this.old;

      public void Operate() => this.enclosingInstance.list.RemoveAt(this.index);
    }

    protected sealed class SimpleRemoveDelayedOperation : 
      AbstractPersistentCollection.IDelayedOperation
    {
      private readonly PersistentList enclosingInstance;
      private readonly object value;

      public SimpleRemoveDelayedOperation(PersistentList enclosingInstance, object value)
      {
        this.enclosingInstance = enclosingInstance;
        this.value = value;
      }

      public object AddedInstance => (object) null;

      public object Orphan => this.value;

      public void Operate() => this.enclosingInstance.list.Remove(this.value);
    }
  }
}
