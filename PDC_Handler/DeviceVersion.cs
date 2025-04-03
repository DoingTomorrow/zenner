// Decompiled with JetBrains decompiler
// Type: PDC_Handler.DeviceVersion
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using DeviceCollector;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace PDC_Handler
{
  public sealed class DeviceVersion
  {
    public const int SIZE = 18;

    public uint Version { get; set; }

    public byte Major => (byte) (this.Version >> 24);

    public byte Minor => (byte) (this.Version >> 16 & (uint) byte.MaxValue);

    public ushort Revision => (ushort) ((this.Version & 61440U) >> 12);

    public ushort TypeValue => (ushort) (this.Version & 4095U);

    public string VersionString
    {
      get
      {
        return string.Format("{0}.{1}.{2}:{3}", (object) this.Major, (object) this.Minor, (object) this.Revision, (object) this.Type);
      }
    }

    public uint HardwareTypeID { get; set; }

    public uint SvnRevision { get; set; }

    public DateTime? BuildTime { get; set; }

    public ushort Signatur { get; set; }

    public PDC_DeviceIdentity Type
    {
      get
      {
        return Enum.IsDefined(typeof (PDC_DeviceIdentity), (object) this.TypeValue) ? (PDC_DeviceIdentity) Enum.ToObject(typeof (PDC_DeviceIdentity), this.TypeValue) : PDC_DeviceIdentity.Unknown;
      }
    }

    public DeviceVersion DeepCopy() => this.MemberwiseClone() as DeviceVersion;

    public override string ToString() => this.VersionString;

    public string ToString(int spaces)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Version: ".PadRight(spaces)).AppendLine(this.VersionString);
      stringBuilder.Append("Version (dec): ".PadRight(spaces)).AppendLine(this.Version.ToString());
      stringBuilder.Append("Version (hex): ".PadRight(spaces)).Append("0x").AppendLine(this.Version.ToString("X8"));
      stringBuilder.Append("Device Identity: ".PadRight(spaces)).Append(((byte) this.Type).ToString()).Append(" 0x").AppendLine(((byte) this.Type).ToString("X2"));
      stringBuilder.Append("HardwareTypeID: ".PadRight(spaces)).AppendLine(this.HardwareTypeID.ToString());
      stringBuilder.Append("SvnRevision: ".PadRight(spaces)).AppendLine(this.SvnRevision.ToString());
      stringBuilder.Append("BuildTime: ".PadRight(spaces)).AppendLine(this.BuildTime.HasValue ? this.BuildTime.Value.ToString("G") : "null");
      stringBuilder.Append("Signatur: ".PadRight(spaces)).AppendLine("0x" + this.Signatur.ToString("X4"));
      return stringBuilder.ToString();
    }

    internal byte[] GetBytes()
    {
      List<byte> byteList = new List<byte>(18);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.Version));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.HardwareTypeID));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.SvnRevision));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(0));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.Signatur));
      if (byteList.Count != 18)
      {
        int num = 18;
        string str1 = num.ToString();
        num = byteList.Count;
        string str2 = num.ToString();
        throw new ArgumentOutOfRangeException("Invalid size of versions buffer! Expected: " + str1 + ", Actual: " + str2);
      }
      return byteList.ToArray();
    }

    internal static DeviceVersion Parse(byte[] buffer, ref int offset)
    {
      try
      {
        DeviceVersion deviceVersion = new DeviceVersion();
        deviceVersion.Version = BitConverter.ToUInt32(buffer, offset);
        offset += 4;
        deviceVersion.HardwareTypeID = BitConverter.ToUInt32(buffer, offset);
        offset += 4;
        deviceVersion.SvnRevision = BitConverter.ToUInt32(buffer, offset);
        offset += 4;
        deviceVersion.BuildTime = MBusDifVif.GetMBusDateTime(buffer, offset);
        offset += 4;
        deviceVersion.Signatur = BitConverter.ToUInt16(buffer, offset);
        offset += 2;
        return deviceVersion;
      }
      catch
      {
        return (DeviceVersion) null;
      }
    }

    internal static bool IsEqual(DeviceVersion a, DeviceVersion b)
    {
      return a != null && b != null && (int) a.Version == (int) b.Version;
    }
  }
}
