// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.ThreadSafeDictionary`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace NHibernate.Util
{
  [Serializable]
  public class ThreadSafeDictionary<TKey, TValue> : 
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable
  {
    private object _syncRoot;
    private readonly IDictionary<TKey, TValue> dictionary;

    public ThreadSafeDictionary(IDictionary<TKey, TValue> dictionary)
    {
      this.dictionary = dictionary;
    }

    public object SyncRoot
    {
      get
      {
        if (this._syncRoot == null)
          Interlocked.CompareExchange(ref this._syncRoot, new object(), (object) null);
        return this._syncRoot;
      }
    }

    public bool ContainsKey(TKey key) => this.dictionary.ContainsKey(key);

    public void Add(TKey key, TValue value)
    {
      lock (this.SyncRoot)
        this.dictionary.Add(key, value);
    }

    public bool Remove(TKey key)
    {
      lock (this.SyncRoot)
        return this.dictionary.Remove(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      return this.dictionary.TryGetValue(key, out value);
    }

    public TValue this[TKey key]
    {
      get => this.dictionary[key];
      set
      {
        lock (this.SyncRoot)
          this.dictionary[key] = value;
      }
    }

    public ICollection<TKey> Keys
    {
      get
      {
        lock (this.SyncRoot)
          return this.dictionary.Keys;
      }
    }

    public ICollection<TValue> Values
    {
      get
      {
        lock (this.SyncRoot)
          return this.dictionary.Values;
      }
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
      lock (this.SyncRoot)
        this.dictionary.Add(item);
    }

    public void Clear()
    {
      lock (this.SyncRoot)
        this.dictionary.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) => this.dictionary.Contains(item);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      lock (this.SyncRoot)
        this.dictionary.CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
      lock (this.SyncRoot)
        return ((ICollection<KeyValuePair<TKey, TValue>>) this.dictionary).Remove(item);
    }

    public int Count => this.dictionary.Count;

    public bool IsReadOnly => false;

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
      lock (this.SyncRoot)
      {
        KeyValuePair<TKey, TValue>[] array = new KeyValuePair<TKey, TValue>[this.dictionary.Count];
        this.dictionary.CopyTo(array, 0);
        return Array.AsReadOnly<KeyValuePair<TKey, TValue>>(array).GetEnumerator();
      }
    }

    public IEnumerator GetEnumerator()
    {
      return (IEnumerator) ((IEnumerable<KeyValuePair<TKey, TValue>>) this).GetEnumerator();
    }
  }
}
