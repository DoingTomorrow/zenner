// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelChartTrendlineCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public class ExcelChartTrendlineCollection : IEnumerable<ExcelChartTrendline>, IEnumerable
  {
    private List<ExcelChartTrendline> _list = new List<ExcelChartTrendline>();
    private ExcelChartSerie _serie;

    internal ExcelChartTrendlineCollection(ExcelChartSerie serie)
    {
      this._serie = serie;
      foreach (XmlNode selectNode in this._serie.TopNode.SelectNodes("c:trendline", this._serie.NameSpaceManager))
        this._list.Add(new ExcelChartTrendline(this._serie.NameSpaceManager, selectNode));
    }

    public ExcelChartTrendline Add(eTrendLine Type)
    {
      if (this._serie._chartSeries._chart.IsType3D() || this._serie._chartSeries._chart.IsTypePercentStacked() || this._serie._chartSeries._chart.IsTypeStacked() || this._serie._chartSeries._chart.IsTypePieDoughnut())
        throw new ArgumentException("Trendlines don't apply to 3d-charts, stacked charts, pie charts or doughnut charts");
      XmlNode refChild = this._list.Count <= 0 ? this._serie.TopNode.SelectSingleNode("c:marker", this._serie.NameSpaceManager) ?? this._serie.TopNode.SelectSingleNode("c:tx", this._serie.NameSpaceManager) ?? this._serie.TopNode.SelectSingleNode("c:order", this._serie.NameSpaceManager) : this._list[this._list.Count - 1].TopNode;
      XmlElement element = this._serie.TopNode.OwnerDocument.CreateElement("c", "trendline", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      this._serie.TopNode.InsertAfter((XmlNode) element, refChild);
      return new ExcelChartTrendline(this._serie.NameSpaceManager, (XmlNode) element)
      {
        Type = Type
      };
    }

    IEnumerator<ExcelChartTrendline> IEnumerable<ExcelChartTrendline>.GetEnumerator()
    {
      return (IEnumerator<ExcelChartTrendline>) this._list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();
  }
}
