// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_CommandsCOMPATIBLE
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using AsyncCom;
using CommunicationPort.Functions;
using CommunicationPort.UserInterface;
using DeviceCollector;
using GmmDbLib;
using HandlerLib;
using NLog;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class S3_CommandsCOMPATIBLE : S3_CommandsBase
  {
    internal static Logger S3_CommandsCOMPATIBLE_Logger = LogManager.GetLogger(nameof (S3_CommandsCOMPATIBLE));
    internal ConfigList configList;
    internal CommunicationPortFunctions myPort;
    internal CommunicationPortWindowFunctions myPortWinFunction;
    internal BaseDbConnection myDb;
    internal Common16BitCommands myCommands16Bit;
    internal DeviceCommandsMBus myDeviceCommandsMBus;
    internal Common32BitCommands myCommands32Bit;
    internal CommonLoRaCommands myCommandsLoRa;
    internal CommonMBusCommands myCommandsMBus;
    internal CommonRadioCommands myCommandsRadio;
    internal DeviceIdentification myDeviceIdentification;
    internal ProgressHandler progress;
    internal CancellationToken cancelToken;
    internal const int MaxWriteBlockSizeLow = 16;
    internal const long HighBlockSizeVersion = 17104897;
    internal const int MaxWriteBlockSizeHigh = 32;
    internal const int MaxWriteBlockSizeSerie3 = 150;
    internal const int MaxWriteBlockSizeSerie3LowBaud = 16;
    internal const long Baud38400Version = 17104897;
    public static int[] AllBaudrates = new int[5]
    {
      2400,
      4800,
      9600,
      38400,
      300
    };
    public static int[] All_C2_Baudrates = new int[2]
    {
      2400,
      4800
    };
    public static int[] All_WR3_Baudrates = new int[4]
    {
      2400,
      9600,
      38400,
      300
    };
    internal int MaxWriteBlockSize;
    private bool disableBusWriteOnDispose = false;

    public S3_CommandsCOMPATIBLE(
      ProgressHandler mainProgress,
      CancellationToken mainCancel,
      CommunicationPortWindowFunctions myPortWinFunction)
    {
      this.progress = mainProgress;
      this.cancelToken = mainCancel;
      this.myPortWinFunction = myPortWinFunction;
      this.myPort = myPortWinFunction.portFunctions;
      this.configList = this.myPort.GetReadoutConfiguration();
      this.myDeviceCommandsMBus = new DeviceCommandsMBus((IPort) this.myPort);
      this.myCommands16Bit = new Common16BitCommands(this.myDeviceCommandsMBus);
      this.myCommands32Bit = new Common32BitCommands(this.myDeviceCommandsMBus);
      this.myCommandsLoRa = new CommonLoRaCommands(this.myCommands32Bit);
      this.myCommandsMBus = new CommonMBusCommands(this.myCommands32Bit);
      this.myCommandsRadio = new CommonRadioCommands(this.myCommands32Bit);
    }

    public override async Task SendTestPacketAsync(
      ushort interval,
      ushort timeoutInSec,
      uint deviceID,
      byte[] arbitraryData,
      string syncWord)
    {
      if (deviceID == 0U)
        await this.myCommandsRadio.SendTestPacketAsync(interval, timeoutInSec, arbitraryData, this.progress, this.cancelToken);
      else
        await this.myCommandsRadio.SendTestPacketAsync(interval, timeoutInSec, deviceID, arbitraryData, this.progress, this.cancelToken, syncWord);
    }

    public override async Task<double?> ReceiveTestPacketAsync(
      byte timeoutInSec,
      uint deviceID,
      string syncWord)
    {
      double? testPacketAsync = await this.myCommandsRadio.ReceiveTestPacketAsync(timeoutInSec, deviceID, this.progress, this.cancelToken, syncWord);
      return testPacketAsync;
    }

    public override void ShowTestWindow(CommonCommandWindowsName windowName)
    {
      switch (windowName)
      {
        case CommonCommandWindowsName.COMMON:
          new CommandWindowCommon(this.myCommands32Bit).ShowDialog();
          break;
        case CommonCommandWindowsName.MBUS:
          new MBusCommandWindow(this.myCommandsMBus).ShowDialog();
          break;
        case CommonCommandWindowsName.RADIO:
          new RadioCommandWindow(this.myCommandsRadio, (IPort) this.myPort).ShowDialog();
          break;
        case CommonCommandWindowsName.LORA:
          new LoRaCommandWindow(this.myCommandsLoRa).ShowDialog();
          break;
      }
    }

    public override void SetReadoutConfiguration(ConfigList ConfigList)
    {
      if (ConfigList == null)
        throw new ArgumentNullException(nameof (ConfigList));
      if (this.configList == null)
      {
        this.configList = ConfigList;
        this.configList.PropertyChanged += new PropertyChangedEventHandler(this.configList_PropertyChanged);
      }
      else if (this.configList != ConfigList)
        throw new ArgumentException("this.configList != configList");
      if (this.myPort == null)
        return;
      this.myPort.SetReadoutConfiguration(this.configList);
    }

    private void configList_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
    }

    public override ConfigList GetReadoutConfiguration() => this.configList;

    public override void Connection_Dispose()
    {
      if (this.myPort == null)
        return;
      this.myPort.Dispose();
    }

    public override void DisableBusWriteOnDispose(bool disable)
    {
      this.disableBusWriteOnDispose = disable;
    }

    public override bool ReadMemory(
      MemoryLocation Location,
      int StartAddress,
      int NumberOfBytes,
      out ByteField MemoryData)
    {
      try
      {
        S3_CommandsCOMPATIBLE.S3_CommandsCOMPATIBLE_Logger.Debug("read block at " + StartAddress.ToString("x4") + " - bytes " + NumberOfBytes.ToString("x4"));
        ushort startADR = ushort.Parse(StartAddress.ToString());
        uint count = uint.Parse(NumberOfBytes.ToString());
        byte maxBpP = Convert.ToByte(this.MaxWriteBlockSize);
        Task<byte[]> task = Task.Run<byte[]>((Func<byte[]>) (() => this.myCommands16Bit.ReadMemory(this.progress, this.cancelToken, startADR, count, maxBpP)));
        TimeSpan timeSpan1 = new TimeSpan(0L);
        while (!task.IsCompleted)
        {
          DateTime now = DateTime.Now;
          Thread.Sleep(80);
          Application.DoEvents();
          TimeSpan timeSpan2 = DateTime.Now.Subtract(now);
          if (timeSpan2 > timeSpan1)
            timeSpan1 = timeSpan2;
        }
        byte[] ByteArray = task.Exception == null ? task.Result : throw task.Exception;
        MemoryData = new ByteField(ByteArray);
        S3_CommandsCOMPATIBLE.S3_CommandsCOMPATIBLE_Logger.Trace("MaxDoEventsTimeSpan [ms]: " + timeSpan1.TotalMilliseconds.ToString());
        S3_CommandsCOMPATIBLE.S3_CommandsCOMPATIBLE_Logger.Debug("read result: " + BitConverter.ToString(MemoryData.GetByteArray()));
        this.progress.Report("MessageEnd", (object) GMM_EventArgs.MessageType.EndMessage);
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "ReadMemory error");
        MemoryData = (ByteField) null;
        this.progress.Report("MessageEnd", (object) GMM_EventArgs.MessageType.EndMessage);
      }
      return false;
    }

    public override bool WriteMemory(MemoryLocation Location, int StartAddress, ByteField data)
    {
      try
      {
        if (!this.CheckMemoryLocation(Location))
          throw new Exception("Only FLASH and RAM are acceptable memory locations !!!");
        byte[] writeData = data.GetByteArray();
        ushort address = 0;
        address = Convert.ToUInt16(StartAddress);
        byte cmd = 3;
        switch (Location)
        {
          case MemoryLocation.EEPROM:
            cmd = (byte) 1;
            break;
          case MemoryLocation.RAM:
            cmd = (byte) 3;
            break;
          case MemoryLocation.FLASH:
            cmd = (byte) 1;
            break;
        }
        byte maxBPP = Convert.ToByte(this.MaxWriteBlockSize);
        Task task = Task.Run((Action) (() => this.myCommands16Bit.WriteMemory(this.progress, this.cancelToken, cmd, address, writeData, maxBPP)));
        while (!task.IsCompleted)
        {
          Thread.Sleep(100);
          Application.DoEvents();
        }
        if (task.Exception != null)
          throw task.Exception;
        S3_CommandsCOMPATIBLE.S3_CommandsCOMPATIBLE_Logger.Debug("write block at " + address.ToString("x4") + " - bytes " + data.GetByteArray().Length.ToString("x4"));
        S3_CommandsCOMPATIBLE.S3_CommandsCOMPATIBLE_Logger.Debug(BitConverter.ToString(data.GetByteArray()));
        this.progress.Report("MessageEnd", (object) GMM_EventArgs.MessageType.EndMessage);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "WriteMemory error");
        this.progress.Report("MessageEnd", (object) GMM_EventArgs.MessageType.EndMessage);
        return false;
      }
      return true;
    }

    private bool CheckMemoryLocation(MemoryLocation location)
    {
      if (location == MemoryLocation.RAM || location == MemoryLocation.FLASH)
        return true;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal memory location");
      return false;
    }

    public override bool ReadVersion(out ReadVersionData versionData)
    {
      versionData = new ReadVersionData();
      try
      {
        this.MaxWriteBlockSize = 16;
        DeviceVersionMBus deviceVersionMbus = this.myDeviceCommandsMBus.ReadVersion(this.progress, this.cancelToken);
        ReadVersionData readVersionData1 = versionData;
        DateTime? buildTime = deviceVersionMbus.BuildTime;
        uint? nullable1;
        int num1;
        if (buildTime.HasValue)
        {
          nullable1 = deviceVersionMbus.SvnRevision;
          num1 = (int) nullable1.Value;
        }
        else
          num1 = 0;
        readVersionData1.BuildRevision = (uint) num1;
        ReadVersionData readVersionData2 = versionData;
        buildTime = deviceVersionMbus.BuildTime;
        DateTime minValue;
        if (buildTime.HasValue)
        {
          buildTime = deviceVersionMbus.BuildTime;
          minValue = buildTime.Value;
        }
        else
          minValue = DateTime.MinValue;
        readVersionData2.BuildTime = minValue;
        versionData.FirmwareSignature = !deviceVersionMbus.Signatur.HasValue ? (ushort) 0 : deviceVersionMbus.Signatur.Value;
        ReadVersionData readVersionData3 = versionData;
        nullable1 = deviceVersionMbus.HardwareID;
        int num2;
        if (nullable1.HasValue)
        {
          nullable1 = deviceVersionMbus.HardwareID;
          num2 = (int) nullable1.Value;
        }
        else
          num2 = 0;
        readVersionData3.HardwareIdentification = (uint) num2;
        ReadVersionData readVersionData4 = versionData;
        byte? nullable2 = deviceVersionMbus.Generation;
        int num3;
        if (nullable2.HasValue)
        {
          nullable2 = deviceVersionMbus.Generation;
          num3 = (int) nullable2.Value;
        }
        else
          num3 = 0;
        readVersionData4.MBusGeneration = (byte) num3;
        ReadVersionData readVersionData5 = versionData;
        nullable2 = deviceVersionMbus.Medium;
        int num4;
        if (nullable2.HasValue)
        {
          nullable2 = deviceVersionMbus.Medium;
          num4 = (int) nullable2.Value;
        }
        else
          num4 = 0;
        readVersionData5.MBusMedium = (byte) num4;
        versionData.MBusSerialNr = 0U;
        ReadVersionData readVersionData6 = versionData;
        nullable1 = deviceVersionMbus.FirmwareVersion;
        int num5;
        if (nullable1.HasValue)
        {
          nullable1 = deviceVersionMbus.FirmwareVersion;
          num5 = (int) nullable1.Value;
        }
        else
          num5 = 0;
        readVersionData6.Version = (uint) num5;
        versionData.PacketSizeOfResponceByGetVersionCommand = new int?(deviceVersionMbus.FirmwareVersionObj.VersionString.Length);
        nullable1 = deviceVersionMbus.FirmwareVersion;
        if (nullable1.Value >= 17104897U)
          this.MaxWriteBlockSize = 32;
        this.MaxWriteBlockSize = this.configList.Baudrate == 115200 ? 150 : 16;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read version error.");
        return false;
      }
      return true;
    }

    public override void ShowCommunicatioWindow() => this.myPortWinFunction.ShowMainWindow();

    public override void ShowComWindow() => this.myPortWinFunction.ShowMainWindow();

    public override bool IsSelectedDevice(DeviceTypes TestType) => true;

    public override BusDevice GetSelectedDevice() => (BusDevice) null;

    public override void ClearWakeup() => this.myPort.ForceWakeup();

    public override ZR_ClassLibrary.BusMode GetBaseMode()
    {
      try
      {
        return (ZR_ClassLibrary.BusMode) Enum.Parse(typeof (ZR_ClassLibrary.BusMode), this.configList.BusMode, true);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        return ZR_ClassLibrary.BusMode.unknown;
      }
    }

    public override string SingleParameter(string Parameter, string ParameterValue)
    {
      return this.configList.ContainsKey(Parameter) ? this.configList[Parameter] : string.Empty;
    }

    public override string SingleParameter(CommParameter Parameter, string ParameterValue)
    {
      string key = Parameter.ToString();
      return this.configList.ContainsKey(key) ? this.configList[key] : string.Empty;
    }

    public override bool RunBackup()
    {
      int timeBeforFirstByte = this.configList.RecTime_BeforFirstByte;
      try
      {
        this.configList.RecTime_BeforFirstByte = 5000;
        Task task = Task.Run((Action) (() => this.myCommands16Bit.BackupDevice(this.progress, this.cancelToken)));
        while (!task.IsCompleted)
        {
          Thread.Sleep(100);
          Application.DoEvents();
        }
        if (task.Exception != null)
          throw task.Exception;
        this.configList.RecTime_BeforFirstByte = timeBeforFirstByte;
        this.progress.Report("MessageEnd", (object) GMM_EventArgs.MessageType.EndMessage);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "RunBackup error");
        this.configList.RecTime_BeforFirstByte = timeBeforFirstByte;
        return false;
      }
      return true;
    }

    public override bool ResetDevice()
    {
      int timeBeforFirstByte = this.configList.RecTime_BeforFirstByte;
      try
      {
        this.configList.RecTime_BeforFirstByte = 1000;
        Task task = Task.Run((Action) (() => this.myCommands16Bit.ResetDevice(this.progress, this.cancelToken)));
        while (!task.IsCompleted)
        {
          Thread.Sleep(100);
          Application.DoEvents();
        }
        if (task.Exception != null)
          throw task.Exception;
        this.configList.RecTime_BeforFirstByte = timeBeforFirstByte;
        this.progress.Report("MessageEnd", (object) GMM_EventArgs.MessageType.EndMessage);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "ResetDevice error");
        this.configList.RecTime_BeforFirstByte = timeBeforFirstByte;
        return false;
      }
      return true;
    }

    public override bool SetEmergencyMode()
    {
      try
      {
        S3_CommandsCOMPATIBLE.S3_CommandsCOMPATIBLE_Logger.Debug("SetEmergencyMode !!!");
        Task<bool> task = Task.Run<bool>((Func<bool>) (() => this.myCommands16Bit.SetEmergencyMode(this.progress, this.cancelToken)));
        while (!task.IsCompleted)
        {
          Thread.Sleep(100);
          Application.DoEvents();
        }
        if (task.Exception != null)
          throw task.Exception;
        this.progress.Report("MessageEnd", (object) GMM_EventArgs.MessageType.EndMessage);
        return task.Result;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "SetEmergencyMode error");
        return false;
      }
    }

    public override bool EraseFlash(int StartAddress, int NumberOfBytes)
    {
      try
      {
        S3_CommandsCOMPATIBLE.S3_CommandsCOMPATIBLE_Logger.Debug("EraseFlash at adress 0x" + StartAddress.ToString("x4") + " size 0x" + NumberOfBytes.ToString("x2"));
        this.myCommands16Bit.EraseFLASH(this.progress, this.cancelToken, ushort.Parse(StartAddress.ToString()), NumberOfBytes);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "EraseFlash error");
        return false;
      }
      return true;
    }

    public override bool Open()
    {
      try
      {
        if (!this.myPort.IsOpen)
          this.myPort.Open();
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Com port open error");
        return false;
      }
    }

    public override bool Close()
    {
      try
      {
        if (this.myPort.IsOpen)
          this.myPort.Close();
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Port close error");
        return false;
      }
    }

    public override bool DeviceProtectionGet()
    {
      try
      {
        return this.myCommands16Bit.DeviceProtectionGet(this.progress, this.cancelToken);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "DeviceProtectionGet error");
        return false;
      }
    }

    public override bool DeviceProtectionSet()
    {
      int timeBeforFirstByte = this.configList.RecTime_BeforFirstByte;
      try
      {
        this.configList.RecTime_BeforFirstByte = 500;
        return this.myCommands16Bit.DeviceProtectionSet(this.progress, this.cancelToken);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "DeviceProtectionSet error");
        return false;
      }
      finally
      {
        this.configList.RecTime_BeforFirstByte = timeBeforFirstByte;
      }
    }

    public override bool DeviceProtectionReset(uint meterKey)
    {
      try
      {
        return this.myCommands16Bit.DeviceProtectionReset(this.progress, this.cancelToken, meterKey);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "DeviceProtectionReset error");
        return false;
      }
    }

    public override bool DeviceProtectionSetKey(uint meterKey)
    {
      try
      {
        return this.myCommands16Bit.DeviceProtectionSetKey(this.progress, this.cancelToken, meterKey);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "DeviceProtectionSetKey error");
        return false;
      }
    }

    public override bool FlyingTestActivate()
    {
      try
      {
        return this.myCommands16Bit.FlyingTestActivate(this.progress, this.cancelToken);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "FlyingTestActivate error");
        return false;
      }
    }

    public override bool FlyingTestStart()
    {
      try
      {
        this.myCommands16Bit.FlyingTestStart(this.progress, this.cancelToken);
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "FlyingTestStart error");
        return false;
      }
    }

    public override bool FlyingTestStop()
    {
      try
      {
        this.myCommands16Bit.FlyingTestStop(this.progress, this.cancelToken);
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "FlyingTestStop error");
        return false;
      }
    }

    public override bool FlyingTestReadVolume(out float volume, out MBusDeviceState state)
    {
      volume = 0.0f;
      state = MBusDeviceState.AnyError;
      try
      {
        byte state1;
        this.myCommands16Bit.FlyingTestReadVolume(this.progress, this.cancelToken, out volume, out state1);
        state = (MBusDeviceState) state1;
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "FlyingTestReadVolume error");
        return false;
      }
    }

    public override bool AdcTestActivate()
    {
      try
      {
        return this.myCommands16Bit.AdcTestActivate(this.progress, this.cancelToken);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "AdcTestActivate error");
        return false;
      }
    }

    public override bool AdcTestCycleWithSimulatedVolume(float simulationVolume)
    {
      try
      {
        return this.myCommands16Bit.AdcTestCycleWithSimulatedVolume(this.progress, this.cancelToken, simulationVolume);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "AdcTestCycleWithSimulatedVolume error");
        return false;
      }
    }

    public override bool CapacityOfTestActivate()
    {
      try
      {
        return this.myCommands16Bit.CapacityOfTestActivate(this.progress, this.cancelToken);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "CapacityOfTestActivate error");
        return false;
      }
    }

    public override bool Start512HzRtcCalibration()
    {
      try
      {
        return this.myCommands16Bit.Start512HzRtcCalibration(this.progress, this.cancelToken);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Start512HzRtcCalibration error");
        return false;
      }
    }

    public override bool TestDone(long dispValueId)
    {
      try
      {
        return this.myCommands16Bit.TestDone(this.progress, this.cancelToken, dispValueId);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "TestDone error");
        return false;
      }
    }

    public override bool GetMeterMonitorData(out ByteField MonitorData)
    {
      int timeBeforFirstByte = this.configList.RecTime_BeforFirstByte;
      MonitorData = new ByteField(504);
      try
      {
        this.configList.RecTime_BeforFirstByte = 4000;
        byte[] meterMonitorData = this.myCommands16Bit.GetMeterMonitorData(this.progress, this.cancelToken);
        MonitorData.Add(meterMonitorData);
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "GetMeterMonitorData error");
        return false;
      }
      finally
      {
        this.configList.RecTime_BeforFirstByte = timeBeforFirstByte;
      }
    }

    public override ImpulseInputCounters ReadInputCounters()
    {
      int timeBeforFirstByte = this.configList.RecTime_BeforFirstByte;
      try
      {
        cImpulseInputCounters impulseInputCounters = this.myCommands16Bit.ReadInputCounters(this.progress, this.cancelToken);
        return new ImpulseInputCounters()
        {
          HardwareCounter = impulseInputCounters.HardwareCounter,
          ImputState = impulseInputCounters.ImputState,
          Input0Counter = impulseInputCounters.Input0Counter,
          Input1Counter = impulseInputCounters.Input1Counter,
          Input2Counter = impulseInputCounters.Input2Counter,
          VolumePulseCounter = impulseInputCounters.VolumePulseCounter
        };
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "ReadInputCounters error");
        return (ImpulseInputCounters) null;
      }
      finally
      {
        this.configList.RecTime_BeforFirstByte = timeBeforFirstByte;
      }
    }

    public override bool RadioTestActivate(RadioTestMode testMode)
    {
      try
      {
        this.myCommands16Bit.RadioTest(this.progress, this.cancelToken, (byte) testMode);
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "RadioTestActivate error");
        return false;
      }
    }

    public override byte[] RunIoTest(IoTestFunctions theFunction)
    {
      int timeBeforFirstByte = this.configList.RecTime_BeforFirstByte;
      try
      {
        if (theFunction == IoTestFunctions.IoTest_Run)
          this.configList.RecTime_BeforFirstByte = 500;
        return this.myCommands16Bit.RunIoTest(this.progress, this.cancelToken, (eIoTestFunctions) theFunction);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "RunIoTest error");
        return (byte[]) null;
      }
      finally
      {
        this.configList.RecTime_BeforFirstByte = timeBeforFirstByte;
      }
    }

    public override bool DigitalInputsAndOutputs(
      uint NewOutputMask,
      uint NewOutputState,
      ref uint OldOutputState,
      ref uint OldInputState)
    {
      try
      {
        this.myCommands16Bit.DigitalInputsAndOutputs(this.progress, this.cancelToken, NewOutputMask, NewOutputState, ref OldOutputState, ref OldInputState);
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "DigitalInputsAndOutputs error");
        return false;
      }
    }

    public override bool SetOptoTimeoutSeconds(int Seconds)
    {
      try
      {
        return this.myCommands16Bit.SetOptoTimeoutSeconds(this.progress, this.cancelToken, Seconds);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "SetOptoTimeoutSeconds error");
        return false;
      }
    }

    public override bool TransmitBlock(byte[] buffer)
    {
      try
      {
        ByteField buffer1 = new ByteField(buffer);
        return this.TransmitBlock(ref buffer1);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "TransmitBlock error");
        return false;
      }
    }

    public override bool TransmitBlock(ref ByteField DataBlock)
    {
      try
      {
        byte[] buffer = new byte[DataBlock.Count];
        Buffer.BlockCopy((Array) DataBlock.Data, 0, (Array) buffer, 0, DataBlock.Data.Length);
        bool flag = this.myCommands16Bit.TransmitBlock(this.progress, this.cancelToken, ref buffer);
        DataBlock.Data = buffer;
        return flag;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "TransmitBlock error");
        return false;
      }
    }

    public override bool SendBlock(ref ByteField DataBlock)
    {
      try
      {
        byte[] buffer = new byte[DataBlock.Count];
        Buffer.BlockCopy((Array) DataBlock.Data, 0, (Array) buffer, 0, DataBlock.Data.Length);
        bool flag = this.myCommands16Bit.SendBlock(this.progress, this.cancelToken, ref buffer);
        DataBlock.Data = buffer;
        return flag;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "SendBlock error");
        return false;
      }
    }

    public override bool ReceiveBlock(ref ByteField DataBlock, int MinByteNb, bool first)
    {
      try
      {
        byte[] block = this.myCommands16Bit.ReceiveBlock(this.progress, this.cancelToken, MinByteNb, first);
        DataBlock.Add(block);
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "ReceiveBlock error");
        return false;
      }
    }

    public override bool ReceiveBlock(ref ByteField DataBlock)
    {
      try
      {
        return this.ReceiveBlock(ref DataBlock, 1, true);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "ReceiveBlock error");
        return false;
      }
    }

    public ParameterListInfo GetTransmitList()
    {
      return this.myCommands16Bit.GetTransmitListInfo(this.progress, this.cancelToken);
    }

    public bool SetTransmitList(
      ushort list,
      bool isradio,
      ushort enc_mode,
      out ByteField DataBlock)
    {
      DataBlock = new ByteField();
      try
      {
        DataBlock.Data = this.myCommands16Bit.SetTransmitList(this.progress, this.cancelToken, list, isradio, enc_mode);
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "SetTransmitList error");
        return false;
      }
    }

    public bool SetTransmitList(
      ushort list,
      bool isradio,
      ushort enc_mode,
      ushort radio_cycletime,
      out ByteField DataBlock)
    {
      DataBlock = new ByteField();
      try
      {
        DataBlock.Data = this.myCommands16Bit.SetTransmitList(this.progress, this.cancelToken, list, isradio, enc_mode, radio_cycletime);
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "SetTransmitList error");
        return false;
      }
    }

    public async Task SetRadioParametersAsync(
      byte listNumber,
      RADIO_MODE radioMode,
      AES_ENCRYPTION_MODE AES_Encryption,
      ushort intervallSeconds)
    {
      await this.myCommands16Bit.SetRadioParametersAsync(this.progress, this.cancelToken, listNumber, radioMode, AES_Encryption, intervallSeconds);
    }
  }
}
