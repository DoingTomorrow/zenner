// Decompiled with JetBrains decompiler
// Type: Castle.Core.Pair`2
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.Core
{
  public class Pair<TFirst, TSecond> : IEquatable<Pair<TFirst, TSecond>>
  {
    private readonly TFirst first;
    private readonly TSecond second;

    public Pair(TFirst first, TSecond second)
    {
      this.first = first;
      this.second = second;
    }

    public TFirst First => this.first;

    public TSecond Second => this.second;

    public override string ToString() => this.first.ToString() + " " + (object) this.second;

    public bool Equals(Pair<TFirst, TSecond> other)
    {
      return other != null && object.Equals((object) this.first, (object) other.first) && object.Equals((object) this.second, (object) other.second);
    }

    public override bool Equals(object obj)
    {
      return object.ReferenceEquals((object) this, obj) || this.Equals(obj as Pair<TFirst, TSecond>);
    }

    public override int GetHashCode() => this.first.GetHashCode() + 29 * this.second.GetHashCode();
  }
}
