// Decompiled with JetBrains decompiler
// Type: HandlerLib.VersionWindow
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
  public class VersionWindow : Window, IComponentConnector
  {
    private bool isRunning;
    private CancellationTokenSource cancelToken;
    private ProgressHandler progress;
    private DeviceCommandsMBus deviceCommands;
    internal Button ButtonReadVersion;
    internal Button ButtonReadVersionAsync;
    internal Button ButtonReadVersionAsyncLoop;
    internal Button ButtonStop;
    internal Label LabelLoopInfo;
    internal ProgressBar ProgressBar1;
    internal TextBlock TextBlockMessage;
    internal TextBox TextBlockOutput;
    private bool _contentLoaded;

    public VersionWindow()
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

    public static void Show(Window owner, DeviceCommandsMBus deviceCommands)
    {
      VersionWindow versionWindow = new VersionWindow();
      versionWindow.deviceCommands = deviceCommands;
      versionWindow.Owner = owner;
      versionWindow.ShowDialog();
    }

    private void ButtonReadVersion_Click(object sender, RoutedEventArgs e)
    {
      this.TextBlockOutput.Text = string.Empty;
      try
      {
        this.cancelToken = new CancellationTokenSource();
        this.progress.Reset();
        this.TextBlockOutput.Text = this.deviceCommands.ReadVersion(this.progress, this.cancelToken.Token).Print(0);
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Reset("Canceled");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((Window) this, ex.Message, "Read version error", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
    }

    private async void ButtonReadVersionAsync_Click(object sender, RoutedEventArgs e)
    {
      if (this.isRunning)
        return;
      this.isRunning = true;
      this.TextBlockOutput.Text = string.Empty;
      try
      {
        this.cancelToken = new CancellationTokenSource();
        this.progress.Reset();
        DeviceVersionMBus version = await this.deviceCommands.ReadVersionAsync(this.progress, this.cancelToken.Token);
        this.TextBlockOutput.Text = version.Print(0);
        version = (DeviceVersionMBus) null;
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Reset("Canceled");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((Window) this, ex.Message, "Read version error", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      finally
      {
        this.isRunning = false;
      }
    }

    private async void ButtonReadVersionAsyncLoop_Click(object sender, RoutedEventArgs e)
    {
      if (this.isRunning)
        return;
      this.TextBlockOutput.Text = string.Empty;
      this.isRunning = true;
      this.cancelToken = new CancellationTokenSource();
      int count = 0;
      int successful = 0;
      int failed = 0;
      while (!this.cancelToken.IsCancellationRequested)
      {
        if (count % 100 == 0)
          this.progress.Reset(100);
        try
        {
          DeviceVersionMBus version = await this.deviceCommands.ReadVersionAsync(this.progress, this.cancelToken.Token);
          this.TextBlockOutput.Text = version.Print(0);
          ++successful;
          version = (DeviceVersionMBus) null;
        }
        catch (OperationCanceledException ex)
        {
          this.progress.Reset("Canceled");
          break;
        }
        catch
        {
          ++failed;
        }
        finally
        {
          this.isRunning = false;
        }
        this.LabelLoopInfo.Content = (object) string.Format("Count: {0}, Successful: {1}, Failed: {2}", (object) ++count, (object) successful, (object) failed);
      }
    }

    private void ButtonStop_Click(object sender, RoutedEventArgs e)
    {
      if (this.cancelToken == null)
        return;
      this.cancelToken.Cancel();
      this.isRunning = false;
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      if (this.cancelToken == null)
        return;
      this.cancelToken.Cancel();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/versionwindow.xaml", UriKind.Relative));
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
          this.ButtonReadVersion = (Button) target;
          this.ButtonReadVersion.Click += new RoutedEventHandler(this.ButtonReadVersion_Click);
          break;
        case 3:
          this.ButtonReadVersionAsync = (Button) target;
          this.ButtonReadVersionAsync.Click += new RoutedEventHandler(this.ButtonReadVersionAsync_Click);
          break;
        case 4:
          this.ButtonReadVersionAsyncLoop = (Button) target;
          this.ButtonReadVersionAsyncLoop.Click += new RoutedEventHandler(this.ButtonReadVersionAsyncLoop_Click);
          break;
        case 5:
          this.ButtonStop = (Button) target;
          this.ButtonStop.Click += new RoutedEventHandler(this.ButtonStop_Click);
          break;
        case 6:
          this.LabelLoopInfo = (Label) target;
          break;
        case 7:
          this.ProgressBar1 = (ProgressBar) target;
          break;
        case 8:
          this.TextBlockMessage = (TextBlock) target;
          break;
        case 9:
          this.TextBlockOutput = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
