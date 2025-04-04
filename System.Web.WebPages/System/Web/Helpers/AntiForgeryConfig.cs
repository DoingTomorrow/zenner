// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.AntiForgeryConfig
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.ComponentModel;
using System.Text;

#nullable disable
namespace System.Web.Helpers
{
  public static class AntiForgeryConfig
  {
    internal const string AntiForgeryTokenFieldName = "__RequestVerificationToken";
    private static string _cookieName;
    private static string _uniqueClaimTypeIdentifier;

    public static IAntiForgeryAdditionalDataProvider AdditionalDataProvider { get; set; }

    public static string CookieName
    {
      get
      {
        if (AntiForgeryConfig._cookieName == null)
          AntiForgeryConfig._cookieName = AntiForgeryConfig.GetAntiForgeryCookieName();
        return AntiForgeryConfig._cookieName;
      }
      set => AntiForgeryConfig._cookieName = value;
    }

    public static bool RequireSsl { get; set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static bool SuppressIdentityHeuristicChecks { get; set; }

    public static string UniqueClaimTypeIdentifier
    {
      get => AntiForgeryConfig._uniqueClaimTypeIdentifier ?? string.Empty;
      set => AntiForgeryConfig._uniqueClaimTypeIdentifier = value;
    }

    private static string GetAntiForgeryCookieName()
    {
      return AntiForgeryConfig.GetAntiForgeryCookieName(HttpRuntime.AppDomainAppVirtualPath);
    }

    internal static string GetAntiForgeryCookieName(string appPath)
    {
      return string.IsNullOrEmpty(appPath) || appPath == "/" ? "__RequestVerificationToken" : "__RequestVerificationToken_" + HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes(appPath));
    }
  }
}
