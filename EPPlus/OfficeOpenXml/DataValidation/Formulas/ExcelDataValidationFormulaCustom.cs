// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.Formulas.ExcelDataValidationFormulaCustom
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.DataValidation.Formulas.Contracts;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.DataValidation.Formulas
{
  internal class ExcelDataValidationFormulaCustom : 
    ExcelDataValidationFormula,
    IExcelDataValidationFormula
  {
    public ExcelDataValidationFormulaCustom(
      XmlNamespaceManager namespaceManager,
      XmlNode topNode,
      string formulaPath)
      : base(namespaceManager, topNode, formulaPath)
    {
      string xmlNodeString = this.GetXmlNodeString(formulaPath);
      if (!string.IsNullOrEmpty(xmlNodeString))
        this.ExcelFormula = xmlNodeString;
      this.State = FormulaState.Formula;
    }

    internal override string GetXmlValue() => this.ExcelFormula;

    protected override string GetValueAsString() => this.ExcelFormula;

    internal override void ResetValue() => this.ExcelFormula = (string) null;
  }
}
