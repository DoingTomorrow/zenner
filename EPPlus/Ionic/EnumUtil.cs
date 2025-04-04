// Decompiled with JetBrains decompiler
// Type: Ionic.EnumUtil
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Ionic
{
  internal sealed class EnumUtil
  {
    private EnumUtil()
    {
    }

    internal static string GetDescription(Enum value)
    {
      DescriptionAttribute[] customAttributes = (DescriptionAttribute[]) value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof (DescriptionAttribute), false);
      return customAttributes.Length > 0 ? customAttributes[0].Description : value.ToString();
    }

    internal static object Parse(Type enumType, string stringRepresentation)
    {
      return EnumUtil.Parse(enumType, stringRepresentation, false);
    }

    internal static object Parse(Type enumType, string stringRepresentation, bool ignoreCase)
    {
      if (ignoreCase)
        stringRepresentation = stringRepresentation.ToLower();
      foreach (Enum @enum in Enum.GetValues(enumType))
      {
        string str = EnumUtil.GetDescription(@enum);
        if (ignoreCase)
          str = str.ToLower();
        if (str == stringRepresentation)
          return (object) @enum;
      }
      return Enum.Parse(enumType, stringRepresentation, ignoreCase);
    }
  }
}
