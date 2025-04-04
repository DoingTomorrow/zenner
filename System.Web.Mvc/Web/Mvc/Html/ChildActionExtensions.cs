// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.ChildActionExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc.Properties;
using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc.Html
{
  public static class ChildActionExtensions
  {
    public static MvcHtmlString Action(this HtmlHelper htmlHelper, string actionName)
    {
      return ChildActionExtensions.Action(htmlHelper, actionName, (string) null, (RouteValueDictionary) null);
    }

    public static MvcHtmlString Action(
      this HtmlHelper htmlHelper,
      string actionName,
      object routeValues)
    {
      return ChildActionExtensions.Action(htmlHelper, actionName, (string) null, new RouteValueDictionary(routeValues));
    }

    public static MvcHtmlString Action(
      this HtmlHelper htmlHelper,
      string actionName,
      RouteValueDictionary routeValues)
    {
      return ChildActionExtensions.Action(htmlHelper, actionName, (string) null, routeValues);
    }

    public static MvcHtmlString Action(
      this HtmlHelper htmlHelper,
      string actionName,
      string controllerName)
    {
      return ChildActionExtensions.Action(htmlHelper, actionName, controllerName, (RouteValueDictionary) null);
    }

    public static MvcHtmlString Action(
      this HtmlHelper htmlHelper,
      string actionName,
      string controllerName,
      object routeValues)
    {
      return ChildActionExtensions.Action(htmlHelper, actionName, controllerName, new RouteValueDictionary(routeValues));
    }

    public static MvcHtmlString Action(
      this HtmlHelper htmlHelper,
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues)
    {
      using (StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.CurrentCulture))
      {
        ChildActionExtensions.ActionHelper(htmlHelper, actionName, controllerName, routeValues, (TextWriter) stringWriter);
        return MvcHtmlString.Create(stringWriter.ToString());
      }
    }

    public static void RenderAction(this HtmlHelper htmlHelper, string actionName)
    {
      ChildActionExtensions.RenderAction(htmlHelper, actionName, (string) null, (RouteValueDictionary) null);
    }

    public static void RenderAction(
      this HtmlHelper htmlHelper,
      string actionName,
      object routeValues)
    {
      ChildActionExtensions.RenderAction(htmlHelper, actionName, (string) null, new RouteValueDictionary(routeValues));
    }

    public static void RenderAction(
      this HtmlHelper htmlHelper,
      string actionName,
      RouteValueDictionary routeValues)
    {
      ChildActionExtensions.RenderAction(htmlHelper, actionName, (string) null, routeValues);
    }

    public static void RenderAction(
      this HtmlHelper htmlHelper,
      string actionName,
      string controllerName)
    {
      ChildActionExtensions.RenderAction(htmlHelper, actionName, controllerName, (RouteValueDictionary) null);
    }

    public static void RenderAction(
      this HtmlHelper htmlHelper,
      string actionName,
      string controllerName,
      object routeValues)
    {
      ChildActionExtensions.RenderAction(htmlHelper, actionName, controllerName, new RouteValueDictionary(routeValues));
    }

    public static void RenderAction(
      this HtmlHelper htmlHelper,
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues)
    {
      ChildActionExtensions.ActionHelper(htmlHelper, actionName, controllerName, routeValues, htmlHelper.ViewContext.Writer);
    }

    internal static void ActionHelper(
      HtmlHelper htmlHelper,
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues,
      TextWriter textWriter)
    {
      if (htmlHelper == null)
        throw new ArgumentNullException(nameof (htmlHelper));
      if (string.IsNullOrEmpty(actionName))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (actionName));
      RouteValueDictionary routeValueDictionary = routeValues;
      routeValues = ChildActionExtensions.MergeDictionaries(routeValues, htmlHelper.ViewContext.RouteData.Values);
      routeValues["action"] = (object) actionName;
      if (!string.IsNullOrEmpty(controllerName))
        routeValues["controller"] = (object) controllerName;
      bool usingAreas;
      VirtualPathData virtualPathForArea = htmlHelper.RouteCollection.GetVirtualPathForArea(htmlHelper.ViewContext.RequestContext, (string) null, routeValues, out usingAreas);
      if (virtualPathForArea == null)
        throw new InvalidOperationException(MvcResources.Common_NoRouteMatched);
      if (usingAreas)
      {
        routeValues.Remove("area");
        routeValueDictionary?.Remove("area");
      }
      if (routeValueDictionary != null)
        routeValues[ChildActionValueProvider.ChildActionValuesKey] = (object) new DictionaryValueProvider<object>((IDictionary<string, object>) routeValueDictionary, CultureInfo.InvariantCulture);
      RouteData routeData = ChildActionExtensions.CreateRouteData(virtualPathForArea.Route, routeValues, virtualPathForArea.DataTokens, htmlHelper.ViewContext);
      HttpContextBase httpContext = htmlHelper.ViewContext.HttpContext;
      ChildActionExtensions.ChildActionMvcHandler actionMvcHandler = new ChildActionExtensions.ChildActionMvcHandler(new RequestContext(httpContext, routeData));
      httpContext.Server.Execute(HttpHandlerUtil.WrapForServerExecute((IHttpHandler) actionMvcHandler), textWriter, true);
    }

    private static RouteData CreateRouteData(
      RouteBase route,
      RouteValueDictionary routeValues,
      RouteValueDictionary dataTokens,
      ViewContext parentViewContext)
    {
      RouteData routeData = new RouteData();
      foreach (KeyValuePair<string, object> routeValue in routeValues)
        routeData.Values.Add(routeValue.Key, routeValue.Value);
      foreach (KeyValuePair<string, object> dataToken in dataTokens)
        routeData.DataTokens.Add(dataToken.Key, dataToken.Value);
      routeData.Route = route;
      routeData.DataTokens["ParentActionViewContext"] = (object) parentViewContext;
      return routeData;
    }

    private static RouteValueDictionary MergeDictionaries(params RouteValueDictionary[] dictionaries)
    {
      RouteValueDictionary routeValueDictionary1 = new RouteValueDictionary();
      foreach (RouteValueDictionary routeValueDictionary2 in ((IEnumerable<RouteValueDictionary>) dictionaries).Where<RouteValueDictionary>((Func<RouteValueDictionary, bool>) (d => d != null)))
      {
        foreach (KeyValuePair<string, object> keyValuePair in routeValueDictionary2)
        {
          if (!routeValueDictionary1.ContainsKey(keyValuePair.Key))
            routeValueDictionary1.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
      return routeValueDictionary1;
    }

    internal class ChildActionMvcHandler(RequestContext context) : MvcHandler(context)
    {
      protected internal override void AddVersionHeader(HttpContextBase httpContext)
      {
      }
    }
  }
}
