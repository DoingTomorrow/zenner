// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ResultExecutedContext
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public class ResultExecutedContext : ControllerContext
  {
    public ResultExecutedContext()
    {
    }

    public ResultExecutedContext(
      ControllerContext controllerContext,
      ActionResult result,
      bool canceled,
      Exception exception)
      : base(controllerContext)
    {
      this.Result = result != null ? result : throw new ArgumentNullException(nameof (result));
      this.Canceled = canceled;
      this.Exception = exception;
    }

    public virtual bool Canceled { get; set; }

    public virtual Exception Exception { get; set; }

    public bool ExceptionHandled { get; set; }

    public virtual ActionResult Result { get; set; }
  }
}
