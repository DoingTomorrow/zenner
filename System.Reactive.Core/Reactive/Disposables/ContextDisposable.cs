// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.ContextDisposable
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Reactive.Concurrency;
using System.Threading;

#nullable disable
namespace System.Reactive.Disposables
{
  public sealed class ContextDisposable : ICancelable, IDisposable
  {
    private readonly SynchronizationContext _context;
    private volatile IDisposable _disposable;

    public ContextDisposable(SynchronizationContext context, IDisposable disposable)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      if (disposable == null)
        throw new ArgumentNullException(nameof (disposable));
      this._context = context;
      this._disposable = disposable;
    }

    public SynchronizationContext Context => this._context;

    public bool IsDisposed => this._disposable == BooleanDisposable.True;

    public void Dispose()
    {
      IDisposable state = Interlocked.Exchange<IDisposable>(ref this._disposable, (IDisposable) BooleanDisposable.True);
      if (state == BooleanDisposable.True)
        return;
      this._context.PostWithStartComplete<IDisposable>((Action<IDisposable>) (d => d.Dispose()), state);
    }
  }
}
