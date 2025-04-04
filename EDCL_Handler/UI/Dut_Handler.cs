// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.UI.Dut_Handler
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using CommunicationPort.Functions;
using HandlerLib;
using ReadoutConfiguration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ZENNER.CommonLibrary;

#nullable disable
namespace EDCL_Handler.UI
{
  internal class Dut_Handler
  {
    private static ConfigList DefaultConfigList;
    public static RefreshDGBack RefreshDGInvoke;
    private bool isrunning = false;
    private object isrunningLocker = new object();
    private bool ishide = false;
    private object ishideLocker = new object();
    private int Phase = 0;
    private string[] PhaseList = new string[14]
    {
      string.Empty,
      "Read Device",
      "Backup Device",
      "Load BootLoader",
      "Write BootLoader",
      "Verify BootLoader",
      "Start BootLoader",
      "Load Firmware",
      "Delete Old Firmware",
      "Wirte New Firmware",
      "Verify New Firmware",
      "Start Firmware",
      "Overwrite From Backup",
      "Do VCC2 Calibration"
    };
    private ProgressHandler ProgressDummy = new ProgressHandler((Action<ProgressArg>) (x => { }));
    private CancellationTokenSource CancelSourceDummy = new CancellationTokenSource();
    private CancellationToken TokenDummy;
    public bool Initial = false;
    private static object InitialLocker = new object();

    public static FWConfig Config { set; get; }

    public static void SetConfig(string ConfigFileName)
    {
      Dut_Handler.Config = new FWConfig(ConfigFileName);
      Dut_Handler.DefaultConfigList = ReadoutPreferences.GetConfigListFromProfileId(Dut_Handler.Config.ConnectionProfileID);
    }

    public string Index { set; get; }

    public CommunicationPortFunctions ComPort { set; get; }

    private FirmwareUpdateFunctions FWUpdateFunction { set; get; }

    private EDCL_HandlerFunctions Handler { set; get; }

    public bool Failed { set; get; }

    public bool Success { set; get; }

    public bool IsRunning
    {
      set
      {
        lock (this.isrunningLocker)
          this.isrunning = value;
      }
      get
      {
        lock (this.isrunningLocker)
          return this.isrunning;
      }
    }

    public bool IsHide
    {
      set
      {
        lock (this.ishideLocker)
          this.ishide = value;
      }
      get
      {
        lock (this.ishideLocker)
          return this.ishide;
      }
    }

    public string Hide => this.IsHide ? nameof (Hide) : "Show";

    public string State
    {
      get
      {
        if (this.Failed)
          return "Fail";
        return this.Success ? "Success" : (this.IsRunning ? "Running" : "Ready");
      }
    }

    public string FailReason { set; get; }

    public string Progress => this.Phase.ToString() + "/13";

    public string COM { set; get; }

    public string FirmwareVersion { set; get; }

    private Dispatcher MainDispatcher { set; get; }

    public int MeterID { set; get; }

    public string PhaseString => this.PhaseList[this.Phase];

    private int FW_MapID { set; get; }

    private int BL_MapID { set; get; }

    private string FW_Data { set; get; }

    private string BL_Data { set; get; }

    public Dut_Handler(string PortName)
      : this(PortName, Dispatcher.CurrentDispatcher)
    {
    }

    public Dut_Handler(string PortName, Dispatcher mainDispatcher)
    {
      this.COM = PortName;
      this.MainDispatcher = mainDispatcher;
      this.TokenDummy = this.CancelSourceDummy.Token;
      this.Initial = false;
      this.IsHide = false;
    }

    private static void InitialDut(Dut_Handler dut)
    {
      lock (Dut_Handler.InitialLocker)
      {
        ConfigList configList = new ConfigList(Dut_Handler.DefaultConfigList.GetSortedList());
        configList.Port = dut.COM;
        dut.ComPort = new CommunicationPortFunctions();
        dut.ComPort.SetReadoutConfiguration(configList);
        dut.Handler = new EDCL_HandlerFunctions(dut.ComPort);
        dut.FWUpdateFunction = new FirmwareUpdateFunctions(dut.ComPort, (FirmwareUpdateToolDeviceCommands) new FirmwareUpdateToolDeviceCommands_MBus(dut.ComPort));
        dut.FWUpdateFunction.myHandlerForProduction = (HandlerFunctionsForProduction) dut.Handler;
        dut.FW_MapID = Dut_Handler.Config.FW_MapID;
        dut.BL_MapID = Dut_Handler.Config.BL_MapID;
        dut.FW_Data = Dut_Handler.Config.FW_Data;
        dut.BL_Data = Dut_Handler.Config.BL_Data;
        dut.Initial = true;
      }
    }

