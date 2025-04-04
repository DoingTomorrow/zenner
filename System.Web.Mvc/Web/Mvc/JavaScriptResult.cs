// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.JavaScriptResult
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public class JavaScriptResult : ActionResult
  {
    public string Script { get; set; }

    public override void ExecuteResult(ControllerContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      HttpResponseBase response = context.HttpContext.Response;
      response.ContentType = "application/x-javascript";
      if (this.Script == null)
        return;
      response.Write(this.Script);
    }
  }
}
