// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.AnonymousDisposable
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Threading;

#nullable disable
namespace System.Reactive.Disposables
{
  internal sealed class AnonymousDisposable : ICancelable, IDisposable
  {
    private volatile Action _dispose;

    public AnonymousDisposable(Action dispose) => this._dispose = dispose;

    public bool IsDisposed => this._dispose == null;

    public void Dispose()
    {
      Action action = Interlocked.Exchange<Action>(ref this._dispose, (Action) null);
      if (action == null)
        return;
      action();
    }
  }
}
