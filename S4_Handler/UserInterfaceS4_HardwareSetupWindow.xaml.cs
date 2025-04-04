// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_HardwareSetupWindow
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using HandlerLib.NFC;
using NLog;
using S4_Handler.Functions;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.UserInterface
{
  public partial class S4_HardwareSetupWindow : Window, IComponentConnector
  {
    private static Logger S4_HandlerTestWindowsLogger = LogManager.GetLogger("S4_HandlerTestWindows");
    private S4_HandlerWindowFunctions myWindowFunctions;
    private S4_HandlerFunctions myFunctions;
    private S4_DeviceCommandsNFC myCommands;
    private Cursor defaultCursor;
    private ProgressHandler progress;
    private CancellationTokenSource cancelTokenSource;
    internal ProgressBar ProgressBar1;
    internal Button ButtonBreak;
    internal GroupBox GroupBoxMeterProtection;
    internal Button ButtonSetProtectionDB;
    internal Button ButtonResetProtectionDB;
    internal TextBox TextBoxMeterKey;
    internal Button ButtonSetProtectionMK;
    internal Button ButtonResetProtectionMK;
    internal Button ButtonReactivateProtection;
    internal Button ButtonGetUnlockPinState;
    internal Button ButtonGenerateAllChecksums;
    internal Button ButtonGenerateFirmwareChecksum;
    internal Button ButtonGenerateParameterChecksum;
    internal Button CheckAllChecksums;
    internal TextBox TextBoxLcdTestStep;
    internal Button ButtonNextLcdTestStep;
    internal Button ButtonSetLcdTestStep;
    internal GroupBox GroupBoxBackupHandling;
    internal Button ButtonReset;
    internal Button ButtonResetAndLoadBackup;
    internal Button ButtonSaveBackup;
    internal Button ButtonForceSaveBackup;
    internal Button ButtonShowBackup;
    internal Button ButtonGetVersion;
    internal Button ButtonSetIdentificationByDB;
    internal TextBox TextBoxDeliveryModeText;
    internal Button ButtonSetDeliveryMode;
    internal Button ButtonSetOperationMode;
    internal Button ButtonSetRadioSimulationMode;
    internal Button ButtonSetStandbyCurrentMode;
    internal Button ButtonReset_first_day_flag;
    internal Button UpdateNdef;
    internal Button ButtonClearEventLogger;
    internal TextBox TextBoxAccumulatedVolume;
    internal TextBox TextBoxAccumulatedFlowVolume;
    internal TextBox TextBoxAccumulatedReturnVolume;
    internal Button ButtonResetDeviceAndSetVolume;
    internal TextBlock TextBlockStatus;
    internal TextBox TextBoxCommandResult;
    private bool _contentLoaded;

    public S4_HardwareSetupWindow(S4_HandlerWindowFunctions myWindowFunctions)
    {
      this.myWindowFunctions = myWindowFunctions;
      this.myFunctions = myWindowFunctions.MyFunctions;
      this.myCommands = this.myFunctions.myDeviceCommands;
      this.InitializeComponent();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.SetStopState();
    }

    private void SetRunState()
    {
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress.Reset();
      this.progress.Split(new double[2]{ 2.0, 98.0 });
      this.progress.Report("Run");
      this.ButtonBreak.IsEnabled = true;
      if (this.Cursor == Cursors.Wait)
        return;
      this.defaultCursor = this.Cursor;
      this.Cursor = Cursors.Wait;
    }

    private void SetStopState()
    {
      this.ButtonBreak.IsEnabled = false;
      this.Cursor = this.defaultCursor;
      this.progress.Reset();
      this.TextBlockStatus.Text = "";
      if (!UserManager.CheckPermission(UserManager.Right_ReadOnly))
        return;
      this.ButtonNextLcdTestStep.IsEnabled = false;
      this.ButtonReactivateProtection.IsEnabled = false;
      this.ButtonResetProtectionDB.IsEnabled = false;
      this.ButtonResetProtectionMK.IsEnabled = false;
      this.ButtonSetDeliveryMode.IsEnabled = false;
      this.ButtonSetIdentificationByDB.IsEnabled = false;
      this.ButtonSetLcdTestStep.IsEnabled = false;
      this.ButtonSetOperationMode.IsEnabled = false;
      this.ButtonSetProtectionDB.IsEnabled = false;
      this.ButtonSetProtectionMK.IsEnabled = false;
      this.ButtonSetStandbyCurrentMode.IsEnabled = false;
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
        if (obj.Tag != null && obj.Tag.GetType() == typeof (string))
        {
          string tag = (string) obj.Tag;
          if (this.TextBoxCommandResult.Text.Length == 0)
            this.TextBoxCommandResult.AppendText(tag);
          else
            this.TextBoxCommandResult.AppendText(Environment.NewLine + tag);
          if (this.TextBoxCommandResult.Text.Length > 10000)
          {
            int start = this.TextBoxCommandResult.Text.IndexOf(Environment.NewLine);
            int num = this.TextBoxCommandResult.Text.IndexOf(Environment.NewLine, 1000);
            if (start > 0 && num > 0)
            {
              int length = num - start;
              this.TextBoxCommandResult.Select(start, length);
              this.TextBoxCommandResult.SelectedText = "";
            }
          }
          this.TextBoxCommandResult.ScrollToEnd();
        }
        else
          this.TextBlockStatus.Text = obj.Message;
      }
    }

    private void ButtonBreak_Click(object sender, RoutedEventArgs e)
    {
      this.cancelTokenSource.Cancel();
    }

    private async void ButtonX_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      try
      {
        if (sender == this.ButtonGetVersion)
        {
          DeviceIdentification versionResult = await this.myCommands.ReadVersionAsync(this.progress, this.cancelTokenSource.Token);
          this.TextBoxCommandResult.Text = versionResult.ToString();
          versionResult = (DeviceIdentification) null;
        }
        else if (sender == this.ButtonGetUnlockPinState)
        {
          bool unlockPinState = await this.myFunctions.GetUnlockPinState(this.progress, this.cancelTokenSource.Token);
          this.TextBoxCommandResult.Text = !unlockPinState ? "Unlock pin is not pressed" : "Unlock pin is pressed";
        }
        else if (sender == this.ButtonSetIdentificationByDB)
        {
          if (this.myFunctions.myDb.ConnectionInfo.DbType != MeterDbTypes.MSSQL || !this.myFunctions.myDb.ConnectionInfo.UrlOrPath.Contains("dev."))
            throw new Exception("Identification by DB only possible on developer databases");
          int num1 = await this.myFunctions.ReadDeviceAsync(this.progress, this.cancelTokenSource.Token, ReadPartsSelection.Identification | ReadPartsSelection.EnhancedIdentification);
          S4_DeviceIdentification theIdent = (S4_DeviceIdentification) this.myFunctions.GetDeviceIdentification();
          if (theIdent.Unique_ID == null)
          {
            this.TextBoxCommandResult.Text = "ARM Unique_ID not defined";
          }
          else
          {
            MeterUniqueIdByARM idManager = new MeterUniqueIdByARM(this.myFunctions.myDb);
            uint? tempMeterID = new uint?();
            bool newCreated = idManager.ManageMeterID(theIdent.Unique_ID, ref tempMeterID);
            StringBuilder theMessage = new StringBuilder();
            if (newCreated)
              theMessage.AppendLine("The ArmID is new on this database. It is registerd now and a new MeterID is created.");
            else
              theMessage.AppendLine("The ID's exists on the databse. The existing MeterID is loaded form database und used for this device.");
            int num2 = (int) tempMeterID.Value;
            uint? meterId = theIdent.MeterID;
            int num3 = (int) meterId.Value;
            uint num4;
            if (num2 != num3)
            {
              StringBuilder stringBuilder = theMessage;
              meterId = theIdent.MeterID;
              string str1 = meterId.Value.ToString();
              num4 = tempMeterID.Value;
              string str2 = num4.ToString();
              string str3 = "MeterID of this device is changed from: " + str1 + " to " + str2;
              stringBuilder.AppendLine(str3);
            }
            theIdent.MeterID = new uint?(tempMeterID.Value);
            if (theIdent.IdentificationPrefix == null || theIdent.IdentificationPrefix.Length != 6 || theIdent.IdentificationPrefix.Substring(1, 3) != "ZRI" || theIdent.PrintedSerialNumberAsString == null || theIdent.PrintedSerialNumberAsString.Length != 14 && theIdent.PrintedSerialNumberAsString.Length != 16)
            {
              theMessage.AppendLine("Not possible to check or create a serial number. Please programm the SAP type first by SAPcache");
            }
            else
            {
              using (DbConnection MyConnection = this.myFunctions.myDb.GetNewConnection())
              {
                BaseTables.MeterDataTable meterDataTable = new BaseTables.MeterDataTable();
                meterId = theIdent.MeterID;
                num4 = meterId.Value;
                string SqlString = "SELECT * FROM Meter WHERE MeterID = " + num4.ToString();
                DbCommandBuilder myCommandBuilder;
                DbDataAdapter meterDataAdapter = this.myFunctions.myDb.GetDataAdapter(SqlString, MyConnection, out myCommandBuilder);
                meterDataAdapter.Fill((DataTable) meterDataTable);
                if (meterDataTable.Count != 1)
                  throw new Exception("MeterID searching and generation problem.");
                bool createNewSerialNumber = false;
                uint numberPart = 0;
                if (meterDataTable[0].IsSerialNrNull() || meterDataTable[0].SerialNr.Length < 14 || meterDataTable[0].SerialNr.Contains("DEV") || meterDataTable[0].SerialNr.EndsWith("00000000"))
                {
                  createNewSerialNumber = true;
                }
                else
                {
                  theMessage.AppendLine("Serial number in tabel Meter: " + meterDataTable[0].SerialNr);
                  numberPart = uint.Parse(meterDataTable[0].SerialNr.Substring(meterDataTable[0].SerialNr.Length - 8));
                  theMessage.AppendLine("Serial number part loaded from Meter table: " + numberPart.ToString("d08"));
                }
                bool? twoTransducerChannels = theIdent.IsTwoTransducerChannels;
                if (!twoTransducerChannels.HasValue)
                {
                  theMessage.AppendLine("Not possible to create a serial number. Please read the device first.");
                }
                else
                {
                  if (createNewSerialNumber)
                  {
                    if (twoTransducerChannels.Value)
                    {
                      numberPart = (uint) this.myFunctions.myDb.GetNewId("SerialNumber_IUW");
                      theMessage.AppendLine("Serial number created by SerialNumber_IUW: " + numberPart.ToString("d08"));
                    }
                    else
                    {
                      numberPart = (uint) this.myFunctions.myDb.GetNewId("SerialNumber_IUWS");
                      theMessage.AppendLine("Serial number created by SerialNumber_IUWS: " + numberPart.ToString("d08"));
                    }
                  }
                  theIdent.FullSerialNumber = theIdent.IdentificationPrefix + numberPart.ToString("d08");
                  theIdent.LoRa_DevEUI = new ulong?(new IdentificationMapping(theIdent.FullSerialNumber).GetAsDevEUI_Value());
                  theIdent.Set_FD_Values();
                  if (theIdent.PrintedSerialNumberAsString.Length == 14)
                    theIdent.PrintedSerialNumberAsString = theIdent.FullSerialNumber;
                  else
                    theIdent.PrintedSerialNumberAsString = theIdent.LoRa_DevEUI_AsString;
                  if (meterDataTable[0].IsSerialNrNull() || theIdent.PrintedSerialNumberAsString != meterDataTable[0].SerialNr)
                  {
                    meterDataTable[0].SerialNr = theIdent.PrintedSerialNumberAsString;
                    meterDataAdapter.Update((DataTable) meterDataTable);
                    theMessage.AppendLine("Serial number updated in table Meter");
                  }
                  else
                    theMessage.AppendLine("Serial number in table Meter is ok. Not changed");
                }
                meterDataTable = (BaseTables.MeterDataTable) null;
                SqlString = (string) null;
                myCommandBuilder = (DbCommandBuilder) null;
                meterDataAdapter = (DbDataAdapter) null;
                twoTransducerChannels = new bool?();
              }
            }
            await this.myFunctions.WriteDeviceAsync(this.progress, this.cancelTokenSource.Token);
            int num5 = await this.myFunctions.ReadDeviceAsync(this.progress, this.cancelTokenSource.Token, ReadPartsSelection.Identification);
            this.TextBoxCommandResult.Text = theMessage.ToString();
            theIdent = (S4_DeviceIdentification) null;
            idManager = (MeterUniqueIdByARM) null;
            tempMeterID = new uint?();
            theMessage = (StringBuilder) null;
          }
        }
        else if (sender == this.ButtonSetLcdTestStep)
        {
          byte testStep;
          if (!byte.TryParse(this.TextBoxLcdTestStep.Text, out testStep) || testStep > (byte) 6)
          {
            int num = (int) MessageBox.Show("Illegal test state. Allowed range 0..6");
          }
          else
            await this.myFunctions.SetLcdTestStateAsync(this.progress, this.cancelTokenSource.Token, testStep);
        }
        else if (sender == this.ButtonNextLcdTestStep)
        {
          byte testStep;
          if (!byte.TryParse(this.TextBoxLcdTestStep.Text, out testStep))
          {
            int num = (int) MessageBox.Show("Illegal test state. Allowed range 0..6");
          }
          else
          {
            ++testStep;
            if (testStep > (byte) 6)
              testStep = (byte) 0;
            this.TextBoxLcdTestStep.Text = testStep.ToString();
            await this.myFunctions.SetLcdTestStateAsync(this.progress, this.cancelTokenSource.Token, testStep);
          }
        }
        else if (sender == this.ButtonSetDeliveryMode)
        {
          string lcdText = this.TextBoxDeliveryModeText.Text.Trim();
          if (lcdText.Length > 0)
          {
            byte[] asciiString = lcdText.Length <= 4 ? Encoding.ASCII.GetBytes(lcdText) : throw new Exception("Maximal 4 characters alowed");
            await this.myFunctions.SetModeAsync(this.progress, this.cancelTokenSource.Token, (Enum) HandlerFunctionsForProduction.CommonDeviceModes.DeliveryMode, arbitraryData: asciiString);
            asciiString = (byte[]) null;
          }
          else
            await this.myFunctions.SetModeAsync(this.progress, this.cancelTokenSource.Token, (Enum) HandlerFunctionsForProduction.CommonDeviceModes.DeliveryMode, (byte) 0, (ushort) 10, (ushort) 0, 305419896U, (ushort) 37331, (byte[]) null);
          this.TextBoxCommandResult.Text = "Sleep mode activated";
          lcdText = (string) null;
        }
        else if (sender == this.ButtonSetOperationMode)
        {
          await this.myFunctions.SetModeAsync(this.progress, this.cancelTokenSource.Token, (Enum) HandlerFunctionsForProduction.CommonDeviceModes.OperationMode, (byte) 0, (ushort) 10, (ushort) 0, 305419896U, (ushort) 37331, (byte[]) null);
          this.TextBoxCommandResult.Text = "Operation mode activated";
        }
        else if (sender == this.ButtonSetRadioSimulationMode)
        {
          await this.myFunctions.SetModeAsync(this.progress, this.cancelTokenSource.Token, (Enum) S4_DeviceModes.RadioTestRadioSimulation, (byte) 0, (ushort) 10, (ushort) 0, 305419896U, (ushort) 37331, (byte[]) null);
          this.TextBoxCommandResult.Text = "Radio simulation mode activated";
        }
        else if (sender == this.ButtonReset_first_day_flag)
        {
          await this.myFunctions.checkedCommands.Delete_wMBus_first_day_flag(this.progress, this.cancelTokenSource.Token, this.myFunctions.myMeters.ConnectedMeter.meterMemory);
          this.TextBoxCommandResult.Text = "first_day_flag set to 0";
        }
        else if (sender == this.ButtonSetStandbyCurrentMode)
          await this.myFunctions.SetModeAsync(this.progress, this.cancelTokenSource.Token, (Enum) HandlerFunctionsForProduction.CommonDeviceModes.StandbyCurrentMode, (byte) 0, (ushort) 10, (ushort) 0, 305419896U, (ushort) 37331, (byte[]) null);
        else if (sender == this.ButtonClearEventLogger)
        {
          await this.myCommands.CommonNfcCommands.ClearEventLogger(this.progress, this.cancelTokenSource.Token);
          this.TextBoxCommandResult.Text = "Cleare event logger done.";
        }
        else if (sender == this.ButtonResetDeviceAndSetVolume)
        {
          List<double> values = new List<double>();
          try
          {
            values.Add(double.Parse(this.TextBoxAccumulatedVolume.Text));
            if (this.TextBoxAccumulatedFlowVolume.Text.Length > 0 && this.TextBoxAccumulatedReturnVolume.Text.Length > 0)
            {
              values.Add(double.Parse(this.TextBoxAccumulatedFlowVolume.Text));
              values.Add(double.Parse(this.TextBoxAccumulatedReturnVolume.Text));
            }
            else
            {
              this.TextBoxAccumulatedFlowVolume.Clear();
              this.TextBoxAccumulatedReturnVolume.Clear();
            }
          }
          catch (Exception ex)
          {
            throw new Exception("Illegal double values", ex);
          }
          await this.myCommands.CommonNfcCommands.SetAccumulatedValues(this.progress, this.cancelTokenSource.Token, values);
          this.TextBoxCommandResult.Text = "Set accumulated velues and reset loggers done.";
          values = (List<double>) null;
        }
        else
          this.TextBoxCommandResult.Text = "Not supported button";
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (TimeoutException ex)
      {
        this.TextBoxCommandResult.Text = "*** Timeout ***" + Environment.NewLine + ex.Message;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.TextBoxCommandResult.Text = "*** Exception ***" + Environment.NewLine + ex.Message;
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void ButtonProtection_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      try
      {
        if (sender == this.ButtonSetProtectionDB)
        {
          await this.myFunctions.ProtectionSetByDb(this.progress, this.cancelTokenSource.Token);
          this.AddCommendResult("Set protection by database keys: done");
        }
        else if (sender == this.ButtonResetProtectionDB)
        {
          await this.myFunctions.ProtectionResetByDb(this.progress, this.cancelTokenSource.Token, (BaseDbConnection) null);
          this.AddCommendResult("Reset protection by database keys: done");
        }
        else if (sender == this.ButtonSetProtectionMK)
        {
          uint MeterKey;
          if (!uint.TryParse(this.TextBoxMeterKey.Text, out MeterKey) || MeterKey == 0U)
            throw new Exception("Illegal MeterKey");
          uint? meterId = this.myFunctions.myMeters.ConnectedMeter.deviceIdentification.MeterID;
          int num1;
          if (meterId.HasValue)
          {
            meterId = this.myFunctions.myMeters.ConnectedMeter.deviceIdentification.MeterID;
            uint num2 = 0;
            num1 = (int) meterId.GetValueOrDefault() == (int) num2 & meterId.HasValue ? 1 : 0;
          }
          else
            num1 = 1;
          if (num1 != 0)
            throw new Exception("Illegal data. Set protection is only possible by MeterID > 0");
          await this.myFunctions.checkedNfcCommands.LockDevice(MeterKey, this.progress, this.cancelTokenSource.Token);
          this.AddCommendResult("Set protection by typed MeterKey done");
        }
        else if (sender == this.ButtonResetProtectionMK)
        {
          uint MeterKey;
          if (!uint.TryParse(this.TextBoxMeterKey.Text, out MeterKey) || MeterKey == 0U)
            throw new Exception("Illegal MeterKey");
          await this.myFunctions.checkedNfcCommands.UnlockDevice(MeterKey, this.progress, this.cancelTokenSource.Token);
          this.AddCommendResult("Clear protection by typed MeterKey: done");
        }
        else if (sender == this.ButtonReactivateProtection)
        {
          await this.myFunctions.ProtectionSetAgainAsync(this.progress, this.cancelTokenSource.Token);
          this.AddCommendResult("Protection activated: done");
        }
        else if (sender == this.UpdateNdef)
        {
          byte[] resultAsync = await this.myFunctions.myDeviceCommands.CommonNfcCommands.SendCommandAndGetResultAsync(this.progress, this.cancelTokenSource.Token, NfcCommands.UpdateNdef);
          this.TextBoxCommandResult.Text = "Update NFC tag data: done";
        }
        else if (sender == this.ButtonGenerateAllChecksums)
        {
          await this.myFunctions.GenerateAllChecksums(this.progress, this.cancelTokenSource.Token);
          this.AddCommendResult();
          this.AddCommendResult("Generate all checksums: done");
        }
        else if (sender == this.ButtonGenerateFirmwareChecksum)
        {
          await this.myFunctions.GenerateFirmwareChecksums(this.progress, this.cancelTokenSource.Token);
          this.AddCommendResult("Generate firmware checksum: done");
        }
        else if (sender == this.ButtonGenerateParameterChecksum)
        {
          await this.myFunctions.GenerateParameterChecksum(this.progress, this.cancelTokenSource.Token);
          this.AddCommendResult("Generate parameter checksum: done");
        }
        else if (sender == this.CheckAllChecksums)
        {
          S4_ChecksumCheckResults result = await this.myFunctions.CheckAllChecksums(this.progress, this.cancelTokenSource.Token);
          this.AddCommendResult();
          this.AddCommendResult("Check all checksums: done");
          this.AddCommendResult();
          if ((result & S4_ChecksumCheckResults.IdentityChecksumNotOk) == (S4_ChecksumCheckResults) 0)
            this.AddCommendResult("Identification checksum Ok");
          else
            this.AddCommendResult("!!! Identification checksum not ok !!!!!!!");
          if ((result & S4_ChecksumCheckResults.ParameterChecksumNotOk) == (S4_ChecksumCheckResults) 0)
            this.AddCommendResult("Parameter checksum Ok");
          else
            this.AddCommendResult("!!! Parameter checksum not Ok !!!!!!!");
          if ((result & S4_ChecksumCheckResults.FirmwareMetrologicalPartNotOk) == (S4_ChecksumCheckResults) 0)
            this.AddCommendResult("Firmware checksum metrological part Ok");
          else
            this.AddCommendResult("!!! Firmware checksum metrological part not ok !!!!!!!");
          if ((result & S4_ChecksumCheckResults.FirmwareCommunicationPartNotOk) == (S4_ChecksumCheckResults) 0)
            this.AddCommendResult("Firmware checksum communication part Ok");
          else
            this.AddCommendResult("!!! Firmware checksum communication part not ok !!!!!!!");
          if ((result & S4_ChecksumCheckResults.FirmwareCompleteNotOk) == (S4_ChecksumCheckResults) 0)
            this.AddCommendResult("Firmware checksum complete Ok");
          else
            this.AddCommendResult("!!! Firmware checksum complete not ok !!!!!!!");
        }
        else
          this.AddCommendResult("Not supported button");
      }
      catch (OperationCanceledException ex)
      {
        this.TextBoxCommandResult.Text = "Canceled";
      }
      catch (TimeoutException ex)
      {
        this.TextBoxCommandResult.Text = "*** Timeout ***" + Environment.NewLine + ex.Message;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.TextBoxCommandResult.Text = "*** Exception ***" + Environment.NewLine + ex.Message;
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void ButtonResetAndBackup_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      Stopwatch sw = new Stopwatch();
      sw.Start();
      try
      {
        if (sender == this.ButtonReset)
        {
          await this.myFunctions.ResetDeviceAsync(this.progress, this.cancelTokenSource.Token);
          sw.Stop();
          this.AddCommendResult("Software reset: done. Time:" + sw.ElapsedMilliseconds.ToString() + "ms");
          sw = (Stopwatch) null;
        }
        else if (sender == this.ButtonResetAndLoadBackup)
        {
          await this.myFunctions.ResetDeviceAsync(this.progress, this.cancelTokenSource.Token, true);
          sw.Stop();
          this.AddCommendResult("Software reset and load backup: done. Time:" + sw.ElapsedMilliseconds.ToString() + "ms");
          sw = (Stopwatch) null;
        }
        else if (sender == this.ButtonSaveBackup)
        {
          await this.myFunctions.BackupDeviceAsync(this.progress, this.cancelTokenSource.Token);
          sw.Stop();
          this.AddCommendResult("Save backup: done. Time:" + sw.ElapsedMilliseconds.ToString() + "ms");
          sw = (Stopwatch) null;
        }
        else if (sender == this.ButtonForceSaveBackup)
        {
          S4_DeviceMemory deviceMemory = this.myFunctions.myMeters.checkedConnectedMeter.meterMemory;
          uint firstByteAddress = deviceMemory.FlashBackupRange.StartAddress;
          byte[] firstByte = await this.myFunctions.myDeviceCommands.CommonNfcCommands.ReadMemoryAsync(this.progress, this.cancelTokenSource.Token, firstByteAddress, 1U);
          firstByte[0] = ~firstByte[0];
          await this.myFunctions.myDeviceCommands.CommonNfcCommands.WriteMemoryAsync(this.progress, this.cancelTokenSource.Token, firstByteAddress, firstByte);
          firstByteAddress = deviceMemory.FlashBackupRange.StartAddress + deviceMemory.FlashBackupRange.ByteSize / 2U;
          firstByte = await this.myFunctions.myDeviceCommands.CommonNfcCommands.ReadMemoryAsync(this.progress, this.cancelTokenSource.Token, firstByteAddress, 1U);
          firstByte[0] = ~firstByte[0];
          await this.myFunctions.myDeviceCommands.CommonNfcCommands.WriteMemoryAsync(this.progress, this.cancelTokenSource.Token, firstByteAddress, firstByte);
          await this.myFunctions.BackupDeviceAsync(this.progress, this.cancelTokenSource.Token);
          sw.Stop();
          this.AddCommendResult("Force save backup: done. Time:" + sw.ElapsedMilliseconds.ToString() + "ms");
          deviceMemory = (S4_DeviceMemory) null;
          firstByte = (byte[]) null;
          sw = (Stopwatch) null;
        }
        else if (sender == this.ButtonShowBackup)
        {
          this.TextBoxCommandResult.Text = "";
          S4_Meter cm = this.myFunctions.myMeters.checkedConnectedMeter;
          AddressRange flashBackUpRange = cm.meterMemory.FlashBackupRange;
          cm.meterMemory.GarantMemoryAvailable(flashBackUpRange);
          await this.myFunctions.nfcCmd.ReadMemoryAsync(flashBackUpRange, (DeviceMemory) cm.meterMemory, this.progress, this.cancelTokenSource.Token);
          string dataInHEX = cm.GetBackupData();
          sw.Stop();
          this.AddCommendResult("Show backup: done. Time:" + sw.ElapsedMilliseconds.ToString() + "ms");
          if (!string.IsNullOrEmpty(dataInHEX))
            this.AddCommendResult(dataInHEX);
          cm = (S4_Meter) null;
          flashBackUpRange = (AddressRange) null;
          dataInHEX = (string) null;
          sw = (Stopwatch) null;
        }
        else
        {
          this.TextBoxCommandResult.Text = "Not supported button";
          sw = (Stopwatch) null;
        }
      }
      catch (OperationCanceledException ex)
      {
        sw = (Stopwatch) null;
      }
      catch (TimeoutException ex)
      {
        this.TextBoxCommandResult.Text = "*** Timeout ***" + Environment.NewLine + ex.Message;
        sw = (Stopwatch) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.TextBoxCommandResult.Text = "*** Exception ***" + Environment.NewLine + ex.Message;
        sw = (Stopwatch) null;
      }
      finally
      {
        this.SetStopState();
      }
    }

    private void AddCommendResult(string result = "")
    {
      if (this.TextBoxCommandResult.Text.Length > 0)
        this.TextBoxCommandResult.AppendText(Environment.NewLine + result);
      else
        this.TextBoxCommandResult.AppendText(result);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_hardwaresetupwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ProgressBar1 = (ProgressBar) target;
          break;
        case 2:
          this.ButtonBreak = (Button) target;
          this.ButtonBreak.Click += new RoutedEventHandler(this.ButtonBreak_Click);
          break;
        case 3:
          this.GroupBoxMeterProtection = (GroupBox) target;
          break;
        case 4:
          this.ButtonSetProtectionDB = (Button) target;
          this.ButtonSetProtectionDB.Click += new RoutedEventHandler(this.ButtonProtection_Click);
          break;
        case 5:
          this.ButtonResetProtectionDB = (Button) target;
          this.ButtonResetProtectionDB.Click += new RoutedEventHandler(this.ButtonProtection_Click);
          break;
        case 6:
          this.TextBoxMeterKey = (TextBox) target;
          break;
        case 7:
          this.ButtonSetProtectionMK = (Button) target;
          this.ButtonSetProtectionMK.Click += new RoutedEventHandler(this.ButtonProtection_Click);
          break;
        case 8:
          this.ButtonResetProtectionMK = (Button) target;
          this.ButtonResetProtectionMK.Click += new RoutedEventHandler(this.ButtonProtection_Click);
          break;
        case 9:
          this.ButtonReactivateProtection = (Button) target;
          this.ButtonReactivateProtection.Click += new RoutedEventHandler(this.ButtonProtection_Click);
          break;
        case 10:
          this.ButtonGetUnlockPinState = (Button) target;
          this.ButtonGetUnlockPinState.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 11:
          this.ButtonGenerateAllChecksums = (Button) target;
          this.ButtonGenerateAllChecksums.Click += new RoutedEventHandler(this.ButtonProtection_Click);
          break;
        case 12:
          this.ButtonGenerateFirmwareChecksum = (Button) target;
          this.ButtonGenerateFirmwareChecksum.Click += new RoutedEventHandler(this.ButtonProtection_Click);
          break;
        case 13:
          this.ButtonGenerateParameterChecksum = (Button) target;
          this.ButtonGenerateParameterChecksum.Click += new RoutedEventHandler(this.ButtonProtection_Click);
          break;
        case 14:
          this.CheckAllChecksums = (Button) target;
          this.CheckAllChecksums.Click += new RoutedEventHandler(this.ButtonProtection_Click);
          break;
        case 15:
          this.TextBoxLcdTestStep = (TextBox) target;
          break;
        case 16:
          this.ButtonNextLcdTestStep = (Button) target;
          this.ButtonNextLcdTestStep.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 17:
          this.ButtonSetLcdTestStep = (Button) target;
          this.ButtonSetLcdTestStep.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 18:
          this.GroupBoxBackupHandling = (GroupBox) target;
          break;
        case 19:
          this.ButtonReset = (Button) target;
          this.ButtonReset.Click += new RoutedEventHandler(this.ButtonResetAndBackup_Click);
          break;
        case 20:
          this.ButtonResetAndLoadBackup = (Button) target;
          this.ButtonResetAndLoadBackup.Click += new RoutedEventHandler(this.ButtonResetAndBackup_Click);
          break;
        case 21:
          this.ButtonSaveBackup = (Button) target;
          this.ButtonSaveBackup.Click += new RoutedEventHandler(this.ButtonResetAndBackup_Click);
          break;
        case 22:
          this.ButtonForceSaveBackup = (Button) target;
          this.ButtonForceSaveBackup.Click += new RoutedEventHandler(this.ButtonResetAndBackup_Click);
          break;
        case 23:
          this.ButtonShowBackup = (Button) target;
          this.ButtonShowBackup.Click += new RoutedEventHandler(this.ButtonResetAndBackup_Click);
          break;
        case 24:
          this.ButtonGetVersion = (Button) target;
          this.ButtonGetVersion.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 25:
          this.ButtonSetIdentificationByDB = (Button) target;
          this.ButtonSetIdentificationByDB.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 26:
          this.TextBoxDeliveryModeText = (TextBox) target;
          break;
        case 27:
          this.ButtonSetDeliveryMode = (Button) target;
          this.ButtonSetDeliveryMode.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 28:
          this.ButtonSetOperationMode = (Button) target;
          this.ButtonSetOperationMode.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 29:
          this.ButtonSetRadioSimulationMode = (Button) target;
          this.ButtonSetRadioSimulationMode.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 30:
          this.ButtonSetStandbyCurrentMode = (Button) target;
          this.ButtonSetStandbyCurrentMode.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 31:
          this.ButtonReset_first_day_flag = (Button) target;
          this.ButtonReset_first_day_flag.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 32:
          this.UpdateNdef = (Button) target;
          this.UpdateNdef.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 33:
          this.ButtonClearEventLogger = (Button) target;
          this.ButtonClearEventLogger.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 34:
          this.TextBoxAccumulatedVolume = (TextBox) target;
          break;
        case 35:
          this.TextBoxAccumulatedFlowVolume = (TextBox) target;
          break;
        case 36:
          this.TextBoxAccumulatedReturnVolume = (TextBox) target;
          break;
        case 37:
          this.ButtonResetDeviceAndSetVolume = (Button) target;
          this.ButtonResetDeviceAndSetVolume.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 38:
          this.TextBlockStatus = (TextBlock) target;
          break;
        case 39:
          this.TextBoxCommandResult = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
