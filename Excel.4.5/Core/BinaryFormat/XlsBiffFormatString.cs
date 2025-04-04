// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsBiffFormatString
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System.Text;

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsBiffFormatString : XlsBiffRecord
  {
    private Encoding m_UseEncoding = Encoding.Default;
    private string m_value;

    internal XlsBiffFormatString(byte[] bytes, uint offset, ExcelBinaryReader reader)
      : base(bytes, offset, reader)
    {
    }

    public Encoding UseEncoding
    {
      get => this.m_UseEncoding;
      set => this.m_UseEncoding = value;
    }

    public ushort Length
    {
      get => this.ID == BIFFRECORDTYPE.FORMAT_V23 ? (ushort) this.ReadByte(0) : this.ReadUInt16(2);
    }

    public string Value
    {
      get
      {
        if (this.m_value == null)
        {
          switch (this.ID)
          {
            case BIFFRECORDTYPE.FORMAT_V23:
              this.m_value = this.m_UseEncoding.GetString(this.m_bytes, this.m_readoffset + 1, (int) this.Length);
              break;
            case BIFFRECORDTYPE.FORMAT:
              int index = this.m_readoffset + 5;
              byte num = this.ReadByte(3);
              this.m_UseEncoding = ((int) num & 1) == 1 ? Encoding.Unicode : Encoding.Default;
              if (((int) num & 4) == 1)
                index += 4;
              if (((int) num & 8) == 1)
                index += 2;
              this.m_value = this.m_UseEncoding.IsSingleByte ? this.m_UseEncoding.GetString(this.m_bytes, index, (int) this.Length) : this.m_UseEncoding.GetString(this.m_bytes, index, (int) this.Length * 2);
              break;
          }
        }
        return this.m_value;
      }
    }

    public ushort Index => this.ID == BIFFRECORDTYPE.FORMAT_V23 ? (ushort) 0 : this.ReadUInt16(0);
  }
}
