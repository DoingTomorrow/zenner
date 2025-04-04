// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.ExcelDrawingBorder
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing
{
  public sealed class ExcelDrawingBorder : XmlHelper
  {
    private string _linePath;
    private ExcelDrawingFill _fill;
    private string _lineStylePath = "{0}/a:prstDash/@val";
    private string _lineCapPath = "{0}/@cap";
    private string _lineWidth = "{0}/@w";

    internal ExcelDrawingBorder(
      XmlNamespaceManager nameSpaceManager,
      XmlNode topNode,
      string linePath)
      : base(nameSpaceManager, topNode)
    {
      this.SchemaNodeOrder = new string[19]
      {
        "chart",
        "tickLblPos",
        "spPr",
        "txPr",
        "crossAx",
        "printSettings",
        "showVal",
        "showCatName",
        "showSerName",
        "showPercent",
        "separator",
        "showLeaderLines",
        "noFill",
        "solidFill",
        "blipFill",
        "gradFill",
        "noFill",
        "pattFill",
        "prstDash"
      };
      this._linePath = linePath;
      this._lineStylePath = string.Format(this._lineStylePath, (object) linePath);
      this._lineCapPath = string.Format(this._lineCapPath, (object) linePath);
      this._lineWidth = string.Format(this._lineWidth, (object) linePath);
    }

    public ExcelDrawingFill Fill
    {
      get
      {
        if (this._fill == null)
          this._fill = new ExcelDrawingFill(this.NameSpaceManager, this.TopNode, this._linePath);
        return this._fill;
      }
    }

    public eLineStyle LineStyle
    {
      get => this.TranslateLineStyle(this.GetXmlNodeString(this._lineStylePath));
      set
      {
        this.CreateNode(this._linePath, false);
        this.SetXmlNodeString(this._lineStylePath, this.TranslateLineStyleText(value));
      }
    }

    public eLineCap LineCap
    {
      get => this.TranslateLineCap(this.GetXmlNodeString(this._lineCapPath));
      set
      {
        this.CreateNode(this._linePath, false);
        this.SetXmlNodeString(this._lineCapPath, this.TranslateLineCapText(value));
      }
    }

    public int Width
    {
      get => this.GetXmlNodeInt(this._lineWidth) / 12700;
      set => this.SetXmlNodeString(this._lineWidth, (value * 12700).ToString());
    }

    private string TranslateLineStyleText(eLineStyle value)
    {
      string str = value.ToString();
      switch (value)
      {
        case eLineStyle.Dash:
        case eLineStyle.DashDot:
        case eLineStyle.Dot:
        case eLineStyle.Solid:
          return str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1);
        case eLineStyle.LongDash:
        case eLineStyle.LongDashDot:
        case eLineStyle.LongDashDotDot:
          return "lg" + str.Substring(4, str.Length - 4);
        case eLineStyle.SystemDash:
        case eLineStyle.SystemDashDot:
        case eLineStyle.SystemDashDotDot:
        case eLineStyle.SystemDot:
          return "sys" + str.Substring(6, str.Length - 6);
        default:
          throw new Exception("Invalid Linestyle");
      }
    }

    private eLineStyle TranslateLineStyle(string text)
    {
      switch (text)
      {
        case "dash":
        case "dot":
        case "dashDot":
        case "solid":
          return (eLineStyle) Enum.Parse(typeof (eLineStyle), text, true);
        case "lgDash":
        case "lgDashDot":
        case "lgDashDotDot":
          return (eLineStyle) Enum.Parse(typeof (eLineStyle), "Long" + text.Substring(2, text.Length - 2));
        case "sysDash":
        case "sysDashDot":
        case "sysDashDotDot":
        case "sysDot":
          return (eLineStyle) Enum.Parse(typeof (eLineStyle), "System" + text.Substring(3, text.Length - 3));
        default:
          throw new Exception("Invalid Linestyle");
      }
    }

    private string TranslateLineCapText(eLineCap value)
    {
      switch (value)
      {
        case eLineCap.Round:
          return "rnd";
        case eLineCap.Square:
          return "sq";
        default:
          return "flat";
      }
    }

    private eLineCap TranslateLineCap(string text)
    {
      switch (text)
      {
        case "rnd":
          return eLineCap.Round;
        case "sq":
          return eLineCap.Square;
        default:
          return eLineCap.Flat;
      }
    }
  }
}
