// Decompiled with JetBrains decompiler
// Type: System.Reactive.AnonymousObservable`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive
{
  public sealed class AnonymousObservable<T> : ObservableBase<T>
  {
    private readonly Func<IObserver<T>, IDisposable> _subscribe;

    public AnonymousObservable(Func<IObserver<T>, IDisposable> subscribe)
    {
      this._subscribe = subscribe != null ? subscribe : throw new ArgumentNullException(nameof (subscribe));
    }

    protected override IDisposable SubscribeCore(IObserver<T> observer)
    {
      return this._subscribe(observer) ?? Disposable.Empty;
    }
  }
}
