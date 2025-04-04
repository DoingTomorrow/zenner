// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.BrowserHelpers
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.IO;
using System.Web.Hosting;

#nullable disable
namespace System.Web.WebPages
{
  public static class BrowserHelpers
  {
    private const string DesktopUserAgent = "Mozilla/4.0 (compatible; MSIE 6.1; Windows XP)";
    private const string MobileUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows CE; IEMobile 8.12; MSIEMobile 6.0)";
    private static readonly object _browserOverrideKey = new object();
    private static readonly object _userAgentKey = new object();

    public static void ClearOverriddenBrowser(this HttpContextBase httpContext)
    {
      string userAgent = (string) null;
      httpContext.SetOverriddenBrowser(userAgent);
    }

    private static HttpBrowserCapabilitiesBase CreateOverriddenBrowser(string userAgent)
    {
      return (HttpBrowserCapabilitiesBase) new HttpBrowserCapabilitiesWrapper(new HttpContext((HttpWorkerRequest) new BrowserHelpers.UserAgentWorkerRequest(userAgent)).Request.Browser);
    }

    public static HttpBrowserCapabilitiesBase GetOverriddenBrowser(this HttpContextBase httpContext)
    {
      return httpContext.GetOverriddenBrowser(new Func<string, HttpBrowserCapabilitiesBase>(BrowserHelpers.CreateOverriddenBrowser));
    }

    internal static HttpBrowserCapabilitiesBase GetOverriddenBrowser(
      this HttpContextBase httpContext,
      Func<string, HttpBrowserCapabilitiesBase> createBrowser)
    {
      HttpBrowserCapabilitiesBase overriddenBrowser = (HttpBrowserCapabilitiesBase) httpContext.Items[BrowserHelpers._browserOverrideKey];
      if (overriddenBrowser == null)
      {
        string overriddenUserAgent = httpContext.GetOverriddenUserAgent();
        overriddenBrowser = string.Equals(overriddenUserAgent, httpContext.Request.UserAgent) ? httpContext.Request.Browser : createBrowser(overriddenUserAgent);
        httpContext.Items[BrowserHelpers._browserOverrideKey] = (object) overriddenBrowser;
      }
      return overriddenBrowser;
    }

    public static string GetOverriddenUserAgent(this HttpContextBase httpContext)
    {
      return (string) httpContext.Items[BrowserHelpers._userAgentKey] ?? BrowserOverrideStores.Current.GetOverriddenUserAgent(httpContext) ?? httpContext.Request.UserAgent;
    }

    public static string GetVaryByCustomStringForOverriddenBrowser(this HttpContext httpContext)
    {
      return new HttpContextWrapper(httpContext).GetVaryByCustomStringForOverriddenBrowser();
    }

    public static string GetVaryByCustomStringForOverriddenBrowser(this HttpContextBase httpContext)
    {
      return httpContext.GetVaryByCustomStringForOverriddenBrowser((Func<string, HttpBrowserCapabilitiesBase>) (userAgent => BrowserHelpers.CreateOverriddenBrowser(userAgent)));
    }

    internal static string GetVaryByCustomStringForOverriddenBrowser(
      this HttpContextBase httpContext,
      Func<string, HttpBrowserCapabilitiesBase> generateBrowser)
    {
      return httpContext.GetOverriddenBrowser(generateBrowser).Type;
    }

    public static void SetOverriddenBrowser(
      this HttpContextBase httpContext,
      BrowserOverride browserOverride)
    {
      string userAgent = (string) null;
      switch (browserOverride)
      {
        case BrowserOverride.Desktop:
          if (httpContext.Request.Browser == null || httpContext.Request.Browser.IsMobileDevice)
          {
            userAgent = "Mozilla/4.0 (compatible; MSIE 6.1; Windows XP)";
            break;
          }
          break;
        case BrowserOverride.Mobile:
          if (httpContext.Request.Browser == null || !httpContext.Request.Browser.IsMobileDevice)
          {
            userAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows CE; IEMobile 8.12; MSIEMobile 6.0)";
            break;
          }
          break;
      }
      if (userAgent != null)
        httpContext.SetOverriddenBrowser(userAgent);
      else
        httpContext.ClearOverriddenBrowser();
    }

    public static void SetOverriddenBrowser(this HttpContextBase httpContext, string userAgent)
    {
      httpContext.Items[BrowserHelpers._userAgentKey] = (object) userAgent;
      httpContext.Items[BrowserHelpers._browserOverrideKey] = (object) null;
      BrowserOverrideStores.Current.SetOverriddenUserAgent(httpContext, userAgent);
    }

    private sealed class UserAgentWorkerRequest : SimpleWorkerRequest
    {
      private readonly string _userAgent;

      public UserAgentWorkerRequest(string userAgent)
      {
        TextWriter output = (TextWriter) null;
        // ISSUE: explicit constructor call
        base.\u002Ector(string.Empty, string.Empty, output);
        this._userAgent = userAgent;
      }

      public override string GetKnownRequestHeader(int index)
      {
        return index != 39 ? (string) null : this._userAgent;
      }
    }
  }
}
