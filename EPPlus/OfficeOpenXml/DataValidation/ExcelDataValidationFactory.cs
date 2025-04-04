// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.ExcelDataValidationFactory
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Utils;
using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.DataValidation
{
  internal static class ExcelDataValidationFactory
  {
    public static ExcelDataValidation Create(
      ExcelDataValidationType type,
      ExcelWorksheet worksheet,
      string address,
      XmlNode itemElementNode)
    {
      Require.Argument<ExcelDataValidationType>(type).IsNotNull<ExcelDataValidationType>("validationType");
      switch (type.Type)
      {
        case eDataValidationType.Whole:
        case eDataValidationType.TextLength:
          return (ExcelDataValidation) new ExcelDataValidationInt(worksheet, address, type, itemElementNode);
        case eDataValidationType.Decimal:
          return (ExcelDataValidation) new ExcelDataValidationDecimal(worksheet, address, type, itemElementNode);
        case eDataValidationType.List:
          return (ExcelDataValidation) new ExcelDataValidationList(worksheet, address, type, itemElementNode);
        case eDataValidationType.DateTime:
          return (ExcelDataValidation) new ExcelDataValidationDateTime(worksheet, address, type, itemElementNode);
        case eDataValidationType.Time:
          return (ExcelDataValidation) new ExcelDataValidationTime(worksheet, address, type, itemElementNode);
        case eDataValidationType.Custom:
          return (ExcelDataValidation) new ExcelDataValidationCustom(worksheet, address, type, itemElementNode);
        default:
          throw new InvalidOperationException("Non supported validationtype: " + type.Type.ToString());
      }
    }
  }
}
