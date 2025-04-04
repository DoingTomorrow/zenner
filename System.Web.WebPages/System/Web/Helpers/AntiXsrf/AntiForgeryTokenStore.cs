// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.AntiXsrf.AntiForgeryTokenStore
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

#nullable disable
namespace System.Web.Helpers.AntiXsrf
{
  internal sealed class AntiForgeryTokenStore : ITokenStore
  {
    private readonly IAntiForgeryConfig _config;
    private readonly IAntiForgeryTokenSerializer _serializer;

    internal AntiForgeryTokenStore(
      IAntiForgeryConfig config,
      IAntiForgeryTokenSerializer serializer)
    {
      this._config = config;
      this._serializer = serializer;
    }

    public AntiForgeryToken GetCookieToken(HttpContextBase httpContext)
    {
      HttpCookie cookie = httpContext.Request.Cookies[this._config.CookieName];
      return cookie == null || string.IsNullOrEmpty(cookie.Value) ? (AntiForgeryToken) null : this._serializer.Deserialize(cookie.Value);
    }

    public AntiForgeryToken GetFormToken(HttpContextBase httpContext)
    {
      string serializedToken = httpContext.Request.Form[this._config.FormFieldName];
      return string.IsNullOrEmpty(serializedToken) ? (AntiForgeryToken) null : this._serializer.Deserialize(serializedToken);
    }

    public void SaveCookieToken(HttpContextBase httpContext, AntiForgeryToken token)
    {
      HttpCookie cookie = new HttpCookie(this._config.CookieName, this._serializer.Serialize(token))
      {
        HttpOnly = true
      };
      if (this._config.RequireSSL)
        cookie.Secure = true;
      httpContext.Response.Cookies.Set(cookie);
    }
  }
}
