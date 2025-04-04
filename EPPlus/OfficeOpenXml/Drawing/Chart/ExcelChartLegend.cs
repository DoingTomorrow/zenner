// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelChartLegend
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Style;
using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public class ExcelChartLegend : XmlHelper
  {
    private const string POSITION_PATH = "c:legendPos/@val";
    private const string OVERLAY_PATH = "c:overlay/@val";
    private ExcelChart _chart;
    private ExcelDrawingFill _fill;
    private ExcelDrawingBorder _border;
    private ExcelTextFont _font;

    internal ExcelChartLegend(XmlNamespaceManager ns, XmlNode node, ExcelChart chart)
      : base(ns, node)
    {
      this._chart = chart;
      this.SchemaNodeOrder = new string[7]
      {
        "legendPos",
        "layout",
        "overlay",
        "txPr",
        "bodyPr",
        "lstStyle",
        "spPr"
      };
    }

    public eLegendPosition Position
    {
      get
      {
        switch (this.GetXmlNodeString("c:legendPos/@val").ToLower())
        {
          case "t":
            return eLegendPosition.Top;
          case "b":
            return eLegendPosition.Bottom;
          case "l":
            return eLegendPosition.Left;
          case "tr":
            return eLegendPosition.TopRight;
          default:
            return eLegendPosition.Right;
        }
      }
      set
      {
        if (this.TopNode == null)
          throw new Exception("Can't set position. Chart has no legend");
        switch (value)
        {
          case eLegendPosition.Top:
            this.SetXmlNodeString("c:legendPos/@val", "t");
            break;
          case eLegendPosition.Left:
            this.SetXmlNodeString("c:legendPos/@val", "l");
            break;
          case eLegendPosition.Bottom:
            this.SetXmlNodeString("c:legendPos/@val", "b");
            break;
          case eLegendPosition.TopRight:
            this.SetXmlNodeString("c:legendPos/@val", "tr");
            break;
          default:
            this.SetXmlNodeString("c:legendPos/@val", "r");
            break;
        }
      }
    }

    public bool Overlay
    {
      get => this.GetXmlNodeBool("c:overlay/@val");
      set
      {
        if (this.TopNode == null)
          throw new Exception("Can't set overlay. Chart has no legend");
        this.SetXmlNodeBool("c:overlay/@val", value);
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
          this._font = new ExcelTextFont(this.NameSpaceManager, this.TopNode, "c:txPr/a:p/a:pPr/a:defRPr", new string[11]
          {
            "legendPos",
            "layout",
            "pPr",
            "defRPr",
            "solidFill",
            "uFill",
            "latin",
            "cs",
            "r",
            "rPr",
            "t"
          });
        }
        return this._font;
      }
    }

    public void Remove()
    {
      if (this.TopNode == null)
        return;
      this.TopNode.ParentNode.RemoveChild(this.TopNode);
      this.TopNode = (XmlNode) null;
    }

    public void Add()
    {
      if (this.TopNode != null)
        return;
      XmlHelper xmlHelper = XmlHelperFactory.Create(this.NameSpaceManager, (XmlNode) this._chart.ChartXml);
      xmlHelper.SchemaNodeOrder = this._chart.SchemaNodeOrder;
      xmlHelper.CreateNode("c:chartSpace/c:chart/c:legend");
      this.TopNode = this._chart.ChartXml.SelectSingleNode("c:chartSpace/c:chart/c:legend", this.NameSpaceManager);
      this.TopNode.InnerXml = "<c:legendPos val=\"r\" /><c:layout />";
    }
  }
}
