// Decompiled with JetBrains decompiler
// Type: HandlerLib.SpecialCommands_EFC
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public enum SpecialCommands_EFC : byte
  {
    GetSpecialCommandFCVersion_0x00 = 0,
    GetCurrentMeasuringMode_0x01 = 1,
    GetSetMetrologyParameters_0x02 = 2,
    GetSetProductionFactor_0x04 = 4,
    GetSetCountingMode_0x05 = 5,
    GetSetSummertimeCouningSuppression_0x06 = 6,
    GetClearFlowCheckStates_0x09 = 9,
    GetSetSDRules_0x0A = 10, // 0x0A
    SendToNfcDevice_0x0B = 11, // 0x0B
    GetNfcDeviceIdentification_0x0C = 12, // 0x0C
    SetReligiousDay_0x0D = 13, // 0x0D
    SetGetSmartFunctions_0x0E = 14, // 0x0E
  }
}
