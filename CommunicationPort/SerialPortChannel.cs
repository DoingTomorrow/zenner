// Decompiled with JetBrains decompiler
// Type: CommunicationPort.SerialPortChannel
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using System;
using System.IO.Ports;

#nullable disable
namespace CommunicationPort
{
  public sealed class SerialPortChannel : SerialPort, IChannel, IDisposable
  {
    public SerialPortChannel(string portName)
    {
      this.PortName = portName;
      this.BaudRate = 115200;
      this.Parity = Parity.None;
      this.Handshake = Handshake.RequestToSend;
      this.ReadTimeout = 0;
      this.WriteTimeout = 5000;
    }

    public void WriteBaudrateCarrier(int numberOfBytes)
    {
      byte[] buffer = new byte[numberOfBytes];
      for (int index = 0; index < buffer.Length; ++index)
        buffer[index] = (byte) 85;
      this.Write(buffer, 0, buffer.Length);
    }

    string IChannel.get_PortName() => this.PortName;

    void IChannel.set_PortName(string value) => this.PortName = value;

    bool IChannel.get_IsOpen() => this.IsOpen;

    int IChannel.get_BytesToRead() => this.BytesToRead;

    void IChannel.Open() => this.Open();

    void IChannel.Close() => this.Close();

    void IChannel.DiscardInBuffer() => this.DiscardInBuffer();

    void IChannel.DiscardOutBuffer() => this.DiscardOutBuffer();

    int IChannel.ReadByte() => this.ReadByte();

    int IChannel.Read(byte[] buffer, int offset, int count) => this.Read(buffer, offset, count);

    void IChannel.Write(byte[] buffer, int offset, int count) => this.Write(buffer, offset, count);

    void IChannel.Write(string text) => this.Write(text);
  }
}
