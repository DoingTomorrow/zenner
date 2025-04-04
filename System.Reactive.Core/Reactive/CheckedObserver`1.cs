// Decompiled with JetBrains decompiler
// Type: System.Reactive.CheckedObserver`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Threading;

#nullable disable
namespace System.Reactive
{
  internal class CheckedObserver<T> : IObserver<T>
  {
    private readonly IObserver<T> _observer;
    private int _state;
    private const int IDLE = 0;
    private const int BUSY = 1;
    private const int DONE = 2;

    public CheckedObserver(IObserver<T> observer) => this._observer = observer;

    public void OnNext(T value)
    {
      this.CheckAccess();
      try
      {
        this._observer.OnNext(value);
      }
      finally
      {
        Interlocked.Exchange(ref this._state, 0);
      }
    }

    public void OnError(Exception error)
    {
      this.CheckAccess();
      try
      {
        this._observer.OnError(error);
      }
      finally
      {
        Interlocked.Exchange(ref this._state, 2);
      }
    }

    public void OnCompleted()
    {
      this.CheckAccess();
      try
      {
        this._observer.OnCompleted();
      }
      finally
      {
        Interlocked.Exchange(ref this._state, 2);
      }
    }

    private void CheckAccess()
    {
      switch (Interlocked.CompareExchange(ref this._state, 1, 0))
      {
        case 1:
          throw new InvalidOperationException(Strings_Core.REENTRANCY_DETECTED);
        case 2:
          throw new InvalidOperationException(Strings_Core.OBSERVER_TERMINATED);
      }
    }
  }
}
