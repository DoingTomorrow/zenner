// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.DeviceManager
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using PlugInLib;
using System;
using System.Collections.Generic;
using System.Linq;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace ReadoutConfiguration
{
  public sealed class DeviceManager
  {
    internal ConnectionProfileFilter selectedFilterObject;
    internal List<ConnectionProfile> FilteredConnectionProfiles;
    internal List<EquipmentGroup> equipmentGroups;
    internal List<EquipmentModel> equipmentModels;
    internal List<DeviceGroup> deviceGroups;
    internal List<DeviceModel> deviceModels;
    private bool fullConfiguration = false;

    public event EventHandler<string> OnProfileFilterChanged;

    static DeviceManager()
    {
      LicenseManager.LicenseChanged += new System.EventHandler(DeviceManager.LicenseManager_LicenseChanged);
    }

    private static void LicenseManager_LicenseChanged(object sender, EventArgs e)
    {
      DeviceManager.Dispose();
    }

    public DeviceManager() => this.SelectedFilter = (string) null;

    public ConnectionProfileFilter SelectedFilterObject
    {
      get => this.selectedFilterObject;
      set
      {
        this.selectedFilterObject = !this.GetFilterList().Contains(value.Name) ? value : throw new Exception("This filter name '" + value.Name + "' is a predefined fileter name. Please set a different name for customised filters!");
        this.LoadFilterdProfiles();
      }
    }

    public List<string> GetFilterList(
      SortedList<ConnectionProfileParameter, string> filtersFor = null)
    {
      return ReadoutConfigFunctions.DbData.GetFilterList(filtersFor);
    }

    public string SelectedFilter
    {
      get => this.selectedFilterObject == null ? (string) null : this.selectedFilterObject.Name;
      set
      {
        if (this.FilteredConnectionProfiles != null && (value == null && this.selectedFilterObject == null || this.selectedFilterObject != null && this.selectedFilterObject.Name == value))
          return;
        this.LoadFilterdProfiles(value);
      }
    }

    public bool FullConfiguration
    {
      get => this.fullConfiguration;
      set
      {
        if (value == this.fullConfiguration)
          return;
        this.fullConfiguration = value;
        this.LoadFilterdProfiles(this.SelectedFilter);
      }
    }

    internal void LoadFilterdProfiles(string filterName)
    {
      ConfigDatabaseAccess dbData = ReadoutConfigFunctions.DbData;
      if (filterName == null)
      {
        this.selectedFilterObject = (ConnectionProfileFilter) null;
      }
      else
      {
        if (!dbData.CachedProfileFilters.ContainsKey(filterName))
          throw new Exception("Unknown profile filter: " + filterName);
        this.selectedFilterObject = dbData.CachedProfileFilters[filterName];
      }
      this.LoadFilterdProfiles();
    }

    internal void LoadFilterdProfiles()
    {
      this.FilteredConnectionProfiles = ReadoutConfigFunctions.DbData.GetConnectionProfiles(this.selectedFilterObject, this.fullConfiguration);
      if (this.OnProfileFilterChanged != null && this.selectedFilterObject != null)
        this.OnProfileFilterChanged((object) this, this.selectedFilterObject.Name);
      this.equipmentGroups = new List<EquipmentGroup>();
      this.equipmentModels = new List<EquipmentModel>();
      this.deviceGroups = new List<DeviceGroup>();
      this.deviceModels = new List<DeviceModel>();
      foreach (ConnectionProfile connectionProfile in this.FilteredConnectionProfiles)
      {
        ConnectionProfile theProfile = connectionProfile;
        if (this.deviceGroups.FirstOrDefault<DeviceGroup>((Func<DeviceGroup, bool>) (x => x.DeviceGroupID == theProfile.DeviceModel.DeviceGroup.DeviceGroupID)) == null)
          this.deviceGroups.Add(theProfile.DeviceModel.DeviceGroup);
        if (this.equipmentGroups.FirstOrDefault<EquipmentGroup>((Func<EquipmentGroup, bool>) (x => x.EquipmentGroupID == theProfile.EquipmentModel.EquipmentGroup.EquipmentGroupID)) == null)
          this.equipmentGroups.Add(theProfile.EquipmentModel.EquipmentGroup);
        DeviceModel deviceModel = this.deviceModels.FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (x => x.DeviceModelID == theProfile.DeviceModel.DeviceModelID));
        if (deviceModel == null)
          this.deviceModels.Add(theProfile.DeviceModel.DeepCopy());
        else if (theProfile.DeviceModel.ChangeableParameters != null)
        {
          foreach (ChangeableParameter changeableParameter1 in theProfile.DeviceModel.ChangeableParameters)
          {
            ChangeableParameter configurationParameter = changeableParameter1;
            ChangeableParameter changeableParameter2 = (ChangeableParameter) null;
            if (deviceModel.ChangeableParameters == null)
              deviceModel.ChangeableParameters = new List<ChangeableParameter>();
            else
              changeableParameter2 = deviceModel.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == configurationParameter.Key));
            if (changeableParameter2 == null)
              deviceModel.ChangeableParameters.Add(configurationParameter.DeepCopy());
          }
        }
        EquipmentModel equipmentModel = this.equipmentModels.FirstOrDefault<EquipmentModel>((Func<EquipmentModel, bool>) (x => x.EquipmentModelID == theProfile.EquipmentModel.EquipmentModelID));
        if (equipmentModel == null)
          this.equipmentModels.Add(theProfile.EquipmentModel.DeepCopy());
        else if (theProfile.EquipmentModel.ChangeableParameters != null)
        {
          foreach (ChangeableParameter changeableParameter3 in theProfile.EquipmentModel.ChangeableParameters)
          {
            ChangeableParameter configurationParameter = changeableParameter3;
            ChangeableParameter changeableParameter4 = (ChangeableParameter) null;
            if (equipmentModel.ChangeableParameters == null)
              equipmentModel.ChangeableParameters = new List<ChangeableParameter>();
            else
              changeableParameter4 = equipmentModel.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == configurationParameter.Key));
            if (changeableParameter4 == null)
              equipmentModel.ChangeableParameters.Add(configurationParameter.DeepCopy());
          }
        }
      }
    }

    private List<ConnectionProfile> GetChoosedProfiles(
      TransceiverType type,
      DeviceModelTags? dTags,
      ProfileTypeTags? tTags)
    {
      return this.GetChoosedProfiles(this.FilteredConnectionProfiles, type, dTags, tTags);
    }

    private List<ConnectionProfile> GetChoosedProfiles(
      List<ConnectionProfile> inputProfiles,
      TransceiverType type,
      DeviceModelTags? dTags,
      ProfileTypeTags? tTags)
    {
      string str = (string) null;
      if (type != 0)
      {
        switch (type)
        {
          case TransceiverType.Listener:
            str = "Listener";
            break;
          case TransceiverType.Reader:
            str = "Reader";
            break;
          case TransceiverType.Receiver:
            str = "Receiver";
            break;
          default:
            throw new Exception("Unknown TransceiverType");
        }
      }
      else if (!dTags.HasValue && !tTags.HasValue)
        return inputProfiles;
      List<ConnectionProfileParameter> profileParameterList1 = (List<ConnectionProfileParameter>) null;
      bool flag1 = false;
      if (dTags.HasValue)
      {
        DeviceModelTags? nullable1 = dTags;
        DeviceModelTags deviceModelTags1 = DeviceModelTags.Undefined | DeviceModelTags.Radio2 | DeviceModelTags.Radio3 | DeviceModelTags.MBus | DeviceModelTags.wMBus | DeviceModelTags.LoRa;
        if (nullable1.GetValueOrDefault() == deviceModelTags1 & nullable1.HasValue)
        {
          flag1 = true;
        }
        else
        {
          profileParameterList1 = new List<ConnectionProfileParameter>();
          DeviceModelTags? nullable2 = dTags;
          nullable1 = nullable2.HasValue ? new DeviceModelTags?(nullable2.GetValueOrDefault() & DeviceModelTags.LoRa) : new DeviceModelTags?();
          DeviceModelTags deviceModelTags2 = DeviceModelTags.None;
          if (!(nullable1.GetValueOrDefault() == deviceModelTags2 & nullable1.HasValue))
            profileParameterList1.Add(ConnectionProfileParameter.LoRa);
          nullable2 = dTags;
          nullable1 = nullable2.HasValue ? new DeviceModelTags?(nullable2.GetValueOrDefault() & DeviceModelTags.Radio2) : new DeviceModelTags?();
          DeviceModelTags deviceModelTags3 = DeviceModelTags.None;
          if (!(nullable1.GetValueOrDefault() == deviceModelTags3 & nullable1.HasValue))
            profileParameterList1.Add(ConnectionProfileParameter.Radio2);
          nullable2 = dTags;
          nullable1 = nullable2.HasValue ? new DeviceModelTags?(nullable2.GetValueOrDefault() & DeviceModelTags.Radio3) : new DeviceModelTags?();
          DeviceModelTags deviceModelTags4 = DeviceModelTags.None;
          if (!(nullable1.GetValueOrDefault() == deviceModelTags4 & nullable1.HasValue))
            profileParameterList1.Add(ConnectionProfileParameter.Radio3);
          nullable2 = dTags;
          nullable1 = nullable2.HasValue ? new DeviceModelTags?(nullable2.GetValueOrDefault() & DeviceModelTags.MBus) : new DeviceModelTags?();
          DeviceModelTags deviceModelTags5 = DeviceModelTags.None;
          if (!(nullable1.GetValueOrDefault() == deviceModelTags5 & nullable1.HasValue))
            profileParameterList1.Add(ConnectionProfileParameter.MBus);
          nullable2 = dTags;
          nullable1 = nullable2.HasValue ? new DeviceModelTags?(nullable2.GetValueOrDefault() & DeviceModelTags.wMBus) : new DeviceModelTags?();
          DeviceModelTags deviceModelTags6 = DeviceModelTags.None;
          if (!(nullable1.GetValueOrDefault() == deviceModelTags6 & nullable1.HasValue))
            profileParameterList1.Add(ConnectionProfileParameter.wMBus);
          nullable2 = dTags;
          nullable1 = nullable2.HasValue ? new DeviceModelTags?(nullable2.GetValueOrDefault() & DeviceModelTags.SystemDevice) : new DeviceModelTags?();
          DeviceModelTags deviceModelTags7 = DeviceModelTags.None;
          if (!(nullable1.GetValueOrDefault() == deviceModelTags7 & nullable1.HasValue))
            profileParameterList1.Add(ConnectionProfileParameter.SystemDevice);
        }
      }
      List<ConnectionProfileParameter> profileParameterList2 = (List<ConnectionProfileParameter>) null;
      if (tTags.HasValue)
      {
        profileParameterList2 = new List<ConnectionProfileParameter>();
        ProfileTypeTags? nullable3 = tTags;
        ProfileTypeTags? nullable4 = nullable3.HasValue ? new ProfileTypeTags?(nullable3.GetValueOrDefault() & ProfileTypeTags.JobManager) : new ProfileTypeTags?();
        ProfileTypeTags profileTypeTags1 = ProfileTypeTags.None;
        if (!(nullable4.GetValueOrDefault() == profileTypeTags1 & nullable4.HasValue))
          profileParameterList2.Add(ConnectionProfileParameter.JobManager);
        nullable3 = tTags;
        nullable4 = nullable3.HasValue ? new ProfileTypeTags?(nullable3.GetValueOrDefault() & ProfileTypeTags.Scanning) : new ProfileTypeTags?();
        ProfileTypeTags profileTypeTags2 = ProfileTypeTags.None;
        if (!(nullable4.GetValueOrDefault() == profileTypeTags2 & nullable4.HasValue))
          profileParameterList2.Add(ConnectionProfileParameter.Scanning);
      }
      List<ConnectionProfile> choosedProfiles = new List<ConnectionProfile>();
      foreach (ConnectionProfile inputProfile in inputProfiles)
      {
        if (str == null || inputProfile.CombinedParameters.ContainsKey(ConnectionProfileParameter.TransceiverType) && !(inputProfile.CombinedParameters[ConnectionProfileParameter.TransceiverType] != str))
        {
          if (profileParameterList1 != null)
          {
            bool flag2 = true;
            foreach (ConnectionProfileParameter key in profileParameterList1)
            {
              if (inputProfile.CombinedParameters.ContainsKey(key))
              {
                flag2 = false;
                break;
              }
            }
            if (flag2)
              continue;
          }
          if (profileParameterList2 != null)
          {
            bool flag3 = true;
            foreach (ConnectionProfileParameter key in profileParameterList2)
            {
              if (inputProfile.CombinedParameters.ContainsKey(key))
              {
                flag3 = false;
                break;
              }
            }
            if (flag3)
              continue;
          }
          if (!flag1 || !inputProfile.CombinedParameters.ContainsKey(ConnectionProfileParameter.SystemDevice))
            choosedProfiles.Add(inputProfile);
        }
      }
      return choosedProfiles;
    }

    public List<DeviceGroup> GetDeviceGroups()
    {
      List<ConnectionProfile> profiles = this.GetConnectionProfiles();
      return profiles == null ? (List<DeviceGroup>) null : this.deviceGroups.FindAll((Predicate<DeviceGroup>) (x => profiles.Exists((Predicate<ConnectionProfile>) (y => x.DeviceGroupID == y.DeviceModel.DeviceGroup.DeviceGroupID))));
    }

    public List<DeviceModel> GetDeviceModels()
    {
      return this.GetDeviceModels(TransceiverType.None, (EquipmentModel) null, new DeviceModelTags?());
    }

    public List<DeviceModel> GetDeviceModels(DeviceModelTags tags)
    {
      return this.GetDeviceModels(TransceiverType.None, (EquipmentModel) null, new DeviceModelTags?(tags));
    }

    public List<DeviceModel> GetDeviceModels(ConnectionProfileParameter requiredDeviceParameter)
    {
      return this.GetDeviceModels(new List<ConnectionProfileParameter>()
      {
        requiredDeviceParameter
      });
    }

    public List<DeviceModel> GetDeviceModels(
      List<ConnectionProfileParameter> requiredDeviceParameters)
    {
      List<DeviceModel> source = new List<DeviceModel>();
      foreach (ConnectionProfile connectionProfile in this.FilteredConnectionProfiles)
      {
        ConnectionProfile theProfile = connectionProfile;
        if (source.FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (x => x.DeviceModelID == theProfile.DeviceModel.DeviceModelID)) != null)
          source.Add(theProfile.DeviceModel);
      }
      List<DeviceModel> deviceModels = new List<DeviceModel>();
      foreach (DeviceModel deviceModel in deviceModels)
      {
        bool flag = true;
        foreach (ConnectionProfileParameter requiredDeviceParameter in requiredDeviceParameters)
        {
          if (!deviceModel.Parameters.ContainsKey(requiredDeviceParameter))
          {
            flag = false;
            break;
          }
        }
        if (flag)
          deviceModels.Add(deviceModel.DeepCopy());
      }
      return deviceModels;
    }

    public List<DeviceModel> GetDeviceModels(EquipmentModel equipment, DeviceModelTags? tags)
    {
      return this.GetDeviceModels(TransceiverType.None, equipment, tags);
    }

    public List<DeviceModel> GetDeviceModels(DeviceModelTags tags, TransceiverType type)
    {
      return this.GetDeviceModels(type, (EquipmentModel) null, new DeviceModelTags?(tags));
    }

    public List<DeviceModel> GetDeviceModels(DeviceGroup deviceGroup)
    {
      List<DeviceModel> deviceModels = this.GetDeviceModels();
      if (deviceGroup == null)
        return deviceModels;
      return deviceModels?.FindAll((Predicate<DeviceModel>) (x => x.DeviceGroup.DeviceGroupID == deviceGroup.DeviceGroupID));
    }

    public List<DeviceModel> GetDeviceModels(TransceiverType type, EquipmentModel equipment)
    {
      return this.GetDeviceModels(type, equipment, new DeviceModelTags?());
    }

    public List<DeviceModel> GetDeviceModels(EquipmentModel equipment)
    {
      return this.GetDeviceModels(TransceiverType.None, equipment, new DeviceModelTags?());
    }

    public List<DeviceModel> GetDeviceModels(TransceiverType transceiverType)
    {
      return this.GetDeviceModels(transceiverType, (EquipmentModel) null, new DeviceModelTags?());
    }

    public DeviceModel GetDeviceModel(string name)
    {
      if (string.IsNullOrEmpty(name))
        return (DeviceModel) null;
      return this.GetDeviceModels()?.Find((Predicate<DeviceModel>) (x => x.Name == name));
    }

    public DeviceModel GetDeviceModel(int deviceModelID)
    {
      return this.GetDeviceModels()?.Find((Predicate<DeviceModel>) (x => x.DeviceModelID == deviceModelID));
    }

    public List<DeviceModel> GetDeviceModels(
      TransceiverType type,
      EquipmentModel equipment,
      DeviceModelTags? tags)
    {
      List<DeviceModel> source = new List<DeviceModel>();
      foreach (ConnectionProfile choosedProfile in this.GetChoosedProfiles(type, tags, new ProfileTypeTags?()))
      {
        ConnectionProfile theProfile = choosedProfile;
        if ((equipment == null || theProfile.EquipmentModel.EquipmentModelID == equipment.EquipmentModelID) && source.FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (x => x.DeviceModelID == theProfile.DeviceModel.DeviceModelID)) == null)
          source.Add(theProfile.DeviceModel.DeepCopy());
      }
      return source;
    }

    public List<DeviceModel> GetDeviceModels(EquipmentModel equipmentModel, ProfileType profileType)
    {
      if (equipmentModel == null)
        throw new ArgumentNullException(nameof (equipmentModel));
      if (profileType == null)
        throw new ArgumentNullException(nameof (profileType));
      List<DeviceModel> source = new List<DeviceModel>();
      foreach (ConnectionProfile connectionProfile in this.FilteredConnectionProfiles)
      {
        ConnectionProfile theProfile = connectionProfile;
        if (theProfile.EquipmentModel.EquipmentModelID == equipmentModel.EquipmentModelID && theProfile.ProfileType.ProfileTypeID == profileType.ProfileTypeID && source.FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (x => x.DeviceModelID == theProfile.DeviceModel.DeviceModelID)) == null)
          source.Add(theProfile.DeviceModel.DeepCopy());
      }
      return source;
    }

    public List<DeviceModel> GetDeviceModels(ProfileType profileType)
    {
      if (profileType == null)
        throw new ArgumentNullException(nameof (profileType));
      List<DeviceModel> source = new List<DeviceModel>();
      foreach (ConnectionProfile connectionProfile in this.FilteredConnectionProfiles)
      {
        ConnectionProfile theProfile = connectionProfile;
        if (theProfile.ProfileType.ProfileTypeID == profileType.ProfileTypeID && source.FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (x => x.DeviceModelID == theProfile.DeviceModel.DeviceModelID)) == null)
          source.Add(theProfile.DeviceModel.DeepCopy());
      }
      return source;
    }

    public List<ConnectionProfile> GetConnectionProfiles(DeviceModel model)
    {
      return this.GetConnectionProfiles(model, (EquipmentModel) null);
    }

    public List<ConnectionProfile> GetConnectionProfiles(
      DeviceModel model,
      EquipmentModel equipment)
    {
      if (model == null)
        return this.GetConnectionProfiles(equipment);
      List<ConnectionProfile> connectionProfiles = this.GetConnectionProfiles();
      if (connectionProfiles == null)
        return (List<ConnectionProfile>) null;
      if (model != null && equipment != null)
        return connectionProfiles.FindAll((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceModelID == model.DeviceModelID && x.EquipmentModel.EquipmentModelID == equipment.EquipmentModelID));
      if (model != null)
        return connectionProfiles.FindAll((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceModelID == model.DeviceModelID));
      return equipment != null ? connectionProfiles.FindAll((Predicate<ConnectionProfile>) (x => x.EquipmentModel.EquipmentModelID == equipment.EquipmentModelID)) : connectionProfiles;
    }

    public List<ConnectionProfile> GetConnectionProfiles(
      DeviceModel model,
      EquipmentModel equipment,
      TransceiverType type)
    {
      List<ConnectionProfile> choosedProfiles = this.GetChoosedProfiles(type, new DeviceModelTags?(), new ProfileTypeTags?());
      if (choosedProfiles == null)
        return (List<ConnectionProfile>) null;
      if (model != null && equipment != null)
        return choosedProfiles.FindAll((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceModelID == model.DeviceModelID && x.EquipmentModel.EquipmentModelID == equipment.EquipmentModelID));
      if (model != null)
        return choosedProfiles.FindAll((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceModelID == model.DeviceModelID));
      return equipment != null ? choosedProfiles.FindAll((Predicate<ConnectionProfile>) (x => x.EquipmentModel.EquipmentModelID == equipment.EquipmentModelID)) : choosedProfiles;
    }

    private List<ConnectionProfile> GetConnectionProfiles(
      TransceiverType type,
      EquipmentModel equipment)
    {
      if (equipment == null)
        return this.GetConnectionProfiles(type);
      return this.GetChoosedProfiles(type, new DeviceModelTags?(), new ProfileTypeTags?())?.FindAll((Predicate<ConnectionProfile>) (x => x.EquipmentModel.EquipmentModelID == equipment.EquipmentModelID));
    }

    private List<ConnectionProfile> GetConnectionProfiles(TransceiverType type, DeviceModel model)
    {
      List<ConnectionProfile> connectionProfiles = this.GetConnectionProfiles(model);
      if (connectionProfiles == null)
        return (List<ConnectionProfile>) null;
      return type == TransceiverType.None ? connectionProfiles : connectionProfiles.FindAll((Predicate<ConnectionProfile>) (x => x.ConnectionSettings.TransceiverType == type));
    }

    private List<ConnectionProfile> GetConnectionProfiles(EquipmentModel equipment)
    {
      if (equipment == null)
        return this.GetConnectionProfiles();
      return this.GetConnectionProfiles()?.FindAll((Predicate<ConnectionProfile>) (x => x.EquipmentModel.EquipmentModelID == equipment.EquipmentModelID));
    }

    public List<ConnectionProfile> GetConnectionProfiles(TransceiverType type)
    {
      return this.GetChoosedProfiles(type, new DeviceModelTags?(), new ProfileTypeTags?());
    }

    public List<ConnectionProfile> GetConnectionProfiles() => this.FilteredConnectionProfiles;

    public ConnectionProfile GetConnectionProfile(int connectionProfileID)
    {
      return this.FilteredConnectionProfiles.Find((Predicate<ConnectionProfile>) (x => x.ConnectionProfileID == connectionProfileID))?.DeepCopy();
    }

    public ConnectionProfile GetConnectionProfile(
      DeviceModel deviceModel,
      EquipmentModel equipmentModel,
      ProfileType profileType)
    {
      if (deviceModel == null)
        throw new NullReferenceException(nameof (deviceModel));
      if (equipmentModel == null)
        throw new NullReferenceException(nameof (equipmentModel));
      if (profileType == null)
        throw new NullReferenceException(nameof (profileType));
      return this.GetConnectionProfiles()?.Find((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceModelID == deviceModel.DeviceModelID && x.EquipmentModel.EquipmentModelID == equipmentModel.EquipmentModelID && x.ProfileType.Name == profileType.Name))?.DeepCopy();
    }

    public ConnectionAdjuster GetConnectionAdjuster(
      DeviceModel deviceModel,
      EquipmentModel equipmentModel,
      ProfileType profileType)
    {
      if (deviceModel == null)
        throw new NullReferenceException(nameof (deviceModel));
      if (equipmentModel == null)
        throw new NullReferenceException(nameof (equipmentModel));
      if (profileType == null)
        throw new NullReferenceException(nameof (profileType));
      List<ConnectionProfile> connectionProfiles = this.GetConnectionProfiles();
      if (connectionProfiles == null)
        return (ConnectionAdjuster) null;
      ConnectionProfile connectionProfile = connectionProfiles.Find((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceModelID == deviceModel.DeviceModelID && x.EquipmentModel.EquipmentModelID == equipmentModel.EquipmentModelID && x.ProfileType.Name == profileType.Name));
      if (connectionProfile == null)
        throw new NullReferenceException("profile not selected by defined filter");
      List<ChangeableParameter> changeableParameterList = new List<ChangeableParameter>();
      foreach (ChangeableParameter changeableParameter in connectionProfile.ChangeableParameters)
        changeableParameterList.Add(changeableParameter.DeepCopy());
      this.IncludePresettings(changeableParameterList, deviceModel.ChangeableParameters);
      this.IncludePresettings(changeableParameterList, equipmentModel.ChangeableParameters);
      this.IncludePresettings(changeableParameterList, profileType.ChangeableParameters);
      foreach (KeyValuePair<string, string> setupParameter in connectionProfile.ConnectionSettings.SetupParameterList)
      {
        KeyValuePair<string, string> theParam = setupParameter;
        if (changeableParameterList.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == theParam.Key)) == null)
        {
          ChangeableParameter changeableParameter1 = ReadoutConfigFunctions.DbData.ChangableParameterByName[theParam.Key];
          if (changeableParameter1.ParameterEnvironment == null || !changeableParameter1.ParameterEnvironment.Contains(ConfigurationParameterEnvironment.Static) && !changeableParameter1.ParameterEnvironment.Contains(ConfigurationParameterEnvironment.UI))
          {
            ChangeableParameter changeableParameter2 = changeableParameter1.DeepCopy();
            changeableParameter2.Value = theParam.Value;
            changeableParameterList.Add(changeableParameter2);
          }
        }
      }
      return new ConnectionAdjuster(connectionProfile.ConnectionProfileID, connectionProfile.DeviceModel.Name + "_" + connectionProfile.EquipmentModel.Name + "_" + connectionProfile.ProfileType.Name, changeableParameterList);
    }

    private void IncludePresettings(
      List<ChangeableParameter> resultList,
      List<ChangeableParameter> presetList)
    {
      if (presetList == null)
        return;
      foreach (ChangeableParameter preset in presetList)
      {
        ChangeableParameter presetParameter = preset;
        ChangeableParameter changeableParameter = resultList.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == presetParameter.Key));
        if (changeableParameter != null)
          changeableParameter.Value = presetParameter.Value;
      }
    }

    public List<EquipmentGroup> GetEquipmentGroups(DeviceModel deviceModel)
    {
      List<EquipmentModel> equipmentModels = this.GetEquipmentModels(deviceModel);
      if (equipmentModels == null)
        return (List<EquipmentGroup>) null;
      return this.GetEquipmentGroups()?.FindAll((Predicate<EquipmentGroup>) (x => equipmentModels.Exists((Predicate<EquipmentModel>) (y => y.EquipmentGroup.EquipmentGroupID == x.EquipmentGroupID))));
    }

    public List<EquipmentGroup> GetEquipmentGroups()
    {
      List<ConnectionProfile> profiles = this.GetConnectionProfiles();
      return profiles == null ? (List<EquipmentGroup>) null : this.equipmentGroups.FindAll((Predicate<EquipmentGroup>) (x => profiles.Exists((Predicate<ConnectionProfile>) (y => x.EquipmentGroupID == y.EquipmentModel.EquipmentGroup.EquipmentGroupID))));
    }

    public EquipmentModel GetEquipmentModel(int equipmentModelID)
    {
      return this.GetEquipmentModels()?.Find((Predicate<EquipmentModel>) (x => x.EquipmentModelID == equipmentModelID))?.DeepCopy();
    }

    public List<EquipmentModel> GetEquipmentModels() => this.CreateDeepCopy(this.equipmentModels);

    public List<EquipmentModel> GetEquipmentModels(DeviceModel deviceModel)
    {
      if (deviceModel == null)
        throw new ArgumentNullException(nameof (deviceModel));
      List<EquipmentModel> source = new List<EquipmentModel>();
      foreach (ConnectionProfile connectionProfile in this.FilteredConnectionProfiles)
      {
        ConnectionProfile theProfile = connectionProfile;
        if (theProfile.DeviceModel.DeviceModelID == deviceModel.DeviceModelID && source.FirstOrDefault<EquipmentModel>((Func<EquipmentModel, bool>) (x => x.EquipmentModelID == theProfile.EquipmentModel.EquipmentModelID)) == null)
          source.Add(theProfile.EquipmentModel.DeepCopy());
      }
      return source;
    }

    public List<EquipmentModel> GetEquipmentModels(ProfileType profileType)
    {
      if (profileType == null)
        throw new ArgumentNullException(nameof (profileType));
      List<EquipmentModel> source = new List<EquipmentModel>();
      foreach (ConnectionProfile connectionProfile in this.FilteredConnectionProfiles)
      {
        ConnectionProfile theProfile = connectionProfile;
        if (theProfile.ProfileType.ProfileTypeID == profileType.ProfileTypeID && source.FirstOrDefault<EquipmentModel>((Func<EquipmentModel, bool>) (x => x.EquipmentModelID == theProfile.EquipmentModel.EquipmentModelID)) == null)
          source.Add(theProfile.EquipmentModel.DeepCopy());
      }
      return source;
    }

    public List<EquipmentModel> GetEquipmentModels(ProfileType profileType, DeviceModel deviceModel)
    {
      if (profileType == null)
        throw new ArgumentNullException(nameof (profileType));
      if (deviceModel == null)
        throw new ArgumentNullException(nameof (deviceModel));
      List<EquipmentModel> source = new List<EquipmentModel>();
      foreach (ConnectionProfile connectionProfile in this.FilteredConnectionProfiles)
      {
        ConnectionProfile theProfile = connectionProfile;
        if (theProfile.ProfileType.ProfileTypeID == profileType.ProfileTypeID && theProfile.DeviceModel.DeviceModelID == deviceModel.DeviceModelID && source.FirstOrDefault<EquipmentModel>((Func<EquipmentModel, bool>) (x => x.EquipmentModelID == theProfile.EquipmentModel.EquipmentModelID)) == null)
          source.Add(theProfile.EquipmentModel.DeepCopy());
      }
      return source;
    }

    public List<EquipmentModel> GetEquipmentModels(List<ConnectionProfile> profiles)
    {
      List<EquipmentModel> equipmentModels = this.GetEquipmentModels();
      if (equipmentModels == null)
        return (List<EquipmentModel>) null;
      return profiles == null ? equipmentModels : this.CreateDeepCopy(equipmentModels.FindAll((Predicate<EquipmentModel>) (x => profiles.Exists((Predicate<ConnectionProfile>) (y => y.EquipmentModel.EquipmentModelID == x.EquipmentModelID)))));
    }

    public List<EquipmentModel> GetEquipmentModels(EquipmentGroup equipmentGroup)
    {
      return this.GetEquipmentModels(equipmentGroup, (DeviceModel) null);
    }

    public List<EquipmentModel> GetEquipmentModels(
      EquipmentGroup equipmentGroup,
      DeviceModel deviceModel)
    {
      List<EquipmentModel> equipmentModels = this.GetEquipmentModels();
      if (equipmentModels == null)
        return (List<EquipmentModel>) null;
      if (equipmentGroup == null && deviceModel == null)
        return this.CreateDeepCopy(equipmentModels);
      if (equipmentGroup != null && deviceModel == null)
        return this.CreateDeepCopy(equipmentModels.FindAll((Predicate<EquipmentModel>) (x => x.EquipmentGroup.EquipmentGroupID == equipmentGroup.EquipmentGroupID)));
      List<ConnectionProfile> profiles = this.GetConnectionProfiles(deviceModel);
      if (profiles == null)
        return (List<EquipmentModel>) null;
      if (equipmentGroup == null && deviceModel != null)
        return this.GetEquipmentModels(profiles);
      return equipmentGroup != null && deviceModel != null ? this.CreateDeepCopy(equipmentModels.FindAll((Predicate<EquipmentModel>) (x => x.EquipmentGroup.EquipmentGroupID == equipmentGroup.EquipmentGroupID && profiles.Exists((Predicate<ConnectionProfile>) (y => y.EquipmentModel.EquipmentModelID == x.EquipmentModelID))))) : (List<EquipmentModel>) null;
    }

    public bool ExistsConnectionProfile(DeviceModel deviceModel, ProfileType profileType)
    {
      List<ConnectionProfile> connectionProfiles = this.GetConnectionProfiles();
      return connectionProfiles != null && connectionProfiles.Exists((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceModelID == deviceModel.DeviceModelID && x.ProfileType.Name == profileType.Name));
    }

    public List<ProfileType> GetProfileTypes()
    {
      List<ProfileType> source = new List<ProfileType>();
      foreach (ConnectionProfile connectionProfile in this.FilteredConnectionProfiles)
      {
        ConnectionProfile theProfile = connectionProfile;
        if (source.FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (x => x.ProfileTypeID == theProfile.ProfileType.ProfileTypeID)) == null)
          source.Add(theProfile.ProfileType.DeepCopy());
      }
      return source;
    }

    public List<ProfileType> GetProfileTypes(EquipmentModel equipmentModel)
    {
      if (equipmentModel == null)
        throw new ArgumentNullException(nameof (equipmentModel));
      List<ProfileType> source = new List<ProfileType>();
      foreach (ConnectionProfile connectionProfile in this.FilteredConnectionProfiles)
      {
        ConnectionProfile theProfile = connectionProfile;
        if (theProfile.EquipmentModel.EquipmentModelID == equipmentModel.EquipmentModelID && source.FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (x => x.ProfileTypeID == theProfile.ProfileType.ProfileTypeID)) == null)
          source.Add(theProfile.ProfileType.DeepCopy());
      }
      return source;
    }

    public List<ProfileType> GetProfileTypes(DeviceModel deviceModel)
    {
      if (deviceModel == null)
        throw new ArgumentNullException(nameof (deviceModel));
      List<ProfileType> source = new List<ProfileType>();
      foreach (ConnectionProfile connectionProfile in this.FilteredConnectionProfiles)
      {
        ConnectionProfile theProfile = connectionProfile;
        if (theProfile.DeviceModel.DeviceModelID == deviceModel.DeviceModelID && source.FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (x => x.ProfileTypeID == theProfile.ProfileType.ProfileTypeID)) == null)
          source.Add(theProfile.ProfileType.DeepCopy());
      }
      return source;
    }

    public List<ProfileType> GetProfileTypes(DeviceModel deviceModel, EquipmentModel equipmentModel)
    {
      if (deviceModel == null)
        throw new ArgumentNullException(nameof (deviceModel));
      if (equipmentModel == null)
        throw new ArgumentNullException(nameof (equipmentModel));
      List<ProfileType> source = new List<ProfileType>();
      foreach (ConnectionProfile connectionProfile in this.FilteredConnectionProfiles)
      {
        ConnectionProfile theProfile = connectionProfile;
        if (theProfile.EquipmentModel.EquipmentModelID == equipmentModel.EquipmentModelID && theProfile.DeviceModel.DeviceModelID == deviceModel.DeviceModelID && source.FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (x => x.ProfileTypeID == theProfile.ProfileType.ProfileTypeID)) == null)
          source.Add(theProfile.ProfileType.DeepCopy());
      }
      return source;
    }

    public List<ProfileType> GetProfileTypes(
      DeviceModel deviceModel,
      EquipmentModel equipmentModel,
      ProfileTypeTags? tags)
    {
      List<ConnectionProfile> connectionProfiles = this.GetConnectionProfiles(deviceModel, equipmentModel);
      if (connectionProfiles == null || connectionProfiles.Count == 0)
        return (List<ProfileType>) null;
      List<ConnectionProfile> choosedProfiles = this.GetChoosedProfiles(connectionProfiles, TransceiverType.None, new DeviceModelTags?(), tags);
      List<ProfileType> result = new List<ProfileType>();
      foreach (ConnectionProfile connectionProfile in choosedProfiles)
      {
        ConnectionProfile profile = connectionProfile;
        if (!result.Exists((Predicate<ProfileType>) (x => x.Name == profile.ProfileType.Name)))
          result.Add(profile.ProfileType);
      }
      return this.CreateDeepCopy(result);
    }

    public List<ProfileType> GetProfileTypes(
      DeviceModel deviceModel,
      EquipmentModel equipmentModel,
      TransceiverType type)
    {
      List<ConnectionProfile> connectionProfiles = this.GetConnectionProfiles(deviceModel, equipmentModel, type);
      if (connectionProfiles == null || connectionProfiles.Count == 0)
        return (List<ProfileType>) null;
      List<ProfileType> result = new List<ProfileType>();
      foreach (ConnectionProfile connectionProfile in connectionProfiles)
      {
        ConnectionProfile profile = connectionProfile;
        if (!result.Exists((Predicate<ProfileType>) (x => x.Name == profile.ProfileType.Name)))
          result.Add(profile.ProfileType);
      }
      return this.CreateDeepCopy(result);
    }

    public List<ProfileType> GetProfileTypes(ZENNER.CommonLibrary.Entities.Meter meter)
    {
      return this.GetProfileTypes(new List<ZENNER.CommonLibrary.Entities.Meter>()
      {
        meter
      }, TransceiverType.None);
    }

    public List<ProfileType> GetProfileTypes(List<ZENNER.CommonLibrary.Entities.Meter> meters)
    {
      return this.GetProfileTypes(meters, TransceiverType.None);
    }

    public List<ProfileType> GetProfileTypes(
      List<ZENNER.CommonLibrary.Entities.Meter> meters,
      EquipmentModel equipmentModel,
      ProfileTypeTags? tags)
    {
      if (meters == null)
        return (List<ProfileType>) null;
      List<ProfileType> result = new List<ProfileType>();
      foreach (ZENNER.CommonLibrary.Entities.Meter meter in meters)
      {
        List<ConnectionProfile> connectionProfiles = this.GetConnectionProfiles(meter.DeviceModel, equipmentModel);
        if (connectionProfiles != null && connectionProfiles.Count != 0)
        {
          foreach (ConnectionProfile choosedProfile in this.GetChoosedProfiles(connectionProfiles, TransceiverType.None, new DeviceModelTags?(), tags))
          {
            ConnectionProfile profile = choosedProfile;
            if (!result.Exists((Predicate<ProfileType>) (x => x.ProfileTypeID == profile.ProfileType.ProfileTypeID)))
              result.Add(profile.ProfileType);
          }
        }
      }
      return this.CreateDeepCopy(result);
    }

    public List<ProfileType> GetProfileTypes(
      List<ZENNER.CommonLibrary.Entities.Meter> meters,
      EquipmentModel equipmentModel,
      TransceiverType type)
    {
      if (meters == null)
        return (List<ProfileType>) null;
      List<ProfileType> result = new List<ProfileType>();
      foreach (ZENNER.CommonLibrary.Entities.Meter meter in meters)
      {
        List<ConnectionProfile> connectionProfiles = this.GetConnectionProfiles(meter.DeviceModel, equipmentModel, type);
        if (connectionProfiles != null && connectionProfiles.Count != 0)
        {
          foreach (ConnectionProfile connectionProfile in connectionProfiles)
          {
            ConnectionProfile profile = connectionProfile;
            if (!result.Exists((Predicate<ProfileType>) (x => x.Name == profile.ProfileType.Name)))
              result.Add(profile.ProfileType);
          }
        }
      }
      return this.CreateDeepCopy(result);
    }

    public List<ProfileType> GetProfileTypes(List<ZENNER.CommonLibrary.Entities.Meter> meters, TransceiverType type)
    {
      if (meters == null)
        return (List<ProfileType>) null;
      List<ProfileType> result = new List<ProfileType>();
      foreach (ZENNER.CommonLibrary.Entities.Meter meter in meters)
      {
        List<ConnectionProfile> connectionProfiles = this.GetConnectionProfiles(type, meter.DeviceModel);
        if (connectionProfiles != null && connectionProfiles.Count != 0)
        {
          foreach (ConnectionProfile connectionProfile in connectionProfiles)
          {
            ConnectionProfile profile = connectionProfile;
            if (!result.Exists((Predicate<ProfileType>) (x => x.Name == profile.ProfileType.Name)))
              result.Add(profile.ProfileType);
          }
        }
      }
      return this.CreateDeepCopy(result);
    }

    public List<string> GetProfileTypes(DeviceModel deviceModel, TransceiverType type)
    {
      if (deviceModel == null)
        return (List<string>) null;
      List<ConnectionProfile> connectionProfiles = this.GetConnectionProfiles(type, deviceModel);
      if (connectionProfiles == null || connectionProfiles.Count == 0)
        return (List<string>) null;
      List<string> profileTypes = new List<string>();
      foreach (ConnectionProfile connectionProfile in connectionProfiles)
      {
        if (!profileTypes.Contains(connectionProfile.ProfileType.Name))
          profileTypes.Add(connectionProfile.ProfileType.Name);
      }
      return profileTypes;
    }

    public static void Dispose() => ReadoutConfigFunctions.Dispose();

    public DeviceModel DetermineDeviceModel(ValueIdentSet e)
    {
      if (e == null)
        return (DeviceModel) null;
      string manufacturer = string.Empty;
      string generation = string.Empty;
      bool isRadio = false;
      bool isMbus = false;
      if (e.ZDF != null)
      {
        manufacturer = ParameterService.GetParameter(e.ZDF, "MAN");
        generation = ParameterService.GetParameter(e.ZDF, "GEN");
        int num;
        if (!e.ZDF.Contains("RSSI;"))
        {
          string deviceType1 = e.DeviceType;
          DeviceTypes deviceTypes = DeviceTypes.AquaMicroRadio3;
          string str1 = deviceTypes.ToString();
          if (!(deviceType1 == str1))
          {
            string deviceType2 = e.DeviceType;
            deviceTypes = DeviceTypes.EHCA_M6_Radio3;
            string str2 = deviceTypes.ToString();
            if (!(deviceType2 == str2))
            {
              string deviceType3 = e.DeviceType;
              deviceTypes = DeviceTypes.HumiditySensor;
              string str3 = deviceTypes.ToString();
              if (!(deviceType3 == str3))
              {
                string deviceType4 = e.DeviceType;
                deviceTypes = DeviceTypes.EDC;
                string str4 = deviceTypes.ToString();
                if (!(deviceType4 == str4))
                {
                  string deviceType5 = e.DeviceType;
                  deviceTypes = DeviceTypes.MinotelContactRadio3;
                  string str5 = deviceTypes.ToString();
                  if (!(deviceType5 == str5))
                  {
                    string deviceType6 = e.DeviceType;
                    deviceTypes = DeviceTypes.PDC;
                    string str6 = deviceTypes.ToString();
                    if (!(deviceType6 == str6))
                    {
                      string deviceType7 = e.DeviceType;
                      deviceTypes = DeviceTypes.SmokeDetector;
                      string str7 = deviceTypes.ToString();
                      if (!(deviceType7 == str7))
                      {
                        string deviceType8 = e.DeviceType;
                        deviceTypes = DeviceTypes.TemperatureSensor;
                        string str8 = deviceTypes.ToString();
                        num = deviceType8 == str8 ? 1 : 0;
                        goto label_13;
                      }
                    }
                  }
                }
              }
            }
          }
        }
        num = 1;
label_13:
        isRadio = num != 0;
        isMbus = e.ZDF.Contains("RTIME") && e.ZDF.Contains("SID") && e.ZDF.Contains("MAN") && e.ZDF.Contains("GEN") && e.ZDF.Contains("MED") && e.ZDF.Contains("RADR");
      }
      DeviceModel deviceModel = this.DetermineDeviceModel(e.SerialNumber, manufacturer, generation, isRadio, isMbus);
      if (deviceModel != null)
        deviceModel.ChangeableParameters = (List<ChangeableParameter>) null;
      return deviceModel;
    }

    public DeviceModel DetermineDeviceModel(
      string serialNumber = "",
      string manufacturer = "",
      string generation = "",
      bool isRadio = false,
      bool isMbus = false)
    {
      bool flag = manufacturer.Contains("MINOL") || manufacturer.Contains("Minol");
      if (!flag && !string.IsNullOrEmpty(manufacturer) && !string.IsNullOrEmpty(generation))
      {
        List<DeviceModel> all = this.deviceModels.FindAll((Predicate<DeviceModel>) (x => x.Manufacturer == manufacturer && x.Generation == generation));
        if (all != null && all.Count > 0)
          return all.Count > 1 && isRadio ? all.Find((Predicate<DeviceModel>) (x => x.Parameters.ContainsKey(ConnectionProfileParameter.Radio2) || x.Parameters.ContainsKey(ConnectionProfileParameter.Radio3) || x.Parameters.ContainsKey(ConnectionProfileParameter.wMBus))) : all[0];
      }
      if (!isMbus && !string.IsNullOrEmpty(serialNumber))
      {
        DeviceTypes type = NumberRanges.GetTypeOfMinolDevice(serialNumber);
        if (type != 0)
        {
          List<DeviceModel> all = this.deviceModels.FindAll((Predicate<DeviceModel>) (x => x.Medium == type.ToString()));
          if (all != null && all.Count > 0)
            return all.Count > 1 ? all.Find((Predicate<DeviceModel>) (x => x.Parameters.ContainsKey(ConnectionProfileParameter.Radio2) || x.Parameters.ContainsKey(ConnectionProfileParameter.Radio3))) ?? all[0] : all[0];
        }
      }
      if (flag)
        return this.deviceModels.Find((Predicate<DeviceModel>) (x => x.Name == "Generic Minol device"));
      return !string.IsNullOrEmpty(manufacturer) && !string.IsNullOrEmpty(generation) ? (isRadio ? this.deviceModels.Find((Predicate<DeviceModel>) (x => x.Name == "Generic wM-Bus")) : this.deviceModels.Find((Predicate<DeviceModel>) (x => x.Name == "Generic M-Bus"))) : (isMbus ? this.deviceModels.Find((Predicate<DeviceModel>) (x => x.Name == "Generic M-Bus")) : (DeviceModel) null);
    }

    public DeviceModel GetDeviceModelGenericMBus()
    {
      return this.deviceModels.Find((Predicate<DeviceModel>) (x => x.Name == "Generic M-Bus"))?.DeepCopy();
    }

    private List<DeviceModel> CreateDeepCopy(List<DeviceModel> result)
    {
      if (result == null)
        return (List<DeviceModel>) null;
      if (result.Count == 0)
        return result;
      List<DeviceModel> deepCopy = new List<DeviceModel>(result.Count);
      result.ForEach((Action<DeviceModel>) (t => deepCopy.Add(t.DeepCopy())));
      return deepCopy;
    }

    private List<EquipmentModel> CreateDeepCopy(List<EquipmentModel> result)
    {
      if (result == null)
        return (List<EquipmentModel>) null;
      if (result.Count == 0)
        return result;
      List<EquipmentModel> deepCopy = new List<EquipmentModel>(result.Count);
      result.ForEach((Action<EquipmentModel>) (t => deepCopy.Add(t.DeepCopy())));
      return deepCopy;
    }

    private List<ProfileType> CreateDeepCopy(List<ProfileType> result)
    {
      if (result == null)
        return (List<ProfileType>) null;
      if (result.Count == 0)
        return result;
      List<ProfileType> deepCopy = new List<ProfileType>(result.Count);
      result.ForEach((Action<ProfileType>) (t => deepCopy.Add(t.DeepCopy())));
      return deepCopy;
    }
  }
}
