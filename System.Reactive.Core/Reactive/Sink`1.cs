// Decompiled with JetBrains decompiler
// Type: System.Reactive.Sink`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Threading;

#nullable disable
namespace System.Reactive
{
  internal abstract class Sink<TSource> : IDisposable
  {
    protected internal volatile IObserver<TSource> _observer;
    private IDisposable _cancel;

    public Sink(IObserver<TSource> observer, IDisposable cancel)
    {
      this._observer = observer;
      this._cancel = cancel;
    }

    public virtual void Dispose()
    {
      this._observer = NopObserver<TSource>.Instance;
      Interlocked.Exchange<IDisposable>(ref this._cancel, (IDisposable) null)?.Dispose();
    }

    public IObserver<TSource> GetForwarder() => (IObserver<TSource>) new Sink<TSource>._(this);

    private class _ : IObserver<TSource>
    {
      private readonly Sink<TSource> _forward;

      public _(Sink<TSource> forward) => this._forward = forward;

      public void OnNext(TSource value) => this._forward._observer.OnNext(value);

      public void OnError(Exception error)
      {
        this._forward._observer.OnError(error);
        this._forward.Dispose();
      }

      public void OnCompleted()
      {
        this._forward._observer.OnCompleted();
        this._forward.Dispose();
      }
    }
  }
}
