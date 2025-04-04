// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Html.ModelStateDictionary
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Web.WebPages.Html
{
  public class ModelStateDictionary : 
    IDictionary<string, ModelState>,
    ICollection<KeyValuePair<string, ModelState>>,
    IEnumerable<KeyValuePair<string, ModelState>>,
    IEnumerable
  {
    internal const string FormFieldKey = "_FORM";
    private readonly Dictionary<string, ModelState> _innerDictionary = new Dictionary<string, ModelState>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    public ModelStateDictionary()
    {
    }

    public ModelStateDictionary(ModelStateDictionary dictionary)
    {
      if (dictionary == null)
        throw new ArgumentNullException(nameof (dictionary));
      foreach (KeyValuePair<string, ModelState> keyValuePair in dictionary)
        this._innerDictionary.Add(keyValuePair.Key, keyValuePair.Value);
    }

    public int Count => this._innerDictionary.Count;

    public bool IsReadOnly
    {
      get => ((ICollection<KeyValuePair<string, ModelState>>) this._innerDictionary).IsReadOnly;
    }

    public bool IsValid
    {
      get
      {
        return !this.Values.SelectMany<ModelState, string>((Func<ModelState, IEnumerable<string>>) (modelState => (IEnumerable<string>) modelState.Errors)).Any<string>();
      }
    }

    public ICollection<string> Keys => (ICollection<string>) this._innerDictionary.Keys;

    public ICollection<ModelState> Values => (ICollection<ModelState>) this._innerDictionary.Values;

    public ModelState this[string key]
    {
      get
      {
        ModelState modelState;
        this._innerDictionary.TryGetValue(key, out modelState);
        return modelState;
      }
      set => this._innerDictionary[key] = value;
    }

    public void Add(KeyValuePair<string, ModelState> item)
    {
      ((ICollection<KeyValuePair<string, ModelState>>) this._innerDictionary).Add(item);
    }

    public void Add(string key, ModelState value) => this._innerDictionary.Add(key, value);

    public void AddError(string key, string errorMessage)
    {
      this.GetModelStateForKey(key).Errors.Add(errorMessage);
    }

    public void AddFormError(string errorMessage)
    {
      this.GetModelStateForKey("_FORM").Errors.Add(errorMessage);
    }

    public void Clear() => this._innerDictionary.Clear();

    public bool Contains(KeyValuePair<string, ModelState> item)
    {
      return ((ICollection<KeyValuePair<string, ModelState>>) this._innerDictionary).Contains(item);
    }

    public bool ContainsKey(string key) => this._innerDictionary.ContainsKey(key);

    public void CopyTo(KeyValuePair<string, ModelState>[] array, int arrayIndex)
    {
      ((ICollection<KeyValuePair<string, ModelState>>) this._innerDictionary).CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<string, ModelState>> GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<string, ModelState>>) this._innerDictionary.GetEnumerator();
    }

    private ModelState GetModelStateForKey(string key)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      ModelState modelStateForKey;
      if (!this.TryGetValue(key, out modelStateForKey))
      {
        modelStateForKey = new ModelState();
        this._innerDictionary[key] = modelStateForKey;
      }
      return modelStateForKey;
    }

    public bool IsValidField(string key)
    {
      ModelState modelState = key != null ? this[key] : throw new ArgumentNullException(nameof (key));
      return modelState == null || !modelState.Errors.Any<string>();
    }

    public void Merge(ModelStateDictionary dictionary)
    {
      if (dictionary == null)
        return;
      foreach (KeyValuePair<string, ModelState> keyValuePair in dictionary)
        this[keyValuePair.Key] = keyValuePair.Value;
    }

    public bool Remove(KeyValuePair<string, ModelState> item)
    {
      return ((ICollection<KeyValuePair<string, ModelState>>) this._innerDictionary).Remove(item);
    }

    public bool Remove(string key) => this._innerDictionary.Remove(key);

    public bool TryGetValue(string key, out ModelState value)
    {
      return this._innerDictionary.TryGetValue(key, out value);
    }

    public void SetModelValue(string key, object value)
    {
      this.GetModelStateForKey(key).Value = value;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable) this._innerDictionary).GetEnumerator();
    }
  }
}
