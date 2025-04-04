// Decompiled with JetBrains decompiler
// Type: System.Reactive.Subjects.Subject`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;
using System.Threading;

#nullable disable
namespace System.Reactive.Subjects
{
  public sealed class Subject<T> : SubjectBase<T>, IDisposable
  {
    private volatile IObserver<T> _observer;

    public Subject() => this._observer = NopObserver<T>.Instance;

    public override bool HasObservers
    {
      get
      {
        return this._observer != NopObserver<T>.Instance && !(this._observer is DoneObserver<T>) && this._observer != DisposedObserver<T>.Instance;
      }
    }

    public override bool IsDisposed => this._observer is DisposedObserver<T>;

    public override void OnCompleted()
    {
      IObserver<T> completed = DoneObserver<T>.Completed;
      IObserver<T> observer;
      do
      {
        observer = this._observer;
      }
      while (observer != DisposedObserver<T>.Instance && !(observer is DoneObserver<T>) && Interlocked.CompareExchange<IObserver<T>>(ref this._observer, completed, observer) != observer);
      observer.OnCompleted();
    }

    public override void OnError(Exception error)
    {
      DoneObserver<T> doneObserver = error != null ? new DoneObserver<T>()
      {
        Exception = error
      } : throw new ArgumentNullException(nameof (error));
      IObserver<T> observer;
      do
      {
        observer = this._observer;
      }
      while (observer != DisposedObserver<T>.Instance && !(observer is DoneObserver<T>) && Interlocked.CompareExchange<IObserver<T>>(ref this._observer, (IObserver<T>) doneObserver, observer) != observer);
      observer.OnError(error);
    }

    public override void OnNext(T value) => this._observer.OnNext(value);

    public override IDisposable Subscribe(IObserver<T> observer)
    {
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      IObserver<T> observer1;
      IObserver<T> observer2;
      do
      {
        observer1 = this._observer;
        if (observer1 == DisposedObserver<T>.Instance)
          throw new ObjectDisposedException("");
        if (observer1 == DoneObserver<T>.Completed)
        {
          observer.OnCompleted();
          return Disposable.Empty;
        }
        if (observer1 is DoneObserver<T> doneObserver)
        {
          observer.OnError(doneObserver.Exception);
          return Disposable.Empty;
        }
        if (observer1 == NopObserver<T>.Instance)
          observer2 = observer;
        else if (observer1 is Observer<T> observer3)
          observer2 = observer3.Add(observer);
        else
          observer2 = (IObserver<T>) new Observer<T>(new ImmutableList<IObserver<T>>(new IObserver<T>[2]
          {
            observer1,
            observer
          }));
      }
      while (Interlocked.CompareExchange<IObserver<T>>(ref this._observer, observer2, observer1) != observer1);
      return (IDisposable) new Subject<T>.Subscription(this, observer);
    }

    private void Unsubscribe(IObserver<T> observer)
    {
      IObserver<T> observer1;
      IObserver<T> observer2;
      do
      {
        observer1 = this._observer;
        if (observer1 == DisposedObserver<T>.Instance || observer1 is DoneObserver<T>)
          break;
        if (observer1 is Observer<T> observer3)
        {
          observer2 = observer3.Remove(observer);
        }
        else
        {
          if (observer1 != observer)
            break;
          observer2 = NopObserver<T>.Instance;
        }
      }
      while (Interlocked.CompareExchange<IObserver<T>>(ref this._observer, observer2, observer1) != observer1);
    }

    public override void Dispose() => this._observer = DisposedObserver<T>.Instance;

    private class Subscription : IDisposable
    {
      private Subject<T> _subject;
      private IObserver<T> _observer;

      public Subscription(Subject<T> subject, IObserver<T> observer)
      {
        this._subject = subject;
        this._observer = observer;
      }

      public void Dispose()
      {
        IObserver<T> observer = Interlocked.Exchange<IObserver<T>>(ref this._observer, (IObserver<T>) null);
        if (observer == null)
          return;
        this._subject.Unsubscribe(observer);
        this._subject = (Subject<T>) null;
      }
    }
  }
}
