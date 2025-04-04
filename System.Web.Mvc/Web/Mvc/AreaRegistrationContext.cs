// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.AreaRegistrationContext
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc
{
  public class AreaRegistrationContext
  {
    private readonly HashSet<string> _namespaces = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    public AreaRegistrationContext(string areaName, RouteCollection routes)
      : this(areaName, routes, (object) null)
    {
    }

    public AreaRegistrationContext(string areaName, RouteCollection routes, object state)
    {
      if (string.IsNullOrEmpty(areaName))
        throw Error.ParameterCannotBeNullOrEmpty(nameof (areaName));
      if (routes == null)
        throw new ArgumentNullException(nameof (routes));
      this.AreaName = areaName;
      this.Routes = routes;
      this.State = state;
    }

    public string AreaName { get; private set; }

    public ICollection<string> Namespaces => (ICollection<string>) this._namespaces;

    public RouteCollection Routes { get; private set; }

    public object State { get; private set; }

    public Route MapRoute(string name, string url) => this.MapRoute(name, url, (object) null);

    public Route MapRoute(string name, string url, object defaults)
    {
      return this.MapRoute(name, url, defaults, (object) null);
    }

    public Route MapRoute(string name, string url, object defaults, object constraints)
    {
      return this.MapRoute(name, url, defaults, constraints, (string[]) null);
    }

    public Route MapRoute(string name, string url, string[] namespaces)
    {
      return this.MapRoute(name, url, (object) null, namespaces);
    }

    public Route MapRoute(string name, string url, object defaults, string[] namespaces)
    {
      return this.MapRoute(name, url, defaults, (object) null, namespaces);
    }

    public Route MapRoute(
      string name,
      string url,
      object defaults,
      object constraints,
      string[] namespaces)
    {
      if (namespaces == null && this.Namespaces != null)
        namespaces = this.Namespaces.ToArray<string>();
      Route route = this.Routes.MapRoute(name, url, defaults, constraints, namespaces);
      route.DataTokens["area"] = (object) this.AreaName;
      bool flag = namespaces == null || namespaces.Length == 0;
      route.DataTokens["UseNamespaceFallback"] = (object) flag;
      return route;
    }
  }
}
