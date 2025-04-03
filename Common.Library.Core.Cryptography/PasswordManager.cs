// Decompiled with JetBrains decompiler
// Type: CA.Cryptography.PasswordManager
// Assembly: Common.Library.Core.Cryptography, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 243CB101-D6B4-4E20-AB19-910BDFBC9174
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.Core.Cryptography.dll

using System;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace CA.Cryptography
{
  public class PasswordManager
  {
    private HashAlgorithm HashProvider;
    private int SalthLength;

    public PasswordManager(HashAlgorithm HashAlgorithm, int theSaltLength)
    {
      this.HashProvider = HashAlgorithm;
      this.SalthLength = theSaltLength;
    }

    public PasswordManager()
      : this((HashAlgorithm) new SHA256Managed(), 4)
    {
    }

    private byte[] ComputeHash(byte[] password, byte[] salt)
    {
      byte[] numArray = new byte[password.Length + this.SalthLength];
      Array.Copy((Array) password, (Array) numArray, password.Length);
      Array.Copy((Array) salt, 0, (Array) numArray, password.Length, this.SalthLength);
      return this.HashProvider.ComputeHash(numArray);
    }

    public void GetPasswordHashAndSalt(byte[] password, out byte[] hash, out byte[] salt)
    {
      salt = new byte[this.SalthLength];
      new RNGCryptoServiceProvider().GetNonZeroBytes(salt);
      hash = this.ComputeHash(password, salt);
    }

    public void GetPasswordHashAndSaltString(string password, out string hash, out string salt)
    {
      byte[] hash1;
      byte[] salt1;
      this.GetPasswordHashAndSalt(Encoding.UTF8.GetBytes(password), out hash1, out salt1);
      hash = Convert.ToBase64String(hash1);
      salt = Convert.ToBase64String(salt1);
    }

    public void GetPasswordHashAndSaltConcatenatedString(string password, out string hashAndSalt)
    {
      string hash;
      string salt;
      this.GetPasswordHashAndSaltString(password, out hash, out salt);
      hashAndSalt = salt + hash;
    }

    public bool VerifyPasswordHash(byte[] password, byte[] hash, byte[] salt)
    {
      byte[] hash1 = this.ComputeHash(password, salt);
      if (hash1.Length != hash.Length)
        return false;
      for (int index = 0; index < hash.Length; ++index)
      {
        if (!hash[index].Equals(hash1[index]))
          return false;
      }
      return true;
    }

    public bool VerifyPassowrdHashString(string password, string hash, string salt)
    {
      byte[] hash1 = Convert.FromBase64String(hash);
      byte[] salt1 = Convert.FromBase64String(salt);
      return this.VerifyPasswordHash(Encoding.UTF8.GetBytes(password), hash1, salt1);
    }

    public bool VerifyPassowrdHashString(string password, string hashAndSalt)
    {
      string s = hashAndSalt.Substring(0, this.SalthLength * 2);
      byte[] hash = Convert.FromBase64String(hashAndSalt.Substring(this.SalthLength * 2));
      byte[] salt = Convert.FromBase64String(s);
      return this.VerifyPasswordHash(Encoding.UTF8.GetBytes(password), hash, salt);
    }

    public byte[] GenerateKeyFromPassword(string password)
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
      return new Rfc2898DeriveBytes(password, salt, iterations).GetBytes(16);
    }
  }
}
