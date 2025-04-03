// Decompiled with JetBrains decompiler
// Type: CommunicationPort.UserInterface.MiConTestWindow
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using CommonWPF;
using Microsoft.Win32;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Windows.Devices.Enumeration;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace CommunicationPort.UserInterface
{
  public partial class MiConTestWindow : Window, IComponentConnector
  {
    internal BluetoothChannel_LE BLE_Channel;
    internal CommunicationPortWindowFunctions MyWindowFunctions;
    private string TestPort;
    private SerialPort SerialTestPort;
    private ObservableCollection<BluetoothLEDeviceDisplay> KnownDevices = new ObservableCollection<BluetoothLEDeviceDisplay>();
    private List<DeviceInformation> UnknownDevices = new List<DeviceInformation>();
    private string SingleIndentString = "   ";
    private string IndentString = string.Empty;
    private int AsciiCommandStartIndex = -1;
    private Queue<byte> BluetoothReceiveQueue = new Queue<byte>();
    private List<string> CommandHistory = new List<string>();
    private int CommandHistoryIndex = 0;
    private Random BcTestCharacters = new Random();
    private Random BcTestWait = new Random();
    private Random BcTestDataCount = new Random();
    private Random BcTestData = new Random();
    private bool AutoTestOn = false;
    private ConcurrentQueue<byte> AutoReceiveQueue;
    private bool breakLoop;
    internal TabItem TabItemBasicCommands;
    internal StackPanel StackPanelBoxes;
    internal TextBox TextBoxOwnBLMAC;
    internal TextBox TextBoxMiConBLMAC;
    internal GroupBox GroupBoxInsertCommandTemplates;
    internal Button ButtonTypeCom115200;
    internal Button ButtonTypeBcTest;
    internal StackPanel StackPanelButtons;
    internal Button ButtonClear;
    internal Button ButtonConnectClose;
    internal TabItem TabItemAutoTests;
    internal Button ButtonBcRandomTest;
    internal Button ButtonBcRandomLoop;
    internal Button ButtonBreak;
    internal ComboBox ComboBoxTestComPort;
    internal Button ButtonTestCOM_open;
    internal Button ButtonTestCOM_open96;
    internal Button ButtonTestCOM_close;
    internal Button ButtonTestCOM_check;
    internal Button ButtonLoadHTermCommandLog;
    internal Button ButtonSendMany;
    internal Button ButtonSendParityError;
    internal Button ButtonSendFramingError;
    internal Button ButtonReceiveMany;
    internal TextBox TextBoxTerminal;
    internal TextBox TextBoxBinary;
    internal TextBox TextBoxTest;
    private bool _contentLoaded;

    public MiConTestWindow(
      CommunicationPortWindowFunctions comPortWindowsFunctions,
      ulong BTMAC,
      BluetoothChannel_LE BLE_Channel = null)
    {
      this.InitializeComponent();
      this.MyWindowFunctions = comPortWindowsFunctions;
      if (BTMAC > 0UL)
        this.TextBoxMiConBLMAC.Text = BTMAC.ToString("X");
      if (BLE_Channel != null)
      {
        this.BLE_Channel = BLE_Channel;
        if (BLE_Channel.IsConnected)
        {
          this.WriteTerminalLine("Device is connected. Use ASCII commands");
          this.StartCommandLine();
        }
      }
      if (this.MyWindowFunctions.IsPluginObject)
        this.TestPort = PlugInLoader.GmmConfiguration.GetValue("CommunicationPort", CommunicationPortWindowFunctions.ConfigVariables.MiConBLE_TestPort.ToString());
      this.SetComPortList();
      this.SetButtonConnectCloseState();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      try
      {
        DateTime.Now.AddSeconds(5.0);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      try
      {
        if (this.MyWindowFunctions.portFunctions.IsOpen)
          this.MyWindowFunctions.Close();
        if (this.SerialTestPort == null || !this.SerialTestPort.IsOpen)
          return;
        this.SerialTestPort.Close();
      }
      catch
      {
      }
    }

    private void BluetootDataReceived(object sender, object parameter)
    {
      if (this.Dispatcher.Thread != Thread.CurrentThread)
      {
        try
        {
          this.Dispatcher.BeginInvoke((Delegate) new System.EventHandler(this.BluetootDataReceived), sender, parameter);
        }
        catch
        {
        }
      }
      else
      {
        if (!(parameter is ReceivedCountEventArgs))
          return;
        byte[] receivedData = this.BLE_Channel.GetReceivedData(((ReceivedCountEventArgs) parameter).Count);
        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte num in receivedData)
        {
          stringBuilder.Append((char) num);
          this.BluetoothReceiveQueue.Enqueue(num);
        }
        this.DeleteCommandLine();
        this.WriteTerminalData("Data received (" + stringBuilder.Length.ToString() + "Byte):" + Environment.NewLine);
        string singleIndentString = this.SingleIndentString;
        this.SingleIndentString = " -> ";
        this.Indent();
        this.WriteTerminalData(stringBuilder.ToString());
        this.Unindent();
        this.SingleIndentString = singleIndentString;
        this.ShowBinaryData(receivedData, true);
        this.StartCommandLine();
      }
    }

    private void ShowBinaryData(byte[] data, bool received)
    {
      string hexStringFormated = ZR_ClassLibrary.Util.ByteArrayToHexStringFormated(data);
      if (received)
        this.TextBoxBinary.AppendText(Environment.NewLine + "<-" + DateTime.Now.ToString("mm:ss.fff") + ": " + hexStringFormated);
      else
        this.TextBoxBinary.AppendText(Environment.NewLine + "->" + DateTime.Now.ToString("mm:ss.fff") + ": " + hexStringFormated);
    }

    private async void ButtonConnectClose_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.ButtonConnectClose.Content.ToString() == "Connect to MiCon")
        {
          if (this.BLE_Channel == null)
          {
            ulong MiConBLMAC = ulong.Parse(this.TextBoxMiConBLMAC.Text, NumberStyles.HexNumber);
            this.BLE_Channel = new BluetoothChannel_LE(MiConBLMAC);
          }
          else
            this.BLE_Channel.OnDataReceived -= new System.EventHandler(this.BluetootDataReceived);
          this.BLE_Channel.OnDataReceived += new System.EventHandler(this.BluetootDataReceived);
          if (!this.BLE_Channel.IsConnected)
            await this.BLE_Channel.ConnectAsync();
          this.WriteTerminalLine("Device connected. Use ASCII commands");
          this.TabItemAutoTests.IsEnabled = true;
          this.GroupBoxInsertCommandTemplates.IsEnabled = true;
          this.StartCommandLine();
        }
        else
        {
          this.BLE_Channel.Close();
          this.WriteTerminalLine();
          this.WriteTerminalLine("Closed");
          this.TabItemAutoTests.IsEnabled = false;
          this.GroupBoxInsertCommandTemplates.IsEnabled = false;
        }
      }
      catch (Exception ex)
      {
        this.WriteTerminalLine("Bluetooth connection error");
        ExceptionViewer.Show(ex);
      }
      this.SetButtonConnectCloseState();
    }

    private void SetButtonConnectCloseState()
    {
      if (this.BLE_Channel == null || !this.BLE_Channel.IsConnected)
        this.ButtonConnectClose.Content = (object) "Connect to MiCon";
      else
        this.ButtonConnectClose.Content = (object) "Break MiCon connection";
    }

    private void StartCommandLine()
    {
      if (!this.TextBoxTerminal.Text.EndsWith(Environment.NewLine))
        this.TextBoxTerminal.AppendText(Environment.NewLine);
      this.SingleIndentString = "   ";
      this.IndentString = string.Empty;
      this.AsciiCommandStartIndex = this.TextBoxTerminal.Text.Length;
      this.TextBoxTerminal.AppendText("#");
      this.TextBoxTerminal.CaretIndex = this.TextBoxTerminal.Text.Length;
      this.TextBoxTerminal.ScrollToEnd();
      this.TextBoxTerminal.Focus();
    }

    private void DeleteCommandLine()
    {
      if (this.TextBoxTerminal.Text.Length <= this.AsciiCommandStartIndex)
        return;
      this.TextBoxTerminal.Select(this.AsciiCommandStartIndex, this.TextBoxTerminal.Text.Length - this.AsciiCommandStartIndex);
      this.TextBoxTerminal.SelectedText = "";
      this.AsciiCommandStartIndex = -1;
    }

    private void Indent() => this.IndentString += this.SingleIndentString;

    private void Unindent()
    {
      if (this.IndentString.Length > this.SingleIndentString.Length)
        this.IndentString = this.IndentString.Substring(this.SingleIndentString.Length);
      else
        this.IndentString = string.Empty;
    }

    private void WriteTerminalLine(string theLine = "")
    {
      this.AsciiCommandStartIndex = -1;
      this.TextBoxTerminal.AppendText(this.IndentString + theLine + Environment.NewLine);
      this.TextBoxTerminal.Focus();
      this.TextBoxTerminal.CaretIndex = this.TextBoxTerminal.Text.Length;
    }

    private void WriteTerminalData(string theData = "")
    {
      this.TextBoxTerminal.AppendText(this.IndentString + theData);
    }

    private static string ByteArrayToString(byte[] data)
    {
      if (data == null)
        return "<NULL>";
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte num in data)
        stringBuilder.Append(num.ToString("X"));
      return stringBuilder.ToString();
    }

    private async void TextBoxTerminal_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      if (this.BLE_Channel != null && this.BLE_Channel.IsConnected && this.AsciiCommandStartIndex < this.TextBoxTerminal.Text.Length)
      {
        e.Handled = true;
        try
        {
          if (this.BLE_Channel.IsConnected && this.AsciiCommandStartIndex >= 0)
          {
            string commandString = this.TextBoxTerminal.Text.Substring(this.AsciiCommandStartIndex);
            this.CommandHistory.Add(commandString);
            this.CommandHistoryIndex = this.CommandHistory.Count;
            byte[] data = (byte[]) null;
            int endIndex = commandString.IndexOf("\\n");
            if (endIndex >= 0)
            {
              string hexDataString = commandString.Substring(endIndex + 2);
              data = ZR_ClassLibrary.Util.HexStringToByteArray(hexDataString);
              commandString = commandString.Substring(0, endIndex);
              hexDataString = (string) null;
            }
            byte[] commandBytes = Encoding.ASCII.GetBytes(commandString + "\r\n");
            if (data != null)
            {
              int length = commandBytes.Length;
              Array.Resize<byte>(ref commandBytes, commandBytes.Length + data.Length);
              data.CopyTo((Array) commandBytes, length);
              this.ShowBinaryData(data, false);
            }
            this.StartCommandLine();
            await this.BLE_Channel.SendData(commandBytes);
            commandString = (string) null;
            data = (byte[]) null;
            commandBytes = (byte[]) null;
          }
          else
            this.WriteTerminalLine("Command not prepared. Data not send.");
        }
        catch (Exception ex)
        {
          this.WriteTerminalLine("Bluetooth send command error");
          ExceptionViewer.Show(ex);
        }
      }
      else
        e.Handled = false;
    }

    private void TextBoxTerminal_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (this.AsciiCommandStartIndex < 0)
        return;
      if (e.Key == Key.Up)
      {
        e.Handled = true;
        if (this.AsciiCommandStartIndex < this.TextBoxTerminal.Text.Length && this.CommandHistory.Count > 0)
        {
          this.TextBoxTerminal.Select(this.AsciiCommandStartIndex, this.TextBoxTerminal.Text.Length - this.AsciiCommandStartIndex);
          this.TextBoxTerminal.SelectedText = "";
          --this.CommandHistoryIndex;
          if (this.CommandHistoryIndex < 0)
            this.CommandHistoryIndex = this.CommandHistory.Count - 1;
          this.TextBoxTerminal.AppendText(this.CommandHistory[this.CommandHistoryIndex]);
        }
      }
      if (e.Key == Key.Down)
      {
        e.Handled = true;
        if (this.AsciiCommandStartIndex < this.TextBoxTerminal.Text.Length && this.CommandHistory.Count > 0)
        {
          this.TextBoxTerminal.Select(this.AsciiCommandStartIndex, this.TextBoxTerminal.Text.Length - this.AsciiCommandStartIndex);
          this.TextBoxTerminal.SelectedText = "";
          ++this.CommandHistoryIndex;
          if (this.CommandHistoryIndex >= this.CommandHistory.Count)
            this.CommandHistoryIndex = 0;
          this.TextBoxTerminal.AppendText(this.CommandHistory[this.CommandHistoryIndex]);
        }
      }
    }

    private void ButtonTypeCom115200_Click(object sender, RoutedEventArgs e)
    {
      this.DeleteCommandLine();
      this.StartCommandLine();
      this.TextBoxTerminal.AppendText("com rs232 115200 8e1");
      this.TextBoxTerminal.Focus();
    }

    private void ButtonTypeBcTest_Click(object sender, RoutedEventArgs e)
    {
      this.DeleteCommandLine();
      this.StartCommandLine();
      this.TextBoxTerminal.AppendText("bc 1000,10,3\\n010203");
      this.TextBoxTerminal.Focus();
    }

    private async void ButtonBcRandomLoop_Click(object sender, RoutedEventArgs e)
    {
      this.DeleteCommandLine();
      this.IndentString = string.Empty;
      this.WriteTerminalLine("*** Random bc test loop ***");
      this.Indent();
      this.AutoTestOn = true;
      this.AutoReceiveQueue = new ConcurrentQueue<byte>();
      this.breakLoop = false;
      while (!this.breakLoop)
        await this.BcRandomTest();
      this.AutoTestOn = false;
      this.Unindent();
      this.StartCommandLine();
    }

    private async void ButtonBcRandomTest_Click(object sender, RoutedEventArgs e)
    {
      this.DeleteCommandLine();
      this.IndentString = string.Empty;
      this.WriteTerminalLine("*** Random bc test ***");
      this.Indent();
      this.AutoTestOn = true;
      this.AutoReceiveQueue = new ConcurrentQueue<byte>();
      await this.BcRandomTest();
      this.AutoTestOn = false;
      this.Unindent();
      this.StartCommandLine();
    }

    private async Task BcRandomTest()
    {
      byte[] data = (byte[]) null;
      byte[] wakeupData;
      try
      {
        int WakeupChars = this.BcTestCharacters.Next(1, 3000);
        wakeupData = new byte[WakeupChars];
        for (int i = 0; i < wakeupData.Length; ++i)
          wakeupData[i] = (byte) 85;
        int Wait_10ms = this.BcTestWait.Next(1, 30);
        int DataCount = this.BcTestDataCount.Next(0, 500);
        data = new byte[DataCount];
        for (int i = 0; i < DataCount; ++i)
        {
          byte nextByte;
          do
          {
            nextByte = (byte) this.BcTestData.Next(0, (int) byte.MaxValue);
          }
          while (nextByte == (byte) 35);
          data[i] = nextByte;
        }
        string command = "#bc " + WakeupChars.ToString() + "," + Wait_10ms.ToString() + "," + DataCount.ToString() + "\r\n";
        this.WriteTerminalLine(command);
        byte[] commandBytes = Encoding.ASCII.GetBytes(command);
        int commandLength = commandBytes.Length;
        Array.Resize<byte>(ref commandBytes, commandLength + data.Length);
        data.CopyTo((Array) commandBytes, commandLength);
        int command_ms = (int) (0.0 * (double) commandBytes.Length + (double) (Wait_10ms * 10));
        DateTime startTime = DateTime.Now;
        DateTime errorTime = startTime.AddSeconds(10.0);
        await this.BLE_Channel.SendData(commandBytes);
        while (this.AutoReceiveQueue.Count == 0)
        {
          if (DateTime.Now > errorTime)
            throw new TimeoutException();
        }
        DateTime firstWakeupByteTime = DateTime.Now;
        for (int i = 0; i < WakeupChars; ++i)
        {
          while (this.AutoReceiveQueue.Count == 0)
          {
            if (DateTime.Now > errorTime)
              throw new TimeoutException();
            await Task.Delay(1);
          }
          byte nextByte;
          if (!this.AutoReceiveQueue.TryDequeue(out nextByte))
            throw new Exception("Unexpected dequeue result");
          if (nextByte != (byte) 85)
            throw new Exception("Illegal wakeup byte! index:" + i.ToString() + " req:0x55 rec:0x" + nextByte.ToString("x02"));
        }
        DateTime wakeupEndTime = DateTime.Now;
        while (this.AutoReceiveQueue.Count == 0)
        {
          if (DateTime.Now > errorTime)
            throw new TimeoutException();
        }
        DateTime firstDataByteTime = DateTime.Now;
        for (int i = 0; i < data.Length; ++i)
        {
          while (this.AutoReceiveQueue.Count == 0)
          {
            if (DateTime.Now > errorTime)
              throw new TimeoutException();
            await Task.Delay(1);
          }
          byte nextByte;
          if (!this.AutoReceiveQueue.TryDequeue(out nextByte))
            throw new Exception("Unexpected dequeue result");
          if ((int) nextByte != (int) data[i])
            throw new Exception("Illegal date byte! index:" + i.ToString() + " req:0x" + data[i].ToString("x02") + " rec:0x" + nextByte.ToString("x02"));
        }
        this.WriteTerminalLine("Random bc test ok");
        command = (string) null;
        commandBytes = (byte[]) null;
        wakeupData = (byte[]) null;
        data = (byte[]) null;
      }
      catch (Exception ex)
      {
        this.WriteTerminalLine("Random bc test Exception: " + ex.Message);
        if (data == null)
        {
          wakeupData = (byte[]) null;
          data = (byte[]) null;
        }
        else
        {
          string dataString = ZR_ClassLibrary.Util.ByteArrayToHexStringFormated(data);
          this.WriteTerminalLine("data: " + dataString);
          dataString = (string) null;
          wakeupData = (byte[]) null;
          data = (byte[]) null;
        }
      }
    }

    private void ButtonClear_Click(object sender, RoutedEventArgs e)
    {
      this.TextBoxTerminal.Clear();
      this.TextBoxBinary.Clear();
      this.TextBoxTest.Clear();
    }

    private void ButtonLoadHTermCommandLog_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.DefaultExt = ".log";
        openFileDialog.Filter = "HTerm log file (.log)|*.log";
        openFileDialog.CheckFileExists = true;
        openFileDialog.Multiselect = false;
        bool? nullable = openFileDialog.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        string end;
        using (StreamReader streamReader = new StreamReader(openFileDialog.FileName, Encoding.ASCII))
          end = streamReader.ReadToEnd();
        if (string.IsNullOrEmpty(end))
          throw new Exception("No data");
        this.WriteTerminalLine();
        this.WriteTerminalLine("HTerm command log");
        MiConTestWindow.readState readState = MiConTestWindow.readState.Wait02;
        ushort num1 = 0;
        int num2 = 0;
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        StringBuilder stringBuilder3 = new StringBuilder();
        byte num3 = 0;
        foreach (char ch in end)
        {
          byte num4 = (byte) ch;
          if (readState != MiConTestWindow.readState.CS)
            num3 ^= num4;
          switch (readState)
          {
            case MiConTestWindow.readState.Wait02:
              stringBuilder1.Clear();
              stringBuilder2.Clear();
              if (num4 != (byte) 2)
              {
                this.WriteTerminalLine("Illegal start byte: 0x" + num4.ToString("x02"));
                break;
              }
              stringBuilder1.Append("02 ");
              readState = MiConTestWindow.readState.Command;
              break;
            case MiConTestWindow.readState.Command:
              stringBuilder1.Append(num4.ToString("x02") + " ");
              readState = MiConTestWindow.readState.LengthLow;
              break;
            case MiConTestWindow.readState.LengthLow:
              num1 = (ushort) num4;
              readState = MiConTestWindow.readState.LengthHigh;
              break;
            case MiConTestWindow.readState.LengthHigh:
              num1 += (ushort) ((uint) num4 << 8);
              stringBuilder1.Append(num1.ToString("d4") + " ");
              num2 = 0;
              readState = MiConTestWindow.readState.Data;
              break;
            case MiConTestWindow.readState.Data:
              stringBuilder2.Append(ch);
              stringBuilder1.Append(num4.ToString("x02"));
              ++num2;
              if (num2 >= (int) num1)
              {
                readState = MiConTestWindow.readState.CS;
                break;
              }
              break;
            case MiConTestWindow.readState.CS:
              stringBuilder1.Append(" CS:" + num4.ToString("x02"));
              if ((int) num4 == (int) num3)
                stringBuilder1.Append("->OK");
              else
                stringBuilder1.Append("->Error");
              num3 = (byte) 0;
              this.WriteTerminalLine(stringBuilder1.ToString());
              stringBuilder3.AppendLine(stringBuilder2.ToString());
              readState = MiConTestWindow.readState.Wait02;
              break;
          }
        }
        this.WriteTerminalLine();
        this.WriteTerminalLine("ASCII data from HTerm log");
        this.WriteTerminalLine(stringBuilder3.ToString());
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ComboBoxTestComPort_DropDownOpened(object sender, EventArgs e)
    {
      this.SetComPortList();
    }

    private void SetComPortList()
    {
      string mainPort = this.MyWindowFunctions.GetReadoutConfiguration().Port;
      List<ValueItem> availableComPorts = Constants.GetAvailableComPorts();
      int index1 = availableComPorts.FindIndex((Predicate<ValueItem>) (item => item.Value == mainPort));
      if (index1 >= 0)
        availableComPorts.RemoveAt(index1);
      if (availableComPorts.Count == 0)
      {
        this.ComboBoxTestComPort.Items.Clear();
        this.ComboBoxTestComPort.Items.Add((object) "Test COM port not available");
        this.StackPanelButtons.IsEnabled = false;
      }
      else if (this.ComboBoxTestComPort.SelectedItem == null || !(this.ComboBoxTestComPort.SelectedItem is ValueItem))
      {
        this.ComboBoxTestComPort.ItemsSource = (IEnumerable) availableComPorts;
        if (this.TestPort != null)
        {
          int index2 = availableComPorts.FindIndex((Predicate<ValueItem>) (item => item.Value == this.TestPort));
          if (index2 >= 0)
            this.ComboBoxTestComPort.SelectedIndex = index2;
        }
        if (this.ComboBoxTestComPort.SelectedIndex >= 0)
          return;
        this.ComboBoxTestComPort.SelectedIndex = 0;
      }
      else
      {
        ValueItem selectedItemBefore = this.ComboBoxTestComPort.SelectedItem as ValueItem;
        this.ComboBoxTestComPort.ItemsSource = (IEnumerable) availableComPorts;
        int index3 = availableComPorts.FindIndex((Predicate<ValueItem>) (item => item.Value == selectedItemBefore.Value));
        if (index3 >= 0)
          this.ComboBoxTestComPort.SelectedIndex = index3;
      }
    }

    private void ComboBoxMinoConnectComPort_SelectionChanged(
      object sender,
      SelectionChangedEventArgs e)
    {
      this.TestPort = (string) null;
      if (this.ComboBoxTestComPort.SelectedIndex < 0 || !(this.ComboBoxTestComPort.SelectedItem is ValueItem))
        return;
      this.TestPort = ((ValueItem) this.ComboBoxTestComPort.SelectedItem).Value;
      if (!this.MyWindowFunctions.IsPluginObject)
        return;
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("CommunicationPort", CommunicationPortWindowFunctions.ConfigVariables.MiConBLE_TestPort.ToString(), this.TestPort);
    }

    private void ButtonTestCOM_open_Click(object sender, RoutedEventArgs e) => this.OpenCom(115200);

    private void ButtonTestCOM_open96_Click(object sender, RoutedEventArgs e) => this.OpenCom(9600);

    private void OpenCom(int baud)
    {
      try
      {
        if (this.SerialTestPort == null)
        {
          this.SerialTestPort = new SerialPort();
          this.SerialTestPort.PortName = this.TestPort;
          this.SerialTestPort.BaudRate = baud;
          this.SerialTestPort.Parity = Parity.Even;
          this.SerialTestPort.WriteTimeout = 5000;
          this.SerialTestPort.DtrEnable = true;
          this.SerialTestPort.ParityReplace = (byte) 0;
        }
        if (!this.SerialTestPort.IsOpen)
          this.SerialTestPort.Open();
        this.SerialTestPort.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialDataReceivedEventHandler);
        this.SerialTestPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialDataReceivedEventHandler);
        this.DeleteCommandLine();
        this.WriteTerminalLine("Test COM opened: " + this.SerialTestPort.PortName);
        this.ButtonSendFramingError.IsEnabled = true;
        this.ButtonSendMany.IsEnabled = true;
        this.ButtonSendParityError.IsEnabled = true;
        this.ButtonReceiveMany.IsEnabled = true;
        this.ButtonBcRandomLoop.IsEnabled = true;
        this.ButtonBcRandomTest.IsEnabled = true;
        this.ButtonTestCOM_close.IsEnabled = true;
        this.ButtonTestCOM_check.IsEnabled = true;
        this.StartCommandLine();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonTestCOM_close_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.SerialTestPort != null)
        {
          this.SerialTestPort.Close();
          this.SerialTestPort = (SerialPort) null;
        }
      }
      catch
      {
      }
      this.ButtonSendFramingError.IsEnabled = false;
      this.ButtonSendMany.IsEnabled = false;
      this.ButtonSendParityError.IsEnabled = false;
      this.ButtonReceiveMany.IsEnabled = false;
      this.ButtonBcRandomLoop.IsEnabled = false;
      this.ButtonBcRandomTest.IsEnabled = false;
      this.ButtonTestCOM_close.IsEnabled = false;
      this.ButtonTestCOM_check.IsEnabled = false;
    }

    private void SerialDataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e)
    {
      int bytesToRead = this.SerialTestPort.BytesToRead;
      byte[] numArray = new byte[bytesToRead];
      this.SerialTestPort.Read(numArray, 0, bytesToRead);
      if (this.AutoTestOn)
      {
        for (int index = 0; index < numArray.Length; ++index)
          this.AutoReceiveQueue.Enqueue(numArray[index]);
      }
      this.TestDataReceived((object) this, numArray);
    }

    private void TestDataReceived(object sender, byte[] byteData)
    {
      if (this.Dispatcher.Thread != Thread.CurrentThread)
      {
        try
        {
          this.Dispatcher.BeginInvoke((Delegate) new EventHandler<byte[]>(this.TestDataReceived), sender, (object) byteData);
        }
        catch
        {
        }
      }
      else
      {
        string hexStringFormated = ZR_ClassLibrary.Util.ByteArrayToHexStringFormated(byteData);
        this.TextBoxTest.AppendText(DateTime.Now.ToString("mm:ss.fff;") + byteData.Length.ToString() + ": " + hexStringFormated + Environment.NewLine);
      }
    }

    private void ButtonTestCOM_check_Click(object sender, RoutedEventArgs e)
    {
      this.DeleteCommandLine();
      this.WriteTerminalLine("Test COM check:");
      this.Indent();
      if (this.SerialTestPort == null)
        this.WriteTerminalLine("Port object not evailable");
      else if (!this.SerialTestPort.IsOpen)
      {
        this.WriteTerminalLine("Test port not open");
      }
      else
      {
        int bytesToRead = this.SerialTestPort.BytesToRead;
        this.WriteTerminalLine("BytesToRead: " + bytesToRead.ToString());
        if (bytesToRead > 0)
        {
          byte[] buffer = new byte[bytesToRead];
          this.SerialTestPort.Read(buffer, 0, bytesToRead);
          this.WriteTerminalLine("Received bytes: " + ZR_ClassLibrary.Util.ByteArrayToHexString(buffer));
        }
        this.StartCommandLine();
      }
    }

    private void ButtonBreak_Click(object sender, RoutedEventArgs e) => this.breakLoop = true;

    private void ButtonSendMany_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        byte[] numArray = new byte[20000];
        for (int index = 0; index < numArray.Length; ++index)
          numArray[index] = (byte) (index + 85 & (int) byte.MaxValue);
        this.SerialTestPort.Write(numArray, 0, numArray.Length);
        this.ShowBinaryData(numArray, false);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonSendParityError_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SerialTestPort.Close();
        this.SerialTestPort.Parity = Parity.Odd;
        this.SerialTestPort.Open();
        byte[] numArray = new byte[10];
        for (int index = 0; index < numArray.Length; ++index)
          numArray[index] = (byte) (index + 85 & (int) byte.MaxValue);
        this.SerialTestPort.Write(numArray, 0, numArray.Length);
        this.ShowBinaryData(numArray, false);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.SerialTestPort.Close();
      this.SerialTestPort.Parity = Parity.Even;
      this.SerialTestPort.Open();
    }

    private void ButtonSendFramingError_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SerialTestPort.Close();
        this.SerialTestPort.DataBits = 7;
        this.SerialTestPort.Open();
        byte[] numArray = new byte[10];
        for (int index = 0; index < numArray.Length; ++index)
          numArray[index] = (byte) (index + 85 & (int) byte.MaxValue);
        this.SerialTestPort.Write(numArray, 0, numArray.Length);
        this.ShowBinaryData(numArray, false);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.SerialTestPort.Close();
      this.SerialTestPort.DataBits = 8;
      this.SerialTestPort.Open();
    }

    private async void ButtonReceiveMany_Click(object sender, RoutedEventArgs e)
    {
      this.DeleteCommandLine();
      this.IndentString = string.Empty;
      this.WriteTerminalLine("*** ReceiveMany ***");
      this.Indent();
      this.AutoTestOn = true;
      this.AutoReceiveQueue = new ConcurrentQueue<byte>();
      try
      {
        List<byte> dataList = new List<byte>();
        byte[] byteArray = new byte[20000];
        for (int i = 0; i < 20000; ++i)
        {
          byte theByte = (byte) (i + 85 & (int) byte.MaxValue);
          byteArray[i] = theByte;
          dataList.Add(theByte);
          if (theByte == (byte) 35)
            dataList.Add(theByte);
        }
        byte[] buffer = dataList.ToArray();
        this.ShowBinaryData(buffer, true);
        int num = byteArray.Length;
        this.WriteTerminalLine("Transmit byte count: " + num.ToString());
        await this.BLE_Channel.SendData(buffer);
        int receiceCount = 0;
        while (true)
        {
          await Task.Delay(2000);
          if (this.AutoReceiveQueue.Count != receiceCount)
            receiceCount = this.AutoReceiveQueue.Count;
          else
            break;
        }
        this.WriteTerminalLine("Receive byte count: " + receiceCount.ToString());
        for (int i = 0; i < receiceCount; num = i++)
        {
          byte qb = 0;
          if (!this.AutoReceiveQueue.TryDequeue(out qb))
            throw new Exception("TryDequeue error");
          if ((int) qb != (int) byteArray[i])
          {
            this.WriteTerminalLine("First difference at index: " + i.ToString());
            this.WriteTerminalLine("  Transmit byte: 0x: " + byteArray[i].ToString("x02"));
            this.WriteTerminalLine("  Receive  byte: 0x: " + qb.ToString("x02"));
            break;
          }
        }
        dataList = (List<byte>) null;
        byteArray = (byte[]) null;
        buffer = (byte[]) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.AutoTestOn = false;
      this.Unindent();
      this.StartCommandLine();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/CommunicationPort;component/userinterface/micontestwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
          ((Window) target).Closing += new CancelEventHandler(this.Window_Closing);
          break;
        case 2:
          this.TabItemBasicCommands = (TabItem) target;
          break;
        case 3:
          this.StackPanelBoxes = (StackPanel) target;
          break;
        case 4:
          this.TextBoxOwnBLMAC = (TextBox) target;
          break;
        case 5:
          this.TextBoxMiConBLMAC = (TextBox) target;
          break;
        case 6:
          this.GroupBoxInsertCommandTemplates = (GroupBox) target;
          break;
        case 7:
          this.ButtonTypeCom115200 = (Button) target;
          this.ButtonTypeCom115200.Click += new RoutedEventHandler(this.ButtonTypeCom115200_Click);
          break;
        case 8:
          this.ButtonTypeBcTest = (Button) target;
          this.ButtonTypeBcTest.Click += new RoutedEventHandler(this.ButtonTypeBcTest_Click);
          break;
        case 9:
          this.StackPanelButtons = (StackPanel) target;
          break;
        case 10:
          this.ButtonClear = (Button) target;
          this.ButtonClear.Click += new RoutedEventHandler(this.ButtonClear_Click);
          break;
        case 11:
          this.ButtonConnectClose = (Button) target;
          this.ButtonConnectClose.Click += new RoutedEventHandler(this.ButtonConnectClose_Click);
          break;
        case 12:
          this.TabItemAutoTests = (TabItem) target;
          break;
        case 13:
          this.ButtonBcRandomTest = (Button) target;
          this.ButtonBcRandomTest.Click += new RoutedEventHandler(this.ButtonBcRandomTest_Click);
          break;
        case 14:
          this.ButtonBcRandomLoop = (Button) target;
          this.ButtonBcRandomLoop.Click += new RoutedEventHandler(this.ButtonBcRandomLoop_Click);
          break;
        case 15:
          this.ButtonBreak = (Button) target;
          this.ButtonBreak.Click += new RoutedEventHandler(this.ButtonBreak_Click);
          break;
        case 16:
          this.ComboBoxTestComPort = (ComboBox) target;
          this.ComboBoxTestComPort.DropDownOpened += new System.EventHandler(this.ComboBoxTestComPort_DropDownOpened);
          this.ComboBoxTestComPort.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxMinoConnectComPort_SelectionChanged);
          break;
        case 17:
          this.ButtonTestCOM_open = (Button) target;
          this.ButtonTestCOM_open.Click += new RoutedEventHandler(this.ButtonTestCOM_open_Click);
          break;
        case 18:
          this.ButtonTestCOM_open96 = (Button) target;
          this.ButtonTestCOM_open96.Click += new RoutedEventHandler(this.ButtonTestCOM_open96_Click);
          break;
        case 19:
          this.ButtonTestCOM_close = (Button) target;
          this.ButtonTestCOM_close.Click += new RoutedEventHandler(this.ButtonTestCOM_close_Click);
          break;
        case 20:
          this.ButtonTestCOM_check = (Button) target;
          this.ButtonTestCOM_check.Click += new RoutedEventHandler(this.ButtonTestCOM_check_Click);
          break;
        case 21:
          this.ButtonLoadHTermCommandLog = (Button) target;
          this.ButtonLoadHTermCommandLog.Click += new RoutedEventHandler(this.ButtonLoadHTermCommandLog_Click);
          break;
        case 22:
          this.ButtonSendMany = (Button) target;
          this.ButtonSendMany.Click += new RoutedEventHandler(this.ButtonSendMany_Click);
          break;
        case 23:
          this.ButtonSendParityError = (Button) target;
          this.ButtonSendParityError.Click += new RoutedEventHandler(this.ButtonSendParityError_Click);
          break;
        case 24:
          this.ButtonSendFramingError = (Button) target;
          this.ButtonSendFramingError.Click += new RoutedEventHandler(this.ButtonSendFramingError_Click);
          break;
        case 25:
          this.ButtonReceiveMany = (Button) target;
          this.ButtonReceiveMany.Click += new RoutedEventHandler(this.ButtonReceiveMany_Click);
          break;
        case 26:
          this.TextBoxTerminal = (TextBox) target;
          this.TextBoxTerminal.KeyDown += new KeyEventHandler(this.TextBoxTerminal_KeyDown);
          this.TextBoxTerminal.PreviewKeyDown += new KeyEventHandler(this.TextBoxTerminal_PreviewKeyDown);
          break;
        case 27:
          this.TextBoxBinary = (TextBox) target;
          break;
        case 28:
          this.TextBoxTest = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    private enum readState
    {
      Wait02,
      Command,
      LengthLow,
      LengthHigh,
      Data,
      CS,
    }
  }
}
