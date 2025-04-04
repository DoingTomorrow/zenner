// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.SmokeDetectorVersion
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using DeviceCollector;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace SmokeDetectorHandler
{
  public class SmokeDetectorVersion
  {
    public const int BUFFER_SIZE = 10;

    public uint Serialnumber { get; set; }

    public string Manufacturer { get; set; }

    public byte Generation { get; set; }

    public MBusDeviceType Medium { get; set; }

    public ushort DeviceIdentity { get; set; }

    public byte Revision { get; set; }

    public byte Subversion { get; set; }

    public byte VersionNumber { get; set; }

    public ushort HardwareTypeID { get; set; }

    public uint SapNumber { get; set; }

    public ushort HardwareVersion { get; set; }

    public SmokeDetectorStatusInformation Status { get; set; }

    public string VersionString
    {
      get
      {
        return string.Format("{0}.{1}.{2}", (object) this.VersionNumber, (object) this.Subversion, (object) this.Revision);
      }
    }

    public byte[] Buffer { get; private set; }

    public override string ToString() => this.ToString(20);

    public string ToString(int spaces)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Serial number: ".PadRight(spaces, ' ')).AppendLine(this.Serialnumber.ToString());
      stringBuilder.Append("Manufacturer: ".PadRight(spaces, ' ')).AppendLine(this.Manufacturer);
      stringBuilder.Append("Generation: ".PadRight(spaces, ' ')).AppendLine(this.Generation.ToString());
      stringBuilder.Append("Medium: ".PadRight(spaces, ' ')).Append(this.Medium.ToString()).Append(" (0x").Append(((byte) this.Medium).ToString("X2")).AppendLine(")");
      stringBuilder.Append("Firmware version: ".PadRight(spaces, ' ')).AppendLine(this.VersionString);
      stringBuilder.Append("Device identity: ".PadRight(spaces, ' ')).AppendLine(this.DeviceIdentity.ToString());
      stringBuilder.Append("HardwareTypeID: ".PadRight(spaces, ' ')).Append(this.HardwareTypeID.ToString()).Append(" (0x").Append(this.HardwareTypeID.ToString("X3")).AppendLine(")");
      stringBuilder.Append("SAP Number: ".PadRight(spaces, ' ')).AppendLine(this.SapNumber.ToString());
      byte[] bytes = BitConverter.GetBytes(this.HardwareVersion);
      byte num1 = bytes[0];
      byte num2 = bytes[1];
      stringBuilder.Append("Hardware version: ".PadRight(spaces, ' ')).Append(num2.ToString()).Append('.').Append(num1.ToString()).AppendLine(string.Format("  (0x{0:X4}, {0})", (object) this.HardwareVersion));
      stringBuilder.Append("Status: ".PadRight(spaces, ' ')).Append("(0x" + ((byte) this.Status).ToString("X2") + ") ").AppendLine(this.Status.ToString());
      stringBuilder.Append("Buffer: ").AppendLine(Util.ByteArrayToHexString(this.Buffer));
      return stringBuilder.ToString();
    }

    internal static SmokeDetectorVersion Parse(
      uint serialnumber,
      string manufacturer,
      byte generation,
      byte medium,
      byte status,
      byte[] buffer)
    {
      if (buffer == null)
        throw new NullReferenceException("Can not parse version of smoke detector! The buffer is null.");
      if (buffer.Length != 10)
        throw new NullReferenceException("Can not parse version of smoke detector! Wrong length of buffer. Expected: 10 bytes, Actual: " + buffer.Length.ToString() + " bytes.");
      return new SmokeDetectorVersion()
      {
        Buffer = buffer,
        Serialnumber = serialnumber,
        Manufacturer = manufacturer,
        Medium = (MBusDeviceType) medium,
        Generation = generation,
        Status = (SmokeDetectorStatusInformation) status,
        DeviceIdentity = BitConverter.ToUInt16(new byte[2]
        {
          buffer[0],
          (byte) ((uint) buffer[1] & 15U)
        }, 0),
        Revision = (byte) ((uint) buffer[1] >> 4),
        Subversion = buffer[2],
        VersionNumber = buffer[3],
        SapNumber = BitConverter.ToUInt32(new byte[4]
        {
          buffer[4],
          buffer[5],
          (byte) ((uint) buffer[6] & 15U),
          (byte) 0
        }, 0),
        HardwareTypeID = BitConverter.ToUInt16(new byte[2]
        {
          buffer[7],
          (byte) ((uint) buffer[6] >> 4)
        }, 0),
        HardwareVersion = BitConverter.ToUInt16(buffer, 8)
      };
    }

    internal SmokeDetectorVersion DeepCopy()
    {
      return new SmokeDetectorVersion()
      {
        Buffer = this.Buffer,
        Serialnumber = this.Serialnumber,
        Manufacturer = this.Manufacturer,
        Medium = this.Medium,
        Generation = this.Generation,
        Status = this.Status,
        DeviceIdentity = this.DeviceIdentity,
        Revision = this.Revision,
        Subversion = this.Subversion,
        VersionNumber = this.VersionNumber,
        HardwareTypeID = this.HardwareTypeID,
        SapNumber = this.SapNumber,
        HardwareVersion = this.HardwareVersion
      };
    }

    internal byte[] CreateWriteBuffer1()
    {
      uint bcdUint32 = Util.ConvertUnt32ToBcdUInt32(this.Serialnumber);
      ushort manufacturerCode = MBusDevice.GetManufacturerCode("ZRI");
      if (!string.IsNullOrEmpty(this.Manufacturer) && this.Manufacturer.Length == 3)
        manufacturerCode = MBusDevice.GetManufacturerCode(this.Manufacturer);
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(bcdUint32));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(manufacturerCode));
      byteList.Add(this.Generation);
      byteList.Add((byte) this.Medium);
      return byteList.ToArray();
    }

    internal byte[] CreateWriteBuffer2()
    {
      byte[] bytes1 = BitConverter.GetBytes(this.SapNumber);
      byte[] bytes2 = BitConverter.GetBytes(this.HardwareTypeID);
      int num1 = (int) bytes1[2] & 15;
      byte num2 = (byte) (((int) bytes2[1] & 15) << 4 | num1);
      List<byte> byteList = new List<byte>();
      byteList.Add(bytes1[0]);
      byteList.Add(bytes1[1]);
      byteList.Add(num2);
      byteList.Add(bytes2[0]);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.HardwareVersion));
      return byteList.ToArray();
    }
  }
}
