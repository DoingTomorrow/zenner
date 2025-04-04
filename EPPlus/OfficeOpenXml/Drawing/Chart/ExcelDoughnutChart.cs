// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelDoughnutChart
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public class ExcelDoughnutChart : ExcelPieChart
  {
    private string _firstSliceAngPath = "c:firstSliceAng/@val";
    private string _holeSizePath = "c:holeSize/@val";

    internal ExcelDoughnutChart(
      ExcelDrawings drawings,
      XmlNode node,
      eChartType type,
      bool isPivot)
      : base(drawings, node, type, isPivot)
    {
    }

    internal ExcelDoughnutChart(
      ExcelDrawings drawings,
      XmlNode node,
      eChartType type,
      ExcelChart topChart,
      ExcelPivotTable PivotTableSource)
      : base(drawings, node, type, topChart, PivotTableSource)
    {
    }

    internal ExcelDoughnutChart(
      ExcelDrawings drawings,
      XmlNode node,
      Uri uriChart,
      ZipPackagePart part,
      XmlDocument chartXml,
      XmlNode chartNode)
      : base(drawings, node, uriChart, part, chartXml, chartNode)
    {
    }

    internal ExcelDoughnutChart(ExcelChart topChart, XmlNode chartNode)
      : base(topChart, chartNode)
    {
    }

    public Decimal FirstSliceAngle
    {
      get => this._chartXmlHelper.GetXmlNodeDecimal(this._firstSliceAngPath);
      internal set
      {
        this._chartXmlHelper.SetXmlNodeString(this._firstSliceAngPath, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public Decimal HoleSize
    {
      get => this._chartXmlHelper.GetXmlNodeDecimal(this._holeSizePath);
      internal set
      {
        this._chartXmlHelper.SetXmlNodeString(this._holeSizePath, value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    internal override eChartType GetChartType(string name)
    {
      if (!(name == "doughnutChart"))
        return base.GetChartType(name);
      return ((ExcelPieChartSerie) this.Series[0]).Explosion > 0 ? eChartType.DoughnutExploded : eChartType.Doughnut;
    }
  }
}
