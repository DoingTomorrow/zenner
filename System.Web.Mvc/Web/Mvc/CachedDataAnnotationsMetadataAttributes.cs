// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.CachedDataAnnotationsMetadataAttributes
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
  public class CachedDataAnnotationsMetadataAttributes
  {
    public CachedDataAnnotationsMetadataAttributes(Attribute[] attributes)
    {
      this.DataType = attributes.OfType<DataTypeAttribute>().FirstOrDefault<DataTypeAttribute>();
      this.Display = attributes.OfType<DisplayAttribute>().FirstOrDefault<DisplayAttribute>();
      this.DisplayColumn = attributes.OfType<DisplayColumnAttribute>().FirstOrDefault<DisplayColumnAttribute>();
      this.DisplayFormat = attributes.OfType<DisplayFormatAttribute>().FirstOrDefault<DisplayFormatAttribute>();
      this.DisplayName = attributes.OfType<DisplayNameAttribute>().FirstOrDefault<DisplayNameAttribute>();
      this.Editable = attributes.OfType<EditableAttribute>().FirstOrDefault<EditableAttribute>();
      this.HiddenInput = attributes.OfType<HiddenInputAttribute>().FirstOrDefault<HiddenInputAttribute>();
      this.ReadOnly = attributes.OfType<ReadOnlyAttribute>().FirstOrDefault<ReadOnlyAttribute>();
      this.Required = attributes.OfType<RequiredAttribute>().FirstOrDefault<RequiredAttribute>();
      this.ScaffoldColumn = attributes.OfType<ScaffoldColumnAttribute>().FirstOrDefault<ScaffoldColumnAttribute>();
      IEnumerable<UIHintAttribute> source = attributes.OfType<UIHintAttribute>();
      this.UIHint = source.FirstOrDefault<UIHintAttribute>((Func<UIHintAttribute, bool>) (a => string.Equals(a.PresentationLayer, "MVC", StringComparison.OrdinalIgnoreCase))) ?? source.FirstOrDefault<UIHintAttribute>((Func<UIHintAttribute, bool>) (a => string.IsNullOrEmpty(a.PresentationLayer)));
      if (this.DisplayFormat != null || this.DataType == null)
        return;
      this.DisplayFormat = this.DataType.DisplayFormat;
    }

    public DataTypeAttribute DataType { get; protected set; }

    public DisplayAttribute Display { get; protected set; }

    public DisplayColumnAttribute DisplayColumn { get; protected set; }

    public DisplayFormatAttribute DisplayFormat { get; protected set; }

    public DisplayNameAttribute DisplayName { get; protected set; }

    public EditableAttribute Editable { get; protected set; }

    public HiddenInputAttribute HiddenInput { get; protected set; }

    public ReadOnlyAttribute ReadOnly { get; protected set; }

    public RequiredAttribute Required { get; protected set; }

    public ScaffoldColumnAttribute ScaffoldColumn { get; protected set; }

    public UIHintAttribute UIHint { get; protected set; }
  }
}
