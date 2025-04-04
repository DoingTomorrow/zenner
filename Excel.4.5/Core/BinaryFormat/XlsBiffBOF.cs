// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsBiffBOF
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsBiffBOF : XlsBiffRecord
  {
    internal XlsBiffBOF(byte[] bytes, uint offset, ExcelBinaryReader reader)
      : base(bytes, offset, reader)
    {
    }

    public ushort Version => this.ReadUInt16(0);

    public BIFFTYPE Type => (BIFFTYPE) this.ReadUInt16(2);

    public ushort CreationID => this.RecordSize < (ushort) 6 ? (ushort) 0 : this.ReadUInt16(4);

    public ushort CreationYear => this.RecordSize < (ushort) 8 ? (ushort) 0 : this.ReadUInt16(6);

    public uint HistoryFlag => this.RecordSize < (ushort) 12 ? 0U : this.ReadUInt32(8);

    public uint MinVersionToOpen => this.RecordSize < (ushort) 16 ? 0U : this.ReadUInt32(12);
  }
}
