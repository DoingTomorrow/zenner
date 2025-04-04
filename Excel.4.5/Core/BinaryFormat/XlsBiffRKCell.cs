// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsBiffRKCell
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsBiffRKCell : XlsBiffBlankCell
  {
    internal XlsBiffRKCell(byte[] bytes, uint offset, ExcelBinaryReader reader)
      : base(bytes, offset, reader)
    {
    }

    public double Value => XlsBiffRKCell.NumFromRK(this.ReadUInt32(6));

    public static double NumFromRK(uint rk)
    {
      double num = ((int) rk & 2) != 2 ? Helpers.Int64BitsToDouble((long) (rk & 4294967292U) << 32) : (double) ((int) (rk >> 2) | (((int) rk & int.MinValue) == 0 ? 0 : -1073741824));
      if (((int) rk & 1) == 1)
        num /= 100.0;
      return num;
    }
  }
}
