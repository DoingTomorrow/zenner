// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.UnmodifiableDictionary`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Util
{
  [Serializable]
  public class UnmodifiableDictionary<TKey, TValue> : 
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable
  {
    private readonly IDictionary<TKey, TValue> dictionary;

    public UnmodifiableDictionary(IDictionary<TKey, TValue> dictionary)
    {
      this.dictionary = dictionary;
    }

    public bool ContainsKey(TKey key) => this.dictionary.ContainsKey(key);

    public void Add(TKey key, TValue value) => throw new NotSupportedException();

    public bool Remove(TKey key) => throw new NotSupportedException();

    public bool TryGetValue(TKey key, out TValue value)
    {
      return this.dictionary.TryGetValue(key, out value);
    }

    public TValue this[TKey key]
    {
      get => this.dictionary[key];
      set => throw new NotSupportedException();
    }

    public ICollection<TKey> Keys => this.dictionary.Keys;

    public ICollection<TValue> Values => this.dictionary.Values;

    public void Add(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

    public void Clear() => throw new NotSupportedException();

    public bool Contains(KeyValuePair<TKey, TValue> item) => this.dictionary.Contains(item);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      this.dictionary.CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

    public int Count => this.dictionary.Count;

    public bool IsReadOnly => true;

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
      return this.dictionary.GetEnumerator();
    }

    public IEnumerator GetEnumerator()
    {
      return (IEnumerator) ((IEnumerable<KeyValuePair<TKey, TValue>>) this).GetEnumerator();
    }
  }
}
