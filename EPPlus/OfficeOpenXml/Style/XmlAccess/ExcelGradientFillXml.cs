// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.XmlAccess.ExcelGradientFillXml
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Style.XmlAccess
{
  public sealed class ExcelGradientFillXml : ExcelFillXml
  {
    private const string _typePath = "d:gradientFill/@type";
    private const string _degreePath = "d:gradientFill/@degree";
    private const string _gradientColor1Path = "d:gradientFill/d:stop[@position=\"0\"]/d:color";
    private const string _gradientColor2Path = "d:gradientFill/d:stop[@position=\"1\"]/d:color";
    private const string _bottomPath = "d:gradientFill/@bottom";
    private const string _topPath = "d:gradientFill/@top";
    private const string _leftPath = "d:gradientFill/@left";
    private const string _rightPath = "d:gradientFill/@right";

    internal ExcelGradientFillXml(XmlNamespaceManager nameSpaceManager)
      : base(nameSpaceManager)
    {
      this.GradientColor1 = new ExcelColorXml(nameSpaceManager);
      this.GradientColor2 = new ExcelColorXml(nameSpaceManager);
    }

    internal ExcelGradientFillXml(XmlNamespaceManager nsm, XmlNode topNode)
      : base(nsm, topNode)
    {
      this.Degree = this.GetXmlNodeDouble("d:gradientFill/@degree");
      this.Type = this.GetXmlNodeString("d:gradientFill/@type") == "path" ? ExcelFillGradientType.Path : ExcelFillGradientType.Linear;
      this.GradientColor1 = new ExcelColorXml(nsm, topNode.SelectSingleNode("d:gradientFill/d:stop[@position=\"0\"]/d:color", nsm));
      this.GradientColor2 = new ExcelColorXml(nsm, topNode.SelectSingleNode("d:gradientFill/d:stop[@position=\"1\"]/d:color", nsm));
      this.Top = this.GetXmlNodeDouble("d:gradientFill/@top");
      this.Bottom = this.GetXmlNodeDouble("d:gradientFill/@bottom");
      this.Left = this.GetXmlNodeDouble("d:gradientFill/@left");
      this.Right = this.GetXmlNodeDouble("d:gradientFill/@right");
    }

    public ExcelFillGradientType Type { get; internal set; }

    public double Degree { get; internal set; }

    public ExcelColorXml GradientColor1 { get; private set; }

    public ExcelColorXml GradientColor2 { get; private set; }

    public double Bottom { get; internal set; }

    public double Top { get; internal set; }

    public double Left { get; internal set; }

    public double Right { get; internal set; }

    internal override string Id
    {
      get
      {
        return base.Id + this.Degree.ToString() + this.GradientColor1.Id + this.GradientColor2.Id + (object) this.Type + this.Left.ToString() + this.Right.ToString() + this.Bottom.ToString() + this.Top.ToString();
      }
    }

    internal override ExcelFillXml Copy()
    {
      ExcelGradientFillXml excelGradientFillXml = new ExcelGradientFillXml(this.NameSpaceManager);
      excelGradientFillXml.PatternType = this._fillPatternType;
      excelGradientFillXml.BackgroundColor = this._backgroundColor.Copy();
      excelGradientFillXml.PatternColor = this._patternColor.Copy();
      excelGradientFillXml.GradientColor1 = this.GradientColor1.Copy();
      excelGradientFillXml.GradientColor2 = this.GradientColor2.Copy();
      excelGradientFillXml.Type = this.Type;
      excelGradientFillXml.Degree = this.Degree;
      excelGradientFillXml.Top = this.Top;
      excelGradientFillXml.Bottom = this.Bottom;
      excelGradientFillXml.Left = this.Left;
      excelGradientFillXml.Right = this.Right;
      return (ExcelFillXml) excelGradientFillXml;
    }

    internal override XmlNode CreateXmlNode(XmlNode topNode)
    {
      this.TopNode = topNode;
      this.CreateNode("d:gradientFill");
      if (this.Type == ExcelFillGradientType.Path)
        this.SetXmlNodeString("d:gradientFill/@type", "path");
      if (!double.IsNaN(this.Degree))
        this.SetXmlNodeString("d:gradientFill/@degree", this.Degree.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      if (this.GradientColor1 != null)
      {
        XmlNode xmlNode = this.TopNode.SelectSingleNode("d:gradientFill", this.NameSpaceManager);
        XmlElement element1 = xmlNode.OwnerDocument.CreateElement("stop", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element1.SetAttribute("position", "0");
        xmlNode.AppendChild((XmlNode) element1);
        XmlElement element2 = xmlNode.OwnerDocument.CreateElement("color", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element1.AppendChild((XmlNode) element2);
        this.GradientColor1.CreateXmlNode((XmlNode) element2);
        XmlElement element3 = xmlNode.OwnerDocument.CreateElement("stop", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element3.SetAttribute("position", "1");
        xmlNode.AppendChild((XmlNode) element3);
        XmlElement element4 = xmlNode.OwnerDocument.CreateElement("color", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element3.AppendChild((XmlNode) element4);
        this.GradientColor2.CreateXmlNode((XmlNode) element4);
      }
      if (!double.IsNaN(this.Top))
        this.SetXmlNodeString("d:gradientFill/@top", this.Top.ToString("F5", (IFormatProvider) CultureInfo.InvariantCulture));
      if (!double.IsNaN(this.Bottom))
        this.SetXmlNodeString("d:gradientFill/@bottom", this.Bottom.ToString("F5", (IFormatProvider) CultureInfo.InvariantCulture));
      if (!double.IsNaN(this.Left))
        this.SetXmlNodeString("d:gradientFill/@left", this.Left.ToString("F5", (IFormatProvider) CultureInfo.InvariantCulture));
      if (!double.IsNaN(this.Right))
        this.SetXmlNodeString("d:gradientFill/@right", this.Right.ToString("F5", (IFormatProvider) CultureInfo.InvariantCulture));
      return topNode;
    }
  }
}
