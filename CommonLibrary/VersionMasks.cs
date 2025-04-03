// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.VersionMasks
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

#nullable disable
namespace ZENNER.CommonLibrary
{
  public enum VersionMasks : uint
  {
    Type = 4095, // 0x00000FFF
    Revision = 61440, // 0x0000F000
    Minor = 16711680, // 0x00FF0000
    BSL_Type = 16773120, // 0x00FFF000
    Major = 2130706432, // 0x7F000000
    Debug = 2147483648, // 0x80000000
  }
}
