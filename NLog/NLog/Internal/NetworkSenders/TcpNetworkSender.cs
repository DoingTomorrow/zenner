// Decompiled with JetBrains decompiler
// Type: NLog.Internal.NetworkSenders.TcpNetworkSender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

#nullable disable
namespace NLog.Internal.NetworkSenders
{
  internal class TcpNetworkSender : NetworkSender
  {
    private readonly Queue<SocketAsyncEventArgs> _pendingRequests = new Queue<SocketAsyncEventArgs>();
    private ISocket _socket;
    private Exception _pendingError;
    private bool _asyncOperationInProgress;
    private AsyncContinuation _closeContinuation;
    private AsyncContinuation _flushContinuation;

    public TcpNetworkSender(string url, AddressFamily addressFamily)
      : base(url)
    {
      this.AddressFamily = addressFamily;
    }

    internal AddressFamily AddressFamily { get; set; }

    internal int MaxQueueSize { get; set; }

    protected internal virtual ISocket CreateSocket(
      AddressFamily addressFamily,
      SocketType socketType,
      ProtocolType protocolType)
    {
      return (ISocket) new SocketProxy(addressFamily, socketType, protocolType);
    }

    protected override void DoInitialize()
    {
      TcpNetworkSender.MySocketAsyncEventArgs socketAsyncEventArgs = new TcpNetworkSender.MySocketAsyncEventArgs();
      socketAsyncEventArgs.RemoteEndPoint = this.ParseEndpointAddress(new Uri(this.Address), this.AddressFamily);
      socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.SocketOperationCompleted);
      socketAsyncEventArgs.UserToken = (object) null;
      this._socket = this.CreateSocket(socketAsyncEventArgs.RemoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
      this._asyncOperationInProgress = true;
      if (this._socket.ConnectAsync((SocketAsyncEventArgs) socketAsyncEventArgs))
        return;
      this.SocketOperationCompleted((object) this._socket, (SocketAsyncEventArgs) socketAsyncEventArgs);
    }

    protected override void DoClose(AsyncContinuation continuation)
    {
      lock (this)
      {
        if (this._asyncOperationInProgress)
          this._closeContinuation = continuation;
        else
          this.CloseSocket(continuation);
      }
    }

    protected override void DoFlush(AsyncContinuation continuation)
    {
      lock (this)
      {
        if (!this._asyncOperationInProgress && this._pendingRequests.Count == 0)
          continuation((Exception) null);
        else
          this._flushContinuation = continuation;
      }
    }

    protected override void DoSend(
      byte[] bytes,
      int offset,
      int length,
      AsyncContinuation asyncContinuation)
    {
      TcpNetworkSender.MySocketAsyncEventArgs socketAsyncEventArgs = new TcpNetworkSender.MySocketAsyncEventArgs();
      socketAsyncEventArgs.SetBuffer(bytes, offset, length);
      socketAsyncEventArgs.UserToken = (object) asyncContinuation;
      socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.SocketOperationCompleted);
      lock (this)
      {
        if (this.MaxQueueSize != 0 && this._pendingRequests.Count >= this.MaxQueueSize)
          this._pendingRequests.Dequeue()?.Dispose();
        this._pendingRequests.Enqueue((SocketAsyncEventArgs) socketAsyncEventArgs);
      }
      this.ProcessNextQueuedItem();
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

    private void SocketOperationCompleted(object sender, SocketAsyncEventArgs e)
    {
      lock (this)
      {
        this._asyncOperationInProgress = false;
        AsyncContinuation userToken = e.UserToken as AsyncContinuation;
        if (e.SocketError != SocketError.Success)
          this._pendingError = (Exception) new IOException("Error: " + (object) e.SocketError);
        e.Dispose();
        if (userToken != null)
          userToken(this._pendingError);
      }
      this.ProcessNextQueuedItem();
    }

    private void ProcessNextQueuedItem()
    {
      lock (this)
      {
        if (this._asyncOperationInProgress)
          return;
        if (this._pendingError != null)
        {
          while (this._pendingRequests.Count != 0)
          {
            SocketAsyncEventArgs socketAsyncEventArgs = this._pendingRequests.Dequeue();
            AsyncContinuation userToken = (AsyncContinuation) socketAsyncEventArgs.UserToken;
            socketAsyncEventArgs.Dispose();
            Exception pendingError = this._pendingError;
            userToken(pendingError);
          }
        }
        if (this._pendingRequests.Count == 0)
        {
          AsyncContinuation flushContinuation = this._flushContinuation;
          if (flushContinuation != null)
          {
            this._flushContinuation = (AsyncContinuation) null;
            flushContinuation(this._pendingError);
          }
          AsyncContinuation closeContinuation = this._closeContinuation;
          if (closeContinuation == null)
            return;
          this._closeContinuation = (AsyncContinuation) null;
          this.CloseSocket(closeContinuation);
        }
        else
        {
          SocketAsyncEventArgs socketAsyncEventArgs = this._pendingRequests.Dequeue();
          this._asyncOperationInProgress = true;
          if (this._socket.SendAsync(socketAsyncEventArgs))
            return;
          this.SocketOperationCompleted((object) this._socket, socketAsyncEventArgs);
        }
      }
    }

    public override void CheckSocket()
    {
      if (this._socket != null)
        return;
      this.DoInitialize();
    }

    internal class MySocketAsyncEventArgs : SocketAsyncEventArgs
    {
      public void RaiseCompleted() => this.OnCompleted((SocketAsyncEventArgs) this);
    }
  }
}
