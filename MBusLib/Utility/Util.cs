// Decompiled with JetBrains decompiler
// Type: MBusLib.Utility.Util
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using ZENNER.CommonLibrary;

#nullable disable
namespace MBusLib.Utility
{
  public static class Util
  {
    public static readonly byte[] REQUEST_PREFIX_NO_STUFFING = Encoding.ASCII.GetBytes("_ no byte stuffing please _");
    private static readonly bool console_present;
    private static readonly char[] HEX_DIGITS = new char[16]
    {
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F'
    };
    private static readonly DateTime linuxStart1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);

    static Util()
    {
      Util.console_present = true;
      try
      {
        int windowHeight = Console.WindowHeight;
      }
      catch
      {
        Util.console_present = false;
      }
    }

    public static event EventHandler<string> LoggerEvent;

    public static void Log(object sender, string msg, Exception exc)
    {
      TimeSpan timeSpan = TimeSpan.FromMilliseconds((double) (DateTime.Now.Ticks / 10000L));
      string str = string.Format("[TID:{0}]({1})[{2}] {3} ERROR: {4}", (object) Thread.CurrentThread.ManagedThreadId, (object) string.Format("{0:0}:{1:0}:{2:0}", (object) timeSpan.Minutes, (object) timeSpan.Seconds, (object) timeSpan.Milliseconds), sender, (object) msg, (object) Util.PrintException(exc));
      if (!Util.console_present)
        Debug.WriteLine(str);
      else
        Console.WriteLine(str);
      EventHandler<string> loggerEvent = Util.LoggerEvent;
      if (loggerEvent == null)
        return;
      loggerEvent(sender, str);
    }

    public static void Log(object sender, string msg, List<byte> bytes)
    {
      if (bytes == null || bytes.Count == 0)
        return;
      Util.Log(sender, string.Format("{0} ({1}){2}", (object) msg, (object) bytes.Count, (object) Util.ByteArrayToHexString((IEnumerable<byte>) bytes)));
    }

    public static void Log(object sender, string msg, byte[] bytes)
    {
      if (bytes == null)
        Util.Log(sender, msg);
      else
        Util.Log(sender, string.Format("{0} ({1}){2}", (object) msg, (object) bytes.Length, (object) Util.ByteArrayToHexString((IEnumerable<byte>) bytes)));
    }

    public static void Log(object sender, string msg)
    {
      TimeSpan timeSpan = TimeSpan.FromMilliseconds((double) (DateTime.Now.Ticks / 10000L));
      string str = string.Format("[TID:{0}]({1})[{2}] {3}", (object) Thread.CurrentThread.ManagedThreadId, (object) string.Format("{0:0}:{1:0}:{2:0}", (object) timeSpan.Minutes, (object) timeSpan.Seconds, (object) timeSpan.Milliseconds), sender, (object) msg);
      if (!Util.console_present)
        Debug.WriteLine(str);
      else
        Console.WriteLine(str);
      EventHandler<string> loggerEvent = Util.LoggerEvent;
      if (loggerEvent == null)
        return;
      loggerEvent(sender, str);
    }

    public static bool IsNumber(object value)
    {
      int num;
      switch (value)
      {
        case sbyte _:
        case byte _:
        case short _:
        case ushort _:
        case int _:
        case uint _:
        case long _:
          num = 1;
          break;
        default:
          num = value is ulong ? 1 : 0;
          break;
      }
      return num != 0;
    }

    public static bool IsDecimalNumber(object value)
    {
      int num;
      switch (value)
      {
        case float _:
        case double _:
          num = 1;
          break;
        default:
          num = value is Decimal ? 1 : 0;
          break;
      }
      return num != 0;
    }

    public static object GetEnumAsUnderlyingType(Enum value)
    {
      value.GetType();
      Type underlyingType = Enum.GetUnderlyingType(value.GetType());
      return Convert.ChangeType((object) value, underlyingType);
    }

    public static string TimeSpanToReadableString(TimeSpan span, bool showSeconds = true)
    {
      string readableString = string.Format("{0}{1}{2}{3}", span.Duration().Days > 0 ? (object) string.Format("{0:0} day{1}, ", (object) span.Days, span.Days == 1 ? (object) string.Empty : (object) "s") : (object) string.Empty, span.Duration().Hours > 0 ? (object) string.Format("{0:0} hour{1}, ", (object) span.Hours, span.Hours == 1 ? (object) string.Empty : (object) "s") : (object) string.Empty, span.Duration().Minutes > 0 ? (object) string.Format("{0:0} minute{1}, ", (object) span.Minutes, span.Minutes == 1 ? (object) string.Empty : (object) "s") : (object) string.Empty, span.Duration().Seconds > 0 & showSeconds ? (object) string.Format("{0:0} second{1}", (object) span.Seconds, span.Seconds == 1 ? (object) string.Empty : (object) "s") : (object) string.Empty);
      if (readableString.EndsWith(", "))
        readableString = readableString.Substring(0, readableString.Length - 2);
      if (string.IsNullOrEmpty(readableString))
        readableString = "0 seconds";
      return readableString;
    }

    public static DateTime? UnixTimeStampToDateTime(long? unixTimeStamp)
    {
      return !unixTimeStamp.HasValue ? new DateTime?() : new DateTime?(Util.linuxStart1970.AddSeconds((double) unixTimeStamp.Value).ToLocalTime());
    }

    public static long? DateTimeToUnixTimeStamp(string date)
    {
      DateTime result;
      return DateTime.TryParse(date, out result) ? Util.DateTimeToUnixTimeStamp(new DateTime?(result)) : new long?();
    }

