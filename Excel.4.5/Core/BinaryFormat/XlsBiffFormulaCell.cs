// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsBiffFormulaCell
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System.Text;

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsBiffFormulaCell : XlsBiffNumberCell
  {
    private Encoding m_UseEncoding = Encoding.Default;

    internal XlsBiffFormulaCell(byte[] bytes, uint offset, ExcelBinaryReader reader)
      : base(bytes, offset, reader)
    {
    }

    public Encoding UseEncoding
    {
      get => this.m_UseEncoding;
      set => this.m_UseEncoding = value;
    }

    public XlsBiffFormulaCell.FormulaFlags Flags
    {
      get => (XlsBiffFormulaCell.FormulaFlags) this.ReadUInt16(14);
    }

    public byte FormulaLength => this.ReadByte(15);

    public object Value
    {
      get
      {
        long num1 = this.ReadInt64(6);
        if ((num1 & -281474976710656L) != -281474976710656L)
          return (object) Helpers.Int64BitsToDouble(num1);
        byte num2 = (byte) ((ulong) num1 & (ulong) byte.MaxValue);
        byte num3 = (byte) ((ulong) (num1 >> 16) & (ulong) byte.MaxValue);
        switch (num2)
        {
          case 0:
            XlsBiffRecord record = XlsBiffRecord.GetRecord(this.m_bytes, (uint) (this.Offset + this.Size), this.reader);
            XlsBiffFormulaString biffFormulaString = record.ID != BIFFRECORDTYPE.SHRFMLA ? record as XlsBiffFormulaString : XlsBiffRecord.GetRecord(this.m_bytes, (uint) (this.Offset + this.Size + record.Size), this.reader) as XlsBiffFormulaString;
            if (biffFormulaString == null)
              return (object) string.Empty;
            biffFormulaString.UseEncoding = this.m_UseEncoding;
            return (object) biffFormulaString.Value;
          case 1:
            return (object) (num3 != (byte) 0);
          case 2:
            return (object) (FORMULAERROR) num3;
          default:
            return (object) null;
        }
      }
    }

    public string Formula
    {
      get
      {
        byte[] bytes = this.ReadArray(16, (int) this.FormulaLength);
        return Encoding.Default.GetString(bytes, 0, bytes.Length);
      }
    }

    [System.Flags]
    public enum FormulaFlags : ushort
    {
      AlwaysCalc = 1,
      CalcOnLoad = 2,
      SharedFormulaGroup = 8,
    }
  }
}
