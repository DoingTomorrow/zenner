// Decompiled with JetBrains decompiler
// Type: PDCL2_Handler.PDCL2_HandlerFunctions
// Assembly: PDCL2_Handler, Version=2.22.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 03BA4C2D-69FE-4DA6-9C3F-B3D5471C4058
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDCL2_Handler.dll

using CommunicationPort.Functions;
using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace PDCL2_Handler
{
  public sealed class PDCL2_HandlerFunctions : 
    HandlerFunctionsForProduction,
    IReadoutConfig,
    IDisposable,
    IHandler
  {
    private static Logger logger = LogManager.GetLogger(nameof (PDCL2_HandlerFunctions));
    public const string HandlerName = "PDCL2_Handler";
    private CommunicationPortFunctions port;
    private BaseDbConnection db;
    internal PDCL2_DeviceCommands cmd;
    internal PDCL2_AllMeters myMeters;

    public event EventHandler<EncabulatorData> OnEncabulatorDataReceived;

    public PDCL2_HandlerFunctions(CommunicationPortFunctions myPort, BaseDbConnection myDb)
      : this(myPort)
    {
      this.db = myDb;
    }

    public PDCL2_HandlerFunctions(CommunicationPortFunctions myPort)
      : this()
    {
      this.port = myPort;
      this.cmd = new PDCL2_DeviceCommands(this.port);
      this.myMeters = new PDCL2_AllMeters(this);
    }

    public PDCL2_HandlerFunctions(BaseDbConnection myDb)
      : this()
    {
      this.db = myDb;
      this.myMeters = new PDCL2_AllMeters(this);
    }

    public PDCL2_HandlerFunctions() => this.myMeters = new PDCL2_AllMeters(this);

    public override void SetCommunicationPort(CommunicationPortFunctions myPort = null)
    {
      this.port = myPort;
      if (this.cmd != null)
        return;
      this.cmd = new PDCL2_DeviceCommands(this.port);
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
      this.myMeters = (PDCL2_AllMeters) null;
    }

    public override void Clear() => this.myMeters = new PDCL2_AllMeters(this);

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
      string serialNumberAsString = deviceIdentification.PrintedSerialNumberAsString;
      string orderNr = deviceIdentification.SAP_ProductionOrderNumber != null ? deviceIdentification.SAP_ProductionOrderNumber.ToString() : string.Empty;
      byte[] compressedData = this.myMeters.WorkMeter.meterMemory.GetCompressedData();
      DateTime? nullable = Device.Save(baseDbConnection, meterID, meterInfoID, hardwareTypeID_OR_firmwareVersion, serialNumberAsString, orderNr, compressedData, false);
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
      return 3;
    }

    public override async Task ResetDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.cmd.Device.ResetDeviceAsync(progress, token);
    }

    public void SetconfigParameters()
    {
      SortedList<OverrideID, ConfigurationParameter> configurationParameters = this.GetConfigurationParameters(0);
      int numberOfIds = 1;
      int nextId = DbBasis.PrimaryDB.BaseDbConnection.GetNewIds("SerialNumber_PDC_LoRa", numberOfIds).GetNextID();
      configurationParameters[OverrideID.SerialNumberFull].ParameterValue = (object) Utility.ConvertSerialNumberToFullserialNumber(Utility.LoraDevice.PDC, nextId);
      configurationParameters[OverrideID.DevEUI].ParameterValue = (object) Utility.ConvertSerialNumberToEUI64(Utility.LoraDevice.PDC, nextId);
      this.SetConfigurationParameters(configurationParameters, 0);
    }

    public override bool SetConfigurationParameterFromBackup()
    {
      SortedList<OverrideID, ConfigurationParameter> configurationParameters = this.GetConfigurationParameters(0);
      new ConfigurationParameter(OverrideID.MeterID).ParameterValue = (object) this.myMeters.BackupMeter.deviceIdentification.MeterID;
      this.SetConfigurationParameters(configurationParameters, 0);
      return true;
    }

    public override DeviceIdentification GetDeviceIdentification(int channel)
    {
      switch (channel)
      {
        case 0:
          return (DeviceIdentification) this.myMeters.checkedWorkMeter.deviceIdentification;
        case 1:
          return (DeviceIdentification) this.myMeters.checkedWorkMeter.deviceIdentification1;
        case 2:
          return (DeviceIdentification) this.myMeters.checkedWorkMeter.deviceIdentification2;
        default:
          throw new Exception("Not supported channel: " + channel.ToString());
      }
    }

    public override async Task WriteDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.myMeters.WriteDeviceAsync(progress, token);
    }

    public override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      int subDevice = 0)
    {
      return this.myMeters.GetConfigurationParameters(subDevice);
    }

    public override void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameter,
      int subDevice = 0)
    {
      this.myMeters.SetConfigurationParameters(parameter, subDevice);
    }

    public void SetHardwareErrorsA(byte value)
    {
      this.myMeters.SetParameter(PDCL2_Params.hwStatusFlagsA, (object) value);
    }

    public void SetHardwareErrorsB(byte value)
    {
      this.myMeters.SetParameter(PDCL2_Params.hwStatusFlagsB, (object) value);
    }

    public void SetWarningsA(byte value)
    {
      this.myMeters.SetParameter(PDCL2_Params.persistent_warning_flagsA, (object) value);
    }

    public void SetWarningsB(byte value)
    {
      this.myMeters.SetParameter(PDCL2_Params.persistent_warning_flagsB, (object) value);
    }

    public bool GetSwitchStatus()
    {
      return BitConverter.ToBoolean(new byte[1]
      {
        (byte) this.myMeters.GetParameter(PDCL2_Params.tamper_detect_level)
      }, 0);
    }

    public void SetRadioTransmitSyncWord(byte radio_transmit_sync1, byte radio_transmit_sync2)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(PDCL2_Params.radio_transmit_sync1, radio_transmit_sync1);
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(PDCL2_Params.radio_transmit_sync2, radio_transmit_sync2);
    }

    public void SetRadioResceiveSyncWord(byte radio_resceive_sync1, byte radio_resceive_sync2)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(PDCL2_Params.radio_resceive_sync1, radio_resceive_sync1);
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(PDCL2_Params.radio_resceive_sync2, radio_resceive_sync2);
    }

    public bool AccessRadioKey_IsOK()
    {
      return (uint) this.myMeters.GetParameter(PDCL2_Params.cfg_AccessRadioKey) == 386474500U;
    }

    public async Task WriteAccessRadioKeyAsync(
      ProgressHandler progress,
      CancellationToken token,
      uint key = 0)
    {
      byte[] buffer = BitConverter.GetBytes(key);
      uint address = this.myMeters.WorkMeter.meterMemory.GetParameterAddress(PDCL2_Params.cfg_AccessRadioKey.ToString());
      await this.cmd.Device.WriteMemoryAsync(progress, token, address, buffer);
      buffer = (byte[]) null;
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

    public async Task<bool> ReceiveTestPacket_UDC(
      byte timeoutInSec,
      ProgressHandler progress,
      CancellationToken token)
    {
      DateTime start = DateTime.Now;
      DateTime end = start.AddSeconds((double) timeoutInSec);
      while (DateTime.Now <= end)
      {
        byte[] buffer;
        try
        {
          buffer = await this.cmd.Radio.ReceiveRadio3Scenario3TelegramViaRadioAsync((byte) 4, Util.HexStringToByteArray("91D3"), (byte) 2, progress, token);
        }
        catch
        {
          continue;
        }
        if (buffer != null && buffer.Length > 2)
        {
          string data = string.Empty;
          for (int i = 2; i < buffer.Length; ++i)
            data += buffer[i].ToString("X2");
          if (!(data != "504F4E47"))
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

    public async Task ClearResetCounterAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.ClearResetCounterAsync(progress, token);
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

    public new async Task<byte> GetModeAsync(ProgressHandler progress, CancellationToken token)
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

    public async Task<uint> GetCenterFrequency(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      uint centerFrequencyAsync = await this.cmd.Radio.GetCenterFrequencyAsync(progress, token);
      return centerFrequencyAsync;
    }

    public async Task SetCenterFrequency(
      uint value,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Radio.SetCenterFrequencyAsync(value, progress, token);
    }

    public async Task<OTAA_ABP> GetActivitionMode(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      OTAA_ABP otaaAbpAsync = await this.cmd.LoRa.GetOTAA_ABPAsync(progress, token);
      return otaaAbpAsync;
    }

    public async Task SetActivitionMode(
      OTAA_ABP value,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.LoRa.SetOTAA_ABPAsync(value, progress, token);
    }

    public async Task<Common32BitCommands.KeyDate> GetKeyDate(
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      Common32BitCommands.KeyDate keydate = await this.cmd.Device.GetKeyDateAsync(progress, token);
      Common32BitCommands.KeyDate keyDate = keydate;
      keydate = (Common32BitCommands.KeyDate) null;
      return keyDate;
    }

    public async Task SetKeyDate(
      Common32BitCommands.KeyDate value,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.SetKeyDateAsync(value, progress, token);
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
      if (this.myMeters.TypeMeter == null)
        throw new Exception("TypeMeter not available for overwrite");
      if (this.myMeters.WorkMeter == null)
        throw new Exception("WorkMeter not available for overwrite");
      foreach (CommonOverwriteGroups overwriteGroup in overwriteGroups)
        this.myMeters.WorkMeter.meterMemory.OverwriteUsedParameters((DeviceMemory) this.myMeters.TypeMeter.meterMemory, PDCL2_DeviceMemory.ParameterGroups[overwriteGroup]);
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
        if (PDCL2_DeviceMemory.ParameterGroups.ContainsKey(overwriteGroup))
        {
          foreach (string parameterName in PDCL2_DeviceMemory.ParameterGroups[overwriteGroup])
          {
            if (allMeterMemories.Values[index2].IsParameterAvailable(parameterName) && allMeterMemories.Values[index1].IsParameterAvailable(parameterName))
              stringList.Add(parameterName);
          }
        }
        allMeterMemories.Values[index2].OverwriteUsedParameters(allMeterMemories.Values[index1], stringList.ToArray());
      }
    }

    public override CommonOverwriteGroups[] GetAllOverwriteGroups()
    {
      return HandlerFunctionsForProduction.GetImplementedOverwriteGroups(Enum.GetNames(typeof (PDCL2_HandlerFunctions.OverwriteGroups)));
    }

    public override void SaveMeterObject(HandlerMeterObjects meterObject)
    {
      this.myMeters.SaveMeterObject(meterObject);
    }

    public async Task WriteMeterValueA(
      int value,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.MBusCmd.SetChannelValueAsync((byte) 1, (uint) value, progress, token);
    }

    public async Task WriteMeterValueB(
      int value,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.MBusCmd.SetChannelValueAsync((byte) 2, (uint) value, progress, token);
    }

    public async Task<uint> ReadMeterValueA(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      uint channelValueAsync = await this.cmd.MBusCmd.GetChannelValueAsync((byte) 1, progress, token);
      return channelValueAsync;
    }

    public async Task<uint> ReadMeterValueB(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      uint channelValueAsync = await this.cmd.MBusCmd.GetChannelValueAsync((byte) 2, progress, token);
      return channelValueAsync;
    }

    public async Task StopRadioTests(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Radio.StopRadioTests(progress, token);
    }

    public async Task SetTransmissionScenario(
      byte[] scenario,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.LoRa.SetTransmissionScenarioAsync(scenario, progress, token);
    }

    public async Task<byte> GetTransmissionScenario(
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte transmissionScenarioAsync = await this.cmd.LoRa.GetTransmissionScenarioAsync(progress, token);
      return transmissionScenarioAsync;
    }

    public void Set_pulseReadingErrorTimeChA(uint value)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<uint>(PDCL2_Params.pulse_reading_error_time_A, value);
    }

    public void Set_pulseReadingErrorTimeChB(uint value)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<uint>(PDCL2_Params.pulse_reading_error_time_B, value);
    }

    public async Task StartVolumeMonitorAsync(CancellationToken token)
    {
      this.cmd.StartVolumeMonitor();
      await Task.Run((Action) (() =>
      {
        Queue<byte> byteQueue = new Queue<byte>();
        try
        {
          while (!token.IsCancellationRequested)
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
              byteQueue.Enqueue(buffer[index]);
            while (byteQueue.Count > 0 && byteQueue.Peek() != (byte) 2)
              Debug.WriteLine(byteQueue.Dequeue().ToString("X2"));
            if (byteQueue.Count >= 42)
            {
              byte[] array = byteQueue.ToArray();
              if (array[0] != (byte) 2 && array[array.Length - 1] != (byte) 3)
              {
                int num = (int) byteQueue.Dequeue();
              }
              byteQueue.Clear();
              if (this.OnEncabulatorDataReceived != null)
                this.OnEncabulatorDataReceived((object) this, new EncabulatorData(array));
            }
          }
        }
        finally
        {
          this.cmd.StopVolumeMonitor();
        }
      }));
    }

    public enum OverwriteGroups
    {
      IdentData = 0,
      BasicConfiguration = 8,
    }
  }
}
