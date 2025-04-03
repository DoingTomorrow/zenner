// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.ConnectionProfileIdentification
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using System;
using System.Linq;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace ReadoutConfiguration
{
  public class ConnectionProfileIdentification
  {
    public int ConnectionProfileID { get; private set; }

    public int DeviceModelID { get; private set; }

    public string DeviceModelName { get; private set; }

    public int DeviceGroupID { get; private set; }

    public string DeviceGroupName { get; private set; }

    public int EquipmentModelID { get; private set; }

    public string EquipmentModelName { get; private set; }

    public int EquipmentGroupID { get; private set; }

    public string EquipmentGroupName { get; private set; }

    public int ProfileTypeID { get; private set; }

    public string ProfileTypeName { get; private set; }

    public int ProfileTypeGroupID { get; private set; }

    public string ProfileTypeGroupName { get; private set; }

    public int ConnectionSettingsID { get; private set; }

    public override string ToString()
    {
      return this.DeviceModelName + ";" + this.EquipmentModelName + ";" + this.ProfileTypeName;
    }

    public ConnectionProfileIdentification(int profileID)
    {
      ConnectionProfile connectionProfile = ReadoutConfigFunctions.DbData.CachedPartiallyConnectionProfiles.FirstOrDefault<ConnectionProfile>((Func<ConnectionProfile, bool>) (x => x.ConnectionProfileID == profileID));
      this.ConnectionProfileID = connectionProfile != null ? connectionProfile.ConnectionProfileID : throw new Exception("Unknown connection profile ID");
      this.DeviceModelID = connectionProfile.DeviceModel.DeviceModelID;
      this.DeviceModelName = connectionProfile.DeviceModel.Name;
      this.DeviceGroupID = connectionProfile.DeviceModel.DeviceGroup.DeviceGroupID;
      this.DeviceGroupName = connectionProfile.DeviceModel.DeviceGroup.Name;
      this.EquipmentModelID = connectionProfile.EquipmentModel.EquipmentModelID;
      this.EquipmentModelName = connectionProfile.EquipmentModel.Name;
      this.EquipmentGroupID = connectionProfile.EquipmentModel.EquipmentGroup.EquipmentGroupID;
      this.EquipmentGroupName = connectionProfile.EquipmentModel.EquipmentGroup.Name;
      this.ProfileTypeID = connectionProfile.ProfileType.ProfileTypeID;
      this.ProfileTypeName = connectionProfile.ProfileType.Name;
      this.ProfileTypeGroupID = connectionProfile.ProfileType.ProfileTypeGroup.ProfileTypeGroupID;
      this.ProfileTypeGroupName = connectionProfile.ProfileType.ProfileTypeGroup.Name;
      this.ConnectionSettingsID = connectionProfile.ConnectionSettings.ConnectionSettingsID;
    }
  }
}
