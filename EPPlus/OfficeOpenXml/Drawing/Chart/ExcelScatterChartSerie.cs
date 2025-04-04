// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelScatterChartSerie
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public sealed class ExcelScatterChartSerie : ExcelChartSerie
  {
    private const string smoothPath = "c:smooth/@val";
    private const string markerPath = "c:marker/c:symbol/@val";
    private ExcelChartSerieDataLabel _DataLabel;

    internal ExcelScatterChartSerie(
      ExcelChartSeries chartSeries,
      XmlNamespaceManager ns,
      XmlNode node,
      bool isPivot)
      : base(chartSeries, ns, node, isPivot)
    {
      if (chartSeries.Chart.ChartType == eChartType.XYScatterLines || chartSeries.Chart.ChartType == eChartType.XYScatterSmooth)
        this.Marker = eMarkerStyle.Square;
      if (chartSeries.Chart.ChartType == eChartType.XYScatterSmooth || chartSeries.Chart.ChartType == eChartType.XYScatterSmoothNoMarkers)
        this.Smooth = 1;
      else
        this.Smooth = 0;
    }

    public ExcelChartSerieDataLabel DataLabel
    {
      get
      {
        if (this._DataLabel == null)
          this._DataLabel = new ExcelChartSerieDataLabel(this._ns, this._node);
        return this._DataLabel;
      }
    }

    public int Smooth
    {
      get => this.GetXmlNodeInt("c:smooth/@val");
      internal set => this.SetXmlNodeString("c:smooth/@val", value.ToString());
    }

    public eMarkerStyle Marker
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("c:marker/c:symbol/@val");
        return xmlNodeString == "" ? eMarkerStyle.None : (eMarkerStyle) Enum.Parse(typeof (eMarkerStyle), xmlNodeString, true);
      }
      internal set => this.SetXmlNodeString("c:marker/c:symbol/@val", value.ToString().ToLower());
    }
  }
}
