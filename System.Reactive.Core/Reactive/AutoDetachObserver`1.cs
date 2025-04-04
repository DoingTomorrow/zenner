// Decompiled with JetBrains decompiler
// Type: System.Reactive.AutoDetachObserver`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive
{
  internal class AutoDetachObserver<T> : ObserverBase<T>
  {
    private readonly IObserver<T> observer;
    private readonly SingleAssignmentDisposable m = new SingleAssignmentDisposable();

    public AutoDetachObserver(IObserver<T> observer) => this.observer = observer;

    public IDisposable Disposable
    {
      set => this.m.Disposable = value;
    }

    protected override void OnNextCore(T value)
    {
      bool flag = false;
      try
      {
        this.observer.OnNext(value);
        flag = true;
      }
      finally
      {
        if (!flag)
          this.Dispose();
      }
    }

    protected override void OnErrorCore(Exception exception)
    {
      try
      {
        this.observer.OnError(exception);
      }
      finally
      {
        this.Dispose();
      }
    }

    protected override void OnCompletedCore()
    {
      try
      {
        this.observer.OnCompleted();
      }
      finally
      {
        this.Dispose();
      }
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (!disposing)
        return;
      this.m.Dispose();
    }
  }
}
