// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.ReadoutPreferences
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using StartupLib;
using System;
using System.Collections.Generic;
using System.Linq;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace ReadoutConfiguration
{
  [Serializable]
  public class ReadoutPreferences
  {
    internal List<ConnectionProfile> FilteredProfiles;
    internal ConnectionProfileAdjusted AdjustedProfile;
    internal ConnectionProfileFilter ProfileFilter;
    private bool isProfileEditingEnabled;

    public List<int> ChangeDeviceGroupsAllowed { get; set; }

    public List<int> ChangeDeviceModelsAllowed { get; set; }

    public List<int> ChangeEquipmentGroupsAllowed { get; set; }

    public List<int> ChangeEquipmentModelsAllowed { get; set; }

    public List<int> ChangeProfileTypeGroupsAllowed { get; set; }

    public List<int> ChangeProfileTypeModelsAllowed { get; set; }

    public List<int> AllDeviceGroupsIds
    {
      get
      {
        return ReadoutConfigFunctions.DbData.CachedDeviceGroups.Select<DeviceGroup, int>((Func<DeviceGroup, int>) (x => x.DeviceGroupID)).ToList<int>();
      }
    }

    public List<int> AllDeviceModelsIds
    {
      get
      {
        return ReadoutConfigFunctions.DbData.CachedDeviceModels.Select<DeviceModel, int>((Func<DeviceModel, int>) (x => x.DeviceModelID)).ToList<int>();
      }
    }

    public List<int> AllEquipmentGroupsIds
    {
      get
      {
        return ReadoutConfigFunctions.DbData.CachedEquipmentGroups.Select<EquipmentGroup, int>((Func<EquipmentGroup, int>) (x => x.EquipmentGroupID)).ToList<int>();
      }
    }

    public List<int> AllEquipmentModelsIds
    {
      get
      {
        return ReadoutConfigFunctions.DbData.CachedEquipmentModels.Select<EquipmentModel, int>((Func<EquipmentModel, int>) (x => x.EquipmentModelID)).ToList<int>();
      }
    }

    public List<int> AllProfilTypeGroupsIds
    {
      get
      {
        return ReadoutConfigFunctions.DbData.CachedProfileTypeGroups.Select<ProfileTypeGroup, int>((Func<ProfileTypeGroup, int>) (x => x.ProfileTypeGroupID)).ToList<int>();
      }
    }

    public List<int> AllProfilTypesIds
    {
      get
      {
        return ReadoutConfigFunctions.DbData.CachedProfileTypes.Select<ProfileType, int>((Func<ProfileType, int>) (x => x.ProfileTypeID)).ToList<int>();
      }
    }

    public bool IsProfileEditingEnabled
    {
      get => this.isProfileEditingEnabled;
      set
      {
        if (value && !UserManager.CheckPermission("Developer"))
          return;
        this.isProfileEditingEnabled = value;
      }
    }

    public ReadoutPreferences(ConnectionProfileAdjusted adjustedProfile)
    {
      this.AdjustedProfile = adjustedProfile;
      this.BaseConstructor();
    }

    public ReadoutPreferences(ConnectionProfile connectionProfile)
    {
      this.AdjustedProfile = new ConnectionProfileAdjusted(connectionProfile.ConnectionProfileID);
      this.BaseConstructor();
    }

    public ReadoutPreferences(ConfigList configList)
    {
      this.AdjustedProfile = new ConnectionProfileAdjusted(configList);
      this.BaseConstructor();
    }

    public ReadoutPreferences(ConfigList configList, ConnectionProfileFilter profileFilter)
    {
      if (profileFilter == null)
        throw new ArgumentNullException(nameof (profileFilter));
      this.AdjustedProfile = new ConnectionProfileAdjusted(configList);
      this.ProfileFilter = profileFilter;
      this.FilteredProfiles = ReadoutConfigFunctions.DbData.GetPartiallyConnectionProfiles(profileFilter);
      this.BaseConstructor();
    }

    private void BaseConstructor()
    {
      this.isProfileEditingEnabled = false;
      if (this.FilteredProfiles == null)
      {
        this.ChangeDeviceGroupsAllowed = this.AllDeviceGroupsIds;
        this.ChangeDeviceModelsAllowed = this.AllDeviceModelsIds;
        this.ChangeEquipmentGroupsAllowed = this.AllEquipmentGroupsIds;
        this.ChangeEquipmentModelsAllowed = this.AllEquipmentModelsIds;
        this.ChangeProfileTypeGroupsAllowed = this.AllProfilTypeGroupsIds;
        this.ChangeProfileTypeModelsAllowed = this.AllProfilTypesIds;
      }
      else
      {
        this.ChangeDeviceGroupsAllowed = new List<int>();
        this.ChangeDeviceModelsAllowed = new List<int>();
        this.ChangeEquipmentGroupsAllowed = new List<int>();
        this.ChangeEquipmentModelsAllowed = new List<int>();
        this.ChangeProfileTypeGroupsAllowed = new List<int>();
        this.ChangeProfileTypeModelsAllowed = new List<int>();
        foreach (ConnectionProfile filteredProfile in this.FilteredProfiles)
        {
          if (this.ChangeDeviceGroupsAllowed.IndexOf(filteredProfile.DeviceModel.GroupID) < 0)
            this.ChangeDeviceGroupsAllowed.Add(filteredProfile.DeviceModel.GroupID);
          if (this.ChangeDeviceModelsAllowed.IndexOf(filteredProfile.DeviceModel.DeviceModelID) < 0)
            this.ChangeDeviceModelsAllowed.Add(filteredProfile.DeviceModel.DeviceModelID);
          if (this.ChangeEquipmentGroupsAllowed.IndexOf(filteredProfile.EquipmentModel.GroupID) < 0)
            this.ChangeEquipmentGroupsAllowed.Add(filteredProfile.EquipmentModel.GroupID);
          if (this.ChangeEquipmentModelsAllowed.IndexOf(filteredProfile.EquipmentModel.EquipmentModelID) < 0)
            this.ChangeEquipmentModelsAllowed.Add(filteredProfile.EquipmentModel.EquipmentModelID);
          if (this.ChangeProfileTypeGroupsAllowed.IndexOf(filteredProfile.ProfileType.GroupID) < 0)
            this.ChangeProfileTypeGroupsAllowed.Add(filteredProfile.ProfileType.GroupID);
          if (this.ChangeProfileTypeModelsAllowed.IndexOf(filteredProfile.ProfileType.ProfileTypeID) < 0)
            this.ChangeProfileTypeModelsAllowed.Add(filteredProfile.ProfileType.ProfileTypeID);
        }
      }
    }

    public void UpdateConfigList() => this.AdjustedProfile.UpdateConfigList();

    public void GarantSelectedProfile()
    {
      if (!this.CheckIfProfileAllowed(this.AdjustedProfile.ConnectionProfileID))
      {
        foreach (ConnectionProfile connectionProfile in ReadoutConfigFunctions.DbData.CachedPartiallyConnectionProfiles)
        {
          if (this.CheckIfProfileAllowed(connectionProfile.ConnectionProfileID))
          {
            this.AdjustedProfile = new ConnectionProfileAdjusted(connectionProfile.ConnectionProfileID, this.AdjustedProfile);
            return;
          }
        }
        throw new Exception("Defined readout preference not possible for any profile");
      }
    }

    private bool CheckIfProfileAllowed(int connectionProfileID)
    {
      ConnectionProfile connectionProfile = ReadoutConfigFunctions.DbData.CachedPartiallyConnectionProfiles.FirstOrDefault<ConnectionProfile>((Func<ConnectionProfile, bool>) (x => x.ConnectionProfileID == connectionProfileID));
      return connectionProfile != null && (this.ChangeDeviceModelsAllowed == null || this.ChangeDeviceModelsAllowed.Contains(connectionProfile.DeviceModel.DeviceModelID)) && (this.ChangeEquipmentModelsAllowed == null || this.ChangeEquipmentModelsAllowed.Contains(connectionProfile.EquipmentModel.EquipmentModelID)) && (this.ChangeProfileTypeModelsAllowed == null || this.ChangeProfileTypeModelsAllowed.Contains(connectionProfile.ProfileType.ProfileTypeID));
    }

    public void ChangeToProfile(int connectionProfileID)
    {
      ConnectionProfileAdjusted connectionProfileAdjusted = new ConnectionProfileAdjusted(connectionProfileID, this.AdjustedProfile);
      ConnectionSettings connectionSettings = ReadoutConfigFunctions.DbData.CachedConnectionSettingsById[connectionProfileAdjusted.SettingsID];
      if (this.AdjustedProfile.AdjustedParameters.ContainsKey("Port") && connectionSettings.AllChangableParameters.ContainsKey("Port"))
        connectionProfileAdjusted.ChangeParameter("Port", this.AdjustedProfile.AdjustedParameters["Port"]);
      this.AdjustedProfile = connectionProfileAdjusted;
    }

    public void ReloadDatabaseAndEnableAllChanges()
    {
      ReadoutConfigFunctions.DbData.LoadAllConnectionTables();
      this.EnableAllChanges();
    }

    public void EnableAllChanges()
    {
      this.FilteredProfiles = ReadoutConfigFunctions.DbData.GetPartiallyConnectionProfiles();
      this.ChangeDeviceGroupsAllowed = this.AllDeviceGroupsIds;
      this.ChangeDeviceModelsAllowed = this.AllDeviceModelsIds;
      this.ChangeEquipmentGroupsAllowed = this.AllEquipmentGroupsIds;
      this.ChangeEquipmentModelsAllowed = this.AllEquipmentModelsIds;
      this.ChangeProfileTypeGroupsAllowed = this.AllProfilTypeGroupsIds;
      this.ChangeProfileTypeModelsAllowed = this.AllProfilTypesIds;
    }

    public void DisableBaseChanges()
    {
      this.ChangeDeviceGroupsAllowed = (List<int>) null;
      this.ChangeDeviceModelsAllowed = (List<int>) null;
      this.ChangeEquipmentGroupsAllowed = (List<int>) null;
      this.ChangeEquipmentModelsAllowed = (List<int>) null;
      this.ChangeProfileTypeGroupsAllowed = (List<int>) null;
      this.ChangeProfileTypeModelsAllowed = (List<int>) null;
    }

    public static ConfigList GetConfigListFromProfileId(int connectionProfileID)
    {
      if (!ReadoutConfigFunctions.DbData.SettingsID_FromProfileID.ContainsKey(connectionProfileID))
        throw new ArgumentException("Unknown ConnectionProfileID:" + connectionProfileID.ToString());
      return new ConfigList(ReadoutConfigFunctions.DbData.CachedConnectionSettingsById[ReadoutConfigFunctions.DbData.SettingsID_FromProfileID[connectionProfileID]].SetupParameterList)
      {
        ConnectionProfileID = connectionProfileID
      };
    }
  }
}
