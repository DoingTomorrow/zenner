// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.TakeWhile`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class TakeWhile<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, bool> _predicate;
    private readonly Func<TSource, int, bool> _predicateI;

    public TakeWhile(IObservable<TSource> source, Func<TSource, bool> predicate)
    {
      this._source = source;
      this._predicate = predicate;
    }

    public TakeWhile(IObservable<TSource> source, Func<TSource, int, bool> predicate)
    {
      this._source = source;
      this._predicateI = predicate;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._predicate != null)
      {
        TakeWhile<TSource>._ obj = new TakeWhile<TSource>._(this, observer, cancel);
        setSink((IDisposable) obj);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
      }
      TakeWhile<TSource>.TakeWhileImpl takeWhileImpl = new TakeWhile<TSource>.TakeWhileImpl(this, observer, cancel);
      setSink((IDisposable) takeWhileImpl);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) takeWhileImpl);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly TakeWhile<TSource> _parent;
      private bool _running;

      public _(TakeWhile<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._running = true;
      }

      public void OnNext(TSource value)
      {
        if (!this._running)
          return;
        try
        {
          this._running = this._parent._predicate(value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        if (this._running)
        {
          this._observer.OnNext(value);
        }
        else
        {
          this._observer.OnCompleted();
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
        this._observer.OnCompleted();
        this.Dispose();
      }
    }

    private class TakeWhileImpl : Sink<TSource>, IObserver<TSource>
    {
      private readonly TakeWhile<TSource> _parent;
      private bool _running;
      private int _index;

      public TakeWhileImpl(
        TakeWhile<TSource> parent,
        IObserver<TSource> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._running = true;
        this._index = 0;
      }

      public void OnNext(TSource value)
      {
        if (!this._running)
          return;
        try
        {
          this._running = this._parent._predicateI(value, checked (this._index++));
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        if (this._running)
        {
          this._observer.OnNext(value);
        }
        else
        {
          this._observer.OnCompleted();
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
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
