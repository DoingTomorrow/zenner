// Decompiled with JetBrains decompiler
// Type: RestSharp.JsonObject
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

#nullable disable
namespace RestSharp
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public class JsonObject : 
    DynamicObject,
    IDictionary<string, object>,
    ICollection<KeyValuePair<string, object>>,
    IEnumerable<KeyValuePair<string, object>>,
    IEnumerable
  {
    private readonly Dictionary<string, object> _members = new Dictionary<string, object>();

    public object this[int index]
    {
      get => JsonObject.GetAtIndex((IDictionary<string, object>) this._members, index);
    }

    internal static object GetAtIndex(IDictionary<string, object> obj, int index)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      if (index >= obj.Count)
        throw new ArgumentOutOfRangeException(nameof (index));
      int num = 0;
      foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) obj)
      {
        if (num++ == index)
          return keyValuePair.Value;
      }
      return (object) null;
    }

    public void Add(string key, object value) => this._members.Add(key, value);

    public bool ContainsKey(string key) => this._members.ContainsKey(key);

    public ICollection<string> Keys => (ICollection<string>) this._members.Keys;

    public bool Remove(string key) => this._members.Remove(key);

    public bool TryGetValue(string key, out object value)
    {
      return this._members.TryGetValue(key, out value);
    }

    public ICollection<object> Values => (ICollection<object>) this._members.Values;

    public object this[string key]
    {
      get => this._members[key];
      set => this._members[key] = value;
    }

    public void Add(KeyValuePair<string, object> item) => this._members.Add(item.Key, item.Value);

    public void Clear() => this._members.Clear();

    public bool Contains(KeyValuePair<string, object> item)
    {
      return this._members.ContainsKey(item.Key) && this._members[item.Key] == item.Value;
    }

    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
      int count = this.Count;
      foreach (KeyValuePair<string, object> keyValuePair in this)
      {
        array[arrayIndex++] = keyValuePair;
        if (--count <= 0)
          break;
      }
    }

    public int Count => this._members.Count;

    public bool IsReadOnly => false;

    public bool Remove(KeyValuePair<string, object> item) => this._members.Remove(item.Key);

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<string, object>>) this._members.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._members.GetEnumerator();

    public override string ToString() => SimpleJson.SerializeObject((object) this);

    public override bool TryConvert(ConvertBinder binder, out object result)
    {
      Type type = binder != null ? binder.Type : throw new ArgumentNullException(nameof (binder));
      if (!(type == typeof (IEnumerable)) && !(type == typeof (IEnumerable<KeyValuePair<string, object>>)) && !(type == typeof (IDictionary<string, object>)) && !(type == typeof (IDictionary)))
        return base.TryConvert(binder, out result);
      result = (object) this;
      return true;
    }

    public override bool TryDeleteMember(DeleteMemberBinder binder)
    {
      if (binder == null)
        throw new ArgumentNullException(nameof (binder));
      return this._members.Remove(binder.Name);
    }

    public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
    {
      if (indexes.Length == 1)
      {
        result = this[(string) indexes[0]];
        return true;
      }
      result = (object) null;
      return true;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
      object obj;
      if (this._members.TryGetValue(binder.Name, out obj))
      {
        result = obj;
        return true;
      }
      result = (object) null;
      return true;
    }

    public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
    {
      if (indexes.Length != 1)
        return base.TrySetIndex(binder, indexes, value);
      this[(string) indexes[0]] = value;
      return true;
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
      if (binder == null)
        throw new ArgumentNullException(nameof (binder));
      this._members[binder.Name] = value;
      return true;
    }

    public override IEnumerable<string> GetDynamicMemberNames()
    {
      foreach (string key in (IEnumerable<string>) this.Keys)
        yield return key;
    }
  }
}
