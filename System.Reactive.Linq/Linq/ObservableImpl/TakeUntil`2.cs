// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.TakeUntil`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class TakeUntil<TSource, TOther> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly IObservable<TOther> _other;

    public TakeUntil(IObservable<TSource> source, IObservable<TOther> other)
    {
      this._source = source;
      this._other = other;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      TakeUntil<TSource, TOther>._ obj = new TakeUntil<TSource, TOther>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>
    {
      private readonly TakeUntil<TSource, TOther> _parent;

      public _(TakeUntil<TSource, TOther> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        TakeUntil<TSource, TOther>._.T sourceObserver = new TakeUntil<TSource, TOther>._.T(this);
        TakeUntil<TSource, TOther>._.O o = new TakeUntil<TSource, TOther>._.O(this, sourceObserver);
        IDisposable disposable1 = this._parent._other.SubscribeSafe<TOther>((IObserver<TOther>) o);
        o.Disposable = disposable1;
        IDisposable disposable2 = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) sourceObserver);
        return (IDisposable) StableCompositeDisposable.Create(disposable1, disposable2);
      }

      private class T : IObserver<TSource>
      {
        private readonly TakeUntil<TSource, TOther>._ _parent;
        public volatile bool _open;

        public T(TakeUntil<TSource, TOther>._ parent)
        {
          this._parent = parent;
          this._open = false;
        }

        public void OnNext(TSource value)
        {
          if (this._open)
          {
            this._parent._observer.OnNext(value);
          }
          else
          {
            lock (this._parent)
              this._parent._observer.OnNext(value);
          }
        }

        public void OnError(Exception error)
        {
          lock (this._parent)
          {
            this._parent._observer.OnError(error);
            this._parent.Dispose();
          }
        }

        public void OnCompleted()
        {
          lock (this._parent)
          {
            this._parent._observer.OnCompleted();
            this._parent.Dispose();
          }
        }
      }

      private class O : IObserver<TOther>
      {
        private readonly TakeUntil<TSource, TOther>._ _parent;
        private readonly TakeUntil<TSource, TOther>._.T _sourceObserver;
        private readonly SingleAssignmentDisposable _subscription;

        public O(TakeUntil<TSource, TOther>._ parent, TakeUntil<TSource, TOther>._.T sourceObserver)
        {
          this._parent = parent;
          this._sourceObserver = sourceObserver;
          this._subscription = new SingleAssignmentDisposable();
        }

        public IDisposable Disposable
        {
          set => this._subscription.Disposable = value;
        }

        public void OnNext(TOther value)
        {
          lock (this._parent)
          {
            this._parent._observer.OnCompleted();
            this._parent.Dispose();
          }
        }

        public void OnError(Exception error)
        {
          lock (this._parent)
          {
            this._parent._observer.OnError(error);
            this._parent.Dispose();
          }
        }

        public void OnCompleted()
        {
          lock (this._parent)
          {
            this._sourceObserver._open = true;
            this._subscription.Dispose();
          }
        }
      }
    }
  }
}
