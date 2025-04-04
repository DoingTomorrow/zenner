// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.FileExtensionsAttributeAdapter
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace System.Web.Mvc
{
  internal class FileExtensionsAttributeAdapter(
    ModelMetadata metadata,
    ControllerContext context,
    ValidationAttribute attribute) : DataAnnotationsModelValidator(metadata, context, attribute)
  {
    private static Lazy<Func<ValidationAttribute, string>> extensions = new Lazy<Func<ValidationAttribute, string>>((Func<Func<ValidationAttribute, string>>) (() => ValidationAttributeHelpers.GetPropertyDelegate<string>(ValidationAttributeHelpers.FileExtensionsAttributeType, "Extensions")));

    public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
    {
      ModelClientValidationRule rule = new ModelClientValidationRule()
      {
        ValidationType = "accept",
        ErrorMessage = this.ErrorMessage
      };
      rule.ValidationParameters["exts"] = (object) FileExtensionsAttributeAdapter.extensions.Value(this.Attribute);
      yield return rule;
    }
  }
}
