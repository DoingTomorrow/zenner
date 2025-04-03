// Decompiled with JetBrains decompiler
// Type: DeviceCollector.ReadVersionData
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using System;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class ReadVersionData
  {
    private short mBusManufacturer;
    public string ManufacturerString = string.Empty;
    public byte mBusMedium;
    public string MBusMediumString;
    public byte MBusGeneration;
    public uint MBusSerialNr;
    private uint version;
    public uint hardwareIdentification;
    public string HardwareIdentificationString = string.Empty;
    public uint BuildRevision;
    public DateTime BuildTime = DateTime.MinValue;
    public ushort FirmwareSignature;

    public short MBusManufacturer
    {
      get => this.mBusManufacturer;
      set
      {
        this.mBusManufacturer = value;
        this.ManufacturerString = MBusDevice.GetManufacturer(this.mBusManufacturer);
      }
    }

    public byte MBusMedium
    {
      get => this.mBusMedium;
      set
      {
        this.mBusMedium = value;
        this.MBusMediumString = MBusDevice.GetMediaString(value);
      }
    }

    public uint Version
    {
      get => this.version;
      set => this.version = value;
    }

    public int? PacketSizeOfResponceByGetVersionCommand { get; set; }

    public string GetVersionString()
    {
      return this.PacketSizeOfResponceByGetVersionCommand.HasValue ? ParameterService.GetVersionString((long) this.Version, this.PacketSizeOfResponceByGetVersionCommand.Value) : string.Empty;
    }

    public uint HardwareIdentification
    {
      get => this.hardwareIdentification;
      set
      {
        this.hardwareIdentification = value;
        string versionString = this.GetVersionString();
        if (versionString.IndexOf("C5") <= -1 && versionString.IndexOf("WR4") <= -1)
          return;
        this.HardwareIdentificationString = ParameterService.GetHardwareString(value);
      }
    }
  }
}
