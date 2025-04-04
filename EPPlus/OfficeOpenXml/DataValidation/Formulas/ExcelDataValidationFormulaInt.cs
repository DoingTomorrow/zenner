// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.Formulas.ExcelDataValidationFormulaInt
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.DataValidation.Formulas.Contracts;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.DataValidation.Formulas
{
  internal class ExcelDataValidationFormulaInt : 
    ExcelDataValidationFormulaValue<int?>,
    IExcelDataValidationFormulaInt,
    IExcelDataValidationFormulaWithValue<int?>,
    IExcelDataValidationFormula
  {
    public ExcelDataValidationFormulaInt(
      XmlNamespaceManager namespaceManager,
      XmlNode topNode,
      string formulaPath)
      : base(namespaceManager, topNode, formulaPath)
    {
      string xmlNodeString = this.GetXmlNodeString(formulaPath);
      if (string.IsNullOrEmpty(xmlNodeString))
        return;
      int result = 0;
      if (int.TryParse(xmlNodeString, out result))
        this.Value = new int?(result);
      else
        this.ExcelFormula = xmlNodeString;
    }

    protected override string GetValueAsString()
    {
      return !this.Value.HasValue ? string.Empty : this.Value.Value.ToString();
    }
  }
}
