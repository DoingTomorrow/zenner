// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.UrlHelper
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc.Properties;
using System.Web.Routing;
using System.Web.WebPages;

#nullable disable
namespace System.Web.Mvc
{
  public class UrlHelper
  {
    private const string HttpRouteKey = "httproute";

    public UrlHelper(RequestContext requestContext)
      : this(requestContext, RouteTable.Routes)
    {
    }

    public UrlHelper(RequestContext requestContext, RouteCollection routeCollection)
    {
      if (requestContext == null)
        throw new ArgumentNullException(nameof (requestContext));
      if (routeCollection == null)
        throw new ArgumentNullException(nameof (routeCollection));
      this.RequestContext = requestContext;
      this.RouteCollection = routeCollection;
    }

    public RequestContext RequestContext { get; private set; }

    public RouteCollection RouteCollection { get; private set; }

    public string Action(string actionName)
    {
      return this.GenerateUrl((string) null, actionName, (string) null, (RouteValueDictionary) null);
    }

    public string Action(string actionName, object routeValues)
    {
      return this.GenerateUrl((string) null, actionName, (string) null, new RouteValueDictionary(routeValues));
    }

    public string Action(string actionName, RouteValueDictionary routeValues)
    {
      return this.GenerateUrl((string) null, actionName, (string) null, routeValues);
    }

    public string Action(string actionName, string controllerName)
    {
      return this.GenerateUrl((string) null, actionName, controllerName, (RouteValueDictionary) null);
    }

    public string Action(string actionName, string controllerName, object routeValues)
    {
      return this.GenerateUrl((string) null, actionName, controllerName, new RouteValueDictionary(routeValues));
    }

    public string Action(
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues)
    {
      return this.GenerateUrl((string) null, actionName, controllerName, routeValues);
    }

    public string Action(
      string actionName,
      string controllerName,
      object routeValues,
      string protocol)
    {
      return UrlHelper.GenerateUrl((string) null, actionName, controllerName, protocol, (string) null, (string) null, new RouteValueDictionary(routeValues), this.RouteCollection, this.RequestContext, true);
    }

    public string Action(
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues,
      string protocol,
      string hostName)
    {
      return UrlHelper.GenerateUrl((string) null, actionName, controllerName, protocol, hostName, (string) null, routeValues, this.RouteCollection, this.RequestContext, true);
    }

    public string Content(string contentPath)
    {
      return UrlHelper.GenerateContentUrl(contentPath, this.RequestContext.HttpContext);
    }

    public static string GenerateContentUrl(string contentPath, HttpContextBase httpContext)
    {
      if (string.IsNullOrEmpty(contentPath))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (contentPath));
      if (httpContext == null)
        throw new ArgumentNullException(nameof (httpContext));
      return contentPath[0] == '~' ? PathHelpers.GenerateClientUrl(httpContext, contentPath) : contentPath;
    }

    public string Encode(string url) => HttpUtility.UrlEncode(url);

    private string GenerateUrl(
      string routeName,
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues)
    {
      return UrlHelper.GenerateUrl(routeName, actionName, controllerName, routeValues, this.RouteCollection, this.RequestContext, true);
    }

    public static string GenerateUrl(
      string routeName,
      string actionName,
      string controllerName,
      string protocol,
      string hostName,
      string fragment,
      RouteValueDictionary routeValues,
      RouteCollection routeCollection,
      RequestContext requestContext,
      bool includeImplicitMvcValues)
    {
      string url1 = UrlHelper.GenerateUrl(routeName, actionName, controllerName, routeValues, routeCollection, requestContext, includeImplicitMvcValues);
      if (url1 != null)
      {
        if (!string.IsNullOrEmpty(fragment))
          url1 = url1 + "#" + fragment;
        if (!string.IsNullOrEmpty(protocol) || !string.IsNullOrEmpty(hostName))
        {
          Uri url2 = requestContext.HttpContext.Request.Url;
          protocol = !string.IsNullOrEmpty(protocol) ? protocol : Uri.UriSchemeHttp;
          hostName = !string.IsNullOrEmpty(hostName) ? hostName : url2.Host;
          string str = string.Empty;
          string scheme = url2.Scheme;
          if (string.Equals(protocol, scheme, StringComparison.OrdinalIgnoreCase))
            str = url2.IsDefaultPort ? string.Empty : ":" + Convert.ToString(url2.Port, (IFormatProvider) CultureInfo.InvariantCulture);
          url1 = protocol + Uri.SchemeDelimiter + hostName + str + url1;
        }
      }
      return url1;
    }

    public static string GenerateUrl(
      string routeName,
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues,
      RouteCollection routeCollection,
      RequestContext requestContext,
      bool includeImplicitMvcValues)
    {
      if (routeCollection == null)
        throw new ArgumentNullException(nameof (routeCollection));
      if (requestContext == null)
        throw new ArgumentNullException(nameof (requestContext));
      RouteValueDictionary values = RouteValuesHelpers.MergeRouteValues(actionName, controllerName, requestContext.RouteData.Values, routeValues, includeImplicitMvcValues);
      VirtualPathData virtualPathForArea = routeCollection.GetVirtualPathForArea(requestContext, routeName, values);
      return virtualPathForArea == null ? (string) null : PathHelpers.GenerateClientUrl(requestContext.HttpContext, virtualPathForArea.VirtualPath);
    }

    public bool IsLocalUrl(string url)
    {
      return this.RequestContext.HttpContext.Request.IsUrlLocalToHost(url);
    }

    public string RouteUrl(object routeValues) => this.RouteUrl((string) null, routeValues);

    public string RouteUrl(RouteValueDictionary routeValues)
    {
      return this.RouteUrl((string) null, routeValues);
    }

    public string RouteUrl(string routeName) => this.RouteUrl(routeName, (object) null);

    public string RouteUrl(string routeName, object routeValues)
    {
      return this.RouteUrl(routeName, routeValues, (string) null);
    }

    public string RouteUrl(string routeName, RouteValueDictionary routeValues)
    {
      return this.RouteUrl(routeName, routeValues, (string) null, (string) null);
    }

    public string RouteUrl(string routeName, object routeValues, string protocol)
    {
      return UrlHelper.GenerateUrl(routeName, (string) null, (string) null, protocol, (string) null, (string) null, new RouteValueDictionary(routeValues), this.RouteCollection, this.RequestContext, false);
    }

    public string RouteUrl(
      string routeName,
      RouteValueDictionary routeValues,
      string protocol,
      string hostName)
    {
      return UrlHelper.GenerateUrl(routeName, (string) null, (string) null, protocol, hostName, (string) null, routeValues, this.RouteCollection, this.RequestContext, false);
    }

    public string HttpRouteUrl(string routeName, object routeValues)
    {
      return this.HttpRouteUrl(routeName, new RouteValueDictionary(routeValues));
    }

    public string HttpRouteUrl(string routeName, RouteValueDictionary routeValues)
    {
      if (routeValues == null)
      {
        routeValues = new RouteValueDictionary();
        routeValues.Add("httproute", (object) true);
      }
      else
      {
        routeValues = new RouteValueDictionary((IDictionary<string, object>) routeValues);
        if (!routeValues.ContainsKey("httproute"))
          routeValues.Add("httproute", (object) true);
      }
      return UrlHelper.GenerateUrl(routeName, (string) null, (string) null, (string) null, (string) null, (string) null, routeValues, this.RouteCollection, this.RequestContext, false);
    }
  }
}
