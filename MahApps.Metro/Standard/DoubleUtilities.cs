// Decompiled with JetBrains decompiler
// Type: Standard.DoubleUtilities
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

#nullable disable
namespace Standard
{
  internal static class DoubleUtilities
  {
    private const double Epsilon = 1.53E-06;

    public static bool AreClose(double value1, double value2)
    {
      if (value1 == value2)
        return true;
      double num = value1 - value2;
      return num < 1.53E-06 && num > -1.53E-06;
    }

    public static bool IsCloseTo(this double value1, double value2)
    {
      return DoubleUtilities.AreClose(value1, value2);
    }

    public static bool IsStrictlyLessThan(this double value1, double value2)
    {
      return value1 < value2 && !DoubleUtilities.AreClose(value1, value2);
    }

    public static bool IsStrictlyGreaterThan(this double value1, double value2)
    {
      return value1 > value2 && !DoubleUtilities.AreClose(value1, value2);
    }

    public static bool IsLessThanOrCloseTo(this double value1, double value2)
    {
      return value1 < value2 || DoubleUtilities.AreClose(value1, value2);
    }

    public static bool IsGreaterThanOrCloseTo(this double value1, double value2)
    {
      return value1 > value2 || DoubleUtilities.AreClose(value1, value2);
    }

    public static bool IsFinite(this double value)
    {
      return !double.IsNaN(value) && !double.IsInfinity(value);
    }

    public static bool IsValidSize(this double value)
    {
      return value.IsFinite() && value.IsGreaterThanOrCloseTo(0.0);
    }

    public static bool IsFiniteAndNonNegative(this double d)
    {
      return !double.IsNaN(d) && !double.IsInfinity(d) && d >= 0.0;
    }
  }
}
