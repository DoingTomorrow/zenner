// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.CachedDataAnnotationsModelMetadata
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class CachedDataAnnotationsModelMetadata : 
    CachedModelMetadata<CachedDataAnnotationsMetadataAttributes>
  {
    public CachedDataAnnotationsModelMetadata(
      CachedDataAnnotationsModelMetadata prototype,
      Func<object> modelAccessor)
      : base((CachedModelMetadata<CachedDataAnnotationsMetadataAttributes>) prototype, modelAccessor)
    {
    }

    public CachedDataAnnotationsModelMetadata(
      CachedDataAnnotationsModelMetadataProvider provider,
      Type containerType,
      Type modelType,
      string propertyName,
      IEnumerable<Attribute> attributes)
      : base(provider, containerType, modelType, propertyName, new CachedDataAnnotationsMetadataAttributes(attributes.ToArray<Attribute>()))
    {
    }

    protected override bool ComputeConvertEmptyStringToNull()
    {
      return this.PrototypeCache.DisplayFormat == null ? base.ComputeConvertEmptyStringToNull() : this.PrototypeCache.DisplayFormat.ConvertEmptyStringToNull;
    }

    protected override string ComputeDataTypeName()
    {
      if (this.PrototypeCache.DataType != null)
        return this.PrototypeCache.DataType.ToDataTypeName();
      return this.PrototypeCache.DisplayFormat != null && !this.PrototypeCache.DisplayFormat.HtmlEncode ? DataTypeUtil.HtmlTypeName : base.ComputeDataTypeName();
    }

    protected override string ComputeDescription()
    {
      return this.PrototypeCache.Display == null ? base.ComputeDescription() : this.PrototypeCache.Display.GetDescription();
    }

    protected override string ComputeDisplayFormatString()
    {
      return this.PrototypeCache.DisplayFormat == null ? base.ComputeDisplayFormatString() : this.PrototypeCache.DisplayFormat.DataFormatString;
    }

    protected override string ComputeDisplayName()
    {
      string str = (string) null;
      if (this.PrototypeCache.Display != null)
        str = this.PrototypeCache.Display.GetName();
      if (str == null && this.PrototypeCache.DisplayName != null)
        str = this.PrototypeCache.DisplayName.DisplayName;
      return str ?? base.ComputeDisplayName();
    }

    protected override string ComputeEditFormatString()
    {
      return this.PrototypeCache.DisplayFormat != null && this.PrototypeCache.DisplayFormat.ApplyFormatInEditMode ? this.PrototypeCache.DisplayFormat.DataFormatString : base.ComputeEditFormatString();
    }

    protected override bool ComputeHideSurroundingHtml()
    {
      return this.PrototypeCache.HiddenInput == null ? base.ComputeHideSurroundingHtml() : !this.PrototypeCache.HiddenInput.DisplayValue;
    }

    protected override bool ComputeHtmlEncode()
    {
      return this.PrototypeCache.DisplayFormat == null ? base.ComputeHtmlEncode() : this.PrototypeCache.DisplayFormat.HtmlEncode;
    }

    protected override bool ComputeIsReadOnly()
    {
      if (this.PrototypeCache.Editable != null)
        return !this.PrototypeCache.Editable.AllowEdit;
      return this.PrototypeCache.ReadOnly != null ? this.PrototypeCache.ReadOnly.IsReadOnly : base.ComputeIsReadOnly();
    }

    protected override bool ComputeIsRequired()
    {
      return this.PrototypeCache.Required != null || base.ComputeIsRequired();
    }

    protected override string ComputeNullDisplayText()
    {
      return this.PrototypeCache.DisplayFormat == null ? base.ComputeNullDisplayText() : this.PrototypeCache.DisplayFormat.NullDisplayText;
    }

    protected override int ComputeOrder()
    {
      int? nullable = new int?();
      if (this.PrototypeCache.Display != null)
        nullable = this.PrototypeCache.Display.GetOrder();
      return nullable ?? base.ComputeOrder();
    }

    protected override string ComputeShortDisplayName()
    {
      return this.PrototypeCache.Display == null ? base.ComputeShortDisplayName() : this.PrototypeCache.Display.GetShortName();
    }

    protected override bool ComputeShowForDisplay()
    {
      return this.PrototypeCache.ScaffoldColumn == null ? base.ComputeShowForDisplay() : this.PrototypeCache.ScaffoldColumn.Scaffold;
    }

    protected override bool ComputeShowForEdit()
    {
      return this.PrototypeCache.ScaffoldColumn == null ? base.ComputeShowForEdit() : this.PrototypeCache.ScaffoldColumn.Scaffold;
    }

    protected override string ComputeSimpleDisplayText()
    {
      if (this.Model != null && this.PrototypeCache.DisplayColumn != null && !string.IsNullOrEmpty(this.PrototypeCache.DisplayColumn.DisplayColumn))
      {
        PropertyInfo property = this.ModelType.GetProperty(this.PrototypeCache.DisplayColumn.DisplayColumn, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
        CachedDataAnnotationsModelMetadata.ValidateDisplayColumnAttribute(this.PrototypeCache.DisplayColumn, property, this.ModelType);
        object obj = property.GetValue(this.Model, new object[0]);
        if (obj != null)
          return obj.ToString();
      }
      return base.ComputeSimpleDisplayText();
    }

    protected override string ComputeTemplateHint()
    {
      if (this.PrototypeCache.UIHint != null)
        return this.PrototypeCache.UIHint.UIHint;
      return this.PrototypeCache.HiddenInput != null ? "HiddenInput" : base.ComputeTemplateHint();
    }

    protected override string ComputeWatermark()
    {
      return this.PrototypeCache.Display == null ? base.ComputeWatermark() : this.PrototypeCache.Display.GetPrompt();
    }

    private static void ValidateDisplayColumnAttribute(
      DisplayColumnAttribute displayColumnAttribute,
      PropertyInfo displayColumnProperty,
      Type modelType)
    {
      if (displayColumnProperty == (PropertyInfo) null)
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.DataAnnotationsModelMetadataProvider_UnknownProperty, new object[2]
        {
          (object) modelType.FullName,
          (object) displayColumnAttribute.DisplayColumn
        }));
      if (displayColumnProperty.GetGetMethod() == (MethodInfo) null)
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.DataAnnotationsModelMetadataProvider_UnreadableProperty, new object[2]
        {
          (object) modelType.FullName,
          (object) displayColumnAttribute.DisplayColumn
        }));
    }
  }
}
