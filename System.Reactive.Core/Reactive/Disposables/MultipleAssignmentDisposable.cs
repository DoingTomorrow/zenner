// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.MultipleAssignmentDisposable
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

#nullable disable
namespace System.Reactive.Disposables
{
  public sealed class MultipleAssignmentDisposable : ICancelable, IDisposable
  {
    private readonly object _gate = new object();
    private IDisposable _current;

    public bool IsDisposed
    {
      get
      {
        lock (this._gate)
          return this._current == BooleanDisposable.True;
      }
    }

    public IDisposable Disposable
    {
      get
      {
        lock (this._gate)
          return this._current == BooleanDisposable.True ? (IDisposable) DefaultDisposable.Instance : this._current;
      }
      set
      {
        bool flag = false;
        lock (this._gate)
        {
          flag = this._current == BooleanDisposable.True;
          if (!flag)
            this._current = value;
        }
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
        if (this._current != BooleanDisposable.True)
        {
          disposable = this._current;
          this._current = (IDisposable) BooleanDisposable.True;
        }
      }
      disposable?.Dispose();
    }
  }
}
