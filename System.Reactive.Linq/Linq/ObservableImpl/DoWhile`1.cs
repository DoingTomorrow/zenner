// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.DoWhile`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class DoWhile<TSource> : Producer<TSource>, IConcatenatable<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<bool> _condition;

    public DoWhile(IObservable<TSource> source, Func<bool> condition)
    {
      this._condition = condition;
      this._source = source;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      DoWhile<TSource>._ obj = new DoWhile<TSource>._(observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run(this.GetSources());
    }

    public IEnumerable<IObservable<TSource>> GetSources()
    {
      yield return this._source;
      while (this._condition())
        yield return this._source;
    }

    private class _(IObserver<TSource> observer, IDisposable cancel) : ConcatSink<TSource>(observer, cancel)
    {
      public override void OnNext(TSource value) => this._observer.OnNext(value);

      public override void OnError(Exception error)
      {
        this._observer.OnError(error);
        base.Dispose();
      }
    }
  }
}
