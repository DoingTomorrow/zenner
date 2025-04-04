// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsBiffRecord
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System;

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsBiffRecord
  {
    protected byte[] m_bytes;
    protected readonly ExcelBinaryReader reader;
    protected int m_readoffset;

    protected XlsBiffRecord(byte[] bytes, uint offset, ExcelBinaryReader reader)
    {
      if ((long) bytes.Length - (long) offset < 4L)
        throw new ArgumentException("Error: Buffer size is less than minimum BIFF record size.");
      this.m_bytes = bytes;
      this.reader = reader;
      this.m_readoffset = 4 + (int) offset;
      if (reader.ReadOption == ReadOption.Strict && (long) bytes.Length < (long) offset + (long) this.Size)
        throw new ArgumentException("BIFF Stream error: Buffer size is less than entry length.");
    }

    internal byte[] Bytes => this.m_bytes;

    internal int Offset => this.m_readoffset - 4;

    public BIFFRECORDTYPE ID
    {
      get => (BIFFRECORDTYPE) BitConverter.ToUInt16(this.m_bytes, this.m_readoffset - 4);
    }

    public ushort RecordSize => BitConverter.ToUInt16(this.m_bytes, this.m_readoffset - 2);

    public int Size => 4 + (int) this.RecordSize;

    public static XlsBiffRecord GetRecord(byte[] bytes, uint offset, ExcelBinaryReader reader)
    {
      if ((long) offset >= (long) bytes.Length)
        return (XlsBiffRecord) null;
      BIFFRECORDTYPE uint16 = (BIFFRECORDTYPE) BitConverter.ToUInt16(bytes, (int) offset);
      if ((uint) uint16 <= 218U)
      {
        if ((uint) uint16 <= 94U)
        {
          if ((uint) uint16 <= 30U)
          {
            switch (uint16)
            {
              case BIFFRECORDTYPE.BLANK_OLD:
              case BIFFRECORDTYPE.BOOLERR_OLD:
                goto label_26;
              case BIFFRECORDTYPE.INTEGER_OLD:
                goto label_30;
              case BIFFRECORDTYPE.NUMBER_OLD:
                goto label_31;
              case BIFFRECORDTYPE.LABEL_OLD:
                goto label_28;
              case BIFFRECORDTYPE.FORMULA_OLD:
                goto label_34;
              case BIFFRECORDTYPE.BOF_V2:
                break;
              case BIFFRECORDTYPE.EOF:
                return (XlsBiffRecord) new XlsBiffEOF(bytes, offset, reader);
              case BIFFRECORDTYPE.FORMAT_V23:
                goto label_35;
              default:
                goto label_50;
            }
          }
          else
          {
            switch (uint16)
            {
              case BIFFRECORDTYPE.RECORD1904:
                return (XlsBiffRecord) new XlsBiffSimpleValueRecord(bytes, offset, reader);
              case BIFFRECORDTYPE.CONTINUE:
                return (XlsBiffRecord) new XlsBiffContinue(bytes, offset, reader);
              case BIFFRECORDTYPE.WINDOW1:
                return (XlsBiffRecord) new XlsBiffWindow1(bytes, offset, reader);
              case BIFFRECORDTYPE.BACKUP:
                return (XlsBiffRecord) new XlsBiffSimpleValueRecord(bytes, offset, reader);
              case BIFFRECORDTYPE.CODEPAGE:
                return (XlsBiffRecord) new XlsBiffSimpleValueRecord(bytes, offset, reader);
              case BIFFRECORDTYPE.UNCALCED:
                return (XlsBiffRecord) new XlsBiffUncalced(bytes, offset, reader);
              default:
                goto label_50;
            }
          }
        }
        else if ((uint) uint16 <= 141U)
        {
          if (uint16 == BIFFRECORDTYPE.BOUNDSHEET)
            return (XlsBiffRecord) new XlsBiffBoundSheet(bytes, offset, reader);
          if (uint16 == BIFFRECORDTYPE.HIDEOBJ)
            return (XlsBiffRecord) new XlsBiffSimpleValueRecord(bytes, offset, reader);
          goto label_50;
        }
        else
        {
          switch (uint16)
          {
            case BIFFRECORDTYPE.FNGROUPCOUNT:
              return (XlsBiffRecord) new XlsBiffSimpleValueRecord(bytes, offset, reader);
            case BIFFRECORDTYPE.MULRK:
              return (XlsBiffRecord) new XlsBiffMulRKCell(bytes, offset, reader);
            case BIFFRECORDTYPE.MULBLANK:
              return (XlsBiffRecord) new XlsBiffMulBlankCell(bytes, offset, reader);
            case BIFFRECORDTYPE.RSTRING:
              goto label_28;
            case BIFFRECORDTYPE.DBCELL:
              return (XlsBiffRecord) new XlsBiffDbCell(bytes, offset, reader);
            case BIFFRECORDTYPE.BOOKBOOL:
              return (XlsBiffRecord) new XlsBiffSimpleValueRecord(bytes, offset, reader);
            default:
              goto label_50;
          }
        }
      }
      else if ((uint) uint16 <= 638U)
      {
        if ((uint) uint16 <= 253U)
        {
          switch (uint16)
          {
            case BIFFRECORDTYPE.INTERFACEHDR:
              return (XlsBiffRecord) new XlsBiffInterfaceHdr(bytes, offset, reader);
            case BIFFRECORDTYPE.SST:
              return (XlsBiffRecord) new XlsBiffSST(bytes, offset, reader);
            case BIFFRECORDTYPE.LABELSST:
              return (XlsBiffRecord) new XlsBiffLabelSSTCell(bytes, offset, reader);
            default:
              goto label_50;
          }
        }
        else
        {
          switch (uint16)
          {
            case BIFFRECORDTYPE.USESELFS:
              return (XlsBiffRecord) new XlsBiffSimpleValueRecord(bytes, offset, reader);
            case BIFFRECORDTYPE.DIMENSIONS:
              return (XlsBiffRecord) new XlsBiffDimensions(bytes, offset, reader);
            case BIFFRECORDTYPE.BLANK:
            case BIFFRECORDTYPE.BOOLERR:
              goto label_26;
            case BIFFRECORDTYPE.INTEGER:
              goto label_30;
            case BIFFRECORDTYPE.NUMBER:
              goto label_31;
            case BIFFRECORDTYPE.LABEL:
              goto label_28;
            case BIFFRECORDTYPE.STRING:
              return (XlsBiffRecord) new XlsBiffFormulaString(bytes, offset, reader);
            case BIFFRECORDTYPE.ROW:
              return (XlsBiffRecord) new XlsBiffRow(bytes, offset, reader);
            case BIFFRECORDTYPE.BOF_V3:
              break;
            case BIFFRECORDTYPE.INDEX:
              return (XlsBiffRecord) new XlsBiffIndex(bytes, offset, reader);
            case BIFFRECORDTYPE.RK:
              return (XlsBiffRecord) new XlsBiffRKCell(bytes, offset, reader);
            default:
              goto label_50;
          }
        }
      }
      else if ((uint) uint16 <= 1033U)
      {
        if (uint16 != BIFFRECORDTYPE.FORMULA)
        {
          if (uint16 != BIFFRECORDTYPE.BOF_V4)
            goto label_50;
        }
        else
          goto label_34;
      }
      else
      {
        switch (uint16)
        {
          case BIFFRECORDTYPE.FORMAT:
            goto label_35;
          case BIFFRECORDTYPE.QUICKTIP:
            return (XlsBiffRecord) new XlsBiffQuickTip(bytes, offset, reader);
          case BIFFRECORDTYPE.BOF:
            break;
          default:
            goto label_50;
        }
      }
      return (XlsBiffRecord) new XlsBiffBOF(bytes, offset, reader);
label_26:
      return (XlsBiffRecord) new XlsBiffBlankCell(bytes, offset, reader);
label_28:
      return (XlsBiffRecord) new XlsBiffLabelCell(bytes, offset, reader);
label_30:
      return (XlsBiffRecord) new XlsBiffIntegerCell(bytes, offset, reader);
label_31:
      return (XlsBiffRecord) new XlsBiffNumberCell(bytes, offset, reader);
label_34:
      return (XlsBiffRecord) new XlsBiffFormulaCell(bytes, offset, reader);
label_35:
      return (XlsBiffRecord) new XlsBiffFormatString(bytes, offset, reader);
label_50:
      return new XlsBiffRecord(bytes, offset, reader);
    }

    public bool IsCell
    {
      get
      {
        bool isCell = false;
        BIFFRECORDTYPE id = this.ID;
        if ((uint) id <= 253U)
        {
          switch (id)
          {
            case BIFFRECORDTYPE.MULRK:
            case BIFFRECORDTYPE.MULBLANK:
            case BIFFRECORDTYPE.LABELSST:
              break;
            default:
              goto label_4;
          }
        }
        else
        {
          switch (id)
          {
            case BIFFRECORDTYPE.BLANK:
            case BIFFRECORDTYPE.NUMBER:
            case BIFFRECORDTYPE.BOOLERR:
            case BIFFRECORDTYPE.RK:
            case BIFFRECORDTYPE.FORMULA:
              break;
            default:
              goto label_4;
          }
        }
        isCell = true;
label_4:
        return isCell;
      }
    }

    public byte ReadByte(int offset)
    {
      return Buffer.GetByte((Array) this.m_bytes, this.m_readoffset + offset);
    }

    public ushort ReadUInt16(int offset)
    {
      return BitConverter.ToUInt16(this.m_bytes, this.m_readoffset + offset);
    }

    public uint ReadUInt32(int offset)
    {
      return BitConverter.ToUInt32(this.m_bytes, this.m_readoffset + offset);
    }

    public ulong ReadUInt64(int offset)
    {
      return BitConverter.ToUInt64(this.m_bytes, this.m_readoffset + offset);
    }

    public short ReadInt16(int offset)
    {
      return BitConverter.ToInt16(this.m_bytes, this.m_readoffset + offset);
    }

    public int ReadInt32(int offset)
    {
      return BitConverter.ToInt32(this.m_bytes, this.m_readoffset + offset);
    }

    public long ReadInt64(int offset)
    {
      return BitConverter.ToInt64(this.m_bytes, this.m_readoffset + offset);
    }

    public byte[] ReadArray(int offset, int size)
    {
      byte[] dst = new byte[size];
      Buffer.BlockCopy((Array) this.m_bytes, this.m_readoffset + offset, (Array) dst, 0, size);
      return dst;
    }

    public float ReadFloat(int offset)
    {
      return BitConverter.ToSingle(this.m_bytes, this.m_readoffset + offset);
    }

    public double ReadDouble(int offset)
    {
      return BitConverter.ToDouble(this.m_bytes, this.m_readoffset + offset);
    }
  }
}
