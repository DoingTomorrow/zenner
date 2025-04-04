// Decompiled with JetBrains decompiler
// Type: NHibernate.Collection.PersistentArrayHolder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.DebugHelpers;
using NHibernate.Engine;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Type;
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
  public class PersistentArrayHolder : AbstractPersistentCollection, ICollection, IEnumerable
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (PersistentArrayHolder));
    private System.Array array;
    [NonSerialized]
    private readonly System.Type elementClass;
    [NonSerialized]
    private List<object> tempList;

    public PersistentArrayHolder(ISessionImplementor session, object array)
      : base(session)
    {
      this.array = (System.Array) array;
      this.SetInitialized();
    }

    public PersistentArrayHolder(ISessionImplementor session, ICollectionPersister persister)
      : base(session)
    {
      this.elementClass = persister.ElementClass;
    }

    public object Array
    {
      get => (object) this.array;
      protected set => this.array = (System.Array) value;
    }

    public override object GetValue() => (object) this.array;

    public override ICollection GetSnapshot(ICollectionPersister persister)
    {
      EntityMode entityMode = this.Session.EntityMode;
      int length = this.array.Length;
      System.Array instance = System.Array.CreateInstance(persister.ElementClass, length);
      for (int index = 0; index < length; ++index)
      {
        object val = this.array.GetValue(index);
        try
        {
          instance.SetValue(persister.ElementType.DeepCopy(val, entityMode, persister.Factory), index);
        }
        catch (Exception ex)
        {
          PersistentArrayHolder.log.Error((object) "Array element type error", ex);
          throw new HibernateException("Array element type error", ex);
        }
      }
      return (ICollection) instance;
    }

    public override bool IsSnapshotEmpty(object snapshot) => ((System.Array) snapshot).Length == 0;

    public override ICollection GetOrphans(object snapshot, string entityName)
    {
      object[] collection = (object[]) snapshot;
      object[] array = (object[]) this.array;
      List<object> orphans = new List<object>((IEnumerable<object>) collection);
      for (int index = 0; index < collection.Length; ++index)
        this.IdentityRemove((IList) orphans, array[index], entityName, this.Session);
      return (ICollection) orphans;
    }

    public override bool IsWrapper(object collection) => this.array == collection;

    public override bool EqualsSnapshot(ICollectionPersister persister)
    {
      IType elementType = persister.ElementType;
      System.Array snapshot = (System.Array) this.GetSnapshot();
      int length = snapshot.Length;
      if (length != this.array.Length)
        return false;
      for (int index = 0; index < length; ++index)
      {
        if (elementType.IsDirty(snapshot.GetValue(index), this.array.GetValue(index), this.Session))
          return false;
      }
      return true;
    }

    public ICollection Elements() => (ICollection) this.array.Clone();

    public override bool Empty => false;

    public override object ReadFrom(
      IDataReader rs,
      ICollectionPersister role,
      ICollectionAliases descriptor,
      object owner)
    {
      object obj = role.ReadElement(rs, owner, descriptor.SuffixedElementAliases, this.Session);
      int index = (int) role.ReadIndex(rs, descriptor.SuffixedIndexAliases, this.Session);
      for (int count = this.tempList.Count; count <= index; ++count)
        this.tempList.Add((object) null);
      this.tempList[index] = obj;
      return obj;
    }

    public override IEnumerable Entries(ICollectionPersister persister)
    {
      return (IEnumerable) this.Elements();
    }

    public override void BeginRead()
    {
      base.BeginRead();
      this.tempList = new List<object>();
    }

    public override bool EndRead(ICollectionPersister persister)
    {
      this.SetInitialized();
      this.array = System.Array.CreateInstance(this.elementClass, this.tempList.Count);
      for (int index = 0; index < this.tempList.Count; ++index)
        this.array.SetValue(this.tempList[index], index);
      this.tempList = (List<object>) null;
      return true;
    }

    public override void BeforeInitialize(ICollectionPersister persister, int anticipatedSize)
    {
    }

    public override bool IsDirectlyAccessible => true;

    public override void InitializeFromCache(
      ICollectionPersister persister,
      object disassembled,
      object owner)
    {
      object[] objArray = (object[]) disassembled;
      this.array = System.Array.CreateInstance(persister.ElementClass, objArray.Length);
      for (int index = 0; index < objArray.Length; ++index)
        this.array.SetValue(persister.ElementType.Assemble(objArray[index], this.Session, owner), index);
    }

    public override object Disassemble(ICollectionPersister persister)
    {
      int length = this.array.Length;
      object[] objArray = new object[length];
      for (int index = 0; index < length; ++index)
        objArray[index] = persister.ElementType.Disassemble(this.array.GetValue(index), this.Session, (object) null);
      return (object) objArray;
    }

    public override IEnumerable GetDeletes(ICollectionPersister persister, bool indexIsFormula)
    {
      IList deletes = (IList) new List<object>();
      System.Array snapshot = (System.Array) this.GetSnapshot();
      int length1 = snapshot.Length;
      int length2 = this.array.Length;
      int num;
      if (length1 > length2)
      {
        for (int index = length2; index < length1; ++index)
          deletes.Add((object) index);
        num = length2;
      }
      else
        num = length1;
      for (int index = 0; index < num; ++index)
      {
        if (this.array.GetValue(index) == null && snapshot.GetValue(index) != null)
          deletes.Add((object) index);
      }
      return (IEnumerable) deletes;
    }

    public override bool NeedsInserting(object entry, int i, IType elemType)
    {
      System.Array snapshot = (System.Array) this.GetSnapshot();
      if (this.array.GetValue(i) == null)
        return false;
      return i >= snapshot.Length || snapshot.GetValue(i) == null;
    }

    public override bool NeedsUpdating(object entry, int i, IType elemType)
    {
      System.Array snapshot = (System.Array) this.GetSnapshot();
      return i < snapshot.Length && snapshot.GetValue(i) != null && this.array.GetValue(i) != null && elemType.IsDirty(this.array.GetValue(i), snapshot.GetValue(i), this.Session);
    }

    public override object GetIndex(object entry, int i, ICollectionPersister persister)
    {
      return (object) i;
    }

    public override object GetElement(object entry) => entry;

    public override object GetSnapshotElement(object entry, int i)
    {
      return ((System.Array) this.GetSnapshot()).GetValue(i);
    }

    public override bool EntryExists(object entry, int i) => entry != null;

    void ICollection.CopyTo(System.Array array, int index) => this.array.CopyTo(array, index);

    int ICollection.Count => this.array.Length;

    object ICollection.SyncRoot => (object) this;

    bool ICollection.IsSynchronized => false;

    IEnumerator IEnumerable.GetEnumerator() => this.array.GetEnumerator();
  }
}
