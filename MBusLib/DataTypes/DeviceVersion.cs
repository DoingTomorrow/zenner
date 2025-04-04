// Decompiled with JetBrains decompiler
// Type: MBusLib.DataTypes.DeviceVersion
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using MBusLib.Utility;
using System;
using ZENNER.CommonLibrary;

#nullable disable
namespace MBusLib.DataTypes
{
  [Serializable]
  public class DeviceVersion : Firmware, IPrintable
  {
    private byte[] mfgData;

    public uint ID_BCD { get; set; }

    public ushort Manufacturer { get; set; }

    public byte Generation { get; set; }

    public Medium Medium { get; set; }

    public int EncryptionMode { get; set; }

    public HardwareFlags? Hardware { get; private set; }

    public uint? SvnRevision { get; private set; }

    public DateTime? BuildTime { get; private set; }

    public uint? CompilerChecksum { get; private set; }

    public byte[] MfgData
    {
      get => this.mfgData;
      set
      {
        this.mfgData = value;
        if (this.mfgData != null)
        {
          if (this.mfgData.Length >= 4 && this.mfgData.Length <= 7 && ((int) this.mfgData[1] & 15) == 0 && ((int) this.mfgData[0] & (int) byte.MaxValue) == 0 && BitConverter.ToUInt32(this.MfgData, 0) < 33554432U)
          {
            this.RawVersion = BitConverter.ToUInt32(this.MfgData, 0);
            if (this.RawVersion < 17039360U)
              this.RawVersion |= 4065U;
            else
              this.RawVersion |= 1U;
          }
          else if (this.mfgData.Length == 4 || this.mfgData.Length == 7 || this.mfgData.Length == 10 || this.mfgData.Length == 52 || this.mfgData.Length == 18)
          {
            this.RawVersion = BitConverter.ToUInt32(this.MfgData, 0);
            if (this.mfgData.Length != 18)
              return;
            this.Hardware = new HardwareFlags?((HardwareFlags) BitConverter.ToUInt32(this.mfgData, 4));
            this.SvnRevision = new uint?(BitConverter.ToUInt32(this.mfgData, 8));
            this.BuildTime = Util.ConvertToDate_MBus_CP16_TypeG(this.mfgData, 12);
            this.CompilerChecksum = new uint?((uint) BitConverter.ToUInt16(this.mfgData, 16));
          }
          else
          {
            if (this.mfgData.Length != 19)
              return;
            this.RawVersion = BitConverter.ToUInt32(this.mfgData, 1);
            this.Hardware = new HardwareFlags?((HardwareFlags) BitConverter.ToUInt32(this.mfgData, 5));
            this.SvnRevision = new uint?(BitConverter.ToUInt32(this.mfgData, 9));
            this.BuildTime = Util.ConvertToDate_MBus_CP16_TypeG(this.mfgData, 13);
            this.CompilerChecksum = new uint?((uint) BitConverter.ToUInt16(this.mfgData, 17));
          }
        }
        else
        {
          this.RawVersion = 0U;
          this.Hardware = new HardwareFlags?();
          this.SvnRevision = new uint?();
          this.BuildTime = new DateTime?();
          this.CompilerChecksum = new uint?();
        }
      }
    }

    public uint ID => Util.ConvertBcdUInt32ToUInt32(this.ID_BCD);

    public string ManufacturerString => Util.GetManufacturer(this.Manufacturer);

    public string Print(bool expert = true, int spaces = 0)
    {
      return Util.PrintObject((object) this, spaces);
    }

    public string Print(int spaces = 0) => Util.PrintObject((object) this);
  }
}
