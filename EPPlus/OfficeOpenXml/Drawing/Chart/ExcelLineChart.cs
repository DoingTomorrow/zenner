// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelLineChart
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public class ExcelLineChart : ExcelChart
  {
    private string MARKER_PATH = "c:marker/@val";
    private string SMOOTH_PATH = "c:smooth/@val";
    private ExcelChartDataLabel _DataLabel;

    internal ExcelLineChart(
      ExcelDrawings drawings,
      XmlNode node,
      Uri uriChart,
      ZipPackagePart part,
      XmlDocument chartXml,
      XmlNode chartNode)
      : base(drawings, node, uriChart, part, chartXml, chartNode)
    {
    }

    internal ExcelLineChart(ExcelChart topChart, XmlNode chartNode)
      : base(topChart, chartNode)
    {
    }

    internal ExcelLineChart(
      ExcelDrawings drawings,
      XmlNode node,
      eChartType type,
      ExcelChart topChart,
      ExcelPivotTable PivotTableSource)
      : base(drawings, node, type, topChart, PivotTableSource)
    {
      this.Smooth = false;
    }

    public bool Marker
    {
      get => this._chartXmlHelper.GetXmlNodeBool(this.MARKER_PATH, false);
      set => this._chartXmlHelper.SetXmlNodeBool(this.MARKER_PATH, value, false);
    }

    public bool Smooth
    {
      get => this._chartXmlHelper.GetXmlNodeBool(this.SMOOTH_PATH, false);
      set => this._chartXmlHelper.SetXmlNodeBool(this.SMOOTH_PATH, value);
    }

    public ExcelChartDataLabel DataLabel
    {
      get
      {
        if (this._DataLabel == null)
          this._DataLabel = new ExcelChartDataLabel(this.NameSpaceManager, this.ChartNode);
        return this._DataLabel;
      }
    }

    internal override eChartType GetChartType(string name)
    {
      switch (name)
      {
        case "lineChart":
          if (this.Marker)
          {
            if (this.Grouping == eGrouping.Stacked)
              return eChartType.LineMarkersStacked;
            return this.Grouping == eGrouping.PercentStacked ? eChartType.LineMarkersStacked100 : eChartType.LineMarkers;
          }
          if (this.Grouping == eGrouping.Stacked)
            return eChartType.LineStacked;
          return this.Grouping == eGrouping.PercentStacked ? eChartType.LineStacked100 : eChartType.Line;
        case "line3DChart":
          return eChartType.Line3D;
        default:
          return base.GetChartType(name);
      }
    }
  }
}
