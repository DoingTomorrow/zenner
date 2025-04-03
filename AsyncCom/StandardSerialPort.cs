// Decompiled with JetBrains decompiler
// Type: AsyncCom.StandardSerialPort
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using GmmDbLib;
using NLog;
using System;
using System.IO;
using System.IO.Ports;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace AsyncCom
{
  internal class StandardSerialPort : Port
  {
    protected SerialPort MySerialPort;
    internal AsyncFunctions MyFunctions;
    private static Logger logger = LogManager.GetLogger(nameof (StandardSerialPort));

    internal StandardSerialPort(int baudRate, Parity parity, AsyncFunctions MyFunctionsIn)
    {
      this.MyFunctions = MyFunctionsIn;
      this.InitializeSerialPort(baudRate, parity);
    }

    private void InitializeSerialPort(int baudRate, Parity parity)
    {
      this.MySerialPort = new SerialPort(this.MyFunctions.ComPort, baudRate, parity, 8, StopBits.One);
      this.MySerialPort.DtrEnable = true;
      this.MySerialPort.DiscardNull = false;
      this.MySerialPort.ParityReplace = (byte) 0;
      this.MySerialPort.WriteTimeout = 5000;
      this.MySerialPort.ReadTimeout = 5000;
    }

    internal override bool Open()
    {
      if (this.IsOpen)
      {
        StandardSerialPort.logger.Trace("Com is already open.");
        return true;
      }
      StandardSerialPort.logger.Trace("Open() called.");
      try
      {
        if (this.MyFunctions.HardwareHandshake)
          this.MySerialPort.Handshake = Handshake.RequestToSend;
        else
          this.MySerialPort.RtsEnable = true;
        this.MySerialPort.PortName = this.MyFunctions.ComPort;
        this.MySerialPort.Open();
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.ComOpenError, Ot.Gtm(Tg.CommunicationLogic, "ComOpenError", "Failed to open") + " " + this.MySerialPort.PortName);
        StandardSerialPort.logger.Error(ex.Message);
        return false;
      }
      this.FramingError = false;
      this.MySerialPort.ErrorReceived += new SerialErrorReceivedEventHandler(((Port) this).ErrorReceived);
      return true;
    }

    internal override void Close()
    {
      try
      {
        StandardSerialPort.logger.Trace("Close()");
        this.MySerialPort.ErrorReceived -= new SerialErrorReceivedEventHandler(((Port) this).ErrorReceived);
        this.MySerialPort.Close();
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, Ot.Gtm(Tg.CommunicationLogic, "ComCloseError", "Failed to close") + " " + this.MySerialPort.PortName);
        StandardSerialPort.logger.Error(ex.Message);
      }
      finally
      {
        this.MyFunctions.ComIsOpen = false;
      }
    }

    internal override void SetRTS(bool state)
    {
      StandardSerialPort.logger.Trace("SetRTS = {0}", state);
      this.MySerialPort.RtsEnable = state;
    }

    internal override void SetDTR(bool state)
    {
      StandardSerialPort.logger.Trace("SetDTR = {0}", state);
      this.MySerialPort.DtrEnable = state;
    }

    protected override void ErrorReceived(object o, SerialErrorReceivedEventArgs ErrorType)
    {
      StandardSerialPort.logger.Error<SerialError>(ErrorType.EventType);
      this.FramingError = true;
    }

    internal override void Write(string text)
    {
      if (!this.IsOpen)
        StandardSerialPort.logger.Error("Error: The serial port is closed!");
      else
        this.MySerialPort.Write(text);
    }

    internal override void Write(byte[] buffer, int offset, int count)
    {
      if (!this.IsOpen)
      {
        StandardSerialPort.logger.Error("Error: The serial port is closed!");
      }
      else
      {
        try
        {
          this.MySerialPort.Write(buffer, offset, count);
        }
        catch (TimeoutException ex)
        {
          StandardSerialPort.logger.Error("Timeout while write the buffer!");
          throw ex;
        }
        catch (IOException ex)
        {
          string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
          StandardSerialPort.logger.Error((Exception) ex, message);
          throw ex;
        }
        catch (Exception ex)
        {
          string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
          StandardSerialPort.logger.Error(ex, message);
        }
      }
    }

    internal override int ReadByte()
    {
      if (!this.IsOpen)
        throw new Exception("Failed to read! Com port is closed.");
      int num = 0;
      Exception exception = (Exception) null;
      try
      {
        num = this.MySerialPort.ReadByte();
      }
      catch (Exception ex)
      {
        string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        StandardSerialPort.logger.Error(ex, message);
        exception = ex;
      }
      if (this.FramingError && !this.IgnoreFramingError)
      {
        this.FramingError = false;
        StandardSerialPort.logger.Error("Framing error");
        throw new FramingErrorException(Ot.Gtm(Tg.CommunicationLogic, "MiConFramingError", "MinoConnect framing error"));
      }
      if (exception != null)
        throw exception;
      return num;
    }

    internal override int ReadChar() => this.ReadByte();

    internal override int Read(byte[] buffer, int offset, int count)
    {
      if (!this.IsOpen)
        throw new Exception("Failed to read! Com port is closed.");
      int num = 0;
      Exception exception = (Exception) null;
      try
      {
        num = this.MySerialPort.Read(buffer, offset, count);
      }
      catch (TimeoutException ex)
      {
        string message = string.Format("Timeout while read the buffer! ReadTimeout: {0}, Expected bytes: {1}, Error: {2}", (object) this.MySerialPort.ReadTimeout, (object) count, (object) ex.Message);
        StandardSerialPort.logger.Warn((Exception) ex, message);
        throw ex;
      }
      catch (Exception ex)
      {
        StandardSerialPort.logger.Warn(ex, ex.Message);
        exception = ex;
      }
      if (this.FramingError && !this.IgnoreFramingError)
      {
        this.FramingError = false;
        StandardSerialPort.logger.Error("Framing error");
        throw new FramingErrorException(Ot.Gtm(Tg.CommunicationLogic, "MiConFramingError", "MinoConnect framing error"));
      }
      if (exception != null)
        throw exception;
      return num;
    }

    internal override string ReadExisting()
    {
      if (!this.IsOpen)
        return string.Empty;
      string str = string.Empty;
      Exception exception = (Exception) null;
      try
      {
        str = this.MySerialPort.ReadExisting();
      }
      catch (Exception ex)
      {
        StandardSerialPort.logger.Error(ex, ex.Message);
        exception = ex;
      }
      if (this.FramingError && !this.IgnoreFramingError)
      {
        this.FramingError = false;
        StandardSerialPort.logger.Error("Framing error");
        throw new FramingErrorException(Ot.Gtm(Tg.CommunicationLogic, "MiConFramingError", "MinoConnect framing error"));
      }
      if (exception != null)
        throw exception;
      return str;
    }

    internal override void DiscardInBuffer()
    {
      if (this.IsOpen)
      {
        StandardSerialPort.logger.Trace("Clear input buffer");
        this.MySerialPort.DiscardInBuffer();
      }
      this.FramingError = false;
    }

    internal override void DiscardOutBuffer()
    {
      if (!this.IsOpen)
        return;
      StandardSerialPort.logger.Trace("Clear output buffer");
      this.MySerialPort.DiscardOutBuffer();
    }

    internal override SerialPort GetPort() => this.MySerialPort;

    internal override bool IsOpen
    {
      get
      {
        bool isOpen;
        try
        {
          isOpen = this.MySerialPort != null && this.MySerialPort.IsOpen && this.MySerialPort.BaseStream != null && this.MySerialPort.BaseStream.CanRead && this.MySerialPort.BaseStream.CanWrite;
          if (!isOpen)
            return false;
        }
        catch (IOException ex)
        {
          this.HandleExceptionByNotAvailablePort(ex);
          return false;
        }
        return isOpen;
      }
    }

    internal override int BytesToRead
    {
      get
      {
        try
        {
          return this.MySerialPort.BytesToRead;
        }
        catch (IOException ex)
        {
          this.HandleExceptionByNotAvailablePort(ex);
          return 0;
        }
      }
    }

    internal override int BytesToWrite
    {
      get
      {
        try
        {
          return this.MySerialPort.BytesToWrite;
        }
        catch (IOException ex)
        {
          this.HandleExceptionByNotAvailablePort(ex);
          return 0;
        }
      }
    }

    internal override int ReadTimeout
    {
      set => this.MySerialPort.ReadTimeout = value;
      get => this.MySerialPort.ReadTimeout;
    }

    internal override int WriteTimeout
    {
      set => this.MySerialPort.WriteTimeout = value;
      get => this.MySerialPort.WriteTimeout;
    }

    internal override int WriteBufferSize
    {
      set => this.MySerialPort.WriteBufferSize = value;
      get => this.MySerialPort.WriteBufferSize;
    }

    internal override int ReadBufferSize
    {
      set => this.MySerialPort.ReadBufferSize = value;
      get => this.MySerialPort.ReadBufferSize;
    }

    internal override bool BreakState
    {
      set => this.MySerialPort.BreakState = value;
      get => this.MySerialPort.BreakState;
    }

    internal override Handshake Handshake
    {
      set => this.MySerialPort.Handshake = value;
      get => this.MySerialPort.Handshake;
    }

    internal override bool DtrEnable
    {
      set => this.MySerialPort.DtrEnable = value;
      get => this.MySerialPort.DtrEnable;
    }

    internal override bool RtsEnable
    {
      set => this.MySerialPort.RtsEnable = value;
      get => this.MySerialPort.RtsEnable;
    }

    internal override bool DiscardNull
    {
      set => this.MySerialPort.DiscardNull = value;
      get => this.MySerialPort.DiscardNull;
    }

    internal override byte ParityReplace
    {
      set => this.MySerialPort.ParityReplace = value;
      get => this.MySerialPort.ParityReplace;
    }

    internal override int ReceivedBytesThreshold
    {
      set => this.MySerialPort.ReceivedBytesThreshold = value;
      get => this.MySerialPort.ReceivedBytesThreshold;
    }

    private void HandleExceptionByNotAvailablePort(IOException exc)
    {
      StandardSerialPort.logger.Fatal("The actual serial port is not more available! Reason: {0}", exc.Message);
      this.MySerialPort.ErrorReceived -= new SerialErrorReceivedEventHandler(((Port) this).ErrorReceived);
      this.MyFunctions.ComIsOpen = false;
      int baudRate = this.MySerialPort.BaudRate;
      Parity parity = this.MySerialPort.Parity;
      StandardSerialPort.logger.Trace("Try MySerialPort.BaseStream.Close();");
      try
      {
        this.MySerialPort.BaseStream.Close();
      }
      catch (Exception ex)
      {
        StandardSerialPort.logger.Fatal("Failed MySerialPort.BaseStream.Close(); Reason: {0}", ex.Message);
      }
      StandardSerialPort.logger.Trace("Try MySerialPort.Close();");
      try
      {
        this.MySerialPort.Close();
      }
      catch (Exception ex)
      {
        StandardSerialPort.logger.Fatal("Failed MySerialPort.Close(); Reason: {0}", ex.Message);
      }
      StandardSerialPort.logger.Trace("Try MySerialPort.Dispose();");
      try
      {
        this.MySerialPort.Dispose();
      }
      catch (Exception ex)
      {
        StandardSerialPort.logger.Fatal("Failed MySerialPort.Dispose(); Reason: {0}", ex.Message);
      }
      this.MySerialPort = (SerialPort) null;
      StandardSerialPort.logger.Trace("Try GC.WaitForPendingFinalizers();");
      GC.WaitForPendingFinalizers();
      this.InitializeSerialPort(baudRate, parity);
    }
  }
}
