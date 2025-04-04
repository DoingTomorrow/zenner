// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.AES
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

#nullable disable
namespace ZR_ClassLibrary
{
  public static class AES
  {
    public const string KEYOFF_NAME = "OFF";
    public const string USRKEY_NAME = "**********";
    public const string RNG_NAME = "RANDOM KEY";
    public const string ZDK_NAME = "ZENNER DEFAULT KEY";
    public const string ZDK = "5A8470C4806F4A87CEF4D5F2D985AB18";
    public const string AES_OFF_KEY = "00000000000000000000000000000000";

    public static string AesKeyToString(byte[] key)
    {
      if (key == null)
        return (string) null;
      string str = key.Length == 16 ? Util.ByteArrayToHexString(key) : throw new Exception("The length of the key is not valid: " + Util.ByteArrayToHexString(key));
      switch (str)
      {
        case "5A8470C4806F4A87CEF4D5F2D985AB18":
          return "ZENNER DEFAULT KEY";
        case "00000000000000000000000000000000":
          return "OFF";
        default:
          return str;
      }
    }

    public static string AesKeyToString(uint Key0, uint Key1, uint Key2, uint Key3)
    {
      byte[] bytes1 = BitConverter.GetBytes(Key0);
      byte[] bytes2 = BitConverter.GetBytes(Key1);
      byte[] bytes3 = BitConverter.GetBytes(Key2);
      byte[] bytes4 = BitConverter.GetBytes(Key3);
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) bytes1);
      byteList.AddRange((IEnumerable<byte>) bytes2);
      byteList.AddRange((IEnumerable<byte>) bytes3);
      byteList.AddRange((IEnumerable<byte>) bytes4);
      return AES.AesKeyToString(byteList.ToArray());
    }

    public static byte[] StringToAesKey(string keyName)
    {
      if (string.IsNullOrEmpty(keyName))
        return (byte[]) null;
      switch (keyName)
      {
        case "OFF":
          return (byte[]) null;
        case "ZENNER DEFAULT KEY":
          return Util.HexStringToByteArray("5A8470C4806F4A87CEF4D5F2D985AB18");
        case "RANDOM KEY":
          return AES.GenerateRandomNumber(16);
        default:
          keyName = keyName.Trim();
          if (keyName.Length != 32)
            throw new Exception("Length of the key is invalid: " + keyName);
          try
          {
            return Util.HexStringToByteArray(keyName);
          }
          catch (Exception ex)
          {
            throw new Exception("Key is invalid: " + keyName, ex);
          }
      }
    }

    public static List<string> AllowedKeys()
    {
      return new List<string>()
      {
        "OFF",
        "ZENNER DEFAULT KEY"
      };
    }

    private static byte[] GenerateRandomNumber(int bytes)
    {
      byte[] data = new byte[bytes];
      bool flag = true;
      do
      {
        new RNGCryptoServiceProvider().GetBytes(data);
        for (int index = 0; index < data.Length; ++index)
        {
          if (index != 0)
          {
            flag = false;
            break;
          }
        }
      }
      while (flag);
      return data;
    }
  }
}
