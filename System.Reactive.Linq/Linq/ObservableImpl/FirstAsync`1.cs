// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.FirstAsync`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class FirstAsync<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, bool> _predicate;
    private readonly bool _throwOnEmpty;

    public FirstAsync(
      IObservable<TSource> source,
      Func<TSource, bool> predicate,
      bool throwOnEmpty)
    {
      this._source = source;
      this._predicate = predicate;
      this._throwOnEmpty = throwOnEmpty;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._predicate != null)
      {
        FirstAsync<TSource>.FirstAsyncImpl firstAsyncImpl = new FirstAsync<TSource>.FirstAsyncImpl(this, observer, cancel);
        setSink((IDisposable) firstAsyncImpl);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) firstAsyncImpl);
      }
      FirstAsync<TSource>._ obj = new FirstAsync<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly FirstAsync<TSource> _parent;

      public _(FirstAsync<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        this._observer.OnNext(value);
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
        if (this._parent._throwOnEmpty)
        {
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
        }
        else
        {
          this._observer.OnNext(default (TSource));
          this._observer.OnCompleted();
        }
        this.Dispose();
      }
    }

    private class FirstAsyncImpl : Sink<TSource>, IObserver<TSource>
    {
      private readonly FirstAsync<TSource> _parent;

      public FirstAsyncImpl(
        FirstAsync<TSource> parent,
        IObserver<TSource> observer,
        IDisposable cancel)
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
        if (this._parent._throwOnEmpty)
        {
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_MATCHING_ELEMENTS));
        }
        else
        {
          this._observer.OnNext(default (TSource));
          this._observer.OnCompleted();
        }
        this.Dispose();
      }
    }
  }
}
