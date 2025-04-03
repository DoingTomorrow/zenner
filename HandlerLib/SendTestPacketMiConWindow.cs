// Decompiled with JetBrains decompiler
// Type: HandlerLib.SendTestPacketMiConWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommunicationPort.Functions;
using ReadoutConfiguration;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class SendTestPacketMiConWindow : Window, IComponentConnector
  {
    private RadioMode mode;
    private CommunicationByMinoConnect micon;
    internal Button ButtonConnectionProfile;
    internal Button ButtonSend;
    internal TextBox TextBoxSerialnumber;
    internal ComboBox ComboBoxMode;
    private bool _contentLoaded;

    public SendTestPacketMiConWindow()
    {
      this.InitializeComponent();
      this.Closing += new CancelEventHandler(this.Window_Closing);
      this.ComboBoxMode.ItemsSource = (IEnumerable) Enum.GetValues(typeof (RadioMode));
      this.ComboBoxMode.SelectedItem = (object) RadioMode.Radio3;
    }

    public static void ShowDialog(Window owner)
    {
      SendTestPacketMiConWindow packetMiConWindow = new SendTestPacketMiConWindow();
      packetMiConWindow.Owner = owner;
      packetMiConWindow.ShowDialog();
    }

    public static void Show(Window owner)
    {
      SendTestPacketMiConWindow packetMiConWindow = new SendTestPacketMiConWindow();
      packetMiConWindow.Owner = owner;
      packetMiConWindow.Show();
    }

    private void ButtonSend_Click(object sender, RoutedEventArgs e)
    {
      if (this.micon == null)
      {
        int num1 = (int) MessageBox.Show((Window) this, "Please select a profile!", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      else
      {
        try
        {
          uint deviceID = uint.Parse(this.TextBoxSerialnumber.Text);
          if (deviceID.ToString().Length > 8)
          {
            int num2 = (int) MessageBox.Show((Window) this, "The serialnumber must be less or equal as 8 chars!", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
          }
          else
            this.micon.SendTestPacket(deviceID, this.mode, (byte) 7, "0FF0");
        }
        catch (Exception ex)
        {
          ErrorMessageBox.ShowDialog((Window) this, ex.Message, ex);
        }
      }
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
          this.mode = (RadioMode) Enum.Parse(typeof (RadioMode), this.ComboBoxMode.SelectedItem.ToString(), true);
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

    private void Dispose()
    {
      if (this.micon == null)
        return;
      this.micon.Close();
      this.micon.Dispose();
      this.micon = (CommunicationByMinoConnect) null;
    }

    private void Window_Closing(object sender, CancelEventArgs e) => this.Dispose();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/sendtestpacketmiconwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ButtonConnectionProfile = (Button) target;
          this.ButtonConnectionProfile.Click += new RoutedEventHandler(this.ButtonConnectionProfile_Click);
          break;
        case 2:
          this.ButtonSend = (Button) target;
          this.ButtonSend.Click += new RoutedEventHandler(this.ButtonSend_Click);
          break;
        case 3:
          this.TextBoxSerialnumber = (TextBox) target;
          break;
        case 4:
          this.ComboBoxMode = (ComboBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
