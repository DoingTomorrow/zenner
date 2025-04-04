// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.MeterMath
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Resources;
using System.Threading;

#nullable disable
namespace ZR_ClassLibrary
{
  public abstract class MeterMath
  {
    internal ResourceManager MyRes;
    public MeterMath.Errors LastError = MeterMath.Errors.NoError;
    public string LastErrorInfo = string.Empty;
    public static MeterMath.ENERGY_UNIT_DEFS[] EnergyUnits = new MeterMath.ENERGY_UNIT_DEFS[35]
    {
      new MeterMath.ENERGY_UNIT_DEFS("0.000000Wh", "0.000001Wh/I", "0.000000W", (short) 6, (short) 0, (sbyte) -1),
      new MeterMath.ENERGY_UNIT_DEFS("0.00000Wh", "0.00001Wh/I", "0.00000W", (short) 5, (short) 1, (sbyte) 0),
      new MeterMath.ENERGY_UNIT_DEFS("0.0000Wh", "0.0001Wh/I", "0.0000W", (short) 4, (short) 2, (sbyte) 1),
      new MeterMath.ENERGY_UNIT_DEFS("0.000Wh", "0.001Wh/I", "0.000W", (short) 3, (short) 3, (sbyte) 1),
      new MeterMath.ENERGY_UNIT_DEFS("0.00Wh", "0.01Wh/I", "0.00W", (short) 2, (short) 4, (sbyte) 2),
      new MeterMath.ENERGY_UNIT_DEFS("0.0Wh", "0.1Wh/I", "0.0W", (short) 1, (short) 5, (sbyte) 3),
      new MeterMath.ENERGY_UNIT_DEFS("0Wh", "1Wh/I", "0W", (short) 0, (short) 6, (sbyte) 4),
      new MeterMath.ENERGY_UNIT_DEFS("0.000kWh", "1Wh/I", "0.000kW", (short) 3, (short) 6, (sbyte) 5),
      new MeterMath.ENERGY_UNIT_DEFS("0.00kWh", "10Wh/I", "0.00kW", (short) 2, (short) 7, (sbyte) 6),
      new MeterMath.ENERGY_UNIT_DEFS("0.0kWh", "100Wh/I", "0.0kW", (short) 1, (short) 8, (sbyte) 7),
      new MeterMath.ENERGY_UNIT_DEFS("0kWh", "1kWh/I", "0kW", (short) 0, (short) 9, (sbyte) 8),
      new MeterMath.ENERGY_UNIT_DEFS("0.000MWh", "1kWh/I", "0.000MW", (short) 3, (short) 9, (sbyte) 9),
      new MeterMath.ENERGY_UNIT_DEFS("0.00MWh", "10kWh/I", "0.00MW", (short) 2, (short) 10, (sbyte) 10),
      new MeterMath.ENERGY_UNIT_DEFS("0.0MWh", "100kWh/I", "0.0MW", (short) 1, (short) 11, (sbyte) 11),
      new MeterMath.ENERGY_UNIT_DEFS("0MWh", "1MWh/I", "0MW", (short) 0, (short) 12, (sbyte) 12),
      new MeterMath.ENERGY_UNIT_DEFS("0.000GWh", "1MWh/I", "0.000GW", (short) 3, (short) 12, (sbyte) 13),
      new MeterMath.ENERGY_UNIT_DEFS("0.00GWh", "10MWh/I", "0.00GW", (short) 2, (short) 13, (sbyte) 14),
      new MeterMath.ENERGY_UNIT_DEFS("0.0GWh", "100MWh/I", "0.0GW", (short) 1, (short) 14, (sbyte) 15),
      new MeterMath.ENERGY_UNIT_DEFS("0GWh", "1GWh/I", "0GW", (short) 0, (short) 15, (sbyte) 16),
      new MeterMath.ENERGY_UNIT_DEFS("0.000J", "0.001J/I", "0.000J/h", (short) 3, (short) 16, (sbyte) 17),
      new MeterMath.ENERGY_UNIT_DEFS("0.00J", "0.01J/I", "0.00J/h", (short) 2, (short) 17, (sbyte) 18),
      new MeterMath.ENERGY_UNIT_DEFS("0.0J", "0.1J/I", "0.0J/h", (short) 1, (short) 18, (sbyte) 19),
      new MeterMath.ENERGY_UNIT_DEFS("0J", "1J/I", "0J/h", (short) 0, (short) 19, (sbyte) 20),
      new MeterMath.ENERGY_UNIT_DEFS("0.000kJ", "1J/I", "0.000kJ/h", (short) 3, (short) 19, (sbyte) 21),
      new MeterMath.ENERGY_UNIT_DEFS("0.00kJ", "10J/I", "0.00kJ/h", (short) 2, (short) 20, (sbyte) 22),
      new MeterMath.ENERGY_UNIT_DEFS("0.0kJ", "100J/I", "0.0kJ/h", (short) 1, (short) 21, (sbyte) 23),
      new MeterMath.ENERGY_UNIT_DEFS("0kJ", "1kJ/I", "0kJ/h", (short) 0, (short) 22, (sbyte) 24),
      new MeterMath.ENERGY_UNIT_DEFS("0.000MJ", "1kJ/I", "0.000MJ/h", (short) 3, (short) 22, (sbyte) 25),
      new MeterMath.ENERGY_UNIT_DEFS("0.00MJ", "10kJ/I", "0.00MJ/h", (short) 2, (short) 23, (sbyte) 26),
      new MeterMath.ENERGY_UNIT_DEFS("0.0MJ", "100kJ/I", "0.0MJ/h", (short) 1, (short) 24, (sbyte) 27),
      new MeterMath.ENERGY_UNIT_DEFS("0MJ", "1MJ/I", "0MJ/h", (short) 0, (short) 25, (sbyte) 28),
      new MeterMath.ENERGY_UNIT_DEFS("0.000GJ", "1MJ/I", "0.000GJ/h", (short) 3, (short) 25, (sbyte) 29),
      new MeterMath.ENERGY_UNIT_DEFS("0.00GJ", "10MJ/I", "0.00GJ/h", (short) 2, (short) 26, (sbyte) 30),
      new MeterMath.ENERGY_UNIT_DEFS("0.0GJ", "100MJ/I", "0.0GJ/h", (short) 1, (short) 27, (sbyte) 31),
      new MeterMath.ENERGY_UNIT_DEFS("0GJ", "1GJ/I", "0GJ/h", (short) 0, (short) 28, (sbyte) 32)
    };
    public static MeterMath.LINEAR_ENERGY_UNIT_DEFS[] LinearEnergyUnits = new MeterMath.LINEAR_ENERGY_UNIT_DEFS[29]
    {
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(1000000.0, (short) -1, (short) -1),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(100000.0, (short) -1, (short) -1),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(10000.0, (short) -1, (short) -1),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(1000.0, (short) 0, (short) 40),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(100.0, (short) 1, (short) 41),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(10.0, (short) 2, (short) 42),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(1.0, (short) 3, (short) 43),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(0.1, (short) 4, (short) 44),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(0.01, (short) 5, (short) 45),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(0.001, (short) 6, (short) 46),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(0.0001, (short) 7, (short) 47),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(1E-05, (short) -1, (short) -1),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(1E-06, (short) -1, (short) -1),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(1E-07, (short) -1, (short) -1),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(1E-08, (short) -1, (short) -1),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(1E-09, (short) -1, (short) -1),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(3600000.0, (short) -1, (short) -1),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(360000.0, (short) -1, (short) -1),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(36000.0, (short) -1, (short) -1),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(3600.0, (short) 8, (short) 48),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(360.0, (short) 9, (short) 49),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(36.0, (short) 10, (short) 50),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(3.6, (short) 11, (short) 51),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(0.36, (short) 12, (short) 52),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(0.036, (short) 13, (short) 53),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(0.0036, (short) 14, (short) 54),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(0.00036, (short) 15, (short) 55),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(3.6E-05, (short) -1, (short) -1),
      new MeterMath.LINEAR_ENERGY_UNIT_DEFS(3.6E-06, (short) -1, (short) -1)
    };
    public static MeterMath.VOLUME_UNIT_DEFS[] VolumeUnits = new MeterMath.VOLUME_UNIT_DEFS[11]
    {
      new MeterMath.VOLUME_UNIT_DEFS("0.000000L", "0.000001L/I", "0.000000L/h", (short) 6, (short) 0, (sbyte) 0),
      new MeterMath.VOLUME_UNIT_DEFS("0.00000L", "0.00001L/I", "0.00000L/h", (short) 5, (short) 1, (sbyte) 1),
      new MeterMath.VOLUME_UNIT_DEFS("0.0000L", "0.0001L/I", "0.0000L/h", (short) 4, (short) 2, (sbyte) 2),
      new MeterMath.VOLUME_UNIT_DEFS("0.000L", "0.001L/I", "0.000L/h", (short) 3, (short) 3, (sbyte) 3),
      new MeterMath.VOLUME_UNIT_DEFS("0.00L", "0.01L/I", "0.00L/h", (short) 2, (short) 4, (sbyte) 4),
      new MeterMath.VOLUME_UNIT_DEFS("0.0L", "0.1L/I", "0.0L/h", (short) 1, (short) 5, (sbyte) 5),
      new MeterMath.VOLUME_UNIT_DEFS("0L", "1L/I", "0L/h", (short) 0, (short) 6, (sbyte) 6),
      new MeterMath.VOLUME_UNIT_DEFS("0.000m\u00B3", "1L/I", "0.000m\u00B3/h", (short) 3, (short) 6, (sbyte) 7),
      new MeterMath.VOLUME_UNIT_DEFS("0.00m\u00B3", "10L/I", "0.00m\u00B3/h", (short) 2, (short) 7, (sbyte) 8),
      new MeterMath.VOLUME_UNIT_DEFS("0.0m\u00B3", "100L/I", "0.0m\u00B3/h", (short) 1, (short) 8, (sbyte) 9),
      new MeterMath.VOLUME_UNIT_DEFS("0m\u00B3", "1000L/I", "0m\u00B3/h", (short) 0, (short) 9, (sbyte) 10)
    };
    public static MeterMath.LINEAR_VOLUME_UNIT_DEFS[] LinearVolumeUnits = new MeterMath.LINEAR_VOLUME_UNIT_DEFS[10]
    {
      new MeterMath.LINEAR_VOLUME_UNIT_DEFS(1000000.0, (short) -1, (short) -1, "L/1000000"),
      new MeterMath.LINEAR_VOLUME_UNIT_DEFS(100000.0, (short) -1, (short) -1, "L/100000"),
      new MeterMath.LINEAR_VOLUME_UNIT_DEFS(10000.0, (short) -1, (short) -1, "L/10000"),
      new MeterMath.LINEAR_VOLUME_UNIT_DEFS(1000.0, (short) 16, (short) 56, "L/1000"),
      new MeterMath.LINEAR_VOLUME_UNIT_DEFS(100.0, (short) 17, (short) 57, "L/100"),
      new MeterMath.LINEAR_VOLUME_UNIT_DEFS(10.0, (short) 18, (short) 58, "L/10"),
      new MeterMath.LINEAR_VOLUME_UNIT_DEFS(1.0, (short) 19, (short) 59, "L"),
      new MeterMath.LINEAR_VOLUME_UNIT_DEFS(0.1, (short) 20, (short) 60, "10 * L"),
      new MeterMath.LINEAR_VOLUME_UNIT_DEFS(0.01, (short) 21, (short) 61, "100 * L"),
      new MeterMath.LINEAR_VOLUME_UNIT_DEFS(0.001, (short) 22, (short) 62, "m\u00B3")
    };
    public static MeterMath.INPUT_UNIT_DEFS[] InputUnits = new MeterMath.INPUT_UNIT_DEFS[44]
    {
      new MeterMath.INPUT_UNIT_DEFS("0.000", MeterMath.InputFrameType.Empty, (short) 3, (short) 0, (sbyte) 0),
      new MeterMath.INPUT_UNIT_DEFS("0.00", MeterMath.InputFrameType.Empty, (short) 2, (short) 1, (sbyte) 1),
      new MeterMath.INPUT_UNIT_DEFS("0.0", MeterMath.InputFrameType.Empty, (short) 1, (short) 2, (sbyte) 2),
      new MeterMath.INPUT_UNIT_DEFS("0", MeterMath.InputFrameType.Empty, (short) 0, (short) 3, (sbyte) 3),
      new MeterMath.INPUT_UNIT_DEFS("0.000Wh", MeterMath.InputFrameType.Energy, (short) 3, (short) 4, (sbyte) 4),
      new MeterMath.INPUT_UNIT_DEFS("0.00Wh", MeterMath.InputFrameType.Energy, (short) 2, (short) 5, (sbyte) 5),
      new MeterMath.INPUT_UNIT_DEFS("0.0Wh", MeterMath.InputFrameType.Energy, (short) 1, (short) 6, (sbyte) 6),
      new MeterMath.INPUT_UNIT_DEFS("0Wh", MeterMath.InputFrameType.Energy, (short) 0, (short) 7, (sbyte) 7),
      new MeterMath.INPUT_UNIT_DEFS("0.000kWh", MeterMath.InputFrameType.Energy, (short) 3, (short) 7, (sbyte) 8),
      new MeterMath.INPUT_UNIT_DEFS("0.00kWh", MeterMath.InputFrameType.Energy, (short) 2, (short) 8, (sbyte) 9),
      new MeterMath.INPUT_UNIT_DEFS("0.0kWh", MeterMath.InputFrameType.Energy, (short) 1, (short) 9, (sbyte) 10),
      new MeterMath.INPUT_UNIT_DEFS("0kWh", MeterMath.InputFrameType.Energy, (short) 0, (short) 10, (sbyte) 11),
      new MeterMath.INPUT_UNIT_DEFS("0.000MWh", MeterMath.InputFrameType.Energy, (short) 3, (short) 10, (sbyte) 12),
      new MeterMath.INPUT_UNIT_DEFS("0.00MWh", MeterMath.InputFrameType.Energy, (short) 2, (short) 11, (sbyte) 13),
      new MeterMath.INPUT_UNIT_DEFS("0.0MWh", MeterMath.InputFrameType.Energy, (short) 1, (short) 12, (sbyte) 14),
      new MeterMath.INPUT_UNIT_DEFS("0MWh", MeterMath.InputFrameType.Energy, (short) 0, (short) 13, (sbyte) 15),
      new MeterMath.INPUT_UNIT_DEFS("0.000GWh", MeterMath.InputFrameType.Energy, (short) 3, (short) 13, (sbyte) 16),
      new MeterMath.INPUT_UNIT_DEFS("0.00GWh", MeterMath.InputFrameType.Energy, (short) 2, (short) 14, (sbyte) 17),
      new MeterMath.INPUT_UNIT_DEFS("0.0GWh", MeterMath.InputFrameType.Energy, (short) 1, (short) 15, (sbyte) 18),
      new MeterMath.INPUT_UNIT_DEFS("0GWh", MeterMath.InputFrameType.Energy, (short) 0, (short) 16, (sbyte) 19),
      new MeterMath.INPUT_UNIT_DEFS("0.000J", MeterMath.InputFrameType.Energy, (short) 3, (short) 17, (sbyte) 20),
      new MeterMath.INPUT_UNIT_DEFS("0.00J", MeterMath.InputFrameType.Energy, (short) 2, (short) 18, (sbyte) 21),
      new MeterMath.INPUT_UNIT_DEFS("0.0J", MeterMath.InputFrameType.Energy, (short) 1, (short) 19, (sbyte) 22),
      new MeterMath.INPUT_UNIT_DEFS("0J", MeterMath.InputFrameType.Energy, (short) 0, (short) 20, (sbyte) 23),
      new MeterMath.INPUT_UNIT_DEFS("0.000kJ", MeterMath.InputFrameType.Energy, (short) 3, (short) 20, (sbyte) 24),
      new MeterMath.INPUT_UNIT_DEFS("0.00kJ", MeterMath.InputFrameType.Energy, (short) 2, (short) 21, (sbyte) 25),
      new MeterMath.INPUT_UNIT_DEFS("0.0kJ", MeterMath.InputFrameType.Energy, (short) 1, (short) 22, (sbyte) 26),
      new MeterMath.INPUT_UNIT_DEFS("0kJ", MeterMath.InputFrameType.Energy, (short) 0, (short) 23, (sbyte) 27),
      new MeterMath.INPUT_UNIT_DEFS("0.000MJ", MeterMath.InputFrameType.Energy, (short) 3, (short) 23, (sbyte) 28),
      new MeterMath.INPUT_UNIT_DEFS("0.00MJ", MeterMath.InputFrameType.Energy, (short) 2, (short) 24, (sbyte) 29),
      new MeterMath.INPUT_UNIT_DEFS("0.0MJ", MeterMath.InputFrameType.Energy, (short) 1, (short) 25, (sbyte) 30),
      new MeterMath.INPUT_UNIT_DEFS("0MJ", MeterMath.InputFrameType.Energy, (short) 0, (short) 26, (sbyte) 31),
      new MeterMath.INPUT_UNIT_DEFS("0.000GJ", MeterMath.InputFrameType.Energy, (short) 3, (short) 26, (sbyte) 32),
      new MeterMath.INPUT_UNIT_DEFS("0.00GJ", MeterMath.InputFrameType.Energy, (short) 2, (short) 27, (sbyte) 33),
      new MeterMath.INPUT_UNIT_DEFS("0.0GJ", MeterMath.InputFrameType.Energy, (short) 1, (short) 28, (sbyte) 34),
      new MeterMath.INPUT_UNIT_DEFS("0GJ", MeterMath.InputFrameType.Energy, (short) 0, (short) 29, (sbyte) 35),
      new MeterMath.INPUT_UNIT_DEFS("0.000L", MeterMath.InputFrameType.Volume, (short) 3, (short) 30, (sbyte) 36),
      new MeterMath.INPUT_UNIT_DEFS("0.00L", MeterMath.InputFrameType.Volume, (short) 2, (short) 31, (sbyte) 37),
      new MeterMath.INPUT_UNIT_DEFS("0.0L", MeterMath.InputFrameType.Volume, (short) 1, (short) 32, (sbyte) 38),
      new MeterMath.INPUT_UNIT_DEFS("0L", MeterMath.InputFrameType.Volume, (short) 0, (short) 33, (sbyte) 39),
      new MeterMath.INPUT_UNIT_DEFS("0.000m\u00B3", MeterMath.InputFrameType.Volume, (short) 3, (short) 33, (sbyte) 40),
      new MeterMath.INPUT_UNIT_DEFS("0.00m\u00B3", MeterMath.InputFrameType.Volume, (short) 2, (short) 34, (sbyte) 41),
      new MeterMath.INPUT_UNIT_DEFS("0.0m\u00B3", MeterMath.InputFrameType.Volume, (short) 1, (short) 35, (sbyte) 42),
      new MeterMath.INPUT_UNIT_DEFS("0m\u00B3", MeterMath.InputFrameType.Volume, (short) 0, (short) 36, (sbyte) 43)
    };
    public static MeterMath.EMPTY_UNIT_DEFS[] EmptyUnits = new MeterMath.EMPTY_UNIT_DEFS[8]
    {
      new MeterMath.EMPTY_UNIT_DEFS("0.0000000", (short) 7, (short) 0),
      new MeterMath.EMPTY_UNIT_DEFS("0.000000", (short) 6, (short) 1),
      new MeterMath.EMPTY_UNIT_DEFS("0.00000", (short) 5, (short) 2),
      new MeterMath.EMPTY_UNIT_DEFS("0.0000", (short) 4, (short) 3),
      new MeterMath.EMPTY_UNIT_DEFS("0.000", (short) 3, (short) 4),
      new MeterMath.EMPTY_UNIT_DEFS("0.00", (short) 2, (short) 5),
      new MeterMath.EMPTY_UNIT_DEFS("0.0", (short) 1, (short) 6),
      new MeterMath.EMPTY_UNIT_DEFS("0", (short) 0, (short) 7)
    };
    public static MeterMath.LINEAR_EMPTY_UNIT_DEFS[] LinearEmptyUnits = new MeterMath.LINEAR_EMPTY_UNIT_DEFS[8]
    {
      new MeterMath.LINEAR_EMPTY_UNIT_DEFS(10000000.0, (short) -1),
      new MeterMath.LINEAR_EMPTY_UNIT_DEFS(1000000.0, (short) -1),
      new MeterMath.LINEAR_EMPTY_UNIT_DEFS(100000.0, (short) -1),
      new MeterMath.LINEAR_EMPTY_UNIT_DEFS(10000.0, (short) -1),
      new MeterMath.LINEAR_EMPTY_UNIT_DEFS(1000.0, (short) 16),
      new MeterMath.LINEAR_EMPTY_UNIT_DEFS(100.0, (short) 17),
      new MeterMath.LINEAR_EMPTY_UNIT_DEFS(10.0, (short) 18),
      new MeterMath.LINEAR_EMPTY_UNIT_DEFS(1.0, (short) 19)
    };

