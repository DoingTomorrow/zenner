// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.ExcelDataValidationDateTime
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.DataValidation.Contracts;
using OfficeOpenXml.DataValidation.Formulas;
using OfficeOpenXml.DataValidation.Formulas.Contracts;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.DataValidation
{
  public class ExcelDataValidationDateTime : 
    ExcelDataValidationWithFormula2<IExcelDataValidationFormulaDateTime>,
    IExcelDataValidationDateTime,
    IExcelDataValidationWithFormula2<IExcelDataValidationFormulaDateTime>,
    IExcelDataValidationWithFormula<IExcelDataValidationFormulaDateTime>,
    IExcelDataValidation,
    IExcelDataValidationWithOperator
  {
    internal ExcelDataValidationDateTime(
      ExcelWorksheet worksheet,
      string address,
      ExcelDataValidationType validationType)
      : base(worksheet, address, validationType)
    {
      this.Formula = (IExcelDataValidationFormulaDateTime) new ExcelDataValidationFormulaDateTime(this.NameSpaceManager, this.TopNode, this._formula1Path);
      this.Formula2 = (IExcelDataValidationFormulaDateTime) new ExcelDataValidationFormulaDateTime(this.NameSpaceManager, this.TopNode, this._formula2Path);
    }

    internal ExcelDataValidationDateTime(
      ExcelWorksheet worksheet,
      string address,
      ExcelDataValidationType validationType,
      XmlNode itemElementNode)
      : base(worksheet, address, validationType, itemElementNode)
    {
      this.Formula = (IExcelDataValidationFormulaDateTime) new ExcelDataValidationFormulaDateTime(this.NameSpaceManager, this.TopNode, this._formula1Path);
      this.Formula2 = (IExcelDataValidationFormulaDateTime) new ExcelDataValidationFormulaDateTime(this.NameSpaceManager, this.TopNode, this._formula2Path);
    }

    internal ExcelDataValidationDateTime(
      ExcelWorksheet worksheet,
      string address,
      ExcelDataValidationType validationType,
      XmlNode itemElementNode,
      XmlNamespaceManager namespaceManager)
      : base(worksheet, address, validationType, itemElementNode, namespaceManager)
    {
      this.Formula = (IExcelDataValidationFormulaDateTime) new ExcelDataValidationFormulaDateTime(this.NameSpaceManager, this.TopNode, this._formula1Path);
      this.Formula2 = (IExcelDataValidationFormulaDateTime) new ExcelDataValidationFormulaDateTime(this.NameSpaceManager, this.TopNode, this._formula2Path);
    }
  }
}
