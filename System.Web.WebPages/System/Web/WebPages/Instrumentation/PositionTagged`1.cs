// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Instrumentation.PositionTagged`1
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.Internal.Web.Utils;
using System.Diagnostics;

#nullable disable
namespace System.Web.WebPages.Instrumentation
{
  [DebuggerDisplay("({Position})\"{Value}\"")]
  public class PositionTagged<T>
  {
    private PositionTagged()
    {
      this.Position = 0;
      this.Value = default (T);
    }

    public PositionTagged(T value, int offset)
    {
      this.Position = offset;
      this.Value = value;
    }

    public int Position { get; private set; }

    public T Value { get; private set; }

    public override bool Equals(object obj)
    {
      PositionTagged<T> positionTagged = obj as PositionTagged<T>;
      return positionTagged != (PositionTagged<T>) null && positionTagged.Position == this.Position && object.Equals((object) positionTagged.Value, (object) this.Value);
    }

    public override int GetHashCode()
    {
      return HashCodeCombiner.Start().Add(this.Position).Add((object) this.Value).CombinedHash;
    }

    public override string ToString() => this.Value.ToString();

    public static implicit operator T(PositionTagged<T> value) => value.Value;

    public static implicit operator PositionTagged<T>(Tuple<T, int> value)
    {
      return new PositionTagged<T>(value.Item1, value.Item2);
    }

    public static bool operator ==(PositionTagged<T> left, PositionTagged<T> right)
    {
      return object.Equals((object) left, (object) right);
    }

    public static bool operator !=(PositionTagged<T> left, PositionTagged<T> right)
    {
      return !object.Equals((object) left, (object) right);
    }
  }
}
