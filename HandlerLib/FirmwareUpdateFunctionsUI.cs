// Decompiled with JetBrains decompiler
// Type: HandlerLib.FirmwareUpdateFunctionsUI
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using GmmDbLib.DataSets;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class FirmwareUpdateFunctionsUI : Window, IZDebugInfo, IComponentConnector
  {
    private const string translaterBaseKey = "FirmwareUpdateTool_";
    internal readonly List<string> HardwareNames = new List<string>()
    {
      "FWUpdateToolUI"
    };
    private FirmwareUpdateFunctions oFWUpFunc;
    private Cursor defaultCursor;
    private CancellationTokenSource cancelTokenSource;
    private ProgressHandler progress;
    private DeviceIdentification deviceID;
    private uint loopsToVerify;
    private uint? MeterID;
    internal Menu menuMain;
    internal MenuItem MenuItemComponents;
    internal GmmCorporateControl gmmCorporateControl1;
    internal TextBlock TextBlockStatus;
    internal ProgressBar ProgressBar1;
    internal GroupBox GroupBoxConnected;
    internal TextBox TextBoxFWDB;
    internal ComboBox ComboBoxFWDB;
    internal TextBox TextBoxBSLDB;
    internal ComboBox ComboBoxBSLDB;
    internal TextBox TextBoxInfo;
    internal Button ButtonBackUPDevice;
    internal Button ButtonLoadBackUP;
    internal Button ButtonOverrideDevice;
    internal TextBox TextBoxMeterID;
    internal Button ButtonCreatefromArmID;
    internal StackPanel StackPanalButtons;
    internal Label LabelByteSize;
    internal TextBox TextBoxVerifyReadByteSize;
    internal Label LabelLoops;
    internal TextBox TextBoxVerifyLoops;
    internal Button ButtonFWConnect;
    internal Button ButtonFWReadARMID;
    internal Button ButtonFWLoadFromFile;
    internal Button ButtonFWLoadFromDB;
    internal Button ButtonFWDeleteOnDevice;
    internal Button ButtonFWWriteToDevice;
    internal Button ButtonFWVerifyOnDevice;
    internal Button ButtonFWReadFlashSize;
    internal Button ButtonFWReadVectorTable;
    internal Button ButtonFWWriteVectorTable;
    internal Button ButtonGo;
    internal Button ButtonFWReadMeterKey;
    internal Label LabelRange;
    internal TextBox TextBoxStartRange;
    internal TextBox TextBoxEndRange;
    internal Button ButtonBSLConnect;
    internal Button ButtonBSLReadARMID;
    internal Button ButtonBSLLoadFromFile;
    internal Button ButtonBSLLoadFromDB;
    internal Button ButtonBSLWriteToDevice;
    internal Button ButtonBSLVerifyOnDevice;
    internal Button ButtonBSLDeleteOnDevice;
    internal Button ButtonBSLOverwriteOnDevice;
    internal Button ButtonBSLReadFlashSize;
    internal Button ButtonBSLResetMeterKey;
    internal Button ButtonBSLReadFirst8Byte;
    internal Button ButtonBSLWriteFirst8Byte;
    internal Button ButtonDeviceReset;
    internal CheckBox CheckShowDetails;
    internal CheckBox CheckIgnoreError;
    internal Button ButtonWriteBSLtoDB;
    internal Button ButtonCheckFWworkWithBSL;
    internal Button ButtonClear;
    internal Button ButtonBreak;
    private bool _contentLoaded;

    public FirmwareUpdateFunctionsUI(
      FirmwareUpdateFunctions fwUpdateFunc,
      DeviceIdentification devID = null)
    {
      this.oFWUpFunc = fwUpdateFunc;
      this.oFWUpFunc.oFUI = (IZDebugInfo) this;
      this.deviceID = devID;
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.loopsToVerify = 1U;
      this.MeterID = new uint?();
      this.InitializeComponent();
      this.Title = this.Title + "Firmware Update Tool UI - CoreVersion: " + fwUpdateFunc.CoreVersion;
      if (this.deviceID != null)
      {
        this.getAllPossibleBSLfromDB();
        this.getAllPossibleFWfromDB();
        this.SetStopState();
      }
      else
        this.SetStopState(false);
    }

    private void SetRunState()
    {
      this.progress.Reset("connecting...");
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress.Reset();
      this.progress.Split(new double[2]{ 2.0, 98.0 });
      this.progress.Report("Run");
      this.GroupBoxConnected.IsEnabled = false;
      this.StackPanalButtons.IsEnabled = false;
      this.setButtonsEnabled(false);
      this.ButtonBreak.IsEnabled = true;
      this.defaultCursor = this.Cursor;
      this.Cursor = Cursors.Wait;
    }

    private void SetStopState(bool state = true)
    {
      this.TextBoxVerifyLoops.Text = this.loopsToVerify.ToString();
      this.TextBoxVerifyReadByteSize.Text = this.oFWUpFunc.verifyPageSize.ToString();
      this.GroupBoxConnected.IsEnabled = true;
      this.StackPanalButtons.IsEnabled = true;
      this.ButtonBreak.IsEnabled = false;
      if (this.defaultCursor != Cursors.Wait)
        this.Cursor = this.defaultCursor;
      else
        this.Cursor = Cursors.Arrow;
      this.setButtonsEnabled(state);
      this.progress.Reset();
    }

    private void OnProgress(ProgressArg obj)
    {
      if (!this.CheckAccess())
      {
        this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
      }
      else
      {
        this.ProgressBar1.Value = obj.ProgressPercentage;
        this.TextBlockStatus.Text = obj.Message;
      }
    }

    private void ButtonBreak_Click(object sender, RoutedEventArgs e)
    {
      this.cancelTokenSource.Cancel();
    }

    private void ButtonClear_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      this.oFWUpFunc.clearVariables();
      this.setInfoText(DateTime.Now.ToString(), true);
      this.SetStopState();
    }

    private void getAllPossibleFWfromDB()
    {
      try
      {
        this.SetRunState();
        this.ComboBoxFWDB.Items.Clear();
        SortedList<uint, HardwareTypeTables.ProgFilesRow> sortedList1 = new SortedList<uint, HardwareTypeTables.ProgFilesRow>();
        SortedList<uint, HardwareTypeTables.ProgFilesRow> sortedList2 = this.deviceID == null ? this.oFWUpFunc.revealAllPossibleFirmwareFromDatabase() : this.oFWUpFunc.revealAllPossibleFirmwareFromDatabase(this.deviceID.FirmwareVersion.Value, this.deviceID.FirmwareVersionObj.TypeString);
        if (sortedList2.Count > 0)
        {
          foreach (uint key in (IEnumerable<uint>) sortedList2.Keys)
          {
            HardwareTypeTables.ProgFilesRow progFilesRow = sortedList2[key];
            ListBoxItem newItem = new ListBoxItem();
            FirmwareVersion firmwareVersion = new FirmwareVersion((uint) progFilesRow.FirmwareVersion);
            string[] strArray = new string[6]
            {
              "FW: ",
              firmwareVersion.VersionString,
              " / ",
              firmwareVersion.TypeString,
              " - MAP: ",
              null
            };
            int num = progFilesRow.MapID;
            strArray[5] = num.ToString();
            string str1 = string.Concat(strArray);
            newItem.Tag = (object) progFilesRow;
            ListBoxItem listBoxItem = newItem;
            num = progFilesRow.FirmwareVersion;
            string str2 = "FW_" + num.ToString();
            listBoxItem.Name = str2;
            newItem.Content = (object) str1;
            this.ComboBoxFWDB.Items.Add((object) newItem);
          }
          this.ComboBoxFWDB.Text = " -- select a firmware from here -- ";
        }
        else
        {
          this.ComboBoxFWDB.Items.Clear();
          this.ComboBoxFWDB.Text = "No Firmware found in this database.";
        }
      }
      finally
      {
        this.SetStopState();
      }
    }

    private void getAllPossibleBSLfromDB()
    {
      try
      {
        this.SetRunState();
        this.ComboBoxBSLDB.Items.Clear();
        SortedList<uint, HardwareTypeTables.ProgFilesRow> sortedList1 = new SortedList<uint, HardwareTypeTables.ProgFilesRow>();
        SortedList<uint, HardwareTypeTables.ProgFilesRow> sortedList2 = this.deviceID == null ? this.oFWUpFunc.revealAllPossibleBootloaderFromDatabase() : this.oFWUpFunc.revealAllPossibleBootloaderFromDatabase(this.deviceID.FirmwareVersion.Value);
        if (sortedList2.Count > 0)
        {
          foreach (uint key in (IEnumerable<uint>) sortedList2.Keys)
          {
            HardwareTypeTables.ProgFilesRow progFilesRow = sortedList2[key];
            ListBoxItem newItem = new ListBoxItem();
            FirmwareVersion firmwareVersion = new FirmwareVersion((uint) progFilesRow.FirmwareVersion);
            string[] strArray = new string[6]
            {
              "FW: ",
              firmwareVersion.VersionString,
              " / ",
              firmwareVersion.TypeBSLString,
              " MAP: ",
              null
            };
            int mapId = progFilesRow.MapID;
            strArray[5] = mapId.ToString();
            string str1 = string.Concat(strArray);
            newItem.Tag = (object) progFilesRow;
            ListBoxItem listBoxItem = newItem;
            string str2 = progFilesRow.HardwareName.Replace(" ", "");
            mapId = progFilesRow.MapID;
            string str3 = mapId.ToString();
            string str4 = str2 + "_" + str3;
            listBoxItem.Name = str4;
            newItem.Content = (object) str1;
            this.ComboBoxBSLDB.Items.Add((object) newItem);
          }
          this.ComboBoxBSLDB.Text = " -- select a firmware from here -- ";
        }
        else
        {
          this.ComboBoxBSLDB.Items.Clear();
          this.ComboBoxBSLDB.Text = "No Firmware found in this database.";
        }
      }
      finally
      {
        this.SetStopState();
      }
    }

    public bool? ignoreError()
    {
      return this.CheckIgnoreError.Dispatcher.Invoke<bool?>((Func<bool?>) (() => this.CheckIgnoreError.IsChecked));
    }

    public void setDeviceID(DeviceIdentification devID)
    {
      this.Dispatcher.Invoke((Action) (() => this.deviceID = devID));
      if (this.deviceID == null)
        return;
      this.MeterID = this.deviceID.MeterID;
    }

    public void setDebugInfo(string txt)
    {
      bool? nullable1 = this.CheckShowDetails.Dispatcher.Invoke<bool?>((Func<bool?>) (() => this.CheckShowDetails.IsChecked));
      int num;
      if (nullable1.HasValue)
      {
        bool? nullable2 = nullable1;
        bool flag = true;
        num = nullable2.GetValueOrDefault() == flag & nullable2.HasValue ? 1 : 0;
      }
      else
        num = 0;
      if (num == 0)
        return;
      this.Dispatcher.Invoke((Action) (() => this.setInfoText(txt)));
    }

    public void setDebugInfo(string txt, bool clear = false, int clearRows = 0)
    {
      bool? nullable1 = this.CheckShowDetails.Dispatcher.Invoke<bool?>((Func<bool?>) (() => this.CheckShowDetails.IsChecked));
      int num;
      if (nullable1.HasValue)
      {
        bool? nullable2 = nullable1;
        bool flag = true;
        num = nullable2.GetValueOrDefault() == flag & nullable2.HasValue ? 1 : 0;
      }
      else
        num = 0;
      if (num == 0)
        return;
      this.Dispatcher.Invoke((Action) (() => this.setInfoText(txt, clear, clearRows)));
    }

    public void setErrorInfo(string txt)
    {
      bool? nullable1 = this.CheckIgnoreError.Dispatcher.Invoke<bool?>((Func<bool?>) (() => this.CheckIgnoreError.IsChecked));
      int num;
      if (nullable1.HasValue)
      {
        bool? nullable2 = nullable1;
        bool flag = true;
        num = nullable2.GetValueOrDefault() == flag & nullable2.HasValue ? 1 : 0;
      }
      else
        num = 0;
      if (num == 0)
        return;
      this.Dispatcher.Invoke((Action) (() => this.setInfoText(txt)));
    }

    private void setInfoText(string txt, bool clear = false, int clearRows = 0)
    {
      if (clear)
        this.TextBoxInfo.Text = txt;
      else
        this.TextBoxInfo.Text += txt;
      this.TextBoxInfo.SelectionStart = this.TextBoxInfo.Text.Length;
      this.TextBoxInfo.ScrollToEnd();
    }

    private void setButtonsEnabled(bool enableFW = true)
    {
      this.ButtonFWConnect.IsEnabled = true;
      this.ButtonFWLoadFromFile.IsEnabled = true;
      this.ButtonFWLoadFromDB.IsEnabled = true;
      this.ButtonBSLLoadFromFile.IsEnabled = true;
      this.ButtonBSLLoadFromDB.IsEnabled = true;
      this.ButtonFWDeleteOnDevice.IsEnabled = enableFW;
      this.ButtonFWWriteToDevice.IsEnabled = enableFW;
      this.ButtonFWReadFlashSize.IsEnabled = enableFW;
      this.ButtonFWVerifyOnDevice.IsEnabled = enableFW;
      this.ButtonBSLWriteToDevice.IsEnabled = enableFW;
      this.ButtonBSLVerifyOnDevice.IsEnabled = enableFW;
      this.ButtonBSLDeleteOnDevice.IsEnabled = enableFW;
      this.ButtonBSLOverwriteOnDevice.IsEnabled = enableFW;
      this.ButtonBSLReadFlashSize.IsEnabled = enableFW;
      this.ButtonBSLReadFirst8Byte.IsEnabled = enableFW;
      this.ButtonBSLWriteFirst8Byte.IsEnabled = enableFW;
      this.ButtonDeviceReset.IsEnabled = enableFW;
      this.ButtonBSLResetMeterKey.IsEnabled = enableFW;
      this.ButtonFWReadMeterKey.IsEnabled = enableFW;
      this.ButtonGo.IsEnabled = enableFW && this.oFWUpFunc.isReady2GO;
      this.ButtonFWWriteVectorTable.Background = this.oFWUpFunc.isVectorTableLoaded ? (Brush) Brushes.Lime : (Brush) Brushes.Gainsboro;
      this.ButtonBSLWriteFirst8Byte.Background = this.oFWUpFunc.isFirstEightBytesLoaded ? (Brush) Brushes.Lime : (Brush) Brushes.Gainsboro;
      this.CheckShowDetails.IsEnabled = enableFW;
      this.TextBoxVerifyLoops.IsEnabled = enableFW;
      this.TextBoxVerifyReadByteSize.IsEnabled = enableFW;
      this.ButtonBackUPDevice.IsEnabled = enableFW && this.MeterID.HasValue;
      this.ButtonLoadBackUP.IsEnabled = enableFW && this.MeterID.HasValue;
      this.ButtonOverrideDevice.IsEnabled = enableFW && this.MeterID.HasValue && this.oFWUpFunc.zippedBackUpData != null;
      this.ButtonCreatefromArmID.IsEnabled = enableFW;
      this.ButtonFWReadVectorTable.IsEnabled = enableFW;
      this.ButtonFWWriteVectorTable.IsEnabled = enableFW;
      this.ButtonWriteBSLtoDB.IsEnabled = true;
    }

    private async void ButtonHandlerDoEvent_Click(object sender, RoutedEventArgs e)
    {
      Stopwatch myStopWatch = new Stopwatch();
      this.SetRunState();
      try
      {
        this.progress.Reset();
        if (this.cancelTokenSource == null)
          this.cancelTokenSource = new CancellationTokenSource();
        if ((Button) sender == this.ButtonFWConnect)
        {
          this.ComboBoxFWDB.Items.Clear();
          this.ComboBoxBSLDB.Items.Clear();
          myStopWatch.Start();
          DeviceIdentification deviceIdentification = await this.oFWUpFunc.ReadVersionAsync(this.progress, this.cancelTokenSource.Token);
          this.deviceID = deviceIdentification;
          deviceIdentification = (DeviceIdentification) null;
          myStopWatch.Stop();
          FirmwareVersion FWVersion = new FirmwareVersion(this.deviceID.FirmwareVersion.Value);
          SortedList<uint, HardwareTypeTables.ProgFilesRow> FWrows = this.oFWUpFunc.revealAllPossibleFirmwareFromDatabase(FWVersion.Version, FWVersion.TypeString);
          if (FWrows.Count > 0)
          {
            foreach (HardwareTypeTables.ProgFilesRow myRow in (IEnumerable<HardwareTypeTables.ProgFilesRow>) FWrows.Values)
            {
              FirmwareVersion oFWVer = new FirmwareVersion((uint) myRow.FirmwareVersion);
              ListBoxItem lbx = new ListBoxItem();
              string strBX = "FW: " + oFWVer.VersionString + " / " + myRow.HardwareName + " - MAP: " + myRow.MapID.ToString();
              lbx.Tag = (object) myRow;
              lbx.Name = "FW_" + myRow.FirmwareVersion.ToString();
              lbx.Content = (object) strBX;
              this.ComboBoxFWDB.Items.Add((object) lbx);
              oFWVer = new FirmwareVersion();
              lbx = (ListBoxItem) null;
              strBX = (string) null;
            }
            this.ComboBoxFWDB.SelectedIndex = 0;
          }
          SortedList<uint, HardwareTypeTables.ProgFilesRow> BSLrows = this.oFWUpFunc.revealAllPossibleBootloaderFromDatabase(FWVersion.Version);
          if (BSLrows.Count > 0)
          {
            foreach (HardwareTypeTables.ProgFilesRow myRow in (IEnumerable<HardwareTypeTables.ProgFilesRow>) BSLrows.Values)
            {
              FirmwareVersion oFWVer = new FirmwareVersion((uint) myRow.FirmwareVersion);
              ListBoxItem lbx = new ListBoxItem();
              string strBX = "FW: " + oFWVer.VersionString + " / " + myRow.HardwareName + " - MAP: " + myRow.MapID.ToString();
              lbx.Tag = (object) myRow;
              lbx.Name = myRow.HardwareName.Replace(" ", "") + "_" + myRow.MapID.ToString();
              lbx.Content = (object) strBX;
              this.ComboBoxBSLDB.Items.Add((object) lbx);
              oFWVer = new FirmwareVersion();
              lbx = (ListBoxItem) null;
              strBX = (string) null;
            }
            this.ComboBoxBSLDB.SelectedIndex = 0;
          }
          this.setInfoText("\n");
          this.setInfoText(this.getDeviceInfoString(this.deviceID));
          this.setInfoText("\nElapsed time: " + ((double) myStopWatch.ElapsedMilliseconds / 1000.0).ToString() + " sec.");
          FWVersion = new FirmwareVersion();
          FWrows = (SortedList<uint, HardwareTypeTables.ProgFilesRow>) null;
          BSLrows = (SortedList<uint, HardwareTypeTables.ProgFilesRow>) null;
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonBSLConnect)
        {
          myStopWatch.Start();
          this.ComboBoxFWDB.Items.Clear();
          string allowedFW = string.Empty;
          uint BSLVersion = await this.oFWUpFunc.BSL_getVersion(this.progress, this.cancelTokenSource);
          long mapid = this.oFWUpFunc.revealMapIDForBSLFirmwareFromDatabase(out allowedFW, BSLVersion);
          if (mapid > 0L)
            this.oFWUpFunc.loadThatBootloaderFileFromDB((uint) mapid);
          DeviceIdentification BSL = new DeviceIdentification(BSLVersion);
          SortedList<uint, HardwareTypeTables.ProgFilesRow> FWrows = this.oFWUpFunc.revealAllPossibleFirmwareFromDatabase(BSL.FirmwareVersion.Value, BSL.FirmwareVersionObj.TypeBSLString, allowedFW);
          if (FWrows.Count > 0)
          {
            foreach (HardwareTypeTables.ProgFilesRow myRow in (IEnumerable<HardwareTypeTables.ProgFilesRow>) FWrows.Values)
            {
              FirmwareVersion oFWVer = new FirmwareVersion((uint) myRow.FirmwareVersion);
              ListBoxItem lbx = new ListBoxItem();
              string strBX = "FW: " + oFWVer.VersionString + " / " + myRow.HardwareName + " - MAP: " + myRow.MapID.ToString();
              lbx.Tag = (object) myRow;
              lbx.Name = "FW_" + myRow.FirmwareVersion.ToString();
              lbx.Content = (object) strBX;
              this.ComboBoxFWDB.Items.Add((object) lbx);
              oFWVer = new FirmwareVersion();
              lbx = (ListBoxItem) null;
              strBX = (string) null;
            }
            this.ComboBoxFWDB.SelectedIndex = 0;
          }
          myStopWatch.Stop();
          this.setInfoText("\n");
          this.setInfoText("\nBOOTLOADER: " + this.oFWUpFunc.BSLversion.ToString());
          this.setInfoText("\nElapsed time: " + ((double) myStopWatch.ElapsedMilliseconds / 1000.0).ToString() + " sec.");
          allowedFW = (string) null;
          BSL = (DeviceIdentification) null;
          FWrows = (SortedList<uint, HardwareTypeTables.ProgFilesRow>) null;
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonBSLWriteToDevice)
        {
          myStopWatch.Start();
          this.setInfoText("\nStart writing Bootloader to device... ");
          await this.oFWUpFunc.writeBootLoaderToDevice128kb(this.progress, this.cancelTokenSource.Token);
          myStopWatch.Stop();
          this.setInfoText("\nDONE, written BOOTLOADER to device in " + ((double) myStopWatch.ElapsedMilliseconds / 1000.0).ToString() + " sec.");
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonFWWriteToDevice)
        {
          myStopWatch.Start();
          this.setInfoText("\nStart writing NEW FIRMWARE to device... ");
          if (this.oFWUpFunc.BSL_isIUW())
          {
            if (this.oFWUpFunc.blockInfo_Firmware != null)
              await this.oFWUpFunc.BSL_writeNewFW_IUW_BLOCKMODE(this.progress, this.cancelTokenSource);
            else
              await this.oFWUpFunc.BSL_writeNewFW_IUW(this.progress, this.cancelTokenSource);
          }
          else
            await this.oFWUpFunc.BSL_writeNewFW(this.progress, this.cancelTokenSource);
          myStopWatch.Stop();
          this.setInfoText("\nDONE, written firmware to device in " + ((double) myStopWatch.ElapsedMilliseconds / 1000.0).ToString() + " sec.");
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonFWVerifyOnDevice)
        {
          if (this.oFWUpFunc.dicBytesToFlashFIRMWARE == null || this.oFWUpFunc.dicBytesToFlashFIRMWARE.Count < 1)
            throw new Exception("Select FIRMWARE first !!!");
          List<AddressRange> listADR = new List<AddressRange>();
          AddressRange gapBSL = (AddressRange) null;
          if (this.oFWUpFunc.iBootLoaderStartAddress > 0U && this.oFWUpFunc.iBootLoaderEndAddress > 0U)
          {
            gapBSL = new AddressRange(this.oFWUpFunc.iBootLoaderStartAddress);
            gapBSL.EndAddress = this.oFWUpFunc.iBootLoaderEndAddress;
            listADR.Add(gapBSL);
          }
          else
            this.CheckIgnoreError.IsChecked = new bool?(true);
          uint loops = 1;
          uint size = 128;
          uint.TryParse(this.TextBoxVerifyReadByteSize.Text, out size);
          uint.TryParse(this.TextBoxVerifyLoops.Text, out loops);
          this.oFWUpFunc.verifyPageSize = size;
          uint runs = 1;
          while (runs <= loops)
          {
            this.setInfoText("\nVerify FIRMWARE ...START...");
            this.setInfoText("\nreading size set to (" + size.ToString() + ") bytes.");
            this.setInfoText("\nlooping verify (" + runs.ToString() + "/" + loops.ToString() + ")");
            myStopWatch.Restart();
            if (this.oFWUpFunc.BSL_isIUW())
            {
              await this.oFWUpFunc.verifyFirmwareCRCAsync(this.progress, this.cancelTokenSource);
              break;
            }
            bool isGapVerify = true;
            uint startRange = 0;
            uint endRange = 0;
            isGapVerify &= FirmwareUpdateFunctionsUI.TryParseHex(this.TextBoxStartRange.Text, out startRange);
            isGapVerify &= FirmwareUpdateFunctionsUI.TryParseHex(this.TextBoxEndRange.Text, out endRange);
            if (isGapVerify)
            {
              AddressRange gapVerify = new AddressRange(startRange);
              gapVerify.EndAddress = endRange;
              listADR.Add(gapVerify);
              gapVerify = (AddressRange) null;
            }
            await this.oFWUpFunc.verifyFirmwareAsync(this.progress, this.cancelTokenSource, listADR);
            myStopWatch.Stop();
            this.setInfoText("\nVerify FIRMWARE ... running: " + ((double) myStopWatch.ElapsedMilliseconds / 1000.0).ToString() + " sec.");
            ++runs;
            if (loops == 99U)
            {
              if (size >= 1024U)
                size = 64U;
              this.oFWUpFunc.verifyPageSize = (size += 64U);
            }
          }
          listADR = (List<AddressRange>) null;
          gapBSL = (AddressRange) null;
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonBSLVerifyOnDevice)
        {
          myStopWatch.Start();
          uint pageSize = 128;
          if (uint.TryParse(this.TextBoxVerifyReadByteSize.Text, out pageSize))
          {
            if (this.oFWUpFunc.BSL_isIUW())
              await this.oFWUpFunc.verifyBootloaderCRCAsync(this.progress, this.cancelTokenSource);
            else
              await this.oFWUpFunc.verifyBootLoaderAsync(this.progress, this.cancelTokenSource, pageSize: pageSize);
          }
          else
            this.setErrorInfo("ByteSize has wrong format !!! Please enter correct value.");
          myStopWatch.Stop();
          this.setInfoText("\nVerify BOOTLOADER ... running: " + ((double) myStopWatch.ElapsedMilliseconds / 1000.0).ToString() + " sec.");
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonGo)
        {
          myStopWatch.Start();
          this.oFWUpFunc.BSL_GO(this.progress, this.cancelTokenSource).Wait(1500);
          myStopWatch.Stop();
          this.setInfoText("\nstarting FW init process ... running: " + ((double) myStopWatch.ElapsedMilliseconds / 1000.0).ToString() + " sec.");
          this.setInfoText("\nDONE.");
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonBSLReadFlashSize)
        {
          myStopWatch.Start();
          ushort BTL_FlashSize = await this.oFWUpFunc.BSL_getFLASHSize(this.progress, this.cancelTokenSource);
          this.setInfoText("\nFlashSize: " + BTL_FlashSize.ToString() + "kByte");
          myStopWatch.Stop();
          this.setInfoText("\nRead Flash size from device ... running: " + ((double) myStopWatch.ElapsedMilliseconds / 1000.0).ToString() + " sec.");
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonFWReadFlashSize)
        {
          myStopWatch.Start();
          ushort BTL_FlashSize = await this.oFWUpFunc.BSL_getFLASHSize(this.progress, this.cancelTokenSource, false);
          this.setInfoText("\nFlashSize: " + BTL_FlashSize.ToString() + "kByte");
          myStopWatch.Stop();
          this.setInfoText("\nRead Flash size from device ... running: " + ((double) myStopWatch.ElapsedMilliseconds / 1000.0).ToString() + " sec.");
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonBSLReadARMID)
        {
          myStopWatch.Start();
          byte[] ArmID = await this.oFWUpFunc.BSL_getARM_ID(this.progress, this.cancelTokenSource);
          this.setInfoText("\nUnique ID: " + Util.ByteArrayToHexString(ArmID));
          uint? nullable = await this.oFWUpFunc.getMeterIDFromARM_ID(this.progress, this.cancelTokenSource, ArmID);
          this.MeterID = nullable;
          nullable = new uint?();
          this.setInfoText("\nMeterID: " + (this.MeterID.HasValue ? this.MeterID.Value.ToString() : " ---> no MeterID found !!!"));
          this.TextBoxMeterID.Text = this.MeterID.ToString();
          myStopWatch.Stop();
          this.setInfoText("\nRead Flash size from device ... running: " + ((double) myStopWatch.ElapsedMilliseconds / 1000.0).ToString() + " sec.");
          ArmID = (byte[]) null;
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonFWReadARMID)
        {
          myStopWatch.Start();
          byte[] ArmID = await this.oFWUpFunc.BSL_getARM_ID(this.progress, this.cancelTokenSource, false);
          this.setInfoText("\nUnique ID: " + Util.ByteArrayToHexString(ArmID));
          uint? nullable = await this.oFWUpFunc.getMeterIDFromARM_ID(this.progress, this.cancelTokenSource, ArmID);
          this.MeterID = nullable;
          nullable = new uint?();
          this.setInfoText("\nMeterID: " + (this.MeterID.HasValue ? this.MeterID.Value.ToString() : " ---> no MeterID found !!!"));
          this.TextBoxMeterID.Text = this.MeterID.ToString();
          myStopWatch.Stop();
          this.setInfoText("\nRead Flash size from device ... running: " + ((double) myStopWatch.ElapsedMilliseconds / 1000.0).ToString() + " sec.");
          ArmID = (byte[]) null;
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonFWDeleteOnDevice)
        {
          myStopWatch.Start();
          await this.oFWUpFunc.BSL_eraseOldFW(this.progress, this.cancelTokenSource);
          myStopWatch.Stop();
          this.setInfoText("\nErase old FIRMWARE ... running: " + ((double) myStopWatch.ElapsedMilliseconds / 1000.0).ToString() + " sec.");
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonBSLDeleteOnDevice)
        {
          myStopWatch.Start();
          uint size = this.oFWUpFunc.iBootLoaderEndAddress - this.oFWUpFunc.iBootLoaderStartAddress;
          await this.oFWUpFunc.deleteBSLfromDevice(this.progress, this.cancelTokenSource, this.oFWUpFunc.iBootLoaderStartAddress, size);
          myStopWatch.Stop();
          this.setInfoText("\nDelete BOOTLOADER from device ... running: " + ((double) myStopWatch.ElapsedMilliseconds / 1000.0).ToString() + " sec.");
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonFWReadVectorTable)
        {
          myStopWatch.Start();
          await this.oFWUpFunc.readVectorTableFromDevice(this.progress, this.cancelTokenSource, this.oFWUpFunc.iFlashStartAdr);
          myStopWatch.Stop();
          this.setInfoText("\nRead vector table from device ... running: " + ((double) myStopWatch.ElapsedMilliseconds / 1000.0).ToString() + " sec.");
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonFWWriteVectorTable)
        {
          myStopWatch.Start();
          await this.oFWUpFunc.writeVectorTableToFLASH(this.progress, this.cancelTokenSource);
          myStopWatch.Stop();
          this.setInfoText("\nWrite vector table to device ... running: " + ((double) myStopWatch.ElapsedMilliseconds / 1000.0).ToString() + " sec.");
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonFWLoadFromFile)
        {
          myStopWatch.Start();
          this.oFWUpFunc.loadFirmwareFromFile();
          foreach (FirmwareBlockInfoClass fwBI in this.oFWUpFunc.blockInfo_Firmware)
            this.setInfoText(fwBI.ToString(0U));
          myStopWatch.Stop();
          this.setInfoText("\nLoad FW for device: " + this.oFWUpFunc.strFileName);
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonBSLLoadFromFile)
        {
          myStopWatch.Start();
          this.oFWUpFunc.loadBootloaderFromFile();
          if (this.oFWUpFunc.blockInfo_Bootloader != null)
          {
            foreach (FirmwareBlockInfoClass fwBI in this.oFWUpFunc.blockInfo_Bootloader)
              this.setInfoText(fwBI.ToString(0U));
            myStopWatch.Stop();
            this.setInfoText("\nLoad BSL file for device: " + this.oFWUpFunc.strFileName);
            myStopWatch = (Stopwatch) null;
          }
          else
          {
            this.setInfoText("\nno bootloader selected!!!");
            myStopWatch = (Stopwatch) null;
          }
        }
        else if ((Button) sender == this.ButtonBSLReadFirst8Byte)
        {
          myStopWatch.Start();
          this.oFWUpFunc.readFirstEightBytesFromBootLoader();
          myStopWatch.Stop();
          this.setInfoText("\nRead first eight bytes: " + Util.ByteArrayToHexString(this.oFWUpFunc.baFirstEightBytesFromHexFile));
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonBSLWriteFirst8Byte)
        {
          myStopWatch.Start();
          await this.oFWUpFunc.writeFirstEightBytesToFLASH(this.progress, this.cancelTokenSource.Token);
          myStopWatch.Stop();
          this.setInfoText("\nFirst eight bytes written successfully to device.");
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonDeviceReset)
        {
          myStopWatch.Start();
          this.oFWUpFunc.readFirstEightBytesFromBootLoader();
          await this.oFWUpFunc.writeFirstEightBytesToFLASH(this.progress, this.cancelTokenSource.Token);
          await this.oFWUpFunc.doSystemResetFunction(this.progress, this.cancelTokenSource);
          myStopWatch.Stop();
          this.setInfoText("\nSystem Reset done....");
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonBSLLoadFromDB)
        {
          this.setInfoText("\nLoading selected bootloader from database...");
          if (this.ComboBoxBSLDB.SelectedItem != null)
          {
            myStopWatch.Start();
            HardwareTypeTables.ProgFilesRow select = (HardwareTypeTables.ProgFilesRow) ((FrameworkElement) this.ComboBoxBSLDB.SelectedItem).Tag;
            string fileName = this.oFWUpFunc.loadThatBootloaderFileFromDB((uint) select.MapID);
            myStopWatch.Stop();
            this.setInfoText("\nBSL: " + fileName);
            this.setInfoText("\nBootloader successfully loaded...");
            select = (HardwareTypeTables.ProgFilesRow) null;
            fileName = (string) null;
            myStopWatch = (Stopwatch) null;
          }
          else
          {
            this.setInfoText("Please select a Bootloader from ComboBox first ...");
            myStopWatch = (Stopwatch) null;
          }
        }
        else if ((Button) sender == this.ButtonFWLoadFromDB)
        {
          this.setInfoText("\nLoading selected firmware from database...");
          if (this.ComboBoxFWDB.SelectedItem != null)
          {
            myStopWatch.Start();
            HardwareTypeTables.ProgFilesRow select = (HardwareTypeTables.ProgFilesRow) ((FrameworkElement) this.ComboBoxFWDB.SelectedItem).Tag;
            this.oFWUpFunc.loadFirmwareFileFromDB((uint) select.MapID);
            myStopWatch.Stop();
            if (this.oFWUpFunc.dicBytesToFlashFIRMWARE.Count > 0)
              this.setInfoText("\nFirmware successfully loaded...");
            else
              this.setInfoText("\nFirmware NOT loaded, please connect again to the device ...");
            select = (HardwareTypeTables.ProgFilesRow) null;
            myStopWatch = (Stopwatch) null;
          }
          else
          {
            this.setInfoText("Please select a Firmware from ComboBox first ...");
            myStopWatch = (Stopwatch) null;
          }
        }
        else if ((Button) sender == this.ButtonWriteBSLtoDB)
        {
          CompatibleFirmwareWindow compatibleFirmwareWindow = new CompatibleFirmwareWindow(105, "Bootloader", new uint?(this.oFWUpFunc.BSLversion.Version));
          compatibleFirmwareWindow.ShowDialog();
          compatibleFirmwareWindow = (CompatibleFirmwareWindow) null;
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonCheckFWworkWithBSL)
        {
          this.setInfoText("\nChecking firmware and bootloader memory area...");
          string strMessage = string.Empty;
          if (!this.oFWUpFunc.isFirmwarecompatibleWithActualSelectedBootloader(out strMessage))
            this.setInfoText("\nAttention!!!" + strMessage + "\nCONFLICTS !!! ... FAIL !!!");
          else
            this.setInfoText(strMessage + "\nNO CONFLICTS ... OK");
          strMessage = (string) null;
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonBSLOverwriteOnDevice)
        {
          this.setInfoText("\nOverwriting bootloader with firmware ...");
          string strMessage = string.Empty;
          int num = await this.oFWUpFunc.overwriteBSLfromDevice(this.progress, this.cancelTokenSource) ? 1 : 0;
          this.setInfoText("\nOverwrite DONE!");
          strMessage = (string) null;
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender == this.ButtonFWReadMeterKey)
        {
          await this.readOrResetMeterKey(true);
          myStopWatch = (Stopwatch) null;
        }
        else if ((Button) sender != this.ButtonBSLResetMeterKey)
        {
          myStopWatch = (Stopwatch) null;
        }
        else
        {
          this.setInfoText("\n\nResetting MeterKey for protection ... ");
          await this.readOrResetMeterKey();
          this.setInfoText("done.");
          this.setInfoText("\nRESET of MeterKey DONE!");
          myStopWatch = (Stopwatch) null;
        }
      }
      catch (Exception ex)
      {
        this.setInfoText("\nError occured while button was pressed... \n" + ex.Message + "\n" + ex.InnerException?.ToString());
        myStopWatch = (Stopwatch) null;
      }
      finally
      {
        this.SetStopState();
      }
    }

    internal async Task readOrResetMeterKey(bool bReadOnly = false)
    {
      string strMessage = string.Empty;
      byte[] baVectorTables = (byte[]) null;
      if (!this.oFWUpFunc.isBSLrunning && !bReadOnly)
      {
        this.setInfoText("\n--> reading first 8 bytes from device... ");
        baVectorTables = await this.oFWUpFunc.getVectorTableFromDevice(this.progress, this.cancelTokenSource);
        this.setInfoText("done.");
        this.setInfoText("\n--> loading bootloader from database... ");
        HardwareTypeTables.ProgFilesRow select = (HardwareTypeTables.ProgFilesRow) ((FrameworkElement) this.ComboBoxBSLDB.SelectedItem).Tag;
        string fileName = this.oFWUpFunc.loadThatBootloaderFileFromDB((uint) select.MapID);
        this.setInfoText("\n--> BSL: " + fileName);
        this.setInfoText("\n--> Bootloader successfully loaded...");
        this.setInfoText("\n--> writing bootloader to device... ");
        await this.oFWUpFunc.writeBootLoaderToDevice128kb(this.progress, this.cancelTokenSource.Token);
        this.setInfoText("done.");
        this.setInfoText("\n--> starting bootloader ... ");
        await this.oFWUpFunc.prepareUpdate(this.progress, this.cancelTokenSource);
        this.setInfoText("done.");
        select = (HardwareTypeTables.ProgFilesRow) null;
        fileName = (string) null;
      }
      this.setInfoText("\n--> reading MeterKey from device ... ");
      try
      {
        byte[] meterKey = await this.oFWUpFunc.BSL_readMeterKey(this.progress, this.cancelTokenSource);
        string strMeterKey = Utility.ByteArrayToHexString(meterKey);
        this.setInfoText("done.");
        this.setInfoText("\n--> MeterKey = " + strMeterKey);
        if (!bReadOnly)
        {
          this.setInfoText("\n--> reset MeterKey ... ");
          await this.oFWUpFunc.BSL_writeMeterKey(this.progress, this.cancelTokenSource);
          this.setInfoText("done.");
        }
        meterKey = (byte[]) null;
        strMeterKey = (string) null;
      }
      catch (Exception ex)
      {
        this.setInfoText("\n Message :\n" + ex.Message);
        this.setInfoText("\n--> not able to read MeterKey from device !!! ");
        this.setInfoText("\n--> reboot device to actual firmware ");
      }
      if (bReadOnly)
      {
        strMessage = (string) null;
        baVectorTables = (byte[]) null;
      }
      else
      {
        this.setInfoText("\n--> set vector table ... ");
        if (baVectorTables == null)
          baVectorTables = this.oFWUpFunc.getVectorTableFromFirmware();
        await this.oFWUpFunc.writeVectorTableToFLASH(this.progress, this.cancelTokenSource, baVectorTables);
        this.setInfoText("done.");
        this.setInfoText("\n--> init firmware ... ");
        this.oFWUpFunc.isReady2GO = true;
        await this.oFWUpFunc.BSL_GO(this.progress, this.cancelTokenSource);
        this.oFWUpFunc.isReady2GO = false;
        strMessage = (string) null;
        baVectorTables = (byte[]) null;
      }
    }

    internal string getDeviceInfoString(DeviceIdentification deviceIdentification)
    {
      if (deviceIdentification == null)
        throw new Exception("ERROR: DeviceIdentification not set.");
      string[] strArray = new string[6]
      {
        string.Empty + "\n",
        "FirmwareVersion:   ",
        null,
        null,
        null,
        null
      };
      FirmwareVersion firmwareVersionObj = deviceIdentification.FirmwareVersionObj;
      strArray[2] = firmwareVersionObj.Version.ToString("x8");
      strArray[3] = " - (";
      firmwareVersionObj = deviceIdentification.FirmwareVersionObj;
      strArray[4] = firmwareVersionObj.ToString();
      strArray[5] = ")\r";
      return string.Concat(strArray) + "SerialNo.:         " + (deviceIdentification.FullSerialNumber != null ? deviceIdentification.FullSerialNumber.ToString() : "n/a") + "\r" + "Manufacturer:      " + (!string.IsNullOrEmpty(deviceIdentification.ManufacturerName) ? deviceIdentification.ManufacturerName : "n/a") + "\r" + "Medium:            " + (!string.IsNullOrEmpty(deviceIdentification.GetMediumAsText()) ? deviceIdentification.GetMediumAsText() : "n/a") + "\r" + "MeterID:           " + (deviceIdentification.MeterID.HasValue ? deviceIdentification.MeterID.ToString() : "n/a") + "\r" + "SAP OrderNo.:      " + (deviceIdentification.SAP_ProductionOrderNumber != null ? deviceIdentification.SAP_ProductionOrderNumber : "n/a") + "\r";
    }

    private async void ButtonBackUPDevice_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.setInfoText("\n...read and backup identification of connected device ...");
        bool retVal = await this.oFWUpFunc.makeBackUp(new ProgressHandler(new Action<ProgressArg>(this.OnProgress)), new CancellationToken());
        this.TextBoxMeterID.Text = this.oFWUpFunc.deviceIdentificationForBackup.MeterID.ToString();
        this.setInfoText("\n...done.... " + retVal.ToString());
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error:\n" + ex.Message);
      }
    }

    private async void ButtonOverrideDevice_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string MeterID = this.TextBoxMeterID.Text;
        uint uiMeterID = 0;
        bool isMeterID_OK = uint.TryParse(MeterID, out uiMeterID);
        if (!isMeterID_OK && (this.oFWUpFunc.deviceIdentificationForBackup == null || !this.oFWUpFunc.deviceIdentificationForBackup.MeterID.HasValue))
        {
          this.setInfoText("\n...identification for device is not set correctly! (MeterID) ");
        }
        else
        {
          this.setInfoText("\n...writing backup for device with meterid = " + uiMeterID.ToString());
          int num = await this.oFWUpFunc.writeBackUpToDevice(new ProgressHandler(new Action<ProgressArg>(this.OnProgress)), new CancellationToken()) ? 1 : 0;
          this.setInfoText("\n...DONE. ");
          MeterID = (string) null;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error:\n" + ex.Message);
      }
    }

    private void ButtonLoadBackUP_Click(object sender, RoutedEventArgs e)
    {
      this.setInfoText("\n...select Backup from database ...");
      this.oFWUpFunc.showBackUps(this.MeterID.Value);
      if (this.oFWUpFunc.zippedBackUpData != null)
      {
        this.setInfoText("\n...successfully load backup from database ...");
        this.oFWUpFunc.setBackupData();
        this.setInfoText("\n...set backup data for overwrite ...");
        this.ButtonOverrideDevice.IsEnabled = true;
        this.setInfoText("\n...DONE. ");
      }
      else
      {
        this.setInfoText("\n...no backup loaded !!!");
        this.ButtonOverrideDevice.IsEnabled = false;
      }
    }

    private async void ButtonCreatefromArmID_Click(object sender, RoutedEventArgs e)
    {
      this.setInfoText("\n...creating METERID from ArmID ...");
      byte[] ArmID = await this.oFWUpFunc.BSL_getARM_ID(this.progress, this.cancelTokenSource, false);
      this.setInfoText("\nUnique ID: " + Util.ByteArrayToHexString(ArmID));
      uint? nullable = await this.oFWUpFunc.createMeterIDFromARM_ID(this.progress, this.cancelTokenSource, ArmID);
      uint? localMeterID = nullable;
      nullable = new uint?();
      uint num = localMeterID.Value;
      this.setInfoText("\nMeterID: " + num.ToString());
      this.MeterID = localMeterID;
      TextBox textBoxMeterId = this.TextBoxMeterID;
      num = this.MeterID.Value;
      string str = num.ToString();
      textBoxMeterId.Text = str;
      ArmID = (byte[]) null;
    }

    private static bool TryParseHex(string hex, out uint result)
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

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/hardwaremanagement/firmwareupdatefunctionsui.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.menuMain = (Menu) target;
          break;
        case 2:
          this.MenuItemComponents = (MenuItem) target;
          break;
        case 3:
          this.gmmCorporateControl1 = (GmmCorporateControl) target;
          break;
        case 4:
          this.TextBlockStatus = (TextBlock) target;
          break;
        case 5:
          this.ProgressBar1 = (ProgressBar) target;
          break;
        case 6:
          this.GroupBoxConnected = (GroupBox) target;
          break;
        case 7:
          this.TextBoxFWDB = (TextBox) target;
          break;
        case 8:
          this.ComboBoxFWDB = (ComboBox) target;
          break;
        case 9:
          this.TextBoxBSLDB = (TextBox) target;
          break;
        case 10:
          this.ComboBoxBSLDB = (ComboBox) target;
          break;
        case 11:
          this.TextBoxInfo = (TextBox) target;
          break;
        case 12:
          this.ButtonBackUPDevice = (Button) target;
          this.ButtonBackUPDevice.Click += new RoutedEventHandler(this.ButtonBackUPDevice_Click);
          break;
        case 13:
          this.ButtonLoadBackUP = (Button) target;
          this.ButtonLoadBackUP.Click += new RoutedEventHandler(this.ButtonLoadBackUP_Click);
          break;
        case 14:
          this.ButtonOverrideDevice = (Button) target;
          this.ButtonOverrideDevice.Click += new RoutedEventHandler(this.ButtonOverrideDevice_Click);
          break;
        case 15:
          this.TextBoxMeterID = (TextBox) target;
          break;
        case 16:
          this.ButtonCreatefromArmID = (Button) target;
          this.ButtonCreatefromArmID.Click += new RoutedEventHandler(this.ButtonCreatefromArmID_Click);
          break;
        case 17:
          this.StackPanalButtons = (StackPanel) target;
          break;
        case 18:
          this.LabelByteSize = (Label) target;
          break;
        case 19:
          this.TextBoxVerifyReadByteSize = (TextBox) target;
          break;
        case 20:
          this.LabelLoops = (Label) target;
          break;
        case 21:
          this.TextBoxVerifyLoops = (TextBox) target;
          break;
        case 22:
          this.ButtonFWConnect = (Button) target;
          this.ButtonFWConnect.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 23:
          this.ButtonFWReadARMID = (Button) target;
          this.ButtonFWReadARMID.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 24:
          this.ButtonFWLoadFromFile = (Button) target;
          this.ButtonFWLoadFromFile.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 25:
          this.ButtonFWLoadFromDB = (Button) target;
          this.ButtonFWLoadFromDB.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 26:
          this.ButtonFWDeleteOnDevice = (Button) target;
          this.ButtonFWDeleteOnDevice.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 27:
          this.ButtonFWWriteToDevice = (Button) target;
          this.ButtonFWWriteToDevice.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 28:
          this.ButtonFWVerifyOnDevice = (Button) target;
          this.ButtonFWVerifyOnDevice.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 29:
          this.ButtonFWReadFlashSize = (Button) target;
          this.ButtonFWReadFlashSize.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 30:
          this.ButtonFWReadVectorTable = (Button) target;
          this.ButtonFWReadVectorTable.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 31:
          this.ButtonFWWriteVectorTable = (Button) target;
          this.ButtonFWWriteVectorTable.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 32:
          this.ButtonGo = (Button) target;
          this.ButtonGo.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 33:
          this.ButtonFWReadMeterKey = (Button) target;
          this.ButtonFWReadMeterKey.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 34:
          this.LabelRange = (Label) target;
          break;
        case 35:
          this.TextBoxStartRange = (TextBox) target;
          break;
        case 36:
          this.TextBoxEndRange = (TextBox) target;
          break;
        case 37:
          this.ButtonBSLConnect = (Button) target;
          this.ButtonBSLConnect.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 38:
          this.ButtonBSLReadARMID = (Button) target;
          this.ButtonBSLReadARMID.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 39:
          this.ButtonBSLLoadFromFile = (Button) target;
          this.ButtonBSLLoadFromFile.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 40:
          this.ButtonBSLLoadFromDB = (Button) target;
          this.ButtonBSLLoadFromDB.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 41:
          this.ButtonBSLWriteToDevice = (Button) target;
          this.ButtonBSLWriteToDevice.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 42:
          this.ButtonBSLVerifyOnDevice = (Button) target;
          this.ButtonBSLVerifyOnDevice.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 43:
          this.ButtonBSLDeleteOnDevice = (Button) target;
          this.ButtonBSLDeleteOnDevice.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 44:
          this.ButtonBSLOverwriteOnDevice = (Button) target;
          this.ButtonBSLOverwriteOnDevice.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 45:
          this.ButtonBSLReadFlashSize = (Button) target;
          this.ButtonBSLReadFlashSize.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 46:
          this.ButtonBSLResetMeterKey = (Button) target;
          this.ButtonBSLResetMeterKey.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 47:
          this.ButtonBSLReadFirst8Byte = (Button) target;
          this.ButtonBSLReadFirst8Byte.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 48:
          this.ButtonBSLWriteFirst8Byte = (Button) target;
          this.ButtonBSLWriteFirst8Byte.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 49:
          this.ButtonDeviceReset = (Button) target;
          this.ButtonDeviceReset.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 50:
          this.CheckShowDetails = (CheckBox) target;
          break;
        case 51:
          this.CheckIgnoreError = (CheckBox) target;
          break;
        case 52:
          this.ButtonWriteBSLtoDB = (Button) target;
          this.ButtonWriteBSLtoDB.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 53:
          this.ButtonCheckFWworkWithBSL = (Button) target;
          this.ButtonCheckFWworkWithBSL.Click += new RoutedEventHandler(this.ButtonHandlerDoEvent_Click);
          break;
        case 54:
          this.ButtonClear = (Button) target;
          this.ButtonClear.Click += new RoutedEventHandler(this.ButtonClear_Click);
          break;
        case 55:
          this.ButtonBreak = (Button) target;
          this.ButtonBreak.Click += new RoutedEventHandler(this.ButtonBreak_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
