// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.Contracts.IExcelDataValidationWithFormula2`1
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.DataValidation.Formulas.Contracts;

#nullable disable
namespace OfficeOpenXml.DataValidation.Contracts
{
  public interface IExcelDataValidationWithFormula2<T> : 
    IExcelDataValidationWithFormula<T>,
    IExcelDataValidation
    where T : IExcelDataValidationFormula
  {
    T Formula2 { get; }
  }
}
