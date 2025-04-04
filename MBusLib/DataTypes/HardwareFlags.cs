// Decompiled with JetBrains decompiler
// Type: MBusLib.DataTypes.HardwareFlags
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using System;

#nullable disable
namespace MBusLib.DataTypes
{
  [Flags]
  [Serializable]
  public enum HardwareFlags : uint
  {
    MBus = 1,
    Radio = 2,
    NoCom = 4,
    LoRa = 8,
    Compact = 16, // 0x00000010
    Combi = 32, // 0x00000020
    Ultrasonic = 64, // 0x00000040
    UltrasonicDirect = 128, // 0x00000080
    UndefDev1 = 256, // 0x00000100
    WR4 = 512, // 0x00000200
    UndefDev2 = 1024, // 0x00000400
    C5 = 2048, // 0x00000800
    WRITE_PERMISSION = 4096, // 0x00001000
    EMERGENCY_MODE = 8192, // 0x00002000
    BASE_CONFIG_ACTIVE = 16384, // 0x00004000
    ANY_RESTRICTION = 61440, // 0x0000F000
  }
}
