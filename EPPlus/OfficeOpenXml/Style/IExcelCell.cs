// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.IExcelCell
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.Style
{
  internal interface IExcelCell
  {
    object Value { get; set; }

    string StyleName { get; }

    int StyleID { get; set; }

    ExcelStyle Style { get; }

    Uri Hyperlink { get; set; }

    string Formula { get; set; }

    string FormulaR1C1 { get; set; }
  }
}
