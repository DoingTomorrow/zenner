// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Generate`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Generate<TState, TResult> : Producer<TResult>
  {
    private readonly TState _initialState;
    private readonly Func<TState, bool> _condition;
    private readonly Func<TState, TState> _iterate;
    private readonly Func<TState, TResult> _resultSelector;
    private readonly Func<TState, DateTimeOffset> _timeSelectorA;
    private readonly Func<TState, TimeSpan> _timeSelectorR;
    private readonly IScheduler _scheduler;

    public Generate(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      IScheduler scheduler)
    {
      this._initialState = initialState;
      this._condition = condition;
      this._iterate = iterate;
      this._resultSelector = resultSelector;
      this._scheduler = scheduler;
    }

    public Generate(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, DateTimeOffset> timeSelector,
      IScheduler scheduler)
    {
      this._initialState = initialState;
      this._condition = condition;
      this._iterate = iterate;
      this._resultSelector = resultSelector;
      this._timeSelectorA = timeSelector;
      this._scheduler = scheduler;
    }

    public Generate(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, TimeSpan> timeSelector,
      IScheduler scheduler)
    {
      this._initialState = initialState;
      this._condition = condition;
      this._iterate = iterate;
      this._resultSelector = resultSelector;
      this._timeSelectorR = timeSelector;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._timeSelectorA != null)
      {
        Generate<TState, TResult>.SelectorA selectorA = new Generate<TState, TResult>.SelectorA(this, observer, cancel);
        setSink((IDisposable) selectorA);
        return selectorA.Run();
      }
      if (this._timeSelectorR != null)
      {
        Generate<TState, TResult>.Delta delta = new Generate<TState, TResult>.Delta(this, observer, cancel);
        setSink((IDisposable) delta);
        return delta.Run();
      }
      Generate<TState, TResult>._ obj = new Generate<TState, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class SelectorA : Sink<TResult>
    {
      private readonly Generate<TState, TResult> _parent;
      private bool _first;
      private bool _hasResult;
      private TResult _result;

      public SelectorA(
        Generate<TState, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._first = true;
        this._hasResult = false;
        this._result = default (TResult);
        return this._parent._scheduler.Schedule<TState>(this._parent._initialState, new Func<IScheduler, TState, IDisposable>(this.InvokeRec));
      }

      private IDisposable InvokeRec(IScheduler self, TState state)
      {
        DateTimeOffset dueTime = new DateTimeOffset();
        if (this._hasResult)
          this._observer.OnNext(this._result);
        try
        {
          if (this._first)
            this._first = false;
          else
            state = this._parent._iterate(state);
          this._hasResult = this._parent._condition(state);
          if (this._hasResult)
          {
            this._result = this._parent._resultSelector(state);
            dueTime = this._parent._timeSelectorA(state);
          }
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return Disposable.Empty;
        }
        if (this._hasResult)
          return self.Schedule<TState>(state, dueTime, new Func<IScheduler, TState, IDisposable>(this.InvokeRec));
        this._observer.OnCompleted();
        this.Dispose();
        return Disposable.Empty;
      }
    }

    private class Delta : Sink<TResult>
    {
      private readonly Generate<TState, TResult> _parent;
      private bool _first;
      private bool _hasResult;
      private TResult _result;

      public Delta(
        Generate<TState, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._first = true;
        this._hasResult = false;
        this._result = default (TResult);
        return this._parent._scheduler.Schedule<TState>(this._parent._initialState, new Func<IScheduler, TState, IDisposable>(this.InvokeRec));
      }

      private IDisposable InvokeRec(IScheduler self, TState state)
      {
        TimeSpan dueTime = new TimeSpan();
        if (this._hasResult)
          this._observer.OnNext(this._result);
        try
        {
          if (this._first)
            this._first = false;
          else
            state = this._parent._iterate(state);
          this._hasResult = this._parent._condition(state);
          if (this._hasResult)
          {
            this._result = this._parent._resultSelector(state);
            dueTime = this._parent._timeSelectorR(state);
          }
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return Disposable.Empty;
        }
        if (this._hasResult)
          return self.Schedule<TState>(state, dueTime, new Func<IScheduler, TState, IDisposable>(this.InvokeRec));
        this._observer.OnCompleted();
        this.Dispose();
        return Disposable.Empty;
      }
    }

    private class _ : Sink<TResult>
    {
      private readonly Generate<TState, TResult> _parent;
      private TState _state;
      private bool _first;

      public _(Generate<TState, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._state = this._parent._initialState;
        this._first = true;
        ISchedulerLongRunning scheduler = this._parent._scheduler.AsLongRunning();
        return scheduler != null ? scheduler.ScheduleLongRunning(new Action<ICancelable>(this.Loop)) : this._parent._scheduler.Schedule(new Action<Action>(this.LoopRec));
      }

      private void Loop(ICancelable cancel)
      {
        while (!cancel.IsDisposed)
        {
          TResult result = default (TResult);
          bool flag;
          try
          {
            if (this._first)
              this._first = false;
            else
              this._state = this._parent._iterate(this._state);
            flag = this._parent._condition(this._state);
            if (flag)
              result = this._parent._resultSelector(this._state);
          }
          catch (Exception ex)
          {
            this._observer.OnError(ex);
            this.Dispose();
            return;
          }
          if (flag)
            this._observer.OnNext(result);
          else
            break;
        }
        if (!cancel.IsDisposed)
          this._observer.OnCompleted();
        this.Dispose();
      }

      private void LoopRec(Action recurse)
      {
        TResult result = default (TResult);
        bool flag;
        try
        {
          if (this._first)
            this._first = false;
          else
            this._state = this._parent._iterate(this._state);
          flag = this._parent._condition(this._state);
          if (flag)
            result = this._parent._resultSelector(this._state);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        if (flag)
        {
          this._observer.OnNext(result);
          recurse();
        }
        else
        {
          this._observer.OnCompleted();
          this.Dispose();
        }
      }
    }
  }
}
