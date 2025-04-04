// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.IControllerFactory
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Routing;
using System.Web.SessionState;

#nullable disable
namespace System.Web.Mvc
{
  public interface IControllerFactory
  {
    IController CreateController(RequestContext requestContext, string controllerName);

    SessionStateBehavior GetControllerSessionBehavior(
      RequestContext requestContext,
      string controllerName);

    void ReleaseController(IController controller);
  }
}
