// Decompiled with JetBrains decompiler
// Type: HandlerLib.ReceiveTestPacketWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class ReceiveTestPacketWindow : Window, IComponentConnector
  {
    private DeviceCommandsMBus cmd;
    private CommonRadioCommands radio;
    internal Button ButtonStart;
    internal TextBox TextBoxSize;
    internal TextBox TextBoxTimeout;
    internal TextBox TextBoxOutput;
    internal ProgressBar ProgressBarRadioTest;
    internal Button ButtonExport;
    private bool _contentLoaded;

    public ReceiveTestPacketWindow() => this.InitializeComponent();

    public static void ShowDialog(Window owner, DeviceCommandsMBus cmd, CommonRadioCommands radio)
    {
      if (cmd == null)
        throw new NullReferenceException(nameof (cmd));
      ReceiveTestPacketWindow testPacketWindow = new ReceiveTestPacketWindow();
      testPacketWindow.Owner = owner;
      testPacketWindow.cmd = cmd;
      testPacketWindow.radio = radio;
      if (testPacketWindow.ShowDialog().Value)
        ;
    }

    private async void ButtonStart_Click(object sender, RoutedEventArgs e)
    {
      byte telegramSize;
      ProgressHandler dummy1;
      CancellationToken dummy2;
      if (!byte.TryParse(this.TextBoxSize.Text, out telegramSize))
      {
        int num = (int) MessageBox.Show((Window) this, "Size is wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
        dummy1 = (ProgressHandler) null;
        dummy2 = new CancellationToken();
      }
      else
      {
        ushort timeoutInSec;
        if (!ushort.TryParse(this.TextBoxTimeout.Text, out timeoutInSec))
        {
          int num = (int) MessageBox.Show((Window) this, "Timeout is wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
          dummy1 = (ProgressHandler) null;
          dummy2 = new CancellationToken();
        }
        else if (timeoutInSec == (ushort) 0)
        {
          int num = (int) MessageBox.Show((Window) this, "Timeout is wrong! It should be greater as 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
          dummy1 = (ProgressHandler) null;
          dummy2 = new CancellationToken();
        }
        else
        {
          dummy1 = new ProgressHandler((Action<ProgressArg>) (x => { }));
          dummy2 = new CancellationToken();
          try
          {
            this.ButtonStart.IsEnabled = false;
            bool isConnected = this.cmd.ConnectedReducedID != null;
            if (!isConnected)
            {
              DeviceVersionMBus deviceVersionMbus = await this.cmd.ReadVersionAsync(dummy1, dummy2);
            }
            await this.radio.ReceiveAndStreamRadio3Scenario3TelegramsAsync(telegramSize, timeoutInSec, dummy1, dummy2);
            this.cmd.MBus.Repeater.Port.DiscardInBuffer();
            DateTime start = DateTime.Now;
            DateTime end = start.AddSeconds((double) timeoutInSec);
            while (DateTime.Now <= end)
            {
              await Task.Delay(500);
              ulong current = Convert.ToUInt64((DateTime.Now - start).TotalSeconds);
              this.Log(this.cmd.MBus.Repeater.Port.ReadExisting(), Convert.ToInt32(current * 100UL / (ulong) timeoutInSec));
            }
            dummy1 = (ProgressHandler) null;
            dummy2 = new CancellationToken();
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show((Window) this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            dummy1 = (ProgressHandler) null;
            dummy2 = new CancellationToken();
          }
          finally
          {
            this.ButtonStart.IsEnabled = true;
          }
        }
      }
    }

    private void Log(byte[] buffer, int progress)
    {
      if (!this.CheckAccess())
      {
        this.Dispatcher.Invoke((Action) (() => this.Log(buffer, progress)));
      }
      else
      {
        this.ProgressBarRadioTest.Value = (double) progress;
        if (buffer == null)
          return;
        this.TextBoxOutput.AppendText(Utility.ByteArrayToHexString(buffer));
        this.TextBoxOutput.AppendText(Environment.NewLine);
        this.TextBoxOutput.ScrollToEnd();
      }
    }

    private void TextBoxSize_TextChanged(object sender, TextChangedEventArgs e)
    {
      TextBox textBox = sender as TextBox;
      int result;
      if (!int.TryParse(textBox.Text, out result))
        return;
      if (result > (int) byte.MaxValue)
        textBox.Text = "255";
      else if (result < 0)
        textBox.Text = "0";
    }

    private void TextBoxTimeout_TextChanged(object sender, TextChangedEventArgs e)
    {
      TextBox textBox = sender as TextBox;
      int result;
      if (!int.TryParse(textBox.Text, out result))
        return;
      if (result > (int) byte.MaxValue)
        textBox.Text = "65535";
      else if (result < 0)
        textBox.Text = "0";
    }

    private void ButtonExport_Click(object sender, RoutedEventArgs e)
    {
      string str1 = this.TextBoxOutput.Text.Replace(Environment.NewLine, string.Empty);
      if (string.IsNullOrEmpty(str1))
        return;
      int startIndex1 = 0;
      while (startIndex1 >= 0)
      {
        int startIndex2 = str1.IndexOf("68", startIndex1);
        if (startIndex2 >= 0)
        {
          if (startIndex2 != 0 && startIndex2 >= 0 && startIndex2 + 8 <= str1.Length && str1[startIndex2 + 6] == '6' && str1[startIndex2 + 7] == '8')
          {
            str1 = str1.Insert(startIndex2, "|");
            startIndex1 = startIndex2 + 9;
          }
          else
            startIndex1 = startIndex2 + 1;
        }
        else
          break;
      }
      string[] strArray = str1.Split('|');
      string str2 = Path.Combine(Path.GetTempPath(), "SaveFile.csv");
      using (StreamWriter streamWriter = new StreamWriter(str2))
      {
        foreach (string str3 in strArray)
        {
          streamWriter.Write(str3);
          streamWriter.WriteLine(";");
        }
      }
      Process.Start(str2);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/receivetestpacketwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ButtonStart = (Button) target;
          this.ButtonStart.Click += new RoutedEventHandler(this.ButtonStart_Click);
          break;
        case 2:
          this.TextBoxSize = (TextBox) target;
          this.TextBoxSize.TextChanged += new TextChangedEventHandler(this.TextBoxSize_TextChanged);
          break;
        case 3:
          this.TextBoxTimeout = (TextBox) target;
          this.TextBoxTimeout.TextChanged += new TextChangedEventHandler(this.TextBoxTimeout_TextChanged);
          break;
        case 4:
          this.TextBoxOutput = (TextBox) target;
          break;
        case 5:
          this.ProgressBarRadioTest = (ProgressBar) target;
          break;
        case 6:
          this.ButtonExport = (Button) target;
          this.ButtonExport.Click += new RoutedEventHandler(this.ButtonExport_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
