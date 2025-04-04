// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.All`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class All<TSource> : Producer<bool>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, bool> _predicate;

    public All(IObservable<TSource> source, Func<TSource, bool> predicate)
    {
      this._source = source;
      this._predicate = predicate;
    }

    protected override IDisposable Run(
      IObserver<bool> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      All<TSource>._ obj = new All<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<bool>, IObserver<TSource>
    {
      private readonly All<TSource> _parent;

      public _(All<TSource> parent, IObserver<bool> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        bool flag;
        try
        {
          flag = this._parent._predicate(value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        if (flag)
          return;
        this._observer.OnNext(false);
        this._observer.OnCompleted();
        this.Dispose();
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(true);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
