// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.ScanMask
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  [Flags]
  internal enum ScanMask : byte
  {
    None = 0,
    Inquiry = 1,
    Page = 2,
  }
}
