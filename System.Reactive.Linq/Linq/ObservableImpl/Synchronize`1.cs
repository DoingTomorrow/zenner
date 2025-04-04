// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Synchronize`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Synchronize<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly object _gate;

    public Synchronize(IObservable<TSource> source, object gate)
    {
      this._source = source;
      this._gate = gate;
    }

    public Synchronize(IObservable<TSource> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Synchronize<TSource>._ obj = new Synchronize<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.Subscribe((IObserver<TSource>) obj);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Synchronize<TSource> _parent;
      private object _gate;

      public _(Synchronize<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._gate = this._parent._gate ?? new object();
      }

      public void OnNext(TSource value)
      {
        lock (this._gate)
          this._observer.OnNext(value);
      }

      public void OnError(Exception error)
      {
        lock (this._gate)
        {
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        lock (this._gate)
        {
          this._observer.OnCompleted();
          this.Dispose();
        }
      }
    }
  }
}
