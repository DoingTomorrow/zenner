// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.ExcelGradientFill
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Style.XmlAccess;
using System;

#nullable disable
namespace OfficeOpenXml.Style
{
  public class ExcelGradientFill : StyleBase
  {
    private ExcelColor _gradientColor1;
    private ExcelColor _gradientColor2;

    internal ExcelGradientFill(
      ExcelStyles styles,
      XmlHelper.ChangedEventHandler ChangedEvent,
      int PositionID,
      string address,
      int index)
      : base(styles, ChangedEvent, PositionID, address)
    {
      this.Index = index;
    }

    public double Degree
    {
      get => ((ExcelGradientFillXml) this._styles.Fills[this.Index]).Degree;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.GradientFill, eStyleProperty.GradientDegree, (object) value, this._positionID, this._address));
      }
    }

    public ExcelFillGradientType Type
    {
      get => ((ExcelGradientFillXml) this._styles.Fills[this.Index]).Type;
      set
      {
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.GradientFill, eStyleProperty.GradientType, (object) value, this._positionID, this._address));
      }
    }

    public double Top
    {
      get => ((ExcelGradientFillXml) this._styles.Fills[this.Index]).Top;
      set
      {
        if (value < 0.0 | value > 1.0)
          throw new ArgumentOutOfRangeException("Value must be between 0 and 1");
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.GradientFill, eStyleProperty.GradientTop, (object) value, this._positionID, this._address));
      }
    }

    public double Bottom
    {
      get => ((ExcelGradientFillXml) this._styles.Fills[this.Index]).Bottom;
      set
      {
        if (value < 0.0 | value > 1.0)
          throw new ArgumentOutOfRangeException("Value must be between 0 and 1");
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.GradientFill, eStyleProperty.GradientBottom, (object) value, this._positionID, this._address));
      }
    }

    public double Left
    {
      get => ((ExcelGradientFillXml) this._styles.Fills[this.Index]).Left;
      set
      {
        if (value < 0.0 | value > 1.0)
          throw new ArgumentOutOfRangeException("Value must be between 0 and 1");
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.GradientFill, eStyleProperty.GradientLeft, (object) value, this._positionID, this._address));
      }
    }

    public double Right
    {
      get => ((ExcelGradientFillXml) this._styles.Fills[this.Index]).Right;
      set
      {
        if (value < 0.0 | value > 1.0)
          throw new ArgumentOutOfRangeException("Value must be between 0 and 1");
        int num = this._ChangedEvent((StyleBase) this, new StyleChangeEventArgs(eStyleClass.GradientFill, eStyleProperty.GradientRight, (object) value, this._positionID, this._address));
      }
    }

    public ExcelColor Color1
    {
      get
      {
        if (this._gradientColor1 == null)
          this._gradientColor1 = new ExcelColor(this._styles, this._ChangedEvent, this._positionID, this._address, eStyleClass.FillGradientColor1, (StyleBase) this);
        return this._gradientColor1;
      }
    }

    public ExcelColor Color2
    {
      get
      {
        if (this._gradientColor2 == null)
          this._gradientColor2 = new ExcelColor(this._styles, this._ChangedEvent, this._positionID, this._address, eStyleClass.FillGradientColor2, (StyleBase) this);
        return this._gradientColor2;
      }
    }

    internal override string Id
    {
      get
      {
        return this.Degree.ToString() + (object) this.Type + this.Color1.Id + this.Color2.Id + this.Top.ToString() + this.Bottom.ToString() + this.Left.ToString() + this.Right.ToString();
      }
    }
  }
}
