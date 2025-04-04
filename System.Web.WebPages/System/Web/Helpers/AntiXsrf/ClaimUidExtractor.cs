// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.AntiXsrf.ClaimUidExtractor
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web.Helpers.Claims;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.Helpers.AntiXsrf
{
  internal sealed class ClaimUidExtractor : IClaimUidExtractor
  {
    internal const string NameIdentifierClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    internal const string IdentityProviderClaimType = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";
    private readonly ClaimsIdentityConverter _claimsIdentityConverter;
    private readonly IAntiForgeryConfig _config;

    internal ClaimUidExtractor(
      IAntiForgeryConfig config,
      ClaimsIdentityConverter claimsIdentityConverter)
    {
      this._config = config;
      this._claimsIdentityConverter = claimsIdentityConverter;
    }

    public BinaryBlob ExtractClaimUid(IIdentity identity)
    {
      if (identity == null || !identity.IsAuthenticated || this._config.SuppressIdentityHeuristicChecks)
        return (BinaryBlob) null;
      ClaimsIdentity claimsIdentity = this._claimsIdentityConverter.TryConvert(identity);
      return claimsIdentity == null ? (BinaryBlob) null : new BinaryBlob(256, CryptoUtil.ComputeSHA256((IList<string>) ClaimUidExtractor.GetUniqueIdentifierParameters(claimsIdentity, this._config.UniqueClaimTypeIdentifier)));
    }

    internal static string[] GetUniqueIdentifierParameters(
      ClaimsIdentity claimsIdentity,
      string uniqueClaimTypeIdentifier)
    {
      IEnumerable<Claim> claims = claimsIdentity.GetClaims();
      if (!string.IsNullOrEmpty(uniqueClaimTypeIdentifier))
      {
        Claim claim1 = claims.SingleOrDefault<Claim>((Func<Claim, bool>) (claim => string.Equals(uniqueClaimTypeIdentifier, claim.ClaimType, StringComparison.Ordinal)));
        if (claim1 == null || string.IsNullOrEmpty(claim1.Value))
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.ClaimUidExtractor_ClaimNotPresent, new object[1]
          {
            (object) uniqueClaimTypeIdentifier
          }));
        return new string[2]
        {
          uniqueClaimTypeIdentifier,
          claim1.Value
        };
      }
      Claim claim2 = claims.SingleOrDefault<Claim>((Func<Claim, bool>) (claim => string.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", claim.ClaimType, StringComparison.Ordinal)));
      Claim claim3 = claims.SingleOrDefault<Claim>((Func<Claim, bool>) (claim => string.Equals("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", claim.ClaimType, StringComparison.Ordinal)));
      if (claim2 == null || string.IsNullOrEmpty(claim2.Value) || claim3 == null || string.IsNullOrEmpty(claim3.Value))
        throw new InvalidOperationException(WebPageResources.ClaimUidExtractor_DefaultClaimsNotPresent);
      return new string[4]
      {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
        claim2.Value,
        "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
        claim3.Value
      };
    }
  }
}
