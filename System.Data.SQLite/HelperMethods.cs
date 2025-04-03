// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.HelperMethods
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  internal static class HelperMethods
  {
    private static readonly object staticSyncRoot = new object();
    private static readonly string MonoRuntimeType = "Mono.Runtime";
    private static bool? isMono = new bool?();

    private static bool IsMono()
    {
      try
      {
        lock (HelperMethods.staticSyncRoot)
        {
          if (!HelperMethods.isMono.HasValue)
            HelperMethods.isMono = new bool?(Type.GetType(HelperMethods.MonoRuntimeType) != (Type) null);
          return HelperMethods.isMono.Value;
        }
      }
      catch
      {
      }
      return false;
    }

    internal static bool IsWindows()
    {
      switch (Environment.OSVersion.Platform)
      {
        case PlatformID.Win32S:
        case PlatformID.Win32Windows:
        case PlatformID.Win32NT:
        case PlatformID.WinCE:
          return true;
        default:
          return false;
      }
    }

    internal static string StringFormat(
      IFormatProvider provider,
      string format,
      params object[] args)
    {
      return HelperMethods.IsMono() ? string.Format(format, args) : string.Format(provider, format, args);
    }
  }
}
