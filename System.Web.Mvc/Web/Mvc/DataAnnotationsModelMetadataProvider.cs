// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.DataAnnotationsModelMetadataProvider
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  public class DataAnnotationsModelMetadataProvider : AssociatedMetadataProvider
  {
    protected override ModelMetadata CreateMetadata(
      IEnumerable<Attribute> attributes,
      Type containerType,
      Func<object> modelAccessor,
      Type modelType,
      string propertyName)
    {
      List<Attribute> source1 = new List<Attribute>(attributes);
      DisplayColumnAttribute displayColumnAttribute = source1.OfType<DisplayColumnAttribute>().FirstOrDefault<DisplayColumnAttribute>();
      DataAnnotationsModelMetadata metadata = new DataAnnotationsModelMetadata(this, containerType, modelAccessor, modelType, propertyName, displayColumnAttribute);
      HiddenInputAttribute hiddenInputAttribute = source1.OfType<HiddenInputAttribute>().FirstOrDefault<HiddenInputAttribute>();
      if (hiddenInputAttribute != null)
      {
        metadata.TemplateHint = "HiddenInput";
        metadata.HideSurroundingHtml = !hiddenInputAttribute.DisplayValue;
      }
      IEnumerable<UIHintAttribute> source2 = source1.OfType<UIHintAttribute>();
      UIHintAttribute uiHintAttribute = source2.FirstOrDefault<UIHintAttribute>((Func<UIHintAttribute, bool>) (a => string.Equals(a.PresentationLayer, "MVC", StringComparison.OrdinalIgnoreCase))) ?? source2.FirstOrDefault<UIHintAttribute>((Func<UIHintAttribute, bool>) (a => string.IsNullOrEmpty(a.PresentationLayer)));
      if (uiHintAttribute != null)
        metadata.TemplateHint = uiHintAttribute.UIHint;
      DataTypeAttribute attribute = source1.OfType<DataTypeAttribute>().FirstOrDefault<DataTypeAttribute>();
      if (attribute != null)
        metadata.DataTypeName = attribute.ToDataTypeName();
      EditableAttribute editableAttribute = attributes.OfType<EditableAttribute>().FirstOrDefault<EditableAttribute>();
      if (editableAttribute != null)
      {
        metadata.IsReadOnly = !editableAttribute.AllowEdit;
      }
      else
      {
        ReadOnlyAttribute readOnlyAttribute = source1.OfType<ReadOnlyAttribute>().FirstOrDefault<ReadOnlyAttribute>();
        if (readOnlyAttribute != null)
          metadata.IsReadOnly = readOnlyAttribute.IsReadOnly;
      }
      DisplayFormatAttribute displayFormatAttribute = source1.OfType<DisplayFormatAttribute>().FirstOrDefault<DisplayFormatAttribute>();
      if (displayFormatAttribute == null && attribute != null)
        displayFormatAttribute = attribute.DisplayFormat;
      if (displayFormatAttribute != null)
      {
        metadata.NullDisplayText = displayFormatAttribute.NullDisplayText;
        metadata.DisplayFormatString = displayFormatAttribute.DataFormatString;
        metadata.ConvertEmptyStringToNull = displayFormatAttribute.ConvertEmptyStringToNull;
        metadata.HtmlEncode = displayFormatAttribute.HtmlEncode;
        if (displayFormatAttribute.ApplyFormatInEditMode)
          metadata.EditFormatString = displayFormatAttribute.DataFormatString;
        if (!displayFormatAttribute.HtmlEncode && string.IsNullOrWhiteSpace(metadata.DataTypeName))
          metadata.DataTypeName = DataTypeUtil.HtmlTypeName;
      }
      ScaffoldColumnAttribute scaffoldColumnAttribute = source1.OfType<ScaffoldColumnAttribute>().FirstOrDefault<ScaffoldColumnAttribute>();
      if (scaffoldColumnAttribute != null)
        metadata.ShowForDisplay = metadata.ShowForEdit = scaffoldColumnAttribute.Scaffold;
      DisplayAttribute displayAttribute = attributes.OfType<DisplayAttribute>().FirstOrDefault<DisplayAttribute>();
      string str = (string) null;
      if (displayAttribute != null)
      {
        metadata.Description = displayAttribute.GetDescription();
        metadata.ShortDisplayName = displayAttribute.GetShortName();
        metadata.Watermark = displayAttribute.GetPrompt();
        metadata.Order = displayAttribute.GetOrder() ?? 10000;
        str = displayAttribute.GetName();
      }
      if (str != null)
      {
        metadata.DisplayName = str;
      }
      else
      {
        DisplayNameAttribute displayNameAttribute = source1.OfType<DisplayNameAttribute>().FirstOrDefault<DisplayNameAttribute>();
        if (displayNameAttribute != null)
          metadata.DisplayName = displayNameAttribute.DisplayName;
      }
      if (source1.OfType<RequiredAttribute>().FirstOrDefault<RequiredAttribute>() != null)
        metadata.IsRequired = true;
      return (ModelMetadata) metadata;
    }
  }
}
