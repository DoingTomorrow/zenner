// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ModelMetadata
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc.ExpressionUtil;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class ModelMetadata
  {
    public const int DefaultOrder = 10000;
    private readonly Type _containerType;
    private readonly Type _modelType;
    private readonly string _propertyName;
    private Dictionary<string, object> _additionalValues = new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private bool _convertEmptyStringToNull = true;
    private bool _htmlEncode = true;
    private bool _isRequired;
    private object _model;
    private Func<object> _modelAccessor;
    private int _order = 10000;
    private IEnumerable<ModelMetadata> _properties;
    private Type _realModelType;
    private bool _requestValidationEnabled = true;
    private bool _showForDisplay = true;
    private bool _showForEdit = true;
    private string _simpleDisplayText;

    public ModelMetadata(
      ModelMetadataProvider provider,
      Type containerType,
      Func<object> modelAccessor,
      Type modelType,
      string propertyName)
    {
      if (provider == null)
        throw new ArgumentNullException(nameof (provider));
      if (modelType == (Type) null)
        throw new ArgumentNullException(nameof (modelType));
      this.Provider = provider;
      this._containerType = containerType;
      this._isRequired = !TypeHelpers.TypeAllowsNullValue(modelType);
      this._modelAccessor = modelAccessor;
      this._modelType = modelType;
      this._propertyName = propertyName;
    }

    public virtual Dictionary<string, object> AdditionalValues => this._additionalValues;

    public Type ContainerType => this._containerType;

    public virtual bool ConvertEmptyStringToNull
    {
      get => this._convertEmptyStringToNull;
      set => this._convertEmptyStringToNull = value;
    }

    public virtual string DataTypeName { get; set; }

    public virtual string Description { get; set; }

    public virtual string DisplayFormatString { get; set; }

    public virtual string DisplayName { get; set; }

    public virtual string EditFormatString { get; set; }

    public virtual bool HideSurroundingHtml { get; set; }

    public virtual bool HtmlEncode
    {
      get => this._htmlEncode;
      set => this._htmlEncode = value;
    }

    public virtual bool IsComplexType
    {
      get => !TypeDescriptor.GetConverter(this.ModelType).CanConvertFrom(typeof (string));
    }

    public bool IsNullableValueType => TypeHelpers.IsNullableValueType(this.ModelType);

    public virtual bool IsReadOnly { get; set; }

    public virtual bool IsRequired
    {
      get => this._isRequired;
      set => this._isRequired = value;
    }

    public object Model
    {
      get
      {
        if (this._modelAccessor != null)
        {
          this._model = this._modelAccessor();
          this._modelAccessor = (Func<object>) null;
        }
        return this._model;
      }
      set
      {
        this._model = value;
        this._modelAccessor = (Func<object>) null;
        this._properties = (IEnumerable<ModelMetadata>) null;
        this._realModelType = (Type) null;
      }
    }

    public Type ModelType => this._modelType;

    public virtual string NullDisplayText { get; set; }

    public virtual int Order
    {
      get => this._order;
      set => this._order = value;
    }

    public virtual IEnumerable<ModelMetadata> Properties
    {
      get
      {
        if (this._properties == null)
          this._properties = (IEnumerable<ModelMetadata>) this.Provider.GetMetadataForProperties(this.Model, this.RealModelType).OrderBy<ModelMetadata, int>((Func<ModelMetadata, int>) (m => m.Order));
        return this._properties;
      }
    }

    public string PropertyName => this._propertyName;

    protected ModelMetadataProvider Provider { get; set; }

    internal Type RealModelType
    {
      get
      {
        if (this._realModelType == (Type) null)
        {
          this._realModelType = this.ModelType;
          if (this.Model != null && !TypeHelpers.IsNullableValueType(this.ModelType))
            this._realModelType = this.Model.GetType();
        }
        return this._realModelType;
      }
    }

    public virtual bool RequestValidationEnabled
    {
      get => this._requestValidationEnabled;
      set => this._requestValidationEnabled = value;
    }

    public virtual string ShortDisplayName { get; set; }

    public virtual bool ShowForDisplay
    {
      get => this._showForDisplay;
      set => this._showForDisplay = value;
    }

    public virtual bool ShowForEdit
    {
      get => this._showForEdit;
      set => this._showForEdit = value;
    }

    public virtual string SimpleDisplayText
    {
      get
      {
        if (this._simpleDisplayText == null)
          this._simpleDisplayText = this.GetSimpleDisplayText();
        return this._simpleDisplayText;
      }
      set => this._simpleDisplayText = value;
    }

    public virtual string TemplateHint { get; set; }

    public virtual string Watermark { get; set; }

    public static ModelMetadata FromLambdaExpression<TParameter, TValue>(
      Expression<Func<TParameter, TValue>> expression,
      ViewDataDictionary<TParameter> viewData)
    {
      return ModelMetadata.FromLambdaExpression<TParameter, TValue>(expression, viewData, (ModelMetadataProvider) null);
    }

    internal static ModelMetadata FromLambdaExpression<TParameter, TValue>(
      Expression<Func<TParameter, TValue>> expression,
      ViewDataDictionary<TParameter> viewData,
      ModelMetadataProvider metadataProvider)
    {
      if (expression == null)
        throw new ArgumentNullException(nameof (expression));
      if (viewData == null)
        throw new ArgumentNullException(nameof (viewData));
      string propertyName = (string) null;
      Type containerType = (Type) null;
      bool flag = false;
      switch (expression.Body.NodeType)
      {
        case ExpressionType.ArrayIndex:
          flag = true;
          break;
        case ExpressionType.Call:
          flag = ExpressionHelper.IsSingleArgumentIndexer(expression.Body);
          break;
        case ExpressionType.MemberAccess:
          MemberExpression body = (MemberExpression) expression.Body;
          propertyName = (object) (body.Member as PropertyInfo) != null ? body.Member.Name : (string) null;
          containerType = body.Expression.Type;
          flag = true;
          break;
        case ExpressionType.Parameter:
          return ModelMetadata.FromModel((ViewDataDictionary) viewData, metadataProvider);
      }
      if (!flag)
        throw new InvalidOperationException(MvcResources.TemplateHelpers_TemplateLimitations);
      TParameter container = viewData.Model;
      return ModelMetadata.GetMetadataFromProvider((Func<object>) (() =>
      {
        try
        {
          return (object) CachedExpressionCompiler.Process<TParameter, TValue>(expression)(container);
        }
        catch (NullReferenceException ex)
        {
          return (object) null;
        }
      }), typeof (TValue), propertyName, containerType, metadataProvider);
    }

    private static ModelMetadata FromModel(
      ViewDataDictionary viewData,
      ModelMetadataProvider metadataProvider)
    {
      return viewData.ModelMetadata ?? ModelMetadata.GetMetadataFromProvider((Func<object>) null, typeof (string), (string) null, (Type) null, metadataProvider);
    }

    public static ModelMetadata FromStringExpression(string expression, ViewDataDictionary viewData)
    {
      return ModelMetadata.FromStringExpression(expression, viewData, (ModelMetadataProvider) null);
    }

    internal static ModelMetadata FromStringExpression(
      string expression,
      ViewDataDictionary viewData,
      ModelMetadataProvider metadataProvider)
    {
      if (expression == null)
        throw new ArgumentNullException(nameof (expression));
      if (viewData == null)
        throw new ArgumentNullException(nameof (viewData));
      if (expression.Length == 0)
        return ModelMetadata.FromModel(viewData, metadataProvider);
      ViewDataInfo vdi = viewData.GetViewDataInfo(expression);
      Type type1 = (Type) null;
      Type type2 = (Type) null;
      Func<object> func = (Func<object>) null;
      string str = (string) null;
      if (vdi != null)
      {
        if (vdi.Container != null)
          type1 = vdi.Container.GetType();
        func = (Func<object>) (() => vdi.Value);
        if (vdi.PropertyDescriptor != null)
        {
          str = vdi.PropertyDescriptor.Name;
          type2 = vdi.PropertyDescriptor.PropertyType;
        }
        else if (vdi.Value != null)
          type2 = vdi.Value.GetType();
      }
      else if (viewData.ModelMetadata != null)
      {
        ModelMetadata modelMetadata = viewData.ModelMetadata.Properties.Where<ModelMetadata>((Func<ModelMetadata, bool>) (p => p.PropertyName == expression)).FirstOrDefault<ModelMetadata>();
        if (modelMetadata != null)
          return modelMetadata;
      }
      Func<object> modelAccessor = func;
      Type modelType = type2;
      if ((object) modelType == null)
        modelType = typeof (string);
      string propertyName = str;
      Type containerType = type1;
      ModelMetadataProvider metadataProvider1 = metadataProvider;
      return ModelMetadata.GetMetadataFromProvider(modelAccessor, modelType, propertyName, containerType, metadataProvider1);
    }

    public string GetDisplayName() => this.DisplayName ?? this.PropertyName ?? this.ModelType.Name;

    private static ModelMetadata GetMetadataFromProvider(
      Func<object> modelAccessor,
      Type modelType,
      string propertyName,
      Type containerType,
      ModelMetadataProvider metadataProvider)
    {
      metadataProvider = metadataProvider ?? ModelMetadataProviders.Current;
      return containerType != (Type) null && !string.IsNullOrEmpty(propertyName) ? metadataProvider.GetMetadataForProperty(modelAccessor, containerType, propertyName) : metadataProvider.GetMetadataForType(modelAccessor, modelType);
    }

    protected virtual string GetSimpleDisplayText()
    {
      if (this.Model == null)
        return this.NullDisplayText;
      string simpleDisplayText = Convert.ToString(this.Model, (IFormatProvider) CultureInfo.CurrentCulture);
      if (simpleDisplayText == null)
        return string.Empty;
      if (!simpleDisplayText.Equals(this.Model.GetType().FullName, StringComparison.Ordinal))
        return simpleDisplayText;
      ModelMetadata modelMetadata = this.Properties.FirstOrDefault<ModelMetadata>();
      if (modelMetadata == null)
        return string.Empty;
      return modelMetadata.Model == null ? modelMetadata.NullDisplayText : Convert.ToString(modelMetadata.Model, (IFormatProvider) CultureInfo.CurrentCulture);
    }

    public virtual IEnumerable<ModelValidator> GetValidators(ControllerContext context)
    {
      return ModelValidatorProviders.Providers.GetValidators(this, context);
    }
  }
}
