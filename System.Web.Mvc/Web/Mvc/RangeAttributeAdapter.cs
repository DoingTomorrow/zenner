// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.RangeAttributeAdapter
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace System.Web.Mvc
{
  public class RangeAttributeAdapter(
    ModelMetadata metadata,
    ControllerContext context,
    RangeAttribute attribute) : DataAnnotationsModelValidator<RangeAttribute>(metadata, context, attribute)
  {
    public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
    {
      return (IEnumerable<ModelClientValidationRule>) new ModelClientValidationRangeRule[1]
      {
        new ModelClientValidationRangeRule(this.ErrorMessage, this.Attribute.Minimum, this.Attribute.Maximum)
      };
    }
  }
}
