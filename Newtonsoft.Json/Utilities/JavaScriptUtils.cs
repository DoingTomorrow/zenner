﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.JavaScriptUtils
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace Newtonsoft.Json.Utilities
{
  internal static class JavaScriptUtils
  {
    internal static readonly bool[] SingleQuoteCharEscapeFlags = new bool[128];
    internal static readonly bool[] DoubleQuoteCharEscapeFlags = new bool[128];
    internal static readonly bool[] HtmlCharEscapeFlags = new bool[128];
    private const int UnicodeTextLength = 6;
    private const string EscapedUnicodeText = "!";

    static JavaScriptUtils()
    {
      IList<char> first = (IList<char>) new List<char>()
      {
        '\n',
        '\r',
        '\t',
        '\\',
        '\f',
        '\b'
      };
      for (int index = 0; index < 32; ++index)
        first.Add((char) index);
      foreach (char index in first.Union<char>((IEnumerable<char>) new char[1]
      {
        '\''
      }))
        JavaScriptUtils.SingleQuoteCharEscapeFlags[(int) index] = true;
      foreach (char index in first.Union<char>((IEnumerable<char>) new char[1]
      {
        '"'
      }))
        JavaScriptUtils.DoubleQuoteCharEscapeFlags[(int) index] = true;
      foreach (char index in first.Union<char>((IEnumerable<char>) new char[5]
      {
        '"',
        '\'',
        '<',
        '>',
        '&'
      }))
        JavaScriptUtils.HtmlCharEscapeFlags[(int) index] = true;
    }

    public static bool[] GetCharEscapeFlags(
      StringEscapeHandling stringEscapeHandling,
      char quoteChar)
    {
      if (stringEscapeHandling == StringEscapeHandling.EscapeHtml)
        return JavaScriptUtils.HtmlCharEscapeFlags;
      return quoteChar == '"' ? JavaScriptUtils.DoubleQuoteCharEscapeFlags : JavaScriptUtils.SingleQuoteCharEscapeFlags;
    }

    public static bool ShouldEscapeJavaScriptString(string s, bool[] charEscapeFlags)
    {
      if (s == null)
        return false;
      foreach (char index in s)
      {
        if ((int) index >= charEscapeFlags.Length || charEscapeFlags[(int) index])
          return true;
      }
      return false;
    }

    public static void WriteEscapedJavaScriptString(
      TextWriter writer,
      string s,
      char delimiter,
      bool appendDelimiters,
      bool[] charEscapeFlags,
      StringEscapeHandling stringEscapeHandling,
      IArrayPool<char> bufferPool,
      ref char[] writeBuffer)
    {
      if (appendDelimiters)
        writer.Write(delimiter);
      if (s != null)
      {
        int sourceIndex = 0;
        for (int index = 0; index < s.Length; ++index)
        {
          char c = s[index];
          if ((int) c >= charEscapeFlags.Length || charEscapeFlags[(int) c])
          {
            string a;
            switch (c)
            {
              case '\b':
                a = "\\b";
                break;
              case '\t':
                a = "\\t";
                break;
              case '\n':
                a = "\\n";
                break;
              case '\f':
                a = "\\f";
                break;
              case '\r':
                a = "\\r";
                break;
              case '\\':
                a = "\\\\";
                break;
              case '\u0085':
                a = "\\u0085";
                break;
              case '\u2028':
                a = "\\u2028";
                break;
              case '\u2029':
                a = "\\u2029";
                break;
              default:
                if ((int) c < charEscapeFlags.Length || stringEscapeHandling == StringEscapeHandling.EscapeNonAscii)
                {
                  if (c == '\'' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
                  {
                    a = "\\'";
                    break;
                  }
                  if (c == '"' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
                  {
                    a = "\\\"";
                    break;
                  }
                  if (writeBuffer == null || writeBuffer.Length < 6)
                    writeBuffer = BufferUtils.EnsureBufferSize(bufferPool, 6, writeBuffer);
                  StringUtils.ToCharAsUnicode(c, writeBuffer);
                  a = "!";
                  break;
                }
                a = (string) null;
                break;
            }
            if (a != null)
            {
              bool flag = string.Equals(a, "!");
              if (index > sourceIndex)
              {
                int minSize = index - sourceIndex + (flag ? 6 : 0);
                int num = flag ? 6 : 0;
                if (writeBuffer == null || writeBuffer.Length < minSize)
                {
                  char[] destinationArray = BufferUtils.RentBuffer(bufferPool, minSize);
                  if (flag)
                    Array.Copy((Array) writeBuffer, (Array) destinationArray, 6);
                  BufferUtils.ReturnBuffer(bufferPool, writeBuffer);
                  writeBuffer = destinationArray;
                }
                s.CopyTo(sourceIndex, writeBuffer, num, minSize - num);
                writer.Write(writeBuffer, num, minSize - num);
              }
              sourceIndex = index + 1;
              if (!flag)
                writer.Write(a);
              else
                writer.Write(writeBuffer, 0, 6);
            }
          }
        }
        if (sourceIndex == 0)
        {
          writer.Write(s);
        }
        else
        {
          int num = s.Length - sourceIndex;
          if (writeBuffer == null || writeBuffer.Length < num)
            writeBuffer = BufferUtils.EnsureBufferSize(bufferPool, num, writeBuffer);
          s.CopyTo(sourceIndex, writeBuffer, 0, num);
          writer.Write(writeBuffer, 0, num);
        }
      }
      if (!appendDelimiters)
        return;
      writer.Write(delimiter);
    }

    public static string ToEscapedJavaScriptString(
      string value,
      char delimiter,
      bool appendDelimiters,
      StringEscapeHandling stringEscapeHandling)
    {
      bool[] charEscapeFlags = JavaScriptUtils.GetCharEscapeFlags(stringEscapeHandling, delimiter);
      using (StringWriter stringWriter = StringUtils.CreateStringWriter(value != null ? value.Length : 16))
      {
        char[] writeBuffer = (char[]) null;
        JavaScriptUtils.WriteEscapedJavaScriptString((TextWriter) stringWriter, value, delimiter, appendDelimiters, charEscapeFlags, stringEscapeHandling, (IArrayPool<char>) null, ref writeBuffer);
        return stringWriter.ToString();
      }
    }
  }
}
