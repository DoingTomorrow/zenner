// Decompiled with JetBrains decompiler
// Type: Devices.EdcHandler
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using DeviceCollector;
using EDC_Handler;
using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace Devices
{
  public class EdcHandler(DeviceManager MyDeviceManager) : BaseDevice(MyDeviceManager)
  {
    private SortedList<OverrideID, ConfigurationParameter> newParameters;
    private static Logger logger = LogManager.GetLogger(nameof (EdcHandler));
    private EDC_HandlerFunctions edc;

    public override event EventHandlerEx<int> OnProgress;

    public override object GetHandler()
    {
      this.GarantHandlerLoaded();
      return (object) this.edc;
    }

    private void GarantHandlerLoaded()
    {
      if (this.edc != null)
        return;
      if (ZR_Component.CommonGmmInterface != null)
      {
        ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.EDC_Handler);
        this.edc = (EDC_HandlerFunctions) ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.EDC_Handler];
      }
      else
        this.edc = new EDC_HandlerFunctions((IDeviceCollector) this.MyDeviceManager.MyBus);
      this.edc.OnProgress += new ValueEventHandler<int>(this.edc_OnProgress);
    }

    private void edc_OnProgress(object sender, int e)
    {
      if (this.OnProgress == null)
        return;
      this.OnProgress(sender, e);
    }

    internal override void ShowHandlerWindow()
    {
      this.GarantHandlerLoaded();
      this.edc.ShowHandlerWindow();
    }

    public override void Dispose()
    {
      if (this.edc == null)
        return;
      this.edc.OnProgress -= new ValueEventHandler<int>(this.edc_OnProgress);
      this.edc.GMM_Dispose();
      this.edc = (EDC_HandlerFunctions) null;
    }

    public override List<GlobalDeviceId> GetGlobalDeviceIdList()
    {
      this.GarantHandlerLoaded();
      if (this.edc.Meter == null || this.edc.Meter.Version == null)
        return (List<GlobalDeviceId>) null;
      return new List<GlobalDeviceId>()
      {
        new GlobalDeviceId()
        {
          Serialnumber = this.edc.Meter.GetSerialnumberSecondary().Value.ToString(),
          DeviceTypeName = this.edc.Meter.Version.Type.ToString(),
          Manufacturer = this.edc.Meter.GetManufacturerSecondary(),
          FirmwareVersion = this.edc.Meter.Version.ToString(),
          MeterType = ValueIdent.ValueIdPart_MeterType.Water
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
        return !eventMessage.Cancel && this.edc.ReadDevice();
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
      bool flag = this.edc.ReadDevice(true);
      if (flag)
        this.FireEventOnValueIdentSetReceived();
      return flag;
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
      if (this.edc == null)
        return false;
      List<long> filter = (List<long>) null;
      if (ValueList == null)
        ValueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      else if (ValueList.Count > 0)
      {
        filter = new List<long>();
        filter.AddRange((IEnumerable<long>) ValueList.Keys);
      }
      ValueList = this.edc.GetValues(filter);
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
      this.newParameters = (SortedList<OverrideID, ConfigurationParameter>) null;
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      this.GarantHandlerLoaded();
      if (!this.edc.ReadDevice())
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
      if (this.edc == null)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      if (SubDevice != 0)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "EDC device has no sub devices!");
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      }
      SortedList<OverrideID, ConfigurationParameter> r = new SortedList<OverrideID, ConfigurationParameter>();
      EDC_Meter meter = this.edc.Meter;
      if (meter == null || meter.Version == null)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      DeviceVersion version = meter.Version;
      if (UserManager.IsNewLicenseModel())
      {
        EdcHandler.AddParam(false, r, OverrideID.FirmwareVersion, (object) version.VersionString);
        EdcHandler.AddParam(false, r, OverrideID.SerialNumberFull, (object) meter.GetSerialnumberFull());
        EdcHandler.AddParam(false, r, OverrideID.Manufacturer, (object) meter.GetManufacturerPrimary());
        EdcHandler.AddParam(false, r, OverrideID.DeviceHasError, (object) meter.GetDeviceErrorState());
        EdcHandler.AddParam(false, r, OverrideID.SerialNumber, (object) meter.GetSerialnumberPrimary());
        EdcHandler.AddParam(false, r, OverrideID.MBusGeneration, (object) meter.GetMBusGenerationPrimary());
        EdcHandler.AddParam(false, r, OverrideID.DeviceClock, (object) meter.GetSystemTime());
        EdcHandler.AddParam(false, r, OverrideID.WarningInfo, (object) meter.GetWarnings());
        EdcHandler.AddParam(false, r, OverrideID.Manipulation, (object) meter.GetMagnetDetectionState());
        EdcHandler.AddParam(true, r, OverrideID.PulseMultiplier, (object) (Decimal) meter.GetPulseMultiplier().Value);
        EdcHandler.AddParam(true, r, OverrideID.PulseEnabled, (object) meter.GetCoilSampling());
        EdcHandler.AddParam(true, r, OverrideID.TimeZone, (object) (Decimal) meter.GetTimeZone().Value);
        EdcHandler.AddParam(true, r, OverrideID.MediumSecondary, (object) meter.GetMediumSecondary());
        EdcHandler.AddParam(true, r, OverrideID.SerialNumberSecondary, (object) meter.GetSerialnumberSecondary());
        EdcHandler.AddParam(true, r, OverrideID.MBusAddress, (object) meter.GetMBusAddressSecondary());
        EdcHandler.AddParam(true, r, OverrideID.ManufacturerSecondary, (object) meter.GetManufacturerSecondary());
        EdcHandler.AddParam(true, r, OverrideID.Medium, (object) meter.GetMediumPrimary());
        EdcHandler.AddParam(true, r, OverrideID.TotalVolumePulses, (object) meter.GetMeterValue());
        EdcHandler.AddParam(true, r, OverrideID.DueDate, new ConfigurationParameter(OverrideID.DueDate, (object) meter.GetDueDate())
        {
          Format = "{0:dd.MM}"
        });
        EdcHandler.AddParam(true, r, OverrideID.RegisterDigits, (object) meter.GetCogCount());
        EdcHandler.AddParam(true, r, OverrideID.PulseBlockLimit, (object) meter.GetPulseBlockLimit());
        EdcHandler.AddParam(true, r, OverrideID.PulseLeakLimit, (object) meter.GetPulseLeakLimit());
        EdcHandler.AddParam(true, r, OverrideID.PulseUnleakLimit, (object) meter.GetPulseUnleakLimit());
        EdcHandler.AddParam(true, r, OverrideID.PulseLeakLower, (object) meter.GetPulseLeakLower());
        EdcHandler.AddParam(true, r, OverrideID.PulseLeakUpper, (object) meter.GetPulseLeakUpper());
        EdcHandler.AddParam(true, r, OverrideID.PulseBackLimit, (object) meter.GetPulseBackLimit());
        EdcHandler.AddParam(true, r, OverrideID.PulseUnbackLimit, (object) meter.GetPulseUnbackLimit());
        EdcHandler.AddParam(true, r, OverrideID.OversizeDiff, (object) meter.GetOversizeDiff());
        EdcHandler.AddParam(true, r, OverrideID.OversizeLimit, (object) meter.GetOversizeLimit());
        EdcHandler.AddParam(true, r, OverrideID.UndersizeDiff, (object) meter.GetUndersizeDiff());
        EdcHandler.AddParam(true, r, OverrideID.UndersizeLimit, (object) meter.GetUndersizeLimit());
        EdcHandler.AddParam(true, r, OverrideID.BurstDiff, (object) meter.GetBurstDiff());
        EdcHandler.AddParam(true, r, OverrideID.BurstLimit, (object) meter.GetBurstLimit());
        EdcHandler.AddParam(true, r, OverrideID.ClearWarnings, (object) false, true, (string[]) null);
        EdcHandler.AddParam(true, r, OverrideID.ClearAllLoggers, (object) false, true, (string[]) null);
        EdcHandler.AddParam(true, r, OverrideID.SetPcTime, (object) false, true, (string[]) null);
        EdcHandler.AddParam(true, r, OverrideID.ClearManipulation, (object) false, true, (string[]) null);
        EdcHandler.AddParam(false, r, OverrideID.ListType, (object) meter.GetMBusListType());
        EdcHandler.AddParam(true, r, OverrideID.NominalFlow, (object) meter.GetNominalFlow(), false, meter.GetNominalFlowAllowedValues());
        if (meter.Version.Type == EDC_Hardware.EDC_Radio)
        {
          EdcHandler.AddParam(false, r, OverrideID.RadioMode, (object) meter.GetRadioMode());
          EdcHandler.AddParam(false, r, OverrideID.RadioSendInterval, (object) meter.GetRadioTransmitInterval());
          EdcHandler.AddParam(true, r, OverrideID.AESKey, (object) AES.AesKeyToString(meter.GetAESkey()));
          EdcHandler.AddParam(true, r, OverrideID.LongHeader, (object) meter.GetWMBusLongHeaderState());
          EdcHandler.AddParam(true, r, OverrideID.RadioEnabled, (object) meter.GetRadioState());
          EdcHandler.AddParam(true, r, OverrideID.Encryption, (object) meter.GetWMBusEncryptionState());
          int int32 = Convert.ToInt32(meter.GetVolumeAccumulatedNegativ());
          EdcHandler.AddParam(false, r, OverrideID.TotalVolumePulsesNegativ, (object) int32);
        }
        else if (meter.Version.Type == EDC_Hardware.EDC_mBus)
        {
          EdcHandler.AddParam(true, r, OverrideID.PulseoutMode, (object) meter.GetPulseoutMode());
          double num = Math.Round((double) meter.GetPulseoutWidth().Value * 2000.0 / 32768.0, 2);
          EdcHandler.AddParam(true, r, OverrideID.PulseoutWidth, (object) num);
          EdcHandler.AddParam(true, r, OverrideID.PulseoutResolution, (object) meter.GetPulseoutPPP());
          EdcHandler.AddParam(true, r, OverrideID.Baudrate, (object) meter.GetMbusBaud());
        }
        DeviceIdentification deviceIdentification = meter.GetDeviceIdentification();
        if (deviceIdentification != null)
          EdcHandler.AddParam(false, r, OverrideID.MeterID, (object) deviceIdentification.MeterID);
      }
      else
      {
        bool flag1 = false;
        if (UserRights.GlobalUserRights.PackageName == "ConfigurationManagerPro")
          flag1 = true;
        else if (UserRights.GlobalUserRights.PackageName == "ConfigurationManager" && UserRights.GlobalUserRights.OptionPackageName == "Professional")
          flag1 = true;
        bool flag2 = UserRights.GlobalUserRights.PackageName == "Designer";
        bool flag3 = UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Developer);
        r.Add(OverrideID.FirmwareVersion, new ConfigurationParameter(OverrideID.FirmwareVersion, (object) version.VersionString));
        r.Add(OverrideID.SerialNumberFull, new ConfigurationParameter(OverrideID.SerialNumberFull, (object) meter.GetSerialnumberFull()));
        r.Add(OverrideID.Manufacturer, new ConfigurationParameter(OverrideID.Manufacturer, (object) meter.GetManufacturerPrimary()));
        r.Add(OverrideID.DeviceHasError, new ConfigurationParameter(OverrideID.DeviceHasError, (object) meter.GetDeviceErrorState()));
        r.Add(OverrideID.SerialNumber, new ConfigurationParameter(OverrideID.SerialNumber, (object) meter.GetSerialnumberPrimary()));
        r.Add(OverrideID.MBusGeneration, new ConfigurationParameter(OverrideID.MBusGeneration, (object) meter.GetMBusGenerationPrimary()));
        r.Add(OverrideID.DeviceClock, new ConfigurationParameter(OverrideID.DeviceClock, (object) meter.GetSystemTime()));
        r.Add(OverrideID.WarningInfo, new ConfigurationParameter(OverrideID.WarningInfo, (object) meter.GetWarnings()));
        r.Add(OverrideID.Manipulation, new ConfigurationParameter(OverrideID.Manipulation, (object) meter.GetMagnetDetectionState()));
        r.Add(OverrideID.PulseMultiplier, new ConfigurationParameter(OverrideID.PulseMultiplier, (object) (Decimal) meter.GetPulseMultiplier().Value)
        {
          HasWritePermission = true
        });
        r.Add(OverrideID.PulseEnabled, new ConfigurationParameter(OverrideID.PulseEnabled, (object) meter.GetCoilSampling())
        {
          HasWritePermission = true
        });
        r.Add(OverrideID.TimeZone, new ConfigurationParameter(OverrideID.TimeZone, (object) (Decimal) meter.GetTimeZone().Value)
        {
          HasWritePermission = true
        });
        r.Add(OverrideID.MediumSecondary, new ConfigurationParameter(OverrideID.MediumSecondary, (object) meter.GetMediumSecondary())
        {
          HasWritePermission = true
        });
        r.Add(OverrideID.SerialNumberSecondary, new ConfigurationParameter(OverrideID.SerialNumberSecondary, (object) meter.GetSerialnumberSecondary())
        {
          HasWritePermission = true
        });
        r.Add(OverrideID.MBusAddress, new ConfigurationParameter(OverrideID.MBusAddress, (object) meter.GetMBusAddressSecondary())
        {
          HasWritePermission = true
        });
        if (flag2)
          r.Add(OverrideID.ManufacturerSecondary, new ConfigurationParameter(OverrideID.ManufacturerSecondary, (object) meter.GetManufacturerSecondary())
          {
            HasWritePermission = true
          });
        r.Add(OverrideID.Medium, new ConfigurationParameter(OverrideID.Medium, (object) meter.GetMediumPrimary())
        {
          HasWritePermission = true
        });
        r.Add(OverrideID.TotalVolumePulses, new ConfigurationParameter(OverrideID.TotalVolumePulses, (object) meter.GetMeterValue())
        {
          HasWritePermission = true
        });
        r.Add(OverrideID.DueDate, new ConfigurationParameter(OverrideID.DueDate, (object) meter.GetDueDate())
        {
          HasWritePermission = true,
          Format = "{0:dd.MM}"
        });
        r.Add(OverrideID.RegisterDigits, new ConfigurationParameter(OverrideID.RegisterDigits, (object) meter.GetCogCount())
        {
          HasWritePermission = true
        });
        if (flag1 | flag2 | flag3)
        {
          r.Add(OverrideID.PulseBlockLimit, new ConfigurationParameter(OverrideID.PulseBlockLimit, (object) meter.GetPulseBlockLimit())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.PulseLeakLimit, new ConfigurationParameter(OverrideID.PulseLeakLimit, (object) meter.GetPulseLeakLimit())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.PulseUnleakLimit, new ConfigurationParameter(OverrideID.PulseUnleakLimit, (object) meter.GetPulseUnleakLimit())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.PulseLeakLower, new ConfigurationParameter(OverrideID.PulseLeakLower, (object) meter.GetPulseLeakLower())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.PulseLeakUpper, new ConfigurationParameter(OverrideID.PulseLeakUpper, (object) meter.GetPulseLeakUpper())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.PulseBackLimit, new ConfigurationParameter(OverrideID.PulseBackLimit, (object) meter.GetPulseBackLimit())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.PulseUnbackLimit, new ConfigurationParameter(OverrideID.PulseUnbackLimit, (object) meter.GetPulseUnbackLimit())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.OversizeDiff, new ConfigurationParameter(OverrideID.OversizeDiff, (object) meter.GetOversizeDiff())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.OversizeLimit, new ConfigurationParameter(OverrideID.OversizeLimit, (object) meter.GetOversizeLimit())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.UndersizeDiff, new ConfigurationParameter(OverrideID.UndersizeDiff, (object) meter.GetUndersizeDiff())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.UndersizeLimit, new ConfigurationParameter(OverrideID.UndersizeLimit, (object) meter.GetUndersizeLimit())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.BurstDiff, new ConfigurationParameter(OverrideID.BurstDiff, (object) meter.GetBurstDiff())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.BurstLimit, new ConfigurationParameter(OverrideID.BurstLimit, (object) meter.GetBurstLimit())
          {
            HasWritePermission = true
          });
        }
        r.Add(OverrideID.NominalFlow, new ConfigurationParameter(OverrideID.NominalFlow, (object) meter.GetNominalFlow())
        {
          HasWritePermission = true,
          AllowedValues = meter.GetNominalFlowAllowedValues()
        });
        if (meter.Version.Type == EDC_Hardware.EDC_mBus)
        {
          r.Add(OverrideID.PulseoutResolution, new ConfigurationParameter(OverrideID.PulseoutResolution, (object) meter.GetPulseoutPPP())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.PulseoutMode, new ConfigurationParameter(OverrideID.PulseoutMode, (object) meter.GetPulseoutMode())
          {
            HasWritePermission = true
          });
          double ParameterValue = Math.Round((double) meter.GetPulseoutWidth().Value * 2000.0 / 32768.0, 2);
          r.Add(OverrideID.PulseoutWidth, new ConfigurationParameter(OverrideID.PulseoutWidth, (object) ParameterValue)
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.Baudrate, new ConfigurationParameter(OverrideID.Baudrate, (object) meter.GetMbusBaud())
          {
            HasWritePermission = true
          });
        }
        r.Add(OverrideID.ClearWarnings, new ConfigurationParameter(OverrideID.ClearWarnings)
        {
          HasWritePermission = true,
          ParameterValue = (object) false
        });
        r.Add(OverrideID.ClearAllLoggers, new ConfigurationParameter(OverrideID.ClearAllLoggers)
        {
          HasWritePermission = true,
          ParameterValue = (object) false
        });
        r.Add(OverrideID.SetPcTime, new ConfigurationParameter(OverrideID.SetPcTime)
        {
          HasWritePermission = true,
          ParameterValue = (object) false
        });
        if (flag2 | flag1 | flag3)
          r.Add(OverrideID.ClearManipulation, new ConfigurationParameter(OverrideID.ClearManipulation)
          {
            HasWritePermission = true,
            ParameterValue = (object) false
          });
        DeviceIdentification deviceIdentification = meter.GetDeviceIdentification();
        if (deviceIdentification != null)
          r.Add(OverrideID.MeterID, new ConfigurationParameter(OverrideID.MeterID, (object) deviceIdentification.MeterID));
        r.Add(OverrideID.ListType, new ConfigurationParameter(OverrideID.ListType, (object) meter.GetMBusListType())
        {
          HasWritePermission = flag3
        });
        if (version.Type == EDC_Hardware.EDC_Radio)
        {
          r.Add(OverrideID.RadioMode, new ConfigurationParameter(OverrideID.RadioMode, (object) meter.GetRadioMode()));
          r.Add(OverrideID.RadioSendInterval, new ConfigurationParameter(OverrideID.RadioSendInterval, (object) meter.GetRadioTransmitInterval()));
          r.Add(OverrideID.AESKey, new ConfigurationParameter(OverrideID.AESKey, (object) AES.AesKeyToString(meter.GetAESkey()))
          {
            HasWritePermission = flag2 | flag1 | flag3
          });
          r.Add(OverrideID.LongHeader, new ConfigurationParameter(OverrideID.LongHeader, (object) meter.GetWMBusLongHeaderState())
          {
            HasWritePermission = true
          });
          r.Add(OverrideID.RadioEnabled, new ConfigurationParameter(OverrideID.RadioEnabled, (object) meter.GetRadioState())
          {
            HasWritePermission = true
          });
          int int32 = Convert.ToInt32(meter.GetVolumeAccumulatedNegativ());
          r.Add(OverrideID.TotalVolumePulsesNegativ, new ConfigurationParameter(OverrideID.TotalVolumePulsesNegativ, (object) int32));
          if (flag2 | flag1 | flag3)
            r.Add(OverrideID.Encryption, new ConfigurationParameter(OverrideID.Encryption, (object) meter.GetWMBusEncryptionState())
            {
              HasWritePermission = true
            });
        }
      }
      if (this.newParameters != null && this.newParameters.Count > 0)
      {
        foreach (KeyValuePair<OverrideID, ConfigurationParameter> newParameter in this.newParameters)
          r[newParameter.Key] = newParameter.Value;
      }
      return r;
    }

    private static void AddParam(
      bool canChanged,
      SortedList<OverrideID, ConfigurationParameter> r,
      OverrideID overrideID,
      object obj)
    {
      EdcHandler.AddParam(canChanged, r, overrideID, obj, false, (string[]) null);
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
        AllowedValues = allowedValues,
        IsFunction = isFunction
      });
    }

    private static void AddParam(
      bool canChanged,
      SortedList<OverrideID, ConfigurationParameter> r,
      OverrideID overrideID,
      ConfigurationParameter p)
    {
      if (!UserManager.IsConfigParamVisible(overrideID))
        return;
      if (canChanged)
        p.HasWritePermission = UserManager.IsConfigParamEditable(overrideID);
      r.Add(overrideID, p);
    }

    public override bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameterList,
      int SubDevice)
    {
      if (parameterList == null || this.edc == null || this.edc.Meter == null || this.edc.Meter.Version == null)
        return false;
      SortedList<OverrideID, ConfigurationParameter> sortedList = new SortedList<OverrideID, ConfigurationParameter>();
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> parameter in parameterList)
      {
        if (parameter.Value.HasWritePermission && parameter.Value.ParameterValue != null)
        {
          EDC_Meter meter = this.edc.Meter;
          switch (parameter.Key)
          {
            case OverrideID.MBusAddress:
              if (!meter.SetMBusAddressSecondary(Convert.ToByte(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.Baudrate:
              if (!meter.SetMbusBaud((MbusBaud) Enum.Parse(typeof (MbusBaud), parameter.Value.ParameterValue.ToString(), true)))
                return false;
              break;
            case OverrideID.Medium:
              if (!meter.SetMediumPrimary((MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), parameter.Value.ParameterValue.ToString(), true)))
                return false;
              break;
            case OverrideID.DueDate:
              if (!meter.SetDueDate(Convert.ToDateTime(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.Manipulation:
              if (!meter.SetMagnetDetectionState(Convert.ToBoolean(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.RadioEnabled:
              if (!meter.SetRadioState(Convert.ToBoolean(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.TimeZone:
              if (!meter.SetTimeZone(Convert.ToInt32(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.PulseMultiplier:
              if (!meter.SetPulseMultiplier(Convert.ToByte(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.RegisterDigits:
              if (!meter.SetCogCount(Convert.ToByte(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.AESKey:
              if (!meter.SetAESkey(parameter.Value.ParameterValue))
                return false;
              break;
            case OverrideID.PulseBlockLimit:
              if (!meter.SetPulseBlockLimit(Convert.ToUInt16(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.PulseLeakLimit:
              if (!meter.SetPulseLeakLimit(Convert.ToUInt16(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.PulseUnleakLimit:
              if (!meter.SetPulseUnleakLimit(Convert.ToUInt16(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.PulseLeakLower:
              if (!meter.SetPulseLeakLower(Convert.ToInt16(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.PulseLeakUpper:
              if (!meter.SetPulseLeakUpper(Convert.ToInt16(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.PulseBackLimit:
              if (!meter.SetPulseBackLimit(Convert.ToUInt16(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.PulseUnbackLimit:
              if (!meter.SetPulseUnbackLimit(Convert.ToUInt16(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.OversizeDiff:
              if (!meter.SetOversizeDiff(Convert.ToUInt16(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.OversizeLimit:
              if (!meter.SetOversizeLimit(Convert.ToUInt16(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.UndersizeDiff:
              if (!meter.SetUndersizeDiff(Convert.ToUInt16(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.UndersizeLimit:
              if (!meter.SetUndersizeLimit(Convert.ToUInt16(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.BurstDiff:
              if (!meter.SetBurstDiff(Convert.ToUInt16(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.BurstLimit:
              if (!meter.SetBurstLimit(Convert.ToUInt16(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.SerialNumberSecondary:
              if (!meter.SetSerialnumberSecondary(Convert.ToUInt32(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.LongHeader:
              if (!meter.SetWMBusLongHeaderState(Convert.ToBoolean(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.MediumSecondary:
              if (!meter.SetMediumSecondary((MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), parameter.Value.ParameterValue.ToString(), true)))
                return false;
              break;
            case OverrideID.Encryption:
              if (!meter.SetWMBusEncryptionState(Convert.ToBoolean(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.ManufacturerSecondary:
              if (!meter.SetManufacturerSecondary(parameter.Value.ParameterValue.ToString()))
                return false;
              break;
            case OverrideID.PulseEnabled:
              if (!meter.SetCoilSampling(Convert.ToBoolean(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.PulseoutMode:
              if (!meter.SetPulseoutMode((PulseoutMode) Enum.Parse(typeof (PulseoutMode), parameter.Value.ParameterValue.ToString())))
                return false;
              break;
            case OverrideID.PulseoutWidth:
              ushort num = (ushort) (Convert.ToDouble(parameter.Value.ParameterValue) / 2000.0 * 32768.0);
              if (!meter.SetPulseoutWidth(num))
                return false;
              break;
            case OverrideID.PulseoutResolution:
              if (!meter.SetPulseoutPPP(Convert.ToInt16(parameter.Value.ParameterValue)))
                return false;
              break;
            case OverrideID.NominalFlow:
              meter.SetNominalFlow(parameter.Value.ParameterValue.ToString());
              break;
            default:
              sortedList.Add(parameter.Key, parameter.Value);
              break;
          }
        }
      }
      if (this.newParameters == null)
      {
        this.newParameters = sortedList;
      }
      else
      {
        foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in sortedList)
        {
          if (this.newParameters.ContainsKey(keyValuePair.Key))
            this.newParameters[keyValuePair.Key] = keyValuePair.Value;
          else
            this.newParameters.Add(keyValuePair.Key, keyValuePair.Value);
        }
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
      if (this.newParameters == null || this.edc == null)
        return false;
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      try
      {
        EDC_Meter meter = this.edc.Meter;
        if (meter == null || meter.Version == null)
          return false;
        foreach (KeyValuePair<OverrideID, ConfigurationParameter> newParameter in this.newParameters)
        {
          if (newParameter.Value.HasWritePermission && newParameter.Value.ParameterValue != null)
          {
            switch (newParameter.Key)
            {
              case OverrideID.TotalVolumePulses:
                if (!this.edc.WriteMeterValue(Convert.ToUInt32(newParameter.Value.ParameterValue)))
                  return false;
                break;
              case OverrideID.ClearAllLoggers:
                if (Convert.ToBoolean(newParameter.Value.ParameterValue) && (!this.edc.LogClearAndDisableLog() || !this.edc.LogEnable() && !this.edc.LogEnable() && !this.edc.LogEnable()))
                  return false;
                continue;
              case OverrideID.SetPcTime:
                if (Convert.ToBoolean(newParameter.Value.ParameterValue) && !this.edc.WriteSystemTime(DateTime.Now))
                  return false;
                continue;
              case OverrideID.ClearWarnings:
                if (Convert.ToBoolean(newParameter.Value.ParameterValue))
                {
                  if (this.edc.Meter.Version.Type == EDC_Hardware.EDC_Radio)
                  {
                    if (!this.edc.RemovalFlagClear() || !this.edc.TamperFlagClear() || !this.edc.BackflowFlagClear() || !this.edc.LeakFlagClear() || !this.edc.BlockFlagClear() || !this.edc.OversizeFlagClear() || !this.edc.UndersizeFlagClear() || !this.edc.BurstFlagClear())
                      return false;
                    break;
                  }
                  if (this.edc.Meter.Version.Type == EDC_Hardware.EDC_mBus)
                  {
                    Warning? nullable = this.edc.Meter.GetWarnings();
                    nullable = nullable.HasValue ? new Warning?(nullable.GetValueOrDefault() & (Warning.APP_BUSY | Warning.ABNORMAL | Warning.PERMANENT_ERROR | Warning.TEMPORARY_ERROR | Warning.TAMPER_A | Warning.REMOVAL_A | Warning.LEAK | Warning.LEAK_A | Warning.UNDERSIZE | Warning.BLOCK_A | Warning.BACKFLOW | Warning.BACKFLOW_A | Warning.INTERFERE | Warning.OVERSIZE | Warning.BURST)) : new Warning?();
                    nullable = nullable.HasValue ? new Warning?(nullable.GetValueOrDefault() & (Warning.APP_BUSY | Warning.BATT_LOW | Warning.PERMANENT_ERROR | Warning.TEMPORARY_ERROR | Warning.TAMPER_A | Warning.REMOVAL_A | Warning.LEAK | Warning.LEAK_A | Warning.UNDERSIZE | Warning.BLOCK_A | Warning.BACKFLOW | Warning.BACKFLOW_A | Warning.INTERFERE | Warning.OVERSIZE | Warning.BURST)) : new Warning?();
                    nullable = nullable.HasValue ? new Warning?(nullable.GetValueOrDefault() & (Warning.APP_BUSY | Warning.ABNORMAL | Warning.BATT_LOW | Warning.PERMANENT_ERROR | Warning.TEMPORARY_ERROR | Warning.TAMPER_A | Warning.REMOVAL_A | Warning.LEAK | Warning.LEAK_A | Warning.UNDERSIZE | Warning.BLOCK_A | Warning.BACKFLOW | Warning.BACKFLOW_A | Warning.OVERSIZE | Warning.BURST)) : new Warning?();
                    nullable = nullable.HasValue ? new Warning?(nullable.GetValueOrDefault() & (Warning.APP_BUSY | Warning.ABNORMAL | Warning.BATT_LOW | Warning.PERMANENT_ERROR | Warning.TEMPORARY_ERROR | Warning.TAMPER_A | Warning.REMOVAL_A | Warning.LEAK_A | Warning.UNDERSIZE | Warning.BLOCK_A | Warning.BACKFLOW | Warning.BACKFLOW_A | Warning.INTERFERE | Warning.OVERSIZE | Warning.BURST)) : new Warning?();
                    nullable = nullable.HasValue ? new Warning?(nullable.GetValueOrDefault() & (Warning.APP_BUSY | Warning.ABNORMAL | Warning.BATT_LOW | Warning.PERMANENT_ERROR | Warning.TEMPORARY_ERROR | Warning.TAMPER_A | Warning.REMOVAL_A | Warning.LEAK | Warning.UNDERSIZE | Warning.BLOCK_A | Warning.BACKFLOW | Warning.BACKFLOW_A | Warning.INTERFERE | Warning.OVERSIZE | Warning.BURST)) : new Warning?();
                    nullable = nullable.HasValue ? new Warning?(nullable.GetValueOrDefault() & (Warning.APP_BUSY | Warning.ABNORMAL | Warning.BATT_LOW | Warning.PERMANENT_ERROR | Warning.TEMPORARY_ERROR | Warning.TAMPER_A | Warning.REMOVAL_A | Warning.LEAK | Warning.LEAK_A | Warning.UNDERSIZE | Warning.BLOCK_A | Warning.BACKFLOW_A | Warning.INTERFERE | Warning.OVERSIZE | Warning.BURST)) : new Warning?();
                    nullable = nullable.HasValue ? new Warning?(nullable.GetValueOrDefault() & (Warning.APP_BUSY | Warning.ABNORMAL | Warning.BATT_LOW | Warning.PERMANENT_ERROR | Warning.TEMPORARY_ERROR | Warning.TAMPER_A | Warning.REMOVAL_A | Warning.LEAK | Warning.LEAK_A | Warning.UNDERSIZE | Warning.BLOCK_A | Warning.BACKFLOW | Warning.INTERFERE | Warning.OVERSIZE | Warning.BURST)) : new Warning?();
                    nullable = nullable.HasValue ? new Warning?(nullable.GetValueOrDefault() & (Warning.APP_BUSY | Warning.ABNORMAL | Warning.BATT_LOW | Warning.PERMANENT_ERROR | Warning.TEMPORARY_ERROR | Warning.TAMPER_A | Warning.REMOVAL_A | Warning.LEAK | Warning.LEAK_A | Warning.BLOCK_A | Warning.BACKFLOW | Warning.BACKFLOW_A | Warning.INTERFERE | Warning.OVERSIZE | Warning.BURST)) : new Warning?();
                    nullable = nullable.HasValue ? new Warning?(nullable.GetValueOrDefault() & (Warning.APP_BUSY | Warning.ABNORMAL | Warning.BATT_LOW | Warning.PERMANENT_ERROR | Warning.TEMPORARY_ERROR | Warning.TAMPER_A | Warning.REMOVAL_A | Warning.LEAK | Warning.LEAK_A | Warning.UNDERSIZE | Warning.BLOCK_A | Warning.BACKFLOW | Warning.BACKFLOW_A | Warning.INTERFERE | Warning.BURST)) : new Warning?();
                    nullable = nullable.HasValue ? new Warning?(nullable.GetValueOrDefault() & (Warning.APP_BUSY | Warning.ABNORMAL | Warning.BATT_LOW | Warning.PERMANENT_ERROR | Warning.TEMPORARY_ERROR | Warning.TAMPER_A | Warning.REMOVAL_A | Warning.LEAK | Warning.LEAK_A | Warning.UNDERSIZE | Warning.BLOCK_A | Warning.BACKFLOW | Warning.BACKFLOW_A | Warning.INTERFERE | Warning.OVERSIZE)) : new Warning?();
                    nullable = nullable.HasValue ? new Warning?(nullable.GetValueOrDefault() & (Warning.APP_BUSY | Warning.ABNORMAL | Warning.BATT_LOW | Warning.PERMANENT_ERROR | Warning.TAMPER_A | Warning.REMOVAL_A | Warning.LEAK | Warning.LEAK_A | Warning.UNDERSIZE | Warning.BLOCK_A | Warning.BACKFLOW | Warning.BACKFLOW_A | Warning.INTERFERE | Warning.OVERSIZE | Warning.BURST)) : new Warning?();
                    nullable = nullable.HasValue ? new Warning?(nullable.GetValueOrDefault() & (Warning.APP_BUSY | Warning.ABNORMAL | Warning.BATT_LOW | Warning.PERMANENT_ERROR | Warning.TEMPORARY_ERROR | Warning.REMOVAL_A | Warning.LEAK | Warning.LEAK_A | Warning.UNDERSIZE | Warning.BLOCK_A | Warning.BACKFLOW | Warning.BACKFLOW_A | Warning.INTERFERE | Warning.OVERSIZE | Warning.BURST)) : new Warning?();
                    if (!this.edc.Meter.SetWarnings((nullable.HasValue ? new Warning?(nullable.GetValueOrDefault() & (Warning.APP_BUSY | Warning.ABNORMAL | Warning.BATT_LOW | Warning.PERMANENT_ERROR | Warning.TEMPORARY_ERROR | Warning.TAMPER_A | Warning.LEAK | Warning.LEAK_A | Warning.UNDERSIZE | Warning.BLOCK_A | Warning.BACKFLOW | Warning.BACKFLOW_A | Warning.INTERFERE | Warning.OVERSIZE | Warning.BURST)) : new Warning?()).Value))
                      return false;
                    break;
                  }
                  break;
                }
                continue;
              case OverrideID.ClearManipulation:
                if (Convert.ToBoolean(newParameter.Value.ParameterValue) && !meter.SetMagnetDetectionState(false))
                  return false;
                continue;
              default:
                throw new ArgumentException("Ignored OverrideID found: " + newParameter.ToString());
            }
          }
        }
        if (!this.edc.WriteDevice())
          return false;
        this.newParameters = (SortedList<OverrideID, ConfigurationParameter>) null;
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Failed to write device! " + ex.Message);
        return false;
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
      if (!this.MyDeviceManager.IsValueIdentSetReceivedEventEnabled || this.edc.Meter == null || this.edc.Meter.Version == null)
        return;
      uint? serialnumberSecondary = this.edc.Meter.GetSerialnumberSecondary();
      ValueIdentSet e = new ValueIdentSet();
      e.Manufacturer = this.edc.Meter.GetManufacturerSecondary();
      e.Version = this.edc.Meter.Version.VersionString;
      MBusDeviceType? mediumSecondary = this.edc.Meter.GetMediumSecondary();
      e.DeviceType = !mediumSecondary.HasValue ? MBusDeviceType.WATER.ToString() : mediumSecondary.Value.ToString();
      if (serialnumberSecondary.HasValue)
        e.SerialNumber = serialnumberSecondary.ToString();
      e.ZDF = "SID;" + e.SerialNumber + ";MAN;" + e.Manufacturer + ";MED;" + e.DeviceType;
      SortedList<long, SortedList<DateTime, ReadingValue>> ValueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      if (this.GetValues(ref ValueList))
        e.AvailableValues = ValueList;
      this.MyDeviceManager.OnValueIdentSetReceived((object) this, e);
    }
  }
}
