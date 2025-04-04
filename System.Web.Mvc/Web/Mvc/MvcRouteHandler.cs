// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.MvcRouteHandler
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Mvc.Properties;
using System.Web.Routing;
using System.Web.SessionState;

#nullable disable
namespace System.Web.Mvc
{
  public class MvcRouteHandler : IRouteHandler
  {
    private IControllerFactory _controllerFactory;

    public MvcRouteHandler()
    {
    }

    public MvcRouteHandler(IControllerFactory controllerFactory)
    {
      this._controllerFactory = controllerFactory;
    }

    protected virtual IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      requestContext.HttpContext.SetSessionStateBehavior(this.GetSessionStateBehavior(requestContext));
      return (IHttpHandler) new MvcHandler(requestContext);
    }

    protected virtual SessionStateBehavior GetSessionStateBehavior(RequestContext requestContext)
    {
      string controllerName = (string) requestContext.RouteData.Values["controller"];
      if (string.IsNullOrWhiteSpace(controllerName))
        throw new InvalidOperationException(MvcResources.MvcRouteHandler_RouteValuesHasNoController);
      return (this._controllerFactory ?? ControllerBuilder.Current.GetControllerFactory()).GetControllerSessionBehavior(requestContext, controllerName);
    }

    IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
    {
      return this.GetHttpHandler(requestContext);
    }
  }
}
