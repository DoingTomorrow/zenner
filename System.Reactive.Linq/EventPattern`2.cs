// Decompiled with JetBrains decompiler
// Type: System.Reactive.EventPattern`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive
{
  public class EventPattern<TSender, TEventArgs> : 
    IEquatable<EventPattern<TSender, TEventArgs>>,
    IEventPattern<TSender, TEventArgs>
  {
    public EventPattern(TSender sender, TEventArgs e)
    {
      this.Sender = sender;
      this.EventArgs = e;
    }

    public TSender Sender { get; private set; }

    public TEventArgs EventArgs { get; private set; }

    public bool Equals(EventPattern<TSender, TEventArgs> other)
    {
      if ((object) other == null)
        return false;
      if ((object) this == (object) other)
        return true;
      return EqualityComparer<TSender>.Default.Equals(this.Sender, other.Sender) && EqualityComparer<TEventArgs>.Default.Equals(this.EventArgs, other.EventArgs);
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as EventPattern<TSender, TEventArgs>);
    }

    public override int GetHashCode()
    {
      int hashCode1 = EqualityComparer<TSender>.Default.GetHashCode(this.Sender);
      int hashCode2 = EqualityComparer<TEventArgs>.Default.GetHashCode(this.EventArgs);
      return (hashCode1 << 5) + (hashCode1 ^ hashCode2);
    }

    public static bool operator ==(
      EventPattern<TSender, TEventArgs> first,
      EventPattern<TSender, TEventArgs> second)
    {
      return object.Equals((object) first, (object) second);
    }

    public static bool operator !=(
      EventPattern<TSender, TEventArgs> first,
      EventPattern<TSender, TEventArgs> second)
    {
      return !object.Equals((object) first, (object) second);
    }
  }
}
