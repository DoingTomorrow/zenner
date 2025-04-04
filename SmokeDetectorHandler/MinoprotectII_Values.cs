// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.MinoprotectII_Values
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using DeviceCollector;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace SmokeDetectorHandler
{
  public sealed class MinoprotectII_Values
  {
    public DateTime TimePoint { get; private set; }

    public SmokeDetectorEvent?[] MonthlyEvents { get; private set; }

    public bool?[] MonthlyErrors { get; private set; }

    public SmokeDetectorEvent?[] DailyEvents { get; private set; }

    public bool?[] DailyErrors { get; private set; }

    public MinoprotectII_Values()
    {
      this.MonthlyEvents = new SmokeDetectorEvent?[19];
      this.DailyEvents = new SmokeDetectorEvent?[33];
      this.MonthlyErrors = new bool?[19];
      this.DailyErrors = new bool?[33];
    }

    internal static MinoprotectII_Values Parse(
      MinoprotectII_Parameter parameter,
      MinoprotectII_Events events)
    {
      if (parameter == null)
        throw new ArgumentNullException(nameof (parameter), "Can not parse the event values!");
      if (events == null)
        throw new ArgumentNullException(nameof (events), "Can not parse the event values!");
      if (!parameter.CurrentDateTime.HasValue)
        return (MinoprotectII_Values) null;
      MinoprotectII_Values minoprotectIiValues1 = new MinoprotectII_Values();
      MinoprotectII_Values minoprotectIiValues2 = minoprotectIiValues1;
      DateTime? nullable = parameter.CurrentDateTime;
      DateTime dateTime = nullable.Value;
      minoprotectIiValues2.TimePoint = dateTime;
      DateTime? ofFirstActivation = parameter.DateOfFirstActivation;
      DateTime? currentDateTime = parameter.CurrentDateTime;
      DateTime?[] nullableArray1 = new DateTime?[5]
      {
        events.DateActivation1,
        events.DateActivation2,
        events.DateActivation3,
        events.DateActivation4,
        events.DateActivation5
      };
      foreach (DateTime? date in nullableArray1)
      {
        if (!MinoprotectII_Values.IsNull(ofFirstActivation, date))
        {
          int index1 = MinoprotectII_Values.MonthDifference(currentDateTime.Value, date.Value);
          if (index1 >= 1 && index1 <= 19)
            minoprotectIiValues1.MonthlyEvents[index1] = new SmokeDetectorEvent?(SmokeDetectorEvent.RemovingDetection);
          int index2 = MinoprotectII_Values.DayDifference(currentDateTime.Value, date.Value);
          if (index2 >= 1 && index2 <= 33)
            minoprotectIiValues1.DailyEvents[index2] = new SmokeDetectorEvent?(SmokeDetectorEvent.RemovingDetection);
        }
      }
      DateTime?[] nullableArray2 = new DateTime?[10]
      {
        events.DateAlarmEvent1,
        events.DateAlarmEvent2,
        events.DateAlarmEvent3,
        events.DateAlarmEvent4,
        events.DateAlarmEvent5,
        events.DateAlarmEvent6,
        events.DateAlarmEvent7,
        events.DateAlarmEvent8,
        events.DateAlarmEvent9,
        events.DateAlarmEvent10
      };
      foreach (DateTime? date in nullableArray2)
      {
        if (!MinoprotectII_Values.IsNull(ofFirstActivation, date))
        {
          int index3 = MinoprotectII_Values.MonthDifference(currentDateTime.Value, date.Value);
          if (index3 >= 1 && index3 <= 19)
            minoprotectIiValues1.MonthlyEvents[index3] = new SmokeDetectorEvent?(SmokeDetectorEvent.SmokeAlarmReleased);
          int index4 = MinoprotectII_Values.DayDifference(currentDateTime.Value, date.Value);
          if (index4 >= 1 && index4 <= 33)
            minoprotectIiValues1.DailyEvents[index4] = new SmokeDetectorEvent?(SmokeDetectorEvent.SmokeAlarmReleased);
        }
      }
      foreach (KeyValuePair<DateTime?, FaultIdentification> keyValuePair in new List<KeyValuePair<DateTime?, FaultIdentification>>(11)
      {
        new KeyValuePair<DateTime?, FaultIdentification>(events.DateFirstFault, events.IdentificationFirstFault),
        new KeyValuePair<DateTime?, FaultIdentification>(events.DateFault1, events.IdentificationFault1),
        new KeyValuePair<DateTime?, FaultIdentification>(events.DateFault2, events.IdentificationFault2),
        new KeyValuePair<DateTime?, FaultIdentification>(events.DateFault3, events.IdentificationFault3),
        new KeyValuePair<DateTime?, FaultIdentification>(events.DateFault4, events.IdentificationFault4),
        new KeyValuePair<DateTime?, FaultIdentification>(events.DateFault5, events.IdentificationFault5),
        new KeyValuePair<DateTime?, FaultIdentification>(events.DateFault6, events.IdentificationFault6),
        new KeyValuePair<DateTime?, FaultIdentification>(events.DateFault7, events.IdentificationFault7),
        new KeyValuePair<DateTime?, FaultIdentification>(events.DateFault8, events.IdentificationFault8),
        new KeyValuePair<DateTime?, FaultIdentification>(events.DateFault9, events.IdentificationFault9),
        new KeyValuePair<DateTime?, FaultIdentification>(events.DateFault10, events.IdentificationFault10)
      })
      {
        if (keyValuePair.Value != ~(FaultIdentification.ADC | FaultIdentification.TempDiode | FaultIdentification.LowBattery | FaultIdentification.HiResBattery | FaultIdentification.DegradedChamber | FaultIdentification.CalibrationCorrupted | FaultIdentification.RadioModule | FaultIdentification.RadioModuleBattery | FaultIdentification.Bit8 | FaultIdentification.SPI_BufferOverflow | FaultIdentification.EEPROM | FaultIdentification.EEPROM_BufferOverflow | FaultIdentification.Thermoptek | FaultIdentification.Bit13 | FaultIdentification.Bit14 | FaultIdentification.Bit15) && !MinoprotectII_Values.IsNull(ofFirstActivation, keyValuePair.Key))
        {
          DateTime a1 = currentDateTime.Value;
          nullable = keyValuePair.Key;
          DateTime b1 = nullable.Value;
          int index5 = MinoprotectII_Values.MonthDifference(a1, b1);
          if (index5 >= 1 && index5 <= 19)
          {
            minoprotectIiValues1.MonthlyEvents[index5] = new SmokeDetectorEvent?(MinoprotectII_Values.TranslateFaultEvent(keyValuePair.Value));
            minoprotectIiValues1.MonthlyErrors[index5] = MinoprotectII_Values.HasErrorEvents(keyValuePair.Value);
          }
          DateTime a2 = currentDateTime.Value;
          nullable = keyValuePair.Key;
          DateTime b2 = nullable.Value;
          int index6 = MinoprotectII_Values.DayDifference(a2, b2);
          if (index6 >= 1 && index6 <= 33)
          {
            minoprotectIiValues1.DailyEvents[index6] = new SmokeDetectorEvent?(MinoprotectII_Values.TranslateFaultEvent(keyValuePair.Value));
            minoprotectIiValues1.DailyErrors[index6] = MinoprotectII_Values.HasErrorEvents(keyValuePair.Value);
          }
        }
      }
      return minoprotectIiValues1;
    }

    private static SmokeDetectorEvent TranslateFaultEvent(FaultIdentification faultIdentification)
    {
      SmokeDetectorEvent smokeDetectorEvent = ~(SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionForewarning | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined);
      if ((faultIdentification & FaultIdentification.LowBattery) == FaultIdentification.LowBattery)
        smokeDetectorEvent |= SmokeDetectorEvent.BatteryFault;
      if ((faultIdentification & FaultIdentification.RadioModuleBattery) == FaultIdentification.RadioModuleBattery)
        smokeDetectorEvent |= SmokeDetectorEvent.BatteryWarningRadio;
      if ((faultIdentification & FaultIdentification.DegradedChamber) == FaultIdentification.DegradedChamber)
        smokeDetectorEvent |= SmokeDetectorEvent.SmokeChamberPollutionWarning;
      return smokeDetectorEvent;
    }

    private static bool? HasErrorEvents(FaultIdentification faultIdentification)
    {
      return new bool?(faultIdentification != 0);
    }

    private static bool IsNull(DateTime? nullDate, DateTime? date)
    {
      if (!date.HasValue)
        return false;
      DateTime? nullable1 = date;
      DateTime? nullable2 = nullDate;
      return nullable1.HasValue & nullable2.HasValue && nullable1.GetValueOrDefault() <= nullable2.GetValueOrDefault();
    }

    public static int DayDifference(DateTime a, DateTime b) => (a - b).Days;

    public static int MonthDifference(DateTime a, DateTime b)
    {
      return a.Month - b.Month + 12 * (a.Year - b.Year);
    }

    public string ToString(int spaces)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      SmokeDetectorEvent smokeDetectorEvent;
      for (int index = 1; index < this.MonthlyEvents.Length; ++index)
      {
        if (this.MonthlyEvents[index].HasValue)
        {
          StringBuilder stringBuilder2 = stringBuilder1.Append("M").Append(index.ToString()).Append(" -> ");
          smokeDetectorEvent = this.MonthlyEvents[index].Value;
          string str = smokeDetectorEvent.ToString();
          stringBuilder2.AppendLine(str);
        }
      }
      for (int index = 1; index < this.DailyEvents.Length; ++index)
      {
        if (this.DailyEvents[index].HasValue)
        {
          StringBuilder stringBuilder3 = stringBuilder1.Append("D").Append(index.ToString()).Append(" -> ");
          smokeDetectorEvent = this.DailyEvents[index].Value;
          string str = smokeDetectorEvent.ToString();
          stringBuilder3.AppendLine(str);
        }
      }
      for (int index = 1; index < this.MonthlyErrors.Length; ++index)
      {
        if (this.MonthlyErrors[index].HasValue)
          stringBuilder1.Append("M").Append(index.ToString()).AppendLine(" -> device error");
      }
      for (int index = 1; index < this.DailyErrors.Length; ++index)
      {
        if (this.DailyErrors[index].HasValue)
          stringBuilder1.Append("D").Append(index.ToString()).AppendLine(" -> device error");
      }
      return stringBuilder1.ToString();
    }
  }
}
