// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Collections.ObservableCollection`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace Remotion.Linq.Collections
{
  public class ObservableCollection<T> : Collection<T>
  {
    public event EventHandler ItemsCleared;

    public event EventHandler<ObservableCollectionChangedEventArgs<T>> ItemRemoved;

    public event EventHandler<ObservableCollectionChangedEventArgs<T>> ItemInserted;

    public event EventHandler<ObservableCollectionChangedEventArgs<T>> ItemSet;

    protected override void ClearItems()
    {
      base.ClearItems();
      if (this.ItemsCleared == null)
        return;
      this.ItemsCleared((object) this, EventArgs.Empty);
    }

    protected override void RemoveItem(int index)
    {
      T obj = this[index];
      base.RemoveItem(index);
      if (this.ItemRemoved == null)
        return;
      this.ItemRemoved((object) this, new ObservableCollectionChangedEventArgs<T>(index, obj));
    }

    protected override void InsertItem(int index, T item)
    {
      base.InsertItem(index, item);
      if (this.ItemInserted == null)
        return;
      this.ItemInserted((object) this, new ObservableCollectionChangedEventArgs<T>(index, item));
    }

    protected override void SetItem(int index, T item)
    {
      base.SetItem(index, item);
      if (this.ItemSet == null)
        return;
      this.ItemSet((object) this, new ObservableCollectionChangedEventArgs<T>(index, item));
    }

    public IEnumerable<T> AsChangeResistantEnumerable()
    {
      return (IEnumerable<T>) new ObservableCollection<T>.ChangeResistantEnumerable(this);
    }

    public IEnumerable<ObservableCollection<T>.IndexValuePair> AsChangeResistantEnumerableWithIndex()
    {
      using (ChangeResistantObservableCollectionEnumerator<T> enumerator = (ChangeResistantObservableCollectionEnumerator<T>) this.AsChangeResistantEnumerable().GetEnumerator())
      {
        while (enumerator.MoveNext())
          yield return new ObservableCollection<T>.IndexValuePair(enumerator);
      }
    }

    private class ChangeResistantEnumerable : IEnumerable<T>, IEnumerable
    {
      private readonly ObservableCollection<T> _collection;

      public ChangeResistantEnumerable(ObservableCollection<T> collection)
      {
        ArgumentUtility.CheckNotNull<ObservableCollection<T>>(nameof (collection), collection);
        this._collection = collection;
      }

      public IEnumerator<T> GetEnumerator()
      {
        return (IEnumerator<T>) new ChangeResistantObservableCollectionEnumerator<T>(this._collection);
      }

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
    }

    public struct IndexValuePair
    {
      private readonly ChangeResistantObservableCollectionEnumerator<T> _enumerator;

      public IndexValuePair(
        ChangeResistantObservableCollectionEnumerator<T> enumerator)
      {
        ArgumentUtility.CheckNotNull<ChangeResistantObservableCollectionEnumerator<T>>(nameof (enumerator), enumerator);
        this._enumerator = enumerator;
      }

      public int Index => this._enumerator.Index;

      public T Value => this._enumerator.Current;
    }
  }
}
