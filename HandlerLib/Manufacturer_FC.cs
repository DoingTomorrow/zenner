// Decompiled with JetBrains decompiler
// Type: HandlerLib.Manufacturer_FC
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public enum Manufacturer_FC : byte
  {
    GetVersion_0x06 = 6,
    CommonRadioCommands_0x2f = 47, // 0x2F
    CommonMBusCommands_0x34 = 52, // 0x34
    CommonLoRaCommands_0x35 = 53, // 0x35
    SpecialCommands_0x36 = 54, // 0x36
    CommonNBIoTCommands_0x37 = 55, // 0x37
    SendNfcCommand_0x38 = 56, // 0x38
    ResetDevice_0x80 = 128, // 0x80
    SetWriteProtection_0x81 = 129, // 0x81
    OpenWriteProtectionTemp_0x82 = 130, // 0x82
    GetSetMode_0x83 = 131, // 0x83
    ReadMemory_0x84 = 132, // 0x84
    WriteMemory_0x85 = 133, // 0x85
    RunBackup_0x86 = 134, // 0x86
    GetSetSystemTime_0x87 = 135, // 0x87
    GetSetKeyDate_0x88 = 136, // 0x88
    GetSetRadioOperation_0x89 = 137, // 0x89
    ClearGetResetCounter_0x8a = 138, // 0x8A
    SetLcdTestState_0x8b = 139, // 0x8B
    SwitchLoRaLED_0x8c = 140, // 0x8C
    ActivateDeactivateDisplay_0x8d = 141, // 0x8D
    TimeShift_0x8e = 142, // 0x8E
    ExecuteEvent_0x8f = 143, // 0x8F
    SetRTCCalibration_0x90 = 144, // 0x90
    GetCommunicationScenario_0x91 = 145, // 0x91
    SetCommunicationScenario_0x92 = 146, // 0x92
    GetPrintedSerialNumber_0x93 = 147, // 0x93
    GetLocalInterfaceEncryption_0x94 = 148, // 0x94
    SetLocalInterfaceEncryption_0x94 = 148, // 0x94
  }
}
