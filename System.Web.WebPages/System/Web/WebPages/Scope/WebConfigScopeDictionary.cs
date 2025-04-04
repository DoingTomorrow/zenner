// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Scope.WebConfigScopeDictionary
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Configuration;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.WebPages.Scope
{
  internal class WebConfigScopeDictionary : 
    IDictionary<object, object>,
    ICollection<KeyValuePair<object, object>>,
    IEnumerable<KeyValuePair<object, object>>,
    IEnumerable
  {
    private readonly Lazy<Dictionary<object, object>> _items;

    public WebConfigScopeDictionary()
      : this(WebConfigurationManager.AppSettings)
    {
    }

    public WebConfigScopeDictionary(NameValueCollection appSettings)
    {
      this._items = new Lazy<Dictionary<object, object>>((Func<Dictionary<object, object>>) (() => ((IEnumerable<string>) appSettings.AllKeys).ToDictionary<string, object, object>((Func<string, object>) (key => (object) key), (Func<string, object>) (key => (object) appSettings[key]), ScopeStorageComparer.Instance)));
    }

    private IDictionary<object, object> Items => (IDictionary<object, object>) this._items.Value;

    public ICollection<object> Keys => this.Items.Keys;

    public ICollection<object> Values => this.Items.Values;

    public int Count => this.Items.Count;

    public bool IsReadOnly => true;

    public object this[object key]
    {
      get
      {
        object obj;
        this.TryGetValue(key, out obj);
        return obj;
      }
      set => throw new NotSupportedException(WebPageResources.StateStorage_ScopeIsReadOnly);
    }

    public bool TryGetValue(object key, out object value) => this.Items.TryGetValue(key, out value);

    public IEnumerator<KeyValuePair<object, object>> GetEnumerator() => this.Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public void Add(object key, object value)
    {
      throw new NotSupportedException(WebPageResources.StateStorage_ScopeIsReadOnly);
    }

    public bool ContainsKey(object key) => this.Items.ContainsKey(key);

    public bool Remove(object key)
    {
      throw new NotSupportedException(WebPageResources.StateStorage_ScopeIsReadOnly);
    }

    public void Add(KeyValuePair<object, object> item)
    {
      throw new NotSupportedException(WebPageResources.StateStorage_ScopeIsReadOnly);
    }

    public void Clear()
    {
      throw new NotSupportedException(WebPageResources.StateStorage_ScopeIsReadOnly);
    }

    public bool Contains(KeyValuePair<object, object> item) => this.Items.Contains(item);

    public void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex)
    {
      this.Items.CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<object, object> item)
    {
      throw new NotSupportedException(WebPageResources.StateStorage_ScopeIsReadOnly);
    }
  }
}
