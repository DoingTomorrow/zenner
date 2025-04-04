// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelAddressBase
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelAddressBase : ExcelCellBase
  {
    protected internal int _fromRow = -1;
    protected internal int _toRow;
    protected internal int _fromCol;
    protected internal int _toCol;
    protected internal string _wb;
    protected internal string _ws;
    protected internal string _address;
    private ExcelCellAddress _start;
    private ExcelCellAddress _end;
    private ExcelTableAddress _table;
    private string _firstAddress;
    protected internal List<ExcelAddress> _addresses;

    protected internal event EventHandler AddressChange;

    internal ExcelAddressBase()
    {
    }

    public ExcelAddressBase(int fromRow, int fromCol, int toRow, int toColumn)
    {
      this._fromRow = fromRow;
      this._toRow = toRow;
      this._fromCol = fromCol;
      this._toCol = toColumn;
      this.Validate();
      this._address = ExcelCellBase.GetAddress(this._fromRow, this._fromCol, this._toRow, this._toCol);
    }

    public ExcelAddressBase(string address) => this.SetAddress(address);

    public ExcelAddressBase(string address, ExcelPackage pck, ExcelAddressBase referenceAddress)
    {
      this.SetAddress(address);
      this.SetRCFromTable(pck, referenceAddress);
    }

    internal void SetRCFromTable(ExcelPackage pck, ExcelAddressBase referenceAddress)
    {
      if (!string.IsNullOrEmpty(this._wb) || this.Table == null)
        return;
      foreach (ExcelWorksheet worksheet in pck.Workbook.Worksheets)
      {
        foreach (ExcelTable table in worksheet.Tables)
        {
          if (table.Name.Equals(this.Table.Name, StringComparison.InvariantCultureIgnoreCase))
          {
            this._ws = worksheet.Name;
            if (this.Table.IsAll)
            {
              this._fromRow = table.Address._fromRow;
              this._toRow = table.Address._toRow;
            }
            else if (this.Table.IsThisRow)
            {
              if (referenceAddress == null)
              {
                this._fromRow = -1;
                this._toRow = -1;
              }
              else
              {
                this._fromRow = referenceAddress._fromRow;
                this._toRow = this._fromRow;
              }
            }
            else if (this.Table.IsHeader && this.Table.IsData)
            {
              this._fromRow = table.Address._fromRow;
              this._toRow = table.ShowTotal ? table.Address._toRow - 1 : table.Address._toRow;
            }
            else if (this.Table.IsData && this.Table.IsTotals)
            {
              this._fromRow = table.ShowHeader ? table.Address._fromRow + 1 : table.Address._fromRow;
              this._toRow = table.Address._toRow;
            }
            else if (this.Table.IsHeader)
            {
              this._fromRow = table.ShowHeader ? table.Address._fromRow : -1;
              this._toRow = table.ShowHeader ? table.Address._fromRow : -1;
            }
            else if (this.Table.IsTotals)
            {
              this._fromRow = table.ShowTotal ? table.Address._toRow : -1;
              this._toRow = table.ShowTotal ? table.Address._toRow : -1;
            }
            else
            {
              this._fromRow = table.ShowHeader ? table.Address._fromRow + 1 : table.Address._fromRow;
              this._toRow = table.ShowTotal ? table.Address._toRow - 1 : table.Address._toRow;
            }
            if (string.IsNullOrEmpty(this.Table.ColumnSpan))
            {
              this._fromCol = table.Address._fromCol;
              this._toCol = table.Address._toCol;
              return;
            }
            int fromCol = table.Address._fromCol;
            string[] strArray = this.Table.ColumnSpan.Split(':');
            foreach (ExcelTableColumn column in (IEnumerable<ExcelTableColumn>) table.Columns)
            {
              if (this._fromCol <= 0 && strArray[0].Equals(column.Name))
              {
                this._fromCol = fromCol;
                if (strArray.Length == 1)
                {
                  this._toCol = this._fromCol;
                  return;
                }
              }
              else if (strArray.Length > 1 && this._fromCol > 0 && strArray[1].Equals(column.Name))
              {
                this._toCol = fromCol;
                return;
              }
              ++fromCol;
            }
          }
        }
      }
    }

    internal ExcelAddressBase(string address, bool isName)
    {
      if (isName)
      {
        this._address = address;
        this._fromRow = -1;
        this._fromCol = -1;
        this._toRow = -1;
        this._toCol = -1;
        this._start = (ExcelCellAddress) null;
        this._end = (ExcelCellAddress) null;
      }
      else
        this.SetAddress(address);
    }

    protected internal void SetAddress(string address)
    {
      if (address.StartsWith("'"))
      {
        int num = address.IndexOf("'", 1);
        while (num < address.Length && address[num + 1] == '\'')
          num = address.IndexOf("'", num + 2);
        this._ws = address.Substring(1, num - 1).Replace("''", "'");
        this._address = address.Substring(num + 2);
      }
      else if (address.StartsWith("["))
        this.SetWbWs(address);
      else
        this._address = address;
      if (this._address.IndexOfAny(new char[3]
      {
        ',',
        '!',
        '['
      }) > -1)
      {
        this.ExtractAddress(this._address);
      }
      else
      {
        ExcelCellBase.GetRowColFromAddress(this._address, out this._fromRow, out this._fromCol, out this._toRow, out this._toCol);
        this._addresses = (List<ExcelAddress>) null;
        this._start = (ExcelCellAddress) null;
        this._end = (ExcelCellAddress) null;
      }
      this._address = address;
      this.Validate();
    }

    internal void ChangeAddress()
    {
      if (this.AddressChange == null)
        return;
      this.AddressChange((object) this, new EventArgs());
    }

    private void SetWbWs(string address)
    {
      if (address[0] == '[')
      {
        int num = address.IndexOf("]");
        this._wb = address.Substring(1, num - 1);
        this._ws = address.Substring(num + 1);
      }
      else
      {
        this._wb = "";
        this._ws = address;
      }
      int length = this._ws.IndexOf("!");
      if (length <= -1)
        return;
      this._address = this._ws.Substring(length + 1);
      this._ws = this._ws.Substring(0, length);
    }

    internal void ChangeWorksheet(string wsName, string newWs)
    {
      if (this._ws == wsName)
        this._ws = newWs;
      string str = this.GetAddress();
      if (this.Addresses != null)
      {
        foreach (ExcelAddress address in this.Addresses)
        {
          if (address._ws == wsName)
          {
            address._ws = newWs;
            str = str + "," + address.GetAddress();
          }
          else
            str = str + "," + address._address;
        }
      }
      this._address = str;
    }

    private string GetAddress()
    {
      string str = "";
      if (string.IsNullOrEmpty(this._wb))
        str = "[" + this._wb + "]";
      if (string.IsNullOrEmpty(this._ws))
        str += string.Format("'{0}'!", (object) this._ws);
      return str + ExcelCellBase.GetAddress(this._fromRow, this._fromCol, this._toRow, this._toCol);
    }

    public ExcelCellAddress Start
    {
      get
      {
        if (this._start == null)
          this._start = new ExcelCellAddress(this._fromRow, this._fromCol);
        return this._start;
      }
    }

    public ExcelCellAddress End
    {
      get
      {
        if (this._end == null)
          this._end = new ExcelCellAddress(this._toRow, this._toCol);
        return this._end;
      }
    }

    public ExcelTableAddress Table => this._table;

    public virtual string Address => this._address;

    public bool IsName => this._fromRow < 0;

    public override string ToString() => this._address;

    internal string FirstAddress
    {
      get => string.IsNullOrEmpty(this._firstAddress) ? this._address : this._firstAddress;
    }

    internal string AddressSpaceSeparated => this._address.Replace(',', ' ');

    protected void Validate()
    {
      if (this._fromRow > this._toRow || this._fromCol > this._toCol)
        throw new ArgumentOutOfRangeException("Start cell Address must be less or equal to End cell address");
    }

    internal string WorkSheet => this._ws;

    internal virtual List<ExcelAddress> Addresses => this._addresses;

    private bool ExtractAddress(string fullAddress)
    {
      Stack<int> intStack = new Stack<int>();
      List<string> bracketParts = new List<string>();
      string first = "";
      string second = "";
      bool flag = false;
      bool hasSheet = false;
      try
      {
        if (fullAddress == "#REF!")
        {
          this.SetAddress(ref fullAddress, ref second, ref hasSheet);
          return true;
        }
        for (int index = 0; index < fullAddress.Length; ++index)
        {
          char ch = fullAddress[index];
          if (ch == '\'')
          {
            if (flag && index + 1 < fullAddress.Length && fullAddress[index] == '\'')
            {
              if (hasSheet)
                second += (string) (object) ch;
              else
                first += (string) (object) ch;
            }
            flag = !flag;
          }
          else if (intStack.Count > 0)
          {
            if (ch == '[' && !flag)
              intStack.Push(index);
            else if (ch == ']' && !flag)
            {
              if (intStack.Count <= 0)
                return false;
              int num = intStack.Pop();
              bracketParts.Add(fullAddress.Substring(num + 1, index - num - 1));
              if (intStack.Count == 0)
                this.HandleBrackets(first, second, bracketParts);
            }
          }
          else if (ch == '[' && !flag)
            intStack.Push(index);
          else if (ch == '!' && !flag && !first.EndsWith("#REF") && !second.EndsWith("#REF"))
            hasSheet = true;
          else if (ch == ',' && !flag)
            this.SetAddress(ref first, ref second, ref hasSheet);
          else if (hasSheet)
            second += (string) (object) ch;
          else
            first += (string) (object) ch;
        }
        if (this.Table == null)
          this.SetAddress(ref first, ref second, ref hasSheet);
        return true;
      }
      catch
      {
        return false;
      }
    }

    private void HandleBrackets(string first, string second, List<string> bracketParts)
    {
      if (string.IsNullOrEmpty(first))
        return;
      this._table = new ExcelTableAddress();
      this.Table.Name = first;
      foreach (string bracketPart in bracketParts)
      {
        if (bracketPart.IndexOf("[") < 0)
        {
          switch (bracketPart.ToLower())
          {
            case "#all":
              this._table.IsAll = true;
              continue;
            case "#headers":
              this._table.IsHeader = true;
              continue;
            case "#data":
              this._table.IsData = true;
              continue;
            case "#totals":
              this._table.IsTotals = true;
              continue;
            case "#this row":
              this._table.IsThisRow = true;
              continue;
            default:
              if (string.IsNullOrEmpty(this._table.ColumnSpan))
              {
                this._table.ColumnSpan = bracketPart;
                continue;
              }
              ExcelTableAddress table = this._table;
              table.ColumnSpan = table.ColumnSpan + ":" + bracketPart;
              continue;
          }
        }
      }
    }

    internal ExcelAddressBase.eAddressCollition Collide(ExcelAddressBase address)
    {
      if (address.WorkSheet != this.WorkSheet || address._fromRow > this._toRow || address._fromCol > this._toCol || this._fromRow > address._toRow || this._fromCol > address._toCol)
        return ExcelAddressBase.eAddressCollition.No;
      if (address._fromRow == this._fromRow && address._fromCol == this._fromCol && address._toRow == this._toRow && address._toCol == this._toCol)
        return ExcelAddressBase.eAddressCollition.Equal;
      return address._fromRow >= this._fromRow && address._toRow <= this._toRow && address._fromCol >= this._fromCol && address._toCol <= this._toCol ? ExcelAddressBase.eAddressCollition.Inside : ExcelAddressBase.eAddressCollition.Partly;
    }

    internal ExcelAddressBase AddRow(int row, int rows)
    {
      if (row > this._toRow)
        return this;
      return row <= this._fromRow ? new ExcelAddressBase(this._fromRow + rows, this._fromCol, this._toRow + rows, this._toCol) : new ExcelAddressBase(this._fromRow, this._fromCol, this._toRow + rows, this._toCol);
    }

    internal ExcelAddressBase DeleteRow(int row, int rows)
    {
      if (row > this._toRow)
        return this;
      if (row + rows <= this._fromRow)
        return new ExcelAddressBase(this._fromRow - rows, this._fromCol, this._toRow - rows, this._toCol);
      if (row <= this._fromRow && row + rows > this._toRow)
        return (ExcelAddressBase) null;
      return row <= this._fromRow ? new ExcelAddressBase(row, this._fromCol, this._toRow - rows, this._toCol) : new ExcelAddressBase(this._fromRow, this._fromCol, this._toRow - rows < row ? row - 1 : this._toRow - rows, this._toCol);
    }

    internal ExcelAddressBase AddColumn(int col, int cols)
    {
      if (col > this._toCol)
        return this;
      return col <= this._fromCol ? new ExcelAddressBase(this._fromRow, this._fromCol + cols, this._toRow, this._toCol + cols) : new ExcelAddressBase(this._fromRow, this._fromCol, this._toRow, this._toCol + cols);
    }

    internal ExcelAddressBase DeleteColumn(int col, int cols)
    {
      if (col > this._toCol)
        return this;
      if (col + cols <= this._fromRow)
        return new ExcelAddressBase(this._fromRow, this._fromCol - cols, this._toRow, this._toCol - cols);
      if (col <= this._fromCol && col + cols > this._toCol)
        return (ExcelAddressBase) null;
      return col <= this._fromCol ? new ExcelAddressBase(this._fromRow, col, this._toRow, this._toCol - cols) : new ExcelAddressBase(this._fromRow, this._fromCol, this._toRow, this._toCol - cols < col ? col - 1 : this._toCol - cols);
    }

    internal ExcelAddressBase Insert(ExcelAddressBase address, ExcelAddressBase.eShiftType Shift)
    {
      if (this._toRow < address._fromRow || this._toCol < address._fromCol || this._fromRow > address._toRow && this._fromCol > address._toCol)
        return this;
      int rows = address.Rows;
      int columns = address.Columns;
      if (Shift == ExcelAddressBase.eShiftType.Right)
      {
        if (address._fromRow > this._fromRow)
          ExcelCellBase.GetAddress(this._fromRow, this._fromCol, address._fromRow, this._toCol);
        if (address._fromCol > this._fromCol)
          ExcelCellBase.GetAddress(this._fromRow < address._fromRow ? this._fromRow : address._fromRow, this._fromCol, address._fromRow, this._toCol);
      }
      if (this._toRow < address._fromRow)
      {
        int fromRow1 = this._fromRow;
        int fromRow2 = address._fromRow;
      }
      return (ExcelAddressBase) null;
    }

    private void SetAddress(ref string first, ref string second, ref bool hasSheet)
    {
      string str1;
      string str2;
      if (hasSheet)
      {
        str1 = first;
        str2 = second;
        first = "";
        second = "";
      }
      else
      {
        str2 = first;
        str1 = "";
        first = "";
      }
      hasSheet = false;
      if (string.IsNullOrEmpty(this._firstAddress))
      {
        if (string.IsNullOrEmpty(this._ws) || !string.IsNullOrEmpty(str1))
          this._ws = str1;
        this._firstAddress = str2;
        ExcelCellBase.GetRowColFromAddress(str2, out this._fromRow, out this._fromCol, out this._toRow, out this._toCol);
      }
      else
      {
        if (this._addresses == null)
          this._addresses = new List<ExcelAddress>();
        this._addresses.Add(new ExcelAddress(this._ws, str2));
      }
    }

    internal static ExcelAddressBase.AddressType IsValid(string Address)
    {
      if (Address == "#REF!")
        return ExcelAddressBase.AddressType.Invalid;
      if (ExcelAddressBase.IsFormula(Address))
        return ExcelAddressBase.AddressType.Formula;
      string wb;
      string intAddress;
      if (!ExcelAddressBase.SplitAddress(Address, out wb, out string _, out intAddress))
        return ExcelAddressBase.AddressType.Invalid;
      if (intAddress.Contains("["))
        return !string.IsNullOrEmpty(wb) ? ExcelAddressBase.AddressType.ExternalAddress : ExcelAddressBase.AddressType.InternalAddress;
      if (intAddress.Contains(","))
        intAddress = intAddress.Substring(0, intAddress.IndexOf(','));
      return ExcelAddressBase.IsAddress(intAddress) ? (!string.IsNullOrEmpty(wb) ? ExcelAddressBase.AddressType.ExternalAddress : ExcelAddressBase.AddressType.InternalAddress) : (!string.IsNullOrEmpty(wb) ? ExcelAddressBase.AddressType.ExternalName : ExcelAddressBase.AddressType.InternalName);
    }

    private static bool IsAddress(string intAddress)
    {
      if (string.IsNullOrEmpty(intAddress))
        return false;
      string[] strArray = intAddress.Split(':');
      int row1;
      int col1;
      if (!ExcelCellBase.GetRowCol(strArray[0], out row1, out col1, false))
        return false;
      int row2;
      int col2;
      if (strArray.Length > 1)
      {
        if (!ExcelCellBase.GetRowCol(strArray[1], out row2, out col2, false))
          return false;
      }
      else
      {
        row2 = row1;
        col2 = col1;
      }
      return row1 <= row2 && col1 <= col2 && col1 > -1 && col2 <= 16384 && row1 > -1 && row2 <= 1048576;
    }

    private static bool SplitAddress(
      string Address,
      out string wb,
      out string ws,
      out string intAddress)
    {
      wb = "";
      ws = "";
      intAddress = "";
      string str = "";
      bool flag = false;
      int num = -1;
      for (int index = 0; index < Address.Length; ++index)
      {
        if (Address[index] == '\'')
        {
          flag = !flag;
          if (index > 0 && Address[index - 1] == '\'')
            str += "'";
        }
        else
        {
          if (Address[index] == '!' && !flag)
          {
            ws = str;
            intAddress = Address.Substring(index + 1);
            return true;
          }
          if (Address[index] == '[' && !flag)
          {
            if (index > 0)
            {
              intAddress = Address;
              return true;
            }
            num = index;
          }
          else if (Address[index] == ']')
          {
            if (num <= -1)
              return false;
            wb = str;
            str = "";
          }
          else
            str += (string) (object) Address[index];
        }
      }
      intAddress = str;
      return true;
    }

    private static bool IsFormula(string address)
    {
      bool flag = false;
      for (int index = 0; index < address.Length; ++index)
      {
        if (address[index] == '\'')
          flag = !flag;
        else if (!flag)
        {
          if (address.Substring(index, 1).IndexOfAny(new char[12]
          {
            '(',
            ')',
            '+',
            '-',
            '*',
            '/',
            '.',
            '=',
            '^',
            '&',
            '%',
            '"'
          }) > -1)
            return true;
        }
      }
      return false;
    }

    private static bool IsValidName(string address)
    {
      return Regex.IsMatch(address, "[^0-9./*-+,\u00BD!\"@#£%&/{}()\\[\\]=?`^~':;<>|][^/*-+,\u00BD!\"@#£%&/{}()\\[\\]=?`^~':;<>|]*");
    }

    public int Rows => this._toRow - this._fromRow + 1;

    public int Columns => this._toCol - this._fromCol + 1;

    internal bool IsMultiCell() => this._fromRow < this._fromCol || this._fromCol < this._toCol;

    internal static string GetWorkbookPart(string address)
    {
      if (address[0] == '[')
      {
        int num = address.IndexOf(']') + 1;
        if (num > 0)
          return address.Substring(1, num - 2);
      }
      return "";
    }

    internal static string GetWorksheetPart(string address, string defaultWorkSheet)
    {
      int endIx = 0;
      return ExcelAddressBase.GetWorksheetPart(address, defaultWorkSheet, ref endIx);
    }

    internal static string GetWorksheetPart(string address, string defaultWorkSheet, ref int endIx)
    {
      if (address == "")
        return defaultWorkSheet;
      int num1 = 0;
      if (address[0] == '[')
        num1 = address.IndexOf(']') + 1;
      if (num1 <= 0 || num1 >= address.Length)
        return defaultWorkSheet;
      if (address[num1] == '\'')
        return ExcelAddressBase.GetString(address, num1, out endIx);
      int num2 = address.IndexOf('!', num1);
      return num2 > num1 ? address.Substring(num1, num2 - num1) : defaultWorkSheet;
    }

    internal static string GetAddressPart(string address)
    {
      int endIx = 0;
      ExcelAddressBase.GetWorksheetPart(address, "", ref endIx);
      return endIx < address.Length && address[endIx] == '!' ? address.Substring(endIx + 1) : "";
    }

    internal static void SplitAddress(
      string fullAddress,
      out string wb,
      out string ws,
      out string address,
      string defaultWorksheet = "")
    {
      wb = ExcelAddressBase.GetWorkbookPart(fullAddress);
      int endIx = 0;
      ws = ExcelAddressBase.GetWorksheetPart(fullAddress, defaultWorksheet, ref endIx);
      if (endIx < fullAddress.Length)
      {
        if (fullAddress[endIx] == '!')
          address = fullAddress.Substring(endIx + 1);
        else
          address = fullAddress.Substring(endIx);
      }
      else
        address = "";
    }

    private static string GetString(string address, int ix, out int endIx)
    {
      int num = address.IndexOf("''");
      while (num > -1)
        num = address.IndexOf("''");
      endIx = address.IndexOf("'");
      return address.Substring(ix, endIx - ix).Replace("''", "'");
    }

    internal enum eAddressCollition
    {
      No,
      Partly,
      Inside,
      Equal,
    }

    internal enum eShiftType
    {
      Right,
      Down,
      EntireRow,
      EntireColumn,
    }

    internal enum AddressType
    {
      Invalid,
      InternalAddress,
      ExternalAddress,
      InternalName,
      ExternalName,
      Formula,
    }
  }
}
