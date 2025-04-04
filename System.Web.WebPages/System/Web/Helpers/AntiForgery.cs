// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.AntiForgery
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.ComponentModel;
using System.Web.Helpers.AntiXsrf;
using System.Web.Helpers.Claims;
using System.Web.Mvc;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.Helpers
{
  public static class AntiForgery
  {
    private static readonly AntiForgeryWorker _worker = AntiForgery.CreateSingletonAntiForgeryWorker();

    private static AntiForgeryWorker CreateSingletonAntiForgeryWorker()
    {
      ICryptoSystem cryptoSystem = (ICryptoSystem) MachineKey45CryptoSystem.Instance ?? (ICryptoSystem) new MachineKey40CryptoSystem();
      IAntiForgeryConfig config = (IAntiForgeryConfig) new AntiForgeryConfigWrapper();
      IAntiForgeryTokenSerializer serializer = (IAntiForgeryTokenSerializer) new AntiForgeryTokenSerializer(cryptoSystem);
      ITokenStore tokenStore = (ITokenStore) new AntiForgeryTokenStore(config, serializer);
      IClaimUidExtractor claimUidExtractor = (IClaimUidExtractor) new ClaimUidExtractor(config, ClaimsIdentityConverter.Default);
      ITokenValidator validator = (ITokenValidator) new TokenValidator(config, claimUidExtractor);
      return new AntiForgeryWorker(serializer, config, tokenStore, validator);
    }

    public static HtmlString GetHtml()
    {
      return HttpContext.Current != null ? AntiForgery._worker.GetFormInputElement((HttpContextBase) new HttpContextWrapper(HttpContext.Current)).ToHtmlString(TagRenderMode.SelfClosing) : throw new ArgumentException(WebPageResources.HttpContextUnavailable);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static void GetTokens(
      string oldCookieToken,
      out string newCookieToken,
      out string formToken)
    {
      if (HttpContext.Current == null)
        throw new ArgumentException(WebPageResources.HttpContextUnavailable);
      AntiForgery._worker.GetTokens((HttpContextBase) new HttpContextWrapper(HttpContext.Current), oldCookieToken, out newCookieToken, out formToken);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This method is deprecated. Use the GetHtml() method instead. To specify a custom domain for the generated cookie, use the <httpCookies> configuration element. To specify custom data to be embedded within the token, use the static AntiForgeryConfig.AdditionalDataProvider property.", true)]
    public static HtmlString GetHtml(
      HttpContextBase httpContext,
      string salt,
      string domain,
      string path)
    {
      if (httpContext == null)
        throw new ArgumentNullException(nameof (httpContext));
      if (!string.IsNullOrEmpty(salt) || !string.IsNullOrEmpty(domain) || !string.IsNullOrEmpty(path))
        throw new NotSupportedException("This method is deprecated. Use the GetHtml() method instead. To specify a custom domain for the generated cookie, use the <httpCookies> configuration element. To specify custom data to be embedded within the token, use the static AntiForgeryConfig.AdditionalDataProvider property.");
      return AntiForgery._worker.GetFormInputElement(httpContext).ToHtmlString(TagRenderMode.SelfClosing);
    }

    public static void Validate()
    {
      if (HttpContext.Current == null)
        throw new ArgumentException(WebPageResources.HttpContextUnavailable);
      AntiForgery._worker.Validate((HttpContextBase) new HttpContextWrapper(HttpContext.Current));
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static void Validate(string cookieToken, string formToken)
    {
      if (HttpContext.Current == null)
        throw new ArgumentException(WebPageResources.HttpContextUnavailable);
      AntiForgery._worker.Validate((HttpContextBase) new HttpContextWrapper(HttpContext.Current), cookieToken, formToken);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This method is deprecated. Use the Validate() method instead.", true)]
    public static void Validate(HttpContextBase httpContext, string salt)
    {
      if (httpContext == null)
        throw new ArgumentNullException(nameof (httpContext));
      if (!string.IsNullOrEmpty(salt))
        throw new NotSupportedException("This method is deprecated. Use the Validate() method instead.");
      AntiForgery._worker.Validate(httpContext);
    }
  }
}
