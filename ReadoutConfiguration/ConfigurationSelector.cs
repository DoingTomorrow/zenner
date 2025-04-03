// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.ConfigurationSelector
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using StartupLib;
using System;
using System.Collections.Generic;
using System.Linq;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace ReadoutConfiguration
{
  internal class ConfigurationSelector
  {
    private List<ConnectionProfile> filteredProfilesList;

    internal ConfigurationSelector(List<ConnectionProfile> filteredProfilesList)
    {
      this.filteredProfilesList = filteredProfilesList;
      if (this.filteredProfilesList != null)
        return;
      this.filteredProfilesList = ReadoutConfigFunctions.DbData.GetPartiallyConnectionProfiles();
    }

    internal ProfileSelecterLists GetAllowedSelectorLists(int ConnectionProfileID)
    {
      ConnectionProfile activeProfile = this.filteredProfilesList.Find((Predicate<ConnectionProfile>) (x => x.ConnectionProfileID == ConnectionProfileID));
      if (!UserManager.IsDeviceModelAllowed(activeProfile.DeviceModel.Name))
        throw new Exception("Selected profile not allowed by license.");
      ProfileSelecterLists allowedSelectorLists = new ProfileSelecterLists();
      List<DeviceGroup> source1 = new List<DeviceGroup>();
      List<DeviceModel> allowedDeviceModels = new List<DeviceModel>();
      List<EquipmentGroup> source2 = new List<EquipmentGroup>();
      List<EquipmentModel> source3 = new List<EquipmentModel>();
      List<ProfileTypeGroup> source4 = new List<ProfileTypeGroup>();
      List<ProfileType> source5 = new List<ProfileType>();
      foreach (ConnectionProfile filteredProfiles in this.filteredProfilesList)
      {
        ConnectionProfile theProfile = filteredProfiles;
        if (source1.FirstOrDefault<DeviceGroup>((Func<DeviceGroup, bool>) (x => x.DeviceGroupID == theProfile.DeviceModel.DeviceGroup.DeviceGroupID)) == null)
          source1.Add(theProfile.DeviceModel.DeviceGroup);
        if (allowedDeviceModels.FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (x => x.DeviceModelID == theProfile.DeviceModel.DeviceModelID)) == null)
          allowedDeviceModels.Add(theProfile.DeviceModel);
        if (source2.FirstOrDefault<EquipmentGroup>((Func<EquipmentGroup, bool>) (x => x.EquipmentGroupID == theProfile.EquipmentModel.EquipmentGroup.EquipmentGroupID)) == null)
          source2.Add(theProfile.EquipmentModel.EquipmentGroup);
        if (source3.FirstOrDefault<EquipmentModel>((Func<EquipmentModel, bool>) (x => x.EquipmentModelID == theProfile.EquipmentModel.EquipmentModelID)) == null)
          source3.Add(theProfile.EquipmentModel);
        if (source4.FirstOrDefault<ProfileTypeGroup>((Func<ProfileTypeGroup, bool>) (x => x.ProfileTypeGroupID == theProfile.ProfileType.ProfileTypeGroup.ProfileTypeGroupID)) == null)
          source4.Add(theProfile.ProfileType.ProfileTypeGroup);
        if (source5.FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (x => x.ProfileTypeID == theProfile.ProfileType.ProfileTypeID)) == null)
          source5.Add(theProfile.ProfileType);
      }
      allowedSelectorLists.allDeviceGroupsList = source1.FindAll((Predicate<DeviceGroup>) (x => allowedDeviceModels.FindAll((Predicate<DeviceModel>) (d => d.DeviceGroup.DeviceGroupID == x.DeviceGroupID)).Count > 0));
      allowedSelectorLists.allDeviceGroupsList.Sort((IComparer<DeviceGroup>) new ConnectionItemComparer());
      allowedSelectorLists.allDeviceModelsList = allowedDeviceModels.FindAll((Predicate<DeviceModel>) (x => x.DeviceGroup.DeviceGroupID == activeProfile.DeviceModel.DeviceGroup.DeviceGroupID));
      allowedSelectorLists.allDeviceModelsList.Sort((IComparer<DeviceModel>) new ConnectionItemComparer());
      List<ConnectionProfile> selectedDeviceProfiles = this.filteredProfilesList.FindAll((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceModelID == activeProfile.DeviceModel.DeviceModelID));
      allowedSelectorLists.reducedEquipmentGroups = source2.FindAll((Predicate<EquipmentGroup>) (x => selectedDeviceProfiles.Find((Predicate<ConnectionProfile>) (y => y.EquipmentModel.EquipmentGroup.EquipmentGroupID == x.EquipmentGroupID)) != null));
      allowedSelectorLists.reducedEquipmentGroups.Sort((IComparer<EquipmentGroup>) new ConnectionItemComparer());
      allowedSelectorLists.reducedEquipmentModels = source3.FindAll((Predicate<EquipmentModel>) (x => x.EquipmentGroup.EquipmentGroupID == activeProfile.EquipmentModel.EquipmentGroup.EquipmentGroupID && selectedDeviceProfiles.Find((Predicate<ConnectionProfile>) (y => y.EquipmentModel.EquipmentModelID == x.EquipmentModelID)) != null));
      allowedSelectorLists.reducedEquipmentModels.Sort((IComparer<EquipmentModel>) new ConnectionItemComparer());
      List<ConnectionProfile> selectedEquipmentProfiles = selectedDeviceProfiles.FindAll((Predicate<ConnectionProfile>) (x => x.EquipmentModel.EquipmentModelID == activeProfile.EquipmentModel.EquipmentModelID));
      allowedSelectorLists.reducedProfileTypeGroups = source4.FindAll((Predicate<ProfileTypeGroup>) (x => selectedEquipmentProfiles.Find((Predicate<ConnectionProfile>) (y => y.ProfileType.ProfileTypeGroup.ProfileTypeGroupID == x.ProfileTypeGroupID)) != null));
      allowedSelectorLists.reducedProfileTypeGroups.Sort((IComparer<ProfileTypeGroup>) new ConnectionItemComparer());
      allowedSelectorLists.reducedProfileTypes = source5.FindAll((Predicate<ProfileType>) (x => x.ProfileTypeGroup.ProfileTypeGroupID == activeProfile.ProfileType.ProfileTypeGroup.ProfileTypeGroupID && selectedEquipmentProfiles.Find((Predicate<ConnectionProfile>) (y => y.ProfileType.ProfileTypeID == x.ProfileTypeID)) != null));
      allowedSelectorLists.reducedProfileTypes.Sort((IComparer<ProfileType>) new ConnectionItemComparer());
      return allowedSelectorLists;
    }

    internal int GetProfileIdFromProfileSelecterIDs(
      ConnectionProfile activeProfile,
      ProfileSelecterIDs selectorIDs)
    {
      if (activeProfile.DeviceModel.DeviceGroup.DeviceGroupID != selectorIDs.DeviceGroupID)
      {
        List<ConnectionProfile> all = this.filteredProfilesList.FindAll((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceGroup.DeviceGroupID == selectorIDs.DeviceGroupID && x.EquipmentModel.EquipmentModelID == selectorIDs.EquipmentModelID && x.ProfileType.ProfileTypeID == selectorIDs.ProfileTypeID));
        if (all == null || all.Count == 0)
        {
          all = this.filteredProfilesList.FindAll((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceGroup.DeviceGroupID == selectorIDs.DeviceGroupID && x.EquipmentModel.EquipmentModelID == selectorIDs.EquipmentModelID));
          if (all == null || all.Count == 0)
          {
            all = this.filteredProfilesList.FindAll((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceGroup.DeviceGroupID == selectorIDs.DeviceGroupID));
            if (all == null || all.Count == 0)
              throw new Exception("No ConnectionProfile for DeviceGroupID: " + selectorIDs.DeviceGroupID.ToString());
          }
        }
        return all[0].ConnectionProfileID;
      }
      if (activeProfile.DeviceModel.DeviceModelID != selectorIDs.DeviceModelID)
      {
        List<ConnectionProfile> all = this.filteredProfilesList.FindAll((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceModelID == selectorIDs.DeviceModelID && x.EquipmentModel.EquipmentModelID == selectorIDs.EquipmentModelID && x.ProfileType.ProfileTypeID == selectorIDs.ProfileTypeID));
        if (all == null || all.Count == 0)
        {
          all = this.filteredProfilesList.FindAll((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceModelID == selectorIDs.DeviceModelID && x.EquipmentModel.EquipmentModelID == selectorIDs.EquipmentModelID));
          if (all == null || all.Count == 0)
          {
            all = this.filteredProfilesList.FindAll((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceModelID == selectorIDs.DeviceModelID));
            if (all == null || all.Count == 0)
              throw new Exception("No ConnectionProfile for DeviceModelID: " + selectorIDs.DeviceModelID.ToString());
          }
        }
        return all[0].ConnectionProfileID;
      }
      if (activeProfile.EquipmentModel.EquipmentGroup.EquipmentGroupID != selectorIDs.EquipmentGroupID)
      {
        List<ConnectionProfile> all = this.filteredProfilesList.FindAll((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceModelID == selectorIDs.DeviceModelID && x.EquipmentModel.EquipmentGroup.EquipmentGroupID == selectorIDs.EquipmentGroupID && x.ProfileType.ProfileTypeID == selectorIDs.ProfileTypeID));
        if (all == null || all.Count == 0)
        {
          all = this.filteredProfilesList.FindAll((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceModelID == selectorIDs.DeviceModelID && x.EquipmentModel.EquipmentGroup.EquipmentGroupID == selectorIDs.EquipmentGroupID));
          if (all == null || all.Count == 0)
            throw new Exception("No ConnectionProfile for EquipmentGroupID: " + selectorIDs.EquipmentGroupID.ToString());
        }
        return all[0].ConnectionProfileID;
      }
      if (activeProfile.EquipmentModel.EquipmentModelID != selectorIDs.EquipmentModelID)
      {
        List<ConnectionProfile> all = this.filteredProfilesList.FindAll((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceModelID == selectorIDs.DeviceModelID && x.EquipmentModel.EquipmentModelID == selectorIDs.EquipmentModelID && x.ProfileType.ProfileTypeID == selectorIDs.ProfileTypeID));
        if (all == null || all.Count == 0)
        {
          all = this.filteredProfilesList.FindAll((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceModelID == selectorIDs.DeviceModelID && x.EquipmentModel.EquipmentModelID == selectorIDs.EquipmentModelID));
          if (all == null || all.Count == 0)
            throw new Exception("No ConnectionProfile for EquipmentModelID: " + selectorIDs.EquipmentModelID.ToString());
        }
        return all[0].ConnectionProfileID;
      }
      if (activeProfile.ProfileType.ProfileTypeGroup.ProfileTypeGroupID != selectorIDs.ProfileTypeGroupID)
      {
        List<ConnectionProfile> all = this.filteredProfilesList.FindAll((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceModelID == selectorIDs.DeviceModelID && x.EquipmentModel.EquipmentModelID == selectorIDs.EquipmentModelID && x.ProfileType.ProfileTypeGroup.ProfileTypeGroupID == selectorIDs.ProfileTypeGroupID));
        return all != null && all.Count != 0 ? all[0].ConnectionProfileID : throw new Exception("No ConnectionProfile for ProfileTypeGroupID: " + selectorIDs.ProfileTypeGroupID.ToString());
      }
      if (activeProfile.ProfileType.ProfileTypeID == selectorIDs.ProfileTypeID)
        return activeProfile.ConnectionProfileID;
      List<ConnectionProfile> all1 = this.filteredProfilesList.FindAll((Predicate<ConnectionProfile>) (x => x.DeviceModel.DeviceModelID == selectorIDs.DeviceModelID && x.EquipmentModel.EquipmentModelID == selectorIDs.EquipmentModelID && x.ProfileType.ProfileTypeID == selectorIDs.ProfileTypeID));
      return all1 != null && all1.Count != 0 ? all1[0].ConnectionProfileID : throw new Exception("No ConnectionProfile for ProfileTypeID: " + selectorIDs.ProfileTypeID.ToString());
    }
  }
}
