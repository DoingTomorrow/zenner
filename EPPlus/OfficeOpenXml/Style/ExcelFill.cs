// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.ExcelFill
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.Style
{
  public class ExcelFill : StyleBase
  {
    private ExcelColor _patternColor;
    private ExcelColor _backgroundColor;
    private ExcelGradientFill _gradient;

    internal ExcelFill(
      ExcelStyles styles,
      XmlHelper.ChangedEventHandler ChangedEvent,
      int PositionID,
      string address,
      int index)
      : base(styles, ChangedEvent, PositionID, address)
    {
      this.Index = index;
    }

    public ExcelFillStyle PatternType
    {
      get
      {
        return this.Index == int.MinValue ? ExcelFillStyle.None : this._styles.Fills[this.Index].PatternType;
      }
      set
      {
        if (this._gradient != null)
          this._gradient = (ExcelGradientFill) null;
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.Fill, eStyleProperty.PatternType, (object) value, this._positionID, this._address));
      }
    }

    public ExcelColor PatternColor
    {
      get
      {
        if (this._patternColor == null)
        {
          this._patternColor = new ExcelColor(this._styles, this._ChangedEvent, this._positionID, this._address, eStyleClass.FillPatternColor, (StyleBase) this);
          if (this._gradient != null)
            this._gradient = (ExcelGradientFill) null;
        }
        return this._patternColor;
      }
    }

    public ExcelColor BackgroundColor
    {
      get
      {
        if (this._backgroundColor == null)
        {
          this._backgroundColor = new ExcelColor(this._styles, this._ChangedEvent, this._positionID, this._address, eStyleClass.FillBackgroundColor, (StyleBase) this);
          if (this._gradient != null)
            this._gradient = (ExcelGradientFill) null;
        }
        return this._backgroundColor;
      }
    }

    public ExcelGradientFill Gradient
    {
      get
      {
        if (this._gradient == null)
        {
          this._gradient = new ExcelGradientFill(this._styles, this._ChangedEvent, this._positionID, this._address, this.Index);
          this._backgroundColor = (ExcelColor) null;
          this._patternColor = (ExcelColor) null;
        }
        return this._gradient;
      }
    }

    internal override string Id
    {
      get
      {
        return this._gradient == null ? this.PatternType.ToString() + this.PatternColor.Id + this.BackgroundColor.Id : this._gradient.Id;
      }
    }
  }
}
