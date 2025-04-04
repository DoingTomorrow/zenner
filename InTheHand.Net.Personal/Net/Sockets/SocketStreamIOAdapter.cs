// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.SocketStreamIOAdapter
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.IO;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Sockets
{
  internal abstract class SocketStreamIOAdapter : SocketAdapter
  {
    private readonly Stream m_strm;

    protected SocketStreamIOAdapter(Stream strm)
    {
      if (strm == null)
        throw new ArgumentNullException(nameof (strm));
      if (!strm.CanRead || !strm.CanWrite)
        throw new ArgumentException("Stream is closed.");
      this.m_strm = strm;
    }

    public override int Receive(byte[] buffer, int size, SocketFlags socketFlags)
    {
      return this.Receive(buffer, 0, size, socketFlags);
    }

    public override int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
    {
      SocketAdapter.CheckSocketFlags(socketFlags);
      return this.m_strm.Read(buffer, offset, size);
    }

    public override int Send(byte[] buffer)
    {
      this.m_strm.Write(buffer, 0, buffer.Length);
      return buffer.Length;
    }

    public override void Close() => this.Dispose();

    protected override void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.m_strm.Close();
    }
  }
}
