// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.ServiceClass
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  [Flags]
  public enum ServiceClass
  {
    None = 0,
    Information = 1024, // 0x00000400
    Telephony = 512, // 0x00000200
    Audio = 256, // 0x00000100
    ObjectTransfer = 128, // 0x00000080
    Capturing = 64, // 0x00000040
    Rendering = 32, // 0x00000020
    Network = 16, // 0x00000010
    Positioning = 8,
    LimitedDiscoverableMode = 1,
  }
}
