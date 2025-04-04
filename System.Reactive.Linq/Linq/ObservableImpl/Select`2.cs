// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Select`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Select<TSource, TResult> : Producer<TResult>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TResult> _selector;
    private readonly Func<TSource, int, TResult> _selectorI;

    public Select(IObservable<TSource> source, Func<TSource, TResult> selector)
    {
      this._source = source;
      this._selector = selector;
    }

    public Select(IObservable<TSource> source, Func<TSource, int, TResult> selector)
    {
      this._source = source;
      this._selectorI = selector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._selector != null)
      {
        Select<TSource, TResult>._ obj = new Select<TSource, TResult>._(this, observer, cancel);
        setSink((IDisposable) obj);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
      }
      Select<TSource, TResult>.SelectImpl selectImpl = new Select<TSource, TResult>.SelectImpl(this, observer, cancel);
      setSink((IDisposable) selectImpl);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) selectImpl);
    }

    private class _ : Sink<TResult>, IObserver<TSource>
    {
      private readonly Select<TSource, TResult> _parent;

      public _(Select<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        TResult result1 = default (TResult);
        TResult result2;
        try
        {
          result2 = this._parent._selector(value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this._observer.OnNext(result2);
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

    private class SelectImpl : Sink<TResult>, IObserver<TSource>
    {
      private readonly Select<TSource, TResult> _parent;
      private int _index;

      public SelectImpl(
        Select<TSource, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._index = 0;
      }

      public void OnNext(TSource value)
      {
        TResult result1 = default (TResult);
        TResult result2;
        try
        {
          result2 = this._parent._selectorI(value, checked (this._index++));
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this._observer.OnNext(result2);
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
