// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Catch`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Catch<TSource, TException> : Producer<TSource> where TException : Exception
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TException, IObservable<TSource>> _handler;

    public Catch(IObservable<TSource> source, Func<TException, IObservable<TSource>> handler)
    {
      this._source = source;
      this._handler = handler;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Catch<TSource, TException>._ obj = new Catch<TSource, TException>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Catch<TSource, TException> _parent;
      private SerialDisposable _subscription;

      public _(Catch<TSource, TException> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._subscription = new SerialDisposable();
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        this._subscription.Disposable = (IDisposable) assignmentDisposable;
        assignmentDisposable.Disposable = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return (IDisposable) this._subscription;
      }

      public void OnNext(TSource value) => this._observer.OnNext(value);

      public void OnError(Exception error)
      {
        if (error is TException exception)
        {
          IObservable<TSource> source;
          try
          {
            source = this._parent._handler(exception);
          }
          catch (Exception ex)
          {
            this._observer.OnError(ex);
            this.Dispose();
            return;
          }
          SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
          this._subscription.Disposable = (IDisposable) assignmentDisposable;
          assignmentDisposable.Disposable = source.SubscribeSafe<TSource>((IObserver<TSource>) new Catch<TSource, TException>._.Impl(this));
        }
        else
        {
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        this._observer.OnCompleted();
        this.Dispose();
      }

      private class Impl : IObserver<TSource>
      {
        private readonly Catch<TSource, TException>._ _parent;

        public Impl(Catch<TSource, TException>._ parent) => this._parent = parent;

        public void OnNext(TSource value) => this._parent._observer.OnNext(value);

        public void OnError(Exception error)
        {
          this._parent._observer.OnError(error);
          this._parent.Dispose();
        }

        public void OnCompleted()
        {
          this._parent._observer.OnCompleted();
          this._parent.Dispose();
        }
      }
    }
  }
}
