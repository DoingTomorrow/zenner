// Decompiled with JetBrains decompiler
// Type: NHibernate.Collection.Generic.PersistentGenericList`1
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
  public class PersistentGenericList<T> : 
    PersistentList,
    IList<T>,
    ICollection<T>,
    IEnumerable<T>,
    IEnumerable
  {
    protected IList<T> glist;

    protected override object DefaultForType => (object) default (T);

    public PersistentGenericList()
    {
    }

    public PersistentGenericList(ISessionImplementor session)
      : base(session)
    {
    }

    public PersistentGenericList(ISessionImplementor session, IList<T> list)
      : base(session, list as IList)
    {
      this.glist = list;
    }

    public override void BeforeInitialize(ICollectionPersister persister, int anticipatedSize)
    {
      this.glist = (IList<T>) persister.CollectionType.Instantiate(anticipatedSize);
      this.list = (IList) this.glist;
    }

    int IList<T>.IndexOf(T item)
    {
      this.Read();
      return this.glist.IndexOf(item);
    }

    void IList<T>.Insert(int index, T item)
    {
      if (index < 0)
        throw new IndexOutOfRangeException("negative index");
      if (!this.IsOperationQueueEnabled)
      {
        this.Write();
        this.glist.Insert(index, item);
      }
      else
        this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentList.AddDelayedOperation((PersistentList) this, index, (object) item));
    }

    T IList<T>.this[int index]
    {
      get
      {
        object obj = index >= 0 ? this.ReadElementByIndex((object) index) : throw new IndexOutOfRangeException("negative index");
        if (obj == AbstractPersistentCollection.Unknown)
          return this.glist[index];
        if (obj != AbstractPersistentCollection.NotFound)
          return (T) obj;
        if (index >= this.Count)
          throw new ArgumentOutOfRangeException(nameof (index));
        return default (T);
      }
      set
      {
        if (index < 0)
          throw new IndexOutOfRangeException("negative index");
        object obj = this.PutQueueEnabled ? this.ReadElementByIndex((object) index) : AbstractPersistentCollection.Unknown;
        if (obj == AbstractPersistentCollection.Unknown)
        {
          this.Write();
          this.glist[index] = value;
        }
        else
          this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentList.SetDelayedOperation((PersistentList) this, index, (object) value, obj == AbstractPersistentCollection.NotFound ? (object) null : obj));
      }
    }

    void ICollection<T>.Add(T item)
    {
      if (!this.IsOperationQueueEnabled)
      {
        this.Write();
        this.glist.Add(item);
      }
      else
        this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentList.SimpleAddDelayedOperation((PersistentList) this, (object) item));
    }

    bool ICollection<T>.Contains(T item)
    {
      bool? nullable = this.ReadElementExistence((object) item);
      return nullable.HasValue ? nullable.Value : this.glist.Contains(item);
    }

    void ICollection<T>.CopyTo(T[] array, int arrayIndex)
    {
      for (int index = arrayIndex; index < this.Count; ++index)
        array.SetValue(this[index], index);
    }

    bool ICollection<T>.Remove(T item)
    {
      bool? nullable = this.PutQueueEnabled ? this.ReadElementExistence((object) item) : new bool?();
      if (!nullable.HasValue)
      {
        this.Initialize(true);
        if (this.glist.Remove(item))
        {
          this.Dirty();
          return true;
        }
      }
      else if (nullable.Value)
      {
        this.QueueOperation((AbstractPersistentCollection.IDelayedOperation) new PersistentList.SimpleRemoveDelayedOperation((PersistentList) this, (object) item));
        return true;
      }
      return false;
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      this.Read();
      return this.glist.GetEnumerator();
    }
  }
}
