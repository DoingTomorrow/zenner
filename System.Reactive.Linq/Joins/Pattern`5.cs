// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.Pattern`5
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Joins
{
  public class Pattern<TSource1, TSource2, TSource3, TSource4, TSource5> : Pattern
  {
    internal Pattern(
      IObservable<TSource1> first,
      IObservable<TSource2> second,
      IObservable<TSource3> third,
      IObservable<TSource4> fourth,
      IObservable<TSource5> fifth)
    {
      this.First = first;
      this.Second = second;
      this.Third = third;
      this.Fourth = fourth;
      this.Fifth = fifth;
    }

    internal IObservable<TSource1> First { get; private set; }

    internal IObservable<TSource2> Second { get; private set; }

    internal IObservable<TSource3> Third { get; private set; }

    internal IObservable<TSource4> Fourth { get; private set; }

    internal IObservable<TSource5> Fifth { get; private set; }

    public Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6> And<TSource6>(
      IObservable<TSource6> other)
    {
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      return new Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6>(this.First, this.Second, this.Third, this.Fourth, this.Fifth, other);
    }

    public Plan<TResult> Then<TResult>(
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> selector)
    {
      return selector != null ? (Plan<TResult>) new Plan<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(this, selector) : throw new ArgumentNullException(nameof (selector));
    }
  }
}
