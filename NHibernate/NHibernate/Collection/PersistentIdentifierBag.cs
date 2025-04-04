// Decompiled with JetBrains decompiler
// Type: NHibernate.Collection.PersistentIdentifierBag
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.DebugHelpers;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace NHibernate.Collection
{
  [DebuggerTypeProxy(typeof (CollectionProxy))]
  [Serializable]
  public class PersistentIdentifierBag : 
    AbstractPersistentCollection,
    IList,
    ICollection,
    IEnumerable
  {
    protected IList values;
    protected Dictionary<int, object> identifiers;

    public PersistentIdentifierBag()
    {
    }

    public PersistentIdentifierBag(ISessionImplementor session)
      : base(session)
    {
    }

    public PersistentIdentifierBag(ISessionImplementor session, ICollection coll)
      : base(session)
    {
      this.values = !(coll is IList list) ? (IList) new ArrayList(coll) : list;
      this.SetInitialized();
      this.IsDirectlyAccessible = true;
      this.identifiers = new Dictionary<int, object>();
    }

    public override void InitializeFromCache(
      ICollectionPersister persister,
      object disassembled,
      object owner)
    {
      object[] objArray = (object[]) disassembled;
      int length = objArray.Length;
      this.BeforeInitialize(persister, length);
      for (int index = 0; index < length; index += 2)
      {
        this.identifiers[index / 2] = persister.IdentifierType.Assemble(objArray[index], this.Session, owner);
        this.values.Add(persister.ElementType.Assemble(objArray[index + 1], this.Session, owner));
      }
    }

    private object GetIdentifier(int index)
    {
      object identifier;
      this.identifiers.TryGetValue(index, out identifier);
      return identifier;
    }

    public override object GetIdentifier(object entry, int i) => this.GetIdentifier(i);

    public override bool IsWrapper(object collection) => this.values == collection;

    public override void BeforeInitialize(ICollectionPersister persister, int anticipatedSize)
    {
      this.identifiers = anticipatedSize <= 0 ? new Dictionary<int, object>() : new Dictionary<int, object>(anticipatedSize + 1);
      this.values = anticipatedSize <= 0 ? (IList) new List<object>() : (IList) new List<object>(anticipatedSize);
    }

    public override object Disassemble(ICollectionPersister persister)
    {
      object[] objArray1 = new object[this.values.Count * 2];
      int num1 = 0;
      for (int index1 = 0; index1 < this.values.Count; ++index1)
      {
        object obj1 = this.values[index1];
        object[] objArray2 = objArray1;
        int index2 = num1;
        int num2 = index2 + 1;
        object obj2 = persister.IdentifierType.Disassemble(this.identifiers[index1], this.Session, (object) null);
        objArray2[index2] = obj2;
        object[] objArray3 = objArray1;
        int index3 = num2;
        num1 = index3 + 1;
        object obj3 = persister.ElementType.Disassemble(obj1, this.Session, (object) null);
        objArray3[index3] = obj3;
      }
      return (object) objArray1;
    }

    public override bool Empty => this.values.Count == 0;

    public override IEnumerable Entries(ICollectionPersister persister)
    {
      return (IEnumerable) this.values;
    }

    public override bool EntryExists(object entry, int i) => entry != null;

    public override bool EqualsSnapshot(ICollectionPersister persister)
    {
      IType elementType = persister.ElementType;
      ISet<PersistentIdentifierBag.SnapshotElement> snapshot = (ISet<PersistentIdentifierBag.SnapshotElement>) this.GetSnapshot();
      if (snapshot.Count != this.values.Count)
        return false;
      for (int index = 0; index < this.values.Count; ++index)
      {
        object current = this.values[index];
        object id = this.GetIdentifier(index);
        object old = snapshot.Where<PersistentIdentifierBag.SnapshotElement>((System.Func<PersistentIdentifierBag.SnapshotElement, bool>) (x => object.Equals(x.Id, id))).Select<PersistentIdentifierBag.SnapshotElement, object>((System.Func<PersistentIdentifierBag.SnapshotElement, object>) (x => x.Value)).FirstOrDefault<object>();
        if (elementType.IsDirty(old, current, this.Session))
          return false;
      }
      return true;
    }

    public override bool IsSnapshotEmpty(object snapshot) => ((ICollection) snapshot).Count == 0;

    public override IEnumerable GetDeletes(ICollectionPersister persister, bool indexIsFormula)
    {
      ArrayList deletes = new ArrayList((ICollection) ((IEnumerable<PersistentIdentifierBag.SnapshotElement>) this.GetSnapshot()).Select<PersistentIdentifierBag.SnapshotElement, object>((System.Func<PersistentIdentifierBag.SnapshotElement, object>) (x => x.Id)).ToArray<object>());
      for (int index = 0; index < this.values.Count; ++index)
      {
        if (this.values[index] != null)
          deletes.Remove(this.GetIdentifier(index));
      }
      return (IEnumerable) deletes;
    }

    public override object GetIndex(object entry, int i, ICollectionPersister persister)
    {
      throw new NotSupportedException("Bags don't have indexes");
    }

    public override object GetElement(object entry) => entry;

    public override object GetSnapshotElement(object entry, int i)
    {
      ISet<PersistentIdentifierBag.SnapshotElement> snapshot = (ISet<PersistentIdentifierBag.SnapshotElement>) this.GetSnapshot();
      object id = this.GetIdentifier(i);
      return snapshot.Where<PersistentIdentifierBag.SnapshotElement>((System.Func<PersistentIdentifierBag.SnapshotElement, bool>) (x => object.Equals(x.Id, id))).Select<PersistentIdentifierBag.SnapshotElement, object>((System.Func<PersistentIdentifierBag.SnapshotElement, object>) (x => x.Value)).FirstOrDefault<object>();
    }

    public override bool NeedsInserting(object entry, int i, IType elemType)
    {
      ISet<PersistentIdentifierBag.SnapshotElement> snapshot = (ISet<PersistentIdentifierBag.SnapshotElement>) this.GetSnapshot();
      object id = this.GetIdentifier(i);
      object obj = snapshot.Where<PersistentIdentifierBag.SnapshotElement>((System.Func<PersistentIdentifierBag.SnapshotElement, bool>) (x => object.Equals(x.Id, id))).Select<PersistentIdentifierBag.SnapshotElement, object>((System.Func<PersistentIdentifierBag.SnapshotElement, object>) (x => x.Value)).FirstOrDefault<object>();
      if (entry == null)
        return false;
      return id == null || obj == null;
    }

    public override bool NeedsUpdating(object entry, int i, IType elemType)
    {
      if (entry == null)
        return false;
      ISet<PersistentIdentifierBag.SnapshotElement> snapshot = (ISet<PersistentIdentifierBag.SnapshotElement>) this.GetSnapshot();
      object id = this.GetIdentifier(i);
      if (id == null)
        return false;
      object old = snapshot.Where<PersistentIdentifierBag.SnapshotElement>((System.Func<PersistentIdentifierBag.SnapshotElement, bool>) (x => object.Equals(x.Id, id))).Select<PersistentIdentifierBag.SnapshotElement, object>((System.Func<PersistentIdentifierBag.SnapshotElement, object>) (x => x.Value)).FirstOrDefault<object>();
      return old != null && elemType.IsDirty(old, entry, this.Session);
    }

    public override object ReadFrom(
      IDataReader reader,
      ICollectionPersister persister,
      ICollectionAliases descriptor,
      object owner)
    {
      object obj1 = persister.ReadElement(reader, owner, descriptor.SuffixedElementAliases, this.Session);
      object obj2 = persister.ReadIdentifier(reader, descriptor.SuffixedIdentifierAlias, this.Session);
      if (!this.identifiers.ContainsValue(obj2))
      {
        this.identifiers[this.values.Count] = obj2;
        this.values.Add(obj1);
      }
      return obj1;
    }

    public override ICollection GetSnapshot(ICollectionPersister persister)
    {
      EntityMode entityMode = this.Session.EntityMode;
      HashedSet<PersistentIdentifierBag.SnapshotElement> snapshot = new HashedSet<PersistentIdentifierBag.SnapshotElement>();
      int num = 0;
      foreach (object val in (IEnumerable) this.values)
      {
        object obj1;
        this.identifiers.TryGetValue(num++, out obj1);
        object obj2 = persister.ElementType.DeepCopy(val, entityMode, persister.Factory);
        snapshot.Add(new PersistentIdentifierBag.SnapshotElement()
        {
          Id = obj1,
          Value = obj2
        });
      }
      return (ICollection) snapshot;
    }

    public override ICollection GetOrphans(object snapshot, string entityName)
    {
      return this.GetOrphans((ICollection) ((IEnumerable<PersistentIdentifierBag.SnapshotElement>) this.GetSnapshot()).Select<PersistentIdentifierBag.SnapshotElement, object>((System.Func<PersistentIdentifierBag.SnapshotElement, object>) (x => x.Value)).ToArray<object>(), (ICollection) this.values, entityName, this.Session);
    }

    public override void PreInsert(ICollectionPersister persister)
    {
      if (persister.IdentifierGenerator is IPostInsertIdentifierGenerator)
        return;
      try
      {
        int num = 0;
        foreach (object obj1 in (IEnumerable) this.values)
        {
          int key = num++;
          if (!this.identifiers.ContainsKey(key))
          {
            object obj2 = persister.IdentifierGenerator.Generate(this.Session, obj1);
            this.identifiers[key] = obj2;
          }
        }
      }
      catch (Exception ex)
      {
        throw new ADOException("Could not generate idbag row id.", ex);
      }
    }

    public override void AfterRowInsert(
      ICollectionPersister persister,
      object entry,
      int i,
      object id)
    {
      this.identifiers[i] = id;
    }

    protected void BeforeRemove(int index)
    {
      int key1 = this.values.Count - 1;
      for (int key2 = index; key2 < key1; ++key2)
      {
        object identifier = this.GetIdentifier(key2 + 1);
        if (identifier == null)
          this.identifiers.Remove(key2);
        else
          this.identifiers[key2] = identifier;
      }
      this.identifiers.Remove(key1);
    }

    protected void BeforeInsert(int index)
    {
      for (int index1 = this.values.Count - 1; index1 >= index; --index1)
      {
        object identifier = this.GetIdentifier(index1);
        if (identifier == null)
          this.identifiers.Remove(index1 + 1);
        else
          this.identifiers[index1 + 1] = identifier;
      }
      this.identifiers.Remove(index);
    }

    public int Add(object value)
    {
      this.Write();
      return this.values.Add(value);
    }

    public void Clear()
    {
      this.Initialize(true);
      if (this.values.Count <= 0 && this.identifiers.Count <= 0)
        return;
      this.values.Clear();
      this.identifiers.Clear();
      this.Dirty();
    }

    public bool IsReadOnly => false;

    public object this[int index]
    {
      get
      {
        this.Read();
        return this.values[index];
      }
      set
      {
        this.Write();
        this.identifiers.Remove(index);
        this.values[index] = value;
      }
    }

    public void Insert(int index, object value)
    {
      this.Write();
      this.BeforeInsert(index);
      this.values.Insert(index, value);
    }

    public void RemoveAt(int index)
    {
      this.Write();
      this.BeforeRemove(index);
      this.values.RemoveAt(index);
    }

    public void Remove(object value)
    {
      this.Initialize(true);
      int index = this.values.IndexOf(value);
      if (index < 0)
        return;
      this.BeforeRemove(index);
      this.values.RemoveAt(index);
      this.Dirty();
    }

    public bool Contains(object value)
    {
      this.Read();
      return this.values.Contains(value);
    }

    public int IndexOf(object value)
    {
      this.Read();
      return this.values.IndexOf(value);
    }

    public bool IsFixedSize => false;

    public bool IsSynchronized => false;

    public int Count => !this.ReadSize() ? this.values.Count : this.CachedSize;

    public void CopyTo(Array array, int index)
    {
      for (int index1 = index; index1 < this.Count; ++index1)
        array.SetValue(this[index1], index1);
    }

    public object SyncRoot => (object) this;

    public IEnumerator GetEnumerator()
    {
      this.Read();
      return this.values.GetEnumerator();
    }

    [Serializable]
    private class SnapshotElement : IEquatable<PersistentIdentifierBag.SnapshotElement>
    {
      public object Id { get; set; }

      public object Value { get; set; }

      public bool Equals(PersistentIdentifierBag.SnapshotElement other)
      {
        if (object.ReferenceEquals((object) null, (object) other))
          return false;
        return object.ReferenceEquals((object) this, (object) other) || object.Equals(other.Id, this.Id);
      }

      public override bool Equals(object obj)
      {
        if (object.ReferenceEquals((object) null, obj))
          return false;
        if (object.ReferenceEquals((object) this, obj))
          return true;
        return obj.GetType() == typeof (PersistentIdentifierBag.SnapshotElement) && this.Equals((PersistentIdentifierBag.SnapshotElement) obj);
      }

      public override int GetHashCode() => this.Id == null ? 0 : this.Id.GetHashCode();
    }
  }
}
