// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelSurfaceChartSerie
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public sealed class ExcelSurfaceChartSerie : ExcelChartSerie
  {
    internal ExcelSurfaceChartSerie(
      ExcelChartSeries chartSeries,
      XmlNamespaceManager ns,
      XmlNode node,
      bool isPivot)
      : base(chartSeries, ns, node, isPivot)
    {
    }
  }
}
