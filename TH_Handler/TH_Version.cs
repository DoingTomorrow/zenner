// Decompiled with JetBrains decompiler
// Type: TH_Handler.TH_Version
// Assembly: TH_Handler, Version=1.3.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 02D62764-6653-46F8-9117-1BC5233AD061
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\TH_Handler.dll

using DeviceCollector;
using GmmDbLib;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace TH_Handler
{
  public sealed class TH_Version
  {
    public uint Version { get; set; }

    public byte Major => (byte) (this.Version >> 24);

    public byte Minor => (byte) (this.Version >> 16 & (uint) byte.MaxValue);

    public ushort Revision => (ushort) ((this.Version & 61440U) >> 12);

    public ushort TypeValue => (ushort) (this.Version & 4095U);

    public string VersionString
    {
      get
      {
        return string.Format("{0}.{1}.{2}:{3}", (object) this.Major, (object) this.Minor, (object) this.Revision, (object) this.TypeValue);
      }
    }

    public string ProjectName { get; set; }

    public uint HardwareTypeID { get; set; }

    public uint SvnRevision { get; set; }

    public DateTime? BuildTime { get; set; }

    public ushort Signatur { get; set; }

    public TH_Version DeepCopy() => this.MemberwiseClone() as TH_Version;

    public override string ToString() => this.VersionString;

    public string ToString(int spaces)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Project name: ".PadRight(spaces)).AppendLine(this.ProjectName);
      stringBuilder.Append("Version: ".PadRight(spaces)).AppendLine(this.VersionString);
      stringBuilder.Append("Version (dec): ".PadRight(spaces)).AppendLine(this.Version.ToString());
      stringBuilder.Append("Version (hex): ".PadRight(spaces)).Append("0x").AppendLine(this.Version.ToString("X8"));
      stringBuilder.Append("Device Identity: ".PadRight(spaces)).Append(((byte) this.TypeValue).ToString()).Append(" 0x").AppendLine(((byte) this.TypeValue).ToString("X2"));
      stringBuilder.Append("HardwareTypeID: ".PadRight(spaces)).AppendLine(this.HardwareTypeID.ToString());
      stringBuilder.Append("SvnRevision: ".PadRight(spaces)).AppendLine(this.SvnRevision.ToString());
      stringBuilder.Append("BuildTime: ".PadRight(spaces)).AppendLine(this.BuildTime.HasValue ? this.BuildTime.Value.ToString("G") : "null");
      stringBuilder.Append("Signature: ".PadRight(spaces)).AppendLine("0x" + this.Signatur.ToString("X4"));
      return stringBuilder.ToString();
    }

    internal byte[] GetBytes()
    {
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.Version));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.HardwareTypeID));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.SvnRevision));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(0));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.Signatur));
      byteList.AddRange((IEnumerable<byte>) Encoding.ASCII.GetBytes(this.ProjectName));
      return byteList.ToArray();
    }

    internal static TH_Version Parse(byte[] buffer, int offset)
    {
      return TH_Version.Parse(buffer, ref offset);
    }

    internal static TH_Version Parse(byte[] buffer, ref int offset)
    {
      try
      {
        TH_Version thVersion = new TH_Version();
        thVersion.Version = BitConverter.ToUInt32(buffer, offset);
        offset += 4;
        thVersion.HardwareTypeID = BitConverter.ToUInt32(buffer, offset);
        offset += 4;
        thVersion.SvnRevision = BitConverter.ToUInt32(buffer, offset);
        offset += 4;
        thVersion.BuildTime = MBusDifVif.GetMBusDateTime(buffer, offset);
        offset += 4;
        thVersion.Signatur = BitConverter.ToUInt16(buffer, offset);
        offset += 2;
        int count = 0;
        for (int index = offset; index < buffer.Length && buffer[index] != (byte) 0; ++index)
          ++count;
        thVersion.ProjectName = Encoding.ASCII.GetString(buffer, offset, count);
        offset += count;
        return thVersion;
      }
      catch (Exception ex)
      {
        throw new Exception(Ot.Gtm(Tg.HandlerLogic, "InvalidVersionBuffer", "Can not decode the versions buffer!"), ex);
      }
    }

    internal static bool IsEqual(TH_Version a, TH_Version b)
    {
      return a != null && b != null && (int) a.Version == (int) b.Version;
    }
  }
}
