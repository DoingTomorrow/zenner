// Decompiled with JetBrains decompiler
// Type: NHibernate.Collection.Generic.PersistentGenericBag`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.DebugHelpers;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace NHibernate.Collection.Generic
{
  [DebuggerTypeProxy(typeof (CollectionProxy<>))]
  [Serializable]
  public class PersistentGenericBag<T> : 
    PersistentBag,
    IList<T>,
    ICollection<T>,
    IEnumerable<T>,
    IEnumerable
  {
    private IList<T> gbag;

    public PersistentGenericBag()
    {
    }

    public PersistentGenericBag(ISessionImplementor session)
      : base(session)
    {
    }

    public PersistentGenericBag(ISessionImplementor session, ICollection<T> coll)
      : base(session, coll as ICollection)
    {
      this.gbag = coll as IList<T>;
      if (this.gbag != null)
        return;
      List<T> objList = new List<T>((IEnumerable<T>) coll);
      this.gbag = (IList<T>) objList;
      this.bag = (IList) objList;
    }

    protected IList<T> InternalBag
    {
      get => this.gbag;
      set
      {
        this.gbag = value;
        this.bag = (IList) this.gbag;
      }
    }

    public override void BeforeInitialize(ICollectionPersister persister, int anticipatedSize)
    {
      this.InternalBag = (IList<T>) persister.CollectionType.Instantiate(anticipatedSize);
    }

    int IList<T>.IndexOf(T item)
    {
      this.Read();
      return this.gbag.IndexOf(item);
    }

    void IList<T>.Insert(int index, T item)
    {
      this.Write();
      this.gbag.Insert(index, item);
    }

    T IList<T>.this[int index]
    {
      get
      {
        this.Read();
        return this.gbag[index];
      }
      set
      {
        this.Write();
        this.gbag[index] = value;
      }
    }

    void ICollection<T>.Add(T item)
    {
      if (!this.IsOperationQueueEnabled)
      {
        this.Write();
        this.gbag.Add(item);
      }
      else
        this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentBag.SimpleAddDelayedOperation((PersistentBag) this, (object) item));
    }

    bool ICollection<T>.Contains(T item)
    {
      bool? nullable = this.ReadElementExistence((object) item);
      return nullable.HasValue ? nullable.Value : this.gbag.Contains(item);
    }

    void ICollection<T>.CopyTo(T[] array, int arrayIndex)
    {
      for (int index = arrayIndex; index < this.Count; ++index)
        array.SetValue(this[index], index);
    }

    bool ICollection<T>.Remove(T item)
    {
      this.Initialize(true);
      bool flag = this.gbag.Remove(item);
      if (flag)
        this.Dirty();
      return flag;
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      this.Read();
      return this.gbag.GetEnumerator();
    }
  }
}
