// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.BluetoothListener
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Factory;
using System;
using System.Net;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Sockets
{
  public class BluetoothListener
  {
    private readonly IBluetoothListener m_impl;

    private BluetoothListener(BluetoothFactory factory)
    {
      this.m_impl = factory.DoGetBluetoothListener();
      this.m_impl.ToString();
    }

    private static BluetoothFactory DefaultBluetoothFactory => BluetoothFactory.Factory;

    public BluetoothListener(Guid service)
      : this(BluetoothListener.DefaultBluetoothFactory, service)
    {
    }

    internal BluetoothListener(BluetoothFactory factory, Guid service)
      : this(factory)
    {
      this.m_impl.Construct(service);
    }

    public BluetoothListener(BluetoothAddress localaddr, Guid service)
      : this(BluetoothListener.DefaultBluetoothFactory, localaddr, service)
    {
    }

    internal BluetoothListener(BluetoothFactory factory, BluetoothAddress localaddr, Guid service)
      : this(factory)
    {
      if (localaddr == (BluetoothAddress) null)
        throw new ArgumentNullException(nameof (localaddr));
      if (service == Guid.Empty)
        throw new ArgumentNullException(nameof (service));
      this.m_impl.Construct(localaddr, service);
    }

    public BluetoothListener(BluetoothEndPoint localEP)
      : this(BluetoothListener.DefaultBluetoothFactory, localEP)
    {
    }

    internal BluetoothListener(BluetoothFactory factory, BluetoothEndPoint localEP)
      : this(factory)
    {
      if (localEP == null)
        throw new ArgumentNullException(nameof (localEP));
      this.m_impl.Construct(localEP);
    }

    public BluetoothListener(Guid service, byte[] sdpRecord, int channelOffset)
      : this(BluetoothListener.DefaultBluetoothFactory, service, sdpRecord, channelOffset)
    {
    }

    internal BluetoothListener(
      BluetoothFactory factory,
      Guid service,
      byte[] sdpRecord,
      int channelOffset)
      : this(factory)
    {
      this.m_impl.Construct(service, sdpRecord, channelOffset);
    }

    public BluetoothListener(
      BluetoothAddress localaddr,
      Guid service,
      byte[] sdpRecord,
      int channelOffset)
      : this(BluetoothListener.DefaultBluetoothFactory, localaddr, service, sdpRecord, channelOffset)
    {
    }

    internal BluetoothListener(
      BluetoothFactory factory,
      BluetoothAddress localaddr,
      Guid service,
      byte[] sdpRecord,
      int channelOffset)
      : this(factory)
    {
      this.m_impl.Construct(localaddr, service, sdpRecord, channelOffset);
    }

    public BluetoothListener(BluetoothEndPoint localEP, byte[] sdpRecord, int channelOffset)
      : this(BluetoothListener.DefaultBluetoothFactory, localEP, sdpRecord, channelOffset)
    {
    }

    internal BluetoothListener(
      BluetoothFactory factory,
      BluetoothEndPoint localEP,
      byte[] sdpRecord,
      int channelOffset)
      : this(factory)
    {
      this.m_impl.Construct(localEP, sdpRecord, channelOffset);
    }

    public BluetoothListener(Guid service, ServiceRecord sdpRecord)
      : this(BluetoothListener.DefaultBluetoothFactory, service, sdpRecord)
    {
    }

    internal BluetoothListener(BluetoothFactory factory, Guid service, ServiceRecord sdpRecord)
      : this(factory)
    {
      this.m_impl.Construct(service, sdpRecord);
    }

    public BluetoothListener(BluetoothAddress localaddr, Guid service, ServiceRecord sdpRecord)
      : this(BluetoothListener.DefaultBluetoothFactory, localaddr, service, sdpRecord)
    {
    }

    internal BluetoothListener(
      BluetoothFactory factory,
      BluetoothAddress localaddr,
      Guid service,
      ServiceRecord sdpRecord)
      : this(factory)
    {
      this.m_impl.Construct(localaddr, service, sdpRecord);
    }

    public BluetoothListener(BluetoothEndPoint localEP, ServiceRecord sdpRecord)
      : this(BluetoothListener.DefaultBluetoothFactory, localEP, sdpRecord)
    {
    }

    internal BluetoothListener(
      BluetoothFactory factory,
      BluetoothEndPoint localEP,
      ServiceRecord sdpRecord)
      : this(factory)
    {
      this.m_impl.Construct(localEP, sdpRecord);
    }

    public BluetoothEndPoint LocalEndPoint => this.m_impl.LocalEndPoint;

    public ServiceClass ServiceClass
    {
      get => this.m_impl.ServiceClass;
      set => this.m_impl.ServiceClass = value;
    }

    public string ServiceName
    {
      get => this.m_impl.ServiceName;
      set => this.m_impl.ServiceName = value;
    }

    public Socket Server => this.m_impl.Server;

    public void Start() => this.m_impl.Start();

    public void Start(int backlog) => this.m_impl.Start(backlog);

    public void Stop() => this.m_impl.Stop();

    public IAsyncResult BeginAcceptSocket(AsyncCallback callback, object state)
    {
      return this.m_impl.BeginAcceptSocket(callback, state);
    }

    public Socket EndAcceptSocket(IAsyncResult asyncResult)
    {
      return this.m_impl.EndAcceptSocket(asyncResult);
    }

    public IAsyncResult BeginAcceptBluetoothClient(AsyncCallback callback, object state)
    {
      return this.m_impl.BeginAcceptBluetoothClient(callback, state);
    }

    public BluetoothClient EndAcceptBluetoothClient(IAsyncResult asyncResult)
    {
      return new BluetoothClient(this.m_impl.EndAcceptBluetoothClient(asyncResult));
    }

    public Socket AcceptSocket() => this.m_impl.AcceptSocket();

    public BluetoothClient AcceptBluetoothClient()
    {
      return new BluetoothClient(this.m_impl.AcceptBluetoothClient());
    }

    public bool Pending() => this.m_impl.Pending();

    public ServiceRecord ServiceRecord => this.m_impl.ServiceRecord;

    public bool Authenticate
    {
      get => this.m_impl.Authenticate;
      set => this.m_impl.Authenticate = value;
    }

    public bool Encrypt
    {
      get => this.m_impl.Encrypt;
      set => this.m_impl.Encrypt = value;
    }

    public void SetPin(BluetoothAddress device, string pin) => this.m_impl.SetPin(device, pin);

    internal static Guid HostToNetworkOrder(Guid hostGuid)
    {
      byte[] byteArray = hostGuid.ToByteArray();
      BitConverter.GetBytes(IPAddress.HostToNetworkOrder(BitConverter.ToInt32(byteArray, 0))).CopyTo((Array) byteArray, 0);
      BitConverter.GetBytes(IPAddress.HostToNetworkOrder(BitConverter.ToInt16(byteArray, 4))).CopyTo((Array) byteArray, 4);
      BitConverter.GetBytes(IPAddress.HostToNetworkOrder(BitConverter.ToInt16(byteArray, 6))).CopyTo((Array) byteArray, 6);
      return new Guid(byteArray);
    }
  }
}