    public string getLastErrorString()
    {
      string name = this.LastError.ToString();
      string lastErrorString = this.MyRes.GetString(name);
      if (lastErrorString == null || lastErrorString.Length == 0)
        lastErrorString = "MeterMath error: " + name;
      if (this.LastErrorInfo.Length > 0)
      {
        lastErrorString = lastErrorString + ZR_Constants.SystemNewLine + this.LastErrorInfo;
        this.LastErrorInfo = string.Empty;
      }
      this.LastError = MeterMath.Errors.NoError;
      return lastErrorString;
    }

    public MeterMath()
    {
      this.MyRes = new ResourceManager("ZR_ClassLibrary.MeterMathRes", typeof (MeterMath).Assembly);
    }

    public virtual double calcPulsValue(int PulsValue, string UnitString)
    {
      throw new ApplicationException("Base class function my not be used!");
    }

    public virtual double calcPulsValue(double PulsValue, double Vol_SumExpo, string VolumeUnit)
    {
      throw new ApplicationException("Base class function my not be used!");
    }

    public virtual string GetUnitString(byte[] EEP_Data, int FrameOffset)
    {
      throw new ApplicationException("Base class function my not be used!");
    }

    public virtual bool GetDisplay(
      ByteField EEProm,
      uint EEPromSize,
      uint EEPromStartOffset,
      out bool[] Display)
    {
      throw new ApplicationException("Base class function my not be used!");
    }

