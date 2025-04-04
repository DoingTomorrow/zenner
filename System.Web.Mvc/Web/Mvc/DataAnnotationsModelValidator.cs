// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.DataAnnotationsModelValidator
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  public class DataAnnotationsModelValidator : ModelValidator
  {
    public DataAnnotationsModelValidator(
      ModelMetadata metadata,
      ControllerContext context,
      ValidationAttribute attribute)
      : base(metadata, context)
    {
      this.Attribute = attribute != null ? attribute : throw new ArgumentNullException(nameof (attribute));
    }

    protected internal ValidationAttribute Attribute { get; private set; }

    protected internal string ErrorMessage
    {
      get => this.Attribute.FormatErrorMessage(this.Metadata.GetDisplayName());
    }

    public override bool IsRequired => this.Attribute is RequiredAttribute;

    internal static ModelValidator Create(
      ModelMetadata metadata,
      ControllerContext context,
      ValidationAttribute attribute)
    {
      return (ModelValidator) new DataAnnotationsModelValidator(metadata, context, attribute);
    }

    public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
    {
      IEnumerable<ModelClientValidationRule> first = base.GetClientValidationRules();
      if (this.Attribute is IClientValidatable attribute)
        first = first.Concat<ModelClientValidationRule>(attribute.GetClientValidationRules(this.Metadata, this.ControllerContext));
      return first;
    }

    public override IEnumerable<ModelValidationResult> Validate(object container)
    {
      ValidationResult result = this.Attribute.GetValidationResult(this.Metadata.Model, new ValidationContext(container ?? this.Metadata.Model, (IServiceProvider) null, (IDictionary<object, object>) null)
      {
        DisplayName = this.Metadata.GetDisplayName()
      });
      if (result != ValidationResult.Success)
        yield return new ModelValidationResult()
        {
          Message = result.ErrorMessage
        };
    }
  }
}
