// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.MinoprotectIII
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using DeviceCollector;
using HandlerLib;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace SmokeDetectorHandler
{
  public sealed class MinoprotectIII
  {
    public SmokeDetectorVersion Version { get; set; }

    public MinoprotectIII_Parameter Parameter { get; set; }

    public TestModeParameter TestModeParameter { get; set; }

    public ManufacturingParameter ManufacturingParameter { get; set; }

    public LoRaParameter LoRaParameter { get; set; }

    public List<MinoprotectIII_Events> EventMemory { get; set; }

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
        return this.Parameter == null ? new bool?() : new bool?((this.Parameter.CurrentStateOfEvents & SmokeDetectorEvent.RemovingDetection) == SmokeDetectorEvent.RemovingDetection);
      }
    }

    public SortedList<long, SortedList<DateTime, ReadingValue>> GetValues(List<long> filter)
    {
      if (this.Parameter == null)
        throw new ArgumentNullException("Parameter", "Can not get the meter values!");
      if (!this.Parameter.DateOfFirstActivation.HasValue || !this.Parameter.CurrentDateTime.HasValue)
        return (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      long identDeviceError = SmokeDetectorHandlerFunctions.GetValueIdentDeviceError();
      long identManipulation = SmokeDetectorHandlerFunctions.GetValueIdentManipulation();
      DateTime? nullable1;
      if (this.IsDeviceError.HasValue && this.IsDeviceError.Value)
      {
        ref SortedList<long, SortedList<DateTime, ReadingValue>> local1 = ref valueList;
        nullable1 = this.Parameter.CurrentDateTime;
        int year = nullable1.Value.Year;
        nullable1 = this.Parameter.CurrentDateTime;
        int month = nullable1.Value.Month;
        nullable1 = this.Parameter.CurrentDateTime;
        int day = nullable1.Value.Day;
        DateTime timePoint = new DateTime(year, month, day);
        long valueIdent = identDeviceError;
        // ISSUE: variable of a boxed type
        __Boxed<int> local2 = (System.ValueType) 1;
        ValueIdent.AddValueToValueIdentList(ref local1, timePoint, valueIdent, (object) local2);
      }
      if (this.IsManipulation.HasValue && this.IsManipulation.Value)
      {
        ref SortedList<long, SortedList<DateTime, ReadingValue>> local3 = ref valueList;
        nullable1 = this.Parameter.CurrentDateTime;
        int year = nullable1.Value.Year;
        nullable1 = this.Parameter.CurrentDateTime;
        int month = nullable1.Value.Month;
        nullable1 = this.Parameter.CurrentDateTime;
        int day = nullable1.Value.Day;
        DateTime timePoint = new DateTime(year, month, day);
        long valueIdent = identManipulation;
        // ISSUE: variable of a boxed type
        __Boxed<int> local4 = (System.ValueType) 1;
        ValueIdent.AddValueToValueIdentList(ref local3, timePoint, valueIdent, (object) local4);
      }
      if (this.EventMemory != null)
      {
        int num = 0;
        foreach (MinoprotectIII_Events minoprotectIiiEvents1 in this.EventMemory)
        {
          nullable1 = minoprotectIiiEvents1.EventDate;
          if (nullable1.HasValue)
          {
            MinoprotectIII_Events minoprotectIiiEvents2 = minoprotectIiiEvents1;
            nullable1 = minoprotectIiiEvents1.EventDate;
            DateTime? nullable2 = new DateTime?(nullable1.Value.AddSeconds((double) num++));
            minoprotectIiiEvents2.EventDate = nullable2;
            SmokeDetectorEvent eventIdentification = minoprotectIiiEvents1.EventIdentification;
            if ((uint) eventIdentification <= 64U)
            {
              if ((uint) eventIdentification <= 8U)
              {
                switch (eventIdentification - (ushort) 1)
                {
                  case ~(SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionForewarning | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined):
                    long valueIdForValueEnum1 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 62277025792);
                    if (ValueIdent.IsExpectedValueIdent(filter, valueIdForValueEnum1))
                    {
                      ref SortedList<long, SortedList<DateTime, ReadingValue>> local = ref valueList;
                      nullable1 = minoprotectIiiEvents1.EventDate;
                      DateTime timePoint = nullable1.Value;
                      long valueIdent = valueIdForValueEnum1;
                      // ISSUE: variable of a boxed type
                      __Boxed<ushort> eventData = (System.ValueType) minoprotectIiiEvents1.EventData;
                      ValueIdent.AddValueToValueIdentList(ref local, timePoint, valueIdent, (object) eventData);
                      goto label_77;
                    }
                    else
                      goto label_77;
                  case SmokeDetectorEvent.BatteryForewarning:
                  case SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault:
                    long valueIdForValueEnum2 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 64424509440);
                    if (ValueIdent.IsExpectedValueIdent(filter, valueIdForValueEnum2))
                    {
                      ref SortedList<long, SortedList<DateTime, ReadingValue>> local = ref valueList;
                      nullable1 = minoprotectIiiEvents1.EventDate;
                      DateTime timePoint = nullable1.Value;
                      long valueIdent = valueIdForValueEnum2;
                      // ISSUE: variable of a boxed type
                      __Boxed<ushort> eventData = (System.ValueType) minoprotectIiiEvents1.EventData;
                      ValueIdent.AddValueToValueIdentList(ref local, timePoint, valueIdent, (object) eventData);
                      goto label_77;
                    }
                    else
                      goto label_77;
                  case SmokeDetectorEvent.BatteryFault:
                    goto label_76;
                  default:
                    if (eventIdentification == SmokeDetectorEvent.SmokeChamberPollutionForewarning)
                      break;
                    goto label_76;
                }
              }
              else if (eventIdentification != SmokeDetectorEvent.SmokeChamberPollutionWarning)
              {
                if (eventIdentification != SmokeDetectorEvent.PushButtonFailure)
                {
                  if (eventIdentification == SmokeDetectorEvent.HornFailure)
                  {
                    long valueIdForValueEnum3;
                    if (minoprotectIiiEvents1.EventData == (ushort) 1)
                    {
                      valueIdForValueEnum3 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.ErrorNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 27917287424);
                    }
                    else
                    {
                      if (minoprotectIiiEvents1.EventData != (ushort) 256)
                        throw new NotImplementedException(minoprotectIiiEvents1.EventIdentification.ToString() + " # 0x" + minoprotectIiiEvents1.EventData.ToString("X4"));
                      valueIdForValueEnum3 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.InfoNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 2147483648);
                    }
                    if (ValueIdent.IsExpectedValueIdent(filter, valueIdForValueEnum3))
                    {
                      ref SortedList<long, SortedList<DateTime, ReadingValue>> local5 = ref valueList;
                      nullable1 = minoprotectIiiEvents1.EventDate;
                      DateTime timePoint = nullable1.Value;
                      long valueIdent = valueIdForValueEnum3;
                      // ISSUE: variable of a boxed type
                      __Boxed<int> local6 = (System.ValueType) 0;
                      ValueIdent.AddValueToValueIdentList(ref local5, timePoint, valueIdent, (object) local6);
                      goto label_77;
                    }
                    else
                      goto label_77;
                  }
                  else
                    goto label_76;
                }
                else
                {
                  long valueIdForValueEnum4;
                  if (minoprotectIiiEvents1.EventData == (ushort) 1)
                    valueIdForValueEnum4 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.ErrorNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 30064771072);
                  else if (minoprotectIiiEvents1.EventData == (ushort) 256)
                  {
                    valueIdForValueEnum4 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.InfoNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 4294967296);
                  }
                  else
                  {
                    if (minoprotectIiiEvents1.EventData != (ushort) 0)
                      throw new NotImplementedException(minoprotectIiiEvents1.EventIdentification.ToString() + " # 0x" + minoprotectIiiEvents1.EventData.ToString("X4"));
                    valueIdForValueEnum4 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.InfoNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 4294967296);
                  }
                  if (ValueIdent.IsExpectedValueIdent(filter, valueIdForValueEnum4))
                  {
                    ref SortedList<long, SortedList<DateTime, ReadingValue>> local7 = ref valueList;
                    nullable1 = minoprotectIiiEvents1.EventDate;
                    DateTime timePoint = nullable1.Value;
                    long valueIdent = valueIdForValueEnum4;
                    // ISSUE: variable of a boxed type
                    __Boxed<int> local8 = (System.ValueType) 0;
                    ValueIdent.AddValueToValueIdentList(ref local7, timePoint, valueIdent, (object) local8);
                    goto label_77;
                  }
                  else
                    goto label_77;
                }
              }
              long valueIdForValueEnum5;
              if (minoprotectIiiEvents1.EventData == (ushort) 1)
              {
                valueIdForValueEnum5 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 66571993088);
              }
              else
              {
                if (minoprotectIiiEvents1.EventData != (ushort) 256)
                  throw new NotImplementedException(minoprotectIiiEvents1.EventIdentification.ToString() + " # 0x" + minoprotectIiiEvents1.EventData.ToString("X4"));
                valueIdForValueEnum5 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.InfoNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 8589934592);
              }
              if (ValueIdent.IsExpectedValueIdent(filter, valueIdForValueEnum5))
              {
                ref SortedList<long, SortedList<DateTime, ReadingValue>> local9 = ref valueList;
                nullable1 = minoprotectIiiEvents1.EventDate;
                DateTime timePoint = nullable1.Value;
                long valueIdent = valueIdForValueEnum5;
                // ISSUE: variable of a boxed type
                __Boxed<int> local10 = (System.ValueType) 0;
                ValueIdent.AddValueToValueIdentList(ref local9, timePoint, valueIdent, (object) local10);
                goto label_77;
              }
              else
                goto label_77;
            }
            else if ((uint) eventIdentification <= 512U)
            {
              switch (eventIdentification)
              {
                case SmokeDetectorEvent.RemovingDetection:
                  long valueIdForValueEnum6;
                  if (minoprotectIiiEvents1.EventData == (ushort) 1)
                  {
                    valueIdForValueEnum6 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 21474836480);
                  }
                  else
                  {
                    if (minoprotectIiiEvents1.EventData != (ushort) 256)
                      throw new NotImplementedException(minoprotectIiiEvents1.EventIdentification.ToString() + " # 0x" + minoprotectIiiEvents1.EventData.ToString("X4"));
                    valueIdForValueEnum6 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.InfoNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 6442450944);
                  }
                  if (ValueIdent.IsExpectedValueIdent(filter, valueIdForValueEnum6))
                  {
                    ref SortedList<long, SortedList<DateTime, ReadingValue>> local11 = ref valueList;
                    nullable1 = minoprotectIiiEvents1.EventDate;
                    DateTime timePoint = nullable1.Value;
                    long valueIdent = valueIdForValueEnum6;
                    // ISSUE: variable of a boxed type
                    __Boxed<int> local12 = (System.ValueType) 0;
                    ValueIdent.AddValueToValueIdentList(ref local11, timePoint, valueIdent, (object) local12);
                    goto label_77;
                  }
                  else
                    goto label_77;
                case SmokeDetectorEvent.TestAlarmReleased:
                  long valueIdForValueEnum7 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.InfoNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 15032385536);
                  if (ValueIdent.IsExpectedValueIdent(filter, valueIdForValueEnum7))
                  {
                    ref SortedList<long, SortedList<DateTime, ReadingValue>> local = ref valueList;
                    nullable1 = minoprotectIiiEvents1.EventDate;
                    DateTime timePoint = nullable1.Value;
                    long valueIdent = valueIdForValueEnum7;
                    // ISSUE: variable of a boxed type
                    __Boxed<ushort> eventData = (System.ValueType) minoprotectIiiEvents1.EventData;
                    ValueIdent.AddValueToValueIdentList(ref local, timePoint, valueIdent, (object) eventData);
                    goto label_77;
                  }
                  else
                    goto label_77;
                case SmokeDetectorEvent.SmokeAlarmReleased:
                  long valueIdForValueEnum8 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 68719476736);
                  if (ValueIdent.IsExpectedValueIdent(filter, valueIdForValueEnum8))
                  {
                    ref SortedList<long, SortedList<DateTime, ReadingValue>> local = ref valueList;
                    nullable1 = minoprotectIiiEvents1.EventDate;
                    DateTime timePoint = nullable1.Value;
                    long valueIdent = valueIdForValueEnum8;
                    // ISSUE: variable of a boxed type
                    __Boxed<ushort> eventData = (System.ValueType) minoprotectIiiEvents1.EventData;
                    ValueIdent.AddValueToValueIdentList(ref local, timePoint, valueIdent, (object) eventData);
                    goto label_77;
                  }
                  else
                    goto label_77;
              }
            }
            else
            {
              switch (eventIdentification)
              {
                case SmokeDetectorEvent.IngressAperturesObstructionDetected:
                  long valueIdForValueEnum9;
                  if (minoprotectIiiEvents1.EventData == (ushort) 1)
                  {
                    valueIdForValueEnum9 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 70866960384);
                  }
                  else
                  {
                    if (minoprotectIiiEvents1.EventData != (ushort) 256)
                      throw new NotImplementedException(minoprotectIiiEvents1.EventIdentification.ToString() + " # 0x" + minoprotectIiiEvents1.EventData.ToString("X4"));
                    valueIdForValueEnum9 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.InfoNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 10737418240);
                  }
                  if (ValueIdent.IsExpectedValueIdent(filter, valueIdForValueEnum9))
                  {
                    ref SortedList<long, SortedList<DateTime, ReadingValue>> local13 = ref valueList;
                    nullable1 = minoprotectIiiEvents1.EventDate;
                    DateTime timePoint = nullable1.Value;
                    long valueIdent = valueIdForValueEnum9;
                    // ISSUE: variable of a boxed type
                    __Boxed<int> local14 = (System.ValueType) 0;
                    ValueIdent.AddValueToValueIdentList(ref local13, timePoint, valueIdent, (object) local14);
                    goto label_77;
                  }
                  else
                    goto label_77;
                case SmokeDetectorEvent.ObjectInSurroundingAreaDetected:
                  long valueIdForValueEnum10;
                  if (minoprotectIiiEvents1.EventData == (ushort) 1)
                  {
                    valueIdForValueEnum10 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 73014444032);
                  }
                  else
                  {
                    if (minoprotectIiiEvents1.EventData != (ushort) 256)
                      throw new NotImplementedException(minoprotectIiiEvents1.EventIdentification.ToString() + " # 0x" + minoprotectIiiEvents1.EventData.ToString("X4"));
                    valueIdForValueEnum10 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.InfoNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 12884901888);
                  }
                  if (ValueIdent.IsExpectedValueIdent(filter, valueIdForValueEnum10))
                  {
                    ref SortedList<long, SortedList<DateTime, ReadingValue>> local15 = ref valueList;
                    nullable1 = minoprotectIiiEvents1.EventDate;
                    DateTime timePoint = nullable1.Value;
                    long valueIdent = valueIdForValueEnum10;
                    // ISSUE: variable of a boxed type
                    __Boxed<int> local16 = (System.ValueType) 0;
                    ValueIdent.AddValueToValueIdentList(ref local15, timePoint, valueIdent, (object) local16);
                    goto label_77;
                  }
                  else
                    goto label_77;
                case SmokeDetectorEvent.LED_Failure:
                  long valueIdForValueEnum11 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.ErrorNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.MeterLogger, (object) (ValueIdent.ValueIdPart_Index) 32212254720);
                  if (ValueIdent.IsExpectedValueIdent(filter, valueIdForValueEnum11))
                  {
                    ref SortedList<long, SortedList<DateTime, ReadingValue>> local = ref valueList;
                    nullable1 = minoprotectIiiEvents1.EventDate;
                    DateTime timePoint = nullable1.Value;
                    long valueIdent = valueIdForValueEnum11;
                    // ISSUE: variable of a boxed type
                    __Boxed<ushort> eventData = (System.ValueType) minoprotectIiiEvents1.EventData;
                    ValueIdent.AddValueToValueIdentList(ref local, timePoint, valueIdent, (object) eventData);
                    goto label_77;
                  }
                  else
                    goto label_77;
              }
            }
