// Decompiled with JetBrains decompiler
// Type: PDCL2_Handler.PDCL2_AllMeters
// Assembly: PDCL2_Handler, Version=2.22.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 03BA4C2D-69FE-4DA6-9C3F-B3D5471C4058
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDCL2_Handler.dll

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
namespace PDCL2_Handler
{
  internal class PDCL2_AllMeters : ICreateMeter, IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (PDCL2_AllMeters));
    private PDCL2_HandlerFunctions functions;
    private uint? Perform_SetMeterValueA = new uint?();
    private uint? Perform_SetMeterValueB = new uint?();
    private bool Perform_SetPcTime = false;
    private bool Perform_SendJoinRequest = false;
    private bool Perform_SetOperatingMode = false;
    private bool Perform_ClearWarnings = false;
    private int? Perform_GetCommunicationScenario = new int?();
    private int? Perform_SetCommunicationScenario = new int?();
    internal PDCL2_Meter ConnectedMeter;
    internal PDCL2_Meter TypeMeter;
    internal PDCL2_Meter BackupMeter;
    internal PDCL2_Meter WorkMeter;
    internal PDCL2_Meter SavedMeter;
    public static readonly string[] InputResolutionValues = new string[26]
    {
      "0.000m\u00B3",
      "0.00m\u00B3",
      "0.0m\u00B3",
      "0m\u00B3",
      "0.000MWh",
      "0.00MWh",
      "0.000kWh",
      "0.00kWh",
      "0.0kWh",
      "0kWh",
      "0.000Wh",
      "0.00Wh",
      "0.0Wh",
      "0Wh",
      "0.000GJ",
      "0.00GJ",
      "0MJ",
      "0.000L",
      "0.00L",
      "0.0L",
      "0L",
      "0.0000",
      "0.000",
      "0.00",
      "0.0",
      "0"
    };

    internal PDCL2_Meter checkedWorkMeter
    {
      get => this.WorkMeter != null ? this.WorkMeter : throw new Exception("WorkMeter not defined");
    }

    internal PDCL2_Meter checkedChangeMeter
    {
      get
      {
        if (this.ConnectedMeter == null)
          throw new Exception("ConnectedMeter not available");
        return this.WorkMeter != null ? this.WorkMeter : throw new Exception("WorkMeter not available");
      }
    }

    internal PDCL2_AllMeters()
    {
    }

    internal PDCL2_AllMeters(PDCL2_HandlerFunctions functions) => this.functions = functions;

    public void Dispose()
    {
      this.ConnectedMeter = (PDCL2_Meter) null;
      this.WorkMeter = (PDCL2_Meter) null;
      this.TypeMeter = (PDCL2_Meter) null;
      this.BackupMeter = (PDCL2_Meter) null;
    }

    public IMeter CreateMeter(byte[] zippedBuffer)
    {
      return new PDCL2_Meter().CreateFromData(zippedBuffer);
    }

    public string SetBackupMeter(byte[] zippedBuffer)
    {
      this.BackupMeter = this.CreateMeter(zippedBuffer) as PDCL2_Meter;
      if (this.WorkMeter == null)
        this.WorkMeter = this.BackupMeter.Clone();
      return this.BackupMeter.GetInfo();
    }

    public async Task ConnectDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      DeviceVersionMBus deviceVersion = await this.functions.cmd.ReadVersionAsync(progress, token);
      this.ConnectedMeter = new PDCL2_Meter((DeviceIdentification) deviceVersion);
      this.WorkMeter = this.ConnectedMeter.Clone();
      this.Perform_SetPcTime = false;
      this.Perform_SendJoinRequest = false;
      this.Perform_SetOperatingMode = false;
      this.Perform_SetMeterValueA = new uint?();
      this.Perform_SetMeterValueB = new uint?();
      this.Perform_ClearWarnings = false;
      this.Perform_GetCommunicationScenario = new int?();
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
      if (type != FirmwareType.PDC_LoRa && type != FirmwareType.UDC_LoRa_915 && type != FirmwareType.PDC_LoRa_868MHz_SD && type != FirmwareType.PDC_GAS && type != FirmwareType.PDC_LoRa_915)
        throw new Exception(Ot.Gtt(Tg.Common, "NotPDC", "Connected device is not PDC/UDC."));
      this.ConnectedMeter = new PDCL2_Meter(deviceIdentification);
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
      {
        this.ConnectedMeter.meterMemory.GarantMemoryAvailable(range);
        await this.functions.cmd.Device.ReadMemoryAsync(range, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, token);
      }
      this.WorkMeter = this.ConnectedMeter.Clone();
      this.Perform_SetPcTime = false;
      this.Perform_SendJoinRequest = false;
      this.Perform_SetOperatingMode = false;
      this.Perform_SetMeterValueA = new uint?();
      this.Perform_SetMeterValueB = new uint?();
      this.Perform_ClearWarnings = false;
      this.Perform_SetCommunicationScenario = new int?();
      this.Perform_GetCommunicationScenario = new int?();
      deviceIdentification = (DeviceIdentification) null;
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
        await this.functions.ResetDeviceAsync(progress, token);
        this.functions.Clear();
      }
      DeviceVersionMBus deviceVersionMbus = await this.functions.cmd.ReadVersionAsync(progress, token);
      if (this.Perform_ClearWarnings)
      {
        await this.functions.cmd.Special.ClearFlowCheckStatesAsync((ushort) byte.MaxValue, progress, token);
        this.Perform_ClearWarnings = false;
      }
      if (this.Perform_SetPcTime)
      {
        double timezone = Convert.ToDouble(this.GetParameter(PDCL2_Params.Bak_TimeZoneInQuarterHours));
        DateTime utc_plus_timezone = DateTime.UtcNow.AddHours(timezone);
        await this.functions.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(utc_plus_timezone, (sbyte) 0), progress, token);
        this.Perform_SetPcTime = false;
      }
      bool doReset = false;
      if (this.Perform_SetMeterValueA.HasValue)
      {
        await this.functions.cmd.MBusCmd.SetChannelValueAsync((byte) 1, this.Perform_SetMeterValueA.Value, progress, token);
        doReset = true;
        this.Perform_SetMeterValueA = new uint?();
      }
      if (this.Perform_SetCommunicationScenario.HasValue && this.Perform_SetCommunicationScenario.Value != this.Perform_GetCommunicationScenario.Value)
      {
        byte[] scenario = BitConverter.GetBytes(Convert.ToUInt16(this.Perform_SetCommunicationScenario.Value));
        await this.functions.cmd.Device.SetCommunicationScenarioAsync(scenario, progress, token);
        await this.functions.ResetDeviceAsync(progress, token);
        this.Perform_SetCommunicationScenario = new int?();
        scenario = (byte[]) null;
      }
      if (this.Perform_SetMeterValueB.HasValue)
      {
        await this.functions.cmd.MBusCmd.SetChannelValueAsync((byte) 2, this.Perform_SetMeterValueB.Value, progress, token);
        doReset = true;
        this.Perform_SetMeterValueB = new uint?();
      }
      if (doReset)
        await this.functions.ResetDeviceAsync(progress, token);
      if (this.Perform_SendJoinRequest)
      {
        await this.functions.cmd.LoRa.SendJoinRequestAsync(progress, token);
        this.Perform_SendJoinRequest = false;
      }
      if (!this.Perform_SetOperatingMode)
      {
        ranges = (List<AddressRange>) null;
      }
      else
      {
        await this.functions.cmd.Device.SetModeAsync((byte) 0, progress, token);
        this.Perform_SetOperatingMode = false;
        ranges = (List<AddressRange>) null;
      }
    }

    public async Task<byte[]> ReadParameterAsync(
      ProgressHandler progress,
      CancellationToken token,
      PDCL2_Params p)
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
      PDCL2_Params p,
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

    public void SetParameter(PDCL2_Params p, object value)
    {
      if (value == null)
        return;
      switch (p)
      {
        case PDCL2_Params.Bak_TimeZoneInQuarterHours:
          this.WorkMeter.meterMemory.SetParameterValue<sbyte>(PDCL2_Params.Bak_TimeZoneInQuarterHours, Convert.ToSByte(Convert.ToDouble(value) * 4.0));
          break;
        case PDCL2_Params.cfb_lora_AppKey:
        case PDCL2_Params.cfb_lora_appskey:
        case PDCL2_Params.cfb_lora_nwkskey:
        case PDCL2_Params.cfg_lora_device_id:
          this.WorkMeter.meterMemory.SetData(p, Util.HexStringToByteArray(value.ToString()));
          break;
        case PDCL2_Params.cfg_Key_date:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_Key_date, MBusUtil.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(Convert.ToDateTime(value)));
          break;
        case PDCL2_Params.cfg_battery_end_life_date:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_battery_end_life_date, MBusUtil.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(Convert.ToDateTime(value)));
          break;
        case PDCL2_Params.cfg_otaa_abp_mode:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(PDCL2_Params.cfg_otaa_abp_mode, value.ToString() == "OTAA" ? (byte) 1 : (byte) 2);
          break;
        case PDCL2_Params.cfg_transmission_scenario:
          if (Convert.ToByte(value) > (byte) 2)
            throw new NotSupportedException("Only scenario 1(201) or 2 (202) possible!");
          this.WorkMeter.meterMemory.SetParameterValue<byte>(PDCL2_Params.cfg_transmission_scenario, Convert.ToByte(value));
          break;
        case PDCL2_Params.lora_adr:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(PDCL2_Params.lora_adr, Convert.ToBoolean(value.ToString()) ? (byte) 1 : (byte) 0);
          break;
        case PDCL2_Params.cfg_burst_diff_chA:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_burst_diff_chA, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_burst_diff_chB:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_burst_diff_chB, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_burst_limit_chA:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_burst_limit_chA, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_burst_limit_chB:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_burst_limit_chB, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_oversize_diff_chA:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_oversize_diff_chA, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_oversize_diff_chB:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_oversize_diff_chB, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_pulse_back_limit_chA:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_pulse_back_limit_chA, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_pulse_back_limit_chB:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_pulse_back_limit_chB, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_pulse_block_limit_chA:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_pulse_block_limit_chA, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_pulse_block_limit_chB:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_pulse_block_limit_chB, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_pulse_leak_limit_chA:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_pulse_leak_limit_chA, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_pulse_leak_limit_chB:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_pulse_leak_limit_chB, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_pulse_leak_lower_chA:
          this.WorkMeter.meterMemory.SetParameterValue<short>(PDCL2_Params.cfg_pulse_leak_lower_chA, Convert.ToInt16(value));
          break;
        case PDCL2_Params.cfg_pulse_leak_lower_chB:
          this.WorkMeter.meterMemory.SetParameterValue<short>(PDCL2_Params.cfg_pulse_leak_lower_chB, Convert.ToInt16(value));
          break;
        case PDCL2_Params.cfg_pulse_leak_upper_chA:
          this.WorkMeter.meterMemory.SetParameterValue<short>(PDCL2_Params.cfg_pulse_leak_upper_chA, Convert.ToInt16(value));
          break;
        case PDCL2_Params.cfg_pulse_leak_upper_chB:
          this.WorkMeter.meterMemory.SetParameterValue<short>(PDCL2_Params.cfg_pulse_leak_upper_chB, Convert.ToInt16(value));
          break;
        case PDCL2_Params.cfg_pulse_unback_limit_chA:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_pulse_unback_limit_chA, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_pulse_unback_limit_chB:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_pulse_unback_limit_chB, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_pulse_unleak_limit_chA:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_pulse_unleak_limit_chA, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_pulse_unleak_limit_chB:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_pulse_unleak_limit_chB, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_undersize_diff_chA:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_undersize_diff_chA, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_undersize_diff_chB:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_undersize_diff_chB, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_oversize_limit_chA:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_oversize_limit_chA, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_oversize_limit_chB:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_oversize_limit_chB, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_undersize_limit_chA:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_undersize_limit_chA, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.cfg_undersize_limit_chB:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.cfg_undersize_limit_chB, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.hwStatusFlagsA:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.hwStatusFlagsA, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.hwStatusFlagsB:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.hwStatusFlagsB, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.persistent_warning_flagsA:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.persistent_warning_flagsA, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.persistent_warning_flagsB:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.persistent_warning_flagsB, Convert.ToUInt16(value));
          break;
        case PDCL2_Params.ChannelA_VIF:
          ResolutionData resolutionData1 = MeterUnits.GetResolutionData(value.ToString());
          if (resolutionData1 != null)
          {
            this.WorkMeter.meterMemory.SetParameterValue<byte>(PDCL2_Params.ChannelA_VIF, Convert.ToByte((byte) resolutionData1.mbusVIF));
            break;
          }
          this.WorkMeter.meterMemory.SetParameterValue<byte>(PDCL2_Params.ChannelA_VIF, byte.MaxValue);
          break;
        case PDCL2_Params.ChannelB_VIF:
          ResolutionData resolutionData2 = MeterUnits.GetResolutionData(value.ToString());
          if (resolutionData2 != null)
          {
            this.WorkMeter.meterMemory.SetParameterValue<byte>(PDCL2_Params.ChannelB_VIF, Convert.ToByte((byte) resolutionData2.mbusVIF));
            break;
          }
          this.WorkMeter.meterMemory.SetParameterValue<byte>(PDCL2_Params.ChannelB_VIF, byte.MaxValue);
          break;
        case PDCL2_Params.cfg_channel_B_available:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(PDCL2_Params.cfg_channel_B_available, Convert.ToByte(value));
          break;
        default:
          throw new NotImplementedException(p.ToString());
      }
    }

    public object GetParameter(PDCL2_Params p)
    {
      switch (p)
      {
        case PDCL2_Params.Bak_TimeZoneInQuarterHours:
          return (object) ((double) this.WorkMeter.meterMemory.GetParameterValue<sbyte>(PDCL2_Params.Bak_TimeZoneInQuarterHours) / 4.0);
        case PDCL2_Params.cfb_lora_AppKey:
        case PDCL2_Params.cfb_lora_appskey:
        case PDCL2_Params.cfb_lora_nwkskey:
        case PDCL2_Params.cfg_lora_device_id:
          return (object) Utility.ByteArrayToHexString(this.WorkMeter.meterMemory.GetData(p));
        case PDCL2_Params.cfg_Key_date:
          return (object) Util.ConvertToDate_MBus_CP16_TypeG(BitConverter.GetBytes(this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_Key_date)), 0);
        case PDCL2_Params.cfg_RadioOperationState:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(PDCL2_Params.cfg_RadioOperationState))
          {
            case 0:
              return (object) false;
            case 1:
              return (object) true;
            default:
              throw new NotSupportedException("Unknown value for RadioOperation!");
          }
        case PDCL2_Params.cfg_battery_end_life_date:
          return (object) Util.ConvertToDate_MBus_CP16_TypeG(BitConverter.GetBytes(this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_battery_end_life_date)), 0);
        case PDCL2_Params.cfg_otaa_abp_mode:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(PDCL2_Params.cfg_otaa_abp_mode))
          {
            case 1:
              return (object) "OTAA";
            case 2:
              return (object) "ABP";
            default:
              return (object) string.Empty;
          }
        case PDCL2_Params.cfg_transmission_scenario:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(PDCL2_Params.cfg_transmission_scenario);
        case PDCL2_Params.device_status:
          ushort parameterValue = this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.device_status);
          return parameterValue == (ushort) 0 ? (object) string.Empty : (object) ((DeviceStatus) parameterValue).ToString();
        case PDCL2_Params.lora_adr:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(PDCL2_Params.lora_adr))
          {
            case 0:
              return (object) false;
            case 1:
              return (object) true;
            default:
              throw new NotSupportedException("Unknown value for lora_adr!");
          }
        case PDCL2_Params.radio_center_frequency:
          return (object) ((double) this.WorkMeter.meterMemory.GetParameterValue<uint>(PDCL2_Params.radio_center_frequency) / 1000000.0);
        case PDCL2_Params.cfg_burst_diff_chA:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_burst_diff_chA);
        case PDCL2_Params.cfg_burst_diff_chB:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_burst_diff_chB);
        case PDCL2_Params.cfg_burst_limit_chA:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_burst_limit_chA);
        case PDCL2_Params.cfg_burst_limit_chB:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_burst_limit_chB);
        case PDCL2_Params.cfg_oversize_diff_chA:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_oversize_diff_chA);
        case PDCL2_Params.cfg_oversize_diff_chB:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_oversize_diff_chB);
        case PDCL2_Params.cfg_pulse_back_limit_chA:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_pulse_back_limit_chA);
        case PDCL2_Params.cfg_pulse_back_limit_chB:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_pulse_back_limit_chB);
        case PDCL2_Params.cfg_pulse_block_limit_chA:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_pulse_block_limit_chA);
        case PDCL2_Params.cfg_pulse_block_limit_chB:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_pulse_block_limit_chB);
        case PDCL2_Params.cfg_pulse_leak_limit_chA:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_pulse_leak_limit_chA);
        case PDCL2_Params.cfg_pulse_leak_limit_chB:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_pulse_leak_limit_chB);
        case PDCL2_Params.cfg_pulse_leak_lower_chA:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<short>(PDCL2_Params.cfg_pulse_leak_lower_chA);
        case PDCL2_Params.cfg_pulse_leak_lower_chB:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<short>(PDCL2_Params.cfg_pulse_leak_lower_chB);
        case PDCL2_Params.cfg_pulse_leak_upper_chA:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<short>(PDCL2_Params.cfg_pulse_leak_upper_chA);
        case PDCL2_Params.cfg_pulse_leak_upper_chB:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<short>(PDCL2_Params.cfg_pulse_leak_upper_chB);
        case PDCL2_Params.cfg_pulse_unback_limit_chA:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_pulse_unback_limit_chA);
        case PDCL2_Params.cfg_pulse_unback_limit_chB:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_pulse_unback_limit_chB);
        case PDCL2_Params.cfg_pulse_unleak_limit_chA:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_pulse_unleak_limit_chA);
        case PDCL2_Params.cfg_pulse_unleak_limit_chB:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_pulse_unleak_limit_chB);
        case PDCL2_Params.cfg_undersize_diff_chA:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_undersize_diff_chA);
        case PDCL2_Params.cfg_undersize_diff_chB:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_undersize_diff_chB);
        case PDCL2_Params.cfg_oversize_limit_chA:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_oversize_limit_chA);
        case PDCL2_Params.cfg_oversize_limit_chB:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_oversize_limit_chB);
        case PDCL2_Params.cfg_undersize_limit_chA:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_undersize_limit_chA);
        case PDCL2_Params.cfg_undersize_limit_chB:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.cfg_undersize_limit_chB);
        case PDCL2_Params.hwStatusFlagsA:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.hwStatusFlagsA);
        case PDCL2_Params.hwStatusFlagsB:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.hwStatusFlagsB);
        case PDCL2_Params.persistent_warning_flagsA:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<uint>(PDCL2_Params.persistent_warning_flagsA);
        case PDCL2_Params.persistent_warning_flagsB:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<uint>(PDCL2_Params.persistent_warning_flagsB);
        case PDCL2_Params.ChannelA_VIF:
          ResolutionData resolutionData1 = MeterUnits.GetResolutionData(this.WorkMeter.meterMemory.GetParameterValue<byte>(PDCL2_Params.ChannelA_VIF));
          return resolutionData1 == null ? (object) string.Empty : (object) resolutionData1.resolutionString;
        case PDCL2_Params.ChannelB_VIF:
          ResolutionData resolutionData2 = MeterUnits.GetResolutionData(this.WorkMeter.meterMemory.GetParameterValue<byte>(PDCL2_Params.ChannelB_VIF));
          return resolutionData2 == null ? (object) string.Empty : (object) resolutionData2.resolutionString;
        case PDCL2_Params.cfg_AccessRadioKey:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<uint>(PDCL2_Params.cfg_AccessRadioKey);
        case PDCL2_Params.cfg_device_mode:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(PDCL2_Params.cfg_device_mode))
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
        case PDCL2_Params.rtc_sys_time:
          return (object) Utility.ConvertToDateTime_SystemTime64(this.WorkMeter.meterMemory.GetData(PDCL2_Params.rtc_sys_time), 0);
        case PDCL2_Params.cfg_channel_B_available:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(PDCL2_Params.cfg_channel_B_available))
          {
            case 0:
              return (object) false;
            case 1:
              return (object) true;
            default:
              throw new NotSupportedException("Unknown value for cfg_channel_B_available!");
          }
        case PDCL2_Params.tamper_detect_level:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(PDCL2_Params.tamper_detect_level);
        default:
          throw new NotImplementedException(p.ToString());
      }
    }

    internal SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(int subDevice)
    {
      PDCL2_DeviceMemory pdcL2DeviceMemory = this.WorkMeter != null ? this.WorkMeter.meterMemory : throw new ArgumentNullException("WorkMeter");
      SortedList<OverrideID, ConfigurationParameter> r = new SortedList<OverrideID, ConfigurationParameter>();
      TransmissionScenario parameter = (TransmissionScenario) this.GetParameter(PDCL2_Params.cfg_transmission_scenario);
      int num1;
      switch (parameter)
      {
        case TransmissionScenario.Scenario1:
        case TransmissionScenario.Scenario2:
          num1 = 1;
          break;
        default:
          num1 = parameter == TransmissionScenario.Scenario3 ? 1 : 0;
          break;
      }
      bool flag1 = num1 != 0;
      Version version = new Version((int) this.WorkMeter.deviceIdentification.FirmwareVersionObj.Major, (int) this.WorkMeter.deviceIdentification.FirmwareVersionObj.Minor, (int) this.WorkMeter.deviceIdentification.FirmwareVersionObj.Revision);
      FirmwareType type = (FirmwareType) this.WorkMeter.deviceIdentification.FirmwareVersionObj.Type;
      int num2;
      switch (type)
      {
        case FirmwareType.PDC_LoRa:
        case FirmwareType.PDC_LoRa_915:
        case FirmwareType.PDC_LoRa_868MHz_SD:
          num2 = version >= Version.Parse("2.22.0") ? 1 : 0;
          break;
        default:
          num2 = 0;
          break;
      }
      bool flag2 = num2 != 0;
      switch (subDevice)
      {
        case 0:
          PDCL2_AllMeters.AddParam(false, r, OverrideID.FirmwareVersion, (object) this.WorkMeter.deviceIdentification.FirmwareVersionObj.ToString());
          PDCL2_AllMeters.AddParam(false, r, OverrideID.PrintedSerialNumber, (object) this.WorkMeter.deviceIdentification.PrintedSerialNumberAsString);
          PDCL2_AllMeters.AddParam(false, r, OverrideID.Medium, (object) (MBusDeviceType) this.WorkMeter.deviceIdentification.Medium.Value);
          PDCL2_AllMeters.AddParam(false, r, OverrideID.RadioVersion, (object) string.Format("{0}.{1}", (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(PDCL2_Params.protocol_radio_version_major), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(PDCL2_Params.protocol_radio_version_minor)));
          PDCL2_AllMeters.AddParam(false, r, OverrideID.WarningInfo, this.GetParameter(PDCL2_Params.device_status));
          PDCL2_AllMeters.AddParam(false, r, OverrideID.DeviceMode, this.GetParameter(PDCL2_Params.cfg_device_mode));
          PDCL2_AllMeters.AddParam(false, r, OverrideID.RadioEnabled, this.GetParameter(PDCL2_Params.cfg_RadioOperationState));
          if (flag2 && this.Perform_GetCommunicationScenario.HasValue)
          {
            if (this.Perform_SetCommunicationScenario.HasValue)
              PDCL2_AllMeters.AddParam(true, r, OverrideID.CommunicationScenario, (object) this.Perform_SetCommunicationScenario);
            else
              PDCL2_AllMeters.AddParam(true, r, OverrideID.CommunicationScenario, (object) this.Perform_GetCommunicationScenario);
          }
          if (flag1)
          {
            PDCL2_AllMeters.AddParam(false, r, OverrideID.RadioTechnology, (object) RadioTechnology.LoRa);
            PDCL2_AllMeters.AddParam(false, r, OverrideID.NetID, (object) pdcL2DeviceMemory.GetParameterValue<uint>(PDCL2_Params.cfg_loraWan_netid));
            PDCL2_AllMeters.AddParam(false, r, OverrideID.LoRaWanVersion, (object) string.Format("{0}.{1}.{2}", (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(PDCL2_Params.protocol_lorawan_version_major), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(PDCL2_Params.protocol_lorawan_version_middle), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(PDCL2_Params.protocol_lorawan_version_minor)));
            PDCL2_AllMeters.AddParam(false, r, OverrideID.LoRaVersion, (object) string.Format("{0}.{1}", (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(PDCL2_Params.protocol_lora_version_major), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(PDCL2_Params.protocol_lora_version_minor)));
            if ((this.WorkMeter.meterMemory.FirmwareVersion == 26U || this.WorkMeter.meterMemory.FirmwareVersion > 34734106U) && type != FirmwareType.UDC_LoRa_915)
            {
              uint parameterValue1 = this.WorkMeter.meterMemory.GetParameterValue<uint>(PDCL2_Params.cfg_lora_basic_frequency1);
              uint parameterValue2 = this.WorkMeter.meterMemory.GetParameterValue<uint>(PDCL2_Params.cfg_lora_basic_frequency2);
              uint parameterValue3 = this.WorkMeter.meterMemory.GetParameterValue<uint>(PDCL2_Params.cfg_lora_basic_frequency3);
              uint parameterValue4 = this.WorkMeter.meterMemory.GetParameterValue<uint>(PDCL2_Params.cfg_lora_rx_freq_window2);
              if (parameterValue1 == 868100000U && parameterValue2 == 868300000U && parameterValue3 == 868500000U && parameterValue4 == 869525000U)
                PDCL2_AllMeters.AddParam(true, r, OverrideID.Region, (object) Region.EU_863_870);
              else if (parameterValue1 == 868900000U && parameterValue2 == 869100000U && parameterValue3 == 864100000U && parameterValue4 == 869100000U)
                PDCL2_AllMeters.AddParam(true, r, OverrideID.Region, (object) Region.RU_864_870);
              else
                PDCL2_AllMeters.AddParam(true, r, OverrideID.Region, (object) null);
            }
            PDCL2_AllMeters.AddParam(true, r, OverrideID.JoinEUI, (object) this.WorkMeter.deviceIdentification.LoRa_JoinEUI);
            PDCL2_AllMeters.AddParam(true, r, OverrideID.DevEUI, (object) this.WorkMeter.deviceIdentification.LoRa_DevEUI);
            PDCL2_AllMeters.AddParam(true, r, OverrideID.AppKey, (object) this.WorkMeter.deviceIdentification.LoRa_AppKey);
            PDCL2_AllMeters.AddParam(true, r, OverrideID.NwkSKey, (object) pdcL2DeviceMemory.GetData(PDCL2_Params.cfb_lora_nwkskey));
            PDCL2_AllMeters.AddParam(true, r, OverrideID.AppSKey, (object) pdcL2DeviceMemory.GetData(PDCL2_Params.cfb_lora_appskey));
            PDCL2_AllMeters.AddParam(true, r, OverrideID.DevAddr, (object) pdcL2DeviceMemory.GetParameterValue<uint>(PDCL2_Params.cfg_lora_device_id));
            PDCL2_AllMeters.AddParam(true, r, OverrideID.Activation, this.GetParameter(PDCL2_Params.cfg_otaa_abp_mode), false, new string[2]
            {
              "OTAA",
              "ABP"
            }, string.Empty);
            PDCL2_AllMeters.AddParam(true, r, OverrideID.SendJoinRequest, (object) this.Perform_SendJoinRequest, true, (string[]) null, string.Empty);
            PDCL2_AllMeters.AddParam(true, r, OverrideID.ADR, this.GetParameter(PDCL2_Params.lora_adr));
          }
          if (this.WorkMeter.meterMemory.IsParameterAvailable(PDCL2_Params.rtc_sys_time))
            PDCL2_AllMeters.AddParam(false, r, OverrideID.DeviceClock, this.GetParameter(PDCL2_Params.rtc_sys_time));
          PDCL2_AllMeters.AddParam(false, r, OverrideID.Frequence, this.GetParameter(PDCL2_Params.radio_center_frequency), false, (string[]) null, "MHz");
          if (!flag2)
            PDCL2_AllMeters.AddParam(true, r, OverrideID.TransmissionScenario, this.GetParameter(PDCL2_Params.cfg_transmission_scenario));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.DueDate, this.GetParameter(PDCL2_Params.cfg_Key_date));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.EndOfBatteryDate, this.GetParameter(PDCL2_Params.cfg_battery_end_life_date));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.SetPcTime, (object) this.Perform_SetPcTime, true, (string[]) null, string.Empty);
          PDCL2_AllMeters.AddParam(true, r, OverrideID.SetOperatingMode, (object) this.Perform_SetOperatingMode, true, (string[]) null, string.Empty);
          PDCL2_AllMeters.AddParam(true, r, OverrideID.TimeZone, (object) Convert.ToDecimal(this.GetParameter(PDCL2_Params.Bak_TimeZoneInQuarterHours)));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.ClearWarnings, (object) this.Perform_ClearWarnings, true, (string[]) null, string.Empty);
          break;
        case 1:
          PDCL2_AllMeters.AddParam(true, r, OverrideID.SerialNumberFull, (object) this.WorkMeter.deviceIdentification1.FullSerialNumber);
          PDCL2_AllMeters.AddParam(true, r, OverrideID.Medium, (object) (MBusDeviceType) this.WorkMeter.deviceIdentification1.Medium.Value);
          PDCL2_AllMeters.AddParam(true, r, OverrideID.OversizeLimit, this.GetParameter(PDCL2_Params.cfg_oversize_limit_chA));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.UndersizeLimit, this.GetParameter(PDCL2_Params.cfg_undersize_limit_chA));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.PulseBlockLimit, this.GetParameter(PDCL2_Params.cfg_pulse_block_limit_chA));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.PulseLeakLimit, this.GetParameter(PDCL2_Params.cfg_pulse_leak_limit_chA));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.PulseUnleakLimit, this.GetParameter(PDCL2_Params.cfg_pulse_unleak_limit_chA));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.PulseLeakLower, this.GetParameter(PDCL2_Params.cfg_pulse_leak_lower_chA));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.PulseLeakUpper, this.GetParameter(PDCL2_Params.cfg_pulse_leak_upper_chA));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.OversizeDiff, this.GetParameter(PDCL2_Params.cfg_oversize_diff_chA));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.UndersizeDiff, this.GetParameter(PDCL2_Params.cfg_undersize_diff_chA));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.BurstDiff, this.GetParameter(PDCL2_Params.cfg_burst_diff_chA));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.BurstLimit, this.GetParameter(PDCL2_Params.cfg_burst_limit_chA));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.PulseBackLimit, this.GetParameter(PDCL2_Params.cfg_pulse_back_limit_chA));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.InputResolutionStr, this.GetParameter(PDCL2_Params.ChannelA_VIF), false, PDCL2_AllMeters.InputResolutionValues, string.Empty);
          sbyte parameterValue5 = this.WorkMeter.meterMemory.GetParameterValue<sbyte>(PDCL2_Params.ChannelA_Mbus_Exponent);
          double num3 = (double) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.ChannelA_Mbus_Mantissa) * Math.Pow(10.0, (double) parameterValue5);
          PDCL2_AllMeters.AddParam(true, r, OverrideID.InputPulsValue, (object) num3);
          uint parameterValue6 = this.WorkMeter.meterMemory.GetParameterValue<uint>(PDCL2_Params.pulseReadingChannelA);
          PDCL2_AllMeters.AddParam(true, r, OverrideID.TotalPulse, (object) ((double) parameterValue6 / num3));
          break;
        case 2:
          PDCL2_AllMeters.AddParam(true, r, OverrideID.SerialNumberFull, (object) this.WorkMeter.deviceIdentification2.FullSerialNumber);
          PDCL2_AllMeters.AddParam(true, r, OverrideID.Medium, (object) (MBusDeviceType) this.WorkMeter.deviceIdentification2.Medium.Value);
          PDCL2_AllMeters.AddParam(true, r, OverrideID.OversizeLimit, this.GetParameter(PDCL2_Params.cfg_oversize_limit_chB));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.UndersizeLimit, this.GetParameter(PDCL2_Params.cfg_undersize_limit_chB));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.PulseBlockLimit, this.GetParameter(PDCL2_Params.cfg_pulse_block_limit_chB));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.PulseLeakLimit, this.GetParameter(PDCL2_Params.cfg_pulse_leak_limit_chB));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.PulseUnleakLimit, this.GetParameter(PDCL2_Params.cfg_pulse_unleak_limit_chB));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.PulseLeakLower, this.GetParameter(PDCL2_Params.cfg_pulse_leak_lower_chB));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.PulseLeakUpper, this.GetParameter(PDCL2_Params.cfg_pulse_leak_upper_chB));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.OversizeDiff, this.GetParameter(PDCL2_Params.cfg_oversize_diff_chB));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.UndersizeDiff, this.GetParameter(PDCL2_Params.cfg_undersize_diff_chB));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.BurstDiff, this.GetParameter(PDCL2_Params.cfg_burst_diff_chB));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.BurstLimit, this.GetParameter(PDCL2_Params.cfg_burst_limit_chB));
          PDCL2_AllMeters.AddParam(true, r, OverrideID.InputResolutionStr, this.GetParameter(PDCL2_Params.ChannelB_VIF), false, PDCL2_AllMeters.InputResolutionValues, string.Empty);
          sbyte parameterValue7 = this.WorkMeter.meterMemory.GetParameterValue<sbyte>(PDCL2_Params.ChannelB_Mbus_Exponent);
          double num4 = (double) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.ChannelB_Mbus_Mantissa) * Math.Pow(10.0, (double) parameterValue7);
          PDCL2_AllMeters.AddParam(true, r, OverrideID.InputPulsValue, (object) num4);
          uint parameterValue8 = this.WorkMeter.meterMemory.GetParameterValue<uint>(PDCL2_Params.pulseReadingChannelB);
          PDCL2_AllMeters.AddParam(true, r, OverrideID.TotalPulse, (object) ((double) parameterValue8 / num4));
          if (new Version(new FirmwareVersion(this.WorkMeter.meterMemory.FirmwareVersion).VersionString) >= new Version("2.2.1"))
          {
            PDCL2_AllMeters.AddParam(true, r, OverrideID.RadioEnabled, this.GetParameter(PDCL2_Params.cfg_channel_B_available));
            break;
          }
          break;
        default:
          return (SortedList<OverrideID, ConfigurationParameter>) null;
      }
      return r;
    }

    internal void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameter,
      int subDevice)
    {
      if (parameter == null || parameter.Count == 0)
        return;
      PDCL2_DeviceMemory meterMemory = this.WorkMeter.meterMemory;
      switch (subDevice)
      {
        case 0:
          using (IEnumerator<KeyValuePair<OverrideID, ConfigurationParameter>> enumerator = parameter.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<OverrideID, ConfigurationParameter> current = enumerator.Current;
              if (current.Key == OverrideID.TransmissionScenario)
                this.SetParameter(PDCL2_Params.cfg_transmission_scenario, current.Value.ParameterValue);
              else if (current.Key == OverrideID.DueDate)
                this.SetParameter(PDCL2_Params.cfg_Key_date, current.Value.ParameterValue);
              else if (current.Key == OverrideID.EndOfBatteryDate)
                this.SetParameter(PDCL2_Params.cfg_battery_end_life_date, current.Value.ParameterValue);
              else if (current.Key == OverrideID.Activation)
                this.SetParameter(PDCL2_Params.cfg_otaa_abp_mode, current.Value.ParameterValue);
              else if (current.Key == OverrideID.DevEUI)
                meterMemory.SetParameterValue<ulong>(PDCL2_Params.cfg_lora_deveui, (ulong) current.Value.ParameterValue);
              else if (current.Key == OverrideID.JoinEUI)
                meterMemory.SetParameterValue<ulong>(PDCL2_Params.cfg_lora_appeui, (ulong) current.Value.ParameterValue);
              else if (current.Key == OverrideID.AppKey)
                meterMemory.SetData(PDCL2_Params.cfb_lora_AppKey, (byte[]) current.Value.ParameterValue);
              else if (current.Key == OverrideID.NwkSKey)
                meterMemory.SetData(PDCL2_Params.cfb_lora_nwkskey, (byte[]) current.Value.ParameterValue);
              else if (current.Key == OverrideID.AppSKey)
                meterMemory.SetData(PDCL2_Params.cfb_lora_appskey, (byte[]) current.Value.ParameterValue);
              else if (current.Key == OverrideID.DevAddr)
                meterMemory.SetParameterValue<uint>(PDCL2_Params.cfg_lora_device_id, (uint) current.Value.ParameterValue);
              else if (current.Key == OverrideID.TimeZone)
                this.SetParameter(PDCL2_Params.Bak_TimeZoneInQuarterHours, current.Value.ParameterValue);
              else if (current.Key == OverrideID.ADR)
                this.SetParameter(PDCL2_Params.lora_adr, current.Value.ParameterValue);
              else if (current.Key == OverrideID.SetPcTime)
                this.Perform_SetPcTime = Convert.ToBoolean(current.Value.ParameterValue.ToString());
              else if (current.Key == OverrideID.SendJoinRequest)
                this.Perform_SendJoinRequest = Convert.ToBoolean(current.Value.ParameterValue.ToString());
              else if (current.Key == OverrideID.SetOperatingMode)
                this.Perform_SetOperatingMode = Convert.ToBoolean(current.Value.ParameterValue.ToString());
              else if (current.Key == OverrideID.ClearWarnings)
                this.Perform_ClearWarnings = Convert.ToBoolean(current.Value.ParameterValue.ToString());
              else if (current.Key == OverrideID.Region && (this.WorkMeter.meterMemory.FirmwareVersion == 26U || this.WorkMeter.meterMemory.FirmwareVersion > 34734106U))
              {
                if ((Region) Enum.Parse(typeof (Region), current.Value.ParameterValue.ToString()) == Region.EU_863_870)
                {
                  this.WorkMeter.meterMemory.SetParameterValue<uint>(PDCL2_Params.cfg_lora_basic_frequency1, 868100000U);
                  this.WorkMeter.meterMemory.SetParameterValue<uint>(PDCL2_Params.cfg_lora_basic_frequency2, 868300000U);
                  this.WorkMeter.meterMemory.SetParameterValue<uint>(PDCL2_Params.cfg_lora_basic_frequency3, 868500000U);
                  this.WorkMeter.meterMemory.SetParameterValue<uint>(PDCL2_Params.cfg_lora_rx_freq_window2, 869525000U);
                }
                else if ((Region) Enum.Parse(typeof (Region), current.Value.ParameterValue.ToString()) == Region.RU_864_870)
                {
                  this.WorkMeter.meterMemory.SetParameterValue<uint>(PDCL2_Params.cfg_lora_basic_frequency1, 868900000U);
                  this.WorkMeter.meterMemory.SetParameterValue<uint>(PDCL2_Params.cfg_lora_basic_frequency2, 869100000U);
                  this.WorkMeter.meterMemory.SetParameterValue<uint>(PDCL2_Params.cfg_lora_basic_frequency3, 864100000U);
                  this.WorkMeter.meterMemory.SetParameterValue<uint>(PDCL2_Params.cfg_lora_rx_freq_window2, 869100000U);
                }
                else if ((Region) Enum.Parse(typeof (Region), current.Value.ParameterValue.ToString()) != Region.US_902_928)
                  throw new NotImplementedException(current.Value.ParameterValue.ToString());
              }
            }
            break;
          }
        case 1:
          using (IEnumerator<KeyValuePair<OverrideID, ConfigurationParameter>> enumerator = parameter.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<OverrideID, ConfigurationParameter> current = enumerator.Current;
              if (current.Key == OverrideID.SerialNumberFull)
                this.WorkMeter.deviceIdentification1.FullSerialNumber = current.Value.ParameterValue.ToString();
              else if (current.Key == OverrideID.Medium)
                this.WorkMeter.deviceIdentification1.Medium = new byte?((byte) Enum.Parse(typeof (MBusDeviceType), current.Value.GetStringValueWin()));
              else if (current.Key == OverrideID.DeviceHasError)
                this.SetParameter(PDCL2_Params.hwStatusFlagsA, current.Value.ParameterValue);
              else if (current.Key == OverrideID.OversizeLimit)
                this.SetParameter(PDCL2_Params.cfg_oversize_limit_chA, current.Value.ParameterValue);
              else if (current.Key == OverrideID.UndersizeLimit)
                this.SetParameter(PDCL2_Params.cfg_undersize_limit_chA, current.Value.ParameterValue);
              else if (current.Key == OverrideID.TotalPulse)
              {
                this.Perform_SetMeterValueA = new uint?(Convert.ToUInt32(current.Value.ParameterValue));
                sbyte parameterValue = this.WorkMeter.meterMemory.GetParameterValue<sbyte>(PDCL2_Params.ChannelA_Mbus_Exponent);
                this.WorkMeter.meterMemory.SetParameterValue<uint>(PDCL2_Params.pulseReadingChannelA, Convert.ToUInt32((double) this.Perform_SetMeterValueA.Value * ((double) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.ChannelA_Mbus_Mantissa) * Math.Pow(10.0, (double) parameterValue))));
              }
              else if (current.Key == OverrideID.PulseBlockLimit)
                this.SetParameter(PDCL2_Params.cfg_pulse_block_limit_chA, current.Value.ParameterValue);
              else if (current.Key == OverrideID.PulseLeakLimit)
                this.SetParameter(PDCL2_Params.cfg_pulse_leak_limit_chA, current.Value.ParameterValue);
              else if (current.Key == OverrideID.PulseUnleakLimit)
                this.SetParameter(PDCL2_Params.cfg_pulse_unleak_limit_chA, current.Value.ParameterValue);
              else if (current.Key == OverrideID.PulseLeakLower)
                this.SetParameter(PDCL2_Params.cfg_pulse_leak_lower_chA, current.Value.ParameterValue);
              else if (current.Key == OverrideID.PulseLeakUpper)
                this.SetParameter(PDCL2_Params.cfg_pulse_leak_upper_chA, current.Value.ParameterValue);
              else if (current.Key == OverrideID.PulseBackLimit)
                this.SetParameter(PDCL2_Params.cfg_pulse_back_limit_chA, current.Value.ParameterValue);
              else if (current.Key == OverrideID.PulseUnbackLimit)
                this.SetParameter(PDCL2_Params.cfg_pulse_unback_limit_chA, current.Value.ParameterValue);
              else if (current.Key == OverrideID.OversizeDiff)
                this.SetParameter(PDCL2_Params.cfg_oversize_diff_chA, current.Value.ParameterValue);
              else if (current.Key == OverrideID.UndersizeDiff)
                this.SetParameter(PDCL2_Params.cfg_undersize_diff_chA, current.Value.ParameterValue);
              else if (current.Key == OverrideID.BurstDiff)
                this.SetParameter(PDCL2_Params.cfg_burst_diff_chA, current.Value.ParameterValue);
              else if (current.Key == OverrideID.BurstLimit)
                this.SetParameter(PDCL2_Params.cfg_burst_limit_chA, current.Value.ParameterValue);
              else if (current.Key == OverrideID.InputResolutionStr)
                this.SetParameter(PDCL2_Params.ChannelA_VIF, current.Value.ParameterValue);
              else if (current.Key == OverrideID.InputPulsValue)
              {
                if (current.Value.ParameterValue == null)
                  throw new ArgumentNullException(OverrideID.InputPulsValue.ToString());
                double result;
                if (!double.TryParse(current.Value.ParameterValue.ToString().Replace('.', ','), out result))
                  throw new ArgumentException(OverrideID.InputPulsValue.ToString());
                if (result < 0.0)
                  throw new ArgumentOutOfRangeException(OverrideID.InputPulsValue.ToString());
                sbyte theValue = 0;
                double num = result;
                while (num % 1.0 != 0.0)
                {
                  num *= 10.0;
                  --theValue;
                }
                this.WorkMeter.meterMemory.SetParameterValue<sbyte>(PDCL2_Params.ChannelA_Mbus_Exponent, theValue);
                this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.ChannelA_Mbus_Mantissa, Convert.ToUInt16(num));
              }
            }
            break;
          }
        case 2:
          foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in parameter)
          {
            if (keyValuePair.Key == OverrideID.SerialNumberFull)
              this.WorkMeter.deviceIdentification2.FullSerialNumber = keyValuePair.Value.ParameterValue.ToString();
            else if (keyValuePair.Key == OverrideID.Medium)
              this.WorkMeter.deviceIdentification2.Medium = new byte?((byte) Enum.Parse(typeof (MBusDeviceType), keyValuePair.Value.GetStringValueWin()));
            else if (keyValuePair.Key == OverrideID.DeviceHasError)
              this.SetParameter(PDCL2_Params.hwStatusFlagsB, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.OversizeLimit)
              this.SetParameter(PDCL2_Params.cfg_oversize_limit_chB, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.UndersizeLimit)
              this.SetParameter(PDCL2_Params.cfg_undersize_limit_chB, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.TotalPulse)
            {
              this.Perform_SetMeterValueB = new uint?(Convert.ToUInt32(keyValuePair.Value.ParameterValue));
              sbyte parameterValue = this.WorkMeter.meterMemory.GetParameterValue<sbyte>(PDCL2_Params.ChannelB_Mbus_Exponent);
              this.WorkMeter.meterMemory.SetParameterValue<uint>(PDCL2_Params.pulseReadingChannelB, Convert.ToUInt32((double) this.Perform_SetMeterValueB.Value * ((double) this.WorkMeter.meterMemory.GetParameterValue<ushort>(PDCL2_Params.ChannelB_Mbus_Mantissa) * Math.Pow(10.0, (double) parameterValue))));
            }
            else if (keyValuePair.Key == OverrideID.PulseBlockLimit)
              this.SetParameter(PDCL2_Params.cfg_pulse_block_limit_chB, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.PulseLeakLimit)
              this.SetParameter(PDCL2_Params.cfg_pulse_leak_limit_chB, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.PulseUnleakLimit)
              this.SetParameter(PDCL2_Params.cfg_pulse_unleak_limit_chB, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.PulseLeakLower)
              this.SetParameter(PDCL2_Params.cfg_pulse_leak_lower_chB, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.PulseLeakUpper)
              this.SetParameter(PDCL2_Params.cfg_pulse_leak_upper_chB, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.PulseBackLimit)
              this.SetParameter(PDCL2_Params.cfg_pulse_back_limit_chB, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.PulseUnbackLimit)
              this.SetParameter(PDCL2_Params.cfg_pulse_unback_limit_chB, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.OversizeDiff)
              this.SetParameter(PDCL2_Params.cfg_oversize_diff_chB, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.UndersizeDiff)
              this.SetParameter(PDCL2_Params.cfg_undersize_diff_chB, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.BurstDiff)
              this.SetParameter(PDCL2_Params.cfg_burst_diff_chB, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.BurstLimit)
              this.SetParameter(PDCL2_Params.cfg_burst_limit_chB, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.InputResolutionStr)
              this.SetParameter(PDCL2_Params.ChannelB_VIF, keyValuePair.Value.ParameterValue);
            else if (keyValuePair.Key == OverrideID.InputPulsValue)
            {
              if (keyValuePair.Value.ParameterValue == null)
                throw new ArgumentNullException(OverrideID.InputPulsValue.ToString());
              double result;
              if (!double.TryParse(keyValuePair.Value.ParameterValue.ToString().Replace('.', ','), out result))
                throw new ArgumentException(OverrideID.InputPulsValue.ToString());
              if (result < 0.0)
                throw new ArgumentOutOfRangeException(OverrideID.InputPulsValue.ToString());
              sbyte theValue = 0;
              double num = result;
              while (num % 1.0 != 0.0)
              {
                num *= 10.0;
                --theValue;
              }
              this.WorkMeter.meterMemory.SetParameterValue<sbyte>(PDCL2_Params.ChannelB_Mbus_Exponent, theValue);
              this.WorkMeter.meterMemory.SetParameterValue<ushort>(PDCL2_Params.ChannelB_Mbus_Mantissa, Convert.ToUInt16(num));
            }
            else if (keyValuePair.Key == OverrideID.RadioEnabled)
              this.SetParameter(PDCL2_Params.cfg_channel_B_available, keyValuePair.Value.ParameterValue);
          }
          break;
      }
    }

    internal SortedList<long, SortedList<DateTime, double>> GetValues()
    {
      return new SortedList<long, SortedList<DateTime, double>>();
    }

    internal void CompareConnectedAndWork()
    {
      this.WorkMeter.meterMemory.CompareParameterInfo("PDCL2_DeviceMemory", "WorkMeter object", "ConnectedMeter object", (DeviceMemory) this.ConnectedMeter.meterMemory);
    }

    private static void AddParam(
      bool canChanged,
      SortedList<OverrideID, ConfigurationParameter> r,
      OverrideID overrideID,
      object obj)
    {
      PDCL2_AllMeters.AddParam(canChanged, r, overrideID, obj, false, (string[]) null, string.Empty);
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
      this.TypeMeter = (PDCL2_Meter) this.CreateMeter(baseType.Data.EEPdata);
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
  }
}
