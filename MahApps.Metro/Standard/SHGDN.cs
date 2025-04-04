// Decompiled with JetBrains decompiler
// Type: Standard.SHGDN
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;

#nullable disable
namespace Standard
{
  [Flags]
  internal enum SHGDN
  {
    SHGDN_NORMAL = 0,
    SHGDN_INFOLDER = 1,
    SHGDN_FOREDITING = 4096, // 0x00001000
    SHGDN_FORADDRESSBAR = 16384, // 0x00004000
    SHGDN_FORPARSING = 32768, // 0x00008000
  }
}
