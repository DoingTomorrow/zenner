// Decompiled with JetBrains decompiler
// Type: AsyncCom.Port
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using System;
using System.IO.Ports;

#nullable disable
namespace AsyncCom
{
  internal abstract class Port
  {
    internal bool FramingError;
    internal bool IgnoreFramingError;
    private MBusFrameTestWindow FrameTestWindow;

    internal abstract SerialPort GetPort();

    public virtual event EventHandler ConnectionLost;

    public virtual event EventHandler BatterieLow;

    internal abstract bool Open();

    internal abstract void Close();

    internal virtual int ReadFromTestWindow(byte[] buffer, int offset, int count)
    {
      if (this.FrameTestWindow == null)
        this.FrameTestWindow = new MBusFrameTestWindow();
      return this.FrameTestWindow.Read(buffer, offset, count);
    }

    internal virtual void SetRTS(bool state) => throw new NotImplementedException();

    internal virtual void SetDTR(bool state) => throw new NotImplementedException();

    protected virtual void ErrorReceived(object o, SerialErrorReceivedEventArgs ErrorType)
    {
      throw new NotImplementedException();
    }

    internal abstract void Write(string text);

    internal abstract void Write(byte[] buffer, int offset, int count);

    internal abstract int ReadByte();

    internal abstract int ReadChar();

    internal abstract int Read(byte[] buffer, int offset, int count);

    internal virtual string ReadExisting() => throw new NotImplementedException();

    internal virtual bool ReadExistingBytes(out byte[] buffer)
    {
      throw new NotImplementedException();
    }

    internal virtual void DiscardInBuffer() => throw new NotImplementedException();

    internal virtual void DiscardOutBuffer() => throw new NotImplementedException();

    internal virtual bool IsOpen => throw new NotImplementedException();

    internal virtual int BytesToRead => throw new NotImplementedException();

    internal virtual int BytesToWrite => throw new NotImplementedException();

    internal virtual int ReadTimeout
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    internal virtual int WriteTimeout
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    internal virtual int WriteBufferSize
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    internal virtual int ReadBufferSize
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    internal virtual bool BreakState
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    internal virtual Handshake Handshake
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    internal virtual bool DtrEnable
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    internal virtual bool RtsEnable
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    internal virtual bool DiscardNull
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    internal virtual byte ParityReplace
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    internal virtual int ReceivedBytesThreshold
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    internal virtual bool ChangeDriverSettings() => false;
  }
}
