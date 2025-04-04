// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Encryption.EncryptionVerifier
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.IO;

#nullable disable
namespace OfficeOpenXml.Encryption
{
  internal class EncryptionVerifier
  {
    internal uint SaltSize;
    internal byte[] Salt;
    internal byte[] EncryptedVerifier;
    internal uint VerifierHashSize;
    internal byte[] EncryptedVerifierHash;

    internal byte[] WriteBinary()
    {
      MemoryStream output = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      binaryWriter.Write(this.SaltSize);
      binaryWriter.Write(this.Salt);
      binaryWriter.Write(this.EncryptedVerifier);
      binaryWriter.Write(20);
      binaryWriter.Write(this.EncryptedVerifierHash);
      binaryWriter.Flush();
      return output.ToArray();
    }
  }
}
