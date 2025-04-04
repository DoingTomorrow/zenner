// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.RangeDataValidation
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.DataValidation.Contracts;
using OfficeOpenXml.Utils;

#nullable disable
namespace OfficeOpenXml.DataValidation
{
  internal class RangeDataValidation : IRangeDataValidation
  {
    private ExcelWorksheet _worksheet;
    private string _address;

    public RangeDataValidation(ExcelWorksheet worksheet, string address)
    {
      Require.Argument<ExcelWorksheet>(worksheet).IsNotNull<ExcelWorksheet>(nameof (worksheet));
      Require.Argument<string>(address).IsNotNullOrEmpty(nameof (address));
      this._worksheet = worksheet;
      this._address = address;
    }

    public IExcelDataValidationInt AddIntegerDataValidation()
    {
      return this._worksheet.DataValidations.AddIntegerValidation(this._address);
    }

    public IExcelDataValidationDecimal AddDecimalDataValidation()
    {
      return this._worksheet.DataValidations.AddDecimalValidation(this._address);
    }

    public IExcelDataValidationDateTime AddDateTimeDataValidation()
    {
      return this._worksheet.DataValidations.AddDateTimeValidation(this._address);
    }

    public IExcelDataValidationList AddListDataValidation()
    {
      return this._worksheet.DataValidations.AddListValidation(this._address);
    }

    public IExcelDataValidationInt AddTextLengthDataValidation()
    {
      return this._worksheet.DataValidations.AddTextLengthValidation(this._address);
    }

    public IExcelDataValidationTime AddTimeDataValidation()
    {
      return this._worksheet.DataValidations.AddTimeValidation(this._address);
    }

    public IExcelDataValidationCustom AddCustomDataValidation()
    {
      return this._worksheet.DataValidations.AddCustomValidation(this._address);
    }
  }
}
