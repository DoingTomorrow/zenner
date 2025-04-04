// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.RedirectToRouteResult
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Mvc.Properties;
using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc
{
  public class RedirectToRouteResult : ActionResult
  {
    private RouteCollection _routes;

    public RedirectToRouteResult(RouteValueDictionary routeValues)
      : this((string) null, routeValues)
    {
    }

    public RedirectToRouteResult(string routeName, RouteValueDictionary routeValues)
      : this(routeName, routeValues, false)
    {
    }

    public RedirectToRouteResult(
      string routeName,
      RouteValueDictionary routeValues,
      bool permanent)
    {
      this.Permanent = permanent;
      this.RouteName = routeName ?? string.Empty;
      this.RouteValues = routeValues ?? new RouteValueDictionary();
    }

    public bool Permanent { get; private set; }

    public string RouteName { get; private set; }

    public RouteValueDictionary RouteValues { get; private set; }

    internal RouteCollection Routes
    {
      get
      {
        if (this._routes == null)
          this._routes = RouteTable.Routes;
        return this._routes;
      }
      set => this._routes = value;
    }

    public override void ExecuteResult(ControllerContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      if (context.IsChildAction)
        throw new InvalidOperationException(MvcResources.RedirectAction_CannotRedirectInChildAction);
      string url = UrlHelper.GenerateUrl(this.RouteName, (string) null, (string) null, this.RouteValues, this.Routes, context.RequestContext, false);
      if (string.IsNullOrEmpty(url))
        throw new InvalidOperationException(MvcResources.Common_NoRouteMatched);
      context.Controller.TempData.Keep();
      if (this.Permanent)
        context.HttpContext.Response.RedirectPermanent(url, false);
      else
        context.HttpContext.Response.Redirect(url, false);
    }
  }
}
