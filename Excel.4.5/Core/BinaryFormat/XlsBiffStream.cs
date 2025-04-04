// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsBiffStream
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System;
using System.IO;
using System.Runtime.CompilerServices;

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsBiffStream : XlsStream
  {
    private readonly ExcelBinaryReader reader;
    private readonly byte[] bytes;
    private readonly int m_size;
    private int m_offset;

    public XlsBiffStream(
      XlsHeader hdr,
      uint streamStart,
      bool isMini,
      XlsRootDirectory rootDir,
      ExcelBinaryReader reader)
      : base(hdr, streamStart, isMini, rootDir)
    {
      this.reader = reader;
      this.bytes = base.ReadStream();
      this.m_size = this.bytes.Length;
      this.m_offset = 0;
    }

    public int Size => this.m_size;

    public int Position => this.m_offset;

    [Obsolete("Use BIFF-specific methods for this stream")]
    public new byte[] ReadStream() => this.bytes;

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Seek(int offset, SeekOrigin origin)
    {
      switch (origin)
      {
        case SeekOrigin.Begin:
          this.m_offset = offset;
          break;
        case SeekOrigin.Current:
          this.m_offset += offset;
          break;
        case SeekOrigin.End:
          this.m_offset = this.m_size - offset;
          break;
      }
      if (this.m_offset < 0)
        throw new ArgumentOutOfRangeException(string.Format("{0} On offset={1}", (object) "BIFF Stream error: Moving before stream start.", (object) offset));
      if (this.m_offset > this.m_size)
        throw new ArgumentOutOfRangeException(string.Format("{0} On offset={1}", (object) "BIFF Stream error: Moving after stream end.", (object) offset));
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public XlsBiffRecord Read()
    {
      if ((long) (uint) this.m_offset >= (long) this.bytes.Length)
        return (XlsBiffRecord) null;
      XlsBiffRecord record = XlsBiffRecord.GetRecord(this.bytes, (uint) this.m_offset, this.reader);
      this.m_offset += record.Size;
      return this.m_offset > this.m_size ? (XlsBiffRecord) null : record;
    }

    public XlsBiffRecord ReadAt(int offset)
    {
      if ((long) (uint) offset >= (long) this.bytes.Length)
        return (XlsBiffRecord) null;
      XlsBiffRecord record = XlsBiffRecord.GetRecord(this.bytes, (uint) offset, this.reader);
      return this.reader.ReadOption == ReadOption.Strict && this.m_offset + record.Size > this.m_size ? (XlsBiffRecord) null : record;
    }
  }
}
