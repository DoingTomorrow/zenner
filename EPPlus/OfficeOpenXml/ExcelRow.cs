// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelRow
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Style;
using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelRow : IRangeID
  {
    private ExcelWorksheet _worksheet;
    private XmlElement _rowElement;
    private bool _hidden;
    private double _height = -1.0;
    internal string _styleName = "";

    [Obsolete]
    public ulong RowID => ExcelRow.GetRowID(this._worksheet.SheetID, this.Row);

    internal ExcelRow(ExcelWorksheet Worksheet, int row)
    {
      this._worksheet = Worksheet;
      this.Row = row;
    }

    internal XmlNode Node => (XmlNode) this._rowElement;

    public bool Hidden
    {
      get
      {
        RowInternal rowInternal = (RowInternal) this._worksheet._values.GetValue(this.Row, 0);
        return rowInternal != null && rowInternal.Hidden;
      }
      set => this.GetRowInternal().Hidden = value;
    }

    public double Height
    {
      get
      {
        RowInternal rowInternal = (RowInternal) this._worksheet._values.GetValue(this.Row, 0);
        return rowInternal == null || rowInternal.Height < 0.0 ? this._worksheet.DefaultRowHeight : rowInternal.Height;
      }
      set
      {
        RowInternal rowInternal = this.GetRowInternal();
        if (this._worksheet._package.DoAdjustDrawings)
        {
          int[,] drawingWidths = this._worksheet.Drawings.GetDrawingWidths();
          rowInternal.Height = value;
          this._worksheet.Drawings.AdjustHeight(drawingWidths);
        }
        else
          rowInternal.Height = value;
        if (rowInternal.Hidden && value != 0.0)
          this.Hidden = false;
        rowInternal.CustomHeight = value != this._worksheet.DefaultRowHeight;
      }
    }

    public bool CustomHeight
    {
      get
      {
        RowInternal rowInternal = (RowInternal) this._worksheet._values.GetValue(this.Row, 0);
        return rowInternal != null && rowInternal.CustomHeight;
      }
      set => this.GetRowInternal().CustomHeight = value;
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
      get => this._worksheet._styles.GetValue(this.Row, 0);
      set => this._worksheet._styles.SetValue(this.Row, 0, value);
    }

    public int Row { get; set; }

    public bool Collapsed
    {
      get
      {
        RowInternal rowInternal = (RowInternal) this._worksheet._values.GetValue(this.Row, 0);
        return rowInternal != null && rowInternal.Collapsed;
      }
      set => this.GetRowInternal().Collapsed = value;
    }

    public int OutlineLevel
    {
      get
      {
        RowInternal rowInternal = (RowInternal) this._worksheet._values.GetValue(this.Row, 0);
        return rowInternal == null ? 0 : (int) rowInternal.OutlineLevel;
      }
      set => this.GetRowInternal().OutlineLevel = (short) value;
    }

    private RowInternal GetRowInternal()
    {
      RowInternal rowInternal = (RowInternal) this._worksheet._values.GetValue(this.Row, 0);
      if (rowInternal == null)
      {
        rowInternal = new RowInternal();
        this._worksheet._values.SetValue(this.Row, 0, (object) rowInternal);
      }
      return rowInternal;
    }

    public bool Phonetic
    {
      get
      {
        RowInternal rowInternal = (RowInternal) this._worksheet._values.GetValue(this.Row, 0);
        return rowInternal != null && rowInternal.Phonetic;
      }
      set => this.GetRowInternal().Phonetic = value;
    }

    public ExcelStyle Style
    {
      get
      {
        return this._worksheet.Workbook.Styles.GetStyleObject(this.StyleID, this._worksheet.PositionID, this.Row.ToString() + ":" + this.Row.ToString());
      }
    }

    public bool PageBreak
    {
      get
      {
        RowInternal rowInternal = (RowInternal) this._worksheet._values.GetValue(this.Row, 0);
        return rowInternal != null && rowInternal.PageBreak;
      }
      set => this.GetRowInternal().PageBreak = value;
    }

    internal static ulong GetRowID(int sheetID, int row) => (ulong) sheetID + ((ulong) row << 29);

    ulong IRangeID.RangeID
    {
      get => this.RowID;
      set => this.Row = (int) (value >> 29);
    }

    internal void Clone(ExcelWorksheet added)
    {
      ExcelRow excelRow = added.Row(this.Row);
      excelRow.Collapsed = this.Collapsed;
      excelRow.Height = this.Height;
      excelRow.Hidden = this.Hidden;
      excelRow.OutlineLevel = this.OutlineLevel;
      excelRow.PageBreak = this.PageBreak;
      excelRow.Phonetic = this.Phonetic;
      excelRow._styleName = this._styleName;
      excelRow.StyleID = this.StyleID;
    }
  }
}
