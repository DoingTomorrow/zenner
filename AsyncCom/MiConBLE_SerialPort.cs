// Decompiled with JetBrains decompiler
// Type: AsyncCom.MiConBLE_SerialPort
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using CommunicationPort;
using GmmDbLib;
using NLog;
using System;
using System.IO.Ports;
using ZR_ClassLibrary;

#nullable disable
namespace AsyncCom
{
  internal class MiConBLE_SerialPort : Port
  {
    private static Logger logger = LogManager.GetLogger(nameof (MiConBLE_SerialPort));
    internal BluetoothChannel_LE BLE_Channel;
    private int readTimeout;
    private int writeTimeout;

    internal override int ReadTimeout
    {
      get => this.readTimeout;
      set => this.readTimeout = value;
    }

    internal override int WriteTimeout
    {
      get => this.writeTimeout;
      set => this.writeTimeout = value;
    }

    internal override int BytesToRead => this.BLE_Channel.BytesToRead;

    internal MiConBLE_SerialPort(string BLE_Port)
    {
      this.BLE_Channel = new BluetoothChannel_LE(BLE_Port);
    }

    internal override bool Open()
    {
      try
      {
        this.BLE_Channel.Open();
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.ComOpenError, Ot.Gtm(Tg.CommunicationLogic, "ComOpenError", "Failed to open") + " " + this.BLE_Channel.PortName);
        MiConBLE_SerialPort.logger.Error(ex.Message);
        return false;
      }
      this.FramingError = false;
      return true;
    }

    internal override bool IsOpen => this.BLE_Channel.IsOpen;

    internal override void Close()
    {
      try
      {
        this.BLE_Channel.Close();
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, Ot.Gtm(Tg.CommunicationLogic, "ComCloseError", "Failed to close") + " " + this.BLE_Channel.PortName);
        MiConBLE_SerialPort.logger.Error(ex.Message);
      }
    }

    internal override int ReadByte()
    {
      int num = 0;
      Exception exception = (Exception) null;
      try
      {
        num = this.BLE_Channel.ReadByte();
      }
      catch (Exception ex)
      {
        string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MiConBLE_SerialPort.logger.Error(ex, message);
        exception = ex;
      }
      if (exception != null)
        throw exception;
      return num;
    }

    internal override int ReadChar() => this.ReadByte();

    internal override int Read(byte[] buffer, int offset, int count)
    {
      int num = 0;
      Exception exception = (Exception) null;
      try
      {
        num = this.BLE_Channel.Read(buffer, offset, count);
      }
      catch (TimeoutException ex)
      {
        MiConBLE_SerialPort.logger.Warn((Exception) ex, "Timeout while read the buffer!");
        throw ex;
      }
      catch (Exception ex)
      {
        MiConBLE_SerialPort.logger.Warn(ex, ex.Message);
        exception = ex;
      }
      if (exception != null)
        throw exception;
      return num;
    }

    internal override void Write(string text) => this.BLE_Channel.Write(text);

    internal override void Write(byte[] buffer, int offset, int count)
    {
      try
      {
        this.BLE_Channel.Write(buffer, offset, count);
      }
      catch (TimeoutException ex)
      {
        MiConBLE_SerialPort.logger.Error("Timeout while write the buffer!");
        throw ex;
      }
      catch (Exception ex)
      {
        string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MiConBLE_SerialPort.logger.Error(ex, message);
      }
    }

    internal override void DiscardInBuffer()
    {
      if (this.IsOpen)
      {
        MiConBLE_SerialPort.logger.Trace("Clear input buffer");
        this.BLE_Channel.DiscardInBuffer();
      }
      this.FramingError = false;
    }

    internal override void DiscardOutBuffer()
    {
      if (!this.IsOpen)
        return;
      MiConBLE_SerialPort.logger.Trace("Clear output buffer");
      this.BLE_Channel.DiscardOutBuffer();
    }

    internal override SerialPort GetPort() => throw new Exception("Port not available");
  }
}
