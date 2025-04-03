// Decompiled with JetBrains decompiler
// Type: HandlerLib.Memory16BitWindow
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
  public class Memory16BitWindow : Window, IComponentConnector
  {
    private bool isRunning;
    private CancellationTokenSource cancelToken;
    private ProgressHandler progress;
    private Common16BitCommands deviceCommands;
    internal Button ButtonReadMemory16bit;
    internal TextBox TextBoxAddress16bit;
    internal TextBox TextBoxCount16bit;
    internal Button ButtonWriteRam16bit;
    internal TextBox TextBoxMaxBytesPerPacket16bit;
    internal Button ButtonWriteFlash16bit;
    internal Button ButtonEraseFlash16bit;
    internal Button ButtonBackup16bit;
    internal ProgressBar ProgressBar1;
    internal TextBlock TextBlockMessage;
    internal Button ButtonStop;
    internal TextBox TextBlockOutput;
    internal Button ButtonReset16bit;
    private bool _contentLoaded;

    public Memory16BitWindow()
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

    public static void Show(Window owner, Common16BitCommands deviceCommands)
    {
      Memory16BitWindow memory16BitWindow = new Memory16BitWindow();
      memory16BitWindow.deviceCommands = deviceCommands;
      memory16BitWindow.Owner = owner;
      memory16BitWindow.ShowDialog();
    }

    private async void ButtonReadMemory16bit_Click(object sender, RoutedEventArgs e)
    {
      if (this.isRunning)
        return;
      this.isRunning = true;
      this.TextBlockOutput.Text = string.Empty;
      try
      {
        ushort address = Convert.ToUInt16(this.TextBoxAddress16bit.Text, 16);
        uint count = Convert.ToUInt32(this.TextBoxCount16bit.Text);
        byte max = Convert.ToByte(this.TextBoxMaxBytesPerPacket16bit.Text);
        this.cancelToken = new CancellationTokenSource();
        this.progress.Reset();
        byte[] memory = await this.deviceCommands.ReadMemoryAsync(this.progress, this.cancelToken.Token, address, count, max);
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

    private async void ButtonWriteRam16bit_Click(object sender, RoutedEventArgs e)
    {
      if (this.isRunning)
        return;
      this.isRunning = true;
      try
      {
        ushort address = Convert.ToUInt16(this.TextBoxAddress16bit.Text, 16);
        byte max = Convert.ToByte(this.TextBoxMaxBytesPerPacket16bit.Text);
        byte[] buffer = Utility.HexStringToByteArray(this.TextBlockOutput.Text);
        this.cancelToken = new CancellationTokenSource();
        this.progress.Reset();
        await this.deviceCommands.WriteRAMAsync(this.progress, this.cancelToken.Token, address, buffer, max);
        this.TextBlockOutput.Text = string.Empty;
        buffer = (byte[]) null;
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Reset("Canceled");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((Window) this, ex.Message, "Write RAM error", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      finally
      {
        this.isRunning = false;
      }
    }

    private async void ButtonWriteFlash16bit_Click(object sender, RoutedEventArgs e)
    {
      if (this.isRunning)
        return;
      this.isRunning = true;
      try
      {
        ushort address = Convert.ToUInt16(this.TextBoxAddress16bit.Text, 16);
        byte max = Convert.ToByte(this.TextBoxMaxBytesPerPacket16bit.Text);
        byte[] buffer = Utility.HexStringToByteArray(this.TextBlockOutput.Text);
        this.cancelToken = new CancellationTokenSource();
        this.progress.Reset();
        await this.deviceCommands.WriteFLASHAsync(this.progress, this.cancelToken.Token, address, buffer, max);
        this.TextBlockOutput.Text = string.Empty;
        buffer = (byte[]) null;
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Reset("Canceled");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((Window) this, ex.Message, "Write FLASH error", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      finally
      {
        this.isRunning = false;
      }
    }

    private async void ButtonEraseFlash16bit_Click(object sender, RoutedEventArgs e)
    {
      if (this.isRunning)
        return;
      this.isRunning = true;
      try
      {
        ushort address = Convert.ToUInt16(this.TextBoxAddress16bit.Text, 16);
        this.cancelToken = new CancellationTokenSource();
        this.progress.Reset();
        await this.deviceCommands.EraseFLASHAsync(this.progress, this.cancelToken.Token, address);
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Reset("Canceled");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((Window) this, ex.Message, "Erase FLASH error", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      finally
      {
        this.isRunning = false;
      }
    }

    private async void ButtonBackup16bit_Click(object sender, RoutedEventArgs e)
    {
      if (this.isRunning)
        return;
      this.isRunning = true;
      try
      {
        this.cancelToken = new CancellationTokenSource();
        this.progress.Reset();
        await this.deviceCommands.BackupDeviceAsync(this.progress, this.cancelToken.Token);
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

    private async void ButtonReset16bit_Click(object sender, RoutedEventArgs e)
    {
      if (this.isRunning)
        return;
      this.isRunning = true;
      try
      {
        this.cancelToken = new CancellationTokenSource();
        this.progress.Reset();
        await this.deviceCommands.ResetDeviceAsync(this.progress, this.cancelToken.Token);
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
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/memory16bitwindow.xaml", UriKind.Relative));
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
          this.ButtonReadMemory16bit = (Button) target;
          this.ButtonReadMemory16bit.Click += new RoutedEventHandler(this.ButtonReadMemory16bit_Click);
          break;
        case 3:
          this.TextBoxAddress16bit = (TextBox) target;
          break;
        case 4:
          this.TextBoxCount16bit = (TextBox) target;
          break;
        case 5:
          this.ButtonWriteRam16bit = (Button) target;
          this.ButtonWriteRam16bit.Click += new RoutedEventHandler(this.ButtonWriteRam16bit_Click);
          break;
        case 6:
          this.TextBoxMaxBytesPerPacket16bit = (TextBox) target;
          break;
        case 7:
          this.ButtonWriteFlash16bit = (Button) target;
          this.ButtonWriteFlash16bit.Click += new RoutedEventHandler(this.ButtonWriteFlash16bit_Click);
          break;
        case 8:
          this.ButtonEraseFlash16bit = (Button) target;
          this.ButtonEraseFlash16bit.Click += new RoutedEventHandler(this.ButtonEraseFlash16bit_Click);
          break;
        case 9:
          this.ButtonBackup16bit = (Button) target;
          this.ButtonBackup16bit.Click += new RoutedEventHandler(this.ButtonBackup16bit_Click);
          break;
        case 10:
          this.ProgressBar1 = (ProgressBar) target;
          break;
        case 11:
          this.TextBlockMessage = (TextBlock) target;
          break;
        case 12:
          this.ButtonStop = (Button) target;
          this.ButtonStop.Click += new RoutedEventHandler(this.ButtonStop_Click);
          break;
        case 13:
          this.TextBlockOutput = (TextBox) target;
          break;
        case 14:
          this.ButtonReset16bit = (Button) target;
          this.ButtonReset16bit.Click += new RoutedEventHandler(this.ButtonReset16bit_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
