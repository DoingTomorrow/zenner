// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ControllerContext
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Routing;
using System.Web.WebPages;

#nullable disable
namespace System.Web.Mvc
{
  public class ControllerContext
  {
    internal const string ParentActionViewContextToken = "ParentActionViewContext";
    private HttpContextBase _httpContext;
    private RequestContext _requestContext;
    private RouteData _routeData;

    public ControllerContext()
    {
    }

    protected ControllerContext(ControllerContext controllerContext)
    {
      this.Controller = controllerContext != null ? controllerContext.Controller : throw new ArgumentNullException(nameof (controllerContext));
      this.RequestContext = controllerContext.RequestContext;
    }

    public ControllerContext(
      HttpContextBase httpContext,
      RouteData routeData,
      ControllerBase controller)
      : this(new RequestContext(httpContext, routeData), controller)
    {
    }

    public ControllerContext(RequestContext requestContext, ControllerBase controller)
    {
      if (requestContext == null)
        throw new ArgumentNullException(nameof (requestContext));
      if (controller == null)
        throw new ArgumentNullException(nameof (controller));
      this.RequestContext = requestContext;
      this.Controller = controller;
    }

    public virtual ControllerBase Controller { get; set; }

    public IDisplayMode DisplayMode
    {
      get => DisplayModeProvider.GetDisplayMode(this.HttpContext);
      set => DisplayModeProvider.SetDisplayMode(this.HttpContext, value);
    }

    public virtual HttpContextBase HttpContext
    {
      get
      {
        if (this._httpContext == null)
          this._httpContext = this._requestContext != null ? this._requestContext.HttpContext : (HttpContextBase) new ControllerContext.EmptyHttpContext();
        return this._httpContext;
      }
      set => this._httpContext = value;
    }

    public virtual bool IsChildAction
    {
      get
      {
        RouteData routeData = this.RouteData;
        return routeData != null && routeData.DataTokens.ContainsKey("ParentActionViewContext");
      }
    }

    public ViewContext ParentActionViewContext
    {
      get => this.RouteData.DataTokens[nameof (ParentActionViewContext)] as ViewContext;
    }

    public RequestContext RequestContext
    {
      get
      {
        if (this._requestContext == null)
          this._requestContext = new RequestContext(this.HttpContext ?? (HttpContextBase) new ControllerContext.EmptyHttpContext(), this.RouteData ?? new RouteData());
        return this._requestContext;
      }
      set => this._requestContext = value;
    }

    public virtual RouteData RouteData
    {
      get
      {
        if (this._routeData == null)
          this._routeData = this._requestContext != null ? this._requestContext.RouteData : new RouteData();
        return this._routeData;
      }
      set => this._routeData = value;
    }

    private sealed class EmptyHttpContext : HttpContextBase
    {
    }
  }
}
