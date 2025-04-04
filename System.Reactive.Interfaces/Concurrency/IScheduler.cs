// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.IScheduler
// Assembly: System.Reactive.Interfaces, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: F75E00F4-A403-435F-9B50-2B7670A5231F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Interfaces.dll

#nullable disable
namespace System.Reactive.Concurrency
{
  public interface IScheduler
  {
    DateTimeOffset Now { get; }

    IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action);

    IDisposable Schedule<TState>(
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, IDisposable> action);

    IDisposable Schedule<TState>(
      TState state,
      DateTimeOffset dueTime,
      Func<IScheduler, TState, IDisposable> action);
  }
}
