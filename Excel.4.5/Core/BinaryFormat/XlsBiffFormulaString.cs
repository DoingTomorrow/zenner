// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsBiffFormulaString
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System.Text;

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsBiffFormulaString : XlsBiffRecord
  {
    private const int LEADING_BYTES_COUNT = 3;
    private Encoding m_UseEncoding = Encoding.Default;

    internal XlsBiffFormulaString(byte[] bytes, uint offset, ExcelBinaryReader reader)
      : base(bytes, offset, reader)
    {
    }

    public Encoding UseEncoding
    {
      get => this.m_UseEncoding;
      set => this.m_UseEncoding = value;
    }

    public ushort Length => this.ReadUInt16(0);

    public string Value
    {
      get
      {
        return this.ReadUInt16(1) != (ushort) 0 ? Encoding.Unicode.GetString(this.m_bytes, this.m_readoffset + 3, (int) this.Length * 2) : this.m_UseEncoding.GetString(this.m_bytes, this.m_readoffset + 3, (int) this.Length);
      }
    }
  }
}
