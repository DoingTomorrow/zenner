// Decompiled with JetBrains decompiler
// Type: MBusLib.MBusUtil
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using System;
using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace MBusLib
{
  public static class MBusUtil
  {
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

    public static string GetMedium(byte medium)
    {
      return Enum.IsDefined(typeof (Medium), (object) medium) ? Enum.ToObject(typeof (Medium), medium).ToString() : string.Empty;
    }

    public static byte CalculateChecksum(byte[] buffer, int startIndex, int count)
    {
      byte checksum = 0;
      for (int index = startIndex; index < count; ++index)
        checksum += buffer[index];
      return checksum;
    }

    public static byte[] GetUserData(byte[] buffer, int startIndex)
    {
      int count = buffer.Length - startIndex - 2;
      byte[] dst = new byte[count];
      Buffer.BlockCopy((Array) buffer, startIndex, (Array) dst, 0, count);
      return dst;
    }

    public static ushort ConvertDateTimeTo_YYYYMMMMYYYDDDDD(DateTime date)
    {
      ushort num1 = (ushort) ((uint) (ushort) date.Day | (uint) (ushort) (date.Month << 8));
      ushort num2 = (ushort) (date.Year % 100);
      return (ushort) ((uint) num1 | (uint) (ushort) (((int) num2 & 120) << 9 | ((int) num2 & 7) << 5));
    }

    public static DateTime? ConvertToDateTime_MBus_CP32_TypeF(byte[] buffer)
    {
      return MBusUtil.ConvertToDateTime_MBus_CP32_TypeF(buffer, 0);
    }

    public static DateTime? ConvertToDateTime_MBus_CP32_TypeF(byte[] buffer, int startIndex)
    {
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

    public static DateTime? ConvertToDate_MBus_CP16_TypeG(byte[] buffer)
    {
      return MBusUtil.ConvertToDate_MBus_CP16_TypeG(buffer, 0);
    }

    public static DateTime? ConvertToDate_MBus_CP16_TypeG(byte[] buffer, int startIndex)
    {
      int day = (int) buffer[startIndex] & 31;
      if (day < 1 || day > 31)
        return new DateTime?();
      int month = (int) buffer[startIndex + 1] & 15;
      if (month < 1 || month > 12)
        return new DateTime?();
      int num = (int) buffer[startIndex] >> 5 | ((int) buffer[startIndex + 1] & 240) >> 1;
      if (num > 99)
        return new DateTime?();
      int year = num + 2000;
      try
      {
        return new DateTime?(new DateTime(year, month, day));
      }
      catch
      {
        return new DateTime?();
      }
    }

    public static bool IsBcdValid(byte[] bcd)
    {
      if (bcd == null)
        throw new ArgumentNullException(nameof (bcd));
      if (bcd.Length == 0)
        return false;
      foreach (byte num1 in bcd)
      {
        int num2 = (int) num1 & 15;
        int num3 = (int) num1 >> 4;
        if (num2 >= 10 && num2 <= 15 || num3 >= 10 && num3 <= 14)
          return false;
      }
      return true;
    }

    public static bool IsIntegerValid(byte[] value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      return value.Length != 0 && ((int) value[value.Length - 1] & 128) != 128;
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
        using (MemoryStream memoryStream1 = new MemoryStream(encrypted))
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream1, decryptor, CryptoStreamMode.Read))
          {
            using (MemoryStream memoryStream2 = new MemoryStream())
            {
              byte[] buffer = new byte[encrypted.Length];
              while (true)
              {
                int count = cryptoStream.Read(buffer, 0, buffer.Length);
                if (count > 0)
                  memoryStream2.Write(buffer, 0, count);
                else
                  break;
              }
              return memoryStream2.ToArray();
            }
          }
        }
      }
    }

    public static object GetManufacturer(ushort? value)
    {
      return value.HasValue ? (object) MBusUtil.GetManufacturer(value.Value) : (object) string.Empty;
    }
  }
}
