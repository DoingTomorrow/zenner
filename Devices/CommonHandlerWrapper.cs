// Decompiled with JetBrains decompiler
// Type: Devices.CommonHandlerWrapper
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using HandlerLib;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace Devices
{
  public class CommonHandlerWrapper : BaseDevice
  {
    public HandlerFunctionsForProduction HandlerInterface;

    internal string HandlerName { get; private set; }

    public override event EventHandlerEx<int> OnProgress;

    public CommonHandlerWrapper(
      DeviceManager MyDeviceManager,
      HandlerFunctionsForProduction handlerInterface)
      : base(MyDeviceManager)
    {
      this.HandlerInterface = handlerInterface;
    }

    public static CommonHandlerWrapper CreateFromHandlerName(
      DeviceManager theDeviceManager,
      ConfigList configList,
      string handlerName,
      bool isPlugin)
    {
      if (theDeviceManager.MyCommunicationPort == null || theDeviceManager.MyCommunicationPort.portFunctions == null)
        throw new Exception("CommunicationPortFunctions not defined");
      HandlerFunctionsForProduction handlerInterface;
      if (isPlugin)
      {
        handlerInterface = ((IWindowFunctions) PlugInLoader.GetPlugIn(handlerName).GetPluginInfo().Interface).GetFunctions() as HandlerFunctionsForProduction;
      }
      else
      {
        handlerInterface = Activator.CreateInstance(((IEnumerable<Type>) Assembly.LoadFrom(handlerName + ".dll").GetTypes()).First<Type>((Func<Type, bool>) (x => x.BaseType == typeof (HandlerFunctionsForProduction))), (object) theDeviceManager.MyCommunicationPort.portFunctions, (object) StartupManager.Database.BaseDbConnection) as HandlerFunctionsForProduction;
        handlerInterface.SetReadoutConfiguration(configList);
      }
      return new CommonHandlerWrapper(theDeviceManager, handlerInterface)
      {
        HandlerName = handlerName
      };
    }

    public override bool Open()
    {
      this.HandlerInterface.Open();
      return true;
    }

    public override bool Close()
    {
      this.HandlerInterface.Close();
      return true;
    }

    public override async Task<int> ReadDeviceAsync(
      ProgressHandler progress,
      CancellationToken token,
      ReadPartsSelection readPartsSelection)
    {
      int num = await this.HandlerInterface.ReadDeviceAsync(progress, token, readPartsSelection);
      return num;
    }

    public override async Task WriteDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.HandlerInterface.WriteDeviceAsync(progress, token);
    }

    public override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      SortedList<OverrideID, ConfigurationParameter> configurationParameters = this.HandlerInterface.GetConfigurationParameters(SubDevice);
      if (configurationParameters == null)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      for (int index = configurationParameters.Count - 1; index >= 0; --index)
      {
        if (configurationParameters.Values[index].ParameterID.ToString().StartsWith("MenuView"))
          configurationParameters.RemoveAt(index);
        if (!UserManager.IsConfigParamVisible(configurationParameters.Values[index].ParameterID))
          configurationParameters.RemoveAt(index);
      }
      return configurationParameters;
    }

    public override bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> ConfigParameterList,
      int SubDevice)
    {
      this.HandlerInterface.SetConfigurationParameters(ConfigParameterList, SubDevice);
      return true;
    }

    public override List<GlobalDeviceId> GetGlobalDeviceIdList()
    {
      List<GlobalDeviceId> globalDeviceIdList = new List<GlobalDeviceId>();
      DeviceIdentification deviceIdentification1 = this.HandlerInterface.GetDeviceIdentification();
      GlobalDeviceId globalDeviceId = new GlobalDeviceId();
      globalDeviceId.Serialnumber = deviceIdentification1.PrintedSerialNumberAsString;
      globalDeviceId.FirmwareVersion = deviceIdentification1.GetFirmwareVersionString();
      globalDeviceIdList.Add(globalDeviceId);
      if (deviceIdentification1.SubChannels != null)
      {
        foreach (int subChannel in deviceIdentification1.SubChannels)
        {
          DeviceIdentification deviceIdentification2 = this.HandlerInterface.GetDeviceIdentification(subChannel);
          globalDeviceId.SubDevices.Add(new GlobalDeviceId()
          {
            Serialnumber = deviceIdentification2.PrintedSerialNumberAsString
          });
        }
      }
      return globalDeviceIdList;
    }

    public override bool ReadConfigurationParameters(out GlobalDeviceId UpdatedDeviceIdentification)
    {
      ReadPartsSelection parts = ReadPartsSelection.AllWithoutLogger;
      if (this.HandlerName == "S4_Handler")
      {
        ConfigurationParameter.ActiveConfigurationLevel = ConfigurationLevel.Huge;
        parts |= ReadPartsSelection.SmartFunctions;
        parts |= ReadPartsSelection.ProtocolOnlyMode;
      }
      UpdatedDeviceIdentification = (GlobalDeviceId) null;
      AsyncHelpers.RunSync<int>((Func<Task<int>>) (() => this.ReadDeviceAsync(new ProgressHandler((Action<ProgressArg>) (x =>
      {
        if (this.OnProgress == null)
          return;
        this.OnProgress((object) this, Convert.ToInt32(x.ProgressPercentage));
      })), new CancellationTokenSource().Token, parts)));
      UpdatedDeviceIdentification = new GlobalDeviceId();
      return UpdatedDeviceIdentification != null;
    }

    public override bool WriteChangedConfigurationParametersToDevice()
    {
      AsyncHelpers.RunSync((Func<Task>) (() => this.WriteDeviceAsync(new ProgressHandler((Action<ProgressArg>) (x =>
      {
        if (this.OnProgress == null)
          return;
        this.OnProgress((object) this, Convert.ToInt32(x.ProgressPercentage));
      })), new CancellationTokenSource().Token)));
      return true;
    }
  }
}
