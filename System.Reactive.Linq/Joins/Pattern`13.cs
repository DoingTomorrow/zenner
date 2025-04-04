// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.Pattern`13
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Joins
{
  public class Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13> : 
    Pattern
  {
    internal Pattern(
      IObservable<TSource1> first,
      IObservable<TSource2> second,
      IObservable<TSource3> third,
      IObservable<TSource4> fourth,
      IObservable<TSource5> fifth,
      IObservable<TSource6> sixth,
      IObservable<TSource7> seventh,
      IObservable<TSource8> eighth,
      IObservable<TSource9> ninth,
      IObservable<TSource10> tenth,
      IObservable<TSource11> eleventh,
      IObservable<TSource12> twelfth,
      IObservable<TSource13> thirteenth)
    {
      this.First = first;
      this.Second = second;
      this.Third = third;
      this.Fourth = fourth;
      this.Fifth = fifth;
      this.Sixth = sixth;
      this.Seventh = seventh;
      this.Eighth = eighth;
      this.Ninth = ninth;
      this.Tenth = tenth;
      this.Eleventh = eleventh;
      this.Twelfth = twelfth;
      this.Thirteenth = thirteenth;
    }

    internal IObservable<TSource1> First { get; private set; }

    internal IObservable<TSource2> Second { get; private set; }

    internal IObservable<TSource3> Third { get; private set; }

    internal IObservable<TSource4> Fourth { get; private set; }

    internal IObservable<TSource5> Fifth { get; private set; }

    internal IObservable<TSource6> Sixth { get; private set; }

    internal IObservable<TSource7> Seventh { get; private set; }

    internal IObservable<TSource8> Eighth { get; private set; }

    internal IObservable<TSource9> Ninth { get; private set; }

    internal IObservable<TSource10> Tenth { get; private set; }

    internal IObservable<TSource11> Eleventh { get; private set; }

    internal IObservable<TSource12> Twelfth { get; private set; }

    internal IObservable<TSource13> Thirteenth { get; private set; }

    public Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14> And<TSource14>(
      IObservable<TSource14> other)
    {
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      return new Pattern<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14>(this.First, this.Second, this.Third, this.Fourth, this.Fifth, this.Sixth, this.Seventh, this.Eighth, this.Ninth, this.Tenth, this.Eleventh, this.Twelfth, this.Thirteenth, other);
    }

    public Plan<TResult> Then<TResult>(
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> selector)
    {
      return selector != null ? (Plan<TResult>) new Plan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(this, selector) : throw new ArgumentNullException(nameof (selector));
    }
  }
}
