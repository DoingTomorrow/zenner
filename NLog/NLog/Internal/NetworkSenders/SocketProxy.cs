// Decompiled with JetBrains decompiler
// Type: NLog.Internal.NetworkSenders.SocketProxy
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Net.Sockets;

#nullable disable
namespace NLog.Internal.NetworkSenders
{
  internal sealed class SocketProxy : ISocket, IDisposable
  {
    private readonly Socket _socket;

    internal SocketProxy(
      AddressFamily addressFamily,
      SocketType socketType,
      ProtocolType protocolType)
    {
      this._socket = new Socket(addressFamily, socketType, protocolType);
    }

    public Socket UnderlyingSocket => this._socket;

    public void Close() => this._socket.Close();

    public bool ConnectAsync(SocketAsyncEventArgs args) => this._socket.ConnectAsync(args);

    public bool SendAsync(SocketAsyncEventArgs args) => this._socket.SendAsync(args);

    public bool SendToAsync(SocketAsyncEventArgs args) => this._socket.SendToAsync(args);

    public void Dispose() => this._socket.Dispose();
  }
}
