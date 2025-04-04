// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.VirtualTimeSchedulerExtensions
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Concurrency
{
  public static class VirtualTimeSchedulerExtensions
  {
    public static IDisposable ScheduleRelative<TAbsolute, TRelative>(
      this VirtualTimeSchedulerBase<TAbsolute, TRelative> scheduler,
      TRelative dueTime,
      Action action)
      where TAbsolute : IComparable<TAbsolute>
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return scheduler.ScheduleRelative<Action>(action, dueTime, new Func<IScheduler, Action, IDisposable>(VirtualTimeSchedulerExtensions.Invoke));
    }

    public static IDisposable ScheduleAbsolute<TAbsolute, TRelative>(
      this VirtualTimeSchedulerBase<TAbsolute, TRelative> scheduler,
      TAbsolute dueTime,
      Action action)
      where TAbsolute : IComparable<TAbsolute>
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return scheduler.ScheduleAbsolute<Action>(action, dueTime, new Func<IScheduler, Action, IDisposable>(VirtualTimeSchedulerExtensions.Invoke));
    }

    private static IDisposable Invoke(IScheduler scheduler, Action action)
    {
      action();
      return Disposable.Empty;
    }
  }
}
