// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.ConnectionSettings
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  [Serializable]
  public sealed class ConnectionSettings
  {
    public int ConnectionSettingsID { get; set; }

    public string Name { get; set; }

    public SortedList<string, string> SetupParameterList { get; set; }

    public List<string> ChangableDeviceParameters { get; set; }

    public List<string> ChangableEquipmentParameters { get; set; }

    public List<string> ChangableProfileTypeParameters { get; set; }

    public TransceiverType TransceiverType { get; set; }

    public SortedList<string, string> AllChangableParameters
    {
      get
      {
        if (this.SetupParameterList == null)
          throw new Exception("ConnectionSettings object not initialised");
        SortedList<string, string> changableParameters = new SortedList<string, string>();
        if (this.ChangableDeviceParameters != null)
        {
          foreach (string changableDeviceParameter in this.ChangableDeviceParameters)
          {
            if (!this.SetupParameterList.ContainsKey(changableDeviceParameter))
              throw new Exception("No setup for ChangableDeviceParameter: " + changableDeviceParameter);
            changableParameters.Add(changableDeviceParameter, this.SetupParameterList[changableDeviceParameter]);
          }
        }
        if (this.ChangableEquipmentParameters != null)
        {
          foreach (string equipmentParameter in this.ChangableEquipmentParameters)
          {
            if (!this.SetupParameterList.ContainsKey(equipmentParameter))
              throw new Exception("No setup for ChangableEquipmentParameters: " + equipmentParameter);
            if (changableParameters.ContainsKey(equipmentParameter))
              throw new Exception("Illegal initialising of ConnectionSettings object. Double changable parameter: " + equipmentParameter);
            changableParameters.Add(equipmentParameter, this.SetupParameterList[equipmentParameter]);
          }
        }
        if (this.ChangableProfileTypeParameters != null)
        {
          foreach (string profileTypeParameter in this.ChangableProfileTypeParameters)
          {
            if (!this.SetupParameterList.ContainsKey(profileTypeParameter))
              throw new Exception("No setup for ChangableProfileTypeParameters: " + profileTypeParameter);
            if (changableParameters.ContainsKey(profileTypeParameter))
              throw new Exception("Illegal initialising of ConnectionSettings object. Double changable parameter: " + profileTypeParameter);
            changableParameters.Add(profileTypeParameter, this.SetupParameterList[profileTypeParameter]);
          }
        }
        return changableParameters;
      }
    }

    public override string ToString()
    {
      return string.IsNullOrEmpty(this.Name) ? base.ToString() : this.Name;
    }

    public ConnectionSettings Clone()
    {
      ConnectionSettings connectionSettings = new ConnectionSettings();
      connectionSettings.ConnectionSettingsID = this.ConnectionSettingsID;
      connectionSettings.Name = this.Name;
      connectionSettings.SetupParameterList = new SortedList<string, string>();
      foreach (KeyValuePair<string, string> setupParameter in this.SetupParameterList)
        connectionSettings.SetupParameterList.Add(setupParameter.Key, setupParameter.Value);
      connectionSettings.ChangableDeviceParameters = this.ChangableDeviceParameters;
      connectionSettings.ChangableEquipmentParameters = this.ChangableEquipmentParameters;
      connectionSettings.ChangableProfileTypeParameters = this.ChangableProfileTypeParameters;
      connectionSettings.TransceiverType = this.TransceiverType;
      return connectionSettings;
    }
  }
}
