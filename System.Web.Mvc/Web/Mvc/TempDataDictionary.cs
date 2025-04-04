// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.TempDataDictionary
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  public class TempDataDictionary : 
    IDictionary<string, object>,
    ICollection<KeyValuePair<string, object>>,
    IEnumerable<KeyValuePair<string, object>>,
    IEnumerable
  {
    internal const string TempDataSerializationKey = "__tempData";
    private Dictionary<string, object> _data;
    private HashSet<string> _initialKeys = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private HashSet<string> _retainedKeys = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    public TempDataDictionary()
    {
      this._data = new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    }

    public int Count => this._data.Count;

    public ICollection<string> Keys => (ICollection<string>) this._data.Keys;

    public ICollection<object> Values => (ICollection<object>) this._data.Values;

    bool ICollection<KeyValuePair<string, object>>.IsReadOnly
    {
      get => ((ICollection<KeyValuePair<string, object>>) this._data).IsReadOnly;
    }

    public object this[string key]
    {
      get
      {
        object obj;
        if (!this.TryGetValue(key, out obj))
          return (object) null;
        this._initialKeys.Remove(key);
        return obj;
      }
      set
      {
        this._data[key] = value;
        this._initialKeys.Add(key);
      }
    }

    public void Keep()
    {
      this._retainedKeys.Clear();
      this._retainedKeys.UnionWith((IEnumerable<string>) this._data.Keys);
    }

    public void Keep(string key) => this._retainedKeys.Add(key);

    public void Load(ControllerContext controllerContext, ITempDataProvider tempDataProvider)
    {
      IDictionary<string, object> dictionary = tempDataProvider.LoadTempData(controllerContext);
      this._data = dictionary != null ? new Dictionary<string, object>(dictionary, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase) : new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this._initialKeys = new HashSet<string>((IEnumerable<string>) this._data.Keys, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this._retainedKeys.Clear();
    }

    public object Peek(string key)
    {
      object obj;
      this._data.TryGetValue(key, out obj);
      return obj;
    }

    public void Save(ControllerContext controllerContext, ITempDataProvider tempDataProvider)
    {
      foreach (string key in this._data.Keys.Except<string>((IEnumerable<string>) this._initialKeys.Union<string>((IEnumerable<string>) this._retainedKeys, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase).ToArray<string>(), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase).ToArray<string>())
        this._data.Remove(key);
      tempDataProvider.SaveTempData(controllerContext, (IDictionary<string, object>) this._data);
    }

    public void Add(string key, object value)
    {
      this._data.Add(key, value);
      this._initialKeys.Add(key);
    }

    public void Clear()
    {
      this._data.Clear();
      this._retainedKeys.Clear();
      this._initialKeys.Clear();
    }

    public bool ContainsKey(string key) => this._data.ContainsKey(key);

    public bool ContainsValue(object value) => this._data.ContainsValue(value);

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<string, object>>) new TempDataDictionary.TempDataDictionaryEnumerator(this);
    }

    public bool Remove(string key)
    {
      this._retainedKeys.Remove(key);
      this._initialKeys.Remove(key);
      return this._data.Remove(key);
    }

    public bool TryGetValue(string key, out object value)
    {
      this._initialKeys.Remove(key);
      return this._data.TryGetValue(key, out value);
    }

    void ICollection<KeyValuePair<string, object>>.CopyTo(
      KeyValuePair<string, object>[] array,
      int index)
    {
      ((ICollection<KeyValuePair<string, object>>) this._data).CopyTo(array, index);
    }

    void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> keyValuePair)
    {
      this._initialKeys.Add(keyValuePair.Key);
      ((ICollection<KeyValuePair<string, object>>) this._data).Add(keyValuePair);
    }

    bool ICollection<KeyValuePair<string, object>>.Contains(
      KeyValuePair<string, object> keyValuePair)
    {
      return ((ICollection<KeyValuePair<string, object>>) this._data).Contains(keyValuePair);
    }

    bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> keyValuePair)
    {
      this._initialKeys.Remove(keyValuePair.Key);
      return ((ICollection<KeyValuePair<string, object>>) this._data).Remove(keyValuePair);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) new TempDataDictionary.TempDataDictionaryEnumerator(this);
    }

    private sealed class TempDataDictionaryEnumerator : 
      IEnumerator<KeyValuePair<string, object>>,
      IDisposable,
      IEnumerator
    {
      private IEnumerator<KeyValuePair<string, object>> _enumerator;
      private TempDataDictionary _tempData;

      public TempDataDictionaryEnumerator(TempDataDictionary tempData)
      {
        this._tempData = tempData;
        this._enumerator = (IEnumerator<KeyValuePair<string, object>>) this._tempData._data.GetEnumerator();
      }

      public KeyValuePair<string, object> Current
      {
        get
        {
          KeyValuePair<string, object> current = this._enumerator.Current;
          this._tempData._initialKeys.Remove(current.Key);
          return current;
        }
      }

      object IEnumerator.Current => (object) this.Current;

      public bool MoveNext() => this._enumerator.MoveNext();

      public void Reset() => this._enumerator.Reset();

      void IDisposable.Dispose() => this._enumerator.Dispose();
    }
  }
}
