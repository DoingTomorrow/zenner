// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ValueProviderDictionary
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc
{
  [Obsolete("The recommended alternative is to use one of the specific ValueProvider types, such as FormValueProvider.")]
  public class ValueProviderDictionary : 
    IDictionary<string, ValueProviderResult>,
    ICollection<KeyValuePair<string, ValueProviderResult>>,
    IEnumerable<KeyValuePair<string, ValueProviderResult>>,
    IEnumerable,
    IValueProvider
  {
    private readonly System.Collections.Generic.Dictionary<string, ValueProviderResult> _dictionary = new System.Collections.Generic.Dictionary<string, ValueProviderResult>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    public ValueProviderDictionary(ControllerContext controllerContext)
    {
      this.ControllerContext = controllerContext;
      if (controllerContext == null)
        return;
      this.PopulateDictionary();
    }

    public ControllerContext ControllerContext { get; private set; }

    public int Count => this.Dictionary.Count;

    internal System.Collections.Generic.Dictionary<string, ValueProviderResult> Dictionary
    {
      get => this._dictionary;
    }

    public bool IsReadOnly
    {
      get => ((ICollection<KeyValuePair<string, ValueProviderResult>>) this.Dictionary).IsReadOnly;
    }

    public ICollection<string> Keys => (ICollection<string>) this.Dictionary.Keys;

    public ValueProviderResult this[string key]
    {
      get
      {
        ValueProviderResult valueProviderResult;
        this.Dictionary.TryGetValue(key, out valueProviderResult);
        return valueProviderResult;
      }
      set => this.Dictionary[key] = value;
    }

    public ICollection<ValueProviderResult> Values
    {
      get => (ICollection<ValueProviderResult>) this.Dictionary.Values;
    }

    public void Add(KeyValuePair<string, ValueProviderResult> item)
    {
      ((ICollection<KeyValuePair<string, ValueProviderResult>>) this.Dictionary).Add(item);
    }

    public void Add(string key, object value)
    {
      string attemptedValue = Convert.ToString(value, (IFormatProvider) CultureInfo.InvariantCulture);
      ValueProviderResult valueProviderResult = new ValueProviderResult(value, attemptedValue, CultureInfo.InvariantCulture);
      this.Add(key, valueProviderResult);
    }

    public void Add(string key, ValueProviderResult value) => this.Dictionary.Add(key, value);

    private void AddToDictionaryIfNotPresent(string key, ValueProviderResult result)
    {
      if (string.IsNullOrEmpty(key) || this.Dictionary.ContainsKey(key))
        return;
      this.Dictionary.Add(key, result);
    }

    public void Clear() => this.Dictionary.Clear();

    public bool Contains(KeyValuePair<string, ValueProviderResult> item)
    {
      return ((ICollection<KeyValuePair<string, ValueProviderResult>>) this.Dictionary).Contains(item);
    }

    public bool ContainsKey(string key) => this.Dictionary.ContainsKey(key);

    public void CopyTo(KeyValuePair<string, ValueProviderResult>[] array, int arrayIndex)
    {
      ((ICollection<KeyValuePair<string, ValueProviderResult>>) this.Dictionary).CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<string, ValueProviderResult>> GetEnumerator()
    {
      return ((IEnumerable<KeyValuePair<string, ValueProviderResult>>) this.Dictionary).GetEnumerator();
    }

    private void PopulateDictionary()
    {
      CultureInfo currentCulture = CultureInfo.CurrentCulture;
      CultureInfo invariantCulture = CultureInfo.InvariantCulture;
      NameValueCollection form = this.ControllerContext.HttpContext.Request.Form;
      if (form != null)
      {
        foreach (string allKey in form.AllKeys)
        {
          ValueProviderResult result = new ValueProviderResult((object) form.GetValues(allKey), form[allKey], currentCulture);
          this.AddToDictionaryIfNotPresent(allKey, result);
        }
      }
      RouteValueDictionary values = this.ControllerContext.RouteData.Values;
      if (values != null)
      {
        foreach (KeyValuePair<string, object> keyValuePair in values)
        {
          string key = keyValuePair.Key;
          object rawValue = keyValuePair.Value;
          string attemptedValue = Convert.ToString(rawValue, (IFormatProvider) invariantCulture);
          ValueProviderResult result = new ValueProviderResult(rawValue, attemptedValue, invariantCulture);
          this.AddToDictionaryIfNotPresent(key, result);
        }
      }
      NameValueCollection queryString = this.ControllerContext.HttpContext.Request.QueryString;
      if (queryString == null)
        return;
      foreach (string allKey in queryString.AllKeys)
      {
        ValueProviderResult result = new ValueProviderResult((object) queryString.GetValues(allKey), queryString[allKey], invariantCulture);
        this.AddToDictionaryIfNotPresent(allKey, result);
      }
    }

    public bool Remove(KeyValuePair<string, ValueProviderResult> item)
    {
      return ((ICollection<KeyValuePair<string, ValueProviderResult>>) this.Dictionary).Remove(item);
    }

    public bool Remove(string key) => this.Dictionary.Remove(key);

    public bool TryGetValue(string key, out ValueProviderResult value)
    {
      return this.Dictionary.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this.Dictionary).GetEnumerator();

    bool IValueProvider.ContainsPrefix(string prefix)
    {
      return prefix != null ? ValueProviderUtil.CollectionContainsPrefix((IEnumerable<string>) this.Keys, prefix) : throw new ArgumentNullException(nameof (prefix));
    }

    ValueProviderResult IValueProvider.GetValue(string key)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      ValueProviderResult valueProviderResult;
      this.TryGetValue(key, out valueProviderResult);
      return valueProviderResult;
    }
  }
}
