// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.PageDataDictionary`1
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace System.Web.WebPages
{
  internal class PageDataDictionary<TValue> : 
    IDictionary<object, TValue>,
    ICollection<KeyValuePair<object, TValue>>,
    IEnumerable<KeyValuePair<object, TValue>>,
    IEnumerable
  {
    private IDictionary<object, TValue> _data = (IDictionary<object, TValue>) new Dictionary<object, TValue>((IEqualityComparer<object>) new PageDataDictionary<TValue>.PageDataComparer());
    private IDictionary<string, TValue> _stringDictionary = (IDictionary<string, TValue>) new Dictionary<string, TValue>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private IList<TValue> _indexedValues = (IList<TValue>) new List<TValue>();

    internal IDictionary<object, TValue> Data => this._data;

    internal IDictionary<string, TValue> StringDictionary => this._stringDictionary;

    internal IList<TValue> IndexedValues => this._indexedValues;

    public ICollection<object> Keys
    {
      get
      {
        List<object> keys = new List<object>();
        keys.AddRange((IEnumerable<object>) this._stringDictionary.Keys);
        for (int index = 0; index < this._indexedValues.Count; ++index)
          keys.Add((object) index);
        foreach (object key in (IEnumerable<object>) this._data.Keys)
        {
          if (!this.ContainsIndex(key) && !this.ContainsStringKey(key))
            keys.Add(key);
        }
        return (ICollection<object>) keys;
      }
    }

    public ICollection<TValue> Values
    {
      get
      {
        List<TValue> values = new List<TValue>();
        foreach (object key in (IEnumerable<object>) this.Keys)
          values.Add(this[key]);
        return (ICollection<TValue>) values;
      }
    }

    internal ICollection<KeyValuePair<object, TValue>> Items
    {
      get
      {
        List<KeyValuePair<object, TValue>> items = new List<KeyValuePair<object, TValue>>();
        foreach (object key in (IEnumerable<object>) this.Keys)
        {
          TValue obj = this[key];
          KeyValuePair<object, TValue> keyValuePair = new KeyValuePair<object, TValue>(key, obj);
          items.Add(keyValuePair);
        }
        return (ICollection<KeyValuePair<object, TValue>>) items;
      }
    }

    public int Count => this.Items.Count;

    public bool IsReadOnly => false;

    public TValue this[object key]
    {
      get
      {
        TValue obj = default (TValue);
        this.TryGetValue(key, out obj);
        return obj;
      }
      set
      {
        if (this.ContainsStringKey(key))
          this._stringDictionary[(string) key] = value;
        else if (this.ContainsIndex(key))
          this._indexedValues[(int) key] = value;
        else
          this._data[key] = value;
      }
    }

    public void Add(object key, TValue value) => this._data.Add(key, value);

    internal bool ContainsIndex(object o) => o is int index && this.ContainsIndex(index);

    internal bool ContainsIndex(int index) => this._indexedValues.Count > index && index >= 0;

    internal bool ContainsStringKey(object o) => o is string key && this.ContainsStringKey(key);

    internal bool ContainsStringKey(string key) => this._stringDictionary.ContainsKey(key);

    public bool ContainsKey(object key)
    {
      return this.ContainsIndex(key) || this.ContainsStringKey(key) || this._data.ContainsKey(key);
    }

    public bool Remove(object key)
    {
      if (this.ContainsStringKey(key))
        return this._stringDictionary.Remove((string) key);
      return this.ContainsIndex(key) ? this._indexedValues.Remove(this._indexedValues[(int) key]) : this._data.Remove(key);
    }

    public bool TryGetValue(object key, out TValue value)
    {
      if (this.ContainsStringKey(key))
        return this._stringDictionary.TryGetValue((string) key, out value);
      if (!this.ContainsIndex(key))
        return this._data.TryGetValue(key, out value);
      value = this._indexedValues[(int) key];
      return true;
    }

    public void Add(KeyValuePair<object, TValue> item) => this[item.Key] = item.Value;

    public void Clear()
    {
      this._stringDictionary.Clear();
      this._indexedValues.Clear();
      this._data.Clear();
    }

    public bool Contains(KeyValuePair<object, TValue> item)
    {
      return this.ContainsKey(item.Key) && this.Values.Contains(item.Value);
    }

    public void CopyTo(KeyValuePair<object, TValue>[] array, int arrayIndex)
    {
      this.Items.CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<object, TValue> item)
    {
      return this.Contains(item) && this.Remove(item.Key);
    }

    public IEnumerator<KeyValuePair<object, TValue>> GetEnumerator() => this.Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.Items.GetEnumerator();

    internal static IDictionary<object, object> CreatePageDataFromParameters(
      IDictionary<object, object> previousPageData,
      params object[] data)
    {
      PageDataDictionary<object> pageDataDictionary = previousPageData as PageDataDictionary<object>;
      PageDataDictionary<object> dataFromParameters = new PageDataDictionary<object>();
      foreach (KeyValuePair<object, object> keyValuePair in (IEnumerable<KeyValuePair<object, object>>) pageDataDictionary.Data)
        dataFromParameters.Data.Add(keyValuePair);
      if (data != null && data.Length > 0)
      {
        for (int index = 0; index < data.Length; ++index)
          dataFromParameters.IndexedValues.Add(data[index]);
        object obj = data[0];
        Type type = obj.GetType();
        if (TypeHelper.IsAnonymousType(type))
          TypeHelper.AddAnonymousObjectToDictionary(dataFromParameters.StringDictionary, obj);
        if (typeof (IDictionary<string, object>).IsAssignableFrom(type))
        {
          foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) (obj as IDictionary<string, object>))
            dataFromParameters.StringDictionary.Add(keyValuePair);
        }
      }
      return (IDictionary<object, object>) dataFromParameters;
    }

    private sealed class PageDataComparer : IEqualityComparer<object>
    {
      bool IEqualityComparer<object>.Equals(object x, object y)
      {
        string a = x as string;
        string b = y as string;
        return a != null && b != null ? string.Equals(a, b, StringComparison.OrdinalIgnoreCase) : object.Equals(x, y);
      }

      int IEqualityComparer<object>.GetHashCode(object obj)
      {
        return obj is string str ? str.ToUpperInvariant().GetHashCode() : obj.GetHashCode();
      }
    }
  }
}
