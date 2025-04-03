// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Configuration.ConfigurationValuesToListConfigConverter
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using GmmDbLib;
using MSS.Core.Model.ApplicationParamenters;
using System;
using System.Collections.Generic;
using System.Linq;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.Configuration
{
  public class ConfigurationValuesToListConfigConverter : IConfigurationValuesToListConfigConverter
  {
    public List<Config> GetConfigurationValues(
      SortedList<OverrideID, ConfigurationParameter> parameters)
    {
      List<Config> configurationValues = new List<Config>();
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> parameter in parameters)
      {
        Config config = new Config()
        {
          PropertyName = EnumTranslator.GetTranslatedEnumName((object) parameter.Value.ParameterID),
          Description = EnumTranslator.GetTranslatedEnumDescription((object) parameter.Value.ParameterID),
          ProperListValues = (List<ConfigurationPropertyValue>) null,
          Parameter = (object) parameter,
          IsReadOnly = !parameter.Value.HasWritePermission,
          Type = this.GetConfigType(parameter.Value),
          PropertyValue = this.GetPropertyValue(parameter.Value)
        };
        config.ProperListValues = this.GetProperListValues(parameter.Value);
        configurationValues.Add(config);
      }
      return configurationValues;
    }

    private string GetConfigType(ConfigurationParameter pValue)
    {
      if (pValue.ParameterValue == null || !pValue.HasWritePermission)
        return ViewObjectTypeEnum.TextBox.ToString();
      Type type = pValue.ParameterValue.GetType();
      if (type != (Type) null && type == typeof (string[]) && pValue != null && pValue.AllowedValues != null && pValue.AllowedValues.GetType() == typeof (string[]))
        return ViewObjectTypeEnum.MultiSelectionComboBox.ToString();
      if (type.IsEnum || pValue.AllowedValues != null && pValue.AllowedValues.Length != 0)
        return ViewObjectTypeEnum.ComboBox.ToString();
      return typeof (bool) == type ? ViewObjectTypeEnum.CheckBox.ToString() : ViewObjectTypeEnum.TextBox.ToString();
    }

    private string GetPropertyValue(ConfigurationParameter pValue)
    {
      if (pValue.ParameterValue == null || !pValue.HasWritePermission)
        return pValue.ToString();
      if (pValue.ParameterValue is DateTime)
      {
        DateTime parameterValue = (DateTime) pValue.ParameterValue;
        return parameterValue.TimeOfDay.Ticks == 0L ? parameterValue.ToString("d") : parameterValue.ToString();
      }
      Type type = pValue.ParameterValue.GetType();
      if (type.IsEnum)
        return Enum.Parse(type, pValue.ParameterValue.ToString(), true).ToString();
      if (type.IsArray && type.GetElementType() == typeof (string))
        return string.Join(",", (string[]) pValue.ParameterValue);
      if (pValue.AllowedValues != null && pValue.AllowedValues.Length != 0)
        return pValue.ParameterValue.ToString();
      return typeof (bool) == type ? Convert.ToBoolean(pValue.ParameterValue).ToString() : pValue.ToString();
    }

    private List<ConfigurationPropertyValue> GetProperListValues(ConfigurationParameter pValue)
    {
      if (pValue.ParameterValue == null)
        return (List<ConfigurationPropertyValue>) null;
      Type type = pValue.ParameterValue.GetType();
      if (!type.IsEnum && (pValue.AllowedValues == null || pValue.AllowedValues.Length == 0))
        return (List<ConfigurationPropertyValue>) null;
      if (!type.IsEnum)
        return ((IEnumerable<string>) pValue.AllowedValues).Select<string, ConfigurationPropertyValue>((Func<string, ConfigurationPropertyValue>) (x => new ConfigurationPropertyValue()
        {
          DisplayName = x,
          Value = x,
          OriginalName = x
        })).ToList<ConfigurationPropertyValue>();
      return pValue.AllowedValues == null ? Enum.GetValues(type).Cast<object>().Select<object, string>((Func<object, string>) (value => value.ToString())).ToList<string>().Select<string, ConfigurationPropertyValue>((Func<string, ConfigurationPropertyValue>) (x => new ConfigurationPropertyValue()
      {
        DisplayName = x,
        Value = x,
        OriginalName = x
      })).ToList<ConfigurationPropertyValue>() : ((IEnumerable<string>) pValue.AllowedValues).Select<string, string>((Func<string, string>) (item => Enum.Parse(type, item).ToString())).ToList<string>().Select<string, ConfigurationPropertyValue>((Func<string, ConfigurationPropertyValue>) (x => new ConfigurationPropertyValue()
      {
        DisplayName = x,
        Value = x,
        OriginalName = x
      })).ToList<ConfigurationPropertyValue>();
    }
  }
}
