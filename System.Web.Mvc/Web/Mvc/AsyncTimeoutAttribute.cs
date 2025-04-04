// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.AsyncTimeoutAttribute
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Mvc.Async;

#nullable disable
namespace System.Web.Mvc
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
  public class AsyncTimeoutAttribute : ActionFilterAttribute
  {
    public AsyncTimeoutAttribute(int duration)
    {
      this.Duration = duration >= -1 ? duration : throw Error.AsyncCommon_InvalidTimeout(nameof (duration));
    }

    public int Duration { get; private set; }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      if (filterContext == null)
        throw new ArgumentNullException(nameof (filterContext));
      if (!(filterContext.Controller is IAsyncManagerContainer controller))
        throw Error.AsyncCommon_ControllerMustImplementIAsyncManagerContainer(filterContext.Controller.GetType());
      controller.AsyncManager.Timeout = this.Duration;
      base.OnActionExecuting(filterContext);
    }
  }
}
