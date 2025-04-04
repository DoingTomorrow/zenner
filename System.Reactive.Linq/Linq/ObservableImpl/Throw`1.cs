// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Throw`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Throw<TResult> : Producer<TResult>
  {
    private readonly Exception _exception;
    private readonly IScheduler _scheduler;

    public Throw(Exception exception, IScheduler scheduler)
    {
      this._exception = exception;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Throw<TResult>._ obj = new Throw<TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TResult>
    {
      private readonly Throw<TResult> _parent;

      public _(Throw<TResult> parent, IObserver<TResult> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run() => this._parent._scheduler.Schedule(new Action(this.Invoke));

      private void Invoke()
      {
        this._observer.OnError(this._parent._exception);
        this.Dispose();
      }
    }
  }
}
