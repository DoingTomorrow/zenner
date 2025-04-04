// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_TestDisplay
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib.NFC;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.UserInterface
{
  public partial class S4_TestDisplay : Window, IComponentConnector
  {
    private S4_HandlerWindowFunctions myWindowFunctions;
    private ProgressHandler progress;
    private CancellationTokenSource cancelTokenSource;
    internal GmmCorporateControl gmmCorporateControl1;
    internal ProgressBar ProgressBarLcd;
    internal CheckBox CkBxLoop;
    internal Button ButtonSwitchDisplay;
    internal Button ButtonGetCouplerCurrent;
    internal Label LableRFoff;
    internal Label LableRFon;
    internal Label LableStandby;
    private bool _contentLoaded;

    public S4_TestDisplay(S4_HandlerWindowFunctions myWindowFunctions)
    {
      this.myWindowFunctions = myWindowFunctions;
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.InitializeComponent();
    }

    private void OnProgress(ProgressArg obj)
    {
      if (!this.CheckAccess())
        this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
      else
        this.ProgressBarLcd.Value = obj.ProgressPercentage;
    }

    private void RunState() => this.cancelTokenSource = new CancellationTokenSource();

    private void StopState() => this.cancelTokenSource.Cancel();

    private void ButtonX_Click(object sender, RoutedEventArgs e)
    {
      this.RunState();
      this.Run_Button(sender);
    }

    private async void Run_Button(object sender)
    {
      try
      {
        if (sender == this.ButtonSwitchDisplay)
        {
          int i = 0;
          this.progress.Reset(20);
          bool? isChecked;
          bool flag;
          do
          {
            if (i++ % 10 == 0)
              this.progress.Reset(20);
            await this.myWindowFunctions.MyFunctions.checkedCommands.SwitchLcd(this.progress, this.cancelTokenSource.Token);
            isChecked = this.CkBxLoop.IsChecked;
            flag = true;
          }
          while (isChecked.GetValueOrDefault() == flag & isChecked.HasValue);
        }
        else
        {
          if (sender != this.ButtonGetCouplerCurrent)
            return;
          NfcCouplerCurrents currents = new NfcCouplerCurrents();
          currents = await this.myWindowFunctions.MyFunctions.GetNFC_CouplerCurrentAsync(this.progress, this.cancelTokenSource.Token);
          double current = currents.NfcFieldOffCurrent * 1000.0;
          this.LableRFoff.Content = (object) current.ToString("0.000");
          current = currents.NfcFieldOnCurrent * 1000.0;
          this.LableRFon.Content = (object) current.ToString("0.000");
          current = currents.StandbyCurrent * 1000.0;
          this.LableStandby.Content = (object) current.ToString("0.000");
          currents = (NfcCouplerCurrents) null;
        }
      }
      catch (NfcFrameException ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
      catch (TimeoutException ex)
      {
        int num = (int) MessageBox.Show("Timeout: " + ex?.ToString());
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("*** Exception ***" + Environment.NewLine + ex.Message);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_testdisplay.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.gmmCorporateControl1 = (GmmCorporateControl) target;
          break;
        case 2:
          this.ProgressBarLcd = (ProgressBar) target;
          break;
        case 3:
          this.CkBxLoop = (CheckBox) target;
          break;
        case 4:
          this.ButtonSwitchDisplay = (Button) target;
          this.ButtonSwitchDisplay.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 5:
          this.ButtonGetCouplerCurrent = (Button) target;
          this.ButtonGetCouplerCurrent.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 6:
          this.LableRFoff = (Label) target;
          break;
        case 7:
          this.LableRFon = (Label) target;
          break;
        case 8:
          this.LableStandby = (Label) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
