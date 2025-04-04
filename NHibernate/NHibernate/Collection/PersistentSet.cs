// Decompiled with JetBrains decompiler
// Type: NHibernate.Collection.PersistentSet
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
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
  public class PersistentSet : 
    AbstractPersistentCollection,
    ISet,
    ICollection,
    IEnumerable,
    ICloneable
  {
    protected ISet set;
    [NonSerialized]
    private IList tempList;

    public PersistentSet()
    {
    }

    public PersistentSet(ISessionImplementor session)
      : base(session)
    {
    }

    public PersistentSet(ISessionImplementor session, ISet original)
      : base(session)
    {
      this.set = original;
      this.SetInitialized();
      this.IsDirectlyAccessible = true;
    }

    public override bool RowUpdatePossible => false;

    public override ICollection GetSnapshot(ICollectionPersister persister)
    {
      EntityMode entityMode = this.Session.EntityMode;
      SetSnapShot<object> snapshot = new SetSnapShot<object>(this.set.Count);
      foreach (object val in (IEnumerable) this.set)
      {
        object obj = persister.ElementType.DeepCopy(val, entityMode, persister.Factory);
        snapshot.Add(obj);
      }
      return (ICollection) snapshot;
    }

    public override ICollection GetOrphans(object snapshot, string entityName)
    {
      return this.GetOrphans((ICollection) new SetSnapShot<object>((IEnumerable<object>) snapshot), (ICollection) this.set, entityName, this.Session);
    }

    public override bool EqualsSnapshot(ICollectionPersister persister)
    {
      IType elementType = persister.ElementType;
      ISetSnapshot<object> snapshot = (ISetSnapshot<object>) this.GetSnapshot();
      if (((ICollection) snapshot).Count != this.set.Count)
        return false;
      foreach (object obj in (IEnumerable) this.set)
      {
        object old = snapshot[obj];
        if (old == null || elementType.IsDirty(old, obj, this.Session))
          return false;
      }
      return true;
    }

    public override bool IsSnapshotEmpty(object snapshot) => ((ICollection) snapshot).Count == 0;

    public override void BeforeInitialize(ICollectionPersister persister, int anticipatedSize)
    {
      this.set = (ISet) persister.CollectionType.Instantiate(anticipatedSize);
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
        object o = persister.ElementType.Assemble(objArray[index], this.Session, owner);
        if (o != null)
          this.set.Add(o);
      }
      this.SetInitialized();
    }

    public override bool Empty => this.set.Count == 0;

    public override string ToString()
    {
      this.Read();
      return StringHelper.CollectionToString((ICollection) this.set);
    }

    public override object ReadFrom(
      IDataReader rs,
      ICollectionPersister role,
      ICollectionAliases descriptor,
      object owner)
    {
      object obj = role.ReadElement(rs, owner, descriptor.SuffixedElementAliases, this.Session);
      if (obj != null)
        this.tempList.Add(obj);
      return obj;
    }

    public override void BeginRead()
    {
      base.BeginRead();
      this.tempList = (IList) new List<object>();
    }

    public override bool EndRead(ICollectionPersister persister)
    {
      this.set.AddAll((ICollection) this.tempList);
      this.tempList = (IList) null;
      this.SetInitialized();
      return true;
    }

    public override IEnumerable Entries(ICollectionPersister persister) => (IEnumerable) this.set;

    public override object Disassemble(ICollectionPersister persister)
    {
      object[] objArray = new object[this.set.Count];
      int num = 0;
      foreach (object obj in (IEnumerable) this.set)
        objArray[num++] = persister.ElementType.Disassemble(obj, this.Session, (object) null);
      return (object) objArray;
    }

    public override IEnumerable GetDeletes(ICollectionPersister persister, bool indexIsFormula)
    {
      IType elementType = persister.ElementType;
      ISetSnapshot<object> snapshot = (ISetSnapshot<object>) this.GetSnapshot();
      List<object> deletes = new List<object>(((ICollection) snapshot).Count);
      foreach (object o in (IEnumerable<object>) snapshot)
      {
        if (!this.set.Contains(o))
          deletes.Add(o);
      }
      foreach (object obj in (IEnumerable) this.set)
      {
        object current = snapshot[obj];
        if (current != null && elementType.IsDirty(obj, current, this.Session))
          deletes.Add(current);
      }
      return (IEnumerable) deletes;
    }

    public override bool NeedsInserting(object entry, int i, IType elemType)
    {
      object old = ((ISetSnapshot<object>) this.GetSnapshot())[entry];
      return old == null || elemType.IsDirty(old, entry, this.Session);
    }

    public override bool NeedsUpdating(object entry, int i, IType elemType) => false;

    public override object GetIndex(object entry, int i, ICollectionPersister persister)
    {
      throw new NotSupportedException("Sets don't have indexes");
    }

    public override object GetElement(object entry) => entry;

    public override object GetSnapshotElement(object entry, int i)
    {
      throw new NotSupportedException("Sets don't support updating by element");
    }

    public override bool Equals(object other)
    {
      if (!(other is ICollection c2))
        return false;
      this.Read();
      return CollectionHelper.CollectionEquals((ICollection) this.set, c2);
    }

    public override int GetHashCode()
    {
      this.Read();
      return this.set.GetHashCode();
    }

    public override bool EntryExists(object entry, int i) => true;

    public override bool IsWrapper(object collection) => this.set == collection;

    public ISet Union(ISet a)
    {
      this.Read();
      return this.set.Union(a);
    }

    public ISet Intersect(ISet a)
    {
      this.Read();
      return this.set.Intersect(a);
    }

    public ISet Minus(ISet a)
    {
      this.Read();
      return this.set.Minus(a);
    }

    public ISet ExclusiveOr(ISet a)
    {
      this.Read();
      return this.set.ExclusiveOr(a);
    }

    public bool Contains(object o)
    {
      bool? nullable = this.ReadElementExistence(o);
      return nullable.HasValue ? nullable.Value : this.set.Contains(o);
    }

    public bool ContainsAll(ICollection c)
    {
      this.Read();
      return this.set.ContainsAll(c);
    }

    public bool Add(object o)
    {
      bool? nullable = this.IsOperationQueueEnabled ? this.ReadElementExistence(o) : new bool?();
      if (!nullable.HasValue)
      {
        this.Initialize(true);
        if (!this.set.Add(o))
          return false;
        this.Dirty();
        return true;
      }
      if (nullable.Value)
        return false;
      this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentSet.SimpleAddDelayedOperation(this, o));
      return true;
    }

    public bool AddAll(ICollection c)
    {
      if (c.Count <= 0)
        return false;
      this.Initialize(true);
      if (!this.set.AddAll(c))
        return false;
      this.Dirty();
      return true;
    }

    public bool Remove(object o)
    {
      bool? nullable = this.PutQueueEnabled ? this.ReadElementExistence(o) : new bool?();
      if (!nullable.HasValue)
      {
        this.Initialize(true);
        if (!this.set.Remove(o))
          return false;
        this.Dirty();
        return true;
      }
      if (!nullable.Value)
        return false;
      this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentSet.SimpleRemoveDelayedOperation(this, o));
      return true;
    }

    public bool RemoveAll(ICollection c)
    {
      if (c.Count <= 0)
        return false;
      this.Initialize(true);
      if (!this.set.RemoveAll(c))
        return false;
      this.Dirty();
      return true;
    }

    public bool RetainAll(ICollection c)
    {
      this.Initialize(true);
      if (!this.set.RetainAll(c))
        return false;
      this.Dirty();
      return true;
    }

    public void Clear()
    {
      if (this.ClearQueueEnabled)
      {
        this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentSet.ClearDelayedOperation(this));
      }
      else
      {
        this.Initialize(true);
        if (this.set.Count == 0)
          return;
        this.set.Clear();
        this.Dirty();
      }
    }

    public bool IsEmpty => !this.ReadSize() ? this.set.Count == 0 : this.CachedSize == 0;

    public void CopyTo(Array array, int index)
    {
      this.Read();
      this.set.CopyTo(array, index);
    }

    public int Count => !this.ReadSize() ? this.set.Count : this.CachedSize;

    public object SyncRoot => (object) this;

    public bool IsSynchronized => false;

    public IEnumerator GetEnumerator()
    {
      this.Read();
      return this.set.GetEnumerator();
    }

    public object Clone()
    {
      this.Read();
      return this.set.Clone();
    }

    protected sealed class ClearDelayedOperation : AbstractPersistentCollection.IDelayedOperation
    {
      private readonly PersistentSet enclosingInstance;

      public ClearDelayedOperation(PersistentSet enclosingInstance)
      {
        this.enclosingInstance = enclosingInstance;
      }

      public object AddedInstance => (object) null;

      public object Orphan
      {
        get => throw new NotSupportedException("queued clear cannot be used with orphan delete");
      }

      public void Operate() => this.enclosingInstance.set.Clear();
    }

    protected sealed class SimpleAddDelayedOperation : AbstractPersistentCollection.IDelayedOperation
    {
      private readonly PersistentSet enclosingInstance;
      private readonly object value;

      public SimpleAddDelayedOperation(PersistentSet enclosingInstance, object value)
      {
        this.enclosingInstance = enclosingInstance;
        this.value = value;
      }

      public object AddedInstance => this.value;

      public object Orphan => (object) null;

      public void Operate() => this.enclosingInstance.set.Add(this.value);
    }

    protected sealed class SimpleRemoveDelayedOperation : 
      AbstractPersistentCollection.IDelayedOperation
    {
      private readonly PersistentSet enclosingInstance;
      private readonly object value;

      public SimpleRemoveDelayedOperation(PersistentSet enclosingInstance, object value)
      {
        this.enclosingInstance = enclosingInstance;
        this.value = value;
      }

      public object AddedInstance => (object) null;

      public object Orphan => this.value;

      public void Operate() => this.enclosingInstance.set.Remove(this.value);
    }
  }
}
