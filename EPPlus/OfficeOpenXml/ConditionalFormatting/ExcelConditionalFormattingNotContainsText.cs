// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingNotContainsText
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.ConditionalFormatting.Contracts;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  public class ExcelConditionalFormattingNotContainsText : 
    ExcelConditionalFormattingRule,
    IExcelConditionalFormattingNotContainsText,
    IExcelConditionalFormattingRule,
    IExcelConditionalFormattingWithText
  {
    internal ExcelConditionalFormattingNotContainsText(
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode,
      XmlNamespaceManager namespaceManager)
      : base(eExcelConditionalFormattingRuleType.NotContainsText, address, priority, worksheet, itemElementNode, namespaceManager == null ? worksheet.NameSpaceManager : namespaceManager)
    {
      if (itemElementNode != null)
        return;
      this.Operator = eExcelConditionalFormattingOperatorType.NotContains;
      this.Text = string.Empty;
    }

    internal ExcelConditionalFormattingNotContainsText(
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode)
      : this(address, priority, worksheet, itemElementNode, (XmlNamespaceManager) null)
    {
    }

    internal ExcelConditionalFormattingNotContainsText(
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet)
      : this(address, priority, worksheet, (XmlNode) null, (XmlNamespaceManager) null)
    {
    }

    public string Text
    {
      get => this.GetXmlNodeString("@text");
      set
      {
        this.SetXmlNodeString("@text", value);
        this.Formula = string.Format("ISERROR(SEARCH(\"{1}\",{0}))", (object) this.Address.Start.Address, (object) value.Replace("\"", "\"\""));
      }
    }
  }
}
