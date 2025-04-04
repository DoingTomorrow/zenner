// Decompiled with JetBrains decompiler
// Type: Standard.GPS
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

#nullable disable
namespace Standard
{
  internal enum GPS
  {
    DEFAULT = 0,
    HANDLERPROPERTIESONLY = 1,
    READWRITE = 2,
    TEMPORARY = 4,
    FASTPROPERTIESONLY = 8,
    OPENSLOWITEM = 16, // 0x00000010
    DELAYCREATION = 32, // 0x00000020
    BESTEFFORT = 64, // 0x00000040
    NO_OPLOCK = 128, // 0x00000080
    MASK_VALID = 255, // 0x000000FF
  }
}
