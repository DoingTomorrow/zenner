// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingTimePeriodType
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  internal static class ExcelConditionalFormattingTimePeriodType
  {
    public static string GetAttributeByType(eExcelConditionalFormattingTimePeriodType type)
    {
      switch (type)
      {
        case eExcelConditionalFormattingTimePeriodType.Last7Days:
          return "last7Days";
        case eExcelConditionalFormattingTimePeriodType.LastMonth:
          return "lastMonth";
        case eExcelConditionalFormattingTimePeriodType.LastWeek:
          return "lastWeek";
        case eExcelConditionalFormattingTimePeriodType.NextMonth:
          return "nextMonth";
        case eExcelConditionalFormattingTimePeriodType.NextWeek:
          return "nextWeek";
        case eExcelConditionalFormattingTimePeriodType.ThisMonth:
          return "thisMonth";
        case eExcelConditionalFormattingTimePeriodType.ThisWeek:
          return "thisWeek";
        case eExcelConditionalFormattingTimePeriodType.Today:
          return "today";
        case eExcelConditionalFormattingTimePeriodType.Tomorrow:
          return "tomorrow";
        case eExcelConditionalFormattingTimePeriodType.Yesterday:
          return "yesterday";
        default:
          return string.Empty;
      }
    }

    public static eExcelConditionalFormattingTimePeriodType GetTypeByAttribute(string attribute)
    {
      switch (attribute)
      {
        case "last7Days":
          return eExcelConditionalFormattingTimePeriodType.Last7Days;
        case "lastMonth":
          return eExcelConditionalFormattingTimePeriodType.LastMonth;
        case "lastWeek":
          return eExcelConditionalFormattingTimePeriodType.LastWeek;
        case "nextMonth":
          return eExcelConditionalFormattingTimePeriodType.NextMonth;
        case "nextWeek":
          return eExcelConditionalFormattingTimePeriodType.NextWeek;
        case "thisMonth":
          return eExcelConditionalFormattingTimePeriodType.ThisMonth;
        case "thisWeek":
          return eExcelConditionalFormattingTimePeriodType.ThisWeek;
        case "today":
          return eExcelConditionalFormattingTimePeriodType.Today;
        case "tomorrow":
          return eExcelConditionalFormattingTimePeriodType.Tomorrow;
        case "yesterday":
          return eExcelConditionalFormattingTimePeriodType.Yesterday;
        default:
          throw new Exception("Unexistent eExcelConditionalFormattingTimePeriodType attribute in Conditional Formatting");
      }
    }
  }
}
