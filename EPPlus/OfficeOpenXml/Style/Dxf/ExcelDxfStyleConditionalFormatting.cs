// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.Dxf.ExcelDxfStyleConditionalFormatting
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Drawing;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Style.Dxf
{
  public class ExcelDxfStyleConditionalFormatting : DxfStyleBase<ExcelDxfStyleConditionalFormatting>
  {
    private XmlHelperInstance _helper;

    internal ExcelDxfStyleConditionalFormatting(
      XmlNamespaceManager nameSpaceManager,
      XmlNode topNode,
      ExcelStyles styles)
      : base(styles)
    {
      this.NumberFormat = new ExcelDxfNumberFormat(this._styles);
      this.Font = new ExcelDxfFontBase(this._styles);
      this.Border = new ExcelDxfBorderBase(this._styles);
      this.Fill = new ExcelDxfFill(this._styles);
      if (topNode != null)
      {
        this._helper = new XmlHelperInstance(nameSpaceManager, topNode);
        this.NumberFormat.NumFmtID = this._helper.GetXmlNodeInt("d:numFmt/@numFmtId");
        this.NumberFormat.Format = this._helper.GetXmlNodeString("d:numFmt/@formatCode");
        if (this.NumberFormat.NumFmtID < 164 && string.IsNullOrEmpty(this.NumberFormat.Format))
          this.NumberFormat.Format = ExcelNumberFormat.GetFromBuildInFromID(this.NumberFormat.NumFmtID);
        this.Font.Bold = this._helper.GetXmlNodeBoolNullable("d:font/d:b/@val");
        this.Font.Italic = this._helper.GetXmlNodeBoolNullable("d:font/d:i/@val");
        this.Font.Strike = this._helper.GetXmlNodeBoolNullable("d:font/d:strike");
        this.Font.Underline = this.GetUnderLineEnum(this._helper.GetXmlNodeString("d:font/d:u/@val"));
        this.Font.Color = this.GetColor(this._helper, "d:font/d:color");
        this.Border.Left = this.GetBorderItem(this._helper, "d:border/d:left");
        this.Border.Right = this.GetBorderItem(this._helper, "d:border/d:right");
        this.Border.Bottom = this.GetBorderItem(this._helper, "d:border/d:bottom");
        this.Border.Top = this.GetBorderItem(this._helper, "d:border/d:top");
        this.Fill.PatternType = new ExcelFillStyle?(this.GetPatternTypeEnum(this._helper.GetXmlNodeString("d:fill/d:patternFill/@patternType")));
        this.Fill.BackgroundColor = this.GetColor(this._helper, "d:fill/d:patternFill/d:bgColor/");
        this.Fill.PatternColor = this.GetColor(this._helper, "d:fill/d:patternFill/d:fgColor/");
      }
      else
        this._helper = new XmlHelperInstance(nameSpaceManager);
      this._helper.SchemaNodeOrder = new string[4]
      {
        "font",
        "numFmt",
        "fill",
        "border"
      };
    }

    private ExcelDxfBorderItem GetBorderItem(XmlHelperInstance helper, string path)
    {
      return new ExcelDxfBorderItem(this._styles)
      {
        Style = new ExcelBorderStyle?(this.GetBorderStyleEnum(helper.GetXmlNodeString(path + "/@style"))),
        Color = this.GetColor(helper, path + "/d:color")
      };
    }

    private ExcelBorderStyle GetBorderStyleEnum(string style)
    {
      if (style == "")
        return ExcelBorderStyle.None;
      string str = style.Substring(0, 1).ToUpper() + style.Substring(1, style.Length - 1);
      try
      {
        return (ExcelBorderStyle) Enum.Parse(typeof (ExcelBorderStyle), str);
      }
      catch
      {
        return ExcelBorderStyle.None;
      }
    }

    private ExcelFillStyle GetPatternTypeEnum(string patternType)
    {
      if (patternType == "")
        return ExcelFillStyle.None;
      patternType = patternType.Substring(0, 1).ToUpper() + patternType.Substring(1, patternType.Length - 1);
      try
      {
        return (ExcelFillStyle) Enum.Parse(typeof (ExcelFillStyle), patternType);
      }
      catch
      {
        return ExcelFillStyle.None;
      }
    }

    private ExcelDxfColor GetColor(XmlHelperInstance helper, string path)
    {
      ExcelDxfColor color = new ExcelDxfColor(this._styles);
      color.Theme = helper.GetXmlNodeIntNull(path + "/@theme");
      color.Index = helper.GetXmlNodeIntNull(path + "/@indexed");
      string xmlNodeString = helper.GetXmlNodeString(path + "/@rgb");
      if (xmlNodeString != "")
        color.Color = new Color?(Color.FromArgb(int.Parse(xmlNodeString.Substring(0, 2), NumberStyles.AllowHexSpecifier), int.Parse(xmlNodeString.Substring(2, 2), NumberStyles.AllowHexSpecifier), int.Parse(xmlNodeString.Substring(4, 2), NumberStyles.AllowHexSpecifier), int.Parse(xmlNodeString.Substring(6, 2), NumberStyles.AllowHexSpecifier)));
      color.Auto = helper.GetXmlNodeBoolNullable(path + "/@auto");
      color.Tint = helper.GetXmlNodeDoubleNull(path + "/@tint");
      return color;
    }

    private ExcelUnderLineType? GetUnderLineEnum(string value)
    {
      switch (value.ToLower())
      {
        case "single":
          return new ExcelUnderLineType?(ExcelUnderLineType.Single);
        case "double":
          return new ExcelUnderLineType?(ExcelUnderLineType.Double);
        case "singleaccounting":
          return new ExcelUnderLineType?(ExcelUnderLineType.SingleAccounting);
        case "doubleaccounting":
          return new ExcelUnderLineType?(ExcelUnderLineType.DoubleAccounting);
        default:
          return new ExcelUnderLineType?();
      }
    }

    internal int DxfId { get; set; }

    public ExcelDxfFontBase Font { get; set; }

    public ExcelDxfNumberFormat NumberFormat { get; set; }

    public ExcelDxfFill Fill { get; set; }

    public ExcelDxfBorderBase Border { get; set; }

    protected internal override string Id
    {
      get
      {
        return this.NumberFormat.Id + this.Font.Id + this.Border.Id + this.Fill.Id + (this.AllowChange ? "" : this.DxfId.ToString());
      }
    }

    protected internal override ExcelDxfStyleConditionalFormatting Clone()
    {
      return new ExcelDxfStyleConditionalFormatting(this._helper.NameSpaceManager, (XmlNode) null, this._styles)
      {
        Font = this.Font.Clone(),
        NumberFormat = this.NumberFormat.Clone(),
        Fill = this.Fill.Clone(),
        Border = this.Border.Clone()
      };
    }

    protected internal override void CreateNodes(XmlHelper helper, string path)
    {
      if (this.Font.HasValue)
        this.Font.CreateNodes(helper, "d:font");
      if (this.NumberFormat.HasValue)
        this.NumberFormat.CreateNodes(helper, "d:numFmt");
      if (this.Fill.HasValue)
        this.Fill.CreateNodes(helper, "d:fill");
      if (!this.Border.HasValue)
        return;
      this.Border.CreateNodes(helper, "d:border");
    }

    protected internal override bool HasValue
    {
      get
      {
        return this.Font.HasValue || this.NumberFormat.HasValue || this.Fill.HasValue || this.Border.HasValue;
      }
    }
  }
}
