// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.OpcodeLoadFirmwareParameter
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

#nullable disable
namespace SmartFunctionCompiler
{
  internal enum OpcodeLoadFirmwareParameter : byte
  {
    LoadA = 96, // 0x60
    LoadB = 97, // 0x61
    LoadVolume = 98, // 0x62
    LoadFlow = 99, // 0x63
    LoadVolumeIncrement = 100, // 0x64
    LoadTime = 101, // 0x65
    LoadStatus = 102, // 0x66
    LoadTemp = 103, // 0x67
    LoadEnergy = 104, // 0x68
    LoadPower = 105, // 0x69
    LoadEnergyIncrement = 106, // 0x6A
    LoadSecondTemp = 107, // 0x6B
    LoadTempDiff = 108, // 0x6C
    LoadVolumeCycleSeconds = 109, // 0x6D
    LoadDateTime = 110, // 0x6E
    LoadDay = 111, // 0x6F
    LoadMonth = 112, // 0x70
    LoadYear = 113, // 0x71
    LoadFlowMin = 114, // 0x72
    LoadFlowMax = 115, // 0x73
    LoadFlowQ1 = 116, // 0x74
    LoadFlowQ2 = 117, // 0x75
    LoadFlowQ3 = 118, // 0x76
    LoadFlowQ4 = 119, // 0x77
    LoadFlowVolume = 120, // 0x78
    LoadReturnVolume = 121, // 0x79
    LoadMaxHourlyFlow = 122, // 0x7A
    LoadMinHourlyFlow = 123, // 0x7B
    LoadMaxHourlyTemp = 124, // 0x7C
    LoadMinHourlyTemp = 125, // 0x7D
    LoadCycleSeconds = 126, // 0x7E
    LoadStateCounter = 127, // 0x7F
    LoadMax5MinutesFlow = 128, // 0x80
    LoadMin5MinutesFlow = 129, // 0x81
    LoadMax5MinutesTemp = 130, // 0x82
    LoadMin5MinutesTemp = 131, // 0x83
    LoadSmartFunctionState = 132, // 0x84
    LoadSmartFunctionTimeoutActive = 133, // 0x85
  }
}
