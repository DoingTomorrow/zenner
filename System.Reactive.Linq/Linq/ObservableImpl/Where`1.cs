// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Where`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Where<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, bool> _predicate;
    private readonly Func<TSource, int, bool> _predicateI;

    public Where(IObservable<TSource> source, Func<TSource, bool> predicate)
    {
      this._source = source;
      this._predicate = predicate;
    }

    public Where(IObservable<TSource> source, Func<TSource, int, bool> predicate)
    {
      this._source = source;
      this._predicateI = predicate;
    }

    public IObservable<TSource> Omega(Func<TSource, bool> predicate)
    {
      return this._predicate != null ? (IObservable<TSource>) new Where<TSource>(this._source, (Func<TSource, bool>) (x => this._predicate(x) && predicate(x))) : (IObservable<TSource>) new Where<TSource>((IObservable<TSource>) this, predicate);
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._predicate != null)
      {
        Where<TSource>._ obj = new Where<TSource>._(this, observer, cancel);
        setSink((IDisposable) obj);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
      }
      Where<TSource>.WhereImpl whereImpl = new Where<TSource>.WhereImpl(this, observer, cancel);
      setSink((IDisposable) whereImpl);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) whereImpl);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Where<TSource> _parent;

      public _(Where<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
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
        if (!flag)
          return;
        this._observer.OnNext(value);
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

    private class WhereImpl : Sink<TSource>, IObserver<TSource>
    {
      private readonly Where<TSource> _parent;
      private int _index;

      public WhereImpl(Where<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._index = 0;
      }

      public void OnNext(TSource value)
      {
        bool flag;
        try
        {
          flag = this._parent._predicateI(value, checked (this._index++));
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        if (!flag)
          return;
        this._observer.OnNext(value);
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
