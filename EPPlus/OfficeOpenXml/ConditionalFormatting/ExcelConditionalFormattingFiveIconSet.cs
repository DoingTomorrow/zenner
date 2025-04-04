// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingFiveIconSet
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.ConditionalFormatting.Contracts;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  public class ExcelConditionalFormattingFiveIconSet : 
    ExcelConditionalFormattingIconSetBase<eExcelconditionalFormatting5IconsSetType>,
    IExcelConditionalFormattingFiveIconSet,
    IExcelConditionalFormattingFourIconSet<eExcelconditionalFormatting5IconsSetType>,
    IExcelConditionalFormattingThreeIconSet<eExcelconditionalFormatting5IconsSetType>,
    IExcelConditionalFormattingIconSetGroup<eExcelconditionalFormatting5IconsSetType>,
    IExcelConditionalFormattingRule
  {
    internal ExcelConditionalFormattingFiveIconSet(
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode,
      XmlNamespaceManager namespaceManager)
      : base(eExcelConditionalFormattingRuleType.FiveIconSet, address, priority, worksheet, itemElementNode, namespaceManager == null ? worksheet.NameSpaceManager : namespaceManager)
    {
      if (itemElementNode != null && itemElementNode.HasChildNodes)
      {
        XmlNode itemElementNode1 = this.TopNode.SelectSingleNode("d:iconSet/d:cfvo[position()=4]", this.NameSpaceManager);
        this.Icon4 = new ExcelConditionalFormattingIconDataBarValue(eExcelConditionalFormattingRuleType.FiveIconSet, address, worksheet, itemElementNode1, namespaceManager);
        XmlNode itemElementNode2 = this.TopNode.SelectSingleNode("d:iconSet/d:cfvo[position()=5]", this.NameSpaceManager);
        this.Icon5 = new ExcelConditionalFormattingIconDataBarValue(eExcelConditionalFormattingRuleType.FiveIconSet, address, worksheet, itemElementNode2, namespaceManager);
      }
      else
      {
        XmlNode xmlNode = this.TopNode.SelectSingleNode("d:iconSet", this.NameSpaceManager);
        XmlElement element1 = xmlNode.OwnerDocument.CreateElement("d:cfvo", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        xmlNode.AppendChild((XmlNode) element1);
        this.Icon4 = new ExcelConditionalFormattingIconDataBarValue(eExcelConditionalFormattingValueObjectType.Percent, 60.0, "", eExcelConditionalFormattingRuleType.ThreeIconSet, address, priority, worksheet, (XmlNode) element1, namespaceManager);
        XmlElement element2 = xmlNode.OwnerDocument.CreateElement("d:cfvo", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        xmlNode.AppendChild((XmlNode) element2);
        this.Icon5 = new ExcelConditionalFormattingIconDataBarValue(eExcelConditionalFormattingValueObjectType.Percent, 80.0, "", eExcelConditionalFormattingRuleType.ThreeIconSet, address, priority, worksheet, (XmlNode) element2, namespaceManager);
      }
    }

    internal ExcelConditionalFormattingFiveIconSet(
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode)
      : this(address, priority, worksheet, itemElementNode, (XmlNamespaceManager) null)
    {
    }

    internal ExcelConditionalFormattingFiveIconSet(
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet)
      : this(address, priority, worksheet, (XmlNode) null, (XmlNamespaceManager) null)
    {
    }

    public ExcelConditionalFormattingIconDataBarValue Icon5 { get; internal set; }

    public ExcelConditionalFormattingIconDataBarValue Icon4 { get; internal set; }
  }
}
