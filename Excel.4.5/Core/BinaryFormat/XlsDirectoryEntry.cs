// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsDirectoryEntry
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using Excel.Exceptions;
using System;
using System.Text;

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsDirectoryEntry
  {
    public const int Length = 128;
    private readonly byte[] m_bytes;
    private XlsDirectoryEntry m_child;
    private XlsDirectoryEntry m_leftSibling;
    private XlsDirectoryEntry m_rightSibling;
    private XlsHeader m_header;

    public XlsDirectoryEntry(byte[] bytes, XlsHeader header)
    {
      this.m_bytes = bytes.Length >= 128 ? bytes : throw new BiffRecordException("Directory Entry error: Array is too small.");
      this.m_header = header;
    }

    public string EntryName
    {
      get
      {
        return Encoding.Unicode.GetString(this.m_bytes, 0, (int) this.EntryLength).TrimEnd(new char[1]);
      }
    }

    public ushort EntryLength => BitConverter.ToUInt16(this.m_bytes, 64);

    public STGTY EntryType => (STGTY) Buffer.GetByte((Array) this.m_bytes, 66);

    public DECOLOR EntryColor => (DECOLOR) Buffer.GetByte((Array) this.m_bytes, 67);

    public uint LeftSiblingSid => BitConverter.ToUInt32(this.m_bytes, 68);

    public XlsDirectoryEntry LeftSibling
    {
      get => this.m_leftSibling;
      set
      {
        if (this.m_leftSibling != null)
          return;
        this.m_leftSibling = value;
      }
    }

    public uint RightSiblingSid => BitConverter.ToUInt32(this.m_bytes, 72);

    public XlsDirectoryEntry RightSibling
    {
      get => this.m_rightSibling;
      set
      {
        if (this.m_rightSibling != null)
          return;
        this.m_rightSibling = value;
      }
    }

    public uint ChildSid => BitConverter.ToUInt32(this.m_bytes, 76);

    public XlsDirectoryEntry Child
    {
      get => this.m_child;
      set
      {
        if (this.m_child != null)
          return;
        this.m_child = value;
      }
    }

    public Guid ClassId
    {
      get
      {
        byte[] numArray = new byte[16];
        Buffer.BlockCopy((Array) this.m_bytes, 80, (Array) numArray, 0, 16);
        return new Guid(numArray);
      }
    }

    public uint UserFlags => BitConverter.ToUInt32(this.m_bytes, 96);

    public DateTime CreationTime => DateTime.FromFileTime(BitConverter.ToInt64(this.m_bytes, 100));

    public DateTime LastWriteTime => DateTime.FromFileTime(BitConverter.ToInt64(this.m_bytes, 108));

    public uint StreamFirstSector => BitConverter.ToUInt32(this.m_bytes, 116);

    public uint StreamSize => BitConverter.ToUInt32(this.m_bytes, 120);

    public bool IsEntryMiniStream => this.StreamSize < this.m_header.MiniStreamCutoff;

    public uint PropType => BitConverter.ToUInt32(this.m_bytes, 124);
  }
}
