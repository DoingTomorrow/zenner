// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Encryption.EncryptionHeader
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.IO;
using System.Text;

#nullable disable
namespace OfficeOpenXml.Encryption
{
  internal class EncryptionHeader
  {
    internal Flags Flags;
    internal int SizeExtra;
    internal AlgorithmID AlgID;
    internal AlgorithmHashID AlgIDHash;
    internal int KeySize;
    internal ProviderType ProviderType;
    internal int Reserved1;
    internal int Reserved2;
    internal string CSPName;

    internal byte[] WriteBinary()
    {
      MemoryStream output = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      binaryWriter.Write((int) this.Flags);
      binaryWriter.Write(this.SizeExtra);
      binaryWriter.Write((int) this.AlgID);
      binaryWriter.Write((int) this.AlgIDHash);
      binaryWriter.Write(this.KeySize);
      binaryWriter.Write((int) this.ProviderType);
      binaryWriter.Write(this.Reserved1);
      binaryWriter.Write(this.Reserved2);
      binaryWriter.Write(Encoding.Unicode.GetBytes(this.CSPName));
      binaryWriter.Flush();
      return output.ToArray();
    }
  }
}
