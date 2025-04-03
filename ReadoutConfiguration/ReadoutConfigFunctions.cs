// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.ReadoutConfigFunctions
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace ReadoutConfiguration
{
  public sealed class ReadoutConfigFunctions
  {
    private static Logger logger = LogManager.GetLogger(nameof (ReadoutConfigFunctions));
    private static ConfigDatabaseAccess theOnlyConfigDatabaseAccess;
    internal static DeviceManager theOnlyDeviceManager;
    internal bool IsPluginObject = false;

    internal static ConfigDatabaseAccess DbData
    {
      get
      {
        if (ReadoutConfigFunctions.theOnlyConfigDatabaseAccess == null)
          ReadoutConfigFunctions.theOnlyConfigDatabaseAccess = new ConfigDatabaseAccess();
        return ReadoutConfigFunctions.theOnlyConfigDatabaseAccess;
      }
    }

    public static DeviceManager Manager
    {
      get
      {
        if (ReadoutConfigFunctions.theOnlyDeviceManager == null)
          ReadoutConfigFunctions.theOnlyDeviceManager = new DeviceManager();
        return ReadoutConfigFunctions.theOnlyDeviceManager;
      }
    }

    public static void Dispose()
    {
      ReadoutConfigFunctions.theOnlyConfigDatabaseAccess = (ConfigDatabaseAccess) null;
      ReadoutConfigFunctions.theOnlyDeviceManager = (DeviceManager) null;
    }

    public static bool ChooseConfiguration(ReadoutPreferences readoutPreferences)
    {
      if (readoutPreferences == null)
        throw new ArgumentNullException("ReadoutPreferences not defined");
      if (!new ReadoutConfigMain(readoutPreferences, false).ShowDialog().Value)
        return false;
      readoutPreferences.UpdateConfigList();
      return true;
    }

    public static bool ChooseConfiguration(ConfigList configList)
    {
      ReadoutPreferences readoutPreferences = configList != null ? new ReadoutPreferences(configList) : throw new ArgumentNullException("ConfigList not defined");
      readoutPreferences.IsProfileEditingEnabled = UserManager.CheckPermission("Developer");
      if (!new ReadoutConfigMain(readoutPreferences, false).ShowDialog().Value)
        return false;
      readoutPreferences.UpdateConfigList();
      return true;
    }

    public static ConnectionProfileIdentification GetConnectionProfileIdentification(int profileID)
    {
      return new ConnectionProfileIdentification(profileID);
    }

    private static void PrepareChangeInfo(
      string pluginName,
      string parameterName,
      string newValue,
      string oldValue,
      SortedList<string, string> changeInfo)
    {
      string str1 = pluginName + ":" + newValue;
      int index = changeInfo.IndexOfKey(parameterName);
      if (index < 0)
      {
        changeInfo.Add(parameterName, str1 + "; before:" + oldValue);
      }
      else
      {
        string str2 = pluginName + ":" + newValue + "; " + changeInfo.Values[index];
        changeInfo.RemoveAt(index);
        changeInfo.Add(parameterName, str2);
      }
    }

    public static SortedList<string, ConfigList> GetAllConfigurations()
    {
      SortedList<string, ConfigList> allConfigurations = new SortedList<string, ConfigList>();
      try
      {
        foreach (LoadedPlugin loadedPlugin in PlugInLoader.loadedPlugins)
        {
          if (loadedPlugin.gmmPlugIn is IReadoutConfig)
          {
            ConfigList readoutConfiguration = ((IReadoutConfig) loadedPlugin.gmmPlugIn).GetReadoutConfiguration();
            if (readoutConfiguration != null)
              allConfigurations.Add(loadedPlugin.Name, readoutConfiguration);
          }
        }
      }
      catch (Exception ex)
      {
        string message = "Exception on load IReadoutConfig ";
        ReadoutConfigFunctions.logger.Error(message);
        ReadoutConfigFunctions.logger.Error(ex.ToString());
        throw new Exception(message, ex);
      }
      return allConfigurations;
    }

    public static HashSet<string> GetAllLoadedConfiguratedPlugins()
    {
      HashSet<string> configuratedPlugins = new HashSet<string>();
      try
      {
        foreach (LoadedPlugin loadedPlugin in PlugInLoader.loadedPlugins)
        {
          if (loadedPlugin.gmmPlugIn is IReadoutConfig)
            configuratedPlugins.Add(loadedPlugin.Name);
        }
      }
      catch (Exception ex)
      {
        string message = "Exception on load IReadoutConfig ";
        ReadoutConfigFunctions.logger.Error(message);
        ReadoutConfigFunctions.logger.Error(ex.ToString());
        throw new Exception(message, ex);
      }
      return configuratedPlugins;
    }

    public static ConnectionProfile GetPartialProfile(int profileID)
    {
      return ReadoutConfigFunctions.DbData.CachedPartiallyConnectionProfiles.FirstOrDefault<ConnectionProfile>((Func<ConnectionProfile, bool>) (x => x.ConnectionProfileID == profileID));
    }

    internal string ShowMainWindow(Window owner = null)
    {
      ConfigList configList;
      if (PlugInLoader.ConfigListStatic == null)
      {
        configList = ReadoutPreferences.GetConfigListFromProfileId(ReadoutConfigFunctions.DbData.SettingsID_FromProfileID.Keys[0]);
      }
      else
      {
        if (!ReadoutConfigFunctions.DbData.SettingsID_FromProfileID.ContainsKey(PlugInLoader.ConfigListStatic.ConnectionProfileID))
          PlugInLoader.ConfigListStatic.ConnectionProfileID = ReadoutConfigFunctions.DbData.SettingsID_FromProfileID.Keys[0];
        configList = PlugInLoader.ConfigListStatic;
      }
      ConnectionProfileAdjusted adjustedProfile = new ConnectionProfileAdjusted(configList);
      ReadoutPreferences readoutPreferences = new ReadoutPreferences(adjustedProfile);
      readoutPreferences.IsProfileEditingEnabled = UserManager.CheckPermission("Developer");
      readoutPreferences.EnableAllChanges();
      ReadoutConfigMain readoutConfigMain = new ReadoutConfigMain(readoutPreferences, this.IsPluginObject && PlugInLoader.IsWindowEnabled("GMM"));
      readoutConfigMain.Owner = owner;
      if (readoutConfigMain.ShowDialog().Value)
      {
        SortedList<string, string> resultSetup = readoutConfigMain.GetResultSetup();
        configList.Reset(resultSetup);
        if (this.IsPluginObject)
        {
          List<IReadoutConfig> readoutConfigList = new List<IReadoutConfig>();
          try
          {
            foreach (LoadedPlugin loadedPlugin in PlugInLoader.loadedPlugins)
            {
              if (loadedPlugin.gmmPlugIn is IReadoutConfig)
                readoutConfigList.Add(loadedPlugin.gmmPlugIn as IReadoutConfig);
            }
          }
          catch (Exception ex)
          {
            string message = "Exception on load IReadoutConfig ";
            ReadoutConfigFunctions.logger.Error(message);
            ReadoutConfigFunctions.logger.Error(ex.ToString());
            throw new Exception(message, ex);
          }
          if (readoutConfigList != null && adjustedProfile != null)
          {
            foreach (IReadoutConfig readoutConfig in readoutConfigList)
              readoutConfig.SetReadoutConfiguration(configList);
          }
        }
      }
      return readoutConfigMain.nextComponent;
    }

    public void GMM_Dispose()
    {
    }
  }
}
