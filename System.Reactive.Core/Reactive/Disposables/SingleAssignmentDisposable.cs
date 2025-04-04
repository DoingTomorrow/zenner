// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.SingleAssignmentDisposable
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Threading;

#nullable disable
namespace System.Reactive.Disposables
{
  public sealed class SingleAssignmentDisposable : ICancelable, IDisposable
  {
    private volatile IDisposable _current;

    public bool IsDisposed => this._current == BooleanDisposable.True;

    public IDisposable Disposable
    {
      get
      {
        IDisposable current = this._current;
        return current == BooleanDisposable.True ? (IDisposable) DefaultDisposable.Instance : current;
      }
      set
      {
        IDisposable disposable = Interlocked.CompareExchange<IDisposable>(ref this._current, value, (IDisposable) null);
        if (disposable == null)
          return;
        if (disposable != BooleanDisposable.True)
          throw new InvalidOperationException(Strings_Core.DISPOSABLE_ALREADY_ASSIGNED);
        value?.Dispose();
      }
    }

    public void Dispose()
    {
      Interlocked.Exchange<IDisposable>(ref this._current, (IDisposable) BooleanDisposable.True)?.Dispose();
    }
  }
}
