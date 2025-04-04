// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Encryption.EncryptionInfo
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.Encryption
{
  internal abstract class EncryptionInfo
  {
    internal short MajorVersion;
    internal short MinorVersion;

    internal abstract void Read(byte[] data);

    internal static EncryptionInfo ReadBinary(byte[] data)
    {
      short int16_1 = BitConverter.ToInt16(data, 0);
      short int16_2 = BitConverter.ToInt16(data, 2);
      EncryptionInfo encryptionInfo;
      if (int16_2 == (short) 3 && int16_1 <= (short) 4)
      {
        encryptionInfo = (EncryptionInfo) new EncryptionInfoBinary();
      }
      else
      {
        if (int16_1 != (short) 4 || int16_2 != (short) 4)
          throw new NotSupportedException("Unsupported encryption format");
        encryptionInfo = (EncryptionInfo) new EncryptionInfoAgile();
      }
      encryptionInfo.MajorVersion = int16_1;
      encryptionInfo.MinorVersion = int16_2;
      encryptionInfo.Read(data);
      return encryptionInfo;
    }
  }
}
