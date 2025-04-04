// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.ExcelDataValidationWithFormula2`1
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.DataValidation.Formulas.Contracts;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.DataValidation
{
  public class ExcelDataValidationWithFormula2<T> : ExcelDataValidationWithFormula<T> where T : IExcelDataValidationFormula
  {
    internal ExcelDataValidationWithFormula2(
      ExcelWorksheet worksheet,
      string address,
      ExcelDataValidationType validationType)
      : this(worksheet, address, validationType, (XmlNode) null)
    {
    }

    internal ExcelDataValidationWithFormula2(
      ExcelWorksheet worksheet,
      string address,
      ExcelDataValidationType validationType,
      XmlNode itemElementNode)
      : base(worksheet, address, validationType, itemElementNode)
    {
    }

    internal ExcelDataValidationWithFormula2(
      ExcelWorksheet worksheet,
      string address,
      ExcelDataValidationType validationType,
      XmlNode itemElementNode,
      XmlNamespaceManager namespaceManager)
      : base(worksheet, address, validationType, itemElementNode, namespaceManager)
    {
    }

    public T Formula2 { get; protected set; }
  }
}
