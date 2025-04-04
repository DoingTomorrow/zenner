// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ValidatableObjectAdapter
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class ValidatableObjectAdapter(ModelMetadata metadata, ControllerContext context) : 
    ModelValidator(metadata, context)
  {
    public override IEnumerable<ModelValidationResult> Validate(object container)
    {
      object model = this.Metadata.Model;
      if (model == null)
        return Enumerable.Empty<ModelValidationResult>();
      ValidationContext validationContext = model is IValidatableObject instance ? new ValidationContext((object) instance, (IServiceProvider) null, (IDictionary<object, object>) null) : throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ValidatableObjectAdapter_IncompatibleType, new object[2]
      {
        (object) typeof (IValidatableObject).FullName,
        (object) model.GetType().FullName
      }));
      return this.ConvertResults(instance.Validate(validationContext));
    }

    private IEnumerable<ModelValidationResult> ConvertResults(IEnumerable<ValidationResult> results)
    {
      foreach (ValidationResult result in results)
      {
        if (result != ValidationResult.Success)
        {
          if (result.MemberNames == null || !result.MemberNames.Any<string>())
          {
            yield return new ModelValidationResult()
            {
              Message = result.ErrorMessage
            };
          }
          else
          {
            foreach (string memberName in result.MemberNames)
              yield return new ModelValidationResult()
              {
                Message = result.ErrorMessage,
                MemberName = memberName
              };
          }
        }
      }
    }
  }
}
