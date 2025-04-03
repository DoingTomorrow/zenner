// Decompiled with JetBrains decompiler
// Type: HandlerLib.ByteArrayScanner
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Text;

#nullable disable
namespace HandlerLib
{
  public class ByteArrayScanner
  {
    public static string ScanString(byte[] byteArray, ref int offset)
    {
      if (byteArray == null)
        throw new ArgumentException("byteArray == null");
      if (offset < 0)
        throw new ArgumentException("offset < 0");
      int index = offset;
      while (true)
      {
        if (byteArray.Length > offset)
        {
          if (byteArray[offset] != (byte) 0)
            ++offset;
          else
            goto label_7;
        }
        else
          break;
      }
      throw new Exception("End of ASCII string not found");
label_7:
      string str = Encoding.ASCII.GetString(byteArray, index, offset - index);
      ++offset;
      return str;
    }

    public static DateTime ScanDate(byte[] byteArray, ref int offset)
    {
      ByteArrayScanner.CheckArray(byteArray, offset, 3);
      try
      {
        DateTime dateTime = new DateTime((int) byteArray[offset] + 2000, (int) byteArray[offset + 1], (int) byteArray[offset + 2], 0, 0, 0, 0, DateTimeKind.Local);
        offset += 3;
        return dateTime;
      }
      catch (Exception ex)
      {
        throw new Exception("Illegal Date stamp", ex);
      }
    }

    public static DateTime ScanDateTime(byte[] byteArray, ref int offset)
    {
      ByteArrayScanner.CheckArray(byteArray, offset, 6);
      try
      {
        DateTime dateTime = new DateTime((int) byteArray[offset] + 2000, (int) byteArray[offset + 1], (int) byteArray[offset + 2], (int) byteArray[offset + 3], (int) byteArray[offset + 4], (int) byteArray[offset + 5]);
        offset += 6;
        return dateTime;
      }
      catch (Exception ex)
      {
        throw new Exception("Illegal DateTime stamp", ex);
      }
    }

    public static DateTimeOffset ScanDateTimeOffset(byte[] byteArray, ref int offset)
    {
      ByteArrayScanner.CheckArray(byteArray, offset, 7);
      try
      {
        TimeSpan offset1 = new TimeSpan(0, (int) (sbyte) byteArray[offset + 6] * 15, 0);
        DateTimeOffset dateTimeOffset = new DateTimeOffset((int) byteArray[offset] + 2000, (int) byteArray[offset + 1], (int) byteArray[offset + 2], (int) byteArray[offset + 3], (int) byteArray[offset + 4], (int) byteArray[offset + 5], offset1);
        offset += 6;
        return dateTimeOffset;
      }
      catch (Exception ex)
      {
        throw new Exception("Illegal DateTime stamp", ex);
      }
    }

    public static byte ScanByte(byte[] byteArray, ref int offset)
    {
      ByteArrayScanner.CheckArray(byteArray, offset, 1);
      byte num = byteArray[offset];
      ++offset;
      return num;
    }

    public static sbyte ScanSByte(byte[] byteArray, ref int offset)
    {
      ByteArrayScanner.CheckArray(byteArray, offset, 1);
      sbyte num = (sbyte) byteArray[offset];
      ++offset;
      return num;
    }

    public static ushort ScanUInt16(byte[] byteArray, ref int offset)
    {
      ByteArrayScanner.CheckArray(byteArray, offset, 2);
      ushort uint16 = BitConverter.ToUInt16(byteArray, offset);
      offset += 2;
      return uint16;
    }

    public static short ScanInt16(byte[] byteArray, ref int offset)
    {
      ByteArrayScanner.CheckArray(byteArray, offset, 2);
      short int16 = BitConverter.ToInt16(byteArray, offset);
      offset += 2;
      return int16;
    }

    public static uint ScanUInt32(byte[] byteArray, ref int offset)
    {
      ByteArrayScanner.CheckArray(byteArray, offset, 4);
      uint uint32 = BitConverter.ToUInt32(byteArray, offset);
      offset += 4;
      return uint32;
    }

    public static int ScanInt32(byte[] byteArray, ref int offset)
    {
      ByteArrayScanner.CheckArray(byteArray, offset, 4);
      int int32 = BitConverter.ToInt32(byteArray, offset);
      offset += 4;
      return int32;
    }

    public static ulong ScanUInt64(byte[] byteArray, ref int offset)
    {
      ByteArrayScanner.CheckArray(byteArray, offset, 8);
      ulong uint64 = BitConverter.ToUInt64(byteArray, offset);
      offset += 8;
      return uint64;
    }

    public static long ScanInt64(byte[] byteArray, ref int offset)
    {
      ByteArrayScanner.CheckArray(byteArray, offset, 8);
      long int64 = BitConverter.ToInt64(byteArray, offset);
      offset += 8;
      return int64;
    }

    public static float ScanFloat(byte[] byteArray, ref int offset)
    {
      ByteArrayScanner.CheckArray(byteArray, offset, 4);
      float single = BitConverter.ToSingle(byteArray, offset);
      offset += 4;
      return single;
    }

    public static double ScanDouble(byte[] byteArray, ref int offset)
    {
      ByteArrayScanner.CheckArray(byteArray, offset, 8);
      double num = BitConverter.ToDouble(byteArray, offset);
      offset += 8;
      return num;
    }

    private static void CheckArray(byte[] byteArray, int offset, int minByteSize)
    {
      if (byteArray == null)
        throw new ArgumentException("byteArray == null");
      if (offset < 0)
        throw new ArgumentException("offset < 0");
      if (offset > byteArray.Length - minByteSize)
        throw new ArgumentException("out of array size");
    }

