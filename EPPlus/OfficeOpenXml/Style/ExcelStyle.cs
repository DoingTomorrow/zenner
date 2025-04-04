// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.ExcelStyle
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Style.XmlAccess;
using System;

#nullable disable
namespace OfficeOpenXml.Style
{
  public sealed class ExcelStyle : StyleBase
  {
    private const string xfIdPath = "@xfId";

    internal ExcelStyle(
      ExcelStyles styles,
      XmlHelper.ChangedEventHandler ChangedEvent,
      int positionID,
      string Address,
      int xfsId)
      : base(styles, ChangedEvent, positionID, Address)
    {
      this.Index = xfsId;
      ExcelXfs excelXfs = positionID <= -1 ? this._styles.CellStyleXfs[xfsId] : this._styles.CellXfs[xfsId];
      this.Styles = styles;
      this.PositionID = positionID;
      this.Numberformat = new ExcelNumberFormat(styles, ChangedEvent, this.PositionID, Address, excelXfs.NumberFormatId);
      this.Font = new ExcelFont(styles, ChangedEvent, this.PositionID, Address, excelXfs.FontId);
      this.Fill = new ExcelFill(styles, ChangedEvent, this.PositionID, Address, excelXfs.FillId);
      this.Border = new Border(styles, ChangedEvent, this.PositionID, Address, excelXfs.BorderId);
    }

    public ExcelNumberFormat Numberformat { get; set; }

    public ExcelFont Font { get; set; }

    public ExcelFill Fill { get; set; }

    public Border Border { get; set; }

    public ExcelHorizontalAlignment HorizontalAlignment
    {
      get => this._styles.CellXfs[this.Index].HorizontalAlignment;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Style, eStyleProperty.HorizontalAlign, (object) value, this._positionID, this._address));
      }
    }

    public ExcelVerticalAlignment VerticalAlignment
    {
      get => this._styles.CellXfs[this.Index].VerticalAlignment;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Style, eStyleProperty.VerticalAlign, (object) value, this._positionID, this._address));
      }
    }

    public bool WrapText
    {
      get => this._styles.CellXfs[this.Index].WrapText;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Style, eStyleProperty.WrapText, (object) value, this._positionID, this._address));
      }
    }

    public ExcelReadingOrder ReadingOrder
    {
      get => this._styles.CellXfs[this.Index].ReadingOrder;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Style, eStyleProperty.ReadingOrder, (object) value, this._positionID, this._address));
      }
    }

    public bool ShrinkToFit
    {
      get => this._styles.CellXfs[this.Index].ShrinkToFit;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Style, eStyleProperty.ShrinkToFit, (object) value, this._positionID, this._address));
      }
    }

    public int Indent
    {
      get => this._styles.CellXfs[this.Index].Indent;
      set
      {
        if (value < 0 || value > 250)
          throw new ArgumentOutOfRangeException("Indent must be between 0 and 250");
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Style, eStyleProperty.Indent, (object) value, this._positionID, this._address));
      }
    }

    public int TextRotation
    {
      get => this._styles.CellXfs[this.Index].TextRotation;
      set
      {
        if (value < 0 || value > 180)
          throw new ArgumentOutOfRangeException("TextRotation out of range.");
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Style, eStyleProperty.TextRotation, (object) value, this._positionID, this._address));
      }
    }

    public bool Locked
    {
      get => this._styles.CellXfs[this.Index].Locked;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Style, eStyleProperty.Locked, (object) value, this._positionID, this._address));
      }
    }

    public bool Hidden
    {
      get => this._styles.CellXfs[this.Index].Hidden;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Style, eStyleProperty.Hidden, (object) value, this._positionID, this._address));
      }
    }

    public int XfId
    {
      get => this._styles.CellXfs[this.Index].XfId;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Style, eStyleProperty.XfId, (object) value, this._positionID, this._address));
      }
    }

    internal int PositionID { get; set; }

    internal ExcelStyles Styles { get; set; }

    internal override string Id
    {
      get
      {
        return this.Numberformat.Id + "|" + this.Font.Id + "|" + this.Fill.Id + "|" + this.Border.Id + "|" + (object) this.VerticalAlignment + "|" + (object) this.HorizontalAlignment + "|" + this.WrapText.ToString() + "|" + this.ReadingOrder.ToString() + "|" + this.XfId.ToString();
      }
    }
  }
}
