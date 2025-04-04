// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.ISchedulerLongRunning
// Assembly: System.Reactive.Interfaces, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: F75E00F4-A403-435F-9B50-2B7670A5231F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Interfaces.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Concurrency
{
  public interface ISchedulerLongRunning
  {
    IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action);
  }
}
