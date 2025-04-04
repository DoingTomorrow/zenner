// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.MinoprotectII
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using DeviceCollector;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace SmokeDetectorHandler
{
  public sealed class MinoprotectII
  {
    public SmokeDetectorVersion Version { get; set; }

    public MinoprotectII_Parameter Parameter { get; set; }

    public MinoprotectII_Events Events { get; set; }

    public MinoprotectII_Values EventValues
    {
      get => MinoprotectII_Values.Parse(this.Parameter, this.Events);
    }

    public bool? IsDeviceError
    {
      get
      {
        return this.Version == null ? new bool?() : new bool?((this.Version.Status & SmokeDetectorStatusInformation.BatteryFault) == SmokeDetectorStatusInformation.BatteryFault || (this.Version.Status & SmokeDetectorStatusInformation.PermanentDeviceError) == SmokeDetectorStatusInformation.PermanentDeviceError);
      }
    }

    public bool? IsManipulation
    {
      get
      {
        return this.Version == null ? new bool?() : new bool?((this.Version.Status & SmokeDetectorStatusInformation.TemporaryDeviceError) == SmokeDetectorStatusInformation.TemporaryDeviceError);
      }
    }

    public SortedList<long, SortedList<DateTime, ReadingValue>> GetValues(List<long> filter)
    {
      if (this.EventValues == null || this.Parameter == null || !this.Parameter.CurrentDateTime.HasValue)
        return (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      long identDeviceError = SmokeDetectorHandlerFunctions.GetValueIdentDeviceError();
      long identManipulation = SmokeDetectorHandlerFunctions.GetValueIdentManipulation();
      bool? nullable1;
      int num1;
      if (this.IsDeviceError.HasValue)
      {
        nullable1 = this.IsDeviceError;
        num1 = nullable1.Value ? 1 : 0;
      }
      else
        num1 = 0;
      DateTime? currentDateTime;
      DateTime dateTime;
      if (num1 != 0)
      {
        ref SortedList<long, SortedList<DateTime, ReadingValue>> local1 = ref valueList;
        int year = this.Parameter.CurrentDateTime.Value.Year;
        currentDateTime = this.Parameter.CurrentDateTime;
        dateTime = currentDateTime.Value;
        int month = dateTime.Month;
        currentDateTime = this.Parameter.CurrentDateTime;
        dateTime = currentDateTime.Value;
        int day = dateTime.Day;
        DateTime timePoint = new DateTime(year, month, day);
        long valueIdent = identDeviceError;
        // ISSUE: variable of a boxed type
        __Boxed<int> local2 = (System.ValueType) 1;
        ValueIdent.AddValueToValueIdentList(ref local1, timePoint, valueIdent, (object) local2);
      }
      nullable1 = this.IsManipulation;
      int num2;
      if (nullable1.HasValue)
      {
        nullable1 = this.IsManipulation;
        num2 = nullable1.Value ? 1 : 0;
      }
      else
        num2 = 0;
      if (num2 != 0)
      {
        ref SortedList<long, SortedList<DateTime, ReadingValue>> local3 = ref valueList;
        currentDateTime = this.Parameter.CurrentDateTime;
        dateTime = currentDateTime.Value;
        int year = dateTime.Year;
        currentDateTime = this.Parameter.CurrentDateTime;
        dateTime = currentDateTime.Value;
        int month = dateTime.Month;
        currentDateTime = this.Parameter.CurrentDateTime;
        dateTime = currentDateTime.Value;
        int day = dateTime.Day;
        DateTime timePoint = new DateTime(year, month, day);
        long valueIdent = identManipulation;
        // ISSUE: variable of a boxed type
        __Boxed<int> local4 = (System.ValueType) 1;
        ValueIdent.AddValueToValueIdentList(ref local3, timePoint, valueIdent, (object) local4);
      }
      for (int index = 1; index < this.EventValues.MonthlyErrors.Length; ++index)
      {
        if (this.EventValues.MonthlyErrors[index].HasValue)
        {
          bool? monthlyError = this.EventValues.MonthlyErrors[index];
          DateTime timePoint;
          ref DateTime local = ref timePoint;
          currentDateTime = this.Parameter.CurrentDateTime;
          dateTime = currentDateTime.Value;
          int year = dateTime.Year;
          currentDateTime = this.Parameter.CurrentDateTime;
          dateTime = currentDateTime.Value;
          int month = dateTime.Month;
          local = new DateTime(year, month, 1);
          int num3 = index - 1;
          timePoint = timePoint.AddMonths(-num3);
          ValueIdent.AddValueToValueIdentList(ref valueList, timePoint, identDeviceError, (object) 1);
        }
      }
      for (int index = 1; index < this.EventValues.DailyErrors.Length; ++index)
      {
        if (this.EventValues.DailyErrors[index].HasValue)
        {
          bool? dailyError = this.EventValues.DailyErrors[index];
          DateTime timePoint;
          ref DateTime local = ref timePoint;
          currentDateTime = this.Parameter.CurrentDateTime;
          dateTime = currentDateTime.Value;
          int year = dateTime.Year;
          currentDateTime = this.Parameter.CurrentDateTime;
          dateTime = currentDateTime.Value;
          int month = dateTime.Month;
          local = new DateTime(year, month, 1);
          int num4 = index - 1;
          timePoint = timePoint.AddMonths(-num4);
          ValueIdent.AddValueToValueIdentList(ref valueList, timePoint, identDeviceError, (object) 1);
        }
      }
      long identMonthlyState = SmokeDetectorHandlerFunctions.GetValueIdentMonthlyState();
      SmokeDetectorEvent? nullable2;
      if (ValueIdent.IsExpectedValueIdent(filter, identMonthlyState))
      {
        for (int index = 1; index < this.EventValues.MonthlyEvents.Length; ++index)
        {
          if (this.EventValues.MonthlyEvents[index].HasValue)
          {
            SmokeDetectorEvent? monthlyEvent = this.EventValues.MonthlyEvents[index];
            nullable2 = monthlyEvent;
            SmokeDetectorEvent smokeDetectorEvent = ~(SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionForewarning | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined);
            if (!(nullable2.GetValueOrDefault() == smokeDetectorEvent & nullable2.HasValue))
            {
              DateTime timePoint;
              ref DateTime local = ref timePoint;
              currentDateTime = this.Parameter.CurrentDateTime;
              dateTime = currentDateTime.Value;
              int year = dateTime.Year;
              currentDateTime = this.Parameter.CurrentDateTime;
              dateTime = currentDateTime.Value;
              int month = dateTime.Month;
              local = new DateTime(year, month, 1);
              int num5 = index - 1;
              timePoint = timePoint.AddMonths(-num5);
              ValueIdent.AddValueToValueIdentList(ref valueList, timePoint, identMonthlyState, (object) (ushort) monthlyEvent.Value);
            }
          }
        }
      }
      long valueIdentDailyState = SmokeDetectorHandlerFunctions.GetValueIdentDailyState();
      if (ValueIdent.IsExpectedValueIdent(filter, valueIdentDailyState))
      {
        for (int index = 1; index < this.EventValues.DailyEvents.Length; ++index)
        {
          if (this.EventValues.DailyEvents[index].HasValue)
          {
            SmokeDetectorEvent? dailyEvent = this.EventValues.DailyEvents[index];
            nullable2 = dailyEvent;
            SmokeDetectorEvent smokeDetectorEvent = ~(SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionForewarning | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined);
            if (!(nullable2.GetValueOrDefault() == smokeDetectorEvent & nullable2.HasValue))
            {
              DateTime timePoint;
              ref DateTime local = ref timePoint;
              currentDateTime = this.Parameter.CurrentDateTime;
              dateTime = currentDateTime.Value;
              int year = dateTime.Year;
              currentDateTime = this.Parameter.CurrentDateTime;
              dateTime = currentDateTime.Value;
              int month = dateTime.Month;
              currentDateTime = this.Parameter.CurrentDateTime;
              dateTime = currentDateTime.Value;
              int day = dateTime.Day;
              local = new DateTime(year, month, day);
              int num6 = index - 1;
              timePoint = timePoint.AddDays((double) -num6);
              ValueIdent.AddValueToValueIdentList(ref valueList, timePoint, valueIdentDailyState, (object) (ushort) dailyEvent.Value);
            }
          }
        }
      }
      return valueList;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.Version != null)
        stringBuilder.AppendLine(this.Version.ToString(20));
      if (this.Parameter != null)
        stringBuilder.AppendLine(this.Parameter.ToString(46));
      if (this.Events != null)
        stringBuilder.AppendLine(this.Events.ToString(25));
      if (this.EventValues != null)
        stringBuilder.AppendLine(this.EventValues.ToString(25));
      SortedList<long, SortedList<DateTime, ReadingValue>> values = this.GetValues((List<long>) null);
      if (values != null)
      {
        string str = ValueIdent.ToString(values, new Dictionary<long, Type>()
        {
          {
            SmokeDetectorHandlerFunctions.GetValueIdentManipulation(),
            typeof (bool)
          },
          {
            SmokeDetectorHandlerFunctions.GetValueIdentDeviceError(),
            typeof (bool)
          },
          {
            SmokeDetectorHandlerFunctions.GetValueIdentMonthlyState(),
            typeof (SmokeDetectorEvent)
          },
          {
            SmokeDetectorHandlerFunctions.GetValueIdentDailyState(),
            typeof (SmokeDetectorEvent)
          }
        });
        if (!string.IsNullOrEmpty(str))
          stringBuilder.AppendLine(str);
      }
      return stringBuilder.ToString();
    }

    internal MinoprotectII DeepCopy()
    {
      MinoprotectII minoprotectIi = new MinoprotectII();
      if (this.Events != null)
        minoprotectIi.Events = this.Events.DeepCopy();
      if (this.Parameter != null)
        minoprotectIi.Parameter = this.Parameter.DeepCopy();
      if (this.Version != null)
        minoprotectIi.Version = this.Version.DeepCopy();
      return minoprotectIi;
    }
  }
}
