// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelChart
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using OfficeOpenXml.Table.PivotTable;
using OfficeOpenXml.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public class ExcelChart : ExcelDrawing
  {
    private const string rootPath = "c:chartSpace/c:chart/c:plotArea";
    private const string _plotVisibleOnlyPath = "../../c:plotVisOnly/@val";
    private const string _displayBlanksAsPath = "../../c:dispBlanksAs/@val";
    private const string _showDLblsOverMax = "../../c:showDLblsOverMax/@val";
    protected internal ExcelChartSeries _chartSeries;
    internal ExcelChartAxis[] _axis;
    protected XmlHelper _chartXmlHelper;
    protected internal XmlNode _chartNode;
    private bool _secondaryAxis;
    private ExcelChartPlotArea _plotArea;
    private ExcelChartLegend _legend;
    private ExcelDrawingBorder _border;
    private ExcelDrawingFill _fill;
    private string _groupingPath = "c:grouping/@val";
    private string _varyColorsPath = "c:varyColors/@val";
    private ExcelChartTitle _title;

    internal ExcelChart(ExcelDrawings drawings, XmlNode node, eChartType type, bool isPivot)
      : base(drawings, node, "xdr:graphicFrame/xdr:nvGraphicFramePr/xdr:cNvPr/@name")
    {
      this.ChartType = type;
      this.CreateNewChart(drawings, type, (ExcelChart) null);
      this.Init(drawings, this._chartNode);
      this._chartSeries = new ExcelChartSeries(this, drawings.NameSpaceManager, this._chartNode, isPivot);
      this.SetTypeProperties();
      this.LoadAxis();
    }

    internal ExcelChart(
      ExcelDrawings drawings,
      XmlNode node,
      eChartType type,
      ExcelChart topChart,
      ExcelPivotTable PivotTableSource)
      : base(drawings, node, "xdr:graphicFrame/xdr:nvGraphicFramePr/xdr:cNvPr/@name")
    {
      this.ChartType = type;
      this.CreateNewChart(drawings, type, topChart);
      this.Init(drawings, this._chartNode);
      this._chartSeries = new ExcelChartSeries(this, drawings.NameSpaceManager, this._chartNode, PivotTableSource != null);
      if (PivotTableSource != null)
        this.SetPivotSource(PivotTableSource);
      this.SetTypeProperties();
      if (topChart == null)
      {
        this.LoadAxis();
      }
      else
      {
        this._axis = topChart.Axis;
        if (this._axis.Length <= 0)
          return;
        this.XAxis = this._axis[0];
        this.YAxis = this._axis[1];
      }
    }

    internal ExcelChart(
      ExcelDrawings drawings,
      XmlNode node,
      Uri uriChart,
      ZipPackagePart part,
      XmlDocument chartXml,
      XmlNode chartNode)
      : base(drawings, node, "xdr:graphicFrame/xdr:nvGraphicFramePr/xdr:cNvPr/@name")
    {
      this.UriChart = uriChart;
      this.Part = part;
      this.ChartXml = chartXml;
      this._chartNode = chartNode;
      this.InitChartLoad(drawings, chartNode);
      this.ChartType = this.GetChartType(chartNode.LocalName);
    }

    internal ExcelChart(ExcelChart topChart, XmlNode chartNode)
      : base(topChart._drawings, topChart.TopNode, "xdr:graphicFrame/xdr:nvGraphicFramePr/xdr:cNvPr/@name")
    {
      this.UriChart = topChart.UriChart;
      this.Part = topChart.Part;
      this.ChartXml = topChart.ChartXml;
      this._plotArea = topChart.PlotArea;
      this._chartNode = chartNode;
      this.InitChartLoad(topChart._drawings, chartNode);
    }

    private void InitChartLoad(ExcelDrawings drawings, XmlNode chartNode)
    {
      bool isPivot = false;
      this.Init(drawings, chartNode);
      this._chartSeries = new ExcelChartSeries(this, drawings.NameSpaceManager, this._chartNode, isPivot);
      this.LoadAxis();
    }

    private void Init(ExcelDrawings drawings, XmlNode chartNode)
    {
      this._chartXmlHelper = XmlHelperFactory.Create(drawings.NameSpaceManager, chartNode);
      this._chartXmlHelper.SchemaNodeOrder = new string[32]
      {
        "title",
        "pivotFmt",
        "autoTitleDeleted",
        "view3D",
        "floor",
        "sideWall",
        "backWall",
        "plotArea",
        "wireframe",
        "barDir",
        "grouping",
        "scatterStyle",
        "radarStyle",
        "varyColors",
        "ser",
        "dLbls",
        "bubbleScale",
        "showNegBubbles",
        "dropLines",
        "upDownBars",
        "marker",
        "smooth",
        "shape",
        "legend",
        "plotVisOnly",
        "dispBlanksAs",
        "showDLblsOverMax",
        "overlap",
        "bandFmts",
        "axId",
        "spPr",
        "printSettings"
      };
      this.WorkSheet = drawings.Worksheet;
    }

    private void SetTypeProperties()
    {
      if (this.IsTypeClustered())
        this.Grouping = eGrouping.Clustered;
      else if (this.IsTypeStacked())
        this.Grouping = eGrouping.Stacked;
      else if (this.IsTypePercentStacked())
        this.Grouping = eGrouping.PercentStacked;
      if (!this.IsType3D())
        return;
      this.View3D.RotY = 20M;
      this.View3D.Perspective = 30M;
      if (this.IsTypePieDoughnut())
        this.View3D.RotX = 30M;
      else
        this.View3D.RotX = 15M;
    }

    private void CreateNewChart(ExcelDrawings drawings, eChartType type, ExcelChart topChart)
    {
      if (topChart == null)
      {
        XmlElement element = this.TopNode.OwnerDocument.CreateElement("graphicFrame", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
        element.SetAttribute("macro", "");
        this.TopNode.AppendChild((XmlNode) element);
        element.InnerXml = string.Format("<xdr:nvGraphicFramePr><xdr:cNvPr id=\"{0}\" name=\"Chart 1\" /><xdr:cNvGraphicFramePr /></xdr:nvGraphicFramePr><xdr:xfrm><a:off x=\"0\" y=\"0\" /> <a:ext cx=\"0\" cy=\"0\" /></xdr:xfrm><a:graphic><a:graphicData uri=\"http://schemas.openxmlformats.org/drawingml/2006/chart\"><c:chart xmlns:c=\"http://schemas.openxmlformats.org/drawingml/2006/chart\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\" r:id=\"rId1\" />   </a:graphicData>  </a:graphic>", (object) this._id);
        this.TopNode.AppendChild((XmlNode) this.TopNode.OwnerDocument.CreateElement("clientData", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing"));
        ZipPackage package = drawings.Worksheet._package.Package;
        this.UriChart = XmlHelper.GetNewUri(package, "/xl/charts/chart{0}.xml");
        this.ChartXml = new XmlDocument();
        this.ChartXml.PreserveWhitespace = false;
        XmlHelper.LoadXmlSafe(this.ChartXml, this.ChartStartXml(type), Encoding.UTF8);
        this.Part = package.CreatePart(this.UriChart, "application/vnd.openxmlformats-officedocument.drawingml.chart+xml", this._drawings._package.Compression);
        StreamWriter writer = new StreamWriter((Stream) this.Part.GetStream(FileMode.Create, FileAccess.Write));
        this.ChartXml.Save((TextWriter) writer);
        writer.Close();
        package.Flush();
        ZipPackageRelationship relationship = drawings.Part.CreateRelationship(UriHelper.GetRelativeUri(drawings.UriDrawing, this.UriChart), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chart");
        element.SelectSingleNode("a:graphic/a:graphicData/c:chart", this.NameSpaceManager).Attributes["r:id"].Value = relationship.Id;
        package.Flush();
        this._chartNode = this.ChartXml.SelectSingleNode(string.Format("c:chartSpace/c:chart/c:plotArea/{0}", (object) this.GetChartNodeText()), this.NameSpaceManager);
      }
      else
      {
        this.ChartXml = topChart.ChartXml;
        this.Part = topChart.Part;
        this._plotArea = topChart.PlotArea;
        this.UriChart = topChart.UriChart;
        this._axis = topChart._axis;
        XmlNode chartNode = this._plotArea.ChartTypes[this._plotArea.ChartTypes.Count - 1].ChartNode;
        this._chartNode = (XmlNode) this.ChartXml.CreateElement(this.GetChartNodeText(), "http://schemas.openxmlformats.org/drawingml/2006/chart");
        chartNode.ParentNode.InsertAfter(this._chartNode, chartNode);
        if (topChart.Axis.Length == 0)
          this.AddAxis();
        this._chartNode.InnerXml = this.GetChartSerieStartXml(type, int.Parse(topChart.Axis[0].Id), int.Parse(topChart.Axis[1].Id), topChart.Axis.Length > 2 ? int.Parse(topChart.Axis[2].Id) : -1);
      }
    }

    private void LoadAxis()
    {
      XmlNodeList xmlNodeList1 = this._chartNode.SelectNodes("c:axId", this.NameSpaceManager);
      List<ExcelChartAxis> excelChartAxisList = new List<ExcelChartAxis>();
      foreach (XmlNode xmlNode1 in xmlNodeList1)
      {
        XmlNodeList xmlNodeList2 = this.ChartXml.SelectNodes("c:chartSpace/c:chart/c:plotArea" + string.Format("/*/c:axId[@val=\"{0}\"]", (object) xmlNode1.Attributes["val"].Value), this.NameSpaceManager);
        if (xmlNodeList2 != null && xmlNodeList2.Count > 1)
        {
          foreach (XmlNode xmlNode2 in xmlNodeList2)
          {
            if (xmlNode2.ParentNode.LocalName.EndsWith("Ax"))
            {
              ExcelChartAxis excelChartAxis = new ExcelChartAxis(this.NameSpaceManager, xmlNodeList2[1].ParentNode);
              excelChartAxisList.Add(excelChartAxis);
            }
          }
        }
      }
      this._axis = excelChartAxisList.ToArray();
      if (this._axis.Length > 0)
        this.XAxis = this._axis[0];
      if (this._axis.Length <= 1)
        return;
      this.YAxis = this._axis[1];
    }

    internal virtual eChartType GetChartType(string name)
    {
      switch (name)
      {
        case "area3DChart":
          if (this.Grouping == eGrouping.Stacked)
            return eChartType.AreaStacked3D;
          return this.Grouping == eGrouping.PercentStacked ? eChartType.AreaStacked1003D : eChartType.Area3D;
        case "areaChart":
          if (this.Grouping == eGrouping.Stacked)
            return eChartType.AreaStacked;
          return this.Grouping == eGrouping.PercentStacked ? eChartType.AreaStacked100 : eChartType.Area;
        case "doughnutChart":
          return eChartType.Doughnut;
        case "pie3DChart":
          return eChartType.Pie3D;
        case "pieChart":
          return eChartType.Pie;
        case "radarChart":
          return eChartType.Radar;
        case "scatterChart":
          return eChartType.XYScatter;
        case "surface3DChart":
        case "surfaceChart":
          return eChartType.Surface;
        case "stockChart":
          return eChartType.StockHLC;
        default:
          return (eChartType) 0;
      }
    }

    private string ChartStartXml(eChartType type)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int axID = 1;
      int xAxID = 2;
      int serAxID = this.IsTypeSurface() ? 3 : -1;
      stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
      stringBuilder.AppendFormat("<c:chartSpace xmlns:c=\"{0}\" xmlns:a=\"{1}\" xmlns:r=\"{2}\">", (object) "http://schemas.openxmlformats.org/drawingml/2006/chart", (object) "http://schemas.openxmlformats.org/drawingml/2006/main", (object) "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
      stringBuilder.Append("<c:chart>");
      stringBuilder.AppendFormat("{0}{1}<c:plotArea><c:layout/>", (object) this.AddPerspectiveXml(type), (object) this.AddSurfaceXml(type));
      string chartNodeText = this.GetChartNodeText();
      stringBuilder.AppendFormat("<{0}>", (object) chartNodeText);
      stringBuilder.Append(this.GetChartSerieStartXml(type, axID, xAxID, serAxID));
      stringBuilder.AppendFormat("</{0}>", (object) chartNodeText);
      if (!this.IsTypePieDoughnut())
      {
        if (this.IsTypeScatterBubble())
          stringBuilder.AppendFormat("<c:valAx><c:axId val=\"{0}\"/><c:scaling><c:orientation val=\"minMax\"/></c:scaling><c:delete val=\"0\"/><c:axPos val=\"b\"/><c:tickLblPos val=\"nextTo\"/><c:crossAx val=\"{1}\"/><c:crosses val=\"autoZero\"/></c:valAx>", (object) axID, (object) xAxID);
        else
          stringBuilder.AppendFormat("<c:catAx><c:axId val=\"{0}\"/><c:scaling><c:orientation val=\"minMax\"/></c:scaling><c:delete val=\"0\"/><c:axPos val=\"b\"/><c:tickLblPos val=\"nextTo\"/><c:crossAx val=\"{1}\"/><c:crosses val=\"autoZero\"/><c:auto val=\"1\"/><c:lblAlgn val=\"ctr\"/><c:lblOffset val=\"100\"/></c:catAx>", (object) axID, (object) xAxID);
        stringBuilder.AppendFormat("<c:valAx><c:axId val=\"{1}\"/><c:scaling><c:orientation val=\"minMax\"/></c:scaling><c:delete val=\"0\"/><c:axPos val=\"l\"/><c:majorGridlines/><c:tickLblPos val=\"nextTo\"/><c:crossAx val=\"{0}\"/><c:crosses val=\"autoZero\"/><c:crossBetween val=\"between\"/></c:valAx>", (object) axID, (object) xAxID);
        if (serAxID == 3)
          stringBuilder.AppendFormat("<c:serAx><c:axId val=\"{0}\"/><c:scaling><c:orientation val=\"minMax\"/></c:scaling><c:delete val=\"0\"/><c:axPos val=\"b\"/><c:tickLblPos val=\"nextTo\"/><c:crossAx val=\"{1}\"/><c:crosses val=\"autoZero\"/></c:serAx>", (object) serAxID, (object) xAxID);
      }
      stringBuilder.AppendFormat("</c:plotArea><c:legend><c:legendPos val=\"r\"/><c:layout/><c:overlay val=\"0\" /></c:legend><c:plotVisOnly val=\"1\"/></c:chart>", (object) axID, (object) xAxID);
      stringBuilder.Append("<c:printSettings><c:headerFooter/><c:pageMargins b=\"0.75\" l=\"0.7\" r=\"0.7\" t=\"0.75\" header=\"0.3\" footer=\"0.3\"/><c:pageSetup/></c:printSettings></c:chartSpace>");
      return stringBuilder.ToString();
    }

    private string GetChartSerieStartXml(eChartType type, int axID, int xAxID, int serAxID)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.AddScatterType(type));
      stringBuilder.Append(this.AddRadarType(type));
      stringBuilder.Append(this.AddBarDir(type));
      stringBuilder.Append(this.AddGrouping());
      stringBuilder.Append(this.AddVaryColors());
      stringBuilder.Append(this.AddHasMarker(type));
      stringBuilder.Append(this.AddShape(type));
      stringBuilder.Append(this.AddFirstSliceAng(type));
      stringBuilder.Append(this.AddHoleSize(type));
      if (this.ChartType == eChartType.BarStacked100 || this.ChartType == eChartType.BarStacked || this.ChartType == eChartType.ColumnStacked || this.ChartType == eChartType.ColumnStacked100)
        stringBuilder.Append("<c:overlap val=\"100\"/>");
      if (this.IsTypeSurface())
        stringBuilder.Append("<c:bandFmts/>");
      stringBuilder.Append(this.AddAxisId(axID, xAxID, serAxID));
      return stringBuilder.ToString();
    }

    private string AddAxisId(int axID, int xAxID, int serAxID)
    {
      if (this.IsTypePieDoughnut())
        return "";
      return this.IsTypeSurface() ? string.Format("<c:axId val=\"{0}\"/><c:axId val=\"{1}\"/><c:axId val=\"{2}\"/>", (object) axID, (object) xAxID, (object) serAxID) : string.Format("<c:axId val=\"{0}\"/><c:axId val=\"{1}\"/>", (object) axID, (object) xAxID);
    }

    private string AddAxType()
    {
      switch (this.ChartType)
      {
        case eChartType.XYScatter:
        case eChartType.Bubble:
        case eChartType.XYScatterSmooth:
        case eChartType.XYScatterSmoothNoMarkers:
        case eChartType.XYScatterLines:
        case eChartType.XYScatterLinesNoMarkers:
        case eChartType.Bubble3DEffect:
          return "valAx";
        default:
          return "catAx";
      }
    }

    private string AddScatterType(eChartType type)
    {
      return type == eChartType.XYScatter || type == eChartType.XYScatterLines || type == eChartType.XYScatterLinesNoMarkers || type == eChartType.XYScatterSmooth || type == eChartType.XYScatterSmoothNoMarkers ? "<c:scatterStyle val=\"\" />" : "";
    }

    private string AddRadarType(eChartType type)
    {
      return type == eChartType.Radar || type == eChartType.RadarFilled || type == eChartType.RadarMarkers ? "<c:radarStyle val=\"\" />" : "";
    }

    private string AddGrouping()
    {
      return this.IsTypeShape() || this.IsTypeLine() ? "<c:grouping val=\"standard\"/>" : "";
    }

    private string AddHoleSize(eChartType type)
    {
      return type == eChartType.Doughnut || type == eChartType.DoughnutExploded ? "<c:holeSize val=\"50\" />" : "";
    }

    private string AddFirstSliceAng(eChartType type)
    {
      return type == eChartType.Doughnut || type == eChartType.DoughnutExploded ? "<c:firstSliceAng val=\"0\" />" : "";
    }

    private string AddVaryColors()
    {
      return this.IsTypePieDoughnut() ? "<c:varyColors val=\"1\" />" : "<c:varyColors val=\"0\" />";
    }

    private string AddHasMarker(eChartType type)
    {
      return type == eChartType.LineMarkers || type == eChartType.LineMarkersStacked || type == eChartType.LineMarkersStacked100 ? "<c:marker val=\"1\"/>" : "";
    }

    private string AddShape(eChartType type) => this.IsTypeShape() ? "<c:shape val=\"box\" />" : "";

    private string AddBarDir(eChartType type)
    {
      return this.IsTypeShape() ? "<c:barDir val=\"col\" />" : "";
    }

    private string AddPerspectiveXml(eChartType type)
    {
      return this.IsType3D() ? "<c:view3D><c:perspective val=\"30\" /></c:view3D>" : "";
    }

    private string AddSurfaceXml(eChartType type)
    {
      return this.IsTypeSurface() ? this.AddSurfacePart("floor") + this.AddSurfacePart("sideWall") + this.AddSurfacePart("backWall") : "";
    }

    private string AddSurfacePart(string name)
    {
      return string.Format("<c:{0}><c:thickness val=\"0\"/><c:spPr><a:noFill/><a:ln><a:noFill/></a:ln><a:effectLst/><a:sp3d/></c:spPr></c:{0}>", (object) name);
    }

    internal static bool IsType3D(eChartType chartType)
    {
      return chartType == eChartType.Area3D || chartType == eChartType.AreaStacked3D || chartType == eChartType.AreaStacked1003D || chartType == eChartType.BarClustered3D || chartType == eChartType.BarStacked3D || chartType == eChartType.BarStacked1003D || chartType == eChartType.Column3D || chartType == eChartType.ColumnClustered3D || chartType == eChartType.ColumnStacked3D || chartType == eChartType.ColumnStacked1003D || chartType == eChartType.Line3D || chartType == eChartType.Pie3D || chartType == eChartType.PieExploded3D || chartType == eChartType.ConeBarClustered || chartType == eChartType.ConeBarStacked || chartType == eChartType.ConeBarStacked100 || chartType == eChartType.ConeCol || chartType == eChartType.ConeColClustered || chartType == eChartType.ConeColStacked || chartType == eChartType.ConeColStacked100 || chartType == eChartType.CylinderBarClustered || chartType == eChartType.CylinderBarStacked || chartType == eChartType.CylinderBarStacked100 || chartType == eChartType.CylinderCol || chartType == eChartType.CylinderColClustered || chartType == eChartType.CylinderColStacked || chartType == eChartType.CylinderColStacked100 || chartType == eChartType.PyramidBarClustered || chartType == eChartType.PyramidBarStacked || chartType == eChartType.PyramidBarStacked100 || chartType == eChartType.PyramidCol || chartType == eChartType.PyramidColClustered || chartType == eChartType.PyramidColStacked || chartType == eChartType.PyramidColStacked100 || chartType == eChartType.Surface || chartType == eChartType.SurfaceTopView || chartType == eChartType.SurfaceTopViewWireframe || chartType == eChartType.SurfaceWireframe;
    }

    protected internal bool IsType3D() => ExcelChart.IsType3D(this.ChartType);

    protected bool IsTypeLine()
    {
      return this.ChartType == eChartType.Line || this.ChartType == eChartType.LineMarkers || this.ChartType == eChartType.LineMarkersStacked100 || this.ChartType == eChartType.LineStacked || this.ChartType == eChartType.LineStacked100 || this.ChartType == eChartType.Line3D;
    }

    protected bool IsTypeScatterBubble()
    {
      return this.ChartType == eChartType.XYScatter || this.ChartType == eChartType.XYScatterLines || this.ChartType == eChartType.XYScatterLinesNoMarkers || this.ChartType == eChartType.XYScatterSmooth || this.ChartType == eChartType.XYScatterSmoothNoMarkers || this.ChartType == eChartType.Bubble || this.ChartType == eChartType.Bubble3DEffect;
    }

    protected bool IsTypeSurface()
    {
      return this.ChartType == eChartType.Surface || this.ChartType == eChartType.SurfaceTopView || this.ChartType == eChartType.SurfaceTopViewWireframe || this.ChartType == eChartType.SurfaceWireframe;
    }

    protected bool IsTypeShape()
    {
      return this.ChartType == eChartType.BarClustered3D || this.ChartType == eChartType.BarStacked3D || this.ChartType == eChartType.BarStacked1003D || this.ChartType == eChartType.BarClustered3D || this.ChartType == eChartType.BarStacked3D || this.ChartType == eChartType.BarStacked1003D || this.ChartType == eChartType.Column3D || this.ChartType == eChartType.ColumnClustered3D || this.ChartType == eChartType.ColumnStacked3D || this.ChartType == eChartType.ColumnStacked1003D || this.ChartType == eChartType.ConeBarClustered || this.ChartType == eChartType.ConeBarStacked || this.ChartType == eChartType.ConeBarStacked100 || this.ChartType == eChartType.ConeCol || this.ChartType == eChartType.ConeColClustered || this.ChartType == eChartType.ConeColStacked || this.ChartType == eChartType.ConeColStacked100 || this.ChartType == eChartType.CylinderBarClustered || this.ChartType == eChartType.CylinderBarStacked || this.ChartType == eChartType.CylinderBarStacked100 || this.ChartType == eChartType.CylinderCol || this.ChartType == eChartType.CylinderColClustered || this.ChartType == eChartType.CylinderColStacked || this.ChartType == eChartType.CylinderColStacked100 || this.ChartType == eChartType.PyramidBarClustered || this.ChartType == eChartType.PyramidBarStacked || this.ChartType == eChartType.PyramidBarStacked100 || this.ChartType == eChartType.PyramidCol || this.ChartType == eChartType.PyramidColClustered || this.ChartType == eChartType.PyramidColStacked || this.ChartType == eChartType.PyramidColStacked100;
    }

    protected internal bool IsTypePercentStacked()
    {
      return this.ChartType == eChartType.AreaStacked100 || this.ChartType == eChartType.BarStacked100 || this.ChartType == eChartType.BarStacked1003D || this.ChartType == eChartType.ColumnStacked100 || this.ChartType == eChartType.ColumnStacked1003D || this.ChartType == eChartType.ConeBarStacked100 || this.ChartType == eChartType.ConeColStacked100 || this.ChartType == eChartType.CylinderBarStacked100 || this.ChartType == eChartType.CylinderColStacked || this.ChartType == eChartType.LineMarkersStacked100 || this.ChartType == eChartType.LineStacked100 || this.ChartType == eChartType.PyramidBarStacked100 || this.ChartType == eChartType.PyramidColStacked100;
    }

    protected internal bool IsTypeStacked()
    {
      return this.ChartType == eChartType.AreaStacked || this.ChartType == eChartType.AreaStacked3D || this.ChartType == eChartType.BarStacked || this.ChartType == eChartType.BarStacked3D || this.ChartType == eChartType.ColumnStacked3D || this.ChartType == eChartType.ColumnStacked || this.ChartType == eChartType.ConeBarStacked || this.ChartType == eChartType.ConeColStacked || this.ChartType == eChartType.CylinderBarStacked || this.ChartType == eChartType.CylinderColStacked || this.ChartType == eChartType.LineMarkersStacked || this.ChartType == eChartType.LineStacked || this.ChartType == eChartType.PyramidBarStacked || this.ChartType == eChartType.PyramidColStacked;
    }

    protected bool IsTypeClustered()
    {
      return this.ChartType == eChartType.BarClustered || this.ChartType == eChartType.BarClustered3D || this.ChartType == eChartType.ColumnClustered3D || this.ChartType == eChartType.ColumnClustered || this.ChartType == eChartType.ConeBarClustered || this.ChartType == eChartType.ConeColClustered || this.ChartType == eChartType.CylinderBarClustered || this.ChartType == eChartType.CylinderColClustered || this.ChartType == eChartType.PyramidBarClustered || this.ChartType == eChartType.PyramidColClustered;
    }

    protected internal bool IsTypePieDoughnut()
    {
      return this.ChartType == eChartType.Pie || this.ChartType == eChartType.PieExploded || this.ChartType == eChartType.PieOfPie || this.ChartType == eChartType.Pie3D || this.ChartType == eChartType.PieExploded3D || this.ChartType == eChartType.BarOfPie || this.ChartType == eChartType.Doughnut || this.ChartType == eChartType.DoughnutExploded;
    }

    protected string GetChartNodeText()
    {
      switch (this.ChartType)
      {
        case eChartType.XYScatter:
        case eChartType.XYScatterSmooth:
        case eChartType.XYScatterSmoothNoMarkers:
        case eChartType.XYScatterLines:
        case eChartType.XYScatterLinesNoMarkers:
          return "c:scatterChart";
        case eChartType.Radar:
        case eChartType.RadarMarkers:
        case eChartType.RadarFilled:
          return "c:radarChart";
        case eChartType.Doughnut:
        case eChartType.DoughnutExploded:
          return "c:doughnutChart";
        case eChartType.Pie3D:
        case eChartType.PieExploded3D:
          return "c:pie3DChart";
        case eChartType.Line3D:
          return "c:line3DChart";
        case eChartType.Area3D:
        case eChartType.AreaStacked3D:
        case eChartType.AreaStacked1003D:
          return "c:area3DChart";
        case eChartType.Area:
        case eChartType.AreaStacked:
        case eChartType.AreaStacked100:
          return "c:areaChart";
        case eChartType.Line:
        case eChartType.LineStacked:
        case eChartType.LineStacked100:
        case eChartType.LineMarkers:
        case eChartType.LineMarkersStacked:
        case eChartType.LineMarkersStacked100:
          return "c:lineChart";
        case eChartType.Pie:
        case eChartType.PieExploded:
          return "c:pieChart";
        case eChartType.Bubble:
        case eChartType.Bubble3DEffect:
          return "c:bubbleChart";
        case eChartType.ColumnClustered:
        case eChartType.ColumnStacked:
        case eChartType.ColumnStacked100:
        case eChartType.BarClustered:
        case eChartType.BarStacked:
        case eChartType.BarStacked100:
          return "c:barChart";
        case eChartType.ColumnClustered3D:
        case eChartType.ColumnStacked3D:
        case eChartType.ColumnStacked1003D:
        case eChartType.BarClustered3D:
        case eChartType.BarStacked3D:
        case eChartType.BarStacked1003D:
        case eChartType.CylinderColClustered:
        case eChartType.CylinderColStacked:
        case eChartType.CylinderColStacked100:
        case eChartType.CylinderBarClustered:
        case eChartType.CylinderBarStacked:
        case eChartType.CylinderBarStacked100:
        case eChartType.CylinderCol:
        case eChartType.ConeColClustered:
        case eChartType.ConeColStacked:
        case eChartType.ConeColStacked100:
        case eChartType.ConeBarClustered:
        case eChartType.ConeBarStacked:
        case eChartType.ConeBarStacked100:
        case eChartType.ConeCol:
        case eChartType.PyramidColClustered:
        case eChartType.PyramidColStacked:
        case eChartType.PyramidColStacked100:
        case eChartType.PyramidBarClustered:
        case eChartType.PyramidBarStacked:
        case eChartType.PyramidBarStacked100:
        case eChartType.PyramidCol:
          return "c:bar3DChart";
        case eChartType.PieOfPie:
        case eChartType.BarOfPie:
          return "c:ofPieChart";
        case eChartType.Surface:
        case eChartType.SurfaceWireframe:
          return "c:surface3DChart";
        case eChartType.SurfaceTopView:
        case eChartType.SurfaceTopViewWireframe:
          return "c:surfaceChart";
        case eChartType.StockHLC:
          return "c:stockChart";
        default:
          throw new NotImplementedException("Chart type not implemented");
      }
    }

    internal void AddAxis()
    {
      XmlElement element1 = this.ChartXml.CreateElement(string.Format("c:{0}", (object) this.AddAxType()), "http://schemas.openxmlformats.org/drawingml/2006/chart");
      int num;
      if (this._axis.Length == 0)
      {
        this._plotArea.TopNode.AppendChild((XmlNode) element1);
        num = 1;
      }
      else
      {
        this._axis[0].TopNode.ParentNode.InsertAfter((XmlNode) element1, this._axis[this._axis.Length - 1].TopNode);
        num = int.Parse(this._axis[0].Id) < int.Parse(this._axis[1].Id) ? int.Parse(this._axis[1].Id) + 1 : int.Parse(this._axis[0].Id) + 1;
      }
      XmlElement element2 = this.ChartXml.CreateElement("c:valAx", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      element1.ParentNode.InsertAfter((XmlNode) element2, (XmlNode) element1);
      if (this._axis.Length == 0)
      {
        element1.InnerXml = string.Format("<c:axId val=\"{0}\"/><c:scaling><c:orientation val=\"minMax\"/></c:scaling><c:delete val=\"0\" /><c:axPos val=\"b\"/><c:tickLblPos val=\"nextTo\"/><c:crossAx val=\"{1}\"/><c:crosses val=\"autoZero\"/><c:auto val=\"1\"/><c:lblAlgn val=\"ctr\"/><c:lblOffset val=\"100\"/>", (object) num, (object) (num + 1));
        element2.InnerXml = string.Format("<c:axId val=\"{1}\"/><c:scaling><c:orientation val=\"minMax\"/></c:scaling><c:delete val=\"0\" /><c:axPos val=\"l\"/><c:majorGridlines/><c:tickLblPos val=\"nextTo\"/><c:crossAx val=\"{0}\"/><c:crosses val=\"autoZero\"/><c:crossBetween val=\"between\"/>", (object) num, (object) (num + 1));
      }
      else
      {
        element1.InnerXml = string.Format("<c:axId val=\"{0}\"/><c:scaling><c:orientation val=\"minMax\"/></c:scaling><c:delete val=\"1\" /><c:axPos val=\"b\"/><c:tickLblPos val=\"none\"/><c:crossAx val=\"{1}\"/><c:crosses val=\"autoZero\"/>", (object) num, (object) (num + 1));
        element2.InnerXml = string.Format("<c:axId val=\"{0}\"/><c:scaling><c:orientation val=\"minMax\"/></c:scaling><c:delete val=\"0\" /><c:axPos val=\"r\"/><c:tickLblPos val=\"nextTo\"/><c:crossAx val=\"{1}\"/><c:crosses val=\"max\"/><c:crossBetween val=\"between\"/>", (object) (num + 1), (object) num);
      }
      if (this._axis.Length == 0)
      {
        this._axis = new ExcelChartAxis[2];
      }
      else
      {
        ExcelChartAxis[] destinationArray = new ExcelChartAxis[this._axis.Length + 2];
        Array.Copy((Array) this._axis, (Array) destinationArray, this._axis.Length);
        this._axis = destinationArray;
      }
      this._axis[this._axis.Length - 2] = new ExcelChartAxis(this.NameSpaceManager, (XmlNode) element1);
      this._axis[this._axis.Length - 1] = new ExcelChartAxis(this.NameSpaceManager, (XmlNode) element2);
      foreach (ExcelChart chartType in (IEnumerable<ExcelChart>) this._plotArea.ChartTypes)
        chartType._axis = this._axis;
    }

    internal void RemoveSecondaryAxis() => throw new NotImplementedException("Not yet implemented");

    public ExcelWorksheet WorkSheet { get; internal set; }

    public XmlDocument ChartXml { get; internal set; }

    public eChartType ChartType { get; internal set; }

    internal XmlNode ChartNode => this._chartNode;

    public ExcelChartTitle Title
    {
      get
      {
        if (this._title == null)
          this._title = new ExcelChartTitle(this.NameSpaceManager, this.ChartXml.SelectSingleNode("c:chartSpace/c:chart", this.NameSpaceManager));
        return this._title;
      }
    }

    public virtual ExcelChartSeries Series => this._chartSeries;

    public ExcelChartAxis[] Axis => this._axis;

    public ExcelChartAxis XAxis { get; private set; }

    public ExcelChartAxis YAxis { get; private set; }

    public bool UseSecondaryAxis
    {
      get => this._secondaryAxis;
      set
      {
        if (this._secondaryAxis == value)
          return;
        if (value)
        {
          if (this.IsTypePieDoughnut())
            throw new Exception("Pie charts do not support axis");
          if (!this.HasPrimaryAxis())
            throw new Exception("Can't set to secondary axis when no serie uses the primary axis");
          if (this.Axis.Length == 2)
            this.AddAxis();
          XmlNodeList xmlNodeList = this.ChartNode.SelectNodes("c:axId", this.NameSpaceManager);
          xmlNodeList[0].Attributes["val"].Value = this.Axis[2].Id;
          xmlNodeList[1].Attributes["val"].Value = this.Axis[3].Id;
          this.XAxis = this.Axis[2];
          this.YAxis = this.Axis[3];
        }
        else
        {
          XmlNodeList xmlNodeList = this.ChartNode.SelectNodes("c:axId", this.NameSpaceManager);
          xmlNodeList[0].Attributes["val"].Value = this.Axis[0].Id;
          xmlNodeList[1].Attributes["val"].Value = this.Axis[1].Id;
          this.XAxis = this.Axis[0];
          this.YAxis = this.Axis[1];
        }
        this._secondaryAxis = value;
      }
    }

    public eChartStyle Style
    {
      get
      {
        XmlNode xmlNode = this.ChartXml.SelectSingleNode("c:chartSpace/c:style/@val", this.NameSpaceManager);
        int result;
        return xmlNode == null || !int.TryParse(xmlNode.Value, out result) ? eChartStyle.None : (eChartStyle) result;
      }
      set
      {
        if (value == eChartStyle.None)
        {
          if (!(this.ChartXml.SelectSingleNode("c:chartSpace/c:style", this.NameSpaceManager) is XmlElement oldChild))
            return;
          oldChild.ParentNode.RemoveChild((XmlNode) oldChild);
        }
        else
        {
          XmlElement element = this.ChartXml.CreateElement("c:style", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          element.SetAttribute("val", ((int) value).ToString());
          XmlElement xmlElement = this.ChartXml.SelectSingleNode("c:chartSpace", this.NameSpaceManager) as XmlElement;
          xmlElement.InsertBefore((XmlNode) element, xmlElement.SelectSingleNode("c:chart", this.NameSpaceManager));
        }
      }
    }

    public bool ShowHiddenData
    {
      get => !this._chartXmlHelper.GetXmlNodeBool("../../c:plotVisOnly/@val");
      set => this._chartXmlHelper.SetXmlNodeBool("../../c:plotVisOnly/@val", !value);
    }

    public eDisplayBlanksAs DisplayBlanksAs
    {
      get
      {
        string xmlNodeString = this._chartXmlHelper.GetXmlNodeString("../../c:dispBlanksAs/@val");
        return string.IsNullOrEmpty(xmlNodeString) ? eDisplayBlanksAs.Gap : (eDisplayBlanksAs) Enum.Parse(typeof (eDisplayBlanksAs), xmlNodeString, true);
      }
      set
      {
        if (value == eDisplayBlanksAs.Gap)
          this._chartSeries.DeleteNode("../../c:dispBlanksAs/@val");
        else
          this._chartSeries.SetXmlNodeString("../../c:dispBlanksAs/@val", value.ToString().ToLower());
      }
    }

    public bool ShowDataLabelsOverMaximum
    {
      get => this._chartXmlHelper.GetXmlNodeBool("../../c:showDLblsOverMax/@val", true);
      set => this._chartXmlHelper.SetXmlNodeBool("../../c:showDLblsOverMax/@val", value, true);
    }

    private bool HasPrimaryAxis()
    {
      if (this._plotArea.ChartTypes.Count == 1)
        return false;
      foreach (ExcelChart chartType in (IEnumerable<ExcelChart>) this._plotArea.ChartTypes)
      {
        if (chartType != this && !chartType.UseSecondaryAxis && !chartType.IsTypePieDoughnut())
          return true;
      }
      return false;
    }

    private void CheckRemoveAxis(ExcelChartAxis excelChartAxis)
    {
      if (!this.ExistsAxis(excelChartAxis))
        return;
      ExcelChartAxis[] excelChartAxisArray = new ExcelChartAxis[this.Axis.Length - 1];
      int index = 0;
      foreach (ExcelChartAxis axi in this.Axis)
      {
        if (axi != excelChartAxis)
          excelChartAxisArray[index] = axi;
      }
      foreach (ExcelChart chartType in (IEnumerable<ExcelChart>) this._plotArea.ChartTypes)
        chartType._axis = excelChartAxisArray;
    }

    private bool ExistsAxis(ExcelChartAxis excelChartAxis)
    {
      foreach (ExcelChart chartType in (IEnumerable<ExcelChart>) this._plotArea.ChartTypes)
      {
        if (chartType != this && (chartType.XAxis.AxisPosition == excelChartAxis.AxisPosition || chartType.YAxis.AxisPosition == excelChartAxis.AxisPosition))
          return true;
      }
      return false;
    }

    public ExcelChartPlotArea PlotArea
    {
      get
      {
        if (this._plotArea == null)
          this._plotArea = new ExcelChartPlotArea(this.NameSpaceManager, this.ChartXml.SelectSingleNode("c:chartSpace/c:chart/c:plotArea", this.NameSpaceManager), this);
        return this._plotArea;
      }
    }

    public ExcelChartLegend Legend
    {
      get
      {
        if (this._legend == null)
          this._legend = new ExcelChartLegend(this.NameSpaceManager, this.ChartXml.SelectSingleNode("c:chartSpace/c:chart/c:legend", this.NameSpaceManager), this);
        return this._legend;
      }
    }

    public ExcelDrawingBorder Border
    {
      get
      {
        if (this._border == null)
          this._border = new ExcelDrawingBorder(this.NameSpaceManager, this.ChartXml.SelectSingleNode("c:chartSpace", this.NameSpaceManager), "c:spPr/a:ln");
        return this._border;
      }
    }

    public ExcelDrawingFill Fill
    {
      get
      {
        if (this._fill == null)
          this._fill = new ExcelDrawingFill(this.NameSpaceManager, this.ChartXml.SelectSingleNode("c:chartSpace", this.NameSpaceManager), "c:spPr");
        return this._fill;
      }
    }

    public ExcelView3D View3D
    {
      get
      {
        if (this.IsType3D())
          return new ExcelView3D(this.NameSpaceManager, this.ChartXml.SelectSingleNode("//c:view3D", this.NameSpaceManager));
        throw new Exception("Charttype does not support 3D");
      }
    }

    public eGrouping Grouping
    {
      get => this.GetGroupingEnum(this._chartXmlHelper.GetXmlNodeString(this._groupingPath));
      internal set
      {
        this._chartXmlHelper.SetXmlNodeString(this._groupingPath, this.GetGroupingText(value));
      }
    }

    public bool VaryColors
    {
      get => this._chartXmlHelper.GetXmlNodeBool(this._varyColorsPath);
      set
      {
        if (value)
          this._chartXmlHelper.SetXmlNodeString(this._varyColorsPath, "1");
        else
          this._chartXmlHelper.SetXmlNodeString(this._varyColorsPath, "0");
      }
    }

    internal ZipPackagePart Part { get; set; }

    internal Uri UriChart { get; set; }

    internal new string Id => "";

    private string GetGroupingText(eGrouping grouping)
    {
      switch (grouping)
      {
        case eGrouping.Clustered:
          return "clustered";
        case eGrouping.Stacked:
          return "stacked";
        case eGrouping.PercentStacked:
          return "percentStacked";
        default:
          return "standard";
      }
    }

    private eGrouping GetGroupingEnum(string grouping)
    {
      switch (grouping)
      {
        case "stacked":
          return eGrouping.Stacked;
        case "percentStacked":
          return eGrouping.PercentStacked;
        default:
          return eGrouping.Clustered;
      }
    }

    internal static ExcelChart GetChart(ExcelDrawings drawings, XmlNode node)
    {
      XmlNode xmlNode = node.SelectSingleNode("xdr:graphicFrame/a:graphic/a:graphicData/c:chart", drawings.NameSpaceManager);
      if (xmlNode == null)
        return (ExcelChart) null;
      ZipPackageRelationship relationship = drawings.Part.GetRelationship(xmlNode.Attributes["r:id"].Value);
      Uri uri = UriHelper.ResolvePartUri(drawings.UriDrawing, relationship.TargetUri);
      ZipPackagePart part = drawings.Part.Package.GetPart(uri);
      XmlDocument xmlDocument = new XmlDocument();
      XmlHelper.LoadXmlSafe(xmlDocument, (Stream) part.GetStream());
      ExcelChart chart1 = (ExcelChart) null;
      foreach (XmlElement childNode in xmlDocument.SelectSingleNode("c:chartSpace/c:chart/c:plotArea", drawings.NameSpaceManager).ChildNodes)
      {
        if (chart1 == null)
        {
          chart1 = ExcelChart.GetChart(childNode, drawings, node, uri, part, xmlDocument, (ExcelChart) null);
          chart1?.PlotArea.ChartTypes.Add(chart1);
        }
        else
        {
          ExcelChart chart2 = ExcelChart.GetChart(childNode, (ExcelDrawings) null, (XmlNode) null, (Uri) null, (ZipPackagePart) null, (XmlDocument) null, chart1);
          if (chart2 != null)
            chart1.PlotArea.ChartTypes.Add(chart2);
        }
      }
      return chart1;
    }

    internal static ExcelChart GetChart(
      XmlElement chartNode,
      ExcelDrawings drawings,
      XmlNode node,
      Uri uriChart,
      ZipPackagePart part,
      XmlDocument chartXml,
      ExcelChart topChart)
    {
      switch (chartNode.LocalName)
      {
        case "area3DChart":
        case "areaChart":
        case "stockChart":
          return topChart == null ? new ExcelChart(drawings, node, uriChart, part, chartXml, (XmlNode) chartNode) : new ExcelChart(topChart, (XmlNode) chartNode);
        case "surface3DChart":
        case "surfaceChart":
          return topChart == null ? (ExcelChart) new ExcelSurfaceChart(drawings, node, uriChart, part, chartXml, (XmlNode) chartNode) : (ExcelChart) new ExcelSurfaceChart(topChart, (XmlNode) chartNode);
        case "radarChart":
          return topChart == null ? (ExcelChart) new ExcelRadarChart(drawings, node, uriChart, part, chartXml, (XmlNode) chartNode) : (ExcelChart) new ExcelRadarChart(topChart, (XmlNode) chartNode);
        case "bubbleChart":
          return topChart == null ? (ExcelChart) new ExcelBubbleChart(drawings, node, uriChart, part, chartXml, (XmlNode) chartNode) : (ExcelChart) new ExcelBubbleChart(topChart, (XmlNode) chartNode);
        case "barChart":
        case "bar3DChart":
          return topChart == null ? (ExcelChart) new ExcelBarChart(drawings, node, uriChart, part, chartXml, (XmlNode) chartNode) : (ExcelChart) new ExcelBarChart(topChart, (XmlNode) chartNode);
        case "doughnutChart":
          return topChart == null ? (ExcelChart) new ExcelDoughnutChart(drawings, node, uriChart, part, chartXml, (XmlNode) chartNode) : (ExcelChart) new ExcelDoughnutChart(topChart, (XmlNode) chartNode);
        case "pie3DChart":
        case "pieChart":
          return topChart == null ? (ExcelChart) new ExcelPieChart(drawings, node, uriChart, part, chartXml, (XmlNode) chartNode) : (ExcelChart) new ExcelPieChart(topChart, (XmlNode) chartNode);
        case "ofPieChart":
          return topChart == null ? (ExcelChart) new ExcelOfPieChart(drawings, node, uriChart, part, chartXml, (XmlNode) chartNode) : (ExcelChart) new ExcelBarChart(topChart, (XmlNode) chartNode);
        case "lineChart":
        case "line3DChart":
          return topChart == null ? (ExcelChart) new ExcelLineChart(drawings, node, uriChart, part, chartXml, (XmlNode) chartNode) : (ExcelChart) new ExcelLineChart(topChart, (XmlNode) chartNode);
        case "scatterChart":
          return topChart == null ? (ExcelChart) new ExcelScatterChart(drawings, node, uriChart, part, chartXml, (XmlNode) chartNode) : (ExcelChart) new ExcelScatterChart(topChart, (XmlNode) chartNode);
        default:
          return (ExcelChart) null;
      }
    }

    internal static ExcelChart GetNewChart(
      ExcelDrawings drawings,
      XmlNode drawNode,
      eChartType chartType,
      ExcelChart topChart,
      ExcelPivotTable PivotTableSource)
    {
      switch (chartType)
      {
        case eChartType.XYScatter:
        case eChartType.XYScatterSmooth:
        case eChartType.XYScatterSmoothNoMarkers:
        case eChartType.XYScatterLines:
        case eChartType.XYScatterLinesNoMarkers:
          return (ExcelChart) new ExcelScatterChart(drawings, drawNode, chartType, topChart, PivotTableSource);
        case eChartType.Radar:
        case eChartType.RadarMarkers:
        case eChartType.RadarFilled:
          return (ExcelChart) new ExcelRadarChart(drawings, drawNode, chartType, topChart, PivotTableSource);
        case eChartType.Doughnut:
        case eChartType.DoughnutExploded:
          return (ExcelChart) new ExcelDoughnutChart(drawings, drawNode, chartType, topChart, PivotTableSource);
        case eChartType.Pie3D:
        case eChartType.Pie:
        case eChartType.PieExploded:
        case eChartType.PieExploded3D:
          return (ExcelChart) new ExcelPieChart(drawings, drawNode, chartType, topChart, PivotTableSource);
        case eChartType.Line3D:
        case eChartType.Line:
        case eChartType.LineStacked:
        case eChartType.LineStacked100:
        case eChartType.LineMarkers:
        case eChartType.LineMarkersStacked:
        case eChartType.LineMarkersStacked100:
          return (ExcelChart) new ExcelLineChart(drawings, drawNode, chartType, topChart, PivotTableSource);
        case eChartType.Column3D:
        case eChartType.ColumnClustered:
        case eChartType.ColumnStacked:
        case eChartType.ColumnStacked100:
        case eChartType.ColumnClustered3D:
        case eChartType.ColumnStacked3D:
        case eChartType.ColumnStacked1003D:
        case eChartType.BarClustered:
        case eChartType.BarStacked:
        case eChartType.BarStacked100:
        case eChartType.BarClustered3D:
        case eChartType.BarStacked3D:
        case eChartType.BarStacked1003D:
        case eChartType.CylinderColClustered:
        case eChartType.CylinderColStacked:
        case eChartType.CylinderColStacked100:
        case eChartType.CylinderBarClustered:
        case eChartType.CylinderBarStacked:
        case eChartType.CylinderBarStacked100:
        case eChartType.CylinderCol:
        case eChartType.ConeColClustered:
        case eChartType.ConeColStacked:
        case eChartType.ConeColStacked100:
        case eChartType.ConeBarClustered:
        case eChartType.ConeBarStacked:
        case eChartType.ConeBarStacked100:
        case eChartType.ConeCol:
        case eChartType.PyramidColClustered:
        case eChartType.PyramidColStacked:
        case eChartType.PyramidColStacked100:
        case eChartType.PyramidBarClustered:
        case eChartType.PyramidBarStacked:
        case eChartType.PyramidBarStacked100:
        case eChartType.PyramidCol:
          return (ExcelChart) new ExcelBarChart(drawings, drawNode, chartType, topChart, PivotTableSource);
        case eChartType.Bubble:
        case eChartType.Bubble3DEffect:
          return (ExcelChart) new ExcelBubbleChart(drawings, drawNode, chartType, topChart, PivotTableSource);
        case eChartType.PieOfPie:
        case eChartType.BarOfPie:
          return (ExcelChart) new ExcelOfPieChart(drawings, drawNode, chartType, topChart, PivotTableSource);
        case eChartType.Surface:
        case eChartType.SurfaceWireframe:
        case eChartType.SurfaceTopView:
        case eChartType.SurfaceTopViewWireframe:
          return (ExcelChart) new ExcelSurfaceChart(drawings, drawNode, chartType, topChart, PivotTableSource);
        default:
          return new ExcelChart(drawings, drawNode, chartType, topChart, PivotTableSource);
      }
    }

    public ExcelPivotTable PivotTableSource { get; private set; }

    internal void SetPivotSource(ExcelPivotTable pivotTableSource)
    {
      this.PivotTableSource = pivotTableSource;
      XmlElement refChild = this.ChartXml.SelectSingleNode("c:chartSpace/c:chart", this.NameSpaceManager) as XmlElement;
      XmlElement element1 = this.ChartXml.CreateElement("pivotSource", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      refChild.ParentNode.InsertBefore((XmlNode) element1, (XmlNode) refChild);
      element1.InnerXml = string.Format("<c:name>[]{0}!{1}</c:name><c:fmtId val=\"0\"/>", (object) this.PivotTableSource.WorkSheet.Name, (object) pivotTableSource.Name);
      XmlElement element2 = this.ChartXml.CreateElement("pivotFmts", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      refChild.PrependChild((XmlNode) element2);
      element2.InnerXml = "<c:pivotFmt><c:idx val=\"0\"/><c:marker><c:symbol val=\"none\"/></c:marker></c:pivotFmt>";
      this.Series.AddPivotSerie(pivotTableSource);
    }

    internal override void DeleteMe()
    {
      try
      {
        this.Part.Package.DeletePart(this.UriChart);
      }
      catch (Exception ex)
      {
        throw new InvalidDataException("EPPlus internal error when deleteing chart.", ex);
      }
      base.DeleteMe();
    }
  }
}
