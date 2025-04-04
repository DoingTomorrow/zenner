// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.FirmwareEvents
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

#nullable disable
namespace SmartFunctionCompiler
{
  public enum FirmwareEvents : byte
  {
    NotDefined,
    Minute,
    Minutes5,
    Minutes10,
    Minutes15,
    Minutes30,
    Hour,
    Hours3,
    Hours6,
    Hours12,
    Day,
    Days3,
    Week,
    Weeks2,
    HalfMonth,
    Month,
    Month3,
    HalfYear,
    Year,
    FlowCalculated,
    FlowChanged,
    VolumeCalculated,
    PowerCalculated,
    PowerChanged,
    TemperatureCalculated,
    TemperaturChanged,
    EnergyCalculated,
    StateDefinition,
    LoRaDefinition,
  }
}
