// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingBetween
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.ConditionalFormatting.Contracts;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  public class ExcelConditionalFormattingBetween : 
    ExcelConditionalFormattingRule,
    IExcelConditionalFormattingBetween,
    IExcelConditionalFormattingRule,
    IExcelConditionalFormattingWithFormula2,
    IExcelConditionalFormattingWithFormula
  {
    internal ExcelConditionalFormattingBetween(
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode,
      XmlNamespaceManager namespaceManager)
      : base(eExcelConditionalFormattingRuleType.Between, address, priority, worksheet, itemElementNode, namespaceManager == null ? worksheet.NameSpaceManager : namespaceManager)
    {
      if (itemElementNode != null)
        return;
      this.Operator = eExcelConditionalFormattingOperatorType.Between;
      this.Formula = string.Empty;
      this.Formula2 = string.Empty;
    }

    internal ExcelConditionalFormattingBetween(
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode)
      : this(address, priority, worksheet, itemElementNode, (XmlNamespaceManager) null)
    {
    }

    internal ExcelConditionalFormattingBetween(
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet)
      : this(address, priority, worksheet, (XmlNode) null, (XmlNamespaceManager) null)
    {
    }
  }
}
