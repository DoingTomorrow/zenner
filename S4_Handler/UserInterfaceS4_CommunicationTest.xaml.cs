// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_CommunicationTest
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using CommunicationPort.Functions;
using HandlerLib;
using HandlerLib.NFC;
using S4_Handler.Functions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.UserInterface
{
  public partial class S4_CommunicationTest : Window, IComponentConnector
  {
    private S4_HandlerFunctions myHandlerFunctions;
    private CancellationTokenSource cancelTokenSource;
    private ProgressHandler progress;
    internal Button ButtonRun;
    internal Button ButtonReadNdcMiConModuleIds;
    internal Button ButtonCheckFirmwareVersions;
    internal TextBox TextBoxResult;
    private bool _contentLoaded;

    public S4_CommunicationTest(S4_HandlerFunctions myHandlerFunctions)
    {
      this.myHandlerFunctions = myHandlerFunctions;
      this.InitializeComponent();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
    }

    private void OnProgress(ProgressArg obj)
    {
      if (this.CheckAccess())
        return;
      this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      await this.RunCommunicationTest((object) this.ButtonRun);
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
      await this.RunCommunicationTest(sender);
    }

    private async Task RunCommunicationTest(object sender)
    {
      Cursor defaultCursor = this.Cursor;
      this.Cursor = Cursors.Wait;
      this.IsEnabled = false;
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress.Reset();
      StringBuilder result = new StringBuilder();
      try
      {
        if (sender == this.ButtonRun)
        {
          this.TextBoxResult.Clear();
          this.TextBoxResult.Background = (Brush) SystemColors.ControlBrush;
          TextBox textBox = this.TextBoxResult;
          string str = await this.myHandlerFunctions.CheckNfcCommunication(this.progress, this.cancelTokenSource.Token);
          textBox.Text = str;
          textBox = (TextBox) null;
          str = (string) null;
        }
        else if (sender == this.ButtonReadNdcMiConModuleIds)
        {
          this.TextBoxResult.Clear();
          this.TextBoxResult.Background = (Brush) SystemColors.ControlBrush;
          NdcMiConModuleHardwareIds ModuleIDs = await this.myHandlerFunctions.GetNdcMiConModuleHardwareIds(this.progress, this.cancelTokenSource.Token);
          result.AppendLine("NFC_MiCon_Connector ID = " + ModuleIDs.MiConConnectorID);
          result.AppendLine("NFC_Coupler ID = " + ModuleIDs.NfcCouplerID);
          this.TextBoxResult.Text = result.ToString();
          ModuleIDs = (NdcMiConModuleHardwareIds) null;
        }
        else if (sender == this.ButtonCheckFirmwareVersions)
        {
          try
          {
            CommunicationPortFunctions thePort = this.myHandlerFunctions.myPort;
            NfcDeviceCommands nfcCommands = this.myHandlerFunctions.checkedNfcCommands;
            result.AppendLine("*** All firmware versions ***");
            result.AppendLine();
            StringBuilder stringBuilder = result;
            string str = await NFC_Versions.GetVersionInfoText(this.myHandlerFunctions.myMeters.checkedConnectedMeter.deviceIdentification.FirmwareVersionObj, thePort, nfcCommands, this.progress, this.cancelTokenSource.Token);
            stringBuilder.AppendLine(str);
            stringBuilder = (StringBuilder) null;
            str = (string) null;
            result.AppendLine();
            await this.myHandlerFunctions.CheckCommunicationFirmwareVersionsForConnectedDevice(this.progress, this.cancelTokenSource.Token);
            result.AppendLine("No critical firmware version detected.");
            thePort = (CommunicationPortFunctions) null;
            nfcCommands = (NfcDeviceCommands) null;
          }
          catch (Exception ex)
          {
            ExceptionViewer.Show(ex);
            result.AppendLine();
            result.AppendLine("*** Exception ***");
            result.AppendLine();
            result.AppendLine(ex.Message);
          }
          this.TextBoxResult.Clear();
          this.TextBoxResult.Text = result.ToString();
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.Cursor = defaultCursor;
      this.IsEnabled = true;
      defaultCursor = (Cursor) null;
      result = (StringBuilder) null;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_communicationtest.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
          break;
        case 2:
          this.ButtonRun = (Button) target;
          this.ButtonRun.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 3:
          this.ButtonReadNdcMiConModuleIds = (Button) target;
          this.ButtonReadNdcMiConModuleIds.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 4:
          this.ButtonCheckFirmwareVersions = (Button) target;
          this.ButtonCheckFirmwareVersions.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 5:
          this.TextBoxResult = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
