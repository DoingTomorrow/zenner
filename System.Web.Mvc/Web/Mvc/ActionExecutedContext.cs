// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ActionExecutedContext
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public class ActionExecutedContext : ControllerContext
  {
    private ActionResult _result;

    public ActionExecutedContext()
    {
    }

    public ActionExecutedContext(
      ControllerContext controllerContext,
      ActionDescriptor actionDescriptor,
      bool canceled,
      Exception exception)
      : base(controllerContext)
    {
      this.ActionDescriptor = actionDescriptor != null ? actionDescriptor : throw new ArgumentNullException(nameof (actionDescriptor));
      this.Canceled = canceled;
      this.Exception = exception;
    }

    public virtual ActionDescriptor ActionDescriptor { get; set; }

    public virtual bool Canceled { get; set; }

    public virtual Exception Exception { get; set; }

    public bool ExceptionHandled { get; set; }

    public ActionResult Result
    {
      get => this._result ?? (ActionResult) EmptyResult.Instance;
      set => this._result = value;
    }
  }
}
