// Decompiled with JetBrains decompiler
// Type: MBusLib.Entities.FirmwareInfo
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

#nullable disable
namespace MBusLib.Entities
{
  public static class FirmwareInfo
  {
    public static ControllerType GetControllerType(DeviceIdentity type)
    {
      switch (type)
      {
        case DeviceIdentity.C2:
        case DeviceIdentity.C2a:
        case DeviceIdentity.C5:
        case DeviceIdentity.WR3:
        case DeviceIdentity.WR4:
        case DeviceIdentity.EDC_radio:
        case DeviceIdentity.EDC_MBus:
        case DeviceIdentity.PDC_wMBus:
        case DeviceIdentity.PDC_MBus:
        case DeviceIdentity.PDC_Radio3:
        case DeviceIdentity.TH_sensor:
        case DeviceIdentity.C5_LoRa:
        case DeviceIdentity.WR4_LoRa:
        case DeviceIdentity.C2e:
          return ControllerType.MSP430;
        case DeviceIdentity.Minoprotect_II:
        case DeviceIdentity.Smoke_detector:
          return ControllerType.PIC;
        default:
          return ControllerType.ARM;
      }
    }
  }
}
