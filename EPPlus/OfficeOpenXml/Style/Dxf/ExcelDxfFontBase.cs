// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.Dxf.ExcelDxfFontBase
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.Style.Dxf
{
  public class ExcelDxfFontBase : DxfStyleBase<ExcelDxfFontBase>
  {
    public ExcelDxfFontBase(ExcelStyles styles)
      : base(styles)
    {
      this.Color = new ExcelDxfColor(styles);
    }

    public bool? Bold { get; set; }

    public bool? Italic { get; set; }

    public bool? Strike { get; set; }

    public ExcelDxfColor Color { get; set; }

    public ExcelUnderLineType? Underline { get; set; }

    protected internal override string Id
    {
      get
      {
        return this.GetAsString((object) this.Bold) + "|" + this.GetAsString((object) this.Italic) + "|" + this.GetAsString((object) this.Strike) + "|" + (this.Color == null ? "" : this.Color.Id) + "|" + this.GetAsString((object) this.Underline);
      }
    }

    protected internal override void CreateNodes(XmlHelper helper, string path)
    {
      helper.CreateNode(path);
      this.SetValueBool(helper, path + "/d:b/@val", this.Bold);
      this.SetValueBool(helper, path + "/d:i/@val", this.Italic);
      this.SetValueBool(helper, path + "/d:strike", this.Strike);
      this.SetValue(helper, path + "/d:u/@val", (object) this.Underline);
      this.SetValueColor(helper, path + "/d:color", this.Color);
    }

    protected internal override bool HasValue
    {
      get
      {
        return this.Bold.HasValue || this.Italic.HasValue || this.Strike.HasValue || this.Underline.HasValue || this.Color.HasValue;
      }
    }

    protected internal override ExcelDxfFontBase Clone()
    {
      return new ExcelDxfFontBase(this._styles)
      {
        Bold = this.Bold,
        Color = this.Color.Clone(),
        Italic = this.Italic,
        Strike = this.Strike,
        Underline = this.Underline
      };
    }
  }
}
