// Decompiled with JetBrains decompiler
// Type: ZENNER.M8_DeviceStatus
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using System;

#nullable disable
namespace ZENNER
{
  [Flags]
  [Serializable]
  public enum M8_DeviceStatus : ushort
  {
    OK = 0,
    Tamper = 1,
    Battery = 2,
    BatteryEndLife = 4,
    Hardware = 8,
    HcaError = 16, // 0x0010
    VersionLut = 32, // 0x0020
    RadioError = 64, // 0x0040
    Transceiver = 128, // 0x0080
  }
}
