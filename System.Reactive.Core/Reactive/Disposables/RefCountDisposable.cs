// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.RefCountDisposable
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Threading;

#nullable disable
namespace System.Reactive.Disposables
{
  public sealed class RefCountDisposable : ICancelable, IDisposable
  {
    private readonly object _gate = new object();
    private IDisposable _disposable;
    private bool _isPrimaryDisposed;
    private int _count;

    public RefCountDisposable(IDisposable disposable)
    {
      this._disposable = disposable != null ? disposable : throw new ArgumentNullException(nameof (disposable));
      this._isPrimaryDisposed = false;
      this._count = 0;
    }

    public bool IsDisposed => this._disposable == null;

    public IDisposable GetDisposable()
    {
      lock (this._gate)
      {
        if (this._disposable == null)
          return Disposable.Empty;
        ++this._count;
        return (IDisposable) new RefCountDisposable.InnerDisposable(this);
      }
    }

    public void Dispose()
    {
      IDisposable disposable = (IDisposable) null;
      lock (this._gate)
      {
        if (this._disposable != null)
        {
          if (!this._isPrimaryDisposed)
          {
            this._isPrimaryDisposed = true;
            if (this._count == 0)
            {
              disposable = this._disposable;
              this._disposable = (IDisposable) null;
            }
          }
        }
      }
      disposable?.Dispose();
    }

    private void Release()
    {
      IDisposable disposable = (IDisposable) null;
      lock (this._gate)
      {
        if (this._disposable != null)
        {
          --this._count;
          if (this._isPrimaryDisposed)
          {
            if (this._count == 0)
            {
              disposable = this._disposable;
              this._disposable = (IDisposable) null;
            }
          }
        }
      }
      disposable?.Dispose();
    }

    private sealed class InnerDisposable : IDisposable
    {
      private RefCountDisposable _parent;

      public InnerDisposable(RefCountDisposable parent) => this._parent = parent;

      public void Dispose()
      {
        Interlocked.Exchange<RefCountDisposable>(ref this._parent, (RefCountDisposable) null)?.Release();
      }
    }
  }
}
