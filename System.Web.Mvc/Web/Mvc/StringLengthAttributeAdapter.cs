// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.StringLengthAttributeAdapter
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace System.Web.Mvc
{
  public class StringLengthAttributeAdapter(
    ModelMetadata metadata,
    ControllerContext context,
    StringLengthAttribute attribute) : DataAnnotationsModelValidator<StringLengthAttribute>(metadata, context, attribute)
  {
    public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
    {
      return (IEnumerable<ModelClientValidationRule>) new ModelClientValidationStringLengthRule[1]
      {
        new ModelClientValidationStringLengthRule(this.ErrorMessage, this.Attribute.MinimumLength, this.Attribute.MaximumLength)
      };
    }
  }
}
