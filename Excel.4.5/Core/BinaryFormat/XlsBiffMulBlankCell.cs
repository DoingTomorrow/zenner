// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsBiffMulBlankCell
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsBiffMulBlankCell : XlsBiffBlankCell
  {
    internal XlsBiffMulBlankCell(byte[] bytes, uint offset, ExcelBinaryReader reader)
      : base(bytes, offset, reader)
    {
    }

    public ushort LastColumnIndex => this.ReadUInt16((int) this.RecordSize - 2);

    public ushort GetXF(ushort ColumnIdx)
    {
      int offset = 4 + 6 * ((int) ColumnIdx - (int) this.ColumnIndex);
      return offset > (int) this.RecordSize - 2 ? (ushort) 0 : this.ReadUInt16(offset);
    }
  }
}
