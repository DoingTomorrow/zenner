// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.OnErrorResumeNext`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class OnErrorResumeNext<TSource> : Producer<TSource>
  {
    private readonly IEnumerable<IObservable<TSource>> _sources;

    public OnErrorResumeNext(IEnumerable<IObservable<TSource>> sources) => this._sources = sources;

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      OnErrorResumeNext<TSource>._ obj = new OnErrorResumeNext<TSource>._(observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run(this._sources);
    }

    private class _(IObserver<TSource> observer, IDisposable cancel) : TailRecursiveSink<TSource>(observer, cancel)
    {
      protected override IEnumerable<IObservable<TSource>> Extract(IObservable<TSource> source)
      {
        return source is OnErrorResumeNext<TSource> onErrorResumeNext ? onErrorResumeNext._sources : (IEnumerable<IObservable<TSource>>) null;
      }

      public override void OnNext(TSource value) => this._observer.OnNext(value);

      public override void OnError(Exception error) => this._recurse();

      public override void OnCompleted() => this._recurse();

      protected override bool Fail(Exception error)
      {
        this.OnError(error);
        return true;
      }
    }
  }
}
