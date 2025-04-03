// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.ConnectionProfileAdjusted
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace ReadoutConfiguration
{
  public class ConnectionProfileAdjusted
  {
    private static Logger ConnectionProfileAdjustedLogger = LogManager.GetLogger(nameof (ConnectionProfileAdjusted));
    private ConfigList ConfigListObject = (ConfigList) null;

    public int ConnectionProfileID { get; private set; }

    public int SettingsID { get; private set; }

    internal SortedList<string, string> AdjustedParameters { get; private set; }

    internal SortedList<string, string> DefaultParameters { get; private set; }

    public ConnectionProfileAdjusted(int connectionProfileID)
    {
      this.BaseConstructor(connectionProfileID);
    }

    public ConnectionProfileAdjusted(ConnectionProfile connectionProfile)
    {
      if (connectionProfile == null)
        throw new ArgumentException("ConnectionProfile not defined");
      this.BaseConstructor(connectionProfile.ConnectionProfileID);
    }

    internal ConnectionProfileAdjusted(
      int connectionProfileID,
      ConnectionProfileAdjusted connectionProfileAdjustedBefore)
    {
      if (connectionProfileAdjustedBefore == null)
        throw new ArgumentException("ConnectionProfileAdjustedBefore not defined");
      if (connectionProfileAdjustedBefore.ConfigListObject != null)
        this.ConfigListObject = connectionProfileAdjustedBefore.ConfigListObject;
      this.BaseConstructor(connectionProfileID);
      foreach (KeyValuePair<string, string> adjustedParameter in connectionProfileAdjustedBefore.AdjustedParameters)
      {
        if ((!(adjustedParameter.Key != "Port") || ReadoutConfigFunctions.DbData.ChangableParameterByName[adjustedParameter.Key].ParameterUsing != ChangeableParameterUsings.standard) && this.DefaultParameters.ContainsKey(adjustedParameter.Key) && this.DefaultParameters[adjustedParameter.Key] != adjustedParameter.Value)
          this.AdjustedParameters.Add(adjustedParameter.Key, adjustedParameter.Value);
      }
      this.CheckAdjustedConfig();
    }

    public ConnectionProfileAdjusted(ConfigList configList)
    {
      if (configList == null)
        throw new ArgumentException("ConfigList not defined");
      this.BaseConstructor(configList.ConnectionProfileID);
      foreach (KeyValuePair<string, string> sorted in configList.GetSortedList())
      {
        if (this.DefaultParameters.ContainsKey(sorted.Key) && this.DefaultParameters[sorted.Key] != sorted.Value)
          this.ChangeParameter(sorted.Key, sorted.Value);
      }
      this.ConfigListObject = configList;
      this.CheckAdjustedConfig();
    }

    private void BaseConstructor(int connectionProfileID)
    {
      if (!ReadoutConfigFunctions.DbData.SettingsID_FromProfileID.ContainsKey(connectionProfileID))
        throw new ArgumentException("Unknown ConnectionProfileID:" + connectionProfileID.ToString());
      this.ConnectionProfileID = connectionProfileID;
      this.SettingsID = ReadoutConfigFunctions.DbData.SettingsID_FromProfileID[connectionProfileID];
      this.DefaultParameters = ReadoutConfigFunctions.DbData.CachedConnectionSettingsById[this.SettingsID].SetupParameterList;
      this.AdjustedParameters = new SortedList<string, string>();
    }

    public void ChangeParameter(ChangeableParameter changeableParameter)
    {
      this.ChangeParameter(changeableParameter.Key, changeableParameter.Value);
    }

    public void ChangeParameter(string parameterKey, string newValue)
    {
      if (parameterKey == "ConnectionProfileID")
        return;
      if (!this.DefaultParameters.ContainsKey(parameterKey))
        throw new ArgumentException("Changed parameter '" + parameterKey + "' is not part of profile" + this.ConnectionProfileID.ToString());
      if (this.DefaultParameters[parameterKey] == newValue && this.AdjustedParameters.ContainsKey(parameterKey))
        this.AdjustedParameters.Remove(parameterKey);
      if (this.AdjustedParameters.ContainsKey(parameterKey))
        this.AdjustedParameters[parameterKey] = newValue;
      else
        this.AdjustedParameters.Add(parameterKey, newValue);
    }

    public SortedList<string, string> GetAdjustedList()
    {
      SortedList<string, string> adjustedList = new SortedList<string, string>();
      foreach (KeyValuePair<string, string> defaultParameter in this.DefaultParameters)
      {
        if (this.AdjustedParameters.ContainsKey(defaultParameter.Key))
          adjustedList.Add(defaultParameter.Key, this.AdjustedParameters[defaultParameter.Key]);
        else
          adjustedList.Add(defaultParameter.Key, defaultParameter.Value);
      }
      adjustedList.Add("ConnectionProfileID", this.ConnectionProfileID.ToString());
      return adjustedList;
    }

    public void UpdateConfigList()
    {
      if (this.ConfigListObject == null)
        throw new Exception("ConfigList object not available");
      this.ConfigListObject.Reset(this.GetAdjustedList());
    }

    public ConfigList GetConfigList()
    {
      if (this.ConfigListObject != null)
        throw new Exception("Illegal ConfigList object using. Use UpdateConfigList() to change the ConfigList object deliverd by constructor instead.");
      return new ConfigList(this.GetAdjustedList())
      {
        ConnectionProfileID = this.ConnectionProfileID
      };
    }

    private void CheckAdjustedConfig()
    {
      ConnectionProfile connectionProfile = ReadoutConfigFunctions.DbData.CachedPartiallyConnectionProfiles.FirstOrDefault<ConnectionProfile>((Func<ConnectionProfile, bool>) (x => x.ConnectionProfileID == this.ConnectionProfileID));
      if (connectionProfile == null)
        throw new Exception("ConnectionProfile not found");
      string str1 = (string) null;
      string str2 = (string) null;
      if (this.AdjustedParameters.ContainsKey(ParameterKey.BusMode.ToString()))
      {
        str1 = this.AdjustedParameters[ParameterKey.BusMode.ToString()];
        if (connectionProfile.ConnectionSettings.SetupParameterList.ContainsKey(ParameterKey.BusMode.ToString()))
          str2 = connectionProfile.ConnectionSettings.SetupParameterList[ParameterKey.BusMode.ToString()];
      }
      string str3 = (string) null;
      string str4 = (string) null;
      if (this.AdjustedParameters.ContainsKey(ParameterKey.SelectedDeviceMBusType.ToString()))
      {
        str3 = this.AdjustedParameters[ParameterKey.SelectedDeviceMBusType.ToString()];
        if (connectionProfile.ConnectionSettings.SetupParameterList.ContainsKey(ParameterKey.SelectedDeviceMBusType.ToString()))
          str4 = connectionProfile.ConnectionSettings.SetupParameterList[ParameterKey.SelectedDeviceMBusType.ToString()];
      }
      if (!(str2 != str1) && !(str4 != str3))
        return;
      ConnectionProfileAdjusted.ConnectionProfileAdjustedLogger.Error("Illegal changes");
      this.AdjustedParameters.Clear();
    }
  }
}
