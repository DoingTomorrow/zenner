// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingIconSetBase`1
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.ConditionalFormatting.Contracts;
using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  public class ExcelConditionalFormattingIconSetBase<T> : 
    ExcelConditionalFormattingRule,
    IExcelConditionalFormattingThreeIconSet<T>,
    IExcelConditionalFormattingIconSetGroup<T>,
    IExcelConditionalFormattingRule
  {
    private const string _reversePath = "d:iconSet/@reverse";
    private const string _showValuePath = "d:iconSet/@showValue";
    private const string _iconSetPath = "d:iconSet/@iconSet";

    internal ExcelConditionalFormattingIconSetBase(
      eExcelConditionalFormattingRuleType type,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode,
      XmlNamespaceManager namespaceManager)
      : base(type, address, priority, worksheet, itemElementNode, namespaceManager == null ? worksheet.NameSpaceManager : namespaceManager)
    {
      if (itemElementNode != null && itemElementNode.HasChildNodes)
      {
        int num = 1;
        foreach (XmlNode selectNode in itemElementNode.SelectNodes("d:iconSet/d:cfvo", this.NameSpaceManager))
        {
          switch (num)
          {
            case 1:
              this.Icon1 = new ExcelConditionalFormattingIconDataBarValue(type, address, worksheet, selectNode, namespaceManager);
              break;
            case 2:
              this.Icon2 = new ExcelConditionalFormattingIconDataBarValue(type, address, worksheet, selectNode, namespaceManager);
              break;
            case 3:
              this.Icon3 = new ExcelConditionalFormattingIconDataBarValue(type, address, worksheet, selectNode, namespaceManager);
              break;
            default:
              return;
          }
          ++num;
        }
      }
      else
      {
        XmlNode complexNode = this.CreateComplexNode(this.Node, "d:iconSet");
        double num;
        switch (type)
        {
          case eExcelConditionalFormattingRuleType.ThreeIconSet:
            num = 3.0;
            break;
          case eExcelConditionalFormattingRuleType.FourIconSet:
            num = 4.0;
            break;
          default:
            num = 5.0;
            break;
        }
        XmlElement element1 = complexNode.OwnerDocument.CreateElement("d:cfvo", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        complexNode.AppendChild((XmlNode) element1);
        this.Icon1 = new ExcelConditionalFormattingIconDataBarValue(eExcelConditionalFormattingValueObjectType.Percent, 0.0, "", eExcelConditionalFormattingRuleType.ThreeIconSet, address, priority, worksheet, (XmlNode) element1, namespaceManager);
        XmlElement element2 = complexNode.OwnerDocument.CreateElement("d:cfvo", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        complexNode.AppendChild((XmlNode) element2);
        this.Icon2 = new ExcelConditionalFormattingIconDataBarValue(eExcelConditionalFormattingValueObjectType.Percent, Math.Round(100.0 / num, 0), "", eExcelConditionalFormattingRuleType.ThreeIconSet, address, priority, worksheet, (XmlNode) element2, namespaceManager);
        XmlElement element3 = complexNode.OwnerDocument.CreateElement("d:cfvo", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        complexNode.AppendChild((XmlNode) element3);
        this.Icon3 = new ExcelConditionalFormattingIconDataBarValue(eExcelConditionalFormattingValueObjectType.Percent, Math.Round(100.0 * (2.0 / num), 0), "", eExcelConditionalFormattingRuleType.ThreeIconSet, address, priority, worksheet, (XmlNode) element3, namespaceManager);
        this.Type = type;
      }
    }

    internal ExcelConditionalFormattingIconSetBase(
      eExcelConditionalFormattingRuleType type,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode)
      : this(type, address, priority, worksheet, itemElementNode, (XmlNamespaceManager) null)
    {
    }

    internal ExcelConditionalFormattingIconSetBase(
      eExcelConditionalFormattingRuleType type,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet)
      : this(type, address, priority, worksheet, (XmlNode) null, (XmlNamespaceManager) null)
    {
    }

    public ExcelConditionalFormattingIconDataBarValue Icon1 { get; internal set; }

    public ExcelConditionalFormattingIconDataBarValue Icon2 { get; internal set; }

    public ExcelConditionalFormattingIconDataBarValue Icon3 { get; internal set; }

    public bool Reverse
    {
      get => this.GetXmlNodeBool("d:iconSet/@reverse", false);
      set => this.SetXmlNodeBool("d:iconSet/@reverse", value);
    }

    public bool ShowValue
    {
      get => this.GetXmlNodeBool("d:iconSet/@showValue", true);
      set => this.SetXmlNodeBool("d:iconSet/@showValue", value);
    }

    public T IconSet
    {
      get
      {
        return (T) Enum.Parse(typeof (T), this.GetXmlNodeString("d:iconSet/@iconSet").Substring(1), true);
      }
      set => this.SetXmlNodeString("d:iconSet/@iconSet", this.GetIconSetString(value));
    }

    private string GetIconSetString(T value)
    {
      if (this.Type == eExcelConditionalFormattingRuleType.FourIconSet)
      {
        switch (value.ToString())
        {
          case "Arrows":
            return "4Arrows";
          case "ArrowsGray":
            return "4ArrowsGray";
          case "Rating":
            return "4Rating";
          case "RedToBlack":
            return "4RedToBlack";
          case "TrafficLights":
            return "4TrafficLights";
          default:
            throw new ArgumentException("Invalid type");
        }
      }
      else if (this.Type == eExcelConditionalFormattingRuleType.FiveIconSet)
      {
        switch (value.ToString())
        {
          case "Arrows":
            return "5Arrows";
          case "ArrowsGray":
            return "5ArrowsGray";
          case "Quarters":
            return "5Quarters";
          case "Rating":
            return "5Rating";
          default:
            throw new ArgumentException("Invalid type");
        }
      }
      else
      {
        switch (value.ToString())
        {
          case "Arrows":
            return "3Arrows";
          case "ArrowsGray":
            return "3ArrowsGray";
          case "Flags":
            return "3Flags";
          case "Signs":
            return "3Signs";
          case "Symbols":
            return "3Symbols";
          case "Symbols2":
            return "3Symbols";
          case "TrafficLights1":
            return "3TrafficLights1";
          case "TrafficLights2":
            return "3TrafficLights2";
          default:
            throw new ArgumentException("Invalid type");
        }
      }
    }
  }
}
