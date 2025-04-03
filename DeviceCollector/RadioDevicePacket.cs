// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RadioDevicePacket
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using NLog;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  [Serializable]
  public abstract class RadioDevicePacket : RadioPacket
  {
    private static Logger logger = LogManager.GetLogger(nameof (RadioDevicePacket));
    public const long MinoConnectTestFunkId = 0;

    public bool IsManipulated { get; protected set; }

    public bool IsDeviceError { get; protected set; }

    public DateTime TimePoint { get; protected set; }

    public long GetValueIdentOfDueDateValue(bool scaleFactorAreAvailable, InputUnitsIndex? unit)
    {
      return ValueIdent.GetValueIdForValueEnum(this.GetValueIdPart_PhysicalQuantity(scaleFactorAreAvailable, unit), this.GetValueIdPart_MeterType(), ValueIdent.ValueIdPart_Calculation.Accumulated, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.DueDate, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any);
    }

    public long GetValueIdentOfCurrentValue(bool scaleFactorAreAvailable, InputUnitsIndex? unit)
    {
      return ValueIdent.GetValueIdForValueEnum(this.GetValueIdPart_PhysicalQuantity(scaleFactorAreAvailable, unit), this.GetValueIdPart_MeterType(), ValueIdent.ValueIdPart_Calculation.Accumulated, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any);
    }

    public long GetValueIdentOfHalfMonthValue(bool scaleFactorAreAvailable, InputUnitsIndex? unit)
    {
      return ValueIdent.GetValueIdForValueEnum(this.GetValueIdPart_PhysicalQuantity(scaleFactorAreAvailable, unit), this.GetValueIdPart_MeterType(), ValueIdent.ValueIdPart_Calculation.Accumulated, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.HalfMonth, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any);
    }

    public long GetValueIdentOfMonthValue(bool scaleFactorAreAvailable, InputUnitsIndex? unit)
    {
      return ValueIdent.GetValueIdForValueEnum(this.GetValueIdPart_PhysicalQuantity(scaleFactorAreAvailable, unit), this.GetValueIdPart_MeterType(), ValueIdent.ValueIdPart_Calculation.Accumulated, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.Month, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any);
    }

    public long GetValueIdentOfSignalStrengthValue()
    {
      return ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.SignalStrength, this.GetValueIdPart_MeterType(), ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any);
    }

    public ValueIdent.ValueIdPart_MeterType GetValueIdPart_MeterType()
    {
      ValueIdent.ValueIdPart_MeterType valueIdPartMeterType = ValueIdent.ValueIdPart_MeterType.Any;
      switch (this.DeviceType)
      {
        case DeviceTypes.ZR_Serie1:
        case DeviceTypes.ZR_Serie2:
        case DeviceTypes.ZR_Serie3:
          valueIdPartMeterType = ValueIdent.ValueIdPart_MeterType.ChangeOverHeat;
          break;
        case DeviceTypes.EHCA_M5:
        case DeviceTypes.EHCA_M5p:
        case DeviceTypes.EHCA_M6:
        case DeviceTypes.EHCA_M6_Radio3:
          valueIdPartMeterType = ValueIdent.ValueIdPart_MeterType.HeatCostAllocator;
          break;
        case DeviceTypes.MinotelContact:
        case DeviceTypes.MinotelContactRadio3:
        case DeviceTypes.PDC:
          valueIdPartMeterType = ValueIdent.ValueIdPart_MeterType.PulseCounter;
          break;
        case DeviceTypes.Aqua:
        case DeviceTypes.AquaMicro:
        case DeviceTypes.AquaMicroRadio3:
        case DeviceTypes.ISF:
        case DeviceTypes.EDC:
          valueIdPartMeterType = ValueIdent.ValueIdPart_MeterType.Water;
          break;
        case DeviceTypes.SmokeDetector:
          valueIdPartMeterType = ValueIdent.ValueIdPart_MeterType.SmokeDetector;
          break;
        case DeviceTypes.TemperatureSensor:
          valueIdPartMeterType = ValueIdent.ValueIdPart_MeterType.Thermometer;
          break;
        case DeviceTypes.HumiditySensor:
          valueIdPartMeterType = ValueIdent.ValueIdPart_MeterType.Hygrometer;
          break;
      }
      return valueIdPartMeterType;
    }

    public ValueIdent.ValueIdPart_PhysicalQuantity GetValueIdPart_PhysicalQuantity(
      bool scaleFactorAreAvailable,
      InputUnitsIndex? unit)
    {
      switch (this.DeviceType)
      {
        case DeviceTypes.EHCA_M5:
        case DeviceTypes.EHCA_M5p:
        case DeviceTypes.EHCA_M6:
        case DeviceTypes.EHCA_M6_Radio3:
          return scaleFactorAreAvailable ? ValueIdent.ValueIdPart_PhysicalQuantity.HCA_Weighted : ValueIdent.ValueIdPart_PhysicalQuantity.HCA;
        case DeviceTypes.MinotelContact:
        case DeviceTypes.MinotelContactRadio3:
        case DeviceTypes.Aqua:
        case DeviceTypes.AquaMicro:
        case DeviceTypes.AquaMicroRadio3:
        case DeviceTypes.ISF:
        case DeviceTypes.EDC:
        case DeviceTypes.PDC:
          if (!unit.HasValue)
            return ValueIdent.ValueIdPart_PhysicalQuantity.Pulse;
          switch (unit.Value)
          {
            case InputUnitsIndex.ImpUnit_0_000:
            case InputUnitsIndex.ImpUnit_0_00:
            case InputUnitsIndex.ImpUnit_0_0:
            case InputUnitsIndex.ImpUnit_0:
              return ValueIdent.ValueIdPart_PhysicalQuantity.Pulse;
            case InputUnitsIndex.ImpUnit_0_000Wh:
            case InputUnitsIndex.ImpUnit_0_00Wh:
            case InputUnitsIndex.ImpUnit_0_0Wh:
            case InputUnitsIndex.ImpUnit_0Wh:
            case InputUnitsIndex.ImpUnit_0_000kWh:
            case InputUnitsIndex.ImpUnit_0_00kWh:
            case InputUnitsIndex.ImpUnit_0_0kWh:
            case InputUnitsIndex.ImpUnit_0kWh:
            case InputUnitsIndex.ImpUnit_0_000MWh:
            case InputUnitsIndex.ImpUnit_0_00MWh:
            case InputUnitsIndex.ImpUnit_0_0MWh:
            case InputUnitsIndex.ImpUnit_0MWh:
            case InputUnitsIndex.ImpUnit_0_000GWh:
            case InputUnitsIndex.ImpUnit_0_00GWh:
            case InputUnitsIndex.ImpUnit_0_0GWh:
            case InputUnitsIndex.ImpUnit_0GWh:
              return ValueIdent.ValueIdPart_PhysicalQuantity.Power;
            case InputUnitsIndex.ImpUnit_0_000J:
            case InputUnitsIndex.ImpUnit_0_00J:
            case InputUnitsIndex.ImpUnit_0_0J:
            case InputUnitsIndex.ImpUnit_0J:
            case InputUnitsIndex.ImpUnit_0_000kJ:
            case InputUnitsIndex.ImpUnit_0_00kJ:
            case InputUnitsIndex.ImpUnit_0_0kJ:
            case InputUnitsIndex.ImpUnit_0kJ:
            case InputUnitsIndex.ImpUnit_0_000MJ:
            case InputUnitsIndex.ImpUnit_0_00MJ:
            case InputUnitsIndex.ImpUnit_0_0MJ:
            case InputUnitsIndex.ImpUnit_0MJ:
            case InputUnitsIndex.ImpUnit_0_000GJ:
            case InputUnitsIndex.ImpUnit_0_00GJ:
            case InputUnitsIndex.ImpUnit_0_0GJ:
            case InputUnitsIndex.ImpUnit_0GJ:
              return ValueIdent.ValueIdPart_PhysicalQuantity.Energy;
            case InputUnitsIndex.ImpUnit_0_000L:
            case InputUnitsIndex.ImpUnit_0_00L:
            case InputUnitsIndex.ImpUnit_0_0L:
            case InputUnitsIndex.ImpUnit_0L:
            case InputUnitsIndex.ImpUnit_0_000qm:
            case InputUnitsIndex.ImpUnit_0_00qm:
            case InputUnitsIndex.ImpUnit_0_0qm:
            case InputUnitsIndex.ImpUnit_0qm:
              return ValueIdent.ValueIdPart_PhysicalQuantity.Volume;
          }
          break;
        case DeviceTypes.SmokeDetector:
          return ValueIdent.ValueIdPart_PhysicalQuantity.StatusNumber;
        case DeviceTypes.TemperatureSensor:
          return ValueIdent.ValueIdPart_PhysicalQuantity.Temperature;
        case DeviceTypes.HumiditySensor:
          return ValueIdent.ValueIdPart_PhysicalQuantity.Percent;
      }
      return ValueIdent.ValueIdPart_PhysicalQuantity.Any;
    }

    public Decimal? GetDecimalValue(byte byte1, byte byte2)
    {
      if (byte1 == byte.MaxValue && byte2 == byte.MaxValue)
        return new Decimal?();
      if (byte1 == (byte) 0 && byte2 == (byte) 0)
        return new Decimal?(0M);
      if (this.DeviceType == DeviceTypes.HumiditySensor || this.DeviceType == DeviceTypes.TemperatureSensor)
        return new Decimal?((Decimal) (short) ((int) (short) ((int) byte1 << 8) | (int) byte2) / 10M);
      long num1 = (long) ((int) byte1 << 8) | (long) byte2;
      byte num2 = (byte) ((ulong) num1 & 3UL);
      Decimal num3 = (Decimal) (num1 >> 2);
      switch (num2)
      {
        case 0:
          return new Decimal?(num3);
        case 1:
          return new Decimal?(num3 + 0.25M);
        case 2:
          return new Decimal?(num3 + 0.50M);
        case 3:
          return new Decimal?(num3 + 0.75M);
        default:
          throw new NotImplementedException();
      }
    }

    protected Decimal? GetDecimalValueFromBCD(byte byte1, byte byte2, byte byte3)
    {
      if (byte1 == byte.MaxValue && byte2 == byte.MaxValue && byte3 == byte.MaxValue)
        return new Decimal?();
      return byte1 == (byte) 0 && byte2 == (byte) 0 && byte3 == (byte) 0 ? new Decimal?(0M) : new Decimal?((Decimal) Util.ConvertBcdInt64ToInt64((long) byte1 << 16 | (long) byte2 << 8 | (long) byte3));
    }

    protected Decimal? GetDecimalValueFromBCD(byte byte1, byte byte2, byte byte3, byte byte4)
    {
      if (byte1 == byte.MaxValue && byte2 == byte.MaxValue && byte3 == byte.MaxValue && byte4 == byte.MaxValue)
        return new Decimal?();
      return byte1 == (byte) 0 && byte2 == (byte) 0 && byte3 == (byte) 0 && byte4 == (byte) 0 ? new Decimal?(0M) : new Decimal?((Decimal) Util.ConvertBcdInt64ToInt64((long) byte1 << 24 | (long) byte2 << 16 | (long) byte3 << 8 | (long) byte4));
    }

    protected static void MergeMeterValues(
      SortedList<long, SortedList<DateTime, ReadingValue>> oldMeterValues,
      SortedList<long, SortedList<DateTime, ReadingValue>> newMeterValues,
      long valueIdent)
    {
      if (!newMeterValues.ContainsKey(valueIdent))
        return;
      SortedList<DateTime, ReadingValue> newMeterValue = newMeterValues[valueIdent];
      if (oldMeterValues.ContainsKey(valueIdent))
      {
        SortedList<DateTime, ReadingValue> oldMeterValue = oldMeterValues[valueIdent];
        for (int index = 0; index < newMeterValue.Count; ++index)
        {
          DateTime key = newMeterValue.Keys[index];
          ReadingValue readingValue = newMeterValue.Values[index];
          if (oldMeterValue.ContainsKey(key))
            oldMeterValue[key] = readingValue;
          else
            oldMeterValue.Add(key, readingValue);
        }
      }
      else
        oldMeterValues.Add(valueIdent, newMeterValue);
    }
  }
}
