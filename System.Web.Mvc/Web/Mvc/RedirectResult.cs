// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.RedirectResult
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class RedirectResult : ActionResult
  {
    public RedirectResult(string url)
      : this(url, false)
    {
    }

    public RedirectResult(string url, bool permanent)
    {
      if (string.IsNullOrEmpty(url))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (url));
      this.Permanent = permanent;
      this.Url = url;
    }

    public bool Permanent { get; private set; }

    public string Url { get; private set; }

    public override void ExecuteResult(ControllerContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      if (context.IsChildAction)
        throw new InvalidOperationException(MvcResources.RedirectAction_CannotRedirectInChildAction);
      string contentUrl = UrlHelper.GenerateContentUrl(this.Url, context.HttpContext);
      context.Controller.TempData.Keep();
      if (this.Permanent)
        context.HttpContext.Response.RedirectPermanent(contentUrl, false);
      else
        context.HttpContext.Response.Redirect(contentUrl, false);
    }
  }
}
