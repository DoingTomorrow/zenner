// Decompiled with JetBrains decompiler
// Type: DeviceCollector.MbusTelegram
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using System;

#nullable disable
namespace DeviceCollector
{
  internal sealed class MbusTelegram
  {
    public DateTime TimePoint { get; set; }

    public byte[] Buffer { get; set; }

    public MbusTelegram(DateTime timePoint, byte[] buffer)
    {
      this.TimePoint = timePoint;
      this.Buffer = buffer;
    }
  }
}
