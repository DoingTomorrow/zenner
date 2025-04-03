// Decompiled with JetBrains decompiler
// Type: HandlerLib.RadioCommandWindow
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
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class RadioCommandWindow : Window, IComponentConnector
  {
    private CommonRadioCommands myRadioCommands;
    private IPort myPort;
    private List<string> Argument1_last_values;
    private List<string> Argument2_last_values;
    private List<string> Argument3_last_values;
    private List<string> Argument4_last_values;
    private List<string> Argument5_last_values;
    private List<string> Argument6_last_values;
    private ContextMenu Argument1ValuesMenu;
    private ContextMenu Argument2ValuesMenu;
    private ContextMenu Argument3ValuesMenu;
    private ContextMenu Argument4ValuesMenu;
    private ContextMenu Argument5ValuesMenu;
    private ContextMenu Argument6ValuesMenu;
    private static string result = "";
    private static readonly string CMD_GetRadioVersion = "Get radio version (0x00)";
    private static readonly string CMD_SetTransmitPower = "Set transmit power (0x05)";
    private static readonly string CMD_GetTransmitPower = "Get transmit power (0x05)";
    private static readonly string CMD_SetCenterFrequency = "Set center frequency (0x06)";
    private static readonly string CMD_GetCenterFrequency = "Get center frequency (0x06)";
    private static readonly string CMD_SetFrequencyIncrement = "Set frequency increment (0x07)";
    private static readonly string CMD_GetFrequencyIncrement = "Get frequency increment (0x07)";
    private static readonly string CMD_SetCarrierMode = "Set carrier Mode (0x08)";
    private static readonly string CMD_GetCarrierMode = "Get carrier Mode (0x08)";
    private static readonly string CMD_SetFrequencyDeviation = "Set TX frequency deviation (0x09)";
    private static readonly string CMD_GetFrequencyDeviation = "Get TX frequency deviation (0x09)";
    private static readonly string CMD_SetBandwidth = "Set RX bandwidth (0x0a)";
    private static readonly string CMD_GetBandwidth = "Get RX bandwidth (0x0a)";
    private static readonly string CMD_SetTxDataRate = "Set Tx data rate (0x0b)";
    private static readonly string CMD_GetTxDataRate = "Get Tx data rate (0x0b)";
    private static readonly string CMD_SetRxDataRate = "Set Rx data rate (0x0c)";
    private static readonly string CMD_GetRxDataRate = "Get Rx data rate (0x0c)";
    private static readonly string CMD_StopRadioTest = "Stop radio tests (0x20)";
    private static readonly string CMD_TransmitUnModulatedCarrier = "Transmit unmodulated carrier (0x21)";
    private static readonly string CMD_TransmitModulatedCarrier = "Transmit modulated carrier (0x22)";
    private static readonly string CMD_SendTestPacket = "Send test packet (0x23)";
    private static readonly string CMD_ReceiveRadio3TelegramViaRadio = "Receive radio3 scenario 3 telegram via radio (0x24)";
    private static readonly string CMD_ReceiveAndStreamRadio3Telegrams = "Receive and stream radio3 scenario 3 telegrams (0x25)";
    private static readonly string CMD_MonitorRadio = "Monitor radio  (0x26)";
    private static readonly string CMD_EchoRadio = "Echo radio3 telegram via radio (0x27)";
    private static readonly string CMD_SetTxBandwidth = "Set TX bandwidth (0x28)";
    private static readonly string CMD_GetTxBandwidth = "Get TX bandwidth (0x28)";
    private static readonly string CMD_StartTransmissionCycle = "StartTransmissionCycle (0x29)";
    private static readonly string CMD_SetNFCField = "Set NFC Field (0x2a)";
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
    internal Label TextArgument_6_Label;
    internal TextBox TextExtCommandArgument_6;
    internal StackPanel StackPanalButtons2;
    internal Button ButtonRunCommand;
    internal Button ButtonRunCommandPreview;
    internal Button ButtonBreak;
    private bool _contentLoaded;

    public RadioCommandWindow(CommonRadioCommands RadioCMDs, IPort Port)
    {
      this.InitializeComponent();
      this.myRadioCommands = RadioCMDs;
      this.myRadioCommands.setCryptValuesFromBaseClass();
      this.myPort = Port;
      this.ButtonRunCommand.IsEnabled = false;
      this.SetArgumentFields((Dictionary<int, string>) null);
      this.setFunctionCodes();
      this.Argument1_last_values = new List<string>();
      this.Argument2_last_values = new List<string>();
      this.Argument3_last_values = new List<string>();
      this.Argument4_last_values = new List<string>();
      this.Argument5_last_values = new List<string>();
      this.Argument6_last_values = new List<string>();
      this.Argument1ValuesMenu = new ContextMenu();
      this.Argument2ValuesMenu = new ContextMenu();
      this.Argument3ValuesMenu = new ContextMenu();
      this.Argument4ValuesMenu = new ContextMenu();
      this.Argument5ValuesMenu = new ContextMenu();
      this.Argument6ValuesMenu = new ContextMenu();
      this.setEncryptionState();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
    }

    private void setEncryptionState()
    {
      this.CheckBoxEncryption.IsChecked = new bool?(this.myRadioCommands.enDeCrypt);
      if (!this.myRadioCommands.enDeCrypt)
        this.CheckBoxEncryption_UnChecked((object) null, (RoutedEventArgs) null);
      this.TextBoxEncryptionKey.Text = this.myRadioCommands.AES_Key;
    }

    private void setFunctionCodes()
    {
      this.ComboExtCommand_Label.Visibility = Visibility.Hidden;
      this.ComboExtCommand.Visibility = Visibility.Hidden;
      this.ComboCommand.Items.Clear();
      this.ComboCommand.Items.Add((object) "Radio Test Commands (0x2f)");
      this.ComboCommand.SelectedIndex = 0;
    }

    private void setRadioCommands()
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
        this.TextBoxUniversalCommandResult.Text = "Timeout!";
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
      RadioCommandWindow.result = string.Empty;
      await this.RunCommandFrame();
    }

    private async void ButtonRunCommandPreview_Click(object sender, RoutedEventArgs e)
    {
      RadioCommandWindow.result = "Actual radio data from connected device: \n-----------------------------------------------------";
      this.ComboExtCommand.SelectedItem = (object) RadioCommandWindow.CMD_GetRadioVersion;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) RadioCommandWindow.CMD_GetTransmitPower;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) RadioCommandWindow.CMD_GetCenterFrequency;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) RadioCommandWindow.CMD_GetBandwidth;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) RadioCommandWindow.CMD_GetFrequencyDeviation;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) RadioCommandWindow.CMD_GetFrequencyIncrement;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) RadioCommandWindow.CMD_GetCarrierMode;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) RadioCommandWindow.CMD_GetTxDataRate;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) RadioCommandWindow.CMD_GetRxDataRate;
      await this.RunCommandFrame();
    }

    private void ComboExtCommand_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.ComboExtCommand.SelectedItem != null)
      {
        object selectedItem = this.ComboExtCommand.SelectedItem;
        this.ComboAddCommand.Items.Clear();
        Dictionary<int, string> template = new Dictionary<int, string>();
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_SetTransmitPower))
          template.Add(1, "Transmit power:");
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_SetCenterFrequency))
          template.Add(1, "Center frequency:");
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_SetFrequencyIncrement))
          template.Add(1, "Increment value in Hz:");
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_SetCarrierMode))
          template.Add(0, "Carrier mode:");
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_SetFrequencyDeviation))
          template.Add(1, "Frequency deviation:");
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_SetBandwidth))
        {
          template.Add(1, "Bandwidth:");
          template.Add(2, "AFC value:");
        }
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_SetTxDataRate))
          template.Add(1, "Data rate (Baudrate):");
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_SetRxDataRate))
          template.Add(1, "Data rate (Baudrate):");
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_TransmitUnModulatedCarrier))
          template.Add(1, "Timeout in seconds:");
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_TransmitModulatedCarrier))
          template.Add(1, "Timeout in seconds:");
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_SendTestPacket))
        {
          template.Add(1, "Interval in seconds:");
          template.Add(2, "Timeout in seconds:");
          template.Add(3, "Data to transmit (HEX):");
        }
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_ReceiveRadio3TelegramViaRadio))
        {
          template.Add(1, "Telegram size:");
          template.Add(2, "SyncWord (2 Byte HEX):");
          template.Add(3, "Timeout in seconds (1..255):");
        }
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_ReceiveAndStreamRadio3Telegrams))
        {
          template.Add(1, "Telegram size:");
          template.Add(2, "Timeout in seconds:");
        }
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_MonitorRadio))
          template.Add(1, "Timeout in seconds:");
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_EchoRadio))
        {
          template.Add(1, "Telegram size:");
          template.Add(2, "SyncWord (2 Byte HEX):");
          template.Add(3, "Timeout in seconds (1..65536):");
          template.Add(12, "91D3");
        }
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_SetTxBandwidth))
          template.Add(1, "Bandwidth:");
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_StartTransmissionCycle))
        {
          template.Add(1, "No. of first channel: (0-71)");
          template.Add(2, "Total number of channels: (1-72)");
          template.Add(3, "Payload length (1-60):");
          template.Add(4, "No. of cycles:");
          template.Add(5, "Spreading factor:");
          template.Add(6, "Bandwidth [kHz]:");
        }
        if (selectedItem.ToString().Contains(RadioCommandWindow.CMD_SetNFCField))
        {
          template.Add(1, "Function:");
          template.Add(2, "Timeout:");
        }
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
      if (this.ComboCommand.SelectedItem.ToString().Contains("0x2f"))
      {
        this.setRadioCommands();
        this.ComboExtCommand_Label.Content = (object) "Radio Commands:";
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

    private void SetArgumentFields(Dictionary<int, string> template)
    {
      this.TextExtCommandArgument_1.Text = string.Empty;
      this.TextExtCommandArgument_2.Text = string.Empty;
      this.TextExtCommandArgument_3.Text = string.Empty;
      this.TextExtCommandArgument_4.Text = string.Empty;
      this.TextExtCommandArgument_5.Text = string.Empty;
      this.TextExtCommandArgument_6.Text = string.Empty;
      this.ComboAddCommand_Label.Visibility = Visibility.Collapsed;
      this.ComboAddCommand.Visibility = Visibility.Collapsed;
      this.TextExtCommandArgument_1.Visibility = Visibility.Collapsed;
      this.TextExtCommandArgument_2.Visibility = Visibility.Collapsed;
      this.TextExtCommandArgument_3.Visibility = Visibility.Collapsed;
      this.TextExtCommandArgument_4.Visibility = Visibility.Collapsed;
      this.TextExtCommandArgument_5.Visibility = Visibility.Collapsed;
      this.TextExtCommandArgument_6.Visibility = Visibility.Collapsed;
      this.TextArgument_1_Label.Visibility = Visibility.Collapsed;
      this.TextArgument_2_Label.Visibility = Visibility.Collapsed;
      this.TextArgument_3_Label.Visibility = Visibility.Collapsed;
      this.TextArgument_4_Label.Visibility = Visibility.Collapsed;
      this.TextArgument_5_Label.Visibility = Visibility.Collapsed;
      this.TextArgument_6_Label.Visibility = Visibility.Collapsed;
      if (template == null)
        return;
      foreach (KeyValuePair<int, string> keyValuePair in template)
      {
        if (keyValuePair.Key == 0 || keyValuePair.Key == 10)
        {
          if (keyValuePair.Key == 0)
          {
            this.ComboAddCommand.Visibility = Visibility.Visible;
            this.ComboAddCommand.Items.Add((object) "FSK         (0x00)");
            this.ComboAddCommand.Items.Add((object) "GFSK        (0x01)");
            this.ComboAddCommand.Items.Add((object) "OOK         (0x02)");
            this.ComboAddCommand.Items.Add((object) "SIGFOX      (0x03)");
            this.ComboAddCommand.Items.Add((object) "LoRa SF7    (0x04)");
            this.ComboAddCommand.Items.Add((object) "LoRa SF8    (0x05)");
            this.ComboAddCommand.Items.Add((object) "LoRa SF9    (0x06)");
            this.ComboAddCommand.Items.Add((object) "LoRa SF10   (0x07)");
            this.ComboAddCommand.Items.Add((object) "LoRa SF11   (0x08)");
            this.ComboAddCommand.Items.Add((object) "LoRa SF12   (0x09)");
            this.ComboAddCommand_Label.Visibility = Visibility.Visible;
            this.ComboAddCommand_Label.Content = (object) keyValuePair.Value;
          }
          if (keyValuePair.Key == 10)
            this.ComboAddCommand.Text = keyValuePair.Value;
        }
        if (keyValuePair.Key == 1 || keyValuePair.Key == 11)
        {
          if (keyValuePair.Key == 1)
          {
            this.TextExtCommandArgument_1.Visibility = Visibility.Visible;
            this.TextArgument_1_Label.Visibility = Visibility.Visible;
            this.TextArgument_1_Label.Content = (object) keyValuePair.Value;
            this.TextExtCommandArgument_1.ContextMenu = this.Argument1ValuesMenu;
          }
          if (keyValuePair.Key == 11)
            this.TextExtCommandArgument_1.Text = keyValuePair.Value;
        }
        if (keyValuePair.Key == 2 || keyValuePair.Key == 12)
        {
          if (keyValuePair.Key == 2)
          {
            this.TextExtCommandArgument_2.Visibility = Visibility.Visible;
            this.TextArgument_2_Label.Visibility = Visibility.Visible;
            this.TextArgument_2_Label.Content = (object) keyValuePair.Value;
            this.TextExtCommandArgument_2.ContextMenu = this.Argument2ValuesMenu;
          }
          if (keyValuePair.Key == 12)
            this.TextExtCommandArgument_2.Text = keyValuePair.Value;
        }
        if (keyValuePair.Key == 3 || keyValuePair.Key == 13)
        {
          if (keyValuePair.Key == 3)
          {
            this.TextExtCommandArgument_3.Visibility = Visibility.Visible;
            this.TextArgument_3_Label.Visibility = Visibility.Visible;
            this.TextArgument_3_Label.Content = (object) keyValuePair.Value;
            this.TextExtCommandArgument_3.ContextMenu = this.Argument3ValuesMenu;
          }
          if (keyValuePair.Key == 13)
            this.TextExtCommandArgument_3.Text = keyValuePair.Value;
        }
        if (keyValuePair.Key == 4 || keyValuePair.Key == 14)
        {
          if (keyValuePair.Key == 4)
          {
            this.TextExtCommandArgument_4.Visibility = Visibility.Visible;
            this.TextArgument_4_Label.Visibility = Visibility.Visible;
            this.TextArgument_4_Label.Content = (object) keyValuePair.Value;
            this.TextExtCommandArgument_4.ContextMenu = this.Argument4ValuesMenu;
          }
          if (keyValuePair.Key == 14)
            this.TextExtCommandArgument_4.Text = keyValuePair.Value;
        }
        if (keyValuePair.Key == 5 || keyValuePair.Key == 15)
        {
          if (keyValuePair.Key == 5)
          {
            this.TextExtCommandArgument_5.Visibility = Visibility.Visible;
            this.TextArgument_5_Label.Visibility = Visibility.Visible;
            this.TextArgument_5_Label.Content = (object) keyValuePair.Value;
            this.TextExtCommandArgument_5.ContextMenu = this.Argument5ValuesMenu;
          }
          if (keyValuePair.Key == 15)
            this.TextExtCommandArgument_5.Text = keyValuePair.Value;
        }
        if (keyValuePair.Key == 6 || keyValuePair.Key == 16)
        {
          if (keyValuePair.Key == 6)
          {
            this.TextExtCommandArgument_6.Visibility = Visibility.Visible;
            this.TextArgument_6_Label.Visibility = Visibility.Visible;
            this.TextArgument_6_Label.Content = (object) keyValuePair.Value;
            this.TextExtCommandArgument_6.ContextMenu = this.Argument5ValuesMenu;
          }
          if (keyValuePair.Key == 16)
            this.TextExtCommandArgument_6.Text = keyValuePair.Value;
        }
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
          this.TextExtCommandArgument_1.Text = keyValuePair.Value;
        if (keyValuePair.Key == 2)
          this.TextExtCommandArgument_2.Text = keyValuePair.Value;
        if (keyValuePair.Key == 3)
          this.TextExtCommandArgument_3.Text = keyValuePair.Value;
        if (keyValuePair.Key == 4)
          this.TextExtCommandArgument_4.Text = keyValuePair.Value;
        if (keyValuePair.Key == 5)
          this.TextExtCommandArgument_5.Text = keyValuePair.Value;
        if (keyValuePair.Key == 6)
          this.TextExtCommandArgument_6.Text = keyValuePair.Value;
      }
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
      string arg6 = string.IsNullOrEmpty(this.TextExtCommandArgument_6.Text) ? (string) null : this.TextExtCommandArgument_6.Text.Trim();
      try
      {
        if (FC.Contains("2f"))
        {
          if (EFC.Contains(RadioCommandWindow.CMD_GetRadioVersion))
          {
            ushort version = await this.myRadioCommands.GetRadioVersionAsync(this.progress, this.cancelTokenSource.Token);
            RadioCommandWindow.result = RadioCommandWindow.result + "\nRadio version is " + version.ToString() + " (0x" + version.ToString("x4") + ")";
          }
          uint num1;
          if (EFC.Contains(RadioCommandWindow.CMD_SetTransmitPower))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              string str;
              if (!arg1.Contains("0x"))
              {
                str = arg1;
              }
              else
              {
                num1 = uint.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                str = num1.ToString();
              }
              arg1 = str;
              await this.myRadioCommands.SetTransmitPowerAsync(ushort.Parse(arg1), this.progress, this.cancelTokenSource.Token);
              RadioCommandWindow.result = RadioCommandWindow.result + "\nTransmit power set to " + arg1;
            }
            else
            {
              int num2 = (int) MessageBox.Show("Transmit power was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(RadioCommandWindow.CMD_GetTransmitPower))
          {
            ushort power = await this.myRadioCommands.GetTransmitPowerAsync(this.progress, this.cancelTokenSource.Token);
            RadioCommandWindow.result = RadioCommandWindow.result + "\nTransmit power is " + power.ToString() + " (0x" + power.ToString("x4") + ")";
          }
          if (EFC.Contains(RadioCommandWindow.CMD_SetCenterFrequency))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              string str;
              if (!arg1.Contains("0x"))
              {
                str = arg1;
              }
              else
              {
                num1 = uint.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                str = num1.ToString();
              }
              arg1 = str;
              await this.myRadioCommands.SetCenterFrequencyAsync(uint.Parse(arg1), this.progress, this.cancelTokenSource.Token);
              RadioCommandWindow.result = RadioCommandWindow.result + "\nCenter frequency set to " + arg1;
            }
            else
            {
              int num3 = (int) MessageBox.Show("Center frequency was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(RadioCommandWindow.CMD_GetCenterFrequency))
          {
            uint value = await this.myRadioCommands.GetCenterFrequencyAsync(this.progress, this.cancelTokenSource.Token);
            RadioCommandWindow.result = RadioCommandWindow.result + "\nCenter frequency is " + value.ToString() + " (0x" + value.ToString("x8") + ")";
          }
          if (EFC.Contains(RadioCommandWindow.CMD_SetFrequencyIncrement))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              string str;
              if (!arg1.Contains("0x"))
              {
                str = arg1;
              }
              else
              {
                num1 = uint.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                str = num1.ToString();
              }
              arg1 = str;
              await this.myRadioCommands.SetFrequencyIncrementAsync(int.Parse(arg1), this.progress, this.cancelTokenSource.Token);
              RadioCommandWindow.result = RadioCommandWindow.result + "\nFrequency increment set to " + arg1;
            }
            else
            {
              int num4 = (int) MessageBox.Show("Frequency increment was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(RadioCommandWindow.CMD_GetFrequencyIncrement))
          {
            int value = await this.myRadioCommands.GetFrequencyIncrementAsync(this.progress, this.cancelTokenSource.Token);
            RadioCommandWindow.result = RadioCommandWindow.result + "\nFrequency increment is " + value.ToString();
          }
          if (EFC.Contains(RadioCommandWindow.CMD_SetCarrierMode))
          {
            if (addFC != null)
            {
              string mode = ushort.Parse(addFC.ToString().Split('(')[1].Substring(2, 2)).ToString();
              if (!string.IsNullOrEmpty(mode))
              {
                await this.myRadioCommands.SetCarrierModeAsync(byte.Parse(mode), this.progress, this.cancelTokenSource.Token);
                RadioCommandWindow.result = RadioCommandWindow.result + "\nFrequency increment set to " + this.ComboAddCommand.SelectedItem?.ToString();
              }
              else
              {
                int num5 = (int) MessageBox.Show("Carrier mode was not set correctly!\nPlease set a correct value and try again.");
              }
              mode = (string) null;
            }
            else
            {
              int num6 = (int) MessageBox.Show("Wrong or unknown carrier mode!\nPlease set a correct carrier mode and try again.");
            }
          }
          if (EFC.Contains(RadioCommandWindow.CMD_GetCarrierMode))
          {
            byte value = await this.myRadioCommands.GetCarrierModeAsync(this.progress, this.cancelTokenSource.Token);
            RadioCommandWindow.result = RadioCommandWindow.result + "\nCarrier mode is " + value.ToString();
          }
          if (EFC.Contains(RadioCommandWindow.CMD_SetFrequencyDeviation))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              string str;
              if (!arg1.Contains("0x"))
              {
                str = arg1;
              }
              else
              {
                num1 = uint.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                str = num1.ToString();
              }
              arg1 = str;
              await this.myRadioCommands.SetFrequencyDeviationAsync(ushort.Parse(arg1), this.progress, this.cancelTokenSource.Token);
              RadioCommandWindow.result = RadioCommandWindow.result + "\nFrequency deviation set to " + arg1;
            }
            else
            {
              int num7 = (int) MessageBox.Show("Frequency deviation was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(RadioCommandWindow.CMD_GetFrequencyDeviation))
          {
            ushort value = await this.myRadioCommands.GetFrequencyDeviationAsync(this.progress, this.cancelTokenSource.Token);
            RadioCommandWindow.result = RadioCommandWindow.result + "\nFrequency deviation is " + value.ToString() + " (0x" + value.ToString("x4") + ")";
          }
          if (EFC.Contains(RadioCommandWindow.CMD_SetBandwidth))
          {
            if (!string.IsNullOrEmpty(arg1) && !string.IsNullOrEmpty(arg2))
            {
              string str1;
              if (!arg1.Contains("0x"))
              {
                str1 = arg1;
              }
              else
              {
                num1 = uint.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                str1 = num1.ToString();
              }
              arg1 = str1;
              string str2;
              if (!arg2.Contains("0x"))
              {
                str2 = arg2;
              }
              else
              {
                num1 = uint.Parse(arg2.Substring(2), NumberStyles.HexNumber);
                str2 = num1.ToString();
              }
              arg2 = str2;
              await this.myRadioCommands.SetBandWidthAsync(uint.Parse(arg1), uint.Parse(arg2), this.progress, this.cancelTokenSource.Token);
              RadioCommandWindow.result = RadioCommandWindow.result + "Bandwidth set to " + arg1 + " and AFC set to " + arg2;
            }
            else
            {
              int num8 = (int) MessageBox.Show("Bandwidth or AFC value not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(RadioCommandWindow.CMD_GetBandwidth))
          {
            CommonRadioCommands.RadioBandWidth value = await this.myRadioCommands.GetBandWidthAsync(this.progress, this.cancelTokenSource.Token);
            RadioCommandWindow.result = RadioCommandWindow.result + "\nBandwidth is " + value.BandWidth.ToString() + " (0x" + value.BandWidth.ToString("x8") + ") \nAFC is " + value.AFC.ToString() + " (0x" + value.AFC.ToString("x8") + ")";
            value = (CommonRadioCommands.RadioBandWidth) null;
          }
          if (EFC.Contains(RadioCommandWindow.CMD_SetTxDataRate))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              string str;
              if (!arg1.Contains("0x"))
              {
                str = arg1;
              }
              else
              {
                num1 = uint.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                str = num1.ToString();
              }
              arg1 = str;
              await this.myRadioCommands.SetTxDataRateAsync(uint.Parse(arg1), this.progress, this.cancelTokenSource.Token);
              RadioCommandWindow.result = RadioCommandWindow.result + "\nData rate set to " + arg1;
            }
            else
            {
              int num9 = (int) MessageBox.Show("Data rate was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(RadioCommandWindow.CMD_GetTxDataRate))
          {
            uint value = await this.myRadioCommands.GetTxDataRateAsync(this.progress, this.cancelTokenSource.Token);
            RadioCommandWindow.result = RadioCommandWindow.result + "\nData rate is " + value.ToString() + " (0x" + value.ToString("x8") + ")";
          }
          if (EFC.Contains(RadioCommandWindow.CMD_SetRxDataRate))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              string str;
              if (!arg1.Contains("0x"))
              {
                str = arg1;
              }
              else
              {
                num1 = uint.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                str = num1.ToString();
              }
              arg1 = str;
              await this.myRadioCommands.SetRxDataRateAsync(uint.Parse(arg1), this.progress, this.cancelTokenSource.Token);
              RadioCommandWindow.result = RadioCommandWindow.result + "\nData rate set to " + arg1;
            }
            else
            {
              int num10 = (int) MessageBox.Show("Data rate was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(RadioCommandWindow.CMD_GetRxDataRate))
          {
            uint value = await this.myRadioCommands.GetRxDataRateAsync(this.progress, this.cancelTokenSource.Token);
            RadioCommandWindow.result = RadioCommandWindow.result + "\nData rate is " + value.ToString() + " (0x" + value.ToString("x8") + ")";
          }
          if (EFC.Contains(RadioCommandWindow.CMD_StopRadioTest))
          {
            await this.myRadioCommands.StopRadioTests(this.progress, this.cancelTokenSource.Token);
            RadioCommandWindow.result += "StopRadioTests successfully send.";
          }
          if (EFC.Contains(RadioCommandWindow.CMD_TransmitUnModulatedCarrier))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              await this.myRadioCommands.TransmitUnmodulatedCarrierAsync(ushort.Parse(arg1), this.progress, this.cancelTokenSource.Token);
              RadioCommandWindow.result += "Transmit unmodulated carrier command OK.";
            }
            else
            {
              int num11 = (int) MessageBox.Show("Timeout was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(RadioCommandWindow.CMD_TransmitModulatedCarrier))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              await this.myRadioCommands.TransmitModulatedCarrierAsync(ushort.Parse(arg1), this.progress, this.cancelTokenSource.Token);
              RadioCommandWindow.result += "Transmit modulated carrier command OK.";
            }
            else
            {
              int num12 = (int) MessageBox.Show("Timeout was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(RadioCommandWindow.CMD_SendTestPacket))
          {
            if (!string.IsNullOrEmpty(arg1) && !string.IsNullOrEmpty(arg2) && !string.IsNullOrEmpty(arg3))
            {
              this.updateContextMenu3(arg3);
              string str3;
              if (!arg1.Contains("0x"))
              {
                str3 = arg1;
              }
              else
              {
                num1 = uint.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                str3 = num1.ToString();
              }
              arg1 = str3;
              string str4;
              if (!arg2.Contains("0x"))
              {
                str4 = arg2;
              }
              else
              {
                num1 = uint.Parse(arg2.Substring(2), NumberStyles.HexNumber);
                str4 = num1.ToString();
              }
              arg2 = str4;
              byte[] data = Util.HexStringToByteArray(arg3);
              await this.myRadioCommands.SendTestPacketAsync(ushort.Parse(arg1), ushort.Parse(arg2), data, this.progress, this.cancelTokenSource.Token);
              RadioCommandWindow.result += "Send test packet command done.";
              data = (byte[]) null;
            }
            else
            {
              int num13 = (int) MessageBox.Show("Interval, timeout or data was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(RadioCommandWindow.CMD_ReceiveRadio3TelegramViaRadio))
          {
            if (!string.IsNullOrEmpty(arg1) || !string.IsNullOrEmpty(arg2) || arg2.Length == 4 || !string.IsNullOrEmpty(arg3))
            {
              string str5;
              if (!arg1.Contains("0x"))
              {
                str5 = arg1;
              }
              else
              {
                num1 = uint.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                str5 = num1.ToString();
              }
              arg1 = str5;
              string str6;
              if (!arg3.Contains("0x"))
              {
                str6 = arg3;
              }
              else
              {
                num1 = uint.Parse(arg3.Substring(2), NumberStyles.HexNumber);
                str6 = num1.ToString();
              }
              arg3 = str6;
              byte[] data = new byte[0];
              byte telegramSize = byte.Parse(arg1);
              byte[] syncWord = Util.HexStringToByteArray(arg2);
              byte timeout = byte.Parse(arg3);
              try
              {
                data = await this.myRadioCommands.ReceiveRadio3Scenario3TelegramViaRadioAsync(telegramSize, syncWord, timeout, this.progress, this.cancelTokenSource.Token);
                RadioCommandWindow.result = RadioCommandWindow.result + "Telegram received: " + (data.Length != 0 ? Util.ByteArrayToHexString(data) + "\n" : "none.\n");
              }
              catch (Exception ex)
              {
                if (!ex.Message.Contains("Timeout"))
                  throw new Exception("Exception: " + ex.Message);
                RadioCommandWindow.result += "no data received... \n";
              }
              finally
              {
                RadioCommandWindow.result += "DONE... \n";
              }
              data = (byte[]) null;
              syncWord = (byte[]) null;
            }
            else
            {
              int num14 = (int) MessageBox.Show("Timeout, SyncWord or Telegramsize was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(RadioCommandWindow.CMD_ReceiveAndStreamRadio3Telegrams))
          {
            try
            {
              if (!string.IsNullOrEmpty(arg1) && !string.IsNullOrEmpty(arg2))
              {
                byte[] data = new byte[0];
                byte size = 0;
                ushort timeout = 0;
                string str7;
                if (!arg1.Contains("0x"))
                {
                  str7 = arg1;
                }
                else
                {
                  num1 = uint.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                  str7 = num1.ToString();
                }
                arg1 = str7;
                string str8;
                if (!arg1.Contains("0x"))
                {
                  str8 = arg2;
                }
                else
                {
                  num1 = uint.Parse(arg2.Substring(2), NumberStyles.HexNumber);
                  str8 = num1.ToString();
                }
                arg2 = str8;
                if (!byte.TryParse(arg1, out size))
                  throw new Exception("Telegramm size is not set correctly");
                if (!ushort.TryParse(arg2, out timeout))
                  throw new Exception("Timeout is not set correctly!");
                await this.myRadioCommands.ReceiveAndStreamRadio3Scenario3TelegramsAsync(byte.Parse(arg1), ushort.Parse(arg2), this.progress, this.cancelTokenSource.Token);
                this.myPort.DiscardInBuffer();
                DateTime start = DateTime.Now;
                DateTime end = start.AddSeconds((double) timeout);
                while (DateTime.Now <= end)
                {
                  await Task.Delay(50);
                  byte[] buf = this.myPort.ReadExisting();
                  if (buf != null)
                    Buffer.BlockCopy((Array) buf, 0, (Array) data, data.Length == 0 ? 0 : data.Length, buf.Length);
                  buf = (byte[]) null;
                }
                RadioCommandWindow.result = RadioCommandWindow.result + "Data received: " + (data.Length != 0 ? Util.ByteArrayToHexString(data) : "none.");
                data = (byte[]) null;
              }
              else
              {
                int num15 = (int) MessageBox.Show("Timeout or telegram size was not set correctly!\nPlease set a correct value and try again.");
              }
            }
            catch (Exception ex)
            {
              int num16 = (int) MessageBox.Show("Error occoured: " + ex.Message);
            }
          }
          if (EFC.Contains(RadioCommandWindow.CMD_MonitorRadio))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              string str;
              if (!arg1.Contains("0x"))
              {
                str = arg1;
              }
              else
              {
                num1 = uint.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                str = num1.ToString();
              }
              arg1 = str;
              await this.myRadioCommands.MonitorRadioAsync(ushort.Parse(arg1), this.progress, this.cancelTokenSource.Token);
              RadioCommandWindow.result += "Command initiated ...";
            }
            else
            {
              int num17 = (int) MessageBox.Show("Timeout was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(RadioCommandWindow.CMD_EchoRadio))
          {
            if (!string.IsNullOrEmpty(arg1) || !string.IsNullOrEmpty(arg2) || arg2.Length == 4 || !string.IsNullOrEmpty(arg3))
            {
              string str9;
              if (!arg1.Contains("0x"))
              {
                str9 = arg1;
              }
              else
              {
                num1 = uint.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                str9 = num1.ToString();
              }
              arg1 = str9;
              string str10;
              if (!arg3.Contains("0x"))
              {
                str10 = arg3;
              }
              else
              {
                num1 = uint.Parse(arg3.Substring(2), NumberStyles.HexNumber);
                str10 = num1.ToString();
              }
              arg3 = str10;
              byte telegramSize = byte.Parse(arg1);
              byte[] syncWord = Util.HexStringToByteArray(arg2);
              ushort timeout = ushort.Parse(arg3);
              try
              {
                await this.myRadioCommands.EchoRadio3TelegramViaRadioAsync(telegramSize, syncWord, timeout, this.progress, this.cancelTokenSource.Token);
              }
              catch (Exception ex)
              {
                if (!ex.Message.Contains("Timeout"))
                  throw new Exception("Exception: " + ex.Message);
              }
              finally
              {
                RadioCommandWindow.result += "DONE... \n";
              }
              syncWord = (byte[]) null;
            }
            else
            {
              int num18 = (int) MessageBox.Show("Timeout, SyncWord or Telegramsize was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          ushort num19;
          if (EFC.Contains(RadioCommandWindow.CMD_SetTxBandwidth))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              string str;
              if (!arg1.Contains("0x"))
              {
                str = arg1;
              }
              else
              {
                num19 = ushort.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                str = num19.ToString();
              }
              arg1 = str;
              await this.myRadioCommands.SetTxBandWidthAsync(ushort.Parse(arg1), this.progress, this.cancelTokenSource.Token);
              RadioCommandWindow.result = RadioCommandWindow.result + "Bandwidth set to " + arg1;
            }
            else
            {
              int num20 = (int) MessageBox.Show("Bandwidth value not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(RadioCommandWindow.CMD_GetTxBandwidth))
          {
            ushort value = await this.myRadioCommands.GetTxBandWidthAsync(this.progress, this.cancelTokenSource.Token);
            RadioCommandWindow.result = RadioCommandWindow.result + "\nBandwidth is " + value.ToString() + " (0x" + value.ToString("x4") + ") ";
          }
          if (EFC.Contains(RadioCommandWindow.CMD_StartTransmissionCycle))
          {
            byte firstChannel = string.IsNullOrEmpty(arg1) ? (byte) 1 : (byte) ushort.Parse(arg1);
            byte totalChannels = string.IsNullOrEmpty(arg2) ? (byte) 1 : (byte) ushort.Parse(arg2);
            byte payloadLenght = string.IsNullOrEmpty(arg3) ? (byte) 1 : (byte) ushort.Parse(arg3);
            byte numberCycles = string.IsNullOrEmpty(arg4) ? (byte) 1 : (byte) ushort.Parse(arg4);
            byte spreadFactor = string.IsNullOrEmpty(arg5) ? (byte) 1 : (byte) ushort.Parse(arg5);
            ushort bandwidth = string.IsNullOrEmpty(arg6) ? (ushort) 1024 : ushort.Parse(arg6);
            await this.myRadioCommands.StartTransmissionCycleAsync(firstChannel, totalChannels, payloadLenght, numberCycles, spreadFactor, bandwidth, this.progress, this.cancelTokenSource.Token);
            RadioCommandWindow.result += "\rTransmission Cycle started.... ";
            RadioCommandWindow.result = RadioCommandWindow.result + "\r -> first channel:  " + arg1;
            RadioCommandWindow.result = RadioCommandWindow.result + "\r -> total channels: " + arg2;
            RadioCommandWindow.result = RadioCommandWindow.result + "\r -> payload length: " + arg3;
            RadioCommandWindow.result = RadioCommandWindow.result + "\r -> cycles:         " + arg4;
            RadioCommandWindow.result = RadioCommandWindow.result + "\r -> spread factor:  " + arg5;
            RadioCommandWindow.result = RadioCommandWindow.result + "\r -> bandwidth:      " + arg6;
          }
          if (EFC.Contains(RadioCommandWindow.CMD_SetNFCField))
          {
            if (!string.IsNullOrEmpty(arg1) && !string.IsNullOrEmpty(arg2))
            {
              arg1 = arg1.Contains("0x") ? byte.Parse(arg1.Substring(2), NumberStyles.HexNumber).ToString() : arg1;
              string str;
              if (!arg2.Contains("0x"))
              {
                str = arg2;
              }
              else
              {
                num19 = ushort.Parse(arg2.Substring(2), NumberStyles.HexNumber);
                str = num19.ToString();
              }
              arg2 = str;
              await this.myRadioCommands.SetNFCFieldAsync(byte.Parse(arg1), ushort.Parse(arg2), this.progress, this.cancelTokenSource.Token);
              RadioCommandWindow.result = RadioCommandWindow.result + "NFC Field set CMD: " + arg1 + " - Timeout: " + arg2;
            }
            else
            {
              int num21 = (int) MessageBox.Show("Set NFC Field values not set correctly!\nPlease set a correct value and try again.");
            }
          }
        }
        if (string.IsNullOrEmpty(RadioCommandWindow.result))
        {
          FC = (string) null;
          EFC = (string) null;
          addFC = (object) null;
          arg1 = (string) null;
          arg2 = (string) null;
          arg3 = (string) null;
          arg4 = (string) null;
          arg5 = (string) null;
          arg6 = (string) null;
        }
        else
        {
          this.TextBoxUniversalCommandResult.Text = RadioCommandWindow.result;
          FC = (string) null;
          EFC = (string) null;
          addFC = (object) null;
          arg1 = (string) null;
          arg2 = (string) null;
          arg3 = (string) null;
          arg4 = (string) null;
          arg5 = (string) null;
          arg6 = (string) null;
        }
      }
      catch (Exception ex)
      {
        RadioCommandWindow.result = RadioCommandWindow.result + "\nFunction (" + EFC + ") \nERROR: " + ex.Message;
        this.TextBoxUniversalCommandResult.Text = RadioCommandWindow.result;
        FC = (string) null;
        EFC = (string) null;
        addFC = (object) null;
        arg1 = (string) null;
        arg2 = (string) null;
        arg3 = (string) null;
        arg4 = (string) null;
        arg5 = (string) null;
        arg6 = (string) null;
      }
    }

    private void mi_Click(object sender, RoutedEventArgs e)
    {
      ((TextBox) ((FrameworkElement) sender).Tag).Text = ((HeaderedItemsControl) sender).Header.ToString();
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

    private void CheckBoxEncryption_Checked(object sender, RoutedEventArgs e)
    {
      this.myRadioCommands.enDeCrypt = true;
      this.TextBoxEncryptionKey.Visibility = Visibility.Visible;
      this.EncryptionKey_Label.Visibility = Visibility.Visible;
    }

    private void CheckBoxEncryption_UnChecked(object sender, RoutedEventArgs e)
    {
      this.myRadioCommands.enDeCrypt = false;
      this.TextBoxEncryptionKey.Visibility = Visibility.Collapsed;
      this.EncryptionKey_Label.Visibility = Visibility.Collapsed;
    }

    private void TextBoxEncryptionKey_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.myRadioCommands.AES_Key = this.TextBoxEncryptionKey.Text;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/commandwindowradio.xaml", UriKind.Relative));
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
          this.TextArgument_6_Label = (Label) target;
          break;
        case 24:
          this.TextExtCommandArgument_6 = (TextBox) target;
          break;
        case 25:
          this.StackPanalButtons2 = (StackPanel) target;
          break;
        case 26:
          this.ButtonRunCommand = (Button) target;
          this.ButtonRunCommand.Click += new RoutedEventHandler(this.ButtonRunCommand_Click);
          break;
        case 27:
          this.ButtonRunCommandPreview = (Button) target;
          this.ButtonRunCommandPreview.Click += new RoutedEventHandler(this.ButtonRunCommandPreview_Click);
          break;
        case 28:
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