    public virtual bool GetDisplay(
      byte[] EEProm,
      uint EEPromSize,
      uint EEPromStartOffset,
      out bool[] Display)
    {
      throw new ApplicationException("Base class function my not be used!");
    }

    public virtual bool CalculateMeterSettings(long FirmwareVersion)
    {
      throw new ApplicationException("Base class function my not be used!");
    }

    public virtual bool GetSpecialOverrideFrame(
      FrameTypes Type,
      string SpecialOptions,
      out MeterMath.FrameDescription TheFrame,
      out int Shift)
    {
      throw new ApplicationException("Base class function my not be used!");
    }

    public static string GetUnitText(string DisplayValue)
    {
      for (int index = 0; index < DisplayValue.Length; ++index)
      {
        if (DisplayValue[index] != '0' && DisplayValue[index] != '.')
          return DisplayValue.Substring(index);
      }
      return string.Empty;
    }

    public static string GetUnitMinValue(string DisplayValue)
    {
      for (int index = 0; index < DisplayValue.Length; ++index)
      {
        bool flag1 = index == DisplayValue.Length - 1;
        bool flag2 = DisplayValue[index] == '0';
        bool flag3 = DisplayValue[index] == '.';
        if (((flag2 ? 0 : (!flag3 ? 1 : 0)) | (flag1 ? 1 : 0)) != 0)
          return (DisplayValue.Substring(0, index - (flag1 ? 0 : 1)) + "1").Replace(".", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
      }
      return string.Empty;
    }

    public static Decimal GetWhFactorFromUnitText(string UnitText)
    {
      int energyUnitIndex = MeterMath.GetEnergyUnitIndex("0" + UnitText);
      return energyUnitIndex < 0 ? -1M : (Decimal) MeterMath.LinearEnergyUnits[(int) MeterMath.EnergyUnits[energyUnitIndex].LinearUnitIndex].UnitFactorFromWh;
    }

    public static Decimal GetWhFactor(string DisplayValue)
    {
      int energyUnitIndex = MeterMath.GetEnergyUnitIndex(DisplayValue);
      return energyUnitIndex < 0 ? -1M : (Decimal) MeterMath.LinearEnergyUnits[(int) MeterMath.EnergyUnits[energyUnitIndex].LinearUnitIndex].UnitFactorFromWh;
    }

    public static int GetEnergyUnitIndex(string DisplayValue)
    {
      for (int energyUnitIndex = 0; energyUnitIndex < MeterMath.EnergyUnits.Length; ++energyUnitIndex)
      {
        if (MeterMath.EnergyUnits[energyUnitIndex].EnergieUnitString.ToUpper() == DisplayValue.ToUpper())
          return energyUnitIndex;
      }
      return -1;
    }

    public static string GetEnergyUnitOfID(int OverwriteID)
    {
      string energyUnitOfId = "";
      for (int index = 0; index < MeterMath.EnergyUnits.Length; ++index)
      {
        if ((int) MeterMath.EnergyUnits[index].OverwriteID == OverwriteID)
        {
          energyUnitOfId = MeterMath.EnergyUnits[index].EnergieUnitString;
          break;
        }
      }
      return energyUnitOfId;
    }

    public static int GetEnergyUnitOverwriteID(string DisplayValue)
    {
      for (int index = 0; index < MeterMath.EnergyUnits.Length; ++index)
      {
        if (MeterMath.EnergyUnits[index].EnergieUnitString.ToUpper() == DisplayValue.ToUpper())
          return (int) MeterMath.EnergyUnits[index].OverwriteID;
      }
      return -1;
    }

    public static string GetTrueEnergyUnitString(string TheString)
    {
      for (int index = 0; index < MeterMath.EnergyUnits.Length; ++index)
      {
        if (MeterMath.EnergyUnits[index].EnergieUnitString.ToUpper() == TheString.ToUpper())
          return MeterMath.EnergyUnits[index].EnergieUnitString;
      }
      return "";
    }

    public static int GetEnergyUnitIndexFromLiniarUnitIndex(int LinearUnitIndex)
    {
      for (int index = 0; index < MeterMath.EnergyUnits.Length; ++index)
      {
        if ((int) MeterMath.EnergyUnits[index].LinearUnitIndex == LinearUnitIndex)
          return index == MeterMath.EnergyUnits.Length - 1 || (int) MeterMath.EnergyUnits[index + 1].LinearUnitIndex != LinearUnitIndex ? index : index + 1;
      }
      return -1;
    }

    public static Decimal GetLiterFactorFromUnitText(string UnitText)
    {
      string DisplayValue = "0" + UnitText;
      int volumeUnitIndex = MeterMath.GetVolumeUnitIndex(DisplayValue);
      if (volumeUnitIndex >= 0)
        return (Decimal) MeterMath.LinearVolumeUnits[(int) MeterMath.VolumeUnits[volumeUnitIndex].LinearUnitIndex].UnitFactorFromLiter;
      int energyUnitIndex = MeterMath.GetEnergyUnitIndex(DisplayValue);
      if (energyUnitIndex >= 0)
        return (Decimal) MeterMath.LinearEnergyUnits[(int) MeterMath.EnergyUnits[energyUnitIndex].LinearUnitIndex].UnitFactorFromWh;
      int emptyUnitIndex = MeterMath.GetEmptyUnitIndex(DisplayValue);
      return emptyUnitIndex >= 0 ? (Decimal) MeterMath.LinearEmptyUnits[(int) MeterMath.EmptyUnits[emptyUnitIndex].LinearUnitIndex].UnitFactorFromEmpty : 1M;
    }

    public static Decimal GetLiterFactor(string DisplayValue)
    {
      int volumeUnitIndex = MeterMath.GetVolumeUnitIndex(DisplayValue);
      return volumeUnitIndex < 1 ? -1M : (Decimal) MeterMath.LinearVolumeUnits[(int) MeterMath.VolumeUnits[volumeUnitIndex].LinearUnitIndex].UnitFactorFromLiter;
    }

    public static int GetVolumeUnitIndex(string DisplayValue)
    {
      for (int volumeUnitIndex = 0; volumeUnitIndex < MeterMath.VolumeUnits.Length; ++volumeUnitIndex)
      {
        if (MeterMath.VolumeUnits[volumeUnitIndex].VolumeUnitString.ToUpper() == DisplayValue.ToUpper())
          return volumeUnitIndex;
      }
      return -1;
    }

    public static string GetTrueVolumeUnitString(string TheString)
    {
      for (int index = 0; index < MeterMath.VolumeUnits.Length; ++index)
      {
        if (MeterMath.VolumeUnits[index].VolumeUnitString.ToUpper() == TheString.ToUpper())
          return MeterMath.VolumeUnits[index].VolumeUnitString;
      }
      return "";
    }

    public static string GetVolumeUnitOfID(int OverwriteID)
    {
      string volumeUnitOfId = "";
      for (int index = 0; index < MeterMath.VolumeUnits.Length; ++index)
      {
        if ((int) MeterMath.VolumeUnits[index].OverwriteID == OverwriteID)
          volumeUnitOfId = MeterMath.VolumeUnits[index].VolumeUnitString;
      }
      return volumeUnitOfId;
    }

    public int GetVolumeUnitIndexFromLiniarUnitIndex(int LinearUnitIndex)
    {
      for (int index = 0; index < MeterMath.VolumeUnits.Length; ++index)
      {
        if ((int) MeterMath.VolumeUnits[index].LinearUnitIndex == LinearUnitIndex)
          return index == MeterMath.VolumeUnits.Length - 1 || (int) MeterMath.VolumeUnits[index + 1].LinearUnitIndex != LinearUnitIndex ? index : index + 1;
      }
      return -1;
    }

    public static int GetInputUnitIndex(string DisplayValue)
    {
      for (int inputUnitIndex = 0; inputUnitIndex < MeterMath.InputUnits.Length; ++inputUnitIndex)
      {
        if (MeterMath.InputUnits[inputUnitIndex].InputUnitString.ToUpper() == DisplayValue.ToUpper())
          return inputUnitIndex;
      }
      return -1;
    }

    public static string GetInputUnitOfID(int OverwriteID)
    {
      string inputUnitOfId = "";
      for (int index = 0; index < MeterMath.InputUnits.Length; ++index)
      {
        if ((int) MeterMath.InputUnits[index].OverwriteID == OverwriteID)
          inputUnitOfId = MeterMath.InputUnits[index].InputUnitString;
      }
      return inputUnitOfId;
    }

    public static int GetEmptyUnitIndex(string DisplayValue)
    {
      for (int emptyUnitIndex = 0; emptyUnitIndex < MeterMath.EmptyUnits.Length; ++emptyUnitIndex)
      {
        if (MeterMath.EmptyUnits[emptyUnitIndex].EmptyUnitString.ToUpper() == DisplayValue.ToUpper())
          return emptyUnitIndex;
      }
      return -1;
    }

    public enum Errors
    {
      NoError,
      MathematicError,
      VolumeUnitNotAvailable,
      EnergyUnitNotAvailable,
      PowerUnitNotAvailable,
      PowerOutOfRange,
      FlowOutOfRange,
      PulsValueOutOfRange,
      Input1UnitNotAvailable,
      Input2UnitNotAvailable,
      Input1ToManyDecimalPlaces,
      Input2ToManyDecimalPlaces,
      Input1ToOutOfRange,
      Input2ToOutOfRange,
      InternalError,
    }

    public struct ENERGY_UNIT_DEFS(
      string EnergieUnitStringIn,
      string EnergiePulsValueStringIn,
      string PowerUnitStringIn,
      short AfterPointDigitsIn,
      short LinearUnitIndexIn,
      sbyte OverwriteIDIn)
    {
      public string EnergieUnitString = EnergieUnitStringIn;
      public string EnergiePulsValueString = EnergiePulsValueStringIn;
      public string PowerUnitString = PowerUnitStringIn;
      public short AfterPointDigits = AfterPointDigitsIn;
      public short LinearUnitIndex = LinearUnitIndexIn;
      public sbyte OverwriteID = OverwriteIDIn;
    }

    public struct LINEAR_ENERGY_UNIT_DEFS(
      double UnitFactorFromWhIn,
      short MBusEnergieVIF_In,
      short MBusPowerVIF_In)
    {
      public double UnitFactorFromWh = UnitFactorFromWhIn;
      public short MBusEnergieVIF = MBusEnergieVIF_In;
      public short MBusPowerVIF = MBusPowerVIF_In;
    }

    protected struct ENERGY_FRAMES(string[] EnergieFrameIn, string[] PowerFrameIn)
    {
      public string[] EnergyFrame = EnergieFrameIn;
      public string[] PowerFrame = PowerFrameIn;
    }

    protected struct BC_FRAMES(string[] BC_FrameIn)
    {
      public string[] BC_Frame = BC_FrameIn;
    }

    public struct VOLUME_UNIT_DEFS(
      string VolumeUnitStringIn,
      string VolumePulsValueIn,
      string FlowUnitStringIn,
      short AfterPointDigitsIn,
      short LinearUnitIndexIn,
      sbyte OverwriteIDIn)
    {
      public string VolumeUnitString = VolumeUnitStringIn;
      public string VolumePulsValue = VolumePulsValueIn;
      public string FlowUnitString = FlowUnitStringIn;
      public short AfterPointDigits = AfterPointDigitsIn;
      public short LinearUnitIndex = LinearUnitIndexIn;
      public sbyte OverwriteID = OverwriteIDIn;
    }

    public struct LINEAR_VOLUME_UNIT_DEFS(
      double UnitFactorFromLiterIn,
      short MBusVolumeVIF_In,
      short MBusFlowVIF_In,
      string VisibleUnit_In)
    {
      public double UnitFactorFromLiter = UnitFactorFromLiterIn;
      public short MBusVolumeVIF = MBusVolumeVIF_In;
      public short MBusFlowVIF = MBusFlowVIF_In;
      public string VisibleUnit = VisibleUnit_In;
    }

    public struct VOLUME_FRAMES(string[] VolumeFrameIn, string[] FlowFrameIn)
    {
      public string[] VolumeFrame = VolumeFrameIn;
      public string[] FlowFrame = FlowFrameIn;
    }

    public enum InputFrameType
    {
      Empty,
      Energy,
      Volume,
    }

    public struct INPUT_UNIT_DEFS(
      string InputUnitStringIn,
      MeterMath.InputFrameType FrameTypeIn,
      short AfterPointDigitsIn,
      short LinearUnitIndexIn,
      sbyte OverwriteIDIn)
    {
      public string InputUnitString = InputUnitStringIn;
      public MeterMath.InputFrameType FrameType = FrameTypeIn;
      public short AfterPointDigits = AfterPointDigitsIn;
      public short LinearUnitIndex = LinearUnitIndexIn;
      public sbyte OverwriteID = OverwriteIDIn;
    }

    protected struct INPUT_FRAMES(string[] InputFrameIn)
    {
      public string[] InputFrame = InputFrameIn;
    }

    public struct EMPTY_UNIT_DEFS(
      string EmptyUnitStringIn,
      short AfterPointDigitsIn,
      short LinearUnitIndexIn)
    {
      public string EmptyUnitString = EmptyUnitStringIn;
      public short AfterPointDigits = AfterPointDigitsIn;
      public short LinearUnitIndex = LinearUnitIndexIn;
    }

    public struct LINEAR_EMPTY_UNIT_DEFS(double UnitFactorFromEmptyIn, short MBusEmptyVIF_In)
    {
      public double UnitFactorFromEmpty = UnitFactorFromEmptyIn;
      public short MBusEmptyVIF = MBusEmptyVIF_In;
    }

    public struct EMPTY_FRAMES(string[] EmptyFrameIn)
    {
      public string[] EmptyFrame = EmptyFrameIn;
    }

    public class FrameDescription
    {
      public FrameTypes Type;
      public ZR_Constants.FrameNames FrameName;
      private string NewLine = Environment.NewLine;
      public string FrameType;
      public string[] FrameByteDescription;
      public byte[] FrameByteList;

      private FrameDescription()
      {
      }

      public FrameDescription(string TypeIn, string[] ByteDescription)
      {
        this.FrameByteDescription = (string[]) null;
        this.FrameByteList = (byte[]) null;
        this.Type = FrameTypes.None;
        this.FrameName = ZR_Constants.FrameNames.FrameCode;
        try
        {
          this.FrameName = (ZR_Constants.FrameNames) Enum.Parse(typeof (ZR_Constants.FrameNames), TypeIn, true);
          switch (this.FrameName)
          {
            case ZR_Constants.FrameNames.VolumeFrame:
              this.Type = FrameTypes.Volume;
              break;
            case ZR_Constants.FrameNames.FlowFrame:
              this.Type = FrameTypes.Flow;
              break;
            case ZR_Constants.FrameNames.EnergyFrame:
              this.Type = FrameTypes.Energy;
              break;
            case ZR_Constants.FrameNames.PowerFrame:
              this.Type = FrameTypes.Power;
              break;
            case ZR_Constants.FrameNames.FrameCode:
              this.Type = FrameTypes.Standard;
              break;
            case ZR_Constants.FrameNames.Input1Frame:
              this.Type = FrameTypes.Input1;
              break;
            case ZR_Constants.FrameNames.Input2Frame:
              this.Type = FrameTypes.Input2;
              break;
            case ZR_Constants.FrameNames.Input1ImpValFrame:
              this.Type = FrameTypes.Input1ImpVal;
              break;
            case ZR_Constants.FrameNames.Input2ImpValFrame:
              this.Type = FrameTypes.Input2ImpVal;
              break;
            case ZR_Constants.FrameNames.BCFrame:
              this.Type = FrameTypes.BC;
              break;
            case ZR_Constants.FrameNames.ImpulsValueFrame:
              this.Type = FrameTypes.ImpulsValue;
              break;
          }
        }
        catch
        {
        }
        bool flag = true;
        this.FrameType = TypeIn;
        this.FrameByteDescription = new string[ByteDescription.Length];
        for (int index = 0; index < ByteDescription.Length; ++index)
        {
          if (ByteDescription[index] != string.Empty)
            flag = false;
          this.FrameByteDescription[index] = ByteDescription[index];
        }
        if (flag)
          throw new ArgumentException("Fehler in FrameDescription:" + ZR_Constants.SystemNewLine + "Einheit nicht darstellbar!" + ZR_Constants.SystemNewLine + "FrameType : " + TypeIn);
      }

      public MeterMath.FrameDescription Clone()
      {
        MeterMath.FrameDescription frameDescription = new MeterMath.FrameDescription();
        frameDescription.Type = this.Type;
        frameDescription.FrameName = this.FrameName;
        frameDescription.FrameType = this.FrameType;
        frameDescription.FrameByteDescription = (string[]) this.FrameByteDescription.Clone();
        if (this.FrameByteList != null)
          frameDescription.FrameByteList = (byte[]) this.FrameByteList.Clone();
        return frameDescription;
      }
    }
  }
}
