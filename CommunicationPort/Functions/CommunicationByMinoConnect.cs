// Decompiled with JetBrains decompiler
// Type: CommunicationPort.Functions.CommunicationByMinoConnect
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using GmmDbLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace CommunicationPort.Functions
{
  public class CommunicationByMinoConnect : CommunicationBase, IDisposable
  {
    private static Logger Base_MiConLogger = LogManager.GetLogger(nameof (CommunicationByMinoConnect));
    internal ChannelLogger MiConLogger;
    internal IChannel channel;
    public CancellationToken CancelToken;
    public bool CommandOnlyMode = false;
    internal Queue<byte> ReceiveQueue = new Queue<byte>(300);
    internal Queue<byte> StatusQueue = new Queue<byte>(100);
    internal Queue<byte[]> StatusResponseLines = new Queue<byte[]>(5);
    private ByteField StatusReceiveLine = new ByteField(100);
    private byte[] LastStatusLine = new byte[7];
    internal int KeyReceivedCounter;
    internal int StatusReceivedCounter;
    private bool isTransparent;
    private bool ESC_Reseived = false;
    private bool StatusAnswerActive = false;
    internal MinoConnectState StateRequired;
    internal MinoConnectState StateLastReceived;
    internal MinoConnectState StateSecondLastReceived;
    internal const int StatusPolling_ms_Default = 500;
    internal const int StatusErrorTime_ms_Default = 10000;
    internal int StatusPolling_ms;
    internal int StatusTimeoutCounter;
    private string Version;
    public FirmwareVersion firmwareVersion;
    private static int callCounter = 0;
    internal MiConPollingThread PollingThreadObj;
    private Thread PollThread;

    public event System.EventHandler ConnectionLost;

    public bool is3Von { get; private set; }

    public bool is7Von { get; private set; }

    public bool isOverload { get; private set; }

    public bool isBluetoothBLE => this.channel is BluetoothChannel_LE;

    public int StatusErrorTime_ms { get; set; }

    public Decimal VersionValue { get; internal set; }

    internal CommunicationByMinoConnect(ICommunicationFunctions comPortFunctions)
      : base(comPortFunctions)
    {
      this.StatusPolling_ms = 500;
      this.StatusErrorTime_ms = 10000;
      this.StatusTimeoutCounter = this.StatusErrorTime_ms / this.StatusPolling_ms;
      this.MiConLogger = new ChannelLogger(CommunicationByMinoConnect.Base_MiConLogger, comPortFunctions.GetReadoutConfiguration());
      this.MiConLogger.Debug("CommunicationByMinoConnect object created");
    }

    public override void Open()
    {
      if (this.channel == null)
        this.channel = CommunicationByMinoConnect.CreateChannel(this.configList.Port);
      else if (this.channel.PortName != this.configList.Port)
      {
        this.Close();
        this.channel.Dispose();
        this.channel = CommunicationByMinoConnect.CreateChannel(this.configList.Port);
      }
      if (this.channel.IsOpen)
        return;
      this.MiConLogger.Trace("Start open port");
      this.MyFunctions.TransceiverDeviceInfo = string.Empty;
      try
      {
        this.channel.Open();
        this.ESC_Reseived = false;
        this.StatusAnswerActive = false;
        this.MaxAdditionalStateBytes = 10;
        this.MyFunctions.RaiseMessageEvent(Ot.Gtm(Tg.CommunicationLogic, "MiConInitialising", "Connect and initialize MinoConnect"));
        if (this.isTransparent)
          return;
        this.WriteCommand("#com off\r\n#comcl\r\n#broff\r\n#ver\r\n");
        this.Version = this.ReceiveAnswer();
        if (this.Version.Length < 12)
        {
          this.MiConLogger.Error(Ot.Gtm(Tg.CommunicationLogic, "MiConNoAnswer", "MinoConnect: No Answer!"));
          this.Close();
          return;
        }
        this.MiConLogger.Trace(this.Version);
        string s1 = this.Version.Substring(6, 1);
        string s2 = this.Version.Substring(8, 1);
        string s3 = this.Version.Substring(10, 1);
        string str = FirmwareType.MinoConnect.ToString("X");
        int num1 = int.Parse(s1);
        int num2 = int.Parse(s2);
        int num3 = int.Parse(s3);
        this.firmwareVersion = new FirmwareVersion(Convert.ToUInt32("0x" + num1.ToString("00") + num2.ToString("00") + num3.ToString("0") + str.Substring(str.Length - 3), 16));
        this.VersionValue = (Decimal) num1 + (Decimal) num2 / 10M + (Decimal) num3 / 1000M;
        if (this.channel is BluetoothChannel_LE && this.VersionValue < 3.003M)
          throw new Exception("Not suported MinoConnect firmware version (< 3.0.3)");
        this.Version.Split('|');
        this.MyFunctions.TransceiverDeviceInfo = this.Version.Replace("#Ver", "MinoConnect" + ZR_Constants.SystemNewLine + "Firmware version").Replace("|", ZR_Constants.SystemNewLine);
        this.StateRequired = new MinoConnectState(this, this.MiConLogger);
        this.UpdateIrDaFilterBy9600RoundSite();
        this.WriteCommand(new MinoConnectState(this.MiConLogger).GetChangeCommand(this.StateRequired));
        this.StateLastReceived = new MinoConnectState(this, this.MiConLogger);
        this.StateSecondLastReceived = new MinoConnectState(this, this.MiConLogger);
        this.ConfigurationChanged = false;
        if (!this.CommandOnlyMode)
        {
          this.StartPolling();
          this.DiscardInBuffer();
        }
      }
      catch (TimeoutException ex)
      {
        if (this.channel.IsOpen)
          this.channel.Close();
        this.MiConLogger.Error("MiConOpenTimeout: " + ex.ToString());
        string message = Ot.Gtm(Tg.CommunicationLogic, "MiConOpenTimeout", "MinoConnect open timeout") + " " + this.channel.PortName;
        this.channel = (IChannel) null;
        throw new Exception(message, (Exception) ex);
      }
      catch (Exception ex)
      {
        if (this.channel.IsOpen)
          this.channel.Close();
        string message = Ot.Gtm(Tg.CommunicationLogic, "MiConOpenError", "MinoConnect open error") + " " + this.channel.PortName + Environment.NewLine + ex.Message;
        this.MiConLogger.Error("MiConOpenError: " + ex.ToString());
        this.channel = (IChannel) null;
        throw new Exception(message, ex);
      }
      this.MiConLogger.Trace("Port open");
      this.SetNextTimeAfterOpen();
    }

    public override void Close()
    {
      if (this.channel == null)
        return;
      if (this.IsOpen)
      {
        this.WriteCommand("#com off\r\n");
        Thread.Sleep(10);
        this.channel.Close();
        this.channel = (IChannel) null;
      }
      this.StopPolling();
    }

    public override bool IsOpen
    {
      get
      {
        return this.channel != null && this.channel.IsOpen && !string.IsNullOrWhiteSpace(this.Version);
      }
    }

    public string GetDeviceInfo()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.channel != null && this.channel.IsOpen && this.Version != null)
      {
        string str1 = this.Version.Substring(1);
        char[] chArray = new char[1]{ '|' };
        foreach (string str2 in str1.Split(chArray))
          stringBuilder.AppendLine(str2);
      }
      return stringBuilder.ToString();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Type: MinoConnect");
      if (this.channel != null)
      {
        stringBuilder.AppendLine("Open: " + this.channel.IsOpen.ToString());
        if (this.channel.IsOpen)
        {
          stringBuilder.AppendLine("Port: " + this.configList.Port);
          stringBuilder.AppendLine("Baudrate: " + this.configList.Baudrate.ToString());
          stringBuilder.AppendLine("Parity: " + this.Parity.ToString());
          if (this.MinoConnectBaseState == ZENNER.CommonLibrary.MinoConnectBaseStates.IrCombiHead)
            stringBuilder.AppendLine("IrDA: " + this.IrDaSelection.ToString());
          else if (this.MinoConnectBaseState == ZENNER.CommonLibrary.MinoConnectBaseStates.ZIN_CombiHead)
            stringBuilder.AppendLine("CombiHead selection: " + this.CombiHeadSelection.ToString());
          else
            stringBuilder.AppendLine("No combi head selection");
          stringBuilder.AppendLine("State received: " + this.StatusReceivedCounter.ToString());
          stringBuilder.AppendLine("Key received: " + this.KeyReceivedCounter.ToString());
          if (this.channel is BluetoothChannel_LE)
          {
            BluetoothChannel_LE channel = this.channel as BluetoothChannel_LE;
            stringBuilder.AppendLine("Bluetooth MAC: " + channel.BTMAC.ToString("X012"));
            stringBuilder.AppendLine("BLE TransmitDataLength: " + channel.MaxTransmitDataLength.ToString());
            stringBuilder.AppendLine("BLE ReceiveDataLength: " + channel.MaxReceiveDataLength.ToString());
          }
        }
      }
      else
        stringBuilder.AppendLine("Open: False");
      return stringBuilder.ToString();
    }

    public void readActualState()
    {
      this.is7Von = false;
      this.is3Von = false;
      this.isOverload = false;
      if (this.channel == null || !this.channel.IsOpen)
        return;
      string stateString = this.StateLastReceived.GetStateString(this.StateLastReceived);
      this.is7Von = stateString.Contains("RS232_7V") || stateString.Contains("RS485_7V");
      this.is3Von = stateString.Contains("RS232_3V") || stateString.Contains("RS485_3V");
      this.isOverload = this.StateLastReceived.Overload;
    }

    public string GetActualStateAsString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Type: MinoConnect ");
      if (this.channel != null)
      {
        if (this.channel.IsOpen)
        {
          string stateString = this.StateLastReceived.GetStateString(this.StateLastReceived);
          bool flag1 = stateString.Contains("RS232_7V");
          stringBuilder.AppendLine("RS232_7V: " + (flag1 ? "on" : "off"));
          bool flag2 = stateString.Contains("RS485_7V");
          stringBuilder.AppendLine("RS485_7V: " + (flag2 ? "on" : "off"));
          bool flag3 = stateString.Contains("RS232_3V");
          stringBuilder.AppendLine("RS232_3V: " + (flag3 ? "on" : "off"));
          bool flag4 = stateString.Contains("RS485_3V");
          stringBuilder.AppendLine("RS485_3V: " + (flag4 ? "on" : "off"));
          stringBuilder.AppendLine("overload: " + this.StateLastReceived.Overload.ToString());
          stringBuilder.AppendLine("state: " + stateString);
        }
      }
      else
        stringBuilder.AppendLine("Open: False");
      return stringBuilder.ToString();
    }

    public override void WriteWithoutDiscardInputBuffer(byte[] data)
    {
      lock (this.channel)
      {
        DateTime endTimepoint = this.CalculateEndTimepoint(data.Length);
        this.channel.Write(data, 0, data.Length);
        this.WaitOf(endTimepoint);
      }
      if (!this.MiConLogger.IsTraceEnabled)
        return;
      this.MiConLogger.Trace("Write: " + Utility.ByteArrayToHexString(data));
      LogManager.Flush();
    }

    public override void Write(byte[] data) => this.Write(data, 0, data.Length);

    public override void Write(byte[] data, int offset, int count)
    {
      if (this.CommandOnlyMode)
        throw new Exception("Function Write not allowed in CommandOnlyMode");
      this.Open();
      this.WaitToNextTransmitTime();
      this.DiscardInBuffer();
      int num1 = 0;
      int num2 = offset + count;
      for (int index = offset; index < num2; ++index)
      {
        if (data[index] == (byte) 35)
          ++num1;
      }
      if (num1 == 0)
      {
        lock (this.channel)
        {
          DateTime endTimepoint = this.CalculateEndTimepoint(count);
          this.channel.Write(data, offset, count);
          this.WaitOf(endTimepoint);
        }
      }
      else
      {
        byte[] buffer = new byte[count + num1];
        int index1 = offset;
        for (int index2 = 0; index2 < buffer.Length; buffer[index2++] = data[index1++])
        {
          if (data[index1] == (byte) 35)
            buffer[index2++] = (byte) 35;
        }
        lock (this.channel)
        {
          DateTime endTimepoint = this.CalculateEndTimepoint(buffer.Length);
          this.channel.Write(buffer, 0, buffer.Length);
          this.WaitOf(endTimepoint);
        }
      }
      if (!this.MiConLogger.IsTraceEnabled)
        return;
      this.MiConLogger.Trace("Write: " + Utility.ByteArrayToHexString(data, offset, count));
      LogManager.Flush();
    }

    protected override void WriteBaudrateCarrier(int carrierTime_ms)
    {
      int num = (int) ((double) carrierTime_ms / this.ByteTime);
      lock (this.ReceiveQueue)
      {
        lock (this.channel)
        {
          DateTime timepoint = this.CalculateEndTimepoint(num).AddMilliseconds(-50.0);
          this.channel.WriteBaudrateCarrier(num);
          this.WaitOf(timepoint);
        }
      }
    }

    public override void SetBreak()
    {
      this.MiConLogger.Info("Break signal on");
      this.WriteCommand("#bron\r\n");
    }

    public override void ClearBreak()
    {
      this.MiConLogger.Info("Break signal off");
      this.WriteCommand("#broff\r\n");
    }

    public override int Read(byte[] data, int offset, int count)
    {
      int readHeaderTimeout = this.GetReadHeaderTimeout(count);
      byte[] src = this.ReadData(count, readHeaderTimeout);
      Buffer.BlockCopy((Array) src, 0, (Array) data, offset, src.Length);
      return src.Length;
    }

    public override byte[] ReadHeader(int count)
    {
      int readHeaderTimeout = this.GetReadHeaderTimeout(count);
      if (this.MiConLogger.IsTraceEnabled)
        this.MiConLogger.Trace("ReadHeader. Required bytes: " + count.ToString() + " ; Allowed timeout:" + readHeaderTimeout.ToString() + "ms.");
      byte[] numArray = this.ReadData(count, readHeaderTimeout);
      this.SetNextTimeAfterRead();
      return numArray;
    }

    public override byte[] ReadEnd(int count)
    {
      int readEndTimeout = this.GetReadEndTimeout(count);
      if (this.MiConLogger.IsTraceEnabled)
        this.MiConLogger.Trace("ReadEnd. Required bytes: " + count.ToString() + " ; Allowed timeout:" + readEndTimeout.ToString() + "ms.");
      byte[] numArray = this.ReadData(count, readEndTimeout);
      this.SetNextTimeAfterRead();
      return numArray;
    }

    public override void DiscardCurrentInBuffer()
    {
      List<byte> byteList = new List<byte>();
      this.ReceiveQueueData();
      while (this.ReceiveQueue.Count > 0)
        byteList.Add(this.ReceiveQueue.Dequeue());
      if (byteList.Count <= 0)
        return;
      this.MiConLogger.Error("DiscardInBuffer: " + Utility.ByteArrayToHexString(byteList.ToArray()));
    }

    public override bool DiscardInBuffer()
    {
      List<byte> byteList = new List<byte>();
      for (int index = 0; index < 20; ++index)
      {
        this.ReceiveQueueData();
        if (this.ReceiveQueue.Count != 0)
        {
          while (this.ReceiveQueue.Count > 0)
            byteList.Add(this.ReceiveQueue.Dequeue());
          Thread.Sleep(50);
        }
        else
          break;
      }
      if (byteList.Count > 0)
        this.MiConLogger.Error("DiscardInBuffer: " + Utility.ByteArrayToHexString(byteList.ToArray()));
      if (this.ReceiveQueue.Count > 0)
      {
        string str = "Input buffer contains a Stream of uninterrupted bytes.";
        this.MiConLogger.Error(str);
        throw new Exception(str);
      }
      return byteList.Count > 0;
    }

    public override void Dispose()
    {
      this.StopPolling();
      if (this.channel != null)
        this.Close();
      base.Dispose();
    }

    public byte[] ReadData(int count, int timeout_ms)
    {
      if (this.CommandOnlyMode)
        throw new Exception("Function ReadData not allowed in CommandOnlyMode");
      DateTime dateTime = DateTime.Now.AddMilliseconds((double) timeout_ms);
      while (true)
      {
        this.ReceiveQueueData();
        if (count - this.ReceiveQueue.Count >= 1 && !(DateTime.Now > dateTime))
        {
          CancellationToken cancelToken = this.CancelToken;
          if (!this.CancelToken.IsCancellationRequested)
            Thread.Sleep(20);
          else
            break;
        }
        else
          break;
      }
      byte[] buffer = this.DequeueReadData(count);
      if (this.StateLastReceived.FramingError)
      {
        this.MiConLogger.Error("Framing error exception");
        this.StateLastReceived.FramingError = false;
        throw new FramingErrorException(Ot.Gtm(Tg.CommunicationLogic, "MiConFramingError", "MinoConnect framing error"));
      }
      if (this.StateLastReceived.Overload)
      {
        this.MiConLogger.Error("Overload exception");
        this.StateLastReceived.Overload = false;
        throw new CurrentOverloadException(Ot.Gtm(Tg.CommunicationLogic, "MiConOverload", "MinoConnect current overload"));
      }
      if (buffer.Length != 0)
      {
        if (this.Wakeup != 0)
          this.nextWakeupTime = DateTime.Now.AddMilliseconds((double) this.configList.BreakIntervalTime);
        if (this.MiConLogger.IsTraceEnabled)
          this.MiConLogger.Trace("Read: " + Utility.ByteArrayToHexString(buffer));
      }
      if (buffer.Length >= count)
        return buffer;
      string info = "Timeout " + timeout_ms.ToString() + " ms.";
      int num;
      if (buffer.Length != 0)
      {
        string[] strArray = new string[6]
        {
          info,
          " Received ",
          null,
          null,
          null,
          null
        };
        num = buffer.Length;
        strArray[2] = num.ToString();
        strArray[3] = " byte(s), missing ";
        num = count - buffer.Length;
        strArray[4] = num.ToString();
        strArray[5] = " byte(s).";
        info = string.Concat(strArray);
      }
      if (buffer.Length != 0)
        this.MiConLogger.Error(info + " Input buffer: " + Utility.ByteArrayToHexString(buffer));
      else
        this.MiConLogger.Error(info);
      if (buffer.Length != 0)
      {
        string[] strArray = new string[5]
        {
          "Timeout ",
          timeout_ms.ToString(),
          "ms. Missed ",
          null,
          null
        };
        num = count - buffer.Length;
        strArray[3] = num.ToString();
        strArray[4] = " byte(s)";
        throw new MissingBytesTimeoutException(string.Concat(strArray), count, buffer);
      }
      throw new TimeoutException(Ot.Gtm(Tg.CommunicationLogic, "Timeout", "Timeout") + " " + timeout_ms.ToString() + " ms");
    }

    private byte[] DequeueReadData(int count)
    {
      List<byte> byteList = new List<byte>();
      for (int index = 0; index < count && this.ReceiveQueue.Count != 0; ++index)
        byteList.Add(this.ReceiveQueue.Dequeue());
      return byteList.ToArray();
    }

    internal void UpdateIrDaFilterBy9600RoundSite()
    {
      string empty = string.Empty;
      string text = !(this.VersionValue < 2.405M) ? "#irf1\r\n" : (!this.StateRequired.IsRequiredIrDaFilter ? "#irf1\r\n" : "#irf0\r\n");
      if (string.IsNullOrEmpty(text))
        return;
      this.WriteCommand(text);
      try
      {
        string answer = this.ReceiveAnswer();
        if (!string.IsNullOrEmpty(answer))
          this.MiConLogger.Info(answer);
      }
      catch
      {
      }
    }

    public void WriteCommand(string text)
    {
      if (string.IsNullOrEmpty(text))
        return;
      if (this.MiConLogger.IsTraceEnabled)
        this.MiConLogger.Trace("Write commands: " + text.Trim());
      Exception exception = (Exception) null;
      try
      {
        lock (this.channel)
          this.channel.Write(text);
      }
      catch (Exception ex)
      {
        this.MiConLogger.Error(string.Format("Can not transmit command! Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace), ex);
        exception = ex;
      }
      if (exception != null)
        throw exception;
    }

    public void WriteCommand(byte[] commandBytes)
    {
      if (commandBytes == null || commandBytes.Length == 0)
        return;
      if (this.MiConLogger.IsTraceEnabled)
        this.MiConLogger.Trace("Write commands bytes: " + Util.ByteArrayToHexString(commandBytes));
      Exception exception = (Exception) null;
      try
      {
        lock (this.channel)
          this.channel.Write(commandBytes, 0, commandBytes.Length);
      }
      catch (Exception ex)
      {
        this.MiConLogger.Error(string.Format("Can not transmit command! Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace), ex);
        exception = ex;
      }
      if (exception != null)
        throw exception;
    }

    public string ReceiveAnswer(int timeout_ms = 1500)
    {
      if (!this.channel.IsOpen)
        throw new InvalidOperationException("Port not open");
      DateTime dateTime = SystemValues.DateTimeNow.AddMilliseconds((double) timeout_ms);
      StringBuilder stringBuilder = new StringBuilder(200);
      while (true)
      {
        char ch;
        do
        {
          do
          {
            if (this.channel.BytesToRead != 0)
            {
              if (stringBuilder.Length <= 200)
              {
                Thread.Sleep(1);
                try
                {
                  ch = (char) this.channel.ReadByte();
                  if (ch == char.MinValue)
                    ;
                }
                catch (TimeoutException ex)
                {
                  if (dateTime < SystemValues.DateTimeNow)
                  {
                    if (stringBuilder.Length == 0)
                      throw new TimeoutException("No data from MinoConnect!");
                    throw new TimeoutException("Wrong 'RecTime_BeforFirstByte' parameter! Not all data received from MinoConnect.");
                  }
                }
              }
              else
                goto label_9;
            }
            else
              goto label_3;
          }
          while (ch == '\r');
          if (ch != '\n')
            goto label_19;
        }
        while (stringBuilder.Length == 0);
        goto label_21;
label_3:
        if (!(dateTime < SystemValues.DateTimeNow))
        {
          Thread.Sleep(5);
          continue;
        }
        break;
label_19:
        stringBuilder.Append(ch);
      }
      if (stringBuilder.Length == 0)
        throw new TimeoutException("No data from MinoConnect!");
      throw new TimeoutException("Wrong 'RecTime_BeforFirstByte' parameter! Not all data received from MinoConnect.");
label_9:
      throw new ArgumentOutOfRangeException("Too many received bytes");
label_21:
      return stringBuilder.ToString();
    }

    internal DataReceiveInfo ReceiveQueueData()
    {
      if (this.channel == null || !this.channel.IsOpen)
        return (DataReceiveInfo) null;
      DataReceiveInfo queueData = new DataReceiveInfo();
      lock (this.ReceiveQueue)
      {
        try
        {
          ++CommunicationByMinoConnect.callCounter;
          int bytesToRead = this.channel.BytesToRead;
          if (bytesToRead > 0)
          {
            byte[] buffer = new byte[bytesToRead];
            DateTime now = DateTime.Now;
            int num = this.channel.Read(buffer, 0, bytesToRead);
            if (num != bytesToRead)
              this.MiConLogger.Error("Received bytes not equal available bytes");
            for (int index = 0; index < num; ++index)
            {
              if (this.StatusAnswerActive)
              {
                if (buffer[index] == (byte) 35)
                  this.MiConLogger.Error("# inside status received.");
                this.StatusQueue.Enqueue(buffer[index]);
                if (buffer[index] == (byte) 10)
                  this.StatusAnswerActive = false;
                ++queueData.ReceivedStatusBytes;
              }
              else if (this.ESC_Reseived)
              {
                if (buffer[index] == (byte) 35)
                {
                  this.ReceiveQueue.Enqueue(buffer[index]);
                  ++queueData.ReceivedDataBytes;
                }
                else
                {
                  this.StatusQueue.Enqueue((byte) 35);
                  this.StatusQueue.Enqueue(buffer[index]);
                  this.StatusAnswerActive = true;
                  ++queueData.ReceivedStatusBytes;
                }
                this.ESC_Reseived = false;
              }
              else if (buffer[index] == (byte) 35)
              {
                this.ESC_Reseived = true;
              }
              else
              {
                this.ReceiveQueue.Enqueue(buffer[index]);
                ++queueData.ReceivedDataBytes;
              }
            }
          }
          while (this.StatusQueue.Count > 0)
          {
            if (this.StatusReceiveLine.Count == this.StatusReceiveLine.Data.Length)
            {
              this.StatusReceiveLine.Count = 0;
              this.MiConLogger.Error("To many stats bytes. ");
            }
            byte Byte = this.StatusQueue.Dequeue();
            if (this.StatusReceiveLine.Count == 0 && Byte != (byte) 35)
            {
              this.MiConLogger.Error("Illegal status line. First byte != '#'");
            }
            else
            {
              this.StatusReceiveLine.Add(Byte);
              if (Byte == (byte) 10)
              {
                byte[] byteArray = this.StatusReceiveLine.GetByteArray();
                this.StatusReceiveLine.Count = 0;
                if (MiConPollingThread.Base_MiConPollingLogger.IsTraceEnabled)
                  MiConPollingThread.Base_MiConPollingLogger.Trace(this.MiConLogger.ChannelInfo + "Status received: " + Utility.ByteArrayToHexString(byteArray));
                this.StatusResponseLines.Enqueue(byteArray);
                if (byteArray != null && byteArray.Length > 2 && byteArray[1] == (byte) 115)
                {
                  ++this.StatusReceivedCounter;
                  queueData.StatusInfoReceived = true;
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          string[] strArray = new string[5]
          {
            Ot.Gtm(Tg.CommunicationLogic, "MiConReceiveQueueDataException", "Exception in ReceiveQueueData"),
            " StatusQueue.Count=",
            null,
            null,
            null
          };
          int count = this.StatusQueue.Count;
          strArray[2] = count.ToString();
          strArray[3] = "; , ReceiveQueue.Count=";
          count = this.ReceiveQueue.Count;
          strArray[4] = count.ToString();
          string str = string.Concat(strArray) + "; Exception=" + ex.Message + "; Trace=" + ex.StackTrace;
          this.MiConLogger.Error(str, ex);
          this.MyFunctions.RaiseMessageEvent(str);
        }
        finally
        {
          ++CommunicationByMinoConnect.callCounter;
        }
      }
      return queueData;
    }

    internal void WorkStatusLine()
    {
      while (this.StatusResponseLines.Count > 0)
      {
        byte[] StateLine = this.StatusResponseLines.Dequeue();
        if (StateLine != null && StateLine.Length > 2)
        {
          if (StateLine[1] == (byte) 115)
          {
            if (this.StateLastReceived != null && this.StateLastReceived.SetFromReceivedState(StateLine))
            {
              if (this.StateLastReceived.StateChanged)
              {
                if (this.StateLastReceived.KeyReceived)
                {
                  this.MiConLogger.Info("MinoConnect key event received.");
                  this.MyFunctions.RaiseKeyEvent();
                  ++this.KeyReceivedCounter;
                }
                if (this.StateLastReceived.Overload && !this.StateSecondLastReceived.Overload)
                {
                  string str = Ot.Gtm(Tg.CommunicationLogic, "MiConOverload", "MinoConnect current overload");
                  this.MiConLogger.Error(str);
                  this.MyFunctions.RaiseMessageEvent(str);
                }
                if (this.StateLastReceived.BatteryLow && !this.StateSecondLastReceived.BatteryLow)
                {
                  string str = Ot.Gtm(Tg.CommunicationLogic, "MiConBatteryLow", "MinoConnect battery low");
                  this.MiConLogger.Error(str);
                  this.MyFunctions.RaiseMessageEvent(str);
                  this.MyFunctions.RaiseBatteryLowEvent();
                }
                this.MyFunctions.RaiseMessageEvent(Ot.Gtm(Tg.CommunicationLogic, "MiConStateChanged", "MinoConnect state changed"));
                this.StateSecondLastReceived.SetFromMinoConnectState(this.StateLastReceived);
              }
              if ((this.StatusReceivedCounter & 1) == 0)
                this.MyFunctions.RaiseAliveEvent(this.StatusReceivedCounter);
            }
          }
          else if (StateLine[1] == (byte) 70 && StateLine.Length == 8 && StateLine[2] == (byte) 95 && StateLine[3] == (byte) 69 && StateLine[4] == (byte) 82 && StateLine[5] == (byte) 82)
          {
            this.MiConLogger.Error("Asynchron FramingError from MinoConnect received.");
            this.StateLastReceived.FramingError = true;
          }
        }
      }
    }

    internal bool SetTransparentMode(bool enable)
    {
      this.Open();
      if (enable)
      {
        this.WriteCommand("#apo 0\r\n");
        this.WriteCommand("#coff\r\n");
        this.isTransparent = true;
        this.StopPolling();
        return true;
      }
      this.channel.Write("aaafTzhuZl5c39zUNdWq105bmysloncwalnNIK783BH89kirEWmIkPl(!56)bfrtg984!?eV&29IkoPmt!$ymncSrtIopQ'+*bg%ad279vRzOp;-_4y78JI08NJde6HjiOx");
      this.isTransparent = false;
      this.Close();
      this.Open();
      return true;
    }

    private void StartPolling()
    {
      this.MiConLogger.Trace("StartPolling() called.");
      if (this.channel.IsOpen && this.PollThread != null && this.PollingThreadObj != null && !this.PollingThreadObj.StopThread)
        return;
      if (this.PollingThreadObj == null)
      {
        this.PollingThreadObj = new MiConPollingThread(this);
        this.PollingThreadObj.ConnectionLost += new System.EventHandler(this.PollingThreadObj_ConnectionLost);
      }
      this.PollingThreadObj.StopThread = false;
      if (this.PollThread == null)
      {
        this.PollThread = new Thread(new ThreadStart(new MiConPollingThread.Start(this.PollingThreadObj.PollingThreadMain).Invoke));
        this.PollThread.Name = "MinoConnectPolling";
        this.PollThread.IsBackground = true;
      }
      this.PollThread.Start();
    }

    private void StopPolling()
    {
      if (this.PollThread == null || this.PollingThreadObj == null)
        return;
      this.MiConLogger.Trace("Stop MinoConnect polling thread.");
      this.PollingThreadObj.StopThread = true;
      this.PollingThreadObj.ConnectionLost -= new System.EventHandler(this.PollingThreadObj_ConnectionLost);
      int num = 0;
      while (num < 20 && (this.PollThread == null || !this.PollThread.Join(100)))
        ++num;
      this.PollingThreadObj = (MiConPollingThread) null;
      this.PollThread = (Thread) null;
    }

    private void PollingThreadObj_ConnectionLost(object sender, EventArgs e)
    {
      if (this.ConnectionLost == null)
        return;
      this.ConnectionLost(sender, e);
    }

    private static IChannel CreateChannel(string portName)
    {
      if (portName.StartsWith("Mi"))
        return (IChannel) new BluetoothChannel_LE(portName);
      return BluetoothChannel.IsBluetooth(portName) ? (IChannel) new BluetoothChannel(portName) : (IChannel) new SerialPortChannel(portName);
    }

    public byte[] ReadExisting()
    {
      List<byte> byteList = new List<byte>();
      this.ReceiveQueueData();
      while (this.ReceiveQueue.Count > 0)
        byteList.Add(this.ReceiveQueue.Dequeue());
      return byteList.ToArray();
    }

    public void StartRadio2()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio radio2\r\n");
    }

    public void StartRadio3(string syncWord = null)
    {
      this.WriteCommand("#comcl\r\n");
      if (string.IsNullOrEmpty(syncWord))
        this.WriteCommand("#com radio radio3\r\n");
      else
        this.WriteCommand("#com radio radio3:" + syncWord + "\r\n");
    }

    public void StartRadio4()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio radio4\r\n");
    }

    public void StopRadio()
    {
      this.WriteCommand("#com rsoff\r\n");
      Thread.Sleep(100);
      this.DiscardInBuffer();
      this.channel.DiscardOutBuffer();
    }

    public bool StartMinomatRadioTest(byte networkID)
    {
      if (this.VersionValue < 2.4M)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, Ot.Gtm(Tg.CommunicationLogic, "YouAreUsingOldFirmwareOnMinoConnect", "You are using an MinoConnect with old firmware, this command is not supported by this firmware."));
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#rtest sndinq " + networkID.ToString() + "\r\n");
      return true;
    }

    public bool StartRadio3_868_95_RUSSIA(string syncWord = null)
    {
      if (this.VersionValue < 2.4M)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, Ot.Gtm(Tg.CommunicationLogic, "YouAreUsingOldFirmwareOnMinoConnect", "You are using an MinoConnect with old firmware, this command is not supported by this firmware."));
      this.WriteCommand("#comcl\r\n");
      if (string.IsNullOrEmpty(syncWord))
        this.WriteCommand("#com radio radio3_868.95\r\n");
      else
        this.WriteCommand("#com radio radio3_868.95:" + syncWord + "\r\n");
      return true;
    }

    public bool Start_RadioMS()
    {
      if (this.VersionValue < 2.4M)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, Ot.Gtm(Tg.CommunicationLogic, "YouAreUsingOldFirmwareOnMinoConnect", "You are using an MinoConnect with old firmware, this command is not supported by this firmware."));
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio radio_ms\r\n");
      return true;
    }

    public bool Start_wMBusS1()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio wmbus_s1\r\n");
      return true;
    }

    public bool Start_wMBusS1M()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio wmbus_s1m\r\n");
      return true;
    }

    public bool Start_wMBusS2()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio wmbus_s2\r\n");
      return true;
    }

    public bool Start_wMBusT1()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio wmbus_t1\r\n");
      return true;
    }

    public bool Start_wMBusT1_867_2()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio \r\n");
      return true;
    }

    public bool Start_wMBusT2_meter()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio wmbus_t2_m\r\n");
      return true;
    }

    public bool Start_wMBusT2_other()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio wmbus_t2_o\r\n");
      return true;
    }

    public bool Start_wMBusC1A()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio wmbus_c1a\r\n");
      return true;
    }

    public bool Start_wMBusC1B()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio wmbus_c1b\r\n");
      return true;
    }

    public bool StartSendTestPacket(RadioMode radioMode, byte power)
    {
      if (power < (byte) 0 || power > (byte) 7)
        return false;
      string str;
      switch (radioMode)
      {
        case RadioMode.Radio2:
          str = "radio2";
          break;
        case RadioMode.Radio3:
          str = "radio3";
          break;
        case RadioMode.wMBusS1:
          str = "wmbus_s1";
          break;
        case RadioMode.wMBusS1M:
          str = "wmbus_s1m";
          break;
        case RadioMode.wMBusS2:
          str = "wmbus_s2";
          break;
        case RadioMode.wMBusT1:
          str = "wmbus_t1";
          break;
        case RadioMode.wMBusT2_meter:
          str = "wmbus_t2_m";
          break;
        case RadioMode.wMBusT2_other:
          str = "wmbus_t2_o";
          break;
        case RadioMode.wMBusC1A:
          str = "wmbus_c1a";
          break;
        case RadioMode.wMBusC1B:
          str = "wmbus_c1b";
          break;
        default:
          return false;
      }
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#rtest loop " + str + " " + power.ToString() + "\r\n");
      return true;
    }

    public bool StopSendTestPacket()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#rtest stop\r\n");
      return true;
    }

    public void SendTestPacket(
      uint deviceID,
      RadioMode radioMode,
      byte power,
      string syncWord = "91D3",
      string testPacket = null)
    {
      if (deviceID.ToString().Length > 8)
        throw new ArgumentOutOfRangeException(nameof (deviceID), "Device ID must be less or equal as 8 chars!");
      string str1;
      switch (radioMode)
      {
        case RadioMode.Radio2:
          str1 = "radio2";
          break;
        case RadioMode.Radio3:
          str1 = "radio3";
          break;
        case RadioMode.wMBusS1:
          str1 = "wmbus_s1";
          break;
        case RadioMode.wMBusS1M:
          str1 = "wmbus_s1m";
          break;
        case RadioMode.wMBusS2:
          str1 = "wmbus_s2";
          break;
        case RadioMode.wMBusT1:
          str1 = "wmbus_t1";
          break;
        case RadioMode.wMBusT2_meter:
          str1 = "wmbus_t2_m";
          break;
        case RadioMode.wMBusT2_other:
          str1 = "wmbus_t2_o";
          break;
        case RadioMode.wMBusC1A:
          str1 = "wmbus_c1a";
          break;
        case RadioMode.wMBusC1B:
          str1 = "wmbus_c1b";
          break;
        case RadioMode.wMBusT1_867_2:
          return;
        case RadioMode.Radio3_868_95:
          str1 = "radio3_868.95";
          break;
        case RadioMode.Radio_840:
          str1 = "radio_840";
          break;
        default:
          return;
      }
      string str2 = str1 + ":" + syncWord;
      if (testPacket == null)
        testPacket = CommunicationByMinoConnect.GetMiConRadioTestPacket(deviceID);
      string text = "#rtest send " + str2 + " " + power.ToString() + testPacket + "\r\n";
      lock (this.channel)
      {
        this.channel.Write("#comcl\r\n");
        this.channel.Write(text);
      }
      this.MiConLogger.Trace("MiCon_SendTestPacket: " + text);
    }

    public static string GetMiConRadioTestPacket(uint deviceID)
    {
      return " TEST_PACKET_" + deviceID.ToString().PadLeft(8, '0') + "_TEST_PACKET";
    }

    public override int BytesToRead
    {
      get
      {
        this.ReceiveQueueData();
        return this.ReceiveQueue.Count;
      }
    }

    public RadioTestResult ReceiveOnePacket(
      RadioMode mode,
      int serialnumber,
      ushort timeoutInSec,
      string syncWord = "91D3")
    {
      int timeBeforFirstByte = this.configList.RecTime_BeforFirstByte;
      try
      {
        switch (mode)
        {
          case RadioMode.Radio2:
            this.StartRadio2();
            break;
          case RadioMode.Radio3:
            this.StartRadio3(syncWord);
            break;
          case RadioMode.wMBusS1:
            this.Start_wMBusS1();
            break;
          case RadioMode.wMBusS1M:
            this.Start_wMBusS1M();
            break;
          case RadioMode.wMBusS2:
            this.Start_wMBusS2();
            break;
          case RadioMode.wMBusT1:
            this.Start_wMBusT1();
            break;
          case RadioMode.wMBusT2_meter:
            this.Start_wMBusT2_meter();
            break;
          case RadioMode.wMBusT2_other:
            this.Start_wMBusT2_other();
            break;
          case RadioMode.wMBusC1A:
            this.Start_wMBusC1A();
            break;
          case RadioMode.wMBusC1B:
            this.Start_wMBusC1B();
            break;
          case RadioMode.Radio3_868_95:
            this.StartRadio3_868_95_RUSSIA(syncWord);
            break;
          default:
            throw new Exception("Unknown mode: " + mode.ToString());
        }
        DateTime dateTime = DateTime.Now.AddSeconds((double) timeoutInSec);
        while (DateTime.Now < dateTime)
        {
          this.MiConLogger.Debug(string.Format("ReceiveOnePacket <= Mode:{0}, Serialnumber:{1}, TimeoutInSec:{2}, SyncWord:{3}", (object) mode, (object) serialnumber, (object) timeoutInSec, (object) syncWord));
          ConfigList configList1 = this.configList;
          TimeSpan timeSpan = dateTime.Subtract(DateTime.Now);
          int int32_1 = Convert.ToInt32(timeSpan.TotalMilliseconds);
          configList1.RecTime_BeforFirstByte = int32_1;
          byte[] a = this.ReadHeader(6);
          if (a != null && a.Length == 6 && a[0] == (byte) 170 && a[1] <= (byte) 0)
          {
            int count = ((int) a[2] & (int) sbyte.MaxValue) * 2 + 1;
            ConfigList configList2 = this.configList;
            timeSpan = dateTime.Subtract(DateTime.Now);
            int int32_2 = Convert.ToInt32(timeSpan.TotalMilliseconds);
            configList2.RecTime_BeforFirstByte = int32_2;
            byte[] b = this.ReadHeader(count);
            if (b != null)
            {
              byte[] sourceArray = Util.Combine(a, b);
              byte num1 = 0;
              for (int index = 1; index < sourceArray.Length - 1; ++index)
                num1 += sourceArray[index];
              if ((int) num1 == (int) sourceArray[sourceArray.Length - 1])
              {
                byte[] destinationArray = new byte[sourceArray.Length - 6 - 1];
                Array.Copy((Array) sourceArray, 6, (Array) destinationArray, 0, destinationArray.Length);
                switch (mode)
                {
                  case RadioMode.Radio2:
                  case RadioMode.Radio3:
                  case RadioMode.Radio3_868_95:
                    if ((long) Util.ConvertBcdUInt32ToUInt32(BitConverter.ToUInt32(destinationArray, 2)) == (long) serialnumber)
                    {
                      byte num2 = destinationArray[0];
                      byte rssi = destinationArray[(int) num2 + 1];
                      byte num3 = destinationArray[(int) num2 + 2];
                      uint uint32 = BitConverter.ToUInt32(destinationArray, (int) num2 + 3);
                      int rssiDBm = Util.RssiToRssi_dBm(rssi);
                      return new RadioTestResult()
                      {
                        Payload = destinationArray,
                        RSSI = rssiDBm,
                        LQI = num3,
                        MCT = uint32,
                        ReceiveBuffer = sourceArray
                      };
                    }
                    continue;
                  case RadioMode.wMBusS1:
                  case RadioMode.wMBusS1M:
                  case RadioMode.wMBusS2:
                  case RadioMode.wMBusT1:
                  case RadioMode.wMBusT2_meter:
                  case RadioMode.wMBusT2_other:
                  case RadioMode.wMBusC1A:
                  case RadioMode.wMBusC1B:
                    throw new NotImplementedException();
                  default:
                    return (RadioTestResult) null;
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        return (RadioTestResult) null;
      }
      finally
      {
        if (timeBeforFirstByte == 0)
          this.configList.RecTime_BeforFirstByte = 1;
        else
          this.configList.RecTime_BeforFirstByte = timeBeforFirstByte;
        this.StopRadio();
      }
      return (RadioTestResult) null;
    }

    public System.Version GetTransceiverVersion()
    {
      int startIndex1 = this.MyFunctions.TransceiverDeviceInfo != null ? this.MyFunctions.TransceiverDeviceInfo.IndexOf("Firmware version: ") : throw new Exception("TransceiverDeviceInfo not available");
      if (startIndex1 >= 0)
      {
        int num = this.MyFunctions.TransceiverDeviceInfo.IndexOf(Environment.NewLine, startIndex1);
        if (num > 0)
        {
          int startIndex2 = startIndex1 + "Firmware version: ".Length;
          return System.Version.Parse(this.MyFunctions.TransceiverDeviceInfo.Substring(startIndex2, num - startIndex2));
        }
      }
      throw new Exception("MinoConnect firmware version not found");
    }
  }
}
