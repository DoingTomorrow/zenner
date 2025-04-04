// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.Formulas.ExcelDataValidationFormulaDateTime
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.DataValidation.Formulas.Contracts;
using System;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.DataValidation.Formulas
{
  internal class ExcelDataValidationFormulaDateTime : 
    ExcelDataValidationFormulaValue<DateTime?>,
    IExcelDataValidationFormulaDateTime,
    IExcelDataValidationFormulaWithValue<DateTime?>,
    IExcelDataValidationFormula
  {
    public ExcelDataValidationFormulaDateTime(
      XmlNamespaceManager namespaceManager,
      XmlNode topNode,
      string formulaPath)
      : base(namespaceManager, topNode, formulaPath)
    {
      string xmlNodeString = this.GetXmlNodeString(formulaPath);
      if (string.IsNullOrEmpty(xmlNodeString))
        return;
      double result = 0.0;
      if (double.TryParse(xmlNodeString, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        this.Value = new DateTime?(DateTime.FromOADate(result));
      else
        this.ExcelFormula = xmlNodeString;
    }

    protected override string GetValueAsString()
    {
      return !this.Value.HasValue ? string.Empty : this.Value.Value.ToOADate().ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }
  }
}
