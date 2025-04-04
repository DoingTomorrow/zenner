// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.RequestFieldValidatorBase
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.Internal.Web.Utils;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;
using System.Web.Mvc;

#nullable disable
namespace System.Web.WebPages
{
  public abstract class RequestFieldValidatorBase : IValidator
  {
    private readonly string _errorMessage;
    private readonly bool _useUnvalidatedValues;

    protected RequestFieldValidatorBase(string errorMessage)
    {
      bool useUnvalidatedValues = false;
      // ISSUE: explicit constructor call
      this.\u002Ector(errorMessage, useUnvalidatedValues);
    }

    protected RequestFieldValidatorBase(string errorMessage, bool useUnvalidatedValues)
    {
      this._errorMessage = !string.IsNullOrEmpty(errorMessage) ? errorMessage : throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (errorMessage));
      this._useUnvalidatedValues = useUnvalidatedValues;
    }

    public virtual ModelClientValidationRule ClientValidationRule
    {
      get => (ModelClientValidationRule) null;
    }

    internal static bool IgnoreUseUnvalidatedValues { get; set; }

    protected abstract bool IsValid(HttpContextBase httpContext, string value);

    public virtual ValidationResult Validate(ValidationContext validationContext)
    {
      HttpContextBase httpContext = RequestFieldValidatorBase.GetHttpContext(validationContext);
      string memberName = validationContext.MemberName;
      string requestValue = this.GetRequestValue(httpContext.Request, memberName);
      if (this.IsValid(httpContext, requestValue))
        return ValidationResult.Success;
      return new ValidationResult(this._errorMessage, (IEnumerable<string>) new string[1]
      {
        memberName
      });
    }

    protected static HttpContextBase GetHttpContext(ValidationContext validationContext)
    {
      return (HttpContextBase) validationContext.ObjectInstance;
    }

    protected string GetRequestValue(HttpRequestBase request, string field)
    {
      return RequestFieldValidatorBase.IgnoreUseUnvalidatedValues || !this._useUnvalidatedValues ? request.Form[field] : request.Unvalidated(field);
    }
  }
}
