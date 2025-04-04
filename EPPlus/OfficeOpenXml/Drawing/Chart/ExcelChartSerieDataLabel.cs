// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelChartSerieDataLabel
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Style;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public sealed class ExcelChartSerieDataLabel : ExcelChartDataLabel
  {
    private const string positionPath = "c:dLblPos/@val";
    private ExcelDrawingFill _fill;
    private ExcelDrawingBorder _border;
    private ExcelTextFont _font;

    internal ExcelChartSerieDataLabel(XmlNamespaceManager ns, XmlNode node)
      : base(ns, node)
    {
      this.CreateNode("c:dLblPos/@val");
      this.Position = eLabelPosition.Center;
    }

    public eLabelPosition Position
    {
      get => this.GetPosEnum(this.GetXmlNodeString("c:dLblPos/@val"));
      set => this.SetXmlNodeString("c:dLblPos/@val", this.GetPosText(value));
    }

    public new ExcelDrawingFill Fill
    {
      get
      {
        if (this._fill == null)
          this._fill = new ExcelDrawingFill(this.NameSpaceManager, this.TopNode, "c:spPr");
        return this._fill;
      }
    }

    public new ExcelDrawingBorder Border
    {
      get
      {
        if (this._border == null)
          this._border = new ExcelDrawingBorder(this.NameSpaceManager, this.TopNode, "c:spPr/a:ln");
        return this._border;
      }
    }

    public new ExcelTextFont Font
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
          this._font = new ExcelTextFont(this.NameSpaceManager, this.TopNode, "c:txPr/a:p/a:pPr/a:defRPr", new string[14]
          {
            "spPr",
            "txPr",
            "dLblPos",
            "showVal",
            "showCatName ",
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
  }
}
