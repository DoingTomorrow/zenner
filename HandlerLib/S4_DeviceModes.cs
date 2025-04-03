// Decompiled with JetBrains decompiler
// Type: HandlerLib.S4_DeviceModes
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public enum S4_DeviceModes
  {
    OperationMode = 0,
    DeliveryMode = 1,
    TestModePrepared = 128, // 0x00000080
    FlyingTestStart = 145, // 0x00000091
    FlyingTestRun = 146, // 0x00000092
    FlyingTestOrderStop = 147, // 0x00000093
    FlyingTestStoped = 148, // 0x00000094
    CurrentTest = 161, // 0x000000A1
    ZeroOffsetTestStart = 177, // 0x000000B1
    ZeroOffsetTestOrderStop = 179, // 0x000000B3
    ZeroOffsetTestStoped = 180, // 0x000000B4
    RtcCalibrationTestStart = 193, // 0x000000C1
    RtcCalibrationOrderStop = 195, // 0x000000C3
    RtcCalibrationStoped = 196, // 0x000000C4
    TdcLevelTest = 209, // 0x000000D1
    LcdTest = 225, // 0x000000E1
    RadioTestTransmitUnmodulatedCarrier = 227, // 0x000000E3
    RadioTestTransmitUnmodulatedCarrierFinished = 228, // 0x000000E4
    RadioTestTransmitModulatedCarrier = 229, // 0x000000E5
    RadioTestTransmitModulatedCarrierFinished = 230, // 0x000000E6
    RadioTestSendTestPacket = 231, // 0x000000E7
    RadioTestSendTestPacketFinished = 232, // 0x000000E8
    RadioTestReceiveTestPacket = 233, // 0x000000E9
    RadioTestReceiveTestPacketDone = 234, // 0x000000EA
    RadioTestReceiveTestPacketTimeout = 235, // 0x000000EB
    RadioTestRadioSimulation = 236, // 0x000000EC
  }
}
