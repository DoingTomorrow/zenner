// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Factory.TimeoutDecorStream
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.IO;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Bluetooth.Factory
{
  internal class TimeoutDecorStream : Stream
  {
    private readonly Stream _child;

    public TimeoutDecorStream(Stream child) => this._child = child;

    public override bool CanRead => this._child.CanRead;

    public override bool CanSeek => false;

    public override bool CanWrite => this._child.CanWrite;

    public override void Flush() => this._child.Flush();

    public override long Length => throw new NotSupportedException();

    public override long Position
    {
      get => throw new NotSupportedException();
      set => throw new NotSupportedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      IAsyncResult asyncResult = this._child.BeginRead(buffer, offset, count, (AsyncCallback) null, (object) null);
      this.DoTimeoutIfAndClose(asyncResult, this.ReadTimeout);
      return this._child.EndRead(asyncResult);
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count)
    {
      IAsyncResult asyncResult = this._child.BeginWrite(buffer, offset, count, (AsyncCallback) null, (object) null);
      this.DoTimeoutIfAndClose(asyncResult, this.WriteTimeout);
      this._child.EndWrite(asyncResult);
    }

    public override int ReadTimeout { get; set; }

    public override int WriteTimeout { get; set; }

    private void DoTimeoutIfAndClose(IAsyncResult ar, int timeout)
    {
      if (CommonRfcommStream.IsInfiniteTimeout(timeout))
        return;
      if (ar.AsyncWaitHandle.WaitOne(timeout, false))
        return;
      try
      {
        SocketException innerException = new SocketException(10060);
        throw new IOException(innerException.Message, (Exception) innerException);
      }
      finally
      {
        this.Close();
      }
    }
  }
}
