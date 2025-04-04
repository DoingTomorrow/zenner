// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Range
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Range : Producer<int>
  {
    private readonly int _start;
    private readonly int _count;
    private readonly IScheduler _scheduler;

    public Range(int start, int count, IScheduler scheduler)
    {
      this._start = start;
      this._count = count;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<int> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Range._ obj = new Range._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<int>
    {
      private readonly Range _parent;

      public _(Range parent, IObserver<int> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        ISchedulerLongRunning schedulerLongRunning = this._parent._scheduler.AsLongRunning();
        return schedulerLongRunning != null ? schedulerLongRunning.ScheduleLongRunning<int>(0, new Action<int, ICancelable>(this.Loop)) : this._parent._scheduler.Schedule<int>(0, new Action<int, Action<int>>(this.LoopRec));
      }

      private void Loop(int i, ICancelable cancel)
      {
        for (; !cancel.IsDisposed && i < this._parent._count; ++i)
          this._observer.OnNext(this._parent._start + i);
        if (!cancel.IsDisposed)
          this._observer.OnCompleted();
        this.Dispose();
      }

      private void LoopRec(int i, Action<int> recurse)
      {
        if (i < this._parent._count)
        {
          this._observer.OnNext(this._parent._start + i);
          recurse(i + 1);
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
