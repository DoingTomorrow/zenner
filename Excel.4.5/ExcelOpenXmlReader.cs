// Decompiled with JetBrains decompiler
// Type: Excel.ExcelOpenXmlReader
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using Excel.Core;
using Excel.Core.OpenXmlFormat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Xml;

#nullable disable
namespace Excel
{
  public class ExcelOpenXmlReader : IExcelDataReader, IDataReader, IDisposable, IDataRecord
  {
    private const string COLUMN = "Column";
    private XlsxWorkbook _workbook;
    private bool _isValid;
    private bool _isClosed;
    private bool _isFirstRead;
    private string _exceptionMessage;
    private int _depth;
    private int _resultIndex;
    private int _emptyRowCount;
    private ZipWorker _zipWorker;
    private XmlReader _xmlReader;
    private Stream _sheetStream;
    private object[] _cellsValues;
    private object[] _savedCellsValues;
    private bool disposed;
    private bool _isFirstRowAsColumnNames;
    private string instanceId = Guid.NewGuid().ToString();
    private List<int> _defaultDateTimeStyles;
    private string _namespaceUri;

    internal ExcelOpenXmlReader()
    {
      this._isValid = true;
      this._isFirstRead = true;
      this._defaultDateTimeStyles = new List<int>((IEnumerable<int>) new int[12]
      {
        14,
        15,
        16,
        17,
        18,
        19,
        20,
        21,
        22,
        45,
        46,
        47
      });
    }

    private void ReadGlobals()
    {
      this._workbook = new XlsxWorkbook(this._zipWorker.GetWorkbookStream(), this._zipWorker.GetWorkbookRelsStream(), this._zipWorker.GetSharedStringsStream(), this._zipWorker.GetStylesStream());
      this.CheckDateTimeNumFmts(this._workbook.Styles.NumFmts);
    }

    private void CheckDateTimeNumFmts(List<XlsxNumFmt> list)
    {
      if (list.Count == 0)
        return;
      foreach (XlsxNumFmt xlsxNumFmt in list)
      {
        if (!string.IsNullOrEmpty(xlsxNumFmt.FormatCode))
        {
          string str = xlsxNumFmt.FormatCode.ToLower();
          int startIndex;
          while ((startIndex = str.IndexOf('"')) > 0)
          {
            int num = str.IndexOf('"', startIndex + 1);
            if (num > 0)
              str = str.Remove(startIndex, num - startIndex + 1);
          }
          if (new FormatReader() { FormatString = str }.IsDateFormatString())
            this._defaultDateTimeStyles.Add(xlsxNumFmt.Id);
        }
      }
    }

    private void ReadSheetGlobals(XlsxWorksheet sheet)
    {
      if (this._xmlReader != null)
        this._xmlReader.Close();
      if (this._sheetStream != null)
        this._sheetStream.Close();
      this._sheetStream = this._zipWorker.GetWorksheetStream(sheet.Path);
      if (this._sheetStream == null)
        return;
      this._xmlReader = XmlReader.Create(this._sheetStream);
      int rows = 0;
      int cols = 0;
      this._namespaceUri = (string) null;
      int num = 0;
      while (this._xmlReader.Read())
      {
        if (this._xmlReader.NodeType == XmlNodeType.Element && this._xmlReader.LocalName == "worksheet")
          this._namespaceUri = this._xmlReader.NamespaceURI;
        if (this._xmlReader.NodeType == XmlNodeType.Element && this._xmlReader.LocalName == "dimension")
        {
          string attribute = this._xmlReader.GetAttribute("ref");
          sheet.Dimension = new XlsxDimension(attribute);
          break;
        }
        if (this._xmlReader.NodeType == XmlNodeType.Element && this._xmlReader.LocalName == "row")
          ++rows;
        if (sheet.Dimension == null && cols == 0 && this._xmlReader.NodeType == XmlNodeType.Element && this._xmlReader.LocalName == "c")
        {
          string attribute = this._xmlReader.GetAttribute("r");
          if (attribute != null)
          {
            int[] columnAndRow = ReferenceHelper.ReferenceToColumnAndRow(attribute);
            if (columnAndRow[1] > num)
              num = columnAndRow[1];
          }
        }
      }
      if (sheet.Dimension == null)
      {
        if (cols == 0)
          cols = num;
        if (rows == 0 || cols == 0)
        {
          sheet.IsEmpty = true;
          return;
        }
        sheet.Dimension = new XlsxDimension(rows, cols);
        this._xmlReader.Close();
        this._sheetStream.Close();
        this._sheetStream = this._zipWorker.GetWorksheetStream(sheet.Path);
        this._xmlReader = XmlReader.Create(this._sheetStream);
      }
      this._xmlReader.ReadToFollowing("sheetData", this._namespaceUri);
      if (!this._xmlReader.IsEmptyElement)
        return;
      sheet.IsEmpty = true;
    }

