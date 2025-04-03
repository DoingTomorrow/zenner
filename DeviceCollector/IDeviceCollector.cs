// Decompiled with JetBrains decompiler
// Type: DeviceCollector.IDeviceCollector
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using AsyncCom;
using System;
using System.Collections;
using System.Collections.Generic;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public interface IDeviceCollector : I_ZR_Component, ICancelable, IReadoutConfig
  {
    IAsyncFunctions AsyncCom { get; }

    string ShowBusWindow(string ComponentList);

    void ShowBusWindow();

    bool SetBaseMode(ZR_ClassLibrary.BusMode NewBusMode);

    ZR_ClassLibrary.BusMode GetBaseMode();

    SortedList<string, string> GetAsyncComSettings();

    void SetAsyncComSettings(SortedList<string, string> settings);

    bool SetDeviceCollectorSettings(
      SortedList<DeviceCollectorSettings, object> settings);

    bool SetDeviceCollectorSettings(SortedList<string, string> settings);

    bool SetDeviceCollectorSettings(string settings);

    SortedList<DeviceCollectorSettings, object> GetDeviceCollectorSettings();

    string GetDeviceCollectorSettingsAsString();

    bool ChangeDeviceCollectorSettings(
      SortedList<DeviceCollectorSettings, object> settings);

    bool GetDeviceCollectorInfo(out object InfoObject);

    bool WriteLastUsedBusInfo(string BusInfoName);

    bool ReadBusInfo(string BusInfoName);

    bool ReadBusInfo(string BusInfoName, bool SetNameToDefault);

    bool WriteBusInfo();

    bool WriteBusInfo(string BusInfoName);

    bool WriteBusInfo(string BusInfoName, bool SetNameToDefault);

    bool StartReceiver();

    bool StopReceiver();

    bool ComOpen();

    bool ComClose();

    bool SetBreak();

    bool ClearBreak();

    void BreakAllFunctions();

    event EventHandler<GMM_EventArgs> OnMessage;

    void SetMessageInfo(string info);

    int SetMaxRepeat(int MaxRepeat);

    bool ReadParameter(out DeviceInfo Info);

    bool ReadParameter(out string ZDF_Data);

    bool GetParameter(out DeviceInfo Info);

    bool GetParameter(out DeviceInfo Info, int DeviceListIndex);

    List<DeviceInfo> GetParameters();

    int GetNumberOfDevices();

    bool GetDeviceConfiguration(
      out SortedList<OverrideID, ConfigurationParameter> ConfigParamList);

    void ClearCounters();

    int GetJobCounter();

    int GetErrorCounter();

    int GetTransmitBlockCounter();

    int GetReceiveBlockCounter();

    void DeleteBusInfo();

    bool IsSelectedDevice(DeviceTypes TestType);

    BusDevice GetSelectedDevice();

    int GetIndexOfSelectedDevice();

    bool AddDevice(DeviceTypes NewType, int PrimaryAddress);

    bool AddDevice(DeviceTypes NewType, int PrimaryAddress, long SerialNumber);

    bool AddDevice(DeviceTypes NewType, int PrimaryAddress, string SerialNumber);

    bool ScanFromAddress(int ScanAddress);

    bool ScanFromSerialNumber(string StartSerialnumber);

    bool SearchSingleDeviceByPrimaryAddress(int Address);

    bool SearchSingleDeviceBySerialNumber(string SerialNumber);

    bool SetSelectedDeviceBySerialNumber(string SerialNumber);

    bool SetSelectedDeviceByPrimaryAddress(int Address);

    bool SetSelectedDeviceByIndex(int index);

    bool SetPrimaryAddress(int Address);

    bool SetBaudrate(int Baudrate);

    bool SetBaudrate(int Baudrate, bool ChangeAsyncComBaudrate);

    bool UseMaxBaudrate();

    bool SerchDeviceAcrossBaudrates();

    bool ResetDevice();

    bool ResetDevice(bool loadBackup);

    bool ResetDevice(int AfterResetBaudrate);

    bool RunBackup();

    bool DeviceProtectionGet();

    bool DeviceProtectionSet();

    bool DeviceProtectionReset(uint meterKey);

    bool DeviceProtectionSetKey(uint meterKey);

    bool SetEmergencyMode();

    bool DisableBusWriteOnDispose { get; set; }

    bool ReadVersion(out ReadVersionData versionData);

    bool ReadVersion(
      out short Manufacturer,
      out byte Medium,
      out byte MBusMeterType,
      out long Version,
      out int MBusSerialNr);

    bool ReadVersion(
      out short Manufacturer,
      out byte Medium,
      out byte MBusMeterType,
      out long Version,
      out int MBusSerialNr,
      out int ConfigAdr,
      out int HardwareMask);

    bool IsDeviceModified();

    bool ReadVersion(
      int[] Bautrates,
      out short Manufacturer,
      out byte Medium,
      out byte MBusMeterType,
      out long Version,
      out int MBusSerialNr);

    bool ReadVersion(
      int[] Bautrates,
      out short Manufacturer,
      out byte Medium,
      out byte MBusMeterType,
      out long Version,
      out int MBusSerialNr,
      out int ConfigAdr,
      out int HardwareMask);

    bool ReadMemory(
      MemoryLocation Location,
      int StartAddress,
      int NumberOfBytes,
      out ByteField MemoryData);

    bool ReadMemory(
      MemoryLocation Location,
      int StartAddress,
      int NumberOfBytes,
      out ByteField MemoryData,
      bool useOnlyLongWakeUpSequence);

    bool ReadMemory(int Location, int StartAddress, int NumberOfBytes, out ByteField MemoryData);

    bool ReadMemory(long FullStartAddress, int NumberOfBytes, out ByteField MemoryData);

    bool WriteMemory(MemoryLocation Location, int StartAddress, ByteField data);

    bool WriteMemory(int Location, int StartAddress, ByteField data);

    bool UpdateMemory(
      MemoryLocation Location,
      int StartAddress,
      ByteField OldData,
      ByteField NewData);

    bool WriteBitfield(int Address, byte AndMask, byte OrMask);

    bool WriteBitfield(int Address, uint AndMask, uint OrMask);

    bool WriteBit(long FullAddress, bool BitData);

    bool WriteNibble(long FullAddress, byte NibbleData);

    bool WriteByte(long FullAddress, byte ByteData);

    bool EraseFlash(int StartAddress, int NumberOfBytes);

    bool DeleteMeterKey(int MeterKey);

    bool SetNewPin(int NewPin);

    bool TransmitRadioFrame();

    bool DigitalInputsAndOutputs(
      uint NewOutputMask,
      uint NewOutputState,
      ref uint OldOutputState,
      ref uint OldInputState);

    byte[] RunIoTest(IoTestFunctions theFunction);

    bool SetOutput(int OutputNumber, bool State);

    bool GetInput(int InputNumber, out bool InputState);

    bool StartMeterMonitor(int SampleTime);

    bool GetMeterMonitorData(out ByteField MonitorData);

    bool SetOptoTimeoutSeconds(int Seconds);

    bool FlyingTestActivate();

    bool FlyingTestStart();

    bool FlyingTestStop();

    bool FlyingTestReadVolume(out float volume, out MBusDeviceState state);

    bool AdcTestActivate();

    bool CapacityOfTestActivate();

    bool AdcTestCycleWithSimulatedVolume(float simulationVolume);

    bool RadioTestActivate(RadioTestMode testMode);

    bool Start512HzRtcCalibration();

    bool TestDone(long dispValueId);

    bool ReadWaveFlowParameterGroup(
      ParameterGroups TheParameterGroup,
      out SortedList ParameterList,
      out string Fehlerstring);

    bool WriteWaveFlowParameterGroup(
      ParameterGroups TheParameterGroup,
      SortedList ParameterList,
      out string Fehlerstring);

    bool ResetWaveFlowParameterGroup(ParameterGroups TheParameterGroup, out string Fehlerstring);

    bool ReadWaveFlowAnswer(string RequestString, out string XMLString, out string Fehlerstring);

    bool ReadWavePortFirmware(out string TheFirmwareString, out string Fehlerstring);

    string GetWafeFlowSerialNumber();

    bool SetWafeFlowRepeaters(string[] SerialNumbers, out string Fehlerstring);

    bool ActivateWafeFlowRepeaters(out string Fehlerstring);

    bool DeactivateWafeFlowRepeaters(out string Fehlerstring);

    bool GetWafeFlowRepeaters(out string[] Repeaters, out string Fehlerstring);

    bool GetWafeFlowRepeatersAreActivated(out bool Activated, out string Fehlerstring);

    void StartTestloopReadEEProm();

    void StopTestLoop();

    bool SystemInit();

    bool StartHKVEReceptionWindow();

    bool RegisterHKVE(List<string> list);

    bool DeregisterHKVE(List<string> list);

    List<string> GetRegisteredHKVE();

    List<string> GetUnregisteredHKVE();

    bool SetMinomatV2Configuration(MinomatV2.Configuration configuration);

    MinomatV2.Configuration GetMinomatV2Configuration();

    MinomatV2.SystemStatus GetMinomatV2SystemStatus();

    int MaxRequestRepeat { get; set; }

    bool PingSelectedDevice();

    RadioReader RadioReader { get; }

    EDC EDCHandler { get; }

    PDC PDCHandler { get; }

    SmokeDetector SmokeDetectorHandler { get; }

    bool ReadDeviceList();

    bool DeleteSelectedDevice();

    bool SetTime(DateTime dateTime);

    void RaiseProgressEvent(int progressPercentage);

    void RaiseProgressEvent(int progressPercentage, string status);

    byte[] GetTransmitBuffer();

    byte[] GetReceiveBuffer();

    UniqueIdentification GetUniqueIdentificationOfSelectedDevice();

    Dictionary<string, string> LoadAvailableCOMservers();

    event EventHandler<BusDevice> OnDeviceListChanged;

    bool SendFirstApplicationReset { get; set; }

    bool SendFirstSND_NKE { get; set; }

    bool UseREQ_UD2_5B { get; set; }

    bool IsMultiTelegrammEnabled { get; set; }

    PointToPointDevices? SelectedDeviceMBusType { get; set; }

    string MinomatV4_SourceAddress { get; set; }

    event EventHandlerEx<int> OnProgress;

    event EventHandlerEx<string> OnProgressMessage;

    void Dispose();
  }
}
