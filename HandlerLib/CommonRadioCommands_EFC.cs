// Decompiled with JetBrains decompiler
// Type: HandlerLib.CommonRadioCommands_EFC
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public enum CommonRadioCommands_EFC : byte
  {
    GetRadioVersion_0x00 = 0,
    GetSetTransmitPower_0x05 = 5,
    GetSetCenterFrequency_0x06 = 6,
    GetSetFrequencyIncrement_0x07 = 7,
    GetSetCarrierMode_0x08 = 8,
    GetSetFrequencyDeviation_0x09 = 9,
    GetSetBandWidth_0x0a = 10, // 0x0A
    GetSetTxDataRate_0x0b = 11, // 0x0B
    GetSetRxDataRate_0x0c = 12, // 0x0C
    StopRadioTest_0x20 = 32, // 0x20
    TransmitUnmodulatedCarrier_0x21 = 33, // 0x21
    TransmitModulatedCarrier_0x22 = 34, // 0x22
    SendTestPacket_0x23 = 35, // 0x23
    ReceiveOneR3S3_Telegram_0x24 = 36, // 0x24
    ReceiveAndStreamR3S3_Telegrams_0x25 = 37, // 0x25
    MonitorRadioToOutput_0x26 = 38, // 0x26
    EchoRadio3viaRadio_0x27 = 39, // 0x27
    GetSetTxBandWidth_0x28 = 40, // 0x28
    StartTransmissionCylcle_0x29 = 41, // 0x29
    SetNFCField_0x2a = 42, // 0x2A
  }
}
