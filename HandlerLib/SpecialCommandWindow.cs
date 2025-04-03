// Decompiled with JetBrains decompiler
// Type: HandlerLib.SpecialCommandWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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
  public class SpecialCommandWindow : Window, IComponentConnector
  {
    private SpecialCommands mySpecialCommands;
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
    private static string result = "";
    private static readonly string CMD_GetVersion = "Get version (0x00)";
    private static readonly string CMD_GetMetrologyParameters = "Get metrology parameters (0x02)";
    private static readonly string CMD_SetMetrologyParameters = "Set metrology parameters (0x02)";
    private static readonly string CMD_GetCurrentMeasuringMode = "Get current measuring mode (0x01)";
    private static readonly string CMD_SetProductFactor = "Set product factor (0x04)";
    private static readonly string CMD_GetProductFactor = "Get product factor (0x04)";
    private static readonly string CMD_SetSummertimeCountingSuppression = "Set summertime counting suppression (0x06)";
    private static readonly string CMD_GetSummertimeCountingSuppression = "Get summertime counting suppression (0x06)";
    private static readonly string CMD_ClearFlowCheckStates = "Clear flow check states (0x09)";
    private static readonly string CMD_GetFlowCheckStates = "Get flow check states (0x09)";
    private static readonly string CMD_SetSDRules = "Set SD Rules (0x0A)";
    private static readonly string CMD_GetSDRules = "Get SD Rules (0x0A)";
    private static readonly string CMD_SendNfc2Device = "Send to NFC device (0x0B)";
    private static readonly string CMD_GetNfcDeviceIdentification = "Get NFC device identification (0x0C)";
    private static readonly string CMD_SetReligiousDay = "Set religious day (0x0D)";
    private static readonly string CMD_SetSmartFunctions = "Set smart functions (0x0E)";
    private static readonly string CMD_GetSmartFunctions = "Get smart functions (0x0E)";
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

    public SpecialCommandWindow(SpecialCommands SpecialCMDs)
    {
      this.InitializeComponent();
      this.mySpecialCommands = SpecialCMDs;
      this.mySpecialCommands.setCryptValuesFromBaseClass();
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
      this.CheckBoxEncryption.IsChecked = new bool?(this.mySpecialCommands.enDeCrypt);
      if (!this.mySpecialCommands.enDeCrypt)
        this.CheckBoxEncryption_UnChecked((object) null, (RoutedEventArgs) null);
      this.TextBoxEncryptionKey.Text = this.mySpecialCommands.AES_Key;
    }

    private void setFunctionCodes()
    {
      this.ComboExtCommand_Label.Visibility = Visibility.Hidden;
      this.ComboExtCommand.Visibility = Visibility.Hidden;
      this.ComboCommand.Items.Clear();
      this.ComboCommand.Items.Add((object) "Special Commands (0x36)");
      this.ComboCommand.SelectedIndex = 0;
    }

    private void setSpecialCommands()
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
      SpecialCommandWindow.result = string.Empty;
      await this.RunCommandFrame();
    }

    private async void ButtonRunCommandPreview_Click(object sender, RoutedEventArgs e)
    {
      Dictionary<int, string> myTemplate = new Dictionary<int, string>();
      SpecialCommandWindow.result = "Actual data from connected device: \n-----------------------------------------------------";
      this.ComboExtCommand.SelectedItem = (object) SpecialCommandWindow.CMD_GetVersion;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) SpecialCommandWindow.CMD_GetFlowCheckStates;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) SpecialCommandWindow.CMD_GetCurrentMeasuringMode;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) SpecialCommandWindow.CMD_GetProductFactor;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) SpecialCommandWindow.CMD_GetSummertimeCountingSuppression;
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) SpecialCommandWindow.CMD_GetSDRules;
      myTemplate.Add(1, "0x01");
      this.SetArgumentFieldsValues(myTemplate);
      myTemplate.Clear();
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) SpecialCommandWindow.CMD_GetSDRules;
      myTemplate.Add(1, "0x02");
      this.SetArgumentFieldsValues(myTemplate);
      myTemplate.Clear();
      await this.RunCommandFrame();
      myTemplate = (Dictionary<int, string>) null;
    }

    private void ComboExtCommand_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.ComboExtCommand.SelectedItem != null)
      {
        object selectedItem = this.ComboExtCommand.SelectedItem;
        this.ComboAddCommand.Items.Clear();
        Dictionary<int, string> template = new Dictionary<int, string>();
        if (selectedItem.ToString().Contains(SpecialCommandWindow.CMD_SetMetrologyParameters))
        {
          template.Add(1, "Identity (2 bytes) :");
          template.Add(2, "Options (10 bytes) :");
        }
        if (selectedItem.ToString().Contains(SpecialCommandWindow.CMD_SetProductFactor))
          template.Add(1, "Productfactor :");
        if (selectedItem.ToString().Contains(SpecialCommandWindow.CMD_SetSummertimeCountingSuppression))
          template.Add(1, "Flag (0x00 off : 0x01 on):");
        if (selectedItem.ToString().Contains(SpecialCommandWindow.CMD_ClearFlowCheckStates))
          template.Add(1, "Flow check state (state):");
        if (selectedItem.ToString().Contains(SpecialCommandWindow.CMD_GetSDRules))
          template.Add(1, "Selection (rule):");
        if (selectedItem.ToString().Contains(SpecialCommandWindow.CMD_SetSDRules))
        {
          template.Add(1, "Selection (rule):");
          template.Add(2, "Flag :");
          template.Add(3, "Options :");
        }
        if (selectedItem.ToString().Contains(SpecialCommandWindow.CMD_SendNfc2Device))
          template.Add(1, "NFC request:");
        if (selectedItem.ToString().Contains(SpecialCommandWindow.CMD_GetNfcDeviceIdentification))
          template.Add(1, "NFC request:");
        if (selectedItem.ToString().Contains(SpecialCommandWindow.CMD_SetReligiousDay))
          template.Add(1, "Data:");
        if (selectedItem.ToString().Contains(SpecialCommandWindow.CMD_SetSmartFunctions))
        {
          template.Add(1, "SmartFunctionCode(0~7):");
          template.Add(2, "Parameter A~F decimal values splite by ';'" + Environment.NewLine + "(Example: 0;1;-1;0;0)");
        }
        if (selectedItem.ToString().Contains(SpecialCommandWindow.CMD_GetSmartFunctions))
          template.Add(1, "SmartFunctionCode(0~7):");
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
      if (this.ComboCommand.SelectedItem.ToString().Contains("0x36"))
      {
        this.setSpecialCommands();
        this.ComboExtCommand_Label.Content = (object) "Special Commands:";
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
      this.mySpecialCommands.enDeCrypt = this.CheckBoxEncryption.IsChecked.Value;
      this.mySpecialCommands.AES_Key = this.TextBoxEncryptionKey.Text.Trim();
      try
      {
        if (FC.Contains("36"))
        {
          if (EFC.Contains(SpecialCommandWindow.CMD_GetVersion))
          {
            ushort version = await this.mySpecialCommands.GetSpecialCommandFCVersionAsync(this.progress, this.cancelTokenSource.Token);
            SpecialCommandWindow.result = SpecialCommandWindow.result + "\nSpecial Cmd FC version is " + version.ToString() + " (0x" + version.ToString("x4") + ")";
          }
          if (EFC.Contains(SpecialCommandWindow.CMD_GetCurrentMeasuringMode))
          {
            byte[] mode = await this.mySpecialCommands.GetCurrentMeasuringModeAsync(this.progress, this.cancelTokenSource.Token);
            if (mode != null && mode.Length != 0)
              SpecialCommandWindow.result = SpecialCommandWindow.result + "\nCurrent measureing mode is " + BitConverter.ToUInt16(mode, 0).ToString() + " (0x" + BitConverter.ToUInt16(mode, 0).ToString("x2") + ")";
            else
              SpecialCommandWindow.result += "\nCurrent measureing mode brought no value back ";
            mode = (byte[]) null;
          }
          if (EFC.Contains(SpecialCommandWindow.CMD_GetMetrologyParameters))
          {
            SpecialCommands.Metrology_Parameters parameter = await this.mySpecialCommands.GetSCMetrologyParametersAsync(this.progress, this.cancelTokenSource.Token);
            if (parameter != null)
            {
              SpecialCommandWindow.result = SpecialCommandWindow.result + "\nIdentity: " + BitConverter.ToUInt16(parameter.Identity, 0).ToString();
              SpecialCommandWindow.result = SpecialCommandWindow.result + "\nOptions: " + Utility.ByteArrayToHexString(parameter.Options);
            }
            else
              SpecialCommandWindow.result += "\nNO value returned by this functions !!!";
            parameter = (SpecialCommands.Metrology_Parameters) null;
          }
          ushort num1;
          if (EFC.Contains(SpecialCommandWindow.CMD_SetMetrologyParameters))
          {
            string str;
            if (!arg1.Contains("0x"))
            {
              str = arg1;
            }
            else
            {
              num1 = ushort.Parse(arg1.Substring(2), NumberStyles.HexNumber);
              str = num1.ToString();
            }
            arg1 = str;
            if (!string.IsNullOrEmpty(arg1))
            {
              arg2 = string.IsNullOrEmpty(arg2) ? "00" : arg2;
              byte[] identity = new byte[2];
              identity = !arg1.Contains("0x") ? Encoding.UTF8.GetBytes(arg1) : Utility.HexStringToByteArray(arg1);
              byte[] options = new byte[10];
              options = Encoding.UTF8.GetBytes(arg2);
              await this.mySpecialCommands.SetSCMetrologyParametersAsync(identity, options, this.progress, this.cancelTokenSource.Token);
              SpecialCommandWindow.result += "\nSetting Metrology Parameters ... Done.";
              identity = (byte[]) null;
              options = (byte[]) null;
            }
            else
              SpecialCommandWindow.result += "\nValue for Identity is not set !!!";
          }
          uint uint32;
          if (EFC.Contains(SpecialCommandWindow.CMD_SetProductFactor))
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
                uint32 = uint.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                str = uint32.ToString();
              }
              arg1 = str;
              ushort Options = ushort.Parse(arg1);
              await this.mySpecialCommands.SetProductFactorAsync(BitConverter.GetBytes(Options), this.progress, this.cancelTokenSource.Token);
              SpecialCommandWindow.result += "\nSetting product factor ... Done.";
            }
            else
            {
              int num2 = (int) MessageBox.Show("Product factor was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(SpecialCommandWindow.CMD_GetProductFactor))
          {
            byte[] mode = await this.mySpecialCommands.GetProductFactorAsync(this.progress, this.cancelTokenSource.Token);
            string[] strArray = new string[6]
            {
              SpecialCommandWindow.result,
              "\nCurrent product factor is ",
              null,
              null,
              null,
              null
            };
            num1 = BitConverter.ToUInt16(mode, 0);
            strArray[2] = num1.ToString();
            strArray[3] = " (0x";
            num1 = BitConverter.ToUInt16(mode, 0);
            strArray[4] = num1.ToString("x2");
            strArray[5] = ")";
            SpecialCommandWindow.result = string.Concat(strArray);
            mode = (byte[]) null;
          }
          if (EFC.Contains(SpecialCommandWindow.CMD_SetSummertimeCountingSuppression))
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
                uint32 = uint.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                str = uint32.ToString();
              }
              arg1 = str;
              ushort Options = ushort.Parse(arg1);
              await this.mySpecialCommands.SetSummertimeCountingSuppressionAsync(BitConverter.GetBytes(Options)[0], this.progress, this.cancelTokenSource.Token);
              SpecialCommandWindow.result += "\nSetting summertimecountingsuppression ... Done.";
            }
            else
            {
              int num3 = (int) MessageBox.Show("Product factor was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(SpecialCommandWindow.CMD_GetSummertimeCountingSuppression))
          {
            byte mode = await this.mySpecialCommands.GetSummertimeCountingSuppressionAsync(this.progress, this.cancelTokenSource.Token);
            SpecialCommandWindow.result = SpecialCommandWindow.result + "\nSummertime counting suppression is " + mode.ToString() + " (0x" + mode.ToString("x2") + ")";
          }
          if (EFC.Contains(SpecialCommandWindow.CMD_ClearFlowCheckStates))
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
                num1 = ushort.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                str = num1.ToString();
              }
              arg1 = str;
              ushort state = ushort.Parse(arg1);
              await this.mySpecialCommands.ClearFlowCheckStatesAsync(state, this.progress, this.cancelTokenSource.Token);
              SpecialCommandWindow.result = SpecialCommandWindow.result + "\nClearing CheckState " + state.ToString() + " - Done.";
            }
            else
            {
              int num4 = (int) MessageBox.Show("Clearing CheckState was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(SpecialCommandWindow.CMD_GetFlowCheckStates))
          {
            byte[] bytes = await this.mySpecialCommands.GetFlowCheckStateAsync(this.progress, this.cancelTokenSource.Token);
            EDC_Warning state = (EDC_Warning) BitConverter.ToUInt16(bytes, 0);
            string stateString = string.Join<Enum>(Environment.NewLine, state.GetIndividualFlags());
            string result = SpecialCommandWindow.result;
            num1 = (ushort) state;
            string str = num1.ToString("X4");
            SpecialCommandWindow.result = result + "\n0x" + str;
            SpecialCommandWindow.result = SpecialCommandWindow.result + "\n" + (string.IsNullOrEmpty(stateString.Trim()) ? "No bit set" : stateString);
            bytes = (byte[]) null;
            stateString = (string) null;
          }
          if (EFC.Contains(SpecialCommandWindow.CMD_SetSDRules))
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
                uint32 = uint.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                str1 = uint32.ToString();
              }
              arg1 = str1;
              string str2;
              if (!arg2.Contains("0x"))
              {
                str2 = arg2;
              }
              else
              {
                uint32 = uint.Parse(arg2.Substring(2), NumberStyles.HexNumber);
                str2 = uint32.ToString();
              }
              arg2 = str2;
              string str3;
              if (!arg3.Contains("0x"))
              {
                str3 = arg3;
              }
              else
              {
                uint32 = uint.Parse(arg3.Substring(2), NumberStyles.HexNumber);
                str3 = uint32.ToString();
              }
              arg3 = str3;
              uint Options = uint.Parse(arg3);
              await this.mySpecialCommands.SetSDRulesAsync(byte.Parse(arg1), byte.Parse(arg2), BitConverter.GetBytes(Options), this.progress, this.cancelTokenSource.Token);
              SpecialCommandWindow.result += "\nSetting rules ... Done.";
            }
            else
            {
              int num5 = (int) MessageBox.Show("Configuration Rules was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(SpecialCommandWindow.CMD_GetSDRules))
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
                uint32 = uint.Parse(arg1.Substring(2), NumberStyles.HexNumber);
                str = uint32.ToString();
              }
              arg1 = str;
              SpecialCommands.SD_Rules_Options rules = await this.mySpecialCommands.GetSDRulesAsync(byte.Parse(arg1), this.progress, this.cancelTokenSource.Token);
              string[] strArray = new string[14]
              {
                SpecialCommandWindow.result,
                "\nConfiguration rules are: \nSelection : ",
                rules.Selection.ToString(),
                " (0x",
                rules.Selection.ToString("x2"),
                ")\nFlag : ",
                rules.Flag.ToString(),
                " (0x",
                rules.Flag.ToString("x2"),
                ")\nOptions : ",
                null,
                null,
                null,
                null
              };
              uint32 = BitConverter.ToUInt32(rules.options, 0);
              strArray[10] = uint32.ToString();
              strArray[11] = " (0x";
              strArray[12] = Util.ByteArrayToHexString(rules.options);
              strArray[13] = ")";
              SpecialCommandWindow.result = string.Concat(strArray);
              rules = (SpecialCommands.SD_Rules_Options) null;
            }
            else
            {
              int num6 = (int) MessageBox.Show("Configuration Rules was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(SpecialCommandWindow.CMD_SendNfc2Device))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              byte[] NFCrequest = Util.HexStringToByteArray(arg1);
              byte[] retVal = await this.mySpecialCommands.SendToNfcDeviceAsync(NFCrequest, this.progress, this.cancelTokenSource.Token);
              SpecialCommandWindow.result += Util.ByteArrayToHexString(retVal);
              NFCrequest = (byte[]) null;
              retVal = (byte[]) null;
            }
            else
            {
              int num7 = (int) MessageBox.Show("NFC command not set.");
            }
          }
          if (EFC.Contains(SpecialCommandWindow.CMD_GetNfcDeviceIdentification))
          {
            byte[] NFCrequest = new byte[6];
            if (!string.IsNullOrEmpty(arg1))
              NFCrequest = Util.HexStringToByteArray(arg1);
            SpecialCommands.NFC_Identification devIdent = await this.mySpecialCommands.GetNfcDeviceIdentification(NFCrequest, this.progress, this.cancelTokenSource.Token);
            SpecialCommandWindow.result = SpecialCommandWindow.result + "\nNFC_Identification: \nIdentification response Selection : (0x" + devIdent.IdentificationResponseVersion.ToString("x2") + ")\nNFC protocol version : (0x" + devIdent.NFCProtocolVersion.ToString("x2") + ")\nMBus Medium : (0x" + devIdent.MBusMedium.ToString("x2") + ")\nOBIS Medium : (0x" + devIdent.OBISMedium.ToString("x2") + ")\nManufacturer: (0x" + Util.ByteArrayToHexString(devIdent.Manufacturer) + ")\nGeneration : (0x" + devIdent.Generation.ToString("x2") + ")\nSerial number : (0x" + Util.ByteArrayToHexString(devIdent.Serialnumber) + ")\nHardwareIdentification : (0x" + Util.ByteArrayToHexString(devIdent.HardwareIdentification) + ")\nSystemState : (0x" + Util.ByteArrayToHexString(devIdent.SystemState) + ")\nSystemInfos : (0x" + Util.ByteArrayToHexString(devIdent.SystemInfos) + ")\nFirmwareVersion : (0x" + Util.ByteArrayToHexString(devIdent.FirmwareVersion) + ")\nMeterID : " + devIdent.MeterID.ToString() + "\nBuildRevision : " + devIdent.BuildRevision.ToString() + "\nBuildTime : " + devIdent.BuildTime.ToString() + "\nCompilerVersion : " + devIdent.CompilerVersion.ToString() + "\nFirmwareSignature : " + devIdent.FirmwareSignature.ToString() + "\nNumberOfAvailableParameterGroups : (0x" + devIdent.NumberOfAvailableParameterGroups.ToString("x2") + ")\nNumberOfAvailableParameters : " + devIdent.NumberOfAvailableParameters.ToString() + "\nNumberOfSelectedParameterGroups : (0x" + devIdent.NumberOfSelectedParameterGroups.ToString("x2") + ")\nNumberOfSelectedParameters : " + devIdent.NumberOfSelectedParameters.ToString() + "\nMaximum record length : (0x" + devIdent.MaximumRecordLength.ToString("x2") + ")";
            NFCrequest = (byte[]) null;
            devIdent = (SpecialCommands.NFC_Identification) null;
          }
          if (EFC.Contains(SpecialCommandWindow.CMD_SetReligiousDay))
          {
            byte[] Data = new byte[0];
            if (!string.IsNullOrEmpty(arg1))
              Data = Util.HexStringToByteArray(arg1);
            if (Data.Length == 0 || Data.Length > 2)
            {
              int num8 = (int) MessageBox.Show("Wrong Argument");
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
            if (Data[0] != (byte) 0 || Data[0] != (byte) 1)
            {
              int num9 = (int) MessageBox.Show("Wrong Argument");
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
            await this.mySpecialCommands.SetReligiousDay(Data, this.progress, this.cancelTokenSource.Token);
            SpecialCommandWindow.result += "\nSuccess";
            Data = (byte[]) null;
          }
          if (EFC.Contains(SpecialCommandWindow.CMD_SetSmartFunctions))
          {
            if (string.IsNullOrEmpty(arg1))
            {
              int num10 = (int) MessageBox.Show("SmartFunctionCode is null");
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
            byte Code = byte.Parse(arg1);
            byte[] CmdData = new byte[13];
            CmdData[0] = Code;
            string[] Splits = arg2.Split(new char[1]{ ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < 6; ++i)
            {
              byte[] data = new byte[2];
              if (i < Splits.Length)
                data = !Splits[i].Contains("-") ? BitConverter.GetBytes(ushort.Parse(Splits[i])) : BitConverter.GetBytes(short.Parse(Splits[i]));
              Array.Copy((Array) data, 0, (Array) CmdData, i * 2 + 1, 2);
              data = (byte[]) null;
            }
            await this.mySpecialCommands.SetSmartFunctions(CmdData, this.progress, this.cancelTokenSource.Token);
            SpecialCommandWindow.result += "\nSet smart functions done.";
            CmdData = (byte[]) null;
            Splits = (string[]) null;
          }
          if (EFC.Contains(SpecialCommandWindow.CMD_GetSmartFunctions))
          {
            if (string.IsNullOrEmpty(arg1))
            {
              int num11 = (int) MessageBox.Show("SmartFunctionCode is null");
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
            byte Code = byte.Parse(arg1);
            byte[] ReturnData = await this.mySpecialCommands.GetSmartFunctions(Code, this.progress, this.cancelTokenSource.Token);
            if (Code == (byte) 0)
            {
              SpecialCommandWindow.result = SpecialCommandWindow.result + "\nSmart function codes implemented in the device:" + Environment.NewLine;
              for (int i = 1; i < ReturnData.Length; ++i)
                SpecialCommandWindow.result = SpecialCommandWindow.result + ReturnData[i].ToString("X2") + " ";
            }
            else
            {
              if (ReturnData.Length != 13)
              {
                int num12 = (int) MessageBox.Show("Return data wrong");
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
              SpecialCommandWindow.result = SpecialCommandWindow.result + "\nSmart function code:" + ReturnData[0].ToString("X2") + Environment.NewLine;
              string[] NameList = new string[6]
              {
                "A",
                "B",
                "C",
                "D",
                "E",
                "F"
              };
              for (int i = 0; i < 6; ++i)
              {
                if (i > 0)
                  SpecialCommandWindow.result += Environment.NewLine;
                SpecialCommandWindow.result = SpecialCommandWindow.result + "Parameter " + NameList[i] + ":" + ReturnData[2 * i + 2].ToString("X2") + " " + ReturnData[2 * i + 1].ToString("X2");
              }
              NameList = (string[]) null;
            }
            ReturnData = (byte[]) null;
          }
        }
        if (string.IsNullOrEmpty(SpecialCommandWindow.result))
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
          this.TextBoxUniversalCommandResult.Text = SpecialCommandWindow.result;
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
        SpecialCommandWindow.result = SpecialCommandWindow.result + "\nFunction (" + EFC + ") \nERROR: " + ex.Message;
        this.TextBoxUniversalCommandResult.Text = SpecialCommandWindow.result;
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
      this.mySpecialCommands.enDeCrypt = true;
      this.TextBoxEncryptionKey.Visibility = Visibility.Visible;
      this.EncryptionKey_Label.Visibility = Visibility.Visible;
    }

    private void CheckBoxEncryption_UnChecked(object sender, RoutedEventArgs e)
    {
      this.mySpecialCommands.enDeCrypt = false;
      this.TextBoxEncryptionKey.Visibility = Visibility.Collapsed;
      this.EncryptionKey_Label.Visibility = Visibility.Collapsed;
    }

    private void TextBoxEncryptionKey_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.mySpecialCommands.AES_Key = this.TextBoxEncryptionKey.Text;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/commandwindowspecial.xaml", UriKind.Relative));
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
