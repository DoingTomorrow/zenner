// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.DataTypeValidator
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

#nullable disable
namespace System.Web.WebPages
{
  internal class DataTypeValidator : RequestFieldValidatorBase
  {
    private readonly DataTypeValidator.SupportedValidationDataType _dataType;

    public DataTypeValidator(
      DataTypeValidator.SupportedValidationDataType type,
      string errorMessage = null)
      : base(errorMessage)
    {
      this._dataType = type;
    }

    protected override bool IsValid(HttpContextBase httpContext, string value)
    {
      if (string.IsNullOrEmpty(value))
        return true;
      switch (this._dataType)
      {
        case DataTypeValidator.SupportedValidationDataType.DateTime:
          return value.IsDateTime();
        case DataTypeValidator.SupportedValidationDataType.Decimal:
          return value.IsDecimal();
        case DataTypeValidator.SupportedValidationDataType.Url:
          return Uri.IsWellFormedUriString(value, UriKind.Absolute);
        case DataTypeValidator.SupportedValidationDataType.Integer:
          return value.IsInt();
        case DataTypeValidator.SupportedValidationDataType.Float:
          return value.IsFloat();
        default:
          return true;
      }
    }

    public enum SupportedValidationDataType
    {
      DateTime,
      Decimal,
      Url,
      Integer,
      Float,
    }
  }
}
