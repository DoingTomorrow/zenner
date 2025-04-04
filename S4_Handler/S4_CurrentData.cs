// Decompiled with JetBrains decompiler
// Type: S4_Handler.S4_CurrentData
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace S4_Handler
{
  public class S4_CurrentData
  {
    public static List<KeyValuePair<string, int>> PrintColumns = new List<KeyValuePair<string, int>>();
    public S4_BaseUnitSupport Units;

    public DateTime DeviceTime { get; set; }

    public string VolumeUnit => this.Units.VolumeUnitString;

    public string FlowUnit => this.Units.FlowUnitString;

    public double Volume { get; private set; }

    public double FlowVolume { get; private set; }

    public double ReturnVolume { get; private set; }

    public float Flow { get; private set; }

    public float WaterTemperature { get; private set; }

    static S4_CurrentData()
    {
      S4_CurrentData.PrintColumns.Add(new KeyValuePair<string, int>("Device time", 20));
      S4_CurrentData.PrintColumns.Add(new KeyValuePair<string, int>("Vol [m\u00B3]", 12));
      S4_CurrentData.PrintColumns.Add(new KeyValuePair<string, int>("FlVol [m\u00B3]", 12));
      S4_CurrentData.PrintColumns.Add(new KeyValuePair<string, int>("ReVol [m\u00B3]", 12));
      S4_CurrentData.PrintColumns.Add(new KeyValuePair<string, int>("Flow [m\u00B3/h]", 12));
      S4_CurrentData.PrintColumns.Add(new KeyValuePair<string, int>("Temp [°C]", 10));
    }

    public S4_CurrentData(byte[] receivedFrame)
    {
      int offset = 2;
      this.DeviceTime = ByteArrayScanner.ScanDateTime(receivedFrame, ref offset);
      this.Units = new S4_BaseUnitSupport((S4_BaseUnits) ByteArrayScanner.ScanByte(receivedFrame, ref offset));
      this.Volume = ByteArrayScanner.ScanDouble(receivedFrame, ref offset);
      this.FlowVolume = ByteArrayScanner.ScanDouble(receivedFrame, ref offset);
      this.ReturnVolume = ByteArrayScanner.ScanDouble(receivedFrame, ref offset);
      this.Flow = ByteArrayScanner.ScanFloat(receivedFrame, ref offset);
      if (receivedFrame.Length >= offset + 6)
        this.WaterTemperature = ByteArrayScanner.ScanFloat(receivedFrame, ref offset);
      else
        this.WaterTemperature = float.NaN;
    }

    public string GetHeader(string waitTimeRangeHeader)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < S4_CurrentData.PrintColumns.Count; ++index)
      {
        if (index == 1)
          stringBuilder.Append(waitTimeRangeHeader);
        KeyValuePair<string, int> printColumn = S4_CurrentData.PrintColumns[index];
        stringBuilder.Append(printColumn.Key.PadLeft(printColumn.Value));
      }
      return stringBuilder.ToString();
    }

    public string GetLine(string waitTimeRange)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      int num1 = 0;
      StringBuilder stringBuilder2 = stringBuilder1;
      string str1 = this.DeviceTime.ToString("dd.MM.yyyy HH:mm:ss");
      List<KeyValuePair<string, int>> printColumns1 = S4_CurrentData.PrintColumns;
      int index1 = num1;
      int num2 = index1 + 1;
      int totalWidth1 = printColumns1[index1].Value;
      string str2 = str1.PadLeft(totalWidth1);
      stringBuilder2.Append(str2);
      stringBuilder1.Append(waitTimeRange);
      StringBuilder stringBuilder3 = stringBuilder1;
      string str3 = this.Volume.ToString("0.000");
      List<KeyValuePair<string, int>> printColumns2 = S4_CurrentData.PrintColumns;
      int index2 = num2;
      int num3 = index2 + 1;
      int totalWidth2 = printColumns2[index2].Value;
      string str4 = str3.PadLeft(totalWidth2);
      stringBuilder3.Append(str4);
      StringBuilder stringBuilder4 = stringBuilder1;
      string str5 = this.FlowVolume.ToString("0.000");
      List<KeyValuePair<string, int>> printColumns3 = S4_CurrentData.PrintColumns;
      int index3 = num3;
      int num4 = index3 + 1;
      int totalWidth3 = printColumns3[index3].Value;
      string str6 = str5.PadLeft(totalWidth3);
      stringBuilder4.Append(str6);
      StringBuilder stringBuilder5 = stringBuilder1;
      string str7 = this.ReturnVolume.ToString("0.000");
      List<KeyValuePair<string, int>> printColumns4 = S4_CurrentData.PrintColumns;
      int index4 = num4;
      int num5 = index4 + 1;
      int totalWidth4 = printColumns4[index4].Value;
      string str8 = str7.PadLeft(totalWidth4);
      stringBuilder5.Append(str8);
      StringBuilder stringBuilder6 = stringBuilder1;
      string str9 = this.Flow.ToString("0.000");
      List<KeyValuePair<string, int>> printColumns5 = S4_CurrentData.PrintColumns;
      int index5 = num5;
      int num6 = index5 + 1;
      int totalWidth5 = printColumns5[index5].Value;
      string str10 = str9.PadLeft(totalWidth5);
      stringBuilder6.Append(str10);
      StringBuilder stringBuilder7 = stringBuilder1;
      string str11 = this.WaterTemperature.ToString("0.0");
      List<KeyValuePair<string, int>> printColumns6 = S4_CurrentData.PrintColumns;
      int index6 = num6;
      int num7 = index6 + 1;
      int totalWidth6 = printColumns6[index6].Value;
      string str12 = str11.PadLeft(totalWidth6);
      stringBuilder7.Append(str12);
      return stringBuilder1.ToString();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.DeviceTime.ToString("dd.MM.yyyy HH:mm:ss"));
      stringBuilder.Append(": Vol=" + this.Volume.ToString() + " " + this.VolumeUnit);
      stringBuilder.Append(" ; VolFlow=" + this.FlowVolume.ToString() + " " + this.VolumeUnit);
      stringBuilder.Append(" ; VolRet=" + this.ReturnVolume.ToString() + " " + this.VolumeUnit);
      stringBuilder.Append(" ; Flow=" + this.Flow.ToString() + " " + this.FlowUnit);
      if (!float.IsNaN(this.WaterTemperature))
        stringBuilder.Append(" ; Temp=" + this.WaterTemperature.ToString() + " °C");
      return stringBuilder.ToString();
    }

    public string ToTextBlock()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Device time: .............. " + this.DeviceTime.ToString("dd.MM.yyyy HH:mm:ss"));
      stringBuilder.AppendLine("Volume: ................... " + this.Volume.ToString() + " " + this.VolumeUnit);
      stringBuilder.AppendLine("Volume in flow direction: . " + this.FlowVolume.ToString() + " " + this.VolumeUnit);
      stringBuilder.AppendLine("Volume in return direction: " + this.ReturnVolume.ToString() + " " + this.VolumeUnit);
      stringBuilder.AppendLine("Current flow: ............. " + this.Flow.ToString() + " " + this.FlowUnit);
      if (!float.IsNaN(this.WaterTemperature))
        stringBuilder.AppendLine("Water temperature: ........ " + this.WaterTemperature.ToString() + " °C");
      return stringBuilder.ToString();
    }
  }
}
