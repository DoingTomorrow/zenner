// Decompiled with JetBrains decompiler
// Type: HandlerLib.DeviceVersion
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using MBusLib;
using System;
using System.Collections.Generic;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class DeviceVersion : IPrintable
  {
    public uint FirmwareVersion { get; set; }

    public ushort Signatur { get; set; }

    public uint HardwareTypeID { get; set; }

    public virtual uint ID_BCD { get; set; }

    public virtual ushort Manufacturer { get; set; }

    public virtual byte Generation { get; set; }

    public virtual byte Medium { get; set; }

    public virtual uint HardwareID { get; set; }

    public virtual uint SvnRevision { get; set; }

    public virtual DateTime? BuildTime { get; set; }

    public virtual ushort CompilerChecksum { get; set; }

    public uint ID_Bin => Utility.ConvertBcdUInt32ToUInt32(this.ID_BCD);

    public string ManufacturerName => MBusUtil.GetManufacturer(this.Manufacturer);

    public string MediumName => MBusUtil.GetMedium(this.Medium);

    public ZENNER.CommonLibrary.FirmwareVersion FirmwareVersionObj
    {
      get => new ZENNER.CommonLibrary.FirmwareVersion(this.FirmwareVersion);
    }

    public DeviceVersion()
    {
    }

    public DeviceVersion(
      uint serialNumberBCD,
      ushort manufacturerCode,
      byte generation,
      byte mediumCode)
    {
      this.ID_BCD = serialNumberBCD;
      this.Generation = generation;
      this.Manufacturer = manufacturerCode;
      this.Medium = mediumCode;
    }

    public override string ToString() => this.FirmwareVersion.ToString();

    public virtual string Print(int spaces = 0) => Utility.PrintObject((object) this);

    public byte[] GetBytes()
    {
      List<byte> byteList = new List<byte>(18);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.FirmwareVersion));
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

    public virtual DeviceVersion Clone() => this.MemberwiseClone() as DeviceVersion;
  }
}
