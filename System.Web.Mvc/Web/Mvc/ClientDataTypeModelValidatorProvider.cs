// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ClientDataTypeModelValidatorProvider
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class ClientDataTypeModelValidatorProvider : ModelValidatorProvider
  {
    private static readonly HashSet<Type> _numericTypes = new HashSet<Type>((IEnumerable<Type>) new Type[11]
    {
      typeof (byte),
      typeof (sbyte),
      typeof (short),
      typeof (ushort),
      typeof (int),
      typeof (uint),
      typeof (long),
      typeof (ulong),
      typeof (float),
      typeof (double),
      typeof (Decimal)
    });
    private static string _resourceClassKey;

    public static string ResourceClassKey
    {
      get => ClientDataTypeModelValidatorProvider._resourceClassKey ?? string.Empty;
      set => ClientDataTypeModelValidatorProvider._resourceClassKey = value;
    }

    public override IEnumerable<ModelValidator> GetValidators(
      ModelMetadata metadata,
      ControllerContext context)
    {
      if (metadata == null)
        throw new ArgumentNullException(nameof (metadata));
      return context != null ? ClientDataTypeModelValidatorProvider.GetValidatorsImpl(metadata, context) : throw new ArgumentNullException(nameof (context));
    }

    private static IEnumerable<ModelValidator> GetValidatorsImpl(
      ModelMetadata metadata,
      ControllerContext context)
    {
      Type type = metadata.ModelType;
      if (ClientDataTypeModelValidatorProvider.IsDateTimeType(type, metadata))
        yield return (ModelValidator) new ClientDataTypeModelValidatorProvider.DateModelValidator(metadata, context);
      if (ClientDataTypeModelValidatorProvider.IsNumericType(type))
        yield return (ModelValidator) new ClientDataTypeModelValidatorProvider.NumericModelValidator(metadata, context);
    }

    private static bool IsNumericType(Type type)
    {
      return ClientDataTypeModelValidatorProvider._numericTypes.Contains(ClientDataTypeModelValidatorProvider.GetTypeToValidate(type));
    }

    private static bool IsDateTimeType(Type type, ModelMetadata metadata)
    {
      return typeof (DateTime) == ClientDataTypeModelValidatorProvider.GetTypeToValidate(type) && !string.Equals(metadata.DataTypeName, "Time", StringComparison.OrdinalIgnoreCase);
    }

    private static Type GetTypeToValidate(Type type)
    {
      Type underlyingType = Nullable.GetUnderlyingType(type);
      return (object) underlyingType != null ? underlyingType : type;
    }

    private static string GetUserResourceString(
      ControllerContext controllerContext,
      string resourceName)
    {
      string userResourceString = (string) null;
      if (!string.IsNullOrEmpty(ClientDataTypeModelValidatorProvider.ResourceClassKey) && controllerContext != null && controllerContext.HttpContext != null)
        userResourceString = controllerContext.HttpContext.GetGlobalResourceObject(ClientDataTypeModelValidatorProvider.ResourceClassKey, resourceName, CultureInfo.CurrentUICulture) as string;
      return userResourceString;
    }

    private static string GetFieldMustBeNumericResource(ControllerContext controllerContext)
    {
      return ClientDataTypeModelValidatorProvider.GetUserResourceString(controllerContext, "FieldMustBeNumeric") ?? MvcResources.ClientDataTypeModelValidatorProvider_FieldMustBeNumeric;
    }

    private static string GetFieldMustBeDateResource(ControllerContext controllerContext)
    {
      return ClientDataTypeModelValidatorProvider.GetUserResourceString(controllerContext, "FieldMustBeDate") ?? MvcResources.ClientDataTypeModelValidatorProvider_FieldMustBeDate;
    }

    internal class ClientModelValidator : ModelValidator
    {
      private string _errorMessage;
      private string _validationType;

      public ClientModelValidator(
        ModelMetadata metadata,
        ControllerContext controllerContext,
        string validationType,
        string errorMessage)
        : base(metadata, controllerContext)
      {
        if (string.IsNullOrEmpty(validationType))
          throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (validationType));
        if (string.IsNullOrEmpty(errorMessage))
          throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (errorMessage));
        this._validationType = validationType;
        this._errorMessage = errorMessage;
      }

      public override sealed IEnumerable<ModelClientValidationRule> GetClientValidationRules()
      {
        return (IEnumerable<ModelClientValidationRule>) new ModelClientValidationRule[1]
        {
          new ModelClientValidationRule()
          {
            ValidationType = this._validationType,
            ErrorMessage = this.FormatErrorMessage(this.Metadata.GetDisplayName())
          }
        };
      }

      private string FormatErrorMessage(string displayName)
      {
        return string.Format((IFormatProvider) CultureInfo.CurrentCulture, this._errorMessage, new object[1]
        {
          (object) displayName
        });
      }

      public override sealed IEnumerable<ModelValidationResult> Validate(object container)
      {
        return Enumerable.Empty<ModelValidationResult>();
      }
    }

    internal sealed class DateModelValidator(
      ModelMetadata metadata,
      ControllerContext controllerContext) : 
      ClientDataTypeModelValidatorProvider.ClientModelValidator(metadata, controllerContext, "date", ClientDataTypeModelValidatorProvider.GetFieldMustBeDateResource(controllerContext))
    {
    }

    internal sealed class NumericModelValidator(
      ModelMetadata metadata,
      ControllerContext controllerContext) : 
      ClientDataTypeModelValidatorProvider.ClientModelValidator(metadata, controllerContext, "number", ClientDataTypeModelValidatorProvider.GetFieldMustBeNumericResource(controllerContext))
    {
    }
  }
}
