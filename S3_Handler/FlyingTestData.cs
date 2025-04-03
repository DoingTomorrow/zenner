// Decompiled with JetBrains decompiler
// Type: S3_Handler.FlyingTestData
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.Text;

#nullable disable
namespace S3_Handler
{
  public class FlyingTestData
  {
    public readonly ushort TimeStart_4ms;
    public readonly ushort TimeStop_4ms;
    public readonly ushort TimeVolNotUsed_4ms;
    public readonly uint TimeVolUsed_4ms;
    public readonly uint TimeStart_s;
    public readonly uint TimeStop_s;
    public readonly float FlowStart;
    public readonly float FlowStop;
    public readonly float VolComplete;
    public readonly float VolTimeVolUsed;

    public float TimeInCompleteCycles => (float) this.TimeVolUsed_4ms / 256f;

    public float TimeInStartPlusStopCycle => (float) this.TimeVolNotUsed_4ms / 256f;

    public float CompleteMeasurementTime
    {
      get => this.TimeInCompleteCycles + this.TimeInStartPlusStopCycle;
    }

    public uint CompleteSecondsFromFirmware => this.TimeStop_s - this.TimeStart_s;

    public FlyingTestData(byte[] meterData)
    {
      this.TimeStart_4ms = meterData.Length == 34 ? BitConverter.ToUInt16(meterData, 0) : throw new Exception("meterData array has wrong length");
      this.TimeStop_4ms = BitConverter.ToUInt16(meterData, 2);
      this.TimeVolNotUsed_4ms = BitConverter.ToUInt16(meterData, 4);
      this.TimeVolUsed_4ms = BitConverter.ToUInt32(meterData, 6);
      this.TimeStart_s = BitConverter.ToUInt32(meterData, 10);
      this.TimeStop_s = BitConverter.ToUInt32(meterData, 14);
      this.FlowStart = BitConverter.ToSingle(meterData, 18);
      this.FlowStop = BitConverter.ToSingle(meterData, 22);
      this.VolComplete = BitConverter.ToSingle(meterData, 26);
      this.VolTimeVolUsed = BitConverter.ToSingle(meterData, 30);
    }

    public override string ToString() => this.ToString("f");

    public static string GetListApprevations()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("*******  Flying test diagnostic addreviations *******");
      stringBuilder.AppendLine("CTC: Cycle time in complete cycles");
      stringBuilder.AppendLine("AFT: Additional flow time in start plus stop cycle");
      stringBuilder.AppendLine("CMT: Complete measurement time");
      stringBuilder.AppendLine("CMS: Complete measurement seconds from device clock system");
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("VOL: Measured volume");
      stringBuilder.AppendLine("VSS: Volume in start and stop cycle");
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("FLS: Flow on start of measurement periode");
      stringBuilder.AppendLine("FLE: Flow on end of measurement periode");
      return stringBuilder.ToString();
    }

