// Decompiled with JetBrains decompiler
// Type: System.Reactive.ObserverBase`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Threading;

#nullable disable
namespace System.Reactive
{
  public abstract class ObserverBase<T> : IObserver<T>, IDisposable
  {
    private int isStopped;

    protected ObserverBase() => this.isStopped = 0;

    public void OnNext(T value)
    {
      if (this.isStopped != 0)
        return;
      this.OnNextCore(value);
    }

    protected abstract void OnNextCore(T value);

    public void OnError(Exception error)
    {
      if (error == null)
        throw new ArgumentNullException(nameof (error));
      if (Interlocked.Exchange(ref this.isStopped, 1) != 0)
        return;
      this.OnErrorCore(error);
    }

    protected abstract void OnErrorCore(Exception error);

    public void OnCompleted()
    {
      if (Interlocked.Exchange(ref this.isStopped, 1) != 0)
        return;
      this.OnCompletedCore();
    }

    protected abstract void OnCompletedCore();

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.isStopped = 1;
    }

    internal bool Fail(Exception error)
    {
      if (Interlocked.Exchange(ref this.isStopped, 1) != 0)
        return false;
      this.OnErrorCore(error);
      return true;
    }
  }
}
