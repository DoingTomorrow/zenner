// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ModelBinderDictionary
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class ModelBinderDictionary : 
    IDictionary<Type, IModelBinder>,
    ICollection<KeyValuePair<Type, IModelBinder>>,
    IEnumerable<KeyValuePair<Type, IModelBinder>>,
    IEnumerable
  {
    private readonly Dictionary<Type, IModelBinder> _innerDictionary = new Dictionary<Type, IModelBinder>();
    private IModelBinder _defaultBinder;
    private ModelBinderProviderCollection _modelBinderProviders;

    public ModelBinderDictionary()
      : this(ModelBinderProviders.BinderProviders)
    {
    }

    internal ModelBinderDictionary(ModelBinderProviderCollection modelBinderProviders)
    {
      this._modelBinderProviders = modelBinderProviders;
    }

    public int Count => this._innerDictionary.Count;

    public IModelBinder DefaultBinder
    {
      get
      {
        if (this._defaultBinder == null)
          this._defaultBinder = (IModelBinder) new DefaultModelBinder();
        return this._defaultBinder;
      }
      set => this._defaultBinder = value;
    }

    public bool IsReadOnly
    {
      get => ((ICollection<KeyValuePair<Type, IModelBinder>>) this._innerDictionary).IsReadOnly;
    }

    public ICollection<Type> Keys => (ICollection<Type>) this._innerDictionary.Keys;

    public ICollection<IModelBinder> Values
    {
      get => (ICollection<IModelBinder>) this._innerDictionary.Values;
    }

    public IModelBinder this[Type key]
    {
      get
      {
        IModelBinder modelBinder;
        this._innerDictionary.TryGetValue(key, out modelBinder);
        return modelBinder;
      }
      set => this._innerDictionary[key] = value;
    }

    public void Add(KeyValuePair<Type, IModelBinder> item)
    {
      ((ICollection<KeyValuePair<Type, IModelBinder>>) this._innerDictionary).Add(item);
    }

    public void Add(Type key, IModelBinder value) => this._innerDictionary.Add(key, value);

    public void Clear() => this._innerDictionary.Clear();

    public bool Contains(KeyValuePair<Type, IModelBinder> item)
    {
      return ((ICollection<KeyValuePair<Type, IModelBinder>>) this._innerDictionary).Contains(item);
    }

    public bool ContainsKey(Type key) => this._innerDictionary.ContainsKey(key);

    public void CopyTo(KeyValuePair<Type, IModelBinder>[] array, int arrayIndex)
    {
      ((ICollection<KeyValuePair<Type, IModelBinder>>) this._innerDictionary).CopyTo(array, arrayIndex);
    }

    public IModelBinder GetBinder(Type modelType) => this.GetBinder(modelType, true);

    public virtual IModelBinder GetBinder(Type modelType, bool fallbackToDefault)
    {
      if (modelType == (Type) null)
        throw new ArgumentNullException(nameof (modelType));
      return this.GetBinder(modelType, fallbackToDefault ? this.DefaultBinder : (IModelBinder) null);
    }

    private IModelBinder GetBinder(Type modelType, IModelBinder fallbackBinder)
    {
      IModelBinder binder = this._modelBinderProviders.GetBinder(modelType);
      if (binder != null || this._innerDictionary.TryGetValue(modelType, out binder))
        return binder;
      binder = ModelBinders.GetBinderFromAttributes(modelType, (Func<string>) (() => string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ModelBinderDictionary_MultipleAttributes, new object[1]
      {
        (object) modelType.FullName
      })));
      return binder ?? fallbackBinder;
    }

    public IEnumerator<KeyValuePair<Type, IModelBinder>> GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<Type, IModelBinder>>) this._innerDictionary.GetEnumerator();
    }

    public bool Remove(KeyValuePair<Type, IModelBinder> item)
    {
      return ((ICollection<KeyValuePair<Type, IModelBinder>>) this._innerDictionary).Remove(item);
    }

    public bool Remove(Type key) => this._innerDictionary.Remove(key);

    public bool TryGetValue(Type key, out IModelBinder value)
    {
      return this._innerDictionary.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable) this._innerDictionary).GetEnumerator();
    }
  }
}
