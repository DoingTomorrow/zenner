// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingFourIconSet
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.ConditionalFormatting.Contracts;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  public class ExcelConditionalFormattingFourIconSet : 
    ExcelConditionalFormattingIconSetBase<eExcelconditionalFormatting4IconsSetType>,
    IExcelConditionalFormattingFourIconSet<eExcelconditionalFormatting4IconsSetType>,
    IExcelConditionalFormattingThreeIconSet<eExcelconditionalFormatting4IconsSetType>,
    IExcelConditionalFormattingIconSetGroup<eExcelconditionalFormatting4IconsSetType>,
    IExcelConditionalFormattingRule
  {
    internal ExcelConditionalFormattingFourIconSet(
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode,
      XmlNamespaceManager namespaceManager)
      : base(eExcelConditionalFormattingRuleType.FourIconSet, address, priority, worksheet, itemElementNode, namespaceManager == null ? worksheet.NameSpaceManager : namespaceManager)
    {
      if (itemElementNode != null && itemElementNode.HasChildNodes)
      {
        XmlNode itemElementNode1 = this.TopNode.SelectSingleNode("d:iconSet/d:cfvo[position()=4]", this.NameSpaceManager);
        this.Icon4 = new ExcelConditionalFormattingIconDataBarValue(eExcelConditionalFormattingRuleType.FourIconSet, address, worksheet, itemElementNode1, namespaceManager);
      }
      else
      {
        XmlNode xmlNode = this.TopNode.SelectSingleNode("d:iconSet", this.NameSpaceManager);
        XmlElement element = xmlNode.OwnerDocument.CreateElement("d:cfvo", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        xmlNode.AppendChild((XmlNode) element);
        this.Icon4 = new ExcelConditionalFormattingIconDataBarValue(eExcelConditionalFormattingValueObjectType.Percent, 75.0, "", eExcelConditionalFormattingRuleType.ThreeIconSet, address, priority, worksheet, (XmlNode) element, namespaceManager);
      }
    }

    internal ExcelConditionalFormattingFourIconSet(
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode)
      : this(address, priority, worksheet, itemElementNode, (XmlNamespaceManager) null)
    {
    }

    internal ExcelConditionalFormattingFourIconSet(
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet)
      : this(address, priority, worksheet, (XmlNode) null, (XmlNamespaceManager) null)
    {
    }

    public ExcelConditionalFormattingIconDataBarValue Icon4 { get; internal set; }
  }
}
