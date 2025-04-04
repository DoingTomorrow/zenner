// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelChartSeries
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public class ExcelChartSeries : XmlHelper, IEnumerable
  {
    private List<ExcelChartSerie> _list = new List<ExcelChartSerie>();
    internal ExcelChart _chart;
    private XmlNode _node;
    private XmlNamespaceManager _ns;
    private bool _isPivot;

    internal ExcelChartSeries(
      ExcelChart chart,
      XmlNamespaceManager ns,
      XmlNode node,
      bool isPivot)
      : base(ns, node)
    {
      this._ns = ns;
      this._chart = chart;
      this._node = node;
      this._isPivot = isPivot;
      this.SchemaNodeOrder = new string[14]
      {
        "view3D",
        "plotArea",
        "barDir",
        "grouping",
        "scatterStyle",
        "varyColors",
        "ser",
        "explosion",
        "dLbls",
        "firstSliceAng",
        "holeSize",
        "shape",
        "legend",
        "axId"
      };
      foreach (XmlNode selectNode in node.SelectNodes("c:ser", ns))
        this._list.Add(!(chart.ChartNode.LocalName == "scatterChart") ? (!(chart.ChartNode.LocalName == "lineChart") ? (chart.ChartNode.LocalName == "pieChart" || chart.ChartNode.LocalName == "ofPieChart" || chart.ChartNode.LocalName == "pie3DChart" || chart.ChartNode.LocalName == "doughnutChart" ? (ExcelChartSerie) new ExcelPieChartSerie(this, ns, selectNode, isPivot) : new ExcelChartSerie(this, ns, selectNode, isPivot)) : (ExcelChartSerie) new ExcelLineChartSerie(this, ns, selectNode, isPivot)) : (ExcelChartSerie) new ExcelScatterChartSerie(this, ns, selectNode, isPivot));
    }

    public IEnumerator GetEnumerator() => (IEnumerator) this._list.GetEnumerator();

    public ExcelChartSerie this[int PositionID] => this._list[PositionID];

    public int Count => this._list.Count;

    public void Delete(int PositionID)
    {
      ExcelChartSerie excelChartSerie = this._list[PositionID];
      excelChartSerie.TopNode.ParentNode.RemoveChild(excelChartSerie.TopNode);
      this._list.RemoveAt(PositionID);
    }

    public ExcelChart Chart => this._chart;

    public virtual ExcelChartSerie Add(ExcelRangeBase Serie, ExcelRangeBase XSerie)
    {
      if (this._chart.PivotTableSource != null)
        throw new InvalidOperationException("Can't add a serie to a pivotchart");
      return this.AddSeries(Serie.FullAddressAbsolute, XSerie.FullAddressAbsolute, "");
    }

    public virtual ExcelChartSerie Add(string SerieAddress, string XSerieAddress)
    {
      if (this._chart.PivotTableSource != null)
        throw new InvalidOperationException("Can't add a serie to a pivotchart");
      return this.AddSeries(SerieAddress, XSerieAddress, "");
    }

    protected internal ExcelChartSerie AddSeries(
      string SeriesAddress,
      string XSeriesAddress,
      string bubbleSizeAddress)
    {
      XmlElement element = this._node.OwnerDocument.CreateElement("ser", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      XmlNodeList xmlNodeList = this._node.SelectNodes("c:ser", this._ns);
      if (xmlNodeList.Count > 0)
        this._node.InsertAfter((XmlNode) element, xmlNodeList[xmlNodeList.Count - 1]);
      else
        this.InserAfter(this._node, "c:varyColors,c:grouping,c:barDir,c:scatterStyle", (XmlNode) element);
      int index = this.FindIndex();
      element.InnerXml = string.Format("<c:idx val=\"{1}\" /><c:order val=\"{1}\" /><c:tx><c:strRef><c:f></c:f><c:strCache><c:ptCount val=\"1\" /></c:strCache></c:strRef></c:tx>{5}{0}{2}{3}{4}", (object) this.AddExplosion(this.Chart.ChartType), (object) index, (object) this.AddScatterPoint(this.Chart.ChartType), (object) this.AddAxisNodes(this.Chart.ChartType), (object) this.AddSmooth(this.Chart.ChartType), (object) this.AddMarker(this.Chart.ChartType));
      ExcelChartSerie excelChartSerie;
      switch (this.Chart.ChartType)
      {
        case eChartType.XYScatter:
        case eChartType.XYScatterSmooth:
        case eChartType.XYScatterSmoothNoMarkers:
        case eChartType.XYScatterLines:
        case eChartType.XYScatterLinesNoMarkers:
          excelChartSerie = (ExcelChartSerie) new ExcelScatterChartSerie(this, this.NameSpaceManager, (XmlNode) element, this._isPivot);
          break;
        case eChartType.Radar:
        case eChartType.RadarMarkers:
        case eChartType.RadarFilled:
          excelChartSerie = (ExcelChartSerie) new ExcelRadarChartSerie(this, this.NameSpaceManager, (XmlNode) element, this._isPivot);
          break;
        case eChartType.Doughnut:
        case eChartType.Pie3D:
        case eChartType.Pie:
        case eChartType.PieOfPie:
        case eChartType.PieExploded:
        case eChartType.PieExploded3D:
        case eChartType.BarOfPie:
        case eChartType.DoughnutExploded:
          excelChartSerie = (ExcelChartSerie) new ExcelPieChartSerie(this, this.NameSpaceManager, (XmlNode) element, this._isPivot);
          break;
        case eChartType.Line:
        case eChartType.LineStacked:
        case eChartType.LineStacked100:
        case eChartType.LineMarkers:
        case eChartType.LineMarkersStacked:
        case eChartType.LineMarkersStacked100:
          excelChartSerie = (ExcelChartSerie) new ExcelLineChartSerie(this, this.NameSpaceManager, (XmlNode) element, this._isPivot);
          if (this.Chart.ChartType == eChartType.LineMarkers || this.Chart.ChartType == eChartType.LineMarkersStacked || this.Chart.ChartType == eChartType.LineMarkersStacked100)
            ((ExcelLineChartSerie) excelChartSerie).Marker = eMarkerStyle.Square;
          ((ExcelLineChartSerie) excelChartSerie).Smooth = ((ExcelLineChart) this.Chart).Smooth;
          break;
        case eChartType.Bubble:
        case eChartType.Bubble3DEffect:
          ExcelBubbleChartSerie bubbleChartSerie = new ExcelBubbleChartSerie(this, this.NameSpaceManager, (XmlNode) element, this._isPivot);
          bubbleChartSerie.Bubble3D = this.Chart.ChartType == eChartType.Bubble3DEffect;
          bubbleChartSerie.Series = SeriesAddress;
          bubbleChartSerie.XSeries = XSeriesAddress;
          bubbleChartSerie.BubbleSize = bubbleSizeAddress;
          excelChartSerie = (ExcelChartSerie) bubbleChartSerie;
          break;
        case eChartType.Surface:
        case eChartType.SurfaceWireframe:
        case eChartType.SurfaceTopView:
        case eChartType.SurfaceTopViewWireframe:
          excelChartSerie = (ExcelChartSerie) new ExcelSurfaceChartSerie(this, this.NameSpaceManager, (XmlNode) element, this._isPivot);
          break;
        default:
          excelChartSerie = new ExcelChartSerie(this, this.NameSpaceManager, (XmlNode) element, this._isPivot);
          break;
      }
      excelChartSerie.Series = SeriesAddress;
      excelChartSerie.XSeries = XSeriesAddress;
      this._list.Add(excelChartSerie);
      return excelChartSerie;
    }

    internal void AddPivotSerie(ExcelPivotTable pivotTableSource)
    {
      ExcelRange cell = pivotTableSource.WorkSheet.Cells[pivotTableSource.Address.Address];
      this._isPivot = true;
      this.AddSeries(cell.Offset(0, 1, cell._toRow - cell._fromRow + 1, 1).FullAddressAbsolute, cell.Offset(0, 0, cell._toRow - cell._fromRow + 1, 1).FullAddressAbsolute, "");
    }

    private int FindIndex()
    {
      int num1 = 0;
      int num2 = 0;
      if (this._chart.PlotArea.ChartTypes.Count <= 1)
        return this._list.Count;
      foreach (ExcelChart chartType in (IEnumerable<ExcelChart>) this._chart.PlotArea.ChartTypes)
      {
        if (num2 > 0)
        {
          foreach (ExcelChartSerie excelChartSerie in chartType.Series)
            excelChartSerie.SetID((++num2).ToString());
        }
        else if (chartType == this._chart)
        {
          num1 += this._list.Count + 1;
          num2 = num1;
        }
        else
          num1 += chartType.Series.Count;
      }
      return num1 - 1;
    }

    private string AddMarker(eChartType chartType)
    {
      return chartType == eChartType.Line || chartType == eChartType.LineStacked || chartType == eChartType.LineStacked100 || chartType == eChartType.XYScatterLines || chartType == eChartType.XYScatterSmooth || chartType == eChartType.XYScatterLinesNoMarkers || chartType == eChartType.XYScatterSmoothNoMarkers ? "<c:marker><c:symbol val=\"none\" /></c:marker>" : "";
    }

    private string AddScatterPoint(eChartType chartType)
    {
      return chartType == eChartType.XYScatter ? "<c:spPr><a:ln w=\"28575\"><a:noFill /></a:ln></c:spPr>" : "";
    }

    private string AddAxisNodes(eChartType chartType)
    {
      return chartType == eChartType.XYScatter || chartType == eChartType.XYScatterLines || chartType == eChartType.XYScatterLinesNoMarkers || chartType == eChartType.XYScatterSmooth || chartType == eChartType.XYScatterSmoothNoMarkers || chartType == eChartType.Bubble || chartType == eChartType.Bubble3DEffect ? "<c:xVal /><c:yVal />" : "<c:val />";
    }

    private string AddExplosion(eChartType chartType)
    {
      return chartType == eChartType.PieExploded3D || chartType == eChartType.PieExploded || chartType == eChartType.DoughnutExploded ? "<c:explosion val=\"25\" />" : "";
    }

    private string AddSmooth(eChartType chartType)
    {
      return chartType == eChartType.XYScatterSmooth || chartType == eChartType.XYScatterSmoothNoMarkers ? "<c:smooth val=\"1\" />" : "";
    }
  }
}
