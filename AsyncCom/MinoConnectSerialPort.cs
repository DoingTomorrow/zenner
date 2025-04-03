// Decompiled with JetBrains decompiler
// Type: AsyncCom.MinoConnectSerialPort
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using GmmDbLib;
using MinoConnect;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace AsyncCom
{
  internal class MinoConnectSerialPort : Port
  {
    internal MiConPollingThread PollingThreadObj;
    private Thread PollThread;
    private static Logger logger = LogManager.GetLogger(nameof (MinoConnectSerialPort));
    internal Decimal VersionValue;
    private string Version;
    private MinoConnectVersions MiConVersion;
    private bool isTransparent;
    private bool ESC_Reseived = false;
    private bool AnswerActive = false;
    private bool RequiredBreakState = false;
    internal bool MinoConnectStateError;
    internal bool MiConSupplyOverload = false;
    internal bool MiConBatteryLow = false;
    public bool IsAlive = false;
    private ByteField ReceiveLine = new ByteField(20);
    private byte[] LastStatusLine = new byte[7];
    private int StatusAliveCounter = 0;
    private short OutState = 0;
    private byte[] ReadByteBuffer = new byte[1];
    internal Queue<byte> TransmitQueue = new Queue<byte>(300);
    internal Queue<byte> ReceiveQueue = new Queue<byte>(300);
    internal Queue<byte> StatusQueue = new Queue<byte>(100);
    internal int PollingErrorTime_ms = 3000;
    internal DateTime PollingErrorTime;
    internal Port _base;
    internal MinoConnectState StateRequired;
    internal MinoConnectState StateSet;
    internal MinoConnectState StateLastReceived;
    internal AsyncFunctions MyFunctions;
    internal AutoResetEvent PollingThreadWorkEvent = new AutoResetEvent(false);
    internal string CommandStringForPollingThread;
    internal const int Polling_ms_Default = 500;
    internal const int Polling_ms_Fast = 50;
    internal int Polling_ms = 500;
    private AsyncOperation asyncOperation = (AsyncOperation) null;

    public override event System.EventHandler ConnectionLost;

    public override event System.EventHandler BatterieLow;

    public MinoConnectSerialPort(AsyncFunctions MyFunctionsIn)
    {
      this.MyFunctions = MyFunctionsIn;
      this.isTransparent = false;
      this.StateRequired = new MinoConnectState(this);
      this.StateSet = new MinoConnectState(this);
      MinoConnectSerialPort.logger.Trace("MinoConnectSerialPort -> Constructor");
      if (!this.MyFunctions.ComPort.StartsWith("Mi"))
      {
        this._base = (Port) new StandardSerialPort(115200, Parity.None, MyFunctionsIn);
        this._base.IgnoreFramingError = true;
      }
      else
      {
        this._base = (Port) new MiConBLE_SerialPort(MyFunctionsIn.ComPort);
        MyFunctionsIn.RecTime_GlobalOffset += 200;
      }
      if (MyFunctionsIn.Baudrate > 115200)
      {
        MinoConnectSerialPort.logger.Debug("MyFunctionsIn.Baudrate exceeds 115200.");
        throw new ArgumentException("Value to high", "Baudrate");
      }
    }

    internal override bool Open()
    {
      if (MinoConnectSerialPort.logger.IsDebugEnabled)
        MinoConnectSerialPort.logger.Debug("Open calling. IsOpen={0}, ExistPollingThread={1}, PollingThread.StopThread={2}, PollingThread.doPolling={3}, IsTransparent={4}", new object[5]
        {
          (object) this._base.IsOpen,
          (object) (this.PollingThreadObj != null),
          this.PollingThreadObj != null ? (object) Convert.ToString(this.PollingThreadObj.StopThread) : (object) "",
          this.PollingThreadObj != null ? (object) Convert.ToString(this.PollingThreadObj.doPolling) : (object) "",
          (object) this.isTransparent
        });
      if (this._base.IsOpen & (this.PollingThreadObj != null && !this.PollingThreadObj.StopThread && this.PollingThreadObj.doPolling) || this._base.IsOpen && this.isTransparent)
        return true;
      this.MiConSupplyOverload = false;
      this.MiConBatteryLow = false;
      this._base.FramingError = false;
      this.MinoConnectStateError = true;
      this.MyFunctions.SendAsyncComMessage(new GMM_EventArgs(GMM_EventArgs.MessageType.Wait)
      {
        EventMessage = Ot.Gtt(Tg.CommunicationLogic, "MiConOpen", "Open MinoConnect com port ...")
      });
      if (this._base.IsOpen)
        this._base.Close();
      try
      {
        if (!this._base.Open())
          return false;
        if (this.isTransparent)
          return true;
        this.WriteCommand("#com off\r\n");
        this.WriteCommand("#comcl\r\n");
        this.WriteCommand("#broff\r\n");
        if (!this.ReadMinoConnectVersion())
          return false;
        this.StateRequired.SetFromAsyncCom(this.MyFunctions);
        this.UpdateIrDaFilterBy9600RoundSite();
        this.WriteCommand(new MinoConnectState(this).GetChangeCommand(this.StateRequired));
        this.StateSet = new MinoConnectState(this.StateRequired);
        this.StateLastReceived = new MinoConnectState(this);
        this.StateLastReceived.SetFromAsyncCom(this.MyFunctions);
        if (this.asyncOperation == null)
          this.asyncOperation = AsyncOperationManager.CreateOperation((object) null);
        this.StartPolling();
        this._base.DiscardInBuffer();
        this._base.IgnoreFramingError = false;
        this._base.FramingError = false;
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, Ot.Gtm(Tg.CommunicationLogic, "MiConOpenError", "MinoConnect open error") + " " + this.MyFunctions.ComPort);
        MinoConnectSerialPort.logger.Error("MiConOpenError: " + ex.ToString());
        this.Close();
      }
      return false;
    }

    internal void UpdateIrDaFilterBy9600RoundSite()
    {
      string empty = string.Empty;
      string text = !(this.VersionValue < 2.405M) ? "#irf1\r\n" : (!this.StateRequired.IsRequiredIrDaFilter ? "#irf1\r\n" : "#irf0\r\n");
      if (string.IsNullOrEmpty(text))
        return;
      this.WriteCommand(text);
      this.CheckAnswerOfIrDaFilterCommand();
    }

    internal void CheckAnswerOfIrDaFilterCommand()
    {
      try
      {
        string answer = this.ReceiveAnswer();
        if (string.IsNullOrEmpty(answer))
          return;
        MinoConnectSerialPort.logger.Info(answer);
      }
      catch
      {
      }
    }

    internal override void Close()
    {
      MinoConnectSerialPort.logger.Trace("Close called.");
      this.StopPolling();
      this.StateLastReceived = (MinoConnectState) null;
      this._base.Close();
      this.MyFunctions.ComIsOpen = false;
    }

    internal override bool ChangeDriverSettings()
    {
      MinoConnectSerialPort.logger.Trace("Call MinoConnect ChangeDriverSettings");
      if (!this.IsOpen)
      {
        MinoConnectSerialPort.logger.Error("Can't change the MinoConnect settings! The port is closed.");
        return false;
      }
      bool flag = this.PollingThreadObj != null && this.PollingThreadObj.doPolling && !this.PollingThreadObj.StopThread;
      if (!flag)
      {
        MinoConnectSerialPort.logger.Error("Can't change the MinoConnect settings! The polling thread is not running.");
        return false;
      }
      this.StateRequired.SetFromAsyncCom(this.MyFunctions);
      string changeCommand = (this.StateLastReceived ?? new MinoConnectState(this)).GetChangeCommand(this.StateRequired);
      if (string.IsNullOrEmpty(changeCommand))
        MinoConnectSerialPort.logger.Info("The MinoConenct has the same settings!");
      else
        MinoConnectSerialPort.logger.Info("Try change the MinoConnect settings. CMD: {0}", changeCommand);
      lock (this)
        this.CommandStringForPollingThread = changeCommand;
      this.Polling_ms = 50;
      this.PollingThreadWorkEvent.Set();
      this.StateSet = new MinoConnectState(this.StateRequired);
      this.StateLastReceived = new MinoConnectState(this);
      this.StateLastReceived.SetFromAsyncCom(this.MyFunctions);
      for (int index = 30000; flag && this.CommandStringForPollingThread != null && index > 0; index -= 50)
      {
        if (!ZR_ClassLibrary.Util.Wait(50L, "after sends new settings to MinoConnect", (ICancelable) this.MyFunctions, MinoConnectSerialPort.logger))
          return false;
        flag = this.PollingThreadObj != null && this.PollingThreadObj.doPolling && !this.PollingThreadObj.StopThread;
      }
      if (this.CommandStringForPollingThread == null)
        return true;
      MinoConnectSerialPort.logger.Error("Can not sends new settings to MinoConnect!");
      return false;
    }

    internal override bool BreakState
    {
      set
      {
        if (value)
        {
          string text = "#bron\r\n";
          MinoConnectSerialPort.logger.Trace("SetBrakeOn CMD: {0}", text);
          this.WriteCommand(text);
          this.RequiredBreakState = true;
        }
        else
        {
          string text = "#broff\r\n";
          MinoConnectSerialPort.logger.Trace("SetBrakeOff CMD: {0}", text);
          this.WriteCommand(text);
          this.RequiredBreakState = false;
        }
      }
      get => this.RequiredBreakState;
    }

    internal override bool IsOpen => this._base.IsOpen;

    internal override int ReadTimeout
    {
      get => this._base.ReadTimeout;
      set => this._base.ReadTimeout = value;
    }

    internal override int WriteTimeout
    {
      get => this._base.WriteTimeout;
      set => this._base.WriteTimeout = value;
    }

    internal override int BytesToRead
    {
      get
      {
        this.ReceiveQueueData();
        lock (this.ReceiveQueue)
          return this.ReceiveQueue.Count;
      }
    }

    internal override int BytesToWrite
    {
      get
      {
        lock (this.TransmitQueue)
          return this.TransmitQueue.Count;
      }
    }

    internal override void SetRTS(bool state)
    {
      if (state)
        this.OutState |= (short) 1;
      else
        this.OutState &= (short) -2;
      string text = "#out" + this.OutState.ToString() + "\r\n";
      this.WriteCommand(text);
      MinoConnectSerialPort.logger.Trace(" SetOutRTS: " + text);
    }

    internal override void SetDTR(bool state)
    {
      if (state)
        this.OutState |= (short) 2;
      else
        this.OutState &= (short) -3;
      this.WriteCommand("#out" + this.OutState.ToString() + "\r\n");
    }

    internal override void Write(string text)
    {
      if (this.isTransparent)
      {
        this._base.Write(text);
      }
      else
      {
        Exception exception = (Exception) null;
        if (this.PollThread == null)
          throw new Exception(Ot.Gtm(Tg.CommunicationLogic, "MiConPollingError", "MinoConnect polling thread error"));
        if (this.PollThread.Join(0))
          throw new Exception(Ot.Gtm(Tg.CommunicationLogic, "MiConPollingError", "MinoConnect polling thread error"));
        lock (this.TransmitQueue)
        {
          try
          {
            for (int index = 0; index < text.Length; ++index)
            {
              this.TransmitQueue.Enqueue((byte) text[index]);
              if (text[index] == '#')
                this.TransmitQueue.Enqueue((byte) text[index]);
            }
            this.TransmitQueueData();
          }
          catch (Exception ex)
          {
            MinoConnectSerialPort.logger.Error(ex, "Write()->" + ex.Message);
            exception = ex;
          }
        }
        if (exception != null)
          throw exception;
      }
    }

    internal override void Write(byte[] buffer, int offset, int count)
    {
      if (this.isTransparent)
      {
        this._base.Write(buffer, offset, count);
      }
      else
      {
        Exception exception = (Exception) null;
        if (this.PollThread == null)
          throw new Exception(Ot.Gtm(Tg.CommunicationLogic, "MiConPollingError", "MinoConnect polling thread error"));
        if (this.PollThread.Join(0))
          throw new Exception(Ot.Gtm(Tg.CommunicationLogic, "MiConPollingError", "MinoConnect polling thread error"));
        if (this.MinoConnectStateError)
        {
          MinoConnectSerialPort.logger.Error("Read()->MinoConnect state error");
          throw new IOException(Ot.Gtm(Tg.CommunicationLogic, "MiConStateError", "MinoConnect state error"));
        }
        lock (this.TransmitQueue)
        {
          try
          {
            for (int index = 0; index < count; ++index)
            {
              this.TransmitQueue.Enqueue(buffer[offset + index]);
              if (buffer[offset + index] == (byte) 35)
                this.TransmitQueue.Enqueue(buffer[offset + index]);
            }
            this.TransmitQueueData();
          }
          catch (Exception ex)
          {
            MinoConnectSerialPort.logger.Error(ex, "Write error->" + ex.Message);
            exception = ex;
          }
        }
        if (exception != null)
          throw exception;
      }
    }

    internal override bool ReadExistingBytes(out byte[] buffer)
    {
      buffer = (byte[]) null;
      lock (this.ReceiveQueue)
      {
        if (this.ReceiveQueue.Count > 0)
        {
          int count = this.ReceiveQueue.Count;
          buffer = new byte[count];
          for (int index = 0; index < count; ++index)
            buffer[index] = this.ReceiveQueue.Dequeue();
        }
      }
      return true;
    }

    internal override int Read(byte[] buffer, int offset, int count)
    {
      if (this.isTransparent)
        return this._base.Read(buffer, offset, count);
      if (!this.IsOpen)
        throw new InvalidOperationException("MinoConnect port is not open!");
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer), "The buffer should not be empty!");
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset), "The offset is negative!");
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count), "The count is negative!");
      if (buffer.Length - offset < count)
        throw new ArgumentException("The length is invalid!");
      if (this.FramingError)
      {
        this.FramingError = false;
        MinoConnectSerialPort.logger.Error("Read()->Framing error");
        throw new FramingErrorException(Ot.Gtm(Tg.CommunicationLogic, "MiConFramingError", "MinoConnect framing error"));
      }
      if (this.MinoConnectStateError)
      {
        MinoConnectSerialPort.logger.Error("Read()->MinoConnect state error");
        throw new IOException(Ot.Gtm(Tg.CommunicationLogic, "MiConStateError", "MinoConnect state error"));
      }
      this.ReceiveQueueData();
      lock (this.ReceiveQueue)
      {
        if (this.ReceiveQueue.Count < 1)
          return 0;
        int num = Math.Min(this.ReceiveQueue.Count, count);
        for (int index = 0; index < num; ++index)
          buffer[index + offset] = this.ReceiveQueue.Dequeue();
        return num;
      }
    }

    internal override int ReadChar() => this.ReadByte();

    internal override int ReadByte()
    {
      this.Read(this.ReadByteBuffer, 0, 1);
      return (int) this.ReadByteBuffer[0];
    }

    internal override void DiscardOutBuffer()
    {
    }

    internal override void DiscardInBuffer()
    {
      if (!this._base.IsOpen)
        return;
      this.ClearInputBuffer();
    }

    internal override SerialPort GetPort() => this._base.GetPort();

    internal void StartRadio2()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio radio2\r\n");
    }

    internal void StartRadio3()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio radio3\r\n");
    }

    internal void StartRadio4()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio radio4\r\n");
    }

    internal void StopWalkBy() => this.WriteCommand("#com rsoff\r\n");

    internal bool StartMinomatRadioTest(byte networkID)
    {
      if (this.VersionValue < 2.4M)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, Ot.Gtm(Tg.CommunicationLogic, "YouAreUsingOldFirmwareOnMinoConnect", "You are using an MinoConnect with old firmware, this command is not supported by this firmware."));
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#rtest sndinq " + networkID.ToString() + "\r\n");
      return true;
    }

    internal bool StartRadio3_868_95_RUSSIA()
    {
      if (this.VersionValue < 2.4M)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, Ot.Gtm(Tg.CommunicationLogic, "YouAreUsingOldFirmwareOnMinoConnect", "You are using an MinoConnect with old firmware, this command is not supported by this firmware."));
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio radio3_868.95\r\n");
      return true;
    }

    internal bool Start_RadioMS()
    {
      if (this.VersionValue < 2.4M)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, Ot.Gtm(Tg.CommunicationLogic, "YouAreUsingOldFirmwareOnMinoConnect", "You are using an MinoConnect with old firmware, this command is not supported by this firmware."));
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio radio_ms\r\n");
      return true;
    }

    internal bool Start_wMBusS1()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio wmbus_s1\r\n");
      return true;
    }

    internal bool Start_wMBusS1M()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio wmbus_s1m\r\n");
      return true;
    }

    internal bool Start_wMBusS2()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio wmbus_s2\r\n");
      return true;
    }

    internal bool Start_wMBusT1()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio wmbus_t1\r\n");
      return true;
    }

    internal bool Start_wMBusT2_meter()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio wmbus_t2_m\r\n");
      return true;
    }

    internal bool Start_wMBusT2_other()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio wmbus_t2_o\r\n");
      return true;
    }

    internal bool Start_wMBusC1A()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio wmbus_c1a\r\n");
      return true;
    }

    internal bool Start_wMBusC1B()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#com radio wmbus_c1b\r\n");
      return true;
    }

    internal bool StartSendTestPacket(RadioMode radioMode, byte power)
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

    internal bool StopSendTestPacket()
    {
      this.WriteCommand("#comcl\r\n");
      this.WriteCommand("#rtest stop\r\n");
      return true;
    }

    private string ReceiveAnswer()
    {
      DateTime dateTime = SystemValues.DateTimeNow.AddMilliseconds((double) this.MyFunctions.RecTime_BeforFirstByte);
      StringBuilder stringBuilder = new StringBuilder(200);
      while (true)
      {
        char ch;
        do
        {
          do
          {
            do
            {
              if (this._base.BytesToRead != 0)
              {
                if (stringBuilder.Length <= 200)
                {
                  Thread.Sleep(1);
                  try
                  {
                    ch = (char) this._base.ReadByte();
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
                  goto label_6;
              }
              else
                goto label_1;
            }
            while (ch == '\r');
            if (ch != '\n')
              goto label_16;
          }
          while (stringBuilder.Length == 0);
          goto label_18;
label_1:;
        }
        while (!(dateTime < SystemValues.DateTimeNow));
        break;
label_16:
        stringBuilder.Append(ch);
      }
      if (stringBuilder.Length == 0)
        throw new TimeoutException("No data from MinoConnect!");
      throw new TimeoutException("Wrong 'RecTime_BeforFirstByte' parameter! Not all data received from MinoConnect.");
label_6:
      throw new ArgumentOutOfRangeException("Too many received bytes");
label_18:
      return stringBuilder.ToString();
    }

    public void WriteCommand(string text)
    {
      if (MinoConnectSerialPort.logger.IsTraceEnabled)
        MinoConnectSerialPort.logger.Trace(text.Trim());
      Exception exception = (Exception) null;
      lock (this.TransmitQueue)
      {
        try
        {
          for (int index = 0; index < text.Length; ++index)
            this.TransmitQueue.Enqueue((byte) text[index]);
          this.TransmitQueueData();
        }
        catch (Exception ex)
        {
          string message = string.Format("Can not transmit command! Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
          MinoConnectSerialPort.logger.Error(ex, message);
          exception = ex;
        }
      }
      if (exception != null)
        throw exception;
    }

    internal void TransmitQueueData()
    {
      int count = this.TransmitQueue.Count;
      byte[] buffer = new byte[count];
      for (int index = 0; index < count; ++index)
        buffer[index] = this.TransmitQueue.Dequeue();
      this._base.Write(buffer, 0, count);
    }

    internal void ReceiveQueueData()
    {
      byte[] numArray = (byte[]) null;
      lock (this.ReceiveQueue)
      {
        try
        {
          int bytesToRead = this._base.BytesToRead;
          if (bytesToRead != 0)
          {
            byte[] buffer = new byte[bytesToRead];
            int num = this._base.Read(buffer, 0, bytesToRead);
            for (int index = 0; index < num; ++index)
            {
              if (this.AnswerActive)
              {
                this.StatusQueue.Enqueue(buffer[index]);
                if (buffer[index] == (byte) 10)
                  this.AnswerActive = false;
              }
              else if (this.ESC_Reseived)
              {
                if (buffer[index] == (byte) 35)
                {
                  this.ReceiveQueue.Enqueue(buffer[index]);
                }
                else
                {
                  this.StatusQueue.Enqueue((byte) 35);
                  this.StatusQueue.Enqueue(buffer[index]);
                  this.AnswerActive = true;
                }
                this.ESC_Reseived = false;
              }
              else if (buffer[index] == (byte) 35)
                this.ESC_Reseived = true;
              else
                this.ReceiveQueue.Enqueue(buffer[index]);
            }
          }
          numArray = this.ScanReceiveLine();
        }
        catch (Exception ex)
        {
          string message = string.Format("Error in ReceiveQueueData (StatusQueue.Count = {0}, ReceiveQueue.Count = {1}) Error: {2}, Trace: {3}", (object) this.StatusQueue.Count, (object) this.ReceiveQueue.Count, (object) ex.Message, (object) ex.StackTrace);
          MinoConnectSerialPort.logger.Error(ex, message);
        }
      }
      if (numArray == null)
        return;
      if (numArray.Length > 1)
      {
        if (numArray[1] == (byte) 115)
        {
          this.IsAlive = true;
          this.MyFunctions.ComWriteLoggerEvent(EventLogger.LoggerEvent.ComReceiveMinoConnectStatus);
          if (this.StateLastReceived != null && this.StateLastReceived.SetFromReceivedState(numArray))
          {
            if (this.StateLastReceived.StateChanged)
            {
              if (this.StateLastReceived.KeyReceived)
              {
                MinoConnectSerialPort.logger.Info("MinoConnect key event received. KeyReceived!");
                this.SendAsyncComMessageAsynchronously(new GMM_EventArgs(GMM_EventArgs.MessageType.KeyReceived));
              }
              if (this.StateLastReceived.FramingError)
                this.FramingError = true;
              if (!this.MiConSupplyOverload && this.StateLastReceived.Overload)
              {
                this.MiConSupplyOverload = true;
                this.SendAsyncComMessageAsynchronously(new GMM_EventArgs("MinoConnect supply", GMM_EventArgs.MessageType.Overload));
              }
              if (!this.MiConBatteryLow && this.StateLastReceived.BatteryLow)
              {
                if (!this.MiConBatteryLow)
                  this.OnBatterieLow();
                this.MiConBatteryLow = true;
                this.SendAsyncComMessageAsynchronously(new GMM_EventArgs("MinoConnect battery", GMM_EventArgs.MessageType.BatteryLow));
              }
              this.SendAsyncComMessageAsynchronously(new GMM_EventArgs(GMM_EventArgs.MessageType.StatusChanged));
            }
            if (this.StatusAliveCounter > 0)
            {
              this.StatusAliveCounter = 0;
              this.SendAsyncComMessageAsynchronously(new GMM_EventArgs(GMM_EventArgs.MessageType.Alive));
            }
            else
              ++this.StatusAliveCounter;
          }
        }
      }
      else
        this.WorkResponseLine(numArray);
    }

    private void OnBatterieLow()
    {
      System.EventHandler batterieLow = this.BatterieLow;
      if (batterieLow == null)
        return;
      batterieLow((object) this, new EventArgs());
    }

    private void SendAsyncComMessageAsynchronously(GMM_EventArgs msg)
    {
      this.asyncOperation.Post((SendOrPostCallback) (state =>
      {
        try
        {
          this.MyFunctions.SendAsyncComMessage(msg);
        }
        catch (Exception ex)
        {
          string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
          MinoConnectSerialPort.logger.Error(ex, message);
        }
      }), (object) null);
    }

    private byte[] ScanReceiveLine()
    {
      byte[] line = (byte[]) null;
      while (this.StatusQueue.Count > 0)
      {
        if (this.ReceiveLine.Count == this.ReceiveLine.Data.Length)
          this.ReceiveLine.Count = 0;
        byte Byte = this.StatusQueue.Dequeue();
        if (this.ReceiveLine.Count != 0 || Byte == (byte) 35)
        {
          this.ReceiveLine.Add(Byte);
          if (Byte == (byte) 10)
          {
            DateTime dateTime = SystemValues.DateTimeNow.AddMilliseconds((double) this.PollingErrorTime_ms);
            if (this.PollingErrorTime < dateTime)
              this.PollingErrorTime = dateTime;
            line = this.ReceiveLine.GetByteArray();
            this.ReceiveLine.Count = 0;
          }
        }
      }
      return line;
    }

    private void WorkResponseLine(byte[] StatusLine)
    {
      if (StatusLine.Length != 8 || StatusLine[1] != (byte) 70 || StatusLine[2] != (byte) 95 || StatusLine[3] != (byte) 69 || StatusLine[4] != (byte) 82 || StatusLine[5] != (byte) 82)
        return;
      this.FramingError = true;
    }

    private void StartPolling()
    {
      MinoConnectSerialPort.logger.Trace("StartPolling() called.");
      if (this._base.IsOpen && this.PollThread != null && this.PollingThreadObj != null && this.PollingThreadObj.StopThread && this.PollingThreadObj.doPolling)
        MinoConnectSerialPort.logger.Fatal("INTERNAL ERROR: PollingThread already runs!");
      if (this.PollingThreadObj == null)
      {
        this.PollingThreadObj = new MiConPollingThread();
        this.PollingThreadObj.ConnectionLost += new System.EventHandler(this.PollingThreadObj_ConnectionLost);
        this.PollingThreadObj.MyMinoConnectSerialPort = this;
      }
      this.PollingThreadObj.StopThread = false;
      this.PollingThreadObj.doPolling = true;
      if (this.PollThread == null)
      {
        this.PollThread = new Thread(new ThreadStart(new MiConPollingThread.Start(this.PollingThreadObj.PollingThreadMain).Invoke));
        this.PollThread.Name = "MinoConnectPolling";
        this.PollThread.IsBackground = true;
      }
      this.PollThread.Start();
      if (ZR_ClassLibrary.Util.Wait(100L, "after start PollingThread", (ICancelable) this.MyFunctions, MinoConnectSerialPort.logger))
        ;
    }

    private void PollingThreadObj_ConnectionLost(object sender, EventArgs e)
    {
      if (this.ConnectionLost == null)
        return;
      this.ConnectionLost(sender, e);
    }

    private void StopPolling()
    {
      if (this.PollThread == null || this.PollingThreadObj == null)
        return;
      MinoConnectSerialPort.logger.Trace("Stop MinoConnect polling thread.");
      this.PollingThreadObj.StopThread = true;
      this.PollingThreadObj.ConnectionLost -= new System.EventHandler(this.PollingThreadObj_ConnectionLost);
      Application.DoEvents();
      int num = 0;
      while (num < 20 && (this.PollThread == null || !this.PollThread.Join(100)))
        ++num;
      this.PollingThreadObj = (MiConPollingThread) null;
      this.PollThread = (Thread) null;
    }

    internal void SuspendPolling()
    {
      if (this.PollingThreadObj == null)
        return;
      this.PollingThreadObj.doPolling = false;
    }

    internal void ResumePolling()
    {
      if (this.PollingThreadObj == null)
        return;
      this.PollingThreadObj.doPolling = true;
    }

    private bool ReadMinoConnectVersion()
    {
      if (!this._base.IsOpen)
        return false;
      try
      {
        this.MyFunctions.SendAsyncComMessage(new GMM_EventArgs(GMM_EventArgs.MessageType.StatusChanged)
        {
          EventMessage = "Read MinoConnect version..."
        });
        if (!ZR_ClassLibrary.Util.Wait(300L, "before read version of MinoConnect", (ICancelable) this.MyFunctions, MinoConnectSerialPort.logger))
          return false;
        this.ClearInputBuffer();
        this.WriteCommand("#ver\r\n");
        if (!ZR_ClassLibrary.Util.Wait(200L, "after sends '#ver' command to MinoConnect", (ICancelable) this.MyFunctions, MinoConnectSerialPort.logger))
          return false;
        this.Version = this.ReceiveAnswer();
        if (this.Version.Length < 12)
        {
          string str = "MinoConnect: No Answer!";
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
          MinoConnectSerialPort.logger.Error(str);
          this.Close();
          return false;
        }
        MinoConnectSerialPort.logger.Trace(this.Version);
        this.VersionValue = (Decimal) int.Parse(this.Version.Substring(6, 1)) + (Decimal) int.Parse(this.Version.Substring(8, 1)) / 10M + (Decimal) int.Parse(this.Version.Substring(10, 1)) / 1000M;
        try
        {
          this.MiConVersion = new MinoConnectVersions(this.Version);
        }
        catch
        {
        }
        this.Version.Split('|');
        this.MyFunctions.transceiverDeviceInfo = this.Version.Replace("#Ver", "MinoConnect" + ZR_Constants.SystemNewLine + "Firmware version").Replace("|", ZR_Constants.SystemNewLine);
        return true;
      }
      catch (TimeoutException ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Timeout, Ot.Gtm(Tg.CommunicationLogic, "MiConOpenError", "MinoConnect open error") + " " + this.MyFunctions.ComPort);
        MinoConnectSerialPort.logger.Error("MiConOpenError: " + ex.ToString());
        this.Close();
        return false;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, Ot.Gtm(Tg.CommunicationLogic, "MiConOpenError", "MinoConnect open error") + " " + this.MyFunctions.ComPort + " " + ex.ToString());
        MinoConnectSerialPort.logger.Error("MiConOpenError: " + ex.ToString());
        this.Close();
        return false;
      }
    }

    private void MinoConnectCommunicationTest()
    {
      string str1 = "#s\r\n";
      string empty = string.Empty;
      string str2 = "Send:'" + ParameterService.GetExpandesString(str1) + "';Receive:'";
      while (true)
      {
        this._base.Write(str1);
        string answer = this.ReceiveAnswer();
        Application.DoEvents();
        MinoConnectSerialPort.logger.Trace(str2 + answer + "'");
      }
    }

    private void ClearInputBuffer()
    {
      lock (this.ReceiveQueue)
      {
        do
        {
          this.ReceiveQueueData();
          if (this.ReceiveQueue.Count > 0)
          {
            byte[] array = this.ReceiveQueue.ToArray();
            if (array != null && array.Length != 0 && MinoConnectSerialPort.logger.IsTraceEnabled)
              MinoConnectSerialPort.logger.Trace("Clear input buffer: " + ZR_ClassLibrary.Util.ByteArrayToHexString(array));
            this.ReceiveQueue.Clear();
            this.ReceiveQueueData();
          }
        }
        while (this.ReceiveQueue.Count > 0);
      }
    }

    internal bool SetTransparentMode(bool enable)
    {
      if (!this._base.IsOpen && !this._base.Open())
        return false;
      if (enable)
      {
        this.WriteCommand("#apo 0\r\n");
        this.WriteCommand("#coff\r\n");
        this.isTransparent = true;
        this.StopPolling();
        return true;
      }
      this._base.Write("aaafTzhuZl5c39zUNdWq105bmysloncwalnNIK783BH89kirEWmIkPl(!56)bfrtg984!?eV&29IkoPmt!$ymncSrtIopQ'+*bg%ad279vRzOp;-_4y78JI08NJde6HjiOx");
      this.isTransparent = false;
      this.Close();
      return this.Open();
    }

    internal bool SendTestPacket(BusMode busMode, byte[] buffer)
    {
      throw new NotImplementedException();
    }
  }
}
