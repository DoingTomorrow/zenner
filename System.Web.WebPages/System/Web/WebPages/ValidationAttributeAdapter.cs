// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.ValidationAttributeAdapter
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

#nullable disable
namespace System.Web.WebPages
{
  internal class ValidationAttributeAdapter : RequestFieldValidatorBase
  {
    private readonly ValidationAttribute _attribute;
    private readonly ModelClientValidationRule _clientValidationRule;

    public ValidationAttributeAdapter(
      ValidationAttribute attribute,
      string errorMessage,
      ModelClientValidationRule clientValidationRule)
    {
      bool useUnvalidatedValues = false;
      // ISSUE: explicit constructor call
      this.\u002Ector(attribute, errorMessage, clientValidationRule, useUnvalidatedValues);
    }

    public ValidationAttributeAdapter(
      ValidationAttribute attribute,
      string errorMessage,
      ModelClientValidationRule clientValidationRule,
      bool useUnvalidatedValues)
      : base(errorMessage, useUnvalidatedValues)
    {
      this._attribute = attribute;
      this._clientValidationRule = clientValidationRule;
    }

    public ValidationAttribute Attribute => this._attribute;

    public override ModelClientValidationRule ClientValidationRule => this._clientValidationRule;

    protected override bool IsValid(HttpContextBase httpContext, string value)
    {
      return this._attribute.IsValid((object) value);
    }
  }
}
