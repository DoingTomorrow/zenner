// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.StableCompositeDisposable
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace System.Reactive.Disposables
{
  public abstract class StableCompositeDisposable : ICancelable, IDisposable
  {
    public static ICancelable Create(IDisposable disposable1, IDisposable disposable2)
    {
      if (disposable1 == null)
        throw new ArgumentNullException(nameof (disposable1));
      return disposable2 != null ? (ICancelable) new StableCompositeDisposable.Binary(disposable1, disposable2) : throw new ArgumentNullException(nameof (disposable2));
    }

    public static ICancelable Create(params IDisposable[] disposables)
    {
      return disposables != null ? (ICancelable) new StableCompositeDisposable.NAry(disposables) : throw new ArgumentNullException(nameof (disposables));
    }

    public static ICancelable Create(IEnumerable<IDisposable> disposables)
    {
      return disposables != null ? (ICancelable) new StableCompositeDisposable.NAry(disposables) : throw new ArgumentNullException(nameof (disposables));
    }

    public abstract void Dispose();

    public abstract bool IsDisposed { get; }

    private class Binary : StableCompositeDisposable
    {
      private volatile IDisposable _disposable1;
      private volatile IDisposable _disposable2;

      public Binary(IDisposable disposable1, IDisposable disposable2)
      {
        this._disposable1 = disposable1;
        this._disposable2 = disposable2;
      }

      public override bool IsDisposed => this._disposable1 == null;

      public override void Dispose()
      {
        Interlocked.Exchange<IDisposable>(ref this._disposable1, (IDisposable) null)?.Dispose();
        Interlocked.Exchange<IDisposable>(ref this._disposable2, (IDisposable) null)?.Dispose();
      }
    }

    private class NAry : StableCompositeDisposable
    {
      private volatile List<IDisposable> _disposables;

      public NAry(IDisposable[] disposables)
        : this((IEnumerable<IDisposable>) disposables)
      {
      }

      public NAry(IEnumerable<IDisposable> disposables)
      {
        this._disposables = new List<IDisposable>(disposables);
        if (this._disposables.Contains((IDisposable) null))
          throw new ArgumentException(Strings_Core.DISPOSABLES_CANT_CONTAIN_NULL, nameof (disposables));
      }

      public override bool IsDisposed => this._disposables == null;

      public override void Dispose()
      {
        List<IDisposable> disposableList = Interlocked.Exchange<List<IDisposable>>(ref this._disposables, (List<IDisposable>) null);
        if (disposableList == null)
          return;
        foreach (IDisposable disposable in disposableList)
          disposable.Dispose();
      }
    }
  }
}
