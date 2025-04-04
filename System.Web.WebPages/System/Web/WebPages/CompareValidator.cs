// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.CompareValidator
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Web.Mvc;

#nullable disable
namespace System.Web.WebPages
{
  internal class CompareValidator : RequestFieldValidatorBase
  {
    private readonly string _otherField;
    private readonly ModelClientValidationEqualToRule _clientValidationRule;

    public CompareValidator(string otherField, string errorMessage)
      : base(errorMessage)
    {
      this._otherField = otherField;
      this._clientValidationRule = new ModelClientValidationEqualToRule(errorMessage, (object) otherField);
    }

    public override ModelClientValidationRule ClientValidationRule
    {
      get => (ModelClientValidationRule) this._clientValidationRule;
    }

    protected override bool IsValid(HttpContextBase httpContext, string value)
    {
      string requestValue = this.GetRequestValue(httpContext.Request, this._otherField);
      return string.Equals(value, requestValue, StringComparison.CurrentCulture);
    }
  }
}
