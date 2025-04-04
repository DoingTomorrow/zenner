// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Factory.CommonBluetoothClient
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Collections.Generic;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Bluetooth.Factory
{
  public abstract class CommonBluetoothClient : 
    CommonDiscoveryBluetoothClient,
    IUsesBluetoothConnectorImplementsServiceLookup
  {
    private readonly BluetoothFactory _fcty;
    private readonly BluetoothRfcommStreamConnector m_conn;

    [CLSCompliant(false)]
    protected CommonBluetoothClient(BluetoothFactory factory, CommonRfcommStream conn)
    {
      this._fcty = factory;
      this.m_conn = new BluetoothRfcommStreamConnector((IUsesBluetoothConnectorImplementsServiceLookup) this, conn);
    }

    public override NetworkStream GetStream() => this.m_conn.GetStream();

    public override LingerOption LingerState
    {
      get => this.m_conn.LingerState;
      set => this.m_conn.LingerState = value;
    }

    public override bool Connected => this.m_conn.Connected;

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (!disposing)
          return;
        this.m_conn.Dispose();
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    public override sealed void Connect(BluetoothEndPoint remoteEP)
    {
      this.BeforeConnectAttempt(remoteEP.Address);
      try
      {
        this.m_conn.Connect(remoteEP);
      }
      finally
      {
        this.AfterConnectAttempt();
      }
    }

    public override sealed IAsyncResult BeginConnect(
      BluetoothEndPoint remoteEP,
      AsyncCallback requestCallback,
      object state)
    {
      this.BeforeConnectAttempt(remoteEP.Address);
      return this.m_conn.BeginConnect(remoteEP, requestCallback, state);
    }

    public override sealed void EndConnect(IAsyncResult asyncResult)
    {
      try
      {
        this.m_conn.EndConnect(asyncResult);
      }
      finally
      {
        this.AfterConnectAttempt();
      }
    }

    protected virtual void BeforeConnectAttempt(BluetoothAddress target)
    {
    }

    protected virtual void AfterConnectAttempt()
    {
    }

    public abstract IAsyncResult BeginServiceDiscovery(
      BluetoothAddress address,
      Guid serviceGuid,
      AsyncCallback asyncCallback,
      object state);

    public abstract List<int> EndServiceDiscovery(IAsyncResult ar);

    public override int Available => this.m_conn.Available;

    public override Socket Client
    {
      get => throw new NotSupportedException("This stack does not use Sockets.");
      set => throw new NotSupportedException("This stack does not use Sockets.");
    }

    public override bool Authenticate
    {
      get => false;
      set => throw new NotImplementedException("The method or operation is not implemented.");
    }

    public override bool Encrypt
    {
      get => false;
      set => throw new NotImplementedException("The method or operation is not implemented.");
    }

    public override BluetoothEndPoint RemoteEndPoint => this.m_conn.RemoteEndPoint;

    public override string GetRemoteMachineName(BluetoothAddress device)
    {
      return this._fcty.DoGetBluetoothDeviceInfo(device).DeviceName;
    }

    public override Guid LinkKey
    {
      get => throw new NotImplementedException("The method or operation is not implemented.");
    }

    public override LinkPolicy LinkPolicy
    {
      get => throw new NotImplementedException("The method or operation is not implemented.");
    }

    public override string RemoteMachineName
    {
      get => this.GetRemoteMachineName(this.RemoteEndPoint.Address);
    }
  }
}
