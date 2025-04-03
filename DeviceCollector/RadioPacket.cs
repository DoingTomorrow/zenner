// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RadioPacket
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using NLog;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  [Serializable]
  public abstract class RadioPacket : EventArgs
  {
    private static Logger logger = LogManager.GetLogger(nameof (RadioPacket));

    public long FunkId { get; set; }

    public DeviceTypes DeviceType { get; protected set; }

    public byte[] Buffer { get; protected set; }

    public byte? RSSI { get; set; }

    public byte? LQI { get; protected set; }

    public DateTime ReceivedAt { get; protected set; }

    public bool IsCrcOk { get; protected set; }

    public RadioTestPacket RadioTestPacket { get; protected set; }

    public string Manufacturer { get; protected set; }

    public string Medium { get; protected set; }

    public string Version { get; protected set; }

    public DeviceCollectorFunctions MyFunctions { get; set; }

    public uint MCT { get; protected set; }

    public int? RSSI_dBm
    {
      get => this.RSSI.HasValue ? new int?(Util.RssiToRssi_dBm(this.RSSI.Value)) : new int?();
    }

    public abstract bool Parse(byte[] packet, DateTime receivedAt, bool hasRssi);

    public abstract SortedList<long, SortedList<DateTime, ReadingValue>> GetValues();

    internal abstract SortedList<long, SortedList<DateTime, ReadingValue>> Merge(
      SortedList<long, SortedList<DateTime, ReadingValue>> oldMeterValues);

    public static RadioPacket Parse(byte[] buffer, bool hasRssi)
    {
      try
      {
        RadioPacket radioPacket = (RadioPacket) new RadioPacketRadio2();
        if (radioPacket.Parse(buffer, DateTime.Now, hasRssi))
          return radioPacket;
      }
      catch
      {
      }
      try
      {
        RadioPacket radioPacket = (RadioPacket) new RadioPacketRadio3();
        if (radioPacket.Parse(buffer, DateTime.Now, hasRssi))
          return radioPacket;
      }
      catch
      {
      }
      try
      {
        RadioPacket radioPacket = (RadioPacket) new RadioPacketWirelessMBus();
        if (radioPacket.Parse(buffer, DateTime.Now, hasRssi))
          return radioPacket;
      }
      catch
      {
      }
      return (RadioPacket) null;
    }
  }
}
