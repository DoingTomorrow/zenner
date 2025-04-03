// Decompiled with JetBrains decompiler
// Type: HandlerLib.VolumeSimulator
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommunicationPort.UserInterface;
using ReadoutConfiguration;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class VolumeSimulator : Window, IComponentConnector
  {
    private CommunicationPortWindowFunctions myPort;
    private bool threadRun;
    private int intervalSeconds;
    private DateTime intervalStartTime;
    private DateTime intervalEndTime;
    private DateTime nextRefreshTime;
    private double volume;
    private double flow;
    private uint volMeterID = 12345678;
    internal Button ButtonRunStop;
    internal Button ButtonSetCom;
    internal TextBox TextBoxVolume;
    internal TextBox TextBoxFlow;
    internal TextBox TextBoxCycleTime;
    internal TextBox TextBoxSecondsToProtocol;
    internal TextBox TextBoxProtocolCount;
    internal TextBox TextBoxVolumeMeterID;
    private bool _contentLoaded;

    public static void RunVolumeSimulator()
    {
      Thread thread = new Thread((ThreadStart) (() =>
      {
        SynchronizationContext.SetSynchronizationContext((SynchronizationContext) new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));
        new VolumeSimulator().Show();
        Dispatcher.Run();
      }));
      thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
      thread.Name = "Translator";
      thread.IsBackground = true;
      thread.SetApartmentState(ApartmentState.STA);
      thread.Start();
    }

    public VolumeSimulator()
    {
      this.InitializeComponent();
      int.TryParse(this.TextBoxCycleTime.Text, out this.intervalSeconds);
      this.ButtonRunStop.Content = (object) "Run";
      this.myPort = new CommunicationPortWindowFunctions();
      this.myPort.SetReadoutConfiguration(new ConfigList(ReadoutConfigFunctions.Manager.GetConnectionProfile(155).GetSettingsList())
      {
        Wakeup = WakeupSystem.None.ToString(),
        Port = "COM18",
        MinoConnectBaseState = "RS232_3V"
      });
    }

    private void ButtonSetCom_Click(object sender, RoutedEventArgs e)
    {
      this.myPort.ShowMainWindow();
    }

    private void ButtonRunStop_Click(object sender, RoutedEventArgs e)
    {
      if (this.ButtonRunStop.Content.ToString() == "Run")
      {
        this.TextBoxVolume.IsEnabled = false;
        this.ButtonSetCom.IsEnabled = false;
        this.ButtonRunStop.Content = (object) "Stop";
        this.threadRun = true;
        int.TryParse(this.TextBoxCycleTime.Text, out this.intervalSeconds);
        double.TryParse(this.TextBoxVolume.Text, out this.volume);
        double.TryParse(this.TextBoxFlow.Text, out this.flow);
        uint.TryParse(this.TextBoxVolumeMeterID.Text, out this.volMeterID);
        ThreadPool.QueueUserWorkItem(new WaitCallback(this.ThreadLoopFunction));
      }
      else
      {
        this.TextBoxVolume.IsEnabled = true;
        this.ButtonSetCom.IsEnabled = true;
        this.ButtonRunStop.Content = (object) "Run";
        this.threadRun = false;
      }
    }

    private void RefreshUI(object sender, int dummy)
    {
      if (this.Dispatcher.Thread != Thread.CurrentThread)
      {
        try
        {
          this.Dispatcher.BeginInvoke((Delegate) new EventHandler<int>(this.RefreshUI), sender, (object) dummy);
        }
        catch
        {
        }
      }
      else
        this.TextBoxVolume.Text = this.volume.ToString("0.000000");
    }

    private void ThreadLoopFunction(object dummy)
    {
      this.intervalStartTime = DateTime.Now;
      this.intervalEndTime = this.intervalStartTime.AddSeconds((double) this.intervalSeconds);
      this.nextRefreshTime = DateTime.Now;
      while (this.threadRun)
      {
        DateTime now = DateTime.Now;
        if (now > this.intervalEndTime)
        {
          if (this.intervalStartTime != DateTime.MinValue)
            this.volume += this.flow / 3600.0 * now.Subtract(this.intervalStartTime).TotalSeconds;
          byte[] buffer = new byte[19];
          int num1 = 0;
          byte[] numArray1 = buffer;
          int index1 = num1;
          int num2 = index1 + 1;
          numArray1[index1] = (byte) 91;
          uint volMeterId = this.volMeterID;
          for (int index2 = 0; index2 < 4; ++index2)
          {
            buffer[num2++] = (byte) volMeterId;
            volMeterId >>= 8;
          }
          this.intervalStartTime = now;
          this.intervalEndTime = this.intervalStartTime.AddSeconds((double) this.intervalSeconds);
          string str1 = ((long) (this.volume * 1000000.0)).ToString();
          int num3 = str1.Length - 1;
          for (int index3 = 0; index3 < 7; ++index3)
          {
            byte num4 = 0;
            if (num3 >= 0)
              num4 = (byte) ((uint) (byte) str1[num3--] - 48U);
            byte num5 = num4;
            byte num6 = 0;
            if (num3 >= 0)
              num6 = (byte) ((uint) (byte) str1[num3--] - 48U);
            byte num7 = (byte) ((uint) num5 | (uint) (byte) ((uint) num6 << 4));
            buffer[num2++] = num7;
          }
          string str2 = ((long) (this.flow * 1000.0)).ToString();
          int num8 = str2.Length - 1;
          for (int index4 = 0; index4 < 4; ++index4)
          {
            byte num9 = 0;
            if (num8 >= 0)
              num9 = (byte) ((uint) (byte) str2[num8--] - 48U);
            byte num10 = num9;
            byte num11 = 0;
            if (num8 >= 0)
              num11 = (byte) ((uint) (byte) str2[num8--] - 48U);
            byte num12 = (byte) ((uint) num10 | (uint) (byte) ((uint) num11 << 4));
            buffer[num2++] = num12;
          }
          byte[] numArray2 = buffer;
          int index5 = num2;
          int num13 = index5 + 1;
          numArray2[index5] = (byte) 0;
          byte[] numArray3 = buffer;
          int index6 = num13;
          int index7 = index6 + 1;
          numArray3[index6] = (byte) 0;
          byte num14 = 0;
          for (int index8 = 0; index8 < index7; ++index8)
            num14 += buffer[index8];
          buffer[index7] = num14;
          this.myPort.portFunctions.communicationObject.Write(buffer);
        }
        else if (now > this.nextRefreshTime)
        {
          this.RefreshUI((object) this, 0);
          this.nextRefreshTime = now.AddSeconds(1.0);
        }
        else
          Thread.Sleep(50);
      }
    }

    private void TextBoxFlow_LostFocus(object sender, RoutedEventArgs e) => this.ChangeFlow();

    private void TextBoxFlow_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      this.ChangeFlow();
    }

    private void ChangeFlow() => double.TryParse(this.TextBoxFlow.Text, out this.flow);

    private void TextBoxCycleTime_LostFocus(object sender, RoutedEventArgs e)
    {
      this.ChangeCycleTime();
    }

    private void TextBoxCycleTime_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      this.ChangeCycleTime();
    }

    private void ChangeCycleTime()
    {
      if (int.TryParse(this.TextBoxCycleTime.Text, out this.intervalSeconds))
      {
        if (this.intervalSeconds < 1)
        {
          this.intervalSeconds = 4;
          this.TextBoxCycleTime.Text = this.intervalSeconds.ToString();
        }
        this.intervalEndTime = this.intervalStartTime.AddSeconds((double) this.intervalSeconds);
      }
      else
        this.TextBoxCycleTime.Text = this.intervalSeconds.ToString();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/volumesimulator.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ButtonRunStop = (Button) target;
          this.ButtonRunStop.Click += new RoutedEventHandler(this.ButtonRunStop_Click);
          break;
        case 2:
          this.ButtonSetCom = (Button) target;
          this.ButtonSetCom.Click += new RoutedEventHandler(this.ButtonSetCom_Click);
          break;
        case 3:
          this.TextBoxVolume = (TextBox) target;
          break;
        case 4:
          this.TextBoxFlow = (TextBox) target;
          this.TextBoxFlow.PreviewKeyDown += new KeyEventHandler(this.TextBoxFlow_PreviewKeyDown);
          this.TextBoxFlow.LostFocus += new RoutedEventHandler(this.TextBoxFlow_LostFocus);
          break;
        case 5:
          this.TextBoxCycleTime = (TextBox) target;
          this.TextBoxCycleTime.LostFocus += new RoutedEventHandler(this.TextBoxCycleTime_LostFocus);
          this.TextBoxCycleTime.PreviewKeyDown += new KeyEventHandler(this.TextBoxCycleTime_PreviewKeyDown);
          break;
        case 6:
          this.TextBoxSecondsToProtocol = (TextBox) target;
          break;
        case 7:
          this.TextBoxProtocolCount = (TextBox) target;
          break;
        case 8:
          this.TextBoxVolumeMeterID = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
