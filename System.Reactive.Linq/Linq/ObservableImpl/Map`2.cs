// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Map`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Map<TKey, TValue>
  {
    private const int DEFAULT_CONCURRENCY_MULTIPLIER = 4;
    private readonly ConcurrentDictionary<TKey, TValue> _map;

    private static int DefaultConcurrencyLevel => 4 * Environment.ProcessorCount;

    public Map(int? capacity, IEqualityComparer<TKey> comparer)
    {
      if (capacity.HasValue)
        this._map = new ConcurrentDictionary<TKey, TValue>(Map<TKey, TValue>.DefaultConcurrencyLevel, capacity.Value, comparer);
      else
        this._map = new ConcurrentDictionary<TKey, TValue>(comparer);
    }

    public TValue GetOrAdd(TKey key, Func<TValue> valueFactory, out bool added)
    {
      added = false;
      TValue orAdd = default (TValue);
      TValue obj = default (TValue);
      bool flag = false;
      while (!this._map.TryGetValue(key, out orAdd))
      {
        if (!flag)
        {
          obj = valueFactory();
          flag = true;
        }
        if (this._map.TryAdd(key, obj))
        {
          added = true;
          orAdd = obj;
          break;
        }
      }
      return orAdd;
    }

    public IEnumerable<TValue> Values => (IEnumerable<TValue>) this._map.Values.ToArray<TValue>();

    public bool Remove(TKey key)
    {
      TValue obj = default (TValue);
      return this._map.TryRemove(key, out obj);
    }
  }
}
