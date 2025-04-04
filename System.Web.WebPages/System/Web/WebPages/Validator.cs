// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Validator
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.Internal.Web.Utils;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.WebPages
{
  public abstract class Validator
  {
    public static IValidator Required(string errorMessage = null)
    {
      errorMessage = Validator.DefaultIfEmpty(errorMessage, WebPageResources.ValidationDefault_Required);
      ModelClientValidationRequiredRule validationRequiredRule = new ModelClientValidationRequiredRule(errorMessage);
      bool useUnvalidatedValues = true;
      return (IValidator) new ValidationAttributeAdapter((ValidationAttribute) new RequiredAttribute(), errorMessage, (ModelClientValidationRule) validationRequiredRule, useUnvalidatedValues);
    }

    public static IValidator Range(int minValue, int maxValue, string errorMessage = null)
    {
      errorMessage = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Validator.DefaultIfEmpty(errorMessage, WebPageResources.ValidationDefault_IntegerRange), new object[2]
      {
        (object) minValue,
        (object) maxValue
      });
      ModelClientValidationRangeRule validationRangeRule = new ModelClientValidationRangeRule(errorMessage, (object) minValue, (object) maxValue);
      return (IValidator) new ValidationAttributeAdapter((ValidationAttribute) new RangeAttribute(minValue, maxValue), errorMessage, (ModelClientValidationRule) validationRangeRule);
    }

    public static IValidator Range(double minValue, double maxValue, string errorMessage = null)
    {
      errorMessage = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Validator.DefaultIfEmpty(errorMessage, WebPageResources.ValidationDefault_FloatRange), new object[2]
      {
        (object) minValue,
        (object) maxValue
      });
      ModelClientValidationRangeRule validationRangeRule = new ModelClientValidationRangeRule(errorMessage, (object) minValue, (object) maxValue);
      return (IValidator) new ValidationAttributeAdapter((ValidationAttribute) new RangeAttribute(minValue, maxValue), errorMessage, (ModelClientValidationRule) validationRangeRule);
    }

    public static IValidator StringLength(int maxLength, int minLength = 0, string errorMessage = null)
    {
      if (minLength == 0)
      {
        errorMessage = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Validator.DefaultIfEmpty(errorMessage, WebPageResources.ValidationDefault_StringLength), new object[1]
        {
          (object) maxLength
        });
      }
      else
      {
        errorMessage = Validator.DefaultIfEmpty(errorMessage, WebPageResources.ValidationDefault_StringLengthRange);
        errorMessage = string.Format((IFormatProvider) CultureInfo.CurrentCulture, errorMessage, new object[2]
        {
          (object) minLength,
          (object) maxLength
        });
      }
      ModelClientValidationStringLengthRule stringLengthRule = new ModelClientValidationStringLengthRule(errorMessage, minLength, maxLength);
      bool useUnvalidatedValues = true;
      return (IValidator) new ValidationAttributeAdapter((ValidationAttribute) new StringLengthAttribute(maxLength)
      {
        MinimumLength = minLength
      }, errorMessage, (ModelClientValidationRule) stringLengthRule, useUnvalidatedValues);
    }

    public static IValidator Regex(string pattern, string errorMessage = null)
    {
      if (string.IsNullOrEmpty(pattern))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (pattern));
      errorMessage = Validator.DefaultIfEmpty(errorMessage, WebPageResources.ValidationDefault_Regex);
      ModelClientValidationRegexRule validationRegexRule = new ModelClientValidationRegexRule(errorMessage, pattern);
      return (IValidator) new ValidationAttributeAdapter((ValidationAttribute) new RegularExpressionAttribute(pattern), errorMessage, (ModelClientValidationRule) validationRegexRule);
    }

    public static IValidator EqualsTo(string otherFieldName, string errorMessage = null)
    {
      if (string.IsNullOrEmpty(otherFieldName))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (otherFieldName));
      errorMessage = Validator.DefaultIfEmpty(errorMessage, WebPageResources.ValidationDefault_EqualsTo);
      return (IValidator) new CompareValidator(otherFieldName, errorMessage);
    }

    public static IValidator DateTime(string errorMessage = null)
    {
      errorMessage = Validator.DefaultIfEmpty(errorMessage, WebPageResources.ValidationDefault_DataType);
      return (IValidator) new DataTypeValidator(DataTypeValidator.SupportedValidationDataType.DateTime, errorMessage);
    }

    public static IValidator Decimal(string errorMessage = null)
    {
      errorMessage = Validator.DefaultIfEmpty(errorMessage, WebPageResources.ValidationDefault_DataType);
      return (IValidator) new DataTypeValidator(DataTypeValidator.SupportedValidationDataType.Decimal, errorMessage);
    }

    public static IValidator Integer(string errorMessage = null)
    {
      errorMessage = Validator.DefaultIfEmpty(errorMessage, WebPageResources.ValidationDefault_DataType);
      return (IValidator) new DataTypeValidator(DataTypeValidator.SupportedValidationDataType.Integer, errorMessage);
    }

    public static IValidator Url(string errorMessage = null)
    {
      errorMessage = Validator.DefaultIfEmpty(errorMessage, WebPageResources.ValidationDefault_DataType);
      return (IValidator) new DataTypeValidator(DataTypeValidator.SupportedValidationDataType.Url, errorMessage);
    }

    public static IValidator Float(string errorMessage = null)
    {
      errorMessage = Validator.DefaultIfEmpty(errorMessage, WebPageResources.ValidationDefault_DataType);
      return (IValidator) new DataTypeValidator(DataTypeValidator.SupportedValidationDataType.Float, errorMessage);
    }

    private static string DefaultIfEmpty(string errorMessage, string defaultErrorMessage)
    {
      return string.IsNullOrEmpty(errorMessage) ? defaultErrorMessage : errorMessage;
    }
  }
}
