// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.JsonResult
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Text;
using System.Web.Mvc.Properties;
using System.Web.Script.Serialization;

#nullable disable
namespace System.Web.Mvc
{
  public class JsonResult : ActionResult
  {
    public JsonResult() => this.JsonRequestBehavior = JsonRequestBehavior.DenyGet;

    public Encoding ContentEncoding { get; set; }

    public string ContentType { get; set; }

    public object Data { get; set; }

    public JsonRequestBehavior JsonRequestBehavior { get; set; }

    public int? MaxJsonLength { get; set; }

    public int? RecursionLimit { get; set; }

    public override void ExecuteResult(ControllerContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
        throw new InvalidOperationException(MvcResources.JsonRequest_GetNotAllowed);
      HttpResponseBase response = context.HttpContext.Response;
      response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;
      if (this.ContentEncoding != null)
        response.ContentEncoding = this.ContentEncoding;
      if (this.Data == null)
        return;
      JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
      if (this.MaxJsonLength.HasValue)
        scriptSerializer.MaxJsonLength = this.MaxJsonLength.Value;
      if (this.RecursionLimit.HasValue)
        scriptSerializer.RecursionLimit = this.RecursionLimit.Value;
      response.Write(scriptSerializer.Serialize(this.Data));
    }
  }
}
