// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_CommandsCHANGED
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using AsyncCom;
using CommunicationPort.UserInterface;
using DeviceCollector;
using HandlerLib;
using NLog;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public sealed class S3_CommandsCHANGED : S3_CommandsCOMPATIBLE
  {
    internal static Logger S4_HandlerFunctionsLogger = LogManager.GetLogger("S3_HandlerFunctions");
    private bool disableBusWriteOnDispose = false;

    public S3_CommandsCHANGED(
      ProgressHandler mainProgress,
      CancellationToken mainCancel,
      CommunicationPortWindowFunctions myPortWinFunctions)
      : base(mainProgress, mainCancel, myPortWinFunctions)
    {
      this.progress = mainProgress;
      this.cancelToken = mainCancel;
      this.myPortWinFunction = myPortWinFunctions;
      this.myPort = myPortWinFunctions.portFunctions;
      this.configList = this.myPort.GetReadoutConfiguration();
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

    public override bool ReadVersion(
      out short Connected_Manufacturer,
      out byte Connected_Medium,
      out byte Connected_MBusMeterType,
      out long Connected_Version,
      out int Connected_MBusSerialNr,
      out int Connected_ConfigAdr,
      out int Connected_HardwareMask)
    {
      Connected_Manufacturer = short.Parse("0");
      Connected_Medium = byte.Parse("0");
      Connected_MBusMeterType = byte.Parse("0");
      Connected_Version = long.Parse("0");
      Connected_MBusSerialNr = 0;
      Connected_ConfigAdr = 0;
      Connected_HardwareMask = 0;
      try
      {
        this.myDeviceIdentification = (DeviceIdentification) this.myDeviceCommandsMBus.ReadVersion(this.progress, this.cancelToken);
        Connected_Manufacturer = short.Parse("0");
        Connected_Medium = byte.Parse("0");
        Connected_MBusMeterType = byte.Parse("0");
        Connected_Version = long.Parse("0");
        Connected_MBusSerialNr = 0;
        Connected_ConfigAdr = 0;
        Connected_HardwareMask = 0;
      }
      catch
      {
        return false;
      }
      return true;
    }

    private void readDeviceIdentification()
    {
      this.myDeviceIdentification = (DeviceIdentification) null;
      this.myDeviceIdentification = (DeviceIdentification) this.myDeviceCommandsMBus.ReadVersion(this.progress, this.cancelToken);
    }

    public override bool ReadVersion()
    {
      this.progress.Reset();
      this.myDeviceIdentification = (DeviceIdentification) null;
      this.myDeviceIdentification = (DeviceIdentification) this.myDeviceCommandsMBus.ReadVersion(this.progress, this.cancelToken);
      byte[] numArray = new byte[240];
      numArray = this.myCommands32Bit.ReadMemory(this.progress, this.cancelToken, 16384U, 240U, (byte) 64);
      return true;
    }

    public override DeviceIdentification GetDeviceIdentification()
    {
      if (this.myDeviceIdentification == null)
        this.readDeviceIdentification();
      return this.myDeviceIdentification;
    }

    public override bool IsSelectedDevice(DeviceTypes TestType)
    {
      if (this.myDeviceIdentification == null)
        this.readDeviceIdentification();
      uint? hardwareTypeId = this.myDeviceIdentification.HardwareTypeID;
      uint num = 466;
      return hardwareTypeId.GetValueOrDefault() > num & hardwareTypeId.HasValue;
    }

    public override BusDevice GetSelectedDevice() => (BusDevice) null;

    public override ZR_ClassLibrary.BusMode GetBaseMode()
    {
      return (ZR_ClassLibrary.BusMode) Enum.Parse(typeof (ZR_ClassLibrary.BusMode), this.configList.BusMode, true);
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

    public override bool SetEmergencyMode() => true;

    public override bool RunBackup()
    {
      try
      {
        AsyncHelpers.RunSync((Func<Task>) (async () => await this.myCommands32Bit.BackupDeviceAsync(this.progress, this.cancelToken)));
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        return false;
      }
      return true;
    }

    public override bool ResetDevice()
    {
      try
      {
        AsyncHelpers.RunSync((Func<Task>) (async () => await this.myCommands32Bit.ResetDeviceAsync(this.progress, this.cancelToken)));
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        return false;
      }
      return true;
    }

    public override bool EraseFlash(int StartAddress, int NumberOfBytes)
    {
      try
      {
        ushort.Parse(StartAddress.ToString());
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        return false;
      }
      return true;
    }
  }
}
