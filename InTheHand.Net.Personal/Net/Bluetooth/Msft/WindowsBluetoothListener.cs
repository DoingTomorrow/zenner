// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.WindowsBluetoothListener
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Sockets;
using System;
using System.Net;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  internal class WindowsBluetoothListener : IBluetoothListener
  {
    private readonly BluetoothFactory _fcty;
    private bool active;
    private BluetoothEndPoint serverEP;
    private ISocketOptionHelper m_optionHelper;
    private IntPtr serviceHandle;
    private ServiceRecord m_serviceRecord;
    private bool m_manualServiceRecord;
    private ServiceClass codService;
    private byte[] m_activeServiceRecordBytes;
    private string m_serviceName;
    private Socket serverSocket;

    internal WindowsBluetoothListener(BluetoothFactory fcty) => this._fcty = fcty;

    protected virtual Socket CreateSocket()
    {
      return new Socket(this.BluetoothAddressFamily, SocketType.Stream, ProtocolType.Ggp);
    }

    protected virtual AddressFamily BluetoothAddressFamily => (AddressFamily) 32;

    protected virtual ISocketOptionHelper CreateSocketOptionHelper(Socket socket)
    {
      return (ISocketOptionHelper) new SocketBluetoothClient.MsftSocketOptionHelper(socket);
    }

    public void Construct(Guid service)
    {
      this.InitServiceRecord(service);
      this.serverEP = new BluetoothEndPoint(BluetoothAddress.None, service);
      this.serverSocket = this.CreateSocket();
      this.m_optionHelper = this.CreateSocketOptionHelper(this.serverSocket);
    }

    public void Construct(BluetoothAddress localAddress, Guid service)
    {
      if (localAddress == (BluetoothAddress) null)
        throw new ArgumentNullException(nameof (localAddress));
      if (service == Guid.Empty)
        throw new ArgumentNullException(nameof (service));
      this.InitServiceRecord(service);
      this.serverEP = new BluetoothEndPoint(localAddress, service);
      this.serverSocket = this.CreateSocket();
      this.m_optionHelper = this.CreateSocketOptionHelper(this.serverSocket);
    }

    public void Construct(BluetoothEndPoint localEP)
    {
      if (localEP == null)
        throw new ArgumentNullException(nameof (localEP));
      this.InitServiceRecord(localEP.Service);
      this.serverEP = localEP;
      this.serverSocket = this.CreateSocket();
      this.m_optionHelper = this.CreateSocketOptionHelper(this.serverSocket);
    }

    public void Construct(Guid service, byte[] sdpRecord, int channelOffset)
    {
      this.Construct(service);
      this.InitServiceRecord(sdpRecord, channelOffset);
    }

    public void Construct(
      BluetoothAddress localAddress,
      Guid service,
      byte[] sdpRecord,
      int channelOffset)
    {
      this.Construct(localAddress, service);
      this.InitServiceRecord(sdpRecord, channelOffset);
    }

    public void Construct(BluetoothEndPoint localEP, byte[] sdpRecord, int channelOffset)
    {
      this.Construct(localEP);
      this.InitServiceRecord(sdpRecord, channelOffset);
    }

    public void Construct(Guid service, ServiceRecord sdpRecord)
    {
      this.Construct(service);
      this.InitServiceRecord(sdpRecord);
    }

    public void Construct(BluetoothAddress localAddress, Guid service, ServiceRecord sdpRecord)
    {
      this.Construct(localAddress, service);
      this.InitServiceRecord(sdpRecord);
    }

    public void Construct(BluetoothEndPoint localEP, ServiceRecord sdpRecord)
    {
      this.Construct(localEP);
      this.InitServiceRecord(sdpRecord);
    }

    public BluetoothEndPoint LocalEndPoint
    {
      get => !this.active ? this.serverEP : (BluetoothEndPoint) this.serverSocket.LocalEndPoint;
    }

    public ServiceClass ServiceClass
    {
      get => this.codService;
      set => this.codService = value;
    }

    public string ServiceName
    {
      get => this.m_serviceName;
      set
      {
        if (this.active)
          throw new InvalidOperationException("Can not change ServiceName when started.");
        if (this.m_manualServiceRecord)
          throw new InvalidOperationException("ServiceName may not be specified when a custom Service Record is being used.");
        this.m_serviceName = value;
        this.InitServiceRecord(this.serverEP.Service);
      }
    }

    public Socket Server => this.serverSocket;

    public void Start() => this.Start(int.MaxValue);

    public void Start(int backlog)
    {
      if (backlog > int.MaxValue || backlog < 0)
        throw new ArgumentOutOfRangeException(nameof (backlog));
      if (this.serverSocket == null)
        throw new InvalidOperationException("The socket handle is not valid.");
      if (this.active)
        return;
      this.serverEP = this.PrepareBindEndPoint((BluetoothEndPoint) this.serverEP.Clone());
      this.serverSocket.Bind((EndPoint) this.serverEP);
      this.serverSocket.Listen(backlog);
      this.active = true;
      this.SetService((byte) ((BluetoothEndPoint) this.serverSocket.LocalEndPoint).Port);
    }

    protected virtual BluetoothEndPoint PrepareBindEndPoint(BluetoothEndPoint serverEP) => serverEP;

    private void SetService(byte channelNumber)
    {
      this.SetService(this.GetServiceRecordBytes(channelNumber), this.codService);
    }

    public void Stop()
    {
      if (this.serverSocket != null)
      {
        try
        {
          this.RemoveService();
        }
        finally
        {
          this.serverSocket.Close();
          this.serverSocket = (Socket) null;
        }
      }
      this.active = false;
      this.serverSocket = this.CreateSocket();
    }

    protected virtual void RemoveService()
    {
      if (!(this.serviceHandle != IntPtr.Zero))
        return;
      WindowsBluetoothListener.RemoveService(this.serviceHandle, this.m_activeServiceRecordBytes);
      this.serviceHandle = IntPtr.Zero;
    }

    public IAsyncResult BeginAcceptSocket(AsyncCallback callback, object state)
    {
      if (!this.active)
        throw new InvalidOperationException("Not listening. You must call the Start() method before calling this method.");
      return this.serverSocket.BeginAccept(callback, state);
    }

    public Socket EndAcceptSocket(IAsyncResult asyncResult)
    {
      return asyncResult != null ? this.serverSocket.EndAccept(asyncResult) : throw new ArgumentNullException(nameof (asyncResult));
    }

    public IAsyncResult BeginAcceptBluetoothClient(AsyncCallback callback, object state)
    {
      if (!this.active)
        throw new InvalidOperationException("Not listening. You must call the Start() method before calling this method.");
      return this.serverSocket.BeginAccept(callback, state);
    }

    public IBluetoothClient EndAcceptBluetoothClient(IAsyncResult asyncResult)
    {
      return asyncResult != null ? this.MakeIBluetoothClient(this.serverSocket.EndAccept(asyncResult)) : throw new ArgumentNullException(nameof (asyncResult));
    }

    public Socket AcceptSocket()
    {
      if (!this.active)
        throw new InvalidOperationException("Not listening. You must call the Start() method before calling this method.");
      return this.serverSocket.Accept();
    }

    public IBluetoothClient AcceptBluetoothClient()
    {
      return this.MakeIBluetoothClient(this.AcceptSocket());
    }

    protected virtual IBluetoothClient MakeIBluetoothClient(Socket s)
    {
      return this._fcty.DoGetBluetoothClient(s);
    }

    public bool Pending()
    {
      if (!this.active)
        throw new InvalidOperationException("Not listening. You must call the Start() method before calling this method.");
      return this.serverSocket.Poll(0, SelectMode.SelectRead);
    }

    public ServiceRecord ServiceRecord => this.m_serviceRecord;

    private void InitServiceRecord(Guid serviceClassUuid)
    {
      this.m_serviceRecord = WindowsBluetoothListener.CreateBasicRfcommRecord(serviceClassUuid, this.m_serviceName);
    }

    private static ServiceRecord CreateBasicRfcommRecord(Guid serviceClassUuid, string svcName)
    {
      ServiceRecordBuilder serviceRecordBuilder = new ServiceRecordBuilder();
      serviceRecordBuilder.AddServiceClass(serviceClassUuid);
      if (svcName != null)
        serviceRecordBuilder.ServiceName = svcName;
      return serviceRecordBuilder.ServiceRecord;
    }

    private void InitServiceRecord(ServiceRecord sdpRecord)
    {
      if (sdpRecord == null)
        throw new ArgumentNullException(nameof (sdpRecord));
      this.m_serviceRecord = ServiceRecordHelper.GetRfcommChannelNumber(sdpRecord) != -1 ? sdpRecord : throw new ArgumentException("The ServiceRecord must contain a RFCOMM-style ProtocolDescriptorList.");
      this.m_manualServiceRecord = true;
    }

    private void InitServiceRecord(byte[] sdpRecord, int channelOffset)
    {
      if (sdpRecord.Length == 0)
        throw new ArgumentException("sdpRecord must not be empty.");
      if (channelOffset >= sdpRecord.Length)
        throw new ArgumentOutOfRangeException(nameof (channelOffset));
      this.m_serviceRecord = ServiceRecord.CreateServiceRecordFromBytes(sdpRecord);
      this.m_manualServiceRecord = true;
    }

    private byte[] GetServiceRecordBytes(byte channelNumber)
    {
      ServiceRecord serviceRecord = this.m_serviceRecord;
      ServiceRecordHelper.SetRfcommChannelNumber(serviceRecord, channelNumber);
      this.m_activeServiceRecordBytes = serviceRecord.ToByteArray();
      return this.m_activeServiceRecordBytes;
    }

    private static void RemoveService(IntPtr handle, byte[] sdpRecord)
    {
      MicrosoftSdpService.RemoveService(handle, sdpRecord);
    }

    protected virtual void SetService(byte[] sdpRecord, ServiceClass cod)
    {
      this.serviceHandle = MicrosoftSdpService.SetService(sdpRecord, cod);
    }

    public bool Authenticate
    {
      get => this.m_optionHelper.Authenticate;
      set => this.m_optionHelper.Authenticate = value;
    }

    public bool Encrypt
    {
      get => this.m_optionHelper.Encrypt;
      set => this.m_optionHelper.Encrypt = value;
    }

    public void SetPin(BluetoothAddress device, string pin)
    {
      this.m_optionHelper.SetPin(device, pin);
    }

    public void SetPin(string pin)
    {
      throw new NotSupportedException("Must supply the remote address on Win32.");
    }
  }
}
