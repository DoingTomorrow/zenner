// Decompiled with JetBrains decompiler
// Type: HandlerLib.MapManagement.LoadDataFromMapClass
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Text;

#nullable disable
namespace HandlerLib.MapManagement
{
  public static class LoadDataFromMapClass
  {
    public static string GetString(byte[] byteArray, ref int readIndex)
    {
      if (readIndex >= byteArray.Length)
        throw new IndexOutOfRangeException("LoadBaseDataFromByteArray.GetString from index: " + readIndex.ToString());
      return byteArray[readIndex++] == (byte) 0 ? (string) null : LoadDataFromMapClass.GetStringForced(byteArray, ref readIndex);
    }

    public static string GetStringForced(byte[] byteArray, ref int readIndex)
    {
      int length = LoadDataFromMapClass.GetLength(byteArray, ref readIndex);
      if (length == 0)
        return "";
      if (readIndex + length > byteArray.Length)
        throw new IndexOutOfRangeException("LoadBaseDataFromByteArray.GetStringForced from index: " + readIndex.ToString());
      string stringForced = Encoding.UTF8.GetString(byteArray, readIndex, length);
      readIndex += length;
      return stringForced;
    }

    public static byte GetByteForced(byte[] byteArray, ref int readIndex)
    {
      if (readIndex < byteArray.Length)
        return byteArray[readIndex++];
      throw new IndexOutOfRangeException("LoadBaseDataFromByteArray.GetByte from index: " + readIndex.ToString());
    }

    public static byte? GetByte(byte[] byteArray, ref int readIndex)
    {
      if (readIndex < byteArray.Length)
      {
        if (byteArray[readIndex++] == (byte) 0)
          return new byte?();
        if (readIndex < byteArray.Length)
          return new byte?(byteArray[readIndex++]);
      }
      throw new IndexOutOfRangeException("LoadBaseDataFromByteArray.GetByte from index: " + readIndex.ToString());
    }

    public static byte[] GetBytes(byte[] byteArray, ref int readIndex)
    {
      if (readIndex >= byteArray.Length)
        throw new IndexOutOfRangeException("LoadBaseDataFromByteArray.GetBytes from index: " + readIndex.ToString());
      return byteArray[readIndex++] == (byte) 0 ? (byte[]) null : LoadDataFromMapClass.GetBytesForced(byteArray, ref readIndex);
    }

    public static byte[] GetBytesForced(byte[] byteArray, ref int readIndex)
    {
      int length = LoadDataFromMapClass.GetLength(byteArray, ref readIndex);
      if (readIndex + length > byteArray.Length)
        throw new IndexOutOfRangeException("LoadBaseDataFromByteArray.GetBytesForced from index: " + readIndex.ToString());
      byte[] bytesForced = new byte[length];
      for (int index = 0; index < length; ++index)
        bytesForced[index] = byteArray[readIndex + index];
      readIndex += length;
      return bytesForced;
    }

    public static int? GetInt(byte[] byteArray, ref int readIndex)
    {
      if (readIndex >= byteArray.Length)
        throw new IndexOutOfRangeException("LoadBaseDataFromByteArray.GetInt from index: " + readIndex.ToString());
      return byteArray[readIndex++] == (byte) 0 ? new int?() : new int?(LoadDataFromMapClass.GetIntForced(byteArray, ref readIndex));
    }

    public static int GetIntForced(byte[] byteArray, ref int readIndex)
    {
      if (readIndex + 4 > byteArray.Length)
        throw new IndexOutOfRangeException("LoadBaseDataFromByteArray.GetIntForced from index: " + readIndex.ToString());
      int int32 = BitConverter.ToInt32(byteArray, readIndex);
      readIndex += 4;
      return int32;
    }

    public static uint GetUIntForced(byte[] byteArray, ref int readIndex)
    {
      if (readIndex + 4 > byteArray.Length)
        throw new IndexOutOfRangeException("LoadBaseDataFromByteArray.GetIntForced from index: " + readIndex.ToString());
      uint uint32 = BitConverter.ToUInt32(byteArray, readIndex);
      readIndex += 4;
      return uint32;
    }

    public static ushort? GetUShort(byte[] byteArray, ref int readIndex)
    {
      if (readIndex >= byteArray.Length)
        throw new IndexOutOfRangeException("LoadBaseDataFromByteArray.GetUShort from index: " + readIndex.ToString());
      return byteArray[readIndex++] == (byte) 0 ? new ushort?() : new ushort?(LoadDataFromMapClass.GetUShortForced(byteArray, ref readIndex));
    }

    public static ushort GetUShortForced(byte[] byteArray, ref int readIndex)
    {
      if (readIndex + 2 > byteArray.Length)
        throw new IndexOutOfRangeException("LoadBaseDataFromByteArray.GetUShortForced from index: " + readIndex.ToString());
      ushort uint16 = BitConverter.ToUInt16(byteArray, readIndex);
      readIndex += 2;
      return uint16;
    }

    public static bool? GetBool(byte[] byteArray, ref int readIndex)
    {
      if (readIndex < byteArray.Length)
      {
        if (byteArray[readIndex++] == (byte) 0)
          return new bool?();
        if (readIndex < byteArray.Length)
          return byteArray[readIndex++] > (byte) 0 ? new bool?(true) : new bool?(false);
      }
      throw new IndexOutOfRangeException("LoadBaseDataFromByteArray.GetBool from index: " + readIndex.ToString());
    }

    public static double? GetDouble(byte[] byteArray, ref int readIndex)
    {
      if (readIndex < byteArray.Length)
      {
        if (byteArray[readIndex++] == (byte) 0)
          return new double?();
        if (readIndex + 8 <= byteArray.Length)
        {
          double num = BitConverter.ToDouble(byteArray, readIndex);
          readIndex += 8;
          return new double?(num);
        }
      }
      throw new IndexOutOfRangeException("LoadBaseDataFromByteArray.GetDouble from index: " + readIndex.ToString());
    }

    public static int GetLength(byte[] byteArray, ref int readIndex)
    {
      int length = (int) byteArray[readIndex++];
      if (length == (int) byte.MaxValue)
      {
        if (readIndex + 2 <= byteArray.Length)
        {
          length = (int) byteArray[readIndex++] + ((int) byteArray[readIndex++] << 8);
          if (length == (int) ushort.MaxValue)
          {
            if (readIndex + 4 <= byteArray.Length)
              length = BitConverter.ToInt32(byteArray, readIndex);
            else
              goto label_7;
          }
          goto label_6;
        }
label_7:
        throw new IndexOutOfRangeException("LoadBaseDataFromByteArray.GetDouble from index: " + readIndex.ToString());
      }
label_6:
      return length;
    }

    public static uint GetSectionStartAddressForSection(
      string[] SectionList,
      uint[] SectionAddress,
      string Section)
    {
      uint addressForSection = 0;
      int index = Array.IndexOf<string>(SectionList, Section);
      if (index >= 0)
        addressForSection = SectionAddress[index];
      return addressForSection;
    }

    public static uint GetSectionSizeFor(string[] SectionList, uint[] SectionSize, string Section)
    {
      uint sectionSizeFor = 0;
      int index = Array.IndexOf<string>(SectionList, Section);
      if (index >= 0)
        sectionSizeFor = SectionSize[index];
      return sectionSizeFor;
    }
  }
}