    private bool ReadSheetRow(XlsxWorksheet sheet)
    {
      if (this._xmlReader == null)
        return false;
      if (this._emptyRowCount != 0)
      {
        this._cellsValues = new object[sheet.ColumnsCount];
        --this._emptyRowCount;
        ++this._depth;
        return true;
      }
      if (this._savedCellsValues != null)
      {
        this._cellsValues = this._savedCellsValues;
        this._savedCellsValues = (object[]) null;
        ++this._depth;
        return true;
      }
      if (this._xmlReader.NodeType == XmlNodeType.Element && this._xmlReader.LocalName == "row" || this._xmlReader.ReadToFollowing("row", this._namespaceUri))
      {
        this._cellsValues = new object[sheet.ColumnsCount];
        int num = int.Parse(this._xmlReader.GetAttribute("r"));
        if (num != this._depth + 1 && num != this._depth + 1)
          this._emptyRowCount = num - this._depth - 1;
        bool flag = false;
        string s = string.Empty;
        string str = string.Empty;
        string empty = string.Empty;
        int val1 = 0;
        int val2 = 0;
        while (this._xmlReader.Read() && this._xmlReader.Depth != 2)
        {
          if (this._xmlReader.NodeType == XmlNodeType.Element)
          {
            flag = false;
            if (this._xmlReader.LocalName == "c")
            {
              s = this._xmlReader.GetAttribute("s");
              str = this._xmlReader.GetAttribute("t");
              XlsxDimension.XlsxDim(this._xmlReader.GetAttribute("r"), out val1, out val2);
            }
            else if (this._xmlReader.LocalName == "v" || this._xmlReader.LocalName == "t")
              flag = true;
          }
          if (this._xmlReader.NodeType == XmlNodeType.Text && flag)
          {
            object obj = (object) this._xmlReader.Value;
            NumberStyles style = NumberStyles.Any;
            CultureInfo invariantCulture = CultureInfo.InvariantCulture;
            double result;
            if (double.TryParse(obj.ToString(), style, (IFormatProvider) invariantCulture, out result))
              obj = (object) result;
            switch (str)
            {
              case "s":
                obj = (object) Helpers.ConvertEscapeChars(this._workbook.SST[int.Parse(obj.ToString())]);
                break;
              default:
                if (str != null && str == "inlineStr")
                {
                  obj = (object) Helpers.ConvertEscapeChars(obj.ToString());
                  break;
                }
                if (str == "b")
                {
                  obj = (object) (this._xmlReader.Value == "1");
                  break;
                }
                if (s != null)
                {
                  XlsxXf cellXf = this._workbook.Styles.CellXfs[int.Parse(s)];
                  if (cellXf.ApplyNumberFormat && obj != null && obj.ToString() != string.Empty && this.IsDateTimeStyle(cellXf.NumFmtId))
                  {
                    obj = Helpers.ConvertFromOATime(result);
                    break;
                  }
                  if (cellXf.NumFmtId == 49)
                  {
                    obj = (object) obj.ToString();
                    break;
                  }
                  break;
                }
                break;
            }
            if (val1 - 1 < this._cellsValues.Length)
              this._cellsValues[val1 - 1] = obj;
          }
        }
        if (this._emptyRowCount > 0)
        {
          this._savedCellsValues = this._cellsValues;
          return this.ReadSheetRow(sheet);
        }
        ++this._depth;
        return true;
      }
      this._xmlReader.Close();
      if (this._sheetStream != null)
        this._sheetStream.Close();
      return false;
    }

