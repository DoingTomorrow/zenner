// Decompiled with JetBrains decompiler
// Type: Devices.PdcHandler
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using DeviceCollector;
using NLog;
using PDC_Handler;
using StartupLib;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace Devices
{
  public class PdcHandler(DeviceManager MyDeviceManager) : BaseDevice(MyDeviceManager)
  {
    private SortedList<OverrideID, ConfigurationParameter> prmsMain;
    private SortedList<OverrideID, ConfigurationParameter> prmsInputA;
    private SortedList<OverrideID, ConfigurationParameter> prmsInputB;
    private static Logger logger = LogManager.GetLogger(nameof (PdcHandler));
    private PDC_HandlerFunctions pdc;

    public override event EventHandlerEx<int> OnProgress;

    public override object GetHandler()
    {
      this.GarantHandlerLoaded();
      return (object) this.pdc;
    }

    private void GarantHandlerLoaded()
    {
      if (this.pdc != null)
        return;
      if (ZR_Component.CommonGmmInterface != null)
      {
        ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.PDC_Handler);
        this.pdc = (PDC_HandlerFunctions) ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.PDC_Handler];
      }
      else
        this.pdc = new PDC_HandlerFunctions((IDeviceCollector) this.MyDeviceManager.MyBus);
      this.pdc.OnProgress += new ValueEventHandler<int>(this.PDC_OnProgress);
    }

    private void PDC_OnProgress(object sender, int e)
    {
      if (this.OnProgress == null)
        return;
      this.OnProgress(sender, e);
    }

    internal override void ShowHandlerWindow()
    {
      this.GarantHandlerLoaded();
      this.pdc.ShowHandlerWindow();
    }

    public override void Dispose()
    {
      if (this.pdc == null)
        return;
      this.pdc.OnProgress -= new ValueEventHandler<int>(this.PDC_OnProgress);
      this.pdc.GMM_Dispose();
      this.pdc = (PDC_HandlerFunctions) null;
    }

    public override List<GlobalDeviceId> GetGlobalDeviceIdList()
    {
      this.GarantHandlerLoaded();
      if (this.pdc.Meter == null || this.pdc.Meter.Version == null)
        return (List<GlobalDeviceId>) null;
      List<GlobalDeviceId> globalDeviceIdList = new List<GlobalDeviceId>();
      GlobalDeviceId globalDeviceId1 = new GlobalDeviceId();
      GlobalDeviceId globalDeviceId2 = globalDeviceId1;
      uint? nullable = this.pdc.Meter.GetSerialMBusPDC();
      string str1 = nullable.ToString();
      globalDeviceId2.Serialnumber = str1;
      globalDeviceId1.DeviceTypeName = this.pdc.Meter.Version.Type.ToString();
      globalDeviceId1.Manufacturer = this.pdc.Meter.GetManufacturerPDC();
      globalDeviceId1.FirmwareVersion = this.pdc.Meter.Version.ToString();
      globalDeviceId1.MeterType = ValueIdent.ValueIdPart_MeterType.PulseCounter;
      globalDeviceId1.Generation = this.pdc.Meter.GetMBusGenerationPDC().ToString();
      globalDeviceId1.MeterNumber = this.pdc.Meter.GetSerialnumberFull();
      GlobalDeviceId globalDeviceId3 = new GlobalDeviceId();
      GlobalDeviceId globalDeviceId4 = globalDeviceId3;
      nullable = this.pdc.Meter.GetSerialMBusInputA();
      string str2 = nullable.ToString();
      globalDeviceId4.Serialnumber = str2;
      globalDeviceId3.DeviceTypeName = this.pdc.Meter.GetMediumInputA().ToString();
      globalDeviceId3.Manufacturer = this.pdc.Meter.GetManufacturerInputA();
      globalDeviceId3.MeterType = ValueIdent.ConvertToMeterType(this.pdc.Meter.GetMediumInputA().Value);
      globalDeviceId3.Address = (int) this.pdc.Meter.GetMBusAddressInputA().Value;
      globalDeviceId3.DeviceDetails = "Input A";
      globalDeviceId3.FirmwareVersion = this.pdc.Meter.Version.ToString();
      globalDeviceId3.Generation = this.pdc.Meter.GetMBusGenerationInputA().ToString();
      globalDeviceId3.MeterNumber = this.pdc.Meter.GetSerialnumberFullInputA();
      globalDeviceId1.SubDevices.Add(globalDeviceId3);
      GlobalDeviceId globalDeviceId5 = new GlobalDeviceId();
      GlobalDeviceId globalDeviceId6 = globalDeviceId5;
      nullable = this.pdc.Meter.GetSerialMBusInputB();
      string str3 = nullable.ToString();
      globalDeviceId6.Serialnumber = str3;
      globalDeviceId5.DeviceTypeName = this.pdc.Meter.GetMediumInputB().ToString();
      globalDeviceId5.Manufacturer = this.pdc.Meter.GetManufacturerInputB();
      globalDeviceId5.MeterType = ValueIdent.ConvertToMeterType(this.pdc.Meter.GetMediumInputB().Value);
      globalDeviceId5.Address = (int) this.pdc.Meter.GetMBusAddressInputB().Value;
      globalDeviceId5.DeviceDetails = "Input B";
      globalDeviceId5.FirmwareVersion = this.pdc.Meter.Version.ToString();
      globalDeviceId5.Generation = this.pdc.Meter.GetMBusGenerationInputB().ToString();
      globalDeviceId5.MeterNumber = this.pdc.Meter.GetSerialnumberFullInputB();
      globalDeviceId1.SubDevices.Add(globalDeviceId5);
      globalDeviceIdList.Add(globalDeviceId1);
      return globalDeviceIdList;
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
        return !eventMessage.Cancel && this.pdc.ReadDevice();
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
      bool flag = this.pdc.ReadDevice(true);
      if (flag)
        this.FireEventOnValueIdentSetReceived();
      return flag;
    }

    public override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      int SubDevice)
    {
      if (this.pdc.Meter == null || this.pdc.Meter.Version == null)
        return false;
      List<long> filter = (List<long>) null;
      if (ValueList == null)
        ValueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      else if (ValueList.Count > 0)
      {
        filter = new List<long>();
        filter.AddRange((IEnumerable<long>) ValueList.Keys);
      }
      ValueList = this.pdc.GetValues(SubDevice, filter);
      ValueIdent.CleanUpEmptyValueIdents(ValueList);
      return true;
    }

    public override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      string serialnumber)
    {
      if (this.pdc.Meter == null || this.pdc.Meter.Version == null)
        return false;
      uint? serialMbusInputA = this.pdc.Meter.GetSerialMBusInputA();
      if (serialMbusInputA.HasValue && serialMbusInputA.Value.ToString() == serialnumber)
        return this.GetValues(ref ValueList, 1);
      uint? serialMbusInputB = this.pdc.Meter.GetSerialMBusInputB();
      return serialMbusInputB.HasValue && serialMbusInputB.Value.ToString() == serialnumber && this.GetValues(ref ValueList, 2);
    }

    public override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      return true;
    }

    protected void AddValue(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList,
      DateTime timePoint,
      long valueIdent,
      object obj)
    {
      ReadingValue readingValue = new ReadingValue();
      readingValue.value = Util.ToDouble(obj);
      readingValue.state = ReadingValueState.ok;
      if (valueList.ContainsKey(valueIdent))
      {
        if (valueList[valueIdent].ContainsKey(timePoint))
          return;
        valueList[valueIdent].Add(timePoint, readingValue);
      }
      else
        valueList.Add(valueIdent, new SortedList<DateTime, ReadingValue>()
        {
          {
            timePoint,
            readingValue
          }
        });
    }

    protected void AddErrorValue(
      SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      ValueIdent.ValueIdPart_MeterType meterType,
      ValueIdent.ValueIdentError error,
      DateTime? timepoint)
    {
      long valueIdentOfError = ValueIdent.GetValueIdentOfError(meterType, error);
      ReadingValue readingValue = new ReadingValue();
      readingValue.value = 1.0;
      SortedList<DateTime, ReadingValue> sortedList = new SortedList<DateTime, ReadingValue>();
      if (timepoint.HasValue)
      {
        sortedList.Add(timepoint.Value, readingValue);
      }
      else
      {
        DateTime now = DateTime.Now;
        sortedList.Add(new DateTime(now.Year, now.Month, now.Day), readingValue);
      }
      ValueList.Add(valueIdentOfError, sortedList);
    }

    public override bool ReadConfigurationParameters(out GlobalDeviceId UpdatedDeviceIdentification)
    {
      UpdatedDeviceIdentification = (GlobalDeviceId) null;
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      this.GarantHandlerLoaded();
      this.prmsMain = (SortedList<OverrideID, ConfigurationParameter>) null;
      this.prmsInputA = (SortedList<OverrideID, ConfigurationParameter>) null;
      this.prmsInputB = (SortedList<OverrideID, ConfigurationParameter>) null;
      if (!this.pdc.ReadDevice())
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
      if (this.pdc == null)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      switch (SubDevice)
      {
        case 0:
          SortedList<OverrideID, ConfigurationParameter> configurationParametersPdc = this.GetConfigurationParametersPDC();
          if (configurationParametersPdc != null && this.prmsMain != null)
            this.Merge(configurationParametersPdc, this.prmsMain);
          return configurationParametersPdc;
        case 1:
          SortedList<OverrideID, ConfigurationParameter> parametersInputA = this.GetConfigurationParametersInputA();
          if (parametersInputA != null && this.prmsInputA != null)
            this.Merge(parametersInputA, this.prmsInputA);
          return parametersInputA;
        case 2:
          SortedList<OverrideID, ConfigurationParameter> parametersInputB = this.GetConfigurationParametersInputB();
          if (parametersInputB != null && this.prmsInputB != null)
            this.Merge(parametersInputB, this.prmsInputB);
          return parametersInputB;
        default:
          throw new ArgumentOutOfRangeException(nameof (SubDevice));
      }
    }

    private void Merge(
      SortedList<OverrideID, ConfigurationParameter> list,
      SortedList<OverrideID, ConfigurationParameter> newPrms)
    {
      if (newPrms == null)
        return;
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> newPrm in newPrms)
      {
        if (list.IndexOfKey(newPrm.Key) >= 0)
          list[newPrm.Key] = newPrm.Value;
      }
    }

    private static void AddParam(
      bool canChanged,
      SortedList<OverrideID, ConfigurationParameter> r,
      OverrideID overrideID,
      object obj)
    {
      PdcHandler.AddParam(canChanged, r, overrideID, obj, false, (string[]) null);
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

    private SortedList<OverrideID, ConfigurationParameter> GetConfigurationParametersInputB()
    {
      SortedList<OverrideID, ConfigurationParameter> r = new SortedList<OverrideID, ConfigurationParameter>();
      PDC_Meter meter = this.pdc.Meter;
      if (meter == null || meter.Version == null)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      if (UserManager.IsNewLicenseModel())
      {
        MBusDeviceType? mediumInputB = meter.GetMediumInputB();
        PdcHandler.AddParam(true, r, OverrideID.SerialNumber, (object) meter.GetSerialMBusInputB());
        PdcHandler.AddParam(false, r, OverrideID.SerialNumberFull, (object) meter.GetSerialnumberFullInputB());
        PdcHandler.AddParam(true, r, OverrideID.Manufacturer, (object) meter.GetManufacturerInputB());
        PdcHandler.AddParam(true, r, OverrideID.MBusGeneration, (object) meter.GetMBusGenerationInputB());
        PdcHandler.AddParam(true, r, OverrideID.Medium, (object) mediumInputB);
        PdcHandler.AddParam(false, r, OverrideID.WarningInfo, (object) meter.GetWarningsInputB());
        PdcHandler.AddParam(true, r, OverrideID.TotalPulse, (object) meter.GetMeterValueB());
        PdcHandler.AddParam(true, r, OverrideID.PulseBlockLimit, (object) meter.GetPulseBlockLimitInputB());
        PdcHandler.AddParam(true, r, OverrideID.PulseLeakLimit, (object) meter.GetPulseLeakLimitInputB());
        PdcHandler.AddParam(true, r, OverrideID.PulseUnleakLimit, (object) meter.GetPulseUnleakLimitInputB());
        PdcHandler.AddParam(true, r, OverrideID.PulseLeakLower, (object) meter.GetPulseLeakLowerInputB());
        PdcHandler.AddParam(true, r, OverrideID.PulseLeakUpper, (object) meter.GetPulseLeakUpperInputB());
        PdcHandler.AddParam(true, r, OverrideID.OversizeDiff, (object) meter.GetOversizeDiffInputB());
        PdcHandler.AddParam(true, r, OverrideID.OversizeLimit, (object) meter.GetOversizeLimitInputB());
        PdcHandler.AddParam(true, r, OverrideID.UndersizeDiff, (object) meter.GetUndersizeDiffInputB());
        PdcHandler.AddParam(true, r, OverrideID.UndersizeLimit, (object) meter.GetUndersizeLimitInputB());
        PdcHandler.AddParam(true, r, OverrideID.BurstDiff, (object) meter.GetBurstDiffInputB());
        PdcHandler.AddParam(true, r, OverrideID.BurstLimit, (object) meter.GetBurstLimitInputB());
        PdcHandler.AddParam(true, r, OverrideID.InputPulsValue, (object) meter.GetScaleFactorInputB());
        PdcHandler.AddParam(true, r, OverrideID.ClearWarnings, (object) false, true, (string[]) null);
        PdcHandler.AddParam(true, r, OverrideID.RadioEnabled, (object) ((meter.GetRadioFlagsPDCwMBus().Value & RadioFlagsPDCwMBus.CONFIG_RADIO_CHANNEL_B) != 0));
        if (mediumInputB.HasValue && (mediumInputB.Value == MBusDeviceType.COLD_WATER || mediumInputB.Value == MBusDeviceType.HOT_AND_COLD_WATER || mediumInputB.Value == MBusDeviceType.HOT_WATER || mediumInputB.Value == MBusDeviceType.HOT_WATER_90 || mediumInputB.Value == MBusDeviceType.WATER))
          r.Add(OverrideID.NominalFlow, new ConfigurationParameter(OverrideID.NominalFlow, (object) meter.GetNominalFlowB())
          {
            HasWritePermission = true,
            AllowedValues = meter.GetNominalFlowAllowedValuesB()
          });
        ResolutionData resolutionData = MeterUnits.GetResolutionData(meter.GetVIFInputB().Value);
        PdcHandler.AddParam(true, r, OverrideID.InputResolutionStr, (object) resolutionData.resolutionString, false, InputResolution.Values);
      }
      else
      {
        bool flag1 = false;
        if (UserRights.GlobalUserRights.PackageName == "ConfigurationManagerPro")
          flag1 = true;
        else if (UserRights.GlobalUserRights.PackageName == "ConfigurationManager" && UserRights.GlobalUserRights.OptionPackageName == "Professional")
          flag1 = true;
        bool flag2 = UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Developer);
        MBusDeviceType? mediumInputB = meter.GetMediumInputB();
        r.Add(OverrideID.SerialNumber, new ConfigurationParameter(OverrideID.SerialNumber, (object) meter.GetSerialMBusInputB())
        {
          HasWritePermission = true
        });
        r.Add(OverrideID.SerialNumberFull, new ConfigurationParameter(OverrideID.SerialNumberFull, (object) meter.GetSerialnumberFullInputB()));
        r.Add(OverrideID.Manufacturer, new ConfigurationParameter(OverrideID.Manufacturer, (object) meter.GetManufacturerInputB())
        {
          HasWritePermission = true
        });
        r.Add(OverrideID.MBusGeneration, new ConfigurationParameter(OverrideID.MBusGeneration, (object) meter.GetMBusGenerationInputB())
        {
          HasWritePermission = true
        });
        r.Add(OverrideID.Medium, new ConfigurationParameter(OverrideID.Medium, (object) mediumInputB)
        {
          HasWritePermission = true
        });
        r.Add(OverrideID.WarningInfo, new ConfigurationParameter(OverrideID.WarningInfo, (object) meter.GetWarningsInputB()));
        r.Add(OverrideID.ClearWarnings, new ConfigurationParameter(OverrideID.ClearWarnings)
        {
          HasWritePermission = true,
          ParameterValue = (object) false
        });
        bool ParameterValue = (meter.GetRadioFlagsPDCwMBus().Value & RadioFlagsPDCwMBus.CONFIG_RADIO_CHANNEL_B) != 0;
        r.Add(OverrideID.RadioEnabled, new ConfigurationParameter(OverrideID.RadioEnabled, (object) ParameterValue)
        {
          HasWritePermission = true
        });
        r.Add(OverrideID.TotalPulse, new ConfigurationParameter(OverrideID.TotalPulse, (object) meter.GetMeterValueB())
        {
          HasWritePermission = true
        });
        ResolutionData resolutionData = MeterUnits.GetResolutionData(meter.GetVIFInputB().Value);
        r.Add(OverrideID.InputResolutionStr, new ConfigurationParameter(OverrideID.InputResolutionStr, (object) resolutionData.resolutionString)
        {
          HasWritePermission = true,
          AllowedValues = InputResolution.Values
        });
        if (flag1 | flag2)
        {
          if (mediumInputB.HasValue && (mediumInputB.Value == MBusDeviceType.COLD_WATER || mediumInputB.Value == MBusDeviceType.HOT_AND_COLD_WATER || mediumInputB.Value == MBusDeviceType.HOT_WATER || mediumInputB.Value == MBusDeviceType.HOT_WATER_90 || mediumInputB.Value == MBusDeviceType.WATER))
            r.Add(OverrideID.NominalFlow, new ConfigurationParameter(OverrideID.NominalFlow, (object) meter.GetNominalFlowB())
            {
              HasWritePermission = true,
              AllowedValues = meter.GetNominalFlowAllowedValuesB()
            });
          r.Add(OverrideID.PulseBlockLimit, new ConfigurationParameter(OverrideID.PulseBlockLimit, (object) meter.GetPulseBlockLimitInputB())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.PulseLeakLimit, new ConfigurationParameter(OverrideID.PulseLeakLimit, (object) meter.GetPulseLeakLimitInputB())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.PulseUnleakLimit, new ConfigurationParameter(OverrideID.PulseUnleakLimit, (object) meter.GetPulseUnleakLimitInputB())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.PulseLeakLower, new ConfigurationParameter(OverrideID.PulseLeakLower, (object) meter.GetPulseLeakLowerInputB())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.PulseLeakUpper, new ConfigurationParameter(OverrideID.PulseLeakUpper, (object) meter.GetPulseLeakUpperInputB())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.OversizeDiff, new ConfigurationParameter(OverrideID.OversizeDiff, (object) meter.GetOversizeDiffInputB())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.OversizeLimit, new ConfigurationParameter(OverrideID.OversizeLimit, (object) meter.GetOversizeLimitInputB())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.UndersizeDiff, new ConfigurationParameter(OverrideID.UndersizeDiff, (object) meter.GetUndersizeDiffInputB())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.UndersizeLimit, new ConfigurationParameter(OverrideID.UndersizeLimit, (object) meter.GetUndersizeLimitInputB())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.BurstDiff, new ConfigurationParameter(OverrideID.BurstDiff, (object) meter.GetBurstDiffInputB())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.BurstLimit, new ConfigurationParameter(OverrideID.BurstLimit, (object) meter.GetBurstLimitInputB())
          {
            HasWritePermission = true
          });
        }
        r.Add(OverrideID.InputPulsValue, new ConfigurationParameter(OverrideID.InputPulsValue, (object) meter.GetScaleFactorInputB())
        {
          HasWritePermission = true
        });
      }
      return r;
    }

    private SortedList<OverrideID, ConfigurationParameter> GetConfigurationParametersInputA()
    {
      SortedList<OverrideID, ConfigurationParameter> r = new SortedList<OverrideID, ConfigurationParameter>();
      PDC_Meter meter = this.pdc.Meter;
      if (meter == null || meter.Version == null)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      if (UserManager.IsNewLicenseModel())
      {
        MBusDeviceType? mediumInputA = meter.GetMediumInputA();
        PdcHandler.AddParam(true, r, OverrideID.SerialNumber, (object) meter.GetSerialMBusInputA());
        PdcHandler.AddParam(false, r, OverrideID.SerialNumberFull, (object) meter.GetSerialnumberFullInputA());
        PdcHandler.AddParam(true, r, OverrideID.Manufacturer, (object) meter.GetManufacturerInputA());
        PdcHandler.AddParam(true, r, OverrideID.MBusGeneration, (object) meter.GetMBusGenerationInputA());
        PdcHandler.AddParam(true, r, OverrideID.Medium, (object) mediumInputA);
        PdcHandler.AddParam(false, r, OverrideID.WarningInfo, (object) meter.GetWarningsInputA());
        PdcHandler.AddParam(true, r, OverrideID.TotalPulse, (object) meter.GetMeterValueA());
        PdcHandler.AddParam(true, r, OverrideID.PulseBlockLimit, (object) meter.GetPulseBlockLimitInputA());
        PdcHandler.AddParam(true, r, OverrideID.PulseLeakLimit, (object) meter.GetPulseLeakLimitInputA());
        PdcHandler.AddParam(true, r, OverrideID.PulseUnleakLimit, (object) meter.GetPulseUnleakLimitInputA());
        PdcHandler.AddParam(true, r, OverrideID.PulseLeakLower, (object) meter.GetPulseLeakLowerInputA());
        PdcHandler.AddParam(true, r, OverrideID.PulseLeakUpper, (object) meter.GetPulseLeakUpperInputA());
        PdcHandler.AddParam(true, r, OverrideID.OversizeDiff, (object) meter.GetOversizeDiffInputA());
        PdcHandler.AddParam(true, r, OverrideID.OversizeLimit, (object) meter.GetOversizeLimitInputA());
        PdcHandler.AddParam(true, r, OverrideID.UndersizeDiff, (object) meter.GetUndersizeDiffInputA());
        PdcHandler.AddParam(true, r, OverrideID.UndersizeLimit, (object) meter.GetUndersizeLimitInputA());
        PdcHandler.AddParam(true, r, OverrideID.BurstDiff, (object) meter.GetBurstDiffInputA());
        PdcHandler.AddParam(true, r, OverrideID.BurstLimit, (object) meter.GetBurstLimitInputA());
        PdcHandler.AddParam(true, r, OverrideID.InputPulsValue, (object) meter.GetScaleFactorInputA());
        PdcHandler.AddParam(true, r, OverrideID.ClearWarnings, (object) false, true, (string[]) null);
        PdcHandler.AddParam(true, r, OverrideID.RadioEnabled, (object) ((meter.GetRadioFlagsPDCwMBus().Value & RadioFlagsPDCwMBus.CONFIG_RADIO_CHANNEL_A) != 0));
        if (mediumInputA.HasValue && (mediumInputA.Value == MBusDeviceType.COLD_WATER || mediumInputA.Value == MBusDeviceType.HOT_AND_COLD_WATER || mediumInputA.Value == MBusDeviceType.HOT_WATER || mediumInputA.Value == MBusDeviceType.HOT_WATER_90 || mediumInputA.Value == MBusDeviceType.WATER))
          r.Add(OverrideID.NominalFlow, new ConfigurationParameter(OverrideID.NominalFlow, (object) meter.GetNominalFlowA())
          {
            HasWritePermission = true,
            AllowedValues = meter.GetNominalFlowAllowedValuesA()
          });
        ResolutionData resolutionData = MeterUnits.GetResolutionData(meter.GetVIFInputA().Value);
        PdcHandler.AddParam(true, r, OverrideID.InputResolutionStr, (object) resolutionData.resolutionString, false, InputResolution.Values);
      }
      else
      {
        bool flag1 = false;
        if (UserRights.GlobalUserRights.PackageName == "ConfigurationManagerPro")
          flag1 = true;
        else if (UserRights.GlobalUserRights.PackageName == "ConfigurationManager" && UserRights.GlobalUserRights.OptionPackageName == "Professional")
          flag1 = true;
        bool flag2 = UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Developer);
        MBusDeviceType? mediumInputA = meter.GetMediumInputA();
        r.Add(OverrideID.SerialNumber, new ConfigurationParameter(OverrideID.SerialNumber, (object) meter.GetSerialMBusInputA())
        {
          HasWritePermission = true
        });
        r.Add(OverrideID.SerialNumberFull, new ConfigurationParameter(OverrideID.SerialNumberFull, (object) meter.GetSerialnumberFullInputA()));
        r.Add(OverrideID.Manufacturer, new ConfigurationParameter(OverrideID.Manufacturer, (object) meter.GetManufacturerInputA())
        {
          HasWritePermission = true
        });
        r.Add(OverrideID.MBusGeneration, new ConfigurationParameter(OverrideID.MBusGeneration, (object) meter.GetMBusGenerationInputA())
        {
          HasWritePermission = true
        });
        r.Add(OverrideID.Medium, new ConfigurationParameter(OverrideID.Medium, (object) mediumInputA)
        {
          HasWritePermission = true
        });
        r.Add(OverrideID.WarningInfo, new ConfigurationParameter(OverrideID.WarningInfo, (object) meter.GetWarningsInputA()));
        r.Add(OverrideID.ClearWarnings, new ConfigurationParameter(OverrideID.ClearWarnings)
        {
          HasWritePermission = true,
          ParameterValue = (object) false
        });
        bool ParameterValue = (meter.GetRadioFlagsPDCwMBus().Value & RadioFlagsPDCwMBus.CONFIG_RADIO_CHANNEL_A) != 0;
        r.Add(OverrideID.RadioEnabled, new ConfigurationParameter(OverrideID.RadioEnabled, (object) ParameterValue)
        {
          HasWritePermission = true
        });
        r.Add(OverrideID.TotalPulse, new ConfigurationParameter(OverrideID.TotalPulse, (object) meter.GetMeterValueA())
        {
          HasWritePermission = true
        });
        ResolutionData resolutionData = MeterUnits.GetResolutionData(meter.GetVIFInputA().Value);
        r.Add(OverrideID.InputResolutionStr, new ConfigurationParameter(OverrideID.InputResolutionStr, (object) resolutionData.resolutionString)
        {
          HasWritePermission = true,
          AllowedValues = InputResolution.Values
        });
        if (flag1 | flag2)
        {
          if (mediumInputA.HasValue && (mediumInputA.Value == MBusDeviceType.COLD_WATER || mediumInputA.Value == MBusDeviceType.HOT_AND_COLD_WATER || mediumInputA.Value == MBusDeviceType.HOT_WATER || mediumInputA.Value == MBusDeviceType.HOT_WATER_90 || mediumInputA.Value == MBusDeviceType.WATER))
            r.Add(OverrideID.NominalFlow, new ConfigurationParameter(OverrideID.NominalFlow, (object) meter.GetNominalFlowA())
            {
              HasWritePermission = true,
              AllowedValues = meter.GetNominalFlowAllowedValuesA()
            });
          r.Add(OverrideID.PulseBlockLimit, new ConfigurationParameter(OverrideID.PulseBlockLimit, (object) meter.GetPulseBlockLimitInputA())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.PulseLeakLimit, new ConfigurationParameter(OverrideID.PulseLeakLimit, (object) meter.GetPulseLeakLimitInputA())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.PulseUnleakLimit, new ConfigurationParameter(OverrideID.PulseUnleakLimit, (object) meter.GetPulseUnleakLimitInputA())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.PulseLeakLower, new ConfigurationParameter(OverrideID.PulseLeakLower, (object) meter.GetPulseLeakLowerInputA())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.PulseLeakUpper, new ConfigurationParameter(OverrideID.PulseLeakUpper, (object) meter.GetPulseLeakUpperInputA())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.OversizeDiff, new ConfigurationParameter(OverrideID.OversizeDiff, (object) meter.GetOversizeDiffInputA())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.OversizeLimit, new ConfigurationParameter(OverrideID.OversizeLimit, (object) meter.GetOversizeLimitInputA())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.UndersizeDiff, new ConfigurationParameter(OverrideID.UndersizeDiff, (object) meter.GetUndersizeDiffInputA())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.UndersizeLimit, new ConfigurationParameter(OverrideID.UndersizeLimit, (object) meter.GetUndersizeLimitInputA())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.BurstDiff, new ConfigurationParameter(OverrideID.BurstDiff, (object) meter.GetBurstDiffInputA())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.BurstLimit, new ConfigurationParameter(OverrideID.BurstLimit, (object) meter.GetBurstLimitInputA())
          {
            HasWritePermission = true
          });
        }
        r.Add(OverrideID.InputPulsValue, new ConfigurationParameter(OverrideID.InputPulsValue, (object) meter.GetScaleFactorInputA())
        {
          HasWritePermission = true
        });
      }
      return r;
    }

    private SortedList<OverrideID, ConfigurationParameter> GetConfigurationParametersPDC()
    {
      SortedList<OverrideID, ConfigurationParameter> r1 = new SortedList<OverrideID, ConfigurationParameter>();
      PDC_Meter meter = this.pdc.Meter;
      if (meter == null || meter.Version == null)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      if (UserManager.IsNewLicenseModel())
      {
        PdcHandler.AddParam(false, r1, OverrideID.FirmwareVersion, (object) meter.Version.VersionString);
        PdcHandler.AddParam(false, r1, OverrideID.SerialNumber, (object) meter.GetSerialMBusPDC());
        PdcHandler.AddParam(false, r1, OverrideID.SerialNumberFull, (object) meter.GetSerialnumberFull());
        PdcHandler.AddParam(false, r1, OverrideID.Manufacturer, (object) meter.GetManufacturerPDC());
        PdcHandler.AddParam(false, r1, OverrideID.DeviceClock, (object) meter.GetSystemTime());
        PdcHandler.AddParam(true, r1, OverrideID.TimeZone, (object) (Decimal) meter.GetTimeZone().Value);
        PdcHandler.AddParam(true, r1, OverrideID.DueDate, (object) meter.GetDueDate());
        PdcHandler.AddParam(false, r1, OverrideID.DeviceHasError, (object) (meter.GetHardwareErrors().Value != 0));
        PdcHandler.AddParam(false, r1, OverrideID.MBusGeneration, (object) meter.GetMBusGenerationPDC());
        PdcHandler.AddParam(false, r1, OverrideID.Medium, (object) meter.GetMediumPDC());
        PdcHandler.AddParam(false, r1, OverrideID.EndOfBatteryDate, (object) meter.GetBatteryEndDate());
        PdcHandler.AddParam(false, r1, OverrideID.RadioMode, (object) meter.GetRadioMode());
        PdcHandler.AddParam(true, r1, OverrideID.RadioSendInterval, (object) meter.GetRadioTransmitInterval());
        PdcHandler.AddParam(true, r1, OverrideID.AESKey, (object) AES.AesKeyToString(meter.GetAESkey()));
        PdcHandler.AddParam(true, r1, OverrideID.ListType, (object) meter.GetRadioListType());
        PdcHandler.AddParam(true, r1, OverrideID.RadioEnabled, (object) ((meter.GetConfigFlagsPDCwMBus().Value & ConfigFlagsPDCwMBus.CONFIG_ENABLE_RADIO) != 0));
        PdcHandler.AddParam(true, r1, OverrideID.PulseEnabled, (object) ((meter.GetConfigFlagsPDCwMBus().Value & ConfigFlagsPDCwMBus.CONFIG_ENABLE_PULSE) != 0));
        SortedList<OverrideID, ConfigurationParameter> r2 = r1;
        RadioFlagsPDCwMBus? radioFlagsPdCwMbus1 = meter.GetRadioFlagsPDCwMBus();
        RadioFlagsPDCwMBus? nullable = radioFlagsPdCwMbus1.HasValue ? new RadioFlagsPDCwMBus?(radioFlagsPdCwMbus1.GetValueOrDefault() & RadioFlagsPDCwMBus.CONFIG_RADIO_ENCRYPT) : new RadioFlagsPDCwMBus?();
        RadioFlagsPDCwMBus radioFlagsPdCwMbus2 = (RadioFlagsPDCwMBus) 0;
        // ISSUE: variable of a boxed type
        __Boxed<bool> local = (System.ValueType) !(nullable.GetValueOrDefault() == radioFlagsPdCwMbus2 & nullable.HasValue);
        PdcHandler.AddParam(true, r2, OverrideID.Encryption, (object) local);
        PdcHandler.AddParam(true, r1, OverrideID.LongHeader, (object) ((meter.GetRadioFlagsPDCwMBus().Value & RadioFlagsPDCwMBus.CONFIG_RADIO_LONGHEADER) != 0));
        PdcHandler.AddParam(true, r1, OverrideID.SetPcTime, (object) false, true, (string[]) null);
        DeviceIdentification deviceIdentification = meter.GetDeviceIdentification();
        if (deviceIdentification != null)
          PdcHandler.AddParam(false, r1, OverrideID.MeterID, (object) deviceIdentification.MeterID);
      }
      else
      {
        bool flag1 = false;
        if (UserRights.GlobalUserRights.PackageName == "ConfigurationManagerPro")
          flag1 = true;
        else if (UserRights.GlobalUserRights.PackageName == "ConfigurationManager" && UserRights.GlobalUserRights.OptionPackageName == "Professional")
          flag1 = true;
        bool flag2 = UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Developer);
        r1.Add(OverrideID.FirmwareVersion, new ConfigurationParameter(OverrideID.FirmwareVersion, (object) meter.Version.VersionString));
        r1.Add(OverrideID.SerialNumber, new ConfigurationParameter(OverrideID.SerialNumber, (object) meter.GetSerialMBusPDC()));
        r1.Add(OverrideID.SerialNumberFull, new ConfigurationParameter(OverrideID.SerialNumberFull, (object) meter.GetSerialnumberFull()));
        r1.Add(OverrideID.Manufacturer, new ConfigurationParameter(OverrideID.Manufacturer, (object) meter.GetManufacturerPDC()));
        r1.Add(OverrideID.DeviceClock, new ConfigurationParameter(OverrideID.DeviceClock, (object) meter.GetSystemTime()));
        r1.Add(OverrideID.TimeZone, new ConfigurationParameter(OverrideID.TimeZone, (object) (Decimal) meter.GetTimeZone().Value)
        {
          HasWritePermission = true
        });
        r1.Add(OverrideID.DueDate, new ConfigurationParameter(OverrideID.DueDate, (object) meter.GetDueDate())
        {
          HasWritePermission = true
        });
        r1.Add(OverrideID.DeviceHasError, new ConfigurationParameter(OverrideID.DeviceHasError, (object) (meter.GetHardwareErrors().Value != 0)));
        r1.Add(OverrideID.MBusGeneration, new ConfigurationParameter(OverrideID.MBusGeneration, (object) meter.GetMBusGenerationPDC()));
        r1.Add(OverrideID.ClearAllLoggers, new ConfigurationParameter(OverrideID.ClearAllLoggers)
        {
          HasWritePermission = true,
          ParameterValue = (object) false
        });
        r1.Add(OverrideID.SetPcTime, new ConfigurationParameter(OverrideID.SetPcTime)
        {
          HasWritePermission = true,
          ParameterValue = (object) false
        });
        r1.Add(OverrideID.Medium, new ConfigurationParameter(OverrideID.Medium, (object) meter.GetMediumPDC()));
        r1.Add(OverrideID.EndOfBattery, new ConfigurationParameter(OverrideID.EndOfBattery, (object) meter.GetBatteryEndDate()));
        DeviceIdentification deviceIdentification = meter.GetDeviceIdentification();
        if (deviceIdentification != null)
          r1.Add(OverrideID.MeterID, new ConfigurationParameter(OverrideID.MeterID, (object) deviceIdentification.MeterID));
        r1.Add(OverrideID.RadioMode, new ConfigurationParameter(OverrideID.RadioMode, (object) meter.GetRadioMode()));
        r1.Add(OverrideID.RadioSendInterval, new ConfigurationParameter(OverrideID.RadioSendInterval, (object) meter.GetRadioTransmitInterval()));
        r1.Add(OverrideID.AESKey, new ConfigurationParameter(OverrideID.AESKey, (object) meter.GetAESkey())
        {
          HasWritePermission = flag1 | flag2
        });
        bool ParameterValue1 = (meter.GetRadioFlagsPDCwMBus().Value & RadioFlagsPDCwMBus.CONFIG_RADIO_LONGHEADER) != 0;
        r1.Add(OverrideID.LongHeader, new ConfigurationParameter(OverrideID.LongHeader, (object) ParameterValue1)
        {
          HasWritePermission = flag1 | flag2
        });
        RadioFlagsPDCwMBus? radioFlagsPdCwMbus3 = meter.GetRadioFlagsPDCwMBus();
        RadioFlagsPDCwMBus? nullable = radioFlagsPdCwMbus3.HasValue ? new RadioFlagsPDCwMBus?(radioFlagsPdCwMbus3.GetValueOrDefault() & RadioFlagsPDCwMBus.CONFIG_RADIO_ENCRYPT) : new RadioFlagsPDCwMBus?();
        RadioFlagsPDCwMBus radioFlagsPdCwMbus4 = (RadioFlagsPDCwMBus) 0;
        bool ParameterValue2 = !(nullable.GetValueOrDefault() == radioFlagsPdCwMbus4 & nullable.HasValue);
        r1.Add(OverrideID.Encryption, new ConfigurationParameter(OverrideID.Encryption, (object) ParameterValue2)
        {
          HasWritePermission = flag1 | flag2
        });
        r1.Add(OverrideID.ListType, new ConfigurationParameter(OverrideID.ListType, (object) meter.GetRadioListType()));
        bool ParameterValue3 = (meter.GetConfigFlagsPDCwMBus().Value & ConfigFlagsPDCwMBus.CONFIG_ENABLE_RADIO) != 0;
        r1.Add(OverrideID.RadioEnabled, new ConfigurationParameter(OverrideID.RadioEnabled, (object) ParameterValue3)
        {
          HasWritePermission = true
        });
        bool ParameterValue4 = (meter.GetConfigFlagsPDCwMBus().Value & ConfigFlagsPDCwMBus.CONFIG_ENABLE_PULSE) != 0;
        r1.Add(OverrideID.PulseEnabled, new ConfigurationParameter(OverrideID.PulseEnabled, (object) ParameterValue4)
        {
          HasWritePermission = true
        });
      }
      return r1;
    }

    public override bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameterList,
      int SubDevice)
    {
      if (this.pdc == null)
        return false;
      switch (SubDevice)
      {
        case 0:
          this.prmsMain = this.SetPrmsMain(parameterList);
          break;
        case 1:
          this.prmsInputA = this.SetPrmsInputA(parameterList);
          break;
        case 2:
          this.prmsInputB = this.SetPrmsInputB(parameterList);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (SubDevice));
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
      if (this.pdc == null)
        return false;
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      if (!this.pdc.WriteDevice())
        return false;
      this.CallFunctionPrmsMain();
      this.CallFunctionPrmsInputA();
      this.CallFunctionPrmsInputB();
      this.prmsMain = (SortedList<OverrideID, ConfigurationParameter>) null;
      this.prmsInputA = (SortedList<OverrideID, ConfigurationParameter>) null;
      this.prmsInputB = (SortedList<OverrideID, ConfigurationParameter>) null;
      return true;
    }

    private SortedList<OverrideID, ConfigurationParameter> SetPrmsMain(
      SortedList<OverrideID, ConfigurationParameter> newParameter)
    {
      if (newParameter == null)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      PDC_Meter meter = this.pdc.Meter;
      if (meter == null || meter.Version == null)
        throw new ArgumentNullException("meter");
      SortedList<OverrideID, ConfigurationParameter> sortedList = new SortedList<OverrideID, ConfigurationParameter>();
      RadioFlagsPDCwMBus? nullable1;
      ConfigFlagsPDCwMBus? nullable2;
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in newParameter)
      {
        if (keyValuePair.Value.HasWritePermission && keyValuePair.Value.ParameterValue != null)
        {
          switch (keyValuePair.Key)
          {
            case OverrideID.DueDate:
              if (!meter.SetDueDate(Convert.ToDateTime(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetDueDate failed!");
              break;
            case OverrideID.RadioEnabled:
              ConfigFlagsPDCwMBus? configFlagsPdCwMbus1 = meter.GetConfigFlagsPDCwMBus();
              if (!configFlagsPdCwMbus1.HasValue)
                throw new Exception("GetConfigFlagsPDCwMBus failed!");
              ConfigFlagsPDCwMBus? nullable3;
              if (Convert.ToBoolean(keyValuePair.Value.ParameterValue))
              {
                nullable2 = configFlagsPdCwMbus1;
                nullable3 = nullable2.HasValue ? new ConfigFlagsPDCwMBus?(nullable2.GetValueOrDefault() | ConfigFlagsPDCwMBus.CONFIG_ENABLE_RADIO) : new ConfigFlagsPDCwMBus?();
              }
              else
              {
                nullable2 = configFlagsPdCwMbus1;
                nullable3 = nullable2.HasValue ? new ConfigFlagsPDCwMBus?(nullable2.GetValueOrDefault() & ~ConfigFlagsPDCwMBus.CONFIG_ENABLE_RADIO) : new ConfigFlagsPDCwMBus?();
              }
              if (!meter.SetConfigFlagsPDCwMBus(nullable3.Value))
                throw new Exception("SetConfigFlagsPDCwMBus failed!");
              break;
            case OverrideID.TimeZone:
              if (!meter.SetTimeZone(Convert.ToInt32(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetTimeZone failed!");
              break;
            case OverrideID.AESKey:
              if (keyValuePair.Value.ParameterValue != null)
              {
                if (!meter.SetAESkey(keyValuePair.Value.ParameterValue))
                  throw new Exception("SetAESkey failed!");
                break;
              }
              if (!meter.SetAESkey((byte[]) null))
                throw new Exception("SetAESkey failed!");
              break;
            case OverrideID.ListType:
              meter.SetRadioListType(keyValuePair.Value.ToString());
              break;
            case OverrideID.LongHeader:
              RadioFlagsPDCwMBus? radioFlagsPdCwMbus1 = meter.GetRadioFlagsPDCwMBus();
              if (!radioFlagsPdCwMbus1.HasValue)
                throw new Exception("GetRadioFlagsPDCwMBus failed!");
              RadioFlagsPDCwMBus? nullable4;
              if (Convert.ToBoolean(keyValuePair.Value.ParameterValue))
              {
                nullable1 = radioFlagsPdCwMbus1;
                nullable4 = nullable1.HasValue ? new RadioFlagsPDCwMBus?(nullable1.GetValueOrDefault() | RadioFlagsPDCwMBus.CONFIG_RADIO_LONGHEADER) : new RadioFlagsPDCwMBus?();
              }
              else
              {
                nullable1 = radioFlagsPdCwMbus1;
                nullable4 = nullable1.HasValue ? new RadioFlagsPDCwMBus?(nullable1.GetValueOrDefault() & ~RadioFlagsPDCwMBus.CONFIG_RADIO_LONGHEADER) : new RadioFlagsPDCwMBus?();
              }
              if (!meter.SetRadioFlagsPDCwMBus(nullable4.Value))
                throw new Exception("SetRadioFlagsPDCwMBus failed!");
              break;
            case OverrideID.Encryption:
              RadioFlagsPDCwMBus? radioFlagsPdCwMbus2 = meter.GetRadioFlagsPDCwMBus();
              if (!radioFlagsPdCwMbus2.HasValue)
                throw new Exception("GetRadioFlagsPDCwMBus failed!");
              RadioFlagsPDCwMBus? nullable5;
              if (Convert.ToBoolean(keyValuePair.Value.ParameterValue))
              {
                nullable1 = radioFlagsPdCwMbus2;
                nullable5 = nullable1.HasValue ? new RadioFlagsPDCwMBus?(nullable1.GetValueOrDefault() | RadioFlagsPDCwMBus.CONFIG_RADIO_ENCRYPT) : new RadioFlagsPDCwMBus?();
              }
              else
              {
                nullable1 = radioFlagsPdCwMbus2;
                nullable5 = nullable1.HasValue ? new RadioFlagsPDCwMBus?(nullable1.GetValueOrDefault() & ~RadioFlagsPDCwMBus.CONFIG_RADIO_ENCRYPT) : new RadioFlagsPDCwMBus?();
              }
              if (!meter.SetRadioFlagsPDCwMBus(nullable5.Value))
                throw new Exception("SetRadioFlagsPDCwMBus failed!");
              break;
            case OverrideID.PulseEnabled:
              ConfigFlagsPDCwMBus? configFlagsPdCwMbus2 = meter.GetConfigFlagsPDCwMBus();
              if (!configFlagsPdCwMbus2.HasValue)
                throw new Exception("GetConfigFlagsPDCwMBus failed!");
              ConfigFlagsPDCwMBus? nullable6;
              if (Convert.ToBoolean(keyValuePair.Value.ParameterValue))
              {
                nullable2 = configFlagsPdCwMbus2;
                nullable6 = nullable2.HasValue ? new ConfigFlagsPDCwMBus?(nullable2.GetValueOrDefault() | ConfigFlagsPDCwMBus.CONFIG_ENABLE_PULSE) : new ConfigFlagsPDCwMBus?();
              }
              else
              {
                nullable2 = configFlagsPdCwMbus2;
                nullable6 = nullable2.HasValue ? new ConfigFlagsPDCwMBus?(nullable2.GetValueOrDefault() & ~ConfigFlagsPDCwMBus.CONFIG_ENABLE_PULSE) : new ConfigFlagsPDCwMBus?();
              }
              if (!meter.SetConfigFlagsPDCwMBus(nullable6.Value))
                throw new Exception("SetConfigFlagsPDCwMBus failed!");
              break;
            default:
              sortedList.Add(keyValuePair.Key, keyValuePair.Value);
              break;
          }
        }
      }
      return sortedList;
    }

    private SortedList<OverrideID, ConfigurationParameter> SetPrmsInputA(
      SortedList<OverrideID, ConfigurationParameter> newParameter)
    {
      if (newParameter == null)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      PDC_Meter meter = this.pdc.Meter;
      if (meter == null || meter.Version == null)
        throw new ArgumentNullException("meter");
      SortedList<OverrideID, ConfigurationParameter> sortedList = new SortedList<OverrideID, ConfigurationParameter>();
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in newParameter)
      {
        if (keyValuePair.Value.HasWritePermission && keyValuePair.Value.ParameterValue != null)
        {
          switch (keyValuePair.Key)
          {
            case OverrideID.SerialNumber:
              if (!meter.SetSerialMBusInputA(Convert.ToUInt32(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetSerialMBusInputA failed!");
              break;
            case OverrideID.Medium:
              if (!meter.SetMediumInputA((MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), keyValuePair.Value.ParameterValue.ToString(), true)))
                throw new Exception("SetMediumInputA failed!");
              break;
            case OverrideID.InputPulsValue:
              if (!meter.SetScaleFactorInputA((double) Convert.ToSingle(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetScaleFactorInputA failed!");
              break;
            case OverrideID.RadioEnabled:
              RadioFlagsPDCwMBus? radioFlagsPdCwMbus = meter.GetRadioFlagsPDCwMBus();
              if (!radioFlagsPdCwMbus.HasValue)
                throw new Exception("GetRadioFlagsPDCwMBus failed!");
              RadioFlagsPDCwMBus? nullable1;
              RadioFlagsPDCwMBus? nullable2;
              if (Convert.ToBoolean(keyValuePair.Value.ParameterValue))
              {
                nullable1 = radioFlagsPdCwMbus;
                nullable2 = nullable1.HasValue ? new RadioFlagsPDCwMBus?(nullable1.GetValueOrDefault() | RadioFlagsPDCwMBus.CONFIG_RADIO_CHANNEL_A) : new RadioFlagsPDCwMBus?();
              }
              else
              {
                nullable1 = radioFlagsPdCwMbus;
                nullable2 = nullable1.HasValue ? new RadioFlagsPDCwMBus?(nullable1.GetValueOrDefault() & ~RadioFlagsPDCwMBus.CONFIG_RADIO_CHANNEL_A) : new RadioFlagsPDCwMBus?();
              }
              if (!meter.SetRadioFlagsPDCwMBus(nullable2.Value))
                throw new Exception("SetRadioFlagsPDCwMBus failed!");
              break;
            case OverrideID.Manufacturer:
              if (!meter.SetManufacturerInputA(keyValuePair.Value.ParameterValue.ToString()))
                throw new Exception("SetManufacturerInputA failed!");
              break;
            case OverrideID.InputResolutionStr:
              ResolutionData resolutionData = MeterUnits.GetResolutionData(keyValuePair.Value.ParameterValue.ToString());
              if (!meter.SetVIFInputA((byte) resolutionData.mbusVIF))
                throw new Exception("SetVIFInputA failed!");
              break;
            case OverrideID.PulseBlockLimit:
              if (!meter.SetPulseBlockLimitInputA(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetPulseBlockLimitInputA failed!");
              break;
            case OverrideID.PulseLeakLimit:
              if (!meter.SetPulseLeakLimitInputA(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetPulseLeakLimitInputA failed!");
              break;
            case OverrideID.PulseUnleakLimit:
              if (!meter.SetPulseUnleakLimitInputA(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetPulseUnleakLimitInputA failed!");
              break;
            case OverrideID.PulseLeakLower:
              if (!meter.SetPulseLeakLowerInputA(Convert.ToInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetPulseLeakLowerInputA failed!");
              break;
            case OverrideID.PulseLeakUpper:
              if (!meter.SetPulseLeakUpperInputA(Convert.ToInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetPulseLeakUpperInputA failed!");
              break;
            case OverrideID.OversizeDiff:
              if (!meter.SetOversizeDiffInputA(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetOversizeDiffInputA failed!");
              break;
            case OverrideID.OversizeLimit:
              if (!meter.SetOversizeLimitInputA(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetOversizeLimitInputA failed!");
              break;
            case OverrideID.UndersizeDiff:
              if (!meter.SetUndersizeDiffInputA(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetUndersizeDiffInputA failed!");
              break;
            case OverrideID.UndersizeLimit:
              if (!meter.SetUndersizeLimitInputA(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetUndersizeLimitInputA failed!");
              break;
            case OverrideID.BurstDiff:
              if (!meter.SetBurstDiffInputA(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetBurstDiffInputA failed!");
              break;
            case OverrideID.BurstLimit:
              if (!meter.SetBurstLimitInputA(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetBurstLimitInputA failed!");
              break;
            case OverrideID.MBusGeneration:
              if (!meter.SetMBusGenerationInputA(Convert.ToByte(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetMBusGenerationInputA failed!");
              break;
            case OverrideID.ClearWarnings:
              if (Convert.ToBoolean(keyValuePair.Value.ParameterValue) && !meter.SetWarningsInputA((Warning) 0))
                throw new Exception("SetWarningsInputA failed!");
              continue;
            case OverrideID.NominalFlow:
              double num = Convert.ToDouble(keyValuePair.Value.ParameterValue, (IFormatProvider) FixedFormates.TheFormates.NumberFormat);
              meter.SetNominalFlowA(num);
              break;
            default:
              sortedList.Add(keyValuePair.Key, keyValuePair.Value);
              break;
          }
        }
      }
      return sortedList;
    }

    private SortedList<OverrideID, ConfigurationParameter> SetPrmsInputB(
      SortedList<OverrideID, ConfigurationParameter> newParameter)
    {
      if (newParameter == null)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      PDC_Meter meter = this.pdc.Meter;
      if (meter == null || meter.Version == null)
        throw new ArgumentNullException("meter");
      SortedList<OverrideID, ConfigurationParameter> sortedList = new SortedList<OverrideID, ConfigurationParameter>();
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in newParameter)
      {
        if (keyValuePair.Value.HasWritePermission && keyValuePair.Value.ParameterValue != null)
        {
          switch (keyValuePair.Key)
          {
            case OverrideID.SerialNumber:
              if (!meter.SetSerialMBusInputB(Convert.ToUInt32(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetSerialMBusInputB failed!");
              break;
            case OverrideID.Medium:
              if (!meter.SetMediumInputB((MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), keyValuePair.Value.ParameterValue.ToString(), true)))
                throw new Exception("SetMediumInputB failed!");
              break;
            case OverrideID.InputPulsValue:
              if (!meter.SetScaleFactorInputB((double) Convert.ToSingle(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetScaleFactorInputB failed!");
              break;
            case OverrideID.RadioEnabled:
              RadioFlagsPDCwMBus? radioFlagsPdCwMbus = meter.GetRadioFlagsPDCwMBus();
              if (!radioFlagsPdCwMbus.HasValue)
                throw new Exception("GetRadioFlagsPDCwMBus failed!");
              RadioFlagsPDCwMBus? nullable1;
              RadioFlagsPDCwMBus? nullable2;
              if (Convert.ToBoolean(keyValuePair.Value.ParameterValue))
              {
                nullable1 = radioFlagsPdCwMbus;
                nullable2 = nullable1.HasValue ? new RadioFlagsPDCwMBus?(nullable1.GetValueOrDefault() | RadioFlagsPDCwMBus.CONFIG_RADIO_CHANNEL_B) : new RadioFlagsPDCwMBus?();
              }
              else
              {
                nullable1 = radioFlagsPdCwMbus;
                nullable2 = nullable1.HasValue ? new RadioFlagsPDCwMBus?(nullable1.GetValueOrDefault() & ~RadioFlagsPDCwMBus.CONFIG_RADIO_CHANNEL_B) : new RadioFlagsPDCwMBus?();
              }
              if (!meter.SetRadioFlagsPDCwMBus(nullable2.Value))
                throw new Exception("SetRadioFlagsPDCwMBus failed!");
              break;
            case OverrideID.Manufacturer:
              if (!meter.SetManufacturerInputB(keyValuePair.Value.ParameterValue.ToString()))
                throw new Exception("SetManufacturerInputB failed!");
              break;
            case OverrideID.InputResolutionStr:
              ResolutionData resolutionData = MeterUnits.GetResolutionData(keyValuePair.Value.ParameterValue.ToString());
              if (!meter.SetVIFInputB((byte) resolutionData.mbusVIF))
                throw new Exception("SetVIFInputB failed!");
              break;
            case OverrideID.PulseBlockLimit:
              if (!meter.SetPulseBlockLimitInputB(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetPulseBlockLimitInputB failed!");
              break;
            case OverrideID.PulseLeakLimit:
              if (!meter.SetPulseLeakLimitInputB(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetPulseLeakLimitInputB failed!");
              break;
            case OverrideID.PulseUnleakLimit:
              if (!meter.SetPulseUnleakLimitInputB(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetPulseUnleakLimitInputB failed!");
              break;
            case OverrideID.PulseLeakLower:
              if (!meter.SetPulseLeakLowerInputB(Convert.ToInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetPulseLeakLowerInputB failed!");
              break;
            case OverrideID.PulseLeakUpper:
              if (!meter.SetPulseLeakUpperInputB(Convert.ToInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetPulseLeakUpperInputB failed!");
              break;
            case OverrideID.OversizeDiff:
              if (!meter.SetOversizeDiffInputB(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetOversizeDiffInputB failed!");
              break;
            case OverrideID.OversizeLimit:
              if (!meter.SetOversizeLimitInputB(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetOversizeLimitInputB failed!");
              break;
            case OverrideID.UndersizeDiff:
              if (!meter.SetUndersizeDiffInputB(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetUndersizeDiffInputB failed!");
              break;
            case OverrideID.UndersizeLimit:
              if (!meter.SetUndersizeLimitInputB(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetUndersizeLimitInputB failed!");
              break;
            case OverrideID.BurstDiff:
              if (!meter.SetBurstDiffInputB(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetBurstDiffInputB failed!");
              break;
            case OverrideID.BurstLimit:
              if (!meter.SetBurstLimitInputB(Convert.ToUInt16(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetBurstLimitInputB failed!");
              break;
            case OverrideID.MBusGeneration:
              if (!meter.SetMBusGenerationInputB(Convert.ToByte(keyValuePair.Value.ParameterValue)))
                throw new Exception("SetMBusGenerationInputB failed!");
              break;
            case OverrideID.ClearWarnings:
              if (Convert.ToBoolean(keyValuePair.Value.ParameterValue) && !meter.SetWarningsInputB((Warning) 0))
                throw new Exception("SetWarningsInputB failed!");
              continue;
            case OverrideID.NominalFlow:
              double num = Convert.ToDouble(keyValuePair.Value.ParameterValue, (IFormatProvider) FixedFormates.TheFormates.NumberFormat);
              meter.SetNominalFlowB(num);
              break;
            default:
              sortedList.Add(keyValuePair.Key, keyValuePair.Value);
              break;
          }
        }
      }
      return sortedList;
    }

    private void CallFunctionPrmsMain()
    {
      if (this.prmsMain == null)
        return;
      PDC_Meter meter = this.pdc.Meter;
      DeviceVersion deviceVersion = meter != null && meter.Version != null ? meter.Version : throw new ArgumentNullException("meter");
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in this.prmsMain)
      {
        if (keyValuePair.Value.IsFunction && (keyValuePair.Value.ParameterValue != null || keyValuePair.Value.HasWritePermission))
        {
          if (keyValuePair.Key != OverrideID.SetPcTime)
            throw new ArgumentException("Not handled parameter detected: " + keyValuePair.ToString());
          if (Convert.ToBoolean(keyValuePair.Value.ParameterValue) && !this.pdc.WriteSystemTime(DateTime.Now))
            throw new Exception("WriteSystemTime failed!");
        }
      }
    }

    private void CallFunctionPrmsInputA()
    {
      if (this.prmsInputA == null)
        return;
      PDC_Meter meter = this.pdc.Meter;
      DeviceVersion deviceVersion = meter != null && meter.Version != null ? meter.Version : throw new ArgumentNullException("meter");
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in this.prmsInputA)
      {
        if ((keyValuePair.Key == OverrideID.TotalPulse || keyValuePair.Value.IsFunction) && (keyValuePair.Value.ParameterValue != null || keyValuePair.Value.HasWritePermission))
        {
          if (keyValuePair.Key != OverrideID.TotalPulse)
            throw new ArgumentException("Not handled parameter detected: " + keyValuePair.ToString());
          double? scaleFactorInputA = meter.GetScaleFactorInputA();
          if (!scaleFactorInputA.HasValue)
            throw new Exception("GetScaleFactorInputA failed!");
          uint uint32_1 = Convert.ToUInt32(keyValuePair.Value.ParameterValue);
          PDC_HandlerFunctions pdc = this.pdc;
          double num = (double) uint32_1;
          double? nullable = scaleFactorInputA;
          int uint32_2 = (int) Convert.ToUInt32((object) (nullable.HasValue ? new double?(num * nullable.GetValueOrDefault()) : new double?()));
          if (!pdc.WriteMeterValue((byte) 0, (uint) uint32_2))
            throw new Exception("WriteMeterValue input A failed!");
        }
      }
    }

    private void CallFunctionPrmsInputB()
    {
      if (this.prmsInputB == null)
        return;
      PDC_Meter meter = this.pdc.Meter;
      DeviceVersion deviceVersion = meter != null && meter.Version != null ? meter.Version : throw new ArgumentNullException("meter");
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in this.prmsInputB)
      {
        if ((keyValuePair.Key == OverrideID.TotalPulse || keyValuePair.Value.IsFunction) && (keyValuePair.Value.ParameterValue != null || keyValuePair.Value.HasWritePermission))
        {
          if (keyValuePair.Key != OverrideID.TotalPulse)
            throw new ArgumentException("Not handled parameter detected: " + keyValuePair.ToString());
          double? scaleFactorInputB = meter.GetScaleFactorInputB();
          if (!scaleFactorInputB.HasValue)
            throw new Exception("GetScaleFactorInputB failed!");
          uint uint32_1 = Convert.ToUInt32(keyValuePair.Value.ParameterValue);
          PDC_HandlerFunctions pdc = this.pdc;
          double num = (double) uint32_1;
          double? nullable = scaleFactorInputB;
          int uint32_2 = (int) Convert.ToUInt32((object) (nullable.HasValue ? new double?(num * nullable.GetValueOrDefault()) : new double?()));
          if (!pdc.WriteMeterValue((byte) 1, (uint) uint32_2))
            throw new Exception("WriteMeterValue input B failed!");
        }
      }
    }

    public override bool BeginSearchDevices()
    {
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      return this.ReadAll((List<long>) null);
    }

    private void FireEventOnValueIdentSetReceived()
    {
      if (!this.MyDeviceManager.IsValueIdentSetReceivedEventEnabled || this.pdc.Meter == null || this.pdc.Meter.Version == null)
        return;
      ValueIdentSet e1 = new ValueIdentSet();
      e1.Manufacturer = this.pdc.Meter.GetManufacturerInputA();
      byte? generationInputA = this.pdc.Meter.GetMBusGenerationInputA();
      byte num;
      if (generationInputA.HasValue)
      {
        ValueIdentSet valueIdentSet = e1;
        num = generationInputA.Value;
        string str = num.ToString();
        valueIdentSet.Version = str;
      }
      MBusDeviceType? mediumInputA = this.pdc.Meter.GetMediumInputA();
      MBusDeviceType mbusDeviceType;
      if (mediumInputA.HasValue)
      {
        ValueIdentSet valueIdentSet = e1;
        mbusDeviceType = mediumInputA.Value;
        string str = mbusDeviceType.ToString();
        valueIdentSet.DeviceType = str;
      }
      else
        e1.DeviceType = MBusDeviceType.UNKNOWN.ToString();
      uint? serialMbusInputA = this.pdc.Meter.GetSerialMBusInputA();
      if (serialMbusInputA.HasValue)
        e1.SerialNumber = serialMbusInputA.ToString();
      e1.ZDF = "SID;" + e1.SerialNumber + ";MAN;MINOL;MED;" + e1.DeviceType;
      SortedList<long, SortedList<DateTime, ReadingValue>> ValueList1 = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      if (this.GetValues(ref ValueList1, 1))
        e1.AvailableValues = ValueList1;
      this.MyDeviceManager.OnValueIdentSetReceived((object) this, e1);
      ValueIdentSet e2 = new ValueIdentSet();
      e2.Manufacturer = this.pdc.Meter.GetManufacturerInputB();
      byte? generationInputB = this.pdc.Meter.GetMBusGenerationInputB();
      if (generationInputB.HasValue)
      {
        ValueIdentSet valueIdentSet = e2;
        num = generationInputB.Value;
        string str = num.ToString();
        valueIdentSet.Version = str;
      }
      MBusDeviceType? mediumInputB = this.pdc.Meter.GetMediumInputB();
      if (mediumInputB.HasValue)
      {
        ValueIdentSet valueIdentSet = e2;
        mbusDeviceType = mediumInputB.Value;
        string str = mbusDeviceType.ToString();
        valueIdentSet.DeviceType = str;
      }
      else
      {
        ValueIdentSet valueIdentSet = e2;
        mbusDeviceType = MBusDeviceType.UNKNOWN;
        string str = mbusDeviceType.ToString();
        valueIdentSet.DeviceType = str;
      }
      uint? serialMbusInputB = this.pdc.Meter.GetSerialMBusInputB();
      if (serialMbusInputB.HasValue)
        e2.SerialNumber = serialMbusInputB.ToString();
      e2.ZDF = "SID;" + e2.SerialNumber + ";MAN;MINOL;MED;" + e2.DeviceType;
      SortedList<long, SortedList<DateTime, ReadingValue>> ValueList2 = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      if (this.GetValues(ref ValueList2, 2))
        e2.AvailableValues = ValueList2;
      this.MyDeviceManager.OnValueIdentSetReceived((object) this, e2);
    }
  }
}
