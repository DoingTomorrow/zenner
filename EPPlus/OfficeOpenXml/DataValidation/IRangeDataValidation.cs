// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.IRangeDataValidation
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.DataValidation.Contracts;

#nullable disable
namespace OfficeOpenXml.DataValidation
{
  public interface IRangeDataValidation
  {
    IExcelDataValidationInt AddIntegerDataValidation();

    IExcelDataValidationDecimal AddDecimalDataValidation();

    IExcelDataValidationDateTime AddDateTimeDataValidation();

    IExcelDataValidationList AddListDataValidation();

    IExcelDataValidationInt AddTextLengthDataValidation();

    IExcelDataValidationTime AddTimeDataValidation();

    IExcelDataValidationCustom AddCustomDataValidation();
  }
}
