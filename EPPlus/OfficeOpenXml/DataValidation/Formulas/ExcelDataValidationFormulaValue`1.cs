// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.Formulas.ExcelDataValidationFormulaValue`1
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Xml;

#nullable disable
namespace OfficeOpenXml.DataValidation.Formulas
{
  internal abstract class ExcelDataValidationFormulaValue<T> : ExcelDataValidationFormula
  {
    private T _value;

    public ExcelDataValidationFormulaValue(
      XmlNamespaceManager namespaceManager,
      XmlNode topNode,
      string formulaPath)
      : base(namespaceManager, topNode, formulaPath)
    {
    }

    public T Value
    {
      get => this._value;
      set
      {
        this.State = FormulaState.Value;
        this._value = value;
        this.SetXmlNodeString(this.FormulaPath, this.GetValueAsString());
      }
    }

    internal override void ResetValue() => this.Value = default (T);
  }
}
