// Decompiled with JetBrains decompiler
// Type: Devices.SmokeDetectorHandler
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using DeviceCollector;
using HandlerLib;
using NLog;
using SmokeDetectorHandler;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace Devices
{
  public class SmokeDetectorHandler(DeviceManager MyDeviceManager) : BaseDevice(MyDeviceManager)
  {
    private SortedList<OverrideID, ConfigurationParameter> changedParameters;
    private static Logger logger = LogManager.GetLogger(nameof (SmokeDetectorHandler));
    private SmokeDetectorHandlerFunctions smokeDetector;

    public override event EventHandlerEx<int> OnProgress;

    public override object GetHandler()
    {
      this.GarantHandlerLoaded();
      return (object) this.smokeDetector;
    }

    private void GarantHandlerLoaded()
    {
      if (this.smokeDetector != null)
        return;
      if (ZR_Component.CommonGmmInterface != null)
      {
        ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.SmokeDetectorHandler);
        this.smokeDetector = (SmokeDetectorHandlerFunctions) ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.SmokeDetectorHandler];
      }
      else
        this.smokeDetector = new SmokeDetectorHandlerFunctions((IDeviceCollector) this.MyDeviceManager.MyBus);
      this.smokeDetector.OnProgress += new ValueEventHandler<int>(this.smokeDetector_OnProgress);
    }

    private void smokeDetector_OnProgress(object sender, int e)
    {
      if (this.OnProgress == null)
        return;
      this.OnProgress(sender, e);
    }

    internal override void ShowHandlerWindow()
    {
      this.GarantHandlerLoaded();
      this.smokeDetector.ShowSmokeDetectorWindow();
    }

    public override List<GlobalDeviceId> GetGlobalDeviceIdList()
    {
      this.GarantHandlerLoaded();
      if (this.smokeDetector == null || this.smokeDetector.WorkMeter == null && this.smokeDetector.ConnectedMeterMinoprotectII == null)
        return (List<GlobalDeviceId>) null;
      SmokeDetectorVersion smokeDetectorVersion = (SmokeDetectorVersion) null;
      if (this.smokeDetector.WorkMeter != null)
        smokeDetectorVersion = this.smokeDetector.WorkMeter.Version;
      else if (this.smokeDetector.ConnectedMeterMinoprotectII != null)
        smokeDetectorVersion = this.smokeDetector.ConnectedMeterMinoprotectII.Version;
      if (smokeDetectorVersion == null)
        return (List<GlobalDeviceId>) null;
      return new List<GlobalDeviceId>()
      {
        new GlobalDeviceId()
        {
          Serialnumber = smokeDetectorVersion.Serialnumber.ToString(),
          DeviceTypeName = "SmokeDetector",
          Manufacturer = smokeDetectorVersion.Manufacturer,
          FirmwareVersion = smokeDetectorVersion.VersionString,
          MeterType = ValueIdent.ValueIdPart_MeterType.SmokeDetector
        }
      };
    }

    public override bool SelectDevice(GlobalDeviceId device)
    {
      this.GarantHandlerLoaded();
      return true;
    }

    public override bool Read(StructureTreeNode structureTreeNode, List<long> filter)
    {
      if (structureTreeNode == null)
        throw new NullReferenceException(nameof (structureTreeNode));
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      this.GarantHandlerLoaded();
      GMM_EventArgs eventMessage = new GMM_EventArgs("");
      try
      {
        eventMessage.EventMessage = "Read device";
        this.MyDeviceManager.RaiseEvent(eventMessage);
        return !eventMessage.Cancel && this.smokeDetector != null && this.smokeDetector.ReadDevice(ReadPart.LoggerEvents);
      }
      catch (Exception ex)
      {
        eventMessage.EventMessage = "Can not read (SN: " + structureTreeNode.SerialNumber + ") Reason: " + ex.Message;
        this.MyDeviceManager.RaiseEvent(eventMessage);
      }
      return false;
    }

    public override bool ReadAll(List<long> filter)
    {
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      this.GarantHandlerLoaded();
      if (this.smokeDetector == null)
        return false;
      try
      {
        this.smokeDetector.ValueIdentSetReceived += new EventHandler<ValueIdentSet>(this.SmokeDetector_ValueIdentSetReceived);
        return this.smokeDetector.ReadDevice(true);
      }
      finally
      {
        this.smokeDetector.ValueIdentSetReceived -= new EventHandler<ValueIdentSet>(this.SmokeDetector_ValueIdentSetReceived);
      }
    }

    public override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      string serialnumber)
    {
      return this.GetValues(ref ValueList);
    }

    public override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      if (this.smokeDetector == null)
        return false;
      List<long> filter = (List<long>) null;
      if (ValueList == null)
        ValueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      else if (ValueList.Count > 0)
      {
        filter = new List<long>();
        filter.AddRange((IEnumerable<long>) ValueList.Keys);
      }
      ValueList = this.smokeDetector.GetValues(filter);
      return ValueList != null;
    }

    public override bool ReadConfigurationParameters(out GlobalDeviceId UpdatedDeviceIdentification)
    {
      UpdatedDeviceIdentification = (GlobalDeviceId) null;
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      this.GarantHandlerLoaded();
      if (this.smokeDetector == null || !this.smokeDetector.ReadDevice(ReadPart.ManufacturingParameter | ReadPart.LoRa))
        return false;
      List<GlobalDeviceId> globalDeviceIdList = this.GetGlobalDeviceIdList();
      if (globalDeviceIdList == null && globalDeviceIdList.Count != 1)
        return false;
      UpdatedDeviceIdentification = globalDeviceIdList[0];
      return true;
    }

    public override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      if (this.smokeDetector == null)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      try
      {
        return this.smokeDetector.GetConfigurationParameters(0);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Parameter generation error " + ex.Message);
        return new SortedList<OverrideID, ConfigurationParameter>();
      }
    }

    private static void AddParam(
      bool canChanged,
      SortedList<OverrideID, ConfigurationParameter> r,
      OverrideID overrideID,
      object obj)
    {
      Devices.SmokeDetectorHandler.AddParam(canChanged, r, overrideID, obj, false, (string[]) null);
    }

    private static void AddParam(
      bool canChanged,
      SortedList<OverrideID, ConfigurationParameter> r,
      OverrideID overrideID,
      object obj,
      bool isFunction,
      string[] allowedValues)
    {
      if (!UserManager.IsConfigParamVisible(overrideID))
        return;
      bool flag = false;
      if (canChanged)
        flag = UserManager.IsConfigParamEditable(overrideID);
      r.Add(overrideID, new ConfigurationParameter(overrideID, obj)
      {
        HasWritePermission = flag,
        AllowedValues = allowedValues
      });
    }

    public override bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameterList,
      int SubDevice)
    {
      if (SubDevice != 0)
        return false;
      if (this.changedParameters == null)
        this.changedParameters = new SortedList<OverrideID, ConfigurationParameter>();
      if (parameterList == null || parameterList.Count <= 0)
        return false;
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> parameter in parameterList)
      {
        if (parameter.Key == OverrideID.Activation)
          this.smokeDetector.WorkMeter.LoRaParameter.Activation = (OTAA_ABP) Enum.Parse(typeof (OTAA_ABP), parameter.Value.ParameterValue.ToString());
        if (parameter.Key == OverrideID.TransmissionScenario)
          this.smokeDetector.WorkMeter.LoRaParameter.TransmissionScenario = byte.Parse(parameter.Value.ParameterValue.ToString());
        if (parameter.Key == OverrideID.CommunicationScenario)
          this.smokeDetector.WorkMeter.LoRaParameter.CommunicationScenario = new int?(int.Parse(parameter.Value.GetStringValueWin()));
        if (this.changedParameters.ContainsKey(parameter.Key))
          this.changedParameters[parameter.Key] = parameter.Value;
        else
          this.changedParameters.Add(parameter.Key, parameter.Value);
      }
      return true;
    }

    public override bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameterList)
    {
      return this.SetConfigurationParameters(parameterList, 0);
    }

    public override bool WriteChangedConfigurationParametersToDevice()
    {
      if (this.changedParameters == null)
        return false;
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      try
      {
        foreach (KeyValuePair<OverrideID, ConfigurationParameter> changedParameter in this.changedParameters)
        {
          if (changedParameter.Value.HasWritePermission)
          {
            switch (changedParameter.Key)
            {
              case OverrideID.DeviceClock:
                DateTime result;
                if (changedParameter.Value != null && changedParameter.Value.ParameterValue != null && DateTime.TryParse(changedParameter.Value.ParameterValue.ToString(), out result))
                {
                  this.smokeDetector.WorkMeter.Parameter.CurrentDateTime = new DateTime?(result);
                  break;
                }
                continue;
              case OverrideID.SendJoinRequest:
                try
                {
                  AsyncHelpers.RunSync((Func<Task>) (async () => await this.smokeDetector.LoRa.SendJoinRequestAsync((ProgressHandler) null, CancellationToken.None)));
                  break;
                }
                catch (Exception ex)
                {
                  if (ex is AggregateException && ex.InnerException != null)
                    throw ex.InnerException;
                  break;
                }
            }
          }
        }
        if (!this.smokeDetector.WriteDevice())
          return false;
        bool flag = true;
        foreach (KeyValuePair<OverrideID, ConfigurationParameter> changedParameter in this.changedParameters)
        {
          if (changedParameter.Value.HasWritePermission)
          {
            if (changedParameter.Key == OverrideID.SetToDelivery)
              flag = this.smokeDetector.TC_SetDeliveryState();
            if (!flag)
              return false;
          }
        }
        this.changedParameters.Clear();
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        return false;
      }
    }

    public override bool BeginSearchDevices()
    {
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      return this.ReadAll((List<long>) null);
    }

    private void SmokeDetector_ValueIdentSetReceived(object sender, ValueIdentSet e)
    {
      if (!this.MyDeviceManager.IsValueIdentSetReceivedEventEnabled)
        return;
      this.MyDeviceManager.OnValueIdentSetReceived(sender, e);
    }
  }
}
