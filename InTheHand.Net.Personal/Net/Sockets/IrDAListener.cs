// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.IrDAListener
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Net;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Sockets
{
  public class IrDAListener
  {
    private IrDAEndPoint serverEP;
    private Socket serverSocket;
    private bool active;

    public IrDAListener(IrDAEndPoint ep)
    {
      this.initialize();
      this.serverEP = ep;
    }

    public IrDAListener(string service)
    {
      this.initialize();
      this.serverEP = new IrDAEndPoint(IrDAAddress.None, service);
    }

    private void initialize()
    {
      this.active = false;
      try
      {
        this.serverSocket = new Socket(AddressFamily.Irda, SocketType.Stream, ProtocolType.IP);
      }
      catch (SocketException ex)
      {
        this.serverSocket = new Socket(AddressFamily.Atm, SocketType.Stream, ProtocolType.IP);
      }
    }

    public Socket Server => this.serverSocket;

    public bool Active => this.active;

    public IrDAEndPoint LocalEndpoint
    {
      get
      {
        return this.serverSocket != null ? (IrDAEndPoint) this.serverSocket.LocalEndPoint : this.serverEP;
      }
    }

    public void Start() => this.Start(int.MaxValue);

    public void Start(int backlog)
    {
      if (backlog > int.MaxValue || backlog < 0)
        throw new ArgumentOutOfRangeException(nameof (backlog));
      if (this.serverSocket == null)
        throw new InvalidOperationException("The socket handle is not valid.");
      if (this.active)
        return;
      this.serverSocket.Bind((EndPoint) this.serverEP);
      this.serverSocket.Listen(backlog);
      this.active = true;
    }

    public void Stop()
    {
      if (this.serverSocket != null)
      {
        this.serverSocket.Close();
        this.serverSocket = (Socket) null;
      }
      this.initialize();
    }

    public Socket AcceptSocket()
    {
      if (!this.active)
        throw new InvalidOperationException("Not listening. You must call the Start() method before calling this method.");
      return this.serverSocket.Accept();
    }

    public IrDAClient AcceptIrDAClient() => new IrDAClient(this.AcceptSocket());

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

    public IAsyncResult BeginAcceptIrDAClient(AsyncCallback callback, object state)
    {
      if (!this.active)
        throw new InvalidOperationException("Not listening. You must call the Start() method before calling this method.");
      return this.serverSocket.BeginAccept(callback, state);
    }

    public IrDAClient EndAcceptIrDAClient(IAsyncResult asyncResult)
    {
      return asyncResult != null ? new IrDAClient(this.serverSocket.EndAccept(asyncResult)) : throw new ArgumentNullException(nameof (asyncResult));
    }

    public bool Pending()
    {
      if (!this.active)
        throw new InvalidOperationException("Not listening. You must call the Start() method before calling this method.");
      return this.serverSocket.Poll(0, SelectMode.SelectRead);
    }
  }
}
