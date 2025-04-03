// Decompiled with JetBrains decompiler
// Type: AutoMapper.Internal.DictionaryFactoryOverride
// Assembly: AutoMapper.Net4, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: 30ECE8B3-1802-489A-86AE-267466F9FF1F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.Net4.dll

using System;
using System.Collections.Concurrent;

#nullable disable
namespace AutoMapper.Internal
{
  public class DictionaryFactoryOverride : IDictionaryFactory
  {
    public IDictionary<TKey, TValue> CreateDictionary<TKey, TValue>()
    {
      return (IDictionary<TKey, TValue>) new DictionaryFactoryOverride.ConcurrentDictionaryImpl<TKey, TValue>(new ConcurrentDictionary<TKey, TValue>());
    }

    private class ConcurrentDictionaryImpl<TKey, TValue> : IDictionary<TKey, TValue>
    {
      private readonly ConcurrentDictionary<TKey, TValue> _dictionary;

      public ConcurrentDictionaryImpl(ConcurrentDictionary<TKey, TValue> dictionary)
      {
        this._dictionary = dictionary;
      }

      public TValue AddOrUpdate(
        TKey key,
        TValue addValue,
        Func<TKey, TValue, TValue> updateValueFactory)
      {
        return this._dictionary.AddOrUpdate(key, addValue, updateValueFactory);
      }

      public bool TryGetValue(TKey key, out TValue value)
      {
        return this._dictionary.TryGetValue(key, out value);
      }

      public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
      {
        return this._dictionary.GetOrAdd(key, valueFactory);
      }

      public TValue this[TKey key]
      {
        get => this._dictionary[key];
        set => this._dictionary[key] = value;
      }

      public bool TryRemove(TKey key, out TValue value)
      {
        return this._dictionary.TryRemove(key, out value);
      }
    }
  }
}
