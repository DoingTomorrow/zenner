// Decompiled with JetBrains decompiler
// Type: System.Reactive.ObservableBase`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive
{
  public abstract class ObservableBase<T> : IObservable<T>
  {
    public IDisposable Subscribe(IObserver<T> observer)
    {
      AutoDetachObserver<T> state = observer != null ? new AutoDetachObserver<T>(observer) : throw new ArgumentNullException(nameof (observer));
      if (CurrentThreadScheduler.IsScheduleRequired)
      {
        CurrentThreadScheduler.Instance.Schedule<AutoDetachObserver<T>>(state, new Func<IScheduler, AutoDetachObserver<T>, IDisposable>(this.ScheduledSubscribe));
      }
      else
      {
        try
        {
          state.Disposable = this.SubscribeCore((IObserver<T>) state);
        }
        catch (Exception ex)
        {
          if (!state.Fail(ex))
            throw;
        }
      }
      return (IDisposable) state;
    }

    private IDisposable ScheduledSubscribe(IScheduler _, AutoDetachObserver<T> autoDetachObserver)
    {
      try
      {
        autoDetachObserver.Disposable = this.SubscribeCore((IObserver<T>) autoDetachObserver);
      }
      catch (Exception ex)
      {
        if (!autoDetachObserver.Fail(ex))
          throw;
      }
      return Disposable.Empty;
    }

    protected abstract IDisposable SubscribeCore(IObserver<T> observer);
  }
}
