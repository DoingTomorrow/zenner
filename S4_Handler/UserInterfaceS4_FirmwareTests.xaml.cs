// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_FirmwareTests
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using HandlerLib;
using S4_Handler.Functions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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
  public partial class S4_FirmwareTests : Window, IComponentConnector
  {
    private CancellationTokenSource cancelTokenSource;
    private ProgressHandler progress;
    private S4_HandlerWindowFunctions myWindowFunctions;
    private S4_HandlerFunctions myFunctions;
    private S4_DeviceCommandsNFC myCommands;
    private Cursor defaultCursor;
    private DateTime CronTestTime = new DateTime(2000, 2, 1);
    private bool breakLoop;
    internal StartupLib.GmmCorporateControl gmmCorporateControl1;
    internal GroupBox GroupBoxButtoms;
    internal Button ButtonBreak;
    internal Button ButtonRunCronTest;
    internal Button ButtonShowDebugQueue;
    internal TextBox TextBoxResults;
    private bool _contentLoaded;

    public S4_FirmwareTests(S4_HandlerWindowFunctions myWindowFunctions)
    {
      this.myWindowFunctions = myWindowFunctions;
      this.myFunctions = myWindowFunctions.MyFunctions;
      this.myCommands = this.myFunctions.myDeviceCommands;
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.InitializeComponent();
    }

    private void OnProgress(ProgressArg obj)
    {
      if (this.CheckAccess())
        return;
      this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
    }

    private void ButtonShowDebugQueue_Click(object sender, RoutedEventArgs e)
    {
      this.ShowDebugQueue();
    }

    private void ShowDebugQueue()
    {
      try
      {
        DebugQueueWindow debugQueueWindow = new DebugQueueWindow(new SortedList<int, string>()
        {
          {
            0,
            "none"
          },
          {
            1,
            "cron_func_installed"
          },
          {
            2,
            "cron_func_call"
          },
          {
            3,
            "cron_tick_call"
          },
          {
            4,
            "cron_inst_timespan_GT_cycle_time"
          },
          {
            5,
            "cron_inst_timespan_GT_max_time"
          },
          {
            6,
            "cron_function_array_overflow"
          },
          {
            7,
            "cron_function_0_pointer"
          },
          {
            8,
            "cron_cycle_to_short"
          }
        }, new DebugQueueWindow.ReadDebugQueueDataFunction(this.ReadDebugQueueData));
        debugQueueWindow.Owner = (Window) this;
        debugQueueWindow.Show();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private DebugQueueData ReadDebugQueueData() => new DebugQueueData();

    private async void ButtonRunCronTest_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.TextBoxResults.Text = "Cron test" + Environment.NewLine + Environment.NewLine;
        this.CronTestTime = new DateTime(this.CronTestTime.Year, this.CronTestTime.Month, 1);
        this.CronTestTime = this.CronTestTime.AddSeconds(-1.0);
        this.breakLoop = false;
        while (!this.breakLoop)
        {
          TextBox textBoxResults1 = this.TextBoxResults;
          string str1 = this.CronTestTime.ToString("yyyy.MM.dd HH.mm.ss");
          uint seconds2000 = this.GetSeconds2000(this.CronTestTime);
          string str2 = seconds2000.ToString("x08");
          string textData1 = str1 + " = 0x" + str2 + "...";
          textBoxResults1.AppendText(textData1);
          this.TextBoxResults.ScrollToEnd();
          await this.Call_Cron_GetDateTime(this.CronTestTime);
          await this.Call_Cron_GetSec2000FromDateTime(this.CronTestTime);
          this.TextBoxResults.AppendText("ok" + Environment.NewLine);
          this.CronTestTime = this.CronTestTime.AddSeconds(1.0);
          TextBox textBoxResults2 = this.TextBoxResults;
          string str3 = this.CronTestTime.ToString("yyyy.MM.dd HH.mm.ss");
          seconds2000 = this.GetSeconds2000(this.CronTestTime);
          string str4 = seconds2000.ToString("x08");
          string textData2 = str3 + " = 0x" + str4 + "...";
          textBoxResults2.AppendText(textData2);
          this.TextBoxResults.ScrollToEnd();
          await this.Call_Cron_GetDateTime(this.CronTestTime);
          await this.Call_Cron_GetSec2000FromDateTime(this.CronTestTime);
          this.TextBoxResults.AppendText("ok" + Environment.NewLine);
          this.CronTestTime = this.CronTestTime.AddMonths(1).AddSeconds(-1.0);
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private async Task Call_Cron_GetDateTime(DateTime testTime)
    {
      uint secs2000 = testTime.Year >= 2000 && testTime.Year <= 2099 ? this.GetSeconds2000(testTime) : throw new Exception("Year out of range 2000..2099");
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 0);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(secs2000));
      byte[] result = await this.myCommands.CommonNfcCommands.CallTestFunctionAsync(this.progress, this.cancelTokenSource.Token, byteList.ToArray());
      byte second = result.Length == 7 ? result[1] : throw new Exception("Illegal byte count");
      byte minute = result[2];
      byte hour = result[3];
      byte day = result[4];
      byte month = result[5];
      int year = (int) result[6] + 2000;
      DateTime resultTime = new DateTime(year, (int) month, (int) day, (int) hour, (int) minute, (int) second);
      if (resultTime != testTime)
      {
        this.TextBoxResults.AppendText(Environment.NewLine + Environment.NewLine + "Firmware error:" + Environment.NewLine);
        this.TextBoxResults.AppendText("Result time: " + resultTime.ToString("yyyy.MM.dd HH.mm.ss") + Environment.NewLine);
        throw new Exception("Illegal result time");
      }
      byteList = (List<byte>) null;
      result = (byte[]) null;
    }

    private async Task Call_Cron_GetSec2000FromDateTime(DateTime testTime)
    {
      uint secs2000 = testTime.Year >= 2000 && testTime.Year <= 2099 ? this.GetSeconds2000(testTime) : throw new Exception("Year out of range 2000..2099");
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 1);
      byteList.Add((byte) testTime.Second);
      byteList.Add((byte) testTime.Minute);
      byteList.Add((byte) testTime.Hour);
      byteList.Add((byte) testTime.Day);
      byteList.Add((byte) testTime.Month);
      byteList.Add((byte) (testTime.Year - 2000));
      byte[] result = await this.myCommands.CommonNfcCommands.CallTestFunctionAsync(this.progress, this.cancelTokenSource.Token, byteList.ToArray());
      uint resultSecs = result.Length == 5 ? BitConverter.ToUInt32(result, 1) : throw new Exception("Illegal byte count");
      if ((int) resultSecs != (int) secs2000)
      {
        DateTime resultTime = this.GetDateTimeFromSeconds2000(secs2000);
        this.TextBoxResults.AppendText(Environment.NewLine + Environment.NewLine + "Firmware error:" + Environment.NewLine);
        this.TextBoxResults.AppendText("Result time: " + resultTime.ToString("yyyy.MM.dd HH.mm.ss") + Environment.NewLine);
        throw new Exception("Illegal result time");
      }
      byteList = (List<byte>) null;
      result = (byte[]) null;
    }

    private uint GetSeconds2000(DateTime dateTime)
    {
      return (uint) dateTime.Subtract(new DateTime(2000, 1, 1)).TotalSeconds;
    }

    private DateTime GetDateTimeFromSeconds2000(uint secs2000)
    {
      return new DateTime(2000, 1, 1).AddSeconds((double) secs2000);
    }

    private void ButtonBreak_Click(object sender, RoutedEventArgs e) => this.breakLoop = true;

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_firmwaretests.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.gmmCorporateControl1 = (StartupLib.GmmCorporateControl) target;
          break;
        case 2:
          this.GroupBoxButtoms = (GroupBox) target;
          break;
        case 3:
          this.ButtonBreak = (Button) target;
          this.ButtonBreak.Click += new RoutedEventHandler(this.ButtonBreak_Click);
          break;
        case 4:
          this.ButtonRunCronTest = (Button) target;
          this.ButtonRunCronTest.Click += new RoutedEventHandler(this.ButtonRunCronTest_Click);
          break;
        case 5:
          this.ButtonShowDebugQueue = (Button) target;
          this.ButtonShowDebugQueue.Click += new RoutedEventHandler(this.ButtonShowDebugQueue_Click);
          break;
        case 6:
          this.TextBoxResults = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
