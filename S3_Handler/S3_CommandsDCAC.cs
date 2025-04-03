// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_CommandsDCAC
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using AsyncCom;
using DeviceCollector;
using NLog;
using System;
using System.Collections.Generic;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public sealed class S3_CommandsDCAC : S3_CommandsBase
  {
    internal static Logger S3_HandlerFunctionsLogger = LogManager.GetLogger("S3_HandlerFunctions");
    internal DeviceCollectorFunctions MyDeviceCollector;
    internal AsyncFunctions MyAsyncCom;

    public S3_CommandsDCAC()
    {
    }

    public S3_CommandsDCAC(DeviceCollectorFunctions devColFunc, AsyncFunctions asyncFunc)
    {
      this.MyDeviceCollector = devColFunc;
      this.MyAsyncCom = asyncFunc;
    }

    public S3_CommandsDCAC(DeviceCollectorFunctions devColFunc)
    {
      this.MyDeviceCollector = devColFunc;
      this.MyAsyncCom = (AsyncFunctions) devColFunc.AsyncCom;
    }

    public override void Connection_Dispose()
    {
      if (this.MyAsyncCom != null)
        this.MyAsyncCom.GMM_Dispose();
      if (this.MyDeviceCollector == null)
        return;
      this.MyDeviceCollector.GMM_Dispose();
    }

    public override bool Open() => this.MyAsyncCom.Open();

    public override bool Close() => this.MyAsyncCom.Close();

    public override string SingleParameter(string Parameter, string ParameterValue)
    {
      return this.MyAsyncCom.SingleParameter(Parameter, ParameterValue);
    }

    public override string SingleParameter(CommParameter Parameter, string ParameterValue)
    {
      return this.MyAsyncCom.SingleParameter(Parameter, ParameterValue);
    }

    public override void ClearWakeup() => this.MyAsyncCom.ClearWakeup();

    public override bool ChangeDriverSettings() => this.MyAsyncCom.ChangeDriverSettings();

    public override bool SetAsyncComSettings(SortedList<string, string> asyncComSettings)
    {
      return this.MyAsyncCom.SetAsyncComSettings(asyncComSettings);
    }

    public override bool TransmitBlock(byte[] buffer)
    {
      S3_CommandsDCAC.S3_HandlerFunctionsLogger.Debug("TransmitBlock: " + Util.ByteArrayToHexString(buffer));
      return this.MyAsyncCom.TransmitBlock(buffer);
    }

    public override bool TransmitBlock(ref ByteField buffer)
    {
      S3_CommandsDCAC.S3_HandlerFunctionsLogger.Debug("TransmitBlock: " + buffer.GetTraceString());
      return this.MyAsyncCom.TransmitBlock(ref buffer);
    }

    public override bool SendBlock(ref ByteField DataBlock)
    {
      S3_CommandsDCAC.S3_HandlerFunctionsLogger.Debug("SendBlock: " + DataBlock.GetTraceString());
      return this.MyAsyncCom.SendBlock(ref DataBlock);
    }

    public override bool ReceiveBlock(ref ByteField DataBlock, int MinByteNb, bool first)
    {
      bool block = this.MyAsyncCom.ReceiveBlock(ref DataBlock, MinByteNb, first);
      S3_CommandsDCAC.S3_HandlerFunctionsLogger.Debug("ReceiveBlock: " + DataBlock.GetTraceString());
      return block;
    }

    public override bool ReceiveBlock(ref ByteField DataBlock)
    {
      bool block = this.MyAsyncCom.ReceiveBlock(ref DataBlock);
      S3_CommandsDCAC.S3_HandlerFunctionsLogger.Debug("ReceiveBlock: " + DataBlock.GetTraceString());
      return block;
    }

    public override void DisableBusWriteOnDispose(bool disable)
    {
      this.MyDeviceCollector.DisableBusWriteOnDispose = disable;
    }

    public override bool ReadMemory(
      MemoryLocation Location,
      int StartAddress,
      int NumberOfBytes,
      out ByteField MemoryData)
    {
      bool flag = this.MyDeviceCollector.ReadMemory(Location, StartAddress, NumberOfBytes, out MemoryData);
      S3_CommandsDCAC.S3_HandlerFunctionsLogger.Debug("read block at " + StartAddress.ToString("x4") + " - bytes " + MemoryData.GetByteArray().Length.ToString("x4"));
      S3_CommandsDCAC.S3_HandlerFunctionsLogger.Debug(BitConverter.ToString(MemoryData.GetByteArray()));
      return flag;
    }

    public override bool WriteMemory(MemoryLocation Location, int StartAddress, ByteField data)
    {
      S3_CommandsDCAC.S3_HandlerFunctionsLogger.Debug("write block at " + StartAddress.ToString("x4") + " - bytes " + data.Count.ToString("x4"));
      S3_CommandsDCAC.S3_HandlerFunctionsLogger.Debug(BitConverter.ToString(data.GetByteArray()));
      return this.MyDeviceCollector.WriteMemory(Location, StartAddress, data);
    }

    public override bool ReadVersion(out ReadVersionData versionData)
    {
      return this.MyDeviceCollector.ReadVersion(out versionData);
    }

    public override bool ReadVersion(
      out short Connected_Manufacturer,
      out byte Connected_Medium,
      out byte Connected_MBusMeterType,
      out long Connected_Version,
      out int Connected_MBusSerialNr,
      out int Connected_ConfigAdr,
      out int Connected_HardwareMask)
    {
      return this.MyDeviceCollector.ReadVersion(out Connected_Manufacturer, out Connected_Medium, out Connected_MBusMeterType, out Connected_Version, out Connected_MBusSerialNr, out Connected_ConfigAdr, out Connected_HardwareMask);
    }

    public override bool DeviceProtectionGet() => this.MyDeviceCollector.DeviceProtectionGet();

    public override bool DeviceProtectionSet() => this.MyDeviceCollector.DeviceProtectionSet();

    public override bool DeviceProtectionReset(uint meterKey)
    {
      return this.MyDeviceCollector.DeviceProtectionReset(meterKey);
    }

    public override bool DeviceProtectionSetKey(uint meterKey)
    {
      return this.MyDeviceCollector.DeviceProtectionSetKey(meterKey);
    }

    public override bool RunBackup() => this.MyDeviceCollector.RunBackup();

    public override bool ResetDevice() => this.MyDeviceCollector.ResetDevice();

    public override bool SetEmergencyMode() => this.MyDeviceCollector.SetEmergencyMode();

    public override bool EraseFlash(int StartAddress, int NumberOfBytes)
    {
      S3_CommandsDCAC.S3_HandlerFunctionsLogger.Debug("EraseFlash at adress 0x" + StartAddress.ToString("x4") + " - bytes 0x" + NumberOfBytes.ToString("x4"));
      return this.MyDeviceCollector.EraseFlash(StartAddress, NumberOfBytes);
    }

    public override bool FlyingTestActivate() => this.MyDeviceCollector.FlyingTestActivate();

    public override bool FlyingTestStart() => this.MyDeviceCollector.FlyingTestStart();

    public override bool FlyingTestStop() => this.MyDeviceCollector.FlyingTestStop();

    public override bool FlyingTestReadVolume(out float volume, out MBusDeviceState state)
    {
      return this.MyDeviceCollector.FlyingTestReadVolume(out volume, out state);
    }

    public override bool AdcTestActivate() => this.MyDeviceCollector.AdcTestActivate();

    public override bool AdcTestCycleWithSimulatedVolume(float simulationVolume)
    {
      return this.MyDeviceCollector.AdcTestCycleWithSimulatedVolume(simulationVolume);
    }

    public override bool CapacityOfTestActivate()
    {
      return this.MyDeviceCollector.CapacityOfTestActivate();
    }

    public override bool Start512HzRtcCalibration()
    {
      return this.MyDeviceCollector.Start512HzRtcCalibration();
    }

    public override bool TestDone(long dispValueId) => this.MyDeviceCollector.TestDone(dispValueId);

    public override bool GetMeterMonitorData(out ByteField MonitorData)
    {
      return this.MyDeviceCollector.GetMeterMonitorData(out MonitorData);
    }

    public override BusDevice GetSelectedDevice() => this.MyDeviceCollector.GetSelectedDevice();

    public override bool SetSelectedDeviceBySerialNumber(string SerialNumber)
    {
      return this.MyDeviceCollector.SetSelectedDeviceBySerialNumber(SerialNumber);
    }

    public override bool SetPhysicalDeviceBySerialNumber(string SerialNumber)
    {
      return this.MyDeviceCollector.SetPhysicalDeviceBySerialNumber(SerialNumber);
    }

    public override bool IsSelectedDevice(DeviceTypes TestType)
    {
      return this.MyDeviceCollector.IsSelectedDevice(TestType);
    }

    public override ZR_ClassLibrary.BusMode GetBaseMode() => this.MyDeviceCollector.GetBaseMode();

    public override void DeleteBusInfo() => this.MyDeviceCollector.DeleteBusInfo();

    public override bool AddDevice(DeviceTypes NewType, int PrimaryAddress)
    {
      return this.MyDeviceCollector.AddDevice(NewType, PrimaryAddress);
    }

    public override ImpulseInputCounters ReadInputCounters()
    {
      return this.MyDeviceCollector.ReadInputCounters();
    }

    public override bool RadioTestActivate(RadioTestMode testMode)
    {
      return this.MyDeviceCollector.RadioTestActivate(testMode);
    }

    public override bool SetOptoTimeoutSeconds(int Seconds)
    {
      return this.MyDeviceCollector.SetOptoTimeoutSeconds(Seconds);
    }

    public override void ShowCommunicatioWindow() => this.MyDeviceCollector.ShowBusWindow();

    public override void ShowComWindow() => this.MyAsyncCom.ShowComWindow();

    public override int ConnectionProfileID => this.MyDeviceCollector.ConnectionProfileID;

    public override SortedList<DeviceCollectorSettings, object> GetDeviceCollectorSettings()
    {
      return this.MyDeviceCollector.GetDeviceCollectorSettings();
    }

    public override byte[] RunIoTest(IoTestFunctions theFunction)
    {
      return this.MyDeviceCollector.RunIoTest(theFunction);
    }

    public override bool SetDeviceCollectorSettings(SortedList<string, string> settings)
    {
      return this.MyDeviceCollector.SetDeviceCollectorSettings(settings);
    }

    public override bool DigitalInputsAndOutputs(
      uint NewOutputMask,
      uint NewOutputState,
      ref uint OldOutputState,
      ref uint OldInputState)
    {
      return this.MyDeviceCollector.DigitalInputsAndOutputs(NewOutputMask, NewOutputState, ref OldOutputState, ref OldInputState);
    }

    public override void SetReadoutConfiguration(ConfigList configList)
    {
      this.MyAsyncCom.SetReadoutConfiguration(configList);
      this.MyDeviceCollector.SetReadoutConfiguration(configList);
    }

    public override ConfigList GetReadoutConfiguration()
    {
      return this.MyAsyncCom.GetReadoutConfiguration();
    }
  }
}
