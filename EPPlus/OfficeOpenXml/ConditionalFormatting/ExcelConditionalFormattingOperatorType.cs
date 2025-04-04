// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingOperatorType
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  internal static class ExcelConditionalFormattingOperatorType
  {
    internal static string GetAttributeByType(eExcelConditionalFormattingOperatorType type)
    {
      switch (type)
      {
        case eExcelConditionalFormattingOperatorType.BeginsWith:
          return "beginsWith";
        case eExcelConditionalFormattingOperatorType.Between:
          return "between";
        case eExcelConditionalFormattingOperatorType.ContainsText:
          return "containsText";
        case eExcelConditionalFormattingOperatorType.EndsWith:
          return "endsWith";
        case eExcelConditionalFormattingOperatorType.Equal:
          return "equal";
        case eExcelConditionalFormattingOperatorType.GreaterThan:
          return "greaterThan";
        case eExcelConditionalFormattingOperatorType.GreaterThanOrEqual:
          return "greaterThanOrEqual";
        case eExcelConditionalFormattingOperatorType.LessThan:
          return "lessThan";
        case eExcelConditionalFormattingOperatorType.LessThanOrEqual:
          return "lessThanOrEqual";
        case eExcelConditionalFormattingOperatorType.NotBetween:
          return "notBetween";
        case eExcelConditionalFormattingOperatorType.NotContains:
          return "notContains";
        case eExcelConditionalFormattingOperatorType.NotEqual:
          return "notEqual";
        default:
          return string.Empty;
      }
    }

    internal static eExcelConditionalFormattingOperatorType GetTypeByAttribute(string attribute)
    {
      switch (attribute)
      {
        case "beginsWith":
          return eExcelConditionalFormattingOperatorType.BeginsWith;
        case "between":
          return eExcelConditionalFormattingOperatorType.Between;
        case "containsText":
          return eExcelConditionalFormattingOperatorType.ContainsText;
        case "endsWith":
          return eExcelConditionalFormattingOperatorType.EndsWith;
        case "equal":
          return eExcelConditionalFormattingOperatorType.Equal;
        case "greaterThan":
          return eExcelConditionalFormattingOperatorType.GreaterThan;
        case "greaterThanOrEqual":
          return eExcelConditionalFormattingOperatorType.GreaterThanOrEqual;
        case "lessThan":
          return eExcelConditionalFormattingOperatorType.LessThan;
        case "lessThanOrEqual":
          return eExcelConditionalFormattingOperatorType.LessThanOrEqual;
        case "notBetween":
          return eExcelConditionalFormattingOperatorType.NotBetween;
        case "notContains":
          return eExcelConditionalFormattingOperatorType.NotContains;
        case "notEqual":
          return eExcelConditionalFormattingOperatorType.NotEqual;
        default:
          throw new Exception("Unexistent eExcelConditionalFormattingOperatorType attribute in Conditional Formatting");
      }
    }
  }
}
