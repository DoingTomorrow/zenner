// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.ValidationHelper
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.Internal.Web.Utils;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using System.Web.WebPages.Scope;

#nullable disable
namespace System.Web.WebPages
{
  public sealed class ValidationHelper
  {
    private static readonly object _invalidCssClassKey = new object();
    private static readonly object _validCssClassKey = new object();
    private static IDictionary<object, object> _scopeOverride;
    private readonly Dictionary<string, List<IValidator>> _validators = new Dictionary<string, List<IValidator>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private readonly HttpContextBase _httpContext;
    private readonly ModelStateDictionary _modelStateDictionary;

    internal ValidationHelper(
      HttpContextBase httpContext,
      ModelStateDictionary modelStateDictionary)
    {
      this._httpContext = httpContext;
      this._modelStateDictionary = modelStateDictionary;
    }

    public static string ValidCssClass
    {
      get
      {
        object obj;
        return !ValidationHelper.Scope.TryGetValue(ValidationHelper._validCssClassKey, out obj) ? (string) null : obj as string;
      }
      set => ValidationHelper.Scope[ValidationHelper._validCssClassKey] = (object) value;
    }

    public static string InvalidCssClass
    {
      get
      {
        object obj;
        return !ValidationHelper.Scope.TryGetValue(ValidationHelper._invalidCssClassKey, out obj) ? "input-validation-error" : obj as string;
      }
      set => ValidationHelper.Scope[ValidationHelper._invalidCssClassKey] = (object) value;
    }

    public string FormField => "_FORM";

    internal static IDictionary<object, object> Scope
    {
      get => ValidationHelper._scopeOverride ?? ScopeStorage.CurrentScope;
    }

    public void RequireField(string field)
    {
      string errorMessage = (string) null;
      this.RequireField(field, errorMessage);
    }

    public void RequireField(string field, string errorMessage)
    {
      if (string.IsNullOrEmpty(field))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (field));
      this.Add(field, Validator.Required(errorMessage));
    }

    public void RequireFields(params string[] fields)
    {
      if (fields == null)
        throw new ArgumentNullException(nameof (fields));
      foreach (string field in fields)
        this.RequireField(field);
    }

