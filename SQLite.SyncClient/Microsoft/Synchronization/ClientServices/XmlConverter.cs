﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.XmlConverter
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Globalization;
using System.Text;
using System.Xml;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  internal static class XmlConverter
  {
    public const int MaxDateTimeChars = 64;
    public const int MaxInt32Chars = 16;
    public const int MaxInt64Chars = 32;
    public const int MaxBoolChars = 5;
    public const int MaxFloatChars = 16;
    public const int MaxDoubleChars = 32;
    public const int MaxDecimalChars = 40;
    public const int MaxUInt64Chars = 32;
    public const int MaxPrimitiveChars = 64;
    private static UTF8Encoding utf8Encoding;
    private static UnicodeEncoding unicodeEncoding;

    private static UTF8Encoding UTF8Encoding
    {
      get
      {
        if (XmlConverter.utf8Encoding == null)
          XmlConverter.utf8Encoding = new UTF8Encoding(false, true);
        return XmlConverter.utf8Encoding;
      }
    }

    private static UnicodeEncoding UnicodeEncoding
    {
      get
      {
        if (XmlConverter.unicodeEncoding == null)
          XmlConverter.unicodeEncoding = new UnicodeEncoding(false, false, true);
        return XmlConverter.unicodeEncoding;
      }
    }

    public static bool ToBoolean(string value) => XmlConvert.ToBoolean(value);

    public static bool ToBoolean(byte[] buffer, int offset, int count)
    {
      if (count == 1)
      {
        switch (buffer[offset])
        {
          case 48:
            return false;
          case 49:
            return true;
        }
      }
      return XmlConverter.ToBoolean(XmlConverter.ToString(buffer, offset, count));
    }

    public static int ToInt32(string value) => XmlConvert.ToInt32(value);

    public static int ToInt32(byte[] buffer, int offset, int count)
    {
      int result;
      return XmlConverter.TryParseInt32(buffer, offset, count, out result) ? result : XmlConverter.ToInt32(XmlConverter.ToString(buffer, offset, count));
    }

    public static long ToInt64(string value) => XmlConvert.ToInt64(value);

    public static long ToInt64(byte[] buffer, int offset, int count)
    {
      long result;
      return XmlConverter.TryParseInt64(buffer, offset, count, out result) ? result : XmlConverter.ToInt64(XmlConverter.ToString(buffer, offset, count));
    }

    public static float ToSingle(string value) => XmlConvert.ToSingle(value);

    public static float ToSingle(byte[] buffer, int offset, int count)
    {
      float result;
      return XmlConverter.TryParseSingle(buffer, offset, count, out result) ? result : XmlConverter.ToSingle(XmlConverter.ToString(buffer, offset, count));
    }

    public static double ToDouble(string value) => XmlConvert.ToDouble(value);

    public static double ToDouble(byte[] buffer, int offset, int count)
    {
      double result;
      return XmlConverter.TryParseDouble(buffer, offset, count, out result) ? result : XmlConverter.ToDouble(XmlConverter.ToString(buffer, offset, count));
    }

    public static Decimal ToDecimal(string value) => XmlConvert.ToDecimal(value);

    public static Decimal ToDecimal(byte[] buffer, int offset, int count)
    {
      return XmlConverter.ToDecimal(XmlConverter.ToString(buffer, offset, count));
    }

    public static DateTime ToDateTime(long value) => DateTime.FromBinary(value);

    public static DateTime ToDateTime(string value)
    {
      DateTime result;
      if (DateTime.TryParse(value, out result))
        return result;
      throw new ArgumentException("Cant parse String value to DateTime");
    }

    public static DateTime ToDateTime(byte[] buffer, int offset, int count)
    {
      DateTime result;
      return XmlConverter.TryParseDateTime(buffer, offset, count, out result) ? result : XmlConverter.ToDateTime(XmlConverter.ToString(buffer, offset, count));
    }

    public static UniqueId ToUniqueId(string value) => new UniqueId(XmlConverter.Trim(value));

    public static UniqueId ToUniqueId(byte[] buffer, int offset, int count)
    {
      return XmlConverter.ToUniqueId(XmlConverter.ToString(buffer, offset, count));
    }

    public static TimeSpan ToTimeSpan(string value) => XmlConvert.ToTimeSpan(value);

    public static TimeSpan ToTimeSpan(byte[] buffer, int offset, int count)
    {
      return XmlConverter.ToTimeSpan(XmlConverter.ToString(buffer, offset, count));
    }

    public static Guid ToGuid(string value) => new Guid(XmlConverter.Trim(value));

    public static Guid ToGuid(byte[] buffer, int offset, int count)
    {
      return XmlConverter.ToGuid(XmlConverter.ToString(buffer, offset, count));
    }

    public static ulong ToUInt64(string value)
    {
      return ulong.Parse(value, (IFormatProvider) NumberFormatInfo.InvariantInfo);
    }

    public static ulong ToUInt64(byte[] buffer, int offset, int count)
    {
      return XmlConverter.ToUInt64(XmlConverter.ToString(buffer, offset, count));
    }

    public static string ToString(byte[] buffer, int offset, int count)
    {
      return XmlConverter.UTF8Encoding.GetString(buffer, offset, count);
    }

    public static string ToStringUnicode(byte[] buffer, int offset, int count)
    {
      return XmlConverter.UnicodeEncoding.GetString(buffer, offset, count);
    }

    public static byte[] ToBytes(string value) => XmlConverter.UTF8Encoding.GetBytes(value);

    public static int ToChars(byte[] buffer, int offset, int count, char[] chars, int charOffset)
    {
      return XmlConverter.UTF8Encoding.GetChars(buffer, offset, count, chars, charOffset);
    }

    public static string ToString(bool value) => !value ? "false" : "true";

    public static string ToString(int value) => XmlConvert.ToString(value);

    public static string ToString(long value) => XmlConvert.ToString(value);

    public static string ToString(float value) => XmlConvert.ToString(value);

    public static string ToString(double value) => XmlConvert.ToString(value);

    public static string ToString(Decimal value) => XmlConvert.ToString(value);

    public static string ToString(TimeSpan value) => XmlConvert.ToString(value);

    public static string ToString(UniqueId value) => value.ToString();

    public static string ToString(Guid value) => value.ToString();

    public static string ToString(ulong value)
    {
      return value.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo);
    }

    public static string ToString(DateTime value)
    {
      byte[] numArray = new byte[64];
      int chars = XmlConverter.ToChars(value, numArray, 0);
      return XmlConverter.ToString(numArray, 0, chars);
    }

    private static string ToString(object value)
    {
      switch (value)
      {
        case int num1:
          return XmlConverter.ToString(num1);
        case long num2:
          return XmlConverter.ToString(num2);
        case float num3:
          return XmlConverter.ToString(num3);
        case double num4:
          return XmlConverter.ToString(num4);
        case Decimal num5:
          return XmlConverter.ToString(num5);
        case TimeSpan timeSpan:
          return XmlConverter.ToString(timeSpan);
        case UniqueId _:
          return XmlConverter.ToString((UniqueId) value);
        case Guid guid:
          return XmlConverter.ToString(guid);
        case ulong num6:
          return XmlConverter.ToString(num6);
        case DateTime dateTime:
          return XmlConverter.ToString(dateTime);
        case bool flag:
          return XmlConverter.ToString(flag);
        default:
          return value.ToString();
      }
    }

    public static string ToString(object[] objects)
    {
      if (objects.Length == 0)
        return string.Empty;
      string str = XmlConverter.ToString(objects[0]);
      if (objects.Length > 1)
      {
        StringBuilder stringBuilder = new StringBuilder(str);
        for (int index = 1; index < objects.Length; ++index)
        {
          stringBuilder.Append(' ');
          stringBuilder.Append(XmlConverter.ToString(objects[index]));
        }
        str = stringBuilder.ToString();
      }
      return str;
    }

    public static void ToQualifiedName(string qname, out string prefix, out string localName)
    {
      int length = qname.IndexOf(':');
      if (length < 0)
      {
        prefix = string.Empty;
        localName = XmlConverter.Trim(qname);
      }
      else
      {
        if (length == qname.Length - 1)
          throw new XmlException("XmlInvalidQualifiedName");
        prefix = XmlConverter.Trim(qname.Substring(0, length));
        localName = XmlConverter.Trim(qname.Substring(length + 1));
      }
    }

    private static bool TryParseInt32(byte[] chars, int offset, int count, out int result)
    {
      result = 0;
      if (count == 0)
        return false;
      int num1 = 0;
      int num2 = offset + count;
      if (chars[offset] == (byte) 45)
      {
        if (count == 1)
          return false;
        for (int index = offset + 1; index < num2; ++index)
        {
          int num3 = (int) chars[index] - 48;
          switch (num3)
          {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
              if (num1 < -214748364)
                return false;
              int num4 = num1 * 10;
              if (num4 < int.MinValue + num3)
                return false;
              num1 = num4 - num3;
              continue;
            default:
              return false;
          }
        }
      }
      else
      {
        for (int index = offset; index < num2; ++index)
        {
          int num5 = (int) chars[index] - 48;
          switch (num5)
          {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
              if (num1 > 214748364)
                return false;
              int num6 = num1 * 10;
              if (num6 > int.MaxValue - num5)
                return false;
              num1 = num6 + num5;
              continue;
            default:
              return false;
          }
        }
      }
      result = num1;
      return true;
    }

    private static bool TryParseInt64(byte[] chars, int offset, int count, out long result)
    {
      result = 0L;
      if (count < 11)
      {
        int result1;
        if (!XmlConverter.TryParseInt32(chars, offset, count, out result1))
          return false;
        result = (long) result1;
        return true;
      }
      long num1 = 0;
      int num2 = offset + count;
      if (chars[offset] == (byte) 45)
      {
        if (count == 1)
          return false;
        for (int index = offset + 1; index < num2; ++index)
        {
          int num3 = (int) chars[index] - 48;
          switch (num3)
          {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
              if (num1 < -922337203685477580L)
                return false;
              long num4 = num1 * 10L;
              if (num4 < long.MinValue + (long) num3)
                return false;
              num1 = num4 - (long) num3;
              continue;
            default:
              return false;
          }
        }
      }
      else
      {
        for (int index = offset; index < num2; ++index)
        {
          int num5 = (int) chars[index] - 48;
          switch (num5)
          {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
              if (num1 > 922337203685477580L)
                return false;
              long num6 = num1 * 10L;
              if (num6 > long.MaxValue - (long) num5)
                return false;
              num1 = num6 + (long) num5;
              continue;
            default:
              return false;
          }
        }
      }
      result = num1;
      return true;
    }

    private static bool TryParseSingle(byte[] chars, int offset, int count, out float result)
    {
      result = 0.0f;
      int num1 = offset + count;
      bool flag = false;
      if (offset < num1 && chars[offset] == (byte) 45)
      {
        flag = true;
        ++offset;
        --count;
      }
      if (count < 1 || count > 10)
        return false;
      int num2 = 0;
      for (; offset < num1; ++offset)
      {
        int num3 = (int) chars[offset] - 48;
        if (num3 == -2)
        {
          ++offset;
          int num4 = 1;
          for (; offset < num1; ++offset)
          {
            int num5 = (int) chars[offset] - 48;
            if ((uint) num5 >= 10U)
              return false;
            num4 *= 10;
            num2 = num2 * 10 + num5;
          }
          result = count <= 8 ? (float) num2 / (float) num4 : (float) num2 / (float) num4;
          if (flag)
            result = -result;
          return true;
        }
        if ((uint) num3 >= 10U)
          return false;
        num2 = num2 * 10 + num3;
      }
      if (count == 10)
        return false;
      result = !flag ? (float) num2 : (float) -num2;
      return true;
    }

    private static bool TryParseDouble(byte[] chars, int offset, int count, out double result)
    {
      result = 0.0;
      int num1 = offset + count;
      bool flag = false;
      if (offset < num1 && chars[offset] == (byte) 45)
      {
        flag = true;
        ++offset;
        --count;
      }
      if (count < 1 || count > 10)
        return false;
      int num2 = 0;
      for (; offset < num1; ++offset)
      {
        int num3 = (int) chars[offset] - 48;
        if (num3 == -2)
        {
          ++offset;
          int num4 = 1;
          for (; offset < num1; ++offset)
          {
            int num5 = (int) chars[offset] - 48;
            if ((uint) num5 >= 10U)
              return false;
            num4 *= 10;
            num2 = num2 * 10 + num5;
          }
          result = !flag ? (double) num2 / (double) num4 : -(double) num2 / (double) num4;
          return true;
        }
        if ((uint) num3 >= 10U)
          return false;
        num2 = num2 * 10 + num3;
      }
      if (count == 10)
        return false;
      result = !flag ? (double) num2 : (double) -num2;
      return true;
    }

    private static int ToInt32D2(byte[] chars, int offset)
    {
      byte num1 = (byte) ((uint) chars[offset + 0] - 48U);
      byte num2 = (byte) ((uint) chars[offset + 1] - 48U);
      return num1 > (byte) 9 || num2 > (byte) 9 ? -1 : 10 * (int) num1 + (int) num2;
    }

    private static int ToInt32D4(byte[] chars, int offset, int count)
    {
      return XmlConverter.ToInt32D7(chars, offset, count);
    }

    private static int ToInt32D7(byte[] chars, int offset, int count)
    {
      int int32D7 = 0;
      for (int index = 0; index < count; ++index)
      {
        byte num = (byte) ((uint) chars[offset + index] - 48U);
        if (num > (byte) 9)
          return -1;
        int32D7 = int32D7 * 10 + (int) num;
      }
      return int32D7;
    }

    private static bool TryParseDateTime(byte[] chars, int offset, int count, out DateTime result)
    {
      int num1 = offset + count;
      result = DateTime.MaxValue;
      if (count < 19 || chars[offset + 4] != (byte) 45 || chars[offset + 7] != (byte) 45 || chars[offset + 10] != (byte) 84 || chars[offset + 13] != (byte) 58 || chars[offset + 16] != (byte) 58)
        return false;
      int int32D4 = XmlConverter.ToInt32D4(chars, offset + 0, 4);
      int int32D2_1 = XmlConverter.ToInt32D2(chars, offset + 5);
      int int32D2_2 = XmlConverter.ToInt32D2(chars, offset + 8);
      int int32D2_3 = XmlConverter.ToInt32D2(chars, offset + 11);
      int int32D2_4 = XmlConverter.ToInt32D2(chars, offset + 14);
      int int32D2_5 = XmlConverter.ToInt32D2(chars, offset + 17);
      if ((int32D4 | int32D2_1 | int32D2_2 | int32D2_3 | int32D2_4 | int32D2_5) < 0)
        return false;
      DateTimeKind kind = DateTimeKind.Unspecified;
      offset += 19;
      int num2 = 0;
      if (offset < num1 && chars[offset] == (byte) 46)
      {
        ++offset;
        int offset1 = offset;
        for (; offset < num1; ++offset)
        {
          byte num3 = chars[offset];
          if (num3 < (byte) 48 || num3 > (byte) 57)
            break;
        }
        int count1 = offset - offset1;
        if (count1 < 1 || count1 > 7)
          return false;
        num2 = XmlConverter.ToInt32D7(chars, offset1, count1);
        if (num2 < 0)
          return false;
        for (int index = count1; index < 7; ++index)
          num2 *= 10;
      }
      bool flag = false;
      int hours = 0;
      int minutes = 0;
      if (offset < num1)
      {
        byte num4 = chars[offset];
        int num5;
        switch (num4)
        {
          case 43:
            num5 = 1;
            break;
          case 90:
            ++offset;
            kind = DateTimeKind.Utc;
            goto label_29;
          default:
            num5 = num4 == (byte) 45 ? 1 : 0;
            break;
        }
        if (num5 != 0)
        {
          ++offset;
          if (offset + 5 > num1 || chars[offset + 2] != (byte) 58)
            return false;
          kind = DateTimeKind.Utc;
          flag = true;
          hours = XmlConverter.ToInt32D2(chars, offset);
          minutes = XmlConverter.ToInt32D2(chars, offset + 3);
          if ((hours | minutes) < 0)
            return false;
          if (num4 == (byte) 43)
          {
            hours = -hours;
            minutes = -minutes;
          }
          offset += 5;
        }
label_29:;
      }
      if (offset < num1)
        return false;
      DateTime dateTime;
      try
      {
        dateTime = new DateTime(int32D4, int32D2_1, int32D2_2, int32D2_3, int32D2_4, int32D2_5, kind);
      }
      catch (ArgumentException ex)
      {
        return false;
      }
      if (num2 > 0)
        dateTime = dateTime.AddTicks((long) num2);
      if (flag)
      {
        try
        {
          TimeSpan timeSpan = new TimeSpan(hours, minutes, 0);
          dateTime = (hours < 0 || !(dateTime < DateTime.MaxValue - timeSpan)) && (hours >= 0 || !(dateTime > DateTime.MinValue - timeSpan)) ? dateTime.ToLocalTime().Add(timeSpan) : dateTime.Add(timeSpan).ToLocalTime();
        }
        catch (ArgumentOutOfRangeException ex)
        {
          return false;
        }
      }
      result = dateTime;
      return true;
    }

    public static int ToChars(bool value, byte[] buffer, int offset)
    {
      if (value)
      {
        buffer[offset + 0] = (byte) 116;
        buffer[offset + 1] = (byte) 114;
        buffer[offset + 2] = (byte) 117;
        buffer[offset + 3] = (byte) 101;
        return 4;
      }
      buffer[offset + 0] = (byte) 102;
      buffer[offset + 1] = (byte) 97;
      buffer[offset + 2] = (byte) 108;
      buffer[offset + 3] = (byte) 115;
      buffer[offset + 4] = (byte) 101;
      return 5;
    }

    public static int ToCharsR(int value, byte[] chars, int offset)
    {
      int num1 = 0;
      int charsR;
      if (value >= 0)
      {
        int num2;
        for (; value >= 10; value = num2)
        {
          num2 = value / 10;
          ++num1;
          chars[--offset] = (byte) (48 + (value - num2 * 10));
        }
        chars[--offset] = (byte) (48 + value);
        charsR = num1 + 1;
      }
      else
      {
        int num3;
        for (; value <= -10; value = num3)
        {
          num3 = value / 10;
          ++num1;
          chars[--offset] = (byte) (48 - (value - num3 * 10));
        }
        chars[--offset] = (byte) (48 - value);
        chars[--offset] = (byte) 45;
        charsR = num1 + 2;
      }
      return charsR;
    }

    public static int ToChars(int value, byte[] chars, int offset)
    {
      int charsR = XmlConverter.ToCharsR(value, chars, offset + 16);
      Buffer.BlockCopy((Array) chars, offset + 16 - charsR, (Array) chars, offset, charsR);
      return charsR;
    }

    public static int ToCharsR(long value, byte[] chars, int offset)
    {
      int num1 = 0;
      long num2;
      long num3;
      if (value >= 0L)
      {
        for (; value > (long) int.MaxValue; value = num2)
        {
          num2 = value / 10L;
          ++num1;
          chars[--offset] = (byte) (48U + (uint) (int) (value - num2 * 10L));
        }
      }
      else
      {
        for (; value < (long) int.MinValue; value = num3)
        {
          num3 = value / 10L;
          ++num1;
          chars[--offset] = (byte) (48U - (uint) (int) (value - num3 * 10L));
        }
      }
      return num1 + XmlConverter.ToCharsR((int) value, chars, offset);
    }

    public static int ToChars(long value, byte[] chars, int offset)
    {
      int charsR = XmlConverter.ToCharsR(value, chars, offset + 32);
      Buffer.BlockCopy((Array) chars, offset + 32 - charsR, (Array) chars, offset, charsR);
      return charsR;
    }

    private static int ToInfinity(bool isNegative, byte[] buffer, int offset)
    {
      if (isNegative)
      {
        buffer[offset + 0] = (byte) 45;
        buffer[offset + 1] = (byte) 73;
        buffer[offset + 2] = (byte) 78;
        buffer[offset + 3] = (byte) 70;
        return 4;
      }
      buffer[offset + 0] = (byte) 73;
      buffer[offset + 1] = (byte) 78;
      buffer[offset + 2] = (byte) 70;
      return 3;
    }

    private static int ToZero(bool isNegative, byte[] buffer, int offset)
    {
      if (isNegative)
      {
        buffer[offset + 0] = (byte) 45;
        buffer[offset + 1] = (byte) 48;
        return 2;
      }
      buffer[offset] = (byte) 48;
      return 1;
    }

    public static int ToChars(double value, byte[] buffer, int offset)
    {
      if (double.IsInfinity(value))
        return XmlConverter.ToInfinity(double.IsNegativeInfinity(value), buffer, offset);
      return value == 0.0 ? XmlConverter.ToZero(value < 0.0, buffer, offset) : XmlConverter.ToAsciiChars(value.ToString("R", (IFormatProvider) NumberFormatInfo.InvariantInfo), buffer, offset);
    }

    public static int ToChars(float value, byte[] buffer, int offset)
    {
      if (float.IsInfinity(value))
        return XmlConverter.ToInfinity(float.IsNegativeInfinity(value), buffer, offset);
      return (double) value == 0.0 ? XmlConverter.ToZero((double) value < 0.0, buffer, offset) : XmlConverter.ToAsciiChars(value.ToString("R", (IFormatProvider) NumberFormatInfo.InvariantInfo), buffer, offset);
    }

    public static int ToChars(Decimal value, byte[] buffer, int offset)
    {
      return XmlConverter.ToAsciiChars(value.ToString((string) null, (IFormatProvider) NumberFormatInfo.InvariantInfo), buffer, offset);
    }

    public static int ToChars(ulong value, byte[] buffer, int offset)
    {
      return XmlConverter.ToAsciiChars(value.ToString((string) null, (IFormatProvider) NumberFormatInfo.InvariantInfo), buffer, offset);
    }

    private static int ToAsciiChars(string s, byte[] buffer, int offset)
    {
      for (int index = 0; index < s.Length; ++index)
        buffer[offset++] = (byte) s[index];
      return s.Length;
    }

    private static int ToCharsD2(int value, byte[] chars, int offset)
    {
      if (value < 10)
      {
        chars[offset + 0] = (byte) 48;
        chars[offset + 1] = (byte) (48 + value);
      }
      else
      {
        int num = value / 10;
        chars[offset + 0] = (byte) (48 + num);
        chars[offset + 1] = (byte) (48 + value - num * 10);
      }
      return 2;
    }

    private static int ToCharsD4(int value, byte[] chars, int offset)
    {
      XmlConverter.ToCharsD2(value / 100, chars, offset);
      XmlConverter.ToCharsD2(value % 100, chars, offset + 2);
      return 4;
    }

    private static int ToCharsD7(int value, byte[] chars, int offset)
    {
      int num = 7 - XmlConverter.ToCharsR(value, chars, offset + 7);
      for (int index = 0; index < num; ++index)
        chars[offset + index] = (byte) 48;
      int charsD7 = 7;
      while (charsD7 > 0 && chars[offset + charsD7 - 1] == (byte) 48)
        --charsD7;
      return charsD7;
    }

    public static int ToChars(DateTime value, byte[] chars, int offset)
    {
      int num1 = offset;
      offset += XmlConverter.ToCharsD4(value.Year, chars, offset);
      chars[offset++] = (byte) 45;
      offset += XmlConverter.ToCharsD2(value.Month, chars, offset);
      chars[offset++] = (byte) 45;
      offset += XmlConverter.ToCharsD2(value.Day, chars, offset);
      chars[offset++] = (byte) 84;
      offset += XmlConverter.ToCharsD2(value.Hour, chars, offset);
      chars[offset++] = (byte) 58;
      offset += XmlConverter.ToCharsD2(value.Minute, chars, offset);
      chars[offset++] = (byte) 58;
      offset += XmlConverter.ToCharsD2(value.Second, chars, offset);
      int num2 = (int) (value.Ticks % 10000000L);
      if (num2 != 0)
      {
        chars[offset++] = (byte) 46;
        offset += XmlConverter.ToCharsD7(num2, chars, offset);
      }
      switch (value.Kind)
      {
        case DateTimeKind.Unspecified:
          return offset - num1;
        case DateTimeKind.Utc:
          chars[offset++] = (byte) 90;
          goto case DateTimeKind.Unspecified;
        case DateTimeKind.Local:
          TimeSpan utcOffset = TimeZoneInfo.Local.GetUtcOffset(value);
          chars[offset++] = utcOffset.Ticks >= 0L ? (byte) 43 : (byte) 45;
          offset += XmlConverter.ToCharsD2(Math.Abs(utcOffset.Hours), chars, offset);
          chars[offset++] = (byte) 58;
          offset += XmlConverter.ToCharsD2(Math.Abs(utcOffset.Minutes), chars, offset);
          goto case DateTimeKind.Unspecified;
        default:
          throw new InvalidOperationException();
      }
    }

    public static bool IsWhitespace(string s)
    {
      for (int index = 0; index < s.Length; ++index)
      {
        if (!XmlConverter.IsWhitespace(s[index]))
          return false;
      }
      return true;
    }

    public static bool IsWhitespace(char ch)
    {
      return ch <= ' ' && (ch == ' ' || ch == '\t' || ch == '\r' || ch == '\n');
    }

    public static string StripWhitespace(string s)
    {
      int length = s.Length;
      for (int index = 0; index < s.Length; ++index)
      {
        if (XmlConverter.IsWhitespace(s[index]))
          --length;
      }
      if (length == s.Length)
        return s;
      char[] chArray = new char[length];
      int num = 0;
      for (int index = 0; index < s.Length; ++index)
      {
        char ch = s[index];
        if (!XmlConverter.IsWhitespace(ch))
          chArray[num++] = ch;
      }
      return new string(chArray);
    }

    private static string Trim(string s)
    {
      int num = 0;
      while (num < s.Length && XmlConverter.IsWhitespace(s[num]))
        ++num;
      int length = s.Length;
      while (length > 0 && XmlConverter.IsWhitespace(s[length - 1]))
        --length;
      if (num == 0 && length == s.Length)
        return s;
      return length == 0 ? string.Empty : s.Substring(num, length - num);
    }
  }
}
