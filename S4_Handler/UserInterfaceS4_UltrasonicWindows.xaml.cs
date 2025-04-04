// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_UltrasonicWindows
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using NLog;
using S4_Handler.Functions;
using StartupLib;
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
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.UserInterface
{
  public partial class S4_UltrasonicWindows : Window, IComponentConnector
  {
    private static Logger S4_HandlerTestWindowsLogger = LogManager.GetLogger("S4_HandlerTestWindows");
    private S4_HandlerWindowFunctions myWindowFunctions;
    private S4_HandlerFunctions myFunctions;
    private S4_DeviceCommandsNFC myCommands;
    private ProgressHandler progress;
    private Cursor defaultCursor;
    private CancellationTokenSource cancelTokenSource;
    private S4_TDC_Internals EmcTdcInt;
    internal ProgressBar ProgressBar1;
    internal Button ButtonBreak;
    internal Button ButtonGetUltrasonicState;
    internal Button ButtonUltrasonicTest;
    internal Button ButtonReadAndShowTemperature;
    internal Button ButtonShowCalibrationsFromBackup;
    internal Button ButtonShowCalibrationsFromType;
    internal Button ButtonManualCalibration;
    internal Button ButtonChangeDirection;
    internal TextBox TextBoxCalcCycles;
    internal Button ButtonEmcTestRefCycle;
    internal Button ButtonBeforeEmcTest;
    internal Button ButtonAfterEmcTest;
    internal Button ButtonStartStopEmcTest;
    internal GroupBox GroupBoxZeroOffset;
    internal TextBox TextBoxZeroOffsetTestSeconds;
    internal Button ButtonCalibrateZeroOffset;
    internal TextBox TextBoxRepeads;
    internal Button ButtonCheckZeroOffset;
    internal TextBlock TextBlockStatus;
    internal GroupBox GroupBoxResults;
    internal TextBox TextBoxCommandResult;
    private bool _contentLoaded;

    public S4_UltrasonicWindows(S4_HandlerWindowFunctions myWindowFunctions)
    {
      this.myWindowFunctions = myWindowFunctions;
      this.myFunctions = myWindowFunctions.MyFunctions;
      this.myCommands = this.myFunctions.myDeviceCommands;
      this.InitializeComponent();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.SetStopState();
    }

    private void SetRunState()
    {
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress.Reset();
      this.progress.Split(new double[2]{ 2.0, 98.0 });
      this.progress.Report("Run");
      this.ButtonBreak.IsEnabled = true;
      if (this.Cursor == Cursors.Wait)
        return;
      this.defaultCursor = this.Cursor;
      this.Cursor = Cursors.Wait;
    }

    private void SetStopState()
    {
      this.ButtonShowCalibrationsFromType.IsEnabled = false;
      this.ButtonShowCalibrationsFromBackup.IsEnabled = false;
      if (this.myFunctions.myMeters.WorkMeter != null)
      {
        if (this.myFunctions.myMeters.TypeMeter != null)
          this.ButtonShowCalibrationsFromType.IsEnabled = true;
        if (this.myFunctions.myMeters.BackupMeter != null)
          this.ButtonShowCalibrationsFromBackup.IsEnabled = true;
      }
      this.ButtonBreak.IsEnabled = false;
      this.Cursor = this.defaultCursor;
      this.progress.Reset();
      this.TextBlockStatus.Text = "";
      if (!UserManager.CheckPermission(UserManager.Right_ReadOnly))
        return;
      this.ButtonBeforeEmcTest.IsEnabled = false;
      this.ButtonAfterEmcTest.IsEnabled = false;
      this.ButtonCalibrateZeroOffset.IsEnabled = false;
      this.ButtonEmcTestRefCycle.IsEnabled = false;
      this.ButtonStartStopEmcTest.IsEnabled = false;
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
        if (obj.Tag != null && obj.Tag.GetType() == typeof (string))
        {
          string tag = (string) obj.Tag;
          if (this.TextBoxCommandResult.Text.Length == 0)
            this.TextBoxCommandResult.AppendText(tag);
          else
            this.TextBoxCommandResult.AppendText(Environment.NewLine + tag);
          if (this.TextBoxCommandResult.Text.Length > 10000)
          {
            int start = this.TextBoxCommandResult.Text.IndexOf(Environment.NewLine);
            int num = this.TextBoxCommandResult.Text.IndexOf(Environment.NewLine, 1000);
            if (start > 0 && num > 0)
            {
              int length = num - start;
              this.TextBoxCommandResult.Select(start, length);
              this.TextBoxCommandResult.SelectedText = "";
            }
          }
          this.TextBoxCommandResult.ScrollToEnd();
        }
        else
          this.TextBlockStatus.Text = obj.Message;
      }
    }

    private void ButtonBreak_Click(object sender, RoutedEventArgs e)
    {
      this.cancelTokenSource.Cancel();
    }

    private async void ButtonX_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      try
      {
        if (sender == this.ButtonCheckZeroOffset)
        {
          if (!int.TryParse(this.TextBoxZeroOffsetTestSeconds.Text, out int _))
          {
            int num1 = (int) MessageBox.Show("Illegal test time");
          }
          else
          {
            int checkRepeats;
            if (!int.TryParse(this.TextBoxRepeads.Text, out checkRepeats) || checkRepeats < 1)
            {
              int num2 = (int) MessageBox.Show("Illegal repeat counter");
            }
            else
            {
              StringBuilder infoText = new StringBuilder();
              string reseltHeader = "Repeat; Calib1; Check1;   Err1; Calib2; Check2;   Err2";
              S4_UltrasonicWindows.S4_HandlerTestWindowsLogger.Info("Zero offset check");
              S4_UltrasonicWindows.S4_HandlerTestWindowsLogger.Info(reseltHeader);
              this.TextBoxCommandResult.Text = "***** Zero offset check ***** " + Environment.NewLine + reseltHeader + Environment.NewLine;
              for (int i = 1; i <= checkRepeats && !this.cancelTokenSource.Token.IsCancellationRequested; ++i)
              {
                ZeroFlowCheckResults results = await this.myFunctions.UltrasonicZeroOffset(int.Parse(this.TextBoxZeroOffsetTestSeconds.Text), false, this.progress, this.cancelTokenSource.Token);
                string resultData = i.ToString("d06") + ";" + results.TransducerPair1CalibrationOffset.ToString("d6") + "; " + results.TransducerPair1Offset.ToString("d6") + "; " + results.TransducerPair1OffsetCalibrationError.ToString("d6") + "; " + results.TransducerPair2CalibrationOffset.ToString("d6") + "; " + results.TransducerPair2Offset.ToString("d6") + "; " + results.TransducerPair2OffsetCalibrationError.ToString("d5");
                this.TextBoxCommandResult.AppendText(resultData + Environment.NewLine);
                this.TextBoxCommandResult.ScrollToEnd();
                S4_UltrasonicWindows.S4_HandlerTestWindowsLogger.Info(resultData);
                results = (ZeroFlowCheckResults) null;
                resultData = (string) null;
              }
              infoText = (StringBuilder) null;
              reseltHeader = (string) null;
            }
          }
        }
        else if (sender == this.ButtonCalibrateZeroOffset)
        {
          this.TextBoxCommandResult.Text = "Zero offset calibration ...";
          ZeroFlowCheckResults results = await this.myFunctions.UltrasonicZeroOffset(int.Parse(this.TextBoxZeroOffsetTestSeconds.Text), true, this.progress, this.cancelTokenSource.Token);
          this.TextBoxCommandResult.Text = "***** Zero offset check result ***** " + Environment.NewLine + Environment.NewLine + "Transducer pair 1 offset = " + results.TransducerPair1Offset.ToString() + Environment.NewLine + Environment.NewLine + "Transducer pair 2 offset = " + results.TransducerPair2Offset.ToString() + Environment.NewLine;
          results = (ZeroFlowCheckResults) null;
        }
        else if (sender == this.ButtonGetUltrasonicState)
        {
          UltrasonicState results = await this.myFunctions.ReadUltrasonicState(this.progress, this.cancelTokenSource.Token);
          this.TextBoxCommandResult.Text = results.ToString();
          results = (UltrasonicState) null;
        }
        else if (sender == this.ButtonShowCalibrationsFromBackup)
        {
          S4_TDC_Calibration calibration = new S4_TDC_Calibration();
          calibration.LoadCalibrationFromMemory(this.myFunctions.myMeters.WorkMeter.meterMemory);
          this.TextBoxCommandResult.Text = calibration.GetCalibrationChanges(this.myFunctions.myMeters.BackupMeter.meterMemory, "Compare backup to work");
          calibration = (S4_TDC_Calibration) null;
        }
        else if (sender == this.ButtonShowCalibrationsFromType)
        {
          S4_TDC_Calibration calibration = new S4_TDC_Calibration();
          calibration.LoadCalibrationFromMemory(this.myFunctions.myMeters.WorkMeter.meterMemory);
          this.TextBoxCommandResult.Text = calibration.GetCalibrationChanges(this.myFunctions.myMeters.TypeMeter.meterMemory, "Compare type to work");
          calibration = (S4_TDC_Calibration) null;
        }
        else if (sender == this.ButtonChangeDirection)
        {
          S4_TDC_Internals tdcInternals = new S4_TDC_Internals(this.myFunctions.myMeters.ConnectedMeter, this.myFunctions.checkedCommands);
          tdcInternals.TDC_ChangeDirection(this.progress, this.cancelTokenSource.Token);
          tdcInternals = (S4_TDC_Internals) null;
        }
        else if (sender == this.ButtonUltrasonicTest)
        {
          this.TextBoxCommandResult.Text = "Ultrasonic test";
          this.progress.SplitByReportLoggerTimesString("1;122;124;124;128;123;1016;1012;1001;1000;1016;103;1;1108;1;1118;1;1118;1;1119;1;118;125;122;124");
          UltrasonicTestResults results = await this.myFunctions.RunUltrasonicTest(this.progress, this.cancelTokenSource.Token);
          this.TextBoxCommandResult.Text = results.ToString();
          string timeSplitString = this.progress.GetReportLoggerTimesString();
          results = (UltrasonicTestResults) null;
          timeSplitString = (string) null;
        }
        else if (sender == this.ButtonReadAndShowTemperature)
        {
          double results = await this.myFunctions.ReadAndGetTemperature(this.progress, this.cancelTokenSource.Token);
          this.TextBoxCommandResult.Text = "Current temperature = " + results.ToString() + " °C";
        }
        else if (sender == this.ButtonManualCalibration)
        {
          S4_ManualFlowCalibration calibrationWindow = new S4_ManualFlowCalibration(this.myFunctions);
          calibrationWindow.Owner = (Window) this;
          calibrationWindow.ShowDialog();
          calibrationWindow = (S4_ManualFlowCalibration) null;
        }
        else
          this.TextBoxCommandResult.Text = "Not supported button";
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (TimeoutException ex)
      {
        this.TextBoxCommandResult.Text = "*** Timeout ***" + Environment.NewLine + ex.Message;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.TextBoxCommandResult.Text = "*** Exception ***" + Environment.NewLine + ex.Message;
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void ButtonUltasonicTestEvent_Click(object sender, RoutedEventArgs e)
    {
      Exception exitException = (Exception) null;
      if (this.EmcTdcInt == null || !this.EmcTdcInt.EmcTestActive)
      {
        this.SetRunState();
        this.GroupBoxZeroOffset.IsEnabled = false;
        this.ButtonGetUltrasonicState.IsEnabled = false;
        this.ButtonUltrasonicTest.IsEnabled = false;
        if (this.EmcTdcInt == null)
        {
          this.WindowState = WindowState.Maximized;
          this.TextBoxCommandResult.Clear();
          this.TextBoxCalcCycles.IsEnabled = false;
          this.ButtonBeforeEmcTest.IsEnabled = false;
          this.ButtonAfterEmcTest.IsEnabled = false;
        }
      }
      try
      {
        if (sender == this.ButtonEmcTestRefCycle)
        {
          if (this.EmcTdcInt == null || this.EmcTdcInt.EmcRefFinished)
          {
            this.ButtonEmcTestRefCycle.Content = (object) "Stopp EMC reference cycle";
            int calcCycles = int.Parse(this.TextBoxCalcCycles.Text);
            this.EmcTdcInt = new S4_TDC_Internals(this.myFunctions.myMeters.WorkMeter, this.myFunctions.checkedCommands);
            await this.EmcTdcInt.PrepareEmcTestAsync(calcCycles, this.progress, this.cancelTokenSource.Token);
            if (this.EmcTdcInt.RefCycles < 5U)
            {
              this.EmcTdcInt = (S4_TDC_Internals) null;
            }
            else
            {
              this.ButtonStartStopEmcTest.IsEnabled = true;
              this.ButtonBeforeEmcTest.IsEnabled = true;
            }
          }
          else
          {
            this.EmcTdcInt.EmcRefFinished = true;
            this.ButtonEmcTestRefCycle.Content = (object) "Start EMC reference cycle";
          }
        }
        else if (sender == this.ButtonStartStopEmcTest)
        {
          if (!this.EmcTdcInt.EmcTestActive)
          {
            this.ButtonStartStopEmcTest.Content = (object) "Stop continues EMC test";
            this.ButtonBeforeEmcTest.IsEnabled = false;
            this.ButtonAfterEmcTest.IsEnabled = false;
            await this.EmcTdcInt.RunEmcTestAsync(this.progress, this.cancelTokenSource.Token);
            this.ButtonStartStopEmcTest.Content = (object) "Start continues EMC test";
          }
          else
          {
            this.EmcTdcInt.EmcTestFinished = true;
            this.ButtonBeforeEmcTest.IsEnabled = true;
            this.ButtonAfterEmcTest.IsEnabled = false;
          }
        }
        else if (sender == this.ButtonBeforeEmcTest)
        {
          await this.EmcTdcInt.BeforeEmcTestAsync(this.progress, this.cancelTokenSource.Token);
          this.ButtonBeforeEmcTest.IsEnabled = false;
          this.ButtonAfterEmcTest.IsEnabled = true;
        }
        else if (sender == this.ButtonAfterEmcTest)
        {
          await this.EmcTdcInt.AfterEmcTestAsync(this.progress, this.cancelTokenSource.Token);
          this.ButtonBeforeEmcTest.IsEnabled = true;
          this.ButtonAfterEmcTest.IsEnabled = false;
        }
      }
      catch (TaskCanceledException ex)
      {
        this.progress.Report("Canceled");
        this.EmcTdcInt = (S4_TDC_Internals) null;
      }
      catch (Exception ex)
      {
        this.EmcTdcInt = (S4_TDC_Internals) null;
        exitException = ex;
      }
      if (this.EmcTdcInt == null || !this.EmcTdcInt.EmcTestActive)
      {
        this.GroupBoxZeroOffset.IsEnabled = true;
        this.ButtonGetUltrasonicState.IsEnabled = true;
        this.ButtonUltrasonicTest.IsEnabled = true;
        if (this.EmcTdcInt == null)
        {
          this.TextBoxCalcCycles.IsEnabled = true;
          this.ButtonStartStopEmcTest.Content = (object) "Start EMC test";
          this.ButtonStartStopEmcTest.IsEnabled = false;
          this.ButtonBeforeEmcTest.IsEnabled = false;
          this.ButtonAfterEmcTest.IsEnabled = false;
        }
        this.SetStopState();
      }
      exitException = exitException == null ? (Exception) null : throw new Exception("Exception on EMC test", exitException);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_ultrasonicwindows.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ProgressBar1 = (ProgressBar) target;
          break;
        case 2:
          this.ButtonBreak = (Button) target;
          this.ButtonBreak.Click += new RoutedEventHandler(this.ButtonBreak_Click);
          break;
        case 3:
          this.ButtonGetUltrasonicState = (Button) target;
          this.ButtonGetUltrasonicState.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 4:
          this.ButtonUltrasonicTest = (Button) target;
          this.ButtonUltrasonicTest.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 5:
          this.ButtonReadAndShowTemperature = (Button) target;
          this.ButtonReadAndShowTemperature.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 6:
          this.ButtonShowCalibrationsFromBackup = (Button) target;
          this.ButtonShowCalibrationsFromBackup.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 7:
          this.ButtonShowCalibrationsFromType = (Button) target;
          this.ButtonShowCalibrationsFromType.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 8:
          this.ButtonManualCalibration = (Button) target;
          this.ButtonManualCalibration.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 9:
          this.ButtonChangeDirection = (Button) target;
          this.ButtonChangeDirection.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 10:
          this.TextBoxCalcCycles = (TextBox) target;
          break;
        case 11:
          this.ButtonEmcTestRefCycle = (Button) target;
          this.ButtonEmcTestRefCycle.Click += new RoutedEventHandler(this.ButtonUltasonicTestEvent_Click);
          break;
        case 12:
          this.ButtonBeforeEmcTest = (Button) target;
          this.ButtonBeforeEmcTest.Click += new RoutedEventHandler(this.ButtonUltasonicTestEvent_Click);
          break;
        case 13:
          this.ButtonAfterEmcTest = (Button) target;
          this.ButtonAfterEmcTest.Click += new RoutedEventHandler(this.ButtonUltasonicTestEvent_Click);
          break;
        case 14:
          this.ButtonStartStopEmcTest = (Button) target;
          this.ButtonStartStopEmcTest.Click += new RoutedEventHandler(this.ButtonUltasonicTestEvent_Click);
          break;
        case 15:
          this.GroupBoxZeroOffset = (GroupBox) target;
          break;
        case 16:
          this.TextBoxZeroOffsetTestSeconds = (TextBox) target;
          break;
        case 17:
          this.ButtonCalibrateZeroOffset = (Button) target;
          this.ButtonCalibrateZeroOffset.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 18:
          this.TextBoxRepeads = (TextBox) target;
          break;
        case 19:
          this.ButtonCheckZeroOffset = (Button) target;
          this.ButtonCheckZeroOffset.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 20:
          this.TextBlockStatus = (TextBlock) target;
          break;
        case 21:
          this.GroupBoxResults = (GroupBox) target;
          break;
        case 22:
          this.TextBoxCommandResult = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
