// Decompiled with JetBrains decompiler
// Type: HandlerLib.NFC.NfcCommands
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib.NFC
{
  public enum NfcCommands : byte
  {
    GetIdentification = 1,
    GetAvailableParameterGroupSettings = 2,
    GetSelectedParameterGroupSettings = 3,
    ClearAllConfigurableParameterGroups = 4,
    AddConfigurableParameterGroup = 5,
    GetAvailableParameterGroupData = 6,
    GetSelectedParameterGroupData = 7,
    AddManagedParameter = 10, // 0x0A
    WriteMemory = 11, // 0x0B
    ReadMemory = 12, // 0x0C
    SetTestMode = 14, // 0x0E
    GetSystemState = 15, // 0x0F
    GetChallengeResponseRandomValue = 16, // 0x10
    GetDeviceAesKey = 17, // 0x11
    SetRtcCalibrationValue = 20, // 0x14
    SaveBackup = 21, // 0x15
    GetSystemDateTime = 22, // 0x16
    SetSystemDateTime = 23, // 0x17
    UnlockDevice = 24, // 0x18
    LockDevice = 25, // 0x19
    ResetDevice = 26, // 0x1A
    GetModuleConfiguration = 27, // 0x1B
    GetMeterConfiguration = 28, // 0x1C
    SetMeterConfiguration = 29, // 0x1D
    GetBatteryEndDate = 30, // 0x1E
    SetBatteryEndDate = 31, // 0x1F
    IrDa_Compatible_Command = 32, // 0x20
    GetLoggerList = 33, // 0x21
    ReadLogger = 34, // 0x22
    GetSmartFunctionsList = 35, // 0x23
    GetSmartFunction = 36, // 0x24
    LoadSmartFunction = 37, // 0x25
    DeleteAllSmartFunctions = 38, // 0x26
    GetSmartFunctionParameters = 39, // 0x27
    SetSmartFunctionParameters = 40, // 0x28
    GetVolumeAndFlow = 41, // 0x29
    ChecksumManagement = 42, // 0x2A
    SetModuleConfiguration = 43, // 0x2B
    GetAliveAndStatus = 44, // 0x2C
    SimulateLoggerEvent = 45, // 0x2D
    GetStateCounters = 46, // 0x2E
    SetGroupsForScenario = 47, // 0x2F
    GetSmartFunctionInfo = 48, // 0x30
    ReInitMeasurement = 49, // 0x31
    SetNDC_ModuleState = 50, // 0x32
    GetAvailableModuleConfigurations = 51, // 0x33
    UpdateNdef = 52, // 0x34
    ClearEventLogger = 53, // 0x35
    SetAccumulatedValues = 54, // 0x36
    SetSmartFunctionActivation = 55, // 0x37
    GetBusModuleList = 56, // 0x38
    SendToBusModule = 57, // 0x39
    CallTestFunction = 58, // 0x3A
    ErrorCommand = 254, // 0xFE
    SubUnitCommand = 255, // 0xFF
  }
}
