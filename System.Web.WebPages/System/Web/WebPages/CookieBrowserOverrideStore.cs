// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.CookieBrowserOverrideStore
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

#nullable disable
namespace System.Web.WebPages
{
  public class CookieBrowserOverrideStore : BrowserOverrideStore
  {
    internal static readonly string BrowserOverrideCookieName = ".ASPXBrowserOverride";
    private readonly int _daysToExpire;

    public CookieBrowserOverrideStore()
      : this(7)
    {
    }

    public CookieBrowserOverrideStore(int daysToExpire) => this._daysToExpire = daysToExpire;

    public override string GetOverriddenUserAgent(HttpContextBase httpContext)
    {
      HttpCookieCollection cookies = httpContext.Response.Cookies;
      foreach (string allKey in cookies.AllKeys)
      {
        if (string.Equals(allKey, CookieBrowserOverrideStore.BrowserOverrideCookieName, StringComparison.OrdinalIgnoreCase))
        {
          HttpCookie httpCookie = cookies[CookieBrowserOverrideStore.BrowserOverrideCookieName];
          return httpCookie.Value != null ? httpCookie.Value : (string) null;
        }
      }
      return httpContext.Request.Cookies[CookieBrowserOverrideStore.BrowserOverrideCookieName]?.Value;
    }

    public override void SetOverriddenUserAgent(HttpContextBase httpContext, string userAgent)
    {
      HttpCookie cookie = new HttpCookie(CookieBrowserOverrideStore.BrowserOverrideCookieName, HttpUtility.UrlEncode(userAgent));
      if (userAgent == null)
        cookie.Expires = DateTime.Now.AddDays(-1.0);
      else if (this._daysToExpire > 0)
        cookie.Expires = DateTime.Now.AddDays((double) this._daysToExpire);
      httpContext.Response.Cookies.Remove(CookieBrowserOverrideStore.BrowserOverrideCookieName);
      httpContext.Response.Cookies.Add(cookie);
    }
  }
}
