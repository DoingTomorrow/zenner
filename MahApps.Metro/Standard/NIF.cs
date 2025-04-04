// Decompiled with JetBrains decompiler
// Type: Standard.NIF
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;

#nullable disable
namespace Standard
{
  [Flags]
  internal enum NIF : uint
  {
    MESSAGE = 1,
    ICON = 2,
    TIP = 4,
    STATE = 8,
    INFO = 16, // 0x00000010
    GUID = 32, // 0x00000020
    REALTIME = 64, // 0x00000040
    SHOWTIP = 128, // 0x00000080
    XP_MASK = GUID | INFO | STATE | ICON | MESSAGE, // 0x0000003B
    VISTA_MASK = XP_MASK | SHOWTIP | REALTIME, // 0x000000FB
  }
}
