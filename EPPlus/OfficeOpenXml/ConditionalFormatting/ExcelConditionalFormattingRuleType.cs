// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingRuleType
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  internal static class ExcelConditionalFormattingRuleType
  {
    internal static eExcelConditionalFormattingRuleType GetTypeByAttrbiute(
      string attribute,
      XmlNode topNode,
      XmlNamespaceManager nameSpaceManager)
    {
      switch (attribute)
      {
        case "aboveAverage":
          return ExcelConditionalFormattingRuleType.GetAboveAverageType(topNode, nameSpaceManager);
        case "top10":
          return ExcelConditionalFormattingRuleType.GetTop10Type(topNode, nameSpaceManager);
        case "timePeriod":
          return ExcelConditionalFormattingRuleType.GetTimePeriodType(topNode, nameSpaceManager);
        case "cellIs":
          return ExcelConditionalFormattingRuleType.GetCellIs((XmlElement) topNode);
        case "beginsWith":
          return eExcelConditionalFormattingRuleType.BeginsWith;
        case "containsBlanks":
          return eExcelConditionalFormattingRuleType.ContainsBlanks;
        case "containsErrors":
          return eExcelConditionalFormattingRuleType.ContainsErrors;
        case "containsText":
          return eExcelConditionalFormattingRuleType.ContainsText;
        case "duplicateValues":
          return eExcelConditionalFormattingRuleType.DuplicateValues;
        case "endsWith":
          return eExcelConditionalFormattingRuleType.EndsWith;
        case "expression":
          return eExcelConditionalFormattingRuleType.Expression;
        case "notContainsBlanks":
          return eExcelConditionalFormattingRuleType.NotContainsBlanks;
        case "notContainsErrors":
          return eExcelConditionalFormattingRuleType.NotContainsErrors;
        case "notContainsText":
          return eExcelConditionalFormattingRuleType.NotContainsText;
        case "uniqueValues":
          return eExcelConditionalFormattingRuleType.UniqueValues;
        case "colorScale":
          return ExcelConditionalFormattingRuleType.GetColorScaleType(topNode, nameSpaceManager);
        case "iconSet":
          return ExcelConditionalFormattingRuleType.GetIconSetType(topNode, nameSpaceManager);
        case "dataBar":
          return eExcelConditionalFormattingRuleType.DataBar;
        default:
          throw new Exception("Unexpected eExcelConditionalFormattingRuleType attribute in Conditional Formatting Rule");
      }
    }

    private static eExcelConditionalFormattingRuleType GetCellIs(XmlElement node)
    {
      switch (node.GetAttribute("operator"))
      {
        case "beginsWith":
          return eExcelConditionalFormattingRuleType.BeginsWith;
        case "between":
          return eExcelConditionalFormattingRuleType.Between;
        case "containsText":
          return eExcelConditionalFormattingRuleType.ContainsText;
        case "endsWith":
          return eExcelConditionalFormattingRuleType.EndsWith;
        case "equal":
          return eExcelConditionalFormattingRuleType.Equal;
        case "greaterThan":
          return eExcelConditionalFormattingRuleType.GreaterThan;
        case "greaterThanOrEqual":
          return eExcelConditionalFormattingRuleType.GreaterThanOrEqual;
        case "lessThan":
          return eExcelConditionalFormattingRuleType.LessThan;
        case "lessThanOrEqual":
          return eExcelConditionalFormattingRuleType.LessThanOrEqual;
        case "notBetween":
          return eExcelConditionalFormattingRuleType.NotBetween;
        case "notContains":
          return eExcelConditionalFormattingRuleType.NotContains;
        case "notEqual":
          return eExcelConditionalFormattingRuleType.NotEqual;
        default:
          throw new Exception("Unexistent eExcelConditionalFormattingOperatorType attribute in Conditional Formatting");
      }
    }

    private static eExcelConditionalFormattingRuleType GetIconSetType(
      XmlNode topNode,
      XmlNamespaceManager nameSpaceManager)
    {
      XmlNode xmlNode = topNode.SelectSingleNode("d:iconSet/@iconSet", nameSpaceManager);
      if (xmlNode == null)
        return eExcelConditionalFormattingRuleType.ThreeIconSet;
      string str = xmlNode.Value;
      if (str[0] == '3')
        return eExcelConditionalFormattingRuleType.ThreeIconSet;
      return str[0] == '4' ? eExcelConditionalFormattingRuleType.FourIconSet : eExcelConditionalFormattingRuleType.FiveIconSet;
    }

    internal static eExcelConditionalFormattingRuleType GetColorScaleType(
      XmlNode topNode,
      XmlNamespaceManager nameSpaceManager)
    {
      XmlNodeList xmlNodeList1 = topNode.SelectNodes(string.Format("{0}/{1}", (object) "d:colorScale", (object) "d:cfvo"), nameSpaceManager);
      XmlNodeList xmlNodeList2 = topNode.SelectNodes(string.Format("{0}/{1}", (object) "d:colorScale", (object) "d:color"), nameSpaceManager);
      if (xmlNodeList1 == null || xmlNodeList1.Count < 2 || xmlNodeList1.Count > 3 || xmlNodeList2 == null || xmlNodeList2.Count < 2 || xmlNodeList2.Count > 3 || xmlNodeList1.Count != xmlNodeList2.Count)
        throw new Exception("Wrong number of 'cfvo'/'color' nodes in Conditional Formatting Rule");
      return xmlNodeList1.Count != 2 ? eExcelConditionalFormattingRuleType.ThreeColorScale : eExcelConditionalFormattingRuleType.TwoColorScale;
    }

    internal static eExcelConditionalFormattingRuleType GetAboveAverageType(
      XmlNode topNode,
      XmlNamespaceManager nameSpaceManager)
    {
      int? attributeIntNullable = ExcelConditionalFormattingHelper.GetAttributeIntNullable(topNode, "stdDev");
      int? nullable1 = attributeIntNullable;
      if ((nullable1.GetValueOrDefault() <= 0 ? 0 : (nullable1.HasValue ? 1 : 0)) != 0)
        return eExcelConditionalFormattingRuleType.AboveStdDev;
      int? nullable2 = attributeIntNullable;
      if ((nullable2.GetValueOrDefault() >= 0 ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
        return eExcelConditionalFormattingRuleType.BelowStdDev;
      bool? attributeBoolNullable1 = ExcelConditionalFormattingHelper.GetAttributeBoolNullable(topNode, "aboveAverage");
      bool? attributeBoolNullable2 = ExcelConditionalFormattingHelper.GetAttributeBoolNullable(topNode, "equalAverage");
      if (attributeBoolNullable1.HasValue)
      {
        bool? nullable3 = attributeBoolNullable1;
        if ((!nullable3.GetValueOrDefault() ? 0 : (nullable3.HasValue ? 1 : 0)) == 0)
        {
          bool? nullable4 = attributeBoolNullable2;
          return (!nullable4.GetValueOrDefault() ? 0 : (nullable4.HasValue ? 1 : 0)) != 0 ? eExcelConditionalFormattingRuleType.BelowOrEqualAverage : eExcelConditionalFormattingRuleType.BelowAverage;
        }
      }
      bool? nullable5 = attributeBoolNullable2;
      return (!nullable5.GetValueOrDefault() ? 0 : (nullable5.HasValue ? 1 : 0)) != 0 ? eExcelConditionalFormattingRuleType.AboveOrEqualAverage : eExcelConditionalFormattingRuleType.AboveAverage;
    }

    public static eExcelConditionalFormattingRuleType GetTop10Type(
      XmlNode topNode,
      XmlNamespaceManager nameSpaceManager)
    {
      bool? attributeBoolNullable1 = ExcelConditionalFormattingHelper.GetAttributeBoolNullable(topNode, "bottom");
      bool? attributeBoolNullable2 = ExcelConditionalFormattingHelper.GetAttributeBoolNullable(topNode, "percent");
      bool? nullable1 = attributeBoolNullable1;
      if ((!nullable1.GetValueOrDefault() ? 0 : (nullable1.HasValue ? 1 : 0)) != 0)
      {
        bool? nullable2 = attributeBoolNullable2;
        return (!nullable2.GetValueOrDefault() ? 0 : (nullable2.HasValue ? 1 : 0)) != 0 ? eExcelConditionalFormattingRuleType.BottomPercent : eExcelConditionalFormattingRuleType.Bottom;
      }
      bool? nullable3 = attributeBoolNullable2;
      return (!nullable3.GetValueOrDefault() ? 0 : (nullable3.HasValue ? 1 : 0)) != 0 ? eExcelConditionalFormattingRuleType.TopPercent : eExcelConditionalFormattingRuleType.Top;
    }

    public static eExcelConditionalFormattingRuleType GetTimePeriodType(
      XmlNode topNode,
      XmlNamespaceManager nameSpaceManager)
    {
      switch (ExcelConditionalFormattingTimePeriodType.GetTypeByAttribute(ExcelConditionalFormattingHelper.GetAttributeString(topNode, "timePeriod")))
      {
        case eExcelConditionalFormattingTimePeriodType.Last7Days:
          return eExcelConditionalFormattingRuleType.Last7Days;
        case eExcelConditionalFormattingTimePeriodType.LastMonth:
          return eExcelConditionalFormattingRuleType.LastMonth;
        case eExcelConditionalFormattingTimePeriodType.LastWeek:
          return eExcelConditionalFormattingRuleType.LastWeek;
        case eExcelConditionalFormattingTimePeriodType.NextMonth:
          return eExcelConditionalFormattingRuleType.NextMonth;
        case eExcelConditionalFormattingTimePeriodType.NextWeek:
          return eExcelConditionalFormattingRuleType.NextWeek;
        case eExcelConditionalFormattingTimePeriodType.ThisMonth:
          return eExcelConditionalFormattingRuleType.ThisMonth;
        case eExcelConditionalFormattingTimePeriodType.ThisWeek:
          return eExcelConditionalFormattingRuleType.ThisWeek;
        case eExcelConditionalFormattingTimePeriodType.Today:
          return eExcelConditionalFormattingRuleType.Today;
        case eExcelConditionalFormattingTimePeriodType.Tomorrow:
          return eExcelConditionalFormattingRuleType.Tomorrow;
        case eExcelConditionalFormattingTimePeriodType.Yesterday:
          return eExcelConditionalFormattingRuleType.Yesterday;
        default:
          throw new Exception("Unexistent eExcelConditionalFormattingTimePeriodType attribute in Conditional Formatting");
      }
    }

    public static string GetAttributeByType(eExcelConditionalFormattingRuleType type)
    {
      switch (type)
      {
        case eExcelConditionalFormattingRuleType.AboveAverage:
        case eExcelConditionalFormattingRuleType.AboveOrEqualAverage:
        case eExcelConditionalFormattingRuleType.BelowAverage:
        case eExcelConditionalFormattingRuleType.BelowOrEqualAverage:
        case eExcelConditionalFormattingRuleType.AboveStdDev:
        case eExcelConditionalFormattingRuleType.BelowStdDev:
          return "aboveAverage";
        case eExcelConditionalFormattingRuleType.Bottom:
        case eExcelConditionalFormattingRuleType.BottomPercent:
        case eExcelConditionalFormattingRuleType.Top:
        case eExcelConditionalFormattingRuleType.TopPercent:
          return "top10";
        case eExcelConditionalFormattingRuleType.Last7Days:
        case eExcelConditionalFormattingRuleType.LastMonth:
        case eExcelConditionalFormattingRuleType.LastWeek:
        case eExcelConditionalFormattingRuleType.NextMonth:
        case eExcelConditionalFormattingRuleType.NextWeek:
        case eExcelConditionalFormattingRuleType.ThisMonth:
        case eExcelConditionalFormattingRuleType.ThisWeek:
        case eExcelConditionalFormattingRuleType.Today:
        case eExcelConditionalFormattingRuleType.Tomorrow:
        case eExcelConditionalFormattingRuleType.Yesterday:
          return "timePeriod";
        case eExcelConditionalFormattingRuleType.BeginsWith:
          return "beginsWith";
        case eExcelConditionalFormattingRuleType.Between:
        case eExcelConditionalFormattingRuleType.Equal:
        case eExcelConditionalFormattingRuleType.GreaterThan:
        case eExcelConditionalFormattingRuleType.GreaterThanOrEqual:
        case eExcelConditionalFormattingRuleType.LessThan:
        case eExcelConditionalFormattingRuleType.LessThanOrEqual:
        case eExcelConditionalFormattingRuleType.NotBetween:
        case eExcelConditionalFormattingRuleType.NotEqual:
          return "cellIs";
        case eExcelConditionalFormattingRuleType.ContainsBlanks:
          return "containsBlanks";
        case eExcelConditionalFormattingRuleType.ContainsErrors:
          return "containsErrors";
        case eExcelConditionalFormattingRuleType.ContainsText:
          return "containsText";
        case eExcelConditionalFormattingRuleType.DuplicateValues:
          return "duplicateValues";
        case eExcelConditionalFormattingRuleType.EndsWith:
          return "endsWith";
        case eExcelConditionalFormattingRuleType.Expression:
          return "expression";
        case eExcelConditionalFormattingRuleType.NotContainsBlanks:
          return "notContainsBlanks";
        case eExcelConditionalFormattingRuleType.NotContainsErrors:
          return "notContainsErrors";
        case eExcelConditionalFormattingRuleType.NotContainsText:
          return "notContainsText";
        case eExcelConditionalFormattingRuleType.UniqueValues:
          return "uniqueValues";
        case eExcelConditionalFormattingRuleType.ThreeColorScale:
        case eExcelConditionalFormattingRuleType.TwoColorScale:
          return "colorScale";
        case eExcelConditionalFormattingRuleType.ThreeIconSet:
        case eExcelConditionalFormattingRuleType.FourIconSet:
        case eExcelConditionalFormattingRuleType.FiveIconSet:
          return "iconSet";
        case eExcelConditionalFormattingRuleType.DataBar:
          return "dataBar";
        default:
          throw new Exception("Missing eExcelConditionalFormattingRuleType Type in Conditional Formatting");
      }
    }

    public static string GetCfvoParentPathByType(eExcelConditionalFormattingRuleType type)
    {
      switch (type)
      {
        case eExcelConditionalFormattingRuleType.ThreeColorScale:
        case eExcelConditionalFormattingRuleType.TwoColorScale:
          return "d:colorScale";
        case eExcelConditionalFormattingRuleType.ThreeIconSet:
        case eExcelConditionalFormattingRuleType.FourIconSet:
        case eExcelConditionalFormattingRuleType.FiveIconSet:
          return "iconSet";
        case eExcelConditionalFormattingRuleType.DataBar:
          return "dataBar";
        default:
          throw new Exception("Missing eExcelConditionalFormattingRuleType Type in Conditional Formatting");
      }
    }
  }
}
