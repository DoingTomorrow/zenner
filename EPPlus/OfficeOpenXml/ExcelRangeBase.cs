// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelRangeBase
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.ConditionalFormatting;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using OfficeOpenXml.Style;
using OfficeOpenXml.Style.XmlAccess;
using OfficeOpenXml.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelRangeBase : 
    ExcelAddress,
    IExcelCell,
    IEnumerable<ExcelRangeBase>,
    IEnumerable,
    IEnumerator<ExcelRangeBase>,
    IDisposable,
    IEnumerator
  {
    protected ExcelWorksheet _worksheet;
    internal ExcelWorkbook _workbook;
    private ExcelRangeBase._changeProp _changePropMethod;
    private int _styleID;
    private ExcelRichTextCollection _rtc;
    private CellsStoreEnumerator<object> cellEnum;
    private int _enumAddressIx = -1;

    internal ExcelRangeBase(ExcelWorksheet xlWorksheet)
    {
      this._worksheet = xlWorksheet;
      this._ws = this._worksheet.Name;
      this._workbook = this._worksheet.Workbook;
      this.AddressChange += new EventHandler(this.ExcelRangeBase_AddressChange);
      this.SetDelegate();
    }

    private void ExcelRangeBase_AddressChange(object sender, EventArgs e)
    {
      if (this.Table != null)
        this.SetRCFromTable(this._workbook._package, (ExcelAddressBase) null);
      this.SetDelegate();
    }

    internal ExcelRangeBase(ExcelWorksheet xlWorksheet, string address)
      : base(xlWorksheet == null ? "" : xlWorksheet.Name, address)
    {
      this._worksheet = xlWorksheet;
      this._workbook = this._worksheet.Workbook;
      this.SetRCFromTable(this._worksheet._package, (ExcelAddressBase) null);
      if (string.IsNullOrEmpty(this._ws))
        this._ws = this._worksheet == null ? "" : this._worksheet.Name;
      this.AddressChange += new EventHandler(this.ExcelRangeBase_AddressChange);
      this.SetDelegate();
    }

    internal ExcelRangeBase(
      ExcelWorkbook wb,
      ExcelWorksheet xlWorksheet,
      string address,
      bool isName)
      : base(xlWorksheet == null ? "" : xlWorksheet.Name, address, isName)
    {
      this.SetRCFromTable(wb._package, (ExcelAddressBase) null);
      this._worksheet = xlWorksheet;
      this._workbook = wb;
      if (string.IsNullOrEmpty(this._ws))
        this._ws = xlWorksheet == null ? (string) null : xlWorksheet.Name;
      this.AddressChange += new EventHandler(this.ExcelRangeBase_AddressChange);
      this.SetDelegate();
    }

    ~ExcelRangeBase() => this.AddressChange -= new EventHandler(this.ExcelRangeBase_AddressChange);

    private void SetDelegate()
    {
      if (this._fromRow == -1)
        this._changePropMethod = new ExcelRangeBase._changeProp(this.SetUnknown);
      else if (this._fromRow == this._toRow && this._fromCol == this._toCol && this.Addresses == null)
        this._changePropMethod = new ExcelRangeBase._changeProp(this.SetSingle);
      else if (this.Addresses == null)
        this._changePropMethod = new ExcelRangeBase._changeProp(this.SetRange);
      else
        this._changePropMethod = new ExcelRangeBase._changeProp(this.SetMultiRange);
    }

    private void SetUnknown(ExcelRangeBase._setValue valueMethod, object value)
    {
      if (this._fromRow == -1)
        this.SetToSelectedRange();
      this.SetDelegate();
      this._changePropMethod(valueMethod, value);
    }

    private void SetSingle(ExcelRangeBase._setValue valueMethod, object value)
    {
      valueMethod(value, this._fromRow, this._fromCol);
    }

    private void SetRange(ExcelRangeBase._setValue valueMethod, object value)
    {
      this.SetValueAddress((ExcelAddress) this, valueMethod, value);
    }

    private void SetMultiRange(ExcelRangeBase._setValue valueMethod, object value)
    {
      this.SetValueAddress((ExcelAddress) this, valueMethod, value);
      foreach (ExcelAddress address in this.Addresses)
        this.SetValueAddress(address, valueMethod, value);
    }

    private void SetValueAddress(
      ExcelAddress address,
      ExcelRangeBase._setValue valueMethod,
      object value)
    {
      this.IsRangeValid("");
      if (this._fromRow == 1 && this._fromCol == 1 && this._toRow == 1048576 && this._toCol == 16384)
        throw new ArgumentException("Can't reference all cells. Please use the indexer to set the range");
      for (int column = address.Start.Column; column <= address.End.Column; ++column)
      {
        for (int row = address.Start.Row; row <= address.End.Row; ++row)
          valueMethod(value, row, column);
      }
    }

    private void Set_StyleID(object value, int row, int col)
    {
      this._worksheet._styles.SetValue(row, col, (int) value);
    }

    private void Set_StyleName(object value, int row, int col)
    {
      this._worksheet._styles.SetValue(row, col, this._styleID);
    }

    private void Set_Value(object value, int row, int col)
    {
      object obj = this._worksheet._formulas.GetValue(row, col);
      if (obj is int)
        this.SplitFormulas();
      if (obj != null)
        this._worksheet._formulas.SetValue(row, col, (object) string.Empty);
      this._worksheet._values.SetValue(row, col, value);
    }

    private void Set_Formula(object value, int row, int col)
    {
      if (this._worksheet._formulas.GetValue(row, col) is int num && num > 0)
        this.SplitFormulas();
      string str = value == null ? string.Empty : value.ToString();
      if (str == string.Empty)
      {
        this._worksheet._formulas.SetValue(row, col, (object) string.Empty);
      }
      else
      {
        if (str[0] == '=')
          value = (object) str.Substring(1, str.Length - 1);
        this._worksheet._formulas.SetValue(row, col, (object) str);
        this._worksheet._values.SetValue(row, col, (object) null);
      }
    }

    private void Set_SharedFormula(string value, ExcelAddress address, bool IsArray)
    {
      if (this._fromRow == 1 && this._fromCol == 1 && this._toRow == 1048576 && this._toCol == 16384)
        throw new InvalidOperationException("Can't set a formula for the entire worksheet");
      if (address.Start.Row == address.End.Row && address.Start.Column == address.End.Column && !IsArray)
      {
        this.Set_Formula((object) value, address.Start.Row, address.Start.Column);
      }
      else
      {
        this.CheckAndSplitSharedFormula();
        ExcelWorksheet.Formulas formulas = new ExcelWorksheet.Formulas(SourceCodeTokenizer.Default);
        formulas.Formula = value;
        formulas.Index = this._worksheet.GetMaxShareFunctionIndex(IsArray);
        formulas.Address = address.FirstAddress;
        formulas.StartCol = address.Start.Column;
        formulas.StartRow = address.Start.Row;
        formulas.IsArray = IsArray;
        this._worksheet._sharedFormulas.Add(formulas.Index, formulas);
        for (int column = address.Start.Column; column <= address.End.Column; ++column)
        {
          for (int row = address.Start.Row; row <= address.End.Row; ++row)
          {
            this._worksheet._formulas.SetValue(row, column, (object) formulas.Index);
            this._worksheet._values.SetValue(row, column, (object) null);
          }
        }
      }
    }

    private void Set_HyperLink(object value, int row, int col)
    {
      if ((object) (value as Uri) != null)
      {
        this._worksheet._hyperLinks.SetValue(row, col, (Uri) value);
        if (value is ExcelHyperLink)
          this._worksheet._values.SetValue(row, col, (object) ((ExcelHyperLink) value).Display);
        else
          this._worksheet._values.SetValue(row, col, (object) ((Uri) value).OriginalString);
      }
      else
      {
        this._worksheet._hyperLinks.SetValue(row, col, (Uri) null);
        this._worksheet._values.SetValue(row, col, (object) null);
      }
    }

    private void Set_IsRichText(object value, int row, int col)
    {
      this._worksheet._flags.SetFlagValue(row, col, (bool) value, CellFlags.RichText);
    }

    private void Exists_Comment(object value, int row, int col)
    {
      if (this._worksheet.Comments._comments.ContainsKey(ExcelCellBase.GetCellID(this._worksheet.SheetID, row, col)))
        throw new InvalidOperationException(string.Format("Cell {0} already contain a comment.", (object) new ExcelCellAddress(row, col).Address));
    }

    private void Set_Comment(object value, int row, int col)
    {
      string[] strArray = (string[]) value;
      this.Worksheet.Comments.Add(new ExcelRangeBase(this._worksheet, ExcelCellBase.GetAddress(this._fromRow, this._fromCol)), strArray[0], strArray[1]);
    }

    private void SetToSelectedRange()
    {
      if (this._worksheet.View.SelectedRange == "")
        this.Address = "A1";
      else
        this.Address = this._worksheet.View.SelectedRange;
    }

    private void IsRangeValid(string type)
    {
      if (this._fromRow > 0)
        return;
      if (this._address == "")
      {
        this.SetToSelectedRange();
      }
      else
      {
        if (type == "")
          throw new InvalidOperationException(string.Format("Range is not valid for this operation: {0}", (object) this._address));
        throw new InvalidOperationException(string.Format("Range is not valid for {0} : {1}", (object) type, (object) this._address));
      }
    }

    public ExcelStyle Style
    {
      get
      {
        this.IsRangeValid("styling");
        int Id = 0;
        if (!this._worksheet._styles.Exists(this._fromRow, this._fromCol, ref Id) && !this._worksheet._styles.Exists(this._fromRow, 0, ref Id))
          Id = this.Worksheet.Column(this._fromCol).StyleID;
        return this._worksheet.Workbook.Styles.GetStyleObject(Id, this._worksheet.PositionID, this.Address);
      }
    }

    public string StyleName
    {
      get
      {
        this.IsRangeValid("styling");
        int PositionID;
        if (this._fromRow == 1 && this._toRow == 1048576)
          PositionID = this.GetColumnStyle(this._fromCol);
        else if (this._fromCol == 1 && this._toCol == 16384)
        {
          PositionID = 0;
          if (!this._worksheet._styles.Exists(this._fromRow, 0, ref PositionID))
            PositionID = this.GetColumnStyle(this._fromCol);
        }
        else
        {
          PositionID = 0;
          if (!this._worksheet._styles.Exists(this._fromRow, this._fromCol, ref PositionID) && !this._worksheet._styles.Exists(this._fromRow, 0, ref PositionID))
            PositionID = this.GetColumnStyle(this._fromCol);
        }
        int num = PositionID > 0 ? this.Style.Styles.CellXfs[PositionID].XfId : this.Style.Styles.CellXfs[0].XfId;
        foreach (ExcelNamedStyleXml namedStyle in this.Style.Styles.NamedStyles)
        {
          if (namedStyle.StyleXfId == num)
            return namedStyle.Name;
        }
        return "";
      }
      set
      {
        this._styleID = this._worksheet.Workbook.Styles.GetStyleIdFromName(value);
        int fromCol1 = this._fromCol;
        if (this._fromRow == 1 && this._toRow == 1048576)
        {
          object obj = this._worksheet.GetValue(0, this._fromCol);
          ExcelColumn c = obj != null ? (ExcelColumn) obj : this._worksheet.Column(this._fromCol);
          c.StyleName = value;
          c.StyleID = this._styleID;
          CellsStoreEnumerator<object> cellsStoreEnumerator1 = new CellsStoreEnumerator<object>(this._worksheet._values, 0, this._fromCol + 1, 0, this._toCol);
          if (cellsStoreEnumerator1.Next())
          {
            int fromCol2 = this._fromCol;
            while (c.ColumnMin <= this._toCol)
            {
              if (c.ColumnMax > this._toCol)
              {
                this._worksheet.CopyColumn(c, this._toCol + 1, c.ColumnMax);
                c.ColumnMax = this._toCol;
              }
              c._styleName = value;
              c.StyleID = this._styleID;
              if (cellsStoreEnumerator1.Value != null)
              {
                c = (ExcelColumn) cellsStoreEnumerator1.Value;
                cellsStoreEnumerator1.Next();
              }
              else
                break;
            }
          }
          if (this._fromCol == 1 && this._toCol == 16384)
          {
            CellsStoreEnumerator<object> cellsStoreEnumerator2 = new CellsStoreEnumerator<object>(this._worksheet._values, 1, 0, 1048576, 0);
            cellsStoreEnumerator2.Next();
            while (cellsStoreEnumerator2.Value != null)
            {
              ExcelRow excelRow = cellsStoreEnumerator2.Value as ExcelRow;
              excelRow._styleName = value;
              excelRow.StyleID = this.StyleID;
              if (!cellsStoreEnumerator2.Next())
                break;
            }
          }
        }
        else if (this._fromCol == 1 && this._toCol == 16384)
        {
          for (int fromRow = this._fromRow; fromRow <= this._toRow; ++fromRow)
          {
            this._worksheet.Row(fromRow)._styleName = value;
            this._worksheet.Row(fromRow).StyleID = this.StyleID;
          }
        }
        if ((this._fromRow != 1 || this._toRow != 1048576) && (this._fromCol != 1 || this._toCol != 16384))
        {
          for (int fromCol3 = this._fromCol; fromCol3 <= this._toCol; ++fromCol3)
          {
            for (int fromRow = this._fromRow; fromRow <= this._toRow; ++fromRow)
              this._worksheet._styles.SetValue(fromRow, fromCol3, this._styleID);
          }
        }
        else
        {
          CellsStoreEnumerator<object> cellsStoreEnumerator = new CellsStoreEnumerator<object>(this._worksheet._values, this._fromRow, this._fromCol, this._toRow, this._toCol);
          while (cellsStoreEnumerator.Next())
            this._worksheet._styles.SetValue(cellsStoreEnumerator.Row, cellsStoreEnumerator.Column, this._styleID);
        }
      }
    }

    private int GetColumnStyle(int col)
    {
      object obj = (object) null;
      if (this._worksheet._values.Exists(0, col, ref obj))
        return (obj as ExcelColumn).StyleID;
      int row = 0;
      return this._worksheet._values.PrevCell(ref row, ref col) && (this._worksheet._values.GetValue(row, col) as ExcelColumn).ColumnMax >= col ? this._worksheet._styles.GetValue(row, col) : 0;
    }

    public int StyleID
    {
      get => this._worksheet._styles.GetValue(this._fromRow, this._fromCol);
      set => this._changePropMethod(new ExcelRangeBase._setValue(this.Set_StyleID), (object) value);
    }

    public object Value
    {
      get
      {
        return this.IsName ? (this._worksheet == null ? this._workbook._names[this._address].NameValue : this._worksheet.Names[this._address].NameValue) : (this._fromRow == this._toRow && this._fromCol == this._toCol ? this._worksheet.GetValue(this._fromRow, this._fromCol) : this.GetValueArray());
      }
      set
      {
        if (this.IsName)
        {
          if (this._worksheet == null)
            this._workbook._names[this._address].NameValue = value;
          else
            this._worksheet.Names[this._address].NameValue = value;
        }
        else
          this._changePropMethod(new ExcelRangeBase._setValue(this.Set_Value), value);
      }
    }

    private bool IsInfinityValue(object value)
    {
      double? nullable = value as double?;
      return nullable.HasValue && (double.IsNegativeInfinity(nullable.Value) || double.IsPositiveInfinity(nullable.Value));
    }

    private object GetValueArray()
    {
      ExcelAddressBase excelAddressBase;
      if (this._fromRow == 1 && this._fromCol == 1 && this._toRow == 1048576 && this._toCol == 16384)
      {
        excelAddressBase = this._worksheet.Dimension;
        if (excelAddressBase == null)
          return (object) null;
      }
      else
        excelAddressBase = (ExcelAddressBase) this;
      object[,] valueArray = new object[excelAddressBase._toRow - excelAddressBase._fromRow + 1, excelAddressBase._toCol - excelAddressBase._fromCol + 1];
      for (int fromCol = excelAddressBase._fromCol; fromCol <= excelAddressBase._toCol; ++fromCol)
      {
        for (int fromRow = excelAddressBase._fromRow; fromRow <= excelAddressBase._toRow; ++fromRow)
        {
          if (this._worksheet._values.Exists(fromRow, fromCol))
            valueArray[fromRow - excelAddressBase._fromRow, fromCol - excelAddressBase._fromCol] = !this.IsRichText ? this._worksheet._values.GetValue(fromRow, fromCol) : (object) this.GetRichText(fromRow, fromCol).Text;
        }
      }
      return (object) valueArray;
    }

    private ExcelAddressBase GetAddressDim(ExcelRangeBase addr)
    {
      ExcelAddressBase dimension = this._worksheet.Dimension;
      int fromRow = addr._fromRow < dimension._fromRow ? dimension._fromRow : addr._fromRow;
      int fromCol = addr._fromCol < dimension._fromCol ? dimension._fromCol : addr._fromCol;
      int toRow = addr._toRow > dimension._toRow ? dimension._toRow : addr._toRow;
      int toColumn = addr._toCol > dimension._toCol ? dimension._toCol : addr._toCol;
      if (addr._fromCol == fromRow && addr._fromCol == addr._fromCol && addr._toRow == toRow && addr._toCol == this._toCol)
        return (ExcelAddressBase) addr;
      return this._fromRow > this._toRow || this._fromCol > this._toCol ? (ExcelAddressBase) null : new ExcelAddressBase(fromRow, fromCol, toRow, toColumn);
    }

    private object GetSingleValue()
    {
      return this.IsRichText ? (object) this.RichText.Text : this._worksheet._values.GetValue(this._fromRow, this._fromCol);
    }

    public string Text => this.GetFormattedText(false);

    public void AutoFitColumns() => this.AutoFitColumns(this._worksheet.DefaultColWidth);

    public void AutoFitColumns(double MinimumWidth)
    {
      this.AutoFitColumns(MinimumWidth, double.MaxValue);
    }

    public void AutoFitColumns(double MinimumWidth, double MaximumWidth)
    {
      if (this._worksheet.Dimension == null)
        return;
      if (this._fromCol < 1 || this._fromRow < 1)
        this.SetToSelectedRange();
      Dictionary<int, Font> dictionary = new Dictionary<int, Font>();
      bool doAdjustDrawings = this._worksheet._package.DoAdjustDrawings;
      this._worksheet._package.DoAdjustDrawings = false;
      int[,] drawingWidths = this._worksheet.Drawings.GetDrawingWidths();
      int num1 = this._fromCol > this._worksheet.Dimension._fromCol ? this._fromCol : this._worksheet.Dimension._fromCol;
      int num2 = this._toCol < this._worksheet.Dimension._toCol ? this._toCol : this._worksheet.Dimension._toCol;
      if (this.Addresses == null)
      {
        for (int col = num1; col <= num2; ++col)
          this._worksheet.Column(col).Width = MinimumWidth;
      }
      else
      {
        foreach (ExcelAddress address in this.Addresses)
        {
          int num3 = address._fromCol > this._worksheet.Dimension._fromCol ? address._fromCol : this._worksheet.Dimension._fromCol;
          int num4 = address._toCol < this._worksheet.Dimension._toCol ? address._toCol : this._worksheet.Dimension._toCol;
          for (int col = num3; col <= num4; ++col)
            this._worksheet.Column(col).Width = MinimumWidth;
        }
      }
      List<ExcelAddressBase> excelAddressBaseList = new List<ExcelAddressBase>();
      if (this._worksheet.AutoFilterAddress != null)
      {
        excelAddressBaseList.Add(new ExcelAddressBase(this._worksheet.AutoFilterAddress._fromRow, this._worksheet.AutoFilterAddress._fromCol, this._worksheet.AutoFilterAddress._fromRow, this._worksheet.AutoFilterAddress._toCol));
        excelAddressBaseList[excelAddressBaseList.Count - 1]._ws = this.WorkSheet;
      }
      foreach (ExcelTable table in this._worksheet.Tables)
      {
        if (table.AutoFilterAddress != null)
        {
          excelAddressBaseList.Add(new ExcelAddressBase(table.AutoFilterAddress._fromRow, table.AutoFilterAddress._fromCol, table.AutoFilterAddress._fromRow, table.AutoFilterAddress._toCol));
          excelAddressBaseList[excelAddressBaseList.Count - 1]._ws = this.WorkSheet;
        }
      }
      ExcelStyles styles = this._worksheet.Workbook.Styles;
      ExcelFontXml font1 = styles.Fonts[styles.CellXfs[0].FontId];
      FontStyle style1 = FontStyle.Regular;
      if (font1.Bold)
        style1 |= FontStyle.Bold;
      if (font1.UnderLine)
        style1 |= FontStyle.Underline;
      if (font1.Italic)
        style1 |= FontStyle.Italic;
      if (font1.Strike)
        style1 |= FontStyle.Strikeout;
      Font font2 = new Font(font1.Name, font1.Size, style1);
      using (Bitmap bitmap = new Bitmap(1, 1))
      {
        using (Graphics graphics = Graphics.FromImage((Image) bitmap))
        {
          float num5 = (float) Math.Truncate((double) graphics.MeasureString("00", font2).Width - (double) graphics.MeasureString("0", font2).Width);
          graphics.PageUnit = GraphicsUnit.Pixel;
          foreach (ExcelRangeBase address in this)
          {
            if (!address.Merge && !address.Style.WrapText)
            {
              int fontId = styles.CellXfs[address.StyleID].FontId;
              Font font3;
              if (dictionary.ContainsKey(fontId))
              {
                font3 = dictionary[fontId];
              }
              else
              {
                ExcelFontXml font4 = styles.Fonts[fontId];
                FontStyle style2 = FontStyle.Regular;
                if (font4.Bold)
                  style2 |= FontStyle.Bold;
                if (font4.UnderLine)
                  style2 |= FontStyle.Underline;
                if (font4.Italic)
                  style2 |= FontStyle.Italic;
                if (font4.Strike)
                  style2 |= FontStyle.Strikeout;
                font3 = new Font(font4.Name, font4.Size, style2);
                dictionary.Add(fontId, font3);
              }
              SizeF sizeF = graphics.MeasureString(address.TextForWidth, font3);
              double textRotation = (double) styles.CellXfs[address.StyleID].TextRotation;
              double num6;
              if (textRotation <= 0.0)
              {
                num6 = ((double) sizeF.Width + 5.0) / (double) num5;
              }
              else
              {
                double num7 = textRotation <= 90.0 ? textRotation : textRotation - 90.0;
                num6 = (((double) sizeF.Width - (double) sizeF.Height) * Math.Abs(Math.Cos(Math.PI * num7 / 180.0)) + (double) sizeF.Height + 5.0) / (double) num5;
              }
              foreach (ExcelAddressBase excelAddressBase in excelAddressBaseList)
              {
                if (excelAddressBase.Collide((ExcelAddressBase) address) != ExcelAddressBase.eAddressCollition.No)
                {
                  num6 += 2.25;
                  break;
                }
              }
              if (num6 > this._worksheet.Column(address._fromCol).Width)
                this._worksheet.Column(address._fromCol).Width = num6 > MaximumWidth ? MaximumWidth : num6;
            }
          }
        }
      }
      this._worksheet.Drawings.AdjustWidth(drawingWidths);
      this._worksheet._package.DoAdjustDrawings = doAdjustDrawings;
    }

    internal string TextForWidth => this.GetFormattedText(true);

    private string GetFormattedText(bool forWidthCalc)
    {
      object v = this.Value;
      if (v == null)
        return "";
      ExcelStyles styles = this.Worksheet.Workbook.Styles;
      int numberFormatId = styles.CellXfs[this.StyleID].NumberFormatId;
      ExcelNumberFormatXml.ExcelFormatTranslator nf = (ExcelNumberFormatXml.ExcelFormatTranslator) null;
      for (int PositionID = 0; PositionID < styles.NumberFormats.Count; ++PositionID)
      {
        if (numberFormatId == styles.NumberFormats[PositionID].NumFmtId)
        {
          nf = styles.NumberFormats[PositionID].FormatTranslator;
          break;
        }
      }
      string format;
      string textFormat;
      if (forWidthCalc)
      {
        format = nf.NetFormatForWidth;
        textFormat = nf.NetTextFormatForWidth;
      }
      else
      {
        format = nf.NetFormat;
        textFormat = nf.NetTextFormat;
      }
      return ExcelRangeBase.FormatValue(v, nf, format, textFormat);
    }

    internal static string FormatValue(
      object v,
      ExcelNumberFormatXml.ExcelFormatTranslator nf,
      string format,
      string textFormat)
    {
      if (!(v is Decimal))
      {
        if (!v.GetType().IsPrimitive)
        {
          if (v is DateTime)
          {
            if (nf.DataType == ExcelNumberFormatXml.eFormatType.DateTime)
              return ((DateTime) v).ToString(format, (IFormatProvider) nf.Culture);
            double oaDate = ((DateTime) v).ToOADate();
            return string.IsNullOrEmpty(nf.FractionFormat) ? oaDate.ToString(format, (IFormatProvider) nf.Culture) : nf.FormatFraction(oaDate);
          }
          if (v is TimeSpan)
          {
            if (nf.DataType == ExcelNumberFormatXml.eFormatType.DateTime)
              return new DateTime(((TimeSpan) v).Ticks).ToString(format, (IFormatProvider) nf.Culture);
            double oaDate = new DateTime(((TimeSpan) v).Ticks).ToOADate();
            return string.IsNullOrEmpty(nf.FractionFormat) ? oaDate.ToString(format, (IFormatProvider) nf.Culture) : nf.FormatFraction(oaDate);
          }
          return textFormat == "" ? v.ToString() : string.Format(textFormat, v);
        }
      }
      double d;
      try
      {
        d = Convert.ToDouble(v);
      }
      catch
      {
        return "";
      }
      return nf.DataType == ExcelNumberFormatXml.eFormatType.Number ? (string.IsNullOrEmpty(nf.FractionFormat) ? d.ToString(format, (IFormatProvider) nf.Culture) : nf.FormatFraction(d)) : (nf.DataType == ExcelNumberFormatXml.eFormatType.DateTime ? DateTime.FromOADate(d).ToString(format, (IFormatProvider) nf.Culture) : v.ToString());
    }

    public string Formula
    {
      get
      {
        if (!this.IsName)
          return this._worksheet.GetFormula(this._fromRow, this._fromCol);
        return this._worksheet == null ? this._workbook._names[this._address].NameFormula : this._worksheet.Names[this._address].NameFormula;
      }
      set
      {
        if (this.IsName)
        {
          if (this._worksheet == null)
            this._workbook._names[this._address].NameFormula = value;
          else
            this._worksheet.Names[this._address].NameFormula = value;
        }
        else if (this._fromRow == this._toRow && this._fromCol == this._toCol)
        {
          this.Set_Formula((object) value, this._fromRow, this._fromCol);
        }
        else
        {
          this.Set_SharedFormula(value, (ExcelAddress) this, false);
          if (this.Addresses == null)
            return;
          foreach (ExcelAddress address in this.Addresses)
            this.Set_SharedFormula(value, address, false);
        }
      }
    }

    public string FormulaR1C1
    {
      get
      {
        this.IsRangeValid(nameof (FormulaR1C1));
        return this._worksheet.GetFormulaR1C1(this._fromRow, this._fromCol);
      }
      set
      {
        this.IsRangeValid(nameof (FormulaR1C1));
        if (value.Length > 0 && value[0] == '=')
          value = value.Substring(1, value.Length - 1);
        if (this.Addresses == null)
        {
          this.Set_SharedFormula(ExcelCellBase.TranslateFromR1C1(value, this._fromRow, this._fromCol), (ExcelAddress) this, false);
        }
        else
        {
          this.Set_SharedFormula(ExcelCellBase.TranslateFromR1C1(value, this._fromRow, this._fromCol), new ExcelAddress(this.FirstAddress), false);
          foreach (ExcelAddress address in this.Addresses)
            this.Set_SharedFormula(ExcelCellBase.TranslateFromR1C1(value, address.Start.Row, address.Start.Column), address, false);
        }
      }
    }

    public Uri Hyperlink
    {
      get
      {
        this.IsRangeValid("formulaR1C1");
        return this._worksheet._hyperLinks.GetValue(this._fromRow, this._fromCol);
      }
      set
      {
        this._changePropMethod(new ExcelRangeBase._setValue(this.Set_HyperLink), (object) value);
      }
    }

    public bool Merge
    {
      get
      {
        this.IsRangeValid("merging");
        for (int fromCol = this._fromCol; fromCol <= this._toCol; ++fromCol)
        {
          for (int fromRow = this._fromRow; fromRow <= this._toRow; ++fromRow)
          {
            if (!this._worksheet._flags.GetFlagValue(fromRow, fromCol, CellFlags.Merged))
              return false;
          }
        }
        return true;
      }
      set
      {
        this.IsRangeValid("merging");
        this.SetMerge(value, this.FirstAddress);
        if (this.Addresses == null)
          return;
        foreach (ExcelAddress address in this.Addresses)
          this.SetMerge(value, address._address);
      }
    }

    private void SetMerge(bool value, string address)
    {
      if (!value)
      {
        if (this._worksheet.MergedCells.List.Contains(address))
        {
          this.SetCellMerge(false, address);
          this._worksheet.MergedCells.List.Remove(address);
        }
        else if (!this.CheckMergeDiff(false, address))
          throw new Exception("Range is not fully merged.Specify the exact range");
      }
      else if (this.CheckMergeDiff(false, address))
      {
        this.SetCellMerge(true, address);
        this._worksheet.MergedCells.List.Add(address);
      }
      else if (!this._worksheet.MergedCells.List.Contains(address))
        throw new Exception("Cells are already merged");
    }

    public bool AutoFilter
    {
      get
      {
        this.IsRangeValid("autofilter");
        ExcelAddressBase autoFilterAddress = this._worksheet.AutoFilterAddress;
        return autoFilterAddress != null && this._fromRow >= autoFilterAddress.Start.Row && this._toRow <= autoFilterAddress.End.Row && this._fromCol >= autoFilterAddress.Start.Column && this._toCol <= autoFilterAddress.End.Column;
      }
      set
      {
        this.IsRangeValid("autofilter");
        this._worksheet.AutoFilterAddress = (ExcelAddressBase) this;
        if (this._worksheet.Names.ContainsKey("_xlnm._FilterDatabase"))
          this._worksheet.Names.Remove("_xlnm._FilterDatabase");
        this._worksheet.Names.Add("_xlnm._FilterDatabase", this).IsNameHidden = true;
      }
    }

    public bool IsRichText
    {
      get
      {
        this.IsRangeValid("richtext");
        return this._worksheet._flags.GetFlagValue(this._fromRow, this._fromCol, CellFlags.RichText);
      }
      set
      {
        this._changePropMethod(new ExcelRangeBase._setValue(this.Set_IsRichText), (object) value);
      }
    }

    public bool IsArrayFormula
    {
      get
      {
        this.IsRangeValid("arrayformulas");
        return this._worksheet._flags.GetFlagValue(this._fromRow, this._fromCol, CellFlags.ArrayFormula);
      }
    }

    public ExcelRichTextCollection RichText
    {
      get
      {
        this.IsRangeValid("richtext");
        if (this._rtc == null)
          this._rtc = this.GetRichText(this._fromRow, this._fromCol);
        return this._rtc;
      }
    }

    private ExcelRichTextCollection GetRichText(int row, int col)
    {
      XmlDocument xmlDoc = new XmlDocument();
      object obj = this._worksheet._values.GetValue(row, col);
      bool flagValue = this._worksheet._flags.GetFlagValue(row, col, CellFlags.RichText);
      if (obj != null)
      {
        if (flagValue)
          XmlHelper.LoadXmlSafe(xmlDoc, "<d:si xmlns:d=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" >" + obj.ToString() + "</d:si>", Encoding.UTF8);
        else
          xmlDoc.LoadXml("<d:si xmlns:d=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" ><d:r><d:t>" + SecurityElement.Escape(obj.ToString()) + "</d:t></d:r></d:si>");
      }
      else
        xmlDoc.LoadXml("<d:si xmlns:d=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" />");
      ExcelRichTextCollection richText = new ExcelRichTextCollection(this._worksheet.NameSpaceManager, xmlDoc.SelectSingleNode("d:si", this._worksheet.NameSpaceManager), this);
      if (richText.Count == 1 && !flagValue)
      {
        this.IsRichText = true;
        ExcelFont font = this._worksheet.Workbook.Styles.GetStyleObject(this._worksheet._styles.GetValue(row, col), this._worksheet.PositionID, ExcelCellBase.GetAddress(row, col)).Font;
        richText[0].PreserveSpace = true;
        richText[0].Bold = font.Bold;
        richText[0].FontName = font.Name;
        richText[0].Italic = font.Italic;
        richText[0].Size = font.Size;
        richText[0].UnderLine = font.UnderLine;
        int result;
        if (font.Color.Rgb != "" && int.TryParse(font.Color.Rgb, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          richText[0].Color = Color.FromArgb(result);
      }
      return richText;
    }

    public ExcelComment Comment
    {
      get
      {
        this.IsRangeValid("comments");
        ulong cellId = ExcelCellBase.GetCellID(this._worksheet.SheetID, this._fromRow, this._fromCol);
        return this._worksheet.Comments._comments.ContainsKey(cellId) ? this._worksheet._comments._comments[cellId] as ExcelComment : (ExcelComment) null;
      }
    }

    public ExcelWorksheet Worksheet => this._worksheet;

    public string FullAddress
    {
      get
      {
        string fullAddress = ExcelCellBase.GetFullAddress(this._worksheet.Name, this._address);
        if (this.Addresses != null)
        {
          foreach (ExcelAddress address in this.Addresses)
            fullAddress = fullAddress + "," + ExcelCellBase.GetFullAddress(this._worksheet.Name, address.Address);
        }
        return fullAddress;
      }
    }

    public string FullAddressAbsolute
    {
      get
      {
        string worksheetName = string.IsNullOrEmpty(this._wb) ? this._ws : "[" + this._wb.Replace("'", "''") + "]" + this._ws;
        string fullAddressAbsolute = ExcelCellBase.GetFullAddress(worksheetName, ExcelCellBase.GetAddress(this._fromRow, this._fromCol, this._toRow, this._toCol, true));
        if (this.Addresses != null)
        {
          foreach (ExcelAddress address in this.Addresses)
            fullAddressAbsolute = fullAddressAbsolute + "," + ExcelCellBase.GetFullAddress(worksheetName, ExcelCellBase.GetAddress(address.Start.Row, address.Start.Column, address.End.Row, address.End.Column, true));
        }
        return fullAddressAbsolute;
      }
    }

    internal string FullAddressAbsoluteNoFullRowCol
    {
      get
      {
        string worksheetName = string.IsNullOrEmpty(this._wb) ? this._ws : "[" + this._wb.Replace("'", "''") + "]" + this._ws;
        string absoluteNoFullRowCol = ExcelCellBase.GetFullAddress(worksheetName, ExcelCellBase.GetAddress(this._fromRow, this._fromCol, this._toRow, this._toCol, true), false);
        if (this.Addresses != null)
        {
          foreach (ExcelAddress address in this.Addresses)
            absoluteNoFullRowCol = absoluteNoFullRowCol + "," + ExcelCellBase.GetFullAddress(worksheetName, ExcelCellBase.GetAddress(address.Start.Row, address.Start.Column, address.End.Row, address.End.Column, true), false);
        }
        return absoluteNoFullRowCol;
      }
    }

    private bool CheckMergeDiff(bool startValue, string address)
    {
      ExcelAddress excelAddress = new ExcelAddress(address);
      for (int fromCol = excelAddress._fromCol; fromCol <= excelAddress._toCol; ++fromCol)
      {
        for (int fromRow = excelAddress._fromRow; fromRow <= excelAddress._toRow; ++fromRow)
        {
          if (this._worksheet._flags.GetFlagValue(fromRow, fromCol, CellFlags.Merged) != startValue)
            return false;
        }
      }
      return true;
    }

    internal void SetCellMerge(bool value, string address)
    {
      ExcelAddress excelAddress = new ExcelAddress(address);
      for (int fromCol = excelAddress._fromCol; fromCol <= excelAddress._toCol; ++fromCol)
      {
        for (int fromRow = excelAddress._fromRow; fromRow <= excelAddress._toRow; ++fromRow)
          this._worksheet._flags.SetFlagValue(fromRow, fromCol, value, CellFlags.Merged);
      }
    }

    internal void SetValueRichText(object value)
    {
      if (this._fromRow == 1 && this._fromCol == 1 && this._toRow == 1048576 && this._toCol == 16384)
      {
        this.SetValue(value, 1, 1);
      }
      else
      {
        for (int fromCol = this._fromCol; fromCol <= this._toCol; ++fromCol)
        {
          for (int fromRow = this._fromRow; fromRow <= this._toRow; ++fromRow)
            this.SetValue(value, fromRow, fromCol);
        }
      }
    }

    private void SetValue(object value, int row, int col)
    {
      this._worksheet.SetValue(row, col, value);
      this._worksheet._formulas.SetValue(row, col, (object) "");
    }

    private void RemoveFormuls(ExcelAddress address)
    {
      List<int> intList = new List<int>();
      foreach (int key in this._worksheet._sharedFormulas.Keys)
      {
        int FromRow;
        int FromColumn;
        int ToRow;
        int ToColumn;
        ExcelCellBase.GetRowColFromAddress(this._worksheet._sharedFormulas[key].Address, out FromRow, out FromColumn, out ToRow, out ToColumn);
        if ((FromColumn >= address.Start.Column && FromColumn <= address.End.Column || ToColumn >= address.Start.Column && ToColumn <= address.End.Column) && (FromRow >= address.Start.Row && FromRow <= address.End.Row || ToRow >= address.Start.Row && ToRow <= address.End.Row))
        {
          for (int Column = FromColumn; Column <= ToColumn; ++Column)
          {
            for (int Row = FromRow; Row <= ToRow; ++Row)
              this._worksheet._formulas.SetValue(Row, Column, (object) int.MinValue);
          }
          intList.Add(key);
        }
      }
      foreach (int key in intList)
        this._worksheet._sharedFormulas.Remove(key);
    }

    internal void SetSharedFormulaID(int id)
    {
      for (int fromCol = this._fromCol; fromCol <= this._toCol; ++fromCol)
      {
        for (int fromRow = this._fromRow; fromRow <= this._toRow; ++fromRow)
          this._worksheet._formulas.SetValue(fromRow, fromCol, (object) id);
      }
    }

    private void CheckAndSplitSharedFormula()
    {
      for (int fromCol = this._fromCol; fromCol <= this._toCol; ++fromCol)
      {
        for (int fromRow = this._fromRow; fromRow <= this._toRow; ++fromRow)
        {
          if (this._worksheet._formulas.GetValue(fromRow, fromCol) is int num && num >= 0)
          {
            this.SplitFormulas();
            return;
          }
        }
      }
    }

    private void SplitFormulas()
    {
      List<int> intList = new List<int>();
      for (int fromCol = this._fromCol; fromCol <= this._toCol; ++fromCol)
      {
        for (int fromRow = this._fromRow; fromRow <= this._toRow; ++fromRow)
        {
          if (this._worksheet._formulas.GetValue(fromRow, fromCol) is int key && key >= 0 && !intList.Contains(key))
          {
            if (this._worksheet._sharedFormulas[key].IsArray && this.Collide((ExcelAddressBase) this._worksheet.Cells[this._worksheet._sharedFormulas[key].Address]) == ExcelAddressBase.eAddressCollition.Partly)
              throw new Exception("Can not overwrite a part of an array-formula");
            intList.Add(key);
          }
        }
      }
      foreach (int ix in intList)
        this.SplitFormula(ix);
    }

    private void SplitFormula(int ix)
    {
      ExcelWorksheet.Formulas formulas = this._worksheet._sharedFormulas[ix];
      ExcelRange cell = this._worksheet.Cells[formulas.Address];
      switch (this.Collide((ExcelAddressBase) cell))
      {
        case ExcelAddressBase.eAddressCollition.Partly:
          bool flag = false;
          string formulaR1C1 = cell.FormulaR1C1;
          if (cell._fromRow < this._fromRow)
          {
            formulas.Address = ExcelCellBase.GetAddress(cell._fromRow, cell._fromCol, this._fromRow - 1, cell._toCol);
            flag = true;
          }
          if (cell._fromCol < this._fromCol)
          {
            if (flag)
            {
              formulas = new ExcelWorksheet.Formulas(SourceCodeTokenizer.Default);
              formulas.Index = this._worksheet.GetMaxShareFunctionIndex(false);
              formulas.StartCol = cell._fromCol;
              formulas.IsArray = false;
              this._worksheet._sharedFormulas.Add(formulas.Index, formulas);
            }
            else
              flag = true;
            formulas.StartRow = cell._fromRow >= this._fromRow ? cell._fromRow : this._fromRow;
            formulas.Address = cell._toRow >= this._toRow ? ExcelCellBase.GetAddress(formulas.StartRow, formulas.StartCol, this._toRow, this._fromCol - 1) : ExcelCellBase.GetAddress(formulas.StartRow, formulas.StartCol, cell._toRow, this._fromCol - 1);
            formulas.Formula = ExcelCellBase.TranslateFromR1C1(formulaR1C1, formulas.StartRow, formulas.StartCol);
            this._worksheet.Cells[formulas.Address].SetSharedFormulaID(formulas.Index);
          }
          if (cell._toCol > this._toCol)
          {
            if (flag)
            {
              formulas = new ExcelWorksheet.Formulas(SourceCodeTokenizer.Default);
              formulas.Index = this._worksheet.GetMaxShareFunctionIndex(false);
              formulas.IsArray = false;
              this._worksheet._sharedFormulas.Add(formulas.Index, formulas);
            }
            else
              flag = true;
            formulas.StartCol = this._toCol + 1;
            formulas.StartRow = this._fromRow >= cell._fromRow ? this._fromRow : cell._fromRow;
            formulas.Address = cell._toRow >= this._toRow ? ExcelCellBase.GetAddress(formulas.StartRow, formulas.StartCol, this._toRow, cell._toCol) : ExcelCellBase.GetAddress(formulas.StartRow, formulas.StartCol, cell._toRow, cell._toCol);
            formulas.Formula = ExcelCellBase.TranslateFromR1C1(formulaR1C1, formulas.StartRow, formulas.StartCol);
            this._worksheet.Cells[formulas.Address].SetSharedFormulaID(formulas.Index);
          }
          if (cell._toRow <= this._toRow)
            break;
          if (flag)
          {
            formulas = new ExcelWorksheet.Formulas(SourceCodeTokenizer.Default);
            formulas.Index = this._worksheet.GetMaxShareFunctionIndex(false);
            formulas.IsArray = false;
            this._worksheet._sharedFormulas.Add(formulas.Index, formulas);
          }
          formulas.StartCol = cell._fromCol;
          formulas.StartRow = this._toRow + 1;
          formulas.Formula = ExcelCellBase.TranslateFromR1C1(formulaR1C1, formulas.StartRow, formulas.StartCol);
          formulas.Address = ExcelCellBase.GetAddress(formulas.StartRow, formulas.StartCol, cell._toRow, cell._toCol);
          this._worksheet.Cells[formulas.Address].SetSharedFormulaID(formulas.Index);
          break;
        case ExcelAddressBase.eAddressCollition.Inside:
          this._worksheet._sharedFormulas.Remove(ix);
          cell.SetSharedFormulaID(int.MinValue);
          break;
      }
    }

    private object ConvertData(ExcelTextFormat Format, string v, int col, bool isText)
    {
      if (isText && (Format.DataTypes == null || Format.DataTypes.Length < col))
        return (object) v;
      if (Format.DataTypes == null || Format.DataTypes.Length < col || Format.DataTypes[col] == eDataTypes.Unknown)
      {
        string s = v.EndsWith("%") ? v.Substring(0, v.Length - 1) : v;
        double result1;
        DateTime result2;
        return double.TryParse(s, NumberStyles.Any, (IFormatProvider) Format.Culture, out result1) ? (s == v ? (object) result1 : (object) (result1 / 100.0)) : (DateTime.TryParse(v, (IFormatProvider) Format.Culture, DateTimeStyles.None, out result2) ? (object) result2 : (object) v);
      }
      switch (Format.DataTypes[col])
      {
        case eDataTypes.Number:
          double result3;
          return double.TryParse(v, NumberStyles.Any, (IFormatProvider) Format.Culture, out result3) ? (object) result3 : (object) v;
        case eDataTypes.DateTime:
          DateTime result4;
          return DateTime.TryParse(v, (IFormatProvider) Format.Culture, DateTimeStyles.None, out result4) ? (object) result4 : (object) v;
        case eDataTypes.Percent:
          double result5;
          return double.TryParse(v.EndsWith("%") ? v.Substring(0, v.Length - 1) : v, NumberStyles.Any, (IFormatProvider) Format.Culture, out result5) ? (object) (result5 / 100.0) : (object) v;
        default:
          return (object) v;
      }
    }

    public IRangeConditionalFormatting ConditionalFormatting
    {
      get
      {
        return (IRangeConditionalFormatting) new RangeConditionalFormatting(this._worksheet, new ExcelAddress(this.Address));
      }
    }

    public IRangeDataValidation DataValidation
    {
      get => (IRangeDataValidation) new RangeDataValidation(this._worksheet, this.Address);
    }

    public ExcelRangeBase LoadFromDataReader(
      IDataReader Reader,
      bool PrintHeaders,
      string TableName,
      TableStyles TableStyle = TableStyles.None)
    {
      ExcelRangeBase excelRangeBase = this.LoadFromDataReader(Reader, PrintHeaders);
      int num = excelRangeBase.Rows - 1;
      if (num >= 0 && excelRangeBase.Columns > 0)
      {
        ExcelTable excelTable = this._worksheet.Tables.Add(new ExcelAddressBase(this._fromRow, this._fromCol, this._fromRow + (num <= 0 ? 1 : num), this._fromCol + excelRangeBase.Columns - 1), TableName);
        excelTable.ShowHeader = PrintHeaders;
        excelTable.TableStyle = TableStyle;
      }
      return excelRangeBase;
    }

    public ExcelRangeBase LoadFromDataReader(IDataReader Reader, bool PrintHeaders)
    {
      int num = Reader != null ? Reader.FieldCount : throw new ArgumentNullException(nameof (Reader), "Reader can't be null");
      int fromCol = this._fromCol;
      int fromRow = this._fromRow;
      if (PrintHeaders)
      {
        for (int i = 0; i < num; ++i)
          this._worksheet._values.SetValue(fromRow, fromCol++, (object) Reader.GetName(i));
        ++fromRow;
        fromCol = this._fromCol;
      }
      while (Reader.Read())
      {
        for (int i = 0; i < num; ++i)
          this._worksheet._values.SetValue(fromRow, fromCol++, Reader.GetValue(i));
        ++fromRow;
        fromCol = this._fromCol;
      }
      return (ExcelRangeBase) this._worksheet.Cells[this._fromRow, this._fromCol, fromRow - 1, this._fromCol + num - 1];
    }

    public ExcelRangeBase LoadFromDataTable(
      DataTable Table,
      bool PrintHeaders,
      TableStyles TableStyle)
    {
      ExcelRangeBase excelRangeBase = this.LoadFromDataTable(Table, PrintHeaders);
      int num = Table.Rows.Count + (PrintHeaders ? 1 : 0) - 1;
      if (num >= 0 && Table.Columns.Count > 0)
      {
        ExcelTable excelTable = this._worksheet.Tables.Add(new ExcelAddressBase(this._fromRow, this._fromCol, this._fromRow + (num == 0 ? 1 : num), this._fromCol + Table.Columns.Count - 1), Table.TableName);
        excelTable.ShowHeader = PrintHeaders;
        excelTable.TableStyle = TableStyle;
      }
      return excelRangeBase;
    }

    public ExcelRangeBase LoadFromDataTable(DataTable Table, bool PrintHeaders)
    {
      if (Table == null)
        throw new ArgumentNullException("Table can't be null");
      int fromCol = this._fromCol;
      int fromRow = this._fromRow;
      if (PrintHeaders)
      {
        foreach (DataColumn column in (InternalDataCollectionBase) Table.Columns)
          this._worksheet._values.SetValue(fromRow, fromCol++, (object) column.Caption);
        ++fromRow;
        fromCol = this._fromCol;
      }
      foreach (DataRow row in (InternalDataCollectionBase) Table.Rows)
      {
        foreach (object obj in row.ItemArray)
          this._worksheet._values.SetValue(fromRow, fromCol++, obj);
        ++fromRow;
        fromCol = this._fromCol;
      }
      return (ExcelRangeBase) this._worksheet.Cells[this._fromRow, this._fromCol, fromRow - 1, this._fromCol + Table.Columns.Count - 1];
    }

    public ExcelRangeBase LoadFromArrays(IEnumerable<object[]> Data)
    {
      if (Data == null)
        throw new ArgumentNullException("data");
      int fromCol = this._fromCol;
      int fromRow = this._fromRow;
      foreach (object[] objArray in Data)
      {
        fromCol = this._fromCol;
        foreach (object obj in objArray)
        {
          this._worksheet._values.SetValue(fromRow, fromCol, obj);
          ++fromCol;
        }
        ++fromRow;
      }
      return (ExcelRangeBase) this._worksheet.Cells[this._fromRow, this._fromCol, fromRow - 1, fromCol - 1];
    }

    public ExcelRangeBase LoadFromCollection<T>(IEnumerable<T> Collection)
    {
      return this.LoadFromCollection<T>(Collection, false, TableStyles.None, BindingFlags.Instance | BindingFlags.Public, (MemberInfo[]) null);
    }

    public ExcelRangeBase LoadFromCollection<T>(IEnumerable<T> Collection, bool PrintHeaders)
    {
      return this.LoadFromCollection<T>(Collection, PrintHeaders, TableStyles.None, BindingFlags.Instance | BindingFlags.Public, (MemberInfo[]) null);
    }

    public ExcelRangeBase LoadFromCollection<T>(
      IEnumerable<T> Collection,
      bool PrintHeaders,
      TableStyles TableStyle)
    {
      return this.LoadFromCollection<T>(Collection, PrintHeaders, TableStyle, BindingFlags.Instance | BindingFlags.Public, (MemberInfo[]) null);
    }

    public ExcelRangeBase LoadFromCollection<T>(
      IEnumerable<T> Collection,
      bool PrintHeaders,
      TableStyles TableStyle,
      BindingFlags memberFlags,
      MemberInfo[] Members)
    {
      Type type = typeof (T);
      if (Members == null)
      {
        Members = (MemberInfo[]) type.GetProperties(memberFlags);
      }
      else
      {
        foreach (MemberInfo member in Members)
        {
          if (member.DeclaringType != type)
            throw new Exception("Supplied properties in parameter Properties must be of the same type as T");
        }
      }
      int fromCol = this._fromCol;
      int fromRow = this._fromRow;
      if (Members.Length > 0 && PrintHeaders)
      {
        foreach (MemberInfo member in Members)
        {
          DescriptionAttribute descriptionAttribute = ((IEnumerable<object>) member.GetCustomAttributes(typeof (DescriptionAttribute), false)).FirstOrDefault<object>() as DescriptionAttribute;
          string empty = string.Empty;
          string str = descriptionAttribute == null ? (!(((IEnumerable<object>) member.GetCustomAttributes(typeof (DisplayNameAttribute), false)).FirstOrDefault<object>() is DisplayNameAttribute displayNameAttribute) ? member.Name.Replace('_', ' ') : displayNameAttribute.DisplayName) : descriptionAttribute.Description;
          this._worksheet._values.SetValue(fromRow, fromCol++, (object) str);
        }
        ++fromRow;
      }
      if (Members.Length == 0)
      {
        foreach (T obj in Collection)
          this._worksheet.Cells[fromRow++, fromCol].Value = (object) obj;
      }
      else
      {
        foreach (T obj in Collection)
        {
          fromCol = this._fromCol;
          if ((object) obj is string || (object) obj is Decimal || (object) obj is DateTime || obj.GetType().IsPrimitive)
          {
            this._worksheet.Cells[fromRow, fromCol++].Value = (object) obj;
          }
          else
          {
            foreach (MemberInfo member in Members)
            {
              switch (member)
              {
                case PropertyInfo _:
                  this._worksheet.Cells[fromRow, fromCol++].Value = ((PropertyInfo) member).GetValue((object) obj, (object[]) null);
                  break;
                case FieldInfo _:
                  this._worksheet.Cells[fromRow, fromCol++].Value = ((FieldInfo) member).GetValue((object) obj);
                  break;
                case MethodInfo _:
                  this._worksheet.Cells[fromRow, fromCol++].Value = ((MethodBase) member).Invoke((object) obj, (object[]) null);
                  break;
              }
            }
          }
          ++fromRow;
        }
      }
      ExcelRange cell = this._worksheet.Cells[this._fromRow, this._fromCol, fromRow - 1, Members.Length == 0 ? fromCol : fromCol - 1];
      if (TableStyle != TableStyles.None)
      {
        ExcelTable excelTable = this._worksheet.Tables.Add((ExcelAddressBase) cell, "");
        excelTable.ShowHeader = PrintHeaders;
        excelTable.TableStyle = TableStyle;
      }
      return (ExcelRangeBase) cell;
    }

    public ExcelRangeBase LoadFromText(string Text)
    {
      return this.LoadFromText(Text, new ExcelTextFormat());
    }

    public ExcelRangeBase LoadFromText(string Text, ExcelTextFormat Format)
    {
      if (string.IsNullOrEmpty(Text))
      {
        ExcelRange cell = this._worksheet.Cells[this._fromRow, this._fromCol];
        cell.Value = (object) "";
        return (ExcelRangeBase) cell;
      }
      if (Format == null)
        Format = new ExcelTextFormat();
      string[] strArray = Regex.Split(Text, Format.EOL);
      int fromRow = this._fromRow;
      int ToCol = this._fromCol;
      int num1 = 1;
      foreach (string str in strArray)
      {
        if (num1 > Format.SkipLinesBeginning && num1 <= strArray.Length - Format.SkipLinesEnd)
        {
          int fromCol = this._fromCol;
          string v = "";
          bool isText = false;
          bool flag = false;
          int num2 = 0;
          foreach (char ch in str)
          {
            if (Format.TextQualifier != char.MinValue && (int) ch == (int) Format.TextQualifier)
            {
              if (!isText && v != "")
                throw new Exception(string.Format("Invalid Text Qualifier in line : {0}", (object) str));
              flag = !flag;
              ++num2;
              isText = true;
            }
            else
            {
              if (num2 > 1 && !string.IsNullOrEmpty(v))
                v += new string(Format.TextQualifier, num2 / 2);
              else if (num2 > 2 && string.IsNullOrEmpty(v))
                v += new string(Format.TextQualifier, (num2 - 1) / 2);
              if (flag)
                v += (string) (object) ch;
              else if ((int) ch == (int) Format.Delimiter)
              {
                this._worksheet.SetValue(fromRow, fromCol, this.ConvertData(Format, v, fromCol - this._fromCol, isText));
                v = "";
                isText = false;
                ++fromCol;
              }
              else
              {
                if (num2 % 2 == 1)
                  throw new Exception(string.Format("Text delimiter is not closed in line : {0}", (object) str));
                v += (string) (object) ch;
              }
              num2 = 0;
            }
          }
          if (num2 > 1)
            v += new string(Format.TextQualifier, num2 / 2);
          this._worksheet._values.SetValue(fromRow, fromCol, this.ConvertData(Format, v, fromCol - this._fromCol, isText));
          if (fromCol > ToCol)
            ToCol = fromCol;
          ++fromRow;
        }
        ++num1;
      }
      return (ExcelRangeBase) this._worksheet.Cells[this._fromRow, this._fromCol, fromRow - 1, ToCol];
    }

    public ExcelRangeBase LoadFromText(
      string Text,
      ExcelTextFormat Format,
      TableStyles TableStyle,
      bool FirstRowIsHeader)
    {
      ExcelRangeBase Range = this.LoadFromText(Text, Format);
      ExcelTable excelTable = this._worksheet.Tables.Add((ExcelAddressBase) Range, "");
      excelTable.ShowHeader = FirstRowIsHeader;
      excelTable.TableStyle = TableStyle;
      return Range;
    }

    public ExcelRangeBase LoadFromText(FileInfo TextFile)
    {
      return this.LoadFromText(File.ReadAllText(TextFile.FullName, Encoding.ASCII));
    }

    public ExcelRangeBase LoadFromText(FileInfo TextFile, ExcelTextFormat Format)
    {
      return this.LoadFromText(File.ReadAllText(TextFile.FullName, Format.Encoding), Format);
    }

    public ExcelRangeBase LoadFromText(
      FileInfo TextFile,
      ExcelTextFormat Format,
      TableStyles TableStyle,
      bool FirstRowIsHeader)
    {
      return this.LoadFromText(File.ReadAllText(TextFile.FullName, Format.Encoding), Format, TableStyle, FirstRowIsHeader);
    }

    public T GetValue<T>() => this._worksheet.GetTypedValue<T>(this.Value);

    public ExcelRangeBase Offset(int RowOffset, int ColumnOffset)
    {
      if (this._fromRow + RowOffset < 1 || this._fromCol + ColumnOffset < 1 || this._fromRow + RowOffset > 1048576 || this._fromCol + ColumnOffset > 16384)
        throw new ArgumentOutOfRangeException("Offset value out of range");
      return new ExcelRangeBase(this._worksheet, ExcelCellBase.GetAddress(this._fromRow + RowOffset, this._fromCol + ColumnOffset, this._toRow + RowOffset, this._toCol + ColumnOffset));
    }

    public ExcelRangeBase Offset(
      int RowOffset,
      int ColumnOffset,
      int NumberOfRows,
      int NumberOfColumns)
    {
      if (NumberOfRows < 1 || NumberOfColumns < 1)
        throw new Exception("Number of rows/columns must be greater than 0");
      --NumberOfRows;
      --NumberOfColumns;
      if (this._fromRow + RowOffset < 1 || this._fromCol + ColumnOffset < 1 || this._fromRow + RowOffset > 1048576 || this._fromCol + ColumnOffset > 16384 || this._fromRow + RowOffset + NumberOfRows < 1 || this._fromCol + ColumnOffset + NumberOfColumns < 1 || this._fromRow + RowOffset + NumberOfRows > 1048576 || this._fromCol + ColumnOffset + NumberOfColumns > 16384)
        throw new ArgumentOutOfRangeException("Offset value out of range");
      return new ExcelRangeBase(this._worksheet, ExcelCellBase.GetAddress(this._fromRow + RowOffset, this._fromCol + ColumnOffset, this._fromRow + RowOffset + NumberOfRows, this._fromCol + ColumnOffset + NumberOfColumns));
    }

    public ExcelComment AddComment(string Text, string Author)
    {
      this._changePropMethod(new ExcelRangeBase._setValue(this.Exists_Comment), (object) null);
      this._changePropMethod(new ExcelRangeBase._setValue(this.Set_Comment), (object) new string[2]
      {
        Text,
        Author
      });
      return this._worksheet.Comments[new ExcelCellAddress(this._fromRow, this._fromCol)];
    }

    public void Copy(ExcelRangeBase Destination)
    {
      bool flag = Destination._worksheet.Workbook == this._worksheet.Workbook;
      ExcelStyles styles1 = this._worksheet.Workbook.Styles;
      ExcelStyles styles2 = Destination._worksheet.Workbook.Styles;
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      int rows = Destination._toRow - Destination._fromRow + 1;
      int columns = Destination._toCol - Destination._fromCol + 1;
      string str = "";
      int num1 = 0;
      object obj = (object) null;
      Uri uri = (Uri) null;
      ExcelComment excelComment = (ExcelComment) null;
      CellsStoreEnumerator<object> cellsStoreEnumerator1 = new CellsStoreEnumerator<object>(this._worksheet._values, this._fromRow, this._fromCol, this._toRow, this._toCol);
      List<ExcelRangeBase.CopiedCell> copiedCellList = new List<ExcelRangeBase.CopiedCell>();
      while (cellsStoreEnumerator1.Next())
      {
        int Row = Destination._fromRow + (cellsStoreEnumerator1.Row - this._fromRow);
        int Column = Destination._fromCol + (cellsStoreEnumerator1.Column - this._fromCol);
        ExcelRangeBase.CopiedCell copiedCell = new ExcelRangeBase.CopiedCell()
        {
          Row = Row,
          Column = Column,
          Value = cellsStoreEnumerator1.Value
        };
        if (this._worksheet._types.Exists(Row, Column, ref str))
          copiedCell.Type = str;
        if (this._worksheet._formulas.Exists(Row, Column, ref obj))
          copiedCell.Formula = !(obj is int) ? obj : (object) this._worksheet.GetFormula(cellsStoreEnumerator1.Row, cellsStoreEnumerator1.Column);
        if (this._worksheet._styles.Exists(Row, Column, ref num1))
        {
          if (flag)
          {
            copiedCell.StyleID = new int?(num1);
          }
          else
          {
            int num2 = this._worksheet._styles.GetValue(cellsStoreEnumerator1.Row, cellsStoreEnumerator1.Column);
            if (dictionary.ContainsKey(num2))
            {
              num1 = dictionary[num2];
            }
            else
            {
              num1 = styles2.CloneStyle(styles1, num2);
              dictionary.Add(num2, num1);
            }
            copiedCell.StyleID = new int?(num1);
          }
        }
        if (this._worksheet._hyperLinks.Exists(Row, Column, ref uri))
          copiedCell.HyperLink = uri;
        if (this._worksheet._commentsStore.Exists(Row, Column, ref excelComment))
          copiedCell.Comment = excelComment;
        copiedCellList.Add(copiedCell);
      }
      List<ExcelRangeBase.CopiedFlag> copiedFlagList = new List<ExcelRangeBase.CopiedFlag>();
      CellsStoreEnumerator<byte> cellsStoreEnumerator2 = new CellsStoreEnumerator<byte>((CellStore<byte>) this._worksheet._flags, this._fromRow, this._fromCol, this._toRow, this._toCol);
      while (cellsStoreEnumerator2.Next())
        copiedFlagList.Add(new ExcelRangeBase.CopiedFlag()
        {
          Row = Destination._fromRow + (cellsStoreEnumerator2.Row - this._fromRow),
          Column = Destination._fromCol + (cellsStoreEnumerator2.Column - this._fromCol),
          Flag = cellsStoreEnumerator2.Value
        });
      Destination._worksheet._values.Clear(Destination._fromRow, Destination._fromCol, rows, columns);
      Destination._worksheet._formulas.Clear(Destination._fromRow, Destination._fromCol, rows, columns);
      Destination._worksheet._styles.Clear(Destination._fromRow, Destination._fromCol, rows, columns);
      Destination._worksheet._types.Clear(Destination._fromRow, Destination._fromCol, rows, columns);
      Destination._worksheet._hyperLinks.Clear(Destination._fromRow, Destination._fromCol, rows, columns);
      Destination._worksheet._flags.Clear(Destination._fromRow, Destination._fromCol, rows, columns);
      Destination._worksheet._commentsStore.Clear(Destination._fromRow, Destination._fromCol, rows, columns);
      foreach (ExcelRangeBase.CopiedCell copiedCell in copiedCellList)
      {
        Destination._worksheet._values.SetValue(copiedCell.Row, copiedCell.Column, copiedCell.Value);
        if (copiedCell.Type != null)
          Destination._worksheet._types.SetValue(copiedCell.Row, copiedCell.Column, copiedCell.Type);
        if (copiedCell.StyleID.HasValue)
          Destination._worksheet._styles.SetValue(copiedCell.Row, copiedCell.Column, copiedCell.StyleID.Value);
        if (copiedCell.Formula != null)
          Destination._worksheet._formulas.SetValue(copiedCell.Row, copiedCell.Column, copiedCell.Formula);
        if (copiedCell.HyperLink != (Uri) null)
          Destination._worksheet._hyperLinks.SetValue(copiedCell.Row, copiedCell.Column, copiedCell.HyperLink);
        ExcelComment comment = copiedCell.Comment;
      }
      foreach (ExcelRangeBase.CopiedFlag copiedFlag in copiedFlagList)
        Destination._worksheet._flags.SetValue(copiedFlag.Row, copiedFlag.Column, copiedFlag.Flag);
    }

    public void Clear() => this.Delete((ExcelAddressBase) this, false);

    public void CreateArrayFormula(string ArrayFormula)
    {
      if (this.Addresses != null)
        throw new Exception("An Arrayformula can not have more than one address");
      this.Set_SharedFormula(ArrayFormula, (ExcelAddress) this, true);
    }

    internal void Delete(ExcelAddressBase Range, bool shift)
    {
      this.DeleteCheckMergedCells(Range);
      int rows = Range._toRow - Range._fromRow;
      int columns = Range._toCol - Range._fromCol;
      this._worksheet._values.Delete(Range._fromRow, Range._fromCol, rows, columns, shift);
      this._worksheet._types.Delete(Range._fromRow, Range._fromCol, rows, columns, shift);
      this._worksheet._styles.Delete(Range._fromRow, Range._fromCol, rows, columns, shift);
      this._worksheet._formulas.Delete(Range._fromRow, Range._fromCol, rows, columns, shift);
      this._worksheet._hyperLinks.Delete(Range._fromRow, Range._fromCol, rows, columns, shift);
      this._worksheet._flags.Delete(Range._fromRow, Range._fromCol, rows, columns, shift);
      this._worksheet._commentsStore.Delete(Range._fromRow, Range._fromCol, rows, columns, shift);
      if (this.Addresses == null)
        return;
      foreach (ExcelAddressBase address in this.Addresses)
        this.Delete(address, shift);
    }

    private void DeleteCheckMergedCells(ExcelAddressBase Range)
    {
      List<string> stringList = new List<string>();
      foreach (string mergedCell in this.Worksheet.MergedCells)
      {
        switch (Range.Collide((ExcelAddressBase) new ExcelAddress(Range.WorkSheet, mergedCell)))
        {
          case ExcelAddressBase.eAddressCollition.No:
            continue;
          case ExcelAddressBase.eAddressCollition.Inside:
            stringList.Add(mergedCell);
            continue;
          default:
            throw new InvalidOperationException("Can't remove/overwrite a part of cells that are merged");
        }
      }
      foreach (string str in stringList)
        this.Worksheet.MergedCells.Remove(str);
    }

    public void Dispose()
    {
    }

    public IEnumerator<ExcelRangeBase> GetEnumerator()
    {
      this.Reset();
      return (IEnumerator<ExcelRangeBase>) this;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      this.Reset();
      return (IEnumerator) this;
    }

    public ExcelRangeBase Current
    {
      get
      {
        return new ExcelRangeBase(this._worksheet, ExcelCellBase.GetAddress(this.cellEnum.Row, this.cellEnum.Column));
      }
    }

    object IEnumerator.Current
    {
      get
      {
        return (object) new ExcelRangeBase(this._worksheet, ExcelCellBase.GetAddress(this.cellEnum.Row, this.cellEnum.Column));
      }
    }

    public bool MoveNext()
    {
      if (this.cellEnum.Next())
        return true;
      if (this._addresses == null)
        return false;
      ++this._enumAddressIx;
      if (this._enumAddressIx >= this._addresses.Count)
        return false;
      this.cellEnum = new CellsStoreEnumerator<object>(this._worksheet._values, this._addresses[this._enumAddressIx]._fromRow, this._addresses[this._enumAddressIx]._fromCol, this._addresses[this._enumAddressIx]._toRow, this._addresses[this._enumAddressIx]._toCol);
      return this.MoveNext();
    }

    public void Reset()
    {
      this._enumAddressIx = -1;
      this.cellEnum = new CellsStoreEnumerator<object>(this._worksheet._values, this._fromRow, this._fromCol, this._toRow, this._toCol);
    }

    private delegate void _changeProp(ExcelRangeBase._setValue method, object value);

    private delegate void _setValue(object value, int row, int col);

    private class CopiedCell
    {
      internal int Row { get; set; }

      internal int Column { get; set; }

      internal object Value { get; set; }

      internal string Type { get; set; }

      internal object Formula { get; set; }

      internal int? StyleID { get; set; }

      internal Uri HyperLink { get; set; }

      internal ExcelComment Comment { get; set; }
    }

    private class CopiedFlag
    {
      internal int Row { get; set; }

      internal int Column { get; set; }

      internal byte Flag { get; set; }
    }
  }
}
