// Decompiled with JetBrains decompiler
// Type: AForge.IntPoint
// Assembly: AForge, Version=2.2.5.0, Culture=neutral, PublicKeyToken=c1db6ff4eaa06aeb
// MVID: D4933F01-4742-407D-982E-D47DDB340621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.dll

using System;
using System.Globalization;

#nullable disable
namespace AForge
{
  [Serializable]
  public struct IntPoint(int x, int y)
  {
    public int X = x;
    public int Y = y;

    public float DistanceTo(IntPoint anotherPoint)
    {
      int num1 = this.X - anotherPoint.X;
      int num2 = this.Y - anotherPoint.Y;
      return (float) Math.Sqrt((double) (num1 * num1 + num2 * num2));
    }

    public float SquaredDistanceTo(Point anotherPoint)
    {
      float num1 = (float) this.X - anotherPoint.X;
      float num2 = (float) this.Y - anotherPoint.Y;
      return (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public static IntPoint operator +(IntPoint point1, IntPoint point2)
    {
      return new IntPoint(point1.X + point2.X, point1.Y + point2.Y);
    }

    public static IntPoint Add(IntPoint point1, IntPoint point2)
    {
      return new IntPoint(point1.X + point2.X, point1.Y + point2.Y);
    }

    public static IntPoint operator -(IntPoint point1, IntPoint point2)
    {
      return new IntPoint(point1.X - point2.X, point1.Y - point2.Y);
    }

    public static IntPoint Subtract(IntPoint point1, IntPoint point2)
    {
      return new IntPoint(point1.X - point2.X, point1.Y - point2.Y);
    }

    public static IntPoint operator +(IntPoint point, int valueToAdd)
    {
      return new IntPoint(point.X + valueToAdd, point.Y + valueToAdd);
    }

    public static IntPoint Add(IntPoint point, int valueToAdd)
    {
      return new IntPoint(point.X + valueToAdd, point.Y + valueToAdd);
    }

    public static IntPoint operator -(IntPoint point, int valueToSubtract)
    {
      return new IntPoint(point.X - valueToSubtract, point.Y - valueToSubtract);
    }

    public static IntPoint Subtract(IntPoint point, int valueToSubtract)
    {
      return new IntPoint(point.X - valueToSubtract, point.Y - valueToSubtract);
    }

    public static IntPoint operator *(IntPoint point, int factor)
    {
      return new IntPoint(point.X * factor, point.Y * factor);
    }

    public static IntPoint Multiply(IntPoint point, int factor)
    {
      return new IntPoint(point.X * factor, point.Y * factor);
    }

    public static IntPoint operator /(IntPoint point, int factor)
    {
      return new IntPoint(point.X / factor, point.Y / factor);
    }

    public static IntPoint Divide(IntPoint point, int factor)
    {
      return new IntPoint(point.X / factor, point.Y / factor);
    }

    public static bool operator ==(IntPoint point1, IntPoint point2)
    {
      return point1.X == point2.X && point1.Y == point2.Y;
    }

    public static bool operator !=(IntPoint point1, IntPoint point2)
    {
      return point1.X != point2.X || point1.Y != point2.Y;
    }

    public override bool Equals(object obj) => obj is IntPoint intPoint && this == intPoint;

    public override int GetHashCode() => this.X.GetHashCode() + this.Y.GetHashCode();

    public static implicit operator Point(IntPoint point)
    {
      return new Point((float) point.X, (float) point.Y);
    }

    public static implicit operator DoublePoint(IntPoint point)
    {
      return new DoublePoint((double) point.X, (double) point.Y);
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}, {1}", (object) this.X, (object) this.Y);
    }

    public float EuclideanNorm() => (float) Math.Sqrt((double) (this.X * this.X + this.Y * this.Y));
  }
}
