// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelBubbleChartSeries
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public sealed class ExcelBubbleChartSeries : ExcelChartSeries
  {
    internal ExcelBubbleChartSeries(
      ExcelChart chart,
      XmlNamespaceManager ns,
      XmlNode node,
      bool isPivot)
      : base(chart, ns, node, isPivot)
    {
    }

    public ExcelChartSerie Add(
      ExcelRangeBase Serie,
      ExcelRangeBase XSerie,
      ExcelRangeBase BubbleSize)
    {
      return this.AddSeries(Serie.FullAddressAbsolute, XSerie.FullAddressAbsolute, BubbleSize.FullAddressAbsolute);
    }

    public ExcelChartSerie Add(string SerieAddress, string XSerieAddress, string BubbleSizeAddress)
    {
      return this.AddSeries(SerieAddress, XSerieAddress, BubbleSizeAddress);
    }
  }
}
