// Decompiled with JetBrains decompiler
// Type: HandlerLib.SmartFunctionResult
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public enum SmartFunctionResult : ushort
  {
    NoError = 0,
    IllegalStorageTypeCode = 1,
    NoData = 2,
    EndOfAsciiStringNotFound = 3,
    IllegalDateStamp = 4,
    IllegalDateTimeStamp = 5,
    OutOfMemory = 6,
    LoggerParameterNotFirstParameter = 7,
    LoggerParameterNoUInt16 = 8,
    IllegalLoggerStorage = 9,
    FlashOutOfMemory = 10, // 0x000A
    RamOutOfMemory = 11, // 0x000B
    BackupOutOfMemory = 12, // 0x000C
    IllegalDataTypeCode = 13, // 0x000D
    RuntimeLoop = 14, // 0x000E
    WorkLevelExceeded = 15, // 0x000F
    StringLength20Exceeded = 16, // 0x0010
    UnknownMemory = 17, // 0x0011
    RegisterOpterationWithDifferentTypes = 18, // 0x0012
    NotSupportedOpcode = 19, // 0x0013
    StorageNotAllowedForLogger = 20, // 0x0014
    LoggerParameterNotInitialised = 21, // 0x0015
    LoggerWithoutData = 22, // 0x0016
    IllegalRegisterTypeForSaveEvent = 23, // 0x0017
    IllegalRegisterTypeForLoRaSendAlarm = 24, // 0x0018
    IllegalStateCounter = 25, // 0x0019
    ToManyParameters = 26, // 0x001A
    NotSupportedEvent = 27, // 0x001B
    IllegalParameterValue = 28, // 0x001C
    NotSupportedCycleSeconds = 29, // 0x001D
    NotSupportedInterpreterVersion = 30, // 0x001E
    FunctionAlreadyInstalled = 31, // 0x001F
    FunctionNotFound = 32, // 0x0020
    ParameterNotFound = 33, // 0x0021
    LongRunTime30ms = 34, // 0x0022
    RegisterOperationNAN = 35, // 0x0023
    RegisterOperationDivNULL = 36, // 0x0024
    DeactivatedByCommand = 65535, // 0xFFFF
  }
}
