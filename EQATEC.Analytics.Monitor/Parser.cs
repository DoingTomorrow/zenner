// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Parser
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal static class Parser
  {
    internal static bool TryParseUint(string input, out uint value)
    {
      if (!string.IsNullOrEmpty(input))
        return uint.TryParse(input, out value);
      value = 0U;
      return false;
    }

    internal static bool TryParseLong(string input, out long value)
    {
      if (!string.IsNullOrEmpty(input))
        return long.TryParse(input, out value);
      value = 0L;
      return false;
    }

    internal static bool TryParseEnum<TEnum>(string input, out TEnum value)
    {
      if (string.IsNullOrEmpty(input))
      {
        value = default (TEnum);
        return false;
      }
      try
      {
        value = (TEnum) Enum.Parse(typeof (TEnum), input, true);
        return true;
      }
      catch
      {
        value = default (TEnum);
        return false;
      }
    }

    public static bool TryParseInt(string input, out int value)
    {
      if (!string.IsNullOrEmpty(input))
        return int.TryParse(input, out value);
      value = 0;
      return false;
    }

    public static bool TryParseGuid(string input, out Guid value)
    {
      if (string.IsNullOrEmpty(input))
      {
        value = Guid.Empty;
        return false;
      }
      try
      {
        value = new Guid(input);
        return true;
      }
      catch
      {
        value = Guid.Empty;
        return false;
      }
    }

    public static Version TryParseVersion(string versionValue)
    {
      if (string.IsNullOrEmpty(versionValue))
        return new Version(0, 0, 0);
      try
      {
        return new Version(versionValue);
      }
      catch
      {
        return new Version(0, 0, 0);
      }
    }
  }
}
