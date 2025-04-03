// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.FunctionRecordType
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

#nullable disable
namespace ZR_ClassLibrary
{
  public enum FunctionRecordType
  {
    None = -1, // 0xFFFFFFFF
    RuntimeVars = 0,
    DisplayCode = 2,
    ResetRuntimeCode = 3,
    MesurementRuntimeCode = 4,
    CycleRuntimeCode = 5,
    MBusRuntimeCode = 6,
    RuntimeConstants = 7,
    Lable = 8,
    Pointer = 9,
    EnergyFrameCode = 10, // 0x0000000A
    VolumeFrame = 11, // 0x0000000B
    Input1Frame = 12, // 0x0000000C
    Input2Frame = 13, // 0x0000000D
    Input3Frame = 14, // 0x0000000E
    Event_Click = 15, // 0x0000000F
    Event_Press = 16, // 0x00000010
    Event_Hold = 17, // 0x00000011
    Event_Timeout = 18, // 0x00000012
    Event_None = 19, // 0x00000013
    ResourcesRequired = 20, // 0x00000014
    ResourcesSupplied = 21, // 0x00000015
    FlowFrame = 22, // 0x00000016
    PowerFrame = 23, // 0x00000017
    FunctionFlags = 24, // 0x00000018
    BackupRuntimeVars = 25, // 0x00000019
  }
}
