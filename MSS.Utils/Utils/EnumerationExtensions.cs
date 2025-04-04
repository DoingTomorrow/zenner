// Decompiled with JetBrains decompiler
// Type: MSS.Utils.Utils.EnumerationExtensions
// Assembly: MSS.Utils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8365EDE-890D-4A42-AEA4-3B8FCE5E7B93
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Utils.dll

using System;

#nullable disable
namespace MSS.Utils.Utils
{
  public static class EnumerationExtensions
  {
    public static string GetStringName(this Enum value)
    {
      return Enum.GetName(value.GetType(), (object) value);
    }

    public static string GetStringValue(this Enum value)
    {
      StringEnumAttribute[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof (StringEnumAttribute), false) as StringEnumAttribute[];
      return customAttributes.Length != 0 ? customAttributes[0].Value : (string) null;
    }
  }
}
