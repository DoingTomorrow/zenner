// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Collections.MultiDictionary`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Remotion.Linq.Collections
{
  public class MultiDictionary<TKey, TValue> : 
    IDictionary<TKey, IList<TValue>>,
    ICollection<KeyValuePair<TKey, IList<TValue>>>,
    IEnumerable<KeyValuePair<TKey, IList<TValue>>>,
    IEnumerable
  {
    private readonly Dictionary<TKey, IList<TValue>> _innerDictionary = new Dictionary<TKey, IList<TValue>>();

    public IEnumerator<KeyValuePair<TKey, IList<TValue>>> GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<TKey, IList<TValue>>>) this._innerDictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public void Add(TKey key, TValue item)
    {
      ArgumentUtility.CheckNotNull<TValue>(nameof (item), item);
      this[key].Add(item);
    }

    void ICollection<KeyValuePair<TKey, IList<TValue>>>.Add(KeyValuePair<TKey, IList<TValue>> item)
    {
      ArgumentUtility.CheckNotNull<KeyValuePair<TKey, IList<TValue>>>(nameof (item), item);
      this.Add(item.Key, item.Value);
    }

    public void Add(TKey key, IList<TValue> value)
    {
      ArgumentUtility.CheckNotNull<TKey>(nameof (key), key);
      ArgumentUtility.CheckNotNull<IList<TValue>>(nameof (value), value);
      this._innerDictionary.Add(key, value);
    }

    public void Clear() => this._innerDictionary.Clear();

    bool ICollection<KeyValuePair<TKey, IList<TValue>>>.Contains(
      KeyValuePair<TKey, IList<TValue>> item)
    {
      ArgumentUtility.CheckNotNull<KeyValuePair<TKey, IList<TValue>>>(nameof (item), item);
      return this._innerDictionary.Contains<KeyValuePair<TKey, IList<TValue>>>(item);
    }

    public bool ContainsKey(TKey key) => this._innerDictionary.ContainsKey(key);

    public bool Remove(TKey key)
    {
      ArgumentUtility.CheckNotNull<TKey>(nameof (key), key);
      return this._innerDictionary.Remove(key);
    }

    bool ICollection<KeyValuePair<TKey, IList<TValue>>>.Remove(
      KeyValuePair<TKey, IList<TValue>> item)
    {
      ArgumentUtility.CheckNotNull<KeyValuePair<TKey, IList<TValue>>>(nameof (item), item);
      return ((ICollection<KeyValuePair<TKey, IList<TValue>>>) this._innerDictionary).Remove(item);
    }

    public int KeyCount => this._innerDictionary.Count;

    public int CountValues()
    {
      return this.Sum<KeyValuePair<TKey, IList<TValue>>>((Func<KeyValuePair<TKey, IList<TValue>>, int>) (kvp => kvp.Value.Count));
    }

    int ICollection<KeyValuePair<TKey, IList<TValue>>>.Count => this.KeyCount;

    public bool IsReadOnly => false;

    public bool TryGetValue(TKey key, out IList<TValue> value)
    {
      ArgumentUtility.CheckNotNull<TKey>(nameof (key), key);
      return this._innerDictionary.TryGetValue(key, out value);
    }

    public IList<TValue> this[TKey key]
    {
      get
      {
        IList<TValue> objList;
        if (!this._innerDictionary.TryGetValue(key, out objList))
        {
          objList = (IList<TValue>) new List<TValue>();
          this._innerDictionary.Add(key, objList);
        }
        return objList;
      }
      set
      {
        ArgumentUtility.CheckNotNull<IList<TValue>>(nameof (value), value);
        this._innerDictionary[key] = value;
      }
    }

    public ICollection<TKey> Keys => (ICollection<TKey>) this._innerDictionary.Keys;

    public ICollection<IList<TValue>> Values
    {
      get => (ICollection<IList<TValue>>) this._innerDictionary.Values;
    }

    void ICollection<KeyValuePair<TKey, IList<TValue>>>.CopyTo(
      KeyValuePair<TKey, IList<TValue>>[] array,
      int arrayIndex)
    {
      ((ICollection<KeyValuePair<TKey, IList<TValue>>>) this._innerDictionary).CopyTo(array, arrayIndex);
    }
  }
}
