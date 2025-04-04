// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingDataBar
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.ConditionalFormatting.Contracts;
using System.Drawing;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  public class ExcelConditionalFormattingDataBar : 
    ExcelConditionalFormattingRule,
    IExcelConditionalFormattingDataBarGroup,
    IExcelConditionalFormattingRule
  {
    private const string _showValuePath = "d:dataBar/@showValue";
    private const string _colorPath = "d:dataBar/d:color/@rgb";

    internal ExcelConditionalFormattingDataBar(
      eExcelConditionalFormattingRuleType type,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode,
      XmlNamespaceManager namespaceManager)
      : base(type, address, priority, worksheet, itemElementNode, namespaceManager == null ? worksheet.NameSpaceManager : namespaceManager)
    {
      this.SchemaNodeOrder = new string[2]
      {
        "cfvo",
        "color"
      };
      if (itemElementNode != null && itemElementNode.HasChildNodes)
      {
        bool flag = false;
        foreach (XmlNode selectNode in itemElementNode.SelectNodes("d:dataBar/d:cfvo", this.NameSpaceManager))
        {
          if (!flag)
          {
            this.LowValue = new ExcelConditionalFormattingIconDataBarValue(type, address, worksheet, selectNode, namespaceManager);
            flag = true;
          }
          else
            this.HighValue = new ExcelConditionalFormattingIconDataBarValue(type, address, worksheet, selectNode, namespaceManager);
        }
      }
      else
      {
        XmlNode complexNode = this.CreateComplexNode(this.Node, "d:dataBar");
        XmlElement element1 = complexNode.OwnerDocument.CreateElement("d:cfvo", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        complexNode.AppendChild((XmlNode) element1);
        this.LowValue = new ExcelConditionalFormattingIconDataBarValue(eExcelConditionalFormattingValueObjectType.Min, 0.0, "", eExcelConditionalFormattingRuleType.DataBar, address, priority, worksheet, (XmlNode) element1, namespaceManager);
        XmlElement element2 = complexNode.OwnerDocument.CreateElement("d:cfvo", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        complexNode.AppendChild((XmlNode) element2);
        this.HighValue = new ExcelConditionalFormattingIconDataBarValue(eExcelConditionalFormattingValueObjectType.Max, 0.0, "", eExcelConditionalFormattingRuleType.DataBar, address, priority, worksheet, (XmlNode) element2, namespaceManager);
      }
      this.Type = type;
    }

    internal ExcelConditionalFormattingDataBar(
      eExcelConditionalFormattingRuleType type,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode)
      : this(type, address, priority, worksheet, itemElementNode, (XmlNamespaceManager) null)
    {
    }

    internal ExcelConditionalFormattingDataBar(
      eExcelConditionalFormattingRuleType type,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet)
      : this(type, address, priority, worksheet, (XmlNode) null, (XmlNamespaceManager) null)
    {
    }

    public bool ShowValue
    {
      get => this.GetXmlNodeBool("d:dataBar/@showValue", true);
      set => this.SetXmlNodeBool("d:dataBar/@showValue", value);
    }

    public ExcelConditionalFormattingIconDataBarValue LowValue { get; internal set; }

    public ExcelConditionalFormattingIconDataBarValue HighValue { get; internal set; }

    public Color Color
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("d:dataBar/d:color/@rgb");
        return !string.IsNullOrEmpty(xmlNodeString) ? Color.FromArgb(int.Parse(xmlNodeString, NumberStyles.HexNumber)) : Color.White;
      }
      set => this.SetXmlNodeString("d:dataBar/d:color/@rgb", value.ToArgb().ToString("X"));
    }
  }
}
