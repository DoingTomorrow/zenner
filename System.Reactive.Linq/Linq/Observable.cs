// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observable
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Joins;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace System.Reactive.Linq
{
  public static class Observable
  {
    private static IQueryLanguage s_impl = QueryServices.GetQueryImpl<IQueryLanguage>((IQueryLanguage) new QueryLanguage());

    public static IObservable<TAccumulate> Aggregate<TSource, TAccumulate>(
      this IObservable<TSource> source,
      TAccumulate seed,
      Func<TAccumulate, TSource, TAccumulate> accumulator)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (accumulator == null)
        throw new ArgumentNullException(nameof (accumulator));
      return Observable.s_impl.Aggregate<TSource, TAccumulate>(source, seed, accumulator);
    }

    public static IObservable<TResult> Aggregate<TSource, TAccumulate, TResult>(
      this IObservable<TSource> source,
      TAccumulate seed,
      Func<TAccumulate, TSource, TAccumulate> accumulator,
      Func<TAccumulate, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (accumulator == null)
        throw new ArgumentNullException(nameof (accumulator));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Aggregate<TSource, TAccumulate, TResult>(source, seed, accumulator, resultSelector);
    }

    public static IObservable<TSource> Aggregate<TSource>(
      this IObservable<TSource> source,
      Func<TSource, TSource, TSource> accumulator)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (accumulator == null)
        throw new ArgumentNullException(nameof (accumulator));
      return Observable.s_impl.Aggregate<TSource>(source, accumulator);
    }

    public static IObservable<bool> All<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.All<TSource>(source, predicate);
    }

    public static IObservable<bool> Any<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Any<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<bool> Any<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.Any<TSource>(source, predicate);
    }

    public static IObservable<double> Average(this IObservable<double> source)
    {
      return source != null ? Observable.s_impl.Average(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<float> Average(this IObservable<float> source)
    {
      return source != null ? Observable.s_impl.Average(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<Decimal> Average(this IObservable<Decimal> source)
    {
      return source != null ? Observable.s_impl.Average(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<double> Average(this IObservable<int> source)
    {
      return source != null ? Observable.s_impl.Average(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<double> Average(this IObservable<long> source)
    {
      return source != null ? Observable.s_impl.Average(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<double?> Average(this IObservable<double?> source)
    {
      return source != null ? Observable.s_impl.Average(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<float?> Average(this IObservable<float?> source)
    {
      return source != null ? Observable.s_impl.Average(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<Decimal?> Average(this IObservable<Decimal?> source)
    {
      return source != null ? Observable.s_impl.Average(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<double?> Average(this IObservable<int?> source)
    {
      return source != null ? Observable.s_impl.Average(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<double?> Average(this IObservable<long?> source)
    {
      return source != null ? Observable.s_impl.Average(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<Decimal> Average<TSource>(
      this IObservable<TSource> source,
      Func<TSource, Decimal> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Average<TSource>(source, selector);
    }

    public static IObservable<double> Average<TSource>(
      this IObservable<TSource> source,
      Func<TSource, double> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Average<TSource>(source, selector);
    }

    public static IObservable<float> Average<TSource>(
      this IObservable<TSource> source,
      Func<TSource, float> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Average<TSource>(source, selector);
    }

    public static IObservable<double> Average<TSource>(
      this IObservable<TSource> source,
      Func<TSource, int> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Average<TSource>(source, selector);
    }

    public static IObservable<double> Average<TSource>(
      this IObservable<TSource> source,
      Func<TSource, long> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Average<TSource>(source, selector);
    }

    public static IObservable<Decimal?> Average<TSource>(
      this IObservable<TSource> source,
      Func<TSource, Decimal?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Average<TSource>(source, selector);
    }

    public static IObservable<double?> Average<TSource>(
      this IObservable<TSource> source,
      Func<TSource, double?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Average<TSource>(source, selector);
    }

    public static IObservable<float?> Average<TSource>(
      this IObservable<TSource> source,
      Func<TSource, float?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Average<TSource>(source, selector);
    }

    public static IObservable<double?> Average<TSource>(
      this IObservable<TSource> source,
      Func<TSource, int?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Average<TSource>(source, selector);
    }

    public static IObservable<double?> Average<TSource>(
      this IObservable<TSource> source,
      Func<TSource, long?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Average<TSource>(source, selector);
    }

    public static IObservable<bool> Contains<TSource>(
      this IObservable<TSource> source,
      TSource value)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return Observable.s_impl.Contains<TSource>(source, value);
    }

    public static IObservable<bool> Contains<TSource>(
      this IObservable<TSource> source,
      TSource value,
      IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.Contains<TSource>(source, value, comparer);
    }

    public static IObservable<int> Count<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Count<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<int> Count<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.Count<TSource>(source, predicate);
    }

    public static IObservable<TSource> ElementAt<TSource>(
      this IObservable<TSource> source,
      int index)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      return Observable.s_impl.ElementAt<TSource>(source, index);
    }

    public static IObservable<TSource> ElementAtOrDefault<TSource>(
      this IObservable<TSource> source,
      int index)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      return Observable.s_impl.ElementAtOrDefault<TSource>(source, index);
    }

    public static IObservable<TSource> FirstAsync<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.FirstAsync<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> FirstAsync<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.FirstAsync<TSource>(source, predicate);
    }

    public static IObservable<TSource> FirstOrDefaultAsync<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.FirstOrDefaultAsync<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> FirstOrDefaultAsync<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.FirstOrDefaultAsync<TSource>(source, predicate);
    }

    public static IObservable<bool> IsEmpty<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.IsEmpty<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> LastAsync<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.LastAsync<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> LastAsync<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.LastAsync<TSource>(source, predicate);
    }

    public static IObservable<TSource> LastOrDefaultAsync<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.LastOrDefaultAsync<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> LastOrDefaultAsync<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.LastOrDefaultAsync<TSource>(source, predicate);
    }

    public static IObservable<long> LongCount<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.LongCount<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<long> LongCount<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.LongCount<TSource>(source, predicate);
    }

    public static IObservable<TSource> Max<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Max<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> Max<TSource>(
      this IObservable<TSource> source,
      IComparer<TSource> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.Max<TSource>(source, comparer);
    }

    public static IObservable<double> Max(this IObservable<double> source)
    {
      return source != null ? Observable.s_impl.Max(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<float> Max(this IObservable<float> source)
    {
      return source != null ? Observable.s_impl.Max(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<Decimal> Max(this IObservable<Decimal> source)
    {
      return source != null ? Observable.s_impl.Max(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<int> Max(this IObservable<int> source)
    {
      return source != null ? Observable.s_impl.Max(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<long> Max(this IObservable<long> source)
    {
      return source != null ? Observable.s_impl.Max(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<double?> Max(this IObservable<double?> source)
    {
      return source != null ? Observable.s_impl.Max(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<float?> Max(this IObservable<float?> source)
    {
      return source != null ? Observable.s_impl.Max(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<Decimal?> Max(this IObservable<Decimal?> source)
    {
      return source != null ? Observable.s_impl.Max(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<int?> Max(this IObservable<int?> source)
    {
      return source != null ? Observable.s_impl.Max(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<long?> Max(this IObservable<long?> source)
    {
      return source != null ? Observable.s_impl.Max(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TResult> Max<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TSource, TResult> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Max<TSource, TResult>(source, selector);
    }

    public static IObservable<TResult> Max<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TSource, TResult> selector,
      IComparer<TResult> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.Max<TSource, TResult>(source, selector, comparer);
    }

    public static IObservable<double> Max<TSource>(
      this IObservable<TSource> source,
      Func<TSource, double> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Max<TSource>(source, selector);
    }

    public static IObservable<float> Max<TSource>(
      this IObservable<TSource> source,
      Func<TSource, float> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Max<TSource>(source, selector);
    }

    public static IObservable<Decimal> Max<TSource>(
      this IObservable<TSource> source,
      Func<TSource, Decimal> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Max<TSource>(source, selector);
    }

    public static IObservable<int> Max<TSource>(
      this IObservable<TSource> source,
      Func<TSource, int> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Max<TSource>(source, selector);
    }

    public static IObservable<long> Max<TSource>(
      this IObservable<TSource> source,
      Func<TSource, long> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Max<TSource>(source, selector);
    }

    public static IObservable<double?> Max<TSource>(
      this IObservable<TSource> source,
      Func<TSource, double?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Max<TSource>(source, selector);
    }

    public static IObservable<float?> Max<TSource>(
      this IObservable<TSource> source,
      Func<TSource, float?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Max<TSource>(source, selector);
    }

    public static IObservable<Decimal?> Max<TSource>(
      this IObservable<TSource> source,
      Func<TSource, Decimal?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Max<TSource>(source, selector);
    }

    public static IObservable<int?> Max<TSource>(
      this IObservable<TSource> source,
      Func<TSource, int?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Max<TSource>(source, selector);
    }

    public static IObservable<long?> Max<TSource>(
      this IObservable<TSource> source,
      Func<TSource, long?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Max<TSource>(source, selector);
    }

    public static IObservable<IList<TSource>> MaxBy<TSource, TKey>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      return Observable.s_impl.MaxBy<TSource, TKey>(source, keySelector);
    }

    public static IObservable<IList<TSource>> MaxBy<TSource, TKey>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.MaxBy<TSource, TKey>(source, keySelector, comparer);
    }

    public static IObservable<TSource> Min<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Min<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> Min<TSource>(
      this IObservable<TSource> source,
      IComparer<TSource> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.Min<TSource>(source, comparer);
    }

    public static IObservable<double> Min(this IObservable<double> source)
    {
      return source != null ? Observable.s_impl.Min(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<float> Min(this IObservable<float> source)
    {
      return source != null ? Observable.s_impl.Min(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<Decimal> Min(this IObservable<Decimal> source)
    {
      return source != null ? Observable.s_impl.Min(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<int> Min(this IObservable<int> source)
    {
      return source != null ? Observable.s_impl.Min(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<long> Min(this IObservable<long> source)
    {
      return source != null ? Observable.s_impl.Min(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<double?> Min(this IObservable<double?> source)
    {
      return source != null ? Observable.s_impl.Min(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<float?> Min(this IObservable<float?> source)
    {
      return source != null ? Observable.s_impl.Min(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<Decimal?> Min(this IObservable<Decimal?> source)
    {
      return source != null ? Observable.s_impl.Min(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<int?> Min(this IObservable<int?> source)
    {
      return source != null ? Observable.s_impl.Min(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<long?> Min(this IObservable<long?> source)
    {
      return source != null ? Observable.s_impl.Min(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TResult> Min<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TSource, TResult> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Min<TSource, TResult>(source, selector);
    }

    public static IObservable<TResult> Min<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TSource, TResult> selector,
      IComparer<TResult> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.Min<TSource, TResult>(source, selector, comparer);
    }

    public static IObservable<double> Min<TSource>(
      this IObservable<TSource> source,
      Func<TSource, double> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Min<TSource>(source, selector);
    }

    public static IObservable<float> Min<TSource>(
      this IObservable<TSource> source,
      Func<TSource, float> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Min<TSource>(source, selector);
    }

    public static IObservable<Decimal> Min<TSource>(
      this IObservable<TSource> source,
      Func<TSource, Decimal> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Min<TSource>(source, selector);
    }

    public static IObservable<int> Min<TSource>(
      this IObservable<TSource> source,
      Func<TSource, int> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Min<TSource>(source, selector);
    }

    public static IObservable<long> Min<TSource>(
      this IObservable<TSource> source,
      Func<TSource, long> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Min<TSource>(source, selector);
    }

    public static IObservable<double?> Min<TSource>(
      this IObservable<TSource> source,
      Func<TSource, double?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Min<TSource>(source, selector);
    }

    public static IObservable<float?> Min<TSource>(
      this IObservable<TSource> source,
      Func<TSource, float?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Min<TSource>(source, selector);
    }

    public static IObservable<Decimal?> Min<TSource>(
      this IObservable<TSource> source,
      Func<TSource, Decimal?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Min<TSource>(source, selector);
    }

    public static IObservable<int?> Min<TSource>(
      this IObservable<TSource> source,
      Func<TSource, int?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Min<TSource>(source, selector);
    }

    public static IObservable<long?> Min<TSource>(
      this IObservable<TSource> source,
      Func<TSource, long?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Min<TSource>(source, selector);
    }

    public static IObservable<IList<TSource>> MinBy<TSource, TKey>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      return Observable.s_impl.MinBy<TSource, TKey>(source, keySelector);
    }

    public static IObservable<IList<TSource>> MinBy<TSource, TKey>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.MinBy<TSource, TKey>(source, keySelector, comparer);
    }

    public static IObservable<bool> SequenceEqual<TSource>(
      this IObservable<TSource> first,
      IObservable<TSource> second)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      return Observable.s_impl.SequenceEqual<TSource>(first, second);
    }

    public static IObservable<bool> SequenceEqual<TSource>(
      this IObservable<TSource> first,
      IObservable<TSource> second,
      IEqualityComparer<TSource> comparer)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.SequenceEqual<TSource>(first, second, comparer);
    }

    public static IObservable<bool> SequenceEqual<TSource>(
      this IObservable<TSource> first,
      IEnumerable<TSource> second)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      return Observable.s_impl.SequenceEqual<TSource>(first, second);
    }

    public static IObservable<bool> SequenceEqual<TSource>(
      this IObservable<TSource> first,
      IEnumerable<TSource> second,
      IEqualityComparer<TSource> comparer)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.SequenceEqual<TSource>(first, second, comparer);
    }

    public static IObservable<TSource> SingleAsync<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.SingleAsync<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> SingleAsync<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.SingleAsync<TSource>(source, predicate);
    }

    public static IObservable<TSource> SingleOrDefaultAsync<TSource>(
      this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.SingleOrDefaultAsync<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> SingleOrDefaultAsync<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.SingleOrDefaultAsync<TSource>(source, predicate);
    }

    public static IObservable<double> Sum(this IObservable<double> source)
    {
      return source != null ? Observable.s_impl.Sum(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<float> Sum(this IObservable<float> source)
    {
      return source != null ? Observable.s_impl.Sum(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<Decimal> Sum(this IObservable<Decimal> source)
    {
      return source != null ? Observable.s_impl.Sum(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<int> Sum(this IObservable<int> source)
    {
      return source != null ? Observable.s_impl.Sum(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<long> Sum(this IObservable<long> source)
    {
      return source != null ? Observable.s_impl.Sum(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<double?> Sum(this IObservable<double?> source)
    {
      return source != null ? Observable.s_impl.Sum(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<float?> Sum(this IObservable<float?> source)
    {
      return source != null ? Observable.s_impl.Sum(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<Decimal?> Sum(this IObservable<Decimal?> source)
    {
      return source != null ? Observable.s_impl.Sum(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<int?> Sum(this IObservable<int?> source)
    {
      return source != null ? Observable.s_impl.Sum(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<long?> Sum(this IObservable<long?> source)
    {
      return source != null ? Observable.s_impl.Sum(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<double> Sum<TSource>(
      this IObservable<TSource> source,
      Func<TSource, double> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Sum<TSource>(source, selector);
    }

    public static IObservable<float> Sum<TSource>(
      this IObservable<TSource> source,
      Func<TSource, float> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Sum<TSource>(source, selector);
    }

    public static IObservable<Decimal> Sum<TSource>(
      this IObservable<TSource> source,
      Func<TSource, Decimal> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Sum<TSource>(source, selector);
    }

    public static IObservable<int> Sum<TSource>(
      this IObservable<TSource> source,
      Func<TSource, int> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Sum<TSource>(source, selector);
    }

    public static IObservable<long> Sum<TSource>(
      this IObservable<TSource> source,
      Func<TSource, long> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Sum<TSource>(source, selector);
    }

    public static IObservable<double?> Sum<TSource>(
      this IObservable<TSource> source,
      Func<TSource, double?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Sum<TSource>(source, selector);
    }

    public static IObservable<float?> Sum<TSource>(
      this IObservable<TSource> source,
      Func<TSource, float?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Sum<TSource>(source, selector);
    }

    public static IObservable<Decimal?> Sum<TSource>(
      this IObservable<TSource> source,
      Func<TSource, Decimal?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Sum<TSource>(source, selector);
    }

    public static IObservable<int?> Sum<TSource>(
      this IObservable<TSource> source,
      Func<TSource, int?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Sum<TSource>(source, selector);
    }

    public static IObservable<long?> Sum<TSource>(
      this IObservable<TSource> source,
      Func<TSource, long?> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Sum<TSource>(source, selector);
    }

    public static IObservable<TSource[]> ToArray<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.ToArray<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      return Observable.s_impl.ToDictionary<TSource, TKey>(source, keySelector);
    }

    public static IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.ToDictionary<TSource, TKey>(source, keySelector, comparer);
    }

    public static IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (elementSelector == null)
        throw new ArgumentNullException(nameof (elementSelector));
      return Observable.s_impl.ToDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector);
    }

    public static IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (elementSelector == null)
        throw new ArgumentNullException(nameof (elementSelector));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.ToDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
    }

    public static IObservable<IList<TSource>> ToList<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.ToList<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      return Observable.s_impl.ToLookup<TSource, TKey>(source, keySelector);
    }

    public static IObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.ToLookup<TSource, TKey>(source, keySelector, comparer);
    }

    public static IObservable<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (elementSelector == null)
        throw new ArgumentNullException(nameof (elementSelector));
      return Observable.s_impl.ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector);
    }

    public static IObservable<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (elementSelector == null)
        throw new ArgumentNullException(nameof (elementSelector));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<IObservable<TResult>> FromAsyncPattern<TResult>(
      Func<AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TResult>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, IObservable<TResult>> FromAsyncPattern<TArg1, TResult>(
      Func<TArg1, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TResult>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TResult>(
      Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TResult>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TResult>(
      Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TResult>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TResult>(
      Func<TArg1, TArg2, TArg3, TArg4, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TResult>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IObservable<TResult>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<IObservable<Unit>> FromAsyncPattern(
      Func<AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, IObservable<Unit>> FromAsyncPattern<TArg1>(
      Func<TArg1, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2>(
      Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3>(
      Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4>(
      Func<TArg1, TArg2, TArg3, TArg4, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(begin, end);
    }

    [Obsolete("This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.")]
    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IObservable<Unit>> FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(
      Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      if (begin == null)
        throw new ArgumentNullException(nameof (begin));
      if (end == null)
        throw new ArgumentNullException(nameof (end));
      return Observable.s_impl.FromAsyncPattern<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(begin, end);
    }

    public static IObservable<TResult> Start<TResult>(Func<TResult> function)
    {
      return function != null ? Observable.s_impl.Start<TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static IObservable<TResult> Start<TResult>(Func<TResult> function, IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Start<TResult>(function, scheduler);
    }

    public static IObservable<TResult> StartAsync<TResult>(Func<Task<TResult>> functionAsync)
    {
      return functionAsync != null ? Observable.s_impl.StartAsync<TResult>(functionAsync) : throw new ArgumentNullException(nameof (functionAsync));
    }

    public static IObservable<TResult> StartAsync<TResult>(
      Func<Task<TResult>> functionAsync,
      IScheduler scheduler)
    {
      if (functionAsync == null)
        throw new ArgumentNullException(nameof (functionAsync));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.StartAsync<TResult>(functionAsync, scheduler);
    }

    public static IObservable<TResult> StartAsync<TResult>(
      Func<CancellationToken, Task<TResult>> functionAsync)
    {
      return functionAsync != null ? Observable.s_impl.StartAsync<TResult>(functionAsync) : throw new ArgumentNullException(nameof (functionAsync));
    }

    public static IObservable<TResult> StartAsync<TResult>(
      Func<CancellationToken, Task<TResult>> functionAsync,
      IScheduler scheduler)
    {
      if (functionAsync == null)
        throw new ArgumentNullException(nameof (functionAsync));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.StartAsync<TResult>(functionAsync, scheduler);
    }

    public static IObservable<Unit> Start(Action action)
    {
      return action != null ? Observable.s_impl.Start(action) : throw new ArgumentNullException(nameof (action));
    }

    public static IObservable<Unit> Start(Action action, IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Start(action, scheduler);
    }

    public static IObservable<Unit> StartAsync(Func<Task> actionAsync)
    {
      return actionAsync != null ? Observable.s_impl.StartAsync(actionAsync) : throw new ArgumentNullException(nameof (actionAsync));
    }

    public static IObservable<Unit> StartAsync(Func<Task> actionAsync, IScheduler scheduler)
    {
      if (actionAsync == null)
        throw new ArgumentNullException(nameof (actionAsync));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.StartAsync(actionAsync, scheduler);
    }

    public static IObservable<Unit> StartAsync(Func<CancellationToken, Task> actionAsync)
    {
      return actionAsync != null ? Observable.s_impl.StartAsync(actionAsync) : throw new ArgumentNullException(nameof (actionAsync));
    }

    public static IObservable<Unit> StartAsync(
      Func<CancellationToken, Task> actionAsync,
      IScheduler scheduler)
    {
      if (actionAsync == null)
        throw new ArgumentNullException(nameof (actionAsync));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.StartAsync(actionAsync, scheduler);
    }

    public static IObservable<TResult> FromAsync<TResult>(Func<Task<TResult>> functionAsync)
    {
      return functionAsync != null ? Observable.s_impl.FromAsync<TResult>(functionAsync) : throw new ArgumentNullException(nameof (functionAsync));
    }

    public static IObservable<TResult> FromAsync<TResult>(
      Func<Task<TResult>> functionAsync,
      IScheduler scheduler)
    {
      if (functionAsync == null)
        throw new ArgumentNullException(nameof (functionAsync));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromAsync<TResult>(functionAsync, scheduler);
    }

    public static IObservable<TResult> FromAsync<TResult>(
      Func<CancellationToken, Task<TResult>> functionAsync)
    {
      return functionAsync != null ? Observable.s_impl.FromAsync<TResult>(functionAsync) : throw new ArgumentNullException(nameof (functionAsync));
    }

    public static IObservable<TResult> FromAsync<TResult>(
      Func<CancellationToken, Task<TResult>> functionAsync,
      IScheduler scheduler)
    {
      if (functionAsync == null)
        throw new ArgumentNullException(nameof (functionAsync));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromAsync<TResult>(functionAsync, scheduler);
    }

    public static IObservable<Unit> FromAsync(Func<Task> actionAsync)
    {
      return actionAsync != null ? Observable.s_impl.FromAsync(actionAsync) : throw new ArgumentNullException(nameof (actionAsync));
    }

    public static IObservable<Unit> FromAsync(Func<Task> actionAsync, IScheduler scheduler)
    {
      if (actionAsync == null)
        throw new ArgumentNullException(nameof (actionAsync));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromAsync(actionAsync, scheduler);
    }

    public static IObservable<Unit> FromAsync(Func<CancellationToken, Task> actionAsync)
    {
      return actionAsync != null ? Observable.s_impl.FromAsync(actionAsync) : throw new ArgumentNullException(nameof (actionAsync));
    }

    public static IObservable<Unit> FromAsync(
      Func<CancellationToken, Task> actionAsync,
      IScheduler scheduler)
    {
      if (actionAsync == null)
        throw new ArgumentNullException(nameof (actionAsync));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromAsync(actionAsync, scheduler);
    }

    public static Func<IObservable<TResult>> ToAsync<TResult>(this Func<TResult> function)
    {
      return function != null ? Observable.s_impl.ToAsync<TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static Func<IObservable<TResult>> ToAsync<TResult>(
      this Func<TResult> function,
      IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TResult>(function, scheduler);
    }

    public static Func<TArg1, IObservable<TResult>> ToAsync<TArg1, TResult>(
      this Func<TArg1, TResult> function)
    {
      return function != null ? Observable.s_impl.ToAsync<TArg1, TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static Func<TArg1, IObservable<TResult>> ToAsync<TArg1, TResult>(
      this Func<TArg1, TResult> function,
      IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TResult>(function, scheduler);
    }

    public static Func<TArg1, TArg2, IObservable<TResult>> ToAsync<TArg1, TArg2, TResult>(
      this Func<TArg1, TArg2, TResult> function)
    {
      return function != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static Func<TArg1, TArg2, IObservable<TResult>> ToAsync<TArg1, TArg2, TResult>(
      this Func<TArg1, TArg2, TResult> function,
      IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TResult>(function, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TResult>(
      this Func<TArg1, TArg2, TArg3, TResult> function)
    {
      return function != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static Func<TArg1, TArg2, TArg3, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TResult>(
      this Func<TArg1, TArg2, TArg3, TResult> function,
      IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TResult>(function, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TResult> function)
    {
      return function != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TResult> function,
      IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TResult>(function, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult> function)
    {
      return function != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult> function,
      IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(function, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult> function)
    {
      return function != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult> function,
      IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(function, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult> function)
    {
      return function != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult> function,
      IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(function, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult> function)
    {
      return function != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult> function,
      IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(function, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult> function)
    {
      return function != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult> function,
      IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(function, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult> function)
    {
      return function != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult> function,
      IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(function, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult> function)
    {
      return function != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult> function,
      IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(function, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult> function)
    {
      return function != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult> function,
      IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(function, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult> function)
    {
      return function != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult> function,
      IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(function, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult> function)
    {
      return function != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult> function,
      IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(function, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult> function)
    {
      return function != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult> function,
      IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(function, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult> function)
    {
      return function != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult>(function) : throw new ArgumentNullException(nameof (function));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, IObservable<TResult>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult>(
      this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult> function,
      IScheduler scheduler)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, TResult>(function, scheduler);
    }

    public static Func<IObservable<Unit>> ToAsync(this Action action)
    {
      return action != null ? Observable.s_impl.ToAsync(action) : throw new ArgumentNullException(nameof (action));
    }

    public static Func<IObservable<Unit>> ToAsync(this Action action, IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync(action, scheduler);
    }

    public static Func<TArg1, IObservable<Unit>> ToAsync<TArg1>(this Action<TArg1> action)
    {
      return action != null ? Observable.s_impl.ToAsync<TArg1>(action) : throw new ArgumentNullException(nameof (action));
    }

    public static Func<TArg1, IObservable<Unit>> ToAsync<TArg1>(
      this Action<TArg1> action,
      IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1>(action, scheduler);
    }

    public static Func<TArg1, TArg2, IObservable<Unit>> ToAsync<TArg1, TArg2>(
      this Action<TArg1, TArg2> action)
    {
      return action != null ? Observable.s_impl.ToAsync<TArg1, TArg2>(action) : throw new ArgumentNullException(nameof (action));
    }

    public static Func<TArg1, TArg2, IObservable<Unit>> ToAsync<TArg1, TArg2>(
      this Action<TArg1, TArg2> action,
      IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2>(action, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3>(
      this Action<TArg1, TArg2, TArg3> action)
    {
      return action != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3>(action) : throw new ArgumentNullException(nameof (action));
    }

    public static Func<TArg1, TArg2, TArg3, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3>(
      this Action<TArg1, TArg2, TArg3> action,
      IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3>(action, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4>(
      this Action<TArg1, TArg2, TArg3, TArg4> action)
    {
      return action != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4>(action) : throw new ArgumentNullException(nameof (action));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4>(
      this Action<TArg1, TArg2, TArg3, TArg4> action,
      IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4>(action, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5> action)
    {
      return action != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5>(action) : throw new ArgumentNullException(nameof (action));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5> action,
      IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5>(action, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> action)
    {
      return action != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(action) : throw new ArgumentNullException(nameof (action));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> action,
      IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(action, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> action)
    {
      return action != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(action) : throw new ArgumentNullException(nameof (action));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> action,
      IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(action, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> action)
    {
      return action != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(action) : throw new ArgumentNullException(nameof (action));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> action,
      IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(action, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> action)
    {
      return action != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(action) : throw new ArgumentNullException(nameof (action));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> action,
      IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(action, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> action)
    {
      return action != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(action) : throw new ArgumentNullException(nameof (action));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> action,
      IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(action, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> action)
    {
      return action != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(action) : throw new ArgumentNullException(nameof (action));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> action,
      IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(action, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> action)
    {
      return action != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(action) : throw new ArgumentNullException(nameof (action));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> action,
      IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(action, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> action)
    {
      return action != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(action) : throw new ArgumentNullException(nameof (action));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> action,
      IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(action, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> action)
    {
      return action != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(action) : throw new ArgumentNullException(nameof (action));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> action,
      IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(action, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> action)
    {
      return action != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(action) : throw new ArgumentNullException(nameof (action));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> action,
      IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(action, scheduler);
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> action)
    {
      return action != null ? Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(action) : throw new ArgumentNullException(nameof (action));
    }

    public static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, IObservable<Unit>> ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(
      this Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16> action,
      IScheduler scheduler)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(action, scheduler);
    }

    public static AsyncSubject<TSource> GetAwaiter<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.GetAwaiter<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static AsyncSubject<TSource> GetAwaiter<TSource>(
      this IConnectableObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.GetAwaiter<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static AsyncSubject<TSource> RunAsync<TSource>(
      this IObservable<TSource> source,
      CancellationToken cancellationToken)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return Observable.s_impl.RunAsync<TSource>(source, cancellationToken);
    }

    public static AsyncSubject<TSource> RunAsync<TSource>(
      this IConnectableObservable<TSource> source,
      CancellationToken cancellationToken)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return Observable.s_impl.RunAsync<TSource>(source, cancellationToken);
    }

    public static IConnectableObservable<TResult> Multicast<TSource, TResult>(
      this IObservable<TSource> source,
      ISubject<TSource, TResult> subject)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (subject == null)
        throw new ArgumentNullException(nameof (subject));
      return Observable.s_impl.Multicast<TSource, TResult>(source, subject);
    }

    public static IObservable<TResult> Multicast<TSource, TIntermediate, TResult>(
      this IObservable<TSource> source,
      Func<ISubject<TSource, TIntermediate>> subjectSelector,
      Func<IObservable<TIntermediate>, IObservable<TResult>> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (subjectSelector == null)
        throw new ArgumentNullException(nameof (subjectSelector));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Multicast<TSource, TIntermediate, TResult>(source, subjectSelector, selector);
    }

    public static IConnectableObservable<TSource> Publish<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Publish<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TResult> Publish<TSource, TResult>(
      this IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Publish<TSource, TResult>(source, selector);
    }

    public static IConnectableObservable<TSource> Publish<TSource>(
      this IObservable<TSource> source,
      TSource initialValue)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return Observable.s_impl.Publish<TSource>(source, initialValue);
    }

    public static IObservable<TResult> Publish<TSource, TResult>(
      this IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      TSource initialValue)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Publish<TSource, TResult>(source, selector, initialValue);
    }

    public static IConnectableObservable<TSource> PublishLast<TSource>(
      this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.PublishLast<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TResult> PublishLast<TSource, TResult>(
      this IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.PublishLast<TSource, TResult>(source, selector);
    }

    public static IObservable<TSource> RefCount<TSource>(this IConnectableObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.RefCount<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IConnectableObservable<TSource> Replay<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Replay<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IConnectableObservable<TSource> Replay<TSource>(
      this IObservable<TSource> source,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Replay<TSource>(source, scheduler);
    }

    public static IObservable<TResult> Replay<TSource, TResult>(
      this IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Replay<TSource, TResult>(source, selector);
    }

    public static IObservable<TResult> Replay<TSource, TResult>(
      this IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Replay<TSource, TResult>(source, selector, scheduler);
    }

    public static IConnectableObservable<TSource> Replay<TSource>(
      this IObservable<TSource> source,
      TimeSpan window)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (window < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (window));
      return Observable.s_impl.Replay<TSource>(source, window);
    }

    public static IObservable<TResult> Replay<TSource, TResult>(
      this IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      TimeSpan window)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (window < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (window));
      return Observable.s_impl.Replay<TSource, TResult>(source, selector, window);
    }

    public static IConnectableObservable<TSource> Replay<TSource>(
      this IObservable<TSource> source,
      TimeSpan window,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (window < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (window));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Replay<TSource>(source, window, scheduler);
    }

    public static IObservable<TResult> Replay<TSource, TResult>(
      this IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      TimeSpan window,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (window < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (window));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Replay<TSource, TResult>(source, selector, window, scheduler);
    }

    public static IConnectableObservable<TSource> Replay<TSource>(
      this IObservable<TSource> source,
      int bufferSize,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (bufferSize < 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Replay<TSource>(source, bufferSize, scheduler);
    }

    public static IObservable<TResult> Replay<TSource, TResult>(
      this IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      int bufferSize,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (bufferSize < 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Replay<TSource, TResult>(source, selector, bufferSize, scheduler);
    }

    public static IConnectableObservable<TSource> Replay<TSource>(
      this IObservable<TSource> source,
      int bufferSize)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (bufferSize < 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize));
      return Observable.s_impl.Replay<TSource>(source, bufferSize);
    }

    public static IObservable<TResult> Replay<TSource, TResult>(
      this IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      int bufferSize)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (bufferSize < 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize));
      return Observable.s_impl.Replay<TSource, TResult>(source, selector, bufferSize);
    }

    public static IConnectableObservable<TSource> Replay<TSource>(
      this IObservable<TSource> source,
      int bufferSize,
      TimeSpan window)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (bufferSize < 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize));
      if (window < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (window));
      return Observable.s_impl.Replay<TSource>(source, bufferSize, window);
    }

    public static IObservable<TResult> Replay<TSource, TResult>(
      this IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      int bufferSize,
      TimeSpan window)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (bufferSize < 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize));
      if (window < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (window));
      return Observable.s_impl.Replay<TSource, TResult>(source, selector, bufferSize, window);
    }

    public static IConnectableObservable<TSource> Replay<TSource>(
      this IObservable<TSource> source,
      int bufferSize,
      TimeSpan window,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (bufferSize < 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize));
      if (window < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (window));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Replay<TSource>(source, bufferSize, window, scheduler);
    }

    public static IObservable<TResult> Replay<TSource, TResult>(
      this IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      int bufferSize,
      TimeSpan window,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (bufferSize < 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize));
      if (window < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (window));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Replay<TSource, TResult>(source, selector, bufferSize, window, scheduler);
    }

    public static IEnumerable<IList<TSource>> Chunkify<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Chunkify<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IEnumerable<TResult> Collect<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TResult> newCollector,
      Func<TResult, TSource, TResult> merge)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (newCollector == null)
        throw new ArgumentNullException(nameof (newCollector));
      if (merge == null)
        throw new ArgumentNullException(nameof (merge));
      return Observable.s_impl.Collect<TSource, TResult>(source, newCollector, merge);
    }

    public static IEnumerable<TResult> Collect<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TResult> getInitialCollector,
      Func<TResult, TSource, TResult> merge,
      Func<TResult, TResult> getNewCollector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (getInitialCollector == null)
        throw new ArgumentNullException(nameof (getInitialCollector));
      if (merge == null)
        throw new ArgumentNullException(nameof (merge));
      if (getNewCollector == null)
        throw new ArgumentNullException(nameof (getNewCollector));
      return Observable.s_impl.Collect<TSource, TResult>(source, getInitialCollector, merge, getNewCollector);
    }

    [Obsolete("This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
    public static TSource First<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.First<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    [Obsolete("This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
    public static TSource First<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.First<TSource>(source, predicate);
    }

    [Obsolete("This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
    public static TSource FirstOrDefault<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.FirstOrDefault<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    [Obsolete("This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
    public static TSource FirstOrDefault<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.FirstOrDefault<TSource>(source, predicate);
    }

    [Obsolete("This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
    public static void ForEach<TSource>(this IObservable<TSource> source, Action<TSource> onNext)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      Observable.s_impl.ForEach<TSource>(source, onNext);
    }

    [Obsolete("This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
    public static void ForEach<TSource>(
      this IObservable<TSource> source,
      Action<TSource, int> onNext)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      Observable.s_impl.ForEach<TSource>(source, onNext);
    }

    public static IEnumerator<TSource> GetEnumerator<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.GetEnumerator<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    [Obsolete("This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
    public static TSource Last<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Last<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    [Obsolete("This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
    public static TSource Last<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.Last<TSource>(source, predicate);
    }

    [Obsolete("This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
    public static TSource LastOrDefault<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.LastOrDefault<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    [Obsolete("This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
    public static TSource LastOrDefault<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.LastOrDefault<TSource>(source, predicate);
    }

    public static IEnumerable<TSource> Latest<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Latest<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IEnumerable<TSource> MostRecent<TSource>(
      this IObservable<TSource> source,
      TSource initialValue)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return Observable.s_impl.MostRecent<TSource>(source, initialValue);
    }

    public static IEnumerable<TSource> Next<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Next<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    [Obsolete("This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
    public static TSource Single<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Single<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    [Obsolete("This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
    public static TSource Single<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.Single<TSource>(source, predicate);
    }

    [Obsolete("This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
    public static TSource SingleOrDefault<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.SingleOrDefault<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    [Obsolete("This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.")]
    public static TSource SingleOrDefault<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.SingleOrDefault<TSource>(source, predicate);
    }

    public static TSource Wait<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Wait<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> ObserveOn<TSource>(
      this IObservable<TSource> source,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ObserveOn<TSource>(source, scheduler);
    }

    public static IObservable<TSource> ObserveOn<TSource>(
      this IObservable<TSource> source,
      SynchronizationContext context)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      return Observable.s_impl.ObserveOn<TSource>(source, context);
    }

    public static IObservable<TSource> SubscribeOn<TSource>(
      this IObservable<TSource> source,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.SubscribeOn<TSource>(source, scheduler);
    }

    public static IObservable<TSource> SubscribeOn<TSource>(
      this IObservable<TSource> source,
      SynchronizationContext context)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      return Observable.s_impl.SubscribeOn<TSource>(source, context);
    }

    public static IObservable<TSource> Synchronize<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Synchronize<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> Synchronize<TSource>(
      this IObservable<TSource> source,
      object gate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (gate == null)
        throw new ArgumentNullException(nameof (gate));
      return Observable.s_impl.Synchronize<TSource>(source, gate);
    }

    public static IDisposable Subscribe<TSource>(
      this IEnumerable<TSource> source,
      IObserver<TSource> observer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      return Observable.s_impl.Subscribe<TSource>(source, observer);
    }

    public static IDisposable Subscribe<TSource>(
      this IEnumerable<TSource> source,
      IObserver<TSource> observer,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Subscribe<TSource>(source, observer, scheduler);
    }

    public static IEnumerable<TSource> ToEnumerable<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.ToEnumerable<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IEventSource<Unit> ToEvent(this IObservable<Unit> source)
    {
      return source != null ? Observable.s_impl.ToEvent(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IEventSource<TSource> ToEvent<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.ToEvent<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IEventPatternSource<TEventArgs> ToEventPattern<TEventArgs>(
      this IObservable<EventPattern<TEventArgs>> source)
    {
      return source != null ? Observable.s_impl.ToEventPattern<TEventArgs>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> ToObservable<TSource>(this IEnumerable<TSource> source)
    {
      return source != null ? Observable.s_impl.ToObservable<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> ToObservable<TSource>(
      this IEnumerable<TSource> source,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.ToObservable<TSource>(source, scheduler);
    }

    public static IObservable<TResult> Create<TResult>(
      Func<IObserver<TResult>, IDisposable> subscribe)
    {
      return subscribe != null ? Observable.s_impl.Create<TResult>(subscribe) : throw new ArgumentNullException(nameof (subscribe));
    }

    public static IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, Action> subscribe)
    {
      return subscribe != null ? Observable.s_impl.Create<TResult>(subscribe) : throw new ArgumentNullException(nameof (subscribe));
    }

    public static IObservable<TResult> Create<TResult>(
      Func<IObserver<TResult>, CancellationToken, Task> subscribeAsync)
    {
      return subscribeAsync != null ? Observable.s_impl.Create<TResult>(subscribeAsync) : throw new ArgumentNullException(nameof (subscribeAsync));
    }

    public static IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, Task> subscribeAsync)
    {
      return subscribeAsync != null ? Observable.s_impl.Create<TResult>(subscribeAsync) : throw new ArgumentNullException(nameof (subscribeAsync));
    }

    public static IObservable<TResult> Create<TResult>(
      Func<IObserver<TResult>, CancellationToken, Task<IDisposable>> subscribeAsync)
    {
      return subscribeAsync != null ? Observable.s_impl.Create<TResult>(subscribeAsync) : throw new ArgumentNullException(nameof (subscribeAsync));
    }

    public static IObservable<TResult> Create<TResult>(
      Func<IObserver<TResult>, Task<IDisposable>> subscribeAsync)
    {
      return subscribeAsync != null ? Observable.s_impl.Create<TResult>(subscribeAsync) : throw new ArgumentNullException(nameof (subscribeAsync));
    }

    public static IObservable<TResult> Create<TResult>(
      Func<IObserver<TResult>, CancellationToken, Task<Action>> subscribeAsync)
    {
      return subscribeAsync != null ? Observable.s_impl.Create<TResult>(subscribeAsync) : throw new ArgumentNullException(nameof (subscribeAsync));
    }

    public static IObservable<TResult> Create<TResult>(
      Func<IObserver<TResult>, Task<Action>> subscribeAsync)
    {
      return subscribeAsync != null ? Observable.s_impl.Create<TResult>(subscribeAsync) : throw new ArgumentNullException(nameof (subscribeAsync));
    }

    public static IObservable<TResult> Defer<TResult>(Func<IObservable<TResult>> observableFactory)
    {
      return observableFactory != null ? Observable.s_impl.Defer<TResult>(observableFactory) : throw new ArgumentNullException(nameof (observableFactory));
    }

    public static IObservable<TResult> Defer<TResult>(
      Func<Task<IObservable<TResult>>> observableFactoryAsync)
    {
      return observableFactoryAsync != null ? Observable.s_impl.Defer<TResult>(observableFactoryAsync) : throw new ArgumentNullException(nameof (observableFactoryAsync));
    }

    public static IObservable<TResult> DeferAsync<TResult>(
      Func<CancellationToken, Task<IObservable<TResult>>> observableFactoryAsync)
    {
      return observableFactoryAsync != null ? Observable.s_impl.Defer<TResult>(observableFactoryAsync) : throw new ArgumentNullException(nameof (observableFactoryAsync));
    }

    public static IObservable<TResult> Empty<TResult>() => Observable.s_impl.Empty<TResult>();

    public static IObservable<TResult> Empty<TResult>(TResult witness)
    {
      return Observable.s_impl.Empty<TResult>();
    }

    public static IObservable<TResult> Empty<TResult>(IScheduler scheduler)
    {
      return scheduler != null ? Observable.s_impl.Empty<TResult>(scheduler) : throw new ArgumentNullException(nameof (scheduler));
    }

    public static IObservable<TResult> Empty<TResult>(IScheduler scheduler, TResult witness)
    {
      return scheduler != null ? Observable.s_impl.Empty<TResult>(scheduler) : throw new ArgumentNullException(nameof (scheduler));
    }

    public static IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      if (iterate == null)
        throw new ArgumentNullException(nameof (iterate));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Generate<TState, TResult>(initialState, condition, iterate, resultSelector);
    }

    public static IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      IScheduler scheduler)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      if (iterate == null)
        throw new ArgumentNullException(nameof (iterate));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Generate<TState, TResult>(initialState, condition, iterate, resultSelector, scheduler);
    }

    public static IObservable<TResult> Never<TResult>() => Observable.s_impl.Never<TResult>();

    public static IObservable<TResult> Never<TResult>(TResult witness)
    {
      return Observable.s_impl.Never<TResult>();
    }

    public static IObservable<int> Range(int start, int count)
    {
      long num = (long) start + (long) count - 1L;
      if (count < 0 || num > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (count));
      return Observable.s_impl.Range(start, count);
    }

    public static IObservable<int> Range(int start, int count, IScheduler scheduler)
    {
      long num = (long) start + (long) count - 1L;
      if (count < 0 || num > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Range(start, count, scheduler);
    }

    public static IObservable<TResult> Repeat<TResult>(TResult value)
    {
      return Observable.s_impl.Repeat<TResult>(value);
    }

    public static IObservable<TResult> Repeat<TResult>(TResult value, IScheduler scheduler)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Repeat<TResult>(value, scheduler);
    }

    public static IObservable<TResult> Repeat<TResult>(TResult value, int repeatCount)
    {
      if (repeatCount < 0)
        throw new ArgumentOutOfRangeException(nameof (repeatCount));
      return Observable.s_impl.Repeat<TResult>(value, repeatCount);
    }

    public static IObservable<TResult> Repeat<TResult>(
      TResult value,
      int repeatCount,
      IScheduler scheduler)
    {
      if (repeatCount < 0)
        throw new ArgumentOutOfRangeException(nameof (repeatCount));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Repeat<TResult>(value, repeatCount, scheduler);
    }

    public static IObservable<TResult> Return<TResult>(TResult value)
    {
      return Observable.s_impl.Return<TResult>(value);
    }

    public static IObservable<TResult> Return<TResult>(TResult value, IScheduler scheduler)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Return<TResult>(value, scheduler);
    }

    public static IObservable<TResult> Throw<TResult>(Exception exception)
    {
      return exception != null ? Observable.s_impl.Throw<TResult>(exception) : throw new ArgumentNullException(nameof (exception));
    }

    public static IObservable<TResult> Throw<TResult>(Exception exception, TResult witness)
    {
      return exception != null ? Observable.s_impl.Throw<TResult>(exception) : throw new ArgumentNullException(nameof (exception));
    }

    public static IObservable<TResult> Throw<TResult>(Exception exception, IScheduler scheduler)
    {
      if (exception == null)
        throw new ArgumentNullException(nameof (exception));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Throw<TResult>(exception, scheduler);
    }

    public static IObservable<TResult> Throw<TResult>(
      Exception exception,
      IScheduler scheduler,
      TResult witness)
    {
      if (exception == null)
        throw new ArgumentNullException(nameof (exception));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Throw<TResult>(exception, scheduler);
    }

    public static IObservable<TResult> Using<TResult, TResource>(
      Func<TResource> resourceFactory,
      Func<TResource, IObservable<TResult>> observableFactory)
      where TResource : IDisposable
    {
      if (resourceFactory == null)
        throw new ArgumentNullException(nameof (resourceFactory));
      if (observableFactory == null)
        throw new ArgumentNullException(nameof (observableFactory));
      return Observable.s_impl.Using<TResult, TResource>(resourceFactory, observableFactory);
    }

    public static IObservable<TResult> Using<TResult, TResource>(
      Func<CancellationToken, Task<TResource>> resourceFactoryAsync,
      Func<TResource, CancellationToken, Task<IObservable<TResult>>> observableFactoryAsync)
      where TResource : IDisposable
    {
      if (resourceFactoryAsync == null)
        throw new ArgumentNullException(nameof (resourceFactoryAsync));
      if (observableFactoryAsync == null)
        throw new ArgumentNullException(nameof (observableFactoryAsync));
      return Observable.s_impl.Using<TResult, TResource>(resourceFactoryAsync, observableFactoryAsync);
    }

    public static IObservable<EventPattern<object>> FromEventPattern(
      Action<EventHandler> addHandler,
      Action<EventHandler> removeHandler)
    {
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      return Observable.s_impl.FromEventPattern(addHandler, removeHandler);
    }

    public static IObservable<EventPattern<object>> FromEventPattern(
      Action<EventHandler> addHandler,
      Action<EventHandler> removeHandler,
      IScheduler scheduler)
    {
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromEventPattern(addHandler, removeHandler, scheduler);
    }

    public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler)
    {
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      return Observable.s_impl.FromEventPattern<TDelegate, TEventArgs>(addHandler, removeHandler);
    }

    public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromEventPattern<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);
    }

    public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(
      Func<EventHandler<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler)
    {
      if (conversion == null)
        throw new ArgumentNullException(nameof (conversion));
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      return Observable.s_impl.FromEventPattern<TDelegate, TEventArgs>(conversion, addHandler, removeHandler);
    }

    public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(
      Func<EventHandler<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      if (conversion == null)
        throw new ArgumentNullException(nameof (conversion));
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromEventPattern<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, scheduler);
    }

    public static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler)
    {
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      return Observable.s_impl.FromEventPattern<TDelegate, TSender, TEventArgs>(addHandler, removeHandler);
    }

    public static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromEventPattern<TDelegate, TSender, TEventArgs>(addHandler, removeHandler, scheduler);
    }

    public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      Action<EventHandler<TEventArgs>> addHandler,
      Action<EventHandler<TEventArgs>> removeHandler)
    {
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      return Observable.s_impl.FromEventPattern<TEventArgs>(addHandler, removeHandler);
    }

    public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      Action<EventHandler<TEventArgs>> addHandler,
      Action<EventHandler<TEventArgs>> removeHandler,
      IScheduler scheduler)
    {
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromEventPattern<TEventArgs>(addHandler, removeHandler, scheduler);
    }

    public static IObservable<EventPattern<object>> FromEventPattern(
      object target,
      string eventName)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (eventName == null)
        throw new ArgumentNullException(nameof (eventName));
      return Observable.s_impl.FromEventPattern(target, eventName);
    }

    public static IObservable<EventPattern<object>> FromEventPattern(
      object target,
      string eventName,
      IScheduler scheduler)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (eventName == null)
        throw new ArgumentNullException(nameof (eventName));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromEventPattern(target, eventName, scheduler);
    }

    public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      object target,
      string eventName)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (eventName == null)
        throw new ArgumentNullException(nameof (eventName));
      return Observable.s_impl.FromEventPattern<TEventArgs>(target, eventName);
    }

    public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      object target,
      string eventName,
      IScheduler scheduler)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (eventName == null)
        throw new ArgumentNullException(nameof (eventName));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromEventPattern<TEventArgs>(target, eventName, scheduler);
    }

    public static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(
      object target,
      string eventName)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (eventName == null)
        throw new ArgumentNullException(nameof (eventName));
      return Observable.s_impl.FromEventPattern<TSender, TEventArgs>(target, eventName);
    }

    public static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(
      object target,
      string eventName,
      IScheduler scheduler)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (eventName == null)
        throw new ArgumentNullException(nameof (eventName));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromEventPattern<TSender, TEventArgs>(target, eventName, scheduler);
    }

    public static IObservable<EventPattern<object>> FromEventPattern(Type type, string eventName)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (eventName == null)
        throw new ArgumentNullException(nameof (eventName));
      return Observable.s_impl.FromEventPattern(type, eventName);
    }

    public static IObservable<EventPattern<object>> FromEventPattern(
      Type type,
      string eventName,
      IScheduler scheduler)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (eventName == null)
        throw new ArgumentNullException(nameof (eventName));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromEventPattern(type, eventName, scheduler);
    }

    public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      Type type,
      string eventName)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (eventName == null)
        throw new ArgumentNullException(nameof (eventName));
      return Observable.s_impl.FromEventPattern<TEventArgs>(type, eventName);
    }

    public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      Type type,
      string eventName,
      IScheduler scheduler)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (eventName == null)
        throw new ArgumentNullException(nameof (eventName));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromEventPattern<TEventArgs>(type, eventName, scheduler);
    }

    public static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(
      Type type,
      string eventName)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (eventName == null)
        throw new ArgumentNullException(nameof (eventName));
      return Observable.s_impl.FromEventPattern<TSender, TEventArgs>(type, eventName);
    }

    public static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(
      Type type,
      string eventName,
      IScheduler scheduler)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (eventName == null)
        throw new ArgumentNullException(nameof (eventName));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromEventPattern<TSender, TEventArgs>(type, eventName, scheduler);
    }

    public static IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(
      Func<Action<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler)
    {
      if (conversion == null)
        throw new ArgumentNullException(nameof (conversion));
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      return Observable.s_impl.FromEvent<TDelegate, TEventArgs>(conversion, addHandler, removeHandler);
    }

    public static IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(
      Func<Action<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      if (conversion == null)
        throw new ArgumentNullException(nameof (conversion));
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromEvent<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, scheduler);
    }

    public static IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler)
    {
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      return Observable.s_impl.FromEvent<TDelegate, TEventArgs>(addHandler, removeHandler);
    }

    public static IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromEvent<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);
    }

    public static IObservable<TEventArgs> FromEvent<TEventArgs>(
      Action<Action<TEventArgs>> addHandler,
      Action<Action<TEventArgs>> removeHandler)
    {
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      return Observable.s_impl.FromEvent<TEventArgs>(addHandler, removeHandler);
    }

    public static IObservable<TEventArgs> FromEvent<TEventArgs>(
      Action<Action<TEventArgs>> addHandler,
      Action<Action<TEventArgs>> removeHandler,
      IScheduler scheduler)
    {
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromEvent<TEventArgs>(addHandler, removeHandler, scheduler);
    }

    public static IObservable<Unit> FromEvent(
      Action<Action> addHandler,
      Action<Action> removeHandler)
    {
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      return Observable.s_impl.FromEvent(addHandler, removeHandler);
    }

    public static IObservable<Unit> FromEvent(
      Action<Action> addHandler,
      Action<Action> removeHandler,
      IScheduler scheduler)
    {
      if (addHandler == null)
        throw new ArgumentNullException(nameof (addHandler));
      if (removeHandler == null)
        throw new ArgumentNullException(nameof (removeHandler));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.FromEvent(addHandler, removeHandler, scheduler);
    }

    public static Task ForEachAsync<TSource>(
      this IObservable<TSource> source,
      Action<TSource> onNext)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      return Observable.s_impl.ForEachAsync<TSource>(source, onNext);
    }

    public static Task ForEachAsync<TSource>(
      this IObservable<TSource> source,
      Action<TSource> onNext,
      CancellationToken cancellationToken)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      return Observable.s_impl.ForEachAsync<TSource>(source, onNext, cancellationToken);
    }

    public static Task ForEachAsync<TSource>(
      this IObservable<TSource> source,
      Action<TSource, int> onNext)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      return Observable.s_impl.ForEachAsync<TSource>(source, onNext);
    }

    public static Task ForEachAsync<TSource>(
      this IObservable<TSource> source,
      Action<TSource, int> onNext,
      CancellationToken cancellationToken)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      return Observable.s_impl.ForEachAsync<TSource>(source, onNext, cancellationToken);
    }

    public static IObservable<TResult> Case<TValue, TResult>(
      Func<TValue> selector,
      IDictionary<TValue, IObservable<TResult>> sources,
      IObservable<TResult> defaultSource)
    {
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (sources == null)
        throw new ArgumentNullException(nameof (sources));
      if (defaultSource == null)
        throw new ArgumentNullException(nameof (defaultSource));
      return Observable.s_impl.Case<TValue, TResult>(selector, sources, defaultSource);
    }

    public static IObservable<TResult> Case<TValue, TResult>(
      Func<TValue> selector,
      IDictionary<TValue, IObservable<TResult>> sources,
      IScheduler scheduler)
    {
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (sources == null)
        throw new ArgumentNullException(nameof (sources));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Case<TValue, TResult>(selector, sources, scheduler);
    }

    public static IObservable<TResult> Case<TValue, TResult>(
      Func<TValue> selector,
      IDictionary<TValue, IObservable<TResult>> sources)
    {
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (sources == null)
        throw new ArgumentNullException(nameof (sources));
      return Observable.s_impl.Case<TValue, TResult>(selector, sources);
    }

    public static IObservable<TSource> DoWhile<TSource>(
      this IObservable<TSource> source,
      Func<bool> condition)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      return Observable.s_impl.DoWhile<TSource>(source, condition);
    }

    public static IObservable<TResult> For<TSource, TResult>(
      IEnumerable<TSource> source,
      Func<TSource, IObservable<TResult>> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.For<TSource, TResult>(source, resultSelector);
    }

    public static IObservable<TResult> If<TResult>(
      Func<bool> condition,
      IObservable<TResult> thenSource,
      IObservable<TResult> elseSource)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      if (thenSource == null)
        throw new ArgumentNullException(nameof (thenSource));
      if (elseSource == null)
        throw new ArgumentNullException(nameof (elseSource));
      return Observable.s_impl.If<TResult>(condition, thenSource, elseSource);
    }

    public static IObservable<TResult> If<TResult>(
      Func<bool> condition,
      IObservable<TResult> thenSource)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      if (thenSource == null)
        throw new ArgumentNullException(nameof (thenSource));
      return Observable.s_impl.If<TResult>(condition, thenSource);
    }

    public static IObservable<TResult> If<TResult>(
      Func<bool> condition,
      IObservable<TResult> thenSource,
      IScheduler scheduler)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      if (thenSource == null)
        throw new ArgumentNullException(nameof (thenSource));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.If<TResult>(condition, thenSource, scheduler);
    }

    public static IObservable<TSource> While<TSource>(
      Func<bool> condition,
      IObservable<TSource> source)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return Observable.s_impl.While<TSource>(condition, source);
    }

    public static Pattern<TLeft, TRight> And<TLeft, TRight>(
      this IObservable<TLeft> left,
      IObservable<TRight> right)
    {
      if (left == null)
        throw new ArgumentNullException(nameof (left));
      if (right == null)
        throw new ArgumentNullException(nameof (right));
      return Observable.s_impl.And<TLeft, TRight>(left, right);
    }

    public static Plan<TResult> Then<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TSource, TResult> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Then<TSource, TResult>(source, selector);
    }

    public static IObservable<TResult> When<TResult>(params Plan<TResult>[] plans)
    {
      return plans != null ? Observable.s_impl.When<TResult>(plans) : throw new ArgumentNullException(nameof (plans));
    }

    public static IObservable<TResult> When<TResult>(this IEnumerable<Plan<TResult>> plans)
    {
      return plans != null ? Observable.s_impl.When<TResult>(plans) : throw new ArgumentNullException(nameof (plans));
    }

    public static IObservable<TSource> Amb<TSource>(
      this IObservable<TSource> first,
      IObservable<TSource> second)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      return Observable.s_impl.Amb<TSource>(first, second);
    }

    public static IObservable<TSource> Amb<TSource>(params IObservable<TSource>[] sources)
    {
      return sources != null ? Observable.s_impl.Amb<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<TSource> Amb<TSource>(this IEnumerable<IObservable<TSource>> sources)
    {
      return sources != null ? Observable.s_impl.Amb<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<IList<TSource>> Buffer<TSource, TBufferClosing>(
      this IObservable<TSource> source,
      Func<IObservable<TBufferClosing>> bufferClosingSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (bufferClosingSelector == null)
        throw new ArgumentNullException(nameof (bufferClosingSelector));
      return Observable.s_impl.Buffer<TSource, TBufferClosing>(source, bufferClosingSelector);
    }

    public static IObservable<IList<TSource>> Buffer<TSource, TBufferOpening, TBufferClosing>(
      this IObservable<TSource> source,
      IObservable<TBufferOpening> bufferOpenings,
      Func<TBufferOpening, IObservable<TBufferClosing>> bufferClosingSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (bufferOpenings == null)
        throw new ArgumentNullException(nameof (bufferOpenings));
      if (bufferClosingSelector == null)
        throw new ArgumentNullException(nameof (bufferClosingSelector));
      return Observable.s_impl.Buffer<TSource, TBufferOpening, TBufferClosing>(source, bufferOpenings, bufferClosingSelector);
    }

    public static IObservable<IList<TSource>> Buffer<TSource, TBufferBoundary>(
      this IObservable<TSource> source,
      IObservable<TBufferBoundary> bufferBoundaries)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (bufferBoundaries == null)
        throw new ArgumentNullException(nameof (bufferBoundaries));
      return Observable.s_impl.Buffer<TSource, TBufferBoundary>(source, bufferBoundaries);
    }

    public static IObservable<TSource> Catch<TSource, TException>(
      this IObservable<TSource> source,
      Func<TException, IObservable<TSource>> handler)
      where TException : Exception
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (handler == null)
        throw new ArgumentNullException(nameof (handler));
      return Observable.s_impl.Catch<TSource, TException>(source, handler);
    }

    public static IObservable<TSource> Catch<TSource>(
      this IObservable<TSource> first,
      IObservable<TSource> second)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      return Observable.s_impl.Catch<TSource>(first, second);
    }

    public static IObservable<TSource> Catch<TSource>(params IObservable<TSource>[] sources)
    {
      return sources != null ? Observable.s_impl.Catch<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<TSource> Catch<TSource>(this IEnumerable<IObservable<TSource>> sources)
    {
      return sources != null ? Observable.s_impl.Catch<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<TResult> CombineLatest<TSource1, TSource2, TResult>(
      this IObservable<TSource1> first,
      IObservable<TSource2> second,
      Func<TSource1, TSource2, TResult> resultSelector)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.CombineLatest<TSource1, TSource2, TResult>(first, second, resultSelector);
    }

    public static IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      Func<TSource1, TSource2, TSource3, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.CombineLatest<TSource1, TSource2, TSource3, TResult>(source1, source2, source3, resultSelector);
    }

    public static IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      Func<TSource1, TSource2, TSource3, TSource4, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(source1, source2, source3, source4, resultSelector);
    }

    public static IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(source1, source2, source3, source4, source5, resultSelector);
    }

    public static IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(source1, source2, source3, source4, source5, source6, resultSelector);
    }

    public static IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(source1, source2, source3, source4, source5, source6, source7, resultSelector);
    }

    public static IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, resultSelector);
    }

    public static IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      IObservable<TSource9> source9,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (source9 == null)
        throw new ArgumentNullException(nameof (source9));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, resultSelector);
    }

    public static IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      IObservable<TSource9> source9,
      IObservable<TSource10> source10,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (source9 == null)
        throw new ArgumentNullException(nameof (source9));
      if (source10 == null)
        throw new ArgumentNullException(nameof (source10));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, resultSelector);
    }

    public static IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      IObservable<TSource9> source9,
      IObservable<TSource10> source10,
      IObservable<TSource11> source11,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (source9 == null)
        throw new ArgumentNullException(nameof (source9));
      if (source10 == null)
        throw new ArgumentNullException(nameof (source10));
      if (source11 == null)
        throw new ArgumentNullException(nameof (source11));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, resultSelector);
    }

    public static IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      IObservable<TSource9> source9,
      IObservable<TSource10> source10,
      IObservable<TSource11> source11,
      IObservable<TSource12> source12,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (source9 == null)
        throw new ArgumentNullException(nameof (source9));
      if (source10 == null)
        throw new ArgumentNullException(nameof (source10));
      if (source11 == null)
        throw new ArgumentNullException(nameof (source11));
      if (source12 == null)
        throw new ArgumentNullException(nameof (source12));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, resultSelector);
    }

    public static IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      IObservable<TSource9> source9,
      IObservable<TSource10> source10,
      IObservable<TSource11> source11,
      IObservable<TSource12> source12,
      IObservable<TSource13> source13,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (source9 == null)
        throw new ArgumentNullException(nameof (source9));
      if (source10 == null)
        throw new ArgumentNullException(nameof (source10));
      if (source11 == null)
        throw new ArgumentNullException(nameof (source11));
      if (source12 == null)
        throw new ArgumentNullException(nameof (source12));
      if (source13 == null)
        throw new ArgumentNullException(nameof (source13));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, resultSelector);
    }

    public static IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      IObservable<TSource9> source9,
      IObservable<TSource10> source10,
      IObservable<TSource11> source11,
      IObservable<TSource12> source12,
      IObservable<TSource13> source13,
      IObservable<TSource14> source14,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (source9 == null)
        throw new ArgumentNullException(nameof (source9));
      if (source10 == null)
        throw new ArgumentNullException(nameof (source10));
      if (source11 == null)
        throw new ArgumentNullException(nameof (source11));
      if (source12 == null)
        throw new ArgumentNullException(nameof (source12));
      if (source13 == null)
        throw new ArgumentNullException(nameof (source13));
      if (source14 == null)
        throw new ArgumentNullException(nameof (source14));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, resultSelector);
    }

    public static IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      IObservable<TSource9> source9,
      IObservable<TSource10> source10,
      IObservable<TSource11> source11,
      IObservable<TSource12> source12,
      IObservable<TSource13> source13,
      IObservable<TSource14> source14,
      IObservable<TSource15> source15,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (source9 == null)
        throw new ArgumentNullException(nameof (source9));
      if (source10 == null)
        throw new ArgumentNullException(nameof (source10));
      if (source11 == null)
        throw new ArgumentNullException(nameof (source11));
      if (source12 == null)
        throw new ArgumentNullException(nameof (source12));
      if (source13 == null)
        throw new ArgumentNullException(nameof (source13));
      if (source14 == null)
        throw new ArgumentNullException(nameof (source14));
      if (source15 == null)
        throw new ArgumentNullException(nameof (source15));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, resultSelector);
    }

    public static IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      IObservable<TSource9> source9,
      IObservable<TSource10> source10,
      IObservable<TSource11> source11,
      IObservable<TSource12> source12,
      IObservable<TSource13> source13,
      IObservable<TSource14> source14,
      IObservable<TSource15> source15,
      IObservable<TSource16> source16,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (source9 == null)
        throw new ArgumentNullException(nameof (source9));
      if (source10 == null)
        throw new ArgumentNullException(nameof (source10));
      if (source11 == null)
        throw new ArgumentNullException(nameof (source11));
      if (source12 == null)
        throw new ArgumentNullException(nameof (source12));
      if (source13 == null)
        throw new ArgumentNullException(nameof (source13));
      if (source14 == null)
        throw new ArgumentNullException(nameof (source14));
      if (source15 == null)
        throw new ArgumentNullException(nameof (source15));
      if (source16 == null)
        throw new ArgumentNullException(nameof (source16));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, source16, resultSelector);
    }

    public static IObservable<TResult> CombineLatest<TSource, TResult>(
      this IEnumerable<IObservable<TSource>> sources,
      Func<IList<TSource>, TResult> resultSelector)
    {
      if (sources == null)
        throw new ArgumentNullException(nameof (sources));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.CombineLatest<TSource, TResult>(sources, resultSelector);
    }

    public static IObservable<IList<TSource>> CombineLatest<TSource>(
      this IEnumerable<IObservable<TSource>> sources)
    {
      return sources != null ? Observable.s_impl.CombineLatest<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<IList<TSource>> CombineLatest<TSource>(
      params IObservable<TSource>[] sources)
    {
      return sources != null ? Observable.s_impl.CombineLatest<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<TSource> Concat<TSource>(
      this IObservable<TSource> first,
      IObservable<TSource> second)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      return Observable.s_impl.Concat<TSource>(first, second);
    }

    public static IObservable<TSource> Concat<TSource>(params IObservable<TSource>[] sources)
    {
      return sources != null ? Observable.s_impl.Concat<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<TSource> Concat<TSource>(
      this IEnumerable<IObservable<TSource>> sources)
    {
      return sources != null ? Observable.s_impl.Concat<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<TSource> Concat<TSource>(
      this IObservable<IObservable<TSource>> sources)
    {
      return sources != null ? Observable.s_impl.Concat<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<TSource> Concat<TSource>(this IObservable<Task<TSource>> sources)
    {
      return sources != null ? Observable.s_impl.Concat<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<TSource> Merge<TSource>(this IObservable<IObservable<TSource>> sources)
    {
      return sources != null ? Observable.s_impl.Merge<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<TSource> Merge<TSource>(this IObservable<Task<TSource>> sources)
    {
      return sources != null ? Observable.s_impl.Merge<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<TSource> Merge<TSource>(
      this IObservable<IObservable<TSource>> sources,
      int maxConcurrent)
    {
      if (sources == null)
        throw new ArgumentNullException(nameof (sources));
      if (maxConcurrent <= 0)
        throw new ArgumentOutOfRangeException(nameof (maxConcurrent));
      return Observable.s_impl.Merge<TSource>(sources, maxConcurrent);
    }

    public static IObservable<TSource> Merge<TSource>(
      this IEnumerable<IObservable<TSource>> sources,
      int maxConcurrent)
    {
      if (sources == null)
        throw new ArgumentNullException(nameof (sources));
      if (maxConcurrent <= 0)
        throw new ArgumentOutOfRangeException(nameof (maxConcurrent));
      return Observable.s_impl.Merge<TSource>(sources, maxConcurrent);
    }

    public static IObservable<TSource> Merge<TSource>(
      this IEnumerable<IObservable<TSource>> sources,
      int maxConcurrent,
      IScheduler scheduler)
    {
      if (sources == null)
        throw new ArgumentNullException(nameof (sources));
      if (maxConcurrent <= 0)
        throw new ArgumentOutOfRangeException(nameof (maxConcurrent));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Merge<TSource>(sources, maxConcurrent, scheduler);
    }

    public static IObservable<TSource> Merge<TSource>(
      this IObservable<TSource> first,
      IObservable<TSource> second)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      return Observable.s_impl.Merge<TSource>(first, second);
    }

    public static IObservable<TSource> Merge<TSource>(
      this IObservable<TSource> first,
      IObservable<TSource> second,
      IScheduler scheduler)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Merge<TSource>(first, second, scheduler);
    }

    public static IObservable<TSource> Merge<TSource>(params IObservable<TSource>[] sources)
    {
      return sources != null ? Observable.s_impl.Merge<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<TSource> Merge<TSource>(
      IScheduler scheduler,
      params IObservable<TSource>[] sources)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (sources == null)
        throw new ArgumentNullException(nameof (sources));
      return Observable.s_impl.Merge<TSource>(scheduler, sources);
    }

    public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources)
    {
      return sources != null ? Observable.s_impl.Merge<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<TSource> Merge<TSource>(
      this IEnumerable<IObservable<TSource>> sources,
      IScheduler scheduler)
    {
      if (sources == null)
        throw new ArgumentNullException(nameof (sources));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Merge<TSource>(sources, scheduler);
    }

    public static IObservable<TSource> OnErrorResumeNext<TSource>(
      this IObservable<TSource> first,
      IObservable<TSource> second)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      return Observable.s_impl.OnErrorResumeNext<TSource>(first, second);
    }

    public static IObservable<TSource> OnErrorResumeNext<TSource>(
      params IObservable<TSource>[] sources)
    {
      return sources != null ? Observable.s_impl.OnErrorResumeNext<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<TSource> OnErrorResumeNext<TSource>(
      this IEnumerable<IObservable<TSource>> sources)
    {
      return sources != null ? Observable.s_impl.OnErrorResumeNext<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<TSource> SkipUntil<TSource, TOther>(
      this IObservable<TSource> source,
      IObservable<TOther> other)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      return Observable.s_impl.SkipUntil<TSource, TOther>(source, other);
    }

    public static IObservable<TSource> Switch<TSource>(
      this IObservable<IObservable<TSource>> sources)
    {
      return sources != null ? Observable.s_impl.Switch<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<TSource> Switch<TSource>(this IObservable<Task<TSource>> sources)
    {
      return sources != null ? Observable.s_impl.Switch<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<TSource> TakeUntil<TSource, TOther>(
      this IObservable<TSource> source,
      IObservable<TOther> other)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      return Observable.s_impl.TakeUntil<TSource, TOther>(source, other);
    }

    public static IObservable<IObservable<TSource>> Window<TSource, TWindowClosing>(
      this IObservable<TSource> source,
      Func<IObservable<TWindowClosing>> windowClosingSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (windowClosingSelector == null)
        throw new ArgumentNullException(nameof (windowClosingSelector));
      return Observable.s_impl.Window<TSource, TWindowClosing>(source, windowClosingSelector);
    }

    public static IObservable<IObservable<TSource>> Window<TSource, TWindowOpening, TWindowClosing>(
      this IObservable<TSource> source,
      IObservable<TWindowOpening> windowOpenings,
      Func<TWindowOpening, IObservable<TWindowClosing>> windowClosingSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (windowOpenings == null)
        throw new ArgumentNullException(nameof (windowOpenings));
      if (windowClosingSelector == null)
        throw new ArgumentNullException(nameof (windowClosingSelector));
      return Observable.s_impl.Window<TSource, TWindowOpening, TWindowClosing>(source, windowOpenings, windowClosingSelector);
    }

    public static IObservable<IObservable<TSource>> Window<TSource, TWindowBoundary>(
      this IObservable<TSource> source,
      IObservable<TWindowBoundary> windowBoundaries)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (windowBoundaries == null)
        throw new ArgumentNullException(nameof (windowBoundaries));
      return Observable.s_impl.Window<TSource, TWindowBoundary>(source, windowBoundaries);
    }

    public static IObservable<TResult> WithLatestFrom<TFirst, TSecond, TResult>(
      this IObservable<TFirst> first,
      IObservable<TSecond> second,
      Func<TFirst, TSecond, TResult> resultSelector)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.WithLatestFrom<TFirst, TSecond, TResult>(first, second, resultSelector);
    }

    public static IObservable<TResult> Zip<TSource1, TSource2, TResult>(
      this IObservable<TSource1> first,
      IObservable<TSource2> second,
      Func<TSource1, TSource2, TResult> resultSelector)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Zip<TSource1, TSource2, TResult>(first, second, resultSelector);
    }

    public static IObservable<TResult> Zip<TSource1, TSource2, TSource3, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      Func<TSource1, TSource2, TSource3, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Zip<TSource1, TSource2, TSource3, TResult>(source1, source2, source3, resultSelector);
    }

    public static IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      Func<TSource1, TSource2, TSource3, TSource4, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Zip<TSource1, TSource2, TSource3, TSource4, TResult>(source1, source2, source3, source4, resultSelector);
    }

    public static IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(source1, source2, source3, source4, source5, resultSelector);
    }

    public static IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(source1, source2, source3, source4, source5, source6, resultSelector);
    }

    public static IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(source1, source2, source3, source4, source5, source6, source7, resultSelector);
    }

    public static IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, resultSelector);
    }

    public static IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      IObservable<TSource9> source9,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (source9 == null)
        throw new ArgumentNullException(nameof (source9));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, resultSelector);
    }

    public static IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      IObservable<TSource9> source9,
      IObservable<TSource10> source10,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (source9 == null)
        throw new ArgumentNullException(nameof (source9));
      if (source10 == null)
        throw new ArgumentNullException(nameof (source10));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, resultSelector);
    }

    public static IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      IObservable<TSource9> source9,
      IObservable<TSource10> source10,
      IObservable<TSource11> source11,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (source9 == null)
        throw new ArgumentNullException(nameof (source9));
      if (source10 == null)
        throw new ArgumentNullException(nameof (source10));
      if (source11 == null)
        throw new ArgumentNullException(nameof (source11));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, resultSelector);
    }

    public static IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      IObservable<TSource9> source9,
      IObservable<TSource10> source10,
      IObservable<TSource11> source11,
      IObservable<TSource12> source12,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (source9 == null)
        throw new ArgumentNullException(nameof (source9));
      if (source10 == null)
        throw new ArgumentNullException(nameof (source10));
      if (source11 == null)
        throw new ArgumentNullException(nameof (source11));
      if (source12 == null)
        throw new ArgumentNullException(nameof (source12));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, resultSelector);
    }

    public static IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      IObservable<TSource9> source9,
      IObservable<TSource10> source10,
      IObservable<TSource11> source11,
      IObservable<TSource12> source12,
      IObservable<TSource13> source13,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (source9 == null)
        throw new ArgumentNullException(nameof (source9));
      if (source10 == null)
        throw new ArgumentNullException(nameof (source10));
      if (source11 == null)
        throw new ArgumentNullException(nameof (source11));
      if (source12 == null)
        throw new ArgumentNullException(nameof (source12));
      if (source13 == null)
        throw new ArgumentNullException(nameof (source13));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, resultSelector);
    }

    public static IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      IObservable<TSource9> source9,
      IObservable<TSource10> source10,
      IObservable<TSource11> source11,
      IObservable<TSource12> source12,
      IObservable<TSource13> source13,
      IObservable<TSource14> source14,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (source9 == null)
        throw new ArgumentNullException(nameof (source9));
      if (source10 == null)
        throw new ArgumentNullException(nameof (source10));
      if (source11 == null)
        throw new ArgumentNullException(nameof (source11));
      if (source12 == null)
        throw new ArgumentNullException(nameof (source12));
      if (source13 == null)
        throw new ArgumentNullException(nameof (source13));
      if (source14 == null)
        throw new ArgumentNullException(nameof (source14));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, resultSelector);
    }

    public static IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      IObservable<TSource9> source9,
      IObservable<TSource10> source10,
      IObservable<TSource11> source11,
      IObservable<TSource12> source12,
      IObservable<TSource13> source13,
      IObservable<TSource14> source14,
      IObservable<TSource15> source15,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (source9 == null)
        throw new ArgumentNullException(nameof (source9));
      if (source10 == null)
        throw new ArgumentNullException(nameof (source10));
      if (source11 == null)
        throw new ArgumentNullException(nameof (source11));
      if (source12 == null)
        throw new ArgumentNullException(nameof (source12));
      if (source13 == null)
        throw new ArgumentNullException(nameof (source13));
      if (source14 == null)
        throw new ArgumentNullException(nameof (source14));
      if (source15 == null)
        throw new ArgumentNullException(nameof (source15));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, resultSelector);
    }

    public static IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(
      this IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      IObservable<TSource9> source9,
      IObservable<TSource10> source10,
      IObservable<TSource11> source11,
      IObservable<TSource12> source12,
      IObservable<TSource13> source13,
      IObservable<TSource14> source14,
      IObservable<TSource15> source15,
      IObservable<TSource16> source16,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> resultSelector)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
      if (source3 == null)
        throw new ArgumentNullException(nameof (source3));
      if (source4 == null)
        throw new ArgumentNullException(nameof (source4));
      if (source5 == null)
        throw new ArgumentNullException(nameof (source5));
      if (source6 == null)
        throw new ArgumentNullException(nameof (source6));
      if (source7 == null)
        throw new ArgumentNullException(nameof (source7));
      if (source8 == null)
        throw new ArgumentNullException(nameof (source8));
      if (source9 == null)
        throw new ArgumentNullException(nameof (source9));
      if (source10 == null)
        throw new ArgumentNullException(nameof (source10));
      if (source11 == null)
        throw new ArgumentNullException(nameof (source11));
      if (source12 == null)
        throw new ArgumentNullException(nameof (source12));
      if (source13 == null)
        throw new ArgumentNullException(nameof (source13));
      if (source14 == null)
        throw new ArgumentNullException(nameof (source14));
      if (source15 == null)
        throw new ArgumentNullException(nameof (source15));
      if (source16 == null)
        throw new ArgumentNullException(nameof (source16));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, source16, resultSelector);
    }

    public static IObservable<TResult> Zip<TSource, TResult>(
      this IEnumerable<IObservable<TSource>> sources,
      Func<IList<TSource>, TResult> resultSelector)
    {
      if (sources == null)
        throw new ArgumentNullException(nameof (sources));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Zip<TSource, TResult>(sources, resultSelector);
    }

    public static IObservable<IList<TSource>> Zip<TSource>(
      this IEnumerable<IObservable<TSource>> sources)
    {
      return sources != null ? Observable.s_impl.Zip<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<IList<TSource>> Zip<TSource>(params IObservable<TSource>[] sources)
    {
      return sources != null ? Observable.s_impl.Zip<TSource>(sources) : throw new ArgumentNullException(nameof (sources));
    }

    public static IObservable<TResult> Zip<TSource1, TSource2, TResult>(
      this IObservable<TSource1> first,
      IEnumerable<TSource2> second,
      Func<TSource1, TSource2, TResult> resultSelector)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Zip<TSource1, TSource2, TResult>(first, second, resultSelector);
    }

    public static IObservable<TSource> AsObservable<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.AsObservable<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<IList<TSource>> Buffer<TSource>(
      this IObservable<TSource> source,
      int count)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (count <= 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      return Observable.s_impl.Buffer<TSource>(source, count);
    }

    public static IObservable<IList<TSource>> Buffer<TSource>(
      this IObservable<TSource> source,
      int count,
      int skip)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (count <= 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (skip <= 0)
        throw new ArgumentOutOfRangeException(nameof (skip));
      return Observable.s_impl.Buffer<TSource>(source, count, skip);
    }

    public static IObservable<TSource> Dematerialize<TSource>(
      this IObservable<Notification<TSource>> source)
    {
      return source != null ? Observable.s_impl.Dematerialize<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> DistinctUntilChanged<TSource>(
      this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.DistinctUntilChanged<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> DistinctUntilChanged<TSource>(
      this IObservable<TSource> source,
      IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.DistinctUntilChanged<TSource>(source, comparer);
    }

    public static IObservable<TSource> DistinctUntilChanged<TSource, TKey>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      return Observable.s_impl.DistinctUntilChanged<TSource, TKey>(source, keySelector);
    }

    public static IObservable<TSource> DistinctUntilChanged<TSource, TKey>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.DistinctUntilChanged<TSource, TKey>(source, keySelector, comparer);
    }

    public static IObservable<TSource> Do<TSource>(
      this IObservable<TSource> source,
      Action<TSource> onNext)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      return Observable.s_impl.Do<TSource>(source, onNext);
    }

    public static IObservable<TSource> Do<TSource>(
      this IObservable<TSource> source,
      Action<TSource> onNext,
      Action onCompleted)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      if (onCompleted == null)
        throw new ArgumentNullException(nameof (onCompleted));
      return Observable.s_impl.Do<TSource>(source, onNext, onCompleted);
    }

    public static IObservable<TSource> Do<TSource>(
      this IObservable<TSource> source,
      Action<TSource> onNext,
      Action<Exception> onError)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      if (onError == null)
        throw new ArgumentNullException(nameof (onError));
      return Observable.s_impl.Do<TSource>(source, onNext, onError);
    }

    public static IObservable<TSource> Do<TSource>(
      this IObservable<TSource> source,
      Action<TSource> onNext,
      Action<Exception> onError,
      Action onCompleted)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      if (onError == null)
        throw new ArgumentNullException(nameof (onError));
      if (onCompleted == null)
        throw new ArgumentNullException(nameof (onCompleted));
      return Observable.s_impl.Do<TSource>(source, onNext, onError, onCompleted);
    }

    public static IObservable<TSource> Do<TSource>(
      this IObservable<TSource> source,
      IObserver<TSource> observer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      return Observable.s_impl.Do<TSource>(source, observer);
    }

    public static IObservable<TSource> Finally<TSource>(
      this IObservable<TSource> source,
      Action finallyAction)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (finallyAction == null)
        throw new ArgumentNullException(nameof (finallyAction));
      return Observable.s_impl.Finally<TSource>(source, finallyAction);
    }

    public static IObservable<TSource> IgnoreElements<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.IgnoreElements<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<Notification<TSource>> Materialize<TSource>(
      this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Materialize<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> Repeat<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Repeat<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> Repeat<TSource>(
      this IObservable<TSource> source,
      int repeatCount)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (repeatCount < 0)
        throw new ArgumentOutOfRangeException(nameof (repeatCount));
      return Observable.s_impl.Repeat<TSource>(source, repeatCount);
    }

    public static IObservable<TSource> Retry<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Retry<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> Retry<TSource>(
      this IObservable<TSource> source,
      int retryCount)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (retryCount < 0)
        throw new ArgumentOutOfRangeException(nameof (retryCount));
      return Observable.s_impl.Retry<TSource>(source, retryCount);
    }

    public static IObservable<TAccumulate> Scan<TSource, TAccumulate>(
      this IObservable<TSource> source,
      TAccumulate seed,
      Func<TAccumulate, TSource, TAccumulate> accumulator)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (accumulator == null)
        throw new ArgumentNullException(nameof (accumulator));
      return Observable.s_impl.Scan<TSource, TAccumulate>(source, seed, accumulator);
    }

    public static IObservable<TSource> Scan<TSource>(
      this IObservable<TSource> source,
      Func<TSource, TSource, TSource> accumulator)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (accumulator == null)
        throw new ArgumentNullException(nameof (accumulator));
      return Observable.s_impl.Scan<TSource>(source, accumulator);
    }

    public static IObservable<TSource> SkipLast<TSource>(
      this IObservable<TSource> source,
      int count)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      return Observable.s_impl.SkipLast<TSource>(source, count);
    }

    public static IObservable<TSource> StartWith<TSource>(
      this IObservable<TSource> source,
      params TSource[] values)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      return Observable.s_impl.StartWith<TSource>(source, values);
    }

    public static IObservable<TSource> StartWith<TSource>(
      this IObservable<TSource> source,
      IEnumerable<TSource> values)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      return Observable.s_impl.StartWith<TSource>(source, values);
    }

    public static IObservable<TSource> StartWith<TSource>(
      this IObservable<TSource> source,
      IScheduler scheduler,
      params TSource[] values)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      return Observable.s_impl.StartWith<TSource>(source, scheduler, values);
    }

    public static IObservable<TSource> StartWith<TSource>(
      this IObservable<TSource> source,
      IScheduler scheduler,
      IEnumerable<TSource> values)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      return Observable.s_impl.StartWith<TSource>(source, scheduler, values);
    }

    public static IObservable<TSource> TakeLast<TSource>(
      this IObservable<TSource> source,
      int count)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      return Observable.s_impl.TakeLast<TSource>(source, count);
    }

    public static IObservable<TSource> TakeLast<TSource>(
      this IObservable<TSource> source,
      int count,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.TakeLast<TSource>(source, count, scheduler);
    }

    public static IObservable<IList<TSource>> TakeLastBuffer<TSource>(
      this IObservable<TSource> source,
      int count)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      return Observable.s_impl.TakeLastBuffer<TSource>(source, count);
    }

    public static IObservable<IObservable<TSource>> Window<TSource>(
      this IObservable<TSource> source,
      int count)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (count <= 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      return Observable.s_impl.Window<TSource>(source, count);
    }

    public static IObservable<IObservable<TSource>> Window<TSource>(
      this IObservable<TSource> source,
      int count,
      int skip)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (count <= 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (skip <= 0)
        throw new ArgumentOutOfRangeException(nameof (skip));
      return Observable.s_impl.Window<TSource>(source, count, skip);
    }

    public static IObservable<TResult> Cast<TResult>(this IObservable<object> source)
    {
      return source != null ? Observable.s_impl.Cast<TResult>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> DefaultIfEmpty<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.DefaultIfEmpty<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> DefaultIfEmpty<TSource>(
      this IObservable<TSource> source,
      TSource defaultValue)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return Observable.s_impl.DefaultIfEmpty<TSource>(source, defaultValue);
    }

    public static IObservable<TSource> Distinct<TSource>(this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Distinct<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> Distinct<TSource>(
      this IObservable<TSource> source,
      IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.Distinct<TSource>(source, comparer);
    }

    public static IObservable<TSource> Distinct<TSource, TKey>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      return Observable.s_impl.Distinct<TSource, TKey>(source, keySelector);
    }

    public static IObservable<TSource> Distinct<TSource, TKey>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.Distinct<TSource, TKey>(source, keySelector, comparer);
    }

    public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      return Observable.s_impl.GroupBy<TSource, TKey>(source, keySelector);
    }

    public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.GroupBy<TSource, TKey>(source, keySelector, comparer);
    }

    public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (elementSelector == null)
        throw new ArgumentNullException(nameof (elementSelector));
      return Observable.s_impl.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector);
    }

    public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (elementSelector == null)
        throw new ArgumentNullException(nameof (elementSelector));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
    }

    public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      int capacity)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (capacity < 0)
        throw new ArgumentOutOfRangeException(nameof (capacity));
      return Observable.s_impl.GroupBy<TSource, TKey>(source, keySelector, capacity);
    }

    public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      int capacity,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (capacity < 0)
        throw new ArgumentOutOfRangeException(nameof (capacity));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.GroupBy<TSource, TKey>(source, keySelector, capacity, comparer);
    }

    public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      int capacity)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (elementSelector == null)
        throw new ArgumentNullException(nameof (elementSelector));
      if (capacity < 0)
        throw new ArgumentOutOfRangeException(nameof (capacity));
      return Observable.s_impl.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, capacity);
    }

    public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      int capacity,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (elementSelector == null)
        throw new ArgumentNullException(nameof (elementSelector));
      if (capacity < 0)
        throw new ArgumentOutOfRangeException(nameof (capacity));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, capacity, comparer);
    }

    public static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (elementSelector == null)
        throw new ArgumentNullException(nameof (elementSelector));
      if (durationSelector == null)
        throw new ArgumentNullException(nameof (durationSelector));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.GroupByUntil<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, comparer);
    }

    public static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (elementSelector == null)
        throw new ArgumentNullException(nameof (elementSelector));
      if (durationSelector == null)
        throw new ArgumentNullException(nameof (durationSelector));
      return Observable.s_impl.GroupByUntil<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector);
    }

    public static IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (durationSelector == null)
        throw new ArgumentNullException(nameof (durationSelector));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.GroupByUntil<TSource, TKey, TDuration>(source, keySelector, durationSelector, comparer);
    }

    public static IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (durationSelector == null)
        throw new ArgumentNullException(nameof (durationSelector));
      return Observable.s_impl.GroupByUntil<TSource, TKey, TDuration>(source, keySelector, durationSelector);
    }

    public static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector,
      int capacity,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (elementSelector == null)
        throw new ArgumentNullException(nameof (elementSelector));
      if (durationSelector == null)
        throw new ArgumentNullException(nameof (durationSelector));
      if (capacity < 0)
        throw new ArgumentOutOfRangeException(nameof (capacity));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.GroupByUntil<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, capacity, comparer);
    }

    public static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector,
      int capacity)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (elementSelector == null)
        throw new ArgumentNullException(nameof (elementSelector));
      if (durationSelector == null)
        throw new ArgumentNullException(nameof (durationSelector));
      if (capacity < 0)
        throw new ArgumentOutOfRangeException(nameof (capacity));
      return Observable.s_impl.GroupByUntil<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, capacity);
    }

    public static IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector,
      int capacity,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (durationSelector == null)
        throw new ArgumentNullException(nameof (durationSelector));
      if (capacity < 0)
        throw new ArgumentOutOfRangeException(nameof (capacity));
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      return Observable.s_impl.GroupByUntil<TSource, TKey, TDuration>(source, keySelector, durationSelector, capacity, comparer);
    }

    public static IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(
      this IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector,
      int capacity)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (durationSelector == null)
        throw new ArgumentNullException(nameof (durationSelector));
      if (capacity < 0)
        throw new ArgumentOutOfRangeException(nameof (capacity));
      return Observable.s_impl.GroupByUntil<TSource, TKey, TDuration>(source, keySelector, durationSelector, capacity);
    }

    public static IObservable<TResult> GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
      this IObservable<TLeft> left,
      IObservable<TRight> right,
      Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
      Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
      Func<TLeft, IObservable<TRight>, TResult> resultSelector)
    {
      if (left == null)
        throw new ArgumentNullException(nameof (left));
      if (right == null)
        throw new ArgumentNullException(nameof (right));
      if (leftDurationSelector == null)
        throw new ArgumentNullException(nameof (leftDurationSelector));
      if (rightDurationSelector == null)
        throw new ArgumentNullException(nameof (rightDurationSelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
    }

    public static IObservable<TResult> Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
      this IObservable<TLeft> left,
      IObservable<TRight> right,
      Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
      Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
      Func<TLeft, TRight, TResult> resultSelector)
    {
      if (left == null)
        throw new ArgumentNullException(nameof (left));
      if (right == null)
        throw new ArgumentNullException(nameof (right));
      if (leftDurationSelector == null)
        throw new ArgumentNullException(nameof (leftDurationSelector));
      if (rightDurationSelector == null)
        throw new ArgumentNullException(nameof (rightDurationSelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
    }

    public static IObservable<TResult> OfType<TResult>(this IObservable<object> source)
    {
      return source != null ? Observable.s_impl.OfType<TResult>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TResult> Select<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TSource, TResult> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Select<TSource, TResult>(source, selector);
    }

    public static IObservable<TResult> Select<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TSource, int, TResult> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.Select<TSource, TResult>(source, selector);
    }

    public static IObservable<TOther> SelectMany<TSource, TOther>(
      this IObservable<TSource> source,
      IObservable<TOther> other)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      return Observable.s_impl.SelectMany<TSource, TOther>(source, other);
    }

    public static IObservable<TResult> SelectMany<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TSource, IObservable<TResult>> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.SelectMany<TSource, TResult>(source, selector);
    }

    public static IObservable<TResult> SelectMany<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TSource, int, IObservable<TResult>> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.SelectMany<TSource, TResult>(source, selector);
    }

    public static IObservable<TResult> SelectMany<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TSource, Task<TResult>> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.SelectMany<TSource, TResult>(source, selector);
    }

    public static IObservable<TResult> SelectMany<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TSource, int, Task<TResult>> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.SelectMany<TSource, TResult>(source, selector);
    }

    public static IObservable<TResult> SelectMany<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TSource, CancellationToken, Task<TResult>> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.SelectMany<TSource, TResult>(source, selector);
    }

    public static IObservable<TResult> SelectMany<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TSource, int, CancellationToken, Task<TResult>> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.SelectMany<TSource, TResult>(source, selector);
    }

    public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(
      this IObservable<TSource> source,
      Func<TSource, IObservable<TCollection>> collectionSelector,
      Func<TSource, TCollection, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (collectionSelector == null)
        throw new ArgumentNullException(nameof (collectionSelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
    }

    public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(
      this IObservable<TSource> source,
      Func<TSource, int, IObservable<TCollection>> collectionSelector,
      Func<TSource, int, TCollection, int, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (collectionSelector == null)
        throw new ArgumentNullException(nameof (collectionSelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
    }

    public static IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(
      this IObservable<TSource> source,
      Func<TSource, Task<TTaskResult>> taskSelector,
      Func<TSource, TTaskResult, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (taskSelector == null)
        throw new ArgumentNullException(nameof (taskSelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.SelectMany<TSource, TTaskResult, TResult>(source, taskSelector, resultSelector);
    }

    public static IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(
      this IObservable<TSource> source,
      Func<TSource, int, Task<TTaskResult>> taskSelector,
      Func<TSource, int, TTaskResult, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (taskSelector == null)
        throw new ArgumentNullException(nameof (taskSelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.SelectMany<TSource, TTaskResult, TResult>(source, taskSelector, resultSelector);
    }

    public static IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(
      this IObservable<TSource> source,
      Func<TSource, CancellationToken, Task<TTaskResult>> taskSelector,
      Func<TSource, TTaskResult, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (taskSelector == null)
        throw new ArgumentNullException(nameof (taskSelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.SelectMany<TSource, TTaskResult, TResult>(source, taskSelector, resultSelector);
    }

    public static IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(
      this IObservable<TSource> source,
      Func<TSource, int, CancellationToken, Task<TTaskResult>> taskSelector,
      Func<TSource, int, TTaskResult, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (taskSelector == null)
        throw new ArgumentNullException(nameof (taskSelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.SelectMany<TSource, TTaskResult, TResult>(source, taskSelector, resultSelector);
    }

    public static IObservable<TResult> SelectMany<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TSource, IObservable<TResult>> onNext,
      Func<Exception, IObservable<TResult>> onError,
      Func<IObservable<TResult>> onCompleted)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      if (onError == null)
        throw new ArgumentNullException(nameof (onError));
      if (onCompleted == null)
        throw new ArgumentNullException(nameof (onCompleted));
      return Observable.s_impl.SelectMany<TSource, TResult>(source, onNext, onError, onCompleted);
    }

    public static IObservable<TResult> SelectMany<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TSource, int, IObservable<TResult>> onNext,
      Func<Exception, IObservable<TResult>> onError,
      Func<IObservable<TResult>> onCompleted)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      if (onError == null)
        throw new ArgumentNullException(nameof (onError));
      if (onCompleted == null)
        throw new ArgumentNullException(nameof (onCompleted));
      return Observable.s_impl.SelectMany<TSource, TResult>(source, onNext, onError, onCompleted);
    }

    public static IObservable<TResult> SelectMany<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TSource, IEnumerable<TResult>> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.SelectMany<TSource, TResult>(source, selector);
    }

    public static IObservable<TResult> SelectMany<TSource, TResult>(
      this IObservable<TSource> source,
      Func<TSource, int, IEnumerable<TResult>> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      return Observable.s_impl.SelectMany<TSource, TResult>(source, selector);
    }

    public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(
      this IObservable<TSource> source,
      Func<TSource, IEnumerable<TCollection>> collectionSelector,
      Func<TSource, TCollection, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (collectionSelector == null)
        throw new ArgumentNullException(nameof (collectionSelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
    }

    public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(
      this IObservable<TSource> source,
      Func<TSource, int, IEnumerable<TCollection>> collectionSelector,
      Func<TSource, int, TCollection, int, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (collectionSelector == null)
        throw new ArgumentNullException(nameof (collectionSelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return Observable.s_impl.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
    }

    public static IObservable<TSource> Skip<TSource>(this IObservable<TSource> source, int count)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      return Observable.s_impl.Skip<TSource>(source, count);
    }

    public static IObservable<TSource> SkipWhile<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.SkipWhile<TSource>(source, predicate);
    }

    public static IObservable<TSource> SkipWhile<TSource>(
      this IObservable<TSource> source,
      Func<TSource, int, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.SkipWhile<TSource>(source, predicate);
    }

    public static IObservable<TSource> Take<TSource>(this IObservable<TSource> source, int count)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      return Observable.s_impl.Take<TSource>(source, count);
    }

    public static IObservable<TSource> Take<TSource>(
      this IObservable<TSource> source,
      int count,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Take<TSource>(source, count, scheduler);
    }

    public static IObservable<TSource> TakeWhile<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.TakeWhile<TSource>(source, predicate);
    }

    public static IObservable<TSource> TakeWhile<TSource>(
      this IObservable<TSource> source,
      Func<TSource, int, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.TakeWhile<TSource>(source, predicate);
    }

    public static IObservable<TSource> Where<TSource>(
      this IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.Where<TSource>(source, predicate);
    }

    public static IObservable<TSource> Where<TSource>(
      this IObservable<TSource> source,
      Func<TSource, int, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
      return Observable.s_impl.Where<TSource>(source, predicate);
    }

    public static IObservable<IList<TSource>> Buffer<TSource>(
      this IObservable<TSource> source,
      TimeSpan timeSpan)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (timeSpan < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (timeSpan));
      return Observable.s_impl.Buffer<TSource>(source, timeSpan);
    }

    public static IObservable<IList<TSource>> Buffer<TSource>(
      this IObservable<TSource> source,
      TimeSpan timeSpan,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (timeSpan < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (timeSpan));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Buffer<TSource>(source, timeSpan, scheduler);
    }

    public static IObservable<IList<TSource>> Buffer<TSource>(
      this IObservable<TSource> source,
      TimeSpan timeSpan,
      TimeSpan timeShift)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (timeSpan < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (timeSpan));
      if (timeShift < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (timeShift));
      return Observable.s_impl.Buffer<TSource>(source, timeSpan, timeShift);
    }

    public static IObservable<IList<TSource>> Buffer<TSource>(
      this IObservable<TSource> source,
      TimeSpan timeSpan,
      TimeSpan timeShift,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (timeSpan < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (timeSpan));
      if (timeShift < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (timeShift));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Buffer<TSource>(source, timeSpan, timeShift, scheduler);
    }

    public static IObservable<IList<TSource>> Buffer<TSource>(
      this IObservable<TSource> source,
      TimeSpan timeSpan,
      int count)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (timeSpan < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (timeSpan));
      if (count <= 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      return Observable.s_impl.Buffer<TSource>(source, timeSpan, count);
    }

    public static IObservable<IList<TSource>> Buffer<TSource>(
      this IObservable<TSource> source,
      TimeSpan timeSpan,
      int count,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (timeSpan < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (timeSpan));
      if (count <= 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Buffer<TSource>(source, timeSpan, count, scheduler);
    }

    public static IObservable<TSource> Delay<TSource>(
      this IObservable<TSource> source,
      TimeSpan dueTime)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (dueTime < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (dueTime));
      return Observable.s_impl.Delay<TSource>(source, dueTime);
    }

    public static IObservable<TSource> Delay<TSource>(
      this IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (dueTime < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (dueTime));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Delay<TSource>(source, dueTime, scheduler);
    }

    public static IObservable<TSource> Delay<TSource>(
      this IObservable<TSource> source,
      DateTimeOffset dueTime)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return Observable.s_impl.Delay<TSource>(source, dueTime);
    }

    public static IObservable<TSource> Delay<TSource>(
      this IObservable<TSource> source,
      DateTimeOffset dueTime,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Delay<TSource>(source, dueTime, scheduler);
    }

    public static IObservable<TSource> Delay<TSource, TDelay>(
      this IObservable<TSource> source,
      Func<TSource, IObservable<TDelay>> delayDurationSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (delayDurationSelector == null)
        throw new ArgumentNullException(nameof (delayDurationSelector));
      return Observable.s_impl.Delay<TSource, TDelay>(source, delayDurationSelector);
    }

    public static IObservable<TSource> Delay<TSource, TDelay>(
      this IObservable<TSource> source,
      IObservable<TDelay> subscriptionDelay,
      Func<TSource, IObservable<TDelay>> delayDurationSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (subscriptionDelay == null)
        throw new ArgumentNullException(nameof (subscriptionDelay));
      if (delayDurationSelector == null)
        throw new ArgumentNullException(nameof (delayDurationSelector));
      return Observable.s_impl.Delay<TSource, TDelay>(source, subscriptionDelay, delayDurationSelector);
    }

    public static IObservable<TSource> DelaySubscription<TSource>(
      this IObservable<TSource> source,
      TimeSpan dueTime)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (dueTime < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (dueTime));
      return Observable.s_impl.DelaySubscription<TSource>(source, dueTime);
    }

    public static IObservable<TSource> DelaySubscription<TSource>(
      this IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (dueTime < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (dueTime));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.DelaySubscription<TSource>(source, dueTime, scheduler);
    }

    public static IObservable<TSource> DelaySubscription<TSource>(
      this IObservable<TSource> source,
      DateTimeOffset dueTime)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return Observable.s_impl.DelaySubscription<TSource>(source, dueTime);
    }

    public static IObservable<TSource> DelaySubscription<TSource>(
      this IObservable<TSource> source,
      DateTimeOffset dueTime,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.DelaySubscription<TSource>(source, dueTime, scheduler);
    }

    public static IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, TimeSpan> timeSelector)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      if (iterate == null)
        throw new ArgumentNullException(nameof (iterate));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      if (timeSelector == null)
        throw new ArgumentNullException(nameof (timeSelector));
      return Observable.s_impl.Generate<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector);
    }

    public static IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, TimeSpan> timeSelector,
      IScheduler scheduler)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      if (iterate == null)
        throw new ArgumentNullException(nameof (iterate));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      if (timeSelector == null)
        throw new ArgumentNullException(nameof (timeSelector));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Generate<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
    }

    public static IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, DateTimeOffset> timeSelector)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      if (iterate == null)
        throw new ArgumentNullException(nameof (iterate));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      if (timeSelector == null)
        throw new ArgumentNullException(nameof (timeSelector));
      return Observable.s_impl.Generate<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector);
    }

    public static IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, DateTimeOffset> timeSelector,
      IScheduler scheduler)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      if (iterate == null)
        throw new ArgumentNullException(nameof (iterate));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      if (timeSelector == null)
        throw new ArgumentNullException(nameof (timeSelector));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Generate<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
    }

    public static IObservable<long> Interval(TimeSpan period)
    {
      return !(period < TimeSpan.Zero) ? Observable.s_impl.Interval(period) : throw new ArgumentOutOfRangeException(nameof (period));
    }

    public static IObservable<long> Interval(TimeSpan period, IScheduler scheduler)
    {
      if (period < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (period));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Interval(period, scheduler);
    }

    public static IObservable<TSource> Sample<TSource>(
      this IObservable<TSource> source,
      TimeSpan interval)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (interval < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (interval));
      return Observable.s_impl.Sample<TSource>(source, interval);
    }

    public static IObservable<TSource> Sample<TSource>(
      this IObservable<TSource> source,
      TimeSpan interval,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (interval < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (interval));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Sample<TSource>(source, interval, scheduler);
    }

    public static IObservable<TSource> Sample<TSource, TSample>(
      this IObservable<TSource> source,
      IObservable<TSample> sampler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (sampler == null)
        throw new ArgumentNullException(nameof (sampler));
      return Observable.s_impl.Sample<TSource, TSample>(source, sampler);
    }

    public static IObservable<TSource> Skip<TSource>(
      this IObservable<TSource> source,
      TimeSpan duration)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (duration < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (duration));
      return Observable.s_impl.Skip<TSource>(source, duration);
    }

    public static IObservable<TSource> Skip<TSource>(
      this IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (duration < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (duration));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Skip<TSource>(source, duration, scheduler);
    }

    public static IObservable<TSource> SkipLast<TSource>(
      this IObservable<TSource> source,
      TimeSpan duration)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (duration < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (duration));
      return Observable.s_impl.SkipLast<TSource>(source, duration);
    }

    public static IObservable<TSource> SkipLast<TSource>(
      this IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (duration < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (duration));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.SkipLast<TSource>(source, duration, scheduler);
    }

    public static IObservable<TSource> SkipUntil<TSource>(
      this IObservable<TSource> source,
      DateTimeOffset startTime)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return Observable.s_impl.SkipUntil<TSource>(source, startTime);
    }

    public static IObservable<TSource> SkipUntil<TSource>(
      this IObservable<TSource> source,
      DateTimeOffset startTime,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.SkipUntil<TSource>(source, startTime, scheduler);
    }

    public static IObservable<TSource> Take<TSource>(
      this IObservable<TSource> source,
      TimeSpan duration)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (duration < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (duration));
      return Observable.s_impl.Take<TSource>(source, duration);
    }

    public static IObservable<TSource> Take<TSource>(
      this IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (duration < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (duration));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Take<TSource>(source, duration, scheduler);
    }

    public static IObservable<TSource> TakeLast<TSource>(
      this IObservable<TSource> source,
      TimeSpan duration)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (duration < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (duration));
      return Observable.s_impl.TakeLast<TSource>(source, duration);
    }

    public static IObservable<TSource> TakeLast<TSource>(
      this IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (duration < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (duration));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.TakeLast<TSource>(source, duration, scheduler);
    }

    public static IObservable<TSource> TakeLast<TSource>(
      this IObservable<TSource> source,
      TimeSpan duration,
      IScheduler timerScheduler,
      IScheduler loopScheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (duration < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (duration));
      if (timerScheduler == null)
        throw new ArgumentNullException(nameof (timerScheduler));
      if (loopScheduler == null)
        throw new ArgumentNullException(nameof (loopScheduler));
      return Observable.s_impl.TakeLast<TSource>(source, duration, timerScheduler, loopScheduler);
    }

    public static IObservable<IList<TSource>> TakeLastBuffer<TSource>(
      this IObservable<TSource> source,
      TimeSpan duration)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (duration < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (duration));
      return Observable.s_impl.TakeLastBuffer<TSource>(source, duration);
    }

    public static IObservable<IList<TSource>> TakeLastBuffer<TSource>(
      this IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (duration < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (duration));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.TakeLastBuffer<TSource>(source, duration, scheduler);
    }

    public static IObservable<TSource> TakeUntil<TSource>(
      this IObservable<TSource> source,
      DateTimeOffset endTime)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return Observable.s_impl.TakeUntil<TSource>(source, endTime);
    }

    public static IObservable<TSource> TakeUntil<TSource>(
      this IObservable<TSource> source,
      DateTimeOffset endTime,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.TakeUntil<TSource>(source, endTime, scheduler);
    }

    public static IObservable<TSource> Throttle<TSource>(
      this IObservable<TSource> source,
      TimeSpan dueTime)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (dueTime < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (dueTime));
      return Observable.s_impl.Throttle<TSource>(source, dueTime);
    }

    public static IObservable<TSource> Throttle<TSource>(
      this IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (dueTime < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (dueTime));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Throttle<TSource>(source, dueTime, scheduler);
    }

    public static IObservable<TSource> Throttle<TSource, TThrottle>(
      this IObservable<TSource> source,
      Func<TSource, IObservable<TThrottle>> throttleDurationSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (throttleDurationSelector == null)
        throw new ArgumentNullException(nameof (throttleDurationSelector));
      return Observable.s_impl.Throttle<TSource, TThrottle>(source, throttleDurationSelector);
    }

    public static IObservable<System.Reactive.TimeInterval<TSource>> TimeInterval<TSource>(
      this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.TimeInterval<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<System.Reactive.TimeInterval<TSource>> TimeInterval<TSource>(
      this IObservable<TSource> source,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.TimeInterval<TSource>(source, scheduler);
    }

    public static IObservable<TSource> Timeout<TSource>(
      this IObservable<TSource> source,
      TimeSpan dueTime)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (dueTime < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (dueTime));
      return Observable.s_impl.Timeout<TSource>(source, dueTime);
    }

    public static IObservable<TSource> Timeout<TSource>(
      this IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (dueTime < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (dueTime));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Timeout<TSource>(source, dueTime, scheduler);
    }

    public static IObservable<TSource> Timeout<TSource>(
      this IObservable<TSource> source,
      TimeSpan dueTime,
      IObservable<TSource> other)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (dueTime < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (dueTime));
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      return Observable.s_impl.Timeout<TSource>(source, dueTime, other);
    }

    public static IObservable<TSource> Timeout<TSource>(
      this IObservable<TSource> source,
      TimeSpan dueTime,
      IObservable<TSource> other,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (dueTime < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (dueTime));
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Timeout<TSource>(source, dueTime, other, scheduler);
    }

    public static IObservable<TSource> Timeout<TSource>(
      this IObservable<TSource> source,
      DateTimeOffset dueTime)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return Observable.s_impl.Timeout<TSource>(source, dueTime);
    }

    public static IObservable<TSource> Timeout<TSource>(
      this IObservable<TSource> source,
      DateTimeOffset dueTime,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Timeout<TSource>(source, dueTime, scheduler);
    }

    public static IObservable<TSource> Timeout<TSource>(
      this IObservable<TSource> source,
      DateTimeOffset dueTime,
      IObservable<TSource> other)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      return Observable.s_impl.Timeout<TSource>(source, dueTime, other);
    }

    public static IObservable<TSource> Timeout<TSource>(
      this IObservable<TSource> source,
      DateTimeOffset dueTime,
      IObservable<TSource> other,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      return Observable.s_impl.Timeout<TSource>(source, dueTime, other, scheduler);
    }

    public static IObservable<TSource> Timeout<TSource, TTimeout>(
      this IObservable<TSource> source,
      Func<TSource, IObservable<TTimeout>> timeoutDurationSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (timeoutDurationSelector == null)
        throw new ArgumentNullException(nameof (timeoutDurationSelector));
      return Observable.s_impl.Timeout<TSource, TTimeout>(source, timeoutDurationSelector);
    }

    public static IObservable<TSource> Timeout<TSource, TTimeout>(
      this IObservable<TSource> source,
      Func<TSource, IObservable<TTimeout>> timeoutDurationSelector,
      IObservable<TSource> other)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (timeoutDurationSelector == null)
        throw new ArgumentNullException(nameof (timeoutDurationSelector));
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      return Observable.s_impl.Timeout<TSource, TTimeout>(source, timeoutDurationSelector, other);
    }

    public static IObservable<TSource> Timeout<TSource, TTimeout>(
      this IObservable<TSource> source,
      IObservable<TTimeout> firstTimeout,
      Func<TSource, IObservable<TTimeout>> timeoutDurationSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (firstTimeout == null)
        throw new ArgumentNullException(nameof (firstTimeout));
      if (timeoutDurationSelector == null)
        throw new ArgumentNullException(nameof (timeoutDurationSelector));
      return Observable.s_impl.Timeout<TSource, TTimeout>(source, firstTimeout, timeoutDurationSelector);
    }

    public static IObservable<TSource> Timeout<TSource, TTimeout>(
      this IObservable<TSource> source,
      IObservable<TTimeout> firstTimeout,
      Func<TSource, IObservable<TTimeout>> timeoutDurationSelector,
      IObservable<TSource> other)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (firstTimeout == null)
        throw new ArgumentNullException(nameof (firstTimeout));
      if (timeoutDurationSelector == null)
        throw new ArgumentNullException(nameof (timeoutDurationSelector));
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      return Observable.s_impl.Timeout<TSource, TTimeout>(source, firstTimeout, timeoutDurationSelector, other);
    }

    public static IObservable<long> Timer(TimeSpan dueTime) => Observable.s_impl.Timer(dueTime);

    public static IObservable<long> Timer(DateTimeOffset dueTime)
    {
      return Observable.s_impl.Timer(dueTime);
    }

    public static IObservable<long> Timer(TimeSpan dueTime, TimeSpan period)
    {
      if (period < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (period));
      return Observable.s_impl.Timer(dueTime, period);
    }

    public static IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period)
    {
      if (period < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (period));
      return Observable.s_impl.Timer(dueTime, period);
    }

    public static IObservable<long> Timer(TimeSpan dueTime, IScheduler scheduler)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Timer(dueTime, scheduler);
    }

    public static IObservable<long> Timer(DateTimeOffset dueTime, IScheduler scheduler)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Timer(dueTime, scheduler);
    }

    public static IObservable<long> Timer(TimeSpan dueTime, TimeSpan period, IScheduler scheduler)
    {
      if (period < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (period));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Timer(dueTime, period, scheduler);
    }

    public static IObservable<long> Timer(
      DateTimeOffset dueTime,
      TimeSpan period,
      IScheduler scheduler)
    {
      if (period < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (period));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Timer(dueTime, period, scheduler);
    }

    public static IObservable<Timestamped<TSource>> Timestamp<TSource>(
      this IObservable<TSource> source)
    {
      return source != null ? Observable.s_impl.Timestamp<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<Timestamped<TSource>> Timestamp<TSource>(
      this IObservable<TSource> source,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Timestamp<TSource>(source, scheduler);
    }

    public static IObservable<IObservable<TSource>> Window<TSource>(
      this IObservable<TSource> source,
      TimeSpan timeSpan)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (timeSpan < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (timeSpan));
      return Observable.s_impl.Window<TSource>(source, timeSpan);
    }

    public static IObservable<IObservable<TSource>> Window<TSource>(
      this IObservable<TSource> source,
      TimeSpan timeSpan,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (timeSpan < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (timeSpan));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Window<TSource>(source, timeSpan, scheduler);
    }

    public static IObservable<IObservable<TSource>> Window<TSource>(
      this IObservable<TSource> source,
      TimeSpan timeSpan,
      TimeSpan timeShift)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (timeSpan < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (timeSpan));
      if (timeShift < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (timeShift));
      return Observable.s_impl.Window<TSource>(source, timeSpan, timeShift);
    }

    public static IObservable<IObservable<TSource>> Window<TSource>(
      this IObservable<TSource> source,
      TimeSpan timeSpan,
      TimeSpan timeShift,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (timeSpan < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (timeSpan));
      if (timeShift < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (timeShift));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Window<TSource>(source, timeSpan, timeShift, scheduler);
    }

    public static IObservable<IObservable<TSource>> Window<TSource>(
      this IObservable<TSource> source,
      TimeSpan timeSpan,
      int count)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (timeSpan < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (timeSpan));
      if (count <= 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      return Observable.s_impl.Window<TSource>(source, timeSpan, count);
    }

    public static IObservable<IObservable<TSource>> Window<TSource>(
      this IObservable<TSource> source,
      TimeSpan timeSpan,
      int count,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (timeSpan < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (timeSpan));
      if (count <= 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return Observable.s_impl.Window<TSource>(source, timeSpan, count, scheduler);
    }
  }
}
