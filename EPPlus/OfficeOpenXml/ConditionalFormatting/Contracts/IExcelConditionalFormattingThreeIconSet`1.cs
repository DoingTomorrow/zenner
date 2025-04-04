// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.Contracts.IExcelConditionalFormattingThreeIconSet`1
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting.Contracts
{
  public interface IExcelConditionalFormattingThreeIconSet<T> : 
    IExcelConditionalFormattingIconSetGroup<T>,
    IExcelConditionalFormattingRule
  {
    ExcelConditionalFormattingIconDataBarValue Icon1 { get; }

    ExcelConditionalFormattingIconDataBarValue Icon2 { get; }

    ExcelConditionalFormattingIconDataBarValue Icon3 { get; }
  }
}
