// Decompiled with JetBrains decompiler
// Type: HandlerLib.LoRaCommandWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

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
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class LoRaCommandWindow : Window, IComponentConnector
  {
    private CommonLoRaCommands myLoRaCommands;
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
    private static readonly string CMD_GetLoRaFCVersion = "Get LoRa FC version (0x00)";
    private static readonly string CMD_GetLoRaWANVersion = "Get LoRa WAN version (0x01)";
    private static readonly string CMD_SendJoinRequest = "Send join request (0x06)";
    private static readonly string CMD_CheckJoinRequest = "Check join accept (0x07)";
    private static readonly string CMD_SendUnconfirmedData = "Send unconfirmed data (0x08)";
    private static readonly string CMD_SendConfirmedData = "Send confirmed data (0x09)";
    private static readonly string CMD_SetNetID = "Set Net ID (0x20)";
    private static readonly string CMD_GetNetID = "Get Net ID (0x20)";
    private static readonly string CMD_SetAppEUI = "Set app EUI (0x21)";
    private static readonly string CMD_GetAppEUI = "Get app EUI (0x21)";
    private static readonly string CMD_SetNwkSKey = "Set network session key (0x22)";
    private static readonly string CMD_GetNwkSKey = "Get network session key (0x22)";
    private static readonly string CMD_SetAppSKey = "Set application session key (0x23)";
    private static readonly string CMD_GetAppSKey = "Get application session key (0x23)";
    private static readonly string CMD_SetOTAA_ABP = "Set OTAA-ABP (0x24)";
    private static readonly string CMD_GetOTAA_ABP = "Get OTAA_ABP (0x24)";
    private static readonly string CMD_SetDeviceEUI = "Set device EUI (0x25)";
    private static readonly string CMD_GetDeviceEUI = "Get device EUI (0x25)";
    private static readonly string CMD_SetAppKey = "Set application key (0x26)";
    private static readonly string CMD_GetAppKey = "Get application key (0x26)";
    private static readonly string CMD_SetDevAddr = "Set device address (0x27)";
    private static readonly string CMD_GetDevAddr = "Get device address (0x27)";
    private static readonly string CMD_SetTransmissionScenario = "Set transmission sceanrio (0x28)";
    private static readonly string CMD_GetTransmissionScenario = "Get transmission sceanrio (0x28)";
    private static readonly string CMD_SendConfigurationPaket = "Send configuration packet (0x29)";
    private static readonly string CMD_SetADR = "Set ADR (0x2a)";
    private static readonly string CMD_GetADR = "Get ADR (0x2a)";
    private static readonly string CMD_TriggerSystemDiagnostic = "TriggerSystemDiagnostic (0x2b)";
    private static readonly string CMD_SystemDiagnosticState = "SystemDiagnosticState (0x2b)";
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
    internal Label ComboAddCommand2_Label;
    internal ComboBox ComboAddCommand2;
    internal Label ComboAddCommand3_Label;
    internal ComboBox ComboAddCommand3;
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

    public LoRaCommandWindow(CommonLoRaCommands LoRaCMDs)
    {
      this.InitializeComponent();
      this.myLoRaCommands = LoRaCMDs;
      this.myLoRaCommands.setCryptValuesFromBaseClass();
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
      this.SetStopState();
      this.setEncryptionState();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
    }

    private void setEncryptionState()
    {
      this.CheckBoxEncryption.IsChecked = new bool?(this.myLoRaCommands.enDeCrypt);
      if (!this.myLoRaCommands.enDeCrypt)
        this.CheckBoxEncryption_UnChecked((object) null, (RoutedEventArgs) null);
      this.TextBoxEncryptionKey.Text = this.myLoRaCommands.AES_Key;
    }

    private void setFunctionCodes()
    {
      this.ComboExtCommand_Label.Visibility = Visibility.Hidden;
      this.ComboExtCommand.Visibility = Visibility.Hidden;
      this.ComboCommand.Items.Clear();
      this.ComboCommand.Items.Add((object) "LoRa Commands (0x35)");
      this.ComboCommand.SelectedIndex = 0;
    }

    private void setLoRaCommands()
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
      this.TextBoxEncryptionKey.IsEnabled = false;
      this.CheckBoxEncryption.IsEnabled = false;
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
      this.TextBoxEncryptionKey.IsEnabled = true;
      this.CheckBoxEncryption.IsEnabled = true;
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
      LoRaCommandWindow.result = string.Empty;
      await this.RunCommandFrame();
    }

    private async void ButtonRunCommandPreview_Click(object sender, RoutedEventArgs e)
    {
      LoRaCommandWindow.result = "Actual LoRa data of connected device:\n-------------------------------------------";
      Dictionary<int, string> args = new Dictionary<int, string>();
      this.ComboExtCommand.SelectedItem = (object) LoRaCommandWindow.CMD_GetLoRaFCVersion;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) LoRaCommandWindow.CMD_GetLoRaWANVersion;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) LoRaCommandWindow.CMD_GetNetID;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) LoRaCommandWindow.CMD_GetAppEUI;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) LoRaCommandWindow.CMD_GetNwkSKey;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) LoRaCommandWindow.CMD_GetAppSKey;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) LoRaCommandWindow.CMD_GetOTAA_ABP;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) LoRaCommandWindow.CMD_GetDeviceEUI;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) LoRaCommandWindow.CMD_GetAppKey;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) LoRaCommandWindow.CMD_GetDevAddr;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) LoRaCommandWindow.CMD_GetTransmissionScenario;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) LoRaCommandWindow.CMD_GetADR;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) LoRaCommandWindow.CMD_SystemDiagnosticState;
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
        Dictionary<int, string> values = new Dictionary<int, string>();
        if (selectedItem.ToString().Contains(LoRaCommandWindow.CMD_SendUnconfirmedData))
          template.Add(1, "Unconfirmed data:");
        if (selectedItem.ToString().Contains(LoRaCommandWindow.CMD_SendConfirmedData))
          template.Add(1, "Confirmed data:");
        if (selectedItem.ToString().Contains(LoRaCommandWindow.CMD_SetNetID))
          template.Add(1, "Net ID:");
        if (selectedItem.ToString().Contains(LoRaCommandWindow.CMD_SetAppEUI))
          template.Add(1, "Application EUI (8 Bytes):");
        if (selectedItem.ToString().Contains(LoRaCommandWindow.CMD_SetNwkSKey))
          template.Add(1, "Network session key (16 Bytes):");
        if (selectedItem.ToString().Contains(LoRaCommandWindow.CMD_SetAppSKey))
          template.Add(1, "Application session key:");
        if (selectedItem.ToString().Contains(LoRaCommandWindow.CMD_SetOTAA_ABP))
          template.Add(1, "Flag (0x01 OTAA) / (0x02 ABP):");
        if (selectedItem.ToString().Contains(LoRaCommandWindow.CMD_SetDeviceEUI))
          template.Add(1, "Device EUI (8 Bytes(reversed)):");
        if (selectedItem.ToString().Contains(LoRaCommandWindow.CMD_SetAppKey))
          template.Add(1, "Application Key (16 Bytes):");
        if (selectedItem.ToString().Contains(LoRaCommandWindow.CMD_SetDevAddr))
          template.Add(1, "Device address (4 Bytes):");
        if (selectedItem.ToString().Contains(LoRaCommandWindow.CMD_SetTransmissionScenario))
          template.Add(1, "Transmission scenario:");
        if (selectedItem.ToString().Contains(LoRaCommandWindow.CMD_SetADR))
          template.Add(1, "ADR:");
        if (selectedItem.ToString().Contains(LoRaCommandWindow.CMD_SendConfigurationPaket))
          template.Add(1, "PacketType (hex):");
        if (selectedItem.ToString().Contains(LoRaCommandWindow.CMD_TriggerSystemDiagnostic))
        {
          template.Add(10, "DiagnosticInterval:");
          template.Add(11, "DiagnosticRepeat:");
          template.Add(1, "DailyStartTime (24h):");
          values.Add(1, "00:00");
          template.Add(2, "DailyEndTime (24h):");
          values.Add(2, "00:00");
        }
        this.SetArgumentFields(template);
        this.SetArgumentFieldsValues(values);
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
      if (this.ComboCommand.SelectedItem.ToString().Contains("0x35"))
      {
        this.setLoRaCommands();
        this.ComboExtCommand_Label.Content = (object) "LoRa Commands (EFC):";
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

    private void ComboAddCommand2_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.ComboAddCommand2.SelectedIndex < 0)
        return;
      object selectedItem = this.ComboAddCommand2.SelectedItem;
    }

    private void ComboAddCommand3_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.ComboAddCommand3.SelectedIndex < 0)
        return;
      object selectedItem = this.ComboAddCommand3.SelectedItem;
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
        if (keyValuePair.Key == 10)
          this.ComboAddCommand2.SelectedItem = (object) keyValuePair.Value;
        if (keyValuePair.Key == 11)
          this.ComboAddCommand3.SelectedItem = (object) keyValuePair.Value;
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
      this.ComboAddCommand2_Label.Visibility = Visibility.Collapsed;
      this.ComboAddCommand2.Visibility = Visibility.Collapsed;
      this.ComboAddCommand3_Label.Visibility = Visibility.Collapsed;
      this.ComboAddCommand3.Visibility = Visibility.Collapsed;
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
        if (keyValuePair.Key == 10)
        {
          this.ComboAddCommand2.Visibility = Visibility.Visible;
          this.ComboAddCommand2.Items.Add((object) "Interval 0.25h  (0x00)");
          this.ComboAddCommand2.Items.Add((object) "Interval 0.50h  (0x01)");
          this.ComboAddCommand2.Items.Add((object) "Interval 1.00h  (0x02)");
          this.ComboAddCommand2.Items.Add((object) "Interval 2.00h  (0x03)");
          this.ComboAddCommand2.Items.Add((object) "Interval 3.00h  (0x04)");
          this.ComboAddCommand2.Items.Add((object) "Interval 6.00h  (0x05)");
          this.ComboAddCommand2.Items.Add((object) "Interval 12.00h  (0x06)");
          this.ComboAddCommand2.Items.Add((object) "Interval 24.00h  (0x07)");
          this.ComboAddCommand2_Label.Visibility = Visibility.Visible;
          this.ComboAddCommand2_Label.Content = (object) keyValuePair.Value;
        }
        if (keyValuePair.Key == 11)
        {
          this.ComboAddCommand3.Visibility = Visibility.Visible;
          this.ComboAddCommand3.Items.Add((object) "Repeats 5  (0x00)");
          this.ComboAddCommand3.Items.Add((object) "Repeats 10  (0x10)");
          this.ComboAddCommand3.Items.Add((object) "Repeats 20  (0x20)");
          this.ComboAddCommand3.Items.Add((object) "Repeats 50  (0x30)");
          this.ComboAddCommand3.Items.Add((object) "Repeats 100  (0x40)");
          this.ComboAddCommand3.Items.Add((object) "Repeats 200  (0x50)");
          this.ComboAddCommand3.Items.Add((object) "Repeats 500  (0x60)");
          this.ComboAddCommand3.Items.Add((object) "Repeats 1000  (0x70)");
          this.ComboAddCommand3_Label.Visibility = Visibility.Visible;
          this.ComboAddCommand3_Label.Content = (object) keyValuePair.Value;
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
      CommonLoRaCommands loRaCommands = this.myLoRaCommands;
      bool? isChecked = this.CheckBoxEncryption.IsChecked;
      bool flag1 = true;
      int num1 = isChecked.GetValueOrDefault() == flag1 & isChecked.HasValue ? 1 : 0;
      loRaCommands.enDeCrypt = num1 != 0;
      this.myLoRaCommands.AES_Key = this.TextBoxEncryptionKey.Text.Trim();
      try
      {
        if (FC.Contains("0x35"))
        {
          if (EFC.Contains(LoRaCommandWindow.CMD_GetLoRaFCVersion))
          {
            LoRaFcVersion retVal = await this.myLoRaCommands.GetLoRaFC_VersionAsync(this.progress, this.cancelTokenSource.Token);
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nLoRa FC Version: " + retVal.Version.ToString();
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nData: " + Util.ByteArrayToHexString(retVal.basedata);
            retVal = (LoRaFcVersion) null;
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_GetLoRaWANVersion))
          {
            LoRaWANVersion retVal = await this.myLoRaCommands.GetLoRaWAN_VersionAsync(this.progress, this.cancelTokenSource.Token);
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nLoRa WAN Version: " + ((int) retVal.MainVersion).ToString() + "." + ((int) retVal.MinorVersion).ToString() + "." + ((int) retVal.ReleaseNr).ToString();
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nData: " + Util.ByteArrayToHexString(retVal.basedata);
            retVal = (LoRaWANVersion) null;
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_SendJoinRequest))
          {
            await this.myLoRaCommands.SendJoinRequestAsync(this.progress, this.cancelTokenSource.Token);
            LoRaCommandWindow.result += "\nSend Join request succesfully send to device";
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_CheckJoinRequest))
          {
            LoRaCheckJoinAccept retVal = await this.myLoRaCommands.CheckJoinAcceptAsync(this.progress, this.cancelTokenSource.Token);
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nCheck join request result was succesfully loaded. \nGet: " + Util.ByteArrayToHexString(retVal.basedata);
            if (retVal != null)
            {
              LoRaCommandWindow.result += "\n";
              LoRaCommandWindow.result = LoRaCommandWindow.result + "\n DateTime: " + retVal.Timestamp.ToString();
              LoRaCommandWindow.result = LoRaCommandWindow.result + "\n DeviceAdr.: " + retVal.DeviceAddress.ToString();
              LoRaCommandWindow.result = LoRaCommandWindow.result + "\n NetID: " + Util.ByteArrayToHexString(retVal.NetID);
              LoRaCommandWindow.result += "\n";
            }
            retVal = (LoRaCheckJoinAccept) null;
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_SendUnconfirmedData))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              this.updateContextMenu1(arg1);
              byte[] data = Util.HexStringToByteArray(arg1);
              if (data.Length <= 50)
              {
                await this.myLoRaCommands.SendUnconfirmedDataAsync(data, this.progress, this.cancelTokenSource.Token);
                LoRaCommandWindow.result += "\nUnconfirmed data succesfully sent to device.";
              }
              else
              {
                int num2 = (int) MessageBox.Show("To much bytes to send ... max. 50 Bytes.\nPlease check the lenght of your bytearray.");
              }
              data = (byte[]) null;
            }
            else
            {
              int num3 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_SendConfirmedData))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              this.updateContextMenu1(arg1);
              byte[] data = Util.HexStringToByteArray(arg1);
              if (data.Length <= 50)
              {
                await this.myLoRaCommands.SendConfirmedDataAsync(data, this.progress, this.cancelTokenSource.Token);
                LoRaCommandWindow.result += "\nConfirmed data succesfully sent to device.";
              }
              else
              {
                int num4 = (int) MessageBox.Show("To much bytes to send ... max. 50 Bytes.\nPlease check the lenght of your bytearray.");
              }
              data = (byte[]) null;
            }
            else
            {
              int num5 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_SetNetID))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              this.updateContextMenu1(arg1);
              byte[] data = Util.HexStringToByteArray(arg1);
              await this.myLoRaCommands.SetNetIDAsync(data, this.progress, this.cancelTokenSource.Token);
              LoRaCommandWindow.result += "\nNetID successfully sent to device.";
              data = (byte[]) null;
            }
            else
            {
              int num6 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_GetNetID))
          {
            byte[] retVal = await this.myLoRaCommands.GetNetIDAsync(this.progress, this.cancelTokenSource.Token);
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nNetID: " + Util.ByteArrayToHexString(retVal);
            retVal = (byte[]) null;
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_SetAppEUI))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              byte[] data = Util.HexStringToByteArray(arg1);
              await this.myLoRaCommands.SetAppEUIAsync(data, this.progress, this.cancelTokenSource.Token);
              LoRaCommandWindow.result += "\nData successfully sent to device.";
              data = (byte[]) null;
            }
            else
            {
              int num7 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_GetAppEUI))
          {
            byte[] retVal = await this.myLoRaCommands.GetAppEUIAsync(this.progress, this.cancelTokenSource.Token);
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nAppEUI: " + Util.ByteArrayToHexString(retVal);
            retVal = (byte[]) null;
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_SetNwkSKey))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              this.updateContextMenu1(arg1);
              byte[] data = Util.HexStringToByteArray(arg1);
              await this.myLoRaCommands.SetNwkSKeyAsync(data, this.progress, this.cancelTokenSource.Token);
              LoRaCommandWindow.result += "\nNetwork session key successfully sent to device.";
              data = (byte[]) null;
            }
            else
            {
              int num8 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_GetNwkSKey))
          {
            byte[] retVal = await this.myLoRaCommands.GetNwkSKeyAsync(this.progress, this.cancelTokenSource.Token);
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nNetwork session key: " + Util.ByteArrayToHexString(retVal);
            retVal = (byte[]) null;
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_SetAppSKey))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              this.updateContextMenu1(arg1);
              byte[] data = Util.HexStringToByteArray(arg1);
              await this.myLoRaCommands.SetAppSKeyAsync(data, this.progress, this.cancelTokenSource.Token);
              LoRaCommandWindow.result += "\nApplication session key successfully sent to device.";
              data = (byte[]) null;
            }
            else
            {
              int num9 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_GetAppSKey))
          {
            byte[] retVal = await this.myLoRaCommands.GetAppSKeyAsync(this.progress, this.cancelTokenSource.Token);
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nApplication session key: " + Util.ByteArrayToHexString(retVal);
            retVal = (byte[]) null;
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_SetOTAA_ABP))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              OTAA_ABP flag = (OTAA_ABP) byte.Parse(arg1);
              await this.myLoRaCommands.SetOTAA_ABPAsync(flag, this.progress, this.cancelTokenSource.Token);
              LoRaCommandWindow.result += "\nOTAA-ABP successfully sent to device.";
            }
            else
            {
              int num10 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_GetOTAA_ABP))
          {
            OTAA_ABP retVal = await this.myLoRaCommands.GetOTAA_ABPAsync(this.progress, this.cancelTokenSource.Token);
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nActivation mode:  " + retVal.ToString();
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_SetDeviceEUI))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              byte[] data = Util.HexStringToByteArray(arg1);
              await this.myLoRaCommands.SetDevEUIAsync(data, this.progress, this.cancelTokenSource.Token);
              LoRaCommandWindow.result += "\nDevice EUI successfully sent to device.";
              data = (byte[]) null;
            }
            else
            {
              int num11 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_GetDeviceEUI))
          {
            byte[] retVal = await this.myLoRaCommands.GetDevEUIAsync(this.progress, this.cancelTokenSource.Token);
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nDevice EUI: " + Util.ByteArrayToHexString(retVal);
            retVal = (byte[]) null;
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_SetAppKey))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              byte[] data = Util.HexStringToByteArray(arg1);
              await this.myLoRaCommands.SetAppKeyAsync(data, this.progress, this.cancelTokenSource.Token);
              LoRaCommandWindow.result += "\nApplikation key successfully sent to device.";
              data = (byte[]) null;
            }
            else
            {
              int num12 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_GetAppKey))
          {
            byte[] retVal = await this.myLoRaCommands.GetAppKeyAsync(this.progress, this.cancelTokenSource.Token);
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nApplication Key: " + Util.ByteArrayToHexString(retVal);
            retVal = (byte[]) null;
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_SetDevAddr))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              byte[] data = Util.HexStringToByteArray(arg1);
              await this.myLoRaCommands.SetDevAddrAsync(data, this.progress, this.cancelTokenSource.Token);
              LoRaCommandWindow.result += "\nDevice address successfully sent to device.";
              data = (byte[]) null;
            }
            else
            {
              int num13 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_GetDevAddr))
          {
            byte[] retVal = await this.myLoRaCommands.GetDevAddrAsync(this.progress, this.cancelTokenSource.Token);
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nDevice address: " + Util.ByteArrayToHexString(retVal);
            retVal = (byte[]) null;
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_SetTransmissionScenario))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              byte[] data = Util.HexStringToByteArray(arg1);
              await this.myLoRaCommands.SetTransmissionScenarioAsync(data, this.progress, this.cancelTokenSource.Token);
              LoRaCommandWindow.result += "\nTransmission scenario successfully set.";
              data = (byte[]) null;
            }
            else
            {
              int num14 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_GetTransmissionScenario))
          {
            byte retVal = await this.myLoRaCommands.GetTransmissionScenarioAsync(this.progress, this.cancelTokenSource.Token);
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nTransmission scenario: " + retVal.ToString();
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_SendConfigurationPaket))
          {
            if (!string.IsNullOrEmpty(arg1) && arg1.Length == 2)
            {
              byte[] data = Util.HexStringToByteArray(arg1);
              await this.myLoRaCommands.SendConfigurationPaketAsync(data, this.progress, this.cancelTokenSource.Token);
              LoRaCommandWindow.result += "\nConfiguration packet successfully set.";
              data = (byte[]) null;
            }
            else
            {
              int num15 = (int) MessageBox.Show("Wrong input string format found.\nInput string should be 2 characters!");
            }
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_SetADR))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              byte data = byte.Parse(arg1);
              await this.myLoRaCommands.SetAdrAsync(data, this.progress, this.cancelTokenSource.Token);
              LoRaCommandWindow.result = LoRaCommandWindow.result + "\nADR set to: " + arg1;
            }
            else
            {
              int num16 = (int) MessageBox.Show("Wrong input string format found.\nPlease check your input string.");
            }
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_GetADR))
          {
            byte retVal = await this.myLoRaCommands.GetAdrAsync(this.progress, this.cancelTokenSource.Token);
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nADR: " + retVal.ToString();
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_TriggerSystemDiagnostic))
          {
            LoRaCommandWindow.result = "\nTriggerSystemDiagnostic";
            string selLow = this.ComboAddCommand2.SelectedItem.ToString();
            string selHigh = this.ComboAddCommand3.SelectedItem.ToString();
            selLow = selLow.Substring(selLow.IndexOf('(') + 1);
            selLow = selLow.Remove(selLow.IndexOf(')'));
            selHigh = selHigh.Substring(selHigh.IndexOf('(') + 1);
            selHigh = selHigh.Remove(selHigh.IndexOf(')'));
            ushort usLOW = Convert.ToUInt16(selLow, 16);
            ushort usHIGH = Convert.ToUInt16(selHigh, 16);
            byte diagConfig = (byte) ((uint) usLOW + (uint) usHIGH);
            ushort dailyStartTimeParam = LoRaSystemDiagnostic.getDailyTimeFromString(arg1);
            ushort dailyEndTimeParam = LoRaSystemDiagnostic.getDailyTimeFromString(arg2);
            LoRaSystemDiagnostic lsd = new LoRaSystemDiagnostic();
            lsd.DiagnosticConfig = diagConfig;
            lsd.DailyStartTime = (byte) dailyStartTimeParam;
            lsd.DailyEndTime = (byte) dailyEndTimeParam;
            await this.myLoRaCommands.TriggerSystemDiagnosticAsync(lsd, this.progress, this.cancelTokenSource.Token);
            LoRaCommandWindow.result += "\nTriggerSystemDiagnostic done";
            selLow = (string) null;
            selHigh = (string) null;
            lsd = (LoRaSystemDiagnostic) null;
          }
          if (EFC.Contains(LoRaCommandWindow.CMD_SystemDiagnosticState))
          {
            LoRaSystemDiagnostic lsd = await this.myLoRaCommands.SystemDiagnosticStateAsync(this.progress, this.cancelTokenSource.Token);
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nDiagnostigConfigInterval: " + lsd.getDiagnosticIntervalAsString();
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nDiagnostigConfigRepeat: " + lsd.getDiagnosticRepeatAsString();
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nDailyStartTime: " + lsd.getDailyStartEndTimeAsString() + " h";
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nDailyEndTime: " + lsd.getDailyStartEndTimeAsString(false) + " h";
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nRemainingActivity: " + lsd.RemainigActivity.ToString();
            LoRaCommandWindow.result = LoRaCommandWindow.result + "\nRemainingDiagnostic: " + lsd.RemainigDiagnostic.ToString();
            lsd = (LoRaSystemDiagnostic) null;
          }
        }
        if (string.IsNullOrEmpty(LoRaCommandWindow.result))
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
          this.TextBoxUniversalCommandResult.Text = LoRaCommandWindow.result;
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
        LoRaCommandWindow.result = LoRaCommandWindow.result + "\nERROR: " + ex.Message + "\nFunction (" + EFC + ")";
        this.TextBoxUniversalCommandResult.Text = LoRaCommandWindow.result;
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

    private void CheckBoxEncryption_Checked(object sender, RoutedEventArgs e)
    {
      this.myLoRaCommands.enDeCrypt = true;
      this.TextBoxEncryptionKey.Visibility = Visibility.Visible;
      this.EncryptionKey_Label.Visibility = Visibility.Visible;
    }

    private void CheckBoxEncryption_UnChecked(object sender, RoutedEventArgs e)
    {
      this.myLoRaCommands.enDeCrypt = false;
      this.TextBoxEncryptionKey.Visibility = Visibility.Collapsed;
      this.EncryptionKey_Label.Visibility = Visibility.Collapsed;
    }

    private void TextBoxEncryptionKey_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.myLoRaCommands.AES_Key = this.TextBoxEncryptionKey.Text;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/commandwindowlora.xaml", UriKind.Relative));
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
          this.ComboAddCommand2_Label = (Label) target;
          break;
        case 14:
          this.ComboAddCommand2 = (ComboBox) target;
          this.ComboAddCommand2.SelectionChanged += new SelectionChangedEventHandler(this.ComboAddCommand2_SelectionChanged);
          break;
        case 15:
          this.ComboAddCommand3_Label = (Label) target;
          break;
        case 16:
          this.ComboAddCommand3 = (ComboBox) target;
          this.ComboAddCommand3.SelectionChanged += new SelectionChangedEventHandler(this.ComboAddCommand3_SelectionChanged);
          break;
        case 17:
          this.TextArgument_1_Label = (Label) target;
          break;
        case 18:
          this.TextExtCommandArgument_1 = (TextBox) target;
          break;
        case 19:
          this.TextArgument_2_Label = (Label) target;
          break;
        case 20:
          this.TextExtCommandArgument_2 = (TextBox) target;
          break;
        case 21:
          this.TextArgument_3_Label = (Label) target;
          break;
        case 22:
          this.TextExtCommandArgument_3 = (TextBox) target;
          break;
        case 23:
          this.TextArgument_4_Label = (Label) target;
          break;
        case 24:
          this.TextExtCommandArgument_4 = (TextBox) target;
          break;
        case 25:
          this.TextArgument_5_Label = (Label) target;
          break;
        case 26:
          this.TextExtCommandArgument_5 = (TextBox) target;
          break;
        case 27:
          this.StackPanalButtons2 = (StackPanel) target;
          break;
        case 28:
          this.ButtonRunCommand = (Button) target;
          this.ButtonRunCommand.Click += new RoutedEventHandler(this.ButtonRunCommand_Click);
          break;
        case 29:
          this.ButtonRunCommandPreview = (Button) target;
          this.ButtonRunCommandPreview.Click += new RoutedEventHandler(this.ButtonRunCommandPreview_Click);
          break;
        case 30:
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
