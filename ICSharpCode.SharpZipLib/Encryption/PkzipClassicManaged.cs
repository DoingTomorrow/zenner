// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Encryption.PkzipClassicManaged
// Assembly: ICSharpCode.SharpZipLib, Version=0.85.5.452, Culture=neutral, PublicKeyToken=1b03e6acf1164f73
// MVID: 6C7CFC05-3661-46A0-98AB-FC6F81793937
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ICSharpCode.SharpZipLib.dll

using System;
using System.Security.Cryptography;

#nullable disable
namespace ICSharpCode.SharpZipLib.Encryption
{
  public sealed class PkzipClassicManaged : PkzipClassic
  {
    private byte[] key_;

    public override int BlockSize
    {
      get => 8;
      set
      {
        if (value != 8)
          throw new CryptographicException("Block size is invalid");
      }
    }

    public override KeySizes[] LegalKeySizes
    {
      get => new KeySizes[1]{ new KeySizes(96, 96, 0) };
    }

    public override void GenerateIV()
    {
    }

    public override KeySizes[] LegalBlockSizes
    {
      get => new KeySizes[1]{ new KeySizes(8, 8, 0) };
    }

    public override byte[] Key
    {
      get
      {
        if (this.key_ == null)
          this.GenerateKey();
        return (byte[]) this.key_.Clone();
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (value));
        this.key_ = value.Length == 12 ? (byte[]) value.Clone() : throw new CryptographicException("Key size is illegal");
      }
    }

    public override void GenerateKey()
    {
      this.key_ = new byte[12];
      new Random().NextBytes(this.key_);
    }

    public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
    {
      this.key_ = rgbKey;
      return (ICryptoTransform) new PkzipClassicEncryptCryptoTransform(this.Key);
    }

    public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
    {
      this.key_ = rgbKey;
      return (ICryptoTransform) new PkzipClassicDecryptCryptoTransform(this.Key);
    }
  }
}
