// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.SocketAdapter
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Net;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Sockets
{
  internal abstract class SocketAdapter : IDisposable
  {
    public abstract EndPoint LocalEndPoint { get; }

    public abstract EndPoint RemoteEndPoint { get; }

    public abstract int Available { get; }

    public abstract int Receive(byte[] buffer, int size, SocketFlags socketFlags);

    public abstract int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags);

    public abstract int Send(byte[] buffer);

    public abstract void Close();

    protected static void CheckSocketFlags(SocketFlags socketFlags)
    {
      if (socketFlags != SocketFlags.None)
        throw new ArgumentException("Only SocketFlags.None is supported.");
    }

    public void Dispose() => this.Dispose(true);

    protected abstract void Dispose(bool disposing);
  }
}
