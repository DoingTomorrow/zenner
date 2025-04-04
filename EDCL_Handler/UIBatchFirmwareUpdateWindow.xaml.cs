// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.UI.BatchFirmwareUpdateWindow
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using Microsoft.Win32;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace EDCL_Handler.UI
{
  public partial class BatchFirmwareUpdateWindow : Window, IComponentConnector
  {
    private Thread CheckComTh;
    private List<Dut_Handler> Duts = new List<Dut_Handler>();
    private object DutLocker = new object();
    private AutoResetEvent WaitFlag;
    private bool stopcheckcom = false;
    private object stopcheckcomLocker = new object();
    private bool needrefresh = false;
    private object needrefreshLocker = new object();
    private bool exit = false;
    private object exitLocker = new object();
    private bool working = false;
    private object workingLocker = new object();
    private object RefreshDGLocker = new object();
    internal TextBox FWText;
    internal Button StartBtn;
    internal DataGrid DG;
    internal TextBox BLText;
    internal Button ReadBtn;
    private bool _contentLoaded;

    public BatchFirmwareUpdateWindow()
    {
      this.InitializeComponent();
      this.LoadCOM();
      this.WaitFlag = new AutoResetEvent(false);
      Dut_Handler.RefreshDGInvoke = new RefreshDGBack(this.RefreshDG);
      this.Exit = false;
      this.CheckComTh = new Thread(new ThreadStart(this.CheckCOMThread));
      this.CheckComTh.IsBackground = true;
      this.CheckComTh.Start();
    }

    private bool NeedRefresh
    {
      set
      {
        lock (this.needrefreshLocker)
          this.needrefresh = value;
      }
      get
      {
        lock (this.needrefreshLocker)
          return this.needrefresh;
      }
    }

    private bool StopCheckCOM
    {
      set
      {
        lock (this.stopcheckcomLocker)
          this.stopcheckcom = value;
      }
      get
      {
        lock (this.stopcheckcomLocker)
          return this.stopcheckcom;
      }
    }

    private bool Exit
    {
      set
      {
        lock (this.exitLocker)
          this.exit = value;
      }
      get
      {
        lock (this.exitLocker)
          return this.exit;
      }
    }

    private bool Working
    {
      set
      {
        lock (this.workingLocker)
          this.working = value;
      }
      get
      {
        lock (this.workingLocker)
          return this.working;
      }
    }

    private void LoadCOM()
    {
      foreach (string str in PlugInLoader.GmmConfiguration.GetValue("BatchFirmwareUpdate", "ComPorts").Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
      {
        Dut_Handler newItem = new Dut_Handler(str.Split(';')[0]);
        newItem.IsHide = (bool.Parse(str.Split(';')[1]) ? 1 : 0) != 0;
        this.Duts.Add(newItem);
        this.DG.Items.Add((object) newItem);
        this.RefreshDG();
      }
      this.NeedRefresh = true;
    }

    private void CheckCOMThread()
    {
      while (!this.Exit)
      {
        if (this.StopCheckCOM)
          this.WaitFlag.WaitOne();
        List<ValueItem> availableComPorts = Constants.GetAvailableComPorts();
        lock (this.DutLocker)
        {
          foreach (ValueItem valueItem in availableComPorts)
          {
            ValueItem com = valueItem;
            if (this.Duts.Find((Predicate<Dut_Handler>) (x => x.COM == com.Value)) == null)
            {
              Dut_Handler dut = new Dut_Handler(com.Value, this.Dispatcher);
              this.Duts.Add(dut);
              this.Dispatcher.Invoke((Action) (() => this.DG.Items.Add((object) dut)));
              this.NeedRefresh = true;
            }
          }
          for (int i = 0; i < this.Duts.Count; i++)
          {
            if (availableComPorts.Find((Predicate<ValueItem>) (x => x.Value == this.Duts[i].COM)) == null)
            {
              this.Duts.RemoveAt(i);
              this.Dispatcher.Invoke((Action) (() => this.DG.Items.RemoveAt(i)));
              i--;
              this.NeedRefresh = true;
            }
          }
        }
        if (this.NeedRefresh)
        {
          string strInhalt = string.Empty;
          lock (this.DutLocker)
          {
            for (int index = 0; index < this.Duts.Count; ++index)
            {
              this.Duts[index].Index = (index + 1).ToString();
              if (index > 0)
                strInhalt += "|";
              strInhalt = strInhalt + this.Duts[index].COM + ";" + this.Duts[index].IsHide.ToString();
            }
          }
          PlugInLoader.GmmConfiguration.SetOrUpdateValue("BatchFirmwareUpdate", "ComPorts", strInhalt);
          PlugInLoader.GmmConfiguration.WriteConfigFile();
          this.Dispatcher.Invoke((Action) (() => this.RefreshDG()));
        }
        this.NeedRefresh = false;
        Thread.Sleep(1000);
      }
    }

    private void RefreshDG()
    {
      lock (this.RefreshDGLocker)
        this.DG.Items.Refresh();
    }

    private void StartBtn_Click(object sender, RoutedEventArgs e)
    {
      this.StartBtn.IsEnabled = false;
      this.ReadBtn.IsEnabled = false;
      this.DG.UnselectAll();
      this.DG.IsHitTestVisible = false;
      new Thread(new ThreadStart(this.RunThread))
      {
        IsBackground = true
      }.Start();
    }

    private void ReadThread()
    {
      this.StopCheckCOM = true;
      this.Working = true;
      lock (this.DutLocker)
      {
        foreach (Dut_Handler dut in this.Duts)
          dut.Read();
      }
      this.WaitAll();
      this.StopCheckCOM = false;
      this.WaitFlag.Set();
      this.Dispatcher.Invoke((Action) (() =>
      {
        this.StartBtn.IsEnabled = true;
        this.ReadBtn.IsEnabled = true;
        this.DG.IsHitTestVisible = true;
      }));
      this.Working = false;
    }

    private void RunThread()
    {
      this.StopCheckCOM = true;
      this.Working = true;
      lock (this.DutLocker)
      {
        foreach (Dut_Handler dut in this.Duts)
          dut.Run();
      }
      this.WaitAll();
      this.StopCheckCOM = false;
      this.WaitFlag.Set();
      this.Dispatcher.Invoke((Action) (() =>
      {
        this.StartBtn.IsEnabled = true;
        this.ReadBtn.IsEnabled = true;
        this.DG.IsHitTestVisible = true;
      }));
      this.Working = false;
    }

    private void WaitAll()
    {
      bool flag;
      do
      {
        Thread.Sleep(200);
        flag = true;
        lock (this.DutLocker)
        {
          foreach (Dut_Handler dut in this.Duts)
            flag = flag && !dut.IsRunning;
        }
      }
      while (!flag);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Config File|*.config";
      bool? nullable = openFileDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      lock (this.DutLocker)
      {
        foreach (Dut_Handler dut in this.Duts)
          dut.Initial = false;
      }
      try
      {
        Dut_Handler.SetConfig(openFileDialog.FileName);
      }
      catch
      {
        int num = (int) MessageBox.Show("Config Error");
        this.ReadBtn.IsEnabled = false;
        this.StartBtn.IsEnabled = false;
        return;
      }
      this.FWText.Text = Dut_Handler.Config.FW_FileName;
      this.BLText.Text = Dut_Handler.Config.BL_FileName;
      this.ReadBtn.IsEnabled = true;
      this.StartBtn.IsEnabled = true;
    }

    private void ReadBtn_Click(object sender, RoutedEventArgs e)
    {
      this.StartBtn.IsEnabled = false;
      this.ReadBtn.IsEnabled = false;
      this.DG.UnselectAll();
      this.DG.IsHitTestVisible = false;
      new Thread(new ThreadStart(this.ReadThread))
      {
        IsBackground = true
      }.Start();
    }

    private void HideMenuItem_Click(object sender, RoutedEventArgs e)
    {
      lock (this.DutLocker)
      {
        if (this.DG.SelectedIndex == -1)
          return;
        ((Dut_Handler) this.DG.SelectedItem).IsHide = true;
        this.NeedRefresh = true;
      }
    }

    private void ShowAllMenuItem_Click(object sender, RoutedEventArgs e)
    {
      lock (this.DutLocker)
      {
        foreach (Dut_Handler dut in this.Duts)
          dut.IsHide = false;
        this.NeedRefresh = true;
      }
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      if (this.Working)
        e.Cancel = true;
      else
        this.Exit = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EDCL_Handler;component/ui/batchfirmwareupdatewindow.xaml", UriKind.Relative));
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
          this.FWText = (TextBox) target;
          break;
        case 3:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 4:
          this.StartBtn = (Button) target;
          this.StartBtn.Click += new RoutedEventHandler(this.StartBtn_Click);
          break;
        case 5:
          this.DG = (DataGrid) target;
          break;
        case 6:
          ((MenuItem) target).Click += new RoutedEventHandler(this.HideMenuItem_Click);
          break;
        case 7:
          ((MenuItem) target).Click += new RoutedEventHandler(this.ShowAllMenuItem_Click);
          break;
        case 8:
          this.BLText = (TextBox) target;
          break;
        case 9:
          this.ReadBtn = (Button) target;
          this.ReadBtn.Click += new RoutedEventHandler(this.ReadBtn_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
