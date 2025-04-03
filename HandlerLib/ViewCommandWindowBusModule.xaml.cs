// Decompiled with JetBrains decompiler
// Type: HandlerLib.View.CommandWindowBusModule
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommonWPF;
using System;
using System.CodeDom.Compiler;
using System.Collections;
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
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib.View
{
  public partial class CommandWindowBusModule : Window, IComponentConnector
  {
    private List<string> ModuleCommands;
    private NfcDeviceCommands NFC_Commands;
    private BusModuleInfo ModuleInfo;
    private CancellationTokenSource cancelTokenSource;
    private ProgressHandler progress;
    internal DockPanel DockPanelButtons;
    internal StackPanel StackPanelBottomButtoms;
    internal Button ButtomRunCommand;
    internal Button ButtomBreak;
    internal StackPanel StackPanelTopButtoms;
    internal ComboBox ComboBoxCommands;
    internal Label LabelAdditionalCommands;
    internal TextBox TextBoxAdditionalCommandBytes;
    internal TextBox TextBoxResults;
    private bool _contentLoaded;

    public CommandWindowBusModule(NfcDeviceCommands NFC_Commands, BusModuleInfo moduleInfo)
    {
      this.NFC_Commands = NFC_Commands;
      this.ModuleInfo = moduleInfo;
      this.InitializeComponent();
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.Title = this.Title + "  " + this.ModuleInfo.BusModuleType.ToString() + ": " + this.ModuleInfo.BusModuleSerialNumber.ToString();
      string[] names = Enum.GetNames(typeof (BusModuleCommand));
      BusModuleCommand[] values = (BusModuleCommand[]) Enum.GetValues(typeof (BusModuleCommand));
      this.ModuleCommands = new List<string>();
      for (int index = 0; index < names.Length; ++index)
        this.ModuleCommands.Add(names[index].Replace("BUS_ASYNC_", "") + "_0x" + ((int) values[index]).ToString("x02"));
      this.ModuleCommands.Sort();
      this.ComboBoxCommands.ItemsSource = (IEnumerable) this.ModuleCommands;
    }

    private void OnProgress(ProgressArg obj)
    {
      if (this.CheckAccess())
        return;
      this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
    }

    private void ComboBoxCommands_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.TextBoxAdditionalCommandBytes.Focus();
    }

    private void ComboBoxCommands_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      e.Handled = true;
      this.TextBoxAdditionalCommandBytes.Focus();
    }

    private async void TextBoxAdditionalCommandBytes_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      e.Handled = true;
      await this.RunCommand();
    }

    private async void ButtomRunCommand_Click(object sender, RoutedEventArgs e)
    {
      await this.RunCommand();
    }

    private async Task RunCommand()
    {
      try
      {
        string commandText = this.ComboBoxCommands.Text;
        int hexTokenindex = commandText.IndexOf("_0x");
        byte commandCode = hexTokenindex < 0 ? byte.Parse(commandText, NumberStyles.HexNumber) : byte.Parse(commandText.Substring(hexTokenindex + 3), NumberStyles.HexNumber);
        string additionalBytesString = this.TextBoxAdditionalCommandBytes.Text.Trim();
        string additionalBytesFormated = "";
        byte[] additionalBytes = (byte[]) null;
        if (additionalBytesString.Length > 0)
        {
          additionalBytes = Util.HexStringToByteArray(additionalBytesString);
          additionalBytesFormated = " " + Util.ByteArrayToHexString(additionalBytes);
        }
        this.TextBoxResults.AppendText("Command code: 0x" + commandCode.ToString("x02") + additionalBytesFormated + Environment.NewLine);
        byte[] transparentToModule;
        if (additionalBytes == null)
        {
          transparentToModule = new byte[1];
        }
        else
        {
          transparentToModule = new byte[additionalBytes.Length + 1];
          additionalBytes.CopyTo((Array) transparentToModule, 1);
        }
        transparentToModule[0] = commandCode;
        byte[] result = await this.NFC_Commands.SendTransparentToModuleAsync(this.progress, this.cancelTokenSource.Token, this.ModuleInfo, BusModuleCommand.BUS_ASYNC_TRANSPARENT_TO_MODULE, transparentToModule);
        string resultString = Util.ByteArrayToHexString(result);
        this.TextBoxResults.AppendText("Result: " + resultString + Environment.NewLine);
        commandText = (string) null;
        additionalBytesString = (string) null;
        additionalBytesFormated = (string) null;
        additionalBytes = (byte[]) null;
        transparentToModule = (byte[]) null;
        result = (byte[]) null;
        resultString = (string) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/commandwindowbusmodule.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.DockPanelButtons = (DockPanel) target;
          break;
        case 2:
          this.StackPanelBottomButtoms = (StackPanel) target;
          break;
        case 3:
          this.ButtomRunCommand = (Button) target;
          this.ButtomRunCommand.Click += new RoutedEventHandler(this.ButtomRunCommand_Click);
          break;
        case 4:
          this.ButtomBreak = (Button) target;
          break;
        case 5:
          this.StackPanelTopButtoms = (StackPanel) target;
          break;
        case 6:
          this.ComboBoxCommands = (ComboBox) target;
          this.ComboBoxCommands.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxCommands_SelectionChanged);
          this.ComboBoxCommands.PreviewKeyDown += new KeyEventHandler(this.ComboBoxCommands_PreviewKeyDown);
          break;
        case 7:
          this.LabelAdditionalCommands = (Label) target;
          break;
        case 8:
          this.TextBoxAdditionalCommandBytes = (TextBox) target;
          this.TextBoxAdditionalCommandBytes.PreviewKeyDown += new KeyEventHandler(this.TextBoxAdditionalCommandBytes_PreviewKeyDown);
          break;
        case 9:
          this.TextBoxResults = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
