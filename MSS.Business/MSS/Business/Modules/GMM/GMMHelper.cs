// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.GMMHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Modules.Configuration;
using MSS.Business.Modules.GMMWrapper;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using MSS.Core.Utils;
using MSS.DTO.Meters;
using MSS.DTO.Orders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ZENNER;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public static class GMMHelper
  {
    private static List<ZENNER.CommonLibrary.Entities.Meter> meterList = new List<ZENNER.CommonLibrary.Entities.Meter>();
    private static List<MeterDTO> meterDtoList = new List<MeterDTO>();

    public static void GetDeviceGroupAndModelBasedOnDeviceType(
      DeviceTypeEnum deviceTypeEnum,
      ref DeviceGroup deviceGroup,
      ref DeviceModel deviceModel)
    {
      string deviceModelId = deviceTypeEnum.GetGMMDeviceModelName();
      foreach (DeviceGroup deviceGroup1 in GmmInterface.DeviceManager.GetDeviceGroups())
      {
        List<DeviceModel> deviceModels = GmmInterface.DeviceManager.GetDeviceModels(deviceGroup1);
        if (deviceModels.Any<DeviceModel>((Func<DeviceModel, bool>) (m => m.Name == deviceModelId)))
        {
          deviceModel = deviceModels.Find((Predicate<DeviceModel>) (m => m.Name == deviceModelId));
          deviceGroup = deviceGroup1;
          if (!deviceModel.Parameters.ContainsKey(ConnectionProfileParameter.SystemDevice))
            break;
          deviceModel = (DeviceModel) null;
          deviceGroup = (DeviceGroup) null;
          break;
        }
      }
    }

    public static DeviceModel GetDeviceModel(DeviceTypeEnum deviceTypeEnum)
    {
      string deviceModelId = deviceTypeEnum.GetGMMDeviceModelName();
      return GmmInterface.DeviceManager.GetDeviceModels().FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (d => d.Name == deviceModelId));
    }

    public static DeviceModel GetDeviceModel(DeviceTypeEnum deviceTypeEnum, DeviceModelTags tags)
    {
      string deviceModelId = deviceTypeEnum.GetGMMDeviceModelName();
      return GmmInterface.DeviceManager.GetDeviceModels(tags).FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (d => d.Name == deviceModelId));
    }

    public static List<ZENNER.CommonLibrary.Entities.Meter> GetMeters(
      ObservableCollection<ExecuteOrderStructureNode> devices,
      List<long> filter,
      StructureTypeEnum? structureType,
      string scanParams = null)
    {
      List<ZENNER.CommonLibrary.Entities.Meter> meters = new List<ZENNER.CommonLibrary.Entities.Meter>();
      List<string> deviceModelNameList = GMMHelper.GetDeviceModelNameList(GMMHelper.GetDeviceModelBasedOnStructureType(structureType));
      foreach (ExecuteOrderStructureNode device in (Collection<ExecuteOrderStructureNode>) devices)
      {
        if (!string.IsNullOrEmpty(device.SerialNumber) && device.ReadingEnabled && GMMHelper.IsDeviceIncludedInLicense(device.DeviceType, deviceModelNameList))
        {
          ZENNER.CommonLibrary.Entities.Meter gmmMeter = GMMHelper.GetGMMMeter(device, filter, scanParams);
          if (gmmMeter != null)
            meters.Add(gmmMeter);
        }
      }
      return meters;
    }

    public static bool IsDeviceIncludedInLicense(
      DeviceTypeEnum? deviceType,
      List<string> importableDeviceModelNameList)
    {
      return deviceType.HasValue && importableDeviceModelNameList.Contains(((Enum) (ValueType) deviceType).GetGMMDeviceModelName());
    }

    public static ZENNER.CommonLibrary.Entities.Meter GetGMMMeter(
      ExecuteOrderStructureNode selectedItem,
      List<long> filter,
      string scanParams = null)
    {
      if (!selectedItem.DeviceType.HasValue)
        return (ZENNER.CommonLibrary.Entities.Meter) null;
      DeviceTypeEnum deviceTypeEnum = (DeviceTypeEnum) Enum.Parse(typeof (DeviceTypeEnum), selectedItem.DeviceType.ToString(), true);
      DeviceGroup deviceGroup = new DeviceGroup();
      DeviceModel deviceModel = new DeviceModel();
      GMMHelper.GetDeviceGroupAndModelBasedOnDeviceType(deviceTypeEnum, ref deviceGroup, ref deviceModel);
      ZENNER.CommonLibrary.Entities.Meter meter = new ZENNER.CommonLibrary.Entities.Meter()
      {
        DeviceModel = deviceModel,
        ID = selectedItem.MeterId,
        SerialNumber = selectedItem.SerialNumber,
        Filter = filter,
        AdditionalInfo = GMMHelper.ConstructMeterAdditionalInfo(selectedItem.Manufacturer, selectedItem.InputNumber, selectedItem.Generation, selectedItem.PrimaryAddres, selectedItem.Medium, selectedItem.DeviceInfo)
      };
      if (!string.IsNullOrEmpty(scanParams))
        MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateDeviceWithSavedParams(meter, scanParams);
      return meter;
    }

    public static List<ZENNER.CommonLibrary.Entities.Meter> GetGMMMetersFromStructureNodeDTO(
      StructureNodeDTO structureNode,
      out List<MeterDTO> meterDTOs)
    {
      GMMHelper.meterList.Clear();
      GMMHelper.meterDtoList.Clear();
      if (structureNode.NodeType.Name == "Meter")
      {
        GMMHelper.meterList.Add(GMMHelper.GetGMMMeter((MeterDTO) structureNode.Entity));
        GMMHelper.meterDtoList.Add((MeterDTO) structureNode.Entity);
      }
      GMMHelper.WalkStructure(structureNode);
      meterDTOs = GMMHelper.meterDtoList;
      return GMMHelper.meterList;
    }

    private static void WalkStructure(StructureNodeDTO selectedNode)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) selectedNode.SubNodes)
      {
        if (subNode.NodeType.Name == "Meter" && subNode.Entity != null)
        {
          GMMHelper.meterList.Add(GMMHelper.GetGMMMeter((MeterDTO) subNode.Entity));
          GMMHelper.meterDtoList.Add((MeterDTO) subNode.Entity);
        }
        GMMHelper.WalkStructure(subNode);
      }
    }

    public static List<ZENNER.CommonLibrary.Entities.Meter> GetGMMMeters(List<MSS.Core.Model.Meters.Meter> mssMeters)
    {
      return mssMeters.Select<MSS.Core.Model.Meters.Meter, ZENNER.CommonLibrary.Entities.Meter>(new Func<MSS.Core.Model.Meters.Meter, ZENNER.CommonLibrary.Entities.Meter>(GMMHelper.GetGMMMeter)).ToList<ZENNER.CommonLibrary.Entities.Meter>();
    }

    public static ZENNER.CommonLibrary.Entities.Meter GetGMMMeter(MSS.Core.Model.Meters.Meter mssMeter)
    {
      return GMMHelper.GetGMMMeter(mssMeter.Id, mssMeter.SerialNumber, mssMeter.DeviceType);
    }

    public static ZENNER.CommonLibrary.Entities.Meter GetGMMMeter(MeterDTO meterDto)
    {
      return GMMHelper.GetGMMMeter(meterDto.Id, meterDto.SerialNumber, meterDto.DeviceType);
    }

    private static ZENNER.CommonLibrary.Entities.Meter GetGMMMeter(
      Guid id,
      string serialNumber,
      DeviceTypeEnum deviceType)
    {
      return new ZENNER.CommonLibrary.Entities.Meter()
      {
        ID = id,
        SerialNumber = serialNumber,
        DeviceModel = GMMHelper.GetDeviceModel(deviceType)
      };
    }

    private static Dictionary<AdditionalInfoKey, string> ConstructMeterAdditionalInfo(
      string manufacturer,
      string inputNumber,
      string generation,
      int? primaryAddress,
      DeviceMediumEnum? deviceMedium,
      string deviceInfo)
    {
      Dictionary<AdditionalInfoKey, string> dictionary = new Dictionary<AdditionalInfoKey, string>();
      if (!string.IsNullOrWhiteSpace(manufacturer))
        dictionary.Add(AdditionalInfoKey.Manufacturer, manufacturer);
      if (!string.IsNullOrWhiteSpace(generation))
        dictionary.Add(AdditionalInfoKey.Version, generation);
      if (deviceMedium.HasValue)
        dictionary.Add(AdditionalInfoKey.Medium, deviceMedium.Value.ToString());
      if (!string.IsNullOrWhiteSpace(deviceInfo))
      {
        string propertyFromDeviceInfo1 = GMMHelper.GetPropertyFromDeviceInfo(deviceInfo, "ZDF:");
        if (!string.IsNullOrWhiteSpace(propertyFromDeviceInfo1))
          dictionary.Add(AdditionalInfoKey.ZDF, propertyFromDeviceInfo1);
        string propertyFromDeviceInfo2 = GMMHelper.GetPropertyFromDeviceInfo(deviceInfo, "MainDeviceSecondaryAddress:");
        if (!string.IsNullOrWhiteSpace(propertyFromDeviceInfo2))
          dictionary.Add(AdditionalInfoKey.MainDeviceSecondaryAddress, propertyFromDeviceInfo2);
      }
      if (!string.IsNullOrWhiteSpace(inputNumber))
        dictionary.Add(AdditionalInfoKey.InputNumber, inputNumber);
      if (primaryAddress.HasValue)
        dictionary.Add(AdditionalInfoKey.PrimaryAddress, primaryAddress.Value.ToString());
      return dictionary;
    }

    public static List<ProfileType> GetProfileTypes(
      IDeviceManager deviceManager,
      ObservableCollection<ExecuteOrderStructureNode> devices,
      List<long> filter,
      EquipmentModel equipment,
      StructureTypeEnum? structureType)
    {
      List<ZENNER.CommonLibrary.Entities.Meter> meters = GMMHelper.GetMeters(devices, filter, structureType);
      meters.ForEach((Action<ZENNER.CommonLibrary.Entities.Meter>) (m => m.AdditionalInfo = (Dictionary<AdditionalInfoKey, string>) null));
      return deviceManager.GetProfileTypes(meters, equipment, new ProfileTypeTags?(ProfileTypeTags.All));
    }

    public static List<ChangeableParameter> ReplaceValuesInChangeableParameters(
      List<ChangeableParameter> changeableParameter,
      List<Config> ConfigValues,
      bool replaceOnlyChangeableParameters = false)
    {
      if (changeableParameter == null)
        return (List<ChangeableParameter>) null;
      if (ConfigValues == null)
        return changeableParameter;
      List<ChangeableParameter> changeableParameterList = new List<ChangeableParameter>();
      foreach (ChangeableParameter changeableParameter1 in changeableParameter)
      {
        foreach (Config configValue1 in ConfigValues)
        {
          Config configValue = configValue1;
          if (configValue.PropertyName == changeableParameter1.Key && (!replaceOnlyChangeableParameters || changeableParameter1.ParameterUsing != 0))
          {
            if (changeableParameter1.AvailableValues != null && changeableParameter1.AvailableValues.Count > 0)
            {
              ValueItem valueItem = changeableParameter1.AvailableValues.FirstOrDefault<ValueItem>((Func<ValueItem, bool>) (av => av.Value.ToString() == configValue.PropertyValue));
              if (valueItem != null)
                changeableParameter1.Value = valueItem.Value;
            }
            else
              changeableParameter1.Value = configValue.PropertyValue;
          }
        }
        changeableParameterList.Add(changeableParameter1);
      }
      return changeableParameterList;
    }

    public static List<DeviceModel> GetDeviceModelBasedOnStructureType(
      StructureTypeEnum? structureType)
    {
      List<DeviceModel> basedOnStructureType = new List<DeviceModel>();
      StructureTypeEnum? nullable = structureType;
      if (nullable.HasValue)
      {
        switch (nullable.GetValueOrDefault())
        {
          case StructureTypeEnum.Physical:
          case StructureTypeEnum.Logical:
            basedOnStructureType = GmmInterface.DeviceManager.GetDeviceModels(DeviceModelTags.Undefined | DeviceModelTags.Radio2 | DeviceModelTags.Radio3 | DeviceModelTags.MBus | DeviceModelTags.wMBus | DeviceModelTags.LoRa).FindAll((Predicate<DeviceModel>) (_ => !_.Parameters.ContainsKey(ConnectionProfileParameter.SystemDevice)));
            break;
          case StructureTypeEnum.Fixed:
            basedOnStructureType = GmmInterface.DeviceManager.GetDeviceModels(DeviceModelTags.Radio3);
            break;
        }
      }
      return basedOnStructureType;
    }

    public static List<string> GetDeviceModelNameList(List<DeviceModel> deviceModelList)
    {
      List<string> modelNames = new List<string>();
      deviceModelList.ForEach((Action<DeviceModel>) (d => modelNames.Add(d.Name)));
      return modelNames;
    }

    public static List<string> GetDeviceModelNameList(StructureTypeEnum? structureType)
    {
      return GMMHelper.GetDeviceModelNameList(GMMHelper.GetDeviceModelBasedOnStructureType(structureType));
    }

    public static bool UpdateAvailableValuesForParam(ChangeableParameter param)
    {
      try
      {
        return param.UpdateAvailableValues();
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
        return false;
      }
    }

    public static string GetPropertyFromDeviceInfo(string deviceInfo, string property)
    {
      if (string.IsNullOrEmpty(deviceInfo))
        return (string) null;
      return ((IEnumerable<string>) deviceInfo.Split('\n')).FirstOrDefault<string>((Func<string, bool>) (s => s.StartsWith(property, StringComparison.CurrentCulture)))?.Replace(property, string.Empty);
    }
  }
}