label_76:
            throw new NotImplementedException(minoprotectIiiEvents1.EventIdentification.ToString());
label_77:;
          }
        }
      }
      List<MinoprotectIII_Events> days = this.SummarizeEventsToDays();
      if (days != null && days.Count > 0)
      {
        long identCurrentState = SmokeDetectorHandlerFunctions.GetValueIdentCurrentState();
        if (ValueIdent.IsExpectedValueIdent(filter, identCurrentState))
        {
          ref SortedList<long, SortedList<DateTime, ReadingValue>> local = ref valueList;
          nullable1 = this.Parameter.CurrentDateTime;
          DateTime timePoint = nullable1.Value;
          long valueIdent = identCurrentState;
          // ISSUE: variable of a boxed type
          __Boxed<ushort> currentStateOfEvents = (System.ValueType) (ushort) this.Parameter.CurrentStateOfEvents;
          ValueIdent.AddValueToValueIdentList(ref local, timePoint, valueIdent, (object) currentStateOfEvents);
        }
        long valueIdentDailyState = SmokeDetectorHandlerFunctions.GetValueIdentDailyState();
        if (ValueIdent.IsExpectedValueIdent(filter, valueIdentDailyState))
        {
          foreach (MinoprotectIII_Events minoprotectIiiEvents in days)
          {
            SmokeDetectorEvent eventIdentification = minoprotectIiiEvents.EventIdentification;
            nullable1 = minoprotectIiiEvents.EventDate;
            DateTime timePoint = nullable1.Value;
            if (eventIdentification != ~(SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionForewarning | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined))
            {
              timePoint = timePoint.AddDays(1.0);
              DateTime dateTime1 = new DateTime(timePoint.Year, timePoint.Month, timePoint.Day);
              DateTime dateTime2;
              ref DateTime local = ref dateTime2;
              nullable1 = this.Parameter.CurrentDateTime;
              DateTime dateTime3 = nullable1.Value;
              int year = dateTime3.Year;
              nullable1 = this.Parameter.CurrentDateTime;
              dateTime3 = nullable1.Value;
              int month = dateTime3.Month;
              nullable1 = this.Parameter.CurrentDateTime;
              dateTime3 = nullable1.Value;
              int day = dateTime3.Day;
              local = new DateTime(year, month, day);
              if (!(dateTime1 > dateTime2))
                ValueIdent.AddValueToValueIdentList(ref valueList, timePoint, valueIdentDailyState, (object) (ushort) eventIdentification);
            }
          }
        }
        List<MinoprotectIII_Events> months = this.SummarizeEventsToMonths(days);
        if (months != null && months.Count > 1)
        {
          long identMonthlyState = SmokeDetectorHandlerFunctions.GetValueIdentMonthlyState();
          if (ValueIdent.IsExpectedValueIdent(filter, identMonthlyState))
          {
            foreach (MinoprotectIII_Events minoprotectIiiEvents in months)
            {
              if (minoprotectIiiEvents.EventIdentification != ~(SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionForewarning | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined) && months[months.Count - 1] != minoprotectIiiEvents)
              {
                SmokeDetectorEvent eventIdentification = minoprotectIiiEvents.EventIdentification;
                nullable1 = minoprotectIiiEvents.EventDate;
                DateTime timePoint = nullable1.Value.AddMonths(1);
                timePoint = new DateTime(timePoint.Year, timePoint.Month, 1);
                ValueIdent.AddValueToValueIdentList(ref valueList, timePoint, identMonthlyState, (object) (ushort) eventIdentification);
              }
            }
          }
        }
      }
      return valueList;
    }

    private List<MinoprotectIII_Events> SummarizeEventsToDays()
    {
      if (this.EventMemory == null)
        return (List<MinoprotectIII_Events>) null;
      List<MinoprotectIII_Events> days = new List<MinoprotectIII_Events>();
      SmokeDetectorEvent smokeDetectorEvent1 = ~(SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionForewarning | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined);
      foreach (MinoprotectIII_Events minoprotectIiiEvents1 in this.EventMemory)
      {
        DateTime? nullable1 = minoprotectIiiEvents1.EventDate;
        if (nullable1.HasValue)
        {
          nullable1 = minoprotectIiiEvents1.EventDate;
          DateTime? nullable2 = this.Parameter.DateOfFirstActivation;
          if (!(nullable1.HasValue & nullable2.HasValue) || !(nullable1.GetValueOrDefault() < nullable2.GetValueOrDefault()))
          {
            nullable2 = minoprotectIiiEvents1.EventDate;
            nullable1 = this.Parameter.CurrentDateTime;
            if (!(nullable2.HasValue & nullable1.HasValue) || !(nullable2.GetValueOrDefault() > nullable1.GetValueOrDefault()))
            {
              SmokeDetectorEvent eventIdentification = minoprotectIiiEvents1.EventIdentification;
              nullable1 = minoprotectIiiEvents1.EventDate;
              DateTime eventDate = nullable1.Value;
              object eventValue = minoprotectIiiEvents1.EventValue;
              MinoprotectIII_Events minoprotectIiiEvents2 = days.Find((Predicate<MinoprotectIII_Events>) (q =>
              {
                DateTime? eventDate1 = q.EventDate;
                DateTime dateTime = eventDate;
                if (!eventDate1.HasValue)
                  return false;
                return !eventDate1.HasValue || eventDate1.GetValueOrDefault() == dateTime;
              }));
              if (minoprotectIiiEvents2 != null)
              {
                SmokeDetectorEvent smokeDetectorEvent2 = eventIdentification;
                if ((uint) smokeDetectorEvent2 <= 64U)
                {
                  switch (smokeDetectorEvent2)
                  {
                    case SmokeDetectorEvent.BatteryForewarning:
                      if ((minoprotectIiiEvents2.EventIdentification & SmokeDetectorEvent.BatteryFault) == SmokeDetectorEvent.BatteryFault || (smokeDetectorEvent1 & SmokeDetectorEvent.BatteryFault) == SmokeDetectorEvent.BatteryFault)
                      {
                        smokeDetectorEvent1 &= SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionForewarning | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined;
                        minoprotectIiiEvents2.EventIdentification &= SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionForewarning | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined;
                        goto label_27;
                      }
                      else
                      {
                        smokeDetectorEvent1 |= SmokeDetectorEvent.BatteryForewarning;
                        minoprotectIiiEvents2.EventIdentification |= SmokeDetectorEvent.BatteryForewarning;
                        goto label_27;
                      }
                    case SmokeDetectorEvent.BatteryFault:
                      SmokeDetectorEvent smokeDetectorEvent3 = smokeDetectorEvent1 & (SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionForewarning | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined);
                      minoprotectIiiEvents2.EventIdentification &= SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionForewarning | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined;
                      smokeDetectorEvent1 = smokeDetectorEvent3 | SmokeDetectorEvent.BatteryFault;
                      minoprotectIiiEvents2.EventIdentification |= SmokeDetectorEvent.BatteryFault;
                      goto label_27;
                    case SmokeDetectorEvent.SmokeChamberPollutionForewarning:
                      bool boolean = Convert.ToBoolean(eventValue);
                      bool flag = (minoprotectIiiEvents2.EventIdentification & SmokeDetectorEvent.SmokeChamberPollutionWarning) == SmokeDetectorEvent.SmokeChamberPollutionWarning;
                      if (boolean && !flag)
                      {
                        smokeDetectorEvent1 |= SmokeDetectorEvent.SmokeChamberPollutionForewarning;
                        minoprotectIiiEvents2.EventIdentification |= SmokeDetectorEvent.SmokeChamberPollutionForewarning;
                        goto label_27;
                      }
                      else
                      {
                        smokeDetectorEvent1 &= SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined;
                        minoprotectIiiEvents2.EventIdentification &= SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined;
                        goto label_27;
                      }
                    case SmokeDetectorEvent.SmokeChamberPollutionWarning:
                      SmokeDetectorEvent smokeDetectorEvent4 = smokeDetectorEvent1 & (SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined);
                      minoprotectIiiEvents2.EventIdentification &= SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined;
                      if (Convert.ToBoolean(eventValue))
                      {
                        smokeDetectorEvent1 = smokeDetectorEvent4 | SmokeDetectorEvent.SmokeChamberPollutionWarning;
                        minoprotectIiiEvents2.EventIdentification |= SmokeDetectorEvent.SmokeChamberPollutionWarning;
                        goto label_27;
                      }
                      else
                      {
                        smokeDetectorEvent1 = smokeDetectorEvent4 & (SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionForewarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined);
                        minoprotectIiiEvents2.EventIdentification &= SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionForewarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined;
                        goto label_27;
                      }
                    case SmokeDetectorEvent.PushButtonFailure:
                    case SmokeDetectorEvent.HornFailure:
                      break;
                    default:
                      goto label_27;
                  }
                }
                else
                {
                  if ((uint) smokeDetectorEvent2 <= 512U)
                  {
                    switch (smokeDetectorEvent2)
                    {
                      case SmokeDetectorEvent.RemovingDetection:
                        goto label_23;
                      case SmokeDetectorEvent.TestAlarmReleased:
                      case SmokeDetectorEvent.SmokeAlarmReleased:
                        break;
                      default:
                        goto label_27;
                    }
                  }
                  else
                  {
                    switch (smokeDetectorEvent2)
                    {
                      case SmokeDetectorEvent.IngressAperturesObstructionDetected:
                      case SmokeDetectorEvent.ObjectInSurroundingAreaDetected:
                        goto label_23;
                      case SmokeDetectorEvent.LED_Failure:
                        break;
                      default:
                        goto label_27;
                    }
                  }
                  minoprotectIiiEvents2.EventIdentification |= eventIdentification;
                  goto label_27;
                }
label_23:
                if (Convert.ToBoolean(eventValue))
                {
                  smokeDetectorEvent1 |= eventIdentification;
                  minoprotectIiiEvents2.EventIdentification |= eventIdentification;
                }
                else
                {
                  smokeDetectorEvent1 &= ~eventIdentification;
                  minoprotectIiiEvents2.EventIdentification &= ~eventIdentification;
                }
label_27:
                minoprotectIiiEvents2.EventIdentification |= smokeDetectorEvent1;
              }
              else
              {
                MinoprotectIII_Events minoprotectIiiEvents3 = minoprotectIiiEvents1.DeepCopy();
                minoprotectIiiEvents3.EventIdentification |= smokeDetectorEvent1;
                SmokeDetectorEvent smokeDetectorEvent5 = eventIdentification;
                if ((uint) smokeDetectorEvent5 <= 64U)
                {
                  if ((uint) smokeDetectorEvent5 <= 8U)
                  {
                    if ((uint) (smokeDetectorEvent5 - (ushort) 1) > 1U)
                    {
                      if (smokeDetectorEvent5 == SmokeDetectorEvent.SmokeChamberPollutionForewarning)
                        goto label_37;
                      else
                        goto label_40;
                    }
                  }
                  else if (smokeDetectorEvent5 == SmokeDetectorEvent.SmokeChamberPollutionWarning || smokeDetectorEvent5 == SmokeDetectorEvent.PushButtonFailure || smokeDetectorEvent5 == SmokeDetectorEvent.HornFailure)
                    goto label_37;
                  else
                    goto label_40;
                }
                else if ((uint) smokeDetectorEvent5 <= 512U)
                {
                  switch (smokeDetectorEvent5)
                  {
                    case SmokeDetectorEvent.RemovingDetection:
                      goto label_37;
                    case SmokeDetectorEvent.TestAlarmReleased:
                    case SmokeDetectorEvent.SmokeAlarmReleased:
                      days.Add(minoprotectIiiEvents3);
                      goto label_40;
                    default:
                      goto label_40;
                  }
                }
                else
                {
                  switch (smokeDetectorEvent5)
                  {
                    case SmokeDetectorEvent.IngressAperturesObstructionDetected:
                    case SmokeDetectorEvent.ObjectInSurroundingAreaDetected:
                      goto label_37;
                    case SmokeDetectorEvent.LED_Failure:
                      break;
                    default:
                      goto label_40;
                  }
                }
                days.Add(minoprotectIiiEvents3);
                smokeDetectorEvent1 |= eventIdentification;
                goto label_40;
label_37:
                if (Convert.ToBoolean(minoprotectIiiEvents1.EventValue))
                {
                  days.Add(minoprotectIiiEvents3);
                  smokeDetectorEvent1 |= eventIdentification;
                }
label_40:;
              }
            }
          }
        }
      }
      return days;
    }

    private List<MinoprotectIII_Events> SummarizeEventsToMonths(
      List<MinoprotectIII_Events> summarizedDays)
    {
      if (summarizedDays == null)
        throw new ArgumentException(nameof (summarizedDays));
      List<MinoprotectIII_Events> months = new List<MinoprotectIII_Events>();
      foreach (MinoprotectIII_Events summarizedDay in summarizedDays)
      {
        MinoprotectIII_Events day = summarizedDay;
        MinoprotectIII_Events minoprotectIiiEvents = months.Find((Predicate<MinoprotectIII_Events>) (q =>
        {
          DateTime dateTime1 = q.EventDate.Value;
          int year1 = dateTime1.Year;
          dateTime1 = day.EventDate.Value;
          int year2 = dateTime1.Year;
          if (year1 != year2)
            return false;
          DateTime dateTime2 = q.EventDate.Value;
          int month1 = dateTime2.Month;
          dateTime2 = day.EventDate.Value;
          int month2 = dateTime2.Month;
          return month1 == month2;
        }));
        if (minoprotectIiiEvents != null)
        {
          minoprotectIiiEvents.EventIdentification |= day.EventIdentification;
          minoprotectIiiEvents.EventDate = day.EventDate;
        }
        else
          months.Add(day.DeepCopy());
      }
      return months;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.Version != null)
        stringBuilder.AppendLine(this.Version.ToString(20));
      if (this.Parameter != null)
        stringBuilder.AppendLine(this.Parameter.ToString(46));
      if (this.TestModeParameter != null)
        stringBuilder.AppendLine(this.TestModeParameter.ToString(36));
      if (this.ManufacturingParameter != null)
        stringBuilder.AppendLine(this.ManufacturingParameter.ToString(36));
      if (this.LoRaParameter != null)
        stringBuilder.AppendLine(this.LoRaParameter.ToString(36));
      if (this.EventMemory != null)
      {
        foreach (MinoprotectIII_Events minoprotectIiiEvents in this.EventMemory)
          stringBuilder.AppendLine(minoprotectIiiEvents.ToString());
      }
      SortedList<long, SortedList<DateTime, ReadingValue>> values = this.GetValues((List<long>) null);
      if (values != null)
      {
        stringBuilder.AppendLine();
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
            SmokeDetectorHandlerFunctions.GetValueIdentCurrentState(),
            typeof (SmokeDetectorEvent)
          },
          {
            SmokeDetectorHandlerFunctions.GetValueIdentDailyState(),
            typeof (SmokeDetectorEvent)
          },
          {
            SmokeDetectorHandlerFunctions.GetValueIdentMonthlyState(),
            typeof (SmokeDetectorEvent)
          }
        });
        if (!string.IsNullOrEmpty(str))
          stringBuilder.AppendLine(str);
      }
      return stringBuilder.ToString();
    }

    internal MinoprotectIII DeepCopy()
    {
      MinoprotectIII minoprotectIii = new MinoprotectIII();
      if (this.EventMemory != null)
      {
        minoprotectIii.EventMemory = new List<MinoprotectIII_Events>();
        foreach (MinoprotectIII_Events minoprotectIiiEvents in this.EventMemory)
          minoprotectIii.EventMemory.Add(minoprotectIiiEvents.DeepCopy());
      }
      if (this.Parameter != null)
        minoprotectIii.Parameter = this.Parameter.DeepCopy();
      if (this.LoRaParameter != null)
        minoprotectIii.LoRaParameter = this.LoRaParameter.DeepCopy();
      if (this.TestModeParameter != null)
        minoprotectIii.TestModeParameter = this.TestModeParameter.DeepCopy();
      if (this.ManufacturingParameter != null)
        minoprotectIii.ManufacturingParameter = this.ManufacturingParameter.DeepCopy();
      if (this.Version != null)
        minoprotectIii.Version = this.Version.DeepCopy();
      return minoprotectIii;
    }

    public byte[] Zip()
    {
      if (this.Version == null || this.Version.Buffer == null)
        throw new ArgumentNullException("Version", "Can not zip the data.");
      if (this.Parameter == null || this.Parameter.Buffer == null)
        throw new ArgumentNullException("Parameter", "Can not zip the data.");
      if (this.ManufacturingParameter == null || this.ManufacturingParameter.Buffer == null)
        throw new ArgumentNullException("ManufacturingParameter", "Can not zip the data.");
      if (this.Version.Manufacturer == null || this.Version.Manufacturer.Length != 3)
        this.Version.Manufacturer = "ZRI";
      bool flag = this.LoRaParameter != null;
      List<byte> byteList1 = new List<byte>();
      if (flag)
      {
        byteList1.Add((byte) 9);
        byteList1.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.Version.Serialnumber));
        byteList1.AddRange((IEnumerable<byte>) Encoding.ASCII.GetBytes(this.Version.Manufacturer));
        byteList1.Add((byte) this.Version.Medium);
        byteList1.Add(this.Version.Generation);
        byteList1.Add((byte) this.Version.Status);
        byteList1.AddRange((IEnumerable<byte>) this.Version.Buffer);
        byteList1.AddRange((IEnumerable<byte>) this.Parameter.Buffer);
        byteList1.AddRange((IEnumerable<byte>) this.ManufacturingParameter.Buffer);
        byteList1.AddRange((IEnumerable<byte>) this.LoRaParameter.JoinEUI);
        byteList1.AddRange((IEnumerable<byte>) this.LoRaParameter.DevEUI);
        byteList1.AddRange((IEnumerable<byte>) this.LoRaParameter.DevKey);
        byteList1.AddRange((IEnumerable<byte>) this.LoRaParameter.NwkSKey);
        byteList1.AddRange((IEnumerable<byte>) this.LoRaParameter.AppSKey);
        byteList1.Add((byte) this.LoRaParameter.Activation);
        byteList1.Add(this.LoRaParameter.TransmissionScenario);
        byteList1.Add(this.LoRaParameter.ADR);
        byteList1.AddRange((IEnumerable<byte>) this.LoRaParameter.ArmUniqueID);
        byteList1.AddRange((IEnumerable<byte>) this.LoRaParameter.MBusKey);
        byteList1.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.LoRaParameter.MBusIdent.SerialNumber));
        if (string.IsNullOrEmpty(this.LoRaParameter.MBusIdent.Manufacturer))
        {
          byteList1.AddRange((IEnumerable<byte>) new byte[2]);
        }
        else
        {
          ushort manufacturerCode = MinoprotectIII.GetManufacturerCode(this.LoRaParameter.MBusIdent.Manufacturer);
          byteList1.AddRange((IEnumerable<byte>) BitConverter.GetBytes(manufacturerCode));
        }
        byteList1.Add(this.LoRaParameter.MBusIdent.Generation);
        if (string.IsNullOrEmpty(this.LoRaParameter.MBusIdent.Medium))
        {
          byteList1.Add((byte) 0);
        }
        else
        {
          byte num = byte.Parse(Enum.Format(typeof (MinoprotectIII.Medium), Enum.Parse(typeof (MinoprotectIII.Medium), this.LoRaParameter.MBusIdent.Medium), "d"));
          byteList1.Add(num);
        }
        byteList1.Add(Convert.ToByte(this.LoRaParameter.RadioEnabled));
        ushort? mbusInterval = this.LoRaParameter.Mbus_interval;
        if (mbusInterval.HasValue)
        {
          List<byte> byteList2 = byteList1;
          mbusInterval = this.LoRaParameter.Mbus_interval;
          byte[] bytes = BitConverter.GetBytes(mbusInterval.Value);
          byteList2.AddRange((IEnumerable<byte>) bytes);
        }
        else
          byteList1.AddRange((IEnumerable<byte>) new byte[2]);
        SmokeDetectorHandlerFunctions.WeekDay? radioSuppressionDays = this.LoRaParameter.Mbus_radio_suppression_days;
        if (radioSuppressionDays.HasValue)
        {
          List<byte> byteList3 = byteList1;
          radioSuppressionDays = this.LoRaParameter.Mbus_radio_suppression_days;
          int num = (int) radioSuppressionDays.Value;
          byteList3.Add((byte) num);
        }
        else
          byteList1.Add((byte) 0);
        byte? nullable = this.LoRaParameter.Mbus_nighttime_start;
        if (nullable.HasValue)
        {
          List<byte> byteList4 = byteList1;
          nullable = this.LoRaParameter.Mbus_nighttime_start;
          int num = (int) nullable.Value;
          byteList4.Add((byte) num);
        }
        else
          byteList1.Add((byte) 0);
        nullable = this.LoRaParameter.Mbus_nighttime_stop;
        if (nullable.HasValue)
        {
          List<byte> byteList5 = byteList1;
          nullable = this.LoRaParameter.Mbus_nighttime_stop;
          int num = (int) nullable.Value;
          byteList5.Add((byte) num);
        }
        else
          byteList1.Add((byte) 0);
        byteList1.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.LoRaParameter.MBusIdentRadio3.SerialNumber));
        byteList1.AddRange((IEnumerable<byte>) this.LoRaParameter.LoRaWanVersion.basedata);
        byteList1.AddRange((IEnumerable<byte>) this.LoRaParameter.LoRaVersion.basedata);
        if (this.EventMemory != null && this.EventMemory.Count > 0)
        {
          foreach (MinoprotectIII_Events minoprotectIiiEvents in this.EventMemory)
          {
            byteList1.Add((byte) minoprotectIiiEvents.Buffer.Length);
            byteList1.AddRange((IEnumerable<byte>) minoprotectIiiEvents.Buffer);
          }
        }
        byteList1.Add((byte) 0);
      }
      else
      {
        byteList1.Add((byte) 2);
        byteList1.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.Version.Serialnumber));
        byteList1.AddRange((IEnumerable<byte>) Encoding.ASCII.GetBytes(this.Version.Manufacturer));
        byteList1.Add((byte) this.Version.Medium);
        byteList1.Add(this.Version.Generation);
        byteList1.Add((byte) this.Version.Status);
        byteList1.AddRange((IEnumerable<byte>) this.Version.Buffer);
        byteList1.AddRange((IEnumerable<byte>) this.Parameter.Buffer);
        byteList1.AddRange((IEnumerable<byte>) this.ManufacturingParameter.Buffer);
        if (this.EventMemory != null && this.EventMemory.Count > 0)
        {
          foreach (MinoprotectIII_Events minoprotectIiiEvents in this.EventMemory)
          {
            byteList1.Add((byte) minoprotectIiiEvents.Buffer.Length);
            byteList1.AddRange((IEnumerable<byte>) minoprotectIiiEvents.Buffer);
          }
        }
        byteList1.Add((byte) 0);
      }
      return Util.Zip(byteList1.ToArray());
    }

    public static ushort GetManufacturerCode(string manufacturer)
    {
      if (string.IsNullOrEmpty(manufacturer))
        manufacturer = string.Empty;
      manufacturer = manufacturer.PadRight(3);
      return (ushort) ((uint) (ushort) ((uint) (ushort) (0U + (uint) (ushort) ((uint) (byte) manufacturer[2] - 64U)) + (uint) (ushort) ((uint) (ushort) ((uint) (byte) manufacturer[1] - 64U) << 5)) + (uint) (ushort) ((uint) (ushort) ((uint) (byte) manufacturer[0] - 64U) << 10));
    }

    public static MinoprotectIII Unzip(byte[] buffer)
    {
      byte[] numArray1 = buffer != null ? Util.Unzip(buffer) : throw new ArgumentNullException(nameof (buffer), "Can not unzip the data!");
      if (numArray1 == null || numArray1.Length == 0)
        return (MinoprotectIII) null;
      if (numArray1[0] == (byte) 1)
      {
        int num = 77;
        if (numArray1.Length != num)
          return (MinoprotectIII) null;
        uint uint32 = BitConverter.ToUInt32(numArray1, 1);
        string manufacturer = Encoding.ASCII.GetString(numArray1, 5, 3);
        byte medium = numArray1[8];
        byte generation = numArray1[9];
        byte status = numArray1[10];
        byte[] numArray2 = new byte[10];
        byte[] numArray3 = new byte[44];
        byte[] numArray4 = new byte[12];
        int srcOffset1 = 11;
        int srcOffset2 = 21;
        int srcOffset3 = 65;
        Buffer.BlockCopy((Array) numArray1, srcOffset1, (Array) numArray2, 0, numArray2.Length);
        Buffer.BlockCopy((Array) numArray1, srcOffset2, (Array) numArray3, 0, numArray3.Length);
        Buffer.BlockCopy((Array) numArray1, srcOffset3, (Array) numArray4, 0, numArray4.Length);
        SmokeDetectorVersion smokeDetectorVersion = SmokeDetectorVersion.Parse(uint32, manufacturer, generation, medium, status, numArray2);
        MinoprotectIII_Parameter minoprotectIiiParameter = MinoprotectIII_Parameter.Parse(numArray3);
        ManufacturingParameter manufacturingParameter = ManufacturingParameter.Parse(numArray4);
        return new MinoprotectIII()
        {
          Version = smokeDetectorVersion,
          Parameter = minoprotectIiiParameter,
          ManufacturingParameter = manufacturingParameter
        };
      }
      if (numArray1[0] == (byte) 2)
      {
        int num = 78;
        if (numArray1.Length < num)
          return (MinoprotectIII) null;
        uint uint32 = BitConverter.ToUInt32(numArray1, 1);
        string manufacturer = Encoding.ASCII.GetString(numArray1, 5, 3);
        byte medium = numArray1[8];
        byte generation = numArray1[9];
        byte status = numArray1[10];
        byte[] numArray5 = new byte[10];
        byte[] numArray6 = new byte[44];
        byte[] numArray7 = new byte[12];
        int srcOffset4 = 11;
        int srcOffset5 = 21;
        int srcOffset6 = 65;
        Buffer.BlockCopy((Array) numArray1, srcOffset4, (Array) numArray5, 0, numArray5.Length);
        Buffer.BlockCopy((Array) numArray1, srcOffset5, (Array) numArray6, 0, numArray6.Length);
        Buffer.BlockCopy((Array) numArray1, srcOffset6, (Array) numArray7, 0, numArray7.Length);
        SmokeDetectorVersion smokeDetectorVersion = SmokeDetectorVersion.Parse(uint32, manufacturer, generation, medium, status, numArray5);
        MinoprotectIII_Parameter minoprotectIiiParameter = MinoprotectIII_Parameter.Parse(numArray6);
        ManufacturingParameter manufacturingParameter = ManufacturingParameter.Parse(numArray7);
        int index1 = srcOffset6 + numArray7.Length;
        List<byte> byteList = new List<byte>();
        while (numArray1[index1] > (byte) 0)
        {
          byte[] numArray8 = numArray1;
          int index2 = index1;
          int srcOffset7 = index2 + 1;
          byte[] numArray9 = new byte[(int) numArray8[index2]];
          Buffer.BlockCopy((Array) numArray1, srcOffset7, (Array) numArray9, 0, numArray9.Length);
          index1 = srcOffset7 + numArray9.Length;
          byteList.AddRange((IEnumerable<byte>) numArray9);
        }
        return new MinoprotectIII()
        {
          Version = smokeDetectorVersion,
          Parameter = minoprotectIiiParameter,
          ManufacturingParameter = manufacturingParameter,
          EventMemory = byteList.Count > 0 ? MinoprotectIII_Events.Parse(byteList.ToArray()) : (List<MinoprotectIII_Events>) null
        };
      }
      if (numArray1[0] == (byte) 3)
      {
        int num = 88;
        if (numArray1.Length < num)
          return (MinoprotectIII) null;
        uint uint32 = BitConverter.ToUInt32(numArray1, 1);
        string manufacturer = Encoding.ASCII.GetString(numArray1, 5, 3);
        byte medium = numArray1[8];
        byte generation = numArray1[9];
        byte status = numArray1[10];
        byte[] numArray10 = new byte[10];
        byte[] numArray11 = new byte[44];
        byte[] numArray12 = new byte[12];
        int srcOffset8 = 11;
        int srcOffset9 = 21;
        int srcOffset10 = 65;
        Buffer.BlockCopy((Array) numArray1, srcOffset8, (Array) numArray10, 0, numArray10.Length);
        Buffer.BlockCopy((Array) numArray1, srcOffset9, (Array) numArray11, 0, numArray11.Length);
        Buffer.BlockCopy((Array) numArray1, srcOffset10, (Array) numArray12, 0, numArray12.Length);
        SmokeDetectorVersion smokeDetectorVersion = SmokeDetectorVersion.Parse(uint32, manufacturer, generation, medium, status, numArray10);
        MinoprotectIII_Parameter minoprotectIiiParameter = MinoprotectIII_Parameter.Parse(numArray11);
        ManufacturingParameter manufacturingParameter = ManufacturingParameter.Parse(numArray12);
        int index3 = srcOffset10 + numArray12.Length;
        LoRaParameter loRaParameter = new LoRaParameter()
        {
          JoinEUI = numArray1.SubArray<byte>(index3, 8),
          DevEUI = numArray1.SubArray<byte>(index3 + 8, 8),
          DevKey = numArray1.SubArray<byte>(index3 + 16, 16),
          NwkSKey = numArray1.SubArray<byte>(index3 + 32, 16),
          AppSKey = numArray1.SubArray<byte>(index3 + 48, 16),
          Activation = (OTAA_ABP) numArray1[index3 + 64],
          TransmissionScenario = numArray1[index3 + 65],
          ADR = numArray1[index3 + 66],
          ArmUniqueID = numArray1.SubArray<byte>(index3 + 67, 12)
        };
        int index4 = index3 + 79;
        List<byte> byteList = new List<byte>();
        while (numArray1[index4] > (byte) 0)
        {
          byte[] numArray13 = numArray1;
          int index5 = index4;
          int srcOffset11 = index5 + 1;
          byte[] numArray14 = new byte[(int) numArray13[index5]];
          Buffer.BlockCopy((Array) numArray1, srcOffset11, (Array) numArray14, 0, numArray14.Length);
          index4 = srcOffset11 + numArray14.Length;
          byteList.AddRange((IEnumerable<byte>) numArray14);
        }
        return new MinoprotectIII()
        {
          Version = smokeDetectorVersion,
          Parameter = minoprotectIiiParameter,
          ManufacturingParameter = manufacturingParameter,
          LoRaParameter = loRaParameter,
          EventMemory = byteList.Count > 0 ? MinoprotectIII_Events.Parse(byteList.ToArray()) : (List<MinoprotectIII_Events>) null
        };
      }
      if (numArray1[0] == (byte) 4)
      {
        int num = 104;
        if (numArray1.Length < num)
          return (MinoprotectIII) null;
        uint uint32 = BitConverter.ToUInt32(numArray1, 1);
        string manufacturer = Encoding.ASCII.GetString(numArray1, 5, 3);
        byte medium = numArray1[8];
        byte generation = numArray1[9];
        byte status = numArray1[10];
        byte[] numArray15 = new byte[10];
        byte[] numArray16 = new byte[44];
        byte[] numArray17 = new byte[12];
        int srcOffset12 = 11;
        int srcOffset13 = 21;
        int srcOffset14 = 65;
        Buffer.BlockCopy((Array) numArray1, srcOffset12, (Array) numArray15, 0, numArray15.Length);
        Buffer.BlockCopy((Array) numArray1, srcOffset13, (Array) numArray16, 0, numArray16.Length);
        Buffer.BlockCopy((Array) numArray1, srcOffset14, (Array) numArray17, 0, numArray17.Length);
        SmokeDetectorVersion smokeDetectorVersion = SmokeDetectorVersion.Parse(uint32, manufacturer, generation, medium, status, numArray15);
        MinoprotectIII_Parameter minoprotectIiiParameter = MinoprotectIII_Parameter.Parse(numArray16);
        ManufacturingParameter manufacturingParameter = ManufacturingParameter.Parse(numArray17);
        int index6 = srcOffset14 + numArray17.Length;
        LoRaParameter loRaParameter = new LoRaParameter()
        {
          JoinEUI = numArray1.SubArray<byte>(index6, 8),
          DevEUI = numArray1.SubArray<byte>(index6 + 8, 8),
          DevKey = numArray1.SubArray<byte>(index6 + 16, 16),
          NwkSKey = numArray1.SubArray<byte>(index6 + 32, 16),
          AppSKey = numArray1.SubArray<byte>(index6 + 48, 16),
          Activation = (OTAA_ABP) numArray1[index6 + 64],
          TransmissionScenario = numArray1[index6 + 65],
          ADR = numArray1[index6 + 66],
          ArmUniqueID = numArray1.SubArray<byte>(index6 + 67, 12),
          MBusKey = numArray1.SubArray<byte>(index6 + 79, 16)
        };
        int index7 = index6 + 95;
        List<byte> byteList = new List<byte>();
        while (numArray1[index7] > (byte) 0)
        {
          byte[] numArray18 = numArray1;
          int index8 = index7;
          int srcOffset15 = index8 + 1;
          byte[] numArray19 = new byte[(int) numArray18[index8]];
          Buffer.BlockCopy((Array) numArray1, srcOffset15, (Array) numArray19, 0, numArray19.Length);
          index7 = srcOffset15 + numArray19.Length;
          byteList.AddRange((IEnumerable<byte>) numArray19);
        }
        return new MinoprotectIII()
        {
          Version = smokeDetectorVersion,
          Parameter = minoprotectIiiParameter,
          ManufacturingParameter = manufacturingParameter,
          LoRaParameter = loRaParameter,
          EventMemory = byteList.Count > 0 ? MinoprotectIII_Events.Parse(byteList.ToArray()) : (List<MinoprotectIII_Events>) null
        };
      }
      if (numArray1[0] == (byte) 5)
      {
        int num = 104;
        if (numArray1.Length < num)
          return (MinoprotectIII) null;
        uint uint32 = BitConverter.ToUInt32(numArray1, 1);
        string manufacturer = Encoding.ASCII.GetString(numArray1, 5, 3);
        byte medium = numArray1[8];
        byte generation = numArray1[9];
        byte status = numArray1[10];
        byte[] numArray20 = new byte[10];
        byte[] numArray21 = new byte[44];
        byte[] numArray22 = new byte[12];
        int srcOffset16 = 11;
        int srcOffset17 = 21;
        int srcOffset18 = 65;
        Buffer.BlockCopy((Array) numArray1, srcOffset16, (Array) numArray20, 0, numArray20.Length);
        Buffer.BlockCopy((Array) numArray1, srcOffset17, (Array) numArray21, 0, numArray21.Length);
        Buffer.BlockCopy((Array) numArray1, srcOffset18, (Array) numArray22, 0, numArray22.Length);
        SmokeDetectorVersion smokeDetectorVersion = SmokeDetectorVersion.Parse(uint32, manufacturer, generation, medium, status, numArray20);
        MinoprotectIII_Parameter minoprotectIiiParameter = MinoprotectIII_Parameter.Parse(numArray21);
        ManufacturingParameter manufacturingParameter = ManufacturingParameter.Parse(numArray22);
        int index9 = srcOffset18 + numArray22.Length;
        LoRaParameter loRaParameter = new LoRaParameter()
        {
          JoinEUI = numArray1.SubArray<byte>(index9, 8),
          DevEUI = numArray1.SubArray<byte>(index9 + 8, 8),
          DevKey = numArray1.SubArray<byte>(index9 + 16, 16),
          NwkSKey = numArray1.SubArray<byte>(index9 + 32, 16),
          AppSKey = numArray1.SubArray<byte>(index9 + 48, 16),
          Activation = (OTAA_ABP) numArray1[index9 + 64],
          TransmissionScenario = numArray1[index9 + 65],
          ADR = numArray1[index9 + 66],
          ArmUniqueID = numArray1.SubArray<byte>(index9 + 67, 12),
          MBusKey = numArray1.SubArray<byte>(index9 + 79, 16)
        };
        loRaParameter.MBusIdent.SerialNumber = BitConverter.ToInt64(numArray1, index9 + 95);
        loRaParameter.MBusIdent.Manufacturer = MinoprotectIII.GetManufacturer(BitConverter.ToUInt16(numArray1, index9 + 103));
        loRaParameter.MBusIdent.Generation = numArray1[index9 + 105];
        loRaParameter.MBusIdent.Medium = MinoprotectIII.GetMedium(numArray1[index9 + 106]);
        int index10 = index9 + 107;
        List<byte> byteList = new List<byte>();
        while (numArray1[index10] > (byte) 0)
        {
          byte[] numArray23 = numArray1;
          int index11 = index10;
          int srcOffset19 = index11 + 1;
          byte[] numArray24 = new byte[(int) numArray23[index11]];
          Buffer.BlockCopy((Array) numArray1, srcOffset19, (Array) numArray24, 0, numArray24.Length);
          index10 = srcOffset19 + numArray24.Length;
          byteList.AddRange((IEnumerable<byte>) numArray24);
        }
        return new MinoprotectIII()
        {
          Version = smokeDetectorVersion,
          Parameter = minoprotectIiiParameter,
          ManufacturingParameter = manufacturingParameter,
          LoRaParameter = loRaParameter,
          EventMemory = byteList.Count > 0 ? MinoprotectIII_Events.Parse(byteList.ToArray()) : (List<MinoprotectIII_Events>) null
        };
      }
      if (numArray1[0] == (byte) 6)
      {
        int num = 104;
        if (numArray1.Length < num)
          return (MinoprotectIII) null;
        uint uint32 = BitConverter.ToUInt32(numArray1, 1);
        string manufacturer = Encoding.ASCII.GetString(numArray1, 5, 3);
        byte medium = numArray1[8];
        byte generation = numArray1[9];
        byte status = numArray1[10];
        byte[] numArray25 = new byte[10];
        byte[] numArray26 = new byte[44];
        byte[] numArray27 = new byte[12];
        int srcOffset20 = 11;
        int srcOffset21 = 21;
        int srcOffset22 = 65;
        Buffer.BlockCopy((Array) numArray1, srcOffset20, (Array) numArray25, 0, numArray25.Length);
        Buffer.BlockCopy((Array) numArray1, srcOffset21, (Array) numArray26, 0, numArray26.Length);
        Buffer.BlockCopy((Array) numArray1, srcOffset22, (Array) numArray27, 0, numArray27.Length);
        SmokeDetectorVersion smokeDetectorVersion = SmokeDetectorVersion.Parse(uint32, manufacturer, generation, medium, status, numArray25);
        MinoprotectIII_Parameter minoprotectIiiParameter = MinoprotectIII_Parameter.Parse(numArray26);
        ManufacturingParameter manufacturingParameter = ManufacturingParameter.Parse(numArray27);
        int index12 = srcOffset22 + numArray27.Length;
        LoRaParameter loRaParameter = new LoRaParameter()
        {
          JoinEUI = numArray1.SubArray<byte>(index12, 8),
          DevEUI = numArray1.SubArray<byte>(index12 + 8, 8),
          DevKey = numArray1.SubArray<byte>(index12 + 16, 16),
          NwkSKey = numArray1.SubArray<byte>(index12 + 32, 16),
          AppSKey = numArray1.SubArray<byte>(index12 + 48, 16),
          Activation = (OTAA_ABP) numArray1[index12 + 64],
          TransmissionScenario = numArray1[index12 + 65],
          ADR = numArray1[index12 + 66],
          ArmUniqueID = numArray1.SubArray<byte>(index12 + 67, 12),
          MBusKey = numArray1.SubArray<byte>(index12 + 79, 16)
        };
        loRaParameter.MBusIdent.SerialNumber = BitConverter.ToInt64(numArray1, index12 + 95);
        loRaParameter.MBusIdent.Manufacturer = MinoprotectIII.GetManufacturer(BitConverter.ToUInt16(numArray1, index12 + 103));
        loRaParameter.MBusIdent.Generation = numArray1[index12 + 105];
        loRaParameter.MBusIdent.Medium = MinoprotectIII.GetMedium(numArray1[index12 + 106]);
        loRaParameter.RadioEnabled = BitConverter.ToBoolean(numArray1, index12 + 107);
        int index13 = index12 + 108;
        List<byte> byteList = new List<byte>();
        while (numArray1[index13] > (byte) 0)
        {
          byte[] numArray28 = numArray1;
          int index14 = index13;
          int srcOffset23 = index14 + 1;
          byte[] numArray29 = new byte[(int) numArray28[index14]];
          Buffer.BlockCopy((Array) numArray1, srcOffset23, (Array) numArray29, 0, numArray29.Length);
          index13 = srcOffset23 + numArray29.Length;
          byteList.AddRange((IEnumerable<byte>) numArray29);
        }
        return new MinoprotectIII()
        {
          Version = smokeDetectorVersion,
          Parameter = minoprotectIiiParameter,
          ManufacturingParameter = manufacturingParameter,
          LoRaParameter = loRaParameter,
          EventMemory = byteList.Count > 0 ? MinoprotectIII_Events.Parse(byteList.ToArray()) : (List<MinoprotectIII_Events>) null
        };
      }
      if (numArray1[0] == (byte) 7)
      {
        int num = 104;
        if (numArray1.Length < num)
          return (MinoprotectIII) null;
        uint uint32 = BitConverter.ToUInt32(numArray1, 1);
        string manufacturer = Encoding.ASCII.GetString(numArray1, 5, 3);
        byte medium = numArray1[8];
        byte generation = numArray1[9];
        byte status = numArray1[10];
        byte[] numArray30 = new byte[10];
        byte[] numArray31 = new byte[44];
        byte[] numArray32 = new byte[12];
        int srcOffset24 = 11;
        int srcOffset25 = 21;
        int srcOffset26 = 65;
        Buffer.BlockCopy((Array) numArray1, srcOffset24, (Array) numArray30, 0, numArray30.Length);
        Buffer.BlockCopy((Array) numArray1, srcOffset25, (Array) numArray31, 0, numArray31.Length);
        Buffer.BlockCopy((Array) numArray1, srcOffset26, (Array) numArray32, 0, numArray32.Length);
        SmokeDetectorVersion smokeDetectorVersion = SmokeDetectorVersion.Parse(uint32, manufacturer, generation, medium, status, numArray30);
        MinoprotectIII_Parameter minoprotectIiiParameter = MinoprotectIII_Parameter.Parse(numArray31);
        ManufacturingParameter manufacturingParameter = ManufacturingParameter.Parse(numArray32);
        int index15 = srcOffset26 + numArray32.Length;
        LoRaParameter loRaParameter = new LoRaParameter()
        {
          JoinEUI = numArray1.SubArray<byte>(index15, 8),
          DevEUI = numArray1.SubArray<byte>(index15 + 8, 8),
          DevKey = numArray1.SubArray<byte>(index15 + 16, 16),
          NwkSKey = numArray1.SubArray<byte>(index15 + 32, 16),
          AppSKey = numArray1.SubArray<byte>(index15 + 48, 16),
          Activation = (OTAA_ABP) numArray1[index15 + 64],
          TransmissionScenario = numArray1[index15 + 65],
          ADR = numArray1[index15 + 66],
          ArmUniqueID = numArray1.SubArray<byte>(index15 + 67, 12),
          MBusKey = numArray1.SubArray<byte>(index15 + 79, 16)
        };
        loRaParameter.MBusIdent.SerialNumber = BitConverter.ToInt64(numArray1, index15 + 95);
        loRaParameter.MBusIdent.Manufacturer = MinoprotectIII.GetManufacturer(BitConverter.ToUInt16(numArray1, index15 + 103));
        loRaParameter.MBusIdent.Generation = numArray1[index15 + 105];
        loRaParameter.MBusIdent.Medium = MinoprotectIII.GetMedium(numArray1[index15 + 106]);
        loRaParameter.RadioEnabled = BitConverter.ToBoolean(numArray1, index15 + 107);
        loRaParameter.Mbus_interval = new ushort?(BitConverter.ToUInt16(numArray1, index15 + 108));
        loRaParameter.Mbus_radio_suppression_days = new SmokeDetectorHandlerFunctions.WeekDay?((SmokeDetectorHandlerFunctions.WeekDay) numArray1[index15 + 110]);
        loRaParameter.Mbus_nighttime_start = new byte?(numArray1[index15 + 111]);
        loRaParameter.Mbus_nighttime_stop = new byte?(numArray1[index15 + 112]);
        int index16 = index15 + 113;
        List<byte> byteList = new List<byte>();
        while (numArray1[index16] > (byte) 0)
        {
          byte[] numArray33 = numArray1;
          int index17 = index16;
          int srcOffset27 = index17 + 1;
          byte[] numArray34 = new byte[(int) numArray33[index17]];
          Buffer.BlockCopy((Array) numArray1, srcOffset27, (Array) numArray34, 0, numArray34.Length);
          index16 = srcOffset27 + numArray34.Length;
          byteList.AddRange((IEnumerable<byte>) numArray34);
        }
        return new MinoprotectIII()
        {
          Version = smokeDetectorVersion,
          Parameter = minoprotectIiiParameter,
          ManufacturingParameter = manufacturingParameter,
          LoRaParameter = loRaParameter,
          EventMemory = byteList.Count > 0 ? MinoprotectIII_Events.Parse(byteList.ToArray()) : (List<MinoprotectIII_Events>) null
        };
      }
      if (numArray1[0] == (byte) 8)
      {
        int num = 104;
        if (numArray1.Length < num)
          return (MinoprotectIII) null;
        uint uint32 = BitConverter.ToUInt32(numArray1, 1);
        string manufacturer = Encoding.ASCII.GetString(numArray1, 5, 3);
        byte medium = numArray1[8];
        byte generation = numArray1[9];
        byte status = numArray1[10];
        byte[] numArray35 = new byte[10];
        byte[] numArray36 = new byte[44];
        byte[] numArray37 = new byte[12];
        int srcOffset28 = 11;
        int srcOffset29 = 21;
        int srcOffset30 = 65;
        Buffer.BlockCopy((Array) numArray1, srcOffset28, (Array) numArray35, 0, numArray35.Length);
        Buffer.BlockCopy((Array) numArray1, srcOffset29, (Array) numArray36, 0, numArray36.Length);
        Buffer.BlockCopy((Array) numArray1, srcOffset30, (Array) numArray37, 0, numArray37.Length);
        SmokeDetectorVersion smokeDetectorVersion = SmokeDetectorVersion.Parse(uint32, manufacturer, generation, medium, status, numArray35);
        MinoprotectIII_Parameter minoprotectIiiParameter = MinoprotectIII_Parameter.Parse(numArray36);
        ManufacturingParameter manufacturingParameter = ManufacturingParameter.Parse(numArray37);
        int index18 = srcOffset30 + numArray37.Length;
        LoRaParameter loRaParameter = new LoRaParameter()
        {
          JoinEUI = numArray1.SubArray<byte>(index18, 8),
          DevEUI = numArray1.SubArray<byte>(index18 + 8, 8),
          DevKey = numArray1.SubArray<byte>(index18 + 16, 16),
          NwkSKey = numArray1.SubArray<byte>(index18 + 32, 16),
          AppSKey = numArray1.SubArray<byte>(index18 + 48, 16),
          Activation = (OTAA_ABP) numArray1[index18 + 64],
          TransmissionScenario = numArray1[index18 + 65],
          ADR = numArray1[index18 + 66],
          ArmUniqueID = numArray1.SubArray<byte>(index18 + 67, 12),
          MBusKey = numArray1.SubArray<byte>(index18 + 79, 16)
        };
        loRaParameter.MBusIdent.SerialNumber = BitConverter.ToInt64(numArray1, index18 + 95);
        loRaParameter.MBusIdent.Manufacturer = MinoprotectIII.GetManufacturer(BitConverter.ToUInt16(numArray1, index18 + 103));
        loRaParameter.MBusIdent.Generation = numArray1[index18 + 105];
        loRaParameter.MBusIdent.Medium = MinoprotectIII.GetMedium(numArray1[index18 + 106]);
        loRaParameter.RadioEnabled = BitConverter.ToBoolean(numArray1, index18 + 107);
        loRaParameter.Mbus_interval = new ushort?(BitConverter.ToUInt16(numArray1, index18 + 108));
        loRaParameter.Mbus_radio_suppression_days = new SmokeDetectorHandlerFunctions.WeekDay?((SmokeDetectorHandlerFunctions.WeekDay) numArray1[index18 + 110]);
        loRaParameter.Mbus_nighttime_start = new byte?(numArray1[index18 + 111]);
        loRaParameter.Mbus_nighttime_stop = new byte?(numArray1[index18 + 112]);
        loRaParameter.MBusIdentRadio3.SerialNumber = BitConverter.ToInt64(numArray1, index18 + 113);
        int index19 = index18 + 121;
        List<byte> byteList = new List<byte>();
        while (numArray1[index19] > (byte) 0)
        {
          byte[] numArray38 = numArray1;
          int index20 = index19;
          int srcOffset31 = index20 + 1;
          byte[] numArray39 = new byte[(int) numArray38[index20]];
          Buffer.BlockCopy((Array) numArray1, srcOffset31, (Array) numArray39, 0, numArray39.Length);
          index19 = srcOffset31 + numArray39.Length;
          byteList.AddRange((IEnumerable<byte>) numArray39);
        }
        return new MinoprotectIII()
        {
          Version = smokeDetectorVersion,
          Parameter = minoprotectIiiParameter,
          ManufacturingParameter = manufacturingParameter,
          LoRaParameter = loRaParameter,
          EventMemory = byteList.Count > 0 ? MinoprotectIII_Events.Parse(byteList.ToArray()) : (List<MinoprotectIII_Events>) null
        };
      }
      if (numArray1[0] != (byte) 9)
        return (MinoprotectIII) null;
      int num1 = 104;
      if (numArray1.Length < num1)
        return (MinoprotectIII) null;
      uint uint32_1 = BitConverter.ToUInt32(numArray1, 1);
      string manufacturer1 = Encoding.ASCII.GetString(numArray1, 5, 3);
      byte medium1 = numArray1[8];
      byte generation1 = numArray1[9];
      byte status1 = numArray1[10];
      byte[] numArray40 = new byte[10];
      byte[] numArray41 = new byte[44];
      byte[] numArray42 = new byte[12];
      int srcOffset32 = 11;
      int srcOffset33 = 21;
      int srcOffset34 = 65;
      Buffer.BlockCopy((Array) numArray1, srcOffset32, (Array) numArray40, 0, numArray40.Length);
      Buffer.BlockCopy((Array) numArray1, srcOffset33, (Array) numArray41, 0, numArray41.Length);
      Buffer.BlockCopy((Array) numArray1, srcOffset34, (Array) numArray42, 0, numArray42.Length);
      SmokeDetectorVersion smokeDetectorVersion1 = SmokeDetectorVersion.Parse(uint32_1, manufacturer1, generation1, medium1, status1, numArray40);
      MinoprotectIII_Parameter minoprotectIiiParameter1 = MinoprotectIII_Parameter.Parse(numArray41);
      ManufacturingParameter manufacturingParameter1 = ManufacturingParameter.Parse(numArray42);
      int index21 = srcOffset34 + numArray42.Length;
      LoRaParameter loRaParameter1 = new LoRaParameter()
      {
        JoinEUI = numArray1.SubArray<byte>(index21, 8),
        DevEUI = numArray1.SubArray<byte>(index21 + 8, 8),
        DevKey = numArray1.SubArray<byte>(index21 + 16, 16),
        NwkSKey = numArray1.SubArray<byte>(index21 + 32, 16),
        AppSKey = numArray1.SubArray<byte>(index21 + 48, 16),
        Activation = (OTAA_ABP) numArray1[index21 + 64],
        TransmissionScenario = numArray1[index21 + 65],
        ADR = numArray1[index21 + 66],
        ArmUniqueID = numArray1.SubArray<byte>(index21 + 67, 12),
        MBusKey = numArray1.SubArray<byte>(index21 + 79, 16)
      };
      loRaParameter1.MBusIdent.SerialNumber = BitConverter.ToInt64(numArray1, index21 + 95);
      loRaParameter1.MBusIdent.Manufacturer = MinoprotectIII.GetManufacturer(BitConverter.ToUInt16(numArray1, index21 + 103));
      loRaParameter1.MBusIdent.Generation = numArray1[index21 + 105];
      loRaParameter1.MBusIdent.Medium = MinoprotectIII.GetMedium(numArray1[index21 + 106]);
      loRaParameter1.RadioEnabled = BitConverter.ToBoolean(numArray1, index21 + 107);
      loRaParameter1.Mbus_interval = new ushort?(BitConverter.ToUInt16(numArray1, index21 + 108));
      loRaParameter1.Mbus_radio_suppression_days = new SmokeDetectorHandlerFunctions.WeekDay?((SmokeDetectorHandlerFunctions.WeekDay) numArray1[index21 + 110]);
      loRaParameter1.Mbus_nighttime_start = new byte?(numArray1[index21 + 111]);
      loRaParameter1.Mbus_nighttime_stop = new byte?(numArray1[index21 + 112]);
      loRaParameter1.MBusIdentRadio3.SerialNumber = BitConverter.ToInt64(numArray1, index21 + 113);
      loRaParameter1.LoRaWanVersion = new LoRaWANVersion();
      loRaParameter1.LoRaWanVersion.MainVersion = numArray1[index21 + 121];
      loRaParameter1.LoRaWanVersion.MinorVersion = numArray1[index21 + 122];
      loRaParameter1.LoRaWanVersion.ReleaseNr = numArray1[index21 + 123];
      loRaParameter1.LoRaWanVersion.basedata = new byte[3]
      {
        numArray1[index21 + 121],
        numArray1[index21 + 122],
        numArray1[index21 + 123]
      };
      loRaParameter1.LoRaVersion = new LoRaFcVersion();
      loRaParameter1.LoRaVersion.Version = BitConverter.ToUInt16(numArray1, index21 + 124);
      loRaParameter1.LoRaVersion.basedata = new byte[2]
      {
        numArray1[index21 + 124],
        numArray1[index21 + 125]
      };
      int index22 = index21 + 126;
      List<byte> byteList1 = new List<byte>();
      while (numArray1[index22] > (byte) 0)
      {
        byte[] numArray43 = numArray1;
        int index23 = index22;
        int srcOffset35 = index23 + 1;
        byte[] numArray44 = new byte[(int) numArray43[index23]];
        Buffer.BlockCopy((Array) numArray1, srcOffset35, (Array) numArray44, 0, numArray44.Length);
        index22 = srcOffset35 + numArray44.Length;
        byteList1.AddRange((IEnumerable<byte>) numArray44);
      }
      return new MinoprotectIII()
      {
        Version = smokeDetectorVersion1,
        Parameter = minoprotectIiiParameter1,
        ManufacturingParameter = manufacturingParameter1,
        LoRaParameter = loRaParameter1,
        EventMemory = byteList1.Count > 0 ? MinoprotectIII_Events.Parse(byteList1.ToArray()) : (List<MinoprotectIII_Events>) null
      };
    }

    public static string GetManufacturer(ushort code)
    {
      return "" + ((char) (((int) code >> 10 & 31) + 64)).ToString() + ((char) (((int) code >> 5 & 31) + 64)).ToString() + ((char) (((int) code & 31) + 64)).ToString();
    }

    public static string GetMedium(byte medium)
    {
      return Enum.IsDefined(typeof (MinoprotectIII.Medium), (object) medium) ? Enum.ToObject(typeof (MinoprotectIII.Medium), medium).ToString() : string.Empty;
    }

    public enum Medium : byte
    {
      OTHER = 0,
      OIL = 1,
      ELECTRICITY = 2,
      GAS = 3,
      HEAT_OUTLET = 4,
      STEAM = 5,
      HOT_WATER = 6,
      WATER = 7,
      HCA = 8,
      COMPRESSED_AIR = 9,
      COOL_OUTLET = 10, // 0x0A
      COOL_INLET = 11, // 0x0B
      HEAT_INLET = 12, // 0x0C
      HEAT_AND_COOL = 13, // 0x0D
      BUS_SYSTEM = 14, // 0x0E
      UNKNOWN = 15, // 0x0F
      HEAT_ENERGY_VALUE = 20, // 0x14
      HOT_WATER_90 = 21, // 0x15
      COLD_WATER = 22, // 0x16
      HOT_AND_COLD_WATER = 23, // 0x17
      PRESSURE = 24, // 0x18
      AD_CONVERTER = 25, // 0x19
      SMOKE_DETECTOR = 26, // 0x1A
      ROOM_SENSOR = 27, // 0x1B
      GAS_DETECTOR = 28, // 0x1C
      CIRCUIT_BREAKER = 32, // 0x20
      GAS_WATER_OUTLET = 33, // 0x21
      CUSTOMER_DISPLAY = 37, // 0x25
      EFFLUENT_WATER = 40, // 0x28
      WASTE = 41, // 0x29
      CARBON_MONOXIDE = 42, // 0x2A
      RF_Adapter = 55, // 0x37
    }
  }
}
