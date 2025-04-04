// Decompiled with JetBrains decompiler
// Type: System.Reactive.TimeInterval`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace System.Reactive
{
  [Serializable]
  public struct TimeInterval<T>(T value, TimeSpan interval) : IEquatable<TimeInterval<T>>
  {
    private readonly TimeSpan _interval = interval;
    private readonly T _value = value;

    public T Value => this._value;

    public TimeSpan Interval => this._interval;

    public bool Equals(TimeInterval<T> other)
    {
      return other.Interval.Equals(this.Interval) && EqualityComparer<T>.Default.Equals(this.Value, other.Value);
    }

    public static bool operator ==(TimeInterval<T> first, TimeInterval<T> second)
    {
      return first.Equals(second);
    }

    public static bool operator !=(TimeInterval<T> first, TimeInterval<T> second)
    {
      return !first.Equals(second);
    }

    public override bool Equals(object obj) => obj is TimeInterval<T> other && this.Equals(other);

    public override int GetHashCode()
    {
      int num = (object) this.Value == null ? 1963 : this.Value.GetHashCode();
      return this.Interval.GetHashCode() ^ num;
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}@{1}", new object[2]
      {
        (object) this.Value,
        (object) this.Interval
      });
    }
  }
}
