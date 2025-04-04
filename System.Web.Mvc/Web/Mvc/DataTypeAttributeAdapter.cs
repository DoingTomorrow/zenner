// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.DataTypeAttributeAdapter
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  internal class DataTypeAttributeAdapter : DataAnnotationsModelValidator
  {
    public DataTypeAttributeAdapter(
      ModelMetadata metadata,
      ControllerContext context,
      DataTypeAttribute attribute,
      string ruleName)
      : base(metadata, context, (ValidationAttribute) attribute)
    {
      this.RuleName = !string.IsNullOrEmpty(ruleName) ? ruleName : throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (ruleName));
    }

    public string RuleName { get; set; }

    public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
    {
      yield return new ModelClientValidationRule()
      {
        ValidationType = this.RuleName,
        ErrorMessage = this.ErrorMessage
      };
    }
  }
}
