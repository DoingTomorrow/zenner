// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.DeviceConfigurator
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Errors;
using MSS.Business.Utils;
using MSS.Core.Model.Meters;
using MSS.DTO.Meters;
using MSS.DTO.Structures;
using MSS.Localisation;
using ReadoutConfiguration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ZENNER;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public class DeviceConfigurator
  {
    public static async Task<ParamsReturnedUsingIrExpando> WriteParametersForDevice(
      EquipmentModel equipment,
      LocationDTO location,
      MeterDTO device)
    {
      string radioId = (string) null;
      ConfiguratorManager configManager = GmmInterface.ConfiguratorManager;
      try
      {
        ZENNER.CommonLibrary.Entities.Meter meter;
        ProfileType profileType;
        DeviceConfigurator.InitializeGMMParameters(device.DeviceType, out meter, out profileType);
        DeviceTypeEnum deviceType = device.DeviceType;
        if (deviceType == DeviceTypeEnum.M7 || deviceType == DeviceTypeEnum.MinomessMicroRadio3 || deviceType == DeviceTypeEnum.MinotelContactRadio3 || deviceType == DeviceTypeEnum.C5Radio)
        {
          MessageHandler.LogDebug("Start read - " + (object) DateTime.Now);
          int count = configManager.ReadDevice(equipment, meter, profileType);
          MessageHandler.LogDebug("End read - " + (object) DateTime.Now);
          if (count > 0)
          {
            SortedList<OverrideID, ConfigurationParameter> prms = configManager.GetConfigurationParameters(0);
            if (prms.ContainsKey(OverrideID.SerialNumber))
              radioId = prms[OverrideID.SerialNumber].GetStringValueDb();
            int scenarioToBeWritten = location.Scenario.Code != 4 ? location.Scenario.Code : 1;
            switch (deviceType)
            {
              case DeviceTypeEnum.C5Radio:
                DeviceConfigurator.SetDueDateMonth(location.DueDate, prms);
                DeviceConfigurator.SetConfigParam(prms, OverrideID.RadioProtocol, scenarioToBeWritten.ToString());
                DeviceConfigurator.SetPulseStartValue(device, prms);
                break;
              case DeviceTypeEnum.M7:
                DeviceConfigurator.SetDueDateValue(location.DueDate, prms);
                DeviceConfigurator.SetConfigParam(prms, OverrideID.RadioProtocol, scenarioToBeWritten.ToString());
                break;
              case DeviceTypeEnum.MinomessMicroRadio3:
                DeviceConfigurator.SetDueDateValue(location.DueDate, prms);
                DeviceConfigurator.SetConfigParam(prms, OverrideID.RadioProtocol, scenarioToBeWritten.ToString());
                DeviceConfigurator.SetPulseStartValue(device, prms);
                break;
              case DeviceTypeEnum.MinotelContactRadio3:
                DeviceConfigurator.SetDueDateValue(location.DueDate, prms);
                DeviceConfigurator.SetConfigParam(prms, OverrideID.RadioProtocol, scenarioToBeWritten.ToString());
                int channelCount = int.Parse(device.Channel.Code);
                SortedList<OverrideID, ConfigurationParameter> channelPms = configManager.GetConfigurationParameters(channelCount + 1);
                DeviceConfigurator.SetPulseStartValue(device, channelPms);
                if (channelPms.ContainsKey(OverrideID.SerialNumber))
                {
                  radioId = channelPms[OverrideID.SerialNumber].GetStringValueDb();
                  break;
                }
                break;
            }
            MessageHandler.LogDebug("Start write - " + (object) DateTime.Now);
            configManager.WriteDevice();
            MessageHandler.LogDebug("End write - " + (object) DateTime.Now);
            prms = (SortedList<OverrideID, ConfigurationParameter>) null;
          }
          else
            return new ParamsReturnedUsingIrExpando()
            {
              IsSuccess = false,
              Message = Resources.MSS_Client_Exception_Title
            };
        }
        meter = (ZENNER.CommonLibrary.Entities.Meter) null;
        profileType = (ProfileType) null;
      }
      catch (Exception ex)
      {
        return new ParamsReturnedUsingIrExpando()
        {
          IsSuccess = false,
          Message = ex.Message
        };
      }
      finally
      {
        configManager.CloseConnection();
      }
      ParamsReturnedUsingIrExpando result = new ParamsReturnedUsingIrExpando()
      {
        IsSuccess = true,
        RadioId = radioId
      };
      return result;
    }

    private static void SetDueDateMonth(
      DateTime? dueDate,
      SortedList<OverrideID, ConfigurationParameter> prms)
    {
      if (!dueDate.HasValue)
        return;
      DeviceConfigurator.SetConfigParam(prms, OverrideID.DueDateMonth, dueDate.Value.Month.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    private static void SetDueDateValue(
      DateTime? dueDate,
      SortedList<OverrideID, ConfigurationParameter> prms)
    {
      if (!dueDate.HasValue)
        return;
      DeviceConfigurator.SetConfigParam(prms, OverrideID.DueDate, dueDate.Value.ToString((IFormatProvider) new CultureInfo("de-DE")));
    }

    private static void SetPulseStartValue(
      MeterDTO meter,
      SortedList<OverrideID, ConfigurationParameter> prms)
    {
      double? impulses = UnitConversionHelper.ConvertValueToImpulses(meter.StartValue, meter.ReadingUnit.Code, meter.ImpulsValue, meter.ImpulsUnit.Code);
      if (!impulses.HasValue)
        return;
      DeviceConfigurator.SetConfigParam(prms, OverrideID.InputActualValue, impulses.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    private static ParamsReturnedUsingIrExpando SetConfigParam(
      SortedList<OverrideID, ConfigurationParameter> prms,
      OverrideID key,
      string value)
    {
      if (prms != null && prms.ContainsKey(key))
      {
        ConfigurationParameter prm = prms[key];
        if (value != null)
        {
          prm.SetValueFromStringDb(value);
          GmmInterface.ConfiguratorManager.SetConfigurationParameters(new SortedList<OverrideID, ConfigurationParameter>()
          {
            {
              key,
              prm
            }
          });
          return new ParamsReturnedUsingIrExpando()
          {
            IsSuccess = true
          };
        }
        return new ParamsReturnedUsingIrExpando()
        {
          IsSuccess = false,
          Message = string.Format(Resources.MSS_Client_Exception_Title, (object) key)
        };
      }
      return new ParamsReturnedUsingIrExpando()
      {
        IsSuccess = false,
        Message = string.Format(Resources.MSS_Client_Exception_Title, (object) key)
      };
    }

    private static void InitializeGMMParameters(
      DeviceTypeEnum deviceType,
      out ZENNER.CommonLibrary.Entities.Meter meter,
      out ProfileType profileType)
    {
      DeviceManager deviceManager = GmmInterface.DeviceManager;
      meter = new ZENNER.CommonLibrary.Entities.Meter()
      {
        DeviceModel = GMMHelper.GetDeviceModel(deviceType),
        ID = Guid.NewGuid()
      };
      profileType = deviceManager.GetProfileTypes(meter).FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (p => p.Name.Contains("IR")));
    }
  }
}
