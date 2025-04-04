// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Factory.DecoratorNetworkStream
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Bluetooth.Factory
{
  public abstract class DecoratorNetworkStream : NetworkStream
  {
    private readonly Stream m_child;

    protected DecoratorNetworkStream(Stream child)
      : base(DecoratorNetworkStream.GetAConnectedSocket(), false)
    {
      GC.SuppressFinalize((object) this);
      if (!child.CanRead || !child.CanWrite)
        throw new ArgumentException("Child stream must be connected.");
      this.m_child = child;
    }

    internal static Socket GetAConnectedSocket()
    {
      return DecoratorNetworkStream.SocketPair.GetConnectedSocket();
    }

    public abstract override bool DataAvailable { get; }

    public override bool CanRead
    {
      get => this.m_child == null && PlatformVerification.IsMonoRuntime || this.m_child.CanRead;
    }

    public override bool CanSeek => this.m_child.CanSeek;

    public override bool CanWrite
    {
      get => this.m_child == null && PlatformVerification.IsMonoRuntime || this.m_child.CanWrite;
    }

    public override void Flush() => this.m_child.Flush();

    public override long Length => this.m_child.Length;

    public override long Position
    {
      get => this.m_child.Position;
      set => this.m_child.Position = value;
    }

    public override bool CanTimeout => this.m_child.CanTimeout;

    public override int ReadTimeout
    {
      get => this.m_child.ReadTimeout;
      set => this.m_child.ReadTimeout = value;
    }

    public override int WriteTimeout
    {
      get => this.m_child.WriteTimeout;
      set => this.m_child.WriteTimeout = value;
    }

    [DebuggerStepThrough]
    public override int Read(byte[] buffer, int offset, int count)
    {
      return this.m_child.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin) => this.m_child.Seek(offset, origin);

    public override void SetLength(long value) => this.m_child.SetLength(value);

    [DebuggerStepThrough]
    public override void Write(byte[] buffer, int offset, int count)
    {
      this.m_child.Write(buffer, offset, count);
    }

    public override int EndRead(IAsyncResult asyncResult) => this.m_child.EndRead(asyncResult);

    [DebuggerStepThrough]
    public override void EndWrite(IAsyncResult asyncResult) => this.m_child.EndWrite(asyncResult);

    public override IAsyncResult BeginRead(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      return this.m_child.BeginRead(buffer, offset, count, callback, state);
    }

    [DebuggerStepThrough]
    public override IAsyncResult BeginWrite(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      return this.m_child.BeginWrite(buffer, offset, count, callback, state);
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (!disposing)
          return;
        this.m_child.Close();
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    private sealed class SocketPair
    {
      private Socket m_cli;
      private Socket m_svr;
      private static DecoratorNetworkStream.SocketPair m_SocketPair;

      internal static Socket GetConnectedSocket()
      {
        DecoratorNetworkStream.SocketPair socketPair = DecoratorNetworkStream.SocketPair.m_SocketPair;
        if (socketPair == null || !socketPair.Alive)
          DecoratorNetworkStream.SocketPair.m_SocketPair = socketPair = DecoratorNetworkStream.SocketPair.Create();
        return socketPair.m_cli;
      }

      internal static DecoratorNetworkStream.SocketPair Create()
      {
        try
        {
          return DecoratorNetworkStream.SocketPair.Create(AddressFamily.InterNetworkV6);
        }
        catch
        {
          return DecoratorNetworkStream.SocketPair.Create(AddressFamily.InterNetwork);
        }
      }

      internal static DecoratorNetworkStream.SocketPair Create(AddressFamily af)
      {
        return new DecoratorNetworkStream.SocketPair(af);
      }

      private SocketPair(AddressFamily af)
      {
        using (Socket socket = new Socket(af, SocketType.Stream, ProtocolType.IP))
        {
          socket.Bind((EndPoint) new IPEndPoint(af == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Loopback : IPAddress.Loopback, 0));
          socket.Listen(1);
          EndPoint localEndPoint = socket.LocalEndPoint;
          this.m_cli = new Socket(localEndPoint.AddressFamily, socket.SocketType, socket.ProtocolType);
          this.m_cli.Connect(localEndPoint);
          this.m_svr = socket.Accept();
        }
      }

      private bool Alive => this.m_cli != null && this.m_cli.Connected;
    }
  }
}
