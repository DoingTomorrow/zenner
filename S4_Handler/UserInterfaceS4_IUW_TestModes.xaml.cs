// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_IUW_TestModes
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using HandlerLib.NFC;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.UserInterface
{
  public partial class S4_IUW_TestModes : Window, IComponentConnector
  {
    private S4_HandlerWindowFunctions myWindowFunctions;
    private ProgressHandler progress;
    private EventHandler<string> iuwEvent;
    private CancellationTokenSource cancelTokenSource;
    internal GmmCorporateControl gmmCorporateControl1;
    internal Button BtnMiConPrepFT;
    internal Button BtnMiConResetFT;
    internal Button Button_TestEnable;
    internal Button Button_TestDisable;
    internal ComboBox ComboBox_TestMode;
    internal ComboBoxItem flyingTest;
    internal ComboBoxItem currentTest;
    internal ComboBoxItem calibZeroFlow;
    internal ComboBoxItem rtcCalibrationOutput;
    internal ComboBoxItem lcdTest;
    internal Button Button_TestStart;
    internal Button Button_TestStop;
    internal Button Button_ReadTestValues;
    internal Button Button_ReadDevice;
    internal TextBox TextBox_Delay;
    internal Label Lable_CountOk;
    internal Label Lable_CountFail;
    internal CheckBox CheckBox_Loop;
    internal ProgressBar ProgressBar;
    internal Label ProgressLbl;
    internal TextBox TxTBox_Values;
    private bool _contentLoaded;

    public S4_IUW_TestModes(S4_HandlerWindowFunctions myWindowFunctions)
    {
      this.myWindowFunctions = myWindowFunctions;
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.iuwEvent = new EventHandler<string>(this.IuwEvent);
      this.InitializeComponent();
    }

    private void IuwEvent(object sender, string str)
    {
      if (!this.CheckAccess())
      {
        this.Dispatcher.Invoke((Action) (() => this.IuwEvent(sender, str)));
      }
      else
      {
        this.TxTBox_Values.Text += str;
        this.TxTBox_Values.ScrollToEnd();
      }
    }

    private void OnProgress(ProgressArg obj)
    {
      if (!this.CheckAccess())
      {
        this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
      }
      else
      {
        this.ProgressBar.Value = obj.ProgressPercentage;
        this.ProgressLbl.Content = (object) (obj.ProgressPercentage.ToString() + "%");
      }
    }

    private void RunState()
    {
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress.Reset();
    }

    private void StopState()
    {
    }

    private async void BtnReadResults_Click(object sender, RoutedEventArgs e)
    {
      this.RunState();
      this.progress.Reset();
      try
      {
        this.TxTBox_Values.Text = "PC-Time_Read:\t" + DateTime.Now.ToString("hh:mm:ss.ffff") + "\r\n";
        TextBox textBox1 = this.TxTBox_Values;
        TextBox textBox2 = textBox1;
        string str = textBox1.Text;
        FlyingTestData flyingTestData = await this.myWindowFunctions.MyFunctions.ReadFlyingTestResultsAsync(this.progress, this.cancelTokenSource.Token);
        textBox2.Text = str + flyingTestData.ToString();
        textBox1 = (TextBox) null;
        textBox2 = (TextBox) null;
        str = (string) null;
        flyingTestData = (FlyingTestData) null;
      }
      catch (TaskCanceledException ex)
      {
      }
      catch (NfcFrameException ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
      this.StopState();
    }

    private void Button_Click(object sender, RoutedEventArgs e) => this.Run_Button(sender);

    private async void Run_Button(object sender)
    {
      this.RunState();
      try
      {
        if (sender == this.Button_TestEnable)
        {
          await this.myWindowFunctions.MyFunctions.nfcCmd.SetModeAsync(S4_DeviceModes.TestModePrepared, this.progress, this.cancelTokenSource.Token);
          this.ComboBox_TestMode.IsEnabled = false;
        }
        else if (sender == this.Button_TestDisable)
        {
          await this.myWindowFunctions.MyFunctions.nfcCmd.SetModeAsync(S4_DeviceModes.OperationMode, this.progress, this.cancelTokenSource.Token);
          this.ComboBox_TestMode.IsEnabled = true;
        }
        else if (sender == this.Button_TestStart)
        {
          this.Button_ReadTestValues.Content = (object) "read Value";
          int selectedIndex = this.ComboBox_TestMode.SelectedIndex;
          switch (selectedIndex)
          {
            case 0:
              await this.myWindowFunctions.MyFunctions.nfcCmd.SetModeAsync(S4_DeviceModes.FlyingTestStart, this.progress, this.cancelTokenSource.Token);
              break;
            case 1:
              await this.myWindowFunctions.MyFunctions.nfcCmd.SetModeAsync(S4_DeviceModes.CurrentTest, this.progress, this.cancelTokenSource.Token);
              break;
            case 2:
              await this.myWindowFunctions.MyFunctions.nfcCmd.SetModeAsync(S4_DeviceModes.ZeroOffsetTestStart, this.progress, this.cancelTokenSource.Token);
              break;
            case 3:
              await this.myWindowFunctions.MyFunctions.nfcCmd.SetModeAsync(S4_DeviceModes.RtcCalibrationTestStart, this.progress, this.cancelTokenSource.Token);
              this.Button_ReadTestValues.Content = (object) "write Value";
              break;
            case 4:
              await this.myWindowFunctions.MyFunctions.SetLcdTestStateAsync(this.progress, this.cancelTokenSource.Token, (byte) 1);
              break;
          }
          this.Button_TestStop.IsEnabled = true;
        }
        else if (sender == this.Button_TestStop)
        {
          int selectedIndex = this.ComboBox_TestMode.SelectedIndex;
          switch (selectedIndex)
          {
            case 0:
              await this.myWindowFunctions.MyFunctions.nfcCmd.SetModeAsync(S4_DeviceModes.FlyingTestOrderStop, this.progress, this.cancelTokenSource.Token);
              break;
            case 2:
              await this.myWindowFunctions.MyFunctions.nfcCmd.SetModeAsync(S4_DeviceModes.ZeroOffsetTestOrderStop, this.progress, this.cancelTokenSource.Token);
              break;
            case 3:
              await this.myWindowFunctions.MyFunctions.nfcCmd.SetModeAsync(S4_DeviceModes.RtcCalibrationOrderStop, this.progress, this.cancelTokenSource.Token);
              break;
            case 4:
              await this.myWindowFunctions.MyFunctions.nfcCmd.SetModeAsync(S4_DeviceModes.OperationMode, this.progress, this.cancelTokenSource.Token);
              break;
          }
        }
        else if (sender == this.Button_ReadTestValues)
        {
          int selectedIndex = this.ComboBox_TestMode.SelectedIndex;
          switch (selectedIndex)
          {
            case 0:
              TextBox textBox = this.TxTBox_Values;
              FlyingTestData flyingTestData = await this.myWindowFunctions.MyFunctions.ReadFlyingTestResultsAsync(this.progress, this.cancelTokenSource.Token);
              textBox.Text = flyingTestData.ToString();
              textBox = (TextBox) null;
              flyingTestData = (FlyingTestData) null;
              break;
            case 1:
              this.TxTBox_Values.Text = "Not available!!";
              break;
            case 2:
              await this.myWindowFunctions.MyFunctions.SetZeroOffsetParameters(new EventHandler<string>(this.IuwEvent), this.progress, this.cancelTokenSource.Token);
              break;
            case 3:
              double value = 0.0;
              double.TryParse(this.TxTBox_Values.Text, out value);
              await this.myWindowFunctions.MyFunctions.Calibrate_RTC(this.progress, this.cancelTokenSource.Token, value);
              break;
          }
        }
        else if (sender == this.Button_ReadDevice)
        {
          uint OkCount = 0;
          uint FailCount = 0;
          ushort delay = 0;
          ushort.TryParse(this.TextBox_Delay.Text, out delay);
          this.Button_ReadDevice.IsEnabled = false;
          this.TxTBox_Values.Text = "";
          bool? isChecked;
          bool flag;
          do
          {
            try
            {
              this.progress.Reset();
              int num = await this.myWindowFunctions.MyFunctions.ReadDeviceAsync(this.progress, this.cancelTokenSource.Token, ReadPartsSelection.AllWithoutLogger);
              ++OkCount;
              this.Lable_CountOk.Content = (object) OkCount.ToString();
            }
            catch (NfcFrameException ex)
            {
              ++FailCount;
              this.Lable_CountFail.Content = (object) FailCount.ToString();
              this.TxTBox_Values.AppendText(DateTime.Now.ToString() + "\r\n" + ex.Message + "\r\n");
            }
            catch (TimeoutException ex)
            {
              ++FailCount;
              this.Lable_CountFail.Content = (object) FailCount.ToString();
              TextBox txTboxValues = this.TxTBox_Values;
              txTboxValues.Text = txTboxValues.Text + DateTime.Now.ToString() + " Timeout \r\n";
            }
            catch (Exception ex)
            {
              ++FailCount;
              this.Lable_CountFail.Content = (object) FailCount.ToString();
              TextBox txTboxValues = this.TxTBox_Values;
              txTboxValues.Text = txTboxValues.Text + DateTime.Now.ToString() + " Exception \r\n";
            }
            await Task.Delay((int) delay);
            isChecked = this.CheckBox_Loop.IsChecked;
            flag = true;
          }
          while (isChecked.GetValueOrDefault() == flag & isChecked.HasValue);
          this.Button_ReadDevice.IsEnabled = true;
        }
        else if (sender == this.BtnMiConPrepFT)
          await this.myWindowFunctions.MyFunctions.PrepareForFlyingTestAsync(this.progress, this.cancelTokenSource.Token);
        else if (sender == this.BtnMiConResetFT)
          await this.myWindowFunctions.MyFunctions.SetModeAsync(this.progress, this.cancelTokenSource.Token, (Enum) HandlerFunctionsForProduction.CommonDeviceModes.OperationMode, (byte) 0, (ushort) 10, (ushort) 0, 305419896U, (ushort) 37331, (byte[]) null);
      }
      catch (OperationCanceledException ex)
      {
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
      this.StopState();
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      this.CheckBox_Loop.IsChecked = new bool?(false);
      if (this.cancelTokenSource == null)
        return;
      this.cancelTokenSource.Cancel();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_iuw_testmodes.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((Window) target).Closing += new CancelEventHandler(this.Window_Closing);
          break;
        case 2:
          this.gmmCorporateControl1 = (GmmCorporateControl) target;
          break;
        case 3:
          this.BtnMiConPrepFT = (Button) target;
          this.BtnMiConPrepFT.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 4:
          this.BtnMiConResetFT = (Button) target;
          this.BtnMiConResetFT.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 5:
          this.Button_TestEnable = (Button) target;
          this.Button_TestEnable.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 6:
          this.Button_TestDisable = (Button) target;
          this.Button_TestDisable.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 7:
          this.ComboBox_TestMode = (ComboBox) target;
          break;
        case 8:
          this.flyingTest = (ComboBoxItem) target;
          break;
        case 9:
          this.currentTest = (ComboBoxItem) target;
          break;
        case 10:
          this.calibZeroFlow = (ComboBoxItem) target;
          break;
        case 11:
          this.rtcCalibrationOutput = (ComboBoxItem) target;
          break;
        case 12:
          this.lcdTest = (ComboBoxItem) target;
          break;
        case 13:
          this.Button_TestStart = (Button) target;
          this.Button_TestStart.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 14:
          this.Button_TestStop = (Button) target;
          this.Button_TestStop.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 15:
          this.Button_ReadTestValues = (Button) target;
          this.Button_ReadTestValues.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 16:
          this.Button_ReadDevice = (Button) target;
          this.Button_ReadDevice.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 17:
          this.TextBox_Delay = (TextBox) target;
          break;
        case 18:
          this.Lable_CountOk = (Label) target;
          break;
        case 19:
          this.Lable_CountFail = (Label) target;
          break;
        case 20:
          this.CheckBox_Loop = (CheckBox) target;
          break;
        case 21:
          this.ProgressBar = (ProgressBar) target;
          break;
        case 22:
          this.ProgressLbl = (Label) target;
          break;
        case 23:
          this.TxTBox_Values = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
