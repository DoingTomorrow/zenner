// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Config
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace ZENNER.CommonLibrary
{
  public static class Config
  {
    public static void SetParameter(
      SortedList<string, string> configList,
      ParameterKey key,
      object value)
    {
      if (configList == null)
        configList = new SortedList<string, string>();
      string empty = string.Empty;
      if (value != null)
        empty = value.ToString();
      string key1 = key.ToString();
      if (configList.IndexOfKey(key1) >= 0)
        configList[key1] = empty;
      else
        configList.Add(key1, empty);
    }

    public static T GetParameter<T>(SortedList<string, string> configList, ParameterKey key)
    {
      return Config.GetParameter<T>(configList, key, default (T));
    }

    public static T GetParameter<T>(
      SortedList<string, string> configList,
      ParameterKey key,
      T defaultValue)
    {
      int index = configList != null ? configList.IndexOfKey(key.ToString()) : throw new ArgumentNullException(nameof (configList));
      if (index < 0)
        return defaultValue;
      Type type1 = Nullable.GetUnderlyingType(typeof (T));
      if ((object) type1 == null)
        type1 = typeof (T);
      Type type2 = type1;
      string str = configList.Values[index];
      if (key == ParameterKey.ScanStartSerialnumber)
        str = Convert.ToUInt32(str.Insert(0, "0x"), 16).ToString();
      return type2.IsEnum ? (T) Enum.Parse(type2, str) : (T) Convert.ChangeType((object) str, type2);
    }
  }
}
