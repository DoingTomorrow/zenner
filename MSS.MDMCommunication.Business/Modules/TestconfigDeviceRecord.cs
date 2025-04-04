// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.Modules.TestconfigDeviceRecord
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

#nullable disable
namespace MSS.MDMCommunication.Business.Modules
{
  public class TestconfigDeviceRecord
  {
    public string Config_ID { get; set; }

    public int TestRun_No { get; set; }

    public string Project_ID { get; set; }

    public string Radio_ID { get; set; }

    public int Last_RSSI { get; set; }

    public int AverageRSSI { get; set; }

    public int DeviceType { get; set; }
  }
}
