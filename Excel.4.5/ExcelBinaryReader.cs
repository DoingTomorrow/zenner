// Decompiled with JetBrains decompiler
// Type: Excel.ExcelBinaryReader
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using Excel.Core;
using Excel.Core.BinaryFormat;
using Excel.Exceptions;
using Excel.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

#nullable disable
namespace Excel
{
  public class ExcelBinaryReader : IExcelDataReader, IDataReader, IDisposable, IDataRecord
  {
    private const string WORKBOOK = "Workbook";
    private const string BOOK = "Book";
    private const string COLUMN = "Column";
    private Stream m_file;
    private XlsHeader m_hdr;
    private List<XlsWorksheet> m_sheets;
    private XlsBiffStream m_stream;
    private DataSet m_workbookData;
    private XlsWorkbookGlobals m_globals;
    private ushort m_version;
    private bool m_ConvertOADate;
    private Encoding m_encoding;
    private bool m_isValid;
    private bool m_isClosed;
    private readonly Encoding m_Default_Encoding = Encoding.UTF8;
    private string m_exceptionMessage;
    private object[] m_cellsValues;
    private uint[] m_dbCellAddrs;
    private int m_dbCellAddrsIndex;
    private bool m_canRead;
    private int m_SheetIndex;
    private int m_depth;
    private int m_cellOffset;
    private int m_maxCol;
    private int m_maxRow;
    private bool m_noIndex;
    private XlsBiffRow m_currentRowRecord;
    private readonly ReadOption m_ReadOption;
    private bool m_IsFirstRead;
    private bool _isFirstRowAsColumnNames;
    private bool disposed;

    internal ExcelBinaryReader()
    {
      this.m_encoding = this.m_Default_Encoding;
      this.m_version = (ushort) 1536;
      this.m_isValid = true;
      this.m_SheetIndex = -1;
      this.m_IsFirstRead = true;
    }

