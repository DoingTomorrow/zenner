// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.ConnectionProfileParameter
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  public enum ConnectionProfileParameter
  {
    None = 0,
    NotForConfiguration = 1,
    NotForReading = 2,
    InDevelopement = 3,
    OnlyForProduction = 4,
    OnlyForProfessionals = 5,
    ConfigParam = 10000, // 0x00002710
    Handler = 20000, // 0x00004E20
    Radio2 = 20001, // 0x00004E21
    Radio3 = 20002, // 0x00004E22
    SystemDevice = 20003, // 0x00004E23
    MBus = 20004, // 0x00004E24
    wMBus = 20005, // 0x00004E25
    LoRa = 20006, // 0x00004E26
    Manufacturer = 20007, // 0x00004E27
    Medium = 20008, // 0x00004E28
    Generation = 20009, // 0x00004E29
    UpdateFirmwarePossible = 20010, // 0x00004E2A
    LoggerReadingPossible = 20011, // 0x00004E2B
    Scanning = 40000, // 0x00009C40
    JobManager = 40001, // 0x00009C41
    SystemType = 40002, // 0x00009C42
    TransceiverType = 40003, // 0x00009C43
  }
}
