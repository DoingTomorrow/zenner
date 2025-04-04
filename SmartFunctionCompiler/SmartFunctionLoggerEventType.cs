// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.SmartFunctionLoggerEventType
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

#nullable disable
namespace SmartFunctionCompiler
{
  public enum SmartFunctionLoggerEventType : byte
  {
    LoggerEvent_None = 0,
    LoggerEvent_Leakage = 1,
    LoggerEvent_ReverseInstallation = 2,
    LoggerEvent_BatteryWarning = 3,
    LoggerEvent_Oversized = 4,
    LoggerEvent_Undersized = 5,
    LoggerEvent_Burst = 6,
    LoggerEvent_Dry = 7,
    LoggerEvent_Frost = 8,
    LoggerEvent_Backflow = 9,
    LoggerEvent_NoConsumption = 10, // 0x0A
    LoggerEvent_PermanentFlow = 11, // 0x0B
    LoggerEvent_Overload = 12, // 0x0C
    LoggerEvent_Auto_OFF = 128, // 0x80
    LoggerEvent_Leakage_OFF = 129, // 0x81
    LoggerEvent_ReverseInstallation_OFF = 130, // 0x82
    LoggerEvent_BatteryWarning_OFF = 131, // 0x83
    LoggerEvent_Oversized_OFF = 132, // 0x84
    LoggerEvent_Undersized_OFF = 133, // 0x85
    LoggerEvent_Burst_OFF = 134, // 0x86
    LoggerEvent_Dry_OFF = 135, // 0x87
    LoggerEvent_Frost_OFF = 136, // 0x88
    LoggerEvent_Backflow_OFF = 137, // 0x89
    LoggerEvent_NoConsumption_OFF = 138, // 0x8A
    LoggerEvent_PermanentFlow_OFF = 139, // 0x8B
    LoggerEvent_Overload_OFF = 140, // 0x8C
  }
}