    internal ExcelBinaryReader(ReadOption readOption)
      : this()
    {
      this.m_ReadOption = readOption;
    }

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
        if (this.m_workbookData != null)
          this.m_workbookData.Dispose();
        if (this.m_sheets != null)
          this.m_sheets.Clear();
      }
      this.m_workbookData = (DataSet) null;
      this.m_sheets = (List<XlsWorksheet>) null;
      this.m_stream = (XlsBiffStream) null;
      this.m_globals = (XlsWorkbookGlobals) null;
      this.m_encoding = (Encoding) null;
      this.m_hdr = (XlsHeader) null;
      this.disposed = true;
    }

    ~ExcelBinaryReader() => this.Dispose(false);

    private int findFirstDataCellOffset(int startOffset)
    {
      XlsBiffRecord xlsBiffRecord;
      for (xlsBiffRecord = this.m_stream.ReadAt(startOffset); !(xlsBiffRecord is XlsBiffDbCell); xlsBiffRecord = this.m_stream.Read())
      {
        if (this.m_stream.Position >= this.m_stream.Size || xlsBiffRecord is XlsBiffEOF)
          return -1;
      }
      int rowAddress = ((XlsBiffDbCell) xlsBiffRecord).RowAddress;
      while (this.m_stream.ReadAt(rowAddress) is XlsBiffRow xlsBiffRow)
      {
        rowAddress += xlsBiffRow.Size;
        if (xlsBiffRow == null)
          break;
      }
      return rowAddress;
    }

    private void readWorkBookGlobals()
    {
      try
      {
        this.m_hdr = XlsHeader.ReadHeader(this.m_file);
      }
      catch (HeaderException ex)
      {
        this.fail(ex.Message);
        return;
      }
      catch (FormatException ex)
      {
        this.fail(ex.Message);
        return;
      }
      XlsRootDirectory rootDir = new XlsRootDirectory(this.m_hdr);
      XlsDirectoryEntry xlsDirectoryEntry = rootDir.FindEntry("Workbook") ?? rootDir.FindEntry("Book");
      if (xlsDirectoryEntry == null)
        this.fail("Error: Neither stream 'Workbook' nor 'Book' was found in file.");
      else if (xlsDirectoryEntry.EntryType != STGTY.STGTY_STREAM)
      {
        this.fail("Error: Workbook directory entry is not a Stream.");
      }
      else
      {
        this.m_stream = new XlsBiffStream(this.m_hdr, xlsDirectoryEntry.StreamFirstSector, xlsDirectoryEntry.IsEntryMiniStream, rootDir, this);
        this.m_globals = new XlsWorkbookGlobals();
        this.m_stream.Seek(0, SeekOrigin.Begin);
        if (!(this.m_stream.Read() is XlsBiffBOF xlsBiffBof) || xlsBiffBof.Type != BIFFTYPE.WorkbookGlobals)
        {
          this.fail("Error reading Workbook Globals - Stream has invalid data.");
        }
        else
        {
          bool flag = false;
          this.m_version = xlsBiffBof.Version;
          this.m_sheets = new List<XlsWorksheet>();
          XlsBiffRecord fragment;
          while ((fragment = this.m_stream.Read()) != null)
          {
            BIFFRECORDTYPE id = fragment.ID;
            if ((uint) id <= 140U)
            {
              if ((uint) id <= 49U)
              {
                if ((uint) id <= 19U)
                {
                  switch (id)
                  {
                    case BIFFRECORDTYPE.EOF:
                      if (this.m_globals.SST == null)
                        return;
                      this.m_globals.SST.ReadStrings();
                      return;
                    default:
                      continue;
                  }
                }
                else
                {
                  switch (id)
                  {
                    case BIFFRECORDTYPE.FORMAT_V23:
                      XlsBiffFormatString biffFormatString1 = (XlsBiffFormatString) fragment;
                      biffFormatString1.UseEncoding = this.m_encoding;
                      this.m_globals.Formats.Add((ushort) this.m_globals.Formats.Count, biffFormatString1);
                      continue;
                    case BIFFRECORDTYPE.FONT:
                      break;
                    default:
                      continue;
                  }
                }
              }
              else if ((uint) id <= 67U)
              {
                switch (id)
                {
                  case BIFFRECORDTYPE.CONTINUE:
                    if (flag)
                    {
                      this.m_globals.SST.Append((XlsBiffContinue) fragment);
                      continue;
                    }
                    continue;
                  case BIFFRECORDTYPE.CODEPAGE:
                    this.m_globals.CodePage = (XlsBiffSimpleValueRecord) fragment;
                    try
                    {
                      this.m_encoding = Encoding.GetEncoding((int) this.m_globals.CodePage.Value);
                      continue;
                    }
                    catch (ArgumentException ex)
                    {
                      continue;
                    }
                  case BIFFRECORDTYPE.XF_V2:
                    goto label_37;
                  default:
                    continue;
                }
              }
              else
              {
                switch (id)
                {
                  case BIFFRECORDTYPE.BOUNDSHEET:
                    XlsBiffBoundSheet refSheet = (XlsBiffBoundSheet) fragment;
                    if (refSheet.Type == XlsBiffBoundSheet.SheetType.Worksheet)
                    {
                      refSheet.IsV8 = this.isV8();
                      refSheet.UseEncoding = this.m_encoding;
                      LogManager.Log<ExcelBinaryReader>(this).Debug("BOUNDSHEET IsV8={0}", (object) refSheet.IsV8);
                      this.m_sheets.Add(new XlsWorksheet(this.m_globals.Sheets.Count, refSheet));
                      this.m_globals.Sheets.Add(refSheet);
                      continue;
                    }
                    continue;
                  case BIFFRECORDTYPE.COUNTRY:
                    this.m_globals.Country = fragment;
                    continue;
                  default:
                    continue;
                }
              }
            }
            else if ((uint) id <= (uint) byte.MaxValue)
            {
              if ((uint) id <= 225U)
              {
                switch (id)
                {
                  case BIFFRECORDTYPE.MMS:
                    this.m_globals.MMS = fragment;
                    continue;
                  case BIFFRECORDTYPE.XF:
                    goto label_37;
                  case BIFFRECORDTYPE.INTERFACEHDR:
                    this.m_globals.InterfaceHdr = (XlsBiffInterfaceHdr) fragment;
                    continue;
                  default:
                    continue;
                }
              }
              else
              {
                switch (id)
                {
                  case BIFFRECORDTYPE.SST:
                    this.m_globals.SST = (XlsBiffSST) fragment;
                    flag = true;
                    continue;
                  case BIFFRECORDTYPE.EXTSST:
                    this.m_globals.ExtSST = fragment;
                    flag = false;
                    continue;
                  default:
                    continue;
                }
              }
            }
            else if ((uint) id <= 561U)
            {
              if (id == BIFFRECORDTYPE.PROT4REVPASSWORD || id != BIFFRECORDTYPE.FONT_V34)
                continue;
            }
            else
            {
              switch (id)
              {
                case BIFFRECORDTYPE.XF_V3:
                case BIFFRECORDTYPE.XF_V4:
                  goto label_37;
                case BIFFRECORDTYPE.FORMAT:
                  XlsBiffFormatString biffFormatString2 = (XlsBiffFormatString) fragment;
                  this.m_globals.Formats.Add(biffFormatString2.Index, biffFormatString2);
                  continue;
                default:
                  continue;
              }
            }
            this.m_globals.Fonts.Add(fragment);
            continue;
label_37:
            this.m_globals.ExtendedFormats.Add(fragment);
          }
        }
      }
    }

    private bool readWorkSheetGlobals(XlsWorksheet sheet, out XlsBiffIndex idx, out XlsBiffRow row)
    {
      idx = (XlsBiffIndex) null;
      row = (XlsBiffRow) null;
      this.m_stream.Seek((int) sheet.DataOffset, SeekOrigin.Begin);
      if (!(this.m_stream.Read() is XlsBiffBOF xlsBiffBof) || xlsBiffBof.Type != BIFFTYPE.Worksheet)
        return false;
      XlsBiffRecord xlsBiffRecord1 = this.m_stream.Read();
      if (xlsBiffRecord1 == null)
        return false;
      if (xlsBiffRecord1 is XlsBiffIndex)
        idx = xlsBiffRecord1 as XlsBiffIndex;
      else if (xlsBiffRecord1 is XlsBiffUncalced)
        idx = this.m_stream.Read() as XlsBiffIndex;
      if (idx != null)
      {
        idx.IsV8 = this.isV8();
        LogManager.Log<ExcelBinaryReader>(this).Debug("INDEX IsV8={0}", (object) idx.IsV8);
      }
      XlsBiffDimensions xlsBiffDimensions = (XlsBiffDimensions) null;
      XlsBiffRecord xlsBiffRecord2;
      do
      {
        xlsBiffRecord2 = this.m_stream.Read();
        if (xlsBiffRecord2.ID == BIFFRECORDTYPE.DIMENSIONS)
        {
          xlsBiffDimensions = (XlsBiffDimensions) xlsBiffRecord2;
          break;
        }
      }
      while (xlsBiffRecord2 != null && xlsBiffRecord2.ID != BIFFRECORDTYPE.ROW);
      if (xlsBiffRecord2.ID == BIFFRECORDTYPE.ROW)
        row = (XlsBiffRow) xlsBiffRecord2;
      XlsBiffRow xlsBiffRow;
      XlsBiffRecord xlsBiffRecord3;
      for (xlsBiffRow = (XlsBiffRow) null; xlsBiffRow == null && this.m_stream.Position < this.m_stream.Size; xlsBiffRow = xlsBiffRecord3 as XlsBiffRow)
      {
        xlsBiffRecord3 = this.m_stream.Read();
        LogManager.Log<ExcelBinaryReader>(this).Debug("finding rowRecord offset {0}, rec: {1}", (object) xlsBiffRecord3.Offset, (object) xlsBiffRecord3.ID);
        if (xlsBiffRecord3 is XlsBiffEOF)
          break;
      }
      if (xlsBiffRow != null)
        LogManager.Log<ExcelBinaryReader>(this).Debug("Got row {0}, rec: id={1},rowindex={2}, rowColumnStart={3}, rowColumnEnd={4}", (object) xlsBiffRow.Offset, (object) xlsBiffRow.ID, (object) xlsBiffRow.RowIndex, (object) xlsBiffRow.FirstDefinedColumn, (object) xlsBiffRow.LastDefinedColumn);
      row = xlsBiffRow;
      if (xlsBiffDimensions != null)
      {
        xlsBiffDimensions.IsV8 = this.isV8();
        LogManager.Log<ExcelBinaryReader>(this).Debug("dims IsV8={0}", (object) xlsBiffDimensions.IsV8);
        this.m_maxCol = (int) xlsBiffDimensions.LastColumn - 1;
        if (this.m_maxCol <= 0 && xlsBiffRow != null)
          this.m_maxCol = (int) xlsBiffRow.LastDefinedColumn;
        this.m_maxRow = (int) xlsBiffDimensions.LastRow;
        sheet.Dimensions = xlsBiffDimensions;
      }
      else
      {
        this.m_maxCol = 256;
        this.m_maxRow = (int) idx.LastExistingRow;
      }
      if (idx != null && idx.LastExistingRow <= idx.FirstExistingRow || row == null)
        return false;
      this.m_depth = 0;
      return true;
    }

    private void DumpBiffRecords()
    {
      int position = this.m_stream.Position;
      XlsBiffRecord xlsBiffRecord;
      do
      {
        xlsBiffRecord = this.m_stream.Read();
        LogManager.Log<ExcelBinaryReader>(this).Debug(xlsBiffRecord.ID.ToString());
      }
      while (xlsBiffRecord != null && this.m_stream.Position < this.m_stream.Size);
      this.m_stream.Seek(position, SeekOrigin.Begin);
    }

    private bool readWorkSheetRow()
    {
      this.m_cellsValues = new object[this.m_maxCol];
      while (this.m_cellOffset < this.m_stream.Size)
      {
        XlsBiffRecord xlsBiffRecord = this.m_stream.ReadAt(this.m_cellOffset);
        this.m_cellOffset += xlsBiffRecord.Size;
        switch (xlsBiffRecord)
        {
          case XlsBiffDbCell _:
            goto label_8;
          case XlsBiffEOF _:
            return false;
          case XlsBiffBlankCell cell:
            if ((int) cell.ColumnIndex < this.m_maxCol)
            {
              if ((int) cell.RowIndex != this.m_depth)
              {
                this.m_cellOffset -= xlsBiffRecord.Size;
                goto label_8;
              }
              else
              {
                this.pushCellValue(cell);
                continue;
              }
            }
            else
              continue;
          default:
            continue;
        }
      }
label_8:
      ++this.m_depth;
      return this.m_depth < this.m_maxRow;
    }

    private DataTable readWholeWorkSheet(XlsWorksheet sheet)
    {
      XlsBiffIndex idx;
      if (!this.readWorkSheetGlobals(sheet, out idx, out this.m_currentRowRecord))
        return (DataTable) null;
      DataTable table = new DataTable(sheet.Name);
      bool triggerCreateColumns = true;
      if (idx != null)
        this.readWholeWorkSheetWithIndex(idx, triggerCreateColumns, table);
      else
        this.readWholeWorkSheetNoIndex(triggerCreateColumns, table);
      table.EndLoadData();
      return table;
    }

    private void readWholeWorkSheetWithIndex(
      XlsBiffIndex idx,
      bool triggerCreateColumns,
      DataTable table)
    {
      this.m_dbCellAddrs = idx.DbCellAddresses;
      for (int index1 = 0; index1 < this.m_dbCellAddrs.Length && this.m_depth != this.m_maxRow; ++index1)
      {
        this.m_cellOffset = this.findFirstDataCellOffset((int) this.m_dbCellAddrs[index1]);
        if (this.m_cellOffset < 0)
          break;
        if (triggerCreateColumns)
        {
          if (this._isFirstRowAsColumnNames && this.readWorkSheetRow() || this._isFirstRowAsColumnNames && this.m_maxRow == 1)
          {
            for (int index2 = 0; index2 < this.m_maxCol; ++index2)
            {
              if (this.m_cellsValues[index2] != null && this.m_cellsValues[index2].ToString().Length > 0)
                Helpers.AddColumnHandleDuplicate(table, this.m_cellsValues[index2].ToString());
              else
                Helpers.AddColumnHandleDuplicate(table, "Column" + (object) index2);
            }
          }
          else
          {
            for (int index3 = 0; index3 < this.m_maxCol; ++index3)
              table.Columns.Add((string) null, typeof (object));
          }
          triggerCreateColumns = false;
          table.BeginLoadData();
        }
        while (this.readWorkSheetRow())
          table.Rows.Add(this.m_cellsValues);
        if (this.m_depth > 0 && (!this._isFirstRowAsColumnNames || this.m_maxRow != 1))
          table.Rows.Add(this.m_cellsValues);
      }
    }

    private void readWholeWorkSheetNoIndex(bool triggerCreateColumns, DataTable table)
    {
      while (this.Read() && this.m_depth != this.m_maxRow)
      {
        bool flag = false;
        if (triggerCreateColumns)
        {
          if (this._isFirstRowAsColumnNames || this._isFirstRowAsColumnNames && this.m_maxRow == 1)
          {
            for (int index = 0; index < this.m_maxCol; ++index)
            {
              if (this.m_cellsValues[index] != null && this.m_cellsValues[index].ToString().Length > 0)
                Helpers.AddColumnHandleDuplicate(table, this.m_cellsValues[index].ToString());
              else
                Helpers.AddColumnHandleDuplicate(table, "Column" + (object) index);
            }
          }
          else
          {
            for (int index = 0; index < this.m_maxCol; ++index)
              table.Columns.Add((string) null, typeof (object));
          }
          triggerCreateColumns = false;
          flag = true;
          table.BeginLoadData();
        }
        if (!flag && this.m_depth > 0 && (!this._isFirstRowAsColumnNames || this.m_maxRow != 1))
          table.Rows.Add(this.m_cellsValues);
      }
      if (this.m_depth <= 0 || this._isFirstRowAsColumnNames && this.m_maxRow == 1)
        return;
      table.Rows.Add(this.m_cellsValues);
    }

    private void pushCellValue(XlsBiffBlankCell cell)
    {
      LogManager.Log<ExcelBinaryReader>(this).Debug("pushCellValue {0}", (object) cell.ID);
      BIFFRECORDTYPE id = cell.ID;
      if ((uint) id <= 214U)
      {
        switch (id)
        {
          case BIFFRECORDTYPE.BLANK_OLD:
            return;
          case BIFFRECORDTYPE.INTEGER_OLD:
            break;
          case BIFFRECORDTYPE.NUMBER_OLD:
            goto label_13;
          case BIFFRECORDTYPE.LABEL_OLD:
          case BIFFRECORDTYPE.RSTRING:
            goto label_14;
          case BIFFRECORDTYPE.BOOLERR_OLD:
            if (cell.ReadByte(8) != (byte) 0)
              return;
            this.m_cellsValues[(int) cell.ColumnIndex] = (object) (cell.ReadByte(7) != (byte) 0);
            return;
          case BIFFRECORDTYPE.FORMULA_OLD:
            goto label_21;
          case BIFFRECORDTYPE.MULRK:
            XlsBiffMulRKCell xlsBiffMulRkCell = (XlsBiffMulRKCell) cell;
            for (ushort columnIndex = cell.ColumnIndex; (int) columnIndex <= (int) xlsBiffMulRkCell.LastColumnIndex; ++columnIndex)
            {
              double num = xlsBiffMulRkCell.GetValue(columnIndex);
              LogManager.Log<ExcelBinaryReader>(this).Debug("VALUE[{1}]: {0}", (object) num, (object) columnIndex);
              this.m_cellsValues[(int) columnIndex] = !this.ConvertOaDate ? (object) num : this.tryConvertOADateTime(num, xlsBiffMulRkCell.GetXF(columnIndex));
            }
            return;
          case BIFFRECORDTYPE.MULBLANK:
            return;
          default:
            return;
        }
      }
      else if ((uint) id <= 517U)
      {
        switch (id)
        {
          case BIFFRECORDTYPE.LABELSST:
            string str = this.m_globals.SST.GetString(((XlsBiffLabelSSTCell) cell).SSTIndex);
            LogManager.Log<ExcelBinaryReader>(this).Debug("VALUE: {0}", (object) str);
            this.m_cellsValues[(int) cell.ColumnIndex] = (object) str;
            return;
          case BIFFRECORDTYPE.BLANK:
            return;
          case BIFFRECORDTYPE.INTEGER:
            break;
          case BIFFRECORDTYPE.NUMBER:
            goto label_13;
          case BIFFRECORDTYPE.LABEL:
            goto label_14;
          case BIFFRECORDTYPE.BOOLERR:
            if (cell.ReadByte(7) != (byte) 0)
              return;
            this.m_cellsValues[(int) cell.ColumnIndex] = (object) (cell.ReadByte(6) != (byte) 0);
            return;
          default:
            return;
        }
      }
      else
      {
        switch (id)
        {
          case BIFFRECORDTYPE.RK:
            double num1 = ((XlsBiffRKCell) cell).Value;
            this.m_cellsValues[(int) cell.ColumnIndex] = !this.ConvertOaDate ? (object) num1 : this.tryConvertOADateTime(num1, cell.XFormat);
            LogManager.Log<ExcelBinaryReader>(this).Debug("VALUE: {0}", (object) num1);
            return;
          case BIFFRECORDTYPE.FORMULA:
            goto label_21;
          default:
            return;
        }
      }
      this.m_cellsValues[(int) cell.ColumnIndex] = (object) ((XlsBiffIntegerCell) cell).Value;
      return;
label_13:
      double num2 = ((XlsBiffNumberCell) cell).Value;
      this.m_cellsValues[(int) cell.ColumnIndex] = !this.ConvertOaDate ? (object) num2 : this.tryConvertOADateTime(num2, cell.XFormat);
      LogManager.Log<ExcelBinaryReader>(this).Debug("VALUE: {0}", (object) num2);
      return;
label_14:
      this.m_cellsValues[(int) cell.ColumnIndex] = (object) ((XlsBiffLabelCell) cell).Value;
      LogManager.Log<ExcelBinaryReader>(this).Debug("VALUE: {0}", this.m_cellsValues[(int) cell.ColumnIndex]);
      return;
label_21:
      object obj = ((XlsBiffFormulaCell) cell).Value;
      if (obj == null || !(obj is FORMULAERROR))
        this.m_cellsValues[(int) cell.ColumnIndex] = !this.ConvertOaDate ? obj : this.tryConvertOADateTime(obj, cell.XFormat);
    }

    private bool moveToNextRecord()
    {
      if (this.m_noIndex)
      {
        LogManager.Log<ExcelBinaryReader>(this).Debug("No index");
        return this.moveToNextRecordNoIndex();
      }
      if (this.m_dbCellAddrs == null || this.m_dbCellAddrsIndex == this.m_dbCellAddrs.Length || this.m_depth == this.m_maxRow)
        return false;
      this.m_canRead = this.readWorkSheetRow();
      if (!this.m_canRead && this.m_depth > 0)
        this.m_canRead = true;
      if (!this.m_canRead && this.m_dbCellAddrsIndex < this.m_dbCellAddrs.Length - 1)
      {
        ++this.m_dbCellAddrsIndex;
        this.m_cellOffset = this.findFirstDataCellOffset((int) this.m_dbCellAddrs[this.m_dbCellAddrsIndex]);
        if (this.m_cellOffset < 0)
          return false;
        this.m_canRead = this.readWorkSheetRow();
      }
      return this.m_canRead;
    }

    private bool moveToNextRecordNoIndex()
    {
      xlsBiffRow = this.m_currentRowRecord;
      if (xlsBiffRow == null)
        return false;
      if ((int) xlsBiffRow.RowIndex < this.m_depth)
      {
        this.m_stream.Seek(xlsBiffRow.Offset + xlsBiffRow.Size, SeekOrigin.Begin);
        while (this.m_stream.Position < this.m_stream.Size)
        {
          XlsBiffRecord xlsBiffRecord = this.m_stream.Read();
          if (xlsBiffRecord is XlsBiffEOF)
            return false;
          if (xlsBiffRecord is XlsBiffRow xlsBiffRow && (int) xlsBiffRow.RowIndex >= this.m_depth)
            goto label_9;
        }
        return false;
      }
label_9:
      this.m_currentRowRecord = xlsBiffRow;
      XlsBiffBlankCell xlsBiffBlankCell1 = (XlsBiffBlankCell) null;
      while (this.m_stream.Position < this.m_stream.Size)
      {
        XlsBiffRecord xlsBiffRecord = this.m_stream.Read();
        if (xlsBiffRecord is XlsBiffEOF)
          return false;
        if (xlsBiffRecord.IsCell && xlsBiffRecord is XlsBiffBlankCell xlsBiffBlankCell2 && (int) xlsBiffBlankCell2.RowIndex == (int) this.m_currentRowRecord.RowIndex)
          xlsBiffBlankCell1 = xlsBiffBlankCell2;
        if (xlsBiffBlankCell1 != null)
        {
          this.m_cellOffset = xlsBiffBlankCell1.Offset;
          this.m_canRead = this.readWorkSheetRow();
          return this.m_canRead;
        }
      }
      return false;
    }

    private void initializeSheetRead()
    {
      if (this.m_SheetIndex == this.ResultsCount)
        return;
      this.m_dbCellAddrs = (uint[]) null;
      this.m_IsFirstRead = false;
      if (this.m_SheetIndex == -1)
        this.m_SheetIndex = 0;
      XlsBiffIndex idx;
      if (!this.readWorkSheetGlobals(this.m_sheets[this.m_SheetIndex], out idx, out this.m_currentRowRecord))
      {
        ++this.m_SheetIndex;
        this.initializeSheetRead();
      }
      else if (idx == null)
      {
        this.m_noIndex = true;
      }
      else
      {
        this.m_dbCellAddrs = idx.DbCellAddresses;
        this.m_dbCellAddrsIndex = 0;
        this.m_cellOffset = this.findFirstDataCellOffset((int) this.m_dbCellAddrs[this.m_dbCellAddrsIndex]);
        if (this.m_cellOffset >= 0)
          return;
        this.fail("Badly formed binary file. Has INDEX but no DBCELL");
      }
    }

    private void fail(string message)
    {
      this.m_exceptionMessage = message;
      this.m_isValid = false;
      this.m_file.Close();
      this.m_isClosed = true;
      this.m_workbookData = (DataSet) null;
      this.m_sheets = (List<XlsWorksheet>) null;
      this.m_stream = (XlsBiffStream) null;
      this.m_globals = (XlsWorkbookGlobals) null;
      this.m_encoding = (Encoding) null;
      this.m_hdr = (XlsHeader) null;
    }

    private object tryConvertOADateTime(double value, ushort XFormat)
    {
      ushort key;
      if (XFormat >= (ushort) 0 && (int) XFormat < this.m_globals.ExtendedFormats.Count)
      {
        XlsBiffRecord extendedFormat = this.m_globals.ExtendedFormats[(int) XFormat];
        switch (extendedFormat.ID)
        {
          case BIFFRECORDTYPE.XF_V2:
            key = (ushort) ((uint) extendedFormat.ReadByte(2) & 63U);
            break;
          case BIFFRECORDTYPE.XF_V3:
            if (((int) extendedFormat.ReadByte(3) & 4) == 0)
              return (object) value;
            key = (ushort) extendedFormat.ReadByte(1);
            break;
          case BIFFRECORDTYPE.XF_V4:
            if (((int) extendedFormat.ReadByte(5) & 4) == 0)
              return (object) value;
            key = (ushort) extendedFormat.ReadByte(1);
            break;
          default:
            if (((int) extendedFormat.ReadByte(this.m_globals.Sheets[this.m_globals.Sheets.Count - 1].IsV8 ? 9 : 7) & 4) == 0)
              return (object) value;
            key = extendedFormat.ReadUInt16(2);
            break;
        }
      }
      else
        key = XFormat;
      switch (key)
      {
        case 0:
        case 1:
        case 2:
        case 3:
        case 4:
        case 5:
        case 6:
        case 7:
        case 8:
        case 9:
        case 10:
        case 11:
        case 12:
        case 13:
        case 37:
        case 38:
        case 39:
        case 40:
        case 41:
        case 42:
        case 43:
        case 44:
        case 48:
          return (object) value;
        case 14:
        case 15:
        case 16:
        case 17:
        case 18:
        case 19:
        case 20:
        case 21:
        case 22:
        case 45:
        case 46:
        case 47:
          return Helpers.ConvertFromOATime(value);
        case 49:
          return (object) value.ToString();
        default:
          XlsBiffFormatString biffFormatString;
          if (this.m_globals.Formats.TryGetValue(key, out biffFormatString))
          {
            string str = biffFormatString.Value;
            if (new FormatReader() { FormatString = str }.IsDateFormatString())
              return Helpers.ConvertFromOATime(value);
          }
          return (object) value;
      }
    }

    private object tryConvertOADateTime(object value, ushort XFormat)
    {
      double result;
      return double.TryParse(value.ToString(), out result) ? this.tryConvertOADateTime(result, XFormat) : value;
    }

    public bool isV8() => this.m_version >= (ushort) 1536;

    public void Initialize(Stream fileStream)
    {
      this.m_file = fileStream;
      this.readWorkBookGlobals();
      this.m_SheetIndex = 0;
    }

    public DataSet AsDataSet() => this.AsDataSet(false);

    public DataSet AsDataSet(bool convertOADateTime)
    {
      if (!this.m_isValid)
        return (DataSet) null;
      if (this.m_isClosed)
        return this.m_workbookData;
      this.ConvertOaDate = convertOADateTime;
      this.m_workbookData = new DataSet();
      for (int index = 0; index < this.ResultsCount; ++index)
      {
        DataTable table = this.readWholeWorkSheet(this.m_sheets[index]);
        if (table != null)
          this.m_workbookData.Tables.Add(table);
      }
      this.m_file.Close();
      this.m_isClosed = true;
      this.m_workbookData.AcceptChanges();
      Helpers.FixDataTypes(this.m_workbookData);
      return this.m_workbookData;
    }

    public string ExceptionMessage => this.m_exceptionMessage;

    public string Name
    {
      get
      {
        return this.m_sheets != null && this.m_sheets.Count > 0 ? this.m_sheets[this.m_SheetIndex].Name : (string) null;
      }
    }

    public bool IsValid => this.m_isValid;

    public void Close()
    {
      this.m_file.Close();
      this.m_isClosed = true;
    }

    public int Depth => this.m_depth;

    public int ResultsCount => this.m_globals.Sheets.Count;

    public bool IsClosed => this.m_isClosed;

    public bool NextResult()
    {
      if (this.m_SheetIndex >= this.ResultsCount - 1)
        return false;
      ++this.m_SheetIndex;
      this.m_IsFirstRead = true;
      return true;
    }

    public bool Read()
    {
      if (!this.m_isValid)
        return false;
      if (this.m_IsFirstRead)
        this.initializeSheetRead();
      return this.moveToNextRecord();
    }

    public int FieldCount => this.m_maxCol;

    public bool GetBoolean(int i)
    {
      return !this.IsDBNull(i) && bool.Parse(this.m_cellsValues[i].ToString());
    }

    public DateTime GetDateTime(int i)
    {
      if (this.IsDBNull(i))
        return DateTime.MinValue;
      object cellsValue = this.m_cellsValues[i];
      if (cellsValue is DateTime dateTime)
        return dateTime;
      string s = cellsValue.ToString();
      double d;
      try
      {
        d = double.Parse(s);
      }
      catch (FormatException ex)
      {
        return DateTime.Parse(s);
      }
      return DateTime.FromOADate(d);
    }

    public Decimal GetDecimal(int i)
    {
      return this.IsDBNull(i) ? Decimal.MinValue : Decimal.Parse(this.m_cellsValues[i].ToString());
    }

    public double GetDouble(int i)
    {
      return this.IsDBNull(i) ? double.MinValue : double.Parse(this.m_cellsValues[i].ToString());
    }

    public float GetFloat(int i)
    {
      return this.IsDBNull(i) ? float.MinValue : float.Parse(this.m_cellsValues[i].ToString());
    }

    public short GetInt16(int i)
    {
      return this.IsDBNull(i) ? short.MinValue : short.Parse(this.m_cellsValues[i].ToString());
    }

    public int GetInt32(int i)
    {
      return this.IsDBNull(i) ? int.MinValue : int.Parse(this.m_cellsValues[i].ToString());
    }

    public long GetInt64(int i)
    {
      return this.IsDBNull(i) ? long.MinValue : long.Parse(this.m_cellsValues[i].ToString());
    }

    public string GetString(int i)
    {
      return this.IsDBNull(i) ? (string) null : this.m_cellsValues[i].ToString();
    }

    public object GetValue(int i) => this.m_cellsValues[i];

    public bool IsDBNull(int i)
    {
      return this.m_cellsValues[i] == null || DBNull.Value == this.m_cellsValues[i];
    }

    public object this[int i] => this.m_cellsValues[i];

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

    public bool IsFirstRowAsColumnNames
    {
      get => this._isFirstRowAsColumnNames;
      set => this._isFirstRowAsColumnNames = value;
    }

    public bool ConvertOaDate
    {
      get => this.m_ConvertOADate;
      set => this.m_ConvertOADate = value;
    }

    public ReadOption ReadOption => this.m_ReadOption;
  }
}
