// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Factory.BluetoothRfcommStreamConnector
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Widcomm;
using System;
using System.IO;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Bluetooth.Factory
{
  internal class BluetoothRfcommStreamConnector : BluetoothConnector
  {
    private NetworkStream m_netStrm;
    private readonly CommonRfcommStream m_conn;

    internal BluetoothRfcommStreamConnector(
      IUsesBluetoothConnectorImplementsServiceLookup parent,
      CommonRfcommStream conn)
      : base(parent)
    {
      this.m_conn = conn;
    }

    public NetworkStream GetStream()
    {
      this.GetStream2();
      if (this.m_netStrm == null)
        this.m_netStrm = (NetworkStream) new WidcommDecoratorNetworkStream(this.m_conn);
      return this.m_netStrm;
    }

    public Stream GetStream2()
    {
      if (!this.Connected)
        throw new InvalidOperationException("The operation is not allowed on non-connected sockets.");
      return (Stream) this.m_conn;
    }

    public LingerOption LingerState
    {
      get => this.m_conn.LingerState;
      set => this.m_conn.LingerState = value;
    }

    public bool Connected => this.m_conn.Connected;

    public void Dispose()
    {
      this.m_disposed = true;
      this.m_conn.Close();
      this.Connect_SetAsCompleted_CompletedSyncFalse((AsyncResultNoResult) null, (Exception) new ObjectDisposedException("BluetoothClient"));
    }

    protected override IAsyncResult ConnBeginConnect(
      BluetoothEndPoint remoteEP,
      AsyncCallback requestCallback,
      object state)
    {
      return this.m_conn.BeginConnect(remoteEP, requestCallback, state);
    }

    protected override void ConnEndConnect(IAsyncResult asyncResult)
    {
      this.m_conn.EndConnect(asyncResult);
    }

    public int Available => this.m_conn.AmountInReadBuffers;

    public BluetoothEndPoint RemoteEndPoint
    {
      get
      {
        if (this.m_remoteEndPoint == null)
        {
          BluetoothAddress remoteAddress = this.m_conn.RemoteAddress;
          if (this._remotePort.HasValue)
            this.m_remoteEndPoint = new BluetoothEndPoint(remoteAddress, BluetoothService.Empty, this._remotePort.Value);
          else
            this.m_remoteEndPoint = new BluetoothEndPoint(remoteAddress, BluetoothService.Empty);
        }
        return this.m_remoteEndPoint;
      }
    }
  }
}
