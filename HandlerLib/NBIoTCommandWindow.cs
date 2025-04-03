// Decompiled with JetBrains decompiler
// Type: HandlerLib.NBIoTCommandWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class NBIoTCommandWindow : Window, IComponentConnector
  {
    private CommonNBIoTCommands myNBIotCommands;
    private List<string> Argument1_last_values;
    private List<string> Argument2_last_values;
    private List<string> Argument3_last_values;
    private List<string> Argument4_last_values;
    private List<string> Argument5_last_values;
    private ContextMenu Argument1ValuesMenu;
    private ContextMenu Argument2ValuesMenu;
    private ContextMenu Argument3ValuesMenu;
    private ContextMenu Argument4ValuesMenu;
    private ContextMenu Argument5ValuesMenu;
    private static string result = string.Empty;
    private static readonly string CMD_GetNBIoTModulePartNumber = "Get NBIoT Module Part Number (0x00)";
    private static readonly string CMD_GetNBIoTFirmwareVersion = "Get NBIoT Firmware Version (0x01)";
    private static readonly string CMD_GetNBIoTIMEI_NB = "Get NBIoT IMEI From NB (0x02)";
    private static readonly string CMD_GetSIMIMSI_NB = "Get SIM IMSI From NB (0x03)";
    private static readonly string CMD_GetProtocol = "Get Protocol (0x04)";
    private static readonly string CMD_SetProtocol = "Set Protocol (0x04)";
    private static readonly string CMD_GetBand = "Get Band (0x05)";
    private static readonly string CMD_SetBand = "Set Band (0x05)";
    private static readonly string CMD_GetRemoteIP = "Get Remote IP (0x06)";
    private static readonly string CMD_SetRemoteIP = "Set Remote IP (0x06)";
    private static readonly string CMD_GetRemotePort = "Get Remote Port (0x07)";
    private static readonly string CMD_SetRemotePort = "Set Remote Port (0x07)";
    private static readonly string CMD_SetOperator = "Set Operator (0x08)";
    private static readonly string CMD_GetOperator = "Get Operator (0x08)";
    private static readonly string CMD_SendConfirmedData = "Send Confirmed Data (0x09)";
    private static readonly string CMD_SendUnconfirmedData = "Send Unconfirmed Data (0x0A)";
    private static readonly string CMD_SendTestData = "Send Test Data (0x0B)";
    private static readonly string CMD_SendRadioFullFunctionOn = "Send Radio Full Function On (0x0C)";
    private static readonly string CMD_SendRadioFullFunctionOff = "Send Radio Full Function Off (0x0C)";
    private static readonly string CMD_SetNBIoTPowerOn = "Send NBIoT Power On (0x20)";
    private static readonly string CMD_SetNBIoTPowerOff = "Send NBIoT Power Off (0x20)";
    private static readonly string CMD_SendCommonCommand = "Send Common Command (0x21)";
    private static readonly string CMD_GetIMEI_IMSI_NBVER_RAM = "Get IMEI, IMSI and NBVer from RAM (0x22)";
    private static readonly string CMD_GetICCID_IMSI_RAM = "Get ICCID and IMSI from RAM (0x23)";
    private static readonly string CMD_SetTransmissionScenario = "Set transmission sceanrio (0x28)";
    private static readonly string CMD_GetTransmissionScenario = "Get transmission sceanrio (0x28)";
    private static readonly string CMD_SetDeviceEUI = "Set device EUI (0x25)";
    private static readonly string CMD_GetDeviceEUI = "Get device EUI (0x25)";
    private static readonly string CMD_SendActivePacket = "Send Actived Packet (0x31)";
    private static readonly string CMD_GetSIMICCID_NB = "Get SIM ICCID From NB (0x0D)";
    private static readonly string CMD_GetSecondaryBand = "Get Secondary Band (0x0E)";
    private static readonly string CMD_SetSecondaryBand = "Set Secondary Band (0x0E)";
    private static readonly string CMD_GetDNSName = "Get DNS Name (0x0F)";
    private static readonly string CMD_SetDNSName = "Set DNS Name (0x0F)";
    private static readonly string CMD_GetRadioSendingState = "Get Radio Sending State (0x10)";
    private static readonly string CMD_RestNBModem = "Reset NB modem (0x11)";
    private static readonly string CMD_SetNBModemAutoConnect = "Set NB modem auto connect (0x12)";
    private static readonly string CMD_SetNBModemManualConnect = "Set NB modem manual connect (0x12)";
    private static readonly string CMD_SetNBModemAPN = "Set NB modem APN (0x13)";
    private static readonly string CMD_GetNBModemAPN = "Get NB modem APN (0x13)";
    private static readonly string CMD_SetDNSServerIP = "Set DNS Server IP (0x14)";
    private static readonly string CMD_GetDNSServerIP = "Get DNS Server IP (0x14)";
    private static readonly string CMD_SetDNSEnableByte = "Set DNS Enable Byte (0x29)";
    private static readonly string CMD_GetDNSEnableByte = "Get DNS Enable Byte (0x29)";
    private static readonly string CMD_SetAPNEnableByte = "Set APN Enable Byte (0x30)";
    private static readonly string CMD_GetAPNEnableByte = "Get APN Enable Byte (0x30)";
    private Cursor defaultCursor;
    private CancellationTokenSource cancelTokenSource;
    private ProgressHandler progress;
    internal TextBlock TextBlockStatus;
    internal ProgressBar ProgressBar1;
    internal TextBox TextBoxUniversalCommandResult;
    internal StackPanel StackPanalButtons;
    internal ComboBox ComboCommand;
    internal CheckBox CheckBoxEncryption;
    internal Label EncryptionKey_Label;
    internal TextBox TextBoxEncryptionKey;
    internal Label ComboExtCommand_Label;
    internal ComboBox ComboExtCommand;
    internal Label ComboAddCommand_Label;
    internal ComboBox ComboAddCommand;
    internal Label TextArgument_1_Label;
    internal TextBox TextExtCommandArgument_1;
    internal Label TextArgument_2_Label;
    internal TextBox TextExtCommandArgument_2;
    internal Label TextArgument_3_Label;
    internal TextBox TextExtCommandArgument_3;
    internal Label TextArgument_4_Label;
    internal TextBox TextExtCommandArgument_4;
    internal Label TextArgument_5_Label;
    internal TextBox TextExtCommandArgument_5;
    internal StackPanel StackPanalButtons2;
    internal Button ButtonRunCommand;
    internal Button ButtonRunCommandPreview;
    internal Button ButtonBreak;
    private bool _contentLoaded;

    public ComboBox cmbBox
    {
      get => this.ComboExtCommand;
      set => this.ComboExtCommand.Items.Remove((object) value);
    }

    public NBIoTCommandWindow(CommonNBIoTCommands NBIotCMDs)
    {
      this.InitializeComponent();
      this.myNBIotCommands = NBIotCMDs;
      this.myNBIotCommands.setCryptValuesFromBaseClass();
      this.ButtonRunCommand.IsEnabled = false;
      this.SetArgumentFields((Dictionary<int, string>) null);
      this.setFunctionCodes();
      this.Argument1_last_values = new List<string>();
      this.Argument2_last_values = new List<string>();
      this.Argument3_last_values = new List<string>();
      this.Argument4_last_values = new List<string>();
      this.Argument5_last_values = new List<string>();
      this.Argument1ValuesMenu = new ContextMenu();
      this.Argument2ValuesMenu = new ContextMenu();
      this.Argument3ValuesMenu = new ContextMenu();
      this.Argument4ValuesMenu = new ContextMenu();
      this.Argument5ValuesMenu = new ContextMenu();
      this.setEncryptionState();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
    }

    private void setEncryptionState()
    {
      this.CheckBoxEncryption.IsChecked = new bool?(this.myNBIotCommands.enDeCrypt);
      if (!this.myNBIotCommands.enDeCrypt)
        this.CheckBoxEncryption_UnChecked((object) null, (RoutedEventArgs) null);
      this.TextBoxEncryptionKey.Text = this.myNBIotCommands.AES_Key;
    }

    private void setFunctionCodes()
    {
      this.ComboExtCommand_Label.Visibility = Visibility.Hidden;
      this.ComboExtCommand.Visibility = Visibility.Hidden;
      this.ComboCommand.Items.Clear();
      this.ComboCommand.Items.Add((object) "NBIoT Commands (0x37)");
      this.ComboCommand.SelectedIndex = 0;
    }

    private void setNBIoTCommands()
    {
      Dictionary<string, string> valuesForCommands = GetCommandValues.GetAllPrivateStaticFieldValuesForCommands((object) this);
      this.ComboExtCommand.Items.Clear();
      foreach (KeyValuePair<string, string> keyValuePair in valuesForCommands)
        this.ComboExtCommand.Items.Add((object) keyValuePair.Value);
      this.ComboExtCommand.SelectedIndex = 0;
    }

    private void SetRunState()
    {
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress.Reset();
      this.progress.Split(new double[2]{ 2.0, 98.0 });
      this.progress.Report("Run");
      this.ComboCommand.IsEnabled = false;
      this.ComboExtCommand.IsEnabled = false;
      this.TextBoxUniversalCommandResult.IsEnabled = false;
      this.TextExtCommandArgument_1.IsEnabled = false;
      this.TextExtCommandArgument_2.IsEnabled = false;
      this.TextExtCommandArgument_3.IsEnabled = false;
      this.ButtonRunCommand.IsEnabled = false;
      this.ButtonRunCommandPreview.IsEnabled = false;
      this.ButtonBreak.IsEnabled = true;
      this.defaultCursor = this.Cursor;
      this.Cursor = Cursors.Wait;
    }

    private void SetStopState()
    {
      this.ComboCommand.IsEnabled = true;
      this.ComboExtCommand.IsEnabled = true;
      this.TextBoxUniversalCommandResult.IsEnabled = true;
      this.TextExtCommandArgument_1.IsEnabled = true;
      this.TextExtCommandArgument_2.IsEnabled = true;
      this.TextExtCommandArgument_3.IsEnabled = true;
      this.ButtonRunCommand.IsEnabled = true;
      this.ButtonRunCommandPreview.IsEnabled = true;
      this.ButtonBreak.IsEnabled = false;
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

    private async void ButtonRunCommand_Click(object sender, RoutedEventArgs e)
    {
      NBIoTCommandWindow.result = string.Empty;
      await this.RunCommandFrame();
    }

    private async void ButtonRunCommandPreview_Click(object sender, RoutedEventArgs e)
    {
      NBIoTCommandWindow.result = "Actual NBIoT data of connected device:\n-------------------------------------------";
      Dictionary<int, string> args = new Dictionary<int, string>();
      this.ComboExtCommand.SelectedItem = (object) NBIoTCommandWindow.CMD_GetNBIoTModulePartNumber;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) NBIoTCommandWindow.CMD_GetNBIoTFirmwareVersion;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) NBIoTCommandWindow.CMD_GetNBIoTIMEI_NB;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) NBIoTCommandWindow.CMD_GetSIMIMSI_NB;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) NBIoTCommandWindow.CMD_GetProtocol;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) NBIoTCommandWindow.CMD_GetBand;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) NBIoTCommandWindow.CMD_GetSecondaryBand;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) NBIoTCommandWindow.CMD_GetRemoteIP;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) NBIoTCommandWindow.CMD_GetRemotePort;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) NBIoTCommandWindow.CMD_GetOperator;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) NBIoTCommandWindow.CMD_GetTransmissionScenario;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) NBIoTCommandWindow.CMD_GetDeviceEUI;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) NBIoTCommandWindow.CMD_GetSIMICCID_NB;
      await this.RunCommandFrame();
      args = (Dictionary<int, string>) null;
    }

    private void ComboExtCommand_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.ComboExtCommand.SelectedItem != null)
      {
        object selectedItem = this.ComboExtCommand.SelectedItem;
        this.ComboAddCommand.Items.Clear();
        Dictionary<int, string> template = new Dictionary<int, string>();
        if (selectedItem.ToString().Contains(NBIoTCommandWindow.CMD_SendUnconfirmedData))
          template.Add(1, "Unconfirmed data:");
        if (selectedItem.ToString().Contains(NBIoTCommandWindow.CMD_SendConfirmedData))
          template.Add(1, "Confirmed data:");
        if (selectedItem.ToString().Contains(NBIoTCommandWindow.CMD_SetProtocol))
          template.Add(1, "Protocol (1 Byte):");
        if (selectedItem.ToString().Contains(NBIoTCommandWindow.CMD_SetBand))
          template.Add(1, "Band (1 Byte):");
        if (selectedItem.ToString().Contains(NBIoTCommandWindow.CMD_SetSecondaryBand))
          template.Add(1, "Band (1 Byte):");
        if (selectedItem.ToString().Contains(NBIoTCommandWindow.CMD_SetRemoteIP))
          template.Add(1, "Coap Remote IP (4 Bytes):");
        if (selectedItem.ToString().Contains(NBIoTCommandWindow.CMD_SetRemotePort))
          template.Add(1, "Coap Remote Port (2 Bytes):");
        if (selectedItem.ToString().Contains(NBIoTCommandWindow.CMD_SetOperator))
          template.Add(1, "Operator (2 Bytes):");
        if (selectedItem.ToString().Contains(NBIoTCommandWindow.CMD_SetTransmissionScenario))
          template.Add(1, "Transmission scenario:");
        if (selectedItem.ToString().Contains(NBIoTCommandWindow.CMD_SetDeviceEUI))
          template.Add(1, "Device EUI (8 Bytes(reversed)):");
        if (selectedItem.ToString().Contains(NBIoTCommandWindow.CMD_SendCommonCommand))
          template.Add(1, "NBIoT Common Command:");
        if (selectedItem.ToString().Contains(NBIoTCommandWindow.CMD_SetDNSName))
          template.Add(1, "DNS Name:");
        if (selectedItem.ToString().Contains(NBIoTCommandWindow.CMD_RestNBModem))
          template.Add(1, "Data:");
        if (selectedItem.ToString().Contains(NBIoTCommandWindow.CMD_SetNBModemAPN))
          template.Add(1, "APN:");
        if (selectedItem.ToString().Contains(NBIoTCommandWindow.CMD_SetDNSServerIP))
        {
          template.Add(1, "Primary DNS ServerIP:");
          template.Add(2, "Secondary DNS ServerIP:");
        }
        if (selectedItem.ToString().Contains(NBIoTCommandWindow.CMD_SetDNSEnableByte))
          template.Add(1, "Value(0-255):");
        if (selectedItem.ToString().Contains(NBIoTCommandWindow.CMD_SetAPNEnableByte))
          template.Add(1, "Value(0-1):");
        this.SetArgumentFields(template);
        this.ButtonRunCommand.IsEnabled = true;
      }
      else
        this.ButtonRunCommand.IsEnabled = false;
    }

    private void ComboCommand_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.ComboCommand.SelectedIndex < 0)
        return;
      Dictionary<int, string> dictionary = new Dictionary<int, string>();
      if (this.ComboCommand.SelectedItem.ToString().Contains("0x37"))
      {
        this.setNBIoTCommands();
        this.ComboExtCommand_Label.Content = (object) "NBIoT Commands (EFC):";
        this.ComboExtCommand_Label.Visibility = Visibility.Visible;
        this.ComboExtCommand.Visibility = Visibility.Visible;
      }
    }

    private void ComboAddCommand_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.ComboAddCommand.SelectedIndex < 0)
        return;
      object selectedItem = this.ComboAddCommand.SelectedItem;
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
          this.TextExtCommandArgument_1.Text = keyValuePair.Value;
        if (keyValuePair.Key == 2)
          this.TextExtCommandArgument_2.Text = keyValuePair.Value;
        if (keyValuePair.Key == 3)
          this.TextExtCommandArgument_3.Text = keyValuePair.Value;
        if (keyValuePair.Key == 4)
          this.TextExtCommandArgument_4.Text = keyValuePair.Value;
        if (keyValuePair.Key == 5)
          this.TextExtCommandArgument_5.Text = keyValuePair.Value;
      }
    }

    private void SetArgumentFields(Dictionary<int, string> template)
    {
      this.TextExtCommandArgument_1.Text = string.Empty;
      this.TextExtCommandArgument_2.Text = string.Empty;
      this.TextExtCommandArgument_3.Text = string.Empty;
      this.TextExtCommandArgument_4.Text = string.Empty;
      this.TextExtCommandArgument_5.Text = string.Empty;
      this.ComboAddCommand_Label.Visibility = Visibility.Collapsed;
      this.ComboAddCommand.Visibility = Visibility.Collapsed;
      this.TextExtCommandArgument_1.Visibility = Visibility.Collapsed;
      this.TextExtCommandArgument_2.Visibility = Visibility.Collapsed;
      this.TextExtCommandArgument_3.Visibility = Visibility.Collapsed;
      this.TextExtCommandArgument_4.Visibility = Visibility.Collapsed;
      this.TextExtCommandArgument_5.Visibility = Visibility.Collapsed;
      this.TextArgument_1_Label.Visibility = Visibility.Collapsed;
      this.TextArgument_2_Label.Visibility = Visibility.Collapsed;
      this.TextArgument_3_Label.Visibility = Visibility.Collapsed;
      this.TextArgument_4_Label.Visibility = Visibility.Collapsed;
      this.TextArgument_5_Label.Visibility = Visibility.Collapsed;
      if (template == null)
        return;
      foreach (KeyValuePair<int, string> keyValuePair in template)
      {
        if (keyValuePair.Key == 1)
        {
          this.TextExtCommandArgument_1.Visibility = Visibility.Visible;
          this.TextArgument_1_Label.Visibility = Visibility.Visible;
          this.TextArgument_1_Label.Content = (object) keyValuePair.Value;
          this.TextExtCommandArgument_1.ContextMenu = this.Argument1ValuesMenu;
        }
        if (keyValuePair.Key == 2)
        {
          this.TextExtCommandArgument_2.Visibility = Visibility.Visible;
          this.TextArgument_2_Label.Visibility = Visibility.Visible;
          this.TextArgument_2_Label.Content = (object) keyValuePair.Value;
          this.TextExtCommandArgument_2.ContextMenu = this.Argument2ValuesMenu;
        }
        if (keyValuePair.Key == 3)
        {
          this.TextExtCommandArgument_3.Visibility = Visibility.Visible;
          this.TextArgument_3_Label.Visibility = Visibility.Visible;
          this.TextArgument_3_Label.Content = (object) keyValuePair.Value;
          this.TextExtCommandArgument_3.ContextMenu = this.Argument3ValuesMenu;
        }
        if (keyValuePair.Key == 4)
        {
          this.TextExtCommandArgument_4.Visibility = Visibility.Visible;
          this.TextArgument_4_Label.Visibility = Visibility.Visible;
          this.TextArgument_4_Label.Content = (object) keyValuePair.Value;
          this.TextExtCommandArgument_4.ContextMenu = this.Argument4ValuesMenu;
        }
        if (keyValuePair.Key == 5)
        {
          this.TextExtCommandArgument_5.Visibility = Visibility.Visible;
          this.TextArgument_5_Label.Visibility = Visibility.Visible;
          this.TextArgument_5_Label.Content = (object) keyValuePair.Value;
          this.TextExtCommandArgument_5.ContextMenu = this.Argument5ValuesMenu;
        }
      }
    }

    private void mi_Click(object sender, RoutedEventArgs e)
    {
      ((TextBox) ((FrameworkElement) sender).Tag).Text = ((HeaderedItemsControl) sender).Header.ToString();
    }

    private async Task RunCommand()
    {
      string FC = this.ComboCommand.SelectedItem.ToString();
      string EFC = this.ComboExtCommand.SelectedItem.ToString();
      object addFC = this.ComboAddCommand.SelectedItem;
      string arg1 = string.IsNullOrEmpty(this.TextExtCommandArgument_1.Text) ? (string) null : this.TextExtCommandArgument_1.Text.Trim();
      string arg2 = string.IsNullOrEmpty(this.TextExtCommandArgument_2.Text) ? (string) null : this.TextExtCommandArgument_2.Text.Trim();
      string arg3 = string.IsNullOrEmpty(this.TextExtCommandArgument_3.Text) ? (string) null : this.TextExtCommandArgument_3.Text.Trim();
      string arg4 = string.IsNullOrEmpty(this.TextExtCommandArgument_4.Text) ? (string) null : this.TextExtCommandArgument_4.Text.Trim();
      string arg5 = string.IsNullOrEmpty(this.TextExtCommandArgument_5.Text) ? (string) null : this.TextExtCommandArgument_5.Text.Trim();
      try
      {
        if (FC.Contains("0x37"))
        {
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetNBIoTModulePartNumber))
          {
            byte[] retVal = await this.myNBIotCommands.GetNBIoT_ModulePartNumberAsync(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nNBIoT Module Part Number: " + Util.ByteArrayToHexString(retVal);
            retVal = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetNBIoTFirmwareVersion))
          {
            byte[] retVal = await this.myNBIotCommands.GetNBIoT_FirmwareVersionAsync(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nNBIoT Firmware Version (Hex): " + Util.ByteArrayToHexString(retVal);
            string retString = Encoding.Default.GetString(retVal);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nNBIoT Firmware Version (String): " + retString;
            string[] retCut = retString.Split(new string[1]
            {
              "\r\n"
            }, StringSplitOptions.RemoveEmptyEntries);
            string[] strArray = retCut;
            for (int index = 0; index < strArray.Length; ++index)
            {
              string item = strArray[index];
              if (item.ToLower().Contains("revision"))
              {
                string[] reversion = item.Split(':');
                NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nNBIoT Firmware Version (Cut): " + reversion[1];
                reversion = (string[]) null;
              }
              item = (string) null;
            }
            strArray = (string[]) null;
            retVal = (byte[]) null;
            retString = (string) null;
            retCut = (string[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetNBIoTIMEI_NB))
          {
            byte[] retVal = await this.myNBIotCommands.GetNBIoT_IMEI_NBAsync(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nNBIoT IMEI: (Dec): " + Encoding.Default.GetString(retVal);
            retVal = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetSIMIMSI_NB))
          {
            byte[] retVal = await this.myNBIotCommands.Get_SIM_IMSI_NBAsync(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nSIM IMSI (Dec): " + Encoding.Default.GetString(retVal);
            retVal = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetSIMICCID_NB))
          {
            byte[] retVal = await this.myNBIotCommands.GetSIM_ICCID_NBAsync(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nSIM ICCID (Dec): " + Encoding.Default.GetString(retVal);
            retVal = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetIMEI_IMSI_NBVER_RAM))
          {
            byte[] retVal = await this.myNBIotCommands.GetNBIoT_IMEI_IMSI_NBVER_RAMAsync(this.progress, this.cancelTokenSource.Token);
            if (retVal.Length >= 16)
            {
              byte[] imei = new byte[8];
              Array.Copy((Array) retVal, 0, (Array) imei, 0, 8);
              Array.Reverse((Array) imei);
              NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nIMEI (Hex): " + Util.ByteArrayToHexString(imei);
              byte[] imsi = new byte[8];
              Array.Copy((Array) retVal, 8, (Array) imsi, 0, 8);
              Array.Reverse((Array) imsi);
              NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nIMSI (Hex): " + Util.ByteArrayToHexString(imsi);
              if (retVal.Length > 16)
              {
                byte[] nbver = new byte[retVal.Length - 16];
                Array.Copy((Array) retVal, 16, (Array) nbver, 0, retVal.Length - 16);
                Array.Reverse((Array) nbver);
                NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nNBVer (String): " + Encoding.Default.GetString(nbver);
                nbver = (byte[]) null;
              }
              imei = (byte[]) null;
              imsi = (byte[]) null;
            }
            retVal = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetICCID_IMSI_RAM))
          {
            byte[] retVal = await this.myNBIotCommands.GetNBIoT_ICCID_IMSI_RAMAsync(this.progress, this.cancelTokenSource.Token);
            if (retVal.Length >= 18)
            {
              byte[] iccid = new byte[10];
              Array.Copy((Array) retVal, 0, (Array) iccid, 0, 10);
              Array.Reverse((Array) iccid);
              NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nICCID (Hex): " + Util.ByteArrayToHexString(iccid);
              byte[] imsi = new byte[8];
              Array.Copy((Array) retVal, 10, (Array) imsi, 0, 8);
              Array.Reverse((Array) imsi);
              NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nIMSI (Hex): " + Util.ByteArrayToHexString(imsi);
              iccid = (byte[]) null;
              imsi = (byte[]) null;
            }
            retVal = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetProtocol))
          {
            byte[] retVal = await this.myNBIotCommands.GetNBIoT_Protocol(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nProtocol: " + Util.ByteArrayToHexString(retVal);
            if (retVal.Length == 1 && retVal[0] == (byte) 1)
              NBIoTCommandWindow.result += "\nProtocol: CoAP";
            else if (retVal.Length == 1 && retVal[0] == (byte) 2)
              NBIoTCommandWindow.result += "\nProtocol: UDP";
            else if (retVal.Length == 1 && retVal[0] == (byte) 3)
              NBIoTCommandWindow.result += "\nProtocol: TCP";
            retVal = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SetProtocol))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              this.updateContextMenu1(arg1);
              byte[] data = Util.HexStringToByteArray(arg1);
              if (data.Length == 1)
              {
                await this.myNBIotCommands.SetNBIoT_Protocol(data, this.progress, this.cancelTokenSource.Token);
                NBIoTCommandWindow.result += "\nSet NBIoT Protocol data succesfully sent to device.";
              }
              else
              {
                int num = (int) MessageBox.Show("To much bytes to send ... max. 1 Byte.\nPlease check the lenght of your bytearray.");
              }
              data = (byte[]) null;
            }
            else
            {
              int num1 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetBand))
          {
            byte[] retVal = await this.myNBIotCommands.GetNBIoT_Band(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nBand: " + Util.ByteArrayToHexString(retVal);
            retVal = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SetBand))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              this.updateContextMenu1(arg1);
              byte[] data = Util.HexStringToByteArray(arg1);
              if (data.Length == 1)
              {
                await this.myNBIotCommands.SetNBIoT_Band(data, this.progress, this.cancelTokenSource.Token);
                NBIoTCommandWindow.result += "\nSet NBIoT Band data succesfully sent to device.";
              }
              else
              {
                int num2 = (int) MessageBox.Show("To much bytes to send ... max. 1 Byte.\nPlease check the lenght of your bytearray.");
              }
              data = (byte[]) null;
            }
            else
            {
              int num3 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetSecondaryBand))
          {
            byte[] retVal = await this.myNBIotCommands.GetNBIoT_SecondaryBand(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nSecondaryBand: " + Util.ByteArrayToHexString(retVal);
            retVal = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SetSecondaryBand))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              this.updateContextMenu1(arg1);
              byte[] data = Util.HexStringToByteArray(arg1);
              if (data.Length == 1)
              {
                await this.myNBIotCommands.SetNBIoT_SecondaryBand(data, this.progress, this.cancelTokenSource.Token);
                NBIoTCommandWindow.result += "\nSet NBIoT Secondary Band data succesfully sent to device.";
              }
              else
              {
                int num4 = (int) MessageBox.Show("To much bytes to send ... max. 1 Byte.\nPlease check the lenght of your bytearray.");
              }
              data = (byte[]) null;
            }
            else
            {
              int num5 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetRemoteIP))
          {
            byte[] retVal = await this.myNBIotCommands.GetNBIoT_RemoteIP(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nRemote IP (Hex): " + Util.ByteArrayToHexString(retVal);
            if (retVal.Length == 4)
              NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nRemote IP (Dec): " + retVal[0].ToString() + "." + retVal[1].ToString() + "." + retVal[2].ToString() + "." + retVal[3].ToString();
            else
              NBIoTCommandWindow.result += "\nRemote IP data length is wrong";
            retVal = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SetRemoteIP))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              this.updateContextMenu1(arg1);
              string[] inputStr = arg1.Split('.');
              if (inputStr.Length == 4)
              {
                byte[] data = new byte[4]
                {
                  Convert.ToByte(inputStr[0]),
                  Convert.ToByte(inputStr[1]),
                  Convert.ToByte(inputStr[2]),
                  Convert.ToByte(inputStr[3])
                };
                if (data.Length == 4)
                {
                  await this.myNBIotCommands.SetNBIoT_RemoteIP(data, this.progress, this.cancelTokenSource.Token);
                  NBIoTCommandWindow.result += "\nSet NBIoT Remote IP data succesfully sent to device.";
                }
                else
                {
                  int num6 = (int) MessageBox.Show("To much bytes to send ... max. 1 Byte.\nPlease check the lenght of your bytearray.");
                }
                data = (byte[]) null;
              }
              else
              {
                int num7 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
              }
              inputStr = (string[]) null;
            }
            else
            {
              int num8 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetRemotePort))
          {
            byte[] retVal = await this.myNBIotCommands.GetNBIoT_RemotePort(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nRemote Port (Hex): " + Util.ByteArrayToHexString(retVal);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nRemote Port (Dec): " + ((int) retVal[1] * 256 + (int) retVal[0]).ToString();
            retVal = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SetRemotePort))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              this.updateContextMenu1(arg1);
              ushort decInt16 = Convert.ToUInt16(arg1);
              byte[] data = new byte[2]
              {
                (byte) ((uint) decInt16 & (uint) byte.MaxValue),
                (byte) ((int) decInt16 >> 8 & (int) byte.MaxValue)
              };
              await this.myNBIotCommands.SetNBIoT_RemotePort(data, this.progress, this.cancelTokenSource.Token);
              NBIoTCommandWindow.result += "\nSet NBIoT Coap Remote Port data succesfully sent to device.";
              data = (byte[]) null;
            }
            else
            {
              int num9 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetOperator))
            NBIoTCommandWindow.result += "\nGet Operator Is Not Released";
          if (EFC.Contains(NBIoTCommandWindow.CMD_SetOperator))
            NBIoTCommandWindow.result += "\nSet Operator Is Not Released";
          if (EFC.Contains(NBIoTCommandWindow.CMD_SetNBIoTPowerOn))
          {
            await this.myNBIotCommands.SetNBIoT_PowerON(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result += "\nSet NBIoT Power On succesfully sent to device.";
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SetNBIoTPowerOff))
          {
            await this.myNBIotCommands.SetNBIoT_PowerOFF(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result += "\nSet NBIoT Power Off succesfully sent to device.";
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SendCommonCommand))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              this.updateContextMenu1(arg1);
              byte[] data = Encoding.Default.GetBytes(arg1);
              if (data.Length > 2 && (double) data[0] == 65.0 && (double) data[1] == 84.0)
              {
                byte[] theData = await this.myNBIotCommands.NBIoT_CommonCommand(data, this.progress, this.cancelTokenSource.Token);
                NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nInfo:" + Encoding.Default.GetString(theData);
                theData = (byte[]) null;
              }
              else
              {
                int num10 = (int) MessageBox.Show("Please check the data of your input string.");
              }
              data = (byte[]) null;
            }
            else
            {
              int num11 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SendConfirmedData))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              this.updateContextMenu1(arg1);
              byte[] data = Util.HexStringToByteArray(arg1);
              if (data.Length <= 50)
              {
                await this.myNBIotCommands.SendConfirmedDataAsync(data, this.progress, this.cancelTokenSource.Token);
                NBIoTCommandWindow.result += "\nConfirmed data succesfully sent to device.";
              }
              else
              {
                int num12 = (int) MessageBox.Show("To much bytes to send ... max. 50 Bytes.\nPlease check the lenght of your bytearray.");
              }
              data = (byte[]) null;
            }
            else
            {
              int num13 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SendUnconfirmedData))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              this.updateContextMenu1(arg1);
              byte[] data = Util.HexStringToByteArray(arg1);
              if (data.Length <= 50)
              {
                await this.myNBIotCommands.SendUnconfirmedDataAsync(data, this.progress, this.cancelTokenSource.Token);
                NBIoTCommandWindow.result += "\nUnconfirmed data succesfully sent to device.";
              }
              else
              {
                int num14 = (int) MessageBox.Show("To much bytes to send ... max. 50 Bytes.\nPlease check the lenght of your bytearray.");
              }
              data = (byte[]) null;
            }
            else
            {
              int num15 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SetTransmissionScenario))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              byte[] data = Util.HexStringToByteArray(arg1);
              await this.myNBIotCommands.SetTransmissionScenarioAsync(data, this.progress, this.cancelTokenSource.Token);
              NBIoTCommandWindow.result += "\nTransmission scenario successfully set.";
              data = (byte[]) null;
            }
            else
            {
              int num16 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetTransmissionScenario))
          {
            byte retVal = await this.myNBIotCommands.GetTransmissionScenarioAsync(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nTransmission scenario: " + retVal.ToString();
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SetDeviceEUI))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              byte[] data = Util.HexStringToByteArray(arg1);
              await this.myNBIotCommands.SetDevEUIAsync(data, this.progress, this.cancelTokenSource.Token);
              NBIoTCommandWindow.result += "\nDevice EUI successfully sent to device.";
              data = (byte[]) null;
            }
            else
            {
              int num17 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetDeviceEUI))
          {
            byte[] retVal = await this.myNBIotCommands.GetDevEUIAsync(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nDevice EUI: " + Util.ByteArrayToHexString(retVal);
            retVal = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SendTestData))
          {
            byte[] retVal = await this.myNBIotCommands.NBIoT_SendTestData(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\n(Send Test Data)The back info: " + retVal?.ToString();
            retVal = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SendRadioFullFunctionOn))
          {
            byte[] ReturnData = await this.myNBIotCommands.SetNBIoT_RadioFullFunctionOn(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nReturn:" + Encoding.Default.GetString(ReturnData);
            ReturnData = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SendRadioFullFunctionOff))
          {
            byte[] ReturnData = await this.myNBIotCommands.SetNBIoT_RadioFullFunctionOff(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nReturn:" + Encoding.Default.GetString(ReturnData);
            ReturnData = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SendActivePacket))
          {
            await this.myNBIotCommands.NBIoT_SendActivePacket(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result += "\nActive Packet succesfully sent to device.";
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetDNSName))
          {
            byte[] retVal = await this.myNBIotCommands.GetDNSNameAsync(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nDNS Name: " + Encoding.Default.GetString(retVal);
            retVal = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SetDNSName))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              this.updateContextMenu1(arg1);
              byte[] data = Encoding.Default.GetBytes(arg1);
              if (data.Length > 1)
              {
                await this.myNBIotCommands.SetDNSNameAsync(data, this.progress, this.cancelTokenSource.Token);
                NBIoTCommandWindow.result += "\nDNS Name successfully sent to device.";
              }
              else
              {
                int num18 = (int) MessageBox.Show("Please check the data of your input string.");
              }
              data = (byte[]) null;
            }
            else
            {
              int num19 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetRadioSendingState))
          {
            byte[] retVal = await this.myNBIotCommands.GetRadioSendingState(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nRadioSendingState: " + Util.ByteArrayToHexString(retVal);
            retVal = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_RestNBModem))
          {
            byte Data = 0;
            if (!string.IsNullOrEmpty(arg1))
            {
              byte[] ConvertData = Util.HexStringToByteArray(arg1);
              if (ConvertData.Length > 1 || ConvertData.Length == 0)
              {
                int num20 = (int) MessageBox.Show("Wrong Argument");
                FC = (string) null;
                EFC = (string) null;
                addFC = (object) null;
                arg1 = (string) null;
                arg2 = (string) null;
                arg3 = (string) null;
                arg4 = (string) null;
                arg5 = (string) null;
                return;
              }
              Data = ConvertData[0];
              ConvertData = (byte[]) null;
            }
            byte[] ReturnData = await this.myNBIotCommands.ResetNBModem(this.progress, this.cancelTokenSource.Token, Data);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nRetrun:" + Encoding.Default.GetString(ReturnData);
            ReturnData = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SetNBModemAutoConnect))
          {
            byte[] ReturnData = await this.myNBIotCommands.SetNBModemManualOrAutoConnect((byte) 0, this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nReturn:" + Encoding.Default.GetString(ReturnData);
            ReturnData = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SetNBModemManualConnect))
          {
            byte[] ReturnData = await this.myNBIotCommands.SetNBModemManualOrAutoConnect((byte) 1, this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nReturn:" + Encoding.Default.GetString(ReturnData);
            ReturnData = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SetNBModemAPN))
          {
            byte[] APN = new byte[0];
            if (!string.IsNullOrEmpty(arg1))
            {
              byte[] ConvertData = Encoding.Default.GetBytes(arg1);
              if (ConvertData.Length == 0)
              {
                int num21 = (int) MessageBox.Show("Wrong Argument");
                FC = (string) null;
                EFC = (string) null;
                addFC = (object) null;
                arg1 = (string) null;
                arg2 = (string) null;
                arg3 = (string) null;
                arg4 = (string) null;
                arg5 = (string) null;
                return;
              }
              APN = ConvertData;
              await this.myNBIotCommands.SetNBModemAPN(APN, this.progress, this.cancelTokenSource.Token);
              NBIoTCommandWindow.result += "\nSet NB modem APN: Success";
              ConvertData = (byte[]) null;
              APN = (byte[]) null;
            }
            else
            {
              int num22 = (int) MessageBox.Show("Wrong Argument");
              FC = (string) null;
              EFC = (string) null;
              addFC = (object) null;
              arg1 = (string) null;
              arg2 = (string) null;
              arg3 = (string) null;
              arg4 = (string) null;
              arg5 = (string) null;
              return;
            }
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetNBModemAPN))
          {
            byte[] ReturnData = await this.myNBIotCommands.GetNBModemAPN(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nGet NB modem APN:" + Encoding.Default.GetString(ReturnData);
            ReturnData = (byte[]) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SetDNSServerIP))
          {
            List<byte> ServerIPBytes = new List<byte>();
            string FailString = string.Empty;
            byte[] IP;
            if (!this.ConvertFromIPString(arg1, out IP, out FailString))
            {
              NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nSet DNS Server IP: Failed" + Environment.NewLine + FailString;
              FC = (string) null;
              EFC = (string) null;
              addFC = (object) null;
              arg1 = (string) null;
              arg2 = (string) null;
              arg3 = (string) null;
              arg4 = (string) null;
              arg5 = (string) null;
              return;
            }
            ServerIPBytes.AddRange((IEnumerable<byte>) IP);
            if (!this.ConvertFromIPString(arg2, out IP, out FailString))
            {
              NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nSet DNS Server IP: Failed" + Environment.NewLine + FailString;
              FC = (string) null;
              EFC = (string) null;
              addFC = (object) null;
              arg1 = (string) null;
              arg2 = (string) null;
              arg3 = (string) null;
              arg4 = (string) null;
              arg5 = (string) null;
              return;
            }
            ServerIPBytes.AddRange((IEnumerable<byte>) IP);
            await this.myNBIotCommands.SetDNSServerIP(ServerIPBytes.ToArray(), this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result += "\nSet DNS Server IP: Success";
            ServerIPBytes = (List<byte>) null;
            IP = (byte[]) null;
            FailString = (string) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetDNSServerIP))
          {
            byte[] ServerIPBytes = await this.myNBIotCommands.GetDNSServerIP(this.progress, this.cancelTokenSource.Token);
            string IPString;
            string FailString;
            if (!this.ConvertToIPString(((IEnumerable<byte>) ServerIPBytes).Take<byte>(4).ToArray<byte>(), out IPString, out FailString))
            {
              NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nGet DNS Server IP: Failed" + Environment.NewLine + FailString;
              FC = (string) null;
              EFC = (string) null;
              addFC = (object) null;
              arg1 = (string) null;
              arg2 = (string) null;
              arg3 = (string) null;
              arg4 = (string) null;
              arg5 = (string) null;
              return;
            }
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nPrimary DNS Server IP: " + IPString;
            if (!this.ConvertToIPString(((IEnumerable<byte>) ServerIPBytes).Skip<byte>(4).Take<byte>(4).ToArray<byte>(), out IPString, out FailString))
            {
              NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nGet DNS Server IP: Failed" + Environment.NewLine + FailString;
              FC = (string) null;
              EFC = (string) null;
              addFC = (object) null;
              arg1 = (string) null;
              arg2 = (string) null;
              arg3 = (string) null;
              arg4 = (string) null;
              arg5 = (string) null;
              return;
            }
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nSecondary DNS Server IP: " + IPString;
            ServerIPBytes = (byte[]) null;
            IPString = (string) null;
            FailString = (string) null;
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SetDNSEnableByte))
          {
            byte b;
            if (!byte.TryParse(arg1, out b))
            {
              NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nSet DNS Enable Byte: Faled" + Environment.NewLine + "Can not convert to byte";
              FC = (string) null;
              EFC = (string) null;
              addFC = (object) null;
              arg1 = (string) null;
              arg2 = (string) null;
              arg3 = (string) null;
              arg4 = (string) null;
              arg5 = (string) null;
              return;
            }
            await this.myNBIotCommands.SetDNSEnableByte(b, this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result += "\nSet DNS Enable Byte: Success";
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetDNSEnableByte))
          {
            byte b = await this.myNBIotCommands.GetDNSEnableByte(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nGet DNS Enable Byte: " + b.ToString();
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_SetAPNEnableByte))
          {
            byte b;
            if (!byte.TryParse(arg1, out b))
            {
              NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nSet APN Enable Byte: Faled" + Environment.NewLine + "Can not convert to byte";
              FC = (string) null;
              EFC = (string) null;
              addFC = (object) null;
              arg1 = (string) null;
              arg2 = (string) null;
              arg3 = (string) null;
              arg4 = (string) null;
              arg5 = (string) null;
              return;
            }
            if (b != (byte) 0 && b != (byte) 1)
            {
              NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nSet APN Enable Byte: Faled" + Environment.NewLine + "Only 0 or 1 is permitted";
              FC = (string) null;
              EFC = (string) null;
              addFC = (object) null;
              arg1 = (string) null;
              arg2 = (string) null;
              arg3 = (string) null;
              arg4 = (string) null;
              arg5 = (string) null;
              return;
            }
            await this.myNBIotCommands.SetAPNEanbleByte(b, this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result += "\nSet APN Enable Byte: Success";
          }
          if (EFC.Contains(NBIoTCommandWindow.CMD_GetAPNEnableByte))
          {
            byte b = await this.myNBIotCommands.GetAPNEnabledByte(this.progress, this.cancelTokenSource.Token);
            NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nGet APN Enable Byte: " + b.ToString();
            NBIoTCommandWindow.result += "\n(0 : Will set the APN)";
            NBIoTCommandWindow.result += "\n(1 : Will not set the APN during sending procedure)";
          }
        }
        if (string.IsNullOrEmpty(NBIoTCommandWindow.result))
        {
          FC = (string) null;
          EFC = (string) null;
          addFC = (object) null;
          arg1 = (string) null;
          arg2 = (string) null;
          arg3 = (string) null;
          arg4 = (string) null;
          arg5 = (string) null;
        }
        else
        {
          this.TextBoxUniversalCommandResult.Text = NBIoTCommandWindow.result;
          FC = (string) null;
          EFC = (string) null;
          addFC = (object) null;
          arg1 = (string) null;
          arg2 = (string) null;
          arg3 = (string) null;
          arg4 = (string) null;
          arg5 = (string) null;
        }
      }
      catch (Exception ex)
      {
        NBIoTCommandWindow.result = NBIoTCommandWindow.result + "\nFunction (" + EFC + ") \nERROR: " + ex.Message;
        this.TextBoxUniversalCommandResult.Text = NBIoTCommandWindow.result;
        FC = (string) null;
        EFC = (string) null;
        addFC = (object) null;
        arg1 = (string) null;
        arg2 = (string) null;
        arg3 = (string) null;
        arg4 = (string) null;
        arg5 = (string) null;
      }
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
        newItem.Tag = (object) this.TextExtCommandArgument_1;
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
        newItem.Tag = (object) this.TextExtCommandArgument_2;
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
        newItem.Tag = (object) this.TextExtCommandArgument_3;
        this.Argument3ValuesMenu.Items.Add((object) newItem);
      }
    }

    private void updateContextMenu4(string packet)
    {
      if (!this.Argument4_last_values.Contains(packet))
        this.Argument4_last_values.Add(packet);
      this.Argument4ValuesMenu.Items.Clear();
      if (this.Argument4_last_values.Count > 20)
        this.Argument4_last_values.RemoveRange(0, this.Argument4_last_values.Count - 20);
      foreach (string argument3LastValue in this.Argument3_last_values)
      {
        MenuItem newItem = new MenuItem();
        newItem.Header = (object) argument3LastValue;
        newItem.Click += new RoutedEventHandler(this.mi_Click);
        newItem.Tag = (object) this.TextExtCommandArgument_4;
        this.Argument4ValuesMenu.Items.Add((object) newItem);
      }
    }

    private void updateContextMenu5(string packet)
    {
      if (!this.Argument5_last_values.Contains(packet))
        this.Argument5_last_values.Add(packet);
      this.Argument5ValuesMenu.Items.Clear();
      if (this.Argument5_last_values.Count > 20)
        this.Argument5_last_values.RemoveRange(0, this.Argument5_last_values.Count - 20);
      foreach (string argument3LastValue in this.Argument3_last_values)
      {
        MenuItem newItem = new MenuItem();
        newItem.Header = (object) argument3LastValue;
        newItem.Click += new RoutedEventHandler(this.mi_Click);
        newItem.Tag = (object) this.TextExtCommandArgument_5;
        this.Argument5ValuesMenu.Items.Add((object) newItem);
      }
    }

    private bool ConvertFromIPString(string IPString, out byte[] IPBytes, out string FailString)
    {
      IPBytes = new byte[4];
      FailString = "IP Format Wrong";
      if (string.IsNullOrEmpty(IPString))
        return false;
      string[] strArray = IPString.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
      if (strArray.Length != 4)
        return false;
      for (int index = 0; index < 4; ++index)
      {
        byte result = 0;
        if (!byte.TryParse(strArray[index], out result))
          return false;
        IPBytes[index] = result;
      }
      FailString = string.Empty;
      return true;
    }

    private bool ConvertToIPString(byte[] IPBytes, out string IPString, out string FailString)
    {
      IPString = string.Empty;
      FailString = string.Empty;
      if (IPBytes.Length != 4)
      {
        FailString = "IP Byte Array Wrong";
        return false;
      }
      for (int index = 0; index < 4; ++index)
      {
        if (index > 0)
          IPString += ".";
        IPString += IPBytes[index].ToString();
      }
      return true;
    }

    private void CheckBoxEncryption_Checked(object sender, RoutedEventArgs e)
    {
      this.myNBIotCommands.enDeCrypt = true;
      this.TextBoxEncryptionKey.Visibility = Visibility.Visible;
      this.EncryptionKey_Label.Visibility = Visibility.Visible;
    }

    private void CheckBoxEncryption_UnChecked(object sender, RoutedEventArgs e)
    {
      this.myNBIotCommands.enDeCrypt = false;
      this.TextBoxEncryptionKey.Visibility = Visibility.Collapsed;
      this.EncryptionKey_Label.Visibility = Visibility.Collapsed;
    }

    private void TextBoxEncryptionKey_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.myNBIotCommands.AES_Key = this.TextBoxEncryptionKey.Text;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/commandwindownbiot.xaml", UriKind.Relative));
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
          this.ProgressBar1 = (ProgressBar) target;
          break;
        case 3:
          this.TextBoxUniversalCommandResult = (TextBox) target;
          break;
        case 4:
          this.StackPanalButtons = (StackPanel) target;
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
          this.ComboExtCommand_Label = (Label) target;
          break;
        case 10:
          this.ComboExtCommand = (ComboBox) target;
          this.ComboExtCommand.SelectionChanged += new SelectionChangedEventHandler(this.ComboExtCommand_SelectionChanged);
          break;
        case 11:
          this.ComboAddCommand_Label = (Label) target;
          break;
        case 12:
          this.ComboAddCommand = (ComboBox) target;
          this.ComboAddCommand.SelectionChanged += new SelectionChangedEventHandler(this.ComboAddCommand_SelectionChanged);
          break;
        case 13:
          this.TextArgument_1_Label = (Label) target;
          break;
        case 14:
          this.TextExtCommandArgument_1 = (TextBox) target;
          break;
        case 15:
          this.TextArgument_2_Label = (Label) target;
          break;
        case 16:
          this.TextExtCommandArgument_2 = (TextBox) target;
          break;
        case 17:
          this.TextArgument_3_Label = (Label) target;
          break;
        case 18:
          this.TextExtCommandArgument_3 = (TextBox) target;
          break;
        case 19:
          this.TextArgument_4_Label = (Label) target;
          break;
        case 20:
          this.TextExtCommandArgument_4 = (TextBox) target;
          break;
        case 21:
          this.TextArgument_5_Label = (Label) target;
          break;
        case 22:
          this.TextExtCommandArgument_5 = (TextBox) target;
          break;
        case 23:
          this.StackPanalButtons2 = (StackPanel) target;
          break;
        case 24:
          this.ButtonRunCommand = (Button) target;
          this.ButtonRunCommand.Click += new RoutedEventHandler(this.ButtonRunCommand_Click);
          break;
        case 25:
          this.ButtonRunCommandPreview = (Button) target;
          this.ButtonRunCommandPreview.Click += new RoutedEventHandler(this.ButtonRunCommandPreview_Click);
          break;
        case 26:
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
