// Decompiled with JetBrains decompiler
// Type: NHibernate.Collection.Generic.PersistentGenericSet`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using Iesi.Collections.Generic;
using NHibernate.DebugHelpers;
using NHibernate.Engine;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace NHibernate.Collection.Generic
{
  [DebuggerTypeProxy(typeof (CollectionProxy<>))]
  [Serializable]
  public class PersistentGenericSet<T> : 
    PersistentSet,
    ISet<T>,
    ICollection<T>,
    IEnumerable<T>,
    IEnumerable,
    ICloneable
  {
    protected ISet<T> gset;
    [NonSerialized]
    private IList<T> readList;

    public PersistentGenericSet()
    {
    }

    public PersistentGenericSet(ISessionImplementor session)
      : base(session)
    {
    }

    public PersistentGenericSet(ISessionImplementor session, ISet<T> original)
      : base(session, original as ISet)
    {
      this.gset = original;
      this.set = (ISet) original;
    }

    public override void BeforeInitialize(ICollectionPersister persister, int anticipatedSize)
    {
      this.gset = (ISet<T>) persister.CollectionType.Instantiate(anticipatedSize);
      this.set = (ISet) this.gset;
    }

    public override object ReadFrom(
      IDataReader rs,
      ICollectionPersister role,
      ICollectionAliases descriptor,
      object owner)
    {
      object obj = role.ReadElement(rs, owner, descriptor.SuffixedElementAliases, this.Session);
      if (obj != null)
        this.readList.Add((T) obj);
      return obj;
    }

    public override void BeginRead()
    {
      base.BeginRead();
      this.readList = (IList<T>) new List<T>();
    }

    public override bool EndRead(ICollectionPersister persister)
    {
      this.gset.AddAll((ICollection<T>) this.readList);
      this.readList = (IList<T>) null;
      this.SetInitialized();
      return true;
    }

    ISet<T> ISet<T>.Union(ISet<T> a)
    {
      this.Read();
      return this.gset.Union(a);
    }

    ISet<T> ISet<T>.Intersect(ISet<T> a)
    {
      this.Read();
      return this.gset.Intersect(a);
    }

    ISet<T> ISet<T>.Minus(ISet<T> a)
    {
      this.Read();
      return this.gset.Minus(a);
    }

    ISet<T> ISet<T>.ExclusiveOr(ISet<T> a)
    {
      this.Read();
      return this.gset.ExclusiveOr(a);
    }

    bool ISet<T>.ContainsAll(ICollection<T> c)
    {
      this.Read();
      return this.gset.ContainsAll(c);
    }

    bool ISet<T>.Add(T o) => this.Add((object) o);

    bool ISet<T>.AddAll(ICollection<T> c)
    {
      if (c.Count <= 0)
        return false;
      this.Initialize(true);
      if (!this.gset.AddAll(c))
        return false;
      this.Dirty();
      return true;
    }

    bool ISet<T>.RemoveAll(ICollection<T> c)
    {
      if (c.Count <= 0)
        return false;
      this.Initialize(true);
      if (!this.gset.RemoveAll(c))
        return false;
      this.Dirty();
      return true;
    }

    bool ISet<T>.RetainAll(ICollection<T> c)
    {
      this.Initialize(true);
      if (!this.gset.RetainAll(c))
        return false;
      this.Dirty();
      return true;
    }

    void ICollection<T>.Add(T item) => this.Add((object) item);

    bool ICollection<T>.Contains(T item)
    {
      bool? nullable = this.ReadElementExistence((object) item);
      return nullable.HasValue ? nullable.Value : this.gset.Contains(item);
    }

    void ICollection<T>.CopyTo(T[] array, int arrayIndex)
    {
      this.Read();
      this.gset.CopyTo(array, arrayIndex);
    }

    bool ICollection<T>.Remove(T item)
    {
      bool? nullable = this.PutQueueEnabled ? this.ReadElementExistence((object) item) : new bool?();
      if (!nullable.HasValue)
      {
        this.Initialize(true);
        if (!this.gset.Remove(item))
          return false;
        this.Dirty();
        return true;
      }
      if (!nullable.Value)
        return false;
      this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentSet.SimpleRemoveDelayedOperation((PersistentSet) this, (object) item));
      return true;
    }

    bool ICollection<T>.IsReadOnly => false;

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      this.Read();
      return this.gset.GetEnumerator();
    }
  }
}
