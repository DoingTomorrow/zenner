// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.Claims.ClaimsIdentity
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;

#nullable disable
namespace System.Web.Helpers.Claims
{
  internal abstract class ClaimsIdentity
  {
    public abstract IEnumerable<Claim> GetClaims();

    internal static ClaimsIdentity TryConvert<TClaimsIdentity, TClaim>(IIdentity identity) where TClaimsIdentity : class, IIdentity
    {
      return !(identity is TClaimsIdentity claimsIdentity) ? (ClaimsIdentity) null : (ClaimsIdentity) new ClaimsIdentity.ClaimsIdentityImpl<TClaimsIdentity, TClaim>(claimsIdentity);
    }

    private sealed class ClaimsIdentityImpl<TClaimsIdentity, TClaim> : ClaimsIdentity where TClaimsIdentity : class, IIdentity
    {
      private static readonly Func<TClaimsIdentity, IEnumerable<TClaim>> _claimsGetter = ClaimsIdentity.ClaimsIdentityImpl<TClaimsIdentity, TClaim>.CreateClaimsGetter();
      private readonly TClaimsIdentity _claimsIdentity;

      public ClaimsIdentityImpl(TClaimsIdentity claimsIdentity)
      {
        this._claimsIdentity = claimsIdentity;
      }

      private static Func<TClaimsIdentity, IEnumerable<TClaim>> CreateClaimsGetter()
      {
        return (Func<TClaimsIdentity, IEnumerable<TClaim>>) Delegate.CreateDelegate(typeof (Func<TClaimsIdentity, IEnumerable<TClaim>>), typeof (TClaimsIdentity).GetProperty("Claims", BindingFlags.Instance | BindingFlags.Public).GetGetMethod());
      }

      public override IEnumerable<Claim> GetClaims()
      {
        return ClaimsIdentity.ClaimsIdentityImpl<TClaimsIdentity, TClaim>._claimsGetter(this._claimsIdentity).Select<TClaim, Claim>(new Func<TClaim, Claim>(Claim.Create<TClaim>));
      }
    }
  }
}
