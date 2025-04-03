// Decompiled with JetBrains decompiler
// Type: Castle.Core.StringObjectDictionaryAdapter
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Castle.Core
{
  public sealed class StringObjectDictionaryAdapter : 
    IDictionary<string, object>,
    ICollection<KeyValuePair<string, object>>,
    IEnumerable<KeyValuePair<string, object>>,
    IEnumerable
  {
    private readonly IDictionary dictionary;

    public StringObjectDictionaryAdapter(IDictionary dictionary) => this.dictionary = dictionary;

    bool IDictionary<string, object>.ContainsKey(string key)
    {
      return this.dictionary.Contains((object) key);
    }

    void IDictionary<string, object>.Add(string key, object value)
    {
      throw new NotImplementedException();
    }

    bool IDictionary<string, object>.Remove(string key) => throw new NotImplementedException();

    bool IDictionary<string, object>.TryGetValue(string key, out object value)
    {
      value = (object) null;
      if (!this.dictionary.Contains((object) key))
        return false;
      value = this.dictionary[(object) key];
      return true;
    }

    object IDictionary<string, object>.this[string key]
    {
      get => this.dictionary[(object) key];
      set => throw new NotImplementedException();
    }

    ICollection<string> IDictionary<string, object>.Keys
    {
      get
      {
        string[] keys = new string[this.Count];
        this.dictionary.Keys.CopyTo((Array) keys, 0);
        return (ICollection<string>) keys;
      }
    }

    ICollection<object> IDictionary<string, object>.Values
    {
      get
      {
        object[] values = new object[this.Count];
        this.dictionary.Values.CopyTo((Array) values, 0);
        return (ICollection<object>) values;
      }
    }

    void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
    {
      throw new NotImplementedException();
    }

    bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
    {
      throw new NotImplementedException();
    }

    void ICollection<KeyValuePair<string, object>>.CopyTo(
      KeyValuePair<string, object>[] array,
      int arrayIndex)
    {
      throw new NotImplementedException();
    }

    bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
    {
      throw new NotImplementedException();
    }

    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<string, object>>) new StringObjectDictionaryAdapter.EnumeratorAdapter(this);
    }

    public bool Contains(object key) => this.dictionary.Contains(key);

    public void Add(object key, object value) => this.dictionary.Add(key, value);

    public void Clear() => this.dictionary.Clear();

    public void Remove(object key) => this.dictionary.Remove(key);

    public object this[object key]
    {
      get => this.dictionary[key];
      set => this.dictionary[key] = value;
    }

    public ICollection Keys => this.dictionary.Keys;

    public ICollection Values => this.dictionary.Values;

    public bool IsReadOnly => this.dictionary.IsReadOnly;

    public bool IsFixedSize => this.dictionary.IsFixedSize;

    public void CopyTo(Array array, int index) => this.dictionary.CopyTo(array, index);

    public int Count => this.dictionary.Count;

    public object SyncRoot => this.dictionary.SyncRoot;

    public bool IsSynchronized => this.dictionary.IsSynchronized;

    public IEnumerator GetEnumerator() => this.dictionary.GetEnumerator();

    internal class EnumeratorAdapter : 
      IEnumerator<KeyValuePair<string, object>>,
      IDisposable,
      IEnumerator
    {
      private readonly StringObjectDictionaryAdapter adapter;
      private IEnumerator<string> keyEnumerator;
      private string currentKey;
      private object currentValue;

      public EnumeratorAdapter(StringObjectDictionaryAdapter adapter)
      {
        this.adapter = adapter;
        this.keyEnumerator = ((IDictionary<string, object>) adapter).Keys.GetEnumerator();
      }

      public bool MoveNext()
      {
        if (!this.keyEnumerator.MoveNext())
          return false;
        this.currentKey = this.keyEnumerator.Current;
        this.currentValue = this.adapter[(object) this.currentKey];
        return true;
      }

      public void Reset() => this.keyEnumerator.Reset();

      public object Current
      {
        get => (object) new KeyValuePair<string, object>(this.currentKey, this.currentValue);
      }

      KeyValuePair<string, object> IEnumerator<KeyValuePair<string, object>>.Current
      {
        get => new KeyValuePair<string, object>(this.currentKey, this.currentValue);
      }

      public void Dispose() => GC.SuppressFinalize((object) this);
    }
  }
}
