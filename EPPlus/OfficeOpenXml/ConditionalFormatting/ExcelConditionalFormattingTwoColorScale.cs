// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingTwoColorScale
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.ConditionalFormatting.Contracts;
using System.Drawing;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  public class ExcelConditionalFormattingTwoColorScale : 
    ExcelConditionalFormattingRule,
    IExcelConditionalFormattingTwoColorScale,
    IExcelConditionalFormattingColorScaleGroup,
    IExcelConditionalFormattingRule
  {
    private ExcelConditionalFormattingColorScaleValue _lowValue;
    private ExcelConditionalFormattingColorScaleValue _highValue;

    internal ExcelConditionalFormattingTwoColorScale(
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode,
      XmlNamespaceManager namespaceManager)
      : base(eExcelConditionalFormattingRuleType.TwoColorScale, address, priority, worksheet, itemElementNode, namespaceManager == null ? worksheet.NameSpaceManager : namespaceManager)
    {
      this.CreateComplexNode(this.Node, "d:colorScale");
      this.LowValue = new ExcelConditionalFormattingColorScaleValue(eExcelConditionalFormattingValueObjectPosition.Low, eExcelConditionalFormattingValueObjectType.Min, ColorTranslator.FromHtml("#FFF8696B"), eExcelConditionalFormattingRuleType.TwoColorScale, address, priority, worksheet, this.NameSpaceManager);
      this.HighValue = new ExcelConditionalFormattingColorScaleValue(eExcelConditionalFormattingValueObjectPosition.High, eExcelConditionalFormattingValueObjectType.Max, ColorTranslator.FromHtml("#FF63BE7B"), eExcelConditionalFormattingRuleType.TwoColorScale, address, priority, worksheet, this.NameSpaceManager);
    }

    internal ExcelConditionalFormattingTwoColorScale(
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet,
      XmlNode itemElementNode)
      : this(address, priority, worksheet, itemElementNode, (XmlNamespaceManager) null)
    {
    }

    internal ExcelConditionalFormattingTwoColorScale(
      ExcelAddress address,
      int priority,
      ExcelWorksheet worksheet)
      : this(address, priority, worksheet, (XmlNode) null, (XmlNamespaceManager) null)
    {
    }

    public ExcelConditionalFormattingColorScaleValue LowValue
    {
      get => this._lowValue;
      set => this._lowValue = value;
    }

    public ExcelConditionalFormattingColorScaleValue HighValue
    {
      get => this._highValue;
      set => this._highValue = value;
    }
  }
}
