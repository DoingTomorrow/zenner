// Decompiled with JetBrains decompiler
// Type: M8_Handler.M8_AllMeters
// Assembly: M8_Handler, Version=2.0.6.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 582F1296-F274-42DF-B72B-4C0B4D92AA72
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\M8_Handler.dll

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
namespace M8_Handler
{
  internal class M8_AllMeters : ICreateMeter, IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (M8_AllMeters));
    private M8_HandlerFunctions functions;
    private Common32BitCommands.Scenarios Perform_GetCommunicationScenario = (Common32BitCommands.Scenarios) null;
    private Common32BitCommands.Scenarios Perform_SetCommunicationScenario = (Common32BitCommands.Scenarios) null;
    private bool Perform_SetPcTime = false;
    private bool Perform_SendJoinRequest = false;
    private bool Perform_SetOperatingMode = false;
    internal M8_Meter ConnectedMeter;
    internal M8_Meter TypeMeter;
    internal M8_Meter BackupMeter;
    internal M8_Meter WorkMeter;

    internal M8_AllMeters()
    {
    }

    internal M8_AllMeters(M8_HandlerFunctions functions) => this.functions = functions;

    public void Dispose()
    {
      this.ConnectedMeter = (M8_Meter) null;
      this.WorkMeter = (M8_Meter) null;
      this.TypeMeter = (M8_Meter) null;
      this.BackupMeter = (M8_Meter) null;
    }

    public IMeter CreateMeter(byte[] zippedBuffer) => new M8_Meter().CreateFromData(zippedBuffer);

    public string SetBackupMeter(byte[] zippedBuffer)
    {
      this.BackupMeter = this.CreateMeter(zippedBuffer) as M8_Meter;
      if (this.WorkMeter == null)
        this.WorkMeter = this.BackupMeter.Clone();
      return this.BackupMeter.GetInfo();
    }

    public async Task ConnectDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      DeviceVersionMBus deviceVersionMbus = await this.functions.cmd.ReadVersionAsync(progress, token);
      DeviceIdentification deviceVersion = (DeviceIdentification) deviceVersionMbus;
      deviceVersionMbus = (DeviceVersionMBus) null;
      this.ConnectedMeter = new M8_Meter(deviceVersion);
      this.WorkMeter = this.ConnectedMeter.Clone();
      this.Perform_SetPcTime = false;
      this.Perform_SendJoinRequest = false;
      this.Perform_SetOperatingMode = false;
      this.Perform_SetCommunicationScenario = (Common32BitCommands.Scenarios) null;
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
      if (type != FirmwareType.HCA_LoRa && type != FirmwareType.M7plus)
        throw new Exception(Ot.Gtt(Tg.Common, "NotM8", "Connected device is not M8 or M7+."));
      FirmwareVersion firmwareVersionObj = deviceIdentification.FirmwareVersionObj;
      int major = (int) firmwareVersionObj.Major;
      firmwareVersionObj = deviceIdentification.FirmwareVersionObj;
      int minor = (int) firmwareVersionObj.Minor;
      firmwareVersionObj = deviceIdentification.FirmwareVersionObj;
      int revision = (int) firmwareVersionObj.Revision;
      Version version = new Version(major, minor, revision);
      if (version >= Version.Parse("1.9.0"))
      {
        Common32BitCommands.Scenarios scenarios = await this.functions.cmd.Device.GetCommunicationScenarioAsync(progress, token);
        this.Perform_GetCommunicationScenario = scenarios;
        scenarios = (Common32BitCommands.Scenarios) null;
      }
      this.ConnectedMeter = new M8_Meter(deviceIdentification);
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
      List<AddressRange> ranges = this.ConnectedMeter.meterMemory.GetRangesForRead(includeLogger, isDump);
      progress.Split(ranges.Count);
      foreach (AddressRange range in ranges)
        await this.functions.cmd.Device.ReadMemoryAsync(range, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, token);
      this.WorkMeter = this.ConnectedMeter.Clone();
      this.Perform_SetPcTime = false;
      this.Perform_SendJoinRequest = false;
      this.Perform_SetOperatingMode = false;
      this.Perform_SetCommunicationScenario = (Common32BitCommands.Scenarios) null;
      deviceIdentification = (DeviceIdentification) null;
      version = (Version) null;
      ranges = (List<AddressRange>) null;
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
      int num;
      if (this.Perform_SetCommunicationScenario != null)
      {
        ushort? nullable1 = this.Perform_SetCommunicationScenario.ScenarioOne;
        int? nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
        nullable1 = this.Perform_GetCommunicationScenario.ScenarioOne;
        int? nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
        if (nullable2.GetValueOrDefault() == nullable3.GetValueOrDefault() & nullable2.HasValue == nullable3.HasValue)
        {
          nullable1 = this.Perform_SetCommunicationScenario.ScenarioTwo;
          nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
          nullable1 = this.Perform_GetCommunicationScenario.ScenarioTwo;
          nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
          num = !(nullable3.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable3.HasValue == nullable2.HasValue) ? 1 : 0;
        }
        else
          num = 1;
      }
      else
        num = 0;
      if (num != 0)
      {
        byte[] scenario1 = BitConverter.GetBytes(Convert.ToUInt16((object) this.Perform_SetCommunicationScenario.ScenarioOne));
        byte[] scenario2 = BitConverter.GetBytes(Convert.ToUInt16((object) this.Perform_SetCommunicationScenario.ScenarioTwo));
        await this.functions.cmd.Device.SetCommunicationScenarioAsync(scenario1, progress, token);
        await this.functions.cmd.Device.SetCommunicationScenarioAsync(scenario2, progress, token);
        this.Perform_SetCommunicationScenario = (Common32BitCommands.Scenarios) null;
        doReset = true;
        scenario1 = (byte[]) null;
        scenario2 = (byte[]) null;
      }
      if (this.Perform_SetPcTime)
      {
        double timezone = Convert.ToDouble(this.GetParameter(M8_Params.Bak_TimeZoneInQuarterHours));
        DateTime utc_plus_timezone = DateTime.UtcNow.AddHours(timezone);
        await this.functions.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(utc_plus_timezone, (sbyte) 0), progress, token);
        this.Perform_SetPcTime = false;
      }
      if (this.Perform_SendJoinRequest)
      {
        await this.functions.cmd.LoRa.SendJoinRequestAsync(progress, token);
        this.Perform_SendJoinRequest = false;
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

    public void SetParameter(M8_Params p, HcaConfig flag, object value)
    {
      if (value == null)
        return;
      if (p != M8_Params.cfg_hca_config)
        throw new ArgumentException();
      ushort theValue = (ushort) ((HcaConfig) this.WorkMeter.meterMemory.GetParameterValue<ushort>(M8_Params.cfg_hca_config) & ~flag);
      if (Convert.ToBoolean(value))
        theValue = (ushort) ((HcaConfig) theValue | flag);
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(M8_Params.cfg_hca_config, theValue);
    }

    public void SetParameter(M8_Params p, object value)
    {
      if (value == null)
        return;
      switch (p)
      {
        case M8_Params.Bak_TimeZoneInQuarterHours:
          this.WorkMeter.meterMemory.SetParameterValue<sbyte>(M8_Params.Bak_TimeZoneInQuarterHours, Convert.ToSByte(Convert.ToDouble(value) * 4.0));
          break;
        case M8_Params.cfg_battery_end_life_date:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(M8_Params.cfg_battery_end_life_date, MBusUtil.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(Convert.ToDateTime(value)));
          break;
        case M8_Params.cfg_transmission_scenario:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(M8_Params.cfg_transmission_scenario, Convert.ToByte(value));
          break;
        case M8_Params.cfg_Key_date:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(M8_Params.cfg_Key_date, MBusUtil.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(Convert.ToDateTime(value)));
          break;
        case M8_Params.cfg_hca_K:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(M8_Params.cfg_hca_K, Convert.ToUInt16(Convert.ToDouble(value) * 1000.0));
          break;
        case M8_Params.cfg_hca_winter_start:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(M8_Params.cfg_hca_winter_start, MBusUtil.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(Convert.ToDateTime(value)));
          break;
        case M8_Params.cfg_hca_summer_start:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(M8_Params.cfg_hca_summer_start, MBusUtil.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(Convert.ToDateTime(value)));
          break;
        case M8_Params.cfb_lora_AppKey:
        case M8_Params.cfb_lora_nwkskey:
        case M8_Params.cfb_lora_appskey:
        case M8_Params.cfg_lora_device_id:
        case M8_Params.Mbus_aes_key:
          this.WorkMeter.meterMemory.SetData(p, AES.StringToAesKey(value.ToString()));
          break;
        case M8_Params.cfg_hca:
          byte[] bytes = Metrology.GetBytes(this.WorkMeter.deviceIdentification.FirmwareVersionObj, value.ToString());
          if (bytes == null)
            throw new Exception("Unknown Metrology name: " + value.ToString());
          Utility.ByteArrayToHexString(bytes);
          this.WorkMeter.meterMemory.SetData(p, bytes);
          break;
        case M8_Params.cfg_otaa_abp_mode:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(M8_Params.cfg_otaa_abp_mode, value.ToString() == "OTAA" ? (byte) 1 : (byte) 2);
          break;
        case M8_Params.lora_adr:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(M8_Params.lora_adr, Convert.ToBoolean(value.ToString()) ? (byte) 1 : (byte) 0);
          break;
        case M8_Params.cfg_hca_measure_start:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(M8_Params.cfg_hca_measure_start, MBusUtil.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(Convert.ToDateTime(value)));
          break;
        case M8_Params.Mbus_interval:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(M8_Params.Mbus_interval, Convert.ToUInt16((int) Convert.ToUInt16(value) / 2));
          break;
        case M8_Params.FD_Mbus_interval:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(M8_Params.FD_Mbus_interval, Convert.ToUInt16((int) Convert.ToUInt16(value) / 2));
          break;
        case M8_Params.FD_Mbus_nighttime_start:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(M8_Params.FD_Mbus_nighttime_start, Convert.ToByte(value));
          break;
        case M8_Params.FD_Mbus_nighttime_stop:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(M8_Params.FD_Mbus_nighttime_stop, Convert.ToByte(value));
          break;
        case M8_Params.FD_Mbus_radio_suppression_days:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(M8_Params.FD_Mbus_radio_suppression_days, Convert.ToByte(value));
          break;
        case M8_Params.FD_Radio3_ID:
          this.WorkMeter.meterMemory.SetParameterValue<uint>(M8_Params.FD_Radio3_ID, (uint) value);
          break;
        default:
          throw new NotImplementedException(p.ToString());
      }
    }

    public object GetParameter(M8_Params p)
    {
      switch (p)
      {
        case M8_Params.MeterId:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<uint>(M8_Params.MeterId);
        case M8_Params.Bak_TimeZoneInQuarterHours:
          return (object) ((double) this.WorkMeter.meterMemory.GetParameterValue<sbyte>(M8_Params.Bak_TimeZoneInQuarterHours) / 4.0);
        case M8_Params.cfg_battery_end_life_date:
          return (object) Util.ConvertToDate_MBus_CP16_TypeG(BitConverter.GetBytes(this.WorkMeter.meterMemory.GetParameterValue<ushort>(M8_Params.cfg_battery_end_life_date)), 0);
        case M8_Params.cfg_transmission_scenario:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.cfg_transmission_scenario);
        case M8_Params.cfg_Key_date:
          return (object) Util.ConvertToDate_MBus_CP16_TypeG(BitConverter.GetBytes(this.WorkMeter.meterMemory.GetParameterValue<ushort>(M8_Params.cfg_Key_date)), 0);
        case M8_Params.cfg_hca_K:
          return (object) (Convert.ToDecimal(this.WorkMeter.meterMemory.GetParameterValue<ushort>(M8_Params.cfg_hca_K)) / 1000M);
        case M8_Params.cfg_hca_winter_start:
          return (object) Util.ConvertToDate_MBus_CP16_TypeG(BitConverter.GetBytes(this.WorkMeter.meterMemory.GetParameterValue<ushort>(M8_Params.cfg_hca_winter_start)), 0);
        case M8_Params.cfg_hca_summer_start:
          return (object) Util.ConvertToDate_MBus_CP16_TypeG(BitConverter.GetBytes(this.WorkMeter.meterMemory.GetParameterValue<ushort>(M8_Params.cfg_hca_summer_start)), 0);
        case M8_Params.cfb_lora_AppKey:
        case M8_Params.cfb_lora_nwkskey:
        case M8_Params.cfb_lora_appskey:
        case M8_Params.cfg_lora_device_id:
        case M8_Params.Mbus_aes_key:
          return (object) AES.AesKeyToString(this.WorkMeter.meterMemory.GetData(p));
        case M8_Params.cfg_hca:
          byte[] data = this.WorkMeter.meterMemory.GetData(M8_Params.cfg_hca);
          Utility.ByteArrayToHexString(data);
          return (object) Metrology.GetName(data);
        case M8_Params.cfg_otaa_abp_mode:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.cfg_otaa_abp_mode))
          {
            case 1:
              return (object) "OTAA";
            case 2:
              return (object) "ABP";
            default:
              return (object) string.Empty;
          }
        case M8_Params.cfg_loraWan_netid:
          return (object) Convert.ToUInt32(this.WorkMeter.meterMemory.GetParameterValue<uint>(M8_Params.cfg_loraWan_netid));
        case M8_Params.cfg_device_mode:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.cfg_device_mode))
          {
            case 0:
              return (object) "OPERATION_MODE";
            case 1:
              return (object) "DELIVERY_MODE";
            case 3:
              return (object) "TEMPERATURE_CALIBRATION_MODE";
            case 8:
              return (object) "DELIVERY_MODE_8";
            default:
              throw new NotSupportedException("Unknown value for cfg_device_mode!");
          }
        case M8_Params.device_status:
          ushort parameterValue = this.WorkMeter.meterMemory.GetParameterValue<ushort>(M8_Params.device_status);
          return parameterValue == (ushort) 0 ? (object) string.Empty : (object) ((DeviceStatus) parameterValue).ToString();
        case M8_Params.lora_adr:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.lora_adr))
          {
            case 0:
              return (object) false;
            case 1:
              return (object) true;
            default:
              throw new NotSupportedException("Unknown value for lora_adr!");
          }
        case M8_Params.cfg_hca_measure_start:
          return (object) Util.ConvertToDate_MBus_CP16_TypeG(BitConverter.GetBytes(this.WorkMeter.meterMemory.GetParameterValue<ushort>(M8_Params.cfg_hca_measure_start)), 0);
        case M8_Params.Mbus_interval:
          return (object) ((int) this.WorkMeter.meterMemory.GetParameterValue<ushort>(M8_Params.Mbus_interval) * 2);
        case M8_Params.cfg_AccessRadioKey:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<uint>(M8_Params.cfg_AccessRadioKey);
        case M8_Params.rtc_sys_time:
          return (object) Utility.ConvertToDateTime_SystemTime64(this.WorkMeter.meterMemory.GetData(M8_Params.rtc_sys_time), 0);
        case M8_Params.radio_center_frequency:
          return (object) ((double) this.WorkMeter.meterMemory.GetParameterValue<uint>(M8_Params.radio_center_frequency) / 1000000.0);
        case M8_Params.FD_Mbus_interval:
          return (object) ((int) this.WorkMeter.meterMemory.GetParameterValue<ushort>(M8_Params.FD_Mbus_interval) * 2);
        case M8_Params.Mbus_nighttime_start:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.FD_Mbus_nighttime_start);
        case M8_Params.Mbus_nighttime_stop:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.Mbus_nighttime_stop);
        case M8_Params.Mbus_radio_suppression_days:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.Mbus_radio_suppression_days);
        case M8_Params.cfg_RadioOperationState:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.cfg_RadioOperationState))
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

    public object GetParameter(M8_Params p, HcaConfig flag)
    {
      if (p != M8_Params.cfg_hca_config)
        throw new ArgumentException();
      bool flag1 = ((HcaConfig) this.WorkMeter.meterMemory.GetParameterValue<ushort>(M8_Params.cfg_hca_config) & flag) == flag;
      return flag == HcaConfig.HCA_PSCALE ? (object) (HCA_Scale) (flag1 ? 1 : 0) : (object) flag1;
    }

    internal SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters()
    {
      if (this.WorkMeter == null)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      M8_DeviceMemory meterMemory = this.WorkMeter.meterMemory;
      UserManager.CheckPermission("Role\\Developer");
      SortedList<OverrideID, ConfigurationParameter> r1 = new SortedList<OverrideID, ConfigurationParameter>();
      FirmwareVersion firmwareVersionObj = this.WorkMeter.deviceIdentification.FirmwareVersionObj;
      int major = (int) firmwareVersionObj.Major;
      firmwareVersionObj = this.WorkMeter.deviceIdentification.FirmwareVersionObj;
      int minor = (int) firmwareVersionObj.Minor;
      firmwareVersionObj = this.WorkMeter.deviceIdentification.FirmwareVersionObj;
      int revision = (int) firmwareVersionObj.Revision;
      bool flag1 = new Version(major, minor, revision) >= Version.Parse("1.9.0");
      bool flag2 = false;
      bool flag3 = false;
      if (flag1 && this.Perform_GetCommunicationScenario != null)
      {
        if (this.Perform_SetCommunicationScenario != null)
        {
          M8_AllMeters.AddParam(true, r1, OverrideID.CommunicationScenarioLoRa, (object) this.Perform_SetCommunicationScenario.ScenarioOne);
          M8_AllMeters.AddParam(true, r1, OverrideID.CommunicationScenarioWmbus, (object) this.Perform_SetCommunicationScenario.ScenarioTwo);
          ushort? nullable1 = this.Perform_SetCommunicationScenario.ScenarioOne;
          int? nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
          int num1 = 200;
          int num2;
          if (nullable2.GetValueOrDefault() >= num1 & nullable2.HasValue)
          {
            nullable1 = this.Perform_SetCommunicationScenario.ScenarioOne;
            nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
            int num3 = 300;
            num2 = nullable2.GetValueOrDefault() < num3 & nullable2.HasValue ? 1 : 0;
          }
          else
            num2 = 0;
          flag2 = num2 != 0;
          nullable1 = this.Perform_SetCommunicationScenario.ScenarioTwo;
          nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
          int num4 = 300;
          int num5;
          if (nullable2.GetValueOrDefault() >= num4 & nullable2.HasValue)
          {
            nullable1 = this.Perform_SetCommunicationScenario.ScenarioTwo;
            nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
            int num6 = 400;
            num5 = nullable2.GetValueOrDefault() < num6 & nullable2.HasValue ? 1 : 0;
          }
          else
            num5 = 0;
          flag3 = num5 != 0;
        }
        else
        {
          M8_AllMeters.AddParam(true, r1, OverrideID.CommunicationScenarioLoRa, (object) this.Perform_GetCommunicationScenario.ScenarioOne);
          M8_AllMeters.AddParam(true, r1, OverrideID.CommunicationScenarioWmbus, (object) this.Perform_GetCommunicationScenario.ScenarioTwo);
          ushort? nullable3 = this.Perform_GetCommunicationScenario.ScenarioOne;
          int? nullable4 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
          int num7 = 200;
          int num8;
          if (nullable4.GetValueOrDefault() >= num7 & nullable4.HasValue)
          {
            nullable3 = this.Perform_GetCommunicationScenario.ScenarioOne;
            nullable4 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
            int num9 = 300;
            num8 = nullable4.GetValueOrDefault() < num9 & nullable4.HasValue ? 1 : 0;
          }
          else
            num8 = 0;
          flag2 = num8 != 0;
          nullable3 = this.Perform_GetCommunicationScenario.ScenarioTwo;
          nullable4 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
          int num10 = 300;
          int num11;
          if (nullable4.GetValueOrDefault() >= num10 & nullable4.HasValue)
          {
            nullable3 = this.Perform_GetCommunicationScenario.ScenarioTwo;
            nullable4 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
            int num12 = 400;
            num11 = nullable4.GetValueOrDefault() < num12 & nullable4.HasValue ? 1 : 0;
          }
          else
            num11 = 0;
          flag3 = num11 != 0;
        }
      }
      else if (this.WorkMeter.meterMemory.IsParameterAvailable(M8_Params.cfg_transmission_scenario))
      {
        TransmissionScenario parameter = (TransmissionScenario) this.GetParameter(M8_Params.cfg_transmission_scenario);
        int num;
        switch (parameter)
        {
          case TransmissionScenario.Scenario1:
          case TransmissionScenario.Scenario2:
            num = 1;
            break;
          default:
            num = parameter == TransmissionScenario.Scenario3 ? 1 : 0;
            break;
        }
        flag2 = num != 0;
        flag3 = parameter == TransmissionScenario.WMBusFormatA || parameter == TransmissionScenario.WMBusFormatB;
      }
      if (flag2 & flag3)
      {
        M8_AllMeters.AddParam(false, r1, OverrideID.RadioTechnology, (object) RadioTechnology.LoRa_and_wMBus);
      }
      else
      {
        if (flag2)
          M8_AllMeters.AddParam(false, r1, OverrideID.RadioTechnology, (object) RadioTechnology.LoRa);
        if (flag3)
          M8_AllMeters.AddParam(false, r1, OverrideID.RadioTechnology, (object) RadioTechnology.wMBus);
      }
      if (flag3)
      {
        M8_AllMeters.AddParam(false, r1, OverrideID.SerialNumber, (object) this.WorkMeter.deviceIdentification.ID_BCD_AsString);
        M8_AllMeters.AddParam(false, r1, OverrideID.SerialNumberFull, (object) this.WorkMeter.deviceIdentification.FullSerialNumber);
        M8_AllMeters.AddParam(false, r1, OverrideID.Manufacturer, MBusUtil.GetManufacturer(this.WorkMeter.deviceIdentification.Manufacturer));
        M8_AllMeters.AddParam(false, r1, OverrideID.MBusGeneration, (object) this.WorkMeter.deviceIdentification.Generation);
        M8_AllMeters.AddParam(false, r1, OverrideID.Medium, (object) (MBusDeviceType) this.WorkMeter.deviceIdentification.Medium.Value);
        if (!flag1 && this.WorkMeter.meterMemory.IsParameterAvailable(M8_Params.Mbus_interval))
          M8_AllMeters.AddParam(true, r1, OverrideID.RadioSendInterval, this.GetParameter(M8_Params.Mbus_interval));
        if (this.WorkMeter.meterMemory.IsParameterAvailable(M8_Params.Mbus_aes_key))
          M8_AllMeters.AddParam(true, r1, OverrideID.AESKey, this.GetParameter(M8_Params.Mbus_aes_key));
        if (!flag1 && this.WorkMeter.meterMemory.IsParameterAvailable(M8_Params.Mbus_radio_suppression_days))
        {
          byte parameterValue1 = this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.Mbus_radio_suppression_days);
          byte num = this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.Mbus_nighttime_start);
          byte parameterValue2 = this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.Mbus_nighttime_stop);
          if (num == (byte) 0)
            num = (byte) 24;
          M8_AllMeters.AddParam(true, r1, OverrideID.RadioActiveMonday, (object) !Convert.ToBoolean((int) parameterValue1 & 1));
          M8_AllMeters.AddParam(true, r1, OverrideID.RadioActiveTuesday, (object) !Convert.ToBoolean((int) parameterValue1 & 2));
          M8_AllMeters.AddParam(true, r1, OverrideID.RadioActiveWednesday, (object) !Convert.ToBoolean((int) parameterValue1 & 4));
          M8_AllMeters.AddParam(true, r1, OverrideID.RadioActiveThursday, (object) !Convert.ToBoolean((int) parameterValue1 & 8));
          M8_AllMeters.AddParam(true, r1, OverrideID.RadioActiveFriday, (object) !Convert.ToBoolean((int) parameterValue1 & 16));
          M8_AllMeters.AddParam(true, r1, OverrideID.RadioActiveSaturday, (object) !Convert.ToBoolean((int) parameterValue1 & 32));
          M8_AllMeters.AddParam(true, r1, OverrideID.RadioActiveSunday, (object) !Convert.ToBoolean((int) parameterValue1 & 64));
          M8_AllMeters.AddParam(true, r1, OverrideID.RadioActiveStartTime, (object) parameterValue2);
          M8_AllMeters.AddParam(true, r1, OverrideID.RadioActiveStopTime, (object) num);
        }
      }
      if (flag2)
      {
        M8_AllMeters.AddParam(false, r1, OverrideID.LoRaWanVersion, (object) string.Format("{0}.{1}.{2}", (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.protocol_lorawan_version_major), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.protocol_lorawan_version_middle), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.protocol_lorawan_version_minor)));
        M8_AllMeters.AddParam(false, r1, OverrideID.LoRaVersion, (object) string.Format("{0}.{1}", (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.protocol_lora_version_major), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.protocol_lora_version_minor)));
        M8_AllMeters.AddParam(false, r1, OverrideID.NetID, (object) meterMemory.GetParameterValue<uint>(M8_Params.cfg_loraWan_netid));
        M8_AllMeters.AddParam(true, r1, OverrideID.JoinEUI, (object) this.WorkMeter.deviceIdentification.LoRa_JoinEUI);
        M8_AllMeters.AddParam(true, r1, OverrideID.DevEUI, (object) this.WorkMeter.deviceIdentification.LoRa_DevEUI);
        M8_AllMeters.AddParam(true, r1, OverrideID.AppKey, (object) this.WorkMeter.deviceIdentification.LoRa_AppKey);
        M8_AllMeters.AddParam(true, r1, OverrideID.NwkSKey, (object) meterMemory.GetData(M8_Params.cfb_lora_nwkskey));
        M8_AllMeters.AddParam(true, r1, OverrideID.AppSKey, (object) meterMemory.GetData(M8_Params.cfb_lora_appskey));
        M8_AllMeters.AddParam(true, r1, OverrideID.DevAddr, (object) meterMemory.GetParameterValue<uint>(M8_Params.cfg_lora_device_id));
        M8_AllMeters.AddParam(true, r1, OverrideID.Activation, this.GetParameter(M8_Params.cfg_otaa_abp_mode), false, new string[2]
        {
          "OTAA",
          "ABP"
        }, (string) null);
        M8_AllMeters.AddParam(true, r1, OverrideID.SendJoinRequest, (object) this.Perform_SendJoinRequest, true, (string[]) null, (string) null);
        M8_AllMeters.AddParam(true, r1, OverrideID.ADR, this.GetParameter(M8_Params.lora_adr));
      }
      M8_AllMeters.AddParam(false, r1, OverrideID.Frequence, this.GetParameter(M8_Params.radio_center_frequency), false, (string[]) null, "MHz");
      M8_AllMeters.AddParam(false, r1, OverrideID.PrintedSerialNumber, (object) this.WorkMeter.deviceIdentification.PrintedSerialNumberAsString);
      M8_AllMeters.AddParam(false, r1, OverrideID.RadioEnabled, this.GetParameter(M8_Params.cfg_RadioOperationState));
      SortedList<OverrideID, ConfigurationParameter> r2 = r1;
      firmwareVersionObj = this.WorkMeter.deviceIdentification.FirmwareVersionObj;
      string str = firmwareVersionObj.ToString();
      M8_AllMeters.AddParam(false, r2, OverrideID.FirmwareVersion, (object) str);
      M8_AllMeters.AddParam(false, r1, OverrideID.RadioVersion, (object) string.Format("{0}.{1}", (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.protocol_radio_version_major), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.protocol_radio_version_minor)));
      Decimal num13 = Convert.ToDecimal((double) BitConverter.ToUInt32(this.WorkMeter.meterMemory.GetData(this.WorkMeter.meterMemory.GetParameterAddress(M8_Params.hca_reading.ToString()), 4U), 0) / 65536.0 * (double) this.WorkMeter.meterMemory.GetParameterValue<ushort>(M8_Params.cfg_hca_K) / 4000.0);
      M8_AllMeters.AddParam(false, r1, OverrideID.HCA_ActualValue, (object) num13);
      M8_AllMeters.AddParam(false, r1, OverrideID.WarningInfo, this.GetParameter(M8_Params.device_status));
      M8_AllMeters.AddParam(false, r1, OverrideID.DeviceMode, this.GetParameter(M8_Params.cfg_device_mode));
      if (!flag1)
        M8_AllMeters.AddParam(true, r1, OverrideID.TransmissionScenario, this.GetParameter(M8_Params.cfg_transmission_scenario));
      M8_AllMeters.AddParam(true, r1, OverrideID.DueDate, this.GetParameter(M8_Params.cfg_Key_date));
      M8_AllMeters.AddParam(true, r1, OverrideID.StartDate, this.GetParameter(M8_Params.cfg_hca_measure_start));
      M8_AllMeters.AddParam(true, r1, OverrideID.EndOfBatteryDate, this.GetParameter(M8_Params.cfg_battery_end_life_date));
      M8_AllMeters.AddParam(true, r1, OverrideID.HCA_Scale, this.GetParameter(M8_Params.cfg_hca_config, HcaConfig.HCA_PSCALE));
      M8_AllMeters.AddParam(true, r1, OverrideID.SummerOff, this.GetParameter(M8_Params.cfg_hca_config, HcaConfig.HCA_SUMMER_OFF));
      M8_AllMeters.AddParam(true, r1, OverrideID.HCA_Factor_Weighting, this.GetParameter(M8_Params.cfg_hca_K));
      M8_AllMeters.AddParam(true, r1, OverrideID.WinterStart, this.GetParameter(M8_Params.cfg_hca_winter_start));
      M8_AllMeters.AddParam(true, r1, OverrideID.SummerStart, this.GetParameter(M8_Params.cfg_hca_summer_start));
      M8_AllMeters.AddParam(true, r1, OverrideID.HCA_Metrology, this.GetParameter(M8_Params.cfg_hca), false, Metrology.GetNames(), (string) null);
      M8_AllMeters.AddParam(true, r1, OverrideID.SetPcTime, (object) this.Perform_SetPcTime, true, (string[]) null, (string) null);
      M8_AllMeters.AddParam(true, r1, OverrideID.SetOperatingMode, (object) this.Perform_SetOperatingMode, true, (string[]) null, (string) null);
      M8_AllMeters.AddParam(true, r1, OverrideID.TimeZone, (object) Convert.ToDecimal(this.GetParameter(M8_Params.Bak_TimeZoneInQuarterHours)));
      if (this.WorkMeter.meterMemory.IsParameterAvailable(M8_Params.rtc_sys_time))
        M8_AllMeters.AddParam(false, r1, OverrideID.DeviceClock, this.GetParameter(M8_Params.rtc_sys_time));
      return r1;
    }

    internal void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameter)
    {
      if (parameter == null || parameter.Count == 0)
        return;
      M8_DeviceMemory meterMemory = this.WorkMeter.meterMemory;
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in parameter)
      {
        if (keyValuePair.Key == OverrideID.TransmissionScenario)
        {
          if (this.WorkMeter.meterMemory.IsParameterAvailable(M8_Params.cfg_transmission_scenario))
            this.SetParameter(M8_Params.cfg_transmission_scenario, keyValuePair.Value.ParameterValue);
        }
        else if (keyValuePair.Key == OverrideID.DueDate)
          this.SetParameter(M8_Params.cfg_Key_date, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.EndOfBatteryDate)
          this.SetParameter(M8_Params.cfg_battery_end_life_date, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.HCA_Scale)
          this.SetParameter(M8_Params.cfg_hca_config, HcaConfig.HCA_PSCALE, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.Activation)
          this.SetParameter(M8_Params.cfg_otaa_abp_mode, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.SummerOff)
          this.SetParameter(M8_Params.cfg_hca_config, HcaConfig.HCA_SUMMER_OFF, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.HCA_Factor_Weighting)
          this.SetParameter(M8_Params.cfg_hca_K, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.WinterStart)
          this.SetParameter(M8_Params.cfg_hca_winter_start, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.SummerStart)
          this.SetParameter(M8_Params.cfg_hca_summer_start, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.StartDate)
          this.SetParameter(M8_Params.cfg_hca_measure_start, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.HCA_Metrology)
          this.SetParameter(M8_Params.cfg_hca, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.DevEUI)
          meterMemory.SetParameterValue<ulong>(M8_Params.cfg_lora_deveui, (ulong) keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.JoinEUI)
          meterMemory.SetParameterValue<ulong>(M8_Params.cfg_lora_appeui, (ulong) keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.AppKey)
          meterMemory.SetData(M8_Params.cfb_lora_AppKey, (byte[]) keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.NwkSKey)
          meterMemory.SetData(M8_Params.cfb_lora_nwkskey, (byte[]) keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.AppSKey)
          meterMemory.SetData(M8_Params.cfb_lora_appskey, (byte[]) keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.DevAddr)
          meterMemory.SetParameterValue<uint>(M8_Params.cfg_lora_device_id, (uint) keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.TimeZone)
          this.SetParameter(M8_Params.Bak_TimeZoneInQuarterHours, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.ADR)
          this.SetParameter(M8_Params.lora_adr, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.AESKey)
          this.SetParameter(M8_Params.Mbus_aes_key, keyValuePair.Value.ParameterValue);
        else if (keyValuePair.Key == OverrideID.RadioSendInterval)
          this.SetParameter(M8_Params.Mbus_interval, keyValuePair.Value.ParameterValue);
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
          this.WorkMeter.meterMemory.SetParameterValue<byte>(M8_Params.Mbus_nighttime_stop, theValue);
        }
        else if (keyValuePair.Key == OverrideID.RadioActiveStopTime)
        {
          byte theValue = Convert.ToByte(keyValuePair.Value.ParameterValue);
          if (theValue <= (byte) 1 || theValue > (byte) 24)
            throw new ArgumentOutOfRangeException("RadioActiveStopTime");
          if (theValue == (byte) 24)
            theValue = (byte) 0;
          this.WorkMeter.meterMemory.SetParameterValue<byte>(M8_Params.Mbus_nighttime_start, theValue);
        }
        else if (keyValuePair.Key == OverrideID.CommunicationScenarioLoRa)
        {
          if (this.Perform_SetCommunicationScenario == null)
          {
            if (this.Perform_GetCommunicationScenario == null)
              this.Perform_GetCommunicationScenario = new Common32BitCommands.Scenarios();
            this.Perform_SetCommunicationScenario = new Common32BitCommands.Scenarios()
            {
              ScenarioOne = this.Perform_GetCommunicationScenario.ScenarioOne,
              ScenarioTwo = this.Perform_GetCommunicationScenario.ScenarioTwo
            };
          }
          this.Perform_SetCommunicationScenario.ScenarioOne = new ushort?(Convert.ToUInt16(keyValuePair.Value.ParameterValue));
        }
        else if (keyValuePair.Key == OverrideID.CommunicationScenarioWmbus)
        {
          if (this.Perform_SetCommunicationScenario == null)
          {
            if (this.Perform_GetCommunicationScenario == null)
              this.Perform_GetCommunicationScenario = new Common32BitCommands.Scenarios();
            this.Perform_SetCommunicationScenario = new Common32BitCommands.Scenarios()
            {
              ScenarioOne = this.Perform_GetCommunicationScenario.ScenarioOne,
              ScenarioTwo = this.Perform_GetCommunicationScenario.ScenarioTwo
            };
          }
          this.Perform_SetCommunicationScenario.ScenarioTwo = new ushort?(Convert.ToUInt16(keyValuePair.Value.ParameterValue));
        }
      }
    }

    private void SetMbus_radio_suppression_days(byte mask, bool isSet)
    {
      byte parameterValue = this.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.Mbus_radio_suppression_days);
      this.WorkMeter.meterMemory.SetParameterValue<byte>(M8_Params.Mbus_radio_suppression_days, isSet ? (byte) ((uint) parameterValue & (uint) ~mask) : (byte) ((uint) parameterValue | (uint) mask));
    }

    internal SortedList<long, SortedList<DateTime, double>> GetValues()
    {
      return new SortedList<long, SortedList<DateTime, double>>();
    }

    internal void CompareConnectedAndWork()
    {
      this.WorkMeter.meterMemory.CompareParameterInfo("M8_DeviceMemory", "WorkMeter object", "ConnectedMeter object", (DeviceMemory) this.ConnectedMeter.meterMemory);
    }

    private static void AddParam(
      bool canChanged,
      SortedList<OverrideID, ConfigurationParameter> r,
      OverrideID overrideID,
      object obj)
    {
      M8_AllMeters.AddParam(canChanged, r, overrideID, obj, false, (string[]) null, (string) null);
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
