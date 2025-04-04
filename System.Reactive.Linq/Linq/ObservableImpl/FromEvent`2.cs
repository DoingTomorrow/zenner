// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.FromEvent`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class FromEvent<TDelegate, TEventArgs> : ClassicEventProducer<TDelegate, TEventArgs>
  {
    private readonly Func<Action<TEventArgs>, TDelegate> _conversion;

    public FromEvent(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
      : base(addHandler, removeHandler, scheduler)
    {
    }

    public FromEvent(
      Func<Action<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
      : base(addHandler, removeHandler, scheduler)
    {
      this._conversion = conversion;
    }

    protected override TDelegate GetHandler(Action<TEventArgs> onNext)
    {
      TDelegate @delegate = default (TDelegate);
      return this._conversion != null ? this._conversion(onNext) : ReflectionUtils.CreateDelegate<TDelegate>((object) onNext, typeof (Action<TEventArgs>).GetMethod("Invoke"));
    }
  }
}