    public static long? DateTimeToUnixTimeStamp(DateTime? date)
    {
      return !date.HasValue ? new long?() : new long?((long) date.Value.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).TotalSeconds);
    }

    public static long UnixTimeNow()
    {
      return (long) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }

    public static T DeepClone<T>(this T obj)
    {
      using (MemoryStream serializationStream = new MemoryStream())
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize((Stream) serializationStream, (object) obj);
        serializationStream.Position = 0L;
        return (T) binaryFormatter.Deserialize((Stream) serializationStream);
      }
    }

    public static bool TryParseEnum<TEnum>(byte enumValue, out TEnum retVal)
    {
      retVal = default (TEnum);
      bool flag = Enum.IsDefined(typeof (TEnum), (object) enumValue);
      if (flag)
        retVal = (TEnum) Enum.ToObject(typeof (TEnum), enumValue);
      return flag;
    }

    public static bool TryParseEnum<TEnum>(int enumValue, out TEnum retVal)
    {
      retVal = default (TEnum);
      bool flag = Enum.IsDefined(typeof (TEnum), (object) enumValue);
      if (flag)
        retVal = (TEnum) Enum.ToObject(typeof (TEnum), enumValue);
      return flag;
    }

    public static string ByteArrayToHexString(IEnumerable<byte> buffer)
    {
      if (buffer == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte num in buffer)
        stringBuilder.AppendFormat("{0:X2}", (object) num);
      return stringBuilder.ToString();
    }

    public static int GetHashCode(
      uint serialnumber_BCD,
      ushort manufacturer,
      byte generation,
      Medium medium)
    {
      return Util.GetHashCode(serialnumber_BCD, manufacturer, generation, medium, new uint?(), new ushort?(), new byte?(), new Medium?());
    }

    public static string DateTimeToString(DateTime? dateTime)
    {
      if (!dateTime.HasValue)
        return string.Empty;
      DateTime dateTime1 = dateTime.Value;
      return dateTime1.Hour == 0 && dateTime1.Minute == 0 ? dateTime1.ToShortDateString() : dateTime1.ToString("g");
    }

    public static int GetHashCode(
      uint serialnumber_BCD,
      ushort manufacturer,
      byte generation,
      Medium medium,
      uint? serialnumberSecondary_BCD,
      ushort? manufacturerSecondary,
      byte? generationSecondary,
      Medium? mediumSecondary)
    {
      int hashCode = (((17 * 23 + serialnumber_BCD.GetHashCode()) * 23 + manufacturer.GetHashCode()) * 23 + generation.GetHashCode()) * 23 + medium.GetHashCode();
      if (serialnumberSecondary_BCD.HasValue)
        hashCode = hashCode * 23 + serialnumberSecondary_BCD.GetHashCode();
      if (manufacturerSecondary.HasValue)
        hashCode = hashCode * 23 + manufacturerSecondary.GetHashCode();
      if (generationSecondary.HasValue)
        hashCode = hashCode * 23 + generationSecondary.GetHashCode();
      if (mediumSecondary.HasValue)
        hashCode = hashCode * 23 + mediumSecondary.GetHashCode();
      return hashCode;
    }

    public static string ByteArrayToHexString(IEnumerable<byte> buffer, char separator)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte num in buffer)
        stringBuilder.AppendFormat("{0:X2}" + separator.ToString(), (object) num);
      return stringBuilder.ToString().TrimEnd(separator);
    }

    public static string DumpHexString(
      byte[] array,
      bool showAddress,
      bool showASCII,
      int showBytes = 16)
    {
      return Util.DumpHexString(array, 0, array.Length, showAddress, showASCII, showBytes);
    }

    public static string DumpHexString(
      byte[] array,
      int offset,
      int length,
      bool showAddress,
      bool showASCII,
      int showBytes = 16)
    {
      StringBuilder stringBuilder = new StringBuilder();
      byte[] bytes = new byte[showBytes];
      int num1 = 0;
      if (showAddress)
      {
        stringBuilder.Append("\n0x");
        stringBuilder.Append(Util.ToHexString(offset));
      }
      else
        stringBuilder.AppendLine();
      for (int i = offset; i < offset + length; ++i)
      {
        if (num1 == showBytes)
        {
          stringBuilder.Append(" ");
          if (showASCII)
          {
            for (int startIndex = 0; startIndex < showBytes; ++startIndex)
            {
              if (bytes[startIndex] > (byte) 32 && bytes[startIndex] < (byte) 126)
                stringBuilder.Append(Encoding.Default.GetString(bytes).Substring(startIndex, 1));
              else
                stringBuilder.Append(".");
            }
          }
          if (showAddress)
          {
            stringBuilder.Append("\n0x");
            stringBuilder.Append(Util.ToHexString(i));
          }
          else
            stringBuilder.AppendLine();
          num1 = 0;
        }
        byte num2 = array[i];
        stringBuilder.Append(" ");
        stringBuilder.Append(Util.HEX_DIGITS[(int) num2 >> 4 & 15]);
        stringBuilder.Append(Util.HEX_DIGITS[(int) num2 & 15]);
        bytes[num1++] = num2;
      }
      if (num1 != showBytes)
      {
        int num3 = (showBytes - num1) * 3 + 1;
        for (int index = 0; index < num3; ++index)
          stringBuilder.Append(" ");
        if (showASCII)
        {
          for (int startIndex = 0; startIndex < num1; ++startIndex)
          {
            if (bytes[startIndex] > (byte) 32 && bytes[startIndex] < (byte) 126)
              stringBuilder.Append(Encoding.Default.GetString(bytes).Substring(startIndex, 1));
            else
              stringBuilder.Append(".");
          }
        }
      }
      return stringBuilder.ToString();
    }

    public static string PrintException(Exception exc)
    {
      string str = exc.GetType().Name + ": " + exc.Message + " ";
      Exception exception = exc;
      for (Exception innerException = exc.InnerException; innerException != null; innerException = innerException.InnerException)
      {
        if (innerException.Message != exception.Message)
          str = str + innerException.Message + " ";
        exception = innerException;
      }
      return str + exception.StackTrace;
    }

    public static string ArrayToStringWithNewLine(IEnumerable array)
    {
      if (array == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine();
      foreach (object obj in array)
        stringBuilder.AppendLine(obj.ToString());
      return stringBuilder.ToString().TrimEnd(Environment.NewLine.ToCharArray());
    }

    public static string ToHexString(byte[] byteArray)
    {
      return BitConverter.ToString(byteArray).Replace("-", "");
    }

    public static string ToHexString(byte[] byteArray, int offset, int length)
    {
      StringBuilder stringBuilder = new StringBuilder(length * 2);
      for (; offset < byteArray.Length && length > 0; --length)
      {
        stringBuilder.AppendFormat("{0:X2}", (object) byteArray[offset]);
        ++offset;
      }
      return stringBuilder.ToString();
    }

    public static string ToHexString(int i) => Util.ToHexString(Util.ToByteArray(i));

    public static string ToHexString(short i) => Util.ToHexString(Util.ToByteArray(i));

    public static byte[] ToByteArray(byte b)
    {
      return new byte[1]{ b };
    }

    public static bool ToBoolean(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return false;
      if (obj.GetType() == Type.GetType("System.Int32"))
        return Convert.ToInt32(obj) != 0;
      if (obj.GetType() == Type.GetType("System.SByte"))
        return Convert.ToSByte(obj) != (sbyte) 0;
      if (obj.GetType() == Type.GetType("System.Int16"))
        return Convert.ToInt16(obj) != (short) 0;
      if (obj.GetType() == Type.GetType("System.Decimal"))
        return !(Convert.ToDecimal(obj) == 0M);
      if (obj.GetType() == Type.GetType("System.String"))
      {
        string lower = obj.ToString().ToLower();
        return lower == "true" || lower == "on" || lower == "1";
      }
      return !(obj.GetType() != Type.GetType("System.Boolean")) && bool.Parse(obj.ToString());
    }

    public static long ToLong(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return 0;
      Type type = obj.GetType();
      if (type == typeof (long) || type == typeof (bool) || type == typeof (double))
        return Convert.ToInt64(obj);
      string s = obj.ToString();
      return s == string.Empty ? 0L : long.Parse(s, NumberStyles.Any);
    }

    public static int ToInteger(object obj) => Util.ToInteger(obj, 0);

    public static int ToInteger(object obj, int defaultValue)
    {
      if (obj == null || obj == DBNull.Value)
        return defaultValue;
      if (obj.GetType() == Type.GetType("System.Int32"))
        return Convert.ToInt32(obj);
      if (obj.GetType() == Type.GetType("System.Boolean"))
        return Convert.ToBoolean(obj) ? 1 : 0;
      if (obj.GetType() == Type.GetType("System.Single"))
        return Convert.ToInt32(Math.Floor((double) (float) obj));
      string s = obj.ToString();
      return s == string.Empty ? defaultValue : int.Parse(s, NumberStyles.Any);
    }

    public static double ToDouble(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return 0.0;
      if (obj.GetType() == typeof (double) || obj.GetType() == typeof (Decimal))
        return Convert.ToDouble(obj);
      string s = obj.ToString();
      return s == string.Empty ? 0.0 : double.Parse(s, NumberStyles.Any);
    }

    public static Decimal ToDecimal(object obj)
    {
      if (obj == null || obj == DBNull.Value)
        return 0M;
      if (obj.GetType() == Type.GetType("System.Decimal"))
        return Convert.ToDecimal(obj);
      string s = obj.ToString();
      return s == string.Empty ? 0M : Decimal.Parse(s, NumberStyles.Any);
    }

    public static byte[] ToByteArray(int i)
    {
      byte[] byteArray = new byte[4]
      {
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) (i & (int) byte.MaxValue)
      };
      byteArray[2] = (byte) (i >> 8 & (int) byte.MaxValue);
      byteArray[1] = (byte) (i >> 16 & (int) byte.MaxValue);
      byteArray[0] = (byte) (i >> 24 & (int) byte.MaxValue);
      return byteArray;
    }

    public static byte[] ToByteArray(short i)
    {
      byte[] byteArray = new byte[2]
      {
        (byte) 0,
        (byte) ((uint) i & (uint) byte.MaxValue)
      };
      byteArray[0] = (byte) ((int) i >> 8 & (int) byte.MaxValue);
      return byteArray;
    }

    public static long ConvertInt64ToBcdInt64(long value)
    {
      long bcdInt64 = 0;
      for (int index = 0; index < 8; ++index)
      {
        long num = value % 10L;
        bcdInt64 |= num << index * 4;
        value /= 10L;
      }
      return bcdInt64;
    }

    public static byte[] ConvertLongToByteArray(long obj, int arrayLength)
    {
      byte[] byteArray = new byte[arrayLength];
      for (int index = 0; index < arrayLength; ++index)
        byteArray[index] = (byte) (obj >> index * 8);
      return byteArray;
    }

    public static ushort CalculatesCRC16ForDownlink(byte[] data, int offset, int length)
    {
      ushort num = ushort.MaxValue;
      for (int index1 = offset; index1 < length; ++index1)
      {
        num ^= (ushort) ((uint) data[index1] << 8);
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if ((ushort) ((uint) num & 32768U) > (ushort) 0)
            num = (ushort) ((uint) (ushort) ((uint) num << 1) ^ 32773U);
          else
            num <<= 1;
        }
      }
      return num;
    }

    public static ushort CalculatesCRC16_CC430(byte[] buffer)
    {
      return Util.CalculatesCRC16_CC430(buffer, 0, buffer.Length);
    }

    public static ushort CalculatesCRC16_CC430(byte[] buffer, int offset, int size)
    {
      ushort crc = ushort.MaxValue;
      for (int index = offset; index < size + offset; ++index)
        crc = Util.crc_calc_ccitt_(crc, buffer[index]);
      return crc;
    }

    public static ushort CalculatesCRC16_CC430(List<byte> buffer)
    {
      ushort crc = ushort.MaxValue;
      foreach (byte data in buffer)
        crc = Util.crc_calc_ccitt_(crc, data);
      return crc;
    }

    private static ushort crc_calc_ccitt_(ushort crc, byte data)
    {
      byte num = 0;
      for (byte index = 0; index < (byte) 8; ++index)
      {
        if (((uint) data & 1U << (int) index) > 0U)
          num |= (byte) (128U >> (int) index);
      }
      crc ^= (ushort) ((uint) num << 8);
      for (byte index = 0; index < (byte) 8; ++index)
      {
        if (((uint) crc & 32768U) > 0U)
          crc = (ushort) ((int) crc << 1 ^ 4129);
        else
          crc <<= 1;
      }
      return crc;
    }

    public static ushort CalculateCRC16(ushort? crcInitValue, byte[] buffer)
    {
      return Util.CalculateCRC16(crcInitValue, buffer, 0, buffer.Length);
    }

    public static ushort CalculateCRC16(
      ushort? crcInitValue,
      byte[] buffer,
      int offset,
      int length)
    {
      ushort crC16 = (ushort) ((int) crcInitValue ?? (int) ushort.MaxValue);
      ushort num = 4129;
      for (int index1 = offset; index1 < length; ++index1)
      {
        crC16 ^= (ushort) ((uint) buffer[index1] << 8);
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if (((uint) crC16 & 32768U) > 0U)
            crC16 = (ushort) ((uint) crC16 << 1 ^ (uint) num);
          else
            crC16 <<= 1;
        }
      }
      return crC16;
    }

    public static ushort CalculateCRC16_MBWBLUE(byte[] buffer, int offset, int length)
    {
      ushort crC16Mbwblue = 0;
      ushort num = 15717;
      for (int index1 = offset; index1 < length; ++index1)
      {
        crC16Mbwblue ^= (ushort) ((uint) buffer[index1] << 8);
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if (((uint) crC16Mbwblue & 32768U) > 0U)
            crC16Mbwblue = (ushort) ((uint) crC16Mbwblue << 1 ^ (uint) num);
          else
            crC16Mbwblue <<= 1;
        }
      }
      return crC16Mbwblue;
    }

    public static string PrintObject(object obj, int spaces = 0)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj))
      {
        string name = property.Name;
        object buffer = property.GetValue(obj);
        string str = string.Empty;
        if (buffer != null)
        {
          if (buffer.GetType() == typeof (byte[]))
          {
            str = Util.ByteArrayToHexString((IEnumerable<byte>) (byte[]) buffer);
          }
          else
          {
            switch (buffer)
            {
              case IPrintable printable:
                str = printable.Print();
                break;
              case IList _:
                IEnumerator enumerator = ((IEnumerable) buffer).GetEnumerator();
                try
                {
                  while (enumerator.MoveNext())
                  {
                    object current = enumerator.Current;
                    str += Util.PrintObject(current, spaces + 4);
                  }
                  break;
                }
                finally
                {
                  if (enumerator is IDisposable disposable)
                    disposable.Dispose();
                }
              default:
                str = buffer.ToString();
                break;
            }
          }
        }
        stringBuilder.Append(' ', spaces).AppendFormat("{0}={1}", (object) name, (object) str).AppendLine();
      }
      return stringBuilder.ToString();
    }

    public static long? ConvertBcdInt64ToInt64(byte byte1, byte byte2, byte byte3, byte byte4)
    {
      if (byte1 == byte.MaxValue && byte2 == byte.MaxValue && byte3 == byte.MaxValue && byte4 == byte.MaxValue)
        return new long?();
      return byte1 == (byte) 0 && byte2 == (byte) 0 && byte3 == (byte) 0 && byte4 == (byte) 0 ? new long?(0L) : new long?(Util.ConvertBcdInt64ToInt64((long) byte1 << 24 | (long) byte2 << 16 | (long) byte3 << 8 | (long) byte4));
    }

    public static long ConvertBcdInt64ToInt64(long bcd)
    {
      long num = 1;
      long int64 = 0;
      for (; bcd > 0L; bcd >>= 4)
      {
        int64 += (bcd & 15L) * num;
        num *= 10L;
      }
      return int64;
    }

    public static uint ConvertBcdUInt32ToUInt32(uint bcd)
    {
      uint num = 1;
      uint uint32 = 0;
      for (; bcd > 0U; bcd >>= 4)
      {
        uint32 += (bcd & 15U) * num;
        num *= 10U;
      }
      return uint32;
    }

    public static uint ConvertUnt32ToBcdUInt32(uint value)
    {
      uint bcdUint32 = 0;
      for (int index = 0; index < 8; ++index)
      {
        uint num = value % 10U;
        bcdUint32 |= num << index * 4;
        value /= 10U;
      }
      return bcdUint32;
    }

    public static long DecodeBcd(byte[] data, int size)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      bool flag = false;
      long num = 0;
      for (int index = size - 1; index >= 0; --index)
      {
        if (index == size - 1 && (int) data[size - 1] >> 4 == 15)
          flag = true;
        else
          num = num * 10L + (long) ((int) data[index] >> 4 & 15);
        num = num * 10L + (long) ((int) data[index] & 15);
      }
      return flag ? num * -1L : num;
    }

    public static byte[] HexStringToByteArray(string hex)
    {
      if (string.IsNullOrEmpty(hex))
        return (byte[]) null;
      hex = hex.Replace(" ", string.Empty);
      hex = hex.Replace("-", string.Empty);
      hex = hex.Replace("\n", string.Empty);
      hex = hex.Replace("\r", string.Empty);
      int length = hex.Length;
      byte[] byteArray = length % 2 == 0 ? new byte[length / 2] : throw new ArgumentException("Hex-string is invalid! The hex-string must be modulo 2.");
      for (int startIndex = 0; startIndex < length; startIndex += 2)
        byteArray[startIndex / 2] = Convert.ToByte(hex.Substring(startIndex, 2), 16);
      return byteArray;
    }

    public static DateTime ToDateTime1980(uint seconds)
    {
      return new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).Add(new TimeSpan(0, 0, (int) seconds));
    }

    public static string GetManufacturer(ushort code)
    {
      return "" + ((char) (((int) code >> 10 & 31) + 64)).ToString() + ((char) (((int) code >> 5 & 31) + 64)).ToString() + ((char) (((int) code & 31) + 64)).ToString();
    }

    public static ushort GetManufacturerCode(string manufacturer)
    {
      if (string.IsNullOrEmpty(manufacturer))
        manufacturer = string.Empty;
      manufacturer = manufacturer.PadRight(3);
      return (ushort) ((uint) (ushort) ((uint) (ushort) (0U + (uint) (ushort) ((uint) (byte) manufacturer[2] - 64U)) + (uint) (ushort) ((uint) (ushort) ((uint) (byte) manufacturer[1] - 64U) << 5)) + (uint) (ushort) ((uint) (ushort) ((uint) (byte) manufacturer[0] - 64U) << 10));
    }

    public static byte CalculateChecksum(byte[] buffer, int startIndex, int count)
    {
      byte checksum = 0;
      for (int index = startIndex; index < startIndex + count; ++index)
        checksum += buffer[index];
      return checksum;
    }

    public static DateTime? ConvertToDateTime_SystemTime24(byte[] buffer, int startIndex = 0)
    {
      int num1 = startIndex;
      byte[] numArray1 = buffer;
      int index1 = num1;
      int num2 = index1 + 1;
      byte num3 = numArray1[index1];
      byte[] numArray2 = buffer;
      int index2 = num2;
      int num4 = index2 + 1;
      byte month = numArray2[index2];
      byte[] numArray3 = buffer;
      int index3 = num4;
      int num5 = index3 + 1;
      byte day = numArray3[index3];
      if (month == (byte) 0 || day == (byte) 0)
        return new DateTime?();
      try
      {
        return new DateTime?(new DateTime(2000 + (int) num3, (int) month, (int) day));
      }
      catch
      {
        return new DateTime?();
      }
    }

    public static DateTime? ConvertToDateTime_SystemTime48(byte[] buffer, int startIndex = 0)
    {
      int num1 = startIndex;
      byte[] numArray1 = buffer;
      int index1 = num1;
      int num2 = index1 + 1;
      byte num3 = numArray1[index1];
      byte[] numArray2 = buffer;
      int index2 = num2;
      int num4 = index2 + 1;
      byte month = numArray2[index2];
      byte[] numArray3 = buffer;
      int index3 = num4;
      int num5 = index3 + 1;
      byte day = numArray3[index3];
      byte[] numArray4 = buffer;
      int index4 = num5;
      int num6 = index4 + 1;
      byte hour = numArray4[index4];
      byte[] numArray5 = buffer;
      int index5 = num6;
      int num7 = index5 + 1;
      byte minute = numArray5[index5];
      byte[] numArray6 = buffer;
      int index6 = num7;
      int num8 = index6 + 1;
      byte second = numArray6[index6];
      if (num3 == byte.MaxValue || month == (byte) 0 || day == (byte) 0)
        return new DateTime?();
      try
      {
        return new DateTime?(new DateTime(2000 + (int) num3, (int) month, (int) day, (int) hour, (int) minute, (int) second));
      }
      catch
      {
        return new DateTime?();
      }
    }

    public static DateTime? ConvertToDateTime_SystemTime64(byte[] buffer, int startIndex = 0)
    {
      int num1 = startIndex;
      byte[] numArray1 = buffer;
      int index1 = num1;
      int num2 = index1 + 1;
      byte num3 = numArray1[index1];
      byte[] numArray2 = buffer;
      int index2 = num2;
      int num4 = index2 + 1;
      byte month = numArray2[index2];
      byte[] numArray3 = buffer;
      int index3 = num4;
      int num5 = index3 + 1;
      byte day = numArray3[index3];
      byte[] numArray4 = buffer;
      int index4 = num5;
      int num6 = index4 + 1;
      byte hour = numArray4[index4];
      byte[] numArray5 = buffer;
      int index5 = num6;
      int num7 = index5 + 1;
      byte minute = numArray5[index5];
      byte[] numArray6 = buffer;
      int index6 = num7;
      int num8 = index6 + 1;
      byte second = numArray6[index6];
      if (month == (byte) 0 || day == (byte) 0)
        return new DateTime?();
      try
      {
        return new DateTime?(new DateTime(2000 + (int) num3, (int) month, (int) day, (int) hour, (int) minute, (int) second));
      }
      catch
      {
        return new DateTime?();
      }
    }

    public static byte[] GetBytes_SystemTime24(DateTime value)
    {
      return new byte[3]
      {
        Convert.ToByte(value.Year % 100),
        Convert.ToByte(value.Month),
        Convert.ToByte(value.Day)
      };
    }

    public static byte[] GetBytes_SystemTime64(DateTime value)
    {
      return new byte[7]
      {
        Convert.ToByte(value.Year % 100),
        Convert.ToByte(value.Month),
        Convert.ToByte(value.Day),
        Convert.ToByte(value.Hour),
        Convert.ToByte(value.Minute),
        Convert.ToByte(value.Second),
        (byte) 0
      };
    }

    public static ushort ConvertDateTimeTo_YYYYMMMMYYYDDDDD(DateTime date)
    {
      ushort num1 = (ushort) ((uint) (ushort) date.Day | (uint) (ushort) (date.Month << 8));
      ushort num2 = (ushort) (date.Year % 100);
      return (ushort) ((uint) num1 | (uint) (ushort) (((int) num2 & 120) << 9 | ((int) num2 & 7) << 5));
    }

    public static DateTime? ConvertToDateTime_MBus_CP32_TypeF(byte[] buffer, int startIndex = 0)
    {
      if (startIndex + 4 > buffer.Length)
        return new DateTime?();
      if (((int) buffer[startIndex] & 128) == 128)
        return new DateTime?();
      int minute = (int) buffer[startIndex] & 63;
      if (minute > 59)
        return new DateTime?();
      int hour = (int) buffer[startIndex + 1] & 31;
      if (hour > 23)
        return new DateTime?();
      int day = (int) buffer[startIndex + 2] & 31;
      if (day < 1 || day > 31)
        return new DateTime?();
      int month = (int) buffer[startIndex + 3] & 15;
      if (month < 1 || month > 12)
        return new DateTime?();
      int num = (int) buffer[startIndex + 2] >> 5 | ((int) buffer[startIndex + 3] & 240) >> 1;
      if (num > 99)
        return new DateTime?();
      int year = num + 2000;
      try
      {
        return new DateTime?(new DateTime(year, month, day, hour, minute, 0));
      }
      catch
      {
        return new DateTime?();
      }
    }

    public static byte[] GetBytes_MBus_CP16_TypeG(DateTime? date)
    {
      if (!date.HasValue)
        return new byte[2]{ byte.MaxValue, byte.MaxValue };
      int num = date.Value.Year - 2000;
      return BitConverter.GetBytes((ushort) ((date.Value.Month & 15) << 8 | date.Value.Day & 31 | (num & 7) << 5 | (num & 120) << 9));
    }

    public static DateTime? ConvertToDate_MBus_CP16_TypeG(byte[] buffer)
    {
      return Util.ConvertToDate_MBus_CP16_TypeG(buffer, 0);
    }

    public static DateTime? ConvertToDate_MBus_CP16_TypeG(byte[] buffer, int startIndex)
    {
      if (buffer == null)
        return new DateTime?();
      if (startIndex + 1 >= buffer.Length)
        return new DateTime?();
      int num1 = (int) buffer[startIndex] & 31;
      if (num1 < 1 || num1 > 31)
        return new DateTime?();
      int num2 = (int) buffer[startIndex + 1] & 15;
      if (num2 < 1 || num2 > 12)
        return new DateTime?();
      int num3 = (int) buffer[startIndex] >> 5 | ((int) buffer[startIndex + 1] & 240) >> 1;
      if (num3 > 99)
        return new DateTime?();
      DateTime result;
      return DateTime.TryParse(string.Format("{0}-{1}-{2}T00:00:00", (object) (num3 + 2000), (object) num2, (object) num1), out result) ? new DateTime?(result) : new DateTime?();
    }

    public static DateTime? ToDateTime_12Byte(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (buffer.Length != 12)
        throw new ArgumentException(nameof (buffer));
      BitConverter.ToUInt32(buffer, 0);
      uint uint32_1 = BitConverter.ToUInt32(buffer, 4);
      uint uint32_2 = BitConverter.ToUInt32(buffer, 8);
      uint num1 = (uint) (((int) uint32_1 & 15) + (int) ((uint32_1 & 112U) >> 4) * 10);
      uint num2 = ((uint32_1 & 3840U) >> 8) + ((uint32_1 & 28672U) >> 12) * 10U;
      uint num3 = ((uint32_1 & 983040U) >> 16) + ((uint32_1 & 3145728U) >> 20) * 10U;
      uint num4 = (uint) (((int) uint32_2 & 15) + (int) ((uint32_2 & 48U) >> 4) * 10);
      uint num5 = ((uint32_2 & 3840U) >> 8) + ((uint32_2 & 4096U) >> 12) * 10U;
      DateTime result;
      return DateTime.TryParse(string.Format("{0}-{1}-{2}T{3}:{4}:{5}", (object) (uint) ((int) ((uint32_2 & 983040U) >> 16) + (int) ((uint32_2 & 15728640U) >> 20) * 10 + 2000), (object) num5, (object) num4, (object) num3, (object) num2, (object) num1), out result) ? new DateTime?(result) : new DateTime?();
    }

    public static bool IsBcdValid(byte[] data, int size)
    {
      if (data == null || data.Length == 0)
        return false;
      for (int index = size - 1; index >= 0; --index)
      {
        if (index != size - 1 || (int) data[size - 1] >> 4 != 15)
        {
          int num1 = (int) data[index] & 15;
          int num2 = (int) data[index] >> 4;
          if (num1 >= 10 && num1 <= 15 || num2 >= 10 && num2 <= 15)
            return false;
        }
      }
      return true;
    }

    public static bool IsBcdValid(string bcd)
    {
      foreach (char ch in bcd)
      {
        if (ch < '0' || ch > '9')
          return false;
      }
      return true;
    }

    public static bool IsIntegerValid(byte[] value, int size)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      return value.Length == size;
    }

    public static byte[] DecryptCBC_AES_128(byte[] key, byte[] IV, byte[] encrypted)
    {
      if (encrypted == null || encrypted.Length == 0)
        throw new ArgumentNullException(nameof (encrypted));
      if (key == null || key.Length == 0)
        throw new ArgumentNullException(nameof (key));
      if (IV == null || IV.Length == 0)
        throw new ArgumentNullException(nameof (IV));
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      rijndaelManaged.Mode = CipherMode.CBC;
      rijndaelManaged.IV = IV;
      rijndaelManaged.Key = key;
      rijndaelManaged.Padding = PaddingMode.Zeros;
      using (ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor())
      {
        CryptoStream cryptoStream = new CryptoStream((Stream) new MemoryStream(encrypted), decryptor, CryptoStreamMode.Read);
        using (MemoryStream memoryStream = new MemoryStream())
        {
          byte[] buffer = new byte[encrypted.Length];
          while (true)
          {
            int count = cryptoStream.Read(buffer, 0, buffer.Length);
            if (count > 0)
              memoryStream.Write(buffer, 0, count);
            else
              break;
          }
          return memoryStream.ToArray();
        }
      }
    }

    public static byte[] EncryptCBC_AES_128(byte[] key, byte[] IV, byte[] decrypted)
    {
      if (decrypted == null || decrypted.Length == 0)
        throw new ArgumentNullException(nameof (decrypted));
      if (key == null || key.Length == 0)
        throw new ArgumentNullException(nameof (key));
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      rijndaelManaged.Mode = CipherMode.CBC;
      rijndaelManaged.IV = IV;
      rijndaelManaged.Key = key;
      rijndaelManaged.Padding = PaddingMode.Zeros;
      using (ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor())
      {
        CryptoStream cryptoStream = new CryptoStream((Stream) new MemoryStream(decrypted), encryptor, CryptoStreamMode.Read);
        using (MemoryStream memoryStream = new MemoryStream())
        {
          byte[] buffer = new byte[decrypted.Length];
          while (true)
          {
            int count = cryptoStream.Read(buffer, 0, buffer.Length);
            if (count > 0)
              memoryStream.Write(buffer, 0, count);
            else
              break;
          }
          return memoryStream.ToArray();
        }
      }
    }

    public static string ReverseString(string s)
    {
      char[] charArray = s.ToCharArray();
      Array.Reverse((Array) charArray);
      return new string(charArray);
    }

    public static uint ReverseBytes(uint value)
    {
      return (uint) (((int) value & (int) byte.MaxValue) << 24 | ((int) value & 65280) << 8) | (value & 16711680U) >> 8 | (value & 4278190080U) >> 24;
    }

    public static ulong ReverseBytes(ulong value)
    {
      return (ulong) (((long) value & (long) byte.MaxValue) << 56 | ((long) value & 65280L) << 40 | ((long) value & 16711680L) << 24 | ((long) value & 4278190080L) << 8) | (value & 1095216660480UL) >> 8 | (value & 280375465082880UL) >> 24 | (value & 71776119061217280UL) >> 40 | (value & 18374686479671623680UL) >> 56;
    }

    public static string ElapsedToString(TimeSpan elapsed)
    {
      if ((long) elapsed.TotalHours > 0L)
        return string.Format("{0:0}h {1:0}m {2:0}s {3:0}ms", (object) elapsed.TotalHours, (object) elapsed.Minutes, (object) elapsed.Seconds, (object) elapsed.Milliseconds);
      if ((long) elapsed.TotalMinutes > 0L)
        return string.Format("{0:0}m {1:0}s {2:0}ms", (object) elapsed.TotalMinutes, (object) elapsed.Seconds, (object) elapsed.Milliseconds);
      if ((long) elapsed.TotalSeconds > 0L)
        return string.Format("{0:0}s {1:0}ms", (object) elapsed.TotalSeconds, (object) elapsed.Milliseconds);
      return (long) elapsed.TotalMilliseconds > 0L ? string.Format("{0:0}ms", (object) elapsed.TotalMilliseconds) : string.Empty;
    }

    public static byte[] Zip(byte[] buffer)
    {
      using (MemoryStream src = new MemoryStream(buffer))
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (GZipStream dest = new GZipStream((Stream) memoryStream, CompressionMode.Compress))
            Util.CopyTo((Stream) src, (Stream) dest);
          return memoryStream.ToArray();
        }
      }
    }

    public static byte[] Unzip(byte[] bytes)
    {
      using (MemoryStream memoryStream = new MemoryStream(bytes))
      {
        using (MemoryStream dest = new MemoryStream())
        {
          using (GZipStream src = new GZipStream((Stream) memoryStream, CompressionMode.Decompress))
            Util.CopyTo((Stream) src, (Stream) dest);
          return dest.ToArray();
        }
      }
    }

    public static void CopyTo(Stream src, Stream dest)
    {
      byte[] buffer = new byte[4096];
      int count;
      while ((count = src.Read(buffer, 0, buffer.Length)) != 0)
        dest.Write(buffer, 0, count);
    }

    public static byte[] UnStuffData(byte[] buffer, byte pattern)
    {
      int length = buffer.Length;
      int index1 = 0;
      int index2 = 0;
      while (index1 < buffer.Length)
      {
        buffer[index2] = buffer[index1];
        if ((int) buffer[index1] == (int) pattern && buffer.Length > index1 + 1 && (int) buffer[index1 + 1] == (int) pattern)
        {
          ++index1;
          --length;
        }
        ++index1;
        ++index2;
      }
      return buffer.Length == length ? buffer : buffer.SubArray<byte>(0, length);
    }

    public static double Distance(double lat1, double lon1, double lat2, double lon2, char unit)
    {
      double deg = lon1 - lon2;
      double num = Util.Rad2deg(Math.Acos(Math.Sin(Util.Deg2rad(lat1)) * Math.Sin(Util.Deg2rad(lat2)) + Math.Cos(Util.Deg2rad(lat1)) * Math.Cos(Util.Deg2rad(lat2)) * Math.Cos(Util.Deg2rad(deg)))) * 60.0 * 1.1515;
      switch (unit)
      {
        case 'K':
          num *= 1.609344;
          break;
        case 'N':
          num *= 0.8684;
          break;
      }
      return num;
    }

    private static double Deg2rad(double deg) => deg * Math.PI / 180.0;

    private static double Rad2deg(double rad) => rad / Math.PI * 180.0;

    public static int CalculateDistance(double rssi)
    {
      int num1 = -49;
      if (rssi == 0.0)
        return -1;
      double x = rssi * 1.0 / (double) num1;
      int num2 = x >= 1.0 ? Convert.ToInt32(0.89976 * Math.Pow(x, 7.7095) + 0.111) : Convert.ToInt32(Math.Pow(x, 10.0));
      return num2 > 0 ? num2 : 1;
    }

    public static int RssiToRssi_dBm(byte rssi)
    {
      int num = (int) rssi;
      if (rssi >= (byte) 128)
        num -= 256;
      return num / 2 - 74;
    }

    public static int Rssi_dBmToQuality(int dBm)
    {
      if (dBm <= -100)
        return 0;
      return dBm >= -50 ? 100 : 2 * (dBm + 100);
    }

    public static IReadOnlyList<byte[]> SplitByteArray(IReadOnlyList<byte> bytes, int size)
    {
      List<byte[]> numArrayList = new List<byte[]>();
      for (int index1 = 0; index1 < bytes.Count; index1 += size)
      {
        byte[] numArray = new byte[index1 + size <= bytes.Count ? size : bytes.Count - index1];
        for (int index2 = 0; index2 < numArray.Length; ++index2)
          numArray[index2] = bytes[index1 + index2];
        numArrayList.Add(numArray);
      }
      return (IReadOnlyList<byte[]>) numArrayList;
    }

    public static DateTime ObjectToDateTime(object o, DateTime defaultValue)
    {
      if (o == null)
        return defaultValue;
      if (o is DateTime dateTime)
        return dateTime;
      DateTime result;
      return DateTime.TryParse(o.ToString(), out result) ? result : defaultValue;
    }

    public static SortedDictionary<uint, byte[]> ParseIntelHex(string intelHex)
    {
      SortedDictionary<uint, byte[]> intelHex1 = new SortedDictionary<uint, byte[]>();
      string[] strArray = intelHex.Split('\r');
      string empty1 = string.Empty;
      sbyte chkSUM = 0;
      uint num1 = 0;
      string empty2 = string.Empty;
      string str1 = string.Empty;
      string str2 = string.Empty;
      foreach (string str3 in strArray)
      {
        string empty3 = string.Empty;
        if (str3.Contains(":"))
        {
          string str4 = str3.Replace("\n", "");
          byte[] numArray = new byte[0];
          uint length = uint.Parse(str4.Substring(1, 2), NumberStyles.HexNumber) * 2U;
          string s1 = str4.Substring(3, 4);
          uint num2 = uint.Parse(str4.Substring(7, 2), NumberStyles.HexNumber);
          if (num2 == 0U)
          {
            numArray = Util.HexStringToByteArray(str4.Substring(9, (int) length));
            chkSUM = sbyte.Parse(str4.Substring(9 + (int) length, 2), NumberStyles.HexNumber);
          }
          if (num2 == 1U)
          {
            chkSUM = sbyte.Parse(str4.Substring(9), NumberStyles.HexNumber);
            if (chkSUM == (sbyte) -1)
              break;
          }
          if (num2 == 2U)
          {
            num1 = uint.Parse(str4.Substring(9, 4), NumberStyles.HexNumber) * 16U;
            chkSUM = sbyte.Parse(str4.Substring(13, 2), NumberStyles.HexNumber);
          }
          if (num2 == 3U)
          {
            uint num3 = uint.Parse(str4.Substring(9, 4), NumberStyles.HexNumber);
            uint num4 = uint.Parse(str4.Substring(13, 4), NumberStyles.HexNumber);
            chkSUM = sbyte.Parse(str4.Substring(17, 2), NumberStyles.HexNumber);
            num1 = num3 * 16U + num4;
          }
          if (num2 == 4U)
          {
            str1 = str4.Substring(9, 4);
            chkSUM = sbyte.Parse(str4.Substring(13, 2), NumberStyles.HexNumber);
          }
          if (num2 == 5U)
          {
            str2 = str4.Substring(9, 8);
            chkSUM = sbyte.Parse(str4.Substring(17, 2), NumberStyles.HexNumber);
          }
          if (!Util.IsChecksumOK(Util.HexStringToByteArray(str4.Substring(1, str4.Length - 3)), chkSUM))
            throw new Exception("CHECKSUM ERROR in firmware file !!!\nAddress: " + s1 + "\nPlease check the firmware file and replace with working file.\n");
          string s2 = string.IsNullOrEmpty(str2) ? str1 + s1 : str2;
          uint key = num1 != 0U ? num1 + uint.Parse(s1, NumberStyles.HexNumber) : uint.Parse(s2, NumberStyles.HexNumber);
          if (num2 == 0U && !intelHex1.ContainsKey(key + num1))
            intelHex1.Add(key, numArray);
        }
      }
      return intelHex1;
    }

    private static bool IsChecksumOK(byte[] chkBA, sbyte chkSUM)
    {
      byte num1 = 0;
      foreach (byte num2 in chkBA)
        num1 += num2;
      sbyte num3 = (sbyte) (((int) (sbyte) ((int) num1 & (int) byte.MaxValue) ^ (int) byte.MaxValue) + 1);
      return (int) chkSUM == (int) num3;
    }

    public static bool IsValidTimePoint(int year, int month, int day, int hour, int minute)
    {
      return year >= 0 && year <= 2060 && month >= 1 && month <= 12 && day >= 1 && day <= 31 && hour >= 0 && hour <= 24 && minute >= 0 && minute <= 60;
    }

    public static bool IsValidBCD(string bcdStr)
    {
      foreach (char ch in bcdStr)
      {
        if (ch < '0' || ch > '9')
          return false;
      }
      return true;
    }

    public static bool TryParseToDateTime(string strValue, out DateTime value)
    {
      value = DateTime.MinValue;
      try
      {
        value = DateTime.Parse(strValue);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static bool ByteArrayCompare(List<byte> a1, byte[] a2, int size)
    {
      if (a1.Count < size || a2.Length < size)
        return false;
      for (int index = 0; index < size; ++index)
      {
        if ((int) a1[index] != (int) a2[index])
          return false;
      }
      return true;
    }

    public static bool ArrayStartWith(byte[] array, byte[] prefix)
    {
      if (array == prefix)
        return true;
      if (array == null || prefix == null || array.Length < prefix.Length)
        return false;
      for (int index = 0; index < prefix.Length; ++index)
      {
        if ((int) array[index] != (int) prefix[index])
          return false;
      }
      return true;
    }

    public static bool ArraysEqual(byte[] a1, byte[] a2)
    {
      if (a1 == a2)
        return true;
      if (a1 == null || a2 == null || a1.Length != a2.Length)
        return false;
      for (int index = 0; index < a1.Length; ++index)
      {
        if ((int) a1[index] != (int) a2[index])
          return false;
      }
      return true;
    }

    public static byte[] ObjectToByteArray(object obj)
    {
      if (obj == null)
        return (byte[]) null;
      using (MemoryStream serializationStream = new MemoryStream())
      {
        new BinaryFormatter().Serialize((Stream) serializationStream, obj);
        return serializationStream.ToArray();
      }
    }

    public static object ByteArrayToObject(byte[] bytes)
    {
      using (MemoryStream serializationStream = new MemoryStream())
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        serializationStream.Write(bytes, 0, bytes.Length);
        serializationStream.Seek(0L, SeekOrigin.Begin);
        return binaryFormatter.Deserialize((Stream) serializationStream);
      }
    }

    public static byte[] ReadToEnd(Stream stream)
    {
      long num1 = 0;
      if (stream.CanSeek)
      {
        num1 = stream.Position;
        stream.Position = 0L;
      }
      try
      {
        byte[] numArray = new byte[4096];
        int length = 0;
        int num2;
        while ((num2 = stream.Read(numArray, length, numArray.Length - length)) > 0)
        {
          length += num2;
          if (length == numArray.Length)
          {
            int num3 = stream.ReadByte();
            if (num3 != -1)
            {
              byte[] dst = new byte[numArray.Length * 2];
              Buffer.BlockCopy((Array) numArray, 0, (Array) dst, 0, numArray.Length);
              Buffer.SetByte((Array) dst, length, (byte) num3);
              numArray = dst;
              ++length;
            }
          }
        }
        byte[] dst1 = numArray;
        if (numArray.Length != length)
        {
          dst1 = new byte[length];
          Buffer.BlockCopy((Array) numArray, 0, (Array) dst1, 0, length);
        }
        return dst1;
      }
      finally
      {
        if (stream.CanSeek)
          stream.Position = num1;
      }
    }

    public static bool IsHex(IEnumerable<char> chars)
    {
      foreach (char ch in chars)
      {
        if ((ch < '0' || ch > '9') && (ch < 'a' || ch > 'f') && (ch < 'A' || ch > 'F'))
          return false;
      }
      return true;
    }

    public static P UnitToParameter(string unit)
    {
      switch (unit)
      {
        case "L":
          return P.PRM_Volume;
        case "Wh":
        case "kJ":
          return P.PRM_Energy;
        default:
          return P.PRM_Pulse;
      }
    }
  }
}
