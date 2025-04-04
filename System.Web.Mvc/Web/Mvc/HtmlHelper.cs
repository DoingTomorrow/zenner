// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.HtmlHelper
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc.Properties;
using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc
{
  public class HtmlHelper
  {
    public static readonly string ValidationInputCssClassName = "input-validation-error";
    public static readonly string ValidationInputValidCssClassName = "input-validation-valid";
    public static readonly string ValidationMessageCssClassName = "field-validation-error";
    public static readonly string ValidationMessageValidCssClassName = "field-validation-valid";
    public static readonly string ValidationSummaryCssClassName = "validation-summary-errors";
    public static readonly string ValidationSummaryValidCssClassName = "validation-summary-valid";
    private DynamicViewDataDictionary _dynamicViewDataDictionary;

    public HtmlHelper(ViewContext viewContext, IViewDataContainer viewDataContainer)
      : this(viewContext, viewDataContainer, RouteTable.Routes)
    {
    }

    public HtmlHelper(
      ViewContext viewContext,
      IViewDataContainer viewDataContainer,
      RouteCollection routeCollection)
    {
      if (viewContext == null)
        throw new ArgumentNullException(nameof (viewContext));
      if (viewDataContainer == null)
        throw new ArgumentNullException(nameof (viewDataContainer));
      if (routeCollection == null)
        throw new ArgumentNullException(nameof (routeCollection));
      this.ViewContext = viewContext;
      this.ViewDataContainer = viewDataContainer;
      this.RouteCollection = routeCollection;
      this.ClientValidationRuleFactory = (Func<string, ModelMetadata, IEnumerable<ModelClientValidationRule>>) ((name, metadata) => ModelValidatorProviders.Providers.GetValidators(metadata ?? ModelMetadata.FromStringExpression(name, this.ViewData), (ControllerContext) this.ViewContext).SelectMany<ModelValidator, ModelClientValidationRule>((Func<ModelValidator, IEnumerable<ModelClientValidationRule>>) (v => v.GetClientValidationRules())));
    }

    public static bool ClientValidationEnabled
    {
      get => ViewContext.GetClientValidationEnabled();
      set => ViewContext.SetClientValidationEnabled(value);
    }

    public static string IdAttributeDotReplacement
    {
      get => System.Web.WebPages.Html.HtmlHelper.IdAttributeDotReplacement;
      set => System.Web.WebPages.Html.HtmlHelper.IdAttributeDotReplacement = value;
    }

    internal Func<string, ModelMetadata, IEnumerable<ModelClientValidationRule>> ClientValidationRuleFactory { get; set; }

    public RouteCollection RouteCollection { get; private set; }

    public static bool UnobtrusiveJavaScriptEnabled
    {
      get => ViewContext.GetUnobtrusiveJavaScriptEnabled();
      set => ViewContext.SetUnobtrusiveJavaScriptEnabled(value);
    }

    public object ViewBag
    {
      get
      {
        if (this._dynamicViewDataDictionary == null)
          this._dynamicViewDataDictionary = new DynamicViewDataDictionary((Func<ViewDataDictionary>) (() => this.ViewData));
        return (object) this._dynamicViewDataDictionary;
      }
    }

    public ViewContext ViewContext { get; private set; }

    public ViewDataDictionary ViewData => this.ViewDataContainer.ViewData;

    public IViewDataContainer ViewDataContainer { get; internal set; }

    public static RouteValueDictionary AnonymousObjectToHtmlAttributes(object htmlAttributes)
    {
      RouteValueDictionary htmlAttributes1 = new RouteValueDictionary();
      if (htmlAttributes != null)
      {
        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
          htmlAttributes1.Add(property.Name.Replace('_', '-'), property.GetValue(htmlAttributes));
      }
      return htmlAttributes1;
    }

    public MvcHtmlString AntiForgeryToken() => new MvcHtmlString(AntiForgery.GetHtml().ToString());

    [Obsolete("This method is deprecated. Use the AntiForgeryToken() method instead. To specify custom data to be embedded within the token, use the static AntiForgeryConfig.AdditionalDataProvider property.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public MvcHtmlString AntiForgeryToken(string salt)
    {
      if (!string.IsNullOrEmpty(salt))
        throw new NotSupportedException("This method is deprecated. Use the AntiForgeryToken() method instead. To specify custom data to be embedded within the token, use the static AntiForgeryConfig.AdditionalDataProvider property.");
      return this.AntiForgeryToken();
    }

    [Obsolete("This method is deprecated. Use the AntiForgeryToken() method instead. To specify a custom domain for the generated cookie, use the <httpCookies> configuration element. To specify custom data to be embedded within the token, use the static AntiForgeryConfig.AdditionalDataProvider property.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public MvcHtmlString AntiForgeryToken(string salt, string domain, string path)
    {
      if (!string.IsNullOrEmpty(salt) || !string.IsNullOrEmpty(domain) || !string.IsNullOrEmpty(path))
        throw new NotSupportedException("This method is deprecated. Use the AntiForgeryToken() method instead. To specify a custom domain for the generated cookie, use the <httpCookies> configuration element. To specify custom data to be embedded within the token, use the static AntiForgeryConfig.AdditionalDataProvider property.");
      return this.AntiForgeryToken();
    }

    public string AttributeEncode(string value)
    {
      return string.IsNullOrEmpty(value) ? string.Empty : HttpUtility.HtmlAttributeEncode(value);
    }

    public string AttributeEncode(object value)
    {
      return this.AttributeEncode(Convert.ToString(value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public void EnableClientValidation() => this.EnableClientValidation(true);

    public void EnableClientValidation(bool enabled)
    {
      this.ViewContext.ClientValidationEnabled = enabled;
    }

    public void EnableUnobtrusiveJavaScript() => this.EnableUnobtrusiveJavaScript(true);

    public void EnableUnobtrusiveJavaScript(bool enabled)
    {
      this.ViewContext.UnobtrusiveJavaScriptEnabled = enabled;
    }

    public string Encode(string value)
    {
      return string.IsNullOrEmpty(value) ? string.Empty : HttpUtility.HtmlEncode(value);
    }

    public string Encode(object value)
    {
      return value == null ? string.Empty : HttpUtility.HtmlEncode(value);
    }

    internal string EvalString(string key)
    {
      return Convert.ToString(this.ViewData.Eval(key), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    internal string EvalString(string key, string format)
    {
      return Convert.ToString(this.ViewData.Eval(key, format), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    public string FormatValue(object value, string format)
    {
      return ViewDataDictionary.FormatValueInternal(value, format);
    }

    internal bool EvalBoolean(string key)
    {
      return Convert.ToBoolean(this.ViewData.Eval(key), (IFormatProvider) CultureInfo.InvariantCulture);
    }

    internal static IView FindPartialView(
      ViewContext viewContext,
      string partialViewName,
      ViewEngineCollection viewEngineCollection)
    {
      ViewEngineResult partialView = viewEngineCollection.FindPartialView((ControllerContext) viewContext, partialViewName);
      if (partialView.View != null)
        return partialView.View;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string searchedLocation in partialView.SearchedLocations)
      {
        stringBuilder.AppendLine();
        stringBuilder.Append(searchedLocation);
      }
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.Common_PartialViewNotFound, new object[2]
      {
        (object) partialViewName,
        (object) stringBuilder
      }));
    }

    public static string GenerateIdFromName(string name)
    {
      return HtmlHelper.GenerateIdFromName(name, HtmlHelper.IdAttributeDotReplacement);
    }

    public static string GenerateIdFromName(string name, string idAttributeDotReplacement)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (idAttributeDotReplacement == null)
        throw new ArgumentNullException(nameof (idAttributeDotReplacement));
      return name.Length == 0 ? string.Empty : TagBuilder.CreateSanitizedId(name, idAttributeDotReplacement);
    }

    public static string GenerateLink(
      RequestContext requestContext,
      RouteCollection routeCollection,
      string linkText,
      string routeName,
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues,
      IDictionary<string, object> htmlAttributes)
    {
      return HtmlHelper.GenerateLink(requestContext, routeCollection, linkText, routeName, actionName, controllerName, (string) null, (string) null, (string) null, routeValues, htmlAttributes);
    }

    public static string GenerateLink(
      RequestContext requestContext,
      RouteCollection routeCollection,
      string linkText,
      string routeName,
      string actionName,
      string controllerName,
      string protocol,
      string hostName,
      string fragment,
      RouteValueDictionary routeValues,
      IDictionary<string, object> htmlAttributes)
    {
      return HtmlHelper.GenerateLinkInternal(requestContext, routeCollection, linkText, routeName, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes, true);
    }

    private static string GenerateLinkInternal(
      RequestContext requestContext,
      RouteCollection routeCollection,
      string linkText,
      string routeName,
      string actionName,
      string controllerName,
      string protocol,
      string hostName,
      string fragment,
      RouteValueDictionary routeValues,
      IDictionary<string, object> htmlAttributes,
      bool includeImplicitMvcValues)
    {
      string url = UrlHelper.GenerateUrl(routeName, actionName, controllerName, protocol, hostName, fragment, routeValues, routeCollection, requestContext, includeImplicitMvcValues);
      TagBuilder tagBuilder = new TagBuilder("a")
      {
        InnerHtml = !string.IsNullOrEmpty(linkText) ? HttpUtility.HtmlEncode(linkText) : string.Empty
      };
      tagBuilder.MergeAttributes<string, object>(htmlAttributes);
      tagBuilder.MergeAttribute("href", url);
      return tagBuilder.ToString(TagRenderMode.Normal);
    }

    public static string GenerateRouteLink(
      RequestContext requestContext,
      RouteCollection routeCollection,
      string linkText,
      string routeName,
      RouteValueDictionary routeValues,
      IDictionary<string, object> htmlAttributes)
    {
      return HtmlHelper.GenerateRouteLink(requestContext, routeCollection, linkText, routeName, (string) null, (string) null, (string) null, routeValues, htmlAttributes);
    }

    public static string GenerateRouteLink(
      RequestContext requestContext,
      RouteCollection routeCollection,
      string linkText,
      string routeName,
      string protocol,
      string hostName,
      string fragment,
      RouteValueDictionary routeValues,
      IDictionary<string, object> htmlAttributes)
    {
      return HtmlHelper.GenerateLinkInternal(requestContext, routeCollection, linkText, routeName, (string) null, (string) null, protocol, hostName, fragment, routeValues, htmlAttributes, false);
    }

    public static string GetFormMethodString(FormMethod method)
    {
      switch (method)
      {
        case FormMethod.Get:
          return "get";
        case FormMethod.Post:
          return "post";
        default:
          return "post";
      }
    }

    public static string GetInputTypeString(InputType inputType)
    {
      switch (inputType)
      {
        case InputType.CheckBox:
          return "checkbox";
        case InputType.Hidden:
          return "hidden";
        case InputType.Password:
          return "password";
        case InputType.Radio:
          return "radio";
        case InputType.Text:
          return "text";
        default:
          return "text";
      }
    }

    internal object GetModelStateValue(string key, Type destinationType)
    {
      ModelState modelState;
      return this.ViewData.ModelState.TryGetValue(key, out modelState) && modelState.Value != null ? modelState.Value.ConvertTo(destinationType, (CultureInfo) null) : (object) null;
    }

    public IDictionary<string, object> GetUnobtrusiveValidationAttributes(string name)
    {
      return this.GetUnobtrusiveValidationAttributes(name, (ModelMetadata) null);
    }

    public IDictionary<string, object> GetUnobtrusiveValidationAttributes(
      string name,
      ModelMetadata metadata)
    {
      Dictionary<string, object> results = new Dictionary<string, object>();
      if (!this.ViewContext.UnobtrusiveJavaScriptEnabled)
        return (IDictionary<string, object>) results;
      FormContext clientValidation = this.ViewContext.GetFormContextForClientValidation();
      if (clientValidation == null)
        return (IDictionary<string, object>) results;
      string fullHtmlFieldName = this.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
      if (clientValidation.RenderedField(fullHtmlFieldName))
        return (IDictionary<string, object>) results;
      clientValidation.RenderedField(fullHtmlFieldName, true);
      UnobtrusiveValidationAttributesGenerator.GetValidationAttributes(this.ClientValidationRuleFactory(name, metadata), (IDictionary<string, object>) results);
      return (IDictionary<string, object>) results;
    }

    public MvcHtmlString HttpMethodOverride(HttpVerbs httpVerb)
    {
      string httpMethod;
      switch (httpVerb)
      {
        case HttpVerbs.Put:
          httpMethod = "PUT";
          break;
        case HttpVerbs.Delete:
          httpMethod = "DELETE";
          break;
        case HttpVerbs.Head:
          httpMethod = "HEAD";
          break;
        case HttpVerbs.Patch:
          httpMethod = "PATCH";
          break;
        case HttpVerbs.Options:
          httpMethod = "OPTIONS";
          break;
        default:
          throw new ArgumentException(MvcResources.HtmlHelper_InvalidHttpVerb, nameof (httpVerb));
      }
      return this.HttpMethodOverride(httpMethod);
    }

    public MvcHtmlString HttpMethodOverride(string httpMethod)
    {
      if (string.IsNullOrEmpty(httpMethod))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (httpMethod));
      return !string.Equals(httpMethod, "GET", StringComparison.OrdinalIgnoreCase) && !string.Equals(httpMethod, "POST", StringComparison.OrdinalIgnoreCase) ? new TagBuilder("input")
      {
        Attributes = {
          ["type"] = "hidden",
          ["name"] = "X-HTTP-Method-Override",
          ["value"] = httpMethod
        }
      }.ToMvcHtmlString(TagRenderMode.SelfClosing) : throw new ArgumentException(MvcResources.HtmlHelper_InvalidHttpMethod, nameof (httpMethod));
    }

    public IHtmlString Raw(string value) => (IHtmlString) new HtmlString(value);

    public IHtmlString Raw(object value)
    {
      return (IHtmlString) new HtmlString(value == null ? (string) null : value.ToString());
    }

    internal virtual void RenderPartialInternal(
      string partialViewName,
      ViewDataDictionary viewData,
      object model,
      TextWriter writer,
      ViewEngineCollection viewEngineCollection)
    {
      if (string.IsNullOrEmpty(partialViewName))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (partialViewName));
      ViewDataDictionary viewData1;
      if (model == null)
        viewData1 = viewData != null ? new ViewDataDictionary(viewData) : new ViewDataDictionary(this.ViewData);
      else if (viewData == null)
        viewData1 = new ViewDataDictionary(model);
      else
        viewData1 = new ViewDataDictionary(viewData)
        {
          Model = model
        };
      ViewContext viewContext = new ViewContext((ControllerContext) this.ViewContext, this.ViewContext.View, viewData1, this.ViewContext.TempData, writer);
      HtmlHelper.FindPartialView(viewContext, partialViewName, viewEngineCollection).Render(viewContext, writer);
    }
  }
}
