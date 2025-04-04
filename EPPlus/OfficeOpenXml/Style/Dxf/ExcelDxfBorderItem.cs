// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.Dxf.ExcelDxfBorderItem
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.Style.Dxf
{
  public class ExcelDxfBorderItem : DxfStyleBase<ExcelDxfBorderItem>
  {
    internal ExcelDxfBorderItem(ExcelStyles styles)
      : base(styles)
    {
      this.Color = new ExcelDxfColor(styles);
    }

    public ExcelBorderStyle? Style { get; set; }

    public ExcelDxfColor Color { get; internal set; }

    protected internal override string Id
    {
      get
      {
        return this.GetAsString((object) this.Style) + "|" + (this.Color == null ? "" : this.Color.Id);
      }
    }

    protected internal override void CreateNodes(XmlHelper helper, string path)
    {
      this.SetValueEnum(helper, path + "/@style", (Enum) (ValueType) this.Style);
      this.SetValueColor(helper, path + "/d:color", this.Color);
    }

    protected internal override bool HasValue => this.Style.HasValue || this.Color.HasValue;

    protected internal override ExcelDxfBorderItem Clone()
    {
      return new ExcelDxfBorderItem(this._styles)
      {
        Style = this.Style,
        Color = this.Color
      };
    }
  }
}
