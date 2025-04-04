// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_MonitorData
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using System;

#nullable disable
namespace S4_Handler.UserInterface
{
  public class S4_MonitorData
  {
    public DateTime PcTime { get; set; }

    public DateTime DeviceTime { get; set; }

    public double Volume { get; set; }

    public double FlowVolume { get; set; }

    public double ReturnVolume { get; set; }

    public double Flow { get; set; }

    public double Temp { get; set; }

    public string Flags { get; set; }

    public byte CCC { get; set; }

    public string Alarm { get; set; }

    public S4_MonitorData()
    {
      this.PcTime = DateTime.Now;
      this.Temp = double.NaN;
    }
  }
}
