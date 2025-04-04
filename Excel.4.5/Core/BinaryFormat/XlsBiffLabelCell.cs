// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsBiffLabelCell
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System.Text;

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsBiffLabelCell : XlsBiffBlankCell
  {
    private Encoding m_UseEncoding = Encoding.Default;

    internal XlsBiffLabelCell(byte[] bytes, uint offset, ExcelBinaryReader reader)
      : base(bytes, offset, reader)
    {
    }

    public Encoding UseEncoding
    {
      get => this.m_UseEncoding;
      set => this.m_UseEncoding = value;
    }

    public ushort Length => this.ReadUInt16(6);

    public string Value
    {
      get
      {
        byte[] bytes = !this.reader.isV8() ? this.ReadArray(2, (int) this.Length * (Helpers.IsSingleByteEncoding(this.m_UseEncoding) ? 1 : 2)) : this.ReadArray(9, (int) this.Length * (Helpers.IsSingleByteEncoding(this.m_UseEncoding) ? 1 : 2));
        return this.m_UseEncoding.GetString(bytes, 0, bytes.Length);
      }
    }
  }
}
