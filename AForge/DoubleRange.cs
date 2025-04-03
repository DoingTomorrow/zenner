// Decompiled with JetBrains decompiler
// Type: AForge.DoubleRange
// Assembly: AForge, Version=2.2.5.0, Culture=neutral, PublicKeyToken=c1db6ff4eaa06aeb
// MVID: D4933F01-4742-407D-982E-D47DDB340621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.dll

using System;
using System.Globalization;

#nullable disable
namespace AForge
{
  [Serializable]
  public struct DoubleRange(double min, double max)
  {
    private double min = min;
    private double max = max;

    public double Min
    {
      get => this.min;
      set => this.min = value;
    }

    public double Max
    {
      get => this.max;
      set => this.max = value;
    }

    public double Length => this.max - this.min;

    public bool IsInside(double x) => x >= this.min && x <= this.max;

    public bool IsInside(DoubleRange range) => this.IsInside(range.min) && this.IsInside(range.max);

    public bool IsOverlapping(DoubleRange range)
    {
      return this.IsInside(range.min) || this.IsInside(range.max) || range.IsInside(this.min) || range.IsInside(this.max);
    }

    public IntRange ToIntRange(bool provideInnerRange)
    {
      int min;
      int max;
      if (provideInnerRange)
      {
        min = (int) Math.Ceiling(this.min);
        max = (int) Math.Floor(this.max);
      }
      else
      {
        min = (int) Math.Floor(this.min);
        max = (int) Math.Ceiling(this.max);
      }
      return new IntRange(min, max);
    }

    public static bool operator ==(DoubleRange range1, DoubleRange range2)
    {
      return range1.min == range2.min && range1.max == range2.max;
    }

    public static bool operator !=(DoubleRange range1, DoubleRange range2)
    {
      return range1.min != range2.min || range1.max != range2.max;
    }

    public override bool Equals(object obj) => obj is Range && this == (DoubleRange) obj;

    public override int GetHashCode() => this.min.GetHashCode() + this.max.GetHashCode();

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}, {1}", (object) this.min, (object) this.max);
    }
  }
}
