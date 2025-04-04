// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelCellAddress
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelCellAddress
  {
    private int _row;
    private int _column;
    private string _address;

    public ExcelCellAddress()
      : this(1, 1)
    {
    }

    public ExcelCellAddress(int row, int column)
    {
      this.Row = row;
      this.Column = column;
    }

    public ExcelCellAddress(string address) => this.Address = address;

    public int Row
    {
      get => this._row;
      private set
      {
        this._row = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof (value), "Row cannot be less than 1.");
        if (this._column > 0)
          this._address = ExcelCellBase.GetAddress(this._row, this._column);
        else
          this._address = "#REF!";
      }
    }

    public int Column
    {
      get => this._column;
      private set
      {
        this._column = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof (value), "Column cannot be less than 1.");
        if (this._row > 0)
          this._address = ExcelCellBase.GetAddress(this._row, this._column);
        else
          this._address = "#REF!";
      }
    }

    public string Address
    {
      get => this._address;
      internal set
      {
        this._address = value;
        ExcelCellBase.GetRowColFromAddress(this._address, out this._row, out this._column);
      }
    }

    public bool IsRef => this._row <= 0;
  }
}
