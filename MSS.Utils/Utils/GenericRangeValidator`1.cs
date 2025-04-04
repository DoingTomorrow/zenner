// Decompiled with JetBrains decompiler
// Type: MSS.Utils.Utils.GenericRangeValidator`1
// Assembly: MSS.Utils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8365EDE-890D-4A42-AEA4-3B8FCE5E7B93
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Utils.dll

using System;

#nullable disable
namespace MSS.Utils.Utils
{
  public class GenericRangeValidator<T> where T : struct, IComparable<T>
  {
    public GenericRangeValidator(T min, T max, RangeTypeEnum rangeType = RangeTypeEnum.Inclusive)
    {
      this.Min = min;
      this.Max = max;
      this.RangeType = rangeType;
    }

    public T Min { get; }

    public T Max { get; }

    public RangeTypeEnum RangeType { get; }

    private bool IsInclusive(T value)
    {
      return value.IsGreaterThanOrEqualTo<T>(this.Min) && value.IsLessThanOrEqualTo<T>(this.Max);
    }

    private bool IsInclusiveMin(T value)
    {
      return value.IsGreaterThanOrEqualTo<T>(this.Min) && value.IsLessThan<T>(this.Max);
    }

    private bool IsInclusiveMax(T value)
    {
      return value.IsGreaterThan<T>(this.Min) && value.IsLessThanOrEqualTo<T>(this.Max);
    }

    private bool IsExclusive(T value)
    {
      return value.IsGreaterThan<T>(this.Min) && value.IsLessThan<T>(this.Max);
    }

    public bool Contains(T value)
    {
      switch (this.RangeType)
      {
        case RangeTypeEnum.Inclusive:
          return this.IsInclusive(value);
        case RangeTypeEnum.Exclusive:
          return this.IsExclusive(value);
        case RangeTypeEnum.InclusiveMin:
          return this.IsInclusiveMin(value);
        case RangeTypeEnum.InclusiveMax:
          return this.IsInclusiveMax(value);
        default:
          throw new NotImplementedException();
      }
    }

    public override string ToString()
    {
      return string.Format("Min: {0}, Max: {1}, Type: {2}", (object) this.Min, (object) this.Max, (object) this.RangeType);
    }
  }
}