    public void Add(string field, params IValidator[] validators)
    {
      if (string.IsNullOrEmpty(field))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (field));
      if (validators == null || ((IEnumerable<IValidator>) validators).Any<IValidator>((Func<IValidator, bool>) (v => v == null)))
        throw new ArgumentNullException(nameof (validators));
      this.AddFieldValidators(field, validators);
    }

    public void Add(IEnumerable<string> fields, params IValidator[] validators)
    {
      if (fields == null)
        throw new ArgumentNullException(nameof (fields));
      if (validators == null)
        throw new ArgumentNullException(nameof (validators));
      foreach (string field in fields)
        this.Add(field, validators);
    }

    public void AddFormError(string errorMessage)
    {
      this._modelStateDictionary.AddFormError(errorMessage);
    }

    public bool IsValid(params string[] fields) => !this.Validate(fields).Any<ValidationResult>();

    public IEnumerable<ValidationResult> Validate(params string[] fields)
    {
      IEnumerable<string> fields1 = (IEnumerable<string>) fields;
      if (fields == null || !((IEnumerable<string>) fields).Any<string>())
        fields1 = this._validators.Keys.Concat<string>((IEnumerable<string>) new string[1]
        {
          this.FormField
        });
      return this.ValidateFieldsAndUpdateModelState(fields1);
    }

    public IEnumerable<string> GetErrors(params string[] fields)
    {
      return this.Validate(fields).Select<ValidationResult, string>((Func<ValidationResult, string>) (r => r.ErrorMessage));
    }

    public HtmlString For(string field)
    {
      return !string.IsNullOrEmpty(field) ? ValidationHelper.GenerateHtmlFromClientValidationRules(this.GetClientValidationRules(field)) : throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (field));
    }

    public HtmlString ClassFor(string field)
    {
      if (this._httpContext == null || !string.Equals("POST", this._httpContext.Request.HttpMethod, StringComparison.OrdinalIgnoreCase))
        return (HtmlString) null;
      string str = this.IsValid(field) ? ValidationHelper.ValidCssClass : ValidationHelper.InvalidCssClass;
      return str != null ? new HtmlString(str) : (HtmlString) null;
    }

    internal static IDisposable OverrideScope()
    {
      ValidationHelper._scopeOverride = (IDictionary<object, object>) new Dictionary<object, object>();
      return (IDisposable) new DisposableAction((Action) (() => ValidationHelper._scopeOverride = (IDictionary<object, object>) null));
    }

    internal IDictionary<string, object> GetUnobtrusiveValidationAttributes(string field)
    {
      IEnumerable<ModelClientValidationRule> clientValidationRules = this.GetClientValidationRules(field);
      Dictionary<string, object> results = new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      UnobtrusiveValidationAttributesGenerator.GetValidationAttributes(clientValidationRules, (IDictionary<string, object>) results);
      return (IDictionary<string, object>) results;
    }

    private IEnumerable<ValidationResult> ValidateFieldsAndUpdateModelState(
      IEnumerable<string> fields)
    {
      ValidationContext context = new ValidationContext((object) this._httpContext, (IServiceProvider) null, (IDictionary<object, object>) null);
      List<ValidationResult> validationResultList = new List<ValidationResult>();
      using (IEnumerator<string> enumerator = fields.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string field = enumerator.Current;
          IEnumerable<ValidationResult> validationResults = this.ValidateField(field, context);
          IEnumerable<string> first = validationResults.Select<ValidationResult, string>((Func<ValidationResult, string>) (c => c.ErrorMessage));
          ModelState modelState = this._modelStateDictionary[field];
          if (modelState != null && modelState.Errors.Any<string>())
          {
            first = first.Except<string>((IEnumerable<string>) modelState.Errors, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
            validationResults = validationResults.Concat<ValidationResult>(modelState.Errors.Select<string, ValidationResult>((Func<string, ValidationResult>) (e => new ValidationResult(e, (IEnumerable<string>) new string[1]
            {
              field
            }))));
          }
          foreach (string errorMessage in first)
            this._modelStateDictionary.AddError(field, errorMessage);
          validationResultList.AddRange(validationResults);
        }
      }
      return (IEnumerable<ValidationResult>) validationResultList;
    }

    private void AddFieldValidators(string field, params IValidator[] validators)
    {
      List<IValidator> validatorList = (List<IValidator>) null;
      if (!this._validators.TryGetValue(field, out validatorList))
      {
        validatorList = new List<IValidator>();
        this._validators[field] = validatorList;
      }
      foreach (IValidator validator in validators)
        validatorList.Add(validator);
    }

    private IEnumerable<ValidationResult> ValidateField(string field, ValidationContext context)
    {
      List<IValidator> source;
      if (!this._validators.TryGetValue(field, out source))
        return Enumerable.Empty<ValidationResult>();
      context.MemberName = field;
      return source.Select<IValidator, ValidationResult>((Func<IValidator, ValidationResult>) (f => f.Validate(context))).Where<ValidationResult>((Func<ValidationResult, bool>) (result => result != ValidationResult.Success));
    }

    private IEnumerable<ModelClientValidationRule> GetClientValidationRules(string field)
    {
      List<IValidator> source = (List<IValidator>) null;
      return !this._validators.TryGetValue(field, out source) ? Enumerable.Empty<ModelClientValidationRule>() : source.Select(item => new
      {
        item = item,
        clientRule = item.ClientValidationRule
      }).Where(_param0 => _param0.clientRule != null).Select(_param0 => _param0.clientRule);
    }

    internal static HtmlString GenerateHtmlFromClientValidationRules(
      IEnumerable<ModelClientValidationRule> clientRules)
    {
      if (!clientRules.Any<ModelClientValidationRule>())
        return (HtmlString) null;
      Dictionary<string, object> results = new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      UnobtrusiveValidationAttributesGenerator.GetValidationAttributes(clientRules, (IDictionary<string, object>) results);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<string, object> keyValuePair in results)
      {
        string key = keyValuePair.Key;
        string str = HttpUtility.HtmlEncode(Convert.ToString(keyValuePair.Value, (IFormatProvider) CultureInfo.InvariantCulture));
        stringBuilder.Append(key).Append("=\"").Append(str).Append('"').Append(' ');
      }
      if (stringBuilder.Length > 0)
        --stringBuilder.Length;
      return new HtmlString(stringBuilder.ToString());
    }
  }
}
