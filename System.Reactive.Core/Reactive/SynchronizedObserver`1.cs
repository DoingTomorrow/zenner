// Decompiled with JetBrains decompiler
// Type: System.Reactive.SynchronizedObserver`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

#nullable disable
namespace System.Reactive
{
  internal class SynchronizedObserver<T> : ObserverBase<T>
  {
    private readonly object _gate;
    private readonly IObserver<T> _observer;

    public SynchronizedObserver(IObserver<T> observer, object gate)
    {
      this._gate = gate;
      this._observer = observer;
    }

    protected override void OnNextCore(T value)
    {
      lock (this._gate)
        this._observer.OnNext(value);
    }

    protected override void OnErrorCore(Exception exception)
    {
      lock (this._gate)
        this._observer.OnError(exception);
    }

    protected override void OnCompletedCore()
    {
      lock (this._gate)
        this._observer.OnCompleted();
    }
  }
}
