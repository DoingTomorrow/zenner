// Decompiled with JetBrains decompiler
// Type: M8_Handler.M8_HandlerFunctions
// Assembly: M8_Handler, Version=2.0.6.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 582F1296-F274-42DF-B72B-4C0B4D92AA72
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\M8_Handler.dll

using CommunicationPort.Functions;
using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace M8_Handler
{
  public sealed class M8_HandlerFunctions : HandlerFunctionsForProduction
  {
    public const string HandlerName = "M8_Handler";
    private CommunicationPortFunctions port;
    private BaseDbConnection db;
    internal M8_DeviceCommands cmd;
    internal M8_AllMeters myMeters;

    public M8_HandlerFunctions(CommunicationPortFunctions myPort, BaseDbConnection myDb)
      : this(myPort)
    {
      this.db = myDb;
    }

    public M8_HandlerFunctions(CommunicationPortFunctions myPort)
      : this()
    {
      this.port = myPort;
      this.cmd = new M8_DeviceCommands(this.port);
      this.myMeters = new M8_AllMeters(this);
    }

    public M8_HandlerFunctions(BaseDbConnection myDb)
      : this()
    {
      this.db = myDb;
      this.myMeters = new M8_AllMeters(this);
    }

    public M8_HandlerFunctions() => this.myMeters = new M8_AllMeters(this);

    public override bool LoadLastBackup(int meterID)
    {
      BaseTables.MeterDataRow meterDataRow = GmmDbLib.MeterData.LoadLastBackup(DbBasis.PrimaryDB.BaseDbConnection, meterID);
      if (meterDataRow == null)
        return false;
      this.myMeters.SetBackupMeter(meterDataRow.PValueBinary);
      return true;
    }

    public override void SetReadoutConfiguration(ConfigList configList)
    {
      if (this.port == null)
        return;
      this.port.SetReadoutConfiguration(configList);
    }

    public override ConfigList GetReadoutConfiguration()
    {
      return this.port == null ? (ConfigList) null : this.port.GetReadoutConfiguration();
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
      this.myMeters = (M8_AllMeters) null;
    }

    public override void Clear() => this.myMeters = new M8_AllMeters(this);

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
      DeviceIdentification deviceIdentification = this.GetDeviceIdentification();
      if (deviceIdentification == null || !deviceIdentification.MeterID.HasValue)
        throw new ArgumentException("Invalid MeterID!");
      BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
      int meterID = (int) deviceIdentification.MeterID.Value;
      int meterInfoID = (int) deviceIdentification.MeterInfoID.Value;
      uint hardwareTypeID_OR_firmwareVersion = deviceIdentification.FirmwareVersion.Value;
      string serialNumberAsString = deviceIdentification.PrintedSerialNumberAsString;
      string orderNr = deviceIdentification.SAP_ProductionOrderNumber != null ? deviceIdentification.SAP_ProductionOrderNumber.ToString() : string.Empty;
      byte[] compressedData = this.myMeters.WorkMeter.meterMemory.GetCompressedData();
      DateTime? nullable = GmmDbLib.Device.Save(baseDbConnection, meterID, meterInfoID, hardwareTypeID_OR_firmwareVersion, serialNumberAsString, orderNr, compressedData, false);
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
      return 1;
    }

    public override DeviceIdentification GetDeviceIdentification()
    {
      return (DeviceIdentification) this.myMeters.WorkMeter.deviceIdentification;
    }

    public override void OpenType(int meterInfoID)
    {
      BaseType baseType = BaseType.GetBaseType(meterInfoID);
      if (baseType == null || baseType.Data == null)
        return;
      this.myMeters.TypeMeter = (M8_Meter) this.myMeters.CreateMeter(baseType.Data.EEPdata);
      this.myMeters.TypeMeter.BaseType = baseType;
      if (this.myMeters.WorkMeter == null)
        this.myMeters.WorkMeter = this.myMeters.TypeMeter.Clone();
      else
        this.myMeters.WorkMeter.BaseType = this.myMeters.TypeMeter.BaseType.DeepCopy();
    }

    public override void OverwriteFromType(CommonOverwriteGroups[] overwriteGroups)
    {
      this.OverwriteSrcToDest(HandlerMeterObjects.TypeMeter, HandlerMeterObjects.WorkMeter, overwriteGroups);
    }

    public override CommonOverwriteGroups[] GetAllOverwriteGroups()
    {
      return HandlerFunctionsForProduction.GetImplementedOverwriteGroups(Enum.GetNames(typeof (M8_HandlerFunctions.OverwriteGroups)));
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
        foreach (string parameterName in M8_DeviceMemory.ParameterGroups[overwriteGroup])
        {
          if (allMeterMemories.Values[index2].IsParameterAvailable(parameterName) && allMeterMemories.Values[index1].IsParameterAvailable(parameterName))
            stringList.Add(parameterName);
        }
        allMeterMemories.Values[index2].OverwriteUsedParameters(allMeterMemories.Values[index1], stringList.ToArray());
      }
    }

    public override async Task WriteDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.myMeters.WriteDeviceAsync(progress, token);
    }

    public void SetFD_Mbus_interval(ushort value)
    {
      this.myMeters.SetParameter(M8_Params.FD_Mbus_interval, (object) value);
    }

    public void SetFD_Mbus_nighttime_start(byte value)
    {
      this.myMeters.SetParameter(M8_Params.FD_Mbus_nighttime_start, (object) value);
    }

    public void SetFD_Mbus_nighttime_stop(byte value)
    {
      this.myMeters.SetParameter(M8_Params.FD_Mbus_nighttime_stop, (object) value);
    }

    public void SetFD_Mbus_radio_suppression_days(byte value)
    {
      this.myMeters.SetParameter(M8_Params.FD_Mbus_radio_suppression_days, (object) value);
    }

    public ushort GetFD_Mbus_interval()
    {
      if (!this.myMeters.WorkMeter.meterMemory.IsParameterAvailable(M8_Params.FD_Mbus_interval))
        throw new Exception("Parameter 'FD_Mbus_interval' is not available in the firmware.");
      return Convert.ToUInt16(this.myMeters.GetParameter(M8_Params.FD_Mbus_interval));
    }

    public ushort GetMbus_interval()
    {
      if (!this.myMeters.WorkMeter.meterMemory.IsParameterAvailable(M8_Params.Mbus_interval))
        throw new Exception("Parameter 'Mbus_interval' is not available in the firmware.");
      return Convert.ToUInt16(this.myMeters.GetParameter(M8_Params.Mbus_interval));
    }

    public byte GetMbus_nighttime_start()
    {
      return this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.Mbus_nighttime_start);
    }

    public byte GetMbus_nighttime_stop()
    {
      return this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.Mbus_nighttime_stop);
    }

    public byte GetMbus_radio_suppression_days()
    {
      return this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.Mbus_radio_suppression_days);
    }

    public byte Getcfg_transmission_scenario()
    {
      return this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.cfg_transmission_scenario);
    }

    public uint GetFD_Radio3_ID()
    {
      return Util.ConvertBcdUInt32ToUInt32(this.myMeters.WorkMeter.meterMemory.GetParameterValue<uint>(M8_Params.FD_Radio3_ID));
    }

    public void SetFD_Radio3_ID(uint value)
    {
      this.myMeters.SetParameter(M8_Params.FD_Radio3_ID, (object) Util.ConvertUnt32ToBcdUInt32(value));
    }

    public bool GetDevice_With_TRX()
    {
      return this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.Device_With_TRX) == (byte) 1;
    }

    public void SetRadioTransmitSyncWord(byte radio_transmit_sync1, byte radio_transmit_sync2)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(M8_Params.radio_transmit_sync1, radio_transmit_sync1);
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(M8_Params.radio_transmit_sync2, radio_transmit_sync2);
    }

    public void SetRadioResceiveSyncWord(byte radio_resceive_sync1, byte radio_resceive_sync2)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(M8_Params.radio_resceive_sync1, radio_resceive_sync1);
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(M8_Params.radio_resceive_sync2, radio_resceive_sync2);
    }

    public override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      int subDevice = 0)
    {
      return subDevice != 0 ? (SortedList<OverrideID, ConfigurationParameter>) null : this.myMeters.GetConfigurationParameters();
    }

    public override void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameter,
      int subDevice = 0)
    {
      if (subDevice != 0)
        throw new NotSupportedException();
      this.myMeters.SetConfigurationParameters(parameter);
    }

    public override SortedList<long, SortedList<DateTime, double>> GetValues(int subDevice = 0)
    {
      return this.myMeters.GetValues();
    }

    public async Task<Temperature> ReadTemperatureAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      M8_DeviceMemory meterMemory1 = this.myMeters.WorkMeter.meterMemory;
      M8_Params m8Params = M8_Params.runtime_flags;
      string parameterName1 = m8Params.ToString();
      uint address_runtime_flags = meterMemory1.GetParameterAddress(parameterName1);
      uint runtime_flags = 1;
      await this.cmd.Device.WriteMemoryAsync(progress, token, address_runtime_flags, BitConverter.GetBytes(runtime_flags));
      await Task.Delay(500, token);
      M8_DeviceMemory meterMemory2 = this.myMeters.WorkMeter.meterMemory;
      m8Params = M8_Params.ntc_rs_t;
      string parameterName2 = m8Params.ToString();
      uint address_ntc_rs_t = meterMemory2.GetParameterAddress(parameterName2);
      byte[] buffer_ntc_rs_t = await this.cmd.Device.ReadMemoryAsync(progress, token, address_ntc_rs_t, (byte) 4);
      if (buffer_ntc_rs_t == null)
        throw new HandlerMessageException("Can not read the temperature of environment (ntc_rs_t)!");
      M8_DeviceMemory meterMemory3 = this.myMeters.WorkMeter.meterMemory;
      m8Params = M8_Params.ntc_hs_t;
      string parameterName3 = m8Params.ToString();
      uint address_ntc_hs_t = meterMemory3.GetParameterAddress(parameterName3);
      byte[] buffer_ntc_hs_t = await this.cmd.Device.ReadMemoryAsync(progress, token, address_ntc_hs_t, (byte) 4);
      if (buffer_ntc_hs_t == null)
        throw new HandlerMessageException("Can not read the temperature of radiator (ntc_hs_t)!");
      M8_DeviceMemory meterMemory4 = this.myMeters.WorkMeter.meterMemory;
      m8Params = M8_Params.ntc_remote_t;
      string parameterName4 = m8Params.ToString();
      uint address_ntc_remote_t = meterMemory4.GetParameterAddress(parameterName4);
      byte[] buffer_ntc_remote_t = await this.cmd.Device.ReadMemoryAsync(progress, token, address_ntc_remote_t, (byte) 4);
      if (buffer_ntc_remote_t == null)
        throw new HandlerMessageException("Can not read the temperature of remote sensor (ntc_remote_t)!");
      int environment = BitConverter.ToInt32(buffer_ntc_rs_t, 0);
      int radiator = BitConverter.ToInt32(buffer_ntc_hs_t, 0);
      int remote = BitConverter.ToInt32(buffer_ntc_remote_t, 0);
      Temperature temperature = new Temperature()
      {
        Environment = (double) environment / 256.0,
        Radiator = (double) radiator / 256.0,
        Remote = (double) remote / 256.0
      };
      buffer_ntc_rs_t = (byte[]) null;
      buffer_ntc_hs_t = (byte[]) null;
      buffer_ntc_remote_t = (byte[]) null;
      return temperature;
    }

    public async Task<Tamper> ReadTamperSwitchAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      uint address_runtime_flags = this.myMeters.WorkMeter.meterMemory.GetParameterAddress(M8_Params.runtime_flags.ToString());
      uint runtime_flags = 131072;
      await this.cmd.Device.WriteMemoryAsync(progress, token, address_runtime_flags, BitConverter.GetBytes(runtime_flags));
      await Task.Delay(100, token);
      uint address_tamper_switch_state = this.myMeters.WorkMeter.meterMemory.GetParameterAddress(M8_Params.tamper_switch_state.ToString());
      byte[] buffer_tamper_switch_state = await this.cmd.Device.ReadMemoryAsync(progress, token, address_tamper_switch_state, (byte) 1);
      Tamper tamper = buffer_tamper_switch_state != null ? (Tamper) buffer_tamper_switch_state[0] : throw new HandlerMessageException("Can not read the state of tamper switch (tamper_switch_state)!");
      buffer_tamper_switch_state = (byte[]) null;
      return tamper;
    }

    public async Task TransmitModulatedCarrierAsync(
      ushort timeoutInSec,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Radio.TransmitModulatedCarrierAsync(timeoutInSec, progress, token);
    }

    public async Task TransmitUnmodulatedCarrierAsync(
      ushort timeoutInSec,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Radio.TransmitUnmodulatedCarrierAsync(timeoutInSec, progress, token);
    }

    public async Task StopRadioTests(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Radio.StopRadioTests(progress, token);
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

    public async Task SetCenterFrequencyAsync(
      ProgressHandler progress,
      CancellationToken token,
      uint frequency_Hz)
    {
      DeviceVersionMBus deviceVersionMbus = await this.cmd.Device.DeviceCMD.ReadVersionAsync(progress, token);
      await this.cmd.Radio.SetCenterFrequencyAsync(frequency_Hz, progress, token);
      await this.cmd.Device.ResetDeviceAsync(progress, token);
    }

    public async Task SetFrequencyIncrementAsync(
      ProgressHandler progress,
      CancellationToken token,
      int frequency_Hz)
    {
      await this.Prepare(progress, token);
      await this.cmd.Radio.SetFrequencyIncrementAsync(frequency_Hz, progress, token);
      await this.cmd.Device.ResetDeviceAsync(progress, token);
    }

    public async Task<int> GetFrequencyIncrementAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      int freOffSet = await this.cmd.Radio.GetFrequencyIncrementAsync(progress, token);
      return freOffSet;
    }

    public async Task SetLcdTestStateAsync(
      ProgressHandler progress,
      CancellationToken token,
      LcdTest lcdTest)
    {
      await this.SetLcdTestStateAsync(progress, token, (byte) lcdTest);
    }

    public override async Task SetLcdTestStateAsync(
      ProgressHandler progress,
      CancellationToken token,
      byte lcdTestState)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.SetLcdTestStateAsync(lcdTestState, (byte[]) null, progress, token);
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

    public override async Task ResetDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.ResetDeviceAsync(progress, token);
    }

    public override async Task ResetDiagnosticData(
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.ClearResetCounterAsync(progress, token);
    }

    public async Task ClearResetCounterAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.ClearResetCounterAsync(progress, token);
    }

    public override async Task SetModeAsync(
      ProgressHandler progress,
      CancellationToken token,
      Enum mode)
    {
      await this.Prepare(progress, token);
      Mode mode1 = (Mode) mode;
      switch (mode1)
      {
        case Mode.OperationMode:
          await this.cmd.Device.SetModeAsync((byte) 0, progress, token);
          break;
        case Mode.DeliveryMode:
          await this.cmd.Device.SetModeAsync((byte) 1, progress, token);
          break;
        case Mode.StandbyCurrentMode:
          await this.cmd.Device.ExecuteEventAsync((byte) 0, progress, token);
          break;
        case Mode.TemperatureCalibrationMode:
          await this.cmd.Device.SetModeAsync((byte) 3, progress, token);
          break;
        default:
          throw new NotImplementedException("SetModeAsync for mode: " + mode?.ToString());
      }
    }

    internal async Task ConnectDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.myMeters.ConnectDeviceAsync(progress, token);
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

    public bool AccessRadioKey_IsOK()
    {
      return (uint) this.myMeters.GetParameter(M8_Params.cfg_AccessRadioKey) == 386474500U;
    }

    public void SetKeydateToZero()
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(M8_Params.cfg_Key_date, (ushort) 0);
    }

    public enum OverwriteGroups
    {
      BasicConfiguration = 8,
    }
  }
}
