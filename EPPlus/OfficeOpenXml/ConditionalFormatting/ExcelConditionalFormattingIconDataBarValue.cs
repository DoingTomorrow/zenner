// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingIconDataBarValue
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Utils;
using System;
using System.Drawing;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  public class ExcelConditionalFormattingIconDataBarValue : XmlHelper
  {
    private eExcelConditionalFormattingRuleType _ruleType;
    private ExcelWorksheet _worksheet;

    internal ExcelConditionalFormattingIconDataBarValue(
      eExcelConditionalFormattingValueObjectType type,
      double value,
      string formula,
      eExcelConditionalFormattingRuleType ruleType,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode,
      XmlNamespaceManager namespaceManager)
      : this(ruleType, address, worksheet, itemElementNode, namespaceManager)
    {
      Require.Argument<int>(priority).IsInRange<int>(1, int.MaxValue, nameof (priority));
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
      this.RuleType = ruleType;
      this.Type = type;
      this.Value = value;
      this.Formula = formula;
    }

    internal ExcelConditionalFormattingIconDataBarValue(
      eExcelConditionalFormattingRuleType ruleType,
      ExcelAddress address,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode,
      XmlNamespaceManager namespaceManager)
      : base(namespaceManager, itemElementNode)
    {
      Require.Argument<ExcelAddress>(address).IsNotNull<ExcelAddress>(nameof (address));
      Require.Argument<ExcelWorksheet>(worksheet).IsNotNull<ExcelWorksheet>(nameof (worksheet));
      this._worksheet = worksheet;
      this.SchemaNodeOrder = new string[1]{ "cfvo" };
      if (itemElementNode == null && ExcelConditionalFormattingValueObjectType.GetParentPathByRuleType(ruleType) == string.Empty)
        throw new Exception("Missing 'cfvo' parent node in Conditional Formatting");
      this.RuleType = ruleType;
    }

    internal ExcelConditionalFormattingIconDataBarValue(
      eExcelConditionalFormattingValueObjectType type,
      double value,
      string formula,
      eExcelConditionalFormattingRuleType ruleType,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNamespaceManager namespaceManager)
      : this(type, value, formula, ruleType, address, priority, worksheet, (XmlNode) null, namespaceManager)
    {
    }

    internal ExcelConditionalFormattingIconDataBarValue(
      eExcelConditionalFormattingValueObjectType type,
      Color color,
      eExcelConditionalFormattingRuleType ruleType,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNamespaceManager namespaceManager)
      : this(type, 0.0, (string) null, ruleType, address, priority, worksheet, (XmlNode) null, namespaceManager)
    {
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
        return ExcelConditionalFormattingValueObjectType.GetTypeByAttrbiute(this.GetXmlNodeString("@type"));
      }
      set
      {
        if ((this._ruleType == eExcelConditionalFormattingRuleType.ThreeIconSet || this._ruleType == eExcelConditionalFormattingRuleType.FourIconSet || this._ruleType == eExcelConditionalFormattingRuleType.FiveIconSet) && (value == eExcelConditionalFormattingValueObjectType.Min || value == eExcelConditionalFormattingValueObjectType.Max))
          throw new ArgumentException("Value type can't be Min or Max for icon sets");
        this.SetXmlNodeString("@type", value.ToString().ToLower());
      }
    }

    public double Value
    {
      get
      {
        return this.Type == eExcelConditionalFormattingValueObjectType.Num || this.Type == eExcelConditionalFormattingValueObjectType.Percent || this.Type == eExcelConditionalFormattingValueObjectType.Percentile ? this.GetXmlNodeDouble("@val") : 0.0;
      }
      set
      {
        string empty = string.Empty;
        if (this.Type == eExcelConditionalFormattingValueObjectType.Num || this.Type == eExcelConditionalFormattingValueObjectType.Percent || this.Type == eExcelConditionalFormattingValueObjectType.Percentile)
          empty = value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        this.SetXmlNodeString("@val", empty);
      }
    }

    public string Formula
    {
      get
      {
        return this.Type != eExcelConditionalFormattingValueObjectType.Formula ? string.Empty : this.GetXmlNodeString("@val");
      }
      set
      {
        if (this.Type != eExcelConditionalFormattingValueObjectType.Formula)
          return;
        this.SetXmlNodeString("@val", value);
      }
    }
  }
}
