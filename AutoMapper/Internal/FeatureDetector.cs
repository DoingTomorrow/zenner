// Decompiled with JetBrains decompiler
// Type: AutoMapper.Internal.FeatureDetector
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;

#nullable disable
namespace AutoMapper.Internal
{
  public static class FeatureDetector
  {
    public static Func<Type, bool> IsIDataRecordType = (Func<Type, bool>) (t => false);
    private static bool? _isEnumGetNamesSupported;

    public static bool IsEnumGetNamesSupported
    {
      get
      {
        if (!FeatureDetector._isEnumGetNamesSupported.HasValue)
          FeatureDetector._isEnumGetNamesSupported = new bool?(FeatureDetector.ResolveIsEnumGetNamesSupported());
        return FeatureDetector._isEnumGetNamesSupported.Value;
      }
    }

    private static bool ResolveIsEnumGetNamesSupported()
    {
      return (object) typeof (Enum).GetMethod("GetNames") != null;
    }
  }
}
