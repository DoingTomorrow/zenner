// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.CompareAttributeAdapter
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace System.Web.Mvc
{
  internal class CompareAttributeAdapter(
    ModelMetadata metadata,
    ControllerContext context,
    ValidationAttribute attribute) : DataAnnotationsModelValidator(metadata, context, attribute)
  {
    private static Lazy<Func<ValidationAttribute, string>> otherProperty = new Lazy<Func<ValidationAttribute, string>>((Func<Func<ValidationAttribute, string>>) (() => ValidationAttributeHelpers.GetPropertyDelegate<string>(ValidationAttributeHelpers.CompareAttributeType, "OtherProperty")));

    public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
    {
      yield return (ModelClientValidationRule) new ModelClientValidationEqualToRule(this.ErrorMessage, (object) CompareAttributeAdapter.FormatPropertyForClientValidation(CompareAttributeAdapter.otherProperty.Value(this.Attribute)));
    }

    private static string FormatPropertyForClientValidation(string property) => "*." + property;
  }
}
