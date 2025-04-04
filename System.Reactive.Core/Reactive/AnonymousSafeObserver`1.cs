// Decompiled with JetBrains decompiler
// Type: System.Reactive.AnonymousSafeObserver`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Threading;

#nullable disable
namespace System.Reactive
{
  internal class AnonymousSafeObserver<T> : IObserver<T>
  {
    private readonly Action<T> _onNext;
    private readonly Action<Exception> _onError;
    private readonly Action _onCompleted;
    private readonly IDisposable _disposable;
    private int isStopped;

    public AnonymousSafeObserver(
      Action<T> onNext,
      Action<Exception> onError,
      Action onCompleted,
      IDisposable disposable)
    {
      this._onNext = onNext;
      this._onError = onError;
      this._onCompleted = onCompleted;
      this._disposable = disposable;
    }

    public void OnNext(T value)
    {
      if (this.isStopped != 0)
        return;
      bool flag = false;
      try
      {
        this._onNext(value);
        flag = true;
      }
      finally
      {
        if (!flag)
          this._disposable.Dispose();
      }
    }

    public void OnError(Exception error)
    {
      if (Interlocked.Exchange(ref this.isStopped, 1) != 0)
        return;
      try
      {
        this._onError(error);
      }
      finally
      {
        this._disposable.Dispose();
      }
    }

    public void OnCompleted()
    {
      if (Interlocked.Exchange(ref this.isStopped, 1) != 0)
        return;
      try
      {
        this._onCompleted();
      }
      finally
      {
        this._disposable.Dispose();
      }
    }
  }
}
