// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingValueObjectType
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  internal static class ExcelConditionalFormattingValueObjectType
  {
    internal static int GetOrderByPosition(
      eExcelConditionalFormattingValueObjectPosition position,
      eExcelConditionalFormattingRuleType ruleType)
    {
      switch (position)
      {
        case eExcelConditionalFormattingValueObjectPosition.Low:
          return 1;
        case eExcelConditionalFormattingValueObjectPosition.Middle:
          return 2;
        case eExcelConditionalFormattingValueObjectPosition.High:
          return ruleType == eExcelConditionalFormattingRuleType.TwoColorScale ? 2 : 3;
        default:
          return 0;
      }
    }

    public static eExcelConditionalFormattingValueObjectType GetTypeByAttrbiute(string attribute)
    {
      switch (attribute)
      {
        case "min":
          return eExcelConditionalFormattingValueObjectType.Min;
        case "max":
          return eExcelConditionalFormattingValueObjectType.Max;
        case "num":
          return eExcelConditionalFormattingValueObjectType.Num;
        case "formula":
          return eExcelConditionalFormattingValueObjectType.Formula;
        case "percent":
          return eExcelConditionalFormattingValueObjectType.Percent;
        case "percentile":
          return eExcelConditionalFormattingValueObjectType.Percentile;
        default:
          throw new Exception("Unexistent eExcelConditionalFormattingValueObjectType attribute in Conditional Formatting");
      }
    }

    public static XmlNode GetCfvoNodeByPosition(
      eExcelConditionalFormattingValueObjectPosition position,
      eExcelConditionalFormattingRuleType ruleType,
      XmlNode topNode,
      XmlNamespaceManager nameSpaceManager)
    {
      return topNode.SelectSingleNode(string.Format("{0}[position()={1}]", (object) "d:cfvo", (object) ExcelConditionalFormattingValueObjectType.GetOrderByPosition(position, ruleType)), nameSpaceManager) ?? throw new Exception("Missing 'cfvo' node in Conditional Formatting");
    }

    public static string GetAttributeByType(eExcelConditionalFormattingValueObjectType type)
    {
      switch (type)
      {
        case eExcelConditionalFormattingValueObjectType.Formula:
          return "formula";
        case eExcelConditionalFormattingValueObjectType.Max:
          return "max";
        case eExcelConditionalFormattingValueObjectType.Min:
          return "min";
        case eExcelConditionalFormattingValueObjectType.Num:
          return "num";
        case eExcelConditionalFormattingValueObjectType.Percent:
          return "percent";
        case eExcelConditionalFormattingValueObjectType.Percentile:
          return "percentile";
        default:
          return string.Empty;
      }
    }

    public static string GetParentPathByRuleType(eExcelConditionalFormattingRuleType ruleType)
    {
      switch (ruleType)
      {
        case eExcelConditionalFormattingRuleType.ThreeColorScale:
        case eExcelConditionalFormattingRuleType.TwoColorScale:
          return "d:colorScale";
        case eExcelConditionalFormattingRuleType.ThreeIconSet:
        case eExcelConditionalFormattingRuleType.FourIconSet:
        case eExcelConditionalFormattingRuleType.FiveIconSet:
          return "d:iconSet";
        case eExcelConditionalFormattingRuleType.DataBar:
          return "d:dataBar";
        default:
          return string.Empty;
      }
    }

    public static string GetNodePathByNodeType(
      eExcelConditionalFormattingValueObjectNodeType nodeType)
    {
      switch (nodeType)
      {
        case eExcelConditionalFormattingValueObjectNodeType.Cfvo:
          return "d:cfvo";
        case eExcelConditionalFormattingValueObjectNodeType.Color:
          return "d:color";
        default:
          return string.Empty;
      }
    }
  }
}
