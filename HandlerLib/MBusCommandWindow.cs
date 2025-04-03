// Decompiled with JetBrains decompiler
// Type: HandlerLib.MBusCommandWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using GmmDbLib;
using MBusLib;
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
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class MBusCommandWindow : Window, IComponentConnector
  {
    private const string translaterBaseKey = "MBusCommandWindow";
    private CommonMBusCommands myMBusCommands;
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
    private static readonly string CMD_GetChannelMBusAddress = "Get channel MBus address (0x01)";
    private static readonly string CMD_SetChannelMBusAddress = "Set channel MBus address (0x01)";
    private static readonly string CMD_GetChannelIdentification = "Get channel identification (0x02)";
    private static readonly string CMD_SetChannelIdentification = "Set channel identification (0x02)";
    private static readonly string CMD_GetChannelOBISCode = "Get channel OBIS code (0x03)";
    private static readonly string CMD_SetChannelOBISCode = "Set channel OBIS code (0x03)";
    private static readonly string CMD_GetChannelConfiguration = "Get channel configuration (0x04)";
    private static readonly string CMD_SetChannelConfiguration = "Set channel configuration (0x04)";
    private static readonly string CMD_GetChannelValue = "Get channel value (0x05)";
    private static readonly string CMD_SetChannelValue = "Set channel value (0x05)";
    private static readonly string CMD_ReadChannelLOG = "Read channel log value (0x10)";
    private static readonly string CMD_ReadEventLOG = "Read event log (0x11)";
    private static readonly string CMD_ClearEventLOG = "Clear event log (0x12)";
    private static readonly string CMD_ReadSystemLOG = "Read system log (0x13)";
    private static readonly string CMD_ClearSystemLOG = "Clear system log (0x14)";
    private static readonly string CMD_ReadChannelSingleLogValue = "Read channel single log value (0x15)";
    private static readonly string CMD_GetRadioList = "Get radio list (0x16)";
    private static readonly string CMD_SetRadioList = "Set radio list (0x16)";
    private static readonly string CMD_GetTXTimings = "Get TX timings (0x17)";
    private static readonly string CMD_SetTXTimings = "Set TX timings (0x17)";
    private static readonly string CMD_GetMBusKey = "Get MBusKey (0x18)";
    private static readonly string CMD_SetMBusKey = "Set MBusKey (0x18)";
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

    public MBusCommandWindow(CommonMBusCommands MBusCMDs)
    {
      this.InitializeComponent();
      WpfTranslatorSupport.TranslateWindow(Tg.MBus_CommandWindow, (Window) this);
      this.myMBusCommands = MBusCMDs;
      this.myMBusCommands.setCryptValuesFromBaseClass();
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
      this.setCryptState();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
    }

    private void setCryptState()
    {
      this.CheckBoxEncryption.IsChecked = new bool?(this.myMBusCommands.enDeCrypt);
      if (!this.myMBusCommands.enDeCrypt)
        this.CheckBoxEncryption_UnChecked((object) null, (RoutedEventArgs) null);
      this.TextBoxEncryptionKey.Text = this.myMBusCommands.AES_Key;
    }

    private void setFunctionCodes()
    {
      this.ComboExtCommand_Label.Visibility = Visibility.Hidden;
      this.ComboExtCommand.Visibility = Visibility.Hidden;
      this.ComboCommand.Items.Clear();
      this.ComboCommand.Items.Add((object) "MBus Commands (0x34)");
      this.ComboCommand.SelectedIndex = 0;
    }

    private void setMBusCommands()
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
      MBusCommandWindow.result = string.Empty;
      await this.RunCommandFrame();
    }

    private async void ButtonRunCommandPreview_Click(object sender, RoutedEventArgs e)
    {
      MBusCommandWindow.result = "Actual MBus info data from connected device: \n-----------------------------------------------------";
      Dictionary<int, string> args = new Dictionary<int, string>();
      this.ComboExtCommand.SelectedItem = (object) MBusCommandWindow.CMD_GetChannelMBusAddress;
      args.Clear();
      args.Add(1, "0");
      this.SetArgumentFieldsValues(args);
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) MBusCommandWindow.CMD_GetChannelMBusAddress;
      args.Clear();
      args.Add(1, "1");
      this.SetArgumentFieldsValues(args);
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) MBusCommandWindow.CMD_GetChannelConfiguration;
      args.Clear();
      args.Add(1, "0");
      this.SetArgumentFieldsValues(args);
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) MBusCommandWindow.CMD_GetChannelConfiguration;
      args.Clear();
      args.Add(1, "1");
      this.SetArgumentFieldsValues(args);
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) MBusCommandWindow.CMD_GetChannelIdentification;
      args.Clear();
      args.Add(1, "0");
      this.SetArgumentFieldsValues(args);
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) MBusCommandWindow.CMD_GetChannelIdentification;
      args.Clear();
      args.Add(1, "1");
      this.SetArgumentFieldsValues(args);
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) MBusCommandWindow.CMD_GetChannelOBISCode;
      args.Clear();
      args.Add(1, "0");
      this.SetArgumentFieldsValues(args);
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) MBusCommandWindow.CMD_GetChannelOBISCode;
      args.Clear();
      args.Add(1, "1");
      this.SetArgumentFieldsValues(args);
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) MBusCommandWindow.CMD_GetChannelValue;
      args.Clear();
      args.Add(1, "0");
      this.SetArgumentFieldsValues(args);
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) MBusCommandWindow.CMD_GetChannelValue;
      args.Clear();
      args.Add(1, "1");
      this.SetArgumentFieldsValues(args);
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) MBusCommandWindow.CMD_GetRadioList;
      args.Clear();
      this.SetArgumentFieldsValues(args);
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) MBusCommandWindow.CMD_GetTXTimings;
      args.Clear();
      this.SetArgumentFieldsValues(args);
      await this.RunCommandFrame();
      this.ComboExtCommand.SelectedItem = (object) MBusCommandWindow.CMD_GetMBusKey;
      args.Clear();
      this.SetArgumentFieldsValues(args);
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
        if (selectedItem.ToString().Contains(MBusCommandWindow.CMD_SetChannelMBusAddress))
        {
          template.Add(1, "Channel:");
          template.Add(2, "Address:");
        }
        if (selectedItem.ToString().Contains(MBusCommandWindow.CMD_GetChannelMBusAddress))
          template.Add(1, "Channel:");
        if (selectedItem.ToString().Contains(MBusCommandWindow.CMD_SetChannelIdentification))
        {
          template.Add(1, "Channel:");
          template.Add(2, "Serialnumber:");
          template.Add(3, "Manufacturer:");
          template.Add(4, "Generation:");
          template.Add(5, "Medium:");
        }
        if (selectedItem.ToString().Contains(MBusCommandWindow.CMD_GetChannelIdentification))
          template.Add(1, "Channel:");
        if (selectedItem.ToString().Contains(MBusCommandWindow.CMD_SetChannelOBISCode))
        {
          template.Add(1, "Channel:");
          template.Add(2, "OBIS code:");
        }
        if (selectedItem.ToString().Contains(MBusCommandWindow.CMD_GetChannelOBISCode))
          template.Add(1, "Channel:");
        if (selectedItem.ToString().Contains(MBusCommandWindow.CMD_SetChannelConfiguration))
        {
          template.Add(1, "Channel:");
          template.Add(2, "Masntissa(2 bytes):");
          template.Add(3, "Exponent:");
          template.Add(4, "VIF:");
        }
        if (selectedItem.ToString().Contains(MBusCommandWindow.CMD_GetChannelConfiguration))
          template.Add(1, "Channel:");
        if (selectedItem.ToString().Contains(MBusCommandWindow.CMD_SetChannelValue))
        {
          template.Add(1, "Channel:");
          template.Add(2, "Value:");
        }
        if (selectedItem.ToString().Contains(MBusCommandWindow.CMD_GetChannelValue))
          template.Add(1, "Channel:");
        if (selectedItem.ToString().Contains(MBusCommandWindow.CMD_ReadChannelLOG))
        {
          template.Add(1, "Channel:");
          template.Add(2, "Log select:");
          template.Add(3, "Start index:");
          template.Add(4, "End index:");
        }
        if (selectedItem.ToString().Contains(MBusCommandWindow.CMD_ReadEventLOG))
          template.Add(1, "Flow control:");
        if (selectedItem.ToString().Contains(MBusCommandWindow.CMD_ReadSystemLOG))
          template.Add(1, "Flow control:");
        if (selectedItem.ToString().Contains(MBusCommandWindow.CMD_ReadChannelSingleLogValue))
        {
          template.Add(1, "Channel:");
          template.Add(2, "Date:");
        }
        if (selectedItem.ToString().Contains(MBusCommandWindow.CMD_SetRadioList))
          template.Add(1, "Radio list identifier:");
        if (selectedItem.ToString().Contains(MBusCommandWindow.CMD_SetTXTimings))
        {
          template.Add(1, "Interval (sec.):");
          template.Add(2, "Nighttime start(hour):");
          template.Add(3, "Nighttime end(hour):");
          template.Add(4, "Radio suppression days:");
        }
        if (selectedItem.ToString().Contains(MBusCommandWindow.CMD_SetMBusKey))
          template.Add(1, "MBusKey:");
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
      if (this.ComboCommand.SelectedItem.ToString().Contains("0x34"))
      {
        this.setMBusCommands();
        this.ComboExtCommand_Label.Content = (object) "MBus Commands (EFC):";
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
        if (FC.Contains("0x34"))
        {
          if (EFC.Contains(MBusCommandWindow.CMD_SetChannelMBusAddress))
          {
            if (!string.IsNullOrEmpty(arg1) && !string.IsNullOrEmpty(arg2))
            {
              await this.myMBusCommands.SetMBusChannelAddressAsync(byte.Parse(arg1), byte.Parse(arg2), this.progress, this.cancelTokenSource.Token);
              MBusCommandWindow.result = MBusCommandWindow.result + "\nChannel " + arg1 + " MBus address set to " + arg2;
            }
            else
            {
              int num1 = (int) MessageBox.Show("Channel or address was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(MBusCommandWindow.CMD_GetChannelMBusAddress))
          {
            if (!string.IsNullOrEmpty(arg1))
            {
              MBusChannelAddress ChannelAddress = await this.myMBusCommands.GetMBusChannelAddressAsync(byte.Parse(arg1), this.progress, this.cancelTokenSource.Token);
              MBusCommandWindow.result = MBusCommandWindow.result + "\nChannel " + ChannelAddress.Channel.ToString() + " has address " + ChannelAddress.Address.ToString();
              ChannelAddress = (MBusChannelAddress) null;
            }
            else
            {
              int num2 = (int) MessageBox.Show("Channel was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(MBusCommandWindow.CMD_SetChannelIdentification))
          {
            if (!string.IsNullOrEmpty(arg1) && !string.IsNullOrEmpty(arg2) && !string.IsNullOrEmpty(arg3) && !string.IsNullOrEmpty(arg4) && !string.IsNullOrEmpty(arg5))
            {
              MBusChannelIdentification mci = new MBusChannelIdentification()
              {
                Channel = byte.Parse(arg1),
                SerialNumber = (long) Util.ConvertUnt32ToBcdUInt32(uint.Parse(arg2)),
                Manufacturer = MBusUtil.GetManufacturer(ushort.Parse(arg3)),
                Generation = byte.Parse(arg4),
                Medium = MBusUtil.GetMedium(byte.Parse(arg5))
              };
              mci.Medium = string.IsNullOrEmpty(mci.Medium) ? MBusUtil.GetMedium((byte) 15) : mci.Medium;
              await this.myMBusCommands.SetChannelIdentificationAsync(mci, this.progress, this.cancelTokenSource.Token);
              MBusCommandWindow.result += "\nChannel identification was successfully set to device";
              mci = (MBusChannelIdentification) null;
            }
            else
            {
              int num3 = (int) MessageBox.Show("Channel, serialnumber, manufacturer, generation or medium was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(MBusCommandWindow.CMD_GetChannelIdentification) && !string.IsNullOrEmpty(arg1))
          {
            MBusChannelIdentification retVal = await this.myMBusCommands.GetChannelIdentificationAsync(byte.Parse(arg1), this.progress, this.cancelTokenSource.Token);
            MBusCommandWindow.result = MBusCommandWindow.result + "\nChannel: " + retVal.Channel.ToString() + "\nSerialnumber: " + retVal.SerialNumber.ToString() + "\nManufacturer: " + retVal.Manufacturer + "\nGeneration: " + retVal.Generation.ToString() + "\nMedium: " + retVal.Medium;
            retVal = (MBusChannelIdentification) null;
          }
          if (EFC.Contains(MBusCommandWindow.CMD_SetChannelOBISCode) && !string.IsNullOrEmpty(arg1) && !string.IsNullOrEmpty(arg2))
          {
            await this.myMBusCommands.SetChannelOBISCodeAsync(byte.Parse(arg1), byte.Parse(arg2), this.progress, this.cancelTokenSource.Token);
            MBusCommandWindow.result = MBusCommandWindow.result + "\nChannel " + arg1 + " set OBIS code to: " + arg2;
          }
          if (EFC.Contains(MBusCommandWindow.CMD_GetChannelOBISCode) && !string.IsNullOrEmpty(arg1))
          {
            MBusChannelOBIS retVal = await this.myMBusCommands.GetChannelOBISCodeAsync(byte.Parse(arg1), this.progress, this.cancelTokenSource.Token);
            MBusCommandWindow.result = MBusCommandWindow.result + "\nChannel: " + retVal.Channel.ToString() + " OBIS_Code: " + retVal.OBIS_code.ToString();
            retVal = (MBusChannelOBIS) null;
          }
          if (EFC.Contains(MBusCommandWindow.CMD_SetChannelConfiguration))
          {
            if (!string.IsNullOrEmpty(arg1) && !string.IsNullOrEmpty(arg2) && !string.IsNullOrEmpty(arg3) && !string.IsNullOrEmpty(arg4))
            {
              MBusChannelConfiguration mbChannelConfig = new MBusChannelConfiguration();
              mbChannelConfig.Channel = byte.Parse(arg1);
              mbChannelConfig.Mantissa = BitConverter.GetBytes(ushort.Parse(arg2));
              mbChannelConfig.Exponent = sbyte.Parse(arg3);
              mbChannelConfig.VIF = byte.Parse(arg4);
              await this.myMBusCommands.SetChannelConfigurationAsync(mbChannelConfig, this.progress, this.cancelTokenSource.Token);
              MBusCommandWindow.result += "\nChannel configuration was successfully set to device";
              mbChannelConfig = (MBusChannelConfiguration) null;
            }
            else
            {
              int num4 = (int) MessageBox.Show("Channel, mantissa, exponent or vif was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(MBusCommandWindow.CMD_GetChannelConfiguration) && !string.IsNullOrEmpty(arg1))
          {
            MBusChannelConfiguration retVal = await this.myMBusCommands.GetChannelConfigurationAsync(byte.Parse(arg1), this.progress, this.cancelTokenSource.Token);
            ushort Mantissa = BitConverter.ToUInt16(retVal.Mantissa, 0);
            MBusCommandWindow.result = MBusCommandWindow.result + "\nChannel: " + retVal.Channel.ToString() + "\nMantissa: " + Mantissa.ToString() + "\nExponent: " + retVal.Exponent.ToString() + "\nVIF: " + retVal.VIF.ToString();
            retVal = (MBusChannelConfiguration) null;
          }
          if (EFC.Contains(MBusCommandWindow.CMD_SetChannelValue))
          {
            if (!string.IsNullOrEmpty(arg1) && !string.IsNullOrEmpty(arg2))
            {
              await this.myMBusCommands.SetChannelValueAsync(byte.Parse(arg1), uint.Parse(arg2), this.progress, this.cancelTokenSource.Token);
              MBusCommandWindow.result += "\nChannel value was successfully set to device";
            }
            else
            {
              int num5 = (int) MessageBox.Show("Channel or value was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(MBusCommandWindow.CMD_GetChannelValue) && !string.IsNullOrEmpty(arg1))
          {
            uint retVal = await this.myMBusCommands.GetChannelValueAsync(byte.Parse(arg1), this.progress, this.cancelTokenSource.Token);
            MBusCommandWindow.result = MBusCommandWindow.result + "\nChannel: " + arg1 + "\nValue: " + retVal.ToString();
          }
          byte num6;
          if (EFC.Contains(MBusCommandWindow.CMD_ReadChannelLOG))
          {
            if (!string.IsNullOrEmpty(arg1) && !string.IsNullOrEmpty(arg2) && !string.IsNullOrEmpty(arg3) && !string.IsNullOrEmpty(arg4))
            {
              int channel = int.Parse(arg1);
              if (channel < 0 || channel > 4)
              {
                MBusCommandWindow.result += "\nChannel is not supported!";
              }
              else
              {
                MBusChannelLog retVal = await this.myMBusCommands.ReadChannelLogValueAsync(byte.Parse(arg1), byte.Parse(arg2), byte.Parse(arg3), byte.Parse(arg4), this.progress, this.cancelTokenSource.Token);
                if (retVal != null)
                {
                  MBusCommandWindow.result += "\nChannel log values read successfully from device.\n";
                  if (retVal.Year != byte.MaxValue)
                  {
                    string[] strArray = new string[13];
                    strArray[0] = MBusCommandWindow.result;
                    strArray[1] = "\nStartTime: 20";
                    strArray[2] = retVal.Year.ToString("D2");
                    strArray[3] = "-";
                    strArray[4] = retVal.Month.ToString("D2");
                    strArray[5] = "-";
                    num6 = retVal.Day;
                    strArray[6] = num6.ToString("D2");
                    strArray[7] = " ";
                    num6 = retVal.Hour;
                    strArray[8] = num6.ToString("D2");
                    strArray[9] = ":";
                    num6 = retVal.Minute;
                    strArray[10] = num6.ToString("D2");
                    strArray[11] = ":";
                    num6 = retVal.Second;
                    strArray[12] = num6.ToString("D2");
                    MBusCommandWindow.result = string.Concat(strArray);
                  }
                  else
                    MBusCommandWindow.result += "\nStartTime: N/A";
                  string[] strArray1 = new string[5]
                  {
                    MBusCommandWindow.result,
                    "\nChannel: (",
                    null,
                    null,
                    null
                  };
                  num6 = retVal.Channel;
                  strArray1[2] = num6.ToString();
                  strArray1[3] = ") - ";
                  strArray1[4] = (int) retVal.Channel == (int) byte.Parse(arg1) ? " OK." : "FALSE !!!";
                  MBusCommandWindow.result = string.Concat(strArray1);
                  string result = MBusCommandWindow.result;
                  num6 = retVal.LogSelected;
                  string str = num6.ToString();
                  MBusCommandWindow.result = result + "\nLog selected: " + str;
                  MBusCommandWindow.result += "\n------------------------";
                  int index = 0;
                  for (int i = 0; i < retVal.LogValues.Length / 4; ++i)
                  {
                    byte[] data = new byte[4];
                    Buffer.BlockCopy((Array) retVal.LogValues, index, (Array) data, 0, data.Length);
                    index += 4;
                    uint logVal = BitConverter.ToUInt32(data, 0);
                    MBusCommandWindow.result = MBusCommandWindow.result + "\nLogValues(" + (i + 1).ToString() + "): " + (logVal != uint.MaxValue ? logVal.ToString() : "N/A");
                    data = (byte[]) null;
                  }
                }
                else
                  MBusCommandWindow.result += "\nCould not read channel log from device!!!\n";
                retVal = (MBusChannelLog) null;
              }
            }
            else
            {
              int num7 = (int) MessageBox.Show("Channel, logSelect, StartIndex or EndIndex was not set correctly!\nPlease set a correct value and try again.");
            }
          }
          if (EFC.Contains(MBusCommandWindow.CMD_ReadEventLOG) && !string.IsNullOrEmpty(arg1))
          {
            MBusEventLog retVal = await this.myMBusCommands.ReadEventLogAsync(byte.Parse(arg1), this.progress, this.cancelTokenSource.Token);
            if (retVal.FlowControl == (byte) 1)
            {
              string[] strArray = new string[17];
              strArray[0] = MBusCommandWindow.result;
              strArray[1] = "\nEvent log read successfully. \nSystemEventType: ";
              num6 = retVal.SystemEventType;
              strArray[2] = num6.ToString();
              strArray[3] = "\nEventTime: 20";
              strArray[4] = retVal.EventTime[0].ToString("D2");
              strArray[5] = "-";
              strArray[6] = retVal.EventTime[1].ToString("D2");
              strArray[7] = "-";
              strArray[8] = retVal.EventTime[2].ToString("D2");
              strArray[9] = " ";
              strArray[10] = retVal.EventTime[3].ToString("D2");
              strArray[11] = ":";
              strArray[12] = retVal.EventTime[4].ToString("D2");
              strArray[13] = "\nChannel0Value: ";
              strArray[14] = Util.ByteArrayToHexString(retVal.Channel0Value);
              strArray[15] = "\nChannel1Value: ";
              strArray[16] = Util.ByteArrayToHexString(retVal.Channel1Value);
              MBusCommandWindow.result = string.Concat(strArray);
            }
            else if (retVal.FlowControl == (byte) 0)
              MBusCommandWindow.result += "\nNo additional data";
            retVal = (MBusEventLog) null;
          }
          if (EFC.Contains(MBusCommandWindow.CMD_ClearEventLOG))
          {
            await this.myMBusCommands.ClearEventLogAsync(this.progress, this.cancelTokenSource.Token);
            MBusCommandWindow.result += "\nEvent log successfully cleared.";
          }
          if (EFC.Contains(MBusCommandWindow.CMD_ReadSystemLOG) && !string.IsNullOrEmpty(arg1))
          {
            MBusSystemLog retVal = await this.myMBusCommands.ReadSystemLogAsync(byte.Parse(arg1), this.progress, this.cancelTokenSource.Token);
            if (retVal.FlowControl == (byte) 1)
            {
              string[] strArray = new string[17];
              strArray[0] = MBusCommandWindow.result;
              strArray[1] = "\nSystem log read successfully. \nSystemEventType: ";
              num6 = retVal.SystemEventType;
              strArray[2] = num6.ToString();
              strArray[3] = "\nEventTime: 20";
              strArray[4] = retVal.EventTime[0].ToString("D2");
              strArray[5] = "-";
              strArray[6] = retVal.EventTime[1].ToString("D2");
              strArray[7] = "-";
              strArray[8] = retVal.EventTime[2].ToString("D2");
              strArray[9] = " ";
              strArray[10] = retVal.EventTime[3].ToString("D2");
              strArray[11] = ":";
              strArray[12] = retVal.EventTime[4].ToString("D2");
              strArray[13] = "\nChannel0Value: ";
              strArray[14] = Util.ByteArrayToHexString(retVal.Channel0Value);
              strArray[15] = "\nChannel1Value: ";
              strArray[16] = Util.ByteArrayToHexString(retVal.Channel1Value);
              MBusCommandWindow.result = string.Concat(strArray);
            }
            else if (retVal.FlowControl == (byte) 0)
              MBusCommandWindow.result += "\nNo additional data";
            retVal = (MBusSystemLog) null;
          }
          if (EFC.Contains(MBusCommandWindow.CMD_ClearSystemLOG))
          {
            await this.myMBusCommands.ClearSystemLogAsync(this.progress, this.cancelTokenSource.Token);
            MBusCommandWindow.result += "\nSystem log successfully cleared.";
          }
          if (EFC.Contains(MBusCommandWindow.CMD_ReadChannelSingleLogValue) && !string.IsNullOrEmpty(arg1) && !string.IsNullOrEmpty(arg2))
          {
            byte channel = byte.Parse(arg1);
            byte[] date = BitConverter.GetBytes(ushort.Parse(arg2));
            MBusChannelSingleLogValue retVal = await this.myMBusCommands.ReadChannelSingleLogValueAsync(channel, date, this.progress, this.cancelTokenSource.Token);
            if ((int) retVal.Channel == (int) channel)
            {
              string[] strArray = new string[7];
              strArray[0] = MBusCommandWindow.result;
              strArray[1] = "\nChannel log value read successfully. \nChannel: ";
              num6 = retVal.Channel;
              strArray[2] = num6.ToString();
              strArray[3] = "\nDate:";
              strArray[4] = retVal.DateAndTime.ToString();
              strArray[5] = "\nValue: ";
              strArray[6] = retVal.Value.ToString();
              MBusCommandWindow.result = string.Concat(strArray);
            }
            else
              MBusCommandWindow.result += "\nNo informational data";
            date = (byte[]) null;
            retVal = (MBusChannelSingleLogValue) null;
          }
          if (EFC.Contains(MBusCommandWindow.CMD_GetRadioList))
          {
            byte[] retVal = await this.myMBusCommands.GetRadioListAsync(this.progress, this.cancelTokenSource.Token);
            if (retVal != null && retVal.Length != 0)
            {
              if (retVal.Length > 1)
                throw new Exception("Not specified in documentation!");
              MBusCommandWindow.result = MBusCommandWindow.result + "\nRadio list identifier read successfully. \nIdentifier: " + retVal[0].ToString();
            }
            else
              MBusCommandWindow.result += "\nNo data received.";
            retVal = (byte[]) null;
          }
          if (EFC.Contains(MBusCommandWindow.CMD_SetRadioList) && !string.IsNullOrEmpty(arg1))
          {
            await this.myMBusCommands.SetRadioListAsync(byte.Parse(arg1), this.progress, this.cancelTokenSource.Token);
            MBusCommandWindow.result += "\nRadio list set.";
          }
          if (EFC.Contains(MBusCommandWindow.CMD_SetTXTimings) && !string.IsNullOrEmpty(arg1) && !string.IsNullOrEmpty(arg2) && !string.IsNullOrEmpty(arg3) && !string.IsNullOrEmpty(arg4))
          {
            MBusTXTimings values = new MBusTXTimings();
            values.Interval = ushort.Parse(arg1);
            values.NightTimeStart = byte.Parse(arg2);
            values.NightTimeEnd = byte.Parse(arg3);
            values.RadioSuppressionDays = byte.Parse(arg4);
            await this.myMBusCommands.SetTXTimingsAsync(values, this.progress, this.cancelTokenSource.Token);
            MBusCommandWindow.result += "\nTX timings successfully set.";
            values = (MBusTXTimings) null;
          }
          if (EFC.Contains(MBusCommandWindow.CMD_GetTXTimings))
          {
            MBusTXTimings retVal = await this.myMBusCommands.GetTXTimingsAsync(this.progress, this.cancelTokenSource.Token);
            if (retVal != null)
            {
              MBusCommandWindow.result = MBusCommandWindow.result + "\nInterval: " + retVal.Interval.ToString();
              string result1 = MBusCommandWindow.result;
              num6 = retVal.NightTimeStart;
              string str1 = num6.ToString();
              MBusCommandWindow.result = result1 + "\nNighttime Start: " + str1;
              string result2 = MBusCommandWindow.result;
              num6 = retVal.NightTimeEnd;
              string str2 = num6.ToString();
              MBusCommandWindow.result = result2 + "\nNighttime End: " + str2;
              string result3 = MBusCommandWindow.result;
              num6 = retVal.RadioSuppressionDays;
              string str3 = num6.ToString();
              MBusCommandWindow.result = result3 + "\nRadio suppression days: " + str3;
              if (retVal.Reserved != uint.MaxValue)
                MBusCommandWindow.result = MBusCommandWindow.result + "\nReserved: " + Util.ByteArrayToHexString(BitConverter.GetBytes(retVal.Reserved));
            }
            else
              MBusCommandWindow.result += "Timing settings not available ...";
            retVal = (MBusTXTimings) null;
          }
          if (EFC.Contains(MBusCommandWindow.CMD_SetMBusKey) && !string.IsNullOrEmpty(arg1))
          {
            byte[] values = Utility.HexStringToByteArray(arg1);
            await this.myMBusCommands.SetMBusKeyAsync(values, this.progress, this.cancelTokenSource.Token);
            MBusCommandWindow.result += "\nMBusKey successfully set.";
            values = (byte[]) null;
          }
          if (EFC.Contains(MBusCommandWindow.CMD_GetMBusKey))
          {
            byte[] retVal = await this.myMBusCommands.GetMBusKeyAsync(this.progress, this.cancelTokenSource.Token);
            MBusCommandWindow.result = retVal == null ? MBusCommandWindow.result + "MBusKey not available ..." : MBusCommandWindow.result + "\nMBusKey: " + Utility.ByteArrayToHexString(retVal);
            retVal = (byte[]) null;
          }
        }
        if (string.IsNullOrEmpty(MBusCommandWindow.result))
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
          this.TextBoxUniversalCommandResult.Text = MBusCommandWindow.result;
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
        MBusCommandWindow.result = MBusCommandWindow.result + "\nFunction (" + EFC + ") - FAILED !!!\n --> ERROR: " + ex.Message;
        this.TextBoxUniversalCommandResult.Text = MBusCommandWindow.result;
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
        this.Argument5ValuesMenu.Items.Add((object) newItem);
      }
    }

    private void CheckBoxEncryption_Checked(object sender, RoutedEventArgs e)
    {
      this.myMBusCommands.enDeCrypt = true;
      this.TextBoxEncryptionKey.Visibility = Visibility.Visible;
      this.EncryptionKey_Label.Visibility = Visibility.Visible;
    }

    private void CheckBoxEncryption_UnChecked(object sender, RoutedEventArgs e)
    {
      this.myMBusCommands.enDeCrypt = false;
      this.TextBoxEncryptionKey.Visibility = Visibility.Collapsed;
      this.EncryptionKey_Label.Visibility = Visibility.Collapsed;
    }

    private void TextBoxEncryptionKey_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.myMBusCommands.AES_Key = this.TextBoxEncryptionKey.Text;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/commandwindowmbus.xaml", UriKind.Relative));
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
