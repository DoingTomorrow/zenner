// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelPieChartSerie
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public sealed class ExcelPieChartSerie : ExcelChartSerie
  {
    private const string explosionPath = "c:explosion/@val";
    private ExcelChartSerieDataLabel _DataLabel;

    internal ExcelPieChartSerie(
      ExcelChartSeries chartSeries,
      XmlNamespaceManager ns,
      XmlNode node,
      bool isPivot)
      : base(chartSeries, ns, node, isPivot)
    {
    }

    public int Explosion
    {
      get => this.GetXmlNodeInt("c:explosion/@val");
      set
      {
        if (value < 0 || value > 400)
          throw new ArgumentOutOfRangeException("Explosion range is 0-400");
        this.SetXmlNodeString("c:explosion/@val", value.ToString());
      }
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
  }
}
