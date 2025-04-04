// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.ExcelTime
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Globalization;

#nullable disable
namespace OfficeOpenXml.DataValidation
{
  public class ExcelTime
  {
    public const int NumberOfDecimals = 15;
    private readonly Decimal SecondsPerDay = 86400M;
    private readonly Decimal SecondsPerHour = 3600M;
    private readonly Decimal SecondsPerMinute = 60M;
    private int _hour;
    private int _minute;
    private int? _second;

    private event EventHandler _timeChanged;

    public ExcelTime()
    {
    }

    public ExcelTime(Decimal value)
    {
      if (value < 0M)
        throw new ArgumentException("Value cannot be less than 0");
      if (value >= 1M)
        throw new ArgumentException("Value cannot be greater or equal to 1");
      this.Init(value);
    }

    private void Init(Decimal value)
    {
      Decimal num = value * this.SecondsPerDay;
      this.Hour = (int) Math.Floor(num / this.SecondsPerHour);
      this.Minute = (int) Math.Floor((num - (Decimal) this.Hour * this.SecondsPerHour) / this.SecondsPerMinute);
      this.SetSecond((int) Math.Round(num - (Decimal) this.Hour * this.SecondsPerHour - (Decimal) this.Minute * this.SecondsPerMinute, MidpointRounding.AwayFromZero));
    }

    private void SetSecond(int value)
    {
      if (value == 60)
      {
        this.Second = new int?(0);
        this.SetMinute(this.Minute + 1);
      }
      else
        this.Second = new int?(value);
    }

    private void SetMinute(int value)
    {
      if (value == 60)
      {
        this.Minute = 0;
        this.SetHour(this.Hour + 1);
      }
      else
        this.Minute = value;
    }

    private void SetHour(int value)
    {
      if (value != 24)
        return;
      this.Hour = 0;
    }

    internal event EventHandler TimeChanged
    {
      add => this._timeChanged += value;
      remove => this._timeChanged -= value;
    }

    private void OnTimeChanged()
    {
      if (this._timeChanged == null)
        return;
      this._timeChanged((object) this, EventArgs.Empty);
    }

    public int Hour
    {
      get => this._hour;
      set
      {
        if (value < 0)
          throw new InvalidOperationException("Value for hour cannot be negative");
        this._hour = value <= 23 ? value : throw new InvalidOperationException("Value for hour cannot be greater than 23");
        this.OnTimeChanged();
      }
    }

    public int Minute
    {
      get => this._minute;
      set
      {
        if (value < 0)
          throw new InvalidOperationException("Value for minute cannot be negative");
        this._minute = value <= 59 ? value : throw new InvalidOperationException("Value for minute cannot be greater than 59");
        this.OnTimeChanged();
      }
    }

    public int? Second
    {
      get => this._second;
      set
      {
        int? nullable1 = value;
        if ((nullable1.GetValueOrDefault() >= 0 ? 0 : (nullable1.HasValue ? 1 : 0)) != 0)
          throw new InvalidOperationException("Value for second cannot be negative");
        int? nullable2 = value;
        if ((nullable2.GetValueOrDefault() <= 59 ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
          throw new InvalidOperationException("Value for second cannot be greater than 59");
        this._second = value;
        this.OnTimeChanged();
      }
    }

    private Decimal Round(Decimal value) => Math.Round(value, 15);

    private Decimal ToSeconds()
    {
      return (Decimal) this.Hour * this.SecondsPerHour + (Decimal) this.Minute * this.SecondsPerMinute + (Decimal) (this.Second ?? 0);
    }

    public Decimal ToExcelTime() => this.Round(this.ToSeconds() / this.SecondsPerDay);

    public string ToExcelString()
    {
      return this.ToExcelTime().ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    public override string ToString()
    {
      int num = this.Second ?? 0;
      return string.Format("{0}:{1}:{2}", this.Hour < 10 ? (object) ("0" + this.Hour.ToString()) : (object) this.Hour.ToString(), this.Minute < 10 ? (object) ("0" + this.Minute.ToString()) : (object) this.Minute.ToString(), num < 10 ? (object) ("0" + num.ToString()) : (object) num.ToString());
    }
  }
}
