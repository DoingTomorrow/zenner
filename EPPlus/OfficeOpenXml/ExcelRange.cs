// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelRange
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelRange : ExcelRangeBase
  {
    internal ExcelRange(ExcelWorksheet sheet)
      : base(sheet)
    {
    }

    internal ExcelRange(ExcelWorksheet sheet, string address)
      : base(sheet, address)
    {
    }

    internal ExcelRange(ExcelWorksheet sheet, int fromRow, int fromCol, int toRow, int toCol)
      : base(sheet)
    {
      this._fromRow = fromRow;
      this._fromCol = fromCol;
      this._toRow = toRow;
      this._toCol = toCol;
    }

    public ExcelRange this[string Address]
    {
      get
      {
        if (this._worksheet.Names.ContainsKey(Address))
        {
          if (this._worksheet.Names[Address].IsName)
            return (ExcelRange) null;
          this.Address = this._worksheet.Names[Address].Address;
        }
        else
          this.Address = Address;
        return this;
      }
    }

    private ExcelRange GetTableAddess(ExcelWorksheet _worksheet, string address)
    {
      int num1 = address.IndexOf('[');
      if (num1 == 0)
      {
        int num2 = address.IndexOf(']', num1 + 1);
        if (num1 >= 0 & num2 >= 0)
          address.Substring(num1 + 1, num2 - 1);
      }
      return (ExcelRange) null;
    }

    public ExcelRange this[int Row, int Col]
    {
      get
      {
        ExcelRange.ValidateRowCol(Row, Col);
        this._fromCol = Col;
        this._fromRow = Row;
        this._toCol = Col;
        this._toRow = Row;
        this.Address = ExcelCellBase.GetAddress(this._fromRow, this._fromCol);
        return this;
      }
    }

    public ExcelRange this[int FromRow, int FromCol, int ToRow, int ToCol]
    {
      get
      {
        ExcelRange.ValidateRowCol(FromRow, FromCol);
        ExcelRange.ValidateRowCol(ToRow, ToCol);
        this._fromCol = FromCol;
        this._fromRow = FromRow;
        this._toCol = ToCol;
        this._toRow = ToRow;
        this.Address = ExcelCellBase.GetAddress(this._fromRow, this._fromCol, this._toRow, this._toCol);
        return this;
      }
    }

    private static void ValidateRowCol(int Row, int Col)
    {
      if (Row < 1 || Row > 1048576)
        throw new ArgumentException("Row out of range");
      if (Col < 1 || Col > 16384)
        throw new ArgumentException("Column out of range");
    }
  }
}
