// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.VolumeLoggerData
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using System;

#nullable disable
namespace S4_Handler.Functions
{
  public class VolumeLoggerData
  {
    public int No { get; set; }

    public DateTime LoggerTime { get; set; }

    public double Volume { get; set; }

    public string Unit { get; set; }

    public VolumeLoggerData(
      byte[] byteArray,
      ref int scanOffset,
      S4_BaseUnitSupport unitSupport,
      int no,
      VolumeLoggerData.DateTimeFormat dateTimeFormat = VolumeLoggerData.DateTimeFormat.DateAndTime)
    {
      this.No = no;
      switch (dateTimeFormat)
      {
        case VolumeLoggerData.DateTimeFormat.DateAndTime:
          this.LoggerTime = ByteArrayScanner.ScanDateTime(byteArray, ref scanOffset);
          break;
        case VolumeLoggerData.DateTimeFormat.DateAndTimeSec2000:
          this.LoggerTime = CalendarBase2000.Cal_GetDateTime(ByteArrayScanner.ScanUInt32(byteArray, ref scanOffset));
          break;
        default:
          this.LoggerTime = ByteArrayScanner.ScanDate(byteArray, ref scanOffset);
          break;
      }
      this.Volume = ByteArrayScanner.ScanDouble(byteArray, ref scanOffset);
      if (unitSupport == null)
      {
        this.Unit = "";
      }
      else
      {
        this.Unit = unitSupport.VolumeUnitString;
        if (dateTimeFormat == VolumeLoggerData.DateTimeFormat.DateAndTimeSec2000)
          this.Volume = unitSupport.GetDisplayVolume(this.Volume);
      }
    }

    public override string ToString()
    {
      return this.No.ToString("d02") + " " + this.LoggerTime.ToString("dd.MM.yyyy HH:mm:ss") + " " + this.Volume.ToString().PadRight(20) + this.Unit;
    }

    public enum DateTimeFormat
    {
      Date,
      DateAndTime,
      DateAndTimeSec2000,
    }
  }
}
