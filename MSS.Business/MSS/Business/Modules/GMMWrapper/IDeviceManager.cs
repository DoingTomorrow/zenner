// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMMWrapper.IDeviceManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System.Collections.Generic;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS.Business.Modules.GMMWrapper
{
  public interface IDeviceManager
  {
    string SetFilter(string newValue);

    string ResetFilter();

    ConnectionProfile GetConnectionProfile(int connectionProfileId);

    List<ProfileType> GetProfileTypes(
      List<Meter> meters,
      EquipmentModel equipmentModel,
      ProfileTypeTags? tags);

    List<EquipmentModel> GetEquipmentModels();

    List<DeviceModel> GetDeviceModels();

    DeviceModel GetDeviceModel(string name);

    ConnectionAdjuster GetConnectionAdjuster(
      DeviceModel deviceModel,
      EquipmentModel equipmentModel,
      ProfileType profileType);

    List<ProfileType> GetProfileTypes(DeviceModel deviceModel, EquipmentModel equipmentModel);

    List<DeviceModel> GetDeviceModels(DeviceModelTags tags);
  }
}
