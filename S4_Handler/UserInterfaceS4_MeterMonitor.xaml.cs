// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_MeterMonitor
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using HandlerLib;
using S4_Handler.Functions;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.ObjectModel;
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
  public partial class S4_MeterMonitor : Window, IComponentConnector
  {
    private S4_DeviceCommandsNFC Nfc;
    private bool breakRequired = true;
    private ProgressHandler progress;
    private CancellationTokenSource cancelTokenSource;
    private CancellationToken cancelToken;
    public ObservableCollection<S4_MonitorData> MonitorData;
    internal StartupLib.GmmCorporateControl gmmCorporateControl1;
    internal Button ButtonClear;
    internal Button ButtonRunStop;
    internal DataGrid DataGridMonitor;
    private bool _contentLoaded;

    internal S4_MeterMonitor(S4_DeviceCommandsNFC nfc)
    {
      this.Nfc = nfc;
      this.InitializeComponent();
      this.cancelTokenSource = new CancellationTokenSource();
      this.cancelToken = this.cancelTokenSource.Token;
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.MonitorData = new ObservableCollection<S4_MonitorData>();
    }

    private void OnProgress(ProgressArg obj)
    {
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      if (!(this.ButtonRunStop.Content.ToString() == "Break"))
        return;
      e.Cancel = true;
    }

    private async void ButtonRunStop_Click(object sender, RoutedEventArgs e)
    {
      if (this.ButtonRunStop.Content.ToString() == "Break")
      {
        this.breakRequired = true;
      }
      else
      {
        try
        {
          this.ButtonRunStop.Content = (object) "Break";
          this.breakRequired = false;
          this.progress.Reset();
          while (!this.breakRequired)
          {
            S4_CurrentData currentData = await this.Nfc.ReadCurrentDataAsync(this.progress, this.cancelToken);
            S4_SystemState deviceState = await this.Nfc.GetDeviceStatesAsync(this.progress, this.cancelToken);
            S4_FunctionalState functionState = await this.Nfc.ReadAlliveAndStateAsync(this.progress, this.cancelToken);
            S4_MonitorData newData = new S4_MonitorData();
            newData.DeviceTime = currentData.DeviceTime;
            newData.Volume = currentData.Volume;
            newData.FlowVolume = currentData.FlowVolume;
            newData.ReturnVolume = currentData.ReturnVolume;
            newData.Flow = (double) currentData.Flow;
            newData.Temp = (double) currentData.WaterTemperature;
            newData.Alarm = functionState.Alarm;
            newData.CCC = functionState.ConfigChangedCounter.Value;
            newData.Flags = functionState.ToString();
            this.MonitorData.Add(newData);
            if (this.DataGridMonitor.ItemsSource == null)
              this.DataGridMonitor.ItemsSource = (IEnumerable) this.MonitorData;
            this.DataGridMonitor.ScrollIntoView((object) newData);
            await Task.Delay(2000);
            currentData = (S4_CurrentData) null;
            deviceState = (S4_SystemState) null;
            functionState = (S4_FunctionalState) null;
            newData = (S4_MonitorData) null;
          }
        }
        catch (Exception ex)
        {
          ExceptionViewer.Show(ex);
        }
        finally
        {
          this.ButtonRunStop.Content = (object) "Run";
        }
      }
    }

    private void DataGridMonitor_AutoGeneratingColumn(
      object sender,
      DataGridAutoGeneratingColumnEventArgs e)
    {
      if (!(e.Column is DataGridTextColumn column))
        return;
      if (e.PropertyName == "PcTime" || e.PropertyName == "DeviceTime")
        column.Binding.StringFormat = "dd.MM.yyyy HH.mm.ss";
      else if (e.PropertyName.Contains("Vol") || e.PropertyName == "Flow")
        column.Binding.StringFormat = "0.000";
      else if (e.PropertyName == "Temp")
      {
        column.Binding.StringFormat = "0.00";
        column.Header = (object) "Temp [°C]";
      }
    }

    private void ButtonClear_Click(object sender, RoutedEventArgs e) => this.MonitorData.Clear();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_metermonitor.xaml", UriKind.Relative));
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
          this.ButtonClear = (Button) target;
          this.ButtonClear.Click += new RoutedEventHandler(this.ButtonClear_Click);
          break;
        case 4:
          this.ButtonRunStop = (Button) target;
          this.ButtonRunStop.Click += new RoutedEventHandler(this.ButtonRunStop_Click);
          break;
        case 5:
          this.DataGridMonitor = (DataGrid) target;
          this.DataGridMonitor.AutoGeneratingColumn += new EventHandler<DataGridAutoGeneratingColumnEventArgs>(this.DataGridMonitor_AutoGeneratingColumn);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