    public void ClearState()
    {
      this.Success = false;
      this.Failed = false;
      this.FirmwareVersion = string.Empty;
      this.MeterID = 0;
      this.Phase = 0;
      if (this.ComPort != null)
        this.ComPort.Close();
      if (this.FWUpdateFunction != null)
        this.FWUpdateFunction.clearVariables();
      this.FailReason = string.Empty;
    }

    public void Run()
    {
      if (this.IsHide)
        return;
      this.ClearState();
      new Thread(new ThreadStart(this.WorkThread))
      {
        IsBackground = true
      }.Start();
    }

    public void Read()
    {
      if (this.IsHide)
        return;
      this.ClearState();
      new Thread(new ThreadStart(this.ReadThread))
      {
        IsBackground = true
      }.Start();
    }

    private async void ReadThread()
    {
      this.IsRunning = true;
      this.RefreshView();
      if (!this.Initial)
        Dut_Handler.InitialDut(this);
      DeviceIdentification di = (DeviceIdentification) null;
      try
      {
        di = await this.FWUpdateFunction.ReadVersionAsync(this.ProgressDummy, this.TokenDummy);
        this.FirmwareVersion = di.FirmwareVersionObj.ToString();
      }
      catch (object ex)
      {
        try
        {
          int version = (int) await this.FWUpdateFunction.BSL_getVersion(this.ProgressDummy, this.CancelSourceDummy);
          this.FirmwareVersion = "BootLoader Mode";
        }
        catch
        {
          this.Failed = true;
          this.FailReason = "Read Device Failed";
        }
      }
      this.IsRunning = false;
      this.ComPort.Close();
      this.RefreshView();
      di = (DeviceIdentification) null;
    }

