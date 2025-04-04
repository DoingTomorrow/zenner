// Decompiled with JetBrains decompiler
// Type: NLog.Internal.EnumHelpers
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;

#nullable disable
namespace NLog.Internal
{
  internal static class EnumHelpers
  {
    public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct
    {
      return EnumHelpers.TryParse<TEnum>(value, false, out result);
    }

    public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct
    {
      return Enum.TryParse<TEnum>(value, ignoreCase, out result);
    }

    private static bool TryParseEnum_net3<TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct
    {
      Type type = typeof (TEnum);
      if (!type.IsEnum())
        throw new ArgumentException(string.Format("Type '{0}' is not an enum", (object) type.FullName));
      if (StringHelpers.IsNullOrWhiteSpace(value))
      {
        result = default (TEnum);
        return false;
      }
      try
      {
        result = (TEnum) Enum.Parse(type, value, ignoreCase);
        return true;
      }
      catch (Exception ex)
      {
        result = default (TEnum);
        return false;
      }
    }
  }
}
