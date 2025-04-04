// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.PivotTable.eSubTotalFunctions
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.Table.PivotTable
{
  [Flags]
  public enum eSubTotalFunctions
  {
    None = 1,
    Count = 2,
    CountA = 4,
    Avg = 8,
    Default = 16, // 0x00000010
    Min = 32, // 0x00000020
    Max = 64, // 0x00000040
    Product = 128, // 0x00000080
    StdDev = 256, // 0x00000100
    StdDevP = 512, // 0x00000200
    Sum = 1024, // 0x00000400
    Var = 2048, // 0x00000800
    VarP = 4096, // 0x00001000
  }
}
