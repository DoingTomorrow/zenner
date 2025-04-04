// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.Contracts.IExcelConditionalFormattingRule
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Style.Dxf;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting.Contracts
{
  public interface IExcelConditionalFormattingRule
  {
    XmlNode Node { get; }

    eExcelConditionalFormattingRuleType Type { get; }

    ExcelAddress Address { get; set; }

    int Priority { get; set; }

    bool StopIfTrue { get; set; }

    ExcelDxfStyleConditionalFormatting Style { get; }
  }
}
