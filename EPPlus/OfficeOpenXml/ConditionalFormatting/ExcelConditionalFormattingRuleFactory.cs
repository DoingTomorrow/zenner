// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingRuleFactory
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Utils;
using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  internal static class ExcelConditionalFormattingRuleFactory
  {
    public static ExcelConditionalFormattingRule Create(
      eExcelConditionalFormattingRuleType type,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode)
    {
      Require.Argument<eExcelConditionalFormattingRuleType>(type);
      Require.Argument<ExcelAddress>(address).IsNotNull<ExcelAddress>(nameof (address));
      Require.Argument<int>(priority).IsInRange<int>(1, int.MaxValue, nameof (priority));
      Require.Argument<ExcelWorksheet>(worksheet).IsNotNull<ExcelWorksheet>(nameof (worksheet));
      switch (type)
      {
        case eExcelConditionalFormattingRuleType.AboveAverage:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingAboveAverage(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.AboveOrEqualAverage:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingAboveOrEqualAverage(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.BelowAverage:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingBelowAverage(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.BelowOrEqualAverage:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingBelowOrEqualAverage(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.AboveStdDev:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingAboveStdDev(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.BelowStdDev:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingBelowStdDev(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.Bottom:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingBottom(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.BottomPercent:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingBottomPercent(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.Top:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingTop(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.TopPercent:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingTopPercent(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.Last7Days:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingLast7Days(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.LastMonth:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingLastMonth(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.LastWeek:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingLastWeek(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.NextMonth:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingNextMonth(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.NextWeek:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingNextWeek(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.ThisMonth:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingThisMonth(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.ThisWeek:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingThisWeek(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.Today:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingToday(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.Tomorrow:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingTomorrow(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.Yesterday:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingYesterday(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.BeginsWith:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingBeginsWith(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.Between:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingBetween(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.ContainsBlanks:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingContainsBlanks(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.ContainsErrors:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingContainsErrors(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.ContainsText:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingContainsText(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.DuplicateValues:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingDuplicateValues(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.EndsWith:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingEndsWith(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.Equal:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingEqual(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.Expression:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingExpression(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.GreaterThan:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingGreaterThan(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.GreaterThanOrEqual:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingGreaterThanOrEqual(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.LessThan:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingLessThan(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.LessThanOrEqual:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingLessThanOrEqual(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.NotBetween:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingNotBetween(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.NotContainsBlanks:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingNotContainsBlanks(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.NotContainsErrors:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingNotContainsErrors(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.NotContainsText:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingNotContainsText(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.NotEqual:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingNotEqual(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.UniqueValues:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingUniqueValues(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.ThreeColorScale:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingThreeColorScale(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.TwoColorScale:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingTwoColorScale(address, priority, worksheet, itemElementNode);
        case eExcelConditionalFormattingRuleType.ThreeIconSet:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingThreeIconSet(address, priority, worksheet, itemElementNode, (XmlNamespaceManager) null);
        case eExcelConditionalFormattingRuleType.FourIconSet:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingFourIconSet(address, priority, worksheet, itemElementNode, (XmlNamespaceManager) null);
        case eExcelConditionalFormattingRuleType.FiveIconSet:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingFiveIconSet(address, priority, worksheet, itemElementNode, (XmlNamespaceManager) null);
        case eExcelConditionalFormattingRuleType.DataBar:
          return (ExcelConditionalFormattingRule) new ExcelConditionalFormattingDataBar(eExcelConditionalFormattingRuleType.DataBar, address, priority, worksheet, itemElementNode, (XmlNamespaceManager) null);
        default:
          throw new InvalidOperationException(string.Format("Non supported conditionalFormattingType: {0}", (object) type.ToString()));
      }
    }
  }
}
