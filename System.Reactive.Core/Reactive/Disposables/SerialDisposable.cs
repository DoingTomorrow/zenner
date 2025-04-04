// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.SerialDisposable
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

#nullable disable
namespace System.Reactive.Disposables
{
  public sealed class SerialDisposable : ICancelable, IDisposable
  {
    private readonly object _gate = new object();
    private IDisposable _current;
    private bool _disposed;

    public bool IsDisposed
    {
      get
      {
        lock (this._gate)
          return this._disposed;
      }
    }

    public IDisposable Disposable
    {
      get => this._current;
      set
      {
        bool flag = false;
        IDisposable disposable = (IDisposable) null;
        lock (this._gate)
        {
          flag = this._disposed;
          if (!flag)
          {
            disposable = this._current;
            this._current = value;
          }
        }
        disposable?.Dispose();
        if (!flag || value == null)
          return;
        value.Dispose();
      }
    }

    public void Dispose()
    {
      IDisposable disposable = (IDisposable) null;
      lock (this._gate)
      {
        if (!this._disposed)
        {
          this._disposed = true;
          disposable = this._current;
          this._current = (IDisposable) null;
        }
      }
      disposable?.Dispose();
    }
  }
}
