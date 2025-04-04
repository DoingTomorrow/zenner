// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.Contracts.IExcelDataValidation
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.DataValidation.Contracts
{
  public interface IExcelDataValidation
  {
    ExcelAddress Address { get; }

    ExcelDataValidationType ValidationType { get; }

    ExcelDataValidationWarningStyle ErrorStyle { get; set; }

    bool? AllowBlank { get; set; }

    bool? ShowInputMessage { get; set; }

    bool? ShowErrorMessage { get; set; }

    string ErrorTitle { get; set; }

    string Error { get; set; }

    string PromptTitle { get; set; }

    string Prompt { get; set; }

    bool AllowsOperator { get; }

    void Validate();
  }
}
