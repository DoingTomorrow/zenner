// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ContentResult
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Text;

#nullable disable
namespace System.Web.Mvc
{
  public class ContentResult : ActionResult
  {
    public string Content { get; set; }

    public Encoding ContentEncoding { get; set; }

    public string ContentType { get; set; }

    public override void ExecuteResult(ControllerContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      HttpResponseBase response = context.HttpContext.Response;
      if (!string.IsNullOrEmpty(this.ContentType))
        response.ContentType = this.ContentType;
      if (this.ContentEncoding != null)
        response.ContentEncoding = this.ContentEncoding;
      if (this.Content == null)
        return;
      response.Write(this.Content);
    }
  }
}
