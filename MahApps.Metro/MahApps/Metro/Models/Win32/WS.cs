// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Models.Win32.WS
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;

#nullable disable
namespace MahApps.Metro.Models.Win32
{
  [Flags]
  internal enum WS : uint
  {
    BORDER = 8388608, // 0x00800000
    CAPTION = 12582912, // 0x00C00000
    CHILD = 1073741824, // 0x40000000
    CLIPCHILDREN = 33554432, // 0x02000000
    CLIPSIBLINGS = 67108864, // 0x04000000
    DISABLED = 134217728, // 0x08000000
    DLGFRAME = 4194304, // 0x00400000
    GROUP = 131072, // 0x00020000
    HSCROLL = 1048576, // 0x00100000
    MAXIMIZE = 16777216, // 0x01000000
    MAXIMIZEBOX = 65536, // 0x00010000
    MINIMIZE = 536870912, // 0x20000000
    MINIMIZEBOX = GROUP, // 0x00020000
    OVERLAPPED = 0,
    OVERLAPPEDWINDOW = 13565952, // 0x00CF0000
    POPUP = 2147483648, // 0x80000000
    POPUPWINDOW = 2156396544, // 0x80880000
    SIZEFRAME = 262144, // 0x00040000
    SYSMENU = 524288, // 0x00080000
    TABSTOP = MAXIMIZEBOX, // 0x00010000
    VISIBLE = 268435456, // 0x10000000
    VSCROLL = 2097152, // 0x00200000
  }
}
