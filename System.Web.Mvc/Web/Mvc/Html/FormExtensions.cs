// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.FormExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc.Html
{
  public static class FormExtensions
  {
    public static MvcForm BeginForm(this HtmlHelper htmlHelper)
    {
      string rawUrl = htmlHelper.ViewContext.HttpContext.Request.RawUrl;
      return htmlHelper.FormHelper(rawUrl, FormMethod.Post, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginForm(this HtmlHelper htmlHelper, object routeValues)
    {
      return FormExtensions.BeginForm(htmlHelper, (string) null, (string) null, new RouteValueDictionary(routeValues), FormMethod.Post, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginForm(this HtmlHelper htmlHelper, RouteValueDictionary routeValues)
    {
      return FormExtensions.BeginForm(htmlHelper, (string) null, (string) null, routeValues, FormMethod.Post, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginForm(
      this HtmlHelper htmlHelper,
      string actionName,
      string controllerName)
    {
      return FormExtensions.BeginForm(htmlHelper, actionName, controllerName, new RouteValueDictionary(), FormMethod.Post, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginForm(
      this HtmlHelper htmlHelper,
      string actionName,
      string controllerName,
      object routeValues)
    {
      return FormExtensions.BeginForm(htmlHelper, actionName, controllerName, new RouteValueDictionary(routeValues), FormMethod.Post, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginForm(
      this HtmlHelper htmlHelper,
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues)
    {
      return FormExtensions.BeginForm(htmlHelper, actionName, controllerName, routeValues, FormMethod.Post, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginForm(
      this HtmlHelper htmlHelper,
      string actionName,
      string controllerName,
      FormMethod method)
    {
      return FormExtensions.BeginForm(htmlHelper, actionName, controllerName, new RouteValueDictionary(), method, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginForm(
      this HtmlHelper htmlHelper,
      string actionName,
      string controllerName,
      object routeValues,
      FormMethod method)
    {
      return FormExtensions.BeginForm(htmlHelper, actionName, controllerName, new RouteValueDictionary(routeValues), method, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginForm(
      this HtmlHelper htmlHelper,
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues,
      FormMethod method)
    {
      return FormExtensions.BeginForm(htmlHelper, actionName, controllerName, routeValues, method, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginForm(
      this HtmlHelper htmlHelper,
      string actionName,
      string controllerName,
      FormMethod method,
      object htmlAttributes)
    {
      return FormExtensions.BeginForm(htmlHelper, actionName, controllerName, new RouteValueDictionary(), method, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcForm BeginForm(
      this HtmlHelper htmlHelper,
      string actionName,
      string controllerName,
      FormMethod method,
      IDictionary<string, object> htmlAttributes)
    {
      return FormExtensions.BeginForm(htmlHelper, actionName, controllerName, new RouteValueDictionary(), method, htmlAttributes);
    }

    public static MvcForm BeginForm(
      this HtmlHelper htmlHelper,
      string actionName,
      string controllerName,
      object routeValues,
      FormMethod method,
      object htmlAttributes)
    {
      return FormExtensions.BeginForm(htmlHelper, actionName, controllerName, new RouteValueDictionary(routeValues), method, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcForm BeginForm(
      this HtmlHelper htmlHelper,
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues,
      FormMethod method,
      IDictionary<string, object> htmlAttributes)
    {
      string url = UrlHelper.GenerateUrl((string) null, actionName, controllerName, routeValues, htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, true);
      return htmlHelper.FormHelper(url, method, htmlAttributes);
    }

    public static MvcForm BeginRouteForm(this HtmlHelper htmlHelper, object routeValues)
    {
      return FormExtensions.BeginRouteForm(htmlHelper, (string) null, new RouteValueDictionary(routeValues), FormMethod.Post, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginRouteForm(
      this HtmlHelper htmlHelper,
      RouteValueDictionary routeValues)
    {
      return FormExtensions.BeginRouteForm(htmlHelper, (string) null, routeValues, FormMethod.Post, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginRouteForm(this HtmlHelper htmlHelper, string routeName)
    {
      return FormExtensions.BeginRouteForm(htmlHelper, routeName, new RouteValueDictionary(), FormMethod.Post, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginRouteForm(
      this HtmlHelper htmlHelper,
      string routeName,
      object routeValues)
    {
      return FormExtensions.BeginRouteForm(htmlHelper, routeName, new RouteValueDictionary(routeValues), FormMethod.Post, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginRouteForm(
      this HtmlHelper htmlHelper,
      string routeName,
      RouteValueDictionary routeValues)
    {
      return FormExtensions.BeginRouteForm(htmlHelper, routeName, routeValues, FormMethod.Post, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginRouteForm(
      this HtmlHelper htmlHelper,
      string routeName,
      FormMethod method)
    {
      return FormExtensions.BeginRouteForm(htmlHelper, routeName, new RouteValueDictionary(), method, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginRouteForm(
      this HtmlHelper htmlHelper,
      string routeName,
      object routeValues,
      FormMethod method)
    {
      return FormExtensions.BeginRouteForm(htmlHelper, routeName, new RouteValueDictionary(routeValues), method, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginRouteForm(
      this HtmlHelper htmlHelper,
      string routeName,
      RouteValueDictionary routeValues,
      FormMethod method)
    {
      return FormExtensions.BeginRouteForm(htmlHelper, routeName, routeValues, method, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcForm BeginRouteForm(
      this HtmlHelper htmlHelper,
      string routeName,
      FormMethod method,
      object htmlAttributes)
    {
      return FormExtensions.BeginRouteForm(htmlHelper, routeName, new RouteValueDictionary(), method, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcForm BeginRouteForm(
      this HtmlHelper htmlHelper,
      string routeName,
      FormMethod method,
      IDictionary<string, object> htmlAttributes)
    {
      return FormExtensions.BeginRouteForm(htmlHelper, routeName, new RouteValueDictionary(), method, htmlAttributes);
    }

    public static MvcForm BeginRouteForm(
      this HtmlHelper htmlHelper,
      string routeName,
      object routeValues,
      FormMethod method,
      object htmlAttributes)
    {
      return FormExtensions.BeginRouteForm(htmlHelper, routeName, new RouteValueDictionary(routeValues), method, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcForm BeginRouteForm(
      this HtmlHelper htmlHelper,
      string routeName,
      RouteValueDictionary routeValues,
      FormMethod method,
      IDictionary<string, object> htmlAttributes)
    {
      string url = UrlHelper.GenerateUrl(routeName, (string) null, (string) null, routeValues, htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, false);
      return htmlHelper.FormHelper(url, method, htmlAttributes);
    }

    public static void EndForm(this HtmlHelper htmlHelper)
    {
      FormExtensions.EndForm(htmlHelper.ViewContext);
    }

    internal static void EndForm(ViewContext viewContext)
    {
      viewContext.Writer.Write("</form>");
      viewContext.OutputClientValidation();
      viewContext.FormContext = (FormContext) null;
    }

    private static MvcForm FormHelper(
      this HtmlHelper htmlHelper,
      string formAction,
      FormMethod method,
      IDictionary<string, object> htmlAttributes)
    {
      TagBuilder tagBuilder = new TagBuilder("form");
      tagBuilder.MergeAttributes<string, object>(htmlAttributes);
      tagBuilder.MergeAttribute("action", formAction);
      tagBuilder.MergeAttribute(nameof (method), HtmlHelper.GetFormMethodString(method), true);
      bool flag = htmlHelper.ViewContext.ClientValidationEnabled && !htmlHelper.ViewContext.UnobtrusiveJavaScriptEnabled;
      if (flag)
        tagBuilder.GenerateId(htmlHelper.ViewContext.FormIdGenerator());
      htmlHelper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));
      MvcForm mvcForm = new MvcForm(htmlHelper.ViewContext);
      if (flag)
        htmlHelper.ViewContext.FormContext.FormId = tagBuilder.Attributes["id"];
      return mvcForm;
    }
  }
}
