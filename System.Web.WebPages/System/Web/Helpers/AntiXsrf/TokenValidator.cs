// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.AntiXsrf.TokenValidator
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Globalization;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.Helpers.AntiXsrf
{
  internal sealed class TokenValidator : ITokenValidator
  {
    private readonly IClaimUidExtractor _claimUidExtractor;
    private readonly IAntiForgeryConfig _config;

    internal TokenValidator(IAntiForgeryConfig config, IClaimUidExtractor claimUidExtractor)
    {
      this._config = config;
      this._claimUidExtractor = claimUidExtractor;
    }

    public AntiForgeryToken GenerateCookieToken()
    {
      return new AntiForgeryToken()
      {
        IsSessionToken = true
      };
    }

    public AntiForgeryToken GenerateFormToken(
      HttpContextBase httpContext,
      IIdentity identity,
      AntiForgeryToken cookieToken)
    {
      AntiForgeryToken formToken = new AntiForgeryToken()
      {
        SecurityToken = cookieToken.SecurityToken,
        IsSessionToken = false
      };
      bool flag = false;
      if (identity != null && identity.IsAuthenticated)
      {
        if (!this._config.SuppressIdentityHeuristicChecks)
          flag = true;
        formToken.ClaimUid = this._claimUidExtractor.ExtractClaimUid(identity);
        if (formToken.ClaimUid == null)
          formToken.Username = identity.Name;
      }
      if (this._config.AdditionalDataProvider != null)
        formToken.AdditionalData = this._config.AdditionalDataProvider.GetAdditionalData(httpContext);
      if (flag && string.IsNullOrEmpty(formToken.Username) && formToken.ClaimUid == null && string.IsNullOrEmpty(formToken.AdditionalData))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.TokenValidator_AuthenticatedUserWithoutUsername, new object[1]
        {
          (object) identity.GetType()
        }));
      return formToken;
    }

    public bool IsCookieTokenValid(AntiForgeryToken cookieToken)
    {
      return cookieToken != null && cookieToken.IsSessionToken;
    }

    public void ValidateTokens(
      HttpContextBase httpContext,
      IIdentity identity,
      AntiForgeryToken sessionToken,
      AntiForgeryToken fieldToken)
    {
      if (sessionToken == null)
        throw HttpAntiForgeryException.CreateCookieMissingException(this._config.CookieName);
      if (fieldToken == null)
        throw HttpAntiForgeryException.CreateFormFieldMissingException(this._config.FormFieldName);
      if (!sessionToken.IsSessionToken || fieldToken.IsSessionToken)
        throw HttpAntiForgeryException.CreateTokensSwappedException(this._config.CookieName, this._config.FormFieldName);
      if (!object.Equals((object) sessionToken.SecurityToken, (object) fieldToken.SecurityToken))
        throw HttpAntiForgeryException.CreateSecurityTokenMismatchException();
      string str = string.Empty;
      BinaryBlob objB = (BinaryBlob) null;
      if (identity != null && identity.IsAuthenticated)
      {
        objB = this._claimUidExtractor.ExtractClaimUid(identity);
        if (objB == null)
          str = identity.Name ?? string.Empty;
      }
      bool flag = str.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || str.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
      if (!string.Equals(fieldToken.Username, str, flag ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
        throw HttpAntiForgeryException.CreateUsernameMismatchException(fieldToken.Username, str);
      if (!object.Equals((object) fieldToken.ClaimUid, (object) objB))
        throw HttpAntiForgeryException.CreateClaimUidMismatchException();
      if (this._config.AdditionalDataProvider != null && !this._config.AdditionalDataProvider.ValidateAdditionalData(httpContext, fieldToken.AdditionalData))
        throw HttpAntiForgeryException.CreateAdditionalDataCheckFailedException();
    }
  }
}
