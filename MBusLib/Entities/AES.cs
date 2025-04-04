// Decompiled with JetBrains decompiler
// Type: MBusLib.Entities.AES
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

#nullable disable
namespace MBusLib.Entities
{
  public static class AES
  {
    public static byte[] AESCMAC(byte[] key, byte[] data)
    {
      if (key == null)
        throw new ArgumentNullException("The AES key is null!");
      if (data == null)
        throw new ArgumentNullException("The data for AES encryption is null!");
      byte[] b1 = AES.AESEncrypt(key, new byte[16], new byte[16]);
      byte[] b2 = AES.Rol(b1);
      if (((int) b1[0] & 128) == 128)
        b2[15] ^= (byte) 135;
      byte[] numArray = AES.Rol(b2);
      if (((int) b2[0] & 128) == 128)
        numArray[15] ^= (byte) 135;
      if (data.Length != 0 && data.Length % 16 == 0)
      {
        for (int index = 0; index < b2.Length; ++index)
          data[data.Length - 16 + index] ^= b2[index];
      }
      else
      {
        byte[] source = new byte[16 - data.Length % 16];
        source[0] = (byte) 128;
        data = ((IEnumerable<byte>) data).Concat<byte>(((IEnumerable<byte>) source).AsEnumerable<byte>()).ToArray<byte>();
        for (int index = 0; index < numArray.Length; ++index)
          data[data.Length - 16 + index] ^= numArray[index];
      }
      byte[] sourceArray = AES.AESEncrypt(key, new byte[16], data);
      byte[] destinationArray = new byte[16];
      Array.Copy((Array) sourceArray, sourceArray.Length - destinationArray.Length, (Array) destinationArray, 0, destinationArray.Length);
      return destinationArray;
    }

    private static byte[] AESEncrypt(byte[] key, byte[] iv, byte[] data)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider();
        cryptoServiceProvider.Mode = CipherMode.CBC;
        cryptoServiceProvider.Padding = PaddingMode.None;
        using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, cryptoServiceProvider.CreateEncryptor(key, iv), CryptoStreamMode.Write))
        {
          cryptoStream.Write(data, 0, data.Length);
          cryptoStream.FlushFinalBlock();
          return memoryStream.ToArray();
        }
      }
    }

    private static byte[] Rol(byte[] b)
    {
      byte[] numArray = new byte[b.Length];
      byte num1 = 0;
      for (int index = b.Length - 1; index >= 0; --index)
      {
        ushort num2 = (ushort) ((uint) b[index] << 1);
        numArray[index] = (byte) (((uint) num2 & (uint) byte.MaxValue) + (uint) num1);
        num1 = (byte) (((int) num2 & 65280) >> 8);
      }
      return numArray;
    }
  }
}
