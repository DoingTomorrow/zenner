// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ValueIdentSet
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class ValueIdentSet : EventArgs
  {
    public string SerialNumber;
    public string DeviceType;
    public int Channel;
    public byte[] Buffer;
    public string ZDF;
    public SortedList<long, SortedList<DateTime, ReadingValue>> AvailableValues;

    public object Tag { get; set; }

    public string Manufacturer { get; set; }

    public string Version { get; set; }

    public string MainDeviceSerialNumber { get; set; }

    public string PrimaryAddress { get; set; }

    public int Total { get; set; }

    public int? Scenario { get; set; }
  }
}
