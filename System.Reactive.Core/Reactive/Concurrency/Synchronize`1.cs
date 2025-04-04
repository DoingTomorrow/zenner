// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.Synchronize`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

#nullable disable
namespace System.Reactive.Concurrency
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
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Synchronize<TSource> _parent;
      private readonly object _gate;

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
