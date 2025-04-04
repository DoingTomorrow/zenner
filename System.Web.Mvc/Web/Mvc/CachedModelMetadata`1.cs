// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.CachedModelMetadata`1
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public abstract class CachedModelMetadata<TPrototypeCache> : ModelMetadata
  {
    private bool _convertEmptyStringToNull;
    private string _dataTypeName;
    private string _description;
    private string _displayFormatString;
    private string _displayName;
    private string _editFormatString;
    private bool _hideSurroundingHtml;
    private bool _htmlEncode;
    private bool _isReadOnly;
    private bool _isRequired;
    private string _nullDisplayText;
    private int _order;
    private string _shortDisplayName;
    private bool _showForDisplay;
    private bool _showForEdit;
    private string _templateHint;
    private string _watermark;
    private bool _convertEmptyStringToNullComputed;
    private bool _dataTypeNameComputed;
    private bool _descriptionComputed;
    private bool _displayFormatStringComputed;
    private bool _displayNameComputed;
    private bool _editFormatStringComputed;
    private bool _hideSurroundingHtmlComputed;
    private bool _htmlEncodeComputed;
    private bool _isReadOnlyComputed;
    private bool _isRequiredComputed;
    private bool _nullDisplayTextComputed;
    private bool _orderComputed;
    private bool _shortDisplayNameComputed;
    private bool _showForDisplayComputed;
    private bool _showForEditComputed;
    private bool _templateHintComputed;
    private bool _watermarkComputed;

    protected CachedModelMetadata(
      CachedModelMetadata<TPrototypeCache> prototype,
      Func<object> modelAccessor)
      : base(prototype.Provider, prototype.ContainerType, modelAccessor, prototype.ModelType, prototype.PropertyName)
    {
      this.PrototypeCache = prototype.PrototypeCache;
    }

    protected CachedModelMetadata(
      CachedDataAnnotationsModelMetadataProvider provider,
      Type containerType,
      Type modelType,
      string propertyName,
      TPrototypeCache prototypeCache)
      : base((ModelMetadataProvider) provider, containerType, (Func<object>) null, modelType, propertyName)
    {
      this.PrototypeCache = prototypeCache;
    }

    public override sealed bool ConvertEmptyStringToNull
    {
      get
      {
        return CachedModelMetadata<TPrototypeCache>.CacheOrCompute<bool>(new Func<bool>(this.ComputeConvertEmptyStringToNull), ref this._convertEmptyStringToNull, ref this._convertEmptyStringToNullComputed);
      }
      set
      {
        this._convertEmptyStringToNull = value;
        this._convertEmptyStringToNullComputed = true;
      }
    }

    public override sealed string DataTypeName
    {
      get
      {
        return CachedModelMetadata<TPrototypeCache>.CacheOrCompute<string>(new Func<string>(this.ComputeDataTypeName), ref this._dataTypeName, ref this._dataTypeNameComputed);
      }
      set
      {
        this._dataTypeName = value;
        this._dataTypeNameComputed = true;
      }
    }

    public override sealed string Description
    {
      get
      {
        return CachedModelMetadata<TPrototypeCache>.CacheOrCompute<string>(new Func<string>(this.ComputeDescription), ref this._description, ref this._descriptionComputed);
      }
      set
      {
        this._description = value;
        this._descriptionComputed = true;
      }
    }

    public override sealed string DisplayFormatString
    {
      get
      {
        return CachedModelMetadata<TPrototypeCache>.CacheOrCompute<string>(new Func<string>(this.ComputeDisplayFormatString), ref this._displayFormatString, ref this._displayFormatStringComputed);
      }
      set
      {
        this._displayFormatString = value;
        this._displayFormatStringComputed = true;
      }
    }

    public override sealed string DisplayName
    {
      get
      {
        return CachedModelMetadata<TPrototypeCache>.CacheOrCompute<string>(new Func<string>(this.ComputeDisplayName), ref this._displayName, ref this._displayNameComputed);
      }
      set
      {
        this._displayName = value;
        this._displayNameComputed = true;
      }
    }

    public override sealed string EditFormatString
    {
      get
      {
        return CachedModelMetadata<TPrototypeCache>.CacheOrCompute<string>(new Func<string>(this.ComputeEditFormatString), ref this._editFormatString, ref this._editFormatStringComputed);
      }
      set
      {
        this._editFormatString = value;
        this._editFormatStringComputed = true;
      }
    }

    public override sealed bool HideSurroundingHtml
    {
      get
      {
        return CachedModelMetadata<TPrototypeCache>.CacheOrCompute<bool>(new Func<bool>(this.ComputeHideSurroundingHtml), ref this._hideSurroundingHtml, ref this._hideSurroundingHtmlComputed);
      }
      set
      {
        this._hideSurroundingHtml = value;
        this._hideSurroundingHtmlComputed = true;
      }
    }

    public override sealed bool HtmlEncode
    {
      get
      {
        if (!this._htmlEncodeComputed)
        {
          this._htmlEncode = this.ComputeHtmlEncode();
          this._htmlEncodeComputed = true;
        }
        return this._htmlEncode;
      }
      set
      {
        this._htmlEncode = value;
        this._htmlEncodeComputed = true;
      }
    }

    public override sealed bool IsReadOnly
    {
      get
      {
        return CachedModelMetadata<TPrototypeCache>.CacheOrCompute<bool>(new Func<bool>(this.ComputeIsReadOnly), ref this._isReadOnly, ref this._isReadOnlyComputed);
      }
      set
      {
        this._isReadOnly = value;
        this._isReadOnlyComputed = true;
      }
    }

    public override sealed bool IsRequired
    {
      get
      {
        return CachedModelMetadata<TPrototypeCache>.CacheOrCompute<bool>(new Func<bool>(this.ComputeIsRequired), ref this._isRequired, ref this._isRequiredComputed);
      }
      set
      {
        this._isRequired = value;
        this._isRequiredComputed = true;
      }
    }

    public override sealed string NullDisplayText
    {
      get
      {
        return CachedModelMetadata<TPrototypeCache>.CacheOrCompute<string>(new Func<string>(this.ComputeNullDisplayText), ref this._nullDisplayText, ref this._nullDisplayTextComputed);
      }
      set
      {
        this._nullDisplayText = value;
        this._nullDisplayTextComputed = true;
      }
    }

    public override sealed int Order
    {
      get
      {
        return CachedModelMetadata<TPrototypeCache>.CacheOrCompute<int>(new Func<int>(this.ComputeOrder), ref this._order, ref this._orderComputed);
      }
      set
      {
        this._order = value;
        this._orderComputed = true;
      }
    }

    protected TPrototypeCache PrototypeCache { get; set; }

    public override sealed string ShortDisplayName
    {
      get
      {
        return CachedModelMetadata<TPrototypeCache>.CacheOrCompute<string>(new Func<string>(this.ComputeShortDisplayName), ref this._shortDisplayName, ref this._shortDisplayNameComputed);
      }
      set
      {
        this._shortDisplayName = value;
        this._shortDisplayNameComputed = true;
      }
    }

    public override sealed bool ShowForDisplay
    {
      get
      {
        return CachedModelMetadata<TPrototypeCache>.CacheOrCompute<bool>(new Func<bool>(this.ComputeShowForDisplay), ref this._showForDisplay, ref this._showForDisplayComputed);
      }
      set
      {
        this._showForDisplay = value;
        this._showForDisplayComputed = true;
      }
    }

    public override sealed bool ShowForEdit
    {
      get
      {
        return CachedModelMetadata<TPrototypeCache>.CacheOrCompute<bool>(new Func<bool>(this.ComputeShowForEdit), ref this._showForEdit, ref this._showForEditComputed);
      }
      set
      {
        this._showForEdit = value;
        this._showForEditComputed = true;
      }
    }

    public override sealed string SimpleDisplayText
    {
      get => base.SimpleDisplayText;
      set => base.SimpleDisplayText = value;
    }

    public override sealed string TemplateHint
    {
      get
      {
        return CachedModelMetadata<TPrototypeCache>.CacheOrCompute<string>(new Func<string>(this.ComputeTemplateHint), ref this._templateHint, ref this._templateHintComputed);
      }
      set
      {
        this._templateHint = value;
        this._templateHintComputed = true;
      }
    }

    public override sealed string Watermark
    {
      get
      {
        return CachedModelMetadata<TPrototypeCache>.CacheOrCompute<string>(new Func<string>(this.ComputeWatermark), ref this._watermark, ref this._watermarkComputed);
      }
      set
      {
        this._watermark = value;
        this._watermarkComputed = true;
      }
    }

    private static TResult CacheOrCompute<TResult>(
      Func<TResult> computeThunk,
      ref TResult value,
      ref bool computed)
    {
      if (!computed)
      {
        value = computeThunk();
        computed = true;
      }
      return value;
    }

    protected virtual bool ComputeConvertEmptyStringToNull() => base.ConvertEmptyStringToNull;

    protected virtual string ComputeDataTypeName() => base.DataTypeName;

    protected virtual string ComputeDescription() => base.Description;

    protected virtual string ComputeDisplayFormatString() => base.DisplayFormatString;

    protected virtual string ComputeDisplayName() => base.DisplayName;

    protected virtual string ComputeEditFormatString() => base.EditFormatString;

    protected virtual bool ComputeHideSurroundingHtml() => base.HideSurroundingHtml;

    protected virtual bool ComputeHtmlEncode() => base.HtmlEncode;

    protected virtual bool ComputeIsReadOnly() => base.IsReadOnly;

    protected virtual bool ComputeIsRequired() => base.IsRequired;

    protected virtual string ComputeNullDisplayText() => base.NullDisplayText;

    protected virtual int ComputeOrder() => base.Order;

    protected virtual string ComputeShortDisplayName() => base.ShortDisplayName;

    protected virtual bool ComputeShowForDisplay() => base.ShowForDisplay;

    protected virtual bool ComputeShowForEdit() => base.ShowForEdit;

    protected virtual string ComputeSimpleDisplayText() => base.GetSimpleDisplayText();

    protected virtual string ComputeTemplateHint() => base.TemplateHint;

    protected virtual string ComputeWatermark() => base.Watermark;

    protected override sealed string GetSimpleDisplayText() => this.ComputeSimpleDisplayText();
  }
}
