// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.PropertiesHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Util
{
  public static class PropertiesHelper
  {
    public static bool GetBoolean(
      string property,
      IDictionary<string, string> properties,
      bool defaultValue)
    {
      string str;
      properties.TryGetValue(property, out str);
      bool result;
      return !bool.TryParse(str, out result) ? defaultValue : result;
    }

    public static bool GetBoolean(string property, IDictionary<string, string> properties)
    {
      return PropertiesHelper.GetBoolean(property, properties, false);
    }

    public static int GetInt32(
      string property,
      IDictionary<string, string> properties,
      int defaultValue)
    {
      string s;
      properties.TryGetValue(property, out s);
      int result;
      return !int.TryParse(s, out result) ? defaultValue : result;
    }

    public static long GetInt64(
      string property,
      IDictionary<string, string> properties,
      long defaultValue)
    {
      string s;
      properties.TryGetValue(property, out s);
      long result;
      return !long.TryParse(s, out result) ? defaultValue : result;
    }

    public static string GetString(
      string property,
      IDictionary<string, string> properties,
      string defaultValue)
    {
      string str;
      properties.TryGetValue(property, out str);
      if (str == string.Empty)
        str = (string) null;
      return str ?? defaultValue;
    }

    public static IDictionary<string, string> ToDictionary(
      string property,
      string delim,
      IDictionary<string, string> properties)
    {
      IDictionary<string, string> dictionary = (IDictionary<string, string>) new Dictionary<string, string>();
      if (properties.ContainsKey(property))
      {
        IEnumerator<string> enumerator = new StringTokenizer(properties[property], delim, false).GetEnumerator();
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          string str = enumerator.MoveNext() ? enumerator.Current : string.Empty;
          dictionary[current] = str;
        }
      }
      return dictionary;
    }

    public static string[] ToStringArray(string property, string delim, IDictionary properties)
    {
      return PropertiesHelper.ToStringArray((string) properties[(object) property], delim);
    }

    public static string[] ToStringArray(string propValue, string delim)
    {
      return propValue != null ? StringHelper.Split(delim, propValue) : new string[0];
    }
  }
}
