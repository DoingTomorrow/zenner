// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelScatterChart
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
  public sealed class ExcelScatterChart : ExcelChart
  {
    private string _scatterTypePath = "c:scatterStyle/@val";
    private string MARKER_PATH = "c:marker/@val";

    internal ExcelScatterChart(
      ExcelDrawings drawings,
      XmlNode node,
      eChartType type,
      ExcelChart topChart,
      ExcelPivotTable PivotTableSource)
      : base(drawings, node, type, topChart, PivotTableSource)
    {
      this.SetTypeProperties();
    }

    internal ExcelScatterChart(
      ExcelDrawings drawings,
      XmlNode node,
      Uri uriChart,
      ZipPackagePart part,
      XmlDocument chartXml,
      XmlNode chartNode)
      : base(drawings, node, uriChart, part, chartXml, chartNode)
    {
      this.SetTypeProperties();
    }

    internal ExcelScatterChart(ExcelChart topChart, XmlNode chartNode)
      : base(topChart, chartNode)
    {
      this.SetTypeProperties();
    }

    private void SetTypeProperties()
    {
      if (this.ChartType == eChartType.XYScatter || this.ChartType == eChartType.XYScatterLines || this.ChartType == eChartType.XYScatterLinesNoMarkers)
      {
        this.ScatterStyle = eScatterStyle.LineMarker;
      }
      else
      {
        if (this.ChartType != eChartType.XYScatterSmooth && this.ChartType != eChartType.XYScatterSmoothNoMarkers)
          return;
        this.ScatterStyle = eScatterStyle.SmoothMarker;
      }
    }

    private eScatterStyle GetScatterEnum(string text)
    {
      switch (text)
      {
        case "smoothMarker":
          return eScatterStyle.SmoothMarker;
        default:
          return eScatterStyle.LineMarker;
      }
    }

    private string GetScatterText(eScatterStyle shatterStyle)
    {
      return shatterStyle == eScatterStyle.SmoothMarker ? "smoothMarker" : "lineMarker";
    }

    public eScatterStyle ScatterStyle
    {
      get => this.GetScatterEnum(this._chartXmlHelper.GetXmlNodeString(this._scatterTypePath));
      internal set
      {
        this._chartXmlHelper.CreateNode(this._scatterTypePath, true);
        this._chartXmlHelper.SetXmlNodeString(this._scatterTypePath, this.GetScatterText(value));
      }
    }

    public bool Marker
    {
      get => this.GetXmlNodeBool(this.MARKER_PATH, false);
      set => this.SetXmlNodeBool(this.MARKER_PATH, value, false);
    }

    internal override eChartType GetChartType(string name)
    {
      if (name == "scatterChart")
      {
        if (this.ScatterStyle == eScatterStyle.LineMarker)
        {
          if (((ExcelScatterChartSerie) this.Series[0]).Marker == eMarkerStyle.None)
            return eChartType.XYScatterLinesNoMarkers;
          return this.ExistNode("c:ser/c:spPr/a:ln/noFill") ? eChartType.XYScatter : eChartType.XYScatterLines;
        }
        if (this.ScatterStyle == eScatterStyle.SmoothMarker)
          return ((ExcelScatterChartSerie) this.Series[0]).Marker == eMarkerStyle.None ? eChartType.XYScatterSmoothNoMarkers : eChartType.XYScatterSmooth;
      }
      return base.GetChartType(name);
    }
  }
}
