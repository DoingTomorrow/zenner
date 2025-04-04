// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelBubbleChart
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
  public sealed class ExcelBubbleChart : ExcelChart
  {
    private string BUBBLESCALE_PATH = "c:bubbleScale/@val";
    private string SHOWNEGBUBBLES_PATH = "c:showNegBubbles/@val";
    private string BUBBLE3D_PATH = "c:bubble3D/@val";
    private string SIZEREPRESENTS_PATH = "c:sizeRepresents/@val";
    private ExcelBubbleChartSeries _series;

    internal ExcelBubbleChart(
      ExcelDrawings drawings,
      XmlNode node,
      eChartType type,
      ExcelChart topChart,
      ExcelPivotTable PivotTableSource)
      : base(drawings, node, type, topChart, PivotTableSource)
    {
      this.ShowNegativeBubbles = false;
      this.BubbleScale = 100;
      this._chartSeries = (ExcelChartSeries) new ExcelBubbleChartSeries((ExcelChart) this, drawings.NameSpaceManager, this._chartNode, PivotTableSource != null);
    }

    internal ExcelBubbleChart(ExcelDrawings drawings, XmlNode node, eChartType type, bool isPivot)
      : base(drawings, node, type, isPivot)
    {
      this._chartSeries = (ExcelChartSeries) new ExcelBubbleChartSeries((ExcelChart) this, drawings.NameSpaceManager, this._chartNode, isPivot);
    }

    internal ExcelBubbleChart(
      ExcelDrawings drawings,
      XmlNode node,
      Uri uriChart,
      ZipPackagePart part,
      XmlDocument chartXml,
      XmlNode chartNode)
      : base(drawings, node, uriChart, part, chartXml, chartNode)
    {
      this._chartSeries = (ExcelChartSeries) new ExcelBubbleChartSeries((ExcelChart) this, this._drawings.NameSpaceManager, this._chartNode, false);
    }

    internal ExcelBubbleChart(ExcelChart topChart, XmlNode chartNode)
      : base(topChart, chartNode)
    {
      this._chartSeries = (ExcelChartSeries) new ExcelBubbleChartSeries((ExcelChart) this, this._drawings.NameSpaceManager, this._chartNode, false);
    }

    public int BubbleScale
    {
      get => this._chartXmlHelper.GetXmlNodeInt(this.BUBBLESCALE_PATH);
      set
      {
        if (value < 0 && value > 300)
          throw new ArgumentOutOfRangeException("Bubblescale out of range. 0-300 allowed");
        this._chartXmlHelper.SetXmlNodeString(this.BUBBLESCALE_PATH, value.ToString());
      }
    }

    public bool ShowNegativeBubbles
    {
      get => this._chartXmlHelper.GetXmlNodeBool(this.SHOWNEGBUBBLES_PATH);
      set => this._chartXmlHelper.SetXmlNodeBool(this.BUBBLESCALE_PATH, value, true);
    }

    public bool Bubble3D
    {
      get => this._chartXmlHelper.GetXmlNodeBool(this.BUBBLE3D_PATH);
      set
      {
        this._chartXmlHelper.SetXmlNodeBool(this.BUBBLE3D_PATH, value);
        this.ChartType = value ? eChartType.Bubble3DEffect : eChartType.Bubble;
      }
    }

    public eSizeRepresents SizeRepresents
    {
      get
      {
        return this._chartXmlHelper.GetXmlNodeString(this.SIZEREPRESENTS_PATH).ToLower() == "w" ? eSizeRepresents.Width : eSizeRepresents.Area;
      }
      set
      {
        this._chartXmlHelper.SetXmlNodeString(this.SIZEREPRESENTS_PATH, value == eSizeRepresents.Width ? "w" : "area");
      }
    }

    public ExcelBubbleChartSeries Series => (ExcelBubbleChartSeries) this._chartSeries;

    internal override eChartType GetChartType(string name)
    {
      return this.Bubble3D ? eChartType.Bubble3DEffect : eChartType.Bubble;
    }
  }
}
