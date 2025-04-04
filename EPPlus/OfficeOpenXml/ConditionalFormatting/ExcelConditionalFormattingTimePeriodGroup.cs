// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingTimePeriodGroup
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.ConditionalFormatting.Contracts;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  public class ExcelConditionalFormattingTimePeriodGroup : 
    ExcelConditionalFormattingRule,
    IExcelConditionalFormattingTimePeriodGroup,
    IExcelConditionalFormattingRule
  {
    internal ExcelConditionalFormattingTimePeriodGroup(
      eExcelConditionalFormattingRuleType type,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode,
      XmlNamespaceManager namespaceManager)
      : base(type, address, priority, worksheet, itemElementNode, namespaceManager == null ? worksheet.NameSpaceManager : namespaceManager)
    {
    }

    internal ExcelConditionalFormattingTimePeriodGroup(
      eExcelConditionalFormattingRuleType type,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode)
      : this(type, address, priority, worksheet, itemElementNode, (XmlNamespaceManager) null)
    {
    }

    internal ExcelConditionalFormattingTimePeriodGroup(
      eExcelConditionalFormattingRuleType type,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet)
      : this(type, address, priority, worksheet, (XmlNode) null, (XmlNamespaceManager) null)
    {
    }
  }
}
