// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelColumn
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Style;
using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelColumn : IRangeID
  {
    private ExcelWorksheet _worksheet;
    private XmlElement _colElement;
    private int _columnMin;
    internal int _columnMax;
    internal bool _hidden;
    internal double _width;
    internal string _styleName = "";

    protected internal ExcelColumn(ExcelWorksheet Worksheet, int col)
    {
      this._worksheet = Worksheet;
      this._columnMin = col;
      this._columnMax = col;
      this._width = this._worksheet.DefaultColWidth;
    }

    public int ColumnMin => this._columnMin;

    public int ColumnMax
    {
      get => this._columnMax;
      set
      {
        if (value < this._columnMin && value > 16384)
          throw new Exception("ColumnMax out of range");
        CellsStoreEnumerator<object> cellsStoreEnumerator = new CellsStoreEnumerator<object>(this._worksheet._values, 0, 0, 0, 16384);
        while (cellsStoreEnumerator.Next())
        {
          ExcelColumn excelColumn = cellsStoreEnumerator.Value as ExcelColumn;
          if (cellsStoreEnumerator.Column > this._columnMin && excelColumn.ColumnMax <= value && cellsStoreEnumerator.Column != this._columnMin)
            throw new Exception(string.Format("ColumnMax can not span over existing column {0}.", (object) excelColumn.ColumnMin));
        }
        this._columnMax = value;
      }
    }

    internal ulong ColumnID => ExcelColumn.GetColumnID(this._worksheet.SheetID, this.ColumnMin);

    public bool Hidden
    {
      get => this._hidden;
      set
      {
        if (this._worksheet._package.DoAdjustDrawings)
        {
          int[,] drawingWidths = this._worksheet.Drawings.GetDrawingWidths();
          this._hidden = value;
          this._worksheet.Drawings.AdjustWidth(drawingWidths);
        }
        else
          this._hidden = value;
      }
    }

    internal double VisualWidth
    {
      get => this._hidden || this.Collapsed && this.OutlineLevel > 0 ? 0.0 : this._width;
    }

    public double Width
    {
      get => this._width;
      set
      {
        if (this._worksheet._package.DoAdjustDrawings)
        {
          int[,] drawingWidths = this._worksheet.Drawings.GetDrawingWidths();
          this._width = value;
          this._worksheet.Drawings.AdjustWidth(drawingWidths);
        }
        else
          this._width = value;
        if (!this._hidden || value == 0.0)
          return;
        this._hidden = false;
      }
    }

    public bool BestFit { get; set; }

    public bool Collapsed { get; set; }

    public int OutlineLevel { get; set; }

    public bool Phonetic { get; set; }

    public ExcelStyle Style
    {
      get
      {
        return this._worksheet.Workbook.Styles.GetStyleObject(this.StyleID, this._worksheet.PositionID, ExcelCellBase.GetColumnLetter(this.ColumnMin) + ":" + ExcelCellBase.GetColumnLetter(this.ColumnMax));
      }
    }

    public string StyleName
    {
      get => this._styleName;
      set
      {
        this.StyleID = this._worksheet.Workbook.Styles.GetStyleIdFromName(value);
        this._styleName = value;
      }
    }

    public int StyleID
    {
      get => this._worksheet._styles.GetValue(0, this.ColumnMin);
      set => this._worksheet._styles.SetValue(0, this.ColumnMin, value);
    }

    public bool PageBreak { get; set; }

    public override string ToString()
    {
      return string.Format("Column Range: {0} to {1}", (object) this._colElement.GetAttribute("min"), (object) this._colElement.GetAttribute("min"));
    }

    public void AutoFit()
    {
      this._worksheet.Cells[1, this._columnMin, 1048576, this._columnMax].AutoFitColumns();
    }

    public void AutoFit(double MinimumWidth)
    {
      this._worksheet.Cells[1, this._columnMin, 1048576, this._columnMax].AutoFitColumns(MinimumWidth);
    }

    public void AutoFit(double MinimumWidth, double MaximumWidth)
    {
      this._worksheet.Cells[1, this._columnMin, 1048576, this._columnMax].AutoFitColumns(MinimumWidth, MaximumWidth);
    }

    internal static ulong GetColumnID(int sheetID, int column)
    {
      return (ulong) sheetID + ((ulong) column << 15);
    }

    ulong IRangeID.RangeID
    {
      get => this.ColumnID;
      set
      {
        int columnMin = this._columnMin;
        this._columnMin = (int) (value >> 15) & 1023;
        this._columnMax += columnMin - this.ColumnMin;
        if (this._columnMax <= 16384)
          return;
        this._columnMax = 16384;
      }
    }

    internal ExcelColumn Clone(ExcelWorksheet added) => this.Clone(added, this.ColumnMin);

    internal ExcelColumn Clone(ExcelWorksheet added, int col)
    {
      ExcelColumn excelColumn = added.Column(col);
      excelColumn.ColumnMax = this.ColumnMax;
      excelColumn.BestFit = this.BestFit;
      excelColumn.Collapsed = this.Collapsed;
      excelColumn.OutlineLevel = this.OutlineLevel;
      excelColumn.PageBreak = this.PageBreak;
      excelColumn.Phonetic = this.Phonetic;
      excelColumn._styleName = this._styleName;
      excelColumn.StyleID = this.StyleID;
      excelColumn.Width = this.Width;
      excelColumn.Hidden = this.Hidden;
      return excelColumn;
    }
  }
}
