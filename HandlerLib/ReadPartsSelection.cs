// Decompiled with JetBrains decompiler
// Type: HandlerLib.ReadPartsSelection
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;

#nullable disable
namespace HandlerLib
{
  [Flags]
  public enum ReadPartsSelection : uint
  {
    Dump = 2147483647, // 0x7FFFFFFF
    All = 536870911, // 0x1FFFFFFF
    AllWithoutLogger = 251723775, // 0x0F00FFFF
    IdentificationMask = 15, // 0x0000000F
    FirmwareVersion = 1,
    Identification = 2,
    EnhancedIdentification = 4,
    ConfigMask = 240, // 0x000000F0
    CompleteConfiguration = ConfigMask, // 0x000000F0
    BasicConfiguration = 16, // 0x00000010
    Calibration = 32, // 0x00000020
    RangesMask = 3840, // 0x00000F00
    RAM_range = 256, // 0x00000100
    FLASH_range = 512, // 0x00000200
    BACKUP_range = 1024, // 0x00000400
    CLONE_range = 2048, // 0x00000800
    CumulatedDataMask = 61440, // 0x0000F000
    AllCumulatedData = CumulatedDataMask, // 0x0000F000
    CurrentMeasurementValues = 4096, // 0x00001000
    LoggersMask = 2031616, // 0x001F0000
    AllLoggers = LoggersMask, // 0x001F0000
    KeyData = 65536, // 0x00010000
    MonthLogger = 131072, // 0x00020000
    SmartFunctionLoggers = 524288, // 0x00080000
    SmartFunctions = 2097152, // 0x00200000
    ScenarioMask = 12582912, // 0x00C00000
    AllScenarios = ScenarioMask, // 0x00C00000
    ScenarioConfiguration = 4194304, // 0x00400000
    DeveloperMask = 1879048192, // 0x70000000
    BackupBlocks = 268435456, // 0x10000000
    RamDiagnosticParameters = 536870912, // 0x20000000
    ProtocolOnlyMode = 2147483648, // 0x80000000
  }
}
