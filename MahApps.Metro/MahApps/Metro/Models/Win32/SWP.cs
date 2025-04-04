// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Models.Win32.SWP
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;

#nullable disable
namespace MahApps.Metro.Models.Win32
{
  [Flags]
  internal enum SWP
  {
    NOSIZE = 1,
    NOMOVE = 2,
    NOZORDER = 4,
    NOREDRAW = 8,
    NOACTIVATE = 16, // 0x00000010
    FRAMECHANGED = 32, // 0x00000020
    SHOWWINDOW = 64, // 0x00000040
    NOOWNERZORDER = 512, // 0x00000200
    NOSENDCHANGING = 1024, // 0x00000400
    ASYNCWINDOWPOS = 16384, // 0x00004000
  }
}
