// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_ModuleHandlerWindow
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using HandlerLib;
using HandlerLib.View;
using S4_Handler.Functions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
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
  public partial class S4_ModuleHandlerWindow : Window, IComponentConnector
  {
    private S4_HandlerFunctions HandlerFunction;
    private BusModuleInfo ModuleInfo;
    private S4_ModuleCommands ModuleCommands;
    internal S4_ModuleObjects SelectedModule;
    private CancellationTokenSource cancelTokenSource;
    private ProgressHandler progress;
    private const ReadPartsSelection supportedSelections = ReadPartsSelection.RangesMask;
    private ReadPartsSelection ReadSelection;
    internal DockPanel DockPanelButtons;
    internal StackPanel StackPanelBottomButtoms;
    internal Button ButtomBreak;
    internal StackPanel StackPanelTopButtoms;
    internal Button ButtonShowModuleCommands;
    internal GroupBox GroupBoxManagedMap;
    internal Button ButtonShowParameter;
    internal Button ButtonShowMemory;
    internal GroupBox GroupBoxOutputModule;
    internal TextBox TextBoxImpulseValue;
    internal Button ButtonReadImpulseValue;
    internal Button ButtonWriteImpulseValue;
    internal Grid GridGraphics;
    internal Button ButtonConnect;
    internal Button ButtonRead;
    internal Button ButtonReadSelection;
    internal TextBox TextBoxConnected;
    private bool _contentLoaded;

    public S4_ModuleHandlerWindow(S4_HandlerFunctions myFunctions, BusModuleInfo moduleInfo)
    {
      this.HandlerFunction = myFunctions;
      this.ModuleInfo = moduleInfo;
      this.ModuleCommands = new S4_ModuleCommands(myFunctions.checkedCommands, moduleInfo);
      if (this.HandlerFunction.myModules == null)
        this.HandlerFunction.myModules = new S4_Modules(this.ModuleCommands);
      this.SelectedModule = this.HandlerFunction.myModules.SelectModule(moduleInfo);
      this.InitializeComponent();
      this.TextBoxConnected.Text = moduleInfo.GetModuleInfoText();
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.Title = this.Title + "  " + this.ModuleInfo.BusModuleType.ToString() + ": " + this.ModuleInfo.BusModuleSerialNumber.ToString();
      if (this.ModuleInfo.BusModuleType != BusModuleTypes.OUT2_VMCP)
        return;
      this.GroupBoxOutputModule.Visibility = Visibility.Visible;
    }

    private void OnProgress(ProgressArg obj)
    {
      if (this.CheckAccess())
        return;
      this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
    }

    private void ButtonShowModuleCommands_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        CommandWindowBusModule commandWindowBusModule = new CommandWindowBusModule(this.HandlerFunction.checkedNfcCommands, this.ModuleInfo);
        commandWindowBusModule.Owner = (Window) this;
        commandWindowBusModule.ShowDialog();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private async void ButtonConnect_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        await this.SelectedModule.ConnectModule(this.progress, this.cancelTokenSource.Token);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private async void ButtonRead_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        await this.SelectedModule.ReadModule(this.progress, this.cancelTokenSource.Token, this.ReadSelection);
        this.ButtonShowMemory.IsEnabled = true;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonShowMemory_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        MemoryViewer memoryViewer = new MemoryViewer((DeviceMemory) this.SelectedModule.ConnectedObject.ModuleMemory, (BaseMemoryAccess) this.ModuleCommands);
        memoryViewer.Owner = (Window) this;
        memoryViewer.ShowDialog();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonReadSelection_Click(object sender, RoutedEventArgs e)
    {
      DeviceReadRangeSelection.DefineReadPartsSelections(ReadPartsSelection.RangesMask, ref this.ReadSelection, (Window) this);
    }

    private async void ButtonReadImpulseValue_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        float ImpulseValue = await this.SelectedModule.ReadImpulseValue(this.progress, this.cancelTokenSource.Token);
        this.TextBoxImpulseValue.Text = ImpulseValue.ToString("0.000");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private async void ButtonWriteImpulseValue_Click(object sender, RoutedEventArgs e)
    {
      await this.WriteImpulseValueAsync();
    }

    private async Task WriteImpulseValueAsync()
    {
      try
      {
        float ImpulseValue = float.Parse(this.TextBoxImpulseValue.Text);
        await this.SelectedModule.WriteImpulseValue(this.progress, this.cancelTokenSource.Token, ImpulseValue);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private async void TextBoxImpulseValue_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      e.Handled = true;
      await this.WriteImpulseValueAsync();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_modulehandlerwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.DockPanelButtons = (DockPanel) target;
          break;
        case 2:
          this.StackPanelBottomButtoms = (StackPanel) target;
          break;
        case 3:
          this.ButtomBreak = (Button) target;
          break;
        case 4:
          this.StackPanelTopButtoms = (StackPanel) target;
          break;
        case 5:
          this.ButtonShowModuleCommands = (Button) target;
          this.ButtonShowModuleCommands.Click += new RoutedEventHandler(this.ButtonShowModuleCommands_Click);
          break;
        case 6:
          this.GroupBoxManagedMap = (GroupBox) target;
          break;
        case 7:
          this.ButtonShowParameter = (Button) target;
          break;
        case 8:
          this.ButtonShowMemory = (Button) target;
          this.ButtonShowMemory.Click += new RoutedEventHandler(this.ButtonShowMemory_Click);
          break;
        case 9:
          this.GroupBoxOutputModule = (GroupBox) target;
          break;
        case 10:
          this.TextBoxImpulseValue = (TextBox) target;
          this.TextBoxImpulseValue.KeyDown += new KeyEventHandler(this.TextBoxImpulseValue_KeyDown);
          break;
        case 11:
          this.ButtonReadImpulseValue = (Button) target;
          this.ButtonReadImpulseValue.Click += new RoutedEventHandler(this.ButtonReadImpulseValue_Click);
          break;
        case 12:
          this.ButtonWriteImpulseValue = (Button) target;
          this.ButtonWriteImpulseValue.Click += new RoutedEventHandler(this.ButtonWriteImpulseValue_Click);
          break;
        case 13:
          this.GridGraphics = (Grid) target;
          break;
        case 14:
          this.ButtonConnect = (Button) target;
          this.ButtonConnect.Click += new RoutedEventHandler(this.ButtonConnect_Click);
          break;
        case 15:
          this.ButtonRead = (Button) target;
          this.ButtonRead.Click += new RoutedEventHandler(this.ButtonRead_Click);
          break;
        case 16:
          this.ButtonReadSelection = (Button) target;
          this.ButtonReadSelection.Click += new RoutedEventHandler(this.ButtonReadSelection_Click);
          break;
        case 17:
          this.TextBoxConnected = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
