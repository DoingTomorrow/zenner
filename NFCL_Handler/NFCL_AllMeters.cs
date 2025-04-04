// Decompiled with JetBrains decompiler
// Type: NFCL_Handler.NFCL_AllMeters
// Assembly: NFCL_Handler, Version=2.3.2.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 887E21A2-7448-48CC-AF3E-C39E4C7B3AFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NFCL_Handler.dll

using GmmDbLib;
using HandlerLib;
using MBusLib;
using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace NFCL_Handler
{
  internal class NFCL_AllMeters : ICreateMeter, IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (NFCL_AllMeters));
    private NFCL_HandlerFunctions functions;
    internal IuwData iuwData;
    internal IuwData iuwDataOriginal;
    private bool Perform_SendJoinRequest = false;
    private bool Perform_SetOperatingMode = false;
    internal NFCL_Meter ConnectedMeter;
    internal NFCL_Meter TypeMeter;
    internal NFCL_Meter BackupMeter;
    internal NFCL_Meter WorkMeter;

    internal NFCL_AllMeters()
    {
    }

    internal NFCL_AllMeters(NFCL_HandlerFunctions functions) => this.functions = functions;

    public void Dispose()
    {
      this.ConnectedMeter = (NFCL_Meter) null;
      this.WorkMeter = (NFCL_Meter) null;
      this.TypeMeter = (NFCL_Meter) null;
      this.BackupMeter = (NFCL_Meter) null;
    }

    public IMeter CreateMeter(byte[] zippedBuffer) => new NFCL_Meter().CreateFromData(zippedBuffer);

    public string SetBackupMeter(byte[] zippedBuffer)
    {
      this.BackupMeter = this.CreateMeter(zippedBuffer) as NFCL_Meter;
      if (this.WorkMeter == null)
        this.WorkMeter = this.BackupMeter.Clone();
      return this.BackupMeter.GetInfo();
    }

    public async Task ConnectDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      DeviceVersionMBus deviceVersionMbus = await this.functions.cmd.ReadVersionAsync(progress, token);
      DeviceIdentification deviceVersion = (DeviceIdentification) deviceVersionMbus;
      deviceVersionMbus = (DeviceVersionMBus) null;
      this.ConnectedMeter = new NFCL_Meter(deviceVersion);
      this.WorkMeter = this.ConnectedMeter.Clone();
      this.Perform_SendJoinRequest = false;
      this.Perform_SetOperatingMode = false;
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
      if (type != FirmwareType.NFC_LoRa_wMBus && type != FirmwareType.NFC_LoRa && type != FirmwareType.NFC_wMBus)
        throw new Exception(Ot.Gtt(Tg.Common, "NotNFCL", "Connected device is not NFC LoRa."));
      this.ConnectedMeter = new NFCL_Meter(deviceIdentification);
      this.iuwData = (IuwData) null;
      IuwData iuwData = await Iuw.TryToReadData(progress, token, this.functions.cmd);
      this.iuwDataOriginal = iuwData;
      iuwData = (IuwData) null;
      if (this.iuwDataOriginal != null && this.iuwDataOriginal.OccuredException == null)
      {
        try
        {
          this.iuwData = Utility.DeepCopy<IuwData>(this.iuwDataOriginal);
        }
        catch (Exception ex)
        {
          NFCL_AllMeters.logger.Error(ex.Message);
        }
      }
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
      this.Perform_SendJoinRequest = false;
      this.Perform_SetOperatingMode = false;
      deviceIdentification = (DeviceIdentification) null;
      ranges = (List<AddressRange>) null;
    }

    public async Task WriteDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      if (this.ConnectedMeter == null)
        throw new ArgumentNullException("ConnectedMeter");
      if (this.WorkMeter == null)
        throw new ArgumentNullException("WorkMeter");
      if (this.iuwData != null)
        await Iuw.TryToWrite(progress, token, this.functions.cmd, this.iuwDataOriginal, this.iuwData);
      List<AddressRange> ranges = this.WorkMeter.meterMemory.GetChangedDataRanges((DeviceMemory) this.ConnectedMeter.meterMemory);
      bool doReset = false;
      if (ranges != null && ranges.Count > 0)
      {
        progress.Split(ranges.Count + 1);
        foreach (AddressRange range in ranges)
        {
          this.WorkMeter.meterMemory.GarantMemoryAvailable(range);
          await this.functions.cmd.Device.WriteMemoryAsync(range, (DeviceMemory) this.WorkMeter.meterMemory, progress, token);
        }
        doReset = true;
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
      this.functions.Clear();
      if (!doReset)
      {
        ranges = (List<AddressRange>) null;
      }
      else
      {
        await this.functions.cmd.Device.ResetDeviceAsync(progress, token);
        ranges = (List<AddressRange>) null;
      }
    }

    public void SetParameter(NFCL_Params p, object value)
    {
      if (value == null)
        return;
      switch (p)
      {
        case NFCL_Params.Bak_TimeZoneInQuarterHours:
          this.WorkMeter.meterMemory.SetParameterValue<sbyte>(NFCL_Params.Bak_TimeZoneInQuarterHours, Convert.ToSByte(Convert.ToDouble(value) * 4.0));
          break;
        case NFCL_Params.cfg_battery_end_life_date:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(NFCL_Params.cfg_battery_end_life_date, MBusUtil.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(Convert.ToDateTime(value)));
          break;
        case NFCL_Params.cfg_transmission_scenario:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(NFCL_Params.cfg_transmission_scenario, Convert.ToByte(value));
          break;
        case NFCL_Params.cfg_otaa_abp_mode:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(NFCL_Params.cfg_otaa_abp_mode, value.ToString() == "OTAA" ? (byte) 1 : (byte) 2);
          break;
        case NFCL_Params.cfg_lora_nwkskey:
        case NFCL_Params.cfg_lora_appskey:
        case NFCL_Params.cfg_lora_device_id:
        case NFCL_Params.Mbus_aes_key:
          this.WorkMeter.meterMemory.SetData(p, AES.StringToAesKey(value.ToString()));
          break;
        case NFCL_Params.lora_adr:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(NFCL_Params.lora_adr, Convert.ToBoolean(value.ToString()) ? (byte) 1 : (byte) 0);
          break;
        case NFCL_Params.Mbus_interval:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(NFCL_Params.Mbus_interval, Convert.ToUInt16((int) Convert.ToUInt16(value) / 2));
          break;
        case NFCL_Params.FD_Mbus_interval:
          this.WorkMeter.meterMemory.SetParameterValue<ushort>(NFCL_Params.FD_Mbus_interval, Convert.ToUInt16((int) Convert.ToUInt16(value) / 2));
          break;
        case NFCL_Params.FD_Mbus_nighttime_start:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(NFCL_Params.FD_Mbus_nighttime_start, Convert.ToByte(value));
          break;
        case NFCL_Params.FD_Mbus_nighttime_stop:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(NFCL_Params.FD_Mbus_nighttime_stop, Convert.ToByte(value));
          break;
        case NFCL_Params.FD_Mbus_radio_suppression_days:
          this.WorkMeter.meterMemory.SetParameterValue<byte>(NFCL_Params.FD_Mbus_radio_suppression_days, Convert.ToByte(value));
          break;
        default:
          throw new NotImplementedException(p.ToString());
      }
    }

    public object GetParameter(NFCL_Params p)
    {
      switch (p)
      {
        case NFCL_Params.MeterId:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<uint>(NFCL_Params.MeterId);
        case NFCL_Params.Bak_TimeZoneInQuarterHours:
          return (object) ((double) this.WorkMeter.meterMemory.GetParameterValue<sbyte>(NFCL_Params.Bak_TimeZoneInQuarterHours) / 4.0);
        case NFCL_Params.cfg_battery_end_life_date:
          return (object) Util.ConvertToDate_MBus_CP16_TypeG(BitConverter.GetBytes(this.WorkMeter.meterMemory.GetParameterValue<ushort>(NFCL_Params.cfg_battery_end_life_date)), 0);
        case NFCL_Params.cfg_transmission_scenario:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.cfg_transmission_scenario);
        case NFCL_Params.cfg_otaa_abp_mode:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.cfg_otaa_abp_mode))
          {
            case 1:
              return (object) "OTAA";
            case 2:
              return (object) "ABP";
            default:
              return (object) string.Empty;
          }
        case NFCL_Params.cfg_lora_nwkskey:
        case NFCL_Params.cfg_lora_appskey:
        case NFCL_Params.cfg_lora_device_id:
        case NFCL_Params.Mbus_aes_key:
          return (object) AES.AesKeyToString(this.WorkMeter.meterMemory.GetData(p));
        case NFCL_Params.cfg_loraWan_netid:
          return (object) Convert.ToUInt32(this.WorkMeter.meterMemory.GetParameterValue<uint>(NFCL_Params.cfg_loraWan_netid));
        case NFCL_Params.cfg_device_mode:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.cfg_device_mode))
          {
            case 0:
              return (object) "OPERATION_MODE";
            case 1:
              return (object) "DELIVERY_MODE";
            case 8:
              return (object) "DELIVERY_MODE_8";
            default:
              throw new NotSupportedException("Unknown value for cfg_device_mode!");
          }
        case NFCL_Params.lora_adr:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.lora_adr))
          {
            case 0:
              return (object) false;
            case 1:
              return (object) true;
            default:
              throw new NotSupportedException("Unknown value for lora_adr!");
          }
        case NFCL_Params.Mbus_interval:
          return (object) ((int) this.WorkMeter.meterMemory.GetParameterValue<ushort>(NFCL_Params.Mbus_interval) * 2);
        case NFCL_Params.cfg_AccessRadioKey:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<uint>(NFCL_Params.cfg_AccessRadioKey);
        case NFCL_Params.rtc_sys_time:
          return (object) Utility.ConvertToDateTime_SystemTime64(this.WorkMeter.meterMemory.GetData(NFCL_Params.rtc_sys_time), 0);
        case NFCL_Params.radio_center_frequency:
          return (object) ((double) this.WorkMeter.meterMemory.GetParameterValue<uint>(NFCL_Params.radio_center_frequency) / 1000000.0);
        case NFCL_Params.FD_Mbus_interval:
          return (object) ((int) this.WorkMeter.meterMemory.GetParameterValue<ushort>(NFCL_Params.FD_Mbus_interval) * 2);
        case NFCL_Params.Mbus_nighttime_start:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.FD_Mbus_nighttime_start);
        case NFCL_Params.Mbus_nighttime_stop:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.Mbus_nighttime_stop);
        case NFCL_Params.Mbus_radio_suppression_days:
          return (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.Mbus_radio_suppression_days);
        case NFCL_Params.cfg_RadioOperationState:
          switch (this.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.cfg_RadioOperationState))
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

    internal SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(int subDevice)
    {
      if (this.WorkMeter == null)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      NFCL_DeviceMemory meterMemory = this.WorkMeter.meterMemory;
      UserManager.CheckPermission("Role\\Developer");
      SortedList<OverrideID, ConfigurationParameter> r1 = new SortedList<OverrideID, ConfigurationParameter>();
      FirmwareVersion firmwareVersion = new FirmwareVersion(meterMemory.FirmwareVersion);
      Version version = new Version((int) firmwareVersion.Major, (int) firmwareVersion.Minor, (int) firmwareVersion.Revision);
      bool flag1 = this.iuwData == null || this.iuwData != null && this.iuwData.CommunicationScenario >= (ushort) 200 && this.iuwData.CommunicationScenario < (ushort) 300;
      bool flag2 = this.iuwData == null || this.iuwData != null && this.iuwData.CommunicationScenario >= (ushort) 300 && this.iuwData.CommunicationScenario < (ushort) 400;
      int num;
      switch (subDevice)
      {
        case 0:
          NFCL_AllMeters.AddParam(false, r1, OverrideID.PrintedSerialNumber, (object) this.WorkMeter.deviceIdentification.PrintedSerialNumberAsString);
          NFCL_AllMeters.AddParam(false, r1, OverrideID.FirmwareVersion, (object) this.WorkMeter.deviceIdentification.FirmwareVersionObj.ToString());
          NFCL_AllMeters.AddParam(false, r1, OverrideID.RadioVersion, (object) string.Format("{0}.{1}", (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.protocol_radio_version_major), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.protocol_radio_version_minor)));
          NFCL_AllMeters.AddParam(true, r1, OverrideID.EndOfBatteryDate, this.GetParameter(NFCL_Params.cfg_battery_end_life_date));
          NFCL_AllMeters.AddParam(true, r1, OverrideID.SetOperatingMode, (object) this.Perform_SetOperatingMode, true, (string[]) null, (string) null);
          NFCL_AllMeters.AddParam(false, r1, OverrideID.DeviceMode, this.GetParameter(NFCL_Params.cfg_device_mode));
          NFCL_AllMeters.AddParam(false, r1, OverrideID.SerialNumber, (object) this.WorkMeter.deviceIdentification.ID_BCD_AsString);
          NFCL_AllMeters.AddParam(false, r1, OverrideID.SerialNumberFull, (object) this.WorkMeter.deviceIdentification.FullSerialNumber);
          NFCL_AllMeters.AddParam(false, r1, OverrideID.Manufacturer, MBusUtil.GetManufacturer(this.WorkMeter.deviceIdentification.Manufacturer));
          NFCL_AllMeters.AddParam(false, r1, OverrideID.MBusGeneration, (object) this.WorkMeter.deviceIdentification.Generation);
          NFCL_AllMeters.AddParam(false, r1, OverrideID.Medium, (object) (MBusDeviceType) this.WorkMeter.deviceIdentification.Medium.Value);
          if (flag2)
          {
            if (this.iuwData != null)
              NFCL_AllMeters.AddParam(false, r1, OverrideID.RadioTechnology, (object) RadioTechnology.wMBus);
            NFCL_AllMeters.AddParam(false, r1, OverrideID.Frequence, this.GetParameter(NFCL_Params.radio_center_frequency), false, (string[]) null, "MHz");
          }
          if (flag1)
          {
            if (this.iuwData != null)
              NFCL_AllMeters.AddParam(false, r1, OverrideID.RadioTechnology, (object) RadioTechnology.LoRa);
            NFCL_AllMeters.AddParam(false, r1, OverrideID.LoRaWanVersion, (object) string.Format("{0}.{1}.{2}", (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.protocol_lorawan_version_major), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.protocol_lorawan_version_middle), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.protocol_lorawan_version_minor)));
            NFCL_AllMeters.AddParam(false, r1, OverrideID.LoRaVersion, (object) string.Format("{0}.{1}", (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.protocol_lora_version_major), (object) this.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.protocol_lora_version_minor)));
            NFCL_AllMeters.AddParam(false, r1, OverrideID.NetID, (object) meterMemory.GetParameterValue<uint>(NFCL_Params.cfg_loraWan_netid));
            NFCL_AllMeters.AddParam(false, r1, OverrideID.NwkSKey, (object) meterMemory.GetData(NFCL_Params.cfg_lora_nwkskey));
            NFCL_AllMeters.AddParam(false, r1, OverrideID.AppSKey, (object) meterMemory.GetData(NFCL_Params.cfg_lora_appskey));
            NFCL_AllMeters.AddParam(false, r1, OverrideID.DevAddr, (object) meterMemory.GetParameterValue<uint>(NFCL_Params.cfg_lora_device_id));
            NFCL_AllMeters.AddParam(true, r1, OverrideID.Activation, this.GetParameter(NFCL_Params.cfg_otaa_abp_mode), false, new string[2]
            {
              "OTAA",
              "ABP"
            }, (string) null);
            NFCL_AllMeters.AddParam(true, r1, OverrideID.SendJoinRequest, (object) this.Perform_SendJoinRequest, true, (string[]) null, (string) null);
            NFCL_AllMeters.AddParam(true, r1, OverrideID.ADR, this.GetParameter(NFCL_Params.lora_adr));
            if (version >= Version.Parse("0.5.0"))
            {
              uint parameterValue1 = this.WorkMeter.meterMemory.GetParameterValue<uint>(NFCL_Params.cfg_lora_basic_frequency1);
              uint parameterValue2 = this.WorkMeter.meterMemory.GetParameterValue<uint>(NFCL_Params.cfg_lora_basic_frequency2);
              uint parameterValue3 = this.WorkMeter.meterMemory.GetParameterValue<uint>(NFCL_Params.cfg_lora_basic_frequency3);
              uint parameterValue4 = this.WorkMeter.meterMemory.GetParameterValue<uint>(NFCL_Params.cfg_lora_rx_freq_window2);
              if (parameterValue1 == 868100000U && parameterValue2 == 868300000U && parameterValue3 == 868500000U && parameterValue4 == 869525000U)
                NFCL_AllMeters.AddParam(true, r1, OverrideID.Region, (object) Region.EU_863_870);
              else if (parameterValue1 == 868900000U && parameterValue2 == 869100000U && parameterValue3 == 864100000U && parameterValue4 == 869100000U)
                NFCL_AllMeters.AddParam(true, r1, OverrideID.Region, (object) Region.RU_864_870);
              else
                NFCL_AllMeters.AddParam(true, r1, OverrideID.Region, (object) null);
            }
            goto label_29;
          }
          else
            goto label_29;
        case 1:
          num = this.iuwData != null ? 1 : 0;
          break;
        default:
          num = 0;
          break;
      }
      if (num != 0)
      {
        if (this.iuwData.OccuredException != null && this.iuwData.OccuredException.Message == "Not_classified_error CMD: SpecialCommands_0x36")
        {
          ushort parameterValue = this.WorkMeter.meterMemory.GetParameterValue<ushort>(NFCL_Params.nfc_err_state);
          if (parameterValue > (ushort) 0)
          {
            CR95_RESULTCODE cr95Resultcode = (CR95_RESULTCODE) ((int) parameterValue & (int) byte.MaxValue);
            NFC_TRANSMISSION_STATE transmissionState = (NFC_TRANSMISSION_STATE) ((int) parameterValue >> 8);
            throw new Exception("Please check the NFC connector. No TAG detected. " + cr95Resultcode.ToString() + " " + transmissionState.ToString());
          }
        }
        uint? deviceStatusFlags = this.iuwData.Ident.DeviceStatusFlags;
        bool flag3 = ((AbstractStatus) deviceStatusFlags.Value).HasFlag((Enum) AbstractStatus.DEVICE_IN_SLEEP);
        NFCL_AllMeters.AddParam(false, r1, OverrideID.DeviceMode, flag3 ? (object) "DELIVERY_MODE" : (object) "OPERATION_MODE");
        NFCL_AllMeters.AddParam(false, r1, OverrideID.FirmwareVersion, (object) this.iuwData.Ident.GetFirmwareVersionString());
        NFCL_AllMeters.AddParam(false, r1, OverrideID.SerialNumber, (object) this.iuwData.Ident.ID_BCD_AsString);
        NFCL_AllMeters.AddParam(false, r1, OverrideID.SerialNumberFull, (object) this.iuwData.Ident.FullSerialNumber);
        SortedList<OverrideID, ConfigurationParameter> r2 = r1;
        deviceStatusFlags = this.iuwData.Ident.DeviceStatusFlags;
        string str = deviceStatusFlags.Value.ToString("X4");
        NFCL_AllMeters.AddParam(false, r2, OverrideID.ErrorCode, (object) str);
        NFCL_AllMeters.AddParam(false, r1, OverrideID.DeviceClock, (object) this.iuwData.IuwCurrentData.DeviceTime);
        NFCL_AllMeters.AddParam(false, r1, OverrideID.VolumeActualValue, (object) (Decimal) this.iuwData.IuwCurrentData.Volume);
        NFCL_AllMeters.AddParam(false, r1, OverrideID.EndOfBatteryDate, (object) this.iuwData.BatteryEndDate);
        NFCL_AllMeters.AddParam(true, r1, OverrideID.CommunicationScenario, (object) (int) this.iuwData.CommunicationScenario);
        NFCL_AllMeters.AddParam(true, r1, OverrideID.DueDate, (object) this.iuwData.KeyDate);
        NFCL_AllMeters.AddParam(true, r1, OverrideID.SetOperatingMode, (object) this.iuwData.SetOperationMode, true, (string[]) null, (string) null);
        if (flag2)
        {
          NFCL_AllMeters.AddParam(false, r1, OverrideID.Manufacturer, (object) this.iuwData.Ident.ManufacturerName);
          NFCL_AllMeters.AddParam(false, r1, OverrideID.MBusGeneration, (object) this.iuwData.Ident.GenerationAsString);
          NFCL_AllMeters.AddParam(false, r1, OverrideID.Medium, (object) this.iuwData.Ident.MediumAsString);
          NFCL_AllMeters.AddParam(false, r1, OverrideID.AESKey, (object) AES.AesKeyToString(this.iuwData.AesKey));
        }
        if (flag1)
        {
          NFCL_AllMeters.AddParam(false, r1, OverrideID.JoinEUI, (object) this.iuwData.JoinEUI);
          NFCL_AllMeters.AddParam(false, r1, OverrideID.DevEUI, (object) this.iuwData.DevEUI);
          NFCL_AllMeters.AddParam(false, r1, OverrideID.AppKey, (object) this.iuwData.AppKey);
        }
      }
