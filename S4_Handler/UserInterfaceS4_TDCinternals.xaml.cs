// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_TDCinternals
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using S4_Handler.Functions;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.UserInterface
{
  public partial class S4_TDCinternals : Window, IComponentConnector
  {
    private Cursor defaultCursor;
    private S4_HandlerWindowFunctions myWindowFunctions;
    private CancellationTokenSource cancelTokenSource;
    private ProgressHandler progress;
    private EventHandler<string> tdcEvent;
    private S4_TDC_Internals tdcInternals;
    private List<string> HidenGridColumns = new List<string>();
    internal DockPanel DockPanelPath;
    internal TextBox TextBoxPath;
    internal Button BtnStart;
    internal Button BtnStop;
    internal StackPanel StackPanalFunctions;
    internal TextBox TxtBxDelayTime;
    internal Button BtnCalibZeroOffset;
    internal Button ButtonTdcHardwareTest;
    internal StackPanel StackPanalButtonsLeft;
    internal Button ButtonShowTDC2;
    internal Button ButtonShowTDC1;
    internal Button ButtonClearLogFile;
    internal Button ButtonOpenLogFile;
    internal TabItem TabItemText;
    internal TextBox TxtBxResult;
    internal TabItem TabItemLog;
    internal DataGrid DataGridResults;
    internal TabItem TabItemLogSetup;
    internal ListBox listBoxActiveLogColumns;
    private bool _contentLoaded;

    public S4_TDCinternals(S4_HandlerWindowFunctions myWindowFunctions)
    {
      this.myWindowFunctions = myWindowFunctions;
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.tdcEvent = new EventHandler<string>(this.TdcEvent);
      this.cancelTokenSource = new CancellationTokenSource();
      this.InitializeComponent();
      this.tdcInternals = new S4_TDC_Internals(this.myWindowFunctions.MyFunctions.myMeters.ConnectedMeter, this.myWindowFunctions.MyFunctions.checkedCommands);
      this.TextBoxPath.Text = this.tdcInternals.LogFilePath;
      this.TxtBxResult.Text = "";
    }

    private void CloseTdcInternals(object sender, CancelEventArgs e)
    {
      this.cancelTokenSource.Cancel();
    }

    private void TdcEvent(object sender, string str)
    {
      if (!this.CheckAccess())
      {
        this.Dispatcher.Invoke((Action) (() => this.TdcEvent(sender, str)));
      }
      else
      {
        this.TxtBxResult.AppendText(str);
        if (this.TxtBxResult.Text.Length > 100000)
        {
          try
          {
            int startIndex = 10000;
            while (this.TxtBxResult.Text.Substring(startIndex, Environment.NewLine.Length) != Environment.NewLine)
              ++startIndex;
            this.TxtBxResult.Text = this.TxtBxResult.Text.Substring(startIndex + Environment.NewLine.Length);
          }
          catch
          {
            this.TxtBxResult.Clear();
          }
        }
        this.TxtBxResult.ScrollToEnd();
      }
    }

    private void OnProgress(ProgressArg obj)
    {
      if (this.CheckAccess())
        return;
      this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
    }

    private async void BtnStart_Click(object sender, RoutedEventArgs e)
    {
      this.TabItemText.Focus();
      this.RunState();
      ushort delay;
      ushort.TryParse(this.TxtBxDelayTime.Text, out delay);
      this.tdcInternals.LogFilePath = this.TextBoxPath.Text;
      await this.tdcInternals.LogTdcInternals((int) delay, (string) null, new EventHandler<string>(this.TdcEvent), this.progress, this.cancelTokenSource.Token);
    }

    private void BtnStop_Click(object sender, RoutedEventArgs e) => this.StopState();

    private void RunState()
    {
      this.cancelTokenSource = new CancellationTokenSource();
      this.BtnStart.IsEnabled = false;
      this.BtnStop.IsEnabled = true;
      this.DockPanelPath.IsEnabled = false;
      this.StackPanalButtonsLeft.IsEnabled = false;
      this.StackPanalFunctions.IsEnabled = false;
      if (this.defaultCursor == null)
        this.defaultCursor = this.Cursor;
      this.Cursor = Cursors.Cross;
    }

    private void StopState()
    {
      this.cancelTokenSource.Cancel();
      this.BtnStart.IsEnabled = true;
      this.BtnStop.IsEnabled = false;
      this.DockPanelPath.IsEnabled = true;
      this.StackPanalButtonsLeft.IsEnabled = true;
      this.StackPanalFunctions.IsEnabled = true;
      this.Cursor = this.defaultCursor;
    }

    private void Button_Click(object sender, RoutedEventArgs e) => this.Run_Button(sender);

    private async void Run_Button(object sender)
    {
      this.RunState();
      this.progress.Reset();
      try
      {
        if (sender == this.BtnCalibZeroOffset)
        {
          int timeS;
          if (!int.TryParse(this.TxtBxDelayTime.Text, out timeS))
            throw new ArgumentException("Wrong time setting Delay Time");
          await this.myWindowFunctions.MyFunctions.CalibrateZeroOffset(timeS, this.tdcEvent, this.progress, this.cancelTokenSource.Token);
        }
        else
        {
          if (sender != this.ButtonTdcHardwareTest)
            return;
          UltrasonicTestResults utr = await this.myWindowFunctions.MyFunctions.RunUltrasonicTest(this.progress, this.cancelTokenSource.Token);
          this.TxtBxResult.Text = utr.ToString();
          utr = (UltrasonicTestResults) null;
        }
      }
      catch (TaskCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.StopState();
      }
    }

    private void ButtonShowTDC2_Click(object sender, RoutedEventArgs e)
    {
      this.DataGridResults.ItemsSource = (IEnumerable) this.tdcInternals.tdc2Log;
      this.SetupGridColumes();
      this.TabItemLog.Focus();
    }

    private void ButtonShowTDC1_Click(object sender, RoutedEventArgs e)
    {
      this.DataGridResults.ItemsSource = (IEnumerable) this.tdcInternals.tdc1Log;
      this.SetupGridColumes();
      this.TabItemLog.Focus();
    }

    private void ButtonClearLogFile_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        File.Delete(this.TextBoxPath.Text);
        GmmMessage.Show("Log file deleted");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonOpenLogFile_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        new Process()
        {
          StartInfo = {
            FileName = this.TextBoxPath.Text
          }
        }.Start();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(e.Source is TabControl))
        return;
      if (((Selector) e.Source).SelectedItem == this.TabItemLogSetup)
      {
        this.CreateCheckBoxList();
      }
      else
      {
        this.SaveHiddenGridColumes();
        if (((Selector) e.Source).SelectedItem == this.TabItemLog)
          this.SetupGridColumes();
      }
    }

    public void CreateCheckBoxList()
    {
      this.listBoxActiveLogColumns.Items.Clear();
      for (int index = 0; index < this.DataGridResults.Columns.Count; ++index)
      {
        CheckBox newItem = new CheckBox();
        newItem.Content = (object) this.DataGridResults.Columns[index].Header.ToString();
        newItem.Tag = (object) index;
        if (this.DataGridResults.Columns[index].Visibility == Visibility.Visible)
          newItem.IsChecked = new bool?(true);
        this.listBoxActiveLogColumns.Items.Add((object) newItem);
      }
    }

    public void SaveHiddenGridColumes()
    {
      if (this.listBoxActiveLogColumns.Items.Count <= 0)
        return;
      this.HidenGridColumns.Clear();
      foreach (CheckBox checkBox in (IEnumerable) this.listBoxActiveLogColumns.Items)
      {
        if (!checkBox.IsChecked.Value)
          this.HidenGridColumns.Add((string) checkBox.Content);
      }
      this.listBoxActiveLogColumns.Items.Clear();
    }

    public void SetupGridColumes()
    {
      foreach (DataGridColumn column in (Collection<DataGridColumn>) this.DataGridResults.Columns)
        column.Visibility = this.HidenGridColumns.IndexOf((string) column.Header) < 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_tdcinternals.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((Window) target).Closing += new CancelEventHandler(this.CloseTdcInternals);
          break;
        case 2:
          this.DockPanelPath = (DockPanel) target;
          break;
        case 3:
          this.TextBoxPath = (TextBox) target;
          break;
        case 4:
          this.BtnStart = (Button) target;
          this.BtnStart.Click += new RoutedEventHandler(this.BtnStart_Click);
          break;
        case 5:
          this.BtnStop = (Button) target;
          this.BtnStop.Click += new RoutedEventHandler(this.BtnStop_Click);
          break;
        case 6:
          this.StackPanalFunctions = (StackPanel) target;
          break;
        case 7:
          this.TxtBxDelayTime = (TextBox) target;
          break;
        case 8:
          this.BtnCalibZeroOffset = (Button) target;
          this.BtnCalibZeroOffset.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 9:
          this.ButtonTdcHardwareTest = (Button) target;
          this.ButtonTdcHardwareTest.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 10:
          this.StackPanalButtonsLeft = (StackPanel) target;
          break;
        case 11:
          this.ButtonShowTDC2 = (Button) target;
          this.ButtonShowTDC2.Click += new RoutedEventHandler(this.ButtonShowTDC2_Click);
          break;
        case 12:
          this.ButtonShowTDC1 = (Button) target;
          this.ButtonShowTDC1.Click += new RoutedEventHandler(this.ButtonShowTDC1_Click);
          break;
        case 13:
          this.ButtonClearLogFile = (Button) target;
          this.ButtonClearLogFile.Click += new RoutedEventHandler(this.ButtonClearLogFile_Click);
          break;
        case 14:
          this.ButtonOpenLogFile = (Button) target;
          this.ButtonOpenLogFile.Click += new RoutedEventHandler(this.ButtonOpenLogFile_Click);
          break;
        case 15:
          ((Selector) target).SelectionChanged += new SelectionChangedEventHandler(this.TabControl_SelectionChanged);
          break;
        case 16:
          this.TabItemText = (TabItem) target;
          break;
        case 17:
          this.TxtBxResult = (TextBox) target;
          break;
        case 18:
          this.TabItemLog = (TabItem) target;
          break;
        case 19:
          this.DataGridResults = (DataGrid) target;
          break;
        case 20:
          this.TabItemLogSetup = (TabItem) target;
          break;
        case 21:
          this.listBoxActiveLogColumns = (ListBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
