// Decompiled with JetBrains decompiler
// Type: System.Reactive.SafeObserver`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

#nullable disable
namespace System.Reactive
{
  internal class SafeObserver<TSource> : IObserver<TSource>
  {
    private readonly IObserver<TSource> _observer;
    private readonly IDisposable _disposable;

    public static IObserver<TSource> Create(IObserver<TSource> observer, IDisposable disposable)
    {
      return observer is AnonymousObserver<TSource> anonymousObserver ? anonymousObserver.MakeSafe(disposable) : (IObserver<TSource>) new SafeObserver<TSource>(observer, disposable);
    }

    private SafeObserver(IObserver<TSource> observer, IDisposable disposable)
    {
      this._observer = observer;
      this._disposable = disposable;
    }

    public void OnNext(TSource value)
    {
      bool flag = false;
      try
      {
        this._observer.OnNext(value);
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
      try
      {
        this._observer.OnError(error);
      }
      finally
      {
        this._disposable.Dispose();
      }
    }

    public void OnCompleted()
    {
      try
      {
        this._observer.OnCompleted();
      }
      finally
      {
        this._disposable.Dispose();
      }
    }
  }
}
