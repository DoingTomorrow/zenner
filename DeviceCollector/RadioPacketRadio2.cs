// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RadioPacketRadio2
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  [Serializable]
  public sealed class RadioPacketRadio2 : RadioDevicePacket
  {
    private static Logger logger = LogManager.GetLogger(nameof (RadioPacketRadio2));
    public const int LENGTH_SHORT_TELEGRAM = 24;
    public const int LENGTH_LONG_TELEGRAM = 32;

    public byte MonthIndex { get; private set; }

    public Decimal Exponent2F { get; private set; }

    public bool MeasurementError { get; private set; }

    public bool IsMeasurementEnabled { get; private set; }

    public bool IsSummerTime { get; private set; }

    public Decimal? CurrentValue { get; private set; }

    public ushort K { get; private set; }

    public ushort ScaleFactor { get; private set; }

    public InputUnitsIndex? Unit { get; private set; }

    public bool IsShortPackage { get; private set; }

    public HCA_SensorMode SensorMode { get; private set; }

    public DateTime DueDate { get; private set; }

    public Decimal? DueDateValue { get; private set; }

    public DateTime DynamicDate { get; private set; }

    public Decimal? DynamicValue { get; private set; }

    public bool IsHeatCostAllocator
    {
      get => this.GetValueIdPart_MeterType() == ValueIdent.ValueIdPart_MeterType.HeatCostAllocator;
    }

    public override bool Parse(byte[] packet, DateTime receivedAt, bool hasRssi)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.WalkBy))
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission of radio2!");
      if (packet == null)
        throw new ArgumentNullException("Input parameter 'packet' can not be null!");
      this.ReceivedAt = receivedAt;
      if (packet.Length < 32)
        return false;
      this.Buffer = packet;
      if (!this.GetCRCValue(6) || !this.GetCRCValue(14) || !this.GetCRCValue(22))
        return false;
      bool crcValue = this.GetCRCValue(30);
      int num1 = 0;
      byte[] buffer1 = this.Buffer;
      int index1 = num1;
      int num2 = index1 + 1;
      this.MonthIndex = buffer1[index1];
      byte[] buffer2 = this.Buffer;
      int index2 = num2;
      int num3 = index2 + 1;
      byte num4 = buffer2[index2];
      this.SensorMode = HCA_SensorMode.Single;
      if (((int) num4 & 128) == 128)
        this.SensorMode = HCA_SensorMode.Double;
      this.Exponent2F = 1.15M;
      if (((int) num4 & 64) == 64)
        this.Exponent2F = 1.35M;
      this.IsManipulated = ((int) num4 & 32) == 32;
      this.IsDeviceError = ((int) num4 & 16) == 16;
      this.MeasurementError = ((int) num4 & 8) == 8;
      this.IsMeasurementEnabled = ((int) num4 & 4) != 4;
      this.IsSummerTime = ((int) num4 & 2) == 2;
      byte[] buffer3 = this.Buffer;
      int index3 = num3;
      int num5 = index3 + 1;
      long num6 = (long) buffer3[index3] << 24;
      byte[] buffer4 = this.Buffer;
      int index4 = num5;
      int num7 = index4 + 1;
      long num8 = (long) buffer4[index4] << 16;
      long num9 = num6 | num8;
      byte[] buffer5 = this.Buffer;
      int index5 = num7;
      int num10 = index5 + 1;
      long num11 = (long) buffer5[index5] << 8;
      long num12 = num9 | num11;
      byte[] buffer6 = this.Buffer;
      int index6 = num10;
      int num13 = index6 + 1;
      long num14 = (long) buffer6[index6];
      this.FunkId = Util.ConvertBcdInt64ToInt64(num12 | num14);
      this.DeviceType = NumberRanges.GetTypeOfMinolDevice(this.FunkId);
      int num15 = num13 + 1 + 1;
      byte[] buffer7 = this.Buffer;
      int index7 = num15;
      int num16 = index7 + 1;
      byte num17 = buffer7[index7];
      byte[] buffer8 = this.Buffer;
      int index8 = num16;
      int num18 = index8 + 1;
      byte num19 = buffer8[index8];
      int day1 = (int) num19 & 31;
      int month1 = (int) num17 & 15;
      int num20 = ((int) num19 & 224) >> 5 | ((int) num17 & 240) >> 1;
      int year1 = num20 < 80 ? num20 + 2000 : num20 + 1900;
      try
      {
        this.TimePoint = new DateTime(year1, month1, day1, 0, 0, 0);
      }
      catch
      {
        return false;
      }
      this.DueDate = DateTime.MinValue;
      byte[] buffer9 = this.Buffer;
      int index9 = num18;
      int num21 = index9 + 1;
      byte num22 = buffer9[index9];
      byte[] buffer10 = this.Buffer;
      int index10 = num21;
      int num23 = index10 + 1;
      byte num24 = buffer10[index10];
      if (num22 > (byte) 0 && num24 > (byte) 0 && num22 != byte.MaxValue && num24 != byte.MaxValue)
      {
        int day2 = (int) num24 & 31;
        int month2 = (int) num22 & 15;
        int num25 = ((int) num24 & 224) >> 5 | ((int) num22 & 240) >> 1;
        int year2 = num25 < 80 ? num25 + 2000 : num25 + 1900;
        try
        {
          this.DueDate = new DateTime(year2, month2, day2, 0, 0, 0);
        }
        catch
        {
          return false;
        }
      }
      byte[] buffer11 = this.Buffer;
      int index11 = num23;
      int num26 = index11 + 1;
      this.K = (ushort) ((uint) buffer11[index11] << 8);
      int k = (int) this.K;
      byte[] buffer12 = this.Buffer;
      int index12 = num26;
      int num27 = index12 + 1;
      int num28 = (int) buffer12[index12];
      this.K = (ushort) (k | num28);
      this.ScaleFactor = (ushort) ((uint) this.K >> 4);
      this.Unit = new InputUnitsIndex?(MinolDevice.ConvertMinolUnitToInputUnitsIndex((byte) this.K));
      int num29 = num27 + 1 + 1;
      this.IsShortPackage = false;
      if (this.DeviceType == DeviceTypes.EHCA_M6 || this.DeviceType == DeviceTypes.EHCA_M5p || this.DeviceType == DeviceTypes.EHCA_M6_Radio3)
        this.IsShortPackage = true;
      else if (this.DeviceType == DeviceTypes.None)
      {
        if (!crcValue)
          return false;
        this.IsShortPackage = false;
      }
      else
        this.IsShortPackage = false;
      int num30;
      if (this.IsShortPackage)
      {
        byte[] buffer13 = this.Buffer;
        int index13 = num29;
        int num31 = index13 + 1;
        byte byte1_1 = buffer13[index13];
        byte[] buffer14 = this.Buffer;
        int index14 = num31;
        int num32 = index14 + 1;
        byte byte2_1 = buffer14[index14];
        this.DynamicValue = this.GetDecimalValue(byte1_1, byte2_1);
        byte[] buffer15 = this.Buffer;
        int index15 = num32;
        int num33 = index15 + 1;
        byte byte1_2 = buffer15[index15];
        byte[] buffer16 = this.Buffer;
        int index16 = num33;
        int num34 = index16 + 1;
        byte byte2_2 = buffer16[index16];
        this.DueDateValue = this.GetDecimalValue(byte1_2, byte2_2);
        byte[] buffer17 = this.Buffer;
        int index17 = num34;
        int num35 = index17 + 1;
        byte byte1_3 = buffer17[index17];
        byte[] buffer18 = this.Buffer;
        int index18 = num35;
        int num36 = index18 + 1;
        byte byte2_3 = buffer18[index18];
        this.CurrentValue = this.GetDecimalValue(byte1_3, byte2_3);
        num30 = num36 + 1 + 1 + 8;
      }
      else
      {
        if (!crcValue)
          return false;
        byte[] buffer19 = this.Buffer;
        int index19 = num29;
        int num37 = index19 + 1;
        byte byte1_4 = buffer19[index19];
        byte[] buffer20 = this.Buffer;
        int index20 = num37;
        int num38 = index20 + 1;
        byte byte2_4 = buffer20[index20];
        byte[] buffer21 = this.Buffer;
        int index21 = num38;
        int num39 = index21 + 1;
        byte byte3_1 = buffer21[index21];
        byte[] buffer22 = this.Buffer;
        int index22 = num39;
        int num40 = index22 + 1;
        byte byte4_1 = buffer22[index22];
        this.DynamicValue = this.GetDecimalValueFromBCD(byte1_4, byte2_4, byte3_1, byte4_1);
        byte[] buffer23 = this.Buffer;
        int index23 = num40;
        int num41 = index23 + 1;
        byte byte1_5 = buffer23[index23];
        byte[] buffer24 = this.Buffer;
        int index24 = num41;
        int num42 = index24 + 1;
        byte byte2_5 = buffer24[index24];
        int num43 = num42 + 1 + 1;
        byte[] buffer25 = this.Buffer;
        int index25 = num43;
        int num44 = index25 + 1;
        byte byte3_2 = buffer25[index25];
        byte[] buffer26 = this.Buffer;
        int index26 = num44;
        int num45 = index26 + 1;
        byte byte4_2 = buffer26[index26];
        this.DueDateValue = this.GetDecimalValueFromBCD(byte1_5, byte2_5, byte3_2, byte4_2);
        byte[] buffer27 = this.Buffer;
        int index27 = num45;
        int num46 = index27 + 1;
        byte byte1_6 = buffer27[index27];
        byte[] buffer28 = this.Buffer;
        int index28 = num46;
        int num47 = index28 + 1;
        byte byte2_6 = buffer28[index28];
        byte[] buffer29 = this.Buffer;
        int index29 = num47;
        int num48 = index29 + 1;
        byte byte3_3 = buffer29[index29];
        byte[] buffer30 = this.Buffer;
        int index30 = num48;
        int num49 = index30 + 1;
        byte byte4_3 = buffer30[index30];
        this.CurrentValue = this.GetDecimalValueFromBCD(byte1_6, byte2_6, byte3_3, byte4_3);
        num30 = num49 + 1 + 1;
      }
      if ((uint) this.MonthIndex % 2U > 0U)
      {
        int num50 = ((int) this.MonthIndex - 1) / 2;
        DateTime dateTime = this.TimePoint;
        int year3 = dateTime.Year;
        dateTime = this.TimePoint;
        int month3 = dateTime.Month;
        dateTime = new DateTime(year3, month3, 1);
        this.DynamicDate = dateTime.AddMonths(-num50);
      }
      else
      {
        DateTime timePoint = this.TimePoint;
        int year4 = timePoint.Year;
        timePoint = this.TimePoint;
        int month4 = timePoint.Month;
        this.DynamicDate = new DateTime(year4, month4, 16);
        if (this.DynamicDate > this.TimePoint)
        {
          this.DynamicValue = new Decimal?();
          RadioPacketRadio2.logger.Warn("Wrong radio2 packet received! The dynamic half month date is greater as device system date.");
        }
      }
      this.Manufacturer = "MINOL";
      this.Version = "2";
      this.Medium = this.DeviceType.ToString();
      if (hasRssi && packet.Length >= 34)
      {
        byte[] buffer31 = this.Buffer;
        int index31 = num30;
        int num51 = index31 + 1;
        this.RSSI = new byte?(buffer31[index31]);
        byte[] buffer32 = this.Buffer;
        int index32 = num51;
        int startIndex = index32 + 1;
        this.LQI = new byte?(buffer32[index32]);
        if (this.Buffer.Length >= startIndex + 4)
          this.MCT = BitConverter.ToUInt32(this.Buffer, startIndex);
        this.IsCrcOk = true;
        return true;
      }
      this.IsCrcOk = true;
      return true;
    }

    private bool GetCRCValue(int index)
    {
      return ((int) this.Buffer[index] | (int) this.Buffer[index + 1] << 8) == (int) CRC.calculateChecksumReversed(this.Buffer, (uint) index, 0U);
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.FunkId.ToString().PadRight(10));
      stringBuilder.Append(this.RSSI_dBm.ToString().PadRight(5));
      stringBuilder.Append(this.DeviceType.ToString().PadRight(21));
      stringBuilder.Append("IsShortPackage: ").Append(this.IsShortPackage.ToString().PadRight(6));
      stringBuilder.Append("MonthIndex: ").Append(this.MonthIndex.ToString().PadRight(3));
      stringBuilder.Append("Exponent2F: ").Append(this.Exponent2F.ToString().PadRight(5));
      stringBuilder.Append("MeasurementError: ").Append(this.MeasurementError.ToString().PadRight(6));
      stringBuilder.Append("IsSummerTime: ").Append(this.IsSummerTime.ToString().PadRight(6));
      stringBuilder.Append("K: ").Append(this.K.ToString().PadRight(7));
      if (this.CurrentValue.HasValue)
        stringBuilder.Append("CurrentValue: ").Append(this.CurrentValue.ToString().PadRight(9));
      else
        stringBuilder.Append("CurrentValue: ").Append("".PadRight(9));
      stringBuilder.Append("MCT: ").Append(this.MCT.ToString().PadRight(12));
      return stringBuilder.ToString();
    }

    public override SortedList<long, SortedList<DateTime, ReadingValue>> GetValues()
    {
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      ValueIdent.ValueIdPart_MeterType valueIdPartMeterType = this.GetValueIdPart_MeterType();
      DateTime dateTime;
      if (this.IsDeviceError)
      {
        ref SortedList<long, SortedList<DateTime, ReadingValue>> local1 = ref valueList;
        int year = this.TimePoint.Year;
        int month = this.TimePoint.Month;
        dateTime = this.TimePoint;
        int day = dateTime.Day;
        DateTime timePoint = new DateTime(year, month, day);
        long valueIdentOfError = ValueIdent.GetValueIdentOfError(valueIdPartMeterType, ValueIdent.ValueIdentError.DeviceError);
        // ISSUE: variable of a boxed type
        __Boxed<int> local2 = (System.ValueType) 1;
        ValueIdent.AddValueToValueIdentList(ref local1, timePoint, valueIdentOfError, (object) local2);
      }
      if (this.IsManipulated)
      {
        ref SortedList<long, SortedList<DateTime, ReadingValue>> local3 = ref valueList;
        dateTime = this.TimePoint;
        int year = dateTime.Year;
        dateTime = this.TimePoint;
        int month = dateTime.Month;
        dateTime = this.TimePoint;
        int day = dateTime.Day;
        DateTime timePoint = new DateTime(year, month, day);
        long valueIdentOfError = ValueIdent.GetValueIdentOfError(valueIdPartMeterType, ValueIdent.ValueIdentError.Manipulation);
        // ISSUE: variable of a boxed type
        __Boxed<int> local4 = (System.ValueType) 1;
        ValueIdent.AddValueToValueIdentList(ref local3, timePoint, valueIdentOfError, (object) local4);
      }
      if (this.CurrentValue.HasValue)
      {
        if (this.GetValueIdPart_MeterType() != 0)
        {
          if (this.IsHeatCostAllocator)
            ValueIdent.AddValueToValueIdentList(ref valueList, this.TimePoint, this.GetValueIdentOfCurrentValue(true, new InputUnitsIndex?()), (object) (this.CurrentValue.Value * ((Decimal) this.K / 1000M)));
          else if (this.Unit.Value == InputUnitsIndex.ImpUnit_0L)
            ValueIdent.AddValueToValueIdentList(ref valueList, this.TimePoint, this.GetValueIdentOfCurrentValue(true, this.Unit), (object) (this.CurrentValue.Value / 1000M * (Decimal) this.ScaleFactor));
        }
        ValueIdent.AddValueToValueIdentList(ref valueList, this.TimePoint, this.GetValueIdentOfCurrentValue(false, new InputUnitsIndex?()), (object) this.CurrentValue.Value);
      }
      if (this.DueDateValue.HasValue)
      {
        if (this.GetValueIdPart_MeterType() != 0)
        {
          if (this.IsHeatCostAllocator)
            ValueIdent.AddValueToValueIdentList(ref valueList, this.DueDate, this.GetValueIdentOfDueDateValue(true, new InputUnitsIndex?()), (object) (this.DueDateValue.Value * ((Decimal) this.K / 1000M)));
          else if (this.Unit.Value == InputUnitsIndex.ImpUnit_0L)
            ValueIdent.AddValueToValueIdentList(ref valueList, this.DueDate, this.GetValueIdentOfDueDateValue(true, this.Unit), (object) (this.DueDateValue.Value / 1000M * (Decimal) this.ScaleFactor));
        }
        ValueIdent.AddValueToValueIdentList(ref valueList, this.DueDate, this.GetValueIdentOfDueDateValue(false, new InputUnitsIndex?()), (object) this.DueDateValue.Value);
      }
      int num1;
      if (this.DynamicValue.HasValue)
      {
        dateTime = this.DynamicDate;
        num1 = dateTime.Day == 1 ? 1 : 0;
      }
      else
        num1 = 0;
      Decimal? nullable1;
      Decimal? nullable2;
      Decimal num2;
      if (num1 != 0)
      {
        if (this.GetValueIdPart_MeterType() != 0)
        {
          if (this.IsHeatCostAllocator)
          {
            ref SortedList<long, SortedList<DateTime, ReadingValue>> local5 = ref valueList;
            DateTime dynamicDate = this.DynamicDate;
            long identOfMonthValue = this.GetValueIdentOfMonthValue(true, new InputUnitsIndex?());
            nullable1 = this.DynamicValue;
            Decimal num3 = (Decimal) this.K / 1000M;
            // ISSUE: variable of a boxed type
            __Boxed<Decimal?> local6 = (System.ValueType) (nullable1.HasValue ? new Decimal?(nullable1.GetValueOrDefault() * num3) : new Decimal?());
            ValueIdent.AddValueToValueIdentList(ref local5, dynamicDate, identOfMonthValue, (object) local6);
          }
          else if (this.Unit.Value == InputUnitsIndex.ImpUnit_0L)
          {
            ref SortedList<long, SortedList<DateTime, ReadingValue>> local7 = ref valueList;
            DateTime dynamicDate = this.DynamicDate;
            long identOfMonthValue = this.GetValueIdentOfMonthValue(true, this.Unit);
            nullable2 = this.DynamicValue;
            num2 = 1000M;
            nullable1 = nullable2.HasValue ? new Decimal?(nullable2.GetValueOrDefault() / num2) : new Decimal?();
            Decimal scaleFactor = (Decimal) this.ScaleFactor;
            Decimal? nullable3;
            if (!nullable1.HasValue)
            {
              nullable2 = new Decimal?();
              nullable3 = nullable2;
            }
            else
              nullable3 = new Decimal?(nullable1.GetValueOrDefault() * scaleFactor);
            // ISSUE: variable of a boxed type
            __Boxed<Decimal?> local8 = (System.ValueType) nullable3;
            ValueIdent.AddValueToValueIdentList(ref local7, dynamicDate, identOfMonthValue, (object) local8);
          }
        }
        ValueIdent.AddValueToValueIdentList(ref valueList, this.DynamicDate, this.GetValueIdentOfMonthValue(false, new InputUnitsIndex?()), (object) this.DynamicValue);
      }
      nullable1 = this.DynamicValue;
      int num4;
      if (nullable1.HasValue)
      {
        dateTime = this.DynamicDate;
        num4 = dateTime.Day == 16 ? 1 : 0;
      }
      else
        num4 = 0;
      if (num4 != 0)
      {
        if (this.GetValueIdPart_MeterType() != 0)
        {
          if (this.IsHeatCostAllocator)
          {
            ref SortedList<long, SortedList<DateTime, ReadingValue>> local9 = ref valueList;
            DateTime dynamicDate = this.DynamicDate;
            long ofHalfMonthValue = this.GetValueIdentOfHalfMonthValue(true, new InputUnitsIndex?());
            nullable1 = this.DynamicValue;
            Decimal num5 = (Decimal) this.K / 1000M;
            Decimal? nullable4;
            if (!nullable1.HasValue)
            {
              nullable2 = new Decimal?();
              nullable4 = nullable2;
            }
            else
              nullable4 = new Decimal?(nullable1.GetValueOrDefault() * num5);
            // ISSUE: variable of a boxed type
            __Boxed<Decimal?> local10 = (System.ValueType) nullable4;
            ValueIdent.AddValueToValueIdentList(ref local9, dynamicDate, ofHalfMonthValue, (object) local10);
          }
          else if (this.Unit.Value == InputUnitsIndex.ImpUnit_0L)
          {
            ref SortedList<long, SortedList<DateTime, ReadingValue>> local11 = ref valueList;
            DateTime dynamicDate = this.DynamicDate;
            long ofHalfMonthValue = this.GetValueIdentOfHalfMonthValue(true, this.Unit);
            nullable2 = this.DynamicValue;
            num2 = 1000M;
            nullable1 = nullable2.HasValue ? new Decimal?(nullable2.GetValueOrDefault() / num2) : new Decimal?();
            Decimal scaleFactor = (Decimal) this.ScaleFactor;
            Decimal? nullable5;
            if (!nullable1.HasValue)
            {
              nullable2 = new Decimal?();
              nullable5 = nullable2;
            }
            else
              nullable5 = new Decimal?(nullable1.GetValueOrDefault() * scaleFactor);
            // ISSUE: variable of a boxed type
            __Boxed<Decimal?> local12 = (System.ValueType) nullable5;
            ValueIdent.AddValueToValueIdentList(ref local11, dynamicDate, ofHalfMonthValue, (object) local12);
          }
        }
        ValueIdent.AddValueToValueIdentList(ref valueList, this.DynamicDate, this.GetValueIdentOfHalfMonthValue(false, new InputUnitsIndex?()), (object) this.DynamicValue);
      }
      if (this.RSSI_dBm.HasValue)
        ValueIdent.AddValueToValueIdentList(ref valueList, this.TimePoint, this.GetValueIdentOfSignalStrengthValue(), (object) this.RSSI_dBm.Value);
      return valueList;
    }

    internal override SortedList<long, SortedList<DateTime, ReadingValue>> Merge(
      SortedList<long, SortedList<DateTime, ReadingValue>> oldMeterValues)
    {
      SortedList<long, SortedList<DateTime, ReadingValue>> values = this.GetValues();
      if (values == null)
        return oldMeterValues;
      if (oldMeterValues == null)
        return values;
      long identOfCurrentValue1 = this.GetValueIdentOfCurrentValue(false, new InputUnitsIndex?());
      if (oldMeterValues.ContainsKey(identOfCurrentValue1) && values.ContainsKey(identOfCurrentValue1))
        oldMeterValues[identOfCurrentValue1] = values[identOfCurrentValue1];
      else if (!oldMeterValues.ContainsKey(identOfCurrentValue1) && values.ContainsKey(identOfCurrentValue1))
        oldMeterValues.Add(identOfCurrentValue1, values[identOfCurrentValue1]);
      long identOfDueDateValue1 = this.GetValueIdentOfDueDateValue(false, new InputUnitsIndex?());
      if (oldMeterValues.ContainsKey(identOfDueDateValue1) && values.ContainsKey(identOfDueDateValue1))
        oldMeterValues[identOfDueDateValue1] = values[identOfDueDateValue1];
      else if (!oldMeterValues.ContainsKey(identOfDueDateValue1) && values.ContainsKey(identOfDueDateValue1))
        oldMeterValues.Add(identOfDueDateValue1, values[identOfDueDateValue1]);
      long identOfMonthValue1 = this.GetValueIdentOfMonthValue(false, new InputUnitsIndex?());
      RadioDevicePacket.MergeMeterValues(oldMeterValues, values, identOfMonthValue1);
      long ofHalfMonthValue1 = this.GetValueIdentOfHalfMonthValue(false, new InputUnitsIndex?());
      RadioDevicePacket.MergeMeterValues(oldMeterValues, values, ofHalfMonthValue1);
      if (this.GetValueIdPart_MeterType() != 0)
      {
        long identOfCurrentValue2 = this.GetValueIdentOfCurrentValue(true, this.IsHeatCostAllocator ? new InputUnitsIndex?() : this.Unit);
        if (oldMeterValues.ContainsKey(identOfCurrentValue2) && values.ContainsKey(identOfCurrentValue2))
          oldMeterValues[identOfCurrentValue2] = values[identOfCurrentValue2];
        else if (!oldMeterValues.ContainsKey(identOfCurrentValue2) && values.ContainsKey(identOfCurrentValue2))
          oldMeterValues.Add(identOfCurrentValue2, values[identOfCurrentValue2]);
        long identOfDueDateValue2 = this.GetValueIdentOfDueDateValue(true, this.IsHeatCostAllocator ? new InputUnitsIndex?() : this.Unit);
        if (oldMeterValues.ContainsKey(identOfDueDateValue2) && values.ContainsKey(identOfDueDateValue2))
          oldMeterValues[identOfDueDateValue2] = values[identOfDueDateValue2];
        else if (!oldMeterValues.ContainsKey(identOfDueDateValue2) && values.ContainsKey(identOfDueDateValue2))
          oldMeterValues.Add(identOfDueDateValue2, values[identOfDueDateValue2]);
        long identOfMonthValue2 = this.GetValueIdentOfMonthValue(true, this.IsHeatCostAllocator ? new InputUnitsIndex?() : this.Unit);
        RadioDevicePacket.MergeMeterValues(oldMeterValues, values, identOfMonthValue2);
        long ofHalfMonthValue2 = this.GetValueIdentOfHalfMonthValue(true, this.IsHeatCostAllocator ? new InputUnitsIndex?() : this.Unit);
        RadioDevicePacket.MergeMeterValues(oldMeterValues, values, ofHalfMonthValue2);
      }
      long signalStrengthValue = this.GetValueIdentOfSignalStrengthValue();
      if (oldMeterValues.ContainsKey(signalStrengthValue) && values.ContainsKey(signalStrengthValue))
        oldMeterValues[signalStrengthValue] = values[signalStrengthValue];
      else if (!oldMeterValues.ContainsKey(signalStrengthValue) && values.ContainsKey(signalStrengthValue))
        oldMeterValues.Add(signalStrengthValue, values[signalStrengthValue]);
      return oldMeterValues;
    }
  }
}
