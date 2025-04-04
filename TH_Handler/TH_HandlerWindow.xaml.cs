// Decompiled with JetBrains decompiler
// Type: TH_Handler.TH_HandlerWindow
// Assembly: TH_Handler, Version=1.3.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 02D62764-6653-46F8-9117-1BC5233AD061
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\TH_Handler.dll

using GmmDbLib;
using HandlerLib;
using ReadoutConfiguration;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace TH_Handler
{
  public partial class TH_HandlerWindow : Window, IComponentConnector
  {
    internal string NextPlugin = "";
    internal Menu menuMain;
    internal MenuItem MenuItemComponents;
    internal CheckBox ckbIrDaDovetailSide;
    internal ComboBox txtSerialPort;
    internal Button btnRead;
    internal ProgressBar progress;
    internal Button btnWrite;
    internal TextBox txtOutput;
    internal Button btnConfigurator;
    internal Button btnSave;
    internal Button btnOpen;
    internal Button btnClear;
    internal Button btnCommands;
    private bool _contentLoaded;

    public TH_HandlerFunctions Handler { get; private set; }

    public TH_HandlerWindow(TH_HandlerFunctions handler)
    {
      this.Handler = handler;
      this.InitializeComponent();
      Version version = typeof (TH_HandlerWindow).Assembly.GetName().Version;
      this.Title = string.Format("Temperature and Humidity Sensor v{0}.{1}.{2}", (object) version.Major, (object) version.Minor, (object) version.Build);
      UserInterfaceServices.AddDefaultMenu(this.menuMain.Items[0] as MenuItem, (RoutedEventHandler) ((sender, e) =>
      {
        this.NextPlugin = ((HeaderedItemsControl) sender).Header.ToString();
        this.Close();
      }));
      WpfTranslatorSupport.TranslateWindow(Tg.TH_Hander, (Window) this);
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.Handler.OnProgress += new ValueEventHandler<int>(this.Handler_OnProgress);
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      this.Handler.OnProgress -= new ValueEventHandler<int>(this.Handler_OnProgress);
    }

    private void btnRead_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.Handler.ClearAllData();
        this.UpdateUI();
        this.Handler.ReadDevice();
        this.UpdateUI();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((Window) this, ex.Message, Ot.Gtt(Tg.Handler_UI, "Error", "Error"), MessageBoxButton.OK, MessageBoxImage.Hand);
      }
    }

    private void btnWrite_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.Handler.WriteDevice())
        {
          string caption = Ot.Gtt(Tg.Handler_UI, "Write", "Write");
          int num = (int) MessageBox.Show((Window) this, Ot.Gtt(Tg.Handler_UI, "Successful", "Successful"), caption, MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        else
        {
          string caption = Ot.Gtt(Tg.Handler_UI, "Write", "Write");
          int num = (int) MessageBox.Show((Window) this, Ot.Gtm(Tg.Handler_UI, "CanNotWriteToDevice", "Can not write to device!"), caption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((Window) this, ex.Message, Ot.Gtt(Tg.Handler_UI, "Error", "Error"), MessageBoxButton.OK, MessageBoxImage.Hand);
      }
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.Handler.SaveDevice())
        {
          string caption = Ot.Gtt(Tg.Handler_UI, "SaveToDatabase", "Save to database");
          int num = (int) MessageBox.Show((Window) this, Ot.Gtt(Tg.Handler_UI, "Successful", "Successful"), caption, MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        else
        {
          string caption = Ot.Gtt(Tg.Handler_UI, "SaveToDatabase", "Save to database");
          int num = (int) MessageBox.Show((Window) this, Ot.Gtm(Tg.Handler_UI, "CanNotSaveDataToDatabase", "Can not save data to database!"), caption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((Window) this, ex.Message, Ot.Gtt(Tg.Handler_UI, "Error", "Error"), MessageBoxButton.OK, MessageBoxImage.Hand);
      }
    }

    private void btnOpen_Click(object sender, RoutedEventArgs e)
    {
      byte[] zippedBuffer = BackupWindow.ShowDialog((Window) this, (ICreateMeter) this.Handler, "TH", false);
      if (zippedBuffer == null)
        return;
      this.Handler.OpenDevice(zippedBuffer);
      this.UpdateUI();
    }

    private void btnConfigurator_Click(object sender, RoutedEventArgs e)
    {
      this.Handler.WorkMeter = ConfiguartionWindow.ShowDialog((Window) this, this.Handler.WorkMeter);
      this.UpdateUI();
    }

    private void btnCommands_Click(object sender, RoutedEventArgs e)
    {
      CommandsWindow.ShowDialog((Window) this, this.Handler.CMD);
    }

    private void btnClear_Click(object sender, RoutedEventArgs e)
    {
      this.Handler.ClearAllData();
      this.UpdateUI();
    }

    private void Handler_OnProgress(object sender, int e)
    {
      if (!this.CheckAccess())
        this.Dispatcher.Invoke((Action) (() => this.Handler_OnProgress(sender, e)));
      else
        this.progress.Value = (double) e;
    }

    private void txtSerialPort_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.UpdateReadoutConfiguration();
    }

    private void ckbIrDaDovetailSide_CheckedUnchecked(object sender, RoutedEventArgs e)
    {
      this.UpdateReadoutConfiguration();
    }

    private void UpdateReadoutConfiguration()
    {
      ConnectionProfile connectionProfile = ReadoutConfigFunctions.Manager.GetConnectionProfile(183);
      if (connectionProfile == null)
      {
        string caption = Ot.Gtt(Tg.Handler_UI, "Error", "Error");
        int num = (int) MessageBox.Show((Window) this, Ot.Gtm(Tg.Handler_UI, "MissingTHCommunicationProfile", "Can not find the connection profile for humidity sensor! Please update you database."), caption, MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      else
      {
        try
        {
          ConfigList readoutConfiguration = this.Handler.GetReadoutConfiguration();
          readoutConfiguration.Reset(connectionProfile.GetSettingsList());
          readoutConfiguration.Port = this.txtSerialPort.SelectedValue.ToString();
          if (this.ckbIrDaDovetailSide.IsChecked.Value)
            readoutConfiguration.IrDaSelection = "DoveTailSide";
          this.Handler.SetReadoutConfiguration(readoutConfiguration);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show((Window) this, ex.Message, Ot.Gtt(Tg.Handler_UI, "Error", "Error"), MessageBoxButton.OK, MessageBoxImage.Hand);
        }
      }
    }

    private void UpdateUI()
    {
      if (this.Handler.WorkMeter != null)
        this.txtOutput.Text = this.Handler.WorkMeter.ToString();
      else
        this.txtOutput.Text = string.Empty;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/TH_Handler;component/th_handlerwindow.xaml", UriKind.Relative));
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
          ((Window) target).Closing += new CancelEventHandler(this.Window_Closing);
          break;
        case 2:
          this.menuMain = (Menu) target;
          break;
        case 3:
          this.MenuItemComponents = (MenuItem) target;
          break;
        case 4:
          this.ckbIrDaDovetailSide = (CheckBox) target;
          this.ckbIrDaDovetailSide.Unchecked += new RoutedEventHandler(this.ckbIrDaDovetailSide_CheckedUnchecked);
          this.ckbIrDaDovetailSide.Checked += new RoutedEventHandler(this.ckbIrDaDovetailSide_CheckedUnchecked);
          break;
        case 5:
          this.txtSerialPort = (ComboBox) target;
          this.txtSerialPort.SelectionChanged += new SelectionChangedEventHandler(this.txtSerialPort_SelectionChanged);
          break;
        case 6:
          this.btnRead = (Button) target;
          this.btnRead.Click += new RoutedEventHandler(this.btnRead_Click);
          break;
        case 7:
          this.progress = (ProgressBar) target;
          break;
        case 8:
          this.btnWrite = (Button) target;
          this.btnWrite.Click += new RoutedEventHandler(this.btnWrite_Click);
          break;
        case 9:
          this.txtOutput = (TextBox) target;
          break;
        case 10:
          this.btnConfigurator = (Button) target;
          this.btnConfigurator.Click += new RoutedEventHandler(this.btnConfigurator_Click);
          break;
        case 11:
          this.btnSave = (Button) target;
          this.btnSave.Click += new RoutedEventHandler(this.btnSave_Click);
          break;
        case 12:
          this.btnOpen = (Button) target;
          this.btnOpen.Click += new RoutedEventHandler(this.btnOpen_Click);
          break;
        case 13:
          this.btnClear = (Button) target;
          this.btnClear.Click += new RoutedEventHandler(this.btnClear_Click);
          break;
        case 14:
          this.btnCommands = (Button) target;
          this.btnCommands.Click += new RoutedEventHandler(this.btnCommands_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
