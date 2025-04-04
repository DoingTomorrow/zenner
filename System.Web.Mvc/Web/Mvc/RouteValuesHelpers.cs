// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.RouteValuesHelpers
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc
{
  internal static class RouteValuesHelpers
  {
    public static RouteValueDictionary GetRouteValues(RouteValueDictionary routeValues)
    {
      return routeValues == null ? new RouteValueDictionary() : new RouteValueDictionary((IDictionary<string, object>) routeValues);
    }

    public static RouteValueDictionary MergeRouteValues(
      string actionName,
      string controllerName,
      RouteValueDictionary implicitRouteValues,
      RouteValueDictionary routeValues,
      bool includeImplicitMvcValues)
    {
      RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
      if (includeImplicitMvcValues)
      {
        object obj;
        if (implicitRouteValues != null && implicitRouteValues.TryGetValue("action", out obj))
          routeValueDictionary["action"] = obj;
        if (implicitRouteValues != null && implicitRouteValues.TryGetValue("controller", out obj))
          routeValueDictionary["controller"] = obj;
      }
      if (routeValues != null)
      {
        foreach (KeyValuePair<string, object> routeValue in RouteValuesHelpers.GetRouteValues(routeValues))
          routeValueDictionary[routeValue.Key] = routeValue.Value;
      }
      if (actionName != null)
        routeValueDictionary["action"] = (object) actionName;
      if (controllerName != null)
        routeValueDictionary["controller"] = (object) controllerName;
      return routeValueDictionary;
    }
  }
}
