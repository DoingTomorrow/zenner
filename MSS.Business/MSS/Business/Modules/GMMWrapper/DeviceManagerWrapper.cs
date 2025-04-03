// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMMWrapper.DeviceManagerWrapper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System.Collections.Generic;
using ZENNER;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS.Business.Modules.GMMWrapper
{
  public class DeviceManagerWrapper : IDeviceManager
  {
    public const string FilterDefaultReceiverRadio3 = "DefaultReceiverRadio3";
    public const string FilterDefaultScanner = "DefaultScanner";

    public DeviceManagerWrapper()
    {
    }

    public DeviceManagerWrapper(string filter) => this.SetFilter(filter);

    public string SetFilter(string newValue)
    {
      string selectedFilter = GmmInterface.DeviceManager.SelectedFilter;
      GmmInterface.DeviceManager.SelectedFilter = newValue;
      return selectedFilter;
    }

    public string ResetFilter()
    {
      string selectedFilter = GmmInterface.DeviceManager.SelectedFilter;
      GmmInterface.DeviceManager.SelectedFilter = (string) null;
      return selectedFilter;
    }

    public ConnectionProfile GetConnectionProfile(int connectionProfileId)
    {
      return GmmInterface.DeviceManager.GetConnectionProfile(connectionProfileId);
    }

    public List<ProfileType> GetProfileTypes(
      List<Meter> meters,
      EquipmentModel equipmentModel,
      ProfileTypeTags? tags)
    {
      return GmmInterface.DeviceManager.GetProfileTypes(meters, equipmentModel, tags);
    }

    public List<EquipmentModel> GetEquipmentModels()
    {
      return GmmInterface.DeviceManager.GetEquipmentModels();
    }

    public List<DeviceModel> GetDeviceModels() => GmmInterface.DeviceManager.GetDeviceModels();

    public DeviceModel GetDeviceModel(string name)
    {
      return GmmInterface.DeviceManager.GetDeviceModel(name);
    }

    public ConnectionAdjuster GetConnectionAdjuster(
      DeviceModel deviceModel,
      EquipmentModel equipmentModel,
      ProfileType profileType)
    {
      return GmmInterface.DeviceManager.GetConnectionAdjuster(deviceModel, equipmentModel, profileType);
    }

    public List<ProfileType> GetProfileTypes(DeviceModel deviceModel, EquipmentModel equipmentModel)
    {
      return GmmInterface.DeviceManager.GetProfileTypes(deviceModel, equipmentModel);
    }

    public List<DeviceModel> GetDeviceModels(DeviceModelTags tags)
    {
      return GmmInterface.DeviceManager.GetDeviceModels(tags);
    }
  }
}
