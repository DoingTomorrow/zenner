// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelRadarChartSerie
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public sealed class ExcelRadarChartSerie : ExcelChartSerie
  {
    private const string markerPath = "c:marker/c:symbol/@val";
    private const string MARKERSIZE_PATH = "c:marker/c:size/@val";
    private ExcelChartSerieDataLabel _DataLabel;

    internal ExcelRadarChartSerie(
      ExcelChartSeries chartSeries,
      XmlNamespaceManager ns,
      XmlNode node,
      bool isPivot)
      : base(chartSeries, ns, node, isPivot)
    {
      if (chartSeries.Chart.ChartType != eChartType.Radar)
        return;
      this.Marker = eMarkerStyle.None;
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

    public eMarkerStyle Marker
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("c:marker/c:symbol/@val");
        return xmlNodeString == "" || xmlNodeString == "none" ? eMarkerStyle.None : (eMarkerStyle) Enum.Parse(typeof (eMarkerStyle), xmlNodeString, true);
      }
      internal set => this.SetXmlNodeString("c:marker/c:symbol/@val", value.ToString().ToLower());
    }

    public int MarkerSize
    {
      get => this.GetXmlNodeInt("c:marker/c:size/@val");
      set
      {
        if (value < 2 && value > 72)
          throw new ArgumentOutOfRangeException("MarkerSize out of range. Range from 2-72 allowed.");
        this.SetXmlNodeString("c:marker/c:size/@val", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }
  }
}
