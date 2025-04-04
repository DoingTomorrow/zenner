// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.Dxf.ExcelDxfColor
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.Style.Dxf
{
  public class ExcelDxfColor(ExcelStyles styles) : DxfStyleBase<ExcelDxfColor>(styles)
  {
    public int? Theme { get; set; }

    public int? Index { get; set; }

    public bool? Auto { get; set; }

    public double? Tint { get; set; }

    public System.Drawing.Color? Color { get; set; }

    protected internal override string Id
    {
      get
      {
        return this.GetAsString((object) this.Theme) + "|" + this.GetAsString((object) this.Index) + "|" + this.GetAsString((object) this.Auto) + "|" + this.GetAsString((object) this.Tint) + "|" + this.GetAsString(!this.Color.HasValue ? (object) "" : (object) this.Color.Value.ToArgb().ToString("x"));
      }
    }

    protected internal override ExcelDxfColor Clone()
    {
      return new ExcelDxfColor(this._styles)
      {
        Theme = this.Theme,
        Index = this.Index,
        Color = this.Color,
        Auto = this.Auto,
        Tint = this.Tint
      };
    }

    protected internal override bool HasValue
    {
      get
      {
        return this.Theme.HasValue || this.Index.HasValue || this.Auto.HasValue || this.Tint.HasValue || this.Color.HasValue;
      }
    }

    protected internal override void CreateNodes(XmlHelper helper, string path)
    {
      throw new NotImplementedException();
    }
  }
}
