// Decompiled with JetBrains decompiler
// Type: Standard.THBF
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;

#nullable disable
namespace Standard
{
  [Flags]
  internal enum THBF : uint
  {
    ENABLED = 0,
    DISABLED = 1,
    DISMISSONCLICK = 2,
    NOBACKGROUND = 4,
    HIDDEN = 8,
    NONINTERACTIVE = 16, // 0x00000010
  }
}
