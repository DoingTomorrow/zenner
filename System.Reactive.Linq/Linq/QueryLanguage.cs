// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.QueryLanguage
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Joins;
using System.Reactive.Linq.ObservableImpl;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace System.Reactive.Linq
{
  internal class QueryLanguage : IQueryLanguage
  {
    public virtual IObservable<TAccumulate> Aggregate<TSource, TAccumulate>(
      IObservable<TSource> source,
      TAccumulate seed,
      Func<TAccumulate, TSource, TAccumulate> accumulator)
    {
      return (IObservable<TAccumulate>) new System.Reactive.Linq.ObservableImpl.Aggregate<TSource, TAccumulate, TAccumulate>(source, seed, accumulator, Stubs<TAccumulate>.I);
    }

    public virtual IObservable<TResult> Aggregate<TSource, TAccumulate, TResult>(
      IObservable<TSource> source,
      TAccumulate seed,
      Func<TAccumulate, TSource, TAccumulate> accumulator,
      Func<TAccumulate, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Aggregate<TSource, TAccumulate, TResult>(source, seed, accumulator, resultSelector);
    }

    public virtual IObservable<TSource> Aggregate<TSource>(
      IObservable<TSource> source,
      Func<TSource, TSource, TSource> accumulator)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Aggregate<TSource>(source, accumulator);
    }

    public virtual IObservable<double> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, double> selector)
    {
      return this.Average(this.Select<TSource, double>(source, selector));
    }

    public virtual IObservable<float> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, float> selector)
    {
      return this.Average(this.Select<TSource, float>(source, selector));
    }

    public virtual IObservable<Decimal> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, Decimal> selector)
    {
      return this.Average(this.Select<TSource, Decimal>(source, selector));
    }

    public virtual IObservable<double> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, int> selector)
    {
      return this.Average(this.Select<TSource, int>(source, selector));
    }

    public virtual IObservable<double> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, long> selector)
    {
      return this.Average(this.Select<TSource, long>(source, selector));
    }

    public virtual IObservable<double?> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, double?> selector)
    {
      return this.Average(this.Select<TSource, double?>(source, selector));
    }

    public virtual IObservable<float?> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, float?> selector)
    {
      return this.Average(this.Select<TSource, float?>(source, selector));
    }

    public virtual IObservable<Decimal?> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, Decimal?> selector)
    {
      return this.Average(this.Select<TSource, Decimal?>(source, selector));
    }

    public virtual IObservable<double?> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, int?> selector)
    {
      return this.Average(this.Select<TSource, int?>(source, selector));
    }

    public virtual IObservable<double?> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, long?> selector)
    {
      return this.Average(this.Select<TSource, long?>(source, selector));
    }

    public virtual IObservable<bool> All<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<bool>) new System.Reactive.Linq.ObservableImpl.All<TSource>(source, predicate);
    }

    public virtual IObservable<bool> Any<TSource>(IObservable<TSource> source)
    {
      return (IObservable<bool>) new System.Reactive.Linq.ObservableImpl.Any<TSource>(source);
    }

    public virtual IObservable<bool> Any<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<bool>) new System.Reactive.Linq.ObservableImpl.Any<TSource>(source, predicate);
    }

    public virtual IObservable<double> Average(IObservable<double> source)
    {
      return (IObservable<double>) new AverageDouble(source);
    }

    public virtual IObservable<float> Average(IObservable<float> source)
    {
      return (IObservable<float>) new AverageSingle(source);
    }

    public virtual IObservable<Decimal> Average(IObservable<Decimal> source)
    {
      return (IObservable<Decimal>) new AverageDecimal(source);
    }

    public virtual IObservable<double> Average(IObservable<int> source)
    {
      return (IObservable<double>) new AverageInt32(source);
    }

    public virtual IObservable<double> Average(IObservable<long> source)
    {
      return (IObservable<double>) new AverageInt64(source);
    }

    public virtual IObservable<double?> Average(IObservable<double?> source)
    {
      return (IObservable<double?>) new AverageDoubleNullable(source);
    }

    public virtual IObservable<float?> Average(IObservable<float?> source)
    {
      return (IObservable<float?>) new AverageSingleNullable(source);
    }

    public virtual IObservable<Decimal?> Average(IObservable<Decimal?> source)
    {
      return (IObservable<Decimal?>) new AverageDecimalNullable(source);
    }

    public virtual IObservable<double?> Average(IObservable<int?> source)
    {
      return (IObservable<double?>) new AverageInt32Nullable(source);
    }

    public virtual IObservable<double?> Average(IObservable<long?> source)
    {
      return (IObservable<double?>) new AverageInt64Nullable(source);
    }

    public virtual IObservable<bool> Contains<TSource>(IObservable<TSource> source, TSource value)
    {
      return (IObservable<bool>) new System.Reactive.Linq.ObservableImpl.Contains<TSource>(source, value, (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default);
    }

    public virtual IObservable<bool> Contains<TSource>(
      IObservable<TSource> source,
      TSource value,
      IEqualityComparer<TSource> comparer)
    {
      return (IObservable<bool>) new System.Reactive.Linq.ObservableImpl.Contains<TSource>(source, value, comparer);
    }

    public virtual IObservable<int> Count<TSource>(IObservable<TSource> source)
    {
      return (IObservable<int>) new System.Reactive.Linq.ObservableImpl.Count<TSource>(source);
    }

    public virtual IObservable<int> Count<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<int>) new System.Reactive.Linq.ObservableImpl.Count<TSource>(source, predicate);
    }

    public virtual IObservable<TSource> ElementAt<TSource>(IObservable<TSource> source, int index)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.ElementAt<TSource>(source, index, true);
    }

    public virtual IObservable<TSource> ElementAtOrDefault<TSource>(
      IObservable<TSource> source,
      int index)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.ElementAt<TSource>(source, index, false);
    }

    public virtual IObservable<TSource> FirstAsync<TSource>(IObservable<TSource> source)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.FirstAsync<TSource>(source, (Func<TSource, bool>) null, true);
    }

    public virtual IObservable<TSource> FirstAsync<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.FirstAsync<TSource>(source, predicate, true);
    }

    public virtual IObservable<TSource> FirstOrDefaultAsync<TSource>(IObservable<TSource> source)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.FirstAsync<TSource>(source, (Func<TSource, bool>) null, false);
    }

    public virtual IObservable<TSource> FirstOrDefaultAsync<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.FirstAsync<TSource>(source, predicate, false);
    }

    public virtual IObservable<bool> IsEmpty<TSource>(IObservable<TSource> source)
    {
      return (IObservable<bool>) new System.Reactive.Linq.ObservableImpl.IsEmpty<TSource>(source);
    }

    public virtual IObservable<TSource> LastAsync<TSource>(IObservable<TSource> source)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.LastAsync<TSource>(source, (Func<TSource, bool>) null, true);
    }

    public virtual IObservable<TSource> LastAsync<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.LastAsync<TSource>(source, predicate, true);
    }

    public virtual IObservable<TSource> LastOrDefaultAsync<TSource>(IObservable<TSource> source)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.LastAsync<TSource>(source, (Func<TSource, bool>) null, false);
    }

    public virtual IObservable<TSource> LastOrDefaultAsync<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.LastAsync<TSource>(source, predicate, false);
    }

    public virtual IObservable<long> LongCount<TSource>(IObservable<TSource> source)
    {
      return (IObservable<long>) new System.Reactive.Linq.ObservableImpl.LongCount<TSource>(source);
    }

    public virtual IObservable<long> LongCount<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<long>) new System.Reactive.Linq.ObservableImpl.LongCount<TSource>(source, predicate);
    }

    public virtual IObservable<TSource> Max<TSource>(IObservable<TSource> source)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Max<TSource>(source, (IComparer<TSource>) Comparer<TSource>.Default);
    }

    public virtual IObservable<TSource> Max<TSource>(
      IObservable<TSource> source,
      IComparer<TSource> comparer)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Max<TSource>(source, comparer);
    }

    public virtual IObservable<double> Max(IObservable<double> source)
    {
      return (IObservable<double>) new MaxDouble(source);
    }

    public virtual IObservable<float> Max(IObservable<float> source)
    {
      return (IObservable<float>) new MaxSingle(source);
    }

    public virtual IObservable<Decimal> Max(IObservable<Decimal> source)
    {
      return (IObservable<Decimal>) new MaxDecimal(source);
    }

    public virtual IObservable<int> Max(IObservable<int> source)
    {
      return (IObservable<int>) new MaxInt32(source);
    }

    public virtual IObservable<long> Max(IObservable<long> source)
    {
      return (IObservable<long>) new MaxInt64(source);
    }

    public virtual IObservable<double?> Max(IObservable<double?> source)
    {
      return (IObservable<double?>) new MaxDoubleNullable(source);
    }

    public virtual IObservable<float?> Max(IObservable<float?> source)
    {
      return (IObservable<float?>) new MaxSingleNullable(source);
    }

    public virtual IObservable<Decimal?> Max(IObservable<Decimal?> source)
    {
      return (IObservable<Decimal?>) new MaxDecimalNullable(source);
    }

    public virtual IObservable<int?> Max(IObservable<int?> source)
    {
      return (IObservable<int?>) new MaxInt32Nullable(source);
    }

    public virtual IObservable<long?> Max(IObservable<long?> source)
    {
      return (IObservable<long?>) new MaxInt64Nullable(source);
    }

    public virtual IObservable<TResult> Max<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, TResult> selector)
    {
      return this.Max<TResult>(this.Select<TSource, TResult>(source, selector));
    }

    public virtual IObservable<TResult> Max<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, TResult> selector,
      IComparer<TResult> comparer)
    {
      return this.Max<TResult>(this.Select<TSource, TResult>(source, selector), comparer);
    }

    public virtual IObservable<double> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, double> selector)
    {
      return this.Max(this.Select<TSource, double>(source, selector));
    }

    public virtual IObservable<float> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, float> selector)
    {
      return this.Max(this.Select<TSource, float>(source, selector));
    }

    public virtual IObservable<Decimal> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, Decimal> selector)
    {
      return this.Max(this.Select<TSource, Decimal>(source, selector));
    }

    public virtual IObservable<int> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, int> selector)
    {
      return this.Max(this.Select<TSource, int>(source, selector));
    }

    public virtual IObservable<long> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, long> selector)
    {
      return this.Max(this.Select<TSource, long>(source, selector));
    }

    public virtual IObservable<double?> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, double?> selector)
    {
      return this.Max(this.Select<TSource, double?>(source, selector));
    }

    public virtual IObservable<float?> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, float?> selector)
    {
      return this.Max(this.Select<TSource, float?>(source, selector));
    }

    public virtual IObservable<Decimal?> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, Decimal?> selector)
    {
      return this.Max(this.Select<TSource, Decimal?>(source, selector));
    }

    public virtual IObservable<int?> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, int?> selector)
    {
      return this.Max(this.Select<TSource, int?>(source, selector));
    }

    public virtual IObservable<long?> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, long?> selector)
    {
      return this.Max(this.Select<TSource, long?>(source, selector));
    }

    public virtual IObservable<IList<TSource>> MaxBy<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.ObservableImpl.MaxBy<TSource, TKey>(source, keySelector, (IComparer<TKey>) Comparer<TKey>.Default);
    }

    public virtual IObservable<IList<TSource>> MaxBy<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IComparer<TKey> comparer)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.ObservableImpl.MaxBy<TSource, TKey>(source, keySelector, comparer);
    }

    public virtual IObservable<TSource> Min<TSource>(IObservable<TSource> source)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Min<TSource>(source, (IComparer<TSource>) Comparer<TSource>.Default);
    }

    public virtual IObservable<TSource> Min<TSource>(
      IObservable<TSource> source,
      IComparer<TSource> comparer)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Min<TSource>(source, comparer);
    }

    public virtual IObservable<double> Min(IObservable<double> source)
    {
      return (IObservable<double>) new MinDouble(source);
    }

    public virtual IObservable<float> Min(IObservable<float> source)
    {
      return (IObservable<float>) new MinSingle(source);
    }

    public virtual IObservable<Decimal> Min(IObservable<Decimal> source)
    {
      return (IObservable<Decimal>) new MinDecimal(source);
    }

    public virtual IObservable<int> Min(IObservable<int> source)
    {
      return (IObservable<int>) new MinInt32(source);
    }

    public virtual IObservable<long> Min(IObservable<long> source)
    {
      return (IObservable<long>) new MinInt64(source);
    }

    public virtual IObservable<double?> Min(IObservable<double?> source)
    {
      return (IObservable<double?>) new MinDoubleNullable(source);
    }

    public virtual IObservable<float?> Min(IObservable<float?> source)
    {
      return (IObservable<float?>) new MinSingleNullable(source);
    }

    public virtual IObservable<Decimal?> Min(IObservable<Decimal?> source)
    {
      return (IObservable<Decimal?>) new MinDecimalNullable(source);
    }

    public virtual IObservable<int?> Min(IObservable<int?> source)
    {
      return (IObservable<int?>) new MinInt32Nullable(source);
    }

    public virtual IObservable<long?> Min(IObservable<long?> source)
    {
      return (IObservable<long?>) new MinInt64Nullable(source);
    }

    public virtual IObservable<TResult> Min<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, TResult> selector)
    {
      return this.Min<TResult>(this.Select<TSource, TResult>(source, selector));
    }

    public virtual IObservable<TResult> Min<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, TResult> selector,
      IComparer<TResult> comparer)
    {
      return this.Min<TResult>(this.Select<TSource, TResult>(source, selector), comparer);
    }

    public virtual IObservable<double> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, double> selector)
    {
      return this.Min(this.Select<TSource, double>(source, selector));
    }

    public virtual IObservable<float> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, float> selector)
    {
      return this.Min(this.Select<TSource, float>(source, selector));
    }

    public virtual IObservable<Decimal> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, Decimal> selector)
    {
      return this.Min(this.Select<TSource, Decimal>(source, selector));
    }

    public virtual IObservable<int> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, int> selector)
    {
      return this.Min(this.Select<TSource, int>(source, selector));
    }

    public virtual IObservable<long> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, long> selector)
    {
      return this.Min(this.Select<TSource, long>(source, selector));
    }

    public virtual IObservable<double?> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, double?> selector)
    {
      return this.Min(this.Select<TSource, double?>(source, selector));
    }

    public virtual IObservable<float?> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, float?> selector)
    {
      return this.Min(this.Select<TSource, float?>(source, selector));
    }

    public virtual IObservable<Decimal?> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, Decimal?> selector)
    {
      return this.Min(this.Select<TSource, Decimal?>(source, selector));
    }

    public virtual IObservable<int?> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, int?> selector)
    {
      return this.Min(this.Select<TSource, int?>(source, selector));
    }

    public virtual IObservable<long?> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, long?> selector)
    {
      return this.Min(this.Select<TSource, long?>(source, selector));
    }

    public virtual IObservable<IList<TSource>> MinBy<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.ObservableImpl.MinBy<TSource, TKey>(source, keySelector, (IComparer<TKey>) Comparer<TKey>.Default);
    }

    public virtual IObservable<IList<TSource>> MinBy<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IComparer<TKey> comparer)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.ObservableImpl.MinBy<TSource, TKey>(source, keySelector, comparer);
    }

    public virtual IObservable<bool> SequenceEqual<TSource>(
      IObservable<TSource> first,
      IObservable<TSource> second)
    {
      return (IObservable<bool>) new System.Reactive.Linq.ObservableImpl.SequenceEqual<TSource>(first, second, (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default);
    }

    public virtual IObservable<bool> SequenceEqual<TSource>(
      IObservable<TSource> first,
      IObservable<TSource> second,
      IEqualityComparer<TSource> comparer)
    {
      return (IObservable<bool>) new System.Reactive.Linq.ObservableImpl.SequenceEqual<TSource>(first, second, comparer);
    }

    public virtual IObservable<bool> SequenceEqual<TSource>(
      IObservable<TSource> first,
      IEnumerable<TSource> second)
    {
      return (IObservable<bool>) new System.Reactive.Linq.ObservableImpl.SequenceEqual<TSource>(first, second, (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default);
    }

    public virtual IObservable<bool> SequenceEqual<TSource>(
      IObservable<TSource> first,
      IEnumerable<TSource> second,
      IEqualityComparer<TSource> comparer)
    {
      return (IObservable<bool>) new System.Reactive.Linq.ObservableImpl.SequenceEqual<TSource>(first, second, comparer);
    }

    public virtual IObservable<TSource> SingleAsync<TSource>(IObservable<TSource> source)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.SingleAsync<TSource>(source, (Func<TSource, bool>) null, true);
    }

    public virtual IObservable<TSource> SingleAsync<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.SingleAsync<TSource>(source, predicate, true);
    }

    public virtual IObservable<TSource> SingleOrDefaultAsync<TSource>(IObservable<TSource> source)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.SingleAsync<TSource>(source, (Func<TSource, bool>) null, false);
    }

    public virtual IObservable<TSource> SingleOrDefaultAsync<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.SingleAsync<TSource>(source, predicate, false);
    }

    public virtual IObservable<double> Sum(IObservable<double> source)
    {
      return (IObservable<double>) new SumDouble(source);
    }

    public virtual IObservable<float> Sum(IObservable<float> source)
    {
      return (IObservable<float>) new SumSingle(source);
    }

    public virtual IObservable<Decimal> Sum(IObservable<Decimal> source)
    {
      return (IObservable<Decimal>) new SumDecimal(source);
    }

    public virtual IObservable<int> Sum(IObservable<int> source)
    {
      return (IObservable<int>) new SumInt32(source);
    }

    public virtual IObservable<long> Sum(IObservable<long> source)
    {
      return (IObservable<long>) new SumInt64(source);
    }

    public virtual IObservable<double?> Sum(IObservable<double?> source)
    {
      return (IObservable<double?>) new SumDoubleNullable(source);
    }

    public virtual IObservable<float?> Sum(IObservable<float?> source)
    {
      return (IObservable<float?>) new SumSingleNullable(source);
    }

    public virtual IObservable<Decimal?> Sum(IObservable<Decimal?> source)
    {
      return (IObservable<Decimal?>) new SumDecimalNullable(source);
    }

    public virtual IObservable<int?> Sum(IObservable<int?> source)
    {
      return (IObservable<int?>) new SumInt32Nullable(source);
    }

    public virtual IObservable<long?> Sum(IObservable<long?> source)
    {
      return (IObservable<long?>) new SumInt64Nullable(source);
    }

    public virtual IObservable<double> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, double> selector)
    {
      return this.Sum(this.Select<TSource, double>(source, selector));
    }

    public virtual IObservable<float> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, float> selector)
    {
      return this.Sum(this.Select<TSource, float>(source, selector));
    }

    public virtual IObservable<Decimal> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, Decimal> selector)
    {
      return this.Sum(this.Select<TSource, Decimal>(source, selector));
    }

    public virtual IObservable<int> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, int> selector)
    {
      return this.Sum(this.Select<TSource, int>(source, selector));
    }

    public virtual IObservable<long> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, long> selector)
    {
      return this.Sum(this.Select<TSource, long>(source, selector));
    }

    public virtual IObservable<double?> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, double?> selector)
    {
      return this.Sum(this.Select<TSource, double?>(source, selector));
    }

    public virtual IObservable<float?> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, float?> selector)
    {
      return this.Sum(this.Select<TSource, float?>(source, selector));
    }

    public virtual IObservable<Decimal?> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, Decimal?> selector)
    {
      return this.Sum(this.Select<TSource, Decimal?>(source, selector));
    }

    public virtual IObservable<int?> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, int?> selector)
    {
      return this.Sum(this.Select<TSource, int?>(source, selector));
    }

    public virtual IObservable<long?> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, long?> selector)
    {
      return this.Sum(this.Select<TSource, long?>(source, selector));
    }

    public virtual IObservable<TSource[]> ToArray<TSource>(IObservable<TSource> source)
    {
      return (IObservable<TSource[]>) new System.Reactive.Linq.ObservableImpl.ToArray<TSource>(source);
    }

    public virtual IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      return (IObservable<IDictionary<TKey, TElement>>) new System.Reactive.Linq.ObservableImpl.ToDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
    }

    public virtual IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector)
    {
      return (IObservable<IDictionary<TKey, TElement>>) new System.Reactive.Linq.ObservableImpl.ToDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      return (IObservable<IDictionary<TKey, TSource>>) new System.Reactive.Linq.ObservableImpl.ToDictionary<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (x => x), comparer);
    }

    public virtual IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return (IObservable<IDictionary<TKey, TSource>>) new System.Reactive.Linq.ObservableImpl.ToDictionary<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (x => x), (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<IList<TSource>> ToList<TSource>(IObservable<TSource> source)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.ObservableImpl.ToList<TSource>(source);
    }

    public virtual IObservable<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      return (IObservable<ILookup<TKey, TElement>>) new System.Reactive.Linq.ObservableImpl.ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
    }

    public virtual IObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      return (IObservable<ILookup<TKey, TSource>>) new System.Reactive.Linq.ObservableImpl.ToLookup<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (x => x), comparer);
    }

    public virtual IObservable<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector)
    {
      return (IObservable<ILookup<TKey, TElement>>) new System.Reactive.Linq.ObservableImpl.ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return (IObservable<ILookup<TKey, TSource>>) new System.Reactive.Linq.ObservableImpl.ToLookup<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (x => x), (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual Func<IObservable<TResult>> FromAsyncPattern<TResult>(
      Func<AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<IObservable<TResult>>) (() =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin((AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          IScheduler asyncConversions = SchedulerDefaults.AsyncConversions;
          return Observable.Throw<TResult>(ex, asyncConversions);
        }
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, IObservable<TResult>> FromAsyncPattern<T1, TResult>(
      Func<T1, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<T1, IObservable<TResult>>) (x =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin(x, (AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          IScheduler asyncConversions = SchedulerDefaults.AsyncConversions;
          return Observable.Throw<TResult>(ex, asyncConversions);
        }
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, IObservable<TResult>> FromAsyncPattern<T1, T2, TResult>(
      Func<T1, T2, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<T1, T2, IObservable<TResult>>) ((x, y) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin(x, y, (AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          IScheduler asyncConversions = SchedulerDefaults.AsyncConversions;
          return Observable.Throw<TResult>(ex, asyncConversions);
        }
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, TResult>(
      Func<T1, T2, T3, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<T1, T2, T3, IObservable<TResult>>) ((x, y, z) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin(x, y, z, (AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          IScheduler asyncConversions = SchedulerDefaults.AsyncConversions;
          return Observable.Throw<TResult>(ex, asyncConversions);
        }
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, TResult>(
      Func<T1, T2, T3, T4, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<T1, T2, T3, T4, IObservable<TResult>>) ((x, y, z, a) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin(x, y, z, a, (AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          IScheduler asyncConversions = SchedulerDefaults.AsyncConversions;
          return Observable.Throw<TResult>(ex, asyncConversions);
        }
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, TResult>(
      Func<T1, T2, T3, T4, T5, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<T1, T2, T3, T4, T5, IObservable<TResult>>) ((x, y, z, a, b) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin(x, y, z, a, b, (AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          IScheduler asyncConversions = SchedulerDefaults.AsyncConversions;
          return Observable.Throw<TResult>(ex, asyncConversions);
        }
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, TResult>(
      Func<T1, T2, T3, T4, T5, T6, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<T1, T2, T3, T4, T5, T6, IObservable<TResult>>) ((x, y, z, a, b, c) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin(x, y, z, a, b, c, (AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          IScheduler asyncConversions = SchedulerDefaults.AsyncConversions;
          return Observable.Throw<TResult>(ex, asyncConversions);
        }
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, IObservable<TResult>>) ((x, y, z, a, b, c, d) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin(x, y, z, a, b, c, d, (AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          IScheduler asyncConversions = SchedulerDefaults.AsyncConversions;
          return Observable.Throw<TResult>(ex, asyncConversions);
        }
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<TResult>>) ((x, y, z, a, b, c, d, e) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin(x, y, z, a, b, c, d, e, (AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          IScheduler asyncConversions = SchedulerDefaults.AsyncConversions;
          return Observable.Throw<TResult>(ex, asyncConversions);
        }
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<TResult>>) ((x, y, z, a, b, c, d, e, f) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin(x, y, z, a, b, c, d, e, f, (AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          IScheduler asyncConversions = SchedulerDefaults.AsyncConversions;
          return Observable.Throw<TResult>(ex, asyncConversions);
        }
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<TResult>>) ((x, y, z, a, b, c, d, e, f, g) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin(x, y, z, a, b, c, d, e, f, g, (AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          IScheduler asyncConversions = SchedulerDefaults.AsyncConversions;
          return Observable.Throw<TResult>(ex, asyncConversions);
        }
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<TResult>>) ((x, y, z, a, b, c, d, e, f, g, h) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin(x, y, z, a, b, c, d, e, f, g, h, (AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          IScheduler asyncConversions = SchedulerDefaults.AsyncConversions;
          return Observable.Throw<TResult>(ex, asyncConversions);
        }
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<TResult>>) ((x, y, z, a, b, c, d, e, f, g, h, i) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin(x, y, z, a, b, c, d, e, f, g, h, i, (AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          IScheduler asyncConversions = SchedulerDefaults.AsyncConversions;
          return Observable.Throw<TResult>(ex, asyncConversions);
        }
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<TResult>>) ((x, y, z, a, b, c, d, e, f, g, h, i, j) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin(x, y, z, a, b, c, d, e, f, g, h, i, j, (AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          IScheduler asyncConversions = SchedulerDefaults.AsyncConversions;
          return Observable.Throw<TResult>(ex, asyncConversions);
        }
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<TResult>>) ((x, y, z, a, b, c, d, e, f, g, h, i, j, k) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin(x, y, z, a, b, c, d, e, f, g, h, i, j, k, (AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          IScheduler asyncConversions = SchedulerDefaults.AsyncConversions;
          return Observable.Throw<TResult>(ex, asyncConversions);
        }
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<IObservable<Unit>> FromAsyncPattern(
      Func<AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual Func<T1, IObservable<Unit>> FromAsyncPattern<T1>(
      Func<T1, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<T1, Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual Func<T1, T2, IObservable<Unit>> FromAsyncPattern<T1, T2>(
      Func<T1, T2, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<T1, T2, Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual Func<T1, T2, T3, IObservable<Unit>> FromAsyncPattern<T1, T2, T3>(
      Func<T1, T2, T3, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<T1, T2, T3, Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual Func<T1, T2, T3, T4, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4>(
      Func<T1, T2, T3, T4, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<T1, T2, T3, T4, Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual Func<T1, T2, T3, T4, T5, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5>(
      Func<T1, T2, T3, T4, T5, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<T1, T2, T3, T4, T5, Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6>(
      Func<T1, T2, T3, T4, T5, T6, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<T1, T2, T3, T4, T5, T6, Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7>(
      Func<T1, T2, T3, T4, T5, T6, T7, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual IObservable<TSource> Start<TSource>(Func<TSource> function)
    {
      return this.ToAsync<TSource>(function)();
    }

    public virtual IObservable<TSource> Start<TSource>(Func<TSource> function, IScheduler scheduler)
    {
      return this.ToAsync<TSource>(function, scheduler)();
    }

    public virtual IObservable<TSource> StartAsync<TSource>(Func<Task<TSource>> functionAsync)
    {
      return this.StartAsyncImpl<TSource>(functionAsync, (IScheduler) null);
    }

    public virtual IObservable<TSource> StartAsync<TSource>(
      Func<Task<TSource>> functionAsync,
      IScheduler scheduler)
    {
      return this.StartAsyncImpl<TSource>(functionAsync, scheduler);
    }

    private IObservable<TSource> StartAsyncImpl<TSource>(
      Func<Task<TSource>> functionAsync,
      IScheduler scheduler)
    {
      Task<TSource> task;
      try
      {
        task = functionAsync();
      }
      catch (Exception ex)
      {
        return this.Throw<TSource>(ex);
      }
      return scheduler != null ? task.ToObservable<TSource>(scheduler) : task.ToObservable<TSource>();
    }

    public virtual IObservable<TSource> StartAsync<TSource>(
      Func<CancellationToken, Task<TSource>> functionAsync)
    {
      return this.StartAsyncImpl<TSource>(functionAsync, (IScheduler) null);
    }

    public virtual IObservable<TSource> StartAsync<TSource>(
      Func<CancellationToken, Task<TSource>> functionAsync,
      IScheduler scheduler)
    {
      return this.StartAsyncImpl<TSource>(functionAsync, scheduler);
    }

    private IObservable<TSource> StartAsyncImpl<TSource>(
      Func<CancellationToken, Task<TSource>> functionAsync,
      IScheduler scheduler)
    {
      CancellationDisposable cancellable = new CancellationDisposable();
      Task<TSource> task;
      try
      {
        task = functionAsync(cancellable.Token);
      }
      catch (Exception ex)
      {
        return this.Throw<TSource>(ex);
      }
      IObservable<TSource> result = (IObservable<TSource>) null;
      result = scheduler == null ? task.ToObservable<TSource>() : task.ToObservable<TSource>(scheduler);
      return (IObservable<TSource>) new AnonymousObservable<TSource>((Func<IObserver<TSource>, IDisposable>) (observer => (IDisposable) StableCompositeDisposable.Create((IDisposable) cancellable, result.Subscribe(observer))));
    }

    public virtual IObservable<Unit> Start(Action action)
    {
      return this.ToAsync(action, SchedulerDefaults.AsyncConversions)();
    }

    public virtual IObservable<Unit> Start(Action action, IScheduler scheduler)
    {
      return this.ToAsync(action, scheduler)();
    }

    public virtual IObservable<Unit> StartAsync(Func<Task> actionAsync)
    {
      return this.StartAsyncImpl(actionAsync, (IScheduler) null);
    }

    public virtual IObservable<Unit> StartAsync(Func<Task> actionAsync, IScheduler scheduler)
    {
      return this.StartAsyncImpl(actionAsync, scheduler);
    }

    private IObservable<Unit> StartAsyncImpl(Func<Task> actionAsync, IScheduler scheduler)
    {
      Task task;
      try
      {
        task = actionAsync();
      }
      catch (Exception ex)
      {
        return this.Throw<Unit>(ex);
      }
      return scheduler != null ? task.ToObservable(scheduler) : task.ToObservable();
    }

    public virtual IObservable<Unit> StartAsync(Func<CancellationToken, Task> actionAsync)
    {
      return this.StartAsyncImpl(actionAsync, (IScheduler) null);
    }

    public virtual IObservable<Unit> StartAsync(
      Func<CancellationToken, Task> actionAsync,
      IScheduler scheduler)
    {
      return this.StartAsyncImpl(actionAsync, scheduler);
    }

    private IObservable<Unit> StartAsyncImpl(
      Func<CancellationToken, Task> actionAsync,
      IScheduler scheduler)
    {
      CancellationDisposable cancellable = new CancellationDisposable();
      Task task;
      try
      {
        task = actionAsync(cancellable.Token);
      }
      catch (Exception ex)
      {
        return this.Throw<Unit>(ex);
      }
      IObservable<Unit> result = (IObservable<Unit>) null;
      result = scheduler == null ? task.ToObservable() : task.ToObservable(scheduler);
      return (IObservable<Unit>) new AnonymousObservable<Unit>((Func<IObserver<Unit>, IDisposable>) (observer => (IDisposable) StableCompositeDisposable.Create((IDisposable) cancellable, result.Subscribe(observer))));
    }

    public virtual IObservable<TResult> FromAsync<TResult>(Func<Task<TResult>> functionAsync)
    {
      return this.Defer<TResult>((Func<IObservable<TResult>>) (() => this.StartAsync<TResult>(functionAsync)));
    }

    public virtual IObservable<TResult> FromAsync<TResult>(
      Func<CancellationToken, Task<TResult>> functionAsync)
    {
      return this.Defer<TResult>((Func<IObservable<TResult>>) (() => this.StartAsync<TResult>(functionAsync)));
    }

    public virtual IObservable<TResult> FromAsync<TResult>(
      Func<Task<TResult>> functionAsync,
      IScheduler scheduler)
    {
      return this.Defer<TResult>((Func<IObservable<TResult>>) (() => this.StartAsync<TResult>(functionAsync, scheduler)));
    }

    public virtual IObservable<TResult> FromAsync<TResult>(
      Func<CancellationToken, Task<TResult>> functionAsync,
      IScheduler scheduler)
    {
      return this.Defer<TResult>((Func<IObservable<TResult>>) (() => this.StartAsync<TResult>(functionAsync, scheduler)));
    }

    public virtual IObservable<Unit> FromAsync(Func<Task> actionAsync)
    {
      return this.Defer<Unit>((Func<IObservable<Unit>>) (() => this.StartAsync(actionAsync)));
    }

    public virtual IObservable<Unit> FromAsync(Func<CancellationToken, Task> actionAsync)
    {
      return this.Defer<Unit>((Func<IObservable<Unit>>) (() => this.StartAsync(actionAsync)));
    }

    public virtual IObservable<Unit> FromAsync(Func<Task> actionAsync, IScheduler scheduler)
    {
      return this.Defer<Unit>((Func<IObservable<Unit>>) (() => this.StartAsync(actionAsync, scheduler)));
    }

    public virtual IObservable<Unit> FromAsync(
      Func<CancellationToken, Task> actionAsync,
      IScheduler scheduler)
    {
      return this.Defer<Unit>((Func<IObservable<Unit>>) (() => this.StartAsync(actionAsync, scheduler)));
    }

    public virtual Func<IObservable<TResult>> ToAsync<TResult>(Func<TResult> function)
    {
      return this.ToAsync<TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<IObservable<TResult>> ToAsync<TResult>(
      Func<TResult> function,
      IScheduler scheduler)
    {
      return (Func<IObservable<TResult>>) (() =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function();
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T, IObservable<TResult>> ToAsync<T, TResult>(Func<T, TResult> function)
    {
      return this.ToAsync<T, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T, IObservable<TResult>> ToAsync<T, TResult>(
      Func<T, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T, IObservable<TResult>>) (first =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, IObservable<TResult>> ToAsync<T1, T2, TResult>(
      Func<T1, T2, TResult> function)
    {
      return this.ToAsync<T1, T2, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, IObservable<TResult>> ToAsync<T1, T2, TResult>(
      Func<T1, T2, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, IObservable<TResult>>) ((first, second) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, IObservable<TResult>> ToAsync<T1, T2, T3, TResult>(
      Func<T1, T2, T3, TResult> function)
    {
      return this.ToAsync<T1, T2, T3, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, IObservable<TResult>> ToAsync<T1, T2, T3, TResult>(
      Func<T1, T2, T3, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, IObservable<TResult>>) ((first, second, third) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second, third);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, IObservable<TResult>> ToAsync<T1, T2, T3, T4, TResult>(
      Func<T1, T2, T3, T4, TResult> function)
    {
      return this.ToAsync<T1, T2, T3, T4, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, IObservable<TResult>> ToAsync<T1, T2, T3, T4, TResult>(
      Func<T1, T2, T3, T4, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, IObservable<TResult>>) ((first, second, third, fourth) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second, third, fourth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, TResult>(
      Func<T1, T2, T3, T4, T5, TResult> function)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, TResult>(
      Func<T1, T2, T3, T4, T5, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, IObservable<TResult>>) ((first, second, third, fourth, fifth) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second, third, fourth, fifth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, TResult>(
      Func<T1, T2, T3, T4, T5, T6, TResult> function)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, TResult>(
      Func<T1, T2, T3, T4, T5, T6, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, IObservable<TResult>>) ((first, second, third, fourth, fifth, sixth) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second, third, fourth, fifth, sixth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, TResult> function)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, IObservable<TResult>>) ((first, second, third, fourth, fifth, sixth, seventh) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second, third, fourth, fifth, sixth, seventh);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<TResult>>) ((first, second, third, fourth, fifth, sixth, seventh, eight) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second, third, fourth, fifth, sixth, seventh, eight);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<TResult>>) ((first, second, third, fourth, fifth, sixth, seventh, eight, ninth) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second, third, fourth, fifth, sixth, seventh, eight, ninth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<TResult>>) ((first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<TResult>>) ((first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<TResult>>) ((first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<TResult>>) ((first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth, thirteenth) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth, thirteenth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<TResult>>) ((first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IObservable<TResult>>) ((first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> function)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IObservable<TResult>>) ((first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth, sixteenth) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second, third, fourth, fifth, sixth, seventh, eight, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth, sixteenth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return subject.AsObservable<TResult>();
      });
    }

    public virtual Func<IObservable<Unit>> ToAsync(Action action)
    {
      return this.ToAsync(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<IObservable<Unit>> ToAsync(Action action, IScheduler scheduler)
    {
      return (Func<IObservable<Unit>>) (() =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action();
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return subject.AsObservable<Unit>();
      });
    }

    public virtual Func<TSource, IObservable<Unit>> ToAsync<TSource>(Action<TSource> action)
    {
      return this.ToAsync<TSource>(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<TSource, IObservable<Unit>> ToAsync<TSource>(
      Action<TSource> action,
      IScheduler scheduler)
    {
      return (Func<TSource, IObservable<Unit>>) (first =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return subject.AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, IObservable<Unit>> ToAsync<T1, T2>(Action<T1, T2> action)
    {
      return this.ToAsync<T1, T2>(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, IObservable<Unit>> ToAsync<T1, T2>(
      Action<T1, T2> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, IObservable<Unit>>) ((first, second) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return subject.AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, T3, IObservable<Unit>> ToAsync<T1, T2, T3>(Action<T1, T2, T3> action)
    {
      return this.ToAsync<T1, T2, T3>(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, IObservable<Unit>> ToAsync<T1, T2, T3>(
      Action<T1, T2, T3> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, IObservable<Unit>>) ((first, second, third) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second, third);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return subject.AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, T3, T4, IObservable<Unit>> ToAsync<T1, T2, T3, T4>(
      Action<T1, T2, T3, T4> action)
    {
      return this.ToAsync<T1, T2, T3, T4>(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, IObservable<Unit>> ToAsync<T1, T2, T3, T4>(
      Action<T1, T2, T3, T4> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, IObservable<Unit>>) ((first, second, third, fourth) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second, third, fourth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return subject.AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5>(
      Action<T1, T2, T3, T4, T5> action)
    {
      return this.ToAsync<T1, T2, T3, T4, T5>(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5>(
      Action<T1, T2, T3, T4, T5> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, IObservable<Unit>>) ((first, second, third, fourth, fifth) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second, third, fourth, fifth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return subject.AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6>(
      Action<T1, T2, T3, T4, T5, T6> action)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6>(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6>(
      Action<T1, T2, T3, T4, T5, T6> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, IObservable<Unit>>) ((first, second, third, fourth, fifth, sixth) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second, third, fourth, fifth, sixth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return subject.AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7>(
      Action<T1, T2, T3, T4, T5, T6, T7> action)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7>(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7>(
      Action<T1, T2, T3, T4, T5, T6, T7> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, IObservable<Unit>>) ((first, second, third, fourth, fifth, sixth, seventh) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second, third, fourth, fifth, sixth, seventh);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return subject.AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8>(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<Unit>>) ((first, second, third, fourth, fifth, sixth, seventh, eight) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second, third, fourth, fifth, sixth, seventh, eight);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return subject.AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<Unit>>) ((first, second, third, fourth, fifth, sixth, seventh, eighth, ninth) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return subject.AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<Unit>>) ((first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return subject.AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<Unit>>) ((first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return subject.AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<Unit>>) ((first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return subject.AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<Unit>>) ((first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return subject.AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<Unit>>) ((first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return subject.AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IObservable<Unit>>) ((first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return subject.AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
      return this.ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
      Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IObservable<Unit>>) ((first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth, sixteenth) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth, sixteenth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return subject.AsObservable<Unit>();
      });
    }

    public virtual AsyncSubject<TSource> GetAwaiter<TSource>(IObservable<TSource> source)
    {
      return this.RunAsync<TSource>(source, CancellationToken.None);
    }

    public virtual AsyncSubject<TSource> GetAwaiter<TSource>(IConnectableObservable<TSource> source)
    {
      return this.RunAsync<TSource>(source, CancellationToken.None);
    }

    public virtual AsyncSubject<TSource> RunAsync<TSource>(
      IObservable<TSource> source,
      CancellationToken cancellationToken)
    {
      AsyncSubject<TSource> subject = new AsyncSubject<TSource>();
      if (cancellationToken.IsCancellationRequested)
        return QueryLanguage.Cancel<TSource>(subject, cancellationToken);
      IDisposable subscription = source.SubscribeSafe<TSource>((IObserver<TSource>) subject);
      if (cancellationToken.CanBeCanceled)
        QueryLanguage.RegisterCancelation<TSource>(subject, subscription, cancellationToken);
      return subject;
    }

    public virtual AsyncSubject<TSource> RunAsync<TSource>(
      IConnectableObservable<TSource> source,
      CancellationToken cancellationToken)
    {
      AsyncSubject<TSource> subject = new AsyncSubject<TSource>();
      if (cancellationToken.IsCancellationRequested)
        return QueryLanguage.Cancel<TSource>(subject, cancellationToken);
      IDisposable disposable1 = source.SubscribeSafe<TSource>((IObserver<TSource>) subject);
      IDisposable disposable2 = source.Connect();
      if (cancellationToken.CanBeCanceled)
        QueryLanguage.RegisterCancelation<TSource>(subject, (IDisposable) StableCompositeDisposable.Create(disposable1, disposable2), cancellationToken);
      return subject;
    }

    private static AsyncSubject<T> Cancel<T>(
      AsyncSubject<T> subject,
      CancellationToken cancellationToken)
    {
      subject.OnError((Exception) new OperationCanceledException(cancellationToken));
      return subject;
    }

    private static void RegisterCancelation<T>(
      AsyncSubject<T> subject,
      IDisposable subscription,
      CancellationToken token)
    {
      CancellationTokenRegistration ctr = token.Register((Action) (() =>
      {
        subscription.Dispose();
        QueryLanguage.Cancel<T>(subject, token);
      }));
      subject.Subscribe<T>(Stubs<T>.Ignore, (Action<Exception>) (_ => ctr.Dispose()), new Action(ctr.Dispose));
    }

    public virtual IConnectableObservable<TResult> Multicast<TSource, TResult>(
      IObservable<TSource> source,
      ISubject<TSource, TResult> subject)
    {
      return (IConnectableObservable<TResult>) new ConnectableObservable<TSource, TResult>(source, subject);
    }

    public virtual IObservable<TResult> Multicast<TSource, TIntermediate, TResult>(
      IObservable<TSource> source,
      Func<ISubject<TSource, TIntermediate>> subjectSelector,
      Func<IObservable<TIntermediate>, IObservable<TResult>> selector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Multicast<TSource, TIntermediate, TResult>(source, subjectSelector, selector);
    }

    public virtual IConnectableObservable<TSource> Publish<TSource>(IObservable<TSource> source)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new Subject<TSource>());
    }

    public virtual IObservable<TResult> Publish<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new Subject<TSource>()), selector);
    }

    public virtual IConnectableObservable<TSource> Publish<TSource>(
      IObservable<TSource> source,
      TSource initialValue)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new BehaviorSubject<TSource>(initialValue));
    }

    public virtual IObservable<TResult> Publish<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      TSource initialValue)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new BehaviorSubject<TSource>(initialValue)), selector);
    }

    public virtual IConnectableObservable<TSource> PublishLast<TSource>(IObservable<TSource> source)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new AsyncSubject<TSource>());
    }

    public virtual IObservable<TResult> PublishLast<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new AsyncSubject<TSource>()), selector);
    }

    public virtual IObservable<TSource> RefCount<TSource>(IConnectableObservable<TSource> source)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.RefCount<TSource>(source);
    }

    public virtual IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new ReplaySubject<TSource>());
    }

    public virtual IConnectableObservable<TSource> Replay<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new ReplaySubject<TSource>(scheduler));
    }

    public virtual IObservable<TResult> Replay<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new ReplaySubject<TSource>()), selector);
    }

    public virtual IObservable<TResult> Replay<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      IScheduler scheduler)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new ReplaySubject<TSource>(scheduler)), selector);
    }

    public virtual IConnectableObservable<TSource> Replay<TSource>(
      IObservable<TSource> source,
      TimeSpan window)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new ReplaySubject<TSource>(window));
    }

    public virtual IObservable<TResult> Replay<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      TimeSpan window)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new ReplaySubject<TSource>(window)), selector);
    }

    public virtual IConnectableObservable<TSource> Replay<TSource>(
      IObservable<TSource> source,
      TimeSpan window,
      IScheduler scheduler)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new ReplaySubject<TSource>(window, scheduler));
    }

    public virtual IObservable<TResult> Replay<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      TimeSpan window,
      IScheduler scheduler)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new ReplaySubject<TSource>(window, scheduler)), selector);
    }

    public virtual IConnectableObservable<TSource> Replay<TSource>(
      IObservable<TSource> source,
      int bufferSize,
      IScheduler scheduler)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new ReplaySubject<TSource>(bufferSize, scheduler));
    }

    public virtual IObservable<TResult> Replay<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      int bufferSize,
      IScheduler scheduler)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new ReplaySubject<TSource>(bufferSize, scheduler)), selector);
    }

    public virtual IConnectableObservable<TSource> Replay<TSource>(
      IObservable<TSource> source,
      int bufferSize)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new ReplaySubject<TSource>(bufferSize));
    }

    public virtual IObservable<TResult> Replay<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      int bufferSize)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new ReplaySubject<TSource>(bufferSize)), selector);
    }

    public virtual IConnectableObservable<TSource> Replay<TSource>(
      IObservable<TSource> source,
      int bufferSize,
      TimeSpan window)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new ReplaySubject<TSource>(bufferSize, window));
    }

    public virtual IObservable<TResult> Replay<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      int bufferSize,
      TimeSpan window)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new ReplaySubject<TSource>(bufferSize, window)), selector);
    }

    public virtual IConnectableObservable<TSource> Replay<TSource>(
      IObservable<TSource> source,
      int bufferSize,
      TimeSpan window,
      IScheduler scheduler)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new ReplaySubject<TSource>(bufferSize, window, scheduler));
    }

    public virtual IObservable<TResult> Replay<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      int bufferSize,
      TimeSpan window,
      IScheduler scheduler)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new ReplaySubject<TSource>(bufferSize, window, scheduler)), selector);
    }

    public virtual IEnumerable<IList<TSource>> Chunkify<TSource>(IObservable<TSource> source)
    {
      return source.Collect<TSource, IList<TSource>>((Func<IList<TSource>>) (() => (IList<TSource>) new List<TSource>()), (Func<IList<TSource>, TSource, IList<TSource>>) ((lst, x) =>
      {
        lst.Add(x);
        return lst;
      }), (Func<IList<TSource>, IList<TSource>>) (_ => (IList<TSource>) new List<TSource>()));
    }

    public virtual IEnumerable<TResult> Collect<TSource, TResult>(
      IObservable<TSource> source,
      Func<TResult> newCollector,
      Func<TResult, TSource, TResult> merge)
    {
      return QueryLanguage.Collect_<TSource, TResult>(source, newCollector, merge, (Func<TResult, TResult>) (_ => newCollector()));
    }

    public virtual IEnumerable<TResult> Collect<TSource, TResult>(
      IObservable<TSource> source,
      Func<TResult> getInitialCollector,
      Func<TResult, TSource, TResult> merge,
      Func<TResult, TResult> getNewCollector)
    {
      return QueryLanguage.Collect_<TSource, TResult>(source, getInitialCollector, merge, getNewCollector);
    }

    private static IEnumerable<TResult> Collect_<TSource, TResult>(
      IObservable<TSource> source,
      Func<TResult> getInitialCollector,
      Func<TResult, TSource, TResult> merge,
      Func<TResult, TResult> getNewCollector)
    {
      return (IEnumerable<TResult>) new System.Reactive.Linq.ObservableImpl.Collect<TSource, TResult>(source, getInitialCollector, merge, getNewCollector);
    }

    public virtual TSource First<TSource>(IObservable<TSource> source)
    {
      return QueryLanguage.FirstOrDefaultInternal<TSource>(source, true);
    }

    public virtual TSource First<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return this.First<TSource>(this.Where<TSource>(source, predicate));
    }

    public virtual TSource FirstOrDefault<TSource>(IObservable<TSource> source)
    {
      return QueryLanguage.FirstOrDefaultInternal<TSource>(source, false);
    }

    public virtual TSource FirstOrDefault<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return this.FirstOrDefault<TSource>(this.Where<TSource>(source, predicate));
    }

    private static TSource FirstOrDefaultInternal<TSource>(
      IObservable<TSource> source,
      bool throwOnEmpty)
    {
      TSource value = default (TSource);
      bool seenValue = false;
      Exception ex = (Exception) null;
      using (QueryLanguage.WaitAndSetOnce evt = new QueryLanguage.WaitAndSetOnce())
      {
        using (source.Subscribe((IObserver<TSource>) new AnonymousObserver<TSource>((Action<TSource>) (v =>
        {
          if (!seenValue)
            value = v;
          seenValue = true;
          evt.Set();
        }), (Action<Exception>) (e =>
        {
          ex = e;
          evt.Set();
        }), (Action) (() => evt.Set()))))
          evt.WaitOne();
      }
      ex.ThrowIfNotNull();
      if (throwOnEmpty && !seenValue)
        throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);
      return value;
    }

    public virtual void ForEach<TSource>(IObservable<TSource> source, Action<TSource> onNext)
    {
      using (QueryLanguage.WaitAndSetOnce evt = new QueryLanguage.WaitAndSetOnce())
      {
        System.Reactive.Linq.ObservableImpl.ForEach<TSource>._ obj = new System.Reactive.Linq.ObservableImpl.ForEach<TSource>._(onNext, (Action) (() => evt.Set()));
        using (source.SubscribeSafe<TSource>((IObserver<TSource>) obj))
          evt.WaitOne();
        obj.Error.ThrowIfNotNull();
      }
    }

    public virtual void ForEach<TSource>(IObservable<TSource> source, Action<TSource, int> onNext)
    {
      using (QueryLanguage.WaitAndSetOnce evt = new QueryLanguage.WaitAndSetOnce())
      {
        System.Reactive.Linq.ObservableImpl.ForEach<TSource>.ForEachImpl forEachImpl = new System.Reactive.Linq.ObservableImpl.ForEach<TSource>.ForEachImpl(onNext, (Action) (() => evt.Set()));
        using (source.SubscribeSafe<TSource>((IObserver<TSource>) forEachImpl))
          evt.WaitOne();
        forEachImpl.Error.ThrowIfNotNull();
      }
    }

    public virtual IEnumerator<TSource> GetEnumerator<TSource>(IObservable<TSource> source)
    {
      return new System.Reactive.Linq.ObservableImpl.GetEnumerator<TSource>().Run(source);
    }

    public virtual TSource Last<TSource>(IObservable<TSource> source)
    {
      return QueryLanguage.LastOrDefaultInternal<TSource>(source, true);
    }

    public virtual TSource Last<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
    {
      return this.Last<TSource>(this.Where<TSource>(source, predicate));
    }

    public virtual TSource LastOrDefault<TSource>(IObservable<TSource> source)
    {
      return QueryLanguage.LastOrDefaultInternal<TSource>(source, false);
    }

    public virtual TSource LastOrDefault<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return this.LastOrDefault<TSource>(this.Where<TSource>(source, predicate));
    }

    private static TSource LastOrDefaultInternal<TSource>(
      IObservable<TSource> source,
      bool throwOnEmpty)
    {
      TSource value = default (TSource);
      bool seenValue = false;
      Exception ex = (Exception) null;
      using (QueryLanguage.WaitAndSetOnce evt = new QueryLanguage.WaitAndSetOnce())
      {
        using (source.Subscribe((IObserver<TSource>) new AnonymousObserver<TSource>((Action<TSource>) (v =>
        {
          seenValue = true;
          value = v;
        }), (Action<Exception>) (e =>
        {
          ex = e;
          evt.Set();
        }), (Action) (() => evt.Set()))))
          evt.WaitOne();
      }
      ex.ThrowIfNotNull();
      if (throwOnEmpty && !seenValue)
        throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);
      return value;
    }

    public virtual IEnumerable<TSource> Latest<TSource>(IObservable<TSource> source)
    {
      return (IEnumerable<TSource>) new System.Reactive.Linq.ObservableImpl.Latest<TSource>(source);
    }

    public virtual IEnumerable<TSource> MostRecent<TSource>(
      IObservable<TSource> source,
      TSource initialValue)
    {
      return (IEnumerable<TSource>) new System.Reactive.Linq.ObservableImpl.MostRecent<TSource>(source, initialValue);
    }

    public virtual IEnumerable<TSource> Next<TSource>(IObservable<TSource> source)
    {
      return (IEnumerable<TSource>) new System.Reactive.Linq.ObservableImpl.Next<TSource>(source);
    }

    public virtual TSource Single<TSource>(IObservable<TSource> source)
    {
      return QueryLanguage.SingleOrDefaultInternal<TSource>(source, true);
    }

    public virtual TSource Single<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return this.Single<TSource>(this.Where<TSource>(source, predicate));
    }

    public virtual TSource SingleOrDefault<TSource>(IObservable<TSource> source)
    {
      return QueryLanguage.SingleOrDefaultInternal<TSource>(source, false);
    }

    public virtual TSource SingleOrDefault<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return this.SingleOrDefault<TSource>(this.Where<TSource>(source, predicate));
    }

    private static TSource SingleOrDefaultInternal<TSource>(
      IObservable<TSource> source,
      bool throwOnEmpty)
    {
      TSource value = default (TSource);
      bool seenValue = false;
      Exception ex = (Exception) null;
      using (QueryLanguage.WaitAndSetOnce evt = new QueryLanguage.WaitAndSetOnce())
      {
        using (source.Subscribe((IObserver<TSource>) new AnonymousObserver<TSource>((Action<TSource>) (v =>
        {
          if (seenValue)
          {
            ex = (Exception) new InvalidOperationException(Strings_Linq.MORE_THAN_ONE_ELEMENT);
            evt.Set();
          }
          value = v;
          seenValue = true;
        }), (Action<Exception>) (e =>
        {
          ex = e;
          evt.Set();
        }), (Action) (() => evt.Set()))))
          evt.WaitOne();
      }
      ex.ThrowIfNotNull();
      if (throwOnEmpty && !seenValue)
        throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);
      return value;
    }

    public virtual TSource Wait<TSource>(IObservable<TSource> source)
    {
      return QueryLanguage.LastOrDefaultInternal<TSource>(source, true);
    }

    public virtual IObservable<TSource> ObserveOn<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      return Synchronization.ObserveOn<TSource>(source, scheduler);
    }

    public virtual IObservable<TSource> ObserveOn<TSource>(
      IObservable<TSource> source,
      SynchronizationContext context)
    {
      return Synchronization.ObserveOn<TSource>(source, context);
    }

    public virtual IObservable<TSource> SubscribeOn<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      return Synchronization.SubscribeOn<TSource>(source, scheduler);
    }

    public virtual IObservable<TSource> SubscribeOn<TSource>(
      IObservable<TSource> source,
      SynchronizationContext context)
    {
      return Synchronization.SubscribeOn<TSource>(source, context);
    }

    public virtual IObservable<TSource> Synchronize<TSource>(IObservable<TSource> source)
    {
      return Synchronization.Synchronize<TSource>(source);
    }

    public virtual IObservable<TSource> Synchronize<TSource>(
      IObservable<TSource> source,
      object gate)
    {
      return Synchronization.Synchronize<TSource>(source, gate);
    }

    public virtual IDisposable Subscribe<TSource>(
      IEnumerable<TSource> source,
      IObserver<TSource> observer)
    {
      return QueryLanguage.Subscribe_<TSource>(source, observer, SchedulerDefaults.Iteration);
    }

    public virtual IDisposable Subscribe<TSource>(
      IEnumerable<TSource> source,
      IObserver<TSource> observer,
      IScheduler scheduler)
    {
      return QueryLanguage.Subscribe_<TSource>(source, observer, scheduler);
    }

    private static IDisposable Subscribe_<TSource>(
      IEnumerable<TSource> source,
      IObserver<TSource> observer,
      IScheduler scheduler)
    {
      return new System.Reactive.Linq.ObservableImpl.ToObservable<TSource>(source, scheduler).Subscribe(observer);
    }

    public virtual IEnumerable<TSource> ToEnumerable<TSource>(IObservable<TSource> source)
    {
      return (IEnumerable<TSource>) new AnonymousEnumerable<TSource>((Func<IEnumerator<TSource>>) (() => source.GetEnumerator<TSource>()));
    }

    public virtual IEventSource<Unit> ToEvent(IObservable<Unit> source)
    {
      return (IEventSource<Unit>) new EventSource<Unit>(source, (Action<Action<Unit>, Unit>) ((h, _) => h(Unit.Default)));
    }

    public virtual IEventSource<TSource> ToEvent<TSource>(IObservable<TSource> source)
    {
      return (IEventSource<TSource>) new EventSource<TSource>(source, (Action<Action<TSource>, TSource>) ((h, value) => h(value)));
    }

    public virtual IEventPatternSource<TEventArgs> ToEventPattern<TEventArgs>(
      IObservable<EventPattern<TEventArgs>> source)
    {
      return (IEventPatternSource<TEventArgs>) new EventPatternSource<TEventArgs>((IObservable<EventPattern<object, TEventArgs>>) source, (Action<Action<object, TEventArgs>, EventPattern<object, TEventArgs>>) ((h, evt) => h(evt.Sender, evt.EventArgs)));
    }

    public virtual IObservable<TSource> ToObservable<TSource>(IEnumerable<TSource> source)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.ToObservable<TSource>(source, SchedulerDefaults.Iteration);
    }

    public virtual IObservable<TSource> ToObservable<TSource>(
      IEnumerable<TSource> source,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.ToObservable<TSource>(source, scheduler);
    }

    public virtual IObservable<TSource> Create<TSource>(
      Func<IObserver<TSource>, IDisposable> subscribe)
    {
      return (IObservable<TSource>) new AnonymousObservable<TSource>(subscribe);
    }

    public virtual IObservable<TSource> Create<TSource>(Func<IObserver<TSource>, Action> subscribe)
    {
      return (IObservable<TSource>) new AnonymousObservable<TSource>((Func<IObserver<TSource>, IDisposable>) (o =>
      {
        Action dispose = subscribe(o);
        return dispose == null ? Disposable.Empty : Disposable.Create(dispose);
      }));
    }

    public virtual IObservable<TResult> Create<TResult>(
      Func<IObserver<TResult>, CancellationToken, Task> subscribeAsync)
    {
      return (IObservable<TResult>) new AnonymousObservable<TResult>((Func<IObserver<TResult>, IDisposable>) (observer =>
      {
        CancellationDisposable disposable1 = new CancellationDisposable();
        IDisposable disposable2 = subscribeAsync(observer, disposable1.Token).ToObservable().Subscribe((IObserver<Unit>) new AnonymousObserver<Unit>(Stubs<Unit>.Ignore, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted)));
        return (IDisposable) StableCompositeDisposable.Create((IDisposable) disposable1, disposable2);
      }));
    }

    public virtual IObservable<TResult> Create<TResult>(
      Func<IObserver<TResult>, Task> subscribeAsync)
    {
      return this.Create<TResult>((Func<IObserver<TResult>, CancellationToken, Task>) ((observer, token) => subscribeAsync(observer)));
    }

    public virtual IObservable<TResult> Create<TResult>(
      Func<IObserver<TResult>, CancellationToken, Task<IDisposable>> subscribeAsync)
    {
      return (IObservable<TResult>) new AnonymousObservable<TResult>((Func<IObserver<TResult>, IDisposable>) (observer =>
      {
        SingleAssignmentDisposable subscription = new SingleAssignmentDisposable();
        CancellationDisposable disposable1 = new CancellationDisposable();
        subscribeAsync(observer, disposable1.Token).ToObservable<IDisposable>().Subscribe((IObserver<IDisposable>) new AnonymousObserver<IDisposable>((Action<IDisposable>) (d => subscription.Disposable = d ?? Disposable.Empty), new Action<Exception>(observer.OnError), Stubs.Nop));
        return (IDisposable) StableCompositeDisposable.Create((IDisposable) disposable1, (IDisposable) subscription);
      }));
    }

    public virtual IObservable<TResult> Create<TResult>(
      Func<IObserver<TResult>, Task<IDisposable>> subscribeAsync)
    {
      return this.Create<TResult>((Func<IObserver<TResult>, CancellationToken, Task<IDisposable>>) ((observer, token) => subscribeAsync(observer)));
    }

    public virtual IObservable<TResult> Create<TResult>(
      Func<IObserver<TResult>, CancellationToken, Task<Action>> subscribeAsync)
    {
      return (IObservable<TResult>) new AnonymousObservable<TResult>((Func<IObserver<TResult>, IDisposable>) (observer =>
      {
        SingleAssignmentDisposable subscription = new SingleAssignmentDisposable();
        CancellationDisposable disposable1 = new CancellationDisposable();
        subscribeAsync(observer, disposable1.Token).ToObservable<Action>().Subscribe((IObserver<Action>) new AnonymousObserver<Action>((Action<Action>) (a => subscription.Disposable = a != null ? Disposable.Create(a) : Disposable.Empty), new Action<Exception>(observer.OnError), Stubs.Nop));
        return (IDisposable) StableCompositeDisposable.Create((IDisposable) disposable1, (IDisposable) subscription);
      }));
    }

    public virtual IObservable<TResult> Create<TResult>(
      Func<IObserver<TResult>, Task<Action>> subscribeAsync)
    {
      return this.Create<TResult>((Func<IObserver<TResult>, CancellationToken, Task<Action>>) ((observer, token) => subscribeAsync(observer)));
    }

    public virtual IObservable<TValue> Defer<TValue>(Func<IObservable<TValue>> observableFactory)
    {
      return (IObservable<TValue>) new System.Reactive.Linq.ObservableImpl.Defer<TValue>(observableFactory);
    }

    public virtual IObservable<TValue> Defer<TValue>(
      Func<Task<IObservable<TValue>>> observableFactoryAsync)
    {
      return this.Defer<TValue>((Func<IObservable<TValue>>) (() => this.StartAsync<IObservable<TValue>>(observableFactoryAsync).Merge<TValue>()));
    }

    public virtual IObservable<TValue> Defer<TValue>(
      Func<CancellationToken, Task<IObservable<TValue>>> observableFactoryAsync)
    {
      return this.Defer<TValue>((Func<IObservable<TValue>>) (() => this.StartAsync<IObservable<TValue>>(observableFactoryAsync).Merge<TValue>()));
    }

    public virtual IObservable<TResult> Empty<TResult>()
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Empty<TResult>(SchedulerDefaults.ConstantTimeOperations);
    }

    public virtual IObservable<TResult> Empty<TResult>(IScheduler scheduler)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Empty<TResult>(scheduler);
    }

    public virtual IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Generate<TState, TResult>(initialState, condition, iterate, resultSelector, SchedulerDefaults.Iteration);
    }

    public virtual IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      IScheduler scheduler)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Generate<TState, TResult>(initialState, condition, iterate, resultSelector, scheduler);
    }

    public virtual IObservable<TResult> Never<TResult>()
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Never<TResult>();
    }

    public virtual IObservable<int> Range(int start, int count)
    {
      return QueryLanguage.Range_(start, count, SchedulerDefaults.Iteration);
    }

    public virtual IObservable<int> Range(int start, int count, IScheduler scheduler)
    {
      return QueryLanguage.Range_(start, count, scheduler);
    }

    private static IObservable<int> Range_(int start, int count, IScheduler scheduler)
    {
      return (IObservable<int>) new System.Reactive.Linq.ObservableImpl.Range(start, count, scheduler);
    }

    public virtual IObservable<TResult> Repeat<TResult>(TResult value)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Repeat<TResult>(value, new int?(), SchedulerDefaults.Iteration);
    }

    public virtual IObservable<TResult> Repeat<TResult>(TResult value, IScheduler scheduler)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Repeat<TResult>(value, new int?(), scheduler);
    }

    public virtual IObservable<TResult> Repeat<TResult>(TResult value, int repeatCount)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Repeat<TResult>(value, new int?(repeatCount), SchedulerDefaults.Iteration);
    }

    public virtual IObservable<TResult> Repeat<TResult>(
      TResult value,
      int repeatCount,
      IScheduler scheduler)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Repeat<TResult>(value, new int?(repeatCount), scheduler);
    }

    public virtual IObservable<TResult> Return<TResult>(TResult value)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Return<TResult>(value, SchedulerDefaults.ConstantTimeOperations);
    }

    public virtual IObservable<TResult> Return<TResult>(TResult value, IScheduler scheduler)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Return<TResult>(value, scheduler);
    }

    public virtual IObservable<TResult> Throw<TResult>(Exception exception)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Throw<TResult>(exception, SchedulerDefaults.ConstantTimeOperations);
    }

    public virtual IObservable<TResult> Throw<TResult>(Exception exception, IScheduler scheduler)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Throw<TResult>(exception, scheduler);
    }

    public virtual IObservable<TSource> Using<TSource, TResource>(
      Func<TResource> resourceFactory,
      Func<TResource, IObservable<TSource>> observableFactory)
      where TResource : IDisposable
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Using<TSource, TResource>(resourceFactory, observableFactory);
    }

    public virtual IObservable<TSource> Using<TSource, TResource>(
      Func<CancellationToken, Task<TResource>> resourceFactoryAsync,
      Func<TResource, CancellationToken, Task<IObservable<TSource>>> observableFactoryAsync)
      where TResource : IDisposable
    {
      return Observable.FromAsync<TResource>(resourceFactoryAsync).SelectMany<TResource, TSource>((Func<TResource, IObservable<TSource>>) (resource => Observable.Using<TSource, TResource>((Func<TResource>) (() => resource), (Func<TResource, IObservable<TSource>>) (resource_ => Observable.FromAsync<IObservable<TSource>>((Func<CancellationToken, Task<IObservable<TSource>>>) (ct => observableFactoryAsync(resource_, ct))).Merge<TSource>()))));
    }

    public virtual IObservable<EventPattern<object>> FromEventPattern(
      Action<EventHandler> addHandler,
      Action<EventHandler> removeHandler)
    {
      return QueryLanguage.FromEventPattern_(addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<object>> FromEventPattern(
      Action<EventHandler> addHandler,
      Action<EventHandler> removeHandler,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_(addHandler, removeHandler, scheduler);
    }

    private static IObservable<EventPattern<object>> FromEventPattern_(
      Action<EventHandler> addHandler,
      Action<EventHandler> removeHandler,
      IScheduler scheduler)
    {
      return (IObservable<EventPattern<object>>) new System.Reactive.Linq.ObservableImpl.FromEventPattern.Impl<EventHandler, object>((Func<EventHandler<object>, EventHandler>) (e => new EventHandler(e.Invoke)), addHandler, removeHandler, scheduler);
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler)
    {
      return QueryLanguage.FromEventPattern_<TDelegate, TEventArgs>(addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);
    }

    private static IObservable<EventPattern<TEventArgs>> FromEventPattern_<TDelegate, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      return (IObservable<EventPattern<TEventArgs>>) new System.Reactive.Linq.ObservableImpl.FromEventPattern.Impl<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(
      Func<EventHandler<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler)
    {
      return QueryLanguage.FromEventPattern_<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(
      Func<EventHandler<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, scheduler);
    }

    private static IObservable<EventPattern<TEventArgs>> FromEventPattern_<TDelegate, TEventArgs>(
      Func<EventHandler<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      return (IObservable<EventPattern<TEventArgs>>) new System.Reactive.Linq.ObservableImpl.FromEventPattern.Impl<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, scheduler);
    }

    public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler)
    {
      return QueryLanguage.FromEventPattern_<TDelegate, TSender, TEventArgs>(addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_<TDelegate, TSender, TEventArgs>(addHandler, removeHandler, scheduler);
    }

    private static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern_<TDelegate, TSender, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      return (IObservable<EventPattern<TSender, TEventArgs>>) new System.Reactive.Linq.ObservableImpl.FromEventPattern.Impl<TDelegate, TSender, TEventArgs>(addHandler, removeHandler, scheduler);
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      Action<EventHandler<TEventArgs>> addHandler,
      Action<EventHandler<TEventArgs>> removeHandler)
    {
      return QueryLanguage.FromEventPattern_<TEventArgs>(addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      Action<EventHandler<TEventArgs>> addHandler,
      Action<EventHandler<TEventArgs>> removeHandler,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_<TEventArgs>(addHandler, removeHandler, scheduler);
    }

    private static IObservable<EventPattern<TEventArgs>> FromEventPattern_<TEventArgs>(
      Action<EventHandler<TEventArgs>> addHandler,
      Action<EventHandler<TEventArgs>> removeHandler,
      IScheduler scheduler)
    {
      return (IObservable<EventPattern<TEventArgs>>) new System.Reactive.Linq.ObservableImpl.FromEventPattern.Impl<EventHandler<TEventArgs>, TEventArgs>((Func<EventHandler<TEventArgs>, EventHandler<TEventArgs>>) (handler => handler), addHandler, removeHandler, scheduler);
    }

    public virtual IObservable<EventPattern<object>> FromEventPattern(
      object target,
      string eventName)
    {
      return QueryLanguage.FromEventPattern_(target, eventName, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<object>> FromEventPattern(
      object target,
      string eventName,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_(target, eventName, scheduler);
    }

    private static IObservable<EventPattern<object>> FromEventPattern_(
      object target,
      string eventName,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_<object, object, EventPattern<object>>(target.GetType(), target, eventName, (Func<object, object, EventPattern<object>>) ((sender, args) => new EventPattern<object>(sender, args)), scheduler);
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      object target,
      string eventName)
    {
      return QueryLanguage.FromEventPattern_<TEventArgs>(target, eventName, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      object target,
      string eventName,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_<TEventArgs>(target, eventName, scheduler);
    }

    private static IObservable<EventPattern<TEventArgs>> FromEventPattern_<TEventArgs>(
      object target,
      string eventName,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_<object, TEventArgs, EventPattern<TEventArgs>>(target.GetType(), target, eventName, (Func<object, TEventArgs, EventPattern<TEventArgs>>) ((sender, args) => new EventPattern<TEventArgs>(sender, args)), scheduler);
    }

    public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(
      object target,
      string eventName)
    {
      return QueryLanguage.FromEventPattern_<TSender, TEventArgs>(target, eventName, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(
      object target,
      string eventName,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_<TSender, TEventArgs>(target, eventName, scheduler);
    }

    private static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern_<TSender, TEventArgs>(
      object target,
      string eventName,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_<TSender, TEventArgs, EventPattern<TSender, TEventArgs>>(target.GetType(), target, eventName, (Func<TSender, TEventArgs, EventPattern<TSender, TEventArgs>>) ((sender, args) => new EventPattern<TSender, TEventArgs>(sender, args)), scheduler);
    }

    public virtual IObservable<EventPattern<object>> FromEventPattern(Type type, string eventName)
    {
      return QueryLanguage.FromEventPattern_(type, eventName, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<object>> FromEventPattern(
      Type type,
      string eventName,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_(type, eventName, scheduler);
    }

    private static IObservable<EventPattern<object>> FromEventPattern_(
      Type type,
      string eventName,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_<object, object, EventPattern<object>>(type, (object) null, eventName, (Func<object, object, EventPattern<object>>) ((sender, args) => new EventPattern<object>(sender, args)), scheduler);
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      Type type,
      string eventName)
    {
      return QueryLanguage.FromEventPattern_<TEventArgs>(type, eventName, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      Type type,
      string eventName,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_<TEventArgs>(type, eventName, scheduler);
    }

    private static IObservable<EventPattern<TEventArgs>> FromEventPattern_<TEventArgs>(
      Type type,
      string eventName,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_<object, TEventArgs, EventPattern<TEventArgs>>(type, (object) null, eventName, (Func<object, TEventArgs, EventPattern<TEventArgs>>) ((sender, args) => new EventPattern<TEventArgs>(sender, args)), scheduler);
    }

    public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(
      Type type,
      string eventName)
    {
      return QueryLanguage.FromEventPattern_<TSender, TEventArgs>(type, eventName, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(
      Type type,
      string eventName,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_<TSender, TEventArgs>(type, eventName, scheduler);
    }

    private static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern_<TSender, TEventArgs>(
      Type type,
      string eventName,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_<TSender, TEventArgs, EventPattern<TSender, TEventArgs>>(type, (object) null, eventName, (Func<TSender, TEventArgs, EventPattern<TSender, TEventArgs>>) ((sender, args) => new EventPattern<TSender, TEventArgs>(sender, args)), scheduler);
    }

    private static IObservable<TResult> FromEventPattern_<TSender, TEventArgs, TResult>(
      Type targetType,
      object target,
      string eventName,
      Func<TSender, TEventArgs, TResult> getResult,
      IScheduler scheduler)
    {
      MethodInfo addMethod = (MethodInfo) null;
      MethodInfo removeMethod = (MethodInfo) null;
      Type delegateType = (Type) null;
      bool isWinRT = false;
      ReflectionUtils.GetEventMethods<TSender, TEventArgs>(targetType, target, eventName, out addMethod, out removeMethod, out delegateType, out isWinRT);
      return isWinRT ? (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.FromEventPattern.Handler<TSender, TEventArgs, TResult>(target, delegateType, addMethod, removeMethod, getResult, true, scheduler) : (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.FromEventPattern.Handler<TSender, TEventArgs, TResult>(target, delegateType, addMethod, removeMethod, getResult, false, scheduler);
    }

    public virtual IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(
      Func<Action<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler)
    {
      return QueryLanguage.FromEvent_<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(
      Func<Action<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEvent_<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, scheduler);
    }

    private static IObservable<TEventArgs> FromEvent_<TDelegate, TEventArgs>(
      Func<Action<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      return (IObservable<TEventArgs>) new System.Reactive.Linq.ObservableImpl.FromEvent<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, scheduler);
    }

    public virtual IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler)
    {
      return QueryLanguage.FromEvent_<TDelegate, TEventArgs>(addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEvent_<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);
    }

    private static IObservable<TEventArgs> FromEvent_<TDelegate, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      return (IObservable<TEventArgs>) new System.Reactive.Linq.ObservableImpl.FromEvent<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);
    }

    public virtual IObservable<TEventArgs> FromEvent<TEventArgs>(
      Action<Action<TEventArgs>> addHandler,
      Action<Action<TEventArgs>> removeHandler)
    {
      return QueryLanguage.FromEvent_<TEventArgs>(addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<TEventArgs> FromEvent<TEventArgs>(
      Action<Action<TEventArgs>> addHandler,
      Action<Action<TEventArgs>> removeHandler,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEvent_<TEventArgs>(addHandler, removeHandler, scheduler);
    }

    private static IObservable<TEventArgs> FromEvent_<TEventArgs>(
      Action<Action<TEventArgs>> addHandler,
      Action<Action<TEventArgs>> removeHandler,
      IScheduler scheduler)
    {
      return (IObservable<TEventArgs>) new System.Reactive.Linq.ObservableImpl.FromEvent<Action<TEventArgs>, TEventArgs>((Func<Action<TEventArgs>, Action<TEventArgs>>) (h => h), addHandler, removeHandler, scheduler);
    }

    public virtual IObservable<Unit> FromEvent(
      Action<Action> addHandler,
      Action<Action> removeHandler)
    {
      return QueryLanguage.FromEvent_(addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<Unit> FromEvent(
      Action<Action> addHandler,
      Action<Action> removeHandler,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEvent_(addHandler, removeHandler, scheduler);
    }

    private static IObservable<Unit> FromEvent_(
      Action<Action> addHandler,
      Action<Action> removeHandler,
      IScheduler scheduler)
    {
      return (IObservable<Unit>) new System.Reactive.Linq.ObservableImpl.FromEvent<Action, Unit>((Func<Action<Unit>, Action>) (h => (Action) (() => h(new Unit()))), addHandler, removeHandler, scheduler);
    }

    private static IScheduler GetSchedulerForCurrentContext()
    {
      SynchronizationContext current = SynchronizationContext.Current;
      return current != null ? (IScheduler) new SynchronizationContextScheduler(current, false) : SchedulerDefaults.ConstantTimeOperations;
    }

    public virtual Task ForEachAsync<TSource>(IObservable<TSource> source, Action<TSource> onNext)
    {
      return QueryLanguage.ForEachAsync_<TSource>(source, onNext, CancellationToken.None);
    }

    public virtual Task ForEachAsync<TSource>(
      IObservable<TSource> source,
      Action<TSource> onNext,
      CancellationToken cancellationToken)
    {
      return QueryLanguage.ForEachAsync_<TSource>(source, onNext, cancellationToken);
    }

    public virtual Task ForEachAsync<TSource>(
      IObservable<TSource> source,
      Action<TSource, int> onNext)
    {
      int i = 0;
      return QueryLanguage.ForEachAsync_<TSource>(source, (Action<TSource>) (x => onNext(x, checked (i++))), CancellationToken.None);
    }

    public virtual Task ForEachAsync<TSource>(
      IObservable<TSource> source,
      Action<TSource, int> onNext,
      CancellationToken cancellationToken)
    {
      int i = 0;
      return QueryLanguage.ForEachAsync_<TSource>(source, (Action<TSource>) (x => onNext(x, checked (i++))), cancellationToken);
    }

    private static Task ForEachAsync_<TSource>(
      IObservable<TSource> source,
      Action<TSource> onNext,
      CancellationToken cancellationToken)
    {
      TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
      SingleAssignmentDisposable subscription = new SingleAssignmentDisposable();
      CancellationTokenRegistration ctr = new CancellationTokenRegistration();
      if (cancellationToken.CanBeCanceled)
        ctr = cancellationToken.Register((Action) (() =>
        {
          tcs.TrySetCanceled<object>(cancellationToken);
          subscription.Dispose();
        }));
      if (!cancellationToken.IsCancellationRequested)
      {
        Action<Action> dispose = (Action<Action>) (action =>
        {
          try
          {
            ctr.Dispose();
            subscription.Dispose();
          }
          catch (Exception ex)
          {
            tcs.TrySetException(ex);
            return;
          }
          action();
        });
        AnonymousObserver<TSource> anonymousObserver = new AnonymousObserver<TSource>((Action<TSource>) (x =>
        {
          if (subscription.IsDisposed)
            return;
          try
          {
            onNext(x);
          }
          catch (Exception ex)
          {
            dispose((Action) (() => tcs.TrySetException(ex)));
          }
        }), (Action<Exception>) (exception => dispose((Action) (() => tcs.TrySetException(exception)))), (Action) (() => dispose((Action) (() => tcs.TrySetResult((object) null)))));
        try
        {
          subscription.Disposable = source.Subscribe((IObserver<TSource>) anonymousObserver);
        }
        catch (Exception ex)
        {
          tcs.TrySetException(ex);
        }
      }
      return (Task) tcs.Task;
    }

    public virtual IObservable<TResult> Case<TValue, TResult>(
      Func<TValue> selector,
      IDictionary<TValue, IObservable<TResult>> sources)
    {
      return this.Case<TValue, TResult>(selector, sources, this.Empty<TResult>());
    }

    public virtual IObservable<TResult> Case<TValue, TResult>(
      Func<TValue> selector,
      IDictionary<TValue, IObservable<TResult>> sources,
      IScheduler scheduler)
    {
      return this.Case<TValue, TResult>(selector, sources, this.Empty<TResult>(scheduler));
    }

    public virtual IObservable<TResult> Case<TValue, TResult>(
      Func<TValue> selector,
      IDictionary<TValue, IObservable<TResult>> sources,
      IObservable<TResult> defaultSource)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Case<TValue, TResult>(selector, sources, defaultSource);
    }

    public virtual IObservable<TSource> DoWhile<TSource>(
      IObservable<TSource> source,
      Func<bool> condition)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.DoWhile<TSource>(source, condition);
    }

    public virtual IObservable<TResult> For<TSource, TResult>(
      IEnumerable<TSource> source,
      Func<TSource, IObservable<TResult>> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.For<TSource, TResult>(source, resultSelector);
    }

    public virtual IObservable<TResult> If<TResult>(
      Func<bool> condition,
      IObservable<TResult> thenSource)
    {
      return this.If<TResult>(condition, thenSource, this.Empty<TResult>());
    }

    public virtual IObservable<TResult> If<TResult>(
      Func<bool> condition,
      IObservable<TResult> thenSource,
      IScheduler scheduler)
    {
      return this.If<TResult>(condition, thenSource, this.Empty<TResult>(scheduler));
    }

    public virtual IObservable<TResult> If<TResult>(
      Func<bool> condition,
      IObservable<TResult> thenSource,
      IObservable<TResult> elseSource)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.If<TResult>(condition, thenSource, elseSource);
    }

    public virtual IObservable<TSource> While<TSource>(
      Func<bool> condition,
      IObservable<TSource> source)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.While<TSource>(condition, source);
    }

    public virtual Pattern<TLeft, TRight> And<TLeft, TRight>(
      IObservable<TLeft> left,
      IObservable<TRight> right)
    {
      return new Pattern<TLeft, TRight>(left, right);
    }

    public virtual Plan<TResult> Then<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, TResult> selector)
    {
      return new Pattern<TSource>(source).Then<TResult>(selector);
    }

    public virtual IObservable<TResult> When<TResult>(params Plan<TResult>[] plans)
    {
      return this.When<TResult>((IEnumerable<Plan<TResult>>) plans);
    }

    public virtual IObservable<TResult> When<TResult>(IEnumerable<Plan<TResult>> plans)
    {
      return (IObservable<TResult>) new AnonymousObservable<TResult>((Func<IObserver<TResult>, IDisposable>) (observer =>
      {
        Dictionary<object, IJoinObserver> externalSubscriptions = new Dictionary<object, IJoinObserver>();
        object gate = new object();
        List<ActivePlan> activePlans = new List<ActivePlan>();
        IObserver<TResult> outObserver = Observer.Create<TResult>(new Action<TResult>(observer.OnNext), (Action<Exception>) (exception =>
        {
          foreach (IDisposable disposable in externalSubscriptions.Values)
            disposable.Dispose();
          observer.OnError(exception);
        }), new Action(observer.OnCompleted));
        try
        {
          foreach (Plan<TResult> plan in plans)
            activePlans.Add(plan.Activate(externalSubscriptions, outObserver, (Action<ActivePlan>) (activePlan =>
            {
              activePlans.Remove(activePlan);
              if (activePlans.Count != 0)
                return;
              outObserver.OnCompleted();
            })));
        }
        catch (Exception ex)
        {
          return this.Throw<TResult>(ex).Subscribe(observer);
        }
        CompositeDisposable compositeDisposable = new CompositeDisposable(externalSubscriptions.Values.Count);
        foreach (IJoinObserver joinObserver in externalSubscriptions.Values)
        {
          joinObserver.Subscribe(gate);
          compositeDisposable.Add((IDisposable) joinObserver);
        }
        return (IDisposable) compositeDisposable;
      }));
    }

    public virtual IObservable<TSource> Amb<TSource>(
      IObservable<TSource> first,
      IObservable<TSource> second)
    {
      return QueryLanguage.Amb_<TSource>(first, second);
    }

    public virtual IObservable<TSource> Amb<TSource>(params IObservable<TSource>[] sources)
    {
      return QueryLanguage.Amb_<TSource>((IEnumerable<IObservable<TSource>>) sources);
    }

    public virtual IObservable<TSource> Amb<TSource>(IEnumerable<IObservable<TSource>> sources)
    {
      return QueryLanguage.Amb_<TSource>(sources);
    }

    private static IObservable<TSource> Amb_<TSource>(IEnumerable<IObservable<TSource>> sources)
    {
      return sources.Aggregate<IObservable<TSource>, IObservable<TSource>>(Observable.Never<TSource>(), (Func<IObservable<TSource>, IObservable<TSource>, IObservable<TSource>>) ((previous, current) => previous.Amb<TSource>(current)));
    }

    private static IObservable<TSource> Amb_<TSource>(
      IObservable<TSource> leftSource,
      IObservable<TSource> rightSource)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Amb<TSource>(leftSource, rightSource);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource, TBufferClosing>(
      IObservable<TSource> source,
      Func<IObservable<TBufferClosing>> bufferClosingSelector)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.ObservableImpl.Buffer<TSource, TBufferClosing>(source, bufferClosingSelector);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource, TBufferOpening, TBufferClosing>(
      IObservable<TSource> source,
      IObservable<TBufferOpening> bufferOpenings,
      Func<TBufferOpening, IObservable<TBufferClosing>> bufferClosingSelector)
    {
      return source.Window<TSource, TBufferOpening, TBufferClosing>(bufferOpenings, bufferClosingSelector).SelectMany<IObservable<TSource>, IList<TSource>>(new Func<IObservable<TSource>, IObservable<IList<TSource>>>(this.ToList<TSource>));
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource, TBufferBoundary>(
      IObservable<TSource> source,
      IObservable<TBufferBoundary> bufferBoundaries)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.ObservableImpl.Buffer<TSource, TBufferBoundary>(source, bufferBoundaries);
    }

    public virtual IObservable<TSource> Catch<TSource, TException>(
      IObservable<TSource> source,
      Func<TException, IObservable<TSource>> handler)
      where TException : Exception
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Catch<TSource, TException>(source, handler);
    }

    public virtual IObservable<TSource> Catch<TSource>(
      IObservable<TSource> first,
      IObservable<TSource> second)
    {
      return QueryLanguage.Catch_<TSource>((IEnumerable<IObservable<TSource>>) new IObservable<TSource>[2]
      {
        first,
        second
      });
    }

    public virtual IObservable<TSource> Catch<TSource>(params IObservable<TSource>[] sources)
    {
      return QueryLanguage.Catch_<TSource>((IEnumerable<IObservable<TSource>>) sources);
    }

    public virtual IObservable<TSource> Catch<TSource>(IEnumerable<IObservable<TSource>> sources)
    {
      return QueryLanguage.Catch_<TSource>(sources);
    }

    private static IObservable<TSource> Catch_<TSource>(IEnumerable<IObservable<TSource>> sources)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Catch<TSource>(sources);
    }

    public virtual IObservable<TResult> CombineLatest<TFirst, TSecond, TResult>(
      IObservable<TFirst> first,
      IObservable<TSecond> second,
      Func<TFirst, TSecond, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.CombineLatest<TFirst, TSecond, TResult>(first, second, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TResult>(
      IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      Func<TSource1, TSource2, TSource3, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.CombineLatest<TSource1, TSource2, TSource3, TResult>(source1, source2, source3, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(
      IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      Func<TSource1, TSource2, TSource3, TSource4, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(source1, source2, source3, source4, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(
      IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(source1, source2, source3, source4, source5, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(
      IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(source1, source2, source3, source4, source5, source6, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(
      IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(source1, source2, source3, source4, source5, source6, source7, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(
      IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(
      IObservable<TSource1> source1,
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
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(
      IObservable<TSource1> source1,
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
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(
      IObservable<TSource1> source1,
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
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(
      IObservable<TSource1> source1,
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
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(
      IObservable<TSource1> source1,
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
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(
      IObservable<TSource1> source1,
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
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(
      IObservable<TSource1> source1,
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
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(
      IObservable<TSource1> source1,
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
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, source16, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource, TResult>(
      IEnumerable<IObservable<TSource>> sources,
      Func<IList<TSource>, TResult> resultSelector)
    {
      return QueryLanguage.CombineLatest_<TSource, TResult>(sources, resultSelector);
    }

    public virtual IObservable<IList<TSource>> CombineLatest<TSource>(
      IEnumerable<IObservable<TSource>> sources)
    {
      return QueryLanguage.CombineLatest_<TSource, IList<TSource>>(sources, (Func<IList<TSource>, IList<TSource>>) (res => (IList<TSource>) res.ToList<TSource>()));
    }

    public virtual IObservable<IList<TSource>> CombineLatest<TSource>(
      params IObservable<TSource>[] sources)
    {
      return QueryLanguage.CombineLatest_<TSource, IList<TSource>>((IEnumerable<IObservable<TSource>>) sources, (Func<IList<TSource>, IList<TSource>>) (res => (IList<TSource>) res.ToList<TSource>()));
    }

    private static IObservable<TResult> CombineLatest_<TSource, TResult>(
      IEnumerable<IObservable<TSource>> sources,
      Func<IList<TSource>, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.CombineLatest<TSource, TResult>(sources, resultSelector);
    }

    public virtual IObservable<TSource> Concat<TSource>(
      IObservable<TSource> first,
      IObservable<TSource> second)
    {
      return QueryLanguage.Concat_<TSource>((IEnumerable<IObservable<TSource>>) new IObservable<TSource>[2]
      {
        first,
        second
      });
    }

    public virtual IObservable<TSource> Concat<TSource>(params IObservable<TSource>[] sources)
    {
      return QueryLanguage.Concat_<TSource>((IEnumerable<IObservable<TSource>>) sources);
    }

    public virtual IObservable<TSource> Concat<TSource>(IEnumerable<IObservable<TSource>> sources)
    {
      return QueryLanguage.Concat_<TSource>(sources);
    }

    private static IObservable<TSource> Concat_<TSource>(IEnumerable<IObservable<TSource>> sources)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Concat<TSource>(sources);
    }

    public virtual IObservable<TSource> Concat<TSource>(IObservable<IObservable<TSource>> sources)
    {
      return this.Concat_<TSource>(sources);
    }

    public virtual IObservable<TSource> Concat<TSource>(IObservable<Task<TSource>> sources)
    {
      return this.Concat_<TSource>(this.Select<Task<TSource>, IObservable<TSource>>(sources, new Func<Task<TSource>, IObservable<TSource>>(TaskObservableExtensions.ToObservable<TSource>)));
    }

    private IObservable<TSource> Concat_<TSource>(IObservable<IObservable<TSource>> sources)
    {
      return this.Merge<TSource>(sources, 1);
    }

    public virtual IObservable<TSource> Merge<TSource>(IObservable<IObservable<TSource>> sources)
    {
      return QueryLanguage.Merge_<TSource>(sources);
    }

    public virtual IObservable<TSource> Merge<TSource>(IObservable<Task<TSource>> sources)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Merge<TSource>(sources);
    }

    public virtual IObservable<TSource> Merge<TSource>(
      IObservable<IObservable<TSource>> sources,
      int maxConcurrent)
    {
      return QueryLanguage.Merge_<TSource>(sources, maxConcurrent);
    }

    public virtual IObservable<TSource> Merge<TSource>(
      IEnumerable<IObservable<TSource>> sources,
      int maxConcurrent)
    {
      return QueryLanguage.Merge_<TSource>(sources.ToObservable<IObservable<TSource>>(SchedulerDefaults.ConstantTimeOperations), maxConcurrent);
    }

    public virtual IObservable<TSource> Merge<TSource>(
      IEnumerable<IObservable<TSource>> sources,
      int maxConcurrent,
      IScheduler scheduler)
    {
      return QueryLanguage.Merge_<TSource>(sources.ToObservable<IObservable<TSource>>(scheduler), maxConcurrent);
    }

    public virtual IObservable<TSource> Merge<TSource>(
      IObservable<TSource> first,
      IObservable<TSource> second)
    {
      return QueryLanguage.Merge_<TSource>(((IEnumerable<IObservable<TSource>>) new IObservable<TSource>[2]
      {
        first,
        second
      }).ToObservable<IObservable<TSource>>(SchedulerDefaults.ConstantTimeOperations));
    }

    public virtual IObservable<TSource> Merge<TSource>(
      IObservable<TSource> first,
      IObservable<TSource> second,
      IScheduler scheduler)
    {
      return QueryLanguage.Merge_<TSource>(((IEnumerable<IObservable<TSource>>) new IObservable<TSource>[2]
      {
        first,
        second
      }).ToObservable<IObservable<TSource>>(scheduler));
    }

    public virtual IObservable<TSource> Merge<TSource>(params IObservable<TSource>[] sources)
    {
      return QueryLanguage.Merge_<TSource>(((IEnumerable<IObservable<TSource>>) sources).ToObservable<IObservable<TSource>>(SchedulerDefaults.ConstantTimeOperations));
    }

    public virtual IObservable<TSource> Merge<TSource>(
      IScheduler scheduler,
      params IObservable<TSource>[] sources)
    {
      return QueryLanguage.Merge_<TSource>(((IEnumerable<IObservable<TSource>>) sources).ToObservable<IObservable<TSource>>(scheduler));
    }

    public virtual IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources)
    {
      return QueryLanguage.Merge_<TSource>(sources.ToObservable<IObservable<TSource>>(SchedulerDefaults.ConstantTimeOperations));
    }

    public virtual IObservable<TSource> Merge<TSource>(
      IEnumerable<IObservable<TSource>> sources,
      IScheduler scheduler)
    {
      return QueryLanguage.Merge_<TSource>(sources.ToObservable<IObservable<TSource>>(scheduler));
    }

    private static IObservable<TSource> Merge_<TSource>(IObservable<IObservable<TSource>> sources)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Merge<TSource>(sources);
    }

    private static IObservable<TSource> Merge_<TSource>(
      IObservable<IObservable<TSource>> sources,
      int maxConcurrent)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Merge<TSource>(sources, maxConcurrent);
    }

    public virtual IObservable<TSource> OnErrorResumeNext<TSource>(
      IObservable<TSource> first,
      IObservable<TSource> second)
    {
      return QueryLanguage.OnErrorResumeNext_<TSource>((IEnumerable<IObservable<TSource>>) new IObservable<TSource>[2]
      {
        first,
        second
      });
    }

    public virtual IObservable<TSource> OnErrorResumeNext<TSource>(
      params IObservable<TSource>[] sources)
    {
      return QueryLanguage.OnErrorResumeNext_<TSource>((IEnumerable<IObservable<TSource>>) sources);
    }

    public virtual IObservable<TSource> OnErrorResumeNext<TSource>(
      IEnumerable<IObservable<TSource>> sources)
    {
      return QueryLanguage.OnErrorResumeNext_<TSource>(sources);
    }

    private static IObservable<TSource> OnErrorResumeNext_<TSource>(
      IEnumerable<IObservable<TSource>> sources)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.OnErrorResumeNext<TSource>(sources);
    }

    public virtual IObservable<TSource> SkipUntil<TSource, TOther>(
      IObservable<TSource> source,
      IObservable<TOther> other)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.SkipUntil<TSource, TOther>(source, other);
    }

    public virtual IObservable<TSource> Switch<TSource>(IObservable<IObservable<TSource>> sources)
    {
      return this.Switch_<TSource>(sources);
    }

    public virtual IObservable<TSource> Switch<TSource>(IObservable<Task<TSource>> sources)
    {
      return this.Switch_<TSource>(this.Select<Task<TSource>, IObservable<TSource>>(sources, new Func<Task<TSource>, IObservable<TSource>>(TaskObservableExtensions.ToObservable<TSource>)));
    }

    private IObservable<TSource> Switch_<TSource>(IObservable<IObservable<TSource>> sources)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Switch<TSource>(sources);
    }

    public virtual IObservable<TSource> TakeUntil<TSource, TOther>(
      IObservable<TSource> source,
      IObservable<TOther> other)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.TakeUntil<TSource, TOther>(source, other);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource, TWindowClosing>(
      IObservable<TSource> source,
      Func<IObservable<TWindowClosing>> windowClosingSelector)
    {
      return (IObservable<IObservable<TSource>>) new System.Reactive.Linq.ObservableImpl.Window<TSource, TWindowClosing>(source, windowClosingSelector);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource, TWindowOpening, TWindowClosing>(
      IObservable<TSource> source,
      IObservable<TWindowOpening> windowOpenings,
      Func<TWindowOpening, IObservable<TWindowClosing>> windowClosingSelector)
    {
      return windowOpenings.GroupJoin<TWindowOpening, TSource, TWindowClosing, Unit, IObservable<TSource>>(source, windowClosingSelector, (Func<TSource, IObservable<Unit>>) (_ => Observable.Empty<Unit>()), (Func<TWindowOpening, IObservable<TSource>, IObservable<TSource>>) ((_, window) => window));
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource, TWindowBoundary>(
      IObservable<TSource> source,
      IObservable<TWindowBoundary> windowBoundaries)
    {
      return (IObservable<IObservable<TSource>>) new System.Reactive.Linq.ObservableImpl.Window<TSource, TWindowBoundary>(source, windowBoundaries);
    }

    public virtual IObservable<TResult> WithLatestFrom<TFirst, TSecond, TResult>(
      IObservable<TFirst> first,
      IObservable<TSecond> second,
      Func<TFirst, TSecond, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.WithLatestFrom<TFirst, TSecond, TResult>(first, second, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TFirst, TSecond, TResult>(
      IObservable<TFirst> first,
      IObservable<TSecond> second,
      Func<TFirst, TSecond, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Zip<TFirst, TSecond, TResult>(first, second, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TSource, TResult>(
      IEnumerable<IObservable<TSource>> sources,
      Func<IList<TSource>, TResult> resultSelector)
    {
      return QueryLanguage.Zip_<TSource>(sources).Select<IList<TSource>, TResult>(resultSelector);
    }

    public virtual IObservable<IList<TSource>> Zip<TSource>(
      IEnumerable<IObservable<TSource>> sources)
    {
      return QueryLanguage.Zip_<TSource>(sources);
    }

    public virtual IObservable<IList<TSource>> Zip<TSource>(params IObservable<TSource>[] sources)
    {
      return QueryLanguage.Zip_<TSource>((IEnumerable<IObservable<TSource>>) sources);
    }

    private static IObservable<IList<TSource>> Zip_<TSource>(
      IEnumerable<IObservable<TSource>> sources)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.ObservableImpl.Zip<TSource>(sources);
    }

    public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TResult>(
      IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      Func<TSource1, TSource2, TSource3, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Zip<TSource1, TSource2, TSource3, TResult>(source1, source2, source3, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TResult>(
      IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      Func<TSource1, TSource2, TSource3, TSource4, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Zip<TSource1, TSource2, TSource3, TSource4, TResult>(source1, source2, source3, source4, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(
      IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(source1, source2, source3, source4, source5, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(
      IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(source1, source2, source3, source4, source5, source6, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(
      IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(source1, source2, source3, source4, source5, source6, source7, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(
      IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      IObservable<TSource5> source5,
      IObservable<TSource6> source6,
      IObservable<TSource7> source7,
      IObservable<TSource8> source8,
      Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(
      IObservable<TSource1> source1,
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
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(
      IObservable<TSource1> source1,
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
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(
      IObservable<TSource1> source1,
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
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(
      IObservable<TSource1> source1,
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
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(
      IObservable<TSource1> source1,
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
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(
      IObservable<TSource1> source1,
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
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(
      IObservable<TSource1> source1,
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
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(
      IObservable<TSource1> source1,
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
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, source16, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TFirst, TSecond, TResult>(
      IObservable<TFirst> first,
      IEnumerable<TSecond> second,
      Func<TFirst, TSecond, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Zip<TFirst, TSecond, TResult>(first, second, resultSelector);
    }

    public virtual IObservable<TSource> AsObservable<TSource>(IObservable<TSource> source)
    {
      return source is System.Reactive.Linq.ObservableImpl.AsObservable<TSource> asObservable ? asObservable.Omega() : (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.AsObservable<TSource>(source);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource>(
      IObservable<TSource> source,
      int count)
    {
      return QueryLanguage.Buffer_<TSource>(source, count, count);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource>(
      IObservable<TSource> source,
      int count,
      int skip)
    {
      return QueryLanguage.Buffer_<TSource>(source, count, skip);
    }

    private static IObservable<IList<TSource>> Buffer_<TSource>(
      IObservable<TSource> source,
      int count,
      int skip)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.ObservableImpl.Buffer<TSource>(source, count, skip);
    }

    public virtual IObservable<TSource> Dematerialize<TSource>(
      IObservable<Notification<TSource>> source)
    {
      return source is System.Reactive.Linq.ObservableImpl.Materialize<TSource> materialize ? materialize.Dematerialize() : (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Dematerialize<TSource>(source);
    }

    public virtual IObservable<TSource> DistinctUntilChanged<TSource>(IObservable<TSource> source)
    {
      return QueryLanguage.DistinctUntilChanged_<TSource, TSource>(source, (Func<TSource, TSource>) (x => x), (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default);
    }

    public virtual IObservable<TSource> DistinctUntilChanged<TSource>(
      IObservable<TSource> source,
      IEqualityComparer<TSource> comparer)
    {
      return QueryLanguage.DistinctUntilChanged_<TSource, TSource>(source, (Func<TSource, TSource>) (x => x), comparer);
    }

    public virtual IObservable<TSource> DistinctUntilChanged<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return QueryLanguage.DistinctUntilChanged_<TSource, TKey>(source, keySelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<TSource> DistinctUntilChanged<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      return QueryLanguage.DistinctUntilChanged_<TSource, TKey>(source, keySelector, comparer);
    }

    private static IObservable<TSource> DistinctUntilChanged_<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.DistinctUntilChanged<TSource, TKey>(source, keySelector, comparer);
    }

    public virtual IObservable<TSource> Do<TSource>(
      IObservable<TSource> source,
      Action<TSource> onNext)
    {
      return QueryLanguage.Do_<TSource>(source, onNext, Stubs<Exception>.Ignore, Stubs.Nop);
    }

    public virtual IObservable<TSource> Do<TSource>(
      IObservable<TSource> source,
      Action<TSource> onNext,
      Action onCompleted)
    {
      return QueryLanguage.Do_<TSource>(source, onNext, Stubs<Exception>.Ignore, onCompleted);
    }

    public virtual IObservable<TSource> Do<TSource>(
      IObservable<TSource> source,
      Action<TSource> onNext,
      Action<Exception> onError)
    {
      return QueryLanguage.Do_<TSource>(source, onNext, onError, Stubs.Nop);
    }

    public virtual IObservable<TSource> Do<TSource>(
      IObservable<TSource> source,
      Action<TSource> onNext,
      Action<Exception> onError,
      Action onCompleted)
    {
      return QueryLanguage.Do_<TSource>(source, onNext, onError, onCompleted);
    }

    public virtual IObservable<TSource> Do<TSource>(
      IObservable<TSource> source,
      IObserver<TSource> observer)
    {
      return QueryLanguage.Do_<TSource>(source, new Action<TSource>(observer.OnNext), new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
    }

    private static IObservable<TSource> Do_<TSource>(
      IObservable<TSource> source,
      Action<TSource> onNext,
      Action<Exception> onError,
      Action onCompleted)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Do<TSource>(source, onNext, onError, onCompleted);
    }

    public virtual IObservable<TSource> Finally<TSource>(
      IObservable<TSource> source,
      Action finallyAction)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Finally<TSource>(source, finallyAction);
    }

    public virtual IObservable<TSource> IgnoreElements<TSource>(IObservable<TSource> source)
    {
      return source is System.Reactive.Linq.ObservableImpl.IgnoreElements<TSource> ignoreElements ? ignoreElements.Omega() : (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.IgnoreElements<TSource>(source);
    }

    public virtual IObservable<Notification<TSource>> Materialize<TSource>(
      IObservable<TSource> source)
    {
      return (IObservable<Notification<TSource>>) new System.Reactive.Linq.ObservableImpl.Materialize<TSource>(source);
    }

    public virtual IObservable<TSource> Repeat<TSource>(IObservable<TSource> source)
    {
      return QueryLanguage.RepeatInfinite<IObservable<TSource>>(source).Concat<TSource>();
    }

    private static IEnumerable<T> RepeatInfinite<T>(T value)
    {
      while (true)
        yield return value;
    }

    public virtual IObservable<TSource> Repeat<TSource>(
      IObservable<TSource> source,
      int repeatCount)
    {
      return Enumerable.Repeat<IObservable<TSource>>(source, repeatCount).Concat<TSource>();
    }

    public virtual IObservable<TSource> Retry<TSource>(IObservable<TSource> source)
    {
      return QueryLanguage.RepeatInfinite<IObservable<TSource>>(source).Catch<TSource>();
    }

    public virtual IObservable<TSource> Retry<TSource>(IObservable<TSource> source, int retryCount)
    {
      return Enumerable.Repeat<IObservable<TSource>>(source, retryCount).Catch<TSource>();
    }

    public virtual IObservable<TAccumulate> Scan<TSource, TAccumulate>(
      IObservable<TSource> source,
      TAccumulate seed,
      Func<TAccumulate, TSource, TAccumulate> accumulator)
    {
      return (IObservable<TAccumulate>) new System.Reactive.Linq.ObservableImpl.Scan<TSource, TAccumulate>(source, seed, accumulator);
    }

    public virtual IObservable<TSource> Scan<TSource>(
      IObservable<TSource> source,
      Func<TSource, TSource, TSource> accumulator)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Scan<TSource>(source, accumulator);
    }

    public virtual IObservable<TSource> SkipLast<TSource>(IObservable<TSource> source, int count)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.SkipLast<TSource>(source, count);
    }

    public virtual IObservable<TSource> StartWith<TSource>(
      IObservable<TSource> source,
      params TSource[] values)
    {
      return QueryLanguage.StartWith_<TSource>(source, SchedulerDefaults.ConstantTimeOperations, values);
    }

    public virtual IObservable<TSource> StartWith<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler,
      params TSource[] values)
    {
      return QueryLanguage.StartWith_<TSource>(source, scheduler, values);
    }

    public virtual IObservable<TSource> StartWith<TSource>(
      IObservable<TSource> source,
      IEnumerable<TSource> values)
    {
      return this.StartWith<TSource>(source, SchedulerDefaults.ConstantTimeOperations, values);
    }

    public virtual IObservable<TSource> StartWith<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler,
      IEnumerable<TSource> values)
    {
      if (!(values is TSource[] sourceArray))
        sourceArray = new List<TSource>(values).ToArray();
      return QueryLanguage.StartWith_<TSource>(source, scheduler, sourceArray);
    }

    private static IObservable<TSource> StartWith_<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler,
      params TSource[] values)
    {
      return ((IEnumerable<TSource>) values).ToObservable<TSource>(scheduler).Concat<TSource>(source);
    }

    public virtual IObservable<TSource> TakeLast<TSource>(IObservable<TSource> source, int count)
    {
      return QueryLanguage.TakeLast_<TSource>(source, count, SchedulerDefaults.Iteration);
    }

    public virtual IObservable<TSource> TakeLast<TSource>(
      IObservable<TSource> source,
      int count,
      IScheduler scheduler)
    {
      return QueryLanguage.TakeLast_<TSource>(source, count, scheduler);
    }

    private static IObservable<TSource> TakeLast_<TSource>(
      IObservable<TSource> source,
      int count,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.TakeLast<TSource>(source, count, scheduler);
    }

    public virtual IObservable<IList<TSource>> TakeLastBuffer<TSource>(
      IObservable<TSource> source,
      int count)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.ObservableImpl.TakeLastBuffer<TSource>(source, count);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource>(
      IObservable<TSource> source,
      int count,
      int skip)
    {
      return QueryLanguage.Window_<TSource>(source, count, skip);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource>(
      IObservable<TSource> source,
      int count)
    {
      return QueryLanguage.Window_<TSource>(source, count, count);
    }

    private static IObservable<IObservable<TSource>> Window_<TSource>(
      IObservable<TSource> source,
      int count,
      int skip)
    {
      return (IObservable<IObservable<TSource>>) new System.Reactive.Linq.ObservableImpl.Window<TSource>(source, count, skip);
    }

    public virtual IObservable<TResult> Cast<TResult>(IObservable<object> source)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Cast<object, TResult>(source);
    }

    public virtual IObservable<TSource> DefaultIfEmpty<TSource>(IObservable<TSource> source)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.DefaultIfEmpty<TSource>(source, default (TSource));
    }

    public virtual IObservable<TSource> DefaultIfEmpty<TSource>(
      IObservable<TSource> source,
      TSource defaultValue)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.DefaultIfEmpty<TSource>(source, defaultValue);
    }

    public virtual IObservable<TSource> Distinct<TSource>(IObservable<TSource> source)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Distinct<TSource, TSource>(source, (Func<TSource, TSource>) (x => x), (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default);
    }

    public virtual IObservable<TSource> Distinct<TSource>(
      IObservable<TSource> source,
      IEqualityComparer<TSource> comparer)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Distinct<TSource, TSource>(source, (Func<TSource, TSource>) (x => x), comparer);
    }

    public virtual IObservable<TSource> Distinct<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Distinct<TSource, TKey>(source, keySelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<TSource> Distinct<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Distinct<TSource, TKey>(source, keySelector, comparer);
    }

    public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector)
    {
      return QueryLanguage.GroupBy_<TSource, TKey, TElement>(source, keySelector, elementSelector, new int?(), (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      return QueryLanguage.GroupBy_<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (x => x), new int?(), comparer);
    }

    public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return QueryLanguage.GroupBy_<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (x => x), new int?(), (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      return QueryLanguage.GroupBy_<TSource, TKey, TElement>(source, keySelector, elementSelector, new int?(), comparer);
    }

    public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      int capacity)
    {
      return QueryLanguage.GroupBy_<TSource, TKey, TElement>(source, keySelector, elementSelector, new int?(capacity), (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      int capacity,
      IEqualityComparer<TKey> comparer)
    {
      return QueryLanguage.GroupBy_<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (x => x), new int?(capacity), comparer);
    }

    public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      int capacity)
    {
      return QueryLanguage.GroupBy_<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (x => x), new int?(capacity), (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      int capacity,
      IEqualityComparer<TKey> comparer)
    {
      return QueryLanguage.GroupBy_<TSource, TKey, TElement>(source, keySelector, elementSelector, new int?(capacity), comparer);
    }

    private static IObservable<IGroupedObservable<TKey, TElement>> GroupBy_<TSource, TKey, TElement>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      int? capacity,
      IEqualityComparer<TKey> comparer)
    {
      return (IObservable<IGroupedObservable<TKey, TElement>>) new System.Reactive.Linq.ObservableImpl.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, capacity, comparer);
    }

    public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector,
      IEqualityComparer<TKey> comparer)
    {
      return QueryLanguage.GroupByUntil_<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, new int?(), comparer);
    }

    public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector)
    {
      return QueryLanguage.GroupByUntil_<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, new int?(), (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector,
      IEqualityComparer<TKey> comparer)
    {
      return QueryLanguage.GroupByUntil_<TSource, TKey, TSource, TDuration>(source, keySelector, (Func<TSource, TSource>) (x => x), durationSelector, new int?(), comparer);
    }

    public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector)
    {
      return QueryLanguage.GroupByUntil_<TSource, TKey, TSource, TDuration>(source, keySelector, (Func<TSource, TSource>) (x => x), durationSelector, new int?(), (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector,
      int capacity,
      IEqualityComparer<TKey> comparer)
    {
      return QueryLanguage.GroupByUntil_<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, new int?(capacity), comparer);
    }

    public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector,
      int capacity)
    {
      return QueryLanguage.GroupByUntil_<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, new int?(capacity), (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector,
      int capacity,
      IEqualityComparer<TKey> comparer)
    {
      return QueryLanguage.GroupByUntil_<TSource, TKey, TSource, TDuration>(source, keySelector, (Func<TSource, TSource>) (x => x), durationSelector, new int?(capacity), comparer);
    }

    public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector,
      int capacity)
    {
      return QueryLanguage.GroupByUntil_<TSource, TKey, TSource, TDuration>(source, keySelector, (Func<TSource, TSource>) (x => x), durationSelector, new int?(capacity), (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    private static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil_<TSource, TKey, TElement, TDuration>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector,
      int? capacity,
      IEqualityComparer<TKey> comparer)
    {
      return (IObservable<IGroupedObservable<TKey, TElement>>) new System.Reactive.Linq.ObservableImpl.GroupByUntil<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, capacity, comparer);
    }

    public virtual IObservable<TResult> GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
      IObservable<TLeft> left,
      IObservable<TRight> right,
      Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
      Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
      Func<TLeft, IObservable<TRight>, TResult> resultSelector)
    {
      return QueryLanguage.GroupJoin_<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
    }

    private static IObservable<TResult> GroupJoin_<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
      IObservable<TLeft> left,
      IObservable<TRight> right,
      Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
      Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
      Func<TLeft, IObservable<TRight>, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
    }

    public virtual IObservable<TResult> Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
      IObservable<TLeft> left,
      IObservable<TRight> right,
      Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
      Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
      Func<TLeft, TRight, TResult> resultSelector)
    {
      return QueryLanguage.Join_<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
    }

    private static IObservable<TResult> Join_<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
      IObservable<TLeft> left,
      IObservable<TRight> right,
      Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
      Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
      Func<TLeft, TRight, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
    }

    public virtual IObservable<TResult> OfType<TResult>(IObservable<object> source)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.OfType<object, TResult>(source);
    }

    public virtual IObservable<TResult> Select<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, TResult> selector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Select<TSource, TResult>(source, selector);
    }

    public virtual IObservable<TResult> Select<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, int, TResult> selector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Select<TSource, TResult>(source, selector);
    }

    public virtual IObservable<TOther> SelectMany<TSource, TOther>(
      IObservable<TSource> source,
      IObservable<TOther> other)
    {
      return QueryLanguage.SelectMany_<TSource, TOther>(source, (Func<TSource, IObservable<TOther>>) (_ => other));
    }

    public virtual IObservable<TResult> SelectMany<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TResult>> selector)
    {
      return QueryLanguage.SelectMany_<TSource, TResult>(source, selector);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, int, IObservable<TResult>> selector)
    {
      return QueryLanguage.SelectMany_<TSource, TResult>(source, selector);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, Task<TResult>> selector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TResult>(source, (Func<TSource, CancellationToken, Task<TResult>>) ((x, token) => selector(x)));
    }

    public virtual IObservable<TResult> SelectMany<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, int, Task<TResult>> selector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TResult>(source, (Func<TSource, int, CancellationToken, Task<TResult>>) ((x, i, token) => selector(x, i)));
    }

    public virtual IObservable<TResult> SelectMany<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, CancellationToken, Task<TResult>> selector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TResult>(source, selector);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, int, CancellationToken, Task<TResult>> selector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TResult>(source, selector);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TCollection, TResult>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TCollection>> collectionSelector,
      Func<TSource, TCollection, TResult> resultSelector)
    {
      return QueryLanguage.SelectMany_<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TCollection, TResult>(
      IObservable<TSource> source,
      Func<TSource, int, IObservable<TCollection>> collectionSelector,
      Func<TSource, int, TCollection, int, TResult> resultSelector)
    {
      return QueryLanguage.SelectMany_<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(
      IObservable<TSource> source,
      Func<TSource, Task<TTaskResult>> taskSelector,
      Func<TSource, TTaskResult, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TTaskResult, TResult>(source, (Func<TSource, CancellationToken, Task<TTaskResult>>) ((x, token) => taskSelector(x)), resultSelector);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(
      IObservable<TSource> source,
      Func<TSource, int, Task<TTaskResult>> taskSelector,
      Func<TSource, int, TTaskResult, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TTaskResult, TResult>(source, (Func<TSource, int, CancellationToken, Task<TTaskResult>>) ((x, i, token) => taskSelector(x, i)), resultSelector);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(
      IObservable<TSource> source,
      Func<TSource, CancellationToken, Task<TTaskResult>> taskSelector,
      Func<TSource, TTaskResult, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TTaskResult, TResult>(source, taskSelector, resultSelector);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(
      IObservable<TSource> source,
      Func<TSource, int, CancellationToken, Task<TTaskResult>> taskSelector,
      Func<TSource, int, TTaskResult, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TTaskResult, TResult>(source, taskSelector, resultSelector);
    }

    private static IObservable<TResult> SelectMany_<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TResult>> selector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TResult>(source, selector);
    }

    private static IObservable<TResult> SelectMany_<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, int, IObservable<TResult>> selector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TResult>(source, selector);
    }

    private static IObservable<TResult> SelectMany_<TSource, TCollection, TResult>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TCollection>> collectionSelector,
      Func<TSource, TCollection, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
    }

    private static IObservable<TResult> SelectMany_<TSource, TCollection, TResult>(
      IObservable<TSource> source,
      Func<TSource, int, IObservable<TCollection>> collectionSelector,
      Func<TSource, int, TCollection, int, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TResult>> onNext,
      Func<Exception, IObservable<TResult>> onError,
      Func<IObservable<TResult>> onCompleted)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TResult>(source, onNext, onError, onCompleted);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, int, IObservable<TResult>> onNext,
      Func<Exception, IObservable<TResult>> onError,
      Func<IObservable<TResult>> onCompleted)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TResult>(source, onNext, onError, onCompleted);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, IEnumerable<TResult>> selector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TResult>(source, selector);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, int, IEnumerable<TResult>> selector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TResult>(source, selector);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TCollection, TResult>(
      IObservable<TSource> source,
      Func<TSource, IEnumerable<TCollection>> collectionSelector,
      Func<TSource, TCollection, TResult> resultSelector)
    {
      return QueryLanguage.SelectMany_<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TCollection, TResult>(
      IObservable<TSource> source,
      Func<TSource, int, IEnumerable<TCollection>> collectionSelector,
      Func<TSource, int, TCollection, int, TResult> resultSelector)
    {
      return QueryLanguage.SelectMany_<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
    }

    private static IObservable<TResult> SelectMany_<TSource, TCollection, TResult>(
      IObservable<TSource> source,
      Func<TSource, IEnumerable<TCollection>> collectionSelector,
      Func<TSource, TCollection, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
    }

    private static IObservable<TResult> SelectMany_<TSource, TCollection, TResult>(
      IObservable<TSource> source,
      Func<TSource, int, IEnumerable<TCollection>> collectionSelector,
      Func<TSource, int, TCollection, int, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
    }

    public virtual IObservable<TSource> Skip<TSource>(IObservable<TSource> source, int count)
    {
      return source is System.Reactive.Linq.ObservableImpl.Skip<TSource> skip && skip._scheduler == null ? skip.Omega(count) : (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Skip<TSource>(source, count);
    }

    public virtual IObservable<TSource> SkipWhile<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.SkipWhile<TSource>(source, predicate);
    }

    public virtual IObservable<TSource> SkipWhile<TSource>(
      IObservable<TSource> source,
      Func<TSource, int, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.SkipWhile<TSource>(source, predicate);
    }

    public virtual IObservable<TSource> Take<TSource>(IObservable<TSource> source, int count)
    {
      return count == 0 ? this.Empty<TSource>() : QueryLanguage.Take_<TSource>(source, count);
    }

    public virtual IObservable<TSource> Take<TSource>(
      IObservable<TSource> source,
      int count,
      IScheduler scheduler)
    {
      return count == 0 ? this.Empty<TSource>(scheduler) : QueryLanguage.Take_<TSource>(source, count);
    }

    private static IObservable<TSource> Take_<TSource>(IObservable<TSource> source, int count)
    {
      return source is System.Reactive.Linq.ObservableImpl.Take<TSource> take && take._scheduler == null ? take.Omega(count) : (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Take<TSource>(source, count);
    }

    public virtual IObservable<TSource> TakeWhile<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.TakeWhile<TSource>(source, predicate);
    }

    public virtual IObservable<TSource> TakeWhile<TSource>(
      IObservable<TSource> source,
      Func<TSource, int, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.TakeWhile<TSource>(source, predicate);
    }

    public virtual IObservable<TSource> Where<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return source is System.Reactive.Linq.ObservableImpl.Where<TSource> where ? where.Omega(predicate) : (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Where<TSource>(source, predicate);
    }

    public virtual IObservable<TSource> Where<TSource>(
      IObservable<TSource> source,
      Func<TSource, int, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Where<TSource>(source, predicate);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan)
    {
      return QueryLanguage.Buffer_<TSource>(source, timeSpan, timeSpan, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      IScheduler scheduler)
    {
      return QueryLanguage.Buffer_<TSource>(source, timeSpan, timeSpan, scheduler);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      TimeSpan timeShift)
    {
      return QueryLanguage.Buffer_<TSource>(source, timeSpan, timeShift, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      TimeSpan timeShift,
      IScheduler scheduler)
    {
      return QueryLanguage.Buffer_<TSource>(source, timeSpan, timeShift, scheduler);
    }

    private static IObservable<IList<TSource>> Buffer_<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      TimeSpan timeShift,
      IScheduler scheduler)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.ObservableImpl.Buffer<TSource>(source, timeSpan, timeShift, scheduler);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      int count)
    {
      return QueryLanguage.Buffer_<TSource>(source, timeSpan, count, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      int count,
      IScheduler scheduler)
    {
      return QueryLanguage.Buffer_<TSource>(source, timeSpan, count, scheduler);
    }

    private static IObservable<IList<TSource>> Buffer_<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      int count,
      IScheduler scheduler)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.ObservableImpl.Buffer<TSource>(source, timeSpan, count, scheduler);
    }

    public virtual IObservable<TSource> Delay<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime)
    {
      return QueryLanguage.Delay_<TSource>(source, dueTime, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Delay<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      return QueryLanguage.Delay_<TSource>(source, dueTime, scheduler);
    }

    private static IObservable<TSource> Delay_<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Delay<TSource>(source, dueTime, scheduler);
    }

    public virtual IObservable<TSource> Delay<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime)
    {
      return QueryLanguage.Delay_<TSource>(source, dueTime, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Delay<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IScheduler scheduler)
    {
      return QueryLanguage.Delay_<TSource>(source, dueTime, scheduler);
    }

    private static IObservable<TSource> Delay_<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Delay<TSource>(source, dueTime, scheduler);
    }

    public virtual IObservable<TSource> Delay<TSource, TDelay>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TDelay>> delayDurationSelector)
    {
      return QueryLanguage.Delay_<TSource, TDelay>(source, (IObservable<TDelay>) null, delayDurationSelector);
    }

    public virtual IObservable<TSource> Delay<TSource, TDelay>(
      IObservable<TSource> source,
      IObservable<TDelay> subscriptionDelay,
      Func<TSource, IObservable<TDelay>> delayDurationSelector)
    {
      return QueryLanguage.Delay_<TSource, TDelay>(source, subscriptionDelay, delayDurationSelector);
    }

    private static IObservable<TSource> Delay_<TSource, TDelay>(
      IObservable<TSource> source,
      IObservable<TDelay> subscriptionDelay,
      Func<TSource, IObservable<TDelay>> delayDurationSelector)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Delay<TSource, TDelay>(source, subscriptionDelay, delayDurationSelector);
    }

    public virtual IObservable<TSource> DelaySubscription<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime)
    {
      return QueryLanguage.DelaySubscription_<TSource>(source, dueTime, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> DelaySubscription<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      return QueryLanguage.DelaySubscription_<TSource>(source, dueTime, scheduler);
    }

    private static IObservable<TSource> DelaySubscription_<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.DelaySubscription<TSource>(source, dueTime, scheduler);
    }

    public virtual IObservable<TSource> DelaySubscription<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime)
    {
      return QueryLanguage.DelaySubscription_<TSource>(source, dueTime, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> DelaySubscription<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IScheduler scheduler)
    {
      return QueryLanguage.DelaySubscription_<TSource>(source, dueTime, scheduler);
    }

    private static IObservable<TSource> DelaySubscription_<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.DelaySubscription<TSource>(source, dueTime, scheduler);
    }

    public virtual IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, TimeSpan> timeSelector)
    {
      return QueryLanguage.Generate_<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, TimeSpan> timeSelector,
      IScheduler scheduler)
    {
      return QueryLanguage.Generate_<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
    }

    private static IObservable<TResult> Generate_<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, TimeSpan> timeSelector,
      IScheduler scheduler)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Generate<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
    }

    public virtual IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, DateTimeOffset> timeSelector)
    {
      return QueryLanguage.Generate_<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, DateTimeOffset> timeSelector,
      IScheduler scheduler)
    {
      return QueryLanguage.Generate_<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
    }

    private static IObservable<TResult> Generate_<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, DateTimeOffset> timeSelector,
      IScheduler scheduler)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.ObservableImpl.Generate<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
    }

    public virtual IObservable<long> Interval(TimeSpan period)
    {
      return QueryLanguage.Timer_(period, period, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<long> Interval(TimeSpan period, IScheduler scheduler)
    {
      return QueryLanguage.Timer_(period, period, scheduler);
    }

    public virtual IObservable<TSource> Sample<TSource>(
      IObservable<TSource> source,
      TimeSpan interval)
    {
      return QueryLanguage.Sample_<TSource>(source, interval, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Sample<TSource>(
      IObservable<TSource> source,
      TimeSpan interval,
      IScheduler scheduler)
    {
      return QueryLanguage.Sample_<TSource>(source, interval, scheduler);
    }

    private static IObservable<TSource> Sample_<TSource>(
      IObservable<TSource> source,
      TimeSpan interval,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Sample<TSource>(source, interval, scheduler);
    }

    public virtual IObservable<TSource> Sample<TSource, TSample>(
      IObservable<TSource> source,
      IObservable<TSample> sampler)
    {
      return QueryLanguage.Sample_<TSource, TSample>(source, sampler);
    }

    private static IObservable<TSource> Sample_<TSource, TSample>(
      IObservable<TSource> source,
      IObservable<TSample> sampler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Sample<TSource, TSample>(source, sampler);
    }

    public virtual IObservable<TSource> Skip<TSource>(
      IObservable<TSource> source,
      TimeSpan duration)
    {
      return QueryLanguage.Skip_<TSource>(source, duration, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Skip<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return QueryLanguage.Skip_<TSource>(source, duration, scheduler);
    }

    private static IObservable<TSource> Skip_<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return source is System.Reactive.Linq.ObservableImpl.Skip<TSource> skip && skip._scheduler == scheduler ? skip.Omega(duration) : (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Skip<TSource>(source, duration, scheduler);
    }

    public virtual IObservable<TSource> SkipLast<TSource>(
      IObservable<TSource> source,
      TimeSpan duration)
    {
      return QueryLanguage.SkipLast_<TSource>(source, duration, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> SkipLast<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return QueryLanguage.SkipLast_<TSource>(source, duration, scheduler);
    }

    private static IObservable<TSource> SkipLast_<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.SkipLast<TSource>(source, duration, scheduler);
    }

    public virtual IObservable<TSource> SkipUntil<TSource>(
      IObservable<TSource> source,
      DateTimeOffset startTime)
    {
      return QueryLanguage.SkipUntil_<TSource>(source, startTime, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> SkipUntil<TSource>(
      IObservable<TSource> source,
      DateTimeOffset startTime,
      IScheduler scheduler)
    {
      return QueryLanguage.SkipUntil_<TSource>(source, startTime, scheduler);
    }

    private static IObservable<TSource> SkipUntil_<TSource>(
      IObservable<TSource> source,
      DateTimeOffset startTime,
      IScheduler scheduler)
    {
      return source is System.Reactive.Linq.ObservableImpl.SkipUntil<TSource> skipUntil && skipUntil._scheduler == scheduler ? skipUntil.Omega(startTime) : (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.SkipUntil<TSource>(source, startTime, scheduler);
    }

    public virtual IObservable<TSource> Take<TSource>(
      IObservable<TSource> source,
      TimeSpan duration)
    {
      return QueryLanguage.Take_<TSource>(source, duration, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Take<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return QueryLanguage.Take_<TSource>(source, duration, scheduler);
    }

    private static IObservable<TSource> Take_<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return source is System.Reactive.Linq.ObservableImpl.Take<TSource> take && take._scheduler == scheduler ? take.Omega(duration) : (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Take<TSource>(source, duration, scheduler);
    }

    public virtual IObservable<TSource> TakeLast<TSource>(
      IObservable<TSource> source,
      TimeSpan duration)
    {
      return QueryLanguage.TakeLast_<TSource>(source, duration, SchedulerDefaults.TimeBasedOperations, SchedulerDefaults.Iteration);
    }

    public virtual IObservable<TSource> TakeLast<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return QueryLanguage.TakeLast_<TSource>(source, duration, scheduler, SchedulerDefaults.Iteration);
    }

    public virtual IObservable<TSource> TakeLast<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler timerScheduler,
      IScheduler loopScheduler)
    {
      return QueryLanguage.TakeLast_<TSource>(source, duration, timerScheduler, loopScheduler);
    }

    private static IObservable<TSource> TakeLast_<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler timerScheduler,
      IScheduler loopScheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.TakeLast<TSource>(source, duration, timerScheduler, loopScheduler);
    }

    public virtual IObservable<IList<TSource>> TakeLastBuffer<TSource>(
      IObservable<TSource> source,
      TimeSpan duration)
    {
      return QueryLanguage.TakeLastBuffer_<TSource>(source, duration, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<IList<TSource>> TakeLastBuffer<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return QueryLanguage.TakeLastBuffer_<TSource>(source, duration, scheduler);
    }

    private static IObservable<IList<TSource>> TakeLastBuffer_<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.ObservableImpl.TakeLastBuffer<TSource>(source, duration, scheduler);
    }

    public virtual IObservable<TSource> TakeUntil<TSource>(
      IObservable<TSource> source,
      DateTimeOffset endTime)
    {
      return QueryLanguage.TakeUntil_<TSource>(source, endTime, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> TakeUntil<TSource>(
      IObservable<TSource> source,
      DateTimeOffset endTime,
      IScheduler scheduler)
    {
      return QueryLanguage.TakeUntil_<TSource>(source, endTime, scheduler);
    }

    private static IObservable<TSource> TakeUntil_<TSource>(
      IObservable<TSource> source,
      DateTimeOffset endTime,
      IScheduler scheduler)
    {
      return source is System.Reactive.Linq.ObservableImpl.TakeUntil<TSource> takeUntil && takeUntil._scheduler == scheduler ? takeUntil.Omega(endTime) : (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.TakeUntil<TSource>(source, endTime, scheduler);
    }

    public virtual IObservable<TSource> Throttle<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime)
    {
      return QueryLanguage.Throttle_<TSource>(source, dueTime, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Throttle<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      return QueryLanguage.Throttle_<TSource>(source, dueTime, scheduler);
    }

    private static IObservable<TSource> Throttle_<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Throttle<TSource>(source, dueTime, scheduler);
    }

    public virtual IObservable<TSource> Throttle<TSource, TThrottle>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TThrottle>> throttleDurationSelector)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Throttle<TSource, TThrottle>(source, throttleDurationSelector);
    }

    public virtual IObservable<System.Reactive.TimeInterval<TSource>> TimeInterval<TSource>(
      IObservable<TSource> source)
    {
      return QueryLanguage.TimeInterval_<TSource>(source, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<System.Reactive.TimeInterval<TSource>> TimeInterval<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      return QueryLanguage.TimeInterval_<TSource>(source, scheduler);
    }

    private static IObservable<System.Reactive.TimeInterval<TSource>> TimeInterval_<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      return (IObservable<System.Reactive.TimeInterval<TSource>>) new System.Reactive.Linq.ObservableImpl.TimeInterval<TSource>(source, scheduler);
    }

    public virtual IObservable<TSource> Timeout<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime)
    {
      return QueryLanguage.Timeout_<TSource>(source, dueTime, Observable.Throw<TSource>((Exception) new TimeoutException()), SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Timeout<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      return QueryLanguage.Timeout_<TSource>(source, dueTime, Observable.Throw<TSource>((Exception) new TimeoutException()), scheduler);
    }

    public virtual IObservable<TSource> Timeout<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IObservable<TSource> other)
    {
      return QueryLanguage.Timeout_<TSource>(source, dueTime, other, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Timeout<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IObservable<TSource> other,
      IScheduler scheduler)
    {
      return QueryLanguage.Timeout_<TSource>(source, dueTime, other, scheduler);
    }

    private static IObservable<TSource> Timeout_<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IObservable<TSource> other,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Timeout<TSource>(source, dueTime, other, scheduler);
    }

    public virtual IObservable<TSource> Timeout<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime)
    {
      return QueryLanguage.Timeout_<TSource>(source, dueTime, Observable.Throw<TSource>((Exception) new TimeoutException()), SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Timeout<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IScheduler scheduler)
    {
      return QueryLanguage.Timeout_<TSource>(source, dueTime, Observable.Throw<TSource>((Exception) new TimeoutException()), scheduler);
    }

    public virtual IObservable<TSource> Timeout<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IObservable<TSource> other)
    {
      return QueryLanguage.Timeout_<TSource>(source, dueTime, other, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Timeout<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IObservable<TSource> other,
      IScheduler scheduler)
    {
      return QueryLanguage.Timeout_<TSource>(source, dueTime, other, scheduler);
    }

    private static IObservable<TSource> Timeout_<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IObservable<TSource> other,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Timeout<TSource>(source, dueTime, other, scheduler);
    }

    public virtual IObservable<TSource> Timeout<TSource, TTimeout>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TTimeout>> timeoutDurationSelector)
    {
      return QueryLanguage.Timeout_<TSource, TTimeout>(source, Observable.Never<TTimeout>(), timeoutDurationSelector, Observable.Throw<TSource>((Exception) new TimeoutException()));
    }

    public virtual IObservable<TSource> Timeout<TSource, TTimeout>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TTimeout>> timeoutDurationSelector,
      IObservable<TSource> other)
    {
      return QueryLanguage.Timeout_<TSource, TTimeout>(source, Observable.Never<TTimeout>(), timeoutDurationSelector, other);
    }

    public virtual IObservable<TSource> Timeout<TSource, TTimeout>(
      IObservable<TSource> source,
      IObservable<TTimeout> firstTimeout,
      Func<TSource, IObservable<TTimeout>> timeoutDurationSelector)
    {
      return QueryLanguage.Timeout_<TSource, TTimeout>(source, firstTimeout, timeoutDurationSelector, Observable.Throw<TSource>((Exception) new TimeoutException()));
    }

    public virtual IObservable<TSource> Timeout<TSource, TTimeout>(
      IObservable<TSource> source,
      IObservable<TTimeout> firstTimeout,
      Func<TSource, IObservable<TTimeout>> timeoutDurationSelector,
      IObservable<TSource> other)
    {
      return QueryLanguage.Timeout_<TSource, TTimeout>(source, firstTimeout, timeoutDurationSelector, other);
    }

    private static IObservable<TSource> Timeout_<TSource, TTimeout>(
      IObservable<TSource> source,
      IObservable<TTimeout> firstTimeout,
      Func<TSource, IObservable<TTimeout>> timeoutDurationSelector,
      IObservable<TSource> other)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.ObservableImpl.Timeout<TSource, TTimeout>(source, firstTimeout, timeoutDurationSelector, other);
    }

    public virtual IObservable<long> Timer(TimeSpan dueTime)
    {
      return QueryLanguage.Timer_(dueTime, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<long> Timer(DateTimeOffset dueTime)
    {
      return QueryLanguage.Timer_(dueTime, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<long> Timer(TimeSpan dueTime, TimeSpan period)
    {
      return QueryLanguage.Timer_(dueTime, period, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period)
    {
      return QueryLanguage.Timer_(dueTime, period, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<long> Timer(TimeSpan dueTime, IScheduler scheduler)
    {
      return QueryLanguage.Timer_(dueTime, scheduler);
    }

    public virtual IObservable<long> Timer(DateTimeOffset dueTime, IScheduler scheduler)
    {
      return QueryLanguage.Timer_(dueTime, scheduler);
    }

    public virtual IObservable<long> Timer(TimeSpan dueTime, TimeSpan period, IScheduler scheduler)
    {
      return QueryLanguage.Timer_(dueTime, period, scheduler);
    }

    public virtual IObservable<long> Timer(
      DateTimeOffset dueTime,
      TimeSpan period,
      IScheduler scheduler)
    {
      return QueryLanguage.Timer_(dueTime, period, scheduler);
    }

    private static IObservable<long> Timer_(TimeSpan dueTime, IScheduler scheduler)
    {
      return (IObservable<long>) new System.Reactive.Linq.ObservableImpl.Timer(dueTime, new TimeSpan?(), scheduler);
    }

    private static IObservable<long> Timer_(
      TimeSpan dueTime,
      TimeSpan period,
      IScheduler scheduler)
    {
      return (IObservable<long>) new System.Reactive.Linq.ObservableImpl.Timer(dueTime, new TimeSpan?(period), scheduler);
    }

    private static IObservable<long> Timer_(DateTimeOffset dueTime, IScheduler scheduler)
    {
      return (IObservable<long>) new System.Reactive.Linq.ObservableImpl.Timer(dueTime, new TimeSpan?(), scheduler);
    }

    private static IObservable<long> Timer_(
      DateTimeOffset dueTime,
      TimeSpan period,
      IScheduler scheduler)
    {
      return (IObservable<long>) new System.Reactive.Linq.ObservableImpl.Timer(dueTime, new TimeSpan?(period), scheduler);
    }

    public virtual IObservable<Timestamped<TSource>> Timestamp<TSource>(IObservable<TSource> source)
    {
      return QueryLanguage.Timestamp_<TSource>(source, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<Timestamped<TSource>> Timestamp<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      return QueryLanguage.Timestamp_<TSource>(source, scheduler);
    }

    private static IObservable<Timestamped<TSource>> Timestamp_<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      return (IObservable<Timestamped<TSource>>) new System.Reactive.Linq.ObservableImpl.Timestamp<TSource>(source, scheduler);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan)
    {
      return QueryLanguage.Window_<TSource>(source, timeSpan, timeSpan, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      IScheduler scheduler)
    {
      return QueryLanguage.Window_<TSource>(source, timeSpan, timeSpan, scheduler);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      TimeSpan timeShift)
    {
      return QueryLanguage.Window_<TSource>(source, timeSpan, timeShift, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      TimeSpan timeShift,
      IScheduler scheduler)
    {
      return QueryLanguage.Window_<TSource>(source, timeSpan, timeShift, scheduler);
    }

    private static IObservable<IObservable<TSource>> Window_<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      TimeSpan timeShift,
      IScheduler scheduler)
    {
      return (IObservable<IObservable<TSource>>) new System.Reactive.Linq.ObservableImpl.Window<TSource>(source, timeSpan, timeShift, scheduler);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      int count)
    {
      return QueryLanguage.Window_<TSource>(source, timeSpan, count, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      int count,
      IScheduler scheduler)
    {
      return QueryLanguage.Window_<TSource>(source, timeSpan, count, scheduler);
    }

    private static IObservable<IObservable<TSource>> Window_<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      int count,
      IScheduler scheduler)
    {
      return (IObservable<IObservable<TSource>>) new System.Reactive.Linq.ObservableImpl.Window<TSource>(source, timeSpan, count, scheduler);
    }

    private class WaitAndSetOnce : IDisposable
    {
      private readonly ManualResetEvent _evt;
      private int _hasSet;

      public WaitAndSetOnce() => this._evt = new ManualResetEvent(false);

      public void Set()
      {
        if (Interlocked.Exchange(ref this._hasSet, 1) != 0)
          return;
        this._evt.Set();
      }

      public void WaitOne() => this._evt.WaitOne();

      public void Dispose() => this._evt.Dispose();
    }
  }
}
