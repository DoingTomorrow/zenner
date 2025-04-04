// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.ValidationExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc.Properties;
using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc.Html
{
  public static class ValidationExtensions
  {
    private const string HiddenListItem = "<li style=\"display:none\"></li>";
    private static string _resourceClassKey;

    public static string ResourceClassKey
    {
      get => ValidationExtensions._resourceClassKey ?? string.Empty;
      set => ValidationExtensions._resourceClassKey = value;
    }

    private static FieldValidationMetadata ApplyFieldValidationMetadata(
      HtmlHelper htmlHelper,
      ModelMetadata modelMetadata,
      string modelName)
    {
      FieldValidationMetadata metadataForField = htmlHelper.ViewContext.FormContext.GetValidationMetadataForField(modelName, true);
      foreach (ModelClientValidationRule clientValidationRule in ModelValidatorProviders.Providers.GetValidators(modelMetadata, (ControllerContext) htmlHelper.ViewContext).SelectMany<ModelValidator, ModelClientValidationRule>((Func<ModelValidator, IEnumerable<ModelClientValidationRule>>) (v => v.GetClientValidationRules())))
        metadataForField.ValidationRules.Add(clientValidationRule);
      return metadataForField;
    }

    private static string GetInvalidPropertyValueResource(HttpContextBase httpContext)
    {
      string str = (string) null;
      if (!string.IsNullOrEmpty(ValidationExtensions.ResourceClassKey) && httpContext != null)
        str = httpContext.GetGlobalResourceObject(ValidationExtensions.ResourceClassKey, "InvalidPropertyValue", CultureInfo.CurrentUICulture) as string;
      return str ?? MvcResources.Common_ValueNotValidForProperty;
    }

    private static string GetUserErrorMessageOrDefault(
      HttpContextBase httpContext,
      ModelError error,
      ModelState modelState)
    {
      if (!string.IsNullOrEmpty(error.ErrorMessage))
        return error.ErrorMessage;
      if (modelState == null)
        return (string) null;
      string attemptedValue = modelState.Value != null ? modelState.Value.AttemptedValue : (string) null;
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, ValidationExtensions.GetInvalidPropertyValueResource(httpContext), new object[1]
      {
        (object) attemptedValue
      });
    }

    public static void Validate(this HtmlHelper htmlHelper, string modelName)
    {
      if (modelName == null)
        throw new ArgumentNullException(nameof (modelName));
      ValidationExtensions.ValidateHelper(htmlHelper, ModelMetadata.FromStringExpression(modelName, htmlHelper.ViewContext.ViewData), modelName);
    }

    public static void ValidateFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression)
    {
      ValidationExtensions.ValidateHelper((HtmlHelper) htmlHelper, ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData), ExpressionHelper.GetExpressionText((LambdaExpression) expression));
    }

    private static void ValidateHelper(
      HtmlHelper htmlHelper,
      ModelMetadata modelMetadata,
      string expression)
    {
      if (htmlHelper.ViewContext.GetFormContextForClientValidation() == null || htmlHelper.ViewContext.UnobtrusiveJavaScriptEnabled)
        return;
      string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expression);
      ValidationExtensions.ApplyFieldValidationMetadata(htmlHelper, modelMetadata, fullHtmlFieldName);
    }

    public static MvcHtmlString ValidationMessage(this HtmlHelper htmlHelper, string modelName)
    {
      return ValidationExtensions.ValidationMessage(htmlHelper, modelName, (string) null, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcHtmlString ValidationMessage(
      this HtmlHelper htmlHelper,
      string modelName,
      object htmlAttributes)
    {
      return ValidationExtensions.ValidationMessage(htmlHelper, modelName, (string) null, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString ValidationMessage(
      this HtmlHelper htmlHelper,
      string modelName,
      string validationMessage)
    {
      return ValidationExtensions.ValidationMessage(htmlHelper, modelName, validationMessage, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcHtmlString ValidationMessage(
      this HtmlHelper htmlHelper,
      string modelName,
      string validationMessage,
      object htmlAttributes)
    {
      return ValidationExtensions.ValidationMessage(htmlHelper, modelName, validationMessage, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString ValidationMessage(
      this HtmlHelper htmlHelper,
      string modelName,
      IDictionary<string, object> htmlAttributes)
    {
      return ValidationExtensions.ValidationMessage(htmlHelper, modelName, (string) null, htmlAttributes);
    }

    public static MvcHtmlString ValidationMessage(
      this HtmlHelper htmlHelper,
      string modelName,
      string validationMessage,
      IDictionary<string, object> htmlAttributes)
    {
      if (modelName == null)
        throw new ArgumentNullException(nameof (modelName));
      return htmlHelper.ValidationMessageHelper(ModelMetadata.FromStringExpression(modelName, htmlHelper.ViewContext.ViewData), modelName, validationMessage, htmlAttributes);
    }

    public static MvcHtmlString ValidationMessageFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression)
    {
      return ValidationExtensions.ValidationMessageFor<TModel, TProperty>(htmlHelper, expression, (string) null, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcHtmlString ValidationMessageFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      string validationMessage)
    {
      return ValidationExtensions.ValidationMessageFor<TModel, TProperty>(htmlHelper, expression, validationMessage, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcHtmlString ValidationMessageFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      string validationMessage,
      object htmlAttributes)
    {
      return ValidationExtensions.ValidationMessageFor<TModel, TProperty>(htmlHelper, expression, validationMessage, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString ValidationMessageFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      string validationMessage,
      IDictionary<string, object> htmlAttributes)
    {
      return htmlHelper.ValidationMessageHelper(ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData), ExpressionHelper.GetExpressionText((LambdaExpression) expression), validationMessage, htmlAttributes);
    }

    private static MvcHtmlString ValidationMessageHelper(
      this HtmlHelper htmlHelper,
      ModelMetadata modelMetadata,
      string expression,
      string validationMessage,
      IDictionary<string, object> htmlAttributes)
    {
      string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expression);
      FormContext clientValidation = htmlHelper.ViewContext.GetFormContextForClientValidation();
      if (!htmlHelper.ViewData.ModelState.ContainsKey(fullHtmlFieldName) && clientValidation == null)
        return (MvcHtmlString) null;
      ModelState modelState = htmlHelper.ViewData.ModelState[fullHtmlFieldName];
      ModelErrorCollection errors = modelState == null ? (ModelErrorCollection) null : modelState.Errors;
      ModelError error = errors == null || errors.Count == 0 ? (ModelError) null : errors.FirstOrDefault<ModelError>((Func<ModelError, bool>) (m => !string.IsNullOrEmpty(m.ErrorMessage))) ?? errors[0];
      if (error == null && clientValidation == null)
        return (MvcHtmlString) null;
      TagBuilder tagBuilder = new TagBuilder("span");
      tagBuilder.MergeAttributes<string, object>(htmlAttributes);
      tagBuilder.AddCssClass(error != null ? HtmlHelper.ValidationMessageCssClassName : HtmlHelper.ValidationMessageValidCssClassName);
      if (!string.IsNullOrEmpty(validationMessage))
        tagBuilder.SetInnerText(validationMessage);
      else if (error != null)
        tagBuilder.SetInnerText(ValidationExtensions.GetUserErrorMessageOrDefault(htmlHelper.ViewContext.HttpContext, error, modelState));
      if (clientValidation != null)
      {
        bool flag = string.IsNullOrEmpty(validationMessage);
        if (htmlHelper.ViewContext.UnobtrusiveJavaScriptEnabled)
        {
          tagBuilder.MergeAttribute("data-valmsg-for", fullHtmlFieldName);
          tagBuilder.MergeAttribute("data-valmsg-replace", flag.ToString().ToLowerInvariant());
        }
        else
        {
          FieldValidationMetadata validationMetadata = ValidationExtensions.ApplyFieldValidationMetadata(htmlHelper, modelMetadata, fullHtmlFieldName);
          validationMetadata.ReplaceValidationMessageContents = flag;
          tagBuilder.GenerateId(fullHtmlFieldName + "_validationMessage");
          validationMetadata.ValidationMessageId = tagBuilder.Attributes["id"];
        }
      }
      return tagBuilder.ToMvcHtmlString(TagRenderMode.Normal);
    }

    public static MvcHtmlString ValidationSummary(this HtmlHelper htmlHelper)
    {
      return htmlHelper.ValidationSummary(false);
    }

    public static MvcHtmlString ValidationSummary(
      this HtmlHelper htmlHelper,
      bool excludePropertyErrors)
    {
      return htmlHelper.ValidationSummary(excludePropertyErrors, (string) null);
    }

    public static MvcHtmlString ValidationSummary(this HtmlHelper htmlHelper, string message)
    {
      return htmlHelper.ValidationSummary(false, message, (object) null);
    }

    public static MvcHtmlString ValidationSummary(
      this HtmlHelper htmlHelper,
      bool excludePropertyErrors,
      string message)
    {
      return htmlHelper.ValidationSummary(excludePropertyErrors, message, (object) null);
    }

    public static MvcHtmlString ValidationSummary(
      this HtmlHelper htmlHelper,
      string message,
      object htmlAttributes)
    {
      return ValidationExtensions.ValidationSummary(htmlHelper, false, message, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString ValidationSummary(
      this HtmlHelper htmlHelper,
      bool excludePropertyErrors,
      string message,
      object htmlAttributes)
    {
      return ValidationExtensions.ValidationSummary(htmlHelper, excludePropertyErrors, message, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString ValidationSummary(
      this HtmlHelper htmlHelper,
      string message,
      IDictionary<string, object> htmlAttributes)
    {
      return ValidationExtensions.ValidationSummary(htmlHelper, false, message, htmlAttributes);
    }

    public static MvcHtmlString ValidationSummary(
      this HtmlHelper htmlHelper,
      bool excludePropertyErrors,
      string message,
      IDictionary<string, object> htmlAttributes)
    {
      FormContext formContext = htmlHelper != null ? htmlHelper.ViewContext.GetFormContextForClientValidation() : throw new ArgumentNullException(nameof (htmlHelper));
      if (htmlHelper.ViewData.ModelState.IsValid)
      {
        if (formContext == null)
          return (MvcHtmlString) null;
        if (htmlHelper.ViewContext.UnobtrusiveJavaScriptEnabled && excludePropertyErrors)
          return (MvcHtmlString) null;
      }
      string str;
      if (!string.IsNullOrEmpty(message))
      {
        TagBuilder tagBuilder = new TagBuilder("span");
        tagBuilder.SetInnerText(message);
        str = tagBuilder.ToString(TagRenderMode.Normal) + Environment.NewLine;
      }
      else
        str = (string) null;
      StringBuilder stringBuilder = new StringBuilder();
      TagBuilder tagBuilder1 = new TagBuilder("ul");
      foreach (ModelState modelState in ValidationExtensions.GetModelStateList(htmlHelper, excludePropertyErrors))
      {
        foreach (ModelError error in (Collection<ModelError>) modelState.Errors)
        {
          string messageOrDefault = ValidationExtensions.GetUserErrorMessageOrDefault(htmlHelper.ViewContext.HttpContext, error, (ModelState) null);
          if (!string.IsNullOrEmpty(messageOrDefault))
          {
            TagBuilder tagBuilder2 = new TagBuilder("li");
            tagBuilder2.SetInnerText(messageOrDefault);
            stringBuilder.AppendLine(tagBuilder2.ToString(TagRenderMode.Normal));
          }
        }
      }
      if (stringBuilder.Length == 0)
        stringBuilder.AppendLine("<li style=\"display:none\"></li>");
      tagBuilder1.InnerHtml = stringBuilder.ToString();
      TagBuilder tagBuilder3 = new TagBuilder("div");
      tagBuilder3.MergeAttributes<string, object>(htmlAttributes);
      tagBuilder3.AddCssClass(htmlHelper.ViewData.ModelState.IsValid ? HtmlHelper.ValidationSummaryValidCssClassName : HtmlHelper.ValidationSummaryCssClassName);
      tagBuilder3.InnerHtml = str + tagBuilder1.ToString(TagRenderMode.Normal);
      if (formContext != null)
      {
        if (htmlHelper.ViewContext.UnobtrusiveJavaScriptEnabled)
        {
          if (!excludePropertyErrors)
            tagBuilder3.MergeAttribute("data-valmsg-summary", "true");
        }
        else
        {
          tagBuilder3.GenerateId("validationSummary");
          formContext.ValidationSummaryId = tagBuilder3.Attributes["id"];
          formContext.ReplaceValidationSummary = !excludePropertyErrors;
        }
      }
      return tagBuilder3.ToMvcHtmlString(TagRenderMode.Normal);
    }

    private static IEnumerable<ModelState> GetModelStateList(
      HtmlHelper htmlHelper,
      bool excludePropertyErrors)
    {
      if (excludePropertyErrors)
      {
        ModelState modelState;
        htmlHelper.ViewData.ModelState.TryGetValue(htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix, out modelState);
        if (modelState == null)
          return (IEnumerable<ModelState>) new ModelState[0];
        return (IEnumerable<ModelState>) new ModelState[1]
        {
          modelState
        };
      }
      Dictionary<string, int> ordering = new Dictionary<string, int>();
      ModelMetadata modelMetadata = htmlHelper.ViewData.ModelMetadata;
      if (modelMetadata != null)
      {
        foreach (ModelMetadata property in modelMetadata.Properties)
          ordering[property.PropertyName] = property.Order;
      }
      return htmlHelper.ViewData.ModelState.Select(kv => new
      {
        kv = kv,
        name = kv.Key
      }).OrderBy(_param1 => ordering.GetOrDefault<string, int>(_param1.name, 10000)).Select(_param0 => _param0.kv.Value);
    }
  }
}
