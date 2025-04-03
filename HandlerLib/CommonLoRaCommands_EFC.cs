// Decompiled with JetBrains decompiler
// Type: HandlerLib.CommonLoRaCommands_EFC
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public enum CommonLoRaCommands_EFC : byte
  {
    GetLoRaFC_Version_0x00 = 0,
    GetLoRaWAN_Version_0x01 = 1,
    SendJoinRequest_0x06 = 6,
    CheckJoinAccept_0x07 = 7,
    SendUnconfirmedData_0x08 = 8,
    SendConfirmedData_0x09 = 9,
    GetSetNetID_0x20 = 32, // 0x20
    GetSetAppEUI_0x21 = 33, // 0x21
    GetSetNwkSKey_0x22 = 34, // 0x22
    GetSetAppSKey_0x23 = 35, // 0x23
    GetSetOTAA_ABP_0x24 = 36, // 0x24
    GetSetDevEUI_0x25 = 37, // 0x25
    GetSetAppKey_0x26 = 38, // 0x26
    GetSetDevAddr_0x27 = 39, // 0x27
    GetSetTransmissionScenario_0x28 = 40, // 0x28
    SendConfigurationPacket_0x29 = 41, // 0x29
    GetSetADR_0x2A = 42, // 0x2A
    SystemDiagnosticState_0x2B = 43, // 0x2B
    TriggerSystemDiagnostic_0x2B = 43, // 0x2B
  }
}
