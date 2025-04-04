// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsBiffIndex
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System.Collections.Generic;

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsBiffIndex : XlsBiffRecord
  {
    private bool isV8 = true;

    internal XlsBiffIndex(byte[] bytes, uint offset, ExcelBinaryReader reader)
      : base(bytes, offset, reader)
    {
    }

    public bool IsV8
    {
      get => this.isV8;
      set => this.isV8 = value;
    }

    public uint FirstExistingRow => !this.isV8 ? (uint) this.ReadUInt16(4) : this.ReadUInt32(4);

    public uint LastExistingRow => !this.isV8 ? (uint) this.ReadUInt16(6) : this.ReadUInt32(8);

    public uint[] DbCellAddresses
    {
      get
      {
        int recordSize = (int) this.RecordSize;
        int num = this.isV8 ? 16 : 12;
        if (recordSize <= num)
          return new uint[0];
        List<uint> uintList = new List<uint>((recordSize - num) / 4);
        for (int offset = num; offset < recordSize; offset += 4)
          uintList.Add(this.ReadUInt32(offset));
        return uintList.ToArray();
      }
    }
  }
}
