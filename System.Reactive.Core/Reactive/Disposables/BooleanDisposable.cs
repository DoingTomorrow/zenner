// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.BooleanDisposable
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

#nullable disable
namespace System.Reactive.Disposables
{
  public sealed class BooleanDisposable : ICancelable, IDisposable
  {
    internal static readonly BooleanDisposable True = new BooleanDisposable(true);
    private volatile bool _isDisposed;

    public BooleanDisposable()
    {
    }

    private BooleanDisposable(bool isDisposed) => this._isDisposed = isDisposed;

    public bool IsDisposed => this._isDisposed;

    public void Dispose() => this._isDisposed = true;
  }
}
