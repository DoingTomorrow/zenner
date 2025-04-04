// Decompiled with JetBrains decompiler
// Type: NLog.Internal.StringSplitter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NLog.Internal
{
  internal static class StringSplitter
  {
    public static IEnumerable<string> SplitWithSelfEscape(this string text, char splitChar)
    {
      return StringSplitter.SplitWithSelfEscape2(text, splitChar);
    }

    public static IEnumerable<string> SplitWithEscape(
      this string text,
      char splitChar,
      char escapeChar)
    {
      return (int) splitChar == (int) escapeChar ? StringSplitter.SplitWithSelfEscape2(text, splitChar) : StringSplitter.SplitWithEscape2(text, splitChar, escapeChar);
    }

    private static IEnumerable<string> SplitWithEscape2(
      string text,
      char splitChar,
      char escapeChar)
    {
      if (!string.IsNullOrEmpty(text))
      {
        bool prevWasEscape = false;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < text.Length; ++i)
        {
          char c = text[i];
          bool flag = (int) c == (int) splitChar;
          if (prevWasEscape)
          {
            if (flag)
            {
              if (sb.Length > 0)
                --sb.Length;
              sb.Append(c);
              prevWasEscape = false;
            }
            else
            {
              sb.Append(c);
              prevWasEscape = (int) c == (int) escapeChar;
            }
          }
          else if (flag)
          {
            string str = sb.ToString();
            sb.Length = 0;
            yield return str;
            if (text.Length - 1 == i)
            {
              yield return string.Empty;
              break;
            }
          }
          else
          {
            sb.Append(c);
            prevWasEscape = (int) c == (int) escapeChar;
          }
        }
        string lastPart = StringSplitter.GetLastPart(sb);
        if (lastPart != null)
          yield return lastPart;
        sb = (StringBuilder) null;
      }
    }

    private static IEnumerable<string> SplitWithSelfEscape2(string text, char splitAndEscapeChar)
    {
      if (!string.IsNullOrEmpty(text))
      {
        bool prevWasEscape = false;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < text.Length; ++i)
        {
          char c = text[i];
          bool isSplitAndEscape = (int) c == (int) splitAndEscapeChar;
          bool isLastChar = i == text.Length - 1;
          if (prevWasEscape)
          {
            if (isSplitAndEscape)
            {
              prevWasEscape = false;
            }
            else
            {
              --sb.Length;
              string str = sb.ToString();
              sb.Length = 0;
              prevWasEscape = false;
              sb.Append(c);
              yield return str;
            }
          }
          else if (isLastChar & isSplitAndEscape)
          {
            string str = sb.ToString();
            sb.Length = 0;
            yield return str;
            yield return string.Empty;
          }
          else
          {
            sb.Append(c);
            if (isSplitAndEscape)
              prevWasEscape = true;
          }
        }
        string lastPart = StringSplitter.GetLastPart(sb);
        if (lastPart != null)
          yield return lastPart;
        sb = (StringBuilder) null;
      }
    }

    public static IEnumerable<string> SplitQuoted(
      this string text,
      char splitChar,
      char quoteChar,
      char escapeChar)
    {
      if (string.IsNullOrEmpty(text))
        return (IEnumerable<string>) new List<string>();
      if ((int) splitChar == (int) quoteChar)
        throw new NotSupportedException("Quote character should different from split character");
      if ((int) splitChar == (int) escapeChar)
        throw new NotSupportedException("Escape character should different from split character");
      return (int) quoteChar == (int) escapeChar ? StringSplitter.SplitSelfQuoted2(text, splitChar, quoteChar) : StringSplitter.SplitQuoted2(text, splitChar, quoteChar, escapeChar);
    }

    private static IEnumerable<string> SplitSelfQuoted2(
      string text,
      char splitChar,
      char quoteAndEscapeChar)
    {
      bool inQuotedMode = false;
      StringBuilder sb = new StringBuilder();
      bool isNewPart = true;
      for (int i = 0; i < text.Length; ++i)
      {
        char c = text[i];
        bool isSplitChar = (int) c == (int) splitChar;
        bool isQuoteAndEscapeChar = (int) c == (int) quoteAndEscapeChar;
        bool isLastChar = i == text.Length - 1;
        if (isNewPart)
        {
          isNewPart = false;
          isQuoteAndEscapeChar = (int) c == (int) quoteAndEscapeChar;
          if (isQuoteAndEscapeChar)
          {
            if (isLastChar)
            {
              sb.Append(c);
              break;
            }
            ++i;
            c = text[i];
            if ((int) c == (int) quoteAndEscapeChar)
            {
              sb.Append(quoteAndEscapeChar);
            }
            else
            {
              sb.Append(c);
              inQuotedMode = true;
            }
          }
          else if (isSplitChar)
          {
            string str = sb.ToString();
            sb.Length = 0;
            yield return str;
            if (isLastChar)
            {
              yield return string.Empty;
              break;
            }
            isNewPart = true;
          }
          else
            sb.Append(c);
        }
        else if (inQuotedMode)
        {
          if (isQuoteAndEscapeChar)
          {
            ++i;
            inQuotedMode = false;
            string str = sb.ToString();
            sb.Length = 0;
            yield return str;
          }
          else
            sb.Append(c);
        }
        else if (isSplitChar)
        {
          string str = sb.ToString();
          sb.Length = 0;
          yield return str;
          if (isLastChar)
          {
            yield return string.Empty;
            break;
          }
          isNewPart = true;
        }
        else
          sb.Append(c);
      }
      string str1 = StringSplitter.GetLastPart(sb);
      if (inQuotedMode)
        str1 = quoteAndEscapeChar.ToString() + str1;
      if (str1 != null)
        yield return str1;
    }

    private static IEnumerable<string> SplitQuoted2(
      string text,
      char splitChar,
      char quoteChar,
      char escapeChar)
    {
      bool inQuotedMode = false;
      StringBuilder sb = new StringBuilder();
      bool isNewPart = true;
      bool prevIsEscape = false;
      for (int i = 0; i < text.Length; ++i)
      {
        char c = text[i];
        bool isSplitChar = (int) c == (int) splitChar;
        bool isQuoteChar = (int) c == (int) quoteChar;
        bool isEscapeChar = (int) c == (int) escapeChar;
        bool isLastChar = i == text.Length - 1;
        if (isNewPart)
        {
          isNewPart = false;
          isQuoteChar = (int) c == (int) quoteChar;
          isEscapeChar = (int) c == (int) escapeChar;
          if (isEscapeChar)
          {
            if (isLastChar)
            {
              sb.Append(c);
              break;
            }
            ++i;
            c = text[i];
            if ((int) c == (int) quoteChar)
            {
              sb.Append(quoteChar);
            }
            else
            {
              sb.Append(escapeChar);
              sb.Append(c);
            }
          }
          else if (isSplitChar)
          {
            string str = sb.ToString();
            sb.Length = 0;
            yield return str;
            if (isLastChar)
            {
              yield return string.Empty;
              break;
            }
            isNewPart = true;
          }
          else if (isQuoteChar)
          {
            if (sb.Length > 0)
              --sb.Length;
            inQuotedMode = true;
          }
          else
            sb.Append(c);
        }
        else if (inQuotedMode)
        {
          if (isQuoteChar)
          {
            if (prevIsEscape)
            {
              if (sb.Length > 0)
                --sb.Length;
              sb.Append(c);
              break;
            }
            ++i;
            inQuotedMode = false;
            string str = sb.ToString();
            sb.Length = 0;
            yield return str;
          }
          else
          {
            prevIsEscape = isEscapeChar;
            sb.Append(c);
          }
        }
        else if (isSplitChar)
        {
          string str = sb.ToString();
          sb.Length = 0;
          yield return str;
          if (isLastChar)
          {
            yield return string.Empty;
            break;
          }
          isNewPart = true;
        }
        else
          sb.Append(c);
      }
      string str1 = StringSplitter.GetLastPart(sb);
      if (inQuotedMode)
        str1 = quoteChar.ToString() + str1;
      if (str1 != null)
        yield return str1;
    }

    private static string GetLastPart(StringBuilder sb)
    {
      return sb.Length > 0 ? sb.ToString() : (string) null;
    }
  }
}
