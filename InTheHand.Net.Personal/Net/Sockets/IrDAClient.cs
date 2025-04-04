// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.IrDAClient
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

#nullable disable
namespace InTheHand.Net.Sockets
{
  public class IrDAClient : IDisposable
  {
    private bool cleanedUp;
    private bool active;
    private Socket clientSocket;
    private NetworkStream dataStream;

    public IrDAClient()
    {
      try
      {
        this.clientSocket = new Socket(AddressFamily.Irda, SocketType.Stream, ProtocolType.IP);
      }
      catch (SocketException ex)
      {
        this.clientSocket = new Socket(AddressFamily.Atm, SocketType.Stream, ProtocolType.IP);
      }
    }

    public IrDAClient(string service)
      : this()
    {
      this.Connect(service);
    }

    public IrDAClient(IrDAEndPoint remoteEP)
      : this()
    {
      this.Connect(remoteEP);
    }

    internal IrDAClient(Socket acceptedSocket)
    {
      this.Client = acceptedSocket;
      this.active = true;
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
      [DebuggerStepThrough] get => this.clientSocket;
      set => this.clientSocket = value;
    }

    public bool Connected => this.clientSocket != null && this.clientSocket.Connected;

    public IrDADeviceInfo[] DiscoverDevices()
    {
      this.EnsureNotDisposed();
      return IrDAClient.DiscoverDevices(8, this.clientSocket);
    }

    public IrDADeviceInfo[] DiscoverDevices(int maxDevices)
    {
      this.EnsureNotDisposed();
      return IrDAClient.DiscoverDevices(maxDevices, this.clientSocket);
    }

    public static IrDADeviceInfo[] DiscoverDevices(int maxDevices, Socket irdaSocket)
    {
      if (irdaSocket == null)
        throw new ArgumentNullException(nameof (irdaSocket));
      return maxDevices <= 67108863 && maxDevices >= 0 ? IrDAClient.ParseDeviceList(irdaSocket.GetSocketOption((SocketOptionLevel) 255, SocketOptionName.DontRoute, 4 + maxDevices * 32)) : throw new ArgumentOutOfRangeException(nameof (maxDevices));
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static IrDADeviceInfo[] ParseDeviceList(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      int length1 = buffer.Length >= 4 ? BitConverter.ToInt32(buffer, 0) : throw new ArgumentException("DEVICE_LIST buffer must be at least four bytes long.");
      IrDADeviceInfo[] deviceList = new IrDADeviceInfo[length1];
      for (int index = 0; index < length1; ++index)
      {
        byte[] numArray = new byte[4];
        Buffer.BlockCopy((Array) buffer, 4 + index * 29, (Array) numArray, 0, 4);
        IrDAAddress id = new IrDAAddress(numArray);
        IrDAHints int16 = (IrDAHints) BitConverter.ToInt16(buffer, 30 + index * 29);
        IrDACharacterSet charset = (IrDACharacterSet) buffer[32 + index * 29];
        Encoding encoding;
        switch (charset)
        {
          case IrDACharacterSet.ASCII:
            encoding = Encoding.ASCII;
            break;
          case IrDACharacterSet.Unicode:
            encoding = Encoding.Unicode;
            break;
          default:
            encoding = Encoding.GetEncoding((int) (28590 + charset));
            break;
        }
        string name = encoding.GetString(buffer, 8 + index * 29, 22);
        int length2 = name.IndexOf(char.MinValue);
        if (length2 > -1)
          name = name.Substring(0, length2);
        deviceList[index] = new IrDADeviceInfo(id, name, int16, charset);
      }
      return deviceList;
    }

    public static string GetRemoteMachineName(Socket irdaSocket)
    {
      if (irdaSocket == null)
        throw new ArgumentNullException(nameof (irdaSocket), "GetRemoteMachineName requires a valid Socket");
      if (!irdaSocket.Connected)
        throw new InvalidOperationException("The socket must be connected to a device to get the remote machine name.");
      IrDAAddress address = ((IrDAEndPoint) irdaSocket.RemoteEndPoint).Address;
      foreach (IrDADeviceInfo discoverDevice in IrDAClient.DiscoverDevices(10, irdaSocket))
      {
        if (address == discoverDevice.DeviceAddress)
          return discoverDevice.DeviceName;
      }
      throw ExceptionFactory.ArgumentOutOfRangeException((string) null, "No matching device discovered.");
    }

    public string RemoteMachineName
    {
      get
      {
        this.EnsureNotDisposed();
        return IrDAClient.GetRemoteMachineName(this.clientSocket);
      }
    }

    public void Connect(IrDAEndPoint remoteEP)
    {
      if (this.cleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (remoteEP == null)
        throw new ArgumentNullException(nameof (remoteEP));
      this.clientSocket.Connect((EndPoint) remoteEP);
      this.active = true;
    }

    public void Connect(string service)
    {
      IrDADeviceInfo[] irDaDeviceInfoArray = this.DiscoverDevices(1);
      if (irDaDeviceInfoArray.Length <= 0)
        throw new InvalidOperationException("No device");
      this.Connect(new IrDAEndPoint(irDaDeviceInfoArray[0].DeviceAddress, service));
    }

    public IAsyncResult BeginConnect(
      IrDAEndPoint remoteEP,
      AsyncCallback requestCallback,
      object state)
    {
      return this.Client.BeginConnect((EndPoint) remoteEP, requestCallback, state);
    }

    public IAsyncResult BeginConnect(string service, AsyncCallback requestCallback, object state)
    {
      IrDADeviceInfo[] irDaDeviceInfoArray = this.DiscoverDevices(1);
      if (irDaDeviceInfoArray.Length > 0)
        return this.BeginConnect(new IrDAEndPoint(irDaDeviceInfoArray[0].DeviceAddress, service), requestCallback, state);
      throw new InvalidOperationException("No remote device");
    }

    public void EndConnect(IAsyncResult asyncResult)
    {
      this.Client.EndConnect(asyncResult);
      this.active = true;
    }

    public void Close() => this.Dispose();

    public NetworkStream GetStream()
    {
      if (this.cleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (!this.Client.Connected)
        throw new InvalidOperationException("The operation is not allowed on non-connected sockets.");
      if (this.dataStream == null)
        this.dataStream = new NetworkStream(this.Client, true);
      return this.dataStream;
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
        else if (this.clientSocket != null)
        {
          this.clientSocket.Close();
          this.clientSocket = (Socket) null;
        }
      }
      this.cleanedUp = true;
    }

    private void EnsureNotDisposed()
    {
      if (this.cleanedUp || this.clientSocket == null)
        throw new ObjectDisposedException(nameof (IrDAClient));
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
