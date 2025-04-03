// Decompiled with JetBrains decompiler
// Type: HandlerLib.CommonMBusCommands_EFC
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public enum CommonMBusCommands_EFC : byte
  {
    GetSetMBusChannelAddress_0x01 = 1,
    GetSetChannelIdentification_0x02 = 2,
    GetSetChannelOBIS_Code_0x03 = 3,
    GetSetChannelConfiguration_0x04 = 4,
    GetSetChannelValue_0x05 = 5,
    ReadChannelLogValue_0x10 = 16, // 0x10
    ReadEventLog_0x11 = 17, // 0x11
    ClearEventLog_0x12 = 18, // 0x12
    ReadSystemLog_0x13 = 19, // 0x13
    ClearSystemLog_0x14 = 20, // 0x14
    ReadChannelSingleLogValue_0x15 = 21, // 0x15
    GetSetRadioList_0x16 = 22, // 0x16
    GetSetTXTimings_0x17 = 23, // 0x17
    GetSetMBusKey_0x18 = 24, // 0x18
  }
}
