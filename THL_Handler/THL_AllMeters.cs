// Decompiled with JetBrains decompiler
// Type: THL_Handler.THL_AllMeters
// Assembly: THL_Handler, Version=1.0.5.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: C9669406-A704-45DE-B726-D8A41F27FFB8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\THL_Handler.dll

using GmmDbLib;
using HandlerLib;
using MBusLib;
using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace THL_Handler
{
  internal class THL_AllMeters : ICreateMeter, IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (THL_AllMeters));
    private THL_HandlerFunctions functions;
    private int? Perform_GetCommunicationScenario = new int?();
    private int? Perform_SetCommunicationScenario = new int?();
    private bool Perform_SetPcTime = false;
    private bool Perform_SendJoinRequest = false;
    private bool Perform_SetOperatingMode = false;
    internal THL_Meter ConnectedMeter;
    internal THL_Meter TypeMeter;
    internal THL_Meter BackupMeter;
    internal THL_Meter WorkMeter;

    internal THL_AllMeters()
    {
    }

    internal THL_AllMeters(THL_HandlerFunctions functions) => this.functions = functions;

    public void Dispose()
    {
      this.ConnectedMeter = (THL_Meter) null;
      this.WorkMeter = (THL_Meter) null;
      this.TypeMeter = (THL_Meter) null;
      this.BackupMeter = (THL_Meter) null;
    }

    public IMeter CreateMeter(byte[] zippedBuffer) => new THL_Meter().CreateFromData(zippedBuffer);

    public string SetBackupMeter(byte[] zippedBuffer)
    {
      this.BackupMeter = this.CreateMeter(zippedBuffer) as THL_Meter;
      if (this.WorkMeter == null)
        this.WorkMeter = this.BackupMeter.Clone();
      return this.BackupMeter.GetInfo();
    }

    public async Task ConnectDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      DeviceVersionMBus deviceVersionMbus = await this.functions.cmd.ReadVersionAsync(progress, token);
      DeviceIdentification deviceVersion = (DeviceIdentification) deviceVersionMbus;
      deviceVersionMbus = (DeviceVersionMBus) null;
      this.ConnectedMeter = new THL_Meter(deviceVersion);
      this.WorkMeter = this.ConnectedMeter.Clone();
      this.Perform_SetPcTime = false;
      this.Perform_SendJoinRequest = false;
      this.Perform_SetOperatingMode = false;
      this.Perform_SetCommunicationScenario = new int?();
      deviceVersion = (DeviceIdentification) null;
    }

    public async Task ReadDeviceAsync(
      ProgressHandler progress,
      CancellationToken token,
      ReadPartsSelection readPartsSelection)
    {
      bool isDump = readPartsSelection == ReadPartsSelection.Dump;
      bool includeLogger = readPartsSelection == ReadPartsSelection.All;
      progress.Split(new double[3]{ 10.0, 10.0, 80.0 });
      DeviceVersionMBus deviceVersionMbus = await this.functions.cmd.ReadVersionAsync(progress, token);
      DeviceIdentification deviceIdentification = (DeviceIdentification) deviceVersionMbus;
      deviceVersionMbus = (DeviceVersionMBus) null;
      FirmwareType type = (FirmwareType) new FirmwareVersion(deviceIdentification.FirmwareVersion.Value).Type;
      if (type != FirmwareType.TH_LoRa && type != FirmwareType.TH_sensor_wMBus && type != FirmwareType.TH_LoRa_wMBus)
        throw new Exception(Ot.Gtt(Tg.Common, "NotTHL", "Connected device is not TH LoRa."));
      this.ConnectedMeter = new THL_Meter(deviceIdentification);
      if (isDump || (readPartsSelection & ReadPartsSelection.EnhancedIdentification) > ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode))
      {
        try
        {
          await this.functions.cmd.Device.ReadMemoryAsync(this.ConnectedMeter.meterMemory.ArmIdRange, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, token);
        }
        catch
        {
        }
      }
      FirmwareVersion firmwareVersionObj = deviceIdentification.FirmwareVersionObj;
      int major = (int) firmwareVersionObj.Major;
      firmwareVersionObj = deviceIdentification.FirmwareVersionObj;
      int minor = (int) firmwareVersionObj.Minor;
      firmwareVersionObj = deviceIdentification.FirmwareVersionObj;
      int revision = (int) firmwareVersionObj.Revision;
      Version version = new Version(major, minor, revision);
      if (version >= Version.Parse("0.23.3"))
      {
        Common32BitCommands.Scenarios scenarios = await this.functions.cmd.Device.GetCommunicationScenarioAsync(progress, token);
        ushort? scenarioOne = scenarios.ScenarioOne;
        this.Perform_GetCommunicationScenario = scenarioOne.HasValue ? new int?((int) scenarioOne.GetValueOrDefault()) : new int?();
        scenarios = (Common32BitCommands.Scenarios) null;
      }
      List<AddressRange> ranges = this.ConnectedMeter.meterMemory.GetRangesForRead(includeLogger, isDump);
      progress.Split(ranges.Count);
      foreach (AddressRange range in ranges)
        await this.functions.cmd.Device.ReadMemoryAsync(range, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, token);
      this.WorkMeter = this.ConnectedMeter.Clone();
      this.Perform_SetPcTime = false;
      this.Perform_SendJoinRequest = false;
      this.Perform_SetOperatingMode = false;
      this.Perform_SetCommunicationScenario = new int?();
      deviceIdentification = (DeviceIdentification) null;
      version = (Version) null;
      ranges = (List<AddressRange>) null;
    }

    public void SetCommunicationScenarioParameter(int SAP_MaterialNumber)
    {
      switch (SAP_MaterialNumber)
      {
        case 178215:
          this.Perform_SetCommunicationScenario = new int?(204);
          if (this.Perform_GetCommunicationScenario.HasValue)
            break;
          this.Perform_GetCommunicationScenario = new int?(0);
          break;
        case 179145:
          this.Perform_SetCommunicationScenario = new int?(331);
          if (!this.Perform_GetCommunicationScenario.HasValue)
            this.Perform_GetCommunicationScenario = new int?(0);
          break;
      }
    }

    public async Task WriteDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      if (this.ConnectedMeter == null)
        throw new ArgumentNullException("ConnectedMeter");
      if (this.WorkMeter == null)
        throw new ArgumentNullException("WorkMeter");
      List<AddressRange> ranges = this.WorkMeter.meterMemory.GetChangedDataRanges((DeviceMemory) this.ConnectedMeter.meterMemory);
      if (ranges != null && ranges.Count > 0)
      {
        progress.Split(ranges.Count + 1);
        foreach (AddressRange range in ranges)
        {
          this.WorkMeter.meterMemory.GarantMemoryAvailable(range);
          await this.functions.cmd.Device.WriteMemoryAsync(range, (DeviceMemory) this.WorkMeter.meterMemory, progress, token);
        }
        await this.functions.cmd.Device.ResetDeviceAsync(progress, token);
        this.functions.Clear();
      }
      DeviceVersionMBus deviceVersionMbus = await this.functions.cmd.ReadVersionAsync(progress, token);
      bool doReset = false;
      if (this.Perform_SetPcTime)
      {
        double timezone = Convert.ToDouble(this.GetParameter(THL_Params.Bak_TimeZoneInQuarterHours));
        DateTime utc_plus_timezone = DateTime.UtcNow.AddHours(timezone);
        await this.functions.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(utc_plus_timezone, (sbyte) 0), progress, token);
        this.Perform_SetPcTime = false;
      }
      if (this.Perform_SendJoinRequest)
      {
        await this.functions.cmd.LoRa.SendJoinRequestAsync(progress, token);
        this.Perform_SendJoinRequest = false;
      }
      if (this.Perform_SetCommunicationScenario.HasValue && this.Perform_SetCommunicationScenario.Value != this.Perform_GetCommunicationScenario.Value)
      {
        byte[] scenario = BitConverter.GetBytes(Convert.ToUInt16(this.Perform_SetCommunicationScenario.Value));
        await this.functions.cmd.Device.SetCommunicationScenarioAsync(scenario, progress, token);
        this.Perform_SetCommunicationScenario = new int?();
        doReset = true;
        scenario = (byte[]) null;
      }
      if (this.Perform_SetOperatingMode)
      {
        await this.functions.cmd.Device.SetModeAsync((byte) 0, progress, token);
        this.Perform_SetOperatingMode = false;
      }
      if (!doReset)
      {
        ranges = (List<AddressRange>) null;
      }
      else
      {
        await this.functions.ResetDeviceAsync(progress, token);
        ranges = (List<AddressRange>) null;
      }
    }

    public void SetParameter(THL_Params p, object value)
    {
      if (value == null)
        return;
      switch (p)
      {
        case THL_Params.Bak_TimeZoneInQuarterHours:
          this.WorkMeter.meterMemory.SetParameterValue<sbyte>(THL_Params.Bak_TimeZoneInQuarterHours, Convert.ToSByte(Convert.ToDouble(value) * 4.0));
          break;
        case THL_Params.cfg_battery_end_life_date:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(THL_Params.cfg_battery_end_life_date, MBusUtil.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(Convert.ToDateTime(value)));
          break;
        case THL_Params.cfg_transmission_scenario:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(THL_Params.cfg_transmission_scenario, Convert.ToByte(value));
          break;
        case THL_Params.cfb_lora_AppKey:
        case THL_Params.cfb_lora_nwkskey:
        case THL_Params.cfb_lora_appskey:
        case THL_Params.cfg_lora_device_id:
        case THL_Params.Mbus_aes_key:
          this.WorkMeter.meterMemory.SetData(p, AES.StringToAesKey(value.ToString()));
          break;
        case THL_Params.cfg_otaa_abp_mode:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(THL_Params.cfg_otaa_abp_mode, value.ToString() == "OTAA" ? (byte) 1 : (byte) 2);
          break;
        case THL_Params.lora_adr:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(THL_Params.lora_adr, Convert.ToBoolean(value.ToString()) ? (byte) 1 : (byte) 0);
          break;
        case THL_Params.Mbus_interval:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(THL_Params.Mbus_interval, Convert.ToUInt16((int) Convert.ToUInt16(value) / 2));
          break;
        case THL_Params.FD_Mbus_interval:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(THL_Params.FD_Mbus_interval, Convert.ToUInt16((int) Convert.ToUInt16(value) / 2));
          break;
        case THL_Params.FD_Mbus_nighttime_start:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(THL_Params.FD_Mbus_nighttime_start, Convert.ToByte(value));
          break;
        case THL_Params.FD_Mbus_nighttime_stop:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(THL_Params.FD_Mbus_nighttime_stop, Convert.ToByte(value));
          break;
        case THL_Params.FD_Mbus_radio_suppression_days:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(THL_Params.FD_Mbus_radio_suppression_days, Convert.ToByte(value));
          break;
        default:
          throw new NotImplementedException(p.ToString());
      }
    }

    public object GetParameter(THL_Params p)
    {
      switch (p)
      {
        case THL_Params.MeterId:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<uint>(THL_Params.MeterId);
        case THL_Params.Bak_TimeZoneInQuarterHours:
          return (object) ((double) this.WorkMeter.meterMemory.GetParameterValue<sbyte>(THL_Params.Bak_TimeZoneInQuarterHours) / 4.0);
        case THL_Params.cfg_battery_end_life_date:
          return (object) Util.ConvertToDate_MBus_CP16_TypeG(BitConverter.GetBytes(this.WorkMeter.meterMemory.GetParameterValue<ushort>(THL_Params.cfg_battery_end_life_date)), 0);
        case THL_Params.cfg_transmission_scenario:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.cfg_transmission_scenario);
        case THL_Params.cfb_lora_AppKey:
        case THL_Params.cfb_lora_nwkskey:
        case THL_Params.cfb_lora_appskey:
        case THL_Params.cfg_lora_device_id:
        case THL_Params.Mbus_aes_key:
          return (object) AES.AesKeyToString(this.WorkMeter.meterMemory.GetData(p));
        case THL_Params.cfg_otaa_abp_mode:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.cfg_otaa_abp_mode))
          {
            case 1:
              return (object) "OTAA";
            case 2:
              return (object) "ABP";
            default:
              return (object) string.Empty;
          }
        case THL_Params.cfg_loraWan_netid:
          return (object) Convert.ToUInt32(this.WorkMeter.meterMemory.GetParameterValue<uint>(THL_Params.cfg_loraWan_netid));
        case THL_Params.cfg_device_mode:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.cfg_device_mode))
          {
            case 0:
              return (object) "OPERATION_MODE";
            case 1:
              return (object) "DELIVERY_MODE";
            default:
              throw new NotSupportedException("Unknown value for cfg_device_mode!");
          }
        case THL_Params.device_status:
          ushort parameterValue = this.WorkMeter.meterMemory.GetParameterValue<ushort>(THL_Params.device_status);
          return parameterValue == (ushort) 0 ? (object) string.Empty : (object) ((DeviceStatus) parameterValue).ToString();
        case THL_Params.lora_adr:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.lora_adr))
          {
            case 0:
              return (object) false;
            case 1:
              return (object) true;
            default:
              throw new NotSupportedException("Unknown value for lora_adr!");
          }
        case THL_Params.Mbus_interval:
          return (object) ((int) this.WorkMeter.meterMemory.GetParameterValue<ushort>(THL_Params.Mbus_interval) * 2);
        case THL_Params.cfg_AccessRadioKey:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<uint>(THL_Params.cfg_AccessRadioKey);
        case THL_Params.rtc_sys_time:
          return (object) Utility.ConvertToDateTime_SystemTime64(this.WorkMeter.meterMemory.GetData(THL_Params.rtc_sys_time), 0);
        case THL_Params.radio_center_frequency:
          return (object) ((double) this.WorkMeter.meterMemory.GetParameterValue<uint>(THL_Params.radio_center_frequency) / 1000000.0);
        case THL_Params.FD_Mbus_interval:
          return (object) ((int) this.WorkMeter.meterMemory.GetParameterValue<ushort>(THL_Params.FD_Mbus_interval) * 2);
        case THL_Params.Mbus_nighttime_start:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.FD_Mbus_nighttime_start);
        case THL_Params.Mbus_nighttime_stop:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.Mbus_nighttime_stop);
        case THL_Params.Mbus_radio_suppression_days:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.Mbus_radio_suppression_days);
        case THL_Params.cfg_RadioOperationState:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.cfg_RadioOperationState))
          {
            case 0:
              return (object) false;
            case 1:
              return (object) true;
            default:
              throw new NotSupportedException("Unknown value for RadioOperation!");
          }
        default:
          throw new NotImplementedException(p.ToString());
      }
    }

    internal SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters()
    {
      if (this.WorkMeter == null)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      THL_DeviceMemory meterMemory = this.WorkMeter.meterMemory;
      UserManager.CheckPermission("Role\\Developer");
      SortedList<OverrideID, ConfigurationParameter> r1 = new SortedList<OverrideID, ConfigurationParameter>();
      FirmwareVersion firmwareVersionObj = this.WorkMeter.deviceIdentification.FirmwareVersionObj;
      bool flag1 = firmwareVersionObj.Type == (ushort) 40;
      firmwareVersionObj = this.WorkMeter.deviceIdentification.FirmwareVersionObj;
      bool flag2 = firmwareVersionObj.Type == (ushort) 45;
      firmwareVersionObj = this.WorkMeter.deviceIdentification.FirmwareVersionObj;
      int major = (int) firmwareVersionObj.Major;
      firmwareVersionObj = this.WorkMeter.deviceIdentification.FirmwareVersionObj;
      int minor = (int) firmwareVersionObj.Minor;
      firmwareVersionObj = this.WorkMeter.deviceIdentification.FirmwareVersionObj;
      int revision = (int) firmwareVersionObj.Revision;
      if (new Version(major, minor, revision) >= Version.Parse("0.23.3") && this.Perform_GetCommunicationScenario.HasValue)
      {
        int num = this.Perform_GetCommunicationScenario.Value;
        flag1 = num >= 200 && num < 300;
        flag2 = num >= 300 && num < 400;
        if (this.Perform_SetCommunicationScenario.HasValue)
          THL_AllMeters.AddParam(true, r1, OverrideID.CommunicationScenario, (object) this.Perform_SetCommunicationScenario);
        else
          THL_AllMeters.AddParam(true, r1, OverrideID.CommunicationScenario, (object) this.Perform_GetCommunicationScenario);
      }
      if (flag2)
      {
        string str = string.Empty;
        try
        {
          str = this.WorkMeter.deviceIdentification.FullSerialNumber;
        }
        catch
        {
        }
        THL_AllMeters.AddParam(false, r1, OverrideID.RadioTechnology, (object) RadioTechnology.wMBus);
        THL_AllMeters.AddParam(false, r1, OverrideID.SerialNumber, (object) this.WorkMeter.deviceIdentification.ID_BCD_AsString);
        THL_AllMeters.AddParam(false, r1, OverrideID.SerialNumberFull, (object) str);
        THL_AllMeters.AddParam(false, r1, OverrideID.Manufacturer, MBusUtil.GetManufacturer(this.WorkMeter.deviceIdentification.Manufacturer));
        THL_AllMeters.AddParam(false, r1, OverrideID.MBusGeneration, (object) this.WorkMeter.deviceIdentification.Generation);
        THL_AllMeters.AddParam(false, r1, OverrideID.Medium, (object) (MBusDeviceType) this.WorkMeter.deviceIdentification.Medium.Value);
        if (this.WorkMeter.meterMemory.IsParameterAvailable(THL_Params.Mbus_interval))
          THL_AllMeters.AddParam(true, r1, OverrideID.RadioSendInterval, this.GetParameter(THL_Params.Mbus_interval));
        if (this.WorkMeter.meterMemory.IsParameterAvailable(THL_Params.Mbus_aes_key))
          THL_AllMeters.AddParam(true, r1, OverrideID.AESKey, this.GetParameter(THL_Params.Mbus_aes_key));
        if (this.WorkMeter.meterMemory.IsParameterAvailable(THL_Params.Mbus_radio_suppression_days))
        {
          byte parameterValue1 = this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.Mbus_radio_suppression_days);
          byte num = this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.Mbus_nighttime_start);
          byte parameterValue2 = this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.Mbus_nighttime_stop);
          if (num == (byte) 0)
            num = (byte) 24;
          THL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveMonday, (object) !Convert.ToBoolean((int) parameterValue1 & 1));
          THL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveTuesday, (object) !Convert.ToBoolean((int) parameterValue1 & 2));
          THL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveWednesday, (object) !Convert.ToBoolean((int) parameterValue1 & 4));
          THL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveThursday, (object) !Convert.ToBoolean((int) parameterValue1 & 8));
          THL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveFriday, (object) !Convert.ToBoolean((int) parameterValue1 & 16));
          THL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveSaturday, (object) !Convert.ToBoolean((int) parameterValue1 & 32));
          THL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveSunday, (object) !Convert.ToBoolean((int) parameterValue1 & 64));
          THL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveStartTime, (object) parameterValue2);
          THL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveStopTime, (object) num);
        }
      }
      if (flag1)
      {
        THL_AllMeters.AddParam(false, r1, OverrideID.RadioTechnology, (object) RadioTechnology.LoRa);
        THL_AllMeters.AddParam(false, r1, OverrideID.LoRaWanVersion, (object) string.Format("{0}.{1}.{2}", (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.protocol_lorawan_version_major), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.protocol_lorawan_version_middle), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.protocol_lorawan_version_minor)));
        THL_AllMeters.AddParam(false, r1, OverrideID.LoRaVersion, (object) string.Format("{0}.{1}", (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.protocol_lora_version_major), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.protocol_lora_version_minor)));
        THL_AllMeters.AddParam(false, r1, OverrideID.NetID, (object) meterMemory.GetParameterValue<uint>(THL_Params.cfg_loraWan_netid));
        THL_AllMeters.AddParam(true, r1, OverrideID.JoinEUI, (object) this.WorkMeter.deviceIdentification.LoRa_JoinEUI);
        THL_AllMeters.AddParam(true, r1, OverrideID.DevEUI, (object) this.WorkMeter.deviceIdentification.LoRa_DevEUI);
        THL_AllMeters.AddParam(true, r1, OverrideID.AppKey, (object) this.WorkMeter.deviceIdentification.LoRa_AppKey);
        THL_AllMeters.AddParam(true, r1, OverrideID.NwkSKey, (object) meterMemory.GetData(THL_Params.cfb_lora_nwkskey));
        THL_AllMeters.AddParam(true, r1, OverrideID.AppSKey, (object) meterMemory.GetData(THL_Params.cfb_lora_appskey));
        THL_AllMeters.AddParam(true, r1, OverrideID.DevAddr, (object) meterMemory.GetParameterValue<uint>(THL_Params.cfg_lora_device_id));
        THL_AllMeters.AddParam(true, r1, OverrideID.Activation, this.GetParameter(THL_Params.cfg_otaa_abp_mode), false, new string[2]
        {
          "OTAA",
          "ABP"
        }, (string) null);
        THL_AllMeters.AddParam(true, r1, OverrideID.SendJoinRequest, (object) this.Perform_SendJoinRequest, true, (string[]) null, (string) null);
        THL_AllMeters.AddParam(true, r1, OverrideID.ADR, this.GetParameter(THL_Params.lora_adr));
      }
      THL_AllMeters.AddParam(false, r1, OverrideID.Frequence, this.GetParameter(THL_Params.radio_center_frequency), false, (string[]) null, "MHz");
      THL_AllMeters.AddParam(false, r1, OverrideID.PrintedSerialNumber, (object) this.WorkMeter.deviceIdentification.PrintedSerialNumberAsString);
      SortedList<OverrideID, ConfigurationParameter> r2 = r1;
      firmwareVersionObj = this.WorkMeter.deviceIdentification.FirmwareVersionObj;
      string str1 = firmwareVersionObj.ToString();
      THL_AllMeters.AddParam(false, r2, OverrideID.FirmwareVersion, (object) str1);
      THL_AllMeters.AddParam(false, r1, OverrideID.RadioVersion, (object) string.Format("{0}.{1}", (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.protocol_radio_version_major), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.protocol_radio_version_minor)));
      THL_AllMeters.AddParam(false, r1, OverrideID.WarningInfo, this.GetParameter(THL_Params.device_status));
      THL_AllMeters.AddParam(false, r1, OverrideID.DeviceMode, this.GetParameter(THL_Params.cfg_device_mode));
      THL_AllMeters.AddParam(true, r1, OverrideID.EndOfBatteryDate, this.GetParameter(THL_Params.cfg_battery_end_life_date));
      THL_AllMeters.AddParam(true, r1, OverrideID.SetPcTime, (object) this.Perform_SetPcTime, true, (string[]) null, (string) null);
      THL_AllMeters.AddParam(true, r1, OverrideID.SetOperatingMode, (object) this.Perform_SetOperatingMode, true, (string[]) null, (string) null);
      THL_AllMeters.AddParam(true, r1, OverrideID.TimeZone, (object) Convert.ToDecimal(this.GetParameter(THL_Params.Bak_TimeZoneInQuarterHours)));
      if (this.WorkMeter.meterMemory.IsParameterAvailable(THL_Params.rtc_sys_time))
        THL_AllMeters.AddParam(false, r1, OverrideID.DeviceClock, this.GetParameter(THL_Params.rtc_sys_time));
      return r1;
    }

    internal void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameter)
    {
      if (parameter == null || parameter.Count == 0)
        return;
      bool flag1 = this.ConnectedMeter.deviceIdentification.FirmwareVersionObj.Type == (ushort) 45;
      bool flag2 = this.ConnectedMeter.deviceIdentification.FirmwareVersionObj.Type == (ushort) 40;
      THL_DeviceMemory meterMemory = this.WorkMeter.meterMemory;
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in parameter)
      {
        if (keyValuePair.Key == OverrideID.CycleTimeRadio)
        {
          string stringValueWin = keyValuePair.Value.GetStringValueWin();
          switch (stringValueWin)
          {
            case "900":
              if (!flag1)
                throw new Exception("Can not set wMbus parameter to LoRa device! Check 'OverrideID.CycleTimeRadio' parameter or use w-MBus firmware.");
              this.WorkMeter.meterMemory.SetParameterValue<byte>(THL_Params.Mbus_radio_suppression_days, (byte) 0);
              this.WorkMeter.meterMemory.SetParameterValue<byte>(THL_Params.Mbus_nighttime_stop, (byte) 0);
              this.WorkMeter.meterMemory.SetParameterValue<byte>(THL_Params.Mbus_nighttime_start, (byte) 0);
              this.SetParameter(THL_Params.Mbus_interval, (object) 900);
              break;
            case "daily":
              if (!flag2)
                throw new Exception("Can not set LoRa parameter to w-MBus device! Check 'OverrideID.CycleTimeRadio' parameter or use LoRa firmware.");
              this.WorkMeter.meterMemory.SetParameterValue<byte>(THL_Params.cfg_transmission_scenario, (byte) 2);
              break;
            default:
              throw new NotImplementedException(stringValueWin);
          }
        }
        else if (keyValuePair.Key == OverrideID.TransmissionScenario)
          this.SetParameter(THL_Params.cfg_transmission_scenario, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.EndOfBatteryDate)
          this.SetParameter(THL_Params.cfg_battery_end_life_date, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.Activation)
          this.SetParameter(THL_Params.cfg_otaa_abp_mode, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.DevEUI)
          meterMemory.SetParameterValue<ulong>(THL_Params.cfg_lora_deveui, (ulong) keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.JoinEUI)
          meterMemory.SetParameterValue<ulong>(THL_Params.cfg_lora_appeui, (ulong) keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.AppKey)
          meterMemory.SetData(THL_Params.cfb_lora_AppKey, (byte[]) keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.NwkSKey)
          meterMemory.SetData(THL_Params.cfb_lora_nwkskey, (byte[]) keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.AppSKey)
          meterMemory.SetData(THL_Params.cfb_lora_appskey, (byte[]) keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.DevAddr)
          meterMemory.SetParameterValue<uint>(THL_Params.cfg_lora_device_id, (uint) keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.TimeZone)
          this.SetParameter(THL_Params.Bak_TimeZoneInQuarterHours, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.ADR)
          this.SetParameter(THL_Params.lora_adr, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.AESKey)
          this.SetParameter(THL_Params.Mbus_aes_key, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.RadioSendInterval)
          this.SetParameter(THL_Params.Mbus_interval, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.SetPcTime)
          this.Perform_SetPcTime = Convert.ToBoolean(keyValuePair.Value.ParameterValue.ToString());
        else if (keyValuePair.Key == OverrideID.SendJoinRequest)
          this.Perform_SendJoinRequest = Convert.ToBoolean(keyValuePair.Value.ParameterValue.ToString());
        else if (keyValuePair.Key == OverrideID.SetOperatingMode)
          this.Perform_SetOperatingMode = Convert.ToBoolean(keyValuePair.Value.ParameterValue.ToString());
        else if (keyValuePair.Key == OverrideID.RadioActiveMonday)
          this.SetMbus_radio_suppression_days((byte) 1, Convert.ToBoolean(keyValuePair.Value.ParameterValue));
        else if (keyValuePair.Key == OverrideID.RadioActiveTuesday)
          this.SetMbus_radio_suppression_days((byte) 2, Convert.ToBoolean(keyValuePair.Value.ParameterValue));
        else if (keyValuePair.Key == OverrideID.RadioActiveWednesday)
          this.SetMbus_radio_suppression_days((byte) 4, Convert.ToBoolean(keyValuePair.Value.ParameterValue));
        else if (keyValuePair.Key == OverrideID.RadioActiveThursday)
          this.SetMbus_radio_suppression_days((byte) 8, Convert.ToBoolean(keyValuePair.Value.ParameterValue));
        else if (keyValuePair.Key == OverrideID.RadioActiveFriday)
          this.SetMbus_radio_suppression_days((byte) 16, Convert.ToBoolean(keyValuePair.Value.ParameterValue));
        else if (keyValuePair.Key == OverrideID.RadioActiveSaturday)
          this.SetMbus_radio_suppression_days((byte) 32, Convert.ToBoolean(keyValuePair.Value.ParameterValue));
        else if (keyValuePair.Key == OverrideID.RadioActiveSunday)
          this.SetMbus_radio_suppression_days((byte) 64, Convert.ToBoolean(keyValuePair.Value.ParameterValue));
        else if (keyValuePair.Key == OverrideID.RadioActiveStartTime)
        {
          byte theValue = Convert.ToByte(keyValuePair.Value.ParameterValue);
          if (theValue < (byte) 0 || theValue > (byte) 23)
            throw new ArgumentOutOfRangeException("RadioActiveStartTime");
          this.WorkMeter.meterMemory.SetParameterValue<byte>(THL_Params.Mbus_nighttime_stop, theValue);
        }
        else if (keyValuePair.Key == OverrideID.RadioActiveStopTime)
        {
          byte theValue = Convert.ToByte(keyValuePair.Value.ParameterValue);
          if (theValue <= (byte) 1 || theValue > (byte) 24)
            throw new ArgumentOutOfRangeException("RadioActiveStopTime");
          if (theValue == (byte) 24)
            theValue = (byte) 0;
          this.WorkMeter.meterMemory.SetParameterValue<byte>(THL_Params.Mbus_nighttime_start, theValue);
        }
        else if (keyValuePair.Key == OverrideID.CommunicationScenario)
          this.Perform_SetCommunicationScenario = new int?((int) Convert.ToUInt16(keyValuePair.Value.ParameterValue));
      }
    }

    private void SetMbus_radio_suppression_days(byte mask, bool isSet)
    {
      byte parameterValue = this.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.Mbus_radio_suppression_days);
      this.WorkMeter.meterMemory.SetParameterValue<byte>(THL_Params.Mbus_radio_suppression_days, isSet ? (byte) ((uint) parameterValue & (uint) ~mask) : (byte) ((uint) parameterValue | (uint) mask));
    }

    internal SortedList<long, SortedList<DateTime, double>> GetValues()
    {
      return new SortedList<long, SortedList<DateTime, double>>();
    }

    internal void CompareConnectedAndWork()
    {
      this.WorkMeter.meterMemory.CompareParameterInfo("THL_DeviceMemory", "WorkMeter object", "ConnectedMeter object", (DeviceMemory) this.ConnectedMeter.meterMemory);
    }

    private static void AddParam(
      bool canChanged,
      SortedList<OverrideID, ConfigurationParameter> r,
      OverrideID overrideID,
      object obj)
    {
      THL_AllMeters.AddParam(canChanged, r, overrideID, obj, false, (string[]) null, (string) null);
    }

    private static void AddParam(
      bool canChanged,
      SortedList<OverrideID, ConfigurationParameter> r,
      OverrideID overrideID,
      object obj,
      bool isFunction,
      string[] allowedValues,
      string unit)
    {
      if (!UserManager.IsConfigParamVisible(overrideID) || (ConfigurationParameter.ActiveConfigurationLevel & ConfigurationParameter.ConfigParametersByOverrideID[overrideID].DefaultConfigurationLevels) == (ConfigurationLevel) 0)
        return;
      bool flag = false;
      if (canChanged)
        flag = UserManager.IsConfigParamEditable(overrideID);
      r.Add(overrideID, new ConfigurationParameter(overrideID, obj)
      {
        HasWritePermission = flag,
        AllowedValues = allowedValues,
        IsFunction = isFunction,
        Unit = unit
      });
    }

    internal SortedList<HandlerMeterObjects, DeviceMemory> GetAllMeterMemories()
    {
      SortedList<HandlerMeterObjects, DeviceMemory> allMeterMemories = new SortedList<HandlerMeterObjects, DeviceMemory>();
      if (this.WorkMeter != null)
        allMeterMemories.Add(HandlerMeterObjects.WorkMeter, (DeviceMemory) this.WorkMeter.meterMemory);
      if (this.ConnectedMeter != null)
        allMeterMemories.Add(HandlerMeterObjects.ConnectedMeter, (DeviceMemory) this.ConnectedMeter.meterMemory);
      if (this.TypeMeter != null)
        allMeterMemories.Add(HandlerMeterObjects.TypeMeter, (DeviceMemory) this.TypeMeter.meterMemory);
      if (this.BackupMeter != null)
        allMeterMemories.Add(HandlerMeterObjects.BackupMeter, (DeviceMemory) this.BackupMeter.meterMemory);
      return allMeterMemories;
    }
  }
}
