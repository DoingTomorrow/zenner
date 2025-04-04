// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.Dxf.ExcelDxfFill
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.Style.Dxf
{
  public class ExcelDxfFill : DxfStyleBase<ExcelDxfFill>
  {
    public ExcelDxfFill(ExcelStyles styles)
      : base(styles)
    {
      this.PatternColor = new ExcelDxfColor(styles);
      this.BackgroundColor = new ExcelDxfColor(styles);
    }

    public ExcelFillStyle? PatternType { get; set; }

    public ExcelDxfColor PatternColor { get; internal set; }

    public ExcelDxfColor BackgroundColor { get; internal set; }

    protected internal override string Id
    {
      get
      {
        return this.GetAsString((object) this.PatternType) + "|" + (this.PatternColor == null ? "" : this.PatternColor.Id) + "|" + (this.BackgroundColor == null ? "" : this.BackgroundColor.Id);
      }
    }

    protected internal override void CreateNodes(XmlHelper helper, string path)
    {
      helper.CreateNode(path);
      this.SetValueEnum(helper, path + "/d:patternFill/@patternType", (Enum) (ValueType) this.PatternType);
      this.SetValueColor(helper, path + "/d:patternFill/d:fgColor", this.PatternColor);
      this.SetValueColor(helper, path + "/d:patternFill/d:bgColor", this.BackgroundColor);
    }

    protected internal override bool HasValue
    {
      get
      {
        return this.PatternType.HasValue || this.PatternColor.HasValue || this.BackgroundColor.HasValue;
      }
    }

    protected internal override ExcelDxfFill Clone()
    {
      return new ExcelDxfFill(this._styles)
      {
        PatternType = this.PatternType,
        PatternColor = this.PatternColor.Clone(),
        BackgroundColor = this.BackgroundColor.Clone()
      };
    }
  }
}
