// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.AreaHelpers
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc
{
  internal static class AreaHelpers
  {
    public static string GetAreaName(RouteBase route)
    {
      switch (route)
      {
        case IRouteWithArea routeWithArea:
          return routeWithArea.Area;
        case Route route1:
          if (route1.DataTokens != null)
            return route1.DataTokens["area"] as string;
          break;
      }
      return (string) null;
    }

    public static string GetAreaName(RouteData routeData)
    {
      object obj;
      return routeData.DataTokens.TryGetValue("area", out obj) ? obj as string : AreaHelpers.GetAreaName(routeData.Route);
    }
  }
}
