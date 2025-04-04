// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.Border
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Drawing;

#nullable disable
namespace OfficeOpenXml.Style
{
  public sealed class Border : StyleBase
  {
    internal Border(
      ExcelStyles styles,
      XmlHelper.ChangedEventHandler ChangedEvent,
      int PositionID,
      string address,
      int index)
      : base(styles, ChangedEvent, PositionID, address)
    {
      this.Index = index;
    }

    public ExcelBorderItem Left
    {
      get
      {
        return new ExcelBorderItem(this._styles, this._ChangedEvent, this._positionID, this._address, eStyleClass.BorderLeft, (StyleBase) this);
      }
    }

    public ExcelBorderItem Right
    {
      get
      {
        return new ExcelBorderItem(this._styles, this._ChangedEvent, this._positionID, this._address, eStyleClass.BorderRight, (StyleBase) this);
      }
    }

    public ExcelBorderItem Top
    {
      get
      {
        return new ExcelBorderItem(this._styles, this._ChangedEvent, this._positionID, this._address, eStyleClass.BorderTop, (StyleBase) this);
      }
    }

    public ExcelBorderItem Bottom
    {
      get
      {
        return new ExcelBorderItem(this._styles, this._ChangedEvent, this._positionID, this._address, eStyleClass.BorderBottom, (StyleBase) this);
      }
    }

    public ExcelBorderItem Diagonal
    {
      get
      {
        return new ExcelBorderItem(this._styles, this._ChangedEvent, this._positionID, this._address, eStyleClass.BorderDiagonal, (StyleBase) this);
      }
    }

    public bool DiagonalUp
    {
      get => this.Index >= 0 && this._styles.Borders[this.Index].DiagonalUp;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Border, eStyleProperty.BorderDiagonalUp, (object) value, this._positionID, this._address));
      }
    }

    public bool DiagonalDown
    {
      get => this.Index >= 0 && this._styles.Borders[this.Index].DiagonalDown;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Border, eStyleProperty.BorderDiagonalDown, (object) value, this._positionID, this._address));
      }
    }

    internal override string Id
    {
      get
      {
        return this.Top.Id + this.Bottom.Id + this.Left.Id + this.Right.Id + this.Diagonal.Id + (object) this.DiagonalUp + (object) this.DiagonalDown;
      }
    }

    public void BorderAround(ExcelBorderStyle Style)
    {
      ExcelAddress addr = new ExcelAddress(this._address);
      this.SetBorderAroundStyle(Style, addr);
    }

    public void BorderAround(ExcelBorderStyle Style, Color Color)
    {
      ExcelAddress addr = new ExcelAddress(this._address);
      this.SetBorderAroundStyle(Style, addr);
      int num1 = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.BorderTop, eStyleProperty.Color, (object) Color.ToArgb().ToString("X"), this._positionID, new ExcelAddress(addr._fromRow, addr._fromCol, addr._fromRow, addr._toCol).Address));
      int num2 = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.BorderBottom, eStyleProperty.Color, (object) Color.ToArgb().ToString("X"), this._positionID, new ExcelAddress(addr._toRow, addr._fromCol, addr._toRow, addr._toCol).Address));
      int num3 = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.BorderLeft, eStyleProperty.Color, (object) Color.ToArgb().ToString("X"), this._positionID, new ExcelAddress(addr._fromRow, addr._fromCol, addr._toRow, addr._fromCol).Address));
      int num4 = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.BorderRight, eStyleProperty.Color, (object) Color.ToArgb().ToString("X"), this._positionID, new ExcelAddress(addr._fromRow, addr._toCol, addr._toRow, addr._toCol).Address));
    }

    private void SetBorderAroundStyle(ExcelBorderStyle Style, ExcelAddress addr)
    {
      int num1 = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.BorderTop, eStyleProperty.Style, (object) Style, this._positionID, new ExcelAddress(addr._fromRow, addr._fromCol, addr._fromRow, addr._toCol).Address));
      int num2 = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.BorderBottom, eStyleProperty.Style, (object) Style, this._positionID, new ExcelAddress(addr._toRow, addr._fromCol, addr._toRow, addr._toCol).Address));
      int num3 = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.BorderLeft, eStyleProperty.Style, (object) Style, this._positionID, new ExcelAddress(addr._fromRow, addr._fromCol, addr._toRow, addr._fromCol).Address));
      int num4 = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.BorderRight, eStyleProperty.Style, (object) Style, this._positionID, new ExcelAddress(addr._fromRow, addr._toCol, addr._toRow, addr._toCol).Address));
    }
  }
}
