// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingRule
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.ConditionalFormatting.Contracts;
using OfficeOpenXml.Style.Dxf;
using OfficeOpenXml.Utils;
using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  public abstract class ExcelConditionalFormattingRule : XmlHelper, IExcelConditionalFormattingRule
  {
    private eExcelConditionalFormattingRuleType? _type;
    private ExcelWorksheet _worksheet;
    private static bool _changingPriority;
    internal ExcelDxfStyleConditionalFormatting _style;

    internal ExcelConditionalFormattingRule(
      eExcelConditionalFormattingRuleType type,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode,
      XmlNamespaceManager namespaceManager)
      : base(namespaceManager, itemElementNode)
    {
      Require.Argument<ExcelAddress>(address).IsNotNull<ExcelAddress>(nameof (address));
      Require.Argument<int>(priority).IsInRange<int>(1, int.MaxValue, nameof (priority));
      Require.Argument<ExcelWorksheet>(worksheet).IsNotNull<ExcelWorksheet>(nameof (worksheet));
      this._type = new eExcelConditionalFormattingRuleType?(type);
      this._worksheet = worksheet;
      this.SchemaNodeOrder = this._worksheet.SchemaNodeOrder;
      if (itemElementNode == null)
        itemElementNode = this.CreateComplexNode((XmlNode) this._worksheet.WorksheetXml.DocumentElement, string.Format("{0}[{1}='{2}']/{1}='{2}'/{3}[{4}='{5}']/{4}='{5}'", (object) "d:conditionalFormatting", (object) "@sqref", (object) address.AddressSpaceSeparated, (object) "d:cfRule", (object) "@priority", (object) priority));
      this.TopNode = itemElementNode;
      this.Address = address;
      this.Priority = priority;
      this.Type = type;
      if (this.DxfId < 0)
        return;
      worksheet.Workbook.Styles.Dxfs[this.DxfId].AllowChange = true;
      this._style = worksheet.Workbook.Styles.Dxfs[this.DxfId].Clone();
    }

    internal ExcelConditionalFormattingRule(
      eExcelConditionalFormattingRuleType type,
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNamespaceManager namespaceManager)
      : this(type, address, priority, worksheet, (XmlNode) null, namespaceManager)
    {
    }

    public XmlNode Node => this.TopNode;

    public ExcelAddress Address
    {
      get => new ExcelAddress(this.Node.ParentNode.Attributes["sqref"].Value);
      set
      {
        if (!(this.Address.Address != value.Address))
          return;
        XmlNode node = this.Node;
        XmlNode parentNode = this.Node.ParentNode;
        this.TopNode = this.CreateComplexNode((XmlNode) this._worksheet.WorksheetXml.DocumentElement, string.Format("{0}[{1}='{2}']/{1}='{2}'", (object) "d:conditionalFormatting", (object) "@sqref", (object) value.AddressSpaceSeparated)).AppendChild(this.Node);
        if (parentNode.HasChildNodes)
          return;
        parentNode.ParentNode.RemoveChild(parentNode);
      }
    }

    public eExcelConditionalFormattingRuleType Type
    {
      get
      {
        if (!this._type.HasValue)
          this._type = new eExcelConditionalFormattingRuleType?(ExcelConditionalFormattingRuleType.GetTypeByAttrbiute(this.GetXmlNodeString("@type"), this.TopNode, this._worksheet.NameSpaceManager));
        return this._type.Value;
      }
      internal set
      {
        this._type = new eExcelConditionalFormattingRuleType?(value);
        this.SetXmlNodeString("@type", ExcelConditionalFormattingRuleType.GetAttributeByType(value), true);
      }
    }

    public int Priority
    {
      get => this.GetXmlNodeInt("@priority");
      set
      {
        int priority1 = this.Priority;
        if (priority1 == value)
          return;
        if (!ExcelConditionalFormattingRule._changingPriority)
        {
          if (value < 1)
            throw new IndexOutOfRangeException("Invalid priority number. Must be bigger than zero");
          ExcelConditionalFormattingRule._changingPriority = true;
          if (priority1 > value)
          {
            for (int priority2 = priority1 - 1; priority2 >= value; --priority2)
            {
              IExcelConditionalFormattingRule conditionalFormattingRule = this._worksheet.ConditionalFormatting.RulesByPriority(priority2);
              if (conditionalFormattingRule != null)
                ++conditionalFormattingRule.Priority;
            }
          }
          else
          {
            for (int priority3 = priority1 + 1; priority3 <= value; ++priority3)
            {
              IExcelConditionalFormattingRule conditionalFormattingRule = this._worksheet.ConditionalFormatting.RulesByPriority(priority3);
              if (conditionalFormattingRule != null)
                --conditionalFormattingRule.Priority;
            }
          }
          ExcelConditionalFormattingRule._changingPriority = false;
        }
        this.SetXmlNodeString("@priority", value.ToString(), true);
      }
    }

    public bool StopIfTrue
    {
      get => this.GetXmlNodeBool("@stopIfTrue");
      set => this.SetXmlNodeString("@stopIfTrue", value ? "1" : string.Empty, true);
    }

    internal int DxfId
    {
      get => this.GetXmlNodeInt("@dxfId");
      set
      {
        this.SetXmlNodeString("@dxfId", value == int.MinValue ? string.Empty : value.ToString(), true);
      }
    }

    public ExcelDxfStyleConditionalFormatting Style
    {
      get
      {
        if (this._style == null)
          this._style = new ExcelDxfStyleConditionalFormatting(this.NameSpaceManager, (XmlNode) null, this._worksheet.Workbook.Styles);
        return this._style;
      }
    }

    public ushort StdDev
    {
      get => Convert.ToUInt16(this.GetXmlNodeInt("@stdDev"));
      set => this.SetXmlNodeString("@stdDev", value == (ushort) 0 ? "1" : value.ToString(), true);
    }

    public ushort Rank
    {
      get => Convert.ToUInt16(this.GetXmlNodeInt("@rank"));
      set => this.SetXmlNodeString("@rank", value == (ushort) 0 ? "1" : value.ToString(), true);
    }

    protected internal bool? AboveAverage
    {
      get
      {
        bool? nodeBoolNullable = this.GetXmlNodeBoolNullable("@aboveAverage");
        bool? nullable = nodeBoolNullable;
        return new bool?((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) != 0 || !nodeBoolNullable.HasValue);
      }
      set
      {
        string str = string.Empty;
        eExcelConditionalFormattingRuleType? type1 = this._type;
        if ((type1.GetValueOrDefault() != eExcelConditionalFormattingRuleType.BelowAverage ? 0 : (type1.HasValue ? 1 : 0)) == 0)
        {
          eExcelConditionalFormattingRuleType? type2 = this._type;
          if ((type2.GetValueOrDefault() != eExcelConditionalFormattingRuleType.BelowOrEqualAverage ? 0 : (type2.HasValue ? 1 : 0)) == 0)
          {
            eExcelConditionalFormattingRuleType? type3 = this._type;
            if ((type3.GetValueOrDefault() != eExcelConditionalFormattingRuleType.BelowStdDev ? 0 : (type3.HasValue ? 1 : 0)) == 0)
              goto label_4;
          }
        }
        str = "0";
label_4:
        this.SetXmlNodeString("@aboveAverage", str, true);
      }
    }

    protected internal bool? EqualAverage
    {
      get
      {
        bool? nodeBoolNullable = this.GetXmlNodeBoolNullable("@equalAverage");
        return new bool?(nodeBoolNullable.GetValueOrDefault() && nodeBoolNullable.HasValue);
      }
      set
      {
        string str = string.Empty;
        eExcelConditionalFormattingRuleType? type1 = this._type;
        if ((type1.GetValueOrDefault() != eExcelConditionalFormattingRuleType.AboveOrEqualAverage ? 0 : (type1.HasValue ? 1 : 0)) == 0)
        {
          eExcelConditionalFormattingRuleType? type2 = this._type;
          if ((type2.GetValueOrDefault() != eExcelConditionalFormattingRuleType.BelowOrEqualAverage ? 0 : (type2.HasValue ? 1 : 0)) == 0)
            goto label_3;
        }
        str = "1";
label_3:
        this.SetXmlNodeString("@equalAverage", str, true);
      }
    }

    protected internal bool? Bottom
    {
      get
      {
        bool? nodeBoolNullable = this.GetXmlNodeBoolNullable("@bottom");
        return new bool?(nodeBoolNullable.GetValueOrDefault() && nodeBoolNullable.HasValue);
      }
      set
      {
        string str = string.Empty;
        eExcelConditionalFormattingRuleType? type1 = this._type;
        if ((type1.GetValueOrDefault() != eExcelConditionalFormattingRuleType.Bottom ? 0 : (type1.HasValue ? 1 : 0)) == 0)
        {
          eExcelConditionalFormattingRuleType? type2 = this._type;
          if ((type2.GetValueOrDefault() != eExcelConditionalFormattingRuleType.BottomPercent ? 0 : (type2.HasValue ? 1 : 0)) == 0)
            goto label_3;
        }
        str = "1";
label_3:
        this.SetXmlNodeString("@bottom", str, true);
      }
    }

    protected internal bool? Percent
    {
      get
      {
        bool? nodeBoolNullable = this.GetXmlNodeBoolNullable("@percent");
        return new bool?(nodeBoolNullable.GetValueOrDefault() && nodeBoolNullable.HasValue);
      }
      set
      {
        string str = string.Empty;
        eExcelConditionalFormattingRuleType? type1 = this._type;
        if ((type1.GetValueOrDefault() != eExcelConditionalFormattingRuleType.BottomPercent ? 0 : (type1.HasValue ? 1 : 0)) == 0)
        {
          eExcelConditionalFormattingRuleType? type2 = this._type;
          if ((type2.GetValueOrDefault() != eExcelConditionalFormattingRuleType.TopPercent ? 0 : (type2.HasValue ? 1 : 0)) == 0)
            goto label_3;
        }
        str = "1";
label_3:
        this.SetXmlNodeString("@percent", str, true);
      }
    }

    protected internal eExcelConditionalFormattingTimePeriodType TimePeriod
    {
      get
      {
        return ExcelConditionalFormattingTimePeriodType.GetTypeByAttribute(this.GetXmlNodeString("@timePeriod"));
      }
      set
      {
        this.SetXmlNodeString("@timePeriod", ExcelConditionalFormattingTimePeriodType.GetAttributeByType(value), true);
      }
    }

    protected internal eExcelConditionalFormattingOperatorType Operator
    {
      get
      {
        return ExcelConditionalFormattingOperatorType.GetTypeByAttribute(this.GetXmlNodeString("@operator"));
      }
      set
      {
        this.SetXmlNodeString("@operator", ExcelConditionalFormattingOperatorType.GetAttributeByType(value), true);
      }
    }

    public string Formula
    {
      get => this.GetXmlNodeString("d:formula");
      set => this.SetXmlNodeString("d:formula", value);
    }

    public string Formula2
    {
      get => this.GetXmlNodeString(string.Format("{0}[position()=2]", (object) "d:formula"));
      set
      {
        this.CreateComplexNode(this.TopNode, string.Format("{0}[position()=1]", (object) "d:formula"));
        this.CreateComplexNode(this.TopNode, string.Format("{0}[position()=2]", (object) "d:formula")).InnerText = value;
      }
    }
  }
}
