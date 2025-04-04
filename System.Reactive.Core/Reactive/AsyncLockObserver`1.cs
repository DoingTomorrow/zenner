// Decompiled with JetBrains decompiler
// Type: System.Reactive.AsyncLockObserver`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Reactive.Concurrency;

#nullable disable
namespace System.Reactive
{
  internal class AsyncLockObserver<T> : ObserverBase<T>
  {
    private readonly AsyncLock _gate;
    private readonly IObserver<T> _observer;

    public AsyncLockObserver(IObserver<T> observer, AsyncLock gate)
    {
      this._gate = gate;
      this._observer = observer;
    }

    protected override void OnNextCore(T value)
    {
      this._gate.Wait((Action) (() => this._observer.OnNext(value)));
    }

    protected override void OnErrorCore(Exception exception)
    {
      this._gate.Wait((Action) (() => this._observer.OnError(exception)));
    }

    protected override void OnCompletedCore()
    {
      this._gate.Wait((Action) (() => this._observer.OnCompleted()));
    }
  }
}
