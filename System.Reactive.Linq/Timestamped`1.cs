// Decompiled with JetBrains decompiler
// Type: System.Reactive.Timestamped`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace System.Reactive
{
  [Serializable]
  public struct Timestamped<T>(T value, DateTimeOffset timestamp) : IEquatable<Timestamped<T>>
  {
    private readonly DateTimeOffset _timestamp = timestamp;
    private readonly T _value = value;

    public T Value => this._value;

    public DateTimeOffset Timestamp => this._timestamp;

    public bool Equals(Timestamped<T> other)
    {
      return other.Timestamp.Equals(this.Timestamp) && EqualityComparer<T>.Default.Equals(this.Value, other.Value);
    }

    public static bool operator ==(Timestamped<T> first, Timestamped<T> second)
    {
      return first.Equals(second);
    }

    public static bool operator !=(Timestamped<T> first, Timestamped<T> second)
    {
      return !first.Equals(second);
    }

    public override bool Equals(object obj) => obj is Timestamped<T> other && this.Equals(other);

    public override int GetHashCode()
    {
      int num = (object) this.Value == null ? 1979 : this.Value.GetHashCode();
      return this._timestamp.GetHashCode() ^ num;
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}@{1}", new object[2]
      {
        (object) this.Value,
        (object) this.Timestamp
      });
    }
  }
}
