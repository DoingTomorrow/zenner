// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ModelBindingContext
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class ModelBindingContext
  {
    private static readonly Predicate<string> _defaultPropertyFilter = (Predicate<string>) (_ => true);
    private string _modelName;
    private ModelStateDictionary _modelState;
    private Predicate<string> _propertyFilter;
    private Dictionary<string, ModelMetadata> _propertyMetadata;

    public ModelBindingContext()
      : this((ModelBindingContext) null)
    {
    }

    public ModelBindingContext(ModelBindingContext bindingContext)
    {
      if (bindingContext == null)
        return;
      this.ModelState = bindingContext.ModelState;
      this.ValueProvider = bindingContext.ValueProvider;
    }

    public bool FallbackToEmptyPrefix { get; set; }

    public object Model
    {
      get => this.ModelMetadata.Model;
      set => throw new InvalidOperationException(MvcResources.ModelMetadata_PropertyNotSettable);
    }

    public ModelMetadata ModelMetadata { get; set; }

    public string ModelName
    {
      get
      {
        if (this._modelName == null)
          this._modelName = string.Empty;
        return this._modelName;
      }
      set => this._modelName = value;
    }

    public ModelStateDictionary ModelState
    {
      get
      {
        if (this._modelState == null)
          this._modelState = new ModelStateDictionary();
        return this._modelState;
      }
      set => this._modelState = value;
    }

    public Type ModelType
    {
      get => this.ModelMetadata.ModelType;
      set => throw new InvalidOperationException(MvcResources.ModelMetadata_PropertyNotSettable);
    }

    public Predicate<string> PropertyFilter
    {
      get
      {
        if (this._propertyFilter == null)
          this._propertyFilter = ModelBindingContext._defaultPropertyFilter;
        return this._propertyFilter;
      }
      set => this._propertyFilter = value;
    }

    public IDictionary<string, ModelMetadata> PropertyMetadata
    {
      get
      {
        if (this._propertyMetadata == null)
          this._propertyMetadata = this.ModelMetadata.Properties.ToDictionary<ModelMetadata, string>((Func<ModelMetadata, string>) (m => m.PropertyName), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
        return (IDictionary<string, ModelMetadata>) this._propertyMetadata;
      }
    }

    public IValueProvider ValueProvider { get; set; }

    internal IUnvalidatedValueProvider UnvalidatedValueProvider
    {
      get
      {
        return this.ValueProvider is IUnvalidatedValueProvider valueProvider ? valueProvider : (IUnvalidatedValueProvider) new ModelBindingContext.UnvalidatedValueProviderWrapper(this.ValueProvider);
      }
    }

    private sealed class UnvalidatedValueProviderWrapper : IUnvalidatedValueProvider, IValueProvider
    {
      private readonly IValueProvider _backingProvider;

      public UnvalidatedValueProviderWrapper(IValueProvider backingProvider)
      {
        this._backingProvider = backingProvider;
      }

      public ValueProviderResult GetValue(string key, bool skipValidation) => this.GetValue(key);

      public bool ContainsPrefix(string prefix) => this._backingProvider.ContainsPrefix(prefix);

      public ValueProviderResult GetValue(string key) => this._backingProvider.GetValue(key);
    }
  }
}
