// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelChartCollection
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
  public class ExcelChartCollection : IEnumerable<ExcelChart>, IEnumerable
  {
    private List<ExcelChart> _list = new List<ExcelChart>();
    private ExcelChart _topChart;

    internal ExcelChartCollection(ExcelChart chart)
    {
      this._topChart = chart;
      this._list.Add(chart);
    }

    internal void Add(ExcelChart chart) => this._list.Add(chart);

    public ExcelChart Add(eChartType chartType)
    {
      if (this._topChart.PivotTableSource != null)
        throw new InvalidOperationException("Can not add other charttypes to a pivot chart");
      if (ExcelChart.IsType3D(chartType) || this._list[0].IsType3D())
        throw new InvalidOperationException("3D charts can not be combined with other charttypes");
      XmlNode topNode = this._list[this._list.Count - 1].TopNode;
      ExcelChart newChart = ExcelChart.GetNewChart(this._topChart.WorkSheet.Drawings, this._topChart.TopNode, chartType, this._topChart, (ExcelPivotTable) null);
      this._list.Add(newChart);
      return newChart;
    }

    public int Count => this._list.Count;

    IEnumerator<ExcelChart> IEnumerable<ExcelChart>.GetEnumerator()
    {
      return (IEnumerator<ExcelChart>) this._list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();

    public ExcelChart this[int PositionID] => this._list[PositionID];
  }
}
