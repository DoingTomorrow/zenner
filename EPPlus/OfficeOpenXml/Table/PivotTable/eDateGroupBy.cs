// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.PivotTable.eDateGroupBy
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.Table.PivotTable
{
  [Flags]
  public enum eDateGroupBy
  {
    Years = 1,
    Quarters = 2,
    Months = 4,
    Days = 8,
    Hours = 16, // 0x00000010
    Minutes = 32, // 0x00000020
    Seconds = 64, // 0x00000040
  }
}
