// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.AntiXsrf.ITokenValidator
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Security.Principal;

#nullable disable
namespace System.Web.Helpers.AntiXsrf
{
  internal interface ITokenValidator
  {
    AntiForgeryToken GenerateCookieToken();

    AntiForgeryToken GenerateFormToken(
      HttpContextBase httpContext,
      IIdentity identity,
      AntiForgeryToken cookieToken);

    bool IsCookieTokenValid(AntiForgeryToken cookieToken);

    void ValidateTokens(
      HttpContextBase httpContext,
      IIdentity identity,
      AntiForgeryToken cookieToken,
      AntiForgeryToken formToken);
  }
}
