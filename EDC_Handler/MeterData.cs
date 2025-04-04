// Decompiled with JetBrains decompiler
// Type: EDC_Handler.MeterData
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;

#nullable disable
namespace EDC_Handler
{
  public sealed class MeterData
  {
    public int MeterID { get; set; }

    public DateTime TimePoint { get; set; }

    public int HardwareTypeID { get; set; }

    public byte[] Buffer { get; set; }
  }
}
