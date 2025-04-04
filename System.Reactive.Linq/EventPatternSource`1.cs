// Decompiled with JetBrains decompiler
// Type: System.Reactive.EventPatternSource`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive
{
  internal class EventPatternSource<TEventArgs>(
    IObservable<EventPattern<object, TEventArgs>> source,
    Action<Action<object, TEventArgs>, EventPattern<object, TEventArgs>> invokeHandler) : 
    EventPatternSourceBase<object, TEventArgs>(source, invokeHandler),
    IEventPatternSource<TEventArgs>
  {
    event EventHandler<TEventArgs> IEventPatternSource<TEventArgs>.OnNext
    {
      add => this.Add((Delegate) value, (Action<object, TEventArgs>) ((o, e) => value(o, e)));
      remove => this.Remove((Delegate) value);
    }
  }
}
