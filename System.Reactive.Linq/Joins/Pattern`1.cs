// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.Pattern`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Joins
{
  public class Pattern<TSource1> : Pattern
  {
    internal Pattern(IObservable<TSource1> first) => this.First = first;

    internal IObservable<TSource1> First { get; private set; }

    public Plan<TResult> Then<TResult>(Func<TSource1, TResult> selector)
    {
      return selector != null ? (Plan<TResult>) new Plan<TSource1, TResult>(this, selector) : throw new ArgumentNullException(nameof (selector));
    }
  }
}
