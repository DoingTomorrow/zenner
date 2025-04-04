// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.LongCount`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class LongCount<TSource> : Producer<long>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, bool> _predicate;

    public LongCount(IObservable<TSource> source) => this._source = source;

    public LongCount(IObservable<TSource> source, Func<TSource, bool> predicate)
    {
      this._source = source;
      this._predicate = predicate;
    }

    protected override IDisposable Run(
      IObserver<long> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._predicate == null)
      {
        LongCount<TSource>._ obj = new LongCount<TSource>._(observer, cancel);
        setSink((IDisposable) obj);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
      }
      LongCount<TSource>.LongCountImpl longCountImpl = new LongCount<TSource>.LongCountImpl(this, observer, cancel);
      setSink((IDisposable) longCountImpl);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) longCountImpl);
    }

    private class _ : Sink<long>, IObserver<TSource>
    {
      private long _count;

      public _(IObserver<long> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._count = 0L;
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

    private class LongCountImpl : Sink<long>, IObserver<TSource>
    {
      private readonly LongCount<TSource> _parent;
      private long _count;

      public LongCountImpl(LongCount<TSource> parent, IObserver<long> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._count = 0L;
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
