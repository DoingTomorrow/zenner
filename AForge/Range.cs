// Decompiled with JetBrains decompiler
// Type: AForge.Range
// Assembly: AForge, Version=2.2.5.0, Culture=neutral, PublicKeyToken=c1db6ff4eaa06aeb
// MVID: D4933F01-4742-407D-982E-D47DDB340621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.dll

using System;
using System.Globalization;

#nullable disable
namespace AForge
{
  [Serializable]
  public struct Range(float min, float max)
  {
    private float min = min;
    private float max = max;

    public float Min
    {
      get => this.min;
      set => this.min = value;
    }

    public float Max
    {
      get => this.max;
      set => this.max = value;
    }

    public float Length => this.max - this.min;

    public bool IsInside(float x)
    {
      return (double) x >= (double) this.min && (double) x <= (double) this.max;
    }

    public bool IsInside(Range range) => this.IsInside(range.min) && this.IsInside(range.max);

    public bool IsOverlapping(Range range)
    {
      return this.IsInside(range.min) || this.IsInside(range.max) || range.IsInside(this.min) || range.IsInside(this.max);
    }

    public IntRange ToIntRange(bool provideInnerRange)
    {
      int min;
      int max;
      if (provideInnerRange)
      {
        min = (int) Math.Ceiling((double) this.min);
        max = (int) Math.Floor((double) this.max);
      }
      else
      {
        min = (int) Math.Floor((double) this.min);
        max = (int) Math.Ceiling((double) this.max);
      }
      return new IntRange(min, max);
    }

    public static bool operator ==(Range range1, Range range2)
    {
      return (double) range1.min == (double) range2.min && (double) range1.max == (double) range2.max;
    }

    public static bool operator !=(Range range1, Range range2)
    {
      return (double) range1.min != (double) range2.min || (double) range1.max != (double) range2.max;
    }

    public override bool Equals(object obj) => obj is Range range && this == range;

    public override int GetHashCode() => this.min.GetHashCode() + this.max.GetHashCode();

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}, {1}", (object) this.min, (object) this.max);
    }
  }
}
