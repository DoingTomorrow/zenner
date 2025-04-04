// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.DynamicPageDataDictionary`1
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.WebPages
{
  internal class DynamicPageDataDictionary<TValue> : 
    DynamicObject,
    IDictionary<object, TValue>,
    ICollection<KeyValuePair<object, TValue>>,
    IEnumerable<KeyValuePair<object, TValue>>,
    IEnumerable
  {
    private PageDataDictionary<TValue> _data;

    public DynamicPageDataDictionary(PageDataDictionary<TValue> dictionary)
    {
      this._data = dictionary;
    }

    public ICollection<object> Keys => this._data.Keys;

    public ICollection<TValue> Values => this._data.Values;

    public int Count => this._data.Count;

    public bool IsReadOnly => this._data.IsReadOnly;

    public TValue this[object key]
    {
      get => this._data[key];
      set => this._data[key] = value;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
      result = (object) this._data[(object) binder.Name];
      return true;
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
      TValue obj = (TValue) value;
      this._data[(object) binder.Name] = obj;
      return true;
    }

    public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
    {
      result = indexes != null && indexes.Length == 1 ? (object) this._data[indexes[0]] : throw new ArgumentException(WebPageResources.DynamicDictionary_InvalidNumberOfIndexes);
      return true;
    }

    public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
    {
      if (indexes == null || indexes.Length != 1)
        throw new ArgumentException(WebPageResources.DynamicDictionary_InvalidNumberOfIndexes);
      this._data[indexes[0]] = (TValue) value;
      return true;
    }

    public void Add(object key, TValue value) => this._data.Add(key, value);

    public bool ContainsKey(object key) => this._data.ContainsKey(key);

    public bool Remove(object key) => this._data.Remove(key);

    public bool TryGetValue(object key, out TValue value) => this._data.TryGetValue(key, out value);

    public void Add(KeyValuePair<object, TValue> item) => this._data.Add(item);

    public void Clear() => this._data.Clear();

    public bool Contains(KeyValuePair<object, TValue> item) => this._data.Contains(item);

    public void CopyTo(KeyValuePair<object, TValue>[] array, int arrayIndex)
    {
      this._data.CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<object, TValue> item) => this._data.Remove(item.Key);

    public IEnumerator<KeyValuePair<object, TValue>> GetEnumerator() => this._data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._data.GetEnumerator();
  }
}
