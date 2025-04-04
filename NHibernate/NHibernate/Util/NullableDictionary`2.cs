// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.NullableDictionary`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Util
{
  public class NullableDictionary<TKey, TValue> : 
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable
    where TKey : class
  {
    private TValue _nullValue;
    private bool _gotNullValue;
    private readonly Dictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>();

    public bool ContainsKey(TKey key)
    {
      return (object) key == null ? this._gotNullValue : this._dict.ContainsKey(key);
    }

    public void Add(TKey key, TValue value)
    {
      if ((object) key == null)
        this._nullValue = value;
      else
        this._dict[key] = value;
    }

    public bool Remove(TKey key)
    {
      if ((object) key != null)
        return this._dict.Remove(key);
      if (!this._gotNullValue)
        return false;
      this._nullValue = default (TValue);
      this._gotNullValue = false;
      return true;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      if ((object) key != null)
        return this._dict.TryGetValue(key, out value);
      if (this._gotNullValue)
      {
        value = this._nullValue;
        return true;
      }
      value = default (TValue);
      return false;
    }

    public TValue this[TKey key]
    {
      get
      {
        if ((object) key == null)
          return this._nullValue;
        TValue obj;
        this._dict.TryGetValue(key, out obj);
        return obj;
      }
      set
      {
        if ((object) key == null)
        {
          this._nullValue = value;
          this._gotNullValue = true;
        }
        else
          this._dict[key] = value;
      }
    }

    public ICollection<TKey> Keys
    {
      get
      {
        if (!this._gotNullValue)
          return (ICollection<TKey>) this._dict.Keys;
        return (ICollection<TKey>) new List<TKey>((IEnumerable<TKey>) this._dict.Keys)
        {
          default (TKey)
        };
      }
    }

    public ICollection<TValue> Values
    {
      get
      {
        if (!this._gotNullValue)
          return (ICollection<TValue>) this._dict.Values;
        return (ICollection<TValue>) new List<TValue>((IEnumerable<TValue>) this._dict.Values)
        {
          this._nullValue
        };
      }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      foreach (KeyValuePair<TKey, TValue> kvp in this._dict)
        yield return kvp;
      if (this._gotNullValue)
        yield return new KeyValuePair<TKey, TValue>(default (TKey), this._nullValue);
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public void Add(KeyValuePair<TKey, TValue> item)
    {
      if ((object) item.Key == null)
      {
        this._nullValue = item.Value;
        this._gotNullValue = true;
      }
      else
        this._dict.Add(item.Key, item.Value);
    }

    public void Clear()
    {
      this._dict.Clear();
      this._nullValue = default (TValue);
      this._gotNullValue = false;
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
      TValue objB = default (TValue);
      return this.TryGetValue(item.Key, out objB) && object.Equals((object) item.Value, (object) objB);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<TKey, TValue> item) => throw new NotImplementedException();

    public int Count => this._gotNullValue ? this._dict.Count + 1 : this._dict.Count;

    public bool IsReadOnly => false;
  }
}
