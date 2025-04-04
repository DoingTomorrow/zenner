// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Ajax.AjaxExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc.Html;
using System.Web.Mvc.Properties;
using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc.Ajax
{
  public static class AjaxExtensions
  {
    private const string LinkOnClickFormat = "Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), {0});";
    private const string FormOnClickValue = "Sys.Mvc.AsyncForm.handleClick(this, new Sys.UI.DomEvent(event));";
    private const string FormOnSubmitFormat = "Sys.Mvc.AsyncForm.handleSubmit(this, new Sys.UI.DomEvent(event), {0});";

    public static MvcHtmlString ActionLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string actionName,
      AjaxOptions ajaxOptions)
    {
      return AjaxExtensions.ActionLink(ajaxHelper, linkText, actionName, (string) null, ajaxOptions);
    }

    public static MvcHtmlString ActionLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string actionName,
      object routeValues,
      AjaxOptions ajaxOptions)
    {
      return ajaxHelper.ActionLink(linkText, actionName, (string) null, routeValues, ajaxOptions);
    }

    public static MvcHtmlString ActionLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string actionName,
      object routeValues,
      AjaxOptions ajaxOptions,
      object htmlAttributes)
    {
      return ajaxHelper.ActionLink(linkText, actionName, (string) null, routeValues, ajaxOptions, htmlAttributes);
    }

    public static MvcHtmlString ActionLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string actionName,
      RouteValueDictionary routeValues,
      AjaxOptions ajaxOptions)
    {
      return AjaxExtensions.ActionLink(ajaxHelper, linkText, actionName, (string) null, routeValues, ajaxOptions);
    }

    public static MvcHtmlString ActionLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string actionName,
      RouteValueDictionary routeValues,
      AjaxOptions ajaxOptions,
      IDictionary<string, object> htmlAttributes)
    {
      return AjaxExtensions.ActionLink(ajaxHelper, linkText, actionName, (string) null, routeValues, ajaxOptions, htmlAttributes);
    }

    public static MvcHtmlString ActionLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string actionName,
      string controllerName,
      AjaxOptions ajaxOptions)
    {
      return AjaxExtensions.ActionLink(ajaxHelper, linkText, actionName, controllerName, (RouteValueDictionary) null, ajaxOptions, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString ActionLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string actionName,
      string controllerName,
      object routeValues,
      AjaxOptions ajaxOptions)
    {
      return ajaxHelper.ActionLink(linkText, actionName, controllerName, routeValues, ajaxOptions, (object) null);
    }

    public static MvcHtmlString ActionLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string actionName,
      string controllerName,
      object routeValues,
      AjaxOptions ajaxOptions,
      object htmlAttributes)
    {
      RouteValueDictionary routeValues1 = new RouteValueDictionary(routeValues);
      RouteValueDictionary htmlAttributes1 = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
      return AjaxExtensions.ActionLink(ajaxHelper, linkText, actionName, controllerName, routeValues1, ajaxOptions, (IDictionary<string, object>) htmlAttributes1);
    }

    public static MvcHtmlString ActionLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues,
      AjaxOptions ajaxOptions)
    {
      return AjaxExtensions.ActionLink(ajaxHelper, linkText, actionName, controllerName, routeValues, ajaxOptions, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString ActionLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues,
      AjaxOptions ajaxOptions,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(linkText))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (linkText));
      string url = UrlHelper.GenerateUrl((string) null, actionName, controllerName, routeValues, ajaxHelper.RouteCollection, ajaxHelper.ViewContext.RequestContext, true);
      return MvcHtmlString.Create(AjaxExtensions.GenerateLink(ajaxHelper, linkText, url, AjaxExtensions.GetAjaxOptions(ajaxOptions), htmlAttributes));
    }

    public static MvcHtmlString ActionLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string actionName,
      string controllerName,
      string protocol,
      string hostName,
      string fragment,
      object routeValues,
      AjaxOptions ajaxOptions,
      object htmlAttributes)
    {
      RouteValueDictionary routeValues1 = new RouteValueDictionary(routeValues);
      RouteValueDictionary htmlAttributes1 = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
      return AjaxExtensions.ActionLink(ajaxHelper, linkText, actionName, controllerName, protocol, hostName, fragment, routeValues1, ajaxOptions, (IDictionary<string, object>) htmlAttributes1);
    }

    public static MvcHtmlString ActionLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string actionName,
      string controllerName,
      string protocol,
      string hostName,
      string fragment,
      RouteValueDictionary routeValues,
      AjaxOptions ajaxOptions,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(linkText))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (linkText));
      string url = UrlHelper.GenerateUrl((string) null, actionName, controllerName, protocol, hostName, fragment, routeValues, ajaxHelper.RouteCollection, ajaxHelper.ViewContext.RequestContext, true);
      return MvcHtmlString.Create(AjaxExtensions.GenerateLink(ajaxHelper, linkText, url, ajaxOptions, htmlAttributes));
    }

    public static MvcForm BeginForm(this AjaxHelper ajaxHelper, AjaxOptions ajaxOptions)
    {
      string rawUrl = ajaxHelper.ViewContext.HttpContext.Request.RawUrl;
      return ajaxHelper.FormHelper(rawUrl, ajaxOptions, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginForm(
      this AjaxHelper ajaxHelper,
      string actionName,
      AjaxOptions ajaxOptions)
    {
      return AjaxExtensions.BeginForm(ajaxHelper, actionName, (string) null, ajaxOptions);
    }

    public static MvcForm BeginForm(
      this AjaxHelper ajaxHelper,
      string actionName,
      object routeValues,
      AjaxOptions ajaxOptions)
    {
      return ajaxHelper.BeginForm(actionName, (string) null, routeValues, ajaxOptions);
    }

    public static MvcForm BeginForm(
      this AjaxHelper ajaxHelper,
      string actionName,
      object routeValues,
      AjaxOptions ajaxOptions,
      object htmlAttributes)
    {
      return ajaxHelper.BeginForm(actionName, (string) null, routeValues, ajaxOptions, htmlAttributes);
    }

    public static MvcForm BeginForm(
      this AjaxHelper ajaxHelper,
      string actionName,
      RouteValueDictionary routeValues,
      AjaxOptions ajaxOptions)
    {
      return AjaxExtensions.BeginForm(ajaxHelper, actionName, (string) null, routeValues, ajaxOptions);
    }

    public static MvcForm BeginForm(
      this AjaxHelper ajaxHelper,
      string actionName,
      RouteValueDictionary routeValues,
      AjaxOptions ajaxOptions,
      IDictionary<string, object> htmlAttributes)
    {
      return AjaxExtensions.BeginForm(ajaxHelper, actionName, (string) null, routeValues, ajaxOptions, htmlAttributes);
    }

    public static MvcForm BeginForm(
      this AjaxHelper ajaxHelper,
      string actionName,
      string controllerName,
      AjaxOptions ajaxOptions)
    {
      return AjaxExtensions.BeginForm(ajaxHelper, actionName, controllerName, (RouteValueDictionary) null, ajaxOptions, (IDictionary<string, object>) null);
    }

    public static MvcForm BeginForm(
      this AjaxHelper ajaxHelper,
      string actionName,
      string controllerName,
      object routeValues,
      AjaxOptions ajaxOptions)
    {
      return ajaxHelper.BeginForm(actionName, controllerName, routeValues, ajaxOptions, (object) null);
    }

    public static MvcForm BeginForm(
      this AjaxHelper ajaxHelper,
      string actionName,
      string controllerName,
      object routeValues,
      AjaxOptions ajaxOptions,
      object htmlAttributes)
    {
      RouteValueDictionary routeValues1 = new RouteValueDictionary(routeValues);
      RouteValueDictionary htmlAttributes1 = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
      return AjaxExtensions.BeginForm(ajaxHelper, actionName, controllerName, routeValues1, ajaxOptions, (IDictionary<string, object>) htmlAttributes1);
    }

    public static MvcForm BeginForm(
      this AjaxHelper ajaxHelper,
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues,
      AjaxOptions ajaxOptions)
    {
      return AjaxExtensions.BeginForm(ajaxHelper, actionName, controllerName, routeValues, ajaxOptions, (IDictionary<string, object>) null);
    }

    public static MvcForm BeginForm(
      this AjaxHelper ajaxHelper,
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues,
      AjaxOptions ajaxOptions,
      IDictionary<string, object> htmlAttributes)
    {
      string url = UrlHelper.GenerateUrl((string) null, actionName, controllerName, routeValues ?? new RouteValueDictionary(), ajaxHelper.RouteCollection, ajaxHelper.ViewContext.RequestContext, true);
      return ajaxHelper.FormHelper(url, ajaxOptions, htmlAttributes);
    }

    public static MvcForm BeginRouteForm(
      this AjaxHelper ajaxHelper,
      string routeName,
      AjaxOptions ajaxOptions)
    {
      return AjaxExtensions.BeginRouteForm(ajaxHelper, routeName, (RouteValueDictionary) null, ajaxOptions, (IDictionary<string, object>) null);
    }

    public static MvcForm BeginRouteForm(
      this AjaxHelper ajaxHelper,
      string routeName,
      object routeValues,
      AjaxOptions ajaxOptions)
    {
      return ajaxHelper.BeginRouteForm(routeName, routeValues, ajaxOptions, (object) null);
    }

    public static MvcForm BeginRouteForm(
      this AjaxHelper ajaxHelper,
      string routeName,
      object routeValues,
      AjaxOptions ajaxOptions,
      object htmlAttributes)
    {
      RouteValueDictionary htmlAttributes1 = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
      return AjaxExtensions.BeginRouteForm(ajaxHelper, routeName, new RouteValueDictionary(routeValues), ajaxOptions, (IDictionary<string, object>) htmlAttributes1);
    }

    public static MvcForm BeginRouteForm(
      this AjaxHelper ajaxHelper,
      string routeName,
      RouteValueDictionary routeValues,
      AjaxOptions ajaxOptions)
    {
      return AjaxExtensions.BeginRouteForm(ajaxHelper, routeName, routeValues, ajaxOptions, (IDictionary<string, object>) null);
    }

    public static MvcForm BeginRouteForm(
      this AjaxHelper ajaxHelper,
      string routeName,
      RouteValueDictionary routeValues,
      AjaxOptions ajaxOptions,
      IDictionary<string, object> htmlAttributes)
    {
      string url = UrlHelper.GenerateUrl(routeName, (string) null, (string) null, routeValues ?? new RouteValueDictionary(), ajaxHelper.RouteCollection, ajaxHelper.ViewContext.RequestContext, false);
      return ajaxHelper.FormHelper(url, ajaxOptions, htmlAttributes);
    }

    private static MvcForm FormHelper(
      this AjaxHelper ajaxHelper,
      string formAction,
      AjaxOptions ajaxOptions,
      IDictionary<string, object> htmlAttributes)
    {
      TagBuilder tagBuilder = new TagBuilder("form");
      tagBuilder.MergeAttributes<string, object>(htmlAttributes);
      tagBuilder.MergeAttribute("action", formAction);
      tagBuilder.MergeAttribute("method", "post");
      ajaxOptions = AjaxExtensions.GetAjaxOptions(ajaxOptions);
      if (ajaxHelper.ViewContext.UnobtrusiveJavaScriptEnabled)
      {
        tagBuilder.MergeAttributes<string, object>(ajaxOptions.ToUnobtrusiveHtmlAttributes());
      }
      else
      {
        tagBuilder.MergeAttribute("onclick", "Sys.Mvc.AsyncForm.handleClick(this, new Sys.UI.DomEvent(event));");
        tagBuilder.MergeAttribute("onsubmit", AjaxExtensions.GenerateAjaxScript(ajaxOptions, "Sys.Mvc.AsyncForm.handleSubmit(this, new Sys.UI.DomEvent(event), {0});"));
      }
      if (ajaxHelper.ViewContext.ClientValidationEnabled)
        tagBuilder.GenerateId(ajaxHelper.ViewContext.FormIdGenerator());
      ajaxHelper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));
      MvcForm mvcForm = new MvcForm(ajaxHelper.ViewContext);
      if (ajaxHelper.ViewContext.ClientValidationEnabled)
        ajaxHelper.ViewContext.FormContext.FormId = tagBuilder.Attributes["id"];
      return mvcForm;
    }

    public static MvcHtmlString GlobalizationScript(this AjaxHelper ajaxHelper)
    {
      return ajaxHelper.GlobalizationScript(CultureInfo.CurrentCulture);
    }

    public static MvcHtmlString GlobalizationScript(
      this AjaxHelper ajaxHelper,
      CultureInfo cultureInfo)
    {
      return AjaxExtensions.GlobalizationScriptHelper(AjaxHelper.GlobalizationScriptPath, cultureInfo);
    }

    internal static MvcHtmlString GlobalizationScriptHelper(
      string scriptPath,
      CultureInfo cultureInfo)
    {
      if (cultureInfo == null)
        throw new ArgumentNullException(nameof (cultureInfo));
      TagBuilder tagBuilder = new TagBuilder("script");
      tagBuilder.MergeAttribute("type", "text/javascript");
      string str = VirtualPathUtility.AppendTrailingSlash(scriptPath) + HttpUtility.UrlEncode(cultureInfo.Name) + ".js";
      tagBuilder.MergeAttribute("src", str);
      return tagBuilder.ToMvcHtmlString(TagRenderMode.Normal);
    }

    public static MvcHtmlString RouteLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      object routeValues,
      AjaxOptions ajaxOptions)
    {
      return AjaxExtensions.RouteLink(ajaxHelper, linkText, (string) null, new RouteValueDictionary(routeValues), ajaxOptions, (IDictionary<string, object>) new Dictionary<string, object>());
    }

    public static MvcHtmlString RouteLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      object routeValues,
      AjaxOptions ajaxOptions,
      object htmlAttributes)
    {
      return AjaxExtensions.RouteLink(ajaxHelper, linkText, (string) null, new RouteValueDictionary(routeValues), ajaxOptions, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString RouteLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      RouteValueDictionary routeValues,
      AjaxOptions ajaxOptions)
    {
      return AjaxExtensions.RouteLink(ajaxHelper, linkText, (string) null, routeValues, ajaxOptions, (IDictionary<string, object>) new Dictionary<string, object>());
    }

    public static MvcHtmlString RouteLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      RouteValueDictionary routeValues,
      AjaxOptions ajaxOptions,
      IDictionary<string, object> htmlAttributes)
    {
      return AjaxExtensions.RouteLink(ajaxHelper, linkText, (string) null, routeValues, ajaxOptions, htmlAttributes);
    }

    public static MvcHtmlString RouteLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string routeName,
      AjaxOptions ajaxOptions)
    {
      return AjaxExtensions.RouteLink(ajaxHelper, linkText, routeName, new RouteValueDictionary(), ajaxOptions, (IDictionary<string, object>) new Dictionary<string, object>());
    }

    public static MvcHtmlString RouteLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string routeName,
      AjaxOptions ajaxOptions,
      object htmlAttributes)
    {
      return AjaxExtensions.RouteLink(ajaxHelper, linkText, routeName, new RouteValueDictionary(), ajaxOptions, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString RouteLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string routeName,
      AjaxOptions ajaxOptions,
      IDictionary<string, object> htmlAttributes)
    {
      return AjaxExtensions.RouteLink(ajaxHelper, linkText, routeName, new RouteValueDictionary(), ajaxOptions, htmlAttributes);
    }

    public static MvcHtmlString RouteLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string routeName,
      object routeValues,
      AjaxOptions ajaxOptions)
    {
      return AjaxExtensions.RouteLink(ajaxHelper, linkText, routeName, new RouteValueDictionary(routeValues), ajaxOptions, (IDictionary<string, object>) new Dictionary<string, object>());
    }

    public static MvcHtmlString RouteLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string routeName,
      object routeValues,
      AjaxOptions ajaxOptions,
      object htmlAttributes)
    {
      return AjaxExtensions.RouteLink(ajaxHelper, linkText, routeName, new RouteValueDictionary(routeValues), ajaxOptions, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString RouteLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string routeName,
      RouteValueDictionary routeValues,
      AjaxOptions ajaxOptions)
    {
      return AjaxExtensions.RouteLink(ajaxHelper, linkText, routeName, routeValues, ajaxOptions, (IDictionary<string, object>) new Dictionary<string, object>());
    }

    public static MvcHtmlString RouteLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string routeName,
      RouteValueDictionary routeValues,
      AjaxOptions ajaxOptions,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(linkText))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (linkText));
      string url = UrlHelper.GenerateUrl(routeName, (string) null, (string) null, routeValues ?? new RouteValueDictionary(), ajaxHelper.RouteCollection, ajaxHelper.ViewContext.RequestContext, false);
      return MvcHtmlString.Create(AjaxExtensions.GenerateLink(ajaxHelper, linkText, url, AjaxExtensions.GetAjaxOptions(ajaxOptions), htmlAttributes));
    }

    public static MvcHtmlString RouteLink(
      this AjaxHelper ajaxHelper,
      string linkText,
      string routeName,
      string protocol,
      string hostName,
      string fragment,
      RouteValueDictionary routeValues,
      AjaxOptions ajaxOptions,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(linkText))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (linkText));
      string url = UrlHelper.GenerateUrl(routeName, (string) null, (string) null, protocol, hostName, fragment, routeValues ?? new RouteValueDictionary(), ajaxHelper.RouteCollection, ajaxHelper.ViewContext.RequestContext, false);
      return MvcHtmlString.Create(AjaxExtensions.GenerateLink(ajaxHelper, linkText, url, AjaxExtensions.GetAjaxOptions(ajaxOptions), htmlAttributes));
    }

    private static string GenerateLink(
      AjaxHelper ajaxHelper,
      string linkText,
      string targetUrl,
      AjaxOptions ajaxOptions,
      IDictionary<string, object> htmlAttributes)
    {
      TagBuilder tagBuilder = new TagBuilder("a")
      {
        InnerHtml = HttpUtility.HtmlEncode(linkText)
      };
      tagBuilder.MergeAttributes<string, object>(htmlAttributes);
      tagBuilder.MergeAttribute("href", targetUrl);
      if (ajaxHelper.ViewContext.UnobtrusiveJavaScriptEnabled)
        tagBuilder.MergeAttributes<string, object>(ajaxOptions.ToUnobtrusiveHtmlAttributes());
      else
        tagBuilder.MergeAttribute("onclick", AjaxExtensions.GenerateAjaxScript(ajaxOptions, "Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), {0});"));
      return tagBuilder.ToString(TagRenderMode.Normal);
    }

    private static string GenerateAjaxScript(AjaxOptions ajaxOptions, string scriptFormat)
    {
      string javascriptString = ajaxOptions.ToJavascriptString();
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, scriptFormat, new object[1]
      {
        (object) javascriptString
      });
    }

    private static AjaxOptions GetAjaxOptions(AjaxOptions ajaxOptions)
    {
      return ajaxOptions == null ? new AjaxOptions() : ajaxOptions;
    }
  }
}
