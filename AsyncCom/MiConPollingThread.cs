// Decompiled with JetBrains decompiler
// Type: AsyncCom.MiConPollingThread
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using GmmDbLib;
using NLog;
using System;
using ZR_ClassLibrary;

#nullable disable
namespace AsyncCom
{
  internal class MiConPollingThread
  {
    internal MinoConnectSerialPort MyMinoConnectSerialPort;
    internal bool StopThread = false;
    internal bool doPolling = true;
    private int PollingFastCounter = 0;
    private string StatusRequestCommand = string.Empty;
    private static Logger logger = LogManager.GetLogger("MinoConnectPollingThread");
    private const string StateRequestString = "#s\r\n";

    public event System.EventHandler ConnectionLost;

    internal void PollingThreadMain()
    {
      MiConPollingThread.logger.Trace("PollingThreadStarts");
      int num = 10;
      this.MyMinoConnectSerialPort.PollingErrorTime = SystemValues.DateTimeNow.AddMilliseconds((double) this.MyMinoConnectSerialPort.PollingErrorTime_ms);
      this.MyMinoConnectSerialPort.MinoConnectStateError = false;
      DateTime dateTimeNow1 = SystemValues.DateTimeNow;
      DateTime dateTimeNow2;
      while (!this.StopThread && this.MyMinoConnectSerialPort.IsOpen)
      {
        if (this.doPolling)
        {
          try
          {
            this.MyMinoConnectSerialPort.ReceiveQueueData();
            TimeSpan timeSpan = SystemValues.DateTimeNow - this.MyMinoConnectSerialPort.PollingErrorTime;
            if (timeSpan.TotalMilliseconds > 0.0)
            {
              this.MyMinoConnectSerialPort.IsAlive = false;
              if (num < 1)
              {
                ZR_ClassLibMessages.AddErrorDescription(this.MyMinoConnectSerialPort.MyFunctions.MainThreadId, ZR_ClassLibMessages.LastErrors.CommunicationError, Ot.Gtm(Tg.CommunicationLogic, "MiConPollingTimeout", "MinoConnect polling thread timeout"));
                break;
              }
              MinoConnectSerialPort connectSerialPort = this.MyMinoConnectSerialPort;
              dateTimeNow2 = SystemValues.DateTimeNow;
              DateTime dateTime = dateTimeNow2.AddMilliseconds((double) this.MyMinoConnectSerialPort.PollingErrorTime_ms);
              connectSerialPort.PollingErrorTime = dateTime;
              MiConPollingThread.logger.Error<int, double>("Polling thread was interrupted! Try-counter: {0} Diff time: {1} ms", num, timeSpan.TotalMilliseconds);
              --num;
            }
            else if (num < 10 && this.MyMinoConnectSerialPort.IsAlive)
            {
              MiConPollingThread.logger.Error("Reset polling timeout counter");
              num = 10;
            }
            lock (this.MyMinoConnectSerialPort)
            {
              if (this.MyMinoConnectSerialPort.CommandStringForPollingThread == null)
              {
                this.StatusRequestCommand = "#s\r\n";
              }
              else
              {
                if (MiConPollingThread.logger.IsTraceEnabled)
                  MiConPollingThread.logger.Trace("CommandStringForPollingThread: " + ZR_Constants.SystemNewLine + this.MyMinoConnectSerialPort.CommandStringForPollingThread.Trim());
                this.MyMinoConnectSerialPort.UpdateIrDaFilterBy9600RoundSite();
                this.StatusRequestCommand = this.MyMinoConnectSerialPort.CommandStringForPollingThread + "#s\r\n";
                this.MyMinoConnectSerialPort.CommandStringForPollingThread = (string) null;
              }
            }
            lock (this.MyMinoConnectSerialPort.TransmitQueue)
            {
              for (int index = 0; index < this.StatusRequestCommand.Length; ++index)
                this.MyMinoConnectSerialPort.TransmitQueue.Enqueue((byte) this.StatusRequestCommand[index]);
              this.MyMinoConnectSerialPort.TransmitQueueData();
              this.MyMinoConnectSerialPort.MyFunctions.ComWriteLoggerEvent(EventLogger.LoggerEvent.ComSendMinoConnectStatusRequest);
            }
          }
          catch (Exception ex)
          {
            string str = Ot.Gtm(Tg.CommunicationLogic, "MiConPollingError", "MinoConnect polling thread error") + " " + ex.ToString();
            MiConPollingThread.logger.Error(str);
            ZR_ClassLibMessages.AddErrorDescription(this.MyMinoConnectSerialPort.MyFunctions.MainThreadId, ZR_ClassLibMessages.LastErrors.CommunicationError, str);
            break;
          }
          this.MyMinoConnectSerialPort.PollingThreadWorkEvent.WaitOne(this.MyMinoConnectSerialPort.Polling_ms, false);
          if (this.MyMinoConnectSerialPort.Polling_ms != 500)
          {
            --this.PollingFastCounter;
            if (this.PollingFastCounter < 0)
              this.PollingFastCounter = 5;
            if (this.PollingFastCounter == 0)
              this.MyMinoConnectSerialPort.Polling_ms = 500;
          }
        }
        else
        {
          dateTimeNow2 = SystemValues.DateTimeNow;
          DateTime dateTime = dateTimeNow2.AddMilliseconds((double) this.MyMinoConnectSerialPort.PollingErrorTime_ms);
          if (this.MyMinoConnectSerialPort.PollingErrorTime < dateTime)
            this.MyMinoConnectSerialPort.PollingErrorTime = dateTime;
        }
      }
      if (!this.StopThread)
      {
        if (this.ConnectionLost != null)
          this.ConnectionLost((object) this, (EventArgs) null);
        MiConPollingThread.logger.Error("MinoConnect polling thread is stoped!");
        this.MyMinoConnectSerialPort.Close();
      }
      else
        MiConPollingThread.logger.Debug("MinoConnect polling thread was successfully stopped!");
      this.MyMinoConnectSerialPort.MinoConnectStateError = true;
      this.MyMinoConnectSerialPort.MyFunctions.SendAsyncComMessage(new GMM_EventArgs(GMM_EventArgs.MessageType.StatusThreadStopped));
      this.MyMinoConnectSerialPort.Close();
    }

    internal delegate void Start();
  }
}
