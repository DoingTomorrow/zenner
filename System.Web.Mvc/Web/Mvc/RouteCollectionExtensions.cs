// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.RouteCollectionExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc
{
  public static class RouteCollectionExtensions
  {
    private static RouteCollection FilterRouteCollectionByArea(
      RouteCollection routes,
      string areaName,
      out bool usingAreas)
    {
      if (areaName == null)
        areaName = string.Empty;
      usingAreas = false;
      RouteCollection routeCollection = new RouteCollection();
      using (routes.GetReadLock())
      {
        foreach (RouteBase route in (Collection<RouteBase>) routes)
        {
          string a = AreaHelpers.GetAreaName(route) ?? string.Empty;
          usingAreas |= a.Length > 0;
          if (string.Equals(a, areaName, StringComparison.OrdinalIgnoreCase))
            routeCollection.Add(route);
        }
      }
      return !usingAreas ? routes : routeCollection;
    }

    public static VirtualPathData GetVirtualPathForArea(
      this RouteCollection routes,
      RequestContext requestContext,
      RouteValueDictionary values)
    {
      return routes.GetVirtualPathForArea(requestContext, (string) null, values);
    }

    public static VirtualPathData GetVirtualPathForArea(
      this RouteCollection routes,
      RequestContext requestContext,
      string name,
      RouteValueDictionary values)
    {
      return routes.GetVirtualPathForArea(requestContext, name, values, out bool _);
    }

    internal static VirtualPathData GetVirtualPathForArea(
      this RouteCollection routes,
      RequestContext requestContext,
      string name,
      RouteValueDictionary values,
      out bool usingAreas)
    {
      if (routes == null)
        throw new ArgumentNullException(nameof (routes));
      if (!string.IsNullOrEmpty(name))
      {
        usingAreas = false;
        return routes.GetVirtualPath(requestContext, name, values);
      }
      string areaName = (string) null;
      if (values != null)
      {
        object obj;
        if (values.TryGetValue("area", out obj))
          areaName = obj as string;
        else if (requestContext != null)
          areaName = AreaHelpers.GetAreaName(requestContext.RouteData);
      }
      RouteValueDictionary values1 = values;
      RouteCollection routeCollection = RouteCollectionExtensions.FilterRouteCollectionByArea(routes, areaName, out usingAreas);
      if (usingAreas)
      {
        values1 = new RouteValueDictionary((IDictionary<string, object>) values);
        values1.Remove("area");
      }
      return routeCollection.GetVirtualPath(requestContext, values1);
    }

    public static void IgnoreRoute(this RouteCollection routes, string url)
    {
      routes.IgnoreRoute(url, (object) null);
    }

    public static void IgnoreRoute(this RouteCollection routes, string url, object constraints)
    {
      if (routes == null)
        throw new ArgumentNullException(nameof (routes));
      RouteCollectionExtensions.IgnoreRouteInternal ignoreRouteInternal1 = url != null ? new RouteCollectionExtensions.IgnoreRouteInternal(url) : throw new ArgumentNullException(nameof (url));
      ignoreRouteInternal1.Constraints = new RouteValueDictionary(constraints);
      RouteCollectionExtensions.IgnoreRouteInternal ignoreRouteInternal2 = ignoreRouteInternal1;
      routes.Add((RouteBase) ignoreRouteInternal2);
    }

    public static Route MapRoute(this RouteCollection routes, string name, string url)
    {
      return routes.MapRoute(name, url, (object) null, (object) null);
    }

    public static Route MapRoute(
      this RouteCollection routes,
      string name,
      string url,
      object defaults)
    {
      return routes.MapRoute(name, url, defaults, (object) null);
    }

    public static Route MapRoute(
      this RouteCollection routes,
      string name,
      string url,
      object defaults,
      object constraints)
    {
      return routes.MapRoute(name, url, defaults, constraints, (string[]) null);
    }

    public static Route MapRoute(
      this RouteCollection routes,
      string name,
      string url,
      string[] namespaces)
    {
      return routes.MapRoute(name, url, (object) null, (object) null, namespaces);
    }

    public static Route MapRoute(
      this RouteCollection routes,
      string name,
      string url,
      object defaults,
      string[] namespaces)
    {
      return routes.MapRoute(name, url, defaults, (object) null, namespaces);
    }

    public static Route MapRoute(
      this RouteCollection routes,
      string name,
      string url,
      object defaults,
      object constraints,
      string[] namespaces)
    {
      if (routes == null)
        throw new ArgumentNullException(nameof (routes));
      if (url == null)
        throw new ArgumentNullException(nameof (url));
      Route route = new Route(url, (IRouteHandler) new MvcRouteHandler())
      {
        Defaults = RouteCollectionExtensions.CreateRouteValueDictionary(defaults),
        Constraints = RouteCollectionExtensions.CreateRouteValueDictionary(constraints),
        DataTokens = new RouteValueDictionary()
      };
      if (namespaces != null && namespaces.Length > 0)
        route.DataTokens["Namespaces"] = (object) namespaces;
      routes.Add(name, (RouteBase) route);
      return route;
    }

    private static RouteValueDictionary CreateRouteValueDictionary(object values)
    {
      return values is IDictionary<string, object> dictionary ? new RouteValueDictionary(dictionary) : new RouteValueDictionary(values);
    }

    private sealed class IgnoreRouteInternal(string url) : Route(url, (IRouteHandler) new StopRoutingHandler())
    {
      public override VirtualPathData GetVirtualPath(
        RequestContext requestContext,
        RouteValueDictionary routeValues)
      {
        return (VirtualPathData) null;
      }
    }
  }
}
