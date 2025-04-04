// Decompiled with JetBrains decompiler
// Type: Standard.WTNCA
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;

#nullable disable
namespace Standard
{
  [Flags]
  internal enum WTNCA : uint
  {
    NODRAWCAPTION = 1,
    NODRAWICON = 2,
    NOSYSMENU = 4,
    NOMIRRORHELP = 8,
    VALIDBITS = NOMIRRORHELP | NOSYSMENU | NODRAWICON | NODRAWCAPTION, // 0x0000000F
  }
}