label_29:
      return r1;
    }

    internal void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameter,
      int subDevice)
    {
      if (parameter == null || parameter.Count == 0)
        return;
      switch (subDevice)
      {
        case 0:
          NFCL_DeviceMemory meterMemory = this.WorkMeter.meterMemory;
          using (IEnumerator<KeyValuePair<OverrideID, ConfigurationParameter>> enumerator = parameter.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<OverrideID, ConfigurationParameter> current = enumerator.Current;
              if (current.Key == OverrideID.TransmissionScenario)
                this.SetParameter(NFCL_Params.cfg_transmission_scenario, current.Value.ParameterValue);
              else if (current.Key != OverrideID.CommunicationScenario)
              {
                if (current.Key == OverrideID.EndOfBatteryDate)
                  this.SetParameter(NFCL_Params.cfg_battery_end_life_date, current.Value.ParameterValue);
                else if (current.Key == OverrideID.Activation)
                  this.SetParameter(NFCL_Params.cfg_otaa_abp_mode, current.Value.ParameterValue);
                else if (current.Key == OverrideID.TimeZone)
                  this.SetParameter(NFCL_Params.Bak_TimeZoneInQuarterHours, current.Value.ParameterValue);
                else if (current.Key == OverrideID.ADR)
                  this.SetParameter(NFCL_Params.lora_adr, current.Value.ParameterValue);
                else if (current.Key == OverrideID.AESKey)
                  this.SetParameter(NFCL_Params.Mbus_aes_key, current.Value.ParameterValue);
                else if (current.Key == OverrideID.RadioSendInterval)
                  this.SetParameter(NFCL_Params.Mbus_interval, current.Value.ParameterValue);
                else if (current.Key == OverrideID.SendJoinRequest)
                  this.Perform_SendJoinRequest = Convert.ToBoolean(current.Value.ParameterValue.ToString());
                else if (current.Key == OverrideID.SetOperatingMode)
                  this.Perform_SetOperatingMode = Convert.ToBoolean(current.Value.ParameterValue.ToString());
                else if (current.Key == OverrideID.RadioActiveMonday)
                  this.SetMbus_radio_suppression_days((byte) 1, Convert.ToBoolean(current.Value.ParameterValue));
                else if (current.Key == OverrideID.RadioActiveTuesday)
                  this.SetMbus_radio_suppression_days((byte) 2, Convert.ToBoolean(current.Value.ParameterValue));
                else if (current.Key == OverrideID.RadioActiveWednesday)
                  this.SetMbus_radio_suppression_days((byte) 4, Convert.ToBoolean(current.Value.ParameterValue));
                else if (current.Key == OverrideID.RadioActiveThursday)
                  this.SetMbus_radio_suppression_days((byte) 8, Convert.ToBoolean(current.Value.ParameterValue));
                else if (current.Key == OverrideID.RadioActiveFriday)
                  this.SetMbus_radio_suppression_days((byte) 16, Convert.ToBoolean(current.Value.ParameterValue));
                else if (current.Key == OverrideID.RadioActiveSaturday)
                  this.SetMbus_radio_suppression_days((byte) 32, Convert.ToBoolean(current.Value.ParameterValue));
                else if (current.Key == OverrideID.RadioActiveSunday)
                  this.SetMbus_radio_suppression_days((byte) 64, Convert.ToBoolean(current.Value.ParameterValue));
                else if (current.Key == OverrideID.RadioActiveStartTime)
                {
                  byte theValue = Convert.ToByte(current.Value.ParameterValue);
                  if (theValue < (byte) 0 || theValue > (byte) 23)
                    throw new ArgumentOutOfRangeException("RadioActiveStartTime");
                  this.WorkMeter.meterMemory.SetParameterValue<byte>(NFCL_Params.Mbus_nighttime_stop, theValue);
                }
                else if (current.Key == OverrideID.RadioActiveStopTime)
                {
                  byte theValue = Convert.ToByte(current.Value.ParameterValue);
                  if (theValue <= (byte) 1 || theValue > (byte) 24)
                    throw new ArgumentOutOfRangeException("RadioActiveStopTime");
                  if (theValue == (byte) 24)
                    theValue = (byte) 0;
                  this.WorkMeter.meterMemory.SetParameterValue<byte>(NFCL_Params.Mbus_nighttime_start, theValue);
                }
                else if (current.Key == OverrideID.Region)
                {
                  switch ((Region) Enum.Parse(typeof (Region), current.Value.ParameterValue.ToString()))
                  {
                    case Region.EU_863_870:
                      this.WorkMeter.meterMemory.SetParameterValue<uint>(NFCL_Params.cfg_lora_basic_frequency1, 868100000U);
                      this.WorkMeter.meterMemory.SetParameterValue<uint>(NFCL_Params.cfg_lora_basic_frequency2, 868300000U);
                      this.WorkMeter.meterMemory.SetParameterValue<uint>(NFCL_Params.cfg_lora_basic_frequency3, 868500000U);
                      this.WorkMeter.meterMemory.SetParameterValue<uint>(NFCL_Params.cfg_lora_rx_freq_window2, 869525000U);
                      break;
                    case Region.RU_864_870:
                      this.WorkMeter.meterMemory.SetParameterValue<uint>(NFCL_Params.cfg_lora_basic_frequency1, 868900000U);
                      this.WorkMeter.meterMemory.SetParameterValue<uint>(NFCL_Params.cfg_lora_basic_frequency2, 869100000U);
                      this.WorkMeter.meterMemory.SetParameterValue<uint>(NFCL_Params.cfg_lora_basic_frequency3, 864100000U);
                      this.WorkMeter.meterMemory.SetParameterValue<uint>(NFCL_Params.cfg_lora_rx_freq_window2, 869100000U);
                      break;
                    default:
                      throw new NotImplementedException(current.Key.ToString() + " " + current.Value.ParameterValue.ToString());
                  }
                }
              }
            }
            break;
          }
        case 1:
          foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in parameter)
          {
            if (keyValuePair.Key == OverrideID.SetOperatingMode)
              this.iuwData.SetOperationMode = Convert.ToBoolean(keyValuePair.Value.GetStringValueWin());
            else if (keyValuePair.Key == OverrideID.DueDate)
            {
              this.iuwData.KeyDate = new DateTime?(Convert.ToDateTime(keyValuePair.Value.GetStringValueWin()));
            }
            else
            {
              ushort num = keyValuePair.Key == OverrideID.CommunicationScenario ? Convert.ToUInt16(keyValuePair.Value.GetStringValueWin()) : throw new NotImplementedException("Set " + keyValuePair.Key.ToString() + " is not implemented!");
              if ((this.iuwData.AvailableScenarios == null || !((IEnumerable<ushort>) this.iuwData.AvailableScenarios).Contains<ushort>(num)) && num > (ushort) 0)
                throw new Exception("CommunicationScenario " + num.ToString() + " is not allowed!");
              this.iuwData.CommunicationScenario = num;
            }
          }
          break;
      }
    }

    private void SetMbus_radio_suppression_days(byte mask, bool isSet)
    {
      byte parameterValue = this.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.Mbus_radio_suppression_days);
      this.WorkMeter.meterMemory.SetParameterValue<byte>(NFCL_Params.Mbus_radio_suppression_days, isSet ? (byte) ((uint) parameterValue & (uint) ~mask) : (byte) ((uint) parameterValue | (uint) mask));
    }

    internal SortedList<long, SortedList<DateTime, double>> GetValues()
    {
      return new SortedList<long, SortedList<DateTime, double>>();
    }

    internal void CompareConnectedAndWork()
    {
      this.WorkMeter.meterMemory.CompareParameterInfo("NFCL_DeviceMemory", "WorkMeter object", "ConnectedMeter object", (DeviceMemory) this.ConnectedMeter.meterMemory);
    }

    private static void AddParam(
      bool canChanged,
      SortedList<OverrideID, ConfigurationParameter> r,
      OverrideID overrideID,
      object obj)
    {
      NFCL_AllMeters.AddParam(canChanged, r, overrideID, obj, false, (string[]) null, (string) null);
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
