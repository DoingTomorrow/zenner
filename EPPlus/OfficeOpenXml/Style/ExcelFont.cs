// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.ExcelFont
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Drawing;

#nullable disable
namespace OfficeOpenXml.Style
{
  public sealed class ExcelFont : StyleBase
  {
    internal ExcelFont(
      ExcelStyles styles,
      XmlHelper.ChangedEventHandler ChangedEvent,
      int PositionID,
      string address,
      int index)
      : base(styles, ChangedEvent, PositionID, address)
    {
      this.Index = index;
    }

    public string Name
    {
      get => this._styles.Fonts[this.Index].Name;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Font, eStyleProperty.Name, (object) value, this._positionID, this._address));
      }
    }

    public float Size
    {
      get => this._styles.Fonts[this.Index].Size;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Font, eStyleProperty.Size, (object) value, this._positionID, this._address));
      }
    }

    public int Family
    {
      get => this._styles.Fonts[this.Index].Family;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Font, eStyleProperty.Family, (object) value, this._positionID, this._address));
      }
    }

    public ExcelColor Color
    {
      get
      {
        return new ExcelColor(this._styles, this._ChangedEvent, this._positionID, this._address, eStyleClass.Font, (StyleBase) this);
      }
    }

    public string Scheme
    {
      get => this._styles.Fonts[this.Index].Scheme;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Font, eStyleProperty.Scheme, (object) value, this._positionID, this._address));
      }
    }

    public bool Bold
    {
      get => this._styles.Fonts[this.Index].Bold;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Font, eStyleProperty.Bold, (object) value, this._positionID, this._address));
      }
    }

    public bool Italic
    {
      get => this._styles.Fonts[this.Index].Italic;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Font, eStyleProperty.Italic, (object) value, this._positionID, this._address));
      }
    }

    public bool Strike
    {
      get => this._styles.Fonts[this.Index].Strike;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Font, eStyleProperty.Strike, (object) value, this._positionID, this._address));
      }
    }

    public bool UnderLine
    {
      get => this._styles.Fonts[this.Index].UnderLine;
      set
      {
        if (value)
          this.UnderLineType = ExcelUnderLineType.Single;
        else
          this.UnderLineType = ExcelUnderLineType.None;
      }
    }

    public ExcelUnderLineType UnderLineType
    {
      get => this._styles.Fonts[this.Index].UnderLineType;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Font, eStyleProperty.UnderlineType, (object) value, this._positionID, this._address));
      }
    }

    public ExcelVerticalAlignmentFont VerticalAlign
    {
      get
      {
        return this._styles.Fonts[this.Index].VerticalAlign == "" ? ExcelVerticalAlignmentFont.None : (ExcelVerticalAlignmentFont) Enum.Parse(typeof (ExcelVerticalAlignmentFont), this._styles.Fonts[this.Index].VerticalAlign, true);
      }
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Font, eStyleProperty.VerticalAlign, (object) value, this._positionID, this._address));
      }
    }

    public void SetFromFont(Font Font)
    {
      this.Name = Font.Name;
      this.Size = (float) (int) Font.Size;
      this.Strike = Font.Strikeout;
      this.Bold = Font.Bold;
      this.UnderLine = Font.Underline;
      this.Italic = Font.Italic;
    }

    internal override string Id
    {
      get
      {
        return this.Name + this.Size.ToString() + this.Family.ToString() + this.Scheme.ToString() + (object) this.Bold.ToString()[0] + (object) this.Italic.ToString()[0] + (object) this.Strike.ToString()[0] + (object) this.UnderLine.ToString()[0] + (object) this.VerticalAlign;
      }
    }
  }
}
