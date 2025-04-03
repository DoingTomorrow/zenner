// Decompiled with JetBrains decompiler
// Type: ZENNER.ConfiguratorManager
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using CommunicationPort.Functions;
using GmmDbLib;
using HandlerLib;
using NLog;
using ReadoutConfiguration;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace ZENNER
{
  public sealed class ConfiguratorManager : IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (ConfiguratorManager));
    private bool isCanceled;

    public event EventHandler<int> OnProgress;

    public event EventHandler<string> OnProgressMessage;

    public event System.EventHandler BatterieLow;

    private static ConnectionProfile GetConnectionProfile(
      EquipmentModel equipment,
      ZENNER.CommonLibrary.Entities.Meter meter,
      ProfileType profileType)
    {
      if (equipment == null)
        throw new NullReferenceException(nameof (equipment));
      if (meter == null)
        throw new NullReferenceException(nameof (meter));
      if (profileType == null)
        throw new NullReferenceException(nameof (profileType));
      if (meter.DeviceModel == null)
      {
        string message = Ot.Gtm(Tg.CommunicationLogic, "DeviceModelMissed", "The meter has no device model!");
        throw new InvalidMeterException(meter, message);
      }
      ConnectionProfile connectionProfile = ReadoutConfigFunctions.Manager.GetConnectionProfile(meter.DeviceModel, equipment, profileType);
      if (connectionProfile == null)
      {
        string message = Ot.Gtm(Tg.DB, "ConnectionProfileMissed", "No connection profile exists!") + " Equipment: " + equipment?.ToString() + " ProfileType: " + profileType?.ToString();
        throw new InvalidMeterException(meter, message);
      }
      connectionProfile.EquipmentModel.ChangeableParameters = equipment.ChangeableParameters;
      connectionProfile.DeviceModel.ChangeableParameters = meter.DeviceModel.ChangeableParameters;
      connectionProfile.ProfileType.ChangeableParameters = profileType.ChangeableParameters;
      return connectionProfile;
    }

    public async Task<ICommand> ConnectAsync(
      EquipmentModel equipment,
      ZENNER.CommonLibrary.Entities.Meter meter,
      ProfileType profileType)
    {
      ConnectionProfile profile = ConfiguratorManager.GetConnectionProfile(equipment, meter, profileType);
      ConfigList theConfig = profile.GetConfigListObject();
      CommunicationPortFunctions port = new CommunicationPortFunctions();
      port.SetReadoutConfiguration(theConfig);
      IrDaCommands cmd = new IrDaCommands(port);
      cmd.EquipmentModel = equipment;
      cmd.Meter = meter;
      cmd.ProfileType = profileType;
      ICommand command;
      try
      {
        if (profileType.ProfileTypeID == 73)
        {
          DeviceIdentification version = await cmd.NFC.ReadVersionAsync((ProgressHandler) null, CancellationToken.None);
          version = (DeviceIdentification) null;
        }
        else
        {
          DeviceVersionMBus version = await cmd.ReadVersionAsync((ProgressHandler) null, CancellationToken.None);
          version = (DeviceVersionMBus) null;
        }
        command = (ICommand) cmd;
      }
      catch (Exception ex)
      {
        port.Dispose();
        throw ex;
      }
      profile = (ConnectionProfile) null;
      theConfig = (ConfigList) null;
      port = (CommunicationPortFunctions) null;
      cmd = (IrDaCommands) null;
      return command;
    }

    public int ReadDevice(ConnectionAdjuster connectionAdjuster)
    {
      ConfiguratorManager.logger.Trace("Start ReadDevice by ConnectionAdjuster");
      if (connectionAdjuster == null)
        throw new NullReferenceException(nameof (connectionAdjuster));
      this.isCanceled = false;
      EventHandlerEx<Exception> eventHandlerEx = (EventHandlerEx<Exception>) ((sender, e) =>
      {
        throw e;
      });
      Devices.DeviceManager devices = GmmInterface.Devices;
      try
      {
        ZR_ClassLibMessages.RegisterThreadErrorMsgList();
        if (this.OnProgress != null)
          this.OnProgress((object) this, 1);
        devices.ParameterType = ConfigurationParameter.ValueType.Complete;
        ConfigList mergedConfiguration = connectionAdjuster.GetMergedConfiguration(GmmInterface.DeviceManager.GetConnectionProfile(connectionAdjuster.ConnectionProfileID) ?? throw new Exception(Ot.Gtm(Tg.DB, "ConnectionProfileMissed", "No connection profile exists!") + " Connection name: " + connectionAdjuster.Name));
        devices.PrepareCommunicationStructure(mergedConfiguration);
        devices.OnMessage += new EventHandler<GMM_EventArgs>(this.OnMessage);
        devices.OnProgress += new EventHandlerEx<int>(this.DeviceManager_OnProgress);
        devices.OnProgressMessage += new EventHandlerEx<string>(this.DeviceManager_OnProgressMessage);
        devices.OnError += eventHandlerEx;
        devices.BatterieLow += new System.EventHandler(this.AsynCom_BatterieLow);
        devices.BreakRequest = false;
        if (!devices.Open())
          throw new Exception(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription);
        if (this.OnProgress != null)
          this.OnProgress((object) this, 2);
        if (!devices.SelectedHandler.ReadConfigurationParameters(out GlobalDeviceId _))
        {
          string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
          if (!string.IsNullOrEmpty(errorDescription) && !this.isCanceled)
            throw new Exception(errorDescription);
        }
        if (this.isCanceled)
          return 0;
        List<GlobalDeviceId> globalDeviceIdList = devices.SelectedHandler.GetGlobalDeviceIdList();
        if (globalDeviceIdList == null)
          return 0;
        int num = 0;
        foreach (GlobalDeviceId globalDeviceId in globalDeviceIdList)
        {
          ++num;
          if (globalDeviceId.SubDevices != null)
            num += globalDeviceId.SubDevices.Count;
        }
        return num;
      }
      finally
      {
        if (this.OnProgress != null)
          this.OnProgress((object) this, 100);
        devices.OnMessage -= new EventHandler<GMM_EventArgs>(this.OnMessage);
        devices.OnProgress -= new EventHandlerEx<int>(this.DeviceManager_OnProgress);
        devices.OnProgressMessage -= new EventHandlerEx<string>(this.DeviceManager_OnProgressMessage);
        devices.OnError -= eventHandlerEx;
        devices.BatterieLow -= new System.EventHandler(this.AsynCom_BatterieLow);
        ZR_ClassLibMessages.DeRegisterThreadErrorMsgList();
        ConfiguratorManager.logger.Trace("End ReadDevice");
      }
    }

    public int ReadDevice(EquipmentModel equipment, ZENNER.CommonLibrary.Entities.Meter meter, ProfileType profileType)
    {
      ConfiguratorManager.logger.Trace("Start ReadDevice");
      if (equipment == null)
        throw new NullReferenceException(nameof (equipment));
      if (meter == null)
        throw new NullReferenceException(nameof (meter));
      if (profileType == null)
        throw new NullReferenceException(nameof (profileType));
      if (meter.DeviceModel == null)
      {
        string message = Ot.Gtm(Tg.CommunicationLogic, "DeviceModelMissed", "The meter has no device model!");
        throw new InvalidMeterException(meter, message);
      }
      this.isCanceled = false;
      ConnectionProfile connectionProfile = ReadoutConfigFunctions.Manager.GetConnectionProfile(meter.DeviceModel, equipment, profileType);
      if (connectionProfile == null)
      {
        string message = Ot.Gtm(Tg.DB, "ConnectionProfileMissed", "No connection profile exists!") + " Equipment: " + equipment?.ToString() + " ProfileType: " + profileType?.ToString();
        throw new InvalidMeterException(meter, message);
      }
      connectionProfile.EquipmentModel.ChangeableParameters = equipment.ChangeableParameters;
      connectionProfile.DeviceModel.ChangeableParameters = meter.DeviceModel.ChangeableParameters;
      connectionProfile.ProfileType.ChangeableParameters = profileType.ChangeableParameters;
      EventHandlerEx<Exception> eventHandlerEx = (EventHandlerEx<Exception>) ((sender, e) =>
      {
        throw e;
      });
      Devices.DeviceManager devices = GmmInterface.Devices;
      try
      {
        ZR_ClassLibMessages.RegisterThreadErrorMsgList();
        if (this.OnProgress != null)
          this.OnProgress((object) this, 1);
        devices.ParameterType = ConfigurationParameter.ValueType.Complete;
        ConfigList configListObject = connectionProfile.GetConfigListObject();
        if (!string.IsNullOrEmpty(meter.SerialNumber))
        {
          uint result;
          if (!uint.TryParse(meter.SerialNumber, out result) || result < 0U || result > 99999999U)
            throw new Exception("Illegal serial number");
          configListObject.SecondaryAddress = result;
        }
        devices.PrepareCommunicationStructure(configListObject);
        devices.OnMessage += new EventHandler<GMM_EventArgs>(this.OnMessage);
        devices.OnProgress += new EventHandlerEx<int>(this.DeviceManager_OnProgress);
        devices.OnProgressMessage += new EventHandlerEx<string>(this.DeviceManager_OnProgressMessage);
        devices.OnError += eventHandlerEx;
        devices.BatterieLow += new System.EventHandler(this.AsynCom_BatterieLow);
        devices.BreakRequest = false;
        if (!devices.Open())
          throw new Exception(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription);
        if (this.OnProgress != null)
          this.OnProgress((object) this, 2);
        GlobalDeviceId UpdatedDeviceIdentification;
        if (!devices.SelectedHandler.ReadConfigurationParameters(out UpdatedDeviceIdentification))
        {
          string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
          if (!string.IsNullOrEmpty(errorDescription) && !this.isCanceled)
            throw new Exception(errorDescription);
        }
        if (this.isCanceled)
          return 0;
        if (string.IsNullOrEmpty(meter.SerialNumber))
          meter.SerialNumber = UpdatedDeviceIdentification.Serialnumber;
        List<GlobalDeviceId> globalDeviceIdList = devices.SelectedHandler.GetGlobalDeviceIdList();
        if (globalDeviceIdList == null)
          return 0;
        int num = 0;
        foreach (GlobalDeviceId globalDeviceId in globalDeviceIdList)
        {
          ++num;
          if (globalDeviceId.SubDevices != null)
            num += globalDeviceId.SubDevices.Count;
        }
        return num;
      }
      finally
      {
        if (this.OnProgress != null)
          this.OnProgress((object) this, 100);
        devices.OnMessage -= new EventHandler<GMM_EventArgs>(this.OnMessage);
        devices.OnProgress -= new EventHandlerEx<int>(this.DeviceManager_OnProgress);
        devices.OnProgressMessage -= new EventHandlerEx<string>(this.DeviceManager_OnProgressMessage);
        devices.OnError -= eventHandlerEx;
        devices.BatterieLow -= new System.EventHandler(this.AsynCom_BatterieLow);
        ZR_ClassLibMessages.DeRegisterThreadErrorMsgList();
        ConfiguratorManager.logger.Trace("End ReadDevice");
      }
    }

    public void WriteDevice()
    {
      ConfiguratorManager.logger.Trace("Start WriteDevice");
      Devices.DeviceManager devices = GmmInterface.Devices;
      if (devices.SelectedHandler == null)
        return;
      this.isCanceled = false;
      EventHandlerEx<Exception> eventHandlerEx = (EventHandlerEx<Exception>) ((sender, e) =>
      {
        throw e;
      });
      try
      {
        devices.OnError += eventHandlerEx;
        devices.BreakRequest = false;
        devices.ParameterType = ConfigurationParameter.ValueType.Complete;
        if (!devices.Open())
          throw new Exception(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription);
        if (!devices.SelectedHandler.WriteChangedConfigurationParametersToDevice())
          throw new Exception(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription);
      }
      finally
      {
        devices.OnError -= eventHandlerEx;
      }
      ConfiguratorManager.logger.Trace("End WriteDevice");
    }

    public void Cancel() => this.isCanceled = true;

    public SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(int channel)
    {
      Devices.DeviceManager devices = GmmInterface.Devices;
      return devices.SelectedHandler == null ? (SortedList<OverrideID, ConfigurationParameter>) null : devices.SelectedHandler.GetConfigurationParameters(ConfigurationParameter.ValueType.Complete, channel);
    }

    public void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameter)
    {
      this.SetConfigurationParameters(parameter, 0);
    }

    public void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameter,
      int channel)
    {
      Devices.DeviceManager devices = GmmInterface.Devices;
      if (parameter == null || devices.SelectedHandler == null)
        return;
      SortedList<OverrideID, ConfigurationParameter> parameterList = new SortedList<OverrideID, ConfigurationParameter>();
      SortedList<OverrideID, ConfigurationParameter> configurationParameters = this.GetConfigurationParameters(channel);
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in parameter)
      {
        if (keyValuePair.Value.HasWritePermission)
        {
          if (configurationParameters != null && configurationParameters.ContainsKey(keyValuePair.Key))
          {
            if (keyValuePair.Value.ParameterInfo.FormatControlled)
            {
              if (keyValuePair.Value.GetStringValueWin() == configurationParameters[keyValuePair.Key].GetStringValueWin())
                continue;
            }
            else
            {
              object parameterValue1 = configurationParameters[keyValuePair.Key].ParameterValue;
              object parameterValue2 = keyValuePair.Value.ParameterValue;
              if (parameterValue1 != null && parameterValue2 != null && parameterValue1.ToString() == parameterValue2.ToString() || parameterValue1 == null && parameterValue2 == null)
                continue;
            }
          }
          parameterList.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
      devices.SelectedHandler.SetConfigurationParameters(parameterList, channel);
    }

    public void Dispose() => ZR_ClassLibMessages.DeRegisterThreadErrorMsgList();

    public void CloseConnection() => GmmInterface.Devices.Close();

    private void OnMessage(object sender, GMM_EventArgs e) => e.Cancel = this.isCanceled;

    private void DeviceManager_OnProgress(object sender, int e)
    {
      if (this.OnProgress == null)
        return;
      this.OnProgress(sender, e);
    }

    private void DeviceManager_OnProgressMessage(object sender, string e)
    {
      if (this.OnProgressMessage == null)
        return;
      this.OnProgressMessage(sender, e);
    }

    public bool ShowHandler()
    {
      if (!UserManager.CheckPermission("Developer"))
        return false;
      GmmInterface.Devices.ShowHandlerWindow();
      return true;
    }

    private void AsynCom_BatterieLow(object sender, EventArgs e)
    {
      if (this.BatterieLow == null)
        return;
      this.BatterieLow(sender, e);
    }
  }
}
