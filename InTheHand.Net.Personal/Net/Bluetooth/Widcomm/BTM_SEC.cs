// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.BTM_SEC
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  [Flags]
  internal enum BTM_SEC : byte
  {
    NONE = 0,
    IN_AUTHORIZE = 1,
    IN_AUTHENTICATE = 2,
    IN_ENCRYPT = 4,
    OUT_AUTHORIZE = 8,
    OUT_AUTHENTICATE = 16, // 0x10
    OUT_ENCRYPT = 32, // 0x20
    BOND = 64, // 0x40
  }
}
