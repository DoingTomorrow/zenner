// Decompiled with JetBrains decompiler
// Type: HandlerLib.Memory32BitWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

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
namespace HandlerLib
{
  public class Memory32BitWindow : Window, IComponentConnector
  {
    private bool isRunning;
    private CancellationTokenSource cancelToken;
    private ProgressHandler progress;
    private Common32BitCommands deviceCMD;
    internal TextBox TextBoxAddress16bit;
    internal TextBox TextBoxCount16bit;
    internal TextBox TextBoxMaxBytesPerPacket16bit;
    internal ProgressBar ProgressBar1;
    internal TextBlock TextBlockMessage;
    internal Button ButtonStop;
    internal TextBox TextBlockOutput;
    internal Button ButtonReadMemory;
    internal Button ButtonWriteMemory;
    internal Button ButtonBackup;
    internal Button ButtonReset;
    private bool _contentLoaded;

    public Memory32BitWindow()
    {
      this.InitializeComponent();
      this.isRunning = false;
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
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
        this.TextBlockMessage.Text = obj.Message;
      }
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      if (this.cancelToken == null)
        return;
      this.cancelToken.Cancel();
    }

    public static void Show(Window owner, Common32BitCommands deviceCMD)
    {
      Memory32BitWindow memory32BitWindow = new Memory32BitWindow();
      memory32BitWindow.deviceCMD = deviceCMD;
      memory32BitWindow.Owner = owner;
      memory32BitWindow.ShowDialog();
    }

    private async void ButtonReadMemory_Click(object sender, RoutedEventArgs e)
    {
      if (this.isRunning)
        return;
      this.isRunning = true;
      this.TextBlockOutput.Text = string.Empty;
      try
      {
        uint address = Convert.ToUInt32(this.TextBoxAddress16bit.Text, 16);
        uint count = Convert.ToUInt32(this.TextBoxCount16bit.Text);
        byte max = Convert.ToByte(this.TextBoxMaxBytesPerPacket16bit.Text);
        this.cancelToken = new CancellationTokenSource();
        this.progress.Reset();
        byte[] memory = await this.deviceCMD.ReadMemoryAsync(this.progress, this.cancelToken.Token, address, count, max);
        this.TextBlockOutput.Text = Utility.ByteArrayToHexString(memory);
        memory = (byte[]) null;
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Reset("Canceled");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((Window) this, ex.Message, "Read memory error", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      finally
      {
        this.isRunning = false;
      }
    }

    private async void ButtonWriteMemory_Click(object sender, RoutedEventArgs e)
    {
      if (this.isRunning)
        return;
      this.isRunning = true;
      try
      {
        uint address = Convert.ToUInt32(this.TextBoxAddress16bit.Text, 16);
        byte max = Convert.ToByte(this.TextBoxMaxBytesPerPacket16bit.Text);
        byte[] buffer = Utility.HexStringToByteArray(this.TextBlockOutput.Text);
        this.cancelToken = new CancellationTokenSource();
        this.progress.Reset();
        await this.deviceCMD.WriteMemoryAsync(this.progress, this.cancelToken.Token, address, buffer, max);
        this.TextBlockOutput.Text = string.Empty;
        buffer = (byte[]) null;
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Reset("Canceled");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((Window) this, ex.Message, "Write Memory error", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      finally
      {
        this.isRunning = false;
      }
    }

    private async void ButtonBackup_Click(object sender, RoutedEventArgs e)
    {
      if (this.isRunning)
        return;
      this.isRunning = true;
      try
      {
        this.cancelToken = new CancellationTokenSource();
        this.progress.Reset();
        await this.deviceCMD.BackupDeviceAsync(this.progress, this.cancelToken.Token);
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Reset("Canceled");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((Window) this, ex.Message, "Backup error", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      finally
      {
        this.isRunning = false;
      }
    }

    private async void ButtonReset_Click(object sender, RoutedEventArgs e)
    {
      if (this.isRunning)
        return;
      this.isRunning = true;
      try
      {
        this.cancelToken = new CancellationTokenSource();
        this.progress.Reset();
        await this.deviceCMD.ResetDeviceAsync(this.progress, this.cancelToken.Token);
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Reset("Canceled");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((Window) this, ex.Message, "Reset error", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      finally
      {
        this.isRunning = false;
      }
    }

    private void ButtonStop_Click(object sender, RoutedEventArgs e)
    {
      if (this.cancelToken == null)
        return;
      this.cancelToken.Cancel();
      this.isRunning = false;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/memory32bitwindow.xaml", UriKind.Relative));
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
          this.TextBoxAddress16bit = (TextBox) target;
          break;
        case 3:
          this.TextBoxCount16bit = (TextBox) target;
          break;
        case 4:
          this.TextBoxMaxBytesPerPacket16bit = (TextBox) target;
          break;
        case 5:
          this.ProgressBar1 = (ProgressBar) target;
          break;
        case 6:
          this.TextBlockMessage = (TextBlock) target;
          break;
        case 7:
          this.ButtonStop = (Button) target;
          this.ButtonStop.Click += new RoutedEventHandler(this.ButtonStop_Click);
          break;
        case 8:
          this.TextBlockOutput = (TextBox) target;
          break;
        case 9:
          this.ButtonReadMemory = (Button) target;
          this.ButtonReadMemory.Click += new RoutedEventHandler(this.ButtonReadMemory_Click);
          break;
        case 10:
          this.ButtonWriteMemory = (Button) target;
          this.ButtonWriteMemory.Click += new RoutedEventHandler(this.ButtonWriteMemory_Click);
          break;
        case 11:
          this.ButtonBackup = (Button) target;
          this.ButtonBackup.Click += new RoutedEventHandler(this.ButtonBackup_Click);
          break;
        case 12:
          this.ButtonReset = (Button) target;
          this.ButtonReset.Click += new RoutedEventHandler(this.ButtonReset_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
