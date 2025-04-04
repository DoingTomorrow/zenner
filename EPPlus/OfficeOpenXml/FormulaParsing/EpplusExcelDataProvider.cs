// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.EpplusExcelDataProvider
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExcelUtilities;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using OfficeOpenXml.Style.XmlAccess;
using OfficeOpenXml.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing
{
  public class EpplusExcelDataProvider : ExcelDataProvider
  {
    private readonly ExcelPackage _package;
    private ExcelWorksheet _currentWorksheet;
    private RangeAddressFactory _rangeAddressFactory;
    private Dictionary<ulong, ExcelDataProvider.INameInfo> _names = new Dictionary<ulong, ExcelDataProvider.INameInfo>();

    public EpplusExcelDataProvider(ExcelPackage package)
    {
      this._package = package;
      this._rangeAddressFactory = new RangeAddressFactory((ExcelDataProvider) this);
    }

    public override ExcelNamedRangeCollection GetWorksheetNames()
    {
      return this._package.Workbook.Worksheets.First<ExcelWorksheet>().Names;
    }

    public override ExcelNamedRangeCollection GetWorkbookNameValues()
    {
      return this._package.Workbook.Names;
    }

    public override ExcelDataProvider.IRangeInfo GetRange(
      string worksheet,
      int row,
      int column,
      string address)
    {
      ExcelAddress excelAddress = new ExcelAddress(worksheet, address);
      if (excelAddress.Table != null)
        excelAddress.SetRCFromTable(this._package, new ExcelAddressBase(row, column, row, column));
      return (ExcelDataProvider.IRangeInfo) new EpplusExcelDataProvider.RangeInfo(this._package.Workbook.Worksheets[string.IsNullOrEmpty(excelAddress.WorkSheet) ? this._currentWorksheet.Name : excelAddress.WorkSheet], excelAddress._fromRow, excelAddress._fromCol, excelAddress._toRow, excelAddress._toCol);
    }

    public override ExcelDataProvider.INameInfo GetName(string worksheet, string name)
    {
      ExcelNamedRange name1;
      ExcelWorksheet excelWorksheet;
      if (string.IsNullOrEmpty(worksheet))
      {
        if (!this._package._workbook.Names.ContainsKey(name))
          return (ExcelDataProvider.INameInfo) null;
        name1 = this._package._workbook.Names[name];
        excelWorksheet = (ExcelWorksheet) null;
      }
      else
      {
        excelWorksheet = this._package._workbook.Worksheets[worksheet];
        if (excelWorksheet != null && excelWorksheet.Names.ContainsKey(name))
        {
          name1 = this._package._workbook.Names[name];
        }
        else
        {
          if (!this._package._workbook.Names.ContainsKey(name))
            return (ExcelDataProvider.INameInfo) null;
          name1 = this._package._workbook.Names[name];
        }
      }
      ulong cellId = ExcelCellBase.GetCellID(name1.LocalSheetId, name1.Index, 0);
      if (this._names.ContainsKey(cellId))
        return this._names[cellId];
      EpplusExcelDataProvider.NameInfo name2 = new EpplusExcelDataProvider.NameInfo()
      {
        Id = cellId,
        Name = name,
        Worksheet = name1.Worksheet == null ? name1._ws : name1.Worksheet.Name,
        Formula = name1.Formula
      };
      name2.Value = name1._fromRow <= 0 ? name1.Value : (object) new EpplusExcelDataProvider.RangeInfo(name1.Worksheet ?? excelWorksheet, name1._fromRow, name1._fromCol, name1._toRow, name1._toCol);
      this._names.Add(cellId, (ExcelDataProvider.INameInfo) name2);
      return (ExcelDataProvider.INameInfo) name2;
    }

    public override IEnumerable<object> GetRangeValues(string address)
    {
      this.SetCurrentWorksheet(ExcelAddressInfo.Parse(address));
      ExcelAddress excelAddress = new ExcelAddress(address);
      return (IEnumerable<object>) new CellsStoreEnumerator<object>(this._package.Workbook.Worksheets[string.IsNullOrEmpty(excelAddress.WorkSheet) ? this._currentWorksheet.Name : excelAddress.WorkSheet]._values, excelAddress._fromRow, excelAddress._fromCol, excelAddress._toRow, excelAddress._toCol);
    }

    public object GetValue(int row, int column)
    {
      return this._currentWorksheet._values.GetValue(row, column);
    }

    public bool IsMerged(int row, int column)
    {
      return this._currentWorksheet._flags.GetFlagValue(row, column, CellFlags.Merged);
    }

    public bool IsHidden(int row, int column)
    {
      return this._currentWorksheet.Column(column).Hidden || this._currentWorksheet.Column(column).Width == 0.0 || this._currentWorksheet.Row(row).Hidden || this._currentWorksheet.Row(column).Height == 0.0;
    }

    public override object GetCellValue(string sheetName, int row, int col)
    {
      this.SetCurrentWorksheet(sheetName);
      return this._currentWorksheet._values.GetValue(row, col);
    }

    private void SetCurrentWorksheet(ExcelAddressInfo addressInfo)
    {
      if (addressInfo.WorksheetIsSpecified)
      {
        this._currentWorksheet = this._package.Workbook.Worksheets[addressInfo.Worksheet];
      }
      else
      {
        if (this._currentWorksheet != null)
          return;
        this._currentWorksheet = this._package.Workbook.Worksheets.First<ExcelWorksheet>();
      }
    }

    private void SetCurrentWorksheet(string worksheetName)
    {
      if (!string.IsNullOrEmpty(worksheetName))
        this._currentWorksheet = this._package.Workbook.Worksheets[worksheetName];
      else
        this._currentWorksheet = this._package.Workbook.Worksheets.First<ExcelWorksheet>();
    }

    public override void Dispose() => this._package.Dispose();

    public override int ExcelMaxColumns => 16384;

    public override int ExcelMaxRows => 1048576;

    public override string GetRangeFormula(string worksheetName, int row, int column)
    {
      this.SetCurrentWorksheet(worksheetName);
      return this._currentWorksheet.GetFormula(row, column);
    }

    public override object GetRangeValue(string worksheetName, int row, int column)
    {
      this.SetCurrentWorksheet(worksheetName);
      return this._currentWorksheet.GetValue(row, column);
    }

    public override string GetFormat(object value, string format)
    {
      ExcelStyles styles = this._package.Workbook.Styles;
      ExcelNumberFormatXml.ExcelFormatTranslator nf = (ExcelNumberFormatXml.ExcelFormatTranslator) null;
      foreach (ExcelNumberFormatXml numberFormat in styles.NumberFormats)
      {
        if (numberFormat.Format == format)
        {
          nf = numberFormat.FormatTranslator;
          break;
        }
      }
      if (nf == null)
        nf = new ExcelNumberFormatXml.ExcelFormatTranslator(format, -1);
      return ExcelRangeBase.FormatValue(value, nf, format, nf.NetFormat);
    }

    public override List<Token> GetRangeFormulaTokens(string worksheetName, int row, int column)
    {
      return this._package.Workbook.Worksheets[worksheetName]._formulaTokens.GetValue(row, column);
    }

    public override bool IsRowHidden(string worksheetName, int row)
    {
      return this._package.Workbook.Worksheets[worksheetName].Row(row).Height == 0.0 || this._package.Workbook.Worksheets[worksheetName].Row(row).Hidden;
    }

    public class RangeInfo : 
      ExcelDataProvider.IRangeInfo,
      IEnumerator<ExcelDataProvider.ICellInfo>,
      IDisposable,
      IEnumerator,
      IEnumerable<ExcelDataProvider.ICellInfo>,
      IEnumerable
    {
      internal ExcelWorksheet _ws;
      private CellsStoreEnumerator<object> _values;
      private int _fromRow;
      private int _toRow;
      private int _fromCol;
      private int _toCol;
      private int _cellCount;
      private ExcelAddressBase _address;
      private ExcelDataProvider.ICellInfo _cell;

      public RangeInfo(ExcelWorksheet ws, int fromRow, int fromCol, int toRow, int toCol)
      {
        this._ws = ws;
        this._fromRow = fromRow;
        this._fromCol = fromCol;
        this._toRow = toRow;
        this._toCol = toCol;
        this._address = new ExcelAddressBase(this._fromRow, this._fromCol, this._toRow, this._toCol);
        this._address._ws = ws.Name;
        this._values = new CellsStoreEnumerator<object>(ws._values, this._fromRow, this._fromCol, this._toRow, this._toCol);
        this._cell = (ExcelDataProvider.ICellInfo) new EpplusExcelDataProvider.CellInfo(this._ws, this._values);
      }

      public int GetNCells()
      {
        return (this._toRow - this._fromRow + 1) * (this._toCol - this._fromCol + 1);
      }

      public bool IsEmpty
      {
        get
        {
          if (this._cellCount > 0)
            return false;
          if (!this._values.Next())
            return true;
          this._values.Reset();
          return false;
        }
      }

      public bool IsMulti
      {
        get
        {
          if (this._cellCount == 0)
          {
            if (this._values.Next() && this._values.Next())
            {
              this._values.Reset();
              return true;
            }
            this._values.Reset();
            return false;
          }
          return this._cellCount > 1;
        }
      }

      public ExcelDataProvider.ICellInfo Current => this._cell;

      public void Dispose()
      {
      }

      object IEnumerator.Current => (object) this;

      public bool MoveNext()
      {
        ++this._cellCount;
        return this._values.MoveNext();
      }

      public void Reset() => this._values.Init();

      public bool NextCell()
      {
        ++this._cellCount;
        return this._values.MoveNext();
      }

      public IEnumerator<ExcelDataProvider.ICellInfo> GetEnumerator()
      {
        this.Reset();
        return (IEnumerator<ExcelDataProvider.ICellInfo>) this;
      }

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this;

      public ExcelAddressBase Address => this._address;

      public object GetValue(int row, int col) => this._ws.GetValue(row, col);

      public object GetOffset(int rowOffset, int colOffset)
      {
        return this._values.Row < this._fromRow || this._values.Column < this._fromCol ? this._ws.GetValue(this._fromRow + rowOffset, this._fromCol + colOffset) : this._ws.GetValue(this._values.Row + rowOffset, this._values.Column + colOffset);
      }
    }

    public class CellInfo : ExcelDataProvider.ICellInfo
    {
      private ExcelWorksheet _ws;
      private CellsStoreEnumerator<object> _values;

      internal CellInfo(ExcelWorksheet ws, CellsStoreEnumerator<object> values)
      {
        this._ws = ws;
        this._values = values;
      }

      public string Address => this._values.CellAddress;

      public int Row => this._values.Row;

      public int Column => this._values.Column;

      public string Formula => this._ws.GetFormula(this._values.Row, this._values.Column);

      public object Value => this._values.Value;

      public double ValueDouble => ConvertUtil.GetValueDouble(this._values.Value, true);

      public double ValueDoubleLogical => ConvertUtil.GetValueDouble(this._values.Value);

      public bool IsHiddenRow
      {
        get
        {
          if (!(this._ws._values.GetValue(this._values.Row, 0) is RowInternal rowInternal))
            return false;
          return rowInternal.Hidden || rowInternal.Height == 0.0;
        }
      }

      public bool IsExcelError => ExcelErrorValue.Values.IsErrorValue(this._values.Value);

      public IList<Token> Tokens
      {
        get
        {
          return (IList<Token>) this._ws._formulaTokens.GetValue(this._values.Row, this._values.Column);
        }
      }
    }

    public class NameInfo : ExcelDataProvider.INameInfo
    {
      public ulong Id { get; set; }

      public string Worksheet { get; set; }

      public string Name { get; set; }

      public string Formula { get; set; }

      public IList<Token> Tokens { get; internal set; }

      public object Value { get; set; }
    }
  }
}
