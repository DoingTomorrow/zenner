// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.BTHNS_RESULT
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  [Flags]
  internal enum BTHNS_RESULT
  {
    None = 0,
    Connected = 65536, // 0x00010000
    Remembered = 131072, // 0x00020000
    Authenticated = 262144, // 0x00040000
  }
}
