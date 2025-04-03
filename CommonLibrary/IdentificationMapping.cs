// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.IdentificationMapping
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace ZENNER.CommonLibrary
{
  public class IdentificationMapping
  {
    private static SortedList<string, ManufacturerDefinitions> ManufacturerList = new SortedList<string, ManufacturerDefinitions>();

    public string Manufacturer { get; private set; }

    public ulong RangeValue { get; private set; }

    public CommonTypeRange TypeRange { get; private set; }

    public LoRa_ProtocolType ProtocolType => this.TypeRange.ProtocolType;

    public char DIN_Sparte => this.TypeRange.DIN_Sparte;

    public uint ID_BCD => (uint) this.RangeValue;

    public byte FabricationBlock => (byte) (this.RangeValue >> 32);

    public byte[] OUI => IdentificationMapping.ManufacturerList[this.Manufacturer].OIU;

    public ulong OUI_value
    {
      get
      {
        byte[] oui = this.OUI;
        return (ulong) (((long) oui[0] << 40) + ((long) oui[1] << 48) + ((long) oui[2] << 56));
      }
    }

    static IdentificationMapping()
    {
      ManufacturerDefinitions manufacturerDefinitions1 = new ManufacturerDefinitions("ZRI", new byte[3]
      {
        (byte) 72,
        (byte) 182,
        (byte) 4
      });
      manufacturerDefinitions1.AddTypeRange(1088774209536UL, 1088935270809UL, '4', LoRa_ProtocolType.HeatCostAllocator);
      manufacturerDefinitions1.AddTypeRange(1084479242240UL, 1084640303513UL, '4', LoRa_ProtocolType.HeatCostAllocator);
      manufacturerDefinitions1.AddTypeRange(592705486848UL, 594745596313UL, '5', LoRa_ProtocolType.CoolingMeter);
      manufacturerDefinitions1.AddTypeRange(588410519552UL, 590450629017UL, '6', LoRa_ProtocolType.HeatMeter);
      manufacturerDefinitions1.AddTypeRange(597000454144UL, 599040563609UL, '6', LoRa_ProtocolType.HeatAndCoolingMeter);
      manufacturerDefinitions1.AddTypeRange(268435456UL, 429496729UL, '7', LoRa_ProtocolType.GasMeter);
      manufacturerDefinitions1.AddTypeRange(658740609024UL, 658884893081UL, '8', LoRa_ProtocolType.WaterMeter);
      manufacturerDefinitions1.AddTypeRange(658891603968UL, 658901670297UL, '9', LoRa_ProtocolType.WaterMeter);
      manufacturerDefinitions1.AddTypeRange(18522046464UL, 18683107737UL, 'E', LoRa_ProtocolType.EDC);
      manufacturerDefinitions1.AddTypeRange(108800245760UL, 108877420953UL, 'E', LoRa_ProtocolType.EDC);
      manufacturerDefinitions1.AddTypeRange(26843545600UL, 27004606873UL, 'E', LoRa_ProtocolType.PDC);
      manufacturerDefinitions1.AddTypeRange(489626271744UL, 492203252121UL, 'E', LoRa_ProtocolType.EDC);
      manufacturerDefinitions1.AddTypeRange(493921239040UL, 496498219417UL, 'E', LoRa_ProtocolType.EDC);
      manufacturerDefinitions1.AddTypeRange(499558383616UL, 499719444889UL, 'E', LoRa_ProtocolType.EDC);
      manufacturerDefinitions1.AddTypeRange(44291850240UL, 45526653337UL, 'E', LoRa_ProtocolType.EDC);
      manufacturerDefinitions1.AddTypeRange(507879882752UL, 508040944025UL, 'E', LoRa_ProtocolType.PDC);
      manufacturerDefinitions1.AddTypeRange(47244640256UL, 49821620633UL, 'F', LoRa_ProtocolType.SmokeDetectorRadio);
      manufacturerDefinitions1.AddTypeRange(65229815808UL, 65390877081UL, 'F', LoRa_ProtocolType.TempHumiditySensor);
      manufacturerDefinitions1.AddTypeRange(1031597457408UL, 1031758453145UL, 'F', LoRa_ProtocolType.TempHumiditySensor);
      manufacturerDefinitions1.AddTypeRange(1031758479360UL, 1031758518681UL, 'F', LoRa_ProtocolType.TempHumiditySensor);
      manufacturerDefinitions1.AddTypeRange(1035892424704UL, 1036053420441UL, 'F', LoRa_ProtocolType.TempHumiditySensor);
      manufacturerDefinitions1.AddTypeRange(1036053446656UL, 1036053485977UL, 'F', LoRa_ProtocolType.TempHumiditySensor);
      IdentificationMapping.ManufacturerList.Add("ZRI", manufacturerDefinitions1);
      ManufacturerDefinitions manufacturerDefinitions2 = new ManufacturerDefinitions("DEV", new byte[3]);
      IdentificationMapping.ManufacturerList.Add("DEV", manufacturerDefinitions2);
    }

    public IdentificationMapping(string fullSerialNumber)
    {
      this.Manufacturer = fullSerialNumber != null && fullSerialNumber.Length == 14 ? fullSerialNumber.Substring(1, 3) : throw new Exception("Illegal full serial number");
      if (this.Manufacturer != "DEV" && !IdentificationMapping.ManufacturerList.ContainsKey(this.Manufacturer))
        throw new NotSupportedException("Not supported manufacturer");
      uint result1;
      if (!uint.TryParse(fullSerialNumber.Substring(6), out result1))
        throw new NotSupportedException("Not a decimal number at last 8 digits");
      ulong result2;
      if (!ulong.TryParse(fullSerialNumber.Substring(4), NumberStyles.HexNumber, (IFormatProvider) null, out result2))
        throw new NotSupportedException("Illegal number format");
      this.RangeValue = result2;
      if (this.Manufacturer != "DEV")
      {
        this.TypeRange = result1 != 0U ? IdentificationMapping.ManufacturerList[this.Manufacturer].TypeRanges.Values.FirstOrDefault<CommonTypeRange>((Func<CommonTypeRange, bool>) (item => item.RangeMin <= this.RangeValue && item.RangeMax >= this.RangeValue)) : IdentificationMapping.ManufacturerList[this.Manufacturer].TypeRanges.Values.FirstOrDefault<CommonTypeRange>((Func<CommonTypeRange, bool>) (item => ((long) item.RangeMin & -4294967296L) == (long) this.RangeValue));
        if (this.TypeRange == null)
          throw new Exception("Number range not defined");
        if ((int) this.TypeRange.DIN_Sparte != (int) fullSerialNumber[0])
          throw new Exception("Sparte illegal for this fabrication block and number range");
      }
      else
      {
        if ((result2 & (ulong) uint.MaxValue) > 0UL)
          throw new Exception("Developer serial number has always to be 0");
        this.TypeRange = new CommonTypeRange(0UL, 0UL, fullSerialNumber[0], LoRa_ProtocolType.WaterMeter);
      }
    }

    public IdentificationMapping(ulong loRa_DevEUI)
    {
      this.RangeValue = loRa_DevEUI & 1099511627775UL;
      this.Manufacturer = (string) null;
      foreach (ManufacturerDefinitions manufacturerDefinitions in (IEnumerable<ManufacturerDefinitions>) IdentificationMapping.ManufacturerList.Values)
      {
        if ((int) manufacturerDefinitions.OIU[0] + ((int) manufacturerDefinitions.OIU[1] << 8) + ((int) manufacturerDefinitions.OIU[2] << 16) == (int) (uint) (loRa_DevEUI >> 40))
        {
          this.Manufacturer = manufacturerDefinitions.Manufacturer;
          this.TypeRange = manufacturerDefinitions.TypeRanges.Values.FirstOrDefault<CommonTypeRange>((Func<CommonTypeRange, bool>) (item => item.RangeMin <= this.RangeValue && item.RangeMax >= this.RangeValue));
          if (this.TypeRange == null)
            throw new Exception("Number range not defined");
          break;
        }
      }
      if (this.Manufacturer == null)
        throw new NotSupportedException("Not supported manufacturer");
    }

    public IdentificationMapping(byte[] loRa_DevEUI)
      : this(BitConverter.ToUInt64(loRa_DevEUI, 0))
    {
      if (loRa_DevEUI == null || loRa_DevEUI.Length != 8)
        throw new Exception("Illegal DevEUI");
    }

    public ulong GetAsDevEUI_Value() => this.OUI_value + this.RangeValue;

    public string GetAsDevEUI_string() => this.GetAsDevEUI_Value().ToString("X016");

    public byte[] GetAsDevEUI_bytes() => BitConverter.GetBytes(this.GetAsDevEUI_Value());

    public string GetAsFullSerialNumber()
    {
      return this.TypeRange.DIN_Sparte.ToString() + this.Manufacturer + this.RangeValue.ToString("X010");
    }
  }
}
