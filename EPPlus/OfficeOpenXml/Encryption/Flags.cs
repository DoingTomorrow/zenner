// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Encryption.Flags
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.Encryption
{
  [Flags]
  internal enum Flags
  {
    Reserved1 = 1,
    Reserved2 = 2,
    fCryptoAPI = 4,
    fDocProps = 8,
    fExternal = 16, // 0x00000010
    fAES = 32, // 0x00000020
  }
}
