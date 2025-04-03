// Decompiled with JetBrains decompiler
// Type: AutoMapper.Internal.PlatformAdapter
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;

#nullable disable
namespace AutoMapper.Internal
{
  public static class PlatformAdapter
  {
    private static readonly string[] KnownPlatformNames = new string[6]
    {
      "Net4",
      "WinRT",
      "SL5",
      "WP8",
      "Android",
      "iOS"
    };
    private static IAdapterResolver _resolver = (IAdapterResolver) new ProbingAdapterResolver(PlatformAdapter.KnownPlatformNames);

    public static T Resolve<T>(bool throwIfNotFound = true)
    {
      T obj = (T) PlatformAdapter._resolver.Resolve(typeof (T));
      return (object) obj != null || !throwIfNotFound ? obj : throw new PlatformNotSupportedException("This type is not supported on this platform " + typeof (T).Name);
    }

    internal static void SetResolver(IAdapterResolver resolver)
    {
      PlatformAdapter._resolver = resolver;
    }
  }
}