    public static string GetListHeader()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("CTC |".PadLeft(10));
      stringBuilder.Append("AFT |".PadLeft(10));
      stringBuilder.Append("CMT |".PadLeft(10));
      stringBuilder.Append("CMS |".PadLeft(10));
      stringBuilder.Append("VOL |".PadLeft(20));
      stringBuilder.Append("VSS |".PadLeft(20));
      stringBuilder.Append("FLS |".PadLeft(14));
      stringBuilder.Append("FLE |".PadLeft(14));
      return stringBuilder.ToString();
    }

    public string ToString(string format)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      switch (format)
      {
        case "f":
          stringBuilder1.AppendLine();
          stringBuilder1.AppendLine("******* Flying Test Diagnostic (firmware view) *******");
          StringBuilder stringBuilder2 = stringBuilder1;
          float num1 = (float) this.TimeStart_4ms / 256f;
          string str1 = "TimeStart .... " + num1.ToString();
          stringBuilder2.AppendLine(str1);
          StringBuilder stringBuilder3 = stringBuilder1;
          num1 = (float) this.TimeStop_4ms / 256f;
          string str2 = "TimeStop ..... " + num1.ToString();
          stringBuilder3.AppendLine(str2);
          StringBuilder stringBuilder4 = stringBuilder1;
          num1 = (float) this.TimeVolNotUsed_4ms / 256f;
          string str3 = "TimeVolNotUsed " + num1.ToString();
          stringBuilder4.AppendLine(str3);
          StringBuilder stringBuilder5 = stringBuilder1;
          num1 = (float) this.TimeVolUsed_4ms / 256f;
          string str4 = "TimeVolUsed .. " + num1.ToString();
          stringBuilder5.AppendLine(str4);
          stringBuilder1.AppendLine("TimeStart .... " + this.TimeStart_s.ToString());
          stringBuilder1.AppendLine("TimeStop ..... " + this.TimeStop_s.ToString());
          StringBuilder stringBuilder6 = stringBuilder1;
          num1 = this.FlowStart;
          string str5 = "FlowStart .... " + num1.ToString();
          stringBuilder6.AppendLine(str5);
          StringBuilder stringBuilder7 = stringBuilder1;
          num1 = this.FlowStop;
          string str6 = "FlowStop ..... " + num1.ToString();
          stringBuilder7.AppendLine(str6);
          StringBuilder stringBuilder8 = stringBuilder1;
          num1 = this.VolComplete;
          string str7 = "VolComplete .. " + num1.ToString();
          stringBuilder8.AppendLine(str7);
          StringBuilder stringBuilder9 = stringBuilder1;
          num1 = this.VolTimeVolUsed;
          string str8 = "VolTimeVolUsed " + num1.ToString();
          stringBuilder9.AppendLine(str8);
          stringBuilder1.AppendLine();
          break;
        case "b":
          stringBuilder1.AppendLine();
          stringBuilder1.AppendLine("******* Flying Test Diagnostic *******");
          stringBuilder1.AppendLine("TimeInCompleteCycles ...... " + this.TimeInCompleteCycles.ToString());
          stringBuilder1.AppendLine("TimeInStartPlusStopCycle .. " + this.TimeInStartPlusStopCycle.ToString());
          stringBuilder1.AppendLine("CompleteMeasurementTime ... " + this.CompleteMeasurementTime.ToString());
          stringBuilder1.AppendLine("CompleteSecondsFromFirmware " + this.CompleteSecondsFromFirmware.ToString());
          stringBuilder1.AppendLine();
          StringBuilder stringBuilder10 = stringBuilder1;
          float num2 = this.VolComplete;
          string str9 = "VolComplete ............... " + num2.ToString();
          stringBuilder10.AppendLine(str9);
          StringBuilder stringBuilder11 = stringBuilder1;
          num2 = this.VolTimeVolUsed;
          string str10 = "VolOutOfCompleteCycles .... " + num2.ToString();
          stringBuilder11.AppendLine(str10);
          stringBuilder1.AppendLine();
          StringBuilder stringBuilder12 = stringBuilder1;
          float num3 = this.FlowStart;
          string str11 = "StartFlow ................. " + num3.ToString();
          stringBuilder12.AppendLine(str11);
          StringBuilder stringBuilder13 = stringBuilder1;
          num3 = this.FlowStop;
          string str12 = "StopFlow .................. " + num3.ToString();
          stringBuilder13.AppendLine(str12);
          stringBuilder1.AppendLine();
          break;
        case "l":
          stringBuilder1.Append((this.TimeInCompleteCycles.ToString("f3") + " |").PadLeft(10));
          stringBuilder1.Append((this.TimeInStartPlusStopCycle.ToString("f3") + " |").PadLeft(10));
          stringBuilder1.Append((this.CompleteMeasurementTime.ToString("f3") + " |").PadLeft(10));
          stringBuilder1.Append((this.CompleteSecondsFromFirmware.ToString("f3") + " |").PadLeft(10));
          stringBuilder1.Append((this.VolComplete.ToString("f6") + " |").PadLeft(20));
          stringBuilder1.Append((this.VolTimeVolUsed.ToString("f6") + " |").PadLeft(20));
          stringBuilder1.Append((this.FlowStart.ToString("f3") + " |").PadLeft(14));
          stringBuilder1.Append((this.FlowStop.ToString("f3") + " |").PadLeft(14));
          break;
        default:
          stringBuilder1.AppendLine("Format error");
          stringBuilder1.AppendLine("Use \"f\" = firmware block");
          stringBuilder1.AppendLine("or  \"b\" = block");
          stringBuilder1.AppendLine("or  \"l\" = list");
          break;
      }
      return stringBuilder1.ToString();
    }
  }
}
