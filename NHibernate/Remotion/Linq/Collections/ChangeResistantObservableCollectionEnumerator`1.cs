// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Collections.ChangeResistantObservableCollectionEnumerator`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Remotion.Linq.Collections
{
  public class ChangeResistantObservableCollectionEnumerator<T> : 
    IEnumerator<T>,
    IDisposable,
    IEnumerator
  {
    private readonly ObservableCollection<T> _collection;
    private int _index;
    private bool _disposed;

    public ChangeResistantObservableCollectionEnumerator(ObservableCollection<T> collection)
    {
      ArgumentUtility.CheckNotNull<ObservableCollection<T>>(nameof (collection), collection);
      this._collection = collection;
      this._index = -1;
      this._disposed = false;
      this._collection.ItemInserted += new EventHandler<ObservableCollectionChangedEventArgs<T>>(this.Collection_ItemInserted);
      this._collection.ItemRemoved += new EventHandler<ObservableCollectionChangedEventArgs<T>>(this.Collection_ItemRemoved);
    }

    public int Index
    {
      get
      {
        if (this._disposed)
          throw new ObjectDisposedException("enumerator");
        return this._index;
      }
    }

    public void Dispose()
    {
      if (this._disposed)
        return;
      this._disposed = true;
      this._collection.ItemInserted -= new EventHandler<ObservableCollectionChangedEventArgs<T>>(this.Collection_ItemInserted);
      this._collection.ItemRemoved -= new EventHandler<ObservableCollectionChangedEventArgs<T>>(this.Collection_ItemRemoved);
    }

    public bool MoveNext()
    {
      if (this._disposed)
        throw new ObjectDisposedException("enumerator");
      ++this._index;
      return this._index < this._collection.Count;
    }

    public void Reset()
    {
      if (this._disposed)
        throw new ObjectDisposedException("enumerator");
      this._index = -1;
    }

    public T Current
    {
      get
      {
        if (this._disposed)
          throw new ObjectDisposedException("enumerator");
        if (this._index < 0)
          throw new InvalidOperationException("MoveNext must be called before invoking Current.");
        if (this._index >= this._collection.Count)
          throw new InvalidOperationException("After MoveNext returned false, Current must not be invoked any more.");
        return this._collection[this._index];
      }
    }

    object IEnumerator.Current => (object) this.Current;

    private void Collection_ItemInserted(object sender, ObservableCollectionChangedEventArgs<T> e)
    {
      if (e.Index > this._index)
        return;
      ++this._index;
    }

    private void Collection_ItemRemoved(object sender, ObservableCollectionChangedEventArgs<T> e)
    {
      if (e.Index > this._index)
        return;
      --this._index;
    }
  }
}
