// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsBiffDbCell
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System.Collections.Generic;

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsBiffDbCell : XlsBiffRecord
  {
    internal XlsBiffDbCell(byte[] bytes, uint offset, ExcelBinaryReader reader)
      : base(bytes, offset, reader)
    {
    }

    public int RowAddress => this.Offset - this.ReadInt32(0);

    public uint[] CellAddresses
    {
      get
      {
        int num = this.RowAddress - 20;
        List<uint> uintList = new List<uint>();
        for (int offset = 4; offset < (int) this.RecordSize; offset += 4)
          uintList.Add((uint) num + (uint) this.ReadUInt16(offset));
        return uintList.ToArray();
      }
    }
  }
}
