// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.DataTypeUtil
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace System.Web.Mvc
{
  internal static class DataTypeUtil
  {
    internal static readonly string CurrencyTypeName = DataType.Currency.ToString();
    internal static readonly string DateTypeName = DataType.Date.ToString();
    internal static readonly string DateTimeTypeName = DataType.DateTime.ToString();
    internal static readonly string DurationTypeName = DataType.Duration.ToString();
    internal static readonly string EmailAddressTypeName = DataType.EmailAddress.ToString();
    internal static readonly string HtmlTypeName = DataType.Html.ToString();
    internal static readonly string ImageUrlTypeName = DataType.ImageUrl.ToString();
    internal static readonly string MultiLineTextTypeName = DataType.MultilineText.ToString();
    internal static readonly string PasswordTypeName = DataType.Password.ToString();
    internal static readonly string PhoneNumberTypeName = DataType.PhoneNumber.ToString();
    internal static readonly string TextTypeName = DataType.Text.ToString();
    internal static readonly string TimeTypeName = DataType.Time.ToString();
    internal static readonly string UrlTypeName = DataType.Url.ToString();
    private static readonly Lazy<Dictionary<object, string>> _dataTypeToName = new Lazy<Dictionary<object, string>>(new Func<Dictionary<object, string>>(DataTypeUtil.CreateDataTypeToName), true);

    internal static string ToDataTypeName(
      this DataTypeAttribute attribute,
      Func<DataTypeAttribute, bool> isDataType = null)
    {
      if (isDataType == null)
        isDataType = (Func<DataTypeAttribute, bool>) (t => t.GetType().Equals(typeof (DataTypeAttribute)));
      if (isDataType(attribute))
      {
        string dataTypeName = DataTypeUtil.KnownDataTypeToString(attribute.DataType);
        if (dataTypeName == null)
          DataTypeUtil._dataTypeToName.Value.TryGetValue((object) attribute.DataType, out dataTypeName);
        if (dataTypeName != null)
          return dataTypeName;
      }
      return attribute.GetDataTypeName();
    }

    private static string KnownDataTypeToString(DataType dataType)
    {
      switch (dataType)
      {
        case DataType.DateTime:
          return DataTypeUtil.DateTimeTypeName;
        case DataType.Date:
          return DataTypeUtil.DateTypeName;
        case DataType.Time:
          return DataTypeUtil.TimeTypeName;
        case DataType.Duration:
          return DataTypeUtil.DurationTypeName;
        case DataType.PhoneNumber:
          return DataTypeUtil.PhoneNumberTypeName;
        case DataType.Currency:
          return DataTypeUtil.CurrencyTypeName;
        case DataType.Text:
          return DataTypeUtil.TextTypeName;
        case DataType.Html:
          return DataTypeUtil.HtmlTypeName;
        case DataType.MultilineText:
          return DataTypeUtil.MultiLineTextTypeName;
        case DataType.EmailAddress:
          return DataTypeUtil.EmailAddressTypeName;
        case DataType.Password:
          return DataTypeUtil.PasswordTypeName;
        case DataType.Url:
          return DataTypeUtil.UrlTypeName;
        case DataType.ImageUrl:
          return DataTypeUtil.ImageUrlTypeName;
        default:
          return (string) null;
      }
    }

    private static Dictionary<object, string> CreateDataTypeToName()
    {
      Dictionary<object, string> dataTypeToName = new Dictionary<object, string>();
      foreach (DataType dataType in Enum.GetValues(typeof (DataType)))
      {
        if (dataType != DataType.Custom && DataTypeUtil.KnownDataTypeToString(dataType) == null)
        {
          string name = Enum.GetName(typeof (DataType), (object) dataType);
          dataTypeToName[(object) dataType] = name;
        }
      }
      return dataTypeToName;
    }
  }
}
