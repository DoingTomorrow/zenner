// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.StringUtil
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal static class StringUtil
  {
    internal static readonly string NewLine = Environment.NewLine;
    internal static readonly string MiddleString = "...." + StringUtil.NewLine;
    private static readonly char[] s_InvalidChars = new List<char>((IEnumerable<char>) Path.GetInvalidPathChars())
    {
      ':'
    }.ToArray();

    internal static string Chop(string input, int length)
    {
      if (input == null || length < 0)
        return "";
      return input.Length > length ? input.Substring(0, length) : input;
    }

    internal static string ChopToEnds(string input, int length)
    {
      if (input == null || length < 0)
        return "";
      if (input.Length <= length)
        return input;
      string[] strArray = StringUtil.Split(input);
      if (strArray.Length >= 3)
      {
        int length1 = StringUtil.MiddleString.Length;
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        for (int index1 = 0; index1 < strArray.Length; ++index1)
        {
          int index2 = strArray.Length - 1 - index1;
          if (index1 < index2)
          {
            string str1 = strArray[index1] + StringUtil.NewLine;
            if (stringBuilder1.Length + stringBuilder2.Length + str1.Length + length1 < length)
            {
              stringBuilder1.Append(str1);
              string str2 = strArray[index2] + StringUtil.NewLine;
              if (stringBuilder1.Length + stringBuilder2.Length + str2.Length + length1 < length)
                stringBuilder2.Insert(0, str2);
              else
                break;
            }
            else
              break;
          }
          else
            break;
        }
        if (stringBuilder1.Length != 0 && stringBuilder2.Length != 0)
          return stringBuilder1.ToString() + StringUtil.MiddleString + stringBuilder2.ToString();
      }
      return input.Substring(0, length);
    }

    private static string[] Split(string input)
    {
      return input.Split(new string[1]{ StringUtil.NewLine }, StringSplitOptions.RemoveEmptyEntries);
    }

    internal static string[] SplitString(string input, string splitString)
    {
      if (string.IsNullOrEmpty(input))
        return new string[0];
      List<string> stringList = new List<string>();
      int startIndex = 0;
      int num;
      for (; startIndex < input.Length && (num = input.IndexOf(splitString, startIndex)) != -1; startIndex = num + splitString.Length)
      {
        string str = input.Substring(startIndex, num - startIndex);
        if (!string.IsNullOrEmpty(str))
          stringList.Add(str);
      }
      if (startIndex < input.Length)
      {
        string str = input.Substring(startIndex);
        if (!string.IsNullOrEmpty(str))
          stringList.Add(str);
      }
      return stringList.ToArray();
    }

    internal static string ToValidFileName(string path) => StringUtil.ToValidFileName(path, '_');

    internal static string ToValidFileName(string path, char replaceChar)
    {
      foreach (char invalidChar in StringUtil.s_InvalidChars)
        path = path.Replace(invalidChar, replaceChar);
      return path;
    }

    internal static string ToList<TType>(
      IEnumerable<TType> list,
      StringUtil.StringFunc<TType> stringFunc,
      string separator)
    {
      if (list == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (TType input in list)
      {
        if (stringBuilder.Length != 0)
          stringBuilder.Append(separator ?? "");
        stringBuilder.Append(stringFunc == null ? input.ToString() : stringFunc(input));
      }
      return stringBuilder.ToString();
    }

    public static string GetStringExtract(string input, int maxLength)
    {
      if (string.IsNullOrEmpty(input) || maxLength <= 0)
        return "";
      if (maxLength >= input.Length)
        return input;
      int length = maxLength / 2;
      return input.Substring(0, length) + "..." + input.Substring(input.Length - length);
    }

    internal delegate string StringFunc<TType>(TType input);
  }
}
