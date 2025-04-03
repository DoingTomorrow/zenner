// Decompiled with JetBrains decompiler
// Type: HandlerLib.ZipUnzip
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.IO;
using System.IO.Compression;
using System.Text;

#nullable disable
namespace HandlerLib
{
  public static class ZipUnzip
  {
    public static byte[] GetZipedBytesFromString(string theString)
    {
      byte[] zipedBytes = ZipUnzip.GetZipedBytes(Encoding.UTF8.GetBytes(theString));
      if (Encoding.UTF8.GetString(ZipUnzip.GetBytesFromZipedBytes(zipedBytes)) != theString)
        throw new Exception("Compression check shows an error");
      return zipedBytes;
    }

    public static string GetZipedStringFromString(string theString)
    {
      string packedStringFromBytes = ZipUnzip.GetPackedStringFromBytes(ZipUnzip.GetZipedBytes(Encoding.UTF8.GetBytes(theString)));
      if (Encoding.UTF8.GetString(ZipUnzip.GetBytesFromZipedBytes(ZipUnzip.GetBytesFromPackedString(packedStringFromBytes))) != theString)
        throw new Exception("Compression check shows an error");
      return packedStringFromBytes;
    }

    public static byte[] GetZipedBytes(byte[] originalBytes)
    {
      byte[] array;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (GZipStream destination = new GZipStream((Stream) memoryStream, CompressionMode.Compress))
        {
          new MemoryStream(originalBytes).CopyTo((Stream) destination);
          destination.Flush();
        }
        array = memoryStream.ToArray();
      }
      return array;
    }

    private static string GetPackedStringFromBytes(byte[] theBytes)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if ((theBytes.Length & 1) != 0)
        stringBuilder.Append('1');
      else
        stringBuilder.Append('0');
      for (int index = 0; index < theBytes.Length; index += 2)
      {
        ushort theByte = (ushort) theBytes[index];
        if (index + 1 < theBytes.Length)
          theByte += (ushort) ((uint) theBytes[index + 1] << 8);
        stringBuilder.Append((char) theByte);
      }
      return stringBuilder.ToString();
    }

    public static string GetStringFromZipedBytes(byte[] zipedBytes)
    {
      return Encoding.UTF8.GetString(ZipUnzip.GetBytesFromZipedBytes(zipedBytes));
    }

    public static string GetStringFromZipedString(string theString)
    {
      return Encoding.UTF8.GetString(ZipUnzip.GetBytesFromZipedBytes(ZipUnzip.GetBytesFromPackedString(theString)));
    }

    public static byte[] GetBytesFromZipedBytes(byte[] zipedBytes)
    {
      byte[] array;
      using (MemoryStream memoryStream = new MemoryStream(zipedBytes))
      {
        using (MemoryStream destination = new MemoryStream())
        {
          using (GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Decompress))
          {
            gzipStream.CopyTo((Stream) destination);
            gzipStream.Flush();
          }
          array = destination.ToArray();
        }
      }
      return array;
    }

    private static byte[] GetBytesFromPackedString(string theString)
    {
      int length = (theString.Length - 1) * 2;
      if (theString[0] == '1')
        --length;
      byte[] fromPackedString = new byte[length];
      int num = 0;
      for (int index = 1; index < theString.Length; ++index)
      {
        fromPackedString[num++] = (byte) ((uint) theString[index] & (uint) byte.MaxValue);
        if (num < fromPackedString.Length)
          fromPackedString[num++] = (byte) ((int) theString[index] >> 8 & (int) byte.MaxValue);
      }
      return fromPackedString;
    }
  }
}
