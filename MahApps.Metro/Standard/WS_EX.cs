﻿// Decompiled with JetBrains decompiler
// Type: Standard.WS_EX
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;

#nullable disable
namespace Standard
{
  [Flags]
  internal enum WS_EX : uint
  {
    None = 0,
    DLGMODALFRAME = 1,
    NOPARENTNOTIFY = 4,
    TOPMOST = 8,
    ACCEPTFILES = 16, // 0x00000010
    TRANSPARENT = 32, // 0x00000020
    MDICHILD = 64, // 0x00000040
    TOOLWINDOW = 128, // 0x00000080
    WINDOWEDGE = 256, // 0x00000100
    CLIENTEDGE = 512, // 0x00000200
    CONTEXTHELP = 1024, // 0x00000400
    RIGHT = 4096, // 0x00001000
    LEFT = 0,
    RTLREADING = 8192, // 0x00002000
    LTRREADING = 0,
    LEFTSCROLLBAR = 16384, // 0x00004000
    RIGHTSCROLLBAR = 0,
    CONTROLPARENT = 65536, // 0x00010000
    STATICEDGE = 131072, // 0x00020000
    APPWINDOW = 262144, // 0x00040000
    LAYERED = 524288, // 0x00080000
    NOINHERITLAYOUT = 1048576, // 0x00100000
    LAYOUTRTL = 4194304, // 0x00400000
    COMPOSITED = 33554432, // 0x02000000
    NOACTIVATE = 134217728, // 0x08000000
    OVERLAPPEDWINDOW = CLIENTEDGE | WINDOWEDGE, // 0x00000300
    PALETTEWINDOW = WINDOWEDGE | TOOLWINDOW | TOPMOST, // 0x00000188
  }
}