    public static void ScanInString(byte[] byteArray, string theString, ref int offset)
    {
      byte[] bytes = Encoding.ASCII.GetBytes(theString);
      if (byteArray.Length < offset + bytes.Length + 1)
        throw new Exception("Size of byteArray to small");
      Array.Copy((Array) bytes, 0, (Array) byteArray, offset, bytes.Length);
      offset += bytes.Length;
      byteArray[offset++] = (byte) 0;
    }

    public static void ScanInByte(byte[] byteArray, byte theByte, ref int offset)
    {
      if (byteArray.Length < offset + 1)
        throw new Exception("Size of byteArray to small");
      byteArray[offset++] = theByte;
    }

    public static void ScanInSByte(byte[] byteArray, sbyte theByte, ref int offset)
    {
      if (byteArray.Length < offset + 1)
        throw new Exception("Size of byteArray to small");
      byteArray[offset++] = (byte) theByte;
    }

    public static void ScanInUInt16(byte[] byteArray, ushort theValue, ref int offset)
    {
      if (byteArray.Length < offset + 2)
        throw new Exception("Size of byteArray to small");
      Array.Copy((Array) BitConverter.GetBytes(theValue), 0, (Array) byteArray, offset, 2);
      offset += 2;
    }

    public static void ScanInInt16(byte[] byteArray, short theValue, ref int offset)
    {
      if (byteArray.Length < offset + 2)
        throw new Exception("Size of byteArray to small");
      Array.Copy((Array) BitConverter.GetBytes(theValue), 0, (Array) byteArray, offset, 2);
      offset += 2;
    }

    public static void ScanInUInt32(byte[] byteArray, uint theValue, ref int offset)
    {
      if (byteArray.Length < offset + 4)
        throw new Exception("Size of byteArray to small");
      Array.Copy((Array) BitConverter.GetBytes(theValue), 0, (Array) byteArray, offset, 4);
      offset += 4;
    }

    public static void ScanInInt32(byte[] byteArray, int theValue, ref int offset)
    {
      if (byteArray.Length < offset + 4)
        throw new Exception("Size of byteArray to small");
      Array.Copy((Array) BitConverter.GetBytes(theValue), 0, (Array) byteArray, offset, 4);
      offset += 4;
    }

    public static void ScanInUInt64(byte[] byteArray, ulong theValue, ref int offset)
    {
      if (byteArray.Length < offset + 8)
        throw new Exception("Size of byteArray to small");
      Array.Copy((Array) BitConverter.GetBytes(theValue), 0, (Array) byteArray, offset, 8);
      offset += 8;
    }

    public static void ScanInInt64(byte[] byteArray, long theValue, ref int offset)
    {
      if (byteArray.Length < offset + 8)
        throw new Exception("Size of byteArray to small");
      Array.Copy((Array) BitConverter.GetBytes(theValue), 0, (Array) byteArray, offset, 8);
      offset += 8;
    }

    public static void ScanInFloat(byte[] byteArray, float theValue, ref int offset)
    {
      if (byteArray.Length < offset + 4)
        throw new Exception("Size of byteArray to small");
      Array.Copy((Array) BitConverter.GetBytes(theValue), 0, (Array) byteArray, offset, 4);
      offset += 4;
    }

    public static void ScanInDouble(byte[] byteArray, double theValue, ref int offset)
    {
      if (byteArray.Length < offset + 8)
        throw new Exception("Size of byteArray to small");
      Array.Copy((Array) BitConverter.GetBytes(theValue), 0, (Array) byteArray, offset, 8);
      offset += 8;
    }

    public static void ScanInDate(byte[] byteArray, DateTime theValue, ref int offset)
    {
      if (byteArray.Length < offset + 3)
        throw new Exception("Size of byteArray to small");
      byte num = (byte) (theValue.Year - 2000);
      byte month = (byte) theValue.Month;
      byte day = (byte) theValue.Day;
      byteArray[offset++] = num;
      byteArray[offset++] = month;
      byteArray[offset++] = day;
    }

    public static void ScanInDateTime(byte[] byteArray, DateTime theValue, ref int offset)
    {
      if (byteArray.Length < offset + 6)
        throw new Exception("Size of byteArray to small");
      ByteArrayScanner.ScanInDate(byteArray, theValue, ref offset);
      byte hour = (byte) theValue.Hour;
      byte minute = (byte) theValue.Minute;
      byte second = (byte) theValue.Second;
      byteArray[offset++] = hour;
      byteArray[offset++] = minute;
      byteArray[offset++] = second;
    }

    public static void ScanInDateTime2000(byte[] byteArray, DateTime theValue, ref int offset)
    {
      if (byteArray.Length < offset + 4)
        throw new Exception("Size of byteArray to small");
      uint meterTime = CalendarBase2000.Cal_GetMeterTime(theValue);
      ByteArrayScanner.ScanInUInt32(byteArray, meterTime, ref offset);
    }

    public static void ScanInDateTimeOffset(
      byte[] byteArray,
      DateTimeOffset theValue,
      ref int offset)
    {
      if (byteArray.Length < offset + 6)
        throw new Exception("Size of byteArray to small");
      ByteArrayScanner.ScanInDateTime(byteArray, theValue.DateTime, ref offset);
      sbyte num = (sbyte) (theValue.Offset.TotalMinutes / 15.0);
      byteArray[offset++] = (byte) num;
    }
  }
}
