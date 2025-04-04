// Decompiled with JetBrains decompiler
// Type: EDC_Handler.Addresses
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;

#nullable disable
namespace EDC_Handler
{
  public sealed class Addresses
  {
    public const ushort ADDR_LOGAREA_START = 32768;

    public ushort LOG_ADDRESS_HALFMONTH_START { get; private set; }

    public ushort LOG_ADDRESS_HALFMONTH_END { get; private set; }

    public ushort LOG_ADDRESS_FULLMONTH_START { get; private set; }

    public ushort LOG_ADDRESS_FULLMONTH_END { get; private set; }

    public ushort LOG_ADDRESS_STICHTAG_START { get; private set; }

    public ushort LOG_ADDRESS_STICHTAG_END { get; private set; }

    public ushort log_halfmonth_address { get; private set; }

    public ushort log_fullmonth_address { get; private set; }

    public ushort log_stichtag_address { get; private set; }

    public int NextIndexFullmonth
    {
      get => ((int) this.log_fullmonth_address - (int) this.LOG_ADDRESS_FULLMONTH_START) / 8;
    }

    public int NextIndexHalfmonth
    {
      get => ((int) this.log_halfmonth_address - (int) this.LOG_ADDRESS_HALFMONTH_START) / 8;
    }

    public int NextIndexStichtag
    {
      get => ((int) this.log_stichtag_address - (int) this.LOG_ADDRESS_STICHTAG_START) / 8;
    }

    public static Addresses Get(EDC_Meter meter)
    {
      if (meter == null)
        return (Addresses) null;
      return new Addresses()
      {
        LOG_ADDRESS_HALFMONTH_START = 32768,
        LOG_ADDRESS_HALFMONTH_END = 34304,
        LOG_ADDRESS_FULLMONTH_START = 34304,
        LOG_ADDRESS_FULLMONTH_END = 35840,
        LOG_ADDRESS_STICHTAG_START = 35840,
        LOG_ADDRESS_STICHTAG_END = 36096,
        log_halfmonth_address = BitConverter.ToUInt16(meter.Map.GetMemoryBytes(meter.GetParameter("log_halfmonth_address")), 0),
        log_fullmonth_address = BitConverter.ToUInt16(meter.Map.GetMemoryBytes(meter.GetParameter("log_fullmonth_address")), 0),
        log_stichtag_address = BitConverter.ToUInt16(meter.Map.GetMemoryBytes(meter.GetParameter("log_stichtag_address")), 0)
      };
    }

    public static void GetRamLogger(
      DeviceVersion version,
      out ushort startRamLogger,
      out ushort endRamLogger)
    {
      Parameter parameter = ((version != null ? EDC_MemoryMap.GetParameter(version) : throw new ArgumentNullException(nameof (version))) ?? throw new Exception("Can not determine the RAM logger addresses! No MAP file available for version: " + version?.ToString())).Find((Predicate<Parameter>) (e => e.Name == "log_fifos"));
      if (parameter == null)
        throw new Exception("INTERNAL ERROR: Invalid MAP file! Can not find the parameter log_fifos. Version: " + version?.ToString());
      startRamLogger = (ushort) ((uint) parameter.Address - 976U);
      endRamLogger = (ushort) ((uint) parameter.Address + 70U);
    }
  }
}
