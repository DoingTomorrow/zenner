// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.L2CapListener
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Bluetooth.Widcomm;
using System;
using System.Diagnostics;

#nullable disable
namespace InTheHand.Net.Sockets
{
  public class L2CapListener
  {
    private readonly IBluetoothListener m_impl;

    private L2CapListener(BluetoothFactory factory)
    {
      if (factory != null)
        Trace.Assert(false, "Specific factory!");
      if (PlatformVerification.IsMonoRuntime)
        return;
      this.m_impl = (IBluetoothListener) WidcommL2CapListener.Create();
    }

    private static BluetoothFactory DefaultBluetoothFactory => (BluetoothFactory) null;

    public L2CapListener(Guid service)
      : this(L2CapListener.DefaultBluetoothFactory, service)
    {
    }

    internal L2CapListener(BluetoothFactory factory, Guid service)
      : this(factory)
    {
      this.m_impl.Construct(service);
    }

    public L2CapListener(BluetoothEndPoint localEP)
      : this(L2CapListener.DefaultBluetoothFactory, localEP)
    {
    }

    internal L2CapListener(BluetoothFactory factory, BluetoothEndPoint localEP)
      : this(factory)
    {
      if (localEP == null)
        throw new ArgumentNullException(nameof (localEP));
      this.m_impl.Construct(localEP);
    }

    public void Start() => this.m_impl.Start();

    public void Start(int backlog) => this.m_impl.Start(backlog);

    public void Stop() => this.m_impl.Stop();

    public BluetoothEndPoint LocalEndPoint => this.m_impl.LocalEndPoint;

    public IAsyncResult BeginAcceptClient(AsyncCallback callback, object state)
    {
      return this.m_impl.BeginAcceptBluetoothClient(callback, state);
    }

    public L2CapClient EndAcceptClient(IAsyncResult asyncResult)
    {
      return new L2CapClient(this.m_impl.EndAcceptBluetoothClient(asyncResult));
    }

    public L2CapClient AcceptClient() => new L2CapClient(this.m_impl.AcceptBluetoothClient());

    public bool Pending() => this.m_impl.Pending();

    public string ServiceName
    {
      get => this.m_impl.ServiceName;
      set => this.m_impl.ServiceName = value;
    }

    public ServiceRecord ServiceRecord => this.m_impl.ServiceRecord;
  }
}
