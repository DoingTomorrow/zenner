// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ReadoutSettingsManager
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
  [XmlRoot]
  [Serializable]
  public sealed class ReadoutSettingsManager
  {
    private const string DEFAULT_SETTINGS_NAME = "ReadoutSettings.xml";
    private static object syncLock = new object();
    private static ReadoutSettingsManager _instance;

    public List<ReadoutSettingsManager.ReadoutDeviceSettings> Settings { get; set; }

    public ReadoutSettingsManager()
    {
      this.Settings = new List<ReadoutSettingsManager.ReadoutDeviceSettings>();
    }

    public static ReadoutSettingsManager Instance
    {
      get
      {
        if (ReadoutSettingsManager._instance == null)
          ReadoutSettingsManager._instance = new ReadoutSettingsManager();
        return ReadoutSettingsManager._instance;
      }
    }

    public static void Save() => ReadoutSettingsManager.Save("ReadoutSettings.xml");

    public static void Save(string fileName)
    {
      if (SystemValues.SettingsPath == null)
        return;
      lock (ReadoutSettingsManager.syncLock)
      {
        string path = Path.Combine(SystemValues.SettingsPath, fileName);
        using (FileStream fileStream = File.Create(path))
          new XmlSerializer(typeof (ReadoutSettingsManager)).Serialize((Stream) fileStream, (object) ReadoutSettingsManager.Instance);
      }
    }

    public static void SetSettings(int meterID, int readoutSettingsID, string settings)
    {
      if (meterID == 0)
        return;
      lock (ReadoutSettingsManager.syncLock)
      {
        List<ReadoutSettingsManager.ReadoutDeviceSettings> settings1 = ReadoutSettingsManager.Instance.Settings;
        if (settings1 == null)
          return;
        ReadoutSettingsManager.ReadoutDeviceSettings readoutDeviceSettings = settings1.Find((Predicate<ReadoutSettingsManager.ReadoutDeviceSettings>) (e => e.MeterID == meterID));
        if (readoutDeviceSettings != null)
        {
          readoutDeviceSettings.ReadoutSettingsID = readoutSettingsID;
          readoutDeviceSettings.Settings = settings;
        }
        else
          settings1.Add(new ReadoutSettingsManager.ReadoutDeviceSettings()
          {
            MeterID = meterID,
            ReadoutSettingsID = readoutSettingsID,
            Settings = settings
          });
      }
    }

    public static void Reload() => ReadoutSettingsManager.InternalLoad("ReadoutSettings.xml");

    public static void Load()
    {
      if (ReadoutSettingsManager._instance != null && ReadoutSettingsManager._instance.Settings.Count > 0)
        return;
      ReadoutSettingsManager.InternalLoad("ReadoutSettings.xml");
    }

    private static void InternalLoad(string fileName)
    {
      if (SystemValues.SettingsPath == null)
        return;
      string path = Path.Combine(SystemValues.SettingsPath, fileName);
      if (!File.Exists(path))
        return;
      lock (ReadoutSettingsManager.syncLock)
      {
        try
        {
          XmlSerializer xmlSerializer = new XmlSerializer(typeof (ReadoutSettingsManager));
          using (FileStream fileStream = File.OpenRead(path))
          {
            try
            {
              ReadoutSettingsManager._instance = xmlSerializer.Deserialize((Stream) fileStream) as ReadoutSettingsManager;
            }
            catch (Exception ex)
            {
              Debug.WriteLine("Wrong settings file detected! Remove it. Error: " + ex.Message);
              File.Delete(path);
            }
          }
        }
        catch (Exception ex)
        {
          Debug.WriteLine(ex.Message);
        }
      }
    }

    public sealed class ReadoutDeviceSettings
    {
      public int MeterID { get; set; }

      public string Settings { get; set; }

      public int ReadoutSettingsID { get; set; }
    }
  }
}
