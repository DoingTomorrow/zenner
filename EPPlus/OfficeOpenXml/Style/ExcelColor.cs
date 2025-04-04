// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.ExcelColor
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Style.XmlAccess;
using System;
using System.Drawing;

#nullable disable
namespace OfficeOpenXml.Style
{
  public sealed class ExcelColor : StyleBase
  {
    private eStyleClass _cls;
    private StyleBase _parent;

    internal ExcelColor(
      ExcelStyles styles,
      XmlHelper.ChangedEventHandler ChangedEvent,
      int worksheetID,
      string address,
      eStyleClass cls,
      StyleBase parent)
      : base(styles, ChangedEvent, worksheetID, address)
    {
      this._parent = parent;
      this._cls = cls;
    }

    public string Theme => this.GetSource().Theme;

    public Decimal Tint
    {
      get => this.GetSource().Tint;
      set
      {
        if (value > 1M || value < -1M)
          throw new ArgumentOutOfRangeException("Value must be between -1 and 1");
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(this._cls, eStyleProperty.Tint, (object) value, this._positionID, this._address));
      }
    }

    public string Rgb
    {
      get => this.GetSource().Rgb;
      internal set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(this._cls, eStyleProperty.Color, (object) value, this._positionID, this._address));
      }
    }

    public int Indexed
    {
      get => this.GetSource().Indexed;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(this._cls, eStyleProperty.IndexedColor, (object) value, this._positionID, this._address));
      }
    }

    public void SetColor(Color color) => this.Rgb = color.ToArgb().ToString("X");

    internal override string Id
    {
      get => this.Theme + (object) this.Tint + this.Rgb + (object) this.Indexed;
    }

    private ExcelColorXml GetSource()
    {
      this.Index = this._parent.Index < 0 ? 0 : this._parent.Index;
      switch (this._cls)
      {
        case eStyleClass.Font:
          return this._styles.Fonts[this.Index].Color;
        case eStyleClass.BorderTop:
          return this._styles.Borders[this.Index].Top.Color;
        case eStyleClass.BorderLeft:
          return this._styles.Borders[this.Index].Left.Color;
        case eStyleClass.BorderBottom:
          return this._styles.Borders[this.Index].Bottom.Color;
        case eStyleClass.BorderRight:
          return this._styles.Borders[this.Index].Right.Color;
        case eStyleClass.BorderDiagonal:
          return this._styles.Borders[this.Index].Diagonal.Color;
        case eStyleClass.FillBackgroundColor:
          return this._styles.Fills[this.Index].BackgroundColor;
        case eStyleClass.FillPatternColor:
          return this._styles.Fills[this.Index].PatternColor;
        default:
          throw new Exception("Invalid style-class for Color");
      }
    }

    internal override void SetIndex(int index) => this._parent.Index = index;

    public string LookupColor(ExcelColor theColor)
    {
      string[] strArray = new string[64]
      {
        "#FF000000",
        "#FFFFFFFF",
        "#FFFF0000",
        "#FF00FF00",
        "#FF0000FF",
        "#FFFFFF00",
        "#FFFF00FF",
        "#FF00FFFF",
        "#FF000000",
        "#FFFFFFFF",
        "#FFFF0000",
        "#FF00FF00",
        "#FF0000FF",
        "#FFFFFF00",
        "#FFFF00FF",
        "#FF00FFFF",
        "#FF800000",
        "#FF008000",
        "#FF000080",
        "#FF808000",
        "#FF800080",
        "#FF008080",
        "#FFC0C0C0",
        "#FF808080",
        "#FF9999FF",
        "#FF993366",
        "#FFFFFFCC",
        "#FFCCFFFF",
        "#FF660066",
        "#FFFF8080",
        "#FF0066CC",
        "#FFCCCCFF",
        "#FF000080",
        "#FFFF00FF",
        "#FFFFFF00",
        "#FF00FFFF",
        "#FF800080",
        "#FF800000",
        "#FF008080",
        "#FF0000FF",
        "#FF00CCFF",
        "#FFCCFFFF",
        "#FFCCFFCC",
        "#FFFFFF99",
        "#FF99CCFF",
        "#FFFF99CC",
        "#FFCC99FF",
        "#FFFFCC99",
        "#FF3366FF",
        "#FF33CCCC",
        "#FF99CC00",
        "#FFFFCC00",
        "#FFFF9900",
        "#FFFF6600",
        "#FF666699",
        "#FF969696",
        "#FF003366",
        "#FF339966",
        "#FF003300",
        "#FF333300",
        "#FF993300",
        "#FF993366",
        "#FF333399",
        "#FF333333"
      };
      string str1;
      if (0 <= theColor.Indexed && strArray.Length > theColor.Indexed)
        str1 = strArray[theColor.Indexed];
      else if (theColor.Rgb != null && 0 < theColor.Rgb.Length)
      {
        str1 = "#" + theColor.Rgb;
      }
      else
      {
        int num = (int) (theColor.Tint * 160M);
        string str2 = ((int) Decimal.Round(theColor.Tint * -512M)).ToString("X");
        str1 = "#FF" + str2 + str2 + str2;
      }
      return str1;
    }
  }
}
