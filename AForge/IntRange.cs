// Decompiled with JetBrains decompiler
// Type: AForge.IntRange
// Assembly: AForge, Version=2.2.5.0, Culture=neutral, PublicKeyToken=c1db6ff4eaa06aeb
// MVID: D4933F01-4742-407D-982E-D47DDB340621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.dll

using System;
using System.Globalization;

#nullable disable
namespace AForge
{
  [Serializable]
  public struct IntRange(int min, int max)
  {
    private int min = min;
    private int max = max;

    public int Min
    {
      get => this.min;
      set => this.min = value;
    }

    public int Max
    {
      get => this.max;
      set => this.max = value;
    }

    public int Length => this.max - this.min;

    public bool IsInside(int x) => x >= this.min && x <= this.max;

    public bool IsInside(IntRange range) => this.IsInside(range.min) && this.IsInside(range.max);

    public bool IsOverlapping(IntRange range)
    {
      return this.IsInside(range.min) || this.IsInside(range.max) || range.IsInside(this.min) || range.IsInside(this.max);
    }

    public static implicit operator Range(IntRange range)
    {
      return new Range((float) range.Min, (float) range.Max);
    }

    public static bool operator ==(IntRange range1, IntRange range2)
    {
      return range1.min == range2.min && range1.max == range2.max;
    }

    public static bool operator !=(IntRange range1, IntRange range2)
    {
      return range1.min != range2.min || range1.max != range2.max;
    }

    public override bool Equals(object obj) => obj is IntRange intRange && this == intRange;

    public override int GetHashCode() => this.min.GetHashCode() + this.max.GetHashCode();

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}, {1}", (object) this.min, (object) this.max);
    }
  }
}
