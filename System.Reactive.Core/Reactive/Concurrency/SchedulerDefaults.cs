// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.SchedulerDefaults
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

#nullable disable
namespace System.Reactive.Concurrency
{
  internal static class SchedulerDefaults
  {
    internal static IScheduler ConstantTimeOperations => (IScheduler) ImmediateScheduler.Instance;

    internal static IScheduler TailRecursion => (IScheduler) ImmediateScheduler.Instance;

    internal static IScheduler Iteration => (IScheduler) CurrentThreadScheduler.Instance;

    internal static IScheduler TimeBasedOperations => (IScheduler) DefaultScheduler.Instance;

    internal static IScheduler AsyncConversions => (IScheduler) DefaultScheduler.Instance;
  }
}
