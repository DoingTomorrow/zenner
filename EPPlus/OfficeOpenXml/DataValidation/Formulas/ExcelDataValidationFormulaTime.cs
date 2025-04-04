// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.Formulas.ExcelDataValidationFormulaTime
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
  internal class ExcelDataValidationFormulaTime : 
    ExcelDataValidationFormulaValue<ExcelTime>,
    IExcelDataValidationFormulaTime,
    IExcelDataValidationFormulaWithValue<ExcelTime>,
    IExcelDataValidationFormula
  {
    public ExcelDataValidationFormulaTime(
      XmlNamespaceManager namespaceManager,
      XmlNode topNode,
      string formulaPath)
      : base(namespaceManager, topNode, formulaPath)
    {
      string xmlNodeString = this.GetXmlNodeString(formulaPath);
      if (!string.IsNullOrEmpty(xmlNodeString))
      {
        Decimal result = 0M;
        if (Decimal.TryParse(xmlNodeString, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        {
          this.Value = new ExcelTime(result);
        }
        else
        {
          this.Value = new ExcelTime();
          this.ExcelFormula = xmlNodeString;
        }
      }
      else
        this.Value = new ExcelTime();
      this.Value.TimeChanged += new EventHandler(this.Value_TimeChanged);
    }

    private void Value_TimeChanged(object sender, EventArgs e)
    {
      this.SetXmlNodeString(this.FormulaPath, this.Value.ToExcelString());
    }

    protected override string GetValueAsString()
    {
      return this.State == FormulaState.Value ? this.Value.ToExcelString() : string.Empty;
    }

    internal override void ResetValue() => this.Value = new ExcelTime();
  }
}
