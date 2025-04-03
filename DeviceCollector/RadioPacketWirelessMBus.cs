// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RadioPacketWirelessMBus
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public sealed class RadioPacketWirelessMBus : RadioDevicePacket
  {
    private static int HEADER_SIZE = 10;
    private static Logger logger = LogManager.GetLogger(nameof (RadioPacketWirelessMBus));
    private byte lengthOfPacket;
    private byte controlField;
    private byte protocolValue;

    public RadioPacketWirelessMBus.PacketType ControlFieldType { get; private set; }

    public new string Manufacturer { get; private set; }

    public byte VersionNumber { get; private set; }

    public byte Medium { get; private set; }

    public string MediumString => MBusDevice.GetMediaString(this.Medium);

    public RadioPacketWirelessMBus.ProtocolType Protocol { get; set; }

    public byte? ACC { get; set; }

    public byte? STS { get; set; }

    public ushort? ConfWord { get; set; }

    public byte[] Data { get; set; }

    public string ZDF { get; set; }

    public long? FunkIdSecundary { get; set; }

    public string ManufacturerSecundary { get; set; }

    public byte? VersionNumberSecundary { get; set; }

    public byte? MediumSecundary { get; set; }

    public bool? Synchronous { get; set; }

    public override bool Parse(byte[] packet, DateTime receivedAt, bool hasRssi)
    {
      this.ZDF = string.Empty;
      if (!UserManager.CheckPermission(UserRights.Rights.WirelessMBus))
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission of wireless M-Bus!");
      if (packet == null)
        throw new ArgumentNullException("Input parameter 'packet' can not be null!");
      this.ReceivedAt = receivedAt;
      this.ZDF = this.ZDF + "RTIME;" + receivedAt.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern) + ";";
      RadioPacketWirelessMBus.logger.Trace(Util.ByteArrayToHexString(packet));
      this.Buffer = packet;
      if (!this.DecodeHeader())
        return false;
      this.ZDF = this.ZDF + "SID;" + this.FunkId.ToString() + ";";
      this.ZDF = this.ZDF + "MAN;" + this.Manufacturer + ";";
      this.ZDF = this.ZDF + "GEN;" + this.VersionNumber.ToString() + ";";
      this.ZDF = this.ZDF + "MED;" + this.MediumString + ";";
      base.Manufacturer = this.Manufacturer;
      this.Version = this.VersionNumber.ToString();
      this.Medium = this.MediumString;
      if (this.Buffer.Length == RadioPacketWirelessMBus.HEADER_SIZE)
        return true;
      int headerSize = RadioPacketWirelessMBus.HEADER_SIZE;
      this.protocolValue = this.Buffer[headerSize];
      byte[] buffer1 = this.Buffer;
      int index1 = headerSize;
      int num1 = index1 + 1;
      this.Protocol = this.DecodeProtocolType(buffer1[index1]);
      if (!this.DecodeData())
        return false;
      int num2 = (int) this.lengthOfPacket + 1;
      if (hasRssi && this.Buffer.Length >= num2 + 2)
      {
        byte[] buffer2 = this.Buffer;
        int index2 = num2;
        int num3 = index2 + 1;
        this.RSSI = new byte?(buffer2[index2]);
        this.ZDF = this.ZDF + "RSSI;" + this.RSSI_dBm.ToString() + ";";
        byte[] buffer3 = this.Buffer;
        int index3 = num3;
        int startIndex = index3 + 1;
        this.LQI = new byte?(buffer3[index3]);
        if (this.Buffer.Length >= startIndex + 4)
          this.MCT = BitConverter.ToUInt32(this.Buffer, startIndex);
        this.IsCrcOk = ((int) this.LQI.Value & 128) == 128;
        if (!this.IsCrcOk)
          return false;
      }
      else
        this.IsCrcOk = true;
      return true;
    }

    private bool DecodeHeader()
    {
      if (this.Buffer == null)
        throw new ArgumentNullException("Buffer can not be null!");
      if (this.Buffer.Length < RadioPacketWirelessMBus.HEADER_SIZE)
        return false;
      int num1 = 0;
      byte[] buffer1 = this.Buffer;
      int index1 = num1;
      int index2 = index1 + 1;
      this.lengthOfPacket = buffer1[index1];
      this.controlField = this.Buffer[index2];
      byte[] buffer2 = this.Buffer;
      int index3 = index2;
      int startIndex1 = index3 + 1;
      this.ControlFieldType = this.DecodePacketType(buffer2[index3]);
      this.Manufacturer = MBusDevice.GetManufacturer(BitConverter.ToInt16(this.Buffer, startIndex1));
      int startIndex2 = startIndex1 + 2;
      this.FunkId = Util.ConvertBcdInt64ToInt64((long) BitConverter.ToUInt32(this.Buffer, startIndex2));
      int num2 = startIndex2 + 4;
      byte[] buffer3 = this.Buffer;
      int index4 = num2;
      int num3 = index4 + 1;
      this.VersionNumber = buffer3[index4];
      byte[] buffer4 = this.Buffer;
      int index5 = num3;
      int num4 = index5 + 1;
      this.Medium = buffer4[index5];
      return true;
    }

    private bool DecodeData()
    {
      if ((int) this.lengthOfPacket <= RadioPacketWirelessMBus.HEADER_SIZE)
        return false;
      int startIndex1 = RadioPacketWirelessMBus.HEADER_SIZE + 1;
      long? funkIdSecundary;
      if (this.Protocol == RadioPacketWirelessMBus.ProtocolType.MBusWithFullHeader)
      {
        this.FunkIdSecundary = new long?(Util.ConvertBcdInt64ToInt64((long) BitConverter.ToUInt32(this.Buffer, startIndex1)));
        int startIndex2 = startIndex1 + 4;
        string zdf = this.ZDF;
        funkIdSecundary = this.FunkIdSecundary;
        string str = funkIdSecundary.ToString();
        this.ZDF = zdf + "SEC_ID;" + str + ";";
        funkIdSecundary = this.FunkIdSecundary;
        this.FunkId = funkIdSecundary.Value;
        this.ManufacturerSecundary = MBusDevice.GetManufacturer(BitConverter.ToInt16(this.Buffer, startIndex2));
        int num1 = startIndex2 + 2;
        this.ZDF = this.ZDF + "SEC_MAN;" + this.ManufacturerSecundary + ";";
        byte[] buffer1 = this.Buffer;
        int index1 = num1;
        int num2 = index1 + 1;
        this.VersionNumberSecundary = new byte?(buffer1[index1]);
        byte[] buffer2 = this.Buffer;
        int index2 = num2;
        startIndex1 = index2 + 1;
        this.MediumSecundary = new byte?(buffer2[index2]);
        this.ZDF = this.ZDF + "SEC_VER;" + this.VersionNumberSecundary.ToString() + ";";
        this.ZDF = this.ZDF + "SEC_MED;" + MBusDevice.GetMediaString(this.MediumSecundary.Value) + ";";
      }
      byte[] buffer3 = this.Buffer;
      int index3 = startIndex1;
      int num3 = index3 + 1;
      this.ACC = new byte?(buffer3[index3]);
      byte[] buffer4 = this.Buffer;
      int index4 = num3;
      int num4 = index4 + 1;
      this.STS = new byte?(buffer4[index4]);
      byte[] buffer5 = this.Buffer;
      int index5 = num4;
      int num5 = index5 + 1;
      this.ConfWord = new ushort?((ushort) buffer5[index5]);
      ushort? nullable1 = this.ConfWord;
      int? nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      byte[] buffer6 = this.Buffer;
      int index6 = num5;
      int srcOffset1 = index6 + 1;
      int num6 = (int) (ushort) ((uint) buffer6[index6] << 8);
      ushort? nullable3;
      if (!nullable2.HasValue)
      {
        nullable1 = new ushort?();
        nullable3 = nullable1;
      }
      else
        nullable3 = new ushort?((ushort) (nullable2.GetValueOrDefault() | num6));
      this.ConfWord = nullable3;
      nullable1 = this.ConfWord;
      int? nullable4;
      if (!nullable1.HasValue)
      {
        nullable2 = new int?();
        nullable4 = nullable2;
      }
      else
        nullable4 = new int?(((int) nullable1.GetValueOrDefault() & 3840) >> 8);
      int? nullable5 = nullable4;
      nullable1 = this.ConfWord;
      int? nullable6;
      if (!nullable1.HasValue)
      {
        nullable2 = new int?();
        nullable6 = nullable2;
      }
      else
        nullable6 = new int?((((int) nullable1.GetValueOrDefault() & 240) >> 4) * 16);
      nullable2 = nullable6;
      int length = nullable2.Value;
      nullable1 = this.ConfWord;
      int? nullable7;
      if (!nullable1.HasValue)
      {
        nullable2 = new int?();
        nullable7 = nullable2;
      }
      else
        nullable7 = new int?(((int) nullable1.GetValueOrDefault() & 8192) >> 13);
      this.Synchronous = new bool?(Convert.ToBoolean((object) nullable7));
      this.ZDF = this.ZDF + "SYNCHRONOUS;" + this.Synchronous.ToString() + ";";
      string zdf1 = this.ZDF;
      nullable2 = nullable5;
      string str1 = nullable2.ToString();
      this.ZDF = zdf1 + "ENCRYPTMODE;" + str1 + ";";
      byte num7 = this.Buffer[srcOffset1];
      byte num8 = this.Buffer[srcOffset1 + 1];
      if (num7 == (byte) 47 && num8 == (byte) 47)
      {
        int srcOffset2 = srcOffset1 + 1 + 1;
        this.Data = new byte[(int) this.lengthOfPacket - srcOffset2 + 1];
        System.Buffer.BlockCopy((Array) this.Buffer, srcOffset2, (Array) this.Data, 0, this.Data.Length);
        this.ZDF += MBusDevice.ParseMBusDifVif(this.Data);
      }
      else
      {
        byte num9 = this.ACC.Value;
        byte[] bytes1;
        byte[] bytes2;
        byte versionNumber;
        byte medium;
        if (this.Protocol == RadioPacketWirelessMBus.ProtocolType.MBusWithFullHeader)
        {
          funkIdSecundary = this.FunkIdSecundary;
          bytes1 = BitConverter.GetBytes(Util.ConvertUnt32ToBcdUInt32((uint) funkIdSecundary.Value));
          bytes2 = BitConverter.GetBytes(MBusDevice.GetManufacturerCode(this.ManufacturerSecundary));
          versionNumber = this.VersionNumberSecundary.Value;
          medium = this.MediumSecundary.Value;
        }
        else
        {
          bytes1 = BitConverter.GetBytes(Util.ConvertUnt32ToBcdUInt32((uint) this.FunkId));
          bytes2 = BitConverter.GetBytes(MBusDevice.GetManufacturerCode(this.Manufacturer));
          versionNumber = this.VersionNumber;
          medium = this.Medium;
        }
        nullable2 = nullable5;
        int num10 = 5;
        if (nullable2.GetValueOrDefault() == num10 & nullable2.HasValue)
        {
          byte[] IV = new byte[16]
          {
            bytes2[0],
            bytes2[1],
            bytes1[0],
            bytes1[1],
            bytes1[2],
            bytes1[3],
            versionNumber,
            medium,
            num9,
            num9,
            num9,
            num9,
            num9,
            num9,
            num9,
            num9
          };
          try
          {
            this.Data = new byte[length];
            System.Buffer.BlockCopy((Array) this.Buffer, srcOffset1, (Array) this.Data, 0, this.Data.Length);
            byte[] key = (byte[]) null;
            if (UserManager.CheckPermission("ZennerDefaultKey"))
              key = Util.HexStringToByteArray("5A8470C4806F4A87CEF4D5F2D985AB18");
            if (key == null)
            {
              this.ZDF = this.ZDF + "AES;" + Util.ByteArrayToHexString(this.Data) + ";";
            }
            else
            {
              this.Data = Util.DecryptCBC_AES_128(key, IV, this.Data);
              if (this.Data[0] == (byte) 47 && this.Data[1] == (byte) 47)
              {
                List<byte> byteList = new List<byte>((IEnumerable<byte>) this.Data);
                byteList.RemoveAt(0);
                byteList.RemoveAt(0);
                this.Data = byteList.ToArray();
                this.ZDF += MBusDevice.ParseMBusDifVif(this.Data);
              }
              else
                this.ZDF = this.ZDF + "AES;" + Util.ByteArrayToHexString(this.Data) + ";";
            }
          }
          catch (Exception ex)
          {
            RadioPacketWirelessMBus.logger.Error("Error occurred while decrypt the AES data! " + ex.Message);
            this.ZDF = this.ZDF + "AES;" + Util.ByteArrayToHexString(this.Data) + ";";
          }
        }
      }
      return true;
    }

    private RadioPacketWirelessMBus.PacketType DecodePacketType(byte value)
    {
      return Enum.IsDefined(typeof (RadioPacketWirelessMBus.PacketType), (object) value) ? (RadioPacketWirelessMBus.PacketType) Enum.ToObject(typeof (RadioPacketWirelessMBus.PacketType), value) : RadioPacketWirelessMBus.PacketType.S2;
    }

    private RadioPacketWirelessMBus.ProtocolType DecodeProtocolType(byte value)
    {
      if (Enum.IsDefined(typeof (RadioPacketWirelessMBus.ProtocolType), (object) value))
        return (RadioPacketWirelessMBus.ProtocolType) Enum.ToObject(typeof (RadioPacketWirelessMBus.ProtocolType), value);
      return value >= (byte) 160 || value <= (byte) 183 ? RadioPacketWirelessMBus.ProtocolType.ManufacturerSpecific : RadioPacketWirelessMBus.ProtocolType.Unknown;
    }

    public override string ToString() => this.ZDF.ToString();

    public override SortedList<long, SortedList<DateTime, ReadingValue>> GetValues()
    {
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      TranslationRulesManager.Instance?.TryParse(this.ZDF, 0, ref valueList);
      if (this.RSSI_dBm.HasValue)
        ValueIdent.AddValueToValueIdentList(ref valueList, this.ReceivedAt, this.GetValueIdentOfSignalStrengthValue(), (object) this.RSSI_dBm.Value);
      return valueList;
    }

    internal override SortedList<long, SortedList<DateTime, ReadingValue>> Merge(
      SortedList<long, SortedList<DateTime, ReadingValue>> oldMeterValues)
    {
      return this.GetValues();
    }

    public enum PacketType : byte
    {
      S2_Install = 6,
      S2 = 7,
      SendNoReply = 68, // 0x44
      InstallationTransmit = 70, // 0x46
    }

    public enum ProtocolType : byte
    {
      ReadoutDeviceToMeter = 81, // 0x51
      AlarmReport = 113, // 0x71
      MBusWithFullHeader = 114, // 0x72
      MBusWithoutHeader = 120, // 0x78
      MBusWithShortHeader = 122, // 0x7A
      Repeater = 129, // 0x81
      ForFutureUse = 130, // 0x82
      ManufacturerSpecific = 131, // 0x83
      Unknown = 132, // 0x84
    }
  }
}
