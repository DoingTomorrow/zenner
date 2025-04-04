// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.RequireHttpsAttribute
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
  public class RequireHttpsAttribute : FilterAttribute, IAuthorizationFilter
  {
    public virtual void OnAuthorization(AuthorizationContext filterContext)
    {
      if (filterContext == null)
        throw new ArgumentNullException(nameof (filterContext));
      if (filterContext.HttpContext.Request.IsSecureConnection)
        return;
      this.HandleNonHttpsRequest(filterContext);
    }

    protected virtual void HandleNonHttpsRequest(AuthorizationContext filterContext)
    {
      if (!string.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
        throw new InvalidOperationException(MvcResources.RequireHttpsAttribute_MustUseSsl);
      string url = "https://" + filterContext.HttpContext.Request.Url.Host + filterContext.HttpContext.Request.RawUrl;
      filterContext.Result = (ActionResult) new RedirectResult(url);
    }
  }
}
