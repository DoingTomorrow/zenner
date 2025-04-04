// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelChartSerie
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public class ExcelChartSerie : XmlHelper
  {
    private const string headerPath = "c:tx/c:v";
    private const string headerAddressPath = "c:tx/c:strRef/c:f";
    internal ExcelChartSeries _chartSeries;
    protected XmlNode _node;
    protected XmlNamespaceManager _ns;
    private string _seriesTopPath;
    private string _seriesPath = "{0}/c:numRef/c:f";
    private string _xSeriesTopPath;
    private string _xSeriesPath = "{0}/{1}/c:f";
    private ExcelChartTrendlineCollection _trendLines;

    internal ExcelChartSerie(
      ExcelChartSeries chartSeries,
      XmlNamespaceManager ns,
      XmlNode node,
      bool isPivot)
      : base(ns, node)
    {
      this._chartSeries = chartSeries;
      this._node = node;
      this._ns = ns;
      this.SchemaNodeOrder = new string[16]
      {
        "idx",
        "order",
        "spPr",
        "tx",
        "marker",
        "trendline",
        "explosion",
        "invertIfNegative",
        "dLbls",
        "cat",
        "val",
        "xVal",
        "yVal",
        "bubbleSize",
        "bubble3D",
        "smooth"
      };
      if (chartSeries.Chart.ChartType == eChartType.XYScatter || chartSeries.Chart.ChartType == eChartType.XYScatterLines || chartSeries.Chart.ChartType == eChartType.XYScatterLinesNoMarkers || chartSeries.Chart.ChartType == eChartType.XYScatterSmooth || chartSeries.Chart.ChartType == eChartType.XYScatterSmoothNoMarkers || chartSeries.Chart.ChartType == eChartType.Bubble || chartSeries.Chart.ChartType == eChartType.Bubble3DEffect)
      {
        this._seriesTopPath = "c:yVal";
        this._xSeriesTopPath = "c:xVal";
      }
      else
      {
        this._seriesTopPath = "c:val";
        this._xSeriesTopPath = "c:cat";
      }
      this._seriesPath = string.Format(this._seriesPath, (object) this._seriesTopPath);
      this._xSeriesPath = string.Format(this._xSeriesPath, (object) this._xSeriesTopPath, isPivot ? (object) "c:multiLvlStrRef" : (object) "c:numRef");
    }

    internal void SetID(string id)
    {
      this.SetXmlNodeString("c:idx/@val", id);
      this.SetXmlNodeString("c:order/@val", id);
    }

    public string Header
    {
      get => this.GetXmlNodeString("c:tx/c:v");
      set
      {
        this.Cleartx();
        this.SetXmlNodeString("c:tx/c:v", value);
      }
    }

    private void Cleartx()
    {
      XmlNode xmlNode = this.TopNode.SelectSingleNode("c:tx", this.NameSpaceManager);
      if (xmlNode == null)
        return;
      xmlNode.InnerXml = "";
    }

    public ExcelAddressBase HeaderAddress
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("c:tx/c:strRef/c:f");
        return xmlNodeString == "" ? (ExcelAddressBase) null : new ExcelAddressBase(xmlNodeString);
      }
      set
      {
        if (value._fromCol != value._toCol || value._fromRow != value._toRow || value.Addresses != null)
          throw new Exception("Address must be a single cell");
        this.Cleartx();
        this.SetXmlNodeString("c:tx/c:strRef/c:f", ExcelCellBase.GetFullAddress(value.WorkSheet, value.Address));
        this.SetXmlNodeString("c:tx/c:strRef/c:strCache/c:ptCount/@val", "0");
      }
    }

    public virtual string Series
    {
      get => this.GetXmlNodeString(this._seriesPath);
      set
      {
        this.CreateNode(this._seriesPath, true);
        this.SetXmlNodeString(this._seriesPath, ExcelCellBase.GetFullAddress(this._chartSeries.Chart.WorkSheet.Name, value));
        XmlNode oldChild1 = this.TopNode.SelectSingleNode(string.Format("{0}/c:numRef/c:numCache", (object) this._seriesTopPath), this._ns);
        oldChild1?.ParentNode.RemoveChild(oldChild1);
        if (this._chartSeries.Chart.PivotTableSource != null)
          this.SetXmlNodeString(string.Format("{0}/c:numRef/c:numCache", (object) this._seriesTopPath), "General");
        XmlNode oldChild2 = this.TopNode.SelectSingleNode(string.Format("{0}/c:numLit", (object) this._seriesTopPath), this._ns);
        oldChild2?.ParentNode.RemoveChild(oldChild2);
      }
    }

    public virtual string XSeries
    {
      get => this.GetXmlNodeString(this._xSeriesPath);
      set
      {
        this.CreateNode(this._xSeriesPath, true);
        this.SetXmlNodeString(this._xSeriesPath, ExcelCellBase.GetFullAddress(this._chartSeries.Chart.WorkSheet.Name, value));
        XmlNode oldChild1 = this.TopNode.SelectSingleNode(string.Format("{0}/c:numRef/c:numCache", (object) this._xSeriesTopPath), this._ns);
        oldChild1?.ParentNode.RemoveChild(oldChild1);
        XmlNode oldChild2 = this.TopNode.SelectSingleNode(string.Format("{0}/c:numLit", (object) this._xSeriesTopPath), this._ns);
        oldChild2?.ParentNode.RemoveChild(oldChild2);
      }
    }

    public ExcelChartTrendlineCollection TrendLines
    {
      get
      {
        if (this._trendLines == null)
          this._trendLines = new ExcelChartTrendlineCollection(this);
        return this._trendLines;
      }
    }
  }
}
