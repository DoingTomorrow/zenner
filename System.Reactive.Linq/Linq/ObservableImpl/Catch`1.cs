// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Catch`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Catch<TSource> : Producer<TSource>
  {
    private readonly IEnumerable<IObservable<TSource>> _sources;

    public Catch(IEnumerable<IObservable<TSource>> sources) => this._sources = sources;

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Catch<TSource>._ obj = new Catch<TSource>._(observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run(this._sources);
    }

    private class _(IObserver<TSource> observer, IDisposable cancel) : TailRecursiveSink<TSource>(observer, cancel)
    {
      private Exception _lastException;

      protected override IEnumerable<IObservable<TSource>> Extract(IObservable<TSource> source)
      {
        return source is Catch<TSource> @catch ? @catch._sources : (IEnumerable<IObservable<TSource>>) null;
      }

      public override void OnNext(TSource value) => this._observer.OnNext(value);

      public override void OnError(Exception error)
      {
        this._lastException = error;
        this._recurse();
      }

      public override void OnCompleted()
      {
        this._observer.OnCompleted();
        base.Dispose();
      }

      protected override void Done()
      {
        if (this._lastException != null)
          this._observer.OnError(this._lastException);
        else
          this._observer.OnCompleted();
        base.Dispose();
      }

      protected override bool Fail(Exception error)
      {
        this.OnError(error);
        return true;
      }
    }
  }
}
