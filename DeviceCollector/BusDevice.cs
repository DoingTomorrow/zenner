// Decompiled with JetBrains decompiler
// Type: DeviceCollector.BusDevice
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using HandlerLib;
using PlugInLib;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class BusDevice : EventArgs
  {
    private static SortedList<string, RightInfo> EnabledLicenseRights = (SortedList<string, RightInfo>) ((IEnumerable<FieldInfo>) typeof (UserManager).GetFields(BindingFlags.Static | BindingFlags.NonPublic)).Where<FieldInfo>((System.Func<FieldInfo, bool>) (f => f.GetValue((object) null) is SortedList<string, RightInfo>)).FirstOrDefault<FieldInfo>().GetValue((object) null);
    internal bool UseMaxBaudrate = false;
    internal int TableIndex = -1;
    internal DataRow TableDataRow = (DataRow) null;
    public DeviceCollectorFunctions MyBus;
    internal ByteField TransmitBuffer;
    internal ByteField ReceiveBuffer;
    internal List<byte> TotalReceiveBuffer;
    internal MemoryLocation WatchMemoryLocation;
    internal int WatchStartAddress;
    internal int WatchNumberOfBytes;
    public DeviceInfo Info = new DeviceInfo();

    public static void CheckReadOnlyRight()
    {
      if (!UserManager.CheckPermission("Role\\Developer") && BusDevice.EnabledLicenseRights != null && BusDevice.EnabledLicenseRights.ContainsKey("ReadOnly") && UserManager.CheckPermission("Right\\ReadOnly"))
        throw new Exception("Access denied! The right 'ReadOnly' is set to true. Please check your licence.");
    }

    internal DeviceTypes DeviceType
    {
      set => this.Info.DeviceType = value;
      get => this.Info.DeviceType;
    }

    public BusDevice()
    {
    }

    public BusDevice(DeviceCollectorFunctions TheBus)
    {
      this.MyBus = TheBus;
      this.DeviceType = DeviceTypes.None;
      this.TotalReceiveBuffer = new List<byte>();
    }

    internal virtual bool SetRepeaters(string[] SerialNumbers, out string Fehlerstring)
    {
      Fehlerstring = "Not implemented!";
      return false;
    }

    internal virtual void ActivateRepeaters()
    {
    }

    internal virtual void DeactivateRepeaters()
    {
    }

    internal virtual string[] GetRepeaters() => new string[0];

    internal virtual bool GetRepeatersAreActivated() => false;

    internal virtual bool ReadVersion(out ReadVersionData versionData)
    {
      versionData = (ReadVersionData) null;
      this.FunctionNotAvailable(nameof (ReadVersion));
      return false;
    }

    internal virtual bool ResetDevice(int AfterResetBaudrate) => this.ResetNotImplemented();

    internal virtual bool ResetDevice() => this.ResetNotImplemented();

    internal virtual bool ResetDevice(bool loadBackup) => this.ResetNotImplemented();

    internal virtual byte[] RunIoTest(IoTestFunctions theFunction)
    {
      this.FunctionNotAvailable(nameof (RunIoTest));
      return (byte[]) null;
    }

    private bool ResetNotImplemented()
    {
      this.FunctionNotAvailable(DeviceCollectorFunctions.SerialBusMessage.GetString(DeviceCollectorFunctions.SerialBusMessage.GetString("9")));
      return false;
    }

    internal virtual bool ReadAnswerString(string RequestString, out string AnswerString)
    {
      AnswerString = string.Empty;
      return false;
    }

    internal virtual bool ReadParameterGroup(
      ParameterGroups TheParameterGroup,
      int Retries,
      out object ParameterData)
    {
      ParameterData = (object) null;
      return false;
    }

    internal virtual bool ReadParameterGroup(
      ParameterGroups TheParameterGroup,
      out object ParameterData)
    {
      ParameterData = (object) null;
      return false;
    }

    internal virtual bool WriteParameterGroup(
      ParameterGroups TheParameterGroup,
      object ParameterData)
    {
      return false;
    }

    internal virtual bool ResetParameterGroup(ParameterGroups TheParameterGroup) => false;

    internal virtual bool GetDeviceConfiguration(out SortedList<OverrideID, object> ConfigParamList)
    {
      ConfigParamList = (SortedList<OverrideID, object>) null;
      return false;
    }

    internal virtual bool EraseFlash(int StartAddress, int NumberOfBytes)
    {
      return this.FunctionNotAvailable(nameof (EraseFlash));
    }

    internal virtual bool SelectParameterList(int ListNumber, int function)
    {
      return this.FunctionNotAvailable(nameof (SelectParameterList));
    }

    internal virtual ParameterListInfo ReadParameterList()
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented, nameof (ReadParameterList));
      return (ParameterListInfo) null;
    }

    internal virtual bool WriteDueDateMonth(ushort month)
    {
      return this.FunctionNotAvailable("SelectParameterList");
    }

    internal virtual bool SetOptoTimeoutSeconds(int Seconds)
    {
      return this.FunctionNotAvailable(nameof (SetOptoTimeoutSeconds));
    }

    internal virtual bool FlyingTestActivate()
    {
      return this.FunctionNotAvailable(nameof (FlyingTestActivate));
    }

    internal virtual bool FlyingTestStart() => this.FunctionNotAvailable(nameof (FlyingTestStart));

    internal virtual bool FlyingTestStop() => this.FunctionNotAvailable(nameof (FlyingTestStop));

    internal virtual bool FlyingTestReadVolume(out float volume, out MBusDeviceState state)
    {
      volume = 0.0f;
      state = MBusDeviceState.AnyError;
      return this.FunctionNotAvailable(nameof (FlyingTestReadVolume));
    }

    internal virtual bool AdcTestActivate() => this.FunctionNotAvailable(nameof (AdcTestActivate));

    internal virtual bool CapacityOfTestActivate()
    {
      return this.FunctionNotAvailable(nameof (CapacityOfTestActivate));
    }

    internal virtual bool AdcTestCycleWithSimulatedVolume(float simulationVolume)
    {
      return this.FunctionNotAvailable(nameof (AdcTestCycleWithSimulatedVolume));
    }

    internal virtual bool RadioTest(RadioTestMode testMode)
    {
      return this.FunctionNotAvailable(nameof (RadioTest));
    }

    internal virtual bool Start512HzRtcCalibration()
    {
      return this.FunctionNotAvailable(nameof (Start512HzRtcCalibration));
    }

    internal virtual bool TestDone(long dispValueId)
    {
      return this.FunctionNotAvailable(nameof (TestDone));
    }

    private bool FunctionNotAvailable(string FunctionName)
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented, FunctionName);
      return false;
    }

    internal virtual bool GetMeterMonitorData(out ByteField Buffer)
    {
      Buffer = (ByteField) null;
      return this.FunctionNotAvailable(nameof (GetMeterMonitorData));
    }

    internal virtual bool DeviceProtectionGet()
    {
      return this.FunctionNotAvailable(nameof (DeviceProtectionGet));
    }

    internal virtual bool DeviceProtectionSet()
    {
      return this.FunctionNotAvailable(nameof (DeviceProtectionSet));
    }

    internal virtual bool DeviceProtectionReset(uint meterKey)
    {
      return this.FunctionNotAvailable(nameof (DeviceProtectionReset));
    }

    internal virtual bool DeviceProtectionSetKey(uint meterKey)
    {
      return this.FunctionNotAvailable(nameof (DeviceProtectionSetKey));
    }
  }
}
