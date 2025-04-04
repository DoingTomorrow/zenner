// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ExceptionContext
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public class ExceptionContext : ControllerContext
  {
    private ActionResult _result;

    public ExceptionContext()
    {
    }

    public ExceptionContext(ControllerContext controllerContext, Exception exception)
      : base(controllerContext)
    {
      this.Exception = exception != null ? exception : throw new ArgumentNullException(nameof (exception));
    }

    public virtual Exception Exception { get; set; }

    public bool ExceptionHandled { get; set; }

    public ActionResult Result
    {
      get => this._result ?? (ActionResult) EmptyResult.Instance;
      set => this._result = value;
    }
  }
}
