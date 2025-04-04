// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.EventProducer`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal abstract class EventProducer<TDelegate, TArgs> : Producer<TArgs>
  {
    private readonly IScheduler _scheduler;
    private readonly object _gate;
    private EventProducer<TDelegate, TArgs>.Session _session;

    public EventProducer(IScheduler scheduler)
    {
      this._scheduler = scheduler;
      this._gate = new object();
    }

    protected abstract TDelegate GetHandler(Action<TArgs> onNext);

    protected abstract IDisposable AddHandler(TDelegate handler);

    protected override IDisposable Run(
      IObserver<TArgs> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      lock (this._gate)
      {
        if (this._session == null)
          this._session = new EventProducer<TDelegate, TArgs>.Session(this);
        return this._session.Connect(observer);
      }
    }

    private class Session
    {
      private readonly EventProducer<TDelegate, TArgs> _parent;
      private readonly Subject<TArgs> _subject;
      private SingleAssignmentDisposable _removeHandler;
      private int _count;

      public Session(EventProducer<TDelegate, TArgs> parent)
      {
        this._parent = parent;
        this._subject = new Subject<TArgs>();
      }

      public IDisposable Connect(IObserver<TArgs> observer)
      {
        IDisposable connection = this._subject.Subscribe(observer);
        if (++this._count == 1)
        {
          try
          {
            this.Initialize();
          }
          catch (Exception ex)
          {
            --this._count;
            connection.Dispose();
            observer.OnError(ex);
            return Disposable.Empty;
          }
        }
        return Disposable.Create((Action) (() =>
        {
          connection.Dispose();
          lock (this._parent._gate)
          {
            if (--this._count != 0)
              return;
            this._parent._scheduler.Schedule(new Action(this._removeHandler.Dispose));
            this._parent._session = (EventProducer<TDelegate, TArgs>.Session) null;
          }
        }));
      }

      private void Initialize()
      {
        this._removeHandler = new SingleAssignmentDisposable();
        this._parent._scheduler.Schedule<TDelegate>(this._parent.GetHandler(new Action<TArgs>(((SubjectBase<TArgs>) this._subject).OnNext)), new Func<IScheduler, TDelegate, IDisposable>(this.AddHandler));
      }

      private IDisposable AddHandler(IScheduler self, TDelegate onNext)
      {
        IDisposable disposable;
        try
        {
          disposable = this._parent.AddHandler(onNext);
        }
        catch (Exception ex)
        {
          this._subject.OnError(ex);
          return Disposable.Empty;
        }
        this._removeHandler.Disposable = disposable;
        return Disposable.Empty;
      }
    }
  }
}
