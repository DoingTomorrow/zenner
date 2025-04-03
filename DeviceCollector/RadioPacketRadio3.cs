// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RadioPacketRadio3
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
  public sealed class RadioPacketRadio3 : RadioDevicePacket
  {
    private static Logger logger = LogManager.GetLogger(nameof (RadioPacketRadio3));
    public const int COUNT_OF_MONTHS = 12;
    public const int HEADER_SIZE = 13;

    public byte LengthOfPacket { get; private set; }

    public byte PacketNr { get; private set; }

    public byte? ScenarioNr { get; private set; }

    public HCA_Scale Scale { get; private set; }

    public bool IsAccuDefect { get; private set; }

    public RadioPacketRadio3.MonthValueCollection Months { get; private set; }

    private RadioPacketRadio3.EncodingType ValueEncodingType { get; set; }

    private RadioPacketRadio3.RadioPacketType PacketType { get; set; }

    public HCA_SensorMode SensorMode { get; private set; }

    public TemperaturUnit TemperaturUnit { get; private set; }

    public DateTime? DueDate { get; private set; }

    public Decimal? DueDateValue { get; private set; }

    public RadioPacketRadio3SmokeDetector SmokeDetector { get; private set; }

    public ushort? K { get; private set; }

    public ushort? ScaleFactor { get; private set; }

    public InputUnitsIndex? Unit { get; private set; }

    public Decimal? CurrentValue { get; private set; }

    public DateTime? ResetDate { get; private set; }

    public DateTime? DeviceErrorDate { get; private set; }

    public DateTime? ManipulationDate { get; private set; }

    public bool IsHeatCostAllocator
    {
      get => this.GetValueIdPart_MeterType() == ValueIdent.ValueIdPart_MeterType.HeatCostAllocator;
    }

    public override bool Parse(byte[] packet, DateTime receivedAt, bool hasRssi)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.Radio3))
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission of radio3!");
      if (packet == null)
        throw new ArgumentNullException("Input parameter 'packet' can not be null!");
      if (this.MyFunctions == null)
        throw new ArgumentNullException("DeviceCollectorHandler can not be null!");
      this.ReceivedAt = receivedAt;
      if (packet.Length > 6 && packet[1] == (byte) 47 && packet[2] == (byte) 47 && packet[3] == (byte) 47 && packet[4] == (byte) 47 && packet[5] == (byte) 47 && packet[6] == (byte) 47)
      {
        this.RadioTestPacket = RadioTestPacket.Parse(packet);
        this.LengthOfPacket = packet[0];
        this.Buffer = packet;
        this.FunkId = (long) this.RadioTestPacket.SerialNumber;
        if (hasRssi)
        {
          this.RSSI = new byte?(this.Buffer[(int) this.LengthOfPacket + 1]);
          this.LQI = new byte?(this.Buffer[(int) this.LengthOfPacket + 2]);
          if (this.Buffer.Length >= (int) this.LengthOfPacket + 3 + 4)
            this.MCT = BitConverter.ToUInt32(this.Buffer, (int) this.LengthOfPacket + 3);
          this.IsCrcOk = ((int) this.LQI.Value & 128) == 128;
          if (!this.IsCrcOk)
            return false;
        }
        return true;
      }
      if (packet.Length < 13 || !this.IsValid(packet))
        return false;
      this.Buffer = packet;
      if (!this.DecodeHeader())
        return false;
      int offset = 0;
      if (this.DeviceType == DeviceTypes.SmokeDetector)
      {
        if (this.MyFunctions.MyBusMode == BusMode.Radio4)
          return true;
        if (!this.ParseSmokeDetector(out offset))
          return false;
      }
      else
      {
        if (!this.DecodeDueDateValue(out offset))
          return false;
        if (this.DeviceType == DeviceTypes.TemperatureSensor || this.DeviceType == DeviceTypes.HumiditySensor)
        {
          if (this.IsDeviceError)
          {
            this.DeviceErrorDate = this.DueDate;
            this.DueDate = new DateTime?();
          }
          else if (this.IsManipulated)
          {
            this.ManipulationDate = this.DueDate;
            this.DueDate = new DateTime?();
          }
        }
        else if (this.IsDeviceError && (uint) this.PacketNr % 2U > 0U)
        {
          this.DeviceErrorDate = this.DueDate;
          this.DueDate = new DateTime?();
          this.DueDateValue = new Decimal?();
        }
        else if (this.IsManipulated && (uint) this.PacketNr % 2U > 0U)
        {
          this.ManipulationDate = this.DueDate;
          this.DueDate = new DateTime?();
          this.DueDateValue = new Decimal?();
        }
        byte? scenarioNr = this.ScenarioNr;
        int? nullable = scenarioNr.HasValue ? new int?((int) scenarioNr.GetValueOrDefault()) : new int?();
        int num1 = 5;
        int num2;
        if (!(nullable.GetValueOrDefault() == num1 & nullable.HasValue))
        {
          scenarioNr = this.ScenarioNr;
          nullable = scenarioNr.HasValue ? new int?((int) scenarioNr.GetValueOrDefault()) : new int?();
          int num3 = 6;
          if (!(nullable.GetValueOrDefault() == num3 & nullable.HasValue) && this.DeviceType != DeviceTypes.TemperatureSensor)
          {
            num2 = this.DeviceType == DeviceTypes.HumiditySensor ? 1 : 0;
            goto label_38;
          }
        }
        num2 = 1;
label_38:
        if (num2 != 0)
        {
          this.CurrentValue = this.DueDateValue;
          this.DueDateValue = new Decimal?();
          if (this.DueDate.HasValue && (this.DeviceType == DeviceTypes.TemperatureSensor || this.DeviceType == DeviceTypes.HumiditySensor))
          {
            this.DueDate = new DateTime?();
            RadioPacketRadio3.logger.Fatal("T&H: Invalid packet detected. DueDate has value: " + Util.ByteArrayToHexString(this.Buffer));
          }
          else
          {
            this.ResetDate = this.DueDate;
            this.DueDate = new DateTime?();
          }
        }
        switch (this.LengthOfPacket)
        {
          case 32:
          case 47:
            if (!this.DecodeScenario1Data(ref offset))
              return false;
            offset = (int) this.LengthOfPacket + 1;
            break;
          case 38:
          case 53:
            if (!this.DecodeWalkByData(ref offset))
              return false;
            break;
          case 44:
          case 54:
            if (!this.DecodeScenario2Data(ref offset))
              return false;
            offset = (int) this.LengthOfPacket + 1;
            break;
          default:
            return false;
        }
      }
      this.Manufacturer = "MINOL";
      this.Version = "3";
      this.Medium = this.DeviceType.ToString();
      if (hasRssi)
      {
        byte[] buffer1 = this.Buffer;
        int index1 = offset;
        int num = index1 + 1;
        this.RSSI = new byte?(buffer1[index1]);
        byte[] buffer2 = this.Buffer;
        int index2 = num;
        int startIndex = index2 + 1;
        this.LQI = new byte?(buffer2[index2]);
        if (this.Buffer.Length >= startIndex + 4)
          this.MCT = BitConverter.ToUInt32(this.Buffer, startIndex);
        this.IsCrcOk = true;
      }
      else
        this.IsCrcOk = true;
      return true;
    }

    private bool DecodeHeader()
    {
      int num1 = 0;
      byte[] buffer1 = this.Buffer;
      int index1 = num1;
      int num2 = index1 + 1;
      this.LengthOfPacket = buffer1[index1];
      switch (this.LengthOfPacket)
      {
        case 32:
        case 38:
        case 44:
        case 47:
        case 53:
        case 54:
          this.PacketType = (RadioPacketRadio3.RadioPacketType) Enum.ToObject(typeof (RadioPacketRadio3.RadioPacketType), this.LengthOfPacket);
          byte[] buffer2 = this.Buffer;
          int index2 = num2;
          int num3 = index2 + 1;
          byte num4 = buffer2[index2];
          this.PacketNr = (byte) ((uint) num4 & 7U);
          this.ValueEncodingType = ((int) num4 & 24) != 0 ? (((int) num4 & 24) != 8 ? RadioPacketRadio3.EncodingType.Unknown : RadioPacketRadio3.EncodingType.BCD_4Byte) : RadioPacketRadio3.EncodingType.Binary_2Byte;
          if (((int) num4 & 224) == 0)
            this.ScenarioNr = new byte?((byte) 1);
          else if (((int) num4 & 224) == 32)
          {
            this.ScenarioNr = new byte?((byte) 2);
          }
          else
          {
            if (((int) num4 & 224) != 64)
              return false;
            this.ScenarioNr = this.MyFunctions.MyBusMode != BusMode.Radio3_868_95_RUSSIA ? new byte?((byte) 6) : new byte?((byte) 5);
          }
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
          byte[] buffer7 = this.Buffer;
          int index7 = num13;
          int num15 = index7 + 1;
          byte num16 = buffer7[index7];
          this.Scale = HCA_Scale.Uniform;
          if (((int) num16 & 4) == 4)
            this.Scale = HCA_Scale.Product;
          this.SensorMode = HCA_SensorMode.Single;
          this.TemperaturUnit = TemperaturUnit.C;
          if (((int) num16 & 8) == 8)
          {
            this.SensorMode = HCA_SensorMode.Double;
            this.TemperaturUnit = TemperaturUnit.F;
          }
          this.IsAccuDefect = ((int) num16 & 32) == 32;
          this.IsManipulated = ((int) num16 & 64) == 64;
          this.IsDeviceError = ((int) num16 & 128) == 128;
          if (this.MyFunctions.MyBusMode == BusMode.Radio4)
            return true;
          int hour = 0;
          int minute = 0;
          byte? scenarioNr = this.ScenarioNr;
          int? nullable1 = scenarioNr.HasValue ? new int?((int) scenarioNr.GetValueOrDefault()) : new int?();
          int num17 = 5;
          int num18;
          if (!(nullable1.GetValueOrDefault() == num17 & nullable1.HasValue))
          {
            scenarioNr = this.ScenarioNr;
            nullable1 = scenarioNr.HasValue ? new int?((int) scenarioNr.GetValueOrDefault()) : new int?();
            int num19 = 6;
            num18 = nullable1.GetValueOrDefault() == num19 & nullable1.HasValue ? 1 : 0;
          }
          else
            num18 = 1;
          int num20;
          if (num18 != 0)
          {
            byte[] buffer8 = this.Buffer;
            int index8 = num15;
            int num21 = index8 + 1;
            this.K = new ushort?((ushort) ((uint) buffer8[index8] << 8));
            ushort? k = this.K;
            nullable1 = k.HasValue ? new int?((int) k.GetValueOrDefault()) : new int?();
            byte[] buffer9 = this.Buffer;
            int index9 = num21;
            num20 = index9 + 1;
            int num22 = (int) buffer9[index9];
            this.K = new ushort?((ushort) (nullable1.HasValue ? new int?(nullable1.GetValueOrDefault() | num22) : new int?()).Value);
            k = this.K;
            int? nullable2;
            if (!k.HasValue)
            {
              nullable1 = new int?();
              nullable2 = nullable1;
            }
            else
              nullable2 = new int?((int) k.GetValueOrDefault() >> 4);
            nullable1 = nullable2;
            this.ScaleFactor = new ushort?((ushort) nullable1.Value);
            k = this.K;
            this.Unit = new InputUnitsIndex?(MinolDevice.ConvertMinolUnitToInputUnitsIndex((byte) k.Value));
          }
          else
          {
            byte[] buffer10 = this.Buffer;
            int index10 = num15;
            int num23 = index10 + 1;
            hour = (int) buffer10[index10];
            byte[] buffer11 = this.Buffer;
            int index11 = num23;
            num20 = index11 + 1;
            minute = (int) buffer11[index11];
          }
          byte[] buffer12 = this.Buffer;
          int index12 = num20;
          int num24 = index12 + 1;
          byte num25 = buffer12[index12];
          byte[] buffer13 = this.Buffer;
          int index13 = num24;
          int num26 = index13 + 1;
          byte num27 = buffer13[index13];
          int day1 = (int) num27 & 31;
          int month1 = (int) num25 & 15;
          int num28 = ((int) num27 & 224) >> 5 | ((int) num25 & 240) >> 1;
          int year1 = num28 < 80 ? num28 + 2000 : num28 + 1900;
          try
          {
            this.TimePoint = new DateTime(year1, month1, day1, hour, minute, 0);
          }
          catch
          {
            return false;
          }
          byte[] buffer14 = this.Buffer;
          int index14 = num26;
          int num29 = index14 + 1;
          byte num30 = buffer14[index14];
          byte[] buffer15 = this.Buffer;
          int index15 = num29;
          int num31 = index15 + 1;
          byte num32 = buffer15[index15];
          if ((num30 != (byte) 0 || num32 != (byte) 0) && (num30 != byte.MaxValue || num32 != byte.MaxValue))
          {
            int day2 = (int) num32 & 31;
            int month2 = (int) num30 & 15;
            int num33 = ((int) num32 & 224) >> 5 | ((int) num30 & 240) >> 1;
            int year2 = num33 < 80 ? num33 + 2000 : num33 + 1900;
            try
            {
              this.DueDate = new DateTime?(new DateTime(year2, month2, day2, 0, 0, 0));
            }
            catch
            {
              return false;
            }
          }
          return num31 == 13;
        default:
          return false;
      }
    }

    private bool DecodeDueDateValue(out int offset)
    {
      offset = 13;
      if (this.ValueEncodingType == RadioPacketRadio3.EncodingType.Binary_2Byte)
      {
        this.DueDateValue = this.GetDecimalValue(this.Buffer[offset], this.Buffer[offset + 1]);
        offset += 2;
      }
      else
      {
        if (this.ValueEncodingType != RadioPacketRadio3.EncodingType.BCD_4Byte)
          return false;
        this.DueDateValue = this.GetDecimalValueFromBCD(this.Buffer[offset], this.Buffer[offset + 1], this.Buffer[offset + 2], this.Buffer[offset + 3]);
        offset += 4;
      }
      return true;
    }

    private bool DecodeWalkByData(ref int offset)
    {
      DateTime timePoint = this.TimePoint;
      int year = timePoint.Year;
      timePoint = this.TimePoint;
      int month = timePoint.Month;
      this.Months = new RadioPacketRadio3.MonthValueCollection(12, new DateTime(year, month, 1));
      if (this.ValueEncodingType == RadioPacketRadio3.EncodingType.Binary_2Byte)
      {
        for (int key = 0; key < 12; ++key)
        {
          this.Months[key].Value = this.GetDecimalValue(this.Buffer[offset], this.Buffer[offset + 1]);
          offset += 2;
        }
      }
      else
      {
        if (this.ValueEncodingType != RadioPacketRadio3.EncodingType.BCD_4Byte)
          return false;
        this.Months[0].Value = this.GetDecimalValueFromBCD(this.Buffer[offset], this.Buffer[offset + 1], this.Buffer[offset + 2], this.Buffer[offset + 3]);
        offset += 4;
        for (int key = 1; key < 12; ++key)
        {
          this.Months[key].Value = this.Convert3ByteBCDDiffValueToMonthValue(this.Months[key - 1].Value, offset);
          offset += 3;
        }
      }
      return true;
    }

    private bool DecodeScenario1Data(ref int offset)
    {
      DateTime timePoint = this.TimePoint;
      int year = timePoint.Year;
      timePoint = this.TimePoint;
      int month = timePoint.Month;
      this.Months = new RadioPacketRadio3.MonthValueCollection(1, new DateTime(year, month, 1));
      if (this.ValueEncodingType == RadioPacketRadio3.EncodingType.Binary_2Byte)
      {
        this.Months[0].Value = this.GetDecimalValue(this.Buffer[offset], this.Buffer[offset + 1]);
        offset += 2;
      }
      else
      {
        if (this.ValueEncodingType != RadioPacketRadio3.EncodingType.BCD_4Byte)
          return false;
        this.Months[0].Value = this.GetDecimalValueFromBCD(this.Buffer[offset], this.Buffer[offset + 1], this.Buffer[offset + 2], this.Buffer[offset + 3]);
        offset += 4;
      }
      return true;
    }

    private bool DecodeScenario2Data(ref int offset)
    {
      DateTime timePoint = this.TimePoint;
      int year = timePoint.Year;
      timePoint = this.TimePoint;
      int month = timePoint.Month;
      this.Months = new RadioPacketRadio3.MonthValueCollection(18, new DateTime(year, month, 1));
      if (this.ValueEncodingType == RadioPacketRadio3.EncodingType.Binary_2Byte)
      {
        this.Months[0].Value = this.GetDecimalValue(this.Buffer[offset], this.Buffer[offset + 1]);
        offset += 2;
        this.Months[(int) this.PacketNr + 1].Value = this.GetDecimalValue(this.Buffer[offset], this.Buffer[offset + 1]);
        offset += 2;
        if (this.PacketNr <= (byte) 5)
          this.Months[(int) this.PacketNr + 9].Value = this.GetDecimalValue(this.Buffer[offset], this.Buffer[offset + 1]);
        else
          this.Months[(int) this.PacketNr + 10].Value = this.GetDecimalValue(this.Buffer[offset], this.Buffer[offset + 1]);
        offset += 2;
      }
      else
      {
        if (this.ValueEncodingType != RadioPacketRadio3.EncodingType.BCD_4Byte)
          return false;
        this.Months[0].Value = this.GetDecimalValueFromBCD(this.Buffer[offset], this.Buffer[offset + 1], this.Buffer[offset + 2], this.Buffer[offset + 3]);
        offset += 4;
      }
      return true;
    }

    private Decimal? Convert3ByteBCDDiffValueToMonthValue(Decimal? valueOfPreviousMonth, int offset)
    {
      if (!valueOfPreviousMonth.HasValue)
        return new Decimal?();
      Decimal? decimalValueFromBcd = this.GetDecimalValueFromBCD(this.Buffer[offset], this.Buffer[offset + 1], this.Buffer[offset + 2]);
      if (!decimalValueFromBcd.HasValue)
        return new Decimal?();
      if (decimalValueFromBcd.Value >= 999000M)
      {
        RadioPacketRadio3.logger.Info("Rücklauf entdekt!");
        Decimal? nullable1 = valueOfPreviousMonth;
        Decimal num1 = decimalValueFromBcd.Value;
        Decimal? nullable2 = nullable1.HasValue ? new Decimal?(nullable1.GetValueOrDefault() - num1) : new Decimal?();
        Decimal num2 = (Decimal) 1000000;
        Decimal? monthValue;
        if (!nullable2.HasValue)
        {
          nullable1 = new Decimal?();
          monthValue = nullable1;
        }
        else
          monthValue = new Decimal?(nullable2.GetValueOrDefault() + num2);
        return monthValue;
      }
      Decimal? nullable3 = valueOfPreviousMonth;
      Decimal? nullable4 = decimalValueFromBcd;
      return nullable3.HasValue & nullable4.HasValue ? new Decimal?(nullable3.GetValueOrDefault() - nullable4.GetValueOrDefault()) : new Decimal?();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.FunkId.ToString().PadRight(10));
      stringBuilder.Append(this.RSSI_dBm.ToString().PadRight(5));
      stringBuilder.Append(this.DeviceType.ToString().PadRight(21));
      stringBuilder.Append("Packet: ").Append(this.PacketNr.ToString().PadRight(2));
      stringBuilder.Append("Length: ").Append(this.LengthOfPacket.ToString().PadRight(3));
      stringBuilder.Append("Type: ").Append(this.PacketType.ToString().PadRight(14));
      if (this.ScenarioNr.HasValue)
        stringBuilder.Append("Scenario: ").Append(this.ScenarioNr.ToString().PadRight(2));
      else
        stringBuilder.Append("Scenario: ").Append("".ToString().PadRight(2));
      stringBuilder.Append("HCA_Scale: ").Append(this.Scale.ToString().PadRight(9));
      stringBuilder.Append("IsAccuDefect: ").Append(this.IsAccuDefect.ToString().PadRight(6));
      stringBuilder.Append("ValueEncodingType: ").Append(this.ValueEncodingType.ToString().PadRight(14));
      stringBuilder.Append("MCT: ").Append(this.MCT.ToString().PadRight(12));
      return stringBuilder.ToString();
    }

    public override SortedList<long, SortedList<DateTime, ReadingValue>> GetValues()
    {
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      ValueIdent.ValueIdPart_MeterType valueIdPartMeterType = this.GetValueIdPart_MeterType();
      DateTime timePoint1;
      DateTime? nullable1;
      if (this.IsDeviceError)
      {
        DateTime dateTime;
        if (!this.DeviceErrorDate.HasValue)
        {
          int year = this.TimePoint.Year;
          timePoint1 = this.TimePoint;
          int month = timePoint1.Month;
          timePoint1 = this.TimePoint;
          int day = timePoint1.Day;
          dateTime = new DateTime(year, month, day);
        }
        else
        {
          nullable1 = this.DeviceErrorDate;
          dateTime = nullable1.Value;
        }
        DateTime timePoint2 = dateTime;
        long valueIdentOfError = ValueIdent.GetValueIdentOfError(valueIdPartMeterType, ValueIdent.ValueIdentError.DeviceError);
        ValueIdent.AddValueToValueIdentList(ref valueList, timePoint2, valueIdentOfError, (object) 1);
      }
      if (this.IsManipulated)
      {
        nullable1 = this.ManipulationDate;
        DateTime dateTime;
        if (!nullable1.HasValue)
        {
          timePoint1 = this.TimePoint;
          int year = timePoint1.Year;
          timePoint1 = this.TimePoint;
          int month = timePoint1.Month;
          timePoint1 = this.TimePoint;
          int day = timePoint1.Day;
          dateTime = new DateTime(year, month, day);
        }
        else
        {
          nullable1 = this.ManipulationDate;
          dateTime = nullable1.Value;
        }
        DateTime timePoint3 = dateTime;
        long valueIdentOfError = ValueIdent.GetValueIdentOfError(valueIdPartMeterType, ValueIdent.ValueIdentError.Manipulation);
        ValueIdent.AddValueToValueIdentList(ref valueList, timePoint3, valueIdentOfError, (object) 1);
      }
      if (this.DeviceType == DeviceTypes.SmokeDetector)
      {
        if (this.SmokeDetector == null)
          return valueList;
        long identCurrentState = this.GetValueIdentCurrentState();
        ref SortedList<long, SortedList<DateTime, ReadingValue>> local1 = ref valueList;
        timePoint1 = this.TimePoint;
        int year1 = timePoint1.Year;
        timePoint1 = this.TimePoint;
        int month1 = timePoint1.Month;
        timePoint1 = this.TimePoint;
        int day1 = timePoint1.Day;
        DateTime timePoint4 = new DateTime(year1, month1, day1);
        long valueIdent = identCurrentState;
        // ISSUE: variable of a boxed type
        __Boxed<ushort> local2 = (System.ValueType) (ushort) this.SmokeDetector.DailyEvents[1].Value;
        ValueIdent.AddValueToValueIdentList(ref local1, timePoint4, valueIdent, (object) local2);
        long identMonthlyState = this.GetValueIdentMonthlyState();
        SmokeDetectorEvent? nullable2;
        for (int index = 1; index < this.SmokeDetector.MonthlyEvents.Length; ++index)
        {
          if (this.SmokeDetector.MonthlyEvents[index].HasValue)
          {
            nullable2 = this.SmokeDetector.MonthlyEvents[index];
            SmokeDetectorEvent smokeDetectorEvent = ~(SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionForewarning | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined);
            if (!(nullable2.GetValueOrDefault() == smokeDetectorEvent & nullable2.HasValue))
            {
              SmokeDetectorEvent? monthlyEvent = this.SmokeDetector.MonthlyEvents[index];
              DateTime timePoint5;
              ref DateTime local3 = ref timePoint5;
              timePoint1 = this.TimePoint;
              int year2 = timePoint1.Year;
              timePoint1 = this.TimePoint;
              int month2 = timePoint1.Month;
              local3 = new DateTime(year2, month2, 1);
              int num = index - 1;
              timePoint5 = timePoint5.AddMonths(-num);
              ValueIdent.AddValueToValueIdentList(ref valueList, timePoint5, identMonthlyState, (object) (ushort) monthlyEvent.Value);
            }
          }
        }
        long valueIdentDailyState = this.GetValueIdentDailyState();
        for (int index = 1; index < this.SmokeDetector.DailyEvents.Length; ++index)
        {
          if (this.SmokeDetector.DailyEvents[index].HasValue)
          {
            nullable2 = this.SmokeDetector.DailyEvents[index];
            SmokeDetectorEvent smokeDetectorEvent = ~(SmokeDetectorEvent.BatteryForewarning | SmokeDetectorEvent.BatteryFault | SmokeDetectorEvent.BatteryWarningRadio | SmokeDetectorEvent.SmokeChamberPollutionForewarning | SmokeDetectorEvent.SmokeChamberPollutionWarning | SmokeDetectorEvent.PushButtonFailure | SmokeDetectorEvent.HornFailure | SmokeDetectorEvent.RemovingDetection | SmokeDetectorEvent.TestAlarmReleased | SmokeDetectorEvent.SmokeAlarmReleased | SmokeDetectorEvent.IngressAperturesObstructionDetected | SmokeDetectorEvent.ObjectInSurroundingAreaDetected | SmokeDetectorEvent.LED_Failure | SmokeDetectorEvent.Bit13_undefined | SmokeDetectorEvent.Bit14_undefined | SmokeDetectorEvent.Bit15_undefined);
            if (!(nullable2.GetValueOrDefault() == smokeDetectorEvent & nullable2.HasValue))
            {
              SmokeDetectorEvent? dailyEvent = this.SmokeDetector.DailyEvents[index];
              DateTime timePoint6;
              ref DateTime local4 = ref timePoint6;
              timePoint1 = this.TimePoint;
              int year3 = timePoint1.Year;
              timePoint1 = this.TimePoint;
              int month3 = timePoint1.Month;
              timePoint1 = this.TimePoint;
              int day2 = timePoint1.Day;
              local4 = new DateTime(year3, month3, day2);
              int num = index - 1;
              timePoint6 = timePoint6.AddDays((double) -num);
              ValueIdent.AddValueToValueIdentList(ref valueList, timePoint6, valueIdentDailyState, (object) (ushort) dailyEvent.Value);
            }
          }
        }
      }
      else
      {
        int num1;
        if (this.DueDateValue.HasValue)
        {
          nullable1 = this.DueDate;
          num1 = nullable1.HasValue ? 1 : 0;
        }
        else
          num1 = 0;
        ushort? nullable3;
        if (num1 != 0)
        {
          if (this.GetValueIdPart_MeterType() != 0 && this.ScaleFactor.HasValue)
          {
            if (this.IsHeatCostAllocator)
            {
              ref SortedList<long, SortedList<DateTime, ReadingValue>> local5 = ref valueList;
              nullable1 = this.DueDate;
              DateTime timePoint7 = nullable1.Value;
              long identOfDueDateValue = this.GetValueIdentOfDueDateValue(true, new InputUnitsIndex?());
              Decimal num2 = this.DueDateValue.Value;
              nullable3 = this.K;
              Decimal num3 = (Decimal) nullable3.Value / 1000M;
              // ISSUE: variable of a boxed type
              __Boxed<Decimal> local6 = (System.ValueType) (num2 * num3);
              ValueIdent.AddValueToValueIdentList(ref local5, timePoint7, identOfDueDateValue, (object) local6);
            }
            else if (this.Unit.Value == InputUnitsIndex.ImpUnit_0L)
            {
              ref SortedList<long, SortedList<DateTime, ReadingValue>> local7 = ref valueList;
              nullable1 = this.DueDate;
              DateTime timePoint8 = nullable1.Value;
              long identOfDueDateValue = this.GetValueIdentOfDueDateValue(true, this.Unit);
              // ISSUE: variable of a boxed type
              __Boxed<Decimal> local8 = (System.ValueType) (this.DueDateValue.Value / 1000M * (Decimal) this.ScaleFactor.Value);
              ValueIdent.AddValueToValueIdentList(ref local7, timePoint8, identOfDueDateValue, (object) local8);
            }
          }
          ref SortedList<long, SortedList<DateTime, ReadingValue>> local9 = ref valueList;
          nullable1 = this.DueDate;
          DateTime timePoint9 = nullable1.Value;
          long identOfDueDateValue1 = this.GetValueIdentOfDueDateValue(false, new InputUnitsIndex?());
          // ISSUE: variable of a boxed type
          __Boxed<Decimal> local10 = (System.ValueType) this.DueDateValue.Value;
          ValueIdent.AddValueToValueIdentList(ref local9, timePoint9, identOfDueDateValue1, (object) local10);
        }
        if (this.CurrentValue.HasValue)
        {
          if (this.GetValueIdPart_MeterType() != 0)
          {
            nullable3 = this.ScaleFactor;
            if (nullable3.HasValue)
            {
              if (this.IsHeatCostAllocator)
              {
                ref SortedList<long, SortedList<DateTime, ReadingValue>> local11 = ref valueList;
                DateTime timePoint10 = this.TimePoint;
                long identOfCurrentValue = this.GetValueIdentOfCurrentValue(true, new InputUnitsIndex?());
                Decimal num4 = this.CurrentValue.Value;
                nullable3 = this.K;
                Decimal num5 = (Decimal) nullable3.Value / 1000M;
                // ISSUE: variable of a boxed type
                __Boxed<Decimal> local12 = (System.ValueType) (num4 * num5);
                ValueIdent.AddValueToValueIdentList(ref local11, timePoint10, identOfCurrentValue, (object) local12);
              }
              else if (this.Unit.Value == InputUnitsIndex.ImpUnit_0L)
              {
                ref SortedList<long, SortedList<DateTime, ReadingValue>> local13 = ref valueList;
                DateTime timePoint11 = this.TimePoint;
                long identOfCurrentValue = this.GetValueIdentOfCurrentValue(true, this.Unit);
                Decimal num6 = this.CurrentValue.Value / 1000M;
                nullable3 = this.ScaleFactor;
                Decimal num7 = (Decimal) nullable3.Value;
                // ISSUE: variable of a boxed type
                __Boxed<Decimal> local14 = (System.ValueType) (num6 * num7);
                ValueIdent.AddValueToValueIdentList(ref local13, timePoint11, identOfCurrentValue, (object) local14);
              }
            }
          }
          ValueIdent.AddValueToValueIdentList(ref valueList, this.TimePoint, this.GetValueIdentOfCurrentValue(false, new InputUnitsIndex?()), (object) this.CurrentValue.Value);
        }
        nullable1 = this.ResetDate;
        if (nullable1.HasValue)
        {
          long identOfResetDate = this.GetValueIdentOfResetDate();
          ref SortedList<long, SortedList<DateTime, ReadingValue>> local = ref valueList;
          nullable1 = this.ResetDate;
          DateTime timePoint12 = nullable1.Value;
          long valueIdent = identOfResetDate;
          nullable1 = this.ResetDate;
          timePoint1 = nullable1.Value;
          // ISSUE: variable of a boxed type
          __Boxed<double> oaDate = (System.ValueType) timePoint1.ToOADate();
          ValueIdent.AddValueToValueIdentList(ref local, timePoint12, valueIdent, (object) oaDate);
        }
        if (this.Months != null && this.Months.Count > 0)
        {
          for (int key = 0; key < this.Months.Count; ++key)
          {
            Decimal? nullable4 = this.Months[key].Value;
            if (nullable4.HasValue)
            {
              if (this.GetValueIdPart_MeterType() != 0)
              {
                nullable3 = this.ScaleFactor;
                Decimal? nullable5;
                if (nullable3.HasValue)
                {
                  if (this.IsHeatCostAllocator)
                  {
                    ref SortedList<long, SortedList<DateTime, ReadingValue>> local15 = ref valueList;
                    DateTime timePoint13 = this.Months[key].TimePoint;
                    long identOfMonthValue = this.GetValueIdentOfMonthValue(true, new InputUnitsIndex?());
                    nullable4 = this.Months[key].Value;
                    nullable3 = this.K;
                    Decimal num8 = (Decimal) nullable3.Value / 1000M;
                    Decimal? nullable6;
                    if (!nullable4.HasValue)
                    {
                      nullable5 = new Decimal?();
                      nullable6 = nullable5;
                    }
                    else
                      nullable6 = new Decimal?(nullable4.GetValueOrDefault() * num8);
                    // ISSUE: variable of a boxed type
                    __Boxed<Decimal?> local16 = (System.ValueType) nullable6;
                    ValueIdent.AddValueToValueIdentList(ref local15, timePoint13, identOfMonthValue, (object) local16);
                  }
                  else if (this.Unit.Value == InputUnitsIndex.ImpUnit_0L)
                  {
                    ref SortedList<long, SortedList<DateTime, ReadingValue>> local17 = ref valueList;
                    DateTime timePoint14 = this.Months[key].TimePoint;
                    long identOfMonthValue = this.GetValueIdentOfMonthValue(true, this.Unit);
                    nullable5 = this.Months[key].Value;
                    Decimal num9 = 1000M;
                    nullable4 = nullable5.HasValue ? new Decimal?(nullable5.GetValueOrDefault() / num9) : new Decimal?();
                    nullable3 = this.ScaleFactor;
                    Decimal num10 = (Decimal) nullable3.Value;
                    Decimal? nullable7;
                    if (!nullable4.HasValue)
                    {
                      nullable5 = new Decimal?();
                      nullable7 = nullable5;
                    }
                    else
                      nullable7 = new Decimal?(nullable4.GetValueOrDefault() * num10);
                    // ISSUE: variable of a boxed type
                    __Boxed<Decimal?> local18 = (System.ValueType) nullable7;
                    ValueIdent.AddValueToValueIdentList(ref local17, timePoint14, identOfMonthValue, (object) local18);
                  }
                }
              }
              ValueIdent.AddValueToValueIdentList(ref valueList, this.Months[key].TimePoint, this.GetValueIdentOfMonthValue(false, new InputUnitsIndex?()), (object) this.Months[key].Value);
            }
          }
        }
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
      if (this.DeviceType == DeviceTypes.SmokeDetector)
      {
        long valueIdentOfError1 = ValueIdent.GetValueIdentOfError(this.GetValueIdPart_MeterType(), ValueIdent.ValueIdentError.DeviceError);
        RadioDevicePacket.MergeMeterValues(oldMeterValues, values, valueIdentOfError1);
        long valueIdentOfError2 = ValueIdent.GetValueIdentOfError(this.GetValueIdPart_MeterType(), ValueIdent.ValueIdentError.Manipulation);
        RadioDevicePacket.MergeMeterValues(oldMeterValues, values, valueIdentOfError2);
        long identCurrentState = this.GetValueIdentCurrentState();
        RadioDevicePacket.MergeMeterValues(oldMeterValues, values, identCurrentState);
        long identMonthlyState = this.GetValueIdentMonthlyState();
        RadioDevicePacket.MergeMeterValues(oldMeterValues, values, identMonthlyState);
        long valueIdentDailyState = this.GetValueIdentDailyState();
        RadioDevicePacket.MergeMeterValues(oldMeterValues, values, valueIdentDailyState);
      }
      else
      {
        long valueIdentOfError3 = ValueIdent.GetValueIdentOfError(this.GetValueIdPart_MeterType(), ValueIdent.ValueIdentError.DeviceError);
        if (oldMeterValues.ContainsKey(valueIdentOfError3) && values.ContainsKey(valueIdentOfError3))
          oldMeterValues[valueIdentOfError3] = values[valueIdentOfError3];
        else if (!oldMeterValues.ContainsKey(valueIdentOfError3) && values.ContainsKey(valueIdentOfError3))
          oldMeterValues.Add(valueIdentOfError3, values[valueIdentOfError3]);
        long valueIdentOfError4 = ValueIdent.GetValueIdentOfError(this.GetValueIdPart_MeterType(), ValueIdent.ValueIdentError.Manipulation);
        if (oldMeterValues.ContainsKey(valueIdentOfError4) && values.ContainsKey(valueIdentOfError4))
          oldMeterValues[valueIdentOfError4] = values[valueIdentOfError4];
        else if (!oldMeterValues.ContainsKey(valueIdentOfError4) && values.ContainsKey(valueIdentOfError4))
          oldMeterValues.Add(valueIdentOfError4, values[valueIdentOfError4]);
        long identOfDueDateValue1 = this.GetValueIdentOfDueDateValue(false, new InputUnitsIndex?());
        if (oldMeterValues.ContainsKey(identOfDueDateValue1) && values.ContainsKey(identOfDueDateValue1))
          oldMeterValues[identOfDueDateValue1] = values[identOfDueDateValue1];
        else if (!oldMeterValues.ContainsKey(identOfDueDateValue1) && values.ContainsKey(identOfDueDateValue1))
          oldMeterValues.Add(identOfDueDateValue1, values[identOfDueDateValue1]);
        long identOfCurrentValue1 = this.GetValueIdentOfCurrentValue(false, new InputUnitsIndex?());
        if (oldMeterValues.ContainsKey(identOfCurrentValue1) && values.ContainsKey(identOfCurrentValue1))
          oldMeterValues[identOfCurrentValue1] = values[identOfCurrentValue1];
        else if (!oldMeterValues.ContainsKey(identOfCurrentValue1) && values.ContainsKey(identOfCurrentValue1))
          oldMeterValues.Add(identOfCurrentValue1, values[identOfCurrentValue1]);
        long identOfResetDate = this.GetValueIdentOfResetDate();
        if (oldMeterValues.ContainsKey(identOfResetDate) && values.ContainsKey(identOfResetDate))
          oldMeterValues[identOfResetDate] = values[identOfResetDate];
        else if (!oldMeterValues.ContainsKey(identOfResetDate) && values.ContainsKey(identOfResetDate))
          oldMeterValues.Add(identOfResetDate, values[identOfResetDate]);
        long identOfMonthValue1 = this.GetValueIdentOfMonthValue(false, new InputUnitsIndex?());
        RadioDevicePacket.MergeMeterValues(oldMeterValues, values, identOfMonthValue1);
        if (this.GetValueIdPart_MeterType() != 0)
        {
          long identOfDueDateValue2 = this.GetValueIdentOfDueDateValue(true, this.IsHeatCostAllocator ? new InputUnitsIndex?() : this.Unit);
          if (oldMeterValues.ContainsKey(identOfDueDateValue2) && values.ContainsKey(identOfDueDateValue2))
            oldMeterValues[identOfDueDateValue2] = values[identOfDueDateValue2];
          else if (!oldMeterValues.ContainsKey(identOfDueDateValue2) && values.ContainsKey(identOfDueDateValue2))
            oldMeterValues.Add(identOfDueDateValue2, values[identOfDueDateValue2]);
          long identOfCurrentValue2 = this.GetValueIdentOfCurrentValue(true, this.IsHeatCostAllocator ? new InputUnitsIndex?() : this.Unit);
          if (oldMeterValues.ContainsKey(identOfCurrentValue2) && values.ContainsKey(identOfCurrentValue2))
            oldMeterValues[identOfCurrentValue2] = values[identOfCurrentValue2];
          else if (!oldMeterValues.ContainsKey(identOfCurrentValue2) && values.ContainsKey(identOfCurrentValue2))
            oldMeterValues.Add(identOfCurrentValue2, values[identOfCurrentValue2]);
          long identOfMonthValue2 = this.GetValueIdentOfMonthValue(true, this.IsHeatCostAllocator ? new InputUnitsIndex?() : this.Unit);
          RadioDevicePacket.MergeMeterValues(oldMeterValues, values, identOfMonthValue2);
        }
      }
      long signalStrengthValue = this.GetValueIdentOfSignalStrengthValue();
      if (oldMeterValues.ContainsKey(signalStrengthValue) && values.ContainsKey(signalStrengthValue))
        oldMeterValues[signalStrengthValue] = values[signalStrengthValue];
      else if (!oldMeterValues.ContainsKey(signalStrengthValue) && values.ContainsKey(signalStrengthValue))
        oldMeterValues.Add(signalStrengthValue, values[signalStrengthValue]);
      return oldMeterValues;
    }

    private bool ParseSmokeDetector(out int offset)
    {
      offset = 13;
      this.SmokeDetector = new RadioPacketRadio3SmokeDetector();
      this.SmokeDetector.DateOfFirstActivation = this.DueDate;
      this.DueDate = new DateTime?(DateTime.MinValue);
      this.SmokeDetector.DailyEvents[1] = new SmokeDetectorEvent?((SmokeDetectorEvent) BitConverter.ToUInt16(new byte[2]
      {
        this.Buffer[offset + 1],
        this.Buffer[offset]
      }, 0));
      offset += 2;
      this.SmokeDetector.MonthlyEvents[1] = new SmokeDetectorEvent?((SmokeDetectorEvent) BitConverter.ToUInt16(new byte[2]
      {
        this.Buffer[offset + 1],
        this.Buffer[offset]
      }, 0));
      offset += 2;
      this.SmokeDetector.MonthlyEvents[(int) this.PacketNr + 2] = new SmokeDetectorEvent?((SmokeDetectorEvent) BitConverter.ToUInt16(new byte[2]
      {
        this.Buffer[offset + 1],
        this.Buffer[offset]
      }, 0));
      offset += 2;
      this.SmokeDetector.MonthlyEvents[this.PacketNr <= (byte) 5 ? (int) this.PacketNr + 10 : (int) this.PacketNr + 11] = new SmokeDetectorEvent?((SmokeDetectorEvent) BitConverter.ToUInt16(new byte[2]
      {
        this.Buffer[offset + 1],
        this.Buffer[offset]
      }, 0));
      offset += 2;
      this.SmokeDetector.DailyEvents[this.PacketNr <= (byte) 3 ? (int) this.PacketNr + 1 : (int) this.PacketNr - 3] = new SmokeDetectorEvent?((SmokeDetectorEvent) BitConverter.ToUInt16(new byte[2]
      {
        this.Buffer[offset + 1],
        this.Buffer[offset]
      }, 0));
      offset += 2;
      this.SmokeDetector.DailyEvents[this.PacketNr <= (byte) 3 ? (int) this.PacketNr + 5 : (int) this.PacketNr + 1] = new SmokeDetectorEvent?((SmokeDetectorEvent) BitConverter.ToUInt16(new byte[2]
      {
        this.Buffer[offset + 1],
        this.Buffer[offset]
      }, 0));
      offset += 2;
      this.SmokeDetector.DailyEvents[this.PacketNr <= (byte) 3 ? (int) this.PacketNr + 9 : (int) this.PacketNr + 5] = new SmokeDetectorEvent?((SmokeDetectorEvent) BitConverter.ToUInt16(new byte[2]
      {
        this.Buffer[offset + 1],
        this.Buffer[offset]
      }, 0));
      offset += 2;
      this.SmokeDetector.DailyEvents[this.PacketNr <= (byte) 3 ? (int) this.PacketNr + 13 : (int) this.PacketNr + 9] = new SmokeDetectorEvent?((SmokeDetectorEvent) BitConverter.ToUInt16(new byte[2]
      {
        this.Buffer[offset + 1],
        this.Buffer[offset]
      }, 0));
      offset += 2;
      this.SmokeDetector.DailyEvents[(int) this.PacketNr + 17] = new SmokeDetectorEvent?((SmokeDetectorEvent) BitConverter.ToUInt16(new byte[2]
      {
        this.Buffer[offset + 1],
        this.Buffer[offset]
      }, 0));
      offset += 2;
      this.SmokeDetector.DailyEvents[(int) this.PacketNr + 25] = new SmokeDetectorEvent?((SmokeDetectorEvent) BitConverter.ToUInt16(new byte[2]
      {
        this.Buffer[offset + 1],
        this.Buffer[offset]
      }, 0));
      offset += 2;
      return true;
    }

    private long GetValueIdentCurrentState()
    {
      return ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.StatusNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.BitCompression, (object) ValueIdent.ValueIdPart_Index.Any);
    }

    private long GetValueIdentMonthlyState()
    {
      return ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.StatusNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.Month, ValueIdent.ValueIdPart_Creation.BitCompression, (object) ValueIdent.ValueIdPart_Index.Any);
    }

    private long GetValueIdentDailyState()
    {
      return ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.StatusNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.Day, ValueIdent.ValueIdPart_Creation.BitCompression, (object) ValueIdent.ValueIdPart_Index.Any);
    }

    private long GetValueIdentOfResetDate()
    {
      return ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.DateTime, this.GetValueIdPart_MeterType(), ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.DueDate, ValueIdent.ValueIdPart_StorageInterval.DueDate, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any);
    }

    private bool IsValid(byte[] packet)
    {
      if ((int) packet[0] != (int) packet[6] || (int) packet[1] != (int) packet[7] || (int) packet[2] != (int) packet[8] || (int) packet[3] != (int) packet[9] || (int) packet[4] != (int) packet[10] || (int) packet[5] != (int) packet[11])
        return true;
      RadioPacketRadio3.logger.Fatal("INVALID PACKET: " + Util.ByteArrayToHexString(packet));
      return false;
    }

    public enum EncodingType
    {
      Unknown,
      Binary_2Byte,
      BCD_4Byte,
    }

    private enum RadioPacketType : byte
    {
      Scenario1Short = 32, // 0x20
      WalkByShort = 38, // 0x26
      Scenario2Short = 44, // 0x2C
      Scenario1Long = 47, // 0x2F
      WalkByLong = 53, // 0x35
      Scenario2Long = 54, // 0x36
    }

    public sealed class MonthValue
    {
      public DateTime TimePoint { get; set; }

      public Decimal? Value { get; set; }
    }

    public sealed class MonthValueCollection : Dictionary<int, RadioPacketRadio3.MonthValue>
    {
      public MonthValueCollection(int count, DateTime dateOfCurrentMonth)
      {
        for (int key = 0; key < count; ++key)
          this.Add(key, new RadioPacketRadio3.MonthValue()
          {
            TimePoint = dateOfCurrentMonth.AddMonths(-key)
          });
      }
    }
  }
}
