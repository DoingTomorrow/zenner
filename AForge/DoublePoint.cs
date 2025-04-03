// Decompiled with JetBrains decompiler
// Type: AForge.DoublePoint
// Assembly: AForge, Version=2.2.5.0, Culture=neutral, PublicKeyToken=c1db6ff4eaa06aeb
// MVID: D4933F01-4742-407D-982E-D47DDB340621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.dll

using System;
using System.Globalization;

#nullable disable
namespace AForge
{
  [Serializable]
  public struct DoublePoint(double x, double y)
  {
    public double X = x;
    public double Y = y;

    public double DistanceTo(DoublePoint anotherPoint)
    {
      double num1 = this.X - anotherPoint.X;
      double num2 = this.Y - anotherPoint.Y;
      return Math.Sqrt(num1 * num1 + num2 * num2);
    }

    public double SquaredDistanceTo(DoublePoint anotherPoint)
    {
      double num1 = this.X - anotherPoint.X;
      double num2 = this.Y - anotherPoint.Y;
      return num1 * num1 + num2 * num2;
    }

    public static DoublePoint operator +(DoublePoint point1, DoublePoint point2)
    {
      return new DoublePoint(point1.X + point2.X, point1.Y + point2.Y);
    }

    public static DoublePoint Add(DoublePoint point1, DoublePoint point2)
    {
      return new DoublePoint(point1.X + point2.X, point1.Y + point2.Y);
    }

    public static DoublePoint operator -(DoublePoint point1, DoublePoint point2)
    {
      return new DoublePoint(point1.X - point2.X, point1.Y - point2.Y);
    }

    public static DoublePoint Subtract(DoublePoint point1, DoublePoint point2)
    {
      return new DoublePoint(point1.X - point2.X, point1.Y - point2.Y);
    }

    public static DoublePoint operator +(DoublePoint point, double valueToAdd)
    {
      return new DoublePoint(point.X + valueToAdd, point.Y + valueToAdd);
    }

    public static DoublePoint Add(DoublePoint point, double valueToAdd)
    {
      return new DoublePoint(point.X + valueToAdd, point.Y + valueToAdd);
    }

    public static DoublePoint operator -(DoublePoint point, double valueToSubtract)
    {
      return new DoublePoint(point.X - valueToSubtract, point.Y - valueToSubtract);
    }

    public static DoublePoint Subtract(DoublePoint point, double valueToSubtract)
    {
      return new DoublePoint(point.X - valueToSubtract, point.Y - valueToSubtract);
    }

    public static DoublePoint operator *(DoublePoint point, double factor)
    {
      return new DoublePoint(point.X * factor, point.Y * factor);
    }

    public static DoublePoint Multiply(DoublePoint point, double factor)
    {
      return new DoublePoint(point.X * factor, point.Y * factor);
    }

    public static DoublePoint operator /(DoublePoint point, double factor)
    {
      return new DoublePoint(point.X / factor, point.Y / factor);
    }

    public static DoublePoint Divide(DoublePoint point, double factor)
    {
      return new DoublePoint(point.X / factor, point.Y / factor);
    }

    public static bool operator ==(DoublePoint point1, DoublePoint point2)
    {
      return point1.X == point2.X && point1.Y == point2.Y;
    }

    public static bool operator !=(DoublePoint point1, DoublePoint point2)
    {
      return point1.X != point2.X || point1.Y != point2.Y;
    }

    public override bool Equals(object obj)
    {
      return obj is DoublePoint doublePoint && this == doublePoint;
    }

    public override int GetHashCode() => this.X.GetHashCode() + this.Y.GetHashCode();

    public static explicit operator IntPoint(DoublePoint point)
    {
      return new IntPoint((int) point.X, (int) point.Y);
    }

    public static explicit operator Point(DoublePoint point)
    {
      return new Point((float) point.X, (float) point.Y);
    }

    public IntPoint Round() => new IntPoint((int) Math.Round(this.X), (int) Math.Round(this.Y));

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}, {1}", (object) this.X, (object) this.Y);
    }

    public double EuclideanNorm() => Math.Sqrt(this.X * this.X + this.Y * this.Y);
  }
}
