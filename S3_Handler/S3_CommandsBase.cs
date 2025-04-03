// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_CommandsBase
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using AsyncCom;
using DeviceCollector;
using HandlerLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class S3_CommandsBase
  {
    public virtual void Connection_Dispose()
    {
      throw new NotImplementedException(nameof (Connection_Dispose));
    }

    public virtual void ShowTestWindow(CommonCommandWindowsName windowName)
    {
      throw new NotImplementedException(nameof (ShowTestWindow));
    }

    public virtual DeviceIdentification GetDeviceIdentification()
    {
      throw new NotImplementedException(nameof (GetDeviceIdentification));
    }

    public virtual bool Open() => throw new NotImplementedException(nameof (Open));

    public virtual bool Close() => throw new NotImplementedException(nameof (Close));

    public virtual string SingleParameter(string Parameter, string ParameterValue)
    {
      throw new NotImplementedException(nameof (SingleParameter));
    }

    public virtual string SingleParameter(CommParameter Parameter, string ParameterValue)
    {
      throw new NotImplementedException("SingleParameter1");
    }

    public virtual void ClearWakeup() => throw new NotImplementedException(nameof (ClearWakeup));

    public virtual bool ChangeDriverSettings()
    {
      throw new NotImplementedException(nameof (ChangeDriverSettings));
    }

    public virtual bool SetAsyncComSettings(SortedList<string, string> asyncComSettings)
    {
      throw new NotImplementedException(nameof (SetAsyncComSettings));
    }

    public virtual bool TransmitBlock(byte[] buffer)
    {
      throw new NotImplementedException(nameof (TransmitBlock));
    }

    public virtual bool TransmitBlock(ref ByteField buffer)
    {
      throw new NotImplementedException(nameof (TransmitBlock));
    }

    public virtual bool SendBlock(ref ByteField DataBlock)
    {
      throw new NotImplementedException(nameof (SendBlock));
    }

    public virtual bool ReceiveBlock(ref ByteField DataBlock, int MinByteNb, bool first)
    {
      throw new NotImplementedException(nameof (ReceiveBlock));
    }

    public virtual bool ReceiveBlock(ref ByteField DataBlock)
    {
      throw new NotImplementedException(nameof (ReceiveBlock));
    }

    public virtual void DisableBusWriteOnDispose(bool disable = true)
    {
      throw new NotImplementedException(nameof (DisableBusWriteOnDispose));
    }

    public virtual bool ReadMemory(
      MemoryLocation Location,
      int StartAddress,
      int NumberOfBytes,
      out ByteField MemoryData)
    {
      throw new NotImplementedException(nameof (ReadMemory));
    }

    public virtual bool WriteMemory(MemoryLocation Location, int StartAddress, ByteField data)
    {
      throw new NotImplementedException(nameof (WriteMemory));
    }

    public virtual bool ReadVersion() => throw new NotImplementedException(nameof (ReadVersion));

    public virtual bool ReadVersion(out ReadVersionData versionData)
    {
      throw new NotImplementedException("ReadVersion1");
    }

    public virtual bool ReadVersion(
      out short Connected_Manufacturer,
      out byte Connected_Medium,
      out byte Connected_MBusMeterType,
      out long Connected_Version,
      out int Connected_MBusSerialNr,
      out int Connected_ConfigAdr,
      out int Connected_HardwareMask)
    {
      Connected_Manufacturer = (short) 0;
      Connected_Medium = (byte) 0;
      Connected_MBusMeterType = (byte) 0;
      Connected_Version = 0L;
      Connected_MBusSerialNr = 0;
      Connected_ConfigAdr = 0;
      Connected_HardwareMask = 0;
      ReadVersionData versionData;
      if (!this.ReadVersion(out versionData))
        return false;
      Connected_Manufacturer = versionData.MBusManufacturer;
      Connected_Medium = versionData.mBusMedium;
      Connected_MBusMeterType = versionData.MBusGeneration;
      Connected_Version = (long) versionData.Version;
      Connected_MBusSerialNr = (int) versionData.MBusSerialNr;
      Connected_ConfigAdr = 0;
      Connected_HardwareMask = (int) versionData.HardwareIdentification;
      return true;
    }

    public virtual bool DeviceProtectionGet()
    {
      throw new NotImplementedException(nameof (DeviceProtectionGet));
    }

    public virtual bool DeviceProtectionSet()
    {
      throw new NotImplementedException(nameof (DeviceProtectionSet));
    }

    public virtual bool DeviceProtectionReset(uint meterKey)
    {
      throw new NotImplementedException(nameof (DeviceProtectionReset));
    }

    public virtual bool DeviceProtectionSetKey(uint meterKey)
    {
      throw new NotImplementedException(nameof (DeviceProtectionSetKey));
    }

    public virtual bool RunBackup() => throw new NotImplementedException(nameof (RunBackup));

    public virtual bool ResetDevice() => throw new NotImplementedException("RunBackup");

    public virtual bool SetEmergencyMode()
    {
      throw new NotImplementedException(nameof (SetEmergencyMode));
    }

    public virtual bool EraseFlash(int StartAddress, int NumberOfBytes)
    {
      throw new NotImplementedException(nameof (EraseFlash));
    }

    public virtual bool FlyingTestActivate()
    {
      throw new NotImplementedException(nameof (FlyingTestActivate));
    }

    public virtual bool FlyingTestStart()
    {
      throw new NotImplementedException(nameof (FlyingTestStart));
    }

    public virtual bool FlyingTestStop()
    {
      throw new NotImplementedException(nameof (FlyingTestStop));
    }

    public virtual bool FlyingTestReadVolume(out float volume, out MBusDeviceState state)
    {
      throw new NotImplementedException(nameof (FlyingTestReadVolume));
    }

    public virtual bool AdcTestActivate()
    {
      throw new NotImplementedException(nameof (AdcTestActivate));
    }

    public virtual bool AdcTestCycleWithSimulatedVolume(float simulationVolume)
    {
      throw new NotImplementedException(nameof (AdcTestCycleWithSimulatedVolume));
    }

    public virtual bool CapacityOfTestActivate()
    {
      throw new NotImplementedException(nameof (CapacityOfTestActivate));
    }

    public virtual bool Start512HzRtcCalibration()
    {
      throw new NotImplementedException(nameof (Start512HzRtcCalibration));
    }

    public virtual bool TestDone(long dispValueId)
    {
      throw new NotImplementedException(nameof (TestDone));
    }

    public virtual bool GetMeterMonitorData(out ByteField MonitorData)
    {
      throw new NotImplementedException(nameof (GetMeterMonitorData));
    }

    public virtual BusDevice GetSelectedDevice()
    {
      throw new NotImplementedException(nameof (GetSelectedDevice));
    }

    public virtual bool SetSelectedDeviceBySerialNumber(string SerialNumber)
    {
      throw new NotImplementedException(nameof (SetSelectedDeviceBySerialNumber));
    }

    public virtual bool SetPhysicalDeviceBySerialNumber(string SerialNumber)
    {
      throw new NotImplementedException("SetSelectedDeviceBySerialNumber");
    }

    public virtual bool IsSelectedDevice(DeviceTypes TestType)
    {
      throw new NotImplementedException(nameof (IsSelectedDevice));
    }

    public virtual ZR_ClassLibrary.BusMode GetBaseMode()
    {
      throw new NotImplementedException(nameof (GetBaseMode));
    }

    public virtual void DeleteBusInfo()
    {
      throw new NotImplementedException(nameof (DeleteBusInfo));
    }

    public virtual bool AddDevice(DeviceTypes NewType, int PrimaryAddress)
    {
      throw new NotImplementedException(nameof (AddDevice));
    }

    public virtual ImpulseInputCounters ReadInputCounters()
    {
      throw new NotImplementedException(nameof (ReadInputCounters));
    }

    public virtual bool RadioTestActivate(RadioTestMode testMode)
    {
      throw new NotImplementedException(nameof (RadioTestActivate));
    }

    public virtual bool SetOptoTimeoutSeconds(int Seconds)
    {
      throw new NotImplementedException(nameof (SetOptoTimeoutSeconds));
    }

    public virtual void ShowCommunicatioWindow()
    {
      throw new NotImplementedException(nameof (ShowCommunicatioWindow));
    }

    public virtual void ShowComWindow()
    {
      throw new NotImplementedException(nameof (ShowComWindow));
    }

    public void ShowBusWindow() => this.ShowCommunicatioWindow();

    public virtual int ConnectionProfileID
    {
      get => throw new NotImplementedException(nameof (ConnectionProfileID));
    }

    public virtual SortedList<DeviceCollectorSettings, object> GetDeviceCollectorSettings()
    {
      throw new NotImplementedException(nameof (GetDeviceCollectorSettings));
    }

    public virtual byte[] RunIoTest(IoTestFunctions theFunction)
    {
      throw new NotImplementedException(nameof (RunIoTest));
    }

    public virtual bool SetDeviceCollectorSettings(SortedList<string, string> settings)
    {
      throw new NotImplementedException(nameof (SetDeviceCollectorSettings));
    }

    public virtual bool DigitalInputsAndOutputs(
      uint NewOutputMask,
      uint NewOutputState,
      ref uint OldOutputState,
      ref uint OldInputState)
    {
      throw new NotImplementedException(nameof (DigitalInputsAndOutputs));
    }

    public virtual void SetReadoutConfiguration(ConfigList configList)
    {
      throw new NotImplementedException(nameof (SetReadoutConfiguration));
    }

    public virtual ConfigList GetReadoutConfiguration()
    {
      throw new NotImplementedException("SetReadoutConfiguration");
    }

    public virtual async Task SendTestPacketAsync(
      ushort interval,
      ushort timeoutInSec,
      uint deviceID,
      byte[] arbitraryData,
      string syncWord)
    {
      throw new NotImplementedException(nameof (SendTestPacketAsync));
    }

    public virtual async Task<double?> ReceiveTestPacketAsync(
      byte timeoutInSec,
      uint deviceID,
      string syncWord)
    {
      throw new NotImplementedException(nameof (ReceiveTestPacketAsync));
    }
  }
}
