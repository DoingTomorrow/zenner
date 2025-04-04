// Decompiled with JetBrains decompiler
// Type: MinolHandler.TestStationDataStatus
// Assembly: MinolHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: A1A42975-0CFC-4FCB-838E-3BA18C5EABDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinolHandler.dll

#nullable disable
namespace MinolHandler
{
  public enum TestStationDataStatus : byte
  {
    OK = 0,
    Error1 = 1,
    Error2 = 2,
    NotCalibrated = 3,
    ERR_Flags = 4,
    MAX_DAC0_max = 5,
    MIN_DAC0_max = 6,
    MAX_DAC1_min = 7,
    MIN_DAC1_min = 8,
    DAC0_cal_unequal_DAC0_max = 9,
    DAC1_cal_unequal_DAC1_min = 10, // 0x0A
    ImpulsAFail = 11, // 0x0B
    ImpulsBFail = 12, // 0x0C
    ImpulsABShortcut = 13, // 0x0D
    Undefined = 253, // 0xFD
    Calibrated = 254, // 0xFE
    NotTested = 255, // 0xFF
  }
}
