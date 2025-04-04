// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.CellFlags
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml
{
  [Flags]
  internal enum CellFlags
  {
    Merged = 1,
    RichText = 2,
    SharedFormula = 4,
    ArrayFormula = 8,
  }
}
