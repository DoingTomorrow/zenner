// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.IStyle
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.Style
{
  internal interface IStyle
  {
    void SetNewStyleID(string value);

    ulong Id { get; }

    ExcelStyle ExcelStyle { get; }
  }
}
