// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_CurrentMeasureWindow
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using S4_Handler.Functions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.UserInterface
{
  public partial class S4_CurrentMeasureWindow : Window, IComponentConnector
  {
    private S4_HandlerFunctions MyFunctions;
    private S4_CurrentMeasure MyMeasure;
    private Cursor defaultCursor;
    private ProgressHandler progress;
    private CancellationTokenSource CancelTokenSource;
    internal StartupLib.GmmCorporateControl gmmCorporateControl1;
    internal StackPanel StackPanelButtons;
    internal Button ButtonRun;
    internal Button ButtonBreak;
    internal StackPanel StackPanelSetup;
    internal GroupBox GroupBoxMode;
    internal RadioButton RadioButtonCPU_standby;
    internal RadioButton RadioButtonOperationMode;
    internal RadioButton RadioButtonWorking;
    internal RadioButton RadioButtonRadio_active;
    internal Button ButtonShowDatabaseSetup;
    internal GroupBox GroupBoxMeasureValues;
    internal TextBox TextBoxMeasureTime;
    internal GroupBox GroupBoxLimits;
    internal TextBox TextBoxGraphicLimitsMin;
    internal TextBox TextBoxGraphicLimitsMax;
    internal Button ButtonRefreshGraphic;
    internal Button ButtonClearGraphic;
    internal GroupBox GroupBoxLoops;
    internal CheckBox CheckBoxRepeatToBreak;
    internal TextBox TextBoxPauseBetweenLoops;
    internal Button ButtonIsShuntAssambled;
    internal TextBlock TextBlockStatus;
    internal Grid GridGraphicAndText;
    internal GmmGraphicControl MyGraphicControl;
    internal TextBox TextBoxOutput;
    private bool _contentLoaded;

    public S4_CurrentMeasureWindow(S4_HandlerFunctions myFunctions)
    {
      this.MyFunctions = myFunctions;
      this.InitializeComponent();
      this.MyMeasure = new S4_CurrentMeasure(this.MyFunctions);
      this.TextBoxOutput.Text = !this.MyMeasure.ConfigFromDatabaseOK ? this.MyMeasure.TestInfoText.ToString() : this.MyMeasure.GetPreparedDataAsText();
      bool flag = true;
      if (this.MyMeasure.IsMeasureModePossible(S4_CurrentMeasureMode.Standby, false))
      {
        this.RadioButtonCPU_standby.IsEnabled = true;
        if (flag)
        {
          flag = false;
          this.RadioButtonCPU_standby.IsChecked = new bool?(true);
        }
      }
      if (this.MyMeasure.IsMeasureModePossible(S4_CurrentMeasureMode.OperationMode, false))
      {
        this.RadioButtonOperationMode.IsEnabled = true;
        if (flag)
        {
          flag = false;
          this.RadioButtonOperationMode.IsChecked = new bool?(true);
        }
      }
      if (this.MyMeasure.IsMeasureModePossible(S4_CurrentMeasureMode.WorkingMode, false))
      {
        this.RadioButtonWorking.IsEnabled = true;
        if (flag)
        {
          flag = false;
          this.RadioButtonWorking.IsChecked = new bool?(true);
        }
      }
      if (this.MyMeasure.IsMeasureModePossible(S4_CurrentMeasureMode.RadioCarrierOn, false))
      {
        this.RadioButtonRadio_active.IsEnabled = true;
        if (flag)
          this.RadioButtonRadio_active.IsChecked = new bool?(true);
      }
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.SetStopState();
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      if (this.CancelTokenSource == null)
        return;
      this.CancelTokenSource.Cancel();
    }

    private void ButtonShowDatabaseSetup_Click(object sender, RoutedEventArgs e)
    {
      this.TextBoxOutput.Text = this.MyMeasure.TestInfoText.ToString();
      if (this.MyMeasure.DatabaseDataException == null)
        return;
      ExceptionViewer.Show(this.MyMeasure.DatabaseDataException, "Database data exception");
    }

    private void RadioButtonCPU_standby_Checked(object sender, RoutedEventArgs e)
    {
      this.SetDataForMeasureMode(S4_CurrentMeasureMode.Standby);
    }

    private void RadioButtonOperationMode_Checked(object sender, RoutedEventArgs e)
    {
      this.SetDataForMeasureMode(S4_CurrentMeasureMode.OperationMode);
    }

    private void RadioButtonWorking_Checked(object sender, RoutedEventArgs e)
    {
      this.SetDataForMeasureMode(S4_CurrentMeasureMode.WorkingMode);
    }

    private void RadioButtonRadio_active_Checked(object sender, RoutedEventArgs e)
    {
      this.SetDataForMeasureMode(S4_CurrentMeasureMode.RadioCarrierOn);
    }

    private void SetDataForMeasureMode(S4_CurrentMeasureMode mode)
    {
      this.MyMeasure.PrepareMeasure(mode);
      this.TextBoxMeasureTime.Text = ((int) this.MyMeasure.PreparedMeasureTime_ms).ToString();
      this.TextBoxOutput.Text = this.MyMeasure.GetPreparedDataAsText();
    }

    private void ButtonBreak_Click(object sender, RoutedEventArgs e)
    {
      if (this.CancelTokenSource == null)
        return;
      this.CancelTokenSource.Cancel();
    }

    private void ButtonRefreshGraphic_Click(object sender, RoutedEventArgs e)
    {
      this.ShowTheGraphic();
    }

    private void ButtonClearGraphic_Click(object sender, RoutedEventArgs e)
    {
      this.MyGraphicControl.ClearGraphic();
    }

    private void SetRunState()
    {
      this.CancelTokenSource = new CancellationTokenSource();
      this.progress.Reset();
      this.ButtonRun.IsEnabled = false;
      this.ButtonBreak.IsEnabled = true;
      this.ButtonRefreshGraphic.IsEnabled = false;
      this.GroupBoxMode.IsEnabled = false;
      this.GroupBoxMeasureValues.IsEnabled = false;
      this.GroupBoxLimits.IsEnabled = false;
      this.CheckBoxRepeatToBreak.IsEnabled = false;
      if (this.Cursor == Cursors.Wait)
        return;
      this.defaultCursor = this.Cursor;
      this.Cursor = Cursors.Wait;
    }

    private void SetStopState()
    {
      this.ButtonRun.IsEnabled = true;
      this.ButtonBreak.IsEnabled = false;
      this.ButtonRefreshGraphic.IsEnabled = true;
      this.GroupBoxMode.IsEnabled = true;
      this.GroupBoxMeasureValues.IsEnabled = true;
      this.GroupBoxLimits.IsEnabled = true;
      this.CheckBoxRepeatToBreak.IsEnabled = true;
      this.Cursor = this.defaultCursor;
      this.progress.Reset();
      this.TextBlockStatus.Text = "";
    }

    private void OnProgress(ProgressArg obj)
    {
      if (!this.CheckAccess())
      {
        this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
      }
      else
      {
        if (obj.Message == null)
          return;
        this.TextBlockStatus.Text = obj.Message;
      }
    }

    private S4_CurrentMeasureMode GetMeasureMode()
    {
      if (this.RadioButtonCPU_standby.IsChecked.Value)
        return S4_CurrentMeasureMode.Standby;
      bool? isChecked = this.RadioButtonOperationMode.IsChecked;
      if (isChecked.Value)
        return S4_CurrentMeasureMode.OperationMode;
      isChecked = this.RadioButtonWorking.IsChecked;
      return isChecked.Value ? S4_CurrentMeasureMode.WorkingMode : S4_CurrentMeasureMode.RadioCarrierOn;
    }

    private async void ButtonRun_Click(object sender, RoutedEventArgs e)
    {
      int iPause = 0;
      this.SetRunState();
      this.MyGraphicControl.ClearGraphic();
      this.TextBoxOutput.Clear();
      string sPause = this.TextBoxPauseBetweenLoops.Text.Trim();
      if (!string.IsNullOrEmpty(sPause))
        int.TryParse(sPause, out iPause);
      try
      {
        double middleCurrent_mA = 0.0;
        int n = 1;
        uint time_ms;
        while (uint.TryParse(this.TextBoxMeasureTime.Text, out time_ms))
        {
          S4_CurrentMeasureMode measureMode = this.GetMeasureMode();
          await this.MyMeasure.RunMeasureAsync(this.progress, this.CancelTokenSource.Token, measureMode, (double) time_ms);
          this.AutoScaleGraphics();
          this.ShowTheGraphic();
          double current_mA = this.MyMeasure.Current_mA;
          middleCurrent_mA = n != 1 ? middleCurrent_mA / (double) n * (double) (n - 1) + current_mA / (double) n : current_mA;
          bool outOfLimits = false;
          double? nullable = this.MyMeasure.PreparedMinCurrent_mA;
          int num1;
          if (nullable.HasValue)
          {
            nullable = this.MyMeasure.PreparedMaxCurrent_mA;
            num1 = nullable.HasValue ? 1 : 0;
          }
          else
            num1 = 0;
          if (num1 != 0)
          {
            double num2 = current_mA;
            nullable = this.MyMeasure.PreparedMinCurrent_mA;
            double num3 = nullable.Value;
            int num4;
            if (num2 >= num3)
            {
              double num5 = current_mA;
              nullable = this.MyMeasure.PreparedMaxCurrent_mA;
              double num6 = nullable.Value;
              num4 = num5 > num6 ? 1 : 0;
            }
            else
              num4 = 1;
            outOfLimits = num4 != 0;
          }
          if (this.CheckBoxRepeatToBreak.IsChecked.Value)
          {
            this.TextBoxOutput.AppendText("Time: " + DateTime.Now.ToLongTimeString() + Environment.NewLine);
            if (outOfLimits)
              this.TextBoxOutput.AppendText(n.ToString() + ": " + current_mA.ToString("0.000") + " mA !!! out of limits !!!;  middeleValue: " + middleCurrent_mA.ToString("0.0000") + Environment.NewLine);
            else
              this.TextBoxOutput.AppendText(n.ToString() + ": " + current_mA.ToString("0.000") + " mA;  middeleValue: " + middleCurrent_mA.ToString("0.0000") + Environment.NewLine);
            this.TextBoxOutput.ScrollToEnd();
            Thread.Sleep(iPause * 1000);
            ++n;
          }
          else
          {
            this.TextBoxOutput.Text = this.MyMeasure.ToString();
            if (outOfLimits)
            {
              this.TextBoxOutput.Background = (Brush) Brushes.LightYellow;
              goto label_25;
            }
            else
            {
              this.TextBoxOutput.Background = this.TextBlockStatus.Background;
              goto label_25;
            }
          }
        }
        throw new Exception("Illegal time value");
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Current measurement");
        this.TextBoxOutput.Text = this.MyMeasure.ToString();
      }
label_25:
      this.SetStopState();
      sPause = (string) null;
    }

    private async void ButtonIsShuntAssambled_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      this.TextBoxOutput.Clear();
      try
      {
        bool shuntAssembled = await this.MyMeasure.IsShuntAssembled(this.progress, this.CancelTokenSource.Token);
        this.AutoScaleGraphics();
        this.ShowTheGraphic();
        this.TextBoxOutput.Text = !shuntAssembled ? "Shunt is not assembled" + Environment.NewLine + this.MyMeasure.ToString() : "!!!! Shunt is assembled. Current measurement not prepared. !!!!" + Environment.NewLine + this.MyMeasure.ToString();
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "IsShuntAssambled test");
        this.TextBoxOutput.Text = this.MyMeasure.ToString();
      }
      this.SetStopState();
    }

    private void AutoScaleGraphics()
    {
      if (this.MyMeasure.Voltages == null)
        return;
      double num1 = double.MinValue;
      double num2 = double.MaxValue;
      for (int samplesSuppression = this.MyMeasure.StartSamplesSuppression; samplesSuppression < this.MyMeasure.Voltages.Length; ++samplesSuppression)
      {
        double voltage = this.MyMeasure.Voltages[samplesSuppression];
        if (voltage > num1)
          num1 = voltage;
        if (voltage < num2)
          num2 = voltage;
      }
      double num3 = num1 + 0.1;
      double num4 = num2 - 0.1;
      if (num3 > 3.8)
        num3 = 3.8;
      if (num4 < 2.6)
        num4 = 2.6;
      double num5 = (double) (int) (num3 * 10.0 + 0.999) / 10.0;
      double num6 = (double) (int) (num4 * 10.0 - 0.999) / 10.0;
      this.TextBoxGraphicLimitsMax.Text = num5.ToString("0.00");
      this.TextBoxGraphicLimitsMin.Text = num6.ToString("0.00");
    }

    private void ShowTheGraphic()
    {
      if (this.MyMeasure.Voltages == null)
        return;
      double result1;
      double.TryParse(this.TextBoxGraphicLimitsMin.Text, out result1);
      double result2;
      double.TryParse(this.TextBoxGraphicLimitsMax.Text, out result2);
      double TheStep = 0.1 * 2.0;
      GmmGraphicControl.GraphicData TheGraphicData = new GmmGraphicControl.GraphicData();
      TheGraphicData.SetBackGroundColor(Colors.Black);
      TheGraphicData.SetGridColor(Colors.LightGray);
      TheGraphicData.SetAxisColor(Colors.Red);
      TheGraphicData.SetLabelColor(Colors.White);
      TheGraphicData.AddGrid(0.0, 10.0, result1, 0.1);
      TheGraphicData.SetYAxis(result1, TheStep, "Voltage/V", true);
      TheGraphicData.SetXAxis(0.0, 10.0, "ADC_Steps/10", true);
      TheGraphicData.SetYLimits(result1, result2);
      TheGraphicData.AddLine(0, ((IEnumerable<double>) this.MyMeasure.Voltages).ToList<double>());
      TheGraphicData.SetColor(0, Colors.Yellow);
      this.MyGraphicControl.ShowGraphic(TheGraphicData);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_currentmeasurewindow.xaml", UriKind.Relative));
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
          this.gmmCorporateControl1 = (StartupLib.GmmCorporateControl) target;
          break;
        case 3:
          this.StackPanelButtons = (StackPanel) target;
          break;
        case 4:
          this.ButtonRun = (Button) target;
          this.ButtonRun.Click += new RoutedEventHandler(this.ButtonRun_Click);
          break;
        case 5:
          this.ButtonBreak = (Button) target;
          this.ButtonBreak.Click += new RoutedEventHandler(this.ButtonBreak_Click);
          break;
        case 6:
          this.StackPanelSetup = (StackPanel) target;
          break;
        case 7:
          this.GroupBoxMode = (GroupBox) target;
          break;
        case 8:
          this.RadioButtonCPU_standby = (RadioButton) target;
          this.RadioButtonCPU_standby.Checked += new RoutedEventHandler(this.RadioButtonCPU_standby_Checked);
          break;
        case 9:
          this.RadioButtonOperationMode = (RadioButton) target;
          this.RadioButtonOperationMode.Checked += new RoutedEventHandler(this.RadioButtonOperationMode_Checked);
          break;
        case 10:
          this.RadioButtonWorking = (RadioButton) target;
          this.RadioButtonWorking.Checked += new RoutedEventHandler(this.RadioButtonWorking_Checked);
          break;
        case 11:
          this.RadioButtonRadio_active = (RadioButton) target;
          this.RadioButtonRadio_active.Checked += new RoutedEventHandler(this.RadioButtonRadio_active_Checked);
          break;
        case 12:
          this.ButtonShowDatabaseSetup = (Button) target;
          this.ButtonShowDatabaseSetup.Click += new RoutedEventHandler(this.ButtonShowDatabaseSetup_Click);
          break;
        case 13:
          this.GroupBoxMeasureValues = (GroupBox) target;
          break;
        case 14:
          this.TextBoxMeasureTime = (TextBox) target;
          break;
        case 15:
          this.GroupBoxLimits = (GroupBox) target;
          break;
        case 16:
          this.TextBoxGraphicLimitsMin = (TextBox) target;
          break;
        case 17:
          this.TextBoxGraphicLimitsMax = (TextBox) target;
          break;
        case 18:
          this.ButtonRefreshGraphic = (Button) target;
          this.ButtonRefreshGraphic.Click += new RoutedEventHandler(this.ButtonRefreshGraphic_Click);
          break;
        case 19:
          this.ButtonClearGraphic = (Button) target;
          this.ButtonClearGraphic.Click += new RoutedEventHandler(this.ButtonClearGraphic_Click);
          break;
        case 20:
          this.GroupBoxLoops = (GroupBox) target;
          break;
        case 21:
          this.CheckBoxRepeatToBreak = (CheckBox) target;
          break;
        case 22:
          this.TextBoxPauseBetweenLoops = (TextBox) target;
          break;
        case 23:
          this.ButtonIsShuntAssambled = (Button) target;
          this.ButtonIsShuntAssambled.Click += new RoutedEventHandler(this.ButtonIsShuntAssambled_Click);
          break;
        case 24:
          this.TextBlockStatus = (TextBlock) target;
          break;
        case 25:
          this.GridGraphicAndText = (Grid) target;
          break;
        case 26:
          this.MyGraphicControl = (GmmGraphicControl) target;
          break;
        case 27:
          this.TextBoxOutput = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
