// Decompiled with JetBrains decompiler
// Type: HandlerLib.TestCommunicationWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommunicationPort.UserInterface;
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
using System.Windows.Media;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class TestCommunicationWindow : Window, IComponentConnector
  {
    private bool isRunning;
    private bool isInit;
    private CancellationTokenSource cancelToken;
    private ProgressHandler progress;
    public BaseMemoryAccess DeviceCommands;
    public CommunicationPortWindowFunctions myPort;
    internal ConfigList usedConfigList;
    private Brush DefaultBackground;
    private Brush ChangedBackground;
    internal TextBlock TextBlockStatus;
    internal ProgressBar ProgressBar1;
    internal TextBox TextBlockResult;
    internal WrapPanel PanalButtons;
    internal TextBox TextBoxCyles;
    internal TextBox TextBoxCycleSuccessful;
    internal TextBox TextBoxCycleFailed;
    internal WrapPanel PanalButtons2;
    internal TextBox TextBoxCycleStopAfterErrors;
    internal TextBox TextBoxCycleTime;
    internal WrapPanel PanalButtons3;
    internal CheckBox CheckBoxHaltOnFirst;
    internal CheckBox CheckBoxRunCyle;
    internal CheckBox CheckBoxInterruptConnection;
    internal StackPanel StackPanalButtons2;
    internal StackPanel StackPanalButton3;
    internal Button ButtonRunCommand;
    internal Button ButtonBreak;
    private bool _contentLoaded;

    public TestCommunicationWindow()
    {
      this.InitializeComponent();
      this.isRunning = false;
      this.isInit = true;
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.TextBoxCyles.Text = "0";
      this.TextBoxCycleFailed.Text = "0";
      this.TextBoxCycleSuccessful.Text = "0";
      this.TextBoxCycleStopAfterErrors.Text = "3";
      this.TextBoxCycleTime.Text = "200";
      this.DefaultBackground = this.TextBoxCycleTime.Background;
      this.ChangedBackground = (Brush) Brushes.Yellow;
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
        this.TextBlockStatus.Text = obj.Message;
      }
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      if (this.cancelToken == null)
        return;
      this.cancelToken.Cancel();
    }

    public static void Show(CommunicationPortWindowFunctions port, BaseMemoryAccess deviceCommands)
    {
      TestCommunicationWindow communicationWindow = new TestCommunicationWindow()
      {
        DeviceCommands = deviceCommands,
        myPort = port
      };
      communicationWindow.usedConfigList = port.GetReadoutConfiguration();
      communicationWindow.TextBoxCycleTime.Text = communicationWindow.usedConfigList.CycleTime.ToString();
      communicationWindow.isInit = false;
      communicationWindow.ShowDialog();
    }

    private void ButtonBreak_Click(object sender, RoutedEventArgs e)
    {
      if (this.cancelToken == null)
        return;
      this.cancelToken.Cancel();
      this.isRunning = false;
    }

    private async void ButtonRunCommand_Click(object sender, RoutedEventArgs e)
    {
      this.ButtonRunCommand.IsEnabled = false;
      this.TextBoxCycleStopAfterErrors.IsReadOnly = true;
      this.CheckBoxHaltOnFirst.IsEnabled = false;
      await this.RunCommand();
      this.ButtonRunCommand.IsEnabled = true;
      this.TextBoxCycleStopAfterErrors.IsReadOnly = false;
      this.CheckBoxHaltOnFirst.IsEnabled = true;
    }

    private async Task RunCommand()
    {
      if (this.isRunning)
        return;
      this.TextBlockResult.Text = string.Empty;
      this.isRunning = true;
      this.cancelToken = new CancellationTokenSource();
      int count = 0;
      int successful = 0;
      int failed = 0;
      int maxFails = int.Parse(this.TextBoxCycleStopAfterErrors.Text);
      this.SetValuesForProgress(0, 0, 0);
      DateTime startTime = DateTime.Now;
      while (!this.cancelToken.IsCancellationRequested)
      {
        if (count % 100 == 0)
          this.progress.Reset(100);
        try
        {
          if (maxFails == failed)
          {
            this.TextBlockStatus.Text = "STOPPED ... TO MANY FAILURES ...";
            break;
          }
          DeviceIdentification version = await this.DeviceCommands.ReadVersionAsync(this.progress, this.cancelToken.Token);
          ++successful;
          if (this.CheckBoxInterruptConnection.IsChecked.Value)
            await this.DeviceCommands.InterruptConnection(this.progress, this.cancelToken.Token);
          if (this.usedConfigList.CycleTime > 0)
          {
            startTime = startTime.AddMilliseconds((double) this.usedConfigList.CycleTime);
            int wait_ms = (int) (startTime - DateTime.Now).TotalMilliseconds;
            string versionPrint = version.Print();
            this.TextBlockResult.Text = "Wait ms: " + wait_ms.ToString() + Environment.NewLine + versionPrint;
            if (wait_ms > 0)
              await Task.Delay(wait_ms);
            else
              startTime = DateTime.Now;
            versionPrint = (string) null;
          }
          else
            this.TextBlockResult.Text = version.Print();
          version = (DeviceIdentification) null;
        }
        catch (OperationCanceledException ex)
        {
          this.progress.Reset("Canceled");
          break;
        }
        catch (Exception ex)
        {
          this.TextBlockResult.Text = ex.Message;
          ++failed;
        }
        finally
        {
          this.isRunning = false;
        }
        this.SetValuesForProgress(++count, successful, failed);
        if (!this.CheckBoxRunCyle.IsChecked.Value)
          break;
      }
    }

    public void SetValuesForProgress(int cycle, int success, int fail)
    {
      this.TextBoxCyles.Text = cycle.ToString();
      this.TextBoxCycleSuccessful.Text = success.ToString();
      this.TextBoxCycleFailed.Text = fail.ToString();
    }

    private void TextBoxCycleTime_LostFocus(object sender, RoutedEventArgs e)
    {
      if (this.isInit)
        return;
      string text = this.TextBoxCycleTime.Text;
      int result = 200;
      if (int.TryParse(text, out result) && result != this.usedConfigList.CycleTime)
        this.usedConfigList.CycleTime = result;
    }

    private void TextBoxCycleTime_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
      {
        string text = this.TextBoxCycleTime.Text;
        int result = 200;
        if (!int.TryParse(text, out result) || result == this.usedConfigList.CycleTime)
          return;
        this.usedConfigList.CycleTime = result;
        this.TextBoxCycleTime.Background = this.DefaultBackground;
      }
      else
        this.TextBoxCycleTime.Background = this.ChangedBackground;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/testcommunicationwindow.xaml", UriKind.Relative));
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
          this.TextBlockStatus = (TextBlock) target;
          break;
        case 3:
          this.ProgressBar1 = (ProgressBar) target;
          break;
        case 4:
          this.TextBlockResult = (TextBox) target;
          break;
        case 5:
          this.PanalButtons = (WrapPanel) target;
          break;
        case 6:
          this.TextBoxCyles = (TextBox) target;
          break;
        case 7:
          this.TextBoxCycleSuccessful = (TextBox) target;
          break;
        case 8:
          this.TextBoxCycleFailed = (TextBox) target;
          break;
        case 9:
          this.PanalButtons2 = (WrapPanel) target;
          break;
        case 10:
          this.TextBoxCycleStopAfterErrors = (TextBox) target;
          break;
        case 11:
          this.TextBoxCycleTime = (TextBox) target;
          this.TextBoxCycleTime.LostFocus += new RoutedEventHandler(this.TextBoxCycleTime_LostFocus);
          this.TextBoxCycleTime.KeyDown += new KeyEventHandler(this.TextBoxCycleTime_KeyDown);
          break;
        case 12:
          this.PanalButtons3 = (WrapPanel) target;
          break;
        case 13:
          this.CheckBoxHaltOnFirst = (CheckBox) target;
          break;
        case 14:
          this.CheckBoxRunCyle = (CheckBox) target;
          break;
        case 15:
          this.CheckBoxInterruptConnection = (CheckBox) target;
          break;
        case 16:
          this.StackPanalButtons2 = (StackPanel) target;
          break;
        case 17:
          this.StackPanalButton3 = (StackPanel) target;
          break;
        case 18:
          this.ButtonRunCommand = (Button) target;
          this.ButtonRunCommand.Click += new RoutedEventHandler(this.ButtonRunCommand_Click);
          break;
        case 19:
          this.ButtonBreak = (Button) target;
          this.ButtonBreak.Click += new RoutedEventHandler(this.ButtonBreak_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
