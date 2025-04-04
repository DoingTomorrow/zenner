// Decompiled with JetBrains decompiler
// Type: PDCL2_Handler.UI.EncabulatorWindow
// Assembly: PDCL2_Handler, Version=2.22.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 03BA4C2D-69FE-4DA6-9C3F-B3D5471C4058
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDCL2_Handler.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace PDCL2_Handler.UI
{
  public partial class EncabulatorWindow : Window, IComponentConnector
  {
    private PDCL2_HandlerFunctions myFunctions;
    private CancellationTokenSource tokenSource;
    private string fileName;
    internal Button ButtonStart;
    internal TextBox TextBoxOutput;
    internal Button ButtonStop;
    internal Button ButtonShowFile;
    private bool _contentLoaded;

    public EncabulatorWindow() => this.InitializeComponent();

    public EncabulatorWindow(PDCL2_HandlerFunctions myFunctions)
      : this()
    {
      this.myFunctions = myFunctions;
      this.myFunctions.OnEncabulatorDataReceived += new EventHandler<EncabulatorData>(this.myFunctions_OnEncabulatorDataReceived);
      this.fileName = Path.GetTempPath() + Guid.NewGuid().ToString() + ".txt";
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      this.myFunctions.OnEncabulatorDataReceived -= new EventHandler<EncabulatorData>(this.myFunctions_OnEncabulatorDataReceived);
    }

    private void myFunctions_OnEncabulatorDataReceived(object sender, EncabulatorData frame)
    {
      if (!this.CheckAccess())
      {
        this.Dispatcher.Invoke((Action) (() => this.myFunctions_OnEncabulatorDataReceived(sender, frame)));
      }
      else
      {
        string str = frame.ToString() + Environment.NewLine;
        File.AppendAllText(this.fileName, str);
        this.TextBoxOutput.AppendText(str);
        this.TextBoxOutput.ScrollToEnd();
      }
    }

    private async void ButtonStart_Click(object sender, RoutedEventArgs e)
    {
      this.TextBoxOutput.Text = string.Empty;
      try
      {
        this.tokenSource = new CancellationTokenSource();
        await this.myFunctions.StartVolumeMonitorAsync(this.tokenSource.Token);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void ButtonStop_Click(object sender, RoutedEventArgs e)
    {
      if (this.tokenSource == null)
        return;
      this.tokenSource.Cancel();
    }

    private void ButtonShowFile_Click(object sender, RoutedEventArgs e)
    {
      Process.Start(this.fileName);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/PDCL2_Handler;component/ui/encabulatorwindow.xaml", UriKind.Relative));
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
          this.ButtonStart = (Button) target;
          this.ButtonStart.Click += new RoutedEventHandler(this.ButtonStart_Click);
          break;
        case 3:
          this.TextBoxOutput = (TextBox) target;
          break;
        case 4:
          this.ButtonStop = (Button) target;
          this.ButtonStop.Click += new RoutedEventHandler(this.ButtonStop_Click);
          break;
        case 5:
          this.ButtonShowFile = (Button) target;
          this.ButtonShowFile.Click += new RoutedEventHandler(this.ButtonShowFile_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
