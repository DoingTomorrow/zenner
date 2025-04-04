// Decompiled with JetBrains decompiler
// Type: NHibernate.Collection.PersistentBag
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
  public class PersistentBag : AbstractPersistentCollection, IList, ICollection, IEnumerable
  {
    protected IList bag;

    public PersistentBag()
    {
    }

    public PersistentBag(ISessionImplementor session)
      : base(session)
    {
    }

    public PersistentBag(ISessionImplementor session, ICollection coll)
      : base(session)
    {
      this.bag = coll as IList;
      if (this.bag == null)
        this.bag = (IList) new ArrayList(coll);
      this.SetInitialized();
      this.IsDirectlyAccessible = true;
    }

    public override bool RowUpdatePossible => false;

    public override bool IsWrapper(object collection) => this.bag == collection;

    public override bool Empty => this.bag.Count == 0;

    public override IEnumerable Entries(ICollectionPersister persister) => (IEnumerable) this.bag;

    public override object ReadFrom(
      IDataReader reader,
      ICollectionPersister role,
      ICollectionAliases descriptor,
      object owner)
    {
      object obj = role.ReadElement(reader, owner, descriptor.SuffixedElementAliases, this.Session);
      this.bag.Add(obj);
      return obj;
    }

    public override void BeforeInitialize(ICollectionPersister persister, int anticipatedSize)
    {
      this.bag = (IList) persister.CollectionType.Instantiate(anticipatedSize);
    }

    public override bool EqualsSnapshot(ICollectionPersister persister)
    {
      IType elementType = persister.ElementType;
      EntityMode entityMode = this.Session.EntityMode;
      IList snapshot = (IList) this.GetSnapshot();
      if (snapshot.Count != this.bag.Count)
        return false;
      foreach (object element in (IEnumerable) this.bag)
      {
        if (PersistentBag.CountOccurrences(element, this.bag, elementType, entityMode) != PersistentBag.CountOccurrences(element, snapshot, elementType, entityMode))
          return false;
      }
      return true;
    }

    public override bool IsSnapshotEmpty(object snapshot) => ((ICollection) snapshot).Count == 0;

    private static int CountOccurrences(
      object element,
      IList list,
      IType elementType,
      EntityMode entityMode)
    {
      int num = 0;
      foreach (object y in (IEnumerable) list)
      {
        if (elementType.IsSame(element, y, entityMode))
          ++num;
      }
      return num;
    }

    public override ICollection GetSnapshot(ICollectionPersister persister)
    {
      EntityMode entityMode = this.Session.EntityMode;
      List<object> snapshot = new List<object>(this.bag.Count);
      foreach (object val in (IEnumerable) this.bag)
        snapshot.Add(persister.ElementType.DeepCopy(val, entityMode, persister.Factory));
      return (ICollection) snapshot;
    }

    public override ICollection GetOrphans(object snapshot, string entityName)
    {
      return this.GetOrphans((ICollection) snapshot, (ICollection) this.bag, entityName, this.Session);
    }

    public override object Disassemble(ICollectionPersister persister)
    {
      int count = this.bag.Count;
      object[] objArray = new object[count];
      for (int index = 0; index < count; ++index)
        objArray[index] = persister.ElementType.Disassemble(this.bag[index], this.Session, (object) null);
      return (object) objArray;
    }

    public override void InitializeFromCache(
      ICollectionPersister persister,
      object disassembled,
      object owner)
    {
      object[] objArray = (object[]) disassembled;
      int length = objArray.Length;
      this.BeforeInitialize(persister, length);
      for (int index = 0; index < length; ++index)
      {
        object obj = persister.ElementType.Assemble(objArray[index], this.Session, owner);
        if (obj != null)
          this.bag.Add(obj);
      }
    }

    public override bool NeedsRecreate(ICollectionPersister persister) => !persister.IsOneToMany;

    public override IEnumerable GetDeletes(ICollectionPersister persister, bool indexIsFormula)
    {
      IType elementType = persister.ElementType;
      EntityMode entityMode = this.Session.EntityMode;
      List<object> deletes = new List<object>();
      IList snapshot = (IList) this.GetSnapshot();
      int num = 0;
      foreach (object x in (IEnumerable) snapshot)
      {
        bool flag = false;
        if (this.bag.Count > num && elementType.IsSame(x, this.bag[num++], entityMode))
        {
          flag = true;
        }
        else
        {
          foreach (object y in (IEnumerable) this.bag)
          {
            if (elementType.IsSame(x, y, entityMode))
            {
              flag = true;
              break;
            }
          }
        }
        if (!flag)
          deletes.Add(x);
      }
      return (IEnumerable) deletes;
    }

    public override bool NeedsInserting(object entry, int i, IType elemType)
    {
      IList snapshot = (IList) this.GetSnapshot();
      EntityMode entityMode = this.Session.EntityMode;
      if (snapshot.Count > i && elemType.IsSame(snapshot[i], entry, entityMode))
        return false;
      foreach (object x in (IEnumerable) snapshot)
      {
        if (elemType.IsEqual(x, entry, entityMode))
          return false;
      }
      return true;
    }

    public override bool NeedsUpdating(object entry, int i, IType elemType) => false;

    public override object GetIndex(object entry, int i, ICollectionPersister persister)
    {
      throw new NotSupportedException("Bags don't have indexes");
    }

    public override object GetElement(object entry) => entry;

    public override object GetSnapshotElement(object entry, int i)
    {
      return ((IList) this.GetSnapshot())[i];
    }

    public override bool EntryExists(object entry, int i) => entry != null;

    public override string ToString()
    {
      this.Read();
      return StringHelper.CollectionToString((ICollection) this.bag);
    }

    public bool IsReadOnly => false;

    public object this[int index]
    {
      get
      {
        this.Read();
        return this.bag[index];
      }
      set
      {
        this.Write();
        this.bag[index] = value;
      }
    }

    public void RemoveAt(int index)
    {
      this.Write();
      this.bag.RemoveAt(index);
    }

    public void Insert(int index, object value)
    {
      this.Write();
      this.bag.Insert(index, value);
    }

    public void Remove(object value)
    {
      this.Initialize(true);
      int count = this.bag.Count;
      this.bag.Remove(value);
      if (count == this.bag.Count)
        return;
      this.Dirty();
    }

    public bool Contains(object value)
    {
      bool? nullable = this.ReadElementExistence(value);
      return nullable.HasValue ? nullable.Value : this.bag.Contains(value);
    }

    public void Clear()
    {
      if (this.ClearQueueEnabled)
      {
        this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentBag.ClearDelayedOperation(this));
      }
      else
      {
        this.Initialize(true);
        if (this.bag.Count == 0)
          return;
        this.bag.Clear();
        this.Dirty();
      }
    }

    public int IndexOf(object value)
    {
      this.Read();
      return this.bag.IndexOf(value);
    }

    public int Add(object value)
    {
      if (!this.IsOperationQueueEnabled)
      {
        this.Write();
        return this.bag.Add(value);
      }
      this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentBag.SimpleAddDelayedOperation(this, value));
      return -1;
    }

    public bool IsFixedSize => false;

    public bool IsSynchronized => false;

    public int Count => !this.ReadSize() ? this.bag.Count : this.CachedSize;

    public void CopyTo(Array array, int index)
    {
      for (int index1 = index; index1 < this.Count; ++index1)
        array.SetValue(this[index1], index1);
    }

    public object SyncRoot => (object) this;

    public IEnumerator GetEnumerator()
    {
      this.Read();
      return this.bag.GetEnumerator();
    }

    public override bool AfterInitialize(ICollectionPersister persister)
    {
      bool flag;
      if (persister.IsOneToMany && this.HasQueuedOperations)
      {
        int count = this.bag.Count;
        IList list = (IList) new List<object>(count);
        foreach (object objA in this.QueuedAdditionIterator)
        {
          if (objA != null)
          {
            for (int index = 0; index < this.bag.Count; ++index)
            {
              if (object.ReferenceEquals(objA, this.bag[index]))
              {
                list.Add(objA);
                break;
              }
            }
          }
        }
        flag = base.AfterInitialize(persister);
        if (!flag)
        {
          foreach (object objA in (IEnumerable) list)
          {
            for (int index = count; index < this.bag.Count; ++index)
            {
              if (object.ReferenceEquals(objA, this.bag[index]))
              {
                this.bag.RemoveAt(index);
                break;
              }
            }
          }
        }
      }
      else
        flag = base.AfterInitialize(persister);
      return flag;
    }

    protected sealed class ClearDelayedOperation : AbstractPersistentCollection.IDelayedOperation
    {
      private readonly PersistentBag enclosingInstance;

      public ClearDelayedOperation(PersistentBag enclosingInstance)
      {
        this.enclosingInstance = enclosingInstance;
      }

      public object AddedInstance => (object) null;

      public object Orphan
      {
        get => throw new NotSupportedException("queued clear cannot be used with orphan delete");
      }

      public void Operate() => this.enclosingInstance.bag.Clear();
    }

    protected sealed class SimpleAddDelayedOperation : AbstractPersistentCollection.IDelayedOperation
    {
      private readonly PersistentBag enclosingInstance;
      private readonly object value;

      public SimpleAddDelayedOperation(PersistentBag enclosingInstance, object value)
      {
        this.enclosingInstance = enclosingInstance;
        this.value = value;
      }

      public object AddedInstance => this.value;

      public object Orphan => (object) null;

      public void Operate() => this.enclosingInstance.bag.Add(this.value);
    }
  }
}
