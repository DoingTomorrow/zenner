// Decompiled with JetBrains decompiler
// Type: M8_Handler.UserInterface.M8_HandlerWindowFunctions
// Assembly: M8_Handler, Version=2.0.6.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 582F1296-F274-42DF-B72B-4C0B4D92AA72
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\M8_Handler.dll

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
namespace M8_Handler.UserInterface
{
  public class M8_HandlerWindowFunctions : IWindowFunctions
  {
    internal bool IsPluginObject = false;
    public M8_HandlerFunctions MyFunctions;
    internal CommunicationPortWindowFunctions myPort;
    internal M8_HandlerWindow MyHandlerWindow;
    private static Logger M8_HandlerLogger = LogManager.GetLogger(nameof (M8_HandlerWindowFunctions));

    public M8_HandlerWindowFunctions()
    {
      this.myPort = new CommunicationPortWindowFunctions();
      this.MyFunctions = new M8_HandlerFunctions(this.myPort.portFunctions, DbBasis.PrimaryDB.BaseDbConnection);
    }

    public M8_HandlerWindowFunctions(CommunicationPortWindowFunctions myPort, BaseDbConnection myDb)
    {
      this.myPort = myPort;
      this.MyFunctions = new M8_HandlerFunctions(myPort.portFunctions, myDb);
    }

    public M8_HandlerWindowFunctions(CommunicationPortWindowFunctions myPort)
    {
      this.myPort = myPort;
      this.MyFunctions = new M8_HandlerFunctions(myPort.portFunctions);
    }

    public M8_HandlerWindowFunctions(BaseDbConnection myDb)
    {
      this.MyFunctions = new M8_HandlerFunctions(myDb);
    }

    public object GetFunctions() => (object) this.MyFunctions;

    public Version GetActualPluginVersion()
    {
      return AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location).Version;
    }

    public string ShowMainWindow() => this.ShowMainWindow(false);

    public string ShowMainWindow(bool isPluginCall)
    {
      this.MyHandlerWindow = new M8_HandlerWindow(this, this.IsPluginObject & isPluginCall);
      this.MyHandlerWindow.ShowDialog();
      return this.MyHandlerWindow.NextPlugin;
    }

    public void DisposeComponent() => this.MyFunctions.Dispose();

    internal void OpenMapClassManagerToReadFromMapFile()
    {
      List<AddressRange> addressRanges = new List<AddressRange>();
      addressRanges.Add(new AddressRange(536870912U, 20479U));
      addressRanges.Add(new AddressRange(134217728U, 196607U));
      addressRanges.Add(new AddressRange(134742016U, 6143U));
      string directoryName = Path.GetDirectoryName(new StackTrace(true).GetFrame(1).GetFileName());
      new MapClassManager("M8_Handler", addressRanges: addressRanges, pathToMapClasses: directoryName).ShowDialog();
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
        byte[] zippedBuffer = BackupWindow.ShowDialog((Window) this.MyHandlerWindow, (ICreateMeter) this.MyFunctions.myMeters, new FirmwareType[2]
        {
          FirmwareType.HCA_LoRa,
          FirmwareType.M7plus
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
