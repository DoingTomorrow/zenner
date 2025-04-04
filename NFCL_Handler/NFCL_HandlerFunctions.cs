// Decompiled with JetBrains decompiler
// Type: NFCL_Handler.NFCL_HandlerFunctions
// Assembly: NFCL_Handler, Version=2.3.2.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 887E21A2-7448-48CC-AF3E-C39E4C7B3AFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NFCL_Handler.dll

using CommunicationPort.Functions;
using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using HandlerLib.NFC;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace NFCL_Handler
{
  public sealed class NFCL_HandlerFunctions : HandlerFunctionsForProduction
  {
    public const string HandlerName = "NFCL_Handler";
    private CommunicationPortFunctions port;
    public BaseDbConnection db;
    internal NFCL_DeviceCommands cmd;
    internal NFCL_AllMeters myMeters;

    public NFCL_HandlerFunctions(CommunicationPortFunctions myPort, BaseDbConnection myDb)
      : this(myPort)
    {
      this.db = myDb;
    }

    public NFCL_HandlerFunctions(CommunicationPortFunctions myPort)
      : this()
    {
      this.port = myPort;
      this.cmd = new NFCL_DeviceCommands(this.port);
      this.myMeters = new NFCL_AllMeters(this);
    }

    public NFCL_HandlerFunctions(BaseDbConnection myDb)
      : this()
    {
      this.db = myDb;
      this.myMeters = new NFCL_AllMeters(this);
    }

    public NFCL_HandlerFunctions() => this.myMeters = new NFCL_AllMeters(this);

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
      this.myMeters = (NFCL_AllMeters) null;
    }

    public override void Clear() => this.myMeters = new NFCL_AllMeters(this);

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
      string productionOrderNumber = deviceIdentification.SAP_ProductionOrderNumber;
      byte[] compressedData = this.myMeters.WorkMeter.meterMemory.GetCompressedData();
      DateTime? nullable = Device.Save(baseDbConnection, meterID, meterInfoID, hardwareTypeID_OR_firmwareVersion, serialNumberAsString, productionOrderNumber, compressedData, false);
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
      return this.myMeters.iuwData != null ? 2 : 1;
    }

    public override DeviceIdentification GetDeviceIdentification()
    {
      DeviceIdentification deviceIdentification = this.GetDeviceIdentification(0);
      if (this.myMeters.iuwData != null)
        deviceIdentification.SubChannels = new int[1]{ 1 };
      return deviceIdentification;
    }

    public override DeviceIdentification GetDeviceIdentification(int channel)
    {
      switch (channel)
      {
        case 0:
          return (DeviceIdentification) this.myMeters.WorkMeter.deviceIdentification;
        case 1:
          return new DeviceIdentification(0U);
        default:
          throw new Exception("Not supported channel: " + channel.ToString());
      }
    }

    public override void OpenType(int meterInfoID)
    {
      BaseType baseType = BaseType.GetBaseType(meterInfoID);
      if (baseType == null || baseType.Data == null)
        return;
      this.myMeters.TypeMeter = (NFCL_Meter) this.myMeters.CreateMeter(baseType.Data.EEPdata);
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
      return HandlerFunctionsForProduction.GetImplementedOverwriteGroups(Enum.GetNames(typeof (NFCL_HandlerFunctions.OverwriteGroups)));
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
        foreach (string parameterName in NFCL_DeviceMemory.ParameterGroups[overwriteGroup])
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

    public void SetFD_Mbus_interval(ushort value)
    {
      this.myMeters.SetParameter(NFCL_Params.FD_Mbus_interval, (object) value);
    }

    public void SetMbus_interval(ushort value)
    {
      this.myMeters.SetParameter(NFCL_Params.Mbus_interval, (object) value);
    }

    public void SetFD_Mbus_nighttime_start(byte value)
    {
      this.myMeters.SetParameter(NFCL_Params.FD_Mbus_nighttime_start, (object) value);
    }

    public void SetFD_Mbus_nighttime_stop(byte value)
    {
      this.myMeters.SetParameter(NFCL_Params.FD_Mbus_nighttime_stop, (object) value);
    }

    public void SetFD_Mbus_radio_suppression_days(byte value)
    {
      this.myMeters.SetParameter(NFCL_Params.FD_Mbus_radio_suppression_days, (object) value);
    }

    public ushort GetFD_Mbus_interval()
    {
      if (!this.myMeters.WorkMeter.meterMemory.IsParameterAvailable(NFCL_Params.FD_Mbus_interval))
        throw new Exception("Parameter 'FD_Mbus_interval' is not available in the firmware.");
      return Convert.ToUInt16(this.myMeters.GetParameter(NFCL_Params.FD_Mbus_interval));
    }

    public ushort GetMbus_interval()
    {
      if (!this.myMeters.WorkMeter.meterMemory.IsParameterAvailable(NFCL_Params.Mbus_interval))
        throw new Exception("Parameter 'Mbus_interval' is not available in the firmware.");
      return Convert.ToUInt16(this.myMeters.GetParameter(NFCL_Params.Mbus_interval));
    }

    public byte GetMbus_nighttime_start()
    {
      return this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.Mbus_nighttime_start);
    }

    public byte GetMbus_nighttime_stop()
    {
      return this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.Mbus_nighttime_stop);
    }

    public byte GetMbus_radio_suppression_days()
    {
      return this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.Mbus_radio_suppression_days);
    }

    public void SetRadioTransmitSyncWord(byte radio_transmit_sync1, byte radio_transmit_sync2)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(NFCL_Params.radio_transmit_sync1, radio_transmit_sync1);
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(NFCL_Params.radio_transmit_sync2, radio_transmit_sync2);
    }

    public void SetRadioResceiveSyncWord(byte radio_resceive_sync1, byte radio_resceive_sync2)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(NFCL_Params.radio_resceive_sync1, radio_resceive_sync1);
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(NFCL_Params.radio_resceive_sync2, radio_resceive_sync2);
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
      if (subDevice != 0 && subDevice != 1)
        throw new NotSupportedException();
      this.myMeters.SetConfigurationParameters(parameter, subDevice);
    }

    public override SortedList<long, SortedList<DateTime, double>> GetValues(int subDevice = 0)
    {
      return this.myMeters.GetValues();
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

    public async Task<double> ReadTemperatureAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      uint val = await this.cmd.MBusCmd.GetChannelValueAsync((byte) 1, progress, token);
      return (double) ((int) Convert.ToInt16(val) / 10);
    }

    public async Task<double> ReadHumidityAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      uint val = await this.cmd.MBusCmd.GetChannelValueAsync((byte) 2, progress, token);
      return (double) ((int) Convert.ToInt16(val) / 10);
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
        case Mode.DeliveryMode8:
          await this.cmd.Device.SetModeAsync((byte) 8, progress, token);
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
      return (uint) this.myMeters.GetParameter(NFCL_Params.cfg_AccessRadioKey) == 386474500U;
    }

    public async Task<NfcDeviceIdentification> GetIdentificationIUW(
      ProgressHandler progress,
      CancellationToken token)
    {
      NfcFrame nfcFrame = new NfcFrame(NfcCommands.GetIdentification, "GetIdentification");
      byte[] getIdentification = await this.cmd.Special.SendToNfcDeviceAsync(nfcFrame.NfcRequestFrame, progress, token);
      NfcDeviceIdentification identificationIuw = new NfcDeviceIdentification(getIdentification);
      nfcFrame = (NfcFrame) null;
      getIdentification = (byte[]) null;
      return identificationIuw;
    }

    public async Task DisableIrDaAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.ExecuteEventAsync((byte) 0, progress, token);
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

    public async Task<byte> GetResetCounterAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      byte resetCounterAsync = await this.cmd.Device.GetResetCounterAsync(progress, token);
      return resetCounterAsync;
    }

    public async Task BackupDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.BackupDeviceAsync(progress, token);
    }

    public async Task SwitchLoRaLEDAsync(
      byte state,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Device.SwitchLoRaLEDAsync(state, progress, token);
    }

    public async Task WriteAccessRadioKeyAsync(
      ProgressHandler progress,
      CancellationToken token,
      uint key = 0)
    {
      byte[] buffer = BitConverter.GetBytes(key);
      uint address = this.myMeters.WorkMeter.meterMemory.GetParameterAddress(NFCL_Params.cfg_AccessRadioKey.ToString());
      await this.cmd.Device.WriteMemoryAsync(progress, token, address, buffer);
      buffer = (byte[]) null;
    }

    public async Task<byte> GetMBusStatusByteAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      byte mbusStatusByte = await this.cmd.Device.GetMBusStatusByte(progress, token);
      return mbusStatusByte;
    }

    public byte[] GetAesKey(ProgressHandler progress, CancellationToken token)
    {
      return this.myMeters.WorkMeter.meterMemory.GetData(NFCL_Params.Mbus_aes_key);
    }

    public async Task<bool> SetNFCFieldAsync(
      byte function,
      ushort timeout,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      await this.cmd.Radio.SetNFCFieldAsync(function, timeout, progress, token);
      return true;
    }

    public async Task SetAdaptiveDataRate(
      byte data,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Prepare(progress, token);
      this.myMeters.WorkMeter.meterMemory.SetData("lora_adr", new byte[1]
      {
        data
      });
    }

    public enum OverwriteGroups
    {
      BasicConfiguration = 8,
    }
  }
}
