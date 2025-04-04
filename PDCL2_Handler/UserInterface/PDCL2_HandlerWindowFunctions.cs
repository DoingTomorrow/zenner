// Decompiled with JetBrains decompiler
// Type: PDCL2_Handler.UserInterface.PDCL2_HandlerWindowFunctions
// Assembly: PDCL2_Handler, Version=2.22.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 03BA4C2D-69FE-4DA6-9C3F-B3D5471C4058
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDCL2_Handler.dll

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
namespace PDCL2_Handler.UserInterface
{
  public class PDCL2_HandlerWindowFunctions : IWindowFunctions
  {
    internal bool IsPluginObject = false;
    public PDCL2_HandlerFunctions MyFunctions;
    internal CommunicationPortWindowFunctions myPort;
    internal PDCL2_HandlerWindow MyHandlerWindow;
    private static Logger PDCL2_HandlerLogger = LogManager.GetLogger(nameof (PDCL2_HandlerWindowFunctions));

    public PDCL2_HandlerWindowFunctions(
      CommunicationPortWindowFunctions myPort,
      BaseDbConnection myDb)
    {
      this.myPort = myPort;
      this.MyFunctions = new PDCL2_HandlerFunctions(myPort.portFunctions, myDb);
    }

    public PDCL2_HandlerWindowFunctions(CommunicationPortWindowFunctions myPort)
    {
      this.myPort = myPort;
      this.MyFunctions = new PDCL2_HandlerFunctions(myPort.portFunctions);
    }

    public PDCL2_HandlerWindowFunctions(BaseDbConnection myDb)
    {
      this.MyFunctions = new PDCL2_HandlerFunctions(myDb);
    }

    public Version GetActualPluginVersion()
    {
      return AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location).Version;
    }

    public string ShowMainWindow() => this.ShowMainWindow(false);

    public string ShowMainWindow(bool isPluginCall)
    {
      this.MyHandlerWindow = new PDCL2_HandlerWindow(this, this.IsPluginObject & isPluginCall);
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
      new MapClassManager("PDCL2_Handler", addressRanges: addressRanges, pathToMapClasses: directoryName).ShowDialog();
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
        byte[] zippedBuffer = BackupWindow.ShowDialog((Window) this.MyHandlerWindow, (ICreateMeter) this.MyFunctions.myMeters, new FirmwareType[4]
        {
          FirmwareType.PDC_LoRa,
          FirmwareType.UDC_LoRa_915,
          FirmwareType.PDC_LoRa_868MHz_SD,
          FirmwareType.PDC_LoRa_915
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
