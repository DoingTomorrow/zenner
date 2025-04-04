// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.DataErrorInfoModelValidatorProvider
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  public class DataErrorInfoModelValidatorProvider : ModelValidatorProvider
  {
    public override IEnumerable<ModelValidator> GetValidators(
      ModelMetadata metadata,
      ControllerContext context)
    {
      if (metadata == null)
        throw new ArgumentNullException(nameof (metadata));
      return context != null ? DataErrorInfoModelValidatorProvider.GetValidatorsImpl(metadata, context) : throw new ArgumentNullException(nameof (context));
    }

    private static IEnumerable<ModelValidator> GetValidatorsImpl(
      ModelMetadata metadata,
      ControllerContext context)
    {
      if (DataErrorInfoModelValidatorProvider.TypeImplementsIDataErrorInfo(metadata.ModelType))
        yield return (ModelValidator) new DataErrorInfoModelValidatorProvider.DataErrorInfoClassModelValidator(metadata, context);
      if (DataErrorInfoModelValidatorProvider.TypeImplementsIDataErrorInfo(metadata.ContainerType))
        yield return (ModelValidator) new DataErrorInfoModelValidatorProvider.DataErrorInfoPropertyModelValidator(metadata, context);
    }

    private static bool TypeImplementsIDataErrorInfo(Type type)
    {
      return typeof (IDataErrorInfo).IsAssignableFrom(type);
    }

    internal sealed class DataErrorInfoClassModelValidator(
      ModelMetadata metadata,
      ControllerContext controllerContext) : ModelValidator(metadata, controllerContext)
    {
      public override IEnumerable<ModelValidationResult> Validate(object container)
      {
        if (this.Metadata.Model is IDataErrorInfo model)
        {
          string error = model.Error;
          if (!string.IsNullOrEmpty(error))
            return (IEnumerable<ModelValidationResult>) new ModelValidationResult[1]
            {
              new ModelValidationResult() { Message = error }
            };
        }
        return Enumerable.Empty<ModelValidationResult>();
      }
    }

    internal sealed class DataErrorInfoPropertyModelValidator(
      ModelMetadata metadata,
      ControllerContext controllerContext) : ModelValidator(metadata, controllerContext)
    {
      public override IEnumerable<ModelValidationResult> Validate(object container)
      {
        if (container is IDataErrorInfo dataErrorInfo && !string.Equals(this.Metadata.PropertyName, "error", StringComparison.OrdinalIgnoreCase))
        {
          string str = dataErrorInfo[this.Metadata.PropertyName];
          if (!string.IsNullOrEmpty(str))
            return (IEnumerable<ModelValidationResult>) new ModelValidationResult[1]
            {
              new ModelValidationResult() { Message = str }
            };
        }
        return Enumerable.Empty<ModelValidationResult>();
      }
    }
  }
}
