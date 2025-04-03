// Decompiled with JetBrains decompiler
// Type: HandlerLib.HandlerFunctionsForProduction
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommunicationPort.Functions;
using GmmDbLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class HandlerFunctionsForProduction : IHandler, IReadoutConfig
  {
    public virtual bool MapCheckDisabled { get; set; }

    public DeviceMemory DeviceMemory { get; set; }

    public virtual void SetCommunicationPort(CommunicationPortFunctions myPort = null)
    {
      throw new NotImplementedException(nameof (SetCommunicationPort));
    }

    public virtual void Open() => throw new NotImplementedException(nameof (Open));

    public virtual void Close() => throw new NotImplementedException(nameof (Close));

    public virtual void SetReadoutConfiguration(ConfigList configList)
    {
      throw new NotImplementedException(nameof (SetReadoutConfiguration));
    }

    public virtual ConfigList GetReadoutConfiguration()
    {
      throw new NotImplementedException(nameof (GetReadoutConfiguration));
    }

    public virtual Task<int> ReadDeviceAsync(
      ProgressHandler progress,
      CancellationToken token,
      ReadPartsSelection readPartsSelection)
    {
      throw new NotImplementedException(nameof (ReadDeviceAsync));
    }

    public virtual Task WriteDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      throw new NotImplementedException(nameof (WriteDeviceAsync));
    }

    public virtual Task WriteDeviceOnlyAsync(ProgressHandler progress, CancellationToken token)
    {
      throw new NotImplementedException(nameof (WriteDeviceOnlyAsync));
    }

    public virtual DeviceIdentification GetDeviceIdentification()
    {
      return this.GetDeviceIdentification(0);
    }

    public virtual DeviceIdentification GetDeviceIdentification(int channel)
    {
      throw new NotImplementedException(nameof (GetDeviceIdentification));
    }

    public virtual SortedList<long, SortedList<DateTime, double>> GetValues(int subDevice = 0)
    {
      throw new NotImplementedException(nameof (GetValues));
    }

    public virtual SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      int subDevice = 0)
    {
      throw new NotImplementedException(nameof (GetConfigurationParameters));
    }

    public virtual void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameter,
      int subDevice = 0)
    {
      throw new NotImplementedException(nameof (SetConfigurationParameters));
    }

    public virtual void SetDataToWorkMeter(uint Address, byte[] data)
    {
      throw new NotImplementedException(nameof (SetDataToWorkMeter));
    }

    public virtual void WriteConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameter,
      int subDevice = 0)
    {
      throw new NotImplementedException(nameof (WriteConfigurationParameters));
    }

    public virtual BatteryEnergyManagement GetBatteryCalculations()
    {
      throw new NotImplementedException(nameof (GetBatteryCalculations));
    }

    public virtual async Task<NfcDeviceIdentification> GetMiConDeviceIdentification(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      throw new NotImplementedException(nameof (GetMiConDeviceIdentification));
    }

    public virtual async Task ProtectionResetByDb(
      ProgressHandler progress,
      CancellationToken cancelToken,
      BaseDbConnection dbCon)
    {
      throw new NotImplementedException(nameof (ProtectionResetByDb));
    }

    public virtual bool IsProtected() => throw new NotImplementedException(nameof (IsProtected));

    public virtual async Task<DeviceHistoryData> GetDeviceHistory(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      throw new NotImplementedException(nameof (GetDeviceHistory));
    }

    public virtual bool LoadLastBackup(int meterID)
    {
      throw new NotImplementedException(nameof (LoadLastBackup));
    }

    public virtual bool LoadBackup(int meterID, DateTime TimePoint)
    {
      throw new NotImplementedException(nameof (LoadBackup));
    }

    public virtual bool SetBackup(byte[] zippedBuffer)
    {
      throw new NotImplementedException("SetBackupToBackUPMeter");
    }

    public virtual bool SetConfigurationParameterFromBackup()
    {
      throw new NotImplementedException(nameof (SetConfigurationParameterFromBackup));
    }

    public virtual DateTime? SaveMeter() => throw new NotImplementedException(nameof (SaveMeter));

    public virtual void OpenType(int meterInfoID)
    {
      throw new NotImplementedException(nameof (OpenType));
    }

    public virtual void OpenType(string typeCreationString)
    {
      throw new NotImplementedException("OpenType by type TypeCreationString");
    }

    public virtual void OpenCompareType(int meterInfoID)
    {
      throw new NotImplementedException(nameof (OpenCompareType));
    }

    public virtual int SaveType(
      OpenTransaction openTransaction,
      string sapNumber,
      string description)
    {
      throw new NotImplementedException(nameof (SaveType));
    }

    public virtual int SaveType(
      OpenTransaction openTransaction,
      string sapNumber,
      string description,
      int meterhardwareid)
    {
      throw new NotImplementedException(nameof (SaveType));
    }

    public virtual void SaveTypeData(int meterInfoId)
    {
      throw new NotImplementedException(nameof (SaveTypeData));
    }

    public virtual void OverwriteFromType(CommonOverwriteGroups[] overwriteGroups)
    {
      throw new NotImplementedException(nameof (OverwriteFromType));
    }

    public virtual void OverwriteSrcToDest(
      HandlerMeterObjects sourceObject,
      HandlerMeterObjects destinationObject,
      CommonOverwriteGroups[] overwriteGroups)
    {
      throw new NotImplementedException("OverwriteFromType");
    }

    public virtual CommonOverwriteGroups[] GetAllOverwriteGroups() => new CommonOverwriteGroups[0];

    public virtual string GetOverwriteGroupInfo(CommonOverwriteGroups overwriteGroupe)
    {
      return "No overwrite group info prepared from handler.";
    }

    protected static CommonOverwriteGroups[] GetImplementedOverwriteGroups(string[] allNames)
    {
      if (allNames == null || allNames.Length == 0)
        return new CommonOverwriteGroups[0];
      CommonOverwriteGroups[] implementedOverwriteGroups = new CommonOverwriteGroups[allNames.Length];
      for (int index = 0; index < allNames.Length; ++index)
        implementedOverwriteGroups[index] = (CommonOverwriteGroups) Enum.Parse(typeof (CommonOverwriteGroups), allNames[index]);
      return implementedOverwriteGroups;
    }

    public virtual void SaveMeterObject(HandlerMeterObjects meterObject)
    {
      throw new NotImplementedException(nameof (SaveMeterObject));
    }

    public virtual async Task SetModeAsync(
      ProgressHandler progress,
      CancellationToken token,
      Enum mode)
    {
      await Task.Delay(0);
      throw new NotImplementedException(nameof (SetModeAsync));
    }

    public virtual async Task<byte> GetModeAsync(ProgressHandler progress, CancellationToken token)
    {
      await Task.Delay(0);
      throw new NotImplementedException(nameof (GetModeAsync));
    }

    public virtual async Task SetSystemTimeAsync(
      ProgressHandler progress,
      CancellationToken token,
      Common32BitCommands.SystemTime sysTime)
    {
      await Task.Delay(0);
      throw new NotImplementedException("SetModeAsync");
    }

    public virtual async Task ResetDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await Task.Delay(0);
      throw new NotImplementedException(nameof (ResetDeviceAsync));
    }

    public virtual async Task ResetAccumulatedData(
      ProgressHandler progress,
      CancellationToken token)
    {
      await Task.Delay(0);
    }

    public virtual async Task ResetDiagnosticData(ProgressHandler progress, CancellationToken token)
    {
      await Task.Delay(0);
    }

    public virtual void Clear() => throw new NotImplementedException(nameof (Clear));

    public virtual async Task Calibrate_RTC(
      ProgressHandler progress,
      CancellationToken token,
      double calibrationValue)
    {
      throw new NotImplementedException(nameof (Calibrate_RTC));
    }

    public virtual async Task SetLcdTestStateAsync(
      ProgressHandler progress,
      CancellationToken token,
      byte lcdTestState)
    {
      await Task.Delay(0);
      throw new NotImplementedException(nameof (SetLcdTestStateAsync));
    }

    public virtual async Task ReInitMeasurementAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await Task.Delay(0);
      throw new NotImplementedException("ReInitMeasurement");
    }

    public virtual void AddEventHandler(EventHandler<GMM_EventArgs> TheEventHandlerArgs)
    {
      throw new NotImplementedException(nameof (AddEventHandler));
    }

    public virtual void RemoveEventHandler(EventHandler<GMM_EventArgs> TheEventHandlerArgs)
    {
      throw new NotImplementedException("AddEventHandler");
    }

    public virtual void SetCompatibleCommunicationStructure()
    {
      throw new NotImplementedException(nameof (SetCompatibleCommunicationStructure));
    }

    public virtual IEnumerable GetEvents() => throw new NotImplementedException(nameof (GetEvents));

    public enum CommonDeviceModes
    {
      OperationMode = 0,
      DeliveryMode = 1,
      StandbyCurrentMode = 2,
      TemperatureCalibrationMode = 3,
      RTC_CalibrationMode = 4,
      RTC_CalibrationVerifyMode = 5,
      DeliveryMode8 = 8,
      UltrasonicLevelTest = 9,
      LcdTest = 10, // 0x0000000A
      RadioTestTransmitUnmodulatedCarrier = 11, // 0x0000000B
      RadioTestTransmitModulatedCarrier = 12, // 0x0000000C
      RadioTestReceiveTestPacket = 13, // 0x0000000D
      RadioTestSendTestPacket = 14, // 0x0000000E
    }
  }
}
