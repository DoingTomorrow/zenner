// Decompiled with JetBrains decompiler
// Type: Ninject.Infrastructure.Multimap`2
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Ninject.Infrastructure
{
  public class Multimap<K, V> : IEnumerable<KeyValuePair<K, ICollection<V>>>, IEnumerable
  {
    private readonly Dictionary<K, ICollection<V>> _items = new Dictionary<K, ICollection<V>>();

    public ICollection<V> this[K key]
    {
      get
      {
        Ensure.ArgumentNotNull((object) key, nameof (key));
        if (!this._items.ContainsKey(key))
          this._items[key] = (ICollection<V>) new List<V>();
        return this._items[key];
      }
    }

    public ICollection<K> Keys => (ICollection<K>) this._items.Keys;

    public ICollection<ICollection<V>> Values => (ICollection<ICollection<V>>) this._items.Values;

    public void Add(K key, V value)
    {
      Ensure.ArgumentNotNull((object) key, nameof (key));
      Ensure.ArgumentNotNull((object) value, nameof (value));
      this[key].Add(value);
    }

    public bool Remove(K key, V value)
    {
      Ensure.ArgumentNotNull((object) key, nameof (key));
      Ensure.ArgumentNotNull((object) value, nameof (value));
      return this._items.ContainsKey(key) && this._items[key].Remove(value);
    }

    public bool RemoveAll(K key)
    {
      Ensure.ArgumentNotNull((object) key, nameof (key));
      return this._items.Remove(key);
    }

    public void Clear() => this._items.Clear();

    public bool ContainsKey(K key)
    {
      Ensure.ArgumentNotNull((object) key, nameof (key));
      return this._items.ContainsKey(key);
    }

    public bool ContainsValue(K key, V value)
    {
      Ensure.ArgumentNotNull((object) key, nameof (key));
      Ensure.ArgumentNotNull((object) value, nameof (value));
      return this._items.ContainsKey(key) && this._items[key].Contains(value);
    }

    public IEnumerator GetEnumerator() => (IEnumerator) this._items.GetEnumerator();

    IEnumerator<KeyValuePair<K, ICollection<V>>> IEnumerable<KeyValuePair<K, ICollection<V>>>.GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<K, ICollection<V>>>) this._items.GetEnumerator();
    }
  }
}
