// Decompiled with JetBrains decompiler
// Type: Standard.ComStream
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable disable
namespace Standard
{
  internal sealed class ComStream : Stream
  {
    private const int STATFLAG_NONAME = 1;
    private IStream _source;

    private void _Validate()
    {
      if (this._source == null)
        throw new ObjectDisposedException("this");
    }

    public ComStream(ref IStream stream)
    {
      Verify.IsNotNull<IStream>(stream, nameof (stream));
      this._source = stream;
      stream = (IStream) null;
    }

    public override void Close()
    {
      if (this._source == null)
        return;
      Utility.SafeRelease<IStream>(ref this._source);
    }

    public override bool CanRead => true;

    public override bool CanSeek => true;

    public override bool CanWrite => false;

    public override void Flush()
    {
    }

    public override long Length
    {
      get
      {
        this._Validate();
        System.Runtime.InteropServices.ComTypes.STATSTG pstatstg;
        this._source.Stat(out pstatstg, 1);
        return pstatstg.cbSize;
      }
    }

    public override long Position
    {
      get => this.Seek(0L, SeekOrigin.Current);
      set => this.Seek(value, SeekOrigin.Begin);
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      this._Validate();
      IntPtr hglobal = IntPtr.Zero;
      try
      {
        hglobal = Marshal.AllocHGlobal(4);
        byte[] numArray = new byte[count];
        this._source.Read(numArray, count, hglobal);
        Array.Copy((Array) numArray, 0, (Array) buffer, offset, Marshal.ReadInt32(hglobal));
        return Marshal.ReadInt32(hglobal);
      }
      finally
      {
        Utility.SafeFreeHGlobal(ref hglobal);
      }
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      this._Validate();
      IntPtr hglobal = IntPtr.Zero;
      try
      {
        hglobal = Marshal.AllocHGlobal(8);
        this._source.Seek(offset, (int) origin, hglobal);
        return Marshal.ReadInt64(hglobal);
      }
      finally
      {
        Utility.SafeFreeHGlobal(ref hglobal);
      }
    }

    public override void SetLength(long value) => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count)
    {
      throw new NotSupportedException();
    }
  }
}
