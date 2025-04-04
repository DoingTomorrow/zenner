// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelRadarChart
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
  public class ExcelRadarChart : ExcelChart
  {
    private string STYLE_PATH = "c:radarStyle/@val";
    private ExcelChartDataLabel _DataLabel;

    internal ExcelRadarChart(
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

    internal ExcelRadarChart(ExcelChart topChart, XmlNode chartNode)
      : base(topChart, chartNode)
    {
      this.SetTypeProperties();
    }

    internal ExcelRadarChart(
      ExcelDrawings drawings,
      XmlNode node,
      eChartType type,
      ExcelChart topChart,
      ExcelPivotTable PivotTableSource)
      : base(drawings, node, type, topChart, PivotTableSource)
    {
      this.SetTypeProperties();
    }

    private void SetTypeProperties()
    {
      if (this.ChartType == eChartType.RadarFilled)
        this.RadarStyle = eRadarStyle.Filled;
      else if (this.ChartType == eChartType.RadarMarkers)
        this.RadarStyle = eRadarStyle.Marker;
      else
        this.RadarStyle = eRadarStyle.Standard;
    }

    public eRadarStyle RadarStyle
    {
      get
      {
        string xmlNodeString = this._chartXmlHelper.GetXmlNodeString(this.STYLE_PATH);
        return string.IsNullOrEmpty(xmlNodeString) ? eRadarStyle.Standard : (eRadarStyle) Enum.Parse(typeof (eRadarStyle), xmlNodeString, true);
      }
      set => this._chartXmlHelper.SetXmlNodeString(this.STYLE_PATH, value.ToString().ToLower());
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
      if (this.RadarStyle == eRadarStyle.Filled)
        return eChartType.RadarFilled;
      return this.RadarStyle == eRadarStyle.Marker ? eChartType.RadarMarkers : eChartType.Radar;
    }
  }
}
