// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.EDCL_AllMeters
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using MBusLib;
using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace EDCL_Handler
{
  internal class EDCL_AllMeters : ICreateMeter, IDisposable, IMeterDataSpecial
  {
    private static Logger logger = LogManager.GetLogger(nameof (EDCL_AllMeters));
    private EDCL_HandlerFunctions functions;
    private uint? Perform_SetMeterValue = new uint?();
    private int? Perform_GetCommunicationScenario = new int?();
    private int? Perform_SetCommunicationScenario = new int?();
    private bool Perform_SetPcTime = false;
    private bool Perform_SendJoinRequest = false;
    private bool Perform_SetOperatingMode = false;
    private bool Perform_ClearWarnings = false;
    internal EDCL_Meter ConnectedMeter;
    internal EDCL_Meter TypeMeter;
    internal EDCL_Meter BackupMeter;
    internal EDCL_Meter WorkMeter;
    internal EDCL_Meter SavedMeter;
    internal List<VolumeMonitorEventArgs> EncabulatorData;

    internal EDCL_Meter checkedWorkMeter
    {
      get => this.WorkMeter != null ? this.WorkMeter : throw new Exception("WorkMeter not defined");
    }

    internal EDCL_Meter checkedChangeMeter
    {
      get
      {
        if (this.ConnectedMeter == null)
          throw new Exception("ConnectedMeter not available");
        return this.WorkMeter != null ? this.WorkMeter : throw new Exception("WorkMeter not available");
      }
    }

    internal EDCL_AllMeters()
    {
    }

    internal EDCL_AllMeters(EDCL_HandlerFunctions functions) => this.functions = functions;

    public void Dispose()
    {
      this.ConnectedMeter = (EDCL_Meter) null;
      this.WorkMeter = (EDCL_Meter) null;
      this.TypeMeter = (EDCL_Meter) null;
      this.BackupMeter = (EDCL_Meter) null;
    }

    public IMeter CreateMeter(byte[] zippedBuffer) => new EDCL_Meter().CreateFromData(zippedBuffer);

    public string SetBackupMeter(byte[] zippedBuffer)
    {
      this.BackupMeter = this.CreateMeter(zippedBuffer) as EDCL_Meter;
      if (this.WorkMeter == null)
        this.WorkMeter = this.BackupMeter.Clone();
      this.BackupMeter.meterMemory.MeterTypeName = MeterTypeNAME.BACKUP;
      return this.BackupMeter.GetInfo();
    }

    public async Task ConnectDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      DeviceVersionMBus deviceVersion = await this.functions.cmd.ReadVersionAsync(progress, token);
      this.ConnectedMeter = new EDCL_Meter((DeviceIdentification) deviceVersion);
      this.ConnectedMeter.meterMemory.MeterTypeName = MeterTypeNAME.CONNECTED;
      this.WorkMeter = this.ConnectedMeter.Clone();
      this.WorkMeter.meterMemory.MeterTypeName = MeterTypeNAME.WORK;
      this.Perform_SetPcTime = false;
      this.Perform_SendJoinRequest = false;
      this.Perform_SetOperatingMode = false;
      this.Perform_SetMeterValue = new uint?();
      this.Perform_ClearWarnings = false;
      this.Perform_SetCommunicationScenario = new int?();
      deviceVersion = (DeviceVersionMBus) null;
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
      if (type != FirmwareType.EDC_LoRa && type != FirmwareType.EDC_LoRa_470Mhz && type != FirmwareType.EDC_wMBus_ST && type != FirmwareType.micro_LoRa && type != FirmwareType.micro_wMBus && type != FirmwareType.micro_LoRa_LL && type != FirmwareType.micro_wMBus_LL && type != FirmwareType.micro_LL_radio3 && type != FirmwareType.EDC_LoRa_915MHz && type != FirmwareType.EDC_NBIoT && type != FirmwareType.EDC_NBIoT_LCSW && type != FirmwareType.EDC_NBIoT_YJSW && type != FirmwareType.EDC_NBIoT_FSNH && type != FirmwareType.EDC_NBIoT_XM && type != FirmwareType.EDC_NBIoT_Israel && type != FirmwareType.EDC_NBIoT_TaiWan && type != FirmwareType.EDC_LoRa_868_v3 && type != FirmwareType.EDC_LoRa_915_v2_US && type != FirmwareType.EDC_LoRa_915_v2_BR)
        throw new Exception(Ot.Gtt(Tg.Common, "NotEDC", "Connected device is not EDC or Micro."));
      this.ConnectedMeter = new EDCL_Meter(deviceIdentification);
      this.ConnectedMeter.meterMemory.MeterTypeName = MeterTypeNAME.CONNECTED;
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
      if ((type == FirmwareType.EDC_wMBus_ST || type == FirmwareType.micro_wMBus || type == FirmwareType.micro_wMBus_LL || type == FirmwareType.micro_LoRa || type == FirmwareType.micro_LoRa_LL || type == FirmwareType.EDC_LoRa || type == FirmwareType.micro_LL_radio3 || type == FirmwareType.EDC_LoRa_470Mhz || type == FirmwareType.EDC_LoRa_915MHz || type == FirmwareType.EDC_LoRa_868_v3) && version >= Version.Parse("1.69.0") || type == FirmwareType.EDC_LoRa_915_v2_US || type == FirmwareType.EDC_LoRa_915_v2_BR)
      {
        Common32BitCommands.Scenarios someBytesFromFunction = await this.functions.cmd.Device.GetCommunicationScenarioAsync(progress, token);
        ushort? scenarioOne = someBytesFromFunction.ScenarioOne;
        this.Perform_GetCommunicationScenario = scenarioOne.HasValue ? new int?((int) scenarioOne.GetValueOrDefault()) : new int?();
        someBytesFromFunction = (Common32BitCommands.Scenarios) null;
      }
      List<AddressRange> ranges = this.ConnectedMeter.meterMemory.GetRangesForRead(includeLogger, isDump);
      progress.Split(ranges.Count);
      foreach (AddressRange range in ranges)
      {
        this.ConnectedMeter.meterMemory.GarantMemoryAvailable(range);
        await this.functions.cmd.Device.ReadMemoryAsync(range, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, token);
      }
      this.WorkMeter = this.ConnectedMeter.Clone();
      this.WorkMeter.meterMemory.MeterTypeName = MeterTypeNAME.WORK;
      this.Perform_SetPcTime = false;
      this.Perform_SendJoinRequest = false;
      this.Perform_SetOperatingMode = false;
      this.Perform_SetMeterValue = new uint?();
      this.Perform_ClearWarnings = false;
      this.Perform_SetCommunicationScenario = new int?();
      deviceIdentification = (DeviceIdentification) null;
      version = (Version) null;
      ranges = (List<AddressRange>) null;
    }

    public async Task WriteDeviceAsync(
      ProgressHandler progress,
      CancellationToken token,
      bool makeResetAfterWriteMemory = true)
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
        if (makeResetAfterWriteMemory)
          await this.functions.ResetDeviceAsync(progress, token);
        this.functions.Clear();
      }
      DeviceVersionMBus deviceVersionMbus = await this.functions.cmd.ReadVersionAsync(progress, token);
      if (this.Perform_ClearWarnings)
      {
        await this.functions.cmd.Special.ClearFlowCheckStatesAsync(ushort.MaxValue, progress, token);
        this.Perform_ClearWarnings = false;
      }
      if (this.Perform_SetPcTime)
      {
        double timezone = Convert.ToDouble(this.GetParameter(EDCL_Params.Bak_TimeZoneInQuarterHours));
        DateTime utc_plus_timezone = DateTime.UtcNow.AddHours(timezone);
        await this.functions.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(utc_plus_timezone, (sbyte) 0), progress, token);
        this.Perform_SetPcTime = false;
      }
      bool doReset = false;
      if (this.Perform_SetMeterValue.HasValue)
      {
        await this.functions.cmd.MBusCmd.SetChannelValueAsync((byte) 0, this.Perform_SetMeterValue.Value, progress, token);
        this.Perform_SetMeterValue = new uint?();
        doReset = true;
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
      if (this.Perform_SendJoinRequest)
      {
        await this.functions.cmd.LoRa.SendJoinRequestAsync(progress, token);
        this.Perform_SendJoinRequest = false;
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

    public async Task<byte[]> ReadParameterAsync(
      ProgressHandler progress,
      CancellationToken token,
      EDCL_Params p)
    {
      if (this.WorkMeter == null)
        throw new ArgumentNullException("WorkMeter");
      Parameter32bit prm = this.WorkMeter.meterMemory.MapDef.GetParameter(p.ToString());
      if (prm == null)
        throw new Exception("Unknown parameter! Value: " + p.ToString());
      byte[] numArray = await this.functions.cmd.Device.ReadMemoryAsync(progress, token, prm.Address, prm.Size, (byte) 200);
      prm = (Parameter32bit) null;
      return numArray;
    }

    public async Task WriteParameterAsync(
      ProgressHandler progress,
      CancellationToken token,
      EDCL_Params p,
      byte[] value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (this.WorkMeter == null)
        throw new ArgumentNullException("WorkMeter");
      Parameter32bit prm = this.WorkMeter.meterMemory.MapDef.GetParameter(p.ToString());
      if (prm == null)
        throw new Exception("Unknown parameter! Value: " + p.ToString());
      await this.functions.cmd.Device.WriteMemoryAsync(progress, token, prm.Address, value, (byte) 200);
      prm = (Parameter32bit) null;
    }

    public void SetParameter(EDCL_Params p, object value)
    {
      if (value == null)
        return;
      switch (p)
      {
        case EDCL_Params.cfg_battery_end_life_date:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_battery_end_life_date, MBusUtil.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(Convert.ToDateTime(value)));
          break;
        case EDCL_Params.cfg_transmission_scenario:
          if (Convert.ToByte(value) == (byte) 3 && this.WorkMeter.deviceIdentification.FirmwareVersion.Value >> 12 <= 4640U)
            throw new NotSupportedException("Only scenario 1(201) or 2 (202) possible!");
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.cfg_transmission_scenario, Convert.ToByte(value));
          break;
        case EDCL_Params.cfg_Key_date:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_Key_date, MBusUtil.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(Convert.ToDateTime(value)));
          break;
        case EDCL_Params.cfg_coil_b_offset:
          this.WorkMeter.meterMemory.SetParameterValue<sbyte>(EDCL_Params.cfg_coil_b_offset, Convert.ToSByte(value));
          break;
        case EDCL_Params.cfg_scanif_a:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_scanif_a, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_scanif_b:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_scanif_b, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_coil_max_threshold:
          this.WorkMeter.meterMemory.SetParameterValue<sbyte>(EDCL_Params.cfg_coil_max_threshold, Convert.ToSByte(value));
          break;
        case EDCL_Params.cfg_coil_min_threshold:
          this.WorkMeter.meterMemory.SetParameterValue<sbyte>(EDCL_Params.cfg_coil_min_threshold, Convert.ToSByte(value));
          break;
        case EDCL_Params.cfb_lora_nwkskey:
        case EDCL_Params.cfb_lora_appskey:
        case EDCL_Params.cfg_lora_device_id:
        case EDCL_Params.cfb_lora_AppKey:
          this.WorkMeter.meterMemory.SetData(p, Util.HexStringToByteArray(value.ToString()));
          break;
        case EDCL_Params.cfg_otaa_abp_mode:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.cfg_otaa_abp_mode, value.ToString() == "OTAA" ? (byte) 1 : (byte) 2);
          break;
        case EDCL_Params.hwStatusFlags:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.hwStatusFlags, Convert.ToUInt16(value));
          break;
        case EDCL_Params.persistent_warning_flags:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.persistent_warning_flags, Convert.ToUInt16(value));
          break;
        case EDCL_Params.pulseReading:
          this.WorkMeter.meterMemory.SetParameterValue<int>(EDCL_Params.pulseReading, Convert.ToInt32(value));
          break;
        case EDCL_Params.cfg_pulse_multiplier:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.cfg_pulse_multiplier, Convert.ToByte(value));
          break;
        case EDCL_Params.pulseAbnormalCounter:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.pulseAbnormalCounter, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_cog_count:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.cfg_cog_count, Convert.ToByte(value));
          break;
        case EDCL_Params.Rx2DownLinkChannelIndex:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.Rx2DownLinkChannelIndex, (byte) value);
          break;
        case EDCL_Params.UpDownChannelScenario:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.UpDownChannelScenario, (byte) value);
          break;
        case EDCL_Params.UpLinkChannelQuantity:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.UpLinkChannelQuantity, (byte) value);
          break;
        case EDCL_Params.UpLinkStartChannelIndex:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.UpLinkStartChannelIndex, (byte) value);
          break;
        case EDCL_Params.Bak_TimeZoneInQuarterHours:
          this.WorkMeter.meterMemory.SetParameterValue<sbyte>(EDCL_Params.Bak_TimeZoneInQuarterHours, Convert.ToSByte(Convert.ToDouble(value) * 4.0));
          break;
        case EDCL_Params.lora_adr:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.lora_adr, Convert.ToBoolean(value.ToString()) ? (byte) 1 : (byte) 0);
          break;
        case EDCL_Params.Mbus_aes_key:
          byte[] aesKey = AES.StringToAesKey(value.ToString());
          if (aesKey == null)
            break;
          this.WorkMeter.meterMemory.SetData(p, aesKey);
          break;
        case EDCL_Params.Mbus_interval:
          ushort uint16_1 = Convert.ToUInt16((int) Convert.ToUInt16(value) / 2);
          ushort uint16_2 = Convert.ToUInt16(this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.FD_Mbus_interval));
          if ((int) uint16_1 < (int) uint16_2)
            throw new Exception("RadioSendInterval cannot be less as " + ((int) uint16_2 * 2).ToString() + " seconds");
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.Mbus_interval, uint16_1);
          break;
        case EDCL_Params.cfg_scanif_cycle_lora:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_scanif_cycle_lora, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_scanif_cycle_low:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_scanif_cycle_low, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_scanif_cycle_nom:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_scanif_cycle_nom, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_scanif_noflow_timeout:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_scanif_noflow_timeout, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_scanif_period_low:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_scanif_period_low, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_scanif_period_nom:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_scanif_period_nom, Convert.ToUInt16(value));
          break;
        case EDCL_Params.scanif_noflow_timeout:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.scanif_noflow_timeout, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_pulse_block_limit:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_block_limit, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_pulse_leak_limit:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_leak_limit, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_pulse_unleak_limit:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_unleak_limit, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_pulse_leak_lower:
          this.WorkMeter.meterMemory.SetParameterValue<short>(EDCL_Params.cfg_pulse_leak_lower, Convert.ToInt16(value));
          break;
        case EDCL_Params.cfg_pulse_leak_upper:
          this.WorkMeter.meterMemory.SetParameterValue<short>(EDCL_Params.cfg_pulse_leak_upper, Convert.ToInt16(value));
          break;
        case EDCL_Params.cfg_pulse_back_limit:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_back_limit, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_pulse_unback_limit:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_unback_limit, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_oversize_diff:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_oversize_diff, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_oversize_limit:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_oversize_limit, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_undersize_diff:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_undersize_diff, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_undersize_limit:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_undersize_limit, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_burst_diff:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_burst_diff, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_burst_limit:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_burst_limit, Convert.ToUInt16(value));
          break;
        case EDCL_Params.cfg_min_difference_from_max_undamped_coil_counter:
          this.WorkMeter.meterMemory.SetParameterValue<short>(EDCL_Params.cfg_min_difference_from_max_undamped_coil_counter, Convert.ToInt16(value));
          break;
        case EDCL_Params.cfg_max_difference_from_max_undamped_coil_counter:
          this.WorkMeter.meterMemory.SetParameterValue<short>(EDCL_Params.cfg_max_difference_from_max_undamped_coil_counter, Convert.ToInt16(value));
          break;
        case EDCL_Params.cfg_overstep:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.cfg_overstep, (byte) value);
          break;
        case EDCL_Params.cfg_max_a_b_pulse_diff:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.cfg_max_a_b_pulse_diff, (byte) value);
          break;
        case EDCL_Params.cfg_max_b_a_pulse_diff:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.cfg_max_b_a_pulse_diff, (byte) value);
          break;
        case EDCL_Params.cfg_min_undamped_coil_pulses:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.cfg_min_undamped_coil_pulses, (byte) value);
          break;
        case EDCL_Params.FD_Mbus_interval:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.FD_Mbus_interval, Convert.ToUInt16((int) Convert.ToUInt16(value) / 2));
          break;
        case EDCL_Params.FD_Mbus_nighttime_start:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.FD_Mbus_nighttime_start, Convert.ToByte(value));
          break;
        case EDCL_Params.FD_Mbus_nighttime_stop:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.FD_Mbus_nighttime_stop, Convert.ToByte(value));
          break;
        case EDCL_Params.FD_Mbus_radio_suppression_days:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.FD_Mbus_radio_suppression_days, Convert.ToByte(value));
          break;
        case EDCL_Params.VIF:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.VIF, Convert.ToByte(value));
          break;
        case EDCL_Params.FD_NB_ServerIP:
          this.WorkMeter.meterMemory.SetParameterValue<byte[]>(EDCL_Params.FD_NB_ServerIP, (byte[]) value);
          break;
        case EDCL_Params.FD_NB_ServerPort:
          this.WorkMeter.meterMemory.SetParameterValue<byte[]>(EDCL_Params.FD_NB_ServerPort, (byte[]) value);
          break;
        case EDCL_Params.cfg_pulse_coefficient:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.cfg_pulse_coefficient, (byte) value);
          break;
        case EDCL_Params.cfg_reading_unit:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_reading_unit, Convert.ToUInt16(value));
          break;
        default:
          throw new NotImplementedException("Set Parameter Not Defined:" + p.ToString());
      }
    }

    public object GetParameter(EDCL_Params p)
    {
      switch (p)
      {
        case EDCL_Params.cfg_battery_end_life_date:
          return (object) Util.ConvertToDate_MBus_CP16_TypeG(BitConverter.GetBytes(this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_battery_end_life_date)), 0);
        case EDCL_Params.cfg_transmission_scenario:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_transmission_scenario);
        case EDCL_Params.cfg_Key_date:
          return (object) Util.ConvertToDate_MBus_CP16_TypeG(BitConverter.GetBytes(this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_Key_date)), 0);
        case EDCL_Params.cfg_coil_b_offset:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<sbyte>(EDCL_Params.cfg_coil_b_offset);
        case EDCL_Params.cfg_scanif_a:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_scanif_a);
        case EDCL_Params.cfg_scanif_b:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_scanif_b);
        case EDCL_Params.cfg_coil_max_threshold:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<sbyte>(EDCL_Params.cfg_coil_max_threshold);
        case EDCL_Params.cfg_coil_min_threshold:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<sbyte>(EDCL_Params.cfg_coil_min_threshold);
        case EDCL_Params.cfb_lora_nwkskey:
        case EDCL_Params.cfb_lora_appskey:
        case EDCL_Params.cfg_lora_device_id:
        case EDCL_Params.cfb_lora_AppKey:
          return (object) Utility.ByteArrayToHexString(this.WorkMeter.meterMemory.GetData(p));
        case EDCL_Params.cfg_otaa_abp_mode:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_otaa_abp_mode))
          {
            case 1:
              return (object) "OTAA";
            case 2:
              return (object) "ABP";
            default:
              return (object) string.Empty;
          }
        case EDCL_Params.hwStatusFlags:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.hwStatusFlags);
        case EDCL_Params.persistent_warning_flags:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
        case EDCL_Params.pulseReading:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<int>(EDCL_Params.pulseReading);
        case EDCL_Params.cfg_pulse_multiplier:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_pulse_multiplier);
        case EDCL_Params.cfg_cog_count:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_cog_count);
        case EDCL_Params.cfg_coil_error_threshold:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_coil_error_threshold);
        case EDCL_Params.Rx2DownLinkChannelIndex:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.Rx2DownLinkChannelIndex);
        case EDCL_Params.UpDownChannelScenario:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.UpDownChannelScenario);
        case EDCL_Params.UpLinkChannelQuantity:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.UpLinkChannelQuantity);
        case EDCL_Params.UpLinkStartChannelIndex:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.UpLinkStartChannelIndex);
        case EDCL_Params.Bak_TimeZoneInQuarterHours:
          return (object) ((double) this.WorkMeter.meterMemory.GetParameterValue<sbyte>(EDCL_Params.Bak_TimeZoneInQuarterHours) / 4.0);
        case EDCL_Params.cfg_device_mode:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_device_mode))
          {
            case 0:
              return (object) "OPERATION_MODE";
            case 1:
              return (object) "DELIVERY_MODE";
            case 3:
              return (object) "TEMPERATURE_CALIBRATION_MODE";
            case 4:
              return (object) "VOLUME_CALIBRATION_MODE";
            case 8:
              return (object) "DELIVERY_MODE_8";
            case 9:
              return (object) "DELIVERY_MODE_9";
            default:
              throw new NotSupportedException("Unknown value for cfg_device_mode!");
          }
        case EDCL_Params.device_status:
          ushort parameterValue = this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
          return parameterValue == (ushort) 0 ? (object) string.Empty : (object) ((DeviceStatus) parameterValue).ToString();
        case EDCL_Params.lora_adr:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.lora_adr))
          {
            case 0:
              return (object) false;
            case 1:
              return (object) true;
            default:
              throw new NotSupportedException("Unknown value for lora_adr!");
          }
        case EDCL_Params.Mbus_aes_key:
          return (object) AES.AesKeyToString(this.WorkMeter.meterMemory.GetData(p));
        case EDCL_Params.Mbus_interval:
          return (object) ((int) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.Mbus_interval) * 2);
        case EDCL_Params.cfg_scanif_cycle_lora:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_scanif_cycle_lora);
        case EDCL_Params.cfg_scanif_cycle_low:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_scanif_cycle_low);
        case EDCL_Params.cfg_scanif_cycle_nom:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_scanif_cycle_nom);
        case EDCL_Params.cfg_scanif_noflow_timeout:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_scanif_noflow_timeout);
        case EDCL_Params.cfg_scanif_period_nom:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_scanif_period_nom);
        case EDCL_Params.cfg_pulse_block_limit:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_pulse_block_limit);
        case EDCL_Params.cfg_pulse_leak_limit:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_pulse_leak_limit);
        case EDCL_Params.cfg_pulse_unleak_limit:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_pulse_unleak_limit);
        case EDCL_Params.cfg_pulse_leak_lower:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<short>(EDCL_Params.cfg_pulse_leak_lower);
        case EDCL_Params.cfg_pulse_leak_upper:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<short>(EDCL_Params.cfg_pulse_leak_upper);
        case EDCL_Params.cfg_pulse_back_limit:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_pulse_back_limit);
        case EDCL_Params.cfg_pulse_unback_limit:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_pulse_unback_limit);
        case EDCL_Params.cfg_oversize_diff:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_oversize_diff);
        case EDCL_Params.cfg_oversize_limit:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_oversize_limit);
        case EDCL_Params.cfg_undersize_diff:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_undersize_diff);
        case EDCL_Params.cfg_undersize_limit:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_undersize_limit);
        case EDCL_Params.cfg_burst_diff:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_burst_diff);
        case EDCL_Params.cfg_burst_limit:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_burst_limit);
        case EDCL_Params.cfg_min_difference_from_max_undamped_coil_counter:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<short>(EDCL_Params.cfg_min_difference_from_max_undamped_coil_counter);
        case EDCL_Params.cfg_max_difference_from_max_undamped_coil_counter:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<short>(EDCL_Params.cfg_max_difference_from_max_undamped_coil_counter);
        case EDCL_Params.cfg_overstep:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_overstep);
        case EDCL_Params.cfg_max_a_b_pulse_diff:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_max_a_b_pulse_diff);
        case EDCL_Params.cfg_max_b_a_pulse_diff:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_max_b_a_pulse_diff);
        case EDCL_Params.cfg_min_undamped_coil_pulses:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_min_undamped_coil_pulses);
        case EDCL_Params.rtc_sys_time:
          return (object) Utility.ConvertToDateTime_SystemTime64(this.WorkMeter.meterMemory.GetData(EDCL_Params.rtc_sys_time), 0);
        case EDCL_Params.cfg_AccessRadioKey:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<uint>(EDCL_Params.cfg_AccessRadioKey);
        case EDCL_Params.radio_center_frequency:
          return (object) ((double) this.WorkMeter.meterMemory.GetParameterValue<uint>(EDCL_Params.radio_center_frequency) / 1000000.0);
        case EDCL_Params.FD_Mbus_interval:
          return (object) ((int) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.FD_Mbus_interval) * 2);
        case EDCL_Params.cfg_min_dac_offset:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_min_dac_offset);
        case EDCL_Params.cfg_max_dac_offset:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_max_dac_offset);
        case EDCL_Params.Mbus_radio_suppression_days:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.Mbus_radio_suppression_days);
        case EDCL_Params.Mbus_nighttime_start:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.Mbus_nighttime_start);
        case EDCL_Params.Mbus_nighttime_stop:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.Mbus_nighttime_stop);
        case EDCL_Params.cfg_RadioOperationState:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_RadioOperationState))
          {
            case 0:
              return (object) false;
            case 1:
              return (object) true;
            default:
              throw new NotSupportedException("Unknown value for RadioOperation!");
          }
        case EDCL_Params.cfg_start_litre_diff:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_start_litre_diff);
        case EDCL_Params.VIF:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.VIF);
        case EDCL_Params.cfg_NBIOT_ServerIP:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte[]>(EDCL_Params.cfg_NBIOT_ServerIP);
        case EDCL_Params.cfg_NBIOT_ServerPort:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte[]>(EDCL_Params.cfg_NBIOT_ServerPort);
        case EDCL_Params.cfg_pulse_coefficient:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_pulse_coefficient);
        default:
          throw new NotImplementedException("Get Parameter Not Defined:" + p.ToString());
      }
    }

    internal SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(int subDevice)
    {
      EDCL_DeviceMemory edclDeviceMemory = this.WorkMeter != null ? this.WorkMeter.meterMemory : throw new ArgumentNullException("WorkMeter");
      FirmwareType type = (FirmwareType) this.WorkMeter.deviceIdentification.FirmwareVersionObj.Type;
      SortedList<OverrideID, ConfigurationParameter> r1 = new SortedList<OverrideID, ConfigurationParameter>();
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4;
      if (type.ToString().Contains("915"))
      {
        flag4 = true;
        flag3 = true;
      }
      else
      {
        TransmissionScenario parameter = (TransmissionScenario) this.GetParameter(EDCL_Params.cfg_transmission_scenario);
        int num;
        switch (parameter)
        {
          case TransmissionScenario.Scenario1:
          case TransmissionScenario.Scenario2:
          case TransmissionScenario.Scenario3:
            num = 1;
            break;
          default:
            if (type != FirmwareType.micro_LoRa)
            {
              num = type == FirmwareType.micro_LoRa_LL ? 1 : 0;
              break;
            }
            goto case TransmissionScenario.Scenario1;
        }
        flag4 = num != 0;
        flag1 = parameter == TransmissionScenario.WMBusFormatA || parameter == TransmissionScenario.WMBusFormatB || parameter == TransmissionScenario.WMBusFormatC || type == FirmwareType.micro_wMBus || type == FirmwareType.EDC_wMBus_ST || type == FirmwareType.micro_wMBus_LL;
        flag2 = parameter == TransmissionScenario.NBIoTScenario1 || parameter == TransmissionScenario.NBIoTScenario2 || parameter == TransmissionScenario.NBIoTScenario3 || parameter == TransmissionScenario.NBIoTScenario4 || parameter == TransmissionScenario.NBIoTScenario5 || parameter == TransmissionScenario.NBIoTScenario6;
      }
      FirmwareVersion firmwareVersionObj = this.WorkMeter.deviceIdentification.FirmwareVersionObj;
      int major = (int) firmwareVersionObj.Major;
      firmwareVersionObj = this.WorkMeter.deviceIdentification.FirmwareVersionObj;
      int minor = (int) firmwareVersionObj.Minor;
      firmwareVersionObj = this.WorkMeter.deviceIdentification.FirmwareVersionObj;
      int revision = (int) firmwareVersionObj.Revision;
      Version version = new Version(major, minor, revision);
      bool flag5 = ((type == FirmwareType.EDC_wMBus_ST || type == FirmwareType.micro_wMBus || type == FirmwareType.micro_wMBus_LL || type == FirmwareType.EDC_LoRa || type == FirmwareType.EDC_LoRa_470Mhz || type == FirmwareType.EDC_LoRa_915MHz || type == FirmwareType.micro_LoRa || type == FirmwareType.micro_LoRa_LL || type == FirmwareType.EDC_LoRa_868_v3 ? (version >= Version.Parse("1.69.0") ? 1 : 0) : 0) | (flag3 ? 1 : 0)) != 0;
      switch (subDevice)
      {
        case 0:
          SortedList<OverrideID, ConfigurationParameter> r2 = r1;
          firmwareVersionObj = this.WorkMeter.deviceIdentification.FirmwareVersionObj;
          string str1 = firmwareVersionObj.ToString();
          EDCL_AllMeters.AddParam(false, r2, OverrideID.FirmwareVersion, (object) str1);
          EDCL_AllMeters.AddParam(false, r1, OverrideID.PrintedSerialNumber, (object) this.WorkMeter.deviceIdentification.PrintedSerialNumberAsString);
          if (!flag2 && !flag3)
          {
            byte num1 = 0;
            byte num2 = 0;
            try
            {
              num1 = this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.protocol_radio_version_major);
            }
            catch
            {
            }
            try
            {
              num2 = this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.protocol_radio_version_minor);
            }
            catch
            {
            }
            string str2 = Convert.ToString(num1);
            string str3 = Convert.ToString(num2);
            EDCL_AllMeters.AddParam(false, r1, OverrideID.RadioVersion, (object) string.Format("{0}.{1}", (object) str2, (object) str3));
          }
          EDCL_AllMeters.AddParam(false, r1, OverrideID.WarningInfo, this.GetParameter(EDCL_Params.device_status));
          EDCL_AllMeters.AddParam(false, r1, OverrideID.DeviceMode, this.GetParameter(EDCL_Params.cfg_device_mode));
          EDCL_AllMeters.AddParam(false, r1, OverrideID.RadioEnabled, this.GetParameter(EDCL_Params.cfg_RadioOperationState));
          if (flag5 && this.Perform_GetCommunicationScenario.HasValue)
          {
            if (this.Perform_SetCommunicationScenario.HasValue)
              EDCL_AllMeters.AddParam(true, r1, OverrideID.CommunicationScenario, (object) this.Perform_SetCommunicationScenario);
            else
              EDCL_AllMeters.AddParam(true, r1, OverrideID.CommunicationScenario, (object) this.Perform_GetCommunicationScenario);
          }
          if (flag1)
          {
            EDCL_AllMeters.AddParam(false, r1, OverrideID.RadioTechnology, (object) RadioTechnology.wMBus);
            EDCL_AllMeters.AddParam(false, r1, OverrideID.SerialNumber, (object) this.WorkMeter.deviceIdentification.ID_BCD_AsString);
            EDCL_AllMeters.AddParam(false, r1, OverrideID.SerialNumberFull, (object) this.WorkMeter.deviceIdentification.FullSerialNumber);
            EDCL_AllMeters.AddParam(false, r1, OverrideID.Manufacturer, MBusUtil.GetManufacturer(this.WorkMeter.deviceIdentification.Manufacturer));
            EDCL_AllMeters.AddParam(false, r1, OverrideID.MBusGeneration, (object) this.WorkMeter.deviceIdentification.Generation);
            EDCL_AllMeters.AddParam(false, r1, OverrideID.Medium, (object) this.WorkMeter.deviceIdentification.Medium);
            if (!flag5 && this.WorkMeter.meterMemory.IsParameterAvailable(EDCL_Params.Mbus_interval))
              EDCL_AllMeters.AddParam(true, r1, OverrideID.RadioSendInterval, this.GetParameter(EDCL_Params.Mbus_interval));
            if (this.WorkMeter.meterMemory.IsParameterAvailable(EDCL_Params.Mbus_aes_key))
              EDCL_AllMeters.AddParam(true, r1, OverrideID.AESKey, this.GetParameter(EDCL_Params.Mbus_aes_key));
            if (!flag5 && this.WorkMeter.meterMemory.IsParameterAvailable(EDCL_Params.Mbus_radio_suppression_days))
            {
              byte parameterValue1 = this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.Mbus_radio_suppression_days);
              byte num = this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.Mbus_nighttime_start);
              byte parameterValue2 = this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.Mbus_nighttime_stop);
              if (num == (byte) 0)
                num = (byte) 24;
              EDCL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveMonday, (object) !Convert.ToBoolean((int) parameterValue1 & 1));
              EDCL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveTuesday, (object) !Convert.ToBoolean((int) parameterValue1 & 2));
              EDCL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveWednesday, (object) !Convert.ToBoolean((int) parameterValue1 & 4));
              EDCL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveThursday, (object) !Convert.ToBoolean((int) parameterValue1 & 8));
              EDCL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveFriday, (object) !Convert.ToBoolean((int) parameterValue1 & 16));
              EDCL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveSaturday, (object) !Convert.ToBoolean((int) parameterValue1 & 32));
              EDCL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveSunday, (object) !Convert.ToBoolean((int) parameterValue1 & 64));
              EDCL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveStartTime, (object) parameterValue2);
              EDCL_AllMeters.AddParam(true, r1, OverrideID.RadioActiveStopTime, (object) num);
            }
          }
          if (flag4)
          {
            EDCL_AllMeters.AddParam(false, r1, OverrideID.RadioTechnology, (object) RadioTechnology.LoRa);
            EDCL_AllMeters.AddParam(false, r1, OverrideID.NetID, (object) edclDeviceMemory.GetParameterValue<uint>(EDCL_Params.cfg_loraWan_netid));
            if (!flag3)
            {
              EDCL_AllMeters.AddParam(false, r1, OverrideID.LoRaWanVersion, (object) string.Format("{0}.{1}.{2}", (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.protocol_lorawan_version_major), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.protocol_lorawan_version_middle), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.protocol_lorawan_version_minor)));
              EDCL_AllMeters.AddParam(false, r1, OverrideID.LoRaVersion, (object) string.Format("{0}.{1}", (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.protocol_lora_version_major), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.protocol_lora_version_minor)));
              EDCL_AllMeters.AddParam(true, r1, OverrideID.NwkSKey, (object) edclDeviceMemory.GetData(EDCL_Params.cfb_lora_nwkskey));
              EDCL_AllMeters.AddParam(true, r1, OverrideID.AppSKey, (object) edclDeviceMemory.GetData(EDCL_Params.cfb_lora_appskey));
            }
            EDCL_AllMeters.AddParam(true, r1, OverrideID.JoinEUI, (object) this.WorkMeter.deviceIdentification.LoRa_JoinEUI);
            EDCL_AllMeters.AddParam(true, r1, OverrideID.DevEUI, (object) this.WorkMeter.deviceIdentification.LoRa_DevEUI);
            EDCL_AllMeters.AddParam(true, r1, OverrideID.AppKey, (object) this.WorkMeter.deviceIdentification.LoRa_AppKey);
            EDCL_AllMeters.AddParam(true, r1, OverrideID.DevAddr, (object) edclDeviceMemory.GetParameterValue<uint>(EDCL_Params.cfg_lora_device_id));
            EDCL_AllMeters.AddParam(true, r1, OverrideID.Activation, this.GetParameter(EDCL_Params.cfg_otaa_abp_mode), false, new string[2]
            {
              "OTAA",
              "ABP"
            }, string.Empty);
            EDCL_AllMeters.AddParam(true, r1, OverrideID.SendJoinRequest, (object) this.Perform_SendJoinRequest, true, (string[]) null, string.Empty);
            EDCL_AllMeters.AddParam(true, r1, OverrideID.ADR, this.GetParameter(EDCL_Params.lora_adr));
          }
          if (this.WorkMeter.meterMemory.IsParameterAvailable(EDCL_Params.VIF))
          {
            EDCL_AllMeters.AddParam(true, r1, OverrideID.VIF, this.GetParameter(EDCL_Params.VIF));
            Debug.WriteLine("Vif: " + this.GetParameter(EDCL_Params.VIF).ToString());
          }
          if (this.WorkMeter.meterMemory.IsParameterAvailable(EDCL_Params.rtc_sys_time))
            EDCL_AllMeters.AddParam(false, r1, OverrideID.DeviceClock, this.GetParameter(EDCL_Params.rtc_sys_time));
          HardwareStatus parameterValue = (HardwareStatus) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.hwStatusFlags);
          EDCL_AllMeters.AddParam(false, r1, OverrideID.Manipulation, (object) (parameterValue == HardwareStatus.TAMPER));
          EDCL_AllMeters.AddParam(false, r1, OverrideID.DeviceHasError, (object) (parameterValue == HardwareStatus.COILFAIL));
          if (!flag2)
            EDCL_AllMeters.AddParam(false, r1, OverrideID.Frequence, this.GetParameter(EDCL_Params.radio_center_frequency), false, (string[]) null, "MHz");
          EDCL_AllMeters.AddParam(true, r1, OverrideID.RegisterDigits, this.GetParameter(EDCL_Params.cfg_cog_count));
          EDCL_AllMeters.AddParam(true, r1, OverrideID.PulseMultiplier, (object) Convert.ToDecimal(this.GetParameter(EDCL_Params.cfg_pulse_multiplier)));
          if (!flag5)
            EDCL_AllMeters.AddParam(true, r1, OverrideID.TransmissionScenario, this.GetParameter(EDCL_Params.cfg_transmission_scenario));
          EDCL_AllMeters.AddParam(true, r1, OverrideID.DueDate, this.GetParameter(EDCL_Params.cfg_Key_date));
          EDCL_AllMeters.AddParam(true, r1, OverrideID.EndOfBatteryDate, this.GetParameter(EDCL_Params.cfg_battery_end_life_date));
          EDCL_AllMeters.AddParam(true, r1, OverrideID.SetPcTime, (object) this.Perform_SetPcTime, true, (string[]) null, string.Empty);
          EDCL_AllMeters.AddParam(true, r1, OverrideID.SetOperatingMode, (object) this.Perform_SetOperatingMode, true, (string[]) null, string.Empty);
          EDCL_AllMeters.AddParam(true, r1, OverrideID.TimeZone, (object) Convert.ToDecimal(this.GetParameter(EDCL_Params.Bak_TimeZoneInQuarterHours)));
          NominalFlow nominalFlow = this.GetNominalFlow();
          EDCL_AllMeters.AddParam(true, r1, OverrideID.NominalFlow, (object) nominalFlow, false, nominalFlow.AllowedValues, "m^3/h");
          EDCL_AllMeters.AddParam(true, r1, OverrideID.TotalVolumePulses, this.GetParameter(EDCL_Params.pulseReading));
          EDCL_AllMeters.AddParam(true, r1, OverrideID.ClearManipulation, (object) false, true, (string[]) null, string.Empty);
          EDCL_AllMeters.AddParam(true, r1, OverrideID.ClearWarnings, (object) this.Perform_ClearWarnings, true, (string[]) null, string.Empty);
          if (((type == FirmwareType.EDC_LoRa || type == FirmwareType.EDC_LoRa_470Mhz || type == FirmwareType.EDC_LoRa_915MHz || type == FirmwareType.EDC_wMBus_ST || type == FirmwareType.micro_LoRa || type == FirmwareType.micro_LoRa_LL || type == FirmwareType.micro_wMBus || type == FirmwareType.micro_wMBus_LL || type == FirmwareType.EDC_LoRa_868_v3 ? (version >= Version.Parse("1.41.0") ? 1 : 0) : 0) | (flag3 ? 1 : 0)) != 0)
          {
            EDCL_AllMeters.AddParam(true, r1, OverrideID.PulseBlockLimit, this.GetParameter(EDCL_Params.cfg_pulse_block_limit));
            EDCL_AllMeters.AddParam(true, r1, OverrideID.PulseLeakLimit, this.GetParameter(EDCL_Params.cfg_pulse_leak_limit));
            EDCL_AllMeters.AddParam(true, r1, OverrideID.PulseUnleakLimit, this.GetParameter(EDCL_Params.cfg_pulse_unleak_limit));
            EDCL_AllMeters.AddParam(true, r1, OverrideID.PulseLeakLower, this.GetParameter(EDCL_Params.cfg_pulse_leak_lower));
            EDCL_AllMeters.AddParam(true, r1, OverrideID.PulseLeakUpper, this.GetParameter(EDCL_Params.cfg_pulse_leak_upper));
            EDCL_AllMeters.AddParam(true, r1, OverrideID.PulseBackLimit, this.GetParameter(EDCL_Params.cfg_pulse_back_limit));
            EDCL_AllMeters.AddParam(true, r1, OverrideID.PulseUnbackLimit, this.GetParameter(EDCL_Params.cfg_pulse_unback_limit));
            EDCL_AllMeters.AddParam(true, r1, OverrideID.OversizeDiff, this.GetParameter(EDCL_Params.cfg_oversize_diff));
            EDCL_AllMeters.AddParam(true, r1, OverrideID.OversizeLimit, this.GetParameter(EDCL_Params.cfg_oversize_limit));
            EDCL_AllMeters.AddParam(true, r1, OverrideID.UndersizeDiff, this.GetParameter(EDCL_Params.cfg_undersize_diff));
            EDCL_AllMeters.AddParam(true, r1, OverrideID.UndersizeLimit, this.GetParameter(EDCL_Params.cfg_undersize_limit));
            EDCL_AllMeters.AddParam(true, r1, OverrideID.BurstDiff, this.GetParameter(EDCL_Params.cfg_burst_diff));
            EDCL_AllMeters.AddParam(true, r1, OverrideID.BurstLimit, this.GetParameter(EDCL_Params.cfg_burst_limit));
            EDCL_AllMeters.AddParam(true, r1, OverrideID.ActivateSmartFunctions, (object) false, true, (string[]) null, string.Empty);
            bool flag6 = nominalFlow != null && !string.IsNullOrEmpty(nominalFlow.GetNominalFlow()) && (ushort) this.GetParameter(EDCL_Params.cfg_pulse_block_limit) == (ushort) 2880 && (ushort) this.GetParameter(EDCL_Params.cfg_pulse_leak_limit) == (ushort) 96 && (ushort) this.GetParameter(EDCL_Params.cfg_pulse_unleak_limit) == (ushort) 8 && (short) this.GetParameter(EDCL_Params.cfg_pulse_leak_lower) == (short) -1900 && (short) this.GetParameter(EDCL_Params.cfg_pulse_leak_upper) == (short) 1900 && (ushort) this.GetParameter(EDCL_Params.cfg_pulse_back_limit) == (ushort) 48 && (ushort) this.GetParameter(EDCL_Params.cfg_pulse_unback_limit) == (ushort) 20 && (ushort) this.GetParameter(EDCL_Params.cfg_oversize_limit) == (ushort) 2880 && (ushort) this.GetParameter(EDCL_Params.cfg_undersize_limit) == (ushort) 24 && (ushort) this.GetParameter(EDCL_Params.cfg_burst_limit) == (ushort) 2;
            EDCL_AllMeters.AddParam(false, r1, OverrideID.SmartFunctionsActivated, (object) flag6);
          }
          int num3;
          if (flag2)
          {
            switch (type)
            {
              case FirmwareType.EDC_NBIoT:
                num3 = version >= Version.Parse("2.5.0") ? 1 : 0;
                break;
              case FirmwareType.EDC_NBIoT_XM:
              case FirmwareType.EDC_NBIoT_Israel:
              case FirmwareType.EDC_NBIoT_TaiWan:
                num3 = 1;
                break;
              default:
                num3 = 0;
                break;
            }
          }
          else
            num3 = 0;
          if (num3 != 0)
          {
            EDCL_AllMeters.AddParam(true, r1, OverrideID.Coefficient, this.GetParameter(EDCL_Params.cfg_pulse_coefficient));
            break;
          }
          break;
        case 1:
          EDCL_AllMeters.AddParam(true, r1, OverrideID.SerialNumberFull, (object) this.WorkMeter.deviceIdentification1.FullSerialNumber);
          if (flag1)
          {
            byte? medium = this.WorkMeter.deviceIdentification1.Medium;
            int? nullable = medium.HasValue ? new int?((int) medium.GetValueOrDefault()) : new int?();
            int maxValue = (int) byte.MaxValue;
            if (nullable.GetValueOrDefault() == maxValue & nullable.HasValue)
              this.WorkMeter.deviceIdentification1.Medium = new byte?((byte) 7);
            EDCL_AllMeters.AddParam(true, r1, OverrideID.Manufacturer, MBusUtil.GetManufacturer(this.WorkMeter.deviceIdentification1.Manufacturer));
            EDCL_AllMeters.AddParam(true, r1, OverrideID.MBusGeneration, (object) this.WorkMeter.deviceIdentification1.Generation);
            EDCL_AllMeters.AddParam(true, r1, OverrideID.Medium, (object) this.WorkMeter.deviceIdentification1.GetMediumAsText(), false, new string[5]
            {
              MBusDeviceType.WATER.ToString(),
              MBusDeviceType.COLD_WATER.ToString(),
              MBusDeviceType.HOT_AND_COLD_WATER.ToString(),
              MBusDeviceType.HOT_WATER.ToString(),
              MBusDeviceType.HOT_WATER_90.ToString()
            }, (string) null);
            break;
          }
          break;
        default:
          return (SortedList<OverrideID, ConfigurationParameter>) null;
      }
      return r1;
    }

    private NominalFlow GetNominalFlow()
    {
      return new NominalFlow(this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_pulse_multiplier), this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_oversize_diff), this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_undersize_diff), this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_burst_diff));
    }

    internal void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameter,
      int subDevice)
    {
      if (parameter == null || parameter.Count == 0)
        return;
      EDCL_DeviceMemory meterMemory = this.WorkMeter.meterMemory;
      switch (subDevice)
      {
        case 0:
          if (parameter.ContainsKey(OverrideID.NominalFlow))
          {
            ConfigurationParameter configurationParameter = parameter[OverrideID.NominalFlow];
            if (!string.IsNullOrEmpty(configurationParameter.ParameterValue as string))
            {
              NominalFlow nominalFlow = this.GetNominalFlow();
              nominalFlow.SetNominalFlow(configurationParameter.ParameterValue as string);
              this.SetParameter(EDCL_Params.cfg_oversize_diff, (object) nominalFlow.OversizeDiff);
              this.SetParameter(EDCL_Params.cfg_undersize_diff, (object) nominalFlow.UndersizeDiff);
              this.SetParameter(EDCL_Params.cfg_burst_diff, (object) nominalFlow.BurstDiff);
            }
          }
          foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in parameter)
          {
            if (keyValuePair.Key == OverrideID.TransmissionScenario)
              this.SetParameter(EDCL_Params.cfg_transmission_scenario, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.DueDate)
              this.SetParameter(EDCL_Params.cfg_Key_date, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.EndOfBatteryDate)
              this.SetParameter(EDCL_Params.cfg_battery_end_life_date, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.Activation)
              this.SetParameter(EDCL_Params.cfg_otaa_abp_mode, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.DevEUI)
              meterMemory.SetParameterValue<ulong>(EDCL_Params.cfg_lora_deveui, (ulong) keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.JoinEUI)
              meterMemory.SetParameterValue<ulong>(EDCL_Params.cfg_lora_appeui, (ulong) keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.AppKey)
              meterMemory.SetData(EDCL_Params.cfb_lora_AppKey, (byte[]) keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.NwkSKey)
              meterMemory.SetData(EDCL_Params.cfb_lora_nwkskey, (byte[]) keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.AppSKey)
              meterMemory.SetData(EDCL_Params.cfb_lora_appskey, (byte[]) keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.DevAddr)
              meterMemory.SetParameterValue<uint>(EDCL_Params.cfg_lora_device_id, (uint) keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.TimeZone)
              this.SetParameter(EDCL_Params.Bak_TimeZoneInQuarterHours, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.ADR)
              this.SetParameter(EDCL_Params.lora_adr, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.SetPcTime)
              this.Perform_SetPcTime = Convert.ToBoolean(keyValuePair.Value.ParameterValue.ToString());
            else if (keyValuePair.Key == OverrideID.SendJoinRequest)
              this.Perform_SendJoinRequest = Convert.ToBoolean(keyValuePair.Value.ParameterValue.ToString());
            else if (keyValuePair.Key == OverrideID.SetOperatingMode)
              this.Perform_SetOperatingMode = Convert.ToBoolean(keyValuePair.Value.ParameterValue.ToString());
            else if (keyValuePair.Key == OverrideID.ClearWarnings)
              this.Perform_ClearWarnings = Convert.ToBoolean(keyValuePair.Value.ParameterValue.ToString());
            else if (keyValuePair.Key == OverrideID.DeviceHasError)
              this.SetParameter(EDCL_Params.hwStatusFlags, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.RegisterDigits)
              this.SetParameter(EDCL_Params.cfg_cog_count, (object) keyValuePair.Value.ParameterValue.ToString());
            else if (keyValuePair.Key == OverrideID.PulseMultiplier)
              this.SetParameter(EDCL_Params.cfg_pulse_multiplier, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.Coefficient)
              this.SetParameter(EDCL_Params.cfg_pulse_coefficient, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.AESKey)
              this.SetParameter(EDCL_Params.Mbus_aes_key, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.RadioSendInterval)
              this.SetParameter(EDCL_Params.Mbus_interval, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.PulseBlockLimit)
              this.SetParameter(EDCL_Params.cfg_pulse_block_limit, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.PulseLeakLimit)
              this.SetParameter(EDCL_Params.cfg_pulse_leak_limit, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.PulseUnleakLimit)
              this.SetParameter(EDCL_Params.cfg_pulse_unleak_limit, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.PulseLeakLower)
              this.SetParameter(EDCL_Params.cfg_pulse_leak_lower, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.PulseLeakUpper)
              this.SetParameter(EDCL_Params.cfg_pulse_leak_upper, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.PulseBackLimit)
              this.SetParameter(EDCL_Params.cfg_pulse_back_limit, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.PulseUnbackLimit)
              this.SetParameter(EDCL_Params.cfg_pulse_unback_limit, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.OversizeDiff)
              this.SetParameter(EDCL_Params.cfg_oversize_diff, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.OversizeLimit)
              this.SetParameter(EDCL_Params.cfg_oversize_limit, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.UndersizeDiff)
              this.SetParameter(EDCL_Params.cfg_undersize_diff, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.UndersizeLimit)
              this.SetParameter(EDCL_Params.cfg_undersize_limit, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.BurstDiff)
              this.SetParameter(EDCL_Params.cfg_burst_diff, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.VIF)
              this.SetParameter(EDCL_Params.VIF, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.BurstLimit)
              this.SetParameter(EDCL_Params.cfg_burst_limit, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.TotalVolumePulses)
              this.Perform_SetMeterValue = new uint?(Convert.ToUInt32(keyValuePair.Value.ParameterValue));
            else if (keyValuePair.Key == OverrideID.CommunicationScenario)
              this.Perform_SetCommunicationScenario = new int?((int) Convert.ToUInt16(keyValuePair.Value.ParameterValue));
            else if (keyValuePair.Key == OverrideID.ClearManipulation)
              this.SetParameter(EDCL_Params.hwStatusFlags, (object) (HardwareStatus) ((int) this.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.hwStatusFlags) & 64511));
            else if (keyValuePair.Key == OverrideID.ActivateSmartFunctions)
            {
              byte parameterValue = this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_pulse_multiplier);
              NominalFlow nominalFlow = this.GetNominalFlow();
              double result = 0.0;
              if (nominalFlow != null && double.TryParse(nominalFlow.ToString(), out result))
                this.SetSmartFunctions(new double?(result * 1000.0), new byte?(parameterValue), true, true, true, true, true, true);
              else
                this.SetSmartFunctions(new double?(0.0), new byte?(parameterValue), true, true, true, true, true, true);
            }
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
              this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.Mbus_nighttime_stop, theValue);
            }
            else if (keyValuePair.Key == OverrideID.RadioActiveStopTime)
            {
              byte theValue = Convert.ToByte(keyValuePair.Value.ParameterValue);
              if (theValue <= (byte) 1 || theValue > (byte) 24)
                throw new ArgumentOutOfRangeException("RadioActiveStopTime");
              if (theValue == (byte) 24)
                theValue = (byte) 0;
              this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.Mbus_nighttime_start, theValue);
            }
          }
          if (parameter.ContainsKey(OverrideID.LeakDetectionOn) && !Convert.ToBoolean(parameter[OverrideID.LeakDetectionOn].ParameterValue))
            this.SetParameter(EDCL_Params.cfg_pulse_leak_limit, (object) 0);
          if (parameter.ContainsKey(OverrideID.BackflowDetectionOn) && !Convert.ToBoolean(parameter[OverrideID.BackflowDetectionOn].ParameterValue))
            this.SetParameter(EDCL_Params.cfg_pulse_back_limit, (object) 0);
          if (parameter.ContainsKey(OverrideID.BurstDetectionOn) && !Convert.ToBoolean(parameter[OverrideID.BurstDetectionOn].ParameterValue))
            this.SetParameter(EDCL_Params.cfg_burst_limit, (object) 0);
          if (parameter.ContainsKey(OverrideID.StandstillDetectionOn) && !Convert.ToBoolean(parameter[OverrideID.StandstillDetectionOn].ParameterValue))
            this.SetParameter(EDCL_Params.cfg_pulse_block_limit, (object) 0);
          if (parameter.ContainsKey(OverrideID.UndersizeDetectionOn) && !Convert.ToBoolean(parameter[OverrideID.UndersizeDetectionOn].ParameterValue))
            this.SetParameter(EDCL_Params.cfg_undersize_limit, (object) 0);
          if (!parameter.ContainsKey(OverrideID.OversizeDetectionOn) || Convert.ToBoolean(parameter[OverrideID.OversizeDetectionOn].ParameterValue))
            break;
          this.SetParameter(EDCL_Params.cfg_oversize_limit, (object) 0);
          break;
        case 1:
          foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in parameter)
          {
            if (keyValuePair.Key == OverrideID.SerialNumberFull)
              this.WorkMeter.deviceIdentification1.FullSerialNumber = keyValuePair.Value.ParameterValue.ToString();
            else if (keyValuePair.Key == OverrideID.Medium)
              this.WorkMeter.deviceIdentification1.Medium = new byte?((byte) Enum.Parse(typeof (MBusDeviceType), keyValuePair.Value.GetStringValueWin()));
            else if (keyValuePair.Key == OverrideID.Manufacturer)
              this.WorkMeter.deviceIdentification1.Manufacturer = new ushort?(MBusUtil.GetManufacturerCode(keyValuePair.Value.GetStringValueWin()));
            else if (keyValuePair.Key == OverrideID.MBusGeneration)
              this.WorkMeter.deviceIdentification1.Generation = new byte?(byte.Parse(keyValuePair.Value.GetStringValueWin()));
          }
          break;
      }
    }

    private void SetMbus_radio_suppression_days(byte mask, bool isSet)
    {
      byte parameterValue = this.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.Mbus_radio_suppression_days);
      this.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.Mbus_radio_suppression_days, isSet ? (byte) ((uint) parameterValue & (uint) ~mask) : (byte) ((uint) parameterValue | (uint) mask));
    }

    internal SortedList<long, SortedList<DateTime, double>> GetValues()
    {
      return new SortedList<long, SortedList<DateTime, double>>();
    }

    internal void CompareConnectedAndWork()
    {
      this.WorkMeter.meterMemory.CompareParameterInfo("EDCL_DeviceMemory", "WorkMeter object", "ConnectedMeter object", (DeviceMemory) this.ConnectedMeter.meterMemory);
    }

    private static void AddParam(
      bool canChanged,
      SortedList<OverrideID, ConfigurationParameter> r,
      OverrideID overrideID,
      object obj)
    {
      EDCL_AllMeters.AddParam(canChanged, r, overrideID, obj, false, (string[]) null, string.Empty);
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
      if (!UserManager.IsConfigParamVisible(overrideID))
        return;
      bool flag = false;
      if (canChanged)
        flag = UserManager.IsConfigParamEditable(overrideID);
      r.Add(overrideID, new ConfigurationParameter(overrideID, obj)
      {
        HasWritePermission = flag,
        IsFunction = isFunction,
        AllowedValues = allowedValues,
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

    public void OpenType(int meterInfoID)
    {
      BaseType baseType = BaseType.GetBaseType(meterInfoID);
      if (baseType == null || baseType.Data == null)
        return;
      this.TypeMeter = (EDCL_Meter) this.CreateMeter(baseType.Data.EEPdata);
      this.TypeMeter.BaseType = baseType;
      if (this.WorkMeter == null)
        this.WorkMeter = this.TypeMeter.Clone();
      else
        this.WorkMeter.BaseType = this.TypeMeter.BaseType.DeepCopy();
    }

    internal void SaveMeterObject(HandlerMeterObjects meterObject)
    {
      switch (meterObject)
      {
        case HandlerMeterObjects.WorkMeter:
          this.SavedMeter = this.WorkMeter;
          break;
        case HandlerMeterObjects.ConnectedMeter:
          this.SavedMeter = this.ConnectedMeter;
          break;
        case HandlerMeterObjects.TypeMeter:
          this.SavedMeter = this.TypeMeter;
          break;
        case HandlerMeterObjects.BackupMeter:
          this.SavedMeter = this.BackupMeter;
          break;
        default:
          throw new Exception("Illegal meter object for SaveMeterObject");
      }
    }

    public string Create(BaseTables.MeterDataRow meterData)
    {
      if (!Enum.IsDefined(typeof (GmmDbLib.MeterData.Special), (object) meterData.PValueID) || meterData.PValueID != 60000)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      byte[] buffer = Util.Unzip(meterData.PValueBinary);
      this.EncabulatorData = new List<VolumeMonitorEventArgs>();
      int offset = 0;
      while (offset < buffer.Length)
      {
        VolumeMonitorEventArgs monitorEventArgs = VolumeMonitorEventArgs.Parse(buffer, ref offset);
        this.EncabulatorData.Add(monitorEventArgs);
        stringBuilder.AppendLine(monitorEventArgs.ToString());
      }
      return stringBuilder.ToString();
    }

    public bool SetSmartFunctions(
      double? waterMeterNominalFlow,
      byte? pulseMultiplier,
      bool isEnabledLeakDetection,
      bool isEnabledBurstDetection,
      bool isEnabledBackflowDetection,
      bool isEnabledStandstillDetection,
      bool isEnabledUndersizeDetection,
      bool isEnabledOversizeDetection)
    {
      if (waterMeterNominalFlow.HasValue)
      {
        double? nullable1 = waterMeterNominalFlow;
        double num1 = 0.1;
        double? nullable2 = nullable1;
        double? nullable3 = nullable2.HasValue ? new double?(num1 * nullable2.GetValueOrDefault()) : new double?();
        byte? nullable4 = pulseMultiplier;
        double? nullable5;
        if (!nullable4.HasValue)
        {
          nullable2 = new double?();
          nullable5 = nullable2;
        }
        else
          nullable5 = new double?((double) nullable4.GetValueOrDefault());
        double? nullable6 = nullable5;
        double? nullable7;
        if (!(nullable3.HasValue & nullable6.HasValue))
        {
          nullable2 = new double?();
          nullable7 = nullable2;
        }
        else
          nullable7 = new double?(nullable3.GetValueOrDefault() / nullable6.GetValueOrDefault());
        double? nullable8 = nullable7;
        double num2 = 4.0;
        double? nullable9;
        if (!nullable8.HasValue)
        {
          nullable6 = new double?();
          nullable9 = nullable6;
        }
        else
          nullable9 = new double?(nullable8.GetValueOrDefault() / num2);
        if (!this.SetOversizeDiff(Convert.ToUInt16((object) nullable9)))
          return false;
        nullable6 = nullable1;
        nullable4 = pulseMultiplier;
        double? nullable10;
        if (!nullable4.HasValue)
        {
          nullable2 = new double?();
          nullable10 = nullable2;
        }
        else
          nullable10 = new double?((double) nullable4.GetValueOrDefault());
        nullable3 = nullable10;
        double? nullable11;
        if (!(nullable6.HasValue & nullable3.HasValue))
        {
          nullable2 = new double?();
          nullable11 = nullable2;
        }
        else
          nullable11 = new double?(nullable6.GetValueOrDefault() / nullable3.GetValueOrDefault());
        nullable8 = nullable11;
        double num3 = 4.0;
        double? nullable12;
        if (!nullable8.HasValue)
        {
          nullable3 = new double?();
          nullable12 = nullable3;
        }
        else
          nullable12 = new double?(nullable8.GetValueOrDefault() / num3);
        if (!this.SetUndersizeDiff(Convert.ToUInt16((object) nullable12)))
          return false;
        double num4 = 0.3;
        nullable2 = nullable1;
        nullable3 = nullable2.HasValue ? new double?(num4 * nullable2.GetValueOrDefault()) : new double?();
        nullable4 = pulseMultiplier;
        double? nullable13;
        if (!nullable4.HasValue)
        {
          nullable2 = new double?();
          nullable13 = nullable2;
        }
        else
          nullable13 = new double?((double) nullable4.GetValueOrDefault());
        nullable6 = nullable13;
        double? nullable14;
        if (!(nullable3.HasValue & nullable6.HasValue))
        {
          nullable2 = new double?();
          nullable14 = nullable2;
        }
        else
          nullable14 = new double?(nullable3.GetValueOrDefault() / nullable6.GetValueOrDefault());
        nullable8 = nullable14;
        double num5 = 4.0;
        double? nullable15;
        if (!nullable8.HasValue)
        {
          nullable6 = new double?();
          nullable15 = nullable6;
        }
        else
          nullable15 = new double?(nullable8.GetValueOrDefault() / num5);
        if (!this.SetBurstDiff(Convert.ToUInt16((object) nullable15)))
          return false;
      }
      else if (!this.SetOversizeDiff((ushort) 62) || !this.SetUndersizeDiff((ushort) 625) || !this.SetBurstDiff((ushort) 187))
        return false;
      return this.SetPulseBlockLimit(2880) && this.SetPulseLeakLimit(96) && this.SetPulseUnleakLimit(8) && this.SetPulseLeakLower(-1900) && this.SetPulseLeakUpper(1900) && this.SetPulseBackLimit(48) && this.SetPulseUnbackLimit(20) && this.SetOversizeLimit(2880) && this.SetUndersizeLimit(24) && this.SetBurstLimit(2) && (isEnabledLeakDetection || this.DisableLeakDetection()) && (isEnabledBurstDetection || this.DisableBurstDetection()) && (isEnabledBackflowDetection || this.DisableBackflowDetection()) && (isEnabledStandstillDetection || this.DisableStandstillDetection()) && (isEnabledUndersizeDetection || this.DisableUndersizeDetection()) && (isEnabledOversizeDetection || this.DisableOversizeDetection());
    }

    private bool DisableOversizeDetection()
    {
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_oversize_limit, Convert.ToUInt16(0));
      return true;
    }

    private bool DisableUndersizeDetection()
    {
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_undersize_limit, Convert.ToUInt16(0));
      return true;
    }

    private bool DisableStandstillDetection()
    {
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_block_limit, Convert.ToUInt16(0));
      return true;
    }

    private bool DisableBackflowDetection()
    {
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_back_limit, Convert.ToUInt16(0));
      return true;
    }

    private bool DisableBurstDetection()
    {
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_burst_limit, Convert.ToUInt16(0));
      return true;
    }

    private bool DisableLeakDetection()
    {
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_leak_limit, Convert.ToUInt16(0));
      this.WorkMeter.meterMemory.SetParameterValue<short>(EDCL_Params.cfg_pulse_leak_lower, Convert.ToInt16(0));
      this.WorkMeter.meterMemory.SetParameterValue<short>(EDCL_Params.cfg_pulse_leak_upper, Convert.ToInt16(0));
      return true;
    }

    private bool SetBurstLimit(int value)
    {
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_burst_limit, Convert.ToUInt16(value));
      return true;
    }

    private bool SetUndersizeLimit(int value)
    {
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_undersize_limit, Convert.ToUInt16(value));
      return true;
    }

    private bool SetOversizeLimit(int value)
    {
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_oversize_limit, Convert.ToUInt16(value));
      return true;
    }

    private bool SetPulseUnbackLimit(int value)
    {
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_unback_limit, Convert.ToUInt16(value));
      return true;
    }

    private bool SetPulseBackLimit(int value)
    {
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_back_limit, Convert.ToUInt16(value));
      return true;
    }

    private bool SetPulseLeakUpper(int value)
    {
      this.WorkMeter.meterMemory.SetParameterValue<short>(EDCL_Params.cfg_pulse_leak_upper, Convert.ToInt16(value));
      return true;
    }

    private bool SetPulseLeakLower(int value)
    {
      this.WorkMeter.meterMemory.SetParameterValue<short>(EDCL_Params.cfg_pulse_leak_lower, Convert.ToInt16(value));
      return true;
    }

    private bool SetPulseUnleakLimit(int value)
    {
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_unleak_limit, Convert.ToUInt16(value));
      return true;
    }

    private bool SetPulseLeakLimit(int value)
    {
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_leak_limit, Convert.ToUInt16(value));
      return true;
    }

    private bool SetPulseBlockLimit(int value)
    {
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_block_limit, Convert.ToUInt16(value));
      return true;
    }

    private bool SetBurstDiff(ushort value)
    {
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_burst_diff, Convert.ToUInt16(value));
      return true;
    }

    private bool SetUndersizeDiff(ushort value)
    {
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_undersize_diff, Convert.ToUInt16(value));
      return true;
    }

    private bool SetOversizeDiff(ushort value)
    {
      this.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_oversize_diff, Convert.ToUInt16(value));
      return true;
    }
  }
}
