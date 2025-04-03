// Decompiled with JetBrains decompiler
// Type: CA.Cryptography.EncryptionManager
// Assembly: Common.Library.Core.Cryptography, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 243CB101-D6B4-4E20-AB19-910BDFBC9174
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.Core.Cryptography.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace CA.Cryptography
{
  public class EncryptionManager
  {
    private SymmetricAlgorithm symetricAlg;

    public EncryptionManager()
      : this((SymmetricAlgorithm) new RijndaelManaged())
    {
    }

    public EncryptionManager(SymmetricAlgorithm _symetricAlg) => this.symetricAlg = _symetricAlg;

    public string GenerateKey()
    {
      this.symetricAlg.GenerateKey();
      return Convert.ToBase64String(this.symetricAlg.Key);
    }

    public string GenerateKeyFromPassword(string password)
    {
      byte[] salt = new byte[13]
      {
        (byte) 0,
        (byte) 1,
        (byte) 2,
        (byte) 3,
        (byte) 4,
        (byte) 5,
        (byte) 6,
        (byte) 241,
        (byte) 240,
        (byte) 238,
        (byte) 33,
        (byte) 34,
        (byte) 69
      };
      int iterations = 1000;
      Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, iterations);
      this.GenerateKey();
      return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(this.symetricAlg.Key.Length));
    }

    private byte[] GenerateIV()
    {
      this.symetricAlg.GenerateIV();
      return this.symetricAlg.IV;
    }

    public string Encrypt(string plainText, string dataKey)
    {
      byte[] iv = this.GenerateIV();
      Convert.ToBase64String(iv);
      byte[] bytes = Encoding.Unicode.GetBytes(plainText);
      ICryptoTransform encryptor = this.symetricAlg.CreateEncryptor(Convert.FromBase64String(dataKey), iv);
      this.symetricAlg.Key = Convert.FromBase64String(dataKey);
      this.symetricAlg.IV = iv;
      byte[] array;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
          cryptoStream.Write(bytes, 0, bytes.Length);
        array = memoryStream.ToArray();
      }
      byte[] inArray = new byte[iv.Length + array.Length];
      iv.CopyTo((Array) inArray, 0);
      array.CopyTo((Array) inArray, iv.Length);
      return Convert.ToBase64String(inArray);
    }

    public string Decrypt(string encryptedTextValue, string dataKey)
    {
      byte[] source = Convert.FromBase64String(encryptedTextValue);
      int length = this.GenerateIV().Length;
      byte[] array1 = ((IEnumerable<byte>) source).Skip<byte>(length).Take<byte>(source.Length - length).ToArray<byte>();
      byte[] array2 = ((IEnumerable<byte>) source).Take<byte>(length).ToArray<byte>();
      ICryptoTransform decryptor = this.symetricAlg.CreateDecryptor(Convert.FromBase64String(dataKey), array2);
      this.symetricAlg.Key = Convert.FromBase64String(dataKey);
      this.symetricAlg.IV = array2;
      byte[] array3;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Write))
          cryptoStream.Write(array1, 0, array1.Length);
        array3 = memoryStream.ToArray();
      }
      return Encoding.Unicode.GetString(array3);
    }

    public void NewTest()
    {
      byte[] bytes1 = Encoding.Unicode.GetBytes("This is all clear now!");
      using (Rijndael rijndael = (Rijndael) new RijndaelManaged())
      {
        rijndael.Padding = PaddingMode.PKCS7;
        rijndael.GenerateKey();
        rijndael.GenerateIV();
        byte[] buffer = (byte[]) null;
        byte[] bytes2 = (byte[]) null;
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write))
            cryptoStream.Write(bytes1, 0, bytes1.Length);
          buffer = memoryStream.ToArray();
        }
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
            cryptoStream.Write(buffer, 0, buffer.Length);
          bytes2 = memoryStream.ToArray();
        }
        Console.WriteLine(Encoding.Unicode.GetString(bytes2));
      }
    }
  }
}
