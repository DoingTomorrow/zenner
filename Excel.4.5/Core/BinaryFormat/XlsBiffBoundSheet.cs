// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsBiffBoundSheet
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System.Text;

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsBiffBoundSheet : XlsBiffRecord
  {
    private bool isV8 = true;
    private Encoding m_UseEncoding = Encoding.Default;

    internal XlsBiffBoundSheet(byte[] bytes, uint offset, ExcelBinaryReader reader)
      : base(bytes, offset, reader)
    {
    }

    public uint StartOffset => this.ReadUInt32(0);

    public XlsBiffBoundSheet.SheetType Type => (XlsBiffBoundSheet.SheetType) this.ReadByte(4);

    public XlsBiffBoundSheet.SheetVisibility VisibleState
    {
      get => (XlsBiffBoundSheet.SheetVisibility) ((uint) this.ReadByte(5) & 3U);
    }

    public string SheetName
    {
      get
      {
        ushort count = (ushort) this.ReadByte(6);
        int num = 8;
        if (!this.isV8)
          return Encoding.Default.GetString(this.m_bytes, this.m_readoffset + num - 1, (int) count);
        return this.ReadByte(7) == (byte) 0 ? Encoding.Default.GetString(this.m_bytes, this.m_readoffset + num, (int) count) : this.m_UseEncoding.GetString(this.m_bytes, this.m_readoffset + num, Helpers.IsSingleByteEncoding(this.m_UseEncoding) ? (int) count : (int) count * 2);
      }
    }

    public Encoding UseEncoding
    {
      get => this.m_UseEncoding;
      set => this.m_UseEncoding = value;
    }

    public bool IsV8
    {
      get => this.isV8;
      set => this.isV8 = value;
    }

    public enum SheetType : byte
    {
      Worksheet = 0,
      MacroSheet = 1,
      Chart = 2,
      VBModule = 6,
    }

    public enum SheetVisibility : byte
    {
      Visible,
      Hidden,
      VeryHidden,
    }
  }
}
