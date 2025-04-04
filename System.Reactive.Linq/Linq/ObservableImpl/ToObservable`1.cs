// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.ToObservable`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class ToObservable<TSource> : Producer<TSource>
  {
    private readonly IEnumerable<TSource> _source;
    private readonly IScheduler _scheduler;

    public ToObservable(IEnumerable<TSource> source, IScheduler scheduler)
    {
      this._source = source;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      ToObservable<TSource>._ obj = new ToObservable<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>
    {
      private readonly ToObservable<TSource> _parent;

      public _(ToObservable<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        IEnumerator<TSource> enumerator;
        try
        {
          enumerator = this._parent._source.GetEnumerator();
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return Disposable.Empty;
        }
        ISchedulerLongRunning schedulerLongRunning = this._parent._scheduler.AsLongRunning();
        if (schedulerLongRunning != null)
          return schedulerLongRunning.ScheduleLongRunning<IEnumerator<TSource>>(enumerator, new Action<IEnumerator<TSource>, ICancelable>(this.Loop));
        BooleanDisposable flag = new BooleanDisposable();
        this._parent._scheduler.Schedule<ToObservable<TSource>._.State>(new ToObservable<TSource>._.State((ICancelable) flag, enumerator), new Action<ToObservable<TSource>._.State, Action<ToObservable<TSource>._.State>>(this.LoopRec));
        return (IDisposable) flag;
      }

      private void LoopRec(
        ToObservable<TSource>._.State state,
        Action<ToObservable<TSource>._.State> recurse)
      {
        bool flag = false;
        Exception error = (Exception) null;
        TSource source = default (TSource);
        if (state.flag.IsDisposed)
        {
          state.enumerator.Dispose();
        }
        else
        {
          try
          {
            flag = state.enumerator.MoveNext();
            if (flag)
              source = state.enumerator.Current;
          }
          catch (Exception ex)
          {
            error = ex;
          }
          if (error != null)
          {
            state.enumerator.Dispose();
            this._observer.OnError(error);
            this.Dispose();
          }
          else if (!flag)
          {
            state.enumerator.Dispose();
            this._observer.OnCompleted();
            this.Dispose();
          }
          else
          {
            this._observer.OnNext(source);
            recurse(state);
          }
        }
      }

      private void Loop(IEnumerator<TSource> enumerator, ICancelable cancel)
      {
        while (!cancel.IsDisposed)
        {
          bool flag = false;
          Exception error = (Exception) null;
          TSource source = default (TSource);
          try
          {
            flag = enumerator.MoveNext();
            if (flag)
              source = enumerator.Current;
          }
          catch (Exception ex)
          {
            error = ex;
          }
          if (error != null)
          {
            this._observer.OnError(error);
            break;
          }
          if (!flag)
          {
            this._observer.OnCompleted();
            break;
          }
          this._observer.OnNext(source);
        }
        enumerator.Dispose();
        this.Dispose();
      }

      private class State
      {
        public readonly ICancelable flag;
        public readonly IEnumerator<TSource> enumerator;

        public State(ICancelable flag, IEnumerator<TSource> enumerator)
        {
          this.flag = flag;
          this.enumerator = enumerator;
        }
      }
    }
  }
}
