// Decompiled with JetBrains decompiler
// Type: CommunicationPort.Functions.CommunicationByComPort
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using NLog;
using System;
using System.IO.Ports;
using ZR_ClassLibrary;

#nullable disable
namespace CommunicationPort.Functions
{
  internal class CommunicationByComPort : CommunicationBase, IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (CommunicationByComPort));
    private SerialPort serialPort;

    public override int BytesToRead => this.serialPort.BytesToRead;

    internal CommunicationByComPort(ICommunicationFunctions comPortFunctions)
      : base(comPortFunctions)
    {
    }

    public override void Open()
    {
      if (this.serialPort == null)
      {
        this.serialPort = new SerialPort();
        this.serialPort.PortName = this.configList.Port;
        this.serialPort.BaudRate = this.configList.Baudrate;
        this.serialPort.Parity = this.Parity;
        this.serialPort.WriteTimeout = 5000;
        this.serialPort.DtrEnable = true;
        this.serialPort.ParityReplace = (byte) 0;
      }
      if (this.serialPort.IsOpen)
        return;
      this.serialPort.Open();
      this.SetNextTimeAfterOpen();
      CommunicationByComPort.logger.Trace("Port open");
    }

    public override void Close()
    {
      if (!this.IsOpen)
        return;
      this.serialPort.DiscardInBuffer();
      this.serialPort.Close();
    }

    public override bool IsOpen => this.serialPort != null && this.serialPort.IsOpen;

    public override void Write(byte[] data) => this.Write(data, 0, data.Length);

    public override void WriteWithoutDiscardInputBuffer(byte[] data)
    {
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      this.Open();
      this.DiscardInBuffer();
      this.WaitToNextTransmitTime();
      DateTime endTimepoint = this.CalculateEndTimepoint(count);
      this.serialPort.Write(buffer, offset, count);
      this.WaitOf(endTimepoint);
      if (!CommunicationByComPort.logger.IsTraceEnabled)
        return;
      CommunicationByComPort.logger.Trace("Write: " + Util.ByteArrayToHexString(buffer, offset, count));
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      int readHeaderTimeout = this.GetReadHeaderTimeout(count);
      byte[] src = this.ReadData(count, readHeaderTimeout);
      Buffer.BlockCopy((Array) src, 0, (Array) buffer, offset, src.Length);
      return src.Length;
    }

    public override byte[] ReadHeader(int count)
    {
      int readHeaderTimeout = this.GetReadHeaderTimeout(count);
      return this.ReadData(count, readHeaderTimeout);
    }

    public override byte[] ReadEnd(int count)
    {
      int readEndTimeout = this.GetReadEndTimeout(count);
      return this.ReadData(count, readEndTimeout);
    }

    public byte[] ReadData(int count, int timeout_ms)
    {
      this.serialPort.ReadTimeout = timeout_ms;
      byte[] numArray1 = new byte[count];
      int length = 0;
      try
      {
        while (length < count)
          length += this.serialPort.Read(numArray1, length, numArray1.Length - length);
      }
      catch (TimeoutException ex)
      {
        byte[] numArray2 = length > 0 ? new byte[length] : throw new TimeoutException("Timeout " + timeout_ms.ToString() + "ms", (Exception) ex);
        Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, 0, length);
        throw new MissingBytesTimeoutException("Timeout " + timeout_ms.ToString() + "ms. Missed " + (count - length).ToString() + " byte(s)", count, numArray2);
      }
      finally
      {
        this.SetNextTimeAfterRead();
      }
      if (CommunicationByComPort.logger.IsTraceEnabled)
        CommunicationByComPort.logger.Trace("Read: " + Util.ByteArrayToHexString(numArray1));
      return numArray1;
    }

    public override void DiscardCurrentInBuffer()
    {
    }

    public override bool DiscardInBuffer()
    {
      bool flag = false;
      if (this.serialPort == null)
        return flag;
      while (this.serialPort.BytesToRead > 0)
      {
        byte[] buffer = this.ReadHeader(this.serialPort.BytesToRead);
        CommunicationByComPort.logger.Debug("DiscardInBuffer: " + Util.ByteArrayToHexString(buffer));
        flag = true;
      }
      this.serialPort.DiscardInBuffer();
      return flag;
    }

    public override void Dispose()
    {
      if (this.serialPort != null)
        this.serialPort.Dispose();
      this.serialPort = (SerialPort) null;
      base.Dispose();
    }

    protected override void WriteBaudrateCarrier(int carrierTime_ms)
    {
      byte[] buffer = new byte[(int) ((double) carrierTime_ms / this.ByteTime)];
      for (int index = 0; index < buffer.Length; ++index)
        buffer[index] = (byte) 85;
      CommunicationByComPort.logger.Debug("Baudrate carrier " + buffer.Length.ToString() + " bytes.");
      DateTime endTimepoint = this.CalculateEndTimepoint(buffer.Length);
      this.serialPort.Write(buffer, 0, buffer.Length);
      this.WaitOf(endTimepoint);
    }

    public override void SetBreak()
    {
      CommunicationByComPort.logger.Info("Break signal on");
      this.serialPort.BreakState = true;
    }

    public override void ClearBreak()
    {
      CommunicationByComPort.logger.Info("Break signal off");
      this.serialPort.BreakState = false;
    }
  }
}
