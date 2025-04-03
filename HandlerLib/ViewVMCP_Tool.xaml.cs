// Decompiled with JetBrains decompiler
// Type: HandlerLib.View.VMCP_Tool
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommunicationPort.UserInterface;
using ReadoutConfiguration;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib.View
{
  public partial class VMCP_Tool : Window, IComponentConnector
  {
    private CommunicationPortWindowFunctions myPortWindowFunctions;
    private ConfigList configList;
    private bool BreakLoop = false;
    private bool TransmitProtocol = false;
    private byte[] TransmitProtocolData;
    private string TransmitProtocolName;
    private byte[] SetIdProtocol = new byte[15]
    {
      (byte) 104,
      (byte) 9,
      (byte) 9,
      (byte) 104,
      (byte) 83,
      (byte) 1,
      (byte) 81,
      (byte) 12,
      (byte) 121,
      (byte) 85,
      (byte) 22,
      (byte) 96,
      (byte) 96,
      (byte) 85,
      (byte) 22
    };
    private byte[] SetDN50_UnitsProtocol = new byte[17]
    {
      (byte) 104,
      (byte) 11,
      (byte) 11,
      (byte) 104,
      (byte) 83,
      (byte) 1,
      (byte) 81,
      (byte) 15,
      (byte) 22,
      (byte) 0,
      (byte) 0,
      (byte) 6,
      (byte) 1,
      (byte) 250,
      (byte) 0,
      (byte) 203,
      (byte) 22
    };
    private byte[] Set_CycleTime_Protocol = new byte[13]
    {
      (byte) 104,
      (byte) 7,
      (byte) 7,
      (byte) 104,
      (byte) 83,
      (byte) 254,
      (byte) 81,
      (byte) 15,
      (byte) 49,
      (byte) 0,
      (byte) 0,
      (byte) 226,
      (byte) 22
    };
    private byte[] Get_Identification_Protocol = new byte[12]
    {
      (byte) 104,
      (byte) 6,
      (byte) 6,
      (byte) 104,
      (byte) 83,
      (byte) 254,
      (byte) 81,
      (byte) 15,
      (byte) 53,
      (byte) 0,
      (byte) 230,
      (byte) 22
    };
    private byte[] VmcpDemoProtocol = new byte[19]
    {
      (byte) 91,
      (byte) 135,
      (byte) 101,
      (byte) 67,
      (byte) 50,
      (byte) 120,
      (byte) 86,
      (byte) 52,
      (byte) 120,
      (byte) 86,
      (byte) 52,
      (byte) 18,
      (byte) 120,
      (byte) 86,
      (byte) 52,
      (byte) 18,
      (byte) 0,
      (byte) 0,
      (byte) 230
    };
    private StringBuilder StatusInfo = new StringBuilder();
    private DateTime lastCycleTime;
    internal GmmCorporateControl gmmCorporateControl1;
    internal ComboBox ComboBoxComPort;
    internal StackPanel StackPanelButtons;
    internal Button ButtonReceiveCycle;
    internal Button ButtonRequestIdentification;
    internal TextBox TextBoxVmcpCycle;
    internal Button ButtonSetVmcpCycle;
    internal TextBox TextBoxNewId;
    internal Button ButtonSetID;
    internal Button ButtonSetDN50Calibration;
    internal Button ButtonBreak;
    internal TextBox TextBoxStatus;
    private bool _contentLoaded;

    public VMCP_Tool()
    {
      this.InitializeComponent();
      this.myPortWindowFunctions = new CommunicationPortWindowFunctions();
      this.configList = new ConfigList(ReadoutConfigFunctions.Manager.GetConnectionProfile(59).GetSettingsList());
      this.configList.MinoConnectBaseState = "RS232_3V";
      this.myPortWindowFunctions.SetReadoutConfiguration(this.configList);
      this.ComboBoxComPort.ItemsSource = (IEnumerable) Constants.GetAvailableComPorts();
      string str = PlugInLoader.GmmConfiguration.GetValue("HandlerLibVmcpTool", "SelectedComPort");
      if (!string.IsNullOrEmpty(str))
      {
        for (int index = 0; index < this.ComboBoxComPort.Items.Count; ++index)
        {
          string fromComInfoString = this.GetComPortFromComInfoString(this.ComboBoxComPort.Items[index].ToString());
          if (str == fromComInfoString)
          {
            this.ComboBoxComPort.SelectedIndex = index;
            break;
          }
        }
      }
      if (this.ComboBoxComPort.SelectedIndex < 0)
        this.ComboBoxComPort.SelectedIndex = 0;
      bool flag = this.IsProtocolChecksumOk(this.SetIdProtocol);
      flag = this.IsProtocolChecksumOk(this.SetDN50_UnitsProtocol);
      flag = this.IsCycleProtocolChecksumOk(this.VmcpDemoProtocol);
      this.ButtonBreak.IsEnabled = false;
      this.AddStatusTimeLine("Tool loaded");
      this.UpdateStatus();
    }

    private void ComboBoxComPort_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      string fromComInfoString = this.GetComPortFromComInfoString(this.ComboBoxComPort.SelectedItem.ToString());
      if (fromComInfoString == null || this.configList.Port != null && !(this.configList.Port != fromComInfoString))
        return;
      this.configList.Port = fromComInfoString;
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("HandlerLibVmcpTool", "SelectedComPort", fromComInfoString);
    }

    private string GetComPortFromComInfoString(string comInfoString)
    {
      string[] strArray = comInfoString.Split(' ');
      return strArray != null && strArray.Length != 0 && !string.IsNullOrEmpty(strArray[0]) ? strArray[0] : (string) null;
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (sender == this.ButtonReceiveCycle)
        {
          this.BreakLoop = false;
          this.ButtonReceiveCycle.IsEnabled = false;
          this.ButtonBreak.IsEnabled = true;
          await Task.Run((Action) (() => this.RunReceiveLoop()));
          this.ButtonReceiveCycle.IsEnabled = true;
          this.ButtonBreak.IsEnabled = false;
        }
        else if (sender == this.ButtonRequestIdentification)
        {
          this.TransmitProtocolData = this.Get_Identification_Protocol;
          this.TransmitProtocolName = "Get Identification";
          this.TransmitProtocol = true;
        }
        else if (sender == this.ButtonSetID)
        {
          uint theID = uint.Parse(this.TextBoxNewId.Text, NumberStyles.HexNumber);
          Buffer.BlockCopy((Array) BitConverter.GetBytes(theID), 0, (Array) this.SetIdProtocol, 9, 4);
          this.SetProtocolChecksum(this.SetIdProtocol);
          this.TransmitProtocolData = this.SetIdProtocol;
          this.TransmitProtocolName = "Set device ID";
          this.TransmitProtocol = true;
        }
        else if (sender == this.ButtonSetVmcpCycle)
        {
          int cycleTime = int.Parse(this.TextBoxVmcpCycle.Text);
          this.Set_CycleTime_Protocol[this.Set_CycleTime_Protocol.Length - 3] = (byte) cycleTime;
          this.SetProtocolChecksum(this.Set_CycleTime_Protocol);
          this.TransmitProtocolData = this.Set_CycleTime_Protocol;
          this.TransmitProtocolName = "Set cycle time = " + cycleTime.ToString();
          this.TransmitProtocol = true;
        }
        else
        {
          if (sender != this.ButtonSetDN50Calibration)
            return;
          this.TransmitProtocolData = this.SetDN50_UnitsProtocol;
          this.TransmitProtocolName = "Set DN50 Units";
          this.TransmitProtocol = true;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
        this.BreakLoop = true;
        this.ButtonReceiveCycle.IsEnabled = true;
        this.ButtonBreak.IsEnabled = false;
      }
    }

    private void AddStatusTimeLine(string statusInfo)
    {
      this.StartStatusTimeLine(statusInfo);
      this.StatusInfo.AppendLine();
    }

    private void StartStatusTimeLine(string statusInfo)
    {
      DateTime now = DateTime.Now;
      DateTime lastCycleTime = this.lastCycleTime;
      if (false)
        this.lastCycleTime = now;
      TimeSpan timeSpan = now - this.lastCycleTime;
      this.lastCycleTime = now;
      this.StatusInfo.Append(timeSpan.TotalSeconds.ToString("000.000"));
      this.StatusInfo.Append("; " + DateTime.Now.ToString("HH:mm:ss.FFF") + ": ");
      this.StatusInfo.Append(statusInfo);
    }

    private void UpdateStatus() => this.UpdateStatusD((object) this, 0);

    private void UpdateStatusD(object sender, int dummy)
    {
      if (this.Dispatcher.Thread != Thread.CurrentThread)
      {
        try
        {
          this.Dispatcher.BeginInvoke((Delegate) new EventHandler<int>(this.UpdateStatusD), sender, (object) dummy);
        }
        catch
        {
        }
      }
      else
      {
        lock (this.StatusInfo)
        {
          string source = this.StatusInfo.ToString();
          int num1 = source.Count<char>((Func<char, bool>) (x => x == '\n'));
          int num2 = 0;
          for (; num1 > 20; --num1)
            num2 = source.IndexOf(Environment.NewLine, num2) + Environment.NewLine.Length;
          if (num2 > 0)
          {
            this.StatusInfo.Remove(0, num2);
            source = this.StatusInfo.ToString();
          }
          this.TextBoxStatus.Text = source;
        }
      }
    }

    private void RunReceiveLoop()
    {
      this.myPortWindowFunctions.portFunctions.Open();
      while (!this.BreakLoop)
      {
        try
        {
          if (this.TransmitProtocol)
          {
            lock (this.StatusInfo)
              this.AddStatusTimeLine(this.TransmitProtocolName);
            this.myPortWindowFunctions.portFunctions.Write(this.TransmitProtocolData);
            this.TransmitProtocol = false;
            Thread.Sleep(10);
          }
          else
          {
            byte num1 = this.myPortWindowFunctions.portFunctions.ReadHeader(1)[0];
            switch (num1)
            {
              case 91:
                byte[] dst = new byte[19];
                dst[0] = num1;
                Buffer.BlockCopy((Array) this.myPortWindowFunctions.portFunctions.ReadEnd(18), 0, (Array) dst, 1, 18);
                lock (this.StatusInfo)
                {
                  this.StartStatusTimeLine("Cycle");
                  byte num2 = 0;
                  for (int index = 0; index < 18; ++index)
                    num2 += dst[index];
                  if ((int) num2 == (int) dst[18])
                  {
                    this.StatusInfo.Append("; SN:" + BitConverter.ToUInt32(dst, 1).ToString("x08"));
                    double num3 = 0.0;
                    double num4 = 1E-06;
                    for (int index = 5; index <= 11; ++index)
                    {
                      double num5 = num3 + (double) ((int) dst[index] & 15) * num4;
                      double num6 = num4 * 10.0;
                      num3 = num5 + (double) (((int) dst[index] & 240) >> 4) * num6;
                      num4 = num6 * 10.0;
                    }
                    this.StatusInfo.Append("; Vol:" + num3.ToString() + "m\u00B3");
                    double num7 = 0.0;
                    double num8 = 0.001;
                    for (int index = 12; index <= 15; ++index)
                    {
                      double num9 = num7 + (double) ((int) dst[index] & 15) * num8;
                      double num10 = num8 * 10.0;
                      num7 = num9 + (double) (((int) dst[index] & 240) >> 4) * num10;
                      num8 = num10 * 10.0;
                    }
                    this.StatusInfo.Append("; Flow:" + num7.ToString() + "m\u00B3/h");
                    ushort uint16 = BitConverter.ToUInt16(dst, 16);
                    if (((uint) uint16 & 1U) > 0U)
                      this.StatusInfo.Append("; Backflow");
                    if (((uint) uint16 & 4U) > 0U)
                      this.StatusInfo.Append("; Undervoltage");
                    if (((uint) uint16 & 16U) > 0U)
                      this.StatusInfo.Append("; PipeEmpty");
                    if (((uint) uint16 & 32U) > 0U)
                      this.StatusInfo.Append("; TransducerAlarm");
                  }
                  else
                    this.StatusInfo.Append("; Checksum error");
                  this.StatusInfo.AppendLine();
                }
                this.UpdateStatus();
                break;
              case 104:
                byte[] numArray = new byte[15];
                numArray[0] = num1;
                Buffer.BlockCopy((Array) this.myPortWindowFunctions.portFunctions.ReadEnd(14), 0, (Array) numArray, 1, 14);
                lock (this.StatusInfo)
                {
                  this.StartStatusTimeLine("Get_Identification received");
                  if (this.IsProtocolChecksumOk(numArray))
                    this.StatusInfo.Append("; SN:" + BitConverter.ToUInt32(numArray, 9).ToString("x08"));
                  else
                    this.StatusInfo.Append("; Checksum error");
                  this.StatusInfo.AppendLine();
                }
                this.UpdateStatus();
                break;
            }
          }
        }
        catch (TimeoutException ex)
        {
        }
        catch (Exception ex)
        {
          this.StartStatusTimeLine("Exception !!!! -> " + ex.Message);
          this.StatusInfo.AppendLine();
        }
      }
      this.myPortWindowFunctions.portFunctions.Close();
    }

    private bool IsCycleProtocolChecksumOk(byte[] protocol)
    {
      byte num = 0;
      for (int index = 0; index < protocol.Length - 1; ++index)
        num += protocol[index];
      return (int) protocol[protocol.Length - 1] == (int) num;
    }

    private bool IsProtocolChecksumOk(byte[] protocol)
    {
      byte protocolChecksum = this.CalculateProtocolChecksum(protocol);
      return (int) protocol[protocol.Length - 2] == (int) protocolChecksum;
    }

    private void SetProtocolChecksum(byte[] protocol)
    {
      byte protocolChecksum = this.CalculateProtocolChecksum(protocol);
      protocol[protocol.Length - 2] = protocolChecksum;
    }

    private byte CalculateProtocolChecksum(byte[] protocol)
    {
      byte protocolChecksum = 0;
      for (int index = 4; index < protocol.Length - 2; ++index)
        protocolChecksum += protocol[index];
      return protocolChecksum;
    }

    private void ButtonBreak_Click(object sender, RoutedEventArgs e) => this.BreakLoop = true;

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/vmcp_tool.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.gmmCorporateControl1 = (GmmCorporateControl) target;
          break;
        case 2:
          this.ComboBoxComPort = (ComboBox) target;
          this.ComboBoxComPort.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxComPort_SelectionChanged);
          break;
        case 3:
          this.StackPanelButtons = (StackPanel) target;
          break;
        case 4:
          this.ButtonReceiveCycle = (Button) target;
          this.ButtonReceiveCycle.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 5:
          this.ButtonRequestIdentification = (Button) target;
          this.ButtonRequestIdentification.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 6:
          this.TextBoxVmcpCycle = (TextBox) target;
          break;
        case 7:
          this.ButtonSetVmcpCycle = (Button) target;
          this.ButtonSetVmcpCycle.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 8:
          this.TextBoxNewId = (TextBox) target;
          break;
        case 9:
          this.ButtonSetID = (Button) target;
          this.ButtonSetID.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 10:
          this.ButtonSetDN50Calibration = (Button) target;
          this.ButtonSetDN50Calibration.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 11:
          this.ButtonBreak = (Button) target;
          this.ButtonBreak.Click += new RoutedEventHandler(this.ButtonBreak_Click);
          break;
        case 12:
          this.TextBoxStatus = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