    private bool InitializeSheetRead()
    {
      if (this.ResultsCount <= 0)
        return false;
      this.ReadSheetGlobals(this._workbook.Sheets[this._resultIndex]);
      if (this._workbook.Sheets[this._resultIndex].Dimension == null)
        return false;
      this._isFirstRead = false;
      this._depth = 0;
      this._emptyRowCount = 0;
      return true;
    }

    private bool IsDateTimeStyle(int styleId) => this._defaultDateTimeStyles.Contains(styleId);

    public void Initialize(Stream fileStream)
    {
      this._zipWorker = new ZipWorker();
      this._zipWorker.Extract(fileStream);
      if (!this._zipWorker.IsValid)
      {
        this._isValid = false;
        this._exceptionMessage = this._zipWorker.ExceptionMessage;
        this.Close();
      }
      else
        this.ReadGlobals();
    }

    public DataSet AsDataSet() => this.AsDataSet(true);

    public DataSet AsDataSet(bool convertOADateTime)
    {
      if (!this._isValid)
        return (DataSet) null;
      DataSet dataset = new DataSet();
      for (int index1 = 0; index1 < this._workbook.Sheets.Count; ++index1)
      {
        DataTable table = new DataTable(this._workbook.Sheets[index1].Name);
        this.ReadSheetGlobals(this._workbook.Sheets[index1]);
        if (this._workbook.Sheets[index1].Dimension != null)
        {
          this._depth = 0;
          this._emptyRowCount = 0;
          if (!this._isFirstRowAsColumnNames)
          {
            for (int index2 = 0; index2 < this._workbook.Sheets[index1].ColumnsCount; ++index2)
              table.Columns.Add((string) null, typeof (object));
          }
          else if (this.ReadSheetRow(this._workbook.Sheets[index1]))
          {
            for (int index3 = 0; index3 < this._cellsValues.Length; ++index3)
            {
              if (this._cellsValues[index3] != null && this._cellsValues[index3].ToString().Length > 0)
                Helpers.AddColumnHandleDuplicate(table, this._cellsValues[index3].ToString());
              else
                Helpers.AddColumnHandleDuplicate(table, "Column" + (object) index3);
            }
          }
          else
            continue;
          table.BeginLoadData();
          while (this.ReadSheetRow(this._workbook.Sheets[index1]))
            table.Rows.Add(this._cellsValues);
          if (table.Rows.Count > 0)
            dataset.Tables.Add(table);
          table.EndLoadData();
        }
      }
      dataset.AcceptChanges();
      Helpers.FixDataTypes(dataset);
      return dataset;
    }

    public bool IsFirstRowAsColumnNames
    {
      get => this._isFirstRowAsColumnNames;
      set => this._isFirstRowAsColumnNames = value;
    }

    public bool IsValid => this._isValid;

    public string ExceptionMessage => this._exceptionMessage;

    public string Name
    {
      get
      {
        return this._resultIndex < 0 || this._resultIndex >= this.ResultsCount ? (string) null : this._workbook.Sheets[this._resultIndex].Name;
      }
    }

    public void Close()
    {
      this._isClosed = true;
      if (this._xmlReader != null)
        this._xmlReader.Close();
      if (this._sheetStream != null)
        this._sheetStream.Close();
      if (this._zipWorker == null)
        return;
      this._zipWorker.Dispose();
    }

    public int Depth => this._depth;

    public int ResultsCount => this._workbook != null ? this._workbook.Sheets.Count : -1;

    public bool IsClosed => this._isClosed;

