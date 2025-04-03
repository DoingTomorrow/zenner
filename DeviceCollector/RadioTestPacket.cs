// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RadioTestPacket
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using System;

#nullable disable
namespace DeviceCollector
{
  public sealed class RadioTestPacket
  {
    public byte TestPacketVersion { get; set; }

    public uint MeterID { get; set; }

    public uint SapNumber { get; set; }

    public uint SerialNumber { get; set; }

    internal static RadioTestPacket Parse(byte[] packet)
    {
      if (packet.Length < 20)
        return (RadioTestPacket) null;
      return new RadioTestPacket()
      {
        TestPacketVersion = packet[7],
        MeterID = BitConverter.ToUInt32(packet, 8),
        SapNumber = BitConverter.ToUInt32(packet, 12),
        SerialNumber = Convert.ToUInt32(BitConverter.ToUInt32(packet, 16).ToString("X8"))
      };
    }
  }
}
