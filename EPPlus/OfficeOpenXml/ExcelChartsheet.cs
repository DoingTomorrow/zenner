// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelChartsheet
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Drawing.Chart;
using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelChartsheet : ExcelWorksheet
  {
    public ExcelChartsheet(
      XmlNamespaceManager ns,
      ExcelPackage pck,
      string relID,
      Uri uriWorksheet,
      string sheetName,
      int sheetID,
      int positionID,
      eWorkSheetHidden hidden,
      eChartType chartType)
      : base(ns, pck, relID, uriWorksheet, sheetName, sheetID, positionID, hidden)
    {
      this.Drawings.AddChart("Chart 1", chartType);
    }

    public ExcelChartsheet(
      XmlNamespaceManager ns,
      ExcelPackage pck,
      string relID,
      Uri uriWorksheet,
      string sheetName,
      int sheetID,
      int positionID,
      eWorkSheetHidden hidden)
      : base(ns, pck, relID, uriWorksheet, sheetName, sheetID, positionID, hidden)
    {
    }

    public ExcelChart Chart => (ExcelChart) this.Drawings[0];
  }
}
