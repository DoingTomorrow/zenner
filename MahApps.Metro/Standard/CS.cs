﻿// Decompiled with JetBrains decompiler
// Type: Standard.CS
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;

#nullable disable
namespace Standard
{
  [Flags]
  internal enum CS : uint
  {
    VREDRAW = 1,
    HREDRAW = 2,
    DBLCLKS = 8,
    OWNDC = 32, // 0x00000020
    CLASSDC = 64, // 0x00000040
    PARENTDC = 128, // 0x00000080
    NOCLOSE = 512, // 0x00000200
    SAVEBITS = 2048, // 0x00000800
    BYTEALIGNCLIENT = 4096, // 0x00001000
    BYTEALIGNWINDOW = 8192, // 0x00002000
    GLOBALCLASS = 16384, // 0x00004000
    IME = 65536, // 0x00010000
    DROPSHADOW = 131072, // 0x00020000
  }
}
