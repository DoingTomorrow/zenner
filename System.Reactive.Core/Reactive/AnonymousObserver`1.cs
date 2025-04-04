// Decompiled with JetBrains decompiler
// Type: System.Reactive.AnonymousObserver`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

#nullable disable
namespace System.Reactive
{
  public sealed class AnonymousObserver<T> : ObserverBase<T>
  {
    private readonly Action<T> _onNext;
    private readonly Action<Exception> _onError;
    private readonly Action _onCompleted;

    public AnonymousObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
    {
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      if (onError == null)
        throw new ArgumentNullException(nameof (onError));
      if (onCompleted == null)
        throw new ArgumentNullException(nameof (onCompleted));
      this._onNext = onNext;
      this._onError = onError;
      this._onCompleted = onCompleted;
    }

    public AnonymousObserver(Action<T> onNext)
      : this(onNext, Stubs.Throw, Stubs.Nop)
    {
    }

    public AnonymousObserver(Action<T> onNext, Action<Exception> onError)
      : this(onNext, onError, Stubs.Nop)
    {
    }

    public AnonymousObserver(Action<T> onNext, Action onCompleted)
      : this(onNext, Stubs.Throw, onCompleted)
    {
    }

    protected override void OnNextCore(T value) => this._onNext(value);

    protected override void OnErrorCore(Exception error) => this._onError(error);

    protected override void OnCompletedCore() => this._onCompleted();

    internal IObserver<T> MakeSafe(IDisposable disposable)
    {
      return (IObserver<T>) new AnonymousSafeObserver<T>(this._onNext, this._onError, this._onCompleted, disposable);
    }
  }
}
