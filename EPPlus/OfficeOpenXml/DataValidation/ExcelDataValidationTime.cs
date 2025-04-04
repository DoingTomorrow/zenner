// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.ExcelDataValidationTime
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
  public class ExcelDataValidationTime : 
    ExcelDataValidationWithFormula2<IExcelDataValidationFormulaTime>,
    IExcelDataValidationTime,
    IExcelDataValidationWithFormula2<IExcelDataValidationFormulaTime>,
    IExcelDataValidationWithFormula<IExcelDataValidationFormulaTime>,
    IExcelDataValidation,
    IExcelDataValidationWithOperator
  {
    internal ExcelDataValidationTime(
      ExcelWorksheet worksheet,
      string address,
      ExcelDataValidationType validationType)
      : base(worksheet, address, validationType)
    {
      this.Formula = (IExcelDataValidationFormulaTime) new ExcelDataValidationFormulaTime(this.NameSpaceManager, this.TopNode, this._formula1Path);
      this.Formula2 = (IExcelDataValidationFormulaTime) new ExcelDataValidationFormulaTime(this.NameSpaceManager, this.TopNode, this._formula2Path);
    }

    internal ExcelDataValidationTime(
      ExcelWorksheet worksheet,
      string address,
      ExcelDataValidationType validationType,
      XmlNode itemElementNode)
      : base(worksheet, address, validationType, itemElementNode)
    {
      this.Formula = (IExcelDataValidationFormulaTime) new ExcelDataValidationFormulaTime(this.NameSpaceManager, this.TopNode, this._formula1Path);
      this.Formula2 = (IExcelDataValidationFormulaTime) new ExcelDataValidationFormulaTime(this.NameSpaceManager, this.TopNode, this._formula2Path);
    }

    internal ExcelDataValidationTime(
      ExcelWorksheet worksheet,
      string address,
      ExcelDataValidationType validationType,
      XmlNode itemElementNode,
      XmlNamespaceManager namespaceManager)
      : base(worksheet, address, validationType, itemElementNode, namespaceManager)
    {
      this.Formula = (IExcelDataValidationFormulaTime) new ExcelDataValidationFormulaTime(this.NameSpaceManager, this.TopNode, this._formula1Path);
      this.Formula2 = (IExcelDataValidationFormulaTime) new ExcelDataValidationFormulaTime(this.NameSpaceManager, this.TopNode, this._formula2Path);
    }
  }
}
