// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.MinoprotectII_Parameter
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using System;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace SmokeDetectorHandler
{
  public sealed class MinoprotectII_Parameter
  {
    public DateTime? CurrentDateTime { get; set; }

    public DateTime? DateOfFirstActivation { get; set; }

    public ushort BatteryVoltageSmokeDetector { get; set; }

    public byte[] Buffer { get; set; }

    public DateTime? DateOfCalibration { get; set; }

    public uint OperatingHours { get; set; }

    public ushort ReflectionThreshold { get; set; }

    public ushort SmokeThreshold { get; set; }

    public ushort TemperatureThreshold { get; set; }

    public uint IdentificationRemoteAlarmGroup { get; set; }

    public uint IdentificationRemoteAlarmDevice { get; set; }

    public ushort SmokeLevel { get; set; }

    public ushort IR_Current { get; set; }

    internal static MinoprotectII_Parameter Parse(byte[] buffer)
    {
      if (buffer == null)
        throw new NullReferenceException("Can not parse parameters of smoke detector! The buffer is null.");
      return buffer.Length == 29 ? new MinoprotectII_Parameter()
      {
        Buffer = buffer,
        CurrentDateTime = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, 0),
        DateOfCalibration = Util.ConvertToDate_MBus_CP16_TypeG(buffer, 4),
        DateOfFirstActivation = Util.ConvertToDate_MBus_CP16_TypeG(buffer, 6),
        OperatingHours = BitConverter.ToUInt32(new byte[4]
        {
          buffer[8],
          buffer[9],
          buffer[10],
          (byte) 0
        }, 0),
        ReflectionThreshold = BitConverter.ToUInt16(buffer, 11),
        SmokeThreshold = BitConverter.ToUInt16(buffer, 13),
        TemperatureThreshold = BitConverter.ToUInt16(buffer, 15),
        IdentificationRemoteAlarmGroup = BitConverter.ToUInt32(new byte[4]
        {
          buffer[17],
          buffer[18],
          buffer[19],
          (byte) 0
        }, 0),
        IdentificationRemoteAlarmDevice = BitConverter.ToUInt32(new byte[4]
        {
          buffer[20],
          buffer[21],
          buffer[22],
          (byte) 0
        }, 0),
        SmokeLevel = BitConverter.ToUInt16(buffer, 23),
        IR_Current = BitConverter.ToUInt16(buffer, 25),
        BatteryVoltageSmokeDetector = BitConverter.ToUInt16(buffer, 27)
      } : throw new ArgumentException("Can not parse parameters of smoke detector! Unknown buffer. Length: " + buffer.Length.ToString() + " bytes.");
    }

    public string ToString(int spaces)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      if (this.CurrentDateTime.HasValue)
        stringBuilder1.Append("Current Date Time: ".PadRight(spaces, ' ')).AppendLine(this.CurrentDateTime.Value.ToString());
      else
        stringBuilder1.Append("Current Date Time: ".PadRight(spaces, ' ')).AppendLine("null");
      if (this.DateOfCalibration.HasValue)
        stringBuilder1.Append("Date of Calibration: ".PadRight(spaces, ' ')).AppendLine(this.DateOfCalibration.Value.ToShortDateString());
      else
        stringBuilder1.Append("Date of Calibration: ".PadRight(spaces, ' ')).AppendLine("null");
      if (this.DateOfFirstActivation.HasValue)
        stringBuilder1.Append("Date of First Activation: ".PadRight(spaces, ' ')).AppendLine(this.DateOfFirstActivation.Value.ToShortDateString());
      else
        stringBuilder1.Append("Date of First Activation: ".PadRight(spaces, ' ')).AppendLine("null");
      stringBuilder1.Append("Operating Hours: ".PadRight(spaces, ' ')).AppendLine(this.OperatingHours.ToString());
      stringBuilder1.Append("Reflection Threshold: ".PadRight(spaces, ' ')).AppendLine(this.ReflectionThreshold.ToString());
      stringBuilder1.Append("Smoke Threshold: ".PadRight(spaces, ' ')).AppendLine(this.SmokeThreshold.ToString());
      stringBuilder1.Append("Temperature Threshold: ".PadRight(spaces, ' ')).AppendLine(this.TemperatureThreshold.ToString());
      StringBuilder stringBuilder2 = stringBuilder1.Append("Identification Remote Alarm Group: ".PadRight(spaces, ' '));
      uint num = this.IdentificationRemoteAlarmGroup;
      string str1 = num.ToString();
      stringBuilder2.AppendLine(str1);
      StringBuilder stringBuilder3 = stringBuilder1.Append("Identification Remote Alarm Device: ".PadRight(spaces, ' '));
      num = this.IdentificationRemoteAlarmDevice;
      string str2 = num.ToString();
      stringBuilder3.AppendLine(str2);
      stringBuilder1.Append("Smoke Level: ".PadRight(spaces, ' ')).AppendLine(this.SmokeLevel.ToString());
      stringBuilder1.Append("IR Current: ".PadRight(spaces, ' ')).AppendLine(this.IR_Current.ToString());
      stringBuilder1.Append("Battery Voltage: ".PadRight(spaces, ' ')).AppendLine(this.BatteryVoltageSmokeDetector.ToString() + " mV");
      return stringBuilder1.ToString();
    }

    public MinoprotectII_Parameter DeepCopy()
    {
      return new MinoprotectII_Parameter()
      {
        CurrentDateTime = this.CurrentDateTime,
        DateOfFirstActivation = this.DateOfFirstActivation,
        BatteryVoltageSmokeDetector = this.BatteryVoltageSmokeDetector,
        Buffer = this.Buffer,
        DateOfCalibration = this.DateOfCalibration,
        OperatingHours = this.OperatingHours,
        ReflectionThreshold = this.ReflectionThreshold,
        SmokeThreshold = this.SmokeThreshold,
        TemperatureThreshold = this.TemperatureThreshold,
        IdentificationRemoteAlarmGroup = this.IdentificationRemoteAlarmGroup,
        IdentificationRemoteAlarmDevice = this.IdentificationRemoteAlarmDevice,
        SmokeLevel = this.SmokeLevel,
        IR_Current = this.IR_Current
      };
    }
  }
}
