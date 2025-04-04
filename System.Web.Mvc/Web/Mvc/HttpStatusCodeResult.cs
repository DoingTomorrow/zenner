// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.HttpStatusCodeResult
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Net;

#nullable disable
namespace System.Web.Mvc
{
  public class HttpStatusCodeResult : ActionResult
  {
    public HttpStatusCodeResult(int statusCode)
      : this(statusCode, (string) null)
    {
    }

    public HttpStatusCodeResult(HttpStatusCode statusCode)
      : this(statusCode, (string) null)
    {
    }

    public HttpStatusCodeResult(HttpStatusCode statusCode, string statusDescription)
      : this((int) statusCode, statusDescription)
    {
    }

    public HttpStatusCodeResult(int statusCode, string statusDescription)
    {
      this.StatusCode = statusCode;
      this.StatusDescription = statusDescription;
    }

    public int StatusCode { get; private set; }

    public string StatusDescription { get; private set; }

    public override void ExecuteResult(ControllerContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      context.HttpContext.Response.StatusCode = this.StatusCode;
      if (this.StatusDescription == null)
        return;
      context.HttpContext.Response.StatusDescription = this.StatusDescription;
    }
  }
}
