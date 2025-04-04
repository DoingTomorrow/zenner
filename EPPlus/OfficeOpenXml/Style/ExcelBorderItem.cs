// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.ExcelBorderItem
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Style.XmlAccess;
using System;

#nullable disable
namespace OfficeOpenXml.Style
{
  public sealed class ExcelBorderItem : StyleBase
  {
    private eStyleClass _cls;
    private StyleBase _parent;
    private ExcelColor _color;

    internal ExcelBorderItem(
      ExcelStyles styles,
      XmlHelper.ChangedEventHandler ChangedEvent,
      int worksheetID,
      string address,
      eStyleClass cls,
      StyleBase parent)
      : base(styles, ChangedEvent, worksheetID, address)
    {
      this._cls = cls;
      this._parent = parent;
    }

    public ExcelBorderStyle Style
    {
      get => this.GetSource().Style;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(this._cls, eStyleProperty.Style, (object) value, this._positionID, this._address));
      }
    }

    public ExcelColor Color
    {
      get
      {
        if (this._color == null)
          this._color = new ExcelColor(this._styles, this._ChangedEvent, this._positionID, this._address, this._cls, this._parent);
        return this._color;
      }
    }

    internal override string Id => this.Style.ToString() + this.Color.Id;

    internal override void SetIndex(int index) => this._parent.Index = index;

    private ExcelBorderItemXml GetSource()
    {
      int index = this._parent.Index < 0 ? 0 : this._parent.Index;
      switch (this._cls)
      {
        case eStyleClass.BorderTop:
          return this._styles.Borders[index].Top;
        case eStyleClass.BorderLeft:
          return this._styles.Borders[index].Left;
        case eStyleClass.BorderBottom:
          return this._styles.Borders[index].Bottom;
        case eStyleClass.BorderRight:
          return this._styles.Borders[index].Right;
        case eStyleClass.BorderDiagonal:
          return this._styles.Borders[index].Diagonal;
        default:
          throw new Exception("Invalid class for Borderitem");
      }
    }
  }
}
