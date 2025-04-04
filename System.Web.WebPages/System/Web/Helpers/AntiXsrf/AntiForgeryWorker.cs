// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.AntiXsrf.AntiForgeryWorker
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Security.Principal;
using System.Web.Mvc;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.Helpers.AntiXsrf
{
  internal sealed class AntiForgeryWorker
  {
    private readonly IAntiForgeryConfig _config;
    private readonly IAntiForgeryTokenSerializer _serializer;
    private readonly ITokenStore _tokenStore;
    private readonly ITokenValidator _validator;

    internal AntiForgeryWorker(
      IAntiForgeryTokenSerializer serializer,
      IAntiForgeryConfig config,
      ITokenStore tokenStore,
      ITokenValidator validator)
    {
      this._serializer = serializer;
      this._config = config;
      this._tokenStore = tokenStore;
      this._validator = validator;
    }

    private void CheckSSLConfig(HttpContextBase httpContext)
    {
      if (this._config.RequireSSL && !httpContext.Request.IsSecureConnection)
        throw new InvalidOperationException(WebPageResources.AntiForgeryWorker_RequireSSL);
    }

    private AntiForgeryToken DeserializeToken(string serializedToken)
    {
      return string.IsNullOrEmpty(serializedToken) ? (AntiForgeryToken) null : this._serializer.Deserialize(serializedToken);
    }

    private AntiForgeryToken DeserializeTokenNoThrow(string serializedToken)
    {
      try
      {
        return this.DeserializeToken(serializedToken);
      }
      catch
      {
        return (AntiForgeryToken) null;
      }
    }

    private static IIdentity ExtractIdentity(HttpContextBase httpContext)
    {
      if (httpContext != null)
      {
        IPrincipal user = httpContext.User;
        if (user != null)
          return user.Identity;
      }
      return (IIdentity) null;
    }

    private AntiForgeryToken GetCookieTokenNoThrow(HttpContextBase httpContext)
    {
      try
      {
        return this._tokenStore.GetCookieToken(httpContext);
      }
      catch
      {
        return (AntiForgeryToken) null;
      }
    }

    public TagBuilder GetFormInputElement(HttpContextBase httpContext)
    {
      this.CheckSSLConfig(httpContext);
      AntiForgeryToken cookieTokenNoThrow = this.GetCookieTokenNoThrow(httpContext);
      AntiForgeryToken newCookieToken;
      AntiForgeryToken formToken;
      this.GetTokens(httpContext, cookieTokenNoThrow, out newCookieToken, out formToken);
      if (newCookieToken != null)
        this._tokenStore.SaveCookieToken(httpContext, newCookieToken);
      return new TagBuilder("input")
      {
        Attributes = {
          ["type"] = "hidden",
          ["name"] = this._config.FormFieldName,
          ["value"] = this._serializer.Serialize(formToken)
        }
      };
    }

    public void GetTokens(
      HttpContextBase httpContext,
      string serializedOldCookieToken,
      out string serializedNewCookieToken,
      out string serializedFormToken)
    {
      this.CheckSSLConfig(httpContext);
      AntiForgeryToken oldCookieToken = this.DeserializeTokenNoThrow(serializedOldCookieToken);
      AntiForgeryToken newCookieToken;
      AntiForgeryToken formToken;
      this.GetTokens(httpContext, oldCookieToken, out newCookieToken, out formToken);
      serializedNewCookieToken = this.Serialize(newCookieToken);
      serializedFormToken = this.Serialize(formToken);
    }

    private void GetTokens(
      HttpContextBase httpContext,
      AntiForgeryToken oldCookieToken,
      out AntiForgeryToken newCookieToken,
      out AntiForgeryToken formToken)
    {
      newCookieToken = (AntiForgeryToken) null;
      if (!this._validator.IsCookieTokenValid(oldCookieToken))
        oldCookieToken = newCookieToken = this._validator.GenerateCookieToken();
      formToken = this._validator.GenerateFormToken(httpContext, AntiForgeryWorker.ExtractIdentity(httpContext), oldCookieToken);
    }

    private string Serialize(AntiForgeryToken token)
    {
      return token == null ? (string) null : this._serializer.Serialize(token);
    }

    public void Validate(HttpContextBase httpContext)
    {
      this.CheckSSLConfig(httpContext);
      AntiForgeryToken cookieToken = this._tokenStore.GetCookieToken(httpContext);
      AntiForgeryToken formToken = this._tokenStore.GetFormToken(httpContext);
      this._validator.ValidateTokens(httpContext, AntiForgeryWorker.ExtractIdentity(httpContext), cookieToken, formToken);
    }

    public void Validate(HttpContextBase httpContext, string cookieToken, string formToken)
    {
      this.CheckSSLConfig(httpContext);
      AntiForgeryToken cookieToken1 = this.DeserializeToken(cookieToken);
      AntiForgeryToken formToken1 = this.DeserializeToken(formToken);
      this._validator.ValidateTokens(httpContext, AntiForgeryWorker.ExtractIdentity(httpContext), cookieToken1, formToken1);
    }
  }
}
