// Decompiled with JetBrains decompiler
// Type: RestSharp.Authenticators.OAuth.Extensions.StringExtensions
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace RestSharp.Authenticators.OAuth.Extensions
{
  internal static class StringExtensions
  {
    private const RegexOptions Options = RegexOptions.IgnoreCase | RegexOptions.Compiled;

    public static bool IsNullOrBlank(this string value)
    {
      if (string.IsNullOrEmpty(value))
        return true;
      return !string.IsNullOrEmpty(value) && value.Trim() == string.Empty;
    }

    public static bool EqualsIgnoreCase(this string left, string right)
    {
      return string.Compare(left, right, StringComparison.OrdinalIgnoreCase) == 0;
    }

    public static bool EqualsAny(this string input, params string[] args)
    {
      return ((IEnumerable<string>) args).Aggregate<string, bool>(false, (Func<bool, string, bool>) ((current, arg) => current | input.Equals(arg)));
    }

    public static string FormatWith(this string format, params object[] args)
    {
      return string.Format(format, args);
    }

    public static string FormatWithInvariantCulture(this string format, params object[] args)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, format, args);
    }

    public static string Then(this string input, string value) => input + value;

    public static string UrlEncode(this string value) => Uri.EscapeDataString(value);

    public static string UrlDecode(this string value) => Uri.UnescapeDataString(value);

    public static Uri AsUri(this string value) => new Uri(value);

    public static string ToBase64String(this byte[] input) => Convert.ToBase64String(input);

    public static byte[] GetBytes(this string input) => Encoding.UTF8.GetBytes(input);

    public static string PercentEncode(this string s)
    {
      byte[] bytes = s.GetBytes();
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte num in bytes)
      {
        if (num > (byte) 7 && num < (byte) 11 || num == (byte) 13)
          stringBuilder.Append(string.Format("%0{0:X}", (object) num));
        else
          stringBuilder.Append(string.Format("%{0:X}", (object) num));
      }
      return stringBuilder.ToString();
    }

    public static IDictionary<string, string> ParseQueryString(this string query)
    {
      if (query.StartsWith("?"))
        query = query.Substring(1);
      if (query.Equals(string.Empty))
        return (IDictionary<string, string>) new Dictionary<string, string>();
      return (IDictionary<string, string>) ((IEnumerable<string>) query.Split('&')).Select<string, string[]>((Func<string, string[]>) (part => part.Split('='))).ToDictionary<string[], string, string>((Func<string[], string>) (pair => pair[0]), (Func<string[], string>) (pair => pair[1]));
    }
  }
}
