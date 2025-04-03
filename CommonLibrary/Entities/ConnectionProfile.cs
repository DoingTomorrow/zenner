// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.ConnectionProfile
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  [Serializable]
  public sealed class ConnectionProfile
  {
    private SortedList<ConnectionProfileParameter, string> combinedParameters = (SortedList<ConnectionProfileParameter, string>) null;

    public int ConnectionProfileID { get; set; }

    public EquipmentModel EquipmentModel { get; set; }

    public DeviceModel DeviceModel { get; set; }

    public ProfileType ProfileType { get; set; }

    public SortedList<ConnectionProfileParameter, string> Parameters { get; set; }

    public SortedList<ConnectionProfileParameter, string> CombinedParameters
    {
      get
      {
        if (this.combinedParameters != null)
          return this.combinedParameters;
        SortedList<ConnectionProfileParameter, string> combinedParameters = new SortedList<ConnectionProfileParameter, string>();
        if (this.Parameters != null)
        {
          foreach (KeyValuePair<ConnectionProfileParameter, string> parameter in this.Parameters)
            combinedParameters.Add(parameter.Key, parameter.Value);
        }
        if (this.DeviceModel.Parameters != null)
        {
          foreach (KeyValuePair<ConnectionProfileParameter, string> parameter in this.DeviceModel.Parameters)
          {
            if (!combinedParameters.ContainsKey(parameter.Key))
              combinedParameters.Add(parameter.Key, parameter.Value);
          }
        }
        if (this.EquipmentModel.Parameters != null)
        {
          foreach (KeyValuePair<ConnectionProfileParameter, string> parameter in this.EquipmentModel.Parameters)
          {
            if (!combinedParameters.ContainsKey(parameter.Key))
              combinedParameters.Add(parameter.Key, parameter.Value);
          }
        }
        if (this.ProfileType.Parameters != null)
        {
          foreach (KeyValuePair<ConnectionProfileParameter, string> parameter in this.ProfileType.Parameters)
          {
            if (!combinedParameters.ContainsKey(parameter.Key))
              combinedParameters.Add(parameter.Key, parameter.Value);
          }
        }
        this.combinedParameters = combinedParameters;
        return combinedParameters;
      }
    }

    public ConnectionSettings ConnectionSettings { get; set; }

    public string Name
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("#").Append(this.ConnectionProfileID.ToString()).Append(" ");
        if (this.DeviceModel != null)
          stringBuilder.Append(this.DeviceModel.Name).Append(" ");
        if (this.EquipmentModel != null)
          stringBuilder.Append(this.EquipmentModel.Name).Append(" ");
        if (this.ConnectionSettings != null)
          stringBuilder.Append((object) this.ProfileType);
        return stringBuilder.ToString();
      }
    }

    public override string ToString() => this.Name;

    public List<ChangeableParameter> ChangeableParameters
    {
      get
      {
        List<ChangeableParameter> changeableParameters = new List<ChangeableParameter>();
        if (this.DeviceModel != null && this.DeviceModel.ChangeableParameters != null)
          changeableParameters.AddRange((IEnumerable<ChangeableParameter>) this.DeviceModel.ChangeableParameters);
        if (this.EquipmentModel != null && this.EquipmentModel.ChangeableParameters != null)
          changeableParameters.AddRange((IEnumerable<ChangeableParameter>) this.EquipmentModel.ChangeableParameters);
        if (this.ProfileType != null && this.ProfileType.ChangeableParameters != null)
          changeableParameters.AddRange((IEnumerable<ChangeableParameter>) this.ProfileType.ChangeableParameters);
        return changeableParameters;
      }
    }

    public SortedList<string, string> GetSettingsList()
    {
      if (this.ConnectionSettings == null || this.ConnectionSettings.SetupParameterList == null)
        return (SortedList<string, string>) null;
      SortedList<string, string> settingsList = new SortedList<string, string>((IDictionary<string, string>) this.ConnectionSettings.SetupParameterList);
      List<ChangeableParameter> changeableParameters = this.ChangeableParameters;
      if (changeableParameters != null)
      {
        foreach (ChangeableParameter changeableParameter in changeableParameters)
        {
          if (settingsList.ContainsKey(changeableParameter.Key))
            settingsList[changeableParameter.Key] = changeableParameter.Value;
        }
      }
      return settingsList;
    }

    public ConfigList GetConfigListObject()
    {
      return new ConfigList(this.GetSettingsList())
      {
        ConnectionProfileID = this.ConnectionProfileID
      };
    }

    public string GetValue(string key)
    {
      SortedList<string, string> settingsList = this.GetSettingsList();
      return settingsList == null || !settingsList.ContainsKey(key) ? (string) null : settingsList[key];
    }

    public void SetValue(string key, string value)
    {
      List<ChangeableParameter> changeableParameters = this.ChangeableParameters;
      if (changeableParameters == null)
        return;
      ChangeableParameter changeableParameter = changeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == key));
      if (changeableParameter == null)
        throw new KeyNotFoundException(key);
      changeableParameter.Value = value;
    }

    public ConnectionProfile DeepCopy()
    {
      return new ConnectionProfile()
      {
        ConnectionProfileID = this.ConnectionProfileID,
        EquipmentModel = this.EquipmentModel.DeepCopy(),
        DeviceModel = this.DeviceModel.DeepCopy(),
        ProfileType = this.ProfileType.DeepCopy(),
        Parameters = this.Parameters,
        ConnectionSettings = this.ConnectionSettings
      };
    }
  }
}
