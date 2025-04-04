// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.FaultIdentification
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using System;

#nullable disable
namespace SmokeDetectorHandler
{
  [Flags]
  public enum FaultIdentification : ushort
  {
    ADC = 1,
    TempDiode = 2,
    LowBattery = 4,
    HiResBattery = 8,
    DegradedChamber = 16, // 0x0010
    CalibrationCorrupted = 32, // 0x0020
    RadioModule = 64, // 0x0040
    RadioModuleBattery = 128, // 0x0080
    Bit8 = 256, // 0x0100
    SPI_BufferOverflow = 512, // 0x0200
    EEPROM = 1024, // 0x0400
    EEPROM_BufferOverflow = 2048, // 0x0800
    Thermoptek = 4096, // 0x1000
    Bit13 = 8192, // 0x2000
    Bit14 = 16384, // 0x4000
    Bit15 = 32768, // 0x8000
  }
}
