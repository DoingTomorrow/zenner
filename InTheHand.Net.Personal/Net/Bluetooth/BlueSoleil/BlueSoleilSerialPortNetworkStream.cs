// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueSoleil.BlueSoleilSerialPortNetworkStream
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.IO;
using System.IO.Ports;
using System.Net.Sockets;
using System.Threading;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueSoleil
{
  internal class BlueSoleilSerialPortNetworkStream : SerialPortNetworkStream, IBluesoleilConnection
  {
    private const string ObjectDisposedException_ObjectName = "Network";
    internal const string WrappingIOExceptionMessage = "IOError on socket.";
    private uint? _hConn;
    private readonly BluesoleilFactory _factory;
    private EventWaitHandle _received = (EventWaitHandle) new AutoResetEvent(false);
    private volatile BlueSoleilSerialPortNetworkStream.State _state;
    private BlueSoleilSerialPortNetworkStream.DlgtRead _readDlgt;
    private BlueSoleilSerialPortNetworkStream.DlgtWrite _writeDlgt;

    internal BlueSoleilSerialPortNetworkStream(
      SerialPort port,
      uint hConn,
      BluesoleilClient cli,
      BluesoleilFactory factory)
      : this((ISerialPortWrapper) new SerialPortWrapper(port), hConn, cli, factory)
    {
    }

    internal BlueSoleilSerialPortNetworkStream(
      ISerialPortWrapper port,
      uint hConn,
      BluesoleilClient cli,
      BluesoleilFactory factory)
      : base(port, (IBluetoothClient) cli)
    {
      this._hConn = new uint?(hConn);
      this._factory = factory;
      this._state = BlueSoleilSerialPortNetworkStream.State.Connected;
      port.DataReceived += new SerialDataReceivedEventHandler(this.port_DataReceived);
    }

    void IBluesoleilConnection.CloseNetworkOrInternal()
    {
      this._state = BlueSoleilSerialPortNetworkStream.State.PeerDidClose;
      this._received.Set();
      this._received.Close();
      this._hConn = new uint?();
    }

    protected override void Dispose(bool disposing)
    {
      this._state = BlueSoleilSerialPortNetworkStream.State.Closed;
      try
      {
        uint? hConn = this._hConn;
        if (hConn.HasValue)
        {
          this._hConn = new uint?();
          int num = (int) this._factory.Api.Btsdk_Disconnect(hConn.Value);
        }
        try
        {
          this._received.Set();
        }
        catch (ObjectDisposedException ex)
        {
        }
        this._received.Close();
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    internal override bool Connected
    {
      get => this._state == BlueSoleilSerialPortNetworkStream.State.Connected;
    }

    private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      this._received.Set();
    }

    public override IAsyncResult BeginRead(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      if (this._readDlgt == null)
        this._readDlgt = new BlueSoleilSerialPortNetworkStream.DlgtRead(((Stream) this).Read);
      return this._readDlgt.BeginInvoke(buffer, offset, count, callback, state);
    }

    public override int EndRead(IAsyncResult asyncResult) => this._readDlgt.EndInvoke(asyncResult);

    public override int Read(byte[] buffer, int offset, int count)
    {
      if (this._state != BlueSoleilSerialPortNetworkStream.State.Closed)
      {
        lock (this._received)
        {
          if (this._state != BlueSoleilSerialPortNetworkStream.State.Closed)
          {
            while (this._port.BytesToRead < 1)
            {
              if (this._state != BlueSoleilSerialPortNetworkStream.State.PeerDidClose)
              {
                this._received.Reset();
                this._received.WaitOne();
                if (this._state == BlueSoleilSerialPortNetworkStream.State.Connected)
                  Thread.MemoryBarrier();
                else
                  goto label_12;
              }
              else
                goto label_9;
            }
            return base.Read(buffer, offset, count);
          }
        }
label_12:
        switch (this._state)
        {
          case BlueSoleilSerialPortNetworkStream.State.PeerDidClose:
            return 0;
          default:
            throw new IOException("IOError on socket.", (Exception) new SocketException(10004));
        }
      }
label_9:
      switch (this._state)
      {
        case BlueSoleilSerialPortNetworkStream.State.PeerDidClose:
          return 0;
        default:
          throw new ObjectDisposedException("Network");
      }
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      if (this._state == BlueSoleilSerialPortNetworkStream.State.Closed)
        throw new ObjectDisposedException("Network");
      int val1 = this._port.WriteBufferSize / 2;
      int val2 = count;
      int offset1 = offset;
      do
      {
        int count1 = Math.Min(val1, val2);
        try
        {
          if (this._state == BlueSoleilSerialPortNetworkStream.State.PeerDidClose)
            throw new ObjectDisposedException("INTERNAL!!!");
          base.Write(buffer, offset1, count1);
        }
        catch (ObjectDisposedException ex)
        {
          if (this._state != BlueSoleilSerialPortNetworkStream.State.Connected)
          {
            if (this._state == BlueSoleilSerialPortNetworkStream.State.PeerDidClose)
            {
              SocketException innerException = new SocketException(10053);
              throw new IOException(innerException.Message, (Exception) innerException);
            }
            if (this._state == BlueSoleilSerialPortNetworkStream.State.Closed)
              throw new IOException("IOError on socket.", (Exception) new SocketException(10004));
          }
          throw;
        }
        offset1 += count1;
        val2 -= count1;
      }
      while (val2 > 0);
    }

    public override IAsyncResult BeginWrite(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      if (this._writeDlgt == null)
        this._writeDlgt = new BlueSoleilSerialPortNetworkStream.DlgtWrite(((Stream) this).Write);
      return this._writeDlgt.BeginInvoke(buffer, offset, count, callback, state);
    }

    public override void EndWrite(IAsyncResult asyncResult)
    {
      this._writeDlgt.EndInvoke(asyncResult);
    }

    private delegate int DlgtRead(byte[] buffer, int offset, int count);

    private delegate void DlgtWrite(byte[] buffer, int offset, int count);

    private enum State
    {
      New,
      Connected,
      PeerDidClose,
      Closed,
    }
  }
}
