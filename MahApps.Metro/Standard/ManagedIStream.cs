﻿// Decompiled with JetBrains decompiler
// Type: Standard.ManagedIStream
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
  internal sealed class ManagedIStream : IStream, IDisposable
  {
    private const int STGTY_STREAM = 2;
    private const int STGM_READWRITE = 2;
    private const int LOCK_EXCLUSIVE = 2;
    private Stream _source;

    public ManagedIStream(Stream source)
    {
      Verify.IsNotNull<Stream>(source, nameof (source));
      this._source = source;
    }

    private void _Validate()
    {
      if (this._source == null)
        throw new ObjectDisposedException("this");
    }

    [Obsolete("The method is not implemented", true)]
    public void Clone(out IStream ppstm)
    {
      ppstm = (IStream) null;
      HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
    }

    public void Commit(int grfCommitFlags)
    {
      this._Validate();
      this._source.Flush();
    }

    public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
    {
      Verify.IsNotNull<IStream>(pstm, nameof (pstm));
      this._Validate();
      byte[] numArray = new byte[4096];
      long val;
      int cb1;
      for (val = 0L; val < cb; val += (long) cb1)
      {
        cb1 = this._source.Read(numArray, 0, numArray.Length);
        if (cb1 != 0)
          pstm.Write(numArray, cb1, IntPtr.Zero);
        else
          break;
      }
      if (IntPtr.Zero != pcbRead)
        Marshal.WriteInt64(pcbRead, val);
      if (!(IntPtr.Zero != pcbWritten))
        return;
      Marshal.WriteInt64(pcbWritten, val);
    }

    [Obsolete("The method is not implemented", true)]
    public void LockRegion(long libOffset, long cb, int dwLockType)
    {
      HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
    }

    public void Read(byte[] pv, int cb, IntPtr pcbRead)
    {
      this._Validate();
      int val = this._source.Read(pv, 0, cb);
      if (!(IntPtr.Zero != pcbRead))
        return;
      Marshal.WriteInt32(pcbRead, val);
    }

    [Obsolete("The method is not implemented", true)]
    public void Revert()
    {
      HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
    }

    public void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
    {
      this._Validate();
      long val = this._source.Seek(dlibMove, (SeekOrigin) dwOrigin);
      if (!(IntPtr.Zero != plibNewPosition))
        return;
      Marshal.WriteInt64(plibNewPosition, val);
    }

    public void SetSize(long libNewSize)
    {
      this._Validate();
      this._source.SetLength(libNewSize);
    }

    public void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag)
    {
      pstatstg = new System.Runtime.InteropServices.ComTypes.STATSTG();
      this._Validate();
      pstatstg.type = 2;
      pstatstg.cbSize = this._source.Length;
      pstatstg.grfMode = 2;
      pstatstg.grfLocksSupported = 2;
    }

    [Obsolete("The method is not implemented", true)]
    public void UnlockRegion(long libOffset, long cb, int dwLockType)
    {
      HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
    }

    public void Write(byte[] pv, int cb, IntPtr pcbWritten)
    {
      this._Validate();
      this._source.Write(pv, 0, cb);
      if (!(IntPtr.Zero != pcbWritten))
        return;
      Marshal.WriteInt32(pcbWritten, cb);
    }

    public void Dispose() => this._source = (Stream) null;
  }
}
