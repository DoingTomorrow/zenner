// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.EnumHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Utils.Utils;
using System;
using System.Collections.Generic;

#nullable disable
namespace MSS.Business.Utils
{
  public static class EnumHelper
  {
    public static Dictionary<string, string> GetEnumElements<T>()
    {
      Dictionary<string, string> enumElements = new Dictionary<string, string>();
      foreach (Enum @enum in Enum.GetValues(typeof (T)))
      {
        string stringValue = @enum.GetStringValue();
        string key = @enum.ToString();
        enumElements.Add(key, stringValue);
      }
      return enumElements;
    }

    public static Dictionary<T, string> GetEnumTranslationsDictionary<T>()
    {
      Dictionary<T, string> translationsDictionary = new Dictionary<T, string>();
      foreach (Enum enum1 in Enum.GetValues(typeof (T)))
      {
        string stringValue = enum1.GetStringValue();
        Enum enum2 = enum1;
        translationsDictionary.Add((T) Enum.Parse(typeof (T), enum2.ToString()), stringValue);
      }
      return translationsDictionary;
    }
  }
}
