// Decompiled with JetBrains decompiler
// Type: AForge.Point
// Assembly: AForge, Version=2.2.5.0, Culture=neutral, PublicKeyToken=c1db6ff4eaa06aeb
// MVID: D4933F01-4742-407D-982E-D47DDB340621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.dll

using System;
using System.Globalization;

#nullable disable
namespace AForge
{
  [Serializable]
  public struct Point(float x, float y)
  {
    public float X = x;
    public float Y = y;

    public float DistanceTo(Point anotherPoint)
    {
      float num1 = this.X - anotherPoint.X;
      float num2 = this.Y - anotherPoint.Y;
      return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public float SquaredDistanceTo(Point anotherPoint)
    {
      float num1 = this.X - anotherPoint.X;
      float num2 = this.Y - anotherPoint.Y;
      return (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public static Point operator +(Point point1, Point point2)
    {
      return new Point(point1.X + point2.X, point1.Y + point2.Y);
    }

    public static Point Add(Point point1, Point point2)
    {
      return new Point(point1.X + point2.X, point1.Y + point2.Y);
    }

    public static Point operator -(Point point1, Point point2)
    {
      return new Point(point1.X - point2.X, point1.Y - point2.Y);
    }

    public static Point Subtract(Point point1, Point point2)
    {
      return new Point(point1.X - point2.X, point1.Y - point2.Y);
    }

    public static Point operator +(Point point, float valueToAdd)
    {
      return new Point(point.X + valueToAdd, point.Y + valueToAdd);
    }

    public static Point Add(Point point, float valueToAdd)
    {
      return new Point(point.X + valueToAdd, point.Y + valueToAdd);
    }

    public static Point operator -(Point point, float valueToSubtract)
    {
      return new Point(point.X - valueToSubtract, point.Y - valueToSubtract);
    }

    public static Point Subtract(Point point, float valueToSubtract)
    {
      return new Point(point.X - valueToSubtract, point.Y - valueToSubtract);
    }

    public static Point operator *(Point point, float factor)
    {
      return new Point(point.X * factor, point.Y * factor);
    }

    public static Point Multiply(Point point, float factor)
    {
      return new Point(point.X * factor, point.Y * factor);
    }

    public static Point operator /(Point point, float factor)
    {
      return new Point(point.X / factor, point.Y / factor);
    }

    public static Point Divide(Point point, float factor)
    {
      return new Point(point.X / factor, point.Y / factor);
    }

    public static bool operator ==(Point point1, Point point2)
    {
      return (double) point1.X == (double) point2.X && (double) point1.Y == (double) point2.Y;
    }

    public static bool operator !=(Point point1, Point point2)
    {
      return (double) point1.X != (double) point2.X || (double) point1.Y != (double) point2.Y;
    }

    public override bool Equals(object obj) => obj is Point point && this == point;

    public override int GetHashCode() => this.X.GetHashCode() + this.Y.GetHashCode();

    public static explicit operator IntPoint(Point point)
    {
      return new IntPoint((int) point.X, (int) point.Y);
    }

    public static implicit operator DoublePoint(Point point)
    {
      return new DoublePoint((double) point.X, (double) point.Y);
    }

    public IntPoint Round()
    {
      return new IntPoint((int) Math.Round((double) this.X), (int) Math.Round((double) this.Y));
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}, {1}", (object) this.X, (object) this.Y);
    }

    public float EuclideanNorm()
    {
      return (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y);
    }
  }
}
