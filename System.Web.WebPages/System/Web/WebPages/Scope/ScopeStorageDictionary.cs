// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Scope.ScopeStorageDictionary
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Web.WebPages.Scope
{
  public class ScopeStorageDictionary : 
    IDictionary<object, object>,
    ICollection<KeyValuePair<object, object>>,
    IEnumerable<KeyValuePair<object, object>>,
    IEnumerable
  {
    private static readonly ScopeStorageDictionary.StateStorageKeyValueComparer _keyValueComparer = new ScopeStorageDictionary.StateStorageKeyValueComparer();
    private readonly IDictionary<object, object> _baseScope;
    private readonly IDictionary<object, object> _backingStore;

    public ScopeStorageDictionary()
      : this((IDictionary<object, object>) null)
    {
    }

    public ScopeStorageDictionary(IDictionary<object, object> baseScope)
      : this(baseScope, (IDictionary<object, object>) new Dictionary<object, object>(ScopeStorageComparer.Instance))
    {
    }

    internal ScopeStorageDictionary(
      IDictionary<object, object> baseScope,
      IDictionary<object, object> backingStore)
    {
      this._baseScope = baseScope;
      this._backingStore = backingStore;
    }

    protected IDictionary<object, object> BackingStore => this._backingStore;

    protected IDictionary<object, object> BaseScope => this._baseScope;

    public virtual ICollection<object> Keys
    {
      get
      {
        return (ICollection<object>) this.GetItems().Select<KeyValuePair<object, object>, object>((Func<KeyValuePair<object, object>, object>) (item => item.Key)).ToList<object>();
      }
    }

    public virtual ICollection<object> Values
    {
      get
      {
        return (ICollection<object>) this.GetItems().Select<KeyValuePair<object, object>, object>((Func<KeyValuePair<object, object>, object>) (item => item.Value)).ToList<object>();
      }
    }

    public virtual int Count => this.GetItems().Count<KeyValuePair<object, object>>();

    public virtual bool IsReadOnly => false;

    public object this[object key]
    {
      get
      {
        object obj;
        this.TryGetValue(key, out obj);
        return obj;
      }
      set => this.SetValue(key, value);
    }

    public virtual void SetValue(object key, object value) => this._backingStore[key] = value;

    public virtual bool TryGetValue(object key, out object value)
    {
      if (this._backingStore.TryGetValue(key, out value))
        return true;
      return this._baseScope != null && this._baseScope.TryGetValue(key, out value);
    }

    public virtual bool Remove(object key) => this._backingStore.Remove(key);

    public virtual IEnumerator<KeyValuePair<object, object>> GetEnumerator()
    {
      return this.GetItems().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public virtual void Add(object key, object value) => this.SetValue(key, value);

    public virtual bool ContainsKey(object key)
    {
      if (this._backingStore.ContainsKey(key))
        return true;
      return this._baseScope != null && this._baseScope.ContainsKey(key);
    }

    public virtual void Add(KeyValuePair<object, object> item)
    {
      this.SetValue(item.Key, item.Value);
    }

    public virtual void Clear() => this._backingStore.Clear();

    public virtual bool Contains(KeyValuePair<object, object> item)
    {
      if (this._backingStore.Contains(item))
        return true;
      return this._baseScope != null && this._baseScope.Contains(item);
    }

    public virtual void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex)
    {
      this.GetItems().ToList<KeyValuePair<object, object>>().CopyTo(array, arrayIndex);
    }

    public virtual bool Remove(KeyValuePair<object, object> item)
    {
      return this._backingStore.Remove((object) item);
    }

    protected virtual IEnumerable<KeyValuePair<object, object>> GetItems()
    {
      return this._baseScope == null ? (IEnumerable<KeyValuePair<object, object>>) this._backingStore : this._backingStore.Concat<KeyValuePair<object, object>>((IEnumerable<KeyValuePair<object, object>>) this._baseScope).Distinct<KeyValuePair<object, object>>((IEqualityComparer<KeyValuePair<object, object>>) ScopeStorageDictionary._keyValueComparer);
    }

    private class StateStorageKeyValueComparer : IEqualityComparer<KeyValuePair<object, object>>
    {
      private IEqualityComparer<object> _stateStorageComparer = ScopeStorageComparer.Instance;

      public bool Equals(KeyValuePair<object, object> x, KeyValuePair<object, object> y)
      {
        return this._stateStorageComparer.Equals(x.Key, y.Key);
      }

      public int GetHashCode(KeyValuePair<object, object> obj)
      {
        return this._stateStorageComparer.GetHashCode(obj.Key);
      }
    }
  }
}
