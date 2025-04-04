// Decompiled with JetBrains decompiler
// Type: NHibernate.Collection.SetSnapShot`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Collection
{
  [Serializable]
  internal class SetSnapShot<T> : 
    ISetSnapshot<T>,
    ICollection<T>,
    IEnumerable<T>,
    ICollection,
    IEnumerable
  {
    private readonly List<T> elements;

    public SetSnapShot() => this.elements = new List<T>();

    public SetSnapShot(int capacity) => this.elements = new List<T>(capacity);

    public SetSnapShot(IEnumerable<T> collection) => this.elements = new List<T>(collection);

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this.elements.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public void Add(T item) => this.elements.Add(item);

    public void Clear() => throw new InvalidOperationException();

    public bool Contains(T item) => this.elements.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => this.elements.CopyTo(array, arrayIndex);

    public bool Remove(T item) => throw new InvalidOperationException();

    public void CopyTo(Array array, int index)
    {
      ((ICollection) this.elements).CopyTo(array, index);
    }

    int ICollection.Count => this.elements.Count;

    public object SyncRoot => ((ICollection) this.elements).SyncRoot;

    public bool IsSynchronized => ((ICollection) this.elements).IsSynchronized;

    int ICollection<T>.Count => this.elements.Count;

    public bool IsReadOnly => ((ICollection<T>) this.elements).IsReadOnly;

    public T this[T element]
    {
      get
      {
        int index = this.elements.IndexOf(element);
        return index >= 0 ? this.elements[index] : default (T);
      }
    }
  }
}
