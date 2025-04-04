// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.PossibleValueSet
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public class PossibleValueSet
  {
    private List<object> _DistinctValues;

    private Type ExpressionType { get; set; }

    private bool ContainsNull { get; set; }

    private bool ContainsAllNonNullValues { get; set; }

    private List<object> DistinctValues
    {
      get
      {
        if (this._DistinctValues == null)
          this._DistinctValues = new List<object>();
        return this._DistinctValues;
      }
    }

    private bool ContainsAnyNonNullValues
    {
      get => this.ContainsAllNonNullValues || this.DistinctValues.Any<object>();
    }

    private PossibleValueSet(Type expressionType) => this.ExpressionType = expressionType;

    public bool Contains(object obj)
    {
      if (obj == null)
        return this.ContainsNull;
      if (obj.GetType() != this.ExpressionType)
        return false;
      return this.ContainsAllNonNullValues || this.DistinctValues.Contains(obj);
    }

    public PossibleValueSet Add(PossibleValueSet pvs, Type resultType)
    {
      return this.MathOperation(pvs, resultType);
    }

    public PossibleValueSet Divide(PossibleValueSet pvs, Type resultType)
    {
      return this.MathOperation(pvs, resultType);
    }

    public PossibleValueSet Modulo(PossibleValueSet pvs, Type resultType)
    {
      return this.MathOperation(pvs, resultType);
    }

    public PossibleValueSet Multiply(PossibleValueSet pvs, Type resultType)
    {
      return this.MathOperation(pvs, resultType);
    }

    public PossibleValueSet Power(PossibleValueSet pvs, Type resultType)
    {
      return this.MathOperation(pvs, resultType);
    }

    public PossibleValueSet Subtract(PossibleValueSet pvs, Type resultType)
    {
      return this.MathOperation(pvs, resultType);
    }

    public PossibleValueSet And(PossibleValueSet pvs, Type resultType)
    {
      return this.MathOperation(pvs, resultType);
    }

    public PossibleValueSet Or(PossibleValueSet pvs, Type resultType)
    {
      return this.MathOperation(pvs, resultType);
    }

    public PossibleValueSet ExclusiveOr(PossibleValueSet pvs, Type resultType)
    {
      return this.MathOperation(pvs, resultType);
    }

    public PossibleValueSet LeftShift(PossibleValueSet pvs, Type resultType)
    {
      return this.MathOperation(pvs, resultType);
    }

    public PossibleValueSet RightShift(PossibleValueSet pvs, Type resultType)
    {
      return this.MathOperation(pvs, resultType);
    }

    private PossibleValueSet MathOperation(PossibleValueSet pvs, Type resultType)
    {
      if (!this.ContainsAnyNonNullValues || !pvs.ContainsAnyNonNullValues)
        return PossibleValueSet.CreateNull(resultType);
      return !this.ContainsNull && !pvs.ContainsNull ? PossibleValueSet.CreateAllNonNullValues(resultType) : PossibleValueSet.CreateAllValues(resultType);
    }

    public PossibleValueSet AndAlso(PossibleValueSet pvs)
    {
      PossibleValueSet possibleValueSet = new PossibleValueSet(this.DetermineBoolType(pvs));
      if (this.Contains((object) true) && pvs.Contains((object) true) && !possibleValueSet.Contains((object) true))
        possibleValueSet.DistinctValues.Add((object) true);
      if (this.Contains((object) true) && pvs.Contains((object) false) && !possibleValueSet.Contains((object) false))
        possibleValueSet.DistinctValues.Add((object) false);
      if (this.Contains((object) true) && pvs.Contains((object) null))
        possibleValueSet.ContainsNull = true;
      if (this.Contains((object) false) && pvs.Contains((object) true) && !possibleValueSet.Contains((object) false))
        possibleValueSet.DistinctValues.Add((object) false);
      if (this.Contains((object) false) && pvs.Contains((object) false) && !possibleValueSet.Contains((object) false))
        possibleValueSet.DistinctValues.Add((object) false);
      if (this.Contains((object) false) && pvs.Contains((object) null) && !possibleValueSet.Contains((object) false))
        possibleValueSet.DistinctValues.Add((object) false);
      if (this.Contains((object) null) && pvs.Contains((object) true))
        possibleValueSet.ContainsNull = true;
      if (this.Contains((object) null) && pvs.Contains((object) false) && !possibleValueSet.Contains((object) false))
        possibleValueSet.DistinctValues.Add((object) false);
      if (this.Contains((object) null) && pvs.Contains((object) null))
        possibleValueSet.ContainsNull = true;
      return possibleValueSet;
    }

    public PossibleValueSet OrElse(PossibleValueSet pvs)
    {
      PossibleValueSet possibleValueSet = new PossibleValueSet(this.DetermineBoolType(pvs));
      if (this.Contains((object) true) && pvs.Contains((object) true) && !possibleValueSet.Contains((object) true))
        possibleValueSet.DistinctValues.Add((object) true);
      if (this.Contains((object) true) && pvs.Contains((object) false) && !possibleValueSet.Contains((object) true))
        possibleValueSet.DistinctValues.Add((object) true);
      if (this.Contains((object) true) && pvs.Contains((object) null) && !possibleValueSet.Contains((object) true))
        possibleValueSet.DistinctValues.Add((object) true);
      if (this.Contains((object) false) && pvs.Contains((object) true) && !possibleValueSet.Contains((object) true))
        possibleValueSet.DistinctValues.Add((object) true);
      if (this.Contains((object) false) && pvs.Contains((object) false) && !possibleValueSet.Contains((object) false))
        possibleValueSet.DistinctValues.Add((object) false);
      if (this.Contains((object) false) && pvs.Contains((object) null))
        possibleValueSet.ContainsNull = true;
      if (this.Contains((object) null) && pvs.Contains((object) true) && !possibleValueSet.Contains((object) true))
        possibleValueSet.DistinctValues.Add((object) true);
      if (this.Contains((object) null) && pvs.Contains((object) false))
        possibleValueSet.ContainsNull = true;
      if (this.Contains((object) null) && pvs.Contains((object) null))
        possibleValueSet.ContainsNull = true;
      return possibleValueSet;
    }

    public PossibleValueSet Equal(PossibleValueSet pvs) => this.ComparisonOperation(pvs);

    public PossibleValueSet NotEqual(PossibleValueSet pvs) => this.ComparisonOperation(pvs);

    public PossibleValueSet GreaterThanOrEqual(PossibleValueSet pvs)
    {
      return this.ComparisonOperation(pvs);
    }

    public PossibleValueSet GreaterThan(PossibleValueSet pvs) => this.ComparisonOperation(pvs);

    public PossibleValueSet LessThan(PossibleValueSet pvs) => this.ComparisonOperation(pvs);

    public PossibleValueSet LessThanOrEqual(PossibleValueSet pvs) => this.ComparisonOperation(pvs);

    private PossibleValueSet ComparisonOperation(PossibleValueSet pvs)
    {
      return this.MathOperation(pvs, typeof (bool));
    }

    public PossibleValueSet Coalesce(PossibleValueSet pvs)
    {
      if (!this.ContainsNull)
        return this;
      PossibleValueSet possibleValueSet = new PossibleValueSet(this.ExpressionType);
      possibleValueSet.DistinctValues.AddRange((IEnumerable<object>) this.DistinctValues);
      possibleValueSet.ContainsAllNonNullValues = this.ContainsAllNonNullValues;
      possibleValueSet.ContainsNull = pvs.ContainsNull;
      possibleValueSet.ContainsAllNonNullValues |= pvs.ContainsAllNonNullValues;
      possibleValueSet.DistinctValues.AddRange(pvs.DistinctValues.Except<object>((IEnumerable<object>) possibleValueSet.DistinctValues));
      return possibleValueSet;
    }

    public PossibleValueSet Not()
    {
      this.DetermineBoolType();
      PossibleValueSet possibleValueSet = new PossibleValueSet(this.ExpressionType);
      possibleValueSet.ContainsNull = this.ContainsNull;
      possibleValueSet.DistinctValues.AddRange(this.DistinctValues.Cast<bool>().Select<bool, bool>((Func<bool, bool>) (v => !v)).Cast<object>());
      return possibleValueSet;
    }

    public PossibleValueSet BitwiseNot(Type resultType) => this.UnaryMathOperation(resultType);

    public PossibleValueSet ArrayLength(Type resultType)
    {
      return PossibleValueSet.CreateAllNonNullValues(typeof (int));
    }

    public PossibleValueSet Convert(Type resultType) => this.UnaryMathOperation(resultType);

    public PossibleValueSet Negate(Type resultType) => this.UnaryMathOperation(resultType);

    public PossibleValueSet UnaryPlus(Type resultType) => this;

    private PossibleValueSet UnaryMathOperation(Type resultType)
    {
      if (!this.ContainsAnyNonNullValues)
        return PossibleValueSet.CreateNull(resultType);
      return this.ContainsNull ? PossibleValueSet.CreateAllValues(resultType) : PossibleValueSet.CreateAllNonNullValues(resultType);
    }

    public PossibleValueSet IsNull()
    {
      PossibleValueSet possibleValueSet = new PossibleValueSet(typeof (bool));
      if (this.ContainsNull)
        possibleValueSet.DistinctValues.Add((object) true);
      if (this.ContainsAllNonNullValues || this.DistinctValues.Any<object>())
        possibleValueSet.DistinctValues.Add((object) false);
      return possibleValueSet;
    }

    public PossibleValueSet IsNotNull()
    {
      PossibleValueSet possibleValueSet = new PossibleValueSet(typeof (bool));
      if (this.ContainsNull)
        possibleValueSet.DistinctValues.Add((object) false);
      if (this.ContainsAllNonNullValues || this.DistinctValues.Any<object>())
        possibleValueSet.DistinctValues.Add((object) true);
      return possibleValueSet;
    }

    public PossibleValueSet MemberAccess(Type resultType)
    {
      return !this.ContainsAnyNonNullValues ? PossibleValueSet.CreateNull(resultType) : PossibleValueSet.CreateAllValues(resultType);
    }

    private Type DetermineBoolType(PossibleValueSet otherSet)
    {
      this.DetermineBoolType();
      otherSet.DetermineBoolType();
      Type type = typeof (bool?);
      return this.ExpressionType == type || otherSet.ExpressionType == type ? type : typeof (bool);
    }

    private void DetermineBoolType()
    {
      Type type1 = typeof (bool);
      Type type2 = typeof (bool?);
      if (this.ExpressionType != type1 && this.ExpressionType != type2)
        throw new AssertionFailure("Cannot perform desired possible value set operation on expressions of type: " + (object) this.ExpressionType);
    }

    public static PossibleValueSet CreateNull(Type expressionType)
    {
      return new PossibleValueSet(expressionType)
      {
        ContainsNull = true
      };
    }

    public static PossibleValueSet CreateAllNonNullValues(Type expressionType)
    {
      PossibleValueSet allNonNullValues = new PossibleValueSet(expressionType);
      if (expressionType == typeof (bool))
      {
        allNonNullValues.DistinctValues.Add((object) true);
        allNonNullValues.DistinctValues.Add((object) false);
      }
      else
        allNonNullValues.ContainsAllNonNullValues = true;
      return allNonNullValues;
    }

    public static PossibleValueSet CreateAllValues(Type expressionType)
    {
      PossibleValueSet allNonNullValues = PossibleValueSet.CreateAllNonNullValues(expressionType);
      allNonNullValues.ContainsNull = true;
      return allNonNullValues;
    }

    public static PossibleValueSet Create(Type expressionType, params object[] values)
    {
      PossibleValueSet possibleValueSet = new PossibleValueSet(expressionType);
      foreach (object obj in values)
      {
        if (obj == null)
          possibleValueSet.ContainsNull = true;
        else if (obj.GetType() == expressionType && !possibleValueSet.DistinctValues.Contains(obj))
          possibleValueSet.DistinctValues.Add(obj);
        else
          throw new AssertionFailure("Don't know how to add value to possible value set of type " + (object) expressionType + ": " + obj);
      }
      return possibleValueSet;
    }
  }
}
