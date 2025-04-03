// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Utility
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace ZENNER.CommonLibrary
{
  public static class Utility
  {
    public static byte[] GetRandomAppKey()
    {
      byte[] data = new byte[16];
      new RNGCryptoServiceProvider().GetBytes(data);
      return data;
    }

    public static string ConvertSerialNumberToEUI64(Utility.LoraDevice device, int serialnumber)
    {
      List<byte> byteList = new List<byte>()
      {
        (byte) 4,
        (byte) 182,
        (byte) 72,
        (byte) 0
      };
      byteList.AddRange((IEnumerable<byte>) ((IEnumerable<byte>) BitConverter.GetBytes(Utility.ConvertUnt32ToBcdUInt32((uint) serialnumber))).Reverse<byte>().ToArray<byte>());
      switch (device)
      {
        case Utility.LoraDevice.M8:
          byteList[3] = (byte) 253;
          byteList[4] |= (byte) 128;
          return Utility.ByteArrayToHexString(byteList.ToArray());
        case Utility.LoraDevice.EDC:
          byteList[3] = (byte) 4;
          byteList[4] |= (byte) 80;
          return Utility.ByteArrayToHexString(byteList.ToArray());
        default:
          throw new NotImplementedException();
      }
    }

    public static string ConvertSerialNumberToFullserialNumber(
      Utility.LoraDevice device,
      int serialnumber)
    {
      switch (device)
      {
        case Utility.LoraDevice.M8:
          return "4ZRIFD8" + Utility.ByteArrayToHexString(((IEnumerable<byte>) BitConverter.GetBytes(Utility.ConvertUnt32ToBcdUInt32((uint) serialnumber))).Reverse<byte>().ToArray<byte>()).Substring(1);
        case Utility.LoraDevice.EDC:
          return "EZRI045" + Utility.ByteArrayToHexString(((IEnumerable<byte>) BitConverter.GetBytes(Utility.ConvertUnt32ToBcdUInt32((uint) serialnumber))).Reverse<byte>().ToArray<byte>()).Substring(1);
        default:
          throw new NotImplementedException();
      }
    }

    public static string ByteArrayToFullSerialNumber_V1(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (buffer.Length != 8)
        throw new ArgumentOutOfRangeException(nameof (buffer), "Expected 8 bytes");
      if (buffer[0] == byte.MaxValue && buffer[1] == byte.MaxValue && buffer[2] == byte.MaxValue && buffer[3] == byte.MaxValue && buffer[4] == byte.MaxValue && buffer[5] == byte.MaxValue && buffer[6] == byte.MaxValue && buffer[7] == byte.MaxValue)
        return string.Empty;
      if (buffer[0] == (byte) 0 && buffer[1] == (byte) 0 && buffer[2] == (byte) 0 && buffer[3] == (byte) 0 && buffer[4] == (byte) 0 && buffer[5] == (byte) 0 && buffer[6] == (byte) 0 && buffer[7] == (byte) 0)
        return string.Empty;
      try
      {
        string str = Encoding.ASCII.GetString(buffer, 0, 1);
        string manufacturer = Utility.GetManufacturer(BitConverter.ToUInt16(buffer, 1));
        byte num = buffer[3];
        uint uint32 = BitConverter.ToUInt32(new byte[4]
        {
          buffer[7],
          buffer[6],
          buffer[5],
          buffer[4]
        }, 0);
        return string.Format("{0}{1}{2}{3}", (object) str, (object) manufacturer, (object) num.ToString("X2"), (object) uint32.ToString("X8"));
      }
      catch
      {
        return string.Empty;
      }
    }

    public static string ByteArrayToFullSerialNumber_V2(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      int index = buffer.Length - 1;
      while (index >= 0 && buffer[index] == (byte) 0)
        --index;
      byte[] numArray = new byte[index + 1];
      Array.Copy((Array) buffer, (Array) numArray, index + 1);
      return Encoding.ASCII.GetString(numArray);
    }

    public static byte[] FullSerialNumberToByteArray_V1(string value)
    {
      if (string.IsNullOrEmpty(value))
        return new byte[8]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        };
      string s = value.Length == 14 ? value.Substring(0, 1) : throw new ArgumentException("Wrong length of the full serialnumber detected! Expected 14, Value: " + value);
      string manufacturer = value.Substring(1, 3);
      byte num = byte.Parse(value.Substring(4, 2), NumberStyles.HexNumber);
      string str = value.Substring(6);
      if (!Utility.IsValidBCD(str))
        throw new ArgumentException("Wrong full serialnumber detected! The last 8 chars should contains only numbers. Value: " + value);
      List<byte> byteList = new List<byte>(8);
      byteList.AddRange((IEnumerable<byte>) Encoding.ASCII.GetBytes(s));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(Utility.GetManufacturerCode(manufacturer)));
      byteList.Add(num);
      byteList.AddRange((IEnumerable<byte>) ((IEnumerable<byte>) BitConverter.GetBytes(Utility.ConvertUnt32ToBcdUInt32(uint.Parse(str)))).Reverse<byte>().ToArray<byte>());
      return byteList.Count == 8 ? byteList.ToArray() : throw new ArgumentOutOfRangeException("Wrong full serialnumber was generated!");
    }

    public static byte[] FullSerialNumberToByteArray_V2(string value)
    {
      if (string.IsNullOrEmpty(value))
        return new byte[20];
      byte[] src = value.Length <= 16 ? Encoding.ASCII.GetBytes(value + " ") : throw new ArgumentException("Printed_serialnumber must be less as 16 characters.");
      src[src.Length - 1] = (byte) 0;
      byte[] dst = new byte[20];
      Buffer.BlockCopy((Array) src, 0, (Array) dst, 0, src.Length);
      return dst;
    }

    public static string GetManufacturer(ushort code)
    {
      return "" + ((char) (((int) code >> 10 & 31) + 64)).ToString() + ((char) (((int) code >> 5 & 31) + 64)).ToString() + ((char) (((int) code & 31) + 64)).ToString();
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

    public static ushort GetManufacturerCode(string manufacturer)
    {
      if (string.IsNullOrEmpty(manufacturer))
        manufacturer = string.Empty;
      manufacturer = manufacturer.PadRight(3);
      return (ushort) ((uint) (ushort) ((uint) (ushort) (0U + (uint) (ushort) ((uint) (byte) manufacturer[2] - 64U)) + (uint) (ushort) ((uint) (ushort) ((uint) (byte) manufacturer[1] - 64U) << 5)) + (uint) (ushort) ((uint) (ushort) ((uint) (byte) manufacturer[0] - 64U) << 10));
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
            str = Utility.ByteArrayToHexString((byte[]) buffer);
          }
          else
          {
            switch (buffer)
            {
              case IPrintable _:
                str = ((IPrintable) buffer).Print();
                break;
              case IList _:
                IEnumerator enumerator = ((IEnumerable) buffer).GetEnumerator();
                try
                {
                  while (enumerator.MoveNext())
                  {
                    object current = enumerator.Current;
                    str += Utility.PrintObject(current, spaces + 4);
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

    public static string PrintAvailableObjectProperties(object obj, int spaces = 0)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj))
      {
        string name = property.Name;
        object buffer = property.GetValue(obj);
        if (buffer != null)
        {
          string str = string.Empty;
          if (buffer != null)
          {
            if (buffer.GetType() == typeof (byte[]))
            {
              str = Utility.ByteArrayToHexString((byte[]) buffer);
            }
            else
            {
              switch (buffer)
              {
                case IPrintable _:
                  str = ((IPrintable) buffer).Print();
                  break;
                case IList _:
                  IEnumerator enumerator = ((IEnumerable) buffer).GetEnumerator();
                  try
                  {
                    while (enumerator.MoveNext())
                    {
                      object current = enumerator.Current;
                      str += Utility.PrintObject(current, spaces + 4);
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
      }
      return stringBuilder.ToString();
    }

    public static T DeepCopy<T>(T obj)
    {
      if ((object) obj == null)
        throw new ArgumentNullException(nameof (obj));
      using (MemoryStream serializationStream = new MemoryStream())
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize((Stream) serializationStream, (object) obj);
        serializationStream.Position = 0L;
        return (T) binaryFormatter.Deserialize((Stream) serializationStream);
      }
    }

    public static string ByteArrayToAsciiString(byte[] ba)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte num in ba)
        stringBuilder.Append((char) num);
      return stringBuilder.ToString();
    }

    public static string ByteArrayToHexString(byte[] buffer)
    {
      return buffer == null ? string.Empty : Utility.ByteArrayToHexString(buffer, 0, buffer.Length);
    }

    public static string ByteArrayToHexString(byte[] buffer, int startIndex)
    {
      return buffer == null ? string.Empty : Utility.ByteArrayToHexString(buffer, startIndex, buffer.Length - startIndex);
    }

    public static string ByteArrayToHexString(byte[] buffer, int startIndex, int length)
    {
      if (buffer == null)
        return string.Empty;
      char[] chArray = new char[length * 2];
      int num1 = 0;
      int index = 0;
      while (num1 < length)
      {
        byte num2 = (byte) ((uint) buffer[startIndex + num1] >> 4);
        chArray[index] = num2 > (byte) 9 ? (char) ((int) num2 + 55) : (char) ((int) num2 + 48);
        byte num3 = (byte) ((uint) buffer[startIndex + num1] & 15U);
        int num4;
        chArray[num4 = index + 1] = num3 > (byte) 9 ? (char) ((int) num3 + 55) : (char) ((int) num3 + 48);
        ++num1;
        index = num4 + 1;
      }
      return new string(chArray, 0, chArray.Length);
    }

    public static string ByteArrayToHexStringFormated(
      byte[] arr,
      string leftSpacesString,
      int maxLineBytes,
      uint? startAddress = null)
    {
      StringBuilder stringBuilder = new StringBuilder();
      uint num1 = 0;
      int num2 = 0;
      for (int index = 0; index < arr.Length; ++index)
      {
        if (index % maxLineBytes == 0)
        {
          num2 = 0;
          if (num1 > 0U)
            stringBuilder.AppendLine();
          if (leftSpacesString != null)
            stringBuilder.Append(leftSpacesString);
          if (startAddress.HasValue)
          {
            uint num3 = startAddress.Value + num1;
            stringBuilder.Append(num3.ToString("x08") + ": ");
          }
        }
        if (num2 > 0)
        {
          if ((num2 & 7) == 0)
            stringBuilder.Append(':');
          else if ((num2 & 3) == 0)
            stringBuilder.Append('.');
          else
            stringBuilder.Append(' ');
        }
        stringBuilder.Append(arr[index].ToString("x02"));
        ++num1;
        ++num2;
      }
      return stringBuilder.ToString();
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

    public static long DecodeBcd(byte[] data, int size)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      long num = 0;
      for (int index = size; index > 0; --index)
        num = (index != 0 || (int) data[0] >> 4 != 15 ? num * 10L + (long) ((int) data[index - 1] >> 4 & 15) : num * -1L) * 10L + (long) ((int) data[index - 1] & 15);
      return num;
    }

    public static byte[] HexStringToByteArray(string hex)
    {
      hex = hex.Replace(" ", string.Empty);
      hex = hex.Replace("-", string.Empty);
      hex = hex.Replace(":", string.Empty);
      int length = hex.Length;
      byte[] byteArray = length % 2 == 0 ? new byte[length / 2] : throw new ArgumentException("Hex-string is invalid!");
      for (int startIndex = 0; startIndex < length; startIndex += 2)
        byteArray[startIndex / 2] = Convert.ToByte(hex.Substring(startIndex, 2), 16);
      return byteArray;
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

    public static DateTime? ConvertToDateTime_SystemTime64(byte[] buffer, int startIndex)
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
      byte[] numArray7 = buffer;
      int index7 = num8;
      int num9 = index7 + 1;
      byte num10 = numArray7[index7];
      byte[] numArray8 = buffer;
      int index8 = num9;
      int num11 = index8 + 1;
      byte num12 = numArray8[index8];
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

    public static string ZeroTerminatedAsciiStringToString(byte[] zeroTerminatedAsciiString)
    {
      if (zeroTerminatedAsciiString == null)
        return (string) null;
      int count = -1;
      for (int index = 0; index < zeroTerminatedAsciiString.Length; ++index)
      {
        if (zeroTerminatedAsciiString[index] == (byte) 0)
        {
          count = index;
          break;
        }
      }
      return count <= 0 ? (string) null : Encoding.ASCII.GetString(zeroTerminatedAsciiString, 0, count);
    }

    public static byte[] StringToZeroTerminatedAsciiString(string theString, int maxLength)
    {
      if (string.IsNullOrEmpty(theString))
        return new byte[1];
      if (theString.Length >= maxLength)
        throw new Exception("StringToZeroTerminatedAsciiString: string to long for byte[] maxLength");
      byte[] bytes = Encoding.ASCII.GetBytes(theString);
      if (bytes.Length != theString.Length)
        throw new Exception("Illegal ASCII characters");
      Array.Resize<byte>(ref bytes, bytes.Length + 1);
      bytes[bytes.Length - 1] = (byte) 0;
      return bytes;
    }

    public enum LoraDevice
    {
      M8,
      EDC,
      PDC,
      THL,
      NFCL,
    }
  }
}
