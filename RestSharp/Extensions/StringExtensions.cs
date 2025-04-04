// Decompiled with JetBrains decompiler
// Type: RestSharp.Extensions.StringExtensions
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using RestSharp.Contrib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace RestSharp.Extensions
{
  public static class StringExtensions
  {
    public static string UrlDecode(this string input) => HttpUtility.UrlDecode(input);

    public static string UrlEncode(this string input) => Uri.EscapeDataString(input);

    public static string HtmlDecode(this string input) => HttpUtility.HtmlDecode(input);

    public static string HtmlEncode(this string input) => HttpUtility.HtmlEncode(input);

    public static string HtmlAttributeEncode(this string input)
    {
      return HttpUtility.HtmlAttributeEncode(input);
    }

    public static bool HasValue(this string input) => !string.IsNullOrEmpty(input);

    public static string RemoveUnderscoresAndDashes(this string input)
    {
      return input.Replace("_", "").Replace("-", "");
    }

    public static DateTime ParseJsonDate(this string input, CultureInfo culture)
    {
      input = input.Replace("\n", "");
      input = input.Replace("\r", "");
      input = input.RemoveSurroundingQuotes();
      long result;
      if (long.TryParse(input, out result))
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((double) result);
      if (input.Contains("/Date("))
        return StringExtensions.ExtractDate(input, "\\\\?/Date\\((-?\\d+)(-|\\+)?([0-9]{4})?\\)\\\\?/", culture);
      if (!input.Contains("new Date("))
        return StringExtensions.ParseFormattedDate(input, culture);
      input = input.Replace(" ", "");
      return StringExtensions.ExtractDate(input, "newDate\\((-?\\d+)*\\)", culture);
    }

    public static string RemoveSurroundingQuotes(this string input)
    {
      if (input.StartsWith("\"") && input.EndsWith("\""))
        input = input.Substring(1, input.Length - 2);
      return input;
    }

    private static DateTime ParseFormattedDate(string input, CultureInfo culture)
    {
      string[] formats = new string[8]
      {
        "u",
        "s",
        "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'",
        "yyyy-MM-ddTHH:mm:ssZ",
        "yyyy-MM-dd HH:mm:ssZ",
        "yyyy-MM-ddTHH:mm:ss",
        "yyyy-MM-ddTHH:mm:sszzzzzz",
        "M/d/yyyy h:mm:ss tt"
      };
      DateTime result;
      return DateTime.TryParseExact(input, formats, (IFormatProvider) culture, DateTimeStyles.None, out result) || DateTime.TryParse(input, (IFormatProvider) culture, DateTimeStyles.None, out result) ? result : new DateTime();
    }

    private static DateTime ExtractDate(string input, string pattern, CultureInfo culture)
    {
      DateTime date = DateTime.MinValue;
      Regex regex = new Regex(pattern);
      if (regex.IsMatch(input))
      {
        Match match = regex.Matches(input)[0];
        date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((double) Convert.ToInt64(match.Groups[1].Value));
        if (match.Groups.Count > 2 && !string.IsNullOrEmpty(match.Groups[3].Value))
        {
          DateTime exact = DateTime.ParseExact(match.Groups[3].Value, "HHmm", (IFormatProvider) culture);
          date = !(match.Groups[2].Value == "+") ? date.Subtract(exact.TimeOfDay) : date.Add(exact.TimeOfDay);
        }
      }
      return date;
    }

    public static bool Matches(this string input, string pattern) => Regex.IsMatch(input, pattern);

    public static string ToPascalCase(this string lowercaseAndUnderscoredWord, CultureInfo culture)
    {
      return lowercaseAndUnderscoredWord.ToPascalCase(true, culture);
    }

    public static string ToPascalCase(
      this string text,
      bool removeUnderscores,
      CultureInfo culture)
    {
      if (string.IsNullOrEmpty(text))
        return text;
      text = text.Replace("_", " ");
      string separator = removeUnderscores ? string.Empty : "_";
      string[] strArray = text.Split(' ');
      if (strArray.Length <= 1 && !strArray[0].IsUpperCase())
        return strArray[0].Substring(0, 1).ToUpper(culture) + strArray[0].Substring(1);
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index].Length > 0)
        {
          string str = strArray[index];
          string inputString = str.Substring(1);
          if (inputString.IsUpperCase())
            inputString = inputString.ToLower(culture);
          char upper = char.ToUpper(str[0], culture);
          strArray[index] = upper.ToString() + inputString;
        }
      }
      return string.Join(separator, strArray);
    }

    public static string ToCamelCase(this string lowercaseAndUnderscoredWord, CultureInfo culture)
    {
      return lowercaseAndUnderscoredWord.ToPascalCase(culture).MakeInitialLowerCase();
    }

    public static string MakeInitialLowerCase(this string word)
    {
      return word.Substring(0, 1).ToLower() + word.Substring(1);
    }

    public static bool IsUpperCase(this string inputString)
    {
      return Regex.IsMatch(inputString, "^[A-Z]+$");
    }

    public static string AddUnderscores(this string pascalCasedWord)
    {
      return Regex.Replace(Regex.Replace(Regex.Replace(pascalCasedWord, "([A-Z]+)([A-Z][a-z])", "$1_$2"), "([a-z\\d])([A-Z])", "$1_$2"), "[-\\s]", "_");
    }

    public static string AddDashes(this string pascalCasedWord)
    {
      return Regex.Replace(Regex.Replace(Regex.Replace(pascalCasedWord, "([A-Z]+)([A-Z][a-z])", "$1-$2"), "([a-z\\d])([A-Z])", "$1-$2"), "[\\s]", "-");
    }

    public static string AddUnderscorePrefix(this string pascalCasedWord)
    {
      return string.Format("_{0}", (object) pascalCasedWord);
    }

    public static IEnumerable<string> GetNameVariants(this string name, CultureInfo culture)
    {
      if (!string.IsNullOrEmpty(name))
      {
        yield return name;
        yield return name.ToCamelCase(culture);
        yield return name.ToLower(culture);
        yield return name.AddUnderscores();
        yield return name.AddUnderscores().ToLower(culture);
        yield return name.AddDashes();
        yield return name.AddDashes().ToLower(culture);
        yield return name.AddUnderscorePrefix();
        yield return name.ToCamelCase(culture).AddUnderscorePrefix();
      }
    }
  }
}
