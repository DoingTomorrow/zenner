// Decompiled with JetBrains decompiler
// Type: THL_Handler.THL_HandlerFunctions
// Assembly: THL_Handler, Version=1.0.5.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: C9669406-A704-45DE-B726-D8A41F27FFB8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\THL_Handler.dll

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
namespace THL_Handler
{
  public sealed class THL_HandlerFunctions : HandlerFunctionsForProduction
  {
    public const string HandlerName = "THL_Handler";
    private CommunicationPortFunctions port;
    private BaseDbConnection db;
    internal THL_DeviceCommands cmd;
    internal THL_AllMeters myMeters;

    public THL_HandlerFunctions(CommunicationPortFunctions myPort, BaseDbConnection myDb)
      : this(myPort)
    {
      this.db = myDb;
    }

    public THL_HandlerFunctions(CommunicationPortFunctions myPort)
      : this()
    {
      this.port = myPort;
      this.cmd = new THL_DeviceCommands(this.port);
      this.myMeters = new THL_AllMeters(this);
    }

    public THL_HandlerFunctions(BaseDbConnection myDb)
      : this()
    {
      this.db = myDb;
      this.myMeters = new THL_AllMeters(this);
    }

    public THL_HandlerFunctions() => this.myMeters = new THL_AllMeters(this);

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
      this.myMeters = (THL_AllMeters) null;
    }

    public override void Clear() => this.myMeters = new THL_AllMeters(this);

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
      return 1;
    }

    public void SetCommunicationScenarioParameter(int SAP_MaterialNumber)
    {
      this.myMeters.SetCommunicationScenarioParameter(SAP_MaterialNumber);
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
      this.myMeters.TypeMeter = (THL_Meter) this.myMeters.CreateMeter(baseType.Data.EEPdata);
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
      return HandlerFunctionsForProduction.GetImplementedOverwriteGroups(Enum.GetNames(typeof (THL_HandlerFunctions.OverwriteGroups)));
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
        foreach (string parameterName in THL_DeviceMemory.ParameterGroups[overwriteGroup])
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

    public void SetMbus_interval(ushort value)
    {
      this.myMeters.SetParameter(THL_Params.Mbus_interval, (object) value);
    }

    public void SetFD_Mbus_interval(ushort value)
    {
      this.myMeters.SetParameter(THL_Params.FD_Mbus_interval, (object) value);
    }

    public void SetFD_Mbus_nighttime_start(byte value)
    {
      this.myMeters.SetParameter(THL_Params.FD_Mbus_nighttime_start, (object) value);
    }

    public void SetFD_Mbus_nighttime_stop(byte value)
    {
      this.myMeters.SetParameter(THL_Params.FD_Mbus_nighttime_stop, (object) value);
    }

    public void SetFD_Mbus_radio_suppression_days(byte value)
    {
      this.myMeters.SetParameter(THL_Params.FD_Mbus_radio_suppression_days, (object) value);
    }

    public ushort GetFD_Mbus_interval()
    {
      if (!this.myMeters.WorkMeter.meterMemory.IsParameterAvailable(THL_Params.FD_Mbus_interval))
        throw new Exception("Parameter 'FD_Mbus_interval' is not available in the firmware.");
      return Convert.ToUInt16(this.myMeters.GetParameter(THL_Params.FD_Mbus_interval));
    }

    public ushort GetMbus_interval()
    {
      if (!this.myMeters.WorkMeter.meterMemory.IsParameterAvailable(THL_Params.Mbus_interval))
        throw new Exception("Parameter 'Mbus_interval' is not available in the firmware.");
      return Convert.ToUInt16(this.myMeters.GetParameter(THL_Params.Mbus_interval));
    }

    public byte GetMbus_nighttime_start()
    {
      return this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.Mbus_nighttime_start);
    }

    public byte GetMbus_nighttime_stop()
    {
      return this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.Mbus_nighttime_stop);
    }

    public byte GetMbus_radio_suppression_days()
    {
      return this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.Mbus_radio_suppression_days);
    }

    public SwitchState Get_mount_switch_state()
    {
      if (!this.myMeters.WorkMeter.meterMemory.IsParameterAvailable(THL_Params.mount_switch_state))
        throw new Exception("Parameter 'mount_switch_state' is not available in the firmware.");
      return (SwitchState) this.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.mount_switch_state);
    }

    public void SetRadioTransmitSyncWord(byte radio_transmit_sync1, byte radio_transmit_sync2)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(THL_Params.radio_transmit_sync1, radio_transmit_sync1);
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(THL_Params.radio_transmit_sync2, radio_transmit_sync2);
    }

    public void SetRadioResceiveSyncWord(byte radio_resceive_sync1, byte radio_resceive_sync2)
    {
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(THL_Params.radio_resceive_sync1, radio_resceive_sync1);
      this.myMeters.WorkMeter.meterMemory.SetParameterValue<byte>(THL_Params.radio_resceive_sync2, radio_resceive_sync2);
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

    public async Task<byte> GetRadioMode(ProgressHandler progress, CancellationToken cancelToken)
    {
      byte carrierModeAsync = await this.cmd.Radio.GetCarrierModeAsync(progress, cancelToken);
      return carrierModeAsync;
    }

    public async Task SetRadioMode(
      byte mode,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.cmd.Radio.SetCarrierModeAsync(mode, progress, cancelToken);
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
      return (uint) this.myMeters.GetParameter(THL_Params.cfg_AccessRadioKey) == 386474500U;
    }

    public enum OverwriteGroups
    {
      BasicConfiguration = 8,
    }
  }
}
