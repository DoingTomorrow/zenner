// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingColorScaleValue
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Utils;
using System;
using System.Drawing;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  public class ExcelConditionalFormattingColorScaleValue : XmlHelper
  {
    private eExcelConditionalFormattingValueObjectPosition _position;
    private eExcelConditionalFormattingRuleType _ruleType;
    private ExcelWorksheet _worksheet;

    internal ExcelConditionalFormattingColorScaleValue(
      eExcelConditionalFormattingValueObjectPosition position,
      eExcelConditionalFormattingValueObjectType type,
      Color color,
      double value,
      string formula,
      eExcelConditionalFormattingRuleType ruleType,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode,
      XmlNamespaceManager namespaceManager)
      : base(namespaceManager, itemElementNode)
    {
      Require.Argument<int>(priority).IsInRange<int>(1, int.MaxValue, nameof (priority));
      Require.Argument<ExcelAddress>(address).IsNotNull<ExcelAddress>(nameof (address));
      Require.Argument<ExcelWorksheet>(worksheet).IsNotNull<ExcelWorksheet>(nameof (worksheet));
      this._worksheet = worksheet;
      this.SchemaNodeOrder = new string[2]
      {
        "cfvo",
        nameof (color)
      };
      if (itemElementNode == null)
      {
        string parentPathByRuleType = ExcelConditionalFormattingValueObjectType.GetParentPathByRuleType(ruleType);
        if (parentPathByRuleType == string.Empty)
          throw new Exception("Missing 'cfvo' parent node in Conditional Formatting");
        itemElementNode = this._worksheet.WorksheetXml.SelectSingleNode(string.Format("//{0}[{1}='{2}']/{3}[{4}='{5}']/{6}", (object) "d:conditionalFormatting", (object) "@sqref", (object) address.Address, (object) "d:cfRule", (object) "@priority", (object) priority, (object) parentPathByRuleType), this._worksheet.NameSpaceManager);
        if (itemElementNode == null)
          throw new Exception("Missing 'cfvo' parent node in Conditional Formatting");
      }
      this.TopNode = itemElementNode;
      this.Position = position;
      this.RuleType = ruleType;
      this.Type = type;
      this.Color = color;
      this.Value = value;
      this.Formula = formula;
    }

    internal ExcelConditionalFormattingColorScaleValue(
      eExcelConditionalFormattingValueObjectPosition position,
      eExcelConditionalFormattingValueObjectType type,
      Color color,
      double value,
      string formula,
      eExcelConditionalFormattingRuleType ruleType,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNamespaceManager namespaceManager)
      : this(position, type, color, value, formula, ruleType, address, priority, worksheet, (XmlNode) null, namespaceManager)
    {
    }

    internal ExcelConditionalFormattingColorScaleValue(
      eExcelConditionalFormattingValueObjectPosition position,
      eExcelConditionalFormattingValueObjectType type,
      Color color,
      eExcelConditionalFormattingRuleType ruleType,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNamespaceManager namespaceManager)
      : this(position, type, color, 0.0, (string) null, ruleType, address, priority, worksheet, (XmlNode) null, namespaceManager)
    {
    }

    private int GetNodeOrder()
    {
      return ExcelConditionalFormattingValueObjectType.GetOrderByPosition(this.Position, this.RuleType);
    }

    private void CreateNodeByOrdem(
      eExcelConditionalFormattingValueObjectNodeType nodeType,
      string attributePath,
      string attributeValue)
    {
      XmlNode topNode = this.TopNode;
      string nodePathByNodeType = ExcelConditionalFormattingValueObjectType.GetNodePathByNodeType(nodeType);
      int nodeOrder = this.GetNodeOrder();
      XmlHelper.eNodeInsertOrder nodeInsertOrder = XmlHelper.eNodeInsertOrder.SchemaOrder;
      XmlNode referenceNode = (XmlNode) null;
      if (nodeOrder > 1)
      {
        referenceNode = this.TopNode.SelectSingleNode(string.Format("{0}[position()={1}]", (object) nodePathByNodeType, (object) (nodeOrder - 1)), this._worksheet.NameSpaceManager);
        if (referenceNode != null)
          nodeInsertOrder = XmlHelper.eNodeInsertOrder.After;
      }
      XmlNode complexNode = this.CreateComplexNode(this.TopNode, string.Format("{0}[position()={1}]", (object) nodePathByNodeType, (object) nodeOrder), nodeInsertOrder, referenceNode);
      this.TopNode = complexNode;
      this.SetXmlNodeString(complexNode, attributePath, attributeValue, true);
      this.TopNode = topNode;
    }

    internal eExcelConditionalFormattingValueObjectPosition Position
    {
      get => this._position;
      set => this._position = value;
    }

    internal eExcelConditionalFormattingRuleType RuleType
    {
      get => this._ruleType;
      set => this._ruleType = value;
    }

    public eExcelConditionalFormattingValueObjectType Type
    {
      get
      {
        return ExcelConditionalFormattingValueObjectType.GetTypeByAttrbiute(this.GetXmlNodeString(string.Format("{0}[position()={1}]/{2}", (object) "d:cfvo", (object) this.GetNodeOrder(), (object) "@type")));
      }
      set
      {
        this.CreateNodeByOrdem(eExcelConditionalFormattingValueObjectNodeType.Cfvo, "@type", ExcelConditionalFormattingValueObjectType.GetAttributeByType(value));
        bool flag = false;
        switch (this.Type)
        {
          case eExcelConditionalFormattingValueObjectType.Max:
          case eExcelConditionalFormattingValueObjectType.Min:
            flag = true;
            break;
        }
        if (!flag)
          return;
        this.CreateComplexNode(this.TopNode, string.Format("{0}[position()={1}]/{2}=''", (object) ExcelConditionalFormattingValueObjectType.GetNodePathByNodeType(eExcelConditionalFormattingValueObjectNodeType.Cfvo), (object) this.GetNodeOrder(), (object) "@val"));
      }
    }

    public Color Color
    {
      get
      {
        return ExcelConditionalFormattingHelper.ConvertFromColorCode(this.GetXmlNodeString(string.Format("{0}[position()={1}]/{2}", (object) "d:color", (object) this.GetNodeOrder(), (object) "@rgb")));
      }
      set
      {
        this.CreateNodeByOrdem(eExcelConditionalFormattingValueObjectNodeType.Color, "@rgb", value.ToArgb().ToString("x"));
      }
    }

    public double Value
    {
      get
      {
        return this.GetXmlNodeDouble(string.Format("{0}[position()={1}]/{2}", (object) "d:cfvo", (object) this.GetNodeOrder(), (object) "@val"));
      }
      set
      {
        string empty = string.Empty;
        if (this.Type == eExcelConditionalFormattingValueObjectType.Num || this.Type == eExcelConditionalFormattingValueObjectType.Percent || this.Type == eExcelConditionalFormattingValueObjectType.Percentile)
          empty = value.ToString();
        this.CreateNodeByOrdem(eExcelConditionalFormattingValueObjectNodeType.Cfvo, "@val", empty);
      }
    }

    public string Formula
    {
      get
      {
        return this.Type != eExcelConditionalFormattingValueObjectType.Formula ? string.Empty : this.GetXmlNodeString(string.Format("{0}[position()={1}]/{2}", (object) "d:cfvo", (object) this.GetNodeOrder(), (object) "@val"));
      }
      set
      {
        if (this.Type != eExcelConditionalFormattingValueObjectType.Formula)
          return;
        this.CreateNodeByOrdem(eExcelConditionalFormattingValueObjectNodeType.Cfvo, "@val", value == null ? string.Empty : value.ToString());
      }
    }
  }
}
