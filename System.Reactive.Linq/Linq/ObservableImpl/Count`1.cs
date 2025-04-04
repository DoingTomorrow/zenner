// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Count`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Count<TSource> : Producer<int>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, bool> _predicate;

    public Count(IObservable<TSource> source) => this._source = source;

    public Count(IObservable<TSource> source, Func<TSource, bool> predicate)
    {
      this._source = source;
      this._predicate = predicate;
    }

    protected override IDisposable Run(
      IObserver<int> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._predicate == null)
      {
        Count<TSource>._ obj = new Count<TSource>._(observer, cancel);
        setSink((IDisposable) obj);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
      }
      Count<TSource>.CountImpl countImpl = new Count<TSource>.CountImpl(this, observer, cancel);
      setSink((IDisposable) countImpl);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) countImpl);
    }

    private class _ : Sink<int>, IObserver<TSource>
    {
      private int _count;

      public _(IObserver<int> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._count = 0;
      }

      public void OnNext(TSource value)
      {
        try
        {
          checked { ++this._count; }
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
        }
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(this._count);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }

    private class CountImpl : Sink<int>, IObserver<TSource>
    {
      private readonly Count<TSource> _parent;
      private int _count;

      public CountImpl(Count<TSource> parent, IObserver<int> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._count = 0;
      }

      public void OnNext(TSource value)
      {
        try
        {
          if (!this._parent._predicate(value))
            return;
          checked { ++this._count; }
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
        }
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(this._count);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
