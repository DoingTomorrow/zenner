// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsBiffDimensions
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System;

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsBiffDimensions : XlsBiffRecord
  {
    private bool isV8 = true;

    internal XlsBiffDimensions(byte[] bytes, uint offset, ExcelBinaryReader reader)
      : base(bytes, offset, reader)
    {
    }

    public bool IsV8
    {
      get => this.isV8;
      set => this.isV8 = value;
    }

    public uint FirstRow => !this.isV8 ? (uint) this.ReadUInt16(0) : this.ReadUInt32(0);

    public uint LastRow => !this.isV8 ? (uint) this.ReadUInt16(2) : this.ReadUInt32(4);

    public ushort FirstColumn => !this.isV8 ? this.ReadUInt16(4) : this.ReadUInt16(8);

    public ushort LastColumn
    {
      get => !this.isV8 ? this.ReadUInt16(6) : (ushort) (((int) this.ReadUInt16(9) >> 8) + 1);
      set => throw new NotImplementedException();
    }
  }
}
