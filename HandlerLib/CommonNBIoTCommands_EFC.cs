// Decompiled with JetBrains decompiler
// Type: HandlerLib.CommonNBIoTCommands_EFC
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public enum CommonNBIoTCommands_EFC : byte
  {
    GetNBIoTModulePartNumber_0x00 = 0,
    GetNBIoTFirmwareVersion_0x01 = 1,
    GetNBIoTIMEI_NB_0x02 = 2,
    GetSIM_IMSI_NB_0x03 = 3,
    GetSetProtocol_0x04 = 4,
    GetSetBand_0x05 = 5,
    GetSetRemoteIP_0x06 = 6,
    GetSetRemotePort_0x07 = 7,
    GetSetOperator_0x08 = 8,
    SendConfirmedData_0x09 = 9,
    SendUnconfirmedData_0x0A = 10, // 0x0A
    SendTestData_0x0B = 11, // 0x0B
    SendRadioFullFunctionOn_Off_0x0C = 12, // 0x0C
    GetSIM_ICCID_NB_0x0D = 13, // 0x0D
    GetSetSecondaryBand_0x0E = 14, // 0x0E
    GetSetDNSName_0x0F = 15, // 0x0F
    GetRadioSendingState_0x10 = 16, // 0x10
    ResetNBModem_0x11 = 17, // 0x11
    SetNBModemAutoOrManualConnect_0x12 = 18, // 0x12
    SetGetNBModemAPN_0x13 = 19, // 0x13
    SetGetDNSServerIP_0x14 = 20, // 0x14
    SetNBIoTPowerOn_Off_0x20 = 32, // 0x20
    NBIoTCommonCommand_0x21 = 33, // 0x21
    GetIMEI_IMSI_RAM_0x22 = 34, // 0x22
    GetICCID_IMSI_RAM_0x23 = 35, // 0x23
    GetSetDevEUI_0x25 = 37, // 0x25
    GetSetTransmissionScenario_0x28 = 40, // 0x28
    SetGetDNSEnableByte_0x29 = 41, // 0x29
    SetGetAPNEnableByte_0x30 = 48, // 0x30
    SendActivePacket_0x31 = 49, // 0x31
  }
}
