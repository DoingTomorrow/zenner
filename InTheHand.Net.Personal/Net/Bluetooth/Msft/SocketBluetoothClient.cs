// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.SocketBluetoothClient
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Sockets;
using InTheHand.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  internal class SocketBluetoothClient : IBluetoothClient, IDisposable
  {
    private readonly BluetoothFactory _fcty;
    private bool cleanedUp;
    private ISocketOptionHelper m_optionHelper;
    private string m_pinForConnect;
    private int iac = 10390323;
    private TimeSpan inquiryLength = new TimeSpan(0, 0, 10);
    private bool active;
    private Socket clientSocket;
    private NetworkStream dataStream;

    internal SocketBluetoothClient(BluetoothFactory fcty)
    {
      this._fcty = fcty;
      try
      {
        this.Client = this.CreateSocket();
      }
      catch (SocketException ex)
      {
        throw new PlatformNotSupportedException("32feet.NET does not support the Bluetooth stack on this device.", (Exception) ex);
      }
      this.m_optionHelper = this.CreateSocketOptionHelper(this.Client);
    }

    internal SocketBluetoothClient(BluetoothFactory fcty, BluetoothEndPoint localEP)
      : this(fcty)
    {
      if (localEP == null)
        throw new ArgumentNullException(nameof (localEP));
      this.Client.Bind((EndPoint) this.PrepareBindEndPoint(localEP));
    }

    internal SocketBluetoothClient(BluetoothFactory fcty, Socket acceptedSocket)
    {
      this._fcty = fcty;
      this.Client = acceptedSocket;
      this.active = true;
      this.m_optionHelper = this.CreateSocketOptionHelper(this.Client);
    }

    protected virtual Socket CreateSocket()
    {
      return new Socket(this.BluetoothAddressFamily, SocketType.Stream, ProtocolType.Ggp);
    }

    protected virtual AddressFamily BluetoothAddressFamily => (AddressFamily) 32;

    protected virtual BluetoothEndPoint PrepareConnectEndPoint(BluetoothEndPoint serverEP)
    {
      return serverEP;
    }

    protected virtual BluetoothEndPoint PrepareBindEndPoint(BluetoothEndPoint serverEP) => serverEP;

    protected virtual ISocketOptionHelper CreateSocketOptionHelper(Socket socket)
    {
      return (ISocketOptionHelper) new SocketBluetoothClient.MsftSocketOptionHelper(socket);
    }

    public int InquiryAccessCode
    {
      get => throw new NotSupportedException();
      set => throw new NotSupportedException();
    }

    public TimeSpan InquiryLength
    {
      get => this.inquiryLength;
      set
      {
        this.inquiryLength = value.TotalSeconds > 0.0 && value.TotalSeconds <= 60.0 ? value : throw new ArgumentOutOfRangeException(nameof (value), "QueryLength must be a positive timespan between 0 and 60 seconds.");
      }
    }

    IBluetoothDeviceInfo[] IBluetoothClient.DiscoverDevices(
      int maxDevices,
      bool authenticated,
      bool remembered,
      bool unknown,
      bool discoverableOnly)
    {
      return this.DiscoverDevices(maxDevices, authenticated, remembered, unknown, discoverableOnly, (BluetoothClient.LiveDiscoveryCallback) null, (object) null);
    }

    public virtual IBluetoothDeviceInfo[] DiscoverDevices(
      int maxDevices,
      bool authenticated,
      bool remembered,
      bool unknown,
      bool discoverableOnly,
      BluetoothClient.LiveDiscoveryCallback liveDiscoHandler,
      object liveDiscoState)
    {
      BluetoothClient.LiveDiscoveryCallback realLiveDiscoHandler = liveDiscoHandler;
      bool flag = false;
      if (discoverableOnly)
      {
        flag = true;
        int num;
        remembered = (num = 1) != 0;
        authenticated = num != 0;
        unknown = num != 0;
        discoverableOnly = false;
      }
      BluetoothWin32Events bluetoothWin32Events;
      EventHandler<BluetoothWin32RadioInRangeEventArgs> eventHandler;
      List<IBluetoothDeviceInfo> seenDevices;
      if (flag || realLiveDiscoHandler != null)
      {
        bluetoothWin32Events = BluetoothWin32Events.GetInstance();
        seenDevices = new List<IBluetoothDeviceInfo>();
        eventHandler = (EventHandler<BluetoothWin32RadioInRangeEventArgs>) ((sender, e) =>
        {
          WindowsBluetoothDeviceInfo deviceWindows = e.DeviceWindows;
          if (!BluetoothDeviceInfo.AddUniqueDevice(seenDevices, (IBluetoothDeviceInfo) deviceWindows) || realLiveDiscoHandler == null)
            return;
          realLiveDiscoHandler((IBluetoothDeviceInfo) deviceWindows, liveDiscoState);
        });
        bluetoothWin32Events.InRange += eventHandler;
      }
      else
      {
        bluetoothWin32Events = (BluetoothWin32Events) null;
        eventHandler = (EventHandler<BluetoothWin32RadioInRangeEventArgs>) null;
        seenDevices = (List<IBluetoothDeviceInfo>) null;
      }
      liveDiscoHandler = (BluetoothClient.LiveDiscoveryCallback) null;
      IBluetoothDeviceInfo[] list;
      try
      {
        list = this.DoDiscoverDevices(maxDevices, authenticated, remembered, unknown, discoverableOnly, liveDiscoHandler, liveDiscoState);
      }
      finally
      {
        if (bluetoothWin32Events != null && eventHandler != null)
          bluetoothWin32Events.InRange -= eventHandler;
      }
      if (flag)
        list = BluetoothDeviceInfo.Intersect(list, seenDevices);
      return list;
    }

    private IBluetoothDeviceInfo[] DoDiscoverDevices(
      int maxDevices,
      bool authenticated,
      bool remembered,
      bool unknown,
      bool discoverableOnly,
      BluetoothClient.LiveDiscoveryCallback liveDiscoHandler,
      object liveDiscoState)
    {
      BluetoothEndPoint bluetoothEndPoint = new BluetoothEndPoint(BluetoothAddress.None, BluetoothService.Empty);
      int num1 = 0;
      IntPtr lphLookup = IntPtr.Zero;
      int num2 = 0;
      StringBuilder timings = (StringBuilder) null;
      Action<string> action = (Action<string>) (name => timings?.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{1}: {0}\r\n", (object) name, (object) DateTime.UtcNow.TimeOfDay));
      if (discoverableOnly)
        return new IBluetoothDeviceInfo[0];
      List<IBluetoothDeviceInfo> bluetoothDeviceInfoList = new List<IBluetoothDeviceInfo>();
      byte[] numArray = new byte[1024];
      BitConverter.GetBytes(WqsOffset.StructLength_60).CopyTo((Array) numArray, WqsOffset.dwSize_0);
      BitConverter.GetBytes(16).CopyTo((Array) numArray, WqsOffset.dwNameSpace_20);
      int length = numArray.Length;
      GCHandle gcHandle1 = GCHandle.Alloc((object) new BTHNS_INQUIRYBLOB()
      {
        LAP = this.iac,
        length = Convert.ToByte(this.inquiryLength.TotalSeconds)
      }, GCHandleType.Pinned);
      GCHandle gcHandle2 = GCHandle.Alloc((object) new BLOB(8, gcHandle1.AddrOfPinnedObject()), GCHandleType.Pinned);
      Marshal32.WriteIntPtr(numArray, WqsOffset.lpBlob_56, gcHandle2.AddrOfPinnedObject());
      LookupFlags dwFlags1 = LookupFlags.Containers;
      if (unknown || discoverableOnly)
        dwFlags1 |= LookupFlags.FlushCache;
      action("Begin");
      int num3 = NativeMethods.WSALookupServiceBegin(numArray, dwFlags1, out lphLookup);
      action("Begin complete");
      gcHandle2.Free();
      gcHandle1.Free();
      while (num1 < maxDevices && num3 != -1)
      {
        action("Next");
        LookupFlags dwFlags2 = LookupFlags.ReturnAddr;
        num3 = NativeMethods.WSALookupServiceNext(lphLookup, dwFlags2, ref length, numArray);
        action("Next Complete");
        if (num3 != -1)
        {
          ++num1;
          BTHNS_RESULT int32 = (BTHNS_RESULT) BitConverter.ToInt32(numArray, WqsOffset.dwOutputFlags_52);
          bool flag1 = (int32 & BTHNS_RESULT.Authenticated) == BTHNS_RESULT.Authenticated;
          bool flag2 = (int32 & BTHNS_RESULT.Remembered) == BTHNS_RESULT.Remembered;
          if (flag1)
          {
            int num4 = flag2 ? 1 : 0;
          }
          bool flag3 = !flag2 && !flag1;
          if (authenticated && flag1 || remembered && flag2 || unknown && flag3)
          {
            IntPtr ptr1 = Marshal32.ReadIntPtr(numArray, WqsOffset.lpcsaBuffer_48);
            IntPtr ptr2 = Marshal32.ReadIntPtr(ptr1, CsaddrInfoOffsets.OffsetRemoteAddr_lpSockaddr_8);
            int size = Marshal.ReadInt32(ptr1, CsaddrInfoOffsets.OffsetRemoteAddr_iSockaddrLength_12);
            SocketAddress socketAddress = new SocketAddress((AddressFamily) 32, size);
            for (int index = 0; index < size; ++index)
              socketAddress[index] = Marshal.ReadByte(ptr2, index);
            IBluetoothDeviceInfo p1 = (IBluetoothDeviceInfo) new WindowsBluetoothDeviceInfo(((BluetoothEndPoint) bluetoothEndPoint.Create(socketAddress)).Address);
            bluetoothDeviceInfoList.Add(p1);
            if (liveDiscoHandler != null)
              liveDiscoHandler(p1, liveDiscoState);
          }
        }
      }
      if (lphLookup != IntPtr.Zero)
      {
        action("End");
        num2 = NativeMethods.WSALookupServiceEnd(lphLookup);
        action("End Complete");
      }
      if (timings != null)
        Console.WriteLine((object) timings);
      return bluetoothDeviceInfoList.Count == 0 ? new IBluetoothDeviceInfo[0] : bluetoothDeviceInfoList.ToArray();
    }

    private void ReadBlobBTH_DEVICE_INFO(byte[] buffer, IBluetoothDeviceInfo dev)
    {
      IntPtr ptr = Marshal32.ReadIntPtr(buffer, WqsOffset.lpBlob_56);
      if (!(ptr != IntPtr.Zero))
        return;
      BLOB structure = (BLOB) Marshal.PtrToStructure(ptr, typeof (BLOB));
      if (!(structure.pBlobData != IntPtr.Zero))
        return;
      Trace.Assert((((BTH_DEVICE_INFO) Marshal.PtrToStructure(structure.pBlobData, typeof (BTH_DEVICE_INFO))).flags & ~(BluetoothDeviceInfoProperties.Address | BluetoothDeviceInfoProperties.Cod | BluetoothDeviceInfoProperties.Name | BluetoothDeviceInfoProperties.Paired | BluetoothDeviceInfoProperties.Personal | BluetoothDeviceInfoProperties.Connected)) == (BluetoothDeviceInfoProperties) 0, "*Are* new flags there!");
    }

    IAsyncResult IBluetoothClient.BeginDiscoverDevices(
      int maxDevices,
      bool authenticated,
      bool remembered,
      bool unknown,
      bool discoverableOnly,
      AsyncCallback callback,
      object state)
    {
      return ((IBluetoothClient) this).BeginDiscoverDevices(maxDevices, authenticated, remembered, unknown, discoverableOnly, callback, state, (BluetoothClient.LiveDiscoveryCallback) null, (object) null);
    }

    IAsyncResult IBluetoothClient.BeginDiscoverDevices(
      int maxDevices,
      bool authenticated,
      bool remembered,
      bool unknown,
      bool discoverableOnly,
      AsyncCallback callback,
      object state,
      BluetoothClient.LiveDiscoveryCallback liveDiscoHandler,
      object liveDiscoState)
    {
      SocketBluetoothClient.DiscoDevsParamsWithLive args = new SocketBluetoothClient.DiscoDevsParamsWithLive(maxDevices, authenticated, remembered, unknown, discoverableOnly, DateTime.MinValue, liveDiscoHandler, liveDiscoState);
      AsyncResult<IBluetoothDeviceInfo[], SocketBluetoothClient.DiscoDevsParamsWithLive> state1 = new AsyncResult<IBluetoothDeviceInfo[], SocketBluetoothClient.DiscoDevsParamsWithLive>(callback, state, args);
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.BeginDiscoverDevices_Runner), (object) state1);
      return (IAsyncResult) state1;
    }

    private void BeginDiscoverDevices_Runner(object state)
    {
      AsyncResult<IBluetoothDeviceInfo[], SocketBluetoothClient.DiscoDevsParamsWithLive> ar = (AsyncResult<IBluetoothDeviceInfo[], SocketBluetoothClient.DiscoDevsParamsWithLive>) state;
      ar.SetAsCompletedWithResultOf((Func<IBluetoothDeviceInfo[]>) (() => this.DiscoverDevices(ar.BeginParameters.maxDevices, ar.BeginParameters.authenticated, ar.BeginParameters.remembered, ar.BeginParameters.unknown, ar.BeginParameters.discoverableOnly, ar.BeginParameters._liveDiscoHandler, ar.BeginParameters._liveDiscoState)), false);
    }

    IBluetoothDeviceInfo[] IBluetoothClient.EndDiscoverDevices(IAsyncResult asyncResult)
    {
      return ((AsyncResult<IBluetoothDeviceInfo[]>) asyncResult).EndInvoke();
    }

    protected bool Active
    {
      get => this.active;
      set => this.active = value;
    }

    public int Available
    {
      get
      {
        this.EnsureNotDisposed();
        return this.clientSocket.Available;
      }
    }

    public Socket Client
    {
      get => this.clientSocket;
      set => this.clientSocket = value;
    }

    public virtual void Connect(BluetoothEndPoint remoteEP)
    {
      this.EnsureNotDisposed();
      if (remoteEP == null)
        throw new ArgumentNullException(nameof (remoteEP));
      this.Connect_StartAuthenticator(remoteEP);
      try
      {
        this.clientSocket.Connect((EndPoint) this.PrepareConnectEndPoint(remoteEP));
        this.active = true;
      }
      finally
      {
        this.Connect_StopAuthenticator();
      }
    }

    public virtual IAsyncResult BeginConnect(
      BluetoothEndPoint remoteEP,
      AsyncCallback requestCallback,
      object state)
    {
      this.EnsureNotDisposed();
      this.Connect_StartAuthenticator(remoteEP);
      return this.Client.BeginConnect((EndPoint) this.PrepareConnectEndPoint(remoteEP), requestCallback, state);
    }

    public virtual void EndConnect(IAsyncResult asyncResult)
    {
      try
      {
        (this.Client ?? throw new ObjectDisposedException("BluetoothClient")).EndConnect(asyncResult);
        this.active = true;
      }
      finally
      {
        this.Connect_StopAuthenticator();
      }
    }

    public bool Connected => this.clientSocket != null && this.clientSocket.Connected;

    protected virtual NetworkStream MakeStream(Socket sock) => new NetworkStream(this.Client, true);

    public NetworkStream GetStream()
    {
      this.EnsureNotDisposed();
      if (!this.Client.Connected)
        throw new InvalidOperationException("The operation is not allowed on non-connected sockets.");
      if (this.dataStream == null)
        this.dataStream = this.MakeStream(this.Client);
      return this.dataStream;
    }

    public LingerOption LingerState
    {
      get => this.Client.LingerState;
      set => this.Client.LingerState = value;
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

    public Guid LinkKey
    {
      get
      {
        this.EnsureNotDisposed();
        byte[] socketOption = this.clientSocket.GetSocketOption((SocketOptionLevel) 3, SocketOptionName.Debug | SocketOptionName.ReuseAddress, 32);
        byte[] numArray = new byte[16];
        Buffer.BlockCopy((Array) socketOption, 16, (Array) numArray, 0, 16);
        return new Guid(numArray);
      }
    }

    public LinkPolicy LinkPolicy
    {
      get
      {
        this.EnsureNotDisposed();
        return (LinkPolicy) BitConverter.ToInt32(this.clientSocket.GetSocketOption((SocketOptionLevel) 3, SocketOptionName.ReuseAddress | SocketOptionName.Broadcast, 4), 0);
      }
    }

    public void SetPin(string pin)
    {
      if (!this.Connected)
      {
        this.m_pinForConnect = pin;
      }
      else
      {
        EndPoint remoteEndPoint = this.clientSocket.RemoteEndPoint;
        BluetoothAddress device = (BluetoothAddress) null;
        if (remoteEndPoint != null)
          device = ((BluetoothEndPoint) remoteEndPoint).Address;
        if (device == (BluetoothAddress) null)
          throw new InvalidOperationException("The socket needs to be connected to detect the remote device, use the other SetPin method..");
        this.SetPin(device, pin);
      }
    }

    public void SetPin(BluetoothAddress device, string pin)
    {
      this.m_optionHelper.SetPin(device, pin);
    }

    private void Connect_StartAuthenticator(BluetoothEndPoint remoteEP)
    {
      if (this.m_pinForConnect == null)
        return;
      this.SetPin(remoteEP.Address, this.m_pinForConnect);
    }

    private void Connect_StopAuthenticator()
    {
      if (this.m_pinForConnect == null)
        return;
      this.SetPin((BluetoothAddress) null, (string) null);
    }

    public BluetoothEndPoint RemoteEndPoint
    {
      get
      {
        this.EnsureNotDisposed();
        return (BluetoothEndPoint) this.clientSocket.RemoteEndPoint;
      }
    }

    public string RemoteMachineName
    {
      get
      {
        this.EnsureNotDisposed();
        return SocketBluetoothClient.GetRemoteMachineName(this.clientSocket);
      }
    }

    public string GetRemoteMachineName(BluetoothAddress a)
    {
      return this._fcty.DoGetBluetoothDeviceInfo(a).DeviceName;
    }

    public static string GetRemoteMachineName(Socket s)
    {
      return new BluetoothDeviceInfo(((BluetoothEndPoint) s.RemoteEndPoint).Address).DeviceName;
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.cleanedUp)
        return;
      if (disposing)
      {
        IDisposable dataStream = (IDisposable) this.dataStream;
        if (dataStream != null)
          dataStream.Dispose();
        else if (this.Client != null)
        {
          this.Client.Close();
          this.Client = (Socket) null;
        }
      }
      this.cleanedUp = true;
    }

    private void EnsureNotDisposed()
    {
      if (this.cleanedUp || this.clientSocket == null)
        throw new ObjectDisposedException("BluetoothClient");
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    ~SocketBluetoothClient() => this.Dispose(false);

    internal static void ThrowSocketExceptionForHR(int errorCode)
    {
      if (errorCode < 0)
        throw new SocketException(Marshal.GetLastWin32Error());
    }

    internal static void ThrowSocketExceptionForHrExceptFor(
      int errorCode,
      params int[] nonErrorCodes)
    {
      if (errorCode >= 0)
        return;
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (-1 == Array.IndexOf<int>(nonErrorCodes, lastWin32Error, 0, nonErrorCodes.Length))
        throw new SocketException(lastWin32Error);
    }

    internal class DiscoDevsParamsWithLive : DiscoDevsParams
    {
      internal readonly BluetoothClient.LiveDiscoveryCallback _liveDiscoHandler;
      internal readonly object _liveDiscoState;

      public DiscoDevsParamsWithLive(
        int maxDevices,
        bool authenticated,
        bool remembered,
        bool unknown,
        bool discoverableOnly,
        DateTime discoTime,
        BluetoothClient.LiveDiscoveryCallback liveDiscoHandler,
        object liveDiscoState)
        : base(maxDevices, authenticated, remembered, unknown, discoverableOnly, discoTime)
      {
        this._liveDiscoHandler = liveDiscoHandler;
        this._liveDiscoState = liveDiscoState;
      }
    }

    internal class MsftSocketOptionHelper : ISocketOptionHelper
    {
      private readonly Socket m_socket;
      private bool authenticate;
      private BluetoothWin32Authentication m_authenticator;
      private bool encrypt;

      internal MsftSocketOptionHelper(Socket socket) => this.m_socket = socket;

      public bool Authenticate
      {
        get => this.authenticate;
        set
        {
          this.m_socket.SetSocketOption((SocketOptionLevel) 3, (SocketOptionName) -2147483647, value);
          this.authenticate = value;
        }
      }

      public bool Encrypt
      {
        get => this.encrypt;
        set
        {
          this.m_socket.SetSocketOption((SocketOptionLevel) 3, SocketOptionName.AcceptConnection, value ? 1 : 0);
          this.encrypt = value;
        }
      }

      public void SetPin(BluetoothAddress device, string pin)
      {
        if (pin != null)
        {
          this.m_authenticator = new BluetoothWin32Authentication(device, pin);
        }
        else
        {
          if (this.m_authenticator == null)
            return;
          this.m_authenticator.Dispose();
        }
      }
    }
  }
}
