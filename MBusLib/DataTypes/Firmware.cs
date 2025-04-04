// Decompiled with JetBrains decompiler
// Type: MBusLib.DataTypes.Firmware
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using MBusLib.Entities;
using System;

#nullable disable
namespace MBusLib.DataTypes
{
  [Serializable]
  public class Firmware
  {
    public uint RawVersion { get; protected set; }

    public Firmware()
    {
    }

    public Firmware(uint version) => this.RawVersion = version;

    public byte Major => (byte) (this.RawVersion >> 24);

    public byte Minor => (byte) (this.RawVersion >> 16 & (uint) byte.MaxValue);

    public ushort Revision => (ushort) ((this.RawVersion & 61440U) >> 12);

    public uint FwVersion => this.RawVersion >> 12;

    public DeviceIdentity DeviceIdentity => (DeviceIdentity) (this.RawVersion & 4095U);

    public Version Version => new Version((int) this.Major, (int) this.Minor, (int) this.Revision);

    public ControllerType ControllerType => FirmwareInfo.GetControllerType(this.DeviceIdentity);

    public DeviceType DeviceType => DeviceType.P2P;

    public override string ToString()
    {
      return string.Format("{0}.{1}.{2} {3}", (object) this.Major, (object) this.Minor, (object) this.Revision, (object) this.DeviceIdentity);
    }

    public byte[] GetBytes() => BitConverter.GetBytes(this.RawVersion);

    public static Firmware Parse(byte[] bytes, int offset)
    {
      if (bytes.Length - offset < 4)
        throw new ArgumentOutOfRangeException(nameof (bytes));
      return Firmware.Parse(BitConverter.ToUInt32(bytes, offset));
    }

    public bool IsPDCv1
    {
      get
      {
        return this.DeviceIdentity == DeviceIdentity.PDC_MBus || this.DeviceIdentity == DeviceIdentity.PDC_Radio3 || this.DeviceIdentity == DeviceIdentity.PDC_wMBus;
      }
    }

    public bool IsPDCv2
    {
      get
      {
        return this.DeviceIdentity == DeviceIdentity.PDC_LoRa || this.DeviceIdentity == DeviceIdentity.PDC_LoRa_868MHz_Darlington || this.DeviceIdentity == DeviceIdentity.PDC_LoRa_915;
      }
    }

    public bool IsEDCv1
    {
      get
      {
        return this.DeviceIdentity == DeviceIdentity.EDC_radio || this.DeviceIdentity == DeviceIdentity.EDC_MBus || this.DeviceIdentity == DeviceIdentity.EDC_MBus_CJ188 || this.DeviceIdentity == DeviceIdentity.EDC_MBus_CN || this.DeviceIdentity == DeviceIdentity.EDC_MBus_STM32;
      }
    }

    public bool IsEDCv2
    {
      get
      {
        return this.DeviceIdentity == DeviceIdentity.EDC_LoRa || this.DeviceIdentity == DeviceIdentity.EDC_LoRa_470Mhz || this.DeviceIdentity == DeviceIdentity.EDC_LoRa_915 || this.DeviceIdentity == DeviceIdentity.EDC_LoRa_868_v2 || this.DeviceIdentity == DeviceIdentity.EDC_LoRa_915_v2 || this.DeviceIdentity == DeviceIdentity.EDC_wMBus || this.DeviceIdentity == DeviceIdentity.micro_LoRa || this.DeviceIdentity == DeviceIdentity.micro_LoRa_LL || this.DeviceIdentity == DeviceIdentity.micro_wMBus || this.DeviceIdentity == DeviceIdentity.micro_wMBus_LL || this.DeviceIdentity == DeviceIdentity.EDC_NB_IoT || this.DeviceIdentity == DeviceIdentity.EDC_NB_IoT_FSNH || this.DeviceIdentity == DeviceIdentity.EDC_NB_IoT_Israel || this.DeviceIdentity == DeviceIdentity.EDC_NB_IoT_LCSW || this.DeviceIdentity == DeviceIdentity.EDC_NB_IoT_TaiWan || this.DeviceIdentity == DeviceIdentity.EDC_NB_IoT_XM || this.DeviceIdentity == DeviceIdentity.EDC_NB_IoT_YJSW;
      }
    }

