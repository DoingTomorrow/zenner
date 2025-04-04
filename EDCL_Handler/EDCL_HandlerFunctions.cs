// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.EDCL_HandlerFunctions
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using CommunicationPort.Functions;
using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using MBusLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace EDCL_Handler
{
  public sealed class EDCL_HandlerFunctions : 
    HandlerFunctionsForProduction,
    IReadoutConfig,
    IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (EDCL_HandlerFunctions));
    public const string HandlerName = "EDCL_Handler";
    public List<byte> Rec_Pac_470 = new List<byte>();
    private ConfigList configList;
    private CommunicationPortFunctions port;
    private BaseDbConnection db;
    public EDCL_DeviceCommands cmd;
    internal EDCL_AllMeters myMeters;
    private Stopwatch stopwatch;
    private Queue<byte> volumeMonitorQueue;
    private int? maxCountofVolumeMonitorData;
    private List<VolumeMonitorEventArgs> volumeMonitorData;

    public event VolumeMonitorEventHandler OnVolumeMonitorDataReceived;

    public EDCL_HandlerFunctions(CommunicationPortFunctions myPort, BaseDbConnection myDb)
      : this(myPort)
    {
      this.db = myDb;
    }

    public EDCL_HandlerFunctions(CommunicationPortFunctions myPort)
      : this()
    {
      this.port = myPort;
      this.cmd = new EDCL_DeviceCommands(this.port);
      this.myMeters = new EDCL_AllMeters(this);
    }

    public EDCL_HandlerFunctions(BaseDbConnection myDb)
      : this()
    {
      this.db = myDb;
      this.myMeters = new EDCL_AllMeters(this);
    }

    public EDCL_HandlerFunctions() => this.myMeters = new EDCL_AllMeters(this);

    public override void SetCommunicationPort(CommunicationPortFunctions myPort)
    {
      this.port = myPort;
      if (this.cmd != null)
        return;
      this.cmd = new EDCL_DeviceCommands(this.port);
    }

    private void setCommandDeviceAndMeters()
    {
      this.cmd = new EDCL_DeviceCommands(this.port);
      this.myMeters = new EDCL_AllMeters(this);
    }

    public override void SetReadoutConfiguration(ConfigList configList)
    {
      if (configList == null)
        throw new ArgumentNullException(nameof (configList));
      if (this.configList == null)
        this.configList = configList;
      else if (this.configList != configList)
        throw new ArgumentException("this.configList != configList");
      if (this.port == null)
        this.port = new CommunicationPortFunctions();
      this.port.SetReadoutConfiguration(configList);
      this.setCommandDeviceAndMeters();
    }

    public override ConfigList GetReadoutConfiguration()
    {
      return this.port == null ? (ConfigList) null : this.port.GetReadoutConfiguration();
    }

    public override bool LoadLastBackup(int meterID)
    {
      BaseTables.MeterDataRow meterDataRow = GmmDbLib.MeterData.LoadLastBackup(DbBasis.PrimaryDB.BaseDbConnection, meterID);
      if (meterDataRow == null)
        return false;
      this.myMeters.SetBackupMeter(meterDataRow.PValueBinary);
      return true;
    }

    public override bool SetBackup(byte[] zippedBuffer)
    {
      if (zippedBuffer == null)
        return false;
      this.myMeters.SetBackupMeter(zippedBuffer);
      return true;
    }

    public void Dispose()
    {
      if (this.port != null)
      {
        this.port.Dispose();
        this.port = (CommunicationPortFunctions) null;
      }
      if (this.myMeters == null)
        return;
      this.myMeters.Dispose();
      this.myMeters = (EDCL_AllMeters) null;
    }

    public override void Clear() => this.myMeters = new EDCL_AllMeters(this);

    public override void Open()
    {
      if (this.port == null)
        throw new ArgumentNullException("myPort");
      this.port.Open();
    }

    public override void Close()
    {
      if (this.port == null)
        throw new ArgumentNullException("myPort");
      this.port.Close();
    }

    public override DateTime? SaveMeter()
    {
      if (this.myMeters == null)
        throw new ArgumentNullException("myMeters");
      if (this.myMeters.WorkMeter == null)
        throw new ArgumentNullException("WorkMeter");
      DeviceIdentification deviceIdentification = this.GetDeviceIdentification(0);
      if (deviceIdentification == null || !deviceIdentification.MeterID.HasValue)
        throw new ArgumentException("Invalid MeterID!");
      BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
      int meterID = (int) deviceIdentification.MeterID.Value;
      int meterInfoID = (int) deviceIdentification.MeterInfoID.Value;
      uint hardwareTypeID_OR_firmwareVersion = deviceIdentification.FirmwareVersion.Value;
      string serialNr = deviceIdentification.PrintedSerialNumberAsString != null ? deviceIdentification.PrintedSerialNumberAsString : "0";
      string productionOrderNumber = deviceIdentification.SAP_ProductionOrderNumber;
      byte[] compressedData = this.myMeters.WorkMeter.meterMemory.GetCompressedData();
      DateTime? nullable = Device.Save(baseDbConnection, meterID, meterInfoID, hardwareTypeID_OR_firmwareVersion, serialNr, productionOrderNumber, compressedData, false);
      this.myMeters.SetBackupMeter(compressedData);
      return nullable;
    }

    public async Task<FirmwareVersion> ReadVersionAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      DeviceVersionMBus ver = await this.cmd.ReadVersionAsync(progress, token);
      FirmwareVersion firmwareVersionObj = ver.FirmwareVersionObj;
      ver = (DeviceVersionMBus) null;
      return firmwareVersionObj;
    }

    public override async Task<int> ReadDeviceAsync(
      ProgressHandler progress,
      CancellationToken token,
      ReadPartsSelection readPartsSelection)
    {
      await this.myMeters.ReadDeviceAsync(progress, token, readPartsSelection);
      this.DeviceMemory = (DeviceMemory) this.myMeters.ConnectedMeter.meterMemory;
      return 2;
    }

    public override async Task ResetDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.cmd.Device.ResetDeviceAsync(progress, token);
    }

    public void SetconfigParameters()
    {
      SortedList<OverrideID, ConfigurationParameter> configurationParameters = this.GetConfigurationParameters(0);
      int numberOfIds = 1;
      int nextId = DbBasis.PrimaryDB.BaseDbConnection.GetNewIds("SerialNumber_EDC_LoRa", numberOfIds).GetNextID();
      configurationParameters[OverrideID.SerialNumberFull].ParameterValue = (object) Utility.ConvertSerialNumberToFullserialNumber(Utility.LoraDevice.EDC, nextId);
      configurationParameters[OverrideID.DevEUI].ParameterValue = (object) Utility.ConvertSerialNumberToEUI64(Utility.LoraDevice.EDC, nextId);
      this.SetConfigurationParameters(configurationParameters, 0);
    }

    public override DeviceIdentification GetDeviceIdentification(int channel)
    {
      switch (channel)
      {
        case 0:
          return (DeviceIdentification) this.myMeters.checkedWorkMeter.deviceIdentification;
        case 1:
          return (DeviceIdentification) this.myMeters.checkedWorkMeter.deviceIdentification1;
        default:
          throw new Exception("Not supported channel: " + channel.ToString());
      }
    }

    public override async Task WriteDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.myMeters.WriteDeviceAsync(progress, token);
    }

    public override async Task WriteDeviceOnlyAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.myMeters.WriteDeviceAsync(progress, token, false);
    }

    public override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      int subDevice = 0)
    {
      return this.myMeters.GetConfigurationParameters(subDevice);
    }

    public override bool SetConfigurationParameterFromBackup()
    {
      SortedList<OverrideID, ConfigurationParameter> configurationParameters = this.GetConfigurationParameters(0);
      if (this.myMeters.BackupMeter.meterMemory.IsParameterAvailable(EDCL_Params.fullserialnumber))
      {
        ConfigurationParameter configurationParameter = new ConfigurationParameter(OverrideID.SerialNumber);
        configurationParameter.ParameterValue = (object) this.myMeters.BackupMeter.meterMemory.GetParameterValue<ulong>(EDCL_Params.fullserialnumber);
        if (configurationParameters.ContainsKey(OverrideID.PrintedSerialNumber))
          configurationParameters[OverrideID.PrintedSerialNumber].ParameterValue = configurationParameter.ParameterValue;
        else
          configurationParameters.Add(OverrideID.PrintedSerialNumber, configurationParameter);
      }
      this.SetConfigurationParameters(configurationParameters, 0);
      return true;
    }

    public override void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameter,
      int subDevice = 0)
    {
      if (parameter != null && parameter.ContainsKey(OverrideID.LeakDetectionOn) && parameter[OverrideID.LeakDetectionOn].ParameterValue != null && parameter.ContainsKey(OverrideID.BackflowDetectionOn) && parameter[OverrideID.BackflowDetectionOn].ParameterValue != null && parameter.ContainsKey(OverrideID.BurstDetectionOn) && parameter[OverrideID.BurstDetectionOn].ParameterValue != null && parameter.ContainsKey(OverrideID.StandstillDetectionOn) && parameter[OverrideID.StandstillDetectionOn].ParameterValue != null && parameter.ContainsKey(OverrideID.UndersizeDetectionOn) && parameter[OverrideID.UndersizeDetectionOn].ParameterValue != null && parameter.ContainsKey(OverrideID.OversizeDetectionOn) && parameter[OverrideID.OversizeDetectionOn].ParameterValue != null)
      {
        bool parameterValue1 = (bool) parameter[OverrideID.LeakDetectionOn].ParameterValue;
        bool parameterValue2 = (bool) parameter[OverrideID.BackflowDetectionOn].ParameterValue;
        bool parameterValue3 = (bool) parameter[OverrideID.BurstDetectionOn].ParameterValue;
        bool parameterValue4 = (bool) parameter[OverrideID.StandstillDetectionOn].ParameterValue;
        bool parameterValue5 = (bool) parameter[OverrideID.UndersizeDetectionOn].ParameterValue;
        bool parameterValue6 = (bool) parameter[OverrideID.OversizeDetectionOn].ParameterValue;
        double? waterMeterNominalFlow = new double?();
        byte? pulseMultiplier = new byte?();
        if (parameter.ContainsKey(OverrideID.NominalFlow) && parameter[OverrideID.NominalFlow].ParameterValue != null)
        {
          double result;
          if (double.TryParse(parameter[OverrideID.NominalFlow].ParameterValue.ToString().Replace(",", "."), NumberStyles.Float, (IFormatProvider) FixedFormates.TheFormates.NumberFormat, out result))
            waterMeterNominalFlow = new double?(result * 1000.0);
          parameter.Remove(OverrideID.NominalFlow);
        }
        byte result1;
        if (parameter.ContainsKey(OverrideID.PulseMultiplier) && parameter[OverrideID.PulseMultiplier].ParameterValue != null && byte.TryParse(parameter[OverrideID.PulseMultiplier].ParameterValue.ToString(), out result1))
          pulseMultiplier = new byte?(result1);
        if (waterMeterNominalFlow.HasValue && !pulseMultiplier.HasValue)
          throw new Exception("Missed parameter 'OverrideID.PulseMultiplier'");
        this.myMeters.SetSmartFunctions(waterMeterNominalFlow, pulseMultiplier, parameterValue1, parameterValue3, parameterValue2, parameterValue4, parameterValue5, parameterValue6);
        parameter.Remove(OverrideID.LeakDetectionOn);
        parameter.Remove(OverrideID.BackflowDetectionOn);
        parameter.Remove(OverrideID.BurstDetectionOn);
        parameter.Remove(OverrideID.StandstillDetectionOn);
        parameter.Remove(OverrideID.UndersizeDetectionOn);
        parameter.Remove(OverrideID.OversizeDetectionOn);
      }
      this.myMeters.SetConfigurationParameters(parameter, subDevice);
    }

    public override void SetDataToWorkMeter(uint address, byte[] data)
    {
      this.myMeters.WorkMeter.meterMemory.SetData(address, data);
    }

    public bool SetRx2DownLinkChannelIndex(byte value)
    {
      this.myMeters.SetParameter(EDCL_Params.Rx2DownLinkChannelIndex, (object) value);
      return true;
    }

    public bool SetUpDownChannelScenario(byte value)
    {
      this.myMeters.SetParameter(EDCL_Params.UpDownChannelScenario, (object) value);
      return true;
    }

    public bool SetUpLinkChannelQuantity(byte value)
    {
      this.myMeters.SetParameter(EDCL_Params.UpLinkChannelQuantity, (object) value);
      return true;
    }

    public bool SetUpLinkStartChannelIndex(byte value)
    {
      this.myMeters.SetParameter(EDCL_Params.UpLinkStartChannelIndex, (object) value);
      return true;
    }

    public bool SetSamplingRate(EDCL_HandlerFunctions.SamplingRate rate)
    {
      switch (rate)
      {
        case EDCL_HandlerFunctions.SamplingRate.Normal_192_Low_32:
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_period_nom, (object) (ushort) 170);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_nom, (object) (ushort) 130);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_lora, (object) (ushort) 44);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_noflow_timeout, (object) (ushort) 768);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_low, (object) (ushort) 984);
          break;
        case EDCL_HandlerFunctions.SamplingRate.Normal_128_Low_32:
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_period_nom, (object) (ushort) 256);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_nom, (object) (ushort) 216);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_lora, (object) (ushort) 130);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_noflow_timeout, (object) (ushort) 512);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_low, (object) (ushort) 984);
          break;
        case EDCL_HandlerFunctions.SamplingRate.Normal_64_Low_32:
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_period_nom, (object) (ushort) 512);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_nom, (object) (ushort) 472);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_lora, (object) (ushort) 386);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_noflow_timeout, (object) (ushort) 256);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_low, (object) (ushort) 984);
          break;
        case EDCL_HandlerFunctions.SamplingRate.Normal_8_Low_8:
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_period_nom, (object) (ushort) 4096);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_nom, (object) (ushort) 4056);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_lora, (object) (ushort) 3970);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_noflow_timeout, (object) (ushort) 32);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_low, (object) (ushort) 4056);
          break;
        case EDCL_HandlerFunctions.SamplingRate.Normal_40_Low_16:
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_period_nom, (object) (ushort) 819);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_nom, (object) (ushort) 779);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_lora, (object) (ushort) 693);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_noflow_timeout, (object) (ushort) 160);
          this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_low, (object) (ushort) 2008);
          break;
        default:
          throw new NotImplementedException();
      }
      return true;
    }

    public EDCL_HandlerFunctions.SamplingRate GetSamplingRate()
    {
      ushort parameter1 = (ushort) this.myMeters.GetParameter(EDCL_Params.cfg_scanif_period_nom);
      ushort parameter2 = (ushort) this.myMeters.GetParameter(EDCL_Params.cfg_scanif_cycle_nom);
      ushort parameter3 = (ushort) this.myMeters.GetParameter(EDCL_Params.cfg_scanif_cycle_lora);
      ushort parameter4 = (ushort) this.myMeters.GetParameter(EDCL_Params.cfg_scanif_noflow_timeout);
      ushort parameter5 = (ushort) this.myMeters.GetParameter(EDCL_Params.cfg_scanif_cycle_low);
      if (parameter1 == (ushort) 170 && parameter2 == (ushort) 130 && parameter3 == (ushort) 44 && parameter4 == (ushort) 768 && parameter5 == (ushort) 984)
        return EDCL_HandlerFunctions.SamplingRate.Normal_192_Low_32;
      if (parameter1 == (ushort) 256 && parameter2 == (ushort) 216 && parameter3 == (ushort) 130 && parameter4 == (ushort) 512 && parameter5 == (ushort) 984)
        return EDCL_HandlerFunctions.SamplingRate.Normal_128_Low_32;
      if (parameter1 == (ushort) 512 && parameter2 == (ushort) 472 && parameter3 == (ushort) 386 && parameter4 == (ushort) 256 && parameter5 == (ushort) 984)
        return EDCL_HandlerFunctions.SamplingRate.Normal_64_Low_32;
      if (parameter1 == (ushort) 4096 && parameter2 == (ushort) 4056 && parameter3 == (ushort) 3970 && parameter4 == (ushort) 32 && parameter5 == (ushort) 4056)
        return EDCL_HandlerFunctions.SamplingRate.Normal_8_Low_8;
      return parameter1 == (ushort) 819 && parameter2 == (ushort) 779 && parameter3 == (ushort) 693 && parameter4 == (ushort) 160 && parameter5 == (ushort) 2008 ? EDCL_HandlerFunctions.SamplingRate.Normal_40_Low_16 : EDCL_HandlerFunctions.SamplingRate.Unknown;
    }

    public bool SetVIF(byte value)
    {
      this.myMeters.SetParameter(EDCL_Params.VIF, (object) value);
      return true;
    }

    public bool Setcfg_scanif_cycle_lora(ushort value)
    {
      this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_lora, (object) value);
      return true;
    }

    public bool Setcfg_scanif_cycle_low(ushort value)
    {
      this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_low, (object) value);
      return true;
    }

    public bool Setcfg_scanif_cycle_nom(ushort value)
    {
      this.myMeters.SetParameter(EDCL_Params.cfg_scanif_cycle_nom, (object) value);
      return true;
    }

    public bool Setcfg_scanif_noflow_timeout(ushort value)
    {
      this.myMeters.SetParameter(EDCL_Params.cfg_scanif_noflow_timeout, (object) value);
      return true;
    }

    public bool Setcfg_scanif_period_low(ushort value)
    {
      this.myMeters.SetParameter(EDCL_Params.cfg_scanif_period_low, (object) value);
      return true;
    }

    public bool Setcfg_scanif_period_nom(ushort value)
    {
      this.myMeters.SetParameter(EDCL_Params.cfg_scanif_period_nom, (object) value);
      return true;
    }

    public bool Setscanif_noflow_timeout(ushort value)
    {
      this.myMeters.SetParameter(EDCL_Params.scanif_noflow_timeout, (object) value);
      return true;
    }

    public bool SetCoilErrorThreshold(sbyte value)
    {
      this.myMeters.SetParameter(EDCL_Params.cfg_coil_error_threshold, (object) value);
      return true;
    }

    public sbyte GetCoilErrorThreshold()
    {
      return Convert.ToSByte(this.myMeters.GetParameter(EDCL_Params.cfg_coil_error_threshold));
    }

    public void SetPulseAbnormalCounter(byte value)
    {
      this.myMeters.SetParameter(EDCL_Params.pulseAbnormalCounter, (object) value);
    }

    public void SetHardwareErrors(byte value)
    {
      this.myMeters.SetParameter(EDCL_Params.hwStatusFlags, (object) value);
    }

    public HardwareStatus GetHardwareErrors()
    {
      return (HardwareStatus) this.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.hwStatusFlags);
    }

    public void SetWarnings(byte value)
    {
      this.myMeters.SetParameter(EDCL_Params.persistent_warning_flags, (object) value);
    }

    public void SetFD_Mbus_interval(ushort value)
    {
      this.myMeters.SetParameter(EDCL_Params.FD_Mbus_interval, (object) value);
    }

    public void SetFD_Mbus_nighttime_start(byte value)
    {
      this.myMeters.SetParameter(EDCL_Params.FD_Mbus_nighttime_start, (object) value);
    }

    public void SetFD_Mbus_nighttime_stop(byte value)
    {
      this.myMeters.SetParameter(EDCL_Params.FD_Mbus_nighttime_stop, (object) value);
    }

    public void SetFD_Mbus_radio_suppression_days(byte value)
    {
      this.myMeters.SetParameter(EDCL_Params.FD_Mbus_radio_suppression_days, (object) value);
    }

    public ushort GetFD_Mbus_interval()
    {
      if (!this.myMeters.WorkMeter.meterMemory.IsParameterAvailable(EDCL_Params.FD_Mbus_interval))
        throw new Exception("Parameter 'FD_Mbus_interval' is not available in the firmware.");
      return Convert.ToUInt16(this.myMeters.GetParameter(EDCL_Params.FD_Mbus_interval));
    }

    public ushort GetMbus_interval()
    {
      if (!this.myMeters.WorkMeter.meterMemory.IsParameterAvailable(EDCL_Params.Mbus_interval))
        throw new Exception("Parameter 'Mbus_interval' is not available in the firmware.");
      return Convert.ToUInt16(this.myMeters.GetParameter(EDCL_Params.Mbus_interval));
    }

    public byte GetMbus_nighttime_start()
    {
      return this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.Mbus_nighttime_start);
    }

    public byte GetMbus_nighttime_stop()
    {
      return this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.Mbus_nighttime_stop);
    }

    public byte GetMbus_radio_suppression_days()
    {
      return this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.Mbus_radio_suppression_days);
    }

    public void SetRadioTransmitSyncWord(byte radio_transmit_sync1, byte radio_transmit_sync2)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.radio_transmit_sync1, radio_transmit_sync1);
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.radio_transmit_sync2, radio_transmit_sync2);
    }

    public void SetRadioResceiveSyncWord(byte radio_resceive_sync1, byte radio_resceive_sync2)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.radio_resceive_sync1, radio_resceive_sync1);
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.radio_resceive_sync2, radio_resceive_sync2);
    }

    public sbyte GetCoilBOffset()
    {
      return (sbyte) this.myMeters.GetParameter(EDCL_Params.cfg_coil_b_offset);
    }

    public void SetCoilBOffset(sbyte value)
    {
      this.myMeters.SetParameter(EDCL_Params.cfg_coil_b_offset, (object) value);
    }

    public void SetCoilMaxThreshold(sbyte value)
    {
      this.myMeters.SetParameter(EDCL_Params.cfg_coil_max_threshold, (object) value);
    }

    public void SetCoilMinThreshold(sbyte value)
    {
      this.myMeters.SetParameter(EDCL_Params.cfg_coil_min_threshold, (object) value);
    }

    public sbyte GetCoilMaxThreshold()
    {
      return (sbyte) this.myMeters.GetParameter(EDCL_Params.cfg_coil_max_threshold);
    }

    public sbyte GetCoilMinThreshold()
    {
      return (sbyte) this.myMeters.GetParameter(EDCL_Params.cfg_coil_min_threshold);
    }

    public sbyte GetLoRa_Adr() => (sbyte) this.myMeters.GetParameter(EDCL_Params.lora_adr);

    public void SetLoRa_Adr(bool value)
    {
      this.myMeters.SetParameter(EDCL_Params.lora_adr, (object) value);
    }

    public void Set_cfg_max_a_b_pulse_diff(byte value)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.cfg_max_a_b_pulse_diff, value);
    }

    public void Set_cfg_max_b_a_pulse_diff(byte value)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.cfg_max_b_a_pulse_diff, value);
    }

    public void Set_cfg_max_difference_from_max_undamped_coil_counter(short value)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<short>(EDCL_Params.cfg_max_difference_from_max_undamped_coil_counter, value);
    }

    public void Set_cfg_min_undamped_coil_pulses(byte value)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.cfg_min_undamped_coil_pulses, value);
    }

    public byte Get_cfg_max_a_b_pulse_diff()
    {
      return (byte) this.myMeters.GetParameter(EDCL_Params.cfg_max_a_b_pulse_diff);
    }

    public byte Get_cfg_max_b_a_pulse_diff()
    {
      return (byte) this.myMeters.GetParameter(EDCL_Params.cfg_max_b_a_pulse_diff);
    }

    public short Get_cfg_max_difference_from_max_undamped_coil_counter()
    {
      return (short) this.myMeters.GetParameter(EDCL_Params.cfg_max_difference_from_max_undamped_coil_counter);
    }

    public byte Get_cfg_min_undamped_coil_pulses()
    {
      return (byte) this.myMeters.GetParameter(EDCL_Params.cfg_min_undamped_coil_pulses);
    }

    public byte Get_cfg_min_dac_offset()
    {
      return (byte) this.myMeters.GetParameter(EDCL_Params.cfg_min_dac_offset);
    }

    public byte Get_cfg_max_dac_offset()
    {
      return (byte) this.myMeters.GetParameter(EDCL_Params.cfg_max_dac_offset);
    }

    public byte Get_cfg_start_litre_diff()
    {
      return (byte) this.myMeters.GetParameter(EDCL_Params.cfg_start_litre_diff);
    }

    public void Set_cfg_start_litre_diff(byte value)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.cfg_start_litre_diff, value);
    }

    public void Set_cfg_min_dac_offset(byte value)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.cfg_min_dac_offset, value);
    }

    public void Set_cfg_max_dac_offset(byte value)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(EDCL_Params.cfg_max_dac_offset, value);
    }

    public void Set_NBIoT_AesKey(byte[] value)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte[]>(EDCL_Params.cfg_NBIOT_aesKey, value);
    }

    public byte[] GetAesKey()
    {
      return this.myMeters.WorkMeter.meterMemory.GetData(EDCL_Params.Mbus_aes_key);
    }

    public bool AccessRadioKey_IsOK()
    {
      return (uint) this.myMeters.GetParameter(EDCL_Params.cfg_AccessRadioKey) == 386474500U;
    }

    public Calibration GetCalibration()
    {
      return (Calibration) this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.device_is_calibrated);
    }

    public CalibrationFault GetCalibrationFault()
    {
      return (CalibrationFault) this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.calib_fault_flags);
    }

    public override SortedList<long, SortedList<DateTime, double>> GetValues(int subDevice = 0)
    {
      return this.myMeters.GetValues();
    }

    public async Task TransmitModulatedCarrierAsync(
      ushort timeout,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Radio.TransmitModulatedCarrierAsync(timeout, progress, token);
    }

    public async Task TransmitUnmodulatedCarrierAsync(
      ushort timeout,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Radio.TransmitUnmodulatedCarrierAsync(timeout, progress, token);
    }

    public async Task SendTestPacketAsync(
      ProgressHandler progress,
      CancellationToken token,
      ushort interval,
      ushort timeoutInSec,
      uint deviceID,
      byte[] arbitraryData,
      string syncWord = "91D3")
    {
      await this.Prepare(progress, token);
      if (deviceID == 0U)
        await this.cmd.Radio.SendTestPacketAsync(interval, timeoutInSec, arbitraryData, progress, token);
      else
        await this.cmd.Radio.SendTestPacketAsync(interval, timeoutInSec, deviceID, arbitraryData, progress, token, syncWord);
    }

    public async Task<double?> ReceiveTestPacketAsync(
      ProgressHandler progress,
      CancellationToken token,
      byte timeoutInSec,
      uint deviceID,
      string syncWord = "91D3")
    {
      await this.Prepare(progress, token);
      double? testPacketAsync = await this.cmd.Radio.ReceiveTestPacketAsync(timeoutInSec, deviceID, progress, token, syncWord);
      return testPacketAsync;
    }

    public async Task<bool> ReceiveTestPacket(
      byte timeoutInSec,
      ProgressHandler progress,
      CancellationToken token)
    {
      DateTime start = DateTime.Now;
      DateTime end = start.AddSeconds((double) timeoutInSec);
      while (DateTime.Now <= end)
      {
        byte[] buffer = await this.cmd.Radio.ReceiveRadio3Scenario3TelegramViaRadioAsync((byte) 5, new byte[2], timeoutInSec, progress, token);
        this.Rec_Pac_470.Clear();
        byte[] numArray = buffer;
        for (int index = 0; index < numArray.Length; ++index)
        {
          byte item = numArray[index];
          this.Rec_Pac_470.Add(item);
        }
        numArray = (byte[]) null;
        if (buffer != null && buffer.Length != 0)
        {
          string data = Encoding.ASCII.GetString(buffer, 2, 4);
          if (!(data != "PONG"))
            return true;
        }
        else
          buffer = (byte[]) null;
      }
      return false;
    }

    public async Task SetFrequencyIncrementAsync(
      ProgressHandler progress,
      CancellationToken token,
      int frequency_Hz)
    {
      await this.Prepare(progress, token);
      await this.cmd.Radio.SetFrequencyIncrementAsync(frequency_Hz, progress, token);
    }

    public async Task<Common32BitCommands.SystemTime> ReadSystemTimeAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      Common32BitCommands.SystemTime systemTimeAsync = await this.cmd.Device.GetSystemTimeAsync(progress, token);
      return systemTimeAsync;
    }

    public async Task WriteSystemTimeAsync(
      ProgressHandler progress,
      CancellationToken token,
      Common32BitCommands.SystemTime sysTime)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.SetSystemTimeAsync(sysTime, progress, token);
    }

    public override async Task SetSystemTimeAsync(
      ProgressHandler progress,
      CancellationToken token,
      Common32BitCommands.SystemTime sysTime)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.SetSystemTimeAsync(sysTime, progress, token);
    }

    public async Task ClearResetCounterAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.ClearResetCounterAsync(progress, token);
    }

    public async Task ClearFlowCheckStatesAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Special.ClearFlowCheckStatesAsync(ushort.MaxValue, progress, token);
    }

    public async Task<byte> GetResetCounterAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte resetCounterAsync = await this.cmd.Device.GetResetCounterAsync(progress, token);
      return resetCounterAsync;
    }

    public async Task SwitchLoRaLEDAsync(
      byte state,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.SwitchLoRaLEDAsync(state, progress, token);
    }

    public async Task SetModeAsync(ProgressHandler progress, CancellationToken token, Mode mode)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.SetModeAsync((byte) mode, progress, token);
    }

    public override async Task SetModeAsync(
      ProgressHandler progress,
      CancellationToken token,
      Enum modus)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.SetModeAsync((byte) (HandlerFunctionsForProduction.CommonDeviceModes) modus, progress, token);
    }

    public override async Task<byte> GetModeAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte modeAsync = await this.cmd.Device.GetModeAsync(progress, token);
      return modeAsync;
    }

    public async Task BackupDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.BackupDeviceAsync(progress, token);
    }

    public async Task Calibrate_VCC2(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.myMeters.WriteParameterAsync(progress, token, EDCL_Params.hwStatusFlags, new byte[1]);
      await this.cmd.Device.SetModeAsync((byte) 4, progress, token);
      ConfigList configList = new ConfigList();
      int old = configList.RecTime_BeforFirstByte;
      try
      {
        configList.RecTime_BeforFirstByte = 70000;
        DateTime endtime = DateTime.Now.AddMilliseconds((double) configList.RecTime_BeforFirstByte);
        while (true)
        {
          if (DateTime.Now < endtime)
          {
            try
            {
              MBusFrame frame = this.cmd.Device.DeviceCMD.MBus.Repeater.ReadMBusFrame();
              if (frame.UserData[13] != (byte) 254)
              {
                if (frame.UserData[13] != byte.MaxValue)
                  frame = (MBusFrame) null;
                else
                  break;
              }
              else
                goto label_13;
            }
            catch
            {
              configList.RecTime_BeforFirstByte = Convert.ToInt32((endtime - DateTime.Now).TotalMilliseconds);
              if (configList.RecTime_BeforFirstByte < 0)
                break;
            }
          }
          else
            break;
        }
        throw new Exception("Calibration of VCC2 failed!");
      }
      finally
      {
        configList.RecTime_BeforFirstByte = old;
      }
label_13:
      configList = (ConfigList) null;
    }

    public async Task DisableIrDaAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.ExecuteEventAsync((byte) 0, progress, token);
    }

    internal async Task ConnectDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.myMeters.ConnectDeviceAsync(progress, token);
    }

    public async Task<byte> GetRadioState(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte radioState = await this.cmd.Device.GetRadioOperationAsync(progress, token);
      return radioState;
    }

    public async Task<bool> SetRadioState(
      byte state,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.SetRadioOperationAsync(state, progress, token);
      return true;
    }

    public async Task<byte[]> GetCommunicationScenario(
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      Common32BitCommands.Scenarios CommunicationScenario = await this.cmd.Device.GetCommunicationScenarioAsync(progress, token);
      byte[] bytes = BitConverter.GetBytes(CommunicationScenario.ScenarioOne.Value);
      CommunicationScenario = (Common32BitCommands.Scenarios) null;
      return bytes;
    }

    public async Task<bool> SetCommunicationScenario(
      byte[] CommunicationScenario,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.SetCommunicationScenarioAsync(CommunicationScenario, progress, token);
      return true;
    }

    public async Task SetRadioPower(byte value, ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Radio.SetTransmitPowerAsync((ushort) value, progress, token);
    }

    public async Task<ushort> GetRadioPower(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      ushort power = await this.cmd.Radio.GetTransmitPowerAsync(progress, token);
      return power;
    }

    public async Task<byte> GetRadioMode(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte carrierModeAsync = await this.cmd.Radio.GetCarrierModeAsync(progress, token);
      return carrierModeAsync;
    }

    public async Task SetRadioMode(byte mode, ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Radio.SetCarrierModeAsync(mode, progress, token);
    }

    public async Task<int> GetFrequencyOffset(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      int frequencyIncrementAsync = await this.cmd.Radio.GetFrequencyIncrementAsync(progress, token);
      return frequencyIncrementAsync;
    }

    public async Task SetFrequencyOffset(
      int value,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Radio.SetFrequencyIncrementAsync(value, progress, token);
    }

    private async Task Prepare(ProgressHandler progress, CancellationToken token)
    {
      if (this.cmd == null)
        throw new ArgumentNullException("cmd");
      if (this.myMeters == null)
        throw new ArgumentNullException("myMeters");
      if (this.myMeters.WorkMeter != null)
        return;
      await this.myMeters.ConnectDeviceAsync(progress, token);
    }

    public override void OpenType(int meterInfoID) => this.myMeters.OpenType(meterInfoID);

    public override void OverwriteFromType(CommonOverwriteGroups[] overwriteGroups)
    {
      this.OverwriteSrcToDest(HandlerMeterObjects.TypeMeter, HandlerMeterObjects.WorkMeter, overwriteGroups);
    }

    public override void OverwriteSrcToDest(
      HandlerMeterObjects sourceObject,
      HandlerMeterObjects destinationObject,
      CommonOverwriteGroups[] overwriteGroups)
    {
      SortedList<HandlerMeterObjects, DeviceMemory> allMeterMemories = this.myMeters.GetAllMeterMemories();
      int index1 = allMeterMemories.IndexOfKey(sourceObject);
      if (index1 < 0)
        throw new Exception("Overwrite source object not found:" + sourceObject.ToString());
      int index2 = allMeterMemories.IndexOfKey(destinationObject);
      if (index2 < 0)
        throw new Exception("Overwrite destination object not found:" + destinationObject.ToString());
      foreach (CommonOverwriteGroups overwriteGroup in overwriteGroups)
      {
        List<string> stringList = new List<string>();
        foreach (string parameterName in EDCL_DeviceMemory.ParameterGroups[overwriteGroup])
        {
          if (allMeterMemories.Values[index2].IsParameterAvailable(parameterName) && allMeterMemories.Values[index1].IsParameterAvailable(parameterName))
            stringList.Add(parameterName);
        }
        allMeterMemories.Values[index2].OverwriteUsedParameters(allMeterMemories.Values[index1], stringList.ToArray());
        if (overwriteGroup == CommonOverwriteGroups.IdentData && destinationObject == HandlerMeterObjects.WorkMeter)
          this.myMeters.WorkMeter.deviceIdentification.SetCRC();
      }
    }

    public override CommonOverwriteGroups[] GetAllOverwriteGroups()
    {
      return HandlerFunctionsForProduction.GetImplementedOverwriteGroups(Enum.GetNames(typeof (EDCL_HandlerFunctions.OverwriteGroups)));
    }

    public override void SaveMeterObject(HandlerMeterObjects meterObject)
    {
      this.myMeters.SaveMeterObject(meterObject);
    }

    public async Task WriteMeterValue(int value, ProgressHandler progress, CancellationToken token)
    {
      await this.cmd.MBusCmd.SetChannelValueAsync((byte) 0, (uint) value, progress, token);
    }

    public async Task<int> ReadMeterValue(ProgressHandler progress, CancellationToken token)
    {
      uint channelValueAsync = await this.cmd.MBusCmd.GetChannelValueAsync((byte) 0, progress, token);
      return (int) channelValueAsync;
    }

    public async Task<long> ReadRadio3_IDAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      MBusChannelIdentification ident = await this.cmd.MBusCmd.GetChannelIdentificationAsync((byte) 85, progress, token);
      long serialNumber = ident.SerialNumber;
      ident = (MBusChannelIdentification) null;
      return serialNumber;
    }

    public async Task WriteRadio3_IDAsync(
      ProgressHandler progress,
      CancellationToken token,
      long radio3_ID)
    {
      MBusChannelIdentification ident = new MBusChannelIdentification()
      {
        Channel = 85,
        SerialNumber = Util.ConvertInt64ToBcdInt64(radio3_ID),
        Medium = "OTHER",
        Manufacturer = "@@@",
        Generation = 0
      };
      await this.Prepare(progress, token);
      await this.cmd.MBusCmd.SetChannelIdentificationAsync(ident, progress, token);
      ident = (MBusChannelIdentification) null;
    }

    public async Task SetChannelConfiguration(
      byte Channel,
      int Mantissa,
      sbyte Exponent,
      byte VIF,
      ProgressHandler progress,
      CancellationToken token)
    {
      MBusChannelConfiguration mbChannelConfig = new MBusChannelConfiguration();
      mbChannelConfig.Channel = Channel;
      mbChannelConfig.Mantissa = BitConverter.GetBytes(Mantissa);
      mbChannelConfig.Exponent = Exponent;
      mbChannelConfig.VIF = VIF;
      await this.cmd.MBusCmd.SetChannelConfigurationAsync(mbChannelConfig, progress, token);
      mbChannelConfig = (MBusChannelConfiguration) null;
    }

    public int GetMeterValue()
    {
      return Convert.ToInt32(this.myMeters.GetParameter(EDCL_Params.pulseReading));
    }

    public List<VolumeMonitorEventArgs> StartVolumeMonitor(int count)
    {
      this.maxCountofVolumeMonitorData = new int?(count);
      try
      {
        this.StartVolumeMonitor();
        if (this.volumeMonitorData != null && this.volumeMonitorData.Count > count)
          this.volumeMonitorData.RemoveRange(count, this.volumeMonitorData.Count - count);
        return this.volumeMonitorData;
      }
      finally
      {
        this.maxCountofVolumeMonitorData = new int?();
      }
    }

    public void StartVolumeMonitor()
    {
      this.stopwatch = new Stopwatch();
      if (this.maxCountofVolumeMonitorData.HasValue)
        this.volumeMonitorData = new List<VolumeMonitorEventArgs>();
      this.volumeMonitorQueue = new Queue<byte>();
      short num1 = 0;
      ushort num2 = 0;
      ushort num3 = 0;
      this.cmd.StartVolumeMonitor();
      this.stopwatch.Reset();
      this.stopwatch.Start();
      while (true)
      {
        do
        {
          if (!this.maxCountofVolumeMonitorData.HasValue || this.maxCountofVolumeMonitorData.Value > this.volumeMonitorData.Count)
          {
            if (this.stopwatch.IsRunning && this.stopwatch.ElapsedMilliseconds <= 5000L)
            {
              byte[] buffer;
              try
              {
                if (!this.cmd.GetCurrentInputBuffer(out buffer) || buffer == null)
                  continue;
              }
              catch (FramingErrorException ex)
              {
                continue;
              }
              for (int index = 0; index < buffer.Length; ++index)
                this.volumeMonitorQueue.Enqueue(buffer[index]);
            }
            else
              goto label_24;
          }
          else
            goto label_3;
        }
        while (this.volumeMonitorQueue.Count < 12);
        bool flag = false;
        while (!flag)
        {
          while (this.volumeMonitorQueue.Count > 0 && this.volumeMonitorQueue.Peek() != (byte) 2 && !flag)
          {
            byte num4 = this.volumeMonitorQueue.Dequeue();
            if (EDCL_HandlerFunctions.logger.IsWarnEnabled)
              EDCL_HandlerFunctions.logger.Warn("Synchronize... 0x" + num4.ToString("X2"));
          }
          if (this.volumeMonitorQueue.Count != 0 && this.volumeMonitorQueue.Count >= 12)
          {
            int num5 = (int) this.volumeMonitorQueue.Dequeue();
            string empty = string.Empty;
            byte num6;
            byte num7;
            byte num8;
            byte num9;
            byte num10;
            try
            {
              empty = Encoding.ASCII.GetString(new byte[2]
              {
                this.volumeMonitorQueue.Dequeue(),
                this.volumeMonitorQueue.Dequeue()
              }, 0, 2);
              num6 = Convert.ToByte(empty, 16);
              empty = Encoding.ASCII.GetString(new byte[2]
              {
                this.volumeMonitorQueue.Dequeue(),
                this.volumeMonitorQueue.Dequeue()
              }, 0, 2);
              num7 = Convert.ToByte(empty, 16);
              empty = Encoding.ASCII.GetString(new byte[2]
              {
                this.volumeMonitorQueue.Dequeue(),
                this.volumeMonitorQueue.Dequeue()
              }, 0, 2);
              num8 = Convert.ToByte(empty, 16);
              empty = Encoding.ASCII.GetString(new byte[2]
              {
                this.volumeMonitorQueue.Dequeue(),
                this.volumeMonitorQueue.Dequeue()
              }, 0, 2);
              num9 = Convert.ToByte(empty, 16);
              empty = Encoding.ASCII.GetString(new byte[2]
              {
                this.volumeMonitorQueue.Dequeue(),
                this.volumeMonitorQueue.Dequeue()
              }, 0, 2);
              num10 = Convert.ToByte(empty, 16);
            }
            catch (Exception ex)
            {
              EDCL_HandlerFunctions.logger.Error("Error: {0}, VolumeMonitorQueue.Count: {1}, Last ASCII: {2}, " + Environment.NewLine + " Trace: {3}", new object[4]
              {
                (object) ex.Message,
                (object) this.volumeMonitorQueue.Count,
                (object) empty,
                (object) ex.StackTrace
              });
              if (flag)
              {
                this.cmd.StopVolumeMonitor();
                return;
              }
              this.stopwatch.Reset();
              this.stopwatch.Start();
              break;
            }
            if (this.volumeMonitorQueue.Dequeue() == (byte) 3 && (int) (byte) ((uint) num6 + (uint) num7 + (uint) num8 + (uint) num9) == (int) num10)
            {
              VolumeMonitorEventArgs e = new VolumeMonitorEventArgs();
              e.CoilDetectionResult_A = num6;
              e.CoilDetectionResult_B = num7;
              e.StateMachineValue = num8;
              e.PollCounter = num9;
              switch (e.CoilSampleResult)
              {
                case 1:
                  ++num1;
                  break;
                case 2:
                  --num1;
                  ++num2;
                  break;
                case 3:
                  ++num3;
                  break;
                case 4:
                  --num1;
                  ++num2;
                  break;
                case 6:
                  ++num3;
                  break;
                case 7:
                  ++num1;
                  break;
                case 8:
                  ++num1;
                  break;
                case 9:
                  ++num3;
                  break;
                case 11:
                  --num1;
                  ++num2;
                  break;
                case 12:
                  ++num3;
                  break;
                case 13:
                  --num1;
                  ++num2;
                  break;
                case 14:
                  ++num1;
                  break;
              }
              e.PulseForwardCount = num1;
              e.PulseReturnCount = num2;
              e.PulseErrorCount = num3;
              if (this.maxCountofVolumeMonitorData.HasValue)
                this.volumeMonitorData.Add(e);
              if (this.OnVolumeMonitorDataReceived != null)
                this.OnVolumeMonitorDataReceived((object) this, e);
              flag = e.Cancel;
              if (flag)
              {
                this.cmd.StopVolumeMonitor();
                return;
              }
              this.stopwatch.Reset();
              this.stopwatch.Start();
            }
            else
              break;
          }
          else
            break;
        }
      }
label_3:
      this.cmd.StopVolumeMonitor();
      return;
label_24:;
    }

    public void StopVolumeMonitor() => this.cmd.StopVolumeMonitor();

    public void ClearVolumeMonitorData()
    {
      if (this.volumeMonitorQueue != null)
        this.volumeMonitorQueue.Clear();
      if (this.volumeMonitorData == null)
        return;
      this.volumeMonitorData.Clear();
    }

    public List<VolumeMonitorEventArgs> GetVolumeMonitorData() => this.volumeMonitorData;

    public DateTime SaveVolumeMonitorData(int meterID, List<VolumeMonitorEventArgs> data)
    {
      if (data == null || data.Count == 0)
        throw new Exception("No volume monitor data to save!");
      List<byte> byteList = new List<byte>();
      foreach (VolumeMonitorEventArgs monitorEventArgs in data)
        byteList.AddRange(monitorEventArgs.ToByteArray());
      byte[] zippedBuffer = Util.Zip(byteList.ToArray());
      return GmmDbLib.MeterData.InsertData(DbBasis.PrimaryDB.BaseDbConnection, meterID, GmmDbLib.MeterData.Special.EdcEncabulator, zippedBuffer);
    }

    public async Task SendUnconfirmedDataAsync(
      byte[] data,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.LoRa.SendUnconfirmedDataAsync(data, progress, token);
    }

    public async Task SendConfirmedDataAsync(
      byte[] data,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.LoRa.SendConfirmedDataAsync(data, progress, token);
    }

    public byte[] Get_VCC2fifo()
    {
      if (this.myMeters == null || this.myMeters.WorkMeter == null || this.myMeters.WorkMeter.meterMemory == null)
        throw new ArgumentNullException();
      return this.myMeters.WorkMeter.meterMemory.GetData(EDCL_Params.VCC2fifo);
    }

    public ushort Get_vdda()
    {
      if (this.myMeters == null || this.myMeters.WorkMeter == null || this.myMeters.WorkMeter.meterMemory == null)
        throw new ArgumentNullException();
      return this.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.vdda);
    }

    public async Task<byte[]> GetNBIoT_NBIoTIMEI_NB(
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte[] nbIoTImeiNbAsync = await this.cmd.NBIoT.GetNBIoT_IMEI_NBAsync(progress, token);
      return nbIoTImeiNbAsync;
    }

    public async Task<byte[]> GetNBIoT_SIMIMSI_NB(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte[] simImsiNbAsync = await this.cmd.NBIoT.Get_SIM_IMSI_NBAsync(progress, token);
      return simImsiNbAsync;
    }

    public async Task<byte[]> GetNBIoT_SIMICCID_NB(
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte[] simIccidNbAsync = await this.cmd.NBIoT.GetSIM_ICCID_NBAsync(progress, token);
      return simIccidNbAsync;
    }

    public async Task<byte[]> GetIMEI_IMSI_NBVER_RAM(
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte[] imsiNbverRamAsync = await this.cmd.NBIoT.GetNBIoT_IMEI_IMSI_NBVER_RAMAsync(progress, token);
      return imsiNbverRamAsync;
    }

    public async Task<byte[]> GetICCID_IMSI_RAM(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte[] iccidImsiRamAsync = await this.cmd.NBIoT.GetNBIoT_ICCID_IMSI_RAMAsync(progress, token);
      return iccidImsiRamAsync;
    }

    public async Task SetIMEI_IMSI_NBVER_RAM(
      byte[] data,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.NBIoT.SetNBIoT_IMEI_IMSI_NBVER_RAMAsync(data, progress, token);
    }

    public async Task<byte[]> GetNBIoT_FWVersion(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte[] firmwareVersionAsync = await this.cmd.NBIoT.GetNBIoT_FirmwareVersionAsync(progress, token);
      return firmwareVersionAsync;
    }

    public async Task SetNBIoT_PowerOn(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.NBIoT.SetNBIoT_PowerON(progress, token);
    }

    public async Task SetNBIoT_PowerOff(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.NBIoT.SetNBIoT_PowerOFF(progress, token);
    }

    public async Task SetNBIoT_DevEUI(
      byte[] DevEUI,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.NBIoT.SetDevEUIAsync(DevEUI, progress, token);
    }

    public async Task SetNBIoT_Band(byte[] band, ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.NBIoT.SetNBIoT_Band(band, progress, token);
    }

    public async Task<byte[]> GetNBIoT_Band(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte[] nbIoTBand = await this.cmd.NBIoT.GetNBIoT_Band(progress, token);
      return nbIoTBand;
    }

    public async Task SetNBIoT_SecondaryBand(
      byte[] band,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.NBIoT.SetNBIoT_SecondaryBand(band, progress, token);
    }

    public async Task<byte[]> GetNBIoT_SecondaryBand(
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte[] ioTSecondaryBand = await this.cmd.NBIoT.GetNBIoT_SecondaryBand(progress, token);
      return ioTSecondaryBand;
    }

    public async Task SetNBIoT_Operator(
      byte[] Operator,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.NBIoT.SetNBIoT_Operator(Operator, progress, token);
    }

    public async Task GetNBIoT_Operator(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte[] nbIoTOperator = await this.cmd.NBIoT.GetNBIoT_Operator(progress, token);
    }

    public async Task SetNBIoT_RemoteIP(
      byte[] Operator,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.NBIoT.SetNBIoT_RemoteIP(Operator, progress, token);
    }

    public async Task<byte[]> GetNBIoT_RemoteIP(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte[] nbIoTRemoteIp = await this.cmd.NBIoT.GetNBIoT_RemoteIP(progress, token);
      return nbIoTRemoteIp;
    }

    public async Task SetNBIoT_RemotePort(
      byte[] Operator,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.NBIoT.SetNBIoT_RemotePort(Operator, progress, token);
    }

    public async Task<byte[]> GetNBIoT_RemotePort(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte[] nbIoTRemotePort = await this.cmd.NBIoT.GetNBIoT_RemotePort(progress, token);
      return nbIoTRemotePort;
    }

    public async Task SetNBIoT_Protocol(
      byte[] Protocol,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.NBIoT.SetNBIoT_Protocol(Protocol, progress, token);
    }

    public async Task<byte[]> GetNBIoT_Protocol(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte[] nbIoTProtocol = await this.cmd.NBIoT.GetNBIoT_Protocol(progress, token);
      return nbIoTProtocol;
    }

    public async Task SetNBIoT_RadioFullFunctionOn(
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte[] numArray = await this.cmd.NBIoT.SetNBIoT_RadioFullFunctionOn(progress, token);
    }

    public async Task SetNBIoT_RadioFullFunctionOff(
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte[] numArray = await this.cmd.NBIoT.SetNBIoT_RadioFullFunctionOff(progress, token);
    }

    public async Task<byte[]> NBIoT_SendTestData(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte[] numArray = await this.cmd.NBIoT.NBIoT_SendTestData(progress, token);
      return numArray;
    }

    public async Task NBIoT_SendUnconfirmedDataAsync(
      byte[] data,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.NBIoT.SendUnconfirmedDataAsync(data, progress, token);
    }

    public async Task NBIoT_SendActivedPacket(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.NBIoT.NBIoT_SendActivePacket(progress, token);
    }

    public void SetFD_NB_ServerIP(byte[] value)
    {
      this.myMeters.SetParameter(EDCL_Params.FD_NB_ServerIP, (object) value);
    }

    public byte[] Getcfg_NBIOT_ServerIP()
    {
      return this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte[]>(EDCL_Params.cfg_NBIOT_ServerIP);
    }

    public void SetFD_NB_ServerPort(byte[] value)
    {
      this.myMeters.SetParameter(EDCL_Params.FD_NB_ServerPort, (object) value);
    }

    public byte[] Getcfg_NBIOT_ServerPort()
    {
      return this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte[]>(EDCL_Params.cfg_NBIOT_ServerPort);
    }

    public async Task SetDNSName(byte[] name, ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.NBIoT.SetDNSNameAsync(name, progress, token);
    }

    public async Task<byte[]> GetDNSName(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte[] dnsNameAsync = await this.cmd.NBIoT.GetDNSNameAsync(progress, token);
      return dnsNameAsync;
    }

    public async Task SetReligiousDay(
      bool Enabled,
      ProgressHandler progress,
      CancellationToken token,
      byte SecondByte = 0)
    {
      byte[] data = new byte[2]{ (byte) 0, SecondByte };
      if (Enabled)
        data[0] = (byte) 1;
      await this.Prepare(progress, token);
      await this.cmd.Special.SetReligiousDay(data, progress, token);
      data = (byte[]) null;
    }

    public SortedList<string, byte[]> GetDeliveryNoteParameters()
    {
      if (this.myMeters.BackupMeter.meterMemory == null)
        throw new Exception("Backup meter not loaded!");
      SortedList<string, byte[]> deliveryNoteParameters = new SortedList<string, byte[]>();
      foreach (KeyValuePair<string, string> keyValuePair in new SortedList<string, string>()
      {
        {
          "IMSI",
          "NBIoT_ModuleIMSI"
        },
        {
          "IMEI",
          "NBIoT_ModuleIMEI"
        },
        {
          "CommunicationScenario",
          "cfg_communication_scenario"
        },
        {
          "Protocol",
          "cfg_NBIOT_protocol"
        },
        {
          "DevEUI",
          "cfg_lora_deveui"
        },
        {
          "AesKey",
          "cfg_NBIOT_aesKey"
        },
        {
          "NBServerIP",
          "cfg_NBIOT_ServerIP"
        },
        {
          "NBServerPort",
          "cfg_NBIOT_ServerPort"
        },
        {
          "NBServerDNS",
          "cfg_nbiot_domain_name"
        },
        {
          "ProtocolNBIoTVersion_Main",
          "protocol_nbiot_version_major"
        },
        {
          "ProtocolNBIoTVersion_Middle",
          "protocol_nbiot_version_middle"
        },
        {
          "ProtocolNBIoTVersion_Minor",
          "protocol_nbiot_version_minor"
        }
      })
      {
        deliveryNoteParameters.Add(keyValuePair.Key, (byte[]) null);
        try
        {
          deliveryNoteParameters[keyValuePair.Key] = this.myMeters.BackupMeter.meterMemory.GetData(this.myMeters.BackupMeter.meterMemory.MapDef.GetParameter(keyValuePair.Value).AddressRange);
        }
        catch
        {
        }
      }
      return deliveryNoteParameters;
    }

    public bool GetDataFromBackupMeter(
      string ParameterName,
      out byte[] ParameterValue,
      out string FailString)
    {
      ParameterValue = (byte[]) null;
      FailString = string.Empty;
      if (this.myMeters.BackupMeter == null)
      {
        FailString = "Backup meter not loaded!";
        return false;
      }
      if (!this.myMeters.BackupMeter.meterMemory.MapDef.MapVars.ContainsKey(ParameterName))
      {
        FailString = "Parameter not found!";
        return false;
      }
      if (!this.myMeters.BackupMeter.meterMemory.AreDataAvailable(this.myMeters.BackupMeter.meterMemory.GetAddressRange(ParameterName)))
      {
        FailString = "Address not available!";
        return false;
      }
      try
      {
        ParameterValue = this.myMeters.BackupMeter.meterMemory.GetData(this.myMeters.BackupMeter.meterMemory.GetAddressRange(ParameterName));
      }
      catch
      {
        FailString = "Get parameter value failed!";
        return false;
      }
      return true;
    }

    public void Set_PulseMulitplier_CogCount_ReadingUnit(
      int PulseMultiplier,
      int CogCount,
      int ReadingUnit)
    {
      this.myMeters.SetParameter(EDCL_Params.cfg_pulse_multiplier, (object) PulseMultiplier);
      this.myMeters.SetParameter(EDCL_Params.cfg_cog_count, (object) CogCount);
      this.myMeters.SetParameter(EDCL_Params.cfg_reading_unit, (object) ReadingUnit);
    }

    public enum SamplingRate
    {
      Unknown,
      Normal_192_Low_32,
      Normal_128_Low_32,
      Normal_64_Low_32,
      Normal_8_Low_8,
      Normal_40_Low_16,
    }

    public enum OverwriteGroups
    {
      IdentData = 0,
      BasicConfiguration = 8,
    }
  }
}
