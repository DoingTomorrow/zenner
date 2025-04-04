// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.LinkExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Web.Mvc.Properties;
using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc.Html
{
  public static class LinkExtensions
  {
    public static MvcHtmlString ActionLink(
      this HtmlHelper htmlHelper,
      string linkText,
      string actionName)
    {
      return LinkExtensions.ActionLink(htmlHelper, linkText, actionName, (string) null, new RouteValueDictionary(), (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcHtmlString ActionLink(
      this HtmlHelper htmlHelper,
      string linkText,
      string actionName,
      object routeValues)
    {
      return LinkExtensions.ActionLink(htmlHelper, linkText, actionName, (string) null, new RouteValueDictionary(routeValues), (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcHtmlString ActionLink(
      this HtmlHelper htmlHelper,
      string linkText,
      string actionName,
      object routeValues,
      object htmlAttributes)
    {
      return LinkExtensions.ActionLink(htmlHelper, linkText, actionName, (string) null, new RouteValueDictionary(routeValues), (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString ActionLink(
      this HtmlHelper htmlHelper,
      string linkText,
      string actionName,
      RouteValueDictionary routeValues)
    {
      return LinkExtensions.ActionLink(htmlHelper, linkText, actionName, (string) null, routeValues, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcHtmlString ActionLink(
      this HtmlHelper htmlHelper,
      string linkText,
      string actionName,
      RouteValueDictionary routeValues,
      IDictionary<string, object> htmlAttributes)
    {
      return LinkExtensions.ActionLink(htmlHelper, linkText, actionName, (string) null, routeValues, htmlAttributes);
    }

    public static MvcHtmlString ActionLink(
      this HtmlHelper htmlHelper,
      string linkText,
      string actionName,
      string controllerName)
    {
      return LinkExtensions.ActionLink(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(), (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcHtmlString ActionLink(
      this HtmlHelper htmlHelper,
      string linkText,
      string actionName,
      string controllerName,
      object routeValues,
      object htmlAttributes)
    {
      return LinkExtensions.ActionLink(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(routeValues), (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString ActionLink(
      this HtmlHelper htmlHelper,
      string linkText,
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(linkText))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (linkText));
      return MvcHtmlString.Create(HtmlHelper.GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, (string) null, actionName, controllerName, routeValues, htmlAttributes));
    }

    public static MvcHtmlString ActionLink(
      this HtmlHelper htmlHelper,
      string linkText,
      string actionName,
      string controllerName,
      string protocol,
      string hostName,
      string fragment,
      object routeValues,
      object htmlAttributes)
    {
      return LinkExtensions.ActionLink(htmlHelper, linkText, actionName, controllerName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString ActionLink(
      this HtmlHelper htmlHelper,
      string linkText,
      string actionName,
      string controllerName,
      string protocol,
      string hostName,
      string fragment,
      RouteValueDictionary routeValues,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(linkText))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (linkText));
      return MvcHtmlString.Create(HtmlHelper.GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, (string) null, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes));
    }

    public static MvcHtmlString RouteLink(
      this HtmlHelper htmlHelper,
      string linkText,
      object routeValues)
    {
      return LinkExtensions.RouteLink(htmlHelper, linkText, new RouteValueDictionary(routeValues));
    }

    public static MvcHtmlString RouteLink(
      this HtmlHelper htmlHelper,
      string linkText,
      RouteValueDictionary routeValues)
    {
      return LinkExtensions.RouteLink(htmlHelper, linkText, routeValues, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcHtmlString RouteLink(
      this HtmlHelper htmlHelper,
      string linkText,
      string routeName)
    {
      return LinkExtensions.RouteLink(htmlHelper, linkText, routeName, (object) null);
    }

    public static MvcHtmlString RouteLink(
      this HtmlHelper htmlHelper,
      string linkText,
      string routeName,
      object routeValues)
    {
      return LinkExtensions.RouteLink(htmlHelper, linkText, routeName, new RouteValueDictionary(routeValues));
    }

    public static MvcHtmlString RouteLink(
      this HtmlHelper htmlHelper,
      string linkText,
      string routeName,
      RouteValueDictionary routeValues)
    {
      return LinkExtensions.RouteLink(htmlHelper, linkText, routeName, routeValues, (IDictionary<string, object>) new RouteValueDictionary());
    }

    public static MvcHtmlString RouteLink(
      this HtmlHelper htmlHelper,
      string linkText,
      object routeValues,
      object htmlAttributes)
    {
      return LinkExtensions.RouteLink(htmlHelper, linkText, new RouteValueDictionary(routeValues), (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString RouteLink(
      this HtmlHelper htmlHelper,
      string linkText,
      RouteValueDictionary routeValues,
      IDictionary<string, object> htmlAttributes)
    {
      return LinkExtensions.RouteLink(htmlHelper, linkText, (string) null, routeValues, htmlAttributes);
    }

    public static MvcHtmlString RouteLink(
      this HtmlHelper htmlHelper,
      string linkText,
      string routeName,
      object routeValues,
      object htmlAttributes)
    {
      return LinkExtensions.RouteLink(htmlHelper, linkText, routeName, new RouteValueDictionary(routeValues), (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString RouteLink(
      this HtmlHelper htmlHelper,
      string linkText,
      string routeName,
      RouteValueDictionary routeValues,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(linkText))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (linkText));
      return MvcHtmlString.Create(HtmlHelper.GenerateRouteLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, routeName, routeValues, htmlAttributes));
    }

    public static MvcHtmlString RouteLink(
      this HtmlHelper htmlHelper,
      string linkText,
      string routeName,
      string protocol,
      string hostName,
      string fragment,
      object routeValues,
      object htmlAttributes)
    {
      return LinkExtensions.RouteLink(htmlHelper, linkText, routeName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString RouteLink(
      this HtmlHelper htmlHelper,
      string linkText,
      string routeName,
      string protocol,
      string hostName,
      string fragment,
      RouteValueDictionary routeValues,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(linkText))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (linkText));
      return MvcHtmlString.Create(HtmlHelper.GenerateRouteLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, routeName, protocol, hostName, fragment, routeValues, htmlAttributes));
    }
  }
}
