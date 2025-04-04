// Decompiled with JetBrains decompiler
// Type: NLog.Internal.NetworkSenders.UdpNetworkSender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

#nullable disable
namespace NLog.Internal.NetworkSenders
{
  internal class UdpNetworkSender : NetworkSender
  {
    private ISocket _socket;
    private EndPoint _endpoint;

    public UdpNetworkSender(string url, AddressFamily addressFamily)
      : base(url)
    {
      this.AddressFamily = addressFamily;
    }

    internal AddressFamily AddressFamily { get; set; }

    protected internal virtual ISocket CreateSocket(
      AddressFamily addressFamily,
      SocketType socketType,
      ProtocolType protocolType)
    {
      SocketProxy socket = new SocketProxy(addressFamily, socketType, protocolType);
      Uri result;
      if (Uri.TryCreate(this.Address, UriKind.Absolute, out result) && result.Host.Equals(IPAddress.Broadcast.ToString(), StringComparison.OrdinalIgnoreCase))
        socket.UnderlyingSocket.EnableBroadcast = true;
      return (ISocket) socket;
    }

    protected override void DoInitialize()
    {
      this._endpoint = this.ParseEndpointAddress(new Uri(this.Address), this.AddressFamily);
      this._socket = this.CreateSocket(this._endpoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
    }

    protected override void DoClose(AsyncContinuation continuation)
    {
      lock (this)
        this.CloseSocket(continuation);
    }

    private void CloseSocket(AsyncContinuation continuation)
    {
      try
      {
        ISocket socket = this._socket;
        this._socket = (ISocket) null;
        socket?.Close();
        continuation((Exception) null);
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrown())
          throw;
        else
          continuation(ex);
      }
    }

    protected override void DoSend(
      byte[] bytes,
      int offset,
      int length,
      AsyncContinuation asyncContinuation)
    {
      lock (this)
      {
        SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
        socketAsyncEventArgs.SetBuffer(bytes, offset, length);
        socketAsyncEventArgs.UserToken = (object) asyncContinuation;
        socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.SocketOperationCompleted);
        socketAsyncEventArgs.RemoteEndPoint = this._endpoint;
        if (this._socket.SendToAsync(socketAsyncEventArgs))
          return;
        this.SocketOperationCompleted((object) this._socket, socketAsyncEventArgs);
      }
    }

    private void SocketOperationCompleted(object sender, SocketAsyncEventArgs e)
    {
      AsyncContinuation userToken = e.UserToken as AsyncContinuation;
      Exception exception = (Exception) null;
      if (e.SocketError != SocketError.Success)
        exception = (Exception) new IOException("Error: " + (object) e.SocketError);
      e.Dispose();
      if (userToken == null)
        return;
      userToken(exception);
    }

    public override void CheckSocket()
    {
      if (this._socket != null)
        return;
      this.DoInitialize();
    }
  }
}
