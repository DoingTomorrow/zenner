// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.LookupFlags
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  [Flags]
  internal enum LookupFlags : uint
  {
    Containers = 2,
    ReturnName = 16, // 0x00000010
    ReturnAddr = 256, // 0x00000100
    ReturnBlob = 512, // 0x00000200
    FlushCache = 4096, // 0x00001000
    ResService = 32768, // 0x00008000
  }
}