    public bool NextResult()
    {
      if (this._resultIndex >= this.ResultsCount - 1)
        return false;
      ++this._resultIndex;
      this._isFirstRead = true;
      this._savedCellsValues = (object[]) null;
      return true;
    }

    public bool Read()
    {
      return this._isValid && (!this._isFirstRead || this.InitializeSheetRead()) && this.ReadSheetRow(this._workbook.Sheets[this._resultIndex]);
    }

    public int FieldCount
    {
      get
      {
        return this._resultIndex < 0 || this._resultIndex >= this.ResultsCount ? -1 : this._workbook.Sheets[this._resultIndex].ColumnsCount;
      }
    }

    public bool GetBoolean(int i)
    {
      return !this.IsDBNull(i) && bool.Parse(this._cellsValues[i].ToString());
    }

    public DateTime GetDateTime(int i)
    {
      if (this.IsDBNull(i))
        return DateTime.MinValue;
      try
      {
        return (DateTime) this._cellsValues[i];
      }
      catch (InvalidCastException ex)
      {
        return DateTime.MinValue;
      }
    }

    public Decimal GetDecimal(int i)
    {
      return this.IsDBNull(i) ? Decimal.MinValue : Decimal.Parse(this._cellsValues[i].ToString());
    }

    public double GetDouble(int i)
    {
      return this.IsDBNull(i) ? double.MinValue : double.Parse(this._cellsValues[i].ToString());
    }

    public float GetFloat(int i)
    {
      return this.IsDBNull(i) ? float.MinValue : float.Parse(this._cellsValues[i].ToString());
    }

    public short GetInt16(int i)
    {
      return this.IsDBNull(i) ? short.MinValue : short.Parse(this._cellsValues[i].ToString());
    }

    public int GetInt32(int i)
    {
      return this.IsDBNull(i) ? int.MinValue : int.Parse(this._cellsValues[i].ToString());
    }

    public long GetInt64(int i)
    {
      return this.IsDBNull(i) ? long.MinValue : long.Parse(this._cellsValues[i].ToString());
    }

    public string GetString(int i)
    {
      return this.IsDBNull(i) ? (string) null : this._cellsValues[i].ToString();
    }

    public object GetValue(int i) => this._cellsValues[i];

    public bool IsDBNull(int i)
    {
      return this._cellsValues[i] == null || DBNull.Value == this._cellsValues[i];
    }

    public object this[int i] => this._cellsValues[i];

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        if (this._xmlReader != null)
          ((IDisposable) this._xmlReader).Dispose();
        if (this._sheetStream != null)
          this._sheetStream.Dispose();
        if (this._zipWorker != null)
          this._zipWorker.Dispose();
      }
      this._zipWorker = (ZipWorker) null;
      this._xmlReader = (XmlReader) null;
      this._sheetStream = (Stream) null;
      this._workbook = (XlsxWorkbook) null;
      this._cellsValues = (object[]) null;
      this._savedCellsValues = (object[]) null;
      this.disposed = true;
    }

    ~ExcelOpenXmlReader() => this.Dispose(false);

    public DataTable GetSchemaTable() => throw new NotSupportedException();

    public int RecordsAffected => throw new NotSupportedException();

    public byte GetByte(int i) => throw new NotSupportedException();

    public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
    {
      throw new NotSupportedException();
    }

    public char GetChar(int i) => throw new NotSupportedException();

    public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
    {
      throw new NotSupportedException();
    }

    public IDataReader GetData(int i) => throw new NotSupportedException();

    public string GetDataTypeName(int i) => throw new NotSupportedException();

    public Type GetFieldType(int i) => throw new NotSupportedException();

    public Guid GetGuid(int i) => throw new NotSupportedException();

    public string GetName(int i) => throw new NotSupportedException();

    public int GetOrdinal(string name) => throw new NotSupportedException();

    public int GetValues(object[] values) => throw new NotSupportedException();

    public object this[string name] => throw new NotSupportedException();
  }
}
