// Decompiled with JetBrains decompiler
// Type: AsyncCom.AsyncSerial
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using GmmDbLib;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace AsyncCom
{
  internal class AsyncSerial(AsyncFunctions MyParent) : AsyncFunctionsBase(MyParent)
  {
    internal Port MySerialPort;
    private static Logger AsyncSerialLogger = LogManager.GetLogger(nameof (AsyncSerial));
    private Queue<byte> MinoHeadReceiverQueue = new Queue<byte>();
    private bool MinoHeadIsTransparent = false;

    public override event System.EventHandler ConnectionLost;

    public override event System.EventHandler BatterieLow;

    public override long InputBufferLength => (long) this.MySerialPort.BytesToRead;

    public override bool Open()
    {
      if (this.local_Open())
        return true;
      if (this.MyAsyncFunctions.ErrorMessageBox)
        this.MyAsyncFunctions.AsyncComMessageBox(ZR_ClassLibMessages.GetLastErrorMessageAndClearError());
      return false;
    }

    private string GetResString(string name) => this.MyAsyncFunctions.MyRes.GetString(name);

    private bool local_Open()
    {
      Parity parity;
      switch (this.MyAsyncFunctions.Parity)
      {
        case "no":
          parity = Parity.None;
          break;
        case "odd":
          parity = Parity.Odd;
          break;
        case "even":
          parity = Parity.Even;
          break;
        default:
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Wrong parity.");
          return false;
      }
      if (this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoConnect)
      {
        if (this.MySerialPort == null || !(this.MySerialPort is MinoConnectSerialPort))
        {
          this.MySerialPort = (Port) new MinoConnectSerialPort(this.MyAsyncFunctions);
          this.MySerialPort.ConnectionLost += new System.EventHandler(this.MySerialPort_ConnectionLost);
          this.MySerialPort.BatterieLow += new System.EventHandler(this.MySerialPort_BatterieLow);
        }
      }
      else
        this.MySerialPort = (Port) new StandardSerialPort(this.MyAsyncFunctions.Baudrate, parity, this.MyAsyncFunctions);
      if (this.MySerialPort == null)
      {
        AsyncSerial.AsyncSerialLogger.Debug("Can not create com object.");
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Can not create com object.");
        this.MyAsyncFunctions.ComIsOpen = false;
        return false;
      }
      try
      {
        this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComOpen);
        this.MyAsyncFunctions.transceiverDeviceInfo = "";
        this.MySerialPort.ReadTimeout = this.MyAsyncFunctions.WaitBeforeRepeatTime;
        this.MySerialPort.WriteTimeout = 3000;
        if (!this.MySerialPort.Open())
          return false;
        if (this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoHead)
        {
          AsyncSerial.AsyncSerialLogger.Info("Connecting MinoHead ...");
          this.MyAsyncFunctions.SendAsyncComMessage(new GMM_EventArgs(GMM_EventArgs.MessageType.StatusChanged)
          {
            EventMessage = "Connecting MinoHead ..."
          });
          int num1 = 0;
          short num2 = 0;
          short num3 = 0;
          short num4 = 0;
          do
          {
            this.SendMinoHeadWakeup();
            int num5;
            try
            {
              num2 = this.getHeadVersion();
              num5 = 0;
            }
            catch (TimeoutException ex)
            {
              AsyncSerial.AsyncSerialLogger.Error("Can not get MinoHead version Error: {0}", ex.Message);
              num5 = num1 + 1;
              if (num5 > 5)
              {
                ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Timeout, this.GetResString("MinoHeadNotFound"));
                this.MySerialPort.Close();
                return false;
              }
            }
            catch (Exception ex)
            {
              AsyncSerial.AsyncSerialLogger.Error("Can not get MinoHead version Error: {0}", ex.Message);
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, this.GetResString("MinoHeadNotFound") + ZR_Constants.SystemNewLine + ZR_Constants.SystemNewLine + ex.ToString());
              this.MySerialPort.Close();
              return false;
            }
            int num6;
            try
            {
              num3 = this.getEchoIrDa();
              num6 = 0;
            }
            catch (TimeoutException ex)
            {
              AsyncSerial.AsyncSerialLogger.Error("Can not getEchoIrDa Error: {0}", ex.Message);
              num6 = num5 + 1;
              if (num6 > 5)
              {
                ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Timeout, this.GetResString("MinoHeadNotFound"));
                this.MySerialPort.Close();
                return false;
              }
            }
            catch (Exception ex)
            {
              AsyncSerial.AsyncSerialLogger.Error("Can not getEchoIrDa Error: {0}", ex.Message);
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, this.GetResString("MinoHeadNotFound") + ZR_Constants.SystemNewLine + ZR_Constants.SystemNewLine + ex.ToString());
              this.MySerialPort.Close();
              return false;
            }
            try
            {
              num4 = this.getRFModul();
              num1 = 0;
            }
            catch (TimeoutException ex)
            {
              AsyncSerial.AsyncSerialLogger.Error("Can not getRFModul Error: {0}", ex.Message);
              num1 = num6 + 1;
              if (num1 > 5)
              {
                ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Timeout, this.GetResString("MinoHeadNotFound"));
                this.MySerialPort.Close();
                return false;
              }
            }
            catch (Exception ex)
            {
              AsyncSerial.AsyncSerialLogger.Error("Can not getRFModul Error: {0}", ex.Message);
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, this.GetResString("MinoHeadNotFound") + ZR_Constants.SystemNewLine + ZR_Constants.SystemNewLine + ex.ToString());
              this.MySerialPort.Close();
              return false;
            }
          }
          while (num1 > 0);
          if (num2 < (short) 6)
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, this.GetResString("MinoHeadNotFound") + ZR_Constants.SystemNewLine + ZR_Constants.SystemNewLine + "Wrong version: " + num2.ToString());
            AsyncSerial.AsyncSerialLogger.Debug("MinoHeadNotFound: wrong Version: " + num2.ToString());
            this.MySerialPort.Close();
            return false;
          }
          if (this.MyAsyncFunctions.transceiverDeviceInfo.Length > 0)
          {
            AsyncFunctions asyncFunctions = this.MyAsyncFunctions;
            asyncFunctions.transceiverDeviceInfo = asyncFunctions.transceiverDeviceInfo + ZR_Constants.SystemNewLine + ZR_Constants.SystemNewLine + "------------------------" + ZR_Constants.SystemNewLine;
          }
          AsyncFunctions asyncFunctions1 = this.MyAsyncFunctions;
          asyncFunctions1.transceiverDeviceInfo = asyncFunctions1.transceiverDeviceInfo + "Minohead" + ZR_Constants.SystemNewLine + "HeadVersion: " + num2.ToString() + ZR_Constants.SystemNewLine + "IrdaVersion: " + num3.ToString() + ZR_Constants.SystemNewLine + "RFVersion: " + num4.ToString();
          if (AsyncSerial.AsyncSerialLogger.IsDebugEnabled)
            AsyncSerial.AsyncSerialLogger.Debug("Minohead found: " + ZR_Constants.SystemNewLine + "HeadVersion: " + num2.ToString() + ZR_Constants.SystemNewLine + "IrdaVersion: " + num3.ToString() + ZR_Constants.SystemNewLine + "RFVersion: " + num4.ToString());
        }
      }
      catch (IOException ex)
      {
        this.MyAsyncFunctions.ComIsOpen = false;
        if (this.MySerialPort.IsOpen)
          this.MySerialPort.Close();
        this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComIOException);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.ComOpenError, this.MyAsyncFunctions.MyRes.GetString("ComOpenError"));
        AsyncSerial.AsyncSerialLogger.Error(ex.InnerException, this.GetResString("ComOpenError"));
        return false;
      }
      catch (Exception ex)
      {
        if (this.MySerialPort.IsOpen)
          this.MySerialPort.Close();
        this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComUnknownError);
        this.MyAsyncFunctions.ComIsOpen = false;
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.ComOpenError, this.GetResString("ComOpenError"));
        AsyncSerial.AsyncSerialLogger.Error(ex.InnerException, this.GetResString("ComOpenError"));
        return false;
      }
      this.MyAsyncFunctions.ComIsOpen = true;
      if (this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoIR)
      {
        if (this.MySerialPort is StandardSerialPort)
        {
          this.MySerialPort.SetRTS(true);
          this.MySerialPort.SetDTR(false);
          if (!ZR_ClassLibrary.Util.Wait((long) this.MyAsyncFunctions.TransTime_AfterOpen, "MinoIR->TransTime_AfterOpen", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
            return false;
          this.MySerialPort.SetDTR(true);
          if (!ZR_ClassLibrary.Util.Wait(200L, "MinoIR->SetDTR(true)", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
            return false;
          this.MySerialPort.SetDTR(false);
          if (!ZR_ClassLibrary.Util.Wait(400L, "MinoIR->SetDTR(false)", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
            return false;
        }
        else
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented, "MinoIR handshaking not yet supported");
          AsyncSerial.AsyncSerialLogger.Error("MinoIR handshaking not yet supported");
          this.MySerialPort.Close();
          return false;
        }
      }
      else if (!ZR_ClassLibrary.Util.Wait((long) this.MyAsyncFunctions.TransTime_AfterOpen, "TransTime_AfterOpen", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
        return false;
      if (this.MyAsyncFunctions.TestEcho && !this.MyAsyncFunctions.EchoTestIsActive)
      {
        AsyncSerial.AsyncSerialLogger.Debug("Testing for echo");
        this.MyAsyncFunctions.EchoTestIsActive = true;
        ByteField DataBlock1 = new ByteField(6);
        DataBlock1.Add(85);
        DataBlock1.Add(170);
        DataBlock1.Add(90);
        DataBlock1.Add(165);
        DataBlock1.Add(15);
        DataBlock1.Add(240);
        this.MyAsyncFunctions.EchoOn = false;
        this.TransmitBlock(ref DataBlock1);
        if (!ZR_ClassLibrary.Util.Wait((long) (int) (this.MyAsyncFunctions.ByteTime * 6.0 + 100.0), "local_Open->TestEcho", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
          return false;
        this.MyAsyncFunctions.EchoOn = true;
        ByteField DataBlock2 = new ByteField(6);
        if (this.ReceiveBlock(ref DataBlock2, 6, true))
        {
          for (int index = 0; index < 6; ++index)
          {
            if ((int) DataBlock2.Data[index] != (int) DataBlock1.Data[index])
            {
              this.MyAsyncFunctions.EchoOn = false;
              break;
            }
          }
        }
        else
          this.MyAsyncFunctions.EchoOn = false;
        AsyncSerial.AsyncSerialLogger.Debug("EchoOn is " + this.MyAsyncFunctions.EchoOn.ToString());
        if (this.MyAsyncFunctions.ComWindow != null)
          this.MyAsyncFunctions.ComWindow.ShowEcho();
        this.MyAsyncFunctions.EchoTestIsActive = false;
      }
      if (this.MyAsyncFunctions.ComWindow != null)
        this.MyAsyncFunctions.ComWindow.SetComState();
      this.MyAsyncFunctions.LastWakeupRefreshTime = DateTime.MinValue;
      this.MyAsyncFunctions.SendAsyncComMessage(new GMM_EventArgs(GMM_EventArgs.MessageType.StatusChanged)
      {
        EventMessage = "Com opened"
      });
      return true;
    }

    private void MySerialPort_ConnectionLost(object sender, EventArgs e)
    {
      if (this.ConnectionLost == null)
        return;
      this.ConnectionLost(sender, e);
    }

    private void MySerialPort_BatterieLow(object sender, EventArgs e)
    {
      if (this.BatterieLow == null)
        return;
      this.BatterieLow(sender, e);
    }

    public override bool Close()
    {
      this.MyAsyncFunctions.ComIsOpen = false;
      this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComClose);
      if (this.MySerialPort == null)
        return false;
      try
      {
        this.MySerialPort.Close();
      }
      catch (IOException ex)
      {
        AsyncSerial.AsyncSerialLogger.Error((Exception) ex, "Failed close the port!");
        this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComIOException);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "ComIOException");
        return false;
      }
      catch (Exception ex)
      {
        AsyncSerial.AsyncSerialLogger.Error(ex, "Failed close the port!");
        this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComUnknownError);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "ComUnknownError");
        return false;
      }
      this.MyAsyncFunctions.SendAsyncComMessage(new GMM_EventArgs(GMM_EventArgs.MessageType.StatusChanged)
      {
        EventMessage = "Com closed"
      });
      return true;
    }

    public override void ClearCom()
    {
      this.MyAsyncFunctions.LineBuffer.Length = 0;
      this.MySerialPort.FramingError = false;
      this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComClearReceiver);
      if (!this.MySerialPort.IsOpen)
        return;
      try
      {
        if (this.MinoHeadReceiverQueue != null && this.MinoHeadReceiverQueue.Count > 0)
        {
          AsyncSerial.AsyncSerialLogger.Error<int, string>("Clear MinoHeadReceiverQueue SIZE: {0}, BUFFER: {1}", this.MinoHeadReceiverQueue.Count, ZR_ClassLibrary.Util.ByteArrayToHexString(this.MinoHeadReceiverQueue.ToArray()));
          this.MinoHeadReceiverQueue.Clear();
        }
        if (this.MySerialPort.BytesToWrite > 0)
        {
          AsyncSerial.AsyncSerialLogger.Error("DiscardOutBuffer {0} bytes", this.MySerialPort.BytesToWrite);
          this.MySerialPort.DiscardOutBuffer();
        }
        if (this.MySerialPort.BytesToRead > 0)
        {
          byte[] buffer = new byte[this.MySerialPort.BytesToRead];
          this.MySerialPort.Read(buffer, 0, buffer.Length);
          if (AsyncSerial.AsyncSerialLogger.IsTraceEnabled)
            AsyncSerial.AsyncSerialLogger.Trace<int, string>("DiscardInBuffer {0} bytes: {1}", buffer.Length, ZR_ClassLibrary.Util.ByteArrayToHexString(buffer));
          this.MySerialPort.DiscardInBuffer();
        }
        Thread.Sleep(0);
      }
      catch (Exception ex)
      {
        this.WorkException(ex, (ByteField) null, this.MySerialPort.WriteTimeout);
      }
    }

    public override void ClearComErrors() => this.MySerialPort.FramingError = false;

    public override bool ClearBreak()
    {
      if (!this.MySerialPort.IsOpen)
        return false;
      try
      {
        this.ClearCom();
        this.MySerialPort.BreakState = false;
        this.ClearCom();
      }
      catch (Exception ex)
      {
        this.WorkException(ex, (ByteField) null, this.MySerialPort.WriteTimeout);
        return false;
      }
      this.MyAsyncFunctions.LastWakeupRefreshTime = SystemValues.DateTimeNow;
      return true;
    }

    public override bool SetBreak()
    {
      if (!this.MySerialPort.IsOpen)
        return false;
      try
      {
        this.ClearCom();
        this.MySerialPort.BreakState = true;
      }
      catch (Exception ex)
      {
        this.WorkException(ex, (ByteField) null, this.MySerialPort.WriteTimeout);
        return false;
      }
      return true;
    }

    public override void TestComState()
    {
    }

    public override bool SetHandshakeState(HandshakeStates HandshakeState)
    {
      if (!this.MySerialPort.IsOpen)
        return false;
      try
      {
        this.MySerialPort.Handshake = (Handshake) HandshakeState;
      }
      catch (Exception ex)
      {
        this.WorkException(ex, (ByteField) null, this.MySerialPort.WriteTimeout);
        return false;
      }
      return true;
    }

    internal void ManageWakeup()
    {
      if (!this.MySerialPort.IsOpen || this.MyAsyncFunctions.Wakeup == WakeupSystem.None || this.MyAsyncFunctions.WakeupTemporaryOff)
        return;
      double totalMilliseconds = SystemValues.DateTimeNow.Subtract(this.MyAsyncFunctions.LastWakeupRefreshTime).TotalMilliseconds;
      double wakeupIntervalTime = (double) this.MyAsyncFunctions.WakeupIntervalTime;
      bool flag1 = totalMilliseconds > wakeupIntervalTime || totalMilliseconds == wakeupIntervalTime;
      if (this.MySerialPort.WriteTimeout < this.MyAsyncFunctions.TransTime_BreakTime)
        this.MySerialPort.WriteTimeout = this.MyAsyncFunctions.TransTime_BreakTime;
      if (!flag1)
        return;
      if (this.MyAsyncFunctions.Wakeup == WakeupSystem.MinoHead)
      {
        try
        {
          this.SendMinoHeadWakeup();
        }
        catch (Exception ex)
        {
          AsyncSerial.AsyncSerialLogger.Error("Failed SendMinoHeadWakeup! Error: " + ex.Message);
        }
      }
      else
      {
        this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComTransmitBreak);
        if (this.MyAsyncFunctions.Wakeup == WakeupSystem.Break)
        {
          try
          {
            Application.DoEvents();
            AsyncSerial.AsyncSerialLogger.Info("Break signal on");
            this.MySerialPort.BreakState = true;
            if (!ZR_ClassLibrary.Util.Wait((long) this.MyAsyncFunctions.TransTime_BreakTime, "ManageWakeup->TransTime_BreakTime: send break", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
              return;
            this.MySerialPort.BreakState = false;
            AsyncSerial.AsyncSerialLogger.Info("... break signal off");
          }
          catch (Exception ex)
          {
            this.WorkException(ex, (ByteField) null, this.MySerialPort.WriteTimeout);
          }
        }
        else if (this.MyAsyncFunctions.Wakeup == WakeupSystem.BaudrateCarrier)
        {
          int numberOfBytes = !(this.MyAsyncFunctions.Parity != "no") ? (int) ((double) this.MyAsyncFunctions.Baudrate / 10.0 * ((double) this.MyAsyncFunctions.TransTime_BreakTime / 1000.0)) : (int) ((double) this.MyAsyncFunctions.Baudrate / 11.0 * ((double) this.MyAsyncFunctions.TransTime_BreakTime / 1000.0));
          AsyncSerial.AsyncSerialLogger.Info("Start transmit baudrate carrier. " + numberOfBytes.ToString() + " bytes");
          bool flag2 = true;
          if (this.MySerialPort is MinoConnectSerialPort)
          {
            double millisecondsTimeout = !(this.MyAsyncFunctions.Parity != "no") ? (double) numberOfBytes * 1000.0 / ((double) this.MyAsyncFunctions.Baudrate / 10.0) : (double) numberOfBytes * 1000.0 / ((double) this.MyAsyncFunctions.Baudrate / 11.0);
            MinoConnectSerialPort serialPort = (MinoConnectSerialPort) this.MySerialPort;
            serialPort.PollingErrorTime = SystemValues.DateTimeNow.AddMilliseconds(millisecondsTimeout + (double) this.MyAsyncFunctions.TransTime_BreakTime + (double) serialPort.PollingErrorTime_ms);
            if (serialPort._base is MiConBLE_SerialPort)
            {
              ((MiConBLE_SerialPort) serialPort._base).BLE_Channel.WriteBaudrateCarrier(numberOfBytes);
              Thread.Sleep((int) millisecondsTimeout);
              flag2 = false;
            }
          }
          if (flag2)
          {
            byte[] buffer = new byte[numberOfBytes];
            for (int index = 0; index < buffer.Length; ++index)
              buffer[index] = (byte) 85;
            this.Write(buffer, 0, buffer.Length);
          }
          AsyncSerial.AsyncSerialLogger.Info("... transmit baudrate carrier done");
        }
        this.MyAsyncFunctions.LastWakeupRefreshTime = SystemValues.DateTimeNow;
        if (AsyncSerial.AsyncSerialLogger.IsTraceEnabled)
          AsyncSerial.AsyncSerialLogger.Trace("Start TransTime_AfterBreak: " + this.MyAsyncFunctions.TransTime_AfterBreak.ToString() + " ms");
        Thread.Sleep(this.MyAsyncFunctions.TransTime_AfterBreak);
        AsyncSerial.AsyncSerialLogger.Trace("TransTime_AfterBreak time finished.");
      }
    }

    public override bool CallTransceiverDeviceFunction(
      TransceiverDeviceFunction function,
      object param1,
      object param2)
    {
      switch (function)
      {
        case TransceiverDeviceFunction.TransparentModeOn:
          switch (this.MyAsyncFunctions.Transceiver)
          {
            case TransceiverDevice.None:
            case TransceiverDevice.MinoIR:
              break;
            case TransceiverDevice.MinoConnect:
              if (!(this.MySerialPort is MinoConnectSerialPort))
                throw new ArgumentException("MySerialPort");
              ((MinoConnectSerialPort) this.MySerialPort).SetTransparentMode(true);
              break;
            case TransceiverDevice.MinoHead:
              this.setHeadTransparent();
              break;
            default:
              throw new NotImplementedException();
          }
          break;
        case TransceiverDeviceFunction.TransparentModeV3On:
          switch (this.MyAsyncFunctions.Transceiver)
          {
            case TransceiverDevice.None:
            case TransceiverDevice.MinoConnect:
            case TransceiverDevice.MinoIR:
              break;
            case TransceiverDevice.MinoHead:
              this.setHeadTransparentV3();
              break;
            default:
              throw new NotImplementedException();
          }
          break;
        case TransceiverDeviceFunction.TransparentModeOff:
          if (this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoConnect)
            return this.MySerialPort is MinoConnectSerialPort ? ((MinoConnectSerialPort) this.MySerialPort).SetTransparentMode(false) : throw new ArgumentException("MySerialPort");
          break;
        case TransceiverDeviceFunction.EnableMinoConnectPolling:
          if (this.MySerialPort is MinoConnectSerialPort)
          {
            ((MinoConnectSerialPort) this.MySerialPort).ResumePolling();
            break;
          }
          break;
        case TransceiverDeviceFunction.DisableMinoConnectPolling:
          if (this.MySerialPort is MinoConnectSerialPort)
            ((MinoConnectSerialPort) this.MySerialPort).SuspendPolling();
          if (!ZR_ClassLibrary.Util.Wait(300L, "Wait after DisableMinoConnectPolling", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
            return false;
          this.MySerialPort.DiscardInBuffer();
          break;
        case TransceiverDeviceFunction.StopRadio:
          AsyncSerial.AsyncSerialLogger.Info("Stop radio");
          if (this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoHead)
          {
            this.SendMinoHeadWakeup();
            break;
          }
          if (this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoConnect)
          {
            if (!(this.MySerialPort is MinoConnectSerialPort))
              throw new ArgumentException();
            ((MinoConnectSerialPort) this.MySerialPort).StopWalkBy();
            break;
          }
          break;
        case TransceiverDeviceFunction.StartRadio2:
          AsyncSerial.AsyncSerialLogger.Info("StartWalkByRadio2");
          if (this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoHead)
          {
            if (!this.MinoHeadSendData((byte) 105, (byte[]) null, 0) || !ZR_ClassLibrary.Util.Wait(1000L, "after turn on the radio 2", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
              return false;
            this.ClearCom();
            return true;
          }
          if (this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoConnect)
          {
            if (!(this.MySerialPort is MinoConnectSerialPort))
              throw new ArgumentException();
            ((MinoConnectSerialPort) this.MySerialPort).StartRadio2();
            break;
          }
          break;
        case TransceiverDeviceFunction.StartRadio3:
          AsyncSerial.AsyncSerialLogger.Info("StartWalkByRadio3");
          if (this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoHead)
          {
            if (!this.MinoHeadSendData((byte) 106, (byte[]) null, 0) || !ZR_ClassLibrary.Util.Wait(1000L, "after turn on the radio 2", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
              return false;
            this.ClearCom();
            return true;
          }
          if (this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoConnect)
          {
            if (!(this.MySerialPort is MinoConnectSerialPort))
              throw new ArgumentException();
            ((MinoConnectSerialPort) this.MySerialPort).StartRadio3();
            break;
          }
          break;
        case TransceiverDeviceFunction.StartRadio4:
          AsyncSerial.AsyncSerialLogger.Info("StartWalkByRadio4");
          if (this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoConnect)
          {
            if (!(this.MySerialPort is MinoConnectSerialPort))
              throw new ArgumentException();
            ((MinoConnectSerialPort) this.MySerialPort).StartRadio4();
            break;
          }
          break;
        case TransceiverDeviceFunction.StartMinomatradioTest:
        case TransceiverDeviceFunction.Start_wMBusS1:
        case TransceiverDeviceFunction.Start_wMBusS1M:
        case TransceiverDeviceFunction.Start_wMBusS2:
        case TransceiverDeviceFunction.Start_wMBusT1:
        case TransceiverDeviceFunction.Start_wMBusT2_meter:
        case TransceiverDeviceFunction.Start_wMBusT2_other:
        case TransceiverDeviceFunction.Start_wMBusC1A:
        case TransceiverDeviceFunction.Start_wMBusC1B:
        case TransceiverDeviceFunction.Start_RadioMS:
        case TransceiverDeviceFunction.StartRadio3_868_95_RUSSIA:
          AsyncSerial.AsyncSerialLogger.Info(function.ToString());
          if (this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoConnect)
          {
            MinoConnectSerialPort connectSerialPort = this.MySerialPort is MinoConnectSerialPort ? this.MySerialPort as MinoConnectSerialPort : throw new ArgumentException();
            switch (function)
            {
              case TransceiverDeviceFunction.StartMinomatradioTest:
                return connectSerialPort.StartMinomatRadioTest(Convert.ToByte(param1));
              case TransceiverDeviceFunction.Start_wMBusS1:
                return connectSerialPort.Start_wMBusS1();
              case TransceiverDeviceFunction.Start_wMBusS1M:
                return connectSerialPort.Start_wMBusS1M();
              case TransceiverDeviceFunction.Start_wMBusS2:
                return connectSerialPort.Start_wMBusS2();
              case TransceiverDeviceFunction.Start_wMBusT1:
                return connectSerialPort.Start_wMBusT1();
              case TransceiverDeviceFunction.Start_wMBusT2_meter:
                return connectSerialPort.Start_wMBusT2_meter();
              case TransceiverDeviceFunction.Start_wMBusT2_other:
                return connectSerialPort.Start_wMBusT2_other();
              case TransceiverDeviceFunction.Start_wMBusC1A:
                return connectSerialPort.Start_wMBusC1A();
              case TransceiverDeviceFunction.Start_wMBusC1B:
                return connectSerialPort.Start_wMBusC1B();
              case TransceiverDeviceFunction.Start_RadioMS:
                return connectSerialPort.Start_RadioMS();
              case TransceiverDeviceFunction.StartRadio3_868_95_RUSSIA:
                return connectSerialPort.StartRadio3_868_95_RUSSIA();
            }
          }
          else
            break;
          break;
        case TransceiverDeviceFunction.StartSendTestPacket:
          AsyncSerial.AsyncSerialLogger.Info("Start send test packet");
          if (this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoConnect)
          {
            if (param1 == null)
              throw new ArgumentException();
            if (this.MySerialPort is MinoConnectSerialPort)
              return ((MinoConnectSerialPort) this.MySerialPort).StartSendTestPacket((RadioMode) param1, (byte) param2);
            throw new ArgumentException();
          }
          break;
        case TransceiverDeviceFunction.StopSendTestPacket:
          AsyncSerial.AsyncSerialLogger.Info("Stop send test packet");
          if (this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoConnect)
            return this.MySerialPort is MinoConnectSerialPort ? ((MinoConnectSerialPort) this.MySerialPort).StopSendTestPacket() : throw new ArgumentException();
          break;
        case TransceiverDeviceFunction.SendTestPacket:
          AsyncSerial.AsyncSerialLogger.Info("SendTestPacket " + param1?.ToString() + " " + param2?.ToString());
          if (this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoConnect)
          {
            if (param1 == null)
              throw new ArgumentException(nameof (param1));
            if (param2 == null)
              throw new ArgumentException(nameof (param2));
            if (this.MySerialPort is MinoConnectSerialPort)
              return ((MinoConnectSerialPort) this.MySerialPort).SendTestPacket((BusMode) param1, (byte[]) param2);
            throw new ArgumentException();
          }
          break;
        default:
          throw new NotImplementedException();
      }
      return true;
    }

    public override bool TransmitString(string DataString)
    {
      if (!this.MySerialPort.IsOpen)
        return false;
      try
      {
        this.MySerialPort.Write(DataString);
      }
      catch (Exception ex)
      {
        this.WorkException(ex, (ByteField) null, this.MySerialPort.WriteTimeout);
        return false;
      }
      return true;
    }

    public override bool TransmitBlock(string DataString)
    {
      ByteField DataBlock = new ByteField(DataString.Length);
      for (int index = 0; index < DataString.Length; ++index)
        DataBlock.Add((byte) DataString[index]);
      return this.TransmitBlock(ref DataBlock);
    }

    public override bool TransmitBlock(ref ByteField DataBlock)
    {
      if (!this.MySerialPort.IsOpen)
        return false;
      this.MyAsyncFunctions.WaitToEarliestTransmitTime();
      this.ManageWakeup();
      this.ClearCom();
      this.MyAsyncFunctions.Logger.WriteLoggerData(EventLogger.LoggerEvent.ComTransmitData, ref DataBlock);
      try
      {
        this.Write(DataBlock.Data, 0, DataBlock.Count);
      }
      catch (Exception ex)
      {
        this.ResetLastTransmitEndTime();
        this.WorkException(ex, DataBlock, this.MySerialPort.WriteTimeout);
        return false;
      }
      this.ResetLastTransmitEndTime();
      if (this.MyAsyncFunctions.EchoOn)
      {
        ByteField byteField = new ByteField(DataBlock.Count);
        bool flag = this.ReceiveBlock(ref byteField, DataBlock.Count, false);
        if (AsyncSerial.AsyncSerialLogger.IsDebugEnabled)
          AsyncSerial.AsyncSerialLogger.Debug("<TransEcho." + byteField.GetTraceString());
        if (flag)
        {
          for (int index = 0; index < DataBlock.Count; ++index)
          {
            if ((int) DataBlock.Data[index] != (int) byteField.Data[index])
              flag = false;
          }
        }
        if (!flag)
        {
          this.MyAsyncFunctions.Logger.WriteLoggerData(EventLogger.LoggerEvent.ComEchoError, ref byteField);
          AsyncSerial.AsyncSerialLogger.Debug("EchoError");
          return false;
        }
        this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComEchoOk);
        AsyncSerial.AsyncSerialLogger.Debug("EchoOk");
      }
      return true;
    }

    public override bool SendBlock(ref ByteField DataBlock)
    {
      if (!this.MySerialPort.IsOpen)
        return false;
      try
      {
        this.MyAsyncFunctions.LastTransmitEndTime = SystemValues.DateTimeNow.AddMilliseconds(this.MyAsyncFunctions.ByteTime * (double) DataBlock.Count);
        this.Write(DataBlock.Data, 0, DataBlock.Count);
        if (this.MyAsyncFunctions.EchoOn)
        {
          ByteField byteField = new ByteField(DataBlock.Count);
          bool flag = this.ReceiveBlock(ref byteField, DataBlock.Count, false);
          if (flag)
          {
            for (int index = 0; index < DataBlock.Count; ++index)
            {
              if ((int) DataBlock.Data[index] != (int) byteField.Data[index])
                flag = false;
            }
          }
          if (!flag)
          {
            this.MyAsyncFunctions.Logger.WriteLoggerData(EventLogger.LoggerEvent.ComEchoError, ref byteField);
            return false;
          }
          this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComEchoOk);
        }
      }
      catch (Exception ex)
      {
        this.WorkException(ex, DataBlock, this.MySerialPort.WriteTimeout);
        return false;
      }
      return true;
    }

    public override void PureTransmit(byte[] byteList)
    {
      this.MySerialPort.Write(byteList, 0, byteList.Length);
    }

    public override bool ReceiveString(out string DataString)
    {
      DataString = (string) null;
      if (!this.MySerialPort.IsOpen)
        return false;
      try
      {
        DataString = this.MySerialPort.ReadExisting();
      }
      catch (Exception ex)
      {
        this.WorkException(ex, (ByteField) null, this.MySerialPort.ReadTimeout);
        return false;
      }
      return true;
    }

    public override bool GetCurrentInputBuffer(out byte[] buffer)
    {
      if (this.MySerialPort != null)
        return this.MySerialPort.ReadExistingBytes(out buffer);
      buffer = (byte[]) null;
      return false;
    }

    public override bool TryReceiveBlock(out byte[] buffer)
    {
      if (this.MySerialPort == null)
        throw new ArgumentNullException("MySerialPort");
      buffer = (byte[]) null;
      if (!this.MySerialPort.IsOpen)
      {
        AsyncSerial.AsyncSerialLogger.Error("The serial port is closed! Try reopen it...");
        this.MySerialPort.Close();
        if (!ZR_ClassLibrary.Util.Wait(1000L, " before reopen the connection.", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
          return false;
        if (!this.MySerialPort.Open())
        {
          if (this.ConnectionLost != null)
            this.ConnectionLost((object) this, (EventArgs) null);
          throw new IOException("Can not open the serial port!");
        }
      }
      int bytesToRead = this.MySerialPort.BytesToRead;
      if (bytesToRead <= 0)
        return false;
      buffer = new byte[bytesToRead];
      for (int index = 0; index < bytesToRead; ++index)
        buffer[index] = (byte) this.MySerialPort.ReadByte();
      return true;
    }

    public override bool TryReceiveBlock(out byte[] buffer, int numberOfBytesToReceive)
    {
      if (this.MySerialPort == null)
        throw new ArgumentNullException("MySerialPort");
      buffer = (byte[]) null;
      if (!this.MySerialPort.IsOpen)
      {
        AsyncSerial.AsyncSerialLogger.Error("The serial port is closed! Try reopen it...");
        this.MySerialPort.Close();
        if (!ZR_ClassLibrary.Util.Wait(1000L, " before reopen the connection.", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
          return false;
        if (!this.MySerialPort.Open())
        {
          if (this.ConnectionLost != null)
            this.ConnectionLost((object) this, (EventArgs) null);
          throw new IOException("Can not open the serial port!");
        }
      }
      if (this.MySerialPort.BytesToRead <= 0 || this.MySerialPort.BytesToRead < numberOfBytesToReceive)
        return false;
      buffer = new byte[numberOfBytesToReceive];
      for (int index = 0; index < numberOfBytesToReceive; ++index)
        buffer[index] = (byte) this.MySerialPort.ReadByte();
      return true;
    }

    public override bool ReceiveBlock(ref ByteField DataBlock)
    {
      if (!this.MySerialPort.IsOpen)
        return false;
      try
      {
        DataBlock.Count = this.Read(DataBlock.Data, 0, DataBlock.Data.Length);
      }
      catch (Exception ex)
      {
        this.WorkException(ex, DataBlock, this.MySerialPort.ReadTimeout);
        return false;
      }
      return DataBlock.Count != 0;
    }

    private bool WorkException(Exception Ex, ByteField inputBuffer, int actualTimeout)
    {
      switch (Ex)
      {
        case TimeoutException _:
          this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComReceiveTimeout);
          if (inputBuffer != null && inputBuffer.Count > 0 && inputBuffer.Data != null && inputBuffer.Data.Length != 0 && inputBuffer.Data.Length >= inputBuffer.Count)
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.TimeoutReceiveIncomplete, "Timeout " + actualTimeout.ToString() + " ms Input buffer: " + ZR_ClassLibrary.Util.ByteArrayToHexString(inputBuffer.Data, 0, inputBuffer.Count));
          else
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Timeout, Ex.Message);
          AsyncSerial.AsyncSerialLogger.Error(Ex.Message);
          return true;
        case FramingErrorException _:
          this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComIOException);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FramingError, Ex.Message);
          this.Close();
          this.Open();
          return false;
        default:
          AsyncSerial.AsyncSerialLogger.Error(Ex, Ex.Message + " " + Ex.StackTrace);
          if (Ex is IOException)
          {
            if (Ex.Message == "Framing error")
            {
              this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComIOException);
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FramingError, Ex.Message);
            }
            else
            {
              this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComIOException);
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, Ex.Message);
            }
          }
          else
          {
            this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComUnknownError);
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, Ex.Message);
          }
          this.Close();
          this.Open();
          return false;
      }
    }

    public override bool ReceiveBlock(ref ByteField DataBlock, int NumberOfBytes, bool first)
    {
      if (NumberOfBytes < 0 || !this.MySerialPort.IsOpen)
        return false;
      DataBlock.Count = 0;
      DateTime dateTimeNow1 = SystemValues.DateTimeNow;
      DateTime EndTime;
      int ActualTimeout;
      this.GetReceiveBlockTiming(NumberOfBytes, first, out EndTime, out ActualTimeout);
      int count = NumberOfBytes;
      DateTime dateTimeNow2;
      while (true)
      {
        try
        {
          if (this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoConnect)
          {
            double totalMilliseconds = EndTime.Subtract(SystemValues.DateTimeNow).TotalMilliseconds;
            int num = 0;
            if (totalMilliseconds > 0.0 && totalMilliseconds < (double) int.MaxValue)
              num = Convert.ToInt32(totalMilliseconds);
            if (num > 100)
              num = 100;
            this.MySerialPort.ReadTimeout = num;
          }
          else
            this.MySerialPort.ReadTimeout = ActualTimeout;
          int num1 = this.MyAsyncFunctions.MBusFrameTestWindowOn ? this.MySerialPort.ReadFromTestWindow(DataBlock.Data, DataBlock.Count, count) : this.Read(DataBlock.Data, DataBlock.Count, count);
          count -= num1;
          DataBlock.Count += num1;
          if (count == 0)
          {
            this.MyAsyncFunctions.Logger.WriteLoggerData(EventLogger.LoggerEvent.ComReceiveData, ref DataBlock);
            this.MyAsyncFunctions.ResetEarliestTransmitTime();
            this.MyAsyncFunctions.TriggerWakeup();
            return true;
          }
          Thread.Sleep(50);
        }
        catch (Exception ex)
        {
          string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
          AsyncSerial.AsyncSerialLogger.Error(ex, message);
          this.MyAsyncFunctions.ResetEarliestTransmitTime();
          if (!(ex is TimeoutException))
          {
            this.WorkException(ex, DataBlock, this.MySerialPort.ReadTimeout);
            Application.DoEvents();
            dateTimeNow2 = SystemValues.DateTimeNow;
            return false;
          }
        }
        if (!(SystemValues.DateTimeNow >= EndTime))
          this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComReceiverPoll);
        else
          break;
      }
      dateTimeNow2 = SystemValues.DateTimeNow;
      if (DataBlock.Count > 0)
      {
        this.MyAsyncFunctions.Logger.WriteLoggerData(EventLogger.LoggerEvent.ComReceiveData, ref DataBlock);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.TimeoutReceiveIncomplete, "Timeout " + ActualTimeout.ToString() + " ms Input buffer: " + ZR_ClassLibrary.Util.ByteArrayToHexString(DataBlock.Data, 0, DataBlock.Count));
      }
      else
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Timeout, Ot.Gtm(Tg.CommunicationLogic, "Timeout", "Timeout") + " " + ActualTimeout.ToString() + " ms");
      this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComReceiveTimeout);
      return false;
    }

    public override bool ReceiveLine(out string ReceivedData)
    {
      char[] EndCharacters = new char[2]{ '\r', '\n' };
      return this.ReceiveLine(out ReceivedData, EndCharacters, false);
    }

    public override bool ReceiveCRLF_Line(out string ReceivedData)
    {
      char[] EndCharacters = new char[2]{ '\r', '\n' };
      return this.ReceiveLine(out ReceivedData, EndCharacters, true);
    }

    public override bool ReceiveLine(
      out string ReceivedData,
      char[] EndCharacters,
      bool GetEmpty_CRLF_Line)
    {
      ReceivedData = "";
      if (!this.MySerialPort.IsOpen)
        return false;
      int num1 = 0;
      this.MyAsyncFunctions.LineBuffer.Length = 0;
      this.MySerialPort.ReadTimeout = 0;
      long timeTicks = this.MyAsyncFunctions.Logger.GetTimeTicks();
      while (true)
      {
        try
        {
          if (this.MySerialPort.BytesToRead > 0)
          {
            int num2 = this.MySerialPort.ReadChar();
            this.MyAsyncFunctions.LineBuffer.Append((char) num2);
            ++num1;
            if (GetEmpty_CRLF_Line && this.MyAsyncFunctions.LineBuffer[0] == '\r')
            {
              if (this.MyAsyncFunctions.LineBuffer.Length != 1)
              {
                if (this.MyAsyncFunctions.LineBuffer[1] == '\n')
                  return true;
                this.MyAsyncFunctions.LineBuffer.Remove(0, 1);
              }
              else
                continue;
            }
            foreach (int endCharacter in EndCharacters)
            {
              if (endCharacter == num2)
              {
                if (this.MyAsyncFunctions.LineBuffer.Length == 1)
                {
                  this.MyAsyncFunctions.LineBuffer.Length = 0;
                }
                else
                {
                  --this.MyAsyncFunctions.LineBuffer.Length;
                  ReceivedData = this.MyAsyncFunctions.LineBuffer.ToString();
                  return true;
                }
              }
            }
          }
          else
          {
            int num3 = (int) ((double) (this.MyAsyncFunctions.RecTime_BeforFirstByte + this.MyAsyncFunctions.AnswerOffsetTime) + (double) num1 * (this.MyAsyncFunctions.ByteTime + this.MyAsyncFunctions.RecTime_OffsetPerByte));
            if (this.MyAsyncFunctions.Logger.GetTimeDifferenc(this.MyAsyncFunctions.Logger.GetTimeTicks() - timeTicks) < (long) num3)
            {
              if (!ZR_ClassLibrary.Util.Wait(30L, nameof (ReceiveLine), (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
                return false;
              this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComReceiverPoll);
            }
            else
              break;
          }
        }
        catch (Exception ex)
        {
          this.WorkException(ex, (ByteField) null, this.MySerialPort.ReadTimeout);
          return false;
        }
      }
      this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComReceiveTimeout);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Timeout, "ComReceiveTimeout");
      ReceivedData = this.MyAsyncFunctions.LineBuffer.ToString();
      return false;
    }

    public override bool ReceiveBlockToChar(ref ByteField DataBlock, byte EndChar)
    {
      if (!this.MySerialPort.IsOpen)
        return false;
      ByteField data = new ByteField(DataBlock.Data.Length);
      int length = DataBlock.Data.Length;
      int WaitMilliSecounds = this.MyAsyncFunctions.RecTime_BeforFirstByte + this.MyAsyncFunctions.AnswerOffsetTime + (int) (this.MyAsyncFunctions.ByteTime * (double) DataBlock.Data.Length);
      long endTicks = this.MyAsyncFunctions.Logger.GetEndTicks((long) WaitMilliSecounds);
      do
      {
        try
        {
          this.MySerialPort.ReadTimeout = WaitMilliSecounds;
          this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComReceiverPoll);
          int IntToByte = this.MySerialPort.ReadByte();
          DataBlock.Add(IntToByte);
          data.Add(IntToByte);
          --length;
          if (IntToByte == (int) EndChar)
          {
            this.MyAsyncFunctions.Logger.WriteLoggerData(EventLogger.LoggerEvent.ComReceiveData, ref data);
            return true;
          }
        }
        catch (TimeoutException ex)
        {
          if (data.Count > 0)
          {
            this.MyAsyncFunctions.Logger.WriteLoggerData(EventLogger.LoggerEvent.ComReceiveData, ref data);
            data.Count = 0;
          }
          else
            this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComReceiverPoll);
        }
        catch (Exception ex)
        {
          this.WorkException(ex, DataBlock, this.MySerialPort.ReadTimeout);
          return false;
        }
      }
      while (this.MyAsyncFunctions.Logger.GetTimeTicks() <= endTicks);
      return false;
    }

    public override bool TransmitControlCommand(string strSendData) => false;

    public override bool ReceiveControlBlock(
      out string ReceivedData,
      string startTag,
      string endTag)
    {
      ReceivedData = "";
      return false;
    }

    private int Read(byte[] buffer, int offset, int count)
    {
      return this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoHead && !this.MinoHeadIsTransparent ? this.MinoHeadReadData(buffer, offset, count) : this.MySerialPort.Read(buffer, offset, count);
    }

    internal void Write(byte[] buffer, int offset, int count)
    {
      DateTime dateTime = SystemValues.DateTimeNow.AddMilliseconds(!(this.MyAsyncFunctions.Parity != "no") ? (double) count * 1000.0 / ((double) this.MyAsyncFunctions.Baudrate / 10.0) : (double) count * 1000.0 / ((double) this.MyAsyncFunctions.Baudrate / 11.0));
      if (this.MyAsyncFunctions.Transceiver == TransceiverDevice.MinoHead && !this.MinoHeadIsTransparent)
      {
        byte[] outData = new byte[buffer.Length - 2];
        for (int index = 0; index < buffer.Length - 2; ++index)
          outData[index] = buffer[index + 2];
        this.MinoHeadSendData(buffer[1], outData, count - 2);
        if (!ZR_ClassLibrary.Util.Wait((long) (buffer.Length * 2), "after send request to MinoHead", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
          return;
      }
      else
        this.MySerialPort.Write(buffer, offset, count);
      double totalMilliseconds = dateTime.Subtract(SystemValues.DateTimeNow).TotalMilliseconds;
      if (totalMilliseconds < 0.0 || totalMilliseconds > (double) int.MaxValue || ZR_ClassLibrary.Util.Wait((long) (int) totalMilliseconds, "Wait transmit finished", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
        ;
    }

    internal short getHeadVersion()
    {
      byte[] buffer = new byte[50];
      this.MinoHeadSendData((byte) 1, (byte[]) null, 0);
      this.MinoHeadReadData(buffer, 0, 2, true);
      return (short) ((int) buffer[0] | (int) buffer[1] << 8);
    }

    internal short getEchoIrDa()
    {
      byte[] buffer = new byte[50];
      this.MinoHeadSendData((byte) 162, (byte[]) null, 0);
      this.MinoHeadReadData(buffer, 0, 1, true);
      return (short) buffer[0];
    }

    internal short getRFModul()
    {
      byte[] buffer = new byte[50];
      this.MinoHeadSendData((byte) 163, (byte[]) null, 0);
      this.MinoHeadReadData(buffer, 0, 1, true);
      return (short) buffer[0];
    }

    internal void SendMinoHeadWakeup()
    {
      if (!this.MySerialPort.IsOpen)
        throw new Exception("Failed send MinoHead wake up! The com port is closed.");
      this.MySerialPort.SetDTR(true);
      if (!ZR_ClassLibrary.Util.Wait(50L, "after set DTR=true", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
        return;
      this.MySerialPort.SetDTR(false);
      if (!ZR_ClassLibrary.Util.Wait(1000L, "after set DTR=false", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
        return;
      this.MinoHeadIsTransparent = false;
      AsyncSerial.AsyncSerialLogger.Debug("Wake up MinoHead. Send wake up sequence...");
      if (!ZR_ClassLibrary.Util.Wait(200L, "before send wake up to MinoHead", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
        return;
      this.Write(new byte[4]
      {
        (byte) 0,
        (byte) 4,
        (byte) 64,
        (byte) 0
      }, 0, 4);
      if (ZR_ClassLibrary.Util.Wait(200L, "after send wake up to MinoHead", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
        ;
    }

    protected void setHeadTransparent()
    {
      if (!ZR_ClassLibrary.Util.Wait(600L, "befor set transparent mode on MinoHead", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
        return;
      AsyncSerial.AsyncSerialLogger.Debug("Send command to MinoHead => TLP2Commands.lp2Transparent");
      this.MinoHeadSendData((byte) 150, (byte[]) null, 0);
      if (!ZR_ClassLibrary.Util.Wait(800L, "after set transparent mode on MinoHead", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
        return;
      this.MinoHeadIsTransparent = true;
    }

    protected void setHeadTransparentV3()
    {
      AsyncSerial.AsyncSerialLogger.Debug("setHeadTransparentV3()");
      this.MinoHeadSendData((byte) 164, (byte[]) null, 0);
      if (!ZR_ClassLibrary.Util.Wait(800L, "after set transparent mode on MinoHead", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger))
        return;
      this.MinoHeadIsTransparent = true;
    }

    private bool MinoHeadSendData(byte command, byte[] outData, int outCount)
    {
      if (!this.MySerialPort.IsOpen)
        return false;
      if (AsyncSerial.AsyncSerialLogger.IsTraceEnabled)
        AsyncSerial.AsyncSerialLogger.Trace("MinoHeadSendData() Command: 0x" + command.ToString("X"));
      this.ClearCom();
      byte[] numArray = new byte[outCount + 6];
      numArray[0] = (byte) 77;
      numArray[1] = (byte) 1;
      numArray[2] = command;
      numArray[3] = (byte) ((outCount & 65280) >> 8);
      numArray[4] = (byte) outCount;
      for (int index = 0; index < outCount; ++index)
        numArray[5 + index] = outData[index];
      numArray[5 + outCount] = this.cs(numArray, 5 + outCount);
      if (AsyncSerial.AsyncSerialLogger.IsTraceEnabled)
        AsyncSerial.AsyncSerialLogger.Trace("Send to MINO HEAD: {0}", ZR_ClassLibrary.Util.ByteArrayToHexString(numArray));
      this.MySerialPort.Write(numArray, 0, 5 + outCount + 1);
      return true;
    }

    private int MinoHeadReadData(byte[] buffer, int offset, int count)
    {
      return this.MinoHeadReadData(buffer, offset, count, false);
    }

    private int MinoHeadReadData(byte[] buffer, int offset, int count, bool includeStartbyte)
    {
      AsyncSerial.AsyncSerialLogger.Debug("MinoHeadReadData ({0} bytes expected).", count);
      int num1 = 0;
      int num2 = 0;
      byte[] numArray = new byte[1024];
      if (count > this.MinoHeadReceiverQueue.Count)
      {
        DateTime dateTime = SystemValues.DateTimeNow.AddMilliseconds((double) this.MySerialPort.ReadTimeout);
        do
        {
          if (this.MySerialPort.BytesToRead <= 1024)
          {
            if (this.MySerialPort.BytesToRead < 6)
            {
              if (dateTime < SystemValues.DateTimeNow)
                goto label_5;
            }
            else
              goto label_9;
          }
          else
            goto label_2;
        }
        while (ZR_ClassLibrary.Util.Wait(200L, "while read data from MinoHead", (ICancelable) this.MyAsyncFunctions, AsyncSerial.AsyncSerialLogger));
        goto label_7;
label_2:
        return 0;
label_5:
        AsyncSerial.AsyncSerialLogger.Error("MinoHeadReadData: No answer (only {0} bytes available) throw new TimeoutException()", this.MySerialPort.BytesToRead);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "MinoHead: No answer!");
        throw new TimeoutException();
label_7:
        return 0;
label_9:
        AsyncSerial.AsyncSerialLogger.Debug("MinoHeadReadData: {0} bytes available.", this.MySerialPort.BytesToRead);
        for (int index = 0; index < 5; ++index)
          numArray[index] = (byte) this.MySerialPort.ReadByte();
        num1 = (int) numArray[3] << 8 | (int) numArray[4];
        AsyncSerial.AsyncSerialLogger.Debug("MinoHeadReadData: Expecting {0} bytes.", num1);
        if (num1 == 0)
        {
          int num3 = this.MySerialPort.ReadByte();
          AsyncSerial.AsyncSerialLogger.Error<string, string>("Received empty packet from MinoHead! Buffer: {0}{1}", ZR_ClassLibrary.Util.ByteArrayToHexString(numArray, 0, 4), num3.ToString("X2"));
          return this.MinoHeadReadData(buffer, offset, count, includeStartbyte);
        }
        int count1 = num1 + 1;
        int offset1 = 5;
        int num4;
        for (; count1 > 0 && !this.MyAsyncFunctions.BreakRequest; count1 -= num4)
        {
          num4 = this.MySerialPort.Read(numArray, offset1, count1);
          offset1 += num4;
        }
        num2 = num1 + 6;
        if ((int) this.cs(numArray, num1 + 5) != (int) numArray[5 + num1])
        {
          string str = "MinoHead: Answer checksum error!";
          AsyncSerial.AsyncSerialLogger.Error(str);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, str);
          throw new IOException();
        }
        if (includeStartbyte)
        {
          for (int index = 0; index < num1; ++index)
            this.MinoHeadReceiverQueue.Enqueue(numArray[5 + index]);
        }
        else
        {
          for (int index = 1; index < num1; ++index)
            this.MinoHeadReceiverQueue.Enqueue(numArray[5 + index]);
        }
      }
      for (int index = 0; index < count; ++index)
      {
        if (this.MinoHeadReceiverQueue.Count > 0)
        {
          buffer[index + offset] = this.MinoHeadReceiverQueue.Dequeue();
        }
        else
        {
          if (num2 == 7 && num1 == 1 && numArray[5] == (byte) 0)
            return 0;
          string str = "MinoHead: Illegal answer size! throw new IOException()";
          AsyncSerial.AsyncSerialLogger.Error(str);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, str);
          throw new IOException();
        }
      }
      AsyncSerial.AsyncSerialLogger.Debug("MinoHeadReadData: Read {0} bytes", num2);
      return count;
    }

    private byte cs(byte[] buf, int count)
    {
      uint num = 0;
      for (int index = 0; index < count; ++index)
        num += (uint) buf[index];
      return (byte) (num & (uint) byte.MaxValue);
    }

    public override object GetChannel() => (object) this.MySerialPort.GetPort();
  }
}
