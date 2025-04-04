// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.Claims.Claim
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Reflection;

#nullable disable
namespace System.Web.Helpers.Claims
{
  internal sealed class Claim
  {
    public Claim(string claimType, string value)
    {
      this.ClaimType = claimType;
      this.Value = value;
    }

    public string ClaimType { get; private set; }

    public string Value { get; private set; }

    internal static Claim Create<TClaim>(TClaim claim) => Claim.ClaimFactory<TClaim>.Create(claim);

    private static class ClaimFactory<TClaim>
    {
      private static readonly Func<TClaim, string> _claimTypeGetter = Claim.ClaimFactory<TClaim>.CreateClaimTypeGetter();
      private static readonly Func<TClaim, string> _valueGetter = Claim.ClaimFactory<TClaim>.CreateValueGetter();

      public static Claim Create(TClaim claim)
      {
        return new Claim(Claim.ClaimFactory<TClaim>._claimTypeGetter(claim), Claim.ClaimFactory<TClaim>._valueGetter(claim));
      }

      private static Func<TClaim, string> CreateClaimTypeGetter()
      {
        return Claim.ClaimFactory<TClaim>.CreateGeneralPropertyGetter("ClaimType") ?? Claim.ClaimFactory<TClaim>.CreateGeneralPropertyGetter("Type");
      }

      private static Func<TClaim, string> CreateGeneralPropertyGetter(string propertyName)
      {
        PropertyInfo property = typeof (TClaim).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public, (Binder) null, typeof (string), Type.EmptyTypes, (ParameterModifier[]) null);
        if (property == (PropertyInfo) null)
          return (Func<TClaim, string>) null;
        return (Func<TClaim, string>) Delegate.CreateDelegate(typeof (Func<TClaim, string>), property.GetGetMethod());
      }

      private static Func<TClaim, string> CreateValueGetter()
      {
        return Claim.ClaimFactory<TClaim>.CreateGeneralPropertyGetter("Value");
      }
    }
  }
}
