// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.GMMSettingsCollection
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

#nullable disable
namespace ZR_ClassLibrary
{
  [Serializable]
  public class GMMSettingsCollection
  {
    public static string DEFAULT_SETTINGS_NAME = "settings.xml";

    public GMMSettingsName SelectedSettingsName { get; set; }

    public List<GMMSettings> Items { get; set; }

    public GMMSettingsCollection() => this.Items = new List<GMMSettings>();

    public static GMMSettingsCollection Default
    {
      get
      {
        return new GMMSettingsCollection()
        {
          Items = {
            GMMSettings.Default_MBus,
            GMMSettings.Default_MBus_MeterVPN,
            GMMSettings.Default_MBusP2P_IrCombiHeadRoundSide_IrDa_MinoConnect,
            GMMSettings.Default_MBusP2P_IrCombiHeadRoundSide_ZVEI_Break_MinoConnect,
            GMMSettings.Default_MBusP2P_IrCombiHeadRoundSide_ZVEI_MinoConnect,
            GMMSettings.Default_MinolDevice_IrCombiHeadDoveTailSide_MinoConnect,
            GMMSettings.Default_MinolDevice_IrMinoHead,
            GMMSettings.Default_MinomatV2_IrCombiHeadDoveTailSide_MinoConnect,
            GMMSettings.Default_MinomatV2_IrMinoHead,
            GMMSettings.Default_MinomatV3_IrCombiHeadRoundSide_MinoConnect,
            GMMSettings.Default_MinomatV3_IrMinoHead,
            GMMSettings.Default_MinomatV4_IrCombiHeadDoveTailSide_MinoConnect,
            GMMSettings.Default_MinomatV4_IrMinoHead,
            GMMSettings.Default_ModeMinomatRadioTest_MinoConnect,
            GMMSettings.Default_ModeRadioMS_MinoConnect,
            GMMSettings.Default_Radio2_MinoConnect,
            GMMSettings.Default_Radio2_MinoHead,
            GMMSettings.Default_Radio3_868_95_RUSSIA_MinoConnect,
            GMMSettings.Default_Radio3_MinoConnect,
            GMMSettings.Default_Radio3_MinoHead,
            GMMSettings.Default_RelayDevice_MinoConnect,
            GMMSettings.Default_Wavenis,
            GMMSettings.Default_WirelessMBusModeC1A_MinoConnect,
            GMMSettings.Default_WirelessMBusModeC1B_MinoConnect,
            GMMSettings.Default_WirelessMBusModeS1_MinoConnect,
            GMMSettings.Default_WirelessMBusModeS1M_MinoConnect,
            GMMSettings.Default_WirelessMBusModeS2_MinoConnect,
            GMMSettings.Default_WirelessMBusModeT1_MinoConnect,
            GMMSettings.Default_WirelessMBusModeT2_meter_MinoConnect,
            GMMSettings.Default_WirelessMBusModeT2_other_MinoConnect,
            GMMSettings.Default_MBusP2P_IrCombiHeadRoundSide_IrDa_MinoConnect_EDC,
            GMMSettings.Default_SmokeDetector_IrCombiHeadDoveTailSide_MinoConnect
          }
        };
      }
    }

    public GMMSettings this[GMMSettingsName name]
    {
      get
      {
        foreach (GMMSettings gmmSettings in this.Items)
        {
          if (gmmSettings.Name == name)
            return gmmSettings;
        }
        return (GMMSettings) null;
      }
    }

    public static void Save(GMMSettingsCollection settingsCollection)
    {
      GMMSettingsCollection.Save(settingsCollection, GMMSettingsCollection.DEFAULT_SETTINGS_NAME);
    }

    public static void Save(GMMSettingsCollection settingsCollection, string fileName)
    {
      if (settingsCollection == null || SystemValues.SettingsPath == null)
        return;
      string path = Path.Combine(SystemValues.SettingsPath, fileName);
      using (FileStream fileStream = File.Create(path))
        new XmlSerializer(typeof (GMMSettingsCollection)).Serialize((Stream) fileStream, (object) settingsCollection);
    }

    public static GMMSettingsCollection Load()
    {
      return GMMSettingsCollection.Load(GMMSettingsCollection.DEFAULT_SETTINGS_NAME);
    }

    public static GMMSettingsCollection Load(string fileName)
    {
      if (SystemValues.SettingsPath == null)
        return (GMMSettingsCollection) null;
      string path = Path.Combine(SystemValues.SettingsPath, fileName);
      if (!File.Exists(path))
        return (GMMSettingsCollection) null;
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (GMMSettingsCollection));
      using (FileStream fileStream = File.OpenRead(path))
      {
        try
        {
          return xmlSerializer.Deserialize((Stream) fileStream) as GMMSettingsCollection;
        }
        catch (Exception ex)
        {
          Debug.WriteLine("Wrong settings file detected! Remove it.");
          File.Delete(fileName);
          return (GMMSettingsCollection) null;
        }
      }
    }
  }
}
