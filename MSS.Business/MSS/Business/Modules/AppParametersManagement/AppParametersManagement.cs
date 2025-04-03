// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.AppParametersManagement.AppParametersManagement
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Errors;
using MSS.Business.Modules.Configuration;
using MSS.Business.Modules.GMM;
using MSS.Core.Model.ApplicationParamenters;
using MSS.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS.Business.Modules.AppParametersManagement
{
  public class AppParametersManagement
  {
    private readonly IRepository<ApplicationParameter> _appParamRepository;

    public AppParametersManagement(IRepositoryFactory repositoryFactory)
    {
      this._appParamRepository = repositoryFactory.GetRepository<ApplicationParameter>();
    }

    public IEnumerable<ApplicationParameter> GetApplicationParameters()
    {
      return (IEnumerable<ApplicationParameter>) this._appParamRepository.GetAll();
    }

    public void Update(ApplicationParameter appParameter)
    {
      this._appParamRepository.Update(appParameter);
    }

    public ApplicationParameter GetAppParam(string value)
    {
      return this._appParamRepository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (applicationParameter => value.Equals(applicationParameter.Parameter)));
    }

    public T GetAppParam<T>(string parameterName)
    {
      return (T) Convert.ChangeType((object) this._appParamRepository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (applicationParameter => parameterName.Equals(applicationParameter.Parameter))).Value, typeof (T));
    }

    private static string SerializeConfigsList(List<Config> configList)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (List<Config>));
      TextWriter textWriter = (TextWriter) new StringWriter();
      xmlSerializer.Serialize(textWriter, (object) configList);
      textWriter.Close();
      return textWriter.ToString();
    }

    public static List<Config> DeserializeChangeableParams(string configParamValue)
    {
      if (string.IsNullOrEmpty(configParamValue))
        return (List<Config>) null;
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (List<Config>));
      StringReader stringReader = new StringReader(configParamValue);
      List<Config> configList = (List<Config>) xmlSerializer.Deserialize((TextReader) stringReader);
      stringReader.Close();
      return configList;
    }

    public static async Task<List<Config>> CreateEquipmentConfigsList(EquipmentModel equipment)
    {
      if (equipment?.ChangeableParameters == null)
        return (List<Config>) null;
      List<ChangeableParameter> changeableParams = equipment.ChangeableParameters;
      List<Config> configsList = changeableParams.Select<ChangeableParameter, Config>((Func<ChangeableParameter, Config>) (changeableParam =>
      {
        Config equipmentConfigsList = new Config();
        equipmentConfigsList.IsReadOnly = false;
        equipmentConfigsList.ProperListValues = !GMMHelper.UpdateAvailableValuesForParam(changeableParam) || changeableParam.AvailableValues == null ? (List<ConfigurationPropertyValue>) null : MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetAvailableConfigs(changeableParam.AvailableValues);
        equipmentConfigsList.PropertyName = changeableParam.Key;
        Config config = equipmentConfigsList;
        List<ValueItem> availableValues = changeableParam.AvailableValues;
        string str = (availableValues != null ? availableValues.FirstOrDefault<ValueItem>((Func<ValueItem, bool>) (p => p.Value == changeableParam.Value)) : (ValueItem) null) != null ? changeableParam.AvailableValues.FirstOrDefault<ValueItem>((Func<ValueItem, bool>) (p => p.Value == changeableParam.Value)).Value : (MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetControlType(changeableParam) == ViewObjectTypeEnum.CheckBox.ToString() ? changeableParam.Value : string.Empty);
        config.PropertyValue = str;
        equipmentConfigsList.Type = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetControlType(changeableParam);
        equipmentConfigsList.Parameter = (object) changeableParam;
        return equipmentConfigsList;
      })).ToList<Config>();
      return configsList;
    }

    public static List<ConfigurationPropertyValue> GetAvailableConfigs(
      List<ValueItem> availableValues)
    {
      return availableValues.Select<ValueItem, ConfigurationPropertyValue>((Func<ValueItem, ConfigurationPropertyValue>) (currentValue => new ConfigurationPropertyValue()
      {
        OriginalName = string.Format("{0}", (object) currentValue),
        DisplayName = string.Format("{0}", (object) currentValue),
        Value = currentValue.Value
      })).ToList<ConfigurationPropertyValue>();
    }

    public static string GetControlType(ChangeableParameter param)
    {
      if (param.UpdateAvailableValuesHandler != null)
        return ViewObjectTypeEnum.ComboBox.ToString();
      return typeof (bool) == param.Type ? ViewObjectTypeEnum.CheckBox.ToString() : ViewObjectTypeEnum.TextBox.ToString();
    }

    public static List<Config> GetConfigListFromChangeableParameters(
      List<ChangeableParameter> changeableParams)
    {
      if (changeableParams == null)
        return (List<Config>) null;
      List<Config> list = changeableParams.Select<ChangeableParameter, Config>((Func<ChangeableParameter, Config>) (changeableParam => MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetConfigFromChangeableParameters(changeableParam))).ToList<Config>();
      return list.Any<Config>() ? list : (List<Config>) null;
    }

    public static Config GetConfigFromChangeableParameters(ChangeableParameter changeableParam)
    {
      if (changeableParam.Key == "COMserver" && changeableParam.Value == "-")
        changeableParam.Value = string.Empty;
      Config changeableParameters = new Config();
      changeableParameters.IsReadOnly = false;
      changeableParameters.ProperListValues = !GMMHelper.UpdateAvailableValuesForParam(changeableParam) || changeableParam.AvailableValues == null ? (List<ConfigurationPropertyValue>) null : MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetAvailableConfigs(changeableParam.AvailableValues);
      changeableParameters.PropertyName = changeableParam.Key;
      Config config = changeableParameters;
      List<ValueItem> availableValues = changeableParam.AvailableValues;
      string str = (availableValues != null ? availableValues.FirstOrDefault<ValueItem>((Func<ValueItem, bool>) (p => p.Value == changeableParam.Value))?.Value : (string) null) ?? changeableParam.Value ?? string.Empty;
      config.PropertyValue = str;
      changeableParameters.Type = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetControlType(changeableParam);
      changeableParameters.Parameter = (object) changeableParam;
      return changeableParameters;
    }

    public static EquipmentModel UpdateEquipmentWithSavedParams(
      EquipmentModel equipment,
      string dbParamsString)
    {
      List<ChangeableParameter> changeableParameters = equipment?.ChangeableParameters;
      if (changeableParameters == null)
        return equipment;
      MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateChangeableParamsWithSavedParams(changeableParameters, dbParamsString);
      return equipment;
    }

    public static ProfileType UpdateProfileTypeWithSavedParams(
      ProfileType profileType,
      string dbParamsString)
    {
      List<ChangeableParameter> changeableParameters = profileType.ChangeableParameters;
      if (changeableParameters == null)
        return profileType;
      MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateChangeableParamsWithSavedParams(changeableParameters, dbParamsString);
      return profileType;
    }

    public static Meter UpdateDeviceWithSavedParams(Meter meter, string dbParamsString)
    {
      List<ChangeableParameter> changeableParameters = meter.DeviceModel.ChangeableParameters;
      if (changeableParameters == null)
        return meter;
      MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateChangeableParamsWithSavedParams(changeableParameters, dbParamsString);
      return meter;
    }

    public static DeviceModel UpdateDeviceModelWithSavedParams(
      DeviceModel deviceModel,
      string dbParamsString)
    {
      List<ChangeableParameter> changeableParameters = deviceModel.ChangeableParameters;
      if (changeableParameters == null)
        return deviceModel;
      MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateChangeableParamsWithSavedParams(changeableParameters, dbParamsString);
      return deviceModel;
    }

    public static void UpdateChangeableParamsWithSavedParams(
      List<ChangeableParameter> changeableParams,
      List<Config> paramsList)
    {
      foreach (ChangeableParameter changeableParam in changeableParams)
      {
        ChangeableParameter parameter = changeableParam;
        Config dbParam = paramsList.FirstOrDefault<Config>((Func<Config, bool>) (e => e.PropertyName == parameter.Key));
        if (dbParam != null)
        {
          try
          {
            if (GMMHelper.UpdateAvailableValuesForParam(parameter) && parameter.AvailableValues != null && parameter.AvailableValues.Any<ValueItem>())
            {
              ValueItem valueItem1 = parameter.AvailableValues.FirstOrDefault<ValueItem>((Func<ValueItem, bool>) (v => v.Value.ToString() == dbParam.PropertyValue));
              if (valueItem1 != null)
              {
                parameter.Value = valueItem1.Value;
              }
              else
              {
                ValueItem valueItem2 = parameter.AvailableValues.FirstOrDefault<ValueItem>();
                parameter.Value = valueItem2?.Value;
              }
            }
            else
              parameter.Value = dbParam.PropertyValue;
          }
          catch (Exception ex)
          {
            MessageHandler.LogException(ex);
          }
        }
      }
    }

    private static void UpdateChangeableParamsWithSavedParams(
      List<ChangeableParameter> changeableParams,
      string dbParamsString)
    {
      List<Config> paramsList = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.DeserializeChangeableParams(dbParamsString);
      MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateChangeableParamsWithSavedParams(changeableParams, paramsList);
    }

    public void UpdateDefaultEquipment(
      EquipmentModel selectedEquipmentModel,
      List<Config> changeableParameters)
    {
      ApplicationParameter appParam1 = this.GetAppParam("DefaultEquipmentParams");
      ApplicationParameter appParam2 = this.GetAppParam("DefaultEquipment");
      if (selectedEquipmentModel != null)
      {
        if (!selectedEquipmentModel.Name.Equals(appParam2.Value))
        {
          appParam2.Value = selectedEquipmentModel.Name;
          this.Update(appParam2);
        }
        List<ChangeableParameter> changeableParameters1 = selectedEquipmentModel.ChangeableParameters;
        List<Config> configsList = changeableParameters;
        if (configsList != null)
        {
          string str = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.SerializedConfigList(configsList, changeableParameters1);
          if (appParam1.Value == null || !appParam1.Value.Equals(str))
          {
            appParam1.Value = str;
            this.Update(appParam1);
          }
        }
        else
        {
          appParam1.Value = string.Empty;
          this.Update(appParam1);
        }
      }
      else
      {
        appParam2.Value = string.Empty;
        this.Update(appParam2);
      }
      MSS.Business.Utils.AppContext.Current.LoadApplicationParameters(this._appParamRepository.GetAll());
      MSS.Business.Utils.AppContext.Current.LoadDefaultEquipment();
    }

    public bool UpdateExpertConfigurationMode(bool isExpertConfigurationMode)
    {
      ApplicationParameter appParam = this.GetAppParam("ExpertConfigurationMode");
      if (appParam == null)
        return false;
      appParam.Value = isExpertConfigurationMode.ToString();
      this.Update(appParam);
      MSS.Business.Utils.AppContext.Current.LoadApplicationParameters(this._appParamRepository.GetAll());
      return true;
    }

    public static List<Config> CreateScanConfigsList(
      List<ChangeableParameter> changeableParams,
      List<string> skipParameterKeys)
    {
      return changeableParams == null ? (List<Config>) null : changeableParams.Where<ChangeableParameter>((Func<ChangeableParameter, bool>) (config => !skipParameterKeys.Contains(config.Key))).Select<ChangeableParameter, Config>((Func<ChangeableParameter, Config>) (changeableParam => new Config()
      {
        IsReadOnly = false,
        ProperListValues = !GMMHelper.UpdateAvailableValuesForParam(changeableParam) || changeableParam.AvailableValues == null ? (List<ConfigurationPropertyValue>) null : MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetAvailableConfigs(changeableParam.AvailableValues),
        PropertyName = changeableParam.Key,
        PropertyValue = changeableParam.Value,
        Type = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetControlType(changeableParam),
        Parameter = (object) changeableParam
      })).ToList<Config>();
    }

    public static string SerializeEquipementParams(EquipmentModel equipementModel)
    {
      return MSS.Business.Modules.AppParametersManagement.AppParametersManagement.SerializedConfigList(MSS.Business.Modules.AppParametersManagement.AppParametersManagement.CreateEquipmentConfigsList(equipementModel).Result, equipementModel.ChangeableParameters);
    }

    public static string SerializedConfigList(
      List<Config> configsList,
      List<ChangeableParameter> changeableParams)
    {
      if (configsList == null)
        return (string) null;
      foreach (Config configs in configsList)
      {
        Config config = configs;
        ChangeableParameter changeableParameter = changeableParams.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (p => p.Key == config.PropertyName));
        if (changeableParameter != null)
          changeableParameter.Value = config.PropertyValue;
        config.Parameter = (object) null;
      }
      return MSS.Business.Modules.AppParametersManagement.AppParametersManagement.SerializeConfigsList(configsList);
    }

    public void ResetSystemAndScanMode()
    {
      ApplicationParameter appParam1 = this.GetAppParam("System");
      appParam1.Value = string.Empty;
      this.Update(appParam1);
      ApplicationParameter appParam2 = this.GetAppParam("ScanModeParams");
      ApplicationParameter appParam3 = this.GetAppParam("ScanMode");
      appParam3.Value = string.Empty;
      this.Update(appParam3);
      appParam2.Value = string.Empty;
      this.Update(appParam2);
    }
  }
}
