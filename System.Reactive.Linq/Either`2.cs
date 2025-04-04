// Decompiled with JetBrains decompiler
// Type: System.Reactive.Either`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace System.Reactive
{
  internal abstract class Either<TLeft, TRight>
  {
    private Either()
    {
    }

    public static Either<TLeft, TRight> CreateLeft(TLeft value)
    {
      return (Either<TLeft, TRight>) new Either<TLeft, TRight>.Left(value);
    }

    public static Either<TLeft, TRight> CreateRight(TRight value)
    {
      return (Either<TLeft, TRight>) new Either<TLeft, TRight>.Right(value);
    }

    public abstract TResult Switch<TResult>(
      Func<TLeft, TResult> caseLeft,
      Func<TRight, TResult> caseRight);

    public abstract void Switch(Action<TLeft> caseLeft, Action<TRight> caseRight);

    public sealed class Left : Either<TLeft, TRight>, IEquatable<Either<TLeft, TRight>.Left>
    {
      public TLeft Value { get; private set; }

      public Left(TLeft value) => this.Value = value;

      public override TResult Switch<TResult>(
        Func<TLeft, TResult> caseLeft,
        Func<TRight, TResult> caseRight)
      {
        return caseLeft(this.Value);
      }

      public override void Switch(Action<TLeft> caseLeft, Action<TRight> caseRight)
      {
        caseLeft(this.Value);
      }

      public bool Equals(Either<TLeft, TRight>.Left other)
      {
        if (other == this)
          return true;
        return other != null && EqualityComparer<TLeft>.Default.Equals(this.Value, other.Value);
      }

      public override bool Equals(object obj) => this.Equals(obj as Either<TLeft, TRight>.Left);

      public override int GetHashCode() => EqualityComparer<TLeft>.Default.GetHashCode(this.Value);

      public override string ToString()
      {
        return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Left({0})", new object[1]
        {
          (object) this.Value
        });
      }
    }

    public sealed class Right : Either<TLeft, TRight>, IEquatable<Either<TLeft, TRight>.Right>
    {
      public TRight Value { get; private set; }

      public Right(TRight value) => this.Value = value;

      public override TResult Switch<TResult>(
        Func<TLeft, TResult> caseLeft,
        Func<TRight, TResult> caseRight)
      {
        return caseRight(this.Value);
      }

      public override void Switch(Action<TLeft> caseLeft, Action<TRight> caseRight)
      {
        caseRight(this.Value);
      }

      public bool Equals(Either<TLeft, TRight>.Right other)
      {
        if (other == this)
          return true;
        return other != null && EqualityComparer<TRight>.Default.Equals(this.Value, other.Value);
      }

      public override bool Equals(object obj) => this.Equals(obj as Either<TLeft, TRight>.Right);

      public override int GetHashCode() => EqualityComparer<TRight>.Default.GetHashCode(this.Value);

      public override string ToString()
      {
        return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Right({0})", new object[1]
        {
          (object) this.Value
        });
      }
    }
  }
}
