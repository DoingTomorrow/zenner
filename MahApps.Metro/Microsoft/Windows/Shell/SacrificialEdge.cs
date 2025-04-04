// Decompiled with JetBrains decompiler
// Type: Microsoft.Windows.Shell.SacrificialEdge
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;

#nullable disable
namespace Microsoft.Windows.Shell
{
  [Flags]
  public enum SacrificialEdge
  {
    None = 0,
    Left = 1,
    Top = 2,
    Right = 4,
    Bottom = 8,
    Office = Bottom | Right | Left, // 0x0000000D
  }
}
