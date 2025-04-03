// Decompiled with JetBrains decompiler
// Type: HandlerLib.ReceiveTestPacketMiConWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommunicationPort;
using CommunicationPort.Functions;
using ReadoutConfiguration;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class ReceiveTestPacketMiConWindow : Window, IComponentConnector
  {
    private RadioMode mode;
    private CommunicationByMinoConnect micon;
    internal Button ButtonReceiveTestPacket;
    internal TextBox TextBoxTimeout;
    internal TextBox TextBoxOutput;
    internal ProgressBar ProgressBarRadioTest;
    internal TextBox TextBoxSerialnumber;
    internal Button ButtonConnectionProfile;
    private bool _contentLoaded;

    public ReceiveTestPacketMiConWindow()
    {
      this.InitializeComponent();
      this.Closing += new CancelEventHandler(this.Window_Closing);
    }

    private void Window_Closing(object sender, CancelEventArgs e) => this.Dispose();

    public static void Show(Window owner)
    {
      ReceiveTestPacketMiConWindow packetMiConWindow = new ReceiveTestPacketMiConWindow();
      if (owner != null)
        packetMiConWindow.Owner = owner;
      packetMiConWindow.Show();
    }

    public static void ShowDialog(Window owner)
    {
      ReceiveTestPacketMiConWindow packetMiConWindow = new ReceiveTestPacketMiConWindow();
      if (owner != null)
        packetMiConWindow.Owner = owner;
      if (packetMiConWindow.ShowDialog().Value)
        ;
    }

    private void Dispose()
    {
      if (this.micon == null)
        return;
      this.micon.Close();
      this.micon.Dispose();
      this.micon = (CommunicationByMinoConnect) null;
    }

    private void ButtonConnectionProfile_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.Dispose();
        ConfigList configList = ReadoutConfigMain.ShowDialog(32);
        if (configList == null)
          return;
        if (!Enum.IsDefined(typeof (RadioMode), (object) configList["BusMode"]))
        {
          int num = (int) MessageBox.Show((Window) this, "This profile is not supported!", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
        }
        else
        {
          this.mode = (RadioMode) Enum.Parse(typeof (RadioMode), configList["BusMode"], true);
          CommunicationPortFunctions communicationPortFunctions = new CommunicationPortFunctions();
          communicationPortFunctions.SetReadoutConfiguration(configList);
          communicationPortFunctions.Open();
          this.micon = communicationPortFunctions.GetCommunicationByMinoConnect();
        }
      }
      catch (Exception ex)
      {
        ErrorMessageBox.ShowDialog((Window) this, ex.Message, ex);
      }
    }

    private async void ButtonReceiveTestPacket_Click(object sender, RoutedEventArgs e)
    {
      if (this.micon == null)
      {
        int num1 = (int) MessageBox.Show((Window) this, "Please select a profile!", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      else
      {
        ushort timeoutInSec;
        if (!ushort.TryParse(this.TextBoxTimeout.Text, out timeoutInSec))
        {
          int num2 = (int) MessageBox.Show((Window) this, "Timeout is wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
        }
        else if (timeoutInSec == (ushort) 0)
        {
          int num3 = (int) MessageBox.Show((Window) this, "Timeout is wrong! It should be greater as 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
        }
        else if (!int.TryParse(this.TextBoxSerialnumber.Text, out int _))
        {
          int num4 = (int) MessageBox.Show((Window) this, "Serialnumber is invalid!", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
        }
        else
        {
          try
          {
            this.ButtonReceiveTestPacket.IsEnabled = false;
            RadioTestResult result = (RadioTestResult) null;
            await Task.Run((Action) (() => result = this.micon.ReceiveOnePacket(this.mode, serialnumber, timeoutInSec, "0FF0")));
            if (result != null)
            {
              byte[] arbitraryData = new byte[28];
              Buffer.BlockCopy((Array) result.Payload, 11, (Array) arbitraryData, 0, arbitraryData.Length);
              this.TextBoxOutput.AppendText(Utility.ByteArrayToHexString(result.ReceiveBuffer));
              this.TextBoxOutput.AppendText(" RSSI: ");
              this.TextBoxOutput.AppendText(result.RSSI.ToString());
              this.TextBoxOutput.AppendText(" LQI: ");
              this.TextBoxOutput.AppendText(result.LQI.ToString("X2") + "h");
              this.TextBoxOutput.AppendText(" MCT: ");
              this.TextBoxOutput.AppendText(result.MCT.ToString());
              this.TextBoxOutput.AppendText(Environment.NewLine);
              this.TextBoxOutput.ScrollToEnd();
              arbitraryData = (byte[]) null;
            }
            else
            {
              this.TextBoxOutput.AppendText("NULL");
              this.TextBoxOutput.AppendText(Environment.NewLine);
              this.TextBoxOutput.ScrollToEnd();
            }
          }
          catch (Exception ex)
          {
            int num5 = (int) MessageBox.Show((Window) this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
          }
          finally
          {
            this.ButtonReceiveTestPacket.IsEnabled = true;
          }
        }
      }
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

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/receivetestpacketmiconwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ButtonReceiveTestPacket = (Button) target;
          this.ButtonReceiveTestPacket.Click += new RoutedEventHandler(this.ButtonReceiveTestPacket_Click);
          break;
        case 2:
          this.TextBoxTimeout = (TextBox) target;
          this.TextBoxTimeout.TextChanged += new TextChangedEventHandler(this.TextBoxTimeout_TextChanged);
          break;
        case 3:
          this.TextBoxOutput = (TextBox) target;
          break;
        case 4:
          this.ProgressBarRadioTest = (ProgressBar) target;
          break;
        case 5:
          this.TextBoxSerialnumber = (TextBox) target;
          break;
        case 6:
          this.ButtonConnectionProfile = (Button) target;
          this.ButtonConnectionProfile.Click += new RoutedEventHandler(this.ButtonConnectionProfile_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
