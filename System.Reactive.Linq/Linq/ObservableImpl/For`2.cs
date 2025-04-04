// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.For`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class For<TSource, TResult> : Producer<TResult>, IConcatenatable<TResult>
  {
    private readonly IEnumerable<TSource> _source;
    private readonly Func<TSource, IObservable<TResult>> _resultSelector;

    public For(IEnumerable<TSource> source, Func<TSource, IObservable<TResult>> resultSelector)
    {
      this._source = source;
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      For<TSource, TResult>._ obj = new For<TSource, TResult>._(observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run(this.GetSources());
    }

    public IEnumerable<IObservable<TResult>> GetSources()
    {
      foreach (TSource source in this._source)
        yield return this._resultSelector(source);
    }

    private class _(IObserver<TResult> observer, IDisposable cancel) : ConcatSink<TResult>(observer, cancel)
    {
      public override void OnNext(TResult value) => this._observer.OnNext(value);

      public override void OnError(Exception error)
      {
        this._observer.OnError(error);
        base.Dispose();
      }
    }
  }
}
