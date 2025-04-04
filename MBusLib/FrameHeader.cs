// Decompiled with JetBrains decompiler
// Type: MBusLib.FrameHeader
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using MBusLib.Utility;
using System;

#nullable disable
namespace MBusLib
{
  [Serializable]
  public class FrameHeader
  {
    public uint? ID_BCD { get; set; }

    public ushort? Manufacturer { get; set; }

    public byte? Generation { get; set; }

    public MBusLib.Medium? Medium { get; set; }

    public uint? ID
    {
      get
      {
        return this.ID_BCD.HasValue ? new uint?(Util.ConvertBcdUInt32ToUInt32(this.ID_BCD.Value)) : new uint?();
      }
    }

    public string ManufacturerString
    {
      get
      {
        return this.Manufacturer.HasValue ? Util.GetManufacturer(this.Manufacturer.Value) : (string) null;
      }
    }

    public uint? ID_BCD_Secondary { get; set; }

    public ushort? Manufacturer_Secondary { get; set; }

    public byte? Generation_Secondary { get; set; }

    public MBusLib.Medium? Medium_Secondary { get; set; }

    public uint? ID_Secondary
    {
      get
      {
        return this.ID_BCD_Secondary.HasValue ? new uint?(Util.ConvertBcdUInt32ToUInt32(this.ID_BCD_Secondary.Value)) : new uint?();
      }
    }

    public string ManufacturerString_Secondary
    {
      get
      {
        return this.Manufacturer_Secondary.HasValue ? Util.GetManufacturer(this.Manufacturer_Secondary.Value) : (string) null;
      }
    }

    public byte? ACC { get; set; }

    public string IdentityKey
    {
      get
      {
        if (this.ID_BCD.HasValue)
        {
          string str = this.ManufacturerString + "_" + this.Generation.ToString();
          uint? id = this.ID;
          uint? nullable;
          int num1;
          if (str == "ZRI_0")
          {
            nullable = id;
            uint num2 = 112408;
            if (nullable.GetValueOrDefault() >= num2 & nullable.HasValue)
            {
              nullable = id;
              uint num3 = 302598;
              num1 = nullable.GetValueOrDefault() <= num3 & nullable.HasValue ? 1 : 0;
              goto label_5;
            }
          }
          num1 = 0;
label_5:
          if (num1 != 0)
            return "ZRI_13";
          int num4;
          if (str == "@@@_0")
          {
            nullable = id;
            uint num5 = 112408;
            if (nullable.GetValueOrDefault() >= num5 & nullable.HasValue)
            {
              nullable = id;
              uint num6 = 302598;
              num4 = nullable.GetValueOrDefault() <= num6 & nullable.HasValue ? 1 : 0;
              goto label_11;
            }
          }
          num4 = 0;
label_11:
          return num4 != 0 ? "ZRI_13" : str;
        }
        return this.ID_BCD_Secondary.HasValue ? this.ManufacturerString_Secondary + "_" + this.Generation_Secondary.ToString() : (string) null;
      }
    }
  }
}
