// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RadioPacketMinomatV4
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public sealed class RadioPacketMinomatV4 : RadioPacket
  {
    private static Logger logger = LogManager.GetLogger(nameof (RadioPacketMinomatV4));

    public RadioPacketMinomatV4.LpsrHeader Header { get; private set; }

    public RadioPacketMinomatV4.InfoData Data { get; private set; }

    public override bool Parse(byte[] packet, DateTime receivedAt, bool hasRssi)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MinomatV4))
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission of Minomat V4!");
      if (packet == null)
        throw new ArgumentNullException("Input parameter 'packet' can not be null!");
      this.ReceivedAt = receivedAt;
      if (RadioPacketMinomatV4.logger.IsTraceEnabled)
        RadioPacketMinomatV4.logger.Trace(Util.ByteArrayToHexString(packet));
      this.Buffer = packet;
      int offset = 0;
      this.Header = RadioPacketMinomatV4.LpsrHeader.Parse(this.Buffer, ref offset);
      if (this.Header.Type != (byte) 74)
        return false;
      this.Data = RadioPacketMinomatV4.InfoData.Parse(this.Buffer, ref offset);
      if (hasRssi)
      {
        byte[] buffer1 = this.Buffer;
        int index1 = offset;
        int num = index1 + 1;
        this.RSSI = new byte?(buffer1[index1]);
        byte[] buffer2 = this.Buffer;
        int index2 = num;
        int startIndex = index2 + 1;
        this.LQI = new byte?(buffer2[index2]);
        if (this.Buffer.Length >= startIndex + 4)
          this.MCT = BitConverter.ToUInt32(this.Buffer, startIndex);
        this.IsCrcOk = ((int) this.LQI.Value & 128) == 128;
        if (!this.IsCrcOk)
          return false;
      }
      else
        this.IsCrcOk = true;
      this.FunkId = (long) this.Header.NetworkId;
      this.DeviceType = DeviceTypes.MinomatDevice;
      return true;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("NetworkID: ").Append(this.FunkId.ToString().PadRight(10));
      stringBuilder.Append("RSSI MinoConnect (dBm): ").Append(this.RSSI_dBm.ToString().PadRight(5));
      stringBuilder.Append("RSSI Minomat: (raw)").Append(this.Data.InquiryRssi.Value.ToString().PadRight(5));
      return stringBuilder.ToString();
    }

    public override SortedList<long, SortedList<DateTime, ReadingValue>> GetValues()
    {
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      long valueIdForValueEnum1 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.SignalStrength, ValueIdent.ValueIdPart_MeterType.Collector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.Current, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any);
      long valueIdForValueEnum2 = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.SignalStrength, ValueIdent.ValueIdPart_MeterType.Transceiver, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.Current, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any);
      if (this.RSSI_dBm.HasValue)
        ValueIdent.AddValueToValueIdentList(ref valueList, DateTime.Now, valueIdForValueEnum2, (object) this.RSSI_dBm.Value);
      if (this.Data != null && this.Data.InquiryRssi != null)
        ValueIdent.AddValueToValueIdentList(ref valueList, DateTime.Now, valueIdForValueEnum1, (object) this.Data.InquiryRssi.RSSI_dBm);
      return valueList;
    }

    internal override SortedList<long, SortedList<DateTime, ReadingValue>> Merge(
      SortedList<long, SortedList<DateTime, ReadingValue>> oldMeterValues)
    {
      return this.GetValues();
    }

    public sealed class LpsrHeader
    {
      public byte Length { get; set; }

      public byte NetworkId { get; set; }

      public byte Type { get; set; }

      public byte Number { get; set; }

      public ushort To { get; set; }

      public ushort From { get; set; }

      public static RadioPacketMinomatV4.LpsrHeader Parse(byte[] buffer, ref int offset)
      {
        if (buffer == null || offset < 0)
          return (RadioPacketMinomatV4.LpsrHeader) null;
        RadioPacketMinomatV4.LpsrHeader lpsrHeader = new RadioPacketMinomatV4.LpsrHeader();
        lpsrHeader.Length = buffer[offset++];
        lpsrHeader.NetworkId = buffer[offset++];
        lpsrHeader.Type = buffer[offset++];
        lpsrHeader.Number = buffer[offset++];
        lpsrHeader.To = BitConverter.ToUInt16(buffer, offset);
        offset += 2;
        lpsrHeader.From = BitConverter.ToUInt16(buffer, offset);
        offset += 2;
        return lpsrHeader;
      }
    }

    public sealed class RssiFilter
    {
      public byte Count { get; set; }

      public byte Remainder { get; set; }

      public byte Value { get; set; }

      public static RadioPacketMinomatV4.RssiFilter Parse(byte[] buffer, ref int offset)
      {
        if (buffer == null || offset < 0)
          return (RadioPacketMinomatV4.RssiFilter) null;
        return new RadioPacketMinomatV4.RssiFilter()
        {
          Count = buffer[offset++],
          Remainder = buffer[offset++],
          Value = buffer[offset++]
        };
      }

      public int RSSI_dBm => Util.RssiToRssi_dBm(this.Value);
    }

    public sealed class InfoData
    {
      public const int MAX_SAVLEN = 4;
      public const int INFO_DATA_SIZE = 28;

      public ushort Parent { get; private set; }

      public byte Hops { get; private set; }

      public byte Islot { get; private set; }

      public ushort[] SAV { get; private set; }

      public ushort Offset { get; private set; }

      public uint Secs { get; private set; }

      public ushort Millis { get; private set; }

      public ushort FrameCounter { get; private set; }

      public byte Tslot { get; private set; }

      public byte SetupCounter { get; private set; }

      public byte SubTreeId { get; private set; }

      public RadioPacketMinomatV4.RssiFilter InquiryRssi { get; private set; }

      public static RadioPacketMinomatV4.InfoData Parse(byte[] buffer, ref int offset)
      {
        if (buffer == null || offset < 0)
          return (RadioPacketMinomatV4.InfoData) null;
        if (buffer.Length < offset + 28)
          throw new IndexOutOfRangeException("Invalid size of InfoData buffer! Buffer: " + Util.ByteArrayToHexString(buffer));
        RadioPacketMinomatV4.InfoData infoData = new RadioPacketMinomatV4.InfoData();
        infoData.Parent = BitConverter.ToUInt16(buffer, offset);
        offset += 2;
        infoData.Hops = buffer[offset++];
        infoData.Islot = buffer[offset++];
        infoData.SAV = new ushort[4];
        for (int index = 0; index < 4; ++index)
        {
          infoData.SAV[index] = BitConverter.ToUInt16(buffer, offset);
          offset += 2;
        }
        infoData.Offset = BitConverter.ToUInt16(buffer, offset);
        offset += 2;
        infoData.Secs = BitConverter.ToUInt32(buffer, offset);
        offset += 4;
        infoData.Millis = BitConverter.ToUInt16(buffer, offset);
        offset += 2;
        infoData.FrameCounter = BitConverter.ToUInt16(buffer, offset);
        offset += 2;
        infoData.Tslot = buffer[offset++];
        infoData.SetupCounter = buffer[offset++];
        infoData.SubTreeId = buffer[offset++];
        infoData.InquiryRssi = RadioPacketMinomatV4.RssiFilter.Parse(buffer, ref offset);
        return infoData;
      }
    }
  }
}
