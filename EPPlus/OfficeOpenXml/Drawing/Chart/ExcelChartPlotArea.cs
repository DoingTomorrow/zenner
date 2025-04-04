// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelChartPlotArea
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public sealed class ExcelChartPlotArea : XmlHelper
  {
    private ExcelChart _firstChart;
    private ExcelChartCollection _chartTypes;
    private ExcelDrawingFill _fill;
    private ExcelDrawingBorder _border;

    internal ExcelChartPlotArea(XmlNamespaceManager ns, XmlNode node, ExcelChart firstChart)
      : base(ns, node)
    {
      this._firstChart = firstChart;
    }

    public ExcelChartCollection ChartTypes
    {
      get
      {
        if (this._chartTypes == null)
          this._chartTypes = new ExcelChartCollection(this._firstChart);
        return this._chartTypes;
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
  }
}
