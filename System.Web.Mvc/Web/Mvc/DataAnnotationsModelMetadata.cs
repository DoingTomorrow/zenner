// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.DataAnnotationsModelMetadata
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class DataAnnotationsModelMetadata : ModelMetadata
  {
    private DisplayColumnAttribute _displayColumnAttribute;

    public DataAnnotationsModelMetadata(
      DataAnnotationsModelMetadataProvider provider,
      Type containerType,
      Func<object> modelAccessor,
      Type modelType,
      string propertyName,
      DisplayColumnAttribute displayColumnAttribute)
      : base((ModelMetadataProvider) provider, containerType, modelAccessor, modelType, propertyName)
    {
      this._displayColumnAttribute = displayColumnAttribute;
    }

    protected override string GetSimpleDisplayText()
    {
      if (this.Model != null && this._displayColumnAttribute != null && !string.IsNullOrEmpty(this._displayColumnAttribute.DisplayColumn))
      {
        PropertyInfo property = this.ModelType.GetProperty(this._displayColumnAttribute.DisplayColumn, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
        DataAnnotationsModelMetadata.ValidateDisplayColumnAttribute(this._displayColumnAttribute, property, this.ModelType);
        object obj = property.GetValue(this.Model, new object[0]);
        if (obj != null)
          return obj.ToString();
      }
      return base.GetSimpleDisplayText();
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
