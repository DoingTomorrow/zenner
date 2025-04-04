// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.CancellationDisposable
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Threading;

#nullable disable
namespace System.Reactive.Disposables
{
  public sealed class CancellationDisposable : ICancelable, IDisposable
  {
    private readonly CancellationTokenSource _cts;

    public CancellationDisposable(CancellationTokenSource cts)
    {
      this._cts = cts != null ? cts : throw new ArgumentNullException(nameof (cts));
    }

    public CancellationDisposable()
      : this(new CancellationTokenSource())
    {
    }

    public CancellationToken Token => this._cts.Token;

    public void Dispose() => this._cts.Cancel();

    public bool IsDisposed => this._cts.IsCancellationRequested;
  }
}
