// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelChartDataLabel
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Style;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public class ExcelChartDataLabel : XmlHelper
  {
    private const string showValPath = "c:showVal/@val";
    private const string showCatPath = "c:showCatName/@val";
    private const string showSerPath = "c:showSerName/@val";
    private const string showPerentPath = "c:showPercent/@val";
    private const string showLeaderLinesPath = "c:showLeaderLines/@val";
    private const string showBubbleSizePath = "c:showBubbleSize/@val";
    private const string showLegendKeyPath = "c:showLegendKey/@val";
    private const string separatorPath = "c:separator";
    private ExcelDrawingFill _fill;
    private ExcelDrawingBorder _border;
    private string[] _paragraphSchemaOrder = new string[18]
    {
      "spPr",
      "txPr",
      "dLblPos",
      "showVal",
      "showCatName",
      "showSerName",
      "showPercent",
      "separator",
      "showLeaderLines",
      "pPr",
      "defRPr",
      "solidFill",
      "uFill",
      "latin",
      "cs",
      "r",
      "rPr",
      "t"
    };
    private ExcelTextFont _font;

    internal ExcelChartDataLabel(XmlNamespaceManager ns, XmlNode node)
      : base(ns, node)
    {
      XmlNode newNode = node.SelectSingleNode("c:dLbls", this.NameSpaceManager);
      if (newNode == null)
      {
        newNode = (XmlNode) node.OwnerDocument.CreateElement("c", "dLbls", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        this.InserAfter(node, "c:marker,c:tx,c:order,c:ser", newNode);
        this.SchemaNodeOrder = new string[11]
        {
          "spPr",
          "txPr",
          "dLblPos",
          "showLegendKey",
          "showVal",
          "showCatName",
          "showSerName",
          "showPercent",
          "showBubbleSize",
          "separator",
          "showLeaderLines"
        };
        newNode.InnerXml = "<c:showLegendKey val=\"0\" /><c:showVal val=\"0\" /><c:showCatName val=\"0\" /><c:showSerName val=\"0\" /><c:showPercent val=\"0\" /><c:showBubbleSize val=\"0\" /> <c:separator>\r\n</c:separator><c:showLeaderLines val=\"0\" />";
      }
      this.TopNode = newNode;
    }

    public bool ShowValue
    {
      get => this.GetXmlNodeBool("c:showVal/@val");
      set => this.SetXmlNodeString("c:showVal/@val", value ? "1" : "0");
    }

    public bool ShowCategory
    {
      get => this.GetXmlNodeBool("c:showCatName/@val");
      set => this.SetXmlNodeString("c:showCatName/@val", value ? "1" : "0");
    }

    public bool ShowSeriesName
    {
      get => this.GetXmlNodeBool("c:showSerName/@val");
      set => this.SetXmlNodeString("c:showSerName/@val", value ? "1" : "0");
    }

    public bool ShowPercent
    {
      get => this.GetXmlNodeBool("c:showPercent/@val");
      set => this.SetXmlNodeString("c:showPercent/@val", value ? "1" : "0");
    }

    public bool ShowLeaderLines
    {
      get => this.GetXmlNodeBool("c:showLeaderLines/@val");
      set => this.SetXmlNodeString("c:showLeaderLines/@val", value ? "1" : "0");
    }

    public bool ShowBubbleSize
    {
      get => this.GetXmlNodeBool("c:showBubbleSize/@val");
      set => this.SetXmlNodeString("c:showBubbleSize/@val", value ? "1" : "0");
    }

    public bool ShowLegendKey
    {
      get => this.GetXmlNodeBool("c:showLegendKey/@val");
      set => this.SetXmlNodeString("c:showLegendKey/@val", value ? "1" : "0");
    }

    public string Separator
    {
      get => this.GetXmlNodeString("c:separator");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.DeleteNode("c:separator");
        else
          this.SetXmlNodeString("c:separator", value);
      }
    }

    public ExcelDrawingFill Fill
    {
      get
      {
        if (this._fill == null)
          this._fill = new ExcelDrawingFill(this.NameSpaceManager, this.TopNode, "c:spPr");
        return this._fill;
      }
    }

    public ExcelDrawingBorder Border
    {
      get
      {
        if (this._border == null)
          this._border = new ExcelDrawingBorder(this.NameSpaceManager, this.TopNode, "c:spPr/a:ln");
        return this._border;
      }
    }

    public ExcelTextFont Font
    {
      get
      {
        if (this._font == null)
        {
          if (this.TopNode.SelectSingleNode("c:txPr", this.NameSpaceManager) == null)
          {
            this.CreateNode("c:txPr/a:bodyPr");
            this.CreateNode("c:txPr/a:lstStyle");
          }
          this._font = new ExcelTextFont(this.NameSpaceManager, this.TopNode, "c:txPr/a:p/a:pPr/a:defRPr", this._paragraphSchemaOrder);
        }
        return this._font;
      }
    }

    protected string GetPosText(eLabelPosition pos)
    {
      switch (pos)
      {
        case eLabelPosition.Left:
          return "l";
        case eLabelPosition.Right:
          return "r";
        case eLabelPosition.Center:
          return "ctr";
        case eLabelPosition.Top:
          return "t";
        case eLabelPosition.Bottom:
          return "b";
        case eLabelPosition.InBase:
          return "inBase";
        case eLabelPosition.InEnd:
          return "inEnd";
        case eLabelPosition.OutEnd:
          return "outEnd";
        default:
          return "bestFit";
      }
    }

    protected eLabelPosition GetPosEnum(string pos)
    {
      switch (pos)
      {
        case "b":
          return eLabelPosition.Bottom;
        case "ctr":
          return eLabelPosition.Center;
        case "inBase":
          return eLabelPosition.InBase;
        case "inEnd":
          return eLabelPosition.InEnd;
        case "l":
          return eLabelPosition.Left;
        case "r":
          return eLabelPosition.Right;
        case "t":
          return eLabelPosition.Top;
        case "outEnd":
          return eLabelPosition.OutEnd;
        default:
          return eLabelPosition.BestFit;
      }
    }
  }
}
