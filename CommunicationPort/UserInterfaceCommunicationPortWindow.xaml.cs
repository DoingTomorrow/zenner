// Decompiled with JetBrains decompiler
// Type: CommunicationPort.UserInterface.CommunicationPortWindow
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using CommonWPF;
using CommunicationPort.Functions;
using GmmDbLib;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace CommunicationPort.UserInterface
{
  public partial class CommunicationPortWindow : Window, IComponentConnector
  {
    private const string otKey = "ComPortUI_";
    private static int[] BaudrateTable = new int[10]
    {
      300,
      600,
      1200,
      2400,
      4800,
      9600,
      19200,
      38400,
      57600,
      115200
    };
    private static string[] ParityTable = new string[3]
    {
      "None",
      "Even",
      "Odd"
    };
    private static List<CommunicationPortWindow.MiConSetupElement> MiConSetupList;
    private static string[] WakeupTable = new string[3]
    {
      "None",
      "BaudrateCarrier",
      "Break"
    };
    private static string[] EchoTable = new string[2]
    {
      "None",
      "Suppress echo"
    };
    private CommunicationPortWindowFunctions myWindowFunctions;
    internal string NextPlugin = "";
    private string PortTypeCom;
    private string PortTypeMinoConnect;
    private string PortTypeRemote;
    private CommunicationPortFunctions thePort;
    private bool controlInit = true;
    internal Menu menuMain;
    internal MenuItem MenuItemComponents;
    internal MenuItem MenuItemMiConBLE_TEST;
    internal StartupLib.GmmCorporateControl gmmCorporateControl1;
    internal GroupBox GroupBoxComState;
    internal TextBlock TextBlockComState;
    internal GroupBox GroupBoxDeviceInof;
    internal TextBox TextBoxDeviceInfo;
    internal StackPanel StackPanalSetup;
    internal Grid GridPortType;
    internal Label LabelPortType;
    internal ComboBox ComboBoxPortType;
    internal Grid GridPort;
    internal Label LabelPort;
    internal ComboBox ComboBoxPort;
    internal Grid GridBaudrateParity;
    internal Label LabelBaudrate;
    internal ComboBox ComboBoxBaudrate;
    internal Label LabelParity;
    internal ComboBox ComboBoxParity;
    internal Grid GridWakeupEcho;
    internal Label LabelWakeup;
    internal ComboBox ComboBoxWakeup;
    internal Label LabelEcho;
    internal ComboBox ComboBoxEcho;
    internal Grid GridMiConSetup;
    internal Label LabelMiConSetup;
    internal ComboBox ComboBoxMiConSetup;
    internal Button ButtonOpenClose;
    internal Button ButtonTiming;
    internal TextBox TextBoxChannel;
    internal TextBlock TextBlockMessages;
    internal TextBlock TextBlockAlive;
    private bool _contentLoaded;

    private static void AddSetupElement(
      CommunicationPortWindow.MiConSetupEnum theEnum,
      string theText)
    {
      CommunicationPortWindow.MiConSetupList.Add(new CommunicationPortWindow.MiConSetupElement()
      {
        SetupEnum = theEnum,
        SetupText = theText
      });
    }

    static CommunicationPortWindow()
    {
      CommunicationPortWindow.MiConSetupList = new List<CommunicationPortWindow.MiConSetupElement>();
      CommunicationPortWindow.AddSetupElement(CommunicationPortWindow.MiConSetupEnum.IrDaCombiHeadRoundSideInfrared, "IrDaCombiHead round side infrared");
      CommunicationPortWindow.AddSetupElement(CommunicationPortWindow.MiConSetupEnum.IrDaCombiHeadRoundSideIrDA, "IrDaCombiHead round side IrDA");
      CommunicationPortWindow.AddSetupElement(CommunicationPortWindow.MiConSetupEnum.IrDaCombiHeadDoveTailSideIrDA, "IrDaCombiHead dove tail side IrDA");
      CommunicationPortWindow.AddSetupElement(CommunicationPortWindow.MiConSetupEnum.ZIN_CombiHead_RoundSideZVEI, "ZIN_CombiHead round side infrared");
      CommunicationPortWindow.AddSetupElement(CommunicationPortWindow.MiConSetupEnum.ZIN_CombiHead_RoundSideIrDA, "ZIN_CombiHead round side side IrDA");
      CommunicationPortWindow.AddSetupElement(CommunicationPortWindow.MiConSetupEnum.ZIN_CombiHead_DoveTailSideIrDA, "ZIN_CombiHead dove tail side IrDA");
      CommunicationPortWindow.AddSetupElement(CommunicationPortWindow.MiConSetupEnum.ZIN_CombiHead_RoundSideNFC, "ZIN_CombiHead round side NFC");
      CommunicationPortWindow.AddSetupElement(CommunicationPortWindow.MiConSetupEnum.Radio2_receive, "Radio2 receive");
      CommunicationPortWindow.AddSetupElement(CommunicationPortWindow.MiConSetupEnum.Radio3_receive, "Radio3 receive");
      CommunicationPortWindow.AddSetupElement(CommunicationPortWindow.MiConSetupEnum.WirelessMBusReceive, "Wireless MBus receive");
      CommunicationPortWindow.AddSetupElement(CommunicationPortWindow.MiConSetupEnum.NDC_MiCon_Module, "NDC MiCon Module");
      CommunicationPortWindow.AddSetupElement(CommunicationPortWindow.MiConSetupEnum.RS232_PowerOff, "RS232 power off");
      CommunicationPortWindow.AddSetupElement(CommunicationPortWindow.MiConSetupEnum.RS232_3_3V, "RS232 3.3V");
      CommunicationPortWindow.AddSetupElement(CommunicationPortWindow.MiConSetupEnum.RS232_7V, "RS232 7V");
      CommunicationPortWindow.AddSetupElement(CommunicationPortWindow.MiConSetupEnum.RS485_PowerOff, "RS485 power off");
      CommunicationPortWindow.AddSetupElement(CommunicationPortWindow.MiConSetupEnum.RS485_3_3V, "RS485 3.3V");
      CommunicationPortWindow.AddSetupElement(CommunicationPortWindow.MiConSetupEnum.RS485_7V, "RS485 7V");
    }

    public CommunicationPortWindow(
      CommunicationPortWindowFunctions myWindowFunctions,
      bool isPlugin)
    {
      this.myWindowFunctions = myWindowFunctions;
      this.InitializeComponent();
      WpfTranslatorSupport.TranslateWindow(Tg.CommunicationPortWindow, (Window) this);
      if (isPlugin)
      {
        MenuItem componentsMenuItem = (MenuItem) this.menuMain.Items[0];
        componentsMenuItem.Visibility = Visibility.Visible;
        UserInterfaceServices.AddDefaultMenu(componentsMenuItem, new RoutedEventHandler(this.componentsClick));
        componentsMenuItem.Items.Add((object) new Separator());
        UserInterfaceServices.AddMenuItem("ReadoutConfiguration", componentsMenuItem, new RoutedEventHandler(this.componentsClick));
      }
      this.PortTypeCom = Ot.Gtt(Tg.Communication_UI, "ComPortUI_PortTypeCom", "COM port");
      this.PortTypeMinoConnect = Ot.Gtt(Tg.Communication_UI, "ComPortUI_PortTypeMinoConnect", "MinoConnect");
      this.PortTypeRemote = Ot.Gtt(Tg.Communication_UI, "ComPortUI_PortTypeRemote", "Remote connection");
      this.ComboBoxPortType.Items.Add((object) this.PortTypeCom);
      this.ComboBoxPortType.Items.Add((object) this.PortTypeMinoConnect);
      this.ComboBoxPortType.Items.Add((object) this.PortTypeRemote);
      switch (this.myWindowFunctions.portFunctions.communicationObject)
      {
        case CommunicationByComPort _:
          this.ComboBoxPortType.SelectedIndex = 0;
          break;
        case CommunicationByMinoConnect _:
          this.ComboBoxPortType.SelectedIndex = 1;
          break;
        default:
          this.ComboBoxPortType.SelectedIndex = 2;
          break;
      }
      this.ShowPort();
      for (int index = 0; index < CommunicationPortWindow.BaudrateTable.Length; ++index)
        this.ComboBoxBaudrate.Items.Add((object) CommunicationPortWindow.BaudrateTable[index].ToString());
      this.ShowBaudrate();
      for (int index = 0; index < CommunicationPortWindow.ParityTable.Length; ++index)
        this.ComboBoxParity.Items.Add((object) CommunicationPortWindow.ParityTable[index]);
      this.ShowParity();
      for (int index = 0; index < CommunicationPortWindow.WakeupTable.Length; ++index)
        this.ComboBoxWakeup.Items.Add((object) CommunicationPortWindow.WakeupTable[index]);
      this.ShowWakeup();
      for (int index = 0; index < CommunicationPortWindow.EchoTable.Length; ++index)
        this.ComboBoxEcho.Items.Add((object) CommunicationPortWindow.EchoTable[index]);
      this.ShowEcho();
      foreach (CommunicationPortWindow.MiConSetupElement miConSetup in CommunicationPortWindow.MiConSetupList)
        this.ComboBoxMiConSetup.Items.Add((object) miConSetup.SetupText);
      this.ShowMiConSetup();
      this.myWindowFunctions.portFunctions.OnAliveEvent += new EventHandler<int>(this.AliveEventReceived);
      this.myWindowFunctions.portFunctions.OnMessageEvent += new EventHandler<string>(this.MessageEventReceived);
      this.ShowCurrentState();
      this.controlInit = false;
    }

    public string ReadingChannelIdentification
    {
      get => this.myWindowFunctions.portFunctions.configList.ReadingChannelIdentification;
      set => this.myWindowFunctions.portFunctions.configList.ReadingChannelIdentification = value;
    }

    private void componentsClick(object sender, RoutedEventArgs e)
    {
      this.NextPlugin = ((HeaderedItemsControl) sender).Header.ToString();
      this.Close();
    }

    private void AliveEventReceived(object sender, int aliveCounter)
    {
      if (this.Dispatcher.Thread != Thread.CurrentThread)
      {
        try
        {
          this.Dispatcher.BeginInvoke((Delegate) new EventHandler<int>(this.AliveEventReceived), sender, (object) aliveCounter);
        }
        catch
        {
        }
      }
      else
        this.TextBlockAlive.Text = aliveCounter.ToString();
    }

    private void MessageEventReceived(object sender, string theMessage)
    {
      if (this.Dispatcher.Thread != Thread.CurrentThread)
      {
        try
        {
          this.Dispatcher.BeginInvoke((Delegate) new EventHandler<string>(this.MessageEventReceived), sender, (object) theMessage);
        }
        catch
        {
        }
      }
      else
      {
        if (Ot.GetMessageNumberFromLanguageText(theMessage).HasValue)
          theMessage = Ot.GetMessageTextWithoutNumber(theMessage);
        this.TextBlockMessages.Text = theMessage;
        this.ShowCurrentState();
      }
    }

    private void ComboBoxPortType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.controlInit || this.ComboBoxPortType.SelectedIndex < 0)
        return;
      if (this.ComboBoxPortType.SelectedIndex == 0)
        this.myWindowFunctions.portFunctions.PortType = PortTypes.COM;
      else if (this.ComboBoxPortType.SelectedIndex == 1)
      {
        this.myWindowFunctions.portFunctions.PortType = PortTypes.MinoConnect;
      }
      else
      {
        if (this.ComboBoxPortType.SelectedIndex != 2)
          return;
        this.myWindowFunctions.portFunctions.PortType = PortTypes.AsynchronIP;
      }
      this.ShowMiConSetup();
    }

    private void ComboBoxPort_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.controlInit || this.ComboBoxPort.SelectedIndex < 0)
        return;
      if (this.ComboBoxPort.SelectedItem is ValueItem selectedItem)
      {
        this.myWindowFunctions.portFunctions.configList.Port = selectedItem.Value;
        if (selectedItem.Value.StartsWith("Mi"))
          this.MenuItemMiConBLE_TEST.IsEnabled = true;
        else
          this.MenuItemMiConBLE_TEST.IsEnabled = false;
      }
      else
        this.ShowPort();
    }

    private void ComboBoxPort_DropDownOpened(object sender, EventArgs e)
    {
      List<ValueItem> availableComPorts = Constants.GetAvailableComPorts();
      ValueItem selectedItem = this.ComboBoxPort.SelectedItem as ValueItem;
      string selectedText = this.ComboBoxPort.Text;
      if (this.ComboBoxPort.ItemsSource == null)
        this.ComboBoxPort.Items.Clear();
      this.ComboBoxPort.ItemsSource = (IEnumerable) availableComPorts;
      if (selectedItem != null)
      {
        int index = availableComPorts.FindIndex((Predicate<ValueItem>) (item => item.Value == selectedItem.Value));
        if (index < 0)
          return;
        this.ComboBoxPort.SelectedIndex = index;
      }
      else
      {
        if (string.IsNullOrEmpty(selectedText))
          return;
        int index = availableComPorts.FindIndex((Predicate<ValueItem>) (item => item.Value == selectedText));
        if (index >= 0)
          this.ComboBoxPort.SelectedIndex = index;
      }
    }

    private void ComboBoxBaudrate_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.controlInit || this.ComboBoxBaudrate.SelectedItem == null || this.myWindowFunctions.portFunctions.communicationObject == null)
        return;
      this.myWindowFunctions.portFunctions.configList.Baudrate = int.Parse(this.ComboBoxBaudrate.SelectedItem.ToString());
    }

    private void ComboBoxParity_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.controlInit || this.ComboBoxParity.SelectedItem == null || this.myWindowFunctions.portFunctions.communicationObject == null)
        return;
      this.myWindowFunctions.portFunctions.communicationObject.Parity = (Parity) Enum.Parse(typeof (Parity), this.ComboBoxParity.SelectedItem.ToString());
    }

    private void ComboBoxEcho_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.controlInit || this.ComboBoxEcho.SelectedItem == null || this.myWindowFunctions.portFunctions.communicationObject == null)
        return;
      this.myWindowFunctions.portFunctions.configList.EchoOn = !(this.ComboBoxEcho.SelectedItem.ToString() == "None");
    }

    private void ComboBoxWakeup_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.controlInit || this.ComboBoxWakeup.SelectedItem == null || this.myWindowFunctions.portFunctions.communicationObject == null)
        return;
      this.myWindowFunctions.portFunctions.communicationObject.Wakeup = (WakeupSystem) Enum.Parse(typeof (WakeupSystem), this.ComboBoxWakeup.SelectedItem.ToString());
    }

    private void ComboBoxMiConSetup_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.controlInit || this.ComboBoxMiConSetup.SelectedIndex < 0)
        return;
      CommunicationByMinoConnect communicationObject = this.myWindowFunctions.portFunctions.communicationObject as CommunicationByMinoConnect;
      communicationObject.IrDaSelection = IrDaSelection.None;
      communicationObject.CombiHeadSelection = CombiHeadSelection.UART;
      switch (CommunicationPortWindow.MiConSetupList[this.ComboBoxMiConSetup.SelectedIndex].SetupEnum)
      {
        case CommunicationPortWindow.MiConSetupEnum.IrDaCombiHeadRoundSideInfrared:
          communicationObject.MinoConnectBaseState = MinoConnectBaseStates.IrCombiHead;
          break;
        case CommunicationPortWindow.MiConSetupEnum.IrDaCombiHeadDoveTailSideIrDA:
          communicationObject.MinoConnectBaseState = MinoConnectBaseStates.IrCombiHead;
          communicationObject.IrDaSelection = IrDaSelection.DoveTailSide;
          break;
        case CommunicationPortWindow.MiConSetupEnum.IrDaCombiHeadRoundSideIrDA:
          communicationObject.MinoConnectBaseState = MinoConnectBaseStates.IrCombiHead;
          communicationObject.IrDaSelection = IrDaSelection.RoundSide;
          break;
        case CommunicationPortWindow.MiConSetupEnum.RS232_PowerOff:
          communicationObject.MinoConnectBaseState = MinoConnectBaseStates.RS232;
          break;
        case CommunicationPortWindow.MiConSetupEnum.RS232_3_3V:
          communicationObject.MinoConnectBaseState = MinoConnectBaseStates.RS232_3V;
          break;
        case CommunicationPortWindow.MiConSetupEnum.RS232_7V:
          communicationObject.MinoConnectBaseState = MinoConnectBaseStates.RS232_7V;
          break;
        case CommunicationPortWindow.MiConSetupEnum.RS485_PowerOff:
          communicationObject.MinoConnectBaseState = MinoConnectBaseStates.RS485;
          break;
        case CommunicationPortWindow.MiConSetupEnum.RS485_3_3V:
          communicationObject.MinoConnectBaseState = MinoConnectBaseStates.RS485_3V;
          break;
        case CommunicationPortWindow.MiConSetupEnum.RS485_7V:
          communicationObject.MinoConnectBaseState = MinoConnectBaseStates.RS485_7V;
          break;
        case CommunicationPortWindow.MiConSetupEnum.Radio2_receive:
          communicationObject.MinoConnectBaseState = MinoConnectBaseStates.Radio2Receive;
          break;
        case CommunicationPortWindow.MiConSetupEnum.Radio3_receive:
          communicationObject.MinoConnectBaseState = MinoConnectBaseStates.Radio3Receive;
          break;
        case CommunicationPortWindow.MiConSetupEnum.WirelessMBusReceive:
          communicationObject.MinoConnectBaseState = MinoConnectBaseStates.WirelessMBus;
          break;
        case CommunicationPortWindow.MiConSetupEnum.NDC_MiCon_Module:
          communicationObject.MinoConnectBaseState = MinoConnectBaseStates.NDC_MiCon_Module;
          break;
        case CommunicationPortWindow.MiConSetupEnum.ZIN_CombiHead_RoundSideZVEI:
          communicationObject.MinoConnectBaseState = MinoConnectBaseStates.ZIN_CombiHead;
          communicationObject.CombiHeadSelection = CombiHeadSelection.UART;
          break;
        case CommunicationPortWindow.MiConSetupEnum.ZIN_CombiHead_RoundSideIrDA:
          communicationObject.MinoConnectBaseState = MinoConnectBaseStates.ZIN_CombiHead;
          communicationObject.CombiHeadSelection = CombiHeadSelection.IrDa_RoundSide;
          break;
        case CommunicationPortWindow.MiConSetupEnum.ZIN_CombiHead_DoveTailSideIrDA:
          communicationObject.MinoConnectBaseState = MinoConnectBaseStates.ZIN_CombiHead;
          communicationObject.CombiHeadSelection = CombiHeadSelection.IrDA_DoveTailSide;
          break;
        case CommunicationPortWindow.MiConSetupEnum.ZIN_CombiHead_RoundSideNFC:
          communicationObject.MinoConnectBaseState = MinoConnectBaseStates.ZIN_CombiHead;
          communicationObject.CombiHeadSelection = CombiHeadSelection.NFC;
          break;
      }
    }

    private void ButtonTiming_Click(object sender, RoutedEventArgs e)
    {
      new TimingWindow(this.myWindowFunctions).ShowDialog();
    }

    private void ButtonOpenClose_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.ButtonOpenClose.Content.ToString() == "Open")
          this.myWindowFunctions.portFunctions.Open();
        else
          this.myWindowFunctions.portFunctions.Close();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Open error");
      }
      this.ShowCurrentState();
    }

    private void ShowPort()
    {
      if (this.myWindowFunctions.portFunctions.communicationObject == null)
        return;
      string port = this.myWindowFunctions.portFunctions.configList.Port;
      if (port.StartsWith("Mi"))
        this.MenuItemMiConBLE_TEST.IsEnabled = true;
      else
        this.MenuItemMiConBLE_TEST.IsEnabled = false;
      for (int index = 0; index < this.ComboBoxPort.Items.Count; ++index)
      {
        if ((this.ComboBoxPort.Items[index] as ValueItem).Value == port)
        {
          this.ComboBoxPort.SelectedIndex = index;
          return;
        }
      }
      this.ComboBoxPort.Items.Add((object) port);
      this.ComboBoxPort.SelectedIndex = 0;
    }

    private void ShowBaudrate()
    {
      if (this.myWindowFunctions.portFunctions.communicationObject == null)
        return;
      if (this.ComboBoxBaudrate.Items.Count > 0)
      {
        for (int index = 0; index < this.ComboBoxBaudrate.Items.Count; ++index)
        {
          if (this.ComboBoxBaudrate.Items[index].ToString() == this.myWindowFunctions.portFunctions.configList.Baudrate.ToString())
          {
            this.ComboBoxBaudrate.SelectedIndex = index;
            return;
          }
        }
      }
      this.ComboBoxBaudrate.SelectedIndex = -1;
    }

    private void ShowParity()
    {
      if (this.myWindowFunctions.portFunctions.communicationObject == null)
        return;
      if (this.ComboBoxParity.Items.Count > 0)
      {
        for (int index = 0; index < this.ComboBoxParity.Items.Count; ++index)
        {
          if (this.ComboBoxParity.Items[index].ToString() == this.myWindowFunctions.portFunctions.communicationObject.Parity.ToString())
          {
            this.ComboBoxParity.SelectedIndex = index;
            return;
          }
        }
      }
      this.ComboBoxParity.SelectedIndex = -1;
    }

    private void ShowWakeup()
    {
      if (this.myWindowFunctions.portFunctions.communicationObject == null)
        return;
      if (this.ComboBoxWakeup.Items.Count > 0)
      {
        for (int index = 0; index < this.ComboBoxWakeup.Items.Count; ++index)
        {
          if (this.ComboBoxWakeup.Items[index].ToString() == this.myWindowFunctions.portFunctions.communicationObject.Wakeup.ToString())
          {
            this.ComboBoxWakeup.SelectedIndex = index;
            return;
          }
        }
      }
      this.ComboBoxWakeup.SelectedIndex = -1;
    }

    private void ShowEcho()
    {
      if (this.myWindowFunctions.portFunctions.communicationObject == null)
        return;
      if (this.ComboBoxEcho.Items.Count > 0)
      {
        if (this.myWindowFunctions.portFunctions.configList.EchoOn)
          this.ComboBoxEcho.SelectedIndex = 1;
        else
          this.ComboBoxEcho.SelectedIndex = 0;
      }
      else
        this.ComboBoxEcho.SelectedIndex = -1;
    }

    private void ShowMiConSetup()
    {
      if (this.myWindowFunctions.portFunctions.communicationObject is CommunicationByMinoConnect)
      {
        this.GridMiConSetup.Visibility = Visibility.Visible;
        CommunicationByMinoConnect communicationObject = this.myWindowFunctions.portFunctions.communicationObject as CommunicationByMinoConnect;
        if (communicationObject.MinoConnectBaseState >= MinoConnectBaseStates.undefined || communicationObject.MinoConnectBaseState <= MinoConnectBaseStates.off)
        {
          this.ComboBoxMiConSetup.SelectedIndex = -1;
        }
        else
        {
          switch (communicationObject.MinoConnectBaseState)
          {
            case MinoConnectBaseStates.IrCombiHead:
              switch (communicationObject.IrDaSelection)
              {
                case IrDaSelection.None:
                  this.SetMiConIndex(CommunicationPortWindow.MiConSetupEnum.IrDaCombiHeadRoundSideInfrared);
                  break;
                case IrDaSelection.DoveTailSide:
                  this.SetMiConIndex(CommunicationPortWindow.MiConSetupEnum.IrDaCombiHeadDoveTailSideIrDA);
                  break;
                case IrDaSelection.RoundSide:
                  this.SetMiConIndex(CommunicationPortWindow.MiConSetupEnum.IrDaCombiHeadRoundSideInfrared);
                  break;
                default:
                  this.ComboBoxMiConSetup.SelectedIndex = -1;
                  break;
              }
              break;
            case MinoConnectBaseStates.RS232:
              this.SetMiConIndex(CommunicationPortWindow.MiConSetupEnum.RS232_PowerOff);
              break;
            case MinoConnectBaseStates.RS232_3V:
              this.SetMiConIndex(CommunicationPortWindow.MiConSetupEnum.RS232_3_3V);
              break;
            case MinoConnectBaseStates.RS232_7V:
              this.SetMiConIndex(CommunicationPortWindow.MiConSetupEnum.RS232_7V);
              break;
            case MinoConnectBaseStates.RS485:
              this.SetMiConIndex(CommunicationPortWindow.MiConSetupEnum.RS485_PowerOff);
              break;
            case MinoConnectBaseStates.RS485_3V:
              this.SetMiConIndex(CommunicationPortWindow.MiConSetupEnum.RS485_3_3V);
              break;
            case MinoConnectBaseStates.RS485_7V:
              this.SetMiConIndex(CommunicationPortWindow.MiConSetupEnum.RS485_7V);
              break;
            case MinoConnectBaseStates.Radio2Receive:
              this.SetMiConIndex(CommunicationPortWindow.MiConSetupEnum.Radio2_receive);
              break;
            case MinoConnectBaseStates.Radio3Receive:
              this.SetMiConIndex(CommunicationPortWindow.MiConSetupEnum.Radio3_receive);
              break;
            case MinoConnectBaseStates.WirelessMBus:
              this.SetMiConIndex(CommunicationPortWindow.MiConSetupEnum.WirelessMBusReceive);
              break;
            case MinoConnectBaseStates.NDC_MiCon_Module:
              this.SetMiConIndex(CommunicationPortWindow.MiConSetupEnum.NDC_MiCon_Module);
              break;
            case MinoConnectBaseStates.ZIN_CombiHead:
              switch (communicationObject.CombiHeadSelection)
              {
                case CombiHeadSelection.UART:
                  this.SetMiConIndex(CommunicationPortWindow.MiConSetupEnum.ZIN_CombiHead_RoundSideZVEI);
                  break;
                case CombiHeadSelection.IrDA_DoveTailSide:
                  this.SetMiConIndex(CommunicationPortWindow.MiConSetupEnum.ZIN_CombiHead_DoveTailSideIrDA);
                  break;
                case CombiHeadSelection.IrDa_RoundSide:
                  this.SetMiConIndex(CommunicationPortWindow.MiConSetupEnum.ZIN_CombiHead_RoundSideIrDA);
                  break;
                case CombiHeadSelection.NFC:
                  this.SetMiConIndex(CommunicationPortWindow.MiConSetupEnum.ZIN_CombiHead_RoundSideNFC);
                  break;
                default:
                  this.ComboBoxMiConSetup.SelectedIndex = -1;
                  break;
              }
              break;
            default:
              this.ComboBoxMiConSetup.SelectedIndex = -1;
              break;
          }
        }
      }
      else
        this.GridMiConSetup.Visibility = Visibility.Collapsed;
    }

    private void SetMiConIndex(CommunicationPortWindow.MiConSetupEnum setupEnum)
    {
      this.ComboBoxMiConSetup.SelectedIndex = CommunicationPortWindow.MiConSetupList.FindIndex((Predicate<CommunicationPortWindow.MiConSetupElement>) (item => item.SetupEnum == setupEnum));
    }

    private void ShowCurrentState()
    {
      this.thePort = this.myWindowFunctions.portFunctions;
      if (this.thePort.IsOpen)
        this.ButtonOpenClose.Content = (object) "Close";
      else
        this.ButtonOpenClose.Content = (object) "Open";
      this.TextBlockComState.Text = this.myWindowFunctions.portFunctions.communicationObject.ToString();
      this.TextBoxDeviceInfo.Text = !(this.myWindowFunctions.portFunctions.communicationObject is CommunicationByMinoConnect) ? "" : ((CommunicationByMinoConnect) this.myWindowFunctions.portFunctions.communicationObject).GetDeviceInfo();
      this.EnableControls(this.thePort.IsOpen);
    }

    private void EnableControls(bool isOpen)
    {
      this.StackPanalSetup.IsEnabled = !isOpen;
      this.ButtonTiming.IsEnabled = !isOpen;
    }

    private void MiConBLE_Test_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        ulong BTMAC = 0;
        if (this.ComboBoxPort.SelectedItem != null)
        {
          if (this.ComboBoxPort.SelectedItem is ValueItem)
          {
            ValueItem selectedItem = this.ComboBoxPort.SelectedItem as ValueItem;
            if (selectedItem.AdditionalInfo.ContainsKey(AdditionalInfoKey.BluetoothAddress))
              BTMAC = ulong.Parse(selectedItem.AdditionalInfo[AdditionalInfoKey.BluetoothAddress], NumberStyles.HexNumber);
          }
          else if (this.ComboBoxPort.SelectedItem is string)
          {
            string selectedItem = (string) this.ComboBoxPort.SelectedItem;
            if (selectedItem.StartsWith("Mi"))
              BTMAC = Constants.GetMiConBLE_DeviceFromPort(selectedItem).BluetoothAddress;
          }
        }
        BluetoothChannel_LE BLE_Channel = (BluetoothChannel_LE) null;
        try
        {
          BLE_Channel = (BluetoothChannel_LE) this.myWindowFunctions.portFunctions.GetCommunicationByMinoConnect().channel;
          if (BLE_Channel != null)
            BTMAC = BLE_Channel.BTMAC;
        }
        catch
        {
        }
        MiConTestWindow miConTestWindow = new MiConTestWindow(this.myWindowFunctions, BTMAC, BLE_Channel);
        miConTestWindow.Owner = (Window) this;
        miConTestWindow.Show();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/CommunicationPort;component/userinterface/communicationportwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.menuMain = (Menu) target;
          break;
        case 2:
          this.MenuItemComponents = (MenuItem) target;
          break;
        case 3:
          this.MenuItemMiConBLE_TEST = (MenuItem) target;
          this.MenuItemMiConBLE_TEST.Click += new RoutedEventHandler(this.MiConBLE_Test_Click);
          break;
        case 4:
          this.gmmCorporateControl1 = (StartupLib.GmmCorporateControl) target;
          break;
        case 5:
          this.GroupBoxComState = (GroupBox) target;
          break;
        case 6:
          this.TextBlockComState = (TextBlock) target;
          break;
        case 7:
          this.GroupBoxDeviceInof = (GroupBox) target;
          break;
        case 8:
          this.TextBoxDeviceInfo = (TextBox) target;
          break;
        case 9:
          this.StackPanalSetup = (StackPanel) target;
          break;
        case 10:
          this.GridPortType = (Grid) target;
          break;
        case 11:
          this.LabelPortType = (Label) target;
          break;
        case 12:
          this.ComboBoxPortType = (ComboBox) target;
          this.ComboBoxPortType.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxPortType_SelectionChanged);
          break;
        case 13:
          this.GridPort = (Grid) target;
          break;
        case 14:
          this.LabelPort = (Label) target;
          break;
        case 15:
          this.ComboBoxPort = (ComboBox) target;
          this.ComboBoxPort.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxPort_SelectionChanged);
          this.ComboBoxPort.DropDownOpened += new EventHandler(this.ComboBoxPort_DropDownOpened);
          break;
        case 16:
          this.GridBaudrateParity = (Grid) target;
          break;
        case 17:
          this.LabelBaudrate = (Label) target;
          break;
        case 18:
          this.ComboBoxBaudrate = (ComboBox) target;
          this.ComboBoxBaudrate.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxBaudrate_SelectionChanged);
          break;
        case 19:
          this.LabelParity = (Label) target;
          break;
        case 20:
          this.ComboBoxParity = (ComboBox) target;
          this.ComboBoxParity.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxParity_SelectionChanged);
          break;
        case 21:
          this.GridWakeupEcho = (Grid) target;
          break;
        case 22:
          this.LabelWakeup = (Label) target;
          break;
        case 23:
          this.ComboBoxWakeup = (ComboBox) target;
          this.ComboBoxWakeup.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxWakeup_SelectionChanged);
          break;
        case 24:
          this.LabelEcho = (Label) target;
          break;
        case 25:
          this.ComboBoxEcho = (ComboBox) target;
          this.ComboBoxEcho.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxEcho_SelectionChanged);
          break;
        case 26:
          this.GridMiConSetup = (Grid) target;
          break;
        case 27:
          this.LabelMiConSetup = (Label) target;
          break;
        case 28:
          this.ComboBoxMiConSetup = (ComboBox) target;
          this.ComboBoxMiConSetup.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxMiConSetup_SelectionChanged);
          break;
        case 29:
          this.ButtonOpenClose = (Button) target;
          this.ButtonOpenClose.Click += new RoutedEventHandler(this.ButtonOpenClose_Click);
          break;
        case 30:
          this.ButtonTiming = (Button) target;
          this.ButtonTiming.Click += new RoutedEventHandler(this.ButtonTiming_Click);
          break;
        case 31:
          this.TextBoxChannel = (TextBox) target;
          break;
        case 32:
          this.TextBlockMessages = (TextBlock) target;
          break;
        case 33:
          this.TextBlockAlive = (TextBlock) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    internal class MiConSetupElement
    {
      internal CommunicationPortWindow.MiConSetupEnum SetupEnum;
      internal string SetupText;
    }

    internal enum MiConSetupEnum
    {
      IrDaCombiHeadRoundSideInfrared,
      IrDaCombiHeadDoveTailSideIrDA,
      IrDaCombiHeadRoundSideIrDA,
      RS232_PowerOff,
      RS232_3_3V,
      RS232_7V,
      RS485_PowerOff,
      RS485_3_3V,
      RS485_7V,
      Radio2_receive,
      Radio3_receive,
      WirelessMBusReceive,
      NDC_MiCon_Module,
      ZIN_CombiHead_RoundSideZVEI,
      ZIN_CombiHead_RoundSideIrDA,
      ZIN_CombiHead_DoveTailSideIrDA,
      ZIN_CombiHead_RoundSideNFC,
    }
  }
}
