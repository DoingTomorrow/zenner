// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelCellBase
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml
{
  public abstract class ExcelCellBase
  {
    internal static void SplitCellID(ulong cellID, out int sheet, out int row, out int col)
    {
      sheet = (int) (cellID % 32768UL);
      col = (int) (cellID >> 15) & 1023;
      row = (int) (cellID >> 29);
    }

    internal static ulong GetCellID(int SheetID, int row, int col)
    {
      return (ulong) ((long) SheetID + ((long) col << 15) + ((long) row << 29));
    }

    public static string TranslateFromR1C1(string value, int row, int col)
    {
      return ExcelCellBase.Translate(value, new ExcelCellBase.dlgTransl(ExcelCellBase.ToAbs), row, col, -1, -1);
    }

    public static string TranslateToR1C1(string value, int row, int col)
    {
      return ExcelCellBase.Translate(value, new ExcelCellBase.dlgTransl(ExcelCellBase.ToR1C1), row, col, -1, -1);
    }

    private static string Translate(
      string value,
      ExcelCellBase.dlgTransl addressTranslator,
      int row,
      int col,
      int rowIncr,
      int colIncr)
    {
      if (value == "")
        return "";
      bool flag = false;
      string str = "";
      string part = "";
      char ch1 = char.MinValue;
      for (int index = 0; index < value.Length; ++index)
      {
        char ch2 = value[index];
        switch (ch2)
        {
          case '"':
          case '\'':
            if (!flag && part != "" && (int) ch1 == (int) ch2)
            {
              str += addressTranslator(part, row, col, rowIncr, colIncr);
              part = "";
            }
            ch1 = ch2;
            flag = !flag;
            str += (string) (object) ch2;
            break;
          default:
            if (flag)
            {
              str += (string) (object) ch2;
              break;
            }
            if ((ch2 == '-' || ch2 == '+' || ch2 == '*' || ch2 == '/' || ch2 == '=' || ch2 == '^' || ch2 == ',' || ch2 == ':' || ch2 == '<' || ch2 == '>' || ch2 == '(' || ch2 == ')' || ch2 == '!' || ch2 == ' ' || ch2 == '&' || ch2 == '%') && (index == 0 || value[index - 1] != '['))
            {
              str = str + addressTranslator(part, row, col, rowIncr, colIncr) + (object) ch2;
              part = "";
              break;
            }
            part += (string) (object) ch2;
            break;
        }
      }
      if (part != "")
        str += addressTranslator(part, row, col, rowIncr, colIncr);
      return str;
    }

    private static string ToR1C1(string part, int row, int col, int rowIncr, int colIncr)
    {
      string str = "R";
      int row1;
      int col1;
      if (!ExcelCellBase.GetRowCol(part, out row1, out col1, false) || row1 == 0 || col1 == 0)
        return part;
      if (part.IndexOf('$', 1) > 0)
        str += row1.ToString();
      else if (row1 - row != 0)
        str += string.Format("[{0}]", (object) (row1 - row));
      if (part.StartsWith("$"))
        return str + "C" + (object) col1;
      return col1 - col != 0 ? str + "C" + string.Format("[{0}]", (object) (col1 - col)) : str + "C";
    }

    private static string ToAbs(string part, int row, int col, int rowIncr, int colIncr)
    {
      string upper = part.ToUpper();
      if (upper.IndexOf("R") != 0)
        return part;
      if (part.Length == 1)
        return ExcelCellBase.GetAddress(row, col);
      int num = upper.IndexOf("C");
      if (num == -1)
      {
        bool fixedAddr;
        int rc = ExcelCellBase.GetRC(part, row, out fixedAddr);
        return rc > int.MinValue ? ExcelCellBase.GetAddress(rc, fixedAddr, col, false) : part;
      }
      bool fixedAddr1;
      int rc1 = ExcelCellBase.GetRC(part.Substring(1, num - 1), row, out fixedAddr1);
      bool fixedAddr2;
      int rc2 = ExcelCellBase.GetRC(part.Substring(num + 1, part.Length - num - 1), col, out fixedAddr2);
      return rc1 > int.MinValue && rc2 > int.MinValue ? ExcelCellBase.GetAddress(rc1, fixedAddr1, rc2, fixedAddr2) : part;
    }

    private static string AddToRowColumnTranslator(
      string Address,
      int row,
      int col,
      int rowIncr,
      int colIncr)
    {
      int row1;
      int col1;
      if (Address == "#REF!" || !ExcelCellBase.GetRowCol(Address, out row1, out col1, false) || row1 == 0 || col1 == 0)
        return Address;
      if (rowIncr != 0 && row != 0 && row1 >= row && Address.IndexOf('$', 1) == -1)
      {
        if (row1 < row - rowIncr)
          return "#REF!";
        row1 += rowIncr;
      }
      if (colIncr != 0 && col != 0 && col1 >= col && !Address.StartsWith("$"))
      {
        if (col1 < col - colIncr)
          return "#REF!";
        col1 += colIncr;
      }
      Address = ExcelCellBase.GetAddress(row1, Address.IndexOf('$', 1) > -1, col1, Address.StartsWith("$"));
      return Address;
    }

    private static string GetRCFmt(int v)
    {
      if (v < 0)
        return string.Format("[{0}]", (object) v);
      return v <= 0 ? "" : v.ToString();
    }

    private static int GetRC(string value, int OffsetValue, out bool fixedAddr)
    {
      if (value == "")
      {
        fixedAddr = false;
        return OffsetValue;
      }
      if (value[0] == '[' && value[value.Length - 1] == ']')
      {
        fixedAddr = false;
        int result;
        return int.TryParse(value.Substring(1, value.Length - 2), out result) ? OffsetValue + result : int.MinValue;
      }
      fixedAddr = true;
      int result1;
      return int.TryParse(value, out result1) ? result1 : int.MinValue;
    }

    protected internal static string GetColumnLetter(int iColumnNumber)
    {
      return ExcelCellBase.GetColumnLetter(iColumnNumber, false);
    }

    protected internal static string GetColumnLetter(int iColumnNumber, bool fixedCol)
    {
      if (iColumnNumber < 1)
        return "#REF!";
      string str = "";
      do
      {
        str = ((char) (65 + (iColumnNumber - 1) % 26)).ToString() + str;
        iColumnNumber = (iColumnNumber - (iColumnNumber - 1) % 26) / 26;
      }
      while (iColumnNumber > 0);
      return !fixedCol ? str : "$" + str;
    }

    internal static bool GetRowColFromAddress(
      string CellAddress,
      out int FromRow,
      out int FromColumn,
      out int ToRow,
      out int ToColumn)
    {
      if (CellAddress.IndexOf('[') > 0)
      {
        FromRow = -1;
        FromColumn = -1;
        ToRow = -1;
        ToColumn = -1;
        return false;
      }
      CellAddress = CellAddress.ToUpper();
      if (CellAddress.IndexOf(' ') > 0)
        CellAddress = CellAddress.Substring(0, CellAddress.IndexOf(' '));
      bool rowColFromAddress;
      if (CellAddress.IndexOf(':') < 0)
      {
        rowColFromAddress = ExcelCellBase.GetRowColFromAddress(CellAddress, out FromRow, out FromColumn);
        ToColumn = FromColumn;
        ToRow = FromRow;
      }
      else
      {
        string[] strArray = CellAddress.Split(':');
        rowColFromAddress = ExcelCellBase.GetRowColFromAddress(strArray[0], out FromRow, out FromColumn);
        if (rowColFromAddress)
          rowColFromAddress = ExcelCellBase.GetRowColFromAddress(strArray[1], out ToRow, out ToColumn);
        else
          ExcelCellBase.GetRowColFromAddress(strArray[1], out ToRow, out ToColumn);
        if (FromColumn <= 0)
          FromColumn = 1;
        if (FromRow <= 0)
          FromRow = 1;
        if (ToColumn <= 0)
          ToColumn = 16384;
        if (ToRow <= 0)
          ToRow = 1048576;
      }
      return rowColFromAddress;
    }

    internal static bool GetRowColFromAddress(string CellAddress, out int Row, out int Column)
    {
      return ExcelCellBase.GetRowCol(CellAddress, out Row, out Column, true);
    }

    internal static bool GetRowCol(string address, out int row, out int col, bool throwException)
    {
      bool flag = true;
      string s = "";
      string sCol = "";
      col = 0;
      if (address.IndexOf(':') > 0)
        address = address.Substring(0, address.IndexOf(':'));
      if (address.EndsWith("#REF!"))
      {
        row = 0;
        col = 0;
        return true;
      }
      int num = address.IndexOf('!');
      if (num >= 0)
        address = address.Substring(num + 1);
      address = address.ToUpper();
      for (int index = 0; index < address.Length; ++index)
      {
        if (address[index] >= 'A' && address[index] <= 'Z' && flag && sCol.Length <= 3)
          sCol += (string) (object) address[index];
        else if (address[index] >= '0' && address[index] <= '9')
        {
          s += (string) (object) address[index];
          flag = false;
        }
        else if (address[index] != '$')
        {
          if (throwException)
            throw new Exception(string.Format("Invalid Address format {0}", (object) address));
          row = 0;
          col = 0;
          return false;
        }
      }
      if (sCol != "")
      {
        col = ExcelCellBase.GetColumn(sCol);
        if (!(s == ""))
          return int.TryParse(s, out row);
        row = 0;
        return col > 0;
      }
      col = 0;
      int.TryParse(s, out row);
      return row > 0;
    }

    private static int GetColumn(string sCol)
    {
      int column = 0;
      int num = sCol.Length - 1;
      for (int index = num; index >= 0; --index)
        column += ((int) sCol[index] - 64) * (int) Math.Pow(26.0, (double) (num - index));
      return column;
    }

    public static string GetAddress(int Row, int Column)
    {
      return ExcelCellBase.GetAddress(Row, Column, false);
    }

    public static string GetAddress(int Row, bool AbsoluteRow, int Column, bool AbsoluteCol)
    {
      return (AbsoluteCol ? "$" : "") + ExcelCellBase.GetColumnLetter(Column) + (AbsoluteRow ? "$" : "") + Row.ToString();
    }

    public static string GetAddress(int Row, int Column, bool Absolute)
    {
      if (Row == 0 || Column == 0)
        return "#REF!";
      return Absolute ? "$" + ExcelCellBase.GetColumnLetter(Column) + "$" + Row.ToString() : ExcelCellBase.GetColumnLetter(Column) + Row.ToString();
    }

    public static string GetAddress(int FromRow, int FromColumn, int ToRow, int ToColumn)
    {
      return ExcelCellBase.GetAddress(FromRow, FromColumn, ToRow, ToColumn, false);
    }

    public static string GetAddress(
      int FromRow,
      int FromColumn,
      int ToRow,
      int ToColumn,
      bool Absolute)
    {
      if (FromRow == ToRow && FromColumn == ToColumn)
        return ExcelCellBase.GetAddress(FromRow, FromColumn, Absolute);
      if (FromRow == 1 && ToRow == 1048576)
      {
        string str = Absolute ? "$" : "";
        return str + ExcelCellBase.GetColumnLetter(FromColumn) + ":" + str + ExcelCellBase.GetColumnLetter(ToColumn);
      }
      if (FromColumn != 1 || ToColumn != 16384)
        return ExcelCellBase.GetAddress(FromRow, FromColumn, Absolute) + ":" + ExcelCellBase.GetAddress(ToRow, ToColumn, Absolute);
      string str1 = Absolute ? "$" : "";
      return str1 + FromRow.ToString() + ":" + str1 + ToRow.ToString();
    }

    public static string GetAddress(
      int FromRow,
      int FromColumn,
      int ToRow,
      int ToColumn,
      bool FixedFromRow,
      bool FixedFromColumn,
      bool FixedToRow,
      bool FixedToColumn)
    {
      if (FromRow == ToRow && FromColumn == ToColumn)
        return ExcelCellBase.GetAddress(FromRow, FixedFromRow, FromColumn, FixedFromColumn);
      if (FromRow == 1 && ToRow == 1048576)
        return ExcelCellBase.GetColumnLetter(FromColumn, FixedFromColumn) + ":" + ExcelCellBase.GetColumnLetter(ToColumn, FixedToColumn);
      if (FromColumn != 1 || ToColumn != 16384)
        return ExcelCellBase.GetAddress(FromRow, FixedFromRow, FromColumn, FixedFromColumn) + ":" + ExcelCellBase.GetAddress(ToRow, FixedToRow, ToColumn, FixedToColumn);
      return (FixedFromRow ? "$" : "") + FromRow.ToString() + ":" + (FixedToRow ? "$" : "") + ToRow.ToString();
    }

    public static string GetFullAddress(string worksheetName, string address)
    {
      return ExcelCellBase.GetFullAddress(worksheetName, address, true);
    }

    internal static string GetFullAddress(string worksheetName, string address, bool fullRowCol)
    {
      if (address.IndexOf("!") == -1 || address == "#REF!")
      {
        if (fullRowCol)
        {
          string[] strArray = address.Split(':');
          if (strArray.Length > 0)
          {
            address = string.Format("'{0}'!{1}", (object) worksheetName, (object) strArray[0]);
            if (strArray.Length > 1)
              address += string.Format(":{0}", (object) strArray[1]);
          }
        }
        else
        {
          ExcelAddressBase excelAddressBase = new ExcelAddressBase(address);
          if (excelAddressBase._fromRow == 1 && excelAddressBase._toRow == 1048576 || excelAddressBase._fromCol == 1 && excelAddressBase._toCol == 16384)
            address = string.Format("'{0}'!{1}{2}:{3}{4}", (object) worksheetName, (object) ExcelCellBase.GetColumnLetter(excelAddressBase._fromCol), (object) excelAddressBase._fromRow, (object) ExcelCellBase.GetColumnLetter(excelAddressBase._toCol), (object) excelAddressBase._toRow);
          else
            address = ExcelCellBase.GetFullAddress(worksheetName, address, true);
        }
      }
      return address;
    }

    public static bool IsValidAddress(string address)
    {
      address = address.ToUpper();
      string s1 = "";
      string sCol1 = "";
      string s2 = "";
      string sCol2 = "";
      bool flag = false;
      for (int index = 0; index < address.Length; ++index)
      {
        if (address[index] >= 'A' && address[index] <= 'Z')
        {
          if (!flag)
          {
            if (s1 != "")
              return false;
            sCol1 += (string) (object) address[index];
            if (sCol1.Length > 3)
              return false;
          }
          else
          {
            if (s2 != "")
              return false;
            sCol2 += (string) (object) address[index];
            if (sCol2.Length > 3)
              return false;
          }
        }
        else if (address[index] >= '0' && address[index] <= '9')
        {
          if (!flag)
          {
            s1 += (string) (object) address[index];
            if (s1.Length > 7)
              return false;
          }
          else
          {
            s2 += (string) (object) address[index];
            if (s2.Length > 7)
              return false;
          }
        }
        else if (address[index] == ':')
          flag = true;
        else if (address[index] != '$' || index == address.Length - 1 || address[index + 1] == ':')
          return false;
      }
      if (s1 != "" && sCol1 != "" && s2 == "" && sCol2 == "")
        return ExcelCellBase.GetColumn(sCol1) <= 16384 && int.Parse(s1) <= 1048576;
      if (s1 != "" && s2 != "" && sCol1 != "" && sCol2 != "")
      {
        int num = int.Parse(s2);
        int column = ExcelCellBase.GetColumn(sCol2);
        return ExcelCellBase.GetColumn(sCol1) <= column && int.Parse(s1) <= num && column <= 16384 && num <= 1048576;
      }
      if (s1 == "" && s2 == "" && sCol1 != "" && sCol2 != "")
      {
        int column = ExcelCellBase.GetColumn(sCol2);
        return ExcelCellBase.GetColumn(sCol1) <= column && column <= 16384;
      }
      if (!(s1 != "") || !(s2 != "") || !(sCol1 == "") || !(sCol2 == ""))
        return false;
      int num1 = int.Parse(s2);
      return int.Parse(s1) <= num1 && num1 <= 1048576;
    }

    public static bool IsValidCellAddress(string cellAddress)
    {
      bool flag = false;
      try
      {
        int Row;
        int Column;
        if (ExcelCellBase.GetRowColFromAddress(cellAddress, out Row, out Column))
          flag = Row > 0 && Column > 0 && Row <= 1048576 && Column <= 16384;
      }
      catch
      {
      }
      return flag;
    }

    internal static string UpdateFormulaReferences(
      string Formula,
      int rowIncrement,
      int colIncrement,
      int afterRow,
      int afterColumn)
    {
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      try
      {
        IEnumerable<Token> tokens = new SourceCodeTokenizer((IFunctionNameProvider) FunctionNameProvider.Empty, NameValueProvider.Empty).Tokenize(Formula);
        string str = "";
        foreach (Token token in tokens)
        {
          if (token.TokenType == OfficeOpenXml.FormulaParsing.LexicalAnalysis.TokenType.ExcelAddress)
          {
            ExcelAddressBase excelAddressBase = new ExcelAddressBase(token.Value);
            if (rowIncrement > 0)
              excelAddressBase = excelAddressBase.AddRow(afterRow, rowIncrement);
            else if (rowIncrement < 0)
              excelAddressBase = excelAddressBase.DeleteRow(afterRow, -rowIncrement);
            if (colIncrement > 0)
              excelAddressBase = excelAddressBase.AddColumn(afterColumn, colIncrement);
            else if (colIncrement > 0)
              excelAddressBase = excelAddressBase.DeleteColumn(afterColumn, -colIncrement);
            str = excelAddressBase != null ? str + excelAddressBase.Address : str + "#REF!";
          }
          else
            str += token.Value;
        }
        return str;
      }
      catch
      {
        return Formula;
      }
    }

    private delegate string dlgTransl(string part, int row, int col, int rowIncr, int colIncr);
  }
}
