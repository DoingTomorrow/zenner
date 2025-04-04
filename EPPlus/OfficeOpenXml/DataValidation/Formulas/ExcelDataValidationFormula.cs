// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.Formulas.ExcelDataValidationFormula
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Utils;
using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.DataValidation.Formulas
{
  internal abstract class ExcelDataValidationFormula : XmlHelper
  {
    private string _formula;

    public ExcelDataValidationFormula(
      XmlNamespaceManager namespaceManager,
      XmlNode topNode,
      string formulaPath)
      : base(namespaceManager, topNode)
    {
      Require.Argument<string>(formulaPath).IsNotNullOrEmpty(nameof (formulaPath));
      this.FormulaPath = formulaPath;
    }

    protected string FormulaPath { get; private set; }

    protected FormulaState State { get; set; }

    public string ExcelFormula
    {
      get => this._formula;
      set
      {
        if (!string.IsNullOrEmpty(value))
        {
          this.ResetValue();
          this.State = FormulaState.Formula;
        }
        this._formula = value == null || value.Length <= (int) byte.MaxValue ? value : throw new InvalidOperationException("The length of a DataValidation formula cannot exceed 255 characters");
        this.SetXmlNodeString(this.FormulaPath, value);
      }
    }

    internal abstract void ResetValue();

    internal virtual string GetXmlValue()
    {
      return this.State == FormulaState.Formula ? this.ExcelFormula : this.GetValueAsString();
    }

    protected abstract string GetValueAsString();
  }
}
