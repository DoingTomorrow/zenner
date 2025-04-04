// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelPieChart
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
  public class ExcelPieChart : ExcelChart
  {
    private ExcelChartDataLabel _DataLabel;

    internal ExcelPieChart(ExcelDrawings drawings, XmlNode node, eChartType type, bool isPivot)
      : base(drawings, node, type, isPivot)
    {
    }

    internal ExcelPieChart(
      ExcelDrawings drawings,
      XmlNode node,
      eChartType type,
      ExcelChart topChart,
      ExcelPivotTable PivotTableSource)
      : base(drawings, node, type, topChart, PivotTableSource)
    {
    }

    internal ExcelPieChart(
      ExcelDrawings drawings,
      XmlNode node,
      Uri uriChart,
      ZipPackagePart part,
      XmlDocument chartXml,
      XmlNode chartNode)
      : base(drawings, node, uriChart, part, chartXml, chartNode)
    {
    }

    internal ExcelPieChart(ExcelChart topChart, XmlNode chartNode)
      : base(topChart, chartNode)
    {
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
        case "pieChart":
          return this.Series.Count > 0 && ((ExcelPieChartSerie) this.Series[0]).Explosion > 0 ? eChartType.PieExploded : eChartType.Pie;
        case "pie3DChart":
          return this.Series.Count > 0 && ((ExcelPieChartSerie) this.Series[0]).Explosion > 0 ? eChartType.PieExploded3D : eChartType.Pie3D;
        default:
          return base.GetChartType(name);
      }
    }
  }
}
