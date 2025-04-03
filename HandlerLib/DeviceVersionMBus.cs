// Decompiled with JetBrains decompiler
// Type: HandlerLib.DeviceVersionMBus
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using MBusLib;
using System;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class DeviceVersionMBus : DeviceIdentification, IPrintable
  {
    public byte[] LongID { get; set; }

    public VersionProtocolTypes VersionProtocolType { get; set; }

    public override uint? ID_BCD
    {
      get => new uint?(BitConverter.ToUInt32(this.LongID, 0));
      set
      {
        Buffer.BlockCopy((Array) BitConverter.GetBytes(value.Value), 0, (Array) this.LongID, 0, 4);
      }
    }

    public override ushort? Manufacturer
    {
      get => new ushort?(BitConverter.ToUInt16(this.LongID, 4));
      set
      {
        Buffer.BlockCopy((Array) BitConverter.GetBytes(value.Value), 0, (Array) this.LongID, 4, 2);
      }
    }

    public override byte? Generation
    {
      get => new byte?(this.LongID[6]);
      set => this.LongID[6] = value.Value;
    }

    public override byte? Medium
    {
      get => new byte?(this.LongID[7]);
      set => this.LongID[7] = value.Value;
    }

    public DeviceVersionMBus() => this.LongID = new byte[8];

    public DeviceVersionMBus(
      uint serialNumberBCD,
      ushort manufacturerCode,
      byte generation,
      byte mediumCode)
    {
      this.ID_BCD = new uint?(serialNumberBCD);
      this.Manufacturer = new ushort?(manufacturerCode);
      this.Generation = new byte?(generation);
      this.Medium = new byte?(mediumCode);
      this.LongID = new byte[8];
      this.VersionProtocolType = VersionProtocolTypes.Series4;
    }

    public static DeviceVersionMBus Parse(MBusFrame frame)
    {
      VariableDataStructure variableDataStructure = VariableDataStructure.Parse(frame);
      DeviceCommandsMBus.GetLongID(variableDataStructure.Header);
      if (variableDataStructure.MfgData.Length == 4)
      {
        uint uint32 = BitConverter.ToUInt32(variableDataStructure.MfgData, 0);
        DeviceVersionMBus deviceVersionMbus = new DeviceVersionMBus();
        deviceVersionMbus.LongID = DeviceCommandsMBus.GetLongID(variableDataStructure.Header);
        deviceVersionMbus.VersionProtocolType = VersionProtocolTypes.Series2;
        deviceVersionMbus.firmwareVersion = new uint?(uint32);
        deviceVersionMbus.primaryAddress = new byte?(frame.Address);
        return deviceVersionMbus;
      }
      if (variableDataStructure.MfgData.Length == 10)
      {
        uint uint32 = BitConverter.ToUInt32(variableDataStructure.MfgData, 0);
        if (!new ZENNER.CommonLibrary.FirmwareVersion(uint32).TypeString.StartsWith("C5"))
        {
          ushort uint16 = BitConverter.ToUInt16(new byte[2]
          {
            variableDataStructure.MfgData[7],
            (byte) ((uint) variableDataStructure.MfgData[6] >> 4)
          }, 0);
          DeviceVersionMBus deviceVersionMbus = new DeviceVersionMBus();
          deviceVersionMbus.LongID = DeviceCommandsMBus.GetLongID(variableDataStructure.Header);
          deviceVersionMbus.VersionProtocolType = VersionProtocolTypes.SmokeDetector;
          deviceVersionMbus.firmwareVersion = new uint?(uint32);
          deviceVersionMbus.hardwareID = new uint?((uint) uint16);
          deviceVersionMbus.primaryAddress = new byte?(frame.Address);
          return deviceVersionMbus;
        }
        DeviceVersionMBus deviceVersionMbus1 = new DeviceVersionMBus();
        deviceVersionMbus1.LongID = DeviceCommandsMBus.GetLongID(variableDataStructure.Header);
        deviceVersionMbus1.VersionProtocolType = VersionProtocolTypes.Series3;
        deviceVersionMbus1.firmwareVersion = new uint?(uint32);
        deviceVersionMbus1.svnRevision = new uint?(BitConverter.ToUInt32(variableDataStructure.MfgData, 4));
        deviceVersionMbus1.hardwareID = new uint?((uint) BitConverter.ToUInt16(variableDataStructure.MfgData, 8));
        return deviceVersionMbus1;
      }
      VersionProtocolTypes versionProtocolTypes;
      uint uint32_1;
      uint uint32_2;
      uint uint32_3;
      DateTime? dateMbusCp16TypeG;
      ushort uint16_1;
      if (variableDataStructure.MfgData.Length == 18)
      {
        versionProtocolTypes = VersionProtocolTypes.Series3;
        uint32_1 = BitConverter.ToUInt32(variableDataStructure.MfgData, 0);
        uint32_2 = BitConverter.ToUInt32(variableDataStructure.MfgData, 4);
        uint32_3 = BitConverter.ToUInt32(variableDataStructure.MfgData, 8);
        dateMbusCp16TypeG = MBusUtil.ConvertToDate_MBus_CP16_TypeG(variableDataStructure.MfgData, 12);
        uint16_1 = BitConverter.ToUInt16(variableDataStructure.MfgData, 16);
      }
      else
      {
        if (variableDataStructure.MfgData.Length != 19)
          throw new Exception("Illegal GetVersion response length");
        if (variableDataStructure.MfgData[0] != (byte) 6)
          throw new Exception("Illegal FC code in GetVersion response: 0x" + variableDataStructure.MfgData[0].ToString("x02"));
        versionProtocolTypes = VersionProtocolTypes.Series4;
        uint32_1 = BitConverter.ToUInt32(variableDataStructure.MfgData, 1);
        uint32_2 = BitConverter.ToUInt32(variableDataStructure.MfgData, 5);
        uint32_3 = BitConverter.ToUInt32(variableDataStructure.MfgData, 9);
        dateMbusCp16TypeG = MBusUtil.ConvertToDate_MBus_CP16_TypeG(variableDataStructure.MfgData, 13);
        uint16_1 = BitConverter.ToUInt16(variableDataStructure.MfgData, 17);
      }
      DeviceVersionMBus deviceVersionMbus2 = new DeviceVersionMBus();
      deviceVersionMbus2.LongID = DeviceCommandsMBus.GetLongID(variableDataStructure.Header);
      deviceVersionMbus2.VersionProtocolType = versionProtocolTypes;
      deviceVersionMbus2.firmwareVersion = new uint?(uint32_1);
      deviceVersionMbus2.hardwareID = new uint?(uint32_2);
      deviceVersionMbus2.svnRevision = new uint?(uint32_3);
      deviceVersionMbus2.buildTime = dateMbusCp16TypeG;
      deviceVersionMbus2.signatur = new ushort?(uint16_1);
      deviceVersionMbus2.primaryAddress = new byte?(frame.Address);
      return deviceVersionMbus2;
    }

    public override string ToString() => this.FirmwareVersion.ToString();

    public new string Print(int spaces = 0) => Utility.PrintObject((object) this);
  }
}
