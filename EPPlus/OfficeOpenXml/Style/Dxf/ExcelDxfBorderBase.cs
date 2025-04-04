// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.Dxf.ExcelDxfBorderBase
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.Style.Dxf
{
  public class ExcelDxfBorderBase : DxfStyleBase<ExcelDxfBorderBase>
  {
    internal ExcelDxfBorderBase(ExcelStyles styles)
      : base(styles)
    {
      this.Left = new ExcelDxfBorderItem(this._styles);
      this.Right = new ExcelDxfBorderItem(this._styles);
      this.Top = new ExcelDxfBorderItem(this._styles);
      this.Bottom = new ExcelDxfBorderItem(this._styles);
    }

    public ExcelDxfBorderItem Left { get; internal set; }

    public ExcelDxfBorderItem Right { get; internal set; }

    public ExcelDxfBorderItem Top { get; internal set; }

    public ExcelDxfBorderItem Bottom { get; internal set; }

    protected internal override string Id
    {
      get => this.Top.Id + this.Bottom.Id + this.Left.Id + this.Right.Id;
    }

    protected internal override void CreateNodes(XmlHelper helper, string path)
    {
      this.Left.CreateNodes(helper, path + "/d:left");
      this.Right.CreateNodes(helper, path + "/d:right");
      this.Top.CreateNodes(helper, path + "/d:top");
      this.Bottom.CreateNodes(helper, path + "/d:bottom");
    }

    protected internal override bool HasValue
    {
      get => this.Left.HasValue || this.Right.HasValue || this.Top.HasValue || this.Bottom.HasValue;
    }

    protected internal override ExcelDxfBorderBase Clone()
    {
      return new ExcelDxfBorderBase(this._styles)
      {
        Bottom = this.Bottom.Clone(),
        Top = this.Top.Clone(),
        Left = this.Left.Clone(),
        Right = this.Right.Clone()
      };
    }
  }
}
