// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelOfPieChart
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
  public class ExcelOfPieChart : ExcelPieChart
  {
    private const string pieTypePath = "c:chartSpace/c:chart/c:plotArea/c:ofPieChart/c:ofPieType/@val";

    internal ExcelOfPieChart(ExcelDrawings drawings, XmlNode node, eChartType type, bool isPivot)
      : base(drawings, node, type, isPivot)
    {
      this.SetTypeProperties();
    }

    internal ExcelOfPieChart(
      ExcelDrawings drawings,
      XmlNode node,
      eChartType type,
      ExcelChart topChart,
      ExcelPivotTable PivotTableSource)
      : base(drawings, node, type, topChart, PivotTableSource)
    {
      this.SetTypeProperties();
    }

    internal ExcelOfPieChart(
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

    private void SetTypeProperties()
    {
      if (this.ChartType == eChartType.BarOfPie)
        this.OfPieType = ePieType.Bar;
      else
        this.OfPieType = ePieType.Pie;
    }

    public ePieType OfPieType
    {
      get
      {
        return this._chartXmlHelper.GetXmlNodeString("c:chartSpace/c:chart/c:plotArea/c:ofPieChart/c:ofPieType/@val") == "bar" ? ePieType.Bar : ePieType.Pie;
      }
      internal set
      {
        this._chartXmlHelper.CreateNode("c:chartSpace/c:chart/c:plotArea/c:ofPieChart/c:ofPieType/@val", true);
        this._chartXmlHelper.SetXmlNodeString("c:chartSpace/c:chart/c:plotArea/c:ofPieChart/c:ofPieType/@val", value == ePieType.Bar ? "bar" : "pie");
      }
    }

    internal override eChartType GetChartType(string name)
    {
      if (!(name == "ofPieChart"))
        return base.GetChartType(name);
      return this.OfPieType == ePieType.Bar ? eChartType.BarOfPie : eChartType.PieOfPie;
    }
  }
}
