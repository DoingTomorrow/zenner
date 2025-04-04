// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BluetoothDeviceInfoProperties
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  [Flags]
  public enum BluetoothDeviceInfoProperties
  {
    Address = 1,
    Cod = 2,
    Name = 4,
    Paired = 8,
    Personal = 16, // 0x00000010
    Connected = 32, // 0x00000020
    ShortName = 64, // 0x00000040
    Visible = 128, // 0x00000080
    SspSupported = 256, // 0x00000100
    SspPaired = 512, // 0x00000200
    SspMitmProtected = 1024, // 0x00000400
    Rssi = 4096, // 0x00001000
    Eir = 8192, // 0x00002000
    BR = 16384, // 0x00004000
    LE = 32768, // 0x00008000
    LEPaired = 65536, // 0x00010000
    LEPersonal = 131072, // 0x00020000
    LEMitmProtected = 262144, // 0x00040000
  }
}
