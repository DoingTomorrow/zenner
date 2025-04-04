// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelBubbleChartSerie
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Text;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public sealed class ExcelBubbleChartSerie : ExcelChartSerie
  {
    private const string BUBBLE3D_PATH = "c:bubble3D/@val";
    private const string BUBBLESIZE_TOPPATH = "c:bubbleSize";
    private const string BUBBLESIZE_PATH = "c:bubbleSize/c:numRef/c:f";
    private ExcelChartSerieDataLabel _DataLabel;

    internal ExcelBubbleChartSerie(
      ExcelChartSeries chartSeries,
      XmlNamespaceManager ns,
      XmlNode node,
      bool isPivot)
      : base(chartSeries, ns, node, isPivot)
    {
      int chartType = (int) chartSeries.Chart.ChartType;
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

    internal bool Bubble3D
    {
      get => this.GetXmlNodeBool("c:bubble3D/@val", true);
      set => this.SetXmlNodeBool("c:bubble3D/@val", value);
    }

    public override string Series
    {
      get => base.Series;
      set
      {
        base.Series = value;
        if (!string.IsNullOrEmpty(this.BubbleSize))
          return;
        this.GenerateLit();
      }
    }

    public string BubbleSize
    {
      get => this.GetXmlNodeString("c:bubbleSize/c:numRef/c:f");
      set
      {
        if (string.IsNullOrEmpty(value))
        {
          this.GenerateLit();
        }
        else
        {
          this.SetXmlNodeString("c:bubbleSize/c:numRef/c:f", ExcelCellBase.GetFullAddress(this._chartSeries.Chart.WorkSheet.Name, value));
          XmlNode oldChild = this.TopNode.SelectSingleNode(string.Format("{0}/c:numCache", (object) "c:bubbleSize/c:numRef/c:f"), this._ns);
          oldChild?.ParentNode.RemoveChild(oldChild);
          this.DeleteNode(string.Format("{0}/c:numLit", (object) "c:bubbleSize"));
        }
      }
    }

    internal void GenerateLit()
    {
      ExcelAddress excelAddress = new ExcelAddress(this.Series);
      int num = 0;
      StringBuilder stringBuilder = new StringBuilder();
      for (int fromRow = excelAddress._fromRow; fromRow <= excelAddress._toRow; ++fromRow)
      {
        for (int fromCol = excelAddress._fromCol; fromCol <= excelAddress._toCol; ++fromCol)
          stringBuilder.AppendFormat("<c:pt idx=\"{0}\"><c:v>1</c:v></c:pt>", (object) num++);
      }
      this.CreateNode("c:bubbleSize/c:numLit", true);
      this.TopNode.SelectSingleNode(string.Format("{0}/c:numLit", (object) "c:bubbleSize"), this._ns).InnerXml = string.Format("<c:formatCode>General</c:formatCode><c:ptCount val=\"{0}\"/>{1}", (object) num, (object) stringBuilder.ToString());
    }
  }
}
