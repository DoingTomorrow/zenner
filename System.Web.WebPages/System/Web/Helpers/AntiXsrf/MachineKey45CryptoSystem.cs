// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.AntiXsrf.MachineKey45CryptoSystem
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Reflection;
using System.Web.Security;

#nullable disable
namespace System.Web.Helpers.AntiXsrf
{
  internal sealed class MachineKey45CryptoSystem : ICryptoSystem
  {
    private static readonly string[] _purposes = new string[1]
    {
      "System.Web.Helpers.AntiXsrf.AntiForgeryToken.v1"
    };
    private static readonly MachineKey45CryptoSystem _singletonInstance = MachineKey45CryptoSystem.GetSingletonInstance();
    private readonly Func<byte[], string[], byte[]> _protectThunk;
    private readonly Func<byte[], string[], byte[]> _unprotectThunk;

    internal MachineKey45CryptoSystem(
      Func<byte[], string[], byte[]> protectThunk,
      Func<byte[], string[], byte[]> unprotectThunk)
    {
      this._protectThunk = protectThunk;
      this._unprotectThunk = unprotectThunk;
    }

    public static MachineKey45CryptoSystem Instance => MachineKey45CryptoSystem._singletonInstance;

    private static MachineKey45CryptoSystem GetSingletonInstance()
    {
      PropertyInfo property = typeof (HttpRuntime).GetProperty("TargetFramework", typeof (Version));
      Version version = property != (PropertyInfo) null ? property.GetValue((object) null, (object[]) null) as Version : (Version) null;
      if (version != (Version) null && version >= new Version(4, 5))
      {
        Func<byte[], string[], byte[]> protectThunk = (Func<byte[], string[], byte[]>) Delegate.CreateDelegate(typeof (Func<byte[], string[], byte[]>), typeof (MachineKey), "Protect", false, false);
        Func<byte[], string[], byte[]> unprotectThunk = (Func<byte[], string[], byte[]>) Delegate.CreateDelegate(typeof (Func<byte[], string[], byte[]>), typeof (MachineKey), "Unprotect", false, false);
        if (protectThunk != null && unprotectThunk != null)
          return new MachineKey45CryptoSystem(protectThunk, unprotectThunk);
      }
      return (MachineKey45CryptoSystem) null;
    }

    public string Protect(byte[] data)
    {
      return HttpServerUtility.UrlTokenEncode(this._protectThunk(data, MachineKey45CryptoSystem._purposes));
    }

    public byte[] Unprotect(string protectedData)
    {
      return this._unprotectThunk(HttpServerUtility.UrlTokenDecode(protectedData), MachineKey45CryptoSystem._purposes);
    }
  }
}