    private async void WorkThread()
    {
      this.IsRunning = true;
      if (!this.Initial)
        Dut_Handler.InitialDut(this);
      ++this.Phase;
      this.RefreshView();
      DeviceIdentification di = (DeviceIdentification) null;
      bool NeedBackup = true;
      bool InBootLoader = false;
      bool ReadOK = false;
      ZENNER.CommonLibrary.FirmwareVersion firmwareVersionObj;
      for (int i = 0; i < 3; ++i)
      {
        try
        {
          di = await this.FWUpdateFunction.ReadVersionAsync(this.ProgressDummy, this.TokenDummy);
        }
        catch (object ex)
        {
          NeedBackup = false;
          try
          {
            int version = (int) await this.FWUpdateFunction.BSL_getVersion(this.ProgressDummy, this.CancelSourceDummy);
          }
          catch
          {
            Thread.Sleep(2000);
            continue;
          }
          InBootLoader = true;
        }
        if (!InBootLoader)
        {
          Thread.Sleep(2000);
          try
          {
            int num = await this.Handler.ReadDeviceAsync(this.ProgressDummy, this.TokenDummy, ReadPartsSelection.AllWithoutLogger);
            di = this.Handler.GetDeviceIdentification(0);
          }
          catch
          {
            Thread.Sleep(2000);
            continue;
          }
          firmwareVersionObj = di.FirmwareVersionObj;
          this.FirmwareVersion = firmwareVersionObj.ToString();
          uint? meterId = di.MeterID;
          if (meterId.HasValue)
          {
            meterId = di.MeterID;
            this.MeterID = (int) meterId.Value;
          }
          if (this.MeterID <= 0)
          {
            uint? nullable = await this.FWUpdateFunction.getMeterIDFromARM_ID(this.ProgressDummy, new CancellationTokenSource(), di.Unique_ID);
            this.MeterID = (int) nullable.Value;
            nullable = new uint?();
            NeedBackup = false;
          }
          this.RefreshView();
        }
        ReadOK = true;
        break;
      }
      if (!ReadOK)
      {
        this.EndWork(true, "Device Not Working");
        di = (DeviceIdentification) null;
      }
      else
      {
        ++this.Phase;
        this.RefreshView();
        if (NeedBackup)
        {
          Thread.Sleep(2000);
          try
          {
            int num = await this.FWUpdateFunction.makeBackUp(this.ProgressDummy, this.TokenDummy) ? 1 : 0;
          }
          catch
          {
            this.EndWork(true, "Make Backup Failed");
            di = (DeviceIdentification) null;
            return;
          }
        }
        ++this.Phase;
        this.RefreshView();
        if (this.BL_MapID > 0)
        {
          try
          {
            this.FWUpdateFunction.loadThatBootloaderFileFromDB((uint) this.BL_MapID);
          }
          catch
          {
            this.EndWork(true, "Load BootLoader File Failed");
            di = (DeviceIdentification) null;
            return;
          }
        }
        else if (!this.FWUpdateFunction.loadBootloaderFromstring(this.BL_Data))
        {
          this.EndWork(true, "Load BootLoader File Failed");
          di = (DeviceIdentification) null;
          return;
        }
        ++this.Phase;
        this.RefreshView();
        if (!InBootLoader)
        {
          Thread.Sleep(2000);
          try
          {
            await this.FWUpdateFunction.writeBootLoaderToDevice128kb(this.ProgressDummy, this.TokenDummy);
          }
          catch
          {
            this.EndWork(true, "Write BootLoader Failed");
            di = (DeviceIdentification) null;
            return;
          }
        }
        ++this.Phase;
        this.RefreshView();
        if (!InBootLoader)
        {
          Thread.Sleep(2000);
          try
          {
            await this.FWUpdateFunction.verifyBootLoaderAsync(this.ProgressDummy, this.CancelSourceDummy);
          }
          catch
          {
            this.EndWork(true, "Verify BootLoader Failed");
            di = (DeviceIdentification) null;
            return;
          }
        }
        ++this.Phase;
        this.RefreshView();
        if (!InBootLoader)
        {
          Thread.Sleep(2000);
          try
          {
            this.FWUpdateFunction.readFirstEightBytesFromBootLoader();
            await this.FWUpdateFunction.writeFirstEightBytesToFLASH(this.ProgressDummy, this.TokenDummy);
            await this.FWUpdateFunction.doSystemResetFunction(this.ProgressDummy, this.CancelSourceDummy);
          }
          catch
          {
            this.EndWork(true, "Start BootLoader Failed");
            di = (DeviceIdentification) null;
            return;
          }
        }
        ++this.Phase;
        this.RefreshView();
        if (this.FW_MapID > 0)
        {
          try
          {
            this.FWUpdateFunction.loadFirmwareFileFromDB((uint) this.FW_MapID);
          }
          catch
          {
            this.EndWork(true, "Load Firmware File Failed");
            di = (DeviceIdentification) null;
            return;
          }
        }
        else if (!this.FWUpdateFunction.loadFirmwareFromString(this.FW_Data))
        {
          this.EndWork(true, "Load Firmware File Failed");
          di = (DeviceIdentification) null;
          return;
        }
        ++this.Phase;
        this.RefreshView();
        Thread.Sleep(2000);
        try
        {
          await this.FWUpdateFunction.BSL_eraseOldFW(this.ProgressDummy, this.CancelSourceDummy);
        }
        catch
        {
          this.EndWork(true, "Delete Old Firmware Failed");
          di = (DeviceIdentification) null;
          return;
        }
        this.FirmwareVersion = string.Empty;
        ++this.Phase;
        this.RefreshView();
        Thread.Sleep(2000);
        try
        {
          await this.FWUpdateFunction.BSL_writeNewFW(this.ProgressDummy, this.CancelSourceDummy);
        }
        catch
        {
          this.EndWork(true, "Write New Firmware Failed");
          di = (DeviceIdentification) null;
          return;
        }
        ++this.Phase;
        this.RefreshView();
        Thread.Sleep(2000);
        try
        {
          Thread.Sleep(1000);
          await this.VerifyFirmware(this.FWUpdateFunction, this.ProgressDummy, this.CancelSourceDummy);
        }
        catch (Exception ex1)
        {
          Exception ex = ex1;
          this.EndWork(true, "Verify New Firmware Failed");
          di = (DeviceIdentification) null;
          return;
        }
        ++this.Phase;
        this.RefreshView();
        Thread.Sleep(5000);
        for (int i = 0; i < 10; ++i)
        {
          try
          {
            this.FWUpdateFunction.BSL_GO(this.ProgressDummy, this.CancelSourceDummy).Wait(1500);
            break;
          }
          catch
          {
            Thread.Sleep(1000);
          }
        }
        ++this.Phase;
        this.RefreshView();
        Thread.Sleep(2000);
        try
        {
          try
          {
            int num = await this.Handler.ReadDeviceAsync(this.ProgressDummy, this.TokenDummy, ReadPartsSelection.AllWithoutLogger);
          }
          catch
          {
            this.EndWork(true, "Read Device After Flash Firmware Failed");
            di = (DeviceIdentification) null;
            return;
          }
          di = this.Handler.GetDeviceIdentification(0);
          firmwareVersionObj = di.FirmwareVersionObj;
          this.FirmwareVersion = firmwareVersionObj.ToString();
          this.RefreshView();
          if (this.MeterID <= 0)
          {
            uint? nullable = await this.FWUpdateFunction.getMeterIDFromARM_ID(this.ProgressDummy, new CancellationTokenSource(), di.Unique_ID);
            this.MeterID = (int) nullable.Value;
            nullable = new uint?();
          }
          if (!this.Handler.LoadLastBackup(this.MeterID))
          {
            this.EndWork(true, "Load Backup Failed");
            di = (DeviceIdentification) null;
            return;
          }
          this.Handler.OverwriteSrcToDest(HandlerMeterObjects.BackupMeter, HandlerMeterObjects.WorkMeter, this.Handler.GetAllOverwriteGroups());
          await this.Handler.WriteDeviceAsync(this.ProgressDummy, this.TokenDummy);
        }
        catch
        {
          this.EndWork(true, "Overwrite From Backup Failed");
          di = (DeviceIdentification) null;
          return;
        }
        ++this.Phase;
        this.RefreshView();
        Thread.Sleep(2000);
        try
        {
          int num = await this.Handler.ReadDeviceAsync(this.ProgressDummy, this.TokenDummy, ReadPartsSelection.AllWithoutLogger);
          await this.Handler.Calibrate_VCC2(this.ProgressDummy, this.TokenDummy);
        }
        catch
        {
          this.EndWork(true, "Do VCC2 Calibration Failed");
          di = (DeviceIdentification) null;
          return;
        }
        this.EndWork(false, string.Empty);
        di = (DeviceIdentification) null;
      }
    }

