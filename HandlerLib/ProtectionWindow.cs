// Decompiled with JetBrains decompiler
// Type: HandlerLib.ProtectionWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class ProtectionWindow : Window, IComponentConnector
  {
    private bool isWriteProtection;
    private CancellationTokenSource cancelToken;
    private ProgressHandler progress;
    private DeviceCommandsMBus deviceCommands;
    private Common32BitCommands deviceCommonCMD;
    private Cursor defaultCursor;
    internal TextBox TextProtectionKey;
    internal Button ButtonSetProtectionKey;
    internal Button ButtonCheckProtectionKey;
    internal TextBox TextSeriesKey;
    internal Button ButtonSetSeriesKey;
    internal Button ButtonDelSeriesKey;
    internal Button ButtonSaveSeriesKey;
    internal TextBlock TextBlock_Status;
    internal Button ButtonCancel;
    internal Button ButtonOpenWriteProtectionTemporarily;
    private bool _contentLoaded;

    public ProtectionWindow()
    {
      this.InitializeComponent();
      this.isWriteProtection = false;
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
    }

    private void StartUp()
    {
      if (this.deviceCommands == null || this.deviceCommands.SeriesKey <= 0U)
        return;
      this.TextSeriesKey.Text = "0x" + this.deviceCommands.SeriesKey.ToString("x4");
    }

    private void SetRunState()
    {
      this.cancelToken = new CancellationTokenSource();
      this.ButtonCheckProtectionKey.IsEnabled = false;
      this.ButtonSetProtectionKey.IsEnabled = false;
      this.ButtonDelSeriesKey.IsEnabled = false;
      this.ButtonSaveSeriesKey.IsEnabled = false;
      this.ButtonSetSeriesKey.IsEnabled = false;
      this.ButtonSetProtectionKey.IsEnabled = false;
      this.ButtonCancel.IsEnabled = true;
      this.defaultCursor = this.Cursor;
      this.Cursor = Cursors.Wait;
    }

    private void SetStopState()
    {
      this.ButtonCheckProtectionKey.IsEnabled = true;
      this.ButtonSetProtectionKey.IsEnabled = true;
      this.ButtonDelSeriesKey.IsEnabled = true;
      this.ButtonSaveSeriesKey.IsEnabled = true;
      this.ButtonSetSeriesKey.IsEnabled = true;
      this.ButtonSetProtectionKey.IsEnabled = true;
      this.ButtonCancel.IsEnabled = false;
      this.Cursor = this.defaultCursor;
    }

    private void OnProgress(ProgressArg obj)
    {
      if (this.CheckAccess())
        return;
      this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
    }

    public static void Show(
      Window owner,
      DeviceCommandsMBus deviceCommands,
      Common32BitCommands commonCMDs)
    {
      ProtectionWindow protectionWindow1 = new ProtectionWindow();
      protectionWindow1.deviceCommands = deviceCommands;
      protectionWindow1.Owner = owner;
      protectionWindow1.deviceCommonCMD = commonCMDs;
      ProtectionWindow protectionWindow2 = protectionWindow1;
      protectionWindow2.StartUp();
      protectionWindow2.ShowDialog();
    }

    private async void SetWriteProtectionAsync_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        this.progress.Reset();
        this.TextBlock_Status.Text = "Status:";
        string text = this.TextProtectionKey.Text;
        byte[] data = text.Length == 8 ? Util.HexStringToByteArray(text.Replace("0x", "")) : throw new Exception("Key not in a valid format. (4 Bytes)");
        await this.deviceCommonCMD.SetWriteProtectionAsync(data, this.progress, this.cancelToken.Token);
        this.TextBlock_Status.Text = "Status: Write protection key successfully set.";
        text = (string) null;
        data = (byte[]) null;
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Reset("Canceled");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((Window) this, ex.Message, "SetWriteProtection error", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void CheckWriteProtectionAsync_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        this.TextBlock_Status.Text = "Status:";
        this.progress.Reset();
        byte[] key = new byte[4];
        await this.deviceCommonCMD.OpenWriteProtectionTemporarilyAsync(key, this.progress, this.cancelToken.Token);
        this.isWriteProtection = true;
        this.TextBlock_Status.Text = "Status: Write protection key is set.";
        key = (byte[]) null;
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Reset("Canceled");
      }
      catch (Exception ex)
      {
        this.TextBlock_Status.Text = "Status: " + ex.Message;
        this.isWriteProtection = false;
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void OpenWriteProtectionAsync_Click(object sender, RoutedEventArgs e)
    {
      bool oldWRProtection = this.isWriteProtection;
      try
      {
        this.SetRunState();
        this.TextBlock_Status.Text = "Status:";
        this.progress.Reset();
        string text = this.TextProtectionKey.Text;
        byte[] key = text.Length == 8 ? Util.HexStringToByteArray(text.Replace("0x", "")) : throw new Exception("Key not in a valid format. (4 Bytes)");
        await this.deviceCommonCMD.OpenWriteProtectionTemporarilyAsync(key, this.progress, this.cancelToken.Token);
        this.isWriteProtection = false;
        this.TextBlock_Status.Text = "Status: Write protection temporarily disabled !!!";
        text = (string) null;
        key = (byte[]) null;
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Reset("Canceled");
      }
      catch (Exception ex)
      {
        this.TextBlock_Status.Text = "Status: " + ex.Message;
        this.isWriteProtection = oldWRProtection;
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void SaveSeriesKeyAsync_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        this.TextBlock_Status.Text = "Status:";
        this.progress.Reset();
        string text = this.TextSeriesKey.Text;
        if (text.Length != 8 && text.Length != 10)
          throw new Exception("Key not in a valid format. (4 Bytes)");
        uint uintkey = 0;
        uintkey = !text.Substring(0, 2).Equals("0x") ? uint.Parse(text) : uint.Parse(text.Substring(2), NumberStyles.HexNumber);
        await Task.Run((Action) (() => this.deviceCommands.SaveSeriesKey(uintkey)));
        this.TextBlock_Status.Text = "Status: Series Key saved.";
        text = (string) null;
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Reset("Canceled");
      }
      catch (Exception ex)
      {
        this.TextBlock_Status.Text = "Status: " + ex.Message;
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void UseSeriesKeyAsync_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        this.TextBlock_Status.Text = "Status:";
        this.progress.Reset();
        string text = this.TextSeriesKey.Text;
        if (text.Length != 8 && text.Length != 10)
          throw new Exception("Key not in a valid format. (4 Bytes)");
        uint uintkey = 0;
        uintkey = !text.Substring(0, 2).Equals("0x") ? uint.Parse(text) : uint.Parse(text.Substring(2), NumberStyles.HexNumber);
        await Task.Run((Action) (() => this.deviceCommands.SetProtectedIdentification(uintkey)));
        this.TextBlock_Status.Text = "Status: Series Key is in usage now.";
        text = (string) null;
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Reset("Canceled");
      }
      catch (Exception ex)
      {
        this.TextBlock_Status.Text = "Status: " + ex.Message;
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void DeleteSeriesKeyAsync_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        this.TextBlock_Status.Text = "Status:";
        this.progress.Reset();
        await Task.Run((Action) (() => this.deviceCommands.ClearProtectedIdentification()));
        this.TextBlock_Status.Text = "Status: Series Key deleted. ProtectionIdentification cleared.";
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Reset("Canceled");
      }
      catch (Exception ex)
      {
        this.TextBlock_Status.Text = "Status: " + ex.Message;
      }
      finally
      {
        this.SetStopState();
      }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.TextBlock_Status.Text = "Status:";
        this.cancelToken.Cancel();
        this.TextBlock_Status.Text = "Status: operation cancelled by user.";
        this.SetStopState();
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Reset("Canceled");
      }
      catch (Exception ex)
      {
        this.TextBlock_Status.Text = "Status: " + ex.Message;
      }
      finally
      {
        this.SetStopState();
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/protectionwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.TextProtectionKey = (TextBox) target;
          break;
        case 2:
          this.ButtonSetProtectionKey = (Button) target;
          this.ButtonSetProtectionKey.Click += new RoutedEventHandler(this.SetWriteProtectionAsync_Click);
          break;
        case 3:
          this.ButtonCheckProtectionKey = (Button) target;
          this.ButtonCheckProtectionKey.Click += new RoutedEventHandler(this.CheckWriteProtectionAsync_Click);
          break;
        case 4:
          this.TextSeriesKey = (TextBox) target;
          break;
        case 5:
          this.ButtonSetSeriesKey = (Button) target;
          this.ButtonSetSeriesKey.Click += new RoutedEventHandler(this.UseSeriesKeyAsync_Click);
          break;
        case 6:
          this.ButtonDelSeriesKey = (Button) target;
          this.ButtonDelSeriesKey.Click += new RoutedEventHandler(this.DeleteSeriesKeyAsync_Click);
          break;
        case 7:
          this.ButtonSaveSeriesKey = (Button) target;
          this.ButtonSaveSeriesKey.Click += new RoutedEventHandler(this.SaveSeriesKeyAsync_Click);
          break;
        case 8:
          this.TextBlock_Status = (TextBlock) target;
          break;
        case 9:
          this.ButtonCancel = (Button) target;
          this.ButtonCancel.Click += new RoutedEventHandler(this.SaveSeriesKeyAsync_Click);
          break;
        case 10:
          this.ButtonOpenWriteProtectionTemporarily = (Button) target;
          this.ButtonOpenWriteProtectionTemporarily.Click += new RoutedEventHandler(this.CheckWriteProtectionAsync_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