    public bool IsMicrov2
    {
      get
      {
        return this.DeviceIdentity == DeviceIdentity.micro_LoRa || this.DeviceIdentity == DeviceIdentity.micro_LoRa_LL || this.DeviceIdentity == DeviceIdentity.micro_wMBus || this.DeviceIdentity == DeviceIdentity.micro_wMBus_LL || this.DeviceIdentity == DeviceIdentity.micro_radio3;
      }
    }

    public bool IsDefinedDeviceIdentity
    {
      get
      {
        return Enum.IsDefined(typeof (DeviceIdentity), (object) this.DeviceIdentity) && this.DeviceIdentity != 0;
      }
    }

    public bool IsWMBus
    {
      get
      {
        return this.DeviceIdentity == DeviceIdentity.PDC_wMBus || this.DeviceIdentity == DeviceIdentity.micro_wMBus || this.DeviceIdentity == DeviceIdentity.EDC_wMBus || this.DeviceIdentity == DeviceIdentity.micro_wMBus_LL || this.DeviceIdentity == DeviceIdentity.TH_sensor_wMBus || this.DeviceIdentity == DeviceIdentity.TH_sensor_radio || this.DeviceIdentity == DeviceIdentity.M8 || this.DeviceIdentity == DeviceIdentity.NFC_wMBus || this.DeviceIdentity == DeviceIdentity.SD_wM_Bus;
      }
    }

    public bool IsHybridWMBusLoRa
    {
      get
      {
        return this.DeviceIdentity == DeviceIdentity.TH_sensor_radio || this.DeviceIdentity == DeviceIdentity.M8 || this.DeviceIdentity == DeviceIdentity.IUW || this.DeviceIdentity == DeviceIdentity.NFC_LoRa_wMBus;
      }
    }

    public static Firmware Parse(uint rawVersion)
    {
      if (rawVersion == 33752860U || rawVersion == 33752604U || rawVersion == 33752348U || rawVersion == 33752092U || rawVersion == 33751836U || rawVersion == 33751580U || rawVersion == 33751324U || rawVersion == 33751068U || rawVersion == 33686812U || rawVersion == 33686300U || rawVersion == 33686300U || rawVersion == 33686044U || rawVersion == 33685788U || rawVersion == 33685532U || rawVersion == 33623324U || rawVersion == 33623068U || rawVersion == 33622812U || rawVersion == 33622556U || rawVersion == 33621532U || rawVersion == 33621276U || rawVersion == 33621020U || rawVersion == 33620764U || rawVersion == 33555996U || rawVersion == 33555740U || rawVersion == 33554716U || rawVersion == 33554460U || rawVersion == 851996U || rawVersion == 786460U || rawVersion == 720924U || rawVersion == 655388U || rawVersion == 458780U || rawVersion == 17236507U || rawVersion == 17236251U || rawVersion == 17174299U || rawVersion == 17174043U || rawVersion == 17173787U || rawVersion == 17173531U || rawVersion == 17173275U || rawVersion == 17173019U || rawVersion == 17172763U || rawVersion == 17172507U || rawVersion == 17171739U || rawVersion == 17171483U || rawVersion == 17171227U || rawVersion == 17170971U || rawVersion == 17170715U || rawVersion == 17170459U || rawVersion == 17106715U || rawVersion == 17106459U || rawVersion == 17106203U || rawVersion == 33685786U)
      {
        byte[] bytes = BitConverter.GetBytes(rawVersion);
        bytes[1] = (byte) ((uint) bytes[1] << 4);
        return new Firmware()
        {
          RawVersion = BitConverter.ToUInt32(bytes, 0)
        };
      }
      return new Firmware() { RawVersion = rawVersion };
    }
  }
}
