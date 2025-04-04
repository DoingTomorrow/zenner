// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.ByteArrayScanner16Bit
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using HandlerLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace SmartFunctionCompiler
{
  public static class ByteArrayScanner16Bit
  {
    public static string ScanString(byte[] byteArray, ref ushort offset)
    {
      if (Interpreter.IsError)
        return (string) null;
      if (byteArray == null)
      {
        Interpreter.Error = SmartFunctionResult.NoData;
        return (string) null;
      }
      ushort index = offset;
      int count;
      while (true)
      {
        if (byteArray.Length > (int) offset)
        {
          count = (int) offset - (int) index;
          if (count <= 20)
          {
            if (byteArray[(int) offset] != (byte) 0)
              ++offset;
            else
              goto label_9;
          }
          else
            goto label_7;
        }
        else
          break;
      }
      Interpreter.Error = SmartFunctionResult.EndOfAsciiStringNotFound;
      return (string) null;
label_7:
      Interpreter.Error = SmartFunctionResult.StringLength20Exceeded;
      return (string) null;
label_9:
      string str = Encoding.ASCII.GetString(byteArray, (int) index, count);
      ++offset;
      return str;
    }

    public static DateTime ScanDate(byte[] byteArray, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 3);
      if (Interpreter.IsError)
        return DateTime.MinValue;
      try
      {
        DateTime dateTime = new DateTime((int) byteArray[(int) offset], (int) byteArray[(int) offset + 1], (int) byteArray[(int) offset + 2]);
        offset += (ushort) 3;
        return dateTime;
      }
      catch
      {
        Interpreter.Error = SmartFunctionResult.IllegalDateStamp;
        return DateTime.MinValue;
      }
    }

    public static DateTime ScanDateTime(byte[] byteArray, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 6);
      if (Interpreter.IsError)
        return DateTime.MinValue;
      try
      {
        DateTime dateTime = new DateTime((int) byteArray[(int) offset], (int) byteArray[(int) offset + 1], (int) byteArray[(int) offset + 2], (int) byteArray[(int) offset + 3], (int) byteArray[(int) offset + 4], (int) byteArray[(int) offset + 5]);
        offset += (ushort) 6;
        return dateTime;
      }
      catch
      {
        Interpreter.Error = SmartFunctionResult.IllegalDateTimeStamp;
        return DateTime.MinValue;
      }
    }

    public static DateTime ScanDateTimeBase2000(byte[] byteArray, ref ushort offset)
    {
      try
      {
        uint TheTime = ByteArrayScanner16Bit.ScanUInt32(byteArray, ref offset);
        return Interpreter.IsError ? DateTime.MinValue : CalendarBase2000.Cal_GetDateTime(TheTime);
      }
      catch
      {
        Interpreter.Error = SmartFunctionResult.IllegalDateTimeStamp;
        return DateTime.MinValue;
      }
    }

    public static sbyte ScanSByte(byte[] byteArray, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 1);
      if (Interpreter.IsError)
        return 0;
      sbyte num = (sbyte) byteArray[(int) offset];
      ++offset;
      return num;
    }

    public static byte ScanByte(byte[] byteArray, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 1);
      if (Interpreter.IsError)
        return 0;
      byte num = byteArray[(int) offset];
      ++offset;
      return num;
    }

    public static short ScanInt16(byte[] byteArray, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 2);
      if (Interpreter.IsError)
        return 0;
      short int16 = BitConverter.ToInt16(byteArray, (int) offset);
      offset += (ushort) 2;
      return int16;
    }

    public static ushort ScanUInt16(byte[] byteArray, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 2);
      if (Interpreter.IsError)
        return 0;
      ushort uint16 = BitConverter.ToUInt16(byteArray, (int) offset);
      offset += (ushort) 2;
      return uint16;
    }

    public static int ScanInt32(byte[] byteArray, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 4);
      if (Interpreter.IsError)
        return 0;
      int int32 = BitConverter.ToInt32(byteArray, (int) offset);
      offset += (ushort) 4;
      return int32;
    }

    public static uint ScanUInt32(byte[] byteArray, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 4);
      if (Interpreter.IsError)
        return 0;
      uint uint32 = BitConverter.ToUInt32(byteArray, (int) offset);
      offset += (ushort) 4;
      return uint32;
    }

    public static long ScanInt64(byte[] byteArray, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 8);
      if (Interpreter.IsError)
        return 0;
      long int64 = BitConverter.ToInt64(byteArray, (int) offset);
      offset += (ushort) 8;
      return int64;
    }

    public static ulong ScanUInt64(byte[] byteArray, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 8);
      if (Interpreter.IsError)
        return 0;
      ulong uint64 = BitConverter.ToUInt64(byteArray, (int) offset);
      offset += (ushort) 8;
      return uint64;
    }

    public static float ScanFloat(byte[] byteArray, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 4);
      if (Interpreter.IsError)
        return 0.0f;
      float single = BitConverter.ToSingle(byteArray, (int) offset);
      offset += (ushort) 4;
      return single;
    }

    public static double ScanDouble(byte[] byteArray, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 1);
      if (Interpreter.IsError)
        return 0.0;
      double num = BitConverter.ToDouble(byteArray, (int) offset);
      offset += (ushort) 8;
      return num;
    }

    public static byte[] HexStringToByteArray(string hexString)
    {
      List<byte> byteList = new List<byte>();
      hexString = hexString.Trim();
      if ((hexString.Length & 1) != 0)
        throw new Exception("Illegal number of nibbles for hex string");
      for (int startIndex = 0; startIndex < hexString.Length; startIndex += 2)
        byteList.Add(byte.Parse(hexString.Substring(startIndex, 2), NumberStyles.HexNumber));
      return byteList.ToArray();
    }

    public static void ScanInString(byte[] byteArray, string theString, ref ushort offset)
    {
      byte[] bytes = Encoding.ASCII.GetBytes(theString);
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, bytes.Length + 1);
      if (Interpreter.IsError)
        return;
      Array.Copy((Array) bytes, 0, (Array) byteArray, (int) offset, bytes.Length);
      offset += (ushort) bytes.Length;
      byteArray[(int) offset++] = (byte) 0;
    }

    public static void ScanInByte(byte[] byteArray, byte theByte, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 1);
      if (Interpreter.IsError)
        return;
      byteArray[(int) offset++] = theByte;
    }

    public static void ScanInBytes(byte[] byteArray, byte[] theBytes, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, theBytes.Length);
      if (Interpreter.IsError)
        return;
      for (int index = 0; index < theBytes.Length; ++index)
        byteArray[(int) offset++] = theBytes[index];
    }

    public static void ScanInUInt16(byte[] byteArray, ushort theValue, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 2);
      if (Interpreter.IsError)
        return;
      Array.Copy((Array) BitConverter.GetBytes(theValue), 0, (Array) byteArray, (int) offset, 2);
      offset += (ushort) 2;
    }

    public static void ScanInUInt32(byte[] byteArray, uint theValue, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 4);
      if (Interpreter.IsError)
        return;
      Array.Copy((Array) BitConverter.GetBytes(theValue), 0, (Array) byteArray, (int) offset, 4);
      offset += (ushort) 4;
    }

    public static void ScanInDate(byte[] byteArray, DateTime theValue, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 3);
      if (Interpreter.IsError)
        return;
      byte num = (byte) (theValue.Year - 2000);
      byte month = (byte) theValue.Month;
      byte day = (byte) theValue.Day;
      byteArray[(int) offset++] = num;
      byteArray[(int) offset++] = month;
      byteArray[(int) offset++] = day;
    }

    public static void ScanInDateTime(byte[] byteArray, DateTime theValue, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 6);
      if (Interpreter.IsError)
        return;
      ByteArrayScanner16Bit.ScanInDate(byteArray, theValue, ref offset);
      byte hour = (byte) theValue.Hour;
      byte minute = (byte) theValue.Minute;
      byte second = (byte) theValue.Second;
      byteArray[(int) offset++] = hour;
      byteArray[(int) offset++] = minute;
      byteArray[(int) offset++] = second;
    }

    public static void ScanInDateTime2000(byte[] byteArray, DateTime theValue, ref ushort offset)
    {
      ByteArrayScanner16Bit.CheckArray(byteArray, offset, 4);
      if (Interpreter.IsError)
        return;
      uint meterTime = CalendarBase2000.Cal_GetMeterTime(theValue);
      ByteArrayScanner16Bit.ScanInUInt32(byteArray, meterTime, ref offset);
    }

    private static void CheckArray(byte[] byteArray, ushort offset, int minByteSize)
    {
      if (Interpreter.IsError)
        return;
      if (byteArray == null)
      {
        Interpreter.Error = SmartFunctionResult.NoData;
      }
      else
      {
        if ((int) offset <= byteArray.Length - minByteSize)
          return;
        if (byteArray == FunctionLoader.FlashStorage)
          Interpreter.Error = SmartFunctionResult.FlashOutOfMemory;
        else if (byteArray == FunctionLoader.RamStorage)
          Interpreter.Error = SmartFunctionResult.RamOutOfMemory;
        else if (byteArray == FunctionLoader.BackupStorage)
          Interpreter.Error = SmartFunctionResult.BackupOutOfMemory;
        else
          Interpreter.Error = SmartFunctionResult.OutOfMemory;
      }
    }
  }
}
