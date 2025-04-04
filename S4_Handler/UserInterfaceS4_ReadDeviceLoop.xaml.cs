// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_ReadDeviceLoop
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using HandlerLib;
using S4_Handler.Functions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.UserInterface
{
  public partial class S4_ReadDeviceLoop : Window, IComponentConnector
  {
    private S4_HandlerFunctions myFunctions;
    private CancellationTokenSource cancelTokenSource;
    private ProgressHandler progress;
    private bool LockProgress;
    private int Loops;
    private int Errors;
    private int Successful;
    private S4_ReadDeviceLoop.LoopModes MyMode;
    private ReadPartsSelection ReadParts;
    private double avgSeconds;
    internal Grid GridMain;
    internal Label LabelMode;
    internal ComboBox ComboBoxMode;
    internal Label LabelLoopWaitTime;
    internal TextBox TextBoxLoopWaitTime;
    internal Label LabelInitTime;
    internal TextBox TextBoxInitWaitTime;
    internal Label LabelLoops;
    internal TextBox TextBoxLoops;
    internal Label LabelSuccessful;
    internal TextBox TextBoxSuccessful;
    internal Label LabelErrors;
    internal TextBox TextBoxErrors;
    internal Label LabelLastValidActionDate;
    internal TextBox TextBoxLastValidActionDate;
    internal TextBox TextBoxLastValidActionTime;
    internal TextBox TextBoxLastValidDurationTime;
    internal CheckBox CheckBoxStopLoopAfterError;
    internal CheckBox CheckBoxInitializeComPortAfterError;
    internal CheckBox CheckBoxWaitAfterOpen;
    internal GroupBox GroupBoxMessages;
    internal TextBox TextBoxMessages;
    internal Label LabelReadPartsList;
    internal ComboBox ComboBoxPartsList;
    internal Button ButtonReset;
    internal Button ButtonStart;
    internal Button ButtonStop;
    internal StatusBar MyStatusBar;
    internal ProgressBar ProgressBarStatus;
    internal TextBlock TextBoxProgress;
    private bool _contentLoaded;

    public S4_ReadDeviceLoop(S4_HandlerFunctions theFunctions)
    {
      this.InitializeComponent();
      this.myFunctions = theFunctions;
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.LockProgress = false;
      this.TextBoxInitWaitTime.Text = "2000";
      this.TextBoxLoopWaitTime.Text = "0";
      this.TextBoxLastValidActionTime.Text = "0";
      this.ComboBoxMode.Items.Clear();
      foreach (object name in Enum.GetNames(typeof (S4_ReadDeviceLoop.LoopModes)))
        this.ComboBoxMode.Items.Add(name);
      this.ComboBoxMode.DataContext = (object) Enum.GetNames(typeof (S4_ReadDeviceLoop.LoopModes));
      this.ComboBoxMode.SelectedIndex = 0;
      this.ComboBoxPartsList.Items.Clear();
      foreach (object name in Enum.GetNames(typeof (ReadPartsSelection)))
        this.ComboBoxPartsList.Items.Add(name);
      this.ComboBoxPartsList.DataContext = (object) Enum.GetNames(typeof (ReadPartsSelection));
      this.ComboBoxPartsList.SelectedIndex = 0;
      this.ResetResult();
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      if (this.ButtonStart.IsEnabled)
        return;
      e.Cancel = true;
    }

    private void ResetResult()
    {
      this.ReadParts = ReadPartsSelection.AllWithoutLogger;
      this.avgSeconds = 0.0;
      this.Loops = 0;
      this.Errors = 0;
      this.Successful = 0;
      this.TextBoxLoops.Text = "0";
      this.TextBoxSuccessful.Text = "0";
      this.TextBoxErrors.Text = "0";
      this.TextBoxLastValidActionTime.Text = "0";
      this.TextBoxLastValidActionDate.Clear();
      this.TextBoxMessages.Clear();
    }

    private void LockControls()
    {
      this.ButtonStart.IsEnabled = false;
      this.ButtonReset.IsEnabled = false;
      this.TextBoxLoopWaitTime.IsEnabled = false;
      this.TextBoxInitWaitTime.IsEnabled = false;
    }

    private void FreeControls()
    {
      this.ButtonStart.IsEnabled = true;
      this.ButtonReset.IsEnabled = true;
      this.TextBoxLoopWaitTime.IsEnabled = true;
      this.TextBoxInitWaitTime.IsEnabled = true;
    }

    private async void ButtonStart_Click(object sender, RoutedEventArgs e)
    {
      this.LockProgress = false;
      this.cancelTokenSource = new CancellationTokenSource();
      this.LockControls();
      int LoopMilliseconds = 0;
      int InitMilliseconds = 0;
      bool Initialize = false;
      try
      {
        LoopMilliseconds = int.Parse(this.TextBoxLoopWaitTime.Text.Trim());
        InitMilliseconds = int.Parse(this.TextBoxInitWaitTime.Text.Trim());
      }
      catch
      {
        int num = (int) MessageBox.Show("Wrong input", "Start", MessageBoxButton.OK, MessageBoxImage.Hand);
        this.FreeControls();
        this.InitializeProgress();
        return;
      }
      do
      {
        this.AppendEmptyLine();
        this.ClearProgress();
        bool? isChecked;
        try
        {
          TimeSpan myTS = new TimeSpan();
          this.MyMode = (S4_ReadDeviceLoop.LoopModes) this.ComboBoxMode.SelectedIndex;
          this.ReadParts = (ReadPartsSelection) this.ComboBoxPartsList.SelectedIndex;
          this.AppendMessage("Start loop " + (this.Loops + 1).ToString());
          ++this.Loops;
          this.TextBoxLoops.Text = this.Loops.ToString();
          if (Initialize)
          {
            Initialize = false;
            this.AppendMessage("Close connection: ");
            this.myFunctions.Close();
            this.AppendOKMessagePart();
            this.AppendMessage("Clear handler: ");
            this.myFunctions.Clear();
            this.AppendOKMessagePart();
            this.AppendMessage("Wait " + InitMilliseconds.ToString() + " ms: ");
            bool flag1 = await this.WaitMilliSeconds(InitMilliseconds, this.cancelTokenSource.Token);
            if (!flag1)
            {
              this.AppendMessagePart("Cancelled");
              break;
            }
            this.AppendOKMessagePart();
            this.AppendMessage("Open connection: ");
            this.myFunctions.Open();
            this.AppendOKMessagePart();
            isChecked = this.CheckBoxWaitAfterOpen.IsChecked;
            if (isChecked.Value)
            {
              this.AppendMessage("Wait " + InitMilliseconds.ToString() + " ms: ");
              bool flag2 = await this.WaitMilliSeconds(InitMilliseconds, this.cancelTokenSource.Token);
              if (!flag2)
              {
                this.AppendMessagePart("Cancelled");
                break;
              }
              this.AppendOKMessagePart();
            }
          }
          if (this.MyMode == S4_ReadDeviceLoop.LoopModes.ReadDeviceLoop)
          {
            this.AppendMessage("Read device: ");
            DateTime stTime = DateTime.Now;
            int num = await this.myFunctions.ReadDeviceAsync(this.progress, this.cancelTokenSource.Token, this.ReadParts);
            myTS = DateTime.Now - stTime;
            this.avgSeconds += myTS.TotalMilliseconds;
            this.AppendOKMessagePart();
          }
          else if (this.MyMode == S4_ReadDeviceLoop.LoopModes.ReadVersionLoop)
          {
            this.AppendMessage("Read version: ");
            DeviceIdentification TheIdentification = await this.myFunctions.ReadVersionAsync(this.progress, this.cancelTokenSource.Token);
            this.AppendOKMessagePart();
            this.AppendMessage(TheIdentification.ToString());
            TheIdentification = (DeviceIdentification) null;
          }
          else
          {
            this.cancelTokenSource.Cancel();
            throw new ApplicationException("Unknown mode!");
          }
          this.TextBoxLastValidActionDate.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
          double secs = this.avgSeconds / (double) this.Loops;
          this.TextBoxLastValidActionTime.Text = secs.ToString("####.##") + " ms.";
          ++this.Successful;
          this.TextBoxSuccessful.Text = this.Successful.ToString();
          this.AppendMessage("Wait " + LoopMilliseconds.ToString() + " ms: ");
          bool flag = await this.WaitMilliSeconds(LoopMilliseconds, this.cancelTokenSource.Token);
          if (!flag)
          {
            this.AppendMessagePart("Cancelled");
            break;
          }
          this.AppendOKMessagePart();
          myTS = new TimeSpan();
        }
        catch (Exception ex)
        {
          ++this.Errors;
          this.TextBoxErrors.Text = this.Errors.ToString();
          if (this.cancelTokenSource.Token.IsCancellationRequested)
            this.AppendMessagePart("Cancelled");
          else
            this.AppendERRORMessagePart();
          isChecked = this.CheckBoxStopLoopAfterError.IsChecked;
          if (isChecked.Value)
          {
            ExceptionViewer.Show(ex);
            break;
          }
          isChecked = this.CheckBoxInitializeComPortAfterError.IsChecked;
          if (isChecked.Value)
            Initialize = true;
        }
      }
      while (!this.cancelTokenSource.Token.IsCancellationRequested);
      this.FreeControls();
      this.InitializeProgress();
    }

    private void ButtonStop_Click(object sender, RoutedEventArgs e)
    {
      this.cancelTokenSource.Cancel();
    }

    private void ButtonReset_Click(object sender, RoutedEventArgs e) => this.ResetResult();

    private void AppendMessage(string TheMessage)
    {
      try
      {
        if (this.TextBoxMessages.Text == string.Empty)
          this.TextBoxMessages.AppendText(TheMessage);
        else
          this.TextBoxMessages.AppendText(Environment.NewLine + TheMessage);
        this.TextBoxMessages.ScrollToEnd();
      }
      catch
      {
        this.TextBoxMessages.Clear();
      }
    }

    private void AppendMessagePart(string TheMessage)
    {
      this.TextBoxMessages.AppendText(TheMessage);
      this.TextBoxMessages.ScrollToEnd();
    }

    private void AppendOKMessagePart() => this.AppendMessagePart("OK");

    private void AppendERRORMessagePart() => this.AppendMessagePart("ERROR");

    private void AppendEmptyLine() => this.AppendMessage(string.Empty);

    private void InitializeProgress()
    {
      this.LockProgress = true;
      this.ProgressBarStatus.Value = this.ProgressBarStatus.Minimum;
      this.TextBoxProgress.Text = string.Empty;
    }

    private void ClearProgress()
    {
      this.ProgressBarStatus.Value = this.ProgressBarStatus.Minimum;
      this.TextBoxProgress.Text = string.Empty;
      this.progress.Reset();
    }

    private async Task<bool> WaitMilliSeconds(int MilliSeconds, CancellationToken cancelToken)
    {
      DateTime StartTime = DateTime.Now;
      DateTime EndTime = StartTime.AddMilliseconds((double) MilliSeconds);
      long MaxTicks = EndTime.Subtract(StartTime).Ticks;
      do
      {
        await Task.Delay(1, cancelToken);
        if (cancelToken.IsCancellationRequested)
          return false;
      }
      while (DateTime.Now < EndTime);
      return true;
    }

    private void OnProgress(ProgressArg obj)
    {
      if (!this.CheckAccess())
      {
        this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
      }
      else
      {
        if (this.LockProgress)
          return;
        this.ProgressBarStatus.Value = obj.ProgressPercentage;
        this.TextBoxProgress.Text = obj.Message;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_readdeviceloop.xaml", UriKind.Relative));
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
          this.GridMain = (Grid) target;
          break;
        case 3:
          this.LabelMode = (Label) target;
          break;
        case 4:
          this.ComboBoxMode = (ComboBox) target;
          break;
        case 5:
          this.LabelLoopWaitTime = (Label) target;
          break;
        case 6:
          this.TextBoxLoopWaitTime = (TextBox) target;
          break;
        case 7:
          this.LabelInitTime = (Label) target;
          break;
        case 8:
          this.TextBoxInitWaitTime = (TextBox) target;
          break;
        case 9:
          this.LabelLoops = (Label) target;
          break;
        case 10:
          this.TextBoxLoops = (TextBox) target;
          break;
        case 11:
          this.LabelSuccessful = (Label) target;
          break;
        case 12:
          this.TextBoxSuccessful = (TextBox) target;
          break;
        case 13:
          this.LabelErrors = (Label) target;
          break;
        case 14:
          this.TextBoxErrors = (TextBox) target;
          break;
        case 15:
          this.LabelLastValidActionDate = (Label) target;
          break;
        case 16:
          this.TextBoxLastValidActionDate = (TextBox) target;
          break;
        case 17:
          this.TextBoxLastValidActionTime = (TextBox) target;
          break;
        case 18:
          this.TextBoxLastValidDurationTime = (TextBox) target;
          break;
        case 19:
          this.CheckBoxStopLoopAfterError = (CheckBox) target;
          break;
        case 20:
          this.CheckBoxInitializeComPortAfterError = (CheckBox) target;
          break;
        case 21:
          this.CheckBoxWaitAfterOpen = (CheckBox) target;
          break;
        case 22:
          this.GroupBoxMessages = (GroupBox) target;
          break;
        case 23:
          this.TextBoxMessages = (TextBox) target;
          break;
        case 24:
          this.LabelReadPartsList = (Label) target;
          break;
        case 25:
          this.ComboBoxPartsList = (ComboBox) target;
          break;
        case 26:
          this.ButtonReset = (Button) target;
          this.ButtonReset.Click += new RoutedEventHandler(this.ButtonReset_Click);
          break;
        case 27:
          this.ButtonStart = (Button) target;
          this.ButtonStart.Click += new RoutedEventHandler(this.ButtonStart_Click);
          break;
        case 28:
          this.ButtonStop = (Button) target;
          this.ButtonStop.Click += new RoutedEventHandler(this.ButtonStop_Click);
          break;
        case 29:
          this.MyStatusBar = (StatusBar) target;
          break;
        case 30:
          this.ProgressBarStatus = (ProgressBar) target;
          break;
        case 31:
          this.TextBoxProgress = (TextBlock) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    private enum LoopModes
    {
      ReadDeviceLoop,
      ReadVersionLoop,
    }
  }
}
