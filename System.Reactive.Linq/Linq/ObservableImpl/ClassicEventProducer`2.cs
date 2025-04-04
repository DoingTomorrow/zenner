// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.ClassicEventProducer`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal abstract class ClassicEventProducer<TDelegate, TArgs> : EventProducer<TDelegate, TArgs>
  {
    private readonly Action<TDelegate> _addHandler;
    private readonly Action<TDelegate> _removeHandler;

    public ClassicEventProducer(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
      : base(scheduler)
    {
      this._addHandler = addHandler;
      this._removeHandler = removeHandler;
    }

    protected override IDisposable AddHandler(TDelegate handler)
    {
      this._addHandler(handler);
      return Disposable.Create((Action) (() => this._removeHandler(handler)));
    }
  }
}
