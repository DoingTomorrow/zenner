// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_HandlerWindowFunctions
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommunicationPort.UserInterface;
using GmmDbLib;
using HandlerLib;
using HandlerLib.MapManagement;
using NLog;
using S4_Handler.Functions;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.UserInterface
{
  public class S4_HandlerWindowFunctions : IReadoutConfig, IWindowFunctions
  {
    internal const string ConfigNameSpace = "S4_Handler";
    private bool isPluginObject = false;
    public S4_HandlerFunctions MyFunctions;
    internal CommunicationPortWindowFunctions myPort;
    internal S4_HandlerWindow MyHandlerWindow;
    private static Logger S4_HandlerLogger = LogManager.GetLogger("S4_Handler");

    internal bool IsPluginObject
    {
      set
      {
        this.isPluginObject = value;
        if (!value)
          return;
        this.InitPlugin();
      }
      get => this.isPluginObject;
    }

    public S4_HandlerWindowFunctions()
    {
      this.myPort = new CommunicationPortWindowFunctions();
      this.MyFunctions = new S4_HandlerFunctions(this.myPort.portFunctions, DbBasis.PrimaryDB.BaseDbConnection);
    }

    public S4_HandlerWindowFunctions(BaseDbConnection myDb)
    {
      this.MyFunctions = new S4_HandlerFunctions(myDb);
    }

    public S4_HandlerWindowFunctions(CommunicationPortWindowFunctions myPort)
    {
      this.myPort = myPort;
      this.MyFunctions = new S4_HandlerFunctions(myPort.portFunctions);
    }

    public S4_HandlerWindowFunctions(CommunicationPortWindowFunctions myPort, BaseDbConnection myDb)
    {
      this.myPort = myPort;
      this.MyFunctions = new S4_HandlerFunctions(myPort.portFunctions, myDb);
    }

    public S4_HandlerWindowFunctions(
      CommunicationPortWindowFunctions myPort,
      BaseDbConnection myDb,
      Common32BitCommands IrDaCommands)
    {
      this.myPort = myPort;
      this.MyFunctions = new S4_HandlerFunctions(myPort.portFunctions, myDb, IrDaCommands);
    }

    private void InitPlugin()
    {
      if (!this.IsPluginObject)
        return;
      try
      {
      }
      catch
      {
      }
    }

    public Version GetActualPluginVersion()
    {
      return AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location).Version;
    }

    public object GetFunctions() => (object) this.MyFunctions;

    public void SetReadoutConfiguration(ConfigList configList)
    {
      this.MyFunctions.SetReadoutConfiguration(configList);
    }

    public ConfigList GetReadoutConfiguration() => this.MyFunctions.GetReadoutConfiguration();

    public string ShowMainWindow() => this.ShowMainWindow(false);

    public string ShowMainWindow(bool isPluginCall, Window owner = null)
    {
      this.MyHandlerWindow = new S4_HandlerWindow(this, this.IsPluginObject & isPluginCall);
      if (owner != null)
        this.MyHandlerWindow.Owner = owner;
      this.MyHandlerWindow.ShowDialog();
      return this.MyHandlerWindow.NextPlugin;
    }

    public void ShowRadioTestWindow(Window ownerWindow, string radioMiConComPort)
    {
      if (this.MyFunctions.myMeters.ConnectedMeter == null)
        throw new Exception("ShowRadioTestWindow requires an connected device.");
      S4_RadioTestWindow s4RadioTestWindow = new S4_RadioTestWindow(this, this.myPort.GetReadoutConfiguration().Port, this.MyFunctions.myMeters.ConnectedMeter.deviceIdentification.MeterID.Value, radioMiConComPort);
      s4RadioTestWindow.Owner = ownerWindow;
      s4RadioTestWindow.ShowDialog();
    }

    public void DisposeComponent() => this.MyFunctions.Dispose();

    internal void OpenMapClassManagerToReadFromMapFile()
    {
      List<AddressRange> addressRanges = new List<AddressRange>();
      addressRanges.Add(new AddressRange(536870912U, 20479U));
      addressRanges.Add(new AddressRange(134217728U, 196607U));
      addressRanges.Add(new AddressRange(134742016U, 6143U));
      List<string> requiredSections = new List<string>();
      requiredSections.Add("BACKUP_0");
      requiredSections.Add("BACKUP_1");
      requiredSections.Add("SMART_FUNC_RA");
      requiredSections.Add("SMART_FUNC_BA");
      requiredSections.Add("CSTACK");
      string directoryName = Path.GetDirectoryName(new StackTrace(true).GetFrame(1).GetFileName());
      new MapClassManager("S4_Handler", addressRanges: addressRanges, pathToMapClasses: directoryName, requiredSections: requiredSections).ShowDialog();
    }

    internal string ShowBackUpWindow(bool useSecondaryDB)
    {
      try
      {
        byte[] zippedBuffer = BackupWindow.ShowDialog((Window) this.MyHandlerWindow, (ICreateMeter) this.MyFunctions.myMeters, FirmwareType.IUW, true, useSecondaryDB);
        if (zippedBuffer != null)
          return this.MyFunctions.myMeters.SetBackupMeter(zippedBuffer);
      }
      catch (Exception ex)
      {
        return "Error while loading Backup\n" + ex.Message;
      }
      return string.Empty;
    }

    internal void ShowTestConnectionWindow()
    {
      TestCommunicationWindow.Show(this.myPort, (BaseMemoryAccess) this.MyFunctions.myDeviceCommands);
    }

    internal enum ConfigVariables
    {
      TestWindowTabControlSelect,
      NFC_BlockMode,
      ReleaseDocumentFolder,
      Scenario,
      ScenarioInternal,
      ScenarioModule,
      ScenarioNumber,
      ModuleCode,
      ReadPartSelection,
      RadioMinoConnectComPort,
      RadioTestDevice,
      RadioCycleTime,
      RadioTimeoutTime,
      RadioSyncWord,
      RadioTestFrequencyHz,
      MapCheckDisabled,
      SmartFunctionsGroup,
      TimeLapseShowLoRaAlarms,
      TimeLapseShowEvnetLogger,
    }
  }
}