    private void ShowMessage(Exception ex)
    {
      int num = (int) MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
    }

    private async Task VerifyFirmware(
      FirmwareUpdateFunctions oFWUpFunc,
      ProgressHandler progress,
      CancellationTokenSource cancelTokenSource)
    {
      List<AddressRange> listADR = new List<AddressRange>();
      AddressRange gapBSL = (AddressRange) null;
      if (oFWUpFunc.iBootLoaderStartAddress > 0U && oFWUpFunc.iBootLoaderEndAddress > 0U)
      {
        gapBSL = new AddressRange(oFWUpFunc.iBootLoaderStartAddress);
        gapBSL.EndAddress = oFWUpFunc.iBootLoaderEndAddress;
        listADR.Add(gapBSL);
      }
      uint loops = 1;
      uint size = 128;
      oFWUpFunc.verifyPageSize = size;
      uint runs = 1;
      while (runs <= loops)
      {
        if (oFWUpFunc.BSL_isIUW())
        {
          await oFWUpFunc.verifyFirmwareCRCAsync(progress, cancelTokenSource);
          listADR = (List<AddressRange>) null;
          gapBSL = (AddressRange) null;
          return;
        }
        bool isGapVerify = true;
        uint startRange = 0;
        uint endRange = 0;
        isGapVerify &= this.TryParseHex((string) null, out startRange);
        isGapVerify &= this.TryParseHex((string) null, out endRange);
        if (isGapVerify)
        {
          AddressRange gapVerify = new AddressRange(startRange);
          gapVerify.EndAddress = endRange;
          listADR.Add(gapVerify);
          gapVerify = (AddressRange) null;
        }
        await oFWUpFunc.verifyFirmwareAsync(progress, cancelTokenSource, listADR);
        ++runs;
        if (loops == 99U)
        {
          if (size >= 1024U)
            size = 64U;
          oFWUpFunc.verifyPageSize = (size += 64U);
        }
      }
      listADR = (List<AddressRange>) null;
      gapBSL = (AddressRange) null;
    }

    private void RefreshView()
    {
      this.MainDispatcher.Invoke((Delegate) Dut_Handler.RefreshDGInvoke);
    }

    private bool TryParseHex(string hex, out uint result)
    {
      result = 0U;
      if (hex == null)
        return false;
      try
      {
        result = Convert.ToUInt32(hex, 16);
        return true;
      }
      catch
      {
        return false;
      }
    }

    private void EndWork(bool failed, string failreason)
    {
      if (failed)
      {
        this.Failed = true;
      }
      else
      {
        this.Success = true;
        this.Phase = 0;
      }
      this.FailReason = failreason;
      this.IsRunning = false;
      this.ComPort.Close();
      this.RefreshView();
    }
  }
}
