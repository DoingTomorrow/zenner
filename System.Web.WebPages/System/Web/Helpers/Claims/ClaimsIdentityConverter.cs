// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.Claims.ClaimsIdentityConverter
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.Reflection;
using System.Security.Principal;
using System.Web.Security;

#nullable disable
namespace System.Web.Helpers.Claims
{
  internal sealed class ClaimsIdentityConverter
  {
    private static readonly MethodInfo _claimsIdentityTryConvertOpenMethod = typeof (ClaimsIdentity).GetMethod("TryConvert", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    private static readonly ClaimsIdentityConverter _default = new ClaimsIdentityConverter(ClaimsIdentityConverter.GetDefaultConverters());
    private readonly Func<IIdentity, ClaimsIdentity>[] _converters;

    internal ClaimsIdentityConverter(Func<IIdentity, ClaimsIdentity>[] converters)
    {
      this._converters = converters;
    }

    public static ClaimsIdentityConverter Default => ClaimsIdentityConverter._default;

    private static bool IsGrandfatheredIdentityType(IIdentity claimsIdentity)
    {
      switch (claimsIdentity)
      {
        case FormsIdentity _:
        case WindowsIdentity _:
          return true;
        default:
          return claimsIdentity is GenericIdentity;
      }
    }

    public ClaimsIdentity TryConvert(IIdentity identity)
    {
      if (ClaimsIdentityConverter.IsGrandfatheredIdentityType(identity))
        return (ClaimsIdentity) null;
      for (int index = 0; index < this._converters.Length; ++index)
      {
        ClaimsIdentity claimsIdentity = this._converters[index](identity);
        if (claimsIdentity != null)
          return claimsIdentity;
      }
      return (ClaimsIdentity) null;
    }

    private static void AddToList(
      IList<Func<IIdentity, ClaimsIdentity>> converters,
      Type claimsIdentityType,
      Type claimType)
    {
      if (!(claimsIdentityType != (Type) null) || !(claimType != (Type) null))
        return;
      Func<IIdentity, ClaimsIdentity> func = (Func<IIdentity, ClaimsIdentity>) Delegate.CreateDelegate(typeof (Func<IIdentity, ClaimsIdentity>), ClaimsIdentityConverter._claimsIdentityTryConvertOpenMethod.MakeGenericMethod(claimsIdentityType, claimType));
      converters.Add(func);
    }

    private static Func<IIdentity, ClaimsIdentity>[] GetDefaultConverters()
    {
      List<Func<IIdentity, ClaimsIdentity>> converters = new List<Func<IIdentity, ClaimsIdentity>>();
      if (AppDomain.CurrentDomain.IsHomogenous && AppDomain.CurrentDomain.IsFullyTrusted)
      {
        Type type1 = Type.GetType("Microsoft.IdentityModel.Claims.IClaimsIdentity, Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
        Type type2 = Type.GetType("Microsoft.IdentityModel.Claims.Claim, Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
        ClaimsIdentityConverter.AddToList((IList<Func<IIdentity, ClaimsIdentity>>) converters, type1, type2);
      }
      Module module = typeof (object).Module;
      Type type3 = module.GetType("System.Security.Claims.ClaimsIdentity");
      Type type4 = module.GetType("System.Security.Claims.Claim");
      ClaimsIdentityConverter.AddToList((IList<Func<IIdentity, ClaimsIdentity>>) converters, type3, type4);
      return converters.ToArray();
    }
  }
}
