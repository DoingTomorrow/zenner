// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.UserInterface.EDCL_HandlerWindowFunctions
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using CommunicationPort.UserInterface;
using GmmDbLib;
using HandlerLib;
using HandlerLib.MapManagement;
using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using ZENNER.CommonLibrary;

#nullable disable
namespace EDCL_Handler.UserInterface
{
  public class EDCL_HandlerWindowFunctions : IWindowFunctions
  {
    internal bool IsPluginObject = false;
    public EDCL_HandlerFunctions MyFunctions;
    internal CommunicationPortWindowFunctions myPort;
    internal EDCL_HandlerWindow MyHandlerWindow;
    private static Logger EDCL_HandlerLogger = LogManager.GetLogger(nameof (EDCL_HandlerWindowFunctions));

    public EDCL_HandlerWindowFunctions(
      CommunicationPortWindowFunctions myPort,
      BaseDbConnection myDb)
    {
      this.myPort = myPort;
      this.MyFunctions = new EDCL_HandlerFunctions(myPort.portFunctions, myDb);
    }

    public EDCL_HandlerWindowFunctions(CommunicationPortWindowFunctions myPort)
    {
      this.myPort = myPort;
      this.MyFunctions = new EDCL_HandlerFunctions(myPort.portFunctions);
    }

    public EDCL_HandlerWindowFunctions(BaseDbConnection myDb)
    {
      this.MyFunctions = new EDCL_HandlerFunctions(myDb);
    }

    public Version GetActualPluginVersion()
    {
      return AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location).Version;
    }

    public string ShowMainWindow() => this.ShowMainWindow(false);

    public string ShowMainWindow(bool isPluginCall)
    {
      this.MyHandlerWindow = new EDCL_HandlerWindow(this, this.IsPluginObject & isPluginCall);
      this.MyHandlerWindow.ShowDialog();
      return this.MyHandlerWindow.NextPlugin;
    }

    public void DisposeComponent() => this.MyFunctions.Dispose();

    public object GetFunctions() => (object) this.MyFunctions;

    internal void OpenMapClassManagerToReadFromMapFile()
    {
      List<AddressRange> addressRanges = new List<AddressRange>();
      addressRanges.Add(new AddressRange(536870912U, 20479U));
      addressRanges.Add(new AddressRange(134217728U, 196607U));
      addressRanges.Add(new AddressRange(134742016U, 6143U));
      string directoryName = Path.GetDirectoryName(new StackTrace(true).GetFrame(1).GetFileName());
      new MapClassManager("EDCL_Handler", addressRanges: addressRanges, pathToMapClasses: directoryName).ShowDialog();
    }

    internal void ShowParameterForDevice()
    {
      if (this.MyFunctions.myMeters.WorkMeter == null || this.MyFunctions.myMeters.WorkMeter.meterMemory == null)
        throw new Exception("Please connect to a device first.");
      new ParameterWindow((DeviceMemory) this.MyFunctions.myMeters.WorkMeter.meterMemory, (BaseMemoryAccess) this.MyFunctions.cmd.Device, Assembly.GetExecutingAssembly()).ShowDialog();
    }

    internal void ShowMemoryViewWindow()
    {
      if (this.MyFunctions.myMeters.WorkMeter == null)
        throw new Exception("Please connect to a device first.");
      new MemoryViewer((DeviceMemory) this.MyFunctions.myMeters.WorkMeter.meterMemory, (BaseMemoryAccess) this.MyFunctions.cmd.Device).ShowDialog();
    }

    internal string ShowBackUpWindow()
    {
      try
      {
        byte[] zippedBuffer = BackupWindow.ShowDialog((Window) this.MyHandlerWindow, (ICreateMeter) this.MyFunctions.myMeters, new FirmwareType[19]
        {
          FirmwareType.micro_LoRa,
          FirmwareType.micro_wMBus,
          FirmwareType.EDC_LoRa,
          FirmwareType.EDC_wMBus_ST,
          FirmwareType.EDC_LoRa_915MHz,
          FirmwareType.EDC_LoRa_470Mhz,
          FirmwareType.micro_LoRa_LL,
          FirmwareType.micro_wMBus_LL,
          FirmwareType.micro_LL_radio3,
          FirmwareType.EDC_NBIoT,
          FirmwareType.EDC_NBIoT_LCSW,
          FirmwareType.EDC_NBIoT_YJSW,
          FirmwareType.EDC_NBIoT_FSNH,
          FirmwareType.EDC_NBIoT_XM,
          FirmwareType.EDC_NBIoT_Israel,
          FirmwareType.EDC_NBIoT_TaiWan,
          FirmwareType.EDC_LoRa_868_v3,
          FirmwareType.EDC_LoRa_915_v2_US,
          FirmwareType.EDC_LoRa_915_v2_BR
        }, true);
        if (zippedBuffer != null)
          return this.MyFunctions.myMeters.SetBackupMeter(zippedBuffer);
      }
      catch (Exception ex)
      {
        return "Error while loading Backup\n" + ex.Message;
      }
      return string.Empty;
    }

    public void ShowProtectionWindow()
    {
      ProtectionWindow.Show(this.MyHandlerWindow.Owner, (DeviceCommandsMBus) this.MyFunctions.cmd, this.MyFunctions.cmd.Device);
    }
  }
}
