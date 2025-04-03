// Decompiled with JetBrains decompiler
// Type: AutoMapper.Internal.DictionaryFactory
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace AutoMapper.Internal
{
  public class DictionaryFactory : IDictionaryFactory
  {
    public IDictionary<TKey, TValue> CreateDictionary<TKey, TValue>()
    {
      return (IDictionary<TKey, TValue>) new DictionaryFactory.DictionaryAdapter<TKey, TValue>(new Dictionary<TKey, TValue>());
    }

    private class DictionaryAdapter<TKey, TValue> : IDictionary<TKey, TValue>
    {
      private readonly Dictionary<TKey, TValue> _dictionary;

      public DictionaryAdapter(Dictionary<TKey, TValue> dictionary)
      {
        this._dictionary = dictionary;
      }

      public TValue AddOrUpdate(
        TKey key,
        TValue addValue,
        Func<TKey, TValue, TValue> updateValueFactory)
      {
        lock (this._dictionary)
        {
          TValue obj = this._dictionary.ContainsKey(key) ? updateValueFactory(key, addValue) : addValue;
          this._dictionary[key] = obj;
          return obj;
        }
      }

      public bool TryGetValue(TKey key, out TValue value)
      {
        lock (this._dictionary)
          return this._dictionary.TryGetValue(key, out value);
      }

      public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
      {
        lock (this._dictionary)
        {
          if (this._dictionary.ContainsKey(key))
            return this._dictionary[key];
          TValue orAdd = valueFactory(key);
          this._dictionary[key] = orAdd;
          return orAdd;
        }
      }

      public TValue this[TKey key]
      {
        get
        {
          lock (this._dictionary)
            return this._dictionary[key];
        }
        set
        {
          lock (this._dictionary)
            this._dictionary[key] = value;
        }
      }

      public bool TryRemove(TKey key, out TValue value)
      {
        lock (this._dictionary)
        {
          if (!this._dictionary.ContainsKey(key))
          {
            value = default (TValue);
            return false;
          }
          value = this._dictionary[key];
          this._dictionary.Remove(key);
          return true;
        }
      }
    }
  }
}
