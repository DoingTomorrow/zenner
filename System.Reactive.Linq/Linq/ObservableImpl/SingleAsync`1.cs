// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.SingleAsync`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class SingleAsync<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, bool> _predicate;
    private readonly bool _throwOnEmpty;

    public SingleAsync(
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
        SingleAsync<TSource>.SingleAsyncImpl singleAsyncImpl = new SingleAsync<TSource>.SingleAsyncImpl(this, observer, cancel);
        setSink((IDisposable) singleAsyncImpl);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) singleAsyncImpl);
      }
      SingleAsync<TSource>._ obj = new SingleAsync<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly SingleAsync<TSource> _parent;
      private TSource _value;
      private bool _seenValue;

      public _(SingleAsync<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._value = default (TSource);
        this._seenValue = false;
      }

      public void OnNext(TSource value)
      {
        if (this._seenValue)
        {
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.MORE_THAN_ONE_ELEMENT));
          this.Dispose();
        }
        else
        {
          this._value = value;
          this._seenValue = true;
        }
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        if (!this._seenValue && this._parent._throwOnEmpty)
        {
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
        }
        else
        {
          this._observer.OnNext(this._value);
          this._observer.OnCompleted();
        }
        this.Dispose();
      }
    }

    private class SingleAsyncImpl : Sink<TSource>, IObserver<TSource>
    {
      private readonly SingleAsync<TSource> _parent;
      private TSource _value;
      private bool _seenValue;

      public SingleAsyncImpl(
        SingleAsync<TSource> parent,
        IObserver<TSource> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._value = default (TSource);
        this._seenValue = false;
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
        if (this._seenValue)
        {
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.MORE_THAN_ONE_MATCHING_ELEMENT));
          this.Dispose();
        }
        else
        {
          this._value = value;
          this._seenValue = true;
        }
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        if (!this._seenValue && this._parent._throwOnEmpty)
        {
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_MATCHING_ELEMENTS));
        }
        else
        {
          this._observer.OnNext(this._value);
          this._observer.OnCompleted();
        }
        this.Dispose();
      }
    }
  }
}
