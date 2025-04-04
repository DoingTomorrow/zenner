// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Encryption.EncryptionInfoBinary
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace OfficeOpenXml.Encryption
{
  internal class EncryptionInfoBinary : EncryptionInfo
  {
    internal Flags Flags;
    internal uint HeaderSize;
    internal EncryptionHeader Header;
    internal EncryptionVerifier Verifier;

    internal override void Read(byte[] data)
    {
      this.Flags = (Flags) BitConverter.ToInt32(data, 4);
      this.HeaderSize = (uint) BitConverter.ToInt32(data, 8);
      this.Header = new EncryptionHeader();
      this.Header.Flags = (Flags) BitConverter.ToInt32(data, 12);
      this.Header.SizeExtra = BitConverter.ToInt32(data, 16);
      this.Header.AlgID = (AlgorithmID) BitConverter.ToInt32(data, 20);
      this.Header.AlgIDHash = (AlgorithmHashID) BitConverter.ToInt32(data, 24);
      this.Header.KeySize = BitConverter.ToInt32(data, 28);
      this.Header.ProviderType = (ProviderType) BitConverter.ToInt32(data, 32);
      this.Header.Reserved1 = BitConverter.ToInt32(data, 36);
      this.Header.Reserved2 = BitConverter.ToInt32(data, 40);
      byte[] numArray = new byte[(int) this.HeaderSize - 34];
      Array.Copy((Array) data, 44, (Array) numArray, 0, (int) this.HeaderSize - 34);
      this.Header.CSPName = Encoding.Unicode.GetString(numArray);
      int startIndex = (int) this.HeaderSize + 12;
      this.Verifier = new EncryptionVerifier();
      this.Verifier.SaltSize = (uint) BitConverter.ToInt32(data, startIndex);
      this.Verifier.Salt = new byte[(IntPtr) this.Verifier.SaltSize];
      Array.Copy((Array) data, (long) (startIndex + 4), (Array) this.Verifier.Salt, 0L, (long) this.Verifier.SaltSize);
      this.Verifier.EncryptedVerifier = new byte[16];
      Array.Copy((Array) data, startIndex + 20, (Array) this.Verifier.EncryptedVerifier, 0, 16);
      this.Verifier.VerifierHashSize = (uint) BitConverter.ToInt32(data, startIndex + 36);
      this.Verifier.EncryptedVerifierHash = new byte[(IntPtr) this.Verifier.VerifierHashSize];
      Array.Copy((Array) data, (long) (startIndex + 40), (Array) this.Verifier.EncryptedVerifierHash, 0L, (long) this.Verifier.VerifierHashSize);
    }

    internal byte[] WriteBinary()
    {
      MemoryStream output = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      binaryWriter.Write(this.MajorVersion);
      binaryWriter.Write(this.MinorVersion);
      binaryWriter.Write((int) this.Flags);
      byte[] buffer = this.Header.WriteBinary();
      binaryWriter.Write((uint) buffer.Length);
      binaryWriter.Write(buffer);
      binaryWriter.Write(this.Verifier.WriteBinary());
      binaryWriter.Flush();
      return output.ToArray();
    }
  }
}
