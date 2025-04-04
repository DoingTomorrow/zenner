// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.Contracts.IExcelConditionalFormattingFiveIconSet
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting.Contracts
{
  public interface IExcelConditionalFormattingFiveIconSet : 
    IExcelConditionalFormattingFourIconSet<eExcelconditionalFormatting5IconsSetType>,
    IExcelConditionalFormattingThreeIconSet<eExcelconditionalFormatting5IconsSetType>,
    IExcelConditionalFormattingIconSetGroup<eExcelconditionalFormatting5IconsSetType>,
    IExcelConditionalFormattingRule
  {
    ExcelConditionalFormattingIconDataBarValue Icon5 { get; }
  }
}
