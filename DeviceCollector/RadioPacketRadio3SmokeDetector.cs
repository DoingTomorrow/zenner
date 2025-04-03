// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RadioPacketRadio3SmokeDetector
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using System;

#nullable disable
namespace DeviceCollector
{
  public sealed class RadioPacketRadio3SmokeDetector
  {
    public DateTime? DateOfFirstActivation { get; set; }

    public SmokeDetectorEvent?[] MonthlyEvents { get; set; }

    public SmokeDetectorEvent?[] DailyEvents { get; set; }

    public RadioPacketRadio3SmokeDetector()
    {
      this.MonthlyEvents = new SmokeDetectorEvent?[19];
      this.DailyEvents = new SmokeDetectorEvent?[33];
    }
  }
}
