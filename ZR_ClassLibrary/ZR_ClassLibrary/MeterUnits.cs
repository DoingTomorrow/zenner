// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.MeterUnits
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace ZR_ClassLibrary
{
  public static class MeterUnits
  {
    public static ResolutionData[] AllUnits = new ResolutionData[50]
    {
      new ResolutionData("0.0000L", "m\u00B3", 0.001M, 10000M, -1),
      new ResolutionData("0.000L", "m\u00B3", 0.001M, 1000M, 16),
      new ResolutionData("0.00L", "m\u00B3", 0.001M, 100M, 17),
      new ResolutionData("0.0L", "m\u00B3", 0.001M, 10M, 18),
      new ResolutionData("0L", "m\u00B3", 0.001M, 1M, 19),
      new ResolutionData("0.000m\u00B3", "m\u00B3", 1M, 1000M, 19),
      new ResolutionData("0.00m\u00B3", "m\u00B3", 1M, 100M, 20),
      new ResolutionData("0.0m\u00B3", "m\u00B3", 1M, 10M, 21),
      new ResolutionData("0m\u00B3", "m\u00B3", 1M, 1M, 22),
      new ResolutionData("0.000Wh", "MWh", 0.000001M, 1000M, 0),
      new ResolutionData("0.00Wh", "MWh", 0.000001M, 100M, 1),
      new ResolutionData("0.0Wh", "MWh", 0.000001M, 10M, 2),
      new ResolutionData("0Wh", "MWh", 0.000001M, 1M, 3),
      new ResolutionData("0.000kWh", "MWh", 0.001M, 1000M, 3),
      new ResolutionData("0.00kWh", "MWh", 0.001M, 100M, 4),
      new ResolutionData("0.0kWh", "MWh", 0.001M, 10M, 5),
      new ResolutionData("0kWh", "MWh", 0.001M, 1M, 6),
      new ResolutionData("0.000MWh", "MWh", 1M, 1000M, 6),
      new ResolutionData("0.00MWh", "MWh", 1M, 100M, 7),
      new ResolutionData("0J", "GJ", 0.000000001M, 1M, 8),
      new ResolutionData("0.000kJ", "GJ", 0.000001M, 1000M, 8),
      new ResolutionData("0.00kJ", "GJ", 0.000001M, 100M, 9),
      new ResolutionData("0.0kJ", "GJ", 0.000001M, 10M, 10),
      new ResolutionData("0kJ", "GJ", 0.000001M, 1M, 11),
      new ResolutionData("0.000MJ", "GJ", 0.001M, 1000M, 11),
      new ResolutionData("0.00MJ", "GJ", 0.001M, 100M, 12),
      new ResolutionData("0.0MJ", "GJ", 0.001M, 10M, 13),
      new ResolutionData("0MJ", "GJ", 0.001M, 1M, 14),
      new ResolutionData("0.000GJ", "GJ", 1M, 1000M, 14),
      new ResolutionData("0.00GJ", "GJ", 1M, 100M, 15),
      new ResolutionData("0.0000", "1", 1M, 10000M, 110),
      new ResolutionData("0.000", "1", 1M, 1000M, 110),
      new ResolutionData("0.00", "1", 1M, 100M, 110),
      new ResolutionData("0.0", "1", 1M, 10M, 110),
      new ResolutionData("0", "1", 1M, 1M, 110),
      new ResolutionData("0.0L/h", "L/h", 1M, 10M, 58),
      new ResolutionData("0L/h", "L/h", 1M, 1M, 59),
      new ResolutionData("0.000m\u00B3/h", "L/h", 1000M, 1000M, 59),
      new ResolutionData("0.00m\u00B3/h", "L/h", 1000M, 100M, 60),
      new ResolutionData("0.0m\u00B3/h", "L/h", 1000M, 10M, 61),
      new ResolutionData("0m\u00B3/h", "L/h", 1000M, 10M, 62),
      new ResolutionData("0.000kW", "kW", 1M, 1000M, 43),
      new ResolutionData("0.00kW", "kW", 1M, 100M, 44),
      new ResolutionData("0.0kW", "kW", 1M, 10M, 45),
      new ResolutionData("0kW", "kW", 1M, 1M, 46),
      new ResolutionData("0.000MW", "kW", 1000M, 1000M, 46),
      new ResolutionData("0.00MW", "kW", 1000M, 100M, 47),
      new ResolutionData("0.0000GJ/h", "GJ/h", 1M, 10000M, 53),
      new ResolutionData("0.000GJ/h", "GJ/h", 1M, 1000M, 54),
      new ResolutionData("0.00GJ/h", "GJ/h", 1M, 100M, 55)
    };
    public static SortedList<string, ResolutionData> resolutionDataFromResolutionString = new SortedList<string, ResolutionData>();

    static MeterUnits()
    {
      for (ushort index = 0; (int) index < MeterUnits.AllUnits.Length; ++index)
        MeterUnits.resolutionDataFromResolutionString.Add(MeterUnits.AllUnits[(int) index].resolutionString, MeterUnits.AllUnits[(int) index]);
    }

    public static ResolutionData GetResolutionData(byte vif)
    {
      return new List<ResolutionData>((IEnumerable<ResolutionData>) MeterUnits.AllUnits).Find((Predicate<ResolutionData>) (x => x.mbusVIF == (int) vif));
    }

    public static ResolutionData GetResolutionData(string resolutionString)
    {
      return new List<ResolutionData>((IEnumerable<ResolutionData>) MeterUnits.AllUnits).Find((Predicate<ResolutionData>) (x => x.resolutionString == resolutionString));
    }
  }
}
