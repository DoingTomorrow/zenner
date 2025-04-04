// Decompiled with JetBrains decompiler
// Type: NHibernate.Collection.Generic.PersistentIdentifierBag`1
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
  public class PersistentIdentifierBag<T> : 
    PersistentIdentifierBag,
    IList<T>,
    ICollection<T>,
    IEnumerable<T>,
    IEnumerable
  {
    private IList<T> gvalues;

    public PersistentIdentifierBag()
    {
    }

    public PersistentIdentifierBag(ISessionImplementor session)
      : base(session)
    {
    }

    public PersistentIdentifierBag(ISessionImplementor session, ICollection<T> coll)
      : base(session, coll as ICollection)
    {
      this.gvalues = coll as IList<T>;
      if (this.gvalues != null)
        return;
      List<T> objList = new List<T>((IEnumerable<T>) coll);
      this.gvalues = (IList<T>) objList;
      this.values = (IList) objList;
    }

    protected IList<T> InternalValues
    {
      get => this.gvalues;
      set
      {
        this.gvalues = value;
        this.values = (IList) this.gvalues;
      }
    }

    public override void BeforeInitialize(ICollectionPersister persister, int anticipatedSize)
    {
      this.identifiers = anticipatedSize <= 0 ? new Dictionary<int, object>() : new Dictionary<int, object>(anticipatedSize + 1);
      this.InternalValues = (IList<T>) persister.CollectionType.Instantiate(anticipatedSize);
    }

    int IList<T>.IndexOf(T item)
    {
      this.Read();
      return this.gvalues.IndexOf(item);
    }

    void IList<T>.Insert(int index, T item)
    {
      this.Write();
      this.BeforeInsert(index);
      this.gvalues.Insert(index, item);
    }

    T IList<T>.this[int index]
    {
      get
      {
        this.Read();
        return this.gvalues[index];
      }
      set
      {
        this.Write();
        this.gvalues[index] = value;
      }
    }

    void ICollection<T>.Add(T item)
    {
      this.Write();
      this.gvalues.Add(item);
    }

    bool ICollection<T>.Contains(T item)
    {
      this.Read();
      return this.gvalues.Contains(item);
    }

    void ICollection<T>.CopyTo(T[] array, int arrayIndex)
    {
      for (int index = arrayIndex; index < this.Count; ++index)
        array.SetValue(this[index], index);
    }

    bool ICollection<T>.Remove(T item)
    {
      this.Initialize(true);
      int index = this.gvalues.IndexOf(item);
      if (index < 0)
        return false;
      this.BeforeRemove(index);
      this.gvalues.RemoveAt(index);
      this.Dirty();
      return true;
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      this.Read();
      return this.gvalues.GetEnumerator();
    }
  }
}
