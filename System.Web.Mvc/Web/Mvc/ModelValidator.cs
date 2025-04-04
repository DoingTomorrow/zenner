// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ModelValidator
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  public abstract class ModelValidator
  {
    protected ModelValidator(ModelMetadata metadata, ControllerContext controllerContext)
    {
      if (metadata == null)
        throw new ArgumentNullException(nameof (metadata));
      if (controllerContext == null)
        throw new ArgumentNullException(nameof (controllerContext));
      this.Metadata = metadata;
      this.ControllerContext = controllerContext;
    }

    protected internal ControllerContext ControllerContext { get; private set; }

    public virtual bool IsRequired => false;

    protected internal ModelMetadata Metadata { get; private set; }

    public virtual IEnumerable<ModelClientValidationRule> GetClientValidationRules()
    {
      return Enumerable.Empty<ModelClientValidationRule>();
    }

    public static ModelValidator GetModelValidator(
      ModelMetadata metadata,
      ControllerContext context)
    {
      return (ModelValidator) new ModelValidator.CompositeModelValidator(metadata, context);
    }

    public abstract IEnumerable<ModelValidationResult> Validate(object container);

    private class CompositeModelValidator(
      ModelMetadata metadata,
      ControllerContext controllerContext) : ModelValidator(metadata, controllerContext)
    {
      public override IEnumerable<ModelValidationResult> Validate(object container)
      {
        bool propertiesValid = true;
        foreach (ModelMetadata propertyMetadata in this.Metadata.Properties)
        {
          foreach (ModelValidator propertyValidator in propertyMetadata.GetValidators(this.ControllerContext))
          {
            foreach (ModelValidationResult propertyResult in propertyValidator.Validate(this.Metadata.Model))
            {
              propertiesValid = false;
              yield return new ModelValidationResult()
              {
                MemberName = DefaultModelBinder.CreateSubPropertyName(propertyMetadata.PropertyName, propertyResult.MemberName),
                Message = propertyResult.Message
              };
            }
          }
        }
        if (propertiesValid)
        {
          foreach (ModelValidator typeValidator in this.Metadata.GetValidators(this.ControllerContext))
          {
            foreach (ModelValidationResult typeResult in typeValidator.Validate(container))
              yield return typeResult;
          }
        }
      }
    }
  }
}
