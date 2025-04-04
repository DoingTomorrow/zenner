// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.MinoprotectIII_Parameter
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
  public sealed class MinoprotectIII_Parameter
  {
    public const int BUFFER_SIZE = 44;

    public DateTime? CurrentDateTime { get; set; }

    public DateTime? DateOfFirstActivation { get; set; }

    public ushort BatteryVoltageSmokeDetector { get; set; }

    public double BatteryVoltageSmokeDetectorInVoltage
    {
      get => 4.096 * (double) this.BatteryVoltageSmokeDetector / 1024.0;
    }

    public byte[] Buffer { get; set; }

    public ushort RadioTransmitInterval { get; set; }

    public ushort RadioEpsilonValue { get; set; }

    public byte RadioTransmitPower { get; set; }

    public byte RadioFrequencyAdjustment { get; set; }

    public uint NetworkAddressForInterlinkedSmokeAlarms { get; set; }

    public byte RadioLibraryVersionNumber { get; set; }

    public RadioProtocol RadioProtocol { get; set; }

    public byte StatusByte { get; set; }

    public SmokeDetectorEvent CurrentStateOfEvents { get; set; }

    public byte TypeOfTelegram { get; set; }

    public ushort BatteryVoltageRadioPart { get; set; }

    public double BatteryVoltageRadioPartInVoltage
    {
      get => 4.096 * (double) this.BatteryVoltageRadioPart / 1024.0;
    }

    public ushort SmokeChamberPollution { get; set; }

    public PushButtonErrorState PushButtonError { get; set; }

    public HornDriveLevelState HornDriveLevel { get; set; }

    public RemovingDetectionState RemovingDetection { get; set; }

    public ushort NumberSmokeAlarms { get; set; }

    public ushort NumberTestAlarms { get; set; }

    public ObstructionDetectionState ObstructionDetection { get; set; }

    public SurroundingProximityState SurroundingProximity { get; set; }

    public LED_FailureState LED_Failure { get; set; }

    public InterlinkedDeviceStatus StatusOfInterlinkedDevices { get; set; }

    public static MinoprotectIII_Parameter Parse(byte[] buffer)
    {
      if (buffer == null)
        throw new NullReferenceException("Can not parse parameters of smoke detector! The buffer is null.");
      if (buffer.Length != 44)
        throw new ArgumentException("Can not parse parameters of smoke detector! Unknown length of buffer. Length: " + buffer.Length.ToString() + " bytes.");
      MinoprotectIII_Parameter minoprotectIiiParameter = new MinoprotectIII_Parameter();
      minoprotectIiiParameter.Buffer = buffer;
      minoprotectIiiParameter.DateOfFirstActivation = Util.ConvertToDate_MBus_CP16_TypeG(buffer, 0);
      minoprotectIiiParameter.RadioTransmitInterval = BitConverter.ToUInt16(buffer, 2);
      minoprotectIiiParameter.RadioEpsilonValue = BitConverter.ToUInt16(buffer, 4);
      minoprotectIiiParameter.RadioTransmitPower = buffer[6];
      minoprotectIiiParameter.RadioFrequencyAdjustment = buffer[7];
      minoprotectIiiParameter.NetworkAddressForInterlinkedSmokeAlarms = BitConverter.ToUInt32(new byte[4]
      {
        buffer[8],
        buffer[9],
        buffer[10],
        (byte) 0
      }, 0);
      minoprotectIiiParameter.RadioLibraryVersionNumber = buffer[11];
      minoprotectIiiParameter.RadioProtocol = (RadioProtocol) buffer[12];
      minoprotectIiiParameter.StatusByte = buffer[13];
      byte[] buffer1 = new byte[4]
      {
        buffer[16],
        buffer[17],
        buffer[14],
        buffer[15]
      };
      minoprotectIiiParameter.CurrentDateTime = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer1, 0);
      minoprotectIiiParameter.CurrentStateOfEvents = (SmokeDetectorEvent) BitConverter.ToUInt16(buffer, 18);
      minoprotectIiiParameter.TypeOfTelegram = buffer[20];
      minoprotectIiiParameter.BatteryVoltageSmokeDetector = BitConverter.ToUInt16(buffer, 21);
      minoprotectIiiParameter.BatteryVoltageRadioPart = BitConverter.ToUInt16(buffer, 23);
      minoprotectIiiParameter.SmokeChamberPollution = BitConverter.ToUInt16(buffer, 25);
      minoprotectIiiParameter.PushButtonError = (PushButtonErrorState) BitConverter.ToUInt16(buffer, 27);
      minoprotectIiiParameter.HornDriveLevel = (HornDriveLevelState) BitConverter.ToUInt16(buffer, 29);
      minoprotectIiiParameter.RemovingDetection = (RemovingDetectionState) BitConverter.ToUInt16(buffer, 31);
      minoprotectIiiParameter.NumberSmokeAlarms = BitConverter.ToUInt16(buffer, 33);
      minoprotectIiiParameter.NumberTestAlarms = BitConverter.ToUInt16(buffer, 35);
      minoprotectIiiParameter.ObstructionDetection = (ObstructionDetectionState) BitConverter.ToUInt16(buffer, 37);
      minoprotectIiiParameter.SurroundingProximity = (SurroundingProximityState) BitConverter.ToUInt16(buffer, 39);
      minoprotectIiiParameter.LED_Failure = (LED_FailureState) BitConverter.ToUInt16(buffer, 41);
      minoprotectIiiParameter.StatusOfInterlinkedDevices = (InterlinkedDeviceStatus) buffer[43];
      return minoprotectIiiParameter;
    }

    public string ToString(int spaces)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      if (this.DateOfFirstActivation.HasValue)
        stringBuilder1.Append("Date of First Activation: ".PadRight(spaces, ' ')).AppendLine(this.DateOfFirstActivation.Value.ToShortDateString());
      else
        stringBuilder1.Append("Date of First Activation: ".PadRight(spaces, ' ')).AppendLine("Invalid date");
      stringBuilder1.Append("Radio Transmit Interval: ".PadRight(spaces, ' ')).Append(this.RadioTransmitInterval.ToString()).AppendLine(" seconds");
      stringBuilder1.Append("Radio Epsilon Value: ".PadRight(spaces, ' ')).AppendLine(this.RadioEpsilonValue.ToString());
      StringBuilder stringBuilder2 = stringBuilder1.Append("Radio Transmit Power: ".PadRight(spaces, ' '));
      byte num1 = this.RadioTransmitPower;
      string str1 = num1.ToString();
      stringBuilder2.AppendLine(str1);
      StringBuilder stringBuilder3 = stringBuilder1.Append("Radio Frequency Adjustment: ".PadRight(spaces, ' '));
      num1 = this.RadioFrequencyAdjustment;
      string str2 = num1.ToString();
      stringBuilder3.AppendLine(str2);
      stringBuilder1.Append("Network Address for Interlinked Smoke Alarms: ".PadRight(spaces, ' ')).AppendLine(this.NetworkAddressForInterlinkedSmokeAlarms.ToString());
      stringBuilder1.Append("Radio Library Version Number: ".PadRight(spaces, ' ')).Append(this.RadioLibraryVersionNumber.ToString("X2").Insert(1, ".")).Append(" (0x").Append(this.RadioLibraryVersionNumber.ToString("X2")).Append(", ").Append(this.RadioLibraryVersionNumber.ToString()).AppendLine(")");
      StringBuilder stringBuilder4 = stringBuilder1.Append("Radio protocol: ".PadRight(spaces, ' ')).Append(this.RadioProtocol.ToString()).Append(" (0x");
      byte num2 = (byte) this.RadioProtocol;
      string str3 = num2.ToString("X2");
      stringBuilder4.Append(str3).AppendLine(")");
      StringBuilder stringBuilder5 = stringBuilder1.Append("Status Byte: ".PadRight(spaces, ' '));
      num2 = this.StatusByte;
      string str4 = num2.ToString();
      stringBuilder5.AppendLine(str4);
      if (this.CurrentDateTime.HasValue)
        stringBuilder1.Append("Current Date Time: ".PadRight(spaces, ' ')).AppendLine(this.CurrentDateTime.Value.ToString());
      else
        stringBuilder1.Append("Current Date Time: ".PadRight(spaces, ' ')).AppendLine("Invalid date");
      stringBuilder1.Append("Current State of Events: ".PadRight(spaces, ' ')).AppendLine(this.CurrentStateOfEvents.ToString());
      stringBuilder1.Append("Type of Telegram: ".PadRight(spaces, ' ')).AppendLine(this.TypeOfTelegram.ToString());
      StringBuilder stringBuilder6 = stringBuilder1.Append("Battery Voltage Smoke Detector: ".PadRight(spaces, ' '));
      double num3 = this.BatteryVoltageSmokeDetectorInVoltage;
      string str5 = num3.ToString();
      stringBuilder6.Append(str5).Append("V (").Append(this.BatteryVoltageSmokeDetector.ToString()).AppendLine(")");
      StringBuilder stringBuilder7 = stringBuilder1.Append("Battery Voltage Radio Part: ".PadRight(spaces, ' '));
      num3 = this.BatteryVoltageRadioPartInVoltage;
      string str6 = num3.ToString();
      stringBuilder7.Append(str6).Append("V (").Append(this.BatteryVoltageRadioPart.ToString()).AppendLine(")");
      StringBuilder stringBuilder8 = stringBuilder1.Append("Smoke Chamber Pollution: ".PadRight(spaces, ' ')).Append(this.SmokeChamberPollution.ToString()).Append(" (0x");
      ushort num4 = this.SmokeChamberPollution;
      string str7 = num4.ToString("X4");
      stringBuilder8.Append(str7).AppendLine(")");
      StringBuilder stringBuilder9 = stringBuilder1.Append("Push Button Error: ".PadRight(spaces, ' ')).Append(this.PushButtonError.ToString()).Append(" (0x");
      num4 = (ushort) this.PushButtonError;
      string str8 = num4.ToString("X4");
      stringBuilder9.Append(str8).AppendLine(")");
      StringBuilder stringBuilder10 = stringBuilder1.Append("Horn Drive Level: ".PadRight(spaces, ' ')).Append(this.HornDriveLevel.ToString()).Append(" (0x");
      num4 = (ushort) this.HornDriveLevel;
      string str9 = num4.ToString("X4");
      stringBuilder10.Append(str9).AppendLine(")");
      StringBuilder stringBuilder11 = stringBuilder1.Append("Removing Detection: ".PadRight(spaces, ' ')).Append(this.RemovingDetection.ToString()).Append(" (0x");
      num4 = (ushort) this.RemovingDetection;
      string str10 = num4.ToString("X4");
      stringBuilder11.Append(str10).AppendLine(")");
      StringBuilder stringBuilder12 = stringBuilder1.Append("Number Smoke Alarms: ".PadRight(spaces, ' '));
      num4 = this.NumberSmokeAlarms;
      string str11 = num4.ToString();
      stringBuilder12.AppendLine(str11);
      StringBuilder stringBuilder13 = stringBuilder1.Append("Number Test Alarms: ".PadRight(spaces, ' '));
      num4 = this.NumberTestAlarms;
      string str12 = num4.ToString();
      stringBuilder13.AppendLine(str12);
      StringBuilder stringBuilder14 = stringBuilder1.Append("Obstruction Detection: ".PadRight(spaces, ' ')).Append(this.ObstructionDetection.ToString()).Append(" (0x");
      num4 = (ushort) this.ObstructionDetection;
      string str13 = num4.ToString("X4");
      stringBuilder14.Append(str13).AppendLine(")");
      StringBuilder stringBuilder15 = stringBuilder1.Append("Surrounding Proximity: ".PadRight(spaces, ' ')).Append(this.SurroundingProximity.ToString()).Append(" (0x");
      num4 = (ushort) this.SurroundingProximity;
      string str14 = num4.ToString("X4");
      stringBuilder15.Append(str14).AppendLine(")");
      StringBuilder stringBuilder16 = stringBuilder1.Append("LED Failure: ".PadRight(spaces, ' ')).Append(this.LED_Failure.ToString()).Append(" (0x");
      num4 = (ushort) this.LED_Failure;
      string str15 = num4.ToString("X4");
      stringBuilder16.Append(str15).AppendLine(")");
      stringBuilder1.Append("Status of Interlinked Devices: ".PadRight(spaces, ' ')).Append(this.StatusOfInterlinkedDevices.ToString()).Append(" (0x").Append(((byte) this.StatusOfInterlinkedDevices).ToString("X4")).AppendLine(")");
      stringBuilder1.Append("Buffer: ").AppendLine(Util.ByteArrayToHexString(this.Buffer));
      return stringBuilder1.ToString();
    }

    public MinoprotectIII_Parameter DeepCopy()
    {
      return new MinoprotectIII_Parameter()
      {
        CurrentDateTime = this.CurrentDateTime,
        DateOfFirstActivation = this.DateOfFirstActivation,
        BatteryVoltageSmokeDetector = this.BatteryVoltageSmokeDetector,
        Buffer = this.Buffer,
        RadioTransmitInterval = this.RadioTransmitInterval,
        RadioEpsilonValue = this.RadioEpsilonValue,
        RadioTransmitPower = this.RadioTransmitPower,
        RadioFrequencyAdjustment = this.RadioFrequencyAdjustment,
        NetworkAddressForInterlinkedSmokeAlarms = this.NetworkAddressForInterlinkedSmokeAlarms,
        RadioLibraryVersionNumber = this.RadioLibraryVersionNumber,
        RadioProtocol = this.RadioProtocol,
        StatusByte = this.StatusByte,
        CurrentStateOfEvents = this.CurrentStateOfEvents,
        TypeOfTelegram = this.TypeOfTelegram,
        BatteryVoltageRadioPart = this.BatteryVoltageRadioPart,
        SmokeChamberPollution = this.SmokeChamberPollution,
        PushButtonError = this.PushButtonError,
        HornDriveLevel = this.HornDriveLevel,
        RemovingDetection = this.RemovingDetection,
        NumberSmokeAlarms = this.NumberSmokeAlarms,
        NumberTestAlarms = this.NumberTestAlarms,
        ObstructionDetection = this.ObstructionDetection,
        SurroundingProximity = this.SurroundingProximity,
        LED_Failure = this.LED_Failure,
        StatusOfInterlinkedDevices = this.StatusOfInterlinkedDevices
      };
    }

    internal byte[] CreateWriteBuffer()
    {
      DateTime? nullable;
      DateTime dateTime1;
      int num1;
      if (this.DateOfFirstActivation.HasValue)
      {
        nullable = this.DateOfFirstActivation;
        dateTime1 = nullable.Value;
        num1 = dateTime1.Year < 2000 ? 1 : 0;
      }
      else
        num1 = 0;
      if (num1 != 0)
        throw new ArgumentOutOfRangeException("The year of 'DateOfFirstActivation' should be greater or equal as 2000!");
      nullable = this.CurrentDateTime;
      int num2;
      if (nullable.HasValue)
      {
        nullable = this.CurrentDateTime;
        dateTime1 = nullable.Value;
        num2 = dateTime1.Year < 2000 ? 1 : 0;
      }
      else
        num2 = 0;
      if (num2 != 0)
        throw new ArgumentOutOfRangeException("The year of 'CurrentDateTime' should be greater or equal as 2000!");
      byte[] collection1 = new byte[2];
      nullable = this.DateOfFirstActivation;
      if (nullable.HasValue)
      {
        nullable = this.DateOfFirstActivation;
        DateTime dateTime2 = nullable.Value;
        collection1[0] |= (byte) dateTime2.Day;
        collection1[1] |= (byte) dateTime2.Month;
        int num3 = dateTime2.Year - 2000;
        collection1[0] |= (byte) (num3 << 5);
        collection1[1] |= (byte) ((num3 & 120) << 1);
      }
      else
      {
        collection1[0] = byte.MaxValue;
        collection1[1] = byte.MaxValue;
      }
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) collection1);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.RadioTransmitInterval));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.RadioEpsilonValue));
      byteList.Add(this.RadioTransmitPower);
      byteList.Add(this.RadioFrequencyAdjustment);
      byte[] bytes = BitConverter.GetBytes(this.NetworkAddressForInterlinkedSmokeAlarms);
      byteList.Add(bytes[0]);
      byteList.Add(bytes[1]);
      byteList.Add(bytes[2]);
      byteList.Add(this.RadioLibraryVersionNumber);
      byteList.Add((byte) this.RadioProtocol);
      byteList.Add(this.StatusByte);
      byte[] collection2 = new byte[4];
      nullable = this.CurrentDateTime;
      if (nullable.HasValue)
      {
        nullable = this.CurrentDateTime;
        DateTime dateTime3 = nullable.Value;
        collection2[0] |= (byte) dateTime3.Day;
        collection2[1] |= (byte) dateTime3.Month;
        int num4 = dateTime3.Year - 2000;
        collection2[0] |= (byte) (num4 << 5);
        collection2[1] |= (byte) ((num4 & 120) << 1);
        collection2[2] |= (byte) dateTime3.Minute;
        collection2[3] |= (byte) dateTime3.Hour;
      }
      else
      {
        collection2[0] = byte.MaxValue;
        collection2[1] = byte.MaxValue;
        collection2[2] = byte.MaxValue;
        collection2[3] = byte.MaxValue;
      }
      byteList.AddRange((IEnumerable<byte>) collection2);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes((ushort) this.CurrentStateOfEvents));
      byteList.Add(this.TypeOfTelegram);
      return byteList.ToArray();
    }
  }
}
