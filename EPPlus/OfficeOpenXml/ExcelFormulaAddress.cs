// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelFormulaAddress
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelFormulaAddress : ExcelAddressBase
  {
    private bool _fromRowFixed;
    private bool _toRowFixed;
    private bool _fromColFixed;
    private bool _toColFixed;
    internal List<ExcelFormulaAddress> _addresses;

    internal ExcelFormulaAddress()
    {
    }

    public ExcelFormulaAddress(int fromRow, int fromCol, int toRow, int toColumn)
      : base(fromRow, fromCol, toRow, toColumn)
    {
      this._ws = "";
    }

    public ExcelFormulaAddress(string address)
      : base(address)
    {
      this.SetFixed();
    }

    internal ExcelFormulaAddress(string ws, string address)
      : base(address)
    {
      if (string.IsNullOrEmpty(this._ws))
        this._ws = ws;
      this.SetFixed();
    }

    internal ExcelFormulaAddress(string ws, string address, bool isName)
      : base(address, isName)
    {
      if (string.IsNullOrEmpty(this._ws))
        this._ws = ws;
      if (isName)
        return;
      this.SetFixed();
    }

    private void SetFixed()
    {
      if (this.Address.IndexOf("[") >= 0)
        return;
      string firstAddress = this.FirstAddress;
      if (this._fromRow == this._toRow && this._fromCol == this._toCol)
      {
        this.GetFixed(firstAddress, out this._fromRowFixed, out this._fromColFixed);
      }
      else
      {
        string[] strArray = firstAddress.Split(':');
        this.GetFixed(strArray[0], out this._fromRowFixed, out this._fromColFixed);
        this.GetFixed(strArray[1], out this._toRowFixed, out this._toColFixed);
      }
    }

    private void GetFixed(string address, out bool rowFixed, out bool colFixed)
    {
      rowFixed = colFixed = false;
      int num;
      for (int index = address.IndexOf('$'); index > -1; index = address.IndexOf('$', num))
      {
        num = index + 1;
        if (num < address.Length)
        {
          if (address[num] >= '0' && address[num] <= '9')
          {
            rowFixed = true;
            break;
          }
          colFixed = true;
        }
      }
    }

    public new string Address
    {
      get
      {
        if (string.IsNullOrEmpty(this._address) && this._fromRow > 0)
          this._address = ExcelCellBase.GetAddress(this._fromRow, this._fromCol, this._toRow, this._toCol, this._fromRowFixed, this._toRowFixed, this._fromColFixed, this._toColFixed);
        return this._address;
      }
      set
      {
        this.SetAddress(value);
        this.ChangeAddress();
        this.SetFixed();
      }
    }

    public List<ExcelFormulaAddress> Addresses
    {
      get
      {
        if (this._addresses == null)
          this._addresses = new List<ExcelFormulaAddress>();
        return this._addresses;
      }
    }

    internal string GetOffset(int row, int column)
    {
      int fromRow = this._fromRow;
      int fromCol = this._fromCol;
      int toRow = this._toRow;
      int toCol = this._toCol;
      if (!this._fromRowFixed)
        fromRow += row;
      if (!this._fromColFixed)
        fromCol += column;
      if (fromRow != toRow || fromCol != toCol)
      {
        if (!this._toRowFixed)
          toRow += row;
        if (!this._toColFixed)
          toCol += column;
      }
      string offset = ExcelCellBase.GetAddress(fromRow, fromCol, toRow, toCol, this._fromRowFixed, this._fromColFixed, this._toRowFixed, this._toColFixed);
      if (this.Addresses != null)
      {
        foreach (ExcelFormulaAddress address in this.Addresses)
          offset = offset + "," + address.GetOffset(row, column);
      }
      return offset;
    }
  }
}
