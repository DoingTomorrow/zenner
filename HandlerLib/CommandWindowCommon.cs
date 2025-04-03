// Decompiled with JetBrains decompiler
// Type: HandlerLib.CommandWindowCommon
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class CommandWindowCommon : Window, IComponentConnector
  {
    private Common32BitCommands myCommonCommands;
    private List<string> Argument1_last_values;
    private List<string> Argument2_last_values;
    private List<string> Argument3_last_values;
    private List<string> Argument4_last_values;
    private ContextMenu Argument1ValuesMenu;
    private ContextMenu Argument2ValuesMenu;
    private ContextMenu Argument3ValuesMenu;
    private ContextMenu Argument4ValuesMenu;
    private string result = string.Empty;
    private Cursor defaultCursor;
    private CancellationTokenSource cancelTokenSource;
    private byte LCDStateActualTestByte;
    private byte LCDStateStartByte;
    private ProgressHandler progress;
    private static readonly string CMD_GetVersion = "GetVersion";
    private static readonly string CMD_Resetdevice = "Reset device (0x80)";
    private static readonly string CMD_SetWriteProtection = "Set Write Protection (0x81)";
    private static readonly string CMD_OpenWriteProtectionTemporaly = "Open Write Protection Temporarily(0x82)";
    private static readonly string CMD_SetMode = "Set Mode (0x83)";
    private static readonly string CMD_GetMode = "Get Mode (0x83)";
    private static readonly string CMD_RunBackup = "Run Backup (0x86)";
    private static readonly string CMD_SetSystemTime = "Set System Time (0x87)";
    private static readonly string CMD_GetSystemTime = "Get System Time (0x87)";
    private static readonly string CMD_SetKeyDate = "Set KeyDate (0x88)";
    private static readonly string CMD_GetKeyDate = "Get KeyDate (0x88)";
    private static readonly string CMD_SetRadioOperation = "Set RadioOperation (0x89)";
    private static readonly string CMD_GetRadioOperation = "Get RadioOperation (0x89)";
    private static readonly string CMD_ClearResetCounter = "Clear ResetCounter (0x8a)";
    private static readonly string CMD_GetResetCounter = "Get ResetCounter (0x8a)";
    private static readonly string CMD_SetLcdTestState = "Set LCD Test State (0x8b)";
    private static readonly string CMD_SwitchLoRaLED = "Switch LoRa LED (0x8c)";
    private static readonly string CMD_ActivateDeactivateDisplay = "Activate/Deactivate Display (0x8d)";
    private static readonly string CMD_TimeShift = "TimeShift (0x8e)";
    private static readonly string CMD_ExecuteEvent = "Execute Event (0x8f)";
    private static readonly string CMD_SetRTCCalibration = "SetRTCCalibration (0x90)";
    private static readonly string CMD_GetCommunicationScenario = "GetCommunicationScenario (0x91)";
    private static readonly string CMD_SetCommunicationScenario = "SetCommunicationScenario (0x92)";
    private static readonly string CMD_GetPrintedSerialNumber = "GetPrintedSerialNumber (0x93)";
    private static readonly string CMD_GetLocalInterfaceEncryption = "GetLocalInterfaceEncryption (0x94)";
    private static readonly string CMD_SetLocalInterfaceEncryption = "SetLocalInterfaceEncryption (0x94)";
    private static readonly string CMD_ReadMemory = "ReadMemory";
    internal TextBlock TextBlockStatus;
    internal TextBox TextBoxUniversalCommandResult;
    internal StackPanel StackPanalButtons;
    internal Label FunctionCode_Label;
    internal ComboBox ComboCommand;
    internal CheckBox CheckBoxEncryption;
    internal Label EncryptionKey_Label;
    internal TextBox TextBoxEncryptionKey;
    internal Label ComboAddCommand_Label;
    internal ComboBox ComboAddCommand;
    internal Label TextArgument_1_Label;
    internal TextBox TextCommandArgument_1;
    internal Label TextArgument_2_Label;
    internal TextBox TextCommandArgument_2;
    internal Label TextArgument_3_Label;
    internal TextBox TextCommandArgument_3;
    internal Label TextArgument_4_Label;
    internal TextBox TextCommandArgument_4;
    internal StackPanel StackPanalButtons2;
    internal Button ButtonRunCommand;
    internal Button ButtonRunCommandPreview;
    internal Button ButtonBreak;
    internal ProgressBar ProgressBar1;
    private bool _contentLoaded;

    public CommandWindowCommon(Common32BitCommands myWindowFunctions)
    {
      this.myCommonCommands = myWindowFunctions;
      this.InitializeComponent();
      this.setCommonCommands();
      this.SetArgumentFields((Dictionary<int, string>) null);
      this.Argument1_last_values = new List<string>();
      this.Argument2_last_values = new List<string>();
      this.Argument3_last_values = new List<string>();
      this.Argument4_last_values = new List<string>();
      this.Argument1ValuesMenu = new ContextMenu();
      this.Argument2ValuesMenu = new ContextMenu();
      this.Argument3ValuesMenu = new ContextMenu();
      this.Argument4ValuesMenu = new ContextMenu();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.setCryptState();
    }

    private void setCryptState()
    {
      this.CheckBoxEncryption.IsChecked = new bool?(this.myCommonCommands.enDeCrypt);
      if (!this.myCommonCommands.enDeCrypt)
        this.CheckBoxEncryption_UnChecked((object) null, (RoutedEventArgs) null);
      this.TextBoxEncryptionKey.Text = this.myCommonCommands.AES_Key;
    }

    private void SetRunState()
    {
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress.Reset();
      this.progress.Split(new double[2]{ 2.0, 98.0 });
      this.progress.Report("Run");
      this.ComboCommand.IsEnabled = false;
      this.TextBoxUniversalCommandResult.IsEnabled = false;
      this.ButtonRunCommand.IsEnabled = false;
      this.ButtonRunCommandPreview.IsEnabled = false;
      this.ButtonBreak.IsEnabled = true;
      this.TextBoxEncryptionKey.IsEnabled = false;
      this.CheckBoxEncryption.IsEnabled = false;
      this.defaultCursor = this.Cursor;
      this.Cursor = Cursors.Wait;
    }

    private void SetStopState()
    {
      this.ComboCommand.IsEnabled = true;
      this.TextBoxUniversalCommandResult.IsEnabled = true;
      this.ButtonRunCommand.IsEnabled = true;
      this.ButtonRunCommandPreview.IsEnabled = true;
      this.ButtonBreak.IsEnabled = false;
      this.Cursor = this.defaultCursor;
      this.TextBoxEncryptionKey.IsEnabled = true;
      this.CheckBoxEncryption.IsEnabled = true;
      this.Cursor = this.defaultCursor;
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

    private async Task<byte[]> RunDeviceCommend(CommandWindowCommon.DeviceCommand theCommand)
    {
      byte[] result = (byte[]) null;
      this.SetRunState();
      try
      {
        result = await theCommand(this.progress, this.cancelTokenSource.Token);
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (TimeoutException ex)
      {
        int num = (int) MessageBox.Show("Timeout");
      }
      catch (Exception ex)
      {
        bool isTimeout = false;
        if (ex is AggregateException)
        {
          AggregateException aex = ex as AggregateException;
          for (int i = 0; i < aex.InnerExceptions.Count; ++i)
          {
            Exception theException = aex.InnerExceptions[i];
            if (theException is TimeoutException)
            {
              if (i == aex.InnerExceptions.Count - 1)
              {
                isTimeout = true;
                int num = (int) MessageBox.Show("**** Multiple timouts ****" + Environment.NewLine + "Timeout count: " + aex.InnerExceptions.Count.ToString());
              }
              theException = (Exception) null;
            }
            else
              break;
          }
          aex = (AggregateException) null;
        }
        if (!isTimeout)
        {
          int num1 = (int) MessageBox.Show(ex.ToString());
        }
      }
      this.SetStopState();
      byte[] numArray = result;
      result = (byte[]) null;
      return numArray;
    }

    private async Task RunCommandFrame()
    {
      this.SetRunState();
      try
      {
        await this.RunCommand();
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (TimeoutException ex)
      {
        this.TextBoxUniversalCommandResult.Text = "Timeout";
      }
      catch (NACK_Exception ex)
      {
        this.TextBoxUniversalCommandResult.Text = "Device response is NACK: " + ex.Message;
      }
      catch (Exception ex)
      {
        bool isTimeout = false;
        if (ex is AggregateException)
        {
          AggregateException aex = ex as AggregateException;
          int num1;
          for (int i = 0; i < aex.InnerExceptions.Count; num1 = i++)
          {
            Exception theException = aex.InnerExceptions[i];
            if (theException is TimeoutException)
            {
              if (i == aex.InnerExceptions.Count - 1)
              {
                isTimeout = true;
                string newLine = Environment.NewLine;
                num1 = aex.InnerExceptions.Count;
                string str = num1.ToString();
                int num2 = (int) MessageBox.Show("**** Multiple timeouts ****" + newLine + "Timeout count: " + str);
              }
              theException = (Exception) null;
            }
            else
              break;
          }
          aex = (AggregateException) null;
        }
        if (!isTimeout)
        {
          int num = (int) MessageBox.Show(ex.ToString());
        }
      }
      this.SetStopState();
    }

    private void ButtonAddNewMap_Click(object sender, RoutedEventArgs e)
    {
      throw new NotImplementedException();
    }

    private void ComboCommand_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.ComboCommand.SelectedIndex >= 0)
      {
        this.ComboAddCommand.Items.Clear();
        Dictionary<int, string> template = new Dictionary<int, string>();
        object selectedItem = this.ComboCommand.SelectedItem;
        if (selectedItem.ToString().Contains(CommandWindowCommon.CMD_ReadMemory))
        {
          template.Add(1, "Adress(HEX):");
          template.Add(2, "Lenght(max.255): ");
        }
        if (selectedItem.ToString().Contains(CommandWindowCommon.CMD_SetSystemTime))
        {
          template.Add(1, "Day.Month.Year:");
          template.Add(2, "Hour:Min:Sec:");
          template.Add(3, "Timezone:");
        }
        if (selectedItem.ToString().Contains(CommandWindowCommon.CMD_SetWriteProtection))
          template.Add(1, "ProtectionKey:");
        if (selectedItem.ToString().Contains(CommandWindowCommon.CMD_OpenWriteProtectionTemporaly))
          template.Add(1, "ProtectionKey:");
        if (selectedItem.ToString().Contains(CommandWindowCommon.CMD_SetMode))
          template.Add(0, "Set Mode:");
        if (selectedItem.ToString().Contains(CommandWindowCommon.CMD_SetKeyDate))
        {
          template.Add(1, "Month:");
          template.Add(2, "DayofMonth:");
          template.Add(3, "FirstYear:");
        }
        if (selectedItem.ToString().Contains(CommandWindowCommon.CMD_SetRadioOperation))
          template.Add(1, "Set RadioOperation(0x00/0x01):");
        if (selectedItem.ToString().Contains(CommandWindowCommon.CMD_SetLcdTestState))
        {
          template.Add(1, "LCD Test State:");
          template.Add(2, "LCD Ram Data:");
        }
        if (selectedItem.ToString().Contains(CommandWindowCommon.CMD_SwitchLoRaLED))
          template.Add(1, "Switch LoRa LED (0x00/0x01):");
        if (selectedItem.ToString().Contains(CommandWindowCommon.CMD_ActivateDeactivateDisplay))
          template.Add(1, "Switch Display (0x00/0x01):");
        if (selectedItem.ToString().Contains(CommandWindowCommon.CMD_TimeShift))
          template.Add(1, "Data (HEX):");
        if (selectedItem.ToString().Contains(CommandWindowCommon.CMD_ExecuteEvent))
          template.Add(1, "Set Event (HexByte):");
        if (selectedItem.ToString().Contains(CommandWindowCommon.CMD_SetRTCCalibration))
          template.Add(1, "Set Calibration Value:");
        if (selectedItem.ToString().Contains(CommandWindowCommon.CMD_SetCommunicationScenario))
          template.Add(1, "Set Communication Scenario:");
        if (selectedItem.ToString().Contains(CommandWindowCommon.CMD_SetLocalInterfaceEncryption))
        {
          template.Add(1, "Set encryption cmd:");
          template.Add(2, "Set additional data:");
        }
        this.SetArgumentFields(template);
        this.ButtonRunCommand.IsEnabled = true;
      }
      else
        this.ButtonRunCommand.IsEnabled = false;
    }

    private void ComboAddCommand_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.ComboAddCommand.SelectedIndex < 0)
        return;
      object selectedItem = this.ComboAddCommand.SelectedItem;
      if (this.ComboAddCommand.SelectedIndex == 7 || this.ComboAddCommand.SelectedIndex == 8)
      {
        this.SetArgumentFields(new Dictionary<int, string>()
        {
          {
            1,
            "Mode:"
          }
        });
      }
      else
      {
        this.TextCommandArgument_1.Visibility = Visibility.Collapsed;
        this.TextArgument_1_Label.Visibility = Visibility.Collapsed;
      }
    }

    private void SetArgumentFieldsValues(Dictionary<int, string> values)
    {
      if (values == null)
        return;
      foreach (KeyValuePair<int, string> keyValuePair in values)
      {
        if (keyValuePair.Key == 0)
          this.ComboAddCommand.SelectedItem = (object) keyValuePair.Value;
        if (keyValuePair.Key == 1)
          this.TextCommandArgument_1.Text = keyValuePair.Value;
        if (keyValuePair.Key == 2)
          this.TextCommandArgument_2.Text = keyValuePair.Value;
        if (keyValuePair.Key == 3)
          this.TextCommandArgument_3.Text = keyValuePair.Value;
        if (keyValuePair.Key == 4)
          this.TextCommandArgument_4.Text = keyValuePair.Value;
      }
    }

    private void SetArgumentFields(Dictionary<int, string> template)
    {
      this.TextCommandArgument_1.Text = string.Empty;
      this.TextCommandArgument_2.Text = string.Empty;
      this.TextCommandArgument_3.Text = string.Empty;
      this.TextCommandArgument_4.Text = string.Empty;
      this.ComboAddCommand_Label.Visibility = Visibility.Collapsed;
      this.ComboAddCommand.Visibility = Visibility.Collapsed;
      this.TextCommandArgument_1.Visibility = Visibility.Collapsed;
      this.TextCommandArgument_2.Visibility = Visibility.Collapsed;
      this.TextCommandArgument_3.Visibility = Visibility.Collapsed;
      this.TextCommandArgument_4.Visibility = Visibility.Collapsed;
      this.TextArgument_1_Label.Visibility = Visibility.Collapsed;
      this.TextArgument_2_Label.Visibility = Visibility.Collapsed;
      this.TextArgument_3_Label.Visibility = Visibility.Collapsed;
      this.TextArgument_4_Label.Visibility = Visibility.Collapsed;
      if (template == null)
        return;
      foreach (KeyValuePair<int, string> keyValuePair in template)
      {
        if (keyValuePair.Key == 0)
        {
          this.ComboAddCommand.Visibility = Visibility.Visible;
          if (this.ComboCommand.SelectedItem.ToString().Contains(CommandWindowCommon.CMD_SetMode))
          {
            this.ComboAddCommand.Items.Add((object) "Operation Mode(0x00)");
            this.ComboAddCommand.Items.Add((object) "Delivery Mode(0x01)");
            this.ComboAddCommand.Items.Add((object) "Clock calibration mode(0x02)");
            this.ComboAddCommand.Items.Add((object) "Temperature calibration mode(0x03)");
            this.ComboAddCommand.Items.Add((object) "Volume calibration mode(0x04)");
            this.ComboAddCommand.Items.Add((object) "Bootloader prepare mode(0x05)");
            this.ComboAddCommand.Items.Add((object) "Bootloader mode(0x06)");
            this.ComboAddCommand.Items.Add((object) "Reserved for future common definitions(0x07~0x7F)");
            this.ComboAddCommand.Items.Add((object) "Special device modes(0x80~0xFF)");
          }
          this.ComboAddCommand_Label.Visibility = Visibility.Visible;
          this.ComboAddCommand_Label.Content = (object) keyValuePair.Value;
        }
        if (keyValuePair.Key == 1)
        {
          this.TextCommandArgument_1.Visibility = Visibility.Visible;
          this.TextArgument_1_Label.Visibility = Visibility.Visible;
          this.TextArgument_1_Label.Content = (object) keyValuePair.Value;
          this.TextCommandArgument_1.ContextMenu = this.Argument1ValuesMenu;
          this.TextCommandArgument_1.TextChanged += new TextChangedEventHandler(this.TextCommandArgument_1_TextChanged);
          if (this.ComboCommand.SelectedItem.ToString().Contains(CommandWindowCommon.CMD_SetSystemTime))
            this.TextCommandArgument_1.Text = DateTime.Now.ToShortDateString().ToString();
          if (this.ComboCommand.SelectedItem.ToString().Contains(CommandWindowCommon.CMD_SetMode))
          {
            this.ComboAddCommand.Visibility = Visibility.Visible;
            this.ComboAddCommand_Label.Visibility = Visibility.Visible;
            this.ComboAddCommand_Label.Content = (object) keyValuePair.Value;
            string str = this.ComboAddCommand.SelectedItem.ToString();
            this.TextArgument_1_Label.Content = (object) (str.Split('(')[1].Substring(2, 3) + str.Split('(')[1].Substring(7, 2));
          }
        }
        if (keyValuePair.Key == 2)
        {
          this.TextCommandArgument_2.Visibility = Visibility.Visible;
          this.TextArgument_2_Label.Visibility = Visibility.Visible;
          this.TextArgument_2_Label.Content = (object) keyValuePair.Value;
          this.TextCommandArgument_2.ContextMenu = this.Argument2ValuesMenu;
          if (this.ComboCommand.SelectedItem.ToString().Contains(CommandWindowCommon.CMD_SetSystemTime))
            this.TextCommandArgument_2.Text = string.Format("{0:T}", (object) DateTime.Now);
          if (this.ComboCommand.SelectedItem.ToString().Contains(CommandWindowCommon.CMD_SetLcdTestState))
          {
            this.TextCommandArgument_2.Visibility = Visibility.Hidden;
            this.TextArgument_2_Label.Visibility = Visibility.Hidden;
          }
        }
        if (keyValuePair.Key == 3)
        {
          this.TextCommandArgument_3.Visibility = Visibility.Visible;
          this.TextArgument_3_Label.Visibility = Visibility.Visible;
          this.TextArgument_3_Label.Content = (object) keyValuePair.Value;
          this.TextCommandArgument_3.ContextMenu = this.Argument3ValuesMenu;
          if (this.ComboCommand.SelectedItem.ToString().Contains(CommandWindowCommon.CMD_SetSystemTime))
            this.TextCommandArgument_3.Text = "00";
        }
        if (keyValuePair.Key == 4)
        {
          this.TextCommandArgument_4.Visibility = Visibility.Visible;
          this.TextArgument_4_Label.Visibility = Visibility.Visible;
          this.TextArgument_4_Label.Content = (object) keyValuePair.Value;
          this.TextCommandArgument_4.ContextMenu = this.Argument3ValuesMenu;
        }
      }
    }

    private void TextCommandArgument_1_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (!this.ComboCommand.SelectedItem.ToString().Contains(CommandWindowCommon.CMD_SetLcdTestState))
        return;
      if (this.TextCommandArgument_1.Text.Equals("0xff") || this.TextCommandArgument_1.Text.Equals("255"))
      {
        this.TextArgument_2_Label.Content = (object) "DataRam:";
        this.TextArgument_2_Label.Visibility = Visibility.Visible;
        this.TextCommandArgument_2.Visibility = Visibility.Visible;
      }
      else
      {
        this.TextArgument_2_Label.Content = (object) "";
        this.TextArgument_2_Label.Visibility = Visibility.Hidden;
        this.TextCommandArgument_2.Visibility = Visibility.Hidden;
      }
    }

    private void setCommonCommands()
    {
      this.ComboCommand.Items.Clear();
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_GetVersion);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_Resetdevice);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_SetWriteProtection);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_OpenWriteProtectionTemporaly);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_RunBackup);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_SetSystemTime);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_GetSystemTime);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_SetMode);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_GetMode);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_SetKeyDate);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_GetKeyDate);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_SetRadioOperation);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_GetRadioOperation);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_ClearResetCounter);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_GetResetCounter);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_SetLcdTestState);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_SwitchLoRaLED);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_ActivateDeactivateDisplay);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_TimeShift);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_ExecuteEvent);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_SetRTCCalibration);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_GetCommunicationScenario);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_SetCommunicationScenario);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_GetPrintedSerialNumber);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_SetLocalInterfaceEncryption);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_GetLocalInterfaceEncryption);
      this.ComboCommand.Items.Add((object) CommandWindowCommon.CMD_ReadMemory);
      this.ComboCommand.SelectedIndex = 0;
    }

    private void mi_Click(object sender, RoutedEventArgs e)
    {
      ((TextBox) ((FrameworkElement) sender).Tag).Text = ((HeaderedItemsControl) sender).Header.ToString();
    }

    private async Task RunCommand()
    {
      string FC = this.ComboCommand.SelectedItem.ToString();
      object addFC = this.ComboAddCommand.SelectedItem;
      string arg1 = string.IsNullOrEmpty(this.TextCommandArgument_1.Text) ? (string) null : this.TextCommandArgument_1.Text.Trim();
      string arg2 = string.IsNullOrEmpty(this.TextCommandArgument_2.Text) ? (string) null : this.TextCommandArgument_2.Text.Trim();
      string arg3 = string.IsNullOrEmpty(this.TextCommandArgument_3.Text) ? (string) null : this.TextCommandArgument_3.Text.Trim();
      this.myCommonCommands.enDeCrypt = this.CheckBoxEncryption.IsChecked.Value;
      this.myCommonCommands.AES_Key = this.TextBoxEncryptionKey.Text.Trim();
      try
      {
        if (FC.Contains(CommandWindowCommon.CMD_GetVersion))
        {
          try
          {
            byte[] retVal = await this.myCommonCommands.TransmitAndReceiveVersionData((byte) 6, (byte) 0, this.progress, this.cancelTokenSource.Token);
            this.result = this.result + "\nreturn byte array: " + Utility.ByteArrayToHexString(retVal);
            ushort uFV = retVal.Length >= 5 ? (ushort) retVal[0] : throw new Exception("FirmwareVersion has to be at least 4 Bytes long!");
            switch (uFV)
            {
              case 6:
                uint uFV1 = BitConverter.ToUInt32(retVal, 1);
                FirmwareVersion localFV = new FirmwareVersion(uFV1);
                this.result = this.result + "\nVersion: " + localFV.ToString();
                retVal = (byte[]) null;
                localFV = new FirmwareVersion();
                break;
              case (ushort) byte.MaxValue:
                throw new Exception("NACK received!\nCommand not known by this firmware!");
              default:
                throw new Exception("wrong answer received... 0x06 expected!");
            }
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nRead Version ERROR: \n -->" + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_ReadMemory))
        {
          try
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              uint address = Convert.ToUInt32(arg1, 16);
              uint len = 10;
              if (!uint.TryParse(arg2, out len))
                len = 10U;
              len = len > (uint) byte.MaxValue ? (uint) byte.MaxValue : len;
              byte[] retVal = await this.myCommonCommands.ReadMemoryAsync(this.progress, this.cancelTokenSource.Token, address, (byte) len);
              this.result = this.result + "\nData at address(" + arg1 + ")";
              this.result = this.result + "\n" + Utility.ByteArrayToHexString(retVal);
              retVal = (byte[]) null;
            }
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nRead Memory \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_Resetdevice))
        {
          try
          {
            await this.myCommonCommands.ResetDeviceAsync(this.progress, this.cancelTokenSource.Token);
            this.result += "\nReset Device OK";
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (Reset device) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_RunBackup))
        {
          try
          {
            await this.myCommonCommands.BackupDeviceAsync(this.progress, this.cancelTokenSource.Token);
            this.result += "\nBackup Device OK";
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (Run Backup) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_SetWriteProtection))
        {
          try
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              uint uiKey = 0;
              uiKey = arg1.IndexOf("0x") < 0 ? Convert.ToUInt32(arg1, 10) : Convert.ToUInt32(arg1, 16);
              byte[] baKey = BitConverter.GetBytes(uiKey);
              await this.myCommonCommands.SetWriteProtectionAsync(baKey, this.progress, this.cancelTokenSource.Token);
              this.result = this.result + "\n" + CommandWindowCommon.CMD_SetWriteProtection.ToString() + " was send ...";
              baKey = (byte[]) null;
            }
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (" + CommandWindowCommon.CMD_SetWriteProtection.ToString() + ") \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_OpenWriteProtectionTemporaly))
        {
          try
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              uint uiKey = 0;
              uiKey = arg1.IndexOf("0x") < 0 ? Convert.ToUInt32(arg1, 10) : Convert.ToUInt32(arg1, 16);
              byte[] baKey = BitConverter.GetBytes(uiKey);
              await this.myCommonCommands.OpenWriteProtectionTemporarilyAsync(baKey, this.progress, this.cancelTokenSource.Token);
              this.result = this.result + "\n" + CommandWindowCommon.CMD_OpenWriteProtectionTemporaly.ToString() + " was send ...";
              baKey = (byte[]) null;
            }
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (" + CommandWindowCommon.CMD_OpenWriteProtectionTemporaly.ToString() + ") \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_SetSystemTime))
        {
          try
          {
            if (!string.IsNullOrEmpty(arg1) && !string.IsNullOrEmpty(arg2) && !string.IsNullOrEmpty(arg3))
            {
              string myTime = arg1 + " " + arg2;
              DateTime setTime = DateTime.Now;
              if (!DateTime.TryParse(myTime, out setTime))
                throw new FormatException("Date has no the correct format.");
              Common32BitCommands.SystemTime sysTime = Convert.ToInt32(arg3) <= 56 && Convert.ToInt32(arg3) >= -48 ? new Common32BitCommands.SystemTime(setTime, sbyte.Parse(arg3)) : throw new ArgumentOutOfRangeException("Invalid value of 'Timezone'! Max. UTC+14:00 (14*4=56), Min. UTC-12:00 (-12*4=-48), Actual value is: " + arg3);
              await this.myCommonCommands.SetSystemTimeAsync(sysTime, this.progress, this.cancelTokenSource.Token);
              this.result += "\nSet System Time OK";
              this.updateContextMenu1(arg1);
              this.updateContextMenu2(arg2);
              this.updateContextMenu3(arg3);
              myTime = (string) null;
              sysTime = (Common32BitCommands.SystemTime) null;
            }
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (Set System Time) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_GetSystemTime))
        {
          try
          {
            Common32BitCommands.SystemTime sysTime = await this.myCommonCommands.GetSystemTimeAsync(this.progress, this.cancelTokenSource.Token);
            this.result = this.result + "\nSystemTime " + sysTime.ToString();
            sysTime = (Common32BitCommands.SystemTime) null;
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (Get System Time) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_SetMode))
        {
          if (addFC != null)
          {
            try
            {
              if (addFC.ToString().Contains("Reserved for future common definitions") || addFC.ToString().Contains("Special device modes"))
              {
                if (!string.IsNullOrEmpty(arg1))
                {
                  this.updateContextMenu1(arg1);
                  arg1 = arg1.Replace("0x", "");
                  byte mode = Utility.HexStringToByteArray(arg1)[0];
                  await this.myCommonCommands.SetModeAsync(mode, this.progress, this.cancelTokenSource.Token);
                  this.result = this.result + "\nMode set to " + addFC?.ToString();
                }
                else
                {
                  int num = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
                }
              }
              else
              {
                string mode = ushort.Parse(addFC.ToString().Split('(')[1].Substring(2, 2)).ToString();
                if (!string.IsNullOrEmpty(mode))
                {
                  await this.myCommonCommands.SetModeAsync(byte.Parse(mode), this.progress, this.cancelTokenSource.Token);
                  this.result = this.result + "\nMode set to " + addFC?.ToString();
                }
                mode = (string) null;
              }
            }
            catch (Exception ex)
            {
              this.result = this.result + "\nFunction (Set Mode) \nERROR: " + ex.Message;
            }
          }
          else
          {
            int num1 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_GetMode))
        {
          try
          {
            string DeviceMode = string.Empty;
            byte retVal = await this.myCommonCommands.GetModeAsync(this.progress, this.cancelTokenSource.Token);
            if (retVal >= (byte) 0 && retVal <= (byte) 127)
            {
              byte num = retVal;
              switch (num)
              {
                case 0:
                  DeviceMode = "Operation Mode";
                  break;
                case 1:
                  DeviceMode = "Delivery Mode";
                  break;
                case 2:
                  DeviceMode = "Clock calibration mode";
                  break;
                case 3:
                  DeviceMode = "Temperature calibration mode";
                  break;
                case 4:
                  DeviceMode = "Volume calibration mode";
                  break;
                case 5:
                  DeviceMode = "Bootloader prepare mode";
                  break;
                case 6:
                  DeviceMode = "Bootloader mode";
                  break;
                default:
                  DeviceMode = "Reserved for future common definitions";
                  break;
              }
            }
            else
              DeviceMode = "Special device modes";
            this.result = this.result + "\nGet Mode: " + DeviceMode + "(" + retVal.ToString("X2") + ")";
            DeviceMode = (string) null;
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (Get Mode) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_SetKeyDate))
        {
          if (!string.IsNullOrEmpty(arg1) && !string.IsNullOrEmpty(arg2))
          {
            try
            {
              this.updateContextMenu1(arg1);
              this.updateContextMenu2(arg2);
              Common32BitCommands.KeyDate keydate = new Common32BitCommands.KeyDate();
              keydate.Month = byte.Parse(arg1);
              keydate.DayOfMonth = byte.Parse(arg2);
              keydate.FirstYear = byte.Parse("255");
              if (!string.IsNullOrEmpty(arg3))
                keydate.FirstYear = byte.Parse(arg3);
              await this.myCommonCommands.SetKeyDateAsync(keydate, this.progress, this.cancelTokenSource.Token);
              this.result += "\nSet KeyDate OK";
              keydate = (Common32BitCommands.KeyDate) null;
            }
            catch (Exception ex)
            {
              this.result = this.result + "\nFunction (Set KeyDate) \nERROR: " + ex.Message;
            }
          }
          else
          {
            int num2 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_GetKeyDate))
        {
          try
          {
            Common32BitCommands.KeyDate keydate = await this.myCommonCommands.GetKeyDateAsync(this.progress, this.cancelTokenSource.Token);
            this.result += string.Format("\nGet KeyDate: Month {0} DayofMonth {1}  FirstYear {2} ", (object) keydate.Month, (object) keydate.DayOfMonth, (object) keydate.FirstYear);
            keydate = (Common32BitCommands.KeyDate) null;
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (Get KeyDate) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_SetRadioOperation))
        {
          if (!string.IsNullOrEmpty(arg1))
          {
            try
            {
              this.updateContextMenu1(arg1);
              arg1 = arg1.Contains("0x") ? uint.Parse(arg1.Substring(2), NumberStyles.HexNumber).ToString() : arg1;
              byte state = byte.Parse(arg1);
              await this.myCommonCommands.SetRadioOperationAsync(state, this.progress, this.cancelTokenSource.Token);
              this.result += "\nSet RadioOperation OK";
            }
            catch (Exception ex)
            {
              this.result = this.result + "\nFunction (Set RadioOperation) \nERROR: " + ex.Message;
            }
          }
          else
          {
            int num3 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_GetRadioOperation))
        {
          try
          {
            byte retVal = await this.myCommonCommands.GetRadioOperationAsync(this.progress, this.cancelTokenSource.Token);
            this.result = retVal != (byte) 0 ? (retVal != (byte) 1 ? this.result + "Unknown" : this.result + "\nGet RadioOperation: RadioOn(1)") : this.result + "\nGet RadioOperation: RadioOff(0)";
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (Get RadioOperation) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_ClearResetCounter))
        {
          try
          {
            await this.myCommonCommands.ClearResetCounterAsync(this.progress, this.cancelTokenSource.Token);
            this.result += "\nClear ResetCounter OK";
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (Clear ResetCounter) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_GetResetCounter))
        {
          try
          {
            byte retVal = await this.myCommonCommands.GetResetCounterAsync(this.progress, this.cancelTokenSource.Token);
            this.result = this.result + "\nGet ResetCounter: " + retVal.ToString();
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (Get ResetCounter) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_SetLcdTestState))
        {
          try
          {
            if (string.IsNullOrEmpty(arg1))
            {
              this.result += "\n Please input one byte from 0x00 to 0xff, only!";
              FC = (string) null;
              addFC = (object) null;
              arg1 = (string) null;
              arg2 = (string) null;
              arg3 = (string) null;
              return;
            }
            arg1 = arg1.Replace("0x", "");
            if (!string.IsNullOrEmpty(arg1))
            {
              this.TextBoxUniversalCommandResult.Text = string.Empty;
              this.result = string.Empty;
              uint myCMDByte;
              this.LCDStateStartByte = !uint.TryParse(arg1, out myCMDByte) ? (byte) Convert.ToUInt16(arg1, 16) : (byte) myCMDByte;
              if (myCMDByte > (uint) byte.MaxValue)
              {
                this.progress.Report("LCD Test start ...");
                this.result += "\nValue for LCD Test State is not allowed!";
                this.result += "\nPlease use a value from 0x00 up to 0xff.";
              }
              else if (this.LCDStateStartByte != byte.MaxValue && this.LCDStateStartByte != (byte) 254)
              {
                this.LCDStateActualTestByte = this.LCDStateStartByte;
                this.progress.Report("LCD Test start ...");
                await this.myCommonCommands.SetLcdTestStateAsync(this.LCDStateActualTestByte, (byte[]) null, this.progress, this.cancelTokenSource.Token);
                this.result = this.result + "\nSet LCD to State: " + this.LCDStateActualTestByte.ToString("x2");
                this.progress.Report("LCD Test done.");
              }
              else if ((this.LCDStateStartByte == byte.MaxValue || this.LCDStateStartByte == (byte) 254) && string.IsNullOrEmpty(arg2))
                this.result += "\n\nRam data NOT set correctly, please set Ram data.";
              else if (this.LCDStateStartByte == byte.MaxValue && !string.IsNullOrEmpty(arg2))
              {
                byte[] dataRam = new byte[1];
                arg2 = arg2.Replace("0x", "");
                dataRam = Util.HexStringToByteArray(arg2);
                await this.myCommonCommands.SetLcdTestStateAsync(this.LCDStateStartByte, dataRam, this.progress, this.cancelTokenSource.Token);
                this.result += "\nSet LCD Special with DataRam: OK.";
                dataRam = (byte[]) null;
              }
            }
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (SetLcdTestState) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_SwitchLoRaLED))
        {
          try
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              arg1 = arg1.Replace("0x", "");
              byte state = Utility.HexStringToByteArray(arg1)[0];
              await this.myCommonCommands.SwitchLoRaLEDAsync(state, this.progress, this.cancelTokenSource.Token);
              this.result = this.result + "\nSwitched LoRa LED -> " + (state == (byte) 0 ? "OFF" : (state == (byte) 1 ? "ON" : "UNKNOWN"));
            }
            else
              this.result += "\nThe argument was not set correctly.";
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (SwitchLoRaLEDAsync) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_ActivateDeactivateDisplay))
        {
          try
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              arg1 = arg1.Replace("0x", "");
              byte state = Utility.HexStringToByteArray(arg1)[0];
              await this.myCommonCommands.ActivateDeactivateDisplayAsync(state, this.progress, this.cancelTokenSource.Token);
              this.result = this.result + "\nDisplay should be -> " + (state == (byte) 0 ? "DEACTIVATED" : (state == (byte) 1 ? "ACTIVATED" : "UNKNOWN"));
            }
            else
              this.result += "\nThe argument was not set correctly.";
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (ActivateDeactivateDisplayAsync) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_TimeShift))
        {
          try
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              arg1 = arg1.Replace("0x", "");
              byte[] data = Utility.HexStringToByteArray(arg1);
              await this.myCommonCommands.TimeShiftAsync(data, this.progress, this.cancelTokenSource.Token);
              this.result += "\nTimeShift was send ...";
              data = (byte[]) null;
            }
            else
              this.result += "\nThe argument was not set correctly.";
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (TimeShift) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_ExecuteEvent))
        {
          try
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              arg1 = arg1.Replace("0x", "");
              byte[] data = Utility.HexStringToByteArray(arg1);
              await this.myCommonCommands.ExecuteEventAsync(data[0], this.progress, this.cancelTokenSource.Token);
              this.result += "\nExecuteEvent was send ...";
              data = (byte[]) null;
            }
            else
              this.result += "\nThe argument was not set correctly.";
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (ExecuteEvent) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_SetRTCCalibration))
        {
          try
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              short iData = 0;
              iData = arg1.IndexOf("0x") < 0 ? Convert.ToInt16(arg1, 10) : Convert.ToInt16(arg1, 16);
              await this.myCommonCommands.SetRTC_Calibration(iData, this.progress, this.cancelTokenSource.Token);
              this.result += "\nSetRTCCalibration was send ...";
            }
            else
              this.result += "\nThe argument was not set correctly.";
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (SetRTCCalibration) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_GetCommunicationScenario))
        {
          try
          {
            Common32BitCommands.Scenarios retVal = await this.myCommonCommands.GetCommunicationScenarioAsync(this.progress, this.cancelTokenSource.Token);
            this.result = this.result + "\nGet CommunicationScenario: " + Utility.ByteArrayToHexString(retVal.basedata);
            string result1 = this.result;
            ushort? nullable = retVal.ScenarioOne;
            string str1;
            if (nullable.HasValue)
            {
              nullable = retVal.ScenarioOne;
              str1 = nullable.ToString();
            }
            else
              str1 = "n.a.";
            this.result = result1 + "\nScenario 1: " + str1;
            string result2 = this.result;
            nullable = retVal.ScenarioTwo;
            string str2;
            if (nullable.HasValue)
            {
              nullable = retVal.ScenarioTwo;
              str2 = nullable.ToString();
            }
            else
              str2 = "n.a.";
            this.result = result2 + "\nScenario 2: " + str2;
            retVal = (Common32BitCommands.Scenarios) null;
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (GetCommunicationScenario) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_SetCommunicationScenario))
        {
          try
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              ushort iData = 0;
              iData = arg1.IndexOf("0x") < 0 ? Convert.ToUInt16(arg1, 10) : Convert.ToUInt16(arg1, 16);
              byte[] data = BitConverter.GetBytes(iData);
              await this.myCommonCommands.SetCommunicationScenarioAsync(data, this.progress, this.cancelTokenSource.Token);
              this.result += "\nSetCommunicationScenario was send ... OK.";
              this.result += "\nDone.";
              data = (byte[]) null;
            }
            else
              this.result += "\nThe argument was not set correctly.";
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (SetCommunicationScenario) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_GetPrintedSerialNumber))
        {
          try
          {
            string retVal = await this.myCommonCommands.GetPrintedSerialNumberAsync(this.progress, this.cancelTokenSource.Token);
            this.result = this.result + "\nGet PrintedSerialNumber: '" + retVal + "'";
            retVal = (string) null;
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (Get PrintedSerialNumber) \nERROR: " + ex.Message;
          }
        }
        if (FC.Contains(CommandWindowCommon.CMD_SetLocalInterfaceEncryption))
        {
          string cmdName = "SetLocalInterfaceEncryption";
          try
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              ushort iData = 0;
              ushort iDataAdd = 0;
              iData = arg1.IndexOf("0x") < 0 ? (ushort) Convert.ToByte(arg1, 10) : (ushort) Convert.ToByte(arg1, 16);
              if (!string.IsNullOrEmpty(arg2))
                iDataAdd = arg2.IndexOf("0x") < 0 ? Convert.ToUInt16(arg2, 10) : Convert.ToUInt16(arg2, 16);
              byte[] tmpData = BitConverter.GetBytes(iData);
              byte[] tmpDataAdd = BitConverter.GetBytes(iDataAdd);
              byte[] data = new byte[3];
              Buffer.BlockCopy((Array) tmpData, 0, (Array) data, 0, 1);
              Buffer.BlockCopy((Array) tmpDataAdd, 0, (Array) data, 1, 2);
              await this.myCommonCommands.SetLocalInterfaceEncryptionAsync(data, this.progress, this.cancelTokenSource.Token);
              this.result = this.result + "\n" + cmdName + " was send ... OK.";
              this.result += "\nDone.";
              tmpData = (byte[]) null;
              tmpDataAdd = (byte[]) null;
              data = (byte[]) null;
            }
            else
              this.result += "\nThe argument was not set correctly.";
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (" + cmdName + ") \nERROR: " + ex.Message;
          }
          cmdName = (string) null;
        }
        if (FC.Contains(CommandWindowCommon.CMD_GetLocalInterfaceEncryption))
        {
          try
          {
            byte[] retVal = await this.myCommonCommands.GetLocalInterfaceEncryptionAsync(this.progress, this.cancelTokenSource.Token);
            this.result = this.result + "\nGet GetLocalInterfaceEncryption: '" + Utility.ByteArrayToHexString(retVal) + "'";
            retVal = (byte[]) null;
          }
          catch (Exception ex)
          {
            this.result = this.result + "\nFunction (Get GetLocalInterfaceEncryption) \nERROR: " + ex.Message;
          }
        }
        if (string.IsNullOrEmpty(this.result))
        {
          FC = (string) null;
          addFC = (object) null;
          arg1 = (string) null;
          arg2 = (string) null;
          arg3 = (string) null;
        }
        else
        {
          this.TextBoxUniversalCommandResult.Text = this.result;
          this.TextBoxUniversalCommandResult.UpdateLayout();
          CommandWindowCommon.ForceUIToUpdate();
          FC = (string) null;
          addFC = (object) null;
          arg1 = (string) null;
          arg2 = (string) null;
          arg3 = (string) null;
        }
      }
      catch (Exception ex)
      {
        this.result = "Error occoured ...\n\n" + ex.Message;
        this.TextBoxUniversalCommandResult.Text = this.result;
        FC = (string) null;
        addFC = (object) null;
        arg1 = (string) null;
        arg2 = (string) null;
        arg3 = (string) null;
      }
    }

    public static void ForceUIToUpdate()
    {
      DispatcherFrame frame = new DispatcherFrame();
      Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Render, (Delegate) (parameter =>
      {
        frame.Continue = false;
        return (object) null;
      }), (object) null);
      Dispatcher.PushFrame(frame);
    }

    private async void ButtonRunCommand_Click(object sender, RoutedEventArgs e)
    {
      if (this.ButtonRunCommand.Content.ToString().Contains("STOP"))
      {
        this.ButtonBreak.IsEnabled = false;
        this.ButtonRunCommand.IsEnabled = true;
        this.ButtonRunCommand.Content = (object) "Run command";
      }
      else
        await this.RunCommandFrame();
    }

    private async void ButtonRunCommandPreview_Click(object sender, RoutedEventArgs e)
    {
      this.result = "Actual data of connected device:\n-------------------------------------------";
      Dictionary<int, string> args = new Dictionary<int, string>();
      this.ComboCommand.SelectedItem = (object) CommandWindowCommon.CMD_GetSystemTime;
      await this.RunCommandFrame();
      this.ComboCommand.SelectedItem = (object) CommandWindowCommon.CMD_GetMode;
      await this.RunCommandFrame();
      this.ComboCommand.SelectedItem = (object) CommandWindowCommon.CMD_GetKeyDate;
      await this.RunCommandFrame();
      this.ComboCommand.SelectedItem = (object) CommandWindowCommon.CMD_GetRadioOperation;
      await this.RunCommandFrame();
      this.ComboCommand.SelectedItem = (object) CommandWindowCommon.CMD_GetResetCounter;
      await this.RunCommandFrame();
      args = (Dictionary<int, string>) null;
    }

    private async void ButtonGetversion_Click(object sender, RoutedEventArgs e)
    {
      await Task.Delay(1);
      throw new NotImplementedException();
    }

    private void updateContextMenu1(string packet)
    {
      if (!this.Argument1_last_values.Contains(packet))
        this.Argument1_last_values.Add(packet);
      this.Argument1ValuesMenu.Items.Clear();
      if (this.Argument1_last_values.Count > 20)
        this.Argument1_last_values.RemoveRange(0, this.Argument1_last_values.Count - 20);
      foreach (string argument1LastValue in this.Argument1_last_values)
      {
        MenuItem newItem = new MenuItem();
        newItem.Header = (object) argument1LastValue;
        newItem.Click += new RoutedEventHandler(this.mi_Click);
        newItem.Tag = (object) this.TextCommandArgument_1;
        this.Argument1ValuesMenu.Items.Add((object) newItem);
      }
    }

    private void updateContextMenu2(string packet)
    {
      if (!this.Argument2_last_values.Contains(packet))
        this.Argument2_last_values.Add(packet);
      this.Argument2ValuesMenu.Items.Clear();
      if (this.Argument2_last_values.Count > 20)
        this.Argument2_last_values.RemoveRange(0, this.Argument2_last_values.Count - 20);
      foreach (string argument2LastValue in this.Argument2_last_values)
      {
        MenuItem newItem = new MenuItem();
        newItem.Header = (object) argument2LastValue;
        newItem.Click += new RoutedEventHandler(this.mi_Click);
        newItem.Tag = (object) this.TextCommandArgument_2;
        this.Argument2ValuesMenu.Items.Add((object) newItem);
      }
    }

    private void updateContextMenu3(string packet)
    {
      if (!this.Argument3_last_values.Contains(packet))
        this.Argument3_last_values.Add(packet);
      this.Argument3ValuesMenu.Items.Clear();
      if (this.Argument3_last_values.Count > 20)
        this.Argument3_last_values.RemoveRange(0, this.Argument3_last_values.Count - 20);
      foreach (string argument3LastValue in this.Argument3_last_values)
      {
        MenuItem newItem = new MenuItem();
        newItem.Header = (object) argument3LastValue;
        newItem.Click += new RoutedEventHandler(this.mi_Click);
        newItem.Tag = (object) this.TextCommandArgument_3;
        this.Argument3ValuesMenu.Items.Add((object) newItem);
      }
    }

    private void CheckBoxEncryption_Checked(object sender, RoutedEventArgs e)
    {
      this.myCommonCommands.enDeCrypt = true;
      this.TextBoxEncryptionKey.Visibility = Visibility.Visible;
      this.EncryptionKey_Label.Visibility = Visibility.Visible;
    }

    private void CheckBoxEncryption_UnChecked(object sender, RoutedEventArgs e)
    {
      this.myCommonCommands.enDeCrypt = false;
      this.TextBoxEncryptionKey.Visibility = Visibility.Collapsed;
      this.EncryptionKey_Label.Visibility = Visibility.Collapsed;
    }

    private void TextBoxEncryptionKey_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.myCommonCommands.AES_Key = this.TextBoxEncryptionKey.Text;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/commandwindowcommon.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.TextBlockStatus = (TextBlock) target;
          break;
        case 2:
          this.TextBoxUniversalCommandResult = (TextBox) target;
          break;
        case 3:
          this.StackPanalButtons = (StackPanel) target;
          break;
        case 4:
          this.FunctionCode_Label = (Label) target;
          break;
        case 5:
          this.ComboCommand = (ComboBox) target;
          this.ComboCommand.SelectionChanged += new SelectionChangedEventHandler(this.ComboCommand_SelectionChanged);
          break;
        case 6:
          this.CheckBoxEncryption = (CheckBox) target;
          this.CheckBoxEncryption.Checked += new RoutedEventHandler(this.CheckBoxEncryption_Checked);
          this.CheckBoxEncryption.Unchecked += new RoutedEventHandler(this.CheckBoxEncryption_UnChecked);
          break;
        case 7:
          this.EncryptionKey_Label = (Label) target;
          break;
        case 8:
          this.TextBoxEncryptionKey = (TextBox) target;
          this.TextBoxEncryptionKey.TextChanged += new TextChangedEventHandler(this.TextBoxEncryptionKey_TextChanged);
          break;
        case 9:
          this.ComboAddCommand_Label = (Label) target;
          break;
        case 10:
          this.ComboAddCommand = (ComboBox) target;
          this.ComboAddCommand.SelectionChanged += new SelectionChangedEventHandler(this.ComboAddCommand_SelectionChanged);
          break;
        case 11:
          this.TextArgument_1_Label = (Label) target;
          break;
        case 12:
          this.TextCommandArgument_1 = (TextBox) target;
          break;
        case 13:
          this.TextArgument_2_Label = (Label) target;
          break;
        case 14:
          this.TextCommandArgument_2 = (TextBox) target;
          break;
        case 15:
          this.TextArgument_3_Label = (Label) target;
          break;
        case 16:
          this.TextCommandArgument_3 = (TextBox) target;
          break;
        case 17:
          this.TextArgument_4_Label = (Label) target;
          break;
        case 18:
          this.TextCommandArgument_4 = (TextBox) target;
          break;
        case 19:
          this.StackPanalButtons2 = (StackPanel) target;
          break;
        case 20:
          this.ButtonRunCommand = (Button) target;
          this.ButtonRunCommand.Click += new RoutedEventHandler(this.ButtonRunCommand_Click);
          break;
        case 21:
          this.ButtonRunCommandPreview = (Button) target;
          this.ButtonRunCommandPreview.Click += new RoutedEventHandler(this.ButtonRunCommandPreview_Click);
          break;
        case 22:
          this.ButtonBreak = (Button) target;
          this.ButtonBreak.Click += new RoutedEventHandler(this.ButtonBreak_Click);
          break;
        case 23:
          this.ProgressBar1 = (ProgressBar) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    private delegate Task<byte[]> DeviceCommand(
      ProgressHandler progress,
      CancellationToken cancelToken);
  }
}
