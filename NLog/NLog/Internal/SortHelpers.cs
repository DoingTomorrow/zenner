// Decompiled with JetBrains decompiler
// Type: NLog.Internal.SortHelpers
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NLog.Internal
{
  internal static class SortHelpers
  {
    public static Dictionary<TKey, List<TValue>> BucketSort<TValue, TKey>(
      this IEnumerable<TValue> inputs,
      SortHelpers.KeySelector<TValue, TKey> keySelector)
    {
      Dictionary<TKey, List<TValue>> dictionary = new Dictionary<TKey, List<TValue>>();
      foreach (TValue input in inputs)
      {
        TKey key = keySelector(input);
        List<TValue> objList;
        if (!dictionary.TryGetValue(key, out objList))
        {
          objList = new List<TValue>();
          dictionary.Add(key, objList);
        }
        objList.Add(input);
      }
      return dictionary;
    }

    public static SortHelpers.ReadOnlySingleBucketDictionary<TKey, IList<TValue>> BucketSort<TValue, TKey>(
      this IList<TValue> inputs,
      SortHelpers.KeySelector<TValue, TKey> keySelector)
    {
      return inputs.BucketSort<TValue, TKey>(keySelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public static SortHelpers.ReadOnlySingleBucketDictionary<TKey, IList<TValue>> BucketSort<TValue, TKey>(
      this IList<TValue> inputs,
      SortHelpers.KeySelector<TValue, TKey> keySelector,
      IEqualityComparer<TKey> keyComparer)
    {
      Dictionary<TKey, IList<TValue>> multiBucket = (Dictionary<TKey, IList<TValue>>) null;
      bool flag = false;
      TKey key1 = default (TKey);
      for (int index1 = 0; index1 < inputs.Count; ++index1)
      {
        TKey key2 = keySelector(inputs[index1]);
        if (!flag)
        {
          flag = true;
          key1 = key2;
        }
        else if (multiBucket == null)
        {
          if (!keyComparer.Equals(key1, key2))
          {
            multiBucket = new Dictionary<TKey, IList<TValue>>(keyComparer);
            List<TValue> objList = new List<TValue>(index1);
            for (int index2 = 0; index2 < index1; ++index2)
              objList.Add(inputs[index2]);
            multiBucket[key1] = (IList<TValue>) objList;
            multiBucket[key2] = (IList<TValue>) new List<TValue>()
            {
              inputs[index1]
            };
          }
        }
        else
        {
          IList<TValue> objList;
          if (!multiBucket.TryGetValue(key2, out objList))
          {
            objList = (IList<TValue>) new List<TValue>();
            multiBucket.Add(key2, objList);
          }
          objList.Add(inputs[index1]);
        }
      }
      return multiBucket != null ? new SortHelpers.ReadOnlySingleBucketDictionary<TKey, IList<TValue>>(multiBucket, keyComparer) : new SortHelpers.ReadOnlySingleBucketDictionary<TKey, IList<TValue>>(new KeyValuePair<TKey, IList<TValue>>(key1, inputs), keyComparer);
    }

    internal delegate TKey KeySelector<in TValue, out TKey>(TValue value);

    public struct ReadOnlySingleBucketDictionary<TKey, TValue> : 
      IDictionary<TKey, TValue>,
      ICollection<KeyValuePair<TKey, TValue>>,
      IEnumerable<KeyValuePair<TKey, TValue>>,
      IEnumerable
    {
      private readonly KeyValuePair<TKey, TValue>? _singleBucket;
      private readonly Dictionary<TKey, TValue> _multiBucket;
      private readonly IEqualityComparer<TKey> _comparer;

      public IEqualityComparer<TKey> Comparer => this._comparer;

      public ReadOnlySingleBucketDictionary(KeyValuePair<TKey, TValue> singleBucket)
        : this(singleBucket, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default)
      {
      }

      public ReadOnlySingleBucketDictionary(Dictionary<TKey, TValue> multiBucket)
        : this(multiBucket, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default)
      {
      }

      public ReadOnlySingleBucketDictionary(
        KeyValuePair<TKey, TValue> singleBucket,
        IEqualityComparer<TKey> comparer)
      {
        this._comparer = comparer;
        this._multiBucket = (Dictionary<TKey, TValue>) null;
        this._singleBucket = new KeyValuePair<TKey, TValue>?(singleBucket);
      }

      public ReadOnlySingleBucketDictionary(
        Dictionary<TKey, TValue> multiBucket,
        IEqualityComparer<TKey> comparer)
      {
        this._comparer = comparer;
        this._multiBucket = multiBucket;
        this._singleBucket = new KeyValuePair<TKey, TValue>?(new KeyValuePair<TKey, TValue>());
      }

      public int Count
      {
        get
        {
          if (this._multiBucket != null)
            return this._multiBucket.Count;
          return this._singleBucket.HasValue ? 1 : 0;
        }
      }

      public ICollection<TKey> Keys
      {
        get
        {
          if (this._multiBucket != null)
            return (ICollection<TKey>) this._multiBucket.Keys;
          KeyValuePair<TKey, TValue>? singleBucket = this._singleBucket;
          if (!singleBucket.HasValue)
            return (ICollection<TKey>) ArrayHelper.Empty<TKey>();
          TKey[] keys = new TKey[1];
          singleBucket = this._singleBucket;
          keys[0] = singleBucket.Value.Key;
          return (ICollection<TKey>) keys;
        }
      }

      public ICollection<TValue> Values
      {
        get
        {
          if (this._multiBucket != null)
            return (ICollection<TValue>) this._multiBucket.Values;
          KeyValuePair<TKey, TValue>? singleBucket = this._singleBucket;
          if (!singleBucket.HasValue)
            return (ICollection<TValue>) ArrayHelper.Empty<TValue>();
          TValue[] values = new TValue[1];
          singleBucket = this._singleBucket;
          values[0] = singleBucket.Value.Value;
          return (ICollection<TValue>) values;
        }
      }

      public bool IsReadOnly => true;

      public TValue this[TKey key]
      {
        get
        {
          if (this._multiBucket != null)
            return this._multiBucket[key];
          KeyValuePair<TKey, TValue>? singleBucket = this._singleBucket;
          if (singleBucket.HasValue)
          {
            IEqualityComparer<TKey> comparer = this._comparer;
            singleBucket = this._singleBucket;
            TKey key1 = singleBucket.Value.Key;
            TKey y = key;
            if (comparer.Equals(key1, y))
            {
              singleBucket = this._singleBucket;
              return singleBucket.Value.Value;
            }
          }
          throw new KeyNotFoundException();
        }
        set => throw new NotSupportedException("Readonly");
      }

      public SortHelpers.ReadOnlySingleBucketDictionary<TKey, TValue>.Enumerator GetEnumerator()
      {
        if (this._multiBucket != null)
          return new SortHelpers.ReadOnlySingleBucketDictionary<TKey, TValue>.Enumerator(this._multiBucket);
        KeyValuePair<TKey, TValue>? singleBucket = this._singleBucket;
        if (!singleBucket.HasValue)
          return new SortHelpers.ReadOnlySingleBucketDictionary<TKey, TValue>.Enumerator(new Dictionary<TKey, TValue>());
        singleBucket = this._singleBucket;
        return new SortHelpers.ReadOnlySingleBucketDictionary<TKey, TValue>.Enumerator(singleBucket.Value);
      }

      IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
      {
        return (IEnumerator<KeyValuePair<TKey, TValue>>) this.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

      public bool ContainsKey(TKey key)
      {
        if (this._multiBucket != null)
          return this._multiBucket.ContainsKey(key);
        KeyValuePair<TKey, TValue>? singleBucket = this._singleBucket;
        if (!singleBucket.HasValue)
          return false;
        IEqualityComparer<TKey> comparer = this._comparer;
        singleBucket = this._singleBucket;
        TKey key1 = singleBucket.Value.Key;
        TKey y = key;
        return comparer.Equals(key1, y);
      }

      public void Add(TKey key, TValue value) => throw new NotSupportedException();

      public bool Remove(TKey key) => throw new NotSupportedException();

      public bool TryGetValue(TKey key, out TValue value)
      {
        if (this._multiBucket != null)
          return this._multiBucket.TryGetValue(key, out value);
        KeyValuePair<TKey, TValue>? singleBucket = this._singleBucket;
        if (singleBucket.HasValue)
        {
          IEqualityComparer<TKey> comparer = this._comparer;
          singleBucket = this._singleBucket;
          KeyValuePair<TKey, TValue> keyValuePair = singleBucket.Value;
          TKey key1 = keyValuePair.Key;
          TKey y = key;
          if (comparer.Equals(key1, y))
          {
            ref TValue local = ref value;
            singleBucket = this._singleBucket;
            keyValuePair = singleBucket.Value;
            TValue obj = keyValuePair.Value;
            local = obj;
            return true;
          }
        }
        value = default (TValue);
        return false;
      }

      public void Add(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

      public void Clear() => throw new NotSupportedException();

      public bool Contains(KeyValuePair<TKey, TValue> item)
      {
        if (this._multiBucket != null)
          return ((ICollection<KeyValuePair<TKey, TValue>>) this._multiBucket).Contains(item);
        KeyValuePair<TKey, TValue>? singleBucket = this._singleBucket;
        if (!singleBucket.HasValue)
          return false;
        IEqualityComparer<TKey> comparer = this._comparer;
        singleBucket = this._singleBucket;
        TKey key1 = singleBucket.Value.Key;
        TKey key2 = item.Key;
        if (!comparer.Equals(key1, key2))
          return false;
        EqualityComparer<TValue> equalityComparer = EqualityComparer<TValue>.Default;
        singleBucket = this._singleBucket;
        TValue x = singleBucket.Value.Value;
        TValue y = item.Value;
        return equalityComparer.Equals(x, y);
      }

      public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
      {
        if (this._multiBucket != null)
        {
          ((ICollection<KeyValuePair<TKey, TValue>>) this._multiBucket).CopyTo(array, arrayIndex);
        }
        else
        {
          KeyValuePair<TKey, TValue>? singleBucket = this._singleBucket;
          if (!singleBucket.HasValue)
            return;
          KeyValuePair<TKey, TValue>[] keyValuePairArray = array;
          int index = arrayIndex;
          singleBucket = this._singleBucket;
          KeyValuePair<TKey, TValue> keyValuePair = singleBucket.Value;
          keyValuePairArray[index] = keyValuePair;
        }
      }

      public bool Remove(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

      public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDisposable, IEnumerator
      {
        private bool _singleBucketFirstRead;
        private readonly KeyValuePair<TKey, TValue> _singleBucket;
        private readonly IEnumerator<KeyValuePair<TKey, TValue>> _multiBuckets;

        internal Enumerator(Dictionary<TKey, TValue> multiBucket)
        {
          this._singleBucketFirstRead = false;
          this._singleBucket = new KeyValuePair<TKey, TValue>();
          this._multiBuckets = (IEnumerator<KeyValuePair<TKey, TValue>>) multiBucket.GetEnumerator();
        }

        internal Enumerator(KeyValuePair<TKey, TValue> singleBucket)
        {
          this._singleBucketFirstRead = false;
          this._singleBucket = singleBucket;
          this._multiBuckets = (IEnumerator<KeyValuePair<TKey, TValue>>) null;
        }

        public KeyValuePair<TKey, TValue> Current
        {
          get
          {
            return this._multiBuckets != null ? new KeyValuePair<TKey, TValue>(this._multiBuckets.Current.Key, this._multiBuckets.Current.Value) : new KeyValuePair<TKey, TValue>(this._singleBucket.Key, this._singleBucket.Value);
          }
        }

        object IEnumerator.Current => (object) this.Current;

        public void Dispose()
        {
          if (this._multiBuckets == null)
            return;
          this._multiBuckets.Dispose();
        }

        public bool MoveNext()
        {
          if (this._multiBuckets != null)
            return this._multiBuckets.MoveNext();
          return !this._singleBucketFirstRead && (this._singleBucketFirstRead = true);
        }

        public void Reset()
        {
          if (this._multiBuckets != null)
            this._multiBuckets.Reset();
          else
            this._singleBucketFirstRead = false;
        }
      }
    }
  }
}
