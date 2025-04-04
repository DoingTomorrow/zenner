// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ActionFilterAttribute
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
  public abstract class ActionFilterAttribute : FilterAttribute, IActionFilter, IResultFilter
  {
    public virtual void OnActionExecuting(ActionExecutingContext filterContext)
    {
    }

    public virtual void OnActionExecuted(ActionExecutedContext filterContext)
    {
    }

    public virtual void OnResultExecuting(ResultExecutingContext filterContext)
    {
    }

    public virtual void OnResultExecuted(ResultExecutedContext filterContext)
    {
    }
  }
}
